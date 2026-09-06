# Changelog — Core Engine Audit: Anti-Stupid Bot Patch
**Date:** 2026-06-15  
**Version:** Smart Flow v2.2 — Core Audit  
**Scope:** ExecutorBase ส่วนกลาง (ModernExecutor, BoardScorer, GameAI) — ทุก 2026 deck ได้ผลทันที

---

## สรุปการเปลี่ยนแปลง

Re-audit ระบบ Core ทั้งหมด แก้ **9 จุดที่ทำให้บอทเล่นโง่** — ทั้งจากระบบ Smart Flow v2 ที่เข้มเกินไป และจาก logic ดั้งเดิมที่มีปัญหา

### ปัญหาที่แก้ไข

| # | ระดับ | ปัญหา | สาเหตุ | วิธีแก้ |
|---|-------|-------|--------|--------|
| 1 | 🔴 CRITICAL | บอทข้าม combo ไปตีทันที (มี combo starter ในมือแต่ไม่ใช้) | `ShouldAttackBeforeCombo()` bypass CardExecutor loop ทั้งหมด | เพิ่มเช็ค: ไม่มี Summon/SS/ComboStarter ค้าง ถึงจะตี |
| 2 | 🔴 CRITICAL | บอทตรวจ lethal ผิด → ข้ามคอมโบทั้งหมด → ตีไม่เข้า | Face-down DEF default = 0, Rush mode block ทุกอย่าง | Face-down DEF default = 2000, Rush allow ComboStarter+ResourceGain |
| 3 | 🔴 CRITICAL | บอทไม่ยอม summon boss (Accesscode, Link-3, etc.) | เทียบ base ATK ของ ED monster กับ total field ATK (ผิด) | ลบ ATK downgrade check ออก เหลือแค่ lethal guard |
| 4 | 🔴 CRITICAL | บอทหยุด combo ทั้งที่ยังมี Normal Summon/SS ค้าง | `ShouldStopExtending` เช็คแค่ activation ไม่เช็ค summon | เพิ่มเช็ค pending SummonableCards + SpSummonableCards |
| 5 | 🔴 CRITICAL | บอทใช้ Dark Hole/Raigeki จากมือไม่ได้ตอน Rush mode | Hand spell ทั้งหมดถูก classify เป็น ResourceGain | Normal/QuickPlay spell จากมือ = ComboStarter |
| 6 | 🟠 MAJOR | ตีจากตัวเล็กก่อน (สนามว่าง) → โดน trap 1 ใบเสียหมด | Loop สลับทิศ (attackers.Count-1 → 0) | เปลี่ยนเป็น loop จาก 0 → Count (ตัวใหญ่ก่อน) |
| 7 | 🟠 MAJOR | บอทปฏิเสธ optional effect ที่มีประโยชน์ (draw, search) | `OnSelectEffectYn` default = false | เปลี่ยน default = true (สอดคล้องกับ `OnSelectYesNo`) |
| 8 | 🟠 MAJOR | บอทคว่ำ Effect monster → พลาด on-summon trigger | `SummonOrSet` ไม่เช็ค CardType.Effect | เพิ่ม `!card.HasType(CardType.Effect)` → Effect monster ต้อง NS เสมอ |
| 9 | 🟡 MINOR | Crash ใน `AttackOpportunityScore()` กรณี empty field | `.Max()` บน empty LINQ sequence | เพิ่ม `.DefaultIfEmpty(0)` |

---

## ไฟล์ที่เปลี่ยนแปลง

### `ExecutorBase/Game/AI/ModernExecutor.cs` — 5 จุด

#### `OnSelectIdleCmd()` (L704-755)
```diff
 // ═══ Smart Flow v2: Attack Before Combo ═══
-if (ShouldAttackBeforeCombo(main))
-    return new MainPhaseAction(MainPhaseAction.MainAction.ToBattlePhase);
+// [FIX CRITICAL-1] ตีก่อนได้ เฉพาะเมื่อไม่มี action ที่มีค่าเหลือ
+if (ShouldAttackBeforeCombo(main)
+    && main.SummonableCards.Count == 0
+    && main.SpecialSummonableCards.Count == 0
+    && !main.ActivableCards.Any(c => ... <= ActionPriority.ComboStarter))
+    return new MainPhaseAction(MainPhaseAction.MainAction.ToBattlePhase);
```

