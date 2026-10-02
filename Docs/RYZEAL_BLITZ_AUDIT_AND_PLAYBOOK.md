# RyzealBlizt (Ryzeal + Blitzclique) Architecture, Card Audit & AI Playbook (2026)

## 1. Executive Summary & Archetype Philosophy

**RyzealBlizt** (หรือ *Ryzeal Blitz*) เป็นเด็คคอมโบแบบไฮบริดระดับ Modern ที่ผสมผสานระหว่างสองกลยุทธ์อันทรงพลัง:
1. **Ryzeal Engine**: โครงสร้างมอนสเตอร์เลเวล 4 เผ่า Pyro/Thunder ที่เน้นการทำ Rank 4 Xyz Summon อย่างรวดเร็ว นำโดยบอสหลัก **`Ryzeal Detonator`** (3000 ATK) ที่มี Quick Effect ทำลายการ์ดบนสนามทุกครั้งที่ฝ่ายตรงข้ามเปิดใช้งานเอฟเฟกต์การ์ด
2. **Blitzclique Engine**: อาร์คิไทป์เผ่าสายฟ้า (Thunder) สายทำลายตัวเอง (Self-Destruction Trigger) เพื่อเปิดลูปการค้นหาและอัญเชิญพิเศษมอนสเตอร์สายฟ้าจากมืออย่างต่อเนื่อง พร้อมด้วยการ์ดขัดขวางและเคาน์เตอร์ระดับสูงอย่าง **`Whisker Blitzclique`** (Quick Negate มอนสเตอร์) และ **`Blitzclique Return Stroke`** (Quick Negate เวทมนตร์)
3. **Ultimate Slayer & Board Breaking Suite**: แพ็กเกจเคลียร์บอร์ดชั้นเลิศที่ใช้ Extra Deck ส่งตรงลงสุสานเพื่อเด้งมอนสเตอร์คู่แข่งกลับเด็คแบบเชนเอฟเฟกต์มอนสเตอร์ไม่ได้ ควบคู่กับ **`Santa Claws`**, **`Dinowrestler Pankratops`**, และ **`Illusion Gate`**

- **หมวดหมู่ใน DashBot**: **Modern** (แท็กสีน้ำตาลทอง `#C86600`)
- **ชื่อบอทที่ลงทะเบียน**: `RyzealBlizt`, `RyzealBlitz`
- **ประเภทสถาปัตยกรรม**: Rule-Based C# 100% (`ModernExecutor` + `RyzealBlitzPlugin`)
- **ไฟล์เด็ค**: `RyzealBlizt.ydk` (60 Main Deck / 15 Extra Deck / 0 Side Deck)

---

## 2. Card Audit & Exact ID Mapping

รายชื่อการ์ดทั้งหมด 75 ใบ (60 Main + 15 Extra) ได้รับการตรวจสอบและยืนยันเทียบกับฐานข้อมูล `cards.cdb` อย่างแม่นยำ:

### 2.1 Ryzeal Engine (Rank 4 Xyz Focus)
| Card Name | Card ID | Type / Attribute | Level / Rank | บทบาทหลักในเด็ค |
|---|---|---|---|---|
| **Ice Ryzeal** | `8633261` | Pyro / FIRE | Lv 4 (1700 ATK) | **Starter สำคัญ**: Normal Summon แล้วอัญเชิญพิเศษ Ryzeal จากเด็ค (Sword หรือ Ext); หรือทิ้งการ์ด 1 ใบจากมือ/สนามเพื่อโดดตัวเอง |
| **Sword Ryzeal** | `35844557` | Pyro / LIGHT | Lv 4 (1500 ATK) | **Extender / Searcher**: โดดพิเศษจากมือได้หากมี Ryzeal บนสนาม/สุสาน; เสิร์ช LIGHT Pyro ขึ้นมือเมื่อถูกอัญเชิญ |
| **Ext Ryzeal** | `34022970` | Thunder / FIRE | Lv 4 (500 ATK) | **Bridge สำคัญ**: เผ่าสายฟ้า (Thunder) เชื่อมสองเอนจิน! โดดพิเศษจากมือโดยส่ง Xyz จาก Extra Deck ลงสุสาน (ส่ง `Mereologic Aggregator` เพื่อ Negate การ์ดศัตรูฟรี) |
| **Ryzeal Detonator** | `34909328` | Pyro / FIRE | Rank 4 (3000 ATK) | **Ace Boss**: ใช้วัตถุดิบ Ryzeal Lv 4 2 ตัว; ติดการ์ดจากสุสานเป็นวัตถุดิบเมื่อซัมมอน; Quick Effect ถอดวัตถุดิบทำลายการ์ดบนสนาม 1 ใบเมื่อศัตรูใช้เอฟเฟกต์ |

