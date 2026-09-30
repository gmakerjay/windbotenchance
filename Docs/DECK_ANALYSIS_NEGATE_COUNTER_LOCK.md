# การวิเคราะห์และออกแบบเด็คบอทสาย Negate Counter & Complete Lockout
**โครงการ:** YugiohTH / WindBot ModernExecutor Architecture  
**วันที่บันทึก:** 30 กันยายน 2026  
**สถานะ:** บันทึกเอกสารเพื่อเตรียมความพร้อม (รอคำสั่งเพื่อเริ่มดำเนินการ)

---

## 1. วัตถุประสงค์และโจทย์ความต้องการ (Core Directives)

1. **สาย Negate Counter (Spell Speed 3 & Hard Negate)**: เน้นการ์ดขัดขวางที่มีความเด็ดขาดสูง ไม่เปิดช่องให้ฝ่ายตรงข้ามใช้เอฟเฟกต์หลบหรือสร้างเชนแทรก
2. **ห้ามให้ฝ่ายตรงข้ามได้เล่นแบบสมบูรณ์หากคอมโบติด (Complete Lockout)**: เมื่อบอร์ดตั้งติด ฝ่ายตรงข้ามต้องไม่สามารถขยับเกมได้เลย (Cannot Activate / No Special Summon) ไม่ใช่แค่การเนเกตเป็นครั้งคราว
3. **สวนกลับได้รุนแรง (Offensive Push / Turn 2 Comeback / OTK)**: แก้จุดอ่อนของเด็ค Stun ทั่วไปที่มีแต่ตัวพลังโจมตีต่ำ เมื่อถึงเทิร์นเราต้องมีพลังเจาะและปิดเกมได้ใน 1-2 เทิร์น
4. **แผงหลังแข็งแกร่งพอจะรับการ์ดพังบอร์ด/ลูปทำลาย (Anti-Board Breakers)**: มีภูมิคุ้มกันต่อการ์ดแก้ทางยอดฮิต เช่น Harpie's Feather Duster, Lightning Storm, Evenly Matched, Dark Ruler No More, Super Polymerization, และ Kaiju

---

## 2. ทำไมสไตล์นี้จึงเหมาะกับ WindBot Rule-Based AI ระดับสูงสุด (S-Tier)?

| มิติการเล่น | เด็คคอมโบหลายขั้นตอน (Combo Heavy) | เด็ค Negate Counter & Lockout |
|---|---|---|
| **พฤติกรรม AI** | มักจะสับสนกับกิ่งคอมโบ 20+ สเต็ป มีโอกาสเลือก Link Material ผิด | **Deterministic Trigger**: มีเงื่อนไขชัดเจน 100% ไม่มีความลังเล |
| **การเชนสวน** | มอนสเตอร์ Quick Effect ฝ่ายตรงข้ามสามารถเชนหลบได้ | **Spell Speed 3 (Counter Trap)**: กฎเกมห้ามไม่ให้ Monster Effect หรือ Quick Spell เชนสวนได้ |
| **ความผิดพลาด (Misplay)** | เสี่ยงต่อการโดนขัดขวางแล้วเสียทรัพยากรหมดตัว | ควบคุมสถานการณ์ด้วย Continuous Floodgate และ Counter Protection |

---

## 3. เมทริกซ์การรับมือการ์ดพังบอร์ด (Anti-Board Breaker Matrix)

