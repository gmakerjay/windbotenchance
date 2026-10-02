# 2026_SixSamurai AI Executor & Decoupled Deck Plugin Report (v11.0 Audited)

## 1. Executive Summary
- **Deck Name**: `2026_SixSamurai` (Master Duel / ROTA Modern Wave)
- **Deck Files**: `windbot-fork/Decks/2026_SixSamurai.ydk`, `deck/2026_SixSamurai.ydk`
- **Executor**: `windbot-fork/Game/AI/Decks/_2026_SixSamuraiExecutor.cs`
- **Architecture**: `ModernExecutor` with **5-Layer Decoupled Deck Plugin Architecture (v11.0)**
  - `SixSamuraiPlugin` coordinating 7 specialized domain helpers:
    1. `SixSamStrategy` (Phased Going 1st/2nd Strategy & Special Summon Target Selection)
    2. `SixSamCounterEconomy` (Bushido Counter Optimization & Loop Cutoff)
    3. `SixSamKizaruResolver` (Audited `script/c6579928.lua` Dynamic Attribute-Aware Search)
    4. `SixSamMaterialScorer` (Ace Monster Preservation & Fodder Cost Ranking)
    5. `SixSamActionScorer` (Candidate Action Scoring & Value Evaluation)
    6. `SixSamRecoveryPlanner` (Interrupt Recovery & Branch Switching)
    7. `SixSamBoardAssessor` (Opponent Threat & Lethal OTK Window Evaluation)
- **Build & Deploy Status**: Compiled and published successfully via `BUILD_AND_DEPLOY.ps1` with 0 Errors, deployed exclusively to `C:\Users\admin\Documents\EdoGame\`.

---

## 2. Card & Script Audit Table (40 Main + 15 Extra)
| Card Name | Attribute / Level / Type | Role & Lua Script Verification (`script/`) |
|---|---|---|
| **Legendary Six Samurai - Kizan** (x3) | EARTH / Lv4 / Non-Tuner | Extender, no OPT limit on SS. Prime counter generator. |
| **Legendary Six Samurai - Kageki** (x3) | WIND / Lv3 / Non-Tuner | Prime Starter & Bait. SS Lv4 or lower Six Sam on Normal Summon. |
| **Tactical Trainer of the Six Samurai** (x3) | EARTH / Lv2 / Tuner | Free SS; when sent to GY searches "Six Strike" (Double Assault). |
| **Anarchist Monk of the Six Samurai** (x3) | DARK / Lv3 / Tuner | Free SS; when sent to GY searches Quick-Play Six Sam spell. |
| **Secret Six Samurai - Fuma** (x1) | WIND / Lv1 / Tuner | Floats on destroy; Banish from GY for destruction protection. |
| **Secret Six Samurai - Hatsume** (x1) | WATER / Lv3 / Non-Tuner | Banish 2 Six Sam from GY/field to revive 1 Six Sam. |
| **Secret Six Samurai - Kizaru** (x1) | EARTH / Lv4 / Non-Tuner | On SS: Searches Six Sam with Attribute NOT on field (`c6579928.lua`). |
| **Grandmaster of the Six Samurai** (x1) | EARTH / Lv5 / Non-Tuner | Free SS from hand if control Six Sam (max 1 on field). |
| **Great Shogun Shien** (x1) | FIRE / Lv7 / Non-Tuner | Free SS if control 2+ Six Sam; Limits opponent to 1 S/T per turn. |
| **Legendary Six Samurai - Mizuho & Shinai** (x1 ea) | FIRE & WATER / Lv3 | Shinai salvages on tribute; Mizuho tributes Six Sam to pop card. |
| **Gateway of the Six** (x1) | Continuous Spell | Places 2 counters per summon; 4 counters: search (`c27970830.lua`). |
| **Shien's Dojo** (x2) | Continuous Spell | Places 1 counter per summon; Send to SS Six Sam from Deck. |
| **Battle Shogun of the Six Samurai** (x2) | EARTH / Link-2 | Discards 1 to search Gateway/Dojo (`c74752631.lua`); Places counters. |
| **Legendary Six Samurai - Shi En** (x2) | DARK / Lv5 Synchro | Negates S/T 1/turn; Self-destruction substitute. |
| **Legendary Lord Six Samurai - Shi En** (x2) | DARK / Lv6 Synchro | On Synchro: Search; Quick Effect: Negate monster effect & destroy. |
| **Legendary Lord Six Samurai - Enishi** (x1) | LIGHT / Lv6 Synchro | Bounces opponent monsters up to banished Six Sam. |
| **Naturia Barkion** (x1) | EARTH / Lv6 Synchro | Tactical Trainer (Lv2 Earth Tuner) + Kizan (Lv4 Earth) = Unlimited Trap negate. |
| **Naturia Beast** (x1) | EARTH / Lv5 Synchro | Earth Tuner + Earth Non-Tuner = Unlimited Spell negate by milling 2. |
| **Apollousa, Bow of the Goddess** (x1) | Link-4 | Multi-Monster effect negations. |

---

## 3. Sub-Helper Architecture & Decision Logic

```
                    ModernExecutor (Central Core)
                                 │
                   ┌─────────────┴─────────────┐
                   ▼                           ▼
              Generic AI                  SixSamuraiPlugin
      (AIContext, BoardScorer,     ┌───────────┼───────────┐
       ThreatAnalyzer, Belief)     │           │           │
                   │               ▼           ▼           ▼
                   │            Strategy    Counter    Kizaru
                   │            Planner     Economy    Resolver
                   │               │           │           │
                   │               ▼           ▼           ▼
                   │            Material    Action     Recovery
                   │            Scorer      Scorer     Planner
                   │               │           │           │
                   └───────────────┼───────────┴───────────┘
                                   ▼
                            DecisionTracer (Real-Time Explanations)
                                   ▼
                            Executor Pipeline
