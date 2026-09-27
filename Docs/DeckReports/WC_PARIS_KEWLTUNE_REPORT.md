# รายงานการพัฒนา WCParisKewlTune & ปรับปรุง Kwtune ModernExecutor

**วันที่**: 2026-09-21  
**เป้าหมายหลัก**:
1. พัฒนาและเพิ่มเด็คบอทระดับเวิลด์แชมเปียนชิป `WCParisKewlTune` (World Championship Paris Kewl Tune Deck)
2. วิเคราะห์ Side Deck เพื่อยกระดับความสามารถในการเล่นของบอท (นำการ์ดที่จำเป็นเข้า Main Deck)
3. Refactor และอัปเกรดสมอง AI (`_2026_KwtuneExecutor.cs`) ขนานใหญ่ ให้รองรับทั้ง `WCParisKewlTune` และ `2026_Kwtune`
4. Build และ Deploy ไบนารีชุดสมบูรณ์ไปยัง `C:\Users\admin\Documents\EdoGame\` โดยตรง

---

## 1. ผลการวิเคราะห์และปรับปรุงโครงสร้างเด็ค (Side Deck to Main Deck Swap)

### 1.1 ปัญหาทางยุทธวิธีเดิมของเด็ค
- **Extra Deck มี `Visas Amritara` (ID: 821049)** ซิงโครเลเวล 8 (Tuner) ซึ่งมีความสามารถเมื่ออัญเชิญซิงโครสำเร็จ: *เพิ่มการ์ดเวทมนตร์/กับดักที่ระบุชื่อ "Visas Starfrost" 1 ใบจากเด็คขึ้นมือ*
- แต่ใน Main Deck เดิม ไม่มีเวทมนตร์/กับดักสาย Visas เลยแม้แต่ใบเดียว ขณะที่ **`Mannadium Reframing` (ID: 18158393)** (เคาน์เตอร์แทรป Omni-Negate ที่ระบุชื่อ Visas Starfrost) กลับถูกใส่ไว้ใน Side Deck
- หากบอทซิงโครเรียก Amritara จะทำให้เอฟเฟกต์เสิร์ชล้มเหลวและเสียทรัพยากรฟรี

### 1.2 การปรับปรุงสลับการ์ดเพื่อความเหมาะสมของบอท
| การ์ดที่ย้ายจาก Main -> Side (-4 ใบ) | การ์ดที่ย้ายจาก Side -> Main (+4 ใบ) | ผลลัพธ์เชิงยุทธวิธี |
|---|---|---|
| `Ghost Belle & Haunted Mansion` x2 (เหลือ 1 ใน Main) | `Mannadium Reframing` x1 | ปลดล็อกเอฟเฟกต์ `Visas Amritara` ให้บอทได้ Omni-negate Counter Trap บนสนาม |
| `Synchro Emergency` x2 (เหลือ 1 ใน Main) | `Called by the Grave` x1 | ป้องกัน Handtrap ของคู่ต่อสู้ที่จ้องตัดตอนคอมโบหลัก (`Cue` / `Track Maker`) |
| | `Duelist Genesis` x1 | ทำหน้าที่เป็น Starter เพิ่มเติม ค้นหา `Kewl Tune Synchro` หรือ `Synchro Emergency` |
| | `Triple Tactics Talent` x1 | ลงโทษคู่แข่งที่เปิด Handtrap ใน Main Phase (จั่ว 2 / ขโมยมอนสเตอร์ / ริบการ์ดบนมือ) |

*หมายเหตุ: ไฟล์เด็คเดิมของผู้ใช้ได้รับการสำรองไว้อย่างปลอดภัยที่ `deck/WCParisKewlTune_Original.ydk`*

---

## 2. Card Audit Dossier (ข้อมูลการ์ดจริง 100% จาก cards.cdb)

| ID | Card Name | Lv | ATK/DEF | Tuner? | Role | Interaction Summary |
|---|---|---|---|---|---|---|
| `16387555` | Kewl Tune Cue | 3 | 900/1900 | **Yes** | Core Starter | NS -> SS Tuner จากเด็ค (ล็อก SS เฉพาะ Tuner); Hand-Sync ได้; สุสาน -> ขุด 2 แบน 1 |
| `17209452` | Kewl Tune Rotary | 1 | 100/800 | **Yes** | Extender | โชว์บนมือ -> NS Tuner เพิ่ม; Hand-Sync ได้; สุสาน -> สับสุสานศัตรูกลับเด็ค หรือค้นหา KT S/T |
| `89392810` | Kewl Tune Reco | 3 | 1500/800 | **Yes** | Searcher / Removal | NS/SS -> เสิร์ช KT ไม่ใช่ Lv3; Hand-Sync ได้; สุสาน -> ทำลาย S/T ศัตรู |
| `43904702` | Kewl Tune Clip | 2 | 800/1000 | **Yes** | Disrupter | Quick บนมือเทิร์นศัตรู -> SS ตัวเอง + ซิงโคร Tuner Synchro; สุสาน -> สุ่มแบน Extra Deck ศัตรู |
| `16509007` | Kewl Tune Mix | 2 | 100/2000 | **Yes** | Searcher / Removal | NS/SS -> เสิร์ช KT ไม่ใช่ Lv2 (Mix เป็น Warrior); สุสาน -> ทำลายมอนสเตอร์ศัตรู |
| `13021682` | Starjunk Synchron | 3 | 1300/500 | **Yes** | Extender | SS จากบนมือเมื่อควบคุม Warrior (Mix) หรือ Synchron; ซิงโครแทน Junk |
| `9742784` | Jet Synchron | 1 | 500/0 | **Yes** | GY Extender | ทิ้งการ์ด 1 ใบชุบตัวเองจากสุสาน |
| `70088809` | Fidraulis Harmonia | 7 | 2500/2000 | **Yes** | Hand Disrupter | Quick เมื่อศัตรูใช้เอฟเฟกต์มอนสเตอร์: โชว์ 5 Synchro -> SS ตัวเอง + ส่ง Synchro ลงสุสาน (เช่น Malong เด้งการ์ด) + ทำลายมอนสเตอร์ศัตรู |
| `14442329` | JJ "Kewl Tune" | - | Spell (Field) | - | Field Starter | NS Tuner เพิ่มอีก 1 ตัว; บูชายัญ 1 Tuner เสิร์ช/SS KT จากเด็ค; บัฟ Loudness War +3300 ATK |
| `78058681` | Kewl Tune Synchro | - | Quick Spell | - | Core Enabler | เสิร์ช KT 1 ใบ + สั่งซิงโคร Tuner Synchro ทันที (เปิดได้ 2 ครั้ง/เทิร์น) |
| `97474300` | Duelist Genesis | - | Normal Spell | - | Rota Spell | มี Tuner บนสนาม/สุสาน -> เสิร์ช Kewl Tune Synchro หรือ Synchro Emergency |
| `99243014` | Synchro Overtake | - | Normal Spell | - | SS Spell | โชว์ Synchro ใน ED -> SS มอนสเตอร์ที่ระบุชื่อ (Remix -> Mix, RS -> Reco, Crackle -> Clip) |
| `18158393` | Mannadium Reframing | - | Counter Trap | - | Omni-Negate | คุมซิงโคร: เนเกทและทำลายเวท/กับดัก/มอนสเตอร์เมื่อมี Visas Starfrost (Amritara) |
| `42781164` | Kewl Tune Track Maker | 4 | 0/2500 | **Yes** | Bridge Synchro | SS -> เสิร์ชการ์ด KT; สุสาน -> เด้งการ์ดศัตรูขึ้นมือ |
| `15665977` | Kewl Tune RS | 5 | 1700/2400 | **Yes** | Disruption Synchro | Quick (MP) แบน Tuner ในสุสาน -> เนเกทการ์ดหงายหน้าศัตรูจนจบเทิร์น |
| `88170262` | Kewl Tune Remix | 5 | 1500/2000 | **Yes** | Ladder Synchro | Quick เทิร์นศัตรู สังเวยตัวเอง -> เก็บ 1 เรียก 1 Tuner จากสุสาน + สั่งซิงโครต่อ |
| `39576656` | Kewl Tune Crackle | 5 | 800/2000 | **Yes** | ED Stripper | ซิงโครลงมา -> แบน Extra Deck ศัตรู; ตกสุสาน -> ชุบตัวเอง + แบน Extra Deck ศัตรูอีก 2 ใบ |
| `41069676` | Kewl Tune Loudness War | 6 | 0/3000 | **Yes** | Wall / Protector | ป้องกัน Tuner อื่นจากการถูกเล็งเป้าและถูกทำลาย; Quick แบน KT ในสุสานก๊อปปี้เอฟเฟกต์ตกสุสาน |
| `93125329` | Golden Cloud Beast - Malong| 6 | 2200/1000 | **Yes** | Utility Synchro | ตกสุสาน (ผ่าน Harmonia หรือซิงโคร) -> เด้งการ์ดหงายหน้าศัตรู 1 ใบ |
| `4891376` | Zalen the Shackled Dragon | 7 | 2800/2100 | **Yes** | Omni-Negate Boss | Quick Effect เนเกทและทำลาย chain link 1 หรือ 2 |
| `821049` | Visas Amritara | 8 | 2500/2100 | **Yes** | Searcher / Boss | ชื่อเป็น Visas Starfrost; ซิงโครสำเร็จเสิร์ช `Mannadium Reframing` ขึ้นมาเซ็ต |
| `65961304` | Kewl Tune Back 2 Back | 10 | 3200/0 | **Yes** | Primary Finisher | Tuner ทุกตัวโจมตีได้ 2 ครั้ง; Quick Effect ชุบ Tuner จากสุสาน/แบน แล้วซิงโครต่อ (ใช้ได้ 2 ครั้ง/เทิร์น) |

---

## 3. การ Refactor & อัปเกรดความฉลาดของสมองกลบอท (`_2026_KwtuneExecutor.cs`)

1. **แก้ไขข้อผิดพลาดของการ์ดเดิม (Correction of False Assumptions)**:
   - แก้ไข `Kewl Tune Clip` เป็น Level 2 (800/1000) จากเดิมที่ฮาร์ดโค้ดผิดเป็น Lv 1 (0/0)
   - แก้ไข `Zalen the Shackled Dragon` ให้เป็น **Tuner Synchro** อย่างถูกต้อง (เดิมมีโค้ดบั๊กบล็อกการซิงโครเมื่อติด `_tunerOnlyLock`)
   - แก้ไขรหัส `Ash Blossom` เป็น 14558128 และ `Harpies Feather Duster` เป็น 18144507
2. **ขจัด Hardcoded Array ทั้งหมด**:
   - ลบ `OpponentFloodgateCards` แบบฮาร์ดโค้ดออก 100% และเปลี่ยนมาใช้ `CardIntelligence.IsFloodgate`, `CardIntelligence.IsKnownNegator`, `CardIntelligence.IsSpecialSummonBlocked` และ `CardIntelligence.IsTargetImmune` แทน
3. **ระบบการเลือกการ์ดอัจฉริยะ (Strategic `OnSelectCard`)**:
   - **Hint 512 (Hand-Sync & Synchro Materials)**: ปกป้อง Handtrap (Ash, Belle, Veiler, Ogre, Droll) และ Ace Card ไม่ให้นำไปสังเวยซิงโครโดยเด็ดขาด คัดเลือก Tuner ตามประโยชน์ของเอฟเฟกต์ตกสุสาน (Mix ทำลายมอนสเตอร์, Reco ทำลาย S/T, Rotary เสิร์ช/สับสุสาน, Clip แบน Extra Deck)
   - **Hint 502 (Destroy Targets)**: เล็ง Chokepoint / Floodgate ตามฐานข้อมูลกลาง และหลีกเลี่ยงการยิงใส่มอนสเตอร์ที่กันทำลาย/กันตกเป็นเป้า
   - **Hint 509 (Special Summon Targets)**: จัดลำดับการเรียกตัวเปิดคอมโบ เช่น Cue เรียก Rotary (Lv 1) เพื่อพร้อมต่อยอดขึ้น Track Maker (Lv 4) ทันที
   - **Fidraulis Harmonia Integration**: สั่งโชว์ซิงโคร 5 ใบและเลือกดัมพ์ `Golden Cloud Beast - Malong` เพื่อเด้งมอนสเตอร์ศัตรูทันทีในเทิร์นของคู่แข่ง
   - **Visas Amritara Integration**: สั่งเสิร์ช `Mannadium Reframing` ขึ้นมือเพื่อนำมาเซ็ต Omni-Negate ปิดกระดาน
4. **การรองรับทั้งสองเด็คอย่างสมบูรณ์**:
   - ออกแบบ Class Hierarchy ให้ `_2026_KwtuneExecutor` เป็น Base Engine หลัก และสร้าง `WCParisKewlTuneExecutor` สืบทอดคุณสมบัติไปใช้งานร่วมกันอย่างไร้รอยต่อ

---

## 4. สถานะการลงทะเบียนและการ Deploy

- **การลงทะเบียนใน `bots.json`**:
  - `WCParisKewlTune` (Deck: `WCParisKewlTune`, Difficulty: 3)
  - `Expert_WCParisKewlTune` (Deck: `WCParisKewlTune`, Difficulty: 3)
  - `2026_Kwtune` (Deck: `2026_Kwtune`, Difficulty: 3)
  - `Expert_2026_Kwtune` (Deck: `2026_Kwtune`, Difficulty: 3)
- **การจัดหมวดหมู่ใน DashBot WPF Launcher**:
  - เพิ่ม `WCParisKewlTune`, `Kwtune`, `KewlTune` เข้าสู่หมวดหมู่ **Modern Archetypes** (แสดงแท็กสีทอง "Modern" สวยงามและค้นหาง่าย)
- **Deployment สถานะ**:
  - คอมไพล์ผ่าน .NET 10.0 SDK: **0 Errors / 0 Warnings ใหม่**
  - ติดตั้งไบนารี (`WindBot.dll`, `ExecutorBase.dll`, `core.dll`, `DashBot.exe`, `bots.json`, `.ydk`) ลงใน `C:\Users\admin\Documents\EdoGame\` ครบถ้วน 100%
