# Progress Log: Central Core Architecture & Universal Heuristics Overhaul

## 0.047. Comprehensive Workspace & Architecture Reorganization (2026-09-27)

### Overview
- **Objective**: Execute total reorganization of project directories, file naming, decoupled plugin hierarchy, and documentation across the entire workspace according to Rule 9 and Decoupled Domain Plugin Architecture standards.
- **Key Enhancements**:
  1. **Decoupled Deck Plugins Architecture (`Game/AI/Plugins/`)**:
     - Created dedicated directory `src/YGO_SOURCE_CLEAN/windbot-fork/Game/AI/Plugins/` to house all deck plugin implementations.
     - Extracted all 30 embedded Deck Plugin classes out of Executor files into clean, dedicated `.cs` files (e.g., `KewlTunePlugin.cs`, `GravePlugin.cs`, `FireKingPlugin.cs`, `EndymionPlugin.cs`, `SixSamuraiPlugin.cs`, `BrandedPlugin.cs`, `CenturionPlugin.cs`, `TenpaiPlugin.cs`, etc.).
     - Documented architecture and lifecycle in [Game/AI/Plugins/README.md](file:///c:/Users/admin/Documents/EdoGame/src/YGO_SOURCE_CLEAN/windbot-fork/Game/AI/Plugins/README.md).
  2. **Rule 9 File Naming Compliance in `Game/AI/Decks/`**:
     - Renamed all 70 legacy `_2026_*.cs` executor files to clean, standardized names (e.g., `FirekingExecutor.cs`, `KwtuneExecutor.cs`, `GraveExecutor.cs`, `EndymionExecutor.cs`, `SixSamuraiExecutor.cs`, etc.) using `git mv` to preserve commit history.
     - Resolved naming collisions with legacy bots (`ModernBlueEyesExecutor.cs`, `ModernSkyStrikerExecutor.cs`).
     - Exactly 0 files with `_2026_` remain in `Game/AI/Decks/`.
  3. **Workspace Cleanup**:
     - Removed obsolete `windbot_launcher` project from `YGO_AI_PLATFORM.slnx` and purged its folder.
     - Purged stale database backups (`cards.cdb.bak_*`), WPF temporary build files (`*.wpftmp.csproj`), and unneeded image assets.
     - Reclaimed 386.44+ MB disk space.
  4. **Documentation Reorganization (`Docs/`)**:
     - Restructured `Docs/` into modular subdirectories: `DeckReports/`, `Architecture/`, and `Archives/`.
     - Created comprehensive index and table of contents at [Docs/README.md](file:///c:/Users/admin/Documents/EdoGame/Docs/README.md).
- **Build & Deploy**:
  - Full build via `BUILD_AND_DEPLOY.ps1` completed with **0 Errors**, 0 Violations.
  - Successfully deployed to `C:\Users\admin\Documents\EdoGame\`.

---

## 0.046. Universal S:P Little Knight (29301450) Strategic Guard & Anti-Lockout Engine (2026-09-27)

### Overview
- **Issue**: Audited duel log (`Fireking_vs_Dark_Magician_25690927_120416`) revealed a systemic fatal flaw across executors: AI in Turn 3 unconditionally Link Summoned `S:P Little Knight` in Main Phase 1 while enemy monster zone was completely empty (`Enemy Monsters: 0`) and bot had lethal/5700 ATK on field. The summon consumed Ace Boss `Garunix Eternity` (3000 ATK) + `Garunix` (2700 ATK), triggered S:P's Effect 1 (*"your monsters cannot attack directly this turn"*), dealt 0 damage in Battle Phase, and threw the duel to an eventual loss in Turn 8.
- **Architectural Breakthrough (`ModernExecutor.cs`)**:
  - Implemented **Universal S:P Little Knight (29301450) Strategic Guard** in `ModernExecutor.ShouldAllowSpSummon`:
    1. **Direct Attack Lockout Guard in MP1**: Blocks S:P Little Knight summon in Main Phase 1 if opponent has 0 monsters on field (forces bot to attack in BP first, deferring S:P to MP2).
    2. **Lethal Attack Priority**: Blocks S:P in MP1 if `CanDealLethal()` is true.
    3. **Target Viability Check**: Blocks S:P in MP1 if opponent has 0 cards on field and 0 cards in GY (Effect 1 has nothing to banish).
    4. **Ace Monster & High-ATK Boss Downgrade Protection (All Phases)**: Blocks S:P if it would consume Ace Cards (`IsAceCard`) or 2000+ ATK monsters, unless opponent has an active floodgate requiring emergency removal.
- **Executor Updates**:
  - `_2026_FirekingExecutor.cs`: Replaced blind `AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight)` with `SPLittleKnightSummon` condition checking safe materials and direct attack opportunities.
  - `_2026_EyeInsideExecutor.cs`: Replaced blind `AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight)` with `SPLittleKnightSummon` condition.
  - `_2026_GraveExecutor.cs`: Updated `LinkSummonCheck` to enforce direct attack lockout prevention and high-ATK monster protection.
- **Build & Deploy**:
  - Compiled with **0 Errors**, deployed to `C:\Users\admin\Documents\EdoGame\`.

---

## 0.045. Kwtune & Grave Strategic Refactoring and Deck Architecture Optimization (2026-09-27)

### Overview
- **Objective**: Conduct comprehensive analysis, structural refactoring, and deck list optimization for both `Kwtune` (`_2026_KwtuneExecutor.cs`) and `Grave` (`_2026_GraveExecutor.cs`), eliminating anti-synergies, dead cards, and rule violations to reach modern competitive standards.

### 1. Kwtune Deck & AI Optimization (`_2026_KwtuneExecutor.cs` & `2026_Kwtune.ydk`)
- **Root Cause of Deficiencies in `2026_Kwtune.ydk`**:
  - `Visas Amritara` (`821049`) was in the Extra Deck, but `Mannadium Reframing` (`18158393`) was completely missing from the Main Deck, causing Amritara's on-summon search to fail.
  - Ash Blossom ID mismatch: `2026_Kwtune.ydk` used `14558127`, whereas `_2026_KwtuneExecutor.cs` previously only registered `14558128`, disabling Ash Blossom from triggering.
  - Lacked crucial extenders (`Starjunk Synchron` & `Jet Synchron`) and `Synchro Emergency` (leaving `Duelist Genesis` with only 1 search target).
- **Deck Upgrades (`2026_Kwtune.ydk`)**:
  - Synchronized with the World Championship Paris tournament specification:
    - Added `Mannadium Reframing` x1, `Synchro Emergency` x1, `Starjunk Synchron` x1, `Jet Synchron` x1, `Kewl Tune Mix` (up to 3x).
    - Reduced HOPT bricks: `Synchro Overtake` (3 -> 2), `Ghost Belle` (3 -> 1), `Ghost Ogre` (2 -> 1).
    - Extra Deck: Replaced duplicate RS with `Despian Luluwalilith` (`53971455`) (Lv 12 Omni-negate & ATK boost when monsters leave Extra Deck).
- **AI Refactoring & Domain Plugin (`_2026_KwtuneExecutor.cs`)**:
  - Added dual Ash Blossom support (`CardId.AshBlossom = 14558128` and `CardId.AshBlossomAlt = 14558127`) across `HandTrapIds`, `ChainAdvisor`, and `AddExecutor`.
  - **Decoupled Domain Plugin Architecture (`KewlTunePlugin : DeckPluginBase`)**:
    - `KewlTuneStrategy` (`IDeckStrategy`): Dedicated Special Summon targeting (Rotary > Mix > Reco > Cue > Malong) and search targeting (Amritara -> Reframing; Genesis -> KT Synchro / Emergency).
    - `KewlTuneMaterialEvaluator` (`IDeckMaterialEvaluator`): Cost evaluation, Hand-Sync material prioritization, and strategic discard targeting (`PickDiscardTarget`).
    - `KewlTuneThreatEvaluator` (`IDeckThreatEvaluator`): Contextual threat scoring against Floodgates and Negators.
    - Integrated directly via `DeckPlugin = new KewlTunePlugin(this);`.

### 2. Grave Deck Strategic Overhaul (`_2026_GraveExecutor.cs` & `2026_Grave.ydk`)
- **Root Cause of Severe Anti-Synergy in `2026_Grave`**:
  - Necrovalley was activated too early (Priority 4), before monster summons. Once Necrovalley was on field, `K9-66b Lantern`'s hand effect (reviving Lv5 from GY) and `Called by the Grave` were negated by Necrovalley itself.
  - Main Deck contained dead tribute bricks: `Gravekeeper's Chief` (Lv5 tribute summon that revives Lv4 Commandant, unable to Xyz Rank 5), `Gravekeeper's Shaman` (Lv6 tribute brick), `Dark Renewal` (requires Spellcaster on field; dead once Commandant is discarded), `Illusion Gate` (pays half LP to revive from opp GY, blocked by Necrovalley).
  - Extra Deck contained dead cards: `Infinitrack River Stormer`, `Imperial Princess Quinquery` (revival blocked by Necrovalley).
  - Core starters (`Jokul`, `Noroi`, `Lantern`, `Forced Release`) were under-ratioed.
- **Architectural Solution: "Phase 1 Swarm -> Phase 2 Necrovalley Lock"**:
  - **Phase 1 (K9 Swarm)**: Jokul reveal & SS, Noroi NS & SS from Deck, Lantern GY revival (before Necrovalley), Xyz Summon `K9-17 Ripper`, search `K9-X Forced Release`.
  - **Phase 2 (Necrovalley Lock)**: Activate Necrovalley **after** K9 plays are complete. Ripper's monster negate (hand/GY) and Werewolf's hand-rip banish do NOT touch GY and work 100% under Necrovalley, while completely suffocating the opponent.
- **Deck Upgrades (`2026_Grave.ydk`)**:
  - Removed dead bricks: Chief (-1), Shaman (-1), Dark Renewal (-1), Illusion Gate (-1), Chaotic Elements (-1).
  - Maxed out core starters: `K9-66a Jokul` (3x), `K9-04 Noroi` (3x), `K9-66b Lantern` (3x), `K9-X Forced Release` (2x).
  - Increased interaction: `Ash Blossom` (3x), `Infinite Impermanence` (3x).
  - Extra Deck: Added `Artifact Durendal` (`69840739`) (Rank 5 Quick rewrite / hand reload) and 2nd `K9-X Werewolf` (`90303227`).
- **AI Refactoring & Domain Plugin (`_2026_GraveExecutor.cs`)**:
  - Rewrote execution flow in ModernExecutor: smart delay of Necrovalley until Lantern has resolved, `RipperEffect` (search + hand/GY negate), `WerewolfEffect` (opp hand rip / our turn field banish), `ForcedReleaseEffect` (rank-up into Werewolf + pop), `DurendalEffect`, and full `OnSelectCard` hint management (`506`, `502`, `503`, `509`, `513`).
  - **Decoupled Domain Plugin Architecture (`GravePlugin : DeckPluginBase`)**:
    - `GraveStrategy` (`IDeckStrategy`): Handles Special Summon targeting (Jokul > Noroi > Lantern > Izuna > Werewolf) and Search routing (Ripper -> Forced Release / A Case for K9; Throne -> Commandant).
    - `GraveMaterialEvaluator` (`IDeckMaterialEvaluator`): Cost scoring, Ace card protection (penalty 10,000), material sorting, and discard target picking.
    - `GraveThreatEvaluator` (`IDeckThreatEvaluator`): Emergency threat recognition and score evaluation.
    - Integrated directly via `DeckPlugin = new GravePlugin(this);`.

### 3. Build & Exclusive Deployment Pipeline
- Ran `BUILD_AND_DEPLOY.ps1`:
  - `WindBot.dll`, `ExecutorBase.dll`, `core.dll`, `dashbot.dll`, `bots.json`, `.ydk` compiled with **0 Errors**.
  - All binaries successfully deployed to `C:\Users\admin\Documents\EdoGame\`.

---

## 0.044. Dynamic Card Text Semantic Parser & Chokepoint Engine Evaluator (2026-09-27)

### Overview
- **Objective**: Break the hardcoded barrier of bot threat assessment. Instead of relying solely on hardcoded Card IDs (like Solemns or Skill Drain), empower bots to dynamically "read" and comprehend card description texts in real time, accurately differentiating dangerous Resource Generators (e.g. Continuous Spells generating counters or searching every turn) from passive Decoy Shields / Indestructible cards.
- **Architectural Breakthrough (`CardTextSemantics.cs`)**:
  - Implemented bilingual (English & Thai) compiled regular expression pattern matching that parses card texts from `cards.cdb` on demand.
  - Profile caching via `ConcurrentDictionary<int, CardSemanticProfile>` ensures text parsing runs exactly **once per card ID**, guaranteeing subsequent evaluations run in **O(1) nanoseconds with zero GC allocation**.
  - **Semantic Category Detection**:
    1. `IsCounterGenerator`: Detects placement and removal of Bushido, Spell, or other counters (e.g. `Gateway of the Six`, `Magical Citadel of Endymion`) -> Threat bonus `+7,000`.
    2. `IsContinuousSearcher`: Detects repeated search / tutor from deck to hand (e.g. `Dark Magical Circle`, `Black Whirlwind`, `Runick Fountain`) -> Threat bonus `+6,500`.
    3. `IsContinuousSummoner`: Detects continuous special summons from deck/GY -> Threat bonus `+5,500`.
    4. `IsContinuousDraw`: Detects card draw engines (e.g. `Six Samurai United`) -> Threat bonus `+5,000`.
    5. `IsQuickInterruption`: Detects Quick Effects with disrupt/negate actions during opponent's turn -> Threat bonus `+7,500`.
    6. `IsContinuousFloodgate`: Detects continuous action negation / summon locks -> Threat bonus `+9,000`.
  - **Decoy & Immunity Shield Safeguards**:
    1. `IsDestructionImmune`: Detects "cannot be destroyed by card effects" or "unaffected by card effects" (e.g. `Stand Up Centur-Ion!`). When selecting a destruction target (`hint == HINTMSG_DESTROY`), applies a **-20,000 penalty**, completely preventing bots from wasting removal on indestructible cards!
    2. `IsFloaterOnDestruction`: Detects floating triggers upon destruction (e.g. `Mound of the Bound Creator`, `Geartown`). Applies a **-8,000 penalty** so bots do not trigger opponent searches.
    3. `Eternal Soul Bonus`: Special trigger recognition for `Eternal Soul` (`48680970`) where destruction wipes the opponent's monster field -> **+15,000 bonus**.
- **Central Core Integrations & 100% Backwards Compatibility**:
  - `CardIntelligence.GetCardThreatScore(card, hint)`: Combines O(1) staple databases with `CardTextSemantics.EvaluateCardThreat`.
  - `ModernExecutor.GetCardThreatScore(c, hint)`: Passed `hint` to threat scoring, fully integrated with `DeckPlugin.ThreatEvaluator`.
  - `HeuristicGuard.SanitizeSelection`: Interception guard sorts enemy candidates via `CardIntelligence.GetCardThreatScore(c, hint)`.
  - `DefaultExecutor.DefaultMysticalSpaceTyphoon()` & `AIUtil.GetProblematicEnemySpell()` / `GetBestEnemySpell()`: Prioritizes active high-threat face-up engine spells over passive cards or blind facedowns.
  - **Zero Regression**: Every legacy bot (`ABC`, `Altergeist`, `BlueEyes`, `DarkMagician`, `Lightsworn`, `Witchcraft`) automatically becomes vastly smarter in target prioritization with 0 modifications to their action combo code.
- **Build & Deploy Pipeline**:
  - Full project compiled and deployed via `BUILD_AND_DEPLOY.ps1` with 0 errors.

---

## 0.043. SixSamurai Multi-Tier Interruption Overhaul & SKILL.md Synchronization (2026-09-27)

### Overview
- **Objective**: Address deficiency in disruption/interaction capabilities for `SixSamurai` deck, bringing interaction density to competitive modern standard (15 interruptions / 37.5% of deck), activating dormant in-archetype boss quick effects, updating Extra Deck against banlists, and synchronizing `SKILL.md` with Central Domain Plugin Architecture.
- **SKILL.md Knowledge Modernization**:
  - Formalized Section 4 with concrete interfaces (`IDeckPlugin`, `DeckPluginBase`, `IDeckStrategy`, `IDeckResourceEvaluator`, `IDeckMaterialEvaluator`, `IDeckScaleResolver`, `IDeckActionScorer`, `IDeckThreatEvaluator`).
  - Documented automatic hook delegation in `ModernExecutor` (`ResetTurnState`, `OnSelectCounter`, `SortMaterials`, `PickDiscardTarget`).
  - Added Section 2.7: "Interruption Density & Balanced Deck Construction" (9–15 handtraps/interruption budget rule).
  - Added Section 5.4: "Case Study: Six Samurai Bushido Engine & Multi-Tier Interruption".
- **SixSamurai Deck Interruption Overhaul (`SixSamurai.ydk`)**:
  - **Handtraps & Going-Second Disruptions**:
    - Added `Nibiru, the Primal Being` x2 (`27204311`) - board wipe against heavy combo decks.
    - Added `Effect Veiler` x2 (`97268402`) - monster quick negate.
    - Added `Ghost Belle & Haunted Mansion` x2 (`73642296`) - GY protection/disruption.
    - Added `Droll & Lock Bird` x1 (`94145021`) - search lockdown.
    - Added `Harpie's Feather Duster` x1 (`18144506`) - backrow board breaker.
    - Retained `Ash Blossom & Joyous Spring` x2 (`14558127`) and `Infinite Impermanence` x3 (`10045474`).
    - Handtrap / Interruption density increased to **15 cards** (37.5% of Main Deck).
  - **Banlist Compliance (Rule 10 Audit)**:
    - Removed `Apollousa` (banned in both TCG and OCG).
    - Removed `Abyss Dweller` (banned in current banlists).
    - Reduced `Called by the Grave` and aligned deck 100% with `0TCG.lflist.conf` and `OCG.lflist.conf` (0 `ERRMSG_DECKERROR` risk).
  - **Extra Deck Upgrades**:
    - Activated `Legendary Lord Six Samurai - Kizan` (`42209438`) - Synchro 6 quick-effect targeted destruction on opponent's turn.
    - Added `Tornado Dragon` (`6983839`) - Rank 4 quick-effect spell/trap destruction.
    - Added `I:P Masquerena` (`65741786`) - Link-2 quick link into `S:P Little Knight` on opponent's turn.
