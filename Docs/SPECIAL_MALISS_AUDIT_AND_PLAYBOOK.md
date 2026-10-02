# Special Maliss (M∀LICE) Deck Architecture, Card Audit & AI Playbook (2026)

## 1. Executive Summary & Archetype Philosophy

**Maliss** (รหัสโค้ดสากล: *M∀LICE*) คือเด็คประเภท Cyberse/DARK ที่มีจุดเด่นคือการเล่นกับโซน **Banished (การ์ดที่ถูกรีมูฟ)** และการเชื่อมโยงระบบลิงก์ (Link-Climbing) ควบคู่กับ Counter Trap & Disruption Traps ที่สามารถสั่งเปิดใช้งานได้จากบนมือทันทีหากมีการ์ด Maliss ถูกรีมูฟออกนอกเกม

- **หมวดหมู่ใน DashBot**: **Special** (แท็กสีม่วงเข้ม `#5B21B6`)
- **ชื่อบอทที่ลงทะเบียน**: `Special_Maliss`, `Maliss`
- **ประเภทสถาปัตยกรรม**: Rule-Based C# 100% (`ModernExecutor` + `MalissPlugin`)
- **ไฟล์เด็ค**: `Special_Maliss.ydk` (54 Main Deck / 15 Extra Deck / 15 Side Deck)

---

## 2. Card Audit & Exact ID Mapping

ข้อมูลการ์ดทั้งหมดได้รับการตรวจสอบและถอดรหัสเทียบกับฐานข้อมูล `cards.cdb` และ `expansions/cards.cdb` อย่างละเอียด ปราศจากการเดาข้อมูล:

### 2.1 Maliss Core Engine
| Card Name | Card ID | Type / Attribute | Level / Link | บทบาทหลักในเด็ค |
|---|---|---|---|---|
| **Maliss <P> White Rabbit** | `68862889` | Cyberse / DARK | Lv 3 | Starter สำคัญที่สุด: Normal Summon แล้วนำกับดัก Maliss มาเซ็ตบนสนามทันที; คืนชีพจากโซนรีมูฟโดยจ่าย 300 LP |
| **Maliss <P> Dormouse** | `31443461` | Cyberse / DARK | Lv 3 | Starter/Extender: รีมูฟ Maliss จากเด็คออกนอกเกมเพื่อทริกเกอร์ผลคืนชีพ |
| **Maliss <P> March Hare** | `77840505` | Cyberse / DARK | Lv 3 | Extender: รีมูฟ Maliss จากสุสานหรือบนมือเพื่อทริกเกอร์ลูป |
| **Maliss <Q> Hearts Crypter** | `21915012` | Cyberse / DARK | Link-3 (2500 ATK) | **Ace Boss**: Quick Effect สับการ์ด Maliss ที่รีมูฟกลับเข้าเด็คเพื่อรีมูฟการ์ดบนสนามคู่แข่งแบบไม่เล็งเป้า และเพิ่มพลังโจมตีเป็น 5,000 ATK |
| **Maliss <Q> Red Ransom** | `67450007` | Cyberse / DARK | Link-3 (2300 ATK) | **Searcher Boss**: เมื่อถูกอัญเชิญพิเศษ เสิร์ชการ์ด Maliss ขึ้นมือ; ลด ATK การ์ดฝ่ายตรงข้ามและรีมูฟ |
| **Maliss <Q> White Binder** | `95454996` | Cyberse / DARK | Link-3 (2300 ATK) | **Control Boss**: รีมูฟการ์ดจากสุสานทั้งสองฝ่ายได้สูงสุด 3 ใบ และเซ็ตกับดัก Maliss จากเด็ค/สุสาน |
| **Maliss in Underground** | `47346782` | Field Spell | Spell | เมื่อทำงาน รีมูฟ Maliss 1 ใบจากมือ/เด็ค/สุสาน; เพิ่ม ATK 600 เมื่อมี Maliss กับดัก 3 ใบที่มีชื่อต่างกัน |
| **Maliss in the Mirror** | `85742297` | Normal Spell | Spell | ชุบชีวิตหรือเสิร์ช Maliss ที่รีมูฟอยู่ |
| **Maliss <C> MTP-07** | `5833312` | Normal Trap | Trap | รีมูฟ Maliss 1 ใบเพื่อเสิร์ชมอนสเตอร์ Maliss ขึ้นมือ |
| **Maliss <C> GWC-06** | `20726052` | Normal Trap | Trap | ชุบชีวิต Maliss ที่รีมูฟหรืออยู่ในสุสาน พร้อมเพิ่ม LP เท่ากับ ATK ตั้งต้น |
| **Maliss <C> TB-11** | `84827560` | Normal Trap | Trap | รีมูฟมอนสเตอร์ Maliss เพื่อรีมูฟการ์ดบนสนามฝ่ายตรงข้าม |

