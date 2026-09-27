# WindBot Deck Plugin Architecture

โฟลเดอร์นี้รวบรวมไฟล์ Domain Plugin ของแต่ละเด็ค (`IDeckPlugin` / `DeckPluginBase`) โดยแยกไฟล์ออกมาจาก Executor อย่างชัดเจนตามหลักการ Separation of Concerns และ Decoupled Domain Plugin Architecture

## 📁 โครงสร้างสถาปัตยกรรม (Decoupled Domain Plugin)

```
Game/AI/
├── Decks/                 ← Router, Card Activation Rules, Decision Tree (Executor)
└── Plugins/               ← Domain State, Strategy, Material & Threat Scoring (Plugin)
    ├── KewlTunePlugin.cs
    ├── GravePlugin.cs
    ├── FireKingPlugin.cs
    ├── EndymionPlugin.cs
    ├── SixSamuraiPlugin.cs
    └── ...
```

---

## 🧩 ส่วนประกอบหลักของ IDeckPlugin

แต่ละ Deck Plugin สืบทอดจาก `DeckPluginBase` (หรือ implement `IDeckPlugin`) ซึ่งประกอบด้วยโมดูลย่อยดังนี้:

| Interface | หน้าที่หลัก |
|---|---|
| `IDeckStrategy` | การเลือกการ์ด Special Summon, Search, และการวาง Route เชิงกลยุทธ์ |
| `IDeckMaterialEvaluator` | การจัดลำดับความสำคัญของวัตถุดิบ (Material Cost / Discard / Fusion / Synchro / Link) |
| `IDeckThreatEvaluator` | การประเมินคะแนน Threat ของการ์ดฝ่ายตรงข้าม (Floodgate, Boss, Interruption) |
| `IDeckResourceEvaluator` | การบริหารจัดการ Resource เฉพาะเด็ค (เช่น Counters, Scaled Resources) |
| `IDeckActionScorer` | การให้คะแนน Action ในสถานการณ์ซับซ้อน |
| `IDeckScaleResolver` | การคำนวณและเลือก Pendulum Scale ที่เหมาะสมที่สุด |

---

## 🛠️ วิธีการเชื่อมต่อ Plugin เข้ากับ Executor

ใน Constructor ของ Executor ให้กำหนด `DeckPlugin` ดังนี้:

```csharp
using WindBot.Game.AI.Plugins;

namespace WindBot.Game.AI.Decks
{
    [Deck("MyDeck", "MyDeck")]
    public class MyDeckExecutor : ModernExecutor
    {
        public MyDeckExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            DeckPlugin = new MyDeckPlugin(this);
            // ...
        }
    }
}
```
