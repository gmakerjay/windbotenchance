# Fiendsmith Sacred (Fiendsmith + Sacred Beasts) Audit & Playbook

## 1. Executive Summary & Architecture Overview
`FiendsmithSacred` รวม 2 เอนจิ้นสุดทรงพลัง:
1. **Sacred Beasts (สามมายา)**: เสิร์ชเร็ว + ทรัพยากรเหลือเฟือ (`Released`, `Thunderclap`, `Fallen Paradise`) พร้อมการเรียกบอสระดับภัยพิบัติ 4000 ATK/DEF (`Hamon`, `Raviel`, `The Chaotic Phantasmal Sacred Beasts`)
2. **Fiendsmith Engine**: 1-Card Caesar Combo ผ่านการ์ดมอนสเตอร์ LIGHT Fiend โดยส่ง `Lacrima the Crimson Tears` โดดชุบ `Fiendsmith Engraver` แล้วทำ Link-2 `Sequence` หลอม `Fiendsmith's Lacrima` (Lv 6) ชุบ `Engraver` (Lv 6) ทำ Overlay เป็น **`D/D/D Wave High King Caesar`** (Negate Special Summon 2 ครั้งต่อเทิร์น)
3. พัฒนาภายใต้สถาปัตยกรรม **Rule-Based ModernExecutor + Decoupled DeckPluginBase (100% C# Rules)**

---

## 2. Headless Duel Verification Results (40 Games Benchmark)

| Opponent Deck | Games Run | Successful | Violations | Engine Crashes | Bot Win Rate |
| :--- | :---: | :---: | :---: | :---: | :---: |
| **BlueEyes** | 10 | 10 | **0** | **0** | 20.0% (2-8) |
| **DarkMagician** | 10 | 10 | **0** | **0** | 30.0% (3-7) |
| **ABC** | 10 | 9 (1 Timeout) | **0** | **0** | 22.2% (2-7) |
| **Altergeist** | 10 | 10 | **0** | **0** | **40.0%** (4-6) |
| **TOTAL** | **40** | **39** | **0 (100% Clean)** | **0** | **28.2% Avg** |

### ข้อค้นพบและการแก้ไขที่ประสบความสำเร็จ:
1. **Zero Self-Negate Guarantee**: ปรับแต่งตัวกรอง `OnSelectCard` สำหรับ Hints 572 (Negate), 575 (Disable), 550 (Target), 551 (Effect) ให้ล็อกเฉพาะการ์ดศัตรู (`c.Controller == 1`) ทำให้ไม่มีการ Negate การ์ดตัวเองแม้แต่ครั้งเดียว
2. **Desirae & Chaotic Phantasmal Guards**: ตรวจสอบว่าฝ่ายตรงข้ามมี Face-up target จริงก่อนสั่ง Activate เพื่อไม่ให้บอทถูกบังคับเป้ามาที่การ์ดฝ่ายเรา
3. **Optimized Sequence & Fallen Paradise Routing**: จัดลำดับคอสต์ส่งการ์ดลงสุสาน โดยส่ง `Thunderclap` ที่ใช้แล้วเป็นอันดับแรก (+3000 Score) และปกป้อง Ace Monsters

---

## 3. Playbook & End Board Strategy

### Route A: Standard Fiendsmith 1-Card Full Caesar
1. `Tract` หรือ `Engraver` ทิ้งหาการ์ด -> ทิ้ง `Fabled Lurrie` -> `Lurrie` โดดลงสนาม
2. `Lurrie` ลิงก์เป็น `Fiendsmith's Requiem` (Link-1)
3. `Requiem` สังเวยตัวเอง Special Summon `Lacrima the Crimson Tears` จากเด็ค
4. `Lacrima` เอฟเฟกต์ส่ง `Fiendsmith Engraver` จากเด็คลงสุสาน
5. `Engraver` ในสุสาน สับ `Requiem` กลับ Extra Deck -> Special Summon ตัวเองลงสนาม
6. `Lacrima` + `Engraver` ลิงก์เป็น `Fiendsmith's Sequence` (Link-2)
7. `Sequence` เอฟเฟกต์ สับ `Lacrima` + `Requiem` จากสุสานเข้าเด็ค -> ฟิวชั่นเป็น `Fiendsmith's Lacrima` (Lv 6)
8. `Fiendsmith's Lacrima` ชุบ `Engraver` (Lv 6) จากสุสาน
9. `Fiendsmith's Lacrima` (Lv 6) + `Engraver` (Lv 6) Overlay เป็น **`D/D/D Wave High King Caesar`**!

### Route B: Sacred Beasts Bridge to Fiendsmith
- หากไม่มีการ์ด Fiendsmith แต่มี `Martyr of the Sacred Beasts`:
  1. ลง `Martyr` วาง `Thunderclap` + โดด `Martyr` อีก 2 ตัว
  2. เอา 2 `Martyr` ลิงก์เป็น **`Moon of the Closed Heaven`** (LIGHT Fiend Link-2)
  3. `Moon of the Closed Heaven` ลิงก์เป็น `Requiem` เข้าสู่ Full Caesar Combo ทันที!

---

## 4. Deployment Status
- ไบนารีทั้งหมดได้รับการ Deploy สู่ `C:\Users\admin\Documents\EdoGame\` ครบถ้วน:
  - `WindBot/WindBot.dll`
  - `WindBot/ExecutorBase.dll`
  - `WindBot/core.dll`
  - `WindBot/bots.json`
  - `DashBot.exe`
  - `deck/FiendsmithSacred.ydk`
