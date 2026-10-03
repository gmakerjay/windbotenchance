# Known Core Issues & Pitfalls (Audit 2026-10-03 v0.086 → Pro Audit 2026-10-04 v0.094)

> สถานะข้อบกพร่องใน Central Core (`windbot-fork/ExecutorBase`) ที่พบตอน audit skill
> ✅ = แก้ใน core แล้ว — เด็คไม่ต้องหลบอีก · ⚠️ = ยังเป็นพฤติกรรม/ข้อจำกัดของ core ที่ต้องรู้
> รายงานเต็ม v0.094: `Docs/CORE_PRO_AUDIT_2026-10-04.md`
> Card ID ทุกตัวตรวจกับ `cards.cdb` + `WindBot\*.cdb` แล้ว

## ✅ แก้แล้วใน v0.086

| # | ไฟล์ | ปัญหาเดิม | แก้เป็น |
|---|---|---|---|
| 1 | `ModernExecutor.OnSelectCard` | `507` (TODECK) ถูกมองเป็น equip alias → คืน Ace ฝั่งเรากลับเด็ค | 507 ศัตรู = removal; 507 ฝั่งเรา → branch cost (`PickDiscardTarget`/`SortMaterials` = ตัวค่าต่ำสุด) |
| 2 | `ModernExecutor.OnSelectCard` | `518` (EQUIP) อยู่ใน branch POSCHANGE | POSCHANGE = 528 เท่านั้น, EQUIP = 518 เท่านั้น |
| 3 | `ModernExecutor.OnSelectCard` | `552` (COIN) อยู่ใน removal branch | ลบออก |
| 4 | `ModernExecutor.OnSelectCard` | 504 Deck→GY ใช้ `PickDiscardTarget` (เลือกตัวค่าต่ำสุด) | 504 ที่ candidate อยู่ใน Deck ทั้งหมด → `Strategy.PickSearchTarget(cards, Card)` (positive pick); plugin คืน null → ตกไป `AI.SelectCard`/Fallback |
| 5–7 | `ModernExecutor` / `DefaultExecutor` / `AntiFloodgateHelper` / `CardIntelligence` | Bagooska ใช้ ID ผิด (`26593852` Catastor, `26273196` Time Wizard of Tomorrow, `85359414` Freezing Beast) | ใช้ `90590303/90590304` เท่านั้น + helper `IsBagooska(card)` (protected static) ใน repos / position |
| 8 | `ModernExecutor.ShouldAllowActivate` | MP2 guard อยู่หลัง `Phase != Main1 return true` → ไม่เคยทำงาน | ย้ายขึ้นก่อน: MP2 + `IsBoardStrongEnough()` → บล็อก `ResourceGain`/`ComboStarter` (log `[PHASE-GUARD]`) |
| 9 | `BUILD_AND_DEPLOY.ps1` + `configs/*.json` | configs ไม่ถูก deploy และมี ID ปลอม 11+10 ตัว | deploy → `EdoGame\WindBot\configs\`; สร้าง list ใหม่จาก cdb |
| 10 | `CardTextSemantics.cs` | `HINTMSG_TARGET = 506` | `551` |
| 11 | `Executor.FallbackSelectCard` | 519/552/572 ถูกมองเป็น "เลือกศัตรู", 508 ถูกมองเป็น cost | Case1: 502/503/504/505/507/520/528/549/551/575 · Case2 (cost): 500/501/504(ไม่ใช่ Deck)/507(เรา)/511/512/513/519/531/533 · Case3 (positive): 506/508/509/505(เรา)/504(Deck→GY) |
| 12 | `ComboRouter.GetViableLines` | ไม่กรอง `_failedLinesThisTurn` | กรองแล้ว — เส้นที่ Abort จะไม่ถูกเลือกซ้ำในเทิร์นเดียวกัน |
| 14 | `ModernExecutor.SmartHandTrapChain` | overload `params int[]` ไม่มี duplicate guard; overload ปกติเช็คหลัง fallback | ทั้งสอง overload เรียก `IsDuplicateOwnChainActivation()` เป็นอันดับแรก |
| 17 | `dashbot/MainWindow.xaml.cs` | `Watenpai`, `Darklord 2` ไม่อยู่ใน `ModernArchetypes` | เพิ่มแล้ว (`Watenpai`, `Darklord 2`, `Darklord2`) |
| 18 | `Darklord2Executor` / `Darklord2Plugin` | reset ซ้ำ 4 ครั้งต่อเทิร์น | ลบ reset ซ้ำ (ModernExecutor.OnNewTurn → `DeckPlugin?.ResetTurnState()` ทำให้แล้ว) |
| 19 | **Card ID ปลอมทั่ว core** (ใหม่) | ~140 entries ใน `CardIntelligence`, `ModernExecutor`, `AntiFloodgateHelper`, `ChainTimingAdvisor`, `ThreatAnalyzer`, `DefaultExecutor`, `BoardScorer`, `BaitPlanner`, `DynamicValueEvaluator`, `HeuristicGuard`, `CardExtension`, `AIBrain` ใช้ ID ที่ไม่มีจริงหรือเป็นการ์ดอื่น เช่น Megamorph = "Rivalry of Warlords", Necrovalley = "Summon Limit", Accesscode Talker = "Herald of Ultimateness", Buster Blader = "Vanity's Fiend" | แก้ทุก entry ตามชื่อในคอมเมนต์ (ค้นจาก cdb); ลบ "(alt)" ที่ไม่มี alias จริง; ผลคือ floodgate / negator / handtrap / chokepoint detection ทำงานจริงครั้งแรก |

> 🔁 วิธีตรวจซ้ำ: `python tools/audit_card_ids.py windbot-fork/ExecutorBase` (หรือพาธไฟล์เด็ค) — รันทุกครั้งที่เพิ่ม ID · core เหลือ 13 hit ที่ไม่ใช่ ID (คะแนน 100000/999999/11000, token, `NamedCard`) · รายละเอียดใน `Docs/SKILL_AUDIT_2026-10-03.md` § Card-ID Audit
> ⚠️ โค้ดเด็ค (`Game/AI/Decks`, `Plugins`) ยังมี hit ~236 จุด (บางส่วน false positive) — ตรวจไฟล์เด็คก่อนแก้ทุกครั้ง

| 20 | `CardIntelligence` / `AntiFloodgateHelper` / `ModernExecutor` | `IsSpecialSummonBlocked` นับ floodgate ทุกตัว (Bagooska, Majesty's Fiend, Inspector Boarder ฯลฯ) รวมเป็น SS lock | แยก `SpecialSummonLockMonsters` และ `SpecialSummonLockSpellsTraps` เฉพาะตัวที่ล็อก SS จริง + เช็ค Vanity's Ruler ฝั่งเราไม่บล็อกตัวเอง |
| 21 | `ModernExecutor.OnSelectSynchroMaterial` | Synchro material (512) ไม่ผ่าน `MaterialEvaluator` | Implement overloads ส่งต่อให้ `DeckPlugin.MaterialEvaluator.SortMaterials` ครบถ้วน |
| 22 | `ModernExecutor.ShouldAllowActivate` (MP2 guard) | MP2 guard บล็อก search/draw ทุกตัวเมื่อบอร์ดแข็งพอ ปรับแต่งไม่ได้ | เพิ่ม `EnableMP2Guard` (bool) และ virtual method `ShouldAllowMP2Activation(ClientCard card)` ให้ override ได้ |
| 23 | `ModernExecutor.OnSelectCard` (504 Deck→GY) | 504 Deck→GY อาจส่ง starter ลงสุสานแทน foolish target | เพิ่ม `IDeckStrategy.PickFoolishGraveTarget`, `GameAI.HasPreselectedCard`, ตรวจสอบ preselected ก่อนส่งเข้า `PickFoolishGraveTarget` |
| 24 | `ComboRouter` ↔ `OnSelectIdleCmd` | ComboRouter step ยิงตรงไม่ผ่าน func เด็ค และไม่เช็ค phase guards | ตรวจสอบ `step.Condition`, Phase Guards (`ShouldAllow*`), และ `CardExecutor.Func` ก่อนยิงคำสั่ง |
| 25 | `Executor.IsAceCard` | default นับ Extra Deck ทุกใบเป็น Ace | ตรวจสอบ `HeuristicGuard.IsAceCard`, `CardIntelligence.IsKnownNegator`, ATK $\ge 3000$, หรือ Link $\ge 4$ เท่านั้น |
| 26 | Card ID ปลอม/ผิดการ์ดในระดับเด็คและปลั๊กอิน (236 จุด) | RaiOh, Invoke, Blackwings, EneaCraft, Radiant, Dinomorphia, DogmaStun, Labrynth, ModernBlueEyes, Toon, TrirealmRift, Plugins | แก้ครบทุกจุด + ปรับปรุง `tools/audit_card_ids.py` → **0 issue(s) in 243 file(s)** |

## ✅ แก้แล้วใน v0.094 (Central Core Pro Refactor)

| # | ไฟล์ | ปัญหาเดิม | แก้เป็น |
|---|---|---|---|
| 27 | `ModernExecutor.OnSelectIdleCmd` | `Analysis` refresh เฉพาะต้นเทิร์น/เฟส → อ่านบอร์ดค้างทั้ง Main Phase | `Analysis?.Refresh()` ทุก idle prompt |
| 28 | `DecisionContext.ShouldActivate` / `ThreatAnalyzer` | state จับตอน OnNewTurn (0/0); คะแนน activation ตาม location (~10) → Counterfactual ถือ Ash/Veiler ไว้เกือบทุก starter | `UpdateState(Brain.OwnSummons, Brain.OpponentSummonCount)` ก่อนตัดสิน + `GetActivationThreatScore` (engine text / Extra Deck / one-shot spell / disabled) |
| 29 | `ChainTimingAdvisor` | early-combo −10 ทำให้ starter ที่ไม่รู้จักได้ 40 (< 45) → แทบไม่ Ash starter; Eater of Millions อยู่ใน handtrap; Meluseek ID ผิด (53143898 = Marionetter) | unknown engine +15, early −5 (score < 60); ลบ Eater 63845230; Meluseek = 25533642; `CountInteractiveCards` ใช้ `CardIntelligence.IsHandtrap` |
| 30 | `GameAI` / `CardSelector` / `ModernExecutor.OnSelectCard` | `AI.SelectCard` ถูกข้ามใน hint ที่ route (Called by CL3, `DefaultPreemptiveImpermanence`, deck target) แล้ว preselect ค้างไป hijack prompt ถัดไป | step 0 `TryConsumePreselectedCards` (match-only, explicitOnly สำหรับ cost); `PreselectCheckpoint/RollbackPreselect` เมื่อ func คืน false / skip duplicate chain; ล้าง selector ทุก idle prompt |
| 31 | `ModernExecutor.OnSelectCard` | plugin คืน 1 ใบแต่ min > 1 → GameAI เติมแบบสุ่ม | `CompleteSelection(primary, orderedPool, min)` เติมตามลำดับ heuristic (popSub, foolish, 504/506/509 plugin) |
| 32 | `ModernExecutor` ↔ `ComboRouter` | step เรียก executor ตัวแรก (อาจเป็น generic -1) ไม่ตั้ง `ActivateDescription`; fallback line ยิง step ที่ทำแล้วซ้ำ; Maxx "C"/handtrap ใดๆ = negate → Abort | `ComboStepApprovedByExecutors` (exact-ID ก่อน, desc จริง, rollback); per-turn `_executedThisTurn` + `ApplyExecutedHistory`; `IsOpponentCardNegator` เฉพาะ negate จริง; backrow นับใน RequiredCards |
| 33 | `CardTextSemantics` / `CardIntelligence` | "cannot be destroyed by battle" ถูกนับเป็น effect-destruction immune (−20000 ใน 502) | `RegexDestructionImmune` effect-only; `IsBattleImmune` แยก; 502 กรอง immune เมื่อตัวเลือกพอ |
| 34 | `CardIntelligence.GetCardThreatScore` | negator/floodgate ที่ถูก disable แล้วยังได้โบนัสเต็ม | ×0.5 เมื่อ face-up + `IsDisabled()` |
| 35 | `GameAI.ShouldExecute` / `ModernExecutor` | duplicate-handtrap guard นับการ์ดศัตรู (Imperm war); Eater ถูกมองเป็น Veiler/disruption | นับเฉพาะ `Controller == 0`; ลบ Eater ออกจาก Veiler/CountDisruptions; เพิ่ม Ash alt 14558128 |
| 36 | `ModernExecutor.OnSelectPosition` | มอน ATK สูงตกไป DEF ใน MP1 ทั้งที่กดดันได้ | MP1 (Turn > 1), ATK ≥ 1000 & ATK ≥ DEF → ATK เมื่อศัตรูว่างหรือมีเป้า face-up ที่ตีชนะ |

## ⚠️ ข้อจำกัด / พฤติกรรมของ core ที่ยังต้องรู้

| # | จุด | พฤติกรรม | สิ่งที่เด็คต้องทำ |
|---|---|---|---|
| 16 | `HeuristicGuard._aceCardIds` (static) | ไม่ถูกล้างระหว่างดวล/บอท | headless 2 บอทใน process เดียวแชร์รายการ Ace — ยอมรับได้ |
| 37 | `ModernExecutor._negateNever` | ยังมี Terraforming / RotA (ขัดกับ ChainTimingAdvisor ที่นับเป็น starter) | ถ้าเด็คอยาก Ash การ์ดกลุ่มนี้ → handle ใน func เอง |
| 38 | `DefaultExecutor` (`Util.ChainContainsCard`) | handtrap duplicate check ไม่แยก controller → Imperm ศัตรูในเชนทำให้เราไม่ตอบ Imperm | ใช้ `IsDuplicateOwnChainActivation()` / `SmartHandTrapChain()` ของ ModernExecutor |
| 39 | `ModernExecutor.OnSelectCard` hint 551 | เลือกศัตรูเสมอ (บัฟฝั่งเราผิดเป้า) | ตั้ง `AI.SelectCard(ourCard)` ก่อน `return true` (v0.094 honour แล้ว) |
| 40 | `HeuristicGuard.SanitizeSelection` | อาจทับ preselect ที่ตั้งใจเล็งตัวเอง (เช่น ทำลายตัวเองเพื่อ float) | ใช้ Pending-Select + ตรวจ log `[GUARD]` |
| 41 | removal branch (`IsTargetImmune`) | กรอง target-immune แม้ prompt ไม่ได้ target (เช่น mass destroy) | ยอมรับได้ — กรณีพิเศษ override `OnSelectCard` |
| 42 | `OnSelectAttackTarget` / battle | เลือกเป้าตีแบบ greedy (ไม่คำนวณ order ทั้ง Battle Phase) | เด็ค OTK ใช้ `OnSelectAttacker`/`OnPreBattleBetween` ช่วย |
| 43 | `GameAI.ShouldExecute` checkpoint | rollback สมมติว่า func ใช้ `SelectNextCard` หลัง `SelectCard` เท่านั้น | ตั้ง preselect เฉพาะตอนกำลังจะ `return true` |
| 44 | `OnSelectPendulumSummon` | hint 509 ถูก ModernExecutor route ก่อน → callback นี้แทบไม่ถูกเรียก | เด็ค Pendulum ใช้ `PickSpecialSummonTarget` / override `OnSelectCard` |
| 45 | `PreNewTurn` / `PreNewPhase` | ถูกเรียก 2 ครั้งต่อ event (idempotent) | อย่าใส่ counter ที่ไม่ idempotent ใน callback นี้ |
