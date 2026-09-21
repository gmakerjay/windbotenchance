# Progress Log: SacrBeatsMach Competitive Overhaul, Game Deck Cleanup & DashBot Cosmic Background, DashBot Category Segregation & SacrBeatsMach Alias Fix, SacrBeatsMach (FTK & ModernExecutor), Fidraulis Harmonia Lua Fix, 2026_Puppet Bugfix & WCParisKewlTune Refactor, WCParisKewlTune, 2026_Kwtune (Refactor), Anime_JoeyWheeler (Refactor), Anime_JoeyWheeler, Anime_Yugi, 2026_Purrely, 2026_Yummy, 2026_Tearla, GOD-01, Yubel2, 2026_Branded, 2026_DarkTime, 2026_Runick, 2026_RyuGe, 2026_AFS, 2026_Spright, GOD-01, Demise, 2026_Darklord, 2026_DarkWorld & 2026_Hecahand ModernExecutors

## SacrBeatsMach: Competitive Overhaul, Engine Optimization & 4-Deck Benchmark Tournament (2026-09-21)

### Overview
- **Deck**: `SacrBeatsMach.ydk` & alias `ScarbeatMach.ydk` (42 Main Deck, 15 Extra Deck, 15 Side Deck)
- **Problem Statement**:
  - Original 53-card deck suffered from consistency issues and anti-synergy:
    1. `Cannon Soldier MK-2` (14702066) caused catastrophic `MSG_RETRY` / `SelectUnselect` crashes when required tributes had no valid targets except boss monsters. In addition, it constantly tributed 4000/5000 ATK boss monsters for only 1500 burn, losing the AI its board advantage.
    2. `Machina Unclaspare` (45674286) locked Special Summons strictly into Machine-type monsters for the rest of the turn, completely locking out `Fallen Paradise`, `Hamon`, `Raviel`, `Uria`, and `The Chaotic Phantasmal Sacred Beasts`.
    3. `Yomagna the Fire Phantom` (17350692) required convoluted trigger conditions that rarely resolved.
    4. Deck lacked top-tier unconditional board breakers and consistent Rank 10 extension.
- **Key Improvements**:
  1. **Decklist Optimization (Streamlined to 42 Cards)**:
     - Removed: `Cannon Soldier MK-2` x3, `Machina Unclaspare` x2, `Yomagna the Fire Phantom` x2.
     - Added: `Harpie's Feather Duster` x1, `Raigeki` x1, `Machina Fortress` x1 (2 total, allows discarding `Machina Ruinforce` without Machine type locks), `Heavy Freight Train Derricrane` x1 (2 total, free Rank 10 extension and targeted pop upon detaching).
  2. **Rule-Based ModernExecutor Refactor (`_2026_SacrBeatsMachExecutor.cs`)**:
     - **Bulletproof Min Counts in `OnSelectCard`**: Guarded Hint 500, 501, 502, 504, 505, 506, 508, 509, 513, 549, 575 to strictly guarantee `result.Count >= min`, eliminating all `Got MSG_RETRY. Last message is SelectUnselect` OCG core desyncs.
     - **Ace Protection Guard**: Hardened `IsAceOrKeyMonster()` to strictly safeguard `TheChaoticPhantasmalSacredBeasts`, `Armityle`, `Varudras`, `GustavMax`, `GustavRocket`, `SuperDora`, `Liebe`, `MachinaRuinforce`, `MachinaFortress`, `SuperBESMetalSlave`, `Hamon`, `Raviel`, and `Uria` against being tributed or destroyed as cost.
     - **Metal Slave Suicide Guard**: `ShouldMetalSlaveQuickPop` only targets itself if opponent controls `Eternal Soul` (instant board wipe) or if targeted by opponent removal; otherwise, strictly destroys another B.E.S. monster (`BESBlasterCannonCore`).
     - **Turn 2 Board Breaking & OTK Line**: Prioritizes `Harpie's Feather Duster` / `Raigeki` / `Twin Twisters` -> Rank 10 Train Burn (`Gustav Max` 2000 burn) -> `Superdreadnought Rail Cannon Juggernaut Liebe` (6000 ATK, multiple attacks).
  3. **Benchmark Tournament Audit (4 Legacy Decks, 40 Duels Total)**:
     - **vs DarkMagician**: 5 Wins - 5 Losses (50.0% Win Rate) | 10/10 OK, 0 Violations, 0 Crashes
     - **vs Altergeist**: 6 Wins - 4 Losses (60.0% Win Rate) | 10/10 OK, 0 Violations, 0 Crashes
     - **vs ABC**: 2 Wins - 8 Losses (20.0% Win Rate) | 10/10 OK, 0 Violations, 0 Crashes
     - **vs BlueEyes**: 8 Wins - 2 Losses (80.0% Win Rate) | 10/10 OK, 0 Violations, 0 Crashes
     - **Overall Total**: 21 Wins - 19 Losses (**52.5% Win Rate**), 40/40 (100%) Duels completed with Status: OK and 0 Rule Violations.
  4. **Build & Exclusive Deployment**:
     - Successfully built and deployed all binaries and deck files exclusively to `C:\Users\admin\Documents\EdoGame\`.

---

## Game Deck Directory Cleanup & DashBot Cosmic Background (2026-09-21)

### Overview
- **Game Deck Directory Cleanup (`EdoGame\deck\`)**:
  - Problem: `BUILD_AND_DEPLOY.ps1` previously copied all bot decks into the player's game directory `EdoGame\deck\`, flooding the in-game deck editor with 140+ decks.
  - Solution:
    1. Backed up all 144 files to `C:\Users\admin\Documents\EdoGame\deck_bot_backup\`.
    2. Deleted all 143 bot `.ydk` files from `C:\Users\admin\Documents\EdoGame\deck\`, leaving the player's deck folder clean.
    3. Preserved all bot decks strictly in `C:\Users\admin\Documents\EdoGame\WindBot\Decks\` and `src\YGO_SOURCE_CLEAN\windbot-fork\Decks\`.
    4. Modified `BUILD_AND_DEPLOY.ps1` so it no longer deploys bot decks to `EdoGame\deck\`.
- **DashBot Background Enhancement**:
  - Problem: User requested `images (1).jpg` to be set as a subtle background for the DashBot program.
  - Solution:
    1. Upscaled `Docs/images (1).jpg` using Lanczos filter into high-res `dashbot/bg.jpg`.
    2. Integrated `bg.jpg` into `dashbot.csproj` as an embedded Resource and Deployment file.
    3. Updated `MainWindow.xaml`: added subtle cosmic background layer (`Opacity="0.22"`), radial ambient vignette tint, and converted panels to frosted glass (`#F8FFFFFF`) with elegant drop shadows.
