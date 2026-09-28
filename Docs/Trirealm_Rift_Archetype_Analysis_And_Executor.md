# Trirealm Rift (Sky Thunder Trirealm Rift Yomi) Archetype Analysis & Rule-Based ModernExecutor Report

**Date**: 2026-09-28  
**Architecture**: C# .NET 10.0 | WindBot ModernExecutor + Decoupled Domain Plugin (`TrirealmRiftPlugin.cs`)  
**Deployment Target**: `C:\Users\admin\Documents\EdoGame\`  

---

## 1. Overview of Archetype & Core Mechanics

The **Trirealm Rift (異界の裂け目 / 鳴神の裂界 黄泉)** theme is a modern high-complexity archetype centered around:
1. **Face-Down Banishment (`BANISH_FACEDOWN`) as a Living Resource Pool**:
   - Unlike conventional banish decks that treat banished cards as lost or standard GY extenders, Trirealm Rift actively navigates and recycles face-down banished cards.
   - When Trirealm Rift monsters are summoned, they automatically banish cards from the top of the owner's deck face-down equal to their Level (Lv 1 banishes 1, Lv 2 banishes 2, ..., Lv 7 banishes 7, Lv 8 banishes 8).
2. **6-Attribute Engine & Attribute Matching**:
   - The archetype spans all 6 main attributes:
     - `Gehenna` (Lv 1, DARK)
     - `Sheol` (Lv 2, LIGHT)
     - `Tuonela` (Lv 3, WATER)
     - `Naraka` (Lv 4, FIRE)
     - `Yomi` (Lv 5, WIND)
     - `Helheim` (Lv 6, EARTH)
     - `Ploutonion` (Lv 7, WATER)
     - `Darkness` (Lv 8, DARK)
   - Boss monsters interact based on **matching attributes** between face-down banished Trirealm monsters and opponent targets.
3. **Disruption via Hand & Quick Effects**:
   - **`Sky Thunder Trirealm Rift Yomi` (Lv 5, WIND)**: Hand effect: reveals itself and banishes face-down to search any Trirealm Rift card. Field Quick Effect: negates opponent monster activations by shuffling 1 face-down banished Trirealm Rift monster with the *same attribute* into the deck.
   - **`Trirealm Rift of Scarlet Naraka` (Lv 4, FIRE)**: Quick Effect during opponent's Main Phase to Special Summon Lv 5+ Trirealm Rift monsters (`Yomi`, `Ploutonion`, or `Helheim`) directly from face-down banishment.
   - **`Burial Summit Trirealm Rift Helheim` (Lv 6, EARTH)**: Quick Effect from hand to summon a banished Trirealm monster. On-field Continuous Effect: grants complete targeting and destruction immunity to all other Trirealm Rift cards you control.
   - **`Mad Tempest Trirealm Rift Ploutonion` (Lv 7, WATER)**: In hand: banish itself + 1 Trirealm card face-down to draw 2. On field: Quick Effect spins opponent monster with matching attribute into the deck.
   - **`Trirealm Rift Darkness` (Lv 8, DARK)**: Scales with total face-down banished cards (10+ battle indestructible, 20+ Quick Effect destroy 1 card, 40+ both players skip draw phase).
   - **`Trirealm Rift Territory - Valvols` (Field Spell)**: Continuous resource engine searching face-down banished cards once per turn; shuts down opponent activations in your turn if deck is empty.

---

## 2. Card Roster (11 Archetype Cards)

| ID | Name | Type / Attribute / Level | Primary Role |
| :--- | :--- | :--- | :--- |
| `100458031` | Trirealm Rift Gehenna | Fiend / DARK / Lv 1 | Starter / Banish 1 / Recovers 2 face-down banished Trirealm monsters |
| `100458032` | Trirealm Rift Sheol | Fairy / LIGHT / Lv 2 | Extender / Banish 2 / Recovers 2 face-down banished Trirealm Spells/Traps |
| `100458033` | Trirealm Rift Tuonela | Aqua / WATER / Lv 3 | Board Breaker / Banish 3 / Summons Lv 5+ Trirealm from banishment |
| `100458034` | Trirealm Rift of Scarlet Naraka | Pyro / FIRE / Lv 4 | Disruptor / Banish 4 / Opponent Main Phase Quick Summon Lv 5+ Trirealm |
| `100458035` | Sky Thunder Trirealm Rift Yomi | Thunder / WIND / Lv 5 | Searcher & Omni-Monster Negator (Attribute Shuffle) |
| `100458036` | Burial Summit Trirealm Rift Helheim | Zombie / EARTH / Lv 6 | Quick Hand Summon / Full Targeting & Destruction Immunity for board |
| `100458037` | Mad Tempest Trirealm Rift Ploutonion | Sea Serpent / WATER / Lv 7 | Draw 2 Engine (+ Pot of Desires synergy) / Quick Spin Monster |
| `100458038` | Trirealm Rift Darkness | Fiend / DARK / Lv 8 | Win Condition / Scaling Boss / Draw Lockout |
| `100458039` | Trirealm Rift Territory - Valvols | Field Spell | Continuous Searcher / Banish Face-down / Turn lockdown |
| `100458040` | Trirealm Rift Gospel | Quick-Play Spell | Banish 5 GY/Deck / Extra Normal Summon or Level 4->5+ Transformation |
| `100458041` | Trirealm Rift Judgment | Counter Trap | Omni-Negate & Banish Face-down / Recovers from Face-down |

---

## 3. Architecture & Implementation

### A. Decoupled Domain Plugin: `TrirealmRiftPlugin.cs`
- Inherits from `DeckPluginBase` and implements `IDeckPlugin`.
- **`TrirealmRiftStrategy`**:
  - `AttributeCoverage`: Dynamically calculates available attributes in the face-down banished pool to inform `Yomi` and `Ploutonion` negations and spins.
  - `CanNegateAttribute`: O(1) query determining if an opponent monster's attribute can be negated by Yomi.
  - `FacedownBanishCount`: Real-time tracking of banished pool size to modulate bot playstyle (Early Setup vs Mid-Game Disrupt vs End-Game Lockdown).
- **`TrirealmRiftMaterialEvaluator`**:
  - Implements `GetMaterialCost`, `SortMaterials`, and `PickDestructionSubstitute`.
  - Ensures critical combo pieces like `Yomi` or `Helheim` are not blindly sacrificed for generic Link materials unless lethal damage is guaranteed.
- **`TrirealmRiftThreatEvaluator`**:
  - Integrates with `CardIntelligence.IsFloodgate`, `CardIntelligence.IsHighThreatChokepoint`, and `CardIntelligence.IsKnownNegator`.
  - Prioritizes opponent monster threats based on archetype-specific disruption requirements.

### B. ModernExecutor: `TrirealmRiftExecutor.cs`
- Inherits from `ModernExecutor`.
- **Intelligent Sequencing**:
  1. **Quick-Play & Counter Traps**: `TrirealmRiftJudgment`, `Yomi` Attribute Negate, `Called by the Grave`, `Ash Blossom`, `Impermanence`.
  2. **Field Spell Setup**: `Trirealm Rift Territory - Valvols` activated first to gain continuous card advantage and fuel the face-down banish pool.
  3. **Search & Acceleration**: `Yomi` hand search -> `Ploutonion` draw 2 -> `Gospel` extension.
  4. **Field Swarming & Normal Summons**: `Gehenna` (free summon on empty board) -> `Sheol` (free summon when Trirealm on board) -> `Naraka` (on-field quick trigger setup).
  5. **Extra Deck Climax**: Board breaking through `Accesscode Talker`, `Knightmare Unicorn`, `S:P Little Knight`, and `I:P Masquerena`.
  6. **Smart Fallback Handlers**:
     - `OnSelectCard`: Intelligent prioritization picking matching attributes for `Yomi` negation and highest threat cards for removal.
     - `OnSelectOption`: Automatically selects best options for dual-effect Trirealm cards.

---

## 4. Deck Construction (`TrirealmRift.ydk`)

- **Main Deck (40 cards)**:
  - 3x Sky Thunder Trirealm Rift Yomi
  - 3x Trirealm Rift of Scarlet Naraka
  - 3x Trirealm Rift Gehenna
  - 2x Trirealm Rift Sheol
  - 2x Trirealm Rift Tuonela
  - 2x Burial Summit Trirealm Rift Helheim
  - 2x Mad Tempest Trirealm Rift Ploutonion
  - 1x Trirealm Rift Darkness
  - 3x Trirealm Rift Territory - Valvols
  - 3x Trirealm Rift Gospel
  - 3x Trirealm Rift Judgment
  - 2x Pot of Desires (Massive face-down banish synergy)
  - 3x Ash Blossom & Joyous Spring
  - 2x Called by the Grave
  - 1x Crossout Designator
  - 3x Infinite Impermanence
  - 2x Triple Tactics Talent
- **Extra Deck (15 cards)**:
  - Accesscode Talker, Knightmare Unicorn, Knightmare Phoenix, Knightmare Cerberus, S:P Little Knight, I:P Masquerena, Apollousa Bow of the Goddess, Divine Arsenal AA-ZEUS - Sky Thunder, Number 41: Bagooska, Tornado Dragon, Abyss Dweller, Linkuriboh, Relinquished Anima, Salamangreat Almiraj, Dharc the Dark Charmer Gloomy.

---

## 5. Verification & Deployment Status

- All 11 Trirealm Rift cards synchronized to `cards.cdb` (root, `WindBot\`, and `src\YGO_SOURCE_CLEAN\windbot-fork\`).
- `BUILD_AND_DEPLOY.ps1` executed cleanly (Exit code 0).
- Successfully deployed to `C:\Users\admin\Documents\EdoGame\`:
  - `WindBot\WindBot.dll`
  - `WindBot\ExecutorBase.dll`
  - `WindBot\core.dll`
  - `WindBot\bots.json`
  - `WindBot\Decks\TrirealmRift.ydk`
  - `DashBot.exe`
  - `deck\TrirealmRift.ydk`
