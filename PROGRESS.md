# Progress Log: AFS, ArtMage, Tenpai, Centurion, VoicelessVoice & ModernExecutors

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

## 0.010. Anime_Pegasus (Pegasus) S:P Little Knight Audit & Self-Targeting Removal Fix (2026-09-20)
- **Problem Statement**:
  - The user observed "pegasus ดีดการ์ดตัวเองลงหลุมหรอ" (Why does Pegasus send his own cards to GY?) and requested "ดู log ล่าสุดเกี่ยวกับการกระทำของ SP knight" (Analyze recent logs regarding S:P Little Knight's actions).
- **Log Analysis & Root Cause Findings (`WindBot/logs/Blue_Angel_vs_Pegasus_25690920_123602` & `123537`)**:
  1. **Comic Hand Waste on S:P Link Summon (Turn 2)**:
     - Pegasus activated `Comic Hand` (33453260) to take control of Blue Angel's `Trickstar Lilybell` (98700941) and Normal Summoned `Toon Cyber Dragon` (83629030).
     - Because `SPLittleKnightSpSummon()` returned `true` unconditionally, the bot sacrificed both the stolen monster and `Toon Cyber Dragon` as Link materials to summon `S:P Little Knight` (29301450).
     - As a result, the stolen monster left the field, sending `Comic Hand` straight to the GY, destroying Pegasus's own advantage and replacing high-ATK direct attackers under `Toon Kingdom` with a 1600 ATK link monster.
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

---

## 📚 Historical Development Archives
> ประวัติการพัฒนาและบันทึกการทดสอบเวอร์ชันก่อนหน้า (รวมถึง 0.001 - 0.008) ได้รับการย้ายไปจัดเก็บอย่างเป็นหมวดหมู่ที่:
> [Docs/PROGRESS_ARCHIVE.md](Docs/PROGRESS_ARCHIVE.md)

