# Central Core Pro Audit & Refactor (v0.094 · 2026-10-04)

> ขอบเขต: `windbot-fork/ExecutorBase` (Central Core) — การอ่านบอร์ด, Decision Engine, Chain/Handtrap timing,
> Selection pipeline, ComboRouter, CardIntelligence/CardTextSemantics, Position
> เป้าหมาย: บอทเล่นได้ระดับ "Pro / ท้าทายผู้เล่น" แบบ **generic** (ไม่ผูกรายเด็ค/รายการ์ด) โดยไม่ทำให้เด็คเดิม 140+ ตัวพัง
> Checkpoint ก่อน refactor: `6c16d31` · Build: `BUILD_AND_DEPLOY.ps1` → **0 Error(s)** (9 warnings เดิม) · Deploy → `C:\Users\admin\Documents\EdoGame\`
> ⚠️ ยังไม่ได้รัน Headless Text Duel (ตามนโยบาย — รอคำสั่ง "Text Duel")

---

## 1. สรุปสั้น (Executive Summary)

| ด้าน | ก่อน | หลัง |
|---|---|---|
| การอ่านบอร์ด | `Analysis` refresh เฉพาะต้นเทิร์น/เฟส → ข้อมูลค้างทั้ง Main Phase | Refresh ทุก idle prompt |
| Handtrap timing | ให้คะแนน activation ตาม location (~10) + early penalty −10 → แทบไม่ Ash starter | อ่านข้อความการ์ด (search/SS/draw = engine) → ตัด engine ชิ้นแรกได้ |
| เจตนาเด็ค (`AI.SelectCard`) | ถูกข้ามใน hint ที่ core route, preselect ค้างไป hijack prompt อื่น | ใช้เมื่อ match pool, rollback เมื่อ func false, ล้างทุก idle |
| ComboRouter | ยิง executor ตัวแรก (อาจ generic), ไม่มี desc จริง, fallback ยิงซ้ำ, Maxx C = Abort | ผ่าน func ของเด็คพร้อม desc จริง, จำ step ที่ทำแล้ว, Abort เฉพาะ negate จริง |
| Removal targeting | battle-immune ถูกนับเป็น effect-immune (−20000) | แยก battle/effect; 502 กรอง immune เมื่อมีตัวเลือก |
| Position | มอน ATK สูงตก DEF ใน MP1 | ตั้ง ATK เมื่อศัตรูว่างหรือมีเป้าที่ตีชนะ |

---

## 2. Findings → Fixes (รายไฟล์)

### 2.1 Board reading
- **`ModernExecutor.OnSelectIdleCmd`** — เพิ่ม `Analysis?.Refresh()` บนสุด: ทุก heuristic (lethal, combo, stop-extending) เห็นบอร์ดล่าสุดหลังทุก action
- **`DecisionContext.ShouldActivate`** — เดิม `AIContext` state จับตอน `OnNewTurn` ด้วย 0/0 → ตอนนี้เรียก `UpdateState(Brain.OwnSummons, Brain.OpponentSummonCount)` ก่อนทุกการตัดสิน

### 2.2 Decision Engine / Chain timing
- **`ThreatAnalyzer.GetActivationThreatScore(card)`** (ใหม่): base 40 · +30 ถ้าข้อความเป็น engine (search / Special Summon / draw) · +10 Extra Deck · +5 Spell ครั้งเดียว · ×0.5 ถ้า disabled · `max` กับ board threat
  → เดิม CounterfactualSimulator ถือ Ash/Veiler ไว้เกือบทุก starter เมื่อมือศัตรู ≥ 3
- **`ChainTimingAdvisor`**:
  - unknown target ที่ `CardTextSemantics.IsEngineEffect` → +15
  - early-combo penalty −10 (score < 70) → −5 (score < 60) — starter ไม่รู้จักไม่ตกต่ำกว่า threshold 45 อีก
  - ลบ Eater of Millions (63845230 ไม่ใช่ handtrap) · Meluseek แก้เป็น 25533642 (53143898 = Marionetter, ตรวจ cdb)
  - `CountInteractiveCards` ใช้ `CardIntelligence.IsHandtrap` + alt codes
- **`StateRepresentation` / `DynamicValueEvaluator`**: negator รวม `CardIntelligence.IsKnownNegator`; handtrap list ไม่มี Eater, เพิ่ม Ash alt 14558128

### 2.3 Selection pipeline (เจตนาเด็ค)
- **`CardSelector`**: `Match(cards)` (ไม่ pad) + `IsExplicitCardSelection`
- **`GameAI`**:
  - `TryConsumePreselectedCards(cards, max, explicitOnly)` — peek top, pop เฉพาะเมื่อ match ≥ 1
  - `PreselectCheckpoint()` / `RollbackPreselect()` — `ShouldExecute` rollback เมื่อ func คืน false; `OnSelectChain` rollback เมื่อ skip duplicate handtrap
  - `InternalOnSelectIdleCmd` ล้าง selector ทุก idle prompt (กัน stale preselect)
  - duplicate-handtrap guard นับเฉพาะ `Controller == 0` (Imperm war ตอบได้)
- **`ModernExecutor.OnSelectCard`**:
  - step 0 ใช้ preselect ที่ match (ยกเว้น material hint); cost hint (500/501/504 เรา-ไม่ใช่ Deck/507 เรา) ใช้เฉพาะ explicit `ClientCard`
  - partial match → เติมด้วย heuristic ของ hint เดียวกัน (recursive guard `_inPreselectPadding`)
  - `CompleteSelection(primary, orderedPool, min)` ใช้กับ popSub / foolish / 504 / 506 / 509 plugin picks
  - 502 กรอง destruction-immune เมื่อเหลือตัวเลือก ≥ min
  - ลบ early-return `HasPreselectedCard` ใน 504 Deck→GY (step 0 จัดการแล้ว)

### 2.4 ComboRouter continuity
- per-turn `_executedThisTurn` (`RecordExecutedAction` / `GetExecutedCount`, SummonOrSet ≡ Summon) — `CompleteCurrentStep` บันทึก
- `SetActiveLine` + `ApplyExecutedHistory` → สลับไป fallback line จะ auto-complete step ที่ทำแล้ว
- spell/backrow บนสนามนับใน `GetViableLines` RequiredCards
- `TrySwitchToFallback(bot, hasEnemyDisruption)` overload (ตัวเดิมใช้ค่าที่จำจาก `ActivateBestLine`)
- `NotifyStepNegated` ไม่สลับไป fallback ที่ fail แล้ว
- **`ModernExecutor.ComboStepApprovedByExecutors`** (protected virtual): executor ID ตรงก่อน → generic -1; ตั้ง `Card/Type/ActivateDescription` จริง; true ตัวใดตัวหนึ่ง = ผ่าน; false → rollback preselect; ไม่มี executor = อนุญาต · Summon step รับ SummonOrSet
- **`IsOpponentCardNegator`**: เฉพาะ monster negate / KnownNegator / Counter trap / Ash×2, Ghost Belle, Veiler, Imperm, Called by, Crossout Designator, Solemn, Red Reboot — Maxx "C"/Nibiru ฯลฯ ไม่ทำให้ Abort

### 2.5 Card intelligence
- **`CardTextSemantics`**: `RegexDestructionImmune` effect-only (TH ต้องมี "เอฟเฟกต์"; EN `cannot be destroyed by ... effect` / `unaffected by`); `RegexBattleImmune` + `IsBattleImmune`; `IsEngineEffect` (+ static helper); DecoyShield รวม battle immune
- **`CardIntelligence`**: โบนัส negator/floodgate/chokepoint ×0.5 เมื่อ face-up + disabled; `IsBattleImmune(card)`
- **`ModernExecutor.IsDestructionImmune`** เช็ค `CardIntelligence.IsDestructionImmune` ด้วย; `DefaultEffectVeiler` ไม่นับ Eater; `CountDisruptions` ลบ Eater เพิ่ม Ash alt

### 2.6 Position
- **`OnSelectPosition`**: Main1 ของเรา, Turn > 1, ATK ≥ 1000 และ ATK ≥ DEF → ATK เมื่อสนามศัตรูว่าง หรือมีเป้า face-up ที่ตีชนะ (ก่อน fallthrough ไป DEF); กฎ Bagooska DEF ยังอยู่ก่อน

---

## 3. Backward compatibility
- ไม่ลบ/เปลี่ยน signature public/protected เดิม — เพิ่ม overload/virtual ใหม่เท่านั้น
- เด็คที่ override `OnSelectCard` แล้วคืนค่าก่อน `base` → พฤติกรรมเดิม 100%
- เด็คที่ใช้ Pending-Select → ยังทำงาน (deck override มาก่อน step 0)
- `python tools/audit_card_ids.py windbot-fork/ExecutorBase` → ไม่มี hit ใหม่ (6 hit เดิมเป็น non-ID)

## 4. ข้อจำกัดที่ยังเหลือ (⚠️ ดู `known_core_issues.md` #37–45)
1. `_negateNever` ยังมี Terraforming / RotA
2. `DefaultExecutor` handtrap check (`Util.ChainContainsCard`) ไม่แยก controller
3. hint 551 เลือกศัตรูเสมอ (ใช้ `AI.SelectCard` เพื่อบัฟฝั่งเรา)
4. HeuristicGuard อาจทับ self-target ที่ตั้งใจ
5. `IsTargetImmune` filter ใช้กับ prompt ที่ไม่ได้ target ด้วย
6. Battle targeting ยัง greedy
7. checkpoint rollback สมมติ order `SelectCard` → `SelectNextCard`
8. `OnSelectPendulumSummon` แทบไม่ถูกเรียกภายใต้ ModernExecutor
9. `PreNewTurn` / `PreNewPhase` ถูกเรียกซ้ำ (idempotent)

## 5. แผนทดสอบที่แนะนำ
- ผู้ใช้ทดสอบเองใน EDOPro ก่อน (เน้น: บอท Ash/Veiler starter แรก, คอมโบเดินต่อหลังโดน Maxx "C", search ตาม `AI.SelectCard`, ไม่ทำลายเป้า immune)
- เมื่อพร้อม สั่ง **"Text Duel"** → รันกับ `ABC`, `Altergeist`, `BlueEyes`, `DarkMagician` เพื่อวัด Win Rate / Violations (ต้อง 0)

## 6. Next candidates (ยังไม่ทำ)
- Battle planner: จัดลำดับการตีทั้ง Battle Phase (maximize damage / clear blockers)
- hint 551 context-aware (buff vs debuff จากข้อความการ์ด)
- `_negateNever` ย้ายไปใช้ ChainTimingAdvisor เป็นแหล่งเดียว
- DefaultExecutor handtrap helpers → controller-aware
