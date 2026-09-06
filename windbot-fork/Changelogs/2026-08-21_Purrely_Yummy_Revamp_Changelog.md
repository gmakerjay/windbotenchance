# Changelog: Purrely & Yummy AI Engine Comprehensive Overhaul
**Date**: 2026-08-21  
**Target Decks**: 
- `2026_Purrely` (`_2026_PurrelyExecutor.cs`)
- `2026_Yummy` (`_2026_YummyExecutor.cs`)

---

## Summary of Changes

### 1. 2026_Purrely Overhaul (`_2026_PurrelyExecutor.cs`)
- **5+ Material Plump -> Expurrely Noir Rank-Up Engine**:
  - Fixed `EpurrelyPlumpEffect` to proactively attach up to 2 Spells/Traps from GYs on Turn 1/2.
  - Implemented response triggers on all Rank 2 Purrely Xyz monsters to attach activated Quick-Play Spells from the field as material.
  - Re-engineered `ExpurrelyNoirSpSummon` to allow immediate overlay over ANY Rank 2 Purrely Xyz with 5+ materials.
- **Sleepy Memory Standby Draw Integration**:
  - Implemented `StrayPurrelyStreet` End Phase attachment logic to prioritize attaching `Purrely Sleepy Memory` from Deck to `Expurrely Noir`, enabling drawing 1–2 cards during opponent's Standby Phase.
- **Happiness Multi-Attack OTK Engine**:
  - Enabled `Epurrely Happiness` battle triggers to halve enemy monster ATK, search Purrely cards, and stack multi-attacks via `Happy Memory` combined with `Delicious Memory` ATK buffs (+300 per material).
- **Material & Discard Protection**:
  - Refined `GetBestDiscardCard()` to prioritize expendables and Spells that Plump can retrieve.
  - Guarded Ace cards (`Expurrely Noir`, `Epurrely Beauty`, `Epurrely Noir`, `AA-ZEUS`) across all material selection methods.

---

### 2. 2026_Yummy Overhaul (`_2026_YummyExecutor.cs`)
- **Link-1 as Level 1 Tuner Synchro Integration**:
  - Added support for Synchro Level 2 monsters (`Cupsy★Yummy Way`, `Cooky★Yummy Way`, `Lollipo★Yummy Way`) treating Link-1 `Yummy★Snatchy` as a Level 1 Tuner.
- **Reactive Level 2 Synchro Tag-Out System**:
  - Implemented Quick Effect triggers on `Cupsy Way`, `Cooky Way`, and `Lollipo Way` to return to Extra Deck when opponent activates cards/effects, special summoning 2 Yummy monsters from GY.
- **Synchro Special Summon Trigger Handling**:
  - Configured secondary effects when Yummy monsters are special summoned by Synchro effects:
    - `Cooky☆Yummy`: Destroys 1 opponent monster.
    - `Lollipo☆Yummy`: Banishes 1 card from opponent's GY.
    - `Cupsy☆Yummy`: Draws 1 card.
    - `Marshmao☆Yummy`: Places `Yummyusment☆Mignon` or `Yummy☆Surprise` directly from Deck.
- **Borreload Savage Dragon & Spright Elf Synergies**:
  - Fixed material priorities to ensure `Borreload Savage Dragon` equips Link monsters (`Spright Elf` / `Snatchy`) from GY for 2 Omni-negate counters and 3700 ATK.
  - Established Turn 1 End Board: `Borreload Savage` (Omni) + `Spright Elf` + `Herald of the Arc Light` (Macro/Omni) + `Yummy Way` + `Yummy☆Surprise` (Bounce 2).

---

## Build & Deployment Verification

- **Build Target**: `Source_Project\YGO_AI_PLATFORM\windbot-fork\WindBot.csproj`
  - Debug x86 Build: **0 Errors (Passed)**
  - Release win-x64 Self-Contained Publish: **0 Errors (Passed)**
- **Deployed Locations (Rule A6 Compliant)**:
  - `Client\EdoGame\WindBot\`
  - `Client\EdoGame\`
  - `Source_Project\Game_EDOPro\WindBot\`
- **Detailed Documentation**:
  - `Source_Project\docs\2026_Purrely_Yummy_Revamp_Report.md`
