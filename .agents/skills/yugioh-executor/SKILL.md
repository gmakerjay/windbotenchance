---
name: yugioh-executor
description: |
  คู่มือพัฒนา, สร้าง Combo, และ Deploy YugiohTH Rule-Based WindBot Executor
  ครอบคลุมโครงสร้างโปรเจกต์, สถาปัตยกรรม Central Core & ModernExecutor,
  ขั้นตอน Build & Deploy, และหลักการตัดสินใจเชิงกลยุทธ์ (Strategic Decision System)
---

# YugiohTH Executor Development Skill & Strategic Decision System v10.0

## 0. กฎเหล็ก: ห้ามมั่ว ต้องอ้างอิงของจริงเสมอ

**ก่อนแก้โค้ดหรือออกแบบคอมโบใดๆ ห้ามเดาเด็ดขาด** ต้องตรวจสอบจากแหล่งจริงทุกครั้ง:

| สิ่งที่ต้องตรวจ | แหล่งอ้างอิง | เหตุผล |
|---|---|---|
| Card ID, ชื่อ, Effect Text | `cards.cdb` | ข้อมูลการ์ดจริง ห้ามเดา Card ID หรือ Effect Text จากความจำ |
| การ์ดในเด็ค, สัดส่วน | ไฟล์ `.ydk` ของเด็คนั้นๆ | เพื่อรู้ resource จริงที่มีในมือ/เด็ค ไม่ใช่ resource ที่คิดว่าน่าจะมี |
| ฟังก์ชัน/Method/API ที่จะเรียกใช้ | โค้ดจริงใน `ExecutorBase/`, `Game/AI/Decks/` | ห้ามสมมติว่ามี method อยู่ — เปิดไฟล์อ่าน signature จริงก่อนเรียกใช้ทุกครั้ง |
| Threat / Chokepoint Targets | `CardIntelligence.cs` | ฐานข้อมูลกลาง O(1) ของ Floodgate, Negator, Handtrap, Immunity ห้ามฮาร์ดโค้ดซ้ำ |
| Hint ID ของ Effect | OCGCore constants & `ModernExecutor.cs` | Hint ผิดตัวทำให้เลือกการ์ดผิดพลาด |

---

## 1. Workflow เมื่อได้รับคำสั่ง "สร้างเด็คใหม่" / "แก้ไขเด็ค" / "ปรับปรุง Executor"

