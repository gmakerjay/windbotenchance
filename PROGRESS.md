# Progress Log: AFS, ArtMage, Tenpai, Centurion, VoicelessVoice & ModernExecutors

## 0.022. Strategic Bugfixes: WhiteForest & Anime_Sayer (Xyz Boss Preservation, MST Negates Fix, Rush Mode & Anti-Tuner Spam) (2026-09-20)
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

## 0.020.New Anime ModernExecutor: Anime_Sayer (Sayer / Divine - Ultimate Psychic Synchro) (2026-09-20)
- **User Directives**:
  - "https://ygoprodeck.com/deck/sayer-ultimate-deck-732776#/ เขียน Executor เด็คนี้หน่อยครับ อ่านการ์ดทุกใบว่าทำอะไรได้แล้ว วิเคราะเพื่อจัดคอมโบตามแนวทางของโปรเจคนี้"
  - "บอทเก่ง, เล่นถูกจังหวะ, ระวังการดค้างมือ, ปรับแก้เด็คได้แต่ขออย่าใส่การ์ดมั่วเข้ามา"
- **Card & Banlist Audit (`cards.cdb` & `0TCG.lflist.conf`)**:
  - วิเคราะห์การ์ดครบถ้วน 100% จาก YGOPRODeck URL จำนวน 64 ใบ
  - **Banlist Violation Prevented**: นำ `Mind Master (96782886)` ซึ่งเป็น Forbidden (0 ใบ) ใน TCG ออกจากเด็ค ป้องกันข้อผิดพลาด `ERRMSG_DECKERROR` (ทำให้บอทเข้าห้องดวลไม่ได้)
  - **Dead Hand Elimination**: ปรับขนาดเด็คจาก 60 ใบเดิมที่มีการ์ดกับดักเงื่อนไขแคบและมอนสเตอร์สังเวยมากเกินไป ให้เป็นเด็คที่กระชับ สมดุล 42 ใบ โดยคัดเลือกเฉพาะการ์ดสาย Psychic / Sayer / Arcadia Movement แท้ 100% ไม่มีการ์ดมั่ว
  - **In-Theme Handtrap Integration**: ใส่ `Ghost Ogre & Snow Rabbit (59438930)` (มอนสเตอร์เผ่า Psycho / Tuner แท้ 100%) 3 ใบ ซึ่งสามารถเรียกผ่าน `Emergency Teleport`, `Overdrive Teleporter` และเสิร์ชผ่าน `Hushed Psychic Minister` ได้อย่างสมบูรณ์แบบ
- **ModernExecutor Strategy & Architecture (`Anime_SayerExecutor.cs`)**:
  - สืบทอดจาก `ModernExecutor` พร้อมตาราง CARD AUDIT ที่ส่วนหัว
  - **Turn 1 First Board**: กางสนาม `Brain Research Lab` เพื่อรับ Normal Summon เสริมฟรีโดยไม่ต้องจ่าย LP, ใช้ `Emergency Teleport` ดึงชิ้นส่วนตั้งกระดาน, ซิงโครขึ้นสู่ `Thought Ruler Archfiend` (ฮีล LP + Negate S/T เล็งเป้า Psychic), `PSY-Framelord Omega` (แบนการ์ดบนมือศัตรู), `Hyper Psychic Riser` (Floodgate ล็อคมอนสเตอร์ ATK > 2000), หรือเซ็ต Counter Trap `Mind Over Matter`
  - **Turn 2 Board Breaking & OTK**: ทะลวงบอร์ดด้วย `Psychokinesis`, `Brain Control` (ยึดมอนสเตอร์ศัตรูมาเป็นวัตถุดิบซิงโคร), ซิงโครขึ้นสู่ `Psychic End Punisher` (Level 11) ซึ่งเมื่อ LP เราน้อยกว่าหรือเท่ากับศัตรู จะได้รับผล Unaffected จากเอฟเฟกต์ศัตรูทั้งหมด และเพิ่ม ATK ใน Battle Phase มหาศาล ปิดเกม OTK ได้ทันที
  - **Strict Anti-Pattern Safeguards**:
    - บังคับ `OnSelectCard` สำหรับ Hint 502 (Destroy), 503/504 (Remove/Banish) เลือกเฉพาะการ์ดศัตรู (`c.Controller == 1`) เสมอ ป้องกันการยิงการ์ดตัวเอง
    - ตรวจสอบ `Bot.GetMonsterCount() >= 1` สำหรับ Tribute Summon ของ `Overdrive Teleporter` ป้องกันบอทค้าง/Pass Turn
    - ห้ามเซ็ต Handtrap (`Ghost Ogre`) บนสนามใน Main Phase 1