```diff
 // ═══ Lethal Rush ═══
-bool hasEssentialActivation = main.ActivableCards.Any(c =>
-    c != null && c.IsFaceup() && c.Location == CardLocation.MonsterZone);
+// [FIX CRITICAL-2] ให้ ComboStarter ทำงานได้ตอน Rush (เช่น Dark Hole จากมือ)
+bool hasUsefulActivation = main.ActivableCards.Any(c =>
+    c != null && ClassifyAction(c, ExecutorType.Activate) <= ActionPriority.ComboStarter);
```

```diff
 // ═══ Stop Extending ═══
-bool hasEssential = main.ActivableCards.Any(c =>
-    c != null && ClassifyAction(...) == ActionPriority.CombatEssential);
-if (!hasEssential) → ToBattlePhase
+// [FIX CRITICAL-4] เช็ค summon/SS ด้วย ไม่ใช่แค่ activation
+bool hasEssential = ... <= ActionPriority.ComboStarter;
+bool hasPendingSummons = main.SummonableCards.Count > 0 || main.SpecialSummonableCards.Count > 0;
+if (!hasEssential && !hasPendingSummons) → ToBattlePhase
```

#### `ShouldAllowSpSummon()` (L814-831)
```diff
-// Extra Deck result ATK < current total field ATK → ATK downgrade
-if (enemyMonsters == 0 && card.Attack > 0 && card.Attack < totalFieldATK)
-    return false;  // ← บล็อค boss monster ผิดๆ
+// [FIX CRITICAL-3] ลบ ATK downgrade check ออกทั้งหมด
+// เหตุผล: card.Attack = base ATK (ไม่รวม on-summon effect)
+//         ไม่รู้ว่า material กี่ตัว ไม่ใช่ทั้งสนาม
```

#### `ClassifyAction()` (L568-621)
```diff
 // Hand activation
 if (card.Location == CardLocation.Hand)
-    return ActionPriority.ResourceGain;
+{
+    // [FIX CRITICAL-5] Continuous/Field/Equip = setup → ResourceGain
+    if (card.HasType(CardType.Spell) && (Continuous || Field || Equip))
+        return ActionPriority.ResourceGain;
+    // Normal Spell / Quick-Play / Monster = combo → ComboStarter
+    return ActionPriority.ComboStarter;
+}
```

#### `ShouldAllowActivate()` (L896-957)
```diff
 if (ShouldRushAttack)
 {
-    if (priority == ActionPriority.CombatEssential) return true;
-    // Block everything else
-    return false;
+    // [FIX] ให้ CombatEssential + ComboStarter ผ่าน
+    if (priority == ActionPriority.CombatEssential || priority == ActionPriority.ComboStarter)
+        return true;
+    // Block เฉพาะ Deferrable + ComboExtender
+    if (priority == ActionPriority.Deferrable || priority == ActionPriority.ComboExtender)
+        return false;
+    return true;  // ResourceGain ผ่าน — อาจช่วย kill ได้
 }
```

---

### `ExecutorBase/Game/AI/BoardScorer.cs` — 2 จุด

#### `CalculateMaxDamage()` (L291)
```diff
 if (defender.IsFacedown())
 {
-    int defVal = defender.Data?.Defense ?? 0;   // ← ไม่รู้ DEF = 0 → false lethal
+    int defVal = defender.Data?.Defense ?? 2000; // [FIX] conservative default
     if (attacker.Attack > defVal) isDestroyed = true;
 }
```

#### `AttackOpportunityScore()` (L597)
```diff
 int ourBestATK = _bot.GetMonsters()
     .Where(c => c != null && c.IsFaceup() && c.IsAttack())
-    .Max(c => c.Attack);   // ← crash ถ้าไม่มี monster
+    .Select(c => c.Attack)
+    .DefaultIfEmpty(0)     // [FIX] ป้องกัน crash
+    .Max();
```

---

