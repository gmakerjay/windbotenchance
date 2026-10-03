# Raioh (Anti-Meta Stun) — Audit & Playbook Report

## 1. Executive Summary

- **Deck Name**: `Raioh` (Aliases: `RaiOh`, legacy support for `2026_RaiOh`)
- **Archetype / Style**: Anti-Meta Stun / Multi-Dimensional Absolute Lockdown
- **Architecture**: Decoupled Domain Plugin (`RaiohPlugin.cs` + `RaiohExecutor.cs` based on `ModernExecutor`)
- **Banlist Compliance**: 100% Legal under both `0TCG.lflist.conf` (2026.09 TCG) and `OCG.lflist.conf` (0 Errors / 0 Violations)
- **Overall Headless Simulation Performance**: **28 Wins / 12 Losses (70.0% Win Rate)** across 40 games against Legacy benchmarks.

---

## 2. Headless Duel Verification Results

| Opponent Deck | Matches | Wins | Losses | Win Rate (%) | Violations | Crashes | Avg Duration |
| :--- | :---: | :---: | :---: | :---: | :---: | :---: | :---: |
| **BlueEyes** | 10 | 9 | 1 | **90.0%** | 0 | 0 | 11.2s |
| **DarkMagician** | 10 | 7 | 3 | **70.0%** | 0 | 0 | 12.4s |
| **Altergeist** | 10 | 6 | 4 | **60.0%** | 0 | 0 | 17.6s |
| **ABC** | 10 | 6 | 4 | **60.0%** | 0 | 0 | 11.5s |
| **Total / Average** | **40** | **28** | **12** | **70.0%** | **0** | **0** | **13.2s** |

---

## 3. Deck Composition & Card Audit (40 Cards Main / 15 Extra)

### Main Deck (40 Cards)
- **Lockdown Beatsticks (11 Cards)**:
  - `Inspector Boarder` (15397015) x3 — 2000 ATK/DEF. Freezes all monster effects (0 activations unless ED monsters exist).
  - `Thunder King Rai-Oh` (71564252) x3 — 1900 ATK. Complete search denial (blocks all add from Deck) + inherent SS negate.
  - `Banisher of the Radiance` (94853057) x3 — 1600 ATK. Continuous Macro Cosmos on legs (banishes all cards).
  - `Barrier Statue of the Heavens` (46145256) x2 — 1000 ATK. Stops all Special Summons of non-LIGHT monsters.
- **Draw & Consistency Engine (8 Cards)**:
  - `Time-Tearing Morganite` (19403423) x3 — Draw 2 cards per turn + conduct TWO Normal Summons per turn. Zero downside in Stun.
  - `Pot of Duality` (98645731) x3 — Excavate 3 add 1 without SS drawback affecting us.
  - `Pot of Extravagance` (49238328) x2 — Banish 6 Extra Deck to Draw 2.
- **Battle Invincibility & Field Control (6 Cards)**:
  - `Moon Mirror Shield` (19508728) x3 — Equip Spell. Monster ATK/DEF becomes opponent's + 100. Infinite GY recursion for 500 LP.
  - `Necrovalley` (47355498) x2 — Total Graveyard interaction lockout.
  - `Dimensional Fissure` (81674782) x1 — Continuous monster banishing.
- **Counter Traps & Floodgates (15 Cards)**:
  - `Solemn Judgment` (41420027) x1 — Omni-negate protecting backrow against Feather Duster / Lightning Storm.
  - `Solemn Strike` (40605147) x3 — Spell Speed 3 monster effect & special summon negate.
  - `Solemn Warning` (84749824) x2 — Negates any summon or summon spell.
  - `Anti-Spell Fragrance` (58921041) x1 — Forces opponent to Set Spells 1 turn before activating (stops spell board breakers).
  - `Destructive Daruma Karma Cannon` (30748475) x3 — Mass face-down flip + removes Links.
  - `Crackdown` (36975314) x2 — Continuous monster theft.
  - `Skill Drain` (82732705) x1 — Negates all on-field monster effects.
  - `Macro Cosmos` (30241314) x1 — Universal banishment.
  - `There Can Be Only One` (24207889) x1 — 1 monster per Type restriction.

### Extra Deck (15 Cards - Pot Fodder & Emergency Utility)
- `Garura, Wings of Resonant Life` (11765832) x3
- `Mudragon of the Swamp` (54757758) x3
- `Super Starslayer TY-PHON - Sky Crisis` (93039339) x2
- `Starving Venom Fusion Dragon` (41209827) x2
- `Predaplant Dragostapelia` (69946549) x2
- `S:P Little Knight` (29301450) x1
- `Knightmare Phoenix` (2857636) x1
- `Knightmare Cerberus` (75452921) x1

---

## 4. Key AI Heuristics & Architectural Optimizations

1. **Smart Counter Trap Targeting via `Duel.SummoningCards`**:
   - Resolved critical legacy bug where bot paid 2000 LP (Solemn Warning) on 0 ATK tuner materials (e.g. White Stone, Sage).
   - Now filters out weak normal summons (`Attack <= 1000 && Level <= 4 && !IsSpecialSummoned`) and reserves counters for true boss monsters.
2. **Proactive Floodgate Activation (`Duel.Player == 1`)**:
   - `Anti-Spell Fragrance`, `Skill Drain`, `Macro Cosmos`, and `There Can Be Only One` flip in opponent's Draw/Standby Phase before ignition effects or spells can activate.
3. **Equip Routing via Hint 518**:
   - Dedicated branch in `OnSelectCard` routes Moon Mirror Shield directly to `Strategy.PickEquipTarget`, prioritizing unequipped monsters (`Barrier Statue > Rai-Oh > Banisher > Boarder`).
4. **Time-Tearing Morganite Double Summoning**:
   - Accurately tracks Morganite resolution and coordinates 2 Normal Summons per turn, putting pairs of complementary floodgates (e.g. Boarder + Rai-Oh) on the board in a single turn.
