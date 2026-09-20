# 📋 คู่มือรายชื่อเด็คที่ได้รับการปรับปรุงสำหรับทดสอบ (Test Decks Checklist)

> **💡 หมายเหตุเรื่องคำนำหน้า (`Expert_`)**: 
> **ไม่ต้องใช้คำนำหน้า `Expert_` อีกต่อไปครับ!** สามารถใช้ชื่อคลีน **`2026_<ชื่อเด็ค>`** (เช่น `2026_Maliss`, `2026_Tearla`, `2026_Exosister`, `2026_Fireking`) ใน DashBot หรือสั่งรัน WindBot ได้โดยตรงทันที เนื่องจากระบบตัดส่วน AI Training/Dataset Logger ออกเป็น Rule-Based ล้วนแล้ว และระบบจับคู่ชื่อเด็คอัตโนมัติ (Robust Match)

---

## 🎯 หมวดที่ 1: เด็คหลักที่ได้รับการปรับปรุงโค้ดและแก้ไขบั๊กในรอบนี้ (Priority 1)

เด็คกลุ่มนี้คือเด็คที่มีการแก้ไขตรรกะใน C# Executor โดยตรง มีจุดสังเกตเฉพาะทางที่แนะนำให้สังเกตระหว่างดวล:

| ลำดับ | ชื่อเด็คใน DashBot / WindBot | ไฟล์ Executor | สิ่งที่ปรับปรุงและจุดที่ต้องสังเกตในการดวล |
| :---: | :--- | :--- | :--- |
| **1** | `2026_Maliss` | `_2026_MalissExecutor.cs` | **1-Card Link Climbing**: แก้ไขบั๊ก `LinkDiscipleSummon` (เดิมติดเงื่อนไขวัตถุดิบ 2 ใบทำให้ Link-1 ไม่ยอมออก) บอทจะสามารถทำคอมโบตั้งบอร์ดจาก Maliss มอนสเตอร์ตัวเดียวขึ้น `Link Disciple` / `Linguriboh` / `Crypter` ได้อย่างลื่นไหล |
| **2** | `2026_Fireking` | `_2026_FirekingExecutor.cs` | **Field Wipe & Kirin Guard**: แก้ไขไม่ให้บอทเปลี่ยนฟิลด์ `Fire King Island` จนล้างสนามตัวเองหากไม่มี Sanctuary, ป้องกัน `Kirin` ระเบิดตัวเองฟรีเมื่อศัตรูไม่มีการ์ด, ปรับเป้าหมายการทำลายให้เล็งการ์ดศัตรูก่อน |
| **3** | `2026_Exosister` | `_2026_ExosisterExecutor.cs` | **True OCG Martha & GY Disruption**: ปรับ Martha ให้ทำงานตามเงื่อนไขกฎจริง (ต้องมี Elis ในเด็ค และอัญเชิญเฉพาะ Xyz), ปรับ `Magnifica` ให้รีมูฟเฉพาะการ์ดบนสนามศัตรู, เพิ่มลูกเล่นการขัดจังหวะสุสานด้วย `Mikailis` |
| **4** | `2026_KaijuCrusadia`| `_2026_KaijuCrusadiaExecutor.cs`| **OTK Combo & Boss Protection**: ติดตั้ง `ComboRouter` สำหรับขึ้น `Equimax` / `Avramax`, เพิ่ม `IsAceCard` ป้องกันไม่ให้บอทเอา Equimax ไปเป็นวัตถุดิบอื่น และมีระบบเช็คดาเมจปิดเกม OTK |
| **5** | `2026_Tearla` | `_2026_TearlaExecutor.cs` | **ComboRouter Tier-1**: ติดตั้งสายคอมโบหลัก Reinoheart ➔ Kitkallos และ Fiendsmith Line พร้อมระบบล่อ Handtrap (`BaitPlanner`) และการเรียง Chain Link (`ChainAdvisor`) |
| **6** | `2026_Yummy` | `_2026_YummyExecutor.cs` | **ComboRouter Tier-1**: ติดตั้งสายคอมโบ Marshmao ➔ Cupsy และ Cooky ➔ Lollipo ➔ Borreload Savage Dragon พร้อมระบบ Bait & Chain ป้องกันการโดนขัดจังหวะ |
| **7** | `2026_Purrely` | `_2026_PurrelyExecutor.cs` | **Resource & Chain Control**: ติดตั้ง `BaitPlanner`, `ChainAdvisor` และ `ResourcePlan` บริหาร Quick-Play Spell และการซ้อน Xyz Material ขึ้น `Expurrely Noir` อย่างเป็นระบบ |
| **8** | `2026_Stun` | `_2026_StunExecutor.cs` | **Self-Harm & Resource Guard**: ติดตั้ง `ResourcePlan` คุมลำดับการเปิด Floodgate / Counter Trap ไม่ให้จ่ายไลฟ์ซ้ำซ้อน และเพิ่มระบบป้องกันไม่ให้ The Fallen & The Virtuous ทำลายสนามตนเอง |
| **9** | `2026_Angelechy` | `_2026_AngelechyExecutor.cs` | **Database Resolution**: ผสานการ์ด Archetype Angelechy ทั้งหมดลง `cards.cdb` สามารถเรียกเล่นและรันคอมโบอัญเชิญได้สมบูรณ์ ไม่แครช |
| **10** | `2026_DogmaStun` | `_2026_DogmaStunExecutor.cs` | **Dogmatika Anti-Meta Stun (NEW)**: ติดตั้งระบบลำดับการตั้งรับ Boarder/Ecclesia, ระบบดัมพ์ Extra Deck (N'tss, Garura, Dogma Dragon, Titaniklad, Albion) ตามสถานการณ์, การรัน Super Polymerization ไร้การตอบโต้, การบริหาร Card of Demise / Pot of Prosperity, และคุม Floodgate ป้องกันการทำลายสนามตัวเอง |

---

### 🛡️ เด็คที่ปรับปรุง Target Selection (เลิกสุ่ม `cards[0]` บอด)
กลุ่มเด็คที่มีการปรับฟังก์ชันเลือกการ์ด (`OnSelectCard`) ให้ฉลาดขึ้น คัดเลือกทิ้งการ์ดที่มี ATK ต่ำสุด ไม่ทิ้งการ์ดบอส และเลือกเป้าหมายศัตรูที่มี Threat สูงสุดก่อน:
- **`2026_Archfiend`**: ปรับระบบค้นหาและทำลายเป้าหมายตาม Threat
- **`2026_Dreadnought`**: ปรับระบบคัดเลือกวัตถุดิบและสเปเชียลมอนสเตอร์
- **`2026_Regenesis`**: คัดเลือกเป้าหมายคืนชีพและการสังเวยอย่างคุ้มค่า
- **`2026_Darklord`**: คัดเลือกการ์ดทิ้งสุสานอย่างปลอดภัย (ปกป้องการ์ดบอส)
- **`2026_Doomz`**: คัดเลือกเป้าหมายการรีมูฟและการขัดขวางศัตรู

---

## ⚡ หมวดที่ 2: เด็ค ModernExecutor มาตรฐานสูงที่พร้อมให้ทดสอบ (Score 90–100)

เด็คกลุ่มนี้รองรับสถาปัตยกรรม `ModernExecutor`, `CardIntelligence`, `ComboRouter`, และ `BaitPlanner` สามารถเลือกมาดวลทดสอบได้ทันที:

| ชื่อเด็คใน DashBot | รูปแบบเด็ค (Archetype Strategy) | จุดเด่นของ AI |
| :--- | :--- | :--- |
| `2026_Dracotail` | Fusion / Control | ลำดับการฟิวชั่นบอสและการใช้ Pan/Urgula ไม่ทำลายสนามตนเอง |
| `2026_AFS` | Fiendsmith / Snake-Eye Combo | คอมโบต่อเนื่องหลายเอนจิ้น ตั้งบอร์ดบอส Caesar + Despia |
| `2026_Branded` | Fusion Midrange | การออก Mirrorjade, Albion และกระจายทรัพยากรขัดจังหวะ |
| `2026_Runick` | Deck Destruction / Control | การใช้ Quick-Play Spell รีมูฟเด็คฝ่ายตรงข้ามอย่างต่อเนื่อง |
| `2026_Spright` | Rank 2 / Link Combo | การต่อยอด Spright Blue, Jet, Starter ขึ้น Gigantic + Red/Carrot |
| `2026_Labrynth` | Normal Trap Control | การเซ็ตและเด้งกับดัก Welcome Labrynth, Lady, Lovely |
| `2026_BlueEyes` | Beatdown / Synchro | การเสิร์ช Melody, การออก Blue-Eyes Spirit, Chaos MAX |
| `2026_GemKnight` | Fusion FTK / Beatdown | การใช้ Brilliant Fusion, Master Diamond, Lady Lapis |
| `2026_Goldlord` | Eldlich Trap Control | การวนเวียน Eldlich จากสุสานและการคอนโทรลสนาม |
| `2026_Invoke` | Fusion Control | การตั้ง Aleister ➔ Mechaba ป้องกันการขัดจังหวะ |
| `2026_K9` | Synchro / Midrange | การบริหารจังหวะจูนนิ่งและการกวนบอร์ดฝ่ายตรงข้าม |
| `2026_Luna` | Rank 4 / Midrange | การเด้งการ์ดด้วย Luna และการกดดันไลฟ์พอยต์ |
| `2026_Mimighoul` | Flip / Hand Disruption | การส่ง Mimighoul ไปป่วนสนามศัตรูและการควบคุมเอฟเฟกต์ |
| `2026_Puppet` | Gimmick Puppet Lock | คอมโบ Xyz Rank 8 และการล็อกบอร์ดศัตรู |
| `2026_RedDragon` | Dark Synchro Aggro | การเรียก Red Supernova Dragon และการกวาดสนาม |
| `2026_SkyStriker`| Spell Link Control | การสะสมการ์ดเวท 3 ใบในสุสานเพื่อเพิ่มพลังเอฟเฟกต์เวท |
| `2026_Solfachord`| Pendulum Midrange | การตั้ง Scale สเกลและการอัญเชิญเพนดูลัมบอส |
| `2026_Speedroid` | Wind Synchro Combo | การไต่ระดับ Synchro ขึ้น Crystal Wing และ Clear Wing |
| `2026_Tellarknight`| Rank 4 Xyz Swarm | การตั้ง Triverr กวาดการ์ดขึ้นมือและการตัดเกม |
| `2026_TrainCryston`| Machine Rank 10 / Synchro | รถไฟยิง 2000 เบิร์นดาเมจและบอสดาว 10 |
| `2026_DarkTime` | Dark Synchro / Fiend | คอมโบความมืดและการกวาดล้างทรัพยากร |
| `2026_RyuGe` | Wyrm / Dragon Midrange | บอสมังกรใหญ่และการคุมเกมกระดาน |
| `2026_DarkWorld` | Hand Discard Aggro | การทิ้งการ์ดเพื่อสเปเชียลและจั่วการ์ดไม่จำกัด |
| `2026_Hecahand` | Link / Control | การลิงก์และการควบคุมการ์ดบนมือศัตรู |
| `2026_Clown` | Rank 4 Tooling | ลูกเล่นเด้งการ์ดและการกดดันจังหวะแบทเทิล |

---

## ⚙️ กฎความปลอดภัยระดับ Central Core ที่ทำงานกับทุกเด็คโดยอัตโนมัติ

ระหว่างการทดสอบ ทุกเด็คจะได้รับการคุ้มครองด้วยกฎกลาง 3 ประการ:
1. **Ace Protection**: บอทจะไม่ยอมนำบอสหลักที่มีพลังโจมตีสูงหรือมีคุณสมบัติ Ace (เช่น Apollousa, Accesscode, S:P Little Knight, Kitkallos, Magnifica, Equimax) ไปเปลี่ยนเป็น Link เล็กหรือ Xyz อย่างเด็ดขาด
2. **Empty Enemy Field Protection**: เมื่อบอทใช้งานการ์ดทำลายแบบระบุเป้าหมาย หากบนสนามฝ่ายตรงข้ามไม่มีการ์ด บอทจะตอบ "No" เสมอเพื่อไม่ให้ทำลายการ์ดบนสนามของตัวเอง
3. **No Redundant Handtrap Chain**: บอทจะไม่ยิง Infinite Impermanence หรือ Effect Veiler ซ้ำซ้อนลงบนมอนสเตอร์ของศัตรูที่ถูกทำให้ไร้ผล (Negated) ไปแล้ว

---

## 🕹️ วิธีการเปิดทดสอบ

1. เปิดโปรแกรม **`DashBot.exe`** ที่หน้าต่างหลัก `C:\Users\admin\Documents\EdoGame\DashBot.exe`
2. เลือกชื่อเด็คใน ComboBox (เช่น `2026_Maliss`, `2026_Tearla`, `2026_Fireking`)
3. กดปุ่ม **Start Bot**
4. เปิด **EDOPro** แล้วเข้าโหมด **LAN / Custom Duel** เชื่อมต่อเข้า IP `127.0.0.1` พอร์ต `7911` (หรือตามพอร์ตที่ห้องตั้งไว้) เพื่อเริ่มดวลได้ทันที
