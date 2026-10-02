# รายงานการวิเคราะห์ Log การดวลจริง และการแก้ไข Bug AI Memento (Forensic Duel Log Audit & Fix Report)

## 📌 ภาพรวม (Executive Summary)

ผู้ใช้ได้ทำการทดสอบบอทเด็ค **Memento** ด้วยตนเองในการดวลจริงกับ **Dark Magician** และได้ส่ง Log การตัดสินใจ (`[DEBUG-DECISION]`, `[DEBUG-IDLE]`, `[COMBO-FALLBACK]`) กลับมาให้ตรวจสอบ 

ผลการตรวจสอบ Log พบสาเหตุที่แท้จริงที่ทำให้ Memento แพ้และบอร์ดพังทั้งหมด 5 จุดสำคัญ:
1. **Critical Bug 1: Creation King ทำลายตัวเอง (Suicide Loop)** — Quick Effect สั่งยิงการ์ดฝ่ายตรงข้าม แต่เลือก `Creation King` เป็นเป้าทำลายฝั่งตนเอง ทำให้บอทไม่มีมอนสเตอร์เหลือบนสนาม
2. **Critical Bug 2: ไม่ยอมดัมพ์บอส Combined Creation (5000 ATK)** — Creation King ส่งการ์ด 3 ใบจากเด็ค/Extra Deck แต่ AI ไม่มี Handler สำหรับ Hint 504 จึงเลือกการ์ดมั่ว ไม่ยอมดัมพ์ `Mementoal Tecuhtlica - Combined Creation` (23288411) ส่งผลให้บอสไม่เคยถูกอัญเชิญเลยตลอดเกม
3. **Critical Bug 3: Tatsunootoshigo ดัมพ์ไม่ครบ (Empty Dump on 2nd Card)** — ทัตสึโนะโอโตชิโกะ (Lv5) ทำลายตัวเอง ต้องส่งมอนสเตอร์รวมเลเวล <= 5 แต่พอดัมพ์ Shleepy (Lv3) เสร็จ ในการเลือกใบที่สอง (`min=0, max=1`) AI กดยกเลิก ไม่ยอมส่ง `Ghattic` (Lv2) ทำให้ Ghattic ไม่ได้โดดและไม่ได้รีไซเคิลการ์ด
4. **Critical Bug 4: Forbidden Droplet เปลืองทรัพยากรผิดจังหวะ** — Droplet ทำงานใน Turn 2 ทั้งที่มอนสเตอร์ศัตรูคือ Windwitch - Glass Bell (1500 ATK) ธรรมดาที่ใช้เอฟเฟกต์เสร็จไปแล้ว และเอา `Mementotlan Fusion` หรือ `Super Polymerization` จากบนมือไปทิ้งเป็น Cost ทำให้คอมโบขาดมือ
5. **Critical Bug 5: Mementotlan Fusion กินมอนสเตอร์บนสนามแทนสุสาน** — เมื่อใช้ฟิวชั่น AI จัด Priority ให้มอนสเตอร์บนสนามคะแนนสูงกว่าสุสาน ทำให้เอา Angwitch และ Shleepy บนสนามไปเป็นวัตถุดิบ แทนที่จะเด้งมอนสเตอร์จากสุสานเข้าเด็ค

---

## 🔍 รายละเอียดการวิเคราะห์ Log (Root Cause Analysis)

### 1. Creation King Suicide Loop (Turn 2 & Turn 8)
- **Log Snippet**:
  ```text
  [Memento] [DEBUG-DECISION] OnSelectCard: min=1, max=1, hint=502... Options: 14529511 (MonsterZone), 46986414...
  [Memento] [DEBUG-DECISION] OnSelectCard -> Selected (Executor): 71007216 (ศัตรู)
  [Memento] [DEBUG-DECISION] OnSelectCard: min=1, max=1, hint=502... Options: 14529511 (MonsterZone)
  [Memento] [DEBUG-DECISION] OnSelectCard -> Selected (Executor): 14529511 (Creation King ตัวเอง!)
  ```
- **สาเหตุในโค้ดเดิม**:
  1. ใน `CreationKingActivate()`:
     ```csharp
     var friendlyPops = Bot.GetMonsters()
         .Where(m => m.IsFaceup() && !m.IsCode(CardId.MementoalTecuhtlica))
         .OrderBy(m => Plugin.MaterialEvaluator.GetDestructionPriority(m))
     ```
     ไม่ได้ยกเว้น `CreationKing` และใช้ `OrderBy` (จากน้อยไปมาก) ซึ่งคะแนน Creation King คือ `-8000` ทำให้ Creation King ถูกเลือกเป็นอันดับ 1!
  2. เมื่อไม่มีมอนสเตอร์ตัวอื่นในสนาม `CreationKingActivate()` ดัน `return true` ทำให้ Quick Effect สั่งยิง และถูกบังคับให้เลือกตัวเองทำลาย
- **การแก้ไข**:
  1. ใน `CreationKingActivate()`: ตรวจสอบ String ID (`Util.GetStringId(CardId.CreationKing, 2)`) และค้นหา `sacrificialFodder` โดย **ห้ามรวม Creation King และ Combined Creation เด็ดขาด** (`!m.IsCode(CardId.CreationKing) && !m.IsCode(CardId.MementoalTecuhtlica)`) ถ้าไม่มี Fodder อื่นในสนาม ให้ `return false` ทันที!
  2. ใน `OnSelectCard` (Hint 502/503): จัดลำดับด้วย `OrderByDescending` และกรองเอา Bosses ออกก่อนเสมอ