### 2.2 Blitzclique Engine (Thunder Self-Destruct & Disruption)
| Card Name | Card ID | Type | Level / Role | บทบาทหลักในเด็ค |
|---|---|---|---|---|
| **Hideout in the Sky, Coulomb** | `37654623` | Field Spell | Spell | อัญเชิญโทเค่น 2000 ATK ไปสนามศัตรู แล้วเสิร์ช Blitzclique ขึ้นมือ; เด้งกลับขึ้นมือจากสุสานเมื่อการ์ดถูกทำลายด้วย Blitzclique |
| **Blitzclique - Steppleader** | `433377` | Continuous Spell | Spell | สั่งโดดมอนสเตอร์ Thunder จากมือแล้วทำลายตัวเอง ทริกเกอร์ลูปการทำลายของ Blitzclique ทันที |
| **Surge Blitzclique** | `22912101` | Thunder / LIGHT | Lv 4 (1800 ATK) | เผยจากมือทำลายมอนสเตอร์ 1 ตัวบนสนาม (หรือโทเค่น Coulomb) แล้วโดด Thunder; เมื่อมีการ์ดถูกทำลาย เสิร์ชมอนสเตอร์ Blitzclique |
| **Grain Blitzclique** | `84401954` | Thunder / LIGHT | Lv 2 (1200 ATK) | เผยจากมือทำลายเวท/กับดักบนสนาม แล้วโดด Thunder; เมื่อมีการ์ดถูกทำลาย เสิร์ชเวท Blitzclique หรือ Coulomb |
| **Crackle Blitzclique** | `58916810` | Thunder / LIGHT | Lv 3 (1500 ATK) | Quick Handtrap ในเทิร์นศัตรู: เมื่อศัตรูใช้มอนสเตอร์เอฟเฟกต์บนสนาม เผยตัวทำลายมอนสเตอร์นั้นแล้วโดด Thunder จากมือ |
| **Whisker Blitzclique** | `85523502` | Thunder / LIGHT | Lv 8 (3000 ATK) | **Boss Disruption**: โดดพิเศษ Thunder 3 ตัวจากมือแล้วทำลายการ์ด; Quick Effect เด้ง Thunder 1 ตัวกลับมือเพื่อปฏิเสธเอฟเฟกต์มอนสเตอร์และทำลาย! |
| **Emi Blitzclique** | `11895663` | Thunder / LIGHT | Lv 1 (900 ATK) | เมื่อขึ้นมือ ทำลายการ์ดบนสนาม 1 ใบแล้วโดด Thunder; เสิร์ชกับดัก Blitzclique เมื่อมีการ์ดถูกทำลาย |
| **Blitzclique Return Stroke** | `23526128` | Continuous Trap | Trap | ป้องกัน Blitzclique ถูกทำลาย; Quick Effect เด้ง Blitzclique กลับมือเพื่อ Negate และทำลายการ์ดเวทมนตร์ของคู่แข่ง |
| **Blitzclique Alternator** | `59921227` | Continuous Trap | Trap | ดีบัฟพลังศัตรู -300 ATK/DEF ต่อ Thunder; สับการ์ดในมือ/สนามกลับเด็คเพื่อเสิร์ช Thunder |
| **Blitzclique - Breakaway** | `64049762` | Quick-Play Spell | Spell | วาง Continuous Trap บนสนามจากเด็คโดยตรง หรือเด้ง Thunder เพื่อทำลายการ์ด |

