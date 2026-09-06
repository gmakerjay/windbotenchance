# Client_Headless_Fortest — Headless Duel Simulation Guide

> **เครื่องมือจำลองการดวล Yu-Gi-Oh! แบบ Text-Based Headless**  
> ใช้สำหรับทดสอบตรรกะ AI, ตรวจสอบความถูกต้องของกฎ (0 Violations), ป้องกันเกมค้าง/แครช (0 Crashes), และวัดผลสถิติ Win Rate ของแต่ละเด็ค

---

## 🚀 วิธีการใช้งาน (Quick Start)

รันคำสั่งผ่าน PowerShell หรือ Command Prompt จากโฟลเดอร์ `C:\Users\admin\Documents\EdoGame\`:

```powershell
dotnet run --project src\YGO_SOURCE_CLEAN\Client_Headless_Fortest\Client_Headless_Fortest.csproj -c Release -- --deck <DECK_NAME> --opponent <OPPONENT_NAME> --games 10 --timeout 60
```

---

## 📋 พารามิเตอร์ทั้งหมด (Command-Line Arguments)

| Argument | Description | Default | Example |
|---|---|---|---|
| `--deck <name>` | ชื่อเด็ค AI หลักที่ต้องการทดสอบ (Bot A) | `2026_Branded` | `--deck 2026_Branded` |
| `--opponent <name>` | ชื่อเด็คคู่ซ้อม (Bot B) | `BlueEyes` | `--opponent Altergeist` |
| `--games <count>` | จำนวนแมตช์ที่ต้องการให้ดวล | `10` | `--games 20` |
| `--timeout <sec>` | เวลาจำกัดต่อ 1 เกม (วินาที) | `120` | `--timeout 60` |
| `--parallel <n>` | รันการดวลหลายโต๊ะพร้อมกัน (Parallel Workers) | `Off (Sequential)` | `--parallel 4` |
| `--port <port>` | Base Port สำหรับ Socket Server | `20000` | `--port 20000` |
| `--forest` | โหมดทัวร์นาเมนต์พิเศษ Round-robin | `Off` | `--forest` |

---

## 🎯 เด็คคู่ซ้อมมาตรฐานสำหรับการทดสอบ (Legacy Benchmark Suite)

ในการพัฒนาหรือปรับปรุง Executor ต้องทดสอบกับเด็คมาตรฐานทั้ง 4 สายนี้เพื่อวัดความสมบูรณ์รอบด้าน:

1. **`ABC` (Machine / Light / Disruption)**:
   - ทดสอบความสามารถในการรับมือกับ Quick Banishing (`ABC-Dragon Buster`), การ Bait ขัดขวาง และ Link Spam
2. **`Altergeist` (Trap Control / Resource Grind)**:
   - ทดสอบการรับมือกับกับดักหน่วงเกม, Counter Trap (`Protocol`), Handtrap สลับเด้งการ์ด (`Multifaker`, `Silquitous`)
3. **`BlueEyes` (High ATK Beatdown / Rank 8 / Synchro)**:
   - ทดสอบการรับมือกับมอนสเตอร์พลังโจมตีสูง (3000+ ATK), การตั้งรับ (Defense Position), และการทำลายมอนสเตอร์ที่ป้องกันการตกเป็นเป้า
4. **`DarkMagician` (Backrow Banish & Field Spell Wipe)**:
   - ทดสอบความสามารถในการเล็งเป้าทำลายการ์ดต่อเนื่องหลักอย่าง `Eternal Soul` (ชี้ขาดเกม) และ `Dark Magical Circle`

---

## 🔍 การอ่านผลลัพธ์และสถิติ (Interpreting Results)

เมื่อการทดสอบสิ้นสุด ระบบจะสรุปรายงานผลการดวลออกมาในรูปแบบ:

```text
==================================================================
 VERIFICATION SUMMARY
==================================================================
Total Duels Completed : 10
Bot A Wins (2026_Branded): 8 (80.0%)
Bot B Wins (BlueEyes)    : 2 (20.0%)
Draws / Unfinished       : 0
Rule Violations          : 0
Engine Crashes / Errors  : 0
Avg Duel Duration        : 14.2s
==================================================================
```

### เกณฑ์ความสำเร็จ (Pass Criteria):
- **Rule Violations ต้องเป็น 0 เสมอ**: AI ต้องไม่กระทำผิดกฎ หรือส่ง Action ที่ OCGCore ปฏิเสธ (เช่น `MSG_RETRY`)
- **Engine Crashes ต้องเป็น 0 เสมอ**: บอทต้องไม่เกิด `NullReferenceException`, Memory Leak, หรือหลุดการเชื่อมต่อ
- **Win Rate มีทิศทางดีขึ้น**: อัตราการชนะต่อคู่ซ้อมทั้ง 4 เด็คสะท้อนถึงการตัดสินใจและคอมโบที่มีประสิทธิภาพ

---

## 📁 โครงสร้าง Log และไฟล์บันทึก

```
src/YGO_SOURCE_CLEAN/logs/
├── headless/                 # บันทึกข้อความและแชทแบบละเอียดของแต่ละเกม
└── error/                    # รายงานกรณีเกิดข้อผิดพลาดหรือ Crash
```
