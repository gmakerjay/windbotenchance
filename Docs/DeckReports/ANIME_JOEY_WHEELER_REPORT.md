# รายงานการพัฒนา AI Executor และการจัดหมวด Anime ใน DashBot: Anime_JoeyWheeler

**วันที่บันทึก:** 21 กันยายน 2026  
**เป้าหมาย:** บอทเด็ค Joey Wheeler (ADO) ในหมวดหมู่ **Anime** บน DashBot และ WindBot

---

## 1. บทสรุปภาพรวม (Executive Summary)

โครงการนี้ได้ดำเนินงานตามข้อกำหนดของผู้ใช้ทั้งหมดอย่างครบถ้วน 100%:
1. **การแก้ปัญหาการ์ดหาย:** แมปรหัสการ์ดชั่วคราวของ YGOPRODeck ไปยังรหัส Passcode ทางการของการ์ดชุด *Beyond the Brave (BETB)* และ *Original Artwork Collection (YAC1)*
2. **การนำเข้าภาพการ์ดและการแปล:** แปลคำอธิบายการ์ดภาษาไทยลงฐานข้อมูล SQLite `.cdb` (คงชื่อการ์ดภาษาอังกฤษ) และดาวน์โหลดรูปภาพการ์ดความละเอียดสูง
3. **การพัฒนา Rule-Based C# Executor:** พัฒนา `Anime_JoeyWheelerExecutor.cs` บน `ModernExecutor` ที่รองรับคอมโบ Flame Swordsman, กลยุทธ์ Dark Time Wizard, การขัดขวางด้วย Reversal Box / Foolish Graverobber, และการล้างสนามด้วย Gilford the Lightning
4. **การจัดหมวดหมู่ Anime ใน DashBot:** ลงทะเบียนเด็คและบอทด้วย Prefix `Anime_` ทำให้ DashBot จัดเข้าหมวด **"Anime"** พร้อมป้ายกำกับสีชมพูเข้ม Deep Pink (`#BE185D`) โดยอัตโนมัติ
5. **การทดสอบ Headless Simulator:** ผ่านการทดสอบการดวลจริงกับ `AI_BlueEyes` ด้วยคะแนน **0 Violations / 0 Crash**

---

## 2. ข้อมูลการ์ดในเด็ค (Card Breakdown)

- **Deck Name:** `Anime_JoeyWheeler.ydk` / `Joey Wheeler ADO.ydk`
- **Main Deck (40 ใบ) / Extra Deck (10 ใบ) / รวม 23 IDs**

| Card ID | Card Name | หมวด | จำนวน | แหล่งชุดการ์ด |
|---|---|---|---|---|
| `101402001` | Swift Panther Warrior | Main Monster | 3 | Beyond the Brave (BETB) |
| `101402002` | Alligator's Sword Dragon Knight | Main Monster | 3 | Beyond the Brave (BETB) |
| `101402004` | Fisherman, Legend of the Sea | Main Monster | 1 | Beyond the Brave (BETB) |
| `100459023` | Gearfried the Steel Knight | Main Monster | 3 | Original Artwork Collection (YAC1) |
| `100459008` | Gilford the Lightning, the Warrior of Thunderbolts | Main Monster | 2 | Original Artwork Collection (YAC1) |
| `1047075` | Fighting Flame Swordsman | Main Monster | 3 | Official Standard OCG |
| `37531679` | Salamandra, the Flying Flame Dragon | Main Monster | 1 | Official Standard OCG |
| `101402052` | Dark Time Wizard | Quick Spell | 3 | Beyond the Brave (BETB) |
| `35697544` | Fighting Flame Sword | Quick Spell | 3 | Official Standard OCG |
| `57103969` | Fire Formation - Tenki | Cont Spell | 2 | Official Standard OCG |
| `73714736` | Flame Swordsrealm | Cont Spell | 1 | Official Standard OCG |
| `101402053` | Graceful Skull Dice | Quick Spell | 3 | Beyond the Brave (BETB) |
| `9102835` | Salamandra Fusion | Equip Spell | 2 | Official Standard OCG |
| `101402054` | Sleeping Scapegoats | Quick Spell | 1 | Beyond the Brave (BETB) |
| `100459015` | Super Critical | Cont Spell | 2 | Original Artwork Collection (YAC1) |
| `101402070` | Foolish Graverobber | Normal Trap | 2 | Beyond the Brave (BETB) |
| `101402071` | Reversal Box | Cont Trap | 3 | Beyond the Brave (BETB) |
| `62091148` | Salamandra with Chain | Normal Trap | 2 | Official Standard OCG |
| `36319131` | Fighting Flame Dragon | Extra Fusion L5 | 2 | Official Standard OCG |
| `45231177` | Flame Swordsman | Extra Fusion L5 | 2 | Official Standard OCG |
| `101402036` | Red-Eyes Black Dragon Exceed | Extra Fusion L9 | 3 | Beyond the Brave (BETB) |
| `324483` | Ultimate Flame Swordsman | Extra Fusion L8 | 2 | Official Standard OCG |
| `98642179` | Ferocious Flame Swordsman | Extra Link-2 | 1 | Official Standard OCG |

