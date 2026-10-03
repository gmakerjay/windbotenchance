---
name: yugioh-executor
description: |
  Master Skill: พัฒนา/ออกแบบคอมโบ/ตรวจ Stack-OPT/Build-Deploy/Git ของ YugiohTH Rule-Based WindBot Executor
  (C#, ModernExecutor + Decoupled Deck Plugin + Core Modules: ComboRouter, BaitPlanner, ChainTimingAdvisor,
  ResourcePlanner, OpponentProfiler, HeuristicGuard, CardIntelligence). ใช้ทุกครั้งที่ผู้ใช้พูดถึง WindBot, Executor,
  Plugin, เด็ค YGO/EDOPro, .ydk, cards.cdb, bots.json, BUILD_AND_DEPLOY, hint ID, OnSelectCard, คอมโบ/รูท,
  Handtrap/Negate/Bait logic, "สร้างเด็คใหม่", "แก้ไขเด็ค", "ปรับปรุง Executor", "Text Duel", "pull", "push", "commit"
  แม้ไม่ได้เอ่ยชื่อ skill ตรงๆ
---

# YugiohTH Executor — Master Skill

> **Single Source of Truth** — skill นี้มี 2 สำเนาที่ต้องเหมือนกันเสมอ:
> Workspace (IDE โหลด) `EdoGame\.agents\skills\yugioh-executor\` ↔ Git `src\YGO_SOURCE_CLEAN\.agents\skills\yugioh-executor\`
> ซิงค์ด้วย `SYNC_AGENT_ASSETS.ps1` (หัวข้อ 11) · ข้อมูลทุกข้อในนี้ตรวจกับ source จริงเมื่อ 2026-10-03

## 0. วิธีใช้ skill นี้

- 🔒 **HARD** — ฝ่าฝืนแล้วสนามพัง / Self-Harm / ผิดนโยบาย → ไม่มีข้อยกเว้น
- 🎯 **DEFAULT** — ค่าเริ่มต้นที่ดี ปรับได้เมื่อมีเหตุผลเชิงเกม (เขียนเหตุผลเป็นคอมเมนต์ในโค้ด)

| ไฟล์อ้างอิง (`references/`) | อ่านเมื่อ |
|---|---|
| [`executor_template.md`](references/executor_template.md) | สร้างเด็คใหม่ / Refactor — template ที่ลงทะเบียนครบทุกโมดูล (compile ผ่านแล้ว) |
| [`core_api.md`](references/core_api.md) | ต้องการ signature / รู้ว่า core ทำอะไรให้อัตโนมัติ vs ต้องเรียกเอง |
| [`hint_reference.md`](references/hint_reference.md) | เขียน `OnSelectCard` / เจอ hint แปลก / custom `aux.Stringid` |
| [`known_core_issues.md`](references/known_core_issues.md) | **ก่อนเขียน OnSelectCard / ComboRouter / Ace** — สถานะบั๊ก core ที่แก้แล้ว + ข้อจำกัดที่ยังต้องรู้ |
| [`case_studies.md`](references/case_studies.md) | Nibiru/Maxx C, Backrow Fortress, Counter economy, Six Samurai, DeckProbability |

## 1. Project Map

```text
C:\Users\admin\Documents\EdoGame\                 ← Workspace + Deploy Target (เกม + DashBot) 🔒
├── .agents\skills\yugioh-executor\               ← skill (สำเนา IDE)
├── AGENTS.md / PROGRESS.md / Docs\               ← สำเนาที่ซิงค์จาก repo
├── script\constant.lua  script\cXXXX.lua         ← Hint ID / เอฟเฟกต์จริง
└── src\YGO_SOURCE_CLEAN\                         ← Git repo (origin: gmakerjay/windbotenchance)
    ├── .agents\skills\yugioh-executor\           ← skill (สำเนา Git)
    ├── cards.cdb  configs\ (deploy → WindBot\configs)  tools\scan_card_intelligence.py  tools\audit_card_ids.py
    ├── windbot-fork\
    │   ├── ExecutorBase\                         ← Central Core
    │   │   ├── HeuristicGuard.cs DecisionTracer.cs AntiFloodgateHelper.cs
    │   │   ├── Game\GameAI.cs Duel.cs ClientCard.cs ClientField.cs
    │   │   └── Game\AI\  ModernExecutor DefaultExecutor Executor CardIntelligence ComboRouter BaitPlanner
    │   │                 ChainTimingAdvisor ResourcePlanner OpponentProfiler DeckProbability BoardScorer
    │   │                 Plugin\ (DeckPluginBase, IDeck*)  DecisionEngine\  Enums\
    │   ├── Game\AI\Decks\<Deck>Executor.cs       ← Router
    │   ├── Game\AI\Plugins\<Deck>Plugin.cs       ← Domain logic (ns WindBot.Game.AI.Plugins)
    │   ├── Game\DecksManager.cs                  ← แมป [Deck(Name, File)]
    │   ├── Decks\<Deck>.ydk   bots.json
    ├── dashbot\MainWindow.xaml.cs                ← หมวดหมู่เด็ค (ModernArchetypes ฯลฯ)
    ├── Client_Headless_Fortest\  Docs\  PROGRESS.md  AGENTS.md
    ├── BUILD_AND_DEPLOY.ps1   SYNC_AGENT_ASSETS.ps1
