using YGOSharp.OCGWrapper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Plugins;

namespace WindBot.Game.AI.Decks
{
    // ====================================================================================================
    //  LABRYNTH EXECUTOR - MODERN RULE-BASED DECISION ENGINE WITH DECOUPLED DOMAIN PLUGIN
    // ====================================================================================================
    //  Archetype: Labrynth Control
    //  Strategy:
    //  1. Continuous Normal Trap Trigger Loops (Lady Labrynth set from deck, Lovely pop & recycle)
    //  2. High-impact reactive interruptions (Karma Cannon, Ice Dragon's Prison, Punishment, D-Barrier)
    //  3. Furniture recursion engine (Stovie Torbie, Chandraglier, Cooclock)
    //  4. Clean Decoupled Domain Plugin integration (LabrynthPlugin)
    // ====================================================================================================

    [Deck("Labrynth", "Labrynth")]
    [Deck("2026_Labrynth", "2026_Labrynth")]
    public class LabrynthExecutor : ModernExecutor
    {
        public class CardId
        {
            // --- MAIN DECK LABRYNTH ARCHETYPE ---
            public const int LadyLabrynthOfTheSilverCastle = 81497285;
            public const int LovelyLabrynthOfTheSilverCastle = 2347656;
            public const int AriasTheLabrynthButler = 73602965;
            public const int AriannaTheLabrynthServant = 1225009;
            public const int ArianeTheLabrynthServant = 75730490;
            public const int LabrynthChandraglier = 37629703;
            public const int LabrynthStovieTorbie = 74018812;
            public const int LabrynthCooclock = 2511;
            public const int BigWelcomeLabrynth = 92714517;
            public const int WelcomeLabrynth = 5380979;

            // --- STAPLES & HIGH-IMPACT NORMAL TRAPS ---
            public const int AshBlossom = 14558127;
            public const int InfiniteImpermanence = 10045474;
            public const int DestructiveDarumaKarmaCannon = 30748475;
            public const int DogmatikaPunishment = 82956214;
            public const int IceDragonsPrison = 20899496;
            public const int TerrorsOfTheOverroot = 63086455;
            public const int TransactionRollback = 6351147;
            public const int DimensionalBarrier = 83326048;
            public const int EradicatorEpidemicVirus = 54974237;
            public const int TheBlackGoatLaughs = 49299410;
            public const int CompulsoryEvacuationDevice = 94192409;
            public const int TrapTrick = 80101899;
            public const int PotOfDuality = 98645731;

            // --- EXTRA DECK CARDS ---
            public const int ElderEntityNtss = 80532587;
            public const int Garura = 11765832;
            public const int GoldenCloudBeastMalong = 93125329;
            public const int MereologicAggregator = 9940036;
            public const int BucephalusII = 10019086;
            public const int ChaosAngel = 22850702;
            public const int SuperStarslayerTYPHON = 93039339;
            public const int MuckrakerFromTheUnderworld = 71607202;
            public const int SPKnight = 29301450;
            public const int UnderworldGoddess = 98127546;
            public const int RelinquishedAnima = 94259633;
            public const int Dharc = 8264361;
            public const int Number60DugaresTheTimeless = 66011101;
            public const int WindPegasusAtIgnister = 98506199;
        }

        // --- OPT & TURN STATE FLAGS ---
        public bool StovieUsed { get; private set; } = false;
        public bool ChandraglierUsed { get; private set; } = false;
        public bool CooclockHandUsed { get; private set; } = false;
        public bool CooclockGraveUsed { get; private set; } = false;
        public bool AriannaUsed { get; private set; } = false;
        public bool AriasHandUsed { get; private set; } = false;
        public bool AriasGraveUsed { get; private set; } = false;
        public bool LadySummonedThisTurn { get; private set; } = false;
        public bool LadySetUsed { get; private set; } = false;
        public bool LovelySetUsed { get; private set; } = false;
        public bool LovelyDestroyUsed { get; private set; } = false;
        public bool NormalSummonedThisTurn { get; private set; } = false;
        public bool SpecialSummonedThisTurn { get; private set; } = false;
        public bool MuckrakerUsed { get; private set; } = false;
        public ClientCard CurrentLastChainCard => LastChainCard;

        // Decoupled Domain Plugin accessor
        public LabrynthPlugin Plugin => DeckPlugin as LabrynthPlugin;

        // --- ACE CARDS (PROTECTED FROM ACCIDENTAL REMOVAL / LINK / TRIBUTE) ---
        private static readonly int[] AceCardIds = {
            CardId.LadyLabrynthOfTheSilverCastle,
            CardId.LovelyLabrynthOfTheSilverCastle,
            CardId.ChaosAngel,
            CardId.SuperStarslayerTYPHON
        };

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Contains(card.Id);
        }

        protected override bool IsBoardStrongEnough()
        {
            if (Bot.HasInMonstersZone(CardId.LadyLabrynthOfTheSilverCastle) && Bot.GetSpellCount() >= 2) return true;
            if (Bot.HasInMonstersZone(CardId.LovelyLabrynthOfTheSilverCastle) && Bot.GetSpellCount() >= 2) return true;
            if (Bot.HasInMonstersZone(CardId.ChaosAngel)) return true;
            return base.IsBoardStrongEnough();
        }

        public LabrynthExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // 🔒 Decoupled Domain Plugin Architecture (MANDATORY)
            DeckPlugin = new LabrynthPlugin(this);

            ResourcePlan.RegisterAceCards(AceCardIds);

