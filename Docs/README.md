# YugiohTH Documentation Index

เอกสารทั้งหมดของระบบ YugiohTH WindBot & DashBot ถูกจัดหมวดหมู่อย่างเป็นระเบียบตามโครงสร้างดังนี้:

## 📁 โครงสร้างโฟลเดอร์เอกสาร (`Docs/`)

```
Docs/
├── DeckReports/          ← รายงานการเพิ่มประสิทธิภาพ, ผลการทดสอบดวล, และการ Audit รายเด็ค
├── Architecture/         ← เอกสารสถาปัตยกรรมระบบหลัก (Core, OCGCore Protocol, Decoupled Domain Plugins)
├── Archives/             ← ประวัติการพัฒนาย้อนหลัง (PROGRESS_ARCHIVE.md)
└── README.md             ← ดัชนีสารบัญเอกสาร (ไฟล์นี้)
```

---

## 1. 🗂️ Deck Reports (`Docs/DeckReports/`)
รายงานการปรับปรุงประสิทธิภาพ, แผนการเล่น, ผลการวิเคราะห์ Headless Simulation รายเด็ค:

| ไฟล์เอกสาร | คำอธิบายเด็ค / สาระสำคัญ |
|---|---|
| `2026_Kwtune_Grave_Optimization_Report.md` | KewlTune & Gravekeeper Audit, Decoupled Plugin, S:P Attack Lockout fix |
| `2026_Monarch_Refactor_Report.md` | Monarch Domain Lock & Tribute Engine Refactoring |
| `2026_Purrely_Yummy_Optimization_Report.md` | Purrely & Yummy Quick-Play engine optimization |
| `2026_SIX_SAMURAI_REPORT.md` | Six Samurai Gateway Infinite Loop & Decoupled Domain Plugin |
| `2026_Solfachord_Refactor_Report.md` | Solfachord Pendulum Scale Navigation & Harmonia Engine |
| `2026_Tearla_Stun_Optimization_Report.md` | Tearlaments Fusion Mil & Stun Floodgate Evaluation |
| `AFS_Executor_Fix_Report.md` | Azamina Fiendsmith Snake-Eye Link Climbing & Route Routing |
| `ANIME_5_DECK_REPORT.md` | 5 Anime Decks (Jack Atlas, Joey, Yugi, Yusei, Kaiba) Comprehensive Audit |
| `ANIME_JOEY_WHEELER_REPORT.md` | Joey Wheeler Red-Eyes & Gambler Engine Optimization |
| `ArtMage_Optimization_Report.md` | Artmage Board Wipe & Spell Synergy Evaluation |
| `BIG_SHIELD_COUNTER_REPORT.md` | Big Shield Gardna & Defense Reflect OTK Strategy |
| `DEFENSE_REFLECT_OTK_DECKS_REPORT.md` | Stone Statue / Aztec Rock OTK Audit |
| `GOAT_FORMAT_REFACTOR_REPORT.md` | GOAT Format Legacy AI Ruleset (Skill Drain, Panda, Warrior, etc.) |
| `Report_2026_Endymion_Centurion_DDD.md` | Endymion Spell Counter & Centur-Ion Synchro Placement Audit |
| `Report_2026_Morganite_Drytron_Madolche.md` | Time-Tearing Morganite, Drytron Ritual & Madolche Spin Engine |
| `WC_PARIS_KEWLTUNE_REPORT.md` | World Championship Paris KewlTune Prototype Report |

---

## 2. 🏛️ System & Architecture (`Docs/Architecture/`)
เอกสารสถาปัตยกรรมภายในของ WindBot และการเชื่อมต่อ OCGCore:

| ไฟล์เอกสาร | รายละเอียด |
|---|---|
| `CORE_AI_REFACTORING_REPORT.md` | สถาปัตยกรรม ModernExecutor, ComboRouter, CardIntelligence O(1) |
| `Dedicated_Module_Audit_And_Safety_Report.md` | มาตรฐานความปลอดภัยในการเขียน Executor และการป้องกัน Anti-patterns |
| `OCGCore_Hint_Audit_Report.md` | มาตรฐาน OCGCore Hint Messages (502 Destroy, 503 Banish, 506 ToHand) |
| `AUDIT_AND_OPTIMIZATION_REPORT_2026.md` | ภาพรวมการ Audit ทั้งระบบและสถานะเด็ค |
| `HANDOFF_STRATEGY_OPTIONS.md` | แนวทางการส่งมอบงานและโครงสร้างสถาปัตยกรรมระยะยาว |

---

## 3. 📜 Historical Archives (`Docs/Archives/`)
- `PROGRESS_ARCHIVE.md`: บันทึกประวัติการพัฒนาตั้งแต่เวอร์ชันแรกจนถึง 0.045
