# Central Core Competitive Heuristics Audit & Playbook (2026)

## 📌 Executive Summary
ยกระดับสถาปัตยกรรม **Central Core** (`DefaultExecutor`, `ModernExecutor`, `ChainTimingAdvisor`, `BaitPlanner`) สู่มาตรฐาน **Competitive Tournament Mindset** เพื่อให้บอททุกตัวในระบบเล่นได้อย่างมีชั้นเชิงเหมือนนักแข่ง โดยไม่ต้องแก้ไขโค้ดทีละเด็ค

---

## 🛠️ Implemented Systems & Competitive Heuristics

### 1. Universal Handtrap Anti-Bait Guard
- **Anti-Bait Protection**:
  - เมื่อบอทถือ Handtrap เพียง 1 ใบ (หรือมี Disruption ในมือจำกัด) **ห้ามยิงใส่การ์ดจั่ว/การ์ดล่อ** เช่น `Pot of Prosperity` (84211599), `Pot of Desires` (35261759), `Pot of Extravagance` (49238328/49238329), `Pot of Duality` (98645731), `Upstart Goblin` (70368879), `Chicken Game` (67616300), `Into the Void`, `Dark World Dealings`, `Hand Destruction`
  - บอทจะสงวน Handtrap ไว้ตัด Chokepoint หรือ Starter แกนหลักของคู่แข่งเท่านั้น
- **Anti-Redundant Negation**:
  - ตรวจสอบ `Duel.CurrentChain` เสมอ หากมีตัว Negate ของเราทำงานอยู่แล้ว ห้ามกด Handtrap ซ้ำซ้อนให้เสียการ์ดฟรี
- **Preemptive Impermanence**:
  - ใน Turn 2 (Going Second) ช่วง Main Phase 1 หากบอทยังไม่มีมอนสเตอร์บนสนาม และคู่แข่งมี Floodgate/Boss Negator (เช่น Bagooska, Apollousa, Baronne) ให้ยิง Imperm จากมือก่อนลง Normal Summon
- **Mulcharmy Integration**:
  - เพิ่ม `DefaultMulcharmyFuwalos` และ `DefaultMulcharmyPurulia` สำหรับดักการอัญเชิญตามกติกา

### 2. Modern Meta Chokepoint & Threat Matrix
อัปเดต Card ID ที่เป็น Chokepoint สำคัญลงใน `ChainTimingAdvisor` (ตรวจสอบ ID จริงจาก `cards.cdb`):
- **Ryzeal**: Ice Ryzeal (`8633261`), Ryzeal Duo Drive (`7511613`), Ext Ryzeal (`34022970`), Node Ryzeal (`72238166`), Sword Ryzeal (`35844557`), Palm Ryzeal (`61116514`), Ryzeal Detonator (`34909328`), Ryzeal Cross (`6798031`), Ryzeal Plugin (`60394026`)
- **Maliss**: Maliss <P> White Rabbit (`69272449`), Maliss <P> Dormouse (`32061192`), Maliss <P> Chessy Cat (`96676583`), Maliss in Underground (`68337209`/`68337210`), Maliss <Q> Red Ransom (`68059897`), Maliss <Q> Hearts Crypter (`21848500`), Maliss <Q> White Binder (`95454996`)
- **Yubel & Fiendsmith**: Fiendsmith Engraver (`60764609`), Nightmare Throne (`93729896`), Phantom of Yubel (`80453041`), Samsara D Lotus (`62318994`)
- **Tenpai Dragon**: Tenpai Dragon Paidra (`39931513`), Sangen Kaimen (`66730191`)
- **Voiceless Voice**: Lo, the Prayers (`25801745`), Barrier of the Voiceless Voice (`98477480`)
- **Centur-Ion**: Centur-Ion Primera (`15005145`), Stand Up Centur-Ion! (`41371602`), Centur-Ion Primera Primus (`8841431`)
- **Orcust**: Orcust Harp Horror (`57835716`), Galatea (`30741503`), Girsu (`69811710`), Orcust Crescendo (`703897`)

### 3. Competitive Nibiru Apex Drop Heuristic
- **Boss Safeguard**: ห้ามทุบล้างสนามตนเองหากเราคุมบอร์ดบอส/Ace ชนะอยู่แล้ว และคู่แข่งไม่มีดาเมจ Lethal
- **Apex Board Drop**: ทุบเมื่อคู่แข่งสะสมมอนสเตอร์ $\ge 3$ ตัว หรือพลังโจมตีรวม $\ge 3500$
- **Preemptive Negate Drop**: ทุบตัดหน้าก่อนคู่แข่งจะทำมอนสเตอร์ Omni-Negate สำเร็จ
- **Phase Transition Catch**: ดักทุบที่จุดจบ Main Phase หรือก่อนเข้า Battle Phase เพื่อตัดกำลังบุกทั้งหมด

### 4. Going Second Bait Sequencing
- เพิ่มการ์ดล่อและบอร์ดเบรกเกอร์ (`Triple Tactics Talent` 25311006, `Triple Tactics Thrust` 35269904, `Raigeki` 12580477, `Dark Hole` 53129443, `Harpie's Feather Duster` 18144506) ลงใน `BaitPlanner`

---

## 📊 Headless Simulation Results (Client_Headless_Fortest)

### Matchup 1: `2026_Orcust` vs `BlueEyes`
- **Games**: 2/2
- **Result**: 2 Wins (100.0% Win Rate), 0 Losses, 0 Draws
- **Violations**: 0
- **Average Duration**: 18.0 วินาที/เกม

### Matchup 2: `2026_Orcust` vs `DarkMagician`
- **Games**: 2/2
- **Result**: 2 Wins (100.0% Win Rate), 0 Losses, 0 Draws
- **Violations**: 0
- **Average Duration**: 12.8 วินาที/เกม
- **Highlight**: บอทอ่าน Lethal Confirmed เข้าสู่ `Rush Mode` ข้ามการอัญเชิญส่วนเกินและเข้าตีปิดเกมทันที
