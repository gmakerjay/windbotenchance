---
name: yugioh-executor
description: |
  คู่มือพัฒนา, สร้าง Combo, และ Deploy YugiohTH Rule-Based WindBot Executor
  ครอบคลุมโครงสร้างโปรเจกต์, สถาปัตยกรรม Central Core & Decoupled Deck Plugin,
  ขั้นตอน Build & Deploy, และหลักการตัดสินใจเชิงกลยุทธ์ (Strategic Decision System)
---

# YugiohTH Executor Development Skill & Strategic Decision System

## 0. กฎเหล็ก: ห้ามมั่ว ต้องอ้างอิงของจริงจาก Repository ปัจจุบันเสมอ

**ก่อนแก้โค้ดหรือออกแบบคอมโบใดๆ ห้ามเดาเด็ดขาด** ต้องตรวจสอบจากแหล่งจริงของ Repository ปัจจุบันทุกครั้ง:

| สิ่งที่ต้องตรวจ | แหล่งอ้างอิง | วัตถุประสงค์ & ข้อกำหนด |
|---|---|---|
| Card ID, สเตตัส, Effect Text พื้นฐาน | `cards.cdb` | ข้อมูลการ์ดคงที่ (Static Card Data) ห้ามเดา Card ID หรือ Effect Text จากความจำ |
| Effect Logic & Timing เชิงลึก | `script/cXXXX.lua` | ทำความเข้าใจ Trigger Timing, Cost vs Effect, และ Core Behavior ของการ์ดที่ซับซ้อน |
| รายชื่อการ์ดในเด็คและสัดส่วนจริง | ไฟล์ `.ydk` ของเด็คนั้นๆ | ตรวจสอบ Resource ที่มีอยู่จริงในมือ/เด็ค/เอ็กซ์ตร้าเด็ค |
| Method/Callback Signatures จริง | โค้ดจริงใน `windbot-fork/` ปัจจุบัน | **ห้ามสมมติ Signature สากลหรือจำจากเอกสารเก่า** — ต้องเปิดอ่าน method signature ใน repo จริงก่อนเรียกใช้เสมอ |
| Runtime & Target Framework | ไฟล์ `*.csproj` / `*.sln` ปัจจุบัน | ตรวจสอบ .NET Framework/Version จริงจาก Project File ก่อน build ห้ามทึกทักเอาเอง |
| Threat Targets & Chokepoints | `CardIntelligence.cs` | ฐานข้อมูลส่วนกลาง $O(1)$ ของ Floodgate, Negator, Handtrap, Immunity |
| Hint ID ของ OCGCore Constants | `constant.lua` & `ModernExecutor.cs` | ตรวจสอบรหัส Hint ให้ตรงตามเอนจิน OCGCore ห้ามจำสับสน |

---

## 1. Development Workflow & Testing Policy

เมื่อได้รับคำสั่ง **"สร้างเด็คใหม่"**, **"แก้ไขเด็ค"**, หรือ **"ปรับปรุง Executor"** ให้ปฏิบัติตามมาตรฐานนี้อย่างเคร่งครัด:

1. **Card & Engine Audit**: อ่าน `.ydk` + `cards.cdb` + สคริปต์ `script/cXXXX.lua` ของการ์ดหลักทั้งหมด
2. **ออกแบบเชิงกลยุทธ์ (Strategic Design)**:
   - กำหนด Main Route, Backup Routes (Route B/C/D), First Turn End Board, และ Second Turn Board Breaking / OTK
   - วางแผน Resource Management, Negate Budget, และ Stop Conditions
3. **พัฒนาตาม Decoupled Deck Plugin Architecture (MANDATORY)**:
   - ทุกครั้งที่สร้างหรือแก้ไขเด็ค **ต้องนำสถาปัตยกรรม Decoupled Domain Plugin (`DeckPluginBase` / `IDeckPlugin`) มาติดตั้งใช้งานจริงเสมอ โดยไม่ต้องรอให้ผู้ใช้สั่งซ้ำ**
   - แยก Domain Helpers (`IDeckStrategy`, `IDeckMaterialEvaluator`, `IDeckThreatEvaluator`, `IDeckResourceEvaluator`) ออกจาก Flow การสั่งการ์ด
   - ติดตั้งผ่าน `DeckPlugin = new XxxPlugin(this);` ใน Constructor ของ Executor เสมอ
   - สืบทอดจาก `ModernExecutor` และลงทะเบียนใน `bots.json`
