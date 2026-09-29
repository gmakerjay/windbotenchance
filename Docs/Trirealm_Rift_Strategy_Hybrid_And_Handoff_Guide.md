# Trirealm Rift (Sky Thunder Trirealm Rift Yomi) Strategy, Hybrid Analysis & Developer Handoff Guide

**เอกสารแนวทางการพัฒนา กลยุทธ์การเล่น การวิเคราะห์การ์ดไฮบริด และคู่มือส่งต่องานฉบับสมบูรณ์ (Comprehensive Handoff Guide)**  
**Project**: YugiohTH / WindBot ModernExecutor  
**Target Environment**: C# .NET 10.0 | WindBot AI Engine  
**Exclusive Deployment Path**: `C:\Users\admin\Documents\EdoGame\`  
**Date**: 2026-09-29  

---

## 1. ข้อมูลพื้นฐานของ Archetype และสารานุกรมการ์ด (Archetype Identity & Card Encyclopedia)

**Trirealm Rift (異界の裂け目 / 鳴神の裂界 / Xenovader△)** คือเด็คเผ่าไซคิก (Psychic) ที่สร้างเอกลักษณ์การเล่นด้วยกลไก **"Face-Down Banish Resource Engine" (คลังการ์ดรีมูฟคว่ำหน้า)** ซึ่งพลิกโฉมการ์ดที่ถูกรีมูฟคว่ำหน้าจากเดิมที่เป็นขยะนอกเกม ให้กลายเป็น "คลังแสง" ในการค้นหา อัญเชิญพิเศษ และเปิดใช้งาน Quick Effect ขัดขวางคู่แข่ง

### สารานุกรมการ์ดในธีม (In-Archetype Card Catalog)

| Card ID | ชื่อการ์ด (ภาษาอังกฤษ / ญี่ปุ่น) | Lv/Type | ธาตุ | ผล On-Summon (Trigger) | เอฟเฟกต์หลัก / Quick Interruption |
|---|---|---|---|---|---|
| `100458031` | **Gehenna** (虚ノ異解△ゲヘナ) | Lv 1 Psy | DARK | รีมูฟคว่ำ 1 ใบบนสุดเด็ค | • บนมือ: โดดฟรีถ้าไม่มีมอนสเตอร์<br>• สนาม: เสิร์ชมอนสเตอร์ 2 ใบชื่อต่างกันจากคลังรีมูฟ (ติด Xenolock) |
| `100458032` | **Sheol** (空ノ異解△シェオル) | Lv 2 Psy | LIGHT | รีมูฟคว่ำ 2 ใบบนสุดเด็ค | • บนมือ: โดดฟรีถ้าคุม Trirealm<br>• สนาม: เสิร์ชเวท/กับดัก 2 ใบชื่อต่างกันจากคลังรีมูฟ (ติด Xenolock) |
| `100458033` | **Tuonela** (蒼ノ異解△トゥオネラ) | Lv 3 Psy | WATER | รีมูฟคว่ำ 3 ใบบนสุดเด็ค | • บนมือ: โดดฟรีถ้ามอนสเตอร์เราน้อยกว่าคู่แข่ง<br>• สนาม: สเปเชียลมอนสเตอร์ Trirealm Lv 5+ จากคลังรีมูฟ |
| `100458034` | **Naraka** (紅ノ異解△ナラカ) | Lv 4 Psy | FIRE | รีมูฟคว่ำ 4 ใบบนสุดเด็ค | • บนมือ: โดดฟรีถ้าคุม Trirealm<br>• เทิร์นคู่แข่ง (Quick): เมื่อคู่แข่ง SS มอนสเตอร์ สามารถสเปเชียล Trirealm Lv 5+ จากคลังรีมูฟ |
| `100458035` | **Yomi** (鳴神の裂界ヨミ) | Lv 5 Psy | WIND | รีมูฟคว่ำ 5 ใบบนสุดเด็ค | • บนมือ: ทิ้งเพื่อเสิร์ช Trirealm 1 ใบ (ยกเว้น Lv 5)<br>• **Quick Negate**: รีมูฟมอนสเตอร์ธาตุเดียวกันหงายหน้า Negate & ทำลายมอนสเตอร์คู่แข่ง |
| `100458036` | **Helheim** (葬頂の裂界ヘルヘイム) | Lv 6 Psy | EARTH | รีมูฟคว่ำ 6 ใบบนสุดเด็ค | • บนมือ (Quick): โดดลงสนามโดยสับ Trirealm รีมูฟ 1 ใบเข้าเด็ค/สุสาน<br>• สนาม: คุ้มกันการ์ด Trirealm อื่นทั้งหมดจากการตกเป็นเป้าหมายและการถูกทำลาย |
| `100458037` | **Ploutonion** (狂嵐の裂界プルートニオン) | Lv 7 Psy | WATER | รีมูฟคว่ำ 7 ใบบนสุดเด็ค | • บนมือ: ทิ้งตัวเอง + Trirealm 1 ใบเพื่อจั่ว 2 ใบ<br>• **Quick Spin**: เล็งมอนสเตอร์ศัตรูที่มีธาตุตรงกับตัวรีมูฟคว่ำ สับกลับเข้าเด็ค |
| `100458038` | **Darkness** (異界の裂け目 ダークネス) | Lv 8 Psy | DARK | รีมูฟคว่ำ 8 ใบบนสุดเด็ค | • สนาม (Quick): ถ้ารีมูฟคว่ำ $\ge 20$ ใบ สั่งทำลายการ์ด 1 ใบในสนามฟรี<br>• สนาม: ถ้ารีมูฟคว่ำ $\ge 30$ ใบ คู่แข่งข้าม Draw Phase ถาวร |
| `100458039` | **Territory - Valvols** (異解△領域－ヴァルヴォルス) | Field Spell | - | Cost: รีมูฟ 5 ใบคว่ำ | • เสิร์ชการ์ด Trirealm 1 ใบจากคลังรีมูฟขึ้นมือ<br>• **Hard Lock**: ถ้าคุม Trirealm Lv 5+ และเด็คเหลือ 0 ใบ คู่แข่งห้ามเปิดใช้การ์ด/เอฟเฟกต์ในเทิร์นเรา |
| `100458040` | **Trirealm Rift Gospel** (異解△福音) | Cont. Spell | - | Cost: รีมูฟ 5 ใบคว่ำ | • เพิ่มสิทธิ์ Normal Summon มอนสเตอร์ Trirealm 1 ครั้ง<br>• Morph: สังเวยรีมูฟคว่ำมอนสเตอร์ Lv $\le 4$ บนสนาม สเปเชียล Lv 5+ จากคลังรีมูฟ |
| `100458041` | **Trirealm Rift Judgment** (異解△審判) | Counter Trap | - | Cost: รีมูฟ Trirealm หงายหน้า 1 ใบ | • เมื่อคู่แข่งอัญเชิญ หรือใช้ผลใน Damage Step: ปฏิเสธการอัญเชิญ/เอฟเฟกต์ + ปรับ ATK มอนสเตอร์คู่แข่งทั้งสนามเป็น 0 + เสิร์ช/สเปเชียลการ์ดรีมูฟคว่ำ 1 ใบ |

---

## 2. บทสรุปปัญหาและการ Refactor ระบบ (Problem & Refactoring Summary)

จากการทดสอบเชิงลึกในระยะเริ่มต้น เด็ค Trirealm Rift ประสบปัญหาแพ้รวด 10 เกม (0/10) ต่อกรกับเด็คคลาสสิกอย่าง `DarkMagician` สาเหตุรากเหง้า 4 ประการได้รับการแก้ไขและขัดเกลาจนเสร็จสมบูรณ์:

```
[ปัญหาเดิม]                                                [แนวทางแก้ไขเชิงสถาปัตยกรรม]
1. Flag Search ซ้อนกับ Summon Trigger  ─────────────►  แยก Flag อิสระ + ปรับ PickSearchTargets ให้เสิร์ชได้ 2 ใบ
2. ตัวใหญ่ Lv 5-8 ไม่ยอมใช้ Trigger Banish ─────────►  ลงทะเบียน Phase 3: OnSummonBanishTrigger มิลล์การ์ดอัตโนมัติ
3. Gospel ถูก Set คว่ำแทนที่จะเปิดใช้งาน ──────────►  แก้ไข Priority ให้เปิดใช้งานหงายหน้าทันที
4. คลังกระสุนรีมูฟคว่ำขาดแคลนในเทิร์นแรก ────────►  เสริม Gizmek Orochi (แบน 8 คว่ำ) + Desires 3 ใบ + Terraforming
```

1. **Bug Flag ซ้อนทับบล็อกเอฟเฟกต์ Search +2 (State Collision Fixed)**:
   - `Gehenna` (Lv 1) มี Mandatory Summon Trigger รีมูฟ 1 ใบ และ Ignition Search มอนสเตอร์ 2 ใบ
   - `Sheol` (Lv 2) มี Mandatory Summon Trigger รีมูฟ 2 ใบ และ Ignition Search เวท/กับดัก 2 ใบ
   - *เดิม*: ระบบใช้ตัวแปรตรวจจับตัวเดียวกัน เมื่อมอนสเตอร์ลงสนาม Trigger ทำงานเสร็จ ตัวแปรถูกล็อกเป็น `true` บล็อกไม่ให้บอทเปิดใช้ Search +2 ใน Main Phase เลยตลอดทั้งเกม
   - *แก้ไข*: แยก State Flags ชัดเจน (`_gehennaSearchedThisTurn`, `_sheolSearchedThisTurn`) และปรับจูน `PickSearchTargets` ให้สามารถเลือกการ์ดที่มีชื่อไม่ซ้ำกันได้ครบ 2 ใบเต็มโควต้า
2. **Summon Trigger ของมอนสเตอร์ตัวใหญ่ (Lv 5–8) ไม่ทำงาน**:
   - `Yomi` (Lv 5), `Helheim` (Lv 6), `Ploutonion` (Lv 7), `Darkness` (Lv 8) ขาดการลงทะเบียน Executor
   - *แก้ไข*: เพิ่ม **Phase 3: `OnSummonBanishTrigger`** บังคับให้เปิดใช้ Trigger มิลล์การ์ดคว่ำหน้าลงคลังทันทีที่ลงสู่สนามทุกช่องทาง
3. **การ์ด Gospel ถูก Set คว่ำแทนที่จะเปิดใช้**:
   - `Trirealm Rift Gospel` (100458040) เป็น Continuous Spell เดิมลงทะเบียนในคำสั่ง `SpellSet`
   - *แก้ไข*: สั่งเปิดใช้งานหงายหน้าจากมือทันที (`OnGospelHandActivate`) โดยจ่าย Cost รีมูฟ 5 ใบคว่ำหน้า ช่วยเพิ่ม Normal Summon และเปลี่ยนมอนสเตอร์ตัวเล็กเป็นบอส
4. **เร่งคลัง Face-Down Banish ให้พร้อมรบในเทิร์นแรก**:
   - บรรจุ `Gizmek Orochi, the Serpentron Sky Slasher` (71197066) จำนวน 2 ใบ (Quick Effect รีมูฟคว่ำ 8 ใบลอยตัวฟรี)
   - ปรับ `Pot of Desires` (35261759) เป็น 3 ใบเต็ม (รีมูฟ 10 ใบ จั่ว 2)
   - บรรจุ `Terraforming` (73628505) เสิร์ชฟิลด์ `Valvols` การันตีการเปิดคลังรีมูฟ 5 ใบ

---

## 3. การวิเคราะห์การเล่นร่วมกับการ์ดอื่นเชิงลึก (Comprehensive Hybrid Synergy Analysis)

### A. 👹 Gren Maju Da Eiza (36584821) ➔ **เข้ากันได้ดีที่สุด (S-Tier Recommended ⭐⭐⭐⭐⭐)**

* **บทวิเคราะห์**: เป็น **OTK Finisher ระดับจักรพรรดิ** ที่ปิดจุดอ่อนของเด็คที่ขาดมอนสเตอร์ปิดเกมอย่างเด็ดขาด
* **การตัดสินตามกฎสากล (Official Rulings)**:
  1. **นับการ์ดรีมูฟคว่ำหน้าทุกใบ**: ข้อความการ์ดระบุว่า *"ATK/DEF become the number of your banished cards x 400"* ตามกฎ OCG/TCG การ์ดที่ถูกรีมูฟแบบคว่ำหน้า (Face-down Banish) ถือเป็น "banished cards" อย่างสมบูรณ์
  2. **ไม่โดน Xenolock บล็อกเด็ดขาด**: ข้อความ Xenolock ของ Gehenna และ Sheol ระบุชัดเจน: *"cannot **activate** cards or effects, except 'Trirealm Rift'"* แต่การเพิ่มพลังของ Gren Maju เป็น **Continuous Effect (ผลต่อเนื่อง ไม่มีการ Activate / ไม่เกิด Chain Link)** จึงมีพลังเต็มเปี่ยมเสมอ
  3. **Lethal Damage Scaling**:
     - รีมูฟ 10 ใบ (เช่น จาก Pot of Desires 1 ครั้ง) = ATK 4,000
     - รีมูฟ 20 ใบ (คอมโบปกติเทิร์น 1-2) = **ATK 8,000 (OTK ดาเมจเลือดเต็มทันที)**
     - รีมูฟ 30 ใบ (คอมโบเต็มรูปแบบ) = **ATK 12,000**
  4. **การบริหารสิทธิ์ Normal Summon ร่วมกับ Gospel**:
     - `Gospel` มอบสิทธิ์ Normal Summon เพิ่มเติมสำหรับมอนสเตอร์ Trirealm 1 ครั้ง
     - ดังนั้น ผู้เล่นสามารถใช้ **สิทธิ์ Normal Summon ปกติ** ในการลง `Gren Maju Da Eiza` และใช้ **สิทธิ์เสริมของ Gospel** สำหรับตัวเดินเกมในธีมได้อย่างสมบูรณ์แบบ

### B. 🐲 Eater of Millions (63845230) ➔ **เข้ากันได้ยอดเยี่ยม (A-Tier Recommended ⭐⭐⭐⭐)**

* **บทวิเคราะห์**: มอนสเตอร์ทางเลือกที่ช่วยกำจัดบอสคู่แข่งแบบไม่เล็งเป้า พร้อมเติมคลัง Banish คว่ำหน้า
* **จุดเด่นตามกฎการ์ด**:
  1. **กระสุนฟรีจาก Extra Deck**: สามารถอัญเชิญพิเศษจากบนมือได้โดยรีมูฟการ์ดจากมือ, สนาม หรือ **Extra Deck แบบคว่ำหน้า 5 ใบขึ้นไป** (สามารถรีมูฟ Extra Deck ทิ้งรวดเดียว 15 ใบ เพื่อเติมคลังรีมูฟคว่ำหน้าให้ Trirealm ได้ในพริบตา!)
  2. **Non-Target Face-Down Banish**: เมื่อต่อสู้กับมอนสเตอร์คู่แข่ง รีมูฟมอนสเตอร์ตัวนั้นแบบคว่ำหน้าทันทีโดยไม่ต้องคำนวณดาเมจ
  3. **ข้อควรระวัง**: ต้องอัญเชิญก่อนที่จะกดใช้ Search ของ Gehenna หรือ Sheol เนื่องจากเอฟเฟกต์รีมูฟมอนสเตอร์คู่แข่งของ Eater of Millions เป็น Trigger Effect ที่ต้อง **Activate** (ซึ่งจะติด Xenolock ถ้าทำทีหลัง)

### C. 💀 Necroface (28297833) ➔ **ใช้เป็น Safety Net เฉพาะทางเท่านั้น (Niche ⭐⭐)**

* **ข้อจำกัดทางกฎการ์ดที่ร้ายแรง**:
  1. **Face-Down Mill Trigger ไม่ทำงาน**: เมื่อ Necroface ถูกรีมูฟแบบคว่ำหน้า เอฟเฟกต์ *"If this card is banished: Each player banishes 5 cards..."* จะ **ไม่ทำงาน** เพราะการ์ดคว่ำหน้าไม่มีชื่อหรือประเภทระบุตัวตนในขณะรีมูฟ
  2. **Anti-Synergy กับทรัพยากรของเด็ค**: การ Normal Summon Necroface จะบังคับสับการ์ดที่ถูกรีมูฟทั้งหมดกลับเข้าเด็ค ซึ่งจะเป็นการ **ทำลายคลังรีมูฟของ Trirealm Rift จนหมดสิ้น** ทำให้บอสทุกตัวหมดพลังในการขัดขวาง
* **ประโยชน์เชิงกลยุทธ์เพียงหนึ่งเดียว**:
  - ใช้เป็น **Emergency Deck-Out Reset** ในช่วง Late Game เมื่อเด็คเหลือ $\le 2$ ใบ เพื่อป้องกันการแพ้จากการ์ดหมดเด็ค

### D. 🌌 Dimension Shifter (80344569) ➔ **เข้ากันได้ปานกลางค่อนข้างระวัง (Conditional ⭐⭐⭐)**

* **บทวิเคราะห์**:
  1. หากเปิดใช้ในเทิร์น 1 ฝ่ายตรงข้ามจะถูกตัดวงจรสุสานอย่างสิ้นเชิง
  2. สำหรับ Trirealm Rift: การ์ดส่วนใหญ่ถูกมิลล์จากเด็คออกนอกเกมโดยตรงอยู่แล้ว แต่ Shifter จะทำให้การ์ดที่ตกสุสานกลายเป็นการ์ดรีมูฟแบบ **Face-Up (หงายหน้า)** ซึ่งไม่สามารถนำมาใช้เป็นคลังกระสุนของเอฟเฟกต์ที่ต้องการ Face-Down ได้ (ยกเว้นการจ่าย Cost ของ Judgment)
  3. จึงเหมาะสำหรับใช้เป็น Side Deck รับมือเด็คสุสานมากกว่าการใส่ใน Main Deck

### E. 👻 Dark Necrofear (31829185) / Curse Necrofear (14509651) ➔ **เล่นไม่ได้โดยเด็ดขาด (Incompatible ❌)**

* มอนสเตอร์ Trirealm Rift ทั้งหมดเป็น **เผ่าไซคิก (Psychic 0x100000)** ไม่ใช่เผ่าปีศาจ (Fiend) จึงไม่สามารถนำมาใช้เป็นเงื่อนไขเรียก Necrofear ได้ และสุสานของเด็คนี้มักจะว่างเปล่า

---

## 4. แผนการเล่นและผังกลยุทธ์ (Strategic Playbook & Routing Flowchart)

```mermaid
graph TD
    StartHand["มือเริ่มต้น (Starting Hand)"] --> CheckStarter{"มี Starter ตัวไหน?"}
    
    CheckStarter -->|Gehenna / Sheol| RouteA["Route A: Core In-Archetype Line"]
    CheckStarter -->|Valvols / Terraforming| RouteB["Route B: Field Spell Setup"]
    CheckStarter -->|Gospel + Any Fodder| RouteC["Route C: Gospel Morph Line"]
    CheckStarter -->|Gizmek / Desires| RouteD["Route D: Fast Banish Surge"]

    RouteA --> BanishFeed["มิลล์การ์ดคว่ำหน้าลงคลัง (Feed Banish Pool)"]
    RouteB --> BanishFeed
    RouteC --> BanishFeed
    RouteD --> BanishFeed

    BanishFeed --> MidTurn{"ประเมินสภาวะบอร์ด"}
    MidTurn -->|First Turn (Turn 1)| EndBoard["ตั้ง End Board ป้องกัน<br>• Yomi (Quick Negate)<br>• Helheim (Untargetable/Indestructible)<br>• Gospel + Valvols<br>• Trirealm Judgment (Counter Trap)"]
    MidTurn -->|Second Turn (Turn 2)| OTKPush["Board Breaking & OTK Line<br>• Ploutonion Quick Spin เด้งบอสศัตรู<br>• Gizmek Orochi ทำลายตัวปัญหา<br>• Normal Summon Gren Maju Da Eiza (ATK 8000+)"]