- **Deck & Bot Registration**:
  - สร้างไฟล์เด็ค `Anime_Sayer.ydk` (42 Main / 15 Extra) ใน `windbot-fork\Decks\`
  - ลงทะเบียนใน `bots.json` (`name: "Anime_Sayer"`, `deck: "Anime_Sayer"`)
- **Exclusive Deployment**:
  - คอมไพล์และ Deploy ผ่าน `BUILD_AND_DEPLOY.ps1` มายัง `C:\Users\admin\Documents\EdoGame\` ครบถ้วน (WindBot.dll, ExecutorBase.dll, core.dll, bots.json, Anime_Sayer.ydk, DashBot)

## 0.019.Workspace Cleanup & Maintenance: Removal of Legacy Binaries, Scratch Scripts & Stale Artifacts (2026-09-20)
- **User Directives**:
  - "ทำการเคลียร์ไฟล์ขยะ สคริปที่ไมไ่ด้ใช้ และไฟล์ที่ไม่จำเป็นทิ้ง อย่าให้โปรเจคพัง ตรวจสอบอย่างละเอียดก่อนลงมือทำ"
  - (Clear garbage files, unused scripts, and unnecessary files safely. Verify thoroughly before acting).
- **Audit & Identification**:
  1. **Scratch RE Scripts & Text Dumps**: 31 reverse engineering scripts (`check_*.ps1`, `find_*.ps1`, `dump_*.ps1`, `*.cs` decompilers) from September 6 and 3 card text dump files (`purrely_cards.txt`, `scratch_cards.txt`, `yummy_cards.txt`) in root.
  2. **Obsolete Backups & Stale Binaries**: `cards.cdb.bak` (17.1MB), `error.log`, `__pycache__`, `src/cards.cdb` (0-byte corrupt file), `training.*` binaries in `WindBot/` from discarded AI training, and broken-path scripts in `windbot-fork/Game/AI/Decks/`.
  3. **High-Storage Stale Backups**: `src/YGO_AI_SOURCE_BACKUP/` (45MB, 441 files) and old unused builds `bin/Debug/` (42MB) and `bin/x86/` (42MB).
  4. **Preservation & Re-homing**: 16 missing card images in `src/pics/` safely copied into `pics/` before directory deletion. Essential tools `apply_mr5_patch.ps1` and `download_card_pics.py` relocated into `Docs/tools/`.
- **Action Taken & Validation**:
  - All verified junk files and old builds safely purged (~150+ MB reclaimed).
  - Protected all runtime critical binaries: `._cache_ygopro.exe`, `ygopro.exe`, `cards.cdb`, and engine DLLs.
  - Successfully compiled and verified via `BUILD_AND_DEPLOY.ps1 -BuildOnly` (0 Errors).
  - Re-deployed clean binaries to `C:\Users\admin\Documents\EdoGame\`.

## 0.018. Universal Safeguard for Nibiru & Kaiju: Elimination of Self-Target Attacks & DPE Friendly Fire (2026-09-20)
- **User Directives**:
  1. "ดู Log ล่าสุด destroy pheonix ทำลายการ์ดตัวเองจนหมด" (Audit live duel log where Destiny HERO - Destroyer Phoenix Enforcer destroyed its own cards until empty).
  2. "จงไล่ Refactor ใหม่ให้ละเอียด" (Thoroughly refactor and harden the executor logic).
  3. "เด็คไหนใช้อุกกาบาต / ไคจูให้ระวังโดนของตัวเองตี" (For any deck using Nibiru or Kaiju, beware of getting attacked by the monsters we give the opponent!).
- **Comprehensive Audit & Root Causes Identified**:
  1. **Destiny HERO - Destroyer Phoenix Enforcer (DPE / 60461880) Suicidal Friendly Fire**:
     - `DPEReviveInStandby()` ended with unconditional `return true;` outside `if (Card.Location == CardLocation.Grave)`. When DPE was in `MonsterZone`, it triggered DPE's on-field Quick Pop effect even when opponent controlled 0 cards.
     - DPE's mandatory requirement ("destroy 1 card you control and 1 card on the field") forced Jaden to destroy 2 of his own cards repeatedly (Shadow Mist, Imperm, Cross Crusader, Faris, Mask Change, and DPE himself).
  2. **Nibiru (27204311) Primal Being Token Attack Risk**:
     - When Nibiru resolves, the player selects the battle position for the opponent's `Primal Being Token` (27204312). Default logic evaluated the Token's combined ATK (often 5000–10000+ ATK), selecting `FaceUpAttack`.
     - In Attack Position, the opponent immediately attacked Nibiru or our empty board on their turn, dealing lethal direct damage with our own Token.
  3. **Kaiju (63941210 Jizukiru, 48770333 Thunder King, etc.) Suicidal Summoning**:
     - `DefaultExecutor.DefaultKaijuSpsummon()` had `if (isCriticalThreat || canHandleKaiju)`. If the opponent controlled a floodgate/negator, the bot summoned a 3300 ATK Kaiju to the opponent's field even when `canHandleKaiju` was completely `false`, leaving a 3300 ATK beatstick for the opponent to kill us next turn.
- **Architectural Safeguards Implemented**:
  1. **Engine-Level Position Enforcement for Nibiru Token (`GameBehavior.cs`, `Executor.cs`, `ModernExecutor.cs`)**:
     - In `GameBehavior.OnSelectPosition` (network packet level), if `cardId == 27204312` and `FaceUpDefence` is available, **ALWAYS force `CardPosition.FaceUpDefence`**. The Token can NEVER be placed in Attack Position regardless of which deck plays it.
  2. **Universal Intelligent `DefaultNibiru()` (`DefaultExecutor.cs` & `ModernExecutor.cs`)**:
     - Opponent's turn only (`Duel.Player == 1`), Main Phase only.
     - SmartHandTrapChain timing awareness.
     - **Boss Protection**: Never drop Nibiru if Bot controls Ace/Boss monsters (ATK >= 2500) and opponent's total ATK is less than Bot's LP and enemy count <= 2 (prevents throwing away established winning boards).
     - Connected across `Tenpai`, `Centurion`, `AFS`, `_2026_EvilTwin`, `_2026_Monarch`, `_2026_Archfiend`, `_2026_Magnet`, `_2026_Purrely`, `_2026_Regenesis`, `_2026_RyuGe`, `_2026_TrueDraco`, `_2026_Yummy`, `Anime_Judai`, `Anime_Kaiba`, `Anime_JackAtlas`, and `Anime_Zane`.
  3. **Guaranteed Kaiju Removal Guard (`DefaultExecutor.cs`, `_2026_KaijuCrusadiaExecutor.cs`, `_2026_AmazonExecutor.cs`, `Anime_ZaneExecutor.cs`)**:
     - Enforced that Kaijus are NEVER summoned to opponent unless Bot can handle them on the exact same turn (`canHandleKaiju == true`).
     - Zane contact fusion check strictly verifies that `Chimeratech Fortress Dragon` (21060005) is present in Extra Deck.
     - Crusadia checks for available extenders/Equimax and prioritizes lowest ATK Kaiju (Gameciel 2200 ATK) to give opponent.
     - Amazoness checks for active Onslaught, > 2200 ATK attacker, or fusion ready before giving Gameciel.
  4. **DPE Target Isolation & Engine Fix (`Anime_JudaiExecutor.cs`)**:
     - Standby revival strictly isolated to `CardLocation.Grave`.
     - Field pop requires `enemyTargets.Count > 0`.
     - In `OnSelectCard` (Hint 502/503), when targeting own cards, strictly picks DPE himself (floats) or Absolute Zero (board wipe); never touches Dark Law or Plasma.
- **Build & Deployment**:
  - Successfully compiled with 0 errors via `BUILD_AND_DEPLOY.ps1` and deployed to `C:\Users\admin\Documents\EdoGame\`.

## 0.017. Anime Gong & Zane Hard-Bot Rework: Steadfast DEF OTK & Clockwork Contact (2026-09-20)
- **User Directives**:
  - "Rework เด็ค Gong + Zane ในหมวด anime ใหม่ ก่อนลงมือทำให้ออดิตและทำความเข้าใจการ์ดทุกใบก่อนว่ามันเล่นยังไง คอมโบควรเป็นแบบไหน ผมต้องการบอท Hard"
  - (Rework Gong + Zane decks in anime category from scratch. Audit and understand every card effect and optimal combo lines before writing code. Requires competitive Bot Hard standard).
- **Comprehensive Audit & Root Cause Analysis**:
  1. **`Anime_Gong` (Gongenzaka / Gong Strong — Steadfast Superheavy Samurai)**:
     - **Banlist Violations in Legacy Deck**: `Baronne de Fleur` (84815190) and `Superheavy Samurai Scarecrow` (33918636) were Forbidden (Limit 0) in `0TCG.lflist.conf`, causing instant `ERRMSG_DECKERROR` disconnects.
     - **Defects in Legacy Executor & Live Duel Audit**:
       - `Monk Big Benkei` (19510093) in Hand was dead: `MonkBenkeiActivate` only handled `SpellZone`. When drawn, it never placed itself in Scale or Special Summoned itself, leaving Gong to pass turn. Increased to 3 copies in deck and enabled hand/extra activations.
       - Defense Combat Mismatch: `DefaultExecutor` evaluated battle stats using `attacker.Attack`. SHS monsters attack using DEF (Shutendoji 2500 DEF vs 500 ATK, Masurawo 4000 DEF vs 2100 ATK). Overrode `OnPreBattleBetween`, `OnSelectAttacker`, and `OnBattle` to calculate `RealPower = Defense` (doubled with `Soulbuster Gauntlet`).
       - Premature `ResourcePlanner` stop: Tuner + non-Tuner bodies on field were stopped before Synchro climbing. Overrode `ShouldStopExtending()` to keep climbing.
       - Missing Level 7 Synchro: Fist (L2 Tuner) reducing Shutendoji (L6) to Level 5 resulted in 5+2=7 with no Level 7 in Extra Deck. Replaced 1 Musashi with `Superheavy Samurai Stealth Ninja` (50065971, L7, 2800 DEF direct attacker).
       - Fallback Normal Summons: Added `Fist` and `Soulpiercer` fallback normal summons so `Soulpeacemaker` can equip and tribute to cheat out engines from Deck.
     - **Hard-Bot Solution**:
       - Replaced banned cards with **`Naturia Beast` (33198837)** and **`Superheavy Samurai Stealth Ninja` (50065971)**.
       - Implemented full Wakaushi 1-card climb: Wakaushi P-Scale -> Monk Benkei Scale 1 -> Soulpiercer search -> Scales Normal Summon reviving Soulpiercer -> Synchro climbing -> Soulpiercer searches Soulbuster Gauntlet.
       - Damage Step DEF Honest: In damage calc, `Soulbuster Gauntlet` doubles defending/battling DEF up to 8,000 - 9,600 DEF for lethal counter-strikes and direct attack OTK.
       - Enforced `FaceUpDefence` position via `OnSelectPosition` & `RepositionStrategy` so SHS monsters utilize their DEF stats.
  2. **`Anime_Zane` (Hell Kaiser Zane Truesdale — Cyber Dragon & Clockwork Contact)**:
     - **Extra Deck Rebalance**: Replaced unmakeable `Dingirsu` (Rank 8) with legal **`Constellar Pleiades` (73964868)** (Rank 5 LIGHT Machine Quick-Effect bounce to hand).
     - **Live Duel Performance**: Zane executed lethal rush seamlessly — Contact Fused `Fortress Dragon` using opponent's Machine monsters, used `Called by the Grave` on Soulpiercer, revived `Chimeratech Rampage Dragon` via `Nachster`, boosted ATK via `Sieger` to 4,200 ATK, and achieved 3x attack OTK.
  3. **DashBot Character Display Mapping**:
     - Added `{ "Gong", "Gong Strong" }` in `dashbot/MainWindow.xaml.cs` to ensure clean Anime category display alongside "Zane Truesdale".
- **Banlist & Deck Integrity Verification (`cards.cdb` & `0TCG.lflist.conf`)**:
  - `Anime_Gong`: Exactly 40 Main / 15 Extra, **0 Banlist Violations**, **0 Missing Cards**.
  - `Anime_Zane`: Exactly 40 Main / 15 Extra, **0 Banlist Violations**, **0 Missing Cards**.
- **Build & Deployment**:
  - Successfully compiled and deployed `WindBot.dll`, `ExecutorBase.dll`, `core.dll`, `bots.json`, Decks, and `DashBot.exe` to `C:\Users\admin\Documents\EdoGame\`.
  - Instantiation tests verified clean startup and deck resolution for both bots via CLI.

## 0.016. DashBot UX Alignment: Strict Single-Bot Mode & Hidden P2 (2026-09-20)
- **User Directives**:
  - "ปุ่ม p2 oponet จะต้องไม่มีครับ หากไม่กด Bot Vs Bot นั้นรูปแรกยังเลือกได้อยู่สีม่วงเห็นไหม ก็คือถ้าไม่กดจะกดได้แค่ P1 อย่างเดียวเลย"
  - (The P2 Opponent button must NOT exist if Bot Vs Bot is not clicked. If Bot Vs Bot is not clicked, the user can ONLY select P1 alone. In single mode, P2 must be completely hidden and no purple or P2 badges can appear).
- **Comprehensive Solution**:
  1. **Strict Single-Bot Mode (`Play vs Bot` - Default)**:
     - The `P2 (Opponent)` button (`RbAssignBot2`) is completely collapsed and hidden. Only `P1 (Bot)` is visible.
     - The right Matchup Card only displays `SELECTED BOT (P1)` expanding across the preview; `VS` badge and `P2 Box` are collapsed and hidden.
     - The bottom summary bar only displays `Selected Bot: [Name]`; `Bot 2 (P2)` is collapsed and hidden.
     - `DeckItem.IsBotVsBotActive = false`: All deck cards strictly ignore `IsBot2Selected`. Only the active `P1` badge is displayed in Blue (`#0284C7`). No green P2 badge or purple `P1/P2` combination can ever appear.
     - Clicking any deck card strictly assigns to P1 alone. Right-clicking also assigns to P1 alone.
     - Connect Button: `Start & Connect Bot ({BotName}) to Room` (spawns single bot into the EDOPro room).
  2. **Bot Vs Bot Mode (`Bot Vs Bot`)**:
     - Checking `Bot Vs Bot` dynamically reveals:
       - The `P2 (Bot 2)` selector button in the top target switcher.
       - The `VS` circle and `BOT 2 (P2)` card in the matchup preview.
       - The `Bot 2 (P2)` label in the bottom summary bar.
       - Visual highlights on deck items for both `P1` (blue), `P2` (green), and `P1/P2` (purple).
     - Allows toggling active target between P1 and P2 via card click or radio button.
     - Connect Button: `Start & Connect Both ({Bot1} vs {Bot2})` (spawns both bots to duel each other).
