# Orcust WCQ (Modern Orcust WCQ) Audit & Playbook

## 1. Executive Summary & Architecture Overview
`OrcustWCQ` ได้รับการยกเครื่องโครงสร้างและอัปเกรดขนานใหญ่สู่สถาปัตยกรรม **Rule-Based ModernExecutor + Decoupled DeckPluginBase (100% C# Rules)** ร่วมกับการลงทะเบียนโมดูลส่วนกลางครบวงจร:
1. **แก้ปัญหาความฝืด (Anti-Bottleneck Engine)**:
   - ปรับเด็คจากเดิมที่มี Starter แท้จริงเพียง 4 ใบ (Girsu x3, Foolish x1) เพิ่มเป็น **11 Starter (โอกาสจั่วเจอ Starter ในมือแรก > 81.5%)**:
     - `Girsu, the Orcust Mekk-Knight` (3 ใบ)
     - `Scrap Recycler` (3 ใบ)
     - `Armageddon Knight` (1 ใบ)
     - `Dark Grepher` (2 ใบ) — ช่วยปลดล็อคมือ Brick ที่จั่วโดน Garnet เลเวลสูงอย่าง `Orcust Knightmare` หรือ `World Wand`
     - `Reinforcement of the Army` (1 ใบ)
     - `Foolish Burial` (1 ใบ)
2. **ขจัด Anti-Synergy & Dead Cards**:
   - ตัด `Dominus Spark` (3 ใบ) และ `Dominus Impulse` (3 ใบ) ที่ล็อคธาตุ Handtrap ทั้งเกมและแจก Special Summon ฟรีให้คู่แข่ง
   - ตัด `Mulcharmy` (5 ใบ) ที่กลายเป็นการ์ดตายเมื่อเริ่มก่อน
   - ตัด `Foolish Burial Goods` (3 ใบ) ที่ทำให้ Galatea ไม่มี Crescendo ให้หมอบ
3. **ป้องกันข้อผิดพลาดด้านกฎกติกา (Strict Rule Adherence)**:
   - **Galatea-i Xyz Ban**: ป้องกันไม่ให้ Dingirsu พยายาม Overlay ทับ `Galatea-i` (เนื่องจากการ์ดมีเงื่อนไขห้ามใช้เป็นวัตถุดิบ Xyz)
   - **Crescendo Field Active Condition**: ปรับลำดับการตั้ง End Board ให้มี Orcust Link Monster บนสนามเสมอ (เช่น `Longirsu` หรือชุบ `Galatea-i`) เพื่อให้ `Orcust Crescendo` สามารถเปิดใช้งานได้จริง
4. **ลงทะเบียนโมดูลส่วนกลางครบถ้วน**:
   - **`DeckPluginBase`**: `OrcustPlugin` สื่อสารผ่าน `IDeckStrategy`, `IDeckThreatEvaluator`, และ `IDeckMaterialEvaluator`
   - **`ComboRouter`**: ลงทะเบียน 4 สายคอมโบหลัก (`Girsu-FullCombo`, `Scrap-Recycler-Starter`, `Armageddon-Knight-Starter`, `DarkGrepher-Unbricker`)
   - **`BaitPlanner`**: ลงทะเบียนการ์ด Bait หลอกล่อ Handtrap (`RotA`, `Orcustrated Return`, `Triple Tactics Talent`, `Foolish Burial`)
   - **`HeuristicGuard` & `ResourcePlan`**: ปกป้อง Ace Cards ทั้งหมด
   - **`CardIntelligence`**: ลงทะเบียน Chokepoint และ Negator ของ Orcust ในฐานข้อมูล O(1) กลาง

---

## 2. Playbook & Combo Routing

### 🟢 Route A: Girsu 1-Card Full Control Board
1. Normal Summon `Girsu, the Orcust Mekk-Knight`
2. `Girsu` Effect 1: ส่ง `Orcust Harp Horror` จากเด็คลงสุสาน
3. `Girsu` Effect 2: สปอว์น `World Legacy Token` ลงสนามทั้งสองฝ่าย
4. นำ `Girsu` + `Token` ลิงก์เป็น **`Galatea, the Orcust Automaton`** (Link-2)
5. `Harp Horror` ในสุสาน รีมูฟตัวเอง Special Summon `Orcust Cymbal Skeleton` จากเด็ค
6. `Galatea` สับ `Harp Horror` ที่ถูกรีมูฟกลับเข้าเด็ค ➔ หมอบ **`Orcust Crescendo`** จากเด็ค
7. นำ `Galatea` โอเวอร์เลย์เป็น **`Dingirsu, the Orcust of the Evening Star`** (Rank 8)
8. `Dingirsu` Effect: ดึงมอนสเตอร์ Machine ที่ถูกรีมูฟกลับมาเป็นวัตถุดิบ Xyz
9. นำ `Dingirsu` + `Cymbal Skeleton` ลิงก์เป็น **`Longirsu`** (เพื่อคง Orcust Link ไว้เปิด Crescendo) หรือ **`I:P Masquerena`**
10. `Dingirsu` ลงสุสานพร้อมให้ `Cymbal Skeleton` ปลุกชีพขึ้นมาขัดจังหวะในเทิร์นศัตรู!

### 🟡 Route B: Scrap Recycler / Armageddon Knight 1-Card Line
1. Normal Summon `Scrap Recycler` (หรือ `Armageddon Knight`)
2. ส่ง `Orcust Harp Horror` จากเด็คลงสุสาน
3. `Harp Horror` ในสุสาน: รีมูฟตัวเอง Special Summon `Girsu` หรือ `Cymbal Skeleton`
4. นำ Starter + มอนสเตอร์ที่เรียกมา ลิงก์เป็น `Galatea` แล้วเดินคอมโบต่อเข้า Dingirsu + Crescendo

### 🟣 Route C: Dark Grepher Garnet-Unbricker
- หากมี `World Wand` หรือ `Orcust Knightmare` ติดบนมือ:
  - Special Summon `Dark Grepher` โดยทิ้งการ์ดเลเวล 5+ ดังกล่าว
  - หรือ Normal Summon `Dark Grepher` แล้วใช้เอฟเฟกต์ทิ้งมอนสเตอร์ DARK บนมือ ส่ง `Harp Horror` จากเด็กลงสุสาน
  - ปลดล็อคมือ Brick กลายเป็น Full Combo ทันที

### 🔵 Route D: Galatea-i Fallback Bridge (เมื่อมีเพียง Harp หรือ Cymbal ใบเดียว)
1. Normal Summon `Harp Horror` (หรือ `Cymbal Skeleton`)
2. ลิงก์เป็น **`Galatea-i` (Link-1)** เพื่อส่งตัวมันลงสุสาน
3. เอฟเฟกต์ในสุสานของ `Harp Horror` ทำงาน เรียกมอนสเตอร์จากเด็ค
4. รวมร่างกับ `Galatea-i` ทำเป็น `Galatea` (Link-2) แล้วขึ้น Dingirsu ได้ตามปกติ

---

## 3. Turn Opponent Disruption
- **เมื่อศัตรูเปิดเอฟเฟกต์**: `Crescendo` Negate & Banish ทันที (เพราะมี Orcust Link มอนสเตอร์คุมสนาม)
- **เมื่อศัตรูลงมอนสเตอร์หรือเข้าแบทเทิล**: `S:P Little Knight` หรือ `I:P Masquerena` ลิงก์ขัดจังหวะ
- **เมื่อมี `Orcustrated Babel`**: `Cymbal Skeleton` ในสุสานปลุกชีพ `Dingirsu` แบบ Quick Effect ส่งการ์ดศัตรูลงสุสานแบบไม่เล็งเป้าและไม่ทำลาย พร้อม Dingirsu ถอดวัตถุดิบกันการ์ดเราพังทั้งสนาม

---

## 4. Deployment Status
- ไบนารีทั้งหมดได้รับการ Deploy สู่ `C:\Users\admin\Documents\EdoGame\` ครบถ้วน:
  - `WindBot/WindBot.dll`
  - `WindBot/ExecutorBase.dll`
  - `WindBot/core.dll`
  - `WindBot/bots.json`
  - `DashBot.exe`
  - `deck/OrcustWCQ.ydk`

---

## 5. Self-Harm & Ace Protection Audit (การป้องกันบอททำร้ายตัวเอง)
จากการตรวจสอบ Log การดวลจริง พบบั๊กและพฤติกรรมทำร้ายตัวเอง (Self-Harm) 3 จุดสำคัญ และได้รับการแก้ไขอย่างเด็ดขาด 100%:

1. **บั๊ก Dingirsu ถูกนำไปทำ Galatea-i (บรรทัด 104-114 ใน duel.log)**:
   - *สาเหตุ*: `ShouldGalateaISpSummon` เดิมเช็คเพียง `!m.HasType(CardType.Link)` และจำนวนมอนสเตอร์บนสนาม <= 1 เมื่อมี Dingirsu ตัวเดียวบนสนาม ระบบจึงมองว่า Dingirsu เป็น non-link และถูกนำไปสังเวยทำ Galatea-i (Link-1 ATK 0)
   - *การแก้ไข*: สกัดกั้นทันทีหากบนสนามมี `Dingirsu`, `Galatea`, หรือ `Longirsu` และบังคับว่าวัตถุดิบต้องเป็น Main Deck Monster (`Level > 0` และไม่ใช่ Boss) เท่านั้น
2. **บั๊ก Galatea ทำ Galatea ซ้ำซ้อน (Self-Link Away)**:
   - *สาเหตุ*: เดิมเมื่อ Galatea ใช้เอฟเฟกต์แล้ว (`GalateaUsed = true`) เงื่อนไข Summon ไม่ได้รีเทิร์น false ทำให้บอทเอา Galatea ตัวแรกไปลิงก์ทำ Galatea ตัวที่สองโดยเปล่าประโยชน์
   - *การแก้ไข*: ใส่เงื่อนไขเด็ดขาด `if (Bot.HasInMonstersZone(CardId.Galatea)) return false;` และให้วัตถุดิบ Link ต้องเป็นมอนสเตอร์ส่วนเกิน (`!IsProtectedBoss(m)`) เท่านั้น
3. **บั๊ก Forbidden Droplet ส่งบอสลงสุสานซ้ำซ้อน (บรรทัด 369-378 ใน duel.log)**:
   - *สาเหตุ*: เมื่อศัตรูลง Sage with Eyes of Blue บอทเปิด Ash Blossom เนเกทไปแล้ว แต่ Droplet ทำงานต่อทันที (Double Negate ทับตัวเอง) และเลือกส่ง Galatea จากสนามเป็นคอสต์เนื่องจาก Hint 504 ขาดการกรองบอส
   - *การแก้ไข*:
     - เพิ่ม `if (Duel.LastChainPlayer == 0) return false;` ป้องกันการเปิด Droplet เชนทับการขัดจังหวะของตัวเอง
     - ปรับ Hint 504 และเงื่อนไข Droplet ให้ตัด `IsProtectedBoss(c)` ออก 100% ไม่สามารถส่งบอสหรือการ์ดสำคัญ (Babel / Crescendo) เป็นคอสต์ได้อีกเด็ดขาด

