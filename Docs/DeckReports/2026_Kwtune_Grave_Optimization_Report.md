# รายงานการ Refactor & ยกระดับโครงสร้างเด็ค: Kwtune & Grave

**วันที่**: 2026-09-27  
**เด็คที่ดำเนินการ**:
1. `Kwtune` (`_2026_KwtuneExecutor.cs` & `2026_Kwtune.ydk`)
2. `Grave` (`_2026_GraveExecutor.cs` & `2026_Grave.ydk`)

---

## 1. การวิเคราะห์ปัญหาเดิมของเด็ค (Root Cause Analysis)

### 1.1 เด็ค Kwtune (`2026_Kwtune`)
- **การขาดชิ้นส่วนคอมโบหลักใน `2026_Kwtune.ydk`**:
  - Extra Deck มี `Visas Amritara` (`821049`) ซึ่งเมื่อซิงโครลงมาจะต้องค้นหาเวท/กับดักสาย Visas (`Mannadium Reframing` ID: `18158393`) แต่ใน Main Deck เดิมกลับไม่มี Reframing อยู่เลย ทำให้เอฟเฟกต์เสิร์ช Omni-Negate ล้มเหลวและเสียทรัพยากรฟรี
  - `Duelist Genesis` ค้นหาเวท/กับดักที่มีคำว่า "Synchro" แต่ในเด็คมีเพียง `Kewl Tune Synchro` ไม่มี `Synchro Emergency` (`49415281`) ทำให้การเดินเกมในสุสานและตัวเลือกขาดความยืดหยุ่น
  - ขาด Extender นอกธีมที่สำคัญมากอย่าง `Starjunk Synchron` (`13021682`) (SS ฟรีเมื่อมี Mix ที่เป็น Warrior) และ `Jet Synchron` (`9742784`)
- **Card ID Mismatch (ข้อผิดพลาดรหัสการ์ด)**:
  - ใน `2026_Kwtune.ydk` ใช้ Ash Blossom รหัส `14558127` แต่ใน `_2026_KwtuneExecutor.cs` เดิมฮาร์ดโค้ดเป็น `14558128` ส่งผลให้บอทไม่ยอมใช้ Ash Blossom เมื่อเล่นเด็ค `2026_Kwtune`

### 1.2 เด็ค Grave (`2026_Grave`)
- **ข้อขัดแย้งรุนแรงระหว่าง Necrovalley กับ Engine K9 (Fatal Anti-Synergy)**:
  - ในโค้ดเดิม Necrovalley ถูกตั้งไว้ที่ Priority 4 (ร่ายตั้งแต่ต้นเทิร์น ก่อนการซัมมอนของมอนสเตอร์)
  - เมื่อ Necrovalley ทำงานบนสนาม **เอฟเฟกต์บนมือของ `K9-66b Lantern` ที่ต้องชุบ K9 Lv5 จากสุสานจะถูก Necrovalley สั่ง Negate ทันที!** ทำให้ Lantern กลายเป็นการ์ดตายบนมือ
  - `Called by the Grave` และ `Illusion Gate` โดน Necrovalley บล็อกจนไร้ประโยชน์
- **การ์ดที่ไม่เข้าพวกและการ์ดที่เป็นภาระ (Bricks & Dead Cards)**:
  - `Gravekeeper's Chief`: เลเวล 5 สังเวยเรียก ชุบ Commandant (Lv4) ทำให้สนามกลายเป็น Lv5 + Lv4 ไม่สามารถ Xyz แรงค์ 5 ได้
  - `Gravekeeper's Shaman`: เลเวล 6 สังเวยเรียก ไม่เข้ากับแรงค์ 5
  - `Dark Renewal`: ต้องใช้ Spellcaster บนสนาม แต่พอทิ้ง Commandant หา Necrovalley ไปแล้ว บนสนามไม่มี Spellcaster เหลืออยู่เลย กลายเป็นการ์ดที่เปิดใช้ไม่ได้ตลอดทั้งเกม
  - `Illusion Gate`: จ่ายครึ่ง LP เพื่อชุบสุสานศัตรู (ซึ่ง Necrovalley ห้าม)
- **อัตราส่วนตัวเริ่ม K9 ต่ำเกินไป**:
  - `K9-66a Jokul` และ `K9-04 Noroi` ใส่เพียงอย่างละ 2 ใบ ทั้งที่เป็นตัวเริ่ม 1-Card / 2-Card Combo ที่ดีที่สุดในเด็ค

---

## 2. แผนการปรับปรุงโครงสร้างเด็ค (Deck Refactoring & Optimization)

### 2.1 โครงสร้างเด็ค Kwtune ใหม่ (`2026_Kwtune.ydk`)
ปรับสเปกเทียบเท่าเด็คระดับแชมป์โลก (World Championship Tier-1):
- **ตัดออก**:
  - `Synchro Overtake` จาก 3 -> 2 (ลดโอกาสจั่วติดมือซ้ำที่เป็น HOPT)
  - `Ghost Belle & Haunted Mansion` จาก 3 -> 1
  - `Ghost Ogre & Snow Rabbit` จาก 2 -> 1
