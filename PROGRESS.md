# Progress Log: AFS, ArtMage, Tenpai, Centurion, VoicelessVoice & ModernExecutors


## 0.030. Strategic Modernization & Full Refactoring: 2026_Dreadnought (Destiny HERO Dreadnought Engine) (2026-09-20)
- **User Directives**:
  - `_2026_DreadnoughtExecutor.cs refactor + rework`
- **Audit & Identified Issues**:
  1. **Card Identity & Constant Corrections (CRITICAL)**:
     - Card `46759931` was incorrectly labeled as `StarvingVenomFusionDragon`. Corrected to `VisionHEROTrinity` (5000 ATK, 3 attacks on monsters for lethal board breaking).
     - Card `78114463` was labeled as `SolemnAccusation`. Corrected to `SolemnReport`.
  2. **True Engine Synergy: Clock Tower -> Dreadnought Servant -> Dreadmaster -> Dreadnought (+2 Search)**:
     - The previous executor mistakenly assumed `Destiny HERO - Dreadnought` dumped 2 cards from deck to GY. Implemented its true effect: searching TWO Destiny HERO cards to hand upon Special Summon while scaling ATK to all other D-HEROs on field and in GY.
     - Implemented the core engine loop: Activate `Clock Tower Prison City - Dark City` -> search `Dreadnought Servant` -> SS Servant -> Servant pops Dark City to search `Polymerization` -> Dark City triggers to SS `Dreadmaster` from Deck -> Dreadmaster revives D-HEROs from GY and grants complete battle/destruction immunity -> Dreadnought Servant in GY triggers to spin 1 opponent card to top of deck -> Tribute Dreadmaster to SS `Destiny HERO - Dreadnought` -> Dreadnought searches 2 cards (`Fusion Destiny` + `Plasma` / `D-Force` / `Death Dogma`).
  3. **Death Dogma Quick Effect Fusion & Banish Revival**:
     - Implemented `Destiny HERO - Death Dogma` Special Summon from Hand or GY by banishing 3 Warrior/DARK monsters (protecting Malicious/Denier).
     - Implemented Quick Effect during opponent's turn: on opponent effect activation, Fusion Summons `DPE`, `Dystopia`, `Dominance`, or `Dangerous` by shuffling materials from GY/hand/field into Deck.
  4. **Option Bitshift Bug in `OnSelectOption` (CRITICAL)**:
     - Old code compared raw option numbers without decoding OCGCore's 64-bit encoded `options[i] >> 4` and `options[i] & 0xf`, breaking `Solemn Report` choice selection, and fell back to `return 0;` rather than calling `base.OnSelectOption(options)`.
     - Standardized to OCGCore bitshift: Option 1 (pay 3000 to banish all copies from hand & deck) when LP >= 4000 and target is key chokepoint / negator; Option 0 (pay 1500) otherwise.
  5. **Anti-Pattern 1 Violation Fix in `OnSelectCard`**:
     - `hint == 506` (Search): Dedicated search priority for Dreadnought, Dark City, Cross Crusader, Vyon, Shadow Mist, Sabatiel, and ROTA.
     - `hint == 501` (Discard): GY triggers (`Malicious`, `Shadow Mist`, `Denier`) prioritized; single-copy bosses and starters protected.
     - `hint == 502` (Destroy): Self-pop costs prioritize `Clock Tower` (triggers Dreadmaster SS!) and `DPE` (self-reviving); enemy pops strictly enforce `c.Controller == 1 && IsViableEffectTarget(c)`.
     - `hint == 503 / 504` (Banish): Enemy banish for Doom Liege, Dreadnought Servant, and Dominus Spark strictly targets opponent cards; friendly banish costs protect core extenders.
     - `hint == 505` (Recycle): Denier selects `Malicious` to reset the loop.
     - `hint == 508` (Send to GY): Vyon / Doom Liege / Foolish Burial prioritize `Malicious` (if 0 in GY), `Shadow Mist`, `Denier`, `Death Dogma`.
     - `hint == 509` (Special Summon): Clock Tower destruction strictly selects `Dreadmaster`; DPE Standby revival strictly selects `DPE`.
     - `hint == 500 / 512 / 513` (Materials/Release): Strictly protects Ace bosses (`DPE`, `Plasma`, `Dreadnought`, `Dark Law`, `Trinity`).
  6. **Anti-Pattern 5 Violation Fix (Handtraps Set in MP1)**:
     - Purged `MonsterSet` on `AshBlossom` that set Ash face-down. Handtraps (`Ash`, `Fuwalos`, `Impermanence`) are strictly preserved in hand.
  7. **Stat-Aware `OnSelectPosition` & Column-Safe `OnSelectPlace`**:
     - Enforced `FaceUpAttack` for boss beaters (`Dreadnought`, `DPE`, `Trinity`, `Plasma`, `Death Dogma`, `Dogma`, `Dystopia`, `Dusktopia`, `Dominance`, `Contrast HERO Chaos`, `Dread Decimator`).
     - Enforced `FaceUpDefence` for low ATK / utility monsters (`Dreadnought Servant`, `Doom Liege`, `Denier`, `Vyon`, `Shadow Mist`, `Ash Blossom`, `Fuwalos`, `Cross Crusader`, `Wonder Driver`).
     - Column-safe placement avoids columns with active enemy Spell/Trap threats.
  8. **Lethal & Rush Mode Guard (`HasLethalOnBoard`)**:
     - Implemented `HasLethalOnBoard()` calculation across summon and combo methods to prevent overextension when confirmed lethal damage is on board.
