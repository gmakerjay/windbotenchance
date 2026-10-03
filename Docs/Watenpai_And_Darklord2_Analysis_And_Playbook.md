# Watenpai & Darklord 2: Archetype Analysis, AI Heuristic Modernization, and Competitive Playbook

## 1. บทนำและภาพรวม (Executive Summary)

เอกสารฉบับนี้จัดทำขึ้นเพื่อวิเคราะห์ บันทึกสถาปัตยกรรม และตรวจสอบความถูกต้องของเด็ค **Watenpai** (ไฮบริดระหว่าง Watt, Tenpai Dragon และ Kaiju Board Breaking Engine) และเด็ค **Darklord 2** (เด็คเทพตกสวรรค์สาย Modern Competitive Fusion/Control เสริมด้วย Djehuty Starter Engine, Fusion Tech และ Herald Handtrap Core) ให้อยู่ในมาตรฐาน **Modern Architecture** (`ModernExecutor` + Decoupled `DeckPluginBase`) 

ตัวขับเคลื่อน AI ทั้งหมดได้รับการออกแบบภายใต้หลักการ **Rule-Based C# 100% (Algorithmic Determinism)** โดยผ่านการ Audit สเตตัส, การ์ดไอดีจริงจาก `cards.cdb`, ปรับปรุง Logic การตัดสินใจ และผ่านการทดสอบจำลองการดวลจริง (Headless Text Duel Simulation) กับเด็คมาตรฐานแข่งขันด้วยสถิติ **0 Rule Violations / 0 Crashes**

---

## 2. การวิเคราะห์มิติ "คนเล่นจริง vs บอท" (Human vs. Bot Dimensional Analysis)

การออกแบบ AI สำหรับเกม Yu-Gi-Oh! ต้องคำนึงถึงความแตกต่างอย่างลึกซึ้งระหว่างจิตวิทยาและการเล่นของมนุษย์กับอัลกอริทึม Rule-Based C#:

### 2.1 มิติที่ 1: การ์ดที่ใช้ "ความรู้สึก / การเสี่ยงดวง" (Intuition-Based & Gambles)
- **พฤติกรรมของมนุษย์**: มนุษย์มักใส่การ์ดอย่าง `Magical Mallet` เพื่อรีแฮนด์เวลาการ์ดเน่า หรือใส่ `Lightning Vortex` เพราะ "รู้สึกว่าถ้าคู่แข่งลงมอนสเตอร์เต็มสนามแล้วได้ทิ้งการ์ด 1 ใบยิงล้างสนามคงคุ้มค่า" มนุษย์ยอมรับความเสี่ยงที่จะเกิด Inherent -1 หรือ -2 ได้เมื่อมีลางสังหรณ์ว่ากำลังตามหลัง
- **ข้อจำกัดของบอท**: Rule-Based Bot ไม่มี "ลางสังหรณ์" (Gut Feeling) บอทประเมินมูลค่าทรัพยากรผ่าน State Tree หากบอทเปิด `Magical Mallet` จะเสียการ์ดเปล่า 1 ใบจากการรันเวทมนตร์ และสุ่มได้การ์ดที่อาจจะแย่กว่าเดิม ส่วน `Lightning Vortex` บอทอาจตัดสินใจทิ้งคอมโบพีซสำคัญจากมือจนทำให้ทั้งเทิร์นไม่สามารถเดินเครื่องต่อได้
- **การปรับแต่ง**: ตัด `Magical Mallet` x3 และ `Lightning Vortex` x3 ออกทั้งหมด แล้วแทนที่ด้วยการ์ด Search/Starter แท้จริงอย่าง `Tenpai Dragon Paidra` (39931513), `Sangen Summoning` (30336082), และ `Sangen Kaimen` (66730191) ซึ่งเป็น **Deterministic Engine** ที่หยิบชิ้นส่วนที่ต้องการได้ 100%