- **Build, Deployment & Git**:
  - Built and deployed exclusively to `C:\Users\admin\Documents\EdoGame\`.
  - Pushed commit `1259599` to `origin/main`.

---

## DashBot Category Segregation & Deck Alias Fallback Resolution (2026-09-21)

### Overview
- **Issue 1 (EvilTwin Selection Bug)**:
  - When selecting `ScarbeatMach` in DashBot, WindBot launched and defaulted to `2026_EvilTwin`.
  - Root Cause: DashBot requested `ScarbeatMach`, but `_2026_SacrBeatsMachExecutor.cs` was registered only as `[Deck("SacrBeatsMach", ...)]`. In `DecksManager.cs`, `NormalizeDeckName` did not match `"scarbeatmach"` to `"sacrbeatsmach"`, triggering the fallback random loop `do { infos = _list[_rand.Next(_list.Count)]; } while (infos.Level != "Normal");` which randomly selected `2026_EvilTwin`.
  - Fix: Updated `DeckAttribute.cs` to `[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]` and registered multiple aliases: `SacrBeatsMach`, `ScarbeatMach`, `scarbeatmach`, `2026_SacrBeatsMach`, `2026_ScarbeatMach`.
- **Issue 2 (DashBot Categories - GOAT Mixed with Special)**:
  - In `MainWindow.xaml.cs`, `GOAT_*` decks were hardcoded with `category = "Special"`, and `MainWindow.xaml` lacked a GOAT category pill.
  - Fix:
    1. Added `RbCatGoat` category filter pill in `MainWindow.xaml`.
    2. Organized category ordering: `All Decks` -> `Modern` -> `Anime` -> `Legacy` -> `GOAT` -> `Special`.
    3. Isolated `GOAT_*` decks into `Category = "GOAT"`, `tagText = "GOAT"`, `tagBg = "#047857"`.
    4. Registered `SacrBeatsMach` / `ScarbeatMach` / `SacredBeats` in `ModernArchetypes` and added display name override `"Sacred Beasts Machina (FTK)"`.
- **Build & Deployment**: Built and deployed to `C:\Users\admin\Documents\EdoGame\`. Committed and pushed to `main` (`8d6cbd5`).

---

## SacrBeatsMach (Sacred Beasts Machina Trains FTK): ModernExecutor, Thai Localization, Image Ingestion & Engine Audit (2026-09-21)

### Overview
- **Deck**: `SacrBeatsMach.ydk` & alias `ScarbeatMach.ydk` (53 Main Deck, 15 Extra Deck, 15 Side Deck)
- **Investigation of User Inquiries**:
  1. **ABC Mixture Claim**: Verified false. No ABC cards (`A`, `B`, `C`, or `ABC-Dragon Buster`) exist in the deck. However, `Machina Ruinforce` (Lv 10, ATK 4600) exhibits a loop mechanic very similar to ABC: it summons itself repeatedly from GY by banishing 12+ Machine levels, and upon destruction/tribute, floats into up to 3 banished Machina monsters (levels <= 12), enabling perpetual component cycling.
  2. **FTK Feasibility**: Verified true 100%. The deck executes FTK via:
     - `Cannon Soldier MK-2` (14702066): Tributes 2 monsters for 1500 damage with no once-per-turn limit, cycling through `Machina Ruinforce` revivals and Machina component floats.
     - Rank 10 Train Burn: `Gustav Max` (2000 burn) -> `Gustav Rocket` (1000 burn) -> `Calamity Hamon` (1000 burn on opp monster sent to GY).
- **Work Completed**:
  1. **Official Image Ingestion**: Downloaded high-resolution official artwork for 13 missing cards from YGOPRODeck CDN into both `EdoGame\pics\` and `src\YGO_SOURCE_CLEAN\pics\`. Updated `BUILD_AND_DEPLOY.ps1` to automatically deploy `pics\`.
  2. **Thai Localization**: Translated descriptions for 13 cards into standard Yu-Gi-Oh Thai phrasing while strictly keeping English card names. Injected into `cards.delta.cdb` (both source and game runtime).
     - Cards: `1259915` (Sacred Beasts Thunderclap), `7894706` (The Chaotic Phantasmal Sacred Beasts), `17350692` (Yomagna), `18616294` (Gray Layer), `22734799` (Summoner of SB), `23856331` (Inferno Uria), `38776201` (Sacred Beasts Released), `50147815` (Combined Assault), `50251045` (Calamity Hamon), `59138498` (Martyr of SB), `65861210` (Fallen Paradise of SB), `80843006` (Mixousia), `96345184` (Infinity Raviel).
  3. **Rule-Based ModernExecutor (`_2026_SacrBeatsMachExecutor.cs`)**:
     - Inherits `ModernExecutor`.
     - Supports Plan A (FTK Cannon Soldier MK-2 Tribute Loop), Plan B (Rank 10 Train Burn with `Gustav Max` + `Gustav Rocket` & Omni-negate `Varudras` / `The Chaotic Phantasmal Sacred Beasts`), and Plan C (Turn 2 Board Breaking & `Liebe` 6000 ATK OTK).
     - Registered in `bots.json` as both `SacrBeatsMach` and `ScarbeatMach`.
  4. **Build, Deployment & Git Synchronization**:
     - Built and deployed exclusively to `C:\Users\admin\Documents\EdoGame\`.
     - All assets and source code committed and pushed to `origin/main`.

---

## Fidraulis Harmonia (Lua Fix), 2026_Puppet (Bug Fixes), & WCParisKewlTune (Refactor) (2026-09-21)

### Overview
- **Decks Affected**: `WCParisKewlTune.ydk`, `2026_Kwtune.ydk`, and `2026_Puppet.ydk`
- **Primary Tasks Completed**:
  1. **Fixed in-game crash in `c70088809.lua` (`Fidraulis Harmonia` / 調和ノ天救竜)**:
     - Error: `[string "c70088809.lua"]:72: Attempting to access deleted object.`
     - Root Cause: In `s.effcost`, the revealed Synchros group `sg` was created without `sg:KeepAlive()`. At the end of the activation cost, OCGCore deleted `sg`. When resolving `s.effop` line 72, calling `cd.revealed_synchros:Match(Card.IsRelateToEffect,nil,e)` resulted in an access violation on the deleted C++ group object.
     - Solution: Added `sg:KeepAlive()` in `s.effcost`. In `s.effop`, wrapped group validity in safe `pcall`, added fallback to `Duel.GetMatchingGroup(Card.IsRelateToEffect,tp,LOCATION_EXTRA,0,nil,e)` if deleted, and properly cleaned up with `rev_g:DeleteGroup()` at the end of the operation.
     - Deployed to: `src\YGO_SOURCE_CLEAN\script\c70088809.lua`, `script\c70088809.lua`, `script\official\c70088809.lua`, and `repositories\local-patches\script\official\c70088809.lua`.
  2. **Refactored `_2026_KwtuneExecutor.cs` (WCParisKewlTune / 2026_Kwtune)**:
     - **Hand-Sync Material Rule Enforcement**: Fixed Hint 512 material selection where selecting cards from hand and field separately could pick >1 hand monster, violating Yu-Gi-Oh Hand-Sync mechanics (maximum 1 monster from hand). Replaced with unified selection logic: selects 1 on-field Tuner, at most 1 hand monster (`.Take(1)`), and fills remaining materials strictly from field.
     - **Fidraulis Harmonia Integration**: Added intelligent card selection for Hint 508 (dumps `Golden Cloud Beast - Malong` to trigger target bounce, or `Wind Pegasus @Ignister` / `Luluwalilith`) and Hint 502 (selects opponent's highest ATK monster for destruction).
  3. **Comprehensive Bug Fixes in `_2026_PuppetExecutor.cs` (2026_Puppet)**:
     - **Fanatix Machinix Burn Trigger**: Fixed `ShouldFanatixBurnActivate` which checked `LastChainCard.Controller == 1`. When the AI summoned a monster to the opponent's field via Fanatix ignition, Mansion, or Cattle Scream, the summon controller was the AI (`0`), blocking Fanatix from burning and destroying the monster. Removed the faulty check so Fanatix correctly triggers whenever a monster is Special Summoned to the opponent's field.
     - **Missing Hint Handlers in `OnSelectCard`**:
       - `Hint 505` (Search): `Fantasix Machinix` searches `Rank-Up-Magic Argent Chaos Force`; `Fanatix Machinix` searches `Service Puppet Play`; `Mansion` searches `Little Soldiers` -> `Rouge Doll` -> `Bloody Doll`.
       - `Hint 508` (Send to GY): `Little Soldiers` sends Lv8 `Rouge Doll` / `Cattle Scream` to become Level 8; `King's Sarcophagus` sends `Hapi`; `Condolence Puppet` dumps GP monsters not already in GY.
       - `Hint 509` (Special Summon): Handles `Argent Chaos Force` Extra Deck rank-up into `CXyz Fanatix Machinix`.
     - **Fiendish Knight Legal Target**: Restricted GY target in `Bot.Graveyard` to `IsGimmickPuppet(c)`, preventing invalid selections.
     - **Extra Deck Lock Enforcement**: Set `_gpExtraLocked` when using `Rouge Doll`, `Fiendish Knight`, and `Chimera Doll`, preventing illegal `S:P Little Knight` summons while locked into Gimmick Puppet Xyz monsters.
     - **Service Puppet Play Control Limit**: Constrained take-control count to `Math.Min(gpXyzCount, 5 - Bot.GetMonsterCount())` and verified controlling a Gimmick Puppet Xyz before triggering GY revival.
     - **`IsGimmickPuppet` Database**: Added missing GP Xyz IDs (`GPDarkStrings`, `GPStrings`, `GPGigantesDoll`).
  4. **Build & Exclusive Deployment**:
     - Successfully built and deployed all binaries and scripts to `C:\Users\admin\Documents\EdoGame\`.

---

## WCParisKewlTune & 2026_Kwtune: Strategic Deck Optimization & Comprehensive ModernExecutor Refactoring (2026-09-21)

### Overview
- **Deck**: `WCParisKewlTune.ydk` (41 Main Deck, 15 Extra Deck, 15 Side Deck) & `2026_Kwtune.ydk`
- **Primary Tasks**:
  1. Analyzed Side Deck for bot playability: identified that Extra Deck's `Visas Amritara` required a "Visas Starfrost" Spell/Trap in Main Deck to resolve its search effect, which was trapped in the Side Deck (`Mannadium Reframing`).
  2. Swapped 4 cards between Main and Side: added `Mannadium Reframing` x1, `Called by the Grave` x1, `Duelist Genesis` x1, `Triple Tactics Talent` x1 to Main Deck; moved `Ghost Belle` x2 and `Synchro Emergency` x2 to Side Deck. Backed up original deck as `WCParisKewlTune_Original.ydk`.
  3. Refactored `_2026_KwtuneExecutor.cs` completely to power both `WCParisKewlTune` and `2026_Kwtune` with a unified, high-performance Rule-Based ModernExecutor.
  4. Fixed card audit defects (Clip level corrected to 2, Zalen correctly classified as Tuner Synchro, Ash Blossom and Feather Duster IDs updated, hardcoded floodgate arrays eliminated in favor of `CardIntelligence`).
  5. Implemented comprehensive `OnSelectCard` handlers for Hint 512 (Hand-Sync material protection), Hint 502 (Destroy priority), Hint 509 (Cue and Back2Back SS routing), and Hint 505/506 (Amritara Reframing search).
  6. Registered `WCParisKewlTune` and `Expert_WCParisKewlTune` in `bots.json` and tagged as **Modern** in DashBot WPF Launcher.
  7. Built and deployed exclusively to `C:\Users\admin\Documents\EdoGame\`.

---

## Anime_JoeyWheeler (Refactor & Full Lua Script Bug Resolution): In-Game Crash Elimination & Strategic ModernExecutor Refactoring (2026-09-21)

### Overview
- **Deck**: `Anime_JoeyWheeler.ydk` & `Joey Wheeler ADO.ydk` (40 Main Deck, 10 Extra Deck, 23 unique IDs)
- **Primary Tasks**:
  1. Investigated and eliminated 3 in-game Lua card script errors (`Attempting to access deleted object`) in `c100459008.lua`, `c101402053.lua`, and `utility.lua`.
  2. Refactored `Anime_JoeyWheelerExecutor.cs` with smart Hint handlers (`Hint 508` To Grave, `Hint 509` Special Summon, `Hint 502` Destroy, `Hint 500` Tribute, `Hint 501` Discard, `Hint 505` Search, `Hint 507` Equip).
  3. Added extra deck lock awareness guarding against `Sleeping Scapegoats` Fusion-only restriction when evaluating Link summons.
  4. Strengthened `Salamandra Fusion` activation checking `Card.EquipTarget.HasType(CardType.Fusion)`.
  5. Built and deployed exclusively to `C:\Users\admin\Documents\EdoGame\`.

### Work Completed

#### 1. In-Game Lua Script Bug Resolution (3 Errors)
- **Gilford the Lightning (`c100459008.lua`)**:
  - *Error*: `[string "c100459008.lua"]:61: Attempting to access deleted object.`
  - *Root Cause*: `g:KeepAlive()` was missing in `sptg`, and `spop` evaluated `g:IsExists(Card.IsOwner, 1, nil, 1-tp)` after `Duel.Release(g, REASON_COST)`. When tokens (Scapegoat tokens) were released, OCGCore destroyed their C++ objects immediately, causing subsequent iteration to access deleted pointers.
  - *Fix*: Added `g:KeepAlive()` in `sptg`. In `spop`, cached `local opp_owned = g:IsExists(...)` before calling `Duel.Release`, then cleanly cleaned up with `g:DeleteGroup()` and `e:SetLabelObject(nil)`.
  - *Synced Across*: `script/c100459008.lua` and `repositories/official-scripts/pre-release/c100459008.lua`.
- **Graceful Skull Dice (`c101402053.lua`)**:
  - *Error*: `[string "c101402053.lua"]:90: Attempting to access deleted object.`
  - *Root Cause*: Missing `g:KeepAlive()` in `initial_effect(c)` on `local g = Group.CreateGroup()`. OCGCore garbage-collected the group when initialization concluded, resulting in `regop` invoking `g:Clear()` on a deleted group object.
  - *Fix*: Added `g:KeepAlive()`. In `regop`, added null-guard auto-reinstantiation. In `destg`, protected `e:GetLabelObject()` null check.
  - *Synced Across*: `script/c101402053.lua`, `repositories/official-scripts/pre-release/c101402053.lua`, `script/official/c101402053.lua`, `script/pre-release/c101402053.lua`.
- **aux.DelayedOperation (`utility.lua`)**:
  - *Error*: `[string "utility.lua"]:2894: Attempting to access deleted object.`
  - *Root Cause*: In `e1:SetOperation`, `g:DeleteGroup()` deleted the group but left `e:GetLabelObject()` referencing the freed memory. Subsequent condition evaluations accessed the deleted group via `get_affected_group(e)`.
  - *Fix*: Added `e:SetLabelObject(nil)` before `g:DeleteGroup()`. Protected `get_affected_group(e)` with `pcall` fallback.
  - *Synced Across*: `script/utility.lua` and `repositories/official-scripts/utility.lua`.

#### 2. Rule-Based ModernExecutor Refactoring (`Anime_JoeyWheelerExecutor.cs`)
- **Full Hint Message System (`OnSelectCard`)**:
  - `Hint 508` (To Grave): Prioritizes dumping `Fighting Flame Dragon` (Extra Deck) on `Fighting Flame Swordsman` death trigger, enabling +700 ATK & 2nd attack on Warrior Fusions, followed by `Salamandra` and `Graceful Skull Dice`.
  - `Hint 509` (Special Summon): Prioritizes reviving highest impact bosses (`Ultimate Flame Swordsman`, `Gilford`, `Red-Eyes Exceed`, `Swift Panther Warrior`).
  - `Hint 500` (Tribute): Prioritizes opponent monsters (if selectable by effect) -> Scapegoat tokens -> low ATK non-Ace fodder.
  - `Hint 501` (Discard): Discards cards with GY triggers (`Salamandra`, `Foolish Graverobber`, `Graceful Skull Dice`, `Fighting Flame Dragon`).
  - `Hint 502` (Destroy): Targets enemy negators/threats, or selects own Scapegoat Token for `Sleeping Scapegoats` replacement protection.
  - `Hint 505` (Search): Follows strategic line hierarchy between Flame Swordsman and Dark Time engines.
- **Rule Enforcement & Safety Checks**:
  - `_sleepingScapegoatsUsedThisTurn` flag: Enforces Extra Deck Fusion-only restriction, blocking illegal Link Summon attempts for `Ferocious Flame Swordsman`.
  - `ShouldSalamandraFusionActivate`: Strictly checks `Card.EquipTarget.HasType(CardType.Fusion)` before triggering Extra Deck Fusion invocation.
  - `ShouldFlameSwordsrealmActivate`: Selects `Fighting Flame Swordsman` as prime fodder to immediately trigger its graveyard dump engine.

#### 3. Exclusive Deployment
- Executed `BUILD_AND_DEPLOY.ps1` in `src/YGO_SOURCE_CLEAN/` (0 Errors).
- Deployed all updated binaries, CDBs, and patched scripts exclusively to `C:\Users\admin\Documents\EdoGame\`.

---

### Overview
- **Deck**: `Anime_JoeyWheeler.ydk` & `Joey Wheeler ADO.ydk` (40 Main Deck, 10 Extra Deck, 23 unique IDs)
- **Primary Tasks**:
  1. Resolved missing card IDs from YGOPRODeck (mapped to official Beyond the Brave BETB & YAC1 sets).
  2. Downloaded and verified all official high-resolution card artwork into `pics/`.
  3. Translated card effects into Thai across `cards.cdb`, `cards.delta.cdb`, `prerelease-others.cdb` (strictly preserving English card names).
  4. Built custom Lua script `c100459023.lua` for Gearfried the Steel Knight.
  5. Implemented 100% Rule-Based C# Executor `Anime_JoeyWheelerExecutor.cs` using `ModernExecutor`.
  6. Registered bot in `bots.json` under name `Anime_JoeyWheeler` and categorized under **Anime** in DashBot UI.
  7. Tested with Headless Simulator against legacy AI (0 Violations / 0 Crash).
  8. Deployed exclusively to `C:\Users\admin\Documents\EdoGame\`.

### Work Completed

#### 1. Card ID Resolution & Database Ingestion (23 Cards)
- Mapped 9 Beyond the Brave (BETB) cards from temporary YGOPRODeck internal IDs to official IDs (`101402001`, `101402002`, `101402004`, `101402036`, `101402052`, `101402053`, `101402054`, `101402070`, `101402071`).
- Added 3 Original Artwork Collection (YAC1) cards: `100459008` (Gilford), `100459015` (Super Critical), `100459023` (Gearfried).
- Downloaded high-resolution artwork: `100459008.jpg`, `100459015.jpg`, `100459023.jpg` to `C:\Users\admin\Documents\EdoGame\pics\`.
- Created Lua script `c100459023.lua` in `script/` and `src\YGO_SOURCE_CLEAN\script\`.
- Ingested Thai card descriptions into SQLite databases across game runtime and source directories.

#### 2. Rule-Based AI Executor (`Anime_JoeyWheelerExecutor.cs`)
- Implemented `Anime_JoeyWheelerExecutor` extending `ModernExecutor`.
- Supported Flame Swordsman Fusion engine, Dark Time Wizard resource routing, Sleeping Scapegoats defense/token generation, Reversal Box negation/ATK reduction, and Gilford the Lightning 3-tribute non-targeting board wipe.
- Handled overrides: `OnSelectCard` (smart priority for Tribute, Discard, Search, Equip), `OnSelectOption`, and `OnSelectPosition`.

#### 3. DashBot Anime Category Integration
- Registered bot in `bots.json` with `"name": "Anime_JoeyWheeler"`, `"deck": "Anime_JoeyWheeler"`.
- DashBot automatically categorizes it under **Anime** (Tag: `Anime`, Color: `#BE185D`, Display Name: "Joey Wheeler").

