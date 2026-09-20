// =========================================================================================
// CARD AUDIT โ€” 2026_Monarch
// | Card Name                    | Type    | OPT? | Cost              | Effect                                       | Activate When             | NEVER When                |
// | Erebus the Underworld Monarch| Monster | Yes  | Send 2 Monarch S/T| Shuffles 1 card from hand/field/GY into deck | On Tribute Summon         | Opponent has negate setup |
// | Zaborg the Mega Monarch      | Monster | No   | None              | Destroys 1 monster, mills Extra Deck         | On Tribute Summon         | Opponent has negate setup |
// | Caius the Shadow Monarch     | Monster | No   | None              | Banishes 1 card on the field                 | On Tribute Summon         | Opponent has negate setup |
// | Edea the Heavenly Squire     | Monster | Yes  | None              | SS 1 Squire from deck on summon              | In Hand to start combo    | GY effect has no targets  |
// ACE CARDS: Primary: Erebus the Underworld Monarch / Secondary: Zaborg the Mega Monarch
// COMBO STARTERS: 1. Edea the Heavenly Squire 2. Eidos the Underworld Squire
// CHOKEPOINTS: Edea summon negated by handtraps
// WIN CONDITION: Control and restrict opponent's extra deck via Zaborg/Erebus
// GOING 1ST END BOARD: Erebus / Zaborg on field + Domain of the True Monarchs locking Extra Deck
// GOING 2ND GAMEPLAN: Banish threat via Caius / Erebus, then attack for high damage
// =========================================================================================

// =========================================================================================
// COMBO DRAFT โ€” 2026_Monarch
// =========================================================================================
// === COMBO LINE 1: Standard Tribute Setup (Starter: Edea) ===
// HAND REQUIRED: Edea the Heavenly Squire + any Tribute Monarch
// STEP 1: Normal Summon Edea the Heavenly Squire
// STEP 2: Edea Effect: Special Summon Eidos the Underworld Squire from deck
// STEP 3: Eidos Effect: Grant additional Tribute Summon
// STEP 4: Tribute Edea + Eidos โ’ Tribute Summon Erebus / Zaborg
// END BOARD: Erebus/Zaborg on field locking opponent
// === COMBO LINE 2: Extra Deck Lockout ===
// STEP 1: Set Domain of the True Monarchs
// STEP 2: Tribute summon using opponent's monsters via Stormforth
// =========================================================================================

