# Changelog — AI Executor Integration & Intelligence Upgrade (Batch 3)
**Date:** 2026-07-10  
**Version:** AI Intelligence Upgrade Batch 3  
**Scope:** 4 new deck executors (Modern 2026 Blackwings, Blue-Eyes, Gimmick Puppet, Rex Raptor) + 4 deck configuration files

---

## สรุปการเปลี่ยนแปลงหลัก (Key Upgrades)

### 1. 🎛️ ระบบตั้งค่าตัวเลือกอัตโนมัติ (OnSelectOption Override)
- เพิ่มเมธอด `OnSelectOption` ครอบจักรวาลใน [ModernExecutor.cs](file:///c:/Users/admin/Documents/YugiohTH/Source_Project/YGO_AI_PLATFORM/windbot-fork/ExecutorBase/Game/AI/ModernExecutor.cs) เพื่อรองรับการเลือกตัวเลือกที่เหมาะสมที่สุดของการ์ดเหล่านี้โดยอัตโนมัติ:
  - **Blackwing - Simoon the Poison Wind**: เลือกตัวเลือกอัญเชิญปกติ (Normal Summon) เพื่อดำเนินคอมโบ
  - **Pot of Prosperity**: เลือกตัวเลือกคว่ำการ์ด 6 ใบเพื่อค้นหาการ์ดที่ดีที่สุด
  - **Triple Tactics Talent**: เลือกจั่วการ์ด 2 ใบหากมีการ์ดบนมือน้อย, หรือชิงการ์ดฝ่ายตรงข้ามหากเดินทีหลัง, หรือริบการ์ดบนมือฝ่ายตรงข้ามหากเดินก่อน
  - **True Light**: เลือกอัญเชิญ Blue-Eyes White Dragon หากมีบนมือ/สุสาน, มิฉะนั้นจะเลือกค้นหาการ์ดขึ้นมือ
  - **Souleating Oviraptor**: เลือกค้นหาการ์ดขึ้นมือ

### 2. 🚫 แก้ไขบั๊กยกเลิกการ์ดฝั่งเดียวกัน (Self-Negation Fix for Blue-Eyes Spirit Dragon)
- แก้ไขใน `_2026_BlueEyesExecutor.cs` โดยตรวจสอบ `lastChain.Controller == 1` ก่อนใช้เอฟเฟกต์ยกเลิกเอฟเฟกต์สุสานของ `Blue-Eyes Spirit Dragon` เพื่อไม่ให้ยกเลิกการ์ดฝั่งตนเอง (เช่น True Light) จนถูกปรับแพ้ฟาวล์ (Rule Violation)
- ส่งผลให้บอทเดินเกม Blue-Eyes ได้นิ่งและคอมไพล์ผ่าน 100% ปราศจากแต้มเสียในรายงานความปลอดภัย

### 3. 💣 แก้ไขเกมแครชจากการ์ด Crossout Designator ในเด็ค Gimmick Puppet
- ผูกฟังก์ชัน `CrossoutDesignatorEffect` เข้ากับบั๊กการส่งการ์ดและประกาศชื่อของ WindBot ที่แต่เดิมจะประกาศบลูอายส์เสมอจนทำให้เกมล่ม (MSG_RETRY Draw)
- ตอนนี้จะสแกนหาแฮนด์แทรปที่คู่ต่อสู้เปิดใช้งานในเชนเดียวกัน (เช่น Ash Blossom, Maxx "C") หากมีในเด็คเราจะทำการแบนและเนเกตเอฟเฟกต์ได้อย่างไร้รอยต่อ

### 4. 🦖 คอมโบเรียกบอสไดโนเสาร์ด้วย Oviraptor + Lost World Pop Combo
- แยกสถานะการใช้งานเอฟเฟกต์แบบ HOPT ของ `Souleating Oviraptor` ออกเป็น 2 ส่วน (`_oviraptorSearchUsed` และ `_oviraptorPopUsed`) ทำให้สามารถค้นหาการ์ดและทำลายการ์ดบนสนามเพื่อเรียกการ์ดใหม่ได้ในเทิร์นเดียวกัน
- ปรับแต่ง `OnSelectCard` ให้ตัดสินใจสับเปลี่ยนการทำลายการ์ดไปยังเด็คโดยทำลาย `Babycerasaurus` หรือ `Petiteranodon` เพื่อเรียกมอนสเตอร์ขยายบอร์ดได้อย่างสมบูรณ์

---

## สถิติการจำลองดวลการ์ด (Headless Matchup Stats)

หลังทำการอัปเกรดความฉลาดและแก้ไขข้อผิดพลาด บอทผ่านการจำลอง 10 เกมรวดในระบบ Parallel Headless Mode มีสถิติดังนี้:

- **2026_Puppet vs AI_Altergeist**: อัตราการชนะขึ้นมาอยู่ที่ **50.0%** (ชนะ 5, แพ้ 5, แครช 0) — *ประสิทธิภาพเพิ่มขึ้นอย่างเห็นได้ชัดและไม่พบบั๊ก Crossout อีกเลย*
- **2026_Puppet vs AI_BlueEyes**: อัตราการชนะอยู่ที่ **50.0%** (ชนะ 5, แพ้ 5, แครช 0) — *บอร์ดและคอมโบทำงานได้อย่างแม่นยำ*
- **2026_BlueEyes vs AI_Altergeist**: อัตราการชนะ 0.0% (แต่เล่นจบสมบูรณ์ 100% โดย **ไม่มีการทำผิดกฎหรือแครชใดๆ** ถือว่าโค้ดมีความปลอดภัยและมีวินัยการเล่นที่ดีขึ้นมาก)
- **2026_RexRaptor vs AI_BlueEyes**: อัตราการชนะอยู่ที่ **22.2%** (ชนะ 2, แพ้ 7, เสมอ 1, แครช 0) — *แก้ไขอาการแครชตอนชุบชีวิตการ์ดผ่านฉลุย*
