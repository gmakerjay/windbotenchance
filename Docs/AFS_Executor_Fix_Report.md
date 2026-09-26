# AFS (Azamina Fiendsmith Snake-Eye) Executor Fix Report

**Date**: 2026-09-22  
**Target Deck**: `AFS.ydk` / `2026_AFS.ydk`  
**Executor**: `AFSExecutor.cs` (Rule-Based C# .NET 10)  
**Deployment**: `C:\Users\admin\Documents\EdoGame\`  

---

## 1. Problem Statement
The bot deck AFS (Azamina Fiendsmith Snake-Eye) was reported to stall or refuse playing further cards after playing the first card (e.g. Normal Summoning Snake-Eye Ash or activating a starter spell).

---

## 2. Root Cause Analysis & Detailed Fixes

### 1) OCGCore Hint Message Mismatch
- **Issue**: `AFSExecutor.cs` had misaligned Hint IDs compared to OCGCore / EDOPro `strings.conf` standard:
  - `HINTMSG_TOGRAVE` was 508 instead of 504.
  - `HINTMSG_REMOVE` was 504 instead of 503.
  - `HINTMSG_ATOHAND` was 505 instead of 506.
- **Fix**: Realigned Hint constants to exact values:
  - `HINTMSG_TOGRAVE = 504`
  - `HINTMSG_REMOVE = 503`
  - `HINTMSG_ATOHAND = 506`
  - Added `HINTMSG_LMATERIAL = 533`, `HINTMSG_XMATERIAL = 513`, `HINTMSG_SPSUMMON = 509`

### 2) Linkuriboh Priority vs. Snake-Eye Ash Field Send
- **Issue**: Ash ignition send effect was executing *before* Linkuriboh was summoned. When both Ash and Poplar were on field, Ash would send both directly to GY, skipping Poplar's GY trigger opportunity to place itself into the S/T zone as continuous fodder.
- **Fix**: Reordered combo priority so that when Poplar is on field, the bot immediately Link Summons Linkuriboh (using Poplar). Poplar then triggers in GY to place itself into the S/T Zone. Ash's field-send effect then only needs to consume Ash + Poplar (from S/T Zone), leaving field space and maximizing card economy.

### 3) Snake-Eyes Flamberge Dragon Ace Lock
- **Issue**: `SnakeEyesFlambergeDragon` was marked as an Ace card. In WindBot / ModernExecutor, monsters identified as Ace cards are protected from being used as Link Material. When Flamberge was summoned, `Moon of the Closed Heaven` (Link-2) could not use Flamberge as material, causing the combo to deadlock.
- **Fix**: Removed Flamberge from `IsAceCard` during active combo turns so that it can be freely used as Link material to summon `Moon of the Closed Heaven`. This immediately triggers Flamberge's GY effect to resurrect 2 Level 1 FIRE monsters from the GY.

### 4) Deception of the Sinful Spoils Face-Down Lock
- **Issue**: When `Deception` was set on field by an effect, activating/flipping it face-up was setting `_deceptionUsed = true`, preventing its second effect (ignition send to search Azamina cards) from being activated during that turn.
- **Fix**: Separated the face-down flip from the search ignition effect so that `Deception` can be flipped face-up and then immediately send a card from hand/field to search `The Hallowed Azamina`.

### 5) The Hallowed Azamina Fusion Level Calculation
- **Issue**: The bot was expecting Level 8 (`Azamina Mu Rcielago`) and requiring 2 Sinful Spoils cards, which often caused the bot to hold `The Hallowed Azamina` indefinitely if only 1 Sinful Spoils card was available.
- **Fix**: Adjusted the calculation so that sending 1 Sinful Spoils card summons `Azamina Ilia Silvia` (Level 6, 2400 ATK, Omni-Negate).

### 6) Promethean Princess FIRE Lock Guard
- **Issue**: `Promethean Princess, Bestower of Flames` locks the player into Special Summoning only FIRE monsters while face-up. If summoned prematurely, it permanently locked out DARK and LIGHT pieces:
  - `D/D/D Wave High King Caesar` (DARK Fiend Xyz)
  - `Azamina Ilia Silvia` (DARK Illusion Fusion)
  - `Fiendsmith's Sequence` (LIGHT Fiend Link)
- **Fix**: Guarded `PrincessSummon` to strictly execute *after* Caesar, Silvia, and Fiendsmith pieces have been successfully summoned, or when no further non-FIRE lines can be executed.

### 7) Fiendsmith's Lacrima Option 1 (Special Summon)
- **Issue**: Lacrima triggers `aux.ToHandOrElse` upon summon. The default choice was 0 (Add to Hand), which prevented getting two Level 6 Fiends on field for Caesar.
- **Fix**: Handled via `OnSelectOption` returning option 1 (Special Summon) for Lacrima.

### 8) Emergency Hand Starters Added
- Added fallback normal summon for `The Fabled Lurrie` into `Moon of the Closed Heaven` or `Fiendsmith's Requiem`.
- Added ignition for `Snake-Eyes Diabellstar` when placed in the Spell & Trap zone.

---

## 3. Build & Deployment Status
- **Build**: Successfully compiled using .NET 10 SDK via `BUILD_AND_DEPLOY.ps1` with 0 errors.
- **Deployment Target**:
  - `C:\Users\admin\Documents\EdoGame\WindBot\WindBot.dll`
  - `C:\Users\admin\Documents\EdoGame\WindBot\ExecutorBase.dll`
  - `C:\Users\admin\Documents\EdoGame\WindBot\core.dll`
  - `C:\Users\admin\Documents\EdoGame\WindBot\bots.json`
  - `C:\Users\admin\Documents\EdoGame\deck\AFS.ydk` & `2026_AFS.ydk`
  - `C:\Users\admin\Documents\EdoGame\WindBot\Decks\AFS.ydk` & `2026_AFS.ydk`
