# 2026_Monarch ModernExecutor Refactor & Extra Deck Rip/Wipe Engine Report

**Target Deck**: `2026_Monarch.ydk`  
**Executor Class**: `_2026_MonarchExecutor.cs` (`WindBot.Game.AI.Decks._2026_MonarchExecutor`)  
**Architecture**: Rule-Based C# .NET 10.0 (ModernExecutor / ExecutorBase)  
**Deployment Path**: `C:\Users\admin\Documents\EdoGame\`  
**Date**: 2026-09-22  

---

## 1. Executive Summary

`_2026_MonarchExecutor.cs` has been completely refactored from the ground up to fix fundamental card database discrepancies, hallucinated game mechanics, and dead code pathways. The deck is not a traditional Domain lock Monarch deck, but a **Zaborg the Mega Monarch Extra Deck Rip Turbo & Jurrac Meteor Board Wipe** hybrid strategy.

Through a deep audit of the actual card scripts (`c31596518.lua`, `c67584223.lua`, `c9283801.lua`, `c63899196.lua`, `c45445571.lua`, `c52553102.lua`, `c17548456.lua`), critical bugs were resolved:
- A typo in `AshBlossomAndJoyousSpring` Card ID (`14558127` instead of `14558128`) completely prevented Ash Blossom from triggering.
- Custom/modern card scripts (`The Monarchs Revolt`, `Eidos the Underworld Monarch`, `Tessera the Primal Squire`, `The Monarchs Masterplan`) were previously implemented based on faulty assumptions, causing illegal activation attempts or dead hands.
- The Extra Deck mill strategy of `Zaborg the Mega Monarch` was restructured to leave `Jurrac Meteor` in the Extra Deck while sending `Jurrac Astero` and dinosaur fodder to the GY, enabling an on-demand Quick-Effect board wipe during the opponent's turn.

---

## 2. Card-by-Card Real Lua Audit & Mechanics

| Card Name | ID | Real Lua Effect & Cost | Resolved Behavior in Executor |
|---|---|---|---|
| **Ash Blossom & Joyous Spring** | `14558128` | Handtrap negate search/dump/SS from Deck | Fixed ID typo from `14558127`. Handtrap now triggers reliably. |
| **Zaborg the Mega Monarch** | `87602890` | On Tribute Summon: Destroy 1 monster; if LIGHT, both players mill Extra Deck up to its Level/Rank (8 cards). If LIGHT tributed, bot picks opponent's cards. | On Turn 1 (or no opp monster), targets itself to guarantee 8-card mill from both Extra Decks. Precision mill order ensures bot gains card advantage while crippling opponent. |
| **Jurrac Astero** | `52553102` | During opponent's turn: Banish self + 1 Jurrac from GY -> Special Summon `Jurrac Meteor` from Extra Deck. | Added GY Quick Effect during opponent's turn to drop `Jurrac Meteor`, triggering a total board wipe when opponent commits resources. |
| **Jurrac Meteor** | `17548456` | Mandatory Trigger: On Special Summon, destroy all cards on the field! | Preserved in Extra Deck (never milled by Zaborg) to enable Astero's summon. |
| **The Duke of Demise** | `45445571` | GY ignition: Banish self from GY, target 1 Level 4+ Fiend/Zombie in GY -> add to hand. | Added GY recovery executor to retrieve `Erebus the Underworld Monarch` or `Caius the Shadow Monarch`. |
| **The Monarchs Revolt** | `9283801` | Activation: Discard 1, reveal 3 monsters (800 or 2400+ ATK, 1000 DEF), opp picks 1 to hand, rest to GY. GY: Banish self -> SS 800/1000 Squire from hand. | Corrected field activation cost and reveal selection (`Erebus`, `Edea`, `Eidos`). Added GY banish Special Summon. |
| **Eidos the Underworld Monarch** | `31596518` | On Summon: Search 1 Monarch S/T or 2800/1000 Monarch from Deck/GY. In GY: When a Monarch is Tribute Summoned, Special Summons itself to field. | Replaced hallucinated opponent's turn tribute summon with proper summon tutor and GY self-resurrection trigger. |
| **Tessera the Primal Squire** | `67584223` | Hand: Reveal 1 Monarch S/T -> SS self. Field: Tribute Summon 1 monster. GY: When sent to GY -> SS 1 800/1000 Squire from Deck. | Implemented hand reveal SS, field tribute summon ignition, and GY summon trigger (`Edea` or `Eidos`). |
| **The Monarchs Masterplan** | `63899196` | Activation: Send 1 Monarch S/T from Deck to GY. Banish: Target 1 opp monster -> search matching-Attribute 2400/1000 Monarch and immediately Normal Summon it. | Implemented deck dump (`Pantheism` or `The Prime Monarch`) and banish trigger when banished by `The Prime Monarch`. |
| **The Prime Monarch** | `54241725` | Field: Target 2 Monarch S/Ts in GY, shuffle to draw 1. GY: Banish 1 Monarch S/T from GY -> SS as 1000/2400 Normal Monster. | Prioritizes banishing `The Monarchs Masterplan` from GY to trigger Masterplan's banish search. Guards shuffle to preserve `Pantheism`. |

---

## 3. Zaborg Extra Deck Turbo & Board Wipe Strategy

```
                          [Turn 1 Starter]
                     Edea / Tessera / Eidos
                                |
                                v
                   Tribute Summon Mega Zaborg
                                |
             +------------------+------------------+
             |                                     |
             v                                     v
   Target Opponent Monster               Turn 1: Target Itself
 (if going 2nd or established)              (Level 8 LIGHT)
             |                                     |
             +------------------+------------------+
                                |
                                v
               Both Players Mill 8 Extra Deck Cards
                                |
             +------------------+------------------+
             |                                     |
             v                                     v
       Opponent Mills 8                    Bot Mills 8:
   Key Pieces Ripped to GY:             1. Garura (Draw 1)
   - Link-1 combo engines              2. Elder Entity N'tss (Pop)
   - Key Fusion/Synchro bosses         3. The Duke of Demise (Retrieve Erebus)
   - Extra Deck extension staples      4. PSY-Framelord Omega (Recycle)
                                       5. Jurrac Astero (GY Setup)
                                       6. Jurrac Velphito (Dino Fodder)
                                       7. Lunalight Perfume Dancer (Debuff)
                                       8. Millennium-Eyes / 2nd Astero
                                       (Meteor PRESERVED in Extra Deck!)
                                                   |
                                                   v
                                        Opponent's Turn Setup:
                                     Jurrac Astero GY Quick Effect
                                                   |
                                                   v
                                      SS Jurrac Meteor from Extra
                                                   |
                                                   v
                                      Mandatory Trigger: BOARD WIPE!