| การ์ดพังบอร์ดของฝ่ายตรงข้าม | ภัยคุกคาม | วิธีการป้องกันและแก้ทางของเด็ค |
|---|---|---|
| **Harpie's Feather Duster / Lightning Storm** | กวาดการ์ดเวท/กับดักทั้งสนาม | • **`Lord of the Heavenly Prison`**: ขณะอยู่บนมือ การ์ดที่หมอบอยู่ทั้งหมด **ไม่สามารถถูกทำลายด้วยเอฟเฟกต์การ์ดได้**<br>• **`Solemn Judgment`**: สวนล้างทิ้งทันทีด้วย Spell Speed 3<br>• **`The Sanctum of Parshath`** (ถ้าเล่นสาย Fairy): ป้องกันการเล็งเป้าและทำลายการ์ดหมอบ 100% |
| **Evenly Matched** | บังคับเนรเทศการ์ดคว่ำหน้าเกือบเกลี้ยงสนาม | แม้ไม่ใช่การทำลาย แต่เป็นการ์ด Normal Trap จึงถูก **`Solemn Judgment`** หรือ Counter Trap ของธีมสวนเนเกตได้ทันที |
| **Dark Ruler No More (DRNM) / Forbidden Droplet** | ปิดเอฟเฟกต์มอนสเตอร์ทั้งสนาม ห้ามมอนสเตอร์เชนสวน | **จุดตัดสำคัญที่สุด**: เด็คที่พึ่งแต่มอนสเตอร์เนเกตจะไร้ค่าทันที แต่ **Counter Trap เป็นการ์ดกับดัก** จึงสามารถเชนสวน DRNM และเนเกตทิ้งได้สบาย! |
| **Super Polymerization** | กลืนมอนสเตอร์บอสเป็นวัตถุดิบ ห้ามเชนสวน | สนามหลักพึ่งพา Counter Trap แผงหลัง หรือใช้บอสมอนสเตอร์เดี่ยว ทำให้คู่แข่งไม่มีเป้าหมายวัตถุดิบในการฟิวชั่น |
| **Kaiju / Lava Golem / Sphere Mode** | สังเวยบอสมอนสเตอร์ข้ามหัว | พลังขัดขวาง 80% ของเราอยู่ที่แผงหลัง (Counter Traps) แม้โดนสังเวยบอสไป คู่แข่งก็ยังไม่สามารถเดินเครื่องคอมโบได้ |
| **Anti-Spell Fragrance** (Tech Option) | การ์ดเสริมความเด็ดขาด | บังคับให้ฝ่ายตรงข้ามต้องหมอบการ์ดเวทก่อน 1 เทิร์น ทำให้การ์ดเวทพังบอร์ดทั้งหมดกลายเป็นหมันในเทิร์นนั้น |

---

## 4. วิเคราะห์ 3 สุดยอด Archetypes ที่ตรงโจทย์

### 🥇 ตัวเลือกที่ 1 (Supreme Recommendation): Dinomorphia Counter-Lock (ไดโนมอร์เฟีย)

> **คำนิยาม**: *"ยิ่ง LP เหลือน้อย มอนสเตอร์ฝ่ายตรงข้ามยิ่งเป็นใบ้สนิททั้งสนาม พร้อมแผงหลัง Spell Speed 3 และพลังสวนกลับ 4000 ATK"*

1. **การล็อกแบบสมบูรณ์ (The Ultimate Lockout)**:
   - บอส **`Dinomorphia Rexterm`** (Fusion Lv8): มีผลสนามต่อเนื่อง (Continuous Floodgate)
     > *"มอนสเตอร์ของฝ่ายตรงข้ามที่มี ATK มากกว่าหรือเท่ากับ LP ของเรา **ไม่สามารถเปิดใช้งานเอฟเฟกต์ได้เลย**"*
   - เด็คนี้จ่าย LP ครึ่งหนึ่งรัวๆ จนเหลือ **500 หรือ 125 LP** หมายความว่า มอนสเตอร์ของฝ่ายตรงข้าม 99.9% ในเกมจะไม่สามารถเปิดใช้เอฟเฟกต์ได้เลยแม้แต่ตัวเดียว (ไม่ว่าจะเป็นบนมือ ฟิลด์ หรือในสุสาน)
   - Quick Effect ของ Rexterm: ปรับ ATK มอนสเตอร์ฝ่ายตรงข้ามทั้งหมดให้เหลือเท่า LP ของเรา (ลดเหลือ 125-500) ตีเราไม่เข้า และมอนสเตอร์เราตบตายหมด
