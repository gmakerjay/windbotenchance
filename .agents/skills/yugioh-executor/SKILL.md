---
name: yugioh-executor
description: |
  คู่มือพัฒนา, สร้าง Combo, และ Deploy YugiohTH Rule-Based WindBot Executor
  ครอบคลุมโครงสร้างโปรเจกต์, สถาปัตยกรรม Central Core & ModernExecutor,
  ขั้นตอน Build & Deploy, และหลักการออกแบบ AI เชิงกลยุทธ์
---

# YugiohTH Executor Development Skill v9.0

## 0. กฎเหล็ก: ห้ามมั่ว ต้องอ้างอิงของจริงเสมอ

**ก่อนแก้โค้ดหรือออกแบบคอมโบใดๆ ห้ามเดาเด็ดขาด** ต้องตรวจสอบจากแหล่งจริงทุกครั้ง:

| สิ่งที่ต้องตรวจ | แหล่งอ้างอิง | เหตุผล |
|---|---|---|
| Card ID, ชื่อ, Effect Text | `cards.cdb` | ข้อมูลการ์ดจริง ห้ามเดา Card ID หรือ Effect Text จากความจำ |
| การ์ดในเด็ค, สัดส่วน | ไฟล์ `.ydk` ของเด็คนั้นๆ | เพื่อรู้ resource จริงที่มีในมือ/เด็ค ไม่ใช่ resource ที่คิดว่าน่าจะมี |
| ฟังก์ชัน/Method/API ที่จะเรียกใช้ | โค้ดจริงใน `ExecutorBase/`, `Game/AI/Decks/` | ห้ามสมมติว่ามี method อยู่ — เปิดไฟล์อ่าน signature จริงก่อนเรียกใช้ทุกครั้ง |
| Threat / Chokepoint Targets | `CardIntelligence.cs` | ฐานข้อมูลกลาง O(1) ของ Floodgate, Negator, Handtrap, Immunity ห้ามฮาร์ดโค้ดซ้ำ |
| Hint ID ของ Effect | OCGCore constants & `ModernExecutor.cs` | Hint ผิดตัวทำให้เลือกการ์ดผิดพลาด |

**ขั้นตอนก่อนลงมือทุกครั้ง**: อ่านไฟล์ที่เกี่ยวข้องจริง → ยืนยัน Card ID/Effect/Method Signature ตรงกับที่จะใช้ → ถ้าไม่แน่ใจให้เปิดไฟล์เช็คซ้ำ ห้ามเขียนโค้ดที่อ้างอิง field/method ที่ยังไม่ได้ยืนยันว่ามีอยู่จริง

---

## 1. Workflow เมื่อได้รับคำสั่ง "สร้างเด็คใหม่" / "แก้ไขเด็ค" / "ปรับปรุง Executor"

1. **ศึกษาเด็คจากของจริง**: อ่าน `.ydk` + `cards.cdb` ของเด็คเป้าหมายทั้งหมด ก่อนออกแบบใดๆ
2. **ออกแบบคอมโบตามหลักการเชิงกลยุทธ์** (ดูหมวด 2): Main Route + แผนสำรอง (Route B/C/D), First Turn / Second Turn
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

## 2. หลักการออกแบบคอมโบ (Core AI Principles)

Agent ต้องคิดแบบ **Strategic Executor** ไม่ใช่ Combo Script ท่องจำ:

`GAME STATE → SITUATION → WIN CONDITION → TARGET END BOARD → COMBO LINE → RISK/RESOURCE → ACTION → RE-EVALUATE`

> **Do not play cards. Play the game state.**

