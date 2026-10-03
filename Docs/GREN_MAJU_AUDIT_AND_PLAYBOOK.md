# Gren Maju (Danger! Kaiju Level 8 Turbo & Rank 8 Numeron Plan B) - Playbook & Architecture Audit

## 1. Overview & Architecture Philosophy

เด็ค **Gren Maju** (`GrenMaju`) ถูกออกแบบและพัฒนาใหม่อย่างก้าวกระโดดภายใต้หมวด **Special** (Badge: Deep Violet `#5B21B6` ใน DashBot Launcher) 

### ปัญหาของสถาปัตยกรรมเดิม (Golden Castle of Stromberg) และสาเหตุที่ปรับปรุง:
1. **การโม่การ์ดแบบสุ่มดวงไร้การควบคุม (Blind Random Banishing)**:
   - ปราสาท Stromberg บังคับรีมูฟการ์ด 10 ใบบนสุดแบบคว่ำหน้าทุก Standby Phase ทำให้การ์ดปิดเกมหลักอย่าง `Gren Maju Da Eiza` (3 ใบ) มีโอกาสสูงมากที่จะถูกรีมูฟทิ้งทั้งหมดตั้งแต่ 1-2 เทิร์นแรก และทำให้เด็คหมด (Deck Out) ภายในเวลาสั้น
2. **ติดพันธสัญญาห้าม Normal Summon (Oath Conflict)**:
   - Stromberg มีข้อจำกัดเข้มงวด: *"ไม่สามารถ Normal Summon/Set ได้ในเทิร์นที่สั่งเอฟเฟกต์"* ซึ่งขัดกับธรรมชาติของ Gren Maju Da Eiza ที่เป็นมอนสเตอร์เลเวล 3 ที่ต้อง Normal Summon ลงมาตี ทำให้ไม่สามารถปิดเกมด้วย Gren Maju ได้ในเทิร์นเดียวกัน
3. **ขาดแผนสำรองอย่างสิ้นเชิง (No Plan B / Alternative Win Condition)**:
   - หาก Gren Maju ถูกรีมูฟคว่ำหน้าไปหมด เด็คเดิมไม่มีตัวทำเกมระดับ Game-Ender ตัวอื่นเลย และไม่มีทางกู้การ์ดคว่ำหน้ากลับมาได้

---

## 2. โครงสร้างสถาปัตยกรรมใหม่ (Level 8 Turbo & Numeron Plan B)

เพื่อแก้ไขปัญหาการพึ่งพาดวงและแก้ปัญหาเมื่อใบหลักถูกรีมูฟทั้งหมด เด็คได้รับการปรับโครงสร้างใหม่เป็น **Competitive Danger! Kaiju Level 8 Blind-Second Turbo**:

### 🎯 1. สุดยอดแผนสำรอง Plan B: Rank 8 XYZ Numeron Engine (ไม่ต้องพึ่ง Gren Maju 100%)
- **`Number 97: Draglubion`** (Rank 8) ➔ เรียกพิเศษ **`Number 100: Numeron Dragon`** (Rank 1) พร้อมแนบ `Number 38: Hope Harbinger Dragon` เป็นวัตถุดิบ
- **เอฟเฟกต์ Numeron Dragon**: ถอดวัตถุดิบ 1 ชิ้น ได้รับพลังโจมตีเพิ่มขึ้นเท่ากับ Rank รวมของมอนสเตอร์ Xyz ทั้งสนาม $\times 1000$:
  $$\text{Rank 8 (Draglubion)} + \text{Rank 1 (Numeron Dragon)} = \text{Rank 9} \implies \mathbf{9,000 \text{ ถึง } 13,000+ \text{ ATK!}}$$
  - **สามารถตบปิดเกม (OTK 9,000+ DMG) ได้ทันทีในฮิตเดียว** แม้ว่า Gren Maju จะถูกรีมูฟออกนอกเกมไปหมดทั้ง 3 ใบหรือไม่ถูกจั่วขึ้นมือเลยก็ตาม!
