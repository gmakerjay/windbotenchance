# Hint ID Reference (ตรวจกับ `script/constant.lua` + `ModernExecutor.cs` เมื่อ 2026-10-03)

> ที่มา: `C:\Users\admin\Documents\EdoGame\script\constant.lua` (HINTMSG_*)
> Routing: `windbot-fork/ExecutorBase/Game/AI/ModernExecutor.cs` → `OnSelectCard` (L2456–2627, v0.086)
> ถ้าไฟล์จริงเปลี่ยน ให้เชื่อไฟล์จริงและอัปเดตตารางนี้

## 1. ตาราง HINTMSG ครบชุด (constant.lua)

| ID | Constant | ความหมาย | ModernExecutor จัดการ? |
|---|---|---|---|
| 500 | RELEASE | เลือกสังเวย | ✅ ฝั่งเรา → `MaterialEvaluator.PickDiscardTarget/SortMaterials` |
| 501 | DISCARD | ทิ้งการ์ด | ✅ ฝั่งเรา → `MaterialEvaluator.PickDiscardTarget/SortMaterials` |
| 502 | DESTROY | ทำลาย | ✅ ศัตรู ≥ min → ThreatScore สูงสุด; ศัตรูไม่พอ → `PickDestructionSubstitute` (pop ตัวเอง) |
| 503 | REMOVE | รีมูฟ (Banish) | ✅ ศัตรู → ThreatScore |
| 504 | TOGRAVE | ส่งลงสุสาน | ✅ ศัตรู ≥ min → ThreatScore · candidate อยู่ใน Deck ทั้งหมด (Foolish Burial) → `Strategy.PickSearchTarget(cards, Card)` (null → `AI.SelectCard`/Fallback) · อื่นๆ ฝั่งเรา → cost (`PickDiscardTarget`) |
| 505 | RTOHAND | คืนขึ้นมือ | ✅ ศัตรู ≥ min → bounce (ThreatScore) · ไม่มีศัตรู → `Strategy.PickSearchTarget` |
| 506 | ATOHAND | เพิ่มขึ้นมือ (Search) | ✅ → `Strategy.PickSearchTarget(cards, Card)` |
| 507 | TODECK | คืนเด็ค | ✅ ศัตรู ≥ min → spin (ThreatScore) · ศัตรูไม่พอ → cost ฝั่งเรา (`PickDiscardTarget`/`SortMaterials` = ตัวค่าต่ำสุด) |
| 508 | SUMMON | Normal Summon | ❌ (Fallback Case3 = positive pick) |
| 509 | SPSUMMON | Special Summon | ✅ → `Strategy.PickSpecialSummonTarget` (Pendulum: min=1,max>1 → `OnSelectPendulumSummon`) |
| 510 | SET | เซ็ต | ❌ |
| 511 | FMATERIAL | Fusion Material | ✅ `OnSelectFusionMaterial` → `MaterialEvaluator.SortMaterials` |
| 512 | SMATERIAL | Synchro Material | ⚠️ **ModernExecutor ไม่ override** `OnSelectSynchroMaterial` → ต้องเขียนเองในเด็ค Synchro |
| 513 | XMATERIAL | Xyz Material | ✅ `OnSelectXyzMaterial` → `SortMaterials` |
| 514 | FACEUP | เลือกการ์ดหงาย | ⚠️ ถูกรวมใน removal branch (เลือกศัตรู) — ระวังเอฟเฟกต์บัฟฝั่งเรา |
| 515 | FACEDOWN | เลือกการ์ดคว่ำ | ❌ |
| 516 / 517 | ATTACK / DEFENSE | เลือกมอนท่าโจมตี/ป้องกัน | ❌ |
| 518 | EQUIP | เลือกการ์ดที่จะสวม | ✅ branch แยก: ฝั่งเรา → Ace/ATK สูงสุด (สวมบัฟ) · สวมใส่ศัตรู (Snatch Steal ฯลฯ) → เขียนเอง |
| 519 | REMOVEXYZ | ถอด Xyz Material | ❌ ModernExecutor · Fallback Case2 = cost (ตัวค่าต่ำ) |
| 520 | CONTROL | เปลี่ยนการควบคุม | ✅ ศัตรู → ThreatScore |
| 521 | DESREPLACE | เลือกตัวแทนถูกทำลาย | ❌ (ใช้ `PickDestructionSubstitute` เองได้) |
| 522–525 | FACEUP/FACEDOWN ATTACK/DEFENSE | เลือกรูปแบบ | ❌ |
| 526 | CONFIRM | ยืนยัน/เปิดดู | ❌ |
| 527 | TOFIELD | วางลงสนาม | ❌ |
| 528 | POSCHANGE | เปลี่ยนรูปแบบ | ✅ ศัตรู → ATK สูงสุด |
| 529 / 530 | SELF / OPPO | เลือกฝั่งตัวเอง/ศัตรู | ❌ |
| 531 | TRIBUTE | สังเวย (ข้อความแบบใหม่) | ❌ |
| 532 | DEATTACHFROM | เลือกตัวที่จะถอด material | ❌ |
| 533 | LMATERIAL | Link Material | ✅ `OnSelectLinkMaterial` → `SortMaterials` |
| 549 | ATTACKTARGET | เลือกเป้าโจมตี | ❌ |
| 550 | EFFECT | เลือกเอฟเฟกต์ | ❌ |
| 551 | TARGET | เลือกเป้าหมาย | ✅ ศัตรู ≥ min → ThreatScore (เป้าฝั่งเรา เช่นบัฟ → ต้องเขียนเอง) |
| 552 | COIN | ทอยเหรียญ | ❌ (ลบออกจาก removal branch แล้ว v0.086) |
| 553 | DICE | ทอยเต๋า | ❌ |
| 554 | CARDTYPE | ประเภทการ์ด | ❌ |
| 555 | OPTION | ตัวเลือก | ❌ (ตั้งล่วงหน้า `AI.SelectOption(i)` หรือ override `OnSelectOption`) |
| 556 | RESOLVEEFFECT | เลือกเอฟเฟกต์ที่จะ resolve | ❌ |
| 560 | SELECT | เลือก (ทั่วไป) | ❌ |
| 561 | POSITION | เลือกรูปแบบ | ❌ (`AI.SelectPosition` / override `OnSelectPosition`) |
| 562 / 563 | ATTRIBUTE / RACE | ประกาศ Attribute/Race | ❌ (`AI.SelectAttribute(s)` / `AI.SelectRace(s)` ก่อนเปิดใช้) |
| 564 | CODE | ประกาศชื่อการ์ด | ❌ (`AI.SelectAnnounceID(id)` ก่อนเปิดใช้) |
| 565 | NUMBER | ประกาศตัวเลข | ❌ (`AI.SelectNumber(n)` ก่อนเปิดใช้) |
| 566 | EFFACTIVATE | เลือกเอฟเฟกต์ที่จะเปิด | ❌ |
| 567 | LVRANK | ประกาศ Level/Rank | ❌ |
| 568 | RESOLVECARD | เลือกการ์ดที่จะ resolve | ❌ |
| 569 / 570 / 571 | ZONE / DISABLEZONE / TOZONE | เลือกโซน | ❌ (`AI.SelectPlace(zones)` / override `OnSelectPlace`) |
| 572 | COUNTER | วาง/นำ Counter ออก | ❌ ใน OnSelectCard (ใช้ `OnSelectCounter` → `ResourceEvaluator.SelectCounters`) |
| 575 | NEGATE | ขัดขวาง | ✅ ศัตรู → ThreatScore |
| 576 | ATKDEF | เลือก ATK/DEF | ❌ |
| 577 | APPLYTO | เลือกการ์ดที่จะใช้ผล | ❌ |
| 578 | ATTACH | ใส่เป็น material | ❌ |
| 579 | RTOGRAVE | คืนลงสุสาน (จากรีมูฟ) | ❌ |