ในทุก Combo ที่ออกแบบ ต้องครอบคลุมทั้ง 4 ข้อนี้เสมอ:
- **Main Route + แผนสำรอง**: ทุกคอมโบต้องมี Route หลัก และอย่างน้อย 1 Route สำรอง สำหรับกรณีโดน Handtrap ตัด (`Ash Blossom`, `Infinite Impermanence`, `Maxx "C"`, `Nibiru` ฯลฯ)
- **ใช้การ์ดอย่างคุ้มค่า (Resource Efficiency)**: ไม่ทุ่ม Resource เกินจำเป็น ประเมินว่าคู่ต่อสู้ต้องใช้อะไรตอบโต้ ถ้า Bait ด้วยของถูกได้ให้ Bait ก่อนเปิดของแพง
- **ไม่ทำร้ายตัวเอง (Self-Harm Prevention)**: ก่อนเปิดใช้งานการ์ดใดๆ ตรวจสอบว่าเงื่อนไขไม่ทำลายบอร์ดตัวเอง (เช่น การ์ดที่บังคับเลือกทำลายการ์ดบนสนามเมื่อสนามศัตรูว่าง)
- **อ่านบอร์ดฝ่ายตรงข้าม (Opponent Board Reading)**: ประเมินการ์ดที่มองเห็นได้จริง คาดการณ์ความเสี่ยง และเล็งเป้ากำจัด Chokepoint/Floodgate ตาม `CardIntelligence.cs`

---

## 2.1 ข้อห้ามเด็ดขาดในการเขียน Executor (CRITICAL ANTI-PATTERNS & STRICT PROHIBITIONS)

เพื่อป้องกันไม่ให้บอทเล่นพลาด ทำร้ายตัวเอง หรือเกิดข้อผิดพลาดซ้ำเดิม ให้ Agent ตัวถัดไปปฏิบัติตามข้อห้ามเหล่านี้อย่างเคร่งครัด:

### 🚫 1. ห้ามใช้ `preferred` list ใน `OnSelectCard` โดยไม่แยกแยะ Hint ID (ห้ามทำลาย/รีมูฟการ์ดตัวเอง)
- **ปัญหาที่เคยเกิด**: กำหนด `preferred = { ToonBLS, ToonKingdom, ... }` เพื่อหาการ์ดขึ้นมือ แต่ไม่ได้ตรวจสอบ Hint ID เมื่อการ์ดใช้เอฟเฟกต์ "รีมูฟการ์ด 1 ใบ" (`hint=503`) บอทเห็นการ์ดเอซตัวเองอยู่ในรายชื่อ จึงสั่งรีมูฟบอสตัวเองออกจากสนามซะเอง!
- **ข้อปฏิบัติที่ถูกต้อง**:
  - `preferred` search list ต้องทำงานเฉพาะคำสั่งค้นหาขึ้นมือจากเด็ค (`hint == 506` / `HINTMSG_ATOHAND` หรือเมื่อการ์ดทุกใบมาจากเด็ค `cards.All(c => c.Location == CardLocation.Deck)`) เท่านั้น
  - เมื่อ Hint เป็นคำสั่งขจัด/ทำลาย/ส่งลงสุสาน (`hint == 503 [REMOVE]`, `hint == 502 [DESTROY]`, `hint == 504 [TOGRAVE]`): **ต้องบังคับเลือกเฉพาะการ์ดฝ่ายตรงข้าม (`c.Controller == 1`) เสมอ** ห้ามเลือกการ์ดฝั่งเราเด็ดขาดถ้ายังมีการ์ดศัตรูให้เลือก

