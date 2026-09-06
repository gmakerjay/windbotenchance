using YGOSharp.OCGWrapper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    // ====================================================================================================
    //  2026_Labrynth เนโฌโ€ ARCHETYPE STRATEGY & DECISION-TREE ENGINE EXECUTOR
    // ====================================================================================================
    //
    // เน€เธยเน€เธเธ’เน€เธเธเน€เธโฌเน€เธโ€เน€เธยเน€เธย: Labrynth + Dogmatika Control (2026) เนโฌโ€ เน€เธโฌเน€เธโ€เน€เธยเน€เธยเน€เธยเน€เธเธเน€เธยเน€เธยเน€เธโ€”เน€เธเธเน€เธเธ…เน€เธยเน€เธเธ‘เน€เธยเน€เธโ€เน€เธเธ‘เน€เธยเน€เธยเน€เธเธ…เน€เธเธเน€เธเธเน€เธยเน€เธยเน€เธยเน€เธเธ’เน€เธเธเน€เธยเน€เธโ€ Extra Deck เน€เธเธ…เน€เธยเน€เธเธเน€เธเธเน€เธเธเน€เธเธ’เน€เธย
    //
    // เน€เธยเน€เธเธ“เน€เธเธเน€เธยเน€เธเธ”เน€เธยเน€เธเธ’เน€เธเธเน€เธยเน€เธเธ…เน€เธเธเน€เธเธเน€เธโ€”เน€เธยเน€เธยเน€เธยเน€เธเธ…เน€เธเธเน€เธยเน€เธเธ’เน€เธเธเน€เธโ€”เน€เธเธ“เน€เธยเน€เธเธ’เน€เธยเน€เธเธเน€เธเธ…เน€เธเธ‘เน€เธย (Strategy Overview):
    //  1. Normal Trap Interactions (เน€เธยเน€เธเธ’เน€เธเธเน€เธโฌเน€เธเธ…เน€เธยเน€เธย Normal Trap เน€เธโฌเน€เธยเน€เธเธ—เน€เธยเน€เธเธเน€เธยเน€เธเธเน€เธเธเน€เธโ€ขเน€เธเธเน€เธยเน€เธยเน€เธยเน€เธเธเน€เธเธเน€เธยเน€เธย):
    //     เน€เธเธเน€เธเธเน€เธยเน€เธเธเน€เธโฌเน€เธโ€ขเน€เธเธเน€เธเธเน€เธย "Labrynth" เน€เธยเน€เธเธเน€เธยเน€เธโ€เน€เธยเน€เธเธเน€เธเธ‘เน€เธยเน€เธยเน€เธเธ…เน€เธยเน€เธเธเน€เธเธเน€เธยเน€เธเธเน€เธยเน€เธยเน€เธยเน€เธโฌเน€เธเธเน€เธเธ—เน€เธยเน€เธเธเน€เธเธเน€เธเธ•เน€เธเธเน€เธเธเน€เธยเน€เธเธเน€เธโฌเน€เธโ€ขเน€เธเธเน€เธเธเน€เธยเน€เธเธเน€เธเธเน€เธยเน€เธยเน€เธเธ’เน€เธยเน€เธเธเน€เธยเน€เธเธ’เน€เธเธเน€เธยเน€เธโ€เน€เธเธเน€เธยเน€เธเธ…เน€เธยเน€เธเธเน€เธย Normal Trap:
    //     - Lady Labrynth: เน€เธเธเน€เธเธ‘เน€เธยเน€เธโฌเน€เธยเน€เธเธ”เน€เธยเน€เธยเน€เธเธ”เน€เธโฌเน€เธเธเน€เธเธเน€เธยเน€เธเธ’เน€เธยเน€เธเธเน€เธเธ—เน€เธเธ, เน€เธโฌเน€เธยเน€เธยเน€เธโ€ข Normal Trap เน€เธยเน€เธโ€เน€เธเธเน€เธโ€ขเน€เธเธเน€เธยเน€เธยเน€เธเธ’เน€เธยเน€เธโฌเน€เธโ€เน€เธยเน€เธย
    //     - Lovely Labrynth: เน€เธโ€”เน€เธเธ“เน€เธเธ…เน€เธเธ’เน€เธเธเน€เธยเน€เธเธ’เน€เธเธเน€เธยเน€เธโ€เน€เธยเน€เธยเน€เธยเน€เธเธ”เน€เธเธ…เน€เธโ€เน€เธยเน€เธเธเน€เธเธเน€เธเธ—เน€เธเธเน€เธยเน€เธยเน€เธเธเน€เธเธ—เน€เธเธเน€เธยเน€เธเธเน€เธยเน€เธโ€ขเน€เธยเน€เธเธเน€เธเธเน€เธเธเน€เธย 1 เน€เธยเน€เธย, เน€เธโฌเน€เธยเน€เธยเน€เธโ€ข Normal Trap เน€เธยเน€เธเธ’เน€เธย GY
    //     - Arianna/Ariane: เน€เธยเน€เธเธ‘เน€เธยเน€เธเธเน€เธยเน€เธเธ’เน€เธเธเน€เธยเน€เธโ€ 1 เน€เธยเน€เธยเน€เธยเน€เธเธ…เน€เธเธเน€เธเธเน€เธเธ‘เน€เธยเน€เธโฌเน€เธยเน€เธเธ”เน€เธยเน€เธยเน€เธเธ”เน€เธโฌเน€เธเธเน€เธเธเน€เธเธเน€เธเธเน€เธเธ—เน€เธเธเน€เธโฌเน€เธยเน€เธยเน€เธโ€ขเน€เธยเน€เธเธ’เน€เธเธเน€เธยเน€เธโ€เน€เธยเน€เธเธ’เน€เธยเน€เธเธเน€เธเธ—เน€เธเธ
    //     - Furniture (Stovie Torbie/Chandraglier): เน€เธยเน€เธเธเน€เธยเน€เธโ€ขเน€เธเธ‘เน€เธเธเน€เธโฌเน€เธเธเน€เธยเน€เธเธเน€เธเธเน€เธเธ—เน€เธเธเน€เธเธเน€เธเธ•เน€เธยเน€เธยเน€เธโฌเน€เธยเน€เธเธ”เน€เธเธ…เน€เธยเน€เธเธ–เน€เธยเน€เธยเน€เธเธเน€เธเธ—เน€เธเธเน€เธยเน€เธเธ’เน€เธย GY
    //  2. Dogmatika Engine (เน€เธยเน€เธเธ’เน€เธเธเน€เธเธเน€เธยเน€เธยเน€เธยเน€เธเธ’เน€เธเธเน€เธยเน€เธโ€เน€เธยเน€เธเธ’เน€เธย Extra Deck เน€เธโฌเน€เธยเน€เธเธ—เน€เธยเน€เธเธเน€เธยเน€เธเธเน€เธยเน€เธยเน€เธเธเน€เธเธเน€เธยเน€เธเธเน€เธเธเน€เธยเน€เธโ€):
    //     - Dogmatika Ecclesia: เน€เธยเน€เธยเน€เธยเน€เธเธเน€เธเธ’เน€เธยเน€เธเธ’เน€เธเธเน€เธยเน€เธโ€ Dogmatika (Punishment, Fleurdelis, Quadogmatika Beast)
    //     - Dogmatika Fleurdelis: เน€เธโ€ขเน€เธเธ‘เน€เธเธเน€เธยเน€เธเธ‘เน€เธโ€เน€เธยเน€เธเธเน€เธเธ’เน€เธยเน€เธเธ…เน€เธยเน€เธเธ…เน€เธยเน€เธเธ’เน€เธยเน€เธโฌเน€เธเธเน€เธยเน€เธโฌเน€เธยเน€เธยเน€เธโ€ขเน€เธยเน€เธเธเน€เธเธเน€เธยเน€เธเธเน€เธโฌเน€เธโ€ขเน€เธเธเน€เธเธเน€เธยเน€เธยเน€เธยเน€เธเธเน€เธยเน€เธเธ’เน€เธเธเน€เธโฌเน€เธเธเน€เธเธ—เน€เธยเน€เธเธเน€เธเธเน€เธเธ• Extra Deck เน€เธเธเน€เธเธเน€เธยเน€เธเธเน€เธโฌเน€เธโ€ขเน€เธเธเน€เธเธเน€เธยเน€เธเธเน€เธเธเน€เธเธเน€เธย
    //     - Dogmatika Punishment: เน€เธโ€”เน€เธเธ“เน€เธเธ…เน€เธเธ’เน€เธเธเน€เธเธเน€เธเธเน€เธยเน€เธเธเน€เธโฌเน€เธโ€ขเน€เธเธเน€เธเธเน€เธยเน€เธยเน€เธเธเน€เธยเน€เธโ€ขเน€เธยเน€เธเธเน€เธเธเน€เธเธเน€เธยเน€เธยเน€เธโ€เน€เธเธเน€เธยเน€เธเธ’เน€เธเธเน€เธเธเน€เธยเน€เธยเน€เธยเน€เธเธ’เน€เธเธเน€เธยเน€เธโ€ Extra Deck (Garura, N'tss, Mouser) เน€เธเธ…เน€เธยเน€เธเธเน€เธเธเน€เธเธเน€เธเธ’เน€เธย
    //     - Quadogmatika Beast: เน€เธยเน€เธเธเน€เธยเน€เธยเน€เธเธ•เน€เธเธเน€เธเธ”เน€เธโ€ขเน€เธเธเน€เธเธเน€เธยเน€เธเธเน€เธโฌเน€เธโ€ขเน€เธเธเน€เธเธเน€เธยเน€เธยเน€เธเธ’เน€เธย GY/Banish เน€เธยเน€เธโ€เน€เธเธเน€เธยเน€เธเธ’เน€เธเธเน€เธเธเน€เธยเน€เธยเน€เธยเน€เธเธ’เน€เธเธเน€เธยเน€เธโ€ Extra Deck/Dogmatika เน€เธเธ…เน€เธย GY
    //
    // ====================================================================================================

    [Deck("2026_Labrynth", "2026_Labrynth")]
    public class _2026_LabrynthExecutor : ModernExecutor
    {
        public class CardId
        {
            // --- MAIN DECK ARCHETYPE CARDS ---
            public const int LadyLabrynthOfTheSilverCastle = 81497285;
            public const int LovelyLabrynthOfTheSilverCastle = 2347656;
            public const int AriasTheLabrynthButler = 73602965;
            public const int ArianeTheLabrynthServant = 75730490;
            public const int AriannaTheLabrynthServant = 1225009;
            public const int LabrynthChandraglier = 37629703;
            public const int LabrynthStovieTorbie = 74018812;
            public const int AbsoluteKingBackJack = 60990740;
            public const int BigWelcomeLabrynth = 92714517;
            public const int WelcomeLabrynth = 5380979;

            // --- DOGMATIKA ENGINE CARDS ---
            public const int DogmatikaFleurdelis = 73355772;
            public const int DogmatikaEcclesia = 60303688;
            public const int DogmatikaPunishment = 82956214;
            public const int QuadogmatikaBeast = 16693934;

            // --- STAPLES & OTHER TRAPS ---
            public const int LavaGolem = 102380;
            public const int AshBlossom = 14558127;
            public const int DrollAndLockBird = 94145021;
            public const int MulcharmyFuwalos = 42141493;
            public const int CalledByTheGrave = 24224830;
            public const int CrossoutDesignator = 65681983;
            public const int PotOfDuality = 98645731;
            
            public const int DestructiveDarumaKarmaCannon = 30748475;
            public const int IceDragonsPrison = 20899496;
            public const int TerrorsOfTheOverroot = 63086455;
            public const int WarningPoint = 11429811;
            public const int TrapTrick = 80101899;
            public const int DifferentDimensionGround = 31849106;
            public const int TransactionRollback = 6351147;
            public const int TheBlackGoatLaughs = 49299410;
            public const int InfiniteImpermanence = 10045474;

            // --- EXTRA DECK CARDS ---
            public const int ElderEntityNtss = 80532587;
            public const int Garura = 11765832;
            public const int Titaniklad = 41373230;
            public const int ChaosAngel = 22850702;
            public const int SuperStarslayerTYPHON = 93039339;
            public const int Vallon = 40673853;
            public const int Number60DugaresTheTimeless = 66011101;
            public const int BucephalusII = 10019086;
            public const int UnderworldGoddess = 98127546;
            public const int Dharc = 8264361;
            public const int Mouser = 33781156;
            public const int SPKnight = 29301450;
            public const int MuckrakerFromTheUnderworld = 71607202;
            public const int RelinquishedAnima = 94259633;
            public const int MereologicAggregator = 9940036;
        }

        // --- GOING FIRST/SECOND TRACKING ---

        // --- OPT FLAGS ---
        private bool _stovieUsed = false;
        private bool _chandraglierUsed = false;
        private bool _ariannaUsed = false;
        private bool _arianeUsed = false;
        private bool _ariasHandUsed = false;
        private bool _ariasGraveUsed = false;
        private bool _ladySummonedThisTurn = false;
        private bool _ladySetUsed = false;
        private bool _lovelySetUsed = false;
        private bool _lovelyDestroyUsed = false;
        private bool _backJackGraveUsed = false;
        private bool _dualityUsed = false;
        private bool _ecclesiaUsed = false;
        private bool _fleurdelisUsed = false;
        private bool _quadogmatikaUsed = false;
        private bool _trapTrickUsed = false;
        private bool _normalSummonedThisTurn = false;
        private bool _specialSummonedThisTurn = false;

        // --- ACE CARDS (MUST PROTECT) ---
        private static readonly int[] AceCardIds = {
            CardId.LadyLabrynthOfTheSilverCastle,
            CardId.LovelyLabrynthOfTheSilverCastle,
            CardId.ChaosAngel,
            CardId.SuperStarslayerTYPHON
        };

        // OpponentFloodgateCards removed เนโฌโ€ base ModernExecutor._spSummonBlockMonsters has comprehensive superset


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

        protected override bool ShouldStopExtending()
        {
            if (IsBoardStrongEnough())
                return base.ShouldStopExtending();
            return false;
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.IsCode(CardId.AbsoluteKingBackJack)) return 50;
            if (c.IsCode(CardId.LabrynthStovieTorbie) || c.IsCode(CardId.LabrynthChandraglier)) return 100;
            if (c.IsCode(CardId.ArianeTheLabrynthServant)) return 150;
            if (c.IsCode(CardId.DogmatikaEcclesia)) return 180;
            if (c.IsCode(CardId.AriannaTheLabrynthServant)) return 200;
            return base.GetMaterialPriority(c);
        }

        public _2026_LabrynthExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            ResourcePlan.RegisterAceCards(AceCardIds);
            // เนโ€โฌเนโ€โฌ Combo Router: Sequencing เนโ€โฌเนโ€โฌ
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Arianna-Welcome",
                RequiredCards = new List<int> { CardId.AriannaTheLabrynthServant, CardId.BigWelcomeLabrynth },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.AriannaTheLabrynthServant, ActionType = ExecutorType.Summon, Description = "Normal Summon Arianna" },
                    new() { CardId = CardId.AriannaTheLabrynthServant, ActionType = ExecutorType.Activate, Description = "Arianna search" },
                    new() { CardId = CardId.BigWelcomeLabrynth, ActionType = ExecutorType.SpellSet, Description = "Set Big Welcome" }
                },
                EndBoardScore = 80
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Arias-Welcome",
                RequiredCards = new List<int> { CardId.AriasTheLabrynthButler, CardId.BigWelcomeLabrynth },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.AriasTheLabrynthButler, ActionType = ExecutorType.Activate, Description = "Arias hand effect" },
                    new() { CardId = CardId.BigWelcomeLabrynth, ActionType = ExecutorType.SpellSet, Description = "Set Big Welcome via Arias" }
                },
                EndBoardScore = 85
            });

            // เนโ€โฌเนโ€โฌ Bait Planner เนโ€โฌเนโ€โฌ
            BaitPlanner.RegisterComboStarters(CardId.BigWelcomeLabrynth, CardId.WelcomeLabrynth);
            BaitPlanner.RegisterBaitCards(CardId.AriannaTheLabrynthServant, CardId.ArianeTheLabrynthServant, CardId.PotOfDuality);

            // เนโ€โฌเนโ€โฌ Chain Advisor เนโ€โฌเนโ€โฌ
            ChainAdvisor.RegisterHighValueTargets(CardId.BigWelcomeLabrynth, CardId.WelcomeLabrynth, CardId.TransactionRollback);

            // 1. Hand Traps / Quick Effects (opponent reactive / immediate)
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.CrossoutDesignator, CrossoutEffect);
            AddExecutor(ExecutorType.Activate, CardId.AriasTheLabrynthButler, AriasEffect);
            AddExecutor(ExecutorType.Activate, CardId.DogmatikaFleurdelis, FleurdelisEffect);
            AddExecutor(ExecutorType.Activate, CardId.LadyLabrynthOfTheSilverCastle, LadyEffect);
            AddExecutor(ExecutorType.Activate, CardId.LovelyLabrynthOfTheSilverCastle, LovelyEffect);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, InfiniteImpermanenceEffect);
            
            // 2. Normal Traps (highest priority triggers)
            AddExecutor(ExecutorType.Activate, CardId.BigWelcomeLabrynth, BigWelcomeEffect);
            AddExecutor(ExecutorType.Activate, CardId.WelcomeLabrynth, WelcomeEffect);
            AddExecutor(ExecutorType.Activate, CardId.DestructiveDarumaKarmaCannon, KarmaCannonEffect);
            AddExecutor(ExecutorType.Activate, CardId.IceDragonsPrison, IceDragonsPrisonEffect);
            AddExecutor(ExecutorType.Activate, CardId.TerrorsOfTheOverroot, OverrootEffect);
            AddExecutor(ExecutorType.Activate, CardId.WarningPoint, WarningPointEffect);
            AddExecutor(ExecutorType.Activate, CardId.DogmatikaPunishment, PunishmentEffect);
            AddExecutor(ExecutorType.Activate, CardId.TrapTrick, TrapTrickEffect);
            AddExecutor(ExecutorType.Activate, CardId.DifferentDimensionGround, DifferentDimensionGroundEffect);
            AddExecutor(ExecutorType.Activate, CardId.TransactionRollback, TransactionRollbackEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheBlackGoatLaughs, TheBlackGoatLaughsEffect);
            AddExecutor(ExecutorType.Activate, CardId.QuadogmatikaBeast, QuadogmatikaBeastEffect);

            // 3. Spells
            AddExecutor(ExecutorType.Activate, CardId.PotOfDuality, PotOfDualityEffect);

            // 4. Normal Summons / Furniture / Hand activations
            AddExecutor(ExecutorType.Activate, CardId.AriannaTheLabrynthServant, AriannaEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArianeTheLabrynthServant, ArianeEffect);
            AddExecutor(ExecutorType.Activate, CardId.LabrynthStovieTorbie, StovieTorbieEffect);
            AddExecutor(ExecutorType.Activate, CardId.LabrynthChandraglier, ChandraglierEffect);
            AddExecutor(ExecutorType.Activate, CardId.AbsoluteKingBackJack, BackJackEffect);
            
            // Lava Golem check first before normal summoning (which blocks Special Summons)
            AddExecutor(ExecutorType.SpSummon, CardId.LavaGolem, LavaGolemSummon);
            
            AddExecutor(ExecutorType.Summon, CardId.AriannaTheLabrynthServant, AriannaSummon);
            AddExecutor(ExecutorType.Summon, CardId.ArianeTheLabrynthServant, ArianeSummon);
            AddExecutor(ExecutorType.Summon, CardId.DogmatikaEcclesia, DogmatikaEcclesiaSummon);
            AddExecutor(ExecutorType.Summon, CardId.AbsoluteKingBackJack, BackJackSummon);
            
            // 5. Special Summons (Extra Deck & Bosses)
            AddExecutor(ExecutorType.SpSummon, CardId.ChaosAngel, ChaosAngelSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SPKnight, SPKnightSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.MuckrakerFromTheUnderworld, MuckrakerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Number60DugaresTheTimeless, DugaresSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SuperStarslayerTYPHON, TyPhonSummon);

            // Activate Ecclesia search after Extra Deck climbs to avoid ED lock
            AddExecutor(ExecutorType.Activate, CardId.DogmatikaEcclesia, DogmatikaEcclesiaEffect);
            
            // 6. Spell/Trap Sets (last resort for Traps)
            AddExecutor(ExecutorType.SpellSet, CardId.BigWelcomeLabrynth);
            AddExecutor(ExecutorType.SpellSet, CardId.WelcomeLabrynth);
            AddExecutor(ExecutorType.SpellSet, CardId.DogmatikaPunishment);
            AddExecutor(ExecutorType.SpellSet, CardId.DestructiveDarumaKarmaCannon);
            AddExecutor(ExecutorType.SpellSet, CardId.IceDragonsPrison);
            AddExecutor(ExecutorType.SpellSet, CardId.TerrorsOfTheOverroot);
            AddExecutor(ExecutorType.SpellSet, CardId.WarningPoint);
            AddExecutor(ExecutorType.SpellSet, CardId.TrapTrick);
            AddExecutor(ExecutorType.SpellSet, CardId.DifferentDimensionGround);
            AddExecutor(ExecutorType.SpellSet, CardId.TransactionRollback);
            AddExecutor(ExecutorType.SpellSet, CardId.TheBlackGoatLaughs);
            AddExecutor(ExecutorType.SpellSet, CardId.QuadogmatikaBeast);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence);

            // Always last: Repos & Attack
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // Labrynth is a trap control deck เนโฌโ€ strongly prefer going first to set traps
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _stovieUsed = false;
            _chandraglierUsed = false;
            _ariannaUsed = false;
            _arianeUsed = false;
            _ariasHandUsed = false;
            _ariasGraveUsed = false;
            _ladySummonedThisTurn = false;
            _ladySetUsed = false;
            _lovelySetUsed = false;
            _lovelyDestroyUsed = false;
            _backJackGraveUsed = false;
            _dualityUsed = false;
            _ecclesiaUsed = false;
            _fleurdelisUsed = false;
            _quadogmatikaUsed = false;
            _trapTrickUsed = false;
            _normalSummonedThisTurn = false;
            _specialSummonedThisTurn = false;

            // เนโ€โฌเนโ€โฌ Going-Second BreakBoard: prioritize disruption over combo เนโ€โฌเนโ€โฌ
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
            }
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
                    id == CardId.DogmatikaEcclesia ||
                    id == CardId.DogmatikaFleurdelis ||
                    id == CardId.QuadogmatikaBeast ||
                    id == CardId.TransactionRollback)
                {
                    _specialSummonedThisTurn = true;
                }
            }
        }

        // --- STRATEGIC CHECKS & HELPERS ---
        
        protected override bool IsSpecialSummonBlocked()
        {
            // Deck-specific: Pot of Duality lock
            if (_dualityUsed) return true;
            return base.IsSpecialSummonBlocked();
        }

        // EnemyHasActiveNegate เนยโ€ use base.OpponentHasActiveNegator() or EnemyHasKnownNegate()

        private bool IsLabrynthMonster(ClientCard c)
        {
            if (c == null) return false;
            return c.Id == CardId.LadyLabrynthOfTheSilverCastle
                || c.Id == CardId.LovelyLabrynthOfTheSilverCastle
                || c.Id == CardId.AriannaTheLabrynthServant
                || c.Id == CardId.ArianeTheLabrynthServant
                || c.Id == CardId.AriasTheLabrynthButler
                || c.Id == CardId.LabrynthStovieTorbie
                || c.Id == CardId.LabrynthChandraglier;
        }

        private bool IsNormalTrap(ClientCard c)
        {
            if (c == null) return false;
            return c.IsTrap() && !c.HasType(CardType.Continuous) && !c.HasType(CardType.Counter);
        }

        private bool HandHasDiscardTarget(int exceptCardId)
        {
            return Bot.Hand.Any(c => c != null && c.Id != exceptCardId);
        }

        private bool IsEDMonsterOnField()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsExtraCard())
                || Enemy.GetMonsters().Any(c => c != null && c.IsExtraCard());
        }

        private bool IsSafeToAttack(ClientCard attacker)
        {
            foreach (ClientCard enemy in Enemy.MonsterZone)
            {
                if (enemy == null || !enemy.IsFaceup()) continue;
                if (!enemy.IsDisabled())
                {
                    // Mekk-Knight Crusadia Avramax (21887175)
                    // If it battles a Special Summoned monster, it gains ATK equal to that monster's ATK during damage calc.
                    if (enemy.Id == 21887175 && attacker.IsSpecialSummoned)
                        return false;

                    // Crystal Wing Synchro Dragon (50954680)
                    // If it battles a Level 5 or higher monster, it gains ATK equal to that monster's ATK during damage calc.
                    if (enemy.Id == 50954680 && attacker.Level >= 5)
                        return false;
                }
                if (enemy.IsAttack())
                {
                    if (enemy.Attack > attacker.Attack) return false;
                    if (enemy.Attack == attacker.Attack)
                    {
                        // Allow trade if bot has equal or more monsters than enemy
                        if (Bot.GetMonsterCount() < Enemy.GetMonsterCount())
                            return false;
                    }
                }
                if (enemy.IsDefense() && attacker.Attack <= enemy.Defense) return false;
            }
            return true;
        }

        private bool IsSafeToDefend(ClientCard monster)
        {
            foreach (ClientCard enemy in Enemy.MonsterZone)
            {
                if (enemy == null || !enemy.IsFaceup()) continue;
                if (enemy.Attack > monster.Defense) return false;
            }
            return true;
        }

        private bool MonsterRepos()
        {
            if (Card == null) return false;

            bool enemyEmpty = Enemy.GetMonsterCount() == 0;

            if (Card.IsAttack())
            {
                if (!enemyEmpty && !IsSafeToAttack(Card) && IsSafeToDefend(Card))
                    return true;
            }
            else
            {
                if (enemyEmpty || IsSafeToAttack(Card))
                    return true;
            }
            return false;
        }

        private IList<ClientCard> SelectPreferred(IList<ClientCard> cards, int min, int max, params int[] preferredIds)
        {
            var result = new List<ClientCard>();
            foreach (int id in preferredIds)
            {
                var matches = cards.Where(c => c != null && c.Id == id && !result.Contains(c)).ToList();
                foreach (var m in matches)
                {
                    result.Add(m);
                    if (result.Count >= max) break;
                }
                if (result.Count >= max) break;
            }
            if (result.Count < min)
            {
                foreach (var card in cards)
                {
                    if (card != null && !result.Contains(card))
                    {
                        result.Add(card);
                        if (result.Count >= min) break;
                    }
                }
            }
            return result;
        }

        // --- HAND TRAPS & SP/TRAP RESOLUTION TRIGGERS ---

        private ClientCard GetPreemptiveImpermTarget()
        {
            int[] threatIds = {
                21522601, // Witchcrafter Madame Verre
                84523092, // Witchcrafter Haine
                1561110,  // ABC-Dragon Buster
                4280258,  // Apollousa, Bow of the Goddess
                10443957, // Cyber Dragon Infinity
                84815190, // Baronne de Fleur
                1508649   // Altergeist Hexstia
            };

            return Enemy.MonsterZone.GetMonsters().FirstOrDefault(c => 
                c != null && c.IsFaceup() && !c.IsDisabled() && 
                threatIds.Contains(c.Id) && 
                !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget());
        }

        private bool InfiniteImpermanenceEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;

            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                var target = GetPreemptiveImpermTarget();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }

            return DefaultInfiniteImpermanence();
        }

        private bool MulcharmyFuwalosEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }

        private bool DrollEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.LastChainPlayer == 1 && Duel.Player == 1;
        }

        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard == null || LastChainCard.Controller != 1) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool CalledByTheGraveEffect()
        {
            if (LastChainCard == null || LastChainCard.Controller != 1) return false;
            return DefaultCalledByTheGrave();
        }

        private bool CrossoutEffect()
        {
            // NEVER chain to own cards เนโฌโ€ Crossout crashes engine when mis-timed
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard == null || LastChainCard.Controller != 1) return false;

            // Resolve alias (alt-art handling) เนโฌโ€ critical for engine compatibility
            int code = LastChainCard.Id;
            int alias = LastChainCard.Alias;
            if (alias != 0 && alias - code < 10) code = alias;
            if (code == 0) return false;

            // Only negate monster effects (Crossout can't negate S/T effects practically)
            if (!LastChainCard.IsMonster()) return false;

            // Use StartingDeck-based count to verify we have a copy in deck
            if (GetRemainingCount(code) > 0)
            {
                AI.SelectAnnounceID(code);
                return true;
            }
            return false;
        }

        private bool AriasEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.Grave)
            {
                if (_ariasGraveUsed) return false;
                if (IsSpecialSummonBlocked()) return false;
                _ariasGraveUsed = true;
                return true;
            }
            else if (Card.Location == CardLocation.Hand)
            {
                if (_ariasHandUsed) return false;
                if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;

                if (Duel.Player == 0) // Our turn: prefer Special Summoning a Labrynth monster
                {
                    bool hasMonsterToSS = Bot.Hand.Any(c => c != null && c.Id != CardId.AriasTheLabrynthButler && IsLabrynthMonster(c));
                    if (!hasMonsterToSS) return false;
                }
                else // Opponent's turn: set a Trap or summon a monster
                {
                    bool hasTarget = Bot.Hand.Any(c => c != null && c.Id != CardId.AriasTheLabrynthButler && (IsLabrynthMonster(c) || IsNormalTrap(c)));
                    if (!hasTarget) return false;
                }

                _ariasHandUsed = true;
                return true;
            }
            return false;
        }

        private bool FleurdelisEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // ATK boost effect when Dogmatika monster declares attack. No phase restriction (it's Battle Phase)
                return true;
            }

            // Hand summoning effect
            if (_fleurdelisUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
            if (!IsEDMonsterOnField()) return false;

            if (Duel.Player == 1)
            {
                // Opponent's turn negation helper
                if (Enemy.GetMonsterCount() > 0)
                {
                    _fleurdelisUsed = true;
                    return true;
                }
                return false;
            }

            _fleurdelisUsed = true;
            return true;
        }

        private bool LadyEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (ActivateDescription == Util.GetStringId(CardId.LadyLabrynthOfTheSilverCastle, 0))
            {
                return LadySummonEffect();
            }
            if (ActivateDescription == Util.GetStringId(CardId.LadyLabrynthOfTheSilverCastle, 1))
            {
                return LadySetEffect();
            }
            return LadySummonEffect() || LadySetEffect();
        }

        private bool LadySummonEffect()
        {
            if (_ladySummonedThisTurn) return false;
            if (IsSpecialSummonBlocked()) return false;
            
            _ladySummonedThisTurn = true;
            return true;
        }

        private bool LadySetEffect()
        {
            if (_ladySetUsed) return false;
            if (LastChainCard == null || !IsNormalTrap(LastChainCard)) return false;

            _ladySetUsed = true;
            return true;
        }

        private bool LovelyEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (ActivateDescription == Util.GetStringId(CardId.LovelyLabrynthOfTheSilverCastle, 0))
            {
                return LovelySetEffect();
            }
            if (ActivateDescription == Util.GetStringId(CardId.LovelyLabrynthOfTheSilverCastle, 1))
            {
                return LovelyDestroyEffect();
            }
            return LovelyDestroyEffect() || LovelySetEffect();
        }

        private bool LovelyDestroyEffect()
        {
            if (_lovelyDestroyUsed) return false;
            _lovelyDestroyUsed = true;
            return true;
        }

        private bool LovelySetEffect()
        {
            if (_lovelySetUsed) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;

            bool hasTrapInGrave = Bot.Graveyard.Any(c => c != null && IsNormalTrap(c));
            if (!hasTrapInGrave) return false;

            _lovelySetUsed = true;
            return true;
        }

        private bool AriannaEffect()
        {
            if (_ariannaUsed) return false;
            _ariannaUsed = true;
            return true;
        }

        private bool ArianeEffect()
        {
            if (_arianeUsed) return false;
            
            if (ActivateDescription == Util.GetStringId(CardId.ArianeTheLabrynthServant, 0))
            {
                bool hasTrap = Bot.Hand.Any(c => c != null && IsNormalTrap(c))
                    || Bot.GetSpells().Any(c => c != null && IsNormalTrap(c) && c.IsFacedown());
                if (!hasTrap) return false;
                
                // Check if any Level เนยเธ4 Fiend monster remains in deck (excluding Ariane)
                bool hasFiend = false;
                if (StartingDeck != null)
                {
                    foreach (int deckId in StartingDeck.Cards)
                    {
                        if (deckId == CardId.ArianeTheLabrynthServant) continue;
                        var card = YGOSharp.OCGWrapper.NamedCard.Get(deckId);
                        if (card != null && card.Level <= 4 && (card.Race & (int)CardRace.Fiend) != 0
                            && GetRemainingCount(deckId) > 0)
                        {
                            hasFiend = true;
                            break;
                        }
                    }
                }
                if (!hasFiend) return false;
            }
            
            _arianeUsed = true;
            return true;
        }

        private bool DogmatikaEcclesiaEffect()
        {
            if (ActivateDescription == Util.GetStringId(CardId.DogmatikaEcclesia, 0))
            {
                if (IsSpecialSummonBlocked()) return false;
                if (!IsEDMonsterOnField()) return false;
                return true;
            }
            if (ActivateDescription == Util.GetStringId(CardId.DogmatikaEcclesia, 1))
            {
                if (_ecclesiaUsed) return false;
                _ecclesiaUsed = true;
                return true;
            }
            return true;
        }

        // --- NORMAL TRAPS EFFECTS ---

        private bool BigWelcomeEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // GY effect: banish to bounce a Fiend we control and return an opponent's card.
                bool hasFiend = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Fiend));
                bool oppHasCards = Enemy.GetFieldCount() > 0;
                return hasFiend && oppHasCards;
            }

            // Defer if opponent has active negation and we have board-breaking traps set to clear them first
            if (Duel.Player == 1 && Util.OpponentHasNegation())
            {
                bool hasBoardBreaker = Bot.SpellZone.Any(c => c != null && c.IsFacedown() && 
                    (c.Id == CardId.DestructiveDarumaKarmaCannon || c.Id == CardId.DifferentDimensionGround));
                if (hasBoardBreaker)
                    return false;
            }

            // On our turn, only activate in Main Phase to allow Normal Summoning first (preventing boss self-bounce)
            if (Duel.Player == 0 && Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2)
                return false;

            return true;
        }

        private bool WelcomeEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }

            // Defer if opponent has active negation and we have board-breaking traps set to clear them first
            if (Duel.Player == 1 && Util.OpponentHasNegation())
            {
                bool hasBoardBreaker = Bot.SpellZone.Any(c => c != null && c.IsFacedown() && 
                    (c.Id == CardId.DestructiveDarumaKarmaCannon || c.Id == CardId.DifferentDimensionGround));
                if (hasBoardBreaker)
                    return false;
            }

            // On our turn, only activate in Main Phase
            if (Duel.Player == 0 && Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2)
                return false;

            return true;
        }

        private bool PunishmentEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;

            var targets = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() 
                && !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget()).ToList();
            if (targets.Count == 0) return false;

            var target = targets.OrderByDescending(c => Scorer != null ? Scorer.ThreatScore(c) : c.Attack).FirstOrDefault();
            if (target == null) return false;

            bool hasEDTarget = false; // ED cards face-down เนโฌโ€ skip pre-check, let engine filter
            if (!hasEDTarget) return false;

            AI.SelectCard(target);
            return true;
        }

        private bool KarmaCannonEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Enemy.GetMonsterCount() == 0) return false;

            // Protect against direct attacks on opponent's turn
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle)) return true;

            // Excellent if opponent has Link monsters (sent directly to GY)
            bool oppHasLink = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Link));
            if (oppHasLink) return true;

            int faceupEnemyCount = Enemy.GetMonsters().Count(c => c != null && c.IsFaceup());
            if (faceupEnemyCount >= 2) return true;

            // If only 1 monster, only activate in response to opponent's activation
            if (faceupEnemyCount == 1 && LastChainCard != null && LastChainCard.Controller == 1) return true;

            return false;
        }

        private bool IceDragonsPrisonEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Enemy.Graveyard.Count(c => c.IsMonster()) == 0) return false;
            if (Enemy.GetMonsterCount() == 0) return false;

            // Check race compatibility: Opponent must have a faceup monster on the field 
            // that shares a Race with a monster in their Graveyard.
            var oppFieldRaces = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).Select(c => c.Race).ToList();
            bool hasRaceMatch = Enemy.Graveyard.Any(c => c != null && c.IsMonster() && oppFieldRaces.Contains(c.Race));
            
            return hasRaceMatch;
        }

        private bool OverrootEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Enemy.GetMonsterCount() == 0 && Enemy.GetSpells().Count(c => c.IsFaceup()) == 0) return false;
            if (Enemy.Graveyard.Count == 0) return false;
            return true;
        }

        private bool WarningPointEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return true;
        }

        private bool TrapTrickEffect()
        {
            if (_trapTrickUsed) return false;
            if (Duel.Player != 1) return false; // Opponent's turn only to avoid locking ourselves on our turn
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;

            _trapTrickUsed = true;
            return true;
        }

        private bool DifferentDimensionGroundEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.Player == 1; // Opponent's turn GY lock
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

        private bool TheBlackGoatLaughsEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            
            int announceId = 0;

            // 1. Check opponent's GY for monsters, prioritizing Extra Deck monsters or high ATK monsters
            var enemyGYMonsters = Enemy.Graveyard.Where(c => c != null && c.IsMonster()).ToList();
            if (enemyGYMonsters.Count > 0)
            {
                var target = enemyGYMonsters
                    .OrderByDescending(c => c.IsExtraCard() ? 1000 : 0)
                    .ThenByDescending(c => c.Attack)
                    .FirstOrDefault();
                if (target != null)
                {
                    announceId = target.Id;
                }
            }

            // 2. If nothing found in GY, check opponent's faceup monsters on the field
            if (announceId == 0)
            {
                var enemyMon = Enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup())
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();
                if (enemyMon != null)
                {
                    announceId = enemyMon.Id;
                }
            }

            // 3. Fallback
            if (announceId == 0)
            {
                announceId = CardId.AshBlossom; // Default to Ash Blossom
            }

            AI.SelectAnnounceID(announceId);
            return true;
        }

        private bool QuadogmatikaBeastEffect()
        {
            if (_quadogmatikaUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;

            // Targets 1 "Dogmatika" monster in GY
            var gyMonsters = Bot.Graveyard.Where(c => c != null && (c.Id == CardId.DogmatikaEcclesia || c.Id == CardId.DogmatikaFleurdelis || c.Id == CardId.QuadogmatikaBeast)).ToList();
            if (gyMonsters.Count == 0) return false;

            // ED pre-check skipped เนโฌโ€ cards in ED are face-down with 0 ATK

            _quadogmatikaUsed = true;
            return true;
        }

        // --- SPELLS & FURNITURE EFFECTS ---

        private bool PotOfDualityEffect()
        {
            if (_dualityUsed) return false;
            if (_specialSummonedThisTurn) return false;

            // Do not use if we want to Special Summon this turn
            bool hasSpecialSummonPlay = Bot.Hand.Any(c => c != null && 
                (c.Id == CardId.WelcomeLabrynth || 
                 c.Id == CardId.BigWelcomeLabrynth || 
                 c.Id == CardId.AriasTheLabrynthButler || 
                 c.Id == CardId.LadyLabrynthOfTheSilverCastle ||
                 c.Id == CardId.DogmatikaFleurdelis));

            if (hasSpecialSummonPlay) return false;

            // Check if we have active monsters on field that can be used for Extra Deck climb
            if (Bot.GetMonsterCount() >= 2 && !IsSpecialSummonBlocked()) return false;

            _dualityUsed = true;
            return true;
        }

        private bool StovieTorbieEffect()
        {
            if (_stovieUsed) return false;
            if (Card.Location == CardLocation.Grave)
            {
                if (IsSpecialSummonBlocked()) return false;
                _stovieUsed = true;
                _specialSummonedThisTurn = true;
                return true;
            }
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.MonsterZone)
            {
                if (EnemyHasKnownNegate()) return false;
                if (!HandHasDiscardTarget(CardId.LabrynthStovieTorbie)) return false;
                _stovieUsed = true;
                return true;
            }
            return false;
        }

        private bool ChandraglierEffect()
        {
            if (_chandraglierUsed) return false;
            if (Card.Location == CardLocation.Grave)
            {
                _chandraglierUsed = true;
                return true;
            }
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.MonsterZone)
            {
                if (EnemyHasKnownNegate()) return false;
                if (!HandHasDiscardTarget(CardId.LabrynthChandraglier)) return false;
                _chandraglierUsed = true;
                return true;
            }
            return false;
        }

        private bool BackJackEffect()
        {
            if (_backJackGraveUsed) return false;
            if (Card.Location != CardLocation.Grave) return false;
            if (Duel.Player != 1) return false; // Banish only on opponent's turn

            _backJackGraveUsed = true;
            return true;
        }

        private bool DogmatikaEcclesiaSummon()
        {
            if (Duel.Player != 0) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
            if (_normalSummonedThisTurn) return false;

            // Ecclesia locks Extra Deck. If we can make other ED summons, do them first.
            if (CanExtraDeckClimb()) return false;

            _normalSummonedThisTurn = true;
            return true;
        }

        private bool AriannaSummon()
        {
            if (_normalSummonedThisTurn) return false;
            _normalSummonedThisTurn = true;
            return true;
        }

        private bool ArianeSummon()
        {
            if (_normalSummonedThisTurn) return false;
            _normalSummonedThisTurn = true;
            return true;
        }

        private bool BackJackSummon()
        {
            if (_normalSummonedThisTurn) return false;
            _normalSummonedThisTurn = true;
            return true;
        }

        private bool CanExtraDeckClimb()
        {
            if (IsSpecialSummonBlocked()) return false;
            
            int ourMonsters = Bot.GetMonsterCount();
            if (ourMonsters >= 2) return true;

            if (ourMonsters == 1 && Bot.Hand.Any(c => c != null && c.Id == CardId.LadyLabrynthOfTheSilverCastle && !_ladySummonedThisTurn))
                return true;

            return false;
        }

        // --- SUMMONING EXTRA DECK & BOSS MONSTERS ---

        private bool LavaGolemSummon()
        {
            if (_normalSummonedThisTurn) return false;
            if (Enemy.GetMonsterCount() < 2) return false;
            
            // Check if we have normal summons in hand that we want to prioritize
            bool hasNormalSummonInHand = Bot.Hand.Any(c => c != null && (c.Id == CardId.DogmatikaEcclesia || c.Id == CardId.AriannaTheLabrynthServant));
            if (hasNormalSummonInHand && Bot.GetMonsterCount() == 0 && Enemy.GetMonsterCount() < 3)
                return false;

            bool hasThreat = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && 
                (!c.IsDisabled() && (c.Attack >= 2500 || c.IsFloodgate() || c.IsExtraCard())));

            if (hasThreat)
            {
                _normalSummonedThisTurn = true;
                _specialSummonedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool ChaosAngelSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            _specialSummonedThisTurn = true;
            return true;
        }

        private bool SPKnightSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            bool shouldSummon = Enemy.GetMonsterCount() > 0;
            if (shouldSummon)
            {
                _specialSummonedThisTurn = true;
            }
            return shouldSummon;
        }

        private bool MuckrakerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.Hand.Count == 0) return false;

            bool hasFiendToRevive = Bot.Graveyard.Any(c => c.IsMonster() && c.IsCanRevive() && 
                (c.Id == CardId.LovelyLabrynthOfTheSilverCastle || c.Id == CardId.LadyLabrynthOfTheSilverCastle || c.Id == CardId.AriannaTheLabrynthServant));

            if (hasFiendToRevive)
            {
                _specialSummonedThisTurn = true;
            }
            return hasFiendToRevive;
        }

        private bool DugaresSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            bool hasRevive = Bot.Graveyard.Any(c => c.IsMonster() && c.IsCanRevive() && 
                (c.Id == CardId.LovelyLabrynthOfTheSilverCastle || c.Id == CardId.LadyLabrynthOfTheSilverCastle));

            if (hasRevive)
            {
                _specialSummonedThisTurn = true;
            }
            return hasRevive;
        }

        private bool TyPhonSummon()
        {
            if (IsSpecialSummonBlocked()) return false;

            bool oppHasThreat = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && 
                (c.Attack >= 3000 || c.IsExtraCard()));
                
            if (!oppHasThreat) return false;

            bool hasOurBoss = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && 
                (c.Id == CardId.LadyLabrynthOfTheSilverCastle || c.Id == CardId.LovelyLabrynthOfTheSilverCastle || c.Id == CardId.ChaosAngel));

            if (hasOurBoss && Bot.GetMonsterCount() > Enemy.GetMonsterCount())
                return false;

            _specialSummonedThisTurn = true;
            return true;
        }

        // --- CARD / POSITION / OPTION OVERRIDES ---

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // 1. Extra Deck Sends (Dogmatika Punishment / Quadogmatika Beast Extra send)
            if (cards.Count > 0 && cards.All(c => c != null && c.Location == CardLocation.Extra))
            {
                bool oppHasCards = Enemy.GetFieldCount() > 0;
                
                bool hasFaceupDestructionImmune = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.IsShouldNotBeTarget() || c.IsShouldNotBeSpellTrapTarget() || c.Id == 21887175 || c.Id == 50954680));
                bool hasFacedownCard = Enemy.GetSpells().Any(c => c != null && c.IsFacedown()) || Enemy.GetMonsters().Any(c => c != null && c.IsFacedown());
                bool hasFaceupMonsterToFlip = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled() && !c.HasType(CardType.Link) && !c.HasType(CardType.Token));

                var extraList = cards.OrderByDescending(c => {
                    if (c.Id == CardId.ElderEntityNtss)
                    {
                        return (oppHasCards && !hasFaceupDestructionImmune) ? 100 : 40;
                    }
                    if (c.Id == CardId.Garura)
                    {
                        return oppHasCards ? 90 : 100;
                    }
                    if (c.Id == CardId.MereologicAggregator)
                    {
                        return hasFaceupDestructionImmune ? 110 : 75;
                    }
                    if (c.Id == CardId.Mouser)
                    {
                        return hasFaceupMonsterToFlip ? 95 : 65;
                    }
                    if (c.Id == CardId.Vallon)
                    {
                        return hasFacedownCard ? 92 : 60;
                    }
                    if (c.Id == CardId.Titaniklad)
                    {
                        bool hasEcclesiaInDeck = GetRemainingCount(CardId.DogmatikaEcclesia) > 0 || GetRemainingCount(CardId.DogmatikaFleurdelis) > 0;
                        return hasEcclesiaInDeck ? 80 : 30;
                    }
                    if (c.Id == CardId.BucephalusII)
                    {
                        return 50;
                    }
                    return 10;
                }).ToList();
                if (extraList.Count >= min) return extraList.Take(max).ToList();
            }

            // 2. Ice Dragon's Prison Banish (Field resolution: select 1 card from each field of same race)
            var oppFieldIDP = cards.Where(c => c != null && c.Controller == 1 && c.Location == CardLocation.MonsterZone).ToList();
            var ourFieldIDP = cards.Where(c => c != null && c.Controller == 0 && c.Location == CardLocation.MonsterZone).ToList();
            if (oppFieldIDP.Count > 0 && ourFieldIDP.Count > 0 && hint != 502) // ensure not generic destroy
            {
                foreach (var enemyMon in oppFieldIDP)
                {
                    var match = ourFieldIDP.FirstOrDefault(c => c.Race == enemyMon.Race);
                    if (match != null)
                    {
                        if (max >= 2)
                        {
                            return new List<ClientCard> { match, enemyMon };
                        }
                        else
                        {
                            return new List<ClientCard> { enemyMon };
                        }
                    }
                }
            }

            // 3. Terrors of the Overroot (Field Targeting: select 1 opponent card on field to send to GY)
            if (cards.Count > 0 && cards.All(c => c != null && c.Controller == 1 && (c.Location == CardLocation.MonsterZone || c.Location == CardLocation.SpellZone)) && hint != 502 && hint != 505)
            {
                var oppFieldOverroot = cards.OrderByDescending(c => {
                    if (Scorer != null) return Scorer.ThreatScore(c);
                    if (c.IsMonster())
                    {
                        if (c.IsExtraCard()) return 1000 + c.Attack;
                        return 500 + c.Attack;
                    }
                    if (c.IsSpell() || c.IsTrap())
                    {
                        if (c.HasType(CardType.Continuous) || c.HasType(CardType.Field)) return 600;
                        return 100;
                    }
                    return 10;
                }).ToList();
                if (oppFieldOverroot.Count >= min) return oppFieldOverroot.Take(max).ToList();
            }

            // 4. Terrors of the Overroot (GY Targeting: select 1 opponent card in GY to set)
            if (cards.Count > 0 && cards.All(c => c != null && c.Controller == 1 && c.Location == CardLocation.Grave) && hint != 509)
            {
                var oppGraveOverroot = cards.OrderBy(c => {
                    if (c.IsSpell() || c.IsTrap())
                    {
                        if (!c.HasType(CardType.Continuous) && !c.HasType(CardType.Field)) return 10;
                        return 50;
                    }
                    if (c.IsMonster()) return 100 + c.Attack;
                    return 200;
                }).ToList();
                if (oppGraveOverroot.Count >= min) return oppGraveOverroot.Take(max).ToList();
            }

            // 5. Ice Dragon's Prison (Summon from Opponent's GY)
            bool allOppGrave = cards.Count > 0 && cards.All(c => c != null && c.Controller == 1 && c.Location == CardLocation.Grave);
            if (allOppGrave && hint == 509)
            {
                var enemyTypes = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).Select(c => c.Race).ToList();
                var target = cards.OrderByDescending(c => enemyTypes.Contains(c.Race) ? 100 : 10).FirstOrDefault();
                if (target != null) return new List<ClientCard> { target };
            }

            // 6. Transaction Rollback / GY Traps (Copy Trap in GY)
            bool allGYTraps = cards.Count > 0 && cards.All(c => c != null && c.IsTrap() && c.Location == CardLocation.Grave);
            if (allGYTraps)
            {
                return SelectPreferred(cards, min, max,
                    CardId.BigWelcomeLabrynth,
                    CardId.WelcomeLabrynth,
                    CardId.DogmatikaPunishment,
                    CardId.DestructiveDarumaKarmaCannon,
                    CardId.IceDragonsPrison,
                    CardId.TerrorsOfTheOverroot,
                    CardId.WarningPoint
                );
            }

            // 7. Dogmatika GY Special Summon (Quadogmatika Beast GY revive)
            bool isDogmatikaGYSS = cards.Count > 0 && cards.All(c => c != null && c.Location == CardLocation.Grave && (c.Id == CardId.DogmatikaFleurdelis || c.Id == CardId.DogmatikaEcclesia || c.Id == CardId.QuadogmatikaBeast));
            if (isDogmatikaGYSS && hint == 509)
            {
                return SelectPreferred(cards, min, max,
                    CardId.DogmatikaFleurdelis,
                    CardId.DogmatikaEcclesia,
                    CardId.QuadogmatikaBeast
                );
            }

            // 8. Special Summon / Revival from GY/Hand/Deck (General 509: Welcome, Big Welcome, Muckraker, Dugares, etc.)
            if (hint == 509)
            {
                // If the resolving card is Big Welcome (or Transaction Rollback copying Big Welcome) and we control no monsters,
                // we will be forced to bounce the summoned monster. So we should NOT summon boss monsters.
                bool isBigWelcomeResolving = false;
                if (LastChainCard != null)
                {
                    if (LastChainCard.Id == CardId.BigWelcomeLabrynth)
                    {
                        isBigWelcomeResolving = true;
                    }
                    else if (LastChainCard.Id == CardId.TransactionRollback)
                    {
                        isBigWelcomeResolving = Bot.Graveyard.Any(c => c != null && c.Id == CardId.BigWelcomeLabrynth);
                    }
                }

                if (isBigWelcomeResolving && Bot.GetMonsterCount() == 0)
                {
                    // Prioritize low-level monsters to return them to hand (adding them to hand as a search/recycle play)
                    return SelectPreferred(cards, min, max,
                        CardId.AriannaTheLabrynthServant,
                        CardId.AriasTheLabrynthButler,
                        CardId.LabrynthStovieTorbie,
                        CardId.LabrynthChandraglier,
                        CardId.ArianeTheLabrynthServant,
                        CardId.LovelyLabrynthOfTheSilverCastle,
                        CardId.LadyLabrynthOfTheSilverCastle
                    );
                }

                // If options contain GY cards, prioritize boss monsters or Fleurdelis
                bool hasGYTarget = cards.Any(c => c != null && c.Location == CardLocation.Grave);
                if (hasGYTarget)
                {
                    return SelectPreferred(cards, min, max,
                        CardId.LovelyLabrynthOfTheSilverCastle,
                        CardId.LadyLabrynthOfTheSilverCastle,
                        CardId.DogmatikaFleurdelis,
                        CardId.AriannaTheLabrynthServant,
                        CardId.AriasTheLabrynthButler,
                        CardId.ArianeTheLabrynthServant,
                        CardId.LabrynthStovieTorbie,
                        CardId.LabrynthChandraglier
                    );
                }

                // General Special Summon (Welcome, Big Welcome, etc.)
                return SelectPreferred(cards, min, max,
                    CardId.LovelyLabrynthOfTheSilverCastle,
                    CardId.LadyLabrynthOfTheSilverCastle,
                    CardId.AriannaTheLabrynthServant,
                    CardId.AriasTheLabrynthButler,
                    CardId.ArianeTheLabrynthServant,
                    CardId.LabrynthStovieTorbie,
                    CardId.LabrynthChandraglier
                );
            }

            // 9. Return to Hand (Bouncing 505: Big Welcome, CED, etc.)
            if (hint == 505)
            {
                // Bouncing our own card (Big Welcome, etc.)
                bool allOurMonster = cards.Count > 0 && cards.All(c => c != null && c.Controller == 0 && c.Location == CardLocation.MonsterZone);
                if (allOurMonster)
                {
                    return SelectPreferred(cards, min, max,
                        CardId.AbsoluteKingBackJack,
                        CardId.AriannaTheLabrynthServant,
                        CardId.LabrynthStovieTorbie,
                        CardId.LabrynthChandraglier,
                        CardId.ArianeTheLabrynthServant,
                        CardId.AriasTheLabrynthButler,
                        CardId.DogmatikaEcclesia,
                        CardId.LovelyLabrynthOfTheSilverCastle,
                        CardId.LadyLabrynthOfTheSilverCastle
                    );
                }

                // Bouncing opponent's card
                var enemyBounces = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyBounces.Count > 0)
                {
                    return enemyBounces.OrderByDescending(c => {
                        if (c.IsMonster() && c.IsFaceup()) return 500 + c.Attack;
                        if (c.IsSpell() || c.IsTrap()) return 100;
                        return 10;
                    }).Take(max).ToList();
                }
            }

            // 10. Set normal trap (510: Lady Labrynth, Arias, Trap Trick, etc.)
            if (hint == 510)
            {
                return SelectPreferred(cards, min, max,
                    CardId.BigWelcomeLabrynth,
                    CardId.DogmatikaPunishment,
                    CardId.DestructiveDarumaKarmaCannon,
                    CardId.IceDragonsPrison,
                    CardId.TerrorsOfTheOverroot,
                    CardId.TheBlackGoatLaughs,
                    CardId.WarningPoint,
                    CardId.WelcomeLabrynth
                );
            }

            // 11. Search / Draw card (506 or 0: Arianna, Pot of Duality, Dogmatika Ecclesia, etc.)
            if (hint == 506 || hint == 0)
            {
                // Subcase: Dogmatika search
                bool isDogmatikaSearch = cards.Count > 0 && cards.All(c => c != null && (c.Id == CardId.DogmatikaPunishment || c.Id == CardId.DogmatikaFleurdelis || c.Id == CardId.QuadogmatikaBeast));
                if (isDogmatikaSearch)
                {
                    return SelectPreferred(cards, min, max,
                        CardId.DogmatikaPunishment,
                        CardId.DogmatikaFleurdelis,
                        CardId.QuadogmatikaBeast
                    );
                }

                // General search
                bool hasBigWelcome = Bot.HasInHandOrInSpellZone(CardId.BigWelcomeLabrynth);
                bool hasWelcome = Bot.HasInHandOrInSpellZone(CardId.WelcomeLabrynth);

                if (!hasBigWelcome && cards.Any(c => c != null && c.Id == CardId.BigWelcomeLabrynth))
                    return SelectPreferred(cards, min, max, CardId.BigWelcomeLabrynth);
                if (!hasWelcome && cards.Any(c => c != null && c.Id == CardId.WelcomeLabrynth))
                    return SelectPreferred(cards, min, max, CardId.WelcomeLabrynth);

                return SelectPreferred(cards, min, max,
                    CardId.BigWelcomeLabrynth,
                    CardId.WelcomeLabrynth,
                    CardId.AriannaTheLabrynthServant,
                    CardId.LadyLabrynthOfTheSilverCastle,
                    CardId.LovelyLabrynthOfTheSilverCastle,
                    CardId.DogmatikaPunishment,
                    CardId.DestructiveDarumaKarmaCannon,
                    CardId.IceDragonsPrison,
                    CardId.TerrorsOfTheOverroot,
                    CardId.AshBlossom,
                    CardId.AriasTheLabrynthButler,
                    CardId.ArianeTheLabrynthServant,
                    CardId.LabrynthStovieTorbie,
                    CardId.LabrynthChandraglier
                );
            }

            // 12. Send to GY cost / Discard cost / Banish cost (501: Discard, 508: To Grave, 504: Banish/Remove)
            if (hint == 501 || hint == 508 || hint == 504)
            {
                return SelectPreferred(cards, min, max,
                    CardId.TransactionRollback,
                    CardId.AbsoluteKingBackJack,
                    CardId.TheBlackGoatLaughs,
                    CardId.BigWelcomeLabrynth,
                    CardId.WelcomeLabrynth,
                    CardId.LabrynthStovieTorbie,
                    CardId.LabrynthChandraglier,
                    CardId.LavaGolem,
                    CardId.DogmatikaFleurdelis
                );
            }

            // 13. Absolute King Back Jack deck reorder (when not searching/setting/SS)
            if (cards.Count > 0 && cards.All(c => c != null && c.Location == CardLocation.Deck) && hint != 509 && hint != 510 && hint != 506)
            {
                var jackList = cards.OrderByDescending(c => {
                    if (IsNormalTrap(c)) return 100;
                    if (IsLabrynthMonster(c)) return 50;
                    return 10;
                }).ToList();
                if (jackList.Count >= min) return jackList.Take(max).ToList();
            }

            // 14. Generic Material Overrides (Link/Synchro)
            if (hint == 533 || hint == 512)
            {
                var sorted = cards.Where(c => c != null)
                    .OrderBy(c => GetMaterialPriority(c)).ToList();
                if (sorted.Count >= min) return sorted.Take(max).ToList();
            }

            // 15. Generic Destruction (hint == 502)
            if (hint == 502)
            {
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyCards.Count > 0)
                {
                    return enemyCards.OrderByDescending(c => {
                        if (Scorer != null) return Scorer.ThreatScore(c);
                        if (c.IsMonster() && c.IsFaceup() && !c.IsDisabled() && c.Attack >= 2500) return 1000;
                        if (c.IsMonster() && c.IsFaceup() && !c.IsDisabled()) return 500 + c.Attack;
                        if (c.IsSpell() || c.IsTrap())
                        {
                            if (c.HasType(CardType.Continuous) || c.HasType(CardType.Field)) return 400;
                            return 100;
                        }
                        return 10;
                    }).Take(max).ToList();
                }
                
                var ourCards = cards.Where(c => c != null && c.Controller == 0).ToList();
                if (ourCards.Count > 0)
                {
                    return ourCards.OrderBy(c => {
                        if (IsAceCard(c)) return 10000;
                        if (c.Id == CardId.AbsoluteKingBackJack) return 10;
                        if (c.Id == CardId.LabrynthStovieTorbie || c.Id == CardId.LabrynthChandraglier) return 20;
                        if (c.IsSpell() || c.IsTrap()) return 50;
                        return 100;
                    }).Take(max).ToList();
                }
            }

            // 16. Trap Trick (Banish 1 Normal Trap from Deck)
            if (LastChainCard != null && LastChainCard.Id == CardId.TrapTrick && cards.All(c => c != null && c.Location == CardLocation.Deck))
            {
                return SelectPreferred(cards, min, max,
                    CardId.BigWelcomeLabrynth,
                    CardId.DogmatikaPunishment,
                    CardId.DestructiveDarumaKarmaCannon,
                    CardId.IceDragonsPrison,
                    CardId.TerrorsOfTheOverroot
                );
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override IList<ClientCard> OnCardSorting(IList<ClientCard> cards)
        {
            // Place Normal Traps on top of the Deck (first drawn) for Absolute King Back Jack
            return cards.OrderByDescending(c => {
                if (IsNormalTrap(c)) return 100;
                if (IsLabrynthMonster(c)) return 50;
                return 10;
            }).ToList();
        }

        public override int OnSelectOption(IList<long> options)
        {
            if (LastChainCard != null)
            {
                if (LastChainCard.Id == CardId.Number60DugaresTheTimeless)
                {
                    bool hasRevivableBoss = Bot.Graveyard.Any(c => c.IsMonster() && c.IsCanRevive() && 
                        (c.Id == CardId.LovelyLabrynthOfTheSilverCastle || c.Id == CardId.LadyLabrynthOfTheSilverCastle));
                    if (hasRevivableBoss && options.Count > 1) return 1; // Special Summon
                    return 0; // Draw 2
                }

                if (LastChainCard.Id == CardId.AriasTheLabrynthButler)
                {
                    bool hasTrap = Bot.Hand.Any(c => c != null && IsNormalTrap(c));
                    bool hasMon = Bot.Hand.Any(c => c != null && IsLabrynthMonster(c));
                    if (Duel.Player == 1) // Opponent's turn: prefer setting a trap for disruption
                    {
                        if (hasTrap && options.Count > 1) return 1;
                        if (hasMon) return 0;
                    }
                    else // Our turn: prefer Special Summoning
                    {
                        if (hasMon) return 0;
                        if (hasTrap && options.Count > 1) return 1;
                    }
                }
            }
            return base.OnSelectOption(options);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.AbsoluteKingBackJack || cardId == CardId.LabrynthStovieTorbie)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            }
            if (cardId == CardId.AriannaTheLabrynthServant || cardId == CardId.DogmatikaEcclesia)
            {
                if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1 && positions.Contains(CardPosition.FaceUpDefence))
                {
                    return CardPosition.FaceUpDefence;
                }
            }
            return base.OnSelectPosition(cardId, positions);
        }

        public override ClientCard OnSelectAttacker(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            // Attack with Chaos Angel first if available to banish/push
            ClientCard chaosAngel = attackers.GetFirstMatchingCard(card => card.IsCode(CardId.ChaosAngel));
            if (chaosAngel != null) return chaosAngel;

            // Then Lady Labrynth (3000 ATK)
            ClientCard lady = attackers.GetFirstMatchingCard(card => card.IsCode(CardId.LadyLabrynthOfTheSilverCastle));
            if (lady != null) return lady;

            // Then Lovely Labrynth (2900 ATK)
            ClientCard lovely = attackers.GetFirstMatchingCard(card => card.IsCode(CardId.LovelyLabrynthOfTheSilverCastle));
            if (lovely != null) return lovely;

            return base.OnSelectAttacker(attackers, defenders);
        }

        public override BattlePhaseAction OnSelectAttackTarget(ClientCard attacker, IList<ClientCard> defenders)
        {
            foreach (ClientCard defender in defenders)
            {
                attacker.RealPower = attacker.Attack;
                defender.RealPower = defender.GetDefensePower();

                if (defender.RealPower < 0) defender.RealPower = 3000;

                if (!OnPreBattleBetween(attacker, defender)) continue;

                if (attacker.RealPower > defender.RealPower || 
                    (attacker.RealPower == defender.RealPower && defender.IsAttack() && Bot.GetMonsterCount() >= Enemy.GetMonsterCount()))
                {
                    return AI.Attack(attacker, defender);
                }
            }

            if (attacker.CanDirectAttack)
                return AI.Attack(attacker, null);

            return null;
        }

        public override bool OnPreBattleBetween(ClientCard attacker, ClientCard defender)
        {
            if (defender != null && defender.IsFaceup() && !defender.IsDisabled())
            {
                // Avramax / Crystal Wing check in battle too (to prevent attacking them)
                if (defender.Id == 21887175 && attacker.IsSpecialSummoned) return false;
                if (defender.Id == 50954680 && attacker.Level >= 5) return false;

                if (defender.IsAttack())
                {
                    if (defender.Attack > attacker.Attack) return false;
                    if (defender.Attack == attacker.Attack)
                    {
                        // Allow trade if we have equal or more monsters than enemy (meaning we have advantage)
                        if (Bot.GetMonsterCount() < Enemy.GetMonsterCount())
                            return false;
                    }
                }
                if (defender.IsDefense() && attacker.Attack <= defender.Defense) return false;
            }
            return base.OnPreBattleBetween(attacker, defender);
        }
    }

    [Deck("Expert_2026_Labrynth", "2026_Labrynth")]
    public class ExpertLabrynthExecutor : _2026_LabrynthExecutor
    {
        private string _duelId;
        public ExpertLabrynthExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            _duelId = $"duel_{Guid.NewGuid():N}";
            // [REMOVED-AI-TRAINING] ExpertDataLogger.EnsureInitialized(ExpertDataLogger.FindProjectRoot());
        }
        public override MainPhaseAction OnSelectIdleCmd(MainPhase main)
        {
            var action = base.OnSelectIdleCmd(main);
            // [REMOVED-AI-TRAINING] ExpertDataLogger.LogMainPhaseDecision(main, action, Bot, Enemy, _duelId, Duel.Turn);
            return action;
        }
        public override BattlePhaseAction OnBattle(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            var action = base.OnBattle(attackers, defenders);
            // [REMOVED-AI-TRAINING] ExpertDataLogger.LogBattleDecision(attackers, defenders, action, Bot, Enemy, _duelId, Duel.Turn);
            return action;
        }
    }
}