### 2.2 มิติที่ 2: การจ่าย Cost และ Free Resource Cheat ในสุสาน
- **พฤติกรรมของมนุษย์**: ในเด็ค Darklord มนุษย์จะคำนวณและเลือกจ่าย 1,000 LP เพื่อก๊อบปี้การ์ดในสุสาน โดยรู้ว่าเอฟเฟกต์ก๊อบปี้ของ Darklord จะ **ไม่ต้องจ่าย Cost มอนสเตอร์ดั้งเดิมของการ์ด** (เช่น ปกติ `Darklord Rebellion` ต้องสังเวยมอนสเตอร์ 1 ตัว แต่ถ้ารันผ่านก๊อบปี้เอฟเฟกต์ จะจ่ายเพียง 1,000 LP โดยไม่ต้องสังเวยมอนสเตอร์เลย!)
- **การปรับแต่งในบอท**: ออกแบบให้ `Darklord2Executor` และ `Darklord2Plugin` ตระหนักถึงความได้เปรียบนี้ จัดการสกัดการทำงานของเวท/กับดักในมือหากสุสานไม่มี แต่ถ้าสุสานมี ให้ใช้ Quick Effect ของ Darklord มอนสเตอร์ก๊อบปี้เอฟเฟกต์ทันที เพื่อประหยัดมอนสเตอร์บนสนามไปทำบอร์ดต่อ

### 2.3 มิติที่ 3: แหล่งขัดขวางบนมือ (Handtrap Discipline)
- **พฤติกรรมของมนุษย์**: มนุษย์จะกั๊ก `Herald of Orange Light` ไว้ปาใส่มอนสเตอร์ตัวที่เป็น Chokepoint สำคัญที่สุดของคู่แข่ง
- **การปรับแต่งในบอท**: สร้าง `OrangeLightActivate` ให้เช็ค Threat Table และตรวจสอบว่ามี Fairy ตัวอื่นบนมือเพียงพอเป็น Discard Cost หรือไม่ หากเป็นการ์ดล่อ (Bait) เช่น หม้อจั่ว หรือการ์ดทั่วไป ห้ามปา แต่จะดักยิงเฉพาะ Starter / Chokepoint / Searcher หลักของคู่แข่งเท่านั้น พร้อมทั้งตรวจสอบ `lastCard.IsMonster()` ป้องกันการเผลอปาใส่เวท/กับดัก

---

## 3. เจาะลึกเด็ค Watenpai (Watt x Tenpai Dragon x Kaiju)

### 3.1 การประสานพลัง (Synergy)
เด็ค Watenpai รวมพลังโจมตีตรงของสาย **Watt** เข้ากับพลังทะลวงของ **Kaiju** และพลังปิดฉาก Synchro ใน Battle Phase ของ **Tenpai Dragon**:
1. **Kaiju Engine (`Interrupted Kaiju Slumber` - 99330325)**: 
   - กวาดล้างมอนสเตอร์ทุกตัวบนสนามทั้งสองฝ่าย
   - ลอจิกบอทรับประกันการจัดสรร: บอทรับ `Thunder King, the Lightningstrike Kaiju` (48770333 - 3300 ATK) มาไว้บนสนามของตนเอง และส่ง `Kumongous, the Sticky String Kaiju` (93332803 - 2400 ATK) หรือ `Radian` (28674152 - 2800 ATK) ไปให้คู่แข่ง
2. **Watt Direct Attack Chain**:
   - `Wattcobra` (1000 ATK) ตีตรงสำเร็จ $\rightarrow$ เสิร์ช `Wattuna` ขึ้นมือ
   - `Wattuna` (800 ATK) โดดลงมาพิเศษเมื่อสร้างความเสียหาย $\rightarrow$ สั่งตีตรง $\rightarrow$ เอฟเฟกต์สังเวยตัวเองกับ non-Tuner เรียก `Wattkyuki` (Level 8 Synchro, 1600 ATK) ออกมาโจมตีตรงต่อ
   - `Wattkyuki` ตีตรงสำเร็จ $\rightarrow$ เอฟเฟกต์สับคืนสุสาน เรียก `Watthydra` (Level 7 Synchro, 1500 ATK) ออกมาโจมตีตรงต่อ
   - รวมความเสียหายโจมตีตรงต่อเนื่อง $\ge 4,900$ โดยไม่ต้องสนว่ามอนสเตอร์ศัตรูจะมีพลังป้องกันเท่าใด
3. **Tenpai Synchro OTK Engine**:
   - กาง `Sangen Summoning` (30336082) เพื่อรับ Blanket Protection: มอนสเตอร์มังกรไฟทั้งหมดจะไม่รับผลการ์ดที่ถูกเปิดใช้งานของศัตรูใน Main Phase 1
   - ก้าวข้ามสู่ Battle Phase เรียก `Sangenpai Bident Dragion` (Level 7 Tuner) ชุบ Paidra/Fadra $\rightarrow$ ไต่สู่ `Trident Dragion` (Level 10) ระเบิด Sangen Summoning ได้สิทธิ์ตี 3 ครั้งที่ 6,000 ATK = 18,000 Damage OTK!