⚠️ **ไม่มี** 573 / 574 ใน constant.lua · ไม่มี `HINTMSG_TOHAND` (ค้นขึ้นมือใช้ 506) · ไม่มี `HINTMSG_DISABLE` (core ใช้ 575 แทน)

## 2. Custom Hint (สำคัญมาก)

สคริปต์ Lua จำนวนมากใช้ `Duel.Hint(HINT_SELECTMSG, tp, aux.Stringid(id, n))` → hint ที่ส่งมาคือ

```csharp
long hint = Util.GetStringId(cardId, n);   // = (cardId << 20) | n   (AIUtil.cs L253)
```

- ค่าจะใหญ่กว่า 579 มาก → ModernExecutor ไม่รู้จัก → `return base.OnSelectCard(...)` (= `null`)
- ทำให้ `AI.SelectCard(...)` ที่ตั้งไว้ **ทำงานได้** กับ hint แบบนี้ (ดู §3)
- ต้องเปิด `script/cXXXX.lua` ดูว่าเรียก `aux.Stringid(id, n)` ด้วย n เท่าไร แล้วเช็คใน deck `OnSelectCard`:
  `if (hint == Util.GetStringId(CardId.X, 2)) { ... }`

## 3. ลำดับการเลือกการ์ดจริง (GameAI.OnSelectCard L309–440)

