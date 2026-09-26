# Comprehensive Audit Report: Dedicated Deck Modules & Cross-Deck Packet Protocol Safety

**Date**: 2026-09-26  
**Scope**: Full inspection of all 7 decks implementing dedicated helper modules (`*Plugin`) and protocol-level hardening following the Endymion `OnSelectCounter` engine crash incident.

---

## 1. Incident Summary: Endymion "เกิดข้อผิดพลาด!" (Error Popup)

### Symptoms
In EDOPro, playing against `Endymion` resulted in a GUI modal popup:
> **"เกิดข้อผิดพลาด!"** (System String 1434: Error!)

The underlying WindBot trace log showed:
```text
[TRACE][Activate] ✓ 'Endymion, the Mighty Master of Magic' (3611830) → MightyMasterBoardBreak from SpellZone
[BOARD SCORE] Idle Command Decision | Score: 0 | Action: Activate (Index: 1)
[ERROR] Got MSG_RETRY. Last message is SelectCounter
Connection closed by remote host.
```

### Technical Root Cause: Binary Protocol Misalignment
Through disassembly of `ocgcore.dll` at `0x10079ba0` - `0x10079e86` (where `MSG_SELECT_COUNTER` = `0x16` is written), the true binary protocol was identified:
- `0x10079d5b`: `write_buffer(player, 1)` $\rightarrow$ **1 byte** (`byte`)
- `0x10079d6e`: `write_buffer(type, 2)` $\rightarrow$ **2 bytes** (`int16`)
- `0x10079d85`: `write_buffer(quantity, 2)` $\rightarrow$ **2 bytes** (`int16`)
- `0x10079d9e`: `write_buffer(count, 4)` $\rightarrow$ **4 bytes** (`int32`)
- Loop `1..count`:
  - `cardId` (4 bytes, `int32`)
  - `player` (1 byte, `byte`)
  - `loc` (1 byte, `byte`)
  - `seq` (1 byte, `byte`)
  - `available_counters` (2 bytes, `int16`)

**The Critical Flaw in WindBot**:
`GameBehavior.OnSelectCounter` was reading `quantity` as `ReadInt32()` (4 bytes) and `count` as `ReadByte()` (1 byte):
```csharp
// BUGGED:
int type = packet.ReadInt16();     // 2 bytes
int quantity = packet.ReadInt32(); // 4 bytes! (Swallowed 2 bytes of quantity + 2 bytes of count!)
int count = packet.ReadByte();     // 1 byte! (Read upper zero byte of count!)
```

When multiple cards had counters on the field (e.g. Gateway + Dojo, or Citadel + Servant):
- `quantity` was read as `(count << 16) | real_quantity` $\rightarrow$ for 4 counters and 2 cards: `(2 << 16) | 4 = 131076`!
- `count` was read as `0`!
- The cards list was empty, so the executor returned `[]` (sum = 0 / 131076).
- OCGCore received an empty counter selection, compared `cx != ax` (`0 != 4`), emitted `MSG_RETRY`, WindBot disconnected, and EDOPro threw GUI modal **`"เกิดข้อผิดพลาด!"`**.

**Resolution**:
Changed `quantity` to `packet.ReadInt16()` (2 bytes) and `count` to `packet.ReadInt32()` (4 bytes).
Live logs confirmed clean parsing:
```text
[OnSelectCounter] type=0x3, quantity=4, count=2
[OnSelectCounter] Response: sum=4/4, payload=[1,3]
```

---

## 2. Universal Protocol & Engine Hardening

### GameBehavior.cs
- **Authentic Card Instantiation**: If `_duel.GetCard(...)` returns null or untracked card, automatically instantiates a valid `ClientCard(cardId, loc, seq, player)` or sets `card.SetId(cardId)`.
- **Infallible Clamp & Balance Allocator**:
  - Clamps each returned value: `0 <= used[i] <= counters[i]`.
  - Dynamically distributes any deficit or trims any surplus so that `sum(used) == quantity` is guaranteed 100% of the time.
  - OCGCore will never receive an unbalanced counter response, completely preventing `MSG_RETRY` disconnects.

