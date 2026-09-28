# Progress Log: Central Core Architecture & Universal Heuristics Overhaul

## 0.056. Pendulum Magician (Supreme King Z-ARC & Synchro/Xyz/Link Engine) Architecture & Optimization (2026-09-28)

### 1. Human vs Bot Comparative Analysis & Deck Structure
- **Raw List Audit (55 cards dumped under Monster)**:
  - Deconstructed user's raw dump: revealed 40 Main Deck cards (30 Monsters, 9 Spells, 1 Trap) + 15 Extra Deck cards (1 Fusion, 6 Synchro, 5 Xyz, 3 Link).
  - Identified major AI execution bottlenecks & high cognitive-load traps:
    - *Artifact Dagda + Artifact Scythe + Halqifibrax + T.G. Wonder Magician*:
      - *Human*: Holds Halq for opponent's Main Phase 1, chains to summon Wonder Magician, selectively pops their own face-down Scythe to lock opponent from Extra Deck.
      - *Bot*: Prone to premature tag-outs (Draw/Standby phase), heuristic targeting prioritizes opponent backrow instead of self-Scythe, and Scythe drawn in hand is a complete brick. Both Halq and Scythe are banned in modern formats.
      - *Optimization*: Replaced with autonomous, deterministic omni-negates & board breakers: Heavymetalfoes Electrumite, Beyond the Pendulum, Odd-Eyes Absolute Dragon -> Odd-Eyes Vortex Dragon, Baronne de Fleur, and Ignister Prominence.
    - *Tuning Magician (3 copies) + Linkuriboh + Magicians' Souls*:
      - *Human*: Exploits Souls to cycle spells/traps and Tuning Magician for recurrent level 1 Tuner.
      - *Bot*: Bricks hand without Halqifibrax; Souls lacks Level 6+ normal spellcasters to summon itself; Tuning Magician inflicts 400 self-burn and heals opponent. Cut Souls/Linkuriboh and replaced with high-density handtraps (Ash Blossom, Maxx "C", Called by the Grave, Infinite Impermanence).
    - *Pendulum Call vs Wisdom-Eye Magician*:
      - *Human*: Never triggers Pendulum Call before Wisdom-Eye has destroyed itself to set a scale (due to Pendulum Call's lingering protection preventing scale destruction).
      - *Bot*: Added strict sequencing guard: Wisdom-Eye scale pop is evaluated and executed prior to activating Pendulum Call.
    - *Scale Mismatch & Harmonizing Misplacement*:
      - Built `PendulumMagicianScaleResolver` to guarantee pairing Scale 1 (Purple Poison, White Wing, Astrograph) with Scale 8 (Double Iris, Black Fang, Dragonpit), and strictly prohibited placing Harmonizing Magician into the Pendulum Zone.
    - *Allure of Darkness Safety*:
      - Strictly required `Bot.Hand.Count(c => c.IsMonster() && c.HasAttribute(CardAttribute.Dark)) >= 1` before activating Allure to prevent hand-wipe.

### 2. C# Rule-Based Architecture & Decoupled Domain Plugin
- **`PendulumMagicianPlugin.cs`**:
  - `PendulumMagicianStrategy`: Guides search priority (Wisdom-Eye/scales -> Harmonizing -> Double Iris -> Pendulumgraph -> Astrograph) and Harmonizing special summon targets.
  - `PendulumMagicianScaleResolver`: Implements `IDeckScaleResolver` (`PickLowScale`, `PickHighScale`, `PickScalePopTarget`).
  - `PendulumMagicianMaterialEvaluator`: Protects Ace Bosses (Z-ARC, Crystal Wing, Vortex, Baronne, Apollousa, Timestar) with material cost >= 8000; optimizes Pendulum Call discards and Timestar destruction replacement from Deck.
  - `PendulumMagicianThreatEvaluator`: Evaluates threat score for anti-pendulum floodgates (Anti-Spell Fragrance, Skill Drain) and negators.
- **`PendulumMagicianExecutor.cs`**:
  - Full `ModernExecutor` implementation featuring:
    - *Electrumite + Astrograph Loop*: Link summons Electrumite -> sends Astrograph to Extra Deck -> pops scale to add Astrograph -> triggers Astrograph special summon and search.
    - *Absolute -> Vortex Dragon Engine*: Overlays 2 Level 7s (Astrograph + Dragonpit) -> links into S:P/Beyond -> triggers Absolute in GY to summon Vortex Dragon for omni-negate.
    - *Supreme King Z-ARC Board Wipe*: Monitors presence of the 4 Dragon Magicians (Double Iris, White Wing, Black Fang, Purple Poison); activates Astrograph/Chronograph on field to banish them and summon Z-ARC.
    - *Timestar Magician + Time Pendulumgraph Combo*: Employs Timestar's deck-send replacement to trigger Time Pendulumgraph's non-targeting send to GY.
- **Registration & Deployment**:
  - Registered in `windbot-fork/bots.json` as `"PendulumMagician"` and `"SupremeMagician"`.
  - Added clean 40-card Main + 15-card Extra deck: `PendulumMagician.ydk`.
  - Compiled and deployed cleanly to `C:\Users\admin\Documents\EdoGame\` via `BUILD_AND_DEPLOY.ps1`.

---

## 0.055. Anime_Sayer & Anime_Kaiba ModernExecutor Architecture & Decoupled Domain Plugin Refactor (2026-09-28)

### 1. Human vs Bot Architectural Analysis & Card Structure
- **Anime_Sayer (Sayer's Ultimate Psychic Synchro Battlebox)**:
  - *Main Deck Core (40 cards)*: Overdrive Teleporter (x1), Master Gig (x1), Armored Axon Kicker (x1), Silent Psychic Wizard (x2), Psychic Snail (x1), Psi-Blocker (x1), Psychic Commander (x2), Psychic Wheeleder (x3), Psychic Tracker (x3), Hushed Psychic Minister (x2), Mind Procedure (x1), Ghost Ogre & Snow Rabbit (x3), Krebons (x3), Serene Psychic Girl (x2), Psychic Jumper (x1), Emergency Teleport (x3), Brain Research Lab (x3), Terraforming (x1), Psychokinesis (x2), Brain Control (x1), Telekinetic Power Well (x1), Mind Over Matter (x2), Psychic Overload (x2).
  - *Extra Deck (15 Synchros)*: Psychic End Punisher (x1), Overmind Archfiend (x1), Psychic Blaster Mk-II (x1), Hyper Psychic Blaster (x1), PSY-Framelord Omega (x1), Thought Ruler Archfiend (x1), Psychic Omnibuster (x1), Psychic Lifetrancer (x1), Serene Psychic Sorceress (x1), HTS Psyhemuth (x1), Hyper Psychic Riser (x1), Psychic Nightmare (x1), Mind Castlin (x1), Magical Android (x2).
  - *Human Pilot Mindset*: Human players balance steep LP costs (Overdrive Teleporter 2000, Mind Over Matter tribute, Master Gig 1000, Psychokinesis 1000, PEP 1000) using Brain Research Lab's counter replacement or LP recovery (Magical Android, Thought Ruler, Lifetrancer), while actively exploiting Psychic End Punisher's activated effect immunity when LP <= opponent.
  - *Bot Execution Logic*: Built `Anime_SayerPlugin` (`IDeckStrategy`, `IDeckMaterialEvaluator`, `IDeckThreatEvaluator`). Added dynamic LP safety gate (`Bot.LifePoints > 2000 || HasBrainResearchLab()`), strictly prevented `Psychic End Punisher` from banishing itself when alone unless facing lethal threats, and enforced Tuner/Non-Tuner balance to prevent board lock.
- **Anime_Kaiba (Seto Kaiba's Blue-Eyes Jet & Ultimate Dragon Engine)**:
  - *Main Deck Core (40 cards)*: Blue-Eyes White Dragon (x3), Blue-Eyes Alternative White Dragon (x2), Blue-Eyes Jet Dragon (x2), Blue-Eyes Abyss Dragon (x1), Dictator of D. (x3), Sage with Eyes of Blue (x2), The White Stone of Ancients (x3), The White Stone of Legend (x1), Ash Blossom (x3), Effect Veiler (x3), Nibiru (x1), The Melody of Awakening Dragon (x3), Trade-In (x3), Dragon Shrine (x1), Foolish Burial (x1), Triple Tactics Talent (x1), Called by the Grave (x1), Ultimate Fusion (x2), True Light (x2), Infinite Impermanence (x3).
  - *Extra Deck (15 cards)*: Neo Blue-Eyes Ultimate Dragon (x1), Blue-Eyes Tyrant Dragon (x1), Blue-Eyes Twin Burst Dragon (x1), Blue-Eyes Spirit Dragon (x2), Azure-Eyes Silver Dragon (x1), Michael the Arch-Lightsworn (x1), Black Rose Moonlight Dragon (x1), Number 38: Hope Harbinger (x1), Number 90: Galaxy-Eyes Photon Lord (x1), Number 97: Draglubion (x1), Number 100: Numeron Dragon (x1), Hieratic Seal of the Heavenly Spheres (x1), Relinquished Anima (x1), S:P Little Knight (x1).
  - *Human Pilot Mindset*: Going first sets up Spirit Dragon + Jet Dragon + True Light or Hope Harbinger/Photon Lord. Going second breaks boards via Draglubion -> Numeron Dragon (9000-17000 ATK OTK), Alternative pop, or Neo Ultimate triple attack. Discards White Stones for GY recursion.
  - *Bot Execution Logic*: Built `Anime_KaibaPlugin` (`IDeckStrategy`, `IDeckMaterialEvaluator`, `IDeckThreatEvaluator`). Enforced Rule 350 on `S:P Little Knight` (restricted to Main Phase 2, strictly prohibiting sacrificing 2500+ ATK Dragons or locking direct attacks in MP1). Safeguarded `True Light` with `Jet Dragon` board presence awareness and prioritized discard of `The White Stone of Ancients/Legend`.

### 2. C# Rule-Based Architecture & Decoupled Domain Plugin
- Created `Anime_SayerPlugin.cs` in `Game/AI/Plugins/`
- Created `Anime_KaibaPlugin.cs` in `Game/AI/Plugins/`
- Refactored `Anime_SayerExecutor.cs` & `Anime_KaibaExecutor.cs` with full decoupled domain integration and anti-pattern guards.
- Cleanly compiled and deployed to `C:\Users\admin\Documents\EdoGame\` via `BUILD_AND_DEPLOY.ps1`.

---

## 0.054. Water Monarch (Goat Format) ModernExecutor Architecture & Human vs Bot Alignment (2026-09-28)

### 1. Human vs Bot Architectural Analysis & Deck Optimization
- **Raw List Audit (70 cards lumped under Monster)**:
  - Deconstructed user's raw dump: revealed 40 Main Deck cards + 15 Extra/Fusion Deck cards + 15 Side Deck cards.
  - Identified critical AI execution bottlenecks in human deck construction:
    - *Des Wombat (3 copies)*: Passive effect damage prevention with 1600 ATK body. For AI bot against general decks, normal summoning a 1600 vanilla beater lowers pressure. Moved to Side Deck.
    - *Ninja Grandmaster Sasuke (2 copies)*: Face-up DEF destroyer. In AI decision loops, setting up attack-destruction triggers without human anticipation causes sub-optimal battle phase attacks. Moved to Side Deck.
    - *Trap Dustshoot (3 copies)*: Strict Turn 1 / Standby Phase timing requiring opponent hand >= 4. Highly dead topdeck in mid/late game. Moved to Side Deck.
    - *Tsukuyomi TER Loop Optimization*: Added Tsukuyomi to main deck to execute the classic Goat TER soft-reset loop (flip TER face-down to destroy absorbed monster, flip summon to suck a new monster).
    - *Torrential Tribute*: Added staple board wipe to handle opponent swarms.
- **AI Bot Optimization (40 Main / 15 Extra / 15 Side Classic Goat Standard)**:
  - *Main Deck Core (40 cards)*: 2x Mobius the Frost Monarch, 1x Zaborg the Thunder Monarch, 3x Mother Grizzly, 1x Sinister Serpent, 1x Yomi Ship, 3x Gravekeeper's Spy, 3x D.D. Assailant, 1x Breaker the Magical Warrior, 1x Sangan, 1x Tribe-Infecting Virus, 2x Magician of Faith, 1x Morphing Jar, 1x Tsukuyomi, 1x Pot of Greed, 1x Graceful Charity, 1x Delinquent Duo, 1x Snatch Steal, 1x Premature Burial, 1x Heavy Storm, 1x Mystical Space Typhoon, 2x Nobleman of Crossout, 3x Book of Moon, 3x Scapegoat, 2x Metamorphosis, 1x Mirror Force, 1x Torrential Tribute, 1x Ring of Destruction, 1x Call of the Haunted, 1x Bottomless Trap Hole, 1x Solemn Judgment.
  - *Extra Deck (15 Fusion monsters)*: 3x Thousand-Eyes Restrict, 1x Dark Balter the Terrible, 1x Fiend Skull Dragon, 1x Dark Flare Knight, 1x Dark Blade the Dragon Knight, 1x Mystical Sand, 1x King Dragun, 1x Cyber Twin Dragon, 1x Gatling Dragon, 1x Dark Paladin, 1x Arcana Knight Joker, 1x Cyber End Dragon, 1x Blue-Eyes Ultimate Dragon.
  - *Side Deck (15 cards)*: 3x Des Wombat, 2x Ninja Grandmaster Sasuke, 2x Kycoo the Ghost Destroyer, 3x Trap Dustshoot, 2x Sakuretsu Armor, 2x Horn of Heaven, 1x Thousand-Eyes Restrict.

---

### 2. C# Rule-Based Architecture & Decoupled Domain Plugin
- **`WaterMonarchPlugin.cs`**:
  - `WaterMonarchStrategy`: Implements `PickSearchTarget` (Sinister Serpent -> Tsukuyomi -> Magician of Faith -> Yomi Ship -> Tribe -> Assailant), `PickSpecialSummonTarget` (Mobius/Zaborg for push, hooks `PickGrizzlyTarget` on Mother Grizzly battle float).
  - `WaterMonarchMaterialEvaluator`: High tribute protection for Mobius/Zaborg (cost 40,000) and TER with equip (cost 30,000); low cost for Sheep Tokens (5), Sinister Serpent (10), flipped Magician of Faith (20), flipped Gravekeeper's Spy (30); Discard target priority strictly sends Sinister Serpent for free card advantage.
  - `WaterMonarchThreatEvaluator`: Evaluates threat score for high ATK beaters, Jinzo (trap blocker), BLS, and TER.
- **`WaterMonarchExecutor.cs`**:
  - Full `ModernExecutor` implementation with:
    - *Tsukuyomi Soft-Reset Loop*: Automatically detects TER with equipped monster, flipped Magician of Faith, or flipped Spy to flip down and recycle.
    - *Monarch Tribute Discipline*: Mobius requires opponent S/T >= 1; Zaborg requires opponent monsters >= 1; both select lowest-cost fodder via `GetTributeFodder()`.
    - *Tribe-Infecting Virus Board Wipe*: Scans opponent board, determines most populous monster race, calls `AI.SelectRace`, and discards Sinister Serpent.
    - *OnSelectCard Integration*: Explicitly routes SPSUMMON, ATOHAND, DISCARD, RELEASE, and REMOVAL to `WaterMonarchPlugin`.
- **DashBot & Deployment**:
  - Registered `"WaterMonarch"` into `GoatArchetypes` and `CleanDeckDisplayName` in `dashbot/MainWindow.xaml.cs` (appears under GOAT category filter with Emerald Green badge).
  - Registered in `bots.json` under clean names `"WaterMonarch"` and `"GOAT_WaterMonarch"` with Master Rules 1-5 support.
  - Deployed cleanly via `BUILD_AND_DEPLOY.ps1` to `C:\Users\admin\Documents\EdoGame\`.

---

## 0.053. OCGCore Lua Garbage Collection Bugfix & Mermail Atlantean ModernExecutor Architecture (2026-09-28)

### 1. OCGCore Lua Crash Root Cause Analysis & Resolution
- **Error in Screenshot**: `[สคริปต์การ์ดผิดพลาด]: [string "c70088809.lua"]:72: Attempting to access deleted object.`
- **Root Cause**:
  - `c70088809.lua` (`Fydraulis Harmonia`, Card ID: 70088809) effect 1 cost (`effcost`) created a temporary Lua `Group` (`Duel.SelectMatchingCard(tp, s.revealfilter, tp, LOCATION_EXTRA, 0, 1, 2, nil)`) and stored it into `e:SetLabelObject(g)` / chain data (`cd.revealed_synchros = g`).
  - In OCGCore / ygopro C++ engine, transient `Group` pointers created within cost resolution are automatically freed/reclaimed by the internal Lua garbage collector when the cost function stack returns.
  - When effect operation (`effop`) resolved at line 72, invoking `cd.revealed_synchros:GetCount()` attempted to dereference the already deleted C++ pointer, throwing the runtime crash `Attempting to access deleted object.`
- **Resolution**:
  - Replaced the deleted object access in `effop` with `Duel.GetMatchingGroup(Card.IsRelateToEffect, tp, LOCATION_EXTRA, 0, nil, e)` and safe count checks with direct Level extraction via `Card.GetLevel`.
  - Replicated fix across all 6 script locations: `script/c70088809.lua`, `script/official/c70088809.lua`, `src/YGO_SOURCE_CLEAN/script/c70088809.lua`, `repositories/delta-bagooska/...`, `repositories/official-scripts/...`, and `repositories/local-patches/...`.

---

### 2. Mermail / Atlantean Human vs Bot Alignment & Deck Optimization
- **Raw List Audit (74 cards lumped under Monster)**:
  - Deconstructed user's raw dump: revealed 58 Main Deck cards + 16 Extra Deck cards (exceeding 15 Extra limit).
  - Identified critical AI execution bottlenecks in human deck construction:
    - *Mulcharmy Fuwalos & Purulia (5 copies)*: Require empty field to activate. Once the bot summons Neptabyss or sets cards, they become dead weight. Going first, they offer zero interaction.
    - *Anti-Magic Arrows (3 copies)*: Battle Phase spell locks that are completely dead going first and unusable by AI combo routines.
    - *Pot of Sloth (2 copies)*: Excessively slow recovery spell with high activation requirements.
    - *Neo-Spacian Aqua Dolphin (1 copy)*: Requires discard and exact ATK comparisons; low utility in pure WATER combo lines.
    - *Testudo erat Numen (1 copy)*: Locks both players from special summoning 1800+ ATK monsters; heavily disrupts the AI's own boss summons.
    - *Excess First Penguin (3 copies)*: High brick risk in opening hand.
- **AI Bot Optimization (43 Main / 15 Extra Tournament Standard)**:
  - Streamlined Main Deck (43 cards):
    - *Core Atlantean Starters & Dual-Search Engine*: 3x `Neptabyss, the Atlantean Prince`, 3x `Atlantean Dragoons`, 1x `Atlantean Heavy Infantry`, 2x `Poseidra, the Storming Atlantean`.
    - *Mermail Extenders & Discard Triggers*: 3x `Mermail Abyssteus`, 3x `Abyssrhine, the Atlantean Spirit`, 2x `Mermail Abysspike`, 1x `Mermail Abyssocea`, 3x `Mermail Shadow Squad`.
    - *Power Extenders & Hand Rip*: 1x `Moulinglacia the Elemental Lord` (2-card hand rip), 1x `Gluttonous Reptolphin Greethys`, 1x `Deep Sea Minstrel`, 1x `Superancient Deepsea King Coelacanth`, 1x `First Penguin`, 1x `Butterfly Fish`.
    - *Interruption & Board Control Suite*: 3x `Dominus Impulse`, 3x `Infinite Impermanence`, 3x `Ash Blossom & Joyous Spring`, 2x `D.D. Crow`, 2x `Droll & Lock Bird`, 2x `Forbidden Crown`, 1x `Called by the Grave`, 1x `Crossout Designator`, 1x `One for One`.
  - Streamlined Extra Deck (15 cards):
    - *Xyz*: 1x `Poseidra Abyss, the Atlantean Dragon Lord` (Rank 7), 1x `Mermail Abyssgaios` (Rank 7), 1x `LeVirtue Dragon` (Rank 3), 1x `Abysstrite, the Atlantean Spirit`.
    - *Synchro*: 1x `Adamancipator Risen - Dragite` (Spell/Trap negate), 1x `Icejade Gymir Aegirine` (Board immunity + banish), 1x `Swordsoul Supreme Sovereign - Chengying`, 1x `Trishula, Dragon of the Ice Barrier`, 1x `White Aura Monoceros`, 1x `Deep Sea Prima Donna`, 1x `Deep Sea Repetiteur`.
    - *Link*: 1x `Mermail King - Neptabyss` (Link 3), 1x `Marincess Coral Anemone` (Link 2), 1x `Mistar Boy` (Link 2), 1x `Haggard Lizardose` (Link 2).

---

### 3. C# Rule-Based Architecture & Decoupled Domain Plugin
- **`MermailAtlanteanPlugin.cs`**:
  - `MermailAtlanteanStrategy`: Prioritizes Neptabyss -> Dragoons dual-search line, Level 7 Xyz (Poseidra Abyss / Abyssgaios) going 1st, and Dragite / Chengying synchro lines.
  - `MermailAtlanteanMaterialEvaluator`: Protects key boss pieces and engine pieces; prioritizes Dragoons/Shadow Squad as discard costs to trigger free GY searches/SS.
  - `MermailAtlanteanThreatEvaluator`: Evaluates monster effect negates, S/T destruction threats, and opponent high ATK bodies.
- **`MermailAtlanteanExecutor.cs`**:
  - Full `ModernExecutor` implementation with:
    - *Neptabyss Search Loop*: Sends Dragoons from Deck to GY as cost, searching Atlantean monster + Dragoons GY trigger searching SEA SERPENT.
    - *Abyssrhine & Abyssteus Engines*: Discards WATER to special summon and search Level 4 or lower Mermail while triggering Atlantean discard effects.
    - *End Board Disruption*: Poseidra Abyss bounce-3, Abyssgaios continuous monster effect negate, Dragite S/T negate, Gymir Aegirine board protection, and Dominus Impulse / Impermanence traps.
- **`bots.json` & Deployment**:
  - Registered `MermailAtlantean`, `2026_MermailAtlantean`, `Mermail`, and `PoseidraAbyss`.
  - Compiled and published cleanly with 0 Errors; deployed directly to `C:\Users\admin\Documents\EdoGame\`.

---

## 0.052. Elfnote / Power Patron Tournament Synchro Architecture & Human vs Bot Alignment (2026-09-28)

### 1. Human vs Bot Architectural Analysis & Deck Optimization
- **Raw List Audit (71 cards lumped under Monster)**:
  - Deconstructed user's raw dump: revealed 56 Main Deck cards + 15 Extra Deck cards.
  - Identified severe consistency and execution issues for AI bot:
    - *PSY-Framegear Gamma / Delta + Driver*: Required empty field; bot almost always puts an Elfnote on board or normal summons, permanently bricking Gamma/Delta. Driver is a 2500 ATK normal monster brick.
    - *Mulcharmy Fuwalos & Purulia (6 copies)*: Emptiness restriction, 1-per-turn lockout, and massive hand overflow. Drawing 2-3 copies going 1st bricks the opening hand.
    - *Artifact Lancea (3 copies)*: Requires human metagame anticipation in Draw/Standby Phase. For a bot, shotgunning or missing the window makes it a dead 1700 body.
    - *Sphere Mode (3 copies)*: Normal Summon consumption, completely dead Going 1st, unable to break 1-2 monster end boards.
    - *Critical Missing Piece*: `Junora the Power Patron of Tuning` (ID: 5914858) was missing from the user's Extra Deck, preventing `Junordo` and `Elfnote Power Patron` from summoning their chief board-negating boss!
- **AI Bot Optimization (43 Main / 15 Extra)**:
  - Streamlined Main Deck to 43 cards: 3x Lucina, 3x Regina, 3x Tinia, 2x Fortuna, 3x Elfnote Power Patron, 2x Junordo, 2x Medius, 1x Vidrium, 2x Harmonia, 3x Ash Blossom, 1x Druiswurm, 1x Magnamhut, 3x Terminus, 2x Welcome Home, 1x Past Lull, 1x Called by the Grave, 1x Triple Tactics Talent, 1x Rhapsodia, 1x Aristeia, 2x Solemn Judgment, 3x Infinite Impermanence, 1x Anti-Spell Fragrance, 1x Synchro Emergency.
  - Complete 15 Extra Deck: `Elfnote June Pride`, `Junora the Power Patron of Tuning`, `Elfnote Seraphim Strelitzia`, `Arms of Genex Return Zero`, `Chaos Angel`, `CrystalWingSynchroDragon`, `StardustDragonVictimSanctuary`, `AccelSynchroStardustDragon`, `StardustDragon`, `FADawnDragster`, `WindPegasusIgnister`, `GoldenCloudBeastMalong`, `BlackRoseDragon`, `StardustWarrior`, `PurificationPowerPatron`.

### 2. Implementation & Domain Decoupling
- **`ElfnotePlugin.cs`**: Implemented `DeckPluginBase` decoupling `ElfnoteStrategy` (`IDeckStrategy`), `ElfnoteMaterialEvaluator` (`IDeckMaterialEvaluator`), and `ElfnoteThreatEvaluator` (`IDeckThreatEvaluator`).
- **`ElfnoteExecutor.cs`**: Full `ModernExecutor` implementation with:
  - Center-Zone Placement Logic (`OnSelectPlace` priority for Zone 2 sequence).
  - Deterministic option routing for `Medius the Pure` (Option 1 SS) and `Triple Tactics Talent`.
  - Disruption matrix on opponent turn: Lucina (monster bounce), Tinia (hand rip), Fortuna (S/T bounce), Rhapsodia (face-up negate), Aristeia (S/T negate & destroy), Harmonia (field destroy + extra deck dump to GY), Victim Sanctuary (chain response negate & destroy).
- **Registration & Deployment**:
  - Registered in `bots.json` under names `"Elfnote"`, `"2026_Elfnote"`, and `"ElfnotePowerPatron"`.
  - Created canonical deck list `Elfnote.ydk` in `windbot-fork/Decks/`, `deck/`, and `WindBot/Decks/`.
  - Built and deployed via `BUILD_AND_DEPLOY.ps1` with 0 Errors to `C:\Users\admin\Documents\EdoGame\`.
  - In accordance with testing policy and user request ("เดี๋ยวผมทดสอบเอง"), Headless Simulator was not executed.

---

## 0.049. Binary Protocol Disassembly & Universal SelectCounter Engine Fix (2026-09-26)

### Breakthrough Discovery: OCGCore MSG_SELECT_COUNTER Binary Protocol Misalignment
A critical root-cause bug in the foundational network layer of WindBot (`GameBehavior.cs`) was uncovered through binary disassembly of `ocgcore.dll` (at offset `0x10079ba0` - `0x10079e86`):

1. **OCGCore Binary Protocol Structure**:
   - `0x10079d5b`: `write_buffer(player, 1)` $\rightarrow$ **1 byte** (`byte`)
   - `0x10079d6e`: `write_buffer(type, 2)` $\rightarrow$ **2 bytes** (`int16`)
   - `0x10079d85`: `write_buffer(quantity, 2)` $\rightarrow$ **2 bytes** (`int16`)
   - `0x10079d9e`: `write_buffer(count, 4)` $\rightarrow$ **4 bytes** (`int32`)
   - Loop `1..count` at `0x10079df0`:
     - `0x10079e00`: `cardId` (4 bytes, `int32`)
     - `0x10079e19`: `player` (1 byte, `byte`)
     - `0x10079e32`: `loc` (1 byte, `byte`)
     - `0x10079e4b`: `seq` (1 byte, `byte`)
     - `0x10079e66`: `available_counters` (2 bytes, `int16`)

2. **The Flaw in WindBot's Implementation**:
   In `GameBehavior.cs`, `OnSelectCounter` was implemented as:
   ```csharp
   int type = packet.ReadInt16();     // 2 bytes
   int quantity = packet.ReadInt32(); // 4 bytes (WRONG: read quantity + lower 2 bytes of count!)
   int count = packet.ReadByte();     // 1 byte (WRONG: read upper byte of count!)
   ```
   - When 2 cards had counters on the field (e.g. Gateway + Dojo, or Citadel + Servant):
     - `quantity = (count << 16) | real_quantity` $\rightarrow$ `(2 << 16) | 4 = 131076`!
     - `count` was read from the high zero-byte $\rightarrow$ `count = 0`!
     - The cards list was empty, returning `payload=[]` (sum = 0 / 131076).
     - OCGCore compared `cx != ax` at `0x10079f2e`, emitted `MSG_RETRY`, WindBot disconnected, and EDOPro threw GUI modal **`"เกิดข้อผิดพลาด!"`**.

3. **Definitive Fix Applied**:
   - Corrected `quantity` to `packet.ReadInt16()` (2 bytes).
   - Corrected `count` to `packet.ReadInt32()` (4 bytes).
   - Validated live via session log:
     ```text
     [OnSelectCounter] type=0x3, quantity=4, count=2
     [OnSelectCounter] Response: sum=4/4, payload=[1,3]
     ```
     Zero retries, zero disconnects, and 100% protocol adherence!

---

### Audit & Hardening Scope Across All 7 Dedicated Plugin Decks
Following the Endymion `OnSelectCounter` investigation, a comprehensive audit was executed across all 7 decks in the codebase that implement dedicated/custom domain modules (`*Plugin`):

1. **`Endymion` (`_2026_EndymionExecutor.cs` & `EndymionPlugin`)**:
   - **Root Cause Verified**: Duplicate counter tracking (`_cardCounters` dictionary in executor vs `_trackedCounters` in `EndymionCounterEconomy`) caused desynchronization. In addition, `GameBehavior.OnSelectCounter` discarded card ID packets, causing null lookups when selecting counters on newly summoned/moved cards.
   - **Hardening**:
     - Removed redundant `_cardCounters` dictionary completely; all counter lookups, additions, and removals now query `Plugin.CounterEconomy` as the single source of truth.
     - Added `c.Location == CardLocation.MonsterZone` check for monster-effect counter generation (Jackal King, Master Cerberus).
     - Guarded `GravityController` Special Summon to strictly require Extra Monster Zone sequence (`m.Sequence == 5 || m.Sequence == 6`).
     - Added strict opponent board requirement for `MightyMasterBoardBreak` (`Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0`), stopping Turn 1 counter-draining against empty fields.
     - Headless simulation verified: 8-turn full match vs `DarkMagician` with 0 Violations and 0 Crashes.

2. **`Six Samurai` (`_2026_SixSamuraiExecutor.cs` & `SixSamuraiPlugin`)**:
   - **Issue Found**: `OnSelectCounter` directly accessed `Plugin.CounterEconomy.SelectCounters` without null-propagation and lacked length-match safety guards on the input lists.
   - **Hardening**:
     - Added null-safe fallback: `Plugin?.CounterEconomy?.SelectCounters(cards, counters, quantity) ?? base.OnSelectCounter(cards, counters, quantity)`.
     - Added defensive list boundary guard: `if (cards == null || counters == null || cards.Count != counters.Count) return null;`.
     - Headless simulation verified: full 3-game match (2-1 win rate) with 0 Violations and 0 Crashes.

3. **`D/D/D` (`_2026_DDDExecutor.cs` & `DDDPlugin`)**:
   - **Issue Found**:
     - `OnSelectSynchroMaterial` returned `sorted.Take(max)`, completely ignoring the exact `sum` (Level) requirement specified by OCGCore. This caused illegal material selection and engine level-mismatch violations.
     - `OnSelectFusionMaterial` and `OnSelectXyzMaterial` returned `sorted.Take(max)` instead of `sorted.Take(min)`, unnecessarily consuming excess materials.
   - **Hardening**:
     - Delegated `OnSelectSynchroMaterial` to `base.OnSelectSynchroMaterial(cards, sum, min, max)` to leverage the exact subset-sum level solver.
     - Fixed `OnSelectFusionMaterial` and `OnSelectXyzMaterial` to select `sorted.Take(min)` with graceful fallbacks when `cards.Count < min`.
     - Headless simulation verified: 100% win rate (5 turns) vs `DarkMagician` with 0 Violations.

4. **`Morganite Stun` (`MorganiteStunExecutor.cs` & `MorganiteStunPlugin`)**:
   - **Issue Found**: In `OnSelectCard`, returning `new List<ClientCard> { card }` without checking `min <= 1 && 1 <= max` posed violation risks if OCGCore requested multi-card selection.
   - **Hardening**: Wrapped single-card selections with `if (min <= 1 && 1 <= max)` and null-safe plugin access.

5. **`Drytron Tour` (`DrytronTourExecutor.cs` & `DrytronTourPlugin`)**:
   - **Issue Found**: Single-card search/mill returns in `OnSelectCard` lacked `min <= 1 && 1 <= max` boundary guards.
   - **Hardening**: Wrapped all single-card returns with `if (min <= 1 && 1 <= max)` and null-safe plugin access.

6. **`Madolche` (`MadolcheExecutor.cs` & `MadolchePlugin`)**:
   - **Issue Found**: Single-card search returns in `OnSelectCard` lacked `min <= 1 && 1 <= max` boundary guards.
   - **Hardening**: Wrapped all single-card returns with `if (min <= 1 && 1 <= max)` and null-safe plugin access.

7. **`Centur-Ion` (`CenturionExecutor.cs` & `CenturionPlugin`)**:
   - **Audit Result**: Clean. Does not override `OnSelectCard`, `OnSelectCounter`, or material selectors; safely uses base `ModernExecutor` heuristics and OCGCore protocol.

### Verification Results
- All 7 decks compiled and published cleanly with 0 Errors via `BUILD_AND_DEPLOY.ps1`.
- Headless simulation verified: `2026_Endymion` (0 Violations, 0 Crashes), `2026_DDD` (0 Violations, 100% Win Rate), `2026_SixSamurai` (0 Violations, 66.7% Win Rate over 3 matches).
- Deployed exclusively to `C:\Users\admin\Documents\EdoGame\`.

---

## 0.048. Endymion SelectCounter Engine Crash Resolution & Universal Counter Safety (2026-09-26)

### Incident & Root Cause Analysis
- **Symptom**: In EDOPro vs `Endymion` bot on Turn 1, the duel engine halted with popup `"เกิดข้อผิดพลาด!"` (An error occurred!). Duel log recorded:
  ```
  [TRACE][Activate] ✓ 'Endymion, the Mighty Master of Magic' (3611830) → MightyMasterBoardBreak from SpellZone
  [BOARD SCORE] Idle Command Decision | Score: 0 | Action: Activate (Index: 1)
  [ERROR] Got MSG_RETRY. Last message is SelectCounter
  Connection closed by remote host.
  ```
- **Primary Root Causes**:
  1. **Premature Turn 1 Board-Break Activation**:
     `_2026_EndymionExecutor.MightyMasterBoardBreak` contained condition `Duel.Player == 0` which falsely evaluated to `true` on Turn 1 when bot was Player 0. This triggered Mighty Master to remove 6 Spell Counters to destroy opponent cards when the opponent's field was 100% empty, draining vital counter reserves.
  2. **Invalid Pendulum Scale Sequence Checks**:
     Scale placement conditions in `_2026_EndymionExecutor` checked `SpellZone[1]` and `SpellZone[5]`. In MR4/2020, Pendulum scales are in `SpellZone[0]` and `SpellZone[4]`, while `SpellZone[5]` is the Field Spell zone.
  3. **Packet ClientCard Deserialization Incomplete in GameBehavior**:
     `GameBehavior.OnSelectCounter` read the card id from packet via `packet.ReadInt32();` but discarded it, calling `_duel.GetCard(player, loc, seq)` directly. If the field card was untracked or returned null, `cards[i]` became null and card ID matching failed.
  4. **Missing Sum & Boundary Enforcement in Engine Protocol**:
     Neither `GameBehavior.OnSelectCounter` nor `GameAI.OnSelectCounter` verified that `sum(used) == quantity` or guarded against bounds errors. If any executor returned unbalanced distributions, OCGCore rejected the CTOS response packet with `MSG_RETRY` and disconnected.

### Comprehensive Fixes Applied
- **1. GameBehavior.OnSelectCounter Engine Hardening (`GameBehavior.cs`)**:
  - Automatically initializes `ClientCard(cardId, loc, seq, player)` or sets `card.SetId(cardId)` so `cards[i]` is never null and `cards[i].Id` is guaranteed authentic from OCGCore.
  - Implements an infallible mathematical safety allocator: clamps `0 <= used[i] <= counters[i]`, and dynamically distributes any remainder or trims any excess so `sum(used) == quantity` is guaranteed 100% of the time.
  - Added comprehensive diagnostic trace logging for every `OnSelectCounter` packet.
- **2. GameAI Fallback Loop Safety (`GameAI.cs`)**:
  - Replaced unsafe `while (quantity > 0)` loop with a bounded `for` loop, eliminating `IndexOutOfRangeException` risk.
- **3. Endymion Pendulum & Scale Architecture Fix (`_2026_EndymionExecutor.cs`)**:
  - Standardized all scale zone checks to `Bot.SpellZone[0]` and `Bot.SpellZone[4]`.
  - Added strict opponent board check: `MightyMasterBoardBreak` will **NEVER** activate unless opponent controls at least 1 monster or spell/trap to destroy (`Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0`).
  - `MasterCerberusPendulum` now strictly verifies that both scales or the other scale is completely empty before activation.
  - Delegated `OnSelectCounter` cleanly to `EndymionCounterEconomy.SelectCounters` with Jackal King 2-counter negate protection and full decision tracing.
- **Build & Deployment**:
  - Built and verified with 0 errors via `BUILD_AND_DEPLOY.ps1`.
  - Deployed exclusively to `C:\Users\admin\Documents\EdoGame\`.

---

## 0.047. Three Advanced Deck Implementations: MorganiteStun, DrytronTour & Madolche (2026-09-26)

### Overview
- **MorganiteStun (`MorganiteStunExecutor.cs`) — Anti-Meta Stun & Super Poly Board Breaker**:
  - Implements `MorganiteStunPlugin`, `MorganiteStunStrategy`, `MorganiteFloodgateManager`, `SuperPolyAdvisor`, `MorganiteMaterialScorer`, `MorganiteActionScorer`, and `MorganiteBoardAssessor`.
  - Normal Summons `Vanity's Ruler` (one-sided Special Summon lockout) or `Majesty's Fiend` without tribute under `Guilt-Gripping Morganite` with zero LP costs for `Solemn Judgment`, `Solemn Strike`, and `Iron Thunder`.
  - Double Normal Summon & double draw under `Time-Tearing Morganite`, with `Seventh Tachyon` search engine via Number 104/107.
  - Spell Speed 4 board clearing via `Super Polymerization` targeting Mudragon, Garura, Starving Venom, Dragostapelia, Earth Golem, or Triphyoverutum.
  - Trap Handtraps (`Songs of the Dominators`, `Dominus Purge`, `Dominus Impulse`) bypass Morganite's restriction.
- **DrytronTour (`DrytronTourExecutor.cs`) — Machine Ritual Engine & Rank 1 Xyz**:
  - Implements `DrytronTourPlugin`, `DrytronStrategy`, `DrytronTributeManager`, `DrytronRitualAdvisor`, `DrytronMaterialScorer`, and `DrytronBoardAssessor`.
  - Boss `Drytron Meteonis DA Draconids` (5000/5000) provides 2x quick monster effect negations per turn fueled by GY Drytrons.
  - `Drytron Mu Beta Fafnir` mills missing combo pieces and detaches materials as tribute for Ritual Summons.
  - Going second board break via `Dark Ruler No More` or `Gordian Slicer` followed by `Lyrilusc - Assembled Nightingale` direct attack into 4-material `AA-ZEUS`.
- **Madolche (`MadolcheExecutor.cs`) — Non-Targeting Shuffle Control & Vernusylph Engine**:
  - Implements `MadolchePlugin`, `MadolcheStrategy`, `MadolcheGraveyardManager`, `MadolcheMaterialScorer`, and `MadolcheBoardAssessor`.
  - `Madolche Petingcessoeur` start $\rightarrow$ `Anjelly` $\rightarrow$ `Hootcake` $\rightarrow$ `Messengelato` search loop for Chateau, Promenade, and Ticket.
  - Double spin loop: `Queen Tiaramisu` non-targeting spins 2 opponent cards on our turn $\rightarrow$ overlays into `Queen Tiarafraise` which quick-spins 2 more opponent cards on their turn.
  - `Madolche Promenade` omni-negates while `Madolche Teacher Glassouffle` protects from monster effects and purges GY to enable Petingcessoeur.
- **Universal Standards & Skill Update**:
  - Updated `SKILL.md` Section 6.2 & 6.3 with mandatory standard: Every new deck must use Deck + Deck-Specific Helper Modules, Smart Position Control (0 ATK, Handtraps, and DEF > ATK in Defense), and Desperation MonsterSet without withholding handtraps from their primary disruption duties.
  - Registered all 3 decks in `bots.json` with difficulty 3 (Master / Hard) and full aliases.
  - Deployed exclusively via `BUILD_AND_DEPLOY.ps1` to `C:\Users\admin\Documents\EdoGame\` with 0 Errors.

---

## 0.046. Deck Domain Plugin Architecture Elevation & Smart Position Handtrap Survival Overhaul (2026-09-26)

### Overview
- **Domain Plugin Architecture Elevation (Matching Six Samurai Benchmark)**:
  - **Endymion (`_2026_EndymionExecutor.cs`)**: Refactored with decoupled domain classes: `EndymionPlugin`, `EndymionStrategy`, `EndymionCounterEconomy`, `EndymionScaleResolver`, `EndymionMaterialScorer` (-10,000 boss sacrifice penalty), `EndymionActionScorer`, and `EndymionBoardAssessor`.
  - **Centur-Ion (`CenturionExecutor.cs`)**: Refactored with decoupled domain classes: `CenturionPlugin`, `CenturionStrategy`, `CenturionTimingAdvisor` (opponent-turn Quick Synchro choke-point detection & Cosmic Blazar Dragon gating), `CenturionResourceLoop` (End Phase S/T zone recovery & board-clog prevention), `CenturionMaterialScorer`, `CenturionActionScorer`, and `CenturionBoardAssessor`.
  - **D/D/D (`_2026_DDDExecutor.cs`)**: Refactored with decoupled domain classes: `DDDPlugin`, `DDDStrategy`, `DDDContractBurnManager` (Standby burn danger assessment & Contract Clearance protocol when LP $\le 2000$), `DDDScaleAndSearchResolver`, `DDDMaterialScorer` (Deus Machinex, High King Caesar, Siegfried boss protection), `DDDActionScorer`, and `DDDBoardAssessor`.
  - **Six Samurai (`_2026_SixSamuraiExecutor.cs`)**: Unified with `SmartMonsterRepos` and `OnSelectPosition` survival wall safeguards.
- **Smart Position & Handtrap Survival System**:
  - **Problem Solved**: Normal Summons are always in Face-up Attack. Low ATK/high DEF handtraps (Ash Blossom 0/1800, Ghost Belle 0/1800, Veiler 0/0) or searchers/walls (Kepler 0/0, Fuma 200/1800) stranded in Attack position invited lethal/OTK battle damage.
  - **Tier 1 (`OnSelectPosition`)**: Overridden to return `FaceUpDefence` or `FaceDownDefence` for all 0 ATK, Handtrap, and DEF > ATK survival monsters.
  - **Tier 2 (`SmartMonsterRepos`)**: Registered via `ExecutorType.Repos` to actively reposition 0 ATK and high-DEF monsters stranded in Attack position to Defense.
- **Policy Established for Future vs Legacy Decks**:
  - **New Decks**: MUST implement Decoupled Domain Plugin Architecture (Layer 3) and Smart Position/Handtrap defense.
  - **Legacy Working Decks**: Preserved intact without unnecessary modifications to ensure 100% backward compatibility and stability.
- **Build & Deployment Pipeline**:
  - Successfully published and deployed via `BUILD_AND_DEPLOY.ps1` with 0 Errors to `C:\Users\admin\Documents\EdoGame\`.

---

## 0.045. Three Modern Bot Implementations: Endymion, Centur-Ion & D/D/D (2026-09-26)

### Overview
- **1. Endymion (Spell Counter Control) — New Bot & Deck Plugin**:
  - **Canonical Deck**: `2026_Endymion.ydk` (40 Main, 15 Extra, 15 Side) with Mythical Beast engine, Spellbook engine, Selene Queen of Master Magicians, Electrumite, Beyond the Pendulum, and Odd-Eyes Absolute -> Vortex dragon combo.
  - **Executor Architecture**: `_2026_EndymionExecutor.cs` implementing full **Spell Counter Economy**:
    - Five-tier counter reserve level: `Critical` (0-1), `Low` (2-3), `Ready` (4-5), `ComboReady` (6-7), `Surplus` (8+).
    - `Jackal King` monster negate priority: 2-counter budget with threat evaluation (`IsBoss` > `IsHighThreatChokepoint` > `IsKnownNegator`). Rule: 1 Negate = 1 Problem (avoids double-negating already disabled chains).
    - `Mighty Master of Magic`: Quick S/T negate with smart recycling priority (used `Servant` > used `Magister` > `Reflection` > highest counter holder to transfer counters). 6-counter board wipe on Going 2nd/breakout.
    - `Servant of Endymion` & `Magister of Endymion`: 3-counter check ensures extension only when board value or disruption is added.
    - `OnSelectCounter` engine hook: Prioritizes `Magical Citadel` (global fuel) and `Mythical Institution` / surplus monsters, protecting cards building toward 3 counters.
- **2. Centur-Ion (Synchro Control) — Timing Decision & Resource Loop Upgrade**:
  - **Canonical Deck**: `Centurion.ydk` synced and mapped to `Centurion`, `Centur-Ion`, and `2026_CenturIon`.
  - **Executor Upgrade**: `CenturionExecutor.cs`:
    - **Timing Decision**: `StandUpCenturIon` opponent-turn Quick Synchro no longer activates blindly; it monitors the opponent's combo and strikes at the **critical moment** (summon of monster with ATK $\ge 1800$, high-threat starter/chokepoint/boss, 2+ monsters on board, or chain activation).
    - **Crimson Dragon -> Cosmic Blazar Dragon Loop**: Tags out Level 12 Synchros (Legatia/Auxila) into Cosmic Blazar Dragon at the optimal threat window.
    - **Resource Loop**: End Phase triggers for `Primera` and `Trudea` placing themselves into the S/T Zone from GY/banished zone.
- **3. D/D/D (Combo Monster) — Integration & Deployment**:
  - **Canonical Deck**: `2026_DDD.ydk` registered and synced to `deck/2026_DDD.ydk`.
  - **Executor Architecture**: `_2026_DDDExecutor.cs` registered across `2026_DDD`, `DDD`, and `D/D/D`.
  - Combines Pendulum, Fusion, Synchro, Xyz, and Link routes ending on Deus Machinex, High King Caesar, Siegfried, and Sky King Zeus Ragnarok.
- **bots.json Registration**:
  - Registered all 3 bots with official canonical and alias names (`2026_Endymion`, `Endymion`, `2026_CenturIon`, `Centurion`, `Centur-Ion`, `2026_DDD`, `DDD`, `D/D/D`).
- **Deck Synchronization**:
  - Synced `2026_Endymion.ydk`, `Centurion.ydk`, and `2026_DDD.ydk` to `C:\Users\admin\Documents\EdoGame\deck\`.
- **Build & Exclusive Deployment Pipeline**:
  - Executed `BUILD_AND_DEPLOY.ps1` with 0 Errors; deployed to `C:\Users\admin\Documents\EdoGame\`.

---

## 0.044. Developer Mode (โหมดนักพัฒนา) & Logging Control Integration (2026-09-26)

### Overview
- **DashBot Developer Mode Toggle ("โหมดนักพัฒนา")**:
  - Added `ChkDevMode` toggle checkbox to DashBot UI (`MainWindow.xaml`), positioned right beside "Console Output Logs" with `IsChecked="True"` as the default state.
  - **Developer Mode ON (Default)**: Full verbose engine decision traces (`DecisionTracer`, `Logger.WriteTraceLine`) shown in UI console and written to session log files (`WindBot\logs\duel_*.log` and `logs\headless\*.log`).
  - **Developer Mode OFF (Clean Mode)**:
    - Passes `EnableFileLog = false` (`Log=false`) to WindBot and HeadlessClientWrapper, completely suppressing file log writing to keep the disk clean.
    - Filters high-frequency noisy traces (`[DEBUG]`, `[TRACE]`, `Candidate card`, `Score:`) in the DashBot console window, keeping only turn milestones, results, and critical notifications.
- **Engine Core & WindBot Plumbing**:
  - `Logger.cs`: Added `public static bool FileLogEnabled { get; set; } = true;` guarding `StartDuelSession`, `EndDuelSession`, `WriteToLogFile`, and `WriteErrorToLogFile`.
  - `Program.cs`: Configured CLI parameter `Log=bool` to toggle `Logger.FileLogEnabled` and `DecisionTracer.Enabled`.
  - `HeadlessClientWrapper.cs`: Added `EnableFileLog` property to control whether `_logWriter` is created and whether `Log=true/false` is passed to the WindBot process.
- **Deck Taxonomy & Anti-Duplication Standards**:
  - Cleaned up duplicate `.ydk` files (removed duplicate `_2026_SixSamurai.ydk`, preserving single canonical `2026_SixSamurai.ydk`).
  - Enforced strict Anti-Duplication Rule in Section 0 (Rule 6) and Section 8 of `SKILL.md` (Modern `2026_`, Anime `Anime_`, Legacy `AI_`, GOAT `GOAT_`, Special).
- **Log Sanitation**:
  - Cleared all historical duel logs in `WindBot\logs\`, `logs\`, and `src\YGO_SOURCE_CLEAN\logs\`.
- **Build & Deploy Pipeline**:
  - Successfully compiled and deployed via `BUILD_AND_DEPLOY.ps1` with 0 Errors; deployed to `C:\Users\admin\Documents\EdoGame\`.

---

## 0.043. Six Samurai Gateway Engine Audit & DashBot Logs Integration (2026-09-26)

### Overview
- **Six Samurai Engine Optimization & Bugfixes**:
  - **Gateway of the Six Multi-Effect Discrimination**: Added `ActivateDescription` routing in `_2026_SixSamuraiExecutor.cs` to distinguish Effect 1 (Search, cost 4 Bushido counters), Effect 0 (+500 ATK, cost 2 counters), and Effect 2 (Revive Shien, cost 6 counters). Blocks Effect 0 during Main Phase 1 setup to guarantee counters reach $\ge 4$ for loop searching.
  - **SelectCounters Safety Fallback**: Enhanced `SixSamCounterEconomy.SelectCounters` with strict Gateway card checks and residual fallback drain, ensuring `sum(used) == quantity` and eliminating `MSG_RETRY` engine disconnects.
  - **HeuristicGuard Self-Negate False Positive Fix**: Removed `hint == HINTMSG_FACEUP` from `ValidateSelection` Rule 1 in `HeuristicGuard.cs`. Eliminates false self-negate violations on friendly stat buffs and equip targets.
- **DashBot Launcher Quality of Life**:
  - Added **"Open Logs"** button (`BtnOpenLogs`) in `MainWindow.xaml` and `MainWindow.xaml.cs` to open `WindBot\logs\` directly in File Explorer.
- **Build & Deploy Pipeline**:
  - Compiled and deployed via `BUILD_AND_DEPLOY.ps1` with 0 Errors; deployed to `C:\Users\admin\Documents\EdoGame\`.

---

## 0.042. Six Samurai Decoupled Deck Plugin AI Architecture (v11.0 Audited) (2026-09-26)

### Overview
- **Decks Created**:
  - `windbot-fork/Decks/2026_SixSamurai.ydk` & `_2026_SixSamurai.ydk` (2026 Master Duel ROTA Six Samurai Core)
  - Synced to `C:\Users\admin\Documents\EdoGame\deck/`
- **New AI Architecture**:
  - `windbot-fork/Game/AI/Decks/_2026_SixSamuraiExecutor.cs` (Rule-Based ModernExecutor with 5-Layer Decoupled Deck Plugin Model)
  - Coordinated by `SixSamuraiPlugin` with 7 sub-helpers: `SixSamStrategy`, `SixSamCounterEconomy`, `SixSamKizaruResolver`, `SixSamMaterialScorer`, `SixSamActionScorer`, `SixSamRecoveryPlanner`, `SixSamBoardAssessor`.
- **Core Engine Upgrade**:
  - Enhanced `Executor.cs` & `GameAI.cs` with `OnSelectCounter` virtual callback, granting counter-based decks full control over counter spending priorities.
  - Upgraded `SKILL.md` to **v11.0 (Audited Decoupled Plugin & Contextual Reasoning Architecture)**, eliminating incorrect Hint 573, establishing Contextual Removal Evaluation, Compensated Advantage Gate, and Repo-native API Signature rules.
- **Bots Registration**:
  - Added `2026_SixSamurai`, `Six Samurai`, `SixSamurai` to `bots.json`
- **Build & Deploy Pipeline**:
  - Compiled via `BUILD_AND_DEPLOY.ps1` (0 Errors). Deployed exclusively to `C:\Users\admin\Documents\EdoGame\`.

### Key Intelligence & Domain-Specific Implementation
1. **Bushido Counter Economy & Management (`SixSamCounterEconomy`)**:
   - Manages Bushido Counter removal via `OnSelectCounter` override: drains counters from `Battle Shogun` (vulnerable monster body) first, then `Shien's Dojo`, while preserving `Gateway of the Six` counters.
   - Loop cutoff guard prevents infinite activation loops and timeouts (hard safety limit at 20 activations per turn or immediate cutoff when lethal OTK is reached).
2. **Missing Resource Search Routing (`PickSearchTarget`)**:
   - Dynamic search decisions based on hand & board state:
     - Missing Gateway $\to$ `Gateway of the Six` (via Battle Shogun)
     - Missing Starter $\to$ `Kageki` / `Shien's Smoke Signal`
     - 2+ Six Sam on board $\to$ `Great Shogun Shien` (Spell/Trap lockout floodgate)
     - Missing Tuner for Synchro $\to$ `Tactical Trainer` (Lv2) / `Anarchist Monk` (Lv3) / `Fuma` (Lv1)
     - Extender / Loop Continuation $\to$ `Kizan` (free SS, no OPT)
3. **Material Valuation & Boss Monster Protection (`SixSamMaterialScorer`)**:
   - Custom scoring in `OnSelectCard` for `HINT_LMATERIAL` and `HINT_SMATERIAL`:
     - Maximum penalty (10000) for `Legendary Lord Shi En`, `Legendary Shi En`, `Great Shogun Shien`, `Naturia Beast`, `Apollousa`.
     - Preserves `Fuma` for destruction protection if it is our sole Tuner.
     - Selects low-ATK or spent bodies (`Kageki` 200 ATK, `Shinai`, `Mizuho`, duplicate `Kizan`) as link/synchro fodder.
4. **Layered End Board & Going 2nd Board Breaking**:
   - **Going 1st**: `Legendary Lord Shi En` (Monster effect negate), `Legendary Shi En` (Spell/Trap negate), `Great Shogun Shien` (limits opponent to 1 S/T per turn), `Naturia Beast` (unlimited Spell negate via mill), `Apollousa` (multi-monster negate).
   - **Going 2nd**: `Legendary Lord Enishi` (bounce monsters up to banished Six Sam), `Mizuho` (tribute fodder to pop enemy cards).
   - **OTK Cutoff**: Automatically disables combo loop and directs resources to battle phase when lethal damage is secured.

---

## 0.041. Dinomorphia Undying Trap Stun & Kashtira Macro Stun Integration (2026-09-25)

### Overview
- **Decks Created**:
  - `windbot-fork/Decks/2026_Dinomorphia.ydk` (Undying Low-LP Trap Stun)
  - `windbot-fork/Decks/2026_Kashtira.ydk` (Walking Macro Cosmos & Zone Lock Stun)
- **New AI Executor**:
  - `windbot-fork/Game/AI/Decks/_2026_DinomorphiaExecutor.cs` (Rule-Based ModernExecutor)
- **Bots Registration**:
  - Added `2026_Dinomorphia`, `Dinomorphia Stun`, `2026_Kashtira`, `Kashtira Stun` to `bots.json`
- **Build & Deploy Pipeline**: Compiled via `BUILD_AND_DEPLOY.ps1` (0 Errors). Deployed exclusively to `C:\Users\admin\Documents\EdoGame\`.

### Key Intelligence & Strategic Implementation
1. **Dinomorphia Fusion Engine**:
   - `Dinomorphia Frenzy`: Activates strictly in opponent's Main Phase, sending `Kentregina`/`Stealthbergia` from Extra Deck + `Therizia`/`Diplos` from Main Deck to summon `Dinomorphia Rexterm` (3000 ATK).
   - `Dinomorphia Domain`: Activates in Main Phase to fuse Kentregina or Rexterm from hand/field/deck.
2. **Rexterm Lockout & ATK Suppression**:
   - Continuous floodgate prevents opponent monsters with ATK >= LP from activating effects.
   - Quick effect pays half LP to reduce all opponent monsters' ATK to current LP, achieving complete monster lockout.
3. **Graveyard Damage Nullification**:
   - Banishes Counter Traps from GY during damage calculation to make battle damage 0.
   - Banishes Normal Traps from GY in response to card effects to negate effect damage.
4. **Undying Floating Loops**:
   - Rexterm, Kentregina, Stealthbergia, Therizia, and Diplos float into Level 4 Dinomorphia upon destruction.
5. **Miscellaneousaurus Integration**:
   - Quick effect from hand grants all Dinosaurs complete immunity to opponent activated effects throughout the Main Phase.

---

## 0.040. ADML Intelligent Combo Bridge & Dynamic Placement Cognitive Upgrade (2026-09-22)

### Overview
- **Deck**: `ADML.ydk` (Azamina Dark Magician Light and Darkness Ritual)
- **Philosophy**: แทนที่จะใช้วิธีฮาร์ดโค้ดสั่งห้ามหรือปิดกั้นการอัญเชิญ Link Monster (`Cross-Sheep`), ระบบได้รับการยกระดับความฉลาด (Situational Awareness, Synergy Valuation, Precise Zone Placement, และ Advanced Link Climbing) เพื่อให้ AI เข้าใจจังหวะและมูลค่าของ `Cross-Sheep` อย่างแท้จริง
- **Build & Deploy Pipeline**: คอมไพล์ผ่าน `BUILD_AND_DEPLOY.ps1` (0 Errors). Deploy มาที่ `C:\Users\admin\Documents\EdoGame\` โดยตรง

### Key Intelligence Enhancements
1. **Strategic Combo Sequencing (จัดลำดับตาม Value Curve)**:
   - สลับลำดับใน `RegisterExecutors`: ให้ Starters ค้นหาทรัพยากร (`Illusion of Chaos`, `WANTED`, `Diabellstar`, `Deception`, `Magicians' Souls`, `Magician's Rod`) ทำงานก่อนเพื่อนำ fodder ที่หมดบทบาทลงมาบนสนามและเซ็ตอัปสุสาน
   - วาง `Cross-Sheep` เป็น **Combo Bridge Enabler** ก่อนหน้าการสั่งใช้เวทฟิวชัน (`The Hallowed Azamina`, `The Gaze of Timaeus`) และเวทพิธีกรรม (`Light and Darkness Ritual`)
   - ผลลัพธ์: มอนสเตอร์บอส Fusion หรือ Ritual ที่ถูกอัญเชิญตามหลัง จะลงมาทับตำแหน่งลูกศรของ `Cross-Sheep` พอดี ทำให้ทริกเกอร์เอฟเฟกต์ชุบชีวิตหรือจั่วการ์ดทำงาน 100% (แก้ปัญหาบอทเรียก Cross-Sheep มายืนเฉยๆ หลังฟิวชันเสร็จสิ้น)
2. **Proactive Activation Verification (`CanTriggerCrossSheepThisTurn`)**:
   - ประเมินก่อนอัญเชิญเสมอว่าในเทิร์นนี้มีเวท Fusion/Ritual ในมือพร้อมเล่นจริงหรือไม่
   - ตรวจสอบเป้าหมายชุบชีวิตเลเวล 4 หรือต่ำกว่า (`Magicians' Souls`, `Magician's Rod`, `Griffoh`) ทั้งในสุสานหรือตัวที่จะถูกส่งลงสุสานเป็นวัตถุดิบของ `Cross-Sheep`
3. **Strict Material Value Guard (`CrossSheepSpSummon`)**:
   - บังคับใช้เฉพาะมอนสเตอร์ตัวเล็กที่หมดบทบาทแล้ว (ATK < 2000 เช่น Souls 0 ATK, Rod 1600 ATK, Griffoh 300 ATK)
   - ปกป้องบอสตัวหลัก (`Red-Eyes Dark Dragoon`, `Azamina Ilia Silvia`, `Magician of Dark Chaos`, `Black Luster Soldier`, `Black Chaos`) อย่างเด็ดขาด ห้ามนำไปเป็นวัตถุดิบคอร์สชีพ
   - ตรวจสอบเงื่อนไขชื่อต่างกัน 2 ตัว (`Distinct().Count() >= 2`) เพื่อป้องกันปัญหาเลือกตัวซ้ำแล้วเกมปฏิเสธ
4. **Engine-Native Zone Guidance (`OnSelectPlace`)**:
   - ใช้งาน `crossSheep.GetLinkedZones() & 0x1F` จากระดับ Central Core เพื่อคำนวณตำแหน่งช่องว่างบนสนามที่ลูกศรของ `Cross-Sheep` ชี้ลงมาอย่างแม่นยำ (ช่อง 0, 2 หรือ 4)
   - นำทางมอนสเตอร์ Fusion / Ritual ลงมาในตำแหน่งลูกศรโดยตรง ทำให้ทริกเกอร์ทำงานโดยอัตโนมัติ
5. **Seamless Link Climb & Field Recycling**:
   - `Cross-Sheep` ทริกเกอร์ชุบ `Magicians' Souls` ขึ้นมา
   - `Magicians' Souls` ส่งการ์ดเวทที่ใช้งานเสร็จแล้ว (`Deception`, `Wanted`) ลงสุสานเพื่อจั่วการ์ดเพิ่มสูงสุด 2 ใบ
   - เชื่อมต่อไปยัง `Selene, Queen of the Master Magicians` (Link-3) โดยใช้ `Cross-Sheep` (Link-2) + `Souls` (Spellcaster)
   - `Selene` ถอด 3 เคาน์เตอร์เวทมนตร์เพื่อชุบ `Dark Magician` หรือ `Diabellstar the Black Witch` กลับคืนสู่สนาม
   - `Dark Magician` บนสนามพร้อมให้ `The Gaze of Timaeus` สั่งฟิวชันต่อยอดเป็น `Red-Eyes Dark Dragoon` ทันที

---

## 0.039. ADML Rule-Based ModernExecutor Implementation & Architecture Integration (2026-09-22)

### Overview
- **Deck**: `ADML.ydk` (Azamina Dark Magician Light and Darkness Ritual)
- **Files Created / Modified**:
  - `windbot-fork/Game/AI/Decks/ADMLExecutor.cs` (New Rule-Based ModernExecutor)
  - `windbot-fork/Decks/ADML.ydk` (Synchronized from player deck)
  - `windbot-fork/bots.json` (Registered "ADML" and "Azamina Dark Magician")
  - `windbot-fork/ExecutorBase/Game/AI/CardIntelligence.cs` (Added Red-Eyes Dark Dragoon [37818794], Azamina Ilia Silvia [46396218], W:P Fancy Ball [4993187], and Boss Immunities)
  - `dashbot/MainWindow.xaml.cs` (Added "ADML" to `ModernArchetypes`)
- **Build & Deploy Pipeline**: Compiled via `BUILD_AND_DEPLOY.ps1` with 0 Errors. Deployed exclusively to `C:\Users\admin\Documents\EdoGame\`.

### Key Architecture & Strategic Implementation
1. **Multi-Engine Synergy (4 Core Pillars)**:
   - *Azamina Engine*: `WANTED` ➔ `Diabellstar` ➔ `Deception` ➔ `The Hallowed Azamina` ➔ `Azamina Ilia Silvia` (Early Omni-Negate to insulate against Nibiru & handtraps before 5 summons).
   - *Dark Magician Engine*: `Illusion of Chaos` (Searcher + Field Quick Monster Negate) ➔ `Magicians' Souls` (Dump DM/Skull Archfiend & Draw 2) ➔ `The Gaze of Timaeus` (Quick Fusion into `Red-Eyes Dark Dragoon`).
   - *Light & Darkness Ritual Engine*: `Ragged Records of Rites` ➔ `Black Chaos` (Discards to place `Mind Shuffle` face-up) ➔ `Mind Shuffle` (Continuous Trap: Searches Ritual monsters every turn and tags out Level 7+ monsters during opponent turn to summon `Magician of Dark Chaos - Black Chaos` or `Black Luster Soldier - Soldier of Light and Darkness` ignoring summoning conditions!).
   - *Extra Deck Support*: `Cross-Sheep` (Revives Level 4- on Fusion; Draw 2/Discard 2 on Ritual), `Selene` (Revives Spellcasters), `S:P Little Knight`, `W:P Fancy Ball` (Quick Monster Negate), and `Relinquished Anima` (Link-1 monster steal).
2. **Rule & Anti-Pattern Compliance**:
   - `OnSelectCard`: Hint 506 deck searches strictly prioritized. Hint 502/503/504/505/507 removals enforce `c.Controller == 1` only. `Illusion of Chaos` deck placement protects searched cards.
   - `OnSelectPlace`: Master Rule 5 compliance reserves Extra Monster Zone (0x20) exclusively for Link Monsters.
   - `OnSelectEffectYn`: Rejects hostile opponent effect offers (`card.Controller == 1 -> false`).
   - Handtraps (`Ash Blossom`, `Mulcharmy Fuwalos/Purulia`, `Droll & Lock Bird`) preserved in hand.

---

## 0.038. Central Core Human-Like Board Evaluation Engine (2026-09-22)

### Overview
- **Scope**: Central AI Engine (`Executor.cs`, `GameAI.cs`, `ModernExecutor.cs`, `DefaultExecutor.cs`, `BoardScorer.cs`) affecting all 140+ deck executors.
- **Problem Addressed**:
  - Previously, AI lacked human-like situational awareness when completing combos or bricking, leaving an empty board and passing turn blindly without evaluating threat clock or remaining Normal Summon/Set resources.
  - Naive monster setting would sacrifice critical handtraps (e.g. `Effect Veiler`, `Ash Blossom`) uselessly on Turn 1 or during non-lethal situations.
  - In addition, low-ATK monsters were left in Attack position after combo, hand-activatable traps (`Infinite Impermanence`, `Dominus Impulse`) were vulnerable to backrow wipes when set on empty fields, and Quick-Play disruption spells were not set in Main Phase 2.
- **Build & Deployment**: Compiled via `BUILD_AND_DEPLOY.ps1` with 0 Errors. Deployed exclusively to `C:\Users\admin\Documents\EdoGame\`.

### 5 Core Human Cognitive Layers Implemented in Central Core
1. **Opponent Combat Clock & Imminent Lethal Matrix (`BoardScorer.cs`)**:
   - Implemented `CalculateOpponentCombatClock()`:
     - Calculates visible enemy attack power $\sum \text{Attack}$ of active attack-position monsters.
     - Computes turn clock $\text{Clock} = \frac{\text{Bot LP}}{\text{Visible Enemy ATK}}$.
     - Flags `isImminentLethal` when visible ATK $\ge$ Bot LP or when board is empty under critical pressure.
   - Upgraded `BoardSufficiencyScore` to use `CardIntelligence.IsHandtrap` dynamically across all universal handtraps.
2. **Desperation Defense Guard (`ModernExecutor.cs`, `GameAI.cs`)**:
   - Universal Fallback Idle Command hooked directly before `ToEndPhase` in `GameAI.cs`.
   - Strictly gated:
     - **Turn 1 Protection**: Never sets monsters on Turn 1 (`Duel.Turn <= 1`) as opponent cannot attack; handtraps remain in hand for interruption.
     - **Empty Field Only**: Triggers only when `Bot.GetMonsterCount() == 0` and Normal Set is available.
   - **4-Tier Sacrifice Hierarchy**:
     - *Tier 1 (Safe Wall)*: Non-handtrap monster with highest DEF or vanilla fodder without hand effects.
     - *Tier 2 (Redundant Handtraps)*: If holding 2+ handtraps (e.g. 2x Veiler, or Veiler + Imperm) and under threat, sets 1 as a shield while retaining the other to disrupt.
     - *Tier 3 (Critical Sole Handtrap Sacrifice)*: Sacrifices sole handtrap **only** if opponent has visible lethal on board (100% defeat without a shield).
     - *Tier 4 (Non-Lethal Retention)*: If opponent ATK < Bot LP, strictly retains sole handtrap in hand to negate enemy combo starter on their turn.
3. **Handtrap Trap Preservation Guard (`ModernExecutor.cs`)**:
   - `ShouldAllowSpellSet`: Strictly prevents setting `Infinite Impermanence` (10045474) or `Dominus Impulse` (40366667) when `Bot.GetFieldCount() == 0`.
   - Keeps them safely in hand where they activate from hand and are immune to `Harpie's Feather Duster`, `Lightning Storm`, or `S:P Little Knight`.
   - Prevents face-down setting of Normal Spells / Ritual Spells without discard pressure (hand $\le$ 6).
4. **Post-Combo Repositioning Optimizer (`ModernExecutor.cs`, `GameAI.cs`)**:
   - Evaluates `ReposableCards` in MP2 or when ending turn.
   - Automatically repositions low-ATK monsters (ATK < 1500 or DEF > ATK) to Defense position to absorb attacks safely.
   - Enforces Rule 12 for `Number 41: Bagooska` (switches to Defense position to activate floodgate effect).
5. **Smart Backrow MP2 Stewardship (`ModernExecutor.cs`)**:
   - Before ending turn, automatically sets Traps and Quick-Play Spells (`Called by the Grave`, `Super Poly`, `Forbidden Droplet`, `Book of Moon`, `Cosmic Cyclone`) in MP2 so they are armed for the opponent's turn.

---

## 0.037. ArtMage Deep Audit & Multi-Constraint Rule Compliance Patch (2026-09-22)

### Overview
- **Deck**: `ArtMage.ydk`
- **Files Audited & Patched**: `ArtMageExecutor.cs`, `bots.json`
- **Build & Deploy**: Successful with 0 errors via `BUILD_AND_DEPLOY.ps1`
- **Deployment Location**: `C:\Users\admin\Documents\EdoGame\`

### Issues Identified & Fixed in Deep Audit
1. **Nerva Board-Wipe Overriding Combo Setup**:
   - `Nerva the Power Patron of Creation` replaces the chained monster's effect with `"Destroy all cards your opponent controls"`.
   - When `Shadow Beast Nervedo` triggered in the Extra Deck to summon `Artmage Finmel` from Deck, Nerva previously could chain and replace Nervedo's trigger, cancelling Finmel's summon, losing 2400 ATK, a Draw 1, and the 3rd Monster Type.
   - Fixed by explicitly guarding against chaining Nerva to `ShadowBeastNervedo`'s Extra Deck trigger, `Medius` on summon, and `Power Patron`'s GY search.
2. **Extra Deck Fusion Lock Ignored for Link Monsters**:
   - `Artmage Power Patron` (23829452) continuous effect locks Extra Deck Special Summons to Fusion Monsters only while face-up on field.
   - Link summon routines (`CrossSheep`, `SPLittleKnight`, `KnightmareCerberus`, `Accesscode`) lacked this check and could attempt illegal Link summons.
   - Fixed by adding `!Bot.HasInMonstersZone(CardId.ArtmagePowerPatron)` to all Link summon conditions.
3. **Rule 11 Compliance (`OnSelectPlace`)**:
   - Added `OnSelectPlace` override ensuring Fusion and Main Deck monsters are never placed in the Extra Monster Zone (EMZ) while Main Monster Zones are available (`available & 0x1F`), reserving EMZ exclusively for Link Monsters.
4. **Level 7 2-Tribute Bug in `FinmelTributeSummon`**:
   - Level 7 `Artmage Finmel` requires 2 tributes. Code previously checked only `Bot.GetMonsterCount() == 0` fallback and any monster, causing failed tribute attempts with 1 monster.
   - Fixed by requiring `Bot.GetMonsterCount() >= 2` and 2+ valid low-ATK non-Ace tributes.
5. **`PactGYPopEffect` Hard Once-Per-Turn Violation**:
   - `Artmage Pact` shares a hard once-per-turn limit between field activation and GY effect. The GY effect was missing `_pactUsed` checking and tracking.
   - Fixed with strict mutual exclusion.
6. **Acropolis Deck Target Validation**:
   - `AcropolisEffect` announced card names without verifying presence in Deck, risking illegal announcement when cards were already drawn.
   - Fixed by selecting only candidates actually present in `Bot.Deck` and not on field.
7. **S:P Little Knight Target Legality**:
   - `SPLittleKnightQuickEffect` fell back to targeting Spells/Traps, which is illegal for S:P's Quick Effect (monsters only).
   - Fixed to target only face-up enemy monsters.
8. **Diactorus Field Negation Scope**:
   - Expanded from `MonsterZone | SpellZone` to `lastCard.IsOnField()` to properly negate Field Spells (FieldZone) and Pendulums.
9. **Duplicate Registration Cleaned**:
   - Removed redundant duplicate entry for `ArtMage` in `bots.json`.

---

## 0.036. ArtMage Rule-Based ModernExecutor Complete Refactor & Strategic Optimization (2026-09-22)

### Overview
- **Deck**: `ArtMage.ydk` (40 Main Deck, 15 Extra Deck)
- **Executors Modified**: `ArtMageExecutor.cs`, `bots.json`
- **Build & Deploy Pipeline**: `BUILD_AND_DEPLOY.ps1` (Release win-x64 self-contained)
- **Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`

### Root Causes & Key Flaws Identified in Previous Code
1. **Broken 1-Card Primary Combo (`Medius the Pure` + `aux.ToHandOrElse`)**:
   - `Medius the Pure`'s on-summon trigger uses `aux.ToHandOrElse` prompting `Duel.SelectOption(573, str)` (0 = Add to Hand, 1 = Special Summon).
   - Without overriding `OnSelectOption`, WindBot defaulted to 0 (Add to Hand), putting `Shadow Beast Nervedo` in the hand instead of the Monster Zone.
   - Because Nervedo was not on the field, its ignition effect (banish 3 top deck cards face-down to Special Summon `Nerva the Power Patron of Creation`) could never activate, breaking the bot's turn 1 setup immediately.
2. **Missing `OnSelectYesNo` Confirmations**:
   - Multiple key continuous/trigger effects in OCGCore Lua call `SelectYesNo`:
     - `Artmage Finmel` (draw 1 card upon Special Summon)
     - `Artmage Graflare` (Set 1 Artmage Spell from Deck upon Special Summon)
     - `Artmage Litera` (Add 1 Artmage card from GY upon Special Summon)
     - `Artmage Vandalism` (Search Medius upon activation)
     - `Artmage Impasto` (Bounce all opponent Spells/Traps upon monster effect negate)
     - `Shadow Beast Nervedo` (Pendulum Zone monster effect negate)
     - `Vandalism` (Protect Acropolis from destruction)
   - With no `OnSelectYesNo` override, these prompts were unhandled or refused.
3. **Card Effect Hallucination (`Artmage Power Patron` 23829452)**:
   - The legacy executor assigned a non-existent discard-from-hand search effect to `Artmage Power Patron`.
   - Real Card Effects:
     - Field Quick Effect (Main Phase): Fusion Summon 1 Artmage Fusion or Nerva using this card + hand/field.
     - GY Trigger: When sent from hand or field to GY (e.g. as Fusion material, discarded by Acropolis/Super Poly), search 1 Artmage Spell/Trap with a different name from cards in GY.
4. **Suboptimal Board-Wipe Timing for `Nerva the Power Patron of Creation` (53589300)**:
   - Nerva replaces an activated Artmage monster effect with `"Destroy all cards your opponent controls"`.
   - Chaining Nerva overrides the original effect; previously, it lacked turn-phase intelligence, occasionally destroying opponent's empty field or overriding critical setup searches.
5. **Missing Bot Registration**:
   - `ArtMage` and `Artmage` were missing in `bots.json`.

---

## 0.035. Scalable 3-Tier Card Intelligence Architecture & Lua Engine DelayedOperation Fix (2026-09-22)

### Overview
- **Core Architecture Upgraded**: `CardIntelligence.cs`, `CardIntelligence.Generated.cs`, `CardExtension.cs`
- **New Tooling**: `tools/scan_card_intelligence.py` (Automated Lua & CDB metadata extractor)
- **Lua Engine Bugfix**: `repositories/delta-bagooska/script/utility.lua`, `script/utility.lua`
- **Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`

### Enhancements & Fixes Implemented
1. **Automated Lua & CDB Intelligence Scanner (`tools/scan_card_intelligence.py`)**:
   - Replaced manual, error-prone enum maintenance with an automated scanner that parses official scripts in `script/official/*.lua` (13,478+ files) and `cards.cdb` in under 2 seconds.
   - Extracts exact OCGCore effect constants: Target-Immune (202), Battle-Immune (377), Dangerous Battle (40), Fusion Spells (145).
2. **Central Database & Query API Integration (`CardIntelligence.cs`)**:
   - Converted `CardIntelligence` into a partial class.
   - Integrated generated sets into query methods: `IsTargetImmune`, `IsInvincibleBattle`, `IsDangerousBattleTarget`, `IsFusionSpell`.
3. **CardExtension Dynamic Bridging (`CardExtension.cs`)**:
   - Upgraded core extension methods used across all 30+ executors to query `CardIntelligence` $O(1)$ HashSets first.

---

## 0.034. AFS (Azamina Fiendsmith Snake-Eye) Decision Engine Overhaul & Game-Stall Fix (2026-09-22)

### Overview
- **Deck**: `AFS.ydk` & `2026_AFS.ydk` (40 Main Deck, 15 Extra Deck, 15 Side Deck)
- **Executors Modified**: `AFSExecutor.cs`, `ModernExecutor.cs` (Rule-Based C# .NET 10)
- **Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`

### Root Causes & Fixes Implemented
1. **`Deception of the Sinful Spoils` Hand Activation Lock**: Enabled free hand activation and expanded tribute targets.
2. **`Forbidden Droplet` Self-Interruption**: Added `Duel.LastChainPlayer == 0` guard to prevent self-interruption.
3. **`Fiendsmith's Lacrima` SelectOption Bug**: Overrode `OnSelectOption` returning 1 (Special Summon).
4. **`Fiendsmith's Sequence` GY Material Shuffling**: Preserved Engraver in GY.
5. **`DDDWaveHighKingCaesar` Priority & Sequence / Princess Guard**: Promoted Caesar to Tier 3.

---

## 0.032. Central Core Architecture & Universal Heuristics Overhaul (2026-09-21)

### Overview
- **Scope**: Central AI Engine (`ExecutorBase`, `GameAI`, `ModernExecutor`, `DefaultExecutor`, `CardIntelligence`, `AntiFloodgateHelper`) affecting all 140+ deck executors.
- **Objective**: Maximize AI tactical execution and eliminate systemic misplays (EMZ clogging, Bagooska position bugs, duplicate handtraps, harmful opponent prompt acceptance, missed direct attack lethals).
- **Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`

### Key Architectural Enhancements
1. **Master Rule 5 EMZ Preservation & Column Safeguards (`Executor.cs`)**: Restricted EMZ auto-routing strictly to Link and face-up Pendulum monsters.
2. **Floodgate Card ID Corrections & Bagooska Defense Safeguard (`ModernExecutor.cs`, `CardIntelligence.cs`)**: Enforced `FaceUpDefence` for Bagooska.
3. **Universal Duplicate Handtrap & Negate Prevention (`GameAI.cs`, `ModernExecutor.cs`, `DefaultExecutor.cs`)**: Skips duplicate once-per-turn handtraps in the same chain.
4. **Lethal & Archetype Direct Attack Optimization (`DefaultExecutor.cs`)**: Direct attack lethal checks and Hayate direct attack priorities.
5. **Hostile Opponent Prompt Safeguard (`GameAI.cs`)**: `OnSelectEffectYn` automatically declines unhandled opponent card prompts.
