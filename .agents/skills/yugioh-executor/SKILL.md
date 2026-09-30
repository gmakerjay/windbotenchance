---
name: yugioh-executor
description: |
  คู่มือพัฒนา สร้างคอมโบ และ Deploy YugiohTH Rule-Based WindBot Executor (C#, ModernExecutor + Decoupled Deck Plugin).
  ใช้ทุกครั้งที่ผู้ใช้พูดถึง WindBot, Executor, เด็ค Yu-Gi-Oh!/YGO/EDOPro, .ydk, cards.cdb, ModernExecutor, DeckPlugin,
  bots.json, BUILD_AND_DEPLOY, hint ID, คอมโบ/รูท, Handtrap/Negate logic, "สร้างเด็คใหม่", "แก้ไขเด็ค", "ปรับปรุง Executor",
  หรือ "Text Duel" แม้ผู้ใช้ไม่ได้พูดชื่อ skill ตรงๆ ครอบคลุม: ขั้นตอนสร้างเด็ค, สถาปัตยกรรม Plugin,
  Strategic Decision System, Anti-patterns, ตาราง Hint OCGCore
---

# YugiohTH Executor Skill

## 0. วิธีอ่านกฎในเอกสารนี้

กฎแบ่งเป็น 2 ระดับ เพื่อให้เข้มเฉพาะจุดที่พังจริง และยืดหยุ่นในจุดที่ขึ้นกับบริบทเด็ค:

- 🔒 **HARD** — ฝ่าฝืนแล้วพัง/ทำลายสนามตัวเอง/ผิดนโยบายโปรเจกต์ ไม่มีข้อยกเว้น
- 🎯 **DEFAULT** — ค่าเริ่มต้นที่ดีในกรณีทั่วไป ปรับได้เมื่อมีเหตุผลเชิงเกมที่ระบุได้ (ให้บันทึกเหตุผลเป็นคอมเมนต์ในโค้ด)

## 1. Verify-first 🔒

อย่าเดาจากความจำ ให้เปิดของจริงใน repo ปัจจุบันก่อนแก้โค้ดหรือออกแบบคอมโบ:

| ตรวจอะไร | ดูจาก |
|---|---|
| Card ID / สเตตัส / Effect text | `cards.cdb` |
| Timing, Cost vs Effect | `script/cXXXX.lua` |
| รายการการ์ดและสัดส่วนจริง | `.ydk` ของเด็คนั้น |
| Method/Callback signature | โค้ดจริงใน `windbot-fork/` (ห้ามจำจากเอกสารเก่า) |
| Runtime / Target Framework | `*.csproj` / `*.sln` |
| Floodgate, Negator, Handtrap, Immunity | `CardIntelligence.cs` (lookup O(1)) |
| Hint ID | `constant.lua` + `ModernExecutor.cs` |
| Banlist | `0TCG.lflist.conf` / `OCG.lflist.conf` (กัน `ERRMSG_DECKERROR`) |

## 2. Workflow: สร้าง/แก้เด็ค/ปรับ Executor

1. **Audit** — อ่าน `.ydk` + `cards.cdb` + lua ของการ์ดหลักทั้งหมด
2. **Strategic Design** — กำหนด Main Route, Backup (B/C/D), First-turn End Board, Turn 2 Board Break/OTK, Resource/Negate Budget, Stop Conditions
3. **Implement** — สืบทอด `ModernExecutor` + ติดตั้ง Plugin (หัวข้อ 3) โดยไม่ต้องรอให้ผู้ใช้สั่งซ้ำ 🔒
4. **Build & Deploy** — ผ่าน `BUILD_AND_DEPLOY.ps1` ไปที่ `C:\Users\admin\Documents\EdoGame\` เท่านั้น 🔒 แล้วบันทึกใน `PROGRESS.md`
5. **Headless Simulation** 🔒 — ห้ามรันอัตโนมัติ รันเมื่อผู้ใช้สั่ง "Text Duel" / "จำลองดวล" เท่านั้น
   ```powershell
   dotnet run --project src\YGO_SOURCE_CLEAN\Client_Headless_Fortest\Client_Headless_Fortest.csproj -c Release -- --deck <DECK> --opponent <ABC|Altergeist|BlueEyes|DarkMagician> --games 10 --timeout 60
   ```
   สรุป Win Rate %, Violations (เป้าหมาย 0), Playbook Strategy

### 2.1 ลงทะเบียนเด็คใหม่

1. **`.ydk`** → `windbot-fork/Decks/<DeckName>.ydk` ชื่อสะอาด (`Tenpai`, `Centurion`) ไม่ใส่ปี/เวอร์ชันนำหน้า; เด็คอนิเมะใช้ `Anime_<Character>.ydk` 🎯 (ไฟล์เก่าอย่าง `_2026_*` คงไว้ ไม่ต้องรื้อ)
2. **Executor** → `windbot-fork/Game/AI/Decks/<DeckName>Executor.cs` (template ด้านล่าง)
3. **`bots.json`** → `{ "name": "X", "deck": "X", "dialog": "default", "flags": ["OCG","TCG"] }` และเช็คว่า `DecksManager.cs` แมปชื่อถูก
4. **Build** → `cd C:\Users\admin\Documents\EdoGame\src\YGO_SOURCE_CLEAN; powershell -ExecutionPolicy Bypass -File .\BUILD_AND_DEPLOY.ps1`

> path ด้านบนเป็นของเครื่องนี้ ถ้าเปลี่ยนเครื่องให้แก้ที่จุดเดียวคือหัวข้อ 1/2.1/10

### 2.2 Template

```csharp
using System.Collections.Generic;
using System.Linq;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Plugin;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("SampleDeck", "SampleDeck")]
    public class SampleDeckExecutor : ModernExecutor
    {
        public enum CardId { /* ตรวจจาก cards.cdb เสมอ */ }

        public SampleDeckExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            DeckPlugin = new SampleDeckPlugin(this);   // 🔒 ทุกเด็ค
            // AddExecutor ตาม Tier (หัวข้อ 9): Negates > Breakers > Starters > Searchers > Climbs > S/T Set
            // Hint Table สำหรับเลือกการ์ดอัตโนมัติ
        }

        public override bool OnSelectHand() => true; // true = ไปก่อน
    }

    internal class SampleDeckPlugin : DeckPluginBase
    {
        public override string DeckName => "SampleDeck";
        public override IDeckStrategy Strategy { get; }
        public override IDeckMaterialEvaluator MaterialEvaluator { get; }
        public override IDeckThreatEvaluator ThreatEvaluator { get; }

        public SampleDeckPlugin(SampleDeckExecutor e)
        {
            Strategy = new SampleDeckStrategy(e);
            MaterialEvaluator = new SampleDeckMaterialEvaluator(e);
            ThreatEvaluator = new SampleDeckThreatEvaluator(e);
        }
    }

    internal class SampleDeckStrategy : IDeckStrategy
    {
        private readonly SampleDeckExecutor _exec;
        public SampleDeckStrategy(SampleDeckExecutor e) => _exec = e;
        public void Reset() { }
        public ClientCard PickSpecialSummonTarget(IList<ClientCard> c) => c.FirstOrDefault();          // Extender/Boss ตามแผน
        public ClientCard PickSearchTarget(IList<ClientCard> c, ClientCard ctx) => c.FirstOrDefault(); // Starter > Extender > Disruption
    }

    internal class SampleDeckMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly SampleDeckExecutor _exec;
        public SampleDeckMaterialEvaluator(SampleDeckExecutor e) => _exec = e;
        public int GetMaterialCost(ClientCard c) => c == null ? 0 : 10; // Boss/Floodgate/Key = cost สูง
        public IList<ClientCard> SortMaterials(IList<ClientCard> c, int min = 1) => c.OrderBy(GetMaterialCost).ToList();
        public ClientCard PickDiscardTarget(IList<ClientCard> c, int min = 1) => c.OrderBy(GetMaterialCost).FirstOrDefault(); // ทิ้งตัวมีผลใน GY/ตัวซ้ำ
        public ClientCard PickDestructionSubstitute(IList<ClientCard> c, int min = 1) => c.FirstOrDefault();
    }

    internal class SampleDeckThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly SampleDeckExecutor _exec;
        public SampleDeckThreatEvaluator(SampleDeckExecutor e) => _exec = e;
        public int EvaluateThreatScore(ClientCard c) => 0;      // การ์ดที่ตัดเด็คนี้โดยเฉพาะ
        public bool IsEmergencyThreat(ClientCard c) => false;
    }
}
```

## 3. Decoupled Domain Plugin Architecture

**เหตุผล**: แยก Domain logic (การประเมิน material/discard/threat/search) ออกจาก Executor ที่เป็นแค่ router เพื่อให้แก้เด็คหนึ่งแล้วไม่กระทบอีกเด็ค และให้ Core ป้องกันข้อผิดพลาดให้ทุกเด็คเหมือนกัน

```text
CENTRAL CORE (CardIntelligence, BoardScorer, FallbackSelectCard, HeuristicGuard, ChainAdvisor, ResourcePlan)
        ↓ inherits