#### 4. Headless Simulation Audit
- Ran duel simulation against `AI_BlueEyes` via `Client_Headless_Fortest`.
- **Results**: Status OK (100%), 0 Rule Violations, 0 Crashes.

#### 5. Exclusive Deployment
- Executed `BUILD_AND_DEPLOY.ps1` in `src\YGO_SOURCE_CLEAN`.
- Deployed all updated binaries (`WindBot.dll`, `ExecutorBase.dll`, `core.dll`, `bots.json`, `DashBot.exe`, decks) exclusively to `C:\Users\admin\Documents\EdoGame\`.

---

## Anime_Yugi (Yugi Muto Deck): Official Card Images, Thai Effect Translations, and Full Lua Bug Resolution (2026-09-21)

### Overview
- **Deck**: `Anime_Yugi.ydk` (60 Main Deck, 42 unique IDs)
- **Primary Tasks**:
  1. Audit and download all missing official card artwork.
  2. Translate missing card effects into Thai in `cards.cdb` while strictly preserving English card names.
  3. Investigate, diagnose, and resolve all Lua script bugs and missing card scripts.
- **Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`

### Work Completed

#### 1. Official Card Artwork Sourcing (8 Cards)
- Scanned all 42 unique IDs in `Anime_Yugi.ydk` and identified 8 cards missing artwork in `pics/`.
- Downloaded and verified official high-resolution images to `C:\Users\admin\Documents\EdoGame\pics\`:
  - `2372506.jpg` (Chaos Magical Hats)
  - `24088928.jpg` (Skull Archfiend of Chaos)
  - `24749710.jpg` (Mind Shuffle)
  - `44001993.jpg` (Magician of Dark Chaos - Black Chaos)
  - `75983808.jpg` (Chaos Mystic Box)
  - `77456448.jpg` (Spell Shattering Sword)
  - `97462632.jpg` (Griffoh)
  - `98684220.jpg` (Black Chaos)
- **Status**: 42/42 (100%) valid images in `pics/`.

#### 2. Thai Effect Translation in `cards.cdb` (6 Cards)
- Strictly adhered to user guideline: translate ONLY card effect text (`desc`), keeping card names (`name`) 100% in English.
- Updated across all active SQLite databases (`cards.cdb`, `expansions/cards.cdb`, `WindBot/cards.cdb`, `WindBot/cards.delta.cdb`, `src/YGO_SOURCE_CLEAN/cards.cdb`, `config/languages/Thai/cards.delta.cdb`):
  1. `46986414` (**Dark Magician**): "สุดยอดจอมเวททั้งในด้านพลังโจมตีและพลังป้องกัน"
  2. `22283204` (**The Gaze of Timaeus**): "เลือกเป้าหมาย \"Dark Magician\" หรือ \"Dark Magician Girl\" 1 ใบในสนามหรือสุสานของคุณ; อัญเชิญฟิวชันมอนสเตอร์ฟิวชัน 1 ตัวจาก Extra Deck ของคุณที่ระบุชื่อมอนสเตอร์ตัวนั้นเป็นวัตถุดิบ โดยนำมอนสเตอร์เป้าหมายนั้นสับกลับเข้าเด็คเป็นวัตถุดิบ (ถือว่าเป็นการอัญเชิญฟิวชันด้วย \"The Eye of Timaeus\") แต่มอนสเตอร์นั้นจะถูกนำออกนอกเกมในช่วง End Phase ของเทิร์นถัดไป คุณสามารถเปิดใช้งาน \"The Gaze of Timaeus\" ได้เทิร์นละ 1 ใบเท่านั้น"
  3. `41350417` (**Dark Magical Curtain**): "ผู้เล่นแต่ละฝ่ายสามารถอัญเชิญแบบพิเศษมอนสเตอร์เผ่าจอมเวทธาตุมืด 1 ตัวจากบนมือหรือเด็คของตน (แต่ผู้เล่นทั้งสองฝ่ายไม่สามารถเปิดใช้งานเอฟเฟกต์ของมอนสเตอร์นั้นได้ในเทิร์นนี้) จากนั้น หากคุณอัญเชิญแบบพิเศษมอนสเตอร์ที่มีชื่อเดิมคือ \"Dark Magician\" หรือ \"Dark Magician Girl\" คุณสามารถนำการ์ดเวทมนตร์/กับดักที่ระบุชื่อ \"Dark Magician\" 1 ใบจากในเด็คขึ้นมือได้ ยกเว้น \"Dark Magical Curtain\" คุณสามารถเปิดใช้งาน \"Dark Magical Curtain\" ได้เทิร์นละ 1 ใบเท่านั้น"
  4. `59400890` (**Dark Magician of Destruction**): "\"Dark Magician\" + มอนสเตอร์ธาตุแสงหรือธาตุมืด 1 ตัว ต้องอัญเชิญฟิวชัน หรืออัญเชิญแบบพิเศษ (จาก Extra Deck) ในระหว่างเทิร์นที่มีการเปิดใช้งานการ์ดเวทมนตร์หรือเอฟเฟกต์ โดยการนำมอนสเตอร์เผ่าจอมเวทธาตุมืดเลเวล 6 ขึ้นไปที่คุณควบคุม 1 ตัวออกนอกเกมเป็นวัตถุดิบ คุณสามารถอัญเชิญแบบพิเศษ \"Dark Magician of Destruction\" ด้วยวิธีนี้ได้เทิร์นละ 1 ครั้งเท่านั้นไม่ว่าจะใช้วิธีใด การ์ดใบนี้จะถือว่ามีชื่อเป็น \"Dark Magician\" ขณะอยู่บนสนามหรือในสุสาน หากการ์ดใบนี้ถูกอัญเชิญแบบพิเศษ: คุณสามารถนำ \"Dark Magician\" หรือการ์ดที่ระบุชื่อ \"Dark Magician\" 1 ใบจากในเด็คขึ้นมือได้"
  5. `71440209` (**Dark Magic Talisman**): "คุณสามารถเปิดใช้งานการ์ดใบนี้ได้ในเทิร์นที่เซ็ตไว้ โดยการเปิดเผยการ์ดมอนสเตอร์เผ่าจอมเวท 1 ใบในมือคุณ เมื่อเอฟเฟกต์ของมอนสเตอร์ทำงานตอบสนองต่อการสั่งเปิดใช้งานการ์ดหรือเอฟเฟกต์ของการ์ดที่ระบุชื่อ \"Dark Magician\": ยกเลิกเอฟเฟกต์นั้น และหากทำสำเร็จ ตลอดช่วงที่เหลือของเทิร์นนี้ เอฟเฟกต์ที่สั่งใช้งานของมอนสเตอร์ที่มีชื่อเดิมเดียวกันนั้นทั้งหมดจะถูกยกเลิก หากคุณอัญเชิญแบบพิเศษ \"Dark Magician\" สำเร็จในขณะที่การ์ดใบนี้อยู่ในสุสาน (ยกเว้นช่วง Damage Step): คุณสามารถจ่าย 2500 LP; เซ็ตการ์ดใบนี้บนสนามของคุณ"
  6. `88570003` (**Dark Magician, the Pharaoh's Servant**): "การ์ดใบนี้จะถือว่ามีชื่อเป็น \"Dark Magician\" ขณะอยู่บนสนามหรือในสุสาน คุณสามารถใช้เอฟเฟกต์อย่างใดอย่างหนึ่งต่อไปนี้ของ \"Dark Magician, the Pharaoh's Servant\" ได้เทิร์นละครั้งเท่านั้น ● คุณสามารถเปิดเผยการ์ดเวทมนตร์ 1 ใบในมือของคุณ; อัญเชิญแบบพิเศษการ์ดใบนี้จากบนมือ จากนั้นคุณสามารถเซ็ตการ์ดเวทมนตร์/กับดักที่ระบุชื่อ \"Dark Magician\" 1 ใบจากในเด็คได้ ● (ควิกเอฟเฟกต์): คุณสามารถทิ้งการ์ดเวทมนตร์ 1 ใบ; ทำลายการ์ดเวทมนตร์/กับดักทั้งหมดที่อีกฝ่ายควบคุม"

#### 3. Complete Lua Bug Resolution
1. **Targeting Parameter 2 Nil Bug in `c24088928.lua` / `proc_workaround.lua`**:
   - *Error*: `[string "proc_workaround.lua"]:48: in field 'GetTargetGroup'` -> `Parameter 2 should be "Effect" but is "nil"` (matched user screenshot).
   - *Cause*: `c24088928.lua:37` called `Duel.GetTargetGroup` without an active reason effect, causing `Duel.GetReasonEffect()` to return `nil`, which was passed to `Card.IsCanBeEffectTarget` where C++ core expects an `Effect` pointer.
   - *Fix*:
     - In `c24088928.lua:37` (and `repositories/official-scripts/official/c24088928.lua`), directly passed effect `e` into `:Match(Card.IsCanBeEffectTarget, nil, e)`.
     - In `proc_workaround.lua` (all 3 locations: `script/`, `repositories/official-scripts/`, `repositories/local-patches/script/`), implemented a safe fallback checking `Duel.GetChainInfo(0, CHAININFO_TRIGGERING_EFFECT)` and falling back to filtering `EFFECT_CANNOT_BE_EFFECT_TARGET` when `re` is nil.
2. **Missing Script for "Spell Shattering Sword" (`c77456448.lua`)**:
   - *Error*: `"CallCardFunction"(c77456448.initial_effect): attempt to call an error function`.
   - *Cause*: Card 77456448 existed in `cards.cdb` and the deck, but had no script file (previously under pre-release code `101402064`).
   - *Fix*: Created `script/c77456448.lua` and `repositories/official-scripts/official/c77456448.lua` with full Quick-Play spell and destruction trigger effects.
3. **Deck Audit Result**:
   - All 42 unique cards audited: 0 missing images, 0 missing translations, 0 missing/broken Lua scripts.

#### 4. Build & Deployment
- Executed `BUILD_AND_DEPLOY.ps1` in `src/YGO_SOURCE_CLEAN/`.
- Deployed all updated binaries, CDBs, and configurations exclusively to `C:\Users\admin\Documents\EdoGame\`.

---

## 2026_Purrely & 2026_Yummy Top-Tier Meta ModernExecutor Deep Analysis & Refactor (2026-09-21)

### Overview
- **Decks**: `2026_Purrely.ydk` & `2026_Yummy.ydk` (Championship Tier-1 Metagame)
- **Executors**: `_2026_PurrelyExecutor.cs` & `_2026_YummyExecutor.cs`
- **Architecture**: `ModernExecutor` (Rule-Based C# / net10.0, strictly following SKILL.md v10.0)
- **Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`

