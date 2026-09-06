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

## 1. Workflow เมื่อได้รับคำสั่ง "แก้ไขเด็ค" / "ปรับปรุง Executor"

1. **ศึกษาเด็คจากของจริง**: อ่าน `.ydk` + `cards.cdb` ของเด็คเป้าหมายทั้งหมด ก่อนออกแบบใดๆ
2. **ออกแบบคอมโบตามหลักการเชิงกลยุทธ์** (ดูหมวด 2): Main Route + แผนสำรอง (Route B/C/D), First Turn / Second Turn
3. **ทดสอบผ่าน HeadlessSimulator** กับคู่ซ้อม Legacy 4 เด็ค: `ABC`, `Altergeist`, `BlueEyes`, `DarkMagician`
   ```powershell
   dotnet run --project src\YGO_SOURCE_CLEAN\Client_Headless_Fortest\Client_Headless_Fortest.csproj -c Release -- --deck <DECK_NAME> --opponent <ABC|Altergeist|BlueEyes|DarkMagician> --games 10 --timeout 60
   ```
4. **วนลูป Iterative Optimization**: **วิเคราะห์ ➔ แก้ไขโค้ด ➔ Build & Deploy ➔ ทดสอบ Headless ➔ วิเคราะห์ผล ➔ แก้ไขซ้ำ** จนกว่า Win Rate จะดีขึ้นอย่างมีนัยสำคัญ
5. **สรุปผลละเอียด**: Win Rate % แยกรายเด็คคู่ซ้อม + สถิติ Violations (ต้องเป็น 0) + Playbook Strategy
6. **Exclusive Deployment**: Deploy ไบนารีชุดใหม่มาที่ `C:\Users\admin\Documents\EdoGame\` เสมอ และบันทึกประวัติลงใน `PROGRESS.md` และ `Docs/`

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