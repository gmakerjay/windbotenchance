# 📜 Changelog - YugiohTH WindBot Executors & Central AI Engine

---

## [2026.09.05] — Central Core AI Refactoring & Universal Engine Upgrades

### 🎯 Overview & Context
- **Target Subsystem**: Central AI Engine (`ExecutorBase`, `GameAI`, `Executor`, `DefaultExecutor`, `ModernExecutor`)
- **Key Modules**:
  - `src/YGO_SOURCE_CLEAN/windbot-fork/ExecutorBase/Game/AI/CardIntelligence.cs` (New)
  - `src/YGO_SOURCE_CLEAN/windbot-fork/ExecutorBase/Game/AI/Executor.cs`
  - `src/YGO_SOURCE_CLEAN/windbot-fork/ExecutorBase/Game/GameAI.cs`
  - `src/YGO_SOURCE_CLEAN/windbot-fork/ExecutorBase/Game/AI/DefaultExecutor.cs`
  - `src/YGO_SOURCE_CLEAN/windbot-fork/ExecutorBase/Game/AI/ModernExecutor.cs`
  - `src/YGO_SOURCE_CLEAN/windbot-fork/ExecutorBase/AntiFloodgateHelper.cs`
- **Objective**: Eliminate blind `cards[0]` selection, solve suicide attack positioning, prevent Impermanence column hazards, consolidate threat lists, and fix historical agent card ID errors.

### 🐛 Critical Fixes & Enhancements
1. **Universal Fallback Engine (`FallbackSelectCard`)**:
   - Replaced blind `cards[0]` iteration in `GameAI.OnSelectCard` with dynamic threat and cost heuristics.
   - Evaluates opponent threats (ThreatScore), protects starter cards in hand, and prioritizes tokens/fodder for costs.
2. **Stat-Aware Monster Defense (`OnSelectPosition`)**:
   - Enhanced `Executor.OnSelectPosition` and `DefaultExecutor.OnSelectPosition` to calculate Stat Delta (`Defense - Attack`).
   - Weak monsters (e.g. handtraps, low-ATK combo starters with high DEF) now automatically summon in **FaceUpDefence** to avoid free damage.
3. **Column Safety & Anti-Impermanence (`OnSelectPlace`)**:
   - Scans field columns before card placement, avoiding columns with active continuous spells/traps or set cards that could trigger *Infinite Impermanence*.
4. **Centralized Card Intelligence & Agent Bug Fixes (`CardIntelligence.cs`)**:
   - Unified floodgates, negators, chokepoints, handtraps, and target immunities into $O(1)$ HashSets.
   - Fixed historical inverted IDs in `AntiFloodgateHelper.cs` (e.g., ID 85359414 *Bagooska* mislabeled as *Winda*; ID 14212200 *Amano-Iwato* mislabeled as *Kristya*).
5. **Full Hint Table Expansion**:
   - Implemented handlers for `HINTMSG_SPSUMMON (509)`, `HINTMSG_RTOHAND (505)`, `HINTMSG_TODECK (506)`, `HINTMSG_EQUIP (507)`, `HINTMSG_POSCHANGE (518)`, `HINTMSG_DISABLE (552)`, and `HINTMSG_NEGATE (572)`.

### 📊 Verification Benchmark (Headless Simulator)
- `2026_Branded` vs `BlueEyes`: 60.0% Win Rate (3-2), 0 Violations, 0 Crashes.
- `2026_Branded` vs `DarkMagician`: 60.0% Win Rate (3-2), 0 Violations, 0 Crashes.
- `ABC` vs `Altergeist` (Legacy): 100% Stability, 0 Violations, 0 Crashes.

---

## [2026.09.05] — 2026_Branded ModernExecutor Overhaul

### 🎯 Overview & Context
- **Deck Target**: `2026_Branded` (Branded Despia Dogmatika Bystial)
- **Files Modified**: `_2026_BrandedExecutor.cs`, `2026_Branded.ydk`
- **Objective**: Fix incorrect card ruling implementations, eliminate phantom cards, and achieve high win rate against standard legacy decks.

### 🐛 Critical Bug Fixes
1. **Phantom Card Elimination**: Removed phantom references (`Branded Opening`, `Dogmatika Ecclesia`, `Bystial Saronir`) absent from `.ydk`.
2. **Springans Kitt (19304410)**: Corrected graveyard trigger to revive Fallen of Albaz or Albaz-mentioning monsters.
3. **The Fallen & The Virtuous (30271097)**: Fixed conditional logic blocking Option 0 (Destruction) and optimized Turn 1 revival.
4. **Branded in High Spirits (29948294)**: Fixed exact Type matching for Extra Deck dumps (Spellcaster → Granguignol, Dragon → Dogma Dragon/Albion).
5. **Incredible Ecclesia (55273560)**: Prevented wasteful Turn 1 self-tribute when opponent controls no monsters.
6. **Threat Backrow Priority**: Explicitly targeted `Eternal Soul` and `Dark Magical Circle` to wipe opponent boards.

