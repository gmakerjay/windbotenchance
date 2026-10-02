# 2026_Solfachord: Complete Rule-Based ModernExecutor Refactor & Stability Report

## 1. Executive Summary
- **Deck Target**: `2026_Solfachord.ydk`
- **Executor**: `_2026_SolfachordExecutor.cs` & `ExpertSolfachordExecutor`
- **Engine**: Rule-Based C# (.NET 10.0) under `ModernExecutor`
- **Deployment Status**: Built cleanly with **0 Errors** and deployed to `C:\Users\admin\Documents\EdoGame\` via `BUILD_AND_DEPLOY.ps1`

---

## 2. Card Data Audit & Root Cause Resolutions

### 2.1 Setcode Mismatch Bug (`0x15f` vs `0x164`)
- **Issue**: The original executor checked `card.HasSetcode(0x15f)`. Real SQLite data from `cards.cdb` reveals that `SET_SOLFACHORD` is `0x164` (with `GranSolfachord` at `0x1164`). Because `0x15f != 0x164`, every single Solfachord check across the executor was failing.
- **Resolution**: Updated `SetCodeSolfachord = 0x164` and `SetCodeVaylantz = 0x17e`, restoring full functionality to `Cutia`, `Primoa`, `Solfegia`, `Harmonia`, and `Gran Coolia`.

### 2.2 Scale Accuracy Audit
- **Issue**: All Main Deck Vaylantz monsters (`Shinonome`, `Buster Baron`, `Saion`, `Viscount`, `Nazuki`, `Marquess`, `Hojo`) have **Scale 1**. The original code hallucinated random scales (e.g., Viscount 5, Hojo 4, Nazuki 3), breaking scale parity calculations.
- **Resolution**: Aligned scale helper `GetScale(int cardId)` with audited values (Solfegia 9, Cutia 8, Dreamia 7, Disablaster 5, Gracia 4, Coolia 1, Primoa 0, Main Vaylantz 1, Arktos XII 12, Grand Duke 10).

### 2.3 Beyond the Pendulum Permanent Lock Safeguard
- **Issue**: `Beyond the Pendulum` negates all monster effects and PZone effects until a Pendulum Summon occurs. The bot previously summoned Beyond the Pendulum after already Pendulum Summoning, locking its own board permanently.
- **Resolution**: Added `_hasPendulumSummoned` state tracking. Guarded `BeyondThePendulumSpSummon` so it is strictly barred if a Pendulum Summon has already been executed.

### 2.4 Vaylantz PZone SS Restriction
- **Issue**: Summoning a Vaylantz monster from PZone restricts the player from Special Summoning non-Vaylantz monsters from Hand, Deck, or GY for the rest of the turn.
- **Resolution**: Restructured combo sequencing to ensure Solfachord normal summons (`Cutia`, `Primoa`) and hand summons (`Solfegia`, `Dreamia`) are performed before initiating Vaylantz PZone bridges.

### 2.5 W:P Fancy Ball Effect Disambiguation
- **Issue**: Both Quick Negate (field/GY) and Quick Link (opponent's Main Phase) were sharing activate hooks without distinguishing effect IDs.
- **Resolution**: Disambiguated via `ActivateDescription` string IDs. Opponent-turn Quick Link prioritizes `Knightmare Gryphon` (locks non-linked Special Summoned monsters) or `S:P Little Knight` using an opponent's Link-2 or lower monster.

### 2.6 GranSolfachord Coolia Disruption Fix
- **Issue**: Gran Coolia's quick negate requires Special Summoning an odd-scale Solfachord from **PZone** to a zone she points to. The old code checked `Bot.ExtraDeck`.
- **Resolution**: Fixed check to inspect PZone scales (`Solfegia` 9, `Dreamia` 7, `Coolia` 1) and monster zone availability.

---

## 3. OCGCore Hint & Engine Callbacks Compliance

| Hint ID | Action | New Strategic Behavior |
|---|---|---|
| **500** | Tribute / Release | Protects terminal Aces and active scales; sacrifices low-value fodder or Solfegia for GY/Extra SS. |
| **501** | Discard | Discards duplicates or `PsyFrameDriver` first; never discards solitary starters or handtraps. |
| **502 / 503** | Destroy / Banish | Evaluates threats via `CardIntelligence` (immunity-aware); targets own continuous field spells for Electrumite pop. |
| **505** | Return to Hand | Rebounds recyclable scales (Cutia/Solfegia) to hand via Primoa; bounces opponent backrow via Grand Duke. |
| **506** | Add to Hand / Search | Dynamically balances missing Low Scale (0/1) vs High Scale (7/8/9). |
| **509** | Special Summon | Prioritizes on-summon search triggers (`DoSolfachord Cutia`, `Solfachord Primoa`). |
| **511 / 533** | Materials | Orders lowest value link fodder first; preserves terminal Ace bosses. |
| **575** | Negate | Targets face-up active opponent threats and floodgates. |

---

## 4. Position & Zone Architecture

1. **`OnSelectPosition`**:
   - Forces low-ATK combo pieces (`Solfachord Primoa` 0, `DoSolfachord Cutia` 100, `Shinonome` 500, `Ash Blossom` 0, `Maxx "C"` 500, `Gamma` 1000) into Defense Position.
2. **`OnSelectPlace`**:
   - Routes Extra Deck Link summons into Extra Monster Zones (bit 5 / bit 6).
   - Routes Vaylantz PZone summons into columns 1 and 3.
   - Routes Shinonome movements to adjacent open zones to trigger her monster search effect.

---

## 5. Build, Deploy & Deck Sync Verification

- **Compilation**: `dotnet build src\YGO_SOURCE_CLEAN\windbot-fork\WindBot.csproj -c Release` -> **0 Errors, 0 Warnings from executor**.
- **Deploy Pipeline**: `powershell -ExecutionPolicy Bypass -File .\BUILD_AND_DEPLOY.ps1` completed successfully.
  - `WindBot.dll`, `ExecutorBase.dll`, `core.dll`, and `bots.json` deployed to `C:\Users\admin\Documents\EdoGame\`.
  - `2026_Solfachord.ydk` synchronized to `C:\Users\admin\Documents\EdoGame\deck\2026_Solfachord.ydk`.