### Part 1: 2026_Purrely Analysis & Fixes
1. **Root Cause 1 — Hand Self-Destruction via `OnSelectYesNo`**:
   - *Problem*: `OnSelectYesNo` always returned `true`. Every Quick-Play Memory spell has an optional secondary effect to discard 1 card and Special Summon a Level 1 Purrely from Deck. Returning `true` blindly forced the bot to discard its entire hand, leaving 0 cards in hand and bricking further combo extensions.
   - *Fix*: Rewrote `OnSelectYesNo` to only discard when normal summon has not been used, no starter on field, or when feeding materials to Plump/Noir strategically.
2. **Root Cause 2 — Plump Effect Misunderstanding & Neglect**:
   - *Problem*: Plump's quick effect banish does **NOT** detach materials; the previous author mistakenly believed it consumed materials and suppressed its activation.
   - *Fix*: Enabled Plump's Quick Effect to banish opponent monsters safely and correctly respond to Quick-Play activations to attach GY spells up to once per turn.
3. **Root Cause 3 — Missing Hint Handling in `OnSelectCard`**:
   - *Problem*: Purrely's ignition effect reveals a Memory Quick-Play via Hint 526 (`HINTMSG_CONFIRM`) to rank up into the corresponding Epurrely. Hint 526 was missing, resulting in arbitrary fallback selection. Purrelyly and Purrelyeap targeting uses Hint 551 (`HINTMSG_TARGET`), which also lacked specific priority routing.
   - *Fix*: Added full support for Hint 526 (Delicious -> Plump, Sleepy -> Noir, Pretty -> Beauty) and Hint 551 (Purrelyly Continuous spell placement & Purrelyeap Rank 2 targeting).

