# 2026_Yummy Archetype Decoupled Domain Plugin & ModernExecutor Rework Report

**Date**: 2026-09-30  
**Target Deck**: `2026_Yummy`  
**Architecture**: ModernExecutor + Decoupled Domain Plugin (`YummyPlugin.cs`)  
**Deployment Target**: `C:\Users\admin\Documents\EdoGame\`  

---

## 1. Executive Summary

`2026_Yummy` has been completely refactored and reworked from a monolithic legacy executor into the modern project architecture:
1. **Decoupled Domain Plugin Architecture (`YummyPlugin.cs`)**:
   - `YummyStrategy` implementing `IDeckStrategy`: Handles intelligent Special Summon target picking (prioritizing Cooky pop, Marshmao S/T placement, Lollipo GY banish, Cupsy draw during tag-out) and Search target resolution (dual search vs single search).
   - `YummyMaterialEvaluator` implementing `IDeckMaterialEvaluator`: Evaluates material costs (protecting Ace cards `Spright Elf`, `Herald of the Arc Light`, `S:P Little Knight`, `AA-ZEUS`, while enabling Level 2 Synchros to be safely used for `Spright Elf`), handles discard selection for `Cupsy Way`, and material sorting.
   - `YummyThreatEvaluator` implementing `IDeckThreatEvaluator`: Evaluates opponent floodgates and high-threat bosses.
2. **ModernExecutor Standard (`YummyExecutor.cs`)**:
   - Standard 9-Tier `AddExecutor` pipeline.
   - 100% verified Card IDs against `cards.cdb`.
   - Strict adherence to project safety rules:
     - `OnSelectEffectYn`: Automatically rejects opponent hostile prompts (`card.Controller == 1 => false`).
     - `OnSelectOption`: Automatically picks `Destroy it` for Cooky, `Banish it` for Lollipo, and double bounce for `Yummy☆Surprise`.
     - `OnSelectCard`: Separates hint IDs (`506` Search $\rightarrow$ Strategy, `501` Discard $\rightarrow$ MaterialEvaluator, `509` Special Summon $\rightarrow$ Strategy, `502` Destroy $\rightarrow$ Opponent cards only, `505` Bounce $\rightarrow$ 2 our beasts + 2 opponent cards, `527`/`510` Field placement $\rightarrow$ Mignon/Acroquey, `528`/`561` Book of Moon $\rightarrow$ Opponent face-up monsters only).
3. **Deck Optimization (`2026_Yummy.ydk`)**:
   - Streamlined to 42 cards Main Deck (3x Cupsy, 3x Marshmao, 3x Cooky, 3x Lollipo, 3x Piri Reis Map, 1x Mignon, 1x Acroquey, 2x Surprise, handtraps, and staples).
   - Purged dead / banned Extra Deck cards (`Crystron Halqifibrax`, `Borreload Savage Dragon`, `Martial Metal Marcher`, `Cupid Pitch`, `KewlTune RS`).
   - Integrated premier Extra Deck pieces:
     - `Yummy★Snatchy` (Link-1 starter & quick synchro engine)
     - `Cupsy★Yummy Way` & `Cooky★Yummy Way` & `Lollipo★Yummy Way` (Level 2 Synchro core)
     - `Spright Elf` (Target protection & quick revival of Level 2 Synchros on opponent's turn)
     - `S:P Little Knight` (Banish on summon + quick self/opponent banish)
     - `Herald of the Arc Light` (Omni-negate tribute)
     - `Lyrilusc - Assembled Nightingale` (Direct attack + team destruction/damage immunity)
     - `Divine Arsenal AA-ZEUS - Sky Thunder` (Quick field wipe via Nightingale overlay)
     - `Relinquished Anima`, `Salamangreat Almiraj`, `Link Spider`, `Linkuriboh`, `Sky Striker Ace Kagari`
4. **Build & Exclusive Target Deployment**:
   - Compiled with 0 errors via `BUILD_AND_DEPLOY.ps1`.
   - Binaries and deck lists deployed to `C:\Users\admin\Documents\EdoGame\`.

---

## 2. Card Audit & Archetype Mechanics

| Card Name | Card ID | Type | Role | Key Effect / AI Handling |
|---|:---:|:---:|:---:|---|
| **Cupsy☆Yummy** | 31425736 | Lv1 LIGHT Beast | Primary Starter | Free SS if control Link-1 or Lv2 Synchro. On NS/SS: Searches any Yummy card from Deck. If SS by Synchro effect: Draws 1 card. |
| **Marshmao☆Yummy** | 10966439 | Lv1 LIGHT Beast | Starter / Extender | Free SS if control no monsters or all LIGHT Beasts. On NS/SS: Adds Yummy S/T from GY to hand, or places `Acroquey`/`Mignon` from Deck face-up if SS by Synchro effect. |
| **Cooky☆Yummy** | 68810435 | Lv1 LIGHT Beast | Extender / Removal | Free SS if control Link-1 or Lv2 Synchro. On NS/SS: -1000 ATK, or destroys 1 opponent monster if SS by Synchro effect. |
| **Lollipo☆Yummy** | 4215180 | Lv1 LIGHT Beast | Extender / GY Control | Free SS if control Link-1 or Lv2 Synchro. On NS/SS: Shuffles 1 opp GY card, or banishes 1 opp GY card if SS by Synchro effect. |
| **Yummy★Snatchy** | 30581601 | Link-1 (1 LIGHT Beast) | Combo Bridge | On SS: Places `Yummyusment☆Mignon` (or `Acroquey`) from Deck. Quick Effect (100 LP): Synchro Summon using field monsters. |
| **Yummyusment☆Mignon** | 66975205 | Field Spell | Resource Engine | +500 ATK per LIGHT Beast on field. Ignition: Revives Lv1 Yummy from GY if Link-1 on field. GY: Recycles 2 Yummies. |
| **Yummyusment★Acroquey** | 93360904 | Field Spell | Disruption / Float | Trigger: Destroys 1 opp card whenever LIGHT Beast Synchro is SS. If monster leaves field by opp effect: SS Yummy from Deck. |
| **Cupsy★Yummy Way** | 31603289 | Lv2 Synchro | Advantage Engine | Treats Link-1 as Lv1 Tuner. On SS: Searches 2 Yummy monsters and discards 1. Quick Tag-out (opp effect): Returns to ED, revives 2 Yummies. |
| **Cooky★Yummy Way** | 67098897 | Lv2 Synchro | Board Disruption | Treats Link-1 as Lv1 Tuner. On SS: Changes up to 2 monsters to face-down defense. Quick Tag-out: Returns to ED, revives 2 Yummies. |
| **Lollipo★Yummy Way** | 93192592 | Lv2 Synchro | GY Recovery | Treats Link-1 as Lv1 Tuner. On SS: Revives 2 Yummies (effects negated). Quick Tag-out: Returns to ED, revives 2 Yummies. |
| **Yummy☆Surprise** | 29369059 | Normal Trap | Mass Disruption | Option 1: Bounces 2 of our LIGHT Beasts + 2 opponent cards. Option 2: Special Summons 1 Yummy from GY. |
| **Spright Elf** | 27381364 | Link-2 | Disruption Amplifier | Target protects linked monsters. Quick Effect: Revives `Cooky★Yummy Way` on opponent turn to trigger Book of Moon x2 + Acroquey pop! |

---

## 3. Decoupled Domain Plugin Architecture (`YummyPlugin.cs`)

```
CENTRAL CORE (CardIntelligence, BoardScorer, FallbackSelectCard, HeuristicGuard)
        ↓ inherits