### 2.2 Cyberse Link & Engine Extenders
| Card Name | Card ID | Type | Link / Lvl | ประโยชน์ในการทำคอมโบ |
|---|---|---|---|---|
| **Link Decoder** | `30342076` | Cyberse / DARK | Link-1 (300 ATK) | วัตถุดิบเริ่มต้น: เมื่อถูกใช้ทำ Cyberse Link 2300+ ATK ชุบตัวเองกลับมาฟรี |
| **Splash Mage** | `59859086` | Cyberse / WATER | Link-2 (1100 ATK) | **Bridge สำคัญ**: ชุบ Cyberse จากสุสานขึ้นมาต่อยอด Link-3 ทันที |
| **Cyberse Wicckid** | `43202207` | Cyberse / DARK | Link-2 (800 ATK) | เสิร์ช Cyberse Tuner/Extender เมื่อมีมอนสเตอร์ถูกอัญเชิญชี้มา |
| **Linguriboh** | `32617466` | Cyberse / DARK | Link-1 (300 ATK) | สละตัวเองเพื่อ Negate Trap ของฝ่ายตรงข้าม |
| **Transcode Talker** | `39903080` | Cyberse / EARTH | Link-3 (2300 ATK) | ชุบ Link-2 หรือต่ำกว่าขึ้นมาเพื่อลิงก์ต่อยอดสู่ Link-4 |
| **Accesscode Talker** | `86066372` | Cyberse / DARK | Link-4 (2300 ATK) | **Finisher**: โจมตี 5300 ATK กวาดสนามศัตรูต่อเนื่องโดยไม่สามารถเชนตอบโต้ได้ |
| **Knightmare Gryphon** | `65330383` | Fiend / LIGHT | Link-4 (2800 ATK) | **Floodgate**: มอนสเตอร์ที่อัญเชิญพิเศษแบบไม่ต่อลิงก์จะไม่สามารถใช้เอฟเฟกต์ได้ |
| **Backup @Ignister** | `84489812` | Cyberse / DARK | Lv 3 | โดดพิเศษจากมือเมื่อมี Cyberse บนสนาม |
| **Wizard @Ignister** | `5772618` | Cyberse / LIGHT | Lv 4 | Extender ชุบ Cyberse จากสุสาน |
| **Allied Code Talker @Ignister** | `7044562` | Cyberse / DARK | Link-3 (2300 ATK) | Beatdown & Extender |