```

### 4.1 First-Turn End Board Setup (การสร้างป้อมปราการเทิร์นแรก)

เป้าหมายสูงสุดของเทิร์นแรกคือการสร้างบอร์ดที่มีการขัดขวางครบทุกมิติและไม่สามารถถูกเจาะได้ง่าย:
1. **Sky Thunder Trirealm Rift Yomi (WIND Lv 5)**: ยืนสนามในสภาพตั้งรับ (DEF 2,500) พร้อม Quick Negate มอนสเตอร์คู่แข่งทุกธาตุที่มีในคลังรีมูฟคว่ำหน้า
2. **Burial Summit Trirealm Rift Helheim (EARTH Lv 6)**: ยืนสนามมอบบัฟ **Untargetable & Indestructible** ให้กับการ์ด Trirealm ทุกใบ
3. **Trirealm Rift Judgment (Counter Trap)**: หมอบอยู่บนสนาม คอยดักสกัดการอัญเชิญของคู่แข่ง พร้อมปรับ ATK ทั้งสนามของศัตรูเป็น 0
4. **Trirealm Rift Territory - Valvols**: ฟิลด์สเปลล์ที่เตรียมเปิดโหมด **Hard Lockout** เมื่อเด็คเหลือ 0 ใบ

### 4.2 Second-Turn OTK Burst Math (สูตรการคำนวณดาเมจปิดเกม)

เมื่อเล่นเป็นฝ่ายเริ่มหลัง (Going Second):
* **ขั้นตอนที่ 1 (Clearing Threats)**:
  - ใช้ `Ploutonion` (Quick Spin) เด้งมอนสเตอร์หลักของศัตรูกลับเด็ค
  - ใช้ `Gizmek Orochi` ยิงทำลายมอนสเตอร์ตัวขัดขวาง
* **ขั้นตอนที่ 2 (Count Banish Pool)**:
  - Banish จาก `Gizmek Orochi` = 8 ใบ
  - Banish จาก `Pot of Desires` = 10 ใบ
  - Banish จาก `Gospel` หรือ `Valvols` = 5 ใบ
  - Banish จากการ Normal/Special Summon ในเทิร์น = 5–10 ใบ
  - **รวมการ์ดรีมูฟทั้งหมด = 28–33 ใบ**
* **ขั้นตอนที่ 3 (Summon Finisher)**:
  - Normal Summon `Gren Maju Da Eiza` ➔ $28 \times 400 = \mathbf{11,200\text{ ATK}}$
  - โจมตีมอนสเตอร์หรือตีตรง ปิดเกมในเทิร์นเดียว (1-Hit OTK)

---

## 5. สถาปัตยกรรมโค้ด WindBot ModernExecutor & Decoupled Domain Plugin

โครงสร้างโค้ดถูกออกแบบตามหลักการ **Central Core & Decoupled Domain Plugin (Layer 3 Architecture)** เพื่อแยกตรรกะเชิงกลยุทธ์ออกจากระบบตัดสินใจกลาง:

```
┌─────────────────────────────────────────────────────────────┐
│                    CENTRAL CORE PLATFORM                    │
│   CardIntelligence │ BoardScorer │ FallbackSelectCard       │
│   HeuristicGuard   │ ChainAdvisor│ ResourcePlan             │
└──────────────────────────────┬──────────────────────────────┘
                               │ inherits & coordinates
