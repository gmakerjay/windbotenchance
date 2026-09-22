# Progress Log: Central Core Architecture & Universal Heuristics Overhaul, MagistusFairy ModernExecutor Refactor, PhantomKnight ModernExecutor

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

### Enhancements & Architecture Implemented
1. **Core Callback Engine Overhaul**:
   - **`OnSelectOption(IList<long> options)`**:
     - `Medius the Pure`: Returns `1` (Special Summon) when monster zone has room, ensuring Nervedo is summoned directly to field.
     - `Artmage Varnish`: Returns `0` (Add Artmage card to hand) when Acropolis is already face-up on field.
     - `Triple Tactics Talent`: Returns `1` (Take control) if enemy controls monsters, else `0` (Draw 2).
   - **`OnSelectYesNo(long desc)`**:
     - Automatically approves Finmel draw, Graflare SSet, Litera GY retrieve, Vandalism Medius search, Impasto backrow bounce, Nervedo P-zone negate, and Acropolis destruction protection.
   - **`OnSelectCard(cards, min, max, hint, cancelable)`**:
     - Hint 509 (SPSUMMON): Prioritizes Nerva from Extra, and Finmel/Graflare/Power Patron from Deck.
     - Hint 506 (ATOHAND): Prioritizes Medius, Acropolis, Impasto Recapture, Masterwork, Varnish, Finmel.
     - Hint 510 (SET): Prioritizes `ImpastoRecapture` (activatable turn set!) > `Masterwork` > `Pact`.
     - Hint 511 (FMATERIAL) & 533 (LMATERIAL): Strict Ace protection (prevents fusing or linking away Nerva/Diactorus).
     - Hint 501 (DISCARD): Discards redundant/duplicate Spells, strictly protecting `ImpastoRecapture` and `SuperPoly`.
     - Hint 507 (TODECK): Selects distinct GY cards for Masterwork and low-value cards for Medius revival.
2. **Real `Artmage Power Patron` Logic**:
   - Implemented on-field Quick Fusion into Diactorus/Nerva.
   - Implemented GY search for `ImpastoRecapture`, `Masterwork`, `Pact`, `Varnish`, `Vandalism`, `Acropolis`.
3. **Tactical Multi-Type (Race) Synergy**:
   - Dynamic counting of distinct on-field Races (Fairy, Warrior, Dragon, Spellcaster, Illusion).
   - Activates Finmel's Quick Effect blanket monster negate + ATK halving and Diactorus's omni-negate when 3+ Types are present.
4. **Smart Board-Wipe with Nerva**:
   - On opponent's turn: Chains to any Artmage monster quick effect (e.g. Litera bounce, Finmel negate, Diactorus position change) to trigger a surprise Quick Raigeki + Harpie's Feather Duster!
   - On our turn: Chains when opponent controls cards to clear board for lethal attacks.
5. **Bot Registration & Deck Synchronization**:
   - Added `ArtMage` and `Artmage` to `windbot-fork/bots.json`.
   - Synchronized `ArtMage.ydk` to `deck/ArtMage.ydk`.

