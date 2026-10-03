# Progress Log: Central Core Architecture & Universal Heuristics Overhaul

## 0.094. Central Core Pro Refactor — Board Re-read, Preselect Honouring, Activation Threat & ComboRouter Continuity (2026-10-04)

- **Checkpoint**: push `6c16d31` ก่อน refactor · Build 0 Errors · Deploy `C:\Users\admin\Documents\EdoGame\` · ยังไม่รัน Text Duel (รอคำสั่ง)
- **Board reading**: `ModernExecutor.OnSelectIdleCmd` เรียก `Analysis?.Refresh()` ทุก idle prompt; `DecisionContext.ShouldActivate` อัปเดต `UpdateState(Brain.OwnSummons, Brain.OpponentSummonCount)` ก่อนตัดสิน
- **Handtrap timing (generic, อ่านข้อความการ์ด)**:
  - `ThreatAnalyzer.GetActivationThreatScore` ใหม่ (engine text +30 / Extra +10 / one-shot spell +5 / disabled ×0.5) แทนคะแนนตาม location
  - `ChainTimingAdvisor`: unknown engine +15, early penalty −10→−5 (score < 60), ลบ Eater of Millions, Meluseek → 25533642, `CountInteractiveCards` ใช้ `CardIntelligence.IsHandtrap`
  - `CardTextSemantics.IsEngineEffect` (search / Special Summon / draw)
- **Selection pipeline**: `GameAI.TryConsumePreselectedCards` (match-only) + `PreselectCheckpoint/RollbackPreselect` (func false / skip duplicate chain) + ล้าง selector ทุก idle; `ModernExecutor.OnSelectCard` step 0 ใช้ `AI.SelectCard` ที่ match pool (cost hint = explicit card เท่านั้น); `CompleteSelection` เติม pick ให้ครบ min ตาม heuristic; 502 กรอง destruction-immune
- **ComboRouter continuity**: per-turn executed history → fallback line ไม่ยิง step ซ้ำ; `ComboStepApprovedByExecutors` (exact-ID executor ก่อน, ActivateDescription จริง, rollback); `IsOpponentCardNegator` เฉพาะ negate จริง (Maxx "C" ไม่ทำให้ Abort); backrow นับใน RequiredCards
- **Card intelligence**: battle-immune ≠ effect-immune (`IsBattleImmune` แยก); negator/floodgate disabled → threat ×0.5; duplicate-handtrap guard นับเฉพาะการ์ดเรา
- **Position**: MP1 (Turn > 1) มอน ATK ≥ 1000 & ATK ≥ DEF ตั้ง ATK เมื่อศัตรูว่างหรือมีเป้าที่ตีชนะ
- **Skill/Docs**: `Docs/CORE_PRO_AUDIT_2026-10-04.md`; skill `SKILL.md` (§6 Laws 4/6/8/9, §7.1 Pro-Play Principles, §10), `core_api.md`, `hint_reference.md` §3, `known_core_issues.md` (#27–36 ✅, #37–45 ⚠️)
- **Archive**: ย้าย PROGRESS 0.076–0.081 → `Docs/PROGRESS_ARCHIVE.md`

## 0.093. Numeron Dragon & Dynamic ATK FaceUpAttack Position Safeguard (2026-10-04)

- **Root Cause & Universal Position Safeguard Fix (`ModernExecutor.cs`)**:
  - ตรวจพบสาเหตุที่แท้จริง: เครื่องคำนวณฐานของ `ModernExecutor` และ `DefaultExecutor` มีเงื่อนไขส่งมอนสเตอร์ที่มีพลังโจมตีบนการ์ด `Attack == 0` หรือ `Defense >= 0` ไปตั้งรับ (`FaceUpDefence`) อัตโนมัติ ทำให้ `Number 100: Numeron Dragon` (พลังตั้งต้น 0/0), `Gren Maju Da Eiza` (?/?), `Eater of Millions` (?/?), และ `Danger! Bigfoot!` (3000/0) ถูกอัญเชิญในสภาพตั้งรับ ไม่สามารถประกาศโจมตีได้ และเมื่อเข้าเทิร์นศัตรูก็ถูกตีตายทันทีเนื่องจากมี 0 DEF
  - เพิ่ม **Universal Safeguard for Dynamic ATK Boosters & 0-DEF Finishers** ใน `ModernExecutor.OnSelectPosition` บังคับให้มอนสเตอร์สายบูสต์พลังเหล่านี้อัญเชิญในสภาพ **FaceUpAttack เสมอ 100%**
- **Gren Maju Domain Position & Aggression Overrides (`GrenMajuExecutor.cs`)**:
  - Overrode `OnSelectPosition` ใน `GrenMajuExecutor`: บังคับให้มอนสเตอร์ตัวบุกทั้งหมด (`Numeron Dragon`, `Gren Maju`, `Eater of Millions`, `Bigfoot`, `Thunderbird`, `Alpha`, `Dogoran`, `Draglubion`, `Dingirsu`, `Hope Harbinger`) ลงสนามในสภาพ **FaceUpAttack**
  - กำหนดให้ `Lava Golem` และ `Kaiju` ที่ส่งไปให้ศัตรูอยู่ในสภาพ **FaceUpAttack** เพื่อให้ Numeron Dragon (9,000–13,000 ATK) หรือ Gren Maju พุ่งเข้าชนทะลวงพลังโจมตีปิดเกม (Piercing OTK) ได้เต็มดาเมจ
  - เพิ่ม `MonsterRepos` บังคับสลับมอนสเตอร์หลักจากสภาพตั้งรับกลับมาเป็นสภาพโจมตีทันที หากโดนเอฟเฟกต์ศัตรูบังคับเปลี่ยนสภาพ
- **Tactical Timing & Draglubion Execution Safeguards**:
  - กำหนด `DraglubionSummon` ให้รันเฉพาะ **Main Phase 1** เท่านั้น ป้องกันการอัญเชิญใน Main Phase 2 ที่ไม่สามารถประกาศโจมตีได้
  - ใน `DraglubionEffect` และ `OnSelectCard`: ระบุตำแหน่ง `AI.SelectPosition(CardPosition.FaceUpAttack)` ชัดเจนขณะ Special Summon `Numeron Dragon`
  - ปรับปรุง `AlphaEffect`: ป้องกันไม่ให้ Alpha สั่งเด้งตัวเองกลับขึ้นมือโดยไม่จำเป็น หากสนามมีเลเวล 8 ครบ 2 ตัวและพร้อมทำ Draglubion -> Numeron Dragon ปิดเกม
- **Build, Exclusive Deploy & Asset Sync**:
  - คอมไพล์และ Deploy สำเร็จสู่ `C:\Users\admin\Documents\EdoGame\` ด้วย 0 Errors
  - บันทึกรายงานลงใน `Docs/GREN_MAJU_AUDIT_AND_PLAYBOOK.md`

## 0.092. Gren Maju Level 8 Danger! Turbo & Rank 8 Numeron Plan B (2026-10-04)

- **Eliminated RNG Stromberg Engine & Normal Summon Lock**:
  - ยกเลิกการใช้งาน `Golden Castle of Stromberg`, `Glife`, `Hexe Trude` ทั้งหมด เพื่อขจัดปัญหาการสุ่มโม่การ์ดทิ้ง 10 ใบทุกเทิร์นจนการ์ดหลักหายเกลี้ยง และปลดล็อกพันธสัญญาห้าม Normal Summon ทำให้สามารถอัญเชิญ `Gren Maju Da Eiza` ปิดเกมได้อย่างอิสระ
- **Rank 8 XYZ Plan B OTK Engine (`Numeron Dragon` 9000-13000 ATK)**:
  - เพิ่มคอมโบปิดเกมสำรองเมื่อ `Gren Maju Da Eiza` ถูกรีมูฟหมดหรือไม่ถูกจั่ว:
    - เรียก `Number 97: Draglubion` (Rank 8) ➔ สั่งเอฟเฟกต์ Special Summon `Number 100: Numeron Dragon` พร้อมแนบ `Number 38: Hope Harbinger`
    - เอฟเฟกต์ `Numeron Dragon` ถอดวัตถุดิบ 1 ชิ้น บูสต์พลังโจมตีไดนามิกตามผลรวม Rank ทั้งสนาม $\times 1000$ ได้พลังโจมตีมหาศาล **9,000 – 13,000+ ATK** สามารถพุ่งเข้าชนปิดเกม (One-Shot OTK) ทันที 100% โดยไม่ต้องพึ่งพา Gren Maju หรือการ์ดรีมูฟ
- **Targeted Draw & Hand Sculpting (`Trade-In` & `Danger!` Engine)**:
  - บรรจุ `Trade-In` x3 ผสานมอนสเตอร์เลเวล 8 รวม 17 ใบในเด็ค ทิ้งการ์ดเพื่อจั่ว 2 ใบอย่างแม่นยำ ไม่สุ่มโม่ทิ้ง
  - เมื่อทิ้ง `Danger! Bigfoot!` (3000 ATK) ➔ ทริกเกอร์ยิงทำลายการ์ดหงายหน้าศัตรูฟรี 1 ใบ
  - เมื่อทิ้ง `Danger! Thunderbird!` (2800 ATK) ➔ ทริกเกอร์ยิงทำลายการ์ดคว่ำศัตรูฟรี 1 ใบ
  - เสริม `Alpha, the Master of Beasts` (3000 ATK) อัญเชิญพิเศษฟรีเมื่อบอร์ดศัตรูพลังสูงกว่า พร้อมเอฟเฟกต์เด้งบอสศัตรูกลับขึ้นมือแบบ Non-Targeting & Non-Destruction
- **Emergency Deck-Out Recovery Safeguard (`Necroface`)**:
  - บรรจุ `Necroface`: เมื่อเข้าตาจนหรือการ์ดในเด็คเหลือน้อย ($\le 4$ ใบ) สามารถ Normal Summon เพื่อสับการ์ดที่ถูกรีมูฟทั้งหมด (รวมถึงการ์ดคว่ำหน้า!) กลับเข้าเด็ค รีเซ็ตเด็คใหม่ทันที
- **Decoupled Plugin & Material Safety Overhaul**:
  - `GrenMajuPlugin.cs`: ปรับปรุง `GrenMajuStrategy`, `GrenMajuMaterialEvaluator`, `GrenMajuThreatEvaluator`
  - กำหนด Material Cost ใน Extra Deck ให้จัดอันดับ Fusion/Link Fodder เป็นกระสุนสังเวยของ `Eater of Millions` และ `Gizmek Orochi` โดยเด็ดขาด ห้ามรีมูฟชุด Draglubion / Numeron / Hope Harbinger ทิ้ง
- **Build, Exclusive Deploy & Asset Sync**:
  - คอมไพล์และ Deploy สำเร็จสู่ `C:\Users\admin\Documents\EdoGame\` ด้วย 0 Errors
  - อัปเดต Playbook ใน `Docs/GREN_MAJU_AUDIT_AND_PLAYBOOK.md`

## 0.091. Gren Maju Combo Ignition & Executor Collision Resolution (2026-10-04)

- **Executor Reflection Collision Fix**:
  - ตรวจพบสาเหตุหลักที่คอมโบไม่ทำงาน: `GrenMajuThunderBoarderExecutor.cs` เคยมี attribute `[Deck("GrenMaju", "AI_GrenMajuThunderBoarder")]` ซึ่งทับการแมปของ `GrenMajuExecutor` ทำให้เครื่องรันตรรกะเด็คบอร์ดเดอร์สตันโบราณแทนที่จะใช้สถาปัตยกรรม Stromberg / Kaiju ใหม่
  - ลบ attribute ซ้ำซ้อนออกจาก `GrenMajuThunderBoarderExecutor.cs` เพื่อให้เด็ค `GrenMaju` โหลด `GrenMajuExecutor` 100%
- **Golden Castle of Stromberg Field Ignition Fix**:
  - แก้ไข `StrombergActivateEffect` ให้แยกเงื่อนไขชัดเจนระหว่างการร่ายจากมือ (`CardLocation.Hand`) และการสั่งเอฟเฟกต์กางสนาม (`CardLocation.SpellZone`) เพื่อ Special Summon `Hexe Trude` จากในเด็ค
- **Gizmek Orochi Quick Effect & Pop Activation**:
  - เปลี่ยนจากการลงทะเบียนผ่าน `SpSummon` เป็น `Activate` ทั้งจากมือและสุสาน เนื่องจาก Orochi เป็น Quick Effect พร้อมตั้ง margin ป้องกัน Deck Out
  - รองรับเอฟเฟกต์ยิงทำลายมอนสเตอร์ศัตรูบนสนามโดยรีมูฟการ์ด Extra Deck คว่ำหน้า 3 ใบ
- **Multi-Tribute Safeguard (Lava Golem & Kaiju)**:
  - แก้ไข `OnSelectCard` (Hint 500: `HINTMSG_RELEASE`) ให้คืนค่าลิสต์เป้าหมายจำนวน $\ge min$ ใบ (Lava Golem ต้องการ 2 เป้าหมาย) ป้องกันการเลือกหลุด/ค้าง
- **Eater of Millions & Gren Maju Combat RealPower**:
  - บันทึกการเลือก Extra Deck 5 ใบเพื่อเป็น Cost การอัญเชิญ Eater of Millions
  - ใน `OnPreBattleBetween`: ตั้งค่า `attacker.RealPower = 9999` สำหรับ `Eater of Millions` เพื่อให้บอทรับรู้ว่ามอนสเตอร์สามารถรีมูฟศัตรูคว่ำหน้าได้โดยไม่ตาย และคำนวณพลังโจมตีไดนามิก `RealPower = banishedCount * 400` สำหรับ `Gren Maju Da Eiza` เพื่อให้กล้าประกาศโจมตีและปิดเกมทันที
- **Hexe Trude Tributeless Normal Summon**:
  - เพิ่มตัวรับรอง Normal Summon ของ Hexe Trude โดยไม่ต้องสังเวยเมื่อมี `Golden Castle of Stromberg` อยู่บนสนาม
- **Build, Exclusive Deploy & Asset Sync**:
  - คอมไพล์และ Deploy สำเร็จสู่ `C:\Users\admin\Documents\EdoGame\` ด้วย 0 Errors

## 0.090. Gren Maju Stromberg & Mass Banishing OTK Architecture (Special Category) (2026-10-03)

- **Special Category Archetype Implementation**:
  - พัฒนาเด็คสายทรมานผู้เล่นระดับตำนาน `GrenMaju` เข้าสู่หมวด **Special** (Badge: Deep Violet `#5B21B6`) ใน DashBot Launcher
  - `dashbot/MainWindow.xaml.cs`: ลงทะเบียน `"GrenMaju"` ใน `SpecialArchetypes` และกำหนดชื่อแสดงผล `"Gren Maju OTK"` ใน `CleanDeckDisplayName`
