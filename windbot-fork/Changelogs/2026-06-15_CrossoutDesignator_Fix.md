# Changelog — Crossout Designator Crash Fix
**Date:** 2026-06-15  
**Version:** Hotfix — Crossout Designator  
**Scope:** Deck Executors ทุกตัวที่ใช้ Crossout Designator (65681983)

---

## สรุปการเปลี่ยนแปลง

แก้บั๊ก **Crossout Designator ทำให้เซิร์ฟเวอร์ EDOPro crash** — บอทประกาศชื่อการ์ดที่ไม่มีอยู่ในเด็ค ทำให้เกมไม่สามารถ resolve ได้และตัดการเชื่อมต่อทันที

### สาเหตุ (Root Cause)

Crossout Designator ต้อง "ประกาศชื่อการ์ด" ที่ตรงกับการ์ดที่ฝ่ายตรงข้ามเปิดใช้ **และ banish สำเนาจากเด็คของเรา** ถ้าเราไม่มีการ์ดนั้นในเด็ค → เซิร์ฟเวอร์พัง

### ปัญหาที่พบ

| # | Executor | ระดับ | ปัญหา | สาเหตุ |
|---|----------|-------|-------|--------|
| 1 | 🔴 SkyStriker | CRASH | Crossout ใช้ได้ทุกการ์ด → server crash ทุกครั้ง | `Bot.GetRemainingCount(id, 3)` hardcode initialCount=3 → always > 0 |
| 2 | 🟡 Luna | DEAD CODE | Crossout ไม่เคยทำงาน | `Bot.Deck.Any(c => c.Id == x)` ใช้ไม่ได้ — face-down deck cards มี Id=0 |
| 3 | 🟠 Magnet | PARTIAL | ทำงานเฉพาะ 7 hand traps ที่ hardcode ไว้ | `GetMainDeckCount()` lookup table ไม่ครอบคลุม |
| 4 | 🟠 Labrynth | PARTIAL | ทำงานเฉพาะ 4 การ์ด (Ash/Droll/Mulcharmy/Imperm) | hardcode `int[] targets` แค่ 4 ตัว |
| 5 | ✅ EvilTwin | OK | ใช้ `GetRemainingCount(code)` ถูกต้อง | — |
| 6 | ✅ RyuGe | OK | ใช้ `StartingDeck.Cards` + `GetRemainingCount()` ถูกต้อง | — |

---

## ไฟล์ที่เปลี่ยนแปลง

### `_2026_SkyStrikerExecutor.cs` — L587-599

```diff
 private bool CrossoutDesignatorEffect()
 {
     if (LastChainCard == null || LastChainCard.Controller == 0) return false;

     int opponentCardId = LastChainCard.Id;
-    if (Bot.Hand.Any(c => c != null && c.Id == opponentCardId) || Bot.GetRemainingCount(opponentCardId, 3) > 0)
-    {
-        AI.SelectCard(opponentCardId);
+    // Use StartingDeck-based count to verify we actually have this card in our deck
+    if (GetRemainingCount(opponentCardId) > 0)
+    {
+        AI.SelectAnnounceID(opponentCardId);
         return true;
     }
     return false;
 }
```

---

### `_2026_LunaExecutor.cs` — L258-268

```diff
 private bool CrossoutDesignatorEffect()
 {
     if (LastChainCard == null || LastChainCard.Controller != 1) return false;
-    if (Bot.Deck.Any(c => c != null && c.Id == LastChainCard.Id))
-    {
-        AI.SelectCard(LastChainCard.Id);
+    // Use StartingDeck-based count — Bot.Deck cards are face-down with Id=0
+    if (GetRemainingCount(LastChainCard.Id) > 0)
+    {
+        AI.SelectAnnounceID(LastChainCard.Id);
         return true;
     }
     return false;
 }
```

---

### `_2026_MagnetExecutor.cs` — L393-412