### Build & Verification
- **Compilation**: `BUILD_AND_DEPLOY.ps1` succeeded with 0 errors and 0 warnings in `ArtMageExecutor.cs`.
- **Deployment**: Deployed all updated binaries (`WindBot.dll`, `ExecutorBase.dll`, `core.dll`, `bots.json`, `DashBot.exe`, etc.) exclusively to `C:\Users\admin\Documents\EdoGame\`.

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
   - Extracts exact OCGCore effect constants:
     - `EFFECT_CANNOT_BE_EFFECT_TARGET` -> 202 Target-Immune monsters
     - `EFFECT_INDESTRUCTABLE_BATTLE` -> 377 Battle-Immune monsters
     - `EFFECT_REFLECT_BATTLE_DAMAGE` / `EFFECT_AVOID_BATTLE_DAMAGE` -> 40 Dangerous Battle monsters (Yubel all forms, Mikanko, Timelords, Amazoness Swords Woman, Saint Azamina, etc.)
     - `SUMMON_TYPE_FUSION` / `Fusion.CreateSummonEff` / `CATEGORY_FUSION_SUMMON` -> 145 Fusion Spells (Branded Fusion, Invocation, Fusion Destiny, Super Poly, etc.)
   - Generates `CardIntelligence.Generated.cs` as a partial class for seamless compilation.
2. **Central Database & Query API Integration (`CardIntelligence.cs`)**:
   - Converted `CardIntelligence` into a partial class.
   - Integrated generated sets into query methods: `IsTargetImmune(cardId)`, `IsInvincibleBattle(cardId)`, `IsDangerousBattleTarget(defender, attacker)`, and `IsFusionSpell(cardId)`.
3. **CardExtension Dynamic Bridging (`CardExtension.cs`)**:
   - Upgraded core extension methods used across all 30+ executors to query `CardIntelligence` $O(1)$ HashSets first:
     - `card.IsMonsterInvincible()` -> queries `CardIntelligence.IsInvincibleBattle()` || `InvincibleMonster` enum.
     - `card.IsMonsterDangerous()` -> queries `CardIntelligence.IsDangerousBattleTarget()` || `DangerousMonster` enum || Mikanko archetype (0x18d).
     - `card.IsShouldNotBeTarget()` -> queries `CardIntelligence.IsTargetImmune()` || `ShouldNotBeTarget` enum.
     - `card.IsFloodgate()` -> queries `CardIntelligence.IsFloodgate()` || `Floodgate` enum.
     - `card.IsFusionSpell()` -> queries `CardIntelligence.IsFusionSpell()` || `FusionSpell` enum || dynamic PSCT keywords ("Fusion Summon", "อัญเชิญฟิวชั่น").
     - `card.IsMonsterAttackWhileInDefPos()` -> checks Superheavy Samurai archetype (0x9a) || `DefenseAttackMonster` enum.
4. **Lua Engine `Attempting to access deleted object` Crash Resolution (`utility.lua`)**:
   - User reported script error: `[string "utility.lua"]:2889: Attempting to access deleted object` triggered during End Phase / Delayed Operations.
   - Root Cause: In `repositories/delta-bagooska/script/utility.lua`, `Auxiliary.DelayedOperation` created a temporary card `Group` without calling `g:KeepAlive()`. When `EVENT_PHASE` triggered turns later, C++ OCGCore had already freed the group, causing `e:GetLabelObject():Filter(...)` to crash.
   - Fixed by adding `g:KeepAlive()`, safe `pcall` handling in `get_affected_group`, and `g:DeleteGroup()` memory cleanup on completion.
5. **Build & Deployment**:
   - Deployed updated binaries, databases, and Lua patches to `C:\Users\admin\Documents\EdoGame\`.

---

## 0.034. AFS (Azamina Fiendsmith Snake-Eye) Decision Engine Overhaul & Game-Stall Fix (2026-09-22)

### Overview
- **Deck**: `AFS.ydk` & `2026_AFS.ydk` (40 Main Deck, 15 Extra Deck, 15 Side Deck)
- **Executors Modified**: `AFSExecutor.cs`, `ModernExecutor.cs` (Rule-Based C# .NET 10)
- **Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`

### Root Causes & Fixes Implemented
1. **`Deception of the Sinful Spoils` (66328392) Hand Activation Lock**:
   - `DeceptionEffect()` previously only checked `Bot.GetMonsters().FirstOrDefault(c => ... != null)`.
   - Continuous Spell `Deception` can be activated from Hand freely without monsters on field. On field, its ignition effect can tribute from hand OR field.
   - Fixed by allowing free hand activation and expanding tribute targets to hand fodder (`Lurrie`, `Poplar`, `Oak`, handtraps) or field.
