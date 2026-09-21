using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("AFS", "AFS")]
    [Deck("2026_AFS", "2026_AFS")]
    [Deck("Azamina Fiendsmith Snake-Eye", "AFS")]
    [Deck("Azamina Fiendsmith", "AFS")]
    [Deck("Azamina", "AFS")]
    [Deck("Fiendsmith", "AFS")]
    public class AFSExecutor : ModernExecutor
    {
        public class CardId
        {
            // Snake-Eye Engine
            public const int SnakeEyesFlambergeDragon = 48452496;
            public const int SnakeEyesDiabellstar = 27260347;
            public const int DiabellstarTheBlackWitch = 72270339;
            public const int SnakeEyeAsh = 9674034;
            public const int SnakeEyesPoplar = 90241276;
            public const int SnakeEyeOak = 45663742;
            public const int WantedSeekerOfSinfulSpoils = 80845034;
            public const int DivineTempleOfTheSnakeEye = 53639887;
            public const int Bonfire = 85106525;
            public const int OriginalSinfulSpoils = 89023486;

            // Azamina Engine
            public const int DeceptionOfTheSinfulSpoils = 66328392;
            public const int TheHallowedAzamina = 94845588;
            public const int AzaminaIliaSilvia = 46396218;
            public const int AzaminaMuRcielago = 73391962;

            // Fiendsmith Engine
            public const int FiendsmithEngraver = 60764609;
            public const int FiendsmithsTract = 98567237;
            public const int FiendsmithsSanct = 35552985;
            public const int LacrimaTheCrimsonTears = 28803166;
            public const int FabledLurrie = 97651498;
            public const int FiendsmithsRequiem = 2463794;
            public const int FiendsmithsSequence = 49867899;
            public const int FiendsmithsLacrima = 46640168;
            public const int FiendsmithsAgnumday = 32991300;
            public const int DDDWaveHighKingCaesar = 79559912;
            public const int MoonOfTheClosedHeaven = 71818935;

            // Extra Deck Bosses & Staples
            public const int PrometheanPrincess = 2772337;
            public const int SPLittleKnight = 29301450;
            public const int IPMasquerena = 65741786;
            public const int Linkuriboh = 41999284;
            public const int HiitaTheFireCharmer = 48815792;
            public const int SalamangreatRagingPhoenix = 57134592;
            public const int WorldseaDragonZealantis = 45112597;
            public const int AccesscodeTalker = 86066372;

            // Handtraps & Staples
            public const int MaxxC = 23434538;
            public const int AshBlossom = 14558127;
            public const int GhostBelle = 73642296;
            public const int MulcharmyFuwalos = 42141493;
            public const int MulcharmyPurulia = 84192580;
            public const int NibiruThePrimalBeing = 27204311;
            public const int CalledByTheGrave = 24224830;
            public const int CrossoutDesignator = 65681983;
            public const int InfiniteImpermanence = 10045474;
            public const int ForbiddenDroplet = 24299458;
            public const int DarkRulerNoMore = 54693926;
            public const int LightningStorm = 14532163;
            public const int HarpiesFeatherDuster = 18144507;
            public const int EvenlyMatched = 15693423;
            public const int BystialDruiswurm = 6637331;
            public const int BystialMagnamhut = 33854624;
            public const int SolemnStrike = 40605147;
            public const int DimensionalBarrier = 83326048;
        }

        private const long HINT_SELECT_DESTROY = 502;
        private const long HINT_SELECT_REMOVE = 503;
        private const long HINT_SELECT_TOGRAVE = 504;
        private const long HINT_SELECT_TOHAND = 506;
        private const long HINT_SELECT_TODECK = 507;
        private const long HINT_SELECT_SPSUMMON = 509;
        private const long HINT_SELECT_DISCARD = 501;

        private bool _diabellstarSetUsed;
        private bool _snakeEyeAshSearchUsed;
        private bool _snakeEyeAshSummonFromDeckUsed;
        private bool _snakeEyesPoplarSearchUsed;
        private bool _snakeEyeOakSummonUsed;
        private bool _flambergeSTPlaceUsed;
        private bool _flambergeGYTriggerUsed;
        private bool _deceptionUsed;
        private bool _hallowedAzaminaUsed;
        private bool _engraverHandUsed;
        private bool _engraverGYUsed;
        private bool _tractUsed;
        private bool _lacrimaSummonUsed;
        private bool _requiemUsed;
        private bool _sequenceUsed;
        private bool _fiendsmithLacrimaUsed;
        private bool _prometheanPrincessUsed;

        public AFSExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // ── 1. Register Ace Cards in ResourcePlanner ──
            ResourcePlan.RegisterAceCards(
                CardId.DDDWaveHighKingCaesar,
                CardId.AzaminaIliaSilvia,
                CardId.AzaminaMuRcielago,
                CardId.SnakeEyesFlambergeDragon,
                CardId.PrometheanPrincess,
                CardId.SPLittleKnight,
                CardId.IPMasquerena,
                CardId.AccesscodeTalker,
                CardId.SalamangreatRagingPhoenix,
                CardId.WorldseaDragonZealantis
            );

            // ── 2. Register Starters and Baits in BaitPlanner ──
            BaitPlanner.RegisterComboStarters(
                CardId.SnakeEyeAsh,
                CardId.Bonfire,
                CardId.WantedSeekerOfSinfulSpoils,
                CardId.DiabellstarTheBlackWitch,
                CardId.DeceptionOfTheSinfulSpoils,
                CardId.TheHallowedAzamina,
                CardId.FiendsmithEngraver,
                CardId.FiendsmithsTract,
                CardId.FiendsmithsSanct
            );
            BaitPlanner.RegisterBaitCards(
                CardId.DivineTempleOfTheSnakeEye,
                CardId.CrossoutDesignator
            );

            // ── 3. Register Combo Lines in ComboRouter ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "AFS-TripleEngine-CaesarSilvia",
                RequiredCards = new List<int> { CardId.SnakeEyeAsh },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.SnakeEyeAsh, ActionType = ExecutorType.Summon, Description = "Normal Summon Snake-Eye Ash -> Search Poplar" },
                    new() { CardId = CardId.SnakeEyesPoplar, ActionType = ExecutorType.Activate, Description = "Poplar SS from hand -> Search Temple" },
                    new() { CardId = CardId.Linkuriboh, ActionType = ExecutorType.SpSummon, Description = "Link Summon Linkuriboh -> Poplar places self in S/T" },
                    new() { CardId = CardId.SnakeEyeAsh, ActionType = ExecutorType.Activate, Description = "Ash sends self + S/T Poplar -> SS Flamberge Dragon" },
                    new() { CardId = CardId.FiendsmithEngraver, ActionType = ExecutorType.Activate, Description = "Engraver -> Search Tract -> Lurrie into Requiem" },
                    new() { CardId = CardId.DDDWaveHighKingCaesar, ActionType = ExecutorType.SpSummon, Description = "Xyz Summon Caesar (Double SS Negate)" },
                    new() { CardId = CardId.TheHallowedAzamina, ActionType = ExecutorType.Activate, Description = "The Hallowed Azamina -> Fusion Summon Silvia (Omni-Negate)" }
                },
                FallbackLineName = "Fiendsmith-Caesar-Line"
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Fiendsmith-Caesar-Line",
                RequiredCards = new List<int> { CardId.FiendsmithEngraver },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.FiendsmithEngraver, ActionType = ExecutorType.Activate, Description = "Engraver searches Fiendsmith's Tract" },
                    new() { CardId = CardId.FiendsmithsTract, ActionType = ExecutorType.Activate, Description = "Tract searches and discards Fabled Lurrie -> Lurrie SS" },
                    new() { CardId = CardId.FiendsmithsRequiem, ActionType = ExecutorType.SpSummon, Description = "Lurrie into Requiem -> Requiem SS Lacrima from Deck" },
                    new() { CardId = CardId.FiendsmithsSequence, ActionType = ExecutorType.SpSummon, Description = "Link Summon Sequence -> Fusion Fiendsmith's Lacrima" },
                    new() { CardId = CardId.DDDWaveHighKingCaesar, ActionType = ExecutorType.SpSummon, Description = "Xyz Summon Caesar" }
                }
            });

            // ═══════════════════════════════════════════════════════════════
            //  EXECUTORS PIPELINE
            // ═══════════════════════════════════════════════════════════════

            // ── Tier 0: Quick Negates, Handtraps & Interruptions ──
            AddExecutor(ExecutorType.Activate, CardId.DDDWaveHighKingCaesar, CaesarNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.AzaminaIliaSilvia, SilviaNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.PrometheanPrincess, PrometheanPrincessGYQuickEffect);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightQuickEffect);
            AddExecutor(ExecutorType.Activate, CardId.IPMasquerena, IPMasquerenaEffect);
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelle, GhostBelleEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyPurulia, MulcharmyPuruliaEffect);
            AddExecutor(ExecutorType.Activate, CardId.NibiruThePrimalBeing, NibiruEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.CrossoutDesignator, CrossoutDesignatorEffect);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletEffect);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, DefaultInfiniteImpermanence);
            AddExecutor(ExecutorType.Activate, CardId.SolemnStrike, DefaultSolemnStrike);
            AddExecutor(ExecutorType.Activate, CardId.DimensionalBarrier, DimensionalBarrierEffect);
            AddExecutor(ExecutorType.Activate, CardId.BystialDruiswurm, BystialSpecialSummon);
            AddExecutor(ExecutorType.Activate, CardId.BystialMagnamhut, BystialSpecialSummon);
            AddExecutor(ExecutorType.Activate, CardId.DarkRulerNoMore, DarkRulerNoMoreEffect);
            AddExecutor(ExecutorType.Activate, CardId.LightningStorm, LightningStormEffect);
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, HarpiesFeatherDusterEffect);
            AddExecutor(ExecutorType.Activate, CardId.EvenlyMatched, EvenlyMatchedEffect);

            // ── Tier 1: Primary Starters (Bonfire / WANTED / Ash / Engraver / Tract) ──
            AddExecutor(ExecutorType.Activate, CardId.WantedSeekerOfSinfulSpoils, WantedEffect);
            AddExecutor(ExecutorType.Activate, CardId.Bonfire, BonfireEffect);
            AddExecutor(ExecutorType.Activate, CardId.OriginalSinfulSpoils, OriginalSinfulSpoilsEffect);

            AddExecutor(ExecutorType.Activate, CardId.FiendsmithEngraver, EngraverEffect);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsTract, TractEffect);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsSanct, SanctEffect);
            AddExecutor(ExecutorType.Activate, CardId.FabledLurrie, LurrieEffect);

            AddExecutor(ExecutorType.Summon, CardId.SnakeEyeAsh, SnakeEyeAshSummon);
            AddExecutor(ExecutorType.Activate, CardId.SnakeEyeAsh, SnakeEyeAshEffect);
            AddExecutor(ExecutorType.Activate, CardId.SnakeEyesPoplar, PoplarEffect);
            AddExecutor(ExecutorType.Summon, CardId.SnakeEyesPoplar, PoplarSummon);

            // Diabellstar Inherent Special Summon & Trigger
            AddExecutor(ExecutorType.SpSummon, CardId.DiabellstarTheBlackWitch, DiabellstarSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.DiabellstarTheBlackWitch, DiabellstarSetEffect);
            AddExecutor(ExecutorType.Activate, CardId.DivineTempleOfTheSnakeEye, DivineTempleEffect);

            // ── Tier 2: Azamina Fusion & Sinful Spoils Spells ──
            AddExecutor(ExecutorType.Activate, CardId.TheHallowedAzamina, HallowedAzaminaEffect);
            AddExecutor(ExecutorType.Activate, CardId.DeceptionOfTheSinfulSpoils, DeceptionEffect);
            AddExecutor(ExecutorType.Activate, CardId.AzaminaMuRcielago, MuRcielagoSearchEffect);

            // ── Tier 3: Fiendsmith Engine Loop ──
            AddExecutor(ExecutorType.SpSummon, CardId.MoonOfTheClosedHeaven, MoonOfTheClosedHeavenSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithsRequiem, RequiemSummon);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsRequiem, RequiemEffect);
            AddExecutor(ExecutorType.Summon, CardId.LacrimaTheCrimsonTears, LacrimaSummon);
            AddExecutor(ExecutorType.Activate, CardId.LacrimaTheCrimsonTears, LacrimaEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithsSequence, SequenceSummon);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsSequence, SequenceEffect);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsLacrima, FiendsmithLacrimaEffect);

            // ── Tier 4: Snake-Eye Board Development & Revives ──
            AddExecutor(ExecutorType.Summon, CardId.SnakeEyeOak, SnakeEyeOakSummon);
            AddExecutor(ExecutorType.Activate, CardId.SnakeEyeOak, SnakeEyeOakEffect);
            AddExecutor(ExecutorType.Activate, CardId.SnakeEyesFlambergeDragon, FlambergeEffect);
            AddExecutor(ExecutorType.Activate, CardId.SnakeEyesDiabellstar, SnakeEyesDiabellstarEffect);

            // ── Tier 5: Extra Deck Boss Summons ──
            AddExecutor(ExecutorType.SpSummon, CardId.DDDWaveHighKingCaesar, CaesarSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Linkuriboh, LinkuribohSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.PrometheanPrincess, PrometheanPrincessSummon);
            AddExecutor(ExecutorType.Activate, CardId.PrometheanPrincess, PrometheanPrincessMainEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.HiitaTheFireCharmer, HiitaSummon);
            AddExecutor(ExecutorType.Activate, CardId.HiitaTheFireCharmer, HiitaEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightOnSummonEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.IPMasquerena, IPMasquerenaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithsAgnumday, AgnumdaySummon);

            // ── Tier 6: High Link Finishers (Going Second / Push for Game) ──
            AddExecutor(ExecutorType.SpSummon, CardId.SalamangreatRagingPhoenix, RagingPhoenixSummon);
            AddExecutor(ExecutorType.Activate, CardId.SalamangreatRagingPhoenix, RagingPhoenixEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.WorldseaDragonZealantis, ZealantisSummon);
            AddExecutor(ExecutorType.Activate, CardId.WorldseaDragonZealantis, ZealantisEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, AccesscodeTalkerSummon);
            AddExecutor(ExecutorType.Activate, CardId.AccesscodeTalker, AccesscodeTalkerEffect);

            // ── Tier 7: Backrow Setting ──
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet, SpellSetInMain2);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, SpellSetInMain2);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, SpellSetInMain2);
            AddExecutor(ExecutorType.SpellSet, CardId.CrossoutDesignator, SpellSetInMain2);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnStrike, SpellSetInMain2);
            AddExecutor(ExecutorType.SpellSet, CardId.DimensionalBarrier, SpellSetInMain2);

            // ── Tier 8: Position Reposition ──
            AddExecutor(ExecutorType.Repos, MonsterReposOverride);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _diabellstarSetUsed = false;
            _snakeEyeAshSearchUsed = false;
            _snakeEyeAshSummonFromDeckUsed = false;
            _snakeEyesPoplarSearchUsed = false;
            _snakeEyeOakSummonUsed = false;
            _flambergeSTPlaceUsed = false;
            _flambergeGYTriggerUsed = false;
            _deceptionUsed = false;
            _hallowedAzaminaUsed = false;
            _engraverHandUsed = false;
            _engraverGYUsed = false;
            _tractUsed = false;
            _lacrimaSummonUsed = false;
            _requiemUsed = false;
            _sequenceUsed = false;
            _fiendsmithLacrimaUsed = false;
            _prometheanPrincessUsed = false;
        }

        private bool SpellSetInMain2()
        {
            return Duel.Phase == DuelPhase.Main2 || (Duel.Phase == DuelPhase.Main1 && Duel.Turn == 1);
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.DDDWaveHighKingCaesar
                || card.Id == CardId.AzaminaIliaSilvia
                || card.Id == CardId.SnakeEyesFlambergeDragon
                || card.Id == CardId.PrometheanPrincess
                || card.Id == CardId.SPLittleKnight
                || card.Id == CardId.IPMasquerena
                || card.Id == CardId.AccesscodeTalker
                || card.Id == CardId.SalamangreatRagingPhoenix
                || card.Id == CardId.WorldseaDragonZealantis;
        }

        // ═══════════════════════════════════════════════════════════════
        //  HANDLER IMPLEMENTATIONS
        // ═══════════════════════════════════════════════════════════════

        private bool CaesarNegateEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Duel.LastChainPlayer == 1) return true;
            return false;
        }

        private bool SilviaNegateEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Duel.LastChainPlayer == 1) return true;
            return false;
        }

        private bool PrometheanPrincessGYQuickEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                var ourFire = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Attribute == (int)CardAttribute.Fire && !IsAceCard(c))
                           ?? Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Attribute == (int)CardAttribute.Fire);
                var oppTarget = Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup());

                if (ourFire != null && oppTarget != null)
                {
                    AI.SelectCard(ourFire);
                    AI.SelectNextCard(oppTarget);
                    return true;
                }
            }
            return false;
        }

        private bool SPLittleKnightQuickEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Duel.LastChainPlayer == 1)
            {
                var ourTarget = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c != Card && !IsAceCard(c)) ?? Card;
                var oppTarget = Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup())
                             ?? Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());

                if (ourTarget != null && oppTarget != null)
                {
                    AI.SelectCard(ourTarget);
                    AI.SelectNextCard(oppTarget);
                    return true;
                }
            }
            return false;
        }

        private bool IPMasquerenaEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Duel.Player != 1) return false;
            if (Bot.GetMonsterCount() >= 2) return true;
            return false;
        }

        private bool MaxxCEffect()
        {
            if (Duel.Player == 1)
            {
                if (Duel.CurrentChain != null && Duel.CurrentChain.Any(c => c != null && c.Controller == 0 && c.Id == CardId.MaxxC)) return false;
                return DefaultMaxxC();
            }
            return false;
        }

        private bool AshBlossomEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            if (Duel.CurrentChain != null && Duel.CurrentChain.Any(c => c != null && c.Controller == 0 && c.Id == CardId.AshBlossom)) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool GhostBelleEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            if (Duel.CurrentChain != null && Duel.CurrentChain.Any(c => c != null && c.Controller == 0 && c.Id == CardId.GhostBelle)) return false;
            return DefaultGhostBelleAndHauntedMansion();
        }

        private bool MulcharmyFuwalosEffect()
        {
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                if (Duel.CurrentChain != null && Duel.CurrentChain.Any(c => c != null && c.Controller == 0 && c.Id == CardId.MulcharmyFuwalos)) return false;
                return Bot.GetMonsterCount() == 0 && Bot.Hand.Contains(Card);
            }
            return false;
        }

        private bool MulcharmyPuruliaEffect()
        {
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                if (Duel.CurrentChain != null && Duel.CurrentChain.Any(c => c != null && c.Controller == 0 && c.Id == CardId.MulcharmyPurulia)) return false;
                return Bot.GetMonsterCount() == 0 && Bot.Hand.Contains(Card);
            }
            return false;
        }

        private bool NibiruEffect()
        {
            return DefaultNibiru();
        }

        private bool CalledByTheGraveEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            if (Duel.CurrentChain != null && Duel.CurrentChain.Any(c => c != null && c.Controller == 0 && c.Id == CardId.CalledByTheGrave)) return false;
            return DefaultCalledByTheGrave();
        }

        private bool CrossoutDesignatorEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            if (Duel.CurrentChain != null && Duel.CurrentChain.Any(c => c != null && c.Controller == 0 && c.Id == CardId.CrossoutDesignator)) return false;
            return DefaultCrossoutDesignator();
        }

        private bool ForbiddenDropletEffect()
        {
            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled()))
            {
                var sendFodder = Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && (c.Id == CardId.DivineTempleOfTheSnakeEye || c.Id == CardId.DeceptionOfTheSinfulSpoils))
                              ?? Bot.Hand.FirstOrDefault(c => c != null && c != Card && (c.Id == CardId.FabledLurrie || c.Id == CardId.SnakeEyesPoplar))
                              ?? Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !IsAceCard(c));
                if (sendFodder != null)
                {
                    var oppTarget = Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled());
                    if (oppTarget != null)
                    {
                        AI.SelectCard(sendFodder);
                        AI.SelectNextCard(oppTarget);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool DarkRulerNoMoreEffect()
        {
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1 && Enemy.GetMonsterCount() > 0)
            {
                return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled());
            }
            return false;
        }

        private bool LightningStormEffect()
        {
            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup()) || Bot.GetSpells().Any(c => c != null && c.IsFaceup()))
                    return false;
                if (Enemy.GetSpellCount() >= 2)
                {
                    AI.SelectOption(1);
                    return true;
                }
                if (Enemy.GetMonsters().Any(c => c != null && c.IsAttack()))
                {
                    AI.SelectOption(0);
                    return true;
                }
                if (Enemy.GetSpellCount() > 0)
                {
                    AI.SelectOption(1);
                    return true;
                }
            }
            return false;
        }

        private bool HarpiesFeatherDusterEffect()
        {
            return Enemy.GetSpellCount() > 0;
        }

        private bool EvenlyMatchedEffect()
        {
            if (Duel.Phase == DuelPhase.Battle)
            {
                int botTotal = Bot.GetMonsterCount() + Bot.GetSpellCount();
                int enemyTotal = Enemy.GetMonsterCount() + Enemy.GetSpellCount();
                return enemyTotal > botTotal + 1;
            }
            return false;
        }

        private bool DimensionalBarrierEffect()
        {
            if (Duel.Player == 1)
            {
                AI.SelectOption(3); // Declare Xyz
                return true;
            }
            return false;
        }

        private bool BystialSpecialSummon()
        {
            if (Card.Location == CardLocation.Hand)
            {
                var target = Enemy.Graveyard.FirstOrDefault(c => c != null && (c.Attribute == (int)CardAttribute.Light || c.Attribute == (int)CardAttribute.Dark))
                          ?? Bot.Graveyard.FirstOrDefault(c => c != null && (c.Attribute == (int)CardAttribute.Light || c.Attribute == (int)CardAttribute.Dark) && !IsAceCard(c));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool OriginalSinfulSpoilsEffect()
        {
            var sendTarget = Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && (c.Id == CardId.DivineTempleOfTheSnakeEye || c.Id == CardId.SnakeEyesPoplar || c.Id == CardId.DeceptionOfTheSinfulSpoils))
                          ?? Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !IsAceCard(c) && c.Id != CardId.SnakeEyeAsh)
                          ?? Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());
            if (sendTarget != null)
            {
                AI.SelectCard(sendTarget);
                AI.SelectNextCard(CardId.SnakeEyeAsh, CardId.SnakeEyesPoplar, CardId.SnakeEyeOak);
                return true;
            }
            return false;
        }

        private bool MoonOfTheClosedHeavenSummon()
        {
            if (_requiemUsed) return false;
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attribute == (int)CardAttribute.Light && c.Race == (int)CardRace.Fiend))
                return false;
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !c.HasType(CardType.Token) && !IsAceCard(c));
            return mats >= 2;
        }

        private bool WantedEffect()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                AI.SelectCard(CardId.DiabellstarTheBlackWitch);
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                var target = Bot.Graveyard.FirstOrDefault(c => c != null && (c.Id == CardId.DeceptionOfTheSinfulSpoils || c.Id == CardId.TheHallowedAzamina));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool BonfireEffect()
        {
            AI.SelectCard(CardId.SnakeEyeAsh, CardId.SnakeEyesPoplar);
            return true;
        }

        private bool EngraverEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_engraverHandUsed) return false;
                _engraverHandUsed = true;
                AI.SelectCard(CardId.FiendsmithsTract);
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                if (_engraverGYUsed) return false;
                var shufTarget = Bot.Graveyard.FirstOrDefault(c => c != null && c.Attribute == (int)CardAttribute.Light && c.Race == (int)CardRace.Fiend && c != Card)
                              ?? Bot.Graveyard.FirstOrDefault(c => c != null && c.Id == CardId.FiendsmithsRequiem);
                if (shufTarget != null)
                {
                    _engraverGYUsed = true;
                    AI.SelectCard(shufTarget);
                    return true;
                }
            }
            return false;
        }

        private bool TractEffect()
        {
            if (_tractUsed) return false;
            _tractUsed = true;
            AI.SelectCard(CardId.FabledLurrie, CardId.LacrimaTheCrimsonTears, CardId.FiendsmithEngraver);
            return true;
        }

        private bool SanctEffect()
        {
            return Bot.GetMonsters().All(c => c == null || (c.Attribute == (int)CardAttribute.Light && c.Race == (int)CardRace.Fiend));
        }

        private bool LurrieEffect()
        {
            return true;
        }

        private bool SnakeEyeAshSummon()
        {
            return true;
        }

        private bool SnakeEyeAshEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (!_snakeEyeAshSearchUsed)
                {
                    _snakeEyeAshSearchUsed = true;
                    AI.SelectCard(CardId.SnakeEyesPoplar);
                    return true;
                }
                if (!_snakeEyeAshSummonFromDeckUsed)
                {
                    int sendable = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c))
                                 + Bot.GetSpells().Count(c => c != null && c.IsFaceup());
                    if (sendable >= 2)
                    {
                        _snakeEyeAshSummonFromDeckUsed = true;
                        AI.SelectCard(CardId.SnakeEyesFlambergeDragon, CardId.SnakeEyeOak);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool PoplarEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return true;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (!_snakeEyesPoplarSearchUsed)
                {
                    _snakeEyesPoplarSearchUsed = true;
                    AI.SelectCard(CardId.DivineTempleOfTheSnakeEye, CardId.OriginalSinfulSpoils);
                    return true;
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }
            return false;
        }

        private bool PoplarSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool DiabellstarSpSummon()
        {
            var sendFodder = Bot.Hand.FirstOrDefault(c => c != null && c != Card && (c.Id == CardId.FabledLurrie || c.Id == CardId.SnakeEyesPoplar || c.Id == CardId.DeceptionOfTheSinfulSpoils))
                          ?? Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.DivineTempleOfTheSnakeEye)
                          ?? Bot.Hand.FirstOrDefault(c => c != null && c != Card && !IsAceCard(c));
            if (sendFodder != null)
            {
                AI.SelectCard(sendFodder);
                return true;
            }
            return false;
        }

        private bool DiabellstarSetEffect()
        {
            if (_diabellstarSetUsed) return false;
            _diabellstarSetUsed = true;
            AI.SelectCard(CardId.DeceptionOfTheSinfulSpoils, CardId.OriginalSinfulSpoils, CardId.WantedSeekerOfSinfulSpoils);
            return true;
        }

        private bool DivineTempleEffect()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                AI.SelectCard(CardId.SnakeEyesFlambergeDragon, CardId.SnakeEyeAsh, CardId.SnakeEyesPoplar);
                return true;
            }
            return false;
        }

        private bool HallowedAzaminaEffect()
        {
            if (_hallowedAzaminaUsed) return false;
            _hallowedAzaminaUsed = true;
            AI.SelectCard(CardId.AzaminaIliaSilvia, CardId.AzaminaMuRcielago);
            return true;
        }

        private bool DeceptionEffect()
        {
            if (_deceptionUsed) return false;
            var tributeTarget = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !IsAceCard(c));
            if (tributeTarget != null)
            {
                _deceptionUsed = true;
                AI.SelectCard(tributeTarget);
                AI.SelectNextCard(CardId.TheHallowedAzamina);
                return true;
            }
            return false;
        }

        private bool MuRcielagoSearchEffect()
        {
            AI.SelectCard(CardId.TheHallowedAzamina, CardId.DeceptionOfTheSinfulSpoils);
            return true;
        }

        private bool RequiemSummon()
        {
            if (_requiemUsed) return false;
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attribute == (int)CardAttribute.Light && c.Race == (int)CardRace.Fiend && !IsAceCard(c));
        }

        private bool RequiemEffect()
        {
            if (_requiemUsed) return false;
            _requiemUsed = true;
            AI.SelectCard(CardId.LacrimaTheCrimsonTears, CardId.FiendsmithEngraver);
            return true;
        }

        private bool LacrimaSummon()
        {
            return true;
        }

        private bool LacrimaEffect()
        {
            if (_lacrimaSummonUsed) return false;
            _lacrimaSummonUsed = true;
            AI.SelectCard(CardId.FiendsmithEngraver);
            return true;
        }

        private bool SequenceSummon()
        {
            if (_sequenceUsed) return false;
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            return mats >= 2;
        }

        private bool SequenceEffect()
        {
            if (_sequenceUsed) return false;
            var fiendMats = Bot.Graveyard.Where(c => c != null && c.Attribute == (int)CardAttribute.Light && c.Race == (int)CardRace.Fiend).ToList();
            if (fiendMats.Count >= 2)
            {
                _sequenceUsed = true;
                AI.SelectCard(CardId.FiendsmithsLacrima);
                return true;
            }
            return false;
        }

        private bool FiendsmithLacrimaEffect()
        {
            if (_fiendsmithLacrimaUsed) return false;
            _fiendsmithLacrimaUsed = true;
            AI.SelectCard(CardId.FiendsmithEngraver);
            return true;
        }

        private bool SnakeEyeOakSummon()
        {
            return true;
        }

        private bool SnakeEyeOakEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (!_snakeEyeOakSummonUsed)
                {
                    _snakeEyeOakSummonUsed = true;
                    AI.SelectCard(CardId.SnakeEyeAsh, CardId.SnakeEyesPoplar);
                    return true;
                }
            }
            return false;
        }

        private bool CaesarSummon()
        {
            int lv6Fiends = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 6 && c.Race == (int)CardRace.Fiend);
            return lv6Fiends >= 2;
        }

        private bool FlambergeEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (!_flambergeSTPlaceUsed)
                {
                    _flambergeSTPlaceUsed = true;
                    var target = Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup())
                              ?? Bot.Graveyard.FirstOrDefault(c => c != null && c.IsMonster());
                    if (target != null)
                    {
                        AI.SelectCard(target);
                        return true;
                    }
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                if (!_flambergeGYTriggerUsed)
                {
                    _flambergeGYTriggerUsed = true;
                    AI.SelectCard(CardId.SnakeEyeAsh, CardId.SnakeEyesPoplar, CardId.SnakeEyeOak);
                    return true;
                }
            }
            return false;
        }

        private bool SnakeEyesDiabellstarEffect()
        {
            return true;
        }

        private bool LinkuribohSummon()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id == CardId.SnakeEyesPoplar);
        }

        private bool PrometheanPrincessSummon()
        {
            if (_prometheanPrincessUsed) return false;
            if (!_requiemUsed && !_sequenceUsed && Bot.GetMonsters().Any(c => c != null && c.Attribute == (int)CardAttribute.Light && c.Race == (int)CardRace.Fiend))
                return false;

            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            return mats >= 3 || (mats >= 2 && Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Link)));
        }

        private bool PrometheanPrincessMainEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_prometheanPrincessUsed) return false;
                _prometheanPrincessUsed = true;
                AI.SelectCard(CardId.SnakeEyesFlambergeDragon, CardId.SnakeEyeAsh, CardId.SnakeEyesPoplar);
                return true;
            }
            return false;
        }

        private bool HiitaSummon()
        {
            int fires = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Attribute == (int)CardAttribute.Fire && !IsAceCard(c));
            return fires >= 2;
        }

        private bool HiitaEffect()
        {
            var target = Enemy.Graveyard.FirstOrDefault(c => c != null && c.Attribute == (int)CardAttribute.Fire);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool SPLittleKnightSummon()
        {
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            return mats >= 2 && !Bot.HasInMonstersZone(CardId.SPLittleKnight);
        }

        private bool SPLittleKnightOnSummonEffect()
        {
            var target = Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup())
                      ?? Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup())
                      ?? Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsMonster());

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool IPMasquerenaSummon()
        {
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            return mats >= 2 && !Bot.HasInMonstersZone(CardId.IPMasquerena) && Duel.Turn == 1;
        }

        private bool AgnumdaySummon()
        {
            int fiends = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Race == (int)CardRace.Fiend);
            return fiends >= 2;
        }

        private bool RagingPhoenixSummon()
        {
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Attribute == (int)CardAttribute.Fire);
            return mats >= 3;
        }

        private bool RagingPhoenixEffect() => true;

        private bool ZealantisSummon()
        {
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup());
            return mats >= 4 || (mats >= 2 && Bot.GetMonsters().Any(c => c != null && c.HasType(CardType.Link) && c.LinkCount >= 3));
        }

        private bool ZealantisEffect() => true;

        private bool AccesscodeTalkerSummon()
        {
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup());
            return (mats >= 4 || (mats >= 2 && Bot.GetMonsters().Any(c => c != null && c.HasType(CardType.Link) && c.LinkCount >= 3))) && Duel.Turn > 1;
        }

        private bool AccesscodeTalkerEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup()) ?? Enemy.GetSpells().FirstOrDefault(c => c != null);
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TACTICAL DECISION OVERRIDES (OnSelectCard / Position)
        // ═══════════════════════════════════════════════════════════════

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Removal hints: ONLY target opponent cards (Rule 1)
            if (hint == HINT_SELECT_DESTROY || hint == HINT_SELECT_REMOVE)
            {
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyCards.Count >= min)
                {
                    var sorted = enemyCards.OrderByDescending(c => c.Attack).ToList();
                    return Util.CheckSelectCount(sorted, cards, min, max);
                }
            }

            // Original Sinful Spoils send cost and special summon target
            if (LastChainCard != null && LastChainCard.Id == CardId.OriginalSinfulSpoils)
            {
                if (hint == HINT_SELECT_TOGRAVE)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.SnakeEyesPoplar,
                        CardId.DivineTempleOfTheSnakeEye,
                        CardId.DeceptionOfTheSinfulSpoils,
                        CardId.Linkuriboh);
                }
                if (hint == HINT_SELECT_SPSUMMON || hint == 0)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.SnakeEyeAsh,
                        CardId.SnakeEyesPoplar,
                        CardId.SnakeEyeOak);
                }
            }

            // Diabellstar the Black Witch send cost from hand/field
            if (LastChainCard != null && LastChainCard.Id == CardId.DiabellstarTheBlackWitch && (hint == HINT_SELECT_TOGRAVE || hint == HINT_SELECT_DISCARD))
            {
                return SelectPreferredCard(cards, min, max,
                    CardId.FabledLurrie,
                    CardId.SnakeEyesPoplar,
                    CardId.DeceptionOfTheSinfulSpoils,
                    CardId.WantedSeekerOfSinfulSpoils,
                    CardId.DivineTempleOfTheSnakeEye);
            }

            // Fiendsmith's Tract search & discard
            if (LastChainCard != null && LastChainCard.Id == CardId.FiendsmithsTract)
            {
                if (hint == HINT_SELECT_TOHAND)
                {
                    return SelectPreferredCard(cards, min, max, CardId.FabledLurrie, CardId.LacrimaTheCrimsonTears, CardId.FiendsmithEngraver);
                }
                if (hint == HINT_SELECT_DISCARD || hint == HINT_SELECT_TOGRAVE)
                {
                    return SelectPreferredCard(cards, min, max, CardId.FabledLurrie, CardId.SnakeEyesPoplar, CardId.LacrimaTheCrimsonTears);
                }
            }

            // The Hallowed Azamina fusion & send cost
            if (LastChainCard != null && LastChainCard.Id == CardId.TheHallowedAzamina)
            {
                if (hint == HINT_SELECT_TOGRAVE)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.DeceptionOfTheSinfulSpoils,
                        CardId.WantedSeekerOfSinfulSpoils);
                }
            }

            // Snake-Eye Ash & Oak send 2 face-up cards cost
            if (LastChainCard != null && (LastChainCard.Id == CardId.SnakeEyeAsh || LastChainCard.Id == CardId.SnakeEyeOak))
            {
                if (hint == HINT_SELECT_TOGRAVE)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.SnakeEyesPoplar,
                        CardId.DeceptionOfTheSinfulSpoils,
                        CardId.DivineTempleOfTheSnakeEye,
                        CardId.SnakeEyeOak,
                        CardId.SnakeEyeAsh);
                }
            }

            // Snake-Eyes Flamberge Dragon revive 2 Level 1 FIRE
            if (LastChainCard != null && LastChainCard.Id == CardId.SnakeEyesFlambergeDragon && (hint == HINT_SELECT_SPSUMMON || hint == 0))
            {
                return SelectPreferredCard(cards, min, max, CardId.SnakeEyeAsh, CardId.SnakeEyesPoplar, CardId.SnakeEyeOak);
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.DDDWaveHighKingCaesar || cardId == CardId.AzaminaIliaSilvia || cardId == CardId.SnakeEyesFlambergeDragon || cardId == CardId.AccesscodeTalker || cardId == CardId.SalamangreatRagingPhoenix || cardId == CardId.WorldseaDragonZealantis || cardId == CardId.SPLittleKnight)
            {
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }
            if (cardId == CardId.FabledLurrie || cardId == CardId.LacrimaTheCrimsonTears || cardId == CardId.FiendsmithsLacrima || cardId == CardId.AzaminaMuRcielago || cardId == CardId.SnakeEyesPoplar)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            }
            return base.OnSelectPosition(cardId, positions);
        }

        private IList<ClientCard> SelectPreferredCard(IList<ClientCard> cards, int min, int max, params int[] preferredIds)
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

        private bool MonsterReposOverride()
        {
            if (Card == null) return false;
            if (IsAceCard(Card))
            {
                if (Card.IsAttack()) return false;
                return true;
            }

            bool enemyEmpty = Enemy.GetMonsterCount() == 0;
            if (Card.IsAttack())
            {
                if (!enemyEmpty && Card.Attack < 1200) return true;
            }
            else
            {
                if (enemyEmpty || Card.Defense < Card.Attack) return true;
            }
            return false;
        }

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            if (card != null && card.Controller == 1)
                return false;
            return base.OnSelectEffectYn(card, desc);
        }

        // ═══════════════════════════════════════════════════════════════
        //  BATTLE & ATTACK LOGIC
        // ═══════════════════════════════════════════════════════════════

        public override BattlePhaseAction OnBattle(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            if (attackers == null || attackers.Count == 0)
                return null;

            if (defenders == null || defenders.Count == 0)
            {
                var directAttacker = attackers
                    .Where(c => c != null && c.IsFaceup() && c.IsAttack() && c.Attack > 0)
                    .OrderBy(c => IsAceCard(c) ? 0 : 1)
                    .ThenByDescending(c => c.Attack)
                    .FirstOrDefault();

                if (directAttacker != null)
                {
                    return AI.Attack(directAttacker, null);
                }
            }

            return base.OnBattle(attackers, defenders);
        }
    }
}