### 2.3 Extra Deck Suite (15 Cards)
| Card Name | Card ID | Type | ATK / DEF | บทบาทการใช้งาน |
|---|---|---|---|---|
| **Ryzeal Detonator** | `34909328` | Xyz Rank 4 | 3000 / 2500 | Main Ace: Quick pop เมื่อศัตรูขยับ + กันทำลาย |
| **Number 106: Giant Hand** | `63746411` | Xyz Rank 4 | 2000 / 2000 | Secondary Ace: Quick negate มอนสเตอร์เอฟเฟกต์ |
| **Number 60: Dugares the Timeless** | `66011101` | Xyz Rank 4 | 1200 / 1200 | Utility: จั่ว 2 ทิ้ง 1 หรือชุบชีวิต หรือเพิ่มพลังโจมตี 2 เท่าสำหรับ OTK |
| **Mereologic Aggregator** | `9940036` | Xyz Rank 9 | 2600 / 3000 | Extra Dump Target: เมื่อตกสุสาน เล็ง Negate การ์ดที่หงายหน้า 1 ใบบนสนามทันที |
| **Garura, Wings of Resonant Life** | `11765832` | Fusion Lv 6 | 1500 / 2400 | Extra Dump Target: จั่วการ์ด 1 ใบเมื่อตกสุสาน |
| **Golden Cloud Beast - Malong** | `93125329` | Synchro Lv 6 | 2200 / 1000 | Extra Dump Target: เด้งการ์ดบนสนาม 1 ใบกลับมือเมื่อตกสุสาน |
| **Wind Pegasus @Ignister** | `98506199` | Synchro Lv 7 | 2300 / 1500 | Extra Dump Target: สับการ์ดศัตรูกลับเด็คเมื่อการ์ดเราถูกทำลาย |
| **PSY-Framelord Omega** | `74586817` | Synchro Lv 8 | 2800 / 2200 | Extra Dump Target: รีไซเคิลการ์ดจากสุสานกลับเด็ค |
| **Enigmaster Packbit** | `72444406` | Synchro Lv 8 | 2900 / 2500 | Extra Dump / Synchro Option |
| **Borreload Savage Dragon** | `27548199` | Synchro Lv 8 | 3000 / 2500 | Omni-Negate Boss |
| **Topologic Blaster Dragon** | `30064423` | Link-4 | 3500 / - | Field Wipe Link Boss |
| **Borrelcode Dragon** | `67288539` | Link-3 | 2500 / - | Link & GY Banish Trigger |
| **Vallon, the Super Psy Skyblaster** | `40673853` | Xyz Rank 5 | 2500 / 2200 | Quick คว่ำมอนสเตอร์ศัตรู |
| **Divine Arsenal AA-ZEUS - Sky Thunder** | `90448279` | Xyz Rank 12 | 3000 / 3000 | Board Wipe ใน MP2 หลังการต่อสู้ |
| **Super Starslayer TY-PHON - Sky Crisis** | `93039339` | Xyz Rank 12 | 2900 / 2900 | Floodgate ปิดเอฟเฟกต์มอนสเตอร์ 3000+ ATK + เด้งการ์ด |

---

## 3. Playbook & Strategic Routing

### 3.1 Going First Strategy (Control & Multi-Interruption Board)
1. **Setup Phase**:
   - เปิดใช้งาน `Terraforming` เสิร์ช `Hideout in the Sky, Coulomb`
   - เปิดใช้งาน `Coulomb` สร้างโทเค่นให้คู่แข่ง เสิร์ช `Surge Blitzclique` หรือ `Whisker Blitzclique` ขึ้นมือ
   - เปิดใช้งาน `Blitzclique - Steppleader` เพื่อโดดมอนสเตอร์ Thunder แล้วทำลายตัวเอง ➔ ดึง `Coulomb` ในสุสานกลับขึ้นมือ
2. **Ryzeal Rank 4 Climb**:
   - Normal Summon `Ice Ryzeal` ➔ เอฟเฟกต์อัญเชิญ `Sword Ryzeal` หรือ `Ext Ryzeal` จากเด็ค
   - หากมี `Ext Ryzeal` บนมือ ➔ สั่งโดดพิเศษโดยส่ง `Mereologic Aggregator` (9940036) จาก Extra Deck ลงสุสาน
   - ทริกเกอร์ `Ext Ryzeal` ค้นหามอนสเตอร์ FIRE Thunder ขึ้นมือ