2. **`Forbidden Droplet` Self-Interruption**:
   - `ForbiddenDropletEffect` was chaining into bot's own normal summon / starter ignition chains, discarding key resources (`Deception`, `Poplar`) and corrupting `SelectCard` queues.
   - Added `if (Duel.LastChainPlayer == 0) return false;` to prevent interrupting our own combo starters.
3. **`Fiendsmith's Lacrima` (46640168) & `aux.ToHandOrElse` (SelectOption Bug)**:
   - On Fusion Summon, Lacrima calls `aux.ToHandOrElse` prompting `Duel.SelectOption(573, str)` (0 = Add to Hand, 1 = Special Summon it).
   - WindBot defaulted to 0 (Add to Hand), leaving only 1 Level 6 Fiend on field and blocking `D/D/D Wave High King Caesar`.
   - Overrode `OnSelectOption` in both `ModernExecutor.cs` and `AFSExecutor.cs` to return 1 (Special Summon) for Lacrima.
4. **`Fiendsmith's Sequence` GY Material Shuffling**:
   - Sequence was previously shuffling both Engravers from GY, leaving 0 Engravers for Lacrima to revive.
   - Configured `OnSelectCard` hint 511 / `TODECK` to prioritize `LacrimaTheCrimsonTears` > `FabledLurrie` > `FiendsmithsRequiem` > `FiendsmithEngraver`, preserving Engraver in GY.
5. **`DDDWaveHighKingCaesar` Priority & Sequence / Princess Guard**:
   - Promoted `CaesarSummon` to Tier 3 before `SequenceSummon` and `PrincessSummon`.
   - `SequenceSummon` and `PrometheanPrincessSummon` now explicitly guard against consuming Level 6 Fiends while Caesar is unsummoned.
6. **`PrometheanPrincess` Continuous FIRE Lock Guard**:
   - Princess continuously locks player into Special Summoning only FIRE monsters.
   - Added guard requiring at least 1 FIRE monster in GY to revive before summoning Princess, and ensuring Fiendsmith / Azamina lines are not aborted.
7. **Comprehensive 21-Engine `OnSelectCard` Integration**:
   - Explicitly mapped targets for `SnakeEyeAsh` (Search Poplar; send S/T Poplar/Temple/Deception; summon Flamberge/Oak), `SnakeEyesPoplar`, `Bonfire`, `DivineTempleOfTheSnakeEye`, `OriginalSinfulSpoils`, `Wanted`, `Diabellstar`, `Deception`, `TheHallowedAzamina`, `Tract`, `Engraver`, `Requiem`, `Lacrima`, `Sequence`, `FlambergeDragon`, `Oak`, `Princess`, `SPLittleKnight`, `IPMasquerena`.
8. **Build & Exclusive Deployment**:
   - Built and deployed via `BUILD_AND_DEPLOY.ps1` with 0 Errors directly to `C:\Users\admin\Documents\EdoGame\`.

---

## 0.033. Azamina Fiendsmith Snake-Eye & Exosister Lua Error Resolution (2026-09-21)

### Overview
- **Issue Reported by User**:
  1. "azamina fiendsmith เล่นผิดเด็คครับปรับปรุงด้วย" (Bot ran Exosister cards instead of Azamina Fiendsmith when Azamina Fiendsmith Snake-Eye was chosen).
  2. "แล้วก็มี lua error" (Script error: `c52335937.lua:104: Attempting to access deleted object` / `utility.lua:2894`).
- **Root Causes Identified**:
  1. **Lua Script Deallocated Group**: `c52335937.lua` (`Exosister Betrayal`) line 39 created `local g = Group.CreateGroup()` stored in effect LabelObject without calling `g:KeepAlive()`. The C++ OCGCore freed the group on phase/turn changes, causing any subsequent call to `g:Clear()` or `g:Merge()` or filtering in `utility.lua:2894` to throw `Attempting to access deleted object`.
  2. **Wrong Deck Fallback in WindBot**: The DashBot UI passed `Deck="2026_AFS"`. `AFSExecutor.cs` was only annotated with `[Deck("AFS", "AFS")]` and `bots.json` lacked AFS. Because `NormalizeDeckName("2026_AFS")` ("2026afs") did not match `"afs"`, `DecksManager.Instantiate` failed to find a match and fell back to: `Deck not found, loading random: 2026_Exosister`. The bot was literally forced to pilot random Exosister, which then searched and activated `Exosister Betrayal (52335937)`, triggering the Lua crash!
- **Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`