4. **Build & Deploy ไปยัง Exclusive Target**:
   - คอมไพล์และ Deploy มาที่ `C:\Users\admin\Documents\EdoGame\` เสมอ ผ่านสคริปต์ `BUILD_AND_DEPLOY.ps1`
   - บันทึกการเปลี่ยนแปลงลงใน [PROGRESS.md](file:///C:/Users/admin/Documents/EdoGame/PROGRESS.md)
5. **นโยบายการทดสอบ Headless Simulation (กฎเหล็ก)**:
   - 🚫 **ห้ามรันการจำลองดวล Headless Simulator โดยอัตโนมัติ** หลังสร้างหรือแก้ไขเด็คเสร็จ
   - ✅ **จะทำการรัน Headless Text Duel ได้ก็ต่อเมื่อผู้ใช้สั่ง "Text Duel" (หรือ "จำลองดวล") เท่านั้น**
   - เมื่อผู้ใช้สั่ง "Text Duel" เท่านั้น จึงทำการรันดวลทดสอบกับคู่ซ้อม Legacy 4 เด็ค: `ABC`, `Altergeist`, `BlueEyes`, `DarkMagician`
     ```powershell
     dotnet run --project src\YGO_SOURCE_CLEAN\Client_Headless_Fortest\Client_Headless_Fortest.csproj -c Release -- --deck <DECK_NAME> --opponent <ABC|Altergeist|BlueEyes|DarkMagician> --games 10 --timeout 60
     ```
   - สรุปผลสถิติ Win Rate %, สถิติ Violations (ต้องเป็น 0), และ Playbook Strategy

### 1.1 ขั้นตอนมาตรฐานในการสร้างและลงทะเบียนเด็คใหม่ (Step-by-Step Implementation Guide)

#### ขั้นตอนที่ 1: เตรียมไฟล์เด็ค (`.ydk`)
1. จัดเด็คในเกม EDOPro และบันทึกไฟล์ `.ydk`
2. วางไฟล์ `.ydk` ไว้ที่ `windbot-fork/Decks/<DeckName>.ydk`
   - ใช้ชื่อเด็คที่สะอาด กระชับ เช่น `Tenpai.ydk`, `Centurion.ydk`, `Endymion.ydk` (ห้ามมีคำนำหน้าปีหรือเวอร์ชัน เช่น `2026_`)
   - เด็คตัวละครอนิเมะ: ให้ใช้ `Anime_<Character>.ydk`

#### ขั้นตอนที่ 2: สร้างไฟล์ Executor C#
1. สร้างไฟล์ใหม่ที่ `windbot-fork/Game/AI/Decks/<DeckName>Executor.cs`
2. ให้สืบทอดจาก `ModernExecutor`:
```csharp
using System.Collections.Generic;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Plugin;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("SampleDeck", "SampleDeck")]
    public class SampleDeckExecutor : ModernExecutor
    {
        public enum CardId
        {
            // ระบุ Card ID ที่ใช้ในเด็ค (ตรวจสอบจาก cards.cdb เสมอ)
        }

        public SampleDeckExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // 0. ติดตั้ง Decoupled Domain Plugin ประจำเด็คเสมอ (MANDATORY)
            DeckPlugin = new SampleDeckPlugin(this);

            // 1. ลงทะเบียนการ์ดสำคัญ (Starters, Extenders, Handtraps, Board Breakers)
            // 2. กำหนดลำดับการเล่น Spells, Monsters, Traps ผ่าน AddExecutor
            // 3. กำหนด Hint Table สำหรับการเลือกการ์ดอัตโนมัติ
        }

        public override bool OnSelectHand()
        {
            return true; // true = First turn, false = Second turn
        }

        // เขียนตรรกะคอมโบและการ Activate การ์ดเฉพาะ
    }

    // ==========================================
    // Decoupled Domain Plugin Architecture (MANDATORY FOR ALL DECKS)
    // ==========================================
    internal class SampleDeckPlugin : DeckPluginBase
    {
        private readonly SampleDeckExecutor _exec;
        public override string DeckName => "SampleDeck";

        public override IDeckStrategy Strategy { get; }
        public override IDeckMaterialEvaluator MaterialEvaluator { get; }
        public override IDeckThreatEvaluator ThreatEvaluator { get; }

        public SampleDeckPlugin(SampleDeckExecutor exec)
        {
            _exec = exec;
            Strategy = new SampleDeckStrategy(exec);
            MaterialEvaluator = new SampleDeckMaterialEvaluator(exec);
            ThreatEvaluator = new SampleDeckThreatEvaluator(exec);
        }
    }

    internal class SampleDeckStrategy : IDeckStrategy
    {
        private readonly SampleDeckExecutor _exec;
        public SampleDeckStrategy(SampleDeckExecutor exec) => _exec = exec;
        public void Reset() { }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            // กำหนดเป้าหมายอัญเชิญพิเศษตามลำดับความสำคัญ (Combo Extender / Boss)
            return candidates.FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            // กำหนดเป้าหมายค้นหาขึ้นมือตาม Priority (Starter -> Extender -> Disruption)
            return candidates.FirstOrDefault();
        }
    }

    internal class SampleDeckMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly SampleDeckExecutor _exec;
        public SampleDeckMaterialEvaluator(SampleDeckExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 0;
            // คุ้มครอง Boss / Floodgate / Key Monsters ไม่ให้นำไปสังเวยหรือเป็นวัตถุดิบ
            return 10;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            return candidates.OrderBy(c => GetMaterialCost(c)).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            // เลือกทิ้งการ์ดที่มีผลในสุสาน หรือการ์ดซ้ำที่ไม่จำเป็น
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            return candidates.FirstOrDefault();
        }
    }

    internal class SampleDeckThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly SampleDeckExecutor _exec;
        public SampleDeckThreatEvaluator(SampleDeckExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            // เพิ่ม Threat Score สำหรับการ์ดตัวปัญหาเฉพาะทางที่ขัดขวางเด็คนี้
            return 0;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            return false;
        }
    }
}
```

#### ขั้นตอนที่ 3: ลงทะเบียนเด็คในระบบ
1. เพิ่มข้อมูลบอทใน `windbot-fork/bots.json`:
```json
{
  "name": "SampleDeck",
  "deck": "SampleDeck",
  "dialog": "default",
  "flags": ["OCG", "TCG"]
}
```
2. ตรวจสอบให้แน่ใจว่า `DecksManager.cs` แมปชื่อเด็คเข้ากับ Executor ได้อย่างถูกต้อง

#### ขั้นตอนที่ 4: คอมไพล์และ Deploy
รันคำสั่งใน PowerShell:
```powershell
cd C:\Users\admin\Documents\EdoGame\src\YGO_SOURCE_CLEAN
powershell -ExecutionPolicy Bypass -File .\BUILD_AND_DEPLOY.ps1
```

---

## 2. Strategic Decision System (หลักการตัดสินใจเชิงกลยุทธ์)

> **"Do not play cards. Play the game state."**  
> บอทที่ดีไม่ได้แค่รู้ว่าการ์ดทำอะไรได้ แต่รู้ว่า **เมื่อไหร่ควรทำ และเมื่อไหร่ไม่ควรทำ** โดยเลือก Action ที่ให้ **Expected Value สูงสุด** ไม่ใช่คอมโบที่ยืดยาวที่สุด

### 2.1 Core Decision Loop
ทุกการตัดสินใจต้องผ่านกระบวนการประเมินสภาวะของเกม:
```text
OBSERVE (Board/Hand/GY/LP) → IDENTIFY THREATS → CHECK WIN/DEFENSE → SCORE ACTIONS → EXECUTE → PRESERVE FOLLOW-UP
```

### 2.2 Threat Priority Model (ลำดับความสำคัญของภัยคุกคาม)
1. **Priority 1 — Immediate Lethal Threat**: การ์ดหรือสถานการณ์ที่ทำให้เราแพ้ในเทิร์นนี้ (ต้องเคลียร์ก่อนเสมอ)
2. **Priority 2 — Hard Interaction**: Omni Negate, Monster Negate, S/T Negate, Continuous Floodgate, Turn-skip lock
3. **Priority 3 — Resource Engine**: การ์ดค้นหา (Search), จั่ว (Draw), ชุบ (Revive), หรือสร้าง Token ของคู่แข่ง
4. **Priority 4 — Board Pressure**: มอนสเตอร์ ATK สูงแต่ไม่มี Interaction ขัดจังหวะ

---

### 2.3 Contextual Removal Evaluation (การประเมินการขจัดเชิงบริบท)

**หลีกเลี่ยงการล็อกลำดับ Removal แบบตายตัว** (เช่น ไม่ทึกทักว่า Banish > Deck > Bounce > Destroy เสมอไป) เพราะใน Yu-Gi-Oh! สถานการณ์เป็นตัวกำหนดคุณค่า:
- **Destroy**: อาจทริกเกอร์ Float/GY Effect ของคู่แข่ง
- **Bounce**: อาจส่งผลให้คู่แข่งนำ Starter กลับไปร่ายซ้ำ
- **Banish**: บางเด็คได้เปรียบจากการถูกรีมูฟ หรือมีลูกเล่นดึงการ์ดที่ถูกรีมูฟกลับมา
- **Spin to Deck**: มักตัดการวนลูปได้ดี แต่ไม่คุ้มหากเป้าหมายเป็น Extra Deck Monster ที่เรียกกลับมาง่าย

ให้ประเมินการขจัดด้วย **RemovalScore**:
```text
RemovalScore = 
    ThreatValue           (ความอันตรายของเป้าหมาย)
  + ZoneDenialValue       (การเคลียร์พื้นที่โซนสำคัญ)
  + RecursionPrevention   (การตัดวงจรชุบ/ใช้งานซ้ำ)
  + ChainSafety           (ความปลอดภัยจากการโดน Chain ตอบโต้)
  + BoardImpact           (ผลกระทบต่อบอร์ดโดยรวม)
  - OpponentRecoveryValue (มูลค่าที่ฝ่ายตรงข้ามอาจกู้คืนกลับมาได้)
```
แล้วจึงเลือกวิธีขจัด (Banish / Spin / Bounce / Destroy / Tribute) ที่ให้ผลสุทธิสูงสุดต่อรูปเกม

---

### 2.4 Contextual Anti-Advantage Gate (ประตูประเมินการเสียเปรียบทรัพยากร)

**หลักการ**: ห้ามเพิ่มทรัพยากรหรือศักยภาพให้ฝ่ายตรงข้ามโดยไม่มีเหตุผลเชิงกลยุทธ์ที่คุ้มค่าเพียงพอ

ห้ามใช้กฎห้ามแบบเด็ดขาด (Absolute Ban) จนบอทไม่กล้าเล่นการ์ดอย่าง Kaiju, Lava Golem, หรือการ์ดแลกเปลี่ยน ให้ใช้ Decision Gate นี้แทน:
```text
OpponentAdvantageGranted
         ↓
    Necessary?       (จำเป็นต่อการแก้ทาง Threat สำคัญ หรือปลด Floodgate หรือไม่?)
         ↓ (Yes)
   Compensated?      (เราได้แต้มต่อคืนมาคุ้มค่า เช่น Board Breaker / OTK Confirm หรือไม่?)
         ↓ (Yes)
      Safe?          (การ์ดที่มอบให้คู่แข่ง จะไม่สามารถถูกนำมาใช้สวนกลับหรือเป็น Material ได้ทันทีใช่หรือไม่?)
         ↓
  Allow / Reject