MODERN EXECUTOR (router: ลำดับ activation, delegate ผ่าน DeckPlugin)
        ↓ delegates
DOMAIN PLUGIN (IDeckPlugin / DeckPluginBase) → Strategy, MaterialEvaluator, ThreatEvaluator,
                                               ResourceEvaluator*, ScaleResolver*, ActionScorer*
```

🔒 ทุกเด็คต้องมี Plugin สืบทอด `DeckPluginBase` พร้อม **Strategy + MaterialEvaluator + ThreatEvaluator** อย่างน้อย (ถ้าเด็คไม่มีอะไรพิเศษ คืนค่าเริ่มต้นแบบ stub ได้ แต่โครงต้องอยู่) และห้ามอัด domain logic ลง Executor ล้วนๆ
🎯 `*` ตัวเสริม ติดตั้งเมื่อเด็คมี Counter economy (Resource), Pendulum (Scale), หรือต้องการ scoring เชิงคุณค่า (ActionScorer)

| Interface | เมธอด | จุดที่ ModernExecutor เรียกอัตโนมัติ |
|---|---|---|
| `IDeckPlugin`/`DeckPluginBase` | `ResetTurnState()` | `OnNewTurn()` |
| `IDeckStrategy` | `PickSpecialSummonTarget`, `PickSearchTarget` | เลือกเป้า Search (506) / SpSummon (509) |
| `IDeckMaterialEvaluator` | `GetMaterialCost`, `SortMaterials`, `PickDiscardTarget`, `PickDestructionSubstitute` | `OnSelect{Fusion,Synchro,Xyz,Link}Material` → Fodder ก่อน Ace ท้ายสุด; `OnSelectCard` (Discard) |
| `IDeckThreatEvaluator` | `EvaluateThreatScore`, `IsEmergencyThreat` | `CardIntelligence.GetThreatLevel()` บวกคะแนนเพิ่ม |
| `IDeckResourceEvaluator` | `GetAvailableResourceCount`, `CanSafelySpendResource`, `SelectCounters` | `OnSelectCounter` |
| `IDeckScaleResolver` | `PickLowScale`, `PickHighScale`, `PickScalePopTarget` | เด็ค Pendulum |
| `IDeckActionScorer` | `CalculateActionScore(name, boardImpact, netGain, cost)` | ประเมินก่อนร่าย |

ตัวอย่างจริงในโค้ด: `_2026_KwtuneExecutor.cs` (`KewlTunePlugin`), `_2026_GraveExecutor.cs` (`GravePlugin`)

## 4. Strategic Decision System

> *Do not play cards. Play the game state.* เลือก Action ที่ Expected Value สูงสุด ไม่ใช่คอมโบที่ยาวที่สุด

**Loop**: OBSERVE (Board/Hand/GY/LP) → IDENTIFY THREATS → CHECK WIN/DEFENSE → SCORE ACTIONS → EXECUTE → PRESERVE FOLLOW-UP

**Threat Priority**: (1) Lethal ที่ทำให้เราแพ้เทิร์นนี้ → (2) Hard Interaction (Omni/Monster/S&T Negate, Continuous Floodgate, Turn-skip lock) → (3) Resource Engine (Search/Draw/Revive/Token) → (4) Board Pressure ที่ไม่มี Interaction

### 4.1 Contextual Removal 🎯
ไม่มีลำดับตายตัวระหว่าง Banish/Spin/Bounce/Destroy เพราะ Destroy อาจเปิด Float/GY effect, Bounce ให้คู่แข่งใช้ Starter ซ้ำ, Banish บางเด็คได้ประโยชน์, Spin ไม่คุ้มกับ Extra Deck ที่เรียกกลับง่าย
```text
RemovalScore = ThreatValue + ZoneDenial + RecursionPrevention + ChainSafety + BoardImpact − OpponentRecoveryValue
```

### 4.2 Anti-Advantage Gate 🎯
ไม่ห้ามเด็ดขาดที่จะให้ทรัพยากรคู่แข่ง (จะเล่น Kaiju/Lava Golem/การ์ดแลกไม่ได้) แต่ต้องผ่าน 3 ด่าน:
**Necessary?** (จำเป็นต่อการแก้ Threat/ปลด Floodgate) → **Compensated?** (ได้คืนคุ้ม เช่น Board Breaker/OTK Confirm) → **Safe?** (คู่แข่งใช้สวน/เป็น Material ทันทีไม่ได้) → Allow / Reject

### 4.3 Risk Thresholds 🎯
- **Nibiru**: ซัมมอนครั้งที่ 4 คือ *Risk Threshold* ไม่ใช่ Stop Rule ที่ครั้งที่ 4 ประเมิน: มี Negate คุ้มกันแล้ว → เดินต่อ / ครั้งที่ 5 ออก Negate ได้พอดี → เดินต่อ / Lethal Confirmed หรือสนามศัตรูโล่ง → เข้า Battle Phase / มือเหลือแต่ Extender ที่ไม่นำไป Negate หรือ Recovery → หยุดที่ 4
- **Maxx "C"**: อย่า Pass ทันที คำนวณ Minimum Extension (Draws ที่ให้ vs Board Value vs จบเกมได้ไหม vs Interaction ขั้นต่ำ) ปกติ SS 1–2 ครั้งแล้วได้ Negate 1 ใบ ดีกว่าปล่อยสนามว่าง

### 4.4 Interruption Density 🎯 (แนวทาง ไม่ใช่โควตา)
- เด็ค 40 ใบ: Handtrap + Board Breaker ราว **9–15 ใบ** (22–37%) ปรับตามแนวเด็ค (Combo หนัก → ใกล้ขอบบน, Control/Stun → ยืดหยุ่นได้) Starter ราว 9–12 ใบ
- หลีกเลี่ยง Over-combofication (Starter/Extender ล้น ไม่มีที่ให้ Interaction)
- **ไปก่อน**: End Board ที่มี Interruption หลายมิติ (Monster/S&T Negate, Non-target Removal, Floodgate ชั่วคราว) / **ไปหลัง**: Handtrap ตัด Chokepoint + Board Breaker เคลียร์ก่อนเดิน Engine
- ให้ Interaction ในธีมเป็นส่วนหนึ่งของ End Board (Counter Trap, Quick Spell) ไม่ใช่มอนสเตอร์เปล่า

### 4.5 กฎทองคำ
1. 🎯 **Can Activate ≠ Should Activate** ต้องมีเป้าหมายและผลที่ระบุได้
2. 🎯 **Lethal First** Lethal Confirmed หรือศัตรูไม่มีตัวขัด → หยุดขยาย เข้า Battle Phase
3. 🎯 **Material Cost Awareness** อย่าใช้ Ace / Quick-Effect Interruption / Floodgate เป็น Material ถ้าไม่จำเป็น
4. 🎯 **1 Negate = 1 Problem** ไม่ซ้อน Negate ใส่ Action ที่ Neutralize แล้ว (เช็ค `IsChainAlreadyNeutralized()`)
5. 🎯 **Removal Discipline** ทำลายเฉพาะการ์ดที่ต้องคงอยู่จึงส่งผล (Continuous/Field/Equip/Pendulum Scale) — Normal Spell/Trap ที่ถูกทำลายระหว่างเชนยังรีโซลฟ์ผลอยู่ ("MST Negates Fallacy") ยกเว้นเมื่อมีเหตุผลเฉพาะ เช่น เพื่อ trigger ผลอื่น
6. 🔒 **Targeting Sanity** เอฟเฟกต์ลบสนามเล็งการ์ดศัตรู (`c.Controller == 1`) ก่อนเสมอ
7. 🎯 **Preserve Follow-up** เก็บมือ/ทรัพยากรสำหรับเทิร์น 3 บอร์ดมี Interruption + ตัวเดินเกมถัดไป ดีกว่าเทหมด
8. 🎯 **Anti-Hoarding vs Anti-Brick** ใช้เมื่อ Current Value > Expected Future Value; อย่าใช้การ์ดสารพัดประโยชน์กับงานเล็ก
9. 🎯 **Bait Before Starters** ส่ง Searcher/Bait รองล่อ Handtrap ก่อนเดิน Core
10. 🎯 **Information as Resource** จำการ์ดที่คู่แข่งเสิร์ช/ขึ้นมือ และ OPT ที่ใช้ไปแล้ว
11. 🎯 **Attack-Lock Adaptation** ภายใต้ Gravity Bind / Level Limit - Area B สลับเป็น Ignition removal และหาทางแก้ต่อ อย่าหยุดเล่นโดยไม่พิจารณา Out (`RESOURCE-STOP` ใช้เมื่อไม่มี Out จริงเท่านั้น)
12. 🎯 **Hand Limit Buffer** ถ้ามือจะเกิน 6 ที่ End Phase ให้เซ็ต Quick-Play/Trap ใน MP2 แทนทิ้งฟรี

### 4.6 DeckProbability Engine (`WindBot.Game.AI.DeckProbability`, O(1))
- `Hypergeometric`, `AtLeastOne(N,K,n)`, `AtLeast` — เช่น โอกาสเปิด Starter ในมือแรก (เป้า ≥ ~85% ตอนออกแบบเด็ค), โอกาสขุดเจอ Out
- `BanishLossRisk(remainingDeck, criticalCopies, banishCount)` — ประเมิน Pot of Desires (รีมูฟ 10) ถ้าเสี่ยงชิ้นสำคัญหลุดหมด ให้ Search ก่อน Draw
- `MultivariateHypergeometric` — โอกาสเปิดหลายกลุ่มพร้อมกัน (Starter + Extender ไม่มี Garnet)
- `BaitPlanner.EstimateHandTrapLikelihood(oppHand, oppDeck, estimatedHandtraps)` — ตัดสินใจส่ง Bait จากตัวเลข ไม่ใช่การเดา
- Discard Cost: ใช้ `AtLeastOne` เช็คโอกาสได้ตัวทดแทน; ถ้าเป็น one-of สุดท้ายให้กันไว้

### 4.7 Conjunction Fallacy & Attack-Negation Chaining 🔒
- **กติกา "A, and if you do, B"**: การ์ดอย่าง `Magic Cylinder` (62279055) มีข้อความว่า *"Target 1 attacking monster; negate the attack, and if you do, inflict damage to your opponent equal to its ATK."*
- **ห้าม Chain Attack-Negation ซ้อนกันเด็ดขาด**: หากสั่งร่าย `Magic Cylinder` หรือ `Dimension Wall` ซ้อนใส่กันใน Chain เดียวกัน:
  - CL2 ทำงาน: ยกเลิกการโจมตีสำเร็จ $\rightarrow$ ยิงดาเมจตาม ATK
  - CL1 ทำงาน: พยายามจะยกเลิกการโจมตี แต่การโจมตีถูกยกเลิกไปแล้วตั้งแต่ CL2! ส่วน A (negate the attack) จึงล้มเหลว $\rightarrow$ ส่วน B (inflict damage) ไม่เกิดขึ้นตามกติกา Conjunction! ส่งผลให้ **CL1 กลายเป็น 0 Damage และเสียการ์ดฟรีทันที!**
  - เช่นเดียวกับ `Dimension Wall` (67095270): หากการโจมตีถูกยกเลิกไปแล้ว จะไม่มี Battle Damage ให้สะท้อนอีกต่อไป
  - **การป้องกันในโค้ด**: ทุก Executor ที่มีกับดักสกัดโจมตี ต้องใส่ Guard:
    `if (Duel.CurrentChain.Any(c => c.IsCode(CardId.MagicCylinder, CardId.DimensionWall))) return false;`
- **การทำ Double Damage ที่ถูกต้องตามกติกา**: ไม่ใช่การกดใช้กระบอกเวทย์ 2 ใบซ้อนกัน แต่เกิดจาก Quick Effect ในสุสานของ `Magical Cylinders` (15943341): *"When an attack is declared... banish this card from your GY; double the effect damage inflicted by 'Magic Cylinder' to your opponent."* เมื่อ `Magic Cylinder` รีโซลฟ์ ผลสุสานจะคูณดาเมจให้เป็น 2 เท่าจริงๆ (เช่น 3000 ATK $\rightarrow$ 6000 Burn, 4000 ATK $\rightarrow$ 8000 OTK)

### 4.8 Backrow Fortress Defense Architecture (รับมือ Harpie / Mass Backrow Wipe) 🎯
เด็คสาย Backrow-heavy (กับดักหนาแน่น) มีจุดอ่อนร้ายแรงที่สุดคือการโดนล้างกระดานเวทย์/กับดักทั้งแผง เช่น `Harpie's Feather Duster` (18144506, 18144507), `Lightning Storm` (14532163), `Evenly Matched` (15693423), `Heavy Storm` (19613556), `Twin Twisters` (43898403), `Red Reboot` (23002292)
การรับมือต้องใช้โครงสร้างการป้องกัน 3 ระดับ (Three-Layer Fortress Defense):
1. **Layer 1: Pre-Emptive Aura Immunity (เกราะคุ้มกันถาวร)**:
   - `Lord of the Heavenly Prison` (9822220): ประกาศหงายบนมือใน Main Phase $\rightarrow$ การ์ดที่หมอบอยู่บนสนามทั้งหมด **ไม่ถูกทำลายด้วยผลของการ์ดใดๆ ทั้งสิ้น** (Harpie / Lightning Storm กลายเป็นหมันทันที)
