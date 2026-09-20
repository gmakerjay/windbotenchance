# YugiohTH Workspace Agent Guidelines

## 📁 Source Code Location (MANDATORY)

**`C:\Users\admin\Documents\EdoGame\src\YGO_SOURCE_CLEAN\`** คือโฟลเดอร์ **Source Code หลัก (src)** ของโปรเจกต์ทั้งหมดสำหรับการพัฒนา, แก้ไข, คอมไพล์ และทดสอบ

```
C:\Users\admin\Documents\EdoGame\src\YGO_SOURCE_CLEAN\
├── windbot-fork/             ← WindBot AI engine & Executors (C# net10.0)
│   ├── Game/AI/Decks/        ← โฟลเดอร์รวม Executor ของแต่ละเด็ค
│   ├── ExecutorBase/         ← Base classes (ModernExecutor, ComboRouter, etc.)
│   ├── Decks/                ← Deck lists (.ydk)
│   └── bots.json             ← การลงทะเบียน Bot
├── dashbot/                  ← DashBot Launcher UI (WPF)
├── core/                     ← Shared core library (IPC, EventBus, Logger)
├── Client_Headless_Fortest/  ← Headless duel simulator (สำหรับทดสอบ AI logic)
└── BUILD_AND_DEPLOY.ps1      ← สคริปต์คอมไพล์และ Deploy กลาง
```

---

## 🎯 Exclusive Deployment Target (STRICT RULE)

ไบนารี่ชุดใหม่ทั้งหมดหลังได้รับการปรับปรุงหรือคอมไพล์ (WindBot.dll, ExecutorBase.dll, core.dll, bots.json, DashBot Launcher, Deck files) จะ **Deploy มาที่โฟลเดอร์นี้เท่านั้น**:

```
C:\Users\admin\Documents\EdoGame\
```

### รายละเอียดโครงสร้างการ Deploy:
- `C:\Users\admin\Documents\EdoGame\WindBot\WindBot.dll` (ตัวรัน AI หลัก)
- `C:\Users\admin\Documents\EdoGame\WindBot\ExecutorBase.dll`
- `C:\Users\admin\Documents\EdoGame\WindBot\core.dll`
- `C:\Users\admin\Documents\EdoGame\WindBot\bots.json`
- `C:\Users\admin\Documents\EdoGame\WindBot\Decks\` & `Dialogs\`
- `C:\Users\admin\Documents\EdoGame\DashBot.exe` (DashBot WPF Launcher)
- `C:\Users\admin\Documents\EdoGame\deck\` (Game deck lists)
- `C:\Users\admin\Documents\EdoGame\WindBot.dll` (Root fallback binary)

> **ข้อห้ามเด็ดขาด**: ห้าม Deploy ไปยังโฟลเดอร์อื่น นอกเหนือจาก `C:\Users\admin\Documents\EdoGame\` โดยเด็ดขาด

---

## ⚙️ Build & Deploy Pipeline

ทุกครั้งที่มีการแก้ไขโค้ดใน `src\YGO_SOURCE_CLEAN` ให้รันคำสั่ง:
```powershell
cd C:\Users\admin\Documents\EdoGame\src\YGO_SOURCE_CLEAN
powershell -ExecutionPolicy Bypass -File .\BUILD_AND_DEPLOY.ps1
```

---

## 🛑 AI Policy: Rule-Based ModernExecutor Focus (CRITICAL)

1. **Rule-Based C# Executor ล้วน**:
   - โปรเจกต์นี้โฟกัสที่ **Rule-based C# Executor 100%** ไม่มีการใช้ AI Training, Neural Models, หรือ Reinforcement Learning
   - ห้ามสร้าง NeuralExecutor หรือโค้ดที่เกี่ยวกับ AI Training
2. **สถาปัตยกรรม Central Core & ModernExecutor**:
   - พัฒนาผ่าน `CardIntelligence` (ฐานข้อมูล O(1) กลาง), `ModernExecutor`, `ComboRouter`, `BaitPlanner`, `ChainAdvisor`, `HeuristicGuard`, `ResourcePlan`, และ universal heuristics `FallbackSelectCard`
3. **Headless Text Duel Simulation**:
   - เครื่องมือ `Client_Headless_Fortest` มีไว้เพื่อทดสอบตรรกะการเล่น (0 Violations / 0 Crash / Win Rate Audit) เท่านั้น

---

## 📋 Deck & Executor Optimization Workflow (MANDATORY ON "แก้ไขเด็ค")

เมื่อได้รับคำสั่งให้ **"แก้ไขเด็ค"** หรือ **"ปรับปรุง Executor"** ให้ปฏิบัติตามมาตรฐานนี้ทุกครั้งโดยอัตโนมัติ:

1. **วิเคราะห์โครงสร้างก่อนลงมือทำ (ห้ามเดา / ห้ามมั่ว)**:
   - ศึกษา Card ID, สเตตัส, Effect จริงจาก `cards.cdb` และ `.ydk`
   - วิเคราะห์ Starters, Extenders, Handtraps, Board Breakers, และ End Board Targets
   - ออกแบบแผนการเล่น: First Turn (Route A/B/C/D) และ Second Turn (Board Breaking & OTK)
2. **นโยบายการทดสอบ Headless Simulation (สำคัญมาก)**:
   - **ห้ามรันการจำลองดวล Headless Simulator โดยอัตโนมัติ** หลังจากสร้างหรือแก้ไขเด็คเสร็จ เนื่องจากผู้ใช้ต้องการทดสอบด้วยตนเองก่อนเสมอ
   - **จะทำการรัน Headless Text Duel ได้ก็ต่อเมื่อผู้ใช้สั่ง "Text Duel" (หรือ "จำลองดวล") เท่านั้น**
   - เมื่อผู้ใช้สั่ง "Text Duel" เท่านั้น จึงทำการรันดวลกับ 4 เด็คคู่ซ้อม Legacy: `ABC`, `Altergeist`, `BlueEyes`, `DarkMagician`
     `dotnet run --project src\YGO_SOURCE_CLEAN\Client_Headless_Fortest\Client_Headless_Fortest.csproj -c Release -- --deck <DECK_NAME> --opponent <OPPONENT> --games 10 --timeout 60`
   - วิเคราะห์ Log & Snapshots เพื่อหาจุดบกพร่องที่เป็นจุดร่วมกัน (Bottlenecks / Misplays / Missed Triggers)
3. **Iterative Optimization Loop (เมื่อได้รับคำสั่ง Text Duel)**:
   - วนลูป: **วิเคราะห์ ➔ แก้ไขโค้ด ➔ Build & Deploy ➔ ทดสอบ Headless ➔ วิเคราะห์ผล ➔ แก้ไขซ้ำ** เพื่อให้ผลลัพธ์ดีขึ้นอย่างมีนัยสำคัญ
4. **สรุปผลการดวลและสถิติ**:
   - รายงานสถิติการดวลแยกรายเด็ค (Wins / Losses / Win Rate %), สถิติ Violations (ต้องเป็น 0), และ Playbook Strategy
5. **Exclusive Deployment**:
   - Deploy ไบนารีชุดใหม่มาที่ `C:\Users\admin\Documents\EdoGame\` เสมอ
   - บันทึกการเปลี่ยนแปลงลงใน `PROGRESS.md` และบันทึกรายงานลงในโฟลเดอร์ `Docs/`
6. **Progress Log & Archiving Policy**:
   - ควบคุมขนาด `PROGRESS.md` ให้กระชับ (~200–400 บรรทัด)
   - หากความยาวเกิน 500–800 บรรทัด ให้ย้ายประวัติชุดเก่าไปเก็บไว้ที่ `Docs/PROGRESS_ARCHIVE.md` เสมอ เพื่อประหยัด Token และให้อ่านได้ครบใน 1 รอบ

