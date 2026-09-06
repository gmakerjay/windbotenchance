# WindBot Enhanced — Rule-Based YGO AI & ModernExecutor Platform

> **Repository สำหรับพัฒนาและปรับปรุง Rule-Based AI Executor สำหรับ EDOPro / EdoGame**  
> พัฒนาด้วย **C# (.NET 10.0)** 100% Rule-Based Architecture — ไม่มี Machine Learning หรือ Training Weights  
> รองรับการ Clone ไปใช้งานบนเครื่องใหม่ที่ติดตั้งเกม EDOPro ไว้แล้ว โดยไม่ต้องคัดลอกไฟล์ Asset ทั้งหมดของเกม

---

## 🚀 Quick Start: ติดตั้งและใช้งานบนเครื่องใหม่ (New Machine Setup)

หากคุณมีโฟลเดอร์เกม EDOPro อยู่แล้วบนอีกเครื่องหนึ่ง สามารถดึงเฉพาะ Source Code ไปพัฒนาต่อได้ทันที:

### 1. ติดตั้งสิ่งที่จำเป็น (Prerequisites)
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (หรือใหม่กว่า)
- Git สำหรับ Windows
- โฟลเดอร์เกม EDOPro (เช่น `C:\Users\<user>\Documents\EdoGame` หรือ `D:\EdoGame`)

### 2. Clone Repository
เปิด PowerShell หรือ Terminal แล้ว Clone Repository เข้าไปไว้ในโฟลเดอร์เกมของคุณ:

```powershell
# วิธีที่ 1: แนะนำ - Clone ไว้ที่โฟลเดอร์ src ภายในตัวเกม (Auto-Deploy จะตรวจจับเกมอัตโนมัติ)
cd "C:\Path\To\EdoGame"
git clone https://github.com/gmakerjay/windbotenchance.git src\YGO_SOURCE_CLEAN

# วิธีที่ 2: Clone เป็นโฟลเดอร์ windbotenchance ในโฟลเดอร์เกม
cd "C:\Path\To\EdoGame"
git clone https://github.com/gmakerjay/windbotenchance.git

# วิธีที่ 3: Clone ไว้ที่ไหนก็ได้ในเครื่อง
git clone https://github.com/gmakerjay/windbotenchance.git "D:\MyCode\windbotenchance"
```

### 3. Build & Deploy ครั้งแรก
เข้าไปที่โฟลเดอร์ Repository แล้วรันสคริปต์คอมไพล์:

```powershell
cd windbotenchance    # หรือ cd src\YGO_SOURCE_CLEAN

# รัน Build และ Deploy เข้าเกมอัตโนมัติ
powershell -ExecutionPolicy Bypass -File .\BUILD_AND_DEPLOY.ps1

# หรือถ้าวางไว้นอกโฟลเดอร์เกม ให้ระบุปลายทางด้วย -DeployTarget:
powershell -ExecutionPolicy Bypass -File .\BUILD_AND_DEPLOY.ps1 -DeployTarget "C:\Path\To\EdoGame"
```

เมื่อเสร็จสิ้น ไบนารีทั้งหมด (`WindBot.dll`, `ExecutorBase.dll`, `core.dll`, `bots.json`, `cards.cdb`, `DashBot.exe`, เด็ค `.ydk`, และบทพูด `Dialogs`) จะถูกนำไปวางในโฟลเดอร์เกมพร้อมเล่นทันที!

---

## 📁 โครงสร้าง Source Code ภายใน Repository

