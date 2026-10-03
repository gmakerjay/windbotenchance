# Core Module API & Registration Map (ตรวจกับ source เมื่อ 2026-10-03)

> Path หลัก: `windbot-fork/ExecutorBase/` · ทุก module ด้านล่างถูกสร้างอัตโนมัติใน `ModernExecutor` constructor
> คอลัมน์ **Auto** = core เรียกให้เองไหม · **ต้องทำ** = สิ่งที่ deck ต้อง register/เรียกเอง ถึงจะได้ผล

## 0. ภาพรวมการเชื่อมต่อ

```text
GameAI ──► Executor.OnSelectIdleCmd (ModernExecutor)
             ├─ DynamicLethalCheck()
             ├─ ComboRouter.ActivateBestLine → execute step ตรงๆ (⚠️ ข้าม executor func)
             ├─ ShouldAttackBeforeCombo / ShouldRushAttack / ShouldStopExtending / ShouldBattleBeforeSetting
             └─ null → GameAI วน AddExecutor ตามลำดับ (ShouldAllow* guard → func)
GameAI ──► OnSelectCard → deck override → ModernExecutor (hint routing → DeckPlugin) → selector → Fallback → HeuristicGuard
OnNewTurn → ComboRouter/BaitPlanner OnNewTurn + DeckPlugin.ResetTurnState()
OnChaining(opp) → OpponentProfile.OnOpponentActivate + BaitPlanner.OnOpponentChainResponse + combo-negate detect
```

> ⚠️ `ModernExecutor` constructor เรียก `AddExecutor(ExecutorType.GoToEndPhase, ShouldPassTurn)` **เป็นตัวแรก** →
> GoToEndPhase ถูกเช็คก่อนทุก executor ของเด็ค (Turn 1 MP1 เท่านั้น เมื่อ `IsBoardStrongEnough()` และไม่มี action เหลือ)
> ถ้าบอทจบเทิร์นเร็วเกิน → override `IsBoardStrongEnough()` / `CountDisruptions()` / `ShouldPassTurn()`

## 1. Plugin Layer (`ExecutorBase/Game/AI/Plugin/`)

| Interface | Signature | Auto เรียกจาก | หมายเหตุ |
|---|---|---|---|
| `DeckPluginBase` | `abstract string DeckName`; `virtual ResetTurnState()` = `Strategy?.Reset(); ResourceEvaluator?.Reset();` | `ModernExecutor.OnNewTurn` | ❌ อย่าเรียก reset ซ้ำใน executor; override เฉพาะเมื่อมี state เพิ่ม |
| `IDeckStrategy` | `void Reset()`; `ClientCard PickSpecialSummonTarget(IList<ClientCard>)`; `ClientCard PickSearchTarget(IList<ClientCard>, ClientCard context)`; `ClientCard PickFoolishGraveTarget(IList<ClientCard>, ClientCard context)` | Hint 509 / 506 / 505(ฝั่งเรา) / **504 Deck→GY** (branch 1.8 เรียก `PickFoolishGraveTarget` ก่อน) | คืน `null` = ให้ core heuristic ตัดสิน; `HasPreselectedCard()` ตรวจสอบว่ามีการ์ดผ่าน `AI.SelectCard` หรือไม่ |
| `IDeckMaterialEvaluator` | `int GetMaterialCost(ClientCard)`; `IList<ClientCard> SortMaterials(IList<ClientCard>, int min=1)`; `ClientCard PickDiscardTarget(IList<ClientCard>, int min=1)`; `ClientCard PickDestructionSubstitute(IList<ClientCard>, int min=1)` | 511/513/533 materials, 512 (Synchro materials), 500/501/504(ฝั่งเรา ไม่ใช่ Deck)/507(ฝั่งเรา) cost, 502 เมื่อศัตรูไม่พอ | รองรับ Synchro (512) ผ่าน `SortMaterials` แล้ว |
| `IDeckThreatEvaluator` | `int EvaluateThreatScore(ClientCard)`; `bool IsEmergencyThreat(ClientCard)` | `GetCardThreatScore` → ทุก removal branch | `IsEmergencyThreat` **ไม่มี core เรียก** — ใช้เองใน executor |
| `IDeckResourceEvaluator` | `void Reset()`; `int GetAvailableResourceCount()`; `bool CanSafelySpendResource(int)`; `IList<int> SelectCounters(int qty, IList<ClientCard>, IList<int>)` | `OnSelectCounter` | |
| `IDeckScaleResolver` | `ClientCard PickLowScale()`; `PickHighScale()`; `PickScalePopTarget()` | ❌ ไม่มี | เรียกเองใน activate func ของเด็ค Pendulum |
| `IDeckActionScorer` | `int CalculateActionScore(string actionName, int boardImpact, int netGain, int cost)` | ❌ ไม่มี | เรียกเองก่อนร่าย |