### 🚫 2. ห้ามเขียนฟังก์ชัน Extra Deck คืนค่า `return true;` แบบไร้เงื่อนไข
- **ปัญหาที่เคยเกิด**: `SPLittleKnightSpSummon()` คืนค่า `true` ตลอดเวลา บอทจึงนำมอนสเตอร์ที่เพิ่งขโมยมาด้วย `Comic Hand` หรือ Toon พลังสูงใต้ Toon Kingdom ไปทำ Link เป็น S:P Little Knight พลัง 1600 ส่งผลให้การ์ดสวมใส่พังหลุดลงสุสานฟรีๆ และเสียจังหวะโจมตีตรงปิดเกม
- **ข้อปฏิบัติที่ถูกต้อง**:
  - **ห้าม** นำมอนสเตอร์ที่สวมใส่การ์ดขโมย (เช่น `Comic Hand`, `Snatch Steal`) ไปทำวัตถุดิบ Extra Deck (Link/Xyz/Synchro) หรือสังเวยเด็ดขาด
  - **ห้าม** สังเวยมอนสเตอร์พลังโจมตีสูง (2000+) ที่มีผลโจมตีตรง (เช่น ภายใต้ `Toon Kingdom`) ใน Main Phase 1 เพื่อไปทำตัว Extra Deck ที่พลังน้อยกว่าและตีตรงไม่ได้
  - ตัวตั้งรับ/ขัดขวาง (เช่น S:P Little Knight, Big Eye, Hope Harbinger) ควรอัญเชิญใน Main Phase 2 เพื่อตั้งบอร์ดขัดขวาง หรืออัญเชิญเมื่อต้องการขจัดตัวปัญหาของศัตรูเท่านั้น