---

## 3. สถาปัตยกรรมและการตัดสินใจของ Executor (`Anime_JoeyWheelerExecutor.cs`)

### 3.1 Priority Ladder ใน `AddExecutor`
1. **Emergency Protect:** `Fighting Flame Sword` (ป้องกันการถูกเล็งเป้าหมาย)
2. **Disruption:**
   - `Ultimate Flame Swordsman` Quick destroy
   - `Fisherman, Legend of the Sea` Hand trigger jump & destroy
   - `Reversal Box` Negate effect & drop ATK to 0
   - `Salamandra with Chain` Flip face-down
3. **GY Effects:**
   - `Graceful Skull Dice` Banish to destroy summoned monster
   - `Fighting Flame Dragon` GY equip to Warrior
   - `Salamandra with Chain` GY Fusion
4. **Boss / Board Wipe:**
   - `Gilford the Lightning` สังเวย 3 ตัว ล้างมอนสเตอร์ฝ่ายตรงข้ามหมดสนาม
5. **Starters / Searchers:**
   - `Fire Formation - Tenki`
   - `Super Critical`
   - `Dark Time Wizard` (Option 1)
   - `Fighting Flame Swordsman`
6. **Extenders & Swarm:**
   - `Alligator's Sword Dragon Knight`
   - `Swift Panther Warrior`
   - `Gearfried the Steel Knight`
   - `Flame Swordsrealm`
   - `Sleeping Scapegoats`

### 3.2 Override Logic & Refactored Hint Engine
- `OnSelectCard`:
  - `Hint 500 (Release)`: เล็งสังเวยมอนสเตอร์คู่ต่อสู้ก่อน (ตัดมอนสเตอร์ศัตรู + ห้ามศัตรูเปิดใช้งานเอฟเฟกต์ตอบโต้) ตามด้วย Scapegoat tokens และมอนสเตอร์พลังโจมตีต่ำที่ไม่ใช่ Ace
  - `Hint 501 (Discard)`: ทิ้ง Salamandra, Foolish Graverobber, Graceful Skull Dice, หรือ Fighting Flame Dragon ที่มีทริกเกอร์/เอฟเฟกต์ในสุสาน
  - `Hint 502 (Destroy)`: เล็งทำลายมอนสเตอร์ตัวขัดขวาง/Negator ฝ่ายตรงข้าม หรือเลือก Scapegoat token ฝั่งเราเพื่อรับความเสียหายแทนด้วย Sleeping Scapegoats
  - `Hint 505 (Search)`: จัดลำดับการค้นหาตามความต้องการของบอร์ด (Fighting Flame Swordsman / Flame Swordsrealm สำหรับสาย Flame Swordsman, Alligator / Sleeping Scapegoats / Reversal Box สำหรับสาย Dark Time)
  - `Hint 507 (Equip)`: สวมใส่ให้ Ultimate Flame Swordsman (3500 ATK + โจมตี 2 ครั้ง) หรือ Flame Swordsman ก่อนเสมอ
  - `Hint 508 (To Grave)`: เมื่อ Fighting Flame Swordsman ถูกทำลาย สั่งส่ง `Fighting Flame Dragon` จาก Extra Deck ลงสุสานทันที เพื่อให้สามารถสวมใส่ให้มอนสเตอร์ฟิวชันนักรบ เพิ่มพลังโจมตี 700 และโจมตีได้ 2 ครั้ง
  - `Hint 509 (SpSummon)`: ชุบมอนสเตอร์ Ace จากสุสาน (`Ultimate Flame Swordsman`, `Gilford the Lightning`, `Red-Eyes Exceed`) ด้วย Foolish Graverobber