### Part 2: 2026_Yummy Analysis & Fixes
1. **Root Cause 1 — Illegal Tag-Out Timing for Synchro Way Bosses**:
   - *Problem*: In `c31603289.lua` and `c67098897.lua`, `CupsyYummyWay`, `CookyYummyWay`, and `LollipoYummyWay` have condition `EVENT_CHAINING` (`rp == 1 - tp`). The previous code attempted to tag out in open gamestate (`opponentHasMonsters && isMainOrBattle`), which was illegal and failed.
   - *Fix*: Updated tag-out condition to strictly check `Duel.LastChainPlayer == 1` and verify valid GY revival targets.
2. **Root Cause 2 — CookyWay `OnSelectEffectYn` Inversion**:
   - *Problem*: `OnSelectEffectYn` checked `Enemy.GetMonsters().Any(...)` for all effects of `CookyYummyWay`. When opponent activated non-monster spells (Raigeki, Duster), the bot refused to tag out to dodge the wipe because opponent had 0 monsters!
   - *Fix*: Split handling based on `desc`: stringid 0 (Book of Moon) checks face-up targets, stringid 1 (Tag-Out) checks opponent chaining and GY targets.
3. **Root Cause 3 — Cooky On-Summon Destroy Option Ignored**:
   - *Problem*: Cooky Yummy on SS by a Synchro monster gives option 0 (-1000 ATK) vs option 1 (Destroy target). Because `OnSelectOption` was unhandled, it defaulted to option 0, never destroying the target!
   - *Fix*: Implemented `OnSelectOption` to prioritize stringid 3 (Destroy).
