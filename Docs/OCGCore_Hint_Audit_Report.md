# OCGCore Hint Message System Audit & SKILL.md Optimization Report

**Date**: 2026-09-22  
**Skill**: `.agents\skills\yugioh-executor\SKILL.md` (Version 10.1)  
**Deployment Target**: `C:\Users\admin\Documents\EdoGame\`  

---

## 1. Executive Summary

We performed a deep, line-by-line audit of the **YugiohTH Executor Development Skill** and the underlying OCGCore Hint message system:
1. **Condensed & Context-Optimized**: Redesigned `SKILL.md` to exactly **200 lines and 17.4 KB**, allowing it to be read in a **single tool call** (`view_file` supports 800 lines / 46 KB) with zero truncation and no token bloat.
2. **Audited Against Single Source of Truth**: Cross-referenced all Hint message constants directly against `script\constant.lua` (lines 805–865) and `config\strings.conf` (lines 90–155).
3. **Synchronized Core C# Classes**: Updated `ModernExecutor.cs` and `HeuristicGuard.cs` to eliminate silent constant mismatch bugs.
4. **Reaffirmed Testing Policy**: Enforced the strict prohibition against running automated Headless Simulations without explicit user orders.

---

## 2. Complete Audited OCGCore Hint Message Reference Table

| Hint ID | Constant Name | Lua & String Definition | Common Old Mistake |
|---|---|---|---|
| **500** | `HINTMSG_RELEASE` | Select the card(s) to Tribute | None |
| **501** | `HINTMSG_DISCARD` | Select the card(s) to discard | None |
| **502** | `HINTMSG_DESTROY` | Select the card(s) to destroy | None |
| **503** | `HINTMSG_REMOVE` | Select the card(s) to banish | Mistakenly recorded as 504 |
| **504** | `HINTMSG_TOGRAVE` | Select the card(s) to send to the GY | Mistakenly recorded as 508 |
| **505** | `HINTMSG_RTOHAND` | Select the card(s) to return to the hand | Conflated with ATOHAND |
| **506** | `HINTMSG_ATOHAND` | Select the card(s) to add to your hand | Mistakenly recorded as 505 |
| **507** | `HINTMSG_TODECK` | Select the card(s) to return to the Deck | Mistakenly recorded as 506 / 518 |
| **508** | `HINTMSG_SUMMON` | Select the card(s) to Normal Summon | Mistakenly thought to be TOGRAVE |
| **509** | `HINTMSG_SPSUMMON` | Select the card(s) to Special Summon | None |
| **510** | `HINTMSG_SET` | Select the card(s) to Set to the field | None |
| **511** | `HINTMSG_FMATERIAL` | Select the card(s) to use as Fusion Material | None |
| **512** | `HINTMSG_SMATERIAL` | Select the card(s) to use as Synchro Material | None |
| **513** | `HINTMSG_XMATERIAL` | Select the card(s) to use as Xyz Material | Mistakenly recorded as 519 |
| **514** | `HINTMSG_FACEUP` | Select a face-up card(s) | Mistakenly recorded as 575 |
| **515** | `HINTMSG_FACEDOWN` | Select a face-down card(s) | None |
| **516** | `HINTMSG_ATTACK` | Select a monster(s) in Attack Position | None |
| **517** | `HINTMSG_DEFENSE` | Select a monster(s) in Defense Position | None |
| **518** | `HINTMSG_EQUIP` | Select the card(s) to equip | Mistakenly recorded as 507 or POSCHANGE |
| **519** | `HINTMSG_REMOVEXYZ` | Select the Xyz Material(s) to detach | Mistakenly recorded as CONTROL |
| **520** | `HINTMSG_CONTROL` | Select the monster(s) to change control | Mistakenly recorded as 519 |
| **526** | `HINTMSG_CONFIRM` | Select the card(s) to reveal | None |
| **527** | `HINTMSG_TOFIELD` | Select the card(s) to place on the field | None |
| **528** | `HINTMSG_POSCHANGE` | Select a monster to change its battle position | Mistakenly recorded as 518 |
| **531** | `HINTMSG_TRIBUTE` | Select monsters for Tribute Summon | None |
| **533** | `HINTMSG_LMATERIAL` | Select the card(s) to use as Link Material | None |
| **551** | `HINTMSG_TARGET` | Select the target(s) of the effect | None |
| **552** | `HINTMSG_COIN` | Select heads or tails | Mistakenly labeled as DISABLE |
| **555** | `HINTMSG_OPTION` | Select an option | None |
| **571** | `HINTMSG_TOZONE` | Select the zone to move the card to | None |
| **572** | `HINTMSG_COUNTER` | Select the card(s) to place a counter on | Mistakenly labeled as NEGATE |
| **573** | `HINTMSG_TOHAND` | Add the card(s) to your hand (Option text) | None |
| **575** | `HINTMSG_NEGATE` | Select the card(s) to negate its effects | Mistakenly labeled as FACEUP or 552/572 |
| **578** | `HINTMSG_ATTACH` | Select the card(s) to attach as material | None |

---

## 3. Build & Deployment Verification
- Rebuilt using `BUILD_AND_DEPLOY.ps1` with 0 errors.
- Binaries deployed to `C:\Users\admin\Documents\EdoGame\`.
