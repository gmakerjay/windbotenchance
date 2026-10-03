# Skill Audit — yugioh-executor Master Skill (v0.085 skill · v0.086 core fixes, 2026-10-03)

## ขอบเขต
ตรวจ `.agents/skills/yugioh-executor/SKILL.md` เทียบกับซอร์สจริงใน `windbot-fork/ExecutorBase` (ModernExecutor, GameAI, Plugin, ComboRouter, BaitPlanner, ChainAdvisor, ResourcePlan, HeuristicGuard, FallbackSelectCard) และ `script/constant.lua` แล้วเขียนสกิลใหม่เป็น Master Skill

## โครงสร้างสกิลใหม่
| ไฟล์ | เนื้อหา |
|---|---|
| `SKILL.md` | Master prompt: project map, session protocol/DoD, core rules, workflow, Module Registration Matrix, 12 Engineering Laws, hint quick table, safety checklist, build/pull/push |
| `references/hint_reference.md` | HINTMSG 500–579 + การ route ของ ModernExecutor + custom Stringid + selection pipeline |
| `references/core_api.md` | API map ทุกโมดูล (อัตโนมัติ vs เด็คต้องทำเอง) |
| `references/known_core_issues.md` | สถานะบั๊ก core (✅ แก้แล้ว v0.086) + ข้อจำกัดที่ยังต้องรู้ |
| `references/executor_template.md` | Template Executor + Plugin (compile ตรวจแล้ว 0 Error) |
| `references/case_studies.md` | บทเรียนเฉพาะเด็คที่ย้ายออกจากสกิลเดิม |

## Mismatch ที่พบในสกิลเดิม → แก้แล้ว
1. **`AI.SelectCard` ไม่มีผล** กับ hint ที่ ModernExecutor จัดการเอง (500–507, 509, 514, 518, 520, 528, 551, 552, 575) — ต้องจัดการใน `OnSelectCard`/Pending-Select; ใช้ได้กับ custom Stringid
2. **ComboRouter step ข้าม** ฟังก์ชันเด็ค, `ShouldAllow*` guard และ SetCard — รองรับเฉพาะ Activate/Summon/SpSummon/SpellSet และเลือก description แรกที่ใช้ได้
3. **BaitPlanner เฉื่อย** ถ้าไม่เรียก `GetBaitIfNeeded(Card)` (93 ไฟล์ลงทะเบียน แต่เรียกใช้จริงแค่ 4)
4. **`IsAceCard` default** = ATK≥2500 หรือ Fusion/Synchro/Xyz/Link — ต้อง override และซิงค์กับ `ResourcePlan.RegisterAceCards` + `HeuristicGuard.RegisterAceCards`
5. **Optional trigger** (`OnSelectEffectYn`) ใช้ฟังก์ชัน Activate เดียวกัน; ถ้าทุกตัว return false = ปฏิเสธ
6. **Synchro (512)** ไม่ผ่าน Plugin material evaluator
7. **Card/ActivateDescription อาจ stale** ระหว่าง resolve — ใช้ `Duel.GetCurrentSolvingChainCard()`
8. ScaleResolver / ActionScorer / IsEmergencyThreat ไม่ถูกเรียกโดย core
9. `DeckPluginBase.ResetTurnState` reset Strategy/ResourceEvaluator ให้แล้ว

