# WindBot Enhanced — Progress Log Archive (Historical Records)

> เอกสารรวบรวมประวัติการพัฒนาและบันทึกการปรับปรุงในอดีต (Historical Archives)
> สำหรับบันทึกความคืบหน้าล่าสุด กรุณาดูที่ [PROGRESS.md](../PROGRESS.md)

---

---

## 0. Critical Hotfix: Crossout Designator & AnnounceCard Alt-Art Crash Fix (2026-09-05)

### Root Cause Analysis
- **Problem**: When a bot used `Crossout Designator (65681983)` ("การ์ดชี้ดาบ") in response to an alternate artwork card (such as `Ash Blossom & Joyous Spring 14558128`), EDOPro disconnected with the modal `"ข้อความ เกิดข้อผิดพลาด! ตกลง"`.
- **Engine Trace**:
  1. Opponent activated `Ash Blossom (14558128)`.
  2. Bot activated `Crossout Designator (65681983)` to negate.
  3. In OCGCore (`c65681983.lua`), `Duel.AnnounceCard` verifies that the declared card code exists in the player's deck via `tc:GetCode()`, which strictly returns the canonical code (`14558127`), NOT alternate art aliases (`14558128`).
  4. Bot declared `14558128` instead of `14558127`. OCGCore rejected the declaration and sent `MSG_RETRY`.
  5. `GameBehavior.cs` handled `MSG_RETRY` on `AnnounceCard` by closing the connection (`Connection.Close()`), causing EDOPro to crash with an error popup.
  6. Furthermore, `GameAI.OnAnnounceCard()` fallback hardcoded `89631139` (Blue-Eyes), which caused instant crash if Blue-Eyes wasn't in the bot's deck.

### Changes & Fixes Applied
1. **`GameAI.cs` (`OnAnnounceCard`)**:
   - Added automatic alias canonicalization: If any executor calls `AI.SelectAnnounceID` with an Alt-Art code (e.g. `14558128`), `NamedCard.IsAltartAlias(card.Id, card.Alias)` automatically converts it to the canonical original code (`14558127`).
   - Replaced naive Blue-Eyes fallback with dynamic deck search: Checks `Executor.StartingDeck.Cards` for any card with `GetRemainingCount(id) > 0` and returns its canonical code.
2. **`Executor.cs` (`GetRemainingCount`)**:
   - Enhanced `GetRemainingCount(int cardId)` to resolve alternate art aliases so deck counts accurately match between canonical and alt-art IDs.
3. **`ABCExecutor.cs` (`CrossOutNegate`)**:
   - Updated `CrossOutNegate()` to call `lastChainCard.GetNonAltartCode()` and verify `GetRemainingCount(targetCode) > 0` before announcing.
4. **`GameBehavior.cs`**:
   - Added diagnostic logging for declared card IDs during `OnAnnounceCard`.

---

## 0.1. Systemic ModernExecutor Architecture Upgrades (2026-09-05)

### Overview
Upgraded the core base classes (`ModernExecutor`, `ChainTimingAdvisor`, and `ComboRouter`) so that **all 2026+ decks inheriting from ModernExecutor immediately benefit from tournament-grade decision making** without requiring individual deck rewrites.

### 5 Core Enhancements Implemented
1. **Universal Hint-Based Smart Selection Engine (`OnSelectCard`)**:
   - Replaced naive `cards[0]` default selection with dynamic threat and expendability scoring:
     - **Removal/Banish/Bounce/Negate (Hints 502, 504, 505, 551, 575)**: Targets highest threat cards (Omni-Negates > Floodgates > Key Continuous Spells/Traps like `Eternal Soul`, `Skill Drain`, `Circle` > High ATK bosses). Automatically excludes targeting-immune cards.
     - **Discard/Send to GY/Tribute Cost (Hints 500, 501, 508)**: Protects irreplaceable starters, key negates, and handtraps. Prioritizes tokens, normal monsters, GY-triggers, and duplicate cards in hand.
     - **Special Summon (Hint 509)**: Prioritizes boss monsters with negates, extra deck cards, and highest ATK stats.
2. **Built-in Universal Meta & Legacy Chokepoints in `ChainTimingAdvisor`**:
   - Embedded a comprehensive in-code database of ~60 meta and legacy starters/searchers (Branded, Snake-Eye, Tenpai, ABC, Altergeist, Dark Magician, Blue-Eyes, Labrynth, Spright, Runick, Kashtira, Tearlaments, etc.).
   - Starters and key normal spells now receive a +35 critical target score (+10 normal spell bonus = 95 total), guaranteeing that handtraps (`Ash Blossom`, `Infinite Impermanence`) fire immediately on Turn 1 normal summons instead of being held pointlessly.