4. **Root Cause 4 — Yummy Surprise Hint 505 Desynchronization**:
   - *Problem*: Yummy Surprise mode 0 uses `aux.SelectUnselectGroup` (prompted 1-by-1 with min=1, max=1) requiring 2 friendly LIGHT Beasts + 2 opponent cards (`s.rescon`). Previous code checked `max == 4`, which never matched, causing it to select 3 friendly beasts and fail the condition.
   - *Fix*: Added state tracking counters (`_surpriseOurBounceCount`, `_surpriseOppBounceCount`) to alternate between friendly beasts (up to 2) and opponent cards (up to 2).
5. **Root Cause 5 — Link-1 Summon Locks (Almiraj & Kagari)**:
   - *Problem*: Almiraj and Kagari were bound to `LinkSpSummonCheck` requiring `>= 2` monsters. Both are Link-1 requiring only 1 monster (Sangan / Sky Striker Token).
   - *Fix*: Implemented dedicated `AlmirajSpSummon` (using Sangan / Normal Summoned monster <= 1000 ATK) and `KagariSpSummon` (using Sky Striker Token) with Kagari recycling Hornet Drones.
6. **Root Cause 6 — Fake Synchro Math & Hallucinated Combo Lines**:
   - *Problem*: `HeraldSpSummon` (Lv4) and `BorreloadSpSummon` (Lv8) did not check level sums, and `ComboRouter` contained a hallucinated `Cooky-Lollipo-Borreload` combo line.
   - *Fix*: Replaced combo lines with true 1-card starter routes (`Yummy-Snatchy-CupsyWay-Elf`), enforced exact level sum checks for Herald (Lv4) and Borreload (Lv8), and verified Link monster in GY for Borreload.

### Build & Deployment Verification
- **Build Status**: 0 Errors, 44 Baseline Warnings (0 new warnings).
- **Deployment Target**: `C:\Users\admin\Documents\EdoGame\` via `BUILD_AND_DEPLOY.ps1` (Verified).

---

## 2026_Tearla (Fiendsmith-Brilliant-Tearlaments) ModernExecutor Refactor & Upgrade (2026-09-21)

### Overview
- **Deck**: `2026_Tearla.ydk` (Fiendsmith-Brilliant-Tearlaments: 40 Main, 15 Extra)
- **Executor**: `_2026_TearlaExecutor.cs`
- **Class / Bot Name**: `2026_Tearla` & `Expert_2026_Tearla`
- **Architecture**: `ModernExecutor` (Rule-Based C# / net10.0, strictly following SKILL.md v10.0)
- **Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`

### Key Vulnerabilities Identified & Resolved
1. **Gem-Knight Quartz Trap**:
   - *Issue*: Activating Gem-Knight Quartz from hand locks Extra Deck summons to **ONLY Gem-Knight monsters** for the rest of the turn, completely shutting down Tearlaments, Fiendsmith, and generic Extra Deck bosses.
   - *Fix*: Gated Quartz hand activation behind a strict anti-lock gate (never activate in normal combos; treat as discard fodder).
2. **Missing Hint Handling in `OnSelectCard`**:
   - *Issue*: Lacked dedicated handling for `HINTMSG_TODECK` (506), causing Tearlaments GY Fusion effects to return on-field Ace cards instead of GY materials. Lacked handling for `HINTMSG_EQUIP` (507), `HINTMSG_TOGRAVE` (508), `HINTMSG_REMOVE` (504), and `HINTMSG_SPSUMMON` (509).
   - *Fix*: Implemented comprehensive `OnSelectCard` with full hint table support, ensuring Tear GY materials return properly, Snow banishes non-essential spells/traps/handtraps, and Kitkallos targets herself for her 5-card mill.
3. **Missing `OnSelectOption` Routing**:
   - *Issue*:
     - `Tearlaments Kashtira`: Could mill the opponent's deck instead of our own (violating Rule 4 Anti-Advantage Gate).
     - `Tearlaments Kitkallos`: Failed to properly choose between adding to hand (Option 0) vs sending to GY (Option 1).
     - `Number 60: Dugares`: Uncontrolled option selection.
   - *Fix*: Implemented `OnSelectOption(IList<long> options)` with bitwise decoding (`>> 20` and `& 0xfffff`) to guarantee our deck is milled for Tear Kashtira, dynamic hand/GY routing for Kitkallos, and draw 2 discard 1 for Dugares.