```

---

### 2.5 Risk-Aware Decision Thresholds (เกณฑ์ประเมินความเสี่ยง)

#### 1. Nibiru Risk Threshold Model
- **Summon #4 เป็น "Risk Threshold" ไม่ใช่ "Stop Rule"**: กฎ Nibiru ต้องการ Normal/Special Summon ครบ 5 ครั้งขึ้นไปถึงจะเปิดใช้ได้
- บอทต้องประเมินสถานะที่การซัมมอนครั้งที่ 4:
  - **มี Negate ปกป้องอยู่แล้ว**: รันคอมโบต่อได้ทันที
  - **Action ที่ 5 สามารถออก Negate ได้พอดี**: รันต่อไปเพื่อตั้งเสาป้องกัน
  - **มี Lethal Confirmed หรือบอร์ดคู่แข่งเคลียร์หมดแล้ว**: ดันเข้า Battle Phase ทันที
  - **การ์ดบนมือเหลือเพียง Extender ที่ไม่นำไปสู่ Negate หรือ Recovery**: พิจารณา **หยุดที่ครั้งที่ 4** เพื่อไม่ให้บอร์ดพังทั้งหมด

#### 2. Maxx "C" Minimum Extension Line
- เมื่อคู่แข่งเปิด Maxx "C" **ห้ามสั่ง Pass Turn ทันทีอย่างไร้เหตุผล** (อาจโดน OTK พ่ายแพ้ฟรี)
- ให้คำนวณ **Minimum Extension**:
  ```text
  Under Maxx "C"
          ↓
  Estimate Draws Given vs Resulting Board Value vs Can End Game? vs Minimal Interaction
          ↓
  Choose Minimum-Extension Line
  ```
  - สเปเชียล 1–2 ครั้งแล้วได้ Negate/Interruption ขัด 1 ครั้ง มักคุ้มค่ากว่าการปล่อยบอร์ดว่างให้คู่แข่งตีปิดเกม

### 2.7 Interruption Density & Balanced Deck Construction (ความหนาแน่นของการ์ดขัดจังหวะ)

บอทที่เก่งไม่ได้มีเพียงคอมโบที่หมุนได้รอบเด็ค แต่ต้องมี **Interaction Density** ที่เพียงพอต่อการขัดขวางคู่แข่งทั้งตอนเริ่มก่อน (Turn 1) และแก้ทางตอนเริ่มหลัง (Going Second):

1. **สัดส่วนการ์ดขัดจังหวะในเด็ค (Interaction / Handtrap Budget)**:
   - เด็คขนาด 40 ใบ ควรมี Handtraps และ Board Breakers อย่างน้อย **9–15 ใบ** (22%–37% ของเด็ค) เพื่อการันตีโอกาสจั่วได้การ์ดขัดอย่างน้อย 1–2 ใบในมือเริ่มต้น
   - หลีกเลี่ยง "Over-combofication": การใส่ Starter และ Extender มากเกินไปจนไม่มีที่ว่างให้การ์ดขัด จะทำให้เด็คแพ้ทันทีเมื่อเล่น Going Second หรือเมื่อคอมโบหลักถูกคู่แข่งตัดตอน
2. **First Turn Interruption vs Second Turn Breaker**:
   - **First Turn**: มุ่งเน้นการจบกระดานที่มี Interruption หลากหลายมิติ (เช่น Monster Negate, S/T Negate, Non-Target Removal, Floodgate ชั่วคราว)
   - **Going Second**: อาศัย Handtraps ตัดตอน Chokepoints และ Board Breakers เคลียร์ Threat ก่อนเริ่มเดิน Engine หลัก
3. **In-Archetype Disruption Synergy**:
   - ให้ความสำคัญกับการค้นหาการ์ดขัดในธีมขึ้นมือหรือเซ็ตลงสนามเป็นส่วนหนึ่งของ End Board (เช่น Counter Trap, Quick Spell ขัดขวาง) ไม่พึ่งพาเพียงการตั้ง Beatdown มอนสเตอร์ตัวเปล่า

---

### 2.8 Strategic Rules of Engagement (กฎทองคำ 10 ประการ)

1. **Can Activate ≠ Should Activate**: อย่าเปิดใช้เอฟเฟกต์เพียงเพราะทำได้ ต้องระบุเป้าหมายและผลลัพธ์ที่ชัดเจนเสมอ
2. **Anti-Overextend & Win Condition First**: เมื่อเข้าสู่ Lethal Confirmed หรือคู่แข่งไม่มีตัวขัดขวาง ให้หยุดขยายคอมโบและเข้า Battle Phase ตีปิดเกมทันที
3. **High Material Cost Awareness**: ห้ามนำ Ace Boss, Quick-Effect Interruption, หรือ Floodgate ไปเป็น Material โดยไม่จำเป็น
4. **Chain Discipline (1 Negate = 1 Problem)**: ห้ามซ้อน Negate หลายชั้นใส่ Action เดียวของคู่แข่งถ้า Threat นั้นถูก Neutralize ไปแล้ว
5. **MST & Removal Discipline**: ทำลายเฉพาะการ์ดที่ต้องคงอยู่บนสนามเพื่อส่งผล (`Continuous`, `Field`, `Equip`, `Pendulum Scale`) ห้ามโซ่ทำลายใส่ `Normal Spell` / `Normal Trap` เด็ดขาด ("MST Negates Fallacy")
6. **Targeting Sanity**: เมื่อใช้เอฟเฟกต์ลบกระดาน ต้องเล็งการ์ดของฝ่ายตรงข้าม (`c.Controller == 1`) เป็นลำดับแรกเสมอ
7. **Preserve Follow-up**: รักษามือหรือทรัพยากรสำหรับ Turn 3 เสมอ บอร์ดที่มีตัวขัดขวางพร้อมตัวเดินเกมเทิร์นหน้า ดีกว่าบอร์ดที่เทหมดหน้าตัก
8. **Anti-Hoarding vs Anti-Brick**: ใช้การ์ดทันทีเมื่อ Current Value > Expected Future Value อย่ากั๊กจนแพ้ และอย่าทิ้งการ์ดสารพัดประโยชน์ไปกับงานเล็ก
9. **Bait Before Starters**: ส่งการ์ด Bait หรือ Searcher รองไปล่อ Handtrap ของคู่แข่งก่อนเดิน Core Engine
10. **Information As Resource**: จดจำการ์ดที่คู่แข่งนำขึ้นมือ และสถานะ Once-Per-Turn ที่คู่แข่งใช้ไปแล้ว เพื่อวางแผนดักทาง
11. **Continuous Floodgate & Attack-Lock Adaptation**: เมื่ออยู่ภายใต้ `Gravity Bind`, `Level Limit - Area B`, หรือ Floodgate ล็อคการโจมตี บอทต้องสลับโหมดกลยุทธ์จากการ Beatdown ไปเป็นการใช้ Ignition Effect ขจัดมอนสเตอร์ศัตรูทันที และห้ามหยุดเล่น (`RESOURCE-STOP`) เพื่อค้นหาการ์ดแก้ทาง (Outs)
12. **End-Phase Hand Limit Setting Buffer**: เมื่อการ์ดบนมือ $\ge 6$ ใบ (ใกล้ติด Hand Size Limit Discard) ให้จัดสรรการเซ็ต Quick-Play Spells และ Traps ใน Main Phase 2 เพื่อป้องกันการทิ้งการ์ดลงสุสานฟรี

---

### 2.9 DeckProbability Mathematical Engine (Hypergeometric & Multivariate)

ระบบคำนวณสถิติความน่าจะเป็นแบบ $O(1)$ ใน `WindBot.Game.AI.DeckProbability` สำหรับประเมินความเสี่ยงและลำดับการเล่น:

1. **Hypergeometric Distribution (`DeckProbability.Hypergeometric`, `AtLeastOne`, `AtLeast`)**:
   - คำนวณความน่าจะเป็นของการสุ่มหยิบแบบไม่ใส่คืน ($N$ การ์ดทั้งหมด, $K$ การ์ดเป้าหมาย, $n$ จำนวนที่จั่ว/ขุด, $k$ จำนวนที่ต้องการ)
   - **การนำไปใช้:**
     - `AtLeastOne(N, K, n)`: โอกาสเปิดได้ Starter ในมือแรก $\ge 85\%$ หรือโอกาสขุดเจอการ์ดแก้ทาง (Excavation)
     - `BanishLossRisk(remainingDeck, criticalCopies, banishCount)`: ประเมินความเสี่ยงของ `Pot of Desires` (รีมูฟ 10 ใบ) ว่าจะทำให้ชิ้นส่วนสำคัญหลุดหมดเด็คหรือไม่ หากเสี่ยงสูง ให้สั่ง Search ก่อน Draw
2. **Multivariate Hypergeometric (`DeckProbability.MultivariateHypergeometric`)**:
   - คำนวณโอกาสเปิดได้การ์ดหลายกลุ่มพร้อมกัน (เช่น 2-Card Combo: Starter + Extender โดยไม่มี Garnet)
3. **Mathematical Handtrap Estimation ใน `BaitPlanner`**:
   - `EstimateHandTrapLikelihood(opponentHandCount, opponentDeckCount, estimatedHandtraps)`:
     แทนที่การคาดเดาด้วยการคำนวณ Hypergeometric จริงตามจำนวนการ์ดบนมือคู่แข่งและประเมินว่าควรส่งการ์ด Bait ไปล่อหรือไม่
4. **Dynamic Replacement Risk ใน Domain Plugin**:
   - เมื่อต้องทิ้งการ์ด (Discard Cost) ใช้ `DeckProbability.AtLeastOne` เช็คโอกาสจั่วการ์ดทดแทน หากการ์ดในมือเหลือ 0 ใบในเด็ค ให้กันไว้ไม่ทิ้ง (Preserve One-of)

---

## 3. ข้อห้ามเด็ดขาดในการเขียน Executor (CRITICAL ANTI-PATTERNS)

1. 🚫 **ห้ามใช้ `preferred` list ใน `OnSelectCard` โดยไม่แยกแยะ Hint ID**:
   - ลิสต์ค้นหาการ์ดขึ้นมือ ต้องเช็ค `hint == 506 (HINTMSG_ATOHAND)` เท่านั้น
   - เมื่อ Hint เป็นคำสั่งลบสนาม (`502 [DESTROY]`, `503 [REMOVE]`, `504/508 [TOGRAVE]`, `505 [RTOHAND]`, `507 [TODECK]`): **ต้องบังคับเลือกเฉพาะการ์ดของศัตรู (`c.Controller == 1`) เสมอ**
2. 🚫 **ห้าม Extra Deck Summon คืนค่า `return true;` แบบไร้เงื่อนไข**:
   - ห้ามนำมอนสเตอร์ที่สวมใส่การ์ดขโมย (เช่น `Comic Hand`, `Snatch Steal`) ไปทำวัตถุดิบ Extra Deck
   - ห้ามสังเวยมอนสเตอร์ตีตรงพลังสูง (2000+) ใน Main Phase 1 เพื่อทำ Extra Deck ที่พลังน้อยกว่า
   - ห้ามนำ Boss Monster บนสนาม 2 ตัวไปทำ Xyz จนเสียบอร์ดขัดขวางและพลังโจมตีลด
   - ⚔️ **กฎเหล็ก S:P Little Knight (29301450)**: ห้ามอัญเชิญ S:P Little Knight ใน Main Phase 1 หากสนามศัตรูไม่มีมอนสเตอร์ หรือเรามีโอกาสโจมตีตรง (เนื่องจาก Effect 1 จะล็อกห้ามมอนสเตอร์ทุกตัวโจมตีตรงในเทิร์นนั้นจนชวดโอกาสทำดาเมจ/OTK) และห้ามนำ Boss Monster หรือตัว ATK ≥ 2000 ไปสังเวยทำ S:P เด็ดขาด ให้ดันเข้า Battle Phase ตีก่อน แล้วค่อยเรียก S:P ใน Main Phase 2 เสมอ
3. 🚫 **ห้ามเงื่อนไข Tribute Summon ที่เป็นไปไม่ได้**:
   - ห้ามเขียนเงื่อนไขบูชายัญมอนสเตอร์เลเวล 5+ ว่า `Bot.GetMonsterCount() == 0` เด็ดขาด
4. 🚫 **ห้ามเสิร์ชแล้วเลือกการ์ดเดิมวางกลับเด็คทันที**:
   - ใน `OnSelectCard` เมื่อต้องคืนการ์ดเข้าเด็ค (เช่น `Illusion of Chaos`) ต้องเลือกการ์ดที่ไม่จำเป็น ห้ามคืนการ์ดที่เพิ่งเสิร์ชมา
5. 🚫 **ห้ามนำ Handtrap ไปหมอบบนสนามใน Main Phase 1**:
   - การ์ดที่ใช้งานจากบนมือได้ (เช่น `Ash Blossom`, `Dominus Impulse`, `Ghost Ogre`) ต้องเก็บไว้บนมือเท่านั้น
6. 🚫 **ห้ามรีมูฟหรือทิ้ง Core Boss สำคัญของเด็คอย่างไร้เหตุผล**:
   - ใน `Pot of Prosperity` หรือ Cost ต่างๆ ต้องยกเว้น Core Boss ห้ามนำไปรีมูฟ
7. 🚫 **ห้ามโซ่ทำลายใส่ Normal Spell / Normal Trap**:
   - การทำลายไม่ Negate ผล ห้ามโซ่ MST หรือ Removal ทั่วไปใส่ Normal Spell/Trap เด็ดขาด
8. 🚫 **ห้ามรัน Headless Simulation โดยอัตโนมัติ (กฎเหล็ก)**:
   - รอคำสั่ง "Text Duel" (หรือ "จำลองดวล") จากผู้ใช้เท่านั้น
9. 🚫 **ห้าม Deploy นอกโฟลเดอร์ `C:\Users\admin\Documents\EdoGame\` เด็ดขาด**:
   - ทุกไบนารีต้อง Deploy ผ่าน `BUILD_AND_DEPLOY.ps1` มาที่โฟลเดอร์เป้าหมายหลักเท่านั้น
10. 🚫 **ห้ามใช้ AI Training / Neural Models / RL**:
    - โปรเจกต์นี้เป็น Rule-Based C# Executor 100% ห้ามสร้างโมเดล Neural หรือ RL
11. 🚫 **ห้ามใช้คำนำหน้าปีหรือเวอร์ชันในชื่อเด็ค / บอท**:
    - ใช้ชื่อสะอาด เช่น `Tenpai`, `Centurion`, `Endymion` ห้ามมี `2026_` นำหน้า
12. 🚫 **ห้ามใส่ Card ID ผิด หรือการ์ดผิด Banlist**:
    - ตรวจสอบกับ `cards.cdb` และ `0TCG.lflist.conf` / `OCG.lflist.conf` ป้องกันข้อผิดพลาด `ERRMSG_DECKERROR`
13. 🚫 **ห้ามนำมอนสเตอร์ Fusion / Synchro / Xyz ลง Extra Monster Zone (EMZ) โดยไม่จำเป็น**:
    - รักษา EMZ ไว้ให้ Link Monster เพื่อไม่ให้โซนตัน
14. 🚫 **ห้ามอัญเชิญ Number 41: Bagooska ในสภาพ FaceUpAttack**:
    - Bagooska ต้องอัญเชิญในสภาพ **FaceUpDefence** เสมอ เพื่อให้ผลฟลัดเกตทำงานต่อเนื่อง
15. 🚫 **ห้าม Chain Handtrap หรือ Negate ซ้ำซ้อนในเชนเดียวกัน**:
    - ห้ามโยน Ash ซ้อน Ash หรือ Maxx "C" ซ้อน Maxx "C"
16. 🚫 **ห้ามตอบรับ (Accept) เอฟเฟกต์ทางเลือกของศัตรูโดยไม่ตั้งใจ**:
    - ใน `OnSelectEffectYn` หากเป็นการ์ดของฝ่ายตรงข้าม (`card.Controller == 1`) ให้ตอบปฏิเสธ (`false`) เป็นค่าเริ่มต้น
17. 🚫 **ห้ามละเลย Decoupled Domain Plugin Architecture เด็ดขาด (CRITICAL MANDATORY)**:
    - ทุกเด็คที่สร้างหรือ Refactor **ต้องเชื่อมต่อและใช้งานสถาปัตยกรรม Decoupled Domain Plugin (`ExecutorBase/Game/AI/Plugin`) เสมอ 100% โดยไม่ต้องรอให้ผู้ใช้สั่งซ้ำ**
    - ห้ามเขียน Domain Logic (Material evaluation, Discard scoring, Archetype threat scoring, Search target picking) อัดแน่นปะปนใน Executor Router เพียงลำพังโดยไม่มี Plugin รองรับ
18. 🚫 **ห้าม Activate Ignition Removal Effect เมื่อสนามฝ่ายตรงข้ามไม่มีการ์ดเป้าหมายที่ถูกต้อง (Target Verification Safeguard)**:
    - ในเอฟเฟกต์ที่เลือกเป้าหมายเพื่อทำลาย/รีมูฟ/เด้ง (เช่น `BLS`, `Chaos Sorcerer`, `Exiled Force`) ฟังก์ชันต้องตรวจสอบว่า `Enemy.GetMonsters().Any(...)` หรือเป้าหมายของศัตรูมีอยู่จริงก่อน `return true` เสมอ
    - หากไม่มีการ์ดศัตรูให้ทำลาย ต้อง `return false` ทันทีเด็ดขาด ห้ามคืนค่า `true` เพื่อหวังผลอื่น เพราะตัวเกมจะบังคับให้เล็งการ์ดพวกเดียวกันเองหรือเล็งตัวเองจนสนามพัง (เช่น กรณี BLS สั่งแบนตัวเอง)
19. 🚫 **ห้ามจ่าย Flat LP Cost จน Life Points ลดลงต่ำกว่าจุดปลอดภัยวิกฤต (LP Cost Safety Threshold)**:
    - การ์ดที่ต้องจ่าย LP เป็นค่าคงที่ เช่น `Delinquent Duo` (1000 LP), `Premature Burial` (800 LP), `Solemn Warning` (2000 LP) ต้องเช็ค `Bot.LifePoints > 2000` (หรือเกณฑ์ปลอดภัย) เสมอ โดยเฉพาะเมื่อมีฟลัดเกตอย่าง `Chain Energy` (500 LP/action) เพื่อป้องกันไม่ให้ LP เหลือน้อยกว่า 500 จนติด Engine Hard-Lock เล่นการ์ดจากมือไม่ได้

---

## 4. สถาปัตยกรรม Decoupled Domain Plugin Architecture (Layer 3) — MANDATORY FOR ALL DECKS

> 🛑 **กฎเหล็กเชิงสถาปัตยกรรม (MANDATORY ARCHITECTURE RULE)**:  
> ทุกเด็คไม่ว่าจะเป็นเด็ค Combo, Control, Stun, Synchro, Xyz, Link หรือ Beatdown **ต้องแยก Domain Logic ออกเป็นเลเยอร์ผ่าน Decoupled Domain Plugin (`ExecutorBase/Game/AI/Plugin`) เสมอ 100% โดยอัตโนมัติ โดยไม่ต้องรอให้ผู้ใช้สั่งซ้ำ**

```text
┌─────────────────────────────────────────────────────────────┐
│                    CENTRAL CORE PLATFORM                    │
│   CardIntelligence │ BoardScorer │ FallbackSelectCard       │
│   HeuristicGuard   │ ChainAdvisor│ ResourcePlan             │
└──────────────────────────────┬──────────────────────────────┘
                               │ inherits & coordinates