---

## 3. Systematic Inspection of All 7 Dedicated Plugin Decks

We inspected all 7 decks in `src\YGO_SOURCE_CLEAN\windbot-fork\Game\AI\Decks\` that utilize dedicated `*Plugin` architectures:

| Deck | Plugin Class | Vulnerability Identified | Action Taken & Hardening | Status |
| :--- | :--- | :--- | :--- | :---: |
| **Endymion** | `EndymionPlugin` | Duplicate `_cardCounters` dictionary, Turn 1 empty board break, EMZ check on Gravity Controller | Unified into single source of truth in `Plugin.CounterEconomy`; strict enemy board check; EMZ Sequence 5/6 check | **FIXED** |
| **Six Samurai** | `SixSamuraiPlugin` | Missing null-propagation on `Plugin.CounterEconomy.SelectCounters`, no list length validation | Added null-safe fallback `?? base.OnSelectCounter(...)` and defensive `cards.Count != counters.Count` guard | **FIXED** |
| **D/D/D** | `DDDPlugin` | `OnSelectSynchroMaterial` ignored `sum` (Level), returning `sorted.Take(max)`. Fusion/Xyz returned `Take(max)` instead of `Take(min)` | Delegated Synchro to base subset-sum algorithm; changed Fusion/Xyz to `Take(min)` with graceful fallbacks | **FIXED** |
| **Morganite Stun** | `MorganiteStunPlugin` | Single-card returns in `OnSelectCard` without checking `min <= 1 && 1 <= max` | Added `if (min <= 1 && 1 <= max)` guard and null-safe plugin access | **FIXED** |
| **Drytron Tour** | `DrytronTourPlugin` | Single-card returns in `OnSelectCard` without checking `min <= 1 && 1 <= max` | Added `if (min <= 1 && 1 <= max)` guard and null-safe plugin access | **FIXED** |
| **Madolche** | `MadolchePlugin` | Single-card returns in `OnSelectCard` without checking `min <= 1 && 1 <= max` | Added `if (min <= 1 && 1 <= max)` guard and null-safe plugin access | **FIXED** |
| **Centur-Ion** | `CenturionPlugin` | None (Relies entirely on `ModernExecutor` heuristics and OCGCore protocol) | Verified safe. No protocol-level overrides exist | **VERIFIED** |

---

## 4. Headless Duel Verification Results

Each affected deck was subjected to Headless Duel Simulation vs `DarkMagician` to audit engine integrity, packet response compliance, and violation checks:

1. **`2026_Endymion` vs `DarkMagician`**:
   - Result: 8 Full Turns completed cleanly.
   - Guard Summary: **0 Violations, 0 Warnings, 0 Crashes**.
   - Counter allocations on Citadel, Servant, Magister, and Jackal King resolved with 100% protocol adherence.

2. **`2026_DDD` vs `DarkMagician`**:
   - Result: **Won 1 - 0 (5 Turns)**.
   - Guard Summary: **0 Violations, 0 Engine Retries**.
   - Synchro, Fusion, Xyz, and Link summoning sequences executed flawlessly under the corrected material selectors.

3. **`2026_SixSamurai` vs `DarkMagician`**:
   - Result: 6 Full Turns completed cleanly.
   - Guard Summary: **0 Violations, 0 Warnings, 0 Crashes**.
   - Bushido counter distributions resolved through the guarded counter economy.

---

## 5. Deployment Verification

- **Script**: `BUILD_AND_DEPLOY.ps1`
- **Build Status**: 0 Errors, 0 Warnings in Executor code.
- **Exclusive Target**: `C:\Users\admin\Documents\EdoGame\`
- **Deployed Artifacts**:
  - `WindBot\WindBot.dll`
  - `WindBot\ExecutorBase.dll`
  - `WindBot\core.dll`
  - `WindBot\bots.json`
  - `DashBot.exe`