            // Combo lines for sequencing
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Arianna-Welcome",
                RequiredCards = new List<int> { CardId.AriannaTheLabrynthServant, CardId.BigWelcomeLabrynth },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.AriannaTheLabrynthServant, ActionType = ExecutorType.Summon, Description = "Normal Summon Arianna" },
                    new() { CardId = CardId.AriannaTheLabrynthServant, ActionType = ExecutorType.Activate, Description = "Arianna search" },
                    new() { CardId = CardId.BigWelcomeLabrynth, ActionType = ExecutorType.SpellSet, Description = "Set Big Welcome" }
                },
                EndBoardScore = 85
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Cooclock-BigWelcome",
                RequiredCards = new List<int> { CardId.LabrynthCooclock, CardId.BigWelcomeLabrynth },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.LabrynthCooclock, ActionType = ExecutorType.Activate, Description = "Cooclock discard" },
                    new() { CardId = CardId.BigWelcomeLabrynth, ActionType = ExecutorType.SpellSet, Description = "Set Big Welcome" },
                    new() { CardId = CardId.BigWelcomeLabrynth, ActionType = ExecutorType.Activate, Description = "Activate Big Welcome immediately" }
                },
                EndBoardScore = 90
            });

            // Bait Planner
            BaitPlanner.RegisterComboStarters(CardId.BigWelcomeLabrynth, CardId.WelcomeLabrynth);
            BaitPlanner.RegisterBaitCards(CardId.AriannaTheLabrynthServant, CardId.PotOfDuality);

            // Chain Advisor
            ChainAdvisor.RegisterHighValueTargets(CardId.BigWelcomeLabrynth, CardId.WelcomeLabrynth, CardId.TransactionRollback);

            // ─────────────────────────────────────────────────────────────────
            //  EXECUTOR ORDER (Tiered Execution Engine)
            // ─────────────────────────────────────────────────────────────────

            // 0. Handtraps / Counter / Immediate Negates
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, InfiniteImpermanenceEffect);

            // 1. Enabling Quick Effects (Cooclock enables same-turn traps)
            AddExecutor(ExecutorType.Activate, CardId.LabrynthCooclock, CooclockEffect);

            // 2. High-Priority Reactive Boss Triggers
            AddExecutor(ExecutorType.Activate, CardId.LadyLabrynthOfTheSilverCastle, LadyEffect);
            AddExecutor(ExecutorType.Activate, CardId.LovelyLabrynthOfTheSilverCastle, LovelyEffect);
            AddExecutor(ExecutorType.Activate, CardId.AriasTheLabrynthButler, AriasEffect);

            // 3. Normal Traps: High Impact Removal & Floodgates
            AddExecutor(ExecutorType.Activate, CardId.DimensionalBarrier, DimensionalBarrierEffect);
            AddExecutor(ExecutorType.Activate, CardId.DestructiveDarumaKarmaCannon, KarmaCannonEffect);
            AddExecutor(ExecutorType.Activate, CardId.IceDragonsPrison, IceDragonsPrisonEffect);
            AddExecutor(ExecutorType.Activate, CardId.DogmatikaPunishment, PunishmentEffect);
            AddExecutor(ExecutorType.Activate, CardId.CompulsoryEvacuationDevice, CompulsoryEffect);
            AddExecutor(ExecutorType.Activate, CardId.TerrorsOfTheOverroot, OverrootEffect);
            AddExecutor(ExecutorType.Activate, CardId.EradicatorEpidemicVirus, EradicatorVirusEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheBlackGoatLaughs, TheBlackGoatLaughsEffect);

            // 4. Labrynth Engine Traps
            AddExecutor(ExecutorType.Activate, CardId.BigWelcomeLabrynth, BigWelcomeEffect);
            AddExecutor(ExecutorType.Activate, CardId.WelcomeLabrynth, WelcomeEffect);
            AddExecutor(ExecutorType.Activate, CardId.TransactionRollback, TransactionRollbackEffect);

            // 5. Furniture Hand/Field activations (Set Traps from Deck)
            AddExecutor(ExecutorType.Activate, CardId.LabrynthStovieTorbie, StovieTorbieEffect);
            AddExecutor(ExecutorType.Activate, CardId.LabrynthChandraglier, ChandraglierEffect);

            // 6. Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.AriannaTheLabrynthServant, AriannaSummon);
            AddExecutor(ExecutorType.Activate, CardId.AriannaTheLabrynthServant, AriannaEffect);
            AddExecutor(ExecutorType.Summon, CardId.AriasTheLabrynthButler, AriasSummon);
            AddExecutor(ExecutorType.Summon, CardId.LabrynthStovieTorbie, FodderSummon);
            AddExecutor(ExecutorType.Summon, CardId.LabrynthChandraglier, FodderSummon);
            AddExecutor(ExecutorType.Summon, CardId.LabrynthCooclock, FodderSummon);

            // 7. Extra Deck Special Summons & Trigger Activations
            AddExecutor(ExecutorType.Activate, CardId.ChaosAngel, ChaosAngelEffect);
            AddExecutor(ExecutorType.Activate, CardId.SuperStarslayerTYPHON, TyPhonEffect);
            AddExecutor(ExecutorType.Activate, CardId.SPKnight, SPKnightEffect);
            AddExecutor(ExecutorType.Activate, CardId.ElderEntityNtss, NtssEffect);
            AddExecutor(ExecutorType.Activate, CardId.GoldenCloudBeastMalong, MalongEffect);
            AddExecutor(ExecutorType.Activate, CardId.Garura, GaruraEffect);
            AddExecutor(ExecutorType.Activate, CardId.MereologicAggregator, AggregatorEffect);
            AddExecutor(ExecutorType.Activate, CardId.WindPegasusAtIgnister, WindPegasusEffect);
            AddExecutor(ExecutorType.Activate, CardId.BucephalusII, BucephalusEffect);
            AddExecutor(ExecutorType.Activate, CardId.MuckrakerFromTheUnderworld, MuckrakerEffect);
            AddExecutor(ExecutorType.Activate, CardId.RelinquishedAnima, RelinquishedAnimaEffect);
            AddExecutor(ExecutorType.Activate, CardId.Dharc, DharcEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.ChaosAngel, ChaosAngelSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SuperStarslayerTYPHON, TyPhonSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SPKnight, SPKnightSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.MuckrakerFromTheUnderworld, MuckrakerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Dharc, DharcSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Number60DugaresTheTimeless, DugaresSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.RelinquishedAnima, RelinquishedAnimaSummon);

            // 8. Spell/Trap Sets (Preserve hand & ready backrow)
            AddExecutor(ExecutorType.SpellSet, CardId.BigWelcomeLabrynth);
            AddExecutor(ExecutorType.SpellSet, CardId.WelcomeLabrynth);
            AddExecutor(ExecutorType.SpellSet, CardId.DestructiveDarumaKarmaCannon);
            AddExecutor(ExecutorType.SpellSet, CardId.DimensionalBarrier);
            AddExecutor(ExecutorType.SpellSet, CardId.DogmatikaPunishment);
            AddExecutor(ExecutorType.SpellSet, CardId.IceDragonsPrison);
            AddExecutor(ExecutorType.SpellSet, CardId.TerrorsOfTheOverroot);
            AddExecutor(ExecutorType.SpellSet, CardId.CompulsoryEvacuationDevice);
            AddExecutor(ExecutorType.SpellSet, CardId.EradicatorEpidemicVirus);
            AddExecutor(ExecutorType.SpellSet, CardId.TheBlackGoatLaughs);
            AddExecutor(ExecutorType.SpellSet, CardId.TransactionRollback, RollbackSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence);

            // 9. Battle & Reposition
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand() => true; // Always choose to go first for Trap setups

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            StovieUsed = false;
            ChandraglierUsed = false;
            CooclockHandUsed = false;
            CooclockGraveUsed = false;
            AriannaUsed = false;
            AriasHandUsed = false;
            AriasGraveUsed = false;
            LadySummonedThisTurn = false;
            LadySetUsed = false;
            LovelySetUsed = false;
            LovelyDestroyUsed = false;
            NormalSummonedThisTurn = false;
            SpecialSummonedThisTurn = false;
            MuckrakerUsed = false;
        }

        public override void OnChaining(int player, ClientCard card)
        {
            base.OnChaining(player, card);
            if (player == 0 && card != null)
            {
                int id = card.Id;
                if (id == CardId.BigWelcomeLabrynth ||
                    id == CardId.WelcomeLabrynth ||
                    id == CardId.AriasTheLabrynthButler ||
                    id == CardId.LadyLabrynthOfTheSilverCastle ||
                    id == CardId.LovelyLabrynthOfTheSilverCastle ||
                    id == CardId.TransactionRollback)
                {
                    SpecialSummonedThisTurn = true;
                }
            }
        }

        public ClientCard CurrentExecutingCard => Card;

        // --- HELPER QUERIES ---

        public bool IsLabrynthMonster(ClientCard c)
        {
            if (c == null) return false;
            return c.Id == CardId.LadyLabrynthOfTheSilverCastle
                || c.Id == CardId.LovelyLabrynthOfTheSilverCastle
                || c.Id == CardId.AriannaTheLabrynthServant
                || c.Id == CardId.ArianeTheLabrynthServant
                || c.Id == CardId.AriasTheLabrynthButler
                || c.Id == CardId.LabrynthStovieTorbie
                || c.Id == CardId.LabrynthChandraglier
                || c.Id == CardId.LabrynthCooclock;
        }

        public bool IsNormalTrap(ClientCard c)
        {
            if (c == null) return false;
            return c.IsTrap() && !c.HasType(CardType.Continuous) && !c.HasType(CardType.Counter);
        }

        private bool HandHasDiscardFodder(int exceptCardId)
        {
            // Do NOT discard high-impact floodgates, aces, or handtraps just for a Furniture set!
            return Bot.Hand.Any(c => c != null && c.Id != exceptCardId && !IsAceCard(c)
                && c.Id != CardId.DestructiveDarumaKarmaCannon
                && c.Id != CardId.DimensionalBarrier
                && c.Id != CardId.EradicatorEpidemicVirus
                && c.Id != CardId.AshBlossom
                && !(c.Id == CardId.AriannaTheLabrynthServant && !AriannaUsed && Duel.Player == 0));
        }

        private bool RollbackSpellSet()
        {
            // Transaction Rollback's field effect ONLY targets opponent's GY Normal Traps!
            // If opponent has no Normal Trap in GY, keeping Rollback in hand is MUCH better because
            // Furniture (Stovie / Chandra) can discard it for free (+1 value in GY!).
            if (Enemy.Graveyard.Any(c => c != null && IsNormalTrap(c))) return true;
            return Bot.Hand.Count >= 6 && Bot.GetSpellCount() < 4;
        }

        // --- MONSTER REPOSITION ---

        private bool MonsterRepos()
        {
            if (Card == null) return false;

            // 1. Heavy Hitters & Aces: FaceUpAttack
            if (IsAceCard(Card) || Card.Attack >= 2500)
            {
                if (Card.IsDefense() && Card.IsFaceup()) return true;
                return false;
            }

            // 2. Utility & 0 ATK / Low ATK Fodder: FaceUpDefence
            if (Card.IsAttack())
            {
                if (Card.Defense >= Card.Attack || Card.Attack < 2000 || CardIntelligence.IsHandtrap(Card.Id))
                {
                    return true;
                }
            }

            return false;
        }

        // --- HANDTRAP & INTERRUPTIONS ---

        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard == null || LastChainCard.Controller != 1) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool InfiniteImpermanenceEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;

            // 1. If this card (or any of our cards) is targeted by an opponent's card (e.g. Phoenix, Duster, MST, Silquitous)
            // Or if opponent activated a monster effect on field anywhere in current chain (break SEGOC chain-block!)
            var faceupMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsDisabled() && 
                !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget()).ToList();

            if (faceupMonsters.Count > 0)
            {
                // If a monster on field activated in current chain, prioritize negating it!
                var chainActor = Duel.CurrentChain?.FirstOrDefault(c => c != null && c.Controller == 1 && c.Location == CardLocation.MonsterZone);
                if (chainActor != null)
                {
                    var targetOnField = faceupMonsters.FirstOrDefault(c => c.IsCode(chainActor.Id));
                    if (targetOnField != null)
                    {
                        AI.SelectCard(targetOnField);
                        return true;
                    }
                }

                // If targeted for removal / destruction, fire immediately on most dangerous enemy monster
                bool isTargeted = Card != null && Card.Location == CardLocation.SpellZone && 
                    (Card.IsShouldNotBeTarget() || (Duel.CurrentChain != null && Duel.CurrentChain.Any(c => c != null && c.Controller == 1)));
                if (isTargeted)
                {
                    var bestTarget = faceupMonsters.OrderByDescending(c => Scorer != null ? Scorer.ThreatScore(c) : c.Attack).FirstOrDefault();
                    if (bestTarget != null)
                    {
                        AI.SelectCard(bestTarget);
                        return true;
                    }
                }
            }

            return DefaultInfiniteImpermanence();
        }

        private bool CooclockEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (CooclockHandUsed) return false;
                // Only use if we control a Labrynth monster or have Arias/Welcome to put one on field,
                // and we have a set Normal Trap ready to activate this turn!
                bool hasLabrynth = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && IsLabrynthMonster(m)) ||
                                   Bot.Hand.Any(c => c != null && (c.Id == CardId.AriasTheLabrynthButler || c.Id == CardId.AriannaTheLabrynthServant));
                bool hasNormalTrapSet = Bot.GetSpells().Any(s => s != null && s.IsFacedown() && IsNormalTrap(s)) ||
                                        Bot.Hand.Any(c => c != null && IsNormalTrap(c));

                if (hasLabrynth && hasNormalTrapSet)
                {
                    CooclockHandUsed = true;
                    return true;
                }
                return false;
            }

            if (Card.Location == CardLocation.Grave)
            {
                if (CooclockGraveUsed) return false;
                CooclockGraveUsed = true;
                return true; // Add to hand or Special Summon
            }

            return false;
        }

        private bool AriasEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                if (AriasGraveUsed) return false;
                if (IsSpecialSummonBlocked()) return false;
                AriasGraveUsed = true;
                return true;
            }

            if (Card.Location == CardLocation.Hand)
            {
                if (AriasHandUsed) return false;
                if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;

                // Check targets: Normal Trap or Labrynth monster
                bool hasTrap = Bot.Hand.Any(c => c != null && c.Id != CardId.AriasTheLabrynthButler && IsNormalTrap(c));
                bool hasMon = Bot.Hand.Any(c => c != null && c.Id != CardId.AriasTheLabrynthButler && IsLabrynthMonster(c));

                if (!hasTrap && !hasMon) return false;

                AriasHandUsed = true;
                return true;
            }

            return false;
        }

        private bool LadyEffect()
        {
            if (ActivateDescription == Util.GetStringId(CardId.LadyLabrynthOfTheSilverCastle, 0))
            {
                // Effect 0: Special Summon from Hand
                if (LadySummonedThisTurn) return false;
                if (IsSpecialSummonBlocked()) return false;
                LadySummonedThisTurn = true;
                return true;
            }

            if (ActivateDescription == Util.GetStringId(CardId.LadyLabrynthOfTheSilverCastle, 1))
            {
                // Effect 1: Set 1 Normal Trap directly from Deck
                if (LadySetUsed) return false;
                LadySetUsed = true;
                return true;
            }

            return true;
        }

        private bool LovelyEffect()
        {
            if (ActivateDescription == Util.GetStringId(CardId.LovelyLabrynthOfTheSilverCastle, 0))
            {
                // Effect 0: Set 1 Normal Trap from Graveyard
                if (LovelySetUsed) return false;
                if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
                bool hasNormalTrapInGrave = Bot.Graveyard.Any(c => c != null && IsNormalTrap(c));
                if (!hasNormalTrapInGrave) return false;
                LovelySetUsed = true;
                return true;
            }

            if (ActivateDescription == Util.GetStringId(CardId.LovelyLabrynthOfTheSilverCastle, 1))
            {
                // Effect 1: Pop 1 card on field or in hand when monster leaves field by Normal Trap
                if (LovelyDestroyUsed) return false;
                // Target Verification Safeguard: only trigger if enemy has cards
                if (Enemy.GetFieldCount() == 0 && Enemy.Hand.Count == 0) return false;
                LovelyDestroyUsed = true;
                return true;
            }

            return true;
        }

        // --- NORMAL TRAPS ---

        private bool DimensionalBarrierEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            // Best used on opponent's turn to shut down Extra Deck summon
            if (Duel.Player != 1) return false;

            int declaredType = DetermineDBarrierType();
            AI.SelectOption(declaredType);
            return true;
        }

        private bool KarmaCannonEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Enemy.GetMonsterCount() == 0) return false;

            // Protect during Battle Phase against direct attacks
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle)) return true;

            // Instantly clear Link monsters (Links cannot be set face-down, sent to GY!)
            bool oppHasLink = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Link));
            if (oppHasLink) return true;

            int faceupEnemyCount = Enemy.GetMonsters().Count(c => c != null && c.IsFaceup());
            if (faceupEnemyCount >= 2) return true;
            if (faceupEnemyCount == 1 && LastChainCard != null && LastChainCard.Controller == 1) return true;

            return false;
        }

        private bool IceDragonsPrisonEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Enemy.Graveyard.Count(c => c.IsMonster()) == 0) return false;
            if (Enemy.GetMonsterCount() == 0) return false;

            // Opponent must have a faceup monster that shares a Race with a monster in their Graveyard
            var oppFieldRaces = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).Select(c => c.Race).ToList();
            bool hasRaceMatch = Enemy.Graveyard.Any(c => c != null && c.IsMonster() && oppFieldRaces.Contains(c.Race));

            return hasRaceMatch;
        }

        private bool PunishmentEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;

            // Target Verification Safeguard: Target must exist on enemy field
            var targets = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() 
                && !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget()).ToList();
            if (targets.Count == 0) return false;

            // Verify Extra Deck has a candidate with ATK >= target's ATK
            // Our ED has: Bucephalus (3500), Aggregator (2600), N'tss (2500), Malong (2200), Garura (1500)
            int maxEDAtk = 3500;
            var validTargets = targets.Where(t => t.Attack <= maxEDAtk).ToList();
            if (validTargets.Count == 0) return false;

            var target = validTargets.OrderByDescending(c => Scorer != null ? Scorer.ThreatScore(c) : c.Attack).FirstOrDefault();
            if (target == null) return false;

            AI.SelectCard(target);
            return true;
        }

        private bool CompulsoryEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && 
                !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget());
            if (target == null) return false;
            AI.SelectCard(target);
            return true;
        }

        private bool OverrootEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Enemy.GetMonsterCount() == 0 && Enemy.GetSpells().Count(c => c.IsFaceup()) == 0) return false;
            if (Enemy.Graveyard.Count == 0) return false;
            return true;
        }

        private bool EradicatorVirusEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            // Requires 1 DARK monster with 2500+ ATK (Lovely 2900 or Lady 3000)
            bool hasTribute = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && 
                (m.Id == CardId.LovelyLabrynthOfTheSilverCastle || m.Id == CardId.LadyLabrynthOfTheSilverCastle));
            return hasTribute;
        }

        private bool TheBlackGoatLaughsEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            int announceId = 0;

            if (Card.Location == CardLocation.Grave)
            {
                // GY effect: Prevents activation of effects of monsters ON THE FIELD with declared name!
                var dangerousOnField = Enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && !c.IsDisabled())
                    .OrderByDescending(c => Scorer != null ? Scorer.ThreatScore(c) : c.Attack)
                    .ThenByDescending(c => c.Attack).FirstOrDefault();
                if (dangerousOnField != null)
                {
                    announceId = dangerousOnField.Id;
                }
                else
                {
                    // No dangerous monsters on enemy field, don't waste GY effect
                    return false;
                }
            }
            else
            {
                // Field activation: Neither player can Special Summon monsters with declared name (except from GY)
                var enemyGYMonsters = Enemy.Graveyard.Where(c => c != null && c.IsMonster()).ToList();
                if (enemyGYMonsters.Count > 0)
                {
                    var target = enemyGYMonsters.OrderByDescending(c => c.IsExtraCard() ? 1000 : 0)
                        .ThenByDescending(c => c.Attack).FirstOrDefault();
                    if (target != null) announceId = target.Id;
                }
                if (announceId == 0)
                {
                    var enemyMon = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup())
                        .OrderByDescending(c => c.Attack).FirstOrDefault();
                    if (enemyMon != null) announceId = enemyMon.Id;
                }
                if (announceId == 0) announceId = CardId.AshBlossom;
            }

            AI.SelectAnnounceID(announceId);
            return true;
        }

        private bool BigWelcomeEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // GY effect: Banish to return 1 Fiend we control to hand, OR if we control Lv8+ Fiend (Lady/Lovely),
                // target 1 enemy card to bounce without bouncing our own!
                bool hasFiend = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Fiend));
                bool oppHasCards = Enemy.GetFieldCount() > 0;
                return hasFiend && oppHasCards;
            }

            // On our turn, activate in Main Phase
            if (Duel.Player == 0 && Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2)
                return false;

            // On opponent's turn: NEVER shotgun in Draw or Standby Phase!
            if (Duel.Player == 1)
            {
                if (Duel.Phase == DuelPhase.Draw || Duel.Phase == DuelPhase.Standby)
                    return false;

                // Chain reactively if opponent activates effect or targets our card
                bool isReactive = (LastChainCard != null && LastChainCard.Controller == 1) || Util.IsChainTarget(Card);
                bool isEndPhase = Duel.Phase == DuelPhase.End;
                bool isBattlePhase = Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle;
                bool oppHasMonsters = Enemy.GetMonsterCount() > 0;

                if (!isReactive && !isEndPhase && !isBattlePhase && !oppHasMonsters)
                    return false;
            }

            return true;
        }

        private bool WelcomeEffect()
        {
            if (Card.Location == CardLocation.Grave) return true;

            if (Duel.Player == 0 && Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2)
                return false;

            // On opponent's turn: NEVER shotgun in Draw or Standby Phase!
            if (Duel.Player == 1)
            {
                if (Duel.Phase == DuelPhase.Draw || Duel.Phase == DuelPhase.Standby)
                    return false;

                bool isReactive = (LastChainCard != null && LastChainCard.Controller == 1) || Util.IsChainTarget(Card);
                bool isEndPhase = Duel.Phase == DuelPhase.End;
                bool isBattlePhase = Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle;
                bool oppHasMonsters = Enemy.GetMonsterCount() > 0;

                if (!isReactive && !isEndPhase && !isBattlePhase && !oppHasMonsters)
                    return false;
            }

            return true;
        }

        private bool TransactionRollbackEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Card.Location == CardLocation.SpellZone)
            {
                return Enemy.Graveyard.Any(c => c != null && IsNormalTrap(c));
            }
            if (Card.Location == CardLocation.Grave)
            {
                return Bot.Graveyard.Any(c => c != null && IsNormalTrap(c) && c.Id != CardId.TransactionRollback);
            }
            return false;
        }

        // --- FURNITURE & HAND ACTIVATIONS ---

        private bool StovieTorbieEffect()
        {
            if (StovieUsed) return false;
            if (Card.Location == CardLocation.Grave)
            {
                if (IsSpecialSummonBlocked()) return false;
                StovieUsed = true;
                SpecialSummonedThisTurn = true;
                return true;
            }

            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.MonsterZone)
            {
                if (EnemyHasKnownNegate()) return false;
                // On our turn: Wait until Main Phase 1 before discarding from hand, to allow Normal Summons (Arianna) first!
                if (Duel.Player == 0 && Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2)
                    return false;

                // On opponent's turn: If we have no Cooclock, activate in End Phase or reactively
                if (Duel.Player == 1 && !Bot.HasInHand(CardId.LabrynthCooclock) && !Bot.HasInMonstersZone(CardId.LabrynthCooclock))
                {
                    bool isEndPhase = Duel.Phase == DuelPhase.End;
                    bool isReactive = (LastChainCard != null && LastChainCard.Controller == 1) || Util.IsChainTarget(Card);
                    if (!isEndPhase && !isReactive) return false;
                }

                if (!HandHasDiscardFodder(CardId.LabrynthStovieTorbie)) return false;
                StovieUsed = true;
                return true;
            }

            return false;
        }

        private bool ChandraglierEffect()
        {
            if (ChandraglierUsed) return false;
            if (Card.Location == CardLocation.Grave)
            {
                ChandraglierUsed = true;
                return true;
            }

            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.MonsterZone)
            {
                if (EnemyHasKnownNegate()) return false;
                // On our turn: Wait until Main Phase 1 before discarding from hand, to allow Normal Summons (Arianna) first!
                if (Duel.Player == 0 && Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2)
                    return false;

                // On opponent's turn: If we have no Cooclock, activate in End Phase or reactively
                if (Duel.Player == 1 && !Bot.HasInHand(CardId.LabrynthCooclock) && !Bot.HasInMonstersZone(CardId.LabrynthCooclock))
                {
                    bool isEndPhase = Duel.Phase == DuelPhase.End;
                    bool isReactive = (LastChainCard != null && LastChainCard.Controller == 1) || Util.IsChainTarget(Card);
                    if (!isEndPhase && !isReactive) return false;
                }

                if (!HandHasDiscardFodder(CardId.LabrynthChandraglier)) return false;
                ChandraglierUsed = true;
                return true;
            }

            return false;
        }

        // --- NORMAL SUMMONS ---

        private bool AriannaSummon()
        {
            if (NormalSummonedThisTurn) return false;
            NormalSummonedThisTurn = true;
            return true;
        }

        private bool AriannaEffect()
        {
            if (AriannaUsed) return false;
            AriannaUsed = true;
            return true;
        }

        private bool AriasSummon()
        {
            if (NormalSummonedThisTurn) return false;
            if (Bot.GetMonsterCount() > 0) return false;
            NormalSummonedThisTurn = true;
            return true;
        }

        private bool FodderSummon()
        {
            if (NormalSummonedThisTurn) return false;
            // Only summon small monsters if we have no other monsters and need board presence
            if (Bot.GetMonsterCount() > 0) return false;
            NormalSummonedThisTurn = true;
            return true;
        }

        // --- EXTRA DECK SUMMONS ---

        private bool ChaosAngelSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            SpecialSummonedThisTurn = true;
            return true;
        }

        private bool TyPhonSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            bool oppHasBigED = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.Attack >= 3000 || c.IsExtraCard()));
            if (!oppHasBigED) return false;
            SpecialSummonedThisTurn = true;
            return true;
        }

        private bool SPKnightSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            if (Enemy.GetMonsterCount() == 0 && Enemy.Graveyard.Count == 0) return false;
            // 🔒 Anti-pattern 5.2: Do not sacrifice Ace monsters (Lady / Lovely)
            int aceOnField = Bot.GetMonsters().Count(IsAceCard);
            if (Bot.GetMonsterCount() - aceOnField < 2) return false;

            // S:P Little Knight's on-summon banish ONLY triggers if an Extra Deck monster was used as material!
            // Do not consume 2 Main Deck Labrynth monsters (e.g. Arianna + Stovie) for a vanilla 1600 ATK Link-2!
            bool hasEDMaterial = Bot.GetMonsters().Any(c => !IsAceCard(c) && (c.IsExtraCard() || c.HasType(CardType.Fusion | CardType.Synchro | CardType.Xyz | CardType.Link)));
            bool isLethalPush = Duel.Phase == DuelPhase.Battle || (Enemy.GetMonsterCount() == 0 && Util.GetTotalAttackingMonsterAttack(0) >= Enemy.LifePoints);
            if (!hasEDMaterial && !isLethalPush) return false;

            SpecialSummonedThisTurn = true;
            return true;
        }

        private bool MuckrakerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.Hand.Count == 0) return false;
            bool hasFiendToRevive = Bot.Graveyard.Any(c => c.IsMonster() && c.IsCanRevive() &&
                (c.Id == CardId.LovelyLabrynthOfTheSilverCastle || c.Id == CardId.LadyLabrynthOfTheSilverCastle));
            if (!hasFiendToRevive) return false;
            int nonAce = Bot.GetMonsters().Count(c => !IsAceCard(c));
            if (nonAce < 2) return false;
            SpecialSummonedThisTurn = true;
            return true;
        }

        private bool DugaresSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            bool hasRevive = Bot.Graveyard.Any(c => c.IsMonster() && c.IsCanRevive() &&
                (c.Id == CardId.LovelyLabrynthOfTheSilverCastle || c.Id == CardId.LadyLabrynthOfTheSilverCastle));
            return hasRevive;
        }

        private bool RelinquishedAnimaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup());
        }

        private bool RelinquishedAnimaEffect()
        {
            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup());
        }

        private bool MuckrakerEffect()
        {
            if (MuckrakerUsed) return false;
            if (Bot.Hand.Count == 0) return false;
            var fiend = Bot.Graveyard.FirstOrDefault(c => c.IsMonster() && c.IsCanRevive() &&
                (c.Id == CardId.LovelyLabrynthOfTheSilverCastle || c.Id == CardId.LadyLabrynthOfTheSilverCastle));
            if (fiend == null) return false;

            AI.SelectCard(fiend);
            MuckrakerUsed = true;
            return true;
        }

        private bool DharcSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            bool enemyHasDark = Enemy.Graveyard.Any(c => c.IsMonster() && c.HasAttribute(CardAttribute.Dark));
            if (!enemyHasDark) return false;
            int nonAce = Bot.GetMonsters().Count(c => !IsAceCard(c));
            if (nonAce < 2) return false;
            SpecialSummonedThisTurn = true;
            return true;
        }

        private bool DharcEffect()
        {
            var target = Enemy.Graveyard.FirstOrDefault(c => c.IsMonster() && c.HasAttribute(CardAttribute.Dark));
            if (target == null) return false;
            AI.SelectCard(target);
            return true;
        }

        // --- EXTRA DECK TRIGGER & ACTIVATION EFFECTS ---

        private bool ChaosAngelEffect()
        {
            // Banish 1 card on the field (Target Verification Safeguard: target enemy card!)
            return Enemy.GetFieldCount() > 0;
        }

        private bool TyPhonEffect()
        {
            // Detach 1 material to bounce 1 faceup monster (Target Verification Safeguard)
            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup());
        }

        private bool SPKnightEffect()
        {
            // Effect 1: Banish 1 card from enemy field or GY
            return Enemy.GetFieldCount() > 0 || Enemy.Graveyard.Count > 0;
        }

        private bool NtssEffect()
        {
            // Target 1 card on the field and destroy it
            return Enemy.GetFieldCount() > 0;
        }

        private bool MalongEffect()
        {
            // Target 1 faceup card your opponent controls; return it to the hand
            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup()) ||
                   Enemy.GetSpells().Any(c => c != null && c.IsFaceup());
        }

        private bool GaruraEffect() => true; // Draw 1 card

        private bool AggregatorEffect()
        {
            // Target 1 face-up card on the field; negate its effects
            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled());
        }

        private bool WindPegasusEffect()
        {
            // Shuffle 1 card opponent controls into Deck
            return Enemy.GetFieldCount() > 0;
        }

        private bool BucephalusEffect() => true; // Send Garura to draw 1 card

        private int DetermineDBarrierType()
        {
            var oppCards = Enemy.GetMonsters().Concat(Enemy.Graveyard).Concat(Enemy.GetSpells()).ToList();

            // 1. Dynamic Board Check FIRST!
            // If opponent actively controls Tuner + Non-Tuner, they are Synchro summoning RIGHT NOW!
            var oppFaceupMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            bool hasTuner = oppFaceupMonsters.Any(c => c.HasType(CardType.Tuner));
            bool hasNonTuner = oppFaceupMonsters.Any(c => !c.HasType(CardType.Tuner) && !c.HasType(CardType.Xyz) && !c.HasType(CardType.Link));

            // Check matching levels for Xyz
            var levelCounts = new Dictionary<int, int>();
            foreach (var m in oppFaceupMonsters)
            {
                if (m.Level > 0 && !m.HasType(CardType.Xyz) && !m.HasType(CardType.Link))
                {
                    levelCounts[m.Level] = levelCounts.GetValueOrDefault(m.Level, 0) + 1;
                }
            }
            bool hasXyzPair = levelCounts.Values.Any(cnt => cnt >= 2) || oppFaceupMonsters.Any(c => c.IsOneForXyz());

            if (hasTuner && hasNonTuner) return 2; // Synchro!

            // 2. Windwitch Check (Windwitch Engine in Dark Magician makes Crystal Wing Synchro Dragon!)
            bool isWindwitch = oppCards.Any(c =>
                c.Id == 43722862 || // Windwitch - Ice Bell
                c.Id == 71007216 || // Windwitch - Glass Bell
                c.Id == 70117860 || // Windwitch - Snow Bell
                c.Id == 21044178 || // Windwitch - Winter Bell
                c.Id == 50954680    // Crystal Wing Synchro Dragon
            );
            if (isWindwitch) return 2; // Synchro!

            // 3. Fusion Archetype Check (ABC, Dark Magician)
            bool isFusionDeck = oppCards.Any(c =>
                c.HasType(CardType.Fusion) ||
                c.Id == 1561110 ||  // ABC-Dragon Buster
                c.Id == 66399653 || // Union Hangar
                c.Id == 77411244 || // B-Buster Drake
                c.Id == 30012506 || // A-Assault Core
                c.Id == 3405259  || // C-Crush Wyvern (CORRECT ID)
                c.Id == 99249638 || // Union Driver
                c.Id == 12524259 || // Unauthorized Reactivation
                c.Id == 39890958 || // Heavy Mech Support Armor
                c.Id == 65367484 || // Photon Thrasher
                c.Id == 89132148 || // Photon Orbital
                c.Id == 46986414 || // Dark Magician
                c.Id == 47222536 || // Dark Magical Circle
                c.Id == 38033121 || // Dark Magical Circle (alt)
                c.Id == 48680970 || // Eternal Soul
                c.Id == 1784686  || // The Eye of Timaeus
                c.Id == 7084129  || // Magician's Rod
                c.Id == 30603688 || // Apprentice Illusion Magician
                c.Id == 7922915  || // Magician Navigation
                c.Id == 41721210 || // Dark Magician the Dragon Knight
                c.Id == 71413901 || // Secrets of Dark Magic
                c.Id == 70828912 || // Magicians' Souls
                c.Id == 24094653    // Polymerization
            );
            if (isFusionDeck) return 1; // 1 = Fusion

            if (hasXyzPair && !hasTuner) return 3; // Xyz

            // 4. BlueEyes Deck check:
            // BlueEyes in WindBot primarily makes Rank 8 Xyz (Dark Matter, Prime Photon, Full Armor, Hope Harbinger, Felgrand)!
            // Only Synchro if they have a Tuner (Sage / White Stone) on field!
            bool isBlueEyes = oppCards.Any(c =>
                c.Id == 89631139 || // Blue-Eyes White Dragon
                c.Id == 38517737 || // Blue-Eyes Alternative White Dragon
                c.Id == 45467446 || // Dragon Spirit of White
                c.Id == 8240199  || // Sage with Eyes of Blue
                c.Id == 71039903 || // The White Stone of Ancients
                c.Id == 79814787 || // The White Stone of Legend
                c.Id == 41620959 || // Dragon Shrine
                c.Id == 39701395 || // Cards of Consonance
                c.Id == 48800175 || // The Melody of Awakening Dragon
                c.Id == 6853254  || // Return of the Dragon Lords
                c.Id == 18591577 || // Return of the Dragon Lords (alt)
                c.Id == 22804644    // Bingo Machine, Go!!!
            );
            if (isBlueEyes)
            {
                if (hasTuner && hasNonTuner) return 2; // Synchro
                return 3; // Xyz!
            }

            // 5. Other Synchro Decks
            bool isSynchroDeck = oppCards.Any(c => c.HasType(CardType.Synchro) || c.HasType(CardType.Tuner));
            if (isSynchroDeck) return 2; // Synchro

            // 6. Other Xyz Decks
            bool isXyzDeck = oppCards.Any(c => c.HasType(CardType.Xyz));
            if (isXyzDeck) return 3; // Xyz

            return 1; // Default to Fusion
        }

        // ─────────────────────────────────────────────────────────────────
        //  CALLBACK OVERRIDES & STRATEGIC ROUTING (via LabrynthPlugin)
        // ─────────────────────────────────────────────────────────────────

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // 1. Extra Deck Sends (Dogmatika Punishment Extra dump / Bucephalus II dump)
            if (cards.Count > 0 && cards.All(c => c != null && c.Location == CardLocation.Extra))
            {
                // If Bucephalus II activated its effect: send Garura to draw 1 card!
                var garuraCard = cards.FirstOrDefault(c => c.Id == CardId.Garura);
                if (LastChainCard != null && LastChainCard.Id == CardId.BucephalusII && garuraCard != null)
                {
                    return new List<ClientCard> { garuraCard };
                }
                bool oppHasCards = Enemy.GetFieldCount() > 0;
                var orderedExtra = cards.OrderByDescending(c => {
                    // Elder Entity N'tss: Destroy 1 card on field
                    if (c.Id == CardId.ElderEntityNtss) return oppHasCards ? 100 : 40;
                    // Malong: Bounce 1 face-up card
                    if (c.Id == CardId.GoldenCloudBeastMalong) return oppHasCards ? 95 : 30;
                    // Garura: Draw 1 card
                    if (c.Id == CardId.Garura) return 90;
                    // Mereologic Aggregator: Negate 1 face-up card
                    if (c.Id == CardId.MereologicAggregator) return 85;
                    // Wind Pegasus @Ignister: Shuffle from GY when card destroyed
                    if (c.Id == CardId.WindPegasusAtIgnister) return 80;
                    // Bucephalus II: Highest ATK beatstick
                    if (c.Id == CardId.BucephalusII) return 70;
                    return 10;
                }).ToList();

                if (orderedExtra.Count >= min) return orderedExtra.Take(max).ToList();
            }

            // 2. Ice Dragon's Prison Field Resolution (Banish 1 card from each field of same race)
            var oppIDP = cards.Where(c => c != null && c.Controller == 1 && c.Location == CardLocation.MonsterZone).ToList();
            var ourIDP = cards.Where(c => c != null && c.Controller == 0 && c.Location == CardLocation.MonsterZone).ToList();
            if (oppIDP.Count > 0 && ourIDP.Count > 0 && hint != 502)
            {
                foreach (var enemyMon in oppIDP)
                {
                    var match = ourIDP.FirstOrDefault(c => c.Race == enemyMon.Race);
                    if (match != null)
                    {
                        return (max >= 2) ? new List<ClientCard> { match, enemyMon } : new List<ClientCard> { enemyMon };
                    }
                }
            }

            // 3. Special Summon / Revival (Hint 509) -> Route to Plugin Strategy
            if (hint == 509)
            {
                var target = Plugin?.StrategyImpl?.PickSpecialSummonTarget(cards);
                if (target != null) return new List<ClientCard> { target };
            }

            // 4. Set Trap from Deck (Hint 510, 527, or all candidates are deck Normal Traps) -> Route to Plugin Strategy
            if (hint == 510 || hint == 527 || (cards.Count > 0 && cards.All(c => c != null && c.Location == CardLocation.Deck && IsNormalTrap(c))))
            {
                var target = Plugin?.StrategyImpl?.PickTrapToSetFromDeck(cards);
                if (target != null) return new List<ClientCard> { target };
            }

            // 5. Search / Add to Hand (Hint 506 or 0) -> Route to Plugin Strategy
            if (hint == 506 || hint == 0)
            {
                var target = Plugin?.StrategyImpl?.PickSearchTarget(cards, Card);
                if (target != null) return new List<ClientCard> { target };
            }

            // 6. Return to Hand (Hint 505) -> Big Welcome bounce our own / Enemy bounce
            if (hint == 505)
            {
                bool allOurMon = cards.Count > 0 && cards.All(c => c != null && c.Controller == 0);
                if (allOurMon)
                {
                    var target = Plugin?.StrategyImpl?.PickBounceSelfTarget(cards);
                    if (target != null) return new List<ClientCard> { target };
                }

                // Enemy bounce
                var enemyBounces = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyBounces.Count > 0)
                {
                    return enemyBounces.OrderByDescending(c => Scorer != null ? Scorer.ThreatScore(c) : c.Attack).Take(max).ToList();
                }
            }

            // 7. Discard / Send to GY Cost (Hint 501, 508, 504) -> Route to Plugin MaterialEvaluator
            if (hint == 501 || hint == 508 || hint == 504)
            {
                var target = Plugin?.MaterialImpl?.PickDiscardTarget(cards, min);
                if (target != null) return new List<ClientCard> { target };
            }

            // 7.5 Normal Trap Selection from GY (Transaction Rollback copy / Lovely Labrynth Set from GY)
            if (cards.Count > 0 && cards.All(c => c != null && c.Controller == 0 && c.Location == CardLocation.Grave && IsNormalTrap(c)))
            {
                var target = cards.OrderByDescending(c => {
                    if (c.Id == CardId.DestructiveDarumaKarmaCannon) return 100;
                    if (c.Id == CardId.BigWelcomeLabrynth) return 95;
                    if (c.Id == CardId.DimensionalBarrier) return 90;
                    if (c.Id == CardId.DogmatikaPunishment) return 85;
                    if (c.Id == CardId.IceDragonsPrison) return 80;
                    if (c.Id == CardId.WelcomeLabrynth) return 75;
                    if (c.Id == CardId.TerrorsOfTheOverroot) return 70;
                    if (c.Id == CardId.CompulsoryEvacuationDevice) return 65;
                    if (c.Id == CardId.InfiniteImpermanence) return 60;
                    return 10;
                }).FirstOrDefault();
                if (target != null) return new List<ClientCard> { target };
            }

            // 8. 🔒 HARD RULE: Target Verification & Sanity for Destruction / Removal / Targeting (502, 503, 551)
            // Always target enemy cards first! Never target own cards unless no enemy card exists.
            if (hint == 502 || hint == 503 || hint == 551)
            {
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyCards.Count > 0)
                {
                    return enemyCards.OrderByDescending(c => Scorer != null ? Scorer.ThreatScore(c) : (c.IsMonster() ? c.Attack : 100)).Take(max).ToList();
                }

                var ourCards = cards.Where(c => c != null && c.Controller == 0).ToList();
                if (ourCards.Count > 0)
                {
                    return ourCards.OrderBy(c => Plugin?.MaterialImpl?.GetMaterialCost(c) ?? 100).Take(max).ToList();
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count <= 1) return 0;

            // 1. Dimensional Barrier (0: Ritual, 1: Fusion, 2: Synchro, 3: Xyz, 4: Pendulum)
            // Any 5-option modal prompt in YGO is Dimensional Barrier!
            if (options.Count == 5 || (LastChainCard != null && (LastChainCard.Id == CardId.DimensionalBarrier || LastChainCard.Id == CardId.TransactionRollback))
                || (Duel.CurrentChain != null && Duel.CurrentChain.Any(c => c != null && (c.Id == CardId.DimensionalBarrier || c.Id == CardId.TransactionRollback))))
            {
                int declaredType = DetermineDBarrierType();
                if (declaredType < options.Count) return declaredType;
                return 1; // Default Fusion
            }

            // 2. Arias the Labrynth Butler: Special summon monster (0) or Set Trap (1)
            bool isAriasChain = (LastChainCard != null && LastChainCard.Id == CardId.AriasTheLabrynthButler)
                || (Duel.CurrentChain != null && Duel.CurrentChain.Any(c => c != null && c.Id == CardId.AriasTheLabrynthButler));
            if (isAriasChain)
            {
                if (Duel.Player == 1) // Opponent's turn: Set trap for disruption!
                {
                    bool hasTrap = Bot.Hand.Any(c => c != null && IsNormalTrap(c));
                    if (hasTrap && options.Count > 1) return 1;
                    return 0;
                }
                else // Our turn: Special summon monster if available
                {
                    bool hasMon = Bot.Hand.Any(c => c != null && IsLabrynthMonster(c));
                    if (hasMon) return 0;
                    if (options.Count > 1) return 1;
                    return 0;
                }
            }

            // 3. Lovely Labrynth: Pop card on field (0) vs hand (1)
            bool isLovelyChain = (LastChainCard != null && LastChainCard.Id == CardId.LovelyLabrynthOfTheSilverCastle)
                || (Duel.CurrentChain != null && Duel.CurrentChain.Any(c => c != null && c.Id == CardId.LovelyLabrynthOfTheSilverCastle));
            if (isLovelyChain)
            {
                if (Enemy.GetFieldCount() > 0) return 0; // Destroy card on field
                if (options.Count > 1) return 1; // Destroy card in hand
                return 0;
            }

            // 4. Eradicator Epidemic Virus: Declare Spell (0) or Trap (1)
            bool isVirusChain = (LastChainCard != null && LastChainCard.Id == CardId.EradicatorEpidemicVirus)
                || (Duel.CurrentChain != null && Duel.CurrentChain.Any(c => c != null && c.Id == CardId.EradicatorEpidemicVirus));
            if (isVirusChain || options.Count == 2)
            {
                var oppCards = Enemy.GetMonsters().Concat(Enemy.Graveyard).Concat(Enemy.GetSpells()).ToList();
                bool isAltergeist = oppCards.Any(c => c.Id == 25533642 || c.Id == 53143898 || c.Id == 89538537 || c.Id == 42790071 || c.Id == 1508649 || c.Id == 27541563);
                bool isDarkMagician = oppCards.Any(c => c.Id == 46986414 || c.Id == 47222536 || c.Id == 48680970 || c.Id == 7084129 || c.Id == 1784686 || c.Id == 30603688 || c.Id == 7922915 || c.Id == 41721210);
                bool isABC = oppCards.Any(c => c.Id == 1561110 || c.Id == 66399653 || c.Id == 77411244 || c.Id == 30012506 || c.Id == 3405259 || c.Id == 99249638 || c.Id == 12524259);
                bool isBlueEyes = oppCards.Any(c => c.Id == 89631139 || c.Id == 8240199 || c.Id == 71039903 || c.Id == 79814787 || c.Id == 38517737 || c.Id == 41620959);

                // Spell-reliant decks: Dark Magician, ABC, Blue-Eyes
                if (isDarkMagician || isABC || isBlueEyes) return 0; // Declare SPELLS!

                int oppTraps = Enemy.GetSpells().Count(c => c.IsTrap()) + Enemy.Graveyard.Count(c => c.IsTrap());
                int oppSpells = Enemy.GetSpells().Count(c => c.IsSpell()) + Enemy.Graveyard.Count(c => c.IsSpell());
                if (isAltergeist || oppTraps > oppSpells)
                {
                    return 1; // Declare Traps!
                }
                return 0; // Declare Spells
            }

            return 0;
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Core beatsticks: Lady, Lovely, Chaos Angel, TY-PHON -> FaceUpAttack
            if (cardId == CardId.LadyLabrynthOfTheSilverCastle ||
                cardId == CardId.LovelyLabrynthOfTheSilverCastle ||
                cardId == CardId.ChaosAngel ||
                cardId == CardId.SuperStarslayerTYPHON)
            {
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }

            // Small Fodder / Handtraps / Low ATK monsters -> FaceUpDefence
            if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;

            return base.OnSelectPosition(cardId, positions);
        }
    }
}
