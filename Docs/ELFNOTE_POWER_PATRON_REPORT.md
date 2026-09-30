# Elfnote / Power Patron / Theorealize Hybrid Engine Architecture Report

**Version**: 1.0.0  
**Date**: 2026-09-30  
**Author**: Antigravity Pair Programmer  
**Platform**: WindBot .NET 10.0 (Rule-Based C# ModernExecutor + Decoupled Domain Plugin)  
**Deployment Target**: `C:\Users\admin\Documents\EdoGame\`  

---

## 1. Executive Summary & Problem Analysis

The user provided a 66-card hybrid deck list consisting of:
- **Elfnote Archetype**: Center Main Monster Zone (Zone 2) column-locking, recursive Synchro swarm.
- **Power Patron Archetype**: Column lockdown, negation, and Link/Synchro boss engines (*Junora*, *Junordo*, *Medicurius*, *Purification*).
- **Theorealize Archetype**: *Theorealize Past Lull* and *Theorealize Medius* Extra Deck cheat-outs.
- **Ars Magna Engine**: *Citrinitas*, *Infinity and Finity*, *Purification and Corruption* hand/field banish cycles.
- **Handtraps & Staples**: Maxx "C", Fuwalos, Ash Blossom, Ghost Ogre, Nibiru, Impermanence, Solemn Judgment, Triple Tactics Talent.

### Key Optimization Challenges Identified:
1. **Consistency & Overcapacity**: The raw list was 51 Main Deck cards containing conflicting, unsearchable bricks (*Gordian Slicer*, *The Great Gallant Bandit*, *Vidolium*). *Gordian Slicer* was particularly toxic, as it randomly banished 6 Extra Deck monsters face-down, ruining Synchro/Link climb ladders.
2. **Center Zone Placement Requirement**: Elfnote monsters (*Lucina*, *Regina*, *June Pride*, *Strelitzia*) require placement in the Center Main Monster Zone (Zone 2, index `1 << 2`) to activate column-based floodgate and search triggers. Without custom placement heuristics, AI would place monsters in arbitrary leftmost slots (Zone 0 or 1).
3. **Cheat-Out Activation Traps**: *Theorealize Medius* triggers when a monster is banished face-up, offering options to cheat out monsters directly from the Extra Deck (*Artmage Diactorus*). The AI required exact `OnSelectOption` interceptors.
4. **Accidental Tribute Normal Summons**: Level 6 monsters (*Lucina*, *Regina*, *Tinia*, *Fortuna*) could accidentally trigger Tribute Summons, sacrificing valuable board monsters.

---

## 2. Streamlined Deck Construction (40 Main / 15 Extra)

### Main Deck (40 Cards)
- **Elfnote Engine (11 Cards)**:
  - 3x *Elfnote Lucina* (13597785) - Lv 6 Starter, searches Elfnote monster
  - 3x *Elfnote Regina* (56651978) - Lv 6 Quick Effect, Special Summons Tuner from Deck
  - 2x *Elfnote Tinia* (59581480) - Lv 6 Center Zone disruption & Spell placer
  - 1x *Elfnote Fortuna* (85976588) - Lv 6 Trap placer & bounce
  - 2x *Elfnote Power Patron* (12375297) - Lv 1 Tuner & Level modulator
- **Power Patron & Theorealize Engine (7 Cards)**:
  - 1x *Power Patron Shadow Spirit Junordo* (10266279) - Banishes 3 to summon Junora
  - 2x *Medius the Pure* (97556336) - Normal/Special Summon starter for Patron
  - 2x *Theorealize Medius* (90875418) - Special Summons when Patron on field; cheats Extra Deck
  - 1x *Junora the Power Patron of Tuning* (5914858) - Column lock & tuner
  - 1x *Jet Synchron* (9742784) - Lv 1 Machine Tuner & graveyard recursive
- **Ars Magna & Spells (8 Cards)**:
  - 3x *Unleashed Power Patron Portal - Terminus* (25661743) - Dumps Medicurius/Junora, searches
  - 1x *Theorealize Past Lull* (36709484) - Special Summons Medius from Deck
  - 1x *Ars Magna "Citrinitas"* (37279096) - Continuous search engine
  - 1x *Ars Magna of Infinity and Finity* (36270527) - Hand banish search & monster banish
  - 1x *Ars Magna of Purification and Corruption* (62368221) - Hand banish search & backrow banish
  - 1x *Elfnotes: Welcome Home* (64491754) - Swarm extender
- **Handtraps & Staples (14 Cards)**:
  - 1x *Maxx "C"* (23434538)
  - 3x *Mulcharmy Fuwalos* (42141493)
  - 2x *Ash Blossom & Joyous Spring* (14558128)
  - 2x *Ghost Ogre & Snow Rabbit* (59438931)
  - 1x *Nibiru, the Primal Being* (27204313)
  - 1x *Triple Tactics Talent* (25311006)
  - 2x *Infinite Impermanence* (10045474)
  - 1x *Solemn Judgment* (41420027)
  - 1x *Elfnotes: Rhapsodia of Madness* (24092792)

### Extra Deck (15 Cards)
- 1x *Artmage Diactorus* (27184601) - 2800 ATK Omni-Negate cheat-out target
- 1x *Elfnote June Pride* (5559570) - Lv 10 Center-Zone Synchro boss
- 1x *Baronne de Fleur* (84815190) - Lv 10 Omni-Negate & targeted pop
- 1x *Chaos Angel* (24221808) - Lv 10 Banish on summon & protection
- 1x *Ravenous Crocodragon Archethys* (91588074) - Lv 9 Draw & quick pop
- 1x *Crystal Wing Synchro Dragon* (50954680) - Lv 8 Monster effect negate & destroy
- 1x *PSY-Framelord Omega* (74586817) - Lv 8 Hand-rip & graveyard loop
- 1x *Elfnote Seraphim Strelitzia* (42302563) - Lv 7 Center-Zone revive
- 1x *F.A. Dawn Dragster* (38667473) - Lv 7 Spell/Trap negate
- 1x *Black Rose Dragon* (73580471) - Lv 7 Board wipe
- 1x *Accel Synchron* (20932152) - Lv 5 Tuner modulator
- 1x *Medicurius the Power Patron of Illusions* (99131976) - Link-3 Mass field banish
- 1x *S:P Little Knight* (29301450) - Link-2 Target banish & quick dodge
- 1x *Purification Power Patron* (23571046) - Link-2 Column lock & revive

---

## 3. Architecture Implementation

### A. Decoupled Domain Plugin (`ElfnotePlugin.cs`)
- **`ElfnoteStrategy : IDeckStrategy`**:
  - Implements `PickSpecialSummonTarget`: When *Elfnote Regina* activates its on-summon effect, dynamically checks if a Tuner is present. If not, prioritizes `ElfnotePowerPatron` (Lv 1 Tuner) to immediately guarantee a Lv 10 Synchro play (*June Pride* / *Baronne*).
  - Implements `PickSearchTarget`: Context-aware priority for *Terminus* (*Medius the Pure* > *Theorealize Medius* > *Junordo*), and Ars Magna loops.
- **`ElfnoteMaterialEvaluator : IDeckMaterialEvaluator`**:
  - Scores Center-Zone boss monsters (*Artmage Diactorus*, *June Pride*, *Baronne*, *Crystal Wing*, *Medicurius*) with score 900+ (do not use as material unless strictly required).
  - Scores recursive fodder (*Jet Synchron*, *Medius the Pure*, used *Regina*) with score 10-50 for link/synchro consumption.
- **`ElfnoteThreatEvaluator : IDeckThreatEvaluator`**:
  - Prioritizes opponent continuous floodgates (*Skill Drain*, *There Can Be Only One*, *Macro Cosmos*) and high-ATK bosses.

### B. Rule-Based ModernExecutor (`ElfnoteExecutor.cs`)
- **Center-Zone Placement Guard (`OnSelectPlace`)**:
  - Enforces `1 << 2` (Zone 2) for *ElfnoteLucina*, *ElfnoteRegina*, *ElfnoteTinia*, *ElfnoteFortuna*, *ElfnoteJunePride*, *ElfnoteSeraphimStrelitzia*.
  - Masks out Zone 2 (`available & ~(1 << 2)`) for non-Elfnote fodder to keep the center slot reserved.
- **Cheat-Out Interceptor (`OnSelectOption`)**:
  - Intercepts *Theorealize Medius* effect when face-up cards are banished to automatically choose Option 1 (`Stringid 0`), summoning *Artmage Diactorus* directly from the Extra Deck.
- **Tribute Guard**:
  - Explicitly returns `false` for `LucinaNormalSummon`, `ReginaNormalSummon`, `TiniaNormalSummon`, and `FortunaNormalSummon` to prevent the AI from tributing its own board monsters.
- **Link Climax Guard**:
  - Configured `LinkPurification` to require `Bot.GetMonsterCount() >= 3 && !HasAceOnBoard()` so that 2-material boards are reserved for Synchro climbing into Lv 10/8 bosses.

---

## 4. Build & Deployment Verification

1. **Compilation Pipeline**:
   - Project: `windbot-fork`, `dashbot`, `core`, `Client_Headless_Fortest`
   - Command: `powershell -ExecutionPolicy Bypass -File .\BUILD_AND_DEPLOY.ps1`
   - Result: `Build succeeded. 0 Error(s).`
2. **Deployed Artifacts**:
   - `C:\Users\admin\Documents\EdoGame\WindBot\WindBot.dll`
   - `C:\Users\admin\Documents\EdoGame\WindBot\ExecutorBase.dll`
   - `C:\Users\admin\Documents\EdoGame\WindBot\core.dll`
   - `C:\Users\admin\Documents\EdoGame\WindBot\bots.json`
   - `C:\Users\admin\Documents\EdoGame\deck\Elfnote.ydk`
   - `C:\Users\admin\Documents\EdoGame\deck\ElfnotePowerPatron.ydk`
   - `C:\Users\admin\Documents\EdoGame\WindBot\Decks\Elfnote.ydk`
   - `C:\Users\admin\Documents\EdoGame\expansions\release-betb.cdb`
   - `C:\Users\admin\Documents\EdoGame\cards.cdb`
   - `C:\Users\admin\Documents\EdoGame\DashBot.exe`