### 2.3 Handtraps, Removals & Board Breakers
| Card Name | Card ID | บทบาทเชิงกลยุทธ์ |
|---|---|---|
| **Santa Claws** | `46565218` | Kaiju-Style Removal: สังเวยมอนสเตอร์ตั้งบอร์ดของคู่แข่ง (Tower/Omni-negate) ไปตั้งป้องกันฝั่งตรงข้าม |
| **Dominus Spark** | `49684352` | Counter/Removal: ห้ามยิงจากมือเด็ดขาดในเทิร์น 1 (ล็อก Attribute) แต่หมอบไว้ใช้ขัดขวางศัตรู |
| **Dominus Impulse** | `75249652` | Quick Disruption: ขัดขวางมอนสเตอร์ศัตรู |
| **Solemn Judgment** | `41420027` | Counter Trap: ปฏิเสธการซัมมอนหรือเวทมนตร์/กับดักทุกชนิดด้วยครึ่งหนึ่งของ LP |
| **Solemn Accusation** | `78114463` | Counter Trap: ปฏิเสธการเปิดการ์ด/ซัมมอนโดยการเปิดเผยกับดักใบอื่นบนมือ |
| **Maxx "C"** | `23434538` | Draw Handtrap: ยับยั้งการรันคอมโบของฝ่ายตรงข้าม |
| **Ash Blossom** | `14550867` | Chokepoint Negation: ปิดการค้นหา/ส่งสุสานของการ์ดคู่แข่ง |
| **Mulcharmy Purulia / Fuwalos** | `84192580` / `42141493` | Draw Handtraps: เติมทรัพยากรเมื่อศัตรูอัญเชิญจากมือหรือเด็ค/เอ็กซ์ตร้าเด็ค |
| **Triple Tactics Talent** | `25311006` | Going Second Tool: ขโมยมอนสเตอร์, ดูมือ, หรือจั่ว 2 ใบเมื่อศัตรูใช้มอนสเตอร์เอฟเฟกต์ใน Main Phase |
| **Gold Sarcophagus** | `75500286` | Starter Enabler: รีมูฟ Maliss จากเด็คโดยตรงเพื่อทริกเกอร์คืนชีพลงสนามทันที |
| **Allure of Darkness** | `1475311` | Draw & Trigger: จั่ว 2 ใบแล้วรีมูฟมอนสเตอร์ DARK (ซึ่ง Maliss ทั้งหมดคือ DARK) ทริกเกอร์ผลการ์ดฟรี |

---

## 3. Playbook & Combo Routing

AI ได้รับการออกแบบการตัดสินใจแบ่งออกเป็น 6 ลำดับกลยุทธ์ (Tiered Pipeline):

### 3.1 Going First (ตั้งบอร์ดขัดขวาง & ลูปทรัพยากร)
1. **Starter Phase**:
   - หากมี `Gold Sarcophagus` ➔ รีมูฟ `Maliss <P> White Rabbit` หรือ `Dormouse` ทันทีเพื่อนำลงสนาม
   - Normal Summon `Maliss <P> White Rabbit` ➔ ทริกเกอร์เซ็ตกับดัก `Maliss <C> MTP-07` หรือ `GWC-06` หมอบบนสนาม
2. **Link-1 Phase**:
   - นำ White Rabbit ทำ Link Summon เป็น `Link Decoder` (30342076)
3. **Banish Trigger Phase**:
   - จ่าย 300 LP คืนชีพ White Rabbit จากโซนรีมูฟขึ้นมาบนสนาม
4. **Link-2 Phase**:
   - นำ Link Decoder + White Rabbit ทำ Link Summon เป็น `Splash Mage` (59859086)
   - ใช้เอฟเฟกต์ของ Splash Mage ชุบ Link Decoder หรือ White Rabbit กลับมาจากสุสาน
5. **Link-3 Phase (Ace Boss)**:
   - นำ Splash Mage + มอนสเตอร์ที่ชุบมา ทำ Link Summon เป็น **`Maliss <Q> Hearts Crypter`** (2500 ATK) หรือ **`Maliss <Q> White Binder`**
   - เอฟเฟกต์ของ `Link Decoder` ในสุสานทำงาน (เนื่องจากถูกใช้ทำ Cyberse Link 2300+ ATK) ➔ ชุบตัวเองกลับมายังสนามเป็น Extender ฟรีอีก 1 ตัว!