2. **แผงหลังและ Anti-Board Breakers**:
   - `Lord of the Heavenly Prison`: ป้องกันแผงหลังถูกทำลาย 100% + ชุบตัวเอง 3000 ATK + เสิร์ชกับดักมาหมอบฟรี
   - `Solemn Judgment` (x3): จ่าย LP ครึ่งหนึ่ง เนเกตทุกบอร์ดเบรกเกอร์
   - `Dinomorphia Intact` & `Dinomorphia Sonic`: Counter Trap ในธีม
   - **GY Protection**: การ์ดกับดัก Dinomorphia ในสุสาน Banish ตัวเองเพื่อกันทำลายจากการต่อสู้หรือเอฟเฟกต์
3. **การสวนกลับ (Counter-attack / Turn 2)**:
   - **`Dinomorphia Kentregina`**: ATK พื้นฐาน 4000 (เพิ่มขึ้นเมื่อ LP เราลดลง บีดดาวน์หนักหน่วง)
   - **`Ferret Flames`**: ไพ่ตายสวนกลับที่ทรงพลังที่สุดในยูกิโอ บังคับให้ผู้เล่นฝ่ายตรงข้ามสับมอนสเตอร์ทั้งหมดกลับเด็คจน ATK รวมไม่เกิน LP ของเรา (ส่งผลต่อผู้เล่น ไม่สน Immunity ใดๆ ทั้งสิ้น)
   - **`Super Starslayer TY-PHON Sky Crisis`**: ลงมาปิดปากมอนสเตอร์ที่มี ATK 2900+ และเด้งมอนสเตอร์คู่แข่ง
4. **ความพร้อมในโปรเจกต์**:
   - มีไฟล์ `DinomorphiaExecutor.cs` อยู่ในระบบแล้ว สามารถปรับจูน Logic และ Decklist ให้เป็น ModernExecutor Plugin ที่สมบูรณ์แบบได้ทันที

---

### 🥈 ตัวเลือกที่ 2: Pure Counter Fairy / Sanctuary Lock (เคาน์เตอร์ แฟรี่)

> **คำนิยาม**: *"ร่าย Solemn ฟรี 0 LP ด้วย Guiding Ariadne และปิดกั้นการ Special Summon 100% ด้วย Archlord Kristya"*

1. **การล็อกแบบสมบูรณ์**:
   - **`Archlord Kristya`** (ATK 2800): **"ผู้เล่นทั้งสองฝ่ายไม่สามารถ Special Summon มอนสเตอร์ได้เลยโดยเด็ดขาด"** (ตัดขาเด็คเมต้า 99% ในเกมทันที)
2. **แผงหลังและ Anti-Board Breakers**:
   - **`The Sanctum of Parshath`**: การ์ดเวทสนามที่ทำให้ **"การ์ดเวท/กับดักที่หมอบอยู่ทั้งหมด และการ์ดในโซนฟิลด์ ไม่สามารถถูกเล็งเป้าหมาย หรือถูกทำลายด้วยเอฟเฟกต์การ์ดได้!"** (Feather Duster / Lightning Storm ทำอะไรไม่ได้เลย)
   - **`Guiding Ariadne`**: อยู่ใน Pendulum Zone ทำให้ **ไม่ต้องจ่าย LP หรือทิ้งการ์ดเป็น Cost ในการเปิดใช้ Counter Trap อีกต่อไป!** (ร่าย Solemn Judgment, Solemn Strike, Solemn Warning, Ultimate Providence ฟรี 0 LP!)
3. **การสวนกลับ**:
   - Kristya 2800 ATK, `Celestial Knightlord Parshath` (Link-3) เสิร์ชการ์ดต่อเนื่อง, และ `Sacred Arch-Airknight Parshath` (2800 ATK) ตีทะลุ Def พร้อมเสิร์ช Counter Trap ทุกครั้งที่ดาเมจเข้า
4. **ข้อสังเกต**:
   - การเล่น Going Second พึ่งพามือเปิดมากกว่า Dinomorphia เล็กน้อย แต่ความสะใจในการกดเนเกต Spell Speed 3 รัวๆ อยู่ในระดับสูงสุด

---

### 🥉 ตัวเลือกที่ 3: Dogmatika Stun / Anti-Meta Boarder (ด็อกมาติกา สตั๊น)