1. **ศึกษาเด็คจากของจริง**: อ่าน `.ydk` + `cards.cdb` ของเด็คเป้าหมายทั้งหมด ก่อนออกแบบใดๆ
2. **ออกแบบคอมโบตามหลักการเชิงกลยุทธ์**: Main Route + แผนสำรอง (Route B/C/D), First Turn / Second Turn
3. **เขียนโค้ด ModernExecutor และลงทะเบียนเด็ค**: สร้าง `.ydk`, เขียน `Executor.cs`, ลงทะเบียนใน `bots.json`
4. **Build & Deploy ไปยังเป้าหมาย**: คอมไพล์ผ่าน `BUILD_AND_DEPLOY.ps1` และ Deploy มาที่ `C:\Users\admin\Documents\EdoGame\` เสมอ และบันทึกประวัติลงใน `PROGRESS.md`
5. **นโยบายการทดสอบ Headless Simulation (กฎเหล็ก)**:
   - **ห้ามรันการจำลองดวล Headless Simulator โดยอัตโนมัติ** หลังสร้างหรือแก้ไขเด็คเสร็จ
   - **จะทำการรัน Headless Text Duel ได้ก็ต่อเมื่อผู้ใช้สั่ง "Text Duel" (หรือ "จำลองดวล") เท่านั้น** เนื่องจากผู้ใช้ต้องการทดสอบการเล่นด้วยตนเองก่อนเสมอ
   - เมื่อผู้ใช้สั่ง "Text Duel" เท่านั้น จึงทำการรันดวลทดสอบกับคู่ซ้อม Legacy 4 เด็ค: `ABC`, `Altergeist`, `BlueEyes`, `DarkMagician`
     ```powershell
     dotnet run --project src\YGO_SOURCE_CLEAN\Client_Headless_Fortest\Client_Headless_Fortest.csproj -c Release -- --deck <DECK_NAME> --opponent <ABC|Altergeist|BlueEyes|DarkMagician> --games 10 --timeout 60
     ```
   - สรุปผลสถิติ Win Rate % และ Violations ให้ผู้ใช้ทราบ

---

## 2. Strategic Decision System (หลักการตัดสินใจเชิงกลยุทธ์)

> **"Do not play cards. Play the game state."**  
> เป้าหมายสูงสุดคือ: **Bot ที่ไม่ได้แค่รู้ว่าการ์ดทำอะไรได้ แต่รู้ว่าเมื่อไหร่ควรทำ และเมื่อไหร่ไม่ควรทำ** โดยเลือก Action ที่ให้ **Expected Value สูงสุด** ไม่ใช่คอมโบที่ยาวที่สุด

### 2.1 Core Decision Loop
ทุกการกระทำต้องผ่านกระบวนการประเมินสถานการณ์เสมอ:
```text
OBSERVE (Board/Hand/GY/LP) → IDENTIFY THREATS → CHECK WIN/DEFENSE → SCORE ACTIONS → EXECUTE → PRESERVE FOLLOW-UP
```

### 2.2 Threat Priority Model (ลำดับความสำคัญของภัยคุกคาม)
1. **Priority 1 — Immediate Lethal Threat**: การ์ดหรือสถานการณ์ที่ทำให้เราแพ้ในเทิร์นนี้ (ต้องเคลียร์ก่อนเสมอ)
2. **Priority 2 — Hard Interaction**: Omni Negate, Monster Negate, S/T Negate, Continuous Floodgate, Turn-skip lock
3. **Priority 3 — Resource Engine**: การ์ดค้นหา (Search), จั่ว (Draw), ชุบ (Revive), หรือสร้าง Token ของคู่แข่ง
4. **Priority 4 — Board Pressure**: มอนสเตอร์ ATK สูงแต่ไม่มี Interaction ขัดจังหวะ

### 2.3 Strategic Rules of Engagement (กฎทองคำ 10 ประการ)
1. **Can Activate ≠ Should Activate**: อย่าใช้เอฟเฟกต์หรืออัญเชิญเพียงเพราะ "ทำได้" ให้ถามตัวเองเสมอว่าทำแล้วได้อะไร เสียอะไร และคุ้มค่าหรือไม่
2. **Anti-Overextend & Win Condition First**: ถ้าเข้าเงื่อนไขชนะแล้ว (เช่น เข้า RUSH MODE / Lethal Confirmed หรือคู่แข่งไม่มี Interaction ขัดขวาง) **ให้หยุดใช้ทรัพยากรทันทีและสั่งเข้า Battle Phase ปิดเกม** ห้ามรันคอมโบเสิร์ชหรือสเปเชียลต่อให้ยืดเยื้อ
3. **High Material Cost Awareness**: ห้ามนำมอนสเตอร์มูลค่าสูง (Ace Boss, ตัวขัดจังหวะ Quick Effect, Floodgate, หรือตัวที่มีพลังโจมตีสูง) ไปเป็น Material หรือบูชายัญโดยไร้เหตุผลเด็ดขาด
4. **Chain Discipline & Negate Value**: อย่า Chain การ์ดตัวเองโดยไม่เพิ่มคุณค่า และอย่าใช้ Negate/Disruption สำคัญกับการ์ดขยะของคู่แข่ง (เก็บไว้ขัด Chokepoint)
5. **MST & Removal Discipline**: ทำลายเฉพาะการ์ดที่ **ต้องคงอยู่บนสนามเพื่อส่งผล** (`Continuous`, `Field`, `Equip`, `Pendulum Scale`) **ห้ามโซ่ทำลายใส่ Normal Spell / Normal Trap เด็ดขาด** เพราะการทำลายไม่ได้ Negate ผลการ์ด
6. **Targeting Sanity**: ห้ามเพิ่มพลัง/บัฟให้มอนสเตอร์ของคู่แข่ง และเมื่อใช้เอฟเฟกต์ทำลาย/รีมูฟ/ส่งลงสุสาน **ต้องเลือกการ์ดของคู่แข่ง (`c.Controller == 1`) เสมอ**
7. **Preserve Follow-up**: รักษา Resource สำหรับเทิร์นถัดไปเสมอ บอร์ดที่แข็งแกร่งแต่ไม่เหลือการ์ดบนมือเลย ด้อยกว่าบอร์ดที่แข็งแกร่งและมี Starter สำหรับเทิร์นหน้า
8. **Anti-Hoarding vs Anti-Brick**: ใช้การ์ดทันทีเมื่อ Current Value > Expected Future Value อย่ากั๊กการ์ดจนเสียโอกาส และอย่าทิ้งการ์ดอเนกประสงค์ไปกับหน้าที่เล็กๆ
9. **Risk-Aware Simulation**: ประเมินความเสี่ยงของการโดน Handtrap หรือ Board Breaker เสมอ และเลือก Combo Line ที่มี Recovery รองรับ
10. **Information As Resource**: จดจำการ์ดที่คู่แข่งเสิร์ช/เปิดเผย รวมถึง Once-Per-Turn ที่คู่แข่งใช้ไปแล้ว เพื่อวางแผนดักทาง

---

## 3. ข้อห้ามเด็ดขาดในการเขียน Executor (CRITICAL ANTI-PATTERNS)

เพื่อป้องกันบอทเล่นพลาด ทำร้ายตัวเอง หรือเกิดข้อผิดพลาดซ้ำเดิม ให้ปฏิบัติตามข้อห้ามเหล่านี้อย่างเคร่งครัด:

1. 🚫 **ห้ามใช้ `preferred` list ใน `OnSelectCard` โดยไม่แยกแยะ Hint ID**:
   - `preferred` search list ต้องทำงานเฉพาะคำสั่งค้นหาขึ้นมือจากเด็ค (`hint == 506` / `HINTMSG_ATOHAND`) เท่านั้น
   - เมื่อ Hint เป็นคำสั่งขจัด/ทำลาย/ส่งลงสุสาน (`hint == 503 [REMOVE]`, `hint == 502 [DESTROY]`, `hint == 504/508 [TOGRAVE]`): **ต้องบังคับเลือกเฉพาะการ์ดฝ่ายตรงข้าม (`c.Controller == 1`) เสมอ** ห้ามเลือกการ์ดฝั่งเราเด็ดขาดถ้ายังมีการ์ดศัตรูให้เลือก ป้องกันบอททำลาย/รีมูฟเอซตัวเอง
2. 🚫 **ห้ามเขียน Extra Deck Summon คืนค่า `return true;` แบบไร้เงื่อนไข**:
   - **ห้าม** นำมอนสเตอร์ที่สวมใส่การ์ดขโมย (เช่น `Comic Hand`, `Snatch Steal`) ไปทำวัตถุดิบ Extra Deck หรือสังเวยเด็ดขาด (จะทำให้การ์ดสวมใส่หลุดลงสุสานฟรี)
   - **ห้าม** สังเวยมอนสเตอร์พลังโจมตีสูง (2000+) ที่มีผลโจมตีตรง (เช่น ใต้ `Toon Kingdom`) ใน Main Phase 1 เพื่อไปทำตัว Extra Deck ที่พลังน้อยกว่า
   - **ห้าม** นำ Boss Monster หลัก 2 ตัวไปทำ Xyz (เช่น Diabellze + Diabell Queen รวมร่างเป็น Dingirsu) จนพลังโจมตีรวมลดฮวบและเสียบอร์ดขัดจังหวะ
3. 🚫 **ห้ามกำหนดเงื่อนไข Tribute Summon ที่เป็นไปไม่ได้**:
   - ห้ามเขียนเงื่อนไขบูชายัญมอนสเตอร์เลเวล 5+ ว่า `Bot.GetMonsterCount() == 0` เด็ดขาด เพราะจะทำให้บอทค้าง/Pass Turn
4. 🚫 **ห้ามเสิร์ชแล้วเลือกการ์ดใบเดิมกลับเข้าเด็คทันที**:
   - ใน `OnSelectCard` เมื่อต้องคืนการ์ดเข้าเด็ค (เช่น `Illusion of Chaos`) ต้องเลือกการ์ดที่ไม่จำเป็น ห้ามคืนการ์ดที่เพิ่งเสิร์ชมา
5. 🚫 **ห้ามสั่ง `SpellSetStrategy` นำ Handtrap ไปเซ็ตหมอบใน Main Phase 1**:
   - แฮนด์แทรปที่ทำงานจากบนมือได้ (เช่น `Dominus Impulse`, `Ash Blossom`, `Ghost Ogre`) ต้องเก็บไว้บนมือเท่านั้น
6. 🚫 **ห้ามรีมูฟหรือทิ้ง Core Boss สำคัญของเด็คอย่างไร้เหตุผล**:
   - ใน `Pot of Prosperity` หรือ Cost ต่างๆ ต้องยกเว้น Core Boss ห้ามนำไปรีมูฟ
7. 🚫 **ห้ามโซ่ทำลายใส่ Normal Spell / Normal Trap ("MST Negates" Fallacy)**:
   - การ์ดทำลายอย่าง `Mystical Space Typhoon` ไม่ได้ Negate ผล ห้ามโซ่ใส่เวทมนตร์ปกติ/กับดักปกติของศัตรูเด็ดขาด
8. 🚫 **ห้ามคอมโบต่อเมื่อเข้าสู่ RUSH MODE / มี Lethal ยืนยันแล้ว**:
   - เมื่อสนามคู่แข่งว่างเปล่า และบอทมีพลังโจมตีปิดเกมได้ ให้เข้า Battle Phase ตีทันที ห้ามรันคอมโบยืดเยื้อ
9. 🚫 **ห้ามอัญเชิญ Tuner 2 ตัวมาติดบนสนามโดยไม่มี Non-Tuner รองรับ**:
   - หลีกเลี่ยงการ Normal Summon มอนสเตอร์ Tuner ซ้ำซ้อน 2 ตัวหากไม่มีตัว Non-Tuner หรือ Xyz รองรับ
10. 🚫 **ห้ามรัน Headless Simulation โดยอัตโนมัติ (กฎเหล็ก)**:
    - รอคำสั่ง "Text Duel" (หรือ "จำลองดวล") จากผู้ใช้เท่านั้น
11. 🚫 **ห้าม Deploy นอกโฟลเดอร์ `C:\Users\admin\Documents\EdoGame\` เด็ดขาด**:
    - ทุกไบนารีต้อง Deploy มาที่ `C:\Users\admin\Documents\EdoGame\` ผ่าน `BUILD_AND_DEPLOY.ps1` เท่านั้น
12. 🚫 **ห้ามใช้ AI Training / Neural Models / RL**:
    - โปรเจกต์นี้เป็น Rule-Based C# Executor 100% ห้ามสร้างโค้ด Neural หรือ AI Training
13. 🚫 **ห้ามใช้คำนำหน้าปีหรือเวอร์ชันในชื่อเด็ค และห้ามใส่ Card ID ผิดหรือการ์ดผิด Banlist**:
    - ตรวจสอบกับ `cards.cdb` และ `0TCG.lflist.conf` / `OCG.lflist.conf` เสมอ เพื่อป้องกัน `ERRMSG_DECKERROR`

---

## 4. โครงสร้างโปรเจกต์ (Project Overview & Paths)

### Source Location (MANDATORY)
```text
C:\Users\admin\Documents\EdoGame\src\YGO_SOURCE_CLEAN\
```

| Component | Path (Relative to `src\YGO_SOURCE_CLEAN`) | Purpose |
|---|---|---|
| **WindBot** | `windbot-fork/WindBot.csproj` | Bot engine (Exe, net10.0) |
| **ExecutorBase** | `windbot-fork/ExecutorBase/` | Base classes & Central Intelligence engine |
| **Executors** | `windbot-fork/Game/AI/Decks/` | Rule-based AI ต่อเด็ค |
| **DashBot** | `dashbot/dashbot.csproj` | WPF Launcher UI (net10.0-windows) |
| **Headless** | `Client_Headless_Fortest/` | Text duel simulation สำหรับทดสอบ AI logic |
| **Core** | `core/core.csproj` | Shared library (EventBus, IPC, Logger) |
| **Decks** | `windbot-fork/Decks/*.ydk` | ไฟล์เด็คสำหรับ AI |
| **Bots Config** | `windbot-fork/bots.json` | การลงทะเบียน Bot |

---

## 5. Central Core AI Architecture (`ExecutorBase`)

WindBot ได้รับการปฏิรูป Core กลางเพื่อลดการฮาร์ดโค้ดรายเด็ค และช่วยให้บอทตัดสินใจได้อย่างชาญฉลาดโดยอัตโนมัติ:

### 5.1 Central Intelligence (`CardIntelligence.cs`)
- ฐานข้อมูลส่วนกลางเก็บข้อมูลการ์ดในรูปแบบ $O(1)$ HashSets:
  - `FloodgateMonsters`, `FloodgateSpellsTraps` (Skill Drain, Winda, Bagooska, Secret Village)
  - `KnownNegators` (Baronne de Fleur, Savage Dragon, Apollousa, Mechaba)
  - `UniversalChokepoints` (เป้าหมายขัดขวางสำคัญของเด็คเมต้าและเลกาซี่)
  - `Handtraps` (Ash Blossom, Impermanence, Ghost Ogre, Droll, Maxx C, Mulcharmy)
  - `TargetImmuneMonsters` & `DestructionImmuneMonsters` (ป้องกันการยิงใส่เป้าที่กันเอฟเฟกต์)

### 5.2 Universal Fallback Engine (`GameAI.cs` & `Executor.cs`)
- **`FallbackSelectCard()`**: ประเมิน Threat/Cost/Utility อัตโนมัติเมื่ออยู่นอกสคริปต์คอมโบ ขจัดปัญหาบอทหยิบ `cards[0]` มั่ว
- **Stat-Aware `OnSelectPosition()`**: เลือกตั้งรับ (FaceUpDefence) อัตโนมัติหากมอนสเตอร์มีพลังป้องกันเหนือกว่าพลังโจมตีอย่างมีนัยสำคัญ
- **Column-Safe `OnSelectPlace()`**: หลีกเลี่ยงการวางการ์ดในคอลัมน์ที่มี Continuous Spell/Trap หรือเสี่ยงต่อ Infinite Impermanence

### 5.3 Comprehensive Hint Table (`ModernExecutor.cs`)
OCGCore Hint Constants ที่ถูกต้อง และพฤติกรรมการตัดสินใจของ AI:
| Hint ID | Constant | Meaning & AI Behavior |
|---|---|---|
| **500** | `HINTMSG_RELEASE` | บูชายัญ: เลือกลำดับ Fodder/Token ก่อน ห้ามสังเวย Ace |
| **501** | `HINTMSG_DISCARD` | ทิ้งการ์ด: ทิ้งใบที่ได้ผลในสุสาน หรือของซ้ำ ป้องกัน Starter |
| **502** | `HINTMSG_DESTROY` | ทำลาย: เล็งเป้า Threat/Floodgate ของศัตรู (`c.Controller == 1`) ตาม ThreatScore |
| **503** | `HINTMSG_REMOVE` | รีมูฟ/แบน: กำจัดตัวอันตรายสูงสุดของศัตรู (`c.Controller == 1`) ข้ามมอนสเตอร์ที่กันการตกเป็นเป้า |
| **504** | `HINTMSG_TOGRAVE` | ส่งลงสุสาน: ส่งชิ้นส่วนคอมโบ/การ์ดทริกเกอร์ หรือส่งการ์ดศัตรูลงสุสาน |
| **505** | `HINTMSG_RTOHAND` | เด้งการ์ดขึ้นมือ: เล็งตัวเอซ/ตัวปัญหาของศัตรู (`c.Controller == 1`) |
| **506** | `HINTMSG_ATOHAND` | ค้นหาจากเด็คขึ้นมือ (Search): เลือก Ace, Handtraps, Chokepoints และการ์ด Starter ก่อนเสมอ |
| **507** | `HINTMSG_TODECK` | สับ/ส่งกลับเด็ค (Spin removal): เล็งเป้าการ์ดสำคัญของศัตรู |
| **509** | `HINTMSG_SPSUMMON` | อัญเชิญพิเศษ: เลือก Ace/Negate/Extra Deck สูงสุด |
| **510** | `HINTMSG_DISCARD` | ทิ้งการ์ดจากมือ |
| **512** | `HINTMSG_FMATERIAL` | วัตถุดิบ Fusion: ป้องกัน Ace บนสนาม เลือกลำดับ Fodder |
| **513** | `HINTMSG_SMATERIAL` | วัตถุดิบ Synchro: ปกป้องบอส เรียงจาก Tuner/ตัวเล็กขึ้นไป |
| **514** | `HINTMSG_XMATERIAL` | วัตถุดิบ Xyz: ปลดหรือเลือกวัตถุดิบที่ไม่ใช่บอสหลัก |
| **515** | `HINTMSG_POSCHANGE` | ปรับสถานะการตั้ง |
| **516** | `HINTMSG_RELEASE` | สังเวย/บูชายัญ |
| **551 / 552** | `HINTMSG_DISABLE` | เล็ง Negate/ขัดขวางการ์ดสำคัญของศัตรู |
| **572 / 575** | `HINTMSG_NEGATE` | เล็ง Negate เอฟเฟกต์การ์ดสำคัญของศัตรู |

### 5.4 Central Core Universal Heuristics & Guard Standards (สถาปัตยกรรมคอร์กลาง)

ทุก Executor ที่พัฒนาขึ้น จะได้รับประโยชน์จากกลไกกลางเหล่านี้โดยอัตโนมัติ:

1. **Master Rule 5 EMZ Preservation (`Executor.cs`)**:
   - การเลือกลง Extra Monster Zone (`0x20`) อัตโนมัติถูกจำกัดไว้ให้เฉพาะ **Link monsters** และหน้าหงาย Pendulum จาก Extra Deck เท่านั้น
   - มอนสเตอร์ Fusion, Synchro, และ Xyz จะถูกนำไปลง Main Monster Zones (MMZ) เพื่อเปิดทางให้ Link คอมโบดำเนินต่อได้โดยไม่ติดขัด
   - หลีกเลี่ยงคอลัมน์ 1 และ 3 เมื่อฝ่ายตรงข้ามมีหรืออาจเรียก `Relinquished Anima`
2. **Universal Bagooska Defense Safeguard (`ModernExecutor.cs`)**:
   - `Number 41: Bagooska the Terribly Tired Tapir` (IDs `90590303, 90590304`) จะถูกบังคับลงสนามใน **FaceUpDefence** เสมอ เพื่อให้เอฟเฟกต์ฟลัดเกตสนามทำงานต่อเนื่อง
   - มอนสเตอร์ Link บังคับ `FaceUpAttack` เสมอ (ไม่สามารถตั้งรับได้)
   - มอนสเตอร์พลังโจมตีต่ำ (Handtraps, มอนสเตอร์ 0 ATK, มอนสเตอร์ที่มี DEF > ATK และ ATK < 1800) จะเลือกลงใน **FaceUpDefence** เพื่อความปลอดภัย
3. **Universal Duplicate Handtrap & Negate Prevention (`GameAI.cs` & `ModernExecutor.cs`)**:
   - ระบบป้องกันบอทเปิดใช้งาน Handtrap หรือ Negate ซ้ำซ้อนในเชนเดียวกัน (เช่น โยน Ash ซ้อน Ash หรือ Maxx "C" ซ้อน Maxx "C")
   - `DefaultMaxxC` และ `DefaultDrollAndLockBird` ติดตามผลการใช้งานผ่าน `resolvedEffectIdList` ป้องกันการเปิดใช้การ์ดใบที่สองในเทิร์นเดียวกัน
4. **Lethal & Archetype Direct Attack Prioritization (`DefaultExecutor.cs`)**:
   - หากมอนสเตอร์สามารถโจมตีตรงได้ และพลังโจมตีถึง LP คู่แข่ง (`attacker.Attack >= Enemy.LifePoints`) AI จะสั่ง **โจมตีตรงเพื่อชนะเกมทันที** โดยไม่เสียเวลาตีมอนสเตอร์ตั้งรับตัวเล็ก
   - มอนสเตอร์สายโจมตีตรงเพื่อทริกเกอร์เอฟเฟกต์ (เช่น `Sky Striker Ace - Hayate` ส่งเวทลงสุสาน หรือมอนสเตอร์สาย Toon) จะเลือกโจมตีตรงเป็นลำดับแรกเมื่อศัตรูไม่มีฟลัดเกต
5. **Active Intervention Guard (`HeuristicGuard.SanitizeSelection`)**:
   - `HeuristicGuard` ตรวจจับและสกัดกั้นคำสั่งเลือกเป้าหมายที่ผิดพลาด หาก Executor สั่งทำลายหรือเนเกตการ์ดฝั่งเรา ระบบจะบังคับสลับเป้าหมายไปที่การ์ดอันตรายสูงสุดของศัตรูจาก `CardIntelligence` ทันที การันตี **0 Self-Harm Violations**
6. **Hostile Opponent Prompt Safeguard (`GameAI.cs`)**:
   - ใน `OnSelectEffectYn`: คำถามกดใช้เอฟเฟกต์ที่อยู่นอกเหนือ Executor หากเป็นการ์ดของฝ่ายตรงข้าม (`card.Controller == 1`) จะ **ตอบปฏิเสธ (false) เป็นค่าเริ่มต้น** ป้องกันการติดกับดักหรือเสียทรัพยากรฟรี
7. **Option Bitshift Standard (`ModernExecutor.cs`)**:
   - การอ่านรหัส Option ของ OCGCore ต้องใช้ `option >> 4` (ไม่ใช่ `>> 20`) เพื่อให้การเลือกโหมดของการ์ด เช่น `Triple Tactics Talent`, `Pot of Prosperity`, `Medius the Pure` ทำงานได้อย่างถูกต้อง
8. **Modern Meta Chokepoints Database (`CardIntelligence.cs`)**:
   - รวบรวม Chokepoints และ Starters ระดับเมต้า: `Bonfire`, `WANTED`, `Snake-Eye Ash`, `Snake-Eyes Poplar`, `Promethean Princess`, `Fiendsmith Engraver`, `Fiendsmith's Tract`, `Fiendsmith's Sequence`, `S:P Little Knight`, และ `Dimension Shifter`

---

## 6. Build & Deploy Pipeline

```powershell
cd C:\Users\admin\Documents\EdoGame\src\YGO_SOURCE_CLEAN
powershell -ExecutionPolicy Bypass -File .\BUILD_AND_DEPLOY.ps1
```

**Exclusive Deployment Target (STRICT RULE)**:
- Deploy มาที่ `C:\Users\admin\Documents\EdoGame\` เท่านั้น
- ห้าม Deploy ไปยังโฟลเดอร์อื่นโดยเด็ดขาด
- ทุกครั้งหลัง Build & Deploy ให้บันทึกการเปลี่ยนแปลงลงใน [PROGRESS.md](file:///C:/Users/admin/Documents/EdoGame/PROGRESS.md)

---

## 7. Progress Log & Archiving Policy (MANDATORY)

- **ความกระชับของ PROGRESS.md**:
  - ไฟล์ `PROGRESS.md` ต้องถูกรักษาขนาดให้อยู่ในช่วง **~200–400 บรรทัด** เสมอ (เก็บเฉพาะ 5–10 รายการล่าสุด) เพื่อให้ AI อ่านได้สมบูรณ์ใน 1 Tool Call และไม่กิน Token Context เกินจำเป็น
- **เกณฑ์การแยก Archive**:
  - หาก `PROGRESS.md` เริ่มเติบโตเกิน **~500–800 บรรทัด** (เพดาน 1 รอบของ `view_file`) ให้ทำการตัดประวัติชุดเก่าไปบันทึกต่อท้ายไว้ใน [Docs/PROGRESS_ARCHIVE.md](file:///C:/Users/admin/Documents/EdoGame/Docs/PROGRESS_ARCHIVE.md) ทันที
  - คงไว้เฉพาะประวัติการอัปเดตล่าสุด และใส่ลิงก์อ้างอิงไปยัง Archive ที่ท้ายไฟล์ `PROGRESS.md`