- **ระบบป้องกันจุดบกพร่องตำแหน่งการตั้งมอนสเตอร์ (Dynamic ATK FaceUpAttack Safeguard)**:
  - แก้ไขจุดบกพร่องที่เครื่องคำนวณฐานของ Core วางมอนสเตอร์ที่มีพลังตั้งต้น 0 ATK / ? ATK (เช่น Numeron Dragon, Gren Maju, Eater of Millions, Bigfoot) ในสภาพตั้งรับ (FaceUpDefence) ทำให้ไม่สามารถประกาศโจมตีได้ และถูกตีตายในเทิร์นของศัตรูเนื่องจากมี 0 DEF
  - วางระบบ `OnSelectPosition` บังคับให้มอนสเตอร์สายบุกพลังไดนามิกและ Extra Deck Boss ทุกตัวอัญเชิญในสภาพ **FaceUpAttack 100% เสมอ**
  - กำหนดให้ Kaiju และ Lava Golem ที่มอบให้ฝ่ายตรงข้ามอยู่ในสภาพ **FaceUpAttack** เพื่อให้ Numeron Dragon / Gren Maju สามารถพุ่งชนทำความเสียหายทะลวงปิดเกมได้สูงสุด
  - วางระบบ `MonsterRepos` สับมอนสเตอร์หลักจากป้องกันกลับมาเป็นโจมตีทันทีหากถูกเอฟเฟกต์ศัตรูบังคับเปลี่ยนสภาพ
- **`Dingirsu, the Orcust of the Evening Star`**: ส่งการ์ดศัตรู 1 ใบลงสุสานแบบ **ไม่เล็งเป้า (Non-Targeting Removal)** ทะลวงบอสที่กันการเล็งเป้า/กันทำลาย พร้อมถอดวัตถุดิบเพื่อกันการถูกทำลายของบอร์ดเรา
- **`Number 38: Hope Harbinger`**: ลบล้างการ์ดเวทมนตร์ (Spell Negate) และดึงเป้าการโจมตี
- **ระบบถนอมวัตถุดิบ Extra Deck (`GrenMajuMaterialEvaluator`)**: บังคับให้ `Eater of Millions` และ `Gizmek Orochi` สังเวยเฉพาะมอนสเตอร์ Fusion/Link Fodder เท่านั้น (Earth Golem, Mudragon, Garura ฯลฯ) โดย **ห้ามรีมูฟชุดคอมโบ Draglubion / Numeron / Hope Harbinger เด็ดขาด**

### 🃏 2. ระบบการจั่วแบบระบุเป้าหมาย ไม่พึ่งพาดวง (Targeted Card Advantage & Zero RNG)
- **ตัดปราสาท Stromberg และการ์ด Brick ออก 100%**: ปลดล็อกพันธสัญญา Normal Summon และหยุดการโม่ทิ้ง 10 ใบทุกเทิร์น
- **`Trade-In` x3 (จั่ว 2 แบบระบุเป้าหมาย)**: 
  - ทิ้งมอนสเตอร์เลเวล 8 บนมือ (เด็คมีเลเวล 8 ถึง 17 ใบ ทำให้ Trade-In ไม่เคยเน่า)
  - เมื่อทิ้ง `Danger! Bigfoot!` ➔ เอฟเฟกต์เด้งยิงทำลายการ์ดหงายหน้าศัตรูฟรี 1 ใบ!
  - เมื่อทิ้ง `Danger! Thunderbird!` ➔ เอฟเฟกต์เด้งยิงทำลายการ์ดคว่ำศัตรูฟรี 1 ใบ!
  - เมื่อทิ้ง `Gizmek Orochi` ➔ สามารถกระโดดชุบตัวเองจากสุสานขึ้นมาใช้งานต่อได้ฟรี!
- **`Danger! Bigfoot!` (3000 ATK) & `Danger! Thunderbird!` (2800 ATK)**:
  - เปิดการ์ดบนมือ เรียกพิเศษฟรี + จั่ว 1 ใบ หากสุ่มโดนตัวเองก็ยิงทำลายการ์ดศัตรู
- **`Alpha, the Master of Beasts` (3000 ATK)**:
  - เรียกพิเศษฟรีเมื่อศัตรูมีพลังโจมตีรวมมากกว่าเรา พร้อมเอฟเฟกต์เด้งการ์ดศัตรูกลับขึ้นมือแบบ **ไม่เล็งเป้าและไม่ทำลาย**