### Enhancements & Fixes
1. **Lua Script `c52335937.lua` (`Exosister Betrayal`)**:
   - Added `g:KeepAlive()` immediately after `Group.CreateGroup()`.
   - Added null/validity guards in `s.regop` and `s.rmtg` to re-instantiate with `KeepAlive()` if ever lost.
   - Synchronized across all 3 repository locations (`script/c52335937.lua`, `script/official/c52335937.lua`, and `repositories/official-scripts/official/c52335937.lua`).
2. **`DecksManager.cs` Prefix Stripping & Robust Matching**:
   - Implemented `StripPrefix`: strips prefixes (`2026`, `ai`, `expert2026`, `goat`, `anime`) so requests like `2026_AFS` or `AFS` seamlessly resolve to each other.
   - 3-pass lookup: Exact normalized match -> Prefix-stripped match -> Substring contains match.
   - Disk deck preservation: checks if the exact requested deck file exists in `Decks/` (e.g. `2026_AFS.ydk`) and preserves it on the executor instead of overwriting with the default attribute file name.
3. **`AFSExecutor.cs` Aliases & `bots.json` Registration**:
   - Added attributes: `[Deck("AFS", "AFS")]`, `[Deck("2026_AFS", "2026_AFS")]`, `[Deck("Azamina Fiendsmith Snake-Eye", "AFS")]`, `[Deck("Azamina Fiendsmith", "AFS")]`, `[Deck("Azamina", "AFS")]`, `[Deck("Fiendsmith", "AFS")]`.
   - Registered `AFS`, `Azamina Fiendsmith Snake-Eye`, `Azamina Fiendsmith`, and `2026_AFS` in `bots.json`.
4. **Curated Recipe Synchronization (`2026_AFS.ydk` & `AFS.ydk`)**:
   - Synchronized player's curated 40-card + 15-extra deck list to `windbot-fork/Decks/` and runtime folders.
5. **Major Strategic Expansion in `AFSExecutor.cs`**:
   - Added support for `Original Sinful Spoils - Snake-Eye` (`89023486`) send costs and Level 1 FIRE summons.
   - Added `'Moon of the Closed Heaven'` (`71818935`) Link-2 bridge to `Fiendsmith's Requiem`.
   - Added `Maxx "C"` (`23434538`), `Mulcharmy Purulia` (`84192580`), `Forbidden Droplet` (`24299458`), `Dark Ruler No More` (`54693926`), `Lightning Storm` (`14532163`), `Harpie's Feather Duster` (`18144507`), `Evenly Matched` (`15693423`), `Bystials`, `Solemn Strike`, and `Dimensional Barrier`.
   - Implemented Rule 1 separation in `OnSelectCard` (removal vs send costs) and Rule 14 opponent prompt decline in `OnSelectEffectYn`.
   - Added duplicate handtrap prevention guards across chains.

### Build & Verification
- **Compilation**: `BUILD_AND_DEPLOY.ps1` compiled with 0 errors.
- **Verification**: Tested `dotnet WindBot.dll Deck=2026_AFS`, `Deck=AFS`, and `Deck="Azamina Fiendsmith Snake-Eye"` — all successfully logged `Deck found, loading ...` and loaded `AFSExecutor` with 0 failures!
- **Deployment**: Exclusively deployed to `C:\Users\admin\Documents\EdoGame\`.

---

## 0.032. Central Core Architecture & Universal Heuristics Overhaul (2026-09-21)

### Overview
- **Scope**: Central AI Engine (`ExecutorBase`, `GameAI`, `ModernExecutor`, `DefaultExecutor`, `CardIntelligence`, `AntiFloodgateHelper`) affecting all 140+ deck executors.
- **Objective**: Maximize AI tactical execution and eliminate systemic misplays (EMZ clogging, Bagooska position bugs, duplicate handtraps, harmful opponent prompt acceptance, missed direct attack lethals).
- **Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`