┌──────────────────────────────▼──────────────────────────────┐
│                    MODERN EXECUTOR (ROUTER)                 │
│   Maintains execution flow, card activation priorities,     │
│   delegates domain decisions via `IDeckPlugin` property     │
└──────────────────────────────┬──────────────────────────────┘
                               │ delegates domain decisions
┌──────────────────────────────▼──────────────────────────────┐
│             DOMAIN PLUGIN (IDeckPlugin / DeckPluginBase)    │
│   ├─ IDeckStrategy (Combo sequencing & search targets)      │
│   ├─ IDeckMaterialEvaluator (Material scoring & Ace guard)  │
│   ├─ IDeckThreatEvaluator (Archetypal threat analysis)      │
│   ├─ IDeckResourceEvaluator (Counters / specific economy)   │
│   ├─ IDeckScaleResolver (Pendulum scale setup & pop targets)│
│   └─ IDeckActionScorer (Action value curve scoring)         │
└─────────────────────────────────────────────────────────────┘
```

### 4.1 ชุดสัญญา Central Domain Plugin Interfaces (`ExecutorBase/Game/AI/Plugin/`)

| Interface / Class | Contract Responsibilities & Methods | ModernExecutor Integration Points |
|---|---|---|
| **`IDeckPlugin`** / **`DeckPluginBase`** | คอนแทร็กต์หลักรวม Domain Sub-helpers ทั้งหมด และจัดการ `ResetTurnState()` | ผูกผ่านพร็อพเพอร์ตี้ `DeckPlugin = new XxxPlugin(this);` |
| **`IDeckStrategy`** | • `PickSpecialSummonTarget(candidates)`<br>• `PickSearchTarget(candidates, context)` | เรียกใช้เพื่อเลือกเป้าค้นหาจากเด็ค และเลือกเป้าหมายชุบ/สเปเชียลที่ถูกต้องตามลำดับแผน |
| **`IDeckMaterialEvaluator`** | • `GetMaterialCost(card)` (กำหนด Cost ป้องกัน Ace ถูกสังเวย)<br>• `SortMaterials(candidates, min)`<br>• `PickDiscardTarget(candidates, min)`<br>• `PickDestructionSubstitute(candidates, min)` | • `OnSelectFusionMaterial`, `OnSelectSynchroMaterial`, `OnSelectXyzMaterial`, `OnSelectLinkMaterial` เรียก `SortMaterials` อัตโนมัติ<br>• `OnSelectCard` (Hint Discard) เรียก `PickDiscardTarget` อัตโนมัติ |
| **`IDeckThreatEvaluator`** | • `EvaluateThreatScore(card)`<br>• `IsEmergencyThreat(card)` | `CardIntelligence.GetThreatLevel()` ผสาน `score += ThreatEvaluator.EvaluateThreatScore(c)` อัตโนมัติ |
| **`IDeckResourceEvaluator`** | • `GetAvailableResourceCount()`<br>• `CanSafelySpendResource(cost)`<br>• `SelectCounters(quantity, cards, counters)` | `OnSelectCounter` ส่งต่อให้ `SelectCounters` อัตโนมัติ |
| **`IDeckScaleResolver`** | • `PickLowScale()`<br>• `PickHighScale()`<br>• `PickScalePopTarget()` | รองรับเด็ค Pendulum ในการวางสเกลและการระเบิดสเกล |
| **`IDeckActionScorer`** | • `CalculateActionScore(actionName, boardImpact, netGain, cost)` | ประเมินคะแนนความคุ้มค่าของการกระทำก่อนสั่งร่าย |

### 4.2 กลไกการเชื่อมโยงอัตโนมัติใน `ModernExecutor` (Zero-Boilerplate Dispatch)

เมื่อกำหนด `DeckPlugin = new MyDeckPlugin(this);` ใน Constructor ระบบกลางของ `ModernExecutor` จะเชื่อมต่อโดยอัตโนมัติ:
1. **Turn Reset Loop**: `OnNewTurn()` จะเรียก `DeckPlugin?.ResetTurnState()` ให้โดยอัตโนมัติเพื่อล้าง State ของเทิร์นก่อนหน้า
2. **Threat Assessment**: ในการคำนวณ Threat Score ของการ์ดศัตรู ระบบจะเรียก `DeckPlugin.ThreatEvaluator.EvaluateThreatScore(c)` เข้าไปเสริมคะแนนพื้นฐาน
3. **Extra Deck Material Safeguard**: เมื่อต้องเลือกวัตถุดิบ Extra Deck ระบบจะเรียก `DeckPlugin.MaterialEvaluator.SortMaterials(...)` นำมอนสเตอร์ที่มี MaterialCost ต่ำ (Fodder) ขึ้นก่อน และกันมอนสเตอร์ที่มี Cost สูง (Ace Boss, Negators) ไว้ท้ายสุด
4. **Discard Cost Optimization**: เมื่อเจอคำสั่งทิ้งการ์ด (`HINTMSG_DISCARD`) ระบบจะเรียก `DeckPlugin.MaterialEvaluator.PickDiscardTarget(...)` เพื่อเลือกทิ้งการ์ดที่มีผลในสุสานหรือการ์ดที่ไม่จำเป็น
5. **Counter Deduction**: เมื่อเจอคำสั่งเลือกหักเคาน์เตอร์ ระบบจะส่งต่อให้ `DeckPlugin.ResourceEvaluator.SelectCounters(...)`

### 4.3 มาตรฐานการติดตั้งสำหรับทุกเด็ค (Standard Baseline Plugin Pattern)

**ทุกเด็ค (100%)** ต้องมีคลาส Plugin ที่สืบทอดจาก `DeckPluginBase` โดยมี Domain Helpers ประจำเด็คอย่างน้อย 3 ส่วนเสมอ:
1. **`Strategy` (`IDeckStrategy`)**: กำหนดลำดับ Search และ Special Summon
2. **`MaterialEvaluator` (`IDeckMaterialEvaluator`)**: ป้องกัน Ace Boss จากการสังเวย/ทำวัตถุดิบ และเลือก Discard Fodder
3. **`ThreatEvaluator` (`IDeckThreatEvaluator`)**: ประเมิน Threat เฉพาะทางที่เป็นตัวเคาน์เตอร์เด็คเรา
4. *(เสริม)* **`ResourceEvaluator`** หรือ **`ScaleResolver`**: ติดตั้งเพิ่มเติมเมื่อเด็คนั้นมีระบบ Counter หรือ Pendulum Scale

> 💡 **ตัวอย่างการใช้งานจริงใน Codebase**:
> - ดูการใช้งานจริงได้ใน [_2026_KwtuneExecutor.cs](file:///C:/Users/admin/Documents/EdoGame/src/YGO_SOURCE_CLEAN/windbot-fork/Game/AI/Decks/_2026_KwtuneExecutor.cs) (`KewlTunePlugin`)
> - ดูการใช้งานจริงได้ใน [_2026_GraveExecutor.cs](file:///C:/Users/admin/Documents/EdoGame/src/YGO_SOURCE_CLEAN/windbot-fork/Game/AI/Decks/_2026_GraveExecutor.cs) (`GravePlugin`)


---

## 5. Blueprint & Case Study: Endymion Spell Counter Control

พิมพ์เขียวสถาปัตยกรรมสำหรับเด็คที่มีระบบทรัพยากรต่อเนื่อง (Counter Economy):

### 5.1 Spell Counter Economy System
เคาน์เตอร์ไม่ใช่แค่ต้นทุน (Cost) แต่คือกระสุนในการคุมเกม:
```csharp
public enum CounterLevel
{
    Critical = 0, // 0-1 เม็ด: สภาวะวิกฤต ห้ามใช้เคาน์เตอร์กับงานไม่จำเป็น
    Low = 1,      // 2-3 เม็ด: พอสำหรับ Negate สำคัญ 1 ครั้ง (Jackal King)
    Ready = 2,    // 4-5 เม็ด: พร้อมรับมือทั้ง Negate และเปิดใช้คอมโบ
    ComboReady = 3, // 6-7 เม็ด: พร้อมใช้ Mighty Master ระเบิดบอร์ด
    Surplus = 4   // 8+ เม็ด: ทรัพยากรล้นเหลือ สามารถขยายบอร์ดได้เต็มกำลัง
}
```

### 5.2 กฎการบริหารทรัพยากรเฉพาะการ์ด (Key Card Execution Rules)
1. **Mythical Beast Jackal King (2 Counters = Monster Negate 1 ครั้ง)**:
   - ตรวจสอบ Threat Level ของเอฟเฟกต์มอนสเตอร์คู่แข่ง:
     `Boss Effect > Search/Starter > Removal > Extender > Minor Effect`
   - ห้ามใช้ Jackal King กับการ์ดขยะของคู่แข่งถ้าบอสหรือ Starter ของคู่แข่งยังไม่ออก
   - รักษากระสุนสำรอง 2 เม็ดบนสนามเสมอเมื่อมี Jackal King
2. **Endymion, the Mighty Master of Magic**:
   - **Spell/Trap Quick Negate & Intelligent Bounce Recycling**:
     เมื่อต้องคืนการ์ดที่มีเคาน์เตอร์ขึ้นมือเพื่อ Negate ให้เรียงลำดับการเลือกคืน:
     1. การ์ดที่นำกลับมาใช้ใหม่ได้ในเทิร์นเรา (เช่น Servant ที่ใช้เอฟเฟกต์ไปแล้ว)
     2. การ์ดที่หมดหน้าที่แล้ว
     3. การ์ดที่มีเคาน์เตอร์สะสมสูง (เพื่อโอนเคาน์เตอร์เข้า Mighty Master)
     4. ห้ามคืน Key Card ที่ทำให้โครงสร้างคอมโบพัง
   - **6-Counter Board Wipe**: ใช้ระเบิดสนามเมื่อเคลียร์ Threat ของคู่แข่งได้ $\ge 2$ ใบ หรือเปิดทางทำ Lethal
3. **Servant of Endymion (3 Counters Summon Gate)**:
   - ใช้ 3 เคาน์เตอร์ Special Summon ตัวเอง + Jackal King จากเด็ค **เฉพาะเมื่อการเรียกนั้นเปิดคอมโบหรือสร้างบอร์ดต่อได้** ไม่เรียกเล่นๆ เพียงเพื่อเพิ่มจำนวนตัว
4. **Net Gain Arithmetic**:
   - ทุกครั้งที่ร่าย Spell ให้คำนวณการเติบโตสุทธิของเคาน์เตอร์:
     `NetGain = NewCountersGenerated - CountersSpent`
   - สนับสนุน Action ที่ทำให้ NetGain เป็นบวกหรือเท่าทุน
5. **Counter Emergency Breach Policy**:
   - อนุญาตให้ใช้เคาน์เตอร์จนต่ำกว่าเกณฑ์สำรอง (Reserve) ได้เฉพาะกรณีฉุกเฉิน:
     - ฝ่ายตรงข้ามกำลังจะทำ Lethal
     - ฝ่ายตรงข้ามกำลังจะตั้ง Floodgate ถาวร
     - ฝ่ายตรงข้ามกำลัง Resolve Boss Effect รุนแรง

### 5.3 Dynamic Action Scorer Formula
```text
ActionScore = 
    BoardImpact           (ผลกระทบต่อสนาม)
  + CounterNetGain        (เคาน์เตอร์ที่เพิ่มขึ้นสุทธิ)
  + NegateValue           (มูลค่าการสกัดกั้น)
  + FutureUtility         (ประโยชน์ในอนาคต)
  + FollowUpValue         (ทรัพยากรเทิร์นถัดไป)
  + OTKValue              (โอกาสปิดเกม)
  - CounterCost           (ต้นทุนเคาน์เตอร์ที่เสียไป)
  - MaterialLoss          (การสูญเสียตัวบนสนาม)
  - OpponentAdvantage     (แต้มต่อที่คู่แข่งอาจได้รับ)
  - RecoveryRisk          (ความเสี่ยงต่อการถูกแก้ทาง)