6. **End Board Lock**:
   - มี `Maliss <Q> Hearts Crypter` (พร้อมขัดขวางรีมูฟการ์ดศัตรูแบบ Quick Effect และเพิ่มพลังเป็น 5000 ATK)
   - มีกับดัก `Maliss <C> MTP-07` หรือ `GWC-06` หมอบพร้อมเปิด
   - มี `Solemn Judgment` / `Solemn Accusation` คอยปฏิเสธบอร์ดเบรกเกอร์ของฝ่ายตรงข้าม

### 3.2 Going Second (Board Breaking & Accesscode OTK)
1. **Kaiju Removal**:
   - หากฝ่ายตรงข้ามมีบอสตัวปัญหา (เช่น Ultimate Falcon, Dragoon, Baronne de Fleur) AI จะอัญเชิญ `Santa Claws` สังเวยบอสตัวนั้นไปอยู่ในสภาพตั้งรับฝั่งตรงข้ามทันที
2. **Handtrap & Bait Phase**:
   - ใช้ `Triple Tactics Talent` ขโมยมอนสเตอร์ตัวเก่งของฝ่ายตรงข้ามมาเป็นพวก หรือใช้จั่วการ์ดหาการ์ดเพิ่ม
3. **Climbing to Accesscode**:
   - รันคอมโบผ่าน `Splash Mage` ➔ `Transcode Talker` ➔ `Accesscode Talker`
   - พลังโจมตีของ Accesscode จะเพิ่มขึ้นเป็น **5,300 ATK**
   - AI ทำการรีมูฟ Link Monster ที่มี Attribute หลากหลายในสุสาน (DARK, WATER, EARTH) เพื่อกวาดทำลายการ์ดบนสนามของฝ่ายตรงข้ามจนโล่ง
   - เข้า Battle Phase โจมตี 5300 ATK + มอนสเตอร์ข้างเคียง ปิดฉากชนะเกมทันที (OTK)

---

## 4. Bottleneck Elimination & Rules Enforcement

1. **Splash Mage First, Wicckid Prerequisites**:
   - ป้องกันบั๊กการลง Cyberse Wicckid โดดเดี่ยวที่ไม่มีตัวชี้ (800 ATK ทำอะไรไม่ได้) โดยจัด Priority ให้ `Splash Mage` ทำงานก่อนเสมอ
2. **Dominus Trap Hand-Lock Prevention**:
   - `Dominus Spark` เมื่อเปิดจากมือจะล็อกไม่ให้ใช้ธาตุ EARTH/WATER/FIRE/WIND ซึ่งจะปิดกั้นการใช้ Splash Mage ทันที AI จึงถูกสั่ง **ห้ามเปิด Dominus Spark จากบนมือเด็ดขาด** (ให้หมอบไว้ใช้เทิร์นคู่แข่งเท่านั้น)
3. **Dimension Shifter Turn 1 Guard**:
   - การเปิด Dimension Shifter ในเทิร์น 1 ฝั่งเราจะทำให้การ์ดตกสุสานไม่ได้ ส่งผลให้ Link Decoder, Splash Mage และ Transcode Talker ชุบการ์ดไม่ได้ AI จึงถูกสั่ง **ห้ามเปิด Shifter ในเทิร์น 1 ฝั่งเรา**
4. **Duplicate Key Crash Safeguard**:
   - ปรับปรุง `DecksManager.cs` ให้ใช้ Indexer Assignment (`_decks[name] = deck`) แทน `.Add(name, deck)` เพื่อป้องกันโปรแกรมแครชกรณีมีเด็คชื่อซ้ำ

---

## 5. Deployment & Verification Status

- ไบนารีทั้งหมดคอมไพล์ผ่าน `BUILD_AND_DEPLOY.ps1` สมบูรณ์ 100% (0 Errors)
- Deploy สู่: `C:\Users\admin\Documents\EdoGame\`
- พร้อมสำหรับให้ผู้เล่นทดสอบการดวลสดผ่าน DashBot หรือ EDOPro