---

### 2. Creation King 3-Card Dump (Hint 504)
- **Log Snippet**:
  ```text
  [DEBUG-DECISION] OnSelectCard: min=3, max=3, hint=504... Options: ... 23288411 (Loc=Deck) ...
  [DEBUG-DECISION] OnSelectCard -> Selected: 17943271, 81677154, 80722024
  ```
- **สาเหตุ**:
  ใน `OnSelectCard` ไม่มีบล็อกดัก `hint == 504` ทำให้ระบบตกไปที่ Default Fallback ซึ่งหยิบใบที่เจอตามลำดับ โดยไม่สนใจ `Combined Creation` (23288411)
- **การแก้ไข**:
  เพิ่ม Handler สำหรับ `hint == 504 && min == 3 && max == 3`:
  1. ดัมพ์ **`Mementoal Tecuhtlica - Combined Creation` (23288411)** เป็นอันดับแรก (ถ้ายังไม่มีในมือ/สุสาน) เพื่อให้สามารถสเปเชียลจากสุสานได้ทันที!
  2. ดัมพ์ **`Mementotlan Ghattic` (52918032)** (ถ้ายังไม่มีในสุสาน/สนาม) เพื่อให้เอฟเฟกต์ Trigger ชุบตัวเองขึ้นสนามทันที + ดึงการ์ด Memento กลับมือ
  3. ดัมพ์ **`Twin Dragon` (19181420)** จาก Extra Deck หรือ Memento ชื่อที่ไม่ซ้ำ เพื่อให้ครบ 5 ชื่อในสุสาน

---

### 3. Tatsunootoshigo Lv5 Dump (Hint 504)
- **Log Snippet**:
  ```text
  [DEBUG-DECISION] OnSelectCard: min=1, max=1, hint=504... Selected: 50042011 (Shleepy Lv3)
  [DEBUG-DECISION] OnSelectCard: min=0, max=1, hint=504, cancelable=True... Selected: (empty)
  ```
- **สาเหตุ**:
  เมื่อทัตสึโนะส่ง Shleepy (Lv3) ไปแล้ว เลเวลที่เหลือคือ 2 (Lv5 - 3 = 2) ตัวเลือกรอบสองคือ `Ghattic` (Lv2), `Goblin` (Lv1), `Mace` (Lv1) แต่เพราะ `min=0` และไม่มี Hint 504 Handler ทำให้ `base.OnSelectCard` กดยกเลิก
- **การแก้ไข**:
  ใน `hint == 504`: ตรวจจับการส่งจาก Deck หากมี `Ghattic` (52918032) ให้เลือก Ghattic เสมอแม้ว่า `min == 0` ก็ตาม เพื่อให้ได้ Shleepy (Lv3) + Ghattic (Lv2) = 5 พอดี 100%

---

### 4. Forbidden Droplet Activation & Cost Priority
- **Log Snippet**:
  ```text
  Turn 2: Droplet ทิ้ง Mementotlan Fusion (66518509) หรือ Super Polymerization (48130397) เพื่อปิด Glass Bell 1500 ATK
  ```
- **สาเหตุ**:
  `ForbiddenDropletActivate()` เช็คแค่ `faceupEnemies.Count > 0` โดยไม่ดูว่าเป้าหมายคืออะไร และไม่มีการป้องกันคีย์การ์ดตอนทิ้ง Cost
- **การแก้ไข**:
  - เทิร์นตนเอง: ใช้ Droplet เฉพาะเมื่อศัตรูมีตัวลบล้าง (Known Negator), ปิดผนึก (Floodgate) หรือ ATK >= 2500 เท่านั้น
  - Cost: ให้คะแนนติดลบหนักกับการ์ดหลัก (`Mementotlan Fusion` -3000, `Bone Party` -3000, `Super Poly` -4000, `Bosses` -10000) และเลือกส่งเฉพาะการ์ดเวทสนามซ้ำ หรือตัวที่มีเอฟเฟกต์ในสุสาน (`Shleepy`, `Ghattic`, `Akihiron`)

---

### 5. Mementotlan Fusion Material Optimization (Hint 511)
- **สาเหตุ**:
  เอฟเฟกต์ของ `Mementotlan Fusion` เมื่อมีมอนสเตอร์ถูกทำลายในเทิร์นนั้น สามารถนำมอนสเตอร์ Memento ในสุสานสับกลับเข้าเด็คเป็นวัตถุดิบได้ฟรี แต่โค้ดเดิมให้คะแนนมอนสเตอร์บนสนามสูงกว่าสุสาน ทำให้บอทเลือกมอนสเตอร์บนสนามไปฟิวชั่นจนหมดตัว
- **การแก้ไข**:
  ใน `GetFusionMaterialScore`:
  - วัตถุดิบจากสุสาน (GY): ปรับคะแนนเป็น **5000** (สูงสุดรองจาก Super Poly) เพื่อรักษาตัวบนสนามไว้
  - วัตถุดิบจากมือ (Hand): 2000
  - วัตถุดิบจากสนาม (Field): 1000 (เก็บไว้บนสนามเป็น Disruptor หรือสละชีพให้ Creation King ยิง)

---

## 🚀 สรุปผลการ Deploy
- คอมไพล์โปรเจกต์ `WindBot.dll`, `ExecutorBase.dll`, `core.dll`, `dashbot` ผ่านฉลุย **0 Errors**
- รันสคริปต์ `BUILD_AND_DEPLOY.ps1` อัปเดตไฟล์ไปยัง `C:\Users\admin\Documents\EdoGame\` ครบถ้วน