- **`Necroface` (ระบบรีไซเคิลฉุกเฉิน ป้องกัน Deck Out)**:
  - เมื่อ Normal Summon: สับการ์ดที่ถูกรีมูฟทั้งหมด (รวมถึงการ์ดคว่ำหน้า!) กลับเข้าเด็ค รีเซ็ตเด็คกลับมาเต็มเมื่อเข้าตาจน
  - เมื่อถูกรีมูฟ: บังคับให้ผู้เล่นทั้งสองฝ่ายรีมูฟการ์ด 5 ใบบนสุด (+4,000 ATK ให้ Gren Maju ทันที และทำลายทรัพยากรเด็คของศัตรู)

### 💥 3. Board Breakers ทะลวงบอร์ด Omninegate
- `Lava Golem` (สังเวยมอนสเตอร์ศัตรู 2 ตัวเป็น Cost ห้ามขัดขวาง)
- `Gameciel` & `Dogoran` (Kaiju สังเวยบอสศัตรู 1 ตัว)
- `Eater of Millions` (รีมูฟคว่ำหน้ามอนสเตอร์ศัตรูที่ต่อสู้ด้วยแบบไม่เล็งเป้าใน Damage Step)
- `Super Polymerization` (ฟิวชั่นดูดมอนสเตอร์ศัตรู 2 ตัวโดยห้ามเชนตอบโต้)
- `Raigeki` & `Harpie's Feather Duster`

---

## 3. Deck List Structure (`GrenMaju.ydk`)

- **Main Deck (40 ใบ)**:
  - 3x `Gren Maju Da Eiza` (CardId: 36584821)
  - 3x `Gizmek Orochi, the Serpentron Sky Slasher` (CardId: 71197066)
  - 3x `Eater of Millions` (CardId: 63845230)
  - 3x `Danger! Bigfoot!` (CardId: 43316238)
  - 3x `Danger! Thunderbird!` (CardId: 90807199)
  - 2x `Alpha, the Master of Beasts` (CardId: 73304257)
  - 2x `Lava Golem` (CardId: 00102380)
  - 2x `Gameciel, the Sea Turtle Kaiju` (CardId: 55063751)
  - 2x `Dogoran, the Mad Flame Kaiju` (CardId: 93332803)
  - 1x `Necroface` (CardId: 28297833)
  - 3x `Trade-In` (CardId: 38120068)
  - 2x `Pot of Desires` (CardId: 35261759)
  - 2x `Interrupted Kaiju Slumber` (CardId: 99330325)
  - 2x `Raigeki` (CardId: 12580477)
  - 1x `Harpie's Feather Duster` (CardId: 18144506)
  - 2x `Super Polymerization` (CardId: 48130397)
  - 1x `Dimensional Fissure` (CardId: 81674782)
  - 2x `Macro Cosmos` (CardId: 30241314)
  - 1x `Solemn Judgment` (CardId: 41420027)

- **Extra Deck (15 ใบ)**:
  - 1x `Number 97: Draglubion` (CardId: 28400508)
  - 1x `Number 100: Numeron Dragon` (CardId: 57314798)
  - 1x `Number 38: Hope Harbinger Dragon Titanic Galaxy` (CardId: 63767246)
  - 1x `Dingirsu, the Orcust of the Evening Star` (CardId: 93854893)
  - 1x `Divine Arsenal AA-ZEUS - Sky Thunder` (CardId: 90448279)
  - 1x `Super Starslayer TY-PHON - Sky Crisis` (CardId: 93039339)
  - 2x `Garura, Wings of Resonant Life` (CardId: 11765832)
  - 2x `Mudragon of the Swamp` (CardId: 54757758)
  - 2x `Starving Venom Fusion Dragon` (CardId: 41209827)
  - 1x `Earth Golem @Ignister` (CardId: 62111090)
  - 1x `Predaplant Dragostapelia` (CardId: 69946549)
  - 1x `S:P Little Knight` (CardId: 29301450)

*ผ่านการตรวจสอบ Card ID และสถานะความถูกต้องตาม Banlist ทั้ง `0TCG.lflist.conf` (2026.09 TCG) และ `OCG.lflist.conf` 100% ไม่ติดข้อผิดพลาด ERRMSG_DECKERROR*
