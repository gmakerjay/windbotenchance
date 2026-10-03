# Case Studies & Deck-Archetype Patterns (Reference)

> ไฟล์อ้างอิงของ skill `yugioh-executor` — เปิดอ่านเฉพาะเมื่อทำงานกับเด็ค/กลไกที่เกี่ยวข้อง
> ข้อมูลเฉพาะเด็คที่ละเอียดกว่านี้ให้ดูใน `Docs/<DECK>_AUDIT_AND_PLAYBOOK.md`

## 1. Risk Thresholds 🎯

- **Nibiru**: ซัมมอนครั้งที่ 4 = *Risk Threshold* ไม่ใช่ Stop Rule ประเมินที่ครั้งที่ 4:
  มี Negate คุ้มกันแล้ว → เดินต่อ / ครั้งที่ 5 ออก Negate ได้พอดี → เดินต่อ /
  Lethal Confirmed หรือสนามศัตรูโล่ง → เข้า Battle / มือเหลือแต่ Extender ที่ไม่นำไป Negate → หยุดที่ 4
- **Maxx "C"**: อย่า Pass ทันที คำนวณ Minimum Extension (Draw ที่ให้ vs Board Value vs จบเกมได้ไหม)
  ปกติ SS 1–2 ครั้งแล้วได้ Negate 1 ใบ ดีกว่าปล่อยสนามว่าง

## 2. Interruption Density 🎯 (แนวทาง ไม่ใช่โควตา)

- เด็ค 40 ใบ: Handtrap + Board Breaker ราว **9–15 ใบ**, Starter ราว 9–12 ใบ
- หลีกเลี่ยง Over-combofication (Starter/Extender ล้น ไม่มีที่ให้ Interaction)
- **ไปก่อน**: End Board ที่มี Interruption หลายมิติ / **ไปหลัง**: Handtrap ตัด Chokepoint + Board Breaker ก่อนเดิน Engine

## 3. DeckProbability Engine (`WindBot.Game.AI.DeckProbability`)

- `Hypergeometric`, `AtLeastOne(N,K,n)`, `AtLeast` — โอกาสเปิด Starter (เป้า ≥ ~85%)
- `BanishLossRisk(remainingDeck, criticalCopies, banishCount)` — Pot of Desires: เสี่ยงชิ้นสำคัญหลุด → Search ก่อน Draw
- `MultivariateHypergeometric` — โอกาส Starter + Extender โดยไม่มี Garnet
- `BaitPlanner.EstimateHandTrapLikelihood(...)` — ตัดสินใจส่ง Bait จากตัวเลข

## 4. Conjunction Fallacy & Attack-Negation Chaining 🔒

- `Magic Cylinder` (62279055): *"negate the attack, **and if you do**, inflict damage..."*
- Chain `Magic Cylinder` / `Dimension Wall` (67095270) ซ้อนกัน → CL1 ยกเลิกการโจมตีไม่ได้ (ถูกยกเลิกแล้ว) → ส่วน B ไม่เกิด → 0 Damage เสียการ์ดฟรี
- Guard: `if (Duel.CurrentChain.Any(c => c.IsCode(CardId.MagicCylinder, CardId.DimensionWall))) return false;`
- Double Damage ที่ถูกต้อง = GY effect ของ `Magical Cylinders` (15943341) ไม่ใช่การ chain ซ้อน

## 5. Backrow Fortress (รับมือ Mass Backrow Wipe) 🎯

Mass wipe IDs: Harpie's Feather Duster (18144506/18144507), Lightning Storm (14532163),
Evenly Matched (15693423), Heavy Storm (19613556), Twin Twisters (43898403), Red Reboot (23002292)

1. **Layer 1 – Aura Immunity**: `Lord of the Heavenly Prison` (9822220) หงายมือ → การ์ดหมอบไม่ถูกทำลายด้วยผล
2. **Layer 2 – Spell Speed 3**: `Solemn Judgment` (41420027), `Dark Bribe` (77538567) + `IsMassBackrowWipe(card)` = Tier 0
3. **Layer 3 – Floating Punishment**: `Waking the Dragon` (10813327) → Ultimate Falcon (86221741) / Last Warrior (86099788) / Baronne (84815190)

## 6. Dead Hand / Brick Mitigation 🎯

- Consistency: Pot of Duality (98645731), Pot of Prosperity (84211599), Lilith (23898021), Wannabee! (3248469), Trap Trick (80101899)
- Forced Aggression: `Battle Mania` (31245780) ใน Standby ศัตรู; Kaiju ATK สูงเป็นเป้าสะท้อน
- Alt Win-Con: Heavenly Prison x2 → Gustav Max → Juggernaut Liebe

## 7. Endymion Spell Counter Control (Counter Economy)

```csharp
public enum CounterLevel { Critical = 0 /*0-1*/, Low = 1 /*2-3*/, Ready = 2 /*4-5*/, ComboReady = 3 /*6-7*/, Surplus = 4 /*8+*/ }
```

- **Jackal King** (2 Counter = Negate 1 ครั้ง): Boss Effect > Search/Starter > Removal > Extender > Minor; เก็บสำรอง 2 เม็ด
- **Endymion, Mighty Master**: Bounce Recycling (ใช้ซ้ำได้ → หมดหน้าที่ → Counter สูง → ห้ามคืน Key Card); 6-Counter Wipe เมื่อเคลียร์ Threat ≥ 2 หรือเปิด Lethal
- **NetGain** = NewCounters − CountersSpent (เน้น ≥ 0); Emergency Breach ใช้ต่ำกว่า Reserve ได้เมื่อจะแพ้

## 8. Six Samurai (Bushido Economy)

- Loop: `Gateway of the Six` (4 counters) + `Battle Shogun` (Link-2 เสิร์ช Gateway)
- Interruption: `Shi En` (S/T Negate), `Great Shogun Shien`, `Six Style - Dual Wield`
- `Kizaru`: เสิร์ชตัวที่ Attribute ต่างจากบนสนาม

## 9. Default Exceptions Table 🎯

| ค่าเริ่มต้น | ยกเว้นได้เมื่อ |
|---|---|
| **S:P Little Knight (29301450)** ไม่เรียก MP1 เมื่อตีตรงได้ (ล็อก direct attack ทั้งเทิร์น) | ต้องใช้ผลเคลียร์ Threat ทันที |
| **Number 41: Bagooska** ตั้ง FaceUpDefence | ต้องโจมตีปิดเกม |
| **Flat LP Cost**: `LP − cost > max(2000, 500 × actions ภายใต้ Chain Energy)` | ศัตรูไม่มี burn/floodgate และจ่ายแล้วปิดเกม |
| Fusion/Synchro/Xyz ลง MMZ เก็บ EMZ ให้ Link | MMZ เต็ม |
| Handtrap เก็บบนมือ ไม่หมอบ MP1 | ต้องลงสนามจึงใช้ได้ / มือล้น |