- **เพิ่มเข้า**:
  - `Mannadium Reframing` x1 (ปลดล็อก Omni-Negate ของ Visas Amritara)
  - `Synchro Emergency` x1 (เพิ่มเป้าหมายเสิร์ชให้ Duelist Genesis + ชุบ/ซิงโครจากสุสาน)
  - `Starjunk Synchron` x1 + `Jet Synchron` x1 (Extender เสริมความทนทานต่อการถูกขัด)
  - `Kewl Tune Mix` เพิ่มเป็น 3 ใบเต็ม (ตัวเสิร์ชและ Material Warrior สำหรับ Starjunk)
  - Extra Deck: เพิ่ม `Despian Luluwalilith` (`53971455`) มอนสเตอร์ซิงโครเลเวล 12 ที่ Negate มอนสเตอร์และบัฟ ATK +500 ทุกครั้งที่มีการ์ดออกจาก Extra Deck

### 2.2 โครงสร้างเด็ค Grave ใหม่ (`2026_Grave.ydk`)
ออกแบบสถาปัตยกรรมใหม่ **"Phase 1 K9 Swarm -> Phase 2 Necrovalley Lock"**:
- **ตัดออก 5 ใบ (การ์ดขยะ/การ์ดติดมือ)**:
  - `Gravekeeper's Chief` x1
  - `Gravekeeper's Shaman` x1
  - `Dark Renewal` x1
  - `Illusion Gate` x1
  - `Chaotic Elements` ลดเหลือ 1 ใบ
  - Extra Deck: ตัด `Infinitrack River Stormer` และ `Imperial Princess Quinquery`
- **เพิ่มเข้า (เร่งความสม่ำเสมอและพลังบอร์ดเต็มพิกัด)**:
  - `K9-66a Jokul` x3 (ตัวเริ่มหลัก: โชว์ตัวเอง + Lv5 เรียก 2 ตัวทันที + เสิร์ช K9)
  - `K9-04 Noroi` x3 (ตัวเริ่ม 1 ใบ: NS โดยไม่ต้องสังเวย + ดึง K9 ออกจากเด็ค)
  - `K9-66b Lantern` x3 (เสิร์ช K9 S/T + ชุบ)
  - `K9-X Forced Release` x2 (Quick-Play Rank-up เปลี่ยน Ripper เป็น Werewolf + ยิงการ์ดศัตรู 1 ใบ)
  - `Ash Blossom & Joyous Spring` x3 + `Infinite Impermanence` x3
  - Extra Deck: เพิ่ม `Artifact Durendal` (`69840739`) (แรงค์ 5 Quick Effect เปลี่ยนผลของศัตรูเป็นการยิงเวท/กับดัก) และ `K9-X "Werewolf"` x2 (บอสแรงค์ 9 ATK 3300 ริบการ์ดบนมือศัตรู)

---

## 3. การ Refactor ตรรกะ AI (`_2026_KwtuneExecutor.cs` & `_2026_GraveExecutor.cs`)

1. **`_2026_KwtuneExecutor.cs`**:
   - รองรับ Ash Blossom ทั้ง 2 รหัส (`14558128` และ `14558127`) ลงทะเบียนใน `HandTrapIds`, `ChainAdvisor`, และ `AddExecutor` อย่างสมบูรณ์
   - เพิ่มระบบความปลอดภัยในการเลือกซิงโครและเสิร์ชการ์ด

2. **`_2026_GraveExecutor.cs`**:
   - **ระบบ Delay Necrovalley**: ไม่รีบร้อนเปิด Necrovalley ในช่วงต้นเทิร์น แต่รอให้ Engine K9 (`Lantern`, `Jokul`, `Noroi`) ทำการ Special Summon และสร้าง `K9-17 Ripper` บนสนามให้เสร็จสิ้นก่อน แล้วจึงเปิดใช้ Necrovalley เพื่อล็อกสุสานศัตรู 100%
   - **การทำงานของบอสภายใต้ Necrovalley**:
     - `K9-17 Ripper`: ถอด 1 แมตทีเรียล Negate มอนสเตอร์ศัตรูในมือหรือสุสาน (ไม่แตะต้องสุสาน ไม่โดน Necrovalley ห้าม)
     - `K9-X Werewolf`: ถอด 1 แมตทีเรียลในเทิร์นศัตรู เพื่อ **รีมูฟการ์ดบนมือศัตรู 1 ใบแบบหงายหน้า** (ไม่แตะต้องสุสาน ทำงานได้ 100%)
     - `K9-X Forced Release`: แปลงร่าง Ripper เป็น Werewolf พร้อมทำลายการ์ดศัตรู
     - `Artifact Durendal`: เปลี่ยนผลของศัตรู ป้องกันการโดนทำลายบอร์ด
   - กำหนด Hint Constants ครบถ้วน: `506 (Search)`, `502 (Destroy)`, `503 (Remove)`, `509 (SpSummon)`, `513 (XyzMaterial)` ตามมาตรฐาน OCGCore พร้อมตรวจสอบ `c.Controller == 1` เสมอ

---

## 4. ผลการ Build และ Deployment

- **คอมไพล์ผ่าน .NET 10.0 SDK**: **0 Errors / 0 New Warnings**
- **สคริปต์ `BUILD_AND_DEPLOY.ps1`**:
  - Deploy `WindBot.dll`, `ExecutorBase.dll`, `core.dll`, `bots.json`, และไฟล์เด็ค `.ydk` ทั้งหมดไปยัง `C:\Users\admin\Documents\EdoGame\` ครบถ้วน 100%