2. **Layer 2: Spell Speed 3 Interception (สวนกลับด้วย Counter Trap ขั้นสูงสุด)**:
   - `Solemn Judgment` (41420027, จ่ายครึ่ง LP ขัดขวางทุกเวทย์/กับดัก) และ `Dark Bribe` (77538567)
   - ใน Executor ต้องเขียนเมธอด `IsMassBackrowWipe(card)` ดักตรวจ ID ล้างแผงหลังเพื่อสั่งให้ Counter Trap สวนกลับเป็นลำดับความสำคัญสูงสุด (Priority Tier 0) เสมอ
3. **Layer 3: Floating Punishment Traps (กับดักระเบิดแก้ทาง)**:
   - หากคู่แข่งล้างแผงหลังสำเร็จและไม่มี Counter Trap บนสนาม: ให้บรรจุ `Waking the Dragon` (10813327) หมอบรวมไว้ในแผงหลัง เมื่อการ์ดใบนี้ที่หมอบอยู่ถูกส่งออกจากสนามด้วยผลการ์ดของคู่แข่ง จะกระตุ้นผลเรียกมอนสเตอร์สุดยอดจาก Extra Deck หรือ Deck ออกมาทันที:
     - `Raidraptor - Ultimate Falcon` (86221741): ATK 3500, ไม่รับผลของการ์ดใดๆ ทั้งสิ้น (Unaffected Tower)
     - `The Last Warrior from Another Planet` (86099788): ล็อกไม่ให้ผู้เล่นทั้งสองฝ่ายอัญเชิญมอนสเตอร์ใดๆ ได้อีก (Complete Summon Lock)
     - `Baronne de Fleur` (84815190): Omni-Negate 1 ครั้ง + ทำลายการ์ด 1 ใบต่อเทิร์น