## Core bugs → ✅ แก้แล้วใน v0.086
| บั๊ก | แก้เป็น |
|---|---|
| Hint 507 (TODECK) ถูกมองเป็น equip alias → TODECK ฝั่งเราเลือก Ace | 507 ศัตรู = removal · ฝั่งเรา → cost branch (`PickDiscardTarget`/`SortMaterials`) |
| Hint 518 อยู่ใน POSCHANGE; 552 (COIN) อยู่ใน removal | POSCHANGE = 528, EQUIP = 518 แยก branch; ลบ 552 |
| Hint 504 Deck→GY ใช้ `PickDiscardTarget` | branch 1.8: candidate ใน Deck ทั้งหมด → `Strategy.PickSearchTarget` (null → `AI.SelectCard`/Fallback) |
| Bagooska ID ผิด (85359414/26273196/26593852) | `IsBagooska()` = 90590303/90590304 ใน ModernExecutor/DefaultExecutor/AntiFloodgateHelper |
| MP2 guard ใน `ShouldAllowActivate` unreachable | ย้ายก่อน `Phase != Main1` return |
| `configs/*.json` ไม่ถูก deploy + ID ปลอม | `BUILD_AND_DEPLOY.ps1` copy → `WindBot\configs`; สร้าง list ใหม่จาก cdb |
| `CardTextSemantics.HINTMSG_TARGET = 506` | 551 |
| `FallbackSelectCard` จัด hint ผิด (519/552/572/508) | Case1/2/3 เขียนใหม่ตาม constant.lua |
| `SmartHandTrapChain(params)` ไม่มี duplicate guard | `IsDuplicateOwnChainActivation()` ทั้ง 2 overload |
| `ComboRouter.GetViableLines` เลือกเส้นที่ abort ซ้ำ | กรอง `_failedLinesThisTurn` |
| DashBot `ModernArchetypes` ขาด Watenpai, Darklord 2 | เพิ่มแล้ว |
| Darklord2 reset turn state ซ้ำ | ลบ reset ซ้ำ |

ข้อจำกัดที่ยังเหลือ (ไม่ใช่บั๊ก แต่ต้องรู้): ComboRouter bypass func/guard · `IsAceCard` default · Synchro 512 ไม่ผ่าน Plugin ·
`IsSpecialSummonBlocked` นับ floodgate ทุกตัว · MP2 guard ใหม่บล็อก search เมื่อบอร์ดแข็ง → ดู `known_core_issues.md`

## Card-ID Audit (ใหม่ — ผลกระทบใหญ่ที่สุด)
**วิธี**: ดึงตัวเลข 5–9 หลักทุกตัวในซอร์ส C# → เทียบ `cards.cdb` + `EdoGame\WindBot\*.cdb`
- `NOTFOUND` = ไม่มี ID นี้ใน cdb (ปลอม/พิมพ์ผิด)
- `MISMATCH` = บรรทัดมี ID เดียว + คอมเมนต์ชื่อการ์ด แต่ชื่อใน cdb ไม่ตรง → เสนอ ID ที่ถูก

**ผล core (`windbot-fork/ExecutorBase`)**: แก้ ~140 entries ใน 14 ไฟล์ (CardIntelligence, ModernExecutor, DefaultExecutor, Executor,
AntiFloodgateHelper, ChainTimingAdvisor, ThreatAnalyzer, BoardScorer, BaitPlanner, DynamicValueEvaluator, HeuristicGuard,
CardExtension, AIBrain, ActionHistory) — ตัวอย่าง: Megamorph ใส่ ID ของ "Rivalry of Warlords", Necrovalley = "Summon Limit",
Accesscode Talker = "Herald of Ultimateness", Buster Blader = "Vanity's Fiend", Dracotail ทั้งชุดเป็น ID ปลอม
→ **floodgate / negator / handtrap / chokepoint detection ทำงานถูกต้องครั้งแรก** (เดิมจับผิดการ์ดหรือไม่จับเลย)
เหลือ 13 hit ที่ไม่ใช่ ID (คะแนน 100000/999999/11000, token, `NamedCard`, `ShouldNotBeTarget`)

**เครื่องมือ**: `python tools/audit_card_ids.py <ไฟล์|โฟลเดอร์>` — ต้องรันทุกครั้งที่เพิ่ม ID (อยู่ใน SKILL.md §3.1/§4.1/§10)

**⚠️ ระดับเด็ค (ยังไม่แก้ — รออนุมัติ)**: `windbot-fork/Game/AI` พบ 236 hit (163 MISMATCH, 73 NOTFOUND; มี false positive บางส่วน)
| เด็ค | hit |
|---|---|
| TrirealmRift | 55 |
| Tenpai | 30 |
| EneaCraft | 17 |
| RaiOh | 16 |
| Invoke | 10 |
| Blackwings | 8 |
| อื่นๆ | ที่เหลือ (กระจายหลายไฟล์) |

รายละเอียด: `.agents/skills/yugioh-executor/references/known_core_issues.md`