┌──────────────────────────────▼──────────────────────────────┐
│            TrirealmRiftExecutor (ModernExecutor Router)     │
│   9-Phase Execution Pipeline, Priority Management,          │
│   Callback Handlers (OnSelectPosition, OnSelectCard)        │
└──────────────────────────────┬──────────────────────────────┘
                               │ delegates domain decisions
┌──────────────────────────────▼──────────────────────────────┐
│           TrirealmRiftPlugin (DeckPluginBase)               │
│   ├─ TrirealmRiftStrategy (Multi-Search & Priority Routing) │
│   ├─ TrirealmRiftMaterialEvaluator (Boss Guard & Discard)   │
│   └─ TrirealmRiftThreatEvaluator (Emergency Threat Scoring) │
└─────────────────────────────────────────────────────────────┘
```

### 5.1 ผังการทำงาน 9-Phase Pipeline ใน `TrirealmRiftExecutor.cs`

| Phase | หน้าที่และความรับผิดชอบ | ตัวอย่างการ์ดและการกระทำ |
|---|---|---|
| **Phase 1** | Quick Counters & Handtraps | `TrirealmRiftJudgment`, `Yomi` Negate, `Called by the Grave`, `Ash Blossom`, `Impermanence` |
| **Phase 2** | Opponent Turn Disruptions | `Ploutonion` Quick Spin, `Darkness` Quick Pop, `Naraka` SS, `Gizmek Orochi` Quick Pop |
| **Phase 3** | On-Summon Trigger Banishment | บังคับเปิดใช้งาน Trigger มิลล์การ์ดคว่ำหน้าของมอนสเตอร์ Trirealm ทุกตัวเมื่อลงสนาม |
| **Phase 4** | Banish Accelerators & Draw Engine | `Pot of Desires`, `Terraforming`, `Gizmek Orochi` SS, `Valvols` Hand, `Gospel` Hand, `Ploutonion` Draw 2 |
| **Phase 5** | Main Starters, Extenders & Searches | `Gehenna` SS + Search, `Sheol` SS + Search, `Tuonela` SS, `Gospel` Morph, `Helheim` SS |
| **Phase 6** | Normal Summons | ลงมอนสเตอร์ปกติกรณีบนมือไม่มีตัวโดดฟรี (`Naraka` > `Sheol` > `Gehenna`) |
| **Phase 7** | Extra Deck Toolbox (Non-Locked) | เรียก `Accesscode Talker`, `Knightmare Unicorn`, `S:P Little Knight`, `Bagooska`, `Abyss Dweller` |
| **Phase 8** | Spell & Trap Backrow Placement | จัดสรรการเซ็ตการ์ดลงหลังกระดานก่อนจบเทิร์น (`Judgment`, `Impermanence`, `Called by the Grave`) |
| **Phase 9** | Monster Repositioning | ปรับเปลี่ยนตำแหน่งต่อสู้ตามกฎเกณฑ์ ATK/DEF (เช่น Yomi อยู่ในโหมดตั้งรับเสมอ) |

### 5.2 การทำงานของ Domain Sub-helpers ใน `TrirealmRiftPlugin.cs`

1. **`TrirealmRiftStrategy`**:
   - **Multi-Search Logic**: รองรับการเลือกการ์ดที่มีชื่อต่างกันสูงสุด 2 ใบสำหรับ `Gehenna` (มอนสเตอร์) และ `Sheol` (เวท/กับดัก)
   - **Dynamic Turn Priority**:
     - *ในเทิร์นคู่แข่ง*: จัดลำดับความสำคัญในการเรียกมอนสเตอร์มาขัดขวาง: `Yomi` (Quick Negate) > `Helheim` (Protection) > `Ploutonion` (Spin)
     - *ในเทิร์นบอท*: จัดลำดับความสำคัญในการตั้งบอร์ดและทำดาเมจ: `Helheim` > `Darkness` (3000 ATK) > `Yomi` > `Ploutonion`
2. **`TrirealmRiftMaterialEvaluator`**:
   - **Ace Boss Protection (1000 pts)**: คุ้มครอง `Yomi`, `Helheim`, `Darkness`, `Ploutonion` ไม่ให้นำไปสังเวยหรือเป็นวัตถุดิบ Extra Deck
   - **Optimal Discard Target**: จัดลำดับทิ้ง `Gizmek Orochi` เป็นอันดับ 1 เนื่องจากสามารถชุบตัวเองออกจากสุสานได้ฟรี
3. **`TrirealmRiftThreatEvaluator`**:
   - เชื่อมต่อกับ `CardIntelligence` เพื่อตรวจจับ Floodgate และ Chokepoints พร้อมตัดตอนการ์ดอันตรายของคู่แข่ง

---

## 6. โครงสร้าง Decklist ล่าสุด (`TrirealmRift.ydk`)

**Main Deck (40 ใบ)**:
* **Monsters (21 ใบ)**:
  - 3x `Trirealm Rift of Emptiness Gehenna` (100458031) - Starter ฟรี SS / เสิร์ช 2 มอนสเตอร์
  - 3x `Trirealm Rift of Sky Sheol` (100458032) - Extender / เสิร์ช 2 เวทกับดัก
  - 1x `Trirealm Rift of Blue Tuonela` (100458033) - Extender โดด Lv 5+
  - 1x `Trirealm Rift of Scarlet Naraka` (100458034) - Disruption เทิร์นคู่แข่ง
  - 3x `Sky Thunder Trirealm Rift Yomi` (100458035) - Core Boss Quick Negate / Searcher
  - 2x `Burial Summit Trirealm Rift Helheim` (100458036) - Boss คุ้มกันบอร์ด / ชุบตัวใหญ่
  - 2x `Mad Tempest Trirealm Rift Ploutonion` (100458037) - Draw 2 / Quick Spin
  - 2x `Gren Maju Da Eiza` (36584821) - **OTK Finisher (ATK = Banish x 400)** ⭐
  - 2x `Sauravis, the Ancient and Ascended` (4810828) - **Handtrap ป้องกันการเล็งเป้า Gren Maju & บอส** ⭐
  - 2x `Gizmek Orochi, the Serpentron Sky Slasher` (71197066) - Quick Banish 8 ใบคว่ำหน้า
* **Spells (13 ใบ)**:
  - 3x `Trirealm Rift Territory - Valvols` (100458039) - Field Spell ล็อคเทิร์น
  - 1x `Terraforming` (73628505) - เสิร์ชฟิลด์
  - 3x `Trirealm Rift Gospel` (100458040) - Continuous Spell เพิ่ม Normal Summon & Morph
  - 3x `Pot of Desires` (35261759) - Banish 10 คว่ำหน้า จั่ว 2
  - 2x `Called by the Grave` (24224830) - ขัดขวาง Handtrap
  - 1x `Crossout Designator` (65681983) - **สกัด Imperm / Veiler / Handtrap คุ้มกัน Gren Maju** ⭐
* **Traps & Handtraps (6 ใบ)**:
  - 2x `Trirealm Rift Judgment` (100458041) - Counter Trap สกัดกั้น / ATK 0
  - 2x `Infinite Impermanence` (10045474) - Handtrap Negate
  - 2x `Ash Blossom & Joyous Spring` (14558127) - Handtrap ตัดตอน Search

**Extra Deck Toolbox (15 ใบ)**:
- 1x `S:P Little Knight` (29301450) - Link-2 ขจัดแบบ Banish 2 จังหวะ
- 1x `I:P Masquerena` (65741786) - Link-2 สเปเชียลเทิร์นคู่แข่ง
- 1x `Knightmare Phoenix` (2857636) - Link-2 ยิงเวทกับดัก
- 1x `Knightmare Unicorn` (38342335) - Link-3 Spin การ์ดขึ้นเด็ค
- 1x `Accesscode Talker` (86066372) - Link-4 5300 ATK Board Breaker
- 1x `Number 41: Bagooska the Terribly Tired Tapir` (90590303) - Rank-4 Floodgate บังคับตั้งรับ
- 1x `Abyss Dweller` (21044178) - Rank-4 ปิดผนึกเอฟเฟกต์ในสุสาน
- 1x `Divine Arsenal AA-ZEUS - Sky Thunder` (90448279) - Rank-12 Board Wiper
- 1x `Super Starslayer TY-PHON - Sky Crisis` (93039339) - Anti-High ATK Floodgate
- 1x `Linkuriboh` (41999284) - Link-1 ป้องกันดาเมจจาก Gehenna
- 1x `Relinquished Anima` (94259633) - Link-1 ดูดมอนสเตอร์ตรงข้าม
- 1x `Dharc the Dark Charmer, Gloomy` (8264361) - Link-2 ขโมยมอนสเตอร์มืด
- 1x `Lyna the Light Charmer, Shining` (9839945) - Link-2 ขโมยมอนสเตอร์แสง
- 1x `Hiita the Fire Charmer, Ablaze` (48815792) - Link-2 ขโมยมอนสเตอร์ไฟ
- 1x `Wynn the Wind Charmer, Verdant` (30674956) - Link-2 ขโมยมอนสเตอร์ลม

---

## 7. คู่มือส่งต่องานสำหรับนักพัฒนา (Developer Handoff & Maintenance Guide)

### 7.1 ตำแหน่ง Source Code สำคัญ (Source Code Files)
* `src\YGO_SOURCE_CLEAN\windbot-fork\Game\AI\Decks\TrirealmRiftExecutor.cs`: ตัวควบคุมวงจร 9-Phase และ Callback Handlers
* `src\YGO_SOURCE_CLEAN\windbot-fork\Game\AI\Plugins\TrirealmRiftPlugin.cs`: โดเมนปลั๊กอิน (Strategy, Material Evaluator, Threat Evaluator)
* `src\YGO_SOURCE_CLEAN\windbot-fork\Decks\TrirealmRift.ydk`: Decklist ต้นฉบับ
* `src\YGO_SOURCE_CLEAN\windbot-fork\bots.json`: ข้อมูลการลงทะเบียนบอทในระบบ

### 7.2 กฎเหล็กที่ต้องตรวจสอบเมื่อทำการแก้ไข (Developer Checklist)
1. ✅ **ตรวจสอบ Xenolock Guard ก่อนเรียก Extra Deck**:
   - การ์ด Extra Deck ทุกใบที่ต้องกด Activate เอฟเฟกต์ จะไม่สามารถทำงานได้หากบอทได้สั่งใช้ Search ของ Gehenna หรือ Sheol ไปแล้วในเทิร์นนั้น
2. ✅ **ตรวจสอบ Bagooska Defense Position (Rule 14)**:
   - บอทต้องอัญเชิญ Number 41: Bagooska ในสภาพ `FaceUpDefence` เสมอ ห้ามลงในสภาพโจมตีเด็ดขาด
3. ✅ **ตรวจสอบ Target Verification Safeguard (Rule 18)**:
   - ทุกคำสั่ง Quick Destroy ของ Darkness และ Gizmek Orochi ต้องตรวจเช็คว่าสนามคู่แข่งมีการ์ดที่เป็นเป้าหมายได้จริง (`!c.IsShouldNotBeTarget()`) ก่อนคืนค่า `true` เสมอ เพื่อป้องกันไม่ให้เกมบังคับทำลายการ์ดฝั่งตนเอง
4. ✅ **คำสั่ง Build & Deploy ไปยังโฟลเดอร์เป้าหมายหลัก**:
   ```powershell
   cd C:\Users\admin\Documents\EdoGame\src\YGO_SOURCE_CLEAN
   powershell -ExecutionPolicy Bypass -File .\BUILD_AND_DEPLOY.ps1
   ```
   *สคริปต์จะคอมไพล์และกระจายไฟล์ไปยัง `C:\Users\admin\Documents\EdoGame\` โดยอัตโนมัติ*

### 7.3 คำสั่งทดสอบการดวล Headless Simulator (เมื่อผู้ใช้สั่ง Text Duel)
```powershell
dotnet run --project src\YGO_SOURCE_CLEAN\Client_Headless_Fortest\Client_Headless_Fortest.csproj -c Release -- --deck TrirealmRift --opponent DarkMagician --games 10 --timeout 60
dotnet run --project src\YGO_SOURCE_CLEAN\Client_Headless_Fortest\Client_Headless_Fortest.csproj -c Release -- --deck TrirealmRift --opponent ABC --games 10 --timeout 60
dotnet run --project src\YGO_SOURCE_CLEAN\Client_Headless_Fortest\Client_Headless_Fortest.csproj -c Release -- --deck TrirealmRift --opponent BlueEyes --games 10 --timeout 60
dotnet run --project src\YGO_SOURCE_CLEAN\Client_Headless_Fortest\Client_Headless_Fortest.csproj -c Release -- --deck TrirealmRift --opponent Altergeist --games 10 --timeout 60
```
*(เกณฑ์ผ่าน: 0 Rule Violations / 0 Crash / มีการใช้งานคลังรีมูฟคว่ำหน้าอย่างคุ้มค่า)*