- **Build & Exclusive Deployment**:
  - Successfully compiled via `BUILD_AND_DEPLOY.ps1` with 0 errors and deployed exclusively to `C:\Users\admin\Documents\EdoGame\`.
  - Verified runtime instantiation without error via `dotnet WindBot.dll Deck=2026_Dreadnought`.


## 0.029. Strategic Modernization & Full Refactoring: 2026_Doomz (DoomZ WIND Machine Xyz & Power Patron Engine) (2026-09-20)
- **User Directives**:
  - `_2026_DoomzExecutor.cs Refactor`
- **Audit & Identified Issues**:
  1. **Card ID Typo Fix (CRITICAL)**:
     - `AshBlossom` was defined as `14558128` (invalid ID). Corrected to `14558127` matching `cards.cdb`.
  2. **Dead Code & Phantom Card Purge**:
     - Removed phantom card constants not present in `2026_Doomz.ydk`: `ClockworkKnight`, `BystialMagnamhut`, `GhostBelle`, `DrollAndLockBird`.
     - Purged obsolete `NeuralDoomzExecutor` class adhering to Rule 1 (100% Rule-based C# ModernExecutor).
  3. **Option Bitshift Bug in `OnSelectOption` (CRITICAL)**:
     - `OnSelectOption` previously used `options.Contains(1)` on 64-bit encoded numbers, causing option selection to always fail.
     - Standardized to OCGCore bitshift: `long cardId = options[i] >> 4; long optIndex = options[i] & 0xf;` with `base.OnSelectOption(options)` fallback.
     - `Medius the Pure`: Selects Option 1 (Special Summon) when field space exists (`Bot.GetMonsterCount() < 5`), Option 0 (Search) otherwise.
     - `Null Power Patron Realm - Vidria`: Selects Option 1 (SS Zegredo) when space exists, Option 0 otherwise.
     - `DoomZ Raiders`: Selects Option 1 (SS) when space exists, Option 0 otherwise.
     - `The Fallen & The Virtuous`: Selects Option 0 (Destroy enemy face-up) if enemy face-up exists, Option 1 (Revive) otherwise.
  4. **Stat-Aware `OnSelectPosition`**:
     - Enforced `FaceUpDefence` for low ATK and utility monsters (`PowerPatronShadowMachineZegredo` 300 ATK, `PowerPatronDoomZ` 300 ATK, `AshBlossom` 0 ATK, `MulcharmyFuwalos` 100 ATK, `SpringansMerrymaker` 1100 ATK).
     - Enforced `FaceUpAttack` for boss beaters (`Jupiter`, `Drastrius`, `Varudras`, `Diactorus`, `Graflario`, `Sargas`, `Vidrium`).
  5. **Comprehensive Hint-Specific Segregation in `OnSelectCard`**:
     - `hint == 506` (Search): Priority for `Medius`, `Zegredo`, `Amalthe`, `Elara`, `Vidria`, `DoomZ Change`.
     - `hint == 501` (Discard): GY triggers (`Adrasteia`, `DoomZ Change`, `Vidrium`) prioritized; starters and bosses protected.
     - `hint == 502` (Destroy): Self-pop costs destroy `Elara`/`Amalthe` (trigger destruction effects) or `DoomZ Change`; enemy pops strictly enforce `c.Controller == 1`.
     - `hint == 503` (Banish): Enemy banish for `Vidrium`; safe hand banish for `Vidria` cost.
     - `hint == 509` (Special Summon): Prioritizes Deck over Hand, prioritizes Ace Bosses (`Jupiter`, `Drastrius`, `Varudras`) and key extenders.
     - `hint == 500 / 512 / 513`: Strictly protects Ace bosses from tributes/materials.
  6. **Lethal & Rush Mode Guard (`HasLethalOnBoard`)**:
     - Added `HasLethalOnBoard()` calculation across summon and combo methods to prevent overextension when confirmed lethal damage is on board.
- **Build & Exclusive Deployment**:
  - Successfully compiled via `BUILD_AND_DEPLOY.ps1` with 0 errors and deployed exclusively to `C:\Users\admin\Documents\EdoGame\`.
  - Verified runtime instantiation without error via `dotnet WindBot.dll Deck=2026_Doomz`.


## 0.028. Strategic Modernization & Full Refactoring: 2026_BrElfnote (Elfnote Branded Tuning) (2026-09-20)
- **User Directives**:
  - `_2026_BrElfnoteExecutor.cs Refactor`
- **Audit & Identified Issues**:
  1. **Option Bitshift Bug in `OnSelectOption` (CRITICAL)**:
     - `OnSelectOption` previously had `options[i] >> 20`, evaluating card ID to `0` and completely breaking option routing for `Medius the Pure` and `The Fallen & The Virtuous`. In addition, a hardcoded `return 0;` at the end prevented `ModernExecutor` base option logic from running.
     - Standardized to OCGCore bitshift: `long cardId = options[i] >> 4; long optIndex = options[i] & 0xf;` with `base.OnSelectOption(options)` fallback.
     - `Medius the Pure`: Intelligently selects Option 1 (Special Summon) if field has space (`Bot.GetMonsterCount() < 5`) and Option 0 (Add to Hand) otherwise.
     - `The Fallen & The Virtuous`: Selects Option 0 (Destroy 1 face-up card) if enemy has face-up cards and Extra Deck has Albaz dump targets, or Option 1 (Revive from GY) if Ecclesia is present.
  2. **Severe Constructor Duplication & Dead Code Purge**:
     - Eliminated an ~80-line duplicate `AddExecutor` registration block that bloated the file to 2,365 lines.
     - Removed phantom card constant `InfernalStrikeFighter = 66122213` (not in `2026_BrElfnote.ydk` Extra Deck).
  3. **Critical Inverted Boss Scoring Bug in `OnSelectCard` (Hint 509)**:
     - In `hint == 509` (Special Summon / Revival), `int bossScore = IsAceCard(c) ? 100 : 0;` in an ascending `OrderBy` previously sorted Ace Bosses to the back of the revival priority queue.
     - Fixed with descending threat/boss priority: Ace Bosses (`Baronne`, `Mirrorjade`, `PEP`, `Junora`, `Luluwalilith`, `Strelitzia`) and key extenders (`Regina`, `Power Patron`, `Medius`, `Ecclesia`) now take highest priority.
  4. **Self-Negation Bug in `OnSelectCard` (Hint 575)**:
     - In `hint == 575` (`HINTMSG_NEGATE`), if the opponent had no face-up negatable cards, the bot previously targeted and negated its own monsters (`ElfnotePowerPatron`, `MediusThePure`).
     - Fixed: strictly restricts targeting to opponent cards (`c.Controller == 1`). If no enemy targets exist and effect is cancelable, safely cancels without self-negation.
  5. **Comprehensive Hint-Specific Segregation in `OnSelectCard`**:
     - `hint == 506` (`HINTMSG_ATOHAND` / Search): Added dedicated search priority for Elfnote monsters (`Regina` > `Power Patron` > `Welcome Home` > `Fallen & Virtuous` > `Medius` > `Lucina` > `Tinia`).
     - `hint == 501` (`HINTMSG_DISCARD`): Prioritizes GY fodder and triggers (`Junordo`, duplicate Spells/Traps) while strictly protecting single-copy starters and Ace monsters.
     - `hint == 504 / 508` (`HINTMSG_TOGRAVE`): Optimized `Fidraulis Harmonia` dumps (`Malong` to bounce opponent cards, `Luluwalilith` for EP Special Summon, or `DevoursDogma`) and Albaz dumps (`Albion`, `Dogma`, `Rindbrumm`, `Sprind`).
     - `hint == 502 / 503 / 505`: Enforces strict `c.Controller == 1` targeting for destruction, banishing, and bouncing.
  6. **Center Main Monster Zone (MMZ) Strategic Placement (`OnSelectPlace`)**:
     - Archetype center MMZ synergy: `Elfnote Regina` (requires center MMZ to trigger SS from Deck) and `Elfnote Seraphim Strelitzia` (original ATK becomes 3000 in center MMZ) are prioritized for Zone 2 (`1 << 2`).
     - Non-center zones (`available & ~(1 << 2)`) are assigned to utility monsters, normal summons, and handtraps to keep Zone 2 open.
  7. **Position Logic (`OnSelectPosition`) & Lethal Guard (`HasLethalOnBoard`)**:
     - Enforced `FaceUpDefence` for low ATK and utility tuners (`ElfnotePowerPatron`, `Junordo`, `Handtraps`, `Malong`).
     - Enforced `FaceUpAttack` for boss beaters (`Baronne`, `PEP`, `Mirrorjade`, `Junora`, `Luluwalilith`, `Strelitzia`, `DevoursDogma`).
     - Implemented `HasLethalOnBoard()` calculation across summon and combo methods to prevent overextension when confirmed lethal damage is on board.
  8. **Enhanced Card-Specific Handlers**:
     - `Junora the Power Patron of Tuning`: Added steal target selection (takes control of highest ATK enemy monster) and search.
     - `Despian Luluwalilith`: Added face-up negation targeting and End Phase revival of LIGHT Spellcaster (`Ecclesia` / `Medius`).
     - `The Dragon that Devours the Dogma`: Added GY disruption/recycle on summon and End Phase search for `The Fallen & The Virtuous`.
     - `Elfnote Power Patron`: Corrected Extra Deck Synchro target filtering to `Junora` and `Strelitzia`.
- **Build & Exclusive Deployment**:
  - Successfully compiled via `BUILD_AND_DEPLOY.ps1` with 0 errors and deployed exclusively to `C:\Users\admin\Documents\EdoGame\`.
  - Verified runtime instantiation without error via `dotnet WindBot.dll Deck=2026_BrElfnote`.


## 0.027. Strategic Overhaul & Modern Refactor: 2026_Branded (Branded Despia Dogmatika Bystial) (2026-09-20)
- **User Directives**:
  - `_2026_BrandedExecutor.cs Refactor`
- **Audit & Identified Issues**:
  1. **Option Bitshift Bug in `OnSelectOption` (CRITICAL)**:
     - `OnSelectOption` previously had `options[i] >> 20`, evaluating card ID to `0` and completely breaking option routing for `The Fallen & The Virtuous` and `Triple Tactics Talent`.
     - Standardized to OCGCore bitshift: `long cardId = options[i] >> 4; long optIndex = options[i] & 0xf;`.
  2. **Dead Code & Phantom Card Purge**:
     - Removed phantom card constants and dead executors for cards not present in `2026_Branded.ydk`: `BrandedSword` (81767888), `CalledByTheGrave` (24224830), `DespianQuaeritis` (72272462), `AlbaLenatusTheAbyssDragon` (3410461), `BorreloadFuriousDragon` (92892239).
     - Fixed misnamed Side Deck constants aligning with `2026_Branded.ydk` and `cards.cdb` (`KumongousTheStickyStringKaiju = 29726552`, `IllusionGate = 33017964`, `TripleTacticsThrust = 35269904`, `SolemnJudgment = 41420027`).
  3. **Branded Fusion Resource & Discard Safeguard**:
     - When hand count == 0 (Branded Fusion was the last card in hand), summoning `Lubellion the Searing Dragon` previously caused a bot stall because Lubellion requires discarding 1 card.
     - Implemented dynamic route: If hand count == 0, `BrandedFusionEffect` summons `Albion the Branded Dragon` (banishes from GY/field with 0 discard cost) into `Mirrorjade`; if hand count >= 1, summons `Lubellion` (shuffling materials back).
  4. **Strict Hint-Specific Segregation in `OnSelectCard` (Anti-Pattern 1 Violation Fix)**:
     - `hint == 506` (`HINTMSG_ATOHAND`): Dedicated Deck search priority (`BrandedFusion` > `TheFallenAndTheVirtuous` > `BrandedInHighSpirits` > `BrandedRetribution` > `WhiteDragon` > `Quem` > `Cartesia` > `Ecclesia` > `Mercourier` > `SpringansKitt` > `AlbionTheShrouded` > `Albaz`).
     - `hint == 506` under Lubellion: Dedicated shuffle selection of `FallenOfAlbaz` + `LubellionTheSearingDragon`.
     - `hint == 502 / 503 / 504 / 505 / 551 / 552 / 572 / 575`: Enforced strict `c.Controller == 1` opponent targeting to protect friendly Ace bosses from self-removal.
     - `hint == 501` (`HINTMSG_DISCARD`): Prioritizes GY fodder and triggers (`TheGoldenSwordsoul`, `BrandedRetribution`, `SpringansKitt`, `AlbionTheShroudedDragon`, `Mercourier`).
     - `hint == 508` (`HINTMSG_TOGRAVE`): Optimized Extra Deck and Main Deck dumps.
     - `hint == 509` (`HINTMSG_SPSUMMON`): Intelligently prioritizes bosses and key extenders from Deck and GY.
     - `hint == 500 / 511 / 512` (`RELEASE / FMATERIAL / SMATERIAL`): Strictly protects Ace bosses (`Mirrorjade`, `TheDragonThatDevoursTheDogma`, `DespianLuluwalilith`, `EcclesiaAndTheDarkDragon`).
  5. **Lethal & Rush Mode Control (`HasLethalOnBoard`)**:
     - Implemented `HasLethalOnBoard()` calculation. Starters, extenders, and normal summons halt unnecessary combos when confirmed lethal damage is on board to close out the game directly.
  6. **Stat-Aware `OnSelectPosition`**:
     - Enforced `FaceUpDefence` for low ATK and utility monsters (`MulcharmyFuwalos`, `AshBlossom`, `TriBrigadeMercourier`, `IncredibleEcclesia`, `GuidingQuem`, `BlazingCartesia`, `TriBrigadeSpringansKitt`).
     - Enforced `FaceUpAttack` for bosses (`Mirrorjade`, `TheDragonThatDevoursTheDogma`, `DespianLuluwalilith`, `PSYFramelordOmega`, `Titaniklad`, `EcclesiaAndTheDarkDragon`).
  7. **Expanded Engine Synergies & Float Recovery**:
     - `GuidingQuem` revival now includes `Mirrorjade` and `TheDragonThatDevoursTheDogma`.
     - `TheDragonThatDevoursTheDogma` on-summon shuffles enemy GY chokepoints or recycles own Fusions; End Phase search prioritized to `Mercourier`.
     - `DespianLuluwalilith` on-field +500 ATK buff & face-up negate; End Phase float into `Quem`, `Cartesia`, or `Ecclesia`.
     - `Granguignol` tag-out into `Despian Luluwalilith` on opponent monster effect SS.
     - `BrandedRetribution` Counter Trap activation verified for 2 GY Fusions or 1 non-boss field Fusion.
- **Build & Exclusive Deployment**:
  - Successfully compiled via `BUILD_AND_DEPLOY.ps1` with 0 errors and deployed exclusively to `C:\Users\admin\Documents\EdoGame\`.

## 0.026. Strategic Overhaul & Modern Rework: Anime_Crow (Crow Hogan - Blackwing Synchro & Burn Army) (2026-09-20)
- **User Directives**:
  - `Anime_CrowExecutor.cs ปรับปรุง`
- **Audit & Identified Issues**:
  1. **Game-Breaking Turn-1 Freeze Bug in `OnSelectYesNo`**:
     - `OnSelectYesNo` had `if (Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0) return false;`. On Turn 1 going first, the opponent controls 0 cards, causing the bot to answer "NO" to ALL optional effects (searches, summons, level changes, Zephyros bounce), completely breaking the bot and forcing a dead pass.
     - Fixed by returning `base.OnSelectYesNo(desc)` safely.
  2. **Illegal Deck Construction (`Shamal the Sandstorm` & Missing `Black Feather Whirlwind`)**:
     - The deck ran 3x `Shamal the Sandstorm` which discards to place `Black Feather Whirlwind` (7602800) from Deck. However, `Black Feather Whirlwind` was completely missing from the deck list, making Shamal's hand activation 100% illegal and unplayable under OCG/TCG rules.
     - Added 2x `Black Feather Whirlwind` (7602800) into `Anime_Crow.ydk`, enabling Shamal's hand placement and free Blackwing revival from GY on every DARK Synchro summon.
  3. **Banlist Limit Violation (TCG Limit 1)**:
     - `Called by the Grave` was at 2 copies, violating the TCG Limited list (max 1). Adjusted to 1 copy to strictly prevent `ERRMSG_DECKERROR` (Rule 10).
  4. **Missing Extra Deck Summon Registrations**:
     - `Black-Winged Dragon` (Crow's signature Signer Dragon ace) and `Chidori the Rain Sprinkling` were in the Extra Deck but were never registered for Special Summon in `RegisterExecutors`.
     - Fully registered both monsters along with comprehensive climbing conditions.
  5. **Vata Dragon Engine Integration**:
     - Added 2x `Blackwing - Vata the Emblem of Wandering` (71187462) (Crow's signature Darkwing Blast support) to send materials directly from Deck to summon `Black-Winged Dragon` and bridge into `Black-Winged Assault Dragon`.
  6. **Anti-Pattern 5 Violation (Handtraps Set in MP1)**:
     - `Infinite Impermanence` and `Blackbird Close` were being set in MP1 via `SpellSetStrategy`.
     - Fixed: Handtraps are strictly preserved in hand. `Blackbird Close` is retained in hand when controlling a Blackwing Synchro or BWD to function as an unrevealed Counter Trap.
  7. **Anti-Pattern 3 Violation (Boss Sacrificed for Blackbird Close)**:
     - `BlackbirdCloseActivate` had a fallback that could tribute `Full Armor Master` (3000 ATK tower).
     - Fixed: Ace bosses (`FullArmorMaster`, `AssaultDragon`, `Onimaru`, `HawkJoe`) are strictly immune from being sacrificed for Blackbird Close.
  8. **Comprehensive Hint-Specific Segregation in `OnSelectCard`**:
     - `hint == 500` (`RELEASE`): Protects Ace cards; prioritizes Tokens and low-ATK fodder.
     - `hint == 501` (`DISCARD`): Prioritizes Zephyros, Shamal, and duplicate cards.
     - `hint == 502 / 503` (`DESTROY / REMOVE`): Strictly enforces `c.Controller == 1` opponent targets sorted by threat score.
     - `hint == 504` (`FRIENDLY REMOVE`): For `Black-Winged Assault Dragon` contact banish, prioritizes Tuner Synchro (`Boreastorm`) and `Black-Winged Dragon` from GY first to preserve field cards.
     - `hint == 505` (`RTOHAND`): Zephyros bounces `Black Whirlwind` (re-activatable) or `Bora` (re-summonable); protects bosses.
     - `hint == 506` (`ATOHAND`): Sudri only searches cards mentioning BWD (`Shamal`, `Vata`, `Blackbird Close`, `Black Feather Whirlwind`).
     - `hint == 508` (`TOGRAVE`): Boreastorm checks deck for `Zephyros` first for free revival, then Level 4/2 extenders.
     - `hint == 509` (`SPSUMMON`): Prioritizes bosses and key extenders.
     - `hint == 512` (`SMATERIAL`): Excludes established boss monsters from being consumed.
   9. **OCGCore Engine Fix (`c73218989.lua` - Black-Winged Assault Dragon)**:
      - Fixed OCGCore Lua engine script error: `[string "c73218989.lua"]:86: Attempting to access deleted object.` during Contact Banish Special Summon.
      - In `sptg`, the card material group `g` created by `aux.SelectUnselectGroup` lacked `g:KeepAlive()`, causing OCGCore to collect the group before `spop` executed `Duel.Remove(g, POS_FACEUP, REASON_COST)`.
      - Added `g:KeepAlive()` in `sptg` and `g:DeleteGroup()` in `spop` across `script/c73218989.lua` and `repositories/official-scripts/official/c73218989.lua`.
- **Build & Exclusive Deployment**:
  - Successfully compiled with 0 errors via `BUILD_AND_DEPLOY.ps1` and deployed exclusively to `C:\Users\admin\Documents\EdoGame\`.
  - Synchronized `Anime_Crow.ydk` to both `windbot-fork/Decks/` and `deck/`.

## 0.025. Code Cleanup & Strategic Refactoring: 2026_Angelechy (2026-09-20)
- **User Directives**:
  - `_2026_AngelechyExecutor.cs` & `yugioh-executor/SKILL.md`
- **Audit & Identified Issues**:
  1. **Anti-Pattern 1 Violation in `OnSelectCard` (Self-Destruction / Self-Banish Bug)**:
     - In `OnSelectCard()`, `hint == 502` (`HINTMSG_DESTROY`) and `hint == 504` (`HINTMSG_REMOVE`) were mistakenly included in the discard selection block (`c.Controller == 0`), causing the bot to target and destroy or banish its own boss monsters instead of enemy targets.
     - Fixed with strict hint segregation: all removal and disruption hints (`502`, `503`, `504`, `505`, `551`, `552`, `572`) now strictly filter `c.Controller == 1` sorted by `GetCardThreatScore`.
  2. **Dominus Spark Hand Trap Safety Guard (Permanent DARK Lock)**:
     - Activating `Dominus Spark` from hand permanently locks out all DARK monster effects for the remainder of the duel. Since all 7 Labrynth monsters are DARK Fiends, hand activation crippled the entire Labrynth half of the deck.
     - Implemented safety guard: `Dominus Spark` is preserved in hand to be set on the field safely (via `Lady Labrynth`, `Arias`, or Normal Set) where it can be activated with zero restriction, unless facing an immediate lethal attack.
  3. **Furniture Dual-Trigger Recovery**:
     - Single boolean trackers (`_stovieUsed`, `_chandraglierUsed`) blocked the furniture from using their GY recursion triggers after being discarded from hand.
     - Separated trackers into `_stovieHandUsed` / `_stovieGyUsed` and `_chandraglierHandUsed` / `_chandraglierGyUsed`, restoring their automatic return/revival upon Trap removal.
  4. **Angelechy & Labrynth Synergy Loops**:
     - **S/T Placement Route**: Optimized Extra Deck placement so `Angelechy Bastion` triggers `Angelechy Shatranga` (locking the opponent to 5 monster activations and searching Traps), followed by `Angelechy Destrier` (500 burn + Spell search).
     - **Control Swap Loop**: `Angelechy Enlisted` banishes adjacent enemy monsters and swaps control; its trigger on control change returns it to Extra Deck to Special Summon an Angelechy boss (`Shatranga` or `Bastion`). Seamlessly pairs with `Angelechy Disturbance`.
     - **Big Welcome Bouncing**: On-field bounce prioritizes recycling `Cooclock`, `Arianna`, `Arias`, or furniture, strictly protecting Ace monsters. GY bounce requires a Level 8+ Fiend and targets opponent cards.
  5. **Extra Deck Summon Safeguards**:
     - `ChaosAngelSummon`: Validates exact Level 10 requirement (Lv8+Lv2 or Lv7+Lv3) and protects ace monsters from unnecessary sacrifice.
     - `AnimaSummon`: Verifies control of a Level 1 non-token monster (`Cooclock`, `Droll`) and opponent monster in front of EMZ.
  6. **Strategic Overrides Added**:
     - `BlackGoatEffect`: Intelligent card declaration via `AI.SelectAnnounceID` targeting opponent's high-threat boss or universal meta staples (`Ash Blossom`).
     - `OnSelectOption`: Directs `Lovely Labrynth` to deterministic field card destruction.
     - `OnSelectPosition`: Enforces `FaceUpDefence` for 0-ATK utility cards (`Cooclock`, `Stovie`, `Witness`, `Nullgainer`, `ArcToken`) and `FaceUpAttack` for bosses.
     - `OnSelectPlace`: Optimizes column alignment for `Bastion` and `Enlisted`.
     - `HasLethalOnBoard`: Prevents overextending into pointless combos when lethal damage is confirmed.
- **Build & Exclusive Deployment**:
  - Compiled with 0 errors via `BUILD_AND_DEPLOY.ps1` and deployed exclusively to `C:\Users\admin\Documents\EdoGame\`.

## 0.024. Code Cleanup & Strategic Refactoring: 2026_CrystronTrains (2026-09-20)
- **User Directives**:
  - `_2026_CrystronTrainsExecutor.cs Refactor คลีนโค๊ดและปรับปรุง`
- **Audit & Identified Issues**:
  1. **Option Bitshift Bug in `OnSelectOption`**:
     - The executor had `options[i] >> 20`, evaluating card ID to 0 and completely breaking option routing (e.g. `Triple Tactics Talent`). Fixed to standard OCGCore bitshift: `(options[i] >> 4)` and `(options[i] & 0xf)`.
  2. **Dead Code & Phantom Card Constants**:
     - Removed constants and methods for cards not present in `2026_CrystronTrains.ydk`: `TrainConnection` (60879050), `BarrageBlast` (51369889), `Imperm` (10045474).
  3. **Anti-Pattern 5 Violation**:
     - Removed dangerous `MonsterSet` logic for handtraps (`AshBlossom`, `DrollLockBird`, `MulcharmyFuwalos`). Handtraps are now preserved in hand.
  4. **Mojibake Comments**:
     - Replaced corrupted Thai comment text with clean, professional Thai/English documentation.
  5. **Lack of Hint-Specific Segregation in `OnSelectCard`**:
     - Upgraded `OnSelectCard` with strict hint isolation:
       - `hint == 506` (`HINTMSG_ATOHAND`): Prioritizes `Flying Launcher`, `Babel Decker`, `Bleu Traveler`, `Revolving Switchyard`.
       - `hint == 502 / 503 / 504` (`DESTROY` / `REMOVE` / `TOGRAVE`): Enforced `c.Controller == 1` opponent targeting to strictly prevent self-targeting or banishing own boss.
       - `hint == 501` (`HINTMSG_DISCARD`): Prioritizes GY fodder like `BulletTrain`, `Sulfator`, `Smiger`.
       - `hint == 508` (`HINTMSG_TOGRAVE` dump): Prioritizes `BulletTrain`, `Bleu Traveler` for GY effects (`Convex Knight` dump).
       - `hint == 509` (`HINTMSG_SPSUMMON`): Prioritizes `Gustav Rocket`, `Babel Decker`, `Bleu Traveler`, `Exceptional Schedule` targets.
       - `hint == 507` (`HINTMSG_EQUIP`): Prioritizes `Flying Launcher` equip to Rank 10 Machine Xyz.
  6. **Lethal & Ace Protection**:
     - Implemented `HasLethalOnBoard()` calculation and boss preservation before Extra Deck summons (preventing cannibalizing `AA-ZEUS`, `Liebe`, `Super Dora`).
- **Build & Exclusive Deployment**:
  - Compiled with 0 errors via `BUILD_AND_DEPLOY.ps1` and deployed exclusively to `C:\Users\admin\Documents\EdoGame\`.

## 0.023. Strategic Modernization: Anime_Yusei (Yusei Fudo Ultimate Synchro Battlebox) (2026-09-20)
- **User Directives**:
  - `Anime_YuseiExecutor.cs ปรับปรุง`
- **Audit & Identified Issues**:
  1. **Missing Card Registrations**:
     - `Wheel Synchron` (60283232): Missing `Summon`, on-field extra Normal Summon, and GY banish level reduction.
     - `Crossroad Sonic Chick` (80054655): Missing `Summon` and on-field level change to 3 or 4.
     - `Scrap Synchron` (16449363): Missing `Activate` for card destruction when sent as Synchro material.
     - `Junk Meister` (73218792): Missing `Activate` for draw 1 card when used as Synchro material.
     - `Assault Synchron` (77202120): Missing `Activate` in GY to banish itself and revive a tributed/banished Dragon Synchro.
     - `Shooting Riser Dragon` (68431965): Missing `SpSummon`, on-summon deck dump/level adjust, and opponent turn Quick Synchro.
     - `Shooting Star Dragon` (24696097): Missing `SpSummon`, 5-card excavation multi-attack, and destruction negate.
  2. **Anti-Pattern Violations & Overextension**:
     - All Extra Deck summons returned unconditional `return true;`, causing bot to cannibalize established bosses (`Cosmic Blazar`, `Shooting Quasar`, `Stardust Warrior`) in MP1.
  3. **Lack of Strategic Overrides (`OnSelectCard`, `OnSelectOption`, `OnSelectPosition`)**:
     - No hint separation: defaults to random selection on search, discards valuable starters instead of GY fodder, and lacked forced opponent target checks (`c.Controller == 1`) on removal.
     - Low ATK utility monsters (`Formula Synchron`, `Junk Meister`, `Sonic Chick`) were summoned in Attack mode.
  4. **Card-Specific Logic Bugs**:
     - `Synchro Fellowship`: Second search target included `Junk Meister` (which does not mention Junk Warrior or Stardust Dragon), risking activation failure.
     - `Crimson Dragon`: Tag-out did not verify that Extra Deck contains a Dragon Synchro matching the target's Level.
- **Architectural Solutions & Changes Implemented (`Anime_YuseiExecutor.cs`)**:
  1. **Full Card Registration**: Registered all 37 deck/extra deck cards and their primary/secondary effects.
  2. **Lethal & Boss Protection**: Implemented `HasLethalOnBoard()` and `IsMaterialBossProtected()` preventing overextension and sacrificing of ace monsters.
  3. **Structured Synchro Laddering**:
     - Level 5 Engine: `Junk Speeder` (requires 2+ free monster zones, summons distinct-level Synchron tuners).
     - Intermediate / Draw: `Formula Synchron` (L2 Tuner, draw 1), `Scrap Warrior` (L3), `Stardust Charge Warrior` (L6, draw 1), `Shooting Riser Dragon` (L7 Tuner, dump & level modulate).
     - Level 8 Pillars: `Accel Synchro Stardust Dragon` (revives L<=2 tuner, quick tribute for Stardust + unaffected Synchro), `Stardust Dragon` (destruction protection).
     - Level 10 Disruptions: `Satellite Warrior` (board breaker & GY float), `Stardust Warrior` (SS negate), `Shooting Star Dragon` (multi-attack).
     - Level 12 Ultimate Bosses: `Cosmic Blazar Dragon` (Omni-negate), `Shooting Quasar Dragon` (Omni-negate & multi-attack), `Crimson Dragon` (Tag-out into matching Dragon Synchro).
  4. **Intelligent Selection Overrides**:
     - `hint == 506` (`HINTMSG_ATOHAND`): Prioritizes `Synchro Fellowship`, `Tuning`, `Junk Synchron`, `Stardust Synchron`, `Junk Converter`.
     - `hint == 501` (`HINTMSG_DISCARD`): Prioritizes GY fodder (`Jet Synchron`, `Anchorbolt Hedgehog`, `Junk Converter`, `Wheel Synchron`, `Assault Synchron`) over starters/bosses.
     - `hint == 502 / 503 / 504` (`DESTROY` / `REMOVE`): Strictly restricts targeting to opponent cards (`c.Controller == 1`) sorted by `GetCardThreatScore`.
     - `hint == 509` (`HINTMSG_SPSUMMON`): Intelligently picks 1 Tuner per distinct level for `Junk Speeder` (L4, L3, L2, L1, L5).
     - `hint == 500 / 512` (`RELEASE` / `SMATERIAL`): Excludes established bosses from being tributed or consumed.
     - `OnSelectOption()`: Explicit option routing for `Junk Signal`, `Majestic Mirage`, and `Cosmic Blazar Dragon`.
     - `OnSelectPosition()`: Enforces `FaceUpDefence` for low ATK / utility monsters and `FaceUpAttack` for bosses.
- **Build & Exclusive Deployment**:
  - Successfully compiled with 0 errors via `BUILD_AND_DEPLOY.ps1` and deployed exclusively to `C:\Users\admin\Documents\EdoGame\`.

- **User Directives**:
  1. "ป้องกัน Dingirsu กลืนบอส WhiteForestExecutor.cs: ใน DingirsuSummon(): เช็ค !IsWhiteForestBoss(m) และห้ามสังเวย Diabell Queen / Diabellze เด็ดขาด; ใน OnSelectCard(): เพิ่มเคส hint == 513 (HINTMSG_XMATERIAL) เพื่อคัดกรอง Boss ออก"
  2. "แก้ไขตรรกะ MST Negate WhiteForestExecutor.cs: ปรับปรุง MysticalSpaceTyphoonActivate() ให้โซ่ใส่เฉพาะการ์ดที่ ต้องคงอยู่บนสนามเพื่อส่งผล เท่านั้น (IsFaceup() + CardType.Continuous / Field / Equip / Pendulum)"
  3. "ควบคุม Rush Mode / Lethal WhiteForestExecutor.cs: เพิ่ม Guard ให้กับ Starter / Extender ไม่ให้รันคอมโบยืดเยื้อเมื่อเข้าเงื่อนไข Lethal ชนะชัวร์แล้ว"
  4. "แก้ไข Sayer Tuner Spam Anime_SayerExecutor.cs: ตรวจสอบจำนวน Tuner บนสนามก่อน Normal Summon หรือใช้ Brain Control เพื่อให้มั่นใจว่ามี Non-Tuner ประสานงาน Synchro เสมอ"
  5. "บอทเพิ่มพลังให้ niburu ฝั่งตรงข้ามต้องระวังนะ" (Fix Forbidden Chalice buffing opponent monsters / Nibiru in battle, and force Nibiru Token to FaceUpDefence).
- **Audit & Root Causes Identified**:
  - **Dingirsu Cannibalism**: `DingirsuSummon()` lacked an enemy monster presence check and did not protect `Diabellze` or `Diabell Queen`, causing Dingirsu to overlay onto key bosses or summon into an empty board.
  - **MST False Negation**: `MysticalSpaceTyphoonActivate()` chained indiscriminately to Normal Spells (like `Brain Control`) which do not require remaining on the field to resolve.
  - **Combative Chalice Bug**: `ForbiddenChaliceActivate()` targeted `Enemy.BattlingMonster` when face-up in the Battle Phase, erroneously granting the opponent monster +400 ATK.
  - **Sayer Tuner Saturation**: `Anime_SayerExecutor` lacked checks on Tuner count prior to Normal Summon, causing dead boards with multiple Tuners and 0 Non-Tuners.
- **Architectural Solutions & Changes Implemented**:
  1. **Boss Preservation (`WhiteForestExecutor.cs`)**:
     - `IsWhiteForestBoss()` registered `DiabellzeTheWhiteWitch`.
     - `DingirsuSummon()` enforces `Enemy.GetMonsterCount() > 0`, strictly protects `Diabell Queen` and `Diabellze`, and only targets high-threat enemy monsters (ATK >= 2500, target-immune, or high threat).
     - `OnSelectCard()` added case `hint == 513 || hint == 519` (`HINTMSG_XMATERIAL`) excluding boss cards from being selected as Xyz materials.
  2. **Intelligent MST Logic (`WhiteForestExecutor.cs`)**:
     - Rewrote `MysticalSpaceTyphoonActivate()`: Only chains to face-up cards that must stay on field to resolve (`CardType.Continuous`, `CardType.Field`, `CardType.Equip`, Pendulum zones, floodgates like `Skill Drain` / `Necrovalley`). Never chains to Normal Spells/Traps. In non-chain situations, snipes high-threat enemy backrow in the End Phase.
  3. **Rush Mode / Lethal Guard (`WhiteForestExecutor.cs`)**:
     - Added `ShouldSkipForLethal()` checking `IsGoingSecond && Enemy.GetMonsterCount() == 0 && Bot.GetMonsters().Sum(m => m.Attack) >= Enemy.LifePoints`.
     - Guarded all starters and extenders (`ToyBoxActivate`, `ElzetteActivate`, `ElzetteSummon`, `AstellarSummon`, `AstellarActivate`, `SilvySummon`, `SilvyActivate`, `RuciaSummon`, `RuciaActivate`, `TalesActivate`, `WoesActivate`, `ToySoldierEffect`, `ToyTankEffect`).
  4. **Opponent Buff Prevention & Nibiru Token Position (`WhiteForestExecutor.cs`)**:
     - Rewrote `ForbiddenChaliceActivate()`: Removed battle phase targeting of `Enemy.BattlingMonster`. Battle phase Chalice now only targets `Bot.BattlingMonster` when +400 ATK wins combat. Main Phase Chalice prioritizes negating opponent activated monster effects or high-threat floodgates.
     - Added `OnSelectPosition()` override forcing `Primal Being Token` (`27204312`) to `CardPosition.FaceUpDefence`.
     - Enhanced `NibiruActivate()` with boss protection so it never tributes our own established winning board.
  5. **Anti-Tuner Spam & Synchro Synergy (`Anime_SayerExecutor.cs`)**:
     - Added `CanNormalSummonTuner(int level)` ensuring that a Tuner is only Normal Summoned if bot does not already have a Tuner on field and has a playable Non-Tuner.
     - Prioritized Non-Tuner Normal Summons (`PsychicTracker`, `HushedPsychicMinister`, `PsychicSnail`, `SilentPsychicWizard`, `PsiBlocker`) over Tuners.
     - Strengthened `BrainControlActivate()`: Validates that if an opponent Tuner is stolen, the bot has a Non-Tuner ready for Synchro climbing.
  6. **Skill Document Streamlining (`.agents/skills/yugioh-executor/SKILL.md`)**:
     - Condensed document from 1,433 lines down to 186 lines while preserving Strategic Decision System v2 (Core Loop, Threat Priority Model, 10 Golden Rules) and 13 critical anti-patterns.
- **Build & Exclusive Deployment**:
  - Successfully compiled with 0 errors via `BUILD_AND_DEPLOY.ps1` and deployed to `C:\Users\admin\Documents\EdoGame\`.
  - Archived entries 0.001–0.006 to `Docs/PROGRESS_ARCHIVE.md` to maintain `PROGRESS.md` within ~300 lines.

## 0.021. New ModernExecutor: WhiteForest (White Forest & Toy Box Synchro Engine) (2026-09-20)

- **User Directives**:
  - "WhiteForest.ydk ผมจัดเด็คให้แล้วครับ รบกวรวิเคราะอย่างละเอียดทุกใบและเขียน AI"
  - "หมวด Modern"
- **Card & Banlist Audit (`cards.cdb` & `0TCG.lflist.conf`)**:
  - วิเคราะห์การ์ด 43 ใบ (Main 40 / Extra 15 / Side 15) ครบถ้วน 100% จาก `cards.cdb`
  - **Banlist Status**: ผ่านเกณฑ์ 100% (0 Banlist Violations)
  - **Category**: หมวด **Modern** ตาม `ModernArchetypes` ใน `MainWindow.xaml.cs`
- **Core Engine & Architecture (`WhiteForestExecutor.cs`)**:
  - สืบทอดจาก `ModernExecutor` พร้อมตาราง CARD AUDIT ที่ส่วนหัว
  - **White Forest & Toy Box Recursive Loop**:
    - `Astellar` / `Elzette` ส่ง Spell/Trap ลงสุสานเพื่อดึง Tuner/Extender จากเด็ค โดยเลือกลำดับส่ง `Toy Soldier` / `Toy Tank` ที่เซ็ตอยู่ใน S/T zone หรือเวท `Tales` / `Woes` / `Scourge`
    - มอนสเตอร์ Toy จะ Special Summon ตัวเองลงสนามฟรี และเวท White Forest จะเซ็ตตัวเองกลับลงสนามทันที ไม่สูญเสียทรัพยากร
  - **Synchro Climbing & Board Control**:
    - Level 2 + Level 4 = `Rciela` (Lv6 Synchro Tuner ส่ง S/T เสิร์ช White Forest และคุ้มกัน Synchro จากการถูกทำลาย)
    - `Rciela` + `Astellar` (ที่ชุบตัวเองจากสุสาน) = `Diabell, Queen of the White Forest` (Lv8 Boss)
    - `Diabell Queen` Quick Effect: เมื่อศัตรูเปิดใช้เอฟเฟกต์ ส่ง S/T เรียก `Silvera` ออกมาคว่ำหน้ามอนสเตอร์ศัตรูทั้งหมดทันที (Book of Eclipse)
    - ขัดขวางด้วย Handtrap หลายชั้น: `Diabellstar Vengeance` (Negate + Banish), `Nibiru` (พร้อมล็อก Primal Being Token ลงใน FaceUpDefence เสมอ), `Ghost Ogre`, `Effect Veiler`, `D.D. Crow`, และ `Zalen the Shackled Dragon`
  - **Strict Anti-Pattern Safeguards**:
    - บังคับ `OnSelectCard` สำหรับ Hint 502/503/504 เลือกเฉพาะการ์ดศัตรู (`c.Controller == 1`)
    - ฟังก์ชัน `GetExpendableSpellTrapCost()` จัดลำดับการส่ง S/T เป็น Cost โดยคัดเลือกเฉพาะการ์ดที่ลอยหรือเซ็ตกลับมาได้ก่อน ป้องกันการส่งคีย์การ์ดทิ้ง
- **Deck & Bot Registration**:
  - วางไฟล์ `WhiteForest.ydk` ใน `windbot-fork\Decks\`
  - ลงทะเบียนใน `bots.json` (`name: "WhiteForest"`, `deck: "WhiteForest"`, masterRules: [4, 5])
- **Exclusive Deployment**:
  - คอมไพล์และ Deploy ผ่าน `BUILD_AND_DEPLOY.ps1` มายัง `C:\Users\admin\Documents\EdoGame\` ครบถ้วน (WindBot.dll, ExecutorBase.dll, core.dll, bots.json, WhiteForest.ydk, DashBot)


---

## 📚 Historical Development Archives
> ประวัติการพัฒนาและบันทึกการทดสอบเวอร์ชันก่อนหน้า (รวมถึง 0.001 - 0.021) ได้รับการย้ายไปจัดเก็บอย่างเป็นหมวดหมู่ที่:
> [Docs/PROGRESS_ARCHIVE.md](Docs/PROGRESS_ARCHIVE.md)