### 4.9 Dead Hand / Brick Mitigation Protocols 🎯
เมื่อเกิดกรณี "Dead Hand" (มือเปิดไม่มีคอมโบสะท้อนดาเมจ หรือมือติดกับดักที่ไม่เข้าคู่กัน หรือคู่แข่งระแวงไม่ยอมสั่งโจมตี):
1. **Consistency Engines (การขุดหาชิ้นส่วน)**:
   - `Pot of Duality` (98645731) + `Pot of Prosperity` (84211599): ขุด 3-6 ใบเพื่อเลือกการ์ดชิ้นที่ขาด
   - `Lilith, Lady of Lament` (23898021): สังเวยตัวเอง สุ่มหมอบ Normal Trap จากเด็ค 3 ใบ (หากเปิด `Magical Cylinders` 3 ใบ คู่แข่งจะถูกบังคับให้หมอบให้เรา 100%)
   - `Wannabee!` (3248469): ส่งจากมือลงสุสานใน End Phase ขุดเด็คบนสุด 5 ใบ หากมีกับดักสามารถหมอบลงสนามได้ทันที
   - `Trap Trick` (80101899): รีมูฟ 1 ใบเพื่อเซ็ตอีกใบจากเด็คแล้วเปิดใช้งานได้ในเทิร์นนั้นทันที