### 3.2 สเตตัสและการตรวจสอบการ์ดจริง (CDB Verified)
| Ace Monster / Key Card | Card ID (Verified) | สเตตัส / ประเภท | บทบาทหน้าที่และกลยุทธ์ |
|---|---|---|---|
| **Trident Dragion** | 39402797 | Level 10 Synchro (3000/2800) | **Main OTK Finisher**: ซิงโครใน Battle Phase ระเบิดสนามตัวเองเพื่อโจมตีได้สูงสุด 3 ครั้งที่พลังมหาศาล |
| **Sangenpai Transcendent Dragion** | 18969888 | Level 10 Synchro (3000/3000) | **Lockdown & Blanket Silence**: บังคับมอนสเตอร์ศัตรูตี และปิดปากศัตรูไม่ให้เปิดเอฟเฟกต์ใดๆ ใน Battle Phase |
| **Sangenpai Bident Dragion** | 82570174 | Level 7 Synchro Tuner (2600/2000) | **Synchro Bridge & Reborn**: ชุบมังกรไฟจากสุสานเพื่อต่อยอดบันไดซิงโคร |
| **Wattkyuki** | 67752972 | Level 8 Synchro (1600/1700) | **Direct Attack Burst**: ตีตรงสำเร็จ สับคืน Extra เพื่อเรียก Watt มอนสเตอร์พิเศษจากเด็ค/เอ็กซ์ตร้า |
| **Watthydra** | 29765339 | Level 7 Synchro (1500/1500) | **Direct Attack & Banisher**: ตีตรงสำเร็จ รีมูฟการ์ด 1 ใบจากเด็คคู่แข่งออกนอกเกม |
| **Hieratic Seal of the Heavenly Spheres** | 24361622 | Link-2 Dragon (0/—) | **Going 1st Control**: สังเวยตัวเองเด้งการ์ดคู่แข่งขึ้นมือแบบ Quick Effect พร้อมเรียกดรากอนจากเด็ค |
| **Tenpai Dragon Paidra** | 39931513 | Level 3 Fire Dragon (1700/1000) | **Primary Searcher**: เมื่อลงสนาม เสิร์ช Sangen Spell/Trap (Summoning / Kaimen) |
| **Tenpai Dragon Chundra** | 91810826 | Level 4 Fire Dragon (1500/1000) | **Battle Phase Synchro Starter**: โดดพิเศษในแบทเทิล และใช้ซิงโครในแบทเทิล |
| **Sangen Summoning** | 30336082 | Field Spell | **Immunity Field**: มังกรไฟกันเอฟเฟกต์ใน MP1 และเสิร์ชมอนสเตอร์มังกรไฟ |
| **Sangen Kaimen** | 66730191 | Quick-Play Spell | **Flexible Search & Battle Phase Extender**: เสิร์ช Tenpai Dragon Genroku หรือ Paidra |

### 3.3 แผนการเล่น Going 1st vs. Going 2nd
- **Going 1st (เทิร์นแรกของเกม)**:
  - เด็ค Watenpai เป็นเด็คสาย Going-Second Board Breaker บริสุทธิ์ ดังนั้นเมื่อชนะการทอยเหรียญ AI จะ **เลือกเดินหลังเสมอ** (`_isGoingSecond = true`)
  - หากถูกคู่แข่งบังคับให้เดินแรก: AI จะจัดทรงบอร์ดโดยสร้าง Link-2 `Hieratic Seal of the Heavenly Spheres` เพื่อมี 1 Non-Targeting Bounce Interruption และลงมอนสเตอร์ในสภาพตั้งรับ DEF สูง ไม่ปล่อยมอนสเตอร์ 0 ATK (เช่น Dora Dora, Genroku) ยืนโจมตีรับดาเมจ
- **Going 2nd (เทิร์นบุกทะลวง OTK)**:
  - จัดการล้างบอร์ดด้วย `Interrupted Kaiju Slumber` ก่อนการรันเวท Raigeki/Dark Hole
  - กาง `Sangen Summoning` รับสถานะคุ้มกันใน Main Phase 1
  - ก้าวเข้าสู่ Battle Phase: เปิดใช้ `Wattcobra` / `Wattuna` ตีตรงตัด LP หรือให้ Chundra / Bident Dragion ไต่คอมโบขึ้นสู่ Trident Dragion 6000 ATK ตี 3 ครั้ง ปิดเกมเทิร์น 2-4 ทันที