### 📊 Verification Benchmark
- **Overall Win Rate**: **75.0%** (15 wins / 20 duels) across BlueEyes (100%), ABC (80%), DarkMagician (60%), and Altergeist (60%).
- **Rule Violations**: **0**, **Crashes**: **0**.

---

## [2026.09.05] — Systemic ModernExecutor Architecture Upgrades

### 🎯 Overview & Context
- **Base Classes**: `ModernExecutor.cs`, `ChainTimingAdvisor.cs`, `ComboRouter.cs`
- **Enhancements**:
  1. **Smart Material Preservation**: Overrode `OnSelectFusionMaterial`, `OnSelectLinkMaterial`, and `OnSelectXyzMaterial` to prevent sacrificing active field bosses for low-tier extenders.
  2. **Built-in Chokepoints**: Registered ~60 meta and legacy starters into `ChainTimingAdvisor` (+35 priority boost for instant handtrap firing).
  3. **Target & Destruction Guards**: Dynamic detection of untargetable (`Dragoon`, `Avramax`, `Chaos MAX`) and indestructible cards.
  4. **Dynamic Lethal Rush**: Battle phase skip logic when opponent field is empty and lethal damage is guaranteed.
  5. **Alt-Art Alias Compatibility**: Integrated `GetNonAltartCode()` into combo step matching.

---

## [2026.09.05] — Critical Hotfix: Crossout Designator & AnnounceCard Alt-Art Crash Fix

### 🎯 Overview & Root Cause
- **Problem**: Calling `Crossout Designator (65681983)` on alternate art cards (e.g. `Ash Blossom 14558128`) caused OCGCore `MSG_RETRY` rejection, triggering connection termination and EDOPro popups.
- **Resolution**:
  - `GameAI.cs`: Added `NamedCard.IsAltartAlias()` canonicalization to return canonical ID (`14558127`).
  - Dynamic deck search: Replaced hardcoded Blue-Eyes fallback with `GetRemainingCount(id) > 0` validation.
  - `ABCExecutor.cs`: Enhanced `CrossOutNegate()` with canonical ID conversion.

---

## [2026.09.03] — Engine Bugfix: `utility.lua` DelayedOperation Crash

### 🎯 Overview & Root Cause
- **Problem**: `[string "utility.lua"]:2890: Attempting to access deleted object` during End Phase when cards like `S:P Little Knight (29301450)` returned from temporary banishment.
- **Resolution**:
  - In `script/utility.lua`: Added `g:KeepAlive()` to prevent OCGCore C++ garbage collector from destroying delayed group objects.
  - Added null safety checks in `get_affected_group(e)` and proper memory reclamation via `g:DeleteGroup()`.
  - Verified across 5 duels (`2026_Runick vs 2026_AFS`): 100% stability, 0 crashes.

---

## [2026.08.29] — Board Breaker Experiment (AFS, Spright, GOD-01, Demise)

### 🎯 Overview & Results
- Tested universal board breakers (`Lightning Storm`, `Dark Ruler No More`) on 4 meta decks across 160 duels.
- **Findings**:
  - `2026_AFS`: Win rate improved (+7.5%) due to flexible archetype space.
  - `2026_Spright` & `Demise`: Win rate decreased due to loss of combo consistency and protection (Crossout / Ritual searchers).
- **Outcome**: Retained board breakers for flexible decks (AFS, GOD-01) and reverted combo-heavy decks (Spright, Demise).

---

## [2026.08.26] — 2026_Dracotail Executor Overhaul & AI Logic Optimization

### 🎯 Overview & Context
- **Deck Target**: `2026_Dracotail` (Rule-Based C# ModernExecutor)
- **Key Fixes**:
  - Fixed Ace cannibalization during Contact Fusion (`Magistus Chorozo`).
  - Fixed friendly-fire targeting on untargetable enemies (`Avramax`).
  - Fixed empty field self-destruction in graveyard triggers (`Dracotail Pan` & `Dracotail Urgula`).
  - Optimized Quick-Play fusion timing (`Dracotail Faimena`).
  - Enhanced Rahu Dracotail +4 card advantage combo line.
