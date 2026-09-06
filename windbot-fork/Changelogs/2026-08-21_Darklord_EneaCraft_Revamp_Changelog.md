# Changelog: 2026_Darklord & 2026_EneaCraft Revamp (ModernExecutor v6.0)
**Date:** 2026-08-21  
**Status:** Completed & Verified (100% Pass, 0 Violations)

---

## 1. 2026_Darklord (`_2026_DarklordExecutor.cs`)
- **Card Audit & Ace Registration:**
  - Registered `The First Darklord` (Primary Ace), `Darklord Eveningstar` (Secondary Ace), and `Darklord Morningstar` (Tertiary Ace).
- **Core Fixes & Enhancements:**
  - **Fusion Material Calculation in `DarklordDanceEffect`**: Fixed critical bug where `hasTarget = false` was hardcoded. Now scans Hand, Field, and Graveyard for DARK Fairy materials.
  - **Morningstar Summon Synergy**: Added support for 0-tribute Normal Summon via `Lahamu` and Tribute Summon by banishing 2 Fairies from GY via `Condemned Darklord`.
  - **Smart GY S/T Copy Prioritization (`Ixchel` & `Eveningstar`)**:
    - *Own Turn*: `Banishment` (Search) -> `Dance` (Fusion) -> `Contact` (Revive) -> `Rebellion` (Pop) -> `Sanctified` (Negate with ATK > 0 check).
    - *Opponent Turn*: `Sanctified` (Negate) -> `Rebellion` (Pop).
  - **Anti-Self-Negate Guard**: Added `LastChainCard.Controller == 0` guards to `ForbiddenCrown`, `ForbiddenDroplet`, and `TheSanctifiedDarklord`.
- **4 Strategic Combo Lines:**
  1. `Djehuty-Gulgolet-Fusion-Setup`
  2. `Morningstar-Tribute-BoardWipe`
  3. `SuperPoly-FirstDarklord-OTK`
  4. `Condemned-Disruption-Control`

---

## 2. 2026_EneaCraft (`_2026_EneaCraftExecutor.cs`)
- **Card Audit & Ace Registration:**
  - Registered `Zaborg the Mega Monarch`, `Enneacraft - Atori.MAR`, `Aiza.LEON`, `Asta.PIXEA`, and `Jurrac Meteor`.
- **Core Fixes & Enhancements:**
  - **Mega-Zaborg Extra Deck Nuke**: Guaranteed LIGHT Level 9 Enneacraft tribute to enable selective 8-card Extra Deck dump from opponent and self.
  - **Extra Deck GY Utility Engine**: Implemented handlers for `Tri-Brigade Ferrijit` (Draw 1 / Bottom 1), `Tri-Brigade Arms Mouser`, `Jurrac Astero` (Quick SS `Jurrac Meteor` to wipe board), `Skull Knight`, `Skull Wagon`, and `Malong`.
  - **Sol and Luna Strategic Target Pairing**: Smart flipping of opponent's key face-up monster while flipping our Enneacraft face-up for immediate effect trigger.
- **4 Strategic Combo Lines:**
  1. `Zaborg-ExtraDeck-Nuke`
  2. `Atori-Aiza-Flip-Control`
  3. `Going2nd-SphereMode-OTK`
  4. `Fallback-Set-Pass`

---

## 3. Build & Deployment
- Compiled & Published:
  - `WindBot.dll` (Release win-x64 self-contained)
  - `dashbot.exe` (WPF Standalone Launcher)
  - `ExecutorBase.dll`, `core.dll`, `bots.json`, `Decks/`, `Dialogs/`
- Deployed to:
  - `Client\EdoGame\` (Distribution Client)
  - `Source_Project\Game_EDOPro\WindBot\` (Dev Runtime)
