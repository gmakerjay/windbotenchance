# Memento (Mementotlan) Deck Analysis & AI Playbook
**Document Version:** 1.0.0 (2026-10-02)  
**Standard:** YugiohTH Rule-Based ModernExecutor & Decoupled Deck Plugin Architecture  
**Target Deck:** `2026_Memento.ydk` / `Memento.ydk` (Main: 59 cards, Extra: 15 cards, Side: 0 cards)  
**Deployment Target:** `C:\Users\admin\Documents\EdoGame\`  

---

## 1. Executive Summary & Archetype Overview

**Memento (Mementotlan / メメント)** เป็นเด็คประเภท Modern Beatdown & Disruption Control ที่ใช้กลไก **"Self-Destruction & Recursive Advantage"** ในการขับเคลื่อนทรัพยากร:
- การ์ดมอนสเตอร์ขนาดเล็กเกือบทุกใบมี Effect ในการ **ทำลายมอนสเตอร์ Memento บนสนามตนเอง** เพื่อค้นหา (Search), ชุบชีวิต (Revive), สเปเชียลซัมมอนจากเด็ค (Summon from Deck), หรือส่งลงสุสาน (Foolish Burial)
- มีมอนสเตอร์บอสหลัก **"Mementoal Tecuhtlica - Combined Creation"** พลังโจมตีมหาศาล **5,000 ATK / 5,000 DEF** ซึ่งสามารถอัญเชิญพิเศษจากบนมือหรือสุสานได้โดยการรีไซเคิลการ์ด Memento 5 ใบที่มีชื่อต่างกันกลับเข้าเด็ค และมีความสามารถในการ **โจมตีมอนสเตอร์ของฝ่ายตรงข้ามได้ทุกตัว ตัวละหนึ่งครั้ง**
- มีตัวขัดขวางและเสริมบอร์ดที่แข็งแกร่ง:
  - **Mementomictlan Tecuhtlica - Creation King (Fusion L9 / 3000 ATK)**: Quick Effect ทำลายการ์ดบนสนามฝ่ายตรงข้ามตามจำนวนการ์ด Memento บนสนามเรา และดัมพ์การ์ด Memento 3 ใบจากเด็ค/Extra Deck ลงสุสาน
  - **Mementotlan Mace**: Quick Effect ขโมยมอนสเตอร์ฝ่ายตรงข้ามมาควบคุมจนจบเทิร์น เมื่อมี Combined Creation บนสนาม
  - **Mementotlan Goblin**: Quick Effect มอบสถานะป้องกันการตกเป็นเป้าหมาย (Target Immunity) ให้การ์ด Memento ทั้งสนาม
  - **Mementomictlan (Field Spell)**: ล็อกเวทมนตร์/กับดักคู่แข่งไม่ให้ทำงานระหว่าง Damage Step, ชุบชีวิตมอนสเตอร์เมื่อมีการทำลาย, และรีเซ็ตเวท/กับดักจากสุสานใน End Phase
  - **Super Polymerization & Board Breakers**: ดูดมอนสเตอร์ฝ่ายตรงข้ามทำ Fusion Summon (Garura, Mudragon, Dragostapelia, Chimera, Creation King) โดยคู่แข่งไม่สามารถเชนตอบโต้ได้

---

## 2. Card Audit & Database Verification (cards.cdb 100% Match)

| Card Name | ID (Official CDB) | Type / Level | OPT / HOPT | Role in Deck |
|---|---|---|---|---|
| **Mementotlan Angwitch** | `54550967` | Monster / L3 (Spellcaster) | HOPT | **Core Starter #1**: ค้นหา Memento จากเด็ค; ทำลายเพื่อชุบ L<=2 จากสุสาน |
| **Mementotlan Dark Blade** | `18165869` | Monster / L4 (Warrior) | HOPT | **Core Starter #2**: ทำลายตัวเองเพื่อ SS Angwitch L3 จากเด็ค |
| **Mementotlan Tatsunootoshigo** | `81677154` | Monster / L5 (Beast) | HOPT | **Extender / Starter #3**: SS ฟรีจากมือ; ระเบิดตัวเองดัมพ์ Shleepy + Ghattic รวมเลเวล <= 5 |
| **Mementotlan Shleepy** | `50042011` | Monster / L3 (Beast) | HOPT | **Trigger Extender**: โดดจากมือเมื่อมีการระเบิดการ์ด; Fusion ทันที; ดัมพ์การ์ดเมื่อถูกระเบิด |
| **Mementotlan Ghattic** | `52918032` | Monster / L2 (Fiend) | HOPT | **Recursion Engine**: ชุบตัวเองฟรีเมื่อถูกดัมพ์ด้วยมอนสเตอร์ Memento; ดึงการ์ดคืนจากสุสาน |
| **Mementotlan Goblin** | `17943271` | Monster / L1 (Fiend) | HOPT | **Protection / Dumper**: ทิ้งมอบ Target Immunity; ระเบิดเพื่อดัมพ์การ์ด Memento 2 ใบ |
| **Mementotlan Mace** | `81945676` | Monster / L1 (Spellcaster) | HOPT | **Disruption / Searcher**: Quick ทิ้งเพื่อขโมยมอนสเตอร์คู่แข่ง; ระเบิดเพื่อเสิร์ช Memento |
| **Mementotlan Akihiron** | `54207171` | Monster / L5 (Aqua) | HOPT | **Graveyard Floater**: ชุบตัวเองเมื่อ Memento ถูกระเบิด; ดึงการ์ดที่ถูกแบนิช/สุสาน |
| **Mementotlan-Horned Dragon** | `55272555` | Monster / L8 (Dragon) | HOPT | **Beatstick / Removal**: SS จากมือเมื่อมี 3 Memento ในสุสาน; ระเบิด 3 ใบบนสนาม |
| **Mementoal Tecuhtlica - Combined Creation** | `23288411` | Monster / L11 (Illusion) | Soft OPT | **Ace Boss (5000/5000)**: สับ 5 ชื่อกลับเด็คเพื่อลง; ตีทุกตัว; ชุบ Memento เมื่อคู่แข่งเปิดเอฟเฟกต์ |
| **Mementomictlan** | `43338320` | Spell / Field | HOPT | **Field Anchor**: ล็อกเวทกับดักคู่แข่งตอนแบทเทิล; ชุบตัวเลเวลต่ำเมื่อมอนสเตอร์ตาย; เซ็ตการ์ดกลับใน End Phase |
| **Mementotlan Fusion** | `66518509` | Spell / Quick-Play | HOPT | **Fusion Enabler**: ฟิวชั่นจากมือ/สนาม/สุสาน (ถ้ามีตัวตายในเทิร์น); แบนิชในสุสานเสิร์ชการ์ด |
| **Mementotlan Bone Party** | `80722024` | Spell / Quick-Play | HOPT | **Starter / Extender**: ทำลาย 1 ตัว เสิร์ชหรือสเปเชียลจากเด็ค; แบนิชในสุสานให้ Piercing Damage |
| **Goblin Biker Grand Breakout** | `29111045` | Spell / Quick-Play | HOPT | **Starter Support**: สังเวยมอนสเตอร์ 1 ตัว สเปเชียล Mementotlan Goblin จากเด็ค |
| **One for One** | `2295441` | Spell / Normal (Limit 1) | None | **Starter Support**: สเปเชียล Goblin หรือ Mace L1 จากเด็ค |
| **Super Polymerization** | `48130397` | Spell / Quick-Play | None | **Board Breaker**: ฟิวชั่นโดยใช้มอนสเตอร์คู่แข่ง ไม่สามารถเชนตอบโต้ได้ |
| **Forbidden Droplet** | `24299458` | Spell / Quick-Play | None | **Board Breaker**: ส่งการ์ดลงสุสานเพื่อเนเกตมอนสเตอร์คู่แข่งและลดพลังโจมตีครึ่งหนึ่ง |
| **Santa Claws** | `46565218` | Monster / L6 (Fiend) | None | **Removal Out**: สังเวยมอนสเตอร์คู่แข่งลงไปป้องกันบนสนามคู่แข่ง (แก้บอสดื้อยา/กันเป้าหมาย) |
| **Pot of Sloth** | `98476659` | Spell / Normal | HOPT | **Card Advantage**: จั่วการ์ดตามจำนวนการ์ดบนสนามคู่แข่ง |
| **Pot of Prosperity** | `84211599` | Spell / Normal | HOPT | **Consistency**: ขุดหา Starter จากเด็คโดยแบนิช Extra Deck ที่ไม่จำเป็น |
| **Triple Tactics Talent** | `25311006` | Spell / Normal | HOPT | **Going Second Punisher**: จั่ว 2 / ขโมยมอนสเตอร์ / ดูมือฝ่ายตรงข้าม |
| **Evenly Matched** | `15693423` | Trap / Normal | None | **Turn 2 Board Wipe**: บังคับคู่แข่งแบนิชการ์ดคว่ำหน้าจนเหลือเท่าเรา |
| **Mulcharmy Fuwalos** | `42141493` | Monster / L4 | HOPT | **Handtrap**: จั่วการ์ดเมื่อคู่แข่ง Special Summon จากเด็คหรือ Extra Deck |
| **Mulcharmy Purulia** | `84192580` | Monster / L4 | HOPT | **Handtrap**: จั่วการ์ดเมื่อคู่แข่ง Normal หรือ Special Summon จากบนมือ |
| **Droll & Lock Bird** | `94145022` / `94145021` | Monster / L1 | None | **Handtrap**: ล็อกไม่ให้คู่แข่งนำการ์ดจากเด็คขึ้นมือตลอดทั้งเทิร์น |
| **Called by the Grave** | `24224831` / `24224830` | Spell / Quick-Play | None | **Handtrap Counter**: แบนิชและเนเกต Handtrap หรือการ์ดสำคัญในสุสานคู่แข่ง |
| **Mementomictlan Tecuhtlica - Creation King**| `14529511` | Extra / Fusion L9 | HOPT | **Fusion Boss (3000 ATK)**: ดัมพ์ Memento 3 ใบ; Quick ระเบิดสนามคู่แข่ง; แบนิชสุสานค้นหา Field |
| **Mementotlan Twin Dragon** | `19181420` | Extra / Fusion L7 | HOPT | **Mid-Combo Bridge**: ระเบิดเพื่อค้นหา Memento 2 ใบ; ชุบ L<=6 เมื่อถูกระเบิด |
| **Garura, Wings of Resonant Life** | `11765832` | Extra / Fusion L6 | HOPT | **Super Poly Target**: มอนสเตอร์เผ่าเดียวกัน ธาตุเดียวกัน แต่ชื่อต่างกัน; จั่ว 1 เมื่อลงสุสาน |
| **Mudragon of the Swamp** | `54757758` | Extra / Fusion L4 | None | **Super Poly Target**: มอนสเตอร์ธาตุเดียวกันแต่เผ่าต่างกัน |
| **Predaplant Dragostapelia** | `69946549` | Extra / Fusion L8 | Soft OPT | **Super Poly / Fusion Boss**: 1 Fusion + 1 DARK; Quick เนเกตเอฟเฟกต์มอนสเตอร์ |
| **Chimera the King of Phantom Beasts** | `1769875` | Extra / Fusion L6 | HOPT | **Super Poly / Fusion**: 1 Beast + 1 Fiend (Tatsunootoshigo/Shleepy + Goblin/Ghattic) |
| **Berfomet the Mythical King** | `69601012` | Extra / Fusion L6 | HOPT | **Chimera Bridge**: ดัมพ์ Beast/Fiend/Illusion |
| **Favorite HERO Flame Wingman** | `13243125` | Extra / Fusion L6 | None | **Fusion Target**: 2 มอนสเตอร์เผ่าเดียวกันแต่ธาตุต่างกัน |
| **Hyperinvoked Aeon** | `33166263` | Extra / Fusion L10 | HOPT | **Late Game Fusion**: 2+ Fusion Monsters ธาตุต่างกัน |
| **Mixousia the Confounder** | `80843006` | Extra / Fusion L8 | HOPT | **Fusion Utility**: 1 Spellcaster (Angwitch/Mace) + 1 Non-Spellcaster |
| **S:P Little Knight** | `29301451` / `29301450` | Extra / Link 2 | HOPT | **Universal Removal**: แบนิชการ์ดบนสนามหรือสุสาน; Quick แบนิชชั่วคราว |
| **Proxy F Magician** | `12450071` | Extra / Link 2 | None | **Link Fusion**: ใช้วัตถุดิบบนสนามทำ Fusion Summon |
| **Fiendsmith's Sequence** | `49867899` | Extra / Link 2 | None | **Link Utility**: มอนสเตอร์ Fiend/Light |
| **Melomelody the Brass Djinn** | `88942504` | Extra / Xyz Rank 3 | None | **Rank 3 Xyz**: 2 Level 3 (Angwitch + Shleepy); ให้มอนสเตอร์ Xyz ตีได้ 2 ครั้ง |

---

## 3. Deck Architecture & Combo Routings

```
[Starter: Angwitch / Dark Blade / Tatsunootoshigo / Bone Party]
       │
       ▼
