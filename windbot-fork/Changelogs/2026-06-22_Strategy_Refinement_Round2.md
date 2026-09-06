# Changelog: 2026-06-22 — Strategy Refinement Round 2 (All 8 Decks)

## Summary
Deep tactical logic refinement across all 8 executors. Focused on eliminating blind activations, preventing self-destruct behaviors, and improving situational decision-making.

---

## Archfiend (`_2026_ArchfiendExecutor.cs`)

### Critical: 13 Extra Deck Effects Were Blind `=> true`
All Extra Deck effect handlers unconditionally returned true, causing the AI to activate effects even when they'd hurt itself or do nothing.

**Fixed handlers:**
| Card | Before | After |
|------|--------|-------|
| Black Rose Dragon | Always nuke | Only nuke when enemy has ≥2 more cards |
| Topologic Trisbaena | Always banish | Only when enemy has S/T |
| Topologic Bomber | Always destroy | Only when enemy has ≥2 monsters |
| Topologic Zeroboros | Always banish all | Only when enemy has ≥3 more cards (last resort) |
| World Gears | Always activate | Only when enemy has ≥3 field cards |
| OddEyes Meteorburst | Always activate | Only when Pendulum Zone has a monster |
| Bramble/Rage/Garden/Periallis/Crossrose/TripleBurst | Always activate | Only when on MonsterZone |

### Added: NibiruEffect Handler
Was missing entirely — Nibiru would never activate. Now uses `SmartHandTrapChain()` guard.

### Fixed: ArchfiendEmperor Main Monster Zone
Emperor's on-field effect only worked in Extra Monster Zone. Now also activates in Main Monster Zone with banish-from-GY-to-pop logic.

---

## Monarch (`_2026_MonarchExecutor.cs`)

### Fixed: ZaborgMegaEffect Self-Destruct Going Second
Previously, when no enemy monster existed, Zaborg would target itself (line 559-564). Going second, this wastes a 2-tribute investment for no value. Now only self-targets on Turn 1 going first (for Extra Deck mill combo value).

### Fixed: ZaborgEffect (Small) Self-Destruct
`Util.GetBestEnemyMonster()` could return null and the effect would still not return false fast enough. Now explicitly targets only enemy monsters with `IsShouldNotBeTarget` filtering.

---

## Purrely (`_2026_PurrelyExecutor.cs`)

### Fixed: EpurrelyBeauty Detach Guard Contradiction
Comment said "keep at least 1 overlay for protection" but `CanSafelyDetach(Card, 1, 0)` used keepThreshold=0 (meaning detach even with only 1 overlay). Fixed to keepThreshold=1 so Beauty needs 2+ overlays to safely search.

### Improved: EpurrelyPlump Smart Negation
Previously negated any opponent activation indiscriminately. Now evaluates whether the chain card is actually threatening before spending an overlay:
- Monster with ATK ≥2000 → negate (removal threat)
- Spell/Trap → negate (likely disruption)
- Low-impact effects → conserve overlay if we have ≥3 left

### Improved: Purrelyeap Timing
Now activates during End Phase of either player (not just our End Phase).

---

## Runick (`_2026_RunickExecutor.cs`)

### Tightened: Destruction/FlashingFire Without Targets
Previously allowed activation with no target if `Bot.GetMonsterCount() == 0` (for SS option). This wasted the spell effect for just a Special Summon when Tip would be strictly better. Now requires either a valid target OR near-deck-out to justify activation.

---

## Regenesis (`_2026_RegenesisExecutor.cs`)

### Fixed: BystialDruiswurm Bare Boolean Return
`return Enemy.GetMonsters().Any(...)` on line 359 could evaluate to true, activating the effect without `AI.SelectCard` — causing the game to pick a random/wrong target. Now explicitly returns false when no valid target is found.

### Improved: RegenesisBirth Reactive Timing
Now prioritizes activation during opponent's turn or in response to opponent's chain (reactive > proactive). On our turn, only activates against ≥2500 ATK threats.

### Tightened: DragonsMind Activation
Previously activated whenever `NeedsBoardPresence()` returned true, even without a Dragon. Now requires either a Dragon on field OR a completely empty board as fallback.

---

## Yummy (`_2026_YummyExecutor.cs`)

### Fixed: CookyYummy GY Search — No Target Selection
GY search trigger blindly returned true without calling `AI.SelectCard`. Now dynamically selects:
- Priority 1: Marshmao (if not in hand, for next turn's search)
- Priority 2: Lollipo (tuner for Synchro plays)
- Fallback: Any Yummy monster

### Improved: YummySnatchy Target Prioritization
Now scores targets by effect value, not just ATK. Effect monsters and 2500+ ATK monsters get bonus priority, preventing the AI from stealing a vanilla beater over an active threat.

### Fixed: BorreloadSavageDragon Effect Order
Negate activation (chain to opponent) is now checked FIRST before on-summon equip. Previously, the equip check happened first and could consume the activation before getting a chance to negate. Equip now uses LINQ `.OrderByDescending(c => c.LinkCount)` for better selection.

---

## Build Verification
✅ `dotnet build` — **0 errors**, all pre-existing warnings only.
