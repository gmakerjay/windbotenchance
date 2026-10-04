# Lunalight (Luna) Architecture, Audit & Competitive Playbook

## 1. Executive Summary
- **Archetype**: Lunalight (月光 - Lunalight Fusion & Pendulum OTK Engine)
- **Executor**: `LunaExecutor.cs` (Inherits `ModernExecutor`)
- **Decoupled Domain Plugin**: `LunaPlugin.cs` (`IDeckStrategy`, `IDeckMaterialEvaluator`, `IDeckThreatEvaluator`)
- **Naming Standard Compliance**: Rule #9 Clean Naming (`[Deck("Luna", "Luna")]`, `[Deck("Lunalight", "Luna")]`). All `2026_` prefixes permanently removed.
- **Banlist Compliance**: 100% compliant with `0TCG.lflist.conf` and `OCG.lflist.conf`. 0 `ERRMSG_DECKERROR`.
- **Simulation Benchmark (Client_Headless_Fortest)**:
  - **Matchup**: `Luna` vs `BlueEyes` (10 Duels)
  - **Win Rate**: **60.0% (6W / 4L)** (Session `25691004_070502`)
  - **Rule Compliance**: **0 Violations / 0 Warnings / 0 Crashes / 0 OCG Core Errors**
  - **OTK Execution**: Clean Turn 2 and Turn 4 OTKs utilizing `Leo Dancer` (3500 ATK) and `Liger Dancer` (3800 ATK).

---

## 2. Core Architectural & Heuristic Breakthroughs

### 2.1. Eradication of Dominus Impulse Anti-Synergy
- **Issue**: The original deck list contained `Dominus Impulse` (40366667) x3. When activated from hand as a handtrap, Dominus Impulse imposes a strict condition: *"You cannot activate the effects of LIGHT, EARTH, or WIND monsters for the rest of this duel"*. This paralyzed the entire Lunalight core: `Tiger` (LIGHT), `Wolf` (LIGHT), `Gold Leo` (LIGHT), and `Silver Hound` (LIGHT).
- **Resolution**: Replaced with `Infinite Impermanence` (10045474) x3. Unlocked 100% of Lunalight combo lines and allowed unrestricted Pendulum scale activations and graveyard triggers.

### 2.2. Silver Hound Dual-Effect Separation via ActivateDescription
- **Issue**: `Lunalight Silver Hound` (35763582) possesses two distinct effects in Graveyard:
  - Effect 0 (`sptg`/`spop`): Trigger effect when sent to GY by card effect -> Special Summon 1 Lunalight from Deck.
  - Effect 1 (`negtg`/`negop`): Quick Effect -> Banish Silver Hound + 1 Lunalight Fusion from GY to negate on-field Spell/Trap activation.
  - Previously, without checking `ActivateDescription`, when the Bot activated `Luna Light Perfume` on field, the engine offered Silver Hound's Quick Effect prompt. The executor mistook it for the sent-to-GY trigger, returned `true`, and banished `Panther Dancer` to negate its own `Luna Light Perfume`!
- **Resolution**: Implemented explicit opcode checking:
  - Effect 0: `ActivateDescription == Util.GetStringId(CardId.SilverHound, 0)` -> validates `!_silverHoundGyUsed` and empty field space.
  - Effect 1: `ActivateDescription == Util.GetStringId(CardId.SilverHound, 1)` -> strictly enforces `Duel.LastChainPlayer == 1 && currentChainCard.Controller == 1`. Never negates friendly cards.

### 2.3. Pendulum Zone Preservation Strategy (`OnSelectPlace`)
- **Issue**: Under Master Rule 4 and Master Rule 5, Pendulum Zones share Spell & Trap zones 0 and 4. When the bot activated continuous spells (`Fire Formation - Tenki`, `Lunalight Masquerade`) or set traps, it occupied zones 0 and 4, physically preventing `Tiger` and `Wolf` from being placed into the Pendulum Zones.
- **Resolution**: Overrode `OnSelectPlace` in `LunaExecutor.cs` to force all continuous/normal spells and traps into the middle zones (`0x4` Center, `0x2` Left-Center, `0x8` Right-Center) first. Zones `0x1` (Far Left) and `0x10` (Far Right) remain permanently reserved for `Tiger` and `Wolf`.

### 2.4. Proactive Negation of Opponent Spell Negators (`Hope Harbinger` & `Spirit Dragon`)
- **Issue**: On Turn 2 going second, opponent bosses like `Number 38: Hope Harbinger Dragon Titanic Galaxy` (63767246) and `Blue-Eyes Spirit Dragon` (59822133) would negate `Polymerization` and `Lunalight Fusion`, neutralizing the bot's plays.
- **Resolution**: Implemented proactive `InfiniteImpermanenceEffect` in Main Phase 1 on our turn (`Duel.Player == 0`). Before activating any Fusion spells, the bot detects active enemy floodgates/negators and fires `Infinite Impermanence` directly at them. Hope Harbinger cannot respond to Traps and is neutralized, allowing free fusion execution.