```
windbotenchance/
│
├── windbot-fork/                   # WindBot AI Engine & Executors (C# net10.0)
│   ├── WindBot.csproj              # Executable Project (Self-contained win-x64)
│   ├── Program.cs                  # Entry point
│   ├── bots.json                   # ทะเบียน Bot และการแมปเด็ค
│   ├── cards.cdb                   # ฐานข้อมูลการ์ด SQLite 2026 (รวมการ์ด Prerelease/Custom)
│   ├── Game/
│   │   ├── DecksManager.cs         # ตัวจัดการแมปชื่อเด็คเข้ากับ Executor Class
│   │   ├── GameBehavior.cs         # Duel protocol & OCGCore communication
│   │   └── AI/
│   │       └── Decks/              # ★ โฟลเดอร์รวม Executor รายเด็ค (_2026_*.cs, Legacy)
│   ├── ExecutorBase/               # ★ สถาปัตยกรรม Central AI Core
│   │   ├── Game/AI/
│   │   │   ├── CardIntelligence.cs # ฐานข้อมูล O(1) กลาง (Floodgates, Negators, Chokepoints)
│   │   │   ├── ModernExecutor.cs   # Base class หลักของ 2026+ executors พร้อม Hint Table
│   │   │   ├── Executor.cs         # Universal FallbackSelectCard, Safe Placement & Field Logic
│   │   │   ├── DefaultExecutor.cs  # Universal Handtrap, Counter Trap & Battle fallbacks
│   │   │   ├── ComboRouter.cs      # กลไกเลือก Route คอมโบตามการ์ดบนมือและสนาม
│   │   │   ├── BaitPlanner.cs      # ลำดับการหลอกล่อ (Baiting) ทรัพยากรคู่แข่ง
│   │   │   ├── ChainTimingAdvisor.cs # ประเมิน Chain และดักทางขัดจังหวะ
│   │   │   ├── BoardScorer.cs      # คำนวณ Threat Score และประเมินสภาพบอร์ด
│   │   │   └── AntiFloodgateHelper.cs # ตรวจจับ Floodgates และวางแผน Break Board
│   ├── Decks/                      # ไฟล์เด็คบอท (.ydk)
│   └── Dialogs/                    # บทสนทนาของบอท (.json)
│
├── dashbot/                        # DashBot WPF Launcher UI (C# net10.0-windows)
│   ├── dashbot.csproj
│   ├── MainWindow.xaml/.cs
│   └── App.xaml/.cs
│
├── Client_Headless_Fortest/        # Headless Duel Simulator (เครื่องมือทดสอบ AI ไร้หน้าจอ)
│   ├── Client_Headless_Fortest.csproj
│   └── Program.cs
│
├── core/                           # Shared Library ส่วนกลาง (IPC, EventBus, Logger)
│   ├── core.csproj
│   ├── EventBus.cs
│   └── IpcWorker.cs
│
├── Docs/                           # รายงานสถิติ, เอกสาร Architecture, และ Optimization Logs
├── .agents/skills/yugioh-executor/ # Skill & Guidelines สำหรับ AI Coding Assistant (Antigravity/Claude)
├── AGENTS.md                       # ข้อตกลงและกฎเหล็กในการพัฒนา AI Executor
├── PROGRESS.md                     # บันทึกประวัติการปรับปรุง, แก้ไขบั๊ก, และผลการทดสอบ
├── HANDOFF.md                      # สถานะการพัฒนาล่าสุดและ Next Steps
├── BUILD_AND_DEPLOY.ps1            # สคริปต์คอมไพล์และ Deploy กลางแบบ One-Click
└── YGO_AI_PLATFORM.slnx            # Solution รวมทุกโปรเจกต์
```

---

## 🛠️ ขั้นตอนการสร้างเด็คใหม่หรือแก้ไข Executor

### ขั้นตอนที่ 1: เตรียมไฟล์เด็ค (`.ydk`)
1. สร้างเด็คในเกมแล้วบันทึกไฟล์ `.ydk`
2. นำไฟล์ `.ydk` ไปวางไว้ที่ `windbot-fork/Decks/<DeckName>.ydk`