```

---

## 4. OCGCore Hint Message System Calibration

All callback logic in `OnSelectCard` has been strictly aligned with `script\constant.lua` and `config\strings.conf`:

| Hint ID | Constant Name | Executor Action & Safeguards |
|---|---|---|
| **500 / 531** | `HINTMSG_RELEASE` / `HINTMSG_TRIBUTE` | Tributes opponent's monsters via `The Monarchs Stormforth` first, then low-priority squires/tokens (`Eidos` > `Edea` > `Tessera` > `The Prime Monarch`). **Never** tributes Ace monsters. |
| **501** | `HINTMSG_DISCARD` | Discard prioritization: `The Prime Monarch` > `The Monarchs Masterplan` > `Pantheism` > `The Monarchs Revolt` > `Erebus`. |
| **502** | `HINTMSG_DESTROY` | Targets opponent's highest threat; or Zaborg itself on Turn 1 if no opponent monsters exist. |
| **503** | `HINTMSG_REMOVE` | Banish targets: Caius banishes opponent's most threatening card; Prime Monarch banishes `Masterplan` to trigger search. |
| **504** | `HINTMSG_TOGRAVE` | Erebus & Masterplan deck dumps: `The Prime Monarch` + `Pantheism of the Monarchs`. |
| **506** | `HINTMSG_ATOHAND` | Dynamic search prioritizer for missing Monarch starters and spells. |
| **507** | `HINTMSG_TODECK` | Erebus non-targeting spin targeting opponent's boss monster; Omega recycling Pantheism. |
| **509** | `HINTMSG_SPSUMMON` | Squire tutors (`Edea` &rarr; `Eidos`), Prime Monarch monster summon, and Jurrac Meteor. |

---

## 5. Verification & Deployment Status

- **Compilation**: `dotnet build src\YGO_SOURCE_CLEAN\windbot-fork\WindBot.csproj -c Release` &rarr; **0 Errors / 0 Warnings** in `_2026_MonarchExecutor.cs`.
- **Pipeline Deployment**: Successfully executed `src\YGO_SOURCE_CLEAN\BUILD_AND_DEPLOY.ps1`:
  - `WindBot.dll`, `ExecutorBase.dll`, `core.dll`, `bots.json`, and support assets deployed to `C:\Users\admin\Documents\EdoGame\`.
  - `DashBot.exe` launcher built and deployed.
- **Deck Synchronization**: `2026_Monarch.ydk` synchronized to `C:\Users\admin\Documents\EdoGame\deck\`.
- **Testing Readiness**: Bot is registered in `bots.json` under name `"2026_Monarch"` and ready for user testing via DashBot or Headless duel simulator.
