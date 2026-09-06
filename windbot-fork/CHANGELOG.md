# CHANGELOG — WindBot AI Platform (YugiohTH)

All notable changes to the WindBot AI executor system are documented here.  
Format follows [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

---

## [Unreleased] — 2026-05-31

### 🐛 Bug Fixes — Executor Audit (2024 / 2025 / 2026)

---

#### `ExecutorBase/Game/AI/DefaultExecutor.cs`

- **[FIX] `DefaultDontChainMyself()` — Double-Chain Prevention**
  - เพิ่มการตรวจ `Util.ChainContainsCard(Card.Id)` ก่อน return
  - ป้องกันกรณีที่การ์ด ID เดียวกัน 2 ใบถูก activate พร้อมกัน (เช่น 2x Infinite Impermanence ที่ถูกคว่ำแล้ว flip พร้อมกัน) bot จะไม่ chain ทั้งสองใบในช่วง chain เดียวกันอีกต่อไป
  - ครอบคลุมทุก Executor ที่ใช้ `DefaultDontChainMyself` (Generic Executors ทั้ง 9 ตัว, NeuralExecutor, EvilswarmExecutor, GraydleExecutor, DoEveryThingExecutor)

---

#### `Game/AI/Decks/CrystronTrainsExecutor.cs`

- **[FIX] `SulfefnirEffect()` — Self-Pop Deck Guard**
  - เพิ่มการตรวจ `Bot.Deck.Any(c => c.HasSetcode(0xe3) && c.IsMonster())` ก่อน self-pop
  - Sulfefnir จะไม่ทำลายตัวเองเมื่อ Crystron monsters หมด Deck แล้ว ป้องกันการเสียการ์ดฟรีๆ

- **[FIX] `SulfadorEffect()` — Self-Destroy Deck Guard**
  - เพิ่มการตรวจว่า Deck มี Crystron monster ที่ Sulfador จะ Special Summon ได้จริง (Citree / Smiger / Tristaros / Sulfefnir) ก่อนทำลายการ์ดตัวเอง
  - ป้องกัน bot ทำลาย Crystron หรือ Inclusion โดยเปล่าประโยชน์เมื่อ Deck ว่าง

- **[FIX] `SmigerFieldEffect()` — Location Guard + Deck Check (Double-Chain Prevention)**
  - เพิ่ม `if (Card.Location != CardLocation.MonsterZone) return false` เป็น guard แรก
  - เพิ่มการตรวจว่า Deck มี Crystron Level >= 2 ก่อนทำลายการ์ด
  - ป้องกันกรณี Smiger 2 ใบ (Field + Grave) ทำ double-chain ในช่วง chain เดียวกัน

- **[FIX] `SmigerGraveEffect()` — Location Guard (Double-Chain Prevention)**
  - เพิ่ม `if (Card.Location != CardLocation.Grave) return false` เป็น guard แรก
  - ป้องกันการ activate ผิด location เมื่อมี Smiger หลายใบ

- **[FIX] `InclusionActivate()` — OTK Guard**
  - เพิ่มการตรวจ `Duel.MainPhase.CanBattlePhase` + total ATK vs Enemy LP
  - Bot จะไม่ activate Inclusion เพื่อค้นหาเพิ่มเมื่อสามารถ OTK ได้แล้ว

- **[FIX] `SwitchyardActivate()` — Boss + OTK Guard**
  - เพิ่มการตรวจ OTK ผ่าน total ATK check
  - เพิ่มการตรวจว่ามี Train Boss Monster (Gustav Max / Juggernaut Liebe / Gustav Rocket / Super Dora / Flying Launcher) บนสนามและมีมอนสเตอร์ >= 3 ตัว
  - Bot จะไม่ activate Switchyard เพื่อค้นหาต่อเมื่อ board พร้อม OTK แล้ว

---

#### `Game/AI/Decks/Tour2024_DogmaExecutor.cs`

- **[FIX] `ShouldStopSummoning()` — Improved Boss Detection & OTK Guard**
  - ลด Turn threshold จาก `Turn >= 5` → `Turn >= 3`
  - ลด monster count threshold จาก `>= 4` → `>= 3`
  - ขยาย boss detection จากแค่ SkullGuardian/OddEyesPendulumgraph → รวม `Attack >= 2800` (ครอบคลุม boss card อื่นๆ ที่ไม่ได้ระบุชื่อ)
  - เพิ่ม `HasLethal()` check ก่อนทุก condition — หยุด summoning ทันทีเมื่อ OTK possible
  - ผลลัพธ์: Bot จะหยุดค้นหา/ซัมมอนเพิ่มเร็วขึ้นมากเมื่อ board แข็งแกร่งพอ

---

#### `Game/AI/Decks/Tour2025_DrytronExecutor.cs`

- **[FIX] `CyberEmergencyEffect()` — Boss Ritual Guard**
  - เพิ่มการตรวจว่า `DrytronMeteonisDADraconids` หรือ `DrytronMeteonisDraconids` อยู่บนสนามแล้วหรือไม่
  - Bot จะไม่ activate CyberEmergency เพื่อค้นหา Drytron เพิ่มเมื่อ Boss Ritual ยืนอยู่บนสนามแล้ว

- **[FIX] `DrytronNovaEffect()` — Boss Ritual Guard**
  - เพิ่มการตรวจ Boss Ritual เช่นเดียวกับ CyberEmergency
  - Bot จะไม่ waste DrytronNova เพื่อ search เมื่อ boss พร้อมแล้ว

---

### 📋 Summary of Root Causes Found

| Bug | Executor | Root Cause | Severity |
|-----|----------|-----------|---------|
| ทำร้ายตัวเอง | CrystronTrains | SulfefnirEffect / SulfadorEffect / SmigerFieldEffect ไม่มี Deck guard | 🔴 CRITICAL |
| Double-chain | DefaultExecutor | DefaultDontChainMyself ไม่ตรวจ chain ปัจจุบัน | 🔴 CRITICAL |
| Double-chain | CrystronTrains | SmigerFieldEffect / SmigerGraveEffect ไม่มี Location guard | 🔴 CRITICAL |
| Over-search | CrystronTrains | InclusionActivate / SwitchyardActivate return true เสมอ | 🟡 MEDIUM |
| Over-search | Tour2024_Dogma | ShouldStopSummoning threshold สูงเกินไป (Turn 5) | 🟡 MEDIUM |
| Over-search | Tour2025_Drytron | CyberEmergency / DrytronNova ไม่มี boss guard | 🟡 MEDIUM |

---

### ✅ Build Status
- `WindBot.csproj` — **Build Succeeded** (0 Errors, 17 Warnings — all pre-existing)
- `libWindbot.csproj` — Skipped (requires Xamarin Android SDK, unrelated to this fix)

---

*Changelog maintained by: Antigravity (AI Agent)*  
*Session date: 2026-05-31*
