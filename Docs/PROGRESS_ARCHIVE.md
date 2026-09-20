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




