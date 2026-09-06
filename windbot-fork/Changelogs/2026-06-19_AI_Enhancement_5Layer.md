# Changelog — AI Enhancement: 5-Layer Difficulty Upgrade
**Date:** 2026-06-19  
**Version:** Smart Flow v3.0 — Advanced AI Modules  
**Scope:** ExecutorBase (ModernExecutor layer only) — Legacy decks ไม่ได้รับผลกระทบ

---

## สรุปการเปลี่ยนแปลง

เพิ่ม **5 AI Enhancement Modules** ใหม่ เพื่อยกระดับ difficulty ของ bot 2026 โดยไม่ใช้ Reinforcement Learning  
ใช้ **data-driven design** — ข้อมูลการ์ดทั้งหมดอ่านจาก JSON config ไม่ hardcode ใน source code  
ทุก module เป็น **opt-in** — deck executor ต้อง register ถึงจะทำงาน, ไม่ register = behavior เดิม

### ปัญหาที่แก้ไข

| # | Module | ปัญหาเดิม | วิธีแก้ |
|---|--------|-----------|--------|
| 1 | **ComboRouter** | เล่นการ์ดตาม static priority → ผิดลำดับ (NS ก่อน Branded Fusion) | ประเมินมือ → เลือก combo line ที่ end board ดีสุด → เรียงลำดับ |
| 2 | **BaitPlanner** | ยิง combo starter ใบแรก → โดน Ash ตายเทิร์น | ประเมิน hand trap likelihood → เล่น bait card ก่อน → ปกป้อง starter |
| 3 | **ChainTimingAdvisor** | เห็น chain ได้ = chain ทันที → เสียทรัพยากร | Score (0-100) ประเมิน target value + combo stage + resource budget → hold ถ้าไม่คุ้ม |
| 4 | **OpponentProfiler** | ไม่รู้อะไรเลยเกี่ยวกับคู่ต่อสู้ | Track ทุกการ์ดที่เห็น → ระบุ archetype + ประเมิน hand trap + คาดการณ์ play style |
| 5 | **ResourcePlanner** | เล่นทุกอย่างที่เล่นได้ → overcommit → โดน Nibiru/board wipe | Nibiru checkpoint + overextension risk + resource conservation |

---

## ไฟล์ใหม่ที่สร้าง

### AI Modules — `ExecutorBase/Game/AI/`

#### `ComboRouter.cs` — Layer 1: Combo Sequencing Engine
- `ComboLine` class: ลำดับ steps + required cards + end board score
- `RegisterLine()`: deck executor register combo lines ตอน constructor
- `ActivateBestLine()`: ประเมินมือ → เลือก line ที่ดีที่สุด
- `GetNextStep()` / `CompleteCurrentStep()`: track ว่าอยู่ step ไหนของ combo
- `IsPartOfActiveCombo()`: ตรวจว่าการ์ดเป็นส่วนของ combo line ที่กำลังทำอยู่
- `AbortCombo()`: ยกเลิก combo (เช่น ถ้า key card โดน negate)

#### `BaitPlanner.cs` — Layer 2: Hand Trap Bait System
- `RegisterComboStarters()`: บอกว่าการ์ดไหนคือ combo starter ที่ต้องปกป้อง
- `RegisterBaitCards()`: บอกว่าการ์ดไหนเหมาะใช้เป็น bait
- `ShouldBaitFirst()`: ประเมินว่าควร bait ก่อนเล่น combo starter หรือไม่
- `EstimateHandTrapLikelihood()`: คำนวณโอกาสที่คู่ต่อสู้มี hand trap
- `GetBaitCard()`: เลือก bait card ที่ดีที่สุดจากมือ
- Resets ทุก turn ผ่าน `OnNewTurn()`

