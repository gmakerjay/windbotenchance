# รายงานการพัฒนาและตรวจสอบ 5 สุดยอดเด็ค Anime ModernExecutors (Bot Hard)
**วันที่:** 20 กันยายน 2026  
**โปรเจกต์:** YugiohTH / WindBot / DashBot  
**เป้าหมาย:** พัฒนา Rule-Based C# ModernExecutor และ Deck Lists สำหรับหมวด Anime จำนวน 5 เด็ค ที่มีความทนทานสูงมาก (Sticky / การ์ดตายยาก), คอมโบต่อเนื่องรุนแรง, บอทตัดสินใจแม่นยำระดับ Bot Hard, และผ่านการตรวจสอบการ์ดจริง 100% (0 Banlist Violations / 0 CDB Errors)

---

## 1. บทนำและสรุปผลเชิงสถาปัตยกรรม

ตามคำสั่ง `/goal` และหลักเกณฑ์ใน [AGENTS.md](file:///C:/Users/admin/Documents/EdoGame/AGENTS.md):
1. **Rule-Based C# Executor ล้วน (100%)**: พัฒนาบนฐาน `ModernExecutor` โดยไม่มีโมเดล Neural หรือ RL
2. **ห้ามจำลองดวล Headless Simulator อัตโนมัติ**: ระบบปฏิบัติตามกฎอย่างเคร่งครัด โดยเตรียมพร้อมไบนารีและสำรับให้ผู้ใช้เปิดทดสอบเองผ่าน DashBot หรือ EDOPro
3. **ตรวจสอบการ์ดจริง (Zero Guesswork / Real CDB Verification)**: ทุก Card ID ถูกดึงและตรวจสอบโดยตรงจาก `cards.cdb` และ banlist ล่าสุด `0TCG.lflist.conf`
4. **Deploy เฉพาะโฟลเดอร์หลัก**: ไบนารีชุดสมบูรณ์ (`WindBot.dll`, `ExecutorBase.dll`, `core.dll`, `DashBot.exe`, `bots.json`, `.ydk`) ถูกส่งมอบมาที่ `C:\Users\admin\Documents\EdoGame\` เรียบร้อยแล้ว

---

## 2. ตารางภาพรวม 5 เด็ค Anime ระดับ Bot Hard

| ชื่อเด็ค (Deck ID) | ตัวละครอนิเมะ | สไตล์กลยุทธ์หลัก | จุดเด่น "การ์ดตายยาก / Immortal" | ตัวจบเกม (Win Con / Bosses) | สถานะความถูกต้อง |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **`Anime_Kaiba`** *(ใหม่)* | เซโตะ ไคบะ (Seto Kaiba) | Blue-Eyes Jet & Ultimate Fusion | `Blue-Eyes Jet Dragon` (ชุบตัวเองจากสุสาน/มือไม่จำกัดเมื่อมีการ์ดบนสนามถูกทำลาย + ป้องกันการ์ดอื่นถูกทำลาย), `True Light` (คุ้มกันเล็งเป้า), `Twin Burst` (อมตะจากการต่อสู้) | `Neo Blue-Eyes Ultimate` (4500 ATK x 3 ตี), `Numeron Dragon` (17,000 ATK OTK), `Hope Harbinger` (ตัดเวท) | **ผ่าน 100%** (Main 41 / Extra 15) |
| **`Anime_Judai`** *(ใหม่)* | ยูกิ จูได (Jaden Yuki) | HERO Destiny Phoenix & Dark Law Omni-Lock | `Destiny HERO - Destroyer Phoenix Enforcer` (DPE: Quick Pop ทำลายตัวเองกับศัตรู แล้วชุบตัวเองกลับมาทุกเทิร์นไม่รู้จบ), `Wake Up Your E-HERO` (ชุบตัวใหม่เมื่อถูกทำลาย) | `Masked HERO Dark Law` (ตัดมิติการ์ดศัตรูลงสุสาน + ริบการ์ดบนมือ), `Destiny HERO - Plasma` (Skill Drain ฝั่งเดียว + ขโมยมอนสเตอร์) | **ผ่าน 100%** (Main 40 / Extra 15) |
| **`Anime_Zane`** *(ใหม่)* | ซาเนะ ทรูสเดล (Zane Truesdale) | Cyber Dragon Infinity & Clockwork Contact | `Clockwork Night` เปลี่ยนมอนสเตอร์ศัตรูทั้งสนามเป็น Machine นำไปกินเป็นวัตถุดิบ Contact Fusion `Chimeratech Fortress Dragon` โดยไม่ต้องเริ่มเชน, `Cyber Dragon Nova` (ลอยตัวเป็น Cyber End Dragon 4000 ATK เมื่อถูกเก็บ) | `Cyber Dragon Infinity` (Omni-Negate + กลืนมอนสเตอร์ศัตรู), `Chimeratech Rampage` (ล้างกับดักเวท + โจมตี 3 ครั้ง), `Therion Regulus` (Omni-Negate) | **ผ่าน 100%** (Main 40 / Extra 15) |
| **`Anime_JackAtlas`** *(ยกเครื่องใหม่)* | แจ็ค แอทลาส (Jack Atlas) | Resonator & Red Supernova Calamity-Free | ถอดการ์ดโดนแบน `King Calamity` (Limit 0) ออก แล้วใส่ `Bystial Dis Pater` ระดับ 10 แทน; `Red Supernova Dragon` (อมตะจากเอฟเฟกต์ทำลาย + Quick วาร์ปตัวเองล้างบางและรีมูฟศัตรูทั้งสนาม), `Soul Resonator` (โดดคุ้มครองในสุสาน) | `Red Supernova Dragon` (4000+ ATK), `Hot Red Dragon Archfiend Abyss` (ตัดเอฟเฟกต์ Quick), `Hot Red Dragon Archfiend Bane` (ชุบ RDA วนลูป) | **ผ่าน 100%** (Main 41 / Extra 15) |
| **`Anime_Yusei`** *(ยกเครื่องใหม่)* | ฟุโด ยูเซย์ (Yusei Fudo) | Cosmic Blazar & Shooting Majestic Accel Synchro | `Cosmic Blazar Dragon` (รีมูฟตัวเองเป็น Cost เพื่อสั่งลบล้างการใช้การ์ด / การอัญเชิญ / การโจมตี หลบการเล็งเป้าและบอร์ดไวป์ได้สมบูรณ์แบบ), `Accel Synchro Stardust Dragon` (ทำให้มอนสเตอร์ซิงโครตัวบอส Unaffected จากเอฟเฟกต์ที่สั่งใช้งาน) | `Cosmic Blazar Dragon` (Omni-Negate ร่างสมบูรณ์), `Shooting Majestic Star Dragon` (ตัดเอฟเฟกต์ถาวร + ตัดการใช้งาน), `Shooting Quasar Dragon` | **ผ่าน 100%** (Main 41 / Extra 15) |

---

## 3. รายละเอียดการวิเคราะห์เชิงลึกและการ์ดของแต่ละเด็ค

### 3.1 `Anime_Kaiba` (Seto Kaiba)
- **ไฟล์โค้ด:** `src/YGO_SOURCE_CLEAN/windbot-fork/Game/AI/Decks/Anime_KaibaExecutor.cs`
- **ไฟล์เด็ค:** `Anime_Kaiba.ydk` (Main 41, Extra 15)
- **แกนความตายยาก (Immortal Engine):**
  - `Blue-Eyes Jet Dragon` (86188410): การ์ดใบอื่นทั้งหมดบนสนามฝั่งเราไม่สามารถถูกทำลายด้วยเอฟเฟกต์การ์ดของศัตรู และเมื่อมีการ์ดใดๆ บนสนามถูกทำลาย ไม่ว่าจะจากการต่อสู้หรือเอฟเฟกต์ Jet Dragon จะฟื้นชีพตัวเองขึ้นมาจากสุสานหรือมือได้ทันที! ในจังหวะโจมตี สามารถดีดการ์ดศัตรูกลับขึ้นมือได้อีกด้วย
  - `True Light` (97077563): ป้องกันไม่ให้ศัตรูเล็งเป้า Blue-Eyes White Dragon บนสนาม และสามารถเสิร์ช/เซ็ต `Ultimate Fusion` หรือชุบ BEWD ฟรีทุกเทิร์น
  - `Blue-Eyes Spirit Dragon` (59822133): ล็อคไม่ให้ทั้งสองฝ่าย Special Summon พร้อมกันเกิน 1 ตัว (สกัด Pendulum, Nibiru, Soul Charge), ตัดเอฟเฟกต์สุสาน, และสามารถสละตัวเองชุบ `Azure-Eyes Silver Dragon` (ป้องกันการ์ดมังกรทั้งหมดจากการเล็งเป้าและทำลาย) หรือ `Black Rose Moonlight Dragon` (ดีดมอนสเตอร์ศัตรูกลับมือ)
- **สายคอมโบ OTK:**
  - `Number 97: Draglubion` โดด `Number 100: Numeron Dragon` พลังโจมตีทะลุ **17,000 ATK** ทุบทีเดียวชนะ
  - `Ultimate Fusion` ผสานร่างจากสุสานและมือแบบ Quick-Play ในเทิร์นใดก็ได้ ออก `Neo Blue-Eyes Ultimate Dragon` พลัง 4500 โจมตี 3 ครั้ง พร้อมล้างสนามศัตรูตามจำนวนบลูอายส์ที่ใช้

---

### 3.2 `Anime_Judai` (Jaden Yuki)
- **ไฟล์โค้ด:** `src/YGO_SOURCE_CLEAN/windbot-fork/Game/AI/Decks/Anime_JudaiExecutor.cs`
- **ไฟล์เด็ค:** `Anime_Judai.ydk` (Main 40, Extra 15)
- **แกนความตายยาก (Immortal Engine):**
  - `Destiny HERO - Destroyer Phoenix Enforcer` (DPE) (60461804): Quick Effect สั่งทำลายการ์ดตัวเอง (เลือกตัวเอง) ร่วมกับการ์ดศัตรู 1 ใบ จากนั้นใน Standby Phase เทิร์นถัดไป DPE จะชุบตัวเองกลับมายังสนามใหม่ วนลูปการทำลายแบบอมตะทุกเทิร์นอย่างต่อเนื่อง พร้อมลดพลังโจมตีมอนสเตอร์ศัตรู 200 ต่อการ์ด HERO ในสุสาน
- **การล็อคกระดานขั้นสูงสุด (Omni-Lock):**
  - `Masked HERO Dark Law` (58481572): สั่งให้การ์ดทุกใบของศัตรูที่ถูกส่งลงสุสาน โดนนำออกนอกเกม (Banish) ทันที เสมือน Macro Cosmos ด้านเดียว และสุ่มริบการ์ดบนมือศัตรูทิ้งทันทีเมื่อศัตรูเสิร์ชการ์ด
  - `Destiny HERO - Plasma` (83965310): สั่งลบล้างเอฟเฟกต์มอนสเตอร์ที่หงายหน้าทั้งหมดของศัตรูบนสนาม (One-sided Skill Drain) และขโมยมอนสเตอร์ตัวเก่งของศัตรูมาสวมใส่เพิ่มพลัง
- **สายคอมโบลื่นไหล:**
  - `Vision HERO Faris` ทิ้งการ์ด HERO โดดตัวเอง -> วาง `Increase` -> สังเวยเรียก `Vyon` -> โม่ `Shadow Mist` หรือ `Malicious` -> รีไซเคิลด้วย `Denier` -> เสิร์ช `Polymerization` และ `Miracle Fusion` เรียก `Sunrise` และ `Absolute Zero` ซึ่งหากนำไป Mask Change เป็น `Acid` จะเกิดคอมโบทำลายการ์ดทั้งมอนสเตอร์และเวทกับดักของศัตรูจนหมดสิ้น (Total Board Wipe)

---

### 3.3 `Anime_Zane` (Zane Truesdale)
- **ไฟล์โค้ด:** `src/YGO_SOURCE_CLEAN/windbot-fork/Game/AI/Decks/Anime_ZaneExecutor.cs`
- **ไฟล์เด็ค:** `Anime_Zane.ydk` (Main 40, Extra 15)
- **แกนกินสนามไร้การแก้ทาง (Clockwork Field Devour):**
  - `Clockwork Night` (94977269): เปลี่ยนมอนสเตอร์ทุกตัวบนสนามของทั้งสองฝ่ายให้กลายเป็นเผ่า Machine โดยมอนสเตอร์เราพลังเพิ่ม 500 ส่วนมอนสเตอร์ศัตรูพลังลด 1000
  - `Chimeratech Fortress Dragon` (79229522): ทำการ Contact Fusion โดยส่ง Cyber Dragon และมอนสเตอร์เผ่า Machine บนสนาม "ของทั้งสองฝ่าย" ลงสุสาน เนื่องจาก Clockwork Night เปลี่ยนศัตรูเป็น Machine ทั้งหมด บอทจึงสามารถส่งมอนสเตอร์ของศัตรูทั้งสนามลงสุสานเพื่ออัญเชิญ Fortress Dragon ได้ทันที โดยที่ศัตรู **ไม่สามารถกด Negate ได้** เพราะเป็นการอัญเชิญตามเงื่อนไข (Special Summon Procedure) ที่ไม่เริ่มเชน!
  - `Chimeratech Megafleet Dragon` (78063197): ส่งมอนสเตอร์ใน Extra Monster Zone ของศัตรูลงสุสานทันที
- **ตัวคุมและป้องกัน (Disruptions):**
  - `Cyber Dragon Infinity` (10443957): ซ้อนทับบน Nova ได้ทันที สามารถถอดวัตถุดิบเพื่อลบล้างเอฟเฟกต์การ์ดทุกชนิด (Omni-Negate) และสามารถดูดมอนสเตอร์ศัตรูมาเป็นวัตถุดิบได้ทุกเทิร์น
  - `Cyber Dragon Nova` (58069384): หากถูกศัตรูส่งลงสุสานด้วยเอฟเฟกต์ จะลอยตัวเรียก `Cyber End Dragon` พลัง 4000 ลงมาแทนทันที!

---

### 3.4 `Anime_JackAtlas` (Jack Atlas - ยกเครื่องใหม่)
- **ไฟล์โค้ด:** `src/YGO_SOURCE_CLEAN/windbot-fork/Game/AI/Decks/Anime_JackAtlasExecutor.cs`
- **ไฟล์เด็ค:** `Anime_JackAtlas.ydk` (Main 41, Extra 15)
- **การแก้ไข Banlist:**
  - นำ `Hot Red Dragon Archfiend King Calamity` (62242678) ซึ่งติดสถานะแบน (Limit 0) ใน TCG ออกจากเด็ค และแทนที่ด้วยมังกรซิงโครมืดเลเวล 10 ยอดนิยม `Bystial Dis Pater` (27572350)
  - `Bystial Dis Pater` สามารถดึงมอนสเตอร์แสง/มืดที่ถูกรีมูฟกลับมาชุบบนสนามเราได้ฟรี และมี Quick Effect สับการ์ดที่ถูกรีมูฟกลับเข้าเด็คเพื่อ Negate หรือทำลายมอนสเตอร์ศัตรู
- **แกนล้างบางและเหนียวแน่น:**
  - `Red Supernova Dragon` (99585850): พลังโจมตีเริ่มต้น 4000+ ไม่สามารถถูกทำลายด้วยเอฟเฟกต์การ์ด และเมื่อศัตรูสั่งใช้งานเอฟเฟกต์มอนสเตอร์หรือสั่งโจมตี สามารถสั่ง Quick Effect รีมูฟตัวเองพร้อม **รีมูฟการ์ดทั้งหมดของศัตรูออกจากเกม** แบบถอนรากถอนโคน
  - `Soul Resonator` (62991792): สามารถรีมูฟตัวเองจากสุสานเพื่อป้องกันไม่ให้การ์ดบนสนามฝั่งเราถูกทำลาย
  - `Hot Red Dragon Archfiend Abyss` (9753964): Quick Effect สั่ง Negate การ์ดที่หงายหน้าของศัตรูได้ 1 ใบในทุกเทิร์น

---

### 3.5 `Anime_Yusei` (Yusei Fudo - ยกเครื่องใหม่)
- **ไฟล์โค้ด:** `src/YGO_SOURCE_CLEAN/windbot-fork/Game/AI/Decks/Anime_YuseiExecutor.cs`
- **ไฟล์เด็ค:** `Anime_Yusei.ydk` (Main 41, Extra 15)
- **แกนความตายยากระดับหลบมิติ:**
  - `Cosmic Blazar Dragon` (21123811): มังกรซิงโครเลเวล 12 ที่ทรงพลังที่สุด สามารถรีมูฟตัวเองออกจากเกมจนถึงจบเทิร์นเป็น Cost เพื่อ:
    1. ลบล้างการสั่งใช้งานการ์ดหรือเอฟเฟกต์ใดๆ ของศัตรูและทำลาย
    2. ลบล้างการอัญเชิญมอนสเตอร์ของศัตรูและทำลาย
    3. ลบล้างการโจมตีของศัตรูและจบ Battle Phase ทันที
    *(เนื่องจากรีมูฟตัวเองออกนอกเกมเป็น Cost จึงไม่ตกเป็นเป้าหมายของการ์ดแก้ทาง เช่น Super Polymerization หรือ Droplet)*
  - `Shooting Majestic Star Dragon` (40939228): สั่งลบล้างเอฟเฟกต์มอนสเตอร์ศัตรูบนสนามได้ถาวร และมี Quick Effect วาร์ปตัวเองเพื่อลบล้างและรีมูฟการ์ดศัตรู
  - `Accel Synchro Stardust Dragon` (30983281): อัญเชิญซิงโครแบบ Quick Effect ในเทิร์นศัตรู พร้อมมอบบัฟ **Unaffected by opponent's activated effects** ให้แก่มอนสเตอร์บอสที่ลงมาในเทิร์นนั้น

---

## 4. ผลการตรวจสอบความถูกต้องและข้อบังคับ (Audit Checklist)

1. **Card ID Validation (`cards.cdb`)**: ผ่าน 100% ทุกใบมีข้อมูลจริง Type, Stats, Text ครบถ้วน ไม่มีการสุ่ม Card ID
2. **Banlist Check (`0TCG.lflist.conf`)**: ผ่าน 100% ทุกเด็คไม่มีการ์ด Forbidden (Limit 0) และไม่มีการใส่การ์ดเกินโควตา Limit (เช่น Called by the Grave = 1, Triple Tactics = 1)
3. **Deck Error Audit (Room Join Guarantee)**: ผ่าน 100% ทั้ง 5 เด็คทดสอบโหลดเข้า WindBot CLI ด้วยคำสั่ง `dotnet .\WindBot.dll Deck=Anime_*` สามารถค้นพบเด็คและโหลด Executor สำเร็จโดยไม่มีข้อผิดพลาด `ERRMSG_DECKERROR`
4. **Anti-Patterns Enforcement**:
   - แยกแยะ `hint == 506` (`HINTMSG_ATOHAND`) ใน `OnSelectCard` เฉพาะการ์ดที่ต้องการขึ้นมือ
   - บังคับเลือกเฉพาะการ์ดศัตรู (`c.Controller == 1`) ในคำสั่งทำลาย (`hint == 502`) และรีมูฟ (`hint == 503`) ป้องกันบอทเล็งทำลายการ์ดตนเอง
   - ตรวจสอบเงื่อนไข Tribute Summon ป้องกันบอทติดลูปหรือหยุดเล่น
   - เซ็ตกับดักใน Main Phase 2 เพื่อป้องกันการเสียของโดยไม่จำเป็น
5. **DashBot Launcher Integration**:
   - บอททั้ง 5 เด็คแสดงผลภายใต้หมวด **Anime** อย่างถูกต้องใน DashBot Launcher
   - ปรับแต่งชื่อแสดงผลให้สวยงาม: "Seto Kaiba", "Jaden Yuki", "Zane Truesdale", "Jack Atlas", "Yusei Fudo"

---

## 5. คู่มือสำหรับผู้ใช้งานในการทดสอบ

ผู้ใช้สามารถเปิดทดสอบได้ 2 รูปแบบตามความสะดวก:

### วิธีที่ 1: ทดสอบผ่าน DashBot UI Launcher (แนะนำ)
1. เปิดโปรแกรม `C:\Users\admin\Documents\EdoGame\DashBot.exe`
2. คลิกที่แท็บหมวดหมู่ **"Anime"** ทางซ้ายมือ
3. จะพบรายชื่อเด็คทั้ง 5:
   - **Seto Kaiba** (`Anime_Kaiba`)
   - **Jaden Yuki** (`Anime_Judai`)
   - **Zane Truesdale** (`Anime_Zane`)
   - **Jack Atlas** (`Anime_JackAtlas`)
   - **Yusei Fudo** (`Anime_Yusei`)
4. เลือกรหัสห้องและ IP เซิร์ฟเวอร์ EDOPro แล้วกดปุ่ม **"Start & Connect Bot to Room"**
5. หรือหากต้องการทดสอบ **Bot vs Bot**: ติ๊กถูกที่ *"Spawn 2 Bots"*, เลือก Bot 1 และ Bot 2 (เช่น Kaiba vs Zane) แล้วกดเริ่มดวลได้ทันที

### วิธีที่ 2: รันผ่าน Command Line (CLI)
สามารถสั่งรันบอทแต่ละตัวเพื่อเข้าไปยังห้องดวลได้โดยตรง:
```powershell
cd C:\Users\admin\Documents\EdoGame\WindBot

# เรียก Seto Kaiba
dotnet .\WindBot.dll Deck=Anime_Kaiba Host=127.0.0.1 Port=7911

# เรียก Jaden Yuki
dotnet .\WindBot.dll Deck=Anime_Judai Host=127.0.0.1 Port=7911

# เรียก Zane Truesdale
dotnet .\WindBot.dll Deck=Anime_Zane Host=127.0.0.1 Port=7911

# เรียก Jack Atlas
dotnet .\WindBot.dll Deck=Anime_JackAtlas Host=127.0.0.1 Port=7911

# เรียก Yusei Fudo
dotnet .\WindBot.dll Deck=Anime_Yusei Host=127.0.0.1 Port=7911
```
*(หมายเหตุ: ปรับแก้ Port ให้ตรงกับพอร์ตของ EDOPro ที่เปิดห้องไว้ เช่น 7911)*