> **คำนิยาม**: *"ล็อกการเล่นด้วย Inspector Boarder + Necrovalley + Nadir Engine และสวนกลับด้วย Super Polymerization"*

1. **การล็อกแบบสมบูรณ์**:
   - **`Inspector Boarder`**: ห้ามเปิดใช้เอฟเฟกต์มอนสเตอร์ตามจำนวนประเภทมอนสเตอร์พิเศษ
   - **`Necrovalley`**: ปิดตายสุสาน 100%
   - **`Skill Drain`**: ปิดเอฟเฟกต์มอนสเตอร์บนฟิลด์ทั้งหมด
2. **แผงหลังและ Anti-Board Breakers**:
   - `Decisive Battle of Golgonda`: ส่งมอนสเตอร์ Extra Deck ลงสุสานแทนการถูกทำลาย
   - `Solemn Judgment` + `Solemn Strike`
3. **การสวนกลับ**:
   - **`Super Polymerization`**: สวนกลับบอร์ดมอนสเตอร์ของศัตรูแบบไม่สามารถเชนได้
   - **`Dogmatika Punishment`**: ทิ้ง N'tss ทำลาย 2 ใบ
   - **`Enneacraft Asta.PIXEA`** (3000 ATK Beatstick)

---

## 5. ตารางเปรียบเทียบเชิงวิเคราะห์ (Scorecard)

| เกณฑ์การประเมิน | Dinomorphia Counter-Lock | Counter Fairy Lock | Dogmatika Stun |
|---|:---:|:---:|:---:|
| **ระดับการล็อกฝ่ายตรงข้าม (Lockout)** | **5/5 (Rexterm ห้ามใช้เอฟเฟกต์)** | 5/5 (Kristya ห้าม Special) | 4/5 (Boarder + Floodgate) |
| **ความหนาแน่น Counter Trap (SS3)** | **5/5 (Solemn + Intact + Sonic)** | 5/5 (Ariadne + Solemn รัวๆ) | 3/5 (Solemn ชุดพื้นฐาน) |
| **การรับมือ Board Breakers** | **5/5 (Lord + Solemn + GY)** | 5/5 (Sanctum + Solemn) | 4/5 (Golgonda + Solemn) |
| **พลังสวนกลับ (Turn 2 OTK / Beatdown)** | **5/5 (Kentregina 4000 + Ferret)**| 3.5/5 (Beatdown 2800) | 4/5 (Super Poly + Punishment)|
| **ความเข้ากันได้กับ WindBot AI** | **5/5 (มี Executor แล้ว + รูทตรง)** | 4.5/5 (เข้าใจง่าย ทริกเกอร์ชัด) | 4/5 (บริหาร Extra Deck) |

---

## 6. สรุปพิมพ์เขียว (Implementation Blueprint)

เมื่อคุณพร้อมที่จะเริ่มดำเนินการ สามารถเลือกแนวทางได้ทันที:

- **แนวทาง A (แนะนำสูงสุด)**: นำ **`Dinomorphia`** มาอัปเกรด Decklist ใหม่ ใส่ `Lord of the Heavenly Prison` x2, `Solemn Judgment` x3, `Ferret Flames` x2, และเชื่อมต่อเข้ากับสถาปัตยกรรม `DinomorphiaPlugin` (ModernExecutor + Domain Plugin) ตามมาตรฐานใหม่
- **แนวทาง B**: สร้างเด็คใหม่แกะกล่อง **`Counter Fairy (Sanctuary Lock)`** พร้อมไฟล์ `.ydk`, `CounterFairyExecutor.cs`, และลงทะเบียนใน `bots.json`

---
*เอกสารนี้ถูกบันทึกไว้ใน `C:\Users\admin\Documents\EdoGame\Docs\DECK_ANALYSIS_NEGATE_COUNTER_LOCK.md` เรียบร้อยแล้ว ระบบจะหยุดพักและรอคำสั่งของคุณก่อนเริ่มดำเนินการขั้นตอนต่อไป*
