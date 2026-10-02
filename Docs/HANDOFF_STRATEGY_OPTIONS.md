# Project Handoff & Strategic Roadmap: YugiohTH Rule-Based AI (ModernExecutor)

> **บันทึกเอกสารส่งต่องาน (Handoff Document)** สำหรับย้ายไปพัฒนาต่อบนเครื่องใหม่  
> **วันที่บันทึก**: 2026-09-05  
> **สถานะปัจจุบัน**: ระบบเสถียร 100%, Deploy ไบนารีชุดล่าสุดเรียบร้อย, 0 Violations, 0 Engine Crashes

---

## 1. ข้อมูลสำคัญของโปรเจกต์ & ข้อกำหนดเด็ดขาด (MANDATORY RULES)

1. **สถาปัตยกรรม Rule-Based C# .NET 10.0 ล้วน (STRICT RULE)**:
   - ห้ามใช้ Machine Learning, Neural Networks หรือ AI Training ใดๆ ทั้งสิ้น
   - ทุกอย่างขับเคลื่อนด้วย C# Rule-Based ผ่าน `ModernExecutor`, `ComboRouter`, `CardIntelligence`, และ `ChainTimingAdvisor`
2. **ตำแหน่ง Source Code หลัก**:
   ```
   C:\Users\admin\Documents\EdoGame\src\YGO_SOURCE_CLEAN\
   ```
3. **โฟลเดอร์ Deploy ปลายทางเพียงแห่งเดียว (EXCLUSIVE DEPLOY TARGET)**:
   ```
   C:\Users\admin\Documents\EdoGame\
   ├── WindBot\           (WindBot.dll, ExecutorBase.dll, core.dll, bots.json, Decks)
   ├── DashBot.exe        (DashBot WPF Launcher)
   └── deck\              (Game deck lists)
   ```
   *(ห้าม Deploy ไปที่อื่นนอกเหนือจากนี้โดยเด็ดขาด)*
4. **สคริปต์คอมไพล์และ Deploy กลาง**:
   ```powershell
   cd C:\Users\admin\Documents\EdoGame\src\YGO_SOURCE_CLEAN
   powershell -ExecutionPolicy Bypass -File .\BUILD_AND_DEPLOY.ps1
   ```
5. **คำสั่งรัน Headless Simulation ดวลทดสอบ AI Logic**:
   ```powershell
   dotnet run --project src\YGO_SOURCE_CLEAN\Client_Headless_Fortest\Client_Headless_Fortest.csproj -c Release -- --deck <DECK_NAME> --opponent <OPPONENT> --games 5 --timeout 60
   ```
   *(คู่ซ้อมมาตรฐาน 4 เด็ค: `BlueEyes`, `ABC`, `DarkMagician`, `Altergeist`)*

---

## 2. สถานะล่าสุดของระบบ (System Baseline ณ ปัจจุบัน)

| ส่วนประกอบ | สถานะ | รายละเอียด |
|---|---|---|
| **`bots.json` Registry** | **สะอาด 100%** | Purge รายชื่อหลอก `Neural_*` ออกทั้งหมด 42 รายการ (ลดจาก 199 เหลือ 157 บอทจริง) |
| **`Expert_` Difficulty** | **พร้อมใช้งาน** | บอทกลุ่ม `Expert_` คงไว้สำหรับฟิลเตอร์ระดับความยากของ DashBot Launcher และซ่อม Path เด็คทั้งหมดแล้ว |
| **Central Core Engine** | **V1.5 Deployed** | Universal Threat Scoring, Safe Zone Placement, Dynamic Battle Position, Smart Material Preservation |
| **4 Executors ที่เพิ่งกู้ชีพ** | **ผ่านเกณฑ์ 100%** | `2026_DDD` (2-Turn OTK), `2026_Plant` (Win 60%), `2026_Purrely` (Win 40%), `2026_Dracotail` (Win 80%) |
| **Engine Stability** | **0 Violations** | ไม่มี `MSG_RETRY` และไม่มี Engine Crash ในทุกแมตช์ทดสอบ |

---

## 3. รายละเอียดทั้ง 3 ทางเลือกสำหรับการพัฒนาต่อ (The 3 Strategic Options)

ผู้ใช้งานสามารถเลือก 1 ใน 3 ทางเลือกนี้เมื่อเปิดโปรเจกต์บนเครื่องใหม่:

```
┌────────────────────────────────────────────────────────────────────────┐
│                        3 STRATEGIC ROADMAP OPTIONS                     │
├──────────────────────────┬───────────────────────────┬─────────────────┤
│ Option A: Meta Batch     │ Option B: Central Core    │ Option C: Hybrid│
│ (เจาะลึก 3-4 เด็คท็อป)  │ (ยกระดับ Core Phase 2)    │ (ผสมผสาน A + B) │
└──────────────────────────┴───────────────────────────┴─────────────────┘
```

---

### 🟢 ทางเลือกที่ A: Archetype Batch Sprint (เจาะลึกเด็ค Meta 2026 ยอดนิยม)

โฟกัสที่การยกเครื่องเด็คตระกูล 2026 ที่ผู้เล่นนิยมสูงสุด เพื่อดึง Win Rate ขึ้นสู่ระดับ 70–90% พร้อมทำ Combo Line ที่สมบูรณ์แบบ

#### รายชื่อเด็คเป้าหมายชุดถัดไป (Batch 2):
1. **`2026_Maliss` ([_2026_MalissExecutor.cs](file:///C:/Users/admin/Documents/EdoGame/src/YGO_SOURCE_CLEAN/windbot-fork/Game/AI/Decks/_2026_MalissExecutor.cs))**:
   - **Concept**: Link / Cyberse Banish & Recursion Engine (Dormouse, White Rabbit, Cheshire Cat, Crypter, Hearts of Crypt)
   - **สิ่งที่ต้องทำ**:
     - เพิ่ม `OnSelectLinkMaterial` เพื่อป้องกันไม่ให้เผลอเอา Link-3 (Crypter) ไปทำ Link-1 หรือ Link-2 ทิ้ง
     - ผูกเงื่อนไขการ Banish จากสุสานเพื่อ Re-summon ตามลำดับ Chain ไม่ให้ขัดจังหวะกันเอง
     - ลงทะเบียน ComboLine `Maliss-Turn1-Crypter-Lock` ใน `ComboRouter`
2. **`2026_Labrynth` ([_2026_LabrynthExecutor.cs](file:///C:/Users/admin/Documents/EdoGame/src/YGO_SOURCE_CLEAN/windbot-fork/Game/AI/Decks/_2026_LabrynthExecutor.cs))**:
   - **Concept**: Normal Trap Control & Furniture Loop (Lovely, Lady, Arianna, Big Welcome, Welcome, Cooclock)
   - **สิ่งที่ต้องทำ**:
     - จัดลำดับการทิ้งการ์ดของ Furniture (Stovie/Chandraglier) โดยห้ามทิ้ง Lady หรือ Lovely ที่ไม่มีตัวสำรอง
     - จัดจังหวะการเปิด `Big Welcome Labrynth` ในเทิร์นคู่ต่อสู้เพื่อเด้งการ์ดฝ่ายตรงข้าม ไม่ใช่เด้งการ์ดตัวเองตอนบอร์ดว่าง
     - ป้องกัน Lady Labrynth ไม่ให้กดเปิดเอฟเฟกต์ค้นหากับดักถ้าไม่มี Normal Trap เหลือในเด็ค
3. **`2026_Fireking` ([_2026_FirekingExecutor.cs](file:///C:/Users/admin/Documents/EdoGame/src/YGO_SOURCE_CLEAN/windbot-fork/Game/AI/Decks/_2026_FirekingExecutor.cs))**:
   - **Concept**: Self-Destruction & Promethean Princess Recursion (Sacred Garunix, Kirin, Ponix, Sanctuary, Sky Burn, Amblowhale)
   - **สิ่งที่ต้องทำ**:
     - ป้องกันการใช้ Kirin ระเบิด Garunix บนสนามถ้ายังไม่ได้ทำลายบอร์ดฝ่ายตรงข้าม
     - ตั้งค่าจังหวะการชุบ Promethean Princess จากสุสานในเทิร์นคู่ต่อสู้เพื่อทำลายมอนสเตอร์ที่ศัตรูสเปเชียลซัมมอน
     - คุมไม่ให้ติด Fire Lock จนไม่สามารถเล่น Extra Deck ตัวอื่นได้
4. **`2026_Exosister` ([_2026_ExosisterExecutor.cs](file:///C:/Users/admin/Documents/EdoGame/src/YGO_SOURCE_CLEAN/windbot-fork/Game/AI/Decks/_2026_ExosisterExecutor.cs))**:
   - **Concept**: Anti-GY Xyz Chain Reaction (Martha, Elis, Stella, Kaspitell, Mikailis, Magnifica, Vadis)
   - **สิ่งที่ต้องทำ**:
     - ดักจับเงื่อนไข Martha (ห้ามมีมอนสเตอร์อื่นที่ไม่ใช่ Exosister ก่อนกดใช้)
     - วางลำดับการทับ Magnifica และ Mikailis ให้ Banish การ์ดศัตรูแบบ Quick Effect ได้ทันที

---

### 🔵 ทางเลือกที่ B: Advanced Central Core AI Phase 2 (ยกระดับ Core ส่วนกลางขั้นสูง)

มุ่งเน้นการเพิ่มความฉลาดระดับทัวร์นาเมนต์เข้าสู่ Core คลาสแม่ (`ModernExecutor.cs`, `ChainTimingAdvisor.cs`, `DefaultExecutor.cs`) ซึ่งจะส่งผลให้ **บอททั้ง 63 เด็คเก่งขึ้นพร้อมกันทันที**:

#### 4 ฟังก์ชันสำคัญของ Phase 2:
1. **Battle Phase & Attack Order Optimization (`DefaultExecutor.OnSelectBattleTarget`)**:
   - **Attack Sequencing**: เรียงลำดับตัวตีฉลาดขึ้น โดยสั่งให้มอนสเตอร์พลังน้อยตีเคลียร์มอนสเตอร์ที่มี Floating Effect ก่อน หรือตีตัวที่พลังต่ำกว่าเพื่อดูปฏิกิริยา แล้วค่อยส่งมอนสเตอร์ ATK สูงสุดตีปิดเกม
   - **Damage Step Safeguard**: ดักจับและประเมินเอฟเฟกต์บูสต์พลังตอนคำนวณดาเมจ (เช่น *Honest*, *Kalut*, *Tenpai Dragons*) เพื่อไม่ให้บอทวิ่งชนมอนสเตอร์ตายฟรี
2. **Draw / Standby Phase Trap Flipping (`ChainTimingAdvisor.cs`)**:
   - ปัจจุบันกับดักชะลอเกม (Floodgates) หลายใบ บอทรอไปเปิดใน Main Phase ทำให้โดนศัตรูร่าย Quick-Play / Normal Spell แก้ทางได้
   - เพิ่มตรรกะสั่งให้พลิกเปิดกับดัก เช่น *Skill Drain*, *Dimensional Barrier*, *Anti-Spell Fragrance*, *There Can Be Only One* **ตั้งแต่ช่วง Draw / Standby Phase ของฝ่ายตรงข้ามทันที**
3. **Auto Handtrap Defense (Chain Link 3 Guard ใน `ModernExecutor.cs`)**:
   - เมื่อ AI ร่าย Starter สำคัญ (เช่น *Branded Fusion*, *Snake-Eye Ash*, *Purrely My Friend*) แล้วศัตรูสวนด้วย Handtrap ใน Chain Link 2 (*Ash Blossom*, *Effect Veiler*, *Ghost Ogre*)
   - ให้ Core ทำการตรวจสอบมือของบอทโดยอัตโนมัติ หากมี *Called by the Grave* (24224830) หรือ *Crossout Designator* (65681983) ให้ส่งคำสั่งร่ายเป็น Chain Link 3 สวนทันทีโดยไม่ต้องเขียนโค้ดดักในแต่ละเด็ค
4. **Inherent Special Summon Negation Logic**:
   - แยกแยะระหว่าง "การลบล้างเอฟเฟกต์ (Negate Activation)" กับ "การลบล้างการอัญเชิญ (Negate Summon)" เพื่อให้การ์ดอย่าง *Solemn Strike*, *Solemn Warning*, หรือ *Black Horn of Heaven* ถูกสั่งใช้งานในจังหวะก่อนมอนสเตอร์แตะสนามจริงอย่างถูกต้อง

---

### 🟣 ทางเลือกที่ C: Hybrid Strategy (ผสมผสาน Core สำคัญ + เด็คท็อป 3 เด็ค) — *แนะนำ*

เป็นการเดินทางสายกลางที่คุ้มค่าที่สุด โดยแบ่งขั้นตอนเป็น:
- **สัปดาห์ที่ 1 / Step 1**: พัฒนา **Chain Link 3 Guard** (*Called by the Grave* / *Crossout*) และ **Draw Phase Trap Flipping** ใส่เข้า Central Core (`ModernExecutor.cs`)
- **สัปดาห์ที่ 1 / Step 2**: นำผลลัพธ์ของ Core ใหม่ไปใช้งานทันทีกับ 3 เด็คยอดนิยม: **`2026_Maliss`**, **`2026_Labrynth`**, และ **`2026_Fireking`**
- **สัปดาห์ที่ 1 / Step 3**: รัน Headless Simulator เทสต์เทียบกับ 4 เด็คคู่ซ้อมมาตรฐานเพื่อยืนยัน 0 Violations และ Win Rate ที่เพิ่มขึ้น

---

## 4. ข้อควรระวังและเทคนิคแก้บั๊กสำหรับนักพัฒนาคนต่อไป (Gotchas & Rules of Thumb)

1. **กับดัก `OnSelectYesNo` (The Default Yes Trap)**:
   - ฟังก์ชันเริ่มต้นใน `Executor.cs` ของ WindBot จะตอบ `true` เสมอ
   - หากการ์ดใบใดมีเอฟเฟกต์รองแบบ Optional (เช่น *Epurrely Plump*: "จากนั้นสามารถนำการ์ด 1 ใบออกจากเกมได้", *Dracotail Pan*: "จากนั้นสามารถทำลายมอนสเตอร์ 1 ตัวได้") **บอทจะกด Yes และบังคับให้เลือกทำลาย/เนรเทศการ์ดฝ่ายตนเองทันทีหากสนามฝ่ายตรงข้ามว่างเปล่า!**
   - **วิธีแก้**: ต้อง Override `OnSelectYesNo` ใน Executor นั้นๆ และดักจับ `Util.GetStringId(CardId, index)` หากเป้าหมายฝ่ายตรงข้ามมี 0 ใบ ให้ return `false` เสมอ
2. **ห้ามเรียก `AI.SelectNextCard` ซ้ำซ้อนก่อน `SelectCard`**:
   - ห้ามเรียก `AI.SelectNextCard` พร่ำเพรื่อใน Activate Handler เพราะจะทำให้คิวของ `GameAI.m_selector` ไม่ตรงกับ OCGCore Packet ส่งผลให้ขึ้น `Error: Call SelectNextCard() before SelectCard()`
   - ให้ปล่อยให้การเลือกส่งต่อไปยัง `OnSelectCard()` เป็นผู้ตัดสินใจ
3. **การ Deploy ไบนารี**:
   - การรัน `dotnet build` ปกติจะไม่เอาไฟล์ไปลงที่ `C:\Users\admin\Documents\EdoGame\WindBot\`
   - ต้องรัน `BUILD_AND_DEPLOY.ps1` เสมอ เพื่อให้ DLLs, bots.json, และ Decks ถูกติดตั้งลงในโฟลเดอร์รันจริงของเกม

---

## 5. วิธีเริ่มต้นงานต่อทันทีบนเครื่องใหม่ (Quick Start Commands)

```powershell
# 1. ไปยังโฟลเดอร์ Source Code
cd C:\Users\admin\Documents\EdoGame\src\YGO_SOURCE_CLEAN

# 2. คอมไพล์และตรวจสอบสถานะการ Deploy
powershell -ExecutionPolicy Bypass -File .\BUILD_AND_DEPLOY.ps1

# 3. ตรวจสอบความพร้อมด้วยการรัน Headless Test เด็คที่เสร็จแล้ว 1 เกม
dotnet run --project Client_Headless_Fortest\Client_Headless_Fortest.csproj -c Release -- --deck 2026_DDD --opponent BlueEyes --games 1 --timeout 30

# 4. เมื่อพร้อมทำงานต่อ: เลือก Option A, B หรือ C แล้วแจ้ง Prompt ให้ AI ดำเนินการต่อได้ทันที!
```

---
*เอกสารนี้ถูกบันทึกเพื่อใช้เป็น Master Handoff สำหรับโปรเจกต์ YugiohTH EdoGame ModernExecutor*