```diff
 private bool CrossoutEffect()
 {
     ...
     if (lastCard != null)
     {
-        int mainCount = GetMainDeckCount(lastCard.Id);
-        if (mainCount > 0 && Bot.GetRemainingCount(lastCard.Id, mainCount) > 0)
+        // Use StartingDeck-based count to verify we have a copy in deck
+        if (GetRemainingCount(lastCard.Id) > 0)
         {
             _crossoutUsed = true;
             AI.SelectAnnounceID(lastCard.Id);
             return true;
         }
     }
     ...
 }
```

---

### `_2026_LabrynthExecutor.cs` — L459-469

```diff
 private bool CrossoutEffect()
 {
     if (LastChainCard == null || LastChainCard.Controller != 1) return false;
-    int[] targets = { CardId.AshBlossom, CardId.DrollAndLockBird, CardId.MulcharmyFuwalos, CardId.InfiniteImpermanence };
-    foreach (int targetId in targets)
-    {
-        if (LastChainCard.IsCode(targetId))
-        {
-            AI.SelectAnnounceID(targetId);
-            return true;
-        }
+    // Use StartingDeck-based count to verify we have a copy in deck
+    if (GetRemainingCount(LastChainCard.Id) > 0)
+    {
+        AI.SelectAnnounceID(LastChainCard.Id);
+        return true;
     }
     return false;
 }
```

---

## การแก้ไขหลัก 2 จุด

### 1. `AI.SelectCard()` → `AI.SelectAnnounceID()`
Crossout Designator ต้อง **ประกาศชื่อการ์ด** (announce) ไม่ใช่ **เลือกการ์ดจากเด็ค** (select)
- `SelectCard()` = เลือกการ์ดจริงจาก list ที่เซิร์ฟเวอร์ส่งมา
- `SelectAnnounceID()` = ประกาศ card ID ที่ต้องการ declare

### 2. `Bot.GetRemainingCount(id, 3)` → `GetRemainingCount(id)` (Executor-level)
- **เก่า:** `Bot.GetRemainingCount(id, initialCount)` ต้องระบุ initialCount เอง — ถ้าระบุผิด (เช่น 3 สำหรับการ์ดที่ไม่มีในเด็ค) จะ return > 0 เสมอ
- **ใหม่:** `GetRemainingCount(id)` ใน Executor.cs ดึง initialCount จาก `StartingDeck.Cards.Count(x => x == cardId)` → ถ้าไม่มีในเด็ค = return 0

---

## Evidence (Log Analysis)

จาก `client_2026_SkyStriker_20260615_160312_7911.log`:

```
[16:03:23] [TRACE][Activate] ? Crossout Designator (65681983) → CrossoutDesignatorEffect from Hand
[16:03:23] Turn 1            ← เกมจบทันที ไม่มี "Duel finished" = server crash

[16:03:35] [TRACE][Activate] ? Crossout Designator (65681983) → CrossoutDesignatorEffect from Hand
[16:03:35] Turn 1            ← crash อีกครั้ง

[16:03:59] [TRACE][Activate] ? Crossout Designator (65681983) → CrossoutDesignatorEffect from Hand
[16:04:00] Turn 1            ← crash ครั้งที่ 3
```

Pattern: **ทุกครั้งที่ SkyStriker ใช้ Crossout → เกมจบทันทีโดยไม่มี result**

---

## Build Status

- ✅ `WindBot.csproj` — **Build succeeded** (0 new errors, 0 new warnings)

---

## Backward Compatibility

- ✅ ทุก executor ที่ไม่มี Crossout — **ไม่มีผลกระทบ**
- ✅ EvilTwin + RyuGe — **ไม่ต้องแก้** (ใช้ pattern ที่ถูกอยู่แล้ว)
- ✅ `GetRemainingCount(id)` เป็น method ที่มีอยู่ใน base `Executor.cs` — ใช้ได้ทุก executor
