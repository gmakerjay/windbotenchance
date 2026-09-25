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
        //  ACTIVATION & STRATEGY IMPLEMENTATIONS
        // ═══════════════════════════════════════════════════════════════

        private bool SolemnJudgmentEffect()
        {
            // Negate high impact enemy cards
            if (LastChainCard != null && LastChainCard.Controller == 1)
            {
                return true;
            }
            return Duel.LastSummonPlayer == 1;
        }

        private bool SolemnStrikeEffect()
        {
            if (Bot.LifePoints <= 1500) return false;
            if (LastChainCard != null && LastChainCard.Controller == 1 && LastChainCard.IsMonster())
            {
                return true;
            }
            return Duel.LastSummonPlayer == 1;
        }

        private bool DinomorphiaIntactEffect()
        {
            // Negate monster effect activation anywhere
            if (LastChainCard != null && LastChainCard.Controller == 1 && LastChainCard.IsMonster())
            {
                return true;
            }
            return false;
        }

        private bool DinomorphiaSonicEffect()
        {
            // Negate opponent's spell or trap
            if (LastChainCard != null && LastChainCard.Controller == 1 && (LastChainCard.IsSpell() || LastChainCard.IsTrap()))
            {
                // Ensure we have a Dinomorphia to destroy (which will trigger float!)
                return Bot.GetMonsters().Any(m => m.IsFaceup() && IsDinomorphiaMonster(m));
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
            // If in GY: Banish to special summon from deck
            if (Card.Location == CardLocation.Grave)
            {
                int dinoCount = Bot.Graveyard.Count(c => c.HasRace(CardRace.Dinosaur));
                if (dinoCount >= 4 && !Bot.HasInMonstersZone(CardId.DinomorphiaTherizia))
                {
                    return true;
                }
            }
            return false;
        }

        private bool FerretFlamesEffect()
        {
            // Opponent must shuffle monsters until ATK <= our LP!
            // Activate when enemy has face-up monsters and total ATK > our LP, or during battle
            if (Enemy.GetMonsterCount() == 0) return false;
            int enemyTotalAtk = Enemy.GetMonsters().Where(m => m.IsFaceup()).Sum(m => m.Attack);
            if (enemyTotalAtk > Bot.LifePoints || Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep)
            {
                return true;
            }
            return false;
        }

        private bool RextermEffect()
        {
            // Quick effect: Pay half LP, make all opp monsters ATK = our current LP
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_rextermAtkReductionUsed) return false;
                // Activate if opponent controls any face-up monster with ATK > our LP
                bool oppHasHigherAtk = Enemy.GetMonsters().Any(m => m.IsFaceup() && m.Attack > Bot.LifePoints);
                if (oppHasHigherAtk || Duel.Phase == DuelPhase.BattleStep || (Duel.IsMainPhase() && Enemy.GetMonsterCount() > 0))
                {
                    _rextermAtkReductionUsed = true;
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

        private bool KentreginaEffect()
        {
            // Quick effect: During Main Phase, pay half LP, banish 1 Dinomorphia Normal Trap from GY, copy effect
            if (Card.Location == CardLocation.MonsterZone && Duel.IsMainPhase())
            {
                if (_kentreginaCopyUsed) return false;
                // If we don't have Rexterm, copy Frenzy or Domain to summon Rexterm!
                bool hasRexterm = Bot.HasInMonstersZone(CardId.DinomorphiaRexterm);
                if (!hasRexterm)
                {
                    bool hasFrenzyInGY = Bot.Graveyard.Any(c => c.Id == CardId.DinomorphiaFrenzy);
                    bool hasDomainInGY = Bot.Graveyard.Any(c => c.Id == CardId.DinomorphiaDomain);
                    if (hasFrenzyInGY || hasDomainInGY)
                    {
                        _kentreginaCopyUsed = true;
                        return true;
                    }
                }
                // If Rexterm is on field, copy Brute to pop opponent threat!
                bool hasBruteInGY = Bot.Graveyard.Any(c => c.Id == CardId.DinomorphiaBrute);
                if (hasBruteInGY && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0))
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
            // Target 1 Dinomorphia monster we control and 1 card opponent controls; destroy both
            if (Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0) return false;
            return Bot.GetMonsters().Any(m => m.IsFaceup() && IsDinomorphiaMonster(m));
        }

        private bool DinomorphiaFrenzyEffect()
        {
            // Activate during opponent's Main Phase!
            if (Duel.Player == 1 && Duel.IsMainPhase())
            {
                return true;
            }
            // Damage nullification in GY
            if (Card.Location == CardLocation.Grave)
            {
                return Bot.LifePoints <= 2000;
            }
            return false;
        }

        private bool DinomorphiaDomainEffect()
        {
            // Activate during Main Phase (our turn or opponent's turn)
            if (Duel.IsMainPhase())
            {
                // Prioritize summoning Rexterm or Kentregina
                return true;
            }
            // Damage nullification in GY
            if (Card.Location == CardLocation.Grave)
            {
                return Bot.LifePoints <= 2000;
            }
            return false;
        }

        private bool TrapTrickEffect()
        {
            // Banish 1 Normal Trap from Deck, set 1 copy
            // Use during opponent turn to set Frenzy or Ferret Flames
            if (Duel.Player == 1)
            {
                return true;
            }
            return false;
        }

        private bool DinomorphiaAlertEffect()
        {
            if (Card.Location == CardLocation.SpellZone)
            {
                // Special summon up to 2 Dinos from GY whose total level <= 8
                return Bot.Graveyard.Count(c => IsDinomorphiaMonster(c) && c.Level <= 4) >= 1;
            }
            return Card.Location == CardLocation.Grave && Bot.LifePoints <= 2000;
        }

        private bool DinomorphiaShellEffect()
        {
            if (Card.Location == CardLocation.SpellZone)
            {
                return Duel.Phase == DuelPhase.BattleStep || Enemy.GetMonsterCount() > Bot.GetMonsterCount();
            }
            return Card.Location == CardLocation.Grave && Bot.LifePoints <= 2000;
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

                // Main Deck material: Prefer Therizia or Diplos
                var mainMat = cards.FirstOrDefault(c => c.Location == CardLocation.Deck && (c.Id == CardId.DinomorphiaTherizia || c.Id == CardId.DinomorphiaDiplos));
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
                // Self destruction target: Therizia or Diplos or Stealthbergia (they float!)
                var selfFloater = cards.FirstOrDefault(c => c.Controller == 0 && (c.Id == CardId.DinomorphiaTherizia || c.Id == CardId.DinomorphiaDiplos || c.Id == CardId.DinomorphiaStealthbergia));
                if (selfFloater != null) return new List<ClientCard> { selfFloater };

                // Enemy destruction target: Highest threat or attack monster
                var enemyTarget = cards.Where(c => c.Controller == 1).OrderByDescending(c => c.Attack).FirstOrDefault();
                if (enemyTarget != null) return new List<ClientCard> { enemyTarget };
            }

            // 5. Select Card to Banish from GY (Kentregina copy effect / Float cost)
            if (hint == HINT_SELECT_REMOVE)
            {
                // Kentregina copy: Frenzy > Domain > Brute > Alert
                var frenzyGY = cards.FirstOrDefault(c => c.Id == CardId.DinomorphiaFrenzy);
                if (frenzyGY != null && !Bot.HasInMonstersZone(CardId.DinomorphiaRexterm)) return new List<ClientCard> { frenzyGY };

                var domainGY = cards.FirstOrDefault(c => c.Id == CardId.DinomorphiaDomain);
                if (domainGY != null && !Bot.HasInMonstersZone(CardId.DinomorphiaRexterm)) return new List<ClientCard> { domainGY };

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