### Key Architectural Enhancements
1. **Master Rule 5 EMZ Preservation & Column Safeguards (`Executor.cs`)**:
   - Restricted EMZ auto-routing (`0x20`) strictly to Link monsters and face-up Pendulum monsters from Extra Deck.
   - Fusion, Synchro, and Xyz monsters now default to Main Monster Zones, preserving EMZs for subsequent Link combos.
   - Avoids columns 1 and 3 when opposing field contains or threatens `Relinquished Anima`.
2. **Floodgate Card ID Corrections & Bagooska Defense Safeguard (`ModernExecutor.cs`, `CardIntelligence.cs`)**:
   - Fixed official passcode mappings for `Number 41: Bagooska` (`90590303, 90590304`), `El Shaddoll Winda` (`94977269, 94977270`), `El Shaddoll Anoyatyllis` (`19261966`), and `Naturia Exterio` (`99916754`).
   - `OnSelectPosition`: Strictly forces `FaceUpDefence` when summoning Bagooska, enabling its continuous effect (negate all activated monster effects and turn all monsters to Defense).
   - Enforced `FaceUpAttack` for Link monsters and `FaceUpDefence` for low ATK monsters (handtraps, 0 ATK combo starters, DEF > ATK).
3. **Universal Duplicate Handtrap & Negate Prevention (`GameAI.cs`, `ModernExecutor.cs`, `DefaultExecutor.cs`)**:
   - `GameAI.OnSelectChain`: Skips chaining duplicate once-per-turn handtraps (`CardIntelligence.IsHandtrap`) if the bot already has the identical card in the current chain.
   - `ModernExecutor.SmartHandTrapChain`: Guarded against duplicate handtrap activations in the same chain.
   - `DefaultExecutor.DefaultMaxxC` & `DefaultDrollAndLockBird`: Added resolved effect tracking (`resolvedEffectIdList`) and duplicate chain guards to prevent burning multiple copies.
4. **Lethal & Archetype Direct Attack Optimization (`DefaultExecutor.cs`)**:
   - `OnSelectAttackTarget`: Added immediate check for direct attack lethal (`attacker.CanDirectAttack && attacker.Attack >= Enemy.LifePoints`).
   - Prioritizes direct attacks for archetype triggers (e.g. `Sky Striker Ace - Hayate` searching/sending to GY, `Toon` monsters) when enemy controls no floodgates or negators.
5. **Hostile Opponent Prompt Safeguard (`GameAI.cs`)**:
   - `OnSelectEffectYn`: Automatically declines unhandled optional effect prompts from opponent cards (`card.Controller == 1`), preventing self-damage, resource loss, or falling for opponent traps while keeping `true` default for friendly cards (`card.Controller == 0`).
6. **Central Knowledge Ingestion (`CardIntelligence.cs`, `AntiFloodgateHelper.cs`)**:
   - Added high-impact modern starters & chokepoints: `Bonfire`, `WANTED`, `Snake-Eye Ash`, `Snake-Eyes Poplar`, `Promethean Princess`, `Fiendsmith Engraver`, `Fiendsmith's Tract`, `Fiendsmith's Sequence`, `S:P Little Knight`.
   - Added `Dimension Shifter` to `UniversalHandtraps`.

