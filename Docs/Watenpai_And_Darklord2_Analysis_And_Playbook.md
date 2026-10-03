# Watenpai & Darklord 2: Archetype Analysis, AI Heuristic Modernization, and Competitive Playbook

## 1. บทนำและภาพรวม (Executive Summary)

เอกสารฉบับนี้จัดทำขึ้นเพื่อวิเคราะห์และบันทึกสถาปัตยกรรมการปรับแต่งเด็ค **Watenpai** (ไฮบริดระหว่าง Watt, Tenpai Dragon และ Kaiju Engine) และเด็ค **Darklord 2** (เด็คเทพตกสวรรค์สาย Competitive Darklord Fusion/Control ผสม Herald Handtrap Engine) ให้อยู่ในมาตรฐาน **Modern Architecture** (`ModernExecutor` + Decoupled `DeckPluginBase`) โดยมุ่งเน้นการชั่งน้ำหนักระหว่าง **"ผู้เล่นมนุษย์ (Human Intuition)"** กับ **"บอท Rule-Based C# (Algorithmic Determinism)"** เพื่อรีแฟกเตอร์เด็คและสร้างตัวขับเคลื่อน AI ที่เสถียรและทรงพลัง 100%

---

## 2. การวิเคราะห์มิติ "คนเล่นจริง vs บอท" (Human vs. Bot Dimensional Analysis)

การออกแบบ AI สำหรับเกม Yu-Gi-Oh! ต้องเข้าใจความแตกต่างอย่างลึกซึ้งระหว่างจิตวิทยาของมนุษย์กับอัลกอริทึมของคอมพิวเตอร์:

### 2.1 มิติที่ 1: การ์ดที่ใช้ "ความรู้สึก / การเสี่ยงดวง" (Intuition-Based & Gambles)
- **พฤติกรรมของมนุษย์**: มนุษย์มักใส่การ์ดอย่าง `Magical Mallet` เพื่อรีแฮนด์เวลาการ์ดเน่า หรือใส่ `Lightning Vortex` เพราะ "รู้สึกว่าถ้าคู่แข่งลงมอนสเตอร์เต็มสนามแล้วได้ทิ้งการ์ด 1 ใบยิงล้างสนามคงคุ้มค่า" มนุษย์ยอมรับความเสี่ยงที่จะเกิด Inherent -1 หรือ -2 ได้เมื่อมีลางสังหรณ์ว่ากำลังตามหลัง
- **ข้อจำกัดของบอท**: Rule-Based Bot ไม่มี "ลางสังหรณ์" (Gut Feeling) บอทประเมินมูลค่าทรัพยากรผ่าน State Tree หากบอทเปิด `Magical Mallet` จะเสียการ์ดเปล่า 1 ใบจากการรันเวทมนตร์ และสุ่มได้การ์ดที่อาจจะแย่กว่าเดิม ส่วน `Lightning Vortex` บอทอาจตัดสินใจทิ้งคอมโบพีซสำคัญจากมือจนทำให้ทั้งเทิร์นไม่สามารถเดินเครื่องต่อได้
- **การปรับแต่ง**: ตัด `Magical Mallet` x3 และ `Lightning Vortex` x3 ออกทั้งหมด แล้วแทนที่ด้วยการ์ด Search/Starter แท้จริงอย่าง `Tenpai Dragon Paidra` (39931513), `Sangen Summoning` (15848542), และ `Sangen Kaimen` (66730191) ซึ่งเป็น **Deterministic Engine** ที่หยิบชิ้นส่วนที่ต้องการได้ 100%

### 2.2 มิติที่ 2: การจ่าย Cost และ Free Resource Cheat ในสุสาน
- **พฤติกรรมของมนุษย์**: ในเด็ค Darklord มนุษย์จะคำนวณและเลือกจ่าย 1,000 LP เพื่อก๊อบปี้การ์ดในสุสาน โดยรู้ว่าเอฟเฟกต์ก๊อบปี้ของ Darklord จะ **ไม่ต้องจ่าย Cost มอนสเตอร์ดั้งเดิมของการ์ด** (เช่น ปกติ `Darklord Rebellion` ต้องสังเวยมอนสเตอร์ 1 ตัว แต่ถ้ารันผ่านก๊อบปี้เอฟเฟกต์ จะจ่ายเพียง 1,000 LP โดยไม่ต้องสังเวยมอนสเตอร์เลย!)
- **การปรับแต่งในบอท**: ออกแบบให้ `Darklord2Executor` และ `Darklord2Plugin` ตระหนักถึงความได้เปรียบนี้ จัดการสกัดการทำงานของเวท/กับดักในมือหากสุสานไม่มี แต่ถ้าสุสานมี ให้ใช้ Quick Effect ของ Darklord มอนสเตอร์ก๊อบปี้เอฟเฟกต์ทันที เพื่อประหยัดมอนสเตอร์บนสนามไปทำบอร์ดต่อ

