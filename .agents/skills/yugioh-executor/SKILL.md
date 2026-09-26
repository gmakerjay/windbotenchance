---
name: yugioh-executor
description: |
  คู่มือพัฒนา Rule-Based WindBot Executor (C# / EDOPro) — ใช้เมื่อผู้ใช้พูดถึง WindBot, ModernExecutor,
  ExecutorBase, CardIntelligence, cards.cdb, .ydk, การแก้เด็ค, Hint ID, OnSelectCard, หรือปรับปรุง AI บอท
  ครอบคลุม: Card & Script Audit จริง, Decoupled Deck Plugin, Contextual Advantage & Risk Gate,
  OCGCore Hint Table (Audited), Action Scoring, Deploy Pipeline
---

# YugiohTH Executor Development Skill (Audited v11.0)
## Decoupled Plugin & Contextual Reasoning Architecture

เป้าหมาย: **บอทเล่นการ์ดถูกต้องตามกฎ ฉลาดเชิงกลยุทธ์ โค้ดสะอาด อ้างอิงข้อมูลจริงจาก Repository ปัจจุบัน 100%**

---

## 0. กฎเหล็ก 6 ข้อ (Strict Rules)

1. **ห้ามเดา ต้องอ้างอิงของจริงจาก Repo ปัจจุบัน 100%** — Card ID, Effect Text, Method Signatures, Target Framework ต้องเปิดยืนยันจากไฟล์จริงใน repo ปัจจุบัน (`*.csproj`, `ExecutorBase/`, `cards.cdb`, `script/cXXXX.lua`) ห้ามสมมติหรืออ้างอิงจากเอกสาร/ฟอร์กเก่า
2. **Audit ครบทั้ง Data และ Script** — อ่าน Main, Extra, Side จาก `.ydk` + ข้อมูลสถิติจาก `cards.cdb` และตรวจสอบ implementation จาก `script/cXXXX.lua` เมื่อการ์ดมีเอฟเฟกต์ซับซ้อน/ต่อเนื่อง/Timing
3. **Contextual Advantage & Risk Gate** — ห้ามมอบความได้เปรียบหรือทรัพยากรให้ศัตรูโดยไม่มีเหตุผลหรือการชดเชยเชิงกลยุทธ์ที่คุ้มค่า (Compensated Advantage)
4. **สถาปัตยกรรม Decoupled Deck Plugin** — แยกส่วนกลาง (`ModernExecutor`, `CardIntelligence`, `AIContext`) ออกจากความรู้เฉพาะเด็ค (`Deck Plugin / Domain Helpers`), 0 Magic Numbers (ใช้ `CardId`), 0 Build Errors
5. **ห้ามสร้างไฟล์เด็คซ้ำเด็ดขาด (Strict Anti-Duplication)** — 1 เด็คต้องมีไฟล์ `.ydk` Canonical เพียงไฟล์เดียวเท่านั้น ห้ามสร้างไฟล์ซ้ำ เช่น `_2026_Foo.ydk` คู่กับ `2026_Foo.ydk`, ห้ามใส่ขีดล่าง `_` นำหน้าชื่อเด็ค และต้องจำแนกประเภทเด็ค (Modern / Anime / Legacy / GOAT / Special) ให้ถูกต้องตามมาตรฐาน DashBot
6. **ห้ามรันการทดสอบเอง (STRICT)** — ห้ามรัน Headless Simulation โดยพลการ ผู้ใช้จะเป็นคนทดสอบเอง งานจบที่ Build ผ่าน + Deploy + บันทึกเอกสาร + รายงาน

### แหล่งข้อมูลอ้างอิงบังคับใน Repository ปัจจุบัน
- **Card Data**: `cards.cdb` (SQLite ตาราง `datas` + `texts`)
- **Card Scripts**: `script/cXXXX.lua` (ตรรกะการทำงาน, Cost vs Target, Timing ที่แท้จริง)
- **Deck List**: ไฟล์ `.ydk` ใน `windbot-fork/Decks/` หรือ `deck/`
- **Core Framework & Callbacks**: `src\YGO_SOURCE_CLEAN\windbot-fork\ExecutorBase\`
- **Hint Constants**: `script\constant.lua` และ `config\strings.conf` (ดูตารางหมวด 7)
- **Central Knowledge**: `CardIntelligence.cs` (ห้ามสร้างลิสต์ซ้ำในตัว Executor เด็ค)
- **Target Framework**: ตรวจสอบจาก `*.csproj` ใน repo ปัจจุบันเสมอ (เช่น `net10.0`)

---

## 1. Fast Card & Script Audit Protocol

ก่อนแตะต้องโค้ดคอมโบ ให้ตรวจสอบข้อมูลการ์ดจริงจาก `cards.cdb` และ `script/`:
```sql
SELECT d.id, t.name, d.type, d.atk, d.def, d.level, d.race, d.attribute, t.desc
FROM datas d JOIN texts t ON d.id = t.id WHERE d.id IN (<CardIDs_From_YDK>);
```
วิเคราะห์ 6 มิติของการ์ดแต่ละใบ:
1. **Role**: Starter / Extender / Handtrap / Board Breaker / Boss / Recovery / Brick
2. **Activation**: เงื่อนไขและเฟส (Main1/2, Battle, Damage Step, Quick Effect)
3. **Cost vs Target**: ค่า Cost ที่ต้องจ่ายแน่นอน vs สิ่งที่เป็นเพียง Target (ถ้าโดน Negate เสีย Cost ฟรีไหม)
4. **OPT Type**: Hard OPT (ระบุชื่อการ์ด), Soft OPT (ระบุ "1 ใบนี้"), ต่อ Chain, หรือไม่จำกัด
5. **Location**: มือ / สนาม / สุสาน / ถูกแบน
6. **Risk & Compensation**: มอบทรัพยากรให้ศัตรูไหม (ให้จั่ว, ให้มอนสเตอร์, ปูสุสาน) และมีผลตอบแทนเชิงกลยุทธ์คุ้มค่าหรือไม่

---

## 2. Standard Development Workflow

1. **Audit**: อ่าน `.ydk` + `cards.cdb` ทุกใบ, ตรวจสอบ `script/cXXXX.lua` หากมีเอฟเฟกต์ซับซ้อน, และอ่านโค้ด Executor เดิม
2. **Strategy Plan**: กำหนด Main Route, Backup Routes (เมื่อโดนขัด), First/Second Turn, Target End Board, และเกณฑ์ OTK
3. **Safety Pass**: ตรวจสอบผ่าน Contextual Advantage Gate (หมวด 4) และ Play-Correctness (หมวด 5)
4. **Implementation**: เขียน C# Rule-Based ตามสถาปัตยกรรม Decoupled Plugin (หมวด 6)
5. **Build & Deploy**: รันคำสั่งคอมไพล์ตาม repo (เช่น `BUILD_AND_DEPLOY.ps1`) ตรวจสอบว่าสำเร็จ 0 Errors
6. **Deck Sync**: คัดลอกไฟล์ `.ydk` ที่เกี่ยวข้องไปที่ `C:\Users\admin\Documents\EdoGame\deck\`
7. **Document**: บันทึกลง `PROGRESS.md` และสร้างรายงานสรุปใน `Docs/`
8. **Report**: รายงานผู้ใช้โดยสรุปการเปลี่ยนแปลงและ Combo Playbook (ไม่รัน Headless Test เอง)

---

## 3. Strategic Decision Framework

> **"Do not play cards. Play the game state."**

- **Main vs Backup Routes**: ออกแบบทางเลือกสำรองเสมอเมื่อ Starter ถูก Negate (เช่น โดน Ash, Impermanence, Nibiru)
- **Bait Strategy**: สละการ์ดมูลค่าต่ำเพื่อล่อ Negate ก่อนเปิดใช้เอฟเฟกต์สำคัญ
- **Resource Management**: เก็บ Handtrap / Follow-up ไว้ในมือสำหรับเทิร์นถัดไป ไม่ Overextend จนมือหมด
- **Action Scoring**: เลือก Action ตามผลลัพธ์คะแนนรวม (Value Score) ไม่ใช่ Hard-coded if-else ลำดับเดียว

---

## 4. Contextual Advantage & Risk Gate

การตัดสินใจทุกอย่างต้องผ่านการประเมินบริบทเชิงเปรียบเทียบ (Contextual Evaluation) แทนกฎตายตัว:

### 4.1 Contextual Removal Evaluation (ไม่ใช่ลำดับตายตัว)
การกำจัดมอนสเตอร์/การ์ดศัตรู ไม่ควรกำหนดตายตัวว่า Banish ดีกว่าเสมอ ให้คำนวณตามสูตร:
$$\text{RemovalScore} = \text{ThreatValue} + \text{ZoneDenial} + \text{RecursionPrevention} + \text{ChainSafety} - \text{OpponentRecoveryValue}$$
- **Banish**: ยอดเยี่ยมต่อเด็คพึ่งพาสุสาน (GY-reliant) แต่ต้องระวังเด็คที่เล่นกับ Banish Zone (เช่น Kashtira, Thunder Dragon)
- **Return to Deck (Spin)**: ปลอดภัยที่สุดต่อมอนสเตอร์ Extra Deck และมอนสเตอร์ที่มีเอฟเฟกต์ในสุสาน/แบน
- **Bounce to Hand**: เหมาะกับมอนสเตอร์ Extra Deck (ไม่คืนการ์ดขึ้นมือ) แต่ **ห้าม** เด้ง Normal Summon Starter กลับมือศัตรู
- **Destroy**: ใช้กับตัวที่ไม่มี Floating effect เลี่ยงตัวที่มีเอฟเฟกต์ "If destroyed..."
- **ตรวจ Immunity**: ตรวจสอบ `TargetImmuneMonsters` และ `DestructionImmuneMonsters` จาก `CardIntelligence` เสมอ

### 4.2 Compensated Advantage Rule (การให้ทรัพยากรศัตรูอย่างมีกลยุทธ์)
อนุญาตให้ใช้การ์ดที่มอบทรัพยากรให้ศัตรู (เช่น Kaiju, Lava Golem, Token) ได้เมื่อผ่านการตรวจสอบ 3 ข้อ:
1. **จำเป็นจริง (Necessary)**: กำจัดบอสที่เป็น Floodgate หรือ Unaffected ซึ่งเคลียร์ด้วยวิธีอื่นไม่ได้
2. **จัดการได้ในเทิร์นนั้น (Compensated)**: มี Removal ซ้ำ, ตีทะลุได้, หรือปิดเกม (Lethal) ชนะได้ทันที
3. **ปลอดภัย (Safe)**: ศัตรูไม่สามารถนำมอนสเตอร์นั้นไป Link/Xyz สวนกลับเราได้ในเทิร์นถัดไป

### 4.3 Nibiru Awareness: Risk Threshold (ไม่ใช่ Stop Rule)
การนับ Summon Count ครั้งที่ 4 หรือ 5 คือ **เกณฑ์วัดความเสี่ยง (Risk Threshold)** ไม่ใช่คำสั่งหยุดเล่นแบบทื่อๆ:
- **Summon #4 แล้วมี Omni-Negate พร้อมออก**: ให้เร่งออกตัว Negate (Baronne, Savage, Caesar, Lord Shi En) ก่อนครั้งที่ 5
- **Summon #4 แล้วมีสายสำรอง (Can play through)**: หากโดน Nibiru แล้วสุสานหรือบนมือยังต่อคอมโบได้ $\to$ เล่นต่อ
- **Summon #4 แล้วมีโอกาส OTK ได้ในเทิร์นนั้น**: เดินหน้าต่อเพื่อปิดเกม
- **Summon #4 แล้วเหลือแค่ Extender ไร้ประโยชน์**: หยุดเล่นเพื่อรักษากระดานไว้ ไม่เสี่ยงโดนล้างทั้งสนามฟรี

### 4.4 Maxx "C" & Droll Response: Minimum Extension (ไม่ใช่หยุดแบบไร้บอร์ด)
เมื่อติด Maxx "C" ให้ประเมิน **Draws Given vs Board Value**:
- **ห้าม Overextend**: ไม่ควร Special Summon 5+ ครั้งเพื่อสร้างบอร์ดเท่าเดิม (แจกการ์ด 5 ใบให้ศัตรู = แพ้)
- **Minimum Extension**: หาก Special Summon เพียง 1–2 ครั้งแล้วได้ 1 Interruption/Negate หรือได้ตัวป้องกันการโดน OTK ให้ทำทันที คุ้มค่ากว่าการ Pass Turn บนสนามเปล่าๆ
- เมื่อติด **Droll & Lock Bird**: ยกเลิกการค้นหา หันไปพึ่งทรัพยากรจากสนามและสุสาน

---

## 5. Play-Correctness Checklist

- [ ] **Timing**: ตรวจจับ Missing the Timing ("When... you can" vs "If... you can")
- [ ] **Chain Order**: เอฟเฟกต์ที่สำคัญที่สุดให้จัดลำดับให้ปลอดภัยจากการถูกขัด (Chain Blocking)
- [ ] **Zone Management**: เช็คพื้นที่ Monster Zone / S&T Zone / Extra Monster Zone ให้ว่างก่อนเรียก
- [ ] **Material Requirement**: ตรวจสอบจำนวนและเงื่อนไขวัตถุดิบ (Level, Attribute, Race) ให้ครบถ้วน
- [ ] **Ace Card Protection**: ไม่นำ Ace Monster ของบอร์ดสุดท้ายไปเป็นวัตถุดิบ Link/Synchro ต่อ เว้นแต่วางแผนไว้เป็นบันได
- [ ] **Zero Violations**: การเรียกหรือกดเอฟเฟกต์ที่ผิดกฎจนระบบปฏิเสธ ถือเป็น Bug ร้ายแรง

---

## 6. Architecture & Code Quality Standards

### 6.1 สถาปัตยกรรม 5 เลเยอร์ (Decoupled Deck Plugin Model)

```
Layer 1 — WindBot Core
          (Card, Field, Chain, Phase, Action Network Protocol)
                 │
                 ▼
Layer 2 — Generic AI (AIContext)
          (ThreatAnalyzer, BoardScorer, DynamicValueEvaluator, BeliefState)
                 │
                 ▼
Layer 3 — Deck Plugin / Domain Helpers
          (SixSamuraiHelper, DinomorphiaLPEconomy, SkyStrikerZoneHelper, etc.)
                 │
                 ▼
Layer 4 — Strategy & Action Scorer
          (Going 1st/2nd Lines, Bait Sequence, OTK Cutoff, Action Score Vector)
                 │
                 ▼
Layer 5 — Executor
          (ส่ง Action ที่มี Score สูงสุดไปยัง Engine)
```

### 6.2 Deck + Specific Helper Module Architecture (Mandatory Standard)
แนวทางมาตรฐานสำหรับเด็คใหม่ทุกเด็ค:
**ต้องเขียนเด็คควบคู่กับโมดูลช่วยเหลือเฉพาะเด็ค (Deck + Deck-Specific Helper Modules) เสมอ** เพื่อยกระดับความฉลาด ความเสถียรภาพ และความเข้าใจสถานการณ์ของบอทให้สูงสุด (AI ระดับ Hard/Master):

1. **`DeckPlugin`**: ผู้ประสานงานกลางและจัดการ State/Lifecycle ของเด็ค
2. **`DeckStrategy`**: จัดการ Combo Routing, First/Second Turn Lines, Target End Board, และ Breakpoints
3. **`DeckSpecificHelper(s)`**: โมดูลช่วยเหลือเฉพาะทางของแต่ละ Archetype (เช่น Counter Economy, Timing Advisor, Graveyard Manager, Contract Burn Guard, Tribute Resolver)
4. **`DeckMaterialScorer`**: ป้องกัน Ace Monsters และ Key Bosses ไม่ให้ถูกนำไปสังเวย/Link/Xyz ทิ้งอย่างสูญเปล่า
5. **`DeckBoardAssessor`**: ประเมินสถานการณ์บอร์ดสด คำนวณ Lethal และประเมินระดับภัยคุกคาม

> ⚠️ **นโยบายเด็คเก่า (Legacy Decks)**:
> เด็คเก่าที่ยังใช้งานได้ดี ให้คงไว้ตามเดิม **ห้ามแตะต้องหรือแก้ไขโดยไม่จำเป็น** เพื่อรักษาความเสถียรภาพ หากต้องการปรับปรุงให้เลือกขัดเกลาเฉพาะตามที่ผู้ใช้มอบหมายเท่านั้น

### 6.3 Smart Position Control & Handtrap Survival System (Universal Standard)
มาตรฐานบังคับใช้ใน Executor ของทุกเด็คใหม่:

1. **กฎเหล็กใน `OnSelectPosition` (ดัก 100% Defense สำหรับ 0/1800 และ Handtraps)**:
```csharp
public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
{
    if (positions == null || positions.Count == 0) return CardPosition.FaceUpAttack;
    if (positions.Count == 1) return positions[0];

    var cardData = YGOSharp.OCGWrapper.NamedCard.Get(cardId);
    if (cardData != null)
    {
        // Link Monsters cannot be in Defense
        if (cardData.HasType(CardType.Link))
            return CardPosition.FaceUpAttack;

        // 1. Handtraps (Ash 0/1800, Belle 0/1800, Veiler 0/0) หรือ 0 ATK -> บังคับ Defense 100%
        if (cardData.Attack == 0 || CardIntelligence.IsHandtrap(cardId))
        {
            if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
        }

        // 2. High DEF / Wall (DEF > ATK และ ATK < 1800 เช่น Trudea 1000/2000, Servant 900/1500) -> Defense
        if (cardData.Defense > cardData.Attack && cardData.Attack < 1800)
        {
            if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
        }

        // 3. มอนสเตอร์บอส / ATK สูง (>= 1800) -> Attack
        if (cardData.Attack >= 1800 && positions.Contains(CardPosition.FaceUpAttack))
            return CardPosition.FaceUpAttack;
    }

    return base.OnSelectPosition(cardId, positions);
}
```

2. **ระบบสลับท่านอนฉลาด (`SmartMonsterRepos`) ผ่าน `ExecutorType.Repos`**:
```csharp
AddExecutor(ExecutorType.Repos, SmartMonsterRepos);

private bool SmartMonsterRepos()
{
    if (Card == null) return false;
    // สลับมอนสเตอร์ 0 ATK, Handtrap หรือ DEF > ATK ที่เผลอยืนโจมตี ให้หมอบตั้งรับทันที
    if (Card.IsAttack() && (Card.Attack == 0 || (Card.Defense > Card.Attack && Card.Defense >= 1800)))
        return true;
    // สลับมอนสเตอร์พลังโจมตีสูง (ATK >= 1800) ให้เป็นตั้งโจมตีในเทิร์น 2+ เพื่อปิดเกม
    if (Card.IsDefense() && Card.Attack > Card.Defense && Card.Attack >= 1800 && Duel.Turn > 1)
        return true;
    return DefaultMonsterRepos();
}
```

3. **ล็อกคำสั่งกันตายฉุกเฉิน (Desperation Defense) ให้เป็น `MonsterSet` เสมอ**:
   - หากจำเป็นต้องส่ง Handtrap หรือตัว 0/1800 ลงมาเป็นโล่ป้องกันการโจมตีฉุกเฉิน ต้องส่งผ่านคำสั่ง `MonsterSet` (คว่ำป้องกัน) เท่านั้น **ห้าม Normal Summon หน้าหงายเด็ดขาด**
   - **ห้ามกั๊ก Handtrap**: Handtrap ยังคงมีบทบาทขัดขวางคู่ต่อสู้ใน Tier 0 ของ Pipeline เสมอ การส่งลงมาเป็นโล่กันตายจะเกิดขึ้นเฉพาะเมื่อจวนตัวและไม่มีทางเลือกอื่นเท่านั้น

### 6.4 กฎการเขียนโค้ด C#
1. **0 Magic Numbers**: Card ID ทุกตัวต้องอยู่ใน `public static class CardId`
2. **ใช้ Central Intelligence**: ตรวจสอบ Negator, Floodgate, Handtrap จาก `CardIntelligence`
3. **แยก Method สะอาด**: `ShouldActivateX()` (เงื่อนไข) + `ActivateX()` (การกระทำ)
4. **Early Exit**: ใช้ Guard clauses ลดความซับซ้อนของ `if` ซ้อนกัน (ไม่เกิน 3 ระดับ)

---

## 7. OCGCore Hint & Callback Engine (Audited 100%)

อ้างอิงตรงจาก `script\constant.lua` และ `config\strings.conf` ของระบบรันไทม์จริง:

### 7.1 ตาราง OCGCore Hint Message IDs (Audited)
| Hint ID | Constant Name | ความหมายจริง / พฤติกรรมที่ถูกต้อง |
|---|---|---|
| **500** | `HINTMSG_RELEASE` | บูชายัญ (Tribute) — เลือก Fodder/Token ก่อน ห้ามสังเวย Ace |
| **501** | `HINTMSG_DISCARD` | ทิ้งการ์ด — เลือกใบที่ทริกเกอร์ในสุสานหรือใบซ้ำ |
| **502** | `HINTMSG_DESTROY` | ทำลายการ์ด — เล็งการ์ดอันตรายของศัตรู เลี่ยงตัวที่อยากถูกทำลาย |
| **503** | `HINTMSG_REMOVE` | แบนการ์ด (Banish) — กำจัดมอนสเตอร์อันตรายของศัตรู |
| **504** | `HINTMSG_TOGRAVE` | ส่งลงสุสาน (GY) — ส่งชิ้นส่วนคอมโบหรือการ์ดที่มีเอฟเฟกต์ในสุสาน |
| **505** | `HINTMSG_RTOHAND` | เด้งกลับขึ้นมือ (Return to hand / Bounce) |
| **506** | `HINTMSG_ATOHAND` | **ค้นหา/เพิ่มขึ้นมือ (Search / Add to hand จาก Deck หรือ GY)** |
| **507** | `HINTMSG_TODECK` | นำกลับเข้าเด็ค / สปินเข้าเด็ค |
| **508** | `HINTMSG_SUMMON` | อัญเชิญแบบปกติ (Normal Summon) |
| **509** | `HINTMSG_SPSUMMON` | อัญเชิญแบบพิเศษ (Special Summon) |
| **510** | `HINTMSG_SET` | เซ็ตคว่ำลงสนาม |
| **511** | `HINTMSG_FMATERIAL` | วัตถุดิบ Fusion |
| **512** | `HINTMSG_SMATERIAL` | วัตถุดิบ Synchro |
| **513** | `HINTMSG_XMATERIAL` | วัตถุดิบ Xyz (วางเป็นวัตถุดิบใต้การ์ด) |
| **514** | `HINTMSG_FACEUP` | เลือกการ์ดที่หงายหน้า |
| **515** | `HINTMSG_FACEDOWN` | เลือกการ์ดที่คว่ำอยู่ |
| **516** | `HINTMSG_ATTACK` | เลือกมอนสเตอร์ตำแหน่งโจมตี |
| **517** | `HINTMSG_DEFENSE` | เลือกมอนสเตอร์ตำแหน่งป้องกัน |
| **518** | `HINTMSG_EQUIP` | เลือกการ์ดที่จะสวมใส่ (Equip Card) |
| **519** | `HINTMSG_REMOVEXYZ` | ปลดวัตถุดิบ Xyz ออกจากใต้การ์ด (Detach) |
| **520** | `HINTMSG_CONTROL` | เปลี่ยนการควบคุม / ขโมยมอนสเตอร์ (Change control) |
| **526** | `HINTMSG_CONFIRM` | แสดง/เปิดเผยการ์ด (Reveal) |
| **527** | `HINTMSG_TOFIELD` | นำการ์ดวางบนสนาม (เช่น วางในโซนเวท/กับดัก) |
| **528** | `HINTMSG_POSCHANGE` | เปลี่ยนสถานะการต่อสู้ (Attack/Defense Position) |
| **531** | `HINTMSG_TRIBUTE` | สังเวยเพื่อ Tribute Summon |
| **533** | `HINTMSG_LMATERIAL` | วัตถุดิบ Link |
| **551** | `HINTMSG_TARGET` | เลือกเป้าหมายของเอฟเฟกต์ (Target) |
| **555** | `HINTMSG_OPTION` | เลือกตัวเลือกของเอฟเฟกต์ (Option Selection) |
| **571** | `HINTMSG_TOZONE` | เลือกโซนที่จะย้ายการ์ดไป |
| **572** | `HINTMSG_COUNTER` | วางเคาน์เตอร์บนการ์ด |
| **575** | `HINTMSG_NEGATE` | เลือกการ์ดที่จะทำการ Negate / Disable เอฟเฟกต์ |
| **576** | `HINTMSG_ATKDEF` | เลือกการ์ดที่จะเปลี่ยนค่า ATK/DEF |
| **577** | `HINTMSG_APPLYTO` | เลือกการ์ดที่จะมีผลของเอฟเฟกต์ |
| **578** | `HINTMSG_ATTACH` | นำการ์ดมาเป็นวัตถุดิบใต้ Xyz (Attach as Material) |
| **579** | `HINTMSG_RTOGRAVE` | ส่งการ์ดกลับลงสุสาน |

> ⚠️ **คำเตือนความถูกต้องของ Hint**:
> - **506 คือ `HINTMSG_ATOHAND` (Search/Add to Hand) ที่ถูกต้องและเป็นสากล**
> - **ห้ามใช้ 573 เป็น Hint Search เด็ดขาด**: ใน strings.conf รหัส 573 คือข้อความปุ่มตัวเลือก (Option Text "Add the card(s) to your hand") ไม่ใช่ Hint Message Constant ของ OCGCore
> - `503` = Banish (ไม่ใช่ 504)
> - `504` = To Grave (ไม่ใช่ 508)
> - `507` = Return to Deck (ไม่ใช่ 506)
> - `518` = Equip (ไม่ใช่ 507 และไม่ใช่ PosChange)
> - `528` = Position Change (ไม่ใช่ 518)
> - `513` = Xyz Material / `519` = Detach Material
> - `575` = Negate / Disable (ไม่ใช่ 552 หรือ 572)

### 7.2 API Signature Rule (ตรวจสอบจาก Repo ปัจจุบัน)
**ห้ามสมมติ Method Signature จากเอกสารเก่า** ต้องเปิดยืนยันจาก `windbot-fork/ExecutorBase/` ใน repo ปัจจุบันเสมอ:
- `OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)`
- `OnSelectCounter(int type, int quantity, IList<ClientCard> cards, IList<int> counters)`
- `OnSelectOption(IList<int> options)`
- `OnSelectPosition(int cardId, IList<CardPosition> positions)`
- `OnSelectPlace(int cardId, int player, CardLocation location, int available)`

---

## 8. Deck Taxonomy & Anti-Duplication Standards (DashBot Alignment)

### 8.1 กฎเหล็ก Anti-Duplication (1 Deck = 1 Canonical File)
- **ห้ามสร้างไฟล์ `.ydk` ซ้ำเด็ดขาด**: ในระบบต้องมีไฟล์ `.ydk` สำหรับเด็คนั้นเพียงไฟล์เดียวที่เป็น Canonical
- **ห้ามใส่เครื่องหมายขีดล่าง `_` นำหน้าชื่อไฟล์เด็ค**: เช่น `_2026_SixSamurai.ydk` (❌ ห้ามทำ) $\to$ ต้องใช้ `2026_SixSamurai.ydk` (✅ ถูกต้อง) เพราะ DashBot จะมองเป็นคนละเด็คและแสดงผลชื่อเบิ้ลซ้ำใน UI
- **การผูกแอตทริบิวต์ใน Executor**: ใช้ชื่อเดียวกับไฟล์ Canonical เช่น `[Deck("2026_SixSamurai")]`
- **การลงทะเบียนใน `bots.json`**: ให้ฟิลด์ `"deck"` ชี้ไปที่ชื่อ Canonical เดียวกันเสมอ (สามารถเพิ่มชื่อเล่น/Alias ในฟิลด์ `"name"` ได้โดยไม่ต้องสร้างไฟล์เด็คซ้ำ)

### 8.2 การจำแนกประเภทเด็คตามมาตรฐาน DashBot Launcher (Deck Taxonomy)
DashBot จำแนกประเภทเด็คและกำหนดสีป้ายกำกับอัตโนมัติจาก Prefix ของชื่อไฟล์ `.ydk` ดังนี้:

| หมวดหมู่ (Category) | Prefix / รูปแบบชื่อไฟล์ | แท็กใน DashBot | สีป้ายกำกับ | นิยามและความเหมาะสมในการใช้งาน |
|---|---|---|---|---|
| **Modern** | `2026_<Name>.ydk`<br>หรือ Archetype โมเดิร์น (เช่น `ADML`, `AFS`, `CenturIon`, `VoicelessVoice`, `Tenpai`, `WhiteForest`, `Kashtira`) | Modern | สีทอง Amber (`#D97706`) | **เด็คเมต้าปัจจุบัน (ยุค Master Duel / OCG / TCG ปี 2024–2026)**<br>เช่น `2026_SixSamurai.ydk`, `2026_Branded.ydk`, `2026_Purrely.ydk`, `2026_Dinomorphia.ydk` |
| **Anime** | `Anime_<Name>.ydk` | Anime | สีชมพู Rose (`#BE185D`) | **เด็คตัวละครจากอนิเมะ** ที่มีบทพูดและการเล่นตามสไตล์บทบาท เช่น `Anime_Yugi.ydk`, `Anime_JackAtlas.ydk`, `Anime_JoeyWheeler.ydk` |
| **Legacy** | `AI_<Name>.ydk` | Legacy | สีน้ำเงิน Royal Blue (`#1D4ED8`) | **เด็ค AI ดั้งเดิมของ WindBot** และเด็คคู่ซ้อมมาตรฐาน 4 เด็ค (`AI_BlueEyes.ydk`, `AI_DarkMagician.ydk`, `AI_Altergeist.ydk`, `AI_ABC.ydk`) |
| **GOAT** | `GOAT_<Name>.ydk` | GOAT | สีเขียว Emerald (`#047857`) | **เด็คฟอร์แมตย้อนยุค GOAT** (เมษายน 2005) เช่น `GOAT_Standard.ydk`, `GOAT_Chaos.ydk` |
| **Special** | ชื่ออื่นๆ นอกเหนือจากข้างต้น (ไม่มี Prefix มาตรฐาน) | Special | สีม่วง Purple (`#6D28D9`) | **เด็คเฉพาะกิจ**, Puzzle, มินิเกม, เด็คทดสอบเฉพาะกิจ, หรือ Custom Fun format |