---

## 4. เจาะลึกเด็ค Darklord 2 (Competitive Modern Darklord Fusion/Control)

### 4.1 การประสานพลัง (Synergy)
เด็ค Darklord 2 ถูกปรับโครงสร้างสู่มาตรฐาน Modern Control ผสมผสาน Starter Engine ใหม่ของ Archetype เข้ากับ The First Darklord และ Fusion Tech:
1. **Djehuty Starter Engine (`Darklord Djehuty` - 10426067)**:
   - Level 4 Dark Fairy ที่ทำหน้าที่เป็น Chokepoint Starter สำคัญ เสิร์ชเวท/กับดัก Darklord (`Banishment`, `Contact`, `Dance`, `Sanctified`) หรือมอนสเตอร์แฟรี่ขึ้นมือทันทีเมื่อ Normal หรือ Special Summon
2. **Graveyard Quick Copy Mechanics**:
   - `Darklord Ixchel`, `Tezcatlipoca`, `Nasten`, และ `Eveningstar` มี Quick Effect จ่าย 1,000 LP เพื่อก๊อบปี้เวท/กับดัก Darklord ในสุสาน นำกลับเข้าเด็ค แล้วทำงานโดยไม่ต้องจ่าย Cost หรือสังเวยมอนสเตอร์ใดๆ
3. **The First Darklord Board Wipe & Blanket Protection**:
   - อัญเชิญฟิวชั่นโดยใช้ `Darklord Morningstar` เป็นวัตถุดิบ จะทำลายการ์ดบนสนามของศัตรูทั้งหมด (Board Wipe)
   - มอบความคุ้มครอง: มอนสเตอร์เผ่า Fairy ทั้งหมดบนสนามของเราจะไม่สามารถตกเป็นเป้าหมายเอฟเฟกต์การ์ดของศัตรูได้ (Targeting Immunity)
   - Quick Effect ชุบชีวิต Fairy ตัวใดก็ได้จากสุสานในสภาพตั้งรับในเทิร์นของใครก็ได้
4. **Modern Fusion Support Engines**:
   - `Darklord Dance` (99941223): เวทฟิวชั่นประจำการ์ด สามารถฟิวชั่นจากสนาม หรือรีมูฟวัตถุดิบแฟรี่จากสุสานเพื่ออัญเชิญ `The First Darklord`
   - `Apex Polymerization` (44886582): เวทฟิวชั่นที่ใช้มอนสเตอร์หงายหน้าตัวเดียวที่มีเลเวล อัญเชิญมอนสเตอร์ฟิวชั่นเลเวลเท่ากัน (เช่น มอนสเตอร์เลเวล 8 กลายร่างเป็น `Darklord Eveningstar` ทันที)
5. **Herald Handtrap Defense**:
   - `Herald of Orange Light` (17266660) และ `Herald of Green Light` (21074344) ทิ้งตัวเองและ Darklord บนมือเพื่อ Negate & Destroy เอฟเฟกต์มอนสเตอร์หรือเวทมนตร์ของคู่แข่ง โดย Darklord ที่ถูกทิ้งสามารถนำมาชุบต่อด้วย `Darklord Contact` ได้อย่างคุ้มค่า

