# Magical Cylinder Counter-Reflect & Forced-Attack OTK Deck Report

> **วันที่จัดทำ:** 30 กันยายน 2026  
> **เด็ค:** `Magical Cylinder` (`MagicalCylinder.ydk`)  
> **สถาปัตยกรรม:** C# .NET 10.0 / `ModernExecutor` + Decoupled Domain Plugin  
> **สถานะ:** Build สำเร็จ (0 Errors) & Deploy ไปยัง `C:\Users\admin\Documents\EdoGame\` เรียบร้อย

---

## 1. คอนเซ็ปต์และจุดเด่นของเด็ค (Core Mechanics)

เด็คนี้ถูกออกแบบมาเพื่อเป็น **"ฝันร้ายของเด็คบ้าพลัง"** ยิ่งมอนสเตอร์ฝ่ายตรงข้ามมีพลังโจมตีสูงเท่าไหร่ ยิ่งโดนสะท้อนดาเมจตายเร็วขึ้นเท่านั้น!

### 💥 หมัดเด็ดที่ 1: Double Damage Reflect (ดาเมจสะท้อน x2 ปิดเกมใน 1 นัด)
* **`Magical Cylinders` (15943341)**:
  * ในสนาม: เซ็ต `Magic Cylinder` 1 ใบตรงจากในเด็คหรือสุสานฟรี
  * ในสุสาน: เมื่อเราเปิดใช้งาน `Magic Cylinder` สามารถ **Banish การ์ดใบนี้จากสุสาน; ดาเมจสะท้อนจะกลายเป็น "2 เท่า" ทันที!**
* **`Magic Cylinder` (62279055)**:
  * เนเกตการโจมตี + ทำดาเมจใส่ศัตรูเท่ากับ ATK ของมอนสเตอร์ตัวนั้น
  * **ตัวอย่างการคำนวณดาเมจ**:
    * มอนสเตอร์ศัตรู ATK 3,000 โจมตี $\rightarrow$ โดน $3,000 \times 2 = \mathbf{6,000}$ ดาเมจ!
    * มอนสเตอร์ศัตรู ATK 4,000+ โจมตี $\rightarrow$ โดน **8,000+ ดาเมจ (One-Hit KO ชนะเกมทันที!)**

### 🛡️ หมัดเด็ดที่ 2: Non-Targeting Reflect (ทะลวงมอนสเตอร์ที่มีเกราะกันเล็งเป้า)
* **`Dimension Wall` (67095270)**:
  * โอน Battle Damage ทั้งหมดที่เราจะได้รับ ไปให้ฝ่ายตรงข้ามรับแทน
  * **จุดเด่น**: ไม่ได้เล็งเป้าหมายมอนสเตอร์ (Non-targeting) ต่อให้ศัตรูใช้มอนสเตอร์ที่มีเกราะป้องกันการตกเป็นเป้าหมาย (เช่น Chaos MAX, Dragoon) ก็โดนสะท้อนตายคาที่

### ⚔️ หมัดเด็ดที่ 3: Forced Attack Engine (บังคับให้ศัตรูต้องโจมตี)
* **`Battle Mania` (31245780)**:
  * เปิดใน Standby Phase ของศัตรู บังคับให้มอนสเตอร์ทุกตัวของฝ่ายตรงข้ามเปลี่ยนเป็น Face-up Attack และ **"ต้องสั่งโจมตีทุกตัวถ้าทำได้"** (ศัตรูหมดสิทธิ์เลี่ยงการต่อสู้ ต้องวิ่งชนกระบอกเวทมนตร์ตายเอง!)
* **`Kaiju Package` (`Jizukiru` ATK 3300 / `Dogoran` ATK 3000 / `Interrupted Kaiju Slumber`)**:
  * สังเวยบอสของศัตรู มอบ Kaiju ตัวยักษ์ให้ศัตรู แล้วบังคับให้มันตีมาโดนสะท้อน $3,300 \times 2 = 6,600$ ดาเมจ!

### 🏰 หมัดเด็ดที่ 4: Counter Fortress (แผงหลังคุ้มกันระดับ Spell Speed 3)
* **`Lord of the Heavenly Prison` (9822220) x3**:
  * ขณะอยู่บนมือ: การ์ดที่หมอบอยู่ทั้งหมด **ไม่มีวันถูกทำลายด้วยเอฟเฟกต์การ์ด** (Harpie's Feather Duster / Lightning Storm ยิงไม่พัง!)
  * เมื่อเปิดกับดัก: โดดลงมาเป็นกำแพง 3000 DEF และเสิร์ชกับดักมาหมอบฟรี
* **`Solemn Judgment` (41420027) x3**: Spell Speed 3 Omni-Negate สวนทุกล้างบอร์ด
* **`Dark Bribe` (77538567) x3**: Spell Speed 3 ป้องกันแผงหลังถูกยิง
* **`Solemn Strike` (40605147) x2**: สวนมอนสเตอร์และการ Special Summon

---

## 2. โครงสร้าง Decklist (40 Main / 15 Extra)

| กลุ่มการ์ด | รายชื่อการ์ด | จำนวน |
|---|---|:---:|
| **The Reflect Core** | `Magical Cylinders` (15943341)<br>`Magic Cylinder` (62279055)<br>`Dimension Wall` (67095270) | 3<br>3<br>3 |
| **Forced Attack Engine** | `Battle Mania` (31245780)<br>`Jizukiru, the Star Destroying Kaiju` (63941210)<br>`Dogoran, the Mad Flame Kaiju` (93332803)<br>`Interrupted Kaiju Slumber` (99330325) | 3<br>2<br>1<br>1 |
| **Consistency & Search** | `Trap Trick` (80101899)<br>`Lilith, Lady of Lament` (23898021)<br>`Pot of Duality` (98645731)<br>`Pot of Prosperity` (84211599)<br>`Wannabee!` (3248469) | 3<br>2<br>3<br>1<br>2 |
| **Counter Fortress** | `Lord of the Heavenly Prison` (9822220)<br>`Solemn Judgment` (41420027)<br>`Dark Bribe` (77538567)<br>`Solemn Strike` (40605147)<br>`Ring of Destruction` (83555666) | 3<br>3<br>3<br>2<br>2 |
| **Extra Deck** | `TY-PHON`, `AA-ZEUS`, `Gustav Max` (Rank 10 Burn 2000), `Juggernaut Liebe`, `S:P Little Knight`, `Knightmare Phoenix`, `Knightmare Unicorn`, `Abyss Dweller`, `Bagooska` | 15 |

---

## 3. สถาปัตยกรรมโค้ดและการทำงานของ AI

* **Executor**: [`MagicalCylinderExecutor.cs`](file:///C:/Users/admin/Documents/EdoGame/src/YGO_SOURCE_CLEAN/windbot-fork/Game/AI/Decks/MagicalCylinderExecutor.cs)
* **Domain Plugin**: `MagicalCylinderPlugin` (Strategy, MaterialEvaluator, ThreatEvaluator)
* **AI Decision Pipeline**:
  1. **Battle Step Trigger**: ตรวจสอบมอนสเตอร์ที่กำลังโจมตี ถ้ามี `Magical Cylinders` ในสุสาน บอทจะเปิด `Magic Cylinder` และสั่ง Banish `Magical Cylinders` เพื่อคูณ 2 ดาเมจทันที
  2. **Standby Phase Forced Attack**: ถ้าศัตรูมีมอนสเตอร์และเรามีกับดักสะท้อนหมอบอยู่ บอทจะเปิด `Battle Mania` ทันทีเพื่อบังคับให้ศัตรูต้องตี
  3. **Backrow Protection Priority**: ใช้ `Lord of the Heavenly Prison` ในมือเปิดเผยเพื่อสร้างเกราะป้องกันการทำลาย และใช้ `Solemn Judgment` / `Dark Bribe` สวนการ์ดพังบอร์ดทันที

---

## 4. สถานะการ Deploy และการทดสอบ

- ไบนารีชุดใหม่ (`WindBot.dll`, `ExecutorBase.dll`, `core.dll`, `dashbot.exe`) Deploy เรียบร้อยแล้ว
- บันทึกการลงทะเบียนบอทชื่อ `"Magical Cylinder"` ใน `bots.json`
- บันทึกประวัติการพัฒนาลงใน `PROGRESS.md` หัวข้อ `0.060`
- พร้อมสำหรับการนำไปใช้งานจริง หรือเปิดทดสอบด้วย Headless Text Duel เมื่อได้รับคำสั่ง
