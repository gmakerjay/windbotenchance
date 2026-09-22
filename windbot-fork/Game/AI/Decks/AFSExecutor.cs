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

        // OCGCore / strings.conf System Hint Message Constants
        private const long HINTMSG_RELEASE = 500;
        private const long HINTMSG_DISCARD = 501;
        private const long HINTMSG_DESTROY = 502;
        private const long HINTMSG_REMOVE = 503;
        private const long HINTMSG_TOGRAVE = 504;
        private const long HINTMSG_RTOHAND = 505;
        private const long HINTMSG_ATOHAND = 506;
        private const long HINTMSG_TODECK = 507;
        private const long HINTMSG_SUMMON = 508;
        private const long HINTMSG_SPSUMMON = 509;
        private const long HINTMSG_SET = 510;
        private const long HINTMSG_FMATERIAL = 511;
        private const long HINTMSG_SMATERIAL = 512;
        private const long HINTMSG_XMATERIAL = 513;
        private const long HINTMSG_EQUIP = 518;
        private const long HINTMSG_XRELEASE = 519;
        private const long HINTMSG_PLACE = 527;
        private const long HINTMSG_LMATERIAL = 533;
        private const long HINTMSG_TARGET = 551;
        private const long HINTMSG_DISABLE = 552;
        private const long HINTMSG_NEGATE = 572;

        private bool _diabellstarSetUsed;
        private bool _snakeEyeAshSearchUsed;
        private bool _snakeEyeAshSummonFromDeckUsed;
        private bool _snakeEyesPoplarSearchUsed;
        private bool _snakeEyeOakSummonUsed;
        private bool _flambergeSTPlaceUsed;
        private bool _flambergeGYTriggerUsed;
        private bool _deceptionSearchUsed;
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
                CardId.LacrimaTheCrimsonTears
            );
            BaitPlanner.RegisterBaitCards(
                CardId.DivineTempleOfTheSnakeEye,
                CardId.CrossoutDesignator
            );

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

            // ── Tier 1: Primary Starters & Engine Bridges ──
            // Spells that search starters
            AddExecutor(ExecutorType.Activate, CardId.WantedSeekerOfSinfulSpoils, WantedEffect);
            AddExecutor(ExecutorType.Activate, CardId.Bonfire, BonfireEffect);

            // Snake-Eye Normal Summons & Triggers
            AddExecutor(ExecutorType.Summon, CardId.SnakeEyeAsh, SnakeEyeAshSummon);
            AddExecutor(ExecutorType.Activate, CardId.SnakeEyeAsh, SnakeEyeAshSearchEffect);
            AddExecutor(ExecutorType.Activate, CardId.SnakeEyesPoplar, PoplarEffect);
            AddExecutor(ExecutorType.Summon, CardId.SnakeEyesPoplar, PoplarSummon);

            // CRITICAL: Linkuriboh using Poplar BEFORE Ash's field send effect!
            AddExecutor(ExecutorType.SpSummon, CardId.Linkuriboh, LinkuribohSummon);

            // Fiendsmith Normal Summons & Hand Starters
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithEngraver, EngraverHandEffect);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsTract, TractEffect);
            AddExecutor(ExecutorType.Summon, CardId.LacrimaTheCrimsonTears, LacrimaSummon);
            AddExecutor(ExecutorType.Activate, CardId.LacrimaTheCrimsonTears, LacrimaEffect);
            AddExecutor(ExecutorType.Summon, CardId.FabledLurrie, LurrieSummon);
            AddExecutor(ExecutorType.Activate, CardId.FabledLurrie, LurrieEffect);

            // Diabellstar Inherent Special Summon & Spells
            AddExecutor(ExecutorType.SpSummon, CardId.DiabellstarTheBlackWitch, DiabellstarSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.DiabellstarTheBlackWitch, DiabellstarSetEffect);
            AddExecutor(ExecutorType.Activate, CardId.DivineTempleOfTheSnakeEye, DivineTempleEffect);
            AddExecutor(ExecutorType.Activate, CardId.OriginalSinfulSpoils, OriginalSinfulSpoilsEffect);

            // ── Tier 2: Intermediate Links & Extensions (The Engine Bridges) ──
            // Ash sends self + S/T Poplar -> Special Summons Flamberge Dragon
            AddExecutor(ExecutorType.Activate, CardId.SnakeEyeAsh, SnakeEyeAshSummonFromDeckEffect);
            AddExecutor(ExecutorType.Activate, CardId.SnakeEyesFlambergeDragon, FlambergeEffect);

            // Moon of the Closed Heaven bridges Linkuriboh + Flamberge into LIGHT Fiend
            AddExecutor(ExecutorType.SpSummon, CardId.MoonOfTheClosedHeaven, MoonOfTheClosedHeavenSummon);

            // ── Tier 3: Fiendsmith Loop to Caesar ──
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithsRequiem, RequiemSummon);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsRequiem, RequiemEffect);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithEngraver, EngraverGYEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithsSequence, SequenceSummon);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsSequence, SequenceEffect);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsLacrima, FiendsmithLacrimaEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.DDDWaveHighKingCaesar, CaesarSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithsAgnumday, AgnumdaySummon);

            // ── Tier 4: Azamina Fusion Engine to Silvia ──
            AddExecutor(ExecutorType.Activate, CardId.DeceptionOfTheSinfulSpoils, DeceptionEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheHallowedAzamina, HallowedAzaminaEffect);
            AddExecutor(ExecutorType.Activate, CardId.AzaminaMuRcielago, MuRcielagoSearchEffect);

            // ── Tier 5: Snake-Eye Remaining Effects & Diabellstar ──
            AddExecutor(ExecutorType.Summon, CardId.SnakeEyeOak, SnakeEyeOakSummon);
            AddExecutor(ExecutorType.Activate, CardId.SnakeEyeOak, SnakeEyeOakEffect);
            AddExecutor(ExecutorType.Activate, CardId.SnakeEyesDiabellstar, SnakeEyesDiabellstarEffect);

            // ── Tier 6: End Board Extra Deck Bosses (AFTER Caesar & Silvia) ──
            AddExecutor(ExecutorType.SpSummon, CardId.PrometheanPrincess, PrometheanPrincessSummon);
            AddExecutor(ExecutorType.Activate, CardId.PrometheanPrincess, PrometheanPrincessMainEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.HiitaTheFireCharmer, HiitaSummon);
            AddExecutor(ExecutorType.Activate, CardId.HiitaTheFireCharmer, HiitaEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.IPMasquerena, IPMasquerenaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightOnSummonEffect);

            // High Link Finishers (Going Second / Lethal Push)
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
            _deceptionSearchUsed = false;
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
            return Duel.Phase == DuelPhase.Main2
                || (Duel.Phase == DuelPhase.Main1 && Duel.Turn == 1)
                || (Duel.Phase == DuelPhase.Main1 && Duel.Turn > 1 && !Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsAttack() && c.Attack > 0));
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.DDDWaveHighKingCaesar
                || card.Id == CardId.AzaminaIliaSilvia
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
            if (Duel.LastChainPlayer == 0) return false;

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

        private bool WantedEffect()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                AI.SelectCard(CardId.DiabellstarTheBlackWitch);
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                var target = Bot.Graveyard.FirstOrDefault(c => c != null && (c.Id == CardId.DeceptionOfTheSinfulSpoils || c.Id == CardId.OriginalSinfulSpoils || c.Id == CardId.TheHallowedAzamina));
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

        private bool SnakeEyeAshSummon()
        {
            return true;
        }

        private bool SnakeEyeAshSearchEffect()
        {
            if (Card.Location == CardLocation.MonsterZone && !_snakeEyeAshSearchUsed)
            {
                _snakeEyeAshSearchUsed = true;
                AI.SelectCard(CardId.SnakeEyesPoplar);
                return true;
            }
            return false;
        }

        private bool SnakeEyeAshSummonFromDeckEffect()
        {
            if (Card.Location == CardLocation.MonsterZone && !_snakeEyeAshSummonFromDeckUsed)
            {
                int sendable = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c))
                             + Bot.GetSpells().Count(c => c != null && c.IsFaceup());
                if (sendable >= 2)
                {
                    _snakeEyeAshSummonFromDeckUsed = true;
                    // Cost will be handled by OnSelectCard (Ash + S/T Poplar)
                    // Resulting SS will be Flamberge Dragon
                    return true;
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

        private bool LinkuribohSummon()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (
                c.Id == CardId.SnakeEyesPoplar ||
                c.Id == CardId.FabledLurrie
            ));
        }

        private bool EngraverHandEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_engraverHandUsed) return false;
                _engraverHandUsed = true;
                AI.SelectCard(CardId.FiendsmithsTract);
                return true;
            }
            return false;
        }

        private bool EngraverGYEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                if (_engraverGYUsed) return false;
                var shufTarget = Bot.Graveyard.FirstOrDefault(c => c != null && c.Id == CardId.FiendsmithsRequiem)
                              ?? Bot.Graveyard.FirstOrDefault(c => c != null && c.Id == CardId.MoonOfTheClosedHeaven)
                              ?? Bot.Graveyard.FirstOrDefault(c => c != null && c.Attribute == (int)CardAttribute.Light && c.Race == (int)CardRace.Fiend && c != Card);
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

        private bool LurrieSummon()
        {
            // If no monsters on field, normal summon Lurrie to bridge into Requiem
            return Bot.GetMonsterCount() == 0;
        }

        private bool LurrieEffect()
        {
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

            // If Snake-Eye engine hasn't started, prioritize Original Sinful Spoils to summon Ash from deck!
            bool hasSnakeEyeStarted = Bot.GetMonsters().Any(c => c != null && (c.Id == CardId.SnakeEyeAsh || c.Id == CardId.SnakeEyesPoplar || c.Id == CardId.SnakeEyesFlambergeDragon))
                                   || Bot.Graveyard.Any(c => c != null && (c.Id == CardId.SnakeEyeAsh || c.Id == CardId.SnakeEyesPoplar));

            if (!hasSnakeEyeStarted)
            {
                AI.SelectCard(CardId.OriginalSinfulSpoils, CardId.DeceptionOfTheSinfulSpoils, CardId.WantedSeekerOfSinfulSpoils);
            }
            else
            {
                AI.SelectCard(CardId.DeceptionOfTheSinfulSpoils, CardId.OriginalSinfulSpoils, CardId.WantedSeekerOfSinfulSpoils);
            }
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
            // Don't need Moon if we already have a LIGHT Fiend monster
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attribute == (int)CardAttribute.Light && c.Race == (int)CardRace.Fiend))
                return false;

            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !c.HasType(CardType.Token) && !IsAceCard(c));
            return mats >= 2;
        }

        private bool RequiemSummon()
        {
            if (_requiemUsed) return false;
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attribute == (int)CardAttribute.Light && c.Race == (int)CardRace.Fiend && !IsAceCard(c));
        }

        private bool RequiemEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_requiemUsed) return false;
                _requiemUsed = true;
                AI.SelectCard(CardId.LacrimaTheCrimsonTears, CardId.FiendsmithEngraver);
                return true;
            }
            return false;
        }

        private bool SequenceSummon()
        {
            if (_sequenceUsed) return false;

            // Must have at least 1 LIGHT Fiend on field
            bool hasLightFiend = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attribute == (int)CardAttribute.Light && c.Race == (int)CardRace.Fiend && !IsAceCard(c));
            if (!hasLightFiend) return false;

            // If 2 Level 6 Fiends exist on field and Caesar is not yet made, let Caesar summon first!
            int lv6Fiends = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 6 && c.Race == (int)CardRace.Fiend);
            if (lv6Fiends >= 2 && !Bot.HasInMonstersZone(CardId.DDDWaveHighKingCaesar))
                return false;

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

        private bool CaesarSummon()
        {
            int lv6Fiends = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 6 && c.Race == (int)CardRace.Fiend);
            return lv6Fiends >= 2 && !Bot.HasInMonstersZone(CardId.DDDWaveHighKingCaesar);
        }

        private bool AgnumdaySummon()
        {
            int fiends = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Race == (int)CardRace.Fiend && !IsAceCard(c));
            return fiends >= 2;
        }

        private bool DeceptionEffect()
        {
            // Case 1: Activating Continuous Spell from hand
            if (Card.Location == CardLocation.Hand)
            {
                return true;
            }

            // Case 2: In SpellZone
            if (Card.Location == CardLocation.SpellZone)
            {
                // If set face-down, flip it face-up to activate the spell card (does not spend search yet)
                if (Card.IsFacedown())
                {
                    return true;
                }

                // If face-up, check if ignition search effect already used this turn
                if (_deceptionSearchUsed) return false;

                // Send 1 card from hand or field to GY (Fabled Lurrie triggers SS!)
                var sendFodder = Bot.Hand.FirstOrDefault(c => c != null && c != Card && c.Id == CardId.FabledLurrie)
                              ?? Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.SnakeEyesPoplar)
                              ?? Bot.Hand.FirstOrDefault(c => c != null && c != Card && (c.Id == CardId.SnakeEyesPoplar || c.Id == CardId.SnakeEyeOak || c.Id == CardId.SnakeEyesDiabellstar))
                              ?? Bot.Hand.FirstOrDefault(c => c != null && c != Card && c.Id == CardId.FiendsmithEngraver)
                              ?? Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.DivineTempleOfTheSnakeEye)
                              ?? Bot.Hand.FirstOrDefault(c => c != null && c != Card && (c.Id == CardId.AshBlossom || c.Id == CardId.GhostBelle))
                              ?? Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !IsAceCard(c) && c.Id != CardId.SnakeEyeAsh && c.Id != CardId.SnakeEyesFlambergeDragon);

                if (sendFodder != null)
                {
                    _deceptionSearchUsed = true;
                    AI.SelectCard(sendFodder);
                    AI.SelectNextCard(CardId.TheHallowedAzamina, CardId.AzaminaIliaSilvia, CardId.AzaminaMuRcielago);
                    return true;
                }
            }

            return false;
        }

        private bool HallowedAzaminaEffect()
        {
            if (_hallowedAzaminaUsed) return false;

            int sinfulCount = Bot.Hand.Count(c => c != null && IsSinfulSpoils(c))
                            + Bot.GetSpells().Count(c => c != null && c.IsFaceup() && IsSinfulSpoils(c));
            if (sinfulCount == 0) return false;

            _hallowedAzaminaUsed = true;
            // Silvia is Level 6 (requires 6/4 rounded down = 1 Sinful Spoils card)
            AI.SelectCard(CardId.AzaminaIliaSilvia, CardId.AzaminaMuRcielago);
            return true;
        }

        private bool MuRcielagoSearchEffect()
        {
            AI.SelectCard(CardId.TheHallowedAzamina, CardId.DeceptionOfTheSinfulSpoils);
            return true;
        }

        private bool SnakeEyeOakSummon()
        {
            return true;
        }

        private bool SnakeEyeOakEffect()
        {
            if (Card.Location == CardLocation.MonsterZone && !_snakeEyeOakSummonUsed)
            {
                _snakeEyeOakSummonUsed = true;
                AI.SelectCard(CardId.SnakeEyeAsh, CardId.SnakeEyesPoplar);
                return true;
            }
            return false;
        }

        private bool SnakeEyesDiabellstarEffect()
        {
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                var fireTarget = Bot.Graveyard.FirstOrDefault(c => c != null && c.Attribute == (int)CardAttribute.Fire && c.Id != CardId.SnakeEyesDiabellstar && c.IsMonster());
                if (fireTarget != null)
                {
                    AI.SelectCard(fireTarget);
                    return true;
                }
            }
            return false;
        }

        private bool FlambergeEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Our Turn: Main Phase Ignition Effect (place monster into S/T zone)
                if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
                {
                    if (!_flambergeSTPlaceUsed)
                    {
                        _flambergeSTPlaceUsed = true;
                        var target = Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup())
                                  ?? Bot.Graveyard.FirstOrDefault(c => c != null && c.Attribute == (int)CardAttribute.Fire && c.IsMonster());
                        if (target != null)
                        {
                            AI.SelectCard(target);
                            return true;
                        }
                    }
                }
                // Opponent's Turn: Quick Effect to Special Summon 1 monster from S/T zone to OUR field!
                else if (Duel.Player == 1)
                {
                    if (Bot.GetMonsterCount() < 5)
                    {
                        var stTarget = Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsMonster())
                                    ?? Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsMonster());
                        if (stTarget != null)
                        {
                            AI.SelectCard(stTarget);
                            return true;
                        }
                    }
                }
            }

            // In GY: Trigger Effect when sent from field to GY (revive 2 Level 1 FIRE monsters!)
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

        private bool PrometheanPrincessSummon()
        {
            if (_prometheanPrincessUsed) return false;

            // CRITICAL: Princess locks player into FIRE only!
            // Never summon Princess before Caesar or Silvia unless Fiendsmith/Azamina is completed or unreachable!
            bool fiendsmithDone = Bot.HasInMonstersZone(CardId.DDDWaveHighKingCaesar) || _sequenceUsed || _requiemUsed;
            if (!fiendsmithDone && (Bot.Hand.Any(c => c != null && (c.Id == CardId.FiendsmithEngraver || c.Id == CardId.FiendsmithsTract))
                || Bot.GetMonsters().Any(c => c != null && c.Id == CardId.MoonOfTheClosedHeaven)))
            {
                return false;
            }

            // Must have a FIRE monster in GY to revive
            if (!Bot.Graveyard.Any(c => c != null && c.Attribute == (int)CardAttribute.Fire && c.IsMonster()))
                return false;

            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            return mats >= 3 || (mats >= 2 && Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Link) && !IsAceCard(c)));
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

        private bool RagingPhoenixSummon()
        {
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Attribute == (int)CardAttribute.Fire && !IsAceCard(c));
            return mats >= 3;
        }

        private bool RagingPhoenixEffect() => true;

        private bool ZealantisSummon()
        {
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            return mats >= 4 || (mats >= 2 && Bot.GetMonsters().Any(c => c != null && c.HasType(CardType.Link) && c.LinkCount >= 3 && !IsAceCard(c)));
        }

        private bool ZealantisEffect() => true;

        private bool AccesscodeTalkerSummon()
        {
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            return (mats >= 4 || (mats >= 2 && Bot.GetMonsters().Any(c => c != null && c.HasType(CardType.Link) && c.LinkCount >= 3 && !IsAceCard(c)))) && Duel.Turn > 1;
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

        private static bool IsSinfulSpoils(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.DeceptionOfTheSinfulSpoils
                || card.Id == CardId.OriginalSinfulSpoils
                || card.Id == CardId.WantedSeekerOfSinfulSpoils;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TACTICAL DECISION OVERRIDES (OnSelectCard / Position)
        // ═══════════════════════════════════════════════════════════════

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // 1. Removal hints (DESTROY / REMOVE): strictly target opponent cards
            if (hint == HINTMSG_DESTROY || hint == HINTMSG_REMOVE)
            {
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyCards.Count >= min)
                {
                    var sorted = enemyCards.OrderByDescending(c => c.Attack).ToList();
                    return Util.CheckSelectCount(sorted, cards, min, max);
                }
            }

            // 2. Discard hint: prioritize Lurrie (triggers SS), protect starters
            if (hint == HINTMSG_DISCARD)
            {
                return SelectPreferredCard(cards, min, max,
                    CardId.FabledLurrie,
                    CardId.SnakeEyesPoplar,
                    CardId.FiendsmithEngraver,
                    CardId.LacrimaTheCrimsonTears);
            }

            // 3. Link Material Selection (HINTMSG_LMATERIAL = 533)
            if (hint == HINTMSG_LMATERIAL || (hint == 0 && cards.All(c => c != null && c.Location == CardLocation.MonsterZone && c.Controller == 0)))
            {
                // Linkuriboh: select Poplar or Lurrie
                var poplar = cards.FirstOrDefault(c => c != null && c.Id == CardId.SnakeEyesPoplar);
                if (poplar != null && min == 1 && max == 1) return new List<ClientCard> { poplar };

                var lurrie = cards.FirstOrDefault(c => c != null && c.Id == CardId.FabledLurrie);
                if (lurrie != null && min == 1 && max == 1) return new List<ClientCard> { lurrie };

                // Fiendsmith's Requiem: select Moon of the Closed Heaven or Lurrie or Lacrima
                var moon = cards.FirstOrDefault(c => c != null && c.Id == CardId.MoonOfTheClosedHeaven);
                if (moon != null && min == 1 && max == 1) return new List<ClientCard> { moon };

                // Fiendsmith's Sequence: select Lacrima + Engraver
                var seqMats = cards.Where(c => c != null && (c.Id == CardId.LacrimaTheCrimsonTears || c.Id == CardId.FiendsmithEngraver)).ToList();
                if (seqMats.Count >= min) return seqMats.Take(max).ToList();

                // Moon of the Closed Heaven / Princess: non-Ace monsters first
                var nonAce = cards.Where(c => c != null && !IsAceCard(c)).ToList();
                if (nonAce.Count >= min) return nonAce.Take(max).ToList();
            }

            // 4. Xyz Material Selection (HINTMSG_XMATERIAL = 513)
            if (hint == HINTMSG_XMATERIAL)
            {
                var caesarMats = cards.Where(c => c != null && (c.Id == CardId.FiendsmithsLacrima || c.Id == CardId.FiendsmithEngraver)).ToList();
                if (caesarMats.Count >= min) return caesarMats.Take(max).ToList();
            }

            // 5. Diabellstar Special Summon Cost from hand (when hint == 504 and hand cards present)
            if (hint == HINTMSG_TOGRAVE && cards.Any(c => c != null && c.Location == CardLocation.Hand))
            {
                var preferredCost = new[] {
                    CardId.FabledLurrie,
                    CardId.SnakeEyesPoplar,
                    CardId.DeceptionOfTheSinfulSpoils,
                    CardId.OriginalSinfulSpoils,
                    CardId.DivineTempleOfTheSnakeEye,
                    CardId.SnakeEyesDiabellstar,
                    CardId.SnakeEyeOak
                };
                foreach (int cid in preferredCost)
                {
                    var match = cards.FirstOrDefault(c => c != null && c.Id == cid);
                    if (match != null) return new List<ClientCard> { match };
                }
            }

            // 6. Snake-Eye Ash (Search, Cost, Summon from Deck)
            if (LastChainCard != null && LastChainCard.Id == CardId.SnakeEyeAsh)
            {
                if (hint == HINTMSG_ATOHAND || hint == HINTMSG_RTOHAND)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.SnakeEyesPoplar,
                        CardId.SnakeEyeOak);
                }
                if (hint == HINTMSG_TOGRAVE)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.SnakeEyesPoplar,
                        CardId.DivineTempleOfTheSnakeEye,
                        CardId.DeceptionOfTheSinfulSpoils,
                        CardId.SnakeEyeAsh,
                        CardId.Linkuriboh);
                }
                if (hint == HINTMSG_SPSUMMON || hint == 0)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.SnakeEyesFlambergeDragon,
                        CardId.SnakeEyeOak);
                }
            }

            // 7. Snake-Eyes Poplar (Search, S/T Placement)
            if (LastChainCard != null && LastChainCard.Id == CardId.SnakeEyesPoplar)
            {
                if (hint == HINTMSG_ATOHAND || hint == HINTMSG_RTOHAND)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.DivineTempleOfTheSnakeEye,
                        CardId.OriginalSinfulSpoils);
                }
                if (hint == HINTMSG_PLACE || hint == 0)
                {
                    return SelectPreferredCard(cards, min, max, CardId.SnakeEyesPoplar);
                }
            }

            // 8. Bonfire: Ash > Poplar
            if (LastChainCard != null && LastChainCard.Id == CardId.Bonfire)
            {
                if (hint == HINTMSG_ATOHAND || hint == HINTMSG_RTOHAND)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.SnakeEyeAsh,
                        CardId.SnakeEyesPoplar);
                }
            }

            // 9. Divine Temple of the Snake-Eye
            if (LastChainCard != null && LastChainCard.Id == CardId.DivineTempleOfTheSnakeEye)
            {
                if (hint == HINTMSG_PLACE || hint == 0)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.SnakeEyeAsh,
                        CardId.SnakeEyesFlambergeDragon,
                        CardId.SnakeEyesPoplar);
                }
            }

            // 10. Original Sinful Spoils
            if (LastChainCard != null && LastChainCard.Id == CardId.OriginalSinfulSpoils)
            {
                if (hint == HINTMSG_TOGRAVE)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.SnakeEyesPoplar,
                        CardId.DivineTempleOfTheSnakeEye,
                        CardId.DeceptionOfTheSinfulSpoils,
                        CardId.Linkuriboh);
                }
                if (hint == HINTMSG_SPSUMMON || hint == 0)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.SnakeEyeAsh,
                        CardId.SnakeEyesPoplar,
                        CardId.SnakeEyeOak);
                }
            }

            // 11. WANTED: Seeker of Sinful Spoils
            if (LastChainCard != null && LastChainCard.Id == CardId.WantedSeekerOfSinfulSpoils)
            {
                if (hint == HINTMSG_ATOHAND || hint == HINTMSG_RTOHAND)
                {
                    return SelectPreferredCard(cards, min, max, CardId.DiabellstarTheBlackWitch);
                }
                if (hint == HINTMSG_TODECK)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.OriginalSinfulSpoils,
                        CardId.DeceptionOfTheSinfulSpoils);
                }
            }

            // 12. Diabellstar the Black Witch
            if (LastChainCard != null && LastChainCard.Id == CardId.DiabellstarTheBlackWitch)
            {
                if (hint == HINTMSG_TOGRAVE || hint == HINTMSG_DISCARD)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.FabledLurrie,
                        CardId.SnakeEyesPoplar,
                        CardId.DeceptionOfTheSinfulSpoils,
                        CardId.WantedSeekerOfSinfulSpoils,
                        CardId.DivineTempleOfTheSnakeEye);
                }
                if (hint == HINTMSG_SET || hint == 0)
                {
                    bool hasSnakeEyeStarted = Bot.GetMonsters().Any(c => c != null && (c.Id == CardId.SnakeEyeAsh || c.Id == CardId.SnakeEyesPoplar || c.Id == CardId.SnakeEyesFlambergeDragon))
                                           || Bot.Graveyard.Any(c => c != null && (c.Id == CardId.SnakeEyeAsh || c.Id == CardId.SnakeEyesPoplar));
                    if (!hasSnakeEyeStarted)
                    {
                        return SelectPreferredCard(cards, min, max, CardId.OriginalSinfulSpoils, CardId.DeceptionOfTheSinfulSpoils);
                    }
                    return SelectPreferredCard(cards, min, max, CardId.DeceptionOfTheSinfulSpoils, CardId.OriginalSinfulSpoils);
                }
            }

            // 13. Deception of the Sinful Spoils
            if (LastChainCard != null && LastChainCard.Id == CardId.DeceptionOfTheSinfulSpoils)
            {
                if (hint == HINTMSG_TOGRAVE || hint == 0)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.FabledLurrie,
                        CardId.SnakeEyesPoplar,
                        CardId.DivineTempleOfTheSnakeEye,
                        CardId.SnakeEyeOak,
                        CardId.AshBlossom,
                        CardId.GhostBelle);
                }
                if (hint == HINTMSG_ATOHAND || hint == HINTMSG_RTOHAND)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.TheHallowedAzamina,
                        CardId.AzaminaIliaSilvia,
                        CardId.AzaminaMuRcielago);
                }
            }

            // 14. The Hallowed Azamina
            if (LastChainCard != null && LastChainCard.Id == CardId.TheHallowedAzamina)
            {
                if (hint == HINTMSG_SPSUMMON || hint == 0)
                {
                    return SelectPreferredCard(cards, min, max, CardId.AzaminaIliaSilvia, CardId.AzaminaMuRcielago);
                }
                if (hint == HINTMSG_TOGRAVE)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.DeceptionOfTheSinfulSpoils,
                        CardId.WantedSeekerOfSinfulSpoils,
                        CardId.OriginalSinfulSpoils);
                }
            }

            // 15. Fiendsmith's Tract
            if (LastChainCard != null && LastChainCard.Id == CardId.FiendsmithsTract)
            {
                if (hint == HINTMSG_ATOHAND || hint == HINTMSG_RTOHAND)
                {
                    return SelectPreferredCard(cards, min, max, CardId.FabledLurrie, CardId.LacrimaTheCrimsonTears, CardId.FiendsmithEngraver);
                }
                if (hint == HINTMSG_DISCARD || hint == HINTMSG_TOGRAVE)
                {
                    return SelectPreferredCard(cards, min, max, CardId.FabledLurrie, CardId.SnakeEyesPoplar, CardId.LacrimaTheCrimsonTears);
                }
            }

            // 16. Fiendsmith Engraver
            if (LastChainCard != null && LastChainCard.Id == CardId.FiendsmithEngraver)
            {
                if (hint == HINTMSG_ATOHAND || hint == HINTMSG_RTOHAND)
                {
                    return SelectPreferredCard(cards, min, max, CardId.FiendsmithsTract, CardId.FiendsmithsSanct);
                }
                if (hint == HINTMSG_TODECK)
                {
                    return SelectPreferredCard(cards, min, max, CardId.FiendsmithsRequiem, CardId.MoonOfTheClosedHeaven, CardId.LacrimaTheCrimsonTears, CardId.FabledLurrie);
                }
            }

            // 17. Fiendsmith's Requiem
            if (LastChainCard != null && LastChainCard.Id == CardId.FiendsmithsRequiem)
            {
                if (hint == HINTMSG_SPSUMMON || hint == 0)
                {
                    return SelectPreferredCard(cards, min, max, CardId.LacrimaTheCrimsonTears, CardId.FiendsmithEngraver);
                }
            }

            // 18. Lacrima the Crimson Tears
            if (LastChainCard != null && LastChainCard.Id == CardId.LacrimaTheCrimsonTears)
            {
                if (hint == HINTMSG_TOGRAVE)
                {
                    return SelectPreferredCard(cards, min, max, CardId.FiendsmithEngraver, CardId.FiendsmithsTract);
                }
            }

            // 19. Fiendsmith's Sequence
            if (LastChainCard != null && LastChainCard.Id == CardId.FiendsmithsSequence)
            {
                if (hint == HINTMSG_SPSUMMON || hint == 0)
                {
                    return SelectPreferredCard(cards, min, max, CardId.FiendsmithsLacrima);
                }
                if (hint == HINTMSG_FMATERIAL || hint == HINTMSG_TODECK)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.LacrimaTheCrimsonTears,
                        CardId.FiendsmithEngraver,
                        CardId.FabledLurrie,
                        CardId.FiendsmithsRequiem);
                }
            }

            // 20. Fiendsmith's Lacrima (Fusion)
            if (LastChainCard != null && LastChainCard.Id == CardId.FiendsmithsLacrima)
            {
                return SelectPreferredCard(cards, min, max,
                    CardId.FiendsmithEngraver,
                    CardId.LacrimaTheCrimsonTears,
                    CardId.FabledLurrie,
                    CardId.FiendsmithsRequiem);
            }

            // 21. Snake-Eye Oak
            if (LastChainCard != null && LastChainCard.Id == CardId.SnakeEyeOak)
            {
                if (hint == HINTMSG_TOGRAVE)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.SnakeEyesPoplar,
                        CardId.DeceptionOfTheSinfulSpoils,
                        CardId.DivineTempleOfTheSnakeEye,
                        CardId.SnakeEyeOak,
                        CardId.SnakeEyeAsh);
                }
                if (hint == HINTMSG_SPSUMMON || hint == 0)
                {
                    return SelectPreferredCard(cards, min, max, CardId.SnakeEyeAsh, CardId.SnakeEyesPoplar);
                }
            }

            // 22. Snake-Eyes Flamberge Dragon
            if (LastChainCard != null && LastChainCard.Id == CardId.SnakeEyesFlambergeDragon)
            {
                if (hint == HINTMSG_PLACE || hint == 0)
                {
                    var oppTarget = cards.Where(c => c != null && c.Controller == 1).OrderByDescending(c => c.Attack).ToList();
                    if (oppTarget.Count >= min) return Util.CheckSelectCount(oppTarget, cards, min, max);
                }
                if (hint == HINTMSG_SPSUMMON)
                {
                    return SelectPreferredCard(cards, min, max, CardId.SnakeEyeAsh, CardId.SnakeEyesPoplar, CardId.SnakeEyeOak);
                }
            }

            // 23. Promethean Princess GY Quick Effect
            if (LastChainCard != null && LastChainCard.Id == CardId.PrometheanPrincess)
            {
                if (hint == HINTMSG_DESTROY)
                {
                    var opp = cards.FirstOrDefault(c => c != null && c.Controller == 1);
                    if (opp != null) return new List<ClientCard> { opp };

                    var ourFire = cards.FirstOrDefault(c => c != null && c.Controller == 0 && c.Attribute == (int)CardAttribute.Fire && !IsAceCard(c));
                    if (ourFire != null) return new List<ClientCard> { ourFire };
                }
                if (hint == HINTMSG_SPSUMMON)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.SnakeEyesFlambergeDragon,
                        CardId.SnakeEyeAsh,
                        CardId.SnakeEyesPoplar);
                }
            }

            // 24. S:P Little Knight
            if (LastChainCard != null && LastChainCard.Id == CardId.SPLittleKnight)
            {
                if (hint == HINTMSG_REMOVE)
                {
                    var oppTarget = cards.Where(c => c != null && c.Controller == 1).OrderByDescending(c => c.Attack).ToList();
                    if (oppTarget.Count >= min) return Util.CheckSelectCount(oppTarget, cards, min, max);
                }
            }

            // 25. I:P Masquerena Link Summon on Opponent's Turn
            if (LastChainCard != null && LastChainCard.Id == CardId.IPMasquerena)
            {
                if (hint == HINTMSG_SPSUMMON || hint == 0)
                {
                    return SelectPreferredCard(cards, min, max, CardId.SPLittleKnight);
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0)
                return base.OnSelectOption(options);

            // Fiendsmith's Lacrima (46640168): option for Special Summon
            for (int i = 0; i < options.Count; i++)
            {
                long cardId = options[i] >> 4;
                long optIndex = options[i] & 0xf;
                if (cardId == 0)
                {
                    cardId = options[i] >> 20;
                    optIndex = options[i] & 0xfffff;
                }
                if (cardId == CardId.FiendsmithsLacrima)
                {
                    if (optIndex == 3) return i;
                }
            }

            if (LastChainCard != null && LastChainCard.Id == CardId.FiendsmithsLacrima && options.Count >= 2)
            {
                return 1; // Special Summon Engraver
            }

            // Lightning Storm
            if (LastChainCard != null && LastChainCard.Id == CardId.LightningStorm && options.Count >= 2)
            {
                if (Enemy.GetSpellCount() >= 2) return 1;
                if (Enemy.GetMonsters().Any(c => c != null && c.IsAttack())) return 0;
                return 1;
            }

            // Dimensional Barrier
            if (LastChainCard != null && LastChainCard.Id == CardId.DimensionalBarrier && options.Count >= 4)
            {
                return 3; // Declare Xyz
            }

            return base.OnSelectOption(options);
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