2. **Forced Aggression (การบีบให้คู่แข่งต้องเล่น)**:
   - เมื่อคู่แข่งรู้ว่าเป็นเด็คสะท้อนดาเมจและไม่ยอมโจมตี: ให้ร่าย `Battle Mania` (31245780) ใน Standby Phase ของคู่แข่ง $\rightarrow$ มอนสเตอร์คู่แข่งทุกตัวจะถูกบังคับเปลี่ยนเป็น Attack Position และ **ต้องประกาศโจมตีทุกตัวในเทิร์นนั้น** ทำให้กับดักสะท้อนดาเมจทำงานแน่นอน
   - แจกของขวัญมอนสเตอร์ ATK สูงด้วย `Kaiju` (เช่น Jizukiru 3300 ATK) ส่งไปสนามคู่แข่ง แล้วใช้ `Battle Mania` บังคับให้ไคจูตีเข้ามา หรือสะท้อนดาเมจ 3300-6600 ในทันที
3. **Alternative Win-Condition (แผนสำรองทางกายภาพ)**:
   - หากกับดักหมดหรือไม่สามารถชนะด้วยดาเมจสะท้อนได้: สามารถใช้ `Lord of the Heavenly Prison` (Level 10, 3000 ATK) 2 ตัว Xyz เป็น `Superdreadnought Rail Cannon Gustav Max` ยิง 2000 Burn $\rightarrow$ ทับเป็น `Superdreadnought Rail Cannon Juggernaut Liebe` (6000 ATK ตีมอนสเตอร์ได้หลายรอบ) เพื่อบุกปิดเกมด้วยพลังโจมตีกายภาพ
   - หรืออัญเชิญ `Super Starslayer TY-PHON - Sky Crisis` เพื่อตัดการทำงานของมอนสเตอร์คู่แข่งที่มี ATK 3000 ขึ้นไป

## 5. Anti-Patterns