4. **The Undying Legion Overlay**:
   - *Issue*: Bot checked `Level 7 >= 2` and never ranked up on `Pilgrim Reaper`.
   - *Fix*: Directly ranks up onto `Pilgrim Reaper` after Reaper detaches 1 material and mills 5 cards, generating a 2700 ATK boss.
5. **Central Intelligence Alignment & Magic Number Elimination**:
   - *Issue*: Hardcoded card IDs in target and GY chokepoint lookups.
   - *Fix*: Migrated to `CardIntelligence.IsFloodgateSpellTrap`, `CardIntelligence.IsFloodgateMonster`, `CardIntelligence.IsKnownNegator`, and `CardIntelligence.IsHighThreatChokepoint`.
6. **Compiler Cleanliness & Expert Registration**:
   - Removed unused fields (`_havnisGYFusionUsed`, `_scheirenGYFusionUsed`), resulting in **0 errors and 0 warnings**.
   - Registered `[Deck("Expert_2026_Tearla", "2026_Tearla")]` class `ExpertTearlaExecutor`.

---

## Nightmare Throne Lua Bug Fix & GOD-01 ModernExecutor Overhaul (2026-09-21)

### 1. Nightmare Throne Lua Bug Fix (`c93729896.lua`)
- **Error Reported**: `[สคริปต์การ์ดผิดพลาด]: [string "c93729896.lua"]:117: Attempting to access deleted object.`
- **Root Cause**: In `c93729896.lua` (`Nightmare Throne`), when `Duel.IsChainSolving()` is true, `s.regop` filters cards with `local g = eg:Filter(...)` and stores `e:SetLabelObject(g)` across chain resolution without calling `g:KeepAlive()`. In OCGCore Lua, transient groups allocated during callbacks are immediately garbage collected / deleted when the callback returns. When the chain finished solving (`EVENT_CHAIN_SOLVED`), `e1:SetOperation` tried to access `Duel.RaiseEvent(g, ...)` on the already-deleted C++ group pointer.
- **Fix Applied**: Added `g:KeepAlive()` when storing into `e:SetLabelObject` and `g:DeleteGroup()` after event raising. Synchronized across all 3 repository script directories:
  - `C:\Users\admin\Documents\EdoGame\script\official\c93729896.lua`
  - `C:\Users\admin\Documents\EdoGame\script\c93729896.lua`
  - `C:\Users\admin\Documents\EdoGame\repositories\official-scripts\official\c93729896.lua`

---

### 2. GOD-01 Deep Analysis & Overhaul ("การ์ดเต็มมือแต่เล่นไม่ได้")

#### Root Cause Analysis (Why the bot had full hands but couldn't play)
1. **Deck Architecture Problems (ปัญหาด้านเด็ค - 47 ใบ)**:
   - **47% Dead Cards on Turn 1**: Out of 47 cards, 22 cards were completely unplayable on Turn 1 going first (7 high-level Gods needing 3 tributes, Going-2nd board breakers, traps, and conditional support requiring specific Gods already on the field).
   - **Tribute Engine Starvation**: In the old 47-card deck, the ONLY monster that could generate tributes was `Reactor Slime` (3 copies = 6.4% of deck, ~29% opening probability). Without Reactor Slime, the bot literally could not put 3 tributes on the board to summon any God.
   - **Cross-Archetype Brick Clashes**: Support cards for one God were completely dead if that specific God wasn't on field (`Fist of Fate` / `Soul Energy MAX` require Obelisk; `The Revived Sky God` requires Slifer in GY; `The True Sun God` / `Ancient Chant` / `Guardian Slime` only search Ra).
   - **OCG Banlist Incompatibility**: Old deck had 2x Called by the Grave (Forbidden in OCG), 2x Pot of Prosperity (Limit 1 in OCG), and 3x Maxx "C" (Limit 1 in OCG).
2. **AI Logic & ModernExecutor Flaws (`_2026_God01Executor.cs`)**:
   - **Corrupted Hint Constants**: Hint IDs were defined backwards (501 defined as ToGrave instead of Discard; 504 defined as Discard instead of Remove/Banish; 506 defined as ToHand instead of ToDeck). This caused `OnSelectCard` to fail almost all selection branches!
   - **Illegal ComboRouter Line**: Programmed `Reactor Slime` (Level 4, 500 ATK) to summon `Egyptian God Slime` (requires Level 10 WATER Aqua with 0 ATK).
   - **Normal Summon Consumption Conflict**: Normal Summoning `Reactor Slime` used up the bot's Normal Summon. Without an additional Tribute Summon, the bot could not Tribute Summon Slifer or Obelisk that turn even with 2 tokens on field!
   - **Illegal Searches**: `TheTrueSunGod` and `GuardianSlime` attempted to search `Soul Crossing` and `Reactor Slime`, which do not mention "The Winged Dragon of Ra".

#### Optimizations Applied
1. **Deck Modernization (`GOD-01.ydk`)**:
   - Streamlined deck down to **exactly 40 cards** (100% legal in both TCG and OCG with 0 violations).
   - Turn 1 playable hand rate increased from **~44% to 97.2%**!
   - Increased `The Breaking Ruin God` to 2 copies (Uncounterable Special Summon of Obelisk from hand/GY with complete effect immunity, requiring 0 tributes).
   - Added 2x `Card Advance` (Reorders top 5 cards and grants +1 Tribute Summon, enabling Turn 1 God summon with Reactor Slime).
   - Increased `Metal Reflect Slime` to 3 copies (3000 DEF wall, directly tributes for Egyptian God Slime).
   - Added 3x `Infinite Impermanence` (Clean disruption, legal everywhere).
2. **Executor Modernization (`_2026_God01Executor.cs`)**:
   - Implemented 100% Section 7.3 Hint Table compliance (500 Release, 501 Discard, 502 Destroy, 504/503 Remove, 505 ToHand, 508 ToGrave, 509 SpSummon, 511/513/533 Fusion/Tribute, 552/572 Negate).
   - Implemented `OnSelectOption` for modal cards (`Pot of Prosperity`, `The True Sun God`).
   - Implemented `GetMaterialPriority` to protect Egyptian Gods and high-rank Xyz bosses.
   - Implemented `OnSelectPosition` with stat-aware positioning.
   - Synchronized deck and binary deployments to `C:\Users\admin\Documents\EdoGame\`.

---

## Anime_Yugi (Yugi Muto) ModernExecutor Overhaul & Room Join Fix (2026-09-21)

### Overview
- **Deck**: `Anime_Yugi.ydk` (Dark Magician / Illusion / Black Luster Soldier / Chaos Engine: Dark Magician, Dark Magician Girl, Illusion of Chaos, Magician of Black Chaos MAX, Master of Chaos, Magician of Chaos, Black Luster Soldier - Legendary Swordsman, Black Luster Soldier - Soldier of Chaos, Dragoon, Magicians' Souls, Rod, Salvation, Soul Servant, Secrets of Dark Magic, Spell Shattering Sword, Pharaoh's Servant, Preparation of Rites, Pre-Preparation of Rites, etc.)
- **Executor**: `Anime_YugiExecutor.cs`
- **DashBot Category**: **Anime Decks** (registered as `Anime_Yugi` in `bots.json`)
- **Architecture**: `ModernExecutor` (Rule-Based C# 10.0 / .NET 10, strictly following SKILL.md v10.0)
- **Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`