[Destruction & Foolish Engine: Tatsunootoshigo pops self -> dumps Shleepy + Ghattic]
       │
       ▼
[Recursion Trigger: Ghattic revives -> retrieves Memento card; Shleepy triggers]
       │
       ▼
[Fusion Bridge: Mementotlan Fusion shuffles GY materials -> Fusion Creation King (L9)]
       │
       ▼
[Mass Dump: Creation King dumps 3 Mementos (Combined Creation + Twin Dragon + Mace)]
       │
       ▼
[Field Search: Creation King banishes from GY -> adds Mementomictlan (Field Spell)]
       │
       ▼
[Ace Summon: Combined Creation (5000 ATK) shuffles 5 Memento names from GY/Hand]
       │
       ▼
[End Board State: 5000 ATK All-Attacker + 3000 ATK Quick Pop + Mace Steal + Goblin Immune]
```

### Route A: Angwitch 1-Card Starter (Primary Line)
1. **Normal Summon Mementotlan Angwitch** (`54550967`)
   - Effect: Search `Mementotlan Tatsunootoshigo` (`81677154`) ขึ้นมือ
2. **Special Summon Tatsunootoshigo** จากบนมือ (เนื่องจากสนามไม่มีมอนสเตอร์ที่ไม่ใช่ Memento)
3. **Tatsunootoshigo Ignition Effect**:
   - เลือกทำลายตัวเอง (Original Level 5)
   - ส่งมอนสเตอร์ Memento จากเด็คลงสุสานโดยผลรวมเลเวลไม่เกิน 5:
     - `Mementotlan Shleepy` (Level 3) + `Mementotlan Ghattic` (Level 2) = 5
4. **Ghattic Trigger Effect**:
   - เมื่อถูกส่งลงสุสานด้วยเอฟเฟกต์มอนสเตอร์ Memento -> Special Summon ตัวเองขึ้นสนามทันที
   - เอฟเฟกต์เมื่อลงสนาม: ดึงการ์ด Memento 1 ใบจากสุสานขึ้นมือ (ดึง `Tatsunootoshigo` หรือ `Mementotlan Fusion`)
5. **Angwitch Ignition Effect**:
   - เล็งเป้าหมาย `Mementotlan Ghattic` ในสุสาน
   - ทำลาย Angwitch บนสนามตนเองเพื่อ Special Summon มอนสเตอร์เลเวล <= 2 จากสุสาน
6. **Mementotlan Fusion / Shleepy Fusion**:
   - รัน Fusion Summon `Mementomictlan Tecuhtlica - Creation King` (`14529511`) โดยสับการ์ด Memento จากสุสานกลับเข้าเด็ค (เนื่องจากมีมอนสเตอร์ถูกทำลายด้วยเอฟเฟกต์ในเทิร์นนี้)
7. **Creation King Trigger on Summon**:
   - ดัมพ์การ์ด Memento 3 ใบจากเด็คหรือ Extra Deck ลงสุสาน:
     - ส่ง `Mementoal Tecuhtlica - Combined Creation` (`23288411`)
     - ส่ง `Mementotlan Twin Dragon` (`19181420`)
     - ส่ง `Mementotlan Mace` (`81945676`)
8. **Special Summon Combined Creation (5,000 ATK)**:
   - นำการ์ดมอนสเตอร์ Memento 5 ใบที่มีชื่อต่างกันจากสุสาน/มือ สับกลับเข้าเด็ค/Extra Deck เพื่ออัญเชิญพิเศษบอส 5,000 ATK ลงสู่สนาม!

### Route B: Dark Blade 1-Card Starter
1. **Normal Summon Mementotlan Dark Blade** (`18165869`)
2. Dark Blade Ignition Effect: ทำลายตัวเองเพื่อ Special Summon `Mementotlan Angwitch` จากเด็ค
3. Angwitch ลงสนาม Search `Tatsunootoshigo` -> เข้าสู่ Route A ทันที!

### Route C: Tatsunootoshigo Solo Starter
1. Special Summon Tatsunootoshigo จากมือ
2. ทำลายตัวเองดัมพ์ `Mementotlan Angwitch` (Level 3) + `Mementotlan Ghattic` (Level 2) = 5
3. Ghattic ชุบตัวเองขึ้นสนาม ดึง `Angwitch` จากสุสานขึ้นมือ
4. Normal Summon Angwitch ค้นหา `Mementotlan Bone Party` หรือ `Mementotlan Fusion` -> เดินเครื่องคอมโบต่อจนจบ!

### Route D: Going Second Board Breaking & OTK
1. **Battle Phase Break**: เปิด `Evenly Matched` จากบนมือเมื่อจบ Battle Phase เพื่อล้างสนามคู่แข่ง
2. **Super Polymerization**: ดูดมอนสเตอร์บอส/Omni-Negate ของคู่แข่งทำ Fusion Summon โดยไม่ให้คู่แข่งเชนตอบโต้
3. **Forbidden Droplet**: จ่ายการ์ดที่ใช้เอฟเฟกต์ไปแล้วเพื่อปิดเอฟเฟกต์และลดพลังโจมตีสนามคู่แข่งครึ่งหนึ่ง
4. **Santa Claws**: สังเวยบอสที่มีภูมิคุ้มกันดื้อยาของคู่แข่งลงไปตั้งรับ
5. **Combined Creation All-Attack Sweep**: บอส 5,000 ATK โจมตีมอนสเตอร์คู่แข่งทุกตัว กวาดล้างสนามจนพลังชีวิตคู่แข่งเหลือ 0!
6. **Bone Party GY Piercing**: แบนิช Bone Party จากสุสานมอบพลังเจาะทะลุพลังป้องกัน (Piercing Damage) ปิดเกมแบบ Instant OTK!

---

## 4. Safety Checks & Stack-Aware Implementation

### 1. Hard Once Per Turn (HOPT) Budget Tracking
- ทุกเอฟเฟกต์หลักของการ์ด Memento (Angwitch, Dark Blade, Tatsunootoshigo, Shleepy, Ghattic, Goblin, Mace, Bone Party, Mementotlan Fusion, Creation King) เป็น **HOPT (ชื่อการ์ดละ 1 ครั้งต่อเทิร์น)**
- ระบบ `MementoExecutor` มีการตรวจสอบสถานะการเปิดใช้ ไม่เปิดใช้งานซ้ำเพื่อป้องกันการเสียการ์ดฟรีโดยไม่ได้ผลลัพธ์

### 2. Protected Boss Rule (ห้ามใช้บอสเป็น Material หรือ Cost สุ่มสี่สุ่มห้า)
- ใน `MementoMaterialEvaluator`:
  - `Mementoal Tecuhtlica - Combined Creation`: กำหนดความสำคัญสูงสุด (-10,000) ห้ามนำไปเป็นเป้าหมายทำลายหรือวัตถุดิบฟิวชั่น
  - `Creation King`: กำหนดความสำคัญ (-8,000) ป้องกันการนำบอสขัดขวางไปทิ้ง
  - การ์ดที่ให้ความสำคัญในการทำลายก่อน: `Shleepy` (+3,000), `Akihiron` (+2,800), `Horned Dragon` (+2,500), `Twin Dragon` (+2,000) เพราะการ์ดเหล่านี้จะ **ได้ผลประโยชน์มหาศาลเมื่อถูกทำลาย**

### 3. Safe Hint Resolution (OnSelectCard)
- **Hint 501 (Discard)**: ลำดับความสำคัญในการทิ้ง: Shleepy -> Ghattic -> Akihiron -> Goblin -> Mace; ห้ามทิ้ง Handtrap หรือ Boss
- **Hint 502/503 (Destroy)**:
  - หากเป็นการเลือกทำลายการ์ดคู่แข่ง: จัดลำดับทำลายตาม `CardIntelligence.GetCardThreatScore` (ทำลาย Floodgate และ Negator ก่อน)
  - หากเป็นการทำลายการ์ดตนเอง: เลือกลำดับมอนสเตอร์ที่ Floats ได้ตาม `MaterialEvaluator.GetDestructionPriority`
- **Hint 507 (Return to Deck)**: คัดแยกการ์ด Memento 5 ชื่อที่ต่างกันในสุสานเพื่อจ่ายคอสอัญเชิญ Combined Creation อย่างแม่นยำ 100%
- **Hint 511 (Fusion Material)**: ใช้วัตถุดิบคู่แข่งก่อนเสมอ (Super Poly) ตามด้วยวัตถุดิบในสุสาน และมอนสเตอร์ตัวเล็ก

---

---

## 5. Deployment Verification & File Manifest

| Target File | Destination | Status |
|---|---|---|
| `2026_Memento.ydk` | `C:\Users\admin\Documents\EdoGame\deck\2026_Memento.ydk` | **Deployed (40 Cards Main, 15 Extra)** |
| `Memento.ydk` | `C:\Users\admin\Documents\EdoGame\deck\Memento.ydk` | **Deployed (40 Cards Main, 15 Extra)** |
| `2026_Memento.ydk` | `C:\Users\admin\Documents\EdoGame\WindBot\Decks\2026_Memento.ydk` | **Deployed (40 Cards Main, 15 Extra)** |
| `Memento.ydk` | `C:\Users\admin\Documents\EdoGame\WindBot\Decks\Memento.ydk` | **Deployed (40 Cards Main, 15 Extra)** |
| `WindBot.dll` | `C:\Users\admin\Documents\EdoGame\WindBot\WindBot.dll` | **Compiled & Deployed (Release net10.0)** |
| `ExecutorBase.dll`| `C:\Users\admin\Documents\EdoGame\WindBot\ExecutorBase.dll`| **Compiled & Deployed (Release net10.0)** |
| `core.dll` | `C:\Users\admin\Documents\EdoGame\WindBot\core.dll` | **Compiled & Deployed (Release net10.0)** |
| `bots.json` | `C:\Users\admin\Documents\EdoGame\WindBot\bots.json` | **Registered "Memento", "2026_Memento"** |
| `DashBot.exe` | `C:\Users\admin\Documents\EdoGame\DashBot.exe` | **Deployed (WPF Launcher)** |

---

## 6. Competitive 40-Card Optimization Profile

### สถิติเชิงคณิตศาสตร์เปรียบเทียบ (Hypergeometric Analysis)
- **Pure 1-Card Starter Opening:** เพิ่มจาก **53.08%** เป็น **74.18%** (+21.10%)
- **Playable Hand Rate (Turn 1):** เพิ่มจาก **78.31%** เป็น **90.00%** (+11.69%)
- **Turn 1 Brick Rate:** ลดลงจาก **21.69%** เหลือเพียง **10.00%** (-11.69%)
- **Starter + Handtrap In Hand:** เพิ่มจาก **36.10%** เป็น **60.02%** (+23.92%)