### 5.4 Case Study: Six Samurai Bushido Economy & Multi-Tier Interruption

พิมพ์เขียวสถาปัตยกรรมสำหรับเด็ค Six Samurai ยุคใหม่ ที่ผสาน Bushido Loop เข้ากับชุดการ์ดขัดจังหวะรอบด้าน:

1. **Bushido Counter Economy & Gateway Loop**:
   - `Gateway of the Six` (หัก 4 เม็ดเสิร์ช/นำกลับ Six Samurai จากเด็คหรือสุสาน)
   - `Battle Shogun of the Six Samurai` (Link-2 ที่เสิร์ช Gateway เมื่อ Link Summon สำเร็จ)
   - การคำนวณ Counter: บันทึกเคาน์เตอร์รวมจากทุกการ์ด (`Gateway`, `Dojo`, `United`) เพื่อรู้ว่ามีกระสุนเสิร์ชวนซ้ำได้กี่ครั้ง
2. **Multi-Tier Interruption & End Board Targets**:
   - **Spell/Trap Negate**: `Legendary Six Samurai - Shi En` (Quick Negate เวท/กับดักเทิร์นละ 1 ครั้ง และสังเวยซามูไรตัวอื่นแทนการถูกทำลาย)
   - **Monster Effect Negate & Board Control**: `Great Shogun Shien` (จำกัดให้คู่แข่งร่าย Spell/Trap ได้เทิร์นละ 1 ใบ) + ซินโคร/ลิงก์ขัดจังหวะสากล เช่น `Baronne de Fleur` / `Apollousa` / `S:P Little Knight`
   - **In-Archetype Quick Trap**: `Six Style - Dual Wield` (เด้งการ์ดคู่แข่ง 2 ใบขึ้นมือเมื่อคุมซามูไรตัวเดียวในสภาพตั้งโจมตี)
   - **Handtrap Defense**: จัดสรรสล็อตสำหรับ `Ash Blossom`, `Maxx "C"`, `Called by the Grave`, `Crossout Designator` เพื่อป้องกัน Nibiru/Droll ที่จะตัดลูป Bushido