_2026_YummyExecutor (9-Tier router, Activation conditions, Hint dispatcher)
        ↓ delegates
YummyPlugin (DeckPluginBase)
    ├── YummyStrategy : IDeckStrategy
    │   ├── PickSpecialSummonTarget (Opponent Turn: Cooky pop / Marshmao Acroquey / Lollipo banish; Our Turn: Cupsy Way / Cooky Way)
    │   └── PickSearchTarget (Dual: Cooky + Lollipo; Single: Surprise > Cooky > Lollipo > Marshmao > Mignon)
    ├── YummyMaterialEvaluator : IDeckMaterialEvaluator
    │   ├── GetMaterialCost (Ace protection 10000; Level 2 Synchro material gate 250 for Elf / 8000 on end board)
    │   ├── SortMaterials (Lowest cost fodder first)
    │   └── PickDiscardTarget (Illusion > Piri Reis > Jester Confit > duplicates > S/T)
    └── YummyThreatEvaluator : IDeckThreatEvaluator
        ├── EvaluateThreatScore (Floodgates +25000, Bosses +10000)
        └── IsEmergencyThreat (Lethal / Game-ending locks)
```

---

## 4. Turn 1 End-Board & Opponent Turn Interaction Chain

### Turn 1 Primary Line:
1. Normal Summon `Cupsy☆Yummy` (or `Marshmao☆Yummy`) $\rightarrow$ Search `Yummy☆Surprise`.
2. Link Summon `Yummy★Snatchy` (Link-1) $\rightarrow$ Places `Yummyusment☆Mignon` face-up from Deck.
3. Activate `Yummyusment☆Mignon` effect on field $\rightarrow$ Revives `Cupsy☆Yummy` from GY.
4. Synchro Summon `Cupsy★Yummy Way` (Lv2) using Snatchy (treated as Lv1 Tuner) + Cupsy (Lv1 non-Tuner).
5. `Cupsy★Yummy Way` triggers on summon $\rightarrow$ Searches `Cooky☆Yummy` + `Lollipo☆Yummy`, discards dead card / duplicate.
6. Special Summon `Cooky☆Yummy` and `Lollipo☆Yummy` from hand for free (control Level 2 Synchro).
7. Link Summon `Spright Elf` (Link-2) using `Cupsy★Yummy Way` + `Lollipo☆Yummy`.
8. Set `Yummy☆Surprise` and handtraps.
9. **End Board**: `Spright Elf` + `Cooky☆Yummy` + `Yummy☆Surprise` set + `Yummyusment☆Mignon` active + 3-4 handtraps in hand!

### Opponent Turn Disruptions (6-7 Layers Total):
1. **Opponent activates any card/effect**:
   - `Spright Elf` activates Quick Effect $\rightarrow$ Revives `Cooky★Yummy Way` from GY!
   - `Cooky★Yummy Way` triggers on SS $\rightarrow$ Flips up to 2 opponent monsters face-down (**Book of Moon x2**)!
2. **Opponent activates another card/effect**:
   - `Cooky★Yummy Way` tags out $\rightarrow$ Returns to Extra Deck, revives `Cooky☆Yummy` + `Marshmao☆Yummy` from GY!
   - `Cooky☆Yummy` triggers $\rightarrow$ **Destroys 1 opponent monster**!
   - `Marshmao☆Yummy` triggers $\rightarrow$ Places `Yummyusment★Acroquey` face-up from Deck!
3. **If Snatchy is revived or on field**:
   - Quick Synchro into a Synchro monster $\rightarrow$ `Yummyusment★Acroquey` triggers to **destroy another opponent card**!
4. **`Yummy☆Surprise` Trap Activation**:
   - Bounces 2 of our LIGHT Beasts + **2 cards your opponent controls back to hand**!
5. **Graveyard Disruption**:
   - If `Lollipo☆Yummy` was summoned $\rightarrow$ **Banishes 1 key card from opponent GY**!
6. **Handtraps**:
   - `Maxx "C"`, `Ash Blossom`, `Ghost Belle`, `Effect Veiler`, `Dominus Purge`, `Infinite Impermanence`.
