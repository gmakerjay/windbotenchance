# Mikanko & AztecRock: Defense & Reflect OTK Decks Report

> **Date**: September 25, 2026  
> **Status**: Successfully Built & Deployed to `C:\Users\admin\Documents\EdoGame\`  
> **Framework**: C# .NET 10.0 / `ModernExecutor` Architecture  

---

## 1. Deck Summary

| Deck Name | Bot Name | Archetype / Playstyle | Win Condition | Board Resilience |
|---|---|---|---|---|
| **`Mikanko`** | `Mikanko` | Reflect & Force-Attack OTK | 10,600+ Damage Reflect via `Double-Edged Sword` / Kaiju | **Tier 0 Immunity**: Untargetable + Indestructible by battle + Indestructible by card effects |
| **`AztecRock`** | `AztecRock` | Stone Statue of the Aztecs Defense OTK / Rock Stun | 16,000 Damage Reflect (x8 multiplier: Aztec x2, Canyon x2, Cross Counter x2) | **Rock Fortress**: `Block Dragon` (all Rocks immune to destruction) + `Fossil Dyna` + `Koa'ki Meiru` |

---

## 2. Deck 1: Mikanko

### 2.1 Core Strategy & Combos
1. **The 10,600 Damage One-Hit KO**:
   - Special Summon a Kaiju (e.g. `Jizukiru, the Star Destroying Kaiju` 3,300 ATK) to the opponent's field by tributing their monster.
   - Equip `Double-Edged Sword` (+2,000 ATK) to Jizukiru (total 5,300 ATK).
   - Normal/Special Summon any 0 ATK Mikanko monster (e.g. `Hu-Li`, `Ha-Re`, `Ni-Ni`, or `Ohime`).
   - Attack Jizukiru with the 0 ATK Mikanko:
     - Battle damage = 5,300 - 0 = 5,300.
     - `Double-Edged Sword` effect: both players take 5,300 battle damage.
     - `Mikanko` effect: all battle damage the player would take is reflected to the opponent.
     - **Opponent takes 5,300 + 5,300 = 10,600 damage in ONE HIT!**
2. **Forced Attack Lock**:
   - `Heavenly Gate of the Mikanko` (Field Spell): All opponent monsters that can attack **must attack a monster equipped with an Equip Card**.
   - Furthermore, while a Mikanko battles, **opponent cannot activate cards or effects** until the end of the Damage Step!
3. **Impenetrable Board**:
   - While `Hu-Li the Jewel Mikanko` is equipped with an Equip Card, the opponent **cannot target any Mikanko cards on your field with card effects**, and Mikanko monsters cannot be destroyed by battle!
   - `Ni-Ni the Mirror Mikanko` can steal an opponent's monster as a Quick Effect during their turn.

---

## 3. Deck 2: AztecRock (Stone Statue of the Aztecs - Fortress Upgrade)

### 3.1 Core Strategy & Combos
1. **The 16,000 Damage Counter Reflection**:
   - `Stone Statue of the Aztecs` (2,000 DEF): Doubles any battle damage the opponent takes when attacking this card.
   - `Canyon` (Field Spell): Doubles battle damage when a Defense Position Rock monster is attacked (stacking to **x4 multiplier**).
   - `D2 Shield` / `Rise to Full Height`: Doubles the DEF of Stone Statue of the Aztecs from 2,000 to **4,000 DEF**!
   - `Cross Counter`: If the DEF of the attacked monster was higher than the ATK of the attacking monster, doubles the battle damage (stacking to **x8 multiplier**).
   - `Stronghold Guardian`: Handtrap discard in Damage Step gives Aztec +1500 DEF (total **5,500 DEF**!).
   - Opponent attacks 4,000 DEF Aztec $\rightarrow$ (4,000 - 0) x 4 = **16,000 DAMAGE!**
2. **Forced Attack Traps**:
   - `Battle Mania`: Activates in the opponent's Standby Phase. Changes all opponent monsters to face-up Attack Position and **forces them to attack this turn if able**!
   - `Rise to Full Height`: Doubles DEF and locks opponent attacks strictly into Aztec.
3. **Impenetrable Board & Backrow Protection (The Upgrades)**:
   - **`Lord of the Heavenly Prison` (Rock Type)**:
     - In Hand: Reveals in Main Phase 1 $\rightarrow$ **Set cards on the field cannot be destroyed by card effects!** (Protects face-down Aztec + all Set Traps against Feather Duster, Lightning Storm, Raigeki, Baronne, etc.).
     - On Trap Activation: Special Summons itself (3000 ATK / 3000 DEF wall) and **Sets ANY Spell or Trap directly from the Deck** (`Canyon`, `Battle Mania`, `D2 Shield`, `Solemn Judgment`)!
     - Searchable directly by `Gallant Granite`!
   - **`Block Dragon` (Continuous Rock Protection)**:
     - **All Rock monsters you control cannot be destroyed by card effects!** (Protects face-up Aztec, Dyna, Guardian, and Granite).
   - **`Harpie's Feather Duster` & Extra Deck Backrow Removal**:
     - Main Deck `Harpie's Feather Duster` wipes opponent backrow completely.
     - `Tornado Dragon` (Rank 4 Quick Effect) and `Knightmare Phoenix` pop opponent backrow.
   - **Multi-Layered Negation Shield**:
     - `Solemn Judgment` (3x): Omni-negate everything (Duster, Lightning Storm, Raigeki, Evenly Matched, Summons).
     - `Solemn Strike` (2x): Stops monster effects anywhere (field/hand/GY) trying to destroy our monsters, and negates Special Summons!
     - `Koa'ki Meiru Guardian` (3x): Monster effect negate & destroy.
     - `Koa'ki Meiru Wall` (2x): Spell card negate & destroy.
     - `Fossil Dyna Pachycephalo` (3x): Shuts down all Special Summons completely.
     - `Abyss Dweller`: Shuts down GY triggers (Mathmech, Tear, etc.).

---

## 4. Verification & Deployment

```
- WindBot Binaries: Compiled with 0 Errors, 0 Warnings
- Deployed To: C:\Users\admin\Documents\EdoGame\
- Deck Files:
  - C:\Users\admin\Documents\EdoGame\deck\Mikanko.ydk
  - C:\Users\admin\Documents\EdoGame\deck\AztecRock.ydk
- bots.json: Registered "Mikanko" and "AztecRock"
```