### 2.5. Forbidden Droplet Cost Safeguard
- **Issue**: In Main Phase 1, `Forbidden Droplet` was previously allowed to chain to the bot's own monster activations (e.g. `Gold Leo`), and `base.OnSelectCard` discarded the bot's only `Lunalight Fusion` as cost.
- **Resolution**:
  - `ForbiddenDropletEffect` strictly returns `false` if `Duel.CurrentChain.Count > 0 && Duel.LastChainPlayer == 0`.
  - In `OnSelectCard`, core combo cards (`Polymerization`, `Lunalight Fusion`, `Tiger`, `Wolf`, `Kaleido Chick`) are strictly blacklisted from being sent as cost, preferring already-used `Tenki`, `Masquerade`, or surplus monsters (`Silver Hound`, `Yellow Marten` which trigger upon being sent to GY).

---

## 3. Deck Structure & Card Mapping
- **Main Deck (40 Cards)**:
  - Core Starters / Extenders: `Kaleido Chick` x3, `Gold Leo` x3, `Silver Hound` x3, `Yellow Marten` x2, `Emerald Bird` x2, `Black Sheep` x2, `Tiger` x3, `Wolf` x2, `Tri-Brigade Fraktall` x1.
  - Spells: `Lunalight Fusion` x3, `Polymerization` x2, `Luna Light Perfume` x3, `Fire Formation - Tenki` x3, `Lunalight Masquerade` x2, `Apex Polymerization` x1, `Foolish Burial` x1, `Triple Tactics Talent` x1, `Called by the Grave` x1, `Crossout Designator` x1.
  - Handtraps & Board Breakers: `Infinite Impermanence` x3, `Forbidden Droplet` x2, `Ash Blossom & Joyous Spring` x3, `Mulcharmy Fuwalos` x2, `Mulcharmy Purulia` x2, `Droll & Lock Bird` x1.
- **Extra Deck (15 Cards)**:
  - Fusions: `Lunalight Leo Dancer` (Lv10, 3500 ATK), `Lunalight Liger Dancer` (Lv11, 3800 ATK), `Lunalight Panther Dancer` (Lv8, 2800 ATK), `Lunalight Perfume Dancer` (Lv6, 2000 ATK) x2, `Lunalight Sabre Dancer` (Lv9, 3000 ATK).
  - Xyz: `Brotherhood of the Fire Fist - Tiger King` (Rank 4, searches Tenki), `Number 41: Bagooska the Terribly Tired Tapir` (Rank 4, Turn 1 floodgate, Rule #12 FaceUpDefence), `Number 60: Dugares the Timeless`, `Chronomaly N'tp's / Nyarla`.
  - Links: `S:P Little Knight`, `Salamangreat Almiraj`, `Gravity Controller`.

---

## 4. Headless Simulation Audit Log
```
==================================================================
                  MATCHUP VERIFICATION SUMMARY: Luna vs BlueEyes
==================================================================
  Duel 01 | Winner: Luna                 | Turns: 04 | Status: OK | 11.2s
  Duel 02 | Winner: Luna                 | Turns: 02 | Status: OK | 13.6s
  Duel 03 | Winner: BlueEyes             | Turns: 07 | Status: OK | 17.3s
  Duel 04 | Winner: BlueEyes             | Turns: 03 | Status: OK | 9.9s
  Duel 05 | Winner: Luna                 | Turns: 07 | Status: OK | 17.6s
  Duel 06 | Winner: Luna                 | Turns: 04 | Status: OK | 12.1s
  Duel 07 | Winner: BlueEyes             | Turns: 04 | Status: OK | 13.3s
  Duel 08 | Winner: BlueEyes             | Turns: 04 | Status: OK | 9.7s
  Duel 09 | Winner: Luna                 | Turns: 18 | Status: OK | 33.4s
  Duel 10 | Winner: Luna                 | Turns: 02 | Status: OK | 11.1s
------------------------------------------------------------------
  Total Duels Run        : 10 / 10
  Total Successful Duels : 10 / 10
  Luna Win Rate          : 6 wins (60.0%)
  BlueEyes Win Rate      : 4 wins
  Draws                  : 0
  Total Duration         : 149.2s (avg 14.9s/duel)
  Session Logs           : logs/sessions/25691004_070502
==================================================================
```