```

### 3.1 `SixSamCounterEconomy`
- **Drain Priority**: Battle Shogun (monsters are vulnerable to removal/material usage) $\to$ Shien's Dojo $\to$ Gateway of the Six (preserved as core continuous asset).
- **Loop Safety Guard**: Caps loop at 20 activations per turn to eliminate infinite freeze / timeout risk.
- **OTK Cutoff**: Halts Gateway searches immediately when lethal damage is secured.

### 3.2 `SixSamKizaruResolver`
- Strictly filters search pool in `OnSelectCard(hint == 506)` against `LOCATION_MZONE` controlled attributes.
- Guarantees 0 game rule violations when resolving Kizaru's effect.

### 3.3 `SixSamMaterialScorer`
- Assigns 10,000 penalty to Ace Boss Monsters (`Legendary Lord Shi En`, `Legendary Shi En`, `Great Shogun Shien`, `Naturia Beast`, `Apollousa`).
- Designates spent low-ATK bodies (`Kageki` 200 ATK, `Shinai`, `Mizuho`, duplicate `Kizan`) as primary Link/Synchro fodder.

### 3.4 `SixSamActionScorer`
- Replaces rigid hardcoded `if-else` blocks with value evaluation.
- Calculates Action Score before taking decisive lines (e.g. Normal Summon Kageki scored 95 as prime bait & starter).

---

## 4. Build & Deployment Verification
- **Compilation Tool**: `src\YGO_SOURCE_CLEAN\BUILD_AND_DEPLOY.ps1`
- **Verification Result**: 0 Errors, 0 Violations.
- **Target Location**: `C:\Users\admin\Documents\EdoGame\`
  - `WindBot.dll`, `ExecutorBase.dll`, `core.dll`, `bots.json`, `deck/2026_SixSamurai.ydk`, `DashBot.exe`

---

## 5. Post-Match Duel Audit (Six Samurai vs Dark Magician) & Fixes Applied

### 5.1 Verification of Designed Architecture in Live Play
- **MaterialScorer Validation**: In Turn 1 of both duels, the AI reliably selected expendable fodder (`Tactical Trainer` + `Anarchist Monk`) to Link Summon `Battle Shogun`, protecting higher-value cards and triggering Graveyard floaters.
- **Trigger Chain Routing**: `Battle Shogun` link search and `Anarchist Monk` GY search triggered simultaneously in chain order, successfully searching `Gateway of the Six` and `Cunning of the Six Samurai`.
- **Resource Reutilization**: AI placed `Gateway of the Six` onto the field and used `Cunning` to tribute `Battle Shogun` and revive `Monk`.
- **PhaseGuard & Disruption**: `Crossout Designator` and `Called by the Grave` were properly set for the opponent's turn, and `Infinite Impermanence` interrupted `Apprentice Illusion Magician`.

### 5.2 Root Causes of Bottlenecks & Fixes Implemented
1. **Gateway Multi-Effect Discrimination**:
   - *Problem*: `CardId.GatewayOfTheSix` has 3 ignition effects. Previously, any 2 counters on field triggered `GatewayEffect`, which activated Effect 0 (+500 ATK) twice instead of waiting for 4 counters for Effect 1 (Search).
   - *Fix*: Added `ActivateDescription` routing:
     - `descSearch` (Stringid 1): Only activated when 4+ Bushido counters are available via `EvaluateGatewaySearch()`.
     - `descAtk` (Stringid 0): Blocked during Main Phase 1 setup to ensure counters are preserved for searching.
     - `descRevive` (Stringid 2): Only activated when valid Shien targets exist in GY.
2. **SelectCounters Safety Fallback**:
   - *Problem*: `MSG_RETRY` occurred on Turn 3 when counters did not match requested quantity.
   - *Fix*: Added Priority 4 safe residual drain guaranteeing `sum(used) == quantity` across available cards with counters.
3. **HeuristicGuard Self-Negate False Positive**:
   - *Problem*: `HINTMSG_FACEUP` (514) was included in Rule 1 Self-Negate check, flagging friendly ATK buffs as violations.
   - *Fix*: Removed `HINTMSG_FACEUP` from `HeuristicGuard.ValidateSelection`.
4. **DashBot UI "Open Logs" Button**:
   - Added `BtnOpenLogs` to `MainWindow.xaml` and `MainWindow.xaml.cs` for instant 1-click access to snapshot and duel log directories in File Explorer.