---

## 9. Build & Exclusive Deployment Pipeline

### ตรวจสอบ Framework จาก `*.csproj` ใน Repo ปัจจุบัน
ก่อนคอมไพล์ ให้ยืนยัน Target Framework จากไฟล์โปรเจกต์ (ปัจจุบันคือ .NET 10):
```powershell
cd C:\Users\admin\Documents\EdoGame\src\YGO_SOURCE_CLEAN
powershell -ExecutionPolicy Bypass -File .\BUILD_AND_DEPLOY.ps1
```

### Strict Deployment Target
- ไฟล์ไบนารีต้อง Deploy ไปที่ **`C:\Users\admin\Documents\EdoGame\` เท่านั้น**
- คัดลอกไฟล์เด็ค `.ydk` ทั้งหมดไปที่ `C:\Users\admin\Documents\EdoGame\deck\`
- บันทึกการเปลี่ยนแปลงใน [PROGRESS.md](file:///C:/Users/admin/Documents/EdoGame/PROGRESS.md) และ `Docs/`

---

## 10. Testing Policy (STRICT)

- **ห้ามรัน Headless Simulation เองเด็ดขาด** เว้นแต่ได้รับคำสั่งเฉพาะเจาะจงจากผู้ใช้
- เมื่อผู้ใช้สั่งให้ทดสอบ ให้ใช้คำสั่ง:
```powershell
dotnet run --project src\YGO_SOURCE_CLEAN\Client_Headless_Fortest\Client_Headless_Fortest.csproj -c Release -- --deck <DECK_NAME> --opponent <OPPONENT> --games 10 --timeout 60
```
- ในสถานการณ์ปกติ ให้จบงานที่ Build & Deploy สำเร็จ และให้ผู้ใช้ทดสอบด้วยตนเอง