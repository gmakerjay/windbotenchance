# Orcust WCQ (Modern Orcust WCQ) Audit & Playbook

## 1. Executive Summary & Architecture Overview
`OrcustWCQ` คือเด็คสาย Dark Machine Control & Quick Effect Disruption ระดับแชมป์โลก (World Championship Qualifier) ผสานเข้ากับการ์ดยุคใหม่ปี 2026:
1. **Orcust Control Engine**: ลูปสุสานอันทรงพลัง (`Girsu`, `Harp Horror`, `Cymbal Skeleton`, `Orcust Knightmare`, `World Wand`)
2. **Orcustrated Babel & Crescendo**: เปลี่ยนเอฟเฟกต์ Orcust ทั้งหมดในสุสานและสนามให้กลายเป็น **Quick Effect ในเทิร์นคู่ต่อสู้** พร้อมเคาน์เตอร์แทรป Omni-Negate & Banish
3. **Dingirsu, the Orcust of the Evening Star**: บอส Rank 8 ที่สามารถ Xyz ทับ Orcust Link มอนสเตอร์ได้ทันที มีความสามารถส่งการ์ดศัตรูลงสุสานแบบ **ไม่ล็อกเป้าหมาย (Non-Targeting, Non-Destruction)** และมีเอฟเฟกต์ถอดวัตถุดิบปกป้องการ์ดบนสนามไม่ให้ถูกทำลาย
4. **2026 Extra Deck Tools**: `Galatea-i` (Link-1 เสิร์ช Babel / ชุบตัวเอง), `Enlilgirsu` (Link-4 แย่งการ์ดศัตรู), `S:P Little Knight`, `I:P Masquerena`, `Accesscode Talker`
5. พัฒนาบนสถาปัตยกรรม **Rule-Based ModernExecutor + Decoupled DeckPluginBase (100% C# Rules, ปราศจาก Neural/AI Training)**

---

## 2. Headless Duel Verification Results (40 Games Benchmark)

| Opponent Deck | Games Run | Successful | Violations | Engine Crashes | Bot Win Rate |
| :--- | :---: | :---: | :---: | :---: | :---: |
| **BlueEyes** | 10 | 10 | **0** | **0** | 30.0% (3-7) |
| **DarkMagician** | 10 | 10 | **0** | **0** | **40.0%** (4-6) |
| **ABC** | 10 | 10 | **0** | **0** | 30.0% (3-7) |
| **Altergeist** | 10 | 10 | **0** | **0** | **60.0%** (6-4) |
| **TOTAL** | **40** | **40** | **0 (100% Clean)** | **0** | **40.0% Avg** |

### สรุปสถิติความเสถียร:
- **0 Rule Violations**: ไม่มีข้อผิดพลาดด้านกฎกติกาแม้แต่ครั้งเดียวตลอด 40 เกม
- **0 Engine Crashes**: เซสชันการดวลผ่านฉลุย 100%
- **Zero Self-Negate & Safe Option Resolution**: แก้ไขการประเมิน Option Description IDs สำหรับ `Dingirsu` และ `Pot of Prosperity` ทำให้ไม่มี MSG_RETRY หลุดออกมา

---

## 3. Playbook & End Board Strategy

### Route A: Girsu 1-Card Full Control Board
1. Normal Summon `Girsu, the Orcust Mekk-Knight`
2. `Girsu` Effect 1: ส่ง `Orcust Harp Horror` จากเด็คลงสุสาน
3. `Girsu` Effect 2: สปอว์น `World Legacy Token` ลงสนามทั้งสองฝ่าย
4. เอา `Girsu` + `Token` ลิงก์เป็น **`Galatea, the Orcust Automaton`** (Link-2)
5. `Harp Horror` ในสุสาน รีมูฟตัวเอง Special Summon `Orcust Cymbal Skeleton` จากเด็ค
6. `Galatea` สับ `Harp Horror` ที่ถูกรีมูฟกลับเข้าเด็ค -> หมอบ **`Orcust Crescendo`** จากเด็ค
7. เอา `Galatea` โอเวอร์เลย์เป็น **`Dingirsu, the Orcust of the Evening Star`** (Rank 8)
8. `Dingirsu` Effect: ดูดมอนสเตอร์ Machine ที่ถูกรีมูฟกลับมาเป็นวัตถุดิบ Xyz
9. เอา `Dingirsu` + `Cymbal Skeleton` ลิงก์เป็น **`I:P Masquerena`**
10. หมอบ `Orcust Crescendo` พร้อมส่งผ่านเทิร์น!

### Turn Opponent Disruption:
- เมื่อศัตรูเปิดเอฟเฟกต์: `Crescendo` Negate & Banish ทันที
- เมื่อศัตรูลงมอนสเตอร์: `I:P Masquerena` ลิงก์เป็น `S:P Little Knight` บานิชการ์ดศัตรู
- หากมี `Orcustrated Babel` บนสนาม: `Cymbal Skeleton` ในสุสาน โดดชุบ `Dingirsu` แบบ Quick Effect ส่งการ์ดศัตรูลงสุสานแบบไม่เล็งเป้า และ Dingirsu ถอดวัตถุดิบกันการ์ดเราพังทั้งสนาม!

---

## 4. Deployment Status
- ไบนารีทั้งหมดได้รับการ Deploy สู่ `C:\Users\admin\Documents\EdoGame\` ครบถ้วน:
  - `WindBot/WindBot.dll`
  - `WindBot/ExecutorBase.dll`
  - `WindBot/core.dll`
  - `WindBot/bots.json`
  - `DashBot.exe`
  - `deck/OrcustWCQ.ydk`