### 🚫 3. ห้ามกำหนดเงื่อนไข Tribute Summon ที่เป็นไปไม่ได้ (`Bot.GetMonsterCount() == 0`)
- **ปัญหาที่เคยเกิด**: มอนสเตอร์เลเวล 5 ขึ้นไปที่ต้องการ 1-2 บูชายัญ (เช่น Dark Magician Girl, Pharaoh's Servant) ไปใส่เงื่อนไข `return Bot.GetMonsterCount() == 0;` ทำให้เครื่องเกมไม่สามารถเสนอคำสั่งอัญเชิญได้ ส่งผลให้บอทหยุดเล่น (Freeze) / Pass Turn ข้ามเทิร์นไปเฉยๆ
- **ข้อปฏิบัติที่ถูกต้อง**: มอนสเตอร์ที่ต้องบูชายัญ ต้องตรวจสอบว่าบนสนามเรามีมอนสเตอร์ให้บูชายัญ (`Bot.GetMonsterCount() >= 1`) เสมอ

### 🚫 4. ห้ามเสิร์ชแล้วเลือกการ์ดใบเดิมกลับเข้าเด็คทันที
- **ปัญหาที่เคยเกิด**: เอฟเฟกต์ที่เสิร์ชการ์ดขึ้นมือแล้วต้องเลือกการ์ด 1 ใบกลับเด็ค (เช่น `Illusion of Chaos`) ใน `OnSelectCard` ดันเลือกการ์ดที่เพิ่งเสิร์ชมาวางกลับเด็ค
- **ข้อปฏิบัติที่ถูกต้อง**: ใน `OnSelectCard` เมื่อต้องคืนการ์ดเข้าเด็ค ต้องเลือกการ์ดที่ไม่จำเป็นหรือการ์ดขยะ ห้ามคืนการ์ด Starter หรือการ์ดที่เพิ่งหยิบขึ้นมาเด็ดขาด

### 🚫 5. ห้ามสั่ง `SpellSetStrategy` นำ Handtrap ไปเซ็ตหมอบใน Main Phase 1
- **ปัญหาที่เคยเกิด**: แฮนด์แทรปที่ทำงานจากบนมือได้ (เช่น `Dominus Impulse`, `Ash Blossom`) ถูกสั่งให้หมอบลงสนาม ทำให้โดนทำลายฟรีโดยไม่ได้ใช้งาน
- **ข้อปฏิบัติที่ถูกต้อง**: แฮนด์แทรปต้องเก็บไว้บนมือเท่านั้น ยกเว้นกรณีที่เป็น Quick-Play / Normal Trap ที่ต้องเซ็ตเพื่อเปิดใช้ในเทิร์นคู่แข่ง และควรเซ็ตใน Main Phase 2

### 🚫 6. ห้ามรีมูฟหรือทิ้ง Core Piece สำคัญของเด็คอย่างไร้เหตุผล
- **ปัญหาที่เคยเกิด**: เอฟเฟกต์รีมูฟ Extra Deck เพื่อจั่ว (เช่น `Pot of Prosperity`) สุ่มรีมูฟคีย์การ์ดบอสชิ้นเดียวของเด็ค (เช่น Dragoon) ทำให้เด็คหมดทางชนะ
- **ข้อปฏิบัติที่ถูกต้อง**: ต้องระบุลิสต์การ์ดสำรอง (Disposable / Fodder) ให้ชัดเจน และล็อกยกเว้น Core Boss ห้ามนำไปรีมูฟ

### 🚫 7. ห้ามรัน Headless Simulation โดยอัตโนมัติ (กฎเหล็ก)
- ผู้ใช้ต้องการทดสอบการเล่นในเกมจริงด้วยตนเองก่อนเสมอ **ห้ามรันคำสั่งจำลองดวล Headless Simulator เด็ดขาดจนกว่าผู้ใช้จะพิมพ์สั่งคำว่า "Text Duel" (หรือ "จำลองดวล") เท่านั้น**

### 🚫 8. ห้าม Deploy นอกโฟลเดอร์ `C:\Users\admin\Documents\EdoGame\` เด็ดขาด
- ทุกไบนารีต้อง Deploy มาที่ `C:\Users\admin\Documents\EdoGame\` ผ่าน `BUILD_AND_DEPLOY.ps1` เท่านั้น

### 🚫 9. ห้ามใช้ AI Training / Neural Models / RL
- โปรเจกต์นี้เป็น Rule-Based C# Executor 100% ห้ามสร้าง Neural Code หรือโค้ดเทรนโมเดล

---

## 3. Project Overview

YugiohTH เป็น **Rule-Based C# Executor Platform** สำหรับสร้าง AI Bot เล่น Yu-Gi-Oh! ผ่าน EDOPro
โฟกัส 100% ที่ C# ModernExecutor — **ไม่มี AI Training, Neural Network, หรือ RL**

### Source Location (MANDATORY)
```
C:\Users\admin\Documents\EdoGame\src\YGO_SOURCE_CLEAN\
```

### Key Components

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

## 4. Central Core AI Architecture (`ExecutorBase`)

WindBot ได้รับการปฏิรูป Core กลางเพื่อลดการฮาร์ดโค้ดรายเด็ค และช่วยให้บอทตัดสินใจได้อย่างชาญฉลาดโดยอัตโนมัติ:

### 4.1 Central Intelligence (`CardIntelligence.cs`)
- ฐานข้อมูลส่วนกลางเก็บข้อมูลการ์ดในรูปแบบ $O(1)$ HashSets:
  - `FloodgateMonsters`, `FloodgateSpellsTraps` (เช่น Skill Drain, Winda, Bagooska, Secret Village)
  - `KnownNegators` (เช่น Baronne de Fleur, Savage Dragon, Apollousa, Mechaba)
  - `UniversalChokepoints` (เป้าหมายขัดขวางสำคัญของเด็คเมต้าและเลกาซี่)
  - `Handtraps` (Ash Blossom, Impermanence, Ghost Ogre, Droll, Maxx C, Mulcharmy)
  - `TargetImmuneMonsters` & `DestructionImmuneMonsters` (ป้องกันการยิงใส่เป้าที่กันเอฟเฟกต์)

### 4.2 Universal Fallback Engine (`GameAI.cs` & `Executor.cs`)
- **`FallbackSelectCard()`**: ประเมิน Threat/Cost/Utility อัตโนมัติเมื่ออยู่นอกสคริปต์คอมโบ ขจัดปัญหาบอทหยิบ `cards[0]` มั่ว
- **Stat-Aware `OnSelectPosition()`**: เลือกตั้งรับ (FaceUpDefence) อัตโนมัติหากมอนสเตอร์มีพลังป้องกันเหนือกว่าพลังโจมตีอย่างมีนัยสำคัญ ป้องกันมอนสเตอร์คอมโบ/แฮนด์แทรปตกเป็นเป้าตีฟรี
- **Column-Safe `OnSelectPlace()`**: หลีกเลี่ยงการวางการ์ดในคอลัมน์ที่มี Continuous Spell/Trap หรือเสี่ยงต่อ Infinite Impermanence

### 4.3 Comprehensive Hint Table (`ModernExecutor.cs`)
| Hint ID | Constant | Meaning & AI Behavior |
|---|---|---|
| **500** | `HINTMSG_RELEASE` | บูชายัญ: เลือกลำดับ Fodder/Token ก่อน ห้ามสังเวย Ace |
| **501** | `HINTMSG_DISCARD` | ทิ้งการ์ด: ทิ้งใบที่ได้ผลในสุสาน หรือของซ้ำ ป้องกัน Starter |
| **502** | `HINTMSG_DESTROY` | ทำลาย: เล็งเป้า Threat/Floodgate ของศัตรูตาม ThreatScore |
| **504** | `HINTMSG_REMOVE` | แบน: กำจัดตัวอันตรายสูงสุด ข้ามมอนสเตอร์ที่กันการตกเป็นเป้า |
| **505** | `HINTMSG_ATOHAND / RTOHAND` | เสิร์ชขึ้นมือ หรือเด้งการ์ดขึ้นมือศัตรู |
| **506** | `HINTMSG_TODECK` | สับ/ส่งกลับเด็ค (Spin removal) |
| **507** | `HINTMSG_EQUIP` | สวมใส่การ์ด: เล็งเป้าหมายมอนสเตอร์ที่เหมาะสม |
| **508** | `HINTMSG_TOGRAVE` | ส่งลงสุสาน: ส่งชิ้นส่วนคอมโบ/การ์ดทริกเกอร์ |
| **509** | `HINTMSG_SPSUMMON` | อัญเชิญพิเศษ: เลือก Ace/Negate/Extra Deck สูงสุด |
| **518** | `HINTMSG_POSCHANGE` | ปรับสถานะการตั้ง |
| **519** | `HINTMSG_XMATERIAL` | ปลด Xyz Material: ปลดวัตถุดิบที่ใช้แล้ว/มีเอฟเฟกต์ในสุสาน |
| **552 / 572** | `HINTMSG_DISABLE / NEGATE` | เล็ง Negate/ขัดขวางการ์ดสำคัญ |

---

## 5. Build & Deploy Pipeline

```powershell
cd C:\Users\admin\Documents\EdoGame\src\YGO_SOURCE_CLEAN
powershell -ExecutionPolicy Bypass -File .\BUILD_AND_DEPLOY.ps1
```

**Exclusive Deployment Target (STRICT RULE)**:
- Deploy มาที่ `C:\Users\admin\Documents\EdoGame\` เท่านั้น
- ห้าม Deploy ไปยังโฟลเดอร์อื่นโดยเด็ดขาด
- ทุกครั้งหลัง Build & Deploy ให้บันทึกการเปลี่ยนแปลงลงใน [PROGRESS.md](file:///C:/Users/admin/Documents/EdoGame/PROGRESS.md)

---

## 6. Progress Log & Archiving Policy (MANDATORY)

- **ความกระชับของ PROGRESS.md**:
  - ไฟล์ `PROGRESS.md` ต้องถูกรักษาขนาดให้อยู่ในช่วง **~200–400 บรรทัด** เสมอ (เก็บเฉพาะ 5–10 รายการล่าสุด) เพื่อให้ AI อ่านได้สมบูรณ์ใน 1 Tool Call และไม่กิน Token Context เกินจำเป็น
- **เกณฑ์การแยก Archive**:
  - หาก `PROGRESS.md` เริ่มเติบโตเกิน **~500–800 บรรทัด** (เพดาน 1 รอบของ `view_file`) ให้ทำการตัดประวัติชุดเก่าไปบันทึกต่อท้ายไว้ใน [Docs/PROGRESS_ARCHIVE.md](file:///C:/Users/admin/Documents/EdoGame/Docs/PROGRESS_ARCHIVE.md) ทันที
  - คงไว้เฉพาะประวัติการอัปเดตล่าสุด และใส่ลิงก์อ้างอิงไปยัง Archive ที่ท้ายไฟล์ `PROGRESS.md`