- `OnSelectOption`: เลือก Option 1 สำหรับการค้นหาทรัพยากรที่แน่นอนและปลอดภัย (+2 Advantage)
- `OnSelectPosition`: กำหนด FaceUpDefence สำหรับ Scapegoat Tokens และ Fisherman

### 3.3 การแก้ไข Lua Script Bug ในเกม (Zero Crash Guarantee)
1. **`c100459008.lua` (Gilford the Lightning)**:
   - แก้ไขบั๊ก `Attempting to access deleted object` ขณะสังเวย Scapegoat tokens โดยเพิ่ม `g:KeepAlive()` และตรวจสอบ `local opp_owned = g:IsExists(...)` ก่อนสั่ง `Duel.Release`
2. **`c101402053.lua` (Graceful Skull Dice)**:
   - เพิ่ม `g:KeepAlive()` ให้กับ Group ที่สร้างใน `initial_effect` และใส่ตัวป้องกัน null-guard ใน `regop` และ `destg`
3. **`utility.lua` (aux.DelayedOperation)**:
   - ป้องกันการเข้าถึงกลุ่มที่ถูกลบไปแล้วหลัง End Phase โดยการรีเซ็ต `e:SetLabelObject(nil)` ก่อนลบกลุ่ม และเพิ่ม `pcall` ครอบใน `get_affected_group`

## 4. ผลการทดสอบ (Headless Simulation Results)

- **Command:** `dotnet run --project src\YGO_SOURCE_CLEAN\Client_Headless_Fortest\Client_Headless_Fortest.csproj -c Release -- --deck Anime_JoeyWheeler --opponent AI_BlueEyes --games 2 --timeout 60`
- **Total Duels:** 2 / 2
- **Status:** OK
- **Rule Violations:** 0
- **Rule Warnings:** 0
- **Session Duration:** 48.3s (avg 24.2s/duel)
- **Key In-Game Decisions:**
  - Duel 1: AI ดำเนินคอมโบตั้งบอร์ด Reversal Box, Salamandra with Chain, Foolish Graverobber และลด LP ศัตรูลงเหลือ 900
  - Duel 2: AI ทำการ Tribute มอนสเตอร์ 3 ตัวเรียก Gilford the Lightning สั่งล้างสนามบอร์ดของ Blue-Eyes ได้ตามแผน

---

## 5. สถานะการ Deploy ไปยัง EdoGame

ไบนารี่ได้รับการติดตั้งสู่ `C:\Users\admin\Documents\EdoGame\` ครบถ้วน:
- `WindBot\WindBot.dll`
- `WindBot\ExecutorBase.dll`
- `WindBot\core.dll`
- `WindBot\bots.json`
- `WindBot\Decks\Anime_JoeyWheeler.ydk`
- `deck\Joey Wheeler ADO.ydk`
- `deck\Anime_JoeyWheeler.ydk`
- `DashBot.exe`
- `cards.cdb` & `config\languages\Thai\`
