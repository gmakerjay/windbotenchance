using YGOSharp.OCGWrapper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    // ============================================================
    // CARD AUDIT — 2026_Angelechy (Angelechy + Labrynth Hybrid)
    // ============================================================
    // Card Name                            | Type       | OPT? | HOPT? | Effect Summary                                    | Activate When                                   | NEVER Activate When
    // -------------------------------------|------------|------|-------|---------------------------------------------------|------------------------------------------------|--------------------
    // Arias the Labrynth Butler            | Monster    | Yes  | Yes   | Quick effect on hand/GY to set/play Trap or SS    | Main Phase / opp turn to set Big Welcome       | No targets in hand/GY
    // Arianna the Labrynth Servant         | Monster    | Yes  | Yes   | Search Labrynth card on NS/SS                     | Normal Summon, have targets in Deck             | No targets in Deck
    // Lady Labrynth of the Silver Castle   | Monster    | Yes  | Yes   | SS from hand when Trap used; Set Trap from Deck   | Chain to Trap activation / SS on Trap play     | No Traps left in Deck
    // Lovely Labrynth of the Silver Castle | Monster    | Yes  | Yes   | Destroy opp card when Trap triggers; recycle Trap | Main Phase / on Trap removal trigger           | No targets in GY
    // Labrynth Cooclock                    | Monster    | Yes  | Yes   | Discard to allow Trap activation on turn set      | Hand: have Trap set this turn to activate      | No set Traps
    // Labrynth Chandraglier                | Monster    | Yes  | Yes   | Discard with 1 card to set Welcome from Deck      | Hand: have discard fodder & Welcome in Deck    | No fodder in hand
    // Labrynth Stovie Torbie               | Monster    | Yes  | Yes   | Discard with 1 card to set Welcome from Deck      | Hand: have discard fodder & Welcome in Deck    | No fodder in hand
    // Welcome Labrynth                     | NormalTrap | Yes  | Yes   | SS Labrynth from Deck; recycle from GY           | Main Phase / opp turn to trigger Labrynth      | Board full
    // Big Welcome Labrynth                 | NormalTrap | Yes  | Yes   | SS Labrynth from Deck/Hand/GY then bounce 1 card  | Main Phase / opp turn to trigger bounce loop   | Board full
    // Angelechy Problem                    | FieldSpell | Yes  | Yes   | Discard S/T → SS Lv2 Angelechy + Place S/T Zone   | Main Phase: have S/T discard fodder            | No targets in Deck
    // Angelechy Disturbance                | NormalTrap | Yes  | Yes   | Give Angelechy to opp → negate adjacent column    | Opp turn / Main Phase: opp has monsters in col | No Angelechy on field
    // Witness of the Ancient               | Monster    | Yes  | Yes   | SS from hand if Synchro on field → place 3 cards  | Hand: have Synchro on field                    | SS blocked
    // Angelechy Opening to e4              | NormalTrap | Yes  | Yes   | Activate from hand Standby Phase turn 1 → Place  | Turn 1 Opp Standby Phase / Main Phase          | Board full
    // Angelechy Bastion                    | Synchro    | Yes  | Yes   | Banish opp card in col; S/T zone protects cards  | Main Phase: target opp card in column           | Opp board empty
    // Angelechy Shatranga                  | Synchro    | Yes  | Yes   | Banish opp card; S/T zone limits opp to 5 monster | Main Phase / Opp turn: limit opp activations    | Opp board empty
    // Angelechy Destrier                   | Synchro    | Yes  | Yes   | Banish opp card; S/T zone inflicts 500 burn       | Main Phase: target opp card in another column  | Opp board empty
    // Angelechy Enlisted                   | Synchro    | Yes  | Yes   | Banish opp adjacent monster & move control        | Main Phase: target opp adjacent monster         | No adjacent targets
    // Ash Blossom & Joyous Spring          | HandTrap   | Yes  | Yes   | Negate deck search / SS from deck                 | Opponent triggers deck search                   | Own turn / own chain
    // Droll & Lock Bird                    | HandTrap   | Yes  | Yes   | Lock card adding from deck                        | Opponent adds card from deck                    | Own turn
    // Dominus Spark                        | NormalTrap | Yes  | Yes   | Negate monster activation on field/hand           | Opponent activates monster effect               | Own chain
    // Pot of Extravagance                  | NormalSpell| Yes  | Yes   | Banish 3/6 ED cards to draw 1/2 cards             | Main Phase 1 start                              | Already drawn / MP2
    // Trap Trick                           | NormalTrap | Yes  | Yes   | Banish 1 Trap → Set 1 copy & activate same turn   | Main Phase / Opp turn to fetch key Trap         | No duplicate Traps
    // Different Dimension Ground           | NormalTrap | Yes  | Yes   | Banish all monsters sent to GY this turn           | Opponent turn when GY combo starts             | Own turn
    // The Black Goat Laughs                | NormalTrap | Yes  | Yes   | Declare card name → block SS & activation         | Opponent turn / Standby Phase                   | Unknown deck
    // Chaos Angel                          | Synchro    | No   | No    | Banish 1 card on SS; LIGHT/DARK protection        | Main Phase: 2 LIGHT/DARK monsters on field     | SS blocked
    // Glitch Clutch Nullgainer             | Synchro    | Yes  | Yes   | 2000 DEF wall & disruption                        | Main Phase                                      | SS blocked
    // Kewl Tune B2B                        | Synchro    | Yes  | Yes   | 3200 ATK double attacker + Quick Synchro           | Main Phase                                      | SS blocked
    // Relinquished Anima                   | Link       | No   | No    | Absorb opp monster in column                      | Main Phase: opp monster in front of ED zone     | No target in column
    // ============================================================
    // ACE CARDS:
    //   Primary  : Lady Labrynth of the Silver Castle (3000 ATK, Untargetable, Trap search)
    //   Secondary: Angelechy Shatranga (3000 ATK, Banish + Limit opp to 5 monster activations)
    //   Tertiary : Chaos Angel (3500 ATK, Banish on SS + Protection)
    //
    // COMBO STARTERS:
    //   1. Angelechy Opening to e4 (Opp Standby Phase → Place Field Spell + SS Lv2/7 Angelechy)
    //   2. Arias the Labrynth Butler + Big Welcome Labrynth (Instant turn 1 Trap activation)
    //   3. Angelechy Problem (Discard S/T → SS Lv2 Angelechy + Place S/T Zone)
    //   4. Arianna the Labrynth Servant (Normal Summon → Search Big Welcome / Arias)
    //
    // IDEAL END BOARD:
    //   Angelechy Shatranga (in S/T Zone: Opponent limited to 5 monster activations/turn)
    //   + Lady Labrynth / Lovely Labrynth on field
    //   + Set Big Welcome Labrynth + Dominus Spark / Trap Trick
    // ============================================================

    [Deck("2026_Angelechy", "2026_Angelechy")]
    public class _2026_AngelechyExecutor : ModernExecutor
    {
        public class CardId
        {
            // --- LABRYNTH CORE ENGINE ---
            public const int AriasTheLabrynthButler = 73602965;
            public const int AriannaTheLabrynthServant = 1225009;
            public const int LadyLabrynthOfTheSilverCastle = 81497285;
            public const int LovelyLabrynthOfTheSilverCastle = 2347656;
            public const int LabrynthCooclock = 2511;
            public const int LabrynthChandraglier = 37629703;
            public const int LabrynthStovieTorbie = 74018812;
            public const int WelcomeLabrynth = 5380979;
            public const int BigWelcomeLabrynth = 92714517;

            // --- ANGELECHY CORE ENGINE ---
            public const int AngelechyProblem = 17782288;
            public const int AngelechyDisturbance = 54171327;
            public const int WitnessOfTheAncient = 54577949;
            public const int AngelechyOpeningToe4 = 80565021;

            // --- ANGELECHY EXTRA DECK ACES ---
            public const int AngelechyBastion = 28904860;
            public const int AngelechyShatranga = 42410161;
            public const int AngelechyDestrier = 55393975;
            public const int AngelechyEnlisted = 81797573;

            // --- DISRUPTION & STAPLE HAND TRAPS / TRAPS ---
            public const int AshBlossom = 14558127;
            public const int DrollAndLockBird = 94145021;
            public const int DominusSpark = 6325660;
            public const int PotOfExtravagance = 49238328;
            public const int TrapTrick = 80101899;
            public const int DifferentDimensionGround = 31849106;
            public const int TheBlackGoatLaughs = 49299410;

            // --- UTILITY EXTRA DECK ---
            public const int ChaosAngel = 22850702;
            public const int GlitchClutchNullgainer = 32044675;
            public const int KewlTuneB2B = 65961304;
            public const int RelinquishedAnima = 94259633;
        }

        // --- OPT / Turn Trackers ---
        private bool _problemUsed;
        private bool _disturbanceUsed;
        private bool _witnessUsed;
        private bool _openingUsed;
        private bool _ariasUsed;
        private bool _ariannaUsed;
        private bool _ladySummoned;
        private bool _ladySetUsed;
        private bool _lovelyDestroyUsed;
        private bool _stovieUsed;
        private bool _chandraglierUsed;
        private bool _trapTrickUsed;
        private bool _extravaganceUsed;

        // --- ACE CARDS (PROTECTION PRIORITY) ---
        private static readonly int[] AceCardIds = {
            CardId.LadyLabrynthOfTheSilverCastle,
            CardId.LovelyLabrynthOfTheSilverCastle,
            CardId.AngelechyBastion,
            CardId.AngelechyShatranga,
            CardId.AngelechyDestrier,
            CardId.ChaosAngel
        };

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Contains(card.Id);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.Id == CardId.LabrynthCooclock) return 10;
            if (c.Id == CardId.LabrynthStovieTorbie || c.Id == CardId.LabrynthChandraglier) return 20;
            if (c.Id == CardId.WitnessOfTheAncient) return 30;
            if (c.Id == CardId.AriannaTheLabrynthServant) return 150;
            return base.GetMaterialPriority(c);
        }

        protected override bool IsBoardStrongEnough()
        {
            if (Bot.HasInMonstersZone(CardId.LadyLabrynthOfTheSilverCastle) || Bot.HasInMonstersZone(CardId.LovelyLabrynthOfTheSilverCastle))
            {
                if (Bot.HasInSpellZone(CardId.AngelechyShatranga) || Bot.HasInSpellZone(CardId.AngelechyBastion))
                    return true;
                if (Bot.GetSpellCount() >= 2)
                    return true;
            }
            if (Bot.HasInMonstersZone(CardId.ChaosAngel)) return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (IsBoardStrongEnough()) return true;
            return base.ShouldStopExtending();
        }

        public _2026_AngelechyExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // ── Register AI Modules ──
            HeuristicGuard.RegisterAceCards(AceCardIds);
            ResourcePlan.RegisterAceCards(AceCardIds);

            // ── Combo Router Lines ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Opening-e4-Angelechy",
                RequiredCards = new List<int> { CardId.AngelechyOpeningToe4 },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.AngelechyOpeningToe4, ActionType = ExecutorType.Activate, Description = "Activate Opening to e4" },
                    new() { CardId = CardId.AngelechyProblem, ActionType = ExecutorType.Activate, Description = "Set up Angelechy Problem" }
                },
                EndBoardScore = 90
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Arias-BigWelcome-Setup",
                RequiredCards = new List<int> { CardId.AriasTheLabrynthButler, CardId.BigWelcomeLabrynth },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.AriasTheLabrynthButler, ActionType = ExecutorType.Activate, Description = "Arias hand effect" },
                    new() { CardId = CardId.BigWelcomeLabrynth, ActionType = ExecutorType.SpellSet, Description = "Set Big Welcome via Arias" }
                },
                EndBoardScore = 85
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Arianna-Search-Setup",
                RequiredCards = new List<int> { CardId.AriannaTheLabrynthServant, CardId.BigWelcomeLabrynth },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.AriannaTheLabrynthServant, ActionType = ExecutorType.Summon, Description = "Normal Summon Arianna" },
                    new() { CardId = CardId.BigWelcomeLabrynth, ActionType = ExecutorType.SpellSet, Description = "Set Big Welcome" }
                },
                EndBoardScore = 80
            });

            // ── Bait & Chain Advisor ──
            BaitPlanner.RegisterComboStarters(CardId.AngelechyOpeningToe4, CardId.BigWelcomeLabrynth, CardId.AngelechyProblem);
            BaitPlanner.RegisterBaitCards(CardId.PotOfExtravagance, CardId.AriannaTheLabrynthServant);
            ChainAdvisor.RegisterHighValueTargets(CardId.BigWelcomeLabrynth, CardId.AngelechyProblem, CardId.AngelechyShatranga);

            // ═══ TIER 1: Reactive Hand Traps & Negations ═══
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollEffect);
            AddExecutor(ExecutorType.Activate, CardId.DominusSpark, DominusSparkEffect);
            AddExecutor(ExecutorType.Activate, CardId.AriasTheLabrynthButler, AriasEffect);
            AddExecutor(ExecutorType.Activate, CardId.LadyLabrynthOfTheSilverCastle, LadyEffect);
            AddExecutor(ExecutorType.Activate, CardId.LovelyLabrynthOfTheSilverCastle, LovelyEffect);

            // ═══ TIER 2: Angelechy Quick Disruption & Column Controls ═══
            AddExecutor(ExecutorType.Activate, CardId.AngelechyOpeningToe4, AngelechyOpeningEffect);
            AddExecutor(ExecutorType.Activate, CardId.AngelechyDisturbance, AngelechyDisturbanceEffect);
            AddExecutor(ExecutorType.Activate, CardId.AngelechyShatranga, ShatrangaEffect);
            AddExecutor(ExecutorType.Activate, CardId.AngelechyBastion, BastionEffect);
            AddExecutor(ExecutorType.Activate, CardId.AngelechyDestrier, DestrierEffect);
            AddExecutor(ExecutorType.Activate, CardId.AngelechyEnlisted, EnlistedEffect);

            // ═══ TIER 3: Trap Triggers & Removal Loops ═══
            AddExecutor(ExecutorType.Activate, CardId.BigWelcomeLabrynth, BigWelcomeEffect);
            AddExecutor(ExecutorType.Activate, CardId.WelcomeLabrynth, WelcomeEffect);
            AddExecutor(ExecutorType.Activate, CardId.TrapTrick, TrapTrickEffect);
            AddExecutor(ExecutorType.Activate, CardId.DifferentDimensionGround, DDGroundEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheBlackGoatLaughs, BlackGoatEffect);

            // ═══ TIER 4: Spells & Extenders ═══
            AddExecutor(ExecutorType.Activate, CardId.PotOfExtravagance, ExtravaganceEffect);
            AddExecutor(ExecutorType.Activate, CardId.AngelechyProblem, AngelechyProblemEffect);
            AddExecutor(ExecutorType.Activate, CardId.WitnessOfTheAncient, WitnessEffect);

            // ═══ TIER 5: Monster Effects & Furniture ═══
            AddExecutor(ExecutorType.Activate, CardId.AriannaTheLabrynthServant, AriannaEffect);
            AddExecutor(ExecutorType.Activate, CardId.LabrynthCooclock, CooclockEffect);
            AddExecutor(ExecutorType.Activate, CardId.LabrynthStovieTorbie, StovieEffect);
            AddExecutor(ExecutorType.Activate, CardId.LabrynthChandraglier, ChandraglierEffect);

            // ═══ TIER 6: Extra Deck Special Summons ═══
            AddExecutor(ExecutorType.SpSummon, CardId.ChaosAngel, ChaosAngelSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.RelinquishedAnima, AnimaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AngelechyBastion, BastionSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AngelechyShatranga, ShatrangaSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.GlitchClutchNullgainer, () => !IsSpecialSummonBlocked());
            AddExecutor(ExecutorType.SpSummon, CardId.KewlTuneB2B, () => !IsSpecialSummonBlocked());
            AddExecutor(ExecutorType.Activate, CardId.GlitchClutchNullgainer, () => true);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneB2B, () => true);

            // ═══ TIER 7: Normal Summons ═══
            AddExecutor(ExecutorType.Summon, CardId.AriannaTheLabrynthServant, AriannaSummon);
            AddExecutor(ExecutorType.Summon, CardId.AriasTheLabrynthButler, AriasSummon);
            AddExecutor(ExecutorType.Summon, CardId.WitnessOfTheAncient);
            AddExecutor(ExecutorType.Summon, CardId.LabrynthCooclock);

            // ═══ TIER 8: Spell/Trap Sets ═══
            AddExecutor(ExecutorType.SpellSet, CardId.BigWelcomeLabrynth);
            AddExecutor(ExecutorType.SpellSet, CardId.WelcomeLabrynth);
            AddExecutor(ExecutorType.SpellSet, CardId.TrapTrick);
            AddExecutor(ExecutorType.SpellSet, CardId.DominusSpark);
            AddExecutor(ExecutorType.SpellSet, CardId.DifferentDimensionGround);
            AddExecutor(ExecutorType.SpellSet, CardId.TheBlackGoatLaughs);

            // Always last: Battle & Position logic
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand() => true; // Prefer Going First to set up controls

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _problemUsed = false;
            _disturbanceUsed = false;
            _witnessUsed = false;
            _openingUsed = false;
            _ariasUsed = false;
            _ariannaUsed = false;
            _ladySummoned = false;
            _ladySetUsed = false;
            _lovelyDestroyUsed = false;
            _stovieUsed = false;
            _chandraglierUsed = false;
            _trapTrickUsed = false;
            _extravaganceUsed = false;
        }

        // --- STRATEGIC HAND TRAPS & REACTIVE DISRUPTIONS ---

        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool DrollEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.LastChainPlayer == 1 && Duel.Player == 1;
        }

        private bool DominusSparkEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard == null || LastChainCard.Controller != 1) return false;
            return true;
        }

        private bool AriasEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_ariasUsed) return false;

            if (Card.Location == CardLocation.Hand)
            {
                bool hasTarget = Bot.Hand.Any(c => c != null && c.Id != CardId.AriasTheLabrynthButler && (IsLabrynthMonster(c) || c.IsTrap()));
                if (!hasTarget) return false;
                _ariasUsed = true;
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                if (IsSpecialSummonBlocked()) return false;
                _ariasUsed = true;
                return true;
            }
            return false;
        }

        // --- ANGELECHY ARCHETYPE LOGIC ---

        private bool AngelechyOpeningEffect()
        {
            if (_openingUsed) return false;
            _openingUsed = true;
            return true;
        }

        private bool AngelechyProblemEffect()
        {
            if (_problemUsed) return false;
            bool hasDiscard = Bot.Hand.Any(c => c != null && c.Id != CardId.AngelechyProblem && (c.IsSpell() || c.IsTrap()));
            if (!hasDiscard) return false;
            _problemUsed = true;
            return true;
        }

        private bool AngelechyDisturbanceEffect()
        {
            if (_disturbanceUsed) return false;
            bool hasAngelechyMon = Bot.GetMonsters().Any(c => c != null && IsAngelechyCard(c));
            if (!hasAngelechyMon) return false;
            _disturbanceUsed = true;
            return true;
        }

        private bool WitnessEffect()
        {
            if (_witnessUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            _witnessUsed = true;
            return true;
        }

        private bool ShatrangaEffect()
        {
            if (Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0) return false;
            return true;
        }

        private bool BastionEffect() => true;
        private bool DestrierEffect() => true;
        private bool EnlistedEffect() => true;

        // --- LABRYNTH CORE LOGIC ---

        private bool LadyEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (!_ladySummoned && Card.Location == CardLocation.Hand)
            {
                _ladySummoned = true;
                return true;
            }
            if (!_ladySetUsed && Card.Location == CardLocation.MonsterZone)
            {
                if (LastChainCard != null && LastChainCard.IsTrap())
                {
                    _ladySetUsed = true;
                    return true;
                }
            }
            return true;
        }

        private bool LovelyEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (!_lovelyDestroyUsed)
            {
                _lovelyDestroyUsed = true;
                return true;
            }
            return true;
        }

        private bool AriannaEffect()
        {
            if (_ariannaUsed) return false;
            _ariannaUsed = true;
            return true;
        }

        private bool CooclockEffect() => true;

        private bool StovieEffect()
        {
            if (_stovieUsed) return false;
            if (!Bot.Hand.Any(c => c != null && c.Id != CardId.LabrynthStovieTorbie)) return false;
            _stovieUsed = true;
            return true;
        }

        private bool ChandraglierEffect()
        {
            if (_chandraglierUsed) return false;
            if (!Bot.Hand.Any(c => c != null && c.Id != CardId.LabrynthChandraglier)) return false;
            _chandraglierUsed = true;
            return true;
        }

        private bool BigWelcomeEffect() => true;
        private bool WelcomeEffect() => true;

        private bool TrapTrickEffect()
        {
            if (_trapTrickUsed) return false;
            _trapTrickUsed = true;
            return true;
        }

        private bool DDGroundEffect() => Duel.Player == 1;

        private bool BlackGoatEffect() => Duel.Player == 1;

        private bool ExtravaganceEffect()
        {
            if (_extravaganceUsed) return false;
            if (Duel.Phase != DuelPhase.Main1) return false;
            _extravaganceUsed = true;
            return true;
        }

        // --- EXTRA DECK SUMMON CHECKS ---

        private bool ChaosAngelSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            int fiendLightDarkCount = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark)));
            return fiendLightDarkCount >= 2;
        }

        private bool AnimaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            ClientCard oppFrontAnima = Enemy.MonsterZone[0] ?? Enemy.MonsterZone[2] ?? Enemy.MonsterZone[4];
            return oppFrontAnima != null && oppFrontAnima.IsFaceup();
        }

        private bool BastionSpSummon() => !IsSpecialSummonBlocked();
        private bool ShatrangaSpSummon() => !IsSpecialSummonBlocked();

        // --- SUMMON HELPERS ---

        private bool AriannaSummon() => true;
        private bool AriasSummon() => true;

        private bool IsLabrynthMonster(ClientCard c)
        {
            if (c == null) return false;
            return c.Id == CardId.LadyLabrynthOfTheSilverCastle ||
                   c.Id == CardId.LovelyLabrynthOfTheSilverCastle ||
                   c.Id == CardId.AriannaTheLabrynthServant ||
                   c.Id == CardId.AriasTheLabrynthButler ||
                   c.Id == CardId.LabrynthStovieTorbie ||
                   c.Id == CardId.LabrynthChandraglier;
        }

        private bool IsAngelechyCard(ClientCard c)
        {
            if (c == null) return false;
            return c.Id == CardId.AngelechyProblem ||
                   c.Id == CardId.AngelechyDisturbance ||
                   c.Id == CardId.WitnessOfTheAncient ||
                   c.Id == CardId.AngelechyOpeningToe4 ||
                   c.Id == CardId.AngelechyBastion ||
                   c.Id == CardId.AngelechyShatranga ||
                   c.Id == CardId.AngelechyDestrier ||
                   c.Id == CardId.AngelechyEnlisted;
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return base.OnSelectCard(cards, min, max, hint, cancelable);

            // 1. Extra Deck Selection / S&T Zone Placement (Hint 509 / 506 from Extra):
            // When Opening to e4, Problem, or Witness selects Synchros from Extra Deck
            if (cards.Any(c => c != null && c.Location == CardLocation.Extra))
            {
                var extraPriority = cards.Where(c => c != null && c.Location == CardLocation.Extra)
                    .OrderBy(c =>
                    {
                        if (c.Id == CardId.AngelechyBastion && !Bot.HasInSpellZone(CardId.AngelechyBastion)) return 1;
                        if (c.Id == CardId.AngelechyShatranga && !Bot.HasInSpellZone(CardId.AngelechyShatranga)) return 2;
                        if (c.Id == CardId.AngelechyDestrier && !Bot.HasInSpellZone(CardId.AngelechyDestrier)) return 3;
                        if (c.Id == CardId.AngelechyBastion) return 4;
                        if (c.Id == CardId.AngelechyShatranga) return 5;
                        if (c.Id == CardId.AngelechyEnlisted) return 6;
                        if (c.Id == CardId.AngelechyDestrier) return 7;
                        return 50;
                    }).ToList();

                if (extraPriority.Count >= min)
                    return extraPriority.Take(Math.Min(max, extraPriority.Count)).ToList();
            }

            // 2. Big Welcome / Bounce to hand (Hint 551): Bounce small furniture/searchers, protect bosses
            if (hint == 551 || hint == 506 && cards.All(c => c != null && c.Location == CardLocation.MonsterZone))
            {
                var bouncePriority = cards.Where(c => c != null && c.Controller == 0)
                    .OrderBy(c =>
                    {
                        if (c.Id == CardId.LabrynthCooclock) return 1;
                        if (c.Id == CardId.AriannaTheLabrynthServant) return 2;
                        if (c.Id == CardId.AriasTheLabrynthButler) return 3;
                        if (c.Id == CardId.LabrynthStovieTorbie) return 4;
                        if (c.Id == CardId.LabrynthChandraglier) return 5;
                        if (IsAceCard(c)) return 999;
                        return 50;
                    }).ToList();

                if (bouncePriority.Count > 0 && bouncePriority.First().Controller == 0 && !IsAceCard(bouncePriority.First()))
                {
                    var selected = bouncePriority.Take(min).ToList();
                    if (selected.Count >= min) return selected;
                }
            }

            // 3. Search / Add from Deck (Hint 506): Prioritize key starters, Angelechy S/T, and traps
            if (hint == 506)
            {
                var searchPriority = cards.OrderBy(c =>
                {
                    if (c.Id == CardId.BigWelcomeLabrynth) return 1;
                    if (c.Id == CardId.AngelechyProblem && !Bot.HasInHand(CardId.AngelechyProblem) && !Bot.HasInSpellZone(CardId.AngelechyProblem)) return 2;
                    if (c.Id == CardId.AngelechyDisturbance && !Bot.HasInHand(CardId.AngelechyDisturbance) && !Bot.HasInSpellZone(CardId.AngelechyDisturbance)) return 3;
                    if (c.Id == CardId.AriasTheLabrynthButler) return 4;
                    if (c.Id == CardId.AngelechyOpeningToe4) return 5;
                    if (c.Id == CardId.LadyLabrynthOfTheSilverCastle) return 6;
                    if (c.Id == CardId.AriannaTheLabrynthServant) return 7;
                    if (c.Id == CardId.WelcomeLabrynth) return 8;
                    if (c.Id == CardId.DominusSpark) return 9;
                    if (c.Id == CardId.LabrynthCooclock) return 10;
                    return 50;
                }).ToList();

                if (searchPriority.Count > 0)
                {
                    var selected = searchPriority.Take(max).ToList();
                    if (selected.Count >= min) return selected;
                }
            }

            // 4. Set Trap from Deck (Hint 508 - Lady Labrynth effect)
            if (hint == 508)
            {
                var setPriority = cards.OrderBy(c =>
                {
                    if (c.Id == CardId.BigWelcomeLabrynth) return 1;
                    if (c.Id == CardId.DominusSpark) return 2;
                    if (c.Id == CardId.AngelechyDisturbance) return 3;
                    if (c.Id == CardId.DifferentDimensionGround && Duel.Player == 1) return 4;
                    if (c.Id == CardId.TrapTrick) return 5;
                    if (c.Id == CardId.WelcomeLabrynth) return 6;
                    if (c.Id == CardId.TheBlackGoatLaughs) return 7;
                    return 50;
                }).ToList();

                if (setPriority.Count > 0)
                {
                    var selected = setPriority.Take(max).ToList();
                    if (selected.Count >= min) return selected;
                }
            }

            // 5. Discard Costs (Hint 501, 504, 502 - Stovie, Chandraglier, Problem)
            if (hint == 501 || hint == 504 || hint == 502)
            {
                var discardPriority = cards.Where(c => c != null && c.Controller == 0)
                    .OrderBy(c =>
                    {
                        if (c.Id == CardId.LabrynthCooclock) return 1;
                        if (c.Id == CardId.LabrynthStovieTorbie && Bot.HasInHand(CardId.LabrynthStovieTorbie)) return 2;
                        if (c.Id == CardId.LabrynthChandraglier && Bot.HasInHand(CardId.LabrynthChandraglier)) return 3;
                        if (c.IsTrap() && Bot.Hand.Count(h => h != null && h.Id == c.Id) > 1) return 4;
                        if (c.Id == CardId.WitnessOfTheAncient) return 5;
                        if (IsAceCard(c)) return 999;
                        return 50;
                    }).ToList();

                if (discardPriority.Count > 0)
                {
                    var selected = discardPriority.Take(min).ToList();
                    if (selected.Count >= min) return selected;
                }
            }

            // 6. Opponent Removal / Disruption Target Selection (Hint 502, 533)
            if (hint == 502 || hint == 533)
            {
                var enemyTargets = cards.Where(c => c != null && c.Controller == 1).OrderByDescending(c =>
                {
                    int score = 0;
                    if (c.IsFaceup()) score += 50;
                    if (c.IsMonster()) score += c.Attack;
                    if (c.IsSpell() || c.IsTrap()) score += 2000;
                    return score;
                }).ToList();

                if (enemyTargets.Count >= min)
                    return enemyTargets.Take(min).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        private bool MonsterRepos()
        {
            if (Card == null) return false;
            if (Card.IsAttack())
            {
                if (Enemy.GetMonsterCount() > 0 && Card.Attack < 1500) return true;
            }
            else
            {
                if (Card.Attack >= 1500) return true;
            }
            return false;
        }
    }
}