- **Gren Maju Decoupled Domain Plugin & Modern Executor**:
  - `GrenMajuPlugin.cs`: วางระบบ `GrenMajuStrategy`, `GrenMajuMaterialEvaluator`, `GrenMajuThreatEvaluator` แยก Domain Logic ออกจาก Router 100%
  - `GrenMajuExecutor.cs`: สืบทอดจาก `ModernExecutor` วาง Priority Action Pipeline จัดการ Tribute Removal (Lava Golem / Kaiju), Mass Banish Pots, Stromberg Battle Lock, Eater of Millions Face-down Banish, และ Gren Maju Nuclear OTK Normal Summon
- **Deck & Banlist Compliance (`GrenMaju.ydk`)**:
  - จัดเด็ค 40 Main / 15 Extra ถูกต้องตาม Banlist ทั้ง `0TCG.lflist.conf` (2026.09 TCG) และ `OCG.lflist.conf` 100% ปราศจาก ERRMSG_DECKERROR
  - ผนวก `Gren Maju Da Eiza` x3, `Golden Castle of Stromberg` x3, `Glife the Phantom Bird` x3, `Gizmek Orochi` x3, `Eater of Millions` x3, `Lava Golem` x2, `Kaijus` x4, `Pot of Desires` x3, `Pot of Extravagance` x2, `Macro Cosmos` x2, `Super Polymerization` x2