### Build & Verification
- **Build**: `BUILD_AND_DEPLOY.ps1` succeeded with 0 errors.
- **Runtime Load Test**: Verified startup and deck loading with `dotnet WindBot.dll Deck=2026_BrElfnote` (exited cleanly, 186 decks loaded).
- **Deployment**: Deployed all updated binaries (`WindBot.dll`, `ExecutorBase.dll`, `core.dll`, `DashBot.exe`, etc.) exclusively to `C:\Users\admin\Documents\EdoGame\`.

---

## MagistusFairy: Complete Rule-Based ModernExecutor Refactor (2026-09-21)

### Overview
- **Deck**: `2026_MagistusFairy.ydk` (40 Main Deck, 15 Extra Deck, 15 Side Deck)
- **Executor**: `_2026_MagistusFairyExecutor.cs` (Rule-Based C# .NET 10, inheriting `ModernExecutor`)
- **Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`

### Root Causes & Fixes in Refactor
1. **Correction of Hallucinated Card Effects & Wrong Constants**:
   - `CardId.GeniaOfTheRing (99745551)`: Was mislabeled as Normal Summon starter; verified in `cards.cdb` to be `Danger!? Tsuchinoko?` (Hand activation to discard & SS + draw, GY trigger).
   - `CardId.SpentaTheMagistusSealer (42544773)`: Was mislabeled and given fabricated effect; verified to be `Spoon, the Seal of Magistus` (Hand: discard to search Magistus monster; GY: banish to equip Magistus from ED/GY to monster on field).
   - `CardId.FairyTailMatchgiru (19144622)`: Mislabeled with fake bounce effect; verified to be `Fairy Tail - Matchlille` (SS from hand/GY if 1850 ATK on field, on NS/SS searches Fairy Tail S/T, pays 500 LP to rename opponent monster to "Fairy Prince").
   - `CardId.OnceUponAFairyTail (19326613)`: Mislabeled with fake search/discard; verified to be `Fairy Tail Long Long Ago` (Quick-Play Spell to SS up to 1 LIGHT Fairy Tail each from Hand/GY/Banishment, sets itself from GY).
   - `CardId.TailsOfTheFairyTails (82119326)`: Mislabeled; verified to be `Tales of Fairy Tail` (Equip Spell; +1000 ATK; fuses Spellcaster using hand/field + opponent's "Fairy Prince"; GY equip & Normal Summon 1850 ATK).
   - `CardId.TellerOfFairyTails (4026187)`: Mislabeled; verified to be `Chronicler of Fairy Tail Tales` (Full opponent board wipe + 500 burn each if fused with "Fairy Prince"; Quick omni-negate & destroy).
   - `Zoroa, the Magistus Victorious Verethragna (37260677)`: Removed hallucinated GY self-revive; implemented proper equip stealing and Quick monster negate + card pop.
   - `Crowley, the Gifted of Magistus (875572)`: Removed fake attribute declaration / send cost; implemented Special Summon from hand when added, and Fusion Summoning `Invoked Mechaba` (treated as Aleister the Invoker) / `Zoroa Verethragna` / `Weaver`.
   - `Regulus, the Prince of Endymion (96228804)`: Removed fake spell counter destruction; implemented hand reveal SS and search for `Endymion Empire`.
   - `Endymion Empire (34041788)`: Continuous Spell that searches Regulus and Special Summons Spellcaster from hand.
   - `Fairy Tail Ball (56725612)`: Continuous Spell searching Fairy Tail on activation, and negating opponent Special Summoned monsters and turning them into "Fairy Prince".
   - `Summon Sorceress (61665245)`: Disabled summon and effect to prevent giving opponent free monsters (Anti-Advantage Gate).

2. **Strategic Combos & Priority Groups**:
   - **Tier 1 (Omni-Negates & Fast Disruptions)**:
     - `Invoked Mechaba` (Quick omni-negate & banish by discarding matching card type)
     - `Chronicler of Fairy Tail Tales` (Quick omni-negate & destroy by banishing Fairy Tail)
     - `Zoroa the Magistus Victorious Verethragna` (Quick monster negate + opponent pop)
     - `ForbiddenCrown`, `SuperPolymerization` (Garura / Mudragon)
     - `FairyTailLuna` (Quick bounce), `FairyTailSnow` (Book of Moon & GY Quick SS)
     - `CalledByTheGrave`, `AshBlossom`, `MulcharmyFuwalos`, `DrollAndLockBird`
   - **Tier 2 (Spells & Board Actions)**:
     - `InstantFusion`, `EndymionEmpire`, `FairyTailBall`, `TalesOfFairyTail`, `FairyTailLongLongAgo`, `VerreMagicLacrimaOfLight`
   - **Tier 3 (Starters & Extenders)**:
     - Danger Tsuchinoko hand activation
     - Spoon search Zoroa / Crowley
     - Zoroa NS -> equip Artemis -> SS Lv 4 Spellcaster
     - Artemis equip search Crowley -> Crowley triggers SS in hand -> Crowley fuses Mechaba
     - Matchlille Prince rename -> Tales of Fairy Tail fuses opponent Prince into Chronicler -> wipes entire opponent board!
   - **Tier 4 (Extra Deck Extenders & Bosses)**:
     - Artemis Link-1, Fairy Tail Wickat Rank 4 Xyz (dumps Snow & Tales), Weaver of Fairy Tails, Chronicler, Mechaba, Verethragna, Selene Link-3, Four Charmers Link-4.

3. **Robust Engine-Level Selection (`OnSelectCard`)**:
   - Hint 533 (`HINTMSG_FMATERIAL`): Automatically prioritizes opponent's "Fairy Prince" (10000120) to enable Chronicler's board wipe.
   - Hint 501 (`HINTMSG_DISCARD`): Prioritizes Snow, Genni, Tsuchinoko, duplicate spells.
   - Hint 504 (`HINTMSG_REMOVE`): Intelligently banishes spent GY spells and low-impact cards for Snow's 7-card cost without touching Ace cards.
   - Hint 508 (`HINTMSG_TOGRAVE`): Dumps Fairy Tail Snow straight to GY for instant disruption.
   - Guarded against `Bot.Deck.Any` face-down card ID 0 bug using `GetRemainingCount(id) > 0`.

4. **Build & Exclusive Deployment**:
   - Successfully compiled with `BUILD_AND_DEPLOY.ps1` (0 Errors).
   - Deployed all binaries and assets directly to `C:\Users\admin\Documents\EdoGame\`.

---

## PhantomKnight: Rule-Based ModernExecutor, Official Artwork Ingestion & Thai CDB Localization (2026-09-21)

### Overview
- **Deck**: `PhantomKnight.ydk` (40 Main Deck, 15 Extra Deck, 15 Side Deck - 46 Unique IDs)
- **Executor**: `_2026_PhantomKnightExecutor.cs` (Rule-Based C# / .NET 10, inheriting `ModernExecutor`)
- **DashBot Registration**: Categorized as **Modern** (Amber/Gold badge, display name `"Phantom Knights"`)
- **Bot Registration**: Registered in `bots.json` under names `"PhantomKnight"` and `"Phantom Knights"`
- **Exclusive Deploy Target**: `C:\Users\admin\Documents\EdoGame\`

### Work Completed

#### 1. Official Card Artwork Sourcing & Download (7 Cards)
- Audited all 46 unique cards in `PhantomKnight.ydk` against `EdoGame/pics/`. Identified 7 cards missing artwork.
- Downloaded high-resolution official images directly from YGOPRODeck CDN (`https://images.ygoprodeck.com/images/cards/<id>.jpg`):
  - `83566725.jpg` (The Phantom Knights of Doomed Soleret)
  - `46072770.jpg` (The Phantom Knights of Decayed Cloak)
  - `62104532.jpg` (The Phantom Knights' Rank-Up-Magic Requiem)
  - `85257384.jpg` (The Phantom Knights of Umbrage Veil)
  - `78449284.jpg` (The Phantom Knights of Malevolent Scythe)
  - `16237004.jpg` (Shamanite Shamanknight)
  - `98476659.jpg` (Pot of Sloth)
- Verified valid JPEG format and saved to both `EdoGame/pics/` and `src/YGO_SOURCE_CLEAN/pics/`.

#### 2. Thai Localization Ingestion (7 Cards)
- Translated card descriptions into standard Yu-Gi-Oh Thai phrasing while strictly keeping card names 100% in English.
- Injected into SQLite databases:
  - `config\languages\Thai\cards.delta.cdb`
  - `src\YGO_SOURCE_CLEAN\config\languages\Thai\cards.delta.cdb`
  - `cards.cdb` / `src\YGO_SOURCE_CLEAN\cards.cdb` / `src\YGO_SOURCE_CLEAN\windbot-fork\cards.cdb`
  - `WindBot\cards.cdb` / `WindBot\cards.delta.cdb` / `expansions\cards.cdb`
- Verified that all 7 cards have valid Thai effect text and intact English card names.

#### 3. Central Intelligence Updates (`CardIntelligence.cs`)
- Added `DominusImpulse` (40366667) and `DominusSpark` (6325660) to `UniversalHandtraps`.
- Added `DarkRequiem` (1621413) to `KnownNegators`.
- Added `EvilswarmOphion` (91279700) and `TyphonSkyCrisis` (93039339) to `FloodgateMonsters`.

#### 4. Rule-Based ModernExecutor (`_2026_PhantomKnightExecutor.cs`)
- **Zero Magic Numbers**: Full CardId constants for all 46 cards.
- **Anti-Brick Architecture ("ระวังการ์ดค้างมือ")**:
  - `DoomedSoleret` Special Summons from hand when field is empty *before* any Normal Summon.
  - `DecayedCloak` Special Summons by revealing another PK card *before* Normal Summoning.
  - `TornScales` acts as the primary hand cleaner, discarding dead/stuck cards (`Boots`, `Soleret`, `Gloves`, `Fog Blade`, `Wing`) to dump from deck.
  - Full compatibility with `Dominus Impulse` and `Dominus Spark`: because the entire PK engine is pure DARK, the Dominus attribute restriction has 0 penalty.
  - Controlled backrow setting prevents backrow clogging, leaving open zones for `Rusty Bardiche` and `Soleret`.
- **First-Turn End Board**:
  - `The Phantom Knights of Rusty Bardiche` + `Dark Requiem Xyz Dragon` (3x monster negate + pop + revive Xyz) + `Fog Blade` (negate + attack lock) + `Evilswarm Nightmare` / `Evilswarm Ophion` (via `RUM Launch`).
- **Turn 2 Board Breaking & OTK**:
  - `Harpie's Feather Duster` + `Evenly Matched` / `Triple Tactics` / `Pot of Sloth` / `TY-PHON`.
  - `Break Sword` targeted pop floats into two Level 4 PKs -> `Raider's Knight` -> `Arc Rebellion Xyz Dragon` (negates entire board, gains ATK of all other monsters, swings for 8,000-15,000+ ATK OTK).
- **Bulletproof `OnSelectCard`**: Comprehensive hint handlers for 500 (Release), 501 (Discard), 502 (Destroy), 504 (Banish), 505 (Search to hand), 506 (To deck), 508 (To grave), 509 (SpSummon), 512 (Xyz detach), 552/572 (Negate), with fallback guarantee `result.Count >= min`.
- **Stat-Aware `OnSelectPosition` & Smart `OnSelectOption`**.

#### 5. Build, Exclusive Deployment & Git Sync
- Executed `BUILD_AND_DEPLOY.ps1` with 0 Errors and 0 new warnings.
- Exclusive deploy to `C:\Users\admin\Documents\EdoGame\`.
- All changes committed and pushed to `origin/main`.

---


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
