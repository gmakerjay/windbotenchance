# Known Core Issues & Pitfalls (Audit 2026-10-03, v0.086)

> สถานะข้อบกพร่องใน Central Core (`windbot-fork/ExecutorBase`) ที่พบตอน audit skill
> ✅ = แก้ใน core แล้ว (v0.086) — เด็คไม่ต้องหลบอีก · ⚠️ = ยังเป็นพฤติกรรม/ข้อจำกัดของ core ที่ต้องรู้
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

## ⚠️ ข้อจำกัด / พฤติกรรมของ core ที่ยังต้องรู้

| # | จุด | พฤติกรรม | สิ่งที่เด็คต้องทำ |
|---|---|---|---|
| 16 | `HeuristicGuard._aceCardIds` (static) | ไม่ถูกล้างระหว่างดวล/บอท | headless 2 บอทใน process เดียวแชร์รายการ Ace — ยอมรับได้ |