### `ExecutorBase/Game/GameAI.cs` — 3 จุด

#### `InternalOnSelectBattleCmd()` — Direct Attack Order (L259-266)
```diff
 if (defenders.Count == 0)
 {
-    // Attack with the monster with the lowest attack first
-    for (int i = attackers.Count - 1; i >= 0; --i)
+    // [FIX] Attack with HIGHEST ATK first
+    // ตีตัวใหญ่ก่อน → ถ้าโดน trap ก็ยังทำ damage สูงสุดแล้ว
+    for (int i = 0; i < attackers.Count; ++i)
     {
         ClientCard attacker = attackers[i];
         if (attacker.Attack > 0)
             return Attack(attacker, null);
     }
 }
```

#### `OnSelectEffectYn()` — Default Behavior (L463-476)
```diff
 // เช็ค executor ทั้งหมดก่อน
 foreach (CardExecutor exec in Executor.Executors)
     if (ShouldExecute(exec, card, ExecutorType.Activate, desc))
         return true;
-return false;  // ← ปฏิเสธ optional effect ที่ดี (draw, search)
+// [FIX] Default YES — optional effect ส่วนใหญ่มีประโยชน์
+return true;
```

#### `SummonOrSet` Handler (L582-594)
```diff
 if (ShouldExecute(exec, card, ExecutorType.SummonOrSet))
 {
     if (Executor.Util.IsAllEnemyBetter(true) && ... &&
-        main.MonsterSetableCards.Contains(card))
+        main.MonsterSetableCards.Contains(card) && !card.HasType(CardType.Effect))
+    // [FIX] Effect monster ต้อง NS เสมอ — คว่ำ = พลาด on-summon trigger
     {
         return SetMonster;
     }
     return Summon;
 }
```

---

## Build Status

- ✅ `ExecutorBase.csproj` — **0 Errors** (3 pre-existing warnings)
- ✅ `WindBot.csproj` — **0 Errors** (45 pre-existing warnings, ไม่มี warning ใหม่)

---

## Backward Compatibility

- ✅ Legacy decks (DefaultExecutor) — **MAJOR-1,2,3 มีผล** (อยู่ใน GameAI.cs ซึ่ง global)
- ✅ 2026 decks (ModernExecutor) — **ทุก fix มีผลทันที** ไม่ต้องแก้ deck executor
- ✅ ทุกฟังก์ชันที่แก้ยังเป็น `virtual` — deck executor override ได้เหมือนเดิม
- ⚠️ **MAJOR-2** (`OnSelectEffectYn` default=true) เปลี่ยนพฤติกรรมทุก deck — ถ้า deck ไหนพึ่งพา default=false ต้องไป register executor เฉพาะ

---

## Decision Flow (Updated)

```
OnSelectIdleCmd (MP1)
│
├─ DynamicLethalCheck()
│   └─ lethal ใหม่? → ShouldRushAttack = true
│
├─ ShouldAttackBeforeCombo()?
│   └─ บอร์ดว่าง + ไม่มี Summon/SS/ComboStarter ค้าง? → ToBattlePhase
│
├─ ShouldRushAttack?
│   └─ ไม่มี CombatEssential/ComboStarter? → ToBattlePhase
│
├─ ShouldStopExtending()?
│   └─ board ดีพอ + ไม่มี Summon/SS/ComboStarter? → ToBattlePhase
│
├─ ShouldBattleBeforeSetting()?
│   └─ set only + has attacker → ToBattlePhase
│
└─ base.OnSelectIdleCmd() → CardExecutor loop ปกติ
    │
    ├─ ShouldAllowActivate(card)
    │   ├─ Rush: allow CombatEssential + ComboStarter, block Deferrable + ComboExtender
    │   ├─ AttackFirst: block Deferrable only
    │   └─ StopExtending: block ComboExtender only
    │
    ├─ ShouldAllowSpSummon(card)
    │   └─ Block เฉพาะเมื่อ guaranteed lethal (ไม่มี ATK downgrade check)
    │
    └─ ShouldAllowSpellSet(card)
        └─ Defer to MP2 ถ้า Rush/บอร์ดว่าง
```