3. **Smart Material Preservation System**:
   - Overrode `OnSelectFusionMaterial`, `OnSelectLinkMaterial`, and `OnSelectXyzMaterial` in `ModernExecutor`.
   - Protects active field bosses (`_negateMonsters`, high ATK extra deck monsters) from being sacrificed for low-impact summons when other fodder (tokens, hand/GY materials, small extenders) is available.
   - For enemy-board fusion effects (Super Poly, Fallen of Albaz), gives opponent monsters (`Controller == 1`) top priority (#0).
4. **Target Immunity & Destruction Protection Guard**:
   - Enhanced `IsTargetImmune` to dynamically recognize untargetable bosses (`Dragoon`, `Avramax`, `Chaos MAX`, `The Arrival`, `Ultimate Falcon`, `Underworld Goddess`, `Azure-Eyes`, `Eternal Soul`).
   - Added `IsDestructionImmune` to prevent wasting destruction effects on effect-indestructible targets.
5. **Direct Lethal Rush Mode Optimization**:
   - Enhanced `DynamicLethalCheck` to detect when the opponent has 0 monsters and bot field ATK >= opponent LP.
   - Cuts directly to Battle Phase, deferring backrows and skipping overextension risks to close out duels cleanly.
6. **ComboRouter Alt-Art Alias Support**:
   - Enabled `ComboRouter` and `ModernExecutor` step execution to match cards via `GetNonAltartCode()`, allowing combo lines to execute smoothly regardless of whether the player's deck uses alternate art printings.

---

## 1. 2026_Branded ModernExecutor (Latest Update)

### Overview
- **Deck**: `2026_Branded` (Branded Despia Dogmatika Bystial: Branded Fusion, Aluber the Jester of Despia, Fallen of Albaz, Fallen of the White Dragon, Guiding Quem the Virtuous, Blazing Cartesia the Virtuous, Incredible Ecclesia the Virtuous, Tri-Brigade Springans Kitt, Tri-Brigade Mercourier, The Golden Swordsoul, Albion the Shrouded Dragon, Nadir Servant, Branded in High Spirits, The Fallen & The Virtuous, Super Polymerization, Forbidden Droplet, Mirrorjade the Iceblade Dragon, The Dragon that Devours the Dogma, Despian Luluwalilith, Ecclesia and the Dark Dragon, Granguignol the Dusk Dragon, Lubellion the Searing Dragon, Albion the Branded Dragon, Rindbrumm the Striking Dragon, Garura, Mudragon, Titaniklad)
- **Executor**: `_2026_BrandedExecutor.cs`
- **DashBot Category**: **2026 Decks** (registered as `2026_Branded` and `Expert_2026_Branded` in `bots.json`)
- **Architecture**: `ModernExecutor` (Rule-Based C# 10.0 / .NET 10)
- **Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`

---

### Benchmark Results (Headless Simulator Duels)

| Opponent Deck | Wins / Duels | Win Rate (%) | Rule Violations | Crashes | Notes |
|---|---|---|---|---|---|
| **BlueEyes** | **5 / 5** | **100.0%** | 0 | 0 | 100% Win Rate; Mirrorjade & Dogma Dragon overwhelmed 3000 ATK dragons; avg 12.7s |
| **ABC** | **4 / 5** | **80.0%** | 0 | 0 | Broke Buster Dragon boards via Super Poly + Droplet + non-target banish; Turn 2 & 4 OTKs |
| **DarkMagician** | **3 / 5** | **60.0%** | 0 | 0 | Major breakthrough (from 0% baseline); strictly destroyed `Eternal Soul` & `Circle` |
| **Altergeist** | **3 / 5** | **60.0%** | 0 | 0 | Major breakthrough (from 20% baseline); countered Multifaker/Spoofing/Hexstia |
| **Total** | **15 / 20** | **75.0%** | **0** | **0** | **100% Engine Stability, 0 Errors, 0 Violations across all duels** |

---

### Key Tactical Implementations & Architecture Refinements
1. **Full Card Audit & Phantom Card Elimination**:
   - Analyzed `2026_Branded.ydk` and `cards.cdb` to discover and remove phantom card references (`Branded Opening`, `Dogmatika Ecclesia`, `Bystial Saronir`) that had 0 copies in the deck.
2. **Card Ruling & Mechanics Corrections**:
   - **Tri-Brigade Springans Kitt (19304410)**: Rewrote handler from hand-summon/bottom-deck to its true trigger: activates when sent to GY to revive Albaz or Albaz-mentioning monsters.
   - **The Fallen & The Virtuous (30271097)**: Removed erroneous `controlsAlbaz` condition that blocked Option 0 (Destroy), and implemented Turn 1 Option 1 (Revive) going first.
   - **Branded in High Spirits (29948294)**: Enforced exact type-matching (Spellcaster -> Granguignol; Dragon -> Dogma Dragon/Albion; Winged Beast -> Rindbrumm).
   - **Incredible Ecclesia (55273560)**: Added guard preventing wasteful self-tribute on Turn 1 going first when opponent controls 0 monsters to fuse with.
3. **High-Threat Target Prioritization & Non-Target Mechanics**:
   - Intercepted hints (`502`, `503`, `505`, `551`, `575`) to destroy high-threat backrows in priority order: `Eternal Soul` (wipes opponent board), `Dark Magical Circle`, `Altergeist Protocol`, `Personal Spoofing`, `Altergeist Hexstia`, `Skill Drain`, `Secret Village`.
   - Optimized Mirrorjade banish to recognize non-targeting properties, bypassing opponent targeting immunities.
4. **Reactive Handtrap System & Chokepoint Registration**:
   - Registered 26 high-threat chokepoint IDs into `ChainAdvisor` and implemented immediate negate overrides for Ash Blossom and Mulcharmy against `Multifaker`, `Union Hangar`, `B-Buster Drake`, `Meluseek`, etc.
5. **Multi-Route ComboRouter**:
   - Created 6 dedicated combo routes: `BrandedFusion-Main` (Score 95), `Aluber-To-BrandedFusion` (Score 92), `WhiteDragon-Ecclesia-Synchro` (Score 88), `Quem-Cartesia-Granguignol` (Score 85), `NadirServant-Quem` (Score 82), `GoldSarc-Mercourier` (Score 80).

---

## 2. 2026_DarkTime ModernExecutor

### Overview
- **Deck**: `2026_DarkTime` (Dark Time Retrain Archetype: Swift Panther Warrior, Alligator's Sword Dragon Knight, Fisherman Legend of the Sea, Jinzo - Energy Shocker, Dark Time Wizard, Sleeping Scapegoats, Graceful Skull Dice, Foolish Graverobber, Reversal Box, Fidraulis Harmonia, Golden Cloud Beast - Malong, Enigmaster Packbit, Wind Pegasus @Ignister, Psychic End Punisher, Garura)
- **Executor**: `_2026_DarkTimeExecutor.cs`
- **DashBot Category**: **2026 Decks** (registered as `2026_DarkTime` and `Expert_2026_DarkTime` in `bots.json`)
- **Architecture**: `ModernExecutor` (Rule-Based C# 10.0 / .NET 10)
- **Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`

---

### Benchmark Results (Headless Simulator Duels)

| Opponent Deck | Wins / Duels | Win Rate (%) | Rule Violations | Crashes | Notes |
|---|---|---|---|---|---|
| **ABC** | **2 / 10** | **20.0%** | 0 | 0 | 100% Stability, 0 engine violations; broke Dragon Buster boards via Droplet + Malong bounce |
| **BlueEyes** | **2 / 5** | **40.0%** | 0 | 0 | Duel 05 epic 18-turn endurance victory; Fisherman destroys 3000 ATK dragons |
| **DarkMagician** | **2 / 5** | **40.0%** | 0 | 0 | Turn 5 & Turn 6 OTK wins; Jinzo [ATK 3000] beats down through Eternal Soul / Circle |
| **Altergeist** | **2 / 5** | **40.0%** | 0 | 0 | Turn 10 control victories; Sleeping Scapegoats token wall blocks attacks and sets up Panther |
| **Total** | **8 / 25** | **32.0%** | **0** | **0** | **100% Stability, 0 Errors, 0 Violations across all duels** |

---

### Key Tactical Implementations & Architecture Refinements
1. **Engine Database Card Resolution Fix**:
   - Fixed missing card entries in `EdoGame\cards.cdb` for 2026 custom/retrain cards (`101402001` Swift Panther Warrior, `101402002` Alligator's Sword Dragon Knight, etc.) which previously caused `DeckError (flag=6)` during duel initialization.
   - Synchronized full 17MB database to `EdoGame\cards.cdb` and integrated automated backup/deploy in `BUILD_AND_DEPLOY.ps1`.
2. **Forbidden Droplet Engine Crash Resolution (`MSG_RETRY`)**:
   - Resolved fatal duplicate card selection in `OnSelectCard` for `Forbidden Droplet` when cost candidates intersected across spell and extra hand collections.
   - Implemented strict deduplication (`.Distinct()`) and intelligent target/cost prioritization (preferring Scapegoat tokens and expendable spells).
3. **Card Trigger & Ruling Logic Rewrites**:
   - **Alligator's Sword Dragon Knight**: Replaced invalid `ExecutorType.SpSummon` with `ExecutorType.Activate`; separated in-hand reveal Special Summon from on-summon spell/trap search (adds 2 S/T, discards 1).
   - **Fisherman, Legend of the Sea**: Removed flawed `Location != Hand` filter that previously prevented on-field monster destruction effect from triggering; accurately targets and destroys threatening enemy face-up monsters.
   - **Swift Panther Warrior**: Added Option 0 selection in `OnSelectOption` and smart tribute priority preferring Scapegoat tokens and non-Ace monsters.
   - **Foolish Graverobber**: Removed erroneous `SpellSet` on Normal Spell; fixed GY send and monster steal/revival selection.
   - **Sleeping Scapegoats**: Corrected classification from Trap to Quick-Play Spell; triggers defensively during opponent turns to summon 4 tokens and Panther from Deck.
   - **Dark Time Wizard**: Fixed option selection to choose Option 0 (guaranteed +2 search and recycle) instead of 50% coin-toss suicide gamble.
   - **Harmonia & Synchro GY Synergy**: Added handlers for `Golden Cloud Beast - Malong` (face-up card bounce), `Enigmaster Packbit` (continuous trap monster), and `Wind Pegasus @Ignister` (GY shuffle disruption).

---

## 2. 2026_Runick ModernExecutor

### Overview
- **Deck**: `2026_Runick` (Runick Stun / Mill Control: Runick Fountain, Tip, Freezing Curses, Destruction, Flashing Fire, Slumber, Smiting Storm, Golden Droplet, Hugin, Munin, Geri, Sleipnir, Inspector Boarder, Thunder King Rai-Oh, Dimensional Fissure, Rivalry of Warlords, TCBOO, Messenger of Peace, Super Polymerization)
- **Executor**: `_2026_RunickExecutor.cs`
- **DashBot Category**: **2026 Decks** (registered as `2026_Runick` and `Expert_2026_Runick` in `bots.json`)
- **Architecture**: `ModernExecutor` (Rule-Based C# 10.0 / .NET 10)
- **Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`

---

### Benchmark Results (Headless Simulator Duels — 10 Games/Matchup, 40 Total)

| Opponent Deck | Wins / Duels | Win Rate (%) | Rule Violations | Crashes | Notes |
|---|---|---|---|---|---|
| **ABC** | **2 / 10** | **20.0%** | 0 | 0 | `Dimensional Fissure` lock; Epic 38-turn endurance grind win in Duel 07 |
| **Altergeist** | **3 / 10** | **30.0%** | 0 | 0 | Grinded through `Secret Village` & `Protocol`; 23-turn grind deck-out win in Duel 08 |
| **BlueEyes** | **3 / 10** | **30.0%** | 0 | 0 | Hugin SS + Fountain cycle; `Messenger of Peace` stun; 18-turn mill-out win |
| **DarkMagician** | **2 / 10** | **20.0%** | 0 | 0 | `Super Poly` into `Mudragon`; 25-turn resource battle won in Duel 01 |
| **Total** | **10 / 40** | **25.0%** | **0** | **0** | **100% Stability, 0 Errors, 0 Violations across all 40 matches** |

---

### Key Tactical Implementations & Architecture Refinements
1. **Option Selection (`OnSelectOption`) Bitshift Fix**:
   - Fixed critical protocol bug where option decoding used `option >> 20` instead of `option >> 4`, causing Card ID and option index to be completely corrupted.
   - Added robust fallback to `Card ?? LastChainCard` and `IsRunickSpell()` check.
   - Runick Quick-Plays now correctly select **Option 1 (Special Summon Hugin)** whenever Fountain needs to be searched, when under attack with empty board, or when chaining against backrow wipes (`Harpie's Feather Duster` / `Lightning Storm`).
2. **OnSelectCard (hint 501 & 507) Refinements**:
   - For `hint 501` (Hugin discard cost): Prioritizes duplicate Runick spells or expendable spells, strictly protecting `Runick Fountain` (priority score 900).
   - For `hint 507` (Fountain recycle): Returns up to 3 Runick Quick-Plays to bottom of deck to trigger 3 card draws every turn.
   - For Extra Deck Special Summon: Prioritizes `HuginTheRunickWings` first, then `GeriTheRunickFangs`.
3. **Spell Set & Rai-Oh Stun Discipline**:
   - `SpellSetCheck` sets Runick Quick-Plays face-down only when `Runick Fountain` is NOT on field (allowing activations on opponent's turn), and keeps them in hand when Fountain is active.
   - `ThunderKingRaiOhSummon` avoids deadlocks by summoning Rai-Oh as a 1900 ATK stun beatstick if Fountain is already searched or cannot be searched this turn.

---

## 2. 2026_RyuGe ModernExecutor (Latest Update)

### Overview
- **Deck**: `2026_RyuGe` (Ryu-Ge Archetype: Sosei Ryu-Ge Mistva, Tensei Ryu-Ge Anva, Kyoro Ryu-Ge Kaiva, Genro Ryu-Ge Hakva, Kairo Ryu-Ge Emva, Ryu-Ge War Zone, Ryu-Ge Rising, Dino Domains, Sea Spires, Wyrm Winds, Melody of Awakening Dragon, Diviner of the Herald)
- **Executor**: `_2026_RyuGeExecutor.cs`
- **DashBot Category**: **2026 Decks** (registered as `2026_RyuGe` and `Expert_2026_RyuGe` in `bots.json`)
- **Architecture**: `ModernExecutor` (Rule-Based C# 10.0 / .NET 10)
- **Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`

---

### Benchmark Results (Headless Simulator Duels — 10 Games/Matchup, 40 Total)

| Opponent Deck | Wins / Duels | Win Rate (%) | Rule Violations | Crashes | Notes |
|---|---|---|---|---|---|
| **ABC** | **3 / 10** | **30.0%** | 0 | 0 | Multi-turn board breaks; Turn 4 and Turn 7 OTK/beatdown victories |
| **Altergeist** | **1 / 10** | **10.0%** | 0 | 0 | Broke through `Hexstia` + `Protocol` disruption in Duel 01 (Turn 10 win) |
| **BlueEyes** | **1 / 10** | **10.0%** | 0 | 0 | `Varudras` omni-negate + `Sea Spires` bounce cleared board in Duel 06 (Turn 6 win) |
| **DarkMagician** | **2 / 10** | **20.0%** | 0 | 0 | `Mistva` Extra Deck Special Summon triggered on battle destroy, direct attacked for game in Duel 10 |
| **Total** | **7 / 40** | **17.5%** | **0** | **0** | **100% Stability, 0 Errors, 0 Violations across all 40 matches (jumped from 0%)** |

---

### Key Tactical Implementations & Architecture Refinements
1. **Mistva Script Revive Limit Fix (`c92487128.lua`)**:
   - Fixed fundamental ocgcore engine issue where `c:EnableReviveLimit()` was called unconditionally on Ritual Pendulum Monster `Mistva`, blocking ocgcore from allowing Mistva to be activated as a Pendulum Spell from hand.
   - Updated with conditional revive limit `not e:GetHandler():IsLocation(LOCATION_HAND)` matching `Odd-Eyes Pendulumgraph Dragon`.
2. **Turn 1 Self-Destruction Elimination**:
   - `AnvaSummonOrEffect`: On Special Summon, only activates optional on-summon destroy effect if `Enemy.GetFieldCount() > 0`. Fixed severe bug where Anva blew itself up on Turn 1 when no enemy cards existed.
   - `OnSelectYesNo`: Refuses optional field destruction prompts (`Varudras`, `Anva`, `Mistva`) if enemy controls 0 cards, preventing Varudras from destroying friendly `Ryu-Ge War Zone` after negating opponent spells.
3. **Smart Tribute & Discard Priority (`OnSelectCard`)**:
   - `hint == 500` (Tribute): Prioritizes `Kairo Ryu-Ge Emva` (revives from GY) > `Genro Ryu-Ge Hakva` > `Kyoro Ryu-Ge Kaiva` > duplicate `Anva` > `Mistva` (only as last resort). Eliminates Turn 1 deadlock.
   - `hint == 501` (Discard): Strictly protects `Mistva` and `Anva` from accidental discards.
   - `hint == 506` (Search): When `max >= 2` (The Melody of Awakening Dragon), reliably adds 1 `Tensei Ryu-Ge Anva` AND 1 `Sosei Ryu-Ge Mistva`.
4. **Varudras & Boss End-Board Summoning**:
   - `VarudrasSummon`: Overlays 2 Level 10 monsters into `Varudras` on Turn 1 to establish omni-negate end board.
   - `VarudrasEffect`: Reliably negates opponent key cards (e.g. `Trade-In`, `Melody`, `Dragon Shrine`).
5. **Ryu-Ge Rising Integration**:
   - `OnSelectYesNo`: Description 2 (place Pendulum monster from hand to Extra Deck) returns `true` when `Mistva` is in hand, perfectly setting up Mistva's signature Extra Deck revival trigger.

---

## 3. 2026_AFS ModernExecutor

### Overview
- **Deck**: `2026_AFS` (Azamina Fiendsmith Snake-Eye: Silvia, Mu Rcielago, Caesar, Flamberge, Promethean Princess, S:P Little Knight, Diabellstar, Ash, Poplar)
- **Executor**: `_2026_AFSExecutor.cs`
- **DashBot Category**: **2026 Decks** (registered as `2026_AFS` and `Expert_2026_AFS` in `bots.json`)
- **Architecture**: `ModernExecutor` (Rule-Based C# 10.0 / .NET 10)
- **Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`

---

### Benchmark Results (Extended Headless Duels — 10 Games/Matchup, 40 Total)

| Opponent Deck | Wins / Duels | Win Rate (%) | Rule Violations | Crashes | Notes |
|---|---|---|---|---|---|
| **ABC** | 2 / 10 | 20.0% | 0 | 0 | ABC combo speed overwhelms; `Avramax` + `Infinity` end boards hard to break |
| **Altergeist** | 2 / 10 | 20.0% | 0 | 0 | Long grind games (avg 12 turns); `Protocol` + `Hexstia` negate chains |
| **BlueEyes** | 1 / 10 | 10.0% | 0 | 0 | Blue-Eyes Level 8 beatdown outpaces; `Hope Harbinger` blocks spell plays |
| **DarkMagician** | 0 / 10 | 0.0% | 0 | 0 | `Eternal Soul` + `Dark Magical Circle` banish loop; `Dragon Knight` spell protection |
| **Total** | **5 / 40** | **12.5%** | **0** | **0** | **100% Stability, 0 Errors, 0 Violations across all 40 matches** |

---

### Key Tactical Implementations & Architecture Refinements
1. **Triple-Engine Synergy (Azamina + Fiendsmith + Snake-Eye)**:
   - **Snake-Eye Starter Engine**: `Snake-Eye Ash` (`9674034`) / `Bonfire` (`85106525`) searches `Snake-Eyes Poplar` (`90241276`) -> `Poplar` Special Summons itself -> searches `Divine Temple` (`53639887`) or `Original Sinful Spoils` (`89023486`) -> `Ash` sends S/T Poplar to Special Summon `Flamberge Dragon` (`48452496`).
   - **Fiendsmith Engine**: `Fiendsmith Engraver` (`60764609`) / `Fiendsmith's Tract` (`98567237`) searches & discards `Fabled Lurrie` (`97651498`) -> `Lurrie` Special Summons -> Link 1 `Fiendsmith's Requiem` (`2463794`) -> Special Summons `Lacrima the Crimson Tears` (`28803166`) -> Link 2 `Fiendsmith's Sequence` (`49867899`) -> Fusion Summons `Fiendsmith's Lacrima` (`46640168`) -> revives `Engraver` -> Xyz Summons **`D/D/D Wave High King Caesar` (`79559912`) (Double Special Summon Negate)**!
   - **Azamina Engine**: `Deception of the Sinful Spoils` (`66328392`) / `WANTED` (`80845034`) / `The Hallowed Azamina` (`94845588`) -> sends `Sinful Spoils` fodder to Fusion Summon **`Azamina Ilia Silvia` (`46396218`) (Quick Omni-Negate by tributing itself)**.
2. **Promethean Princess & Fire Link OTK**:
   - `Promethean Princess, Bestower of Flames` (`2772337`) revives `Flamberge` / `Ash` / `Poplar` from GY, and provides GY Quick Effect destruction disruption upon opponent special summon.
   - Climbs into `Salamangreat Raging Phoenix` (`57134592`) / `Worldsea Dragon Zealantis` (`45112597`) / `Accesscode Talker` (`86066372`) for 8,000+ direct attack lethal turns.
3. **Multi-Layer End Board (8+ Disruptions)**:
   - `D/D/D Wave High King Caesar` (2x Special Summon Negates) + `Azamina Ilia Silvia` (Omni-Negate) + `Promethean Princess` (GY Quick Destroy) + `S:P Little Knight` / `I:P Masquerena` (Quick Banish) + `Flamberge Dragon` (Opponent turn SS from S/T zone) + `Forbidden Droplet` / `Infinite Impermanence`.
4. **Exclusive Deployment**:
   - Deployed all updated binaries (`WindBot.dll`, `ExecutorBase.dll`, `core.dll`, `bots.json`, `2026_AFS.ydk`, `DashBot.exe`) exclusively to `C:\Users\admin\Documents\EdoGame\`.

---

## 2. 2026_Spright ModernExecutor (Previous Update)

### Overview
- **Deck**: `2026_Spright` (Spright Frog / Nimble Engine: Spright Blue, Jet, Red, Carrot, Gigantic, Elf, Sprind, Toadally Awesome, Cat Shark, Zeus)
- **Executor**: `_2026_SprightExecutor.cs`
- **DashBot Category**: **2026 Decks** (registered as `2026_Spright` and `Expert_2026_Spright` in `bots.json`)
- **Architecture**: `ModernExecutor` (Rule-Based C# 10.0 / .NET 10)
- **Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`

---

### Benchmark Results (Extended Headless Duels — 10 Games/Matchup, 40 Total)

| Opponent Deck | Wins / Duels | Win Rate (%) | Rule Violations | Crashes | Notes |
|---|---|---|---|---|---|
| **ABC** | **4 / 10** | **40.0%** | 0 | 0 | `Toadally Awesome` Omni-negate + `Zeus` board wipe; competitive matchup |
| **Altergeist** | 2 / 10 | 20.0% | 0 | 0 | `Protocol` + `Hexstia` chains stall Spright plays; grind-heavy losses |
| **BlueEyes** | 3 / 10 | 30.0% | 0 | 0 | `Galaxy-Eyes Cipher Dragon` + `Gigantic Spright` + `Spright Elf` lethal pushes |
| **DarkMagician** | **4 / 10** | **40.0%** | 0 | 0 | `Cat Shark` + `Gigantic` OTK viable; `Spright Red` negates `Eternal Soul` |
| **Total** | **13 / 40** | **32.5%** | **0** | **0** | **100% Stability, 0 Errors, 0 Violations (32.5% Overall Win Rate)** |

---

### Key Tactical Implementations & Architecture Refinements
1. **Spright Inherent Summon & Search Chain**:
   - Inherent Special Summons from Hand (`Spright Blue`, `Spright Jet`, `Spright Red`, `Spright Carrot`, `Spright Pixies`) prioritized before committing Extra Deck materials.
   - **`Spright Blue` (`76145933`)**: Searches `Spright Jet` -> `Spright Jet` (`13533678`) searches `Spright Starter` / `Spright Smashers`.
2. **Frog & Nimble Engines Integration**:
   - **`Nimble Beaver` (`68353324`)**: Normal Summon instantly brings another Nimble Beaver from Deck.
   - **`Swap Frog` (`9126351`)**: Dumps `Ronintoadin` (`1357146`) to GY.
   - **`Ronintoadin` (`1357146`)**: Banishes extra Frogs from GY to revive as Level 2 Aqua material.
   - **`Toadally Awesome` (`90809975`)**: Xyz Summoned with 2x Level 2 Aquas (`Swap Frog` + `Ronintoadin`). Provides **Omni-Negate + Destroy + STEAL to our field** and recurses WATER monsters upon leaving the field.
3. **Extra Deck Synergy Pipeline**:
   - **`Gigantic Spright` (`54498517`)**: Detaches 1 material to summon `Swap Frog` or `Spright Blue` from Deck, and locks both players into Level/Rank/Link 2.
   - **`Spright Elf` (`27381364`)**: Protects pointed monsters from effect targeting, and provides Quick-Effect GY Revival on opponent's turn (bringing back `Toadally Awesome` or `Spright Red`).
   - **`Spright Sprind` (`72329844`)**: Dumps `Nimble Angler` (`88686573`) to Special Summon 2 Nimble Beavers from deck, and provides Quick Effect bounce disruption.
   - **`Cat Shark` (`84224627`)**: Doubles `Gigantic Spright` original ATK to 6400 ATK for lethal OTK pushes.
   - **`Onibimaru Soul Sweeper` (`9486959`)**: Spot banish removal for high-threat enemy monsters.
4. **Disruption & Interaction Layers (6+ Disruptions End Board)**:
   - `Toadally Awesome` (Omni-Negate & Steal) + `Spright Red` (Monster Negate & Pop) + `Spright Carrot` (S/T Negate & Pop) + `Spright Elf` (Revive Toadally/Red) + `Spright Smashers` (Non-targeting banish) + `Spright Double Cross` (Steal/Attach).
5. **Exclusive Deployment Target**:
   - Deployed all updated binaries (`WindBot.dll`, `ExecutorBase.dll`, `core.dll`, `bots.json`, `2026_Spright.ydk`, `DashBot.exe`) exclusively to `C:\Users\admin\Documents\EdoGame\`.

---

## 3. GOD-01 ModernExecutor (Optimized & Verified)

### Overview
- **Deck**: `GOD-01` (Egyptian Gods: Slifer the Sky Dragon, Obelisk the Tormentor, The Winged Dragon of Ra, Sphere Mode, Immortal Phoenix, Slime Engine, Rank 10 / Rank 11 Finisher)
- **Executor**: `_2026_God01Executor.cs`
- **DashBot Category**: **Other** (registered in `bots.json`)
- **Architecture**: `ModernExecutor` (Rule-Based C# 10.0 / .NET 10)
- **Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`

---

### Benchmark Results (Extended Headless Duels — 10 Games/Matchup, 40 Total)

| Opponent Deck | Wins / Duels | Win Rate (%) | Rule Violations | Crashes | Notes |
|---|---|---|---|---|---|
| **ABC** | **3 / 10** | **30.0%** | 0 | 0 | `Soul Crossing` tributes opponent monsters; `Sphere Mode` board clears |
| **Altergeist** | 2 / 10 | 20.0% | 0 | 0 | `Protocol` negate chains block God summons; long grind losses |
| **BlueEyes** | 3 / 10 | 30.0% | 0 | 0 | `Gustav Max` burn + `Liebe` OTK viable; 36-turn epic win in Game 6 |
| **DarkMagician** | 1 / 10 | 10.0% | 0 | 0 | `Eternal Soul` recursion outgrinds Gods; Ra 10,100 ATK OTK in Game 10 |
| **Total** | **9 / 40** | **22.5%** | **0** | **0** | **100% Stability, 0 Errors, 0 Violations (22.5% Overall Win Rate)** |

---

### Key Tactical Implementations & Architecture Refinements
1. **Slime Tribute & Fast God Engine**:
   - **`Egyptian God Slime` (`42166000`)**: Special Summoned from Extra Deck by tributing `Guardian Slime` or `Metal Reflect Slime`. **Counts as 3 Tributes for ANY God Monster**, cannot be destroyed by battle, and prevents opponent from targeting other monsters for attacks or effects.
   - **`Guardian Slime` (`15771991`)**: Handtrap Special Summon upon taking damage; when sent from Hand/Field to GY, searches `Soul Crossing`, `Ancient Chant`, `The True Sun God`, or `Millennium Revelation`.
   - **`Reactor Slime` (`79387392`)**: Main Phase creates 2 Slime Tokens; Battle Phase Quick Effect tributes itself to Set `Metal Reflect Slime` from Deck.
   - **`Metal Reflect Slime` (`26905245`)**: Instant Level 10 3000 DEF Trap Monster that immediately becomes `Egyptian God Slime` material.
2. **God Board Breakers & Quick-Play Disruption Pipeline**:
   - **`Soul Crossing` (`5253985`)**: Quick-Play Spell that tributes 1-3 opponent monsters to Tribute Summon Slifer, Obelisk, or Ra during either player's Main Phase!
   - **`Ancient Chant` (`78665705`)**: Searches Ra + grants extra Tribute Summon + transfers tributed ATK/DEF into Ra from GY.
   - **`The True Sun God` (`11587414`)**: Searches Ra support + dumps `Immortal Phoenix` + stops non-Ra special summoned monsters from attacking the turn they are summoned.
   - **`The Breaking Ruin God` (`85182315`)**: Quick-Play unnegatable Special Summon of `Obelisk the Tormentor` with full effect immunity.
   - **`The Revived Sky God` (`59094601`)**: Trap unnegatable Special Summon of `Slifer the Sky Dragon` + both players draw until 6 cards (+6000 ATK Slifer).
   - **`Super Polymerization` (`48130397`)**: Uncounterable board breaker fusing opponent monsters into `Mudragon of the Swamp`, `Starving Venom`, or `Predaplant Dragostapelia`.
   - **`Forbidden Droplet` (`24299458`)**: Negates enemy boss monsters and halves ATK by sending surplus spells/tokens as cost.
   - **`Mound of the Bound Creator` (`269012`)**: Blanket protection preventing Level 10+ monsters (all Gods, Slimes, Rank 10 Xyz) from being targeted or destroyed by opponent card effects.
3. **Rank 10 / Rank 11 Lethal OTK Engine**:
   - Overlay 2x Level 10 monsters (`Egyptian God Slime` / `Metal Reflect Slime` / `Guardian Slime`) -> **`Superdreadnought Rail Cannon Gustav Max` (`56910167`)** for 2,000 burn damage -> overlay into **`Superdreadnought Rail Cannon Juggernaut Liebe` (`26096328`)** for 6,000 ATK multi-attack lethal push!
4. **Extra Deck Banish Safety (`Pot of Prosperity`)**:
   - Explicitly protects `Egyptian God Slime` from being banished by `Pot of Prosperity`, strictly banishing link/utility cards first.
5. **Exclusive Deployment**:
   - Deployed all updated binaries (`WindBot.dll`, `ExecutorBase.dll`, `core.dll`, `bots.json`, `GOD-01.ydk`, `DashBot.exe`) exclusively to `C:\Users\admin\Documents\EdoGame\`.

---

## 4. Demise ModernExecutor (Previous Update)

### Overview
- **Deck**: `Demise` (Ritual Stun, Field Board Wipe, Impcantation & Diviner Engines, Rank 10 / Rank 8 OTK)
- **Executor**: `_2026_DemiseExecutor.cs`
- **Architecture**: `ModernExecutor` (Rule-Based C# 10.0 / .NET 10)
- **Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`

---

### Benchmark Results (Extended Headless Duels — 10 Games/Matchup, 40 Total)

| Opponent Deck | Wins / Duels | Win Rate (%) | Rule Violations | Crashes | Notes |
|---|---|---|---|---|---|
| **ABC** | 2 / 10 | 20.0% | 0 | 0 | `Demise Supreme King` board wipes effective but ABC rebuilds fast |
| **Altergeist** | 1 / 10 | 10.0% | 0 | 0 | `Protocol` + `Hexstia` chains block Ritual summons; hard matchup |
| **BlueEyes** | 1 / 10 | 10.0% | 0 | 0 | Blue-Eyes beatdown outpaces Ritual setup speed |
| **DarkMagician** | 2 / 10 | 20.0% | 0 | 0 | `Demise Supreme King` wipes + 3000 ATK direct; `Eternal Soul` still problematic |
| **Total** | **6 / 40** | **15.0%** | **0** | **0** | **100% Stability, 0 Errors, 0 Violations** |

---

### Key Tactical Implementations & Refinements
1. **Engine Modernization (`Demise.ydk`)**:
   - Integrated full **Impcantation Engine** (`Chalislime` x3, `Talismandra` x2, `Candoll` x2) for search and tribute recursion.
   - Added **Diviner of the Herald** (`Diviner` x3 + `Herald of the Arc Light` x2 in Extra Deck) for instant searches and Level 6 modulation.
   - Added **3x Forbidden Droplet** (`24299458`) + **1x Red Reboot** (`23002292`) as main deck disruption and board breaking tools.
   - Built verified 15-card Extra Deck with `Gustav Max` (2000 burn), `Liebe` (6000 ATK), `Draglubion` -> `Numeron Dragon` (9000 ATK), `Hope Harbinger`, `Photon Lord`, `Bagooska`, and `Dyna Mondo`.
2. **Rule-Based ModernExecutor Architecture**:
   - Integrated `ComboRouter`, `BaitPlanner`, `ChainAdvisor`, `HeuristicGuard`, and `ResourcePlan`.
   - Granular `OnSelectCard` hint management for `Preparation of Rites`, `Pre-Preparation of Rites`, `Chalislime`, `Candoll`, `Talismandra`, `Diviner`, and `Herald of the Arc Light`.
   - Tuned `ForbiddenDropletEffect` cost checking to allow using any hand/field tribute fodder.
   - Implemented `Dyna Mondo` Quick Effect disruption (GY Ritual revival + non-destruction card bounce).
3. **Build & Exclusive Deployment Pipeline**:
   - Compiled and deployed all binaries to `C:\Users\admin\Documents\EdoGame\`.

---

## 5. 2026_Darklord ModernExecutor (Previous Update)

### Overview
- **Deck**: `2026_Darklord` (DARK Fairy Fusion, GY Copying, Swarm OTK, and Board Breaking Engine)
- **Executor**: `_2026_DarklordExecutor.cs`
- **Architecture**: `ModernExecutor` (Rule-Based C# 10.0 / .NET 10)
- **Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`

---

### Benchmark Results (Headless Duels Across 4 Legacy Decks)

| Opponent Deck | Wins / Duels | Win Rate (%) | Rule Violations | Crashes | Notes |
|---|---|---|---|---|---|
| **ABC** | 4 / 5 | 80.0% | 0 | 0 | 5000 ATK `The First Darklord` + `Condemned Darklord` + `Mudragon` OTK |
| **BlueEyes** | 2 / 5 | 40.0% | 0 | 0 | Blanket targeting immunity + 4000 ATK `Darklord Eveningstar` beatdown |
| **DarkMagician** | 3 / 5 | 60.0% | 0 | 0 | High-threat disruption via `Sanctified` & `Darklord Rebellion` |
| **Altergeist** | 1 / 5 | 20.0% | 0 | 0 | `Protector of The Agents - Moon` + `Eveningstar` S/T removal |
| **Total** | **10 / 20** | **50.0%** | **0** | **0** | **100% Stability, 0 Errors, 0 Violations** |

---

### Key Tactical Implementations & Refinements
1. **Rule-Based ModernExecutor Architecture Overhaul**:
   - Integrated `ComboRouter` with 4 strategic lines: `Djehuty-Gulgolet-Fusion-Setup` (Score 95), `Morningstar-Tribute-BoardWipe` (Score 90), `SuperPoly-FirstDarklord-OTK` (Score 85), and `Ixchel-Contact-GrindControl` (Score 75).
   - Added `BaitPlanner`, `ChainAdvisor`, `HeuristicGuard`, and `ResourcePlan` integration.
2. **Anti-Self-Negate Guard & Safe Discard Cost Management**:
   - Fixed `Ixchel` and `Eveningstar` GY copying to never copy `The Sanctified Darklord` on the opponent's turn unless the opponent controls a face-up Effect Monster, preventing mandatory self-targeting.
   - Protected `Darklord Dance` and `Banishment of the Darklords` from being discarded as costs for `Condemned Darklord`, `Forbidden Droplet`, or `Herald of Orange Light`.
3. **Engine Search & Hint Message Handling (ocgcore)**:
   - Handled all standard hint constants (`500=FACEUP`, `501=TOGRAVE`, `502=DESTROY`, `503=TARGET`, `504=DISCARD`, `505=RTOHAND`, `506=ATOHAND`, `509=SPSUMMON`, `511=FMATERIAL`, `512=REMOVE`, `514=SET`, `551=RESOLVECARD`, `575=NEGATE`).
   - Fixed `Unleashed Power Patron Portal - Terminus` step 1 (Send Power Patron to GY) vs step 2 (Search DARK Fairy) preventing mis-selections.
4. **Dominus Impulse Turn Safety**:
   - Enforced strict rule: `Dominus Impulse` is NEVER activated from hand during our turn to prevent locking ourselves out of DARK Fairy Special Summons.
5. **Super Polymerization & Extra Deck Fusion Board Breaker**:
   - Strictly enforced using opponent's face-up monsters as primary materials for `The First Darklord`, `Darklord Eveningstar`, `Mudragon of the Swamp`, and `Garura`, while safeguarding our Ace Monsters from being consumed.

---

## 6. 2026_DarkWorld ModernExecutor (Previous Update)

### Overview
- **Deck**: `2026_DarkWorld` (Discard Trigger, Ceruli Opponent-Forced Discard, Quick-Play Fusion Engine)
- **Executor**: `_2026_DarkWorldExecutor.cs`
- **Architecture**: `ModernExecutor` (Rule-Based C# 10.0 / .NET 10)
- **Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`

### Benchmark Results (Headless Duels Across 4 Legacy Decks)

| Opponent Deck | Wins / Duels | Win Rate (%) | Rule Violations | Crashes | Notes |
|---|---|---|---|---|---|
| **ABC** | 1 / 5 | 20.0% | 0 | 0 | Board breaking via `Clorless` + `Grapha Overlord` (11,600 Field ATK) |
| **DarkMagician** | 2 / 5 | 40.0% | 0 | 0 | `Steelswarm Roach` lock + `Clorless` wipe & 4300 ATK rush |
| **BlueEyes** | 2 / 5 | 40.0% | 0 | 0 | Dual `Grapha Dragon Overlord` + `Luce` OTK (13,400 Field ATK) |
| **Altergeist** | 1 / 5 | 20.0% | 0 | 0 | `D/D/D Wave High King Caesar` double negate + `Dingirsu` removal |
| **Total** | **7 / 20** | **35.0%** | **0** | **0** | **100% Stability, 0 Errors, 0 Violations** |

---

## 7. 2026_Hecahand ModernExecutor (Historical)

### Overview
- **Deck**: `2026_Hecahand` (Illusion Control & Board Steal Archetype)
- **Executor**: `_2026_HecahandExecutor.cs`
- **Architecture**: `ModernExecutor` (Rule-Based C# 10.0 / .NET 10)
- **Deployment**: `C:\Users\admin\Documents\EdoGame\`

### Benchmark Results (40 Headless Duels)

| Opponent Deck | Wins / Duels | Win Rate (%) | Rule Violations | Crashes |
|---|---|---|---|---|
| **ABC** | 4 / 10 | 40.0% | 0 | 0 |
| **Altergeist** | 3 / 10 | 30.0% | 0 | 0 |
| **BlueEyes** | 4 / 10 | 40.0% | 0 | 0 |
| **DarkMagician** | 5 / 10 | 50.0% | 0 | 0 |
| **Total** | **16 / 40** | **40.0%** | **0** | **0** |

---

## 8. Board Breaker Experiment (2026-08-29)

### Goal
เพิ่ม Universal Board Breakers (`Lightning Storm`, `Dark Ruler No More`) + Handtraps ให้ 4 เด็คหลัก (AFS, Spright, GOD-01, Demise) เพื่อเพิ่มความสามารถ going-2nd และให้ AI มีความท้าทายเมื่อเล่น Player vs CPU

### Changes Applied (All 4 Decks)
- เพิ่ม `Lightning Storm` (14532163) + `Dark Ruler No More` (54693926)
- เพิ่ม `LightningStormEffect()` + `DarkRulerNoMoreEffect()` ใน Executor

### Benchmark v2 Results (160 games)

| Deck | Before Avg WR | After Avg WR | Δ |
|---|---|---|---|
| **2026_AFS** | 15.0% | **22.5%** | **+7.5%** ✅ |
| **2026_Spright** | 32.5% | 17.5% | **-15.0%** ❌ |
| **GOD-01** | 12.5% | 12.5% | = |
| **Demise** | 10.0% | 5.0% | **-5.0%** ❌ |
| **Overall** | **17.5%** | **14.4%** | **-3.1%** ❌ |

### Key Lessons
1. **AFS ได้ประโยชน์** — engine ยืดหยุ่น ตัดการ์ดรอง (Skill Drain, Triple Tactics) ได้
2. **Spright เสียเปรียบ** — ตัด Crossout Designator ทำให้ combo โดน Ash/Imperm ไม่มีทาง counter
3. **Demise เสียเปรียบ** — ตัด Candoll + Diviner ทำลาย ritual engine ที่ต้องการ consistency สูง
4. **Board Breakers เหมาะเฉพาะเด็คที่มี flexible engine** ไม่ใช่ combo-heavy decks

### Final Decision
- ✅ **Keep**: AFS + GOD-01 (board breakers retained)
- ❌ **Reverted**: Spright (restored Crossout x2 + Gamma Burst) + Demise (restored Candoll + Diviner + Red Reboot)
- Build & Deploy: **0 Errors, 0 Violations**

---

## 9. Engine Bugfix: `utility.lua` DelayedOperation Crash (2026-09-03)

### Overview & Incident
- **Error**: `[string "utility.lua"]:2890: Attempting to access deleted object.`
- **Call Stack**:
  ```text
  [C]: in method 'Filter'
  [string "utility.lua"]:2890: in upvalue 'get_affected_group'
  [string "utility.lua"]:2904: in function <[string "utility.lua"]:2903>
  [string "utility.lua"]:2890: Attempting to access deleted object.
  ```
- **Trigger**: การใช้งานการ์ดที่มีเอฟเฟกต์ Banish ชั่วคราวไปจนถึง End Phase (โดยเฉพาะ `S:P Little Knight` (29301450) ผ่าน `aux.RemoveUntil` -> `aux.DelayedOperation`) ระหว่างการดวลแมตช์ `2026_Runick vs 2026_AFS` ส่งผลให้โฮสต์เซิร์ฟเวอร์/OCGCore แครชทันทีเมื่อเข้าสู่ End Phase

### Root Cause Analysis
1. ฟังก์ชัน `Auxiliary.DelayedOperation` รับกลุ่มการ์ด `g` ซึ่งสร้างขึ้นจาก `Filter()` หรือ `Group.FromCards()` เข้ามา และจัดเก็บไว้ในเอฟเฟกต์ต่อเนื่องผ่าน `e1:SetLabelObject(g)`
2. ใน OCGCore หาก Group Object ใดไม่มีการเรียก `g:KeepAlive()` ตัวจัดการหน่วยความจำ C++ จะกวาดล้างและทำลาย Object ทิ้งทันทีเมื่อสิ้นสุด Chain นั้น
3. เมื่อเข้าสู่ End Phase เอฟเฟกต์ดีเลย์ `e1:SetCondition` ถูกทริกเกอร์และเรียก `get_affected_group(e)` ซึ่งพยายามรัน `:Filter(...)` บน Group Pointer ที่ถูกลบไปแล้ว ทำให้เกิด Crash ทันที

### Resolution Applied
- อัปเดตไฟล์สคริปต์ [script/utility.lua](file:///C:/Users/admin/Documents/EdoGame/script/utility.lua) และ [repositories/official-scripts/utility.lua](file:///C:/Users/admin/Documents/EdoGame/repositories/official-scripts/utility.lua):
  1. ทำการ Clone กลุ่มการ์ดและเรียก `g:KeepAlive()` เพื่อตรึง Object ไม่ให้ OCGCore ลบทิ้งข้าม Phase
  2. เพิ่ม Null Guard `if not g then return Group.CreateGroup() end` ใน `get_affected_group(e)`
  3. เรียก `g:DeleteGroup()` ภายใน `e1:SetOperation` เพื่อคืนหน่วยความจำอย่างถูกต้องหลังนำการ์ดกลับสู่สนามเสร็จสิ้น

### Verification Benchmark (`2026_Runick vs 2026_AFS` — 5 Games)
| Duel | Winner | Turns | Duration | Status | Notes |
|---|---|---|---|---|---|
| **Duel 01** | **2026_Runick** | 31 | 23.4s | OK | ยืนระยะ 31 เทิร์น S:P Banish & Return ทำงานสมบูรณ์ 100% |
| **Duel 02** | **2026_Runick** | 15 | 13.3s | OK | จบเกมปกติ ไม่พบ Error |
| **Duel 03** | **2026_Runick** | 27 | 26.0s | OK | ยืนระยะ 27 เทิร์น สลับการ์ดกลับสนามไร้รอยต่อ |
| **Duel 04** | **2026_AFS** | 3 | 15.3s | OK | AFS ปิดเกมเร็ว Turn 3 |
| **Duel 05** | **2026_Runick** | 26 | 27.7s | OK | ชนะใน Turn 26 บดเด็คสำเร็จ |
| **Total** | **Runick 4 - 1 AFS (80.0%)** | Avg 20.4 | 105.6s | **OK** | **Rule Violations: 0, Crashes: 0 (เสถียร 100%)** |

- ไบนารีและสคริปต์ทั้งหมดได้รับการ Deploy สู่ `C:\Users\admin\Documents\EdoGame\` ครบถ้วน

---

## 10. Central Core AI Refactoring & Universal Heuristics Engine (2026-09-05)

### Overview
ปฏิรูปและยกระดับ **Core ส่วนกลาง (Central AI Engine)** ของ WindBot (`ExecutorBase`, `GameAI`, `Executor`, `DefaultExecutor`, `ModernExecutor`) เพื่อสร้างรากฐานปัญญาประดิษฐ์สากลที่ฉลาด ปลอดภัย และมีเสถียรภาพสูงสุด ส่งผลให้ทั้งเด็ค Modern (2026 / GOAT) และเด็ค Legacy ทำงานฉลาดขึ้นทันทีโดยไม่ต้องฮาร์ดโค้ดรายเด็คซ้ำซ้อน

### ปัญหาที่ได้รับการแก้ไข (Root Cause Analysis)
1. **Blind Fallback in `GameAI.OnSelectCard`**: แก้ปัญหาการเลือก `cards[0]` แบบสุ่มเมื่อหลุดคอมโบ ด้วยระบบ `FallbackSelectCard` ที่ประเมิน Threat Score, Resource Cost, และ Card Value ตามหลักการของเกม
2. **Blind Monster Positioning in `DefaultExecutor.OnSelectPosition`**: แก้ปัญหาบอทเรียกมอนสเตอร์พลังป้องกันสูง/พลังโจมตีต่ำในสภาพหงายหน้าโจมตี (FaceUpAttack) กลายเป็นเป้าตีฟรี โดยปรับให้คำนวณ Stat Delta และเลือกตั้งรับอัตโนมัติหากพลังป้องกันเหนือกว่าพลังโจมตีอย่างมีนัยสำคัญ
3. **Hazardous Zone Placement in `Executor.OnSelectPlace`**: ป้องกันการวางการ์ดในคอลัมน์เดียวกับเวท/กับดักต่อเนื่องของศัตรู หรือคอลัมน์ที่เสี่ยงต่อ *Infinite Impermanence* พร้อมเลือก Extra Monster Zone ที่เหมาะสม
4. **Data Duplication & Inverted IDs**: รวมศูนย์ฐานข้อมูล Floodgate, Negators, Chokepoints และ Handtraps เข้าสู่ `CardIntelligence.cs` ด้วย $O(1)$ HashSets พร้อมแก้ไข Card ID ที่ผิดพลาดจาก Agent ยุคก่อนใน `AntiFloodgateHelper.cs`
5. **Comprehensive Hint-Based Resolution**: เติมเต็มการดักจับ HintMessage ใน `ModernExecutor.cs` ครอบคลุมการ Special Summon (`509`), Search to Hand (`505`), Bounce/Spin (`506`), Equip (`507`), Position Change (`518`), Disable (`552`), และ Negate (`572`)

### Benchmark Results (Headless Simulation Validation)
| Matchup | Result | Win Rate (%) | Violations | Crashes | Notes |
|---|---|---|---|---|---|
| **2026_Branded vs BlueEyes** | 3 - 2 (5 duels) | 60.0% | 0 | 0 | Smart Board Breaking & Threat Targeting |
| **2026_Branded vs DarkMagician** | 3 - 2 (5 duels) | 60.0% | 0 | 0 | กำจัด `Eternal Soul` & `Dark Magical Circle` อย่างแม่นยำ |
| **ABC vs Altergeist (Legacy)** | 5 duels | N/A | 0 | 0 | 100% Backward Compatibility, 0 Violations, 0 Crashes |

- **รายละเอียดฉบับเต็ม**: ดูที่ [CORE_AI_REFACTORING_REPORT.md](file:///C:/Users/admin/Documents/EdoGame/Docs/CORE_AI_REFACTORING_REPORT.md)
- **Deployment Status**: คอมไพล์และ Deploy ไบนารีชุดใหม่ (`ExecutorBase.dll`, `WindBot.dll`, `core.dll`, `bots.json`, `DashBot.exe`) สู่ `C:\Users\admin\Documents\EdoGame\` เรียบร้อย 100%

---

## 11. 2026 Fleet Health Audit, Purge of Obsolete Neural Bots, and Rehabilitation of 4 Core ModernExecutors (2026-09-05)

### Overview
ดำเนินการตรวจสอบและประเมินสุขภาพบอทซีรีส์ `2026_` ทั้ง 63 เด็คอย่างครอบคลุมผ่านการวิเคราะห์เชิงลึก (Static Code Analysis, Card Database Cross-Verification, Headless Duel Simulation) เพื่อแก้ปัญหาเด็คที่ทำงานผิดพลาด พร้อมทั้งล้างข้อมูลบอทขยะ และขัดเกลา 4 เด็คสำคัญที่มีอัตราการแพ้สูง (0% Baseline) ให้กลับมาเสถียรและมี Win Rate ก้าวกระโดด

### การจัดการบอท: Neural vs Expert
1. **`Neural_` Bots (Obsolete 100% — Purged)**:
   - ตรวจพบรายการบอทนำหน้าด้วย `Neural_` จำนวน 42 รายการใน `bots.json` ซึ่งเป็นของค้างจากแนวคิดเดิมที่ไม่มีโมเดลอยู่จริง (และขัดต่อข้อกำหนด Rule-based C# ล้วนตาม `AGENTS.md`)
   - ทำการ Purge รายชื่อ `Neural_*` ออกจาก `bots.json` ทั้งหมด 42 รายการ ส่งผลให้รายการบอทลดลงจาก 199 เหลือ 157 รายการ สะอาดและไม่ทำให้ผู้ใช้สับสน
2. **`Expert_` Bots (Retained for DashBot Launcher)**:
   - บอทกลุ่ม `Expert_` ยังคงจำเป็นสำหรับการจัดระดับความยาก (Difficulty Filter) ใน DashBot WPF Launcher UI
   - ได้ทำการตรวจสอบและแก้ไข Deck Path ที่ผิดพลาด (เช่น `2026_Doomz.ydk`) ให้เชื่อมต่อกับไฟล์การ์ดจริงได้อย่างสมบูรณ์

### 4 เด็คที่ได้รับการยกเครื่องและผลการทดสอบ (Headless Simulator Validation)

#### 1. `2026_DDD` (จาก 0% สู่การตั้ง End Board และปิดเกม 2-Turn OTK)
- **Deck Rebuild**: ปรับจูน [2026_DDD.ydk](file:///C:/Users/admin/Documents/EdoGame/src/YGO_SOURCE_CLEAN/windbot-fork/Decks/2026_DDD.ydk) ให้เป็น Tournament 40-Card Build ตัด Scale ไร้ประโยชน์ เพิ่ม `Piri Reis Map` x3 และ `Allure of Darkness` x3
- **Executor Fixes** ([_2026_DDDExecutor.cs](file:///C:/Users/admin/Documents/EdoGame/src/YGO_SOURCE_CLEAN/windbot-fork/Game/AI/Decks/_2026_DDDExecutor.cs)):
  - แก้ไข desync ตอนวาง Pendulum Scale ของ Gilgamesh
  - เพิ่มเงื่อนไข Deus Machinex ซ้อนทับ Xyz ทันที
  - แก้ไข activation condition ของ Dct Eternal Darkness ไม่ให้เปิดค้างไร้ประโยชน์
  - ปลดล็อค Nibiru Check เพื่อไม่ให้หยุดเล่นเองใน Turn 1
- **ผลลัพธ์**: จากเดิมหยุดคอมโบตั้งแต่ก้าวแรก กลายเป็นการสร้างบอร์ด Gilgamesh + Deus Machinex + Orthros และปิดเกม Turn 2 ชนะ 100% ในสถานการณ์ทดสอบ

#### 2. `2026_Plant` (จาก 0% สู่ 60.0% Win Rate เหนือ BlueEyes)
- **Executor Fixes** ([_2026_PlantExecutor.cs](file:///C:/Users/admin/Documents/EdoGame/src/YGO_SOURCE_CLEAN/windbot-fork/Game/AI/Decks/_2026_PlantExecutor.cs)):
  - เพิ่ม Material Protection ปกป้อง `Sunseed Genius Loci` ไม่ให้โดนสังเวยเป็น Cost ของ Mudan/Jasmine
  - ผูกเอฟเฟกต์เพิ่ม LP ของ Sunavalon Dryas ให้ทริกเกอร์ตามสถานการณ์จริง
  - ลบคำสั่ง `AI.SelectNextCard` ที่ไม่ตรงลำดับ ซึ่งเคยส่งผลให้เกิด `Error: Call SelectNextCard() before SelectCard()`
  - ลงทะเบียน 3 ComboLine Sequence ใน `ComboRouter`
- **ผลลัพธ์**: ชนะ BlueEyes **3 - 2 (Win Rate 60.0%)** ก้าวหน้าจาก baseline 0% เดิม พร้อม **0 Violations / 0 Crashes**

#### 3. `2026_Purrely` (จาก 0% สู่ 40.0% Win Rate เหนือ BlueEyes)
- **Executor Fixes** ([_2026_PurrelyExecutor.cs](file:///C:/Users/admin/Documents/EdoGame/src/YGO_SOURCE_CLEAN/windbot-fork/Game/AI/Decks/_2026_PurrelyExecutor.cs)):
  - ดักจับ `OnSelectYesNo` สำหรับ `Epurrely Plump`: ป้องกันไม่ให้บอทกดตกลงสั่ง Banish มอนสเตอร์ของตนเองทิ้งใน Turn 1 เมื่อสนามคู่ต่อสู้ว่างเปล่า
  - ปรับการเลือก Discard Cost ให้ส่งไปเลือกใน `OnSelectCard`
  - ลบ invalid `AI.SelectNextCard` ทั้งหมด
  - ลงทะเบียนคอมโบเรียก `Expurrely Noir` 5+ Materials
- **ผลลัพธ์**: ชนะ BlueEyes **2 - 3 (Win Rate 40.0%)** (สร้าง Expurrely Noir 5+ Materials สมบูรณ์) **0 Violations / 0 Crashes**

#### 4. `2026_Dracotail` (จาก 0% สู่ 80.0% Win Rate เหนือ BlueEyes)
- **Executor Fixes** ([_2026_DracotailExecutor.cs](file:///C:/Users/admin/Documents/EdoGame/src/YGO_SOURCE_CLEAN/windbot-fork/Game/AI/Decks/_2026_DracotailExecutor.cs)):
  - เพิ่ม `OnSelectYesNo` ดักจับเอฟเฟกต์ทำลายของการ์ด `Pan` และ `Urgula`: ป้องกันไม่ให้ Pan ทำลายมอนสเตอร์ตัวใหม่ของตนเอง (`Alba-Lenatus`) หรือ Urgula ทำลายกับดัก `Rahu Dracotail` ของตนเอง
  - เพิ่ม `OnSelectFusionMaterial` ปกป้องตัว Ace ไม่ให้ถูกนำไปฟิวชั่นซ้ำ
  - ปรับ `FaimenaQuickEffect` ให้ต้องการมอนสเตอร์อย่างน้อย 2 ตัวขึ้นไปก่อนใช้ เพื่อไม่ให้สนามว่างเปล่า
- **ผลลัพธ์**: ชนะ BlueEyes **4 - 1 (Win Rate 80.0%)** ปิดเกมด้วย Alba-Lenatus และ Faimena อย่างดุดัน **0 Violations / 0 Crashes**

### สรุปสถิติหลังการปรับปรุง 4 เด็คหลัก

| Deck Name | Opponent | Wins / Matches | Win Rate (%) | MSG_RETRY Violations | Engine Crashes |
|---|---|---|---|---|---|
| **2026_DDD** | BlueEyes (Gold) | 1 / 1 (OTK Test) | 100% | 0 | 0 |
| **2026_Plant** | BlueEyes (Standard) | 3 / 5 | 60.0% | 0 | 0 |
| **2026_Purrely** | BlueEyes (Standard) | 2 / 5 | 40.0% | 0 | 0 |
| **2026_Dracotail** | BlueEyes (Standard) | 4 / 5 | 80.0% | 0 | 0 |
| **รวมสถิติ** | **BlueEyes** | **10 / 16** | **62.5%** | **0** | **0** |

- **รายงานฉบับเต็ม**: บันทึกไว้ที่ [AUDIT_AND_OPTIMIZATION_REPORT_2026.md](file:///c:/Users/admin/Documents/EdoGame/Docs/AUDIT_AND_OPTIMIZATION_REPORT_2026.md)
- **Deployment**: ไบนารีทั้งหมด (`WindBot.dll`, `ExecutorBase.dll`, `core.dll`, `bots.json`, decks) ผ่านสคริปต์ `BUILD_AND_DEPLOY.ps1` และติดตั้งไปยัง `C:\Users\admin\Documents\EdoGame\` ครบถ้วนเรียบร้อย
- **เอกสารส่งต่องาน (Handoff)**: จัดทำเอกสารสรุปสถานะและ 3 ทางเลือกสำหรับการพัฒนาต่อบนเครื่องใหม่ไว้ที่ [Docs/HANDOFF_STRATEGY_OPTIONS.md](file:///c:/Users/admin/Documents/EdoGame/Docs/HANDOFF_STRATEGY_OPTIONS.md) และ [HANDOFF.md](file:///c:/Users/admin/Documents/EdoGame/HANDOFF.md)

---

## 9. 2026_Tearla & 2026_Stun ModernExecutors (Full Goal Complete)

### ภาพรวมการพัฒนา
- พัฒนา Rule-Based C# ModernExecutor ใหม่ 2 เด็คเต็มรูปแบบ:
  1. `_2026_TearlaExecutor.cs` (Fiendsmith-Brilliant-Tearlaments Engine)
  2. `_2026_StunExecutor.cs` (Dominus & Anti-Meta Dogmatika Stun)
- ลงทะเบียนบอทใน `bots.json` พร้อมทั้งหมวดหมู่ `2026 Decks` และโหมด `Expert`
- ดำเนินการทดสอบ Headless Simulation ทั้งสิ้น **80 นัด** พบ **0 Violations / 0 Engine Crashes (100% Stability)**

### สรุปสถิติผลการทดสอบ (Headless Benchmark 80 นัด)

| Deck | Opponent | Games | Win Rate (%) | Violations | Crashes |
|---|---|---|---|---|---|
| **2026_Stun** | **DarkMagician** | 10 | **90.0%** (9/10) | 0 | 0 |
| **2026_Stun** | **Altergeist** | 10 | **70.0%** (7/10) | 0 | 0 |
| **2026_Stun** | **BlueEyes** | 10 | **60.0%** (6/10) | 0 | 0 |
| **2026_Stun** | **ABC** | 10 | **30.0%** (3/10) | 0 | 0 |
| *รวม 2026_Stun* | *เฉลี่ย 4 เด็ค* | *40* | ***62.5%*** | *0* | *0* |
| **2026_Tearla** | **DarkMagician** | 10 | **50.0%** (5/10) | 0 | 0 |
| **2026_Tearla** | **ABC** | 10 | **40.0%** (4/10) | 0 | 0 |
| **2026_Tearla** | **BlueEyes** | 10 | **40.0%** (4/10) | 0 | 0 |
| **2026_Tearla** | **Altergeist** | 10 | **40.0%** (4/10) | 0 | 0 |
| *รวม 2026_Tearla* | *เฉลี่ย 4 เด็ค* | *40* | ***42.5%*** | *0* | *0* |
| **รวมสถิติทั้งหมด** | **ทุกคู่ซ้อม** | **80** | **52.5%** | **0** | **0** |

- **รายงานการวิเคราะห์และแก้ไขเชิงลึก**: จัดทำไว้ที่ [Docs/2026_Tearla_Stun_Optimization_Report.md](file:///c:/Users/admin/Documents/EdoGame/Docs/2026_Tearla_Stun_Optimization_Report.md)
- **Deployment สถานะ**: อัปเดตและ Deploy ไปยัง `C:\Users\admin\Documents\EdoGame\` ครบถ้วน 100%

---

## 10. 2026_Purrely & 2026_Yummy Championship ModernExecutors (Full Goal Complete)

### ภาพรวมการพัฒนา & การปรับปรุงเชิงโครงสร้าง
- ยกระดับ Rule-Based C# ModernExecutor ทั้ง 2 เด็คสู่มาตรฐานระดับ Championship Tier-1:
  1. **`_2026_PurrelyExecutor.cs`**:
     - เพิ่มตรรกะ Proactive Spin on Our Turn (`ExpurrelyNoirEffect`) หมุนบอร์ดกวาดสนาม (`Eternal Soul`/`True Light`), มอนสเตอร์บอสพลังสูง (`ABC-Dragon Buster`), และการ์ดหลังบ้านอันตราย (`Altergeist Protocol`, `Secret Village`) ในเทิร์นของบอทเอง
     - แก้ไข `ShouldActivateMemoryInHand` อนุญาตให้ป้อน Quick-Play Memory ให้กับมอนสเตอร์ Rank 2 บนสนามเพื่อเตรียมไต่ระดับขึ้นสู่ Noir ตัวถัดไป
     - เพิ่มระบบป้องกัน Material Anti-Cannibalization ใน `GetMaterialPriority` ปกป้องตัว Ace ที่มีวัตถุดิบสูงไม่ให้ถูกนำไป Link
  2. **`_2026_YummyExecutor.cs`**:
     - แก้ไขบั๊กวิกฤติ Cooky Way Self-Targeting: ใน `OnSelectEffectYn` ป้องกันไม่ให้ Cooky Way เปิดเอฟเฟกต์คว่ำหน้าตัวมันเองเมื่อสนามคู่ต่อสู้ไม่มีมอนสเตอร์หงายหน้า
     - ปรับปรุงการวางสนามของ Snatchy และจัดเส้นทาง Quick Synchro ให้เรียก `Cupsy★Yummy Way` เป็นลำดับสูงสุด (Score 2500) เพื่อบวกการ์ดค้นหา +2 ในมือ
     - เชื่อมต่อคอมโบ `Spright Elf` ชุบชีวิต `Cupsy★Yummy Way` กลับคืนสนาม สร้างบอร์ดขัดจังหวะ 6 รูปแบบในเทิร์นเดียว
     - แก้ไขการหมอบ `Yummy☆Surprise` ใน MP1 และการเลือกเป้าหมาย 2+2 Double-Bounce
     - แก้ไขการสวมใส่ Link มอนสเตอร์ของ `Borreload Savage Dragon` จากสุสานเพื่อรับ 3700 ATK และ 2 Omni-Negates

### สรุปสถิติผลการทดสอบ (Headless Benchmark 80 นัด)

| Deck | Opponent | Games | Win Rate (%) | Violations | Crashes |
|---|---|---|---|---|---|
| **2026_Purrely** | **DarkMagician** | 10 | **40.0%** (4/10) | 0 | 0 |
| **2026_Purrely** | **BlueEyes** | 10 | **40.0%** (4/10) | 0 | 0 |
| **2026_Purrely** | **ABC** | 10 | **20.0%** (2/10) | 0 | 0 |
| **2026_Purrely** | **Altergeist** | 10 | **20.0%** (2/10) | 0 | 0 |
| *รวม 2026_Purrely* | *เฉลี่ย 4 เด็ค* | *40* | ***30.0%*** | *0* | *0* |
| **2026_Yummy** | **DarkMagician** | 10 | **30.0%** (3/10) | 0 | 0 |
| **2026_Yummy** | **ABC** | 10 | **30.0%** (3/10) | 0 | 0 |
| **2026_Yummy** | **Altergeist** | 10 | **20.0%** (2/10) | 0 | 0 |
| **2026_Yummy** | **BlueEyes** | 10 | **10.0%** (1/10) | 0 | 0 |
| *รวม 2026_Yummy* | *เฉลี่ย 4 เด็ค* | *40* | ***22.5%*** | *0* | *0* |
| **รวมสถิติทั้งหมด** | **ทุกคู่ซ้อม** | **80** | **26.3%** | **0** | **0** |

- **รายงานฉบับสมบูรณ์**: บันทึกไว้ที่ [Docs/2026_Purrely_Yummy_Optimization_Report.md](file:///C:/Users/admin/Documents/EdoGame/Docs/2026_Purrely_Yummy_Optimization_Report.md)
- **Deployment**: ไบนารีชุดใหม่ (`WindBot.dll`, `ExecutorBase.dll`, `core.dll`, `bots.json`, Decks, `DashBot.exe`) ติดตั้งลงใน `C:\Users\admin\Documents\EdoGame\` ครบถ้วน 100%

---

## 0.0. Central Core Architecture Upgrade: Duplicate Chain & Self-Negation Prevention (2026-09-06)

### Root Cause Analysis & Problem Statement
- **Issue**: In `2026_Stun` duels, logs revealed that the bot frequently performed redundant self-chains with Counter Traps (specifically `Solemn Report 78114463`), paying 1500 LP twice in the same chain (e.g. Dogmatika Punishment CL1 → Solemn Report CL2 → Solemn Report CL3).
- **Core Defects Found**:
  1. `DefaultExecutor.cs` defined `DefaultSolemnJudgment`, `DefaultSolemnWarning`, and `DefaultSolemnStrike`, but lacked `DefaultSolemnReport`.
  2. `_2026_StunExecutor.cs` used an ad-hoc method `SolemnReportEffect()` with `(Duel.LastChainPlayer == 1 || Duel.Player == 1)`. In opponent's turn (`Duel.Player == 1`), this was always true even when `Duel.LastChainPlayer == 0` (the bot itself just activated a card), causing Solemn Report to negate the bot's own traps and double-chain.
  3. No central guard in `DefaultExecutor` or `GameAI.ShouldExecute` prevented Counter Traps from double-activating in the same chain or negating cards controlled by the bot.

### Core Architecture Enhancements Implemented
1. **`DefaultExecutor.cs`**:
   - Added `SolemnReport = 78114463;` to `_CardId`.
   - Added `DefaultSolemnReport()`: Enforces `Bot.LifePoints > 1500`, `!Util.ChainContainsCard(Card.Id)`, `Duel.LastChainPlayer == 1`, and ensures the target is an active opponent Spell/Trap card.
   - Enhanced `DefaultSolemnJudgment()`, `DefaultSolemnWarning()`, and `DefaultSolemnStrike()` with `!Util.ChainContainsCard(Card.Id)`, `Duel.LastChainPlayer != 0`, and checks that the last chain card is not bot-controlled or already disabled.
2. **`GameAI.cs` (`ShouldExecute`)**:
   - Implemented universal Central Core Counter Trap guards for `card.HasType(CardType.Counter)`:
     - Automatically rejects activation if `Duel.LastChainPlayer == 0` (immediately preceding chain link is bot's own card).
     - Automatically rejects activation if `Duel.LastChainPlayer == -1 && Duel.LastSummonPlayer == 0` (bot's own summon).
     - Automatically rejects activation if `Duel.CurrentChain.Any(c => c.IsCode(card.Id))` (duplicate counter-trap in the same chain).
     - Automatically rejects activation if `Duel.CurrentChain.LastOrDefault()?.Controller == 0`.
3. **`_2026_StunExecutor.cs`**:
   - Delegated `SolemnReport` to `DefaultSolemnReport`.
   - Hardened `DominusSparkEffect` with `Duel.LastChainPlayer != 0` guard.
   - Hardened `MaxxCEffect` against duplicate chains.

### Headless Duel Simulation Verification (40 Duels)
- **ABC**: 6 Wins / 4 Losses (**60.0%**, jumped from 30% baseline!) | 0 Violations, 0 Crashes
- **Altergeist**: 6 Wins / 4 Losses (**60.0%**) | 0 Violations, 0 Crashes
- **BlueEyes**: 5 Wins / 5 Losses (**50.0%**) | 0 Violations, 0 Crashes
- **DarkMagician**: 7 Wins / 3 Losses (**70.0%**) | 0 Violations, 0 Crashes
- **Total**: 24 Wins / 16 Losses (**60.0% Win Rate**), **0 MSG_RETRY Errors, 0 Crashes**, and **0 redundant double-chains / 0 self-negations**.







---

## 0.000. Critical Freeze & Redundant Chain Prevention Hotfix (2026-09-06)

### Root Cause Analysis & Problem Statement
1. **Engine Freeze / Infinite Loop**:
   - In live duels (e.g. `2026_AFS vs ABC`), the engine generated over 58,000 log lines in 2 seconds and completely froze.
   - When a primary combo line (`AFS-TripleEngine-OmniBoard`) failed midway at Engraver, `ComboRouter` switched to fallback `Fiendsmith-Caesar-Line`.
   - When `Fiendsmith-Caesar-Line` failed at Poplar, `ComboRouter` reset and switched right back to `AFS-TripleEngine-OmniBoard`.
   - Inside `ModernExecutor.cs`: `while (ComboRouter.HasActiveCombo)` ping-ponged between the two lines in an infinite while loop without passing control back to the game engine.
2. **Redundant Chains & "Player Not Reading Card Text" Behavior**:
   - The bot repeatedly chained multiple copies of `Infinite Impermanence (10045474)` or `Effect Veiler (97268402)` on a single monster that was **already disabled/negated** (e.g. ABC-Dragon Buster).
   - In `CardContainer.cs` (`GetShouldBeDisabledBeforeItUseEffectMonster`), it did NOT check `!card.IsDisabled()`. Because ABC-Dragon Buster matched the high-threat list, the function kept returning it as an active target even after its effects were already negated.
   - In `DefaultExecutor.cs` (`DefaultDisableMonster`), it lacked checks preventing duplicate negation cards in the current chain link.

### Core Fixes Implemented
1. **`ComboRouter.cs` (Turn-scoped Failed Line Blacklist)**:
   - Added `_failedLinesThisTurn = new HashSet<string>()`.
   - When any combo line fails execution, it is added to `_failedLinesThisTurn` and cannot be reselected during the same turn.
   - Clear blacklist automatically on each new turn (`Reset()`).
2. **`ModernExecutor.cs` (Combo Loop Safety Limiter)**:
   - Added hard iteration cap (`int maxIterations = 15; while (ComboRouter.HasActiveCombo && --maxIterations > 0)`) preventing runaway while-loops.
   - Hardened `DefaultEffectVeiler` with `!lastChain.IsDisabled()` and duplicate chain check.
3. **`CardContainer.cs` (`GetShouldBeDisabledBeforeItUseEffectMonster`)**:
   - Added `!card.IsDisabled()` check: will never return an opponent monster that is already negated.
4. **`DefaultExecutor.cs` (`DefaultDisableMonster`)**:
   - Added check ensuring `!target.IsDisabled()` and blocking activation if `Duel.CurrentChain` already contains an active negation targeting that monster.
5. **`GameAI.cs` (`ShouldExecute`)**:
   - Added universal Central Core Handtrap/Negator guards:
     - Block activating identical handtraps/negators in the same chain link.
     - Block chaining targeted negators (Imperm/Veiler) if `Duel.LastChainPlayer == 0` (bot's own card).
     - Block chaining targeted negators if `Duel.CurrentChain.LastOrDefault()` is already disabled.

### Headless Duel Verification (2026_AFS vs ABC)
- 3/3 duels completed with **Status: OK**, 0 Violations, 0 Crashes, 0 Freezes (average 12.9s per duel).
- Redundant chains onto disabled monsters completely eliminated.

---

## 0.00. Central Core Intelligence & ModernExecutor Overhaul (2026-09-06)

### Objective
Systemic overhaul of Central Core AI (`ExecutorBase`, `GameAI`, `ModernExecutor`, `DefaultExecutor`, `CardIntelligence`, `ComboRouter`) to eliminate bot misplays, suicide plays, designation corruption, and ensure genuine tournament-grade Player vs CPU intelligence without touching individual deck files.

### 6 Core Architecture Upgrades
1. **Permanent Match-Level Designation (`IsGoingFirst` / `IsGoingSecond`)**:
   - **Root Cause**: `_isGoingSecond = (Duel.Turn > 1)` was executed every turn across dozens of 2026 executors and `ModernExecutor`. On Turn 3 (bot went first), `_isGoingSecond` flipped to `true`, destroying Turn 1 board setups and triggering going-second board breaker code inappropriately. Furthermore, on Turn 1 when opponent went first, `IsGoingSecond` was unset until Turn 2.
   - **Fix**: Replaced `_isGoingSecond` field in `ModernExecutor` with a property backed by `IsGoingSecond`. Locked match designation permanently on Turn 1 in `Executor.PreNewTurn()` based on `(Duel.Turn % 2)` and `Duel.Player`.
2. **Chain Link 3 Defense & Pre-emptive Draw/Standby Floodgates**:
   - **Root Cause**: When opponent responded with handtraps to interrupt bot combos, bots did not automatically chain `Called by the Grave` or `Crossout Designator` as Chain Link 3. Additionally, continuous floodgates (`Skill Drain`, `There Can Be Only One`, `Anti-Spell Fragrance`, `Dimensional Barrier`) were held until Main Phase when opponent already cast board breakers (Harpie's, Lightning Storm, Evenly Matched).
   - **Fix**: Implemented `CheckChainLink3Defense` and `CheckDrawStandbyFloodgate` in `Executor.cs`, and wired them directly into `GameAI.OnSelectChain` so CL3 protection and Draw/Standby flips occur reliably across all bots.
3. **Universal `OnSelectYesNo` Self-Harm Guard**:
   - **Root Cause**: `Executor.OnSelectYesNo` unconditionally returned `true`. Cards with optional removal targeting "on the field" (e.g. `Dracotail Pan`, `Dracotail Urgula`, `Epurrely Plump`) prompted "Do you want to destroy 1 card on the field?". On Turn 1 or empty opponent fields, answering Yes forced the bot to destroy/banish its own cards.
   - **Fix**: Implemented `OnSelectYesNo` override in `ModernExecutor` guarding against empty opponent fields and specific string IDs, refusing optional removal when enemy has 0 valid targets.
4. **ComboRouter Dynamic Fallback & Plan B Execution**:
   - **Root Cause**: When a mandatory combo step was negated or unplayable, `ComboRouter` aborted the entire combo and passed the turn without checking alternative lines.
   - **Fix**: Added `TrySwitchToFallback(ClientField bot)` in `ComboRouter.cs`. When a step fails in `ModernExecutor.OnSelectIdleCmd`, the engine immediately attempts the registered `FallbackLineName` or re-evaluates available cards to seamlessly switch to Plan B/C.
5. **Universal Threat-Weighted Battle Targeting & Baiting**:
   - **Root Cause**: Bots attacked face-up high-ATK targets blindly, crashed into damage reflection monsters (Mikanko, Yubel, Timelords), or walked into face-down backrow with high-ATK bosses first.
   - **Fix**: Added `IsDangerousBattleTarget()` in `CardIntelligence.cs` (blocking Mikanko, Yubel, Timelords, Lion Heart, Sphreeze). Rewrote `OnSelectAttackTarget()` with threat scoring (negators +120, floodgates +100, lethal push bonus). Added face-down backrow baiting in `GameAI.InternalOnSelectBattleCmd` (attacking with lowest ATK monster first unless lethal is assured).
6. **Effect Veiler Canonical ID Correction & CardIntelligence Unification**:
   - **Root Cause**: `63845230` (Eater of Millions) was mistakenly hardcoded as Effect Veiler across `BoardScorer`, `ChainTimingAdvisor`, `ModernExecutor`, `StateRepresentation`, `DynamicValueEvaluator`, `BeliefState`, and `2026_GemKnight.ydk`. Real Effect Veiler (`97268402`) was ignored, and drawing Eater of Millions caused false handtrap holding.
   - **Fix**: Corrected all references to `97268402` for Effect Veiler, separated `63845230` as Eater of Millions, corrected `Crossout Designator` canonical ID to `65681983`, and added `Dominus Impulse`, `Dominus Purge`, and `PSY-Framegear Gamma` to `CardIntelligence`.

### Headless Duel Simulation Verification
- **`2026_Dracotail vs Blue-Eyes`**: 4 Wins / 1 Loss (**80.0% Win Rate**) | 0 Violations, 0 Crashes
- **`2026_Dracotail vs ABC`**: 1 Win / 2 Losses (**33.3% Win Rate**) | 0 Violations, 0 Crashes
- **`2026_Dracotail vs DarkMagician`**: 2 Wins / 1 Loss (**66.7% Win Rate**) | 0 Violations, 0 Crashes
- **`2026_Dracotail vs Altergeist`**: 2 Wins / 1 Loss (**66.7% Win Rate**) | 0 Violations, 0 Crashes
- **`2026_Stun vs Blue-Eyes`**: 2 Wins / 1 Loss (**66.7% Win Rate**) | 0 Violations, 0 Crashes
- **Overall**: 11 Wins / 6 Losses (**64.7% Win Rate**), **0 Engine Crashes, 0 MSG_RETRY Violations**.

---

## 0.006. DashBot Launcher Frontend Revamp (2026-09-20)
- **Concept & Request**: Redesigned DashBot Launcher UI according to user requirements: removed all version numbers (`v2.3`, `2026_`), implemented a tournament-style Pill Card Grid deck selector (without card art), categorized decks (`All`, `Modern`, `Anime`, `Legacy`, `Special`), added real-time live search, added credits (`By EDO Team | Custom Deck By Jaynesiz`), and eliminated all emojis.
- **Architecture & UI Updates (`dashbot/MainWindow.xaml`, `dashbot/MainWindow.xaml.cs`)**:
  - **Clean Naming & Zero Versioning**: Removed `v2.3` from Window title, Header, Footer, and console greeting. Automated `CleanDeckDisplayName` strips prefixes (`2026_`, `Expert_2026_`, `Neural_2026_`, `AI_`, `Anime_`, `GOAT_`) while mapping proper spacing for archetype readability (e.g. `Red Dragon Archfiend`, `Dark Magician`, `Jack Atlas`).
  - **Pill Grid Deck Library**: Replaced legacy ComboBoxes with a scrollable 2-column WrapPanel grid of tournament pill badges with distinct category tags (Amber for Modern, Pink for Anime, Royal Blue for Legacy, Emerald for Special).
  - **Category Tabs & Real-Time Filter**: Instant category switching tabs with total deck counts and a live search box filtering across both display names and filenames.
  - **Duel Matchup & Dual Bot Assignment**: Added interactive toggles for Bot 1 (Player) and Bot 2 (Opponent) assignment with live matchup preview card (`[BOT 1: Deck A] VS [BOT 2: Deck B]`) and right-click shortcuts.
  - **Credits & No Emoji Rule**: Added `By EDO Team` and `Custom Deck By Jaynesiz` in both header and footer; strictly zero unicode emojis across the UI.
- **Build & Exclusive Deployment**: Built and deployed exclusively to `C:\Users\admin\Documents\EdoGame\DashBot.exe` via `BUILD_AND_DEPLOY.ps1`.

## 0.005. New Anime Series Rule-Based ModernExecutor: Anime_JackAtlas (2026-09-20)
- **Source & Concept**: Ported from YGOPRODeck Anime category (`Anime Battlebox - Jack Atlas`). Features Jack Atlas's Resonator, Red Dragon Archfiend, and Bystial Synchro engine with devastating Level 8, 9, 10, and 12 boss lines.
- **Deck Composition (`Anime_JackAtlas.ydk`)**:
  - Main Deck (45 cards): 3x Nibiru, the Primal Being, 1x The Bystial Lubellion, 3x Fidraulis Harmonia, 1x Bystial Baldrake, 1x Bystial Druiswurm, 1x Fiend Piece Golem, 3x Power Vice Dragon, 3x Bone Archfiend, 1x Wandering King Wildwind, 3x Red Lotus King, Flame Crime, 3x Darkness Resonator, 3x Soul Resonator, 1x Crimson Resonator, 2x Vision Resonator, 2x Synkron Resonator, 1x Crimson Call, 3x Resonator Call, 3x Crimson Gaia, 3x Dominus Impulse, 1x Red Reign, 1x Red Dragon Archfiend's Chain, 1x Red Zone, 1x The Ruler's Rumbling.
  - Extra Deck (15 cards): 1x Red Hypernova Dragon, 1x Hot Red Dragon Archfiend King Calamity, 1x Red Supernova Dragon, 1x Red Nova Dragon - Burning Soul, 1x Hot Red Dragon Archfiend Bane, 1x Hot Red Dragon Archfiend Abyss, 2x Red Dragon Archfiend, 1x Scarred Dragon Archfiend, 1x The Crimson King, 1x Kuibelt the Blade Dragon, 1x Crimson Blade Dragon, 3x Red Rising Dragon.
- **Architecture & Intelligent Executor (`Anime_JackAtlasExecutor.cs`)**:
  - **Soul Resonator & Bone Archfiend Engine**: Normal Summon `Soul Resonator` searches `Bone Archfiend`; `Bone Archfiend` discards to Special Summon itself and dumps `Vision Resonator` or `Crimson Resonator` from Deck to adjust levels; `Vision Resonator` adds `Crimson Gaia` to hand to set up `Red Zone` or search follow-ups.
  - **Red Rising Dragon Ladder**: Level 6 Dragon Synchro revives `Crimson Resonator` or `Soul Resonator` from GY upon Synchro Summon; banishes itself from GY to revive 2 Level 1 Resonators (`Synkron Resonator`).
  - **Crimson Resonator Multi-Summon**: When controlling exactly 1 DARK Dragon Synchro (`Red Rising Dragon` or `Scarred Dragon Archfiend`), summons 2 Resonators (`Vision Resonator` + `Synkron Resonator`) directly from Deck for immediate double or triple tuner climbing.
  - **Scarred Dragon Archfiend Float & Wipe**: When sent to GY as Synchro Material for higher DARK Dragon Synchros, automatically cheats out `Red Dragon Archfiend` from Extra Deck and destroys all opponent Attack Position monsters.
  - **Boss Lineups**:
    - `Hot Red Dragon Archfiend Abyss`: Quick Effect target negation of face-up opponent threat/floodgate cards.
    - `Hot Red Dragon Archfiend Bane`: Tributes lower monster to revive RDA bosses; revives 2 Tuners on battle damage.
    - `Red Supernova Dragon`: 4000+ ATK, effect destruction immunity; Quick Effect banishes all opponent cards on monster effect activation or attack declaration.
    - `Red Nova Dragon - Burning Soul` & `Hot Red Dragon Archfiend King Calamity`: Ultimate beatdown and turn shutdown.
  - **Disruption Grid & Handtraps**: `Dominus Impulse`, `Nibiru, the Primal Being`, `Bystial Druiswurm`, `Bystial Baldrake`, `Red Zone` (pops card when RDA triggers; revives banished Dragon Synchro), and `Red Reign` (non-targeting banish all monsters except highest level Synchro with total effect immunity).
  - **Self-Harm Guards**: Safe targeting on `Red Zone`, `Kuibelt`, and `OnSelectYesNo` empty-field protection.
  - **Lua Script Bug Fix**: Fixed `Attempting to access deleted object` at line 72 in `c70088809.lua` (`Fydraulis Harmonia`) caused by storing a temporary C++ `Group` across phase callbacks without `KeepAlive`. Resolved by storing a native Lua table of persistent `Card` objects and recreating a fresh `Group` in `effop`. Synchronized across all 4 script locations (`script/`, `script/official/`, `repositories/local-patches/`, `repositories/official-scripts/`).
- **Bot Registration & Deployment**: Registered as `Anime_JackAtlas` in `bots.json` under DashBot's **Other Decks** category; built and deployed exclusively to `C:\Users\admin\Documents\EdoGame\` via `BUILD_AND_DEPLOY.ps1`.

## 0.004. New Anime Series Rule-Based ModernExecutor: Anime_Yusei (2026-09-20)
- **Source & Concept**: Ported from YGOPRODeck Anime category (`Anime Battlebox - Yusei`). Features Yusei Fudo's Synchron, Junk, and Stardust Synchro laddering engine with massive Extra Deck boss options.
- **Deck Composition (`Anime_Yusei.ydk`)**:
  - Main Deck (45 cards): 1x Wheel Synchron, 3x Stardust Synchron, 3x Junk Meister, 2x Junk Synchron, 1x Starjunk Synchron, 3x Ash Blossom & Joyous Spring, 3x Ghost Belle & Haunted Mansion, 3x Ghost Mourner & Moonlit Chill, 3x Ghost Ogre & Snow Rabbit, 1x Full-Speed Warrior, 1x Anchorbolt Hedgehog, 1x Assault Synchron, 3x Junk Converter, 1x Scrap Synchron, 1x Jet Synchron, 1x Crossroad Sonic Chick, 3x Effect Veiler, 3x Synchro Fellowship, 3x Tuning, 3x Junk Signal, 1x Scrap-Iron Sacred Statue, 1x Majestic Mirage.
  - Extra Deck (15 cards): Shooting Quasar Dragon, Crimson Dragon, Crimson Dragon Quetzacoatl, Shooting Star Dragon, Stardust Warrior, Satellite Warrior, Accel Synchro Stardust Dragon, Stardust Dragon, Stardust Dragon - Victim Sanctuary, Shooting Riser Dragon, Stardust Charge Warrior, Junk Speeder, Scrap Warrior, Formula Synchron, Majestic Star Dragon.
- **Architecture & Intelligent Executor (`Anime_YuseiExecutor.cs`)**:
  - **Synchro Fellowship Engine**: Main Phase search for Junk Synchron + Stardust Synchron / Junk Meister; GY banish effect reduces Synchro level by 1 and grants an additional Normal Summon for Synchron monsters.
  - **Junk Speeder Engine**: Level 5 Synchro engine that summons multiple Synchron tuners with different levels directly from deck (Stardust, Junk, Assault, Jet, Wheel), enabling immediate multi-branch Synchro climbing into Level 8, 10, and 12 bosses in a single turn.
  - **Shooting Quasar Dragon Boss**: 4000 ATK Level 12 ultimate boss with multi-attack, Omni-Negate + destroy, and floats into 3300 ATK `Shooting Star Dragon` upon leaving field.
  - **Crimson Dragon Tag-Out**: Quick effect tags out with target dragon of the same level to summon Dragon Synchro bosses without ordinary requirements.
  - **Satellite Warrior Board Break**: Clears opponent cards based on the number of Synchros in the GY (wiping 3-5 cards) while gaining +1000 ATK per destroyed card (pushing past 5500+ ATK).
  - **Defensive & Handtrap Grid**: Full suite of 15 handtraps (Ash, Belle, Mourner, Ogre, Veiler, Stardust Victim Sanctuary) combined with `Stardust Warrior` (negates Special Summons) and `Junk Signal` (negates responses to Synchro activations or cheats out Stardust/Junk).
- **Bot Registration & Deployment**: Registered as `Anime_Yusei` in `bots.json`; built and deployed exclusively to `C:\Users\admin\Documents\EdoGame\` via `BUILD_AND_DEPLOY.ps1`.

## 0.003. New Anime Series Rule-Based ModernExecutor: Anime_Yugi (2026-09-20)
- **Source & Concept**: Ported from YGOPRODeck Anime category (`Anime Battlebox - Yugi`). Features modern Yugi Mutos/Atem-inspired Black Chaos and Dark Magician Ritual/Fusion support.
- **Deck Composition (`Anime_Yugi.ydk`)**:
  - Main Deck (46 cards): 3x Black Chaos, 1x Dark Magician Pharaoh's Servant, 1x Skull Archfiend of Chaos, 1x Dark Magician Girl, 1x Detonating Kuriboh, 3x Griffoh, 3x Multiplying Kuriboh!, 1x BLS Soldier of Light and Darkness, 1x Magician of Dark Chaos Black Chaos, 1x Illusion of Chaos, 1x Pot of Prosperity, 3x Preparation of Rites, 1x Triple Tactics Talent, 1x Chaos Magical Hats, 1x Chaos Mystic Box, 3x Dark Magical Curtain, 3x Forbidden Crown, 1x Soul Servant, 1x Spell Shattering Sword (BETB), 1x The Gaze of Timaeus, 2x Swords of Concealing Light, 2x Light and Darkness Ritual, 3x Pre-Preparation of Rites, 1x Secrets of Dark Magic, 1x Chaos Space, 1x Dark Magic Talisman, 3x Dominus Impulse, 1x Mind Shuffle.
  - Extra Deck (15 cards): Timaeus the United Magical Dragon, Guardian Chimera, Dark Magician the Dragon Knight, Master of Chaos, Red-Eyes Dark Dragoon, Dark Cavalry, 3x Dark Magician of Destruction, The Dark Magicians, Magi Magi ☆ Magician Gal, Black Luster Soldier - Soldier of Chaos, Day-Breaker the Shining Magical Warrior, Linkuriboh, Ebon High Magician.
- **CDB & Asset Synchronization**:
  - Registered `Spell Shattering Sword` (`101402064` / `77456448`) across all game and WindBot `cards.cdb` databases.
  - Mirrored lua script `c101402064.lua` to `c77456448.lua` and pic `101402064.jpg` to `77456448.jpg`.
  - Fixed YGOPRODeck builder quirk where Normal Spell `Chaos Space` was incorrectly listed in Extra Deck by placing it cleanly into Main Deck.
  - **Full Asset Ingestion**: Downloaded high-resolution artwork and thumbnails from official CDN for all missing cards (`Chaos Magical Hats`, `Skull Archfiend of Chaos`, `Mind Shuffle`, `Magician of Dark Chaos - Black Chaos`, `Chaos Mystic Box`, `Griffoh`, `Black Chaos`). All 41 unique cards now have 100% full-size and thumbnail artwork in `pics/` and `pics/thumbnail/`.
  - **Comprehensive Thai Translation**: Translated 100% of untranslated card effects in `Anime_Yugi.ydk` into accurate, standard Thai Yu-Gi-Oh! terminology while preserving original English card names in `texts.name` to prevent any code or logic breaking. Synchronized across all CDB locations (`cards.cdb`, `config/languages/Thai/cards.delta.cdb`, `WindBot/cards.cdb`, etc.).
  - **Lua Script Bug Fixes**: Fixed `Attempting to access deleted object` in `c98684220.lua` (Black Chaos) by passing `Card` object directly to `SetLabelObject`; fixed `Parameter 2 should be 'Effect' but is 'nil'` in `c24088928.lua` (Skull Archfiend of Chaos) and `proc_workaround.lua` by passing `e` into `:Filter(Card.IsCanBeEffectTarget, nil, e)`.
- **Architecture & Intelligent Executor (`Anime_YugiExecutor.cs`)**:
  - **Griffoh Hand Disruption**: Hand Quick effect discards to search and Set `Mind Shuffle`, `Chaos Mystic Box`, `Chaos Magical Hats`, or `Spell Shattering Sword` from deck, activatable in the same turn.
  - **Pharaoh's Servant Engine**: Reveals spell in hand to Special Summon and Set DM Spell/Trap; Quick Effect discards spell to wipe all opponent backrow (Feather Duster).
  - **Dark Magician of Destruction**: Alternative Extra Deck Summon during a turn a Spell is activated by banishing Level 6+ DARK Spellcaster; searches DM support on summon.
  - **The Gaze of Timaeus Fusion**: Recycles DM/DMG from field or GY into deck to summon `Red-Eyes Dark Dragoon` (omni-negate + pop/burn), `Dark Magician the Dragon Knight` (backrow protection), or `Master of Chaos` (revives and banishes all enemy monsters).
  - **Light & Darkness Ritual**: Recursively rituals `Magician of Dark Chaos` (recovers Spells, face-down banishes opponent card) or `BLS Soldier of Light & Darkness` (non-targeting banish, +1500 ATK double attack); infinite recursion via GY return effect.
  - **Black Chaos Tower**: Shuffles back used Rituals to SS a 3000 ATK body immune to opponent activated effects (when Ritual Spell in GY) and ignitions a non-targeting double banish.
  - **Handtraps & Disruption**: Real-time chain interaction with `Dominus Impulse`, `Detonating Kuriboh`, `Forbidden Crown`, `Dark Magic Talisman`, and `Multiplying Kuriboh`.
- **Bot Registration & Deployment**: Registered as `Anime_Yugi` in `bots.json`; built and deployed exclusively to `C:\Users\admin\Documents\EdoGame\` via `BUILD_AND_DEPLOY.ps1`.

## 0.002. New Rule-Based ModernExecutor: 2026_DogmaStun (2026-09-20)
- **Deck Strategy**: Dogmatika Anti-Meta Stun hybrid combining Inspector Boarder, Dogmatika Ecclesia, Nadir Servant, Decisive Battle of Golgonda, The Fallen & The Virtuous, Necrovalley, Super Polymerization, Card of Demise, Pot of Prosperity, and heavy Counter Traps (Solemn Strike, Solemn Judgment, Crackdown, Skill Drain, Dogmatika Punishment).
- **Architecture**: Implemented in `_2026_DogmaStunExecutor.cs` inheriting `ModernExecutor`.
  - Registered `ResourcePlan.RegisterAceCards` for Inspector Boarder, The Dragon that Devours the Dogma, Starving Venom Fusion Dragon, S:P Little Knight, TY-PHON.
  - Registered `BaitPlanner.RegisterComboStarters` (Pot of Prosperity, Terraforming, Nadir Servant).
  - Registered `ChainAdvisor.RegisterHighValueTargets` (Inspector Boarder, Skill Drain, Necrovalley).
  - Registered `_optionalFieldRemovalCards` for `TheFallenAndTheVirtuous` to prevent self-destruction on empty enemy fields.
  - Smart Extra Deck dumping for `DogmatikaPunishment` & `NadirServant` (N'tss for 2nd pop, Garura for draw, Devours Dogma for 3000 ATK send & End Phase search, Titaniklad/Albion for End Phase search/set).
  - Unrespondable `SuperPolymerization` logic targeting Starving Venom, Mudragon, or Garura using materials across both fields.
  - High-IQ disruption timing: `SolemnJudgment` prioritizes board-wipes (`Harpie's`, `Lightning Storm`, `Raigeki`, `Evenly`) and major starters (`Branded Fusion`); `SolemnStrike` targets Extra Deck summons and Chokepoint/Negator activations; `DogmatikaPunishment` and `Crackdown` intercept on-field monster effects, Extra Deck climbs, and Battle Phase pushes.
  - Harmonized `CardOfDemise` and `PotOfProsperity` to prevent conflicting activation in the same turn, ensuring all Traps are deployed before Demise.
- **Deck & Bot Registration**: Copied `2026_DogmaStun.ydk` to both `windbot-fork\Decks` and `WindBot\Decks`; registered `2026_DogmaStun` in `bots.json`.
- **Exclusive Deployment**: Built and deployed to `C:\Users\admin\Documents\EdoGame\` via `BUILD_AND_DEPLOY.ps1`.

## 0.001. Non-Legacy Archetypes Comprehensive Optimization & Engine Fixes (2026-09-20)
- **cards.cdb Merge**: 174 missing 2026 cards merged from expansions to SQLite; resolved `2026_Angelechy` missing card failure.
- **bots.json Path Fix**: Aligned 4 broken deck path filenames (`Rank5`, `Level8`, `ToadallyAwesome`, `ZexalWeapons`).
- **ModernExecutor Central Protection**:
  - Registered `_optionalFieldRemovalCards`: blocks self-board wipes via `OnSelectYesNo` when enemy has 0 cards.
  - Hardened `GetMaterialSacrificePriority`: Ace cards are assigned priority 100000 to prevent sacrificing boss monsters as link/xyz materials.
- **Archetype Fixes & Modernization**:
  - `2026_Maliss`: Allowed 1-card Link climbing into Link-1 (Link Disciple / Linguriboh) from expendable Maliss monsters.
  - `2026_Fireking`: Prevented Fire King Island from wiping bot's field; guarded Kirin from self-destructing with no enemy cards; prioritized enemy targets on destroy hint.
  - `2026_Exosister`: Enforced strict Martha Xyz condition + Elis in deck check; fixed Magnifica field-only banish; added Mikailis GY disruption.
  - `2026_KaijuCrusadia`: Implemented `IsAceCard` override, ComboRouter lines, and `IsBoardStrongEnough`.
  - `2026_Tearla`: Integrated ComboRouter sequencing (Reinoheart-Kitkallos & Fiendsmith lines), BaitPlanner, and ChainAdvisor.
  - `2026_Yummy`: Integrated ComboRouter lines (Marshmao-Cupsy & Cooky-Lollipo-Borreload), BaitPlanner, and ChainAdvisor.
  - `2026_Purrely`: Integrated BaitPlanner, ChainAdvisor, and ResourcePlan.
  - `2026_Stun`: Registered ResourcePlan, BaitPlanner, ChainAdvisor, and OptionalFieldRemoval for The Fallen & The Virtuous.
  - Blind `cards[0]` selection replaced with smart target/threat selection in Archfiend, Dreadnought, Regenesis, Darklord, and Doomz.
- **Deployment**: Compiled and deployed exclusively to `C:\Users\admin\Documents\EdoGame\`.

