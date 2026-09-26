# Progress Log: Central Core Architecture & Universal Heuristics Overhaul

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