### ขั้นตอนที่ 2: สร้างไฟล์ Executor C#
1. สร้างไฟล์ใหม่ที่ `windbot-fork/Game/AI/Decks/_2026_<DeckName>Executor.cs`
2. ให้สืบทอดจาก `ModernExecutor`:
```csharp
using WindBot.Game;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("_2026_Sample", "_2026_Sample")]
    public class _2026_SampleExecutor : ModernExecutor
    {
        public enum CardId
        {
            // ระบุ Card ID ที่ใช้ในเด็ค
        }

        public _2026_SampleExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // กำหนด Starters, Extenders, Handtraps, Board Breakers
            // กำหนดลำดับการเล่น Spells, Monsters, Traps
            // เพิ่ม Hint Table สำหรับการเลือกการ์ดอัตโนมัติ
        }

        public override bool OnSelectHand()
        {
            return true; // true = First turn, false = Second turn
        }

        // เขียนตรรกะคอมโบและการ Activate การ์ดเฉพาะ
    }
}
```

### ขั้นตอนที่ 3: ลงทะเบียนเด็คในระบบ
1. เพิ่มข้อมูลบอทใน `windbot-fork/bots.json`:
```json
{
  "name": "2026_Sample",
  "deck": "_2026_Sample",
  "dialog": "default",
  "flags": ["OCG", "TCG"]
}
```
2. ตรวจสอบให้แน่ใจว่า `DecksManager.cs` สามารถแมปชื่อเด็คได้อย่างถูกต้อง

### ขั้นตอนที่ 4: คอมไพล์และ Deploy
รันคำสั่งใน PowerShell:
```powershell
powershell -ExecutionPolicy Bypass -File .\BUILD_AND_DEPLOY.ps1
```

### ขั้นตอนที่ 5: ทดสอบด้วย Headless Duel Simulator
ทดสอบเสถียรภาพ (0 Violations / 0 Crashes) และ Win Rate โดยไม่ต้องเปิดเกม EDOPro:
```powershell
# ทดสอบดวล 10 เกมกับคู่ซ้อมมาตรฐาน เช่น ABC, Altergeist, BlueEyes, DarkMagician
dotnet run --project Client_Headless_Fortest\Client_Headless_Fortest.csproj -c Release -- --deck 2026_Sample --opponent ABC --games 10 --timeout 60
```

---

## ⚙️ คำสั่ง Build & Deploy เพิ่มเติม

- **Build อย่างเดียว (ไม่ Deploy ไฟล์เข้าเกม)**:
  ```powershell
  .\BUILD_AND_DEPLOY.ps1 -BuildOnly
  ```

- **Deploy ไปยัง Path ที่ต้องการ**:
  ```powershell
  .\BUILD_AND_DEPLOY.ps1 -DeployTarget "D:\Games\EDOPro"
  ```

---

## 🛑 กฎเหล็กและข้อกำหนดทางเทคนิค (Strict Engineering Guidelines)

1. **100% Rule-Based C#**:
   - พัฒนาด้วยตรรกะ Rule-Based ผ่าน `ModernExecutor`, `ComboRouter`, และ `CardIntelligence`
   - ห้ามใช้ Machine Learning, Neural Networks หรือ AI Model Weights
2. **Central Core First**:
   - ใช้งาน `CardIntelligence.cs` สำหรับข้อมูล Floodgate/Negate สากล
   - ใช้ `Executor.FallbackSelectCard()` สำหรับการเลือกการ์ดอัจฉริยะแบบ O(1)
   - ไม่ฮาร์ดโค้ดสิ่งที่ระบบส่วนกลางทำได้อยู่แล้ว
3. **Verified Sources Only**:
   - เช็ค Card ID, ลำดับเชน, และเอฟเฟกต์การ์ดจากการ์ดจริงใน `cards.cdb` หรือ `.ydk` เสมอ ห้ามคาดเดา
4. **Clean Code & Zero Violation**:
   - การ์ดประเภท Counter Trap ต้องตรวจสอบ `Duel.LastChainPlayer != 0` เพื่อป้องกันการเชนซ้อน/จ่าย LP ซ้ำสองรอบ
   - ต้องผ่านการทดสอบ Headless อย่างน้อย 10 เกมโดยไม่มี error `MSG_RETRY` หรือ Crash หลุด