3. **Kizaru Multi-Attribute Resolution**:
   - `Secret Six Samurai - Kizaru`: เมื่อ Special Summon ต้องตรวจสอบ Attribute ของ Six Samurai บนสนาม แล้วเลือกเสิร์ชการ์ดที่มี Attribute แตกต่างกันขึ้นมืออย่างแม่นยำ

---

## 6. Audited OCGCore Hint Constants Table

ตาราง Hint Constants ที่ได้รับการตรวจสอบตรงตามเอนจิน `OCGCore` และพฤติกรรม AI:

| Hint ID | Constant Name | Meaning & AI Behavior |
|---|---|---|
| **500** | `HINTMSG_RELEASE` | สังเวย/บูชายัญ: เลือก Token หรือ Fodder ก่อน ห้ามสังเวย Ace Boss |
| **501** | `HINTMSG_DISCARD` | ทิ้งการ์ด: ทิ้งการ์ดที่มีผลในสุสาน หรือการ์ดซ้ำ ปกป้อง Combo Starters |
| **502** | `HINTMSG_DESTROY` | ทำลาย: บังคับเลือกการ์ดฝ่ายตรงข้าม (`c.Controller == 1`) ตามลำดับ RemovalScore |
| **503** | `HINTMSG_REMOVE` | รีมูฟ/แบน: บังคับเลือกการ์ดฝ่ายตรงข้าม (`c.Controller == 1`) ข้ามตัวที่มี Target Immunity |
| **504** | `HINTMSG_TOGRAVE` | ส่งลงสุสาน: ส่งชิ้นส่วนคอมโบของเรา หรือส่งการ์ดของคู่แข่งลงสุสาน |
| **505** | `HINTMSG_RTOHAND` | เด้งขึ้นมือ (Bounce): บังคับเล็งการ์ดตัวปัญหาของศัตรู (`c.Controller == 1`) |
| **506** | `HINTMSG_ATOHAND` | ค้นหาจากเด็คขึ้นมือ (Search): เลือก Ace, Handtrap, หรือ Starter ตัวสำคัญก่อน |
| **507** | `HINTMSG_TODECK` | สับกลับเข้าเด็ค (Spin): กำจัดตัวปัญหาหลักของคู่แข่ง |
| **508** | `HINTMSG_SUMMON` | อัญเชิญปกติ: เลือกลำดับ Starter หรือตัวเปิดเกม |
| **509** | `HINTMSG_SPSUMMON` | อัญเชิญพิเศษ: เลือก Boss/Extender ที่เหมาะสมตามรูท |
| **511** | `HINTMSG_FMATERIAL` | วัตถุดิบ Fusion: ป้องกัน Ace บนสนาม เลือกลำดับ Fodder |
| **512** | `HINTMSG_SMATERIAL` | วัตถุดิบ Synchro: ปกป้องบอส เรียงจาก Tuner/Non-Tuner ตัวเล็กขึ้นไป |
| **513** | `HINTMSG_XMATERIAL` | วัตถุดิบ Xyz: ปลดหรือเลือกวัตถุดิบที่ไม่ใช่บอสหลัก |
| **514** | `HINTMSG_FACEUP` | เลือกการ์ดหงายหน้า |
| **518** | `HINTMSG_EQUIP` | เลือกการ์ดที่จะสวมใส่ |
| **520** | `HINTMSG_CONTROL` | เปลี่ยนการควบคุม/ขโมยการ์ด |
| **528** | `HINTMSG_POSCHANGE` | ปรับเปลี่ยนรูปแบบการตั้งการ์ด |
| **533** | `HINTMSG_LMATERIAL` | วัตถุดิบ Link: เลือกลำดับตัวโทเคนหรือตัวหมดประโยชน์ ห้ามใช้บอสหลัก |
| **551** | `HINTMSG_TARGET` | เล็งเป้าหมายผลการ์ด |
| **571** | `HINTMSG_TOZONE` | ย้ายโซนการ์ด |
| **572** | `HINTMSG_COUNTER` | วางหรือนำ Spell/Bushido Counter ออก |
| **575** | `HINTMSG_NEGATE` | ขัดขวาง/Negate ผลการ์ดของฝ่ายตรงข้าม |