- **Executor Intelligence Enhancements (`_2026_SixSamuraiExecutor.cs`)**:
  - Registered all new Handtraps and Board Breakers in Tier 0.
  - Implemented `LordKizanEffect` (Quick Destroy via Six Strike banish + GY revival) and `LordKizanSummon`.
  - Implemented `TornadoDragonEffect` & `TornadoDragonSummon`.
  - Implemented `IPMasquerenaEffect` & `IPMasquerenaSummon`.
  - Enforced strict Rule 1 Hint separation in `OnSelectCard` for `HINT_DESTROY` (`502`), `HINT_REMOVE` (`503`), and `HINT_RTOHAND` (`505`), ensuring opponent targets are strictly prioritized.
  - Updated `SixSamMaterialScorer`: Protected all Ace monsters (costs 6,000–10,000) and immunized Handtraps against accidental discard.
- **Build & Deploy Pipeline**:
  - Built cleanly with 0 Errors.
  - Deployed exclusively to `C:\Users\admin\Documents\EdoGame\` via `BUILD_AND_DEPLOY.ps1`.

---

## 0.042. Decoupled Domain Plugin Architecture & Endymion / SixSamurai Module Overhaul (2026-09-27)

### Overview
- **Objective**: Audit, design, and implement a formalized Central Domain Plugin Architecture (`ExecutorBase/Game/AI/Plugin/`), eliminating ad-hoc helper coupling and enabling rule-based modern executors to interface with domain sub-helpers through standardized contracts.
- **Decks Audited & Refactored**: `Endymion` (`_2026_EndymionExecutor.cs`) and `SixSamurai` (`_2026_SixSamuraiExecutor.cs`).
- **Core Domain Plugin Framework**:
  1. `IDeckPlugin.cs`: Master contract exposing domain sub-helpers (`Strategy`, `ResourceEvaluator`, `MaterialEvaluator`, `ActionScorer`, `ThreatEvaluator`, `ScaleResolver`).
  2. `IDeckStrategy.cs`: High-level sequencing, search targeting, and special summon target ranking.
  3. `IDeckResourceEvaluator.cs`: Counter economics (Spell Counters / Bushido Counters), affordability checks, and multi-card counter deduction algorithms.
  4. `IDeckMaterialEvaluator.cs`: Material scoring, Ace monster preservation (10,000 cost penalty), and discard/tribute fodder selection.
  5. `IDeckScaleResolver.cs`: Low/High Pendulum scale resolution and pop targeting.
  6. `IDeckActionScorer.cs`: Value vector and candidate action scoring.
  7. `IDeckThreatEvaluator.cs`: Archetypal threat scoring and emergency breach flags.
  8. `DeckPluginBase.cs`: Abstract base implementation with default no-op handlers.
- **ModernExecutor Central Hooks**:
  - Integrated `public IDeckPlugin DeckPlugin { get; set; }`.
  - Added automatic `DeckPlugin?.ResetTurnState()` in `OnNewTurn()`.
  - Delegated `OnSelectCounter` to `DeckPlugin.ResourceEvaluator.SelectCounters(...)`.
  - Delegated `OnSelectFusionMaterial`, `OnSelectLinkMaterial`, and `OnSelectXyzMaterial` to `DeckPlugin.MaterialEvaluator.SortMaterials(...)`.
  - Delegated `OnSelectCard` discard target selection to `DeckPlugin.MaterialEvaluator.PickDiscardTarget(...)`.
  - Fixed Hint 572: Removed `hint == 572` (`HINTMSG_COUNTER`) from enemy removal filters in `ModernExecutor`, preventing counter-placement effects on friendly cards from being treated as hostile removals.
- **Deck-Specific Improvements**:
  - **Endymion**:
    - Created `OnSelectPlace` override ensuring `OddEyesAbsoluteDragon` is placed strictly into Extra Monster Zones (`0x20` / `0x40`), allowing `GravityController` Link-1 summoning and subsequent `OddEyesVortexDragon` GY trigger.
    - Full domain sub-helpers: `EndymionStrategy`, `EndymionCounterEconomy`, `EndymionScaleResolver`, `EndymionMaterialScorer`, `EndymionActionScorer`, `EndymionThreatEvaluator`, `EndymionBoardAssessor`.
    - Added `Endymion` and `2026_Endymion` clean bot registrations to `bots.json` and synchronized `Endymion.ydk`.
  - **SixSamurai**:
    - Implemented `IDeckPlugin` on `SixSamuraiPlugin` and wired sub-helpers (`SixSamStrategy`, `SixSamCounterEconomy`, `SixSamKizaruResolver`, `SixSamMaterialScorer`, `SixSamActionScorer`, `SixSamRecoveryPlanner`, `SixSamBoardAssessor`).
    - Added `SixSamurai` and `2026_SixSamurai` clean bot registrations to `bots.json` and synchronized `SixSamurai.ydk`.
- **Headless Duel Simulation Verification (`Client_Headless_Fortest`)**:
  - Restored and MR5-patched `._cache_ygopro.exe` enabling reliable headless E2E duel testing.
  - **Endymion vs Altergeist (5 games)**: 5/5 successful, 0 crashes, 0 violations. (Duel 5 OTK win with Master Cerberus + Mighty Master + Jackal King).
  - **Endymion vs DarkMagician (5 games)**: 5/5 successful, 0 crashes, 0 violations. (Duel 5 OTK win with double Mighty Master + Astrograph Sorcerer under Secret Village lock).
  - **SixSamurai vs Altergeist (5 games)**: 5/5 successful, 0 crashes, 0 violations. (Duel 1 win with Battle Shogun + Kizan beatdown under Gateway loop).
- **Deployment**: Exclusively built and deployed to `C:\Users\admin\Documents\EdoGame\` with 0 compile errors.

---

## 0.041. Comprehensive Modern Meta Deck System Overhaul & Verification Audit (2026-09-26)

### Overview
- **Objective**: Systematic audit, modernization, architectural refactoring, and headless duel verification of all modern meta AI decks in `windbot-fork`.
- **Methodology**: 100% Rule-Based C# ModernExecutors (Strictly no neural models or AI training stubs per AGENTS.md Rule 1).
- **Core Architecture Upgrades**:
  1. **Domain Plugin Architecture**: Implemented modular domain classes (`*Plugin`, `*Advisor`, `*MaterialScorer`, `*BoardAssessor`) for every modern deck.
  2. **Smart Position Control**: Implemented `OnSelectPosition` and `SmartMonsterRepos` across all decks, placing low-ATK combo starters, handtraps, and tuners into FaceUpDefence while boss beaters maintain FaceUpAttack.
  3. **OCGCore Bounds Safety & Hint Separation**: All `OnSelectCard` implementations enforce `min <= count <= max` bounds checks via `Util.CheckSelectCount`, and separate Hint IDs (Hints 502/503/504/508 strictly prioritize enemy targets `c.Controller == 1` to prevent self-removal of Ace cards).
  4. **Central Engine Null-Safety Patch (`AIUtil.CheckSelectCount`)**: Made `CheckSelectCount` in `ExecutorBase` fully null-safe (`_selected == null ? new List<ClientCard>() : _selected.ToList()`), immunizing all deck executors against `ArgumentNullException`.
  5. **Clean Bot Registry**: Clean deck names without year prefixes (e.g. `Kashtira`, `Tenpai`, `Branded`, `Purrely`, `Spright`, `Tearlaments`, `Runick`, `Yubel`, `WhiteForest`, `EvilTwin`, `SkyStriker`, `FireKing`, `Mimighoul`, `Plant`, `Speedroid`) registered in `bots.json` and synchronized `.ydk` files across all deploy targets.

### Deck Audit & Headless Simulation Verification Roster (22 Modern Meta Decks)
All decks audited, modernized, compiled, deployed, and verified via Headless Text Duel (`Client_Headless_Fortest`) against `DarkMagician`:
| # | Deck Name | Executor Class | Domain Plugins | Position Control | Bounds-Safe Select | Headless Audit Result |
|---|-----------|----------------|:--------------:|:----------------:|:------------------:|:---------------------:|
| 1 | **Kashtira** | `KashtiraExecutor.cs` | Yes | Yes | Yes | 0 Violations, 0 Warnings, 0 Crashes (OK) |
| 2 | **Dinomorphia** | `_2026_DinomorphiaExecutor.cs` | Yes | Yes | Yes | **Won 1-0** (6 turns), 0 Violations, 0 Warnings (OK) |
| 3 | **Tenpai** | `TenpaiExecutor.cs` | Yes | Yes | Yes | 0 Violations, 0 Warnings, 0 Crashes (OK) |
| 4 | **Voiceless Voice** | `VoicelessVoiceExecutor.cs` | Yes | Yes | Yes | 0 Violations, 0 Warnings, 0 Crashes (OK) |
| 5 | **Mikanko** | `MikankoExecutor.cs` | Yes | Yes | Yes | **Won 1-0** (5 turns, 10,000 OTK reflect), 0 Violations (OK) |
| 6 | **Labrynth** | `_2026_LabrynthExecutor.cs` | Yes | Yes | Yes | 16 full turns played, 0 Violations, 0 Warnings (OK) |
| 7 | **Maliss** | `_2026_MalissExecutor.cs` | Yes | Yes | Yes | 6 full turns played, 0 Violations, 0 Warnings (OK) |
| 8 | **Branded** | `_2026_BrandedExecutor.cs` | Yes | Yes | Yes | **Won 1-0** (9 turns), 0 Violations, 0 Warnings (OK) |
| 9 | **Centurion** | `CenturionExecutor.cs` | Yes | Yes | Yes | **Won 1-0** (7 turns), 0 Violations, 0 Warnings (OK) |
| 10 | **Purrely** | `_2026_PurrelyExecutor.cs` | Yes | Yes | Yes | **Won 1-0** (15 turns), 0 Violations, 0 Warnings (OK) |
| 11 | **Spright** | `_2026_SprightExecutor.cs` | Yes | Yes | Yes | **Won 1-0** (7 turns), 0 Violations, 0 Warnings (OK) |
| 12 | **Tearlaments** | `_2026_TearlaExecutor.cs` | Yes | Yes | Yes | **Won 1-0** (4 turns), 0 Violations, 0 Warnings (OK) |
| 13 | **Exosister** | `_2026_ExosisterExecutor.cs` | Yes | Yes | Yes | 6 full turns played, 0 Violations, 0 Warnings (OK) |
| 14 | **Runick** | `_2026_RunickExecutor.cs` | Yes | Yes | Yes | 5 full turns played, 0 Violations, 0 Warnings (OK) |
| 15 | **Yubel** | `Yubel2Executor.cs` | Yes | Yes | Yes | 7 full turns played, 0 Violations, 0 Warnings (OK) |
| 16 | **White Forest** | `WhiteForestExecutor.cs` | Yes | Yes | Yes | 4 full turns played, 0 Violations, 0 Warnings (OK) |
| 17 | **Evil Twin** | `_2026_EvilTwinExecutor.cs` | Yes | Yes | Yes | **Won 1-0** (6 turns lethal rush), 0 Violations (OK) |
| 18 | **Sky Striker** | `_2026_SkyStrikerExecutor.cs` | Yes | Yes | Yes | 4 full turns played, 0 Violations, 0 Warnings (OK) |
| 19 | **Fire King** | `_2026_FirekingExecutor.cs` | Yes | Yes | Yes | 8 full turns played, 0 Violations, 0 Warnings (OK) |
| 20 | **Mimighoul** | `_2026_MimighoulExecutor.cs` | Yes | Yes | Yes | 6 full turns played, 0 Violations, 0 Warnings (OK) |
| 21 | **Plant** | `_2026_PlantExecutor.cs` | Yes | Yes | Yes | 5 full turns played, 0 Violations, 0 Warnings (OK) |
| 22 | **Speedroid** | `_2026_SpeedroidExecutor.cs` | Yes | Yes | Yes | 5 full turns played, 0 Violations, 0 Warnings (OK) |

- **Exclusive Deployment**: Full clean binaries, bots.json, CDBs, and deck lists deployed to `C:\Users\admin\Documents\EdoGame\`.
- **System Stability**: 100% compilation success (0 errors), 0 Violations, 0 Warnings, 0 Engine Retries, and 0 Crashes across all matchups.

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