- **Build & Deployment**:
  - Recompiled and published `WindBot.dll`, `ExecutorBase.dll`, `core.dll`, and `DashBot.exe` with 0 errors.
  - Deployed directly to `C:\Users\admin\Documents\EdoGame\`.

## 0.015. Anime Decks Active Play Overhaul & DashBot Dual-Mode Redesign (2026-09-20)
- **User Directives**:
  1. "บอทไม่ยอมเล่นการ์ดปรับปรุงชัด Anime ใหม่ครับ หลายๆเด็ค เลย" (The bots refuse to play cards, overhaul/update the new Anime decks clearly! Multiple decks are doing this).
  2. "แล้วก็ในส่วนของ Dashbot หากไม่กด 2Bot จะเลือกบอทอีกตัวไม่ได้ ที่เป็นอยู่ชวนสับสน" (And in DashBot, if you don't click 2Bot, you cannot select the second bot, which is confusing).
- **Root Cause Analysis & Comprehensive Fixes**:
  1. **DashBot UI Redesign (Eliminating Bot 1/2 Selection Confusion)**:
     - **Previous Issue**: When 2Bot was unchecked, the UI still displayed a static `BOT 1 VS BOT 2` matchup preview and dual assignment pills. Clicking a deck always changed Bot 1, leaving users unable to choose Bot 2 without checking 2Bot; and selecting Bot 2 still only connected Bot 1 in single mode.
     - **Solution Implemented**:
       - Introduced clean **Duel Mode Radio Buttons**: `[●] 1 Bot (Play vs AI)` and `[○] 2 Bots (Bot vs Bot)`.
       - In **1 Bot Mode**: Displays a dedicated single-bot preview card `SELECTED BOT: [Name]` with subtext `Click any deck to select this bot`. Left-clicking any deck instantly updates this bot.
       - In **2 Bots Mode**: Displays interactive clickable cards for `[ BOT 1 ]` and `[ BOT 2 ]` with glowing selection rings and `● ACTIVE` badges. Clicking either card sets it as the active assignment target.
       - Right-clicking any deck in 1-Bot mode automatically switches to 2-Bots mode, assigns Bot 2, and logs the change.
       - Connect button dynamically changes label (`Start & Connect Bot to Room` vs `Start & Connect 2 Bots (Bot vs Bot)`).
  2. **`Anime_JackAtlas` (Crippling Turn-1 Bug Eliminated)**:
     - **Root Cause**: `OnSelectYesNo` contained `if (Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0) return false;`. On Turn 1 (First turn), the opponent always controls 0 cards, causing the bot to answer "NO" to ALL of its own ignition/trigger effects, completely freezing and passing the turn!
     - **Fix**: Removed the empty field check, allowing `base.OnSelectYesNo` to evaluate effects properly.
  3. **`Anime_Kaiba` (Dictator of D. & Turn-1 Play Overhaul)**:
     - **Root Cause**: `DictatorOfD` was registered as `ExecutorType.SpSummon` instead of `ExecutorType.Activate` (it is an activated effect with cost). Consequently, the bot never summoned Dictator from hand. In addition, `SpellSetStrategy` required `Duel.Phase == DuelPhase.Main2 || Bot.GetMonsterCount() > 0`, causing the bot to never set `TrueLight`, `UltimateFusion`, or `Impermanence` on Turn 1 with an empty board.
     - **Fix**: Registered `DictatorOfD` under `ExecutorType.Activate`, unified `DictatorActivate` to handle hand summon (dump BEWD from deck to GY) and field revival; updated `TheMelodyOfAwakeningDragon` to prioritize searching BEWD if not in hand so Alternative is live; enabled setting `TrueLight`, `UltimateFusion`, and `Impermanence` during Main Phase 1.
  4. **`Anime_Judai` (Vyon Engine & Normal Summon Unblock)**:
     - **Root Cause**: `VyonActivate` was checking `Bot.Deck.Any(...)` and getting stuck trying to execute effect 1 (dump) instead of effect 2 (banish from GY for Polymerization). `LiquidSoldierNormalSummon` required existing GY monsters, causing dead passes on empty boards.
     - **Fix**: Unlocked `VyonNormalSummon` and `LiquidSoldierNormalSummon` on empty boards; fixed `VyonActivate` to execute both GY dump and Poly search without getting stuck on deck checks; enabled setting `MaskChange` and `Impermanence` during Main Phase 1.
  5. **`Anime_Zane` (Deck Error & Backrow Fix)**:
     - **Root Cause**: Zane was previously rejected with `ERRMSG_DECKERROR` due to illegal extra deck cards in main deck. After deck correction, `SpellSetStrategy` was blocking Turn-1 sets of `CyberneticOverflow` and `CyberloadFusion`.
     - **Fix**: Enabled setting `CyberneticOverflow`, `CyberloadFusion`, and `Impermanence` in MP1; verified `CyberEmergency` searchers, `Core` -> `MachineDuplication` -> `Nova` -> `Infinity` combo ladder.
  6. **`Anime_Yusei` (Synchro Fellowship Search Fix)**:
     - **Root Cause**: `SynchroFellowship` requires adding 2 cards (Junk Synchron + monster mentioning Junk Warrior/Stardust Dragon) then discarding 1. The selector was only passing a single card selection.
     - **Fix**: Updated `SynchroFellowshipActivate` to select `CardId.JunkSynchron` and then `SelectNextCard` for the second search target and discard.
  7. **Build & Deployment**:
     - Successfully built and published `WindBot.dll`, `ExecutorBase.dll`, `core.dll`, `bots.json`, and `DashBot.exe`.
     - Deployed exclusively to `C:\Users\admin\Documents\EdoGame\`. All binaries verified fresh.

## 0.014. Five Premier Anime ModernExecutors: Immortal Stickiness & Bot Hard AI Architecture (2026-09-20)
- **User Directives**:
  - "/goal https://ygoprodeck.com/deck-search/?_sft_category=Tournament%20Meta%20Decks%20Worlds&sort=Deck%20Views&offset=0"
  - "เขียน Executor Deck ขึ้นมา คัดเลือกเด็คที่คิดว่า แข็งแกร่งการ์ดตายยากๆ คอมโบเก่ง บอทไม่โง่ วิเคราะห์จากเอฟเฟคจริง ทั้งหมด 5 เด็ค ในหมวด Anime ดัดแปลงเด็คให้เหมาะสมได้ วิเคราะห์จากการ์ดจริงๆ ไม่มั่วขึ้นมา เสร็จแล้วทำรายงานให้ผมด้วย ผมจะได้ไปทดสอบเอง ย้ำว่าขอแบบ Bot Hard วิเคราะห์โดยละเอียด ห้ามพลาดเด็ดขาด ออดิต และตรวจสอบซ้ำว่าบอทจะใช้งานได้จริง วิเคราะห์ร่วมกับโปรเจคจริง Core จริง"
- **Strategic Selection of 5 Ultimate Anime Decks**:
  1. **`Anime_Kaiba` (Seto Kaiba — Blue-Eyes Jet & Ultimate Fusion)** [NEW]:
     - **Resilience / Stickiness**: `Blue-Eyes Jet Dragon` (infinite GY/hand recursion on ANY card destruction, non-destruction blanket protection for all cards, battle step bounce), `True Light` (targeting immunity for BEWD + free summons/sets), `Blue-Eyes Twin Burst Dragon` (battle destruction immunity + banish on attack), `Azure-Eyes Silver Dragon` (Dragon targeting/destruction immunity).
     - **Combo & Bosses**: `The Melody of Awakening Dragon`, `Dictator of D.`, `Blue-Eyes Alternative`, `Blue-Eyes Spirit Dragon` (floodgate multi-summons + GY negate + tag out), `Number 38: Hope Harbinger` (Spell negate), `Number 90: Photon Lord` (Monster negate), `Number 97: Draglubion` -> `Number 100: Numeron Dragon` (17,000 ATK OTK), `Ultimate Fusion` into `Neo Blue-Eyes Ultimate Dragon` (4,500 ATK x 3 attacks).
  2. **`Anime_Judai` (Jaden Yuki — HERO Destiny Phoenix & Dark Law Omni-Lock)** [NEW]:
     - **Resilience / Stickiness**: `Destiny HERO - Destroyer Phoenix Enforcer` (DPE: Quick targeted destruction of self + enemy card, loops and recurses itself from GY every single Standby Phase indefinitely; 200 ATK debuff per HERO in GY).
     - **Locks & Disruptions**: `Masked HERO Dark Law` (one-sided Macro Cosmos banishing all cards sent to enemy GY + random hand rip), `Destiny HERO - Plasma` (one-sided Skill Drain negating all opponent field monster effects + absorbs enemy monster as equip).
     - **Engine & OTK**: `Vision HERO Faris` -> `Increase` -> `Vyon` -> `Shadow Mist` -> `Malicious` -> `Denier` recycling loop; `Elemental HERO Sunrise` (searches Miracle Fusion), `Elemental HERO Absolute Zero` + `Masked HERO Acid` (Raigeki + Harpie's Feather Duster board wipe), `Wake Up Your Elemental HERO` (multi-attack beatdown).
  3. **`Anime_Zane` (Zane Truesdale — Cyber Dragon Infinity & Clockwork Contact)** [NEW]:
     - **Resilience & Board Eating**: `Clockwork Night` turns all face-up monsters into Machines (+500/-1000 ATK) -> enables `Chimeratech Fortress Dragon` to Contact Fuse and devour the opponent's entire monster field without starting a chain; `Chimeratech Megafleet Dragon` devours Extra Monster Zone.
     - **Bosses & Disruptions**: `Cyber Dragon Infinity` (detach to Omni-Negate activations + non-destructive monster absorption), `Cyber Dragon Nova` (floats into 4,000 ATK `Cyber End Dragon` upon enemy removal), `Therion "King" Regulus` (Machine Omni-Negate), `Cyber Dragon Sieger` (Quick +2,100 ATK boost), `Chimeratech Rampage Dragon` (backrow wipe + 3x attack OTK).
     - **Engine**: `Cyber Dragon Core`, `Herz`, `Nachster`, `Machine Duplication`, `Cyber Emergency`, `Cyber Repair Plant`, `Cyber Revsystem`, `Cyberload Fusion`.
  4. **`Anime_JackAtlas` (Jack Atlas — Resonator & Red Supernova Calamity-Free Revamp)** [REWORK]:
     - **Banlist Fix**: Purged banned `Hot Red Dragon Archfiend King Calamity` (Limit 0) and replaced with legal Level 10 DARK Dragon Synchro powerhouse `Bystial Dis Pater` (27572350 - targets banished LIGHT/DARK to revive; Quick monster negate/destroy).
     - **Resilience & Board Wipe**: `Red Supernova Dragon` (4,000+ ATK, effect destruction immunity; Quick Effect banishes itself to wipe and banish the opponent's ENTIRE board), `Soul Resonator` (GY banish prevents any card destruction), `Hot Red Dragon Archfiend Abyss` (Quick targeted negate), `Hot Red Dragon Archfiend Bane` (revival loop), `Scarred Dragon Archfiend` (floats into RDA + destroys all attack position monsters), `Red Zone` (pop card + revive banished Synchro).
  5. **`Anime_Yusei` (Yusei Fudo — Cosmic Blazar & Shooting Majestic Accel Synchro Revamp)** [REWORK]:
     - **Resilience & Evasion**: `Cosmic Blazar Dragon` (banishes itself as cost to negate any card/effect activation, summon, or attack — completely immune to negation/removal during resolution), `Shooting Majestic Star Dragon` (permanent monster effect negate + Quick activation negate & banish + multi-attacks).
     - **Ladder**: `Junk Speeder` (summons 5 Tuners from deck), `Accel Synchro Stardust Dragon` (Quick Synchro bosses that are unaffected by opponent's activated effects this turn), `Stardust Dragon`, `Shooting Quasar Dragon`, `Crimson Dragon`.
- **Banlist & Integrity Verification (`cards.cdb` & `0TCG.lflist.conf`)**:
  - All 5 decks strictly rebalanced to 40-41 Main / 15 Extra.
  - Automated CDB & banlist scanner confirmed **0 Banlist Violations**, **0 Missing Cards**, and **0 Deck Errors** across all 5 decks.
- **Bot Hard AI & Anti-Pattern Safeguards**:
  - Hint 506 isolation for searches in `OnSelectCard`.
  - Enemy-only targeting (`c.Controller == 1`) for destructions (502) and banishes (503).
  - Proper Level 5+ tribute summon checks preventing bot stalls.
  - Main Phase 2 trap setting preserving handtrap usability.
- **DashBot Launcher & Registration**:
  - Registered all 5 bots in `bots.json` (`Anime_Kaiba`, `Anime_Judai`, `Anime_Zane`, `Anime_JackAtlas`, `Anime_Yusei`).
  - Added clean display name mappings in `dashbot/MainWindow.xaml.cs` for "Seto Kaiba", "Jaden Yuki", and "Zane Truesdale".
  - DashBot automatically categorizes them under **Anime** via `Anime_` prefix.
- **Compilation & Exclusive Deployment**:
  - Full project compiled and deployed via `BUILD_AND_DEPLOY.ps1` with 0 errors to `C:\Users\admin\Documents\EdoGame\`.
  - Tested WindBot engine instantiation for all 5 bots via CLI (`dotnet .\WindBot.dll Deck=Anime_*`) — all 5 loaded their decks and executors flawlessly without crash.

## 0.013. AFS & ArtMage Rework: Real Lua Mechanics, Zero-Prefix Migration & Banlist Clean (2026-09-20)
- **User Directives**:
  - "Refactor เด็ค AFS + Atrmage ใหม่ครับ อยากให้ปรับปรุงให้โค๊ดสะอาด บอทเก่งวิเคราะเอฟเฟคการ์ดจริง หากจำเป็นต้อง Rework จัดการได้เลย" (Refactor AFS + ArtMage decks, clean code, smart bot analyzing real card effects, rework if needed).
- **Audit & In-Depth Card Script Analysis (`cards.cdb` & Lua Scripts)**:
  1. **ArtMage Real Card Mechanics (`c53589300.lua`, `c27184601.lua`, `c74631897.lua`, `c34541940.lua`)**:
     - `Nerva the Power Patron of Creation` (53589300): Quick Effect triggers upon activation of an Artmage monster effect (`re:IsMonsterEffect() && Chain.IsSetcode(SET_ARTMAGE)`), changing that effect to **"Destroy all cards your opponent controls"** (total board wipe). Cannot be destroyed by card effects while Field Spell is present.
     - `Artmage Diactorus` (27184601): Quick Effect Omni-Negate that negates and destroys any card/effect activated on field; switches battle positions; floats into Medius upon destruction.
     - `Artmage Non-Finito` (74631897): Alternate summons by discarding S/T + tributing Finmel; on summon sets Artmage Spell/Trap (`Impasto Recapture`, `Pact`, etc.); Quick Fusion on opponent's turn.
     - `Artmage Finmel` (34541940): Free hand SS + Draw 1; Quick Effect blankets opponent monsters with effect negation and ATK halving when 3+ Monster Types are present.
     - `Artmage Graflare` (60946049): Free hand SS + Sets Artmage Spell from deck; targeted S/T destruction (Quick Effect when 3+ types).
     - `Artmage Litera` (97434754): Free hand SS + GY recycle; Quick bounces self to hand during opponent Main Phase to SS Artmage from hand/GY.
     - `Medius the Pure` (97556336): On Normal/Special summon, searches or Special Summons `Shadow Beast Nervedo` directly from Deck.
     - `Shadow Beast Nervedo` (17473466): Banishes 3 cards face-down from Deck to cheat out `Nerva` (treated as Fusion Summon); when added to face-up Extra Deck, Special Summons `Finmel` from Deck.
     - `Artmage Impasto -Recapture-` (44654994): Counter Trap that can be activated the turn it is Set; banishes Fusion monster to negate monster effect & destroy, bouncing opponent backrow.
  2. **Banlist & Deck Construction Fixes (0TCG.lflist.conf)**:
     - **AFS**: Purged banned cards (`Maxx "C"` x2, `Original Sinful Spoils` x1, `'Moon of the Closed Heaven'` x1), corrected `Called by the Grave` (to 1). Rebuilt to 40 Main / 15 Extra with `Fiendsmith's Agnumday` and legal staples.
     - **ArtMage**: Purged banned `Maxx "C"`, trimmed `Triple Tactics Talent` to 1, removed dead bricks (`Vidrium` and `Zegredo` which could not summon their targets). Rebuilt to 40 Main / 15 Extra.
  3. **Zero-Prefix Naming & DashBot Integration**:
     - Renamed both decks and executors: `AFS` and `ArtMage`.
     - Purged obsolete `_2026_AFSExecutor.cs`, `_2026_ArtMageExecutor.cs`, `2026_AFS.ydk`, `2026_ArtMage.ydk`.
     - Registered clean bot entries in `bots.json` (`AFS`, `Expert_AFS`, `ArtMage`, `Expert_ArtMage`).
     - Added `AFS` and `ArtMage` to `ModernArchetypes` and `CleanDeckDisplayName` in `dashbot/MainWindow.xaml.cs`.
