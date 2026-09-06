# Changelog — AI Executor Intelligence Upgrade (GOAT + 2026)
**Date:** 2026-06-20  
**Version:** Smart Flow v3.1 — Anti-Stupid Executor Patch  
**Scope:** 9 deck executors (4 Modern 2026 + 5 GOAT format)

---

## สรุปการเปลี่ยนแปลง

ปรับปรุง AI ทั้ง 9 deck executors ให้ **ไม่ทำพฤติกรรมโง่** — เช็คเป้าหมายก่อน activate,
ป้องกัน OPT violation, ไม่ทำร้ายตัวเอง, เลือก target ตาม threat level

### ปัญหาหลักที่แก้ไข

| # | หมวด | ปัญหาเดิม | วิธีแก้ |
|---|------|-----------|--------|
| 1 | **OPT Violation** | การ์ดที่ใช้ได้ 1/turn ถูก activate ซ้ำ | เพิ่ม OPT flags (42 ตัว) + OnNewTurn reset |
| 2 | **Targetless Activation** | Activate spell/trap โดยไม่มี target | เช็ค target availability ก่อน return true |
| 3 | **Self-Destruction** | HeavyStorm ลบ GravityBind/SkillDrain ตัวเอง | เช็ค stall/floodgate cards ก่อน activate |
| 4 | **Wasted Resources** | Lily pump 2000LP เปล่า, SpiritRyu discard ไม่ lethal | Lethal/beat-over check ก่อน pay cost |
| 5 | **Boss Sacrifice** | Metamorphosis tribute Armed Dragon LV7 | `!IsAceCard(c)` filter |
| 6 | **State Unawareness** | Fusilier summon 1400 ATK ไม่มี SkillDrain | SkillDrain-aware summon + repos logic |
| 7 | **Missing SelectOption** | BLS double attack ไม่ SelectOption(1) | เพิ่ม SelectOption สำหรับทุก option-based effect |
| 8 | **Duplicate Continuous S/T** | Set SkillDrain/GravityBind ซ้ำ | HasInSpellZone check ก่อน activate |

---

## ไฟล์ที่แก้ไข — Modern 2026 Executors

### `_2026_PurrelyExecutor.cs` — เขียนใหม่ทั้งหมด (201 → 578 lines)
- เพิ่ม OPT flags 15 ตัว + OnNewTurn reset
- Effect handlers ทุกตัว: target validation, threat-aware selection
- Xyz overlay material selection, position override, ComboRouter

### `_2026_YummyExecutor.cs` — เขียนใหม่ทั้งหมด (267 → 843 lines)
- เพิ่ม OPT flags 10 ตัว
- Marshmao search priority, Yummy Surprise GY revive logic
- SantaClaws threat guard, Borreload Savage negate timing

### `_2026_RegenesisExecutor.cs` — แก้ไข 20+ จุด
- เพิ่ม OPT flags 9 ตัว
- แก้ LavaGolem threat check, Fleurdelis targeting bug
- DragonsMind/TheFallenAndTheVirtuous/Lulu condition checks
- Bystial search targets, ExtraDeck SpSummon blocked check

### `_2026_RunickExecutor.cs` — แก้ไข 15+ จุด
- เพิ่ม OPT flags 8 ตัว
- Runick spell target validation (FreezingCurses, Destruction, FlashingFire, Slumber)
- Continuous S/T duplicate checks
- MessengerOfPeace LP threshold 500 → 1000

---

## ไฟล์ที่แก้ไข — GOAT Format Executors

### `GOAT_ItWorkExecutor.cs` — 7 fixes
| แก้ไข | รายละเอียด |
|-------|-----------|
| HeavyStorm | ไม่ลบ GravityBind/LevelLimit/SwordsOfRevealingLight ตัวเอง |
| JudgmentOfAnubis | เพิ่ม GiantTrunade coverage |
| BookOfMoon | Priority system: enemy battle → own flip reuse → enemy threat |
| CreatureSwap | เลือก weakest monster (ATK < 1500) ให้ศัตรู |
| OjamaTrio | เช็ค empty zones ≥ 3 + standalone zone-lock |
| MirrorForce | เช็ค ATK ≥ 1000 หรือ multiple attackers |
| Scapegoat | เฉพาะ opponent's turn (ไม่ block Normal Summon) |

### `GOAT_PandaExecutor.cs` — 6 fixes
| แก้ไข | รายละเอียด |
|-------|-----------|
| InjectionFairyLily | ลบ `3400 >= 2000` constant-true; pump เฉพาะ beat-over/lethal |
| OjamaTrio | เช็ค empty zones ≥ 3 |
| MagicCylinder | ATK ≥ 1500 หรือ lethal burn |
| GravityBind | ไม่ activate ถ้ามี Level 4+ attacker ของเรา |
| HeavyStorm | ป้องกัน stall cards (GravityBind/LevelLimit) |
| Des Koala | MonsterRepos: stay in DEF for flip burn |

### `GOAT_RedEyesExecutor.cs` — 6 fixes
| แก้ไข | รายละเอียด |
|-------|-----------|
| Metamorphosis | `!IsAceCard(c)` ไม่ tribute boss + TER duplicate check |
| CyberStein | ไม่ summon TER ถ้ามีอยู่แล้ว |
| ArmedDragonLv3 | GetRemainingCount(LV5) check |
| SpiritRyu | Pump เฉพาะ lethal direct หรือ beat-over |
| KingDragun | MonsterCount < 5 zone check |
| MaskedDragon | Stay in DEF to float |

### `GOAT_SkillDrianExecutor.cs` — 7 fixes
| แก้ไข | รายละเอียด |
|-------|-----------|
| **FusilierSummon** | SkillDrain active → 2800 ATK; ไม่มี → fallback logic |
| **FinalAttackOrders** | Gate behind SkillDrain (ป้องกัน Goblin/Orc self-harm) |
| MonsterRepos | SkillDrain-aware + Fusilier half-stat handling |
| HeavyStorm/GiantTrunade | เพิ่ม executor + ป้องกัน SkillDrain |
| SolemnJudgment | เพิ่ม TorrentialTribute negate |
| MirrorForce | ATK ≥ 1500 threat check |
| RotA | GetRemainingCount deck check |

### `GOAT_WarriorExecutor.cs` — 7 fixes
| แก้ไข | รายละเอียด |
|-------|-----------|
| **BLS double attack** | `AI.SelectOption(1)` ทำให้ทำงานจริง |
| BLS banish | เพิ่ม face-down targets + lock-aware |
| DonZaloog | Smart option: hand ≥ 2 → discard, < 2 → mill |
| TribeInfectingVirus | Safe race cast + discard weakest non-ace |
| MirrorForce/SakuretsuArmor | ATK threshold checks |
| RotA | GetRemainingCount สำหรับทุก target |
| PrematureBurial | เพิ่ม Kycoo, Breaker, DonZaloog revive list |

---

## Build Status

| Project | Status | Notes |
|---------|:------:|-------|
| `WindBot.dll` | ✅ | 0 errors, 9 pre-existing warnings |
| `ExecutorBase.dll` | ✅ | 0 errors |
| `core.dll` | ✅ | 0 errors |

---

## Backward Compatibility

- ✅ ไม่แตะ `DefaultExecutor.cs` หรือ `ModernExecutor.cs`
- ✅ Legacy decks ไม่ได้รับผลกระทบ
- ✅ ทุก fix เป็น logic improvement ภายใน executor เฉพาะตัว
