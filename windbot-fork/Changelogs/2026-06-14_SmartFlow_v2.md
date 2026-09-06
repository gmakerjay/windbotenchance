# Changelog — Smart Flow v2 (BoardScorer + ModernExecutor Upgrade)
**Date:** 2026-06-14  
**Version:** Smart Flow v2  
**Scope:** ModernExecutor core only — Legacy decks (DefaultExecutor) ไม่ได้รับผลกระทบ

---

## สรุปการเปลี่ยนแปลง

ปรับปรุงระบบ AI ส่วนกลาง (Core) สำหรับเด็คยุค 2026 ให้บอทเล่นได้ "ลื่นไหล" และ "ฉลาดขึ้น" โดยแก้ 5 ปัญหาหลัก:

### ปัญหาที่แก้ไข

| # | ปัญหา | สาเหตุ | วิธีแก้ |
|---|-------|--------|--------|
| 1 | ลงอย่างเดียว ไม่ยอมตี (บอร์ดว่างก็ search ต่อ) | ไม่มี "attack opportunity detection" | เพิ่ม `ShouldAttackBeforeCombo()` + `AttackOpportunityAnalyzer` |
| 2 | ไม่รู้จัก MP1/MP2 | `PhasePlanAnalyzer` อ่อนเกินไป | เพิ่ม `ClassifyAction()` + `ActionPriority` enum |
| 3 | ตีไม่ได้กลางเทิร์น (combo ได้ lethal แต่ยัง search ต่อ) | Lethal check แค่ตอนต้นเทิร์น | เพิ่ม `DynamicLethalCheck()` ทุก idle command |
| 4 | Overextend (combo ต่อจนเสียทรัพยากร/โดน Nibiru) | ไม่มี "board sufficiency" check | เพิ่ม `ShouldStopExtending()` + `ComboSufficiencyAnalyzer` |
| 5 | ShouldAllowActivate ไม่ครอบคลุม | Block แค่ใน rush mode | ปรับให้ block ตาม ActionPriority ใน 3 modes |

---

## ไฟล์ที่เปลี่ยนแปลง

### Modified Files

#### `ExecutorBase/Game/AI/ModernExecutor.cs`
- **เพิ่ม** `ActionPriority` enum (CombatEssential, ComboStarter, ComboExtender, ResourceGain, Deferrable)
- **เพิ่ม** `DynamicLethalCheck()` — re-evaluate lethal ทุก idle command กลางเทิร์น
- **เพิ่ม** `ShouldAttackBeforeCombo(main)` — เช็คว่าควรตีก่อน combo (บอร์ดว่าง/อ่อน)
- **เพิ่ม** `ClassifyAction(card, type)` — แยกประเภท action ตาม priority (virtual, override ได้)
- **เพิ่ม** `ShouldStopExtending()` — เช็คว่า board ดีพอ หยุด combo (virtual, override ได้)
- **ปรับ** `OnSelectIdleCmd` — เพิ่ม 3 checkpoints ใหม่ (DynamicLethal → AttackBeforeCombo → StopExtending)
- **ปรับ** `ShouldAllowActivate` — block ตาม ActionPriority ใน 3 modes (Lethal/AttackFirst/StopExtending)

#### `ExecutorBase/Game/AI/BoardScorer.cs`
- **เพิ่ม** `EstimateDamageIfAttackNow()` — คำนวณ damage ถ้าเข้า battle ตอนนี้
- **เพิ่ม** `BoardSufficiencyScore()` — คะแนน 0-100+ ว่าบอร์ดดีพอแค่ไหน
- **เพิ่ม** `AttackOpportunityScore()` — คะแนน 0-100+ ว่าตอนนี้น่าตีแค่ไหน

#### `ExecutorBase/Game/AI/AnalysisScoreVector.cs`
- **เพิ่ม** 5 fields: `ShouldAttackFirst`, `BoardSufficiency`, `AttackOpportunity`, `ShouldStopExtending`, `EstimatedDirectDamage`

#### `ExecutorBase/Game/AI/AiAnalysisSuite.cs`
- **ลงทะเบียน** 2 analyzers ใหม่

### New Files

