using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    // ============================================================
    // CARD AUDIT — 2026_Magnet (Rock / Magnet Warrior / Ryzeal / Synchro / Xyz)
    // ============================================================
    // | Card Name                                 | Type    | OPT? | Cost            | Effect Summary                                | Activate When                     | NEVER Activate When                    |
    // |-------------------------------------------|---------|------|-----------------|-----------------------------------------------|-----------------------------------|----------------------------------------|
    // | Beta The Electromagnet Warrior (79418928) | Monster | Yes  | None            | Summon: search Lv4- Magnet Warrior            | Need setup / combo pieces         | Already searched / no target in deck   |
    // |                                           |         | Yes  | Tribute (Quick) | Opponent's turn: Tribute to SS Lv4 Magnet     | Opponent's turn / need body       | Our turn / no targets in deck          |
    // | Epsilon The Magnet Warrior (52566270)     | Monster | Yes  | None            | Send Magnet to GY; SS diff Magnet/Magna GY    | Have target in GY / need bodies   | No target in GY / no Magnet in deck    |
    // | Magnet Warrior Sigma Plus (51826619)      | Monster | Yes  | None            | GY: target Lv4- Magnet in GY; hand or SS      | Sent to GY / need extender        | No target in GY                        |
    // | Magnet Warrior Sigma Minus (87814728)     | Monster | Yes  | Reveal (hand)   | Reveal: Fusion summon using Rock monsters     | Have fusion target in hand/field  | No fusion target / no materials        |
    // |                                           |         | Yes  | None            | Summon: send Lv8 Magna from Deck to GY        | Need Tellusion in GY              | Tellusion already in GY                |
    // | Tellusion the Magna Warrior (24431911)    | Monster | Yes  | Banish Sigmas   | SS itself from GY                             | Have Sigma Plus/Minus in GY/field | Already summoned / no materials        |
    // |                                           |         | Yes  | None            | Quick: destroy or take control of Earth enemy | Opponent activates effect         | No target                              |
    // |                                           |         | Yes  | Tribute (Quick) | Quick: Tribute to summon 2 banished Sigmas    | Opponent's turn / need blocker    | Sigma Plus/Minus not in banish zone    |
    // | Conduction Warrior Linear Magnum (44839512)| Monster| Yes  | Send 2 Magnets  | SS from hand (send materials from hand);      | Need beatstick / recovery         | No materials                           |
    // |                                           |         | Yes  | None            | On-SS: add 2 banished Magnets to hand         | Have Sigmas in banish zone        | No banished targets                    |
    // | Magnet Bonding (65514302)                 | Spell   | Yes  | None            | Search Linear/Lv4-, search Lv8, or Fusion     | Main Phase / need search or fusion| Already activated this effect          |
    // | Conduction Warrior Plasma Magnum (47247792)| Fusion  | Yes  | None            | Fusion summoned (2 Earth Rock monsters)       | Extra body / extra normal summon  | Already on field                       |
    // |                                           |         | Yes  | Send Lv8 Magna  | Cost: send Lv8 Magna to GY; Normal Summon     | Need Tellusion in GY / extra sum. | No Magna in deck                       |
    // | Ice Ryzeal (8633261)                      | Monster | Yes  | Send 1 card     | SS from hand (Rank 4 locked)                  | Need Rank 4 setup                 | Hand/field empty                       |
    // |                                           |         | Yes  | None            | Normal summon: SS Ryzeal from Deck            | Need Ryzeal engine setup          | No Ryzeal in deck                      |
    // | Sword Ryzeal (35844557)                   | Monster | Yes  | None            | SS from hand if Ryzeal on field/GY            | Have Ryzeal / need Rank 4         | No Ryzeal / already used               |
    // | Ext Ryzeal (34022970)                     | Monster | Yes  | Send Xyz to GY  | SS from hand (Rank 4 locked)                  | Have extra Xyz / need Rank 4      | No Xyz in Extra / already used         |
    // | Node Ryzeal (72238166)                    | Monster | Yes  | Send 1 card     | Reborn Ryzeal from GY in DEF (negated)        | Need Rank 4 body                  | No Ryzeal in GY / no discard           |
    // | Ryzeal Duo Drive (7511613)                | Xyz     | Yes  | Detach 2        | Search 2 Ryzeal cards from deck               | Main Phase starter/extender       | Already used / < 2 materials           |
    // | Ryzeal Detonator (34909328)              | Xyz     | Yes  | Detach 1 (Quick)| Destroy 1 card on field                       | Opponent activates card / threat  | No materials                           |
    // | Ryzeal Cross (6798031)                    | Field   | Yes  | None            | Draw 1 (recycle 2 Ryzeals); Negate mon effect | Main Phase / Opponent mon effect  | Already activated                      |
    // | Ryzeal Plugin (60394026)                  | Quick   | Yes  | None            | Reborn Ryzeal/Xyz + attach from deck          | Need revive / disruption attach   | No target in GY                        |
    // | Adamancipator Researcher (85914562)       | Monster | Yes  | None            | SS from hand if control Rock; excavate 5      | Need Tuner (Naturia Beast)        | No Rock monster on field               |
    // | Naturia Beast (33198837)                  | Synchro | No   | Mill 2 (Quick)  | Negate and destroy Spell activation           | Opponent activates Spell card     | Self-activation                        |
    // | Gallant Granite (32530043)                | Xyz     | Yes  | Detach 1        | Search ANY Rock monster (Researcher/Guardian) | Need Rock searcher                | No target / no materials               |
    // | Abyss Dweller (21044178)                  | Xyz     | Yes  | Detach 1 (Quick)| Opponent cannot activate effects in GY        | Opponent DP/SP or chain response  | Our turn / already activated           |
    // | Koa'ki Meiru Guardian (45041488)          | Monster | Yes  | Tribute (Quick) | Negate monster effect activation & destroy    | Opponent activates monster effect | Self-activation                        |
    // ============================================================

    [Deck("2026_Magnet", "2026_Magnet")]
    public class _2026_MagnetExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int BystialMagnamhut = 33854624;
            public const int BystialDruiswurm = 6637331;
            public const int AdamancipatorResearcher = 85914562;
            public const int BetaTheElectromagnetWarrior = 79418928;
            public const int AshBlossom = 14558127;
            public const int MagnetWarriorSigmaMinus = 87814728;
            public const int MagnetWarriorSigmaPlus = 51826619;
            public const int EpsilonTheMagnetWarrior = 52566270;
            public const int KoaKiMeiruGuardian = 45041488;
            public const int MulcharmyFuwalos = 42141493;
            public const int ExtRyzeal = 34022970;
            public const int IceRyzeal = 8633261;
            public const int SwordRyzeal = 35844557;
            public const int NodeRyzeal = 72238166;
            public const int ConductionWarriorLinearMagnumPlusMinus = 44839512;
            public const int TellusionTheMagnaWarrior = 24431911;
            public const int MaxxC = 23434538;
            public const int Nibiru = 27204311;

            // Spells & Traps
            public const int RyzealCross = 6798031;
            public const int RyzealPlugin = 60394026;
            public const int MagnetBonding = 65514302;
            public const int SuperPolymerization = 48130397;
            public const int CalledByTheGrave = 24224830;
            public const int CrossoutDesignator = 65681983;
            public const int TripleTacticsTalent = 25311006;

            // Extra Deck
            public const int MudragonOfTheSwamp = 54757758;
            public const int Garura = 11765832;
            public const int ConductionWarriorPlasmaMagnet = 47247792;
            public const int NaturiaBeast = 33198837;
            public const int EvilswarmExcitonKnight = 46772449;
            public const int GallantGranite = 32530043;
            public const int RyzealDetonator = 34909328;
            public const int RyzealDuoDrive = 7511613;
            public const int IPMasquerena = 65741786;
            public const int SPLittleKnight = 29301450;
            public const int UnderworldGoddess = 98127546;
            public const int AbyssDweller = 21044178;
            public const int Bagooska = 90590303;
            public const int Number17LeviathanDragon = 69610924;

            // Side / Tech
            public const int MulcharmyPurulia = 84192580;
            public const int DrollAndLockBird = 94145021;
            public const int DarkRulerNoMore = 54693926;
            public const int TripleTacticsThrust = 35269904;
            public const int EvenlyMatched = 15693423;
            public const int GhostBelle = 73642296;
            public const int HarpiesFeatherDuster = 18144507;
            public const int Kumongous = 29726552;
        }

        private static readonly int[] MagnetCards = {
            CardId.BetaTheElectromagnetWarrior,
            CardId.EpsilonTheMagnetWarrior,
            CardId.MagnetWarriorSigmaMinus,
            CardId.MagnetWarriorSigmaPlus,
            CardId.ConductionWarriorLinearMagnumPlusMinus,
            CardId.TellusionTheMagnaWarrior,
            CardId.ConductionWarriorPlasmaMagnet
        };

        private static readonly int[] RyzealCards = {
            CardId.IceRyzeal,
            CardId.ExtRyzeal,
            CardId.SwordRyzeal,
            CardId.NodeRyzeal,
            CardId.RyzealCross,
            CardId.RyzealPlugin,
            CardId.RyzealDetonator,
            CardId.RyzealDuoDrive
        };

        private static readonly int[] HandTraps = {
            CardId.AshBlossom,
            CardId.GhostBelle,
            CardId.DrollAndLockBird,
            CardId.MulcharmyFuwalos,
            CardId.MulcharmyPurulia,
            CardId.BystialMagnamhut,
            CardId.BystialDruiswurm,
            CardId.MaxxC,
            CardId.Nibiru
        };

        // Once per turn state tracking
        private bool _bondingUsed = false;
        private bool _linearMagnumUsed = false;
        private bool _sigmaMinusUsed = false;
        private bool _sigmaPlusUsed = false;
        private bool _epsilonUsed = false;
        private bool _betaSearchUsed = false;
        private bool _betaTributeUsed = false;
        private bool _researcherUsed = false;
        private bool _crossoutUsed = false;
        private bool _thrustUsed = false;
        private bool _talentUsed = false;
        private bool _ryzealCrossRecycleUsed = false;
        private bool _ryzealCrossNegateUsed = false;
        private bool _dwellerUsedThisTurn = false;
        private bool _normalSummonedThisTurn = false;
        private bool _opponentMonsterEffectActivatedThisTurn = false;

        public override bool OnSelectHand()
        {
            // Going First to establish Naturia Beast / Ryzeal Detonator / Abyss Dweller / Tellusion disruption board
            return true;
        }

        public override void OnChaining(int player, ClientCard card)
        {
            if (card != null && card.Controller == 1 && card.IsMonster())
            {
                _opponentMonsterEffectActivatedThisTurn = true;
            }
            base.OnChaining(player, card);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _bondingUsed = false;
            _linearMagnumUsed = false;
            _sigmaMinusUsed = false;
            _sigmaPlusUsed = false;
            _epsilonUsed = false;
            _betaSearchUsed = false;
            _betaTributeUsed = false;
            _researcherUsed = false;
            _crossoutUsed = false;
            _thrustUsed = false;
            _talentUsed = false;
            _ryzealCrossRecycleUsed = false;
            _ryzealCrossNegateUsed = false;
            _dwellerUsedThisTurn = false;
            _normalSummonedThisTurn = false;
            _opponentMonsterEffectActivatedThisTurn = false;
        }

        public override bool ShouldAllowSpSummon(ClientCard card)
        {
            if (!base.ShouldAllowSpSummon(card)) return false;
            if (card == null) return true;

            if (card.HasType(CardType.Link | CardType.Xyz | CardType.Synchro | CardType.Fusion))
            {
                var activeAces = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && IsAceCard(c)).ToList();
                if (activeAces.Count > 0)
                {
                    var nonAces = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c)).ToList();
                    if (card.HasType(CardType.Xyz) && nonAces.Count(m => m.Level == 4) < 2 && activeAces.Any(a => a.Level == 4))
                    {
                        DecisionTracer.TraceSkip("ShouldAllowSpSummon", $"Summoning {card.Name} would consume Ace card(s)");
                        return false;
                    }
                }
            }
            return true;
        }

        private int GetFreeMonsterZoneCount()
        {
            int count = 0;
            for (int i = 0; i < 5; ++i)
            {
                if (Bot.MonsterZone[i] == null) count++;
            }
            return count;
        }

        private int GetAvailableCount(int cardId, int maxInDeck)
        {
            int inGrave = Bot.Graveyard.Count(c => c != null && c.Id == cardId);
            int inBanished = Bot.Banished.Count(c => c != null && c.Id == cardId);
            int inHand = Bot.Hand.Count(c => c != null && c.Id == cardId);
            int inField = Bot.GetMonsters().Count(c => c != null && c.Id == cardId) + Bot.GetSpells().Count(c => c != null && c.Id == cardId);
            return Math.Max(0, maxInDeck - inGrave - inBanished - inHand - inField);
        }

        public _2026_MagnetExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Register Ace Cards
            HeuristicGuard.RegisterAceCards(
                CardId.TellusionTheMagnaWarrior,
                CardId.NaturiaBeast,
                CardId.ConductionWarriorPlasmaMagnet,
                CardId.RyzealDetonator,
                CardId.RyzealDuoDrive,
                CardId.AbyssDweller,
                CardId.Bagooska,
                CardId.SPLittleKnight,
                CardId.IPMasquerena,
                CardId.UnderworldGoddess,
                CardId.EvilswarmExcitonKnight
            );
            ResourcePlan.RegisterAceCards(
                CardId.TellusionTheMagnaWarrior,
                CardId.NaturiaBeast,
                CardId.ConductionWarriorPlasmaMagnet,
                CardId.RyzealDetonator,
                CardId.RyzealDuoDrive,
                CardId.AbyssDweller,
                CardId.Bagooska,
                CardId.SPLittleKnight,
                CardId.IPMasquerena,
                CardId.UnderworldGoddess,
                CardId.EvilswarmExcitonKnight
            );

            // Combo Starters
            BaitPlanner.RegisterComboStarters(
                CardId.BetaTheElectromagnetWarrior,
                CardId.IceRyzeal,
                CardId.SwordRyzeal,
                CardId.MagnetBonding,
                CardId.ConductionWarriorLinearMagnumPlusMinus
            );
            BaitPlanner.RegisterBaitCards(CardId.MagnetBonding, CardId.RyzealCross);

            ChainAdvisor.RegisterHighValueTargets(CardId.ConductionWarriorLinearMagnumPlusMinus, CardId.AshBlossom);

            // ==========================================
            // TIER 1: Hand Traps & Reactive Negations
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, () => SmartHandTrapChain() && DefaultMaxxC());
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyPurulia, MulcharmyEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelle, GhostBelleEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdEffect);
            AddExecutor(ExecutorType.Activate, CardId.Nibiru, NibiruEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.CrossoutDesignator, CrossoutEffect);
            AddExecutor(ExecutorType.Activate, CardId.BystialMagnamhut, BystialMagnamhutEffect);
            AddExecutor(ExecutorType.Activate, CardId.BystialDruiswurm, BystialDruiswurmEffect);

            // ==========================================
            // TIER 2: Field Disruptions & Quick Effects
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.NaturiaBeast, NaturiaBeastEffect);
            AddExecutor(ExecutorType.Activate, CardId.RyzealCross, RyzealCrossDisruptionEffect);
            AddExecutor(ExecutorType.Activate, CardId.RyzealDetonator, RyzealDetonatorEffect);
            AddExecutor(ExecutorType.Activate, CardId.KoaKiMeiruGuardian, KoaKiMeiruGuardianEffect);
            AddExecutor(ExecutorType.Activate, CardId.AbyssDweller, AbyssDwellerEffect);
            AddExecutor(ExecutorType.Activate, CardId.TellusionTheMagnaWarrior, TellusionQuickEffect);
            AddExecutor(ExecutorType.Activate, CardId.BetaTheElectromagnetWarrior, BetaOpponentTurnTributeEffect);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightEffect);
            AddExecutor(ExecutorType.Activate, CardId.IPMasquerena, IPMasquerenaEffect);
            AddExecutor(ExecutorType.Activate, CardId.EvilswarmExcitonKnight, ExcitonKnightEffect);

            // ==========================================
            // TIER 3: Spells & Fusion Board Breakers
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.DarkRulerNoMore, DarkRulerNoMoreEffect);
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolyEffect);
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, DefaultHarpiesFeatherDusterFirst);
            AddExecutor(ExecutorType.Activate, CardId.RyzealCross, RyzealCrossActivateEffect);
            AddExecutor(ExecutorType.Activate, CardId.MagnetBonding, MagnetBondingSearchEffect);
            AddExecutor(ExecutorType.Activate, CardId.MagnetBonding, MagnetBondingFusionEffect);
            AddExecutor(ExecutorType.Activate, CardId.RyzealPlugin, RyzealPluginEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TalentEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsThrust, ThrustEffect);

            // ==========================================
            // TIER 4: Normal Summons (Prioritize Starters)
            // ==========================================
            AddExecutor(ExecutorType.Summon, CardId.BetaTheElectromagnetWarrior, BetaNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.IceRyzeal, IceRyzealNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.SwordRyzeal, SwordRyzealNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.EpsilonTheMagnetWarrior, EpsilonNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.KoaKiMeiruGuardian, GuardianNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.MagnetWarriorSigmaMinus, SigmaMinusNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.NodeRyzeal, NodeRyzealNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.AdamancipatorResearcher, ResearcherNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.MagnetWarriorSigmaPlus);

            // ==========================================
            // TIER 5: Summon Trigger Effects
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.IceRyzeal, IceRyzealSummonEffect);
            AddExecutor(ExecutorType.Activate, CardId.SwordRyzeal, SwordRyzealSearchEffect);
            AddExecutor(ExecutorType.Activate, CardId.BetaTheElectromagnetWarrior, BetaSearchEffect);
            AddExecutor(ExecutorType.Activate, CardId.EpsilonTheMagnetWarrior, EpsilonSendEffect);
            AddExecutor(ExecutorType.Activate, CardId.MagnetWarriorSigmaMinus, SigmaMinusGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.ExtRyzeal, ExtRyzealSearchEffect);
            AddExecutor(ExecutorType.Activate, CardId.ConductionWarriorPlasmaMagnet, PlasmaMagnumEffect);
            AddExecutor(ExecutorType.Activate, CardId.MudragonOfTheSwamp, MudragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.Garura, GaruraDrawEffect);

            // ==========================================
            // TIER 6: Synchro Summon (Naturia Beast FIRST before Ryzeal lock!)
            // ==========================================
            AddExecutor(ExecutorType.SpSummon, CardId.AdamancipatorResearcher, ResearcherSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.AdamancipatorResearcher, ResearcherEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.NaturiaBeast, NaturiaBeastSummon);

            // ==========================================
            // TIER 7: Monster Hand / GY Ignitions
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.MagnetWarriorSigmaPlus, SigmaPlusEffect);
            AddExecutor(ExecutorType.Activate, CardId.MagnetWarriorSigmaMinus, SigmaMinusFusionEffect);
            AddExecutor(ExecutorType.Activate, CardId.TellusionTheMagnaWarrior, TellusionGYSummonEffect);
            AddExecutor(ExecutorType.Activate, CardId.NodeRyzeal, NodeRyzealEffect);
            AddExecutor(ExecutorType.Activate, CardId.ConductionWarriorLinearMagnumPlusMinus, LinearMagnumEffect);

            // ==========================================
            // TIER 8: Special Summons from Hand (Extenders)
            // ==========================================
            AddExecutor(ExecutorType.SpSummon, CardId.Kumongous, KumongousSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.BystialMagnamhut, BystialSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.BystialDruiswurm, BystialSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ConductionWarriorLinearMagnumPlusMinus, LinearMagnumSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ExtRyzeal, ExtRyzealSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SwordRyzeal, SwordRyzealSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.NodeRyzeal, NodeRyzealSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.IceRyzeal, IceRyzealSpSummon);

            // ==========================================
            // TIER 9: Extra Deck Summons
            // ==========================================
            AddExecutor(ExecutorType.SpSummon, CardId.ConductionWarriorPlasmaMagnet, PlasmaMagnumFusionSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.RyzealDetonator, DetonatorSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.RyzealDuoDrive, DuoDriveSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AbyssDweller, AbyssDwellerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.GallantGranite, GallantGraniteSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.EvilswarmExcitonKnight, ExcitonKnightSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Bagooska, BagooskaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.UnderworldGoddess, UnderworldGoddessSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.IPMasquerena, IPMasquerenaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Number17LeviathanDragon, LeviathanDragonSummon);

            // Extra Deck Ignitions
            AddExecutor(ExecutorType.Activate, CardId.RyzealDuoDrive, DuoDriveEffect);
            AddExecutor(ExecutorType.Activate, CardId.GallantGranite, GallantGraniteEffect);
            AddExecutor(ExecutorType.Activate, CardId.Number17LeviathanDragon, LeviathanDragonEffect);

            // ==========================================
            // TIER 10: Backrow Sets & Repositions
            // ==========================================
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave);
            AddExecutor(ExecutorType.SpellSet, CardId.SuperPolymerization);
            AddExecutor(ExecutorType.SpellSet, CardId.RyzealPlugin);
            AddExecutor(ExecutorType.SpellSet, CardId.EvenlyMatched);

            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        // ==========================================
        //  TIER 1: Hand Traps & Counter Measures
        // ==========================================

        private bool MulcharmyEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }

        private bool AshBlossomEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.LastChainPlayer == 1 && DefaultAshBlossomAndJoyousSpring();
        }

        private bool GhostBelleEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.LastChainPlayer == 1 && DefaultGhostBelleAndHauntedMansion();
        }

        private bool DrollAndLockBirdEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.Player == 1 && Duel.LastChainPlayer == 1;
        }

        private bool CalledByTheGraveEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.LastChainPlayer == 1 && DefaultCalledByTheGrave();
        }

        private bool CrossoutEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (_crossoutUsed) return false;
            if (Duel.LastChainPlayer == 1)
            {
                var lastCard = Util.GetLastChainCard();
                if (lastCard != null && (lastCard.IsMonster() || lastCard.IsSpell() || lastCard.IsTrap()))
                {
                    int code = lastCard.Id;
                    int alias = lastCard.Alias;
                    if (alias != 0 && alias - code < 10) code = alias;
                    if (code == 0) return false;
                    if (GetRemainingCount(code) > 0)
                    {
                        _crossoutUsed = true;
                        AI.SelectAnnounceID(code);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool NibiruEffect()
        {
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                if (Enemy.GetMonsterCount() >= 2 || Enemy.GetMonsters().Any(c => c.Attack >= 2500))
                {
                    return true;
                }
            }
            return false;
        }

        // ==========================================
        //  TIER 2: Quick Effects & Disruptions
        // ==========================================

        private bool RyzealCrossDisruptionEffect()
        {
            if (Card.Location != CardLocation.SpellZone) return false;
            if (_ryzealCrossNegateUsed) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;

            if (Duel.LastChainPlayer == 1)
            {
                bool hasMaterial = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.RyzealDetonator, CardId.RyzealDuoDrive) && c.Overlays.Count > 0);
                if (hasMaterial)
                {
                    _ryzealCrossNegateUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool RyzealDetonatorEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Trigger when summoned: attach 1 monster from GY
            if (Card.Overlays.Count == 0 || ActivateDescription == Util.GetStringId(CardId.RyzealDetonator, 0) || ActivateDescription == 0)
            {
                var gyMonsters = Bot.Graveyard.Where(c => c != null && c.IsMonster()).ToList();
                if (gyMonsters.Count > 0)
                {
                    AI.SelectCard(gyMonsters.OrderBy(c => IsAceCard(c) ? 100 : 0).First());
                    return true;
                }
            }

            // Quick effect: detach 1 to destroy 1 card on field
            if (Card.Overlays.Count > 0)
            {
                var validTargets = Enemy.GetMonsters().Concat(Enemy.GetSpells()).Where(c => {
                    if (c == null || c.IsShouldNotBeTarget()) return false;
                    if (Duel.LastChainPlayer == 1 && LastChainCard == c && (c.HasType(CardType.Normal) || c.HasType(CardType.QuickPlay))) return false;
                    return true;
                }).ToList();

                if (validTargets.Count > 0)
                {
                    if (Duel.LastChainPlayer == 1 || Duel.Player == 1 || (Duel.Player == 0 && Enemy.GetFieldCount() > 0))
                    {
                        var priorityTarget = validTargets.OrderByDescending(c => {
                            int score = 0;
                            if (c.IsMonster() && c.IsFaceup())
                            {
                                score += 60;
                                if (c.Attack >= 2500) score += 40;
                                if (c.HasType(CardType.Fusion | CardType.Synchro | CardType.Xyz | CardType.Link)) score += 30;
                            }
                            else if (c.IsSpell() || c.IsTrap())
                            {
                                if (c.IsFaceup() && (c.HasType(CardType.Continuous) || c.HasType(CardType.Field))) score += 50;
                                else if (c.IsFacedown()) score += 40;
                            }
                            return score;
                        }).First();

                        AI.SelectCard(priorityTarget);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool NaturiaBeastEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.LastChainPlayer == 1;
        }

        private bool KoaKiMeiruGuardianEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.LastChainPlayer == 1;
        }

        private bool AbyssDwellerEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Card.Location != CardLocation.MonsterZone || Card.Overlays.Count == 0) return false;
            if (_dwellerUsedThisTurn) return false;

            if (Duel.Player == 1)
            {
                _dwellerUsedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool TellusionQuickEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Card.Location != CardLocation.MonsterZone) return false;

            if (Duel.Player == 1)
            {
                var targets = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup() && !m.IsShouldNotBeTarget()).ToList();
                if (targets.Count > 0)
                {
                    var earthTarget = targets.FirstOrDefault(m => m.HasAttribute(CardAttribute.Earth));
                    if (earthTarget != null) AI.SelectCard(earthTarget);
                    else AI.SelectCard(targets.OrderByDescending(m => m.Attack).First());
                    return true;
                }

                bool hasPlus = Bot.Banished.Any(c => c != null && c.IsCode(CardId.MagnetWarriorSigmaPlus));
                bool hasMinus = Bot.Banished.Any(c => c != null && c.IsCode(CardId.MagnetWarriorSigmaMinus));
                if (hasPlus && hasMinus && GetFreeMonsterZoneCount() >= 2)
                {
                    var sigmas = Bot.Banished.Where(c => c.IsCode(CardId.MagnetWarriorSigmaPlus, CardId.MagnetWarriorSigmaMinus)).ToList();
                    AI.SelectCard(sigmas);
                    return true;
                }
            }
            return false;
        }

        private bool BetaOpponentTurnTributeEffect()
        {
            if (Duel.Player != 1) return false;
            if (_betaTributeUsed) return false;
            if (Card.Location != CardLocation.MonsterZone) return false;

            var targetsInDeck = new[] {
                CardId.EpsilonTheMagnetWarrior,
                CardId.MagnetWarriorSigmaMinus,
                CardId.MagnetWarriorSigmaPlus
            };

            bool hasTarget = targetsInDeck.Any(id => GetRemainingCount(id) > 0);
            if (hasTarget)
            {
                _betaTributeUsed = true;
                AI.SelectCard(targetsInDeck);
                return true;
            }
            return false;
        }

        private bool SPLittleKnightEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;

            if (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0 || Enemy.Graveyard.Count > 0)
            {
                return true;
            }

            if (Duel.LastChainPlayer == 1)
            {
                bool isTargetingUs = Duel.ChainTargets.Any(c => c != null && c.Controller == 0);
                if (isTargetingUs) return true;
            }

            return false;
        }

        private bool IPMasquerenaEffect()
        {
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                if (IsSpecialSummonBlocked()) return false;
                return true;
            }
            return false;
        }

        private bool ExcitonKnightEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            int ourCount = Bot.Hand.Count + Bot.GetFieldCount();
            int enemyCount = Enemy.Hand.Count + Enemy.GetFieldCount();
            return enemyCount > ourCount;
        }

        // ==========================================
        //  TIER 3: Spells & Board Breakers
        // ==========================================

        private bool DarkRulerNoMoreEffect()
        {
            if (Duel.Player != 0 || !Duel.IsMainPhase()) return false;
            int enemyFaceupCount = Enemy.GetMonsters().Count(c => c != null && c.IsFaceup() && !c.IsDisabled());
            return enemyFaceupCount >= 2;
        }

        private bool SuperPolyEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Bot.Hand.Count == 0) return false;

            if (Enemy.GetMonsterCount() >= 2 || (Enemy.GetMonsterCount() >= 1 && Bot.GetMonsterCount() >= 1))
            {
                return true;
            }
            return false;
        }

        private bool RyzealCrossActivateEffect()
        {
            if (Duel.Player != 0 || !Duel.IsMainPhase()) return false;

            if (Card.Location == CardLocation.SpellZone)
            {
                if (_ryzealCrossRecycleUsed) return false;
                int ryzealInGyOrBanish = Bot.Graveyard.Concat(Bot.Banished).Count(c => c != null && c.IsCode(RyzealCards) && c.Id != CardId.RyzealCross);
                if (ryzealInGyOrBanish >= 2)
                {
                    _ryzealCrossRecycleUsed = true;
                    return true;
                }
                return false;
            }

            if (Card.Location == CardLocation.Hand)
            {
                bool alreadyOnField = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.RyzealCross));
                if (alreadyOnField) return false;
                return true;
            }
            return false;
        }

        private bool MagnetBondingFusionEffect()
        {
            if (_bondingUsed) return false;
            if (ShouldSkipCombo()) return false;
            if (Bot.HasInMonstersZone(CardId.ConductionWarriorPlasmaMagnet)) return false;

            var materials = Bot.Hand.Concat(Bot.GetMonsters()).Concat(Bot.Graveyard).Concat(Bot.Banished)
                .Where(c => c != null && c.HasRace(CardRace.Rock) && !IsAceCard(c) && (!c.IsCode(CardId.BetaTheElectromagnetWarrior) || c.Location != CardLocation.MonsterZone)).ToList();

            if (materials.Count >= 2 && GetRemainingCount(CardId.ConductionWarriorPlasmaMagnet) > 0)
            {
                _bondingUsed = true;
                AI.SelectOption(2);
                return true;
            }
            return false;
        }

        private bool RyzealPluginEffect()
        {
            var targets = Bot.Graveyard.Concat(Bot.Banished)
                .Where(c => c != null && c.IsMonster() && (c.IsCode(RyzealCards) || c.HasType(CardType.Xyz)) && c.IsCanRevive()).ToList();

            if (targets.Count == 0) return false;

            if (Duel.Player == 0 && Duel.IsMainPhase())
            {
                return true;
            }

            if (Duel.Player == 1)
            {
                bool hasDetonator = targets.Any(c => c.IsCode(CardId.RyzealDetonator));
                if (hasDetonator && GetFreeMonsterZoneCount() > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private bool MagnetBondingSearchEffect()
        {
            if (_bondingUsed) return false;
            if (ShouldSkipCombo()) return false;

            bool hasLinear = Bot.HasInHand(CardId.ConductionWarriorLinearMagnumPlusMinus) || Bot.HasInMonstersZone(CardId.ConductionWarriorLinearMagnumPlusMinus);
            if (!hasLinear && GetRemainingCount(CardId.ConductionWarriorLinearMagnumPlusMinus) > 0)
            {
                _bondingUsed = true;
                AI.SelectOption(0);
                AI.SelectCard(CardId.ConductionWarriorLinearMagnumPlusMinus);
                return true;
            }

            bool hasTellusion = Bot.HasInHand(CardId.TellusionTheMagnaWarrior) || Bot.HasInMonstersZone(CardId.TellusionTheMagnaWarrior) || Bot.Graveyard.Any(c => c.IsCode(CardId.TellusionTheMagnaWarrior));
            if (!hasTellusion && GetRemainingCount(CardId.TellusionTheMagnaWarrior) > 0)
            {
                _bondingUsed = true;
                AI.SelectOption(1);
                AI.SelectCard(CardId.TellusionTheMagnaWarrior);
                return true;
            }

            _bondingUsed = true;
            AI.SelectOption(0);
            AI.SelectCard(new[] {
                CardId.BetaTheElectromagnetWarrior,
                CardId.MagnetWarriorSigmaMinus,
                CardId.MagnetWarriorSigmaPlus,
                CardId.EpsilonTheMagnetWarrior
            });
            return true;
        }

        private bool ThrustEffect()
        {
            if (_thrustUsed) return false;
            if (Duel.Player == 0 && _opponentMonsterEffectActivatedThisTurn)
            {
                _thrustUsed = true;
                AI.SelectCard(new[] {
                    CardId.HarpiesFeatherDuster,
                    CardId.SuperPolymerization,
                    CardId.MagnetBonding,
                    CardId.TripleTacticsTalent
                });
                return true;
            }
            return false;
        }

        private bool TalentEffect()
        {
            if (_talentUsed) return false;
            if (Duel.Player == 0 && _opponentMonsterEffectActivatedThisTurn)
            {
                _talentUsed = true;
                if (_isGoingSecond && Enemy.GetMonsterCount() > 0 && Enemy.GetMonsters().Any(c => c.Attack >= 2500 && !c.IsShouldNotBeTarget()))
                {
                    AI.SelectOption(1);
                }
                else
                {
                    AI.SelectOption(0);
                }
                return true;
            }
            return false;
        }

        // ==========================================
        //  TIER 4: Normal Summons
        // ==========================================

        private bool BetaNormalSummon()
        {
            _normalSummonedThisTurn = true;
            return true;
        }

        private bool IceRyzealNormalSummon()
        {
            _normalSummonedThisTurn = true;
            return true;
        }

        private bool SwordRyzealNormalSummon()
        {
            _normalSummonedThisTurn = true;
            return true;
        }

        private bool EpsilonNormalSummon()
        {
            _normalSummonedThisTurn = true;
            return true;
        }

        private bool GuardianNormalSummon()
        {
            _normalSummonedThisTurn = true;
            return true;
        }

        private bool SigmaMinusNormalSummon()
        {
            _normalSummonedThisTurn = true;
            return true;
        }

        private bool NodeRyzealNormalSummon()
        {
            _normalSummonedThisTurn = true;
            return true;
        }

        private bool ResearcherNormalSummon()
        {
            _normalSummonedThisTurn = true;
            return true;
        }

        // ==========================================
        //  TIER 5: Summon Trigger Effects
        // ==========================================

        private bool IceRyzealSummonEffect()
        {
            AI.SelectCard(new[] {
                CardId.SwordRyzeal,
                CardId.NodeRyzeal,
                CardId.ExtRyzeal
            });
            return true;
        }

        private bool SwordRyzealSearchEffect()
        {
            AI.SelectCard(new[] {
                CardId.NodeRyzeal,
                CardId.SwordRyzeal
            });
            return true;
        }

        private bool ExtRyzealSearchEffect()
        {
            AI.SelectCard(new[] {
                CardId.IceRyzeal,
                CardId.ExtRyzeal
            });
            return true;
        }

        private bool BetaSearchEffect()
        {
            if (Duel.Player == 1) return false;
            if (_betaSearchUsed) return false;
            _betaSearchUsed = true;
            AI.SelectCard(new[] {
                CardId.MagnetWarriorSigmaMinus,
                CardId.MagnetWarriorSigmaPlus,
                CardId.EpsilonTheMagnetWarrior,
                CardId.BetaTheElectromagnetWarrior
            });
            return true;
        }

        private bool EpsilonSendEffect()
        {
            if (_epsilonUsed) return false;
            _epsilonUsed = true;
            AI.SelectCard(new[] {
                CardId.MagnetWarriorSigmaPlus,
                CardId.MagnetWarriorSigmaMinus,
                CardId.BetaTheElectromagnetWarrior
            });
            AI.SelectNextCard(new[] {
                CardId.BetaTheElectromagnetWarrior,
                CardId.MagnetWarriorSigmaMinus,
                CardId.MagnetWarriorSigmaPlus
            });
            return true;
        }

        private bool SigmaMinusGraveEffect()
        {
            if (_sigmaMinusUsed) return false;
            if (Card.Location == CardLocation.MonsterZone)
            {
                _sigmaMinusUsed = true;
                AI.SelectCard(CardId.TellusionTheMagnaWarrior);
                return true;
            }
            return false;
        }

        private bool PlasmaMagnumEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                bool hasMagna = GetRemainingCount(CardId.TellusionTheMagnaWarrior) > 0;
                if (hasMagna)
                {
                    AI.SelectCard(CardId.TellusionTheMagnaWarrior);
                    AI.SelectNextCard(new[] {
                        CardId.BetaTheElectromagnetWarrior,
                        CardId.EpsilonTheMagnetWarrior,
                        CardId.MagnetWarriorSigmaMinus,
                        CardId.MagnetWarriorSigmaPlus
                    });
                    return true;
                }
            }
            return false;
        }

        private bool MudragonEffect()
        {
            AI.SelectOption(0);
            return true;
        }

        private bool GaruraDrawEffect()
        {
            return true;
        }

        // ==========================================
        //  TIER 6: Synchro Summon Setup & Naturia Beast
        // ==========================================

        private bool ResearcherSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            bool controlRock = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Rock) && c.Id != CardId.AdamancipatorResearcher);
            return controlRock;
        }

        private bool ResearcherEffect()
        {
            if (_researcherUsed) return false;
            if (Card.Location == CardLocation.MonsterZone && Duel.IsMainPhase())
            {
                _researcherUsed = true;
                return true;
            }
            return false;
        }

        private bool NaturiaBeastSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;

            bool hasTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.AdamancipatorResearcher) && !IsAceCard(c));
            bool hasBeta = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.BetaTheElectromagnetWarrior) && !IsAceCard(c));
            return hasTuner && hasBeta;
        }

        private bool IsNaturiaBeastSetupAvailable()
        {
            if (GetRemainingCount(CardId.NaturiaBeast) == 0) return false;
            if (Bot.HasInMonstersZone(CardId.NaturiaBeast)) return false;

            bool researcherOnField = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.AdamancipatorResearcher));
            bool betaOnField = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.BetaTheElectromagnetWarrior));
            if (researcherOnField && betaOnField) return true;
            if (betaOnField && Bot.HasInHand(CardId.AdamancipatorResearcher)) return true;
            if (researcherOnField && Bot.HasInHand(CardId.BetaTheElectromagnetWarrior) && !_betaSearchUsed) return true;

            return false;
        }

        private bool CanSummonNaturiaBeastNow()
        {
            if (IsSpecialSummonBlocked()) return false;
            bool hasTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.AdamancipatorResearcher) && !IsAceCard(c));
            bool hasBeta = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.BetaTheElectromagnetWarrior) && !IsAceCard(c));
            return hasTuner && hasBeta && GetRemainingCount(CardId.NaturiaBeast) > 0;
        }

        // ==========================================
        //  TIER 7: Monster Hand / GY Ignitions
        // ==========================================

        private bool SigmaPlusEffect()
        {
            if (_sigmaPlusUsed) return false;
            if (Card.Location == CardLocation.Grave)
            {
                var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && c.Level <= 4 && c.IsCode(MagnetCards) && c != Card && c.IsCanRevive());
                if (target != null)
                {
                    _sigmaPlusUsed = true;
                    AI.SelectCard(target);
                    if (GetFreeMonsterZoneCount() > 0) AI.SelectOption(1);
                    else AI.SelectOption(0);
                    return true;
                }
            }
            return false;
        }

        private bool SigmaMinusFusionEffect()
        {
            if (_sigmaMinusUsed) return false;
            if (Card.Location == CardLocation.Hand && Duel.IsMainPhase())
            {
                var rockMonsters = Bot.Hand.Concat(Bot.GetMonsters()).Where(c => c != null && c.HasRace(CardRace.Rock) && c != Card && !IsAceCard(c)).ToList();
                if (rockMonsters.Count >= 1 && GetRemainingCount(CardId.ConductionWarriorPlasmaMagnet) > 0)
                {
                    _sigmaMinusUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool TellusionGYSummonEffect()
        {
            if (Card.Location == CardLocation.Grave && Card.IsCanRevive())
            {
                if (Bot.HasInMonstersZone(CardId.TellusionTheMagnaWarrior)) return false;
                if (GetFreeMonsterZoneCount() == 0) return false;

                var cards = Bot.Hand.Concat(Bot.GetMonsters()).Concat(Bot.Graveyard).ToList();
                bool hasPlus = cards.Any(c => c != null && c.IsCode(CardId.MagnetWarriorSigmaPlus) && (c.Location != CardLocation.MonsterZone || (!IsAceCard(c) && c.IsFaceup())));
                bool hasMinus = cards.Any(c => c != null && c.IsCode(CardId.MagnetWarriorSigmaMinus) && (c.Location != CardLocation.MonsterZone || (!IsAceCard(c) && c.IsFaceup())));

                return hasPlus && hasMinus;
            }
            return false;
        }

        private bool NodeRyzealEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Duel.Player != 0 || !Duel.IsMainPhase()) return false;

            var targets = Bot.Graveyard.Where(c => c != null && c.IsMonster() && c.IsCode(RyzealCards) && c.Id != CardId.NodeRyzeal && c.IsCanRevive()).ToList();
            if (targets.Count == 0) return false;

            var costCandidates = Bot.Hand.Concat(Bot.GetMonsters()).Where(c => c != Card && !IsAceCard(c) && !HandTraps.Contains(c.Id)).ToList();
            return costCandidates.Count > 0;
        }

        private bool LinearMagnumEffect()
        {
            if (_linearMagnumUsed) return false;
            if (Card.Location == CardLocation.MonsterZone)
            {
                var targets = Bot.Banished.Where(c => c != null && c.IsMonster() && c.IsCode(MagnetCards)).Take(2).ToList();
                if (targets.Count > 0)
                {
                    _linearMagnumUsed = true;
                    AI.SelectCard(targets);
                    return true;
                }
            }
            return false;
        }

        // ==========================================
        //  TIER 8: Special Summons from Hand (Extenders)
        // ==========================================

        private bool KumongousSummon()
        {
            var targets = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            if (targets.Count > 0)
            {
                var target = targets.OrderByDescending(c => {
                    int score = 0;
                    if (c.Attack >= 2500) score += 50;
                    if (IsTargetImmune(c)) score += 100;
                    return score;
                }).First();

                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool BystialSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            ClientCard target = GetBystialTarget();
            if (target != null)
            {
                AI.SelectCard(target);
                AI.SelectPosition(CardPosition.FaceUpDefence);
                return true;
            }
            return false;
        }

        private bool BystialMagnamhutEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (IsSpecialSummonBlocked()) return false;
                ClientCard target = GetBystialTarget();
                if (target != null)
                {
                    AI.SelectCard(target);
                    AI.SelectPosition(CardPosition.FaceUpDefence);
                    return true;
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool BystialDruiswurmEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (IsSpecialSummonBlocked()) return false;
                ClientCard target = GetBystialTarget();
                if (target != null)
                {
                    AI.SelectCard(target);
                    AI.SelectPosition(CardPosition.FaceUpDefence);
                    return true;
                }
                return false;
            }
            else
            {
                var targets = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && c.IsSpecialSummoned && !c.IsShouldNotBeTarget()).ToList();
                if (targets.Count > 0)
                {
                    AI.SelectCard(targets.OrderByDescending(c => c.Attack).First());
                    return true;
                }
                return false;
            }
        }

        private ClientCard GetBystialTarget()
        {
            var opponentTargets = Enemy.Graveyard.Where(c => c != null && c.IsMonster() && (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark))).ToList();
            if (opponentTargets.Count > 0)
            {
                return opponentTargets.OrderByDescending(c => c.Attack).First();
            }

            if (Duel.Turn > 1 && Bot.GetMonsterCount() == 0)
            {
                var ourTargets = Bot.Graveyard.Where(c => c != null && c.IsMonster() && (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark)) && !IsAceCard(c) && !c.IsCode(RyzealCards) && !c.IsCode(MagnetCards)).ToList();
                if (ourTargets.Count > 0)
                {
                    return ourTargets.First();
                }
            }

            return null;
        }

        private bool LinearMagnumSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var lv4Magnets = new[] {
                CardId.BetaTheElectromagnetWarrior,
                CardId.EpsilonTheMagnetWarrior,
                CardId.MagnetWarriorSigmaMinus,
                CardId.MagnetWarriorSigmaPlus
            };
            var magnetHands = Bot.Hand.Where(c => c != null && c != Card && c.IsCode(lv4Magnets) && !IsAceCard(c)).ToList();
            return magnetHands.Count >= 2;
        }

        private bool ExtRyzealSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (CanSummonNaturiaBeastNow() || IsNaturiaBeastSetupAvailable()) return false;

            var extraXyz = Bot.ExtraDeck.Where(c => c != null && c.HasType(CardType.Xyz)).ToList();
            return extraXyz.Count > 0;
        }

        private bool IceRyzealSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (!_normalSummonedThisTurn) return false;
            if (CanSummonNaturiaBeastNow() || IsNaturiaBeastSetupAvailable()) return false;

            var costs = Bot.Hand.Concat(Bot.GetMonsters()).Where(c => c != Card && !IsAceCard(c) && !HandTraps.Contains(c.Id)).ToList();
            return costs.Count > 0;
        }

        private bool SwordRyzealSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (CanSummonNaturiaBeastNow() || IsNaturiaBeastSetupAvailable()) return false;

            bool hasRyzeal = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(RyzealCards)) ||
                             Bot.Graveyard.Any(c => c != null && c.IsCode(RyzealCards));
            return hasRyzeal;
        }

        private bool NodeRyzealSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (CanSummonNaturiaBeastNow() || IsNaturiaBeastSetupAvailable()) return false;

            bool hasXyz = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Xyz)) ||
                          Bot.Graveyard.Any(c => c != null && c.HasType(CardType.Xyz));
            return hasXyz;
        }

        // ==========================================
        //  TIER 9: Extra Deck Summons
        // ==========================================

        private bool PlasmaMagnumFusionSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;

            var materials = Bot.Hand.Concat(Bot.GetMonsters()).Concat(Bot.Graveyard).Concat(Bot.Banished)
                .Where(c => c != null && c.HasRace(CardRace.Rock) && !IsAceCard(c) && !HandTraps.Contains(c.Id)).ToList();
            return materials.Count >= 2;
        }

        private bool DuoDriveSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            if (Bot.HasInMonstersZone(CardId.RyzealDuoDrive)) return false;

            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.Level == 4 && !IsAceCard(c)).ToList();
            return materials.Count >= 2;
        }

        private bool DetonatorSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;

            // Detonator requires 2+ Level 4 Ryzeal monsters
            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.Level == 4 && c.IsCode(RyzealCards) && !IsAceCard(c)).ToList();
            return materials.Count >= 2;
        }

        private bool GallantGraniteSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            if (Bot.HasInMonstersZone(CardId.GallantGranite)) return false;

            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.Level == 4 && !IsAceCard(c)).ToList();
            return materials.Count >= 2;
        }

        private bool AbyssDwellerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            if (Bot.HasInMonstersZone(CardId.AbyssDweller)) return false;

            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.Level == 4 && !IsAceCard(c)).ToList();
            if (materials.Count < 2) return false;

            return true;
        }

        private bool ExcitonKnightSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            int ourCount = Bot.GetFieldCount() + Bot.Hand.Count;
            int enemyCount = Enemy.GetFieldCount() + Enemy.Hand.Count;
            if (enemyCount <= ourCount) return false;

            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.Level == 4 && !IsAceCard(c)).ToList();
            return materials.Count >= 2;
        }

        private bool BagooskaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.HasInMonstersZone(CardId.NaturiaBeast)) return false;
            if (Bot.HasInMonstersZone(CardId.RyzealDetonator)) return false;

            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.Level == 4 && !IsAceCard(c)).ToList();
            if (materials.Count < 2) return false;

            bool lowResources = Bot.Hand.Count <= 1;
            if (lowResources || IsInGrindGame())
            {
                AI.SelectPosition(CardPosition.FaceUpDefence);
                return true;
            }
            return false;
        }

        private bool SPLittleKnightSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;

            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect) && (!IsAceCard(c) || c.IsCode(CardId.IPMasquerena))).ToList();
            return materials.Count >= 2;
        }

        private bool IPMasquerenaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;

            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.HasType(CardType.Link) && !IsAceCard(c)).ToList();
            return materials.Count >= 2;
        }

        private bool UnderworldGoddessSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var opponentBosses = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && (c.Attack >= 2500 || IsTargetImmune(c))).ToList();
            if (opponentBosses.Count == 0) return false;

            var ourMaterials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect) && !IsAceCard(c)).ToList();
            if (ourMaterials.Count >= 3)
            {
                AI.SelectCard(opponentBosses.OrderByDescending(c => c.Attack).First());
                return true;
            }
            return false;
        }

        private bool LeviathanDragonSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.Level == 3 && !IsAceCard(c)).ToList();
            return materials.Count >= 2 && CanDealLethal();
        }

        private bool LeviathanDragonEffect()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.Overlays.Count > 0 && Duel.Player == 0)
            {
                return true;
            }
            return false;
        }

        // ==========================================
        //  Extra Deck Monster Effects
        // ==========================================

        private bool DuoDriveEffect()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.Overlays.Count >= 2 && Duel.Player == 0)
            {
                return true;
            }

            if (ActivateDescription == Util.GetStringId(CardId.RyzealDuoDrive, 0) || ActivateDescription == 0)
            {
                var gyRyzeal = Bot.Graveyard.Where(c => c != null && c.IsCode(RyzealCards)).ToList();
                if (gyRyzeal.Count > 0)
                {
                    AI.SelectCard(gyRyzeal.First());
                    return true;
                }
            }
            return false;
        }

        private bool GallantGraniteEffect()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.Overlays.Count >= 1 && Duel.Player == 0)
            {
                AI.SelectOption(0);

                bool hasResearcher = Bot.HasInHand(CardId.AdamancipatorResearcher) || Bot.HasInMonstersZone(CardId.AdamancipatorResearcher);
                if (!hasResearcher && !_researcherUsed && GetRemainingCount(CardId.AdamancipatorResearcher) > 0)
                {
                    AI.SelectCard(CardId.AdamancipatorResearcher);
                    return true;
                }

                bool hasGuardian = Bot.HasInHand(CardId.KoaKiMeiruGuardian) || Bot.HasInMonstersZone(CardId.KoaKiMeiruGuardian);
                if (!hasGuardian && GetRemainingCount(CardId.KoaKiMeiruGuardian) > 0)
                {
                    AI.SelectCard(CardId.KoaKiMeiruGuardian);
                    return true;
                }

                AI.SelectCard(new[] {
                    CardId.BetaTheElectromagnetWarrior,
                    CardId.MagnetWarriorSigmaMinus,
                    CardId.ConductionWarriorLinearMagnumPlusMinus,
                    CardId.Nibiru,
                    CardId.TellusionTheMagnaWarrior,
                    CardId.MagnetWarriorSigmaPlus,
                    CardId.EpsilonTheMagnetWarrior
                });
                return true;
            }
            return false;
        }

        // ==========================================
        //  OnSelectCard Overrides & Location Filters
        // ==========================================

        // ==========================================
        //  OnSelectCard Overrides & Location Filters
        // ==========================================

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Hint 509: Special Summon from Hand / Deck / Extra / GY
            if (hint == 509)
            {
                var extraFusions = cards.Where(c => c.Location == CardLocation.Extra && c.HasType(CardType.Fusion)).ToList();
                if (extraFusions.Count > 0)
                {
                    var plasma = extraFusions.FirstOrDefault(c => c.Id == CardId.ConductionWarriorPlasmaMagnet);
                    if (plasma != null) return new List<ClientCard> { plasma };
                }

                bool hasDeck = cards.Any(c => c.Location == CardLocation.Deck);
                if (hasDeck && cards.Any(c => c.Location == CardLocation.Hand))
                {
                    var deckTargets = cards.Where(c => c.Location == CardLocation.Deck).ToList();
                    if (deckTargets.Count >= min)
                    {
                        return deckTargets.Take(max).ToList();
                    }
                }

                var gyTargets = cards.Where(c => c.Location == CardLocation.Grave).ToList();
                if (gyTargets.Count > 0)
                {
                    var sortedGy = gyTargets.OrderBy(c => {
                        if (c == null) return 999;
                        if (c.Id == CardId.RyzealDetonator) return 5;
                        if (c.Id == CardId.RyzealDuoDrive) return 10;
                        if (c.Id == CardId.IceRyzeal) return 20;
                        if (c.Id == CardId.SwordRyzeal) return 25;
                        if (c.Id == CardId.NodeRyzeal) return 30;
                        if (c.IsCode(CardId.BetaTheElectromagnetWarrior)) return 35;
                        if (c.IsCode(CardId.MagnetWarriorSigmaMinus)) return 40;
                        if (c.IsCode(CardId.MagnetWarriorSigmaPlus)) return 45;
                        return 100;
                    }).ToList();
                    return sortedGy.Take(max).ToList();
                }
            }

            // Hint 506: Search / Add from Deck to Hand
            if (hint == 506)
            {
                if (cards.Any(c => c.IsCode(RyzealCards)))
                {
                    var sortedRyzeals = cards.OrderBy(c => {
                        if (c == null) return 999;
                        if (c.Id == CardId.NodeRyzeal && !Bot.HasInHand(CardId.NodeRyzeal)) return 1;
                        if (c.Id == CardId.RyzealCross && !Bot.HasInHand(CardId.RyzealCross) && !Bot.HasInSpellZone(CardId.RyzealCross)) return 2;
                        if (c.Id == CardId.SwordRyzeal && !Bot.HasInHand(CardId.SwordRyzeal)) return 3;
                        if (c.Id == CardId.RyzealPlugin && !Bot.HasInHand(CardId.RyzealPlugin)) return 4;
                        if (c.Id == CardId.IceRyzeal && !Bot.HasInHand(CardId.IceRyzeal)) return 5;
                        if (c.Id == CardId.ExtRyzeal) return 6;
                        if (c.Id == CardId.NodeRyzeal) return 7;
                        return 50;
                    }).ToList();
                    return sortedRyzeals.Take(max).ToList();
                }

                if (cards.Any(c => c.IsCode(MagnetCards)))
                {
                    var sortedMagnets = cards.OrderBy(c => {
                        if (c == null) return 999;
                        if (c.Id == CardId.ConductionWarriorLinearMagnumPlusMinus && !Bot.HasInHand(CardId.ConductionWarriorLinearMagnumPlusMinus)) return 1;
                        if (c.Id == CardId.TellusionTheMagnaWarrior && !Bot.HasInHand(CardId.TellusionTheMagnaWarrior) && !Bot.Graveyard.Any(g => g.IsCode(CardId.TellusionTheMagnaWarrior))) return 2;
                        if (c.Id == CardId.MagnetWarriorSigmaMinus && !Bot.HasInHand(CardId.MagnetWarriorSigmaMinus)) return 3;
                        if (c.Id == CardId.MagnetWarriorSigmaPlus && !Bot.HasInHand(CardId.MagnetWarriorSigmaPlus)) return 4;
                        if (c.Id == CardId.BetaTheElectromagnetWarrior && !Bot.HasInHand(CardId.BetaTheElectromagnetWarrior)) return 5;
                        if (c.Id == CardId.EpsilonTheMagnetWarrior) return 6;
                        return 50;
                    }).ToList();
                    return sortedMagnets.Take(max).ToList();
                }
            }

            // Hint 501 / 504: Discard / Send to Grave as cost from Hand / Field / Extra
            if (hint == 501 || hint == 504)
            {
                // Extra Deck sends (Ext Ryzeal)
                if (cards.Any(c => c.Location == CardLocation.Extra))
                {
                    var sortedExtra = cards.OrderBy(c => {
                        if (c == null) return 999;
                        if (c.Id == CardId.Number17LeviathanDragon) return 5;
                        if (c.Id == CardId.GallantGranite) return 10;
                        if (c.Id == CardId.Bagooska) return 20;
                        if (c.Id == CardId.AbyssDweller) return 30;
                        if (c.Id == CardId.EvilswarmExcitonKnight) return 40;
                        if (c.Id == CardId.RyzealDetonator) return 50;
                        if (c.Id == CardId.RyzealDuoDrive) return 60;
                        return 100;
                    }).ToList();
                    return sortedExtra.Take(max).ToList();
                }

                var sorted = cards.OrderBy(c => {
                    if (c == null) return 9999;
                    if (c.Controller == 0 && IsAceCard(c)) return 10000;
                    if (c.Controller == 0 && c.Location == CardLocation.MonsterZone && (c.IsCode(CardId.BetaTheElectromagnetWarrior) || c.Id == CardId.AdamancipatorResearcher)) return 9500;
                    if (c.IsCode(CardId.MagnetWarriorSigmaPlus) && c.Location == CardLocation.Hand) return 10;
                    if (c.IsCode(CardId.TellusionTheMagnaWarrior) && c.Location == CardLocation.Hand) return 11;
                    if (c.IsCode(CardId.MagnetWarriorSigmaMinus) && c.Location == CardLocation.Hand) return 12;
                    if (c.IsCode(CardId.NodeRyzeal) && c.Location == CardLocation.Hand) return 15;
                    if (c.IsCode(CardId.RyzealPlugin) && c.Location == CardLocation.Hand) return 20;
                    if (c.IsCode(CardId.IceRyzeal) && c.Location == CardLocation.Hand) return 25;
                    if (c.Location == CardLocation.Hand && !HandTraps.Contains(c.Id)) return 35;
                    if (c.Location == CardLocation.Hand && HandTraps.Contains(c.Id)) return 80;
                    if (c.Location == CardLocation.MonsterZone) return 500;
                    return 100;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // Hint 519: Detach Xyz Material
            if (hint == 519)
            {
                var sortedDetach = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (c.Id == CardId.SwordRyzeal) return 10;
                    if (c.Id == CardId.IceRyzeal) return 20;
                    if (c.Id == CardId.NodeRyzeal) return 30;
                    if (c.Id == CardId.ExtRyzeal) return 40;
                    return 100;
                }).ToList();
                return sortedDetach.Take(max).ToList();
            }

            // Hint 578 / 514: Attach / Overlay Material
            if (hint == 578 || hint == 514)
            {
                var sortedAttach = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (c.Id == CardId.RyzealCross) return 10;
                    if (c.Id == CardId.SwordRyzeal) return 20;
                    if (c.Id == CardId.IceRyzeal) return 30;
                    if (c.Id == CardId.NodeRyzeal) return 40;
                    if (c.Id == CardId.ExtRyzeal) return 50;
                    return 100;
                }).ToList();
                return sortedAttach.Take(max).ToList();
            }

            // Material and Cost Selections (protecting Aces)
            if (hint == 511 || hint == 512 || hint == 513 || hint == 533 || hint == 503 || hint == 502 || hint == 505)
            {
                var safeCards = cards.Where(c => c == null || (c.Controller == 0 && !IsAceCard(c) && c.Id != CardId.AdamancipatorResearcher && !HandTraps.Contains(c.Id)) || c.Controller == 1).ToList();
                if (safeCards.Count >= min)
                {
                    var sortedSafe = safeCards.OrderBy(c => {
                        if (c == null) return 999;
                        if (c.Location == CardLocation.Hand) return 10;
                        if (c.Location == CardLocation.Grave) return 20;
                        if (c.Location == CardLocation.MonsterZone) return 30;
                        return 100;
                    }).ToList();
                    return sortedSafe.Take(max).ToList();
                }
                else
                {
                    var sortedAll = cards.OrderBy(c => {
                        if (c == null) return 999;
                        if (c.Controller == 0 && IsAceCard(c)) return 10000;
                        if (c.Controller == 0 && c.Id == CardId.AdamancipatorResearcher) return 9000;
                        if (c.Controller == 0 && HandTraps.Contains(c.Id)) return 8000;
                        if (c.Location == CardLocation.Hand) return 10;
                        if (c.Location == CardLocation.Grave) return 20;
                        if (c.Location == CardLocation.MonsterZone) return 30;
                        return 100;
                    }).ToList();
                    return sortedAll.Take(max).ToList();
                }
            }

            // Koa'ki Meiru Guardian End Phase reveal
            if (cards.Any(c => c.Location == CardLocation.Hand && c.HasRace(CardRace.Rock)))
            {
                var rockHand = cards.FirstOrDefault(c => c.Location == CardLocation.Hand && c.HasRace(CardRace.Rock));
                if (rockHand != null) return new List<ClientCard> { rockHand };
            }

            // Super Polymerization fusion material selector
            if (Card != null && Card.IsCode(CardId.SuperPolymerization))
            {
                var sorted = cards.OrderByDescending(c => c.Controller == 1 ? 10 : 1).ToList();
                return sorted.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        // ==========================================
        //  Material Selectors Protection
        // ==========================================

        public override IList<ClientCard> OnSelectSynchroMaterial(IList<ClientCard> cards, int sum, int min, int max)
        {
            var sorted = cards.OrderBy(c => {
                if (c == null) return 9999;
                if (c.Controller == 0)
                {
                    if (IsAceCard(c)) return 10000;
                    if (HandTraps.Contains(c.Id)) return 8000;
                    if (c.Id == CardId.AdamancipatorResearcher) return 5;
                    if (c.IsCode(CardId.BetaTheElectromagnetWarrior)) return 10;
                    if (c.IsCode(MagnetCards)) return 20;
                    if (c.IsCode(RyzealCards)) return 30;
                }
                return 100;
            }).ToList();
            return sorted.Take(max).ToList();
        }

        public override IList<ClientCard> OnSelectXyzMaterial(IList<ClientCard> cards, int min, int max)
        {
            var sorted = cards.OrderBy(c => {
                if (c == null) return 9999;
                if (c.Controller == 0)
                {
                    if (IsAceCard(c)) return 10000;
                    if (c.Id == CardId.AdamancipatorResearcher) return 9000;
                    if (HandTraps.Contains(c.Id)) return 8000;
                    if (c.IsCode(CardId.NodeRyzeal)) return 5;
                    if (c.IsCode(RyzealCards)) return 10;
                    if (c.IsCode(MagnetCards)) return 20;
                }
                return 100;
            }).ToList();
            return sorted.Take(max).ToList();
        }

        public override IList<ClientCard> OnSelectLinkMaterial(IList<ClientCard> cards, int min, int max)
        {
            var sorted = cards.OrderBy(c => {
                if (c == null) return 9999;
                if (c.Controller == 0)
                {
                    if (IsAceCard(c) && !c.IsCode(CardId.IPMasquerena)) return 10000;
                    if (c.Id == CardId.AdamancipatorResearcher) return 9000;
                    if (HandTraps.Contains(c.Id)) return 8000;
                    if (c.IsCode(CardId.IPMasquerena)) return 5;
                    if (c.IsCode(MagnetCards)) return 10;
                    if (c.IsCode(RyzealCards)) return 20;
                }
                return 100;
            }).ToList();
            return sorted.Take(max).ToList();
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            if (card.IsCode(CardId.TellusionTheMagnaWarrior) ||
                card.IsCode(CardId.NaturiaBeast) ||
                card.IsCode(CardId.ConductionWarriorPlasmaMagnet) ||
                card.IsCode(CardId.RyzealDetonator) ||
                card.IsCode(CardId.RyzealDuoDrive) ||
                card.IsCode(CardId.SPLittleKnight) ||
                card.IsCode(CardId.IPMasquerena) ||
                card.IsCode(CardId.UnderworldGoddess) ||
                card.IsCode(CardId.AbyssDweller) ||
                card.IsCode(CardId.Bagooska) ||
                card.IsCode(CardId.EvilswarmExcitonKnight))
                return true;

            return base.IsAceCard(card);
        }

        protected override bool IsBoardStrongEnough()
        {
            int bossCount = 0;
            if (Bot.HasInMonstersZone(CardId.NaturiaBeast)) bossCount += 2;
            if (Bot.HasInMonstersZone(CardId.RyzealDetonator)) bossCount += 2;
            if (Bot.HasInMonstersZone(CardId.AbyssDweller)) bossCount += 1;
            if (Bot.HasInMonstersZone(CardId.TellusionTheMagnaWarrior)) bossCount += 1;
            if (Bot.HasInMonstersZone(CardId.KoaKiMeiruGuardian)) bossCount += 1;
            if (Bot.HasInSpellZone(CardId.RyzealCross)) bossCount += 1;

            if (bossCount >= 3) return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (Bot.HasInMonstersZone(CardId.RyzealDetonator) && Bot.HasInMonstersZone(CardId.AbyssDweller) && Bot.HasInSpellZone(CardId.RyzealCross))
                return true;
            if (Bot.HasInMonstersZone(CardId.NaturiaBeast) && Bot.HasInMonstersZone(CardId.RyzealDetonator))
                return true;

            return base.ShouldStopExtending();
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 10000;
            if (HandTraps.Contains(c.Id)) return 8000;
            if (c.IsCode(MagnetCards)) return 20;
            if (c.IsCode(RyzealCards)) return 30;
            return base.GetMaterialPriority(c);
        }
    }
}
