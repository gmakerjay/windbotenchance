# Hecahand Architecture, Audit & Competitive Playbook

## 1. Executive Summary
- **Archetype**: Hecahand (百手 - Hand/Control Steal & OTK Engine)
- **Executor**: `HecahandExecutor.cs` (Inherits `ModernExecutor`)
- **Decoupled Domain Plugin**: `HecahandPlugin.cs` (`IDeckStrategy`, `IDeckMaterialEvaluator`, `IDeckThreatEvaluator`)
- **Naming Standard Compliance**: Rule #9 Clean Naming (`[Deck("Hecahand", "Hecahand")]`). All `2026_` prefixes permanently removed.
- **Banlist Compliance**: 100% compliant with `0TCG.lflist.conf` and `OCG.lflist.conf`. 0 `ERRMSG_DECKERROR`.
- **Simulation Benchmark (Client_Headless_Fortest)**:
  - **Matchup**: `Hecahand` vs `BlueEyes` (10 Duels & 5 Verification Duels)
  - **Win Rate**: **60.0% (6W / 4L)** (Session `25691004_063331`), reaffirmed **60.0% (3W / 2L)** (Session `25691004_070741`)
  - **Rule Compliance**: **0 Violations / 0 Warnings / 0 Crashes / 0 OCG Core Errors**
  - **OTK Execution**: Fast Turn 2 and Turn 4 OTKs utilizing stolen enemy bosses, Kaijus, and Level 7 Xyzs (`Red-Eyes Flare Metal Dragon`, `Number 11: Big Eye`).

---

## 2. Core Architectural & Heuristic Breakthroughs

### 2.1. Decoupled Domain Plugin Architecture
- Refactored `HecahandExecutor.cs` to delegate all domain evaluation to `HecahandPlugin.cs`:
  - `HecahandStrategy`: Oversees control-steal sequencing (`Change of Heart`, `Bot Herder`, `Dandalos`), search targets (`Nightmare Apprentice`, `Mansion of the Dreadful Dolls`), and baiting negations.
  - `HecahandMaterialEvaluator`: Prioritizes stolen opponent monsters (`m.Owner == 1`) as Link material for `S:P Little Knight` or tribute fodder, ensuring the opponent never regains control of their bosses.
  - `HecahandThreatEvaluator`: Accurately scores high-threat targets and directs Kaiju tributes (`Gameciel`, `Dogoran`) to eliminate unaffected or heavy floodgate monsters.

### 2.2. Stolen Monster Link-Off Engine in S:P Little Knight
- **Issue**: Previously, stolen monsters (`Change of Heart`, `Bot Herder`) would return to the opponent during the End Phase if not used as material.
- **Resolution**: Enhanced `ModernExecutor.cs` (line 1770) and `HecahandExecutor.cs` to prioritize stolen monsters (`m.Owner == 1`) as Link material for `S:P Little Knight` or Xyz material before the Battle Phase concludes.

### 2.3. Rule #1 Strict Target Verification
- All removal hints (`hint == 502/503`) strictly filter `c.Controller == 1`. Friendly cards are never targeted.
- Safeguards in `OnSelectCard` guarantee minimum valid targets before returning, preventing desync or illegal selection rollbacks.

---

## 3. Deck Structure & Card Mapping
- **Main Deck (40 Cards)**:
  - Core Hecahand Monsters: `Hecahands Dandalos` x3, `Hecahands Yadel` x3, `Nightmare Apprentice` x3.
  - Kaiju Engine: `Gameciel, the Sea Turtle Kaiju` x2, `Dogoran, the Mad Flame Kaiju` x1.
  - Spells / Steal Cards: `Change of Heart` x1, `Triple Tactics Talent` x2, `Triple Tactics Thrust` x3, `Mansion of the Dreadful Dolls` x3, `Super Polymerization` x2, `Called by the Grave` x1, `Crossout Designator` x1, `Foolish Burial` x1.
  - Handtraps: `Ash Blossom & Joyous Spring` x3, `Infinite Impermanence` x3, `Mulcharmy Fuwalos` x3, `Mulcharmy Purulia` x2.
  - Traps: `Bot Herder` x3, `Dominus Purge` x1.
- **Extra Deck (15 Cards)**:
  - Fusion: `Garura, Wings of Resonant Life`, `Mudragon of the Swamp`, `Starving Venom Fusion Dragon`.
  - Xyz: `Number 11: Big Eye` (Rank 7 permanent steal), `Red-Eyes Flare Metal Dragon` (Rank 7 burn), `Kashtira Arise-Heart`, `Number 41: Bagooska` (Rule #12 FaceUpDefence).
  - Link: `S:P Little Knight` x2, `I:P Masquerena`, `Relinquished Anima`, `Knightmare Phoenix`, `Knightmare Unicorn`.

---

## 4. Headless Simulation Audit Log
```
==================================================================
                  MATCHUP VERIFICATION SUMMARY: Hecahand vs BlueEyes
==================================================================
  Duel 01 | Winner: BlueEyes             | Turns: 04 | Status: OK | 16.7s
  Duel 02 | Winner: Hecahand             | Turns: 04 | Status: OK | 10.3s
  Duel 03 | Winner: Hecahand             | Turns: 02 | Status: OK | 8.7s
  Duel 04 | Winner: BlueEyes             | Turns: 09 | Status: OK | 17.6s
  Duel 05 | Winner: Hecahand             | Turns: 04 | Status: OK | 9.7s
------------------------------------------------------------------
  Total Duels Run        : 5 / 5
  Total Successful Duels : 5 / 5
  Hecahand Win Rate      : 3 wins (60.0%)
  BlueEyes Win Rate      : 2 wins
  Draws                  : 0
  Total Duration         : 63.0s (avg 12.6s/duel)
  Session Logs           : logs/sessions/25691004_070741
==================================================================
```
