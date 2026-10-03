# Tenpai Dragon (天盃龍 / Sangenpai) Architecture & Playbook

> **Version**: 1.0.0 (2026-10-03)  
> **Architecture**: Rule-Based `ModernExecutor` + Decoupled `TenpaiPlugin` (`DeckPluginBase`)  
> **Target Framework**: .NET 10.0 (C#)  
> **Deployment Target**: `C:\Users\admin\Documents\EdoGame\`  

---

## 1. Archetype Overview & Card Audit (การวิเคราะห์การ์ดและระดับเลเวลที่แท้จริง)

Tenpai Dragon (และชุดเวทกับดัก Sangen) เป็นหนึ่งในเด็ค **Going-Second OTK** ที่ทรงพลังที่สุดในประวัติศาสตร์ Yu-Gi-Oh! โดยมีความสามารถพิเศษในการทำ **Synchro Summon แบบ Quick Effect ระหว่าง Battle Phase** พร้อมการปกป้องจากสนาม `Sangen Summoning` ที่ทำให้มอนสเตอร์มังกรไฟทั้งหมดไม่รับผลการ์ดที่ถูกเปิดใช้งานของศัตรูใน Main Phase 1

### การตรวจสอบสเตตัสและการคำนวณเลเวลจริง (Mathematical Calibration)
จากการตรวจสอบ `cards.cdb` พบว่าโครงสร้างเลเวลและบทบาทของการ์ดมีดังนี้:

| Card ID | Card Name | Type / Attribute | Level | ATK / DEF | Real Effect Summary | Role in Deck |
|---|---|---|---|---|---|---|
| **39931513** | Tenpai Dragon Paidra | Dragon / FIRE | **Level 3** | 1700 / 1000 | ลงสนามเสิร์ชการ์ด Sangen; ไม่รับดาเมจต่อสู้จากการต่อสู้ของมังกรไฟ; BP Quick Synchro | **Starter / Searcher** |
| **91810826** | Tenpai Dragon Chundra | Dragon / FIRE / **Tuner** | **Level 4** | 1500 / 1000 | โดดจากมือเมื่อมีมังกรไฟ; เมื่อต่อสู้เสิร์ชโดด Fadra จากเด็ค; BP Quick Synchro | **Primary Tuner / Swarmer** |
| **65326118** | Tenpai Dragon Fadra | Dragon / FIRE | **Level 3** | 1600 / 1000 | มังกรไฟไม่ถูกทำลายจากการต่อสู้; ลงสนาม/เมื่อต่อสู้ชุบมังกรไฟจากสุสาน; BP Quick Synchro | **Extender / Protection** |
| **23657016** | Tenpai Dragon Genroku | Dragon / FIRE | **Level 3** | 0 / 1000 | เมื่อถูกเพิ่มขึ้นมือ โดดตัวเองเป็นตัวจูน (เลือกปรับ Level 4 ได้); สังเวยตัวเองโดด Chundra/Paidra จากเด็ค | **Free Extender / Bridge** |
| **30336082** | Sangen Summoning | Field Spell | - | - | MP1 มังกรไฟไม่รับผลเปิดใช้งานของศัตรู; เสิร์ช Tenpai แล้วทิ้งการ์ด 1 ใบ; **ถูกทำลายใน BP: เบิ้ล ATK มังกร Synchro 2 เท่า!** | **Blanket Shield / OTK Multiplier** |
| **66730191** | Sangen Kaimen | Quick-Play Spell | - | - | MP: เสิร์ชหรือโดดมังกรไฟ; **BP: เสิร์ชแล้วโดดลงสนามได้พร้อมกันทันที!** | **Searcher / Battle Swarmer** |
| **55484152** | Sangen Furo | Continuous Spell | - | - | ชุบมอนสเตอร์ที่ตายจากการต่อสู้ของมังกรไฟ; End Phase จ่าย 1000 เซ็ตการ์ด Sangen จากสุสาน | **Battle Recovery** |
| **25388971** | Sangen Kaiho | Normal Trap | - | - | ถ้าศัตรูมีมอนสเตอร์มากกว่า ข้าม MP1 ศัตรูเข้า BP ทันที; ในสุสาน banish เพื่อจั่ว 1 และโดด Tenpai จากมือ | **Going 1st Control / Turn Skip** |

---

## 2. Ace Monsters คืออะไร? บทบาทและการควบคุมของบอท

ในเด็ค Tenpai มีมอนสเตอร์ Ace สำคัญ 3 ตัวที่ทำงานประสานกันเป็นบันได Synchro:

### Ace 1: The OTK Finisher Ace — `Trident Dragion` (Level 10 FIRE Dragon, 3000 ATK)
- **บทบาท**: ตัวปิดเกมสร้างความเสียหายระดับมหาศาล (18,000+ Damage)
- **เงื่อนไขและการทำงาน**:
  - ต้องจูนด้วย Dragon Tuner + 1+ Dragon non-Tuners (จูนด้วย Bident Dragion Level 7 Tuner + Paidra/Fadra Level 3 = Level 10)
  - เมื่อ Synchro Summon สำเร็จ: สั่งทำลายการ์ดบนสนามเราได้สูงสุด 2 ใบ เพื่อให้ Trident ได้สิทธิ์ **โจมตีเพิ่มตามจำนวนการ์ดที่ถูกทำลาย (สูงสุด 3 ครั้ง!)**
  - **คอมโบไม้ตายสุดโกง**: ให้เลือกทำลาย `Sangen Summoning` (Field Spell)! เมื่อ Sangen Summoning ถูกทำลายใน Battle Phase เอฟเฟกต์สุสานจะทำงานเพื่อ **เพิ่มพลังโจมตีของ Trident Dragion เป็น 2 เท่า กลายเป็น 6,000 ATK**!
  - Trident โจมตี 3 ครั้งที่ 6,000 ATK = **18,000 Damage OTK ในเทิร์นเดียว!**
- **กฎเหล็กของบอท**: **ห้ามอัญเชิญ Trident Dragion ในเทิร์น 1 (Going First) โดยเด็ดขาด!** เพราะ Trident มีไว้เพื่อทำลายการ์ดตนเองเอาสิทธิ์โจมตี การลงในเทิร์น 1 จะทำให้เสียสนามฟรีโดยไม่ได้โจมตี

### Ace 2: The Lockdown Ace — `Sangenpai Transcendent Dragion` (Level 10 FIRE Dragon, 3000 ATK / 3000 DEF)
- **บทบาท**: ปิดกั้นการตอบโต้ของศัตรูอย่างสมบูรณ์แบบใน Battle Phase
- **เงื่อนไขและการทำงาน**:
  - **Complete Battle Phase Silence**: "ศัตรูไม่สามารถเปิดใช้งานการ์ดหรือเอฟเฟกต์ใดๆ ได้เลยในระหว่าง Battle Phase" (หมดสิทธิ์ใช้ Gorz, Bystial, Nibiru ใน BP, กับดักต่อสู้ หรือเอฟเฟกต์ลอยในสุสาน)
  - เมื่อ Synchro สำเร็จ: ปรับมอนสเตอร์ทุกตัวในสนามให้อยู่ในสภาพหงายหน้าโจมตี
  - ในเทิร์นศัตรู: บังคับมอนสเตอร์ทุกตัวของศัตรูให้ต้องสั่งโจมตี (ทำให้ชนกำแพง 3000+ ATK ของเราหรือกระตุ้นเอฟเฟกต์ Chundra/Fadra)
  - เอฟเฟกต์ในสุสาน: หากมีการประกาศโจมตีครบ $\ge 3$ ครั้งในเทิร์นนี้ สามารถโดดตัวเองกลับขึ้นมาสนามและทำลายการ์ดศัตรู 1 ใบ

### Ace 3: The Combo Bridge Ace — `Sangenpai Bident Dragion` (Level 7 FIRE Dragon Tuner, 2600 ATK)
- **บทบาท**: สะพานเชื่อมบันได Synchro และสร้างวัตถุดิบคืนสนาม
- **เงื่อนไขและการทำงาน**:
  - จูนด้วย Chundra (4 Tuner) + Paidra/Fadra (3 non-Tuner) = Level 7 Tuner
  - เมื่อ Synchro สำเร็จ: **ชุบชีวิตมังกรไฟ Level 3 (Paidra หรือ Fadra) กลับมาจากสุสานทันที!**
  - ทำให้บนสนามมี Bident Dragion (Level 7 Tuner) + Paidra (Level 3 non-Tuner) พร้อมใช้จูนต่อเป็น Level 10 (Trident หรือ Transcendent) ทันทีใน Battle Phase!
  - เอฟเฟกต์ในสุสาน: หากมีการโจมตีครบ $\ge 3$ ครั้ง โดดกลับมาสนามพร้อมทำลายเวท/กับดักศัตรู 1 ใบ

---

## 3. Playbook Strategy: เดินก่อน (1st) และ เดินหลัง (2nd)

### ก. แผนการเล่นเมื่อเดินหลัง (Going Second — The Primary OTK God Route)
เป้าหมาย: **เคลียร์บอร์ดศัตรู กางร่มกันเอฟเฟกต์ แล้วปิดเกมด้วย 18,000 OTK**

1. **Step 1: Board Breaking (Main Phase 1)**
   - สกัดกั้น/ทำลายบอร์ดศัตรูด้วยบอร์ดเบรกเกอร์:
     - `Dark Ruler No More`: ปิดเอฟเฟกต์มอนสเตอร์หงายหน้าทั้งหมดของศัตรู
     - `Lightning Storm`: เลือกล้างเวทกับดัก (หากมี $\ge 2$ ใบ) หรือล้างมอนสเตอร์โหมดโจมตี
     - `Super Polymerization`: ฟิวชั่นมอนสเตอร์ศัตรู 2 ตัวกลายเป็น Garura, Mudragon หรือ Starving Venom โดยศัตรูเปิดเชนตอบโต้ไม่ได้
     - `Forbidden Droplet`: ส่งการ์ดส่วนเกินลงสุสานเพื่อเนเกทและลดพลังโจมตีศัตรูลงครึ่งหนึ่ง
2. **Step 2: Blanket Immunity (Main Phase 1)**
   - **เปิด `Sangen Summoning` (Field Spell) เป็นอันดับแรกก่อนลงมอนสเตอร์!**
   - เมื่อสนามทำงาน มังกรไฟทั้งหมดจะ **ไม่รับผลเอฟเฟกต์ที่ถูกเปิดใช้งานของศัตรูใน MP1** (กัน Veiler, Imperm, Nibiru, Ghost Ogre, หรือเอฟเฟกต์ยิงขัดขวาง 100%)
   - สั่งใช้เอฟเฟกต์สนาม: เสิร์ช `Chundra` (ตัวจูน) ขึ้นมือ แล้วทิ้งการ์ดส่วนเกิน (เช่น Sangen Kaiho ซึ่งมีเอฟเฟกต์ในสุสาน)
   - Normal Summon `Paidra`: เสิร์ช `Sangen Kaimen`
   - Special Summon `Chundra` ลงมาจากมือ
   - **ห้าม Overextend ทำ Synchro ใน MP1**: ให้กางมอนสเตอร์ไว้แล้ว **กดเข้าสู่ Battle Phase ทันที!**
3. **Step 3: Battle Phase OTK Synchro Ladder**
   - สั่ง Paidra (1700), Chundra (1500) โจมตี
   - Chundra ทำงานตอนเริ่มการต่อสู้: โดด `Fadra` จากในเด็ค
   - Fadra ทำงานตอนลงสนาม/เริ่มการต่อสู้: ชุบมังกรไฟจากสุสาน
   - สั่ง Quick Synchro: Chundra (4 Tuner) + Paidra (3 non-Tuner) $\rightarrow$ อัญเชิญ **`Sangenpai Bident Dragion` (Level 7 Tuner, 2600 ATK)**
   - Bident Dragion ชุบ Paidra (Level 3) ขึ้นมาจากสุสาน
   - สั่ง Bident Dragion โจมตี
   - สั่ง Quick Synchro: Bident Dragion (7 Tuner) + Paidra (3 non-Tuner) $\rightarrow$ อัญเชิญ **`Trident Dragion` (Level 10, 3000 ATK)**
   - Trident Dragion สั่งทำลาย `Sangen Summoning` + การ์ดส่วนเกิน 1 ใบ $\rightarrow$ ได้สิทธิ์โจมตี 3 ครั้ง
   - `Sangen Summoning` ในสุสานทำงาน $\rightarrow$ **เพิ่ม ATK ของ Trident Dragion เป็น 6,000 ATK!**
   - Trident โจมตี 3 ครั้งที่ 6,000 ATK = **18,000 Damage ปิดฉากเกม!**
   - หากศัตรูยังไม่ตาย: Bident Dragion ในสุสานจะโดดกลับขึ้นมา (2600 ATK) ยิงทำลายการ์ดหลังศัตรู และ Transcendent Dragion จะโดดกลับขึ้นมา (3000 ATK) ช่วยรุมโจมตี

---

### ข. แผนการเล่นเมื่อถูกบังคับเดินก่อน (Going First Turn 1 Control Strategy)
เป้าหมาย: **ตั้งบอร์ดขัดขวางที่ปลอดภัย เซ็ตกับดักตัดเทิร์นศัตรู และรักษาทรัพยากรไว้ปิดเกมเทิร์น 3**

1. **ห้ามอัญเชิญ Trident Dragion ในเทิร์น 1 เด็ดขาด** (เพราะจะระเบิดสนามตัวเองทิ้งฟรีโดยไม่มี Battle Phase)
2. **ตัวเลือกบอร์ดจบเทิร์น 1**:
   - **Route A (Hieratic Seal Link-2 Route)**:
     - กาง Paidra + Chundra/Genroku $\rightarrow$ Link Summon **`Hieratic Seal of the Heavenly Spheres` (Link-2)**
     - ในเทิร์นศัตรู: สังเวยตัวเองเด้งการ์ดศัตรูกลับขึ้นมือ 1 ใบ (Non-targeting bounce) และโดดมังกรไฟจากเด็ค (Paidra เพื่อเสิร์ชต่อ หรือ Fadra เพื่อกันการทำลาย)
   - **Route B (Transcendent Dragion Route)**:
     - กาง Bident Dragion (7) $\rightarrow$ ชุบ Paidra (3) $\rightarrow$ จูนต่อเป็น **`Sangenpai Transcendent Dragion` (Level 10, 3000 DEF)**
     - ในเทิร์นศัตรู: มอนสเตอร์ศัตรูทุกตัวถูกบังคับให้ต้องสั่งโจมตีชนบอส และศัตรูจะไม่สามารถใช้เอฟเฟกต์ใดๆ ได้เลยใน Battle Phase
3. **การเซ็ตหลัง**:
   - เซ็ต **`Sangen Kaiho` (Normal Trap)**: เมื่อศัตรูลงมอนสเตอร์ใน Main Phase จนมีจำนวนมากกว่าเรา ให้เปิดใช้งานทันทีเพื่อ **ข้าม Main Phase 1 ของศัตรูตรงเข้าสู่ Battle Phase** บังคับให้ศัตรูไม่สามารถเซ็ตอัปคอมโบหรือลงบอสใหญ่ได้
   - เซ็ต **`Super Polymerization`**, **`Forbidden Droplet`**, และ **`Infinite Impermanence`**
   - ถือ **`Ash Blossom`**, **`Ghost Belle`**, และ **`Nibiru`** ไว้บนมือคอยดัก Chokepoint

---

## 4. สถาปัตยกรรมระบบ (Decoupled Domain Plugin Architecture)

เพื่อให้สอดคล้องกับมาตรฐานส่วนกลางของโปรเจกต์ โค้ดของ Tenpai ได้รับการแยกส่วนออกจาก Routing อย่างเด็ดขาด:

```
┌────────────────────────────────────────────────────────┐
│                      CENTRAL CORE                      │
│   CardIntelligence / BaitPlanner / BoardScorer         │
│   ChainAdvisor / ThreatEvaluator / FallbackSelectCard  │
└───────────────────────────┬────────────────────────────┘
                            │
┌───────────────────────────▼────────────────────────────┐
│                     MODERN EXECUTOR                    │
│            TenpaiExecutor (ModernExecutor)             │
│   - Registration of Tiers 0-5                          │
│   - ComboRouter Lines                                  │
│   - OnSelectYesNo / OnSelectOption / Callbacks         │
└───────────────────────────┬────────────────────────────┘
                            │
┌───────────────────────────▼────────────────────────────┐
│                    DECK DOMAIN PLUGIN                  │
│             TenpaiPlugin : DeckPluginBase              │
│                                                        │
│  ├─ TenpaiStrategy (IDeckStrategy)                     │
│  │   • PickSpecialSummonTarget (Bident/Chundra/Fadra)  │
│  │   • PickSearchTarget (Summoning/Paidra/Kaimen)      │
│  │                                                     │
│  ├─ TenpaiMaterialEvaluator (IDeckMaterialEvaluator)   │
│  │   • GetMaterialCost (Ace protection)                │
│  │   • PickDiscardTarget (Sangen Kaiho / Fodder)       │
│  │   • PickDestructionSubstitute (Pop Summoning first) │
│  │                                                     │
│  ├─ TenpaiThreatEvaluator (IDeckThreatEvaluator)       │
│  │   • Threat scoring for enemy boards                 │
│  │                                                     │
│  └─ TenpaiBattleOTKPlanner                             │
│      • Lethal board calculation (18,000 Trident burst) │
└────────────────────────────────────────────────────────┘
```

---

## 5. การลงทะเบียนในระบบ (System Registration)

1. **`bots.json` (Central Bot Registry)**:
   ```json
   {
     "name": "Tenpai",
     "deck": "Tenpai",
     "difficulty": 3,
     "dialog": "default",
     "flags": [ "OCG", "TCG" ],
     "masterRules": [ 4, 5 ]
   },
   {
     "name": "Tenpai Dragon",
     "deck": "Tenpai",
     "difficulty": 3,
     "dialog": "default",
     "flags": [ "OCG", "TCG" ],
     "masterRules": [ 4, 5 ]
   },
   {
     "name": "TenpaiDragon",
     "deck": "Tenpai",
     "difficulty": 3,
     "dialog": "default",
     "flags": [ "OCG", "TCG" ],
     "masterRules": [ 4, 5 ]
   }
   ```
2. **DashBot WPF Launcher (`dashbot`)**:
   - เพิ่มชื่อ `"Tenpai"` และ `"Tenpai Dragon"` ในรายการเลือกลิสต์เด็คของ DashBot Launcher
3. **Deck Lists (`.ydk`)**:
   - `C:\Users\admin\Documents\EdoGame\src\YGO_SOURCE_CLEAN\windbot-fork\Decks\Tenpai.ydk`
   - `C:\Users\admin\Documents\EdoGame\WindBot\Decks\Tenpai.ydk`
   - `C:\Users\admin\Documents\EdoGame\deck\Tenpai.ydk`
4. **Deploy Pipeline**:
   - คอมไพล์ผ่าน `BUILD_AND_DEPLOY.ps1`
   - ติดตั้งไบนารี (`WindBot.dll`, `ExecutorBase.dll`, `core.dll`, `bots.json`, `cards.cdb`, `DashBot.exe`) สู่ `C:\Users\admin\Documents\EdoGame\` ครบถ้วน 100%