### 5.1 🔒 HARD (ทำให้สนามพัง / ผิดนโยบาย)
1. **แยก Hint ID ใน `OnSelectCard`** — รายการ Search ใช้เฉพาะ `506 (ATOHAND)`; Hint ลบสนาม (`502` DESTROY, `503` REMOVE, `504` TOGRAVE, `505` RTOHAND, `507` TODECK) ต้องเลือกการ์ดศัตรู (`c.Controller == 1`) เท่านั้น
2. **Target Verification** — Ignition removal (BLS, Chaos Sorcerer, Exiled Force ฯลฯ) ต้องเช็คว่ามีเป้าหมายศัตรูจริง (`Enemy.GetMonsters().Any(...)`) ก่อน `return true` ไม่มี → `return false` (มิฉะนั้นเกมบังคับเล็งพวกเดียวกัน เช่น BLS แบนตัวเอง)
3. **ห้าม Tribute Summon ที่เป็นไปไม่ได้** เช่น `Bot.GetMonsterCount() == 0` สำหรับ Lv5+
4. **ห้ามเสิร์ชแล้วคืนการ์ดที่เพิ่งเสิร์ชเข้าเด็คทันที** (เช่น Illusion of Chaos) เลือกการ์ดที่ไม่จำเป็น
5. **ไม่ Chain Handtrap/Negate ซ้ำชนิดเดียวกันในเชนเดียว** (Ash ซ้อน Ash, Maxx "C" ซ้อน Maxx "C")
6. **`OnSelectEffectYn` ของศัตรู (`card.Controller == 1`) ตอบ `false` เป็นค่าเริ่มต้น**
7. **ห้ามเรียกโมเดล Neural/RL** — โปรเจกต์เป็น Rule-Based 100%
8. **Card ID / Banlist ต้องตรงกับ `cards.cdb` และ lflist**
9. **Deploy เฉพาะ `C:\Users\admin\Documents\EdoGame\`**; **ไม่รัน Headless Simulation เองก่อนผู้ใช้สั่ง**
10. **ต้องมี Decoupled Plugin ทุกเด็ค** (หัวข้อ 3)
11. **Attack-Negation Chaining 🔒**: ห้าม Chain การ์ดที่ Negate การโจมตี (`Magic Cylinder`, `Dimension Wall`) ซ้อนใส่กันใน Chain เดียวกันเด็ดขาด เพราะ Conjunction "and if you do" จะทำให้ Chain Link หลังล้มเหลว (0 Damage เสียการ์ดฟรี)
12. **Backrow Protection & Anti-Mass Wipe Awareness 🔒**: เด็คกับดักต้องมีมาตรการคุ้มกันแผงหลัง (Immunity / Counter Trap / Floating Mine เช่น Waking the Dragon) และต้องแมป Card ID ล้างแผงหลังให้ถูกต้องตรงตาม `cards.cdb` (18144506/18144507 Harpie, 14532163 Lightning Storm, 15693423 Evenly Matched, 43898403 Twin Twisters, 19613556 Heavy Storm, 23002292 Red Reboot)

### 5.2 🎯 DEFAULT (ปรับได้เมื่อมีเหตุผลเชิงเกม)
| ค่าเริ่มต้น | ยกเว้นได้เมื่อ / เหตุผล |
|---|---|
| Extra Deck Summon ต้องมีเงื่อนไข ไม่ `return true` เปล่าๆ; ไม่ใช้มอนที่สวม Comic Hand/Snatch Steal เป็นวัตถุดิบ; ไม่สังเวย ATK 2000+ ใน MP1 เพื่อทำตัวที่อ่อนกว่า; ไม่เอา Boss 2 ตัวไปทำ Xyz | Board สุทธิดีขึ้นชัดเจน หรือเป็นทางเดียวที่แก้ Threat/ปิดเกม |
| **S:P Little Knight (29301450)**: ไม่เรียกใน MP1 เมื่อศัตรูสนามโล่ง/เรามีโอกาสตีตรง (Effect 1 ล็อกไม่ให้มอนตีตรงทั้งเทิร์น) และไม่สังเวย Boss/ATK ≥ 2000; ตีก่อนแล้วเรียกใน MP2 | ต้องใช้ผลเคลียร์ Threat/ Negate ทันที หรือไม่มีทางทำดาเมจอยู่แล้ว |
| **Handtrap** (Ash, Dominus Impulse, Ghost Ogre) เก็บบนมือ ไม่หมอบ MP1 | การ์ดที่ต้องลงสนามจึงใช้ผลได้ หรือมือล้นต้องเซ็ตแทนทิ้ง |
| ไม่รีมูฟ/ทิ้ง Core Boss ใน Pot of Prosperity หรือ Cost | ไม่มีตัวเลือกอื่น และมีสำเนา/ผลกู้ |
| Fusion/Synchro/Xyz ลง Main Monster Zone เก็บ EMZ ให้ Link | MMZ เต็ม/ไม่มีที่เดินต่อ |
| **Number 41: Bagooska** ตั้ง **FaceUpDefence** เพื่อให้ฟลัดเกตทำงาน | ต้องโจมตี/ปิดเกมโดยที่ผลล็อกไม่จำเป็น |
| **Flat LP Cost** (Delinquent Duo 1000, Premature Burial 800, Solemn Warning 2000): เช็คว่า LP หลังจ่ายยังปลอดภัย โดยใช้ `LP − cost > max(2000, 500 × จำนวน action ที่คาดว่าจะต้องจ่ายภายใต้ Chain Energy)` เพื่อไม่ติด Hard-Lock (LP < 500) | ศัตรูไม่มี burn/floodgate และจ่ายแล้วปิดเกมได้ |
| เด็คถูกเสริม Interaction ตามสัดส่วนหัวข้อ 4.4 | สไตล์เด็คต่างออกไปและมีเหตุผลรองรับ |

## 6. ตาราง Hint OCGCore (ตรวจจาก audit ของโปรเจกต์)

| ID | Constant | พฤติกรรม AI |
|---|---|---|
| 500 | RELEASE | Token/Fodder ก่อน ไม่สังเวย Ace |
| 501 | DISCARD | ทิ้งตัวมีผลใน GY/ซ้ำ ปกป้อง Starter |
| 502 | DESTROY | เลือกศัตรู ตาม RemovalScore |
| 503 | REMOVE | เลือกศัตรู ข้ามตัวที่มี Target Immunity |
| 504 | TOGRAVE | ส่งชิ้นส่วนคอมโบเรา หรือส่งการ์ดศัตรู |
| 505 | RTOHAND | Bounce ตัวปัญหาศัตรู |
| 506 | ATOHAND | Search: Ace/Handtrap/Starter สำคัญก่อน |
| 507 | TODECK | Spin ตัวปัญหาหลักศัตรู |
| 508 | SUMMON | ลำดับ Starter/ตัวเปิด |
| 509 | SPSUMMON | Boss/Extender ตามรูท |
| 511/512/513/533 | F/S/X/LMATERIAL | ปกป้อง Ace/Boss เลือก Fodder/Token ก่อน |
| 514 | FACEUP | เลือกการ์ดหงายหน้า |
| 518 | EQUIP | การ์ดที่จะสวม |
| 520 | CONTROL | เปลี่ยนการควบคุม |
| 528 | POSCHANGE | เปลี่ยนรูปแบบ |
| 551 | TARGET | เล็งเป้าหมาย |
| 571 | TOZONE | ย้ายโซน |
| 572 | COUNTER | วาง/นำ Counter ออก |
| 575 | NEGATE | ขัดขวางผลศัตรู |

⚠️ **ไม่มี `HINTMSG_TOHAND = 573`** ค้นขึ้นมือใช้ 506 เท่านั้น; 572 = COUNTER, 575 = NEGATE อย่าสลับ ตารางนี้มาจาก audit ก่อนหน้า ถ้า `constant.lua` ปัจจุบันต่างออกไป ให้เชื่อไฟล์จริง

## 7. Central Core Safeguards (ทุก Executor ได้อัตโนมัติ)

1. **EMZ Preservation** — `0x20` สงวนให้ Link และ Pendulum หงายหน้าจาก Extra Deck; Fusion/Synchro/Xyz ลง MMZ
2. **Bagooska** — FaceUpDefence
3. **Duplicate Handtrap/Negate Prevention** — ไม่ซ้อนในเชนเดียว
4. **Lethal & Direct Attack Prioritization** — ATK ถึง LP และตีตรงได้ → ตีปิดเกม
5. **`HeuristicGuard.SanitizeSelection`** — ถ้าเป้าหมายจะทำลาย/ขจัดพวกเดียวกัน สลับไปการ์ดศัตรู (เป้า 0 Self-Harm Violations)
6. **Hostile Prompt Safeguard** — `OnSelectEffectYn` ของศัตรู = `false`

## 8. Case Studies

### 8.1 Endymion Spell Counter Control (Counter Economy)
```csharp
public enum CounterLevel { Critical = 0 /*0-1*/, Low = 1 /*2-3*/, Ready = 2 /*4-5*/, ComboReady = 3 /*6-7*/, Surplus = 4 /*8+*/ }
```
- **Jackal King** (2 Counter = Negate มอนสเตอร์ 1 ครั้ง): ตามระดับภัย Boss Effect > Search/Starter > Removal > Extender > Minor; ไม่ใช้กับขยะขณะ Boss/Starter ยังไม่ออก; เก็บกระสุนสำรอง 2 เม็ดขณะมี Jackal King
- **Endymion, Mighty Master**: Negate S/T + Bounce Recycling เรียงลำดับคืนมือ: (1) ใช้ซ้ำได้ในเทิร์นเรา (2) หมดหน้าที่ (3) Counter สะสมสูงเพื่อโอนเข้า Mighty Master (4) ห้ามคืน Key Card ที่ทำคอมโบพัง; **6-Counter Wipe** เมื่อเคลียร์ Threat ≥ 2 หรือเปิดทาง Lethal
- **Servant of Endymion** (3 Counter): SS ตัวเอง + Jackal King เมื่อเปิดคอมโบ/สร้างบอร์ดจริง ไม่เรียกเพื่อเพิ่มจำนวน
- **Net Gain**: `NetGain = NewCounters − CountersSpent` เน้น Action ที่ ≥ 0
- **Emergency Breach**: ใช้ต่ำกว่า Reserve ได้เมื่อศัตรูจะ Lethal / ตั้ง Floodgate ถาวร / Resolve Boss Effect รุนแรง

```text
ActionScore = BoardImpact + CounterNetGain + NegateValue + FutureUtility + FollowUpValue + OTKValue
            − CounterCost − MaterialLoss − OpponentAdvantage − RecoveryRisk
