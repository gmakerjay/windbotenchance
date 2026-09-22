# ArtMage Deck Optimization & ModernExecutor Playbook

## 1. Deck Profile & Card Audit

- **Archetype**: ArtMage (アルトメギア) / Power Patron (獄神獣)
- **Deck File**: `ArtMage.ydk` (40 Main Deck, 15 Extra Deck)
- **Executor Class**: `WindBot.Game.AI.Decks.ArtMageExecutor`
- **Registration**: Registered in `bots.json` as `ArtMage` & `Artmage`

### Main Deck Roles
| Card ID | Card Name | Role | Notes / Effect Mechanism |
|---|---|---|---|
| `97556336` | Medius the Pure | **Primary Starter** | On Summon: `aux.ToHandOrElse` (Opt 0: Add, Opt 1: SS Nervedo from Deck). GY: Shuffle 1 card to SS itself. |
| `17473466` | Power Patron Shadow Beast Nervedo | **Key Pivot / Pendulum** | Field: Banish 3 face-down -> Fusion Summon `Nerva` from Extra! Extra Deck Trigger: SS Artmage from Deck. P-Zone: Negates monster effect. |
| `23829452` | Artmage Power Patron | **Extender / Searcher** | Field Quick Effect: Fusion Summon using this card + hand/field. Sent to GY: Search Artmage S/T with different name from GY. |
| `34541940` | Artmage Finmel | **Boss / Blanket Negate** | Level 7 Warrior (2400 ATK). Hand SS + Draw 1. Quick Effect (3+ Types): Negate all face-up opponent monsters & halve ATK. Protects Lv<=6 Artmages from targeting. |
| `60946049` | Artmage Graflare | **Extender / Removal** | Level 5 Dragon (2100 ATK). Hand SS + Set Artmage Spell. Ignition/Quick Effect: Destroy 1 opponent Spell/Trap. |
| `97434754` | Artmage Litera | **Extender / Interruption** | Level 3 Spellcaster. Hand SS + retrieve Artmage card from GY. Opponent Main Phase Quick Effect: SS Artmage from hand/GY & bounce itself. |
| `74733322` | Arcane Arts Acropolis | **Field Spell / Search Engine** | Grants extra Medius NS. Discard S/T -> declare Artmage monster name -> search from Deck. Protects Nerva from effect destruction! |
| `74011784` | Artmage Varnish | **Searcher / Protection** | Places Acropolis, or if Acropolis on field, searches Artmage card. GY: Negates attack targeting Artmage. |
| `1122030` | Artmage Vandalism | **Starter Searcher** | On activation: Adds Medius from Deck to hand. Continuous: Protects Acropolis from destruction. |
| `37517035` | Artmage Masterwork | **Quick Fusion Spell** | Main Phase Quick Fusion using hand/field + Artmage. Field card gives +500 ATK. GY: Shuffles 3 distinct Artmage cards into Deck. |
| `23599634` | Artmage Pact | **Normal Trap / Board Breaker** | SS Medius/Artmage from Deck & makes Fusion activations un-negatable. GY: Returns Artmage to hand/ED to destroy 1 card opponent controls. |
| `44654994` | Artmage Impasto -Recapture- | **Counter Trap** | **Can activate the turn it was Set!** Monster effect negate: Banish 1 Fusion monster until End Phase -> negate & destroy. If 3+ Types: bounces all opponent Spells/Traps! |

### Extra Deck Targets
| Card ID | Card Name | Type / Stats | Role |
|---|---|---|---|
| `53589300` | Nerva the Power Patron of Creation | Fusion (Illusion) 3300/3300 | **Apex Boss**: Indestructible while Field Spell is up. Quick Effect: replaces Artmage monster effect with "Destroy all cards your opponent controls"! |
| `27184601` | Artmage Diactorus | Fusion (Fairy) 2800/2500 | **Omni-Negator**: Negates any card/effect activated on field (requires 3+ Types). Floats into Medius when destroyed. |
| `74631897` | Artmage Non-Finito | Fusion (Illusion) 2400/1500 | **Setup Boss**: On SS: Sets Artmage S/T (Impasto Recapture!). Opponent turn Quick Fusion. |
| `48130397` | Super Poly Targets | Fusions | Garura (`11765832`), Mudragon (`54757758`), Starving Venom (`41209827`), Dragostapelia (`69946549`), Earth Golem (`62111090`). |
| Links | S:P Little Knight, Cross-Sheep, Knightmare Cerberus, Accesscode | Utilities | Utility, recursion, removal, OTK finisher. |

---

## 2. Core Playbook & Combo Strategy

### Turn 1: Standard 1-Card Unbreakable Board
**Starting Card**: `Medius the Pure` (or `Vandalism`, or `Varnish` -> Acropolis)
1. **Normal Summon Medius the Pure**.
2. **Medius On-Summon**: `OnSelectOption` chooses `1` (Special Summon) -> Special Summons `Shadow Beast Nervedo` directly from Deck.
3. **Nervedo On-Field Ignition**: Banishes top 3 cards face-down -> destroys itself -> Special Summons `Nerva the Power Patron of Creation` (3300 ATK Fusion) from Extra Deck.
4. **Nervedo Extra Deck Trigger**: Triggers in face-up Extra Deck -> Special Summons `Artmage Finmel` (2400 ATK) from Deck.
5. **Field State**:
   - `Medius the Pure` (Fairy)
   - `Nerva` (Illusion)
   - `Artmage Finmel` (Warrior)
   - **Total Distinct Monster Types**: Exactly 3!
6. **Extenders**:
   - If holding `Varnish` or `Acropolis`: Place Acropolis -> Nerva gains complete card effect destruction immunity!
   - If holding `Graflare` or `Litera`: Hand Special Summon -> Sets `Masterwork` or adds resources.
   - If activating `Masterwork`: Fuses Medius + Artmage into `Artmage Diactorus` (Fairy Omni-Negator).
   - Medius GY effect: Shuffles 1 monster back to Deck -> revives Medius!
   - Non-Finito: Sets `Impasto Recapture` (Counter Trap that can be activated this turn!).
7. **End Board Disruptions**:
   - **Nerva**: Quick Board Wipe (Raigeki + Feather Duster) chained to friendly Artmage effect.
   - **Diactorus**: Omni-Negate any on-field card/effect.
   - **Finmel**: Quick blanket monster negate + ATK halving.
   - **Impasto Recapture**: Counter Trap monster negate + bounces all opponent backrow.
   - **Handtraps**: Ash Blossom / Impermanence / Called by the Grave.

### Turn 2 / Going Second: Board Breaking & Instant OTK
1. **Super Polymerization**: Fuse away opponent's key monsters without allowing response.
2. **Nerva Board Wipe**: Once Nerva is out, activate a low-cost Artmage ignition/trigger (Diactorus position change targeting an enemy, Graflare spell pop, or Litera) -> chain Nerva -> opponent's entire field is destroyed!
3. **Lethal Attack**: Nerva (3300) + Diactorus (2800) + Finmel (2400) = **8500 Direct Damage** (OTK).
