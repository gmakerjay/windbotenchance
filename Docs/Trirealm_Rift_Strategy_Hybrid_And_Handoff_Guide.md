# Trirealm Rift (Sky Thunder Trirealm Rift Yomi) Strategy, Hybrid Analysis & Developer Handoff Guide

**เอกสารแนวทางการพัฒนา กลยุทธ์การเล่น การวิเคราะห์การ์ดไฮบริด และคู่มือส่งต่องาน (Handoff)**  
**Project**: YugiohTH / WindBot ModernExecutor  
**Target Environment**: C# .NET 10.0 | WindBot AI Engine  
**Exclusive Deployment Path**: `C:\Users\admin\Documents\EdoGame\`  
**Date**: 2026-09-28  

---

## 1. บทสรุปปัญหาและการ Refactor ระบบ (Problem & Refactoring Summary)

ก่อนหน้านี้เด็ค **Trirealm Rift (異界の裂け目 / 鳴神の裂界)** ประสบปัญหา **"การ์ดมาช้า จุดอ่อนเยอะ และแพ้เกมอย่างต่อเนื่อง"** จากการทดสอบผ่าน `Client_Headless_Fortest` ต่อกรกับเด็คมาตรฐาน เช่น `DarkMagician` มีอัตราการชนะ 0% (0/10) จากการสืบสวนเชิงลึกพบสาเหตุหลัก 4 ประการที่ได้รับการแก้ไขแล้ว:

1. **Bug Flag ซ้อนทับบล็อกเอฟเฟกต์ Search +2 (One-Shot Burnout)**:
   - `Gehenna` (Lv 1) มี Trigger รีมูฟ 1 ใบ และ Ignition เสิร์ชมอนสเตอร์ 2 ใบ
   - `Sheol` (Lv 2) มี Trigger รีมูฟ 2 ใบ และ Ignition เสิร์ชเวท/กับดัก 2 ใบ
   - *เดิม*: ระบบใช้ Flag เดียวกันตรวจจับทั้ง Summon Trigger และ Ignition Search ทำให้เมื่อมอนสเตอร์ลงสนาม Trigger ทำงานเสร็จ Flag กลายเป็น `true` บล็อกไม่ให้บอทกด Search +2 ใน Main Phase เลยตลอดทั้งเกม
   - *แก้ไข*: แยก Flag อย่างเด็ดขาด และปรับ `PickSearchTargets` ให้สามารถหยิบการ์ดได้สูงสุดตามโควต้า `max` (หยิบ 2 ใบชื่อต่างกัน) ทำให้การ์ดขึ้นมือมหาศาล
2. **Summon Trigger ของมอนสเตอร์ตัวใหญ่ (Lv 5–8) ไม่ทำงาน**:
   - `Yomi` (Lv 5), `Helheim` (Lv 6), `Ploutonion` (Lv 7), `Darkness` (Lv 8) ทุกตัวมี Trigger รีมูฟใบบนสุด 5, 6, 7, 8 ใบตามลำดับ แต่เดิมไม่มีการลงทะเบียน Executor รองรับ
   - *แก้ไข*: เพิ่ม Phase 3: `OnSummonBanishTrigger` สั่งให้มอนสเตอร์ทุกตัวรีมูฟการ์ดคว่ำหน้าลงคลังทันทีที่ลงสนาม
3. **การ์ด Gospel ถูก Set คว่ำแทนที่จะเปิดใช้**:
   - `Trirealm Rift Gospel` (100458040) เป็น **Continuous Spell** (ไม่ใช่ Quick-Play) เดิมถูกสั่งให้ SpellSet ทำให้ติด PhaseGuard และไม่ถูกเปิดใช้งาน
   - *แก้ไข*: สั่งเปิดใช้งานหงายหน้าจากมือทันที เพื่อรีมูฟ 5 ใบคว่ำหน้า + เพิ่ม Normal Summon อีก 1 ครั้ง และ Morph ตัวเล็กเป็นบอส
4. **ขาดตัวเร่ง Banish Face-Down ในเทิร์นแรก**:
   - เพิ่ม `Gizmek Orochi, the Serpentron Sky Slasher` (71197066) 2 ใบ (Quick Effect รีมูฟคว่ำ 8 ใบ สเปเชียลฟรี)
   - เพิ่ม `Terraforming` (73628505) 1 ใบ (การันตี Field Spell Valvols)
   - ปรับ `Pot of Desires` (35261759) เป็น 3 ใบเต็ม

---

## 2. การวิเคราะห์การเล่นร่วมกับการ์ดอื่น (Hybrid Synergy Analysis)

### A. 👹 Gren Maju Da Eiza (36584821) ➔ **เข้ากันได้ดีมาก (Recommended ⭐⭐⭐⭐⭐)**

* **ผลการวิเคราะห์**: สามารถใส่ 1–2 ใบ เป็น **ไม้ตายปิดเกม (OTK Finisher)**
* **จุดเด่นตามกฎการ์ด (Official Rulings)**:
  1. **นับการ์ดรีมูฟคว่ำหน้าทั้งหมด**: ข้อความระบุว่า *"ATK/DEF become the number of your banished cards x 400"* ซึ่งการ์ดที่ถูกรีมูฟแบบคว่ำหน้า (Face-down Banish) ก็นับเป็น banished cards ตามกฎสากล
  2. **ไม่โดน Xenolock บล็อก**: การ์ด Trirealm Rift แทบทุกใบมีเงื่อนไขล็อค: *"cannot **activate** cards or effects, except 'Trirealm Rift'"* แต่เอฟเฟกต์เพิ่มพลังของ Gren Maju เป็น **Continuous Effect (เอฟเฟกต์ต่อเนื่อง ไม่มีการสั่ง Activate / ไม่เกิด Chain)** ทำให้ Gren Maju มีพลังมหาศาลเสมอแม้ติด Xenolock
  3. **พลังทำลายล้างสูง**:
     - รีมูฟ 10 ใบ (เช่น จาก Pot of Desires) = ATK 4,000
     - รีมูฟ 20 ใบ (คอมโบปกติ) = ATK 8,000 (OTK ทันที)
     - รีมูฟ 30 ใบ = ATK 12,000
  4. **คอมโบร่วมกับ Gospel**: `Trirealm Rift Gospel` ให้สิทธิ์ Normal Summon เพิ่ม 1 ครั้ง ทำให้ลง Gren Maju ได้โดยไม่ต้องแย่งสิทธิ์ Normal Summon ของตัวเริ่มในธีม

### B. 💀 Necroface (28297833) ➔ **เล่นเป็นแกนหลักไม่ได้ / ใช้เป็น Safety Net ได้เฉพาะทาง (Niche ⭐⭐)**

* **ข้อจำกัดร้ายแรง**:
  1. **มิลล์คว่ำหน้า Trigger ไม่ทำงาน**: หาก Necroface ถูกรีมูฟแบบคว่ำหน้า (Face-down) เอฟเฟกต์ *"If this card is banished: Each player banishes 5 cards..."* จะ **ไม่ทำงาน** ตามกฎทางการ
  2. **การ Normal Summon เป็นการล้างกระดานตัวเอง**: เอฟเฟกต์ของ Necroface จะบังคับสับการ์ดที่ถูกรีมูฟทั้งหมดกลับเข้าเด็ค ซึ่งจะทำลายคลังการ์ดรีมูฟของ Trirealm Rift จนหมด ทำให้ Yomi, Helheim, Darkness, Ploutonion ใช้งานเอฟเฟกต์ไม่ได้ทันที
* **ประโยชน์เฉพาะทางเพียงหนึ่งเดียว**:
  - ใช้เป็น **"ปุ่มรีเซ็ตฉุกเฉิน (Emergency Deck-Out Reset)"** ในสถานการณ์ Late Game เมื่อเด็คตัวเองเหลือ 0–2 ใบ และยังปิดเกมไม่ได้ สามารถ Normal Summon Necroface เพื่อสับการ์ดกลับเด็ค ป้องกันการแพ้จากการ์ดหมดเด็ค (Deck Out Loss) ในเทิร์นถัดไป พร้อมได้ตัวตี ATK 3,500–4,500+

### C. 👻 Dark Necrofear (31829185) / Curse Necrofear (14509651) ➔ **เล่นไม่ได้ (Incompatible ❌)**

* **สาเหตุที่ไม่สามารถเล่นร่วมกันได้**:
  1. **เผ่าพันธุ์ไม่ตรงกัน**: มอนสเตอร์ Trirealm Rift ทั้งหมดเป็น **เผ่าไซคิก (Psychic)** ในขณะที่ Necrofear ต้องการมอนสเตอร์ **เผ่าปีศาจ (Fiend)** 3 ตัว
  2. **สุสานว่างเปล่า**: Trirealm Rift มิลล์การ์ดออกนอกเกมแบบคว่ำหน้าโดยตรง สุสานจึงไม่มีการ์ดให้ Dark Necrofear รีมูฟ
  3. **ติด Xenolock**: เอฟเฟกต์ของ Trirealm Rift ปิดกั้นไม่ให้เปิดใช้เอฟเฟกต์ของ Necrofear

---

## 3. การ์ดและกลยุทธ์คุ้มกัน Gren Maju Da Eiza

จุดอ่อนที่สุดของ Gren Maju คือ **"ไม่มีเอฟเฟกต์ป้องกันตัวเอง"** และหากถูก Effect Negate (เช่น Infinite Impermanence หรือ Effect Veiler) **พลังโจมตีจะกลายเป็น 0 ทันที**

แนวทางการคุ้มกันที่ดีที่สุด:
1. **ใช้ [Sky Thunder Trirealm Rift Yomi] บนสนาม**:
   - Yomi มี Quick Negate มอนสเตอร์คู่แข่ง คอยสกัด Effect Veiler หรือมอนสเตอร์ขัดขวางของคู่แข่งได้
2. **ใช้ [Trirealm Rift Territory - Valvols] (เมื่อเด็คหมด 0 ใบ)**:
   - หากเด็คเหลือ 0 ใบ และมีมอนสเตอร์ Trirealm Lv 5+ คู่แข่งจะไม่สามารถเปิดใช้การ์ดหรือเอฟเฟกต์ใดๆ ได้เลยในเทิร์นเรา ทำให้ Gren Maju วิ่งเข้าโจมตีได้อย่างปลอดภัย 100%
3. **การ์ดเสริมคุ้มกัน (Staples & Protections)**:
   - `Called by the Grave`: จัดการ Handtrap มอนสเตอร์ในสุสาน
   - `Crossout Designator`: ประกาศชื่อ Imperm / Veiler เพื่อ Negate และยังช่วยเพิ่มการ์ดรีมูฟให้ Gren Maju +400 ATK
   - `Sauravis, the Ancient and Ascended`: ทิ้งจากมือเพื่อคุ้มกันไม่ให้ Gren Maju ตกเป็นเป้าหมาย
   - `Forbidden Droplet` / `Dark Ruler No More`: ล้างเอฟเฟกต์มอนสเตอร์คู่แข่งทั้งสนามก่อนลง Gren Maju

---

## 4. โครงสร้าง Decklist ล่าสุด (`TrirealmRift.ydk`)

**Main Deck (40 ใบ)**:
* **Monsters (20)**:
  - 3x `Trirealm Rift of Emptiness Gehenna` (100458031) - Starter ฟรี SS / เสิร์ช 2 มอนสเตอร์
  - 3x `Trirealm Rift of Sky Sheol` (100458032) - Extender / เสิร์ช 2 เวทกับดัก
  - 2x `Trirealm Rift of Blue Tuonela` (100458033) - Extender โดด Lv 5+
  - 2x `Trirealm Rift of Scarlet Naraka` (100458034) - Disruption เทิร์นคู่แข่ง
  - 3x `Sky Thunder Trirealm Rift Yomi` (100458035) - Boss Quick Negate / Searcher
  - 2x `Burial Summit Trirealm Rift Helheim` (100458036) - Boss คุ้มกันบอร์ด / ชุบตัวใหญ่
  - 2x `Mad Tempest Trirealm Rift Ploutonion` (100458037) - Draw 2 / Quick Spin
  - 1x `Trirealm Rift Darkness` (100458038) - 3000 ATK Finisher
  - 2x `Gizmek Orochi, the Serpentron Sky Slasher` (71197066) - Quick Banish 8 ใบคว่ำหน้า
* **Spells (12)**:
  - 3x `Trirealm Rift Territory - Valvols` (100458039) - Field Spell
  - 1x `Terraforming` (73628505) - เสิร์ชฟิลด์
  - 3x `Trirealm Rift Gospel` (100458040) - Continuous Spell เพิ่ม Normal Summon & Morph
  - 3x `Pot of Desires` (35261759) - Banish 10 คว่ำหน้า จั่ว 2
  - 2x `Called by the Grave` (24224830)
* **Traps & Handtraps (8)**:
  - 2x `Trirealm Rift Judgment` (100458041) - Counter Trap
  - 3x `Infinite Impermanence` (10045474)
  - 3x `Ash Blossom & Joyous Spring` (14558127)

*(หมายเหตุ: สามารถแทนที่ Darkness 1 ใบ หรือ Naraka 1 ใบ ด้วย `Gren Maju Da Eiza` 1–2 ใบ เพื่อใช้เป็นแผน OTK ได้ทันที)*

---

## 5. คู่มือส่งต่องานสำหรับนักพัฒนา (Developer Handoff Guide)

### ไฟล์ต้นฉบับหลัก (Source Code Location):
* `src\YGO_SOURCE_CLEAN\windbot-fork\Game\AI\Decks\TrirealmRiftExecutor.cs`: ตัวตัดสินใจเล่นการ์ดและ Phase Management
* `src\YGO_SOURCE_CLEAN\windbot-fork\Game\AI\Plugins\TrirealmRiftPlugin.cs`: Decoupled Domain Plugin (Strategy, Material Evaluator, Multi-Search Logic)
* `src\YGO_SOURCE_CLEAN\windbot-fork\Decks\TrirealmRift.ydk`: Decklist ต้นฉบับ

### คำสั่งคอมไพล์และ Deploy (Build & Deploy Pipeline):
```powershell
cd C:\Users\admin\Documents\EdoGame\src\YGO_SOURCE_CLEAN
powershell -ExecutionPolicy Bypass -File .\BUILD_AND_DEPLOY.ps1
```
*ระบบจะคอมไพล์ WindBot, ExecutorBase, core, DashBot และ Deploy ไบนารีทั้งหมดมาที่ `C:\Users\admin\Documents\EdoGame\` โดยอัตโนมัติ*

### คำสั่งทดสอบการดวล Headless Simulator:
```powershell
dotnet run --project src\YGO_SOURCE_CLEAN\Client_Headless_Fortest\Client_Headless_Fortest.csproj -c Release -- --deck TrirealmRift --opponent DarkMagician --games 10 --timeout 60
```