```

## 2. Session Protocol (ทุก Agent ทำเหมือนกัน)

**เริ่มงาน**: `git pull` → `SYNC_AGENT_ASSETS.ps1` → อ่านหัว `PROGRESS.md` (เวอร์ชัน `0.0XX` ล่าสุด) →
อ่าน `Docs/<DECK>_AUDIT_AND_PLAYBOOK.md` ของเด็คที่จะแก้ → อ่าน `references/known_core_issues.md`

**Definition of Done**
1. `BUILD_AND_DEPLOY.ps1` **0 Errors**
2. `PROGRESS.md` รายการบนสุด `## 0.0XX. <หัวข้อ> (YYYY-MM-DD)`
3. งานเด็ค/Core ที่มีนัยสำคัญ → `Docs/<DECK>_AUDIT_AND_PLAYBOOK.md`
4. แก้ core แล้ว → อัปเดต `known_core_issues.md` / `core_api.md` ให้ตรง
5. `SYNC_AGENT_ASSETS.ps1` · Push เมื่อผู้ใช้สั่งเท่านั้น (หัวข้อ 11)

## 3. Core Rules

### 3.1 Verify First 🔒 — ห้ามเดา Card ID / Effect / Cost / Timing / Callback / Hint
| ตรวจอะไร | ดูจาก |
|---|---|
| Card ID / สเตตัส / Effect / alias | `cards.cdb` (`texts`, `datas.alias`) + **`python tools/audit_card_ids.py <ไฟล์/โฟลเดอร์>`** (ต้องไม่มี NOTFOUND/MISMATCH ใหม่) |
| Timing, Cost vs Effect, OPT, `aux.Stringid(id,n)` | `script/cXXXX.lua` |
| จำนวนจริง | `.ydk` |
| Signature / พฤติกรรม core | source ใน `windbot-fork/` + `references/core_api.md` |
| Hint | `script/constant.lua` + `references/hint_reference.md` |
| Banlist | `0TCG.lflist.conf` / `OCG.lflist.conf` (กัน `ERRMSG_DECKERROR`) |

### 3.2 Rule-Based Only 🔒 — ห้าม NeuralExecutor / RL / Training / เรียก AI Model ตัดสินใจ

### 3.3 Target Safety 🔒
- **Removal ต้องเลือกศัตรู** (`c.Controller == 1`) — 502/503/520/575 เสมอ; **504/505/507 ดูบริบท** (อาจเป็น cost ฝั่งเรา)
  → core route ให้แล้ว (504 Deck→GY = positive pick, 507 ฝั่งเรา = cost ค่าต่ำ) แต่ถ้าการ์ดมีเงื่อนไขพิเศษ
  ให้ระบุจากการ์ดต้นทางด้วย `Duel.GetCurrentChainCard()` / `GetCurrentSolvingChainCard()`
- Ignition removal ต้องเช็คว่ามีเป้าศัตรูจริงก่อน `return true` — ไม่มี → `return false`
- ห้าม Self-Harm โดยไม่มีเหตุผลเชิงเกม (`HeuristicGuard.SanitizeSelection` = ด่านสุดท้าย ไม่ใช่ด่านแรก)