### 2.3 มิติที่ 3: แหล่งขัดขวางบนมือ (Handtrap Discipline)
- **พฤติกรรมของมนุษย์**: มนุษย์จะกั๊ก `Herald of Orange Light` ไว้ปาใส่มอนสเตอร์ตัวที่เป็น Chokepoint สำคัญที่สุดของคู่แข่ง
- **การปรับแต่งในบอท**: สร้าง `OrangeLightActivate` ให้เช็ค Threat Table และตรวจสอบว่ามี Fairy ตัวอื่นบนมือเพียงพอเป็น Discard Cost หรือไม่ หากเป็นการ์ดล่อ (Bait) เช่น หม้อจั่ว หรือการ์ดทั่วไป ห้ามปา แต่จะดักยิงเฉพาะ Starter / Chokepoint / Searcher หลักของคู่แข่งเท่านั้น

---

## 3. เจาะลึกเด็ค Watenpai (Watt x Tenpai Dragon x Kaiju)

### 3.1 การประสานพลัง (Synergy)
เด็ค Watenpai รวมพลังโจมตีตรงของสาย **Watt** เข้ากับพลังทะลวงของ **Kaiju** และพลังปิดฉาก Synchro ใน Battle Phase ของ **Tenpai Dragon**:
1. **Kaiju Engine (`Interrupted Kaiju Slumber`)**: กวาดมอนสเตอร์ทุกตัวบนสนาม แจก Kumongous (2400 ATK) ให้ศัตรู และบอทได้ Jizukiru (3300 ATK) บนสนามตัวเอง
2. **Watt Direct Attack Chain**:
   - `Wattcobra` (1000 ATK) ตีตรงสำเร็จ $\rightarrow$ เสิร์ช `Wattuna` ขึ้นมือ
   - `Wattuna` (800 ATK) โดดลงมาพิเศษเมื่อสร้างความเสียหาย $\rightarrow$ สั่งตีตรง $\rightarrow$ เอฟเฟกต์สังเวยตัวเองกับ non-Tuner เรียก `Wattkyuki` (Level 8 Synchro, 1600 ATK) ออกมาโจมตีตรงต่อ
   - `Wattkyuki` ตีตรงสำเร็จ $\rightarrow$ เอฟเฟกต์สับคืนสุสาน เรียก `Watthydra` (Level 7 Synchro, 1500 ATK) ออกมาโจมตีตรงต่อ
   - รวมความเสียหายโจมตีตรงต่อเนื่อง $\ge 4,900$ โดยไม่ต้องสนว่ามอนสเตอร์ศัตรูจะมีพลังป้องกันเท่าใด
3. **Tenpai Synchro OTK Engine**:
   - หากศัตรูมีมอนสเตอร์ขวาง หรือเปิด `Sangen Summoning` ได้ มอนสเตอร์มังกรไฟทั้งหมดจะไม่รับผลการ์ดที่ถูกเปิดของศัตรูใน MP1
   - ก้าวข้ามสู่ Battle Phase เรียก `Sangenpai Bident Dragion` (Level 7 Tuner) ชุบ Paidra/Fadra $\rightarrow$ ไต่สู่ `Trident Dragion` (Level 10) ระเบิด Sangen Summoning ได้สิทธิ์ตี 3 ครั้งที่ 6,000 ATK = 18,000 Damage OTK!