> ⚠️ **ข้อควรระวังสำคัญยิ่ง (HINT AUDIT NOTE)**:
> ในเอนจิน OCGCore / constant.lua ปัจจุบัน **ไม่มี `HINTMSG_TOHAND = 573`** เด็ดขาด! การค้นหาการ์ดขึ้นมือใช้ **`506 (HINTMSG_ATOHAND)`** เท่านั้น และรหัส **`572 คือ HINTMSG_COUNTER`** ส่วน **`575 คือ HINTMSG_NEGATE`** อย่าจำสับสนเด็ดขาด

---

## 7. Central Core Universal Heuristics & Safeguards

ทุก Executor จะได้รับความคุ้มครองจากกลไกกลางของ `ExecutorBase`:

1. **Master Rule 5 EMZ Preservation**:
   - จำกัด Extra Monster Zone (`0x20`) ไว้ให้ Link Monsters และ Pendulum หงายหน้าจาก Extra Deck เท่านั้น
   - Fusion, Synchro, Xyz บังคับลง Main Monster Zones เพื่อไม่ให้ขวาง Link Arrows
2. **Universal Bagooska Defense Safeguard**:
   - `Number 41: Bagooska` บังคับลงในสภาพ **FaceUpDefence** เสมอ เพื่อให้ฟลัดเกตทำงานต่อเนื่อง
3. **Universal Duplicate Handtrap & Negate Prevention**:
   - ป้องกันการเปิดใช้ Handtrap หรือ Negate ซ้อนกันในเชนเดียวกัน (เช่น ห้ามโยน Ash ซ้อน Ash)
4. **Lethal & Direct Attack Prioritization**:
   - เมื่อพลังโจมตีถึง LP คู่แข่ง และโจมตีตรงได้ บอทจะสั่งโจมตีตรงเพื่อปิดเกมทันที
5. **Active Intervention Guard (`HeuristicGuard.SanitizeSelection`)**:
   - หากคำสั่งเลือกเป้าหมายจะทำลายหรือขจัดพวกเดียวกันเอง ระบบ Guard จะสลับเป้าไปยังการ์ดของศัตรูทันที การันตี **0 Self-Harm Violations**
6. **Hostile Opponent Prompt Safeguard**:
   - ใน `OnSelectEffectYn`: หากเป็นการ์ดของคู่แข่ง (`card.Controller == 1`) จะตอบปฏิเสธ (`false`) เป็นค่าเริ่มต้น

---

## 8. Build & Deploy Pipeline

```powershell
cd C:\Users\admin\Documents\EdoGame\src\YGO_SOURCE_CLEAN
powershell -ExecutionPolicy Bypass -File .\BUILD_AND_DEPLOY.ps1
```