### 3.4 Cost / Resource Safety 🎯
- อย่าใช้ Boss / Ace / Floodgate / Quick Negate เป็น Material/Cost ถ้าไม่จำเป็น; อย่าใช้ Resource เพียงเพราะ "ใช้ได้"
- Flat LP Cost: `LP − cost > max(2000, 500 × action ภายใต้ Chain Energy)`

### 3.5 No Duplicate Waste / STACK-AWARE 🔒
- ก่อน Activate/Set/Chain ตรวจว่า Effect ใช้ได้กี่ครั้งต่อ Turn/Chain/Phase
- ใบแรกใช้ Effect ไปแล้ว + OPT/จำกัดจนใบที่สองไม่ให้ผล → **ห้ามเปิดซ้ำ**; ห้าม Set S/T ซ้ำหน้าที่เดียวกันโดยไม่มีเหตุผล
- แยก **Per-Card OPT / Hard OPT (ชื่อ) / Once per Chain / Phase Limit** → เก็บเป็น `bool _xxxUsed` reset ใน `OnNewTurn`
- ประเมิน: `CanResolve? + CanActivate? + RemainingUses + NetValue`

### 3.6 No Redundant Negate 🔒
- Effect ถูก Neutralize แล้ว ห้าม Negate ซ้ำ; ห้าม Chain Handtrap ชื่อเดียวกันซ้ำในเชน
- `IsChainAlreadyNeutralized()` **ไม่มีใน core** → ใช้ helper ใน template (เช็ค controller, `IsDisabled()`, `Duel.NegatedChainIndexList`)
- Duplicate (การ์ดชื่อเดียวกันของเราอยู่ในเชนแล้ว) → core `IsDuplicateOwnChainActivation()` เช็คให้ใน `SmartHandTrapChain()` ทั้ง 2 overload

### 3.7 Lethal First 🎯 — Lethal Confirmed + ไม่มี Threat สำคัญ → หยุดขยาย เข้า Battle Phase

## 4. Workflow: สร้าง / แก้ไขเด็ค / ปรับ Executor

1. **Audit** — `.ydk` + `cards.cdb` + Lua การ์ดหลัก + Executor/Plugin เดิม + Playbook ใน `Docs/`
2. **Plan** — Starter / Extender / Searcher / Bait / Boss / Interaction / Board Breaker / Brick;
   Route A/B/C/D, Going First/Second, End Board, Stop Conditions, OPT Budget
3. **Implement** — เริ่มจาก `references/executor_template.md`; Executor = Router, Domain = Plugin 🔒; ลงทะเบียนโมดูลตามหัวข้อ 5
4. **Build & Deploy** — `BUILD_AND_DEPLOY.ps1` → `C:\Users\admin\Documents\EdoGame\` เท่านั้น 🔒
5. **Document** — `PROGRESS.md` + `Docs/`
6. **Text Duel** 🔒 — รันเมื่อผู้ใช้สั่ง "Text Duel" / "จำลองดวล" เท่านั้น
   ```powershell
   dotnet run --project src\YGO_SOURCE_CLEAN\Client_Headless_Fortest\Client_Headless_Fortest.csproj -c Release -- --deck <DECK> --opponent <ABC|Altergeist|BlueEyes|DarkMagician> --games 10 --timeout 60
   ```
   รายงาน W/L/Win% แยกคู่, Violations = 0, Crashes = 0, ดู log `[PHASE-GUARD]`, `[AUTO-GUARD]`, `[COMBO-STEP]`, `[BAIT]`, `[CHAIN-TIMING]`

### 4.1 ลงทะเบียนเด็คใหม่ (ครบ 7 จุด)
1. `windbot-fork/Decks/<Deck>.ydk` — ชื่อสะอาด 🔒 ห้ามปี/เวอร์ชันนำหน้า (`Anime_<Char>`, `GOAT_`, `Special_` ได้)
2. `Game/AI/Decks/<Deck>Executor.cs` — `[Deck("<Deck>", "<ydk>")]` (`DecksManager` แมปจาก attribute)
3. `Game/AI/Plugins/<Deck>Plugin.cs`
4. `bots.json` — `{ "name", "deck": "<Deck.Name>", "difficulty": 3, "dialog": "default", "flags": ["OCG","TCG"], "masterRules": [4,5] }`
5. `dashbot/MainWindow.xaml.cs` — เพิ่มใน `ModernArchetypes` (ไม่งั้นแสดงผิดหมวด)
6. `CardIntelligence.cs` — เพิ่ม floodgate/negator/handtrap/chokepoint ใหม่ (immunity → `tools/scan_card_intelligence.py`)
   → ทุก ID ใหม่ต้องผ่าน `python tools/audit_card_ids.py <ไฟล์>` (core เคยมี ID ปลอม ~140 จุด แก้ใน v0.086)
7. Build 0 Errors

## 5. Architecture & Module Registration Matrix

```text
CENTRAL CORE (ExecutorBase)  CardIntelligence · HeuristicGuard · BoardScorer · AiAnalysisSuite · DecisionEngine
      ↓ inherits