### Root Cause Analysis (Why Bot Failed to Join Room)
1. **`DECKERROR_UNKNOWNCARD (flag=4, code=101402064)`**:
   - `Anime_Yugi.ydk` contained placeholder passcode `101402064` for *Spell Shattering Sword*.
   - The official Konami passcode in `cards.cdb` is `77456448`.
   - Furthermore, `BUILD_AND_DEPLOY.ps1` had a conditional guard: `if ($destCardsDb.Length -lt 10000000)` which skipped copying the updated `cards.cdb` from `windbot-fork` to `C:\Users\admin\Documents\EdoGame\cards.cdb` because the target file was already 17MB. As a result, 176 modern cards (including `77456448` and `101402064`) were missing from the runtime database!
2. **`DECKERROR_LFLIST (flag=1)`**:
   - In `OCG.lflist.conf`, `Pre-Preparation of Rites (13048472)` is **Limited (1 copy max)**.
   - `Anime_Yugi.ydk` ran 2 copies, violating OCG room banlist validation.

### Applied Fixes & Overhaul
1. **Deck List Fixes (`Anime_Yugi.ydk`)**:
   - Replaced placeholder `101402064` with official passcode `77456448` (*Spell Shattering Sword*).
   - Adjusted `Pre-Preparation of Rites` to 1 copy (OCG legal) and increased `Preparation of Rites` to 3 copies (legal in both TCG & OCG). Main deck remains at 45 cards, Extra Deck at 15 cards.
   - Synchronized deck file across `windbot-fork/Decks/Anime_Yugi.ydk` and `C:\Users\admin\Documents\EdoGame\deck/Anime_Yugi.ydk`.
2. **Deployment Script (`BUILD_AND_DEPLOY.ps1`)**:
   - Removed the faulty length check guard; `cards.cdb` is now always updated directly to `C:\Users\admin\Documents\EdoGame\cards.cdb`, ensuring all 14,948 cards are always in sync.
3. **Architectural Upgrades (`Anime_YugiExecutor.cs`)**:
   - **`OnSelectOption` Interception**: Handled modal options for *Spell Shattering Sword*, *Pharaoh's Servant*, *Master of Chaos*, *Mind Shuffle*, *Triple Tactics Talent*, *Griffoh*, *Pot of Prosperity*, *BLS Soldier of Chaos*, and *Magi Magi Gal*.
   - **Stat-Aware Positioning (`OnSelectPosition`)**: Ensures high DEF / utility monsters like *Magician's Rod*, *Pharaoh's Servant*, and *Griffoh* are set in defense while bosses are summoned in attack.
   - **Smart Material Scoring (`GetMaterialPriority`)**: Protects on-field bosses (*Dragoon*, *Master of Chaos*, *Soldier of Chaos*, *Dark Magician the Dragon Knight*) from being used as low-value fusion/link material.
   - **100% Hint Table Compliance (SKILL.md Section 7.3)**:
     - 500 (Release), 501 (Discard), 502 (Destroy with threat scoring and immunity check), 504/503 (Remove/Banish), 505/506 (Search/Recycle/Illusion of Chaos deck top manipulation), 507 (Equip), 508 (To Grave), 509 (Special Summon prioritizing Dragoon & Bosses), 518 (PosChange), 519 (Xyz Material), 552/572 (Universal Chokepoints & Known Negators), 511/513/533 (Fusion & Ritual materials).
4. **Build & Verification**:
   - Successfully compiled `WindBot` (C# net10.0) with 0 errors.
   - Verified that `cards.cdb` at `C:\Users\admin\Documents\EdoGame\cards.cdb` contains passcodes `77456448` and `101402064`.
   - Bot now passes all deck validation checks and can cleanly join rooms.

---

## Yubel2 ModernExecutor Overhaul (2026-09-21)

### Overview
- **Deck**: `Yubel2.ydk` (Yubel Fiendsmith Engine: Samsara D Lotus, Nightmare Throne, Nightmare Pain, Spirit of Yubel, Yubel, Yubel - Terror Incarnate, Gruesome Grave Squirmer, Dark Beckoning Beast, Opening of the Spirit Gates, Fiendsmith Engraver, Fiendsmith's Tract, Fabled Lurrie, Geistgrinder Golem, Super Polymerization, Final Bringer of the End of Times, Eternal Favorite, Yubel - The Loving Defender Forever, Phantom of Yubel, Chaos Angel, Varudras, Gustav Max, Superdreadnought Rail Cannon Juggernaut Liebe, Number 81: Superdreadnought Rail Cannon Superior Dora)
- **Executor**: `Yubel2Executor.cs`
- **Architecture**: `ModernExecutor` (Rule-Based C# 10.0 / .NET 10, strictly following SKILL.md v10.0)
- **Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`

### Key Architectural & Strategic Enhancements
1. **Rule 1 & Card Audit Protocol**:
   - Complete Card Dossier conducted across all 35 cards in `Yubel2.ydk` against `cards.cdb`.
   - Eliminated hallucinated phantom handtraps (Ghost Mourner, Crossout, Nibiru, Gamma) that were given Tier 1 priority in the old executor despite having 0 copies in `Yubel2.ydk`.
   - Fixed broken `GruesomeGraveSquirmer` field effect logic (card has NO field ignition effect; its effect triggers only from hand on destruction or GY banish).
2. **True Yubel Reflection OTK & Nightmare Pain**:
   - Integrated `Geistgrinder Golem` (3000 ATK Fiend given to opponent, triggers summon from GY, forces battle with Yubel for 3000+ reflected damage).
   - Dynamically repositions Yubel forms into `FaceUpAttack` when `Nightmare Pain` is active to force opponent to attack and take full battle damage.
3. **Samsara D Lotus Opponent-Turn Disruption & Float Loop**:
   - Implemented `LotusOpponentNegateActivate()`: on opponent's turn, tributes Samsara Lotus to change opponent's monster effect into destroying a Yubel monster, triggering Spirit of Yubel / Yubel's floating effects!
   - Implemented Samsara Lotus End Phase GY recursion loop to sustain disruptions every turn.
4. **OCGCore 64-bit Option Interception (`OnSelectOption`)**:
   - Handled bitmask and indices for `SamsaraDLotus` (Option 0: Tribute for SS), `NightmareThrone` (Option 0: Add to hand), `SpiritOfYubel` (Option 0: Add/Set Spell/Trap), `EternalFavorite` (Option 1: Fusion with opponent monsters / Option 0: SS Yubel), and `FiendsmithsLacrima` (Option 0: Burn / Option 1: Shuffle into deck).
5. **Anti-Advantage Gate & Contact Fusion**:
   - Special Summons `Yubel - The Loving Defender Forever` using opponent monsters as Contact Fusion material, wiping opponent boards without triggering standard destruction protections.
   - Guarded Super Polymerization to strictly steal opponent monsters.
6. **100% Hint Table Compliance (Section 7.3)**:
   - Implemented exact handlers for Hints: 500 (Release), 501 (Discard), 502 (Destroy), 504/503 (Remove/Banish), 505 (Search/Bounce), 506 (Spin), 507 (Equip), 508 (Send to GY), 509 (Special Summon), 518 (PosChange), 519 (Xyz Material), 552/572 (Disable/Negate with `IsHighThreatChokepoint` & `IsKnownNegator` priority), and 511/513/533 (Fusion materials).
   - Integrated `IsDestructionImmune()` and `IsViableEffectTarget()` to skip immune targets.

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