#### `ChainTimingAdvisor.cs` — Layer 3: Chain Timing Intelligence ⭐ Data-Driven
- `LoadConfig("chain_targets.json")`: โหลด card classification จาก JSON
- `EvaluateChainValue()`: ให้ score 0-100 ว่าควร chain หรือไม่
  - Factor 1: Target card value (combo starter/extender/low-value)
  - Factor 2: Opponent combo stage (early/mid/late summon count)
  - Factor 3: Our resource budget (กี่ hand trap เหลือ)
  - Factor 4: Opponent hand size context
- `ShouldHoldResponse()`: Quick check — score < threshold → hold
- `RegisterHighValueTarget()` / `RegisterLowValueTarget()`: runtime override
- **Heuristic fallback**: ถ้า JSON ไม่มี → ใช้ card type/level heuristics

#### `OpponentProfiler.cs` — Layer 4: Opponent Hand Reading ⭐ Data-Driven
- `LoadConfig("archetypes.json")`: โหลด archetype database จาก JSON
- `OnCardRevealed()` / `OnOpponentSummon()` / `OnOpponentActivate()`: event hooks
- `UpdateArchetypeDetection()`: match ชื่อ + ID กับ archetype database → confidence (0-1)
- `InferPlayStyleFromBoard()`: fallback — ดูจำนวน ED monsters vs traps → Combo/Control
- `EstimateHandTrapsInHand()`: ประเมินจาก archetype density × hand size
- `IsControlDeck()` / `IsComboDeck()` / `IsOTKDeck()`: play style queries
- `GetNextTurnThreatLevel()`: threat forecast (0-10) ดูจาก board + archetype + hand size

#### `ResourcePlanner.cs` — Layer 5: Resource Management
- `NibiruCheckpoint()`: stop summoning เมื่อ summon ≥ 4 + opponent hand ≥ 2 + ไม่มี negate
- `OverextensionRisk()`: score 0-100 ดูจาก monster count + opponent backrow + archetype
- `ShouldStopExtending()`: รวม Nibiru + overextension → ควรหยุดขยายหรือไม่
- `ShouldConserveResources()`: ประเมินว่าชนะอยู่ → ไม่ต้องเล่นเพิ่ม
- `CardEconomyScore()`: ประเมิน cost vs impact ของแต่ละ action

---

### JSON Configs — `configs/`

#### `chain_targets.json`
```json
{
  "ChainThreshold": 45,
  "ComboStarters": [44362883, 16431364, ...],
  "ComboExtenders": [70534340, 55063560, ...],
  "LowValueTargets": [70368879, 67616300, ...],
  "HandTraps": [14558127, 23434538, ...]
}
```
> **แก้ไฟล์นี้เมื่อ meta เปลี่ยน** — เพิ่ม/ลบ card ID ตาม banlist + new releases, ไม่ต้อง recompile

#### `archetypes.json`
```json
{
  "DefaultHandTrapDensity": 0.15,
  "Archetypes": [
    {
      "Name": "Branded",
      "CardKeywords": ["branded", "despia", "albaz"],
      "KnownCardIds": [44362883, 62962630],
      "PlayStyle": "Midrange",
      "HandTrapDensity": 0.15
    },
    ...
  ]
}
```
> **แก้ไฟล์นี้เมื่อ meta เปลี่ยน** — เพิ่ม archetype ใหม่, ปรับ play style / hand trap density

---

## ไฟล์ที่แก้ไข

### `ExecutorBase/Game/AI/ModernExecutor.cs` — 7 จุด

#### § Properties (L54-76)
```diff
+/// <summary>Hand-aware combo line selection engine.</summary>
+protected ComboRouter ComboRouter { get; private set; }
+
+/// <summary>Hand trap bait system.</summary>
+protected BaitPlanner BaitPlanner { get; private set; }
+
+/// <summary>Smart chain response timing.</summary>
+protected ChainTimingAdvisor ChainAdvisor { get; private set; }
+
+/// <summary>Opponent deck/hand inference engine.</summary>
+protected OpponentProfiler OpponentProfile { get; private set; }
+
+/// <summary>Anti-overextension and Nibiru awareness.</summary>
+protected ResourcePlanner ResourcePlan { get; private set; }
```