```

### 8.2 Six Samurai (Bushido Economy + Multi-Tier Interruption)
- **Loop**: `Gateway of the Six` (หัก 4 เคาน์เตอร์ เสิร์ช/กู้ Six Samurai) + `Battle Shogun` (Link-2 เสิร์ช Gateway ตอน Link Summon); รวม Counter จาก Gateway/Dojo/United เพื่อรู้จำนวนรอบเสิร์ชที่เหลือ
- **Interruption**: S/T Negate = `Shi En` (Quick, เทิร์นละครั้ง + สังเวยซามูไรอื่นแทนถูกทำลาย); ควบคุมบอร์ด = `Great Shogun Shien` (ศัตรูร่าย S/T ได้เทิร์นละใบ) + Baronne de Fleur / Apollousa / S:P Little Knight; Quick Trap = `Six Style - Dual Wield` (เด้ง 2 ใบเมื่อคุมซามูไรตัวเดียวแบบตั้งโจมตี); Handtrap สล็อต Ash / Maxx "C" / Called by the Grave / Crossout Designator กัน Nibiru/Droll ตัดลูป
- **Kizaru**: ตอน SS ตรวจ Attribute ของ Six Samurai บนสนาม แล้วเสิร์ชตัวที่ Attribute ต่างกัน

### 8.3 Magical Cylinder & Counter-Reflect Trap Fortress (Reflect Control + Trap Immunity)
- **True Double Damage Rule**: การเบิ้ลดาเมจ 2 เท่าทำได้โดยการใช้ Quick Effect ในสุสานของ `Magical Cylinders` (15943341) ร่ายคูณสองดาเมจให้ `Magic Cylinder` (62279055) ไม่ใช่การเชนกระบอกซ้อนกัน ห้ามเชน `Magic Cylinder` หรือ `Dimension Wall` ในเชนเดียวกันเด็ดขาด (ป้องกัน 0 Damage)
- **3-Layer Fortress Against Mass Wipe**:
  - *Layer 1*: `Lord of the Heavenly Prison` (9822220) หงายมือค้างไว้ ป้องกันการ์ดหมอบทุกใบจากการถูกทำลายด้วยผลการ์ด
  - *Layer 2*: `Solemn Judgment` (41420027) และ `Dark Bribe` (77538567) สวนกลับ Mass Wipe ทันที
  - *Layer 3*: `Waking the Dragon` (10813327) หมอบล่อเป้า เมื่อศัตรูล้างแผงหลัง จะลาก `Raidraptor - Ultimate Falcon` (3500 ATK อมตะทุกผล) หรือ `The Last Warrior from Another Planet` (Hard Summon Lock) ออกมาคุมกระดานทันที
- **Forced Aggression & OTK Feed**: ใช้ `Battle Mania` (31245780) ใน Standby Phase บังคับมอนสเตอร์ศัตรูตั้งโจมตีและต้องตีทุกตัว หรือส่ง `Kaiju` (Jizukiru 3300 ATK) ให้ศัตรูเพื่อเป็นเป้าสะท้อน 3300-6600 ดาเมจ
- **Beast Mode Recovery**: มือตันสามารถใช้ `Pot of Duality`, `Pot of Prosperity`, `Lilith` (เสิร์ชกับดักปกติ 3 ใบ), `Wannabee!` (ขุด 5 ใบ End Phase) หรือนำ `Lord of the Heavenly Prison` 2 ใบทำ Rank 10 `Gustav Max` (ยิง 2000) $\rightarrow$ `Juggernaut Liebe` (6000 ATK) ทุบปิดเกม

## 9. โปรโตคอล 3 ขั้น (Senior Game AI Engineer)

**ขั้น 1 — วิเคราะห์ Human vs Bot + ปรับ Decklist**
- คัดการ์ดที่ไม่เหมาะกับบอท: Chokepoint Handtrap ที่ต้องอ่านเกมลึก, การ์ดทำลายของตัวเองที่ไม่มี Guard, คอมโบแตกกิ่งข้าม Phase/พึ่ง Action ศัตรู (เช่น Dagda + Scythe + Halq + Wonder Magician), การ์ดจ่าย LP ก้อนโต/ทิ้งหมดมือ (Allure of Darkness ที่ไม่มี DARK สำรอง)
- แทนด้วยการ์ดที่เงื่อนไขชัดเจน: Counter Trap (Solemn), Non-target Removal, Omni-Negate Boss (Vortex Dragon, Baronne, Crystal Wing), Floodgate ที่ไม่ขัดคอมโบ (Abyss Dweller, Bagooska); เน้น Route A/B เส้นตรง; ปรับ Starter/Extender/Interaction ตามหัวข้อ 4.4
- สรุป Decklist: Card ID, ชื่อ, จำนวน (ตาราง + รูปแบบ `.ydk`)

**ขั้น 2 — โครงตรรกะการเล่น (ก่อนเขียนโค้ด)**
- ไปก่อน: ตั้งเสา Omni-Negate, Quick Disruption, สะสม Resource เทิร์น 3 / ไปหลัง: Board Breaking (Harpie, Raigeki, Dark Rebellion, Clear Wing) ก่อนเดิน Engine + Lethal Check
- Safety: Lethal Check (ห้าม Overextend), Field/Continuous Protection (ไม่ทับใบเดิมที่ยังมีประโยชน์), Chaining Guard, Cost & LP Guard
- Targeting: Continuous Floodgate → Negator → Monster ATK สูงสุด และ Target Verification

**ขั้น 3 — Implement**
1. `CardId` ครบจาก `cards.cdb` ใน `public static class CardId`
2. Constructor: `DeckPlugin = new XxxPlugin(this);` + `AddExecutor` ตาม Tier (0 Negates → 1 Board Breakers → 2 Starters → 3 Searchers → 4 Climbs → 5 S/T Set)
3. Callbacks: `OnSelectCard` แยก Hint (506→Strategy, 509→Strategy, 501→MaterialEvaluator, 502/503→เป้าศัตรู/Self-pop Resolver); `OnSelectOption`/`OnSelectEffectYn` ปฏิเสธของศัตรู รับผลที่เป็นประโยชน์ของเรา; `OnSelectPosition` ATK ≥ 1800 ตั้งโจมตี, ATK 0 / Handtrap / Bagooska ตั้งป้องกัน 🎯
4. Build → Deploy → อัปเดต `PROGRESS.md` → รอคำสั่ง "Text Duel"

## 10. Build, Deploy & Progress Log

- Deploy เฉพาะ `C:\Users\admin\Documents\EdoGame\` ผ่าน `BUILD_AND_DEPLOY.ps1` 🔒
- `PROGRESS.md` รักษา ~200–400 บรรทัด (เก็บ 5–10 รายการล่าสุด) เพื่ออ่านจบใน 1 tool call; เกิน ~500–800 บรรทัด ย้ายประวัติเก่าไปต่อท้าย `Docs/PROGRESS_ARCHIVE.md`