## 2. Enhancement Modules (properties ของ `ModernExecutor`)

### 2.1 `ComboRouter` (protected) — เลือกรูทจากมือ
- Register: `ComboRouter.RegisterLine(new ComboRouter.ComboLine { Name, RequiredCards, DesiredCards, Steps, EndBoardScore, Priority, FallbackLineName, RequiresSafeBoard, IsBaitLine, Condition })`
  หรือ shorthand `RegisterLine(name, int[] required, ComboStep[] steps, int endBoardScore, Func<bool> condition = null)`
- `ComboStep { CardId, ActionType, Description, Optional, Condition }` — ActionType ที่ทำงานจริง: **Activate / Summon / SpSummon / SpellSet** เท่านั้น
- Auto: MP1 เลือกเส้นที่ `RequiredCards` อยู่ใน **มือ+สนาม+สุสาน** ครบ และ `Condition()` ผ่าน → เรียง EndBoardScore ↓, Priority ↑ (ถ้าศัตรูมี disruption: IsBaitLine ก่อน, RequiresSafeBoard ท้าย)
- Step ไม่พร้อม → Optional = ข้าม / ไม่ Optional → `FallbackLineName` → ถ้าไม่ได้ Abort
- 🔒 **การตรวจสอบความปลอดภัยของ Step ใน `OnSelectIdleCmd`**: ตรวจสอบ `step.Condition`, Phase Guards (`ShouldAllowActivate`, `ShouldAllowSummon`, `ShouldAllowSpSummon`, `ShouldAllowSpellSet`), และฟังก์ชัน `CardExecutor.Func` ที่ตรงกันก่อนส่งคำสั่ง เพื่อไม่ให้ ComboRouter ข้าม guard และเงื่อนไขเฉพาะของเด็ค
- 🔒 ใส่ใน Step เฉพาะ action ที่ "ทำทันทีได้เสมอเมื่อการ์ดพร้อม" (เช่น Normal Summon starter, การ์ดเอฟเฟกต์เดียว) และใช้ `Condition` กรองสถานะ; ที่เหลือปล่อยให้ AddExecutor
- ปิดทั้งระบบได้: `ComboRouter.Enabled = false;`
- อื่นๆ: `IsNextStep(id)`, `IsPartOfActiveCombo(id)`, `GetComboOrder(id)`, `AbortCombo(reason)`, `HasActiveCombo`, `ActiveComboName`

### 2.2 `BaitPlanner` (protected) — ล่อ Handtrap ก่อนเดิน Starter
- Register: `RegisterComboStarters(params int[])`, `RegisterBaitCards(params int[])`, `RegisterNeverBait(int)`; มี Universal bait ในตัว (Pot of Prosperity/Desires/Duality, Terraforming, Harpie, Raigeki, Dark Hole, Book of Moon, Triple Tactics ฯลฯ)
- 🔒 **Register อย่างเดียวไม่ทำอะไร** — ต้องเรียก `GetBaitIfNeeded(Card)` ใน activate func ของ **Starter** เอง:
  ```csharp
  var bait = GetBaitIfNeeded(Card);
  if (bait != null && bait.Id != Card.Id) return false;   // เลื่อน starter → ให้ executor ของ bait ทำงานก่อน
  ```
  (ปัจจุบันมี 93 ไฟล์ register แต่มีแค่ 4 ไฟล์เรียก `GetBaitIfNeeded`)
- เงื่อนไข bait: card ∈ ComboStarters, มือ ≥ 2, ยังไม่ bait/ศัตรูยังไม่ตอบโต้เทิร์นนี้, ศัตรูมี disruption บนสนาม หรือโอกาสมี handtrap ≥ `BaitThreshold` (0.4), มี bait ในมือ
- ⚠️ `GetBaitCard` ตั้ง `_baitedThisTurn = true` ทันทีที่คืนการ์ด → bait card ต้องมี AddExecutor ที่ **อยู่ก่อน** starter และ func คืน true ได้ในสถานะนั้น ไม่งั้นเสียโอกาส bait ทั้งเทิร์น
- Bait ที่ดี = การ์ดที่ศัตรูอยากตัด แต่ถ้าโดนตัดเราไม่เสียคอมโบ (searcher รอง, Pot, extender ตัวที่ 2)