using System;
using YGOSharp.OCGWrapper.Enums;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_Monarch", "2026_Monarch")]
    public class _2026_MonarchExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck
            public const int ErebusTheUnderworldMonarch = 23064604;
            public const int EidosTheUnderworldMonarch = 31596518;
            public const int CaiusTheShadowMonarch = 9748752;
            public const int ZaborgTheThunderMonarch = 51945556;
            public const int ZaborgTheMegaMonarch = 87602890;
            public const int EdeaTheHeavenlySquire = 95457011;
            public const int EidosTheUnderworldSquire = 59463312;
            public const int TesseraThePrimalSquire = 67584223;
            public const int MulcharmyFuwalos = 42141493;
            public const int AshBlossomAndJoyousSpring = 14558127;
            public const int NibiruThePrimalBeing = 27204311;
            public const int PantheismOfTheMonarchs = 22842126;
            public const int TheMonarchsRevolt = 9283801;
            public const int TenacityOfTheMonarchs = 33609262;
            public const int TheMonarchsMasterplan = 63899196;
            public const int TheMonarchsStormforth = 79844764;
            public const int CalledByTheGrave = 24224830;
            public const int ThePrimeMonarch = 54241725;

            // Extra Deck
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

        private static readonly int[] AceCardIds = {
            CardId.ErebusTheUnderworldMonarch,
            CardId.ZaborgTheMegaMonarch,
            CardId.CaiusTheShadowMonarch,
            CardId.ZaborgTheThunderMonarch
        };

        // Once per turn (OPT) trackers
        private bool _edeaSummonedUsed = false;
        private bool _edeaGyUsed = false;
        private bool _eidosGyUsed = false;
        private bool _tesseraSsUsed = false;
        private bool _tesseraTributeUsed = false;
        private bool _tesseraGyUsed = false;
        private bool _pantheismActivated = false;
        private bool _pantheismGyUsed = false;
        private bool _revoltActivated = false;
        private bool _revoltGyUsed = false;
        private bool _tenacityActivated = false;
        private bool _masterplanActivated = false;
        private bool _masterplanBanishUsed = false;
        private bool _primeShuffleUsed = false;
        private bool _primeGyUsed = false;
        private bool _stormforthActivatedThisTurn = false;

        public _2026_MonarchExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            HeuristicGuard.RegisterAceCards(AceCardIds);
            // โ”€โ”€ Combo Router: Sequencing โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Standard-Setup",
                RequiredCards = new List<int> { CardId.EdeaTheHeavenlySquire, CardId.CaiusTheShadowMonarch },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.EdeaTheHeavenlySquire, ActionType = ExecutorType.Activate, Description = "Play CardId.EdeaTheHeavenlySquire" },
                    new() { CardId = CardId.CaiusTheShadowMonarch, ActionType = ExecutorType.Activate, Description = "Extend with CardId.CaiusTheShadowMonarch" }
                },
                EndBoardScore = 80
            });

            // โ”€โ”€ Bait Planner โ”€โ”€
            BaitPlanner.RegisterComboStarters(CardId.EdeaTheHeavenlySquire, CardId.ErebusTheUnderworldMonarch, CardId.EidosTheUnderworldMonarch);
            BaitPlanner.RegisterBaitCards(CardId.ErebusTheUnderworldMonarch);

            // โ”€โ”€ Chain Advisor โ”€โ”€
            ChainAdvisor.RegisterHighValueTargets(CardId.EdeaTheHeavenlySquire);


            // 1. Hand Traps & Reactive Disruption (Called by, Ash, Mulcharmy, Nibiru)
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAndJoyousSpring, () => SmartHandTrapChain() && DefaultAshBlossomAndJoyousSpring());
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosEffect);
            AddExecutor(ExecutorType.Activate, CardId.NibiruThePrimalBeing, NibiruEffect);

            // 2. Setup Spells (Pantheism, Tenacity, Masterplan, Revolt, Stormforth)
            AddExecutor(ExecutorType.Activate, CardId.PantheismOfTheMonarchs, PantheismHandEffect);
            AddExecutor(ExecutorType.Activate, CardId.TenacityOfTheMonarchs, TenacityEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheMonarchsMasterplan, MasterplanEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheMonarchsRevolt, RevoltEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheMonarchsStormforth, StormforthEffect);

            // 3. Squire Summoning & Field/GY Triggers
            AddExecutor(ExecutorType.SpSummon, CardId.TesseraThePrimalSquire, TesseraSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TesseraThePrimalSquire, TesseraEffect);
            AddExecutor(ExecutorType.Summon, CardId.EdeaTheHeavenlySquire);
            AddExecutor(ExecutorType.Activate, CardId.EdeaTheHeavenlySquire, EdeaEffect);
            AddExecutor(ExecutorType.Summon, CardId.EidosTheUnderworldSquire, EidosSummon);
            AddExecutor(ExecutorType.Activate, CardId.EidosTheUnderworldSquire, EidosGyEffect);
            AddExecutor(ExecutorType.Activate, CardId.ThePrimeMonarch, PrimeGyEffect);

            // 4. Monarch Tribute Summons
            AddExecutor(ExecutorType.Summon, CardId.ZaborgTheMegaMonarch, ZaborgMegaSummon);
            AddExecutor(ExecutorType.Summon, CardId.ErebusTheUnderworldMonarch, ErebusSummon);
            AddExecutor(ExecutorType.Summon, CardId.CaiusTheShadowMonarch, CaiusSummon);
            AddExecutor(ExecutorType.Summon, CardId.ZaborgTheThunderMonarch, ZaborgSummon);
            AddExecutor(ExecutorType.Summon, CardId.EidosTheUnderworldMonarch, EidosMonarchSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.EidosTheUnderworldMonarch, EidosMonarchSpSummon);

            // 5. Monarch Ignition/Trigger Effects
            AddExecutor(ExecutorType.Activate, CardId.ZaborgTheMegaMonarch, ZaborgMegaEffect);
            AddExecutor(ExecutorType.Activate, CardId.ErebusTheUnderworldMonarch, ErebusEffect);
            AddExecutor(ExecutorType.Activate, CardId.CaiusTheShadowMonarch, CaiusEffect);
            AddExecutor(ExecutorType.Activate, CardId.ZaborgTheThunderMonarch, ZaborgEffect);
            AddExecutor(ExecutorType.Activate, CardId.EidosTheUnderworldMonarch, EidosMonarchEffect);

            // 6. Extra Deck Summon (Linkuriboh to trigger Edea GY effect)
            AddExecutor(ExecutorType.SpSummon, CardId.Linkuriboh, LinkuribohSummon);

            // 7. Extra Deck GY Triggers (triggered from Zaborg Extra Deck mill)
            AddExecutor(ExecutorType.Activate, CardId.GaruraWingsOfResonantLife);
            AddExecutor(ExecutorType.Activate, CardId.ElderEntityNtss, NtssGyEffect);
            AddExecutor(ExecutorType.Activate, CardId.LunalightPerfumeDancer, PerfumeDancerGyEffect);
            AddExecutor(ExecutorType.Activate, CardId.PSYFramelordOmega, OmegaGyEffect);

            // 8. Traps and Sets
            AddExecutor(ExecutorType.Activate, CardId.ThePrimeMonarch, PrimeShuffleEffect);
            AddExecutor(ExecutorType.SpellSet, CardId.ThePrimeMonarch, SpellSetCheck);
            AddExecutor(ExecutorType.SpellSet, CardId.TheMonarchsStormforth, SpellSetCheck);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, SpellSetCheck);
            AddExecutor(ExecutorType.SpellSet, CardId.TheMonarchsRevolt, SpellSetCheck);

            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // Prefer going first to set up domain/squire combos
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _edeaSummonedUsed = false;
            _edeaGyUsed = false;
            _eidosGyUsed = false;
            _tesseraSsUsed = false;
            _tesseraTributeUsed = false;
            _tesseraGyUsed = false;
            _pantheismActivated = false;
            _pantheismGyUsed = false;
            _revoltActivated = false;
            _revoltGyUsed = false;
            _tenacityActivated = false;
            _masterplanActivated = false;
            _masterplanBanishUsed = false;
            _primeShuffleUsed = false;
            _primeGyUsed = false;
            _stormforthActivatedThisTurn = false;

            // โ”€โ”€ Going-Second BreakBoard: prioritize disruption over combo โ”€โ”€
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
                _stormforthActivatedThisTurn = false;
            }
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.IsCode(
                CardId.ErebusTheUnderworldMonarch,
                CardId.ZaborgTheMegaMonarch,
                CardId.CaiusTheShadowMonarch
            );
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            return 100;
        }

        protected override bool IsBoardStrongEnough()
        {
            if (BoardScore() >= 15) return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (BoardScore() >= 20) return true;
            return base.ShouldStopExtending();
        }

        // --- Core Helper Check Methods ---

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

        private int GetAvailableTributes()
        {
            int count = Bot.GetMonsterCount();
            if (_stormforthActivatedThisTurn && Enemy.GetMonsterCount() > 0)
            {
                count += 1;
            }
            return count;
        }

        // --- Hand Traps & Reactive Disruption ---

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

        // --- Setup Spells ---

        private bool PantheismHandEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.Hand)
            {
                if (_pantheismActivated) return false;
                var targets = Bot.Hand.Where(c => c != Card && IsMonarchST(c.Id)).ToList();
                if (targets.Count > 0)
                {
                    var cost = targets.FirstOrDefault(c => c.IsCode(CardId.ThePrimeMonarch)) ?? targets.FirstOrDefault();
                    if (cost != null)
                    {
                        AI.SelectCard(cost);
                        _pantheismActivated = true;
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

        private bool TenacityEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_tenacityActivated) return false;
            var revealTarget = Bot.Hand.FirstOrDefault(c => c.IsMonster() && IsMonarchStats(c));
            if (revealTarget != null)
            {
                AI.SelectCard(revealTarget);
                _tenacityActivated = true;
                return true;
            }
            return false;
        }

        private bool MasterplanEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_masterplanActivated) return false;
                _masterplanActivated = true;
                return true;
            }
            else if (Card.Location == CardLocation.Removed)
            {
                if (_masterplanBanishUsed) return false;
                var target = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup());
                if (target != null)
                {
                    AI.SelectCard(target);
                    _masterplanBanishUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool RevoltEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_revoltActivated) return false;
                // Destroy a face-up Monarch card we control (prioritizing The Prime Monarch)
                var destroyTarget = Bot.GetMonsters().Concat(Bot.GetSpells())
                    .Where(c => c != null && c.IsFaceup() && (c.IsCode(CardId.ThePrimeMonarch) || IsMonarchST(c.Id) || IsMonarchStats(c)))
                    .OrderBy(c => c.IsCode(CardId.ThePrimeMonarch) ? 0 : 1)
                    .FirstOrDefault();
                if (destroyTarget != null)
                {
                    AI.SelectCard(destroyTarget);
                    _revoltActivated = true;
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (_revoltGyUsed) return false;
                bool hasSquire = Bot.Hand.Any(c => c.IsMonster() && c.Attack == 800 && c.Defense == 1000);
                if (hasSquire && !IsSpecialSummonBlocked())
                {
                    _revoltGyUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool StormforthEffect()
        {
            if (_stormforthActivatedThisTurn) return false;
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1)
            {
                if (Enemy.GetMonsterCount() > 0)
                {
                    // Check if we have a Monarch in hand OR an extra tribute summon available from squires
                    bool hasMonarchInHand = Bot.Hand.Any(c => c.IsMonster() && IsMonarchStats(c));
                    bool hasExtraTributeFromSquire = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() &&
                        c.IsCode(CardId.EidosTheUnderworldSquire, CardId.TesseraThePrimalSquire));
                    if (hasMonarchInHand || hasExtraTributeFromSquire)
                    {
                        _stormforthActivatedThisTurn = true;
                        return true;
                    }
                }
            }
            return false;
        }

        // --- Squire Summoning & Effects ---

        private bool TesseraSpSummon()
        {
            if (_tesseraSsUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            bool hasMonarch = Bot.Hand.Any(c => c.IsMonster() && IsMonarchStats(c));
            if (!hasMonarch) return false;

            var target = Bot.Hand.FirstOrDefault(c => IsMonarchST(c.Id));
            if (target != null)
            {
                AI.SelectCard(target);
                _tesseraSsUsed = true;
                return true;
            }
            return false;
        }

        private bool TesseraEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_tesseraTributeUsed) return false;
                bool canTributeSummon = Bot.Hand.Any(c => c.IsMonster() && IsMonarchStats(c));
                if (canTributeSummon)
                {
                    _tesseraTributeUsed = true;
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (_tesseraGyUsed) return false;
                _tesseraGyUsed = true;
                return true;
            }
            return false;
        }

        private bool EdeaEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_edeaSummonedUsed) return false;
                AI.SelectCard(CardId.EidosTheUnderworldSquire, CardId.TesseraThePrimalSquire);
                _edeaSummonedUsed = true;
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (_edeaGyUsed) return false;
                var banished = Bot.Banished.Where(c => IsMonarchST(c.Id)).ToList();
                if (banished.Count > 0)
                {
                    var target = banished.FirstOrDefault(c => c.IsCode(CardId.PantheismOfTheMonarchs))
                        ?? banished.FirstOrDefault(c => c.IsCode(CardId.TheMonarchsStormforth))
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

        private bool EidosSummon()
        {
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
            if (_primeGyUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            var cost = Bot.Graveyard.Where(c => c != Card && IsMonarchST(c.Id)).ToList();
            if (cost.Count > 0)
            {
                AI.SelectCard(cost.FirstOrDefault());
                _primeGyUsed = true;
                return true;
            }
            return false;
        }

        // --- Monarch Summoning Logic ---

        private bool ZaborgMegaSummon()
        {
            int tributesNeeded = 2;
            bool hasTributeSummoned = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsSpecialSummoned && (c.Level >= 5 || IsMonarchStats(c)));
            if (hasTributeSummoned)
            {
                tributesNeeded = 1;
            }
            return GetAvailableTributes() >= tributesNeeded;
        }

        private bool ErebusSummon()
        {
            int tributesNeeded = 2;
            bool hasTributeSummoned = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsSpecialSummoned && (c.Level >= 5 || IsMonarchStats(c)));
            if (hasTributeSummoned)
            {
                tributesNeeded = 1;
            }
            return GetAvailableTributes() >= tributesNeeded;
        }

        private bool CaiusSummon()
        {
            return GetAvailableTributes() >= 1 && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0);
        }

        private bool ZaborgSummon()
        {
            return GetAvailableTributes() >= 1 && Enemy.GetMonsterCount() > 0;
        }

        private bool EidosMonarchSummon()
        {
            int tributesNeeded = 2;
            bool hasTributeSummoned = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsSpecialSummoned && (c.Level >= 5 || IsMonarchStats(c)));
            if (hasTributeSummoned)
            {
                tributesNeeded = 1;
            }
            return GetAvailableTributes() >= tributesNeeded;
        }

        // --- Monarch Activation Effects ---

        private bool ZaborgMegaEffect()
        {
            // Zaborg Mega: destroy 1 monster, then both players send from Extra Deck to GY
            // Priority: target opponent's monster
            var oppTarget = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup());
            if (oppTarget != null)
            {
                AI.SelectCard(oppTarget);
                return true;
            }
            // Self-target ONLY going first Turn 1 (for Extra Deck mill value)
            // Going second, self-destroying wastes our 2-tribute investment
            if (Duel.Turn <= 1)
            {
                var itself = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.ZaborgTheMegaMonarch));
                if (itself != null)
                {
                    AI.SelectCard(itself);
                    return true;
                }
            }
            return false;
        }

        private bool ErebusEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                var discard = Bot.Hand.FirstOrDefault(c => IsMonarchST(c.Id));
                if (discard != null)
                {
                    var target = Bot.Graveyard.FirstOrDefault(c => c.IsMonster() && IsMonarchStats(c));
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
                // On-field trigger: send 2 Monarch S/T from hand/deck to GY, then shuffle 1 card
                // from opponent's hand/field/GY into the deck
                var sendTargets = Bot.Hand.Where(c => c != null && c != Card && IsMonarchST(c.Id)).ToList();
                if (sendTargets.Count < 2)
                {
                    // Not enough Monarch S/T in hand โ€” still activate, the game will let us pick from deck
                }
                AI.SelectCard(CardId.ThePrimeMonarch, CardId.PantheismOfTheMonarchs,
                    CardId.TenacityOfTheMonarchs, CardId.TheMonarchsStormforth, CardId.TheMonarchsRevolt);

                // Select best opponent card to shuffle back into deck
                var oppTarget = Util.GetBestEnemyCard(canBeTarget: true);
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
            // Zaborg (small): destroy 1 monster on field
            // NEVER activate if no enemy monster โ€” would self-destruct
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

        private bool EidosMonarchSpSummon()
        {
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                bool hasBanish = Bot.Graveyard.Any(c => IsMonarchST(c.Id));
                if (hasBanish)
                {
                    int tributesNeeded = 2;
                    bool hasTributeSummoned = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsSpecialSummoned && (c.Level >= 5 || IsMonarchStats(c)));
                    if (hasTributeSummoned) tributesNeeded = 1;
                    if (GetAvailableTributes() >= tributesNeeded)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool EidosMonarchEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                var discard = Bot.Hand.FirstOrDefault(c => IsMonarchST(c.Id));
                if (discard != null)
                {
                    AI.SelectCard(discard);
                    return true;
                }
                return false;
            }
            return true;
        }

        // --- Link & Extra Deck GY Effects ---

        private bool LinkuribohSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var edea = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.EdeaTheHeavenlySquire));
            if (edea != null)
            {
                AI.SelectCard(edea);
                return true;
            }
            return false;
        }

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

        private bool PerfumeDancerGyEffect()
        {
            return Card.Location == CardLocation.Grave && Enemy.GetMonsterCount() > 0;
        }

        private bool OmegaGyEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                var target = Enemy.Graveyard.FirstOrDefault(c => c.IsMonster() && c.IsExtraCard())
                    ?? Bot.Graveyard.FirstOrDefault(c => c.IsCode(CardId.PantheismOfTheMonarchs));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        // --- Traps and Sets Check ---

        private bool PrimeShuffleEffect()
        {
            if (_primeShuffleUsed) return false;
            var targets = Bot.Graveyard.Where(c => IsMonarchST(c.Id)).ToList();
            if (targets.Count >= 2)
            {
                AI.SelectCard(targets.Take(2).ToList());
                _primeShuffleUsed = true;
                return true;
            }
            return false;
        }

        private bool SpellSetCheck()
        {
            if (Card.IsTrap()) return true;
            if (Card.IsCode(CardId.CalledByTheGrave) && Duel.Player == 0) return true;
            return false;
        }

        // --- Card Selection Overrides ---

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            ClientCard lastChain = Util.GetLastChainCard();

            // 1. Zaborg the Mega Monarch Extra Deck mill selection
            if (lastChain != null && lastChain.IsCode(CardId.ZaborgTheMegaMonarch) && cards.Any(c => c.Location == CardLocation.Extra))
            {
                List<ClientCard> selected = new List<ClientCard>();
                var ourExtra = cards.Where(c => c.Id != 0).ToList();
                if (ourExtra.Count > 0)
                {
                    var priorityList = new List<int>
                    {
                        CardId.GaruraWingsOfResonantLife,
                        CardId.ElderEntityNtss,
                        CardId.LunalightPerfumeDancer,
                        CardId.PSYFramelordOmega,
                        CardId.JurracAstero,
                        CardId.JurracMeteor,
                        CardId.JurracVelphito,
                        CardId.MillenniumEyesRestrict,
                        CardId.TheDukeOfDemise,
                        CardId.Linkuriboh
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
                    if (selected.Count < min)
                    {
                        foreach (var c in ourExtra)
                        {
                            if (!selected.Contains(c))
                            {
                                selected.Add(c);
                                if (selected.Count >= min) break;
                            }
                        }
                    }
                    return selected;
                }
                
                // For opponent's Extra Deck cards (IDs are 0)
                foreach (var c in cards)
                {
                    selected.Add(c);
                    if (selected.Count >= max) break;
                }
                return selected;
            }

            // 2. Erebus / Ehther spell/trap send cost
            if (lastChain != null && (lastChain.IsCode(CardId.ErebusTheUnderworldMonarch) || lastChain.IsCode(CardId.EidosTheUnderworldMonarch))
                && cards.Any(c => c.Location == CardLocation.Deck || c.Location == CardLocation.Hand))
            {
                List<ClientCard> selected = new List<ClientCard>();
                var target1 = cards.FirstOrDefault(c => c.IsCode(CardId.ThePrimeMonarch));
                var target2 = cards.FirstOrDefault(c => c.IsCode(CardId.PantheismOfTheMonarchs));
                
                if (target1 != null && selected.Count < max) selected.Add(target1);
                if (target2 != null && selected.Count < max) selected.Add(target2);
                
                foreach (var c in cards)
                {
                    if (selected.Count >= max) break;
                    if (!selected.Contains(c))
                    {
                        selected.Add(c);
                    }
                }
                return selected;
            }

            // 2.1 Masterplan banish selection
            if (lastChain != null && lastChain.IsCode(CardId.TheMonarchsMasterplan) && cards.Any(c => c.Location == CardLocation.Grave))
            {
                var target = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (c.IsCode(CardId.EdeaTheHeavenlySquire)) return 10;
                    if (c.IsCode(CardId.EidosTheUnderworldSquire)) return 100;
                    if (IsAceCard(c)) return 500;
                    return 50;
                }).FirstOrDefault();
                if (target != null)
                {
                    return new List<ClientCard> { target };
                }
            }

            // 3. Search target priority (Tenacity, Revolt, Eidos search, Pantheism GY)
            bool isSearch = cards.Any(c => c.Location == CardLocation.Deck);
            if (isSearch)
            {
                if (min > 1)
                {
                    var spells = cards.Where(c => IsMonarchST(c.Id)).ToList();
                    List<ClientCard> selected = new List<ClientCard>();
                    var priorityIds = new[] { CardId.PantheismOfTheMonarchs, CardId.TheMonarchsStormforth, CardId.ThePrimeMonarch };
                    foreach (int id in priorityIds)
                    {
                        var matches = spells.Where(c => c.IsCode(id) && !selected.Contains(c)).ToList();
                        foreach (var match in matches)
                        {
                            if (selected.Count >= max) break;
                            selected.Add(match);
                        }
                        if (selected.Count >= max) break;
                    }
                    foreach (var c in spells)
                    {
                        if (selected.Count >= max) break;
                        if (!selected.Contains(c)) selected.Add(c);
                    }
                    if (selected.Count < min)
                    {
                        foreach (var c in cards)
                        {
                            if (selected.Count >= min) break;
                            if (!selected.Contains(c)) selected.Add(c);
                        }
                    }
                    return selected;
                }
                
                var spellsSingle = cards.Where(c => IsMonarchST(c.Id)).ToList();
                if (spellsSingle.Count > 0)
                {
                    var target = spellsSingle.FirstOrDefault(c => c.IsCode(CardId.PantheismOfTheMonarchs))
                        ?? spellsSingle.FirstOrDefault(c => c.IsCode(CardId.TheMonarchsStormforth))
                        ?? spellsSingle.FirstOrDefault(c => c.IsCode(CardId.ThePrimeMonarch))
                        ?? spellsSingle.FirstOrDefault();
                    if (target != null)
                    {
                        return new List<ClientCard> { target };
                    }
                }
                
                var monsters = cards.Where(c => c.IsMonster()).ToList();
                if (monsters.Count > 0)
                {
                    var target = monsters.FirstOrDefault(c => c.IsCode(CardId.ErebusTheUnderworldMonarch))
                        ?? monsters.FirstOrDefault(c => c.IsCode(CardId.EdeaTheHeavenlySquire))
                        ?? monsters.FirstOrDefault(c => c.IsCode(CardId.EidosTheUnderworldSquire))
                        ?? monsters.FirstOrDefault();
                    if (target != null)
                    {
                        return new List<ClientCard> { target };
                    }
                }
            }

            // 4. General Tribute selection (Tribute summon hint 503)
            if (hint == 503)
            {
                var sortedTribute = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (c.Controller == 1) return 0;
                    if (c.IsCode(CardId.EidosTheUnderworldSquire)) return 10;
                    if (c.IsCode(CardId.EdeaTheHeavenlySquire)) return 20;
                    if (c.IsCode(CardId.TesseraThePrimalSquire)) return 30;
                    if (c.IsCode(CardId.ThePrimeMonarch)) return 40;
                    if (IsAceCard(c)) return 500;
                    return 100;
                }).ToList();
                return sortedTribute.Take(max).ToList();
            }

            // 5. Default opponent target selection
            var oppCards = cards.Where(c => c.Controller == 1).ToList();
            if (oppCards.Count >= min)
            {
                var sortedOpp = oppCards.OrderByDescending(c => {
                    if (c.IsMonster()) return c.Attack;
                    if (c.IsSpell() || c.IsTrap()) return c.IsFaceup() ? 2000 : 100;
                    return 0;
                }).ToList();
                return sortedOpp.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