### 3.2 Ace Monsters & บทบาทหน้าที่
| Ace Monster | Card ID | บทบาทหน้าที่หลัก | กลยุทธ์ในการเล่น |
|---|---|---|---|
| **Trident Dragion** | 39823901 | **Main OTK Finisher** | ออกมาใน Battle Phase เมื่อเดินหลัง ระเบิด Sangen Summoning เพื่อเบิ้ลพลังเป็น 6,000 ATK ตี 3 ครั้ง |
| **Sangenpai Transcendent Dragion** | 2992036 | **Lockdown & Blanket Silence** | บังคับมอนสเตอร์ศัตรูตี และปิดปากศัตรูไม่ให้เปิดเอฟเฟกต์ใดๆ ใน Battle Phase |
| **Wattkyuki & Watthydra** | 78586116 / 9888196 | **Direct Attack Burst Aces** | ตีตรงข้ามหัวศัตรูต่อเนื่อง ดึงกันและกันลงมาทำดาเมจทะลุหลอด |
| **Hieratic Seal of the Heavenly Spheres** | 24361622 | **Going 1st Control Ace** | ปล่อยลงสนามในเทิร์น 1 บังคับเด้งการ์ดคู่แข่งขึ้นมือเมื่อศัตรูเริ่มเดินคอมโบ |

### 3.3 แผนการเล่น Going 1st vs. Going 2nd
- **Going 1st (เทิร์นแรกของเกม)**:
  - บอทจะมุ่งเน้นการจัดทรงบอร์ดเพื่อป้องกันตัว: วาง Paidra ค้นหา `Sangen Kaiho`
  - ทำ Link-2 `Hieratic Seal of the Heavenly Spheres` หรือตั้ง `Sangenpai Transcendent Dragion` ในสภาพตั้งรับ (3000 DEF)
  - หมอบ `Sangen Kaiho` ข้าม Main Phase 1 ของศัตรูตรงเข้าสู่ Battle Phase ทันที พร้อมเปิด Handtrap ขัดจังหวะ
- **Going 2nd (เทิร์นบุกทะลวง OTK)**:
  - เปิด `Interrupted Kaiju Slumber` ล้างสนามทั้งหมด
  - กาง `Sangen Summoning` รับ Blanket Protection ใน MP1
  - สั่งโจมตีด้วยสาย Watt เพื่อรีดความเสียหายตรง หรือไต่บันได Synchro Ladder สู่ Trident Dragion 6000 ATK ตี 3 ครั้ง ปิดฉากเกมในเทิร์นเดียว

---

## 4. เจาะลึกเด็ค Darklord 2 (Competitive Darklord Fusion/Control)

### 4.1 การประสานพลัง (Synergy)
เด็ค Darklord 2 ถูกปรับโครงสร้างให้เน้น **The First Darklord** ผสมผสานกับการขัดขวางผ่านสุสานและ Handtrap แฟรี่:
1. **Graveyard Quick Copy Mechanics**:
   - `Darklord Ixchel`, `Tezcatlipoca`, `Nasten`, และ `Eveningstar` มี Quick Effect จ่าย 1,000 LP เพื่อก๊อบปี้เวท/กับดัก Darklord ในสุสาน นำกลับเข้าเด็ค แล้วทำงานโดยไม่ต้องเสียการ์ดหรือสังเวยมอนสเตอร์
2. **The First Darklord Board Wipe & Protection**:
   - อัญเชิญฟิวชั่นโดยใช้ `Darklord Morningstar` เป็นวัตถุดิบ จะทำลายการ์ดบนสนามของศัตรูทั้งหมด (Board Wipe)
   - มอบความคุ้มครอง: มอนสเตอร์เผ่า Fairy ทั้งหมดบนสนามของเราจะไม่สามารถตกเป็นเป้าหมายเอฟเฟกต์การ์ดของศัตรูได้ (Targeting Immunity)
   - Quick Effect ชุบชีวิต Fairy ตัวใดก็ได้จากสุสานในสภาพตั้งรับในเทิร์นของใครก็ได้
3. **Herald Counter Engine**:
   - `Herald of Orange Light` ทิ้งตัวเองและ Darklord แฟรี่บนมือ สกัดและทำลายเอฟเฟกต์มอนสเตอร์ของศัตรู
   - การทิ้ง Darklord ลงสุสานไม่เสียเปล่า เพราะสามารถชุบขึ้นมาด้วย `Darklord Contact` หรือ `The First Darklord` ได้ทันที