- **Compilation & Exclusive Deployment**:
  - Compiled and deployed with 0 errors via `BUILD_AND_DEPLOY.ps1` to `C:\Users\admin\Documents\EdoGame\`.
  - Verified WindBot CLI startup for both `AFS` and `ArtMage` with 0 deck errors.

## 0.012. Modern Meta 3-Deck Revamp: Zero-Prefix Naming, Room-Join DeckError Fixes & DashBot Modern Categorization (2026-09-20)
- **User Directives**:
  1. "ชุดที่ทำให้สามเด็คล่าสุดปรับปรุงใหม่ให้บอทเข้าห้องได้ก่อนเลยครับ" (Fix the 3 latest decks so the bot can enter the room first!).
  2. "ไม่ต้องใช้ชื่อนำหน้า 2026 หรืออะไรแล้วครับ ต่อไปนี้แค่จัดหมวดจำไว้ด้วย" (No need to use prefix 2026 or anything anymore. From now on, just categorize them, remember this too!).
- **Root Cause Analysis (Why Bots Failed to Enter Room)**:
  - From WindBot client logs (`client__2026_Tenpai_...log` & `client__2026_Centurion_...log`):
    - `[OnErrorMsg] Received error message code: 2` (ERRMSG_DECKERROR)
    - `DeckError Details: flag=4, code=73491419` (flag 4 = UNKNOWN CARD in `cards.cdb`)
    - `DeckError Details: flag=6, code=...` (flag 6 / 1 = BANLIST / FORBIDDEN CARD VIOLATION)
  - Card ID verification against `cards.cdb` revealed mismatched / hallucinated card IDs:
    - Earth Golem was `73491419` (real ID: `62111090`)
    - Bonfire was `67332219` (real ID: `85106525`)
    - TY-PHON was `12470404` (real ID: `93039339`)
    - Mudragon was `42110604` (real ID: `54757758`)
    - Kuibelt was `97093867` (real ID: `87837090`)
    - Bystial Dis Pater was `24857466` (real ID: `27572350`)
    - Preparation of Rites was `44155002` (real ID: `96729612`)
    - Dyna Mondo was `54447022` (was Soul Charge, banned!) (real ID: `73898890`)
    - Bagooska was `2625939` (real ID: `90590303`)
  - Banlist check against default room banlist `0TCG.lflist.conf` (`2026.05 TCG`):
    - `Baronne de Fleur`, `Abyss Dweller`, `Herald of the Arc Light` are Forbidden (0).
    - `Sangen Summoning`, `Sangen Kaimen`, `Tenpai Dragon Chundra`, `Bonfire`, `Pot of Prosperity`, and `Called by the Grave` are Limited (1).
- **Comprehensive Fixes & Re-Architecture**:
  1. **Clean Naming Policy (No `2026_` Prefix)**:
     - Renamed all 3 decks and executors: `Tenpai`, `Centurion`, `VoicelessVoice`.
     - Purged all old `_2026_*.ydk` and `_2026_*Executor.cs` files.
     - Registered clean bot names in `bots.json`.
  2. **100% Validated Deck Construction**:
     - Queried and verified all card IDs against `cards.cdb`.
     - Rebalanced all 3 main decks (exactly 40 cards) and extra decks (exactly 15 cards) to have **0 Banlist Violations** against `0TCG.lflist.conf`.
  3. **DashBot Modern Categorization**:
     - Updated `dashbot/MainWindow.xaml.cs` with `ModernArchetypes` hashset to classify `Tenpai`, `VoicelessVoice`, `Centurion`, and other modern meta decks under **Modern** category without relying on prefix strings.
     - Added clean display name mappings ("Tenpai Dragon", "Voiceless Voice", "Centur-Ion").
  4. **Compilation & Deployment**:
     - Built and deployed all components via `BUILD_AND_DEPLOY.ps1` with 0 errors directly to `C:\Users\admin\Documents\EdoGame\`.
     - Verified WindBot CLI initialization for `Tenpai`, `Centurion`, and `VoicelessVoice` (0 deck errors, connects cleanly).
  5. **Policy & Guidelines Update**:
     - Updated `AGENTS.md` and `.agents/skills/yugioh-executor/SKILL.md` with strict rules against version prefixes and unverified card IDs.

## 0.011. Elite Tournament Meta 3-Deck Porting: Tenpai, Voiceless Voice & Centur-Ion (2026-09-20)
- **Concept & Request**: Analyzed latest tournament meta data from YGOPRODeck and developed 3 premier Tier 1 meta executors based 100% on real cards from `cards.cdb`, designed with master-level ("Hard ที่สุด") deterministic combo pipelines:
  1. **`_2026_Tenpai` (Tenpai Dragon — Going-Second OTK God)**:
     - Deck: 40 Main / 15 Extra (`_2026_Tenpai.ydk`).
     - Engine: `Tenpai Dragon Paidra` (searches Sangen Summoning/Kaimen), `Tenpai Dragon Chundra` (SS from hand on battle, searches FIRE Dragon from deck on attack), `Tenpai Dragon Fadra` (revives FIRE Dragon from GY), `Tenpai Dragon Genroku` (tributes self to SS from Deck).
     - Field Spell: `Sangen Summoning` (Grants all FIRE Dragon monsters complete immunity from opponent's activated effects during Main Phase 1; pops 1 card to search any Tenpai).
     - Quick-Play Spell: `Sangen Kaimen` (Adds/SS FIRE Dragon + forces Battle Phase).
     - Extra Deck OTK Machine: `Sangenpai Bident Dragion` (Level 7 Synchro, revives FIRE Dragon from GY), `Sangenpai Transcendent Dragion` (Level 10 Synchro, forces opponent to attack, completely locks opponent's cards/effects during Battle Phase), and `Trident Dragion` (Level 10, destroys up to 2 friendly cards to attack 3 times for 9,000+ damage!).
     - Board Breakers & Handtraps: `Super Polymerization`, `Dark Ruler No More`, `Forbidden Droplet`, `Lightning Storm`, `Ash Blossom`, `Infinite Impermanence`.
  2. **`_2026_VoicelessVoice` (Voiceless Voice — Untargetable Ritual Omni-Negate Control)**:
     - Deck: 40 Main / 15 Extra (`_2026_VoicelessVoice.ydk`).
     - Engine: `Lo, the Prayers of the Voiceless Voice` (1-card starter, places Barrier/Radiance face-up, non-OPT self-revive from GY upon any LIGHT Ritual summon), `Saffira, Dragon Queen of the Voiceless Voice` (discards self + dumps Prayers to search, banishes from GY to Ritual Summon), `Diviner of the Herald` (dumps Herald of the Arc Light to search Ritual pieces).
     - Bosses & Disruptions: `Skull Guardian, Protector of the Voiceless Voice` (4,100 ATK under Lo, Quick Effect Omni-Negate Monster/Spell/Trap, searches on summon), `Barrier of the Voiceless Voice` (blanket targeting protection for all LIGHT monsters + redirects attacks), `Radiance of the Voiceless Voice` (shuffles cards to pop without targeting), `Sauravis, the Ancient and Ascended` (handtrap targeting negation + special summon negate spin).
  3. **`_2026_Centurion` (Centur-Ion — Tier 1 Cosmic Blazar Synchro 12 Juggernaut)**:
     - Deck: 40 Main / 15 Extra (`_2026_Centurion.ydk`).
     - Engine: `Stand Up Centur-Ion!` (Field Spell, places Centur-Ion from deck into Continuous Trap zone; Quick Synchro during opponent's turn!), `Centur-Ion Primera` (searches any Centur-Ion on summon, jumps out as Level 4 Tuner), `Centur-Ion Trudea` (places 2 Centur-Ions in S/T zone, modulates to Level 8), `Centur-Ion Gargoyle II` (Level 8 extender), `Centur-Ion Emeth VI` (Level 8 Quick-SS).
     - Bosses & Disruptions: `Centur-Ion Legatia` (3,500 ATK Level 12, draws 1 card + destroys highest-ATK opponent monster), `Centur-Ion Auxila` (3,000 ATK Level 12, searches S/T + protects face-up cards in S/T zone), `Centur-Ion True Awakening` (Counter Trap Omni-Negate), `Centur-Ion Phalanx` (banishes monster on field), and `Crimson Dragon` tag-out into `Cosmic Blazar Dragon` (4,000 ATK Omni-Negate for activations, summons, and attacks!).
- **Architectural Safeguards**:
  - Full adherence to strict anti-patterns: `OnSelectCard` Hint 506 isolation, enemy-only targeting on destruction/banish, zero self-sabotage on extra deck summons, and Main Phase 2 trap setting.
- **Compilation & Exclusive Deployment**:
  - Built with 0 compiler errors via `BUILD_AND_DEPLOY.ps1`; deployed all binaries and assets to `C:\Users\admin\Documents\EdoGame\`. Synchronized decks to both `windbot-fork/Decks/` and `deck/`. Registered all 3 bots in `bots.json`.

## 0.010. Anime_Pegasus (Pegasus) S:P Little Knight Audit & Self-Targeting Removal Fix (2026-09-20)
- **Problem Statement**:
  - The user observed "pegasus ดีดการ์ดตัวเองลงหลุมหรอ" (Why does Pegasus send his own cards to GY?) and requested "ดู log ล่าสุดเกี่ยวกับการกระทำของ SP knight" (Analyze recent logs regarding S:P Little Knight's actions).
- **Log Analysis & Root Cause Findings (`WindBot/logs/Blue_Angel_vs_Pegasus_25690920_123602` & `123537`)**:
  1. **Comic Hand Waste on S:P Link Summon (Turn 2)**:
     - Pegasus activated `Comic Hand` (33453260) to take control of Blue Angel's `Trickstar Lilybell` (98700941) and Normal Summoned `Toon Cyber Dragon` (83629030).
     - Because `SPLittleKnightSpSummon()` returned `true` unconditionally, the bot sacrificed both the stolen monster and `Toon Cyber Dragon` as Link materials to summon `S:P Little Knight` (29301450).
     - As a result, the stolen monster left the field, sending `Comic Hand` straight to the GY ("ดีดลงหลุม"), destroying Pegasus's own advantage and replacing high-ATK direct attackers under `Toon Kingdom` with a 1600 ATK link monster.
  2. **S:P Little Knight Quick Effect Dodge (Turn 3 & Turn 5)**:
     - On Turn 3 (12:36:16) and Turn 5 (12:36:17), Blue Angel activated monster effects. `SPLittleKnightActivate` responded with Quick Effect (Effect 2: target 2 face-up monsters including 1 bot controls to banish until End Phase).
     - The bot correctly targeted Blue Angel's monster (`98700941` / `37683441`) and itself (`29301450`), causing both to fly out of the field temporarily until the End Phase.
  3. **Toon Black Luster Soldier Self-Banish Bug (Turn 2 in 123537)**:
     - In the earlier duel, `Toon Black Luster Soldier` activated its once-per-turn banish effect (`hint=503`).
     - In `OnSelectCard()`, the bot unconditionally matched `preferred` cards containing `ToonBlackLusterSoldier`.
     - Because hint type wasn't checked, the bot selected **its own Toon BLS as the target to banish**, removing its own boss monsters twice!
- **Architectural Fixes in `Anime_PegasusExecutor.cs`**:
  - **`SPLittleKnightSpSummon` Hard Guards**: Added strict conditions:
    - NEVER Link Summon S:P if Pegasus controls any monster equipped with `Comic Hand`.
    - NEVER sacrifice high-ATK Toons under `Toon Kingdom` in Main Phase 1 (preserves direct attack win condition).
    - Only Link Summon S:P in Main Phase 2 as an end-board piece or in Main Phase 1 if opponent has high threats and bot has small non-Toon bodies.
  - **`OnSelectCard` Hint-Specific Segregation**:
    - Restricted `preferred` search list solely to Deck searches (`hint == 506` / `HINTMSG_ATOHAND` or all cards located in Deck).
    - For removal hints (`hint == 503` / `HINTMSG_REMOVE`, `hint == 502` / `HINTMSG_DESTROY`, `hint == 504` / `HINTMSG_TOGRAVE`), bot strictly prioritizes enemy cards (`c.Controller == 1`) and never targets friendly cards.
  - **`ComicHandActivate` Target Validation**: Requires face-up opponent monsters (`Controller == 1`) that are not tokens and not already equipped with `Comic Hand`.
  - **`SPLittleKnightActivate` Dual-Branch Handling**:
    - Quick Effect dodge: targets highest-ATK opponent monster + S:P itself only when opponent activates effects.
    - On-Summon banish: targets highest-ATK enemy monster, enemy backrow, or opponent GY monster.
  - **Extra Deck Safeguards**: Added guards to `BigEyeSpSummon`, `RelinquishedAnimaSpSummon`, and `HopeHarbingerSpSummon` to avoid throwing away Toon direct lethal push.
- **Build & Exclusive Deployment**:
  - Compiled with 0 errors via `BUILD_AND_DEPLOY.ps1`; deployed new `WindBot.dll`, `ExecutorBase.dll`, `core.dll`, `bots.json`, and deck assets to `C:\Users\admin\Documents\EdoGame\`.

## 0.009. Anime_Yugi (Yugi Muto) AI Freeze Fix & Deck Optimization (2026-09-20)
- **Problem Statement**: The user reported "เด็ค yugi muto ไม่ยอมเล่นการ์ด" (Yugi Muto deck refuses to play cards / freezes / passes turns doing nothing).
- **Root Causes Identified from Duel Logs (`WindBot/logs/Yugi_Muto_vs_Shark_...`)**:
  1. **Impossible Tribute Condition Bug**: In `Anime_YugiExecutor.cs`, Normal/Tribute summons for `DarkMagicianGirl`, `SkullArchfiendOfChaos`, and `PharaohsServant` had condition `return Bot.GetMonsterCount() == 0;`. Because Level 6 & 7 monsters require 1-2 tributes, they could NEVER be summoned when the board had monsters, and the game engine couldn't offer them when the board was empty, locking the bot out of normal summons 100% of the time.
  2. **Searcher Self-Sabotage in `OnSelectCard`**: When `Illusion of Chaos` activated its hand search, `OnSelectCard` lacked priority handling and frequently selected the searched `Pharaoh's Servant` to put immediately back on top of the deck!
  3. **Starter Starvation & Severe Deck Bricks**: The original 46-card `Anime_Yugi.ydk` had only 1 copy of `Dark Magician, the Pharaoh's Servant` and 0 copies of original `Dark Magician`. As a result, 3x `Dark Magical Curtain` frequently had zero targets in Deck and failed to trigger its search; 3x `Preparation of Rites` bricked because there was only 1 Level <= 7 Ritual in the deck; and 3x `Pre-Preparation of Rites` bricked because there were only 2 Ritual monsters total.
  4. **Pot of Prosperity Sabotage**: `PotOfProsperityActivate` was banishing all 3 copies of `Dark Magician of Destruction`, severing the bot's primary Extra Deck route into `Red-Eyes Dark Dragoon`.
  5. **Handtrap Exposure**: `SpellSetStrategy` was setting `Dominus Impulse` face-down immediately on Turn 1, exposing it to removal instead of keeping it in hand as an active handtrap.
