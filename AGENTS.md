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

---

## 🚫 ข้อห้ามเด็ดขาดและ Anti-Patterns ในการเขียน Executor (CRITICAL)

1. **ห้ามใช้ `preferred` ใน `OnSelectCard` โดยไม่แยกแยะ Hint ID**:
   - ลิสต์ค้นหาการ์ดจากเด็ค ต้องเช็ค `hint == 506 (HINTMSG_ATOHAND)` เท่านั้น
   - เมื่อ Hint เป็นคำสั่งรีมูฟ (`hint == 503`) หรือทำลาย (`hint == 502`) **ต้องบังคับเลือกเฉพาะการ์ดของศัตรู (`c.Controller == 1`) เสมอ** ห้ามเลือกการ์ดฝั่งเราเด็ดขาดถ้ายังมีการ์ดศัตรูให้เลือก ป้องกันบอทเล็งทำลาย/รีมูฟการ์ดเอซตัวเอง
2. **ห้าม Extra Deck Summon คืนค่า `true` แบบไร้เงื่อนไข**:
   - ห้ามนำมอนสเตอร์ที่สวมใส่การ์ดขโมย (เช่น `Comic Hand`, `Snatch Steal`) ไปเป็นวัตถุดิบ Extra Deck หรือสังเวยเด็ดขาด (จะทำให้การ์ดสวมใส่หลุดลงสุสานฟรี)
   - ห้ามสังเวยมอนสเตอร์ตีตรงพลังสูง (2000+) ใน Main Phase 1 เพื่อทำ Extra Deck ที่พลังน้อยกว่าและตีตรงไม่ได้
3. **ห้ามเงื่อนไข Tribute Summon ที่เป็นไปไม่ได้**:
   - ห้ามเขียนเงื่อนไขบูชายัญมอนสเตอร์เลเวล 5+ ว่า `Bot.GetMonsterCount() == 0` เด็ดขาด เพราะจะทำให้บอทค้าง/Pass Turn
4. **ห้ามเสิร์ชแล้วเลือกการ์ดเดิมวางกลับเด็คทันที**:
   - เช่น `Illusion of Chaos` ต้องเลือกคืนการ์ดขยะ ห้ามคืนการ์ดที่เพิ่งเสิร์ชมา
5. **ห้ามนำ Handtrap ไปหมอบบนสนามใน Main Phase 1**:
   - การ์ดที่ใช้งานจากมือได้ (เช่น `Dominus Impulse`, `Ash Blossom`) ต้องเก็บไว้บนมือเท่านั้น
6. **ห้ามรีมูฟหรือทิ้ง Core Boss สำคัญของเด็คอย่างไร้เหตุผล**:
   - ใน `Pot of Prosperity` หรือ Cost ต่างๆ ต้องยกเว้น Core Boss ห้ามนำไปรีมูฟ
7. **ห้ามรัน Headless Simulation อัตโนมัติ**:
   - รอคำสั่ง "Text Duel" (หรือ "จำลองดวล") จากผู้ใช้เท่านั้น
8. **ห้าม Deploy นอกโฟลเดอร์ `C:\Users\admin\Documents\EdoGame\` เด็ดขาด**
