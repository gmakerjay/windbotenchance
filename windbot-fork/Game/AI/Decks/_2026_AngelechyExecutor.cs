using YGOSharp.OCGWrapper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    // ====================================================================================
    // CARD AUDIT — 2026_Angelechy (Angelechy Column Control + Labrynth Trap Engine)
    // ====================================================================================
    // Card Name                            | Type       | OPT? | HOPT? | Effect Summary
    // -------------------------------------|------------|------|-------|---------------------------------------------------
    // Arias the Labrynth Butler            | Monster    | Yes  | Yes   | Quick: Hand/Field to GY → SS Labrynth or Set Trap (active this turn) / GY Quick revive
    // Arianna the Labrynth Servant         | Monster    | Yes  | Yes   | On NS/SS: Search Labrynth card from Deck
    // Lady Labrynth of the Silver Castle   | Monster    | Yes  | Yes   | Hand: Quick SS on Trap / Field: Quick Set Normal Trap from Deck
    // Lovely Labrynth of the Silver Castle | Monster    | Yes  | Yes   | Field: Destroy opp card on Trap removal / Ignition: Set Normal Trap from GY
    // Labrynth Cooclock                    | Monster    | Yes  | Yes   | Quick: Discard → activate Set Normal Trap this turn
    // Labrynth Chandraglier                | Monster    | Yes  | Yes   | Quick: Send with 1 card → Set Welcome / GY: Add to hand on Trap removal
    // Labrynth Stovie Torbie               | Monster    | Yes  | Yes   | Quick: Send with 1 card → Set Welcome / GY: SS to field on Trap removal
    // Welcome Labrynth                     | NormalTrap | Yes  | Yes   | SS Labrynth from Deck; GY recycle
    // Big Welcome Labrynth                 | NormalTrap | Yes  | Yes   | SS Labrynth from Hand/Deck/GY, bounce 1 own monster / GY: Banish to bounce
    // Angelechy Problem                    | FieldSpell | Yes  | Yes   | Discard S/T → SS Lv2 Angelechy (Enlisted) + Place Angelechy in S/T Zone
    // Angelechy Disturbance                | NormalTrap | Yes  | Yes   | Give Angelechy to opp → negate adjacent cards / GY: Banish to search S/T
    // Witness of the Ancient               | Monster    | Yes  | Yes   | SS from hand if Synchro on field/GY → Place up to 3 Synchros in S/T Zone + Token
    // Angelechy Opening to e4              | NormalTrap | Yes  | Yes   | Turn 1 Opp Standby (from hand) / Place Problem + SS Lv2/7 Angelechy + Place S/T
    // Angelechy Bastion                    | Synchro    | Yes  | Yes   | Field: Banish in column / S/T Zone: Place Shatranga + Protect Angelechy from destroy
    // Angelechy Shatranga                  | Synchro    | Yes  | Yes   | Field: Banish opp monster / S/T Zone: Search Trap + Opp max 5 monster activations
    // Angelechy Destrier                   | Synchro    | Yes  | Yes   | Field: Banish another column / S/T Zone: Search Spell + 500 burn per opp activation
    // Angelechy Enlisted                   | Synchro    | Yes  | Yes   | Field: Banish adjacent opp monster & give control; on control change: return to ED & SS Angelechy!
    // Ash Blossom & Joyous Spring          | HandTrap   | Yes  | Yes   | Negate deck search / dump / SS from deck
    // Droll & Lock Bird                    | HandTrap   | Yes  | Yes   | Lock deck addition after 1st add
    // Dominus Spark                        | NormalTrap | Yes  | Yes   | Pop monster on field + revive from GY if Trap controlled; HAND USE LOCKS DARK DUEL-LONG!
    // Pot of Extravagance                  | NormalSpell| Yes  | Yes   | Banish 6 ED cards → draw 2 (MP1 start only)
    // Trap Trick                           | NormalTrap | Yes  | Yes   | Banish 1 Trap → Set 1 copy and activate this turn
    // Different Dimension Ground           | NormalTrap | Yes  | Yes   | Banish all monsters sent to GY this turn
    // The Black Goat Laughs                | NormalTrap | Yes  | Yes   | Declare card name: lock SS / GY: Banish to lock on-field effects
    // Chaos Angel                          | Synchro    | No   | No    | Banish 1 card on SS; LIGHT/DARK protection
    // Glitch Clutch Nullgainer             | Synchro    | Yes  | Yes   | Lv8 Synchro: Search 0 ATK Lv8+ (Witness) + Revive 0 ATK monster
    // Kewl Tune Back 2 Back                | Synchro    | Yes  | Yes   | 3200 ATK double attack; Quick: SS Tuner + Synchro Summon
    // Relinquished Anima                   | Link       | No   | No    | Link-1: Absorb opp monster in front of EMZ
    // ====================================================================================

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

            // --- TOKENS ---
            public const int ArcToken = 54577950;
        }

        // --- OPT / Turn Trackers ---
        private bool _problemUsed;
        private bool _disturbanceUsed;
        private bool _disturbanceGyUsed;
        private bool _witnessUsed;
        private bool _openingUsed;
        private bool _ariasHandUsed;
        private bool _ariasGyUsed;
        private bool _ariannaUsed;
        private bool _ladySummoned;
        private bool _ladySetUsed;
        private bool _lovelyDestroyUsed;
        private bool _lovelySetUsed;
        private bool _stovieHandUsed;
        private bool _stovieGyUsed;
        private bool _chandraglierHandUsed;
        private bool _chandraglierGyUsed;
        private bool _cooclockUsed;
        private bool _trapTrickUsed;
        private bool _extravaganceUsed;
        private bool _bigWelcomeGyUsed;
        private bool _dominusSparkUsed;
        private bool _nullgainerUsed;
        private bool _kewlTuneUsed;

        // --- ACE CARDS (PROTECTION PRIORITY) ---
        private static readonly int[] AceCardIds = {
            CardId.LadyLabrynthOfTheSilverCastle,
            CardId.LovelyLabrynthOfTheSilverCastle,
            CardId.AngelechyBastion,
            CardId.AngelechyShatranga,
            CardId.AngelechyDestrier,
            CardId.ChaosAngel,
            CardId.KewlTuneB2B
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
            if (c.Id == CardId.AngelechyEnlisted) return 40;
            if (c.Id == CardId.AriannaTheLabrynthServant) return 150;
            return base.GetMaterialPriority(c);
        }

        private bool HasLethalOnBoard()
        {
            if (Enemy.LifePoints <= 0) return true;
            if (Enemy.GetMonsterCount() > 0) return false;
            int totalAtk = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && m.IsAttack()).Sum(m => m.Attack);
            return totalAtk >= Enemy.LifePoints;
        }

        protected override bool IsBoardStrongEnough()
        {
            if (HasLethalOnBoard()) return true;

            bool hasLabrynthBoss = Bot.HasInMonstersZone(CardId.LadyLabrynthOfTheSilverCastle) ||
                                   Bot.HasInMonstersZone(CardId.LovelyLabrynthOfTheSilverCastle);
            bool hasAngelechyFloodgate = Bot.HasInSpellZone(CardId.AngelechyShatranga) ||
                                        Bot.HasInSpellZone(CardId.AngelechyBastion);
            bool hasChaosAngel = Bot.HasInMonstersZone(CardId.ChaosAngel);

            if (hasLabrynthBoss && hasAngelechyFloodgate) return true;
            if (hasChaosAngel && Bot.GetSpellCount() >= 2) return true;
            if (hasLabrynthBoss && Bot.GetSpellCount() >= 3) return true;

            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (HasLethalOnBoard()) return true;
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
                RequiredCards = new List<int> { CardId.AriannaTheLabrynthServant },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.AriannaTheLabrynthServant, ActionType = ExecutorType.Summon, Description = "Normal Summon Arianna" },
                    new() { CardId = CardId.AriannaTheLabrynthServant, ActionType = ExecutorType.Activate, Description = "Search Big Welcome or Arias" }
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
            AddExecutor(ExecutorType.SpSummon, CardId.GlitchClutchNullgainer, NullgainerSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.KewlTuneB2B, KewlTuneSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.GlitchClutchNullgainer, NullgainerEffect);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneB2B, KewlTuneEffect);

            // ═══ TIER 7: Normal Summons ═══
            AddExecutor(ExecutorType.Summon, CardId.AriannaTheLabrynthServant, AriannaSummon);
            AddExecutor(ExecutorType.Summon, CardId.AriasTheLabrynthButler, AriasSummon);
            AddExecutor(ExecutorType.Summon, CardId.WitnessOfTheAncient, () => false); // Always keep in hand for SS
            AddExecutor(ExecutorType.Summon, CardId.LabrynthCooclock, () => Bot.GetMonsterCount() == 0);

            // ═══ TIER 8: Spell/Trap Sets ═══
            AddExecutor(ExecutorType.SpellSet, CardId.BigWelcomeLabrynth);
            AddExecutor(ExecutorType.SpellSet, CardId.WelcomeLabrynth);
            AddExecutor(ExecutorType.SpellSet, CardId.TrapTrick);
            AddExecutor(ExecutorType.SpellSet, CardId.DominusSpark);
            AddExecutor(ExecutorType.SpellSet, CardId.DifferentDimensionGround);
            AddExecutor(ExecutorType.SpellSet, CardId.TheBlackGoatLaughs);
            AddExecutor(ExecutorType.SpellSet, CardId.AngelechyDisturbance);

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
            _disturbanceGyUsed = false;
            _witnessUsed = false;
            _openingUsed = false;
            _ariasHandUsed = false;
            _ariasGyUsed = false;
            _ariannaUsed = false;
            _ladySummoned = false;
            _ladySetUsed = false;
            _lovelyDestroyUsed = false;
            _lovelySetUsed = false;
            _stovieHandUsed = false;
            _stovieGyUsed = false;
            _chandraglierHandUsed = false;
            _chandraglierGyUsed = false;
            _cooclockUsed = false;
            _trapTrickUsed = false;
            _extravaganceUsed = false;
            _bigWelcomeGyUsed = false;
            _dominusSparkUsed = false;
            _nullgainerUsed = false;
            _kewlTuneUsed = false;
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
            if (_dominusSparkUsed) return false;

            // If card is in hand: ACTIVATING FROM HAND LOCKS ALL DARK MONSTER EFFECTS FOR ENTIRE DUEL!
            // All Labrynth monsters are DARK Fiends.
            // Only allow activation from hand if:
            // 1) Facing immediate lethal attack from opponent, OR
            // 2) Bot controls and has in hand/GY NO Labrynth monsters.
            if (Card.Location == CardLocation.Hand)
            {
                bool hasLabrynthEngine = Bot.Hand.Any(IsLabrynthMonster) ||
                                         Bot.GetMonsters().Any(IsLabrynthMonster) ||
                                         Bot.Graveyard.Any(IsLabrynthMonster);

                bool isEmergencyLethal = (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep || Duel.Phase == DuelPhase.Damage) &&
                                         Enemy.GetMonsters().Where(m => m != null && m.IsAttack()).Sum(m => m.Attack) >= Bot.LifePoints;

                if (hasLabrynthEngine && !isEmergencyLethal)
                {
                    // Preserve Dominus Spark in hand to be set on field safely via Lady / Arias / Normal Set
                    return false;
                }
            }

            if (!SmartHandTrapChain()) return false;
            if (Enemy.GetMonsterCount() == 0) return false;

            _dominusSparkUsed = true;
            return true;
        }

        private bool AriasEffect()
        {
            if (ShouldSkipCombo()) return false;

            if (Card.Location == CardLocation.Hand)
            {
                if (_ariasHandUsed) return false;
                bool hasTarget = Bot.Hand.Any(c => c != null && c.Id != CardId.AriasTheLabrynthButler && (IsLabrynthMonster(c) || c.IsTrap()));
                if (!hasTarget) return false;
                _ariasHandUsed = true;
                return true;
            }

            if (Card.Location == CardLocation.Grave)
            {
                if (_ariasGyUsed) return false;
                if (IsSpecialSummonBlocked()) return false;
                if (Bot.GetMonsterCount() >= 5) return false;
                _ariasGyUsed = true;
                return true;
            }

            return false;
        }

        // --- ANGELECHY ARCHETYPE LOGIC ---

        private bool AngelechyOpeningEffect()
        {
            if (_openingUsed) return false;
            // Can be activated from hand during opp turn 1 Standby, or normal activation
            _openingUsed = true;
            return true;
        }

        private bool AngelechyProblemEffect()
        {
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                // Trigger effect: When an Angelechy monster is destroyed, recycle S/T monster to ED and SS it
                if (IsSpecialSummonBlocked()) return false;
                bool hasAngelechyInSt = Bot.GetSpells().Any(c => c != null && IsAngelechyCard(c) && c.IsMonster());
                if (hasAngelechyInSt) return true;

                // Ignition effect: Discard 1 S/T -> SS Lv2 Angelechy + Place Angelechy in S/T zone
                if (_problemUsed) return false;
                bool hasDiscard = Bot.Hand.Any(c => c != null && c.Id != CardId.AngelechyProblem && (c.IsSpell() || c.IsTrap()));
                if (!hasDiscard) return false;
                _problemUsed = true;
                return true;
            }

            // Normal activation from hand
            return true;
        }

        private bool AngelechyDisturbanceEffect()
        {
            // GY effect: Banish to search Angelechy S/T
            if (Card.Location == CardLocation.Grave)
            {
                if (_disturbanceGyUsed) return false;
                _disturbanceGyUsed = true;
                return true;
            }

            // On-field Trap activation: Give control of Angelechy monster to opp, negate adjacent cards
            if (_disturbanceUsed) return false;
            bool hasAngelechyMon = Bot.GetMonsters().Any(c => c != null && IsAngelechyCard(c));
            if (!hasAngelechyMon) return false;

            // Highly synergistic if we control Angelechy Enlisted: Enlisted control change returns to ED and SS a boss!
            _disturbanceUsed = true;
            return true;
        }

        private bool WitnessEffect()
        {
            if (_witnessUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            // In hand: SS if Synchro on field or GY
            if (Card.Location == CardLocation.Hand)
            {
                bool hasSynchro = Bot.GetMonsters().Any(c => c != null && c.HasType(CardType.Synchro)) ||
                                  Bot.Graveyard.Any(c => c != null && c.HasType(CardType.Synchro));
                if (!hasSynchro) return false;
                _witnessUsed = true;
                return true;
            }

            // On SS: Place up to 3 Synchros in S/T zone and spawn Arc Token
            if (Bot.GetSpellCount() < 5) return true;
            return false;
        }

        private bool ShatrangaEffect()
        {
            // S/T Zone trigger: When placed in S/T zone, search Angelechy Trap
            if (Card.Location == CardLocation.SpellZone) return true;

            // On field: Target 1 opp monster -> banish it
            if (Card.Location == CardLocation.MonsterZone)
            {
                return Enemy.GetMonsterCount() > 0;
            }
            return true;
        }

        private bool BastionEffect()
        {
            // S/T Zone trigger: When placed in S/T zone, place Shatranga from ED
            if (Card.Location == CardLocation.SpellZone) return true;

            // On field: Target 1 other card in this card's column -> banish it
            if (Card.Location == CardLocation.MonsterZone)
            {
                return Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
            }
            return true;
        }

        private bool DestrierEffect()
        {
            // S/T Zone trigger: When placed in S/T zone, search Angelechy Spell (Problem)
            if (Card.Location == CardLocation.SpellZone) return true;

            // On field: Target 1 opp monster in another column -> banish it
            if (Card.Location == CardLocation.MonsterZone)
            {
                return Enemy.GetMonsterCount() > 0;
            }
            return true;
        }

        private bool EnlistedEffect()
        {
            // Trigger effect: When control changes, return to ED and owner SS Angelechy monster from ED!
            // This is top-tier recursion. Always activate!
            if (Card.Location == CardLocation.Extra) return true;

            // On-field effect: Target 1 opp monster in adjacent column -> banish it & change control
            if (Card.Location == CardLocation.MonsterZone)
            {
                return Enemy.GetMonsterCount() > 0;
            }
            return true;
        }

        // --- LABRYNTH CORE LOGIC ---

        private bool LadyEffect()
        {
            if (ShouldSkipCombo()) return false;

            // Hand effect: Quick SS when Normal Trap / Labrynth card effect activates
            if (Card.Location == CardLocation.Hand)
            {
                if (_ladySummoned) return false;
                if (IsSpecialSummonBlocked()) return false;
                if (Bot.GetMonsterCount() >= 5) return false;
                _ladySummoned = true;
                return true;
            }

            // Field effect: Quick effect to set Normal Trap from Deck when Normal Trap activated
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_ladySetUsed) return false;
                if (Bot.GetSpellCount() >= 5) return false;
                _ladySetUsed = true;
                return true;
            }

            return false;
        }

        private bool LovelyEffect()
        {
            if (ShouldSkipCombo()) return false;

            // On-field trigger: When monster leaves field by Normal Trap -> pop 1 opp card
            if (!_lovelyDestroyUsed && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0 || Enemy.Hand.Count > 0))
            {
                _lovelyDestroyUsed = true;
                return true;
            }

            // Ignition: Set 1 Normal Trap from GY
            if (!_lovelySetUsed && Bot.GetSpellCount() < 5)
            {
                bool hasNormalTrapInGy = Bot.Graveyard.Any(c => c != null && c.IsTrap() && !c.HasType(CardType.Continuous) && !c.HasType(CardType.Counter));
                if (hasNormalTrapInGy)
                {
                    _lovelySetUsed = true;
                    return true;
                }
            }

            return false;
        }

        private bool AriannaEffect()
        {
            if (_ariannaUsed) return false;
            _ariannaUsed = true;
            return true;
        }

        private bool CooclockEffect()
        {
            if (_cooclockUsed) return false;
            bool hasSetTrap = Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsTrap());
            bool hasTrapInHand = Bot.Hand.Any(c => c != null && c.IsTrap());
            bool hasLabrynthMon = Bot.GetMonsters().Any(IsLabrynthMonster);

            if (!hasLabrynthMon) return false;
            if (!hasSetTrap && !hasTrapInHand) return false;

            _cooclockUsed = true;
            return true;
        }

        private bool StovieEffect()
        {
            // GY trigger: Monster leaves field by Normal Trap -> SS to field
            if (Card.Location == CardLocation.Grave)
            {
                if (_stovieGyUsed) return false;
                if (IsSpecialSummonBlocked()) return false;
                if (Bot.GetMonsterCount() >= 5) return false;
                _stovieGyUsed = true;
                return true;
            }

            // Hand effect: Send to GY + discard 1 card -> Set Welcome from Deck
            if (Card.Location == CardLocation.Hand)
            {
                if (_stovieHandUsed) return false;
                if (Bot.GetSpellCount() >= 5) return false;
                if (!Bot.Hand.Any(c => c != null && c.Id != CardId.LabrynthStovieTorbie)) return false;
                _stovieHandUsed = true;
                return true;
            }

            return false;
        }

        private bool ChandraglierEffect()
        {
            // GY trigger: Monster leaves field by Normal Trap -> Add to hand
            if (Card.Location == CardLocation.Grave)
            {
                if (_chandraglierGyUsed) return false;
                _chandraglierGyUsed = true;
                return true;
            }

            // Hand effect: Send to GY + discard 1 card -> Set Welcome from Deck
            if (Card.Location == CardLocation.Hand)
            {
                if (_chandraglierHandUsed) return false;
                if (Bot.GetSpellCount() >= 5) return false;
                if (!Bot.Hand.Any(c => c != null && c.Id != CardId.LabrynthChandraglier)) return false;
                _chandraglierHandUsed = true;
                return true;
            }

            return false;
        }

        private bool BigWelcomeEffect()
        {
            // GY effect: Banish to bounce
            if (Card.Location == CardLocation.Grave)
            {
                if (_bigWelcomeGyUsed) return false;
                bool hasLv8Fiend = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Level >= 8 && m.HasRace(CardRace.Fiend));
                if (hasLv8Fiend && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0))
                {
                    _bigWelcomeGyUsed = true;
                    return true;
                }
                return false;
            }

            // On-field Trap activation
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool WelcomeEffect()
        {
            // GY effect: When monster leaves field by Normal Trap -> Reset Welcome
            if (Card.Location == CardLocation.Grave)
            {
                return Bot.GetSpellCount() < 5;
            }

            // On-field Trap activation: SS Labrynth from Deck
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool TrapTrickEffect()
        {
            if (_trapTrickUsed) return false;
            if (Bot.GetSpellCount() >= 5) return false;
            _trapTrickUsed = true;
            return true;
        }

        private bool DDGroundEffect()
        {
            // Activate on opponent's turn to banish all GY materials
            return Duel.Player == 1;
        }

        private bool BlackGoatEffect()
        {
            if (Duel.Player != 1) return false;

            // Specifically designed for The Black Goat Laughs (49299410)
            // Identify key opponent cards from enemy field or GY
            int announceId = 0;
            var enemyMon = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.Attack >= 2000) ??
                            Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && c.Attack >= 2000);

            if (enemyMon != null)
            {
                announceId = enemyMon.Id;
            }
            else
            {
                announceId = CardId.AshBlossom;
            }

            AI.SelectAnnounceID(announceId);
            return true;
        }

        private bool ExtravaganceEffect()
        {
            if (_extravaganceUsed) return false;
            if (Duel.Phase != DuelPhase.Main1) return false;
            if (Bot.ExtraDeck.Count < 6) return false;
            _extravaganceUsed = true;
            return true;
        }

        // --- EXTRA DECK SUMMON CHECKS ---

        private bool ChaosAngelSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (HasLethalOnBoard()) return false;

            // Chaos Angel requires exact Level 10 Synchro (can treat 1 LIGHT/DARK as Tuner)
            // Available in Deck: Lv8 (Lady/Lovely/Bastion/Witness) + Lv2 (Enlisted/Stovie) = 10
            // or Lv7 (Destrier) + Lv3 (Chandraglier/Ash) = 10
            var faceupMonsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            bool hasLv8 = faceupMonsters.Any(c => c.Level == 8);
            bool hasLv2 = faceupMonsters.Any(c => c.Level == 2);
            bool hasLv7 = faceupMonsters.Any(c => c.Level == 7);
            bool hasLv3 = faceupMonsters.Any(c => c.Level == 3);

            bool canMakeL10 = (hasLv8 && hasLv2) || (hasLv7 && hasLv3);
            if (!canMakeL10) return false;

            // Protect aces if enemy board is already broken
            if (Enemy.GetMonsterCount() == 0 && Bot.GetMonsters().Count(IsAceCard) >= 1) return false;

            return true;
        }

        private bool AnimaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            bool hasLv1 = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Level == 1 && !c.HasType(CardType.Token));
            if (!hasLv1) return false;

            // EMZ columns are zone 1 (col 2) and zone 3 (col 4). Opponent zone facing EMZ:
            ClientCard oppFront1 = Enemy.MonsterZone[3]; // Opposite to EMZ 1
            ClientCard oppFront2 = Enemy.MonsterZone[1]; // Opposite to EMZ 2
            return (oppFront1 != null && oppFront1.IsFaceup()) || (oppFront2 != null && oppFront2.IsFaceup());
        }

        private bool BastionSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (HasLethalOnBoard()) return false;
            return true;
        }

        private bool ShatrangaSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (HasLethalOnBoard()) return false;
            return true;
        }

        private bool NullgainerSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (HasLethalOnBoard()) return false;
            return true;
        }

        private bool NullgainerEffect()
        {
            if (_nullgainerUsed) return false;
            _nullgainerUsed = true;
            return true;
        }

        private bool KewlTuneSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (HasLethalOnBoard()) return false;
            return true;
        }

        private bool KewlTuneEffect()
        {
            if (_kewlTuneUsed) return false;
            _kewlTuneUsed = true;
            return true;
        }

        // --- SUMMON HELPERS ---

        private bool AriannaSummon() => true;
        private bool AriasSummon() => Bot.GetMonsterCount() == 0;

        private bool IsLabrynthMonster(ClientCard c)
        {
            if (c == null) return false;
            return c.Id == CardId.LadyLabrynthOfTheSilverCastle ||
                   c.Id == CardId.LovelyLabrynthOfTheSilverCastle ||
                   c.Id == CardId.AriannaTheLabrynthServant ||
                   c.Id == CardId.AriasTheLabrynthButler ||
                   c.Id == CardId.LabrynthStovieTorbie ||
                   c.Id == CardId.LabrynthChandraglier ||
                   c.Id == CardId.LabrynthCooclock;
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

        // --- INTELLIGENT SELECTION OVERRIDES ---

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return base.OnSelectCard(cards, min, max, hint, cancelable);

            const long HINTMSG_RELEASE = 500;
            const long HINTMSG_DISCARD = 501;
            const long HINTMSG_DESTROY = 502;
            const long HINTMSG_REMOVE = 504;
            const long HINTMSG_RTOHAND = 505;
            const long HINTMSG_ATOHAND = 506;
            const long HINTMSG_TOGRAVE = 508;
            const long HINTMSG_SPSUMMON = 509;
            const long HINTMSG_SMATERIAL = 512;
            const long HINTMSG_XMATERIAL = 513;
            const long HINTMSG_TARGET = 551;
            const long HINTMSG_DISABLE = 552;
            const long HINTMSG_NEGATE = 572;

            // ════════════════════════════════════════════════════════════════
            // RULE 1: STRICT ENEMY TARGETING ON REMOVAL & DISRUPTION
            // Never allow hint 502/503/504 to target own cards!
            // ════════════════════════════════════════════════════════════════
            if (hint == HINTMSG_DESTROY || hint == HINTMSG_REMOVE || hint == 503 ||
                hint == HINTMSG_RTOHAND || hint == HINTMSG_TARGET ||
                hint == HINTMSG_DISABLE || hint == HINTMSG_NEGATE)
            {
                var enemyTargets = cards.Where(c => c != null && c.Controller == 1)
                    .OrderByDescending(c => GetCardThreatScore(c))
                    .ToList();

                if (enemyTargets.Count >= min)
                    return enemyTargets.Take(Math.Min(max, enemyTargets.Count)).ToList();
            }

            // ════════════════════════════════════════════════════════════════
            // 1. EXTRA DECK SELECTION & S/T ZONE PLACEMENT
            // (When Opening to e4, Problem, or Witness selects Synchros from Extra Deck)
            // ════════════════════════════════════════════════════════════════
            if (cards.Any(c => c != null && c.Location == CardLocation.Extra))
            {
                var extraCards = cards.Where(c => c != null && c.Location == CardLocation.Extra).ToList();

                // If prompt is for Special Summoning Lv 2: Enlisted
                if (extraCards.Any(c => c.Level == 2 && c.Id == CardId.AngelechyEnlisted))
                {
                    var enlisted = extraCards.Where(c => c.Id == CardId.AngelechyEnlisted).ToList();
                    if (enlisted.Count >= min) return enlisted.Take(min).ToList();
                }

                // If prompt is for Special Summoning Lv 7: Destrier
                if (extraCards.Any(c => c.Level == 7 && c.Id == CardId.AngelechyDestrier))
                {
                    var destrier = extraCards.Where(c => c.Id == CardId.AngelechyDestrier).ToList();
                    if (destrier.Count >= min) return destrier.Take(min).ToList();
                }

                // S/T Zone Placement Priority:
                // 1. Bastion (placed in S/T triggers Bastion to immediately place Shatranga!)
                // 2. Shatranga (limits opp to 5 activations + searches Trap)
                // 3. Destrier (inflicts 500 burn + searches Problem)
                var placementPriority = extraCards.OrderBy(c =>
                {
                    if (c.Id == CardId.AngelechyBastion && !Bot.HasInSpellZone(CardId.AngelechyBastion)) return 1;
                    if (c.Id == CardId.AngelechyShatranga && !Bot.HasInSpellZone(CardId.AngelechyShatranga)) return 2;
                    if (c.Id == CardId.AngelechyDestrier && !Bot.HasInSpellZone(CardId.AngelechyDestrier)) return 3;
                    if (c.Id == CardId.AngelechyBastion) return 4;
                    if (c.Id == CardId.AngelechyShatranga) return 5;
                    if (c.Id == CardId.AngelechyDestrier) return 6;
                    if (c.Id == CardId.AngelechyEnlisted) return 7;
                    return 50;
                }).ToList();

                if (placementPriority.Count >= min)
                    return placementPriority.Take(Math.Min(max, placementPriority.Count)).ToList();
            }

            // ════════════════════════════════════════════════════════════════
            // 2. BIG WELCOME BOUNCE TO HAND (Our Monster Zone)
            // ════════════════════════════════════════════════════════════════
            if (cards.All(c => c != null && c.Controller == 0 && c.Location == CardLocation.MonsterZone))
            {
                var bouncePriority = cards.OrderBy(c =>
                {
                    if (c.Id == CardId.LabrynthCooclock) return 1;
                    if (c.Id == CardId.AriannaTheLabrynthServant) return 2;
                    if (c.Id == CardId.AriasTheLabrynthButler) return 3;
                    if (c.Id == CardId.LabrynthStovieTorbie) return 4;
                    if (c.Id == CardId.LabrynthChandraglier) return 5;
                    if (c.Id == CardId.AngelechyEnlisted) return 6;
                    if (IsAceCard(c)) return 999;
                    return 50;
                }).ToList();

                if (bouncePriority.Count >= min)
                    return bouncePriority.Take(min).ToList();
            }

            // ════════════════════════════════════════════════════════════════
            // 3. SEARCH / ADD TO HAND (Hint 506 - HINTMSG_ATOHAND)
            // ════════════════════════════════════════════════════════════════
            if (hint == HINTMSG_ATOHAND)
            {
                var searchPriority = cards.OrderBy(c =>
                {
                    if (c.Id == CardId.BigWelcomeLabrynth) return 1;
                    if (c.Id == CardId.AngelechyProblem && !Bot.HasInHand(CardId.AngelechyProblem) && !Bot.HasInSpellZone(CardId.AngelechyProblem)) return 2;
                    if (c.Id == CardId.AngelechyDisturbance && !Bot.HasInHand(CardId.AngelechyDisturbance) && !Bot.HasInSpellZone(CardId.AngelechyDisturbance)) return 3;
                    if (c.Id == CardId.AriasTheLabrynthButler && !Bot.HasInHand(CardId.AriasTheLabrynthButler)) return 4;
                    if (c.Id == CardId.AngelechyOpeningToe4 && !Bot.HasInHand(CardId.AngelechyOpeningToe4)) return 5;
                    if (c.Id == CardId.LadyLabrynthOfTheSilverCastle && !Bot.HasInHand(CardId.LadyLabrynthOfTheSilverCastle) && !Bot.HasInMonstersZone(CardId.LadyLabrynthOfTheSilverCastle)) return 6;
                    if (c.Id == CardId.AriannaTheLabrynthServant) return 7;
                    if (c.Id == CardId.WelcomeLabrynth) return 8;
                    if (c.Id == CardId.DominusSpark) return 9;
                    if (c.Id == CardId.LabrynthCooclock) return 10;
                    if (c.Id == CardId.WitnessOfTheAncient) return 11;
                    return 50;
                }).ToList();

                if (searchPriority.Count >= min)
                    return searchPriority.Take(Math.Min(max, searchPriority.Count)).ToList();
            }

            // ════════════════════════════════════════════════════════════════
            // 4. LADY LABRYNTH SET TRAP FROM DECK (Hint 508 - HINTMSG_TOGRAVE / SET)
            // ════════════════════════════════════════════════════════════════
            if (hint == HINTMSG_TOGRAVE && cards.All(c => c != null && c.IsTrap() && c.Location == CardLocation.Deck))
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

                if (setPriority.Count >= min)
                    return setPriority.Take(Math.Min(max, setPriority.Count)).ToList();
            }

            // ════════════════════════════════════════════════════════════════
            // 5. DISCARD COSTS (Hint 501 - HINTMSG_DISCARD)
            // Strictly enforce our cards only, preserving core starters and aces
            // ════════════════════════════════════════════════════════════════
            if (hint == HINTMSG_DISCARD)
            {
                var discardPriority = cards.Where(c => c != null && c.Controller == 0)
                    .OrderBy(c =>
                    {
                        if (c.Id == CardId.LabrynthCooclock) return 1;
                        if (c.Id == CardId.LabrynthStovieTorbie && Bot.Hand.Count(h => h != null && h.Id == c.Id) > 1) return 2;
                        if (c.Id == CardId.LabrynthChandraglier && Bot.Hand.Count(h => h != null && h.Id == c.Id) > 1) return 3;
                        if (c.IsTrap() && Bot.Hand.Count(h => h != null && h.Id == c.Id) > 1) return 4;
                        if (c.Id == CardId.WitnessOfTheAncient) return 5;
                        if (c.Id == CardId.LabrynthStovieTorbie) return 6;
                        if (c.Id == CardId.LabrynthChandraglier) return 7;
                        if (IsAceCard(c)) return 999;
                        return 50;
                    }).ToList();

                if (discardPriority.Count >= min)
                    return discardPriority.Take(min).ToList();
            }

            // ════════════════════════════════════════════════════════════════
            // 6. SPECIAL SUMMON TARGET SELECTION (Hint 509 - HINTMSG_SPSUMMON)
            // ════════════════════════════════════════════════════════════════
            if (hint == HINTMSG_SPSUMMON)
            {
                var spPriority = cards.OrderByDescending(c =>
                {
                    if (c.Id == CardId.LadyLabrynthOfTheSilverCastle) return 100;
                    if (c.Id == CardId.LovelyLabrynthOfTheSilverCastle) return 95;
                    if (c.Id == CardId.AngelechyShatranga) return 90;
                    if (c.Id == CardId.AngelechyBastion) return 85;
                    if (c.Id == CardId.AngelechyDestrier) return 80;
                    if (c.Id == CardId.AngelechyEnlisted) return 75;
                    if (c.Id == CardId.AriannaTheLabrynthServant) return 70;
                    if (c.Id == CardId.AriasTheLabrynthButler) return 65;
                    return c.Attack;
                }).ToList();

                if (spPriority.Count >= min)
                    return spPriority.Take(Math.Min(max, spPriority.Count)).ToList();
            }

            // ════════════════════════════════════════════════════════════════
            // 7. MATERIAL & SACRIFICE FILTER (Exclude Aces)
            // ════════════════════════════════════════════════════════════════
            if (hint == HINTMSG_RELEASE || hint == HINTMSG_SMATERIAL || hint == HINTMSG_XMATERIAL)
            {
                var safeMaterials = cards.Where(c => !IsAceCard(c))
                    .OrderBy(c => GetMaterialPriority(c))
                    .ToList();

                if (safeMaterials.Count >= min)
                    return safeMaterials.Take(min).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0) return base.OnSelectOption(options);

            // Lovely Labrynth: Option for field card destruction vs random hand card
            // Prioritize guaranteed on-field removal when opponent controls cards
            if (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0)
            {
                return 1; // Target field card
            }

            return base.OnSelectOption(options);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Low-ATK utility cards & tokens: Force FaceUpDefence
            if (cardId == CardId.LabrynthCooclock ||
                cardId == CardId.LabrynthStovieTorbie ||
                cardId == CardId.WitnessOfTheAncient ||
                cardId == CardId.GlitchClutchNullgainer ||
                cardId == CardId.ArcToken)
            {
                if (positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence;
            }

            // Core beaters & aces: Force FaceUpAttack
            if (cardId == CardId.LadyLabrynthOfTheSilverCastle ||
                cardId == CardId.LovelyLabrynthOfTheSilverCastle ||
                cardId == CardId.AngelechyBastion ||
                cardId == CardId.AngelechyShatranga ||
                cardId == CardId.ChaosAngel ||
                cardId == CardId.KewlTuneB2B)
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
            // Column-aware placement for Angelechy Bastion and Angelechy Enlisted
            if (cardId == CardId.AngelechyBastion || cardId == CardId.AngelechyEnlisted)
            {
                for (int i = 0; i < 5; i++)
                {
                    int zoneMask = 1 << i;
                    if ((available & zoneMask) != 0)
                    {
                        // Match column with opponent's highest-threat monster
                        if (Enemy.MonsterZone[4 - i] != null || Enemy.SpellZone[4 - i] != null)
                            return zoneMask;
                    }
                }
            }

            return base.OnSelectPlace(cardId, player, location, available);
        }

        private bool MonsterRepos()
        {
            if (Card == null) return false;
            if (Card.IsAttack())
            {
                if (Enemy.GetMonsterCount() > 0 && Card.Attack < 1500 && !IsAceCard(Card)) return true;
            }
            else
            {
                if (Card.Attack >= 1500) return true;
            }
            return false;
        }
    }
}