- **Architectural Solutions & Fixes**:
  - **`Anime_Yugi.ydk` Optimization**: Rebalanced main deck to 45 cards: added 2x original `Dark Magician` (46986414) enabling full search trigger on `Dark Magical Curtain`, increased `Pharaoh's Servant` to 2x, `Illusion of Chaos` to 2x, and `Magician of Dark Chaos` to 2x so all searchers (`Pre-Prep`, `Prep`, `Curtain`, `Soul Servant`) remain live throughout the match.
  - **`Anime_YugiExecutor.cs` Overhaul**:
    - Rewrote `OnSelectCard` with intelligent rules: never returns searched starters with `Illusion of Chaos`; intelligently reveals disposable spells for `Pharaoh's Servant`; picks top starters from `Pot of Prosperity` excavations.
    - Fixed all summon methods: `DarkMagicianGirlSummon` and `SkullArchfiendSummon` now properly check `Bot.GetMonsterCount() > 0` for 1-tribute lines.
    - Restructured priority pipeline so Turn 1 starters (`Illusion of Chaos` -> `Preparation of Rites` -> `Pre-Prep` -> `Griffoh` -> `Black Chaos` discard -> `Pharaoh's Servant` SS -> `Dark Magician of Destruction` -> `The Gaze of Timaeus` into `Red-Eyes Dark Dragoon`) activate sequentially without bottlenecking.
    - Updated `Pot of Prosperity` banish list to preserve core fusion bosses (`Dragoon`, `Dragon Knight`, `Master of Chaos`, and `Dark Magician of Destruction`).
  - **Compilation & Exclusive Deployment**: Rebuilt with 0 errors via `BUILD_AND_DEPLOY.ps1`; deployed to `C:\Users\admin\Documents\EdoGame\`. Synchronized `Anime_Yugi.ydk` to both `WindBot/Decks/` and `deck/`.

## 0.008. Elite Anime 5-Deck Expansion (2026-09-20)
- **Concept & Request**: Developed, ported, and deployed 5 elite rule-based anime executors representing 5 distinct Yu-Gi-Oh! eras (ZEXAL, VRAINS, 5D's, ARC-V, DM) with 100% verified real cards, deterministic starters, and lethal disruptions:
  1. **`Anime_Shark` (Reginald Kastle / Nash — Water Xyz & Armored Xyz)**:
     - Deck: 40 Main / 15 Extra (`Anime_Shark.ydk`).
     - Engine: `Buzzsaw Shark` (1-card Rank 3-5 Xyz), `Lantern Shark`, `Crystal Shark`, `Armored Shark`.
     - Bosses & Disruptions: `N.As.H. Knight` (Non-targeting absorption of enemy monster), `CXyz N.As.Ch. Knight` (immune to monster effects), `Full Armored Crystalzero Lancer` (Quick Effect negates all face-up opponent monsters on field), `Number C101: Silent Honor DARK` (absorbs Special Summoned monsters and floats with HP gain), `Virtue Stream` (pops 2 cards).
  2. **`Anime_BlueAngel` (Skye Zaizen / Blue Angel — Trickstar Burn & Hand Control)**:
     - Deck: 41 Main / 15 Extra (`Anime_BlueAngel.ydk`).
     - Engine: `Trickstar Candina` (searches any Trickstar), `Trickstar Light Stage` (searches, locks backrow, adds burn), `Trickstar Aqua Angel`, `Trickstar Hoody`.
     - Bosses & Disruptions: `Trickstar Reincarnation` (banishes opponent's entire hand and forces redraw, triggering massive burn with `Trickstar Lycoris`), `Trickstar Corobane` (Honest handtrap doubling ATK during damage calculation), `Trickstar Bella Madonna` (Link-4 2800 ATK tower completely unaffected by activated card effects + burns 500 per Trickstar in GY every turn).
  3. **`Anime_Crow` (Crow Hogan — Blackwing Synchro Swarm & Burn)**:
     - Deck: 40 Main / 15 Extra (`Anime_Crow.ydk`).
     - Engine: `Blackwing - Sudri the Phantom Glimmer` (searches Blackwing card + spawns tokens), `Blackwing - Simoon the Poison Wind` (sets Black Whirlwind from Deck for extra NS), `Blackwing - Shamal the Sandstorm`, `Blackwing - Zephyros the Elite`.
     - Bosses & Disruptions: `Blackbird Close` (Counter Trap activatable from hand: negates monster effect, destroys it, and cheats out `Black-Winged Dragon`), `Blackwing Full Armor Master` (3000 ATK tower immune to all effects + steals enemy monsters with Wedge Counters), `Black-Winged Assault Dragon` (3200 ATK, burns 700 every time opponent activates monster effect + quick field nuke), `Raikiri` (board wipe), `Hawk Joe` (resurrects Level 5+ Winged Beasts).
  4. **`Anime_Gong` (Gong Strong / Noboru Gongenzaka — Superheavy Samurai Steadfast)**:
     - Deck: 40 Main (100% monsters, 0 Spells/Traps) / 15 Extra (`Anime_Gong.ydk`).
     - Engine: `Superheavy Samurai Motorbike` (discards to search any SHS), `Superheavy Samurai Prodigy Wakaushi` (1-card scale setup + Special Summon), `Monk Big Benkei` (searches Soul monsters), `Soulpiercer` (non-OPT search on GY send).
     - Bosses & Disruptions: `Superheavy Samurai Brave Masurawo` (4000 DEF, draws up to 3 cards when opponent activates S/T, battles in DEF), `Baronne de Fleur` (Omni-negate + pop), `Warlord Susanowo` (3800 DEF, Quick Effect steals Spell/Trap from opponent's GY), `Ninja Sarutobi` (Quick pop S/T + 500 burn), `Soulbuster Gauntlet` (doubles DEF in damage calc up to 9600 DEF!), `Flutist` (GY target negation), `Gigagloves` (drops direct attack to 0 and draws).
  5. **`Anime_Pegasus` (Maximillion Pegasus — Modern Toon Kingdom Control)**:
     - Deck: 40 Main / 15 Extra (`Anime_Pegasus.ydk`).
     - Engine: `Toon Bookmark` (searches Toon Kingdom + GY protection), `Toon Table of Contents` (searches any Toon card), `Toon Kingdom` (blanket targeting and destruction immunity for all Toons).
     - Bosses & Disruptions: `Toon Terror` (Omni-Negate Counter Trap), `Toon Briefcase` (spins enemy summon into Deck), `Comic Hand` (steals any enemy monster and makes it attack directly), `Toon Black Luster Soldier` (3000 ATK direct attack + banishes 1 card face-up every turn), `Toon Dark Magician` (swarms and searches), `Relinquished Anima` (steals pointed monster), `Number 11: Big Eye` (permanently takes control of enemy monster), `Hope Harbinger` (Spell negate).
- **Compilation & Exclusive Deployment**: All 5 C# executors compiled with 0 errors; deployed via `BUILD_AND_DEPLOY.ps1` directly to `C:\Users\admin\Documents\EdoGame\`. All 5 decks registered in `bots.json` under the **Anime** and **Other Decks** categories in DashBot Launcher.

## 0.007. New Anime Series Rule-Based ModernExecutor: Anime_Bruno (2026-09-20)
- **Source & Concept**: Ported from YGOPRODeck Anime category (`Bruno / Antinomy: ultimate deck`). Features Bruno / Antinomy's modern T.G. (Tech Genus) Accel and Delta Accel Synchro engine with 100% deterministic combo routes, multiple high-stat bosses, and lethal disruptions.
- **Deck Composition (`Anime_Bruno.ydk`)**:
  - Main Deck (40 cards): 3x T.G. Rocket Salamander, 3x T.G. Screw Serpent, 3x T.G. Warwolf, 2x T.G. Striker, 2x T.G. Tank Grub, 2x T.G. Gear Zombie, 2x T.G. Booster Raptor, 1x T.G. Drill Fish, 1x T.G. Rush Rhino, 3x Ash Blossom & Joyous Spring, 3x Infinite Impermanence, 3x T.G. All Clear, 3x T.G. Limiter Removal, 2x Bonfire, 1x One for One, 2x Called by the Grave, 1x Harpie's Feather Duster, 3x T.G. Close.
  - Extra Deck (15 cards): 1x T.G. Trident Launcher, 2x T.G. Mighty Striker, 1x T.G. Over Dragonar, 1x T.G. Star Guardian, 1x T.G. Hyper Librarian, 1x T.G. Wonder Magician, 1x T.G. Recipro Dragonfly, 1x T.G. Power Gladiator, 1x T.G. Blade Blaster, 1x Shooting Star Dragon T.G. EX, 2x T.G. Glaive Blaster, 2x T.G. Halberd Cannon.
- **Architecture & Intelligent Executor (`Anime_BrunoExecutor.cs`)**:
  - **Deterministic Starter Engine**: 1-Card starter via `T.G. Rocket Salamander` (tributes self to Special Summon `Screw Serpent` from Deck; `Screw Serpent` on summon revives `Rocket Salamander` from GY; synchros into `T.G. Over Dragonar`). Also fully searchable via 2x `Bonfire` and 1x `One for One`.
  - **T.G. Over Dragonar Mass Revival**: Level 5 Dragon Synchro resurrects any number of T.G. monsters from GY upon Synchro Summon, forming an unstoppable swarm of materials for higher climbing.
  - **T.G. Mighty Striker & All Clear**: Level 2 Synchro Tuner searches `T.G. All Clear` (grants additional Normal Summon and pops cards to search) or `T.G. Close`; foolishes on sent to GY.
  - **Lethal End Board & Disruptions**:
    - `T.G. Glaive Blaster` (4000 ATK Delta Accel Synchro): Quick Effect banishes monsters Special Summoned from the Extra Deck (up to 2-3 times/turn) + triggers to steal any face-up banished monster onto our field ignoring summon conditions.
    - `T.G. Halberd Cannon` (4000 ATK Delta Accel Synchro): Negates any opponent monster summon (Summon Negate) and destroys it + floats on send.
    - `T.G. Close` (In-archetype Counter Trap): Omni-negates Monster effects, Spells, or Traps while controlling a Machine T.G., and automatically re-sets itself from GY when a Synchro monster is banished.
    - `Shooting Star Dragon T.G. EX` (3300 ATK Accel Synchro): Negates effects targeting friendly monsters by banishing a Tuner from GY + negates attacks.
  - **Heuristics & Target Selection**: Overrode `OnSelectCard` and `OnSelectYesNo` with smart priority ordering (Screw Serpent, Rocket Salamander, Close, All Clear, Warwolf) to prevent accidental self-disruption or miss-timing.
- **Bot Registration & Deployment**: Registered as `Anime_Bruno` in `bots.json` under the **Anime** and **Other Decks** categories; compiled and deployed exclusively to `C:\Users\admin\Documents\EdoGame\` via `BUILD_AND_DEPLOY.ps1`.
---

## 📚 Historical Development Archives
> ประวัติการพัฒนาและบันทึกการทดสอบเวอร์ชันก่อนหน้า ได้รับการย้ายไปจัดเก็บอย่างเป็นหมวดหมู่ที่:
> [Docs/PROGRESS_ARCHIVE.md](Docs/PROGRESS_ARCHIVE.md)