MODERN EXECUTOR  ComboRouter · BaitPlanner · ChainAdvisor · ResourcePlan · OpponentProfile · AIContext  (สร้างให้อัตโนมัติ)
      ↓ delegates via DeckPlugin
DECK PLUGIN      Strategy · MaterialEvaluator · ThreatEvaluator (+ ResourceEvaluator · ScaleResolver · ActionScorer)
```

| โมดูล | Core ทำให้อัตโนมัติ | เด็ค **ต้อง** ทำ |
|---|---|---|
| `DeckPlugin` 🔒 | route hint 500/501/504(cost)/507(เรา) → `PickDiscardTarget`; 506/505(เรา)/504(Deck→GY) → `PickSearchTarget`; 509 → `PickSpecialSummonTarget`; 502/503/504/507/520/575 ศัตรู → ThreatScore; materials 511/513/533 + counter | `DeckPlugin = new XPlugin(this)` + Strategy/Material/Threat |
| Ace registry 🔒 | — | `override IsAceCard` + `ResourcePlan.RegisterAceCards` + `HeuristicGuard.RegisterAceCards` (ชุดเดียวกัน) |
| `BaitPlanner` 🎯 | reset/feed ทุกเทิร์น | `RegisterComboStarters/BaitCards/NeverBait` **และ** เรียก `GetBaitIfNeeded(Card)` ใน func ของ starter; bait executor อยู่ก่อน starter |
| `ChainAdvisor` 🎯 | ใช้ใน `SmartHandTrapChain()` (+ duplicate guard ในตัว) | `RegisterHighValueTargets(ศัตรู)`; handtrap func ใช้ `SmartHandTrapChain()` + neutralized check |
| `ComboRouter` 🎯 | MP1 เลือก+รัน step | `RegisterLine` เฉพาะ step ปลอดภัย + `Condition`; หรือ `ComboRouter.Enabled = false` |
| `ResourcePlan` 🎯 | `ShouldStopExtending()` | ปรับ threshold / เรียก `EvaluateAceUsage` ก่อนใช้ Ace |
| `RegisterOptionalFieldRemovalCards` 🎯 | `OnSelectYesNo` ปฏิเสธเมื่อสนามศัตรูว่าง | register การ์ดเราที่มี optional removal |
| `OpponentProfile` | feed อัตโนมัติ | (ทางเลือก) `IsControlDeck()`/`ShouldRushSetup()` ปรับความ aggressive |
| `ScaleResolver` / `ActionScorer` / `IsEmergencyThreat` | ❌ ไม่มี core เรียก | เรียกเองใน func ถ้าติดตั้ง |
| `DecisionTracer` | — | `TraceActivate/TraceSkip` ใน func สำคัญ (debug Text Duel) |

## 6. Executor Engineering Laws (พฤติกรรมจริงของ engine)

1. 🔒 **ลำดับ `AddExecutor` = Priority** — GameAI วน executor ตามลำดับที่ add; `ModernExecutor` ctor add `GoToEndPhase(ShouldPassTurn)` ไว้ **ตัวแรก**
2. 🔒 **Global guard ก่อน func** — `ShouldAllowActivate/Summon/SpSummon/SpellSet/MonsterSet/Repos` บล็อกได้ (Rush/Attack-first/Stop-extending/S:P Little Knight/Non-QuickPlay set) — func ไม่ถูกเรียกเลย ให้ดู log `[PHASE-GUARD] BLOCKED`
3. 🔒 **Optional trigger ใช้ func เดียวกับ activate** — `OnSelectEffectYn` เรียก func ของ `AddExecutor(Activate, id)`; func ต้องแยกเอฟเฟกต์ด้วย `ActivateDescription == Util.GetStringId(id, n)`; ทุก func คืน false = ปฏิเสธ trigger
4. 🔒 **`AI.SelectCard` ถูกข้าม** สำหรับ hint ที่ ModernExecutor จัดการ (500–509/514/518/520/528/551/575) → ใช้ **Pending-Select** ใน deck `OnSelectCard` (template §6) · ใช้ได้กับ custom hint `Util.GetStringId(id,n)` · ยกเว้น 504 Deck→GY ที่ plugin คืน null → ตกไป `AI.SelectCard`/Fallback
5. 🔒 **`Card`/`ActivateDescription` อาจ stale ระหว่าง resolve** → ระบุต้นทางด้วย `Duel.GetCurrentChainCard()` (ตอนเลือก cost/target) / `Duel.GetCurrentSolvingChainCard()` (ตอน resolve)
6. 🔒 **ComboRouter step ข้าม func/guard/SetCard** และเลือก description แรกของการ์ด → ใส่เฉพาะ action ที่ปลอดภัยเสมอ
7. 🔒 **`IsAceCard` default ผิดสำหรับเด็คส่วนใหญ่** (Extra Deck ทุกตัว = Ace) → override เสมอ
8. 🔒 **Synchro (512) ไม่ผ่าน Plugin** → เด็ค Synchro override `OnSelectSynchroMaterial` เอง
9. 🔒 **รู้ข้อจำกัด core** ตาม `known_core_issues.md` — บั๊กหลัก (507/504/518/552 routing, Bagooska, MP2 guard, duplicate chain, Card ID ปลอม) แก้แล้วใน v0.086; ที่เหลือ: ComboRouter bypass, `IsAceCard` default, Synchro, `IsSpecialSummonBlocked` กว้างเกิน, MP2 guard บล็อก search เมื่อบอร์ดแข็ง
10. 🎯 **OPT flags** ตั้งตอน `return true` และ reset ใน `OnNewTurn` หลัง `base.OnNewTurn()` — ห้ามเรียก `Plugin.ResetTurnState()` ซ้ำ
11. 🎯 **Extra Deck summon ต้องมีเหตุผล** — `ShouldAvoidGenericExtraDeckSummon(n)`, `ShouldSkipLinkSummon()`, fodder ไม่ใช่ Ace; ห้าม `AddExecutor(SpSummon, id)` แบบไม่มี func
12. 🎯 **Handtrap**: `IsChainAlreadyNeutralized()` → `SmartHandTrapChain()` (duplicate guard ในตัว) → `Default*()`

## 7. Decision Loop & Strategy

```text
OBSERVE → Threat Check → Lethal Check → Resource/OPT/Stack State → Score Actions → Execute → Preserve Follow-up
ActionScore  = BoardImpact + ThreatRemoval + FutureValue + FollowUp + OTK − Cost − MaterialLoss − OpponentAdvantage − RecoveryRisk − Redundancy
RemovalScore = ThreatValue + ZoneDenial + RecursionPrevention + ChainSafety + BoardImpact − OpponentRecoveryValue
```
**Threat Priority**: (1) ทำให้แพ้ทันที → (2) Hard Interaction / Floodgate / Omni-Negate → (3) Search / Draw / Revive / Engine → (4) Board Pressure

**Strategic Rules 🎯**
1. Can Activate ≠ Should Activate · 2. 1 Negate = 1 Problem
3. ให้ทรัพยากรศัตรู (Kaiju/Lava Golem) ต้องผ่าน **Necessary → Compensated → Safe**
4. Removal ดู Zone/Recursion/Float/Immunity; อย่า Destroy การ์ดที่ได้ประโยชน์จากการถูกทำลาย
5. Normal S/T ที่ถูกทำลายกลางเชนยัง Resolve — ทำลายเฉพาะ Continuous/Field/Equip/Scale
6. อย่าใช้การ์ด Multi-purpose กับปัญหาเล็ก · 7. Bait ก่อน Core
8. ห้าม Overextend เมื่อ Lethal/บอร์ดพอชนะ; เก็บ Follow-up · 9. Attack-Lock → หา Out ก่อนหยุด
10. มือจะเกิน 6 ตอน End Phase → เซ็ต Quick-Play/Trap ใน MP2

**Stack Checklist** — ใบแรกใช้แล้ว? ใบที่สอง Resolve ได้? OPT ต่อใบ/ต่อชื่อ? เกิดผลใหม่หรือแค่กินช่อง? มีเป้าคุ้มกว่า? → ไม่มี Net Value = ไม่ใช้ซ้ำ

## 8. Hint Quick Reference (เต็ม: `references/hint_reference.md`)

| ID | Constant | พฤติกรรมที่ต้องการ | Core route |
|---|---|---|---|
| 500 / 501 | RELEASE / DISCARD | Token/Fodder/ตัวซ้ำ/มีผลใน GY ก่อน | `PickDiscardTarget` |
| 502 / 503 | DESTROY / REMOVE | ศัตรู ตาม RemovalScore 🔒 | ThreatScore |
| 504 | TOGRAVE | ศัตรู = removal · เรา = cost · Deck→GY = เลือกตัวที่มีผลใน GY | ศัตรู → ThreatScore · Deck ทั้งหมด → `PickSearchTarget` · อื่น → cost |
| 505 | RTOHAND | ศัตรู = bounce · เรา = กู้ขึ้นมือ | ศัตรู → ThreatScore · เรา → `PickSearchTarget` |
| 506 | ATOHAND | Search ตามแผน | `PickSearchTarget` |
| 507 | TODECK | ศัตรู = spin · เรา = คืนตัวค่าต่ำ | ศัตรู → ThreatScore · เรา → `PickDiscardTarget` |
| 509 | SPSUMMON | Boss/Extender ตามรูท | `PickSpecialSummonTarget` |
| 511 / 512 / 513 / 533 | F / S / X / L MATERIAL | Fodder ก่อน Ace ท้ายสุด | `SortMaterials` (512 ❌) |
| 518 / 519 / 520 | EQUIP / REMOVEXYZ / CONTROL | ตามบริบท / ถอด material ค่าต่ำ / ขโมยศัตรู | 518 แยก branch · 519 = cost (fallback) · 520 ศัตรู |
| 551 | TARGET | ศัตรูถ้าเป็นผลลบ · เราถ้าเป็นบัฟ | ศัตรูเสมอ |
| 572 | COUNTER | ตาม ResourceEvaluator | `OnSelectCounter` |
| 575 | NEGATE | ศัตรู | ThreatScore |
| ≫579 | `Util.GetStringId(id,n)` | custom ของการ์ด — ต้อง handle เอง | ❌ → selector/fallback |

⚠️ ไม่มี 573/574 · ไม่มี `HINTMSG_TOHAND` · 572 = COUNTER, 575 = NEGATE อย่าสลับ · 528 = POSCHANGE · 552 = COIN (ไม่ใช่ removal)

## 9. Material & Interaction Rules

**Material Priority**: Token/Fodder → Low Value → Used Effect → Duplicate → Important → Boss/Ace
- Boss/Ace เป็น Material ได้เฉพาะ: ทางเดียวที่แก้ Threat / Board สุทธิดีขึ้นชัด / ยืนยัน Lethal (`ResourcePlan.EvaluateAceUsage`)
- ไม่ใช้มอนที่สวม Snatch Steal/Comic Hand เป็น Material; ไม่สังเวย ATK 2000+ ใน MP1 เพื่อทำตัวที่อ่อนกว่า
- Fusion/Synchro/Xyz ลง MMZ เก็บ EMZ ให้ Link (core `OnSelectPlace` ทำให้แล้ว) · Bagooska (90590303/90590304) core ตั้ง DEF + กัน repos ให้แล้ว (`IsBagooska`)

**Interaction** 🔒
- Handtrap/Negate ใช้กับ Chokepoint/Threat จริง; Handtrap เก็บบนมือ ไม่หมอบ MP1 (core กัน MonsterSet T1 + Imperm/Dominus set ตอนสนามว่าง)
- ห้าม Chain Attack-Negation ซ้อน (`Magic Cylinder` 62279055 / `Dimension Wall` 67095270)
- `OnSelectEffectYn` ของการ์ดศัตรู → false (core ทำเมื่อไม่มี executor ของการ์ดนั้น)
- ห้าม Tribute Summon ที่เป็นไปไม่ได้ (`Bot.GetMonsterCount() == 0` สำหรับ Lv5+)
- ห้ามเสิร์ชแล้วคืนการ์ดที่เพิ่งเสิร์ชเข้าเด็คทันที
- ตรวจ Resolution ตามข้อความจริง (`"and if you do"` = Conjunction)

## 10. Final Safety Check (ก่อน Build)

- [ ] Card ID / Effect / Cost / `aux.Stringid` n ตรง `cards.cdb` + Lua + Banlist · `python tools/audit_card_ids.py <ไฟล์ที่แก้>` ไม่มี hit ใหม่
- [ ] `DeckPlugin` ครบ Strategy + Material + Threat ใน `Game/AI/Plugins/`
- [ ] `IsAceCard` override + Ace registry 3 ที่ตรงกัน
- [ ] Bait: register **และ** `GetBaitIfNeeded` ใน starter; bait executor อยู่ก่อน starter
- [ ] Handtrap: duplicate guard + neutralized check + `SmartHandTrapChain`
- [ ] ComboRouter step ปลอดภัยทุกตัว มี `Condition`
- [ ] Target ถูกต้อง ไม่มี Self-Harm; 504/505/507 แยกบริบท (หรือยอมรับ core route); ข้อจำกัด core ⚠️ ใน `known_core_issues.md` จัดการแล้ว
- [ ] Activate func รองรับ optional trigger (แยก `ActivateDescription`)
- [ ] Selection ใช้ Pending-Select / custom hint แทน `AI.SelectCard` สำหรับ hint ที่ core จัดการ
- [ ] OPT / Hard OPT / Once-per-Chain ถูกนับและ reset; ไม่มี Duplicate/Stack Waste
- [ ] ไม่ใช้ Resource สำคัญเกินจำเป็น; Lethal Check ผ่าน; เหลือ Follow-up
- [ ] bots.json + DashBot category + Build 0 Errors

> **หลักใหญ่**: อย่าถามแค่ว่า "ใช้การ์ดได้ไหม" — ถามว่า "ใช้ตอนนี้แล้วได้อะไรเพิ่ม และการใช้ซ้ำทำให้เสียช่อง/ทรัพยากรฟรีหรือไม่?"

## 11. Build, Deploy & Git 🔒

### 11.1 Deploy
`C:\Users\admin\Documents\EdoGame\` = ตัวเกม + DashBot — **Deploy ที่นี่เท่านั้น** ผู้ใช้ทดสอบเอง
```powershell
cd C:\Users\admin\Documents\EdoGame\src\YGO_SOURCE_CLEAN
powershell -ExecutionPolicy Bypass -File .\BUILD_AND_DEPLOY.ps1
```

### 11.2 Pull (ผู้ใช้สั่ง "pull")
```powershell
cd C:\Users\admin\Documents\EdoGame\src\YGO_SOURCE_CLEAN
git pull
powershell -ExecutionPolicy Bypass -File .\SYNC_AGENT_ASSETS.ps1   # skill/Docs/PROGRESS/AGENTS → workspace
powershell -ExecutionPolicy Bypass -File .\BUILD_AND_DEPLOY.ps1    # ถ้ามีโค้ดเปลี่ยน
```

### 11.3 Push (ผู้ใช้สั่ง "push" / "commit & push") — ห้ามตกหล่นเด็ดขาด
ทุกครั้งต้อง Push **Source Code + `.agents/` (skill + references) + `Docs/` ทั้งหมด + `PROGRESS.md` + `AGENTS.md`**
```powershell
cd C:\Users\admin\Documents\EdoGame\src\YGO_SOURCE_CLEAN
powershell -ExecutionPolicy Bypass -File .\SYNC_AGENT_ASSETS.ps1   # workspace → repo
git add .agents/ Docs/ PROGRESS.md AGENTS.md SYNC_AGENT_ASSETS.ps1 tools/ configs/
git add -A
git status --short
git commit -m "<type>(<scope>): <summary> (v0.0XX)"
git push
git status --short .agents Docs PROGRESS.md AGENTS.md               # ต้องว่าง
```
- Commit แบบ repo: `feat(ai)`, `feat(core)`, `fix(<deck>)`, `docs(skill)` + เวอร์ชันจาก `PROGRESS.md`
- ห้าม commit เฉพาะ `.cs`/ไบนารีแล้วทิ้ง skill หรือ `Docs/`

### 11.4 Progress Log
`PROGRESS.md` รายการใหม่บนสุด ~200–400 บรรทัด; เกิน ~500–800 → ย้ายเก่าไปต่อท้าย `Docs/PROGRESS_ARCHIVE.md`