#### § Constructor
```diff
 protected ModernExecutor(GameAI ai, Duel duel) : base(ai, duel)
 {
+    ComboRouter = new ComboRouter();
+    BaitPlanner = new BaitPlanner();
+    ChainAdvisor = new ChainTimingAdvisor();
+    OpponentProfile = new OpponentProfiler();
+    ResourcePlan = new ResourcePlanner();
+
+    // Load JSON configs (data-driven)
+    string configDir = FindConfigDirectory();
+    if (configDir != null)
+    {
+        ChainAdvisor.LoadConfig(Path.Combine(configDir, "chain_targets.json"));
+        OpponentProfile.LoadConfig(Path.Combine(configDir, "archetypes.json"));
+    }
 }
```

#### § HasNegateOnField() — ใหม่
```csharp
protected virtual bool HasNegateOnField()
{
    foreach (var m in Bot.GetMonsters())
    {
        if (m == null || !m.IsFaceup() || m.IsDisabled()) continue;
        if (_negateMonsters.Contains(m.Id)) return true;
    }
    return false;
}
```

#### § ShouldStopExtending() — เพิ่ม ResourcePlanner integration
```diff
 protected virtual bool ShouldStopExtending()
 {
     // Use Analysis if available
     if (Analysis?.Current?.ShouldStopExtending == true)
         return true;

+    // ═══ ResourcePlanner: Nibiru + overextension check ═══
+    if (ResourcePlan != null && ResourcePlan.Enabled)
+    {
+        bool shouldStop = ResourcePlan.ShouldStopExtending(
+            summonCountThisTurn: Brain?.OwnSummons ?? 0,
+            opponentHandCount: Enemy.Hand.Count,
+            ourMonsterCount: Bot.GetMonsterCount(),
+            opponentBackrowCount: ...,
+            haveNegateOnField: HasNegateOnField(),
+            opponentIsControlDeck: OpponentProfile?.IsControlDeck() ?? false
+        );
+        if (shouldStop) return true;
+    }

     // Fallback heuristic...
 }
```

#### § OnSelectIdleCmd() — เพิ่ม ComboRouter activation
```diff
 public override MainPhaseAction OnSelectIdleCmd(MainPhase main)
 {
     DynamicLethalCheck();

+    // ═══ ComboRouter: Activate best combo line ═══
+    if (ComboRouter?.Enabled == true && Duel.Phase == DuelPhase.Main1)
+        ComboRouter.ActivateBestLine(Bot);

     // Smart Flow v2: Attack Before Combo...
 }
```

#### § Lifecycle Overrides — ใหม่
```csharp
public override void OnNewTurn()
{
    base.OnNewTurn();
    ComboRouter?.OnNewTurn();
    BaitPlanner?.OnNewTurn();
}

public override void OnChaining(int player, ClientCard card)
{
    base.OnChaining(player, card);
    if (player == 1 && card != null)
    {
        OpponentProfile?.OnOpponentActivate(card);
        BaitPlanner?.OnOpponentChainResponse();
    }
}

public override void OnNewPhase()
{
    base.OnNewPhase();
    // Feed opponent field to profiler
    foreach (var m in Enemy.GetMonsters())
        if (m?.IsFaceup() == true)
            OpponentProfile?.OnCardRevealed(m);
}
```

#### § Helper Methods — ใหม่
```csharp
/// Smart hand trap chain — ใช้แทน "always chain"
protected bool SmartHandTrapChain() { ... }
protected bool SmartHandTrapChain(params int[] chokepointIds) { ... }

/// Check if bait needed before playing combo starter
protected ClientCard GetBaitIfNeeded(ClientCard intendedCard) { ... }
```

---

## วิธีใช้ใน Deck Executor

