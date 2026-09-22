// =========================================================================================
// CARD AUDIT — 2026_Monarch
// | Card Name                     | ID       | Type    | OPT?     | Role / Effect Summary                                   |
// | Erebus the Underworld Monarch | 23064604 | Monster | Soft     | Non-target spin 1 card; GY retrieve 2400/2800 Monarch  |
// | Eidos the Underworld Monarch  | 31596518 | Monster | Hard     | On summon search 2800 Monarch/ST; GY trigger on Tribute |
// | Caius the Shadow Monarch      | 9748752  | Monster | No       | Banish 1 card on field                                  |
// | Zaborg the Thunder Monarch    | 51945556 | Monster | No       | Destroy 1 monster on field                              |
// | Zaborg the Mega Monarch       | 87602890 | Monster | No       | Destroy 1 monster, mill up to 8 from both Extra Decks   |
// | Edea the Heavenly Squire      | 95457011 | Monster | Hard     | SS Squire from Deck; GY recycle banished Monarch ST     |
// | Eidos the Underworld Squire   | 59463312 | Monster | Soft/H   | Extra Tribute Summon; GY banish to SS Squire from GY    |
// | Tessera the Primal Squire     | 67584223 | Monster | Hard     | Hand SS reveal ST; field Tribute; GY SS Squire from Deck|
// | Mulcharmy Fuwalos             | 42141493 | Monster | Hard     | Handtrap: Draw on opponent SS from Deck/Extra Deck      |
// | Ash Blossom & Joyous Spring   | 14558128 | Monster | Hard     | Handtrap: Negate search/dump/SS from Deck               |
// | Nibiru, the Primal Being      | 27204311 | Monster | Hard     | Board Breaker: Tribute all monsters after 5 summons     |
// | Pantheism of the Monarchs     | 22842126 | Spell   | Soft/H   | Hand draw 2; GY banish reveal 3 Monarch STs             |
// | The Monarchs Revolt           | 9283801  | Spell   | Hard     | Discard 1, reveal 3 monsters; GY banish SS Squire       |
// | Tenacity of the Monarchs      | 33609262 | Spell   | Hard     | Reveal Monarch in hand to search Monarch ST             |
// | The Monarchs Masterplan       | 63899196 | Spell   | Hard(rem)| Send Monarch ST from Deck; Banish: search/summon 2400   |
// | The Monarchs Stormforth       | 79844764 | Spell   | Hard     | Tribute 1 opponent monster for Tribute Summon           |
// | Called by the Grave           | 24224830 | Spell   | No       | Quick-Play: Banish & negate opponent GY monster         |
// | The Prime Monarch             | 54241725 | Trap    | Hard(sh) | S/T shuffle 2 draw 1; GY banish ST to SS as 2400 DEF    |
// | Jurrac Astero                 | 52553102 | Extra   | No       | GY Quick: banish self+Jurrac to SS Meteor from Extra    |
// | Jurrac Meteor                 | 17548456 | Extra   | Mandatory| On SS: Destroy all cards on the field (Board Wipe)      |
// | The Duke of Demise            | 45445571 | Extra   | Hard     | GY ignition: banish self to retrieve Erebus/Caius to hand|
// | Elder Entity N'tss            | 80532587 | Extra   | Mandatory| Sent to GY: Target 1 card on field; destroy it          |
// | Garura, Wings of Resonant Life| 11765832 | Extra   | Mandatory| Sent to GY: Draw 1 card                                 |
// | PSY-Framelord Omega           | 74586817 | Extra   | No       | GY ignition: shuffle self + 1 GY card to Deck           |
// | Lunalight Perfume Dancer      | 81196066 | Extra   | Hard     | GY ignition: banish self to debuff opponent ATK by DEF  |
// =========================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_Monarch", "2026_Monarch")]
    public class _2026_MonarchExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Deck Monsters
            public const int ErebusTheUnderworldMonarch = 23064604;
            public const int EidosTheUnderworldMonarch = 31596518;
            public const int CaiusTheShadowMonarch = 9748752;
            public const int ZaborgTheThunderMonarch = 51945556;
            public const int ZaborgTheMegaMonarch = 87602890;
            public const int EdeaTheHeavenlySquire = 95457011;
            public const int EidosTheUnderworldSquire = 59463312;
            public const int TesseraThePrimalSquire = 67584223;
            public const int MulcharmyFuwalos = 42141493;
            public const int AshBlossomAndJoyousSpring = 14558128; // Audited ID (fixed typo 14558127 -> 14558128)
            public const int NibiruThePrimalBeing = 27204311;

            // Spells & Traps
            public const int PantheismOfTheMonarchs = 22842126;
            public const int TheMonarchsRevolt = 9283801;
            public const int TenacityOfTheMonarchs = 33609262;
            public const int TheMonarchsMasterplan = 63899196;
            public const int TheMonarchsStormforth = 79844764;
            public const int CalledByTheGrave = 24224830;
            public const int ThePrimeMonarch = 54241725;

            // Extra Deck (Zaborg Mega Mill / Board Wipe Engine)
            public const int JurracAstero = 52553102;
            public const int JurracMeteor = 17548456;
            public const int JurracVelphito = 65961683;
            public const int PSYFramelordOmega = 74586817;
            public const int Linkuriboh = 41999284;
            public const int GaruraWingsOfResonantLife = 11765832;
            public const int MillenniumEyesRestrict = 41578483;
            public const int LunalightPerfumeDancer = 81196066;
            public const int TheDukeOfDemise = 45445571;
            public const int ElderEntityNtss = 80532587;
        }

        private static readonly int[] AceCardIds =
        {
            CardId.ErebusTheUnderworldMonarch,
            CardId.ZaborgTheMegaMonarch,
            CardId.CaiusTheShadowMonarch,
            CardId.EidosTheUnderworldMonarch,
            CardId.ZaborgTheThunderMonarch
        };

        // Once-Per-Turn (OPT) State Trackers
        private bool _edeaSummonUsed = false;
        private bool _edeaGyUsed = false;
        private bool _eidosGyUsed = false;
        private bool _tesseraHandSsUsed = false;
        private bool _tesseraFieldTributeUsed = false;
        private bool _tesseraGyUsed = false;
        private bool _eidosMonarchSummonUsed = false;
        private bool _eidosMonarchGyUsed = false;
        private bool _pantheismHandUsed = false;
        private bool _pantheismGyUsed = false;
        private bool _revoltHandUsed = false;
        private bool _revoltGyUsed = false;
        private bool _tenacityUsed = false;
        private bool _masterplanBanishUsed = false;
        private bool _primeShuffleUsed = false;
        private bool _stormforthActivatedThisTurn = false;
        private bool _dukeGyUsed = false;
        private bool _perfumeDancerGyUsed = false;

        public _2026_MonarchExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            HeuristicGuard.RegisterAceCards(AceCardIds);

            // Combo Router: Sequencing
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Mega-Zaborg-Extra-Wipe",
                RequiredCards = new List<int> { CardId.EdeaTheHeavenlySquire, CardId.ZaborgTheMegaMonarch },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.EdeaTheHeavenlySquire, ActionType = ExecutorType.Summon, Description = "Normal Summon Edea" },
                    new() { CardId = CardId.EdeaTheHeavenlySquire, ActionType = ExecutorType.Activate, Description = "Edea SS Eidos from Deck" },
                    new() { CardId = CardId.ZaborgTheMegaMonarch, ActionType = ExecutorType.Summon, Description = "Tribute Summon Mega Zaborg" },
                    new() { CardId = CardId.ZaborgTheMegaMonarch, ActionType = ExecutorType.Activate, Description = "Zaborg trigger: destroy & mill 8 Extra Deck cards" }
                },
                EndBoardScore = 95
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Erebus-Control",
                RequiredCards = new List<int> { CardId.EdeaTheHeavenlySquire, CardId.ErebusTheUnderworldMonarch },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.EdeaTheHeavenlySquire, ActionType = ExecutorType.Summon, Description = "Normal Summon Edea" },
                    new() { CardId = CardId.EdeaTheHeavenlySquire, ActionType = ExecutorType.Activate, Description = "Edea SS Eidos from Deck" },
                    new() { CardId = CardId.ErebusTheUnderworldMonarch, ActionType = ExecutorType.Summon, Description = "Tribute Summon Erebus" },
                    new() { CardId = CardId.ErebusTheUnderworldMonarch, ActionType = ExecutorType.Activate, Description = "Erebus send 2 S/Ts to spin opponent card" }
                },
                EndBoardScore = 85
            });

            // Bait Planner & Chain Advisor
            BaitPlanner.RegisterComboStarters(CardId.EdeaTheHeavenlySquire, CardId.TesseraThePrimalSquire, CardId.TheMonarchsRevolt);
            BaitPlanner.RegisterBaitCards(CardId.TenacityOfTheMonarchs, CardId.PantheismOfTheMonarchs);
            ChainAdvisor.RegisterHighValueTargets(CardId.EdeaTheHeavenlySquire, CardId.ZaborgTheMegaMonarch, CardId.ErebusTheUnderworldMonarch);

            // =========================================================================
            // 1. Reactive Disruption & Handtraps (Called by, Ash, Fuwalos, Nibiru)
            // =========================================================================
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAndJoyousSpring, () => SmartHandTrapChain() && DefaultAshBlossomAndJoyousSpring());
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosEffect);
            AddExecutor(ExecutorType.Activate, CardId.NibiruThePrimalBeing, NibiruEffect);

            // =========================================================================
            // 2. GY Quick Disruptions (Opponent's Turn Board Wipe)
            // =========================================================================
            AddExecutor(ExecutorType.Activate, CardId.JurracAstero, JurracAsteroGyQuickEffect);

            // =========================================================================
            // 3. Setup Spells & Starters (Masterplan, Pantheism, Tenacity, Revolt, Stormforth)
            // =========================================================================
            AddExecutor(ExecutorType.Activate, CardId.TheMonarchsStormforth, StormforthEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheMonarchsMasterplan, MasterplanEffect);
            AddExecutor(ExecutorType.Activate, CardId.PantheismOfTheMonarchs, PantheismEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheMonarchsRevolt, RevoltEffect);
            AddExecutor(ExecutorType.Activate, CardId.TenacityOfTheMonarchs, TenacityEffect);

            // =========================================================================
            // 4. Squires & Tribute Fodder Summoning
            // =========================================================================
            // Tessera Hand Special Summon
            AddExecutor(ExecutorType.SpSummon, CardId.TesseraThePrimalSquire, TesseraHandSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TesseraThePrimalSquire, TesseraEffect);

            // Edea Normal Summon & Field/GY Trigger
            AddExecutor(ExecutorType.Summon, CardId.EdeaTheHeavenlySquire, EdeaNormalSummon);
            AddExecutor(ExecutorType.Activate, CardId.EdeaTheHeavenlySquire, EdeaEffect);

            // Eidos Normal Summon & GY Effect
            AddExecutor(ExecutorType.Summon, CardId.EidosTheUnderworldSquire, EidosNormalSummon);
            AddExecutor(ExecutorType.Activate, CardId.EidosTheUnderworldSquire, EidosGyEffect);

            // The Prime Monarch GY Special Summon
            AddExecutor(ExecutorType.Activate, CardId.ThePrimeMonarch, PrimeGyEffect);

            // Eidos the Underworld Monarch GY Trigger on Tribute Summon
            AddExecutor(ExecutorType.Activate, CardId.EidosTheUnderworldMonarch, EidosMonarchEffect);

            // Linkuriboh (if Edea on field and need to trigger Edea GY to recover banished S/T)
            AddExecutor(ExecutorType.SpSummon, CardId.Linkuriboh, LinkuribohSummon);

            // =========================================================================
            // 5. Monarch Tribute Summons
            // =========================================================================
            AddExecutor(ExecutorType.Summon, CardId.ZaborgTheMegaMonarch, ZaborgMegaSummon);
            AddExecutor(ExecutorType.Summon, CardId.ErebusTheUnderworldMonarch, ErebusSummon);
            AddExecutor(ExecutorType.Summon, CardId.CaiusTheShadowMonarch, CaiusSummon);
            AddExecutor(ExecutorType.Summon, CardId.EidosTheUnderworldMonarch, EidosMonarchSummon);
            AddExecutor(ExecutorType.Summon, CardId.ZaborgTheThunderMonarch, ZaborgSummon);

            // =========================================================================
            // 6. Monarch Trigger & Ignition Effects
            // =========================================================================
            AddExecutor(ExecutorType.Activate, CardId.ZaborgTheMegaMonarch, ZaborgMegaEffect);
            AddExecutor(ExecutorType.Activate, CardId.ErebusTheUnderworldMonarch, ErebusEffect);
            AddExecutor(ExecutorType.Activate, CardId.CaiusTheShadowMonarch, CaiusEffect);
            AddExecutor(ExecutorType.Activate, CardId.ZaborgTheThunderMonarch, ZaborgEffect);

            // =========================================================================
            // 7. Extra Deck Mill Triggers & GY Recovery
            // =========================================================================
            AddExecutor(ExecutorType.Activate, CardId.GaruraWingsOfResonantLife);
            AddExecutor(ExecutorType.Activate, CardId.ElderEntityNtss, NtssGyEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheDukeOfDemise, DukeGyEffect);
            AddExecutor(ExecutorType.Activate, CardId.LunalightPerfumeDancer, PerfumeDancerGyEffect);
            AddExecutor(ExecutorType.Activate, CardId.PSYFramelordOmega, OmegaGyEffect);

            // =========================================================================
            // 8. Traps, Continuous Effects, & End Phase Sets
            // =========================================================================
            AddExecutor(ExecutorType.Activate, CardId.ThePrimeMonarch, PrimeShuffleEffect);
            AddExecutor(ExecutorType.SpellSet, CardId.ThePrimeMonarch, SpellSetCheck);
            AddExecutor(ExecutorType.SpellSet, CardId.TheMonarchsStormforth, SpellSetCheck);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, SpellSetCheck);

            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // Prefer going first to execute Zaborg Mega Extra Deck mill & setup Jurrac board wipe
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);

            // Reset turn-based OPT trackers
            _edeaSummonUsed = false;
            _edeaGyUsed = false;
            _eidosGyUsed = false;
            _tesseraHandSsUsed = false;
            _tesseraFieldTributeUsed = false;
            _tesseraGyUsed = false;
            _eidosMonarchSummonUsed = false;
            _eidosMonarchGyUsed = false;
            _pantheismHandUsed = false;
            _pantheismGyUsed = false;
            _revoltHandUsed = false;
            _revoltGyUsed = false;
            _tenacityUsed = false;
            _masterplanBanishUsed = false;
            _primeShuffleUsed = false;
            _stormforthActivatedThisTurn = false;
            _dukeGyUsed = false;
            _perfumeDancerGyUsed = false;
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.IsCode(AceCardIds);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (c.Controller == 1) return 0; // Highest priority: tribute opponent monsters via Stormforth
            if (c.IsCode(CardId.EidosTheUnderworldSquire)) return 10;
            if (c.IsCode(CardId.EdeaTheHeavenlySquire)) return 20;
            if (c.IsCode(CardId.TesseraThePrimalSquire)) return 30;
            if (c.IsCode(CardId.ThePrimeMonarch)) return 40;
            if (IsAceCard(c)) return 900;
            return 100;
        }

        // =========================================================================
        // Helper Queries
        // =========================================================================

        private bool IsMonarchST(int id)
        {
            return id == CardId.PantheismOfTheMonarchs ||
                   id == CardId.TheMonarchsRevolt ||
                   id == CardId.TenacityOfTheMonarchs ||
                   id == CardId.TheMonarchsMasterplan ||
                   id == CardId.TheMonarchsStormforth ||
                   id == CardId.ThePrimeMonarch;
        }

        private bool IsMonarchStats(ClientCard card)
        {
            if (card == null) return false;
            return (card.Attack == 2400 && card.Defense == 1000) ||
                   (card.Attack == 2800 && card.Defense == 1000);
        }

        private bool HasMonarchInHand()
        {
            return Bot.Hand.Any(c => c != null && c.IsMonster() && IsMonarchStats(c));
        }

        private int GetAvailableTributes()
        {
            int count = Bot.GetMonsterCount();
            if (_stormforthActivatedThisTurn && Enemy.GetMonsterCount() > 0)
            {
                count += 1;
            }
            return count;
        }

        private bool HasTributeSummonedMonsterOnField()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsSpecialSummoned && (c.Level >= 5 || IsMonarchStats(c)));
        }

        // =========================================================================
        // 1. Reactive Disruption & Handtraps
        // =========================================================================

        private bool CalledByTheGraveEffect()
        {
            return DefaultCalledByTheGrave();
        }

        private bool MulcharmyFuwalosEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (Bot.GetMonsterCount() > 0 || Bot.GetSpellCount() > 0) return false;
            return Duel.Player == 1;
        }

        private bool NibiruEffect()
        {
            return DefaultNibiru();
        }

        // =========================================================================
        // 2. GY Quick Disruptions (Jurrac Astero Board Wipe)
        // =========================================================================

        private bool JurracAsteroGyQuickEffect()
        {
            // Astero Quick-Effect: During opponent's turn, banish self + 1 Jurrac from GY -> SS Jurrac Meteor from Extra Deck -> Board Wipe!
            if (Duel.Player != 1) return false;
            if (Card.Location != CardLocation.Grave) return false;

            // Check if Extra Deck has Jurrac Meteor
            bool hasMeteorInExtra = Bot.ExtraDeck.Any(c => c.IsCode(CardId.JurracMeteor));
            if (!hasMeteorInExtra) return false;

            // Check if there is another Jurrac in GY to banish as cost
            bool hasOtherJurracInGy = Bot.Graveyard.Any(c => c != Card &&
                c.IsCode(CardId.JurracAstero, CardId.JurracVelphito, CardId.JurracMeteor));
            if (!hasOtherJurracInGy) return false;

            // Trigger when opponent has committed cards to the field
            int enemyFieldCards = Enemy.GetMonsterCount() + Enemy.GetSpellCount();
            if (enemyFieldCards < 2 && Duel.Phase != DuelPhase.BattleStart && Duel.Phase != DuelPhase.Battle)
            {
                return false;
            }

            // Avoid wiping our own high-value field unless necessary
            if (Bot.GetMonsterCount() >= 3 && enemyFieldCards < 3)
            {
                return false;
            }

            return true;
        }

        // =========================================================================
        // 3. Setup Spells
        // =========================================================================

        private bool StormforthEffect()
        {
            if (_stormforthActivatedThisTurn) return false;
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1)
            {
                if (Enemy.GetMonsterCount() > 0 && HasMonarchInHand())
                {
                    _stormforthActivatedThisTurn = true;
                    return true;
                }
            }
            return false;
        }

        private bool MasterplanEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Send 1 Monarch S/T from Deck to GY (primes Pantheism or The Prime Monarch)
                return true;
            }
            if (Card.Location == CardLocation.Removed)
            {
                if (_masterplanBanishUsed) return false;
                var oppMonster = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup());
                if (oppMonster != null)
                {
                    _masterplanBanishUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool PantheismEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_pantheismHandUsed) return false;
                var targets = Bot.Hand.Where(c => c != Card && IsMonarchST(c.Id)).ToList();
                if (targets.Count > 0)
                {
                    // Prioritize discarding The Prime Monarch or Masterplan
                    var cost = targets.FirstOrDefault(c => c.IsCode(CardId.ThePrimeMonarch))
                        ?? targets.FirstOrDefault(c => c.IsCode(CardId.TheMonarchsMasterplan))
                        ?? targets.FirstOrDefault();
                    if (cost != null)
                    {
                        AI.SelectCard(cost);
                        _pantheismHandUsed = true;
                        return true;
                    }
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (_pantheismGyUsed) return false;
                _pantheismGyUsed = true;
                return true;
            }
            return false;
        }

        private bool RevoltEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_revoltHandUsed) return false;
                // Cost: discard 1 card. Needs at least 1 other card in hand.
                var discardCandidates = Bot.Hand.Where(c => c != Card).ToList();
                if (discardCandidates.Count == 0) return false;

                var cost = discardCandidates.FirstOrDefault(c => c.IsCode(CardId.ThePrimeMonarch))
                    ?? discardCandidates.FirstOrDefault(c => c.IsCode(CardId.TheMonarchsMasterplan))
                    ?? discardCandidates.FirstOrDefault(c => c.IsCode(CardId.PantheismOfTheMonarchs))
                    ?? discardCandidates.FirstOrDefault();

                if (cost != null)
                {
                    AI.SelectCard(cost);
                    _revoltHandUsed = true;
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (_revoltGyUsed) return false;
                // Banish self from GY: Special Summon 1 800/1000 monster from hand
                bool hasSquireInHand = Bot.Hand.Any(c => c.IsMonster() && c.Attack == 800 && c.Defense == 1000);
                if (hasSquireInHand && !IsSpecialSummonBlocked())
                {
                    _revoltGyUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool TenacityEffect()
        {
            if (_tenacityUsed) return false;
            var revealTarget = Bot.Hand.FirstOrDefault(c => c.IsMonster() && IsMonarchStats(c));
            if (revealTarget != null)
            {
                AI.SelectCard(revealTarget);
                _tenacityUsed = true;
                return true;
            }
            return false;
        }

        // =========================================================================
        // 4. Squires & Tribute Fodder Summoning
        // =========================================================================

        private bool TesseraHandSpSummon()
        {
            if (_tesseraHandSsUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            // Cost: Reveal 1 Monarch Spell/Trap from hand
            var stInHand = Bot.Hand.FirstOrDefault(c => IsMonarchST(c.Id));
            if (stInHand != null)
            {
                AI.SelectCard(stInHand);
                _tesseraHandSsUsed = true;
                return true;
            }
            return false;
        }

        private bool TesseraEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_tesseraFieldTributeUsed) return false;
                // Field ignition: Immediately Tribute Summon 1 monster from hand
                if (HasMonarchInHand())
                {
                    _tesseraFieldTributeUsed = true;
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (_tesseraGyUsed) return false;
                // Trigger when sent to GY: SS 1 Squire (800/1000) from Deck
                _tesseraGyUsed = true;
                return true;
            }
            return false;
        }

        private bool EdeaNormalSummon()
        {
            // Always prefer normal summoning Edea to tutor Eidos
            return true;
        }

        private bool EdeaEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_edeaSummonUsed) return false;
                // SS Eidos the Underworld Squire from Deck
                AI.SelectCard(CardId.EidosTheUnderworldSquire, CardId.TesseraThePrimalSquire);
                _edeaSummonUsed = true;
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (_edeaGyUsed) return false;
                // Recycle banished Monarch S/T
                var banished = Bot.Banished.Where(c => IsMonarchST(c.Id)).ToList();
                if (banished.Count > 0)
                {
                    var target = banished.FirstOrDefault(c => c.IsCode(CardId.PantheismOfTheMonarchs))
                        ?? banished.FirstOrDefault(c => c.IsCode(CardId.TheMonarchsStormforth))
                        ?? banished.FirstOrDefault(c => c.IsCode(CardId.TheMonarchsMasterplan))
                        ?? banished.FirstOrDefault();
                    if (target != null)
                    {
                        AI.SelectCard(target);
                        _edeaGyUsed = true;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool EidosNormalSummon()
        {
            // If Edea is in hand, summon Edea first instead
            if (Bot.Hand.Any(c => c.IsCode(CardId.EdeaTheHeavenlySquire) && c != Card))
            {
                return false;
            }
            return true;
        }

        private bool EidosGyEffect()
        {
            if (_eidosGyUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            var target = Bot.Graveyard.FirstOrDefault(c => c.IsCode(CardId.EdeaTheHeavenlySquire))
                ?? Bot.Graveyard.FirstOrDefault(c => c.IsCode(CardId.TesseraThePrimalSquire));
            if (target != null)
            {
                AI.SelectCard(target);
                _eidosGyUsed = true;
                return true;
            }
            return false;
        }

        private bool PrimeGyEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            // Prime Monarch in GY: banish 1 Monarch S/T to SS as monster
            var cost = Bot.Graveyard.Where(c => c != Card && IsMonarchST(c.Id)).ToList();
            if (cost.Count > 0)
            {
                // Prioritize banishing Masterplan (to trigger Masterplan's banish search!)
                var target = cost.FirstOrDefault(c => c.IsCode(CardId.TheMonarchsMasterplan))
                    ?? cost.FirstOrDefault(c => c.IsCode(CardId.TheMonarchsRevolt))
                    ?? cost.FirstOrDefault(c => c.IsCode(CardId.TheMonarchsStormforth))
                    ?? cost.FirstOrDefault(c => c.IsCode(CardId.ThePrimeMonarch))
                    ?? cost.FirstOrDefault();

                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool LinkuribohSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            // Only summon Linkuriboh using Edea if we have a banished Monarch S/T to recover
            bool hasBanishedST = Bot.Banished.Any(c => IsMonarchST(c.Id));
            if (!hasBanishedST) return false;

            var edea = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.EdeaTheHeavenlySquire));
            if (edea != null)
            {
                AI.SelectCard(edea);
                return true;
            }
            return false;
        }

        // =========================================================================
        // 5. Monarch Tribute Summons
        // =========================================================================

        private bool ZaborgMegaSummon()
        {
            int tributesNeeded = HasTributeSummonedMonsterOnField() ? 1 : 2;
            return GetAvailableTributes() >= tributesNeeded;
        }

        private bool ErebusSummon()
        {
            int tributesNeeded = HasTributeSummonedMonsterOnField() ? 1 : 2;
            return GetAvailableTributes() >= tributesNeeded;
        }

        private bool CaiusSummon()
        {
            return GetAvailableTributes() >= 1 && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0);
        }

        private bool EidosMonarchSummon()
        {
            return GetAvailableTributes() >= 1;
        }

        private bool ZaborgSummon()
        {
            return GetAvailableTributes() >= 1 && Enemy.GetMonsterCount() > 0;
        }

        // =========================================================================
        // 6. Monarch Trigger & Ignition Effects
        // =========================================================================

        private bool ZaborgMegaEffect()
        {
            // Zaborg Mega: destroy 1 monster on field, then both players send from Extra Deck
            // Priority 1: Target opponent's monster if any
            var oppTarget = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup());
            if (oppTarget != null)
            {
                AI.SelectCard(oppTarget);
                return true;
            }

            // Priority 2: On Turn 1 (or opponent has no monsters), target itself!
            // Zaborg is Level 8 LIGHT, destroying itself triggers 8-card Extra Deck mill from both players
            var itself = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.ZaborgTheMegaMonarch));
            if (itself != null)
            {
                AI.SelectCard(itself);
                return true;
            }

            return false;
        }

        private bool ErebusEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // GY Quick Effect: Discard 1 Monarch S/T, retrieve 2400/2800 Monarch from GY
                var discard = Bot.Hand.FirstOrDefault(c => IsMonarchST(c.Id));
                if (discard != null)
                {
                    var target = Bot.Graveyard.FirstOrDefault(c => c.IsMonster() && IsMonarchStats(c) && c != Card);
                    if (target != null)
                    {
                        AI.SelectCard(discard);
                        AI.SelectNextCard(target);
                        return true;
                    }
                }
            }
            else
            {
                // On-field Tribute Summon trigger: Send 2 Monarch S/Ts to spin 1 card
                AI.SelectCard(CardId.ThePrimeMonarch, CardId.PantheismOfTheMonarchs,
                    CardId.TheMonarchsMasterplan, CardId.TheMonarchsStormforth, CardId.TheMonarchsRevolt);

                var oppTarget = Util.GetBestEnemyCard(canBeTarget: false);
                if (oppTarget != null)
                {
                    AI.SelectNextCard(oppTarget);
                }
                return true;
            }
            return false;
        }

        private bool CaiusEffect()
        {
            var target = Util.GetBestEnemyCard(canBeTarget: true);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool ZaborgEffect()
        {
            var target = Enemy.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget())
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool EidosMonarchEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_eidosMonarchSummonUsed) return false;
                // On summon: Add 1 Monarch S/T or 2800/1000 Monarch from Deck or GY to hand
                _eidosMonarchSummonUsed = true;
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (_eidosMonarchGyUsed) return false;
                // When a Monarch is Tribute Summoned: triggers to SS itself to field
                if (!IsSpecialSummonBlocked() && Bot.GetMonsterCount() < 5)
                {
                    _eidosMonarchGyUsed = true;
                    return true;
                }
            }
            return false;
        }

        // =========================================================================
        // 7. Extra Deck Mill Triggers & GY Recovery
        // =========================================================================

        private bool NtssGyEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                var target = Util.GetBestEnemyCard(canBeTarget: true);
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool DukeGyEffect()
        {
            if (_dukeGyUsed) return false;
            if (Card.Location != CardLocation.Grave) return false;
            // Banish self from GY, target 1 Level 4+ Fiend/Zombie in GY (Erebus or Caius) to retrieve to hand
            var target = Bot.Graveyard.FirstOrDefault(c => c.IsCode(CardId.ErebusTheUnderworldMonarch))
                ?? Bot.Graveyard.FirstOrDefault(c => c.IsCode(CardId.CaiusTheShadowMonarch));
            if (target != null)
            {
                AI.SelectCard(target);
                _dukeGyUsed = true;
                return true;
            }
            return false;
        }

        private bool PerfumeDancerGyEffect()
        {
            if (_perfumeDancerGyUsed) return false;
            if (Card.Location != CardLocation.Grave) return false;
            // Banish self to lower opponent monsters' ATK by their base DEF
            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Battle) && Enemy.GetMonsterCount() > 0)
            {
                _perfumeDancerGyUsed = true;
                return true;
            }
            return false;
        }

        private bool OmegaGyEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // Recycle Pantheism into Deck, or disrupt opponent Extra monster in GY
                var target = Bot.Graveyard.FirstOrDefault(c => c.IsCode(CardId.PantheismOfTheMonarchs))
                    ?? Enemy.Graveyard.FirstOrDefault(c => c.IsMonster() && c.IsExtraCard());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        // =========================================================================
        // 8. Traps, Sets & Callbacks
        // =========================================================================

        private bool PrimeShuffleEffect()
        {
            if (_primeShuffleUsed) return false;
            var targets = Bot.Graveyard.Where(c => IsMonarchST(c.Id)).ToList();
            if (targets.Count >= 2)
            {
                // Do not shuffle Pantheism if Pantheism GY hasn't been used yet!
                var validTargets = targets.Where(c => !c.IsCode(CardId.PantheismOfTheMonarchs) || _pantheismGyUsed).ToList();
                if (validTargets.Count >= 2)
                {
                    AI.SelectCard(validTargets.Take(2).ToList());
                    _primeShuffleUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool SpellSetCheck()
        {
            if (Card.IsTrap()) return true;
            if (Card.IsCode(CardId.CalledByTheGrave) && Duel.Player == 0) return true;
            return false;
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Low-stat starters & handtraps MUST be summoned in Defense Position
            if (cardId == CardId.EdeaTheHeavenlySquire ||
                cardId == CardId.EidosTheUnderworldSquire ||
                cardId == CardId.TesseraThePrimalSquire ||
                cardId == CardId.MulcharmyFuwalos ||
                cardId == CardId.AshBlossomAndJoyousSpring ||
                cardId == CardId.ThePrimeMonarch)
            {
                if (positions.Contains(CardPosition.FaceUpDefence))
                {
                    return CardPosition.FaceUpDefence;
                }
            }

            // High-ATK Monarchs in Attack Position
            if (cardId == CardId.ErebusTheUnderworldMonarch ||
                cardId == CardId.ZaborgTheMegaMonarch ||
                cardId == CardId.CaiusTheShadowMonarch ||
                cardId == CardId.EidosTheUnderworldMonarch)
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                {
                    return CardPosition.FaceUpAttack;
                }
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
            // Prefer middle columns (Zone 2, then 1, 3) to stay safe from column hazard locks
            int[] preferredZones = { 0x4, 0x2, 0x8, 0x1, 0x10 };
            foreach (int zone in preferredZones)
            {
                if ((available & zone) != 0)
                {
                    return zone;
                }
            }
            return base.OnSelectPlace(cardId, player, location, available);
        }

        // =========================================================================
        // OCGCore Hint & Selection Engine
        // =========================================================================

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            ClientCard lastChain = Util.GetLastChainCard();

            // 1. Zaborg the Mega Monarch Extra Deck mill selection
            if (lastChain != null && lastChain.IsCode(CardId.ZaborgTheMegaMonarch) && cards.Any(c => c.Location == CardLocation.Extra))
            {
                List<ClientCard> selected = new List<ClientCard>();
                var ourExtra = cards.Where(c => c.Controller == 0 && c.Id != 0).ToList();

                if (ourExtra.Count > 0)
                {
                    // Precision 8-card dump order:
                    // 1. Garura (draw 1)
                    // 2. N'tss (destroy 1 card)
                    // 3. The Duke of Demise (GY ignition retrieve Erebus/Caius)
                    // 4. PSY-Framelord Omega (GY recycle)
                    // 5. Jurrac Astero (GY Quick Effect setup)
                    // 6. Jurrac Velphito (dino cost fodder for Astero)
                    // 7. Lunalight Perfume Dancer (GY ATK debuff)
                    // 8. Millennium-Eyes Restrict / Extra Astero
                    // CRITICAL: NEVER select all 3 Jurrac Meteor (17548456)! Meteor MUST stay in Extra Deck!
                    var priorityList = new List<int>
                    {
                        CardId.GaruraWingsOfResonantLife,
                        CardId.ElderEntityNtss,
                        CardId.TheDukeOfDemise,
                        CardId.PSYFramelordOmega,
                        CardId.JurracAstero,
                        CardId.JurracVelphito,
                        CardId.LunalightPerfumeDancer,
                        CardId.MillenniumEyesRestrict
                    };

                    foreach (int id in priorityList)
                    {
                        var match = ourExtra.FirstOrDefault(c => c.IsCode(id) && !selected.Contains(c));
                        if (match != null)
                        {
                            selected.Add(match);
                            if (selected.Count >= max) break;
                        }
                    }

                    // Fallback for remaining slots: select any non-Meteor card first
                    if (selected.Count < min)
                    {
                        foreach (var c in ourExtra)
                        {
                            if (!c.IsCode(CardId.JurracMeteor) && !selected.Contains(c))
                            {
                                selected.Add(c);
                                if (selected.Count >= min) break;
                            }
                        }
                    }

                    return selected;
                }

                // Opponent's Extra Deck cards (when we get to choose, e.g. LIGHT tributed)
                // If cards are visible, rip their most impactful combo cards; if face-down (ID == 0), pick first available
                foreach (var c in cards)
                {
                    selected.Add(c);
                    if (selected.Count >= max) break;
                }
                return selected;
            }

            // 2. Erebus S/T dump cost (Hint 504 HINTMSG_TOGRAVE)
            if (lastChain != null && lastChain.IsCode(CardId.ErebusTheUnderworldMonarch)
                && cards.Any(c => c.Location == CardLocation.Deck || c.Location == CardLocation.Hand))
            {
                List<ClientCard> selected = new List<ClientCard>();
                var target1 = cards.FirstOrDefault(c => c.IsCode(CardId.ThePrimeMonarch));
                var target2 = cards.FirstOrDefault(c => c.IsCode(CardId.PantheismOfTheMonarchs));
                var target3 = cards.FirstOrDefault(c => c.IsCode(CardId.TheMonarchsMasterplan));

                if (target1 != null && selected.Count < max) selected.Add(target1);
                if (target2 != null && selected.Count < max && !selected.Contains(target2)) selected.Add(target2);
                if (target3 != null && selected.Count < max && !selected.Contains(target3)) selected.Add(target3);

                foreach (var c in cards)
                {
                    if (selected.Count >= max) break;
                    if (!selected.Contains(c)) selected.Add(c);
                }
                return selected;
            }

            // 3. The Monarchs Masterplan deck dump (Hint 504 HINTMSG_TOGRAVE)
            if (lastChain != null && lastChain.IsCode(CardId.TheMonarchsMasterplan) && hint == 504)
            {
                var target = cards.FirstOrDefault(c => c.IsCode(CardId.PantheismOfTheMonarchs))
                    ?? cards.FirstOrDefault(c => c.IsCode(CardId.ThePrimeMonarch))
                    ?? cards.FirstOrDefault(c => c.IsCode(CardId.TheMonarchsStormforth))
                    ?? cards.FirstOrDefault();
                if (target != null)
                {
                    return new List<ClientCard> { target };
                }
            }

            // 4. The Monarchs Revolt reveal 3 monsters (Hint 526 HINTMSG_CONFIRM / Search)
            if (lastChain != null && lastChain.IsCode(CardId.TheMonarchsRevolt) && cards.Any(c => c.Location == CardLocation.Deck))
            {
                List<ClientCard> selected = new List<ClientCard>();
                var targetErebus = cards.FirstOrDefault(c => c.IsCode(CardId.ErebusTheUnderworldMonarch));
                var targetEdea = cards.FirstOrDefault(c => c.IsCode(CardId.EdeaTheHeavenlySquire));
                var targetEidos = cards.FirstOrDefault(c => c.IsCode(CardId.EidosTheUnderworldSquire));
                var targetZaborg = cards.FirstOrDefault(c => c.IsCode(CardId.ZaborgTheMegaMonarch));

                if (targetErebus != null) selected.Add(targetErebus);
                if (targetEdea != null && !selected.Contains(targetEdea)) selected.Add(targetEdea);
                if (targetEidos != null && !selected.Contains(targetEidos)) selected.Add(targetEidos);
                if (targetZaborg != null && selected.Count < max && !selected.Contains(targetZaborg)) selected.Add(targetZaborg);

                foreach (var c in cards)
                {
                    if (selected.Count >= max) break;
                    if (!selected.Contains(c)) selected.Add(c);
                }
                return selected;
            }

            // 5. Hint 500 (HINTMSG_RELEASE) & Hint 531 (HINTMSG_TRIBUTE) — Tribute Fodder Selection
            if (hint == 500 || hint == 531)
            {
                var sortedTribute = cards.OrderBy(c =>
                {
                    if (c == null) return 999;
                    if (c.Controller == 1) return 0; // Tribute opponent's monster via Stormforth first!
                    if (c.IsCode(CardId.EidosTheUnderworldSquire)) return 10;
                    if (c.IsCode(CardId.EdeaTheHeavenlySquire)) return 20;
                    if (c.IsCode(CardId.TesseraThePrimalSquire)) return 30;
                    if (c.IsCode(CardId.ThePrimeMonarch)) return 40;
                    if (IsAceCard(c)) return 900; // Never tribute our Ace cards unless forced
                    return 100;
                }).ToList();
                return sortedTribute.Take(max).ToList();
            }

            // 6. Hint 501 (HINTMSG_DISCARD) — Discard Selection
            if (hint == 501)
            {
                var sortedDiscard = cards.OrderBy(c =>
                {
                    if (c == null) return 999;
                    if (c.IsCode(CardId.ThePrimeMonarch)) return 10; // Shuffles or summons from GY
                    if (c.IsCode(CardId.TheMonarchsMasterplan)) return 20;
                    if (c.IsCode(CardId.PantheismOfTheMonarchs)) return 30; // Triggers in GY
                    if (c.IsCode(CardId.TheMonarchsRevolt)) return 40; // Triggers in GY
                    if (c.IsCode(CardId.ErebusTheUnderworldMonarch)) return 50; // GY retrieval effect
                    if (IsAceCard(c)) return 900;
                    return 100;
                }).ToList();
                return sortedDiscard.Take(max).ToList();
            }

            // 7. Hint 503 (HINTMSG_REMOVE) — Banish Target Selection
            if (hint == 503)
            {
                // If banishing from opponent field (e.g. Caius)
                var oppCards = cards.Where(c => c.Controller == 1).ToList();
                if (oppCards.Count > 0)
                {
                    var sortedOpp = oppCards.OrderByDescending(c =>
                    {
                        if (c.IsMonster()) return c.Attack;
                        if (c.IsSpell() || c.IsTrap()) return c.IsFaceup() ? 2000 : 500;
                        return 0;
                    }).ToList();
                    return sortedOpp.Take(max).ToList();
                }

                // If banishing from our GY as cost (e.g. The Prime Monarch)
                var ourGyCards = cards.Where(c => c.Controller == 0 && c.Location == CardLocation.Grave).ToList();
                if (ourGyCards.Count > 0)
                {
                    var sortedCost = ourGyCards.OrderBy(c =>
                    {
                        if (c.IsCode(CardId.TheMonarchsMasterplan)) return 10; // Triggers Masterplan search!
                        if (c.IsCode(CardId.TheMonarchsRevolt)) return 20;
                        if (c.IsCode(CardId.TheMonarchsStormforth)) return 30;
                        if (c.IsCode(CardId.ThePrimeMonarch)) return 40;
                        if (c.IsCode(CardId.PantheismOfTheMonarchs)) return 800; // Preserve Pantheism GY search
                        return 100;
                    }).ToList();
                    return sortedCost.Take(max).ToList();
                }
            }

            // 8. Hint 506 (HINTMSG_ATOHAND) — Search Selection
            if (hint == 506 || cards.Any(c => c.Location == CardLocation.Deck))
            {
                var spells = cards.Where(c => IsMonarchST(c.Id)).ToList();
                if (spells.Count > 0)
                {
                    var target = spells.FirstOrDefault(c => c.IsCode(CardId.PantheismOfTheMonarchs))
                        ?? spells.FirstOrDefault(c => c.IsCode(CardId.TheMonarchsStormforth))
                        ?? spells.FirstOrDefault(c => c.IsCode(CardId.ThePrimeMonarch))
                        ?? spells.FirstOrDefault(c => c.IsCode(CardId.TheMonarchsMasterplan))
                        ?? spells.FirstOrDefault();
                    if (target != null)
                    {
                        return new List<ClientCard> { target };
                    }
                }

                var monsters = cards.Where(c => c.IsMonster()).ToList();
                if (monsters.Count > 0)
                {
                    var target = monsters.FirstOrDefault(c => c.IsCode(CardId.ZaborgTheMegaMonarch))
                        ?? monsters.FirstOrDefault(c => c.IsCode(CardId.ErebusTheUnderworldMonarch))
                        ?? monsters.FirstOrDefault(c => c.IsCode(CardId.EdeaTheHeavenlySquire))
                        ?? monsters.FirstOrDefault(c => c.IsCode(CardId.EidosTheUnderworldSquire))
                        ?? monsters.FirstOrDefault();
                    if (target != null)
                    {
                        return new List<ClientCard> { target };
                    }
                }
            }

            // 9. Hint 507 (HINTMSG_TODECK) — Erebus Spin / Omega Recycle
            if (hint == 507)
            {
                var oppCards = cards.Where(c => c.Controller == 1).ToList();
                if (oppCards.Count > 0)
                {
                    var sortedOpp = oppCards.OrderByDescending(c =>
                    {
                        if (c.IsMonster()) return c.Attack;
                        if (c.IsSpell() || c.IsTrap()) return c.IsFaceup() ? 2500 : 1000;
                        return 0;
                    }).ToList();
                    return sortedOpp.Take(max).ToList();
                }

                var ourCards = cards.Where(c => c.Controller == 0).ToList();
                if (ourCards.Count > 0)
                {
                    var target = ourCards.FirstOrDefault(c => c.IsCode(CardId.PantheismOfTheMonarchs))
                        ?? ourCards.FirstOrDefault(c => c.IsCode(CardId.ThePrimeMonarch))
                        ?? ourCards.FirstOrDefault();
                    if (target != null)
                    {
                        return new List<ClientCard> { target };
                    }
                }
            }

            // 10. Hint 509 (HINTMSG_SPSUMMON) — Special Summon Target Selection
            if (hint == 509)
            {
                var sortedSs = cards.OrderBy(c =>
                {
                    if (c == null) return 999;
                    if (c.IsCode(CardId.JurracMeteor)) return 5;
                    if (c.IsCode(CardId.EidosTheUnderworldSquire)) return 10;
                    if (c.IsCode(CardId.EdeaTheHeavenlySquire)) return 20;
                    if (c.IsCode(CardId.TesseraThePrimalSquire)) return 30;
                    if (c.IsCode(CardId.ThePrimeMonarch)) return 40;
                    if (c.IsCode(CardId.ErebusTheUnderworldMonarch)) return 50;
                    return 100;
                }).ToList();
                return sortedSs.Take(max).ToList();
            }

            // 11. Generic Opponent Target Selection
            var enemyCards = cards.Where(c => c.Controller == 1).ToList();
            if (enemyCards.Count >= min)
            {
                var sortedEnemy = enemyCards.OrderByDescending(c =>
                {
                    if (c.IsMonster()) return c.Attack;
                    if (c.IsSpell() || c.IsTrap()) return c.IsFaceup() ? 2000 : 500;
                    return 0;
                }).ToList();
                return sortedEnemy.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