3. **End Board Lock**:
   - นำมอนสเตอร์ Level 4 2 ตัว Overlay เป็น **`Ryzeal Detonator`** (3000 ATK)
   - นำมอนสเตอร์ Level 4 อีก 2 ตัว (เช่น Surge Blitzclique) Overlay เป็น **`Number 106: Giant Hand`** (2000 ATK)
   - หมอบ `Blitzclique Return Stroke` (Negate Spell) และ `Solemn Judgment`
   - ถือ `Whisker Blitzclique` บนมือ (Quick Monster Negate)
   - ถือ `Crackle Blitzclique` บนมือ (Quick Monster Pop & SS)
   - ผลลัพธ์: การขัดขวาง 5-6 จังหวะที่ครอบคลุมทั้ง Monster, Spell, Trap และการทำลายสนาม

### 3.2 Going Second Strategy (Board Breaking & OTK)
1. **Tribute Removal**:
   - สังเวยบอสของคู่แข่งด้วย **`Santa Claws`** ไปอยู่ในสภาพตั้งรับ
2. **Unchainable Extra Removal**:
   - รัน **`Ultimate Slayer`** เล็งเป้าหมายมอนสเตอร์ประเภท Extra Deck ของคู่แข่ง:
     - เล็ง Fusion ➔ ส่ง `Garura` (จั่ว 1)
     - เล็ง Synchro ➔ ส่ง `Golden Cloud Beast - Malong` (เด้งการ์ดบนสนามอีก 1 ใบ)
     - เล็ง Xyz ➔ ส่ง `Mereologic Aggregator` (Negate การ์ดที่หงายหน้าบนสนาม 1 ใบ)
3. **Mass Monster Wipe**:
   - หากคู่แข่งมีมอนสเตอร์เต็มสนาม จ่ายครึ่งหนึ่งของ LP เปิด **`Illusion Gate`** ทำลายมอนสเตอร์ทั้งหมด แล้วขโมยมอนสเตอร์ตัวเก่งจากสุสานคู่แข่งมาเป็นพวก
4. **OTK Execution**:
   - ซัมมอน `Ryzeal Detonator` (3000 ATK)
   - ซัมมอน `Number 60: Dugares the Timeless` ➔ เลือก Option 2 (Double ATK) บัฟพลังให้ Detonator เพิ่มเป็น **6,000 ATK**
   - โจมตีปิดเกมชนะทันที (OTK) หรือ Overlay ทับด้วย `Divine Arsenal AA-ZEUS` ใน MP2 เพื่อล้างสนาม

---

## 4. Bottleneck Elimination & Rules Enforcement

1. **Card Location & Target Verification**:
   - แก้ไขการตรวจสอบพื้นที่การ์ดให้ตรงตามโครงสร้าง WindBot (`HasInSpellZone`, `CardLocation.Overlay`)
   - ป้องกันการเล็งเป้าหมายการ์ดฝั่งเดียวกันเองในเอฟเฟกต์ทำลายของการ์ดศัตรู
2. **Ext Ryzeal Extra Deck Choice**:
   - บังคับลำดับความสำคัญในการทิ้ง Extra Deck ของ Ext Ryzeal: ส่ง `Mereologic Aggregator` ก่อนเสมอเพื่อให้ได้ผลลัพธ์ Negate สนามฟรี 1 ใบ
3. **Dugares Decision Route**:
   - หากตรวจพบว่าสามารถปิดเกมได้ (OTK) ให้เลือก Option 2 (Double ATK) ทันที มิฉะนั้นให้เลือก Option 0 (จั่ว 2 ทิ้ง 1) เพื่อเร่งหาการ์ด

---

## 5. Deployment & System Integration

- **ไฟล์เด็ค**: บันทึก `RyzealBlizt.ydk` และ `RyzealBlitz.ydk` ไปยัง `windbot-fork/Decks/`, `WindBot/Decks/`, และ `deck/`
- **การลงทะเบียนบอท**: ลงทะเบียน `RyzealBlizt` และ `RyzealBlitz` ใน `bots.json`
- **DashBot UI**: เพิ่มเข้าสู่ `ModernArchetypes` แสดงผลในหมวด **Modern** ด้วยสีแท็ก `#C86600`
- **สถานะการคอมไพล์และ Deploy**: สำเร็จ 100% ผ่าน `BUILD_AND_DEPLOY.ps1` (0 Errors) สู่ `C:\Users\admin\Documents\EdoGame\`