**Exclusive Deployment Target (STRICT RULE)**:
- Deploy ไบนารีชุดใหม่มาที่ **`C:\Users\admin\Documents\EdoGame\`** เท่านั้น
- ห้าม Deploy ไปยังโฟลเดอร์อื่นโดยเด็ดขาด

---

## 9. Progress Log & Archiving Policy

- **ความกระชับของ PROGRESS.md**:
  - ไฟล์ [PROGRESS.md](file:///C:/Users/admin/Documents/EdoGame/PROGRESS.md) ต้องรักษาขนาดให้อยู่ในช่วง **~200–400 บรรทัด** เสมอ (เก็บเฉพาะ 5–10 รายการล่าสุด) เพื่อให้อ่านได้สมบูรณ์ใน 1 Tool Call
- **เกณฑ์การแยก Archive**:
  - เมื่อเกิน **~500–800 บรรทัด** ให้ตัดประวัติชุดเก่าไปบันทึกต่อท้ายไว้ใน [Docs/PROGRESS_ARCHIVE.md](file:///C:/Users/admin/Documents/EdoGame/Docs/PROGRESS_ARCHIVE.md) ทันที

---

## 10. Senior Game AI Engineer & Deck Architect 3-Step Protocol (MANDATORY STANDARD)

เมื่อได้รับมอบหมายให้วิเคราะห์, ปรับปรุงเด็ค, หรือเขียน AI Executor สำหรับ WindBot ให้ปฏิบัติตามมาตรฐาน 3 ขั้นตอนนี้เสมอ:

### ขั้นตอนที่ 1: วิเคราะห์เปรียบเทียบ (Human vs Bot Logic) & Deck Optimization
1. **คัดกรองการ์ดที่ไม่เหมาะกับบอท (Identify High-Cognitive Traps)**:
   - **Chokepoint Handtraps ลึกๆ**: การ์ดที่ต้องอาศัยการอ่านเกมล่วงหน้าสูง หรือมีเงื่อนไขการใช้แคบมาก
   - **Trade-off ทำลายการ์ดตัวเองโดยไม่มี Check ชัดเจน**: เช่น การ์ดที่ทำลายการ์ดบนสนามฝั่งเราเพื่อส่งผล หากไม่มี Guard ตรวจสอบความปลอดภัย บอทจะเผลอทำลายบอสหรือสเกลสำคัญของตัวเอง
   - **คอมโบที่แตกกิ่งก้านสาขามากเกินไป (Branching Explosion)**: คอมโบที่มีเงื่อนไขข้าม Phase หรือพึ่งพา Action ของคู่แข่งในเทิร์นฝ่ายตรงข้าม (เช่น Dagda + Scythe + Halq + Wonder Magician) ซึ่งเสี่ยงต่อการผิดพลาดของ Chain Priority
   - **Self-Harm / Resource Depletion**: การ์ดจ่าย LP ก้อนโตหรือทิ้งการ์ดหมดมือ (Allure of Darkness เมื่อไม่มี DARK สำรอง)
2. **การปรับ Decklist (Bot-Friendly Tuning)**:
   - **Deterministic & High-Value Replacements**: แทนที่ด้วยการ์ดที่มีเงื่อนไขชัดเจน 100% เช่น Counter Trap (Solemn Series), Non-Target Removal, Omni-Negate Bosses (Vortex Dragon, Baronne, Crystal Wing), หรือ Floodgate ที่ไม่ขัดคอมโบตัวเอง (Abyss Dweller, Bagooska)
   - **Linear & Consistent Engines**: มุ่งเน้น Route A / Route B ที่เดินคอมโบเป็นเส้นตรง บอร์ดจบมีความสม่ำเสมอสูง
   - **Anti-Brick Optimization**: ปรับสัดส่วน Starters (อย่างน้อย 9-12 ใบ), Extenders, และ Interruption Density (9-15 ใบสำหรับเด็ค 40 ใบ) ลดการใส่การ์ดที่ต้องรอการ์ดอื่นเพื่อคอมโบเพียงอย่างเดียว
3. **สรุป Decklist ฉบับปรับปรุง**:
   - ระบุ Card ID, ชื่อการ์ด, และจำนวนใบอย่างเป็นทางการในรูปแบบตารางและ `.ydk` format

### ขั้นตอนที่ 2: วางโครงสร้างตรรกะการเล่น (Playstyle & Decision Hierarchy)
อธิบายลำดับความสำคัญก่อนเขียนโค้ด:
1. **ลำดับการเล่น Going First vs Going Second**:
   - **Going First**: เน้นตั้งเสา Omni-Negate, Quick Disruption, และ Search Resources เพื่อเทิร์น 3
   - **Going Second**: เน้น Board Breaking (Harpie, Raigeki, Dark Rebellion, Clear Wing) เคลียร์ Threat ก่อนเดิน Engine เพื่อปิดเกม (Lethal Check)
2. **เงื่อนไขความปลอดภัย (Safety Safeguards)**:
   - **Lethal Check**: เมื่อดาเมจบนสนามรวมกันชนะ LP ศัตรูได้ และทางโล่ง ให้มุ่งหน้าสู่ Battle Phase ทันที ห้าม Overextend
   - **Field Spell / Continuous Protection**: ห้ามเปิดร่าย Field Spell หรือ Continuous Card ใบใหม่ทับใบเดิมที่มีประโยชน์อยู่แล้ว
   - **Chaining Guard (1 Negate = 1 Problem)**: เช็ค `IsChainAlreadyNeutralized()` เสมอ ห้ามโซ่ขัดจังหวะทับการ์ดพวกเดียวกันเอง และห้ามโซ่ Negate ซ้อนกันหลายใบใส่ Action เดียวของศัตรู
   - **Cost & LP Guard**: เช็ค LP และมือสำรองก่อนจ่ายเสมอ
3. **เกณฑ์การเลือกเป้าหมาย (Card Selector Priority)**:
   - **Targeting Hierarchy**: เล็งทำลาย Threat สูงสุดของศัตรู (Continuous Floodgate -> Negator -> Monster ATK สูงสุด)
   - **Target Verification**: เอฟเฟกต์ Ignition Removal ต้องตรวจว่ามีเป้าหมายของฝ่ายตรงข้ามอยู่จริงก่อน `return true` เพื่อป้องกันการระเบิดพวกเดียวกันเอง

### ขั้นตอนที่ 3: C# Executor Code Implementation
เขียนโค้ดตามมาตรฐาน Decoupled Domain Architecture ให้สมบูรณ์ 100%:
1. **CardId Constants**: ระบุ Card ID จาก `cards.cdb` อย่างครบถ้วนใน `public static class CardId`
2. **Initialize() / Pipeline Registration**:
   - ติดตั้ง `DeckPlugin = new XxxPlugin(this);`
   - กำหนด `AddExecutor` ตามระดับความสำคัญ (Tier 0: Negates -> Tier 1: Board Breakers -> Tier 2: Starters -> Tier 3: Searchers -> Tier 4: Climbs -> Tier 5: S/T Set)
3. **Callback Implementations**:
   - `OnSelectCard()`: แยกตาม Hint ID (506 Search -> Strategy, 509 SpSummon -> Strategy, 501 Discard -> MaterialEvaluator, 502/503 Removal -> Enemy Target / Self-pop Resolver)
   - `OnSelectOption()` & `OnSelectEffectYn()`: ปฏิเสธเอฟเฟกต์ของศัตรู (`card.Controller == 1 => false`) และเปิดรับเอฟเฟกต์ที่เป็นประโยชน์ของฝั่งเรา
   - `OnSelectPosition()`: มอนสเตอร์ ATK >= 1800 ตั้งโจมตี, มอนสเตอร์ 0 ATK / Handtraps / Bagooska บังคับตั้งป้องกัน
4. **Build, Deploy & Verify**:
   - Deploy ไปยัง `C:\Users\admin\Documents\EdoGame\` ผ่าน `BUILD_AND_DEPLOY.ps1`
   - บันทึกการเปลี่ยนแปลงใน `PROGRESS.md`
   - รอคำสั่ง "Text Duel" ก่อนรัน Headless Simulation เสมอ