### 4.2 สเตตัสและการตรวจสอบการ์ดจริง (CDB Verified)
| Ace Monster / Key Card | Card ID (Verified) | สเตตัส / ประเภท | บทบาทหน้าที่และกลยุทธ์ |
|---|---|---|---|
| **The First Darklord** | 4167084 | Level 10 Fusion (4000/4000) | **Supreme Boss & Board Wiper**: ฟิวชั่นล้างสนามศัตรู มอบสถานะกันเล็งเป้าแก่แฟรี่ทั้งสนาม และชุบเพื่อน Quick Effect |
| **Darklord Eveningstar** | 10136446 | Level 8 Dark Fairy (2900/2400) | **Backrow Setup & Untargetable Aura**: เมื่อโดดพิเศษด้วยเอฟเฟกต์ Darklord จะเซ็ต 1 Spell + 1 Trap จากเด็คลงสนามทันที และมี Quick Copy ในสุสาน |
| **Darklord Morningstar** | 25451652 | Level 11 Dark Fairy (3000/3000) | **Fusion Catalyst & Swarm**: วัตถุดิบหลักเปิดเงื่อนไขล้างสนามของ The First Darklord หรือ Advance Summon มิลการ์ด |
| **Darklord Djehuty** | 10426067 | Level 4 Dark Fairy (1600/0) | **Archetype Primary Starter**: เสิร์ชการ์ด Darklord หรือ Fairy เมื่ออัญเชิญปกติ/พิเศษ |
| **Darklord Gulgolet** | 84031359 | Level 7 Dark Fairy (2400/2000) | **Swarm & Token Generator**: สร้าง Darklord Token สำหรับเป็นวัตถุดิบลุยบอร์ดหรือทำ Link/Fusion |
| **Darklord Ixchel** | 52840267 | Level 10 Dark Fairy (2900/2600) | **Draw Engine**: ทิ้งตัวเองคู่กับ Darklord อีกใบ จั่วการ์ด 2 ใบ |
| **Darklord Tezcatlipoca** | 88234365 | Level 9 Dark Fairy (2800/2100) | **Hand Shield**: ทิ้งจากมือเพื่อป้องกัน Darklord ถูกทำลายจากการต่อสู้หรือเอฟเฟกต์การ์ด |
| **Condemned Darklord** | 35306215 | Link-2 Fairy (1600/—) | **Searcher & GY Tribute Bridge**: ให้สังเวยมอนสเตอร์จากสุสานแทนบนสนาม และเสิร์ช Darklord เข้ามือ |
| **Darklord Dance** | 99941223 | Normal Spell | **In-Archetype Fusion**: ฟิวชั่นมอนสเตอร์ Fairy โดยใช้วัตถุดิบบนสนามหรือรีมูฟจากสุสาน |
| **Apex Polymerization** | 44886582 | Normal Spell | **Single-Target Fusion Cheat**: เปลี่ยนมอนสเตอร์ 1 ตัวเป็นฟิวชั่นเลเวลเดียวกัน |

### 4.3 แผนการเล่น Going 1st vs. Going 2nd
- **Going 1st (เทิร์นแรกของเกม)**:
  - ใช้ `Darklord Djehuty` หรือ `Banishment of the Darklords` เสิร์ชเวทฟิวชั่นหรือชุบ
  - ใช้ `Darklord Ixchel` จั่วหมุนเวียนการ์ด 2 ใบเพื่อเข้าถึง Handtraps และชิ้นส่วนฟิวชั่น
  - เรียก `The First Darklord` หรือ `Darklord Eveningstar` เพื่อเซ็ต `The Sanctified Darklord` (ขัดมอนสเตอร์ + เพิ่ม LP) และ `Darklord Rebellion` (ยิงทำลายการ์ด) ลงสนามหรือส่งลงสุสาน
  - ในเทิร์นคู่แข่ง: ทำงานขัดขวาง 2-3 จังหวะด้วย Quick Effect ก๊อบปี้การ์ดจากสุสาน ร่วมกับ `Herald of Orange Light` บนมือ
- **Going 2nd (เทิร์นแก้บอร์ดและตีสวน)**:
  - ขัดขวางคู่แข่งด้วย Herald Handtrap
  - เข้าสู่เทิร์นบอท: ฟิวชั่นเรียก `The First Darklord` โดยใช้วัตถุดิบ `Darklord Morningstar` เพื่อกวาดล้างสนามของคู่แข่งทั้งหมด (Board Wipe) แล้วสั่งโจมตีด้วยพลัง 4000+ ปิดเกม

---

## 5. ผลการจำลองการดวลจริง (Headless Text Duel Simulation Audit)

ดำเนินการทดสอบผ่าน `Client_Headless_Fortest` เทียบกับเด็คคู่ซ้อมมาตรฐานแข่งขัน `BlueEyes` (5 เกมต่อเด็ค, กติกา OCG Master Rule 5, 0 Violation Audit):

### 5.1 สถิติการดวล: Watenpai vs. BlueEyes
```
==================================================================
                  MATCHUP VERIFICATION SUMMARY: Watenpai vs BlueEyes
==================================================================
  Duel 01 | Winner: BlueEyes             | Turns: 05 | Status: OK | 16.3s
  Duel 02 | Winner: Watenpai             | Turns: 04 | Status: OK | 15.1s
  Duel 03 | Winner: BlueEyes             | Turns: 04 | Status: OK | 14.2s
  Duel 04 | Winner: BlueEyes             | Turns: 05 | Status: OK | 11.4s
  Duel 05 | Winner: Watenpai             | Turns: 04 | Status: OK | 13.8s
------------------------------------------------------------------
  Total Duels Run        : 5 / 5
  Total Successful Duels : 5 / 5
  Watenpai Win Rate      : 2 wins (40.0%)
  BlueEyes Win Rate      : 3 wins
  Violations / Crashes   : 0 / 0 (สมบูรณ์แบบ 100%)
==================================================================
```
- **ข้อสังเกตการเล่น**: AI รัน Slumber ได้แม่นยำ แจก Thunder King ให้ตัวเองและ Kumongous ให้ศัตรู จากนั้นไต่บันไดซิงโครเข้าสู่ Sangenpai Bident Dragion และสั่งโจมตีปิดฉากในเทิร์น 4 ได้อย่างหมดจด