- **Build, Exclusive Deploy & Asset Sync**:
  - คอมไพล์และ Deploy สำเร็จสู่ `C:\Users\admin\Documents\EdoGame\` ด้วย 0 Errors
  - บันทึกรายงาน `Docs/GREN_MAJU_AUDIT_AND_PLAYBOOK.md`

## 0.089. BarrierStun Elemental Lock & Anti-Meta Stun Architecture (2026-10-03)

- **BarrierStun Decoupled Domain Plugin Architecture**:
  - `BarrierStunPlugin.cs`: สร้างสถาปัตยกรรม Decoupled Plugin (`BarrierStunStrategy`, `BarrierStunMaterialEvaluator`, `BarrierStunThreatEvaluator`) เพื่อแยกตรรกะ Domain ออกจาก Executor Router
  - `BarrierStunExecutor.cs`: พัฒนา Rule-Based ModernExecutor สำหรับเด็ค `BarrierStun` ตามมาตรฐาน Rule 9 (Clean Naming)
- **Intelligent Attribute Detection & Positioning Safeguards**:
  - `BarrierStunStrategy.PickNormalSummonTarget`: วิเคราะห์ธาตุของคู่ต่อสู้ หากคู่ต่อสู้เป็นเด็คธาตุแสง (BlueEyes, ABC) บอทจะเลือกอัญเชิญ `Barrier Statue of the Abyss` เพื่อบล็อกการ Special Summon 100% หากคู่ต่อสู้เป็นเด็คธาตุมืด (DarkMagician) จะเลือกอัญเชิญ `Barrier Statue of the Heavens`
  - `OnSelectPosition`: มอนสเตอร์ Barrier Statue ที่ยังไม่มีดาบ `Moon Mirror Shield` จะอัญเชิญในสภาพ **FaceUpDefence** เสมอ เพื่อป้องกันการถูกมอนสเตอร์ Normal Summon ตัวเล็กของฝ่ายตรงข้ามตีตายฟรี
- **Proactive Threat Removal & Daruma Trigger**:
  - `DarumaCannonEffect`: ปรับปรุงให้ยิงสกัดทันทีเมื่อศัตรูมีมอนสเตอร์พลังโจมตี $\ge 2000$ ATK, มอนสเตอร์ Extra Deck, หรือมอนสเตอร์ Link
  - `CrackdownEffect`: ปรับเกณฑ์การขโมยมอนสเตอร์เป้าหมายอันตราย $\ge 1500$ ATK หรือบอสมอนสเตอร์ Extra Deck
- **Deck & Banlist Optimization**:
  - จัดสร้างเด็ค `BarrierStun.ydk` (40 Main / 15 Extra) ถูกต้องตาม Banlist ทั้ง `0TCG.lflist.conf` (2026.09 TCG) และ `OCG.lflist.conf` 100% ปราศจากการ์ดแบน (เช่น Summon Limit = 0)
  - ผนวก `Barrier Statue of the Abyss` x3, `Barrier Statue of the Heavens` x3, `Inspector Boarder` x3, `Kycoo the Ghost Destroyer` x3, `Moon Mirror Shield` x3, `Morganite` x3, และเคาน์เตอร์แทรปครบชุด
- **Headless Duel Verification (40 แมตช์)**:
  - Altergeist: ชนะ 8 จาก 10 แมตช์ (**80.0% Win Rate**)
  - ABC: ชนะ 6 จาก 9 แมตช์ (**66.7% Win Rate**)
  - DarkMagician: ชนะ 5 จาก 10 แมตช์ (**50.0% Win Rate**)
  - BlueEyes: ชนะ 5 จาก 10 แมตช์ (**50.0% Win Rate**)
  - **ภาพรวม: ชนะ 24 จาก 40 แมตช์ (61.5% Win Rate), 0 Violations, 0 Warnings, 0 Bot Crashes**
- **Build, Exclusive Deploy & Asset Sync**:
  - คอมไพล์และ Deploy สำเร็จสู่ `C:\Users\admin\Documents\EdoGame\` ด้วย 0 Errors
  - บันทึกรายงาน `Docs/BARRIER_STUN_AUDIT_AND_PLAYBOOK.md`

## 0.088. Raioh Decoupled Plugin & Anti-Meta Stun Overhaul (2026-10-03)

- **Raioh Decoupled Domain Plugin Architecture**:
  - `RaiohPlugin.cs`: สร้างปลั๊กอินแยกส่วนตามสถาปัตยกรรม Decoupled Plugin (`RaiohStrategy`, `RaiohMaterialEvaluator`, `RaiohThreatEvaluator`) เพื่อแยกตรรกะ Domain ออกจาก Executor Router
  - `RaiohExecutor.cs`: สืบทอดจาก `ModernExecutor` ปรับชื่อเด็คให้สะอาด (`Raioh` ตาม Rule 9) พร้อมคง alias สำหรับ `RaiOh` และ `2026_RaiOh`
- **Smart Counter Trap Targeting (`Duel.SummoningCards`)**:
  - ตรวจสอบ `Duel.SummoningCards` ใน `SolemnWarningEffect`, `SolemnJudgmentEffect`, `SolemnStrikeEffect`, และ `RaiOhNegateEffect`
  - ป้องกันการเสีย LP และคัดแย้งเคาน์เตอร์กับมอนสเตอร์จูนเนอร์/วัตถุดิบ 0 ATK (เช่น The White Stone, Sage) และเก็บเคาน์เตอร์ไว้สกัดบอสมอนสเตอร์จริง
- **Proactive Floodgate Activation**:
  - ปรับปรุงให้ `Anti-Spell Fragrance`, `Skill Drain`, `Macro Cosmos`, และ `There Can Be Only One` ทำงานทันทีในเทิร์นของฝ่ายตรงข้าม (`Duel.Player == 1`) เพื่อล็อกสนามตั้งแต่ต้นเฟส
- **Deck & Banlist Optimization**:
  - ปรับโครงสร้างเด็ค `Raioh.ydk` (40 Main / 15 Extra) ถูกต้องตาม Banlist ทั้ง `0TCG.lflist.conf` (2026.09 TCG) และ `OCG.lflist.conf` 100% (0 Violations / 0 Errors)
  - เพิ่ม `Anti-Spell Fragrance` (1 ใบ) เพื่อบล็อกการ์ดเวทมนตร์กวาดสนามของฝ่ายตรงข้าม
- **Headless Duel Verification (40 แมตช์)**:
  - BlueEyes: ชนะ 9 จาก 10 แมตช์ (**90.0% Win Rate**, เพิ่มจากเด็คเดิมที่ได้ 33.3%)
  - DarkMagician: ชนะ 7 จาก 10 แมตช์ (**70.0% Win Rate**)
  - Altergeist: ชนะ 6 จาก 10 แมตช์ (**60.0% Win Rate**)
  - ABC: ชนะ 6 จาก 10 แมตช์ (**60.0% Win Rate**)
  - **ภาพรวม: ชนะ 28 จาก 40 แมตช์ (70.0% Win Rate), 0 Violations, 0 Warnings, 0 Crashes**
- **Build, Exclusive Deploy & Asset Sync**:
  - คอมไพล์และ Deploy สำเร็จสู่ `C:\Users\admin\Documents\EdoGame\` ด้วย 0 Errors
  - บันทึกรายงาน `Docs/RAIOH_AUDIT_AND_PLAYBOOK.md`

## 0.087. Comprehensive Deck Card-ID Fix & Core Heuristic Enhancements (2026-10-03)

- **Special Summon Lock Floodgate Precision**:
  - `CardIntelligence.cs`: แยก HashSets `SpecialSummonLockMonsters` และ `SpecialSummonLockSpellsTraps` เฉพาะการ์ดที่ล็อก Special Summon จริง (Jowgen, Fossil Dyna, Winda, Vanity's Fiend/Ruler, Archlord Kristya, Summon Limit, Domain of the True Monarchs ฯลฯ) แยกออกจาก generic floodgate (Bagooska, Boarder, Majesty's Fiend, Skill Drain, Anti-Spell) · เช็คคอนโทรลเลอร์ Vanity's Ruler ฝั่งเราไม่บล็อกตัวเอง
  - `AntiFloodgateHelper.cs` & `ModernExecutor.cs`: เชื่อมต่อเข้ากับ `CardIntelligence.IsSpecialSummonLockMonster/SpellTrap` แก้ปัญหาคอมโบล็อกค้างก่อนเวลาอันควรกว่า 600 จุด
- **MP2 Guard Flexibility**:
  - `ModernExecutor.cs`: เพิ่ม property `EnableMP2Guard` และ virtual method `ShouldAllowMP2Activation(ClientCard card)` ให้เด็คและปลั๊กอินสามารถเปิด/ปิด หรือ override เงื่อนไขการเล่นการ์ด Search/Draw ใน Main Phase 2 ได้ ไม่ติดบล็อกบอร์ดแข็งแบบตายตัว
- **504 (Deck → GY) Target Separation**:
  - `IDeckStrategy.cs`: เพิ่มอินเทอร์เฟซ `PickFoolishGraveTarget(IList<ClientCard> candidates, ClientCard context)`
  - `GameAI.cs`: เพิ่ม `HasPreselectedCard()` ตรวจสอบว่ามีการ์ดที่เลือกไว้ล่วงหน้าผ่าน `AI.SelectCard` หรือไม่
  - `ModernExecutor.cs`: Branch 1.8 (Hint 504 จาก Deck ลง GY) ตรวจสอบ preselected card ก่อน และเรียก `Strategy.PickFoolishGraveTarget` แทนการตกไปใช้ `PickSearchTarget` โดยตรง เพื่อป้องกันการทิ้ง starter ลงสุสานผิดพลาด
- **ComboRouter Step & Phase Guard Integration**:
  - `ComboRouter.cs`: เพิ่ม `public Func<bool> Condition { get; set; }` ให้กับ `ComboStep`
  - `ModernExecutor.cs`: `OnSelectIdleCmd` ตรวจสอบ `step.Condition`, Phase Guards (`ShouldAllowActivate`, `ShouldAllowSummon`, `ShouldAllowSpSummon`, `ShouldAllowSpellSet`), และ `CardExecutor.Func` ก่อนส่งคำสั่ง เพื่อไม่ให้ ComboRouter ข้าม guard และเงื่อนไขเฉพาะของเด็ค
- **Default `IsAceCard` Refinement**:
  - `Executor.cs`: ปรับปรุง `IsAceCard(ClientCard card)` ไม่นับการ์ด Extra Deck ทุกใบ (Link-1, Link-2, intermediate Synchro) เป็น Ace แต่ตรวจสอบเฉพาะการ์ดที่ลงทะเบียนใน `HeuristicGuard.IsAceCard`, `CardIntelligence.IsKnownNegator`, มอนสเตอร์ที่มี ATK $\ge 3000$, หรือ Link-4 ขึ้นไป
- **Synchro Material Routing**:
  - `ModernExecutor.cs`: Implement overloads ทั้งสองของ `OnSelectSynchroMaterial` ส่งต่อให้ `DeckPlugin.MaterialEvaluator.SortMaterials` (สอดคล้องกับ Fusion, Link, และ Xyz)
- **Comprehensive Card-ID Audit & Fix (243 ไฟล์ / 0 issues)**:
  - แก้ไข Card ID ปลอม/ผิดการ์ดครบ 236 จุดในเด็คและปลั๊กอินทั้งหมด:
    - `RaiOhExecutor.cs`: Fossil Dyna, Jowgen, Winda, Skill Drain, Summon Limit, Gozen, Rivalry, Dolkka, Herald, Raigeki, Lightning Storm, Cosmic Cyclone, Dark Hole
    - `InvokeExecutor.cs`: Altergeist Hexstia, Primebanshee, Protocol, Manifestation, Pookuery
    - `BlackwingsExecutor.cs`: Pot of Extravagance, Union Driver, Branded Fusion
    - `EneaCraftExecutor.cs`: Evenly Matched, Dark Hole, Domain of the True Monarchs, Floowandereeze & True Draco IDs
    - `RadiantExecutor.cs`: The Arrival Cyberse, Marincess Great Bubble Reef, Blue-Eyes Jet Dragon, Ultimate Falcon
    - `DinomorphiaExecutor.cs`: Lightning Storm, Evenly Matched, Dark Ruler No More
    - `DogmaStunExecutor.cs`: Cosmic Cyclone
    - `LabrynthExecutor.cs`: Windwitch - Winter Bell, Secrets of Dark Magic, Magicians' Souls, Bingo Machine, Go!!!, Dark Magician Girl
    - `ModernBlueEyesExecutor.cs`: Branded Fusion, Nadir Servant, Fossil Dig, Small World, Pot of Extravagance
    - `ToonExecutor.cs`: Raigeki, Book of Eclipse
    - `TrirealmRiftExecutor.cs`: True Light, Magicians' Souls, Jizukiru, Kumongous, Personal Spoofing, Manifestation
    - `AltergeistExecutor.cs` & `SalamangreatExecutor.cs`: Invoked Mechaba
    - `AncientGExecutor.cs`: KnownNegator, Eternal Soul, Dark Magician the Dragon Knight
    - `AztecRockExecutor.cs` & `BigShieldCounterExecutor.cs`: Amazoness Swords Woman, Yubel - The Ultimate Nightmare
    - `BrandedExecutor.cs`: Borreload Savage Dragon, KnownNegator
    - `BrElfnoteExecutor.cs`: Apollousa Bow of the Goddess
    - `TimeThiefExecutor.cs`: Time Thief Bezel Ship
    - Plugins: `Anime_KaibaPlugin.cs`, `Anime_SayerPlugin.cs`, `IceBarrierPlugin.cs`, `LairOfDarknessPlugin.cs`, `OjamaLockPlugin.cs`, `OrcustPlugin.cs`, `SkyStrikerZeroPlugin.cs`, `DinosmasherPlugin.cs`
  - ปรับปรุง `tools/audit_card_ids.py` รองรับ Code Variable Matching, Evaluation Score Filtering, และ Normalized Aliases → **0 issue(s) in 243 file(s)**
- **Build & Exclusive Deploy**:
  - รัน `BUILD_AND_DEPLOY.ps1` คอมไพล์ผ่าน 0 Errors และ Deploy ไปยัง `C:\Users\admin\Documents\EdoGame\` ครบถ้วน

## 0.086. Core Bug Fixes & Card-ID Audit (2026-10-03)

- **Hint routing (`ModernExecutor.OnSelectCard`)**: 507 ฝั่งเรา → cost (`PickDiscardTarget`) แทนการคืน Ace กลับเด็ค · 504 Deck→GY → `Strategy.PickSearchTarget` (branch 1.8) · POSCHANGE = 528 / EQUIP = 518 แยกกัน · ลบ 552 (COIN) ออกจาก removal
- **`Executor.FallbackSelectCard`** เขียน hint case ใหม่ตาม `constant.lua` (Case1 ศัตรู / Case2 cost / Case3 positive) · `CardTextSemantics.HINTMSG_TARGET` = 551
- **Bagooska**: helper `IsBagooska()` (90590303/90590304) ใน position/repos; ลบ ID ผิด (26273196/85359414/26593852) ใน ModernExecutor/DefaultExecutor/AntiFloodgateHelper
- **MP2 guard** ใน `ShouldAllowActivate` ย้ายขึ้นก่อน `Phase != Main1` (เดิม unreachable) — บล็อก ResourceGain/ComboStarter ใน MP2 เมื่อ `IsBoardStrongEnough()`
- **Duplicate chain guard**: `IsDuplicateOwnChainActivation()` เรียกเป็นอันดับแรกใน `SmartHandTrapChain` ทั้ง 2 overload
- **ComboRouter**: `GetViableLines` กรอง `_failedLinesThisTurn`
- **Card-ID Audit (core)**: แก้ ID ปลอม/ผิดการ์ด ~140 entries ใน 14 ไฟล์ (CardIntelligence, ModernExecutor, DefaultExecutor, Executor, AntiFloodgateHelper, ChainTimingAdvisor, ThreatAnalyzer, BoardScorer, BaitPlanner, DynamicValueEvaluator, HeuristicGuard, CardExtension, AIBrain, ActionHistory) → floodgate/negator/handtrap/chokepoint detection ทำงานถูกต้อง · เพิ่ม Herald of Perfection (44665365) ใน `_negateMonsters`
- **configs**: `chain_targets.json` / `archetypes.json` สร้างใหม่จาก cdb · `BUILD_AND_DEPLOY.ps1` deploy `configs/*.json` → `EdoGame\WindBot\configs`
- **เครื่องมือใหม่**: `tools/audit_card_ids.py <path>` (NOTFOUND / MISMATCH + เสนอ ID ที่ถูก) — บังคับใช้ใน SKILL.md §3.1/§4.1/§10
- **Deck/DashBot**: `ModernArchetypes` เพิ่ม Watenpai / Darklord 2 / Darklord2 · Darklord2 ลบ reset turn state ซ้ำ
- **Skill sync**: SKILL.md, `hint_reference.md`, `core_api.md`, `known_core_issues.md` (✅/⚠️), `executor_template.md` (compile 0 Error) อัปเดตตาม core ใหม่
- ⚠️ ยังไม่แก้: Card ID ระดับเด็ค 236 hit (TrirealmRift 55, Tenpai 30, EneaCraft 17, RaiOh 16, Invoke 10, Blackwings 8, …)
- รายงาน: `Docs/SKILL_AUDIT_2026-10-03.md` (§ Core bugs, § Card-ID Audit)

## 0.085. Master Skill Audit & Module Registration (2026-10-03)

- **`yugioh-executor` SKILL.md เขียนใหม่เป็น Master Skill**: Session Protocol / Definition of Done, Module Registration Matrix, 12 Executor Engineering Laws, Hint quick table, Safety checklist, Pull/Push workflow
- **references/ ใหม่ 4 ไฟล์** (ตรวจกับซอร์สจริงแล้ว):
  - `hint_reference.md` — HINTMSG 500–579 จาก `script/constant.lua` + การ route ของ ModernExecutor + Selection pipeline ของ GameAI
  - `core_api.md` — API map ทุกโมดูล (Plugin interfaces, ComboRouter, BaitPlanner, ChainAdvisor, ResourcePlan, HeuristicGuard, OpponentProfile, AIContext ฯลฯ)
  - `known_core_issues.md` — 18 จุดบกพร่องใน core พร้อม workaround ระดับเด็ค
  - `executor_template.md` — Template Executor + Plugin ครบทุกโมดูล (**compile ผ่าน 0 Error**)
- **Audit mismatch สำคัญที่แก้ในสกิล**: `AI.SelectCard` ถูกข้ามสำหรับ hint ที่ ModernExecutor จัดการเอง, ComboRouter step ข้ามฟังก์ชันเด็ค/guard, BaitPlanner ไม่ทำงานถ้าไม่เรียก `GetBaitIfNeeded`, `IsAceCard` default (ATK≥2500/Extra), Optional trigger ใช้ฟังก์ชัน Activate เดียวกัน, Synchro (512) ไม่ผ่าน Plugin
- **Core bugs ที่พบ** → ✅ แก้แล้วใน 0.086
- เพิ่ม `SYNC_AGENT_ASSETS.ps1` + กฎ Push ต้องรวม `.agents/` + `Docs/` + `PROGRESS.md` + `AGENTS.md` ทุกครั้ง
- รายงาน: `Docs/SKILL_AUDIT_2026-10-03.md`

## 0.084. Watenpai & Darklord 2 Deep Code Audit, Logic Optimization & Simulation Verification (2026-10-03)

### 1. Root Cause Analysis & Card Database (CDB) Verification
- **Card ID & Effect Alignment**:
  - `Darklord Eveningstar` (10136446): ตรวจสอบสเตตัสและการทำงานจริงจาก `cards.cdb` พบว่าเอฟเฟกต์ไม่ใช่การมิลการ์ดตามมอนสเตอร์ศัตรู แต่เป็นเอฟเฟกต์ **เมื่อ Special Summon ด้วยเอฟเฟกต์ Darklord จะได้เซ็ต 1 Darklord Spell + 1 Darklord Trap จากเด็คลงสนามทันที**, มีออร่าคุ้มกันมอนสเตอร์แฟรี่ไม่ให้ตกเป็นเป้าหมายเอฟเฟกต์ศัตรู และมี Quick Copy ในสุสาน
  - แก้ไข Card ID ที่บันทึกคลาดเคลื่อนในเอกสารและคอมเมนต์:
    - `The First Darklord`: 4167084 (เดิมระบุ 80004149)
    - `Darklord Morningstar`: 25451652 (เดิมระบุ 82134632)
    - `Darklord Ixchel`: 52840267 (เดิมระบุ 44203504)
    - `Darklord Tezcatlipoca`: 88234365 (เดิมระบุ 39185152)
    - `Condemned Darklord`: 35306215 (เดิมระบุ 33883834)
    - `Trident Dragion`: 39402797 (เดิมระบุ 39823901)
    - `Sangenpai Transcendent Dragion`: 18969888 (เดิมระบุ 2992036)
    - `Wattkyuki` / `Watthydra`: 67752972 / 29765339 (เดิมระบุ 78586116 / 9888196)
    - `Sangen Summoning`: 30336082 (เดิมระบุ 15848542)

### 2. Logic Optimization & Bug Fixes
- **Darklord 2 (`Darklord2Executor.cs` & `Darklord2Plugin.cs`)**:
  - เพิ่ม Deck Alias Attribute: `[Deck("Darklord2", "Darklord 2")]` รองรับการเรียกชื่อทั้งแบบมีและไม่มีช่องว่าง
  - แก้ไข `HeraldOfOrangeLightActivate`: เพิ่ม `lastCard.IsMonster()` ป้องกันการเผลอปาขัดเวทมนตร์หรือกับดัก
  - แก้ไข `Darklord Eveningstar Set Effect`: เดิมเลือก Trap 2 ใบพร้อมกัน (`Rebellion` + `Sanctified`) ทำให้ Engine ล็อก แก้เป็นเลือก **1 Spell (`Banishment`/`Contact`/`Dance`) + 1 Trap (`Sanctified`/`Rebellion`)** ตรงตามข้อกำหนดของการ์ด
  - แก้ไข `Darklord Contact` target ใน `ExecuteDarklordGyCopy` และการเปิดใช้งาน: ปรับจากรายการฮาร์ดโค้ดที่ตกหล่น `Tezcatlipoca` (2800 ATK) เป็น `c.IsMonster() && c.HasRace(CardRace.Fairy)` ครอบคลุมแฟรี่ทุกตัวในสุสาน
  - แก้ไข `ApexPolymerizationActivate`: ปรับเงื่อนไขจากเดิมที่ต้องการมอนสเตอร์ 2 ตัว เป็นมอนสเตอร์หงายหน้า 1 ตัวที่มีเลเวล
  - ปรับปรุง `Darklord2Plugin.cs`: ลำดับความสำคัญในการเสิร์ชให้เลือก `Djehuty` (Starter) ก่อนหากยังไม่มีบนมือ/สนาม และตรวจสอบวัตถุดิบ DARK Fairy $\ge 2$ ตัวก่อนเสิร์ช `DarklordDance`
- **Watenpai (`WatenpaiExecutor.cs` & `WatenpaiPlugin.cs`)**:
  - กำหนด `_isGoingSecond = true`: เด็คนี้ออกแบบเป็น Going-Second Breaker เต็มตัว (ใส่ Slumber x3, Raigeki x3, Dark Hole x3) เดิมเมื่อชนะทอยเหรียญจะเลือกเดินก่อนและตั้งรับไม่ทัน
  - แก้ไข `SangenKaimenActivate`: ปิดกั้นการเปิดใน Draw/Standby Phase และชะลอการเปิดหากมี `Interrupted Kaiju Slumber` บนมือ เพื่อป้องกัน Dragon Lock ก่อนการลง Kaiju / Watt
  - แก้ไข `OnSelectPosition`: ป้องกันการนำมอนสเตอร์พลังโจมตี 0 (`Dora Dora`, `TenpaiDragonGenroku`, `Wattdragonfly`) ลงในสภาพโจมตี
  - แก้ไข Priority และ Allocation ของ `Interrupted Kaiju Slumber`:
    - สลับลำดับให้ Slumber มีความสำคัญสูงกว่า Raigeki/Dark Hole เพื่อไม่ให้บอร์ดโล่งจน Slumber ใช้งานไม่ได้
    - ใน `OnSelectCard`: บังคับบอทเลือกรับ `Thunder King Kaiju` (3300 ATK) มาไว้บนสนามตนเอง และมอบ `Kumongous Kaiju` (2400 ATK) หรือ `Radian` ให้คู่แข่ง
  - ปรับปรุง `WatenpaiPlugin.cs`: เพิ่ม Search Handler สำหรับ `SangenKaimen` (66730191) ให้ค้นหา `TenpaiDragonGenroku` เพื่อโดดพิเศษลงสนามฟรีโดยไม่เสีย Normal Summon

### 3. Verification & Matchup Simulation Results
- **Headless Text Duel Simulator (`Client_Headless_Fortest`) เทียบกับเด็ค `BlueEyes`**:
  - **Watenpai vs BlueEyes**: 5 เกมเต็ม ชนะ 2 แพ้ 3 (Win Rate 40.0%), **0 Violations / 0 Crashes**, ทำ OTK ในเทิร์น 4 ได้อย่างหมดจด
  - **Darklord 2 vs BlueEyes**: 5 เกมเต็ม ชนะ 2 แพ้ 3 (Win Rate 40.0%), **0 Violations / 0 Crashes**, เรียก `The First Darklord` และคุมบอร์ดจนชนะในเทิร์น 7 และเทิร์น 10
- **Build & Exclusive Deployment**:
  - รัน `BUILD_AND_DEPLOY.ps1` อัปเดตไบนารีและดาต้าเบสทั้งหมดมาที่ `C:\Users\admin\Documents\EdoGame\` โดยตรง (100% สำเร็จ)
  - ซิงค์คู่มือวิเคราะห์และ Playbook สู่ `C:\Users\admin\Documents\EdoGame\Docs\Watenpai_And_Darklord2_Analysis_And_Playbook.md`

---

## 0.083. Watenpai (Watt x Tenpai) & Darklord 2 Modern Architecture, Decoupled Plugins & Full Deployment (2026-10-03)

### 1. Archetype Optimization & Human vs. Bot Dimension Analysis
- **Watenpai Deck (`Watenpai.ydk`)**:
  - **Human vs. Bot Analysis**:
    - มนุษย์เล่นการ์ดสุ่มอย่าง `Magical Mallet` หรือ `Lightning Vortex` ด้วยความรู้สึก (intuition) เพื่อลุ้นดวงหรือทิ้งการ์ดตามสัญชาตญาณ แต่สำหรับ Rule-Based Bot การ์ดเหล่านี้เป็น **Inherent -1 Trap** ที่บอทประเมินมูลค่าทรัพยากรผิดพลาดจนมือขาด
    - มนุษย์ทิ้งการ์ดเพื่อหวังผลหน้างาน แต่บอทต้องการ **Deterministic Board States & Consistent Combo Bridges**
  - **Deck Refactoring (Main 40 / Extra 15)**:
    - ตัด: 3x `Magical Mallet` (ขาดทุนการ์ดบนมือ), 3x `Lightning Vortex` (เปลืองการ์ดทิ้ง)
    - เพิ่ม: 2x `Tenpai Dragon Paidra` + 1x `Sangen Summoning` + 1x `Sangen Kaimen` (เพื่อเสริมความเสถียรของเครื่องยนต์ Tenpai ให้ขึ้นมือบ่อย), 1x `Wattuna` (Tuner หัวใจหลักของสาย Watt), 1x `Called by the Grave`
    - ขยาย Extra Deck จาก 6 ใบเป็น 15 ใบสมบูรณ์แบบ: `Wattkyuki` (L8) x2, `Watthydra` (L7) x2, `Bident Dragion` (L7) x2, `Trident Dragion` (L10) x1, `Sangenpai Transcendent Dragion` (L10) x1, `Black Rose Dragon` x1, `Kuibelt the Blade Dragon` x1, `Hieratic Seal of the Heavenly Spheres` x1, `Super Starslayer TY-PHON` x1, `Garura` x1, `Mudragon` x1, `S:P Little Knight` x1
- **Darklord 2 Deck (`Darklord 2.ydk`)**:
  - **Human vs. Bot Analysis**:
    - มนุษย์เล่น Darklord มักจะเดาจังหวะเพื่อกัก `Herald of Orange Light` / `Herald of Green Light` หรือตัดสินใจชุบตัวไหนตามอารมณ์เกม
    - บอทต้องการ O(1) Rule-based Evaluation ในการเลือกจ่าย 1,000 LP ก๊อบปี้เวท/กับดัก Darklord จากสุสาน **โดยไม่ต้องจ่าย Monster Cost ดั้งเดิมของการ์ด** (Free Resource Cheat)
  - **Deck Refactoring (Main 40 / Extra 15)**:
    - ปรับ Main Deck ให้เป็น 40 ใบพอดี ตัดการ์ดล้นมือ
    - Extra Deck ขยายเป็น 15 ใบ โดยเพิ่ม `Condemned Darklord` (Link-2) ช่วยสละมอนสเตอร์ในสุสานแทนเครื่องเซ่น Advance Summon และเสิร์ช Darklord เข้ามือ

### 2. Strategy & Ace Monster Tactics (Going 1st vs Going 2nd)
- **Watenpai**:
  - **Ace Monsters**:
    - **Finisher Ace 1 (`Trident Dragion`)**: 6,000 ATK x 3 Attacks = 18,000 Damage OTK ผ่าน Tenpai Engine
    - **Direct Burst Ace 2 (`Wattkyuki` & `Watthydra`)**: คอมโบตีตรงต่อเนื่อง Wattcobra (1000) -> โดด Wattuna (800) ตีตรง -> แท็ก Wattkyuki (1600) ตีตรง -> แท็ก Watthydra (1500) ตีตรง = 4,900+ Direct Damage ข้ามมอนสเตอร์ศัตรูทั้งหมด
    - **Board Wipe Ace (`Interrupted Kaiju Slumber`)**: เคลียร์บอร์ดศัตรู แจก Kumongous (2400) ให้ศัตรู และบอทได้ Jizukiru (3300) ทุบซ้ำ
    - **Going 1st Ace (`Hieratic Seal` / `Sangenpai Transcendent Dragion`)**: ยืนบอร์ดขัดขวางและข้าม MP1 ด้วย `Sangen Kaiho`
  - **Turn 1 (Going First)**:
    - ตั้ง `Hieratic Seal` หรือ `Transcendent Dragion` (3000 DEF) เซ็ต `Sangen Kaiho` / Handtraps
  - **Turn 2 (Going Second OTK)**:
    - Kaiju Slumber ล้างสนาม -> กาง `Sangen Summoning` ป้องกันมังกรไฟ -> ตีตรงด้วยสาย Watt หรือทำ Synchro Ladder ขึ้น Trident 6000 ATK ปิดเกม
- **Darklord 2**:
  - **Ace Monsters**:
    - **Ultimate Fusion Ace (`The First Darklord` - 4000/4000)**: ทำลายการ์ดบนสนามศัตรูทั้งหมดหากใช้ Morningstar ฟิวชั่น, ป้องกันมอนสเตอร์เผ่าแฟรี่บนสนามเราไม่ให้ตกเป็นเป้าหมายเอฟเฟกต์ (Targeting Immunity), และ Quick Effect ชุบแฟรี่ในสุสานลงมาในสภาพตั้งรับ
    - **Disruption Copier Aces (`Darklord Ixchel`, `Darklord Tezcatlipoca`, `Darklord Nasten`, `Darklord Eveningstar`)**: จ่าย 1,000 LP ก๊อบปี้ `Darklord Rebellion` (ทำลายการ์ดศัตรู) หรือ `The Sanctified Darklord` (ลบล้างเอฟเฟกต์มอนสเตอร์ + เพิ่ม LP เท่า ATK) จากสุสานได้ฟรีในเทิร์นของใครก็ได้
    - **Handtrap Fairy Engines (`Herald of Orange Light` / `Herald of Green Light`)**: ทิ้งตัวเอง + ดาร์กลอร์ดบนมือ ขัดขวางมอนสเตอร์และเวทมนตร์ของศัตรู
  - **Turn 1 (Going First)**:
    - ใช้ `Darklord Ixchel` / `Banishment of the Darklords` จั่วและทิ้ง `Darklord Rebellion` / `The Sanctified Darklord` ลงสุสาน
    - ฟิวชั่นเรียก `The First Darklord` หรือชุบ `Darklord Eveningstar` / `Darklord Tezcatlipoca` ยืนบอร์ดพร้อมเปิดก๊อบปี้ Trap ขัดขวาง 2-3 จังหวะ
  - **Turn 2 (Going Second)**:
    - ใช้ Herald ขัดขวางเทิร์นแรกของศัตรู -> ฟิวชั่น `The First Darklord` ล้างสนามทั้งหมด -> โจมตี 4,000+ ATK ปิดเกม

### 3. Decoupled Plugins & Executors Created
- **Watenpai**:
  - `WatenpaiPlugin.cs`: จัดการลำดับการทิ้งการ์ด, คำนวณ Watt direct damage, ลำดับการเรียก Wattuna/Wattkyuki/Watthydra
  - `WatenpaiExecutor.cs`: Subclass `ModernExecutor`, `[Deck("Watenpai", "Watenpai")]`, จัดการ Battle Phase Chain Synchro และการกาง Field Spell
- **Darklord 2**:
  - `Darklord2Plugin.cs`: จัดการ Fairy discard engine, ลำดับการเลือกเป้าหมายชุบชีวิต, จัดการ GY copy spell/trap
  - `Darklord2Executor.cs`: Subclass `ModernExecutor`, `[Deck("Darklord 2", "Darklord 2")]`, จัดการ Quick Effect GY copy (Rebellion & Sanctified), Contact, Banishment, Greater Polymerization, Morningstar / First Darklord

### 4. Registration & Build/Deploy
- **`bots.json`**: ลงทะเบียน `"Watenpai"`, `"Darklord 2"`, และ `"Darklord2"` (Difficulty 3, Master Rules 4, 5, Flags: OCG/TCG)
- **Deployment Target**:
  - `C:\Users\admin\Documents\EdoGame\WindBot\WindBot.dll`
  - `C:\Users\admin\Documents\EdoGame\WindBot\ExecutorBase.dll`
  - `C:\Users\admin\Documents\EdoGame\WindBot\core.dll`
  - `C:\Users\admin\Documents\EdoGame\WindBot\bots.json`
  - `C:\Users\admin\Documents\EdoGame\deck\Watenpai.ydk`
  - `C:\Users\admin\Documents\EdoGame\deck\Darklord 2.ydk`
  - `C:\Users\admin\Documents\EdoGame\WindBot\Decks\Watenpai.ydk`
  - `C:\Users\admin\Documents\EdoGame\WindBot\Decks\Darklord 2.ydk`
  - `C:\Users\admin\Documents\EdoGame\DashBot.exe`

---

## 0.082. Tenpai Dragon Complete Architecture Refactor, Decoupled Plugin & OTK Optimization (2026-10-03)

### 1. Archetype Audit & Correct Mathematical Calibration
- **Card Level & Math Verification**:
  - แก้ไขความเข้าใจผิดดั้งเดิมที่คิดว่า Paidra และ Fadra เป็น Level 4: ตรวจสอบจาก `cards.cdb` ยืนยันว่า **Paidra (Level 3)**, **Fadra (Level 3)**, **Genroku (Level 3)**, และ **Chundra (Level 4 Tuner)**
  - แก้ไขบันไดการจูน Synchro ที่แท้จริง:
    - **Step 1 (Level 7 Tuner)**: Chundra (4 Tuner) + Paidra/Fadra (3 non-Tuner) = `Sangenpai Bident Dragion` (Level 7 FIRE Dragon Tuner, 2600 ATK)
    - **Step 2 (Level 10 Boss)**: Bident Dragion (7 Tuner) + Paidra/Fadra (3 non-Tuner ที่ Bident ชุบขึ้นมาจากสุสาน) = `Trident Dragion` (Level 10, 3000 ATK) หรือ `Sangenpai Transcendent Dragion` (Level 10, 3000 ATK/3000 DEF)
- **Extra Deck Modernization**:
  - ถอด `Hi-Speedroid Chanbara` (Level 5 Machine ซึ่งเด็คนี้ไม่มีตัวจูน Level 1/2 และติด Dragon Lock) ออก
  - ใส่ **`Black Rose Dragon` (73580471)**: Level 7 FIRE Dragon Synchro สำหรับกวาดล้างสนามฉุกเฉินเมื่อเดินหลัง
  - ใส่ **`Hieratic Seal of the Heavenly Spheres` (24361622)**: Link-2 Dragon (0 ATK) มอนสเตอร์ขัดขวางหัวใจหลักเมื่อถูกบังคับเดินก่อน (Going First)

### 2. Strategy & Ace Monster Tactics (Going 1st vs Going 2nd)
- **Ace Monster Roles**:
  - **Finisher Ace (`Trident Dragion`)**:
    - สั่งระเบิด `Sangen Summoning` (Field Spell) + การ์ดส่วนเกิน 1 ใบ -> Trident ได้สิทธิ์ตี 3 ครั้ง
    - เอฟเฟกต์สุสานของ `Sangen Summoning` ทำงานเมื่อถูกทำลายใน Battle Phase -> เพิ่ม ATK ของ Trident Dragion เป็น **2 เท่า กลายเป็น 6,000 ATK**!
    - โจมตี 3 ครั้งที่ 6,000 ATK = **18,000 Damage OTK ทันที**!
    - ห้ามอัญเชิญ Trident Dragion ใน Turn 1 (Going First) เด็ดขาด
  - **Lockdown Ace (`Sangenpai Transcendent Dragion`)**:
    - เมื่อลงสนาม เปลี่ยนมอนสเตอร์ทุกตัวเป็นหงายหน้าโจมตี บังคับศัตรูต้องสั่งตี และที่สำคัญที่สุด: **ศัตรูไม่สามารถเปิดเอฟเฟกต์การ์ดใดๆ ใน Battle Phase ได้เลย (Opponent Silent in BP)**
  - **Bridge Ace (`Sangenpai Bident Dragion`)**:
    - เมื่อ Synchro ชุบ Paidra/Fadra (Level 3) ขึ้นมาเพื่อต่อยอดเป็น Level 10 ทันที
    - ในสุสาน หากมีการประกาศโจมตีครบ $\ge 3$ ครั้ง สามารถโดดตัวเองกลับมาสนามและทำลายเวท/กับดักศัตรู 1 ใบ
- **Going First (Turn 1 Strategy)**:
  - วางบอร์ดเพื่อเอาชีวิตรอดและสะสมทรัพยากร: ลง Paidra หา `Sangen Kaiho` (Trap) หรือ `Sangen Kaimen`
  - ทำ Link-2 `Hieratic Seal of the Heavenly Spheres` หรือทำ `Sangenpai Transcendent Dragion` (3000 DEF)
  - เซ็ต `Sangen Kaiho` (เมื่อศัตรูมีมอนสเตอร์มากกว่า ข้าม Main Phase 1 ของศัตรูตรงเข้า Battle Phase ทันที) ร่วมกับ Super Poly / Droplet / Impermanence
- **Going Second (Turn 2+ OTK Strategy)**:
  - Main Phase 1: เคลียร์บอร์ดด้วย Dark Ruler No More, Lightning Storm, Super Poly, Droplet
  - เปิด `Sangen Summoning` รับ Blanket Protection: มอนสเตอร์มังกรไฟทั้งหมดไม่รับผลการ์ดที่ถูกเปิดใช้งานของศัตรูใน MP1
  - กาง Paidra + Chundra แล้วเข้า Battle Phase เพื่อรัน OTK Synchro Ladder อย่างปลอดภัย

### 3. Decoupled Domain Plugin Architecture (`TenpaiPlugin`)
- สร้าง `TenpaiPlugin : DeckPluginBase` รองรับ:
  - `TenpaiStrategy : IDeckStrategy`: เลือกเป้าหมายเสิร์ชและชุบชีวิตตามบริบทเกม
  - `TenpaiMaterialEvaluator : IDeckMaterialEvaluator`: จัดลำดับการทิ้งการ์ดและเลือกระเบิด Sangen Summoning สำหรับ Trident
  - `TenpaiThreatEvaluator : IDeckThreatEvaluator`: ประเมินภัยคุกคามของการ์ดคู่แข่ง
  - `TenpaiBattleOTKPlanner`: คำนวณพลังโจมตีและตรวจสอบ Lethal Damage
- เชื่อมต่อ `DeckPlugin = Plugin;` เข้าสู่ Central Core

### 4. Registration & Exclusive Deployment
- **`bots.json`**: ลงทะเบียน `"Tenpai"`, `"Tenpai Dragon"`, `"TenpaiDragon"` (Difficulty 3, Master Rules 4, 5, Flags: OCG/TCG) ครบถ้วน
- **Deploy**: คอมไพล์และติดตั้งไบนารี (`WindBot.dll`, `ExecutorBase.dll`, `core.dll`, `bots.json`, `Tenpai.ydk`, `DashBot.exe`) สู่ `C:\Users\admin\Documents\EdoGame\` ผ่าน `BUILD_AND_DEPLOY.ps1` สมบูรณ์ 100%

---