### 4.2 Ace Monsters & บทบาทหน้าที่
| Ace Monster | Card ID | บทบาทหน้าที่หลัก | กลยุทธ์ในการเล่น |
|---|---|---|---|
| **The First Darklord** | 80004149 | **Supreme Boss & Board Wiper** | 4000/4000 ฟิวชั่นล้างสนามศัตรู ให้สถานะกันเล็งเป้าแก่แฟรี่ทั้งสนาม และชุบเพื่อนแบบ Quick Effect |
| **Darklord Morningstar** | 82134632 | **Swarm & Fusion Catalyst** | Advance Summon เรียกเพื่อนตามจำนวนมอนสเตอร์ศัตรู หรือใช้เป็นวัตถุดิบทำ The First Darklord |
| **Darklord Eveningstar** | 80000000 | **Mass Mill & GY Control** | มิลการ์ดลงสุสานตามจำนวนมอนสเตอร์ศัตรู และมี Quick Effect ก๊อบปี้ Trap ในสุสาน |
| **Darklord Ixchel & Tezcatlipoca** | 44203504 / 39185152 | **Draw Engine & Battle Shield** | Ixchel จั่ว 2 ใบ, Tezcatlipoca ทิ้งจากมือเพื่อกัน Darklord ถูกทำลายจากการต่อสู้/เอฟเฟกต์ |
| **Condemned Darklord** | 33883834 | **Searcher & Tribute Bridge** | Link-2 Fairy ที่ยอมให้สละมอนสเตอร์ในสุสานแทนบนสนาม และเสิร์ช Darklord เข้ามือ |

### 4.3 แผนการเล่น Going 1st vs. Going 2nd
- **Going 1st (เทิร์นแรกของเกม)**:
  - ใช้ `Darklord Ixchel` ทิ้งตัวเองคู่กับ Darklord อีกใบ จั่วการ์ด 2 ใบ
  - ใช้ `Banishment of the Darklords` เสิร์ช `Darklord Contact`, `Darklord Rebellion`, หรือ `The Sanctified Darklord`
  - ฟิวชั่น `The First Darklord` หรือชุบ Darklord ยืนบอร์ด หมอบหรือทิ้ง `Darklord Rebellion` (ทำลายการ์ด) และ `The Sanctified Darklord` (ปิดเอฟเฟกต์มอนสเตอร์ + ดูดเลือด) ไว้ในสุสาน
  - ในเทิร์นศัตรู: เมื่อศัตรูเริ่มเล่น ใช้ Quick Effect ก๊อบปี้ Rebellion และ Sanctified ขัดขวาง 2 จังหวะ
- **Going 2nd (เทิร์นแก้บอร์ดและตีสวน)**:
  - ขัดขวางเทิร์นแรกของศัตรูด้วย `Herald of Orange Light`
  - เข้าเทิร์นบอท: ฟิวชั่นเรียก `The First Darklord` ล้างการ์ดบนสนามศัตรูทั้งหมด แล้วโจมตีด้วยพลัง 4,000+ ปิดฉาก

---

## 5. การลงทะเบียนและการติดตั้ง (Registration & Deployment)

### 5.1 การลงทะเบียนใน `bots.json`
เด็คทั้งสองได้รับการลงทะเบียนอย่างถูกต้องสมบูรณ์:
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

### 5.2 ตำแหน่งไฟล์ไบนารีและเด็คที่ Deploy แล้ว
- `C:\Users\admin\Documents\EdoGame\WindBot\WindBot.dll`
- `C:\Users\admin\Documents\EdoGame\WindBot\ExecutorBase.dll`
- `C:\Users\admin\Documents\EdoGame\WindBot\core.dll`
- `C:\Users\admin\Documents\EdoGame\WindBot\bots.json`
- `C:\Users\admin\Documents\EdoGame\DashBot.exe`
- `C:\Users\admin\Documents\EdoGame\deck\Watenpai.ydk`
- `C:\Users\admin\Documents\EdoGame\deck\Darklord 2.ydk`
- `C:\Users\admin\Documents\EdoGame\WindBot\Decks\Watenpai.ydk`
- `C:\Users\admin\Documents\EdoGame\WindBot\Decks\Darklord 2.ydk`

ทุกไฟล์ได้รับการคอมไพล์ผ่าน `BUILD_AND_DEPLOY.ps1` ด้วยสถานะ **0 Errors / 0 Warnings ปิดกั้น** และพร้อมสำหรับการทดสอบของผู้ใช้งานทันที