#### `ExecutorBase/Game/AI/AttackOpportunityAnalyzer.cs`
- Analyzer ใหม่: วิเคราะห์ว่าควรตีก่อน combo หรือไม่
- Set `ShouldAttackFirst = true` เมื่อ: MP1 + บอร์ดตรงข้ามว่าง/อ่อน + มี attacker + ไม่ใช่ Turn 1

#### `ExecutorBase/Game/AI/ComboSufficiencyAnalyzer.cs`
- Analyzer ใหม่: วิเคราะห์ว่าบอร์ดดีพอ ควรหยุด extend
- Set `ShouldStopExtending = true` เมื่อ: sufficiency ≥ 60 / Turn 1 + sufficiency ≥ 45 / Going-second + 75%+ damage

---

## Decision Flow Diagram

```
OnSelectIdleCmd (MP1)
│
├─ DynamicLethalCheck() ── lethal ใหม่? → ShouldRushAttack = true
│
├─ ShouldAttackBeforeCombo()? ── บอร์ดตรงข้ามว่าง? → ToBattlePhase → MP2 set/search
│
├─ ShouldRushAttack? ── lethal confirmed → skip non-essential → ToBattlePhase
│
├─ ShouldStopExtending()? ── board ดีพอ? → skip combo → ToBattlePhase/EndPhase
│
├─ ShouldBattleBeforeSetting()? ── set only + has attacker → ToBattlePhase
│
└─ base.OnSelectIdleCmd() ── combo ปกติ
```

## Diagnostic Logs

ทุก decision จะ log ออกมาให้ debug:
- `[DYNAMIC-LETHAL] ✓ UPGRADED to rush mode mid-turn` — จับ lethal กลางเทิร์น
- `[ATTACK-OPPORTUNITY] ✓ Attack before combo | Score: X` — ตัดสินใจตีก่อน
- `[COMBO-STOP] ✓ Board sufficient — stop extending | Sufficiency: X` — หยุด combo
- `[PHASE-GUARD] BLOCKED Type: CardName — reason` — block action + เหตุผล

---

## Backward Compatibility

- ✅ Legacy decks (DefaultExecutor) — **ไม่มีผลกระทบ** (ฟังก์ชันใหม่อยู่ใน ModernExecutor เท่านั้น)
- ✅ 2026 decks (ModernExecutor) — **ได้ผลทันที** ไม่ต้องแก้โค้ดใน deck executor
- ✅ ทุกฟังก์ชันเป็น `protected virtual` — deck executor สามารถ override ได้ถ้าต้องการ
- ✅ Build passed: 0 Errors, 0 new Warnings

---

## v2.1 — Anti-Timidity Patch (2026-06-14)

ปรับสมดุลไม่ให้บอทขี้กลัวเกินไป — เล่นตามสถานการณ์ได้ดีขึ้น

### Changes

| จุดที่ปรับ | ก่อน (ขี้กลัว) | หลัง (สมดุล) |
|-----------|---------------|-------------|
| Backrow penalty (AttackOpportunityScore) | -15/ใบ (3 backrow = ไม่ตี) | **-8/ใบ** + Lethal override ignore backrow |
| Dangerous monster penalty | -40 | **-25** |
| Going-second bonus | ไม่มี | **+20** (กล้าตีกว่าเดิม) |
| Hand trap → BoardSufficiency | +8/ใบ | **+5/ใบ** (hand trap ≠ board ดี) |
| Going-second sufficiency | ใช้ threshold เดียวกับ going-first | **-15 penalty** + -10 ถ้า enemy มี monster |
| Turn 1 combo stop threshold | 45 | **55** (ต้องมี disruption จริงๆ) |
| Going-second combo stop | 60 เหมือน going-first | **75 ถ้า enemy มี monster** (ต้อง push) |
| ShouldAttackBeforeCombo min ATK | ไม่มี (ATK 500 ก็ตี) | **≥1000** + check pending big summon |
| ShouldStopExtending going-second | หยุดเหมือน going-first | **ไม่หยุดถ้า enemy มี monster** |
| ShouldAllowActivate attack-first | Block ResourceGain + Deferrable | **Block เฉพาะ Deferrable** |
| ShouldAllowActivate stop-extending | Block ComboExtender + ResourceGain | **Block เฉพาะ ComboExtender** |
| BoardScorer | ไม่มี Duel reference | **เพิ่ม Duel** สำหรับ turn/phase awareness |