```text
1. Executor.OnSelectCard(cards,min,max,hint,cancelable)   ← deck override → ModernExecutor.OnSelectCard
   ├─ step 0 (v0.094): AI.SelectCard/SelectNextCard ที่ match pool → ใช้ทันที (ขาด → เติมด้วย heuristic ของ hint นั้น)
   │    · ไม่ใช้กับ material hint 511/512/513/533 (ไปข้อ 3)
   │    · hint แบบ cost (500/501/504 ฝั่งเราไม่ใช่ Deck/507 ฝั่งเรา) ใช้เฉพาะ preselect แบบ ClientCard/IList<ClientCard>
   │    · ไม่ match → ไม่ pop (เก็บไว้ให้ prompt ถัดไป)
   └─ ถ้าคืนค่า != null → ใช้เลย
2. (hint 509, min==1, max>min) → OnSelectPendulumSummon
3. hint 511/512/513/533 → m_materialSelector หรือ OnSelect{Fusion,Synchro,Xyz,Link}Material
4. AI.SelectCard / SelectNextCard  (CardSelector stack — ล้างเมื่อ OnChainEnd / OnNewPhase / ทุก idle prompt (v0.094) / func คืน false → rollback)
5. Executor.FallbackSelectCard (heuristic)
   ├─ Case1 เลือกศัตรู : 502/503/504/505/507/520/528/549/551/575
   ├─ Case2 cost ฝั่งเรา: 500/501, 504 (ไม่ใช่ Deck), 507 (เรา), 511/512/513/519/531/533
   └─ Case3 positive  : 506/508/509, 505 (เรา), 504 (Deck→GY)
6. เลือกใบแรกๆ ตาม min
→ ทุกเส้นทางผ่าน HeuristicGuard.SanitizeSelection + ValidateSelection ก่อนส่งจริง
```

> ✅ **v0.094**: `AI.SelectCard(...)` ใน activate func **ใช้ได้แล้ว** กับ hint ที่ ModernExecutor route (target/search/removal/SS) เมื่อการ์ดที่เลือกอยู่ใน pool จริง
> — ตั้ง preselect แล้ว func `return false` → core rollback ให้เอง ไม่ค้างไป hijack prompt อื่น
> ⚠️ ยังต้องระวัง: (1) deck override `OnSelectCard` ที่คืนค่าก่อน `base` จะข้าม preselect (2) cost prompt ใช้เฉพาะ preselect แบบระบุ `ClientCard`
> (3) HeuristicGuard ยังอาจแก้ selection ที่เล็งการ์ดตัวเองโดยตั้งใจ → ใช้ Pending-Select (`executor_template.md`) ถ้าต้องการบังคับเด็ดขาด

## 4. รู้ว่า "การเลือกนี้มาจากการ์ดไหน"

`Card` / `ActivateDescription` = การ์ดล่าสุดที่ executor **ประเมิน** (อาจเปลี่ยนไปแล้วถ้ามีเชนตอบโต้) — อย่าพึ่งพาอย่างเดียว ใช้:

| สถานการณ์ | API |
|---|---|
| กำลังจ่าย Cost / เลือกเป้าตอนเปิดใช้ | `Duel.GetCurrentChainCard()` |
| กำลัง Resolve | `Duel.GetCurrentSolvingChainCard()` / `Duel.GetCurrentSolvingChainInfo()` (`ActivateDescription`, `Targets`) |
| เชนที่ resolve อยู่ถูก negate ไหม | `Duel.IsCurrentSolvingChainNegated()` |
| เชนล่าสุด / ผู้เล่น | `Util.GetLastChainCard()`, `Duel.LastChainPlayer`, `Duel.CurrentChain`, `Duel.ChainTargets` |
