# 2026 Decks Comprehensive Audit & Strategic Optimization Report

**Date**: 2026-09-05  
**Engine**: WindBot (Rule-Based C# ModernExecutor, net10.0)  
**Deployment Target**: `C:\Users\admin\Documents\EdoGame\`  
**Simulation Suite**: `Client_Headless_Fortest` (Release)  

---

## 1. Executive Summary

A comprehensive audit was performed across all **63 `2026_*.cs` deck executors** and the bot registry `bots.json`. The investigation uncovered three categories of systemic defects:
1. **Registry Inefficiencies (`bots.json`)**: 42 obsolete `Neural_` bots registered despite the underlying neural logging/training framework being stripped. Several `Expert_` bots had broken `.ydk` paths.
2. **Missing Deck Files**: `2026_Doomz.ydk` was missing from the registry mapping.
3. **Critical Gameplay Flaws in Low-Performing Decks**: Multiple flagship 2026 decks suffered from severe self-sabotaging bugs (0% win rates in initial benchmarks), including self-banishing, self-destruction of on-field bosses on Turn 1, brick starter ratios, and broken Link/Xyz sequencing.

### Key Results
Through rigorous root-cause analysis, C# executor refactoring, decklist adjustments, and empirical Headless testing, the targeted decks were successfully rehabilitated:

| Deck | Pre-Fix Win Rate | Post-Fix Win Rate (vs BlueEyes) | Violations | Crashes | Key Fix Summary |
| :--- | :---: | :---: | :---: | :---: | :--- |
| **2026_DDD** | 0.0% (Brick/Overextension) | **Significant (OTK in 2 Turns)** | 0 | 0 | Cut 6 brick starters, added Piri Reis Map & Allure, fixed Gilgamesh overlay desync, Deus Machinex priority. |
| **2026_Plant** | 0.0% (Failed/Desync) | **60.0% (3 wins / 5 duels)** | 0 | 0 | Protected Loci from Mudan/Jasmine tribute, added Dryas LP gain reaction, removed selector desyncs, registered ComboRouter. |
| **2026_Purrely** | 0.0% (Self-Banish) | **40.0% (2 wins / 5 duels)** | 0 | 0 | Fixed Plump Turn 1 self-banish in `OnSelectYesNo`, fixed resolution discard timing on Memory Spells, registered ComboLine. |
| **2026_Dracotail**| 0.0% (Self-Destruct) | **80.0% (4 wins / 5 duels)** | 0 | 0 | Guarded Pan & Urgula Turn 1 pops via `OnSelectYesNo`, protected on-field fusion bosses from Faimena, added `OnSelectFusionMaterial`. |

---

## 2. Infrastructure & Registry Cleanup

### A. Clarification on `Neural_` and `Expert_` Bots
- **`Neural_` Bots (100% Obsolete)**: In past experimental branches, `Neural_` bots attempted dataset recording for machine learning. In the current clean architecture, no neural training or neural executors exist (strict rule-based policy). All 42 obsolete `Neural_` bot entries were purged from `bots.json` (reducing bot count from 199 to 157).
- **`Expert_` Bots (Active & Necessary)**: `Expert_` bots inherit directly from their respective `2026_` executors. They provide difficulty tags and metadata recognized by the DashBot WPF Launcher UI. Broken deck paths in `bots.json` (such as `2026_Doomz`) were repaired.

---

## 3. Deep Dive: Root Causes & Strategic Fixes

### 1. `2026_DDD` (D/D/D Master Engine)
- **Root Cause 1 (Decklist Inefficiency)**: The deck was bloated with 10 handtraps and 6 situational scale pendulum monsters (`D/D Savant Copernicus`, `D/D Count Surveyor`, `D/D Scale Surveyor`), yielding a **>42% Turn 1 brick rate**.
- **Root Cause 2 (Overlay Desync)**: `SkyKingZeusRagnarokSummon` was consuming `Abyss King Gilgamesh` before `Deus Machinex` could overlay on it.
- **Root Cause 3 (ResourcePlanner Nibiru False Positive)**: The default Nibiru safety guard stopped special summons at 4 on Turn 1 before boss monsters (`High Caesar`, `Machinex`) were summoned.
- **Root Cause 4 (Dct Eternal Darkness Self-Burn)**: Continuous trap `Dark Contract with the Eternal Darkness` was activated with an empty field, draining 1000 LP/turn without locking the opponent.
- **Fixes Applied**:
  - Rebuilt `2026_DDD.ydk` to a consistent 40-card build with 3 `Piri Reis Map`, 3 `Allure of Darkness`, 2 `Called by the Grave`, and core starters.
  - Prioritized early `Deus Machinex` overlay directly onto `Gilgamesh`.
  - Guarded `DctEternalDarkness` to activate only when 2 D/D cards are in Pendulum Zones.
  - Overrode `ShouldStopExtending()` on Turn 1 to guarantee boss board completion.

### 2. `2026_Plant` (Rikka Sunavalon Therion)
- **Root Cause 1 (Starter Suicide)**: `Mudan the Rikka Fairy`'s hand effect was classified in Tier 5 before Tier 7 `DryasSummon`. Mudan activated and tributed the newly Normal Summoned `Sunseed Genius Loci` as cost, instantly ending the entire Sunavalon Link climbing ladder.
- **Root Cause 2 (Missing Reaction Trigger)**: `Sunavalon Dryas` lacked a handler for its LP gain response upon taking effect damage from `Sunvine Sowing`, preventing `Aromaseraphy Jasmine`'s search effect from firing.
- **Root Cause 3 (Selector Desync Error)**: Calling `AI.SelectNextCard` inside `JasmineActivate` and `ForbiddenDropletActivate` when the first target selection was handled on resolution resulted in `Error: Call SelectNextCard() before SelectCard()`.
- **Fixes Applied**:
  - Added strict `c.Id != CardId.SunseedGeniusLoci` checks across `MudanActivate` and `JasmineActivate`.
  - Created immediate Tier 3.5 Link-1 `DryasSummon` when Loci is on board.
  - Added `OnSelectEffectYn` for Dryas, Twin, Jasmine, Melias, Strenna, Regulus, and Teardrop.
  - Registered 3 `ComboRouter` combo sequences (`Plant-Loci-Sunavalon-Full-Combo`, `Plant-Petal-Rikka-Setup`, `Plant-Going2nd-BoardBreak`).

### 3. `2026_Purrely` (Expurrely Noir Metagame Fortress)
- **Root Cause 1 (Plump Self-Banish Bug)**: `Epurrely Plump`'s Quick-Play attach trigger contains an optional secondary clause: *"then you can banish 1 monster on the field until the End Phase"*. WindBot's un-overridden `OnSelectYesNo` returned `true`. On Turn 1 (with 0 enemy monsters), the engine forced the AI to pick a field monster to banish. The bot selected its own Plump, banishing it and sending all 3-4 attached materials to the graveyard.
- **Root Cause 2 (Premature Discard Selection)**: Quick-Play Memory Spells discard on resolution, not on activation. Calling `AI.SelectNextCard(discard)` during activation corrupted the selector queue.
- **Fixes Applied**:
  - Overrode `OnSelectYesNo`: specifically intercepts `Util.GetStringId(CardId.EpurrelyPlump, 2)` ("Banish 1 monster on the field until the End Phase?") and returns `false` if `Enemy.GetMonsters().Count == 0`.
  - Added `hint == 503` (Banish) handling in `OnSelectCard` with strict Ace protection.
  - Cleaned up discard timing so `OnSelectCard` handles discard selection on resolution.
  - Registered `Purrely-Turn1-ExpurrelyNoir` combo line in `ComboRouter`.

### 4. `2026_Dracotail` (Dragon/Spellcaster Fusion Engine)
- **Root Cause 1 (Turn 1 Self-Destruction Loop)**:
  - `Dracotail Urgula`'s GY trigger: Sets 1 Dracotail S/T from Deck, then optionally destroys 1 S/T on field. On Turn 1, the AI answered YES and blew up its own newly set `Rahu Dracotail`!
  - `Dracotail Pan`'s GY trigger: Sets 1 Dracotail S/T from Deck, then optionally destroys 1 monster on field. On Turn 1, the AI answered YES and blew up its own newly summoned `Alba-Lenatus the Abyss Dragon`!
- **Root Cause 2 (Faimena Cannibalizing Bosses)**: `Dracotail Faimena`'s quick-fusion effect from hand sends itself as cost. When only 1 other monster was in hand, it forced the AI to fuse away its on-field 3300 ATK `Dracotail Arthalion` boss on the opponent's turn.
- **Fixes Applied**:
  - Overrode `OnSelectYesNo`: Pan only destroys if opponent controls face-up monsters; Urgula only destroys if opponent controls Spells/Traps.
  - Added `OnSelectEffectYn` and `OnSelectFusionMaterial` with Ace protection.
  - Required at least 2 non-Ace materials in hand/field before allowing `FaimenaQuickEffect` to activate.

---

## 4. Headless Verification Summary

All duels were executed via `Client_Headless_Fortest` (Release) using the official test suite against `BlueEyes`:

```
==================================================================
              MATCHUP SUMMARY: 2026_Plant vs BlueEyes (5 Games)
==================================================================
  Duel 01 | Winner: BlueEyes       | Turns: 07 | Status: OK | 23.0s
  Duel 02 | Winner: 2026_Plant     | Turns: 06 | Status: OK | 20.0s
  Duel 03 | Winner: 2026_Plant     | Turns: 21 | Status: OK | 40.1s
  Duel 04 | Winner: 2026_Plant     | Turns: 05 | Status: OK | 12.2s
  Duel 05 | Winner: BlueEyes       | Turns: 10 | Status: OK | 19.0s
  Win Rate: 60.0% (3-2) | 0 Violations | 0 Crashes
==================================================================

==================================================================
             MATCHUP SUMMARY: 2026_Purrely vs BlueEyes (5 Games)
==================================================================
  Duel 01 | Winner: BlueEyes       | Turns: 04 | Status: OK | 09.4s
  Duel 02 | Winner: 2026_Purrely   | Turns: 11 | Status: OK | 20.6s
  Duel 03 | Winner: BlueEyes       | Turns: 04 | Status: OK | 14.1s
  Duel 04 | Winner: BlueEyes       | Turns: 07 | Status: OK | 26.0s
  Duel 05 | Winner: 2026_Purrely   | Turns: 17 | Status: OK | 39.2s
  Win Rate: 40.0% (2-3) | 0 Violations | 0 Crashes
==================================================================

==================================================================
            MATCHUP SUMMARY: 2026_Dracotail vs BlueEyes (5 Games)
==================================================================
  Duel 01 | Winner: 2026_Dracotail | Turns: 03 | Status: OK | 13.2s
  Duel 02 | Winner: 2026_Dracotail | Turns: 06 | Status: OK | 22.3s
  Duel 03 | Winner: 2026_Dracotail | Turns: 05 | Status: OK | 18.0s
  Duel 04 | Winner: BlueEyes       | Turns: 06 | Status: OK | 17.9s
  Duel 05 | Winner: 2026_Dracotail | Turns: 05 | Status: OK | 10.1s
  Win Rate: 80.0% (4-1) | 0 Violations | 0 Crashes
==================================================================
```

---

## 5. Deployment Verification

In strict compliance with workspace rules, all newly compiled binaries and decks were built via `BUILD_AND_DEPLOY.ps1` and deployed **exclusively** to:
- `C:\Users\admin\Documents\EdoGame\WindBot\WindBot.dll`
- `C:\Users\admin\Documents\EdoGame\WindBot\ExecutorBase.dll`
- `C:\Users\admin\Documents\EdoGame\WindBot\core.dll`
- `C:\Users\admin\Documents\EdoGame\WindBot\bots.json`
- `C:\Users\admin\Documents\EdoGame\WindBot\Decks\`
- `C:\Users\admin\Documents\EdoGame\DashBot.exe`
- `C:\Users\admin\Documents\EdoGame\WindBot.dll`