```csharp
public class _2026_BrandedExecutor : ModernExecutor
{
    public _2026_BrandedExecutor(GameAI ai, Duel duel) : base(ai, duel)
    {
        // ── Combo Router: ลำดับเล่นถูก ──
        ComboRouter.RegisterLine(new ComboRouter.ComboLine {
            Name = "BrandedFusion-First",
            RequiredCards = new List<int> { 44362883 },
            Steps = new List<ComboRouter.ComboStep> {
                new() { CardId = 44362883, ActionType = ExecutorType.Activate },
                new() { CardId = 62962630, ActionType = ExecutorType.Summon, Optional = true },
            },
            EndBoardScore = 85
        });
        
        // ── Bait Planner: ล่อ hand trap ──
        BaitPlanner.RegisterComboStarters(44362883);   // Branded Fusion = ต้อง resolve
        BaitPlanner.RegisterBaitCards(62962630);         // Aluber = ล่อได้
        
        // ── Chain Advisor: negate targets ──
        ChainAdvisor.RegisterHighValueTargets(44362883, 16431364);
        
        // ── Hand Traps: ตอบ chain ถูกจังหวะ ──
        AddExecutor(ExecutorType.Activate, 14558127, () => SmartHandTrapChain());
    }
}
```

---

## Build Status

| Project | Status | Notes |
|---------|:------:|-------|
| `ExecutorBase.dll` | ✅ | 0 errors, 3 pre-existing warnings |
| `WindBot.dll` | ✅ | 0 errors |
| `core.dll` | ✅ | 0 errors |
| `training.dll` | ✅ | 0 errors |
| `libWindbot` (Xamarin) | ❌ | Pre-existing — missing Xamarin SDK |

---

## Backward Compatibility

- ✅ **Legacy decks** (DefaultExecutor) — ไม่แตะ `DefaultExecutor.cs` เลย, ไม่มีผลกระทบ
- ✅ **2026 decks ที่ไม่ได้ register** — ทำงานเหมือนเดิม (modules opt-in)
- ✅ **2026 decks ที่ register แล้ว** — ได้ AI ที่ฉลาดขึ้น
- ✅ ทุกฟังก์ชันยังเป็น `virtual` — deck executor override ได้เหมือนเดิม

---

## Decision Flow (Updated v3.0)

```
OnNewTurn()
│  ├─ ComboRouter.OnNewTurn()     ← reset combo state
│  └─ BaitPlanner.OnNewTurn()     ← reset bait state
│
OnSelectIdleCmd (MP1)
│
├─ DynamicLethalCheck()
│
├─ ComboRouter.ActivateBestLine()  ← [NEW] เลือก combo line
│
├─ ShouldAttackBeforeCombo()?
│   └─ บอร์ดว่าง + ไม่มี action ค้าง? → ToBattlePhase
│
├─ ShouldRushAttack?
│   └─ ไม่มี CombatEssential/ComboStarter? → ToBattlePhase
│
├─ ShouldStopExtending()?
│   ├─ Analysis ShouldStopExtending? → stop
│   ├─ ResourcePlanner.NibiruCheckpoint()? → stop  ← [NEW]
│   └─ ResourcePlanner.OverextensionRisk()? → stop ← [NEW]
│
└─ base.OnSelectIdleCmd() → CardExecutor loop
    │
    └─ Hand Trap activation?
        └─ SmartHandTrapChain()          ← [NEW] ประเมิน target value
            ├─ ChainAdvisor.EvaluateChainValue() 
            ├─ score ≥ threshold → CHAIN
            └─ score < threshold → HOLD (เก็บไว้ใช้กับ target ที่ดีกว่า)

OnChaining (opponent activates)
│  ├─ OpponentProfile.OnOpponentActivate()  ← [NEW] track archetype
│  └─ BaitPlanner.OnOpponentChainResponse() ← [NEW] track bait result

OnNewPhase()
│  └─ OpponentProfile.OnCardRevealed()      ← [NEW] scan field
```