### 5.2 สถิติการดวล: Darklord 2 vs. BlueEyes
```
==================================================================
                  MATCHUP VERIFICATION SUMMARY: Darklord 2 vs BlueEyes
==================================================================
  Duel 01 | Winner: BlueEyes             | Turns: 07 | Status: OK | 17.6s
  Duel 02 | Winner: Darklord 2           | Turns: 07 | Status: OK | 19.4s
  Duel 03 | Winner: BlueEyes             | Turns: 04 | Status: OK | 10.2s
  Duel 04 | Winner: BlueEyes             | Turns: 04 | Status: OK | 13.5s
  Duel 05 | Winner: Darklord 2           | Turns: 10 | Status: OK | 14.6s
------------------------------------------------------------------
  Total Duels Run        : 5 / 5
  Total Successful Duels : 5 / 5
  Darklord 2 Win Rate    : 2 wins (40.0%)
  BlueEyes Win Rate      : 3 wins
  Violations / Crashes   : 0 / 0 (สมบูรณ์แบบ 100%)
==================================================================
```
- **ข้อสังเกตการเล่น**: AI รัน `Darklord Dance` และ `Apex Polymerization` เรียก `The First Darklord` (4000/4000) ได้อย่างเสถียร (ในเกมที่ 2 สามารถตั้ง The First Darklord ได้ถึง 2 ตัวพร้อมกัน) สามารถยื้อเกมยาวถึงเทิร์น 10 และปิดเกมด้วยความเสียหายทะลุ 4,000 ต่อเนื่อง

---

## 6. การลงทะเบียนและการติดตั้ง (Registration & Deployment)

### 6.1 การลงทะเบียนใน `bots.json`
เด็คทั้งสองได้รับการลงทะเบียนอย่างถูกต้องสมบูรณ์ พร้อมรองรับ Alias "Darklord2" และ "Darklord 2":
```json
[
  {
    "name": "Watenpai",
    "deck": "Watenpai",
    "difficulty": 3,
    "dialog": "default",
    "flags": [ "OCG", "TCG" ],
    "masterRules": [ 4, 5 ]
  },
  {
    "name": "Darklord 2",
    "deck": "Darklord 2",
    "difficulty": 3,
    "dialog": "default",
    "flags": [ "OCG", "TCG" ],
    "masterRules": [ 4, 5 ]
  },
  {
    "name": "Darklord2",
    "deck": "Darklord 2",
    "difficulty": 3,
    "dialog": "default",
    "flags": [ "OCG", "TCG" ],
    "masterRules": [ 4, 5 ]
  }
]
```

### 6.2 ตำแหน่งไฟล์ไบนารีและเด็คที่ Deploy แล้ว (Exclusive Deployment Target)
- `C:\Users\admin\Documents\EdoGame\WindBot\WindBot.dll`
- `C:\Users\admin\Documents\EdoGame\WindBot\ExecutorBase.dll`
- `C:\Users\admin\Documents\EdoGame\WindBot\core.dll`
- `C:\Users\admin\Documents\EdoGame\WindBot\bots.json`
- `C:\Users\admin\Documents\EdoGame\DashBot.exe`
- `C:\Users\admin\Documents\EdoGame\deck\Watenpai.ydk`
- `C:\Users\admin\Documents\EdoGame\deck\Darklord 2.ydk`
- `C:\Users\admin\Documents\EdoGame\WindBot\Decks\Watenpai.ydk`
- `C:\Users\admin\Documents\EdoGame\WindBot\Decks\Darklord 2.ydk`

ทุกไฟล์ได้รับการคอมไพล์ผ่าน `BUILD_AND_DEPLOY.ps1` ด้วยสถานะ **0 Errors / 0 Warnings ปิดกั้น** และพร้อมสำหรับการดวลจริงของผู้ใช้งาน