### 2.3 `ChainAdvisor` : `ChainTimingAdvisor` (protected) — จังหวะใช้ Handtrap
- Register: `RegisterHighValueTargets(params int[])` / `RegisterLowValueTarget(int)` (การ์ด**ศัตรู**ที่คุ้ม/ไม่คุ้มตัด)
- ใช้ผ่าน helper: `SmartHandTrapChain()` / `SmartHandTrapChain(params int[] chokepointIds)`
  → คืน false ถ้าไม่ใช่เชนศัตรู หรือ `ShouldHoldResponse` บอกให้เก็บไว้
- ✅ ทั้ง 2 overload เรียก `IsDuplicateOwnChainActivation()` (protected) เป็นอันดับแรก — การ์ดชื่อเดียวกันของเราอยู่ในเชนแล้ว → false (ใช้เรียกเองใน func ที่ไม่ผ่าน `SmartHandTrapChain` ได้)
- Config `configs/chain_targets.json` (ComboStarters/Extenders/LowValueTargets/HandTraps/ChainThreshold) — deploy → `EdoGame\WindBot\configs\` โดย `BUILD_AND_DEPLOY.ps1`; ID ตรวจกับ cdb แล้ว; `LoadConfig` merge แบบ UnionWith (เพิ่มอย่างเดียว)
- อื่นๆ: `EvaluateChainValue(...)` (0–100), `ShouldHoldResponseWithProfile(..., OpponentProfile, ...)`, `CountInteractiveCards(Bot)`

### 2.4 `ResourcePlan` : `ResourcePlanner` (protected) — Nibiru / Overextend / Ace usage
- Register: `RegisterAceCards(params int[])`; ปรับ `NibiruThreshold`, `NibiruHandThreshold`, `OverextensionMonsterThreshold`
- Auto: `ModernExecutor.ShouldStopExtending()` เรียก `ResourcePlan.ShouldStopExtending(...)`
- เรียกเอง: `NibiruCheckpoint(summonCount, oppHand, ourMonsters, haveNegate)`, `OverextensionRisk(...)`, `ShouldConserveResources(...)`, `CardEconomyScore(...)`, `EvaluateAceUsage(card, hasLethalIfUsed, isOnlyAnswerToThreat, haveAlternateWinCon)` → `(bool allowed, string reason)`

### 2.5 `OpponentProfile` : `OpponentProfiler` (public) — อ่านแนวเด็คศัตรู
- Auto feed: activate/draw/revealed (ไม่ต้อง register)
- ใช้: `DetectedArchetype`, `IsControlDeck()`, `IsComboDeck()`, `IsOTKDeck()`, `ShouldPlayConservative()`, `ShouldRushSetup()`, `EstimateHandTrapsInHand(handCount)`, `GetNextTurnThreatLevel(Enemy, turn)`
- `configs/archetypes.json` (KnownCardIds สร้างใหม่จาก cdb, deploy แล้ว) ใช้ตรวจ archetype — ยังไม่มีเด็คไหนอ่าน `DetectedArchetype`

### 2.6 `AIContext` : `DecisionContext` (protected) — Decision Engine
- ใช้ใน `DefaultAshBlossomAndJoyousSpring` / `DefaultEffectVeiler` / `DefaultBossNegate` ผ่าน `AIContext.ShouldActivate(card, lastChain, tag)`
- ไฟล์: `Game/AI/DecisionEngine/` (BeliefState, ThreatAnalyzer, DynamicValueEvaluator, ActionHistory, CounterfactualSimulator)

## 3. Static Services

| Class | API หลัก | ต้องทำ |
|---|---|---|
| `CardIntelligence` (static partial) | `IsFloodgate/IsFloodgateMonster/IsFloodgateSpellTrap/IsDrawStandbyFloodgate`, `IsKnownNegator`, `IsHighThreatChokepoint`, `IsHandtrap`, `IsTargetImmune`, `IsDestructionImmune`, `IsEngineGenerator`, `GetCardThreatScore(card, hint)`, `IsSpecialSummonBlocked(enemy, bot)`, `OpponentHasActiveNegator(enemy)` | การ์ดใหม่ที่เป็น floodgate/negator/handtrap/chokepoint → เพิ่มใน HashSet ใน `CardIntelligence.cs` (คอมเมนต์ชื่อการ์ดทุก ID → รัน `python tools/audit_card_ids.py` ตรวจ MISMATCH); immunity/battle → รัน `tools/scan_card_intelligence.py` (สร้าง `.Generated.cs`) |
| `HeuristicGuard` (static, ns `WindBot`) | `RegisterAceCards(params int[])`, `SanitizeSelection`, `ValidateSelection`, `ViolationCount`, `GetSessionSummary()` | register Ace ใน ctor; Sanitize ทำงานอัตโนมัติทุก OnSelectCard (ด่านสุดท้าย) |
| `DecisionTracer` (static, ns `WindBot`) | `Trace(fn,msg)`, `TraceSelect(fn,label,card)`, `TraceSkip(fn,reason)`, `TraceActivate(fn,reason)` | ใส่ใน func สำคัญ เพื่อ debug Text Duel |
| `AntiFloodgateHelper` (static) | `IsSpecialSummonBlocked(bot,enemy)`, `AreSpellsNegatedOrDisabled(...)`, `EnemyHasKnownNegate`, `EnemyHasOncePerTurnSpellNegator`, `GetPreemptiveImpermTarget`, `IsSafeToAttack(attacker, enemy, extraThreatIds)`, `IsSafeToDefend`, `IsEnemyCardAThreat`, `FilterAceCards`, `SelectPreferred(cards,min,max,preferredIds)` | ใช้ตามต้องการ |
| `DeckProbability` (static) | `Hypergeometric`, `AtLeastOne(N,K,n)`, `AtLeast`, `MultivariateHypergeometric`, `OpeningConsistency(deck, starters, hand=5)`, `ExcavationOdds`, `BanishLossRisk`, `EstimateHandTrapLikelihood(...)` | ใช้ตอนออกแบบเด็ค / ตัดสิน Pot of Desires / excavate |

## 4. ModernExecutor — Protected Virtual ที่ override ได้ (เลือกใช้)

| กลุ่ม | เมธอด |
|---|---|
| Field awareness | `IsSpecialSummonBlocked()`, `OpponentHasActiveNegator(ourCard)`, `ShouldNegateTarget(lastChainCard)`, `EnemyHasKnownNegate()`, `IsTargetImmune(card)`, `IsDestructionImmune(card)`, `IsViableEffectTarget(card)`, `HasEnemyFloodgate()`, `HasEnemyDisruption()` |
| Board state | `NeedsBoardPresence()`, `IsInGrindGame()`, `HasNegateOnField()`, `CountDisruptions()`, `IsBoardStrongEnough()`, `BoardScore()`, `ShouldPassTurn()` |
| Flow | `DynamicLethalCheck()`, `CanDealLethal()`, `CanOTK()`, `ShouldSkipCombo()`, `ShouldAttackBeforeCombo(main)`, `ClassifyAction(card,type)` → `ActionPriority`, `ShouldStopExtending()`, `ShouldDeferToMP2()`, `ShouldBattleBeforeSetting(main)`, `ShouldAvoidGenericExtraDeckSummon(n)`, `ShouldSkipLinkSummon(atk)` |
| Fallback idle | `EvaluateSmartRepositioning`, `EvaluateDesperationDefense`, `EvaluateSmartBackrowSetting` |
| Handtrap defaults | `DefaultAshBlossomAndJoyousSpring`, `DefaultEffectVeiler`, `DefaultInfiniteImpermanence`, `DefaultGhostBelleAndHauntedMansion`, `DefaultCalledByTheGrave`, `DefaultBossNegate`, `DefaultNibiru`, `DefaultPreemptiveImpermanence()`, `SmartHandTrapChain(...)`, `IsDuplicateOwnChainActivation()` |
| Position helpers | `IsBagooska(card)` (protected static, 90590303/90590304) — core ตั้ง DEF ใน `OnSelectPosition` + กัน repos ใน `ShouldAllowRepos`/SmartRepos |
| Targeting | `GetBestRemovalTarget(onlyFaceup, canBeTarget)`, `GetBestMonsterRemovalTarget(...)`, `GetBestSpellRemovalTarget(canBeTarget)`, `GetCardThreatScore(c, hint)`, `GetCardDiscardSacrificeCost(c)`, `GetMaterialSacrificePriority(c)` |
| Bait | `GetBaitIfNeeded(intendedCard)` |
| Yes/No guard | `RegisterOptionalFieldRemovalCards(params int[])` → `OnSelectYesNo` ปฏิเสธ optional removal เมื่อสนามศัตรูว่าง |
| Ace | `IsAceCard(card)` (public virtual ใน `Executor`) — **ค่าเริ่มต้น: ATK ≥ 2500 หรือ Fusion/Synchro/Xyz/Link ทุกตัว** → ต้อง override |
| Global guards (public override) | `ShouldAllowActivate/Summon/SpSummon/SpellSet/MonsterSet/Repos(card)` — เช็คก่อน func ของเด็คทุกครั้ง (log `[PHASE-GUARD] BLOCKED`) |

Fields: `_isGoingSecond` (set ครั้งเดียว), `LastChainCard`, `DeckPlugin`, `ShouldRushAttack`, `InMainPhase2`, `Scorer` (`BoardScorer`), `Analysis` (`AiAnalysisSuite`), `Brain` (`AIBrain`: `EnemyChainedThisChain`, `OpponentSummonCount`)

## 5. Callback ที่ GameAI เรียก (override ได้ใน deck)

`OnNewTurn`, `OnNewPhase`, `OnDraw`, `OnChaining`, `OnChainSolved`, `OnChainEnd`, `OnSummoning`, `OnSpSummoning`, `OnMove`,
`OnSelectHand` (true = ไปก่อน), `OnSelectCard`, `OnSelectYesNo`, `OnSelectEffectYn`, `OnSelectOption`, `OnSelectPosition`, `OnSelectPlace`,
`OnSelect{Fusion,Synchro,Xyz,Link}Material`, `OnSelectTribute`, `OnSelectSum`, `OnSelectRitualTribute`, `OnSelectPendulumSummon`, `OnSelectCounter`,
`OnSelectChain`, `OnSelectAttacker`, `OnSelectAttackTarget`, `OnSelectBattleCmd`, `OnPreBattleBetween`, `OnSelectIdleCmd`, `OnFallbackIdleCmd`

### 5.1 Optional Trigger (`OnSelectEffectYn`) — กติกาจริง (GameAI L552)
1. deck override `OnSelectEffectYn` คืนค่า → ใช้
2. ถ้ามี `AddExecutor(Activate, cardId)` ของการ์ดนั้น → **เรียก func เดียวกับตอนเปิดใช้ปกติ**; ทุก func คืน false → ปฏิเสธ trigger
3. ไม่มี executor + การ์ดศัตรู → false · ไม่มี executor + การ์ดเรา → true
→ 🔒 activate func ต้องรองรับกรณี trigger ด้วย (เช็ค `ActivateDescription == Util.GetStringId(id, n)` / `Card.Location` / phase)

## 6. Pre-select APIs (`AI.*`) — ตั้งก่อน `return true`

`SelectCard(...)`, `SelectNextCard(...)`, `SelectThirdCard(...)`, `SelectMaterials(...)`, `SelectPosition(pos)`, `SelectPlace(zones)`,
`SelectOption(i)`, `SelectNumber(n)`, `SelectAttribute(s)`, `SelectRace(s)`, `SelectAnnounceID(id)`, `SelectYesNo(bool)`
→ ⚠️ `SelectCard`/`SelectNextCard` ถูกข้ามเมื่อ `OnSelectCard` คืนค่าไม่ null (ดู `hint_reference.md` §3)

## 7. Utility ที่ใช้บ่อย

- `Util` (AIUtil): `GetStringId`, `GetLastChainCard`, `ChainContainsCard`, `ChainContainPlayer`, `ChainCountPlayer`, `IsChainTarget`, `GetProblematicEnemyMonster/Card/Spell`, `GetBestEnemyMonster/Card/Spell`, `GetWorstBotMonster`, `GetBestBotMonster`, `IsOneEnemyBetter`, `IsAllEnemyBetter`, `GetTotalAttackingMonsterAttack`, `GetBotAvailZonesFromExtraDeck`, `IsTurn1OrMain2`, `OpponentHasNegation`, `SelectPreferredCards`
- `ClientField` (Bot/Enemy): `HasInHand/HasInMonstersZone/HasInSpellZone/HasInGraveyard/HasInBanished/HasInExtra` (+ combos), `GetMonsters`, `GetSpells`, `GetMonsterCount`, `GetSpellCount`, `GetFieldCount`, `GetRemainingCount(id, initial)`, `GetGraveyardMonsters`, `GetMonstersInExtraZone`, `GetLinkedZones`, `IsFieldEmpty`, `HasAttackingMonster`
- `ClientCard`: `IsCode`, `IsOriginalCode`, `HasType/Race/Attribute/Setcode/Position/LinkMarker/XyzMaterial`, `IsFaceup/Facedown/Attack/Defense/Disabled/Tuner/ExtraCard/OnField/CanRevive`, `GetNonAltartCode()` + extensions `IsShouldNotBeTarget`, `IsMonsterInvincible`, `IsMonsterDangerous`, `IsFloodgate`, `IsDestructionImmune`
- `Duel`: `Turn`, `Player`, `Phase`, `LastChainPlayer`, `CurrentChain`, `ChainTargets`, `LastSummonedCards`, `GetCurrentChainCard()`, `GetCurrentSolvingChainCard()`, `GetCurrentSolvingChainInfo()`, `IsCurrentSolvingChainNegated()`
