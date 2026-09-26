// ============================================================
// CARD AUDIT — 2026_Dinomorphia (Undying Low-LP Trap Stun)
// ============================================================
// | Card Name                     | Type       | OPT? | HOPT? | Cost | Effect Summary                              | Activate When                               | NEVER Activate When                           |
// |-------------------------------|------------|------|-------|------|---------------------------------------------|---------------------------------------------|-----------------------------------------------|
// | Dinomorphia Therizia          | Monster L4 | Yes  | Yes   | None | NS/SS: Set 1 Dinomorphia Trap from Deck     | Turn 1 NS Priority #1 / On Summon           | Already set Frenzy & Domain                   |
// | Dinomorphia Diplos            | Monster L4 | Yes  | Yes   | None | NS/SS: Send 1 Dinomorphia card Dk to GY     | Turn 1 NS Priority #2 / Prime GY traps      | No traps in deck                              |
// | Miscellaneousaurus            | Monster L4 | Yes  | Yes   | Discard| Dinosaurs UNAFFECTED by opp activated eff  | Main Phase quick effect / Protection        | Outside Main Phase                            |
// | Wannabee!                     | Monster L2 | Yes  | Yes   | Send | End Phase excavate 5, set 1 Trap            | End Phase in Hand                           | No Set space                                  |
// | Lord of the Heavenly Prison   | Monster L10| Yes  | Yes   | Reveal| Protect Set cards / SS 3000 body & set Trap | Main Phase hand reveal / On Set Trap activ  | Already revealed / No Set S/T                |
// | Fossil Dig                    | Spell      | No   | No    | None | Search Level 6 or lower Dinosaur            | Main Phase 1 early search                   | Duality SS lock active                         |
// | Pot of Prosperity             | Spell      | Yes  | Yes   | Banish| Excavate 3/6, add 1, halve damage rest turn| Main Phase 1 early                          | After other draw effects                      |
// | Pot of Duality                | Spell      | Yes  | Yes   | None | Excavate 3, add 1, cannot SS rest of turn   | Main Phase 1 early (we SS in opp turn)      | If need to fuse on our turn                   |
// | Dinomorphia Frenzy            | NormalTrap | Yes  | Yes   | LP/2 | Fuse 1 from Deck + 1 from Extra Deck        | Opponent Main Phase -> Summon Rexterm       | Outside Opponent Main Phase                   |
// | Dinomorphia Domain            | NormalTrap | Yes  | Yes   | LP/2 | Fuse from Hand/Field/Deck                   | Main Phase -> Summon Kentregina/Rexterm     | No materials in deck                          |
// | Dinomorphia Intact            | CounterTr  | Yes  | Yes   | LP/2 | Negate monster eff & destroy, halve b-damage| Opponent activates monster effect           | Bot has no LP or already resolved             |
// | Dinomorphia Sonic             | CounterTr  | Yes  | Yes   | LP/2 | Negate Spell/Trap & destroy, pop 1 Dino     | Opponent activates key S/T                  | No Dino to destroy or safe                    |
// | Dinomorphia Brute             | NormalTrap | Yes  | Yes   | LP/2 | Destroy 1 Dino & 1 opp card                | Opponent controls key threat                | Opponent controls 0 cards                     |
// | Dinomorphia Alert             | NormalTrap | Yes  | Yes   | LP/2 | Special Summon up to 2 Dinos from GY        | Need bodies / Recovery / Grind phase        | Zone full or no Dinos in GY                   |
// | Dinomorphia Shell             | NormalTrap | Yes  | Yes   | LP/2 | SS 0/3000 Token, take 0 battle damage       | Emergency defense wall                      | Monster zone full                             |
// | Ferret Flames                 | NormalTrap | No   | No    | None | Opponent shuffles monsters until ATK <= LP  | Opponent controls monsters with sum ATK > LP| Opponent controls 0 monsters                  |
// | Trap Trick                    | NormalTrap | Yes  | Yes   | Banish| Banish 1 Normal Trap, set 1 copy from Deck  | Opponent turn to fetch Frenzy/Domain/Ferret  | Already activated trap after this             |
// | Solemn Judgment               | CounterTr  | No   | No    | LP/2 | Negate S/T or Summon                        | Opponent activates board breaker or boss SS | Safe game state                               |
// | Solemn Strike                 | CounterTr  | No   | No    | 1500 | Negate SS or monster effect                 | Opponent summons boss or key effect         | LP <= 1500                                    |
// | Dinomorphia Kentregina        | Fusion L6  | Yes  | Yes   | LP/2 | Copy Dinomorphia Normal Trap in GY          | Main Phase -> Copy Frenzy/Domain/Brute      | No normal traps in GY                         |
// | Dinomorphia Rexterm           | Fusion L8  | No   | No    | None | Opp monsters with ATK >= LP CANNOT activate | Continuous Floodgate                        | Always maintain on field                      |
// | Dinomorphia Rexterm (Quick)   | Fusion L8  | Yes  | No    | LP/2 | Make all opp monsters ATK = LP              | Opponent has monsters with ATK > LP         | Opponent has 0 face-up monsters               |
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_Dinomorphia", "2026_Dinomorphia")]
    public class _2026_DinomorphiaExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Monsters
            public const int DinomorphiaTherizia = 92133240;
            public const int DinomorphiaDiplos = 38628859;
            public const int Miscellaneousaurus = 38572779;
            public const int Wannabee = 3248469;
            public const int LordOfTheHeavenlyPrison = 9822220;

            // Spells
            public const int FossilDig = 47325505;
            public const int PotOfProsperity = 84211599;
            public const int PotOfDuality = 98645731;

            // Traps - Fusion
            public const int DinomorphiaFrenzy = 78420796;
            public const int DinomorphiaDomain = 26631975;

            // Traps - Archetype
            public const int DinomorphiaIntact = 7336745;
            public const int DinomorphiaSonic = 52807032;
            public const int DinomorphiaBrute = 99414629;
            public const int DinomorphiaAlert = 52020510;
            public const int DinomorphiaShell = 25419323;

            // Traps - Stun & Staples
            public const int FerretFlames = 31044787;
            public const int TrapTrick = 80101899;
            public const int SolemnJudgment = 41420027;
            public const int SolemnStrike = 40605147;

            // Extra Deck
            public const int DinomorphiaKentregina = 48832775;
            public const int DinomorphiaRexterm = 92798873;
            public const int DinomorphiaStealthbergia = 74936480;
            public const int EvolzarLars = 35103106;
            public const int EvolzarDolkka = 42752141;
            public const int EvolzarLaggia = 74294676;
            public const int AbyssDweller = 21044178;
            public const int SuperStarslayerTYPHONSkyCrisis = 93039339;
            public const int DivineArsenalAAZEUS = 90448279;
        }

        // Standard OCG Hint IDs
        private const long HINT_SELECT_FACEUP = 500;
        private const long HINT_SELECT_DISCARD = 501;
        private const long HINT_SELECT_DESTROY = 502;
        private const long HINT_SELECT_REMOVE = 503;
        private const long HINT_SELECT_TOGRAVE = 504;
        private const long HINT_SELECT_RTOHAND = 505;
        private const long HINT_SELECT_ATOHAND = 506;
        private const long HINT_SELECT_SPSUMMON = 509;
        private const long HINT_SELECT_SET = 510;
        private const long HINT_SELECT_FMATERIAL = 511;

        // Turn state trackers
        private bool _theriziaSummonUsed = false;
        private bool _fossilDigUsed = false;
        private bool _dualityUsed = false;
        private bool _prosperityUsed = false;
        private bool _rextermAtkReductionUsed = false;
        private bool _kentreginaCopyUsed = false;
        private bool _heavenlyPrisonRevealed = false;
        private bool _miscProtectionUsed = false;

        public _2026_DinomorphiaExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // Register strategic assets
            ResourcePlan.RegisterAceCards(
                CardId.DinomorphiaRexterm,
                CardId.DinomorphiaKentregina,
                CardId.LordOfTheHeavenlyPrison,
                CardId.EvolzarLars,
                CardId.EvolzarDolkka
            );

            BaitPlanner.RegisterComboStarters(
                CardId.PotOfDuality,
                CardId.PotOfProsperity,
                CardId.FossilDig
            );

            ChainAdvisor.RegisterHighValueTargets(
                CardId.DinomorphiaRexterm,
                CardId.DinomorphiaKentregina,
                CardId.DinomorphiaTherizia
            );

            RegisterOptionalFieldRemovalCards(CardId.DinomorphiaBrute, CardId.DinomorphiaSonic);

            // ═══════════════════════════════════════════════════════════════
            //  EXECUTORS PIPELINE (Tiered Priority Execution)
            // ═══════════════════════════════════════════════════════════════

            // ── Tier 0: Counter Traps, Direct Negation & Protection ──
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, SolemnJudgmentEffect);
            AddExecutor(ExecutorType.Activate, CardId.SolemnStrike, SolemnStrikeEffect);
            AddExecutor(ExecutorType.Activate, CardId.DinomorphiaIntact, DinomorphiaIntactEffect);
            AddExecutor(ExecutorType.Activate, CardId.DinomorphiaSonic, DinomorphiaSonicEffect);
            AddExecutor(ExecutorType.Activate, CardId.Miscellaneousaurus, MiscellaneousaurusEffect);

            // ── Tier 1: Board Wipes & High-Impact Interruptions ──
            AddExecutor(ExecutorType.Activate, CardId.FerretFlames, FerretFlamesEffect);
            AddExecutor(ExecutorType.Activate, CardId.DinomorphiaRexterm, RextermEffect);
            AddExecutor(ExecutorType.Activate, CardId.DinomorphiaKentregina, KentreginaEffect);
            AddExecutor(ExecutorType.Activate, CardId.DinomorphiaBrute, DinomorphiaBruteEffect);
            AddExecutor(ExecutorType.Activate, CardId.DinomorphiaFrenzy, DinomorphiaFrenzyEffect);
            AddExecutor(ExecutorType.Activate, CardId.DinomorphiaDomain, DinomorphiaDomainEffect);
            AddExecutor(ExecutorType.Activate, CardId.TrapTrick, TrapTrickEffect);
            AddExecutor(ExecutorType.Activate, CardId.DinomorphiaAlert, DinomorphiaAlertEffect);
            AddExecutor(ExecutorType.Activate, CardId.DinomorphiaShell, DinomorphiaShellEffect);

            // ── Tier 2: Graveyard Floater Triggers & Hand Activations ──
            AddExecutor(ExecutorType.Activate, CardId.LordOfTheHeavenlyPrison, HeavenlyPrisonEffect);
            AddExecutor(ExecutorType.Activate, CardId.Wannabee, WannabeeEffect);
            AddExecutor(ExecutorType.Activate, CardId.DinomorphiaTherizia, TheriziaEffect);
            AddExecutor(ExecutorType.Activate, CardId.DinomorphiaDiplos, DiplosEffect);
            AddExecutor(ExecutorType.Activate, CardId.DinomorphiaStealthbergia, StealthbergiaEffect);
            AddExecutor(ExecutorType.Activate, CardId.EvolzarDolkka, EvolzarDolkkaEffect);
            AddExecutor(ExecutorType.Activate, CardId.EvolzarLars, EvolzarLarsEffect);
            AddExecutor(ExecutorType.Activate, CardId.EvolzarLaggia, EvolzarLaggiaEffect);

            // ── Tier 3: Searchers, Consistency & Setup Spells ──
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, PotOfProsperityEffect);
            AddExecutor(ExecutorType.Activate, CardId.PotOfDuality, PotOfDualityEffect);
            AddExecutor(ExecutorType.Activate, CardId.FossilDig, FossilDigEffect);

            // ── Tier 4: Normal Summons ──
            AddExecutor(ExecutorType.Summon, CardId.DinomorphiaTherizia, TheriziaSummon);
            AddExecutor(ExecutorType.Summon, CardId.DinomorphiaDiplos, DiplosSummon);
            AddExecutor(ExecutorType.Summon, CardId.Miscellaneousaurus, MiscSummon);

            // ── Tier 5: Extra Deck Xyz Summons (When 2 Lv4 Dinos on field) ──
            AddExecutor(ExecutorType.SpSummon, CardId.EvolzarLars);
            AddExecutor(ExecutorType.SpSummon, CardId.EvolzarDolkka);
            AddExecutor(ExecutorType.SpSummon, CardId.EvolzarLaggia);
            AddExecutor(ExecutorType.SpSummon, CardId.AbyssDweller);
            AddExecutor(ExecutorType.SpSummon, CardId.SuperStarslayerTYPHONSkyCrisis);

            // ── Tier 6: Spell & Trap Sets ──
            AddExecutor(ExecutorType.SpellSet, CardId.DinomorphiaFrenzy, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.DinomorphiaDomain, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.DinomorphiaIntact, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.FerretFlames, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnJudgment, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnStrike, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.TrapTrick, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.DinomorphiaSonic, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.DinomorphiaBrute, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.DinomorphiaAlert, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.DinomorphiaShell, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _theriziaSummonUsed = false;
            _fossilDigUsed = false;
            _dualityUsed = false;
            _prosperityUsed = false;
            _rextermAtkReductionUsed = false;
            _kentreginaCopyUsed = false;
            _heavenlyPrisonRevealed = false;
            _miscProtectionUsed = false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  SITUATIONAL HELPER METHODS (Board Reading & LP Efficiency)
        // ═══════════════════════════════════════════════════════════════

        private bool IsRextermActiveOnField()
        {
            return Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && !m.IsDisabled() && m.IsCode(CardId.DinomorphiaRexterm));
        }

        private bool IsStealthbergiaFreeCostActive()
        {
            return Bot.LifePoints <= 2000 && Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && !m.IsDisabled() && m.IsCode(CardId.DinomorphiaStealthbergia));
        }

        private bool IsOpponentMonsterLockedByRexterm(ClientCard monster)
        {
            if (monster == null || !IsRextermActiveOnField()) return false;
            // Rexterm continuous floodgate: Opponent cannot activate effects of monsters with ATK >= our LP
            return monster.Attack >= Bot.LifePoints;
        }

        private bool HasSafeSelfDestructionTarget()
        {
            // Returns true if we control a Dinomorphia monster OTHER than our only Rexterm
            var dinos = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && IsDinomorphiaMonster(m)).ToList();
            if (dinos.Count == 0) return false;
            // If we have any floater (Therizia, Diplos, Stealthbergia, Kentregina) or token
            if (dinos.Any(m => m.Id != CardId.DinomorphiaRexterm)) return true;
            // If we have multiple Rexterms (more than 1)
            return dinos.Count(m => m.Id == CardId.DinomorphiaRexterm) > 1;
        }

        private bool ShouldPreventBattleDamage()
        {
            if (Bot.LifePoints > 2000) return false;
            if (Duel.Phase != DuelPhase.Damage && Duel.Phase != DuelPhase.DamageCal && Duel.Phase != DuelPhase.BattleStep)
                return false;

            if (Enemy.BattlingMonster != null)
            {
                if (Bot.BattlingMonster == null)
                {
                    // Direct attack incoming!
                    return true;
                }
                if (Bot.BattlingMonster.IsAttack() && Enemy.BattlingMonster.Attack > Bot.BattlingMonster.Attack)
                    return true;
                if (Bot.BattlingMonster.IsDefense() && Enemy.BattlingMonster.Attack > Bot.BattlingMonster.Defense)
                    return true;
            }
            return false;
        }

        private bool ShouldPreventEffectDamage()
        {
            if (Bot.LifePoints > 2000) return false;
            if (LastChainCard == null || LastChainCard.Controller != 1) return false;

            // If LP is critically low (<= 1000), prevent any damage from opponent's chain
            if (Bot.LifePoints <= 1000) return true;

            // Opponent card is an activated effect that could burn
            return LastChainCard.IsSpell() || LastChainCard.IsTrap() || LastChainCard.IsMonster();
        }

        private static bool IsMassBoardWipe(ClientCard card)
        {
            if (card == null) return false;
            int id = card.Id;
            return id == 18144506 || // Harpie's Feather Duster
                   id == 12580477 || // Raigeki
                   id == 43898403 || // Lightning Storm
                   id == 57728570 || // Evenly Matched
                   id == 27204311 || // Nibiru
                   id == 24299458 || // Forbidden Droplet
                   id == 10045474 || // Dark Ruler No More
                   id == 48130397 || // Super Polymerization
                   id == 72302403 || // Swords of Revealing Light
                   id == 23002292;   // Red Reboot
        }

        // ═══════════════════════════════════════════════════════════════
        //  ACTIVATION & STRATEGY IMPLEMENTATIONS
        // ═══════════════════════════════════════════════════════════════

        private bool SolemnJudgmentEffect()
        {
            // Negate high impact enemy threats:
            // 1. Mass removal / board wipes
            if (LastChainCard != null && LastChainCard.Controller == 1)
            {
                if (IsMassBoardWipe(LastChainCard)) return true;

                // Protect backrow & key monsters from dangerous Spells/Traps
                if (LastChainCard.IsSpell() || LastChainCard.IsTrap())
                {
                    return true;
                }
                return false;
            }

            // 2. High-threat Summons:
            // Only negate summons of boss monsters (Level/Rank >= 7, Link >= 3, or ATK >= 2800)
            if (Duel.LastSummonPlayer == 1)
            {
                var summoned = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault();
                if (summoned != null && (summoned.Attack >= 2800 || summoned.Level >= 7 || summoned.Rank >= 7 || summoned.LinkCount >= 3))
                    return true;
            }

            return false;
        }

        private bool SolemnStrikeEffect()
        {
            // Fixed cost: 1500 LP! Must have enough LP to pay
            if (Bot.LifePoints <= 1500) return false;

            // If paying 1500 leaves us below 500, only use for game-saving negations
            bool isCriticalLp = Bot.LifePoints <= 2000;

            // 1. Monster effect negation:
            if (LastChainCard != null && LastChainCard.Controller == 1 && LastChainCard.IsMonster())
            {
                // If Rexterm already locks this monster on field, do NOT waste 1500 LP!
                if (IsOpponentMonsterLockedByRexterm(LastChainCard))
                    return false;

                // Negate dangerous effects from Hand/GY or un-locked field monsters
                return true;
            }

            // 2. Special Summon negation:
            if (Duel.LastSummonPlayer == 1)
            {
                if (isCriticalLp)
                {
                    // Only negate high-threat boss summons
                    var summoned = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault();
                    return summoned != null && summoned.Attack >= 2500;
                }
                return true;
            }

            return false;
        }

        private bool DinomorphiaIntactEffect()
        {
            // 1. GY Effect: Battle damage nullification during damage calculation
            if (Card.Location == CardLocation.Grave)
            {
                return ShouldPreventBattleDamage();
            }

            // 2. Field Activation: Negate monster effect activation
            if (LastChainCard != null && LastChainCard.Controller == 1 && LastChainCard.IsMonster())
            {
                // If Rexterm is on field and the monster is on field with ATK >= LP,
                // that monster's effect is already illegal/locked!
                if (IsOpponentMonsterLockedByRexterm(LastChainCard))
                    return false;

                // Must control a Dinomorphia card to activate
                bool hasDinoCard = Bot.GetMonsters().Any(m => m.IsFaceup() && IsDinomorphiaMonster(m)) ||
                                   Bot.GetSpells().Any(s => s.IsFaceup() && s.HasSetcode(0x173));
                if (!hasDinoCard) return false;

                // Intact halves all battle damage taken this turn, providing huge survival value
                return true;
            }

            return false;
        }

        private bool DinomorphiaSonicEffect()
        {
            // 1. GY Effect: Battle damage nullification
            if (Card.Location == CardLocation.Grave)
            {
                return ShouldPreventBattleDamage();
            }

            // 2. Field Activation: Negate opponent's Spell or Trap
            if (LastChainCard != null && LastChainCard.Controller == 1 && (LastChainCard.IsSpell() || LastChainCard.IsTrap()))
            {
                // CRITICAL SAFETY: "then, destroy 1 Dinomorphia monster you control"
                // Never sacrifice our only Rexterm unless facing a mass wipe that kills Rexterm anyway!
                if (!HasSafeSelfDestructionTarget())
                {
                    if (IsMassBoardWipe(LastChainCard))
                        return true;
                    return false;
                }

                return true;
            }

            return false;
        }

        private bool MiscellaneousaurusEffect()
        {
            // If in Hand during Main Phase: Protect Dinosaurs from opponent activated effects
            if (Card.Location == CardLocation.Hand && Duel.IsMainPhase())
            {
                if (_miscProtectionUsed) return false;
                // Activate if opponent chained something to our Dinos, or before we perform key fusions
                if (LastChainCard != null && LastChainCard.Controller == 1)
                {
                    _miscProtectionUsed = true;
                    return true;
                }
                if (Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasRace(CardRace.Dinosaur)) && Enemy.GetMonsterCount() > 0)
                {
                    _miscProtectionUsed = true;
                    return true;
                }
            }
            // If in GY: Banish to special summon Therizia from deck
            if (Card.Location == CardLocation.Grave)
            {
                int dinoCount = Bot.Graveyard.Count(c => c.HasRace(CardRace.Dinosaur));
                if (dinoCount >= 4 && !Bot.HasInMonstersZone(CardId.DinomorphiaTherizia) && !Bot.HasInHand(CardId.DinomorphiaTherizia))
                {
                    return true;
                }
            }
            return false;
        }

        private bool FerretFlamesEffect()
        {
            // Ferret Flames costs 0 LP!
            // Opponent must shuffle monsters until ATK <= our LP!
            if (Enemy.GetMonsterCount() == 0) return false;

            var oppFaceup = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup()).ToList();
            if (oppFaceup.Count == 0) return false;

            int totalOppAtk = oppFaceup.Sum(m => m.Attack);
            if (totalOppAtk <= Bot.LifePoints) return false;

            // Activate in Battle Phase to blow out attacks, or in Main Phase if opponent finished summoning
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep || Duel.Phase == DuelPhase.Battle)
                return true;

            // In Main Phase: activate if opponent has 2+ monsters or high total ATK
            if (Duel.IsMainPhase() && (oppFaceup.Count >= 2 || totalOppAtk >= 3000))
                return true;

            return false;
        }

        private bool RextermEffect()
        {
            // Quick effect: Pay half LP, make all opp monsters ATK = our current LP
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_rextermAtkReductionUsed) return false;

                var oppFaceupMonsters = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup()).ToList();
                if (oppFaceupMonsters.Count == 0) return false;

                // 1. In Battle Phase:
                // If an opponent monster is attacking and its ATK exceeds our battling monster (or direct attack):
                // Shrinking its ATK to our LP allows Rexterm (3000 ATK) to survive or crush the attacker!
                if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep || Duel.Phase == DuelPhase.Damage)
                {
                    ClientCard attacker = Enemy.BattlingMonster;
                    if (attacker != null && attacker.Attack > Bot.LifePoints / 2)
                    {
                        _rextermAtkReductionUsed = true;
                        return true;
                    }
                    if (oppFaceupMonsters.Any(m => m.Attack >= 3000))
                    {
                        _rextermAtkReductionUsed = true;
                        return true;
                    }
                }

                // 2. In Main Phase:
                // Only activate if opponent controls monsters whose ATK is LESS than our LP (meaning they are NOT locked yet!)
                // If they already have ATK >= LP, Rexterm's continuous effect ALREADY locks them!
                bool hasUnlockedMonsters = oppFaceupMonsters.Any(m => m.Attack < Bot.LifePoints);
                if (hasUnlockedMonsters && Duel.IsMainPhase())
                {
                    // Case 2A: Opponent monster is attempting to activate an on-field effect
                    if (LastChainCard != null && LastChainCard.Controller == 1 &&
                        LastChainCard.Location == CardLocation.MonsterZone && LastChainCard.Attack < Bot.LifePoints)
                    {
                        _rextermAtkReductionUsed = true;
                        return true;
                    }

                    // Case 2B: Opponent has multiple monsters on field (potential Link/Xyz climb) or high threat
                    if (oppFaceupMonsters.Count >= 2 && Bot.LifePoints > 1000)
                    {
                        _rextermAtkReductionUsed = true;
                        return true;
                    }
                }

                return false;
            }

            // Trigger effect when destroyed: Special Summon Lv6 or lower from GY (Kentregina, Stealthbergia, Therizia, Diplos)
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }

            return false;
        }

        private bool KentreginaEffect()
        {
            // Quick effect: During Main Phase, pay half LP, banish 1 Dinomorphia Normal Trap from GY, copy effect
            if (Card.Location == CardLocation.MonsterZone && Duel.IsMainPhase())
            {
                if (_kentreginaCopyUsed) return false;

                bool hasRexterm = Bot.HasInMonstersZone(CardId.DinomorphiaRexterm);

                // Priority 1: If we DON'T have Rexterm on field, summon Rexterm!
                if (!hasRexterm)
                {
                    // In opponent's Main Phase: can copy Frenzy or Domain
                    if (Duel.Player == 1)
                    {
                        bool canCopy = Bot.Graveyard.Any(c => c.Id == CardId.DinomorphiaFrenzy || c.Id == CardId.DinomorphiaDomain);
                        if (canCopy)
                        {
                            _kentreginaCopyUsed = true;
                            return true;
                        }
                    }
                    // In our Main Phase: can ONLY copy Domain (Frenzy requires opponent Main Phase!)
                    else
                    {
                        bool canCopyDomain = Bot.Graveyard.Any(c => c.Id == CardId.DinomorphiaDomain);
                        if (canCopyDomain)
                        {
                            _kentreginaCopyUsed = true;
                            return true;
                        }
                    }
                }

                // Priority 2: If Rexterm IS on field, copy Brute to pop opponent threat
                // ONLY if we have another Dinomorphia to destroy (or Kentregina herself pops and floats)
                if (hasRexterm && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0))
                {
                    bool hasBruteInGY = Bot.Graveyard.Any(c => c.Id == CardId.DinomorphiaBrute);
                    if (hasBruteInGY && HasSafeSelfDestructionTarget())
                    {
                        _kentreginaCopyUsed = true;
                        return true;
                    }
                }

                // Priority 3: Recovery with Alert in End of Main Phase if field has space
                if (Bot.GetMonsterCount() <= 2 && Bot.Graveyard.Any(c => c.Id == CardId.DinomorphiaAlert))
                {
                    _kentreginaCopyUsed = true;
                    return true;
                }
            }

            // Trigger effect when destroyed: Special Summon Lv4 from GY
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }

            return false;
        }

        private bool DinomorphiaBruteEffect()
        {
            // 1. GY Effect: Effect damage nullification
            if (Card.Location == CardLocation.Grave)
            {
                return ShouldPreventEffectDamage();
            }

            // 2. Field Activation: Destroy 1 Dinomorphia monster we control and 1 card opponent controls
            if (Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0) return false;

            // CRITICAL SAFETY: Never destroy our only Rexterm!
            if (!HasSafeSelfDestructionTarget()) return false;

            return true;
        }

        private bool DinomorphiaFrenzyEffect()
        {
            // 1. GY Effect: Effect damage nullification
            if (Card.Location == CardLocation.Grave)
            {
                // Prefer saving Frenzy in GY for Kentregina unless taking fatal effect damage
                if (Bot.LifePoints <= 1000)
                    return ShouldPreventEffectDamage();
                return false;
            }

            // 2. Field Activation: Opponent's Main Phase
            if (Duel.Player == 1 && Duel.IsMainPhase())
            {
                // Always summon Rexterm if not on field
                if (!IsRextermActiveOnField()) return true;

                // If Rexterm is already on field, only summon if we have room and opponent has multiple cards
                return Bot.GetMonsterCount() <= 2 && Enemy.GetMonsterCount() > 0;
            }

            return false;
        }

        private bool DinomorphiaDomainEffect()
        {
            // 1. GY Effect: Effect damage nullification
            if (Card.Location == CardLocation.Grave)
            {
                // Prefer saving Domain in GY for Kentregina unless taking fatal effect damage
                if (Bot.LifePoints <= 1000)
                    return ShouldPreventEffectDamage();
                return false;
            }

            // 2. Field Activation: Main Phase of either player
            if (Duel.IsMainPhase())
            {
                if (!IsRextermActiveOnField()) return true;

                // If Rexterm is on field, can summon Kentregina to provide extra pressure/copying
                return !Bot.HasInMonstersZone(CardId.DinomorphiaKentregina) && Bot.GetMonsterCount() <= 3;
            }

            return false;
        }

        private bool TrapTrickEffect()
        {
            if (Duel.Player != 1) return false;

            // Trap Trick locks us into ONLY 1 MORE TRAP for the rest of the turn!
            // Do NOT activate if we already have Frenzy or Domain set on field!
            bool hasFusionTrap = Bot.GetSpells().Any(s => s != null && s.IsFacedown() &&
                (s.IsCode(CardId.DinomorphiaFrenzy) || s.IsCode(CardId.DinomorphiaDomain)));
            if (hasFusionTrap && !Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Attack > Bot.LifePoints))
            {
                return false;
            }

            // If we have multiple counter traps set (e.g. Intact + Judgment), do not lock them out
            int setCounterTraps = Bot.GetSpells().Count(s => s != null && s.IsFacedown() &&
                (s.IsCode(CardId.DinomorphiaIntact) || s.IsCode(CardId.SolemnJudgment) || s.IsCode(CardId.SolemnStrike)));
            if (setCounterTraps >= 2) return false;

            // Activate if we need Frenzy (and don't have one) or Ferret Flames
            return true;
        }

        private bool DinomorphiaAlertEffect()
        {
            if (Card.Location == CardLocation.SpellZone)
            {
                // Special summon up to 2 Dinos from GY whose total level <= 8
                // Only activate during opponent's End Phase or when we need field recovery
                if (Bot.GetMonsterCount() >= 4) return false;
                return Bot.Graveyard.Count(c => IsDinomorphiaMonster(c) && c.Level <= 4) >= 1;
            }
            return Card.Location == CardLocation.Grave && ShouldPreventEffectDamage();
        }

        private bool DinomorphiaShellEffect()
        {
            // 1. GY Effect: Battle damage nullification
            if (Card.Location == CardLocation.Grave)
            {
                return ShouldPreventBattleDamage();
            }

            // 2. Field Activation: ONLY during Battle Phase when opponent is attacking!
            if (Card.Location == CardLocation.SpellZone)
            {
                if (Duel.Phase != DuelPhase.Battle && Duel.Phase != DuelPhase.BattleStart && Duel.Phase != DuelPhase.BattleStep)
                    return false;

                // Need space for Token
                if (Bot.GetMonsterCount() >= 5) return false;

                // Activate if opponent has attacking monsters
                return Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsAttack());
            }

            return false;
        }

        private bool HeavenlyPrisonEffect()
        {
            // In Hand: Reveal during Main Phase to protect set cards
            if (Card.Location == CardLocation.Hand && Duel.IsMainPhase())
            {
                if (!_heavenlyPrisonRevealed && Bot.GetSpells().Any(s => s.IsFacedown()))
                {
                    _heavenlyPrisonRevealed = true;
                    return true;
                }
            }
            // Trigger when Set card is activated: Special Summon this card!
            if (Card.Location == CardLocation.Hand && _heavenlyPrisonRevealed)
            {
                return true;
            }
            return false;
        }

        private bool WannabeeEffect()
        {
            // End Phase hand activation
            if (Card.Location == CardLocation.Hand && Duel.Phase == DuelPhase.End)
            {
                return Bot.GetSpellCountWithoutField() < 5;
            }
            return false;
        }

        private bool TheriziaEffect()
        {
            // On Normal or Special Summon: Set 1 Dinomorphia Trap directly from deck!
            if (Card.Location == CardLocation.MonsterZone)
            {
                return true;
            }
            // Trigger when destroyed: Special Summon Lv4 from GY by banishing 1 trap
            if (Card.Location == CardLocation.Grave)
            {
                return Bot.Graveyard.Any(c => c.IsTrap());
            }
            return false;
        }

        private bool DiplosEffect()
        {
            // On Summon: Send 1 Dinomorphia card from deck to GY
            if (Card.Location == CardLocation.MonsterZone)
            {
                return true;
            }
            // Trigger when destroyed: Special Summon Lv4 from GY
            if (Card.Location == CardLocation.Grave)
            {
                return Bot.Graveyard.Any(c => c.IsTrap());
            }
            return false;
        }

        private bool StealthbergiaEffect()
        {
            // Trigger when destroyed
            return Card.Location == CardLocation.Grave;
        }

        private bool EvolzarDolkkaEffect()
        {
            // Negate monster effect activation and destroy (no once-per-turn!)
            return LastChainCard != null && LastChainCard.Controller == 1 && LastChainCard.IsMonster();
        }

        private bool EvolzarLarsEffect()
        {
            // Negate face-up card effect
            return LastChainCard != null && LastChainCard.Controller == 1;
        }

        private bool EvolzarLaggiaEffect()
        {
            // Negate Summon or S/T activation
            return LastChainCard != null && LastChainCard.Controller == 1;
        }

        private bool PotOfProsperityEffect()
        {
            if (_prosperityUsed || _dualityUsed) return false;
            _prosperityUsed = true;
            return true;
        }

        private bool PotOfDualityEffect()
        {
            if (_dualityUsed || _prosperityUsed) return false;
            _dualityUsed = true;
            return true;
        }

        private bool FossilDigEffect()
        {
            if (_fossilDigUsed) return false;
            _fossilDigUsed = true;
            return true;
        }

        private bool TheriziaSummon()
        {
            if (_theriziaSummonUsed) return false;
            _theriziaSummonUsed = true;
            return true;
        }

        private bool DiplosSummon()
        {
            // Only normal summon Diplos if we don't have Therizia in hand
            return !Bot.HasInHand(CardId.DinomorphiaTherizia);
        }

        private bool MiscSummon()
        {
            // Fallback summon for beatdown
            return Bot.GetMonsterCount() == 0 && Bot.Hand.Count <= 2;
        }

        // ═══════════════════════════════════════════════════════════════
        //  CARD & TARGET SELECTION OVERRIDES (OnSelectCard)
        // ═══════════════════════════════════════════════════════════════

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return base.OnSelectCard(cards, min, max, hint, cancelable);

            // 1. Select Fusion Material (511 / 504 / 509)
            if (hint == HINT_SELECT_FMATERIAL || hint == HINT_SELECT_TOGRAVE)
            {
                var materials = new List<ClientCard>();

                // Extra Deck material: Prefer Kentregina or Stealthbergia
                var edMat = cards.FirstOrDefault(c => c.Location == CardLocation.Extra && (c.Id == CardId.DinomorphiaKentregina || c.Id == CardId.DinomorphiaStealthbergia));
                if (edMat != null) materials.Add(edMat);

                // Main Deck material: Prefer Diplos over Therizia (save Therizia for normal summons/searches)
                var mainMat = cards.FirstOrDefault(c => c.Location == CardLocation.Deck && c.Id == CardId.DinomorphiaDiplos)
                           ?? cards.FirstOrDefault(c => c.Location == CardLocation.Deck && c.Id == CardId.DinomorphiaTherizia);
                if (mainMat != null && !materials.Contains(mainMat)) materials.Add(mainMat);

                if (materials.Count >= min) return materials.Take(max).ToList();
            }

            // 2. Select Fusion Summon Monster (Frenzy / Domain)
            var rexterm = cards.FirstOrDefault(c => c.Id == CardId.DinomorphiaRexterm);
            if (rexterm != null && !Bot.HasInMonstersZone(CardId.DinomorphiaRexterm))
            {
                return new List<ClientCard> { rexterm };
            }
            var kentregina = cards.FirstOrDefault(c => c.Id == CardId.DinomorphiaKentregina);
            if (kentregina != null)
            {
                return new List<ClientCard> { kentregina };
            }

            // 3. Select Card to Set from Deck (Therizia / Trap Trick)
            if (hint == HINT_SELECT_SET)
            {
                var frenzy = cards.FirstOrDefault(c => c.Id == CardId.DinomorphiaFrenzy);
                if (frenzy != null && !Bot.HasInSpellZone(CardId.DinomorphiaFrenzy)) return new List<ClientCard> { frenzy };

                var domain = cards.FirstOrDefault(c => c.Id == CardId.DinomorphiaDomain);
                if (domain != null && !Bot.HasInSpellZone(CardId.DinomorphiaDomain)) return new List<ClientCard> { domain };

                var intact = cards.FirstOrDefault(c => c.Id == CardId.DinomorphiaIntact);
                if (intact != null) return new List<ClientCard> { intact };

                var ferret = cards.FirstOrDefault(c => c.Id == CardId.FerretFlames);
                if (ferret != null) return new List<ClientCard> { ferret };
            }

            // 4. Select Card to Destroy (Brute / Sonic)
            if (hint == HINT_SELECT_DESTROY)
            {
                // Self destruction target: Prioritize floaters, NEVER sacrifice Rexterm if avoidable!
                var selfFloater = cards.FirstOrDefault(c => c.Controller == 0 && c.Id == CardId.DinomorphiaDiplos)
                               ?? cards.FirstOrDefault(c => c.Controller == 0 && c.Id == CardId.DinomorphiaTherizia)
                               ?? cards.FirstOrDefault(c => c.Controller == 0 && c.Id == CardId.DinomorphiaStealthbergia)
                               ?? cards.FirstOrDefault(c => c.Controller == 0 && c.Id == CardId.DinomorphiaKentregina);
                if (selfFloater != null) return new List<ClientCard> { selfFloater };

                // Enemy destruction target: Highest threat (floodgate / negator / highest ATK)
                var enemyTarget = cards.Where(c => c.Controller == 1)
                    .OrderByDescending(c => CardIntelligence.IsFloodgate(c.Id) ? 10000 : 0)
                    .ThenByDescending(c => CardIntelligence.IsKnownNegator(c.Id) ? 8000 : 0)
                    .ThenByDescending(c => c.Attack)
                    .FirstOrDefault();
                if (enemyTarget != null) return new List<ClientCard> { enemyTarget };
            }

            // 5. Select Card to Banish from GY (Kentregina copy effect / Float cost)
            if (hint == HINT_SELECT_REMOVE)
            {
                // Kentregina copy: Domain > Frenzy > Brute > Alert
                var domainGY = cards.FirstOrDefault(c => c.Id == CardId.DinomorphiaDomain);
                if (domainGY != null && !Bot.HasInMonstersZone(CardId.DinomorphiaRexterm)) return new List<ClientCard> { domainGY };

                var frenzyGY = cards.FirstOrDefault(c => c.Id == CardId.DinomorphiaFrenzy);
                if (frenzyGY != null && !Bot.HasInMonstersZone(CardId.DinomorphiaRexterm) && Duel.Player == 1) return new List<ClientCard> { frenzyGY };

                var bruteGY = cards.FirstOrDefault(c => c.Id == CardId.DinomorphiaBrute);
                if (bruteGY != null && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0)) return new List<ClientCard> { bruteGY };
            }

            // 6. Select Card to Special Summon from GY (Floaters / Alert)
            if (hint == HINT_SELECT_SPSUMMON)
            {
                var ssTherizia = cards.FirstOrDefault(c => c.Id == CardId.DinomorphiaTherizia);
                if (ssTherizia != null) return new List<ClientCard> { ssTherizia };

                var ssDiplos = cards.FirstOrDefault(c => c.Id == CardId.DinomorphiaDiplos);
                if (ssDiplos != null) return new List<ClientCard> { ssDiplos };

                var ssKent = cards.FirstOrDefault(c => c.Id == CardId.DinomorphiaKentregina);
                if (ssKent != null) return new List<ClientCard> { ssKent };
            }

            // 7. Select Card to Add to Hand (Fossil Dig / Duality / Prosperity)
            if (hint == HINT_SELECT_ATOHAND)
            {
                var addTherizia = cards.FirstOrDefault(c => c.Id == CardId.DinomorphiaTherizia);
                if (addTherizia != null && !Bot.HasInHand(CardId.DinomorphiaTherizia)) return new List<ClientCard> { addTherizia };

                var addMisc = cards.FirstOrDefault(c => c.Id == CardId.Miscellaneousaurus);
                if (addMisc != null && !Bot.HasInHand(CardId.Miscellaneousaurus)) return new List<ClientCard> { addMisc };

                var addFrenzy = cards.FirstOrDefault(c => c.Id == CardId.DinomorphiaFrenzy);
                if (addFrenzy != null) return new List<ClientCard> { addFrenzy };

                var addDomain = cards.FirstOrDefault(c => c.Id == CardId.DinomorphiaDomain);
                if (addDomain != null) return new List<ClientCard> { addDomain };
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Rexterm: Always Attack Position (3000 ATK, shrinks enemy ATK to our LP)
            if (cardId == CardId.DinomorphiaRexterm)
            {
                if (positions.Contains(CardPosition.Attack)) return CardPosition.Attack;
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }

            // Stealthbergia: 2500 DEF wall, 0 ATK -> Prefer Defense Position!
            if (cardId == CardId.DinomorphiaStealthbergia)
            {
                if (positions.Contains(CardPosition.Defence)) return CardPosition.Defence;
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            }

            // Kentregina: Loses ATK equal to LP. If LP >= 3000, ATK is <= 1000 -> prefer Defense if possible
            if (cardId == CardId.DinomorphiaKentregina && Bot.LifePoints >= 3000)
            {
                if (positions.Contains(CardPosition.Defence)) return CardPosition.Defence;
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        private static bool IsDinomorphiaMonster(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.DinomorphiaTherizia ||
                   card.Id == CardId.DinomorphiaDiplos ||
                   card.Id == CardId.DinomorphiaKentregina ||
                   card.Id == CardId.DinomorphiaRexterm ||
                   card.Id == CardId.DinomorphiaStealthbergia;
        }
    }
}
