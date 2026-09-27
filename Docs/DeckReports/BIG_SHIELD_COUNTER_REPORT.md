# Big Shield Gardna Stun & Defense OTK Fortress Report

**Project**: WindBot AI Engine (YugiohTH)  
**Deck Name**: `BigShieldCounter` / `Big Shield Gardna`  
**Executor Class**: [`BigShieldCounterExecutor.cs`](file:///C:/Users/admin/Documents/EdoGame/src/YGO_SOURCE_CLEAN/windbot-fork/Game/AI/Decks/BigShieldCounterExecutor.cs)  
**Deck File**: [`BigShieldCounter.ydk`](file:///C:/Users/admin/Documents/EdoGame/src/YGO_SOURCE_CLEAN/windbot-fork/Decks/BigShieldCounter.ydk) & [`deck/BigShieldCounter.ydk`](file:///C:/Users/admin/Documents/EdoGame/deck/BigShieldCounter.ydk)  
**Target Framework**: .NET 10.0 / C# 13 (`ModernExecutor` Architecture)  
**Compiler Validation**: 0 Errors, 0 Warnings  
**Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`  

---

## 1. Concept & Strategic Intent (Stun & Fortress Overhaul)

จากคำขอเพิ่มเติมที่ต้องการให้เด็คมี **"การขัดขวางที่หนักหน่วง และได้ฟีลแบบ สตั๊น (Stun / Floodgate) เต็มรูปแบบ"**  
เด็คได้รับการอัปเกรดเป็น **Stun Fortress** ที่ขังและปิดผนึกการเล่นของศัตรูทุกมิติ:
1. **เค้าเตอร์ / มอนตายยาก**: ป้อมปราการ 2,600 DEF ที่ไม่ตายจากการต่อสู้และมีเคาน์เตอร์แทรปคอยสกัด
2. **ระบบสตั๊นปิดผนึกบอร์ด (Heavy Stun Lock)**:
   - **`Skill Drain`**: ล้างเอฟเฟกต์มอนสเตอร์บนสนามทั้งหมด ทำให้ศัตรูกลายเป็นมอนสเตอร์ธรรมดา (Vanilla) ไร้พิษสง และ **ล็อก Big Shield Gardna ให้อยู่ในท่าตั้งรับ 2,600 DEF ตลอดกาล**!
   - **`Fossil Dyna Pachycephalo`**: แบนการอัญเชิญพิเศษทั้งหมด (Special Summon Lock) และหากคว่ำอยู่ เมื่อหงายหน้าจะระเบิดล้างมอนสเตอร์ที่อัญเชิญพิเศษทั้งหมด
   - **`Summon Limit`**: จำกัดการอัญเชิญของศัตรูไม่เกิน 2 ครั้งต่อเทิร์น สกัดคอมโบกางบอร์ดทุกชนิด
   - **`Anti-Spell Fragrance`**: บังคับให้ต้องเซ็ตการ์ดเวทมนตร์ก่อน 1 เทิร์น ห้ามใช้เวททันที สกัดการ์ดล้างสนามและเวท Quick-Play
   - **`Gozen Match`**: อนุญาตให้มีได้แค่ 1 Attribute บนสนาม ซึ่งการ์ดของเราทุกใบคือ **EARTH (ดิน)** ศัตรูที่มีหลากธาตุจะถูกจำกัดเหลือตัวเดียว
3. **การบังคับให้อีกฝ่ายตีเข้ามา**: `Battle Mania` และ `Staunch Defender` บังคับมอนสเตอร์ศัตรูที่ถูกสตั๊นจนไร้เอฟเฟกต์ ให้ต้องเปลี่ยนเป็นสภาพโจมตีและพุ่งชนกำแพงตายเอง
4. **การป้องกันเอฟเฟค**: `Lord of the Heavenly Prison` ปกป้องการ์ดคว่ำและกับดักทั้งหมดจากการถูกทำลาย พร้อมด้วย `Solemn Judgment` และ `Solemn Strike`

---

## 2. Card Audit & Synergy Analysis (100% Audited Against `cards.cdb`)

| Card Name | ID | Type / Stats | Role in Strategy |
|---|---|---|---|
| **Big Shield Gardna** | `65240384` | Lv4 EARTH Warrior (100/2600) | **Ace Tank**: ป้อม 2,600 DEF; Negate เวทเล็งเป้าเมื่อคว่ำ; เมื่อมี Skill Drain จะไม่มีวันเปลี่ยนเป็นท่าโจมตี! |
| **Mid Shield Gardna** | `75487237` | Lv4 EARTH Warrior (100/1800) | ป้อมเสริม: Negate เวทเมื่อคว่ำ และมีเอฟเฟกต์ **คว่ำตัวเองกลับลงไปได้ทุกเทิร์น** เพื่อรีเซ็ตกับดัก |
| **Fossil Dyna Pachycephalo** | `42009836` | Lv4 EARTH Rock (1200/1300) | **Stun Floodgate**: ล็อกห้าม Special Summon ทั้งสนาม; เมื่อหงายหน้า ทำลายมอนสเตอร์ที่ Special Summon ทั้งหมด |
| **Stronghold Guardian** | `23535429` | Lv4 EARTH Warrior (0/1500) | Damage Step Surprise: ทิ้งจากบนมือเพิ่ม DEF 1,500 ทันที (**ทำงานทะลุ Skill Drain เพราะทำงานบนมือ**) |
| **Lord of the Heavenly Prison**| `9822220` | Lv10 DARK Rock (3000/3000) | **Immunity Engine**: โชว์จากมือใน MP1 การ์ดคว่ำทั้งหมดไม่พังจากเอฟเฟกต์ (**ทำงานทะลุ Skill Drain**) |
| **Ash Blossom & Joyous Spring**| `14558128` | Lv3 FIRE Zombie (0/1800) | Universal Handtrap: สกัดคอมโบศัตรู (**ทำงานทะลุ Skill Drain เพราะทำงานบนมือ**) |
| **Reinforcement of the Army** | `32807846` | Spell Normal | เสิร์ช `Big Shield Gardna` ขึ้นมือ 100% |
| **Pot of Extravagance** | `49238328` | Spell Normal | จั่วการ์ด 2 ใบเติมมือและกับดัก |
| **Harpie's Feather Duster** | `18144506` | Spell Normal | กวาดแผงหลังศัตรู |
| **Skill Drain** | `82732705` | Trap Continuous | **Heavy Stun**: ล้างเอฟเฟกต์มอนสเตอร์บนสนามทั้งหมด! ศัตรูเป็นง่อย และล็อกบิ๊กชีลด์ให้อยู่ในสภาพ 2,600 DEF ตลอดไป |
| **Summon Limit** | `23516703` | Trap Continuous | **Heavy Stun**: ผู้เล่นทั้งสองลงมอนสเตอร์ได้ไม่เกิน 2 ครั้งต่อเทิร์น บล็อกการเดินคอมโบทุกประเภท |
| **Anti-Spell Fragrance** | `58921041` | Trap Continuous | **Heavy Stun**: เวทมนตร์ต้องคว่ำไว้ 1 เทิร์นถึงจะเปิดใช้ได้ สกัด Raigeki, Duster, Lightning Storm |
| **Gozen Match** | `53334471` | Trap Continuous | **Heavy Stun**: ควบคุมได้แค่ 1 ธาตุบนสนาม การ์ดเราทุกตัวคือธาตุดิน (EARTH) ศัตรูจะลงตัวอื่นไม่ได้ |
| **Battle Mania** | `31245780` | Trap Normal | **บังคับตี**: ใน Standby บังคับมอนสเตอร์ศัตรูทุกตัวเป็นตั้งโจมตีและ **ต้องสั่งโจมตีทั้งหมด**! |
| **Staunch Defender** | `92854392` | Trap Normal | **บังคับตี**: เมื่อศัตรูสั่งโจมตี เล็ง `Big Shield Gardna` ศัตรูต้องสั่งมอนสเตอร์ทุกตัวโจมตีใส่ Big Shield Gardna |
| **D2 Shield** | `71249758` | Trap Normal | เล็งมอนสเตอร์ตั้งรับ พลัง DEF เพิ่มเป็น 2 เท่า (`Big Shield Gardna` DEF กลายเป็น **5,200 DEF**!) |
| **Rise to Full Height** | `19254117` | Trap Normal | เพิ่ม DEF เป็น 2 เท่า / ในสุสาน แบนการ์ดนี้เพื่อบังคับให้ศัตรูโจมตีได้แค่มอนสเตอร์เป้าหมายตัวเดียว |
| **Cross Counter** | `37083210` | Trap Normal | หาก DEF สูงกว่า ATK ผู้โจมตี **สะท้อนดาเมจเป็น 2 เท่า** และทำลายมอนสเตอร์ที่โจมตีเข้ามาทันที |
| **Solemn Judgment** | `41420027` | Trap Counter | ปฏิเสธเวท กับดัก หรือการอัญเชิญ เพื่อปกป้องบอร์ดจากการล้างสนาม |
| **Solemn Strike** | `40605147` | Trap Counter | ปฏิเสธมอนสเตอร์เอฟเฟกต์และการอัญเชิญพิเศษ |

---

## 3. ทำไม Skill Drain ถึงทำให้ Big Shield Gardna แข็งแกร่งที่สุด?

ตามกฎปกติของยูกิโอ:
- `Big Shield Gardna` มีข้อเสียคือ: *"หากการ์ดนี้ถูกโจมตี เมื่อสิ้นสุด Damage Step จะต้องเปลี่ยนสภาพเป็นโจมตี (100 ATK)"*
- เมื่อมี **`Skill Drain`** อยู่บนสนาม:
  - เอฟเฟกต์บังคับเปลี่ยนร่างของ Big Shield Gardna เป็นเอฟเฟกต์ทริกเกอร์บนสนาม จึง **ถูกลบล้างทิ้งโดยสิ้นเชิง (Negated)!**
  - ส่งผลให้ **Big Shield Gardna จะอยู่ในสภาพตั้งรับ 2,600 DEF (หรือ 5,200 DEF เมื่อบัฟ D2 Shield) อย่างถาวร ไม่ว่าจะถูกโจมตีกี่ครั้งก็ตาม!**
  - และการ์ดเสริมพลังของเราอย่าง `Stronghold Guardian`, `Ash Blossom`, `Lord of the Heavenly Prison` ทั้งหมดเป็น **Hand Effect (ทำงานบนมือ)** จึง **ไม่ได้รับผลกระทบจาก Skill Drain เลยแม้แต่น้อย!**

---

## 4. The 9,400+ Damage Defense Reflect OTK Math

เมื่อศัตรูถูกสตั๊นจนเดินเกมไม่ได้ แล้วถูก `Battle Mania` บังคับให้ต้องวิ่งเข้ามาชน:
1. `Big Shield Gardna` มีพลังป้องกันเดิม **2,600 DEF**
2. เปิดใช้ `D2 Shield`: พลัง DEF กลายเป็น 2 เท่า = **5,200 DEF**
3. ทิ้ง `Stronghold Guardian` จากมือใน Damage Step: เพิ่มอีก 1,500 DEF = **6,700 DEF**
4. ทริกเกอร์ `Cross Counter`: ดาเมจสะท้อนที่ศัตรูจะได้รับคูณ 2:
   - หากศัตรูถูกบังคับตีเข้ามาด้วยมอนสเตอร์ 2,000 ATK:
     $$\text{Battle Damage} = (6,700 - 2,000) \times 2 = 4,700 \times 2 = \mathbf{9,400 \text{ ดาเมจ (OTK One-Hit Kill!)}}$$
   - ศัตรูตายทันทีในเทิร์นของตัวเอง!

---

## 5. Deployment Verification & Status

- ไบนารี Deploy เรียบร้อยที่: `C:\Users\admin\Documents\EdoGame\`
  - `WindBot\WindBot.dll`
  - `WindBot\bots.json`
  - `WindBot\Decks\BigShieldCounter.ydk`
  - `deck\BigShieldCounter.ydk`
- Build Status: **สำเร็จ 100% (0 Errors, 0 Warnings)**
