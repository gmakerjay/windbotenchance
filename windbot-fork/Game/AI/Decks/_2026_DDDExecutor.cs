using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using static WindBot.Game.AI.ComboRouter;

namespace WindBot.Game.AI.Decks
{
    // ============================================================
    // 2026_DDD Executor (Championship ModernExecutor Architecture)
    // ============================================================
    // D/D/D (Different Dimension Demon) is an intricate multi-method combo engine
    // capable of Fusion, Link, Pendulum, Synchro, and Xyz Summoning in a single turn.
    //
    // End Board Highlights (Turn 1):
    // - D/D/D Wave High King Caesar (2800 ATK / Multi-Special Summon Negate + 1800 ATK Boost)
    // - D/D/D Deviser King Deus Machinex (3000 ATK / Steals opponent monsters that activate effects)
    // - D/D/D Cursed King Siegfried (2800 ATK / Quick Spell/Trap Negate)
    // - D/D/D Sky King Zeus Ragnarok (Handtrap & Hand Monster Effect Negate)
    // - Dark Contract with the Eternal Darkness (Locks opponent targeting, Tributes, and Fusion/Synchro/Xyz!)
    // - Handtraps & Interruptions: Ash Blossom, Mulcharmy Fuwalos, Imperm, Dominus Impulse, Dominus Spark
    //
    // Going-Second & Board Breaking (Turn 2+):
    // - Harpie's Feather Duster, Lightning Storm, Super Polymerization (Garura/Mudragon)
    // - D/D/D/D Dimensional King Arc Crisis (4000 ATK / Negates all enemy monsters & attacks all)
    // - High Caesar + Siegfried + Machinex OTK (10,000+ total ATK)
    // ============================================================

    [Deck("2026_DDD", "2026_DDD")]
    public class _2026_DDDExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck — D/D/D & D/D Monsters
            public const int Lamia = 19580308;
            public const int NecroSlime = 72291412;
            public const int SwirlSlime = 45206713;
            public const int Kepler = 11609969;
            public const int Copernicus = 46796664;
            public const int Gryphon = 28406301;
            public const int ZeroDoomQueenMachinex = 20715411;
            public const int ChaosKingApocalypse = 83303851;
            public const int CountSurveyor = 5997110;
            public const int ScaleSurveyor = 42382265;
            public const int LanceSoldier = 67322708;
            public const int Orthros = 72181263;

            // Spells & Traps — Dark Contracts
            public const int DctGate = 46372010;
            public const int DctSwampKing = 73360025;
            public const int DctZeroKing = 32665564;
            public const int DctEternalDarkness = 9030160;

            // Consistency Spells & Staples
            public const int PiriReisMap = 33907039;
            public const int AllureOfDarkness = 1475311;
            public const int CalledByTheGrave = 24224830;

            // Hand Traps & Staples
            public const int AshBlossom = 14558127;
            public const int MulcharmyFuwalos = 42141493;
            public const int DominusSpark = 6325660;
            public const int DominusImpulse = 40366667;
            public const int InfiniteImperm = 10045474;
            public const int DrollAndLockBird = 94145021;
            public const int SuperPoly = 48130397;
            public const int LightningStorm = 14532163;
            public const int HarpiesFeatherDuster = 18144507;

            // Extra Deck
            public const int AbyssKingGilgamesh = 9024198;
            public const int SkyKingZeusRagnarok = 30998403;
            public const int DeusMachinex = 46593546;
            public const int SuperDoomKingDarkArmageddon = 18897163;
            public const int WaveHighKingCaesar = 79559912;
            public const int MarksmanKingTell = 71612253;
            public const int WiseKingSolomon = 32232538;
            public const int WaveKingCaesarXyz = 3758046;
            public const int CursedKingSiegfried = 44852429;
            public const int FirstKingClovis = 70576413;
            public const int DimensionalKingArcCrisis = 71398055;
            public const int FlameHighKingGenghis = 16006416;
            public const int FlameKingGenghis = 74583607;
            public const int AlfredDivineSage = 11852093;

            // Super Poly Fusion Targets & Side
            public const int MudragonOfTheSwamp = 54757758;
            public const int SeaMonsterOfTheseus = 96334243;
            public const int GaruraWingsOfResonantLife = 11765832;
            public const int SecreterionDragon = 89851827;
            public const int AAZeus = 90448279;
        }

        private static readonly int[] DDDMonsters = {
            CardId.Lamia, CardId.NecroSlime, CardId.SwirlSlime,
            CardId.Kepler, CardId.Copernicus, CardId.Gryphon,
            CardId.ZeroDoomQueenMachinex, CardId.ChaosKingApocalypse,
            CardId.CountSurveyor, CardId.ScaleSurveyor, CardId.LanceSoldier,
            CardId.Orthros
        };

        private static readonly int[] BossMonsters = {
            CardId.WaveHighKingCaesar,
            CardId.CursedKingSiegfried,
            CardId.DeusMachinex,
            CardId.SkyKingZeusRagnarok,
            CardId.AlfredDivineSage,
            CardId.SuperDoomKingDarkArmageddon,
            CardId.DimensionalKingArcCrisis,
            CardId.FlameHighKingGenghis
        };

        private static readonly int[] HandTraps = {
            CardId.AshBlossom,
            CardId.MulcharmyFuwalos,
            CardId.InfiniteImperm,
            CardId.DominusImpulse,
            CardId.DominusSpark,
            CardId.DrollAndLockBird
        };

        private static readonly int[] ContractCards = {
            CardId.DctGate,
            CardId.DctSwampKing,
            CardId.DctZeroKing,
            CardId.DctEternalDarkness
        };

        // Once-per-turn state trackers
        private bool _gateUsed = false;
        private bool _swampKingUsed = false;
        private bool _zeroKingUsed = false;
        private bool _copernicusUsed = false;
        private bool _necroSlimeUsed = false;
        private bool _swirlSlimeHandUsed = false;
        private bool _swirlSlimeGYUsed = false;
        private bool _lamiaReviveUsed = false;
        private bool _gryphonSearchUsed = false;
        private bool _gryphonSSUsed = false;
        private bool _gilgameshUsed = false;
        private bool _eternalDarknessUsed = false;
        private bool _zeroDoomQueenUsed = false;
        private bool _scaleSurveyorSSUsed = false;
        private bool _scaleSurveyorBounceUsed = false;
        private bool _lanceSoldierGYUsed = false;
        private bool _genghisReviveUsed = false;
        private bool _highGenghisReviveUsed = false;
        private bool _clovisReviveUsed = false;
        private bool _tellGYUsed = false;
        private bool _solomonSearchUsed = false;
        private bool _caesarGYUsed = false;
        private bool _alfredFuseUsed = false;
        private bool _zeusRagnarokExtraPendUsed = false;
        private bool _arcCrisisSSUsed = false;

        public override bool OnSelectHand()
        {
            // Prefer going first for overwhelming multi-negate disruption board
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);

            _gateUsed = false;
            _swampKingUsed = false;
            _zeroKingUsed = false;
            _copernicusUsed = false;
            _necroSlimeUsed = false;
            _swirlSlimeHandUsed = false;
            _swirlSlimeGYUsed = false;
            _lamiaReviveUsed = false;
            _gryphonSearchUsed = false;
            _gryphonSSUsed = false;
            _gilgameshUsed = false;
            _eternalDarknessUsed = false;
            _zeroDoomQueenUsed = false;
            _scaleSurveyorSSUsed = false;
            _scaleSurveyorBounceUsed = false;
            _lanceSoldierGYUsed = false;
            _genghisReviveUsed = false;
            _highGenghisReviveUsed = false;
            _clovisReviveUsed = false;
            _tellGYUsed = false;
            _solomonSearchUsed = false;
            _caesarGYUsed = false;
            _alfredFuseUsed = false;
            _zeusRagnarokExtraPendUsed = false;
            _arcCrisisSSUsed = false;
        }

        public _2026_DDDExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Register Ace cards to prevent accidental sacrifice / link off
            HeuristicGuard.RegisterAceCards(
                CardId.WaveHighKingCaesar,
                CardId.CursedKingSiegfried,
                CardId.DeusMachinex,
                CardId.SkyKingZeusRagnarok,
                CardId.AlfredDivineSage,
                CardId.SuperDoomKingDarkArmageddon,
                CardId.DimensionalKingArcCrisis,
                CardId.FlameHighKingGenghis
            );

            // Register Combos & Starters
            BaitPlanner.RegisterComboStarters(CardId.DctGate, CardId.Kepler, CardId.SwirlSlime, CardId.Copernicus);
            BaitPlanner.RegisterBaitCards(CardId.DctGate, CardId.Kepler);
            ChainAdvisor.RegisterHighValueTargets(CardId.DctGate, CardId.AbyssKingGilgamesh, CardId.InfiniteImperm);

            // Register Strategic Combo Lines in ComboRouter
            ComboRouter.RegisterLine(new ComboLine
            {
                Name = "DDD-Swirl-Fusion-Gilgamesh-Setup",
                RequiredCards = new List<int> { CardId.SwirlSlime },
                EndBoardScore = 95,
                Steps = new List<ComboStep>
                {
                    new() { CardId = CardId.SwirlSlime, ActionType = ExecutorType.Activate, Description = "Swirl Slime fuses into Genghis from hand" },
                    new() { CardId = CardId.FlameKingGenghis, ActionType = ExecutorType.SpSummon, Description = "Summon Flame King Genghis" },
                    new() { CardId = CardId.Kepler, ActionType = ExecutorType.Summon, Description = "Normal Summon Kepler" },
                    new() { CardId = CardId.Kepler, ActionType = ExecutorType.Activate, Description = "Kepler searches Gate" },
                    new() { CardId = CardId.DctGate, ActionType = ExecutorType.Activate, Description = "Gate searches Gryphon or D/D extender" },
                    new() { CardId = CardId.AbyssKingGilgamesh, ActionType = ExecutorType.SpSummon, Description = "Link Summon Gilgamesh" },
                    new() { CardId = CardId.AbyssKingGilgamesh, ActionType = ExecutorType.Activate, Description = "Gilgamesh sets Scales from Deck" },
                    new() { CardId = CardId.DeusMachinex, ActionType = ExecutorType.SpSummon, Description = "Xyz Summon Deus Machinex over Gilgamesh" }
                }
            });

            ComboRouter.RegisterLine(new ComboLine
            {
                Name = "DDD-Kepler-Gate-Swirl-Setup",
                RequiredCards = new List<int> { CardId.Kepler },
                EndBoardScore = 90,
                Steps = new List<ComboStep>
                {
                    new() { CardId = CardId.Kepler, ActionType = ExecutorType.Summon, Description = "Normal Summon Kepler" },
                    new() { CardId = CardId.Kepler, ActionType = ExecutorType.Activate, Description = "Kepler searches Gate" },
                    new() { CardId = CardId.DctGate, ActionType = ExecutorType.Activate, Description = "Gate searches Swirl Slime" },
                    new() { CardId = CardId.SwirlSlime, ActionType = ExecutorType.Activate, Description = "Swirl Slime fuses into Genghis" },
                    new() { CardId = CardId.AbyssKingGilgamesh, ActionType = ExecutorType.SpSummon, Description = "Link Summon Gilgamesh" },
                    new() { CardId = CardId.AbyssKingGilgamesh, ActionType = ExecutorType.Activate, Description = "Gilgamesh sets Scales from Deck" }
                }
            });

            ComboRouter.RegisterLine(new ComboLine
            {
                Name = "DDD-Copernicus-Dump-Necro",
                RequiredCards = new List<int> { CardId.Copernicus },
                EndBoardScore = 85,
                Steps = new List<ComboStep>
                {
                    new() { CardId = CardId.Copernicus, ActionType = ExecutorType.Summon, Description = "Normal Summon Copernicus" },
                    new() { CardId = CardId.Copernicus, ActionType = ExecutorType.Activate, Description = "Copernicus dumps Necro Slime or Lamia" },
                    new() { CardId = CardId.NecroSlime, ActionType = ExecutorType.Activate, Description = "Necro Slime fuses from GY into Genghis" },
                    new() { CardId = CardId.AbyssKingGilgamesh, ActionType = ExecutorType.SpSummon, Description = "Link Summon Gilgamesh" }
                }
            });

            ComboRouter.RegisterLine(new ComboLine
            {
                Name = "DDD-BoardBreak-HighCaesar",
                RequiredCards = new List<int> { CardId.LightningStorm },
                EndBoardScore = 80,
                Steps = new List<ComboStep>
                {
                    new() { CardId = CardId.HarpiesFeatherDuster, ActionType = ExecutorType.Activate, Description = "Wipe opponent backrow" },
                    new() { CardId = CardId.LightningStorm, ActionType = ExecutorType.Activate, Description = "Break opponent board" },
                    new() { CardId = CardId.SuperPoly, ActionType = ExecutorType.Activate, Description = "Fuse opponent monsters" },
                    new() { CardId = CardId.WaveHighKingCaesar, ActionType = ExecutorType.SpSummon, Description = "Xyz High Caesar for ATK push and negates" }
                }
            });

            // ==========================================
            // TIER 0: Going-Second Board Breakers & Hand Protection
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, HarpiesFeatherDusterEffect);
            AddExecutor(ExecutorType.Activate, CardId.LightningStorm, LightningStormEffect);
            AddExecutor(ExecutorType.Activate, CardId.SuperPoly, SuperPolyEffect);

            // ==========================================
            // TIER 1: Draw & Search Consistency
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.PiriReisMap, PiriReisMapEffect);
            AddExecutor(ExecutorType.Activate, CardId.AllureOfDarkness, AllureOfDarknessEffect);

            // ==========================================
            // TIER 1.5: Hand Traps & Fast Interruptions (Unconditional opponent responses)
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImperm, InfiniteImpermEffect);
            AddExecutor(ExecutorType.Activate, CardId.DominusImpulse, DominusImpulseEffect);
            AddExecutor(ExecutorType.Activate, CardId.DominusSpark, DominusSparkEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdEffect);

            // ==========================================
            // TIER 2: Boss Quick Effects & Countermeasures
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.WaveHighKingCaesar, HighCaesarEffect);
            AddExecutor(ExecutorType.Activate, CardId.CursedKingSiegfried, SiegfriedEffect);
            AddExecutor(ExecutorType.Activate, CardId.DeusMachinex, DeusMachinexEffect);
            AddExecutor(ExecutorType.Activate, CardId.SkyKingZeusRagnarok, SkyKingZeusRagnarokQuickEffect);
            AddExecutor(ExecutorType.Activate, CardId.FlameHighKingGenghis, FlameHighGenghisQuickEffect);

            // ==========================================
            // TIER 3: Continuous Spells & Core Starters (Gate & Swamp King!)
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.DctGate, DctGateEffect);
            AddExecutor(ExecutorType.Activate, CardId.DctSwampKing, DctSwampKingEffect);
            AddExecutor(ExecutorType.Activate, CardId.ZeroDoomQueenMachinex, ZeroDoomQueenPZoneEffect);
            AddExecutor(ExecutorType.Activate, CardId.DctZeroKing, DctZeroKingEffect);

            // ==========================================
            // TIER 4: Pendulum Scale Setting from Hand
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.ScaleSurveyor, ScaleActivate);
            AddExecutor(ExecutorType.Activate, CardId.CountSurveyor, ScaleActivate);
            AddExecutor(ExecutorType.Activate, CardId.Kepler, ScaleActivate);
            AddExecutor(ExecutorType.Activate, CardId.Copernicus, ScaleActivate);
            AddExecutor(ExecutorType.Activate, CardId.Gryphon, ScaleActivate);

            // ==========================================
            // TIER 5: Fusion Starters (Hand & GY)
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.SwirlSlime, SwirlSlimeEffect);
            AddExecutor(ExecutorType.Activate, CardId.NecroSlime, NecroSlimeEffect);
            AddExecutor(ExecutorType.Activate, CardId.AlfredDivineSage, AlfredDivineSageEffect);

            // ==========================================
            // TIER 6: Normal Summons (Starters & Extenders)
            // ==========================================
            AddExecutor(ExecutorType.Summon, CardId.Kepler, KeplerSummon);
            AddExecutor(ExecutorType.Summon, CardId.Copernicus, CopernicusSummon);
            AddExecutor(ExecutorType.Summon, CardId.Gryphon, GryphonSummon);
            AddExecutor(ExecutorType.Summon, CardId.ScaleSurveyor, ScaleSurveyorSummon);
            AddExecutor(ExecutorType.Summon, CardId.LanceSoldier, LanceSoldierSummon);
            AddExecutor(ExecutorType.Summon, CardId.Lamia, LamiaSummon);

            // ==========================================
            // TIER 6: Monster Effects (Trigger on NS/SS & Revive)
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.Kepler, KeplerEffect);
            AddExecutor(ExecutorType.Activate, CardId.Copernicus, CopernicusEffect);
            AddExecutor(ExecutorType.Activate, CardId.Gryphon, GryphonEffect);
            AddExecutor(ExecutorType.Activate, CardId.ScaleSurveyor, ScaleSurveyorEffect);
            AddExecutor(ExecutorType.Activate, CardId.CountSurveyor, CountSurveyorEffect);
            AddExecutor(ExecutorType.Activate, CardId.LanceSoldier, LanceSoldierEffect);
            AddExecutor(ExecutorType.Activate, CardId.Lamia, LamiaReviveEffect);
            AddExecutor(ExecutorType.Activate, CardId.ChaosKingApocalypse, ChaosKingApocalypseEffect);

            // Extra Deck Trigger Revivals & Searches
            AddExecutor(ExecutorType.Activate, CardId.FlameKingGenghis, FlameKingGenghisReviveEffect);
            AddExecutor(ExecutorType.Activate, CardId.FlameHighKingGenghis, FlameHighKingGenghisReviveEffect);
            AddExecutor(ExecutorType.Activate, CardId.FirstKingClovis, ClovisReviveEffect);
            AddExecutor(ExecutorType.Activate, CardId.MarksmanKingTell, TellGYEffect);
            AddExecutor(ExecutorType.Activate, CardId.WaveKingCaesarXyz, CaesarGYEffect);
            AddExecutor(ExecutorType.Activate, CardId.WiseKingSolomon, SolomonSearchEffect);
            AddExecutor(ExecutorType.Activate, CardId.SuperDoomKingDarkArmageddon, DarkArmageddonPopEffect);

            // Link Effects
            AddExecutor(ExecutorType.Activate, CardId.AbyssKingGilgamesh, GilgameshEffect);
            AddExecutor(ExecutorType.Activate, CardId.SkyKingZeusRagnarok, SkyKingZeusRagnarokExtraPendEffect);

            // ==========================================
            // TIER 7: Extra Deck Summons — Structured Sequence
            // ==========================================
            // 1. Fusion Summons
            AddExecutor(ExecutorType.SpSummon, CardId.FlameKingGenghis, FlameKingGenghisSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.FlameHighKingGenghis, FlameHighKingGenghisSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AlfredDivineSage, AlfredDivineSageSummon);

            // 2. Link Summons (Gilgamesh sets scales from Deck!)
            AddExecutor(ExecutorType.SpSummon, CardId.AbyssKingGilgamesh, GilgameshSummon);

            // 3. PENDULUM SUMMON (Execute multi-monster summon from Hand & Extra Deck!)
            AddExecutor(ExecutorType.SpSummon, PendulumSummonCheck);

            // 4. Xyz Machinex OVERLAY (Immediately over Gilgamesh or Tell/Caesar!)
            AddExecutor(ExecutorType.SpSummon, CardId.DeusMachinex, DeusMachinexSummon);

            // 5. Synchro Summons (Clovis & Siegfried)
            AddExecutor(ExecutorType.SpSummon, CardId.FirstKingClovis, ClovisSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CursedKingSiegfried, SiegfriedSummon);

            // 6. Xyz Summons (Rank 4 -> Tell Rank 5 -> High Caesar Rank 6)
            AddExecutor(ExecutorType.SpSummon, CardId.WaveKingCaesarXyz, WaveKingCaesarSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.WiseKingSolomon, WiseKingSolomonSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.MarksmanKingTell, MarksmanKingTellSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.WaveHighKingCaesar, HighCaesarSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SuperDoomKingDarkArmageddon, DarkArmageddonSummon);

            // 7. Auxiliary Links (Only if Machinex already on board)
            AddExecutor(ExecutorType.SpSummon, CardId.SkyKingZeusRagnarok, SkyKingZeusRagnarokSummon);

            // 8. Ultimate Finisher (Arc Crisis)
            AddExecutor(ExecutorType.SpSummon, CardId.DimensionalKingArcCrisis, ArcCrisisSummon);

            // ==========================================
            // TIER 8: Spells/Traps Setup & Sets
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.DctEternalDarkness, DctEternalDarknessEffect);
            AddExecutor(ExecutorType.SpellSet, CardId.DctEternalDarkness, EternalDarknessSetEffect);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImperm);
            AddExecutor(ExecutorType.SpellSet, CardId.SuperPoly);

            // Reposition monsters
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        // ==========================================
        // Helper Utilities
        // ==========================================

        private bool HasAvailablePZone()
        {
            return Util.GetPZone(0, 0) == null || Util.GetPZone(0, 1) == null;
        }

        private bool HasBothPZonesFilled()
        {
            return Util.GetPZone(0, 0) != null && Util.GetPZone(0, 1) != null;
        }

        private int GetScale(int cardId)
        {
            switch (cardId)
            {
                case CardId.ZeroDoomQueenMachinex: return 0;
                case CardId.CountSurveyor: return 1;
                case CardId.Copernicus: return 1;
                case CardId.Gryphon: return 1;
                case CardId.ChaosKingApocalypse: return 4;
                case CardId.ScaleSurveyor: return 9;
                case CardId.Kepler: return 10;
                default: return -1;
            }
        }

        private bool ScaleActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (!HasAvailablePZone()) return false;

            int myScale = GetScale(Card.Id);
            if (myScale == -1) return false;

            ClientCard leftScale = Util.GetPZone(0, 0);
            ClientCard rightScale = Util.GetPZone(0, 1);

            if (leftScale == null && rightScale == null)
            {
                bool hasLowInHand = Bot.Hand.Any(c => c != Card && GetScale(c.Id) <= 1 && GetScale(c.Id) >= 0);
                bool hasHighInHand = Bot.Hand.Any(c => c != Card && GetScale(c.Id) >= 9);

                if (myScale <= 1 && hasHighInHand) return true;
                if (myScale >= 9 && hasLowInHand) return true;

                if (Card.IsCode(CardId.ScaleSurveyor) || Card.IsCode(CardId.ZeroDoomQueenMachinex)) return true;

                return false;
            }
            else
            {
                ClientCard existing = leftScale ?? rightScale;
                int existingScale = GetScale(existing.Id);

                if (existingScale <= 1 && myScale >= 9) return true;
                if (existingScale >= 9 && myScale <= 1) return true;

                return false;
            }
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.IsCode(BossMonsters);
        }

        // ==========================================
        // TIER 0: Board Breakers
        // ==========================================

        private bool HarpiesFeatherDusterEffect()
        {
            if (Enemy.GetSpellCount() > 0)
                return true;
            return false;
        }

        private bool LightningStormEffect()
        {
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup()) || Bot.GetSpells().Any(c => c != null && c.IsFaceup()))
                return false;

            if (Enemy.GetSpellCount() > 0)
            {
                AI.SelectOption(1); // destroy Spells/Traps
                return true;
            }
            if (Enemy.GetMonsterCount() > 0)
            {
                AI.SelectOption(0); // destroy Attack Position monsters
                return true;
            }
            return false;
        }

        private bool SuperPolyEffect()
        {
            if (Util.GetLastChainCard() != null && Util.GetLastChainCard().Controller == 0)
                return false;

            var oppFaceup = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            if (oppFaceup.Count >= 2)
                return true;
            if (oppFaceup.Count >= 1 && Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !IsAceCard(c)))
                return true;
            return false;
        }

        // ==========================================
        // TIER 1: Hand Traps
        // ==========================================

        private bool MulcharmyEffect()
        {
            if (Util.GetLastChainCard() != null && Util.GetLastChainCard().Controller == 0) return false;
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }

        private bool AshBlossomEffect()
        {
            ClientCard lastChain = Util.GetLastChainCard();
            if (lastChain != null && lastChain.Controller == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool InfiniteImpermEffect()
        {
            ClientCard lastChain = Util.GetLastChainCard();
            if (lastChain != null && lastChain.Controller == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;

            if (Card.Location == CardLocation.Hand)
                return Bot.GetFieldCount() == 0;

            if (Card.Location == CardLocation.SpellZone && Card.IsFacedown())
            {
                var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool DominusImpulseEffect()
        {
            if (Card.Location == CardLocation.Hand && Bot.GetSpellCount() > 0) return false;
            if (Util.GetLastChainCard() != null && Util.GetLastChainCard().Controller == 0)
                return false;
            if (Duel.LastChainPlayer != 1) return false;
            return true;
        }

        private bool DominusSparkEffect()
        {
            if (Util.GetLastChainCard() != null && Util.GetLastChainCard().Controller == 0)
                return false;
            if (Duel.LastChainPlayer != 1) return false;
            return true;
        }

        private bool DrollAndLockBirdEffect()
        {
            return Duel.Player == 1 && Duel.LastChainPlayer == 1;
        }

        // ==========================================
        // TIER 2: Boss Quick Effects
        // ==========================================

        private bool HighCaesarEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;

            ClientCard lastChain = Util.GetLastChainCard();
            if (lastChain != null && lastChain.Controller == 0) return false;
            if (Duel.LastChainPlayer == 1)
            {
                // Caesar negates any activation that includes Special Summoning a monster
                return true;
            }
            return false;
        }

        private bool SiegfriedEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;

            ClientCard lastChain = Util.GetLastChainCard();
            if (lastChain != null && lastChain.Controller == 0) return false;

            // Target face-up opponent Spell/Trap
            var enemySpell = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled());
            if (enemySpell != null)
            {
                AI.SelectCard(enemySpell);
                return true;
            }

            if (Duel.LastChainPlayer == 1 && lastChain != null && (lastChain.IsSpell() || lastChain.IsTrap()))
            {
                return true;
            }
            return false;
        }

        private bool DeusMachinexEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (Duel.LastChainPlayer != 1) return false;
            if (Util.GetLastChainCard() != null && Util.GetLastChainCard().Controller == 0)
                return false;

            // Deus Machinex attaches activating opponent monster
            if (Card.Overlays.Count >= 2) return true;

            // Can destroy a Dark Contract we control as alternative cost
            var contractOnField = Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(ContractCards));
            if (contractOnField != null && Bot.LifePoints > 1000)
            {
                AI.SelectCard(contractOnField);
                return true;
            }

            return Card.Overlays.Count >= 1;
        }

        private bool SkyKingZeusRagnarokQuickEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (Duel.LastChainPlayer != 1) return false;

            ClientCard lastChain = Util.GetLastChainCard();
            if (lastChain != null && lastChain.Controller == 0) return false;

            // Negate opponent hand effect by banishing 1 D/D and 1 Contract from GY
            bool hasDDInGY = Bot.Graveyard.Any(c => c != null && c.HasSetcode(0xaf));
            bool hasContractInGY = Bot.Graveyard.Any(c => c != null && c.IsCode(ContractCards));
            return hasDDInGY && hasContractInGY;
        }

        private bool FlameHighGenghisQuickEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (Duel.Player != 0) return false; // Only during our turn

            ClientCard lastChain = Util.GetLastChainCard();
            if (lastChain != null && lastChain.Controller == 0) return false;
            if (Duel.LastChainPlayer == 1 && lastChain != null && (lastChain.IsSpell() || lastChain.IsTrap()))
            {
                return true;
            }
            return false;
        }

        // ==========================================
        // TIER 1: Consistency Spells
        // ==========================================

        private bool PiriReisMapEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (Duel.Phase != DuelPhase.Main1) return false;
            if (Bot.LifePoints <= 2000) return false;

            // Search Kepler (0 ATK) to start the combo
            AI.SelectCard(CardId.Kepler);
            return true;
        }

        private bool AllureOfDarknessEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // Only activate if we have at least 1 other DARK monster to banish safely
            bool hasDark = Bot.Hand.Any(c => c != null && c != Card && c.HasAttribute(CardAttribute.Dark));
            if (!hasDark) return false;

            AI.SelectCard(new[] {
                CardId.LanceSoldier,
                CardId.CountSurveyor,
                CardId.ScaleSurveyor,
                CardId.ChaosKingApocalypse,
                CardId.ZeroDoomQueenMachinex,
                CardId.NecroSlime,
                CardId.SwirlSlime,
                CardId.Gryphon,
                CardId.Copernicus
            });
            return true;
        }

        // ==========================================
        // TIER 3: Continuous Spells / Searchers
        // ==========================================

        private bool DctGateEffect()
        {
            if (Bot.LifePoints <= 1000 && Bot.GetSpells().Count(c => c != null && c.IsFaceup() && c.IsCode(ContractCards)) > 0)
                return false;

            if (Card.Location == CardLocation.Hand)
            {
                bool gateAlreadyOnField = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.DctGate));
                if (gateAlreadyOnField) return false;
                return true;
            }

            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                if (_gateUsed) return false;

                // Priority search target selection
                if (!Bot.HasInHand(CardId.SwirlSlime) && !Bot.HasInGraveyard(CardId.SwirlSlime))
                {
                    AI.SelectCard(CardId.SwirlSlime);
                }
                else if (!Bot.HasInHand(CardId.Gryphon) && Bot.GetMonsters().Any(c => c != null && c.HasSetcode(0xaf)))
                {
                    AI.SelectCard(CardId.Gryphon);
                }
                else if (!Bot.HasInHand(CardId.Copernicus) && !Bot.HasInGraveyard(CardId.Lamia))
                {
                    AI.SelectCard(CardId.Copernicus);
                }
                else if (!Bot.HasInHand(CardId.Kepler))
                {
                    AI.SelectCard(CardId.Kepler);
                }
                else
                {
                    AI.SelectCard(CardId.Lamia, CardId.NecroSlime, CardId.ScaleSurveyor);
                }

                _gateUsed = true;
                return true;
            }
            return false;
        }

        private bool DctSwampKingEffect()
        {
            if (Bot.LifePoints <= 1000) return false;
            if (IsSpecialSummonBlocked()) return false;

            if (Card.Location == CardLocation.Hand)
            {
                bool swampAlreadyOnField = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.DctSwampKing));
                if (swampAlreadyOnField) return false;
                return true;
            }

            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                if (_swampKingUsed) return false;
                if (GetRemainingCount(CardId.FlameHighKingGenghis) > 0)
                {
                    AI.SelectCard(CardId.FlameHighKingGenghis);
                    _swampKingUsed = true;
                    return true;
                }
                if (GetRemainingCount(CardId.FlameKingGenghis) > 0)
                {
                    AI.SelectCard(CardId.FlameKingGenghis);
                    _swampKingUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool ZeroDoomQueenPZoneEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Place in P-Zone if we need a scale and have P-Zone slot
                if (HasAvailablePZone())
                {
                    return true;
                }
                return false;
            }

            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                if (_zeroDoomQueenUsed) return false;
                // Pendulum effect: place Continuous Dark Contract from Deck face-up
                if (!Bot.HasInSpellZone(CardId.DctGate))
                {
                    AI.SelectCard(CardId.DctGate);
                }
                else if (!Bot.HasInSpellZone(CardId.DctEternalDarkness))
                {
                    AI.SelectCard(CardId.DctEternalDarkness);
                }
                else
                {
                    AI.SelectCard(CardId.DctZeroKing);
                }
                _zeroDoomQueenUsed = true;
                return true;
            }
            return false;
        }

        private bool DctZeroKingEffect()
        {
            if (Bot.LifePoints <= 1000) return false;
            if (_zeroKingUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            if (Card.Location == CardLocation.Hand)
            {
                bool zeroKingAlreadyOnField = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.DctZeroKing));
                if (zeroKingAlreadyOnField) return false;
                return true;
            }

            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                // Target 1 D/D card to destroy and SS 1 D/D from Deck
                var target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !IsAceCard(c))
                    ?? Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && c != Card && c.IsCode(CardId.DctGate));

                if (target != null)
                {
                    AI.SelectCard(target);
                    AI.SelectNextCard(CardId.Copernicus, CardId.Gryphon, CardId.Kepler, CardId.Lamia);
                    _zeroKingUsed = true;
                    return true;
                }
            }
            return false;
        }

        // ==========================================
        // TIER 4: Fusion Starters
        // ==========================================

        private bool SwirlSlimeEffect()
        {
            if (IsSpecialSummonBlocked()) return false;

            if (Card.Location == CardLocation.Hand && !_swirlSlimeHandUsed)
            {
                bool hasFusionTarget = GetRemainingCount(CardId.FlameKingGenghis) > 0
                    || GetRemainingCount(CardId.FlameHighKingGenghis) > 0
                    || GetRemainingCount(CardId.AlfredDivineSage) > 0;
                if (!hasFusionTarget) return false;

                var otherDDD = Bot.Hand.FirstOrDefault(c => c != null && c != Card && c.HasSetcode(0xaf));
                if (otherDDD != null)
                {
                    AI.SelectCard(new[] { CardId.FlameKingGenghis, CardId.FlameHighKingGenghis, CardId.AlfredDivineSage });
                    AI.SelectNextCard(otherDDD);
                    _swirlSlimeHandUsed = true;
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave && !_swirlSlimeGYUsed)
            {
                // Banish Swirl Slime from GY to Special Summon 1 D/D from hand
                var handTarget = Bot.Hand.FirstOrDefault(c => c != null && c.HasSetcode(0xaf));
                if (handTarget != null)
                {
                    AI.SelectCard(handTarget);
                    _swirlSlimeGYUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool NecroSlimeEffect()
        {
            if (Card.Location != CardLocation.Grave || _necroSlimeUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            bool hasFusionTarget = GetRemainingCount(CardId.FlameHighKingGenghis) > 0
                || GetRemainingCount(CardId.FlameKingGenghis) > 0
                || GetRemainingCount(CardId.AlfredDivineSage) > 0;
            if (!hasFusionTarget) return false;

            // Must have another D/D in GY to banish as material
            var otherMat = Bot.Graveyard.FirstOrDefault(c => c != null && c != Card && c.IsMonster() && c.HasSetcode(0xaf) && !IsAceCard(c));
            if (otherMat != null)
            {
                AI.SelectCard(otherMat);
                _necroSlimeUsed = true;
                return true;
            }
            return false;
        }

        private bool AlfredDivineSageEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled() || _alfredFuseUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            // Fuse by shuffling materials from hand/field/banished into deck
            bool hasTarget = GetRemainingCount(CardId.FlameHighKingGenghis) > 0 || GetRemainingCount(CardId.DimensionalKingArcCrisis) > 0;
            if (hasTarget)
            {
                _alfredFuseUsed = true;
                return true;
            }
            return false;
        }

        // ==========================================
        // TIER 5: Normal Summons
        // ==========================================

        private bool KeplerSummon()
        {
            return true;
        }

        private bool CopernicusSummon()
        {
            return true;
        }

        private bool GryphonSummon()
        {
            return true;
        }

        private bool ScaleSurveyorSummon()
        {
            return true;
        }

        private bool LanceSoldierSummon()
        {
            return true;
        }

        private bool LamiaSummon()
        {
            // Summon Lamia if we need a tuner for Synchro
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.HasType(CardType.Tuner));
        }

        // ==========================================
        // TIER 6: Monster Effects (Field & Triggers)
        // ==========================================

        private bool KeplerEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;

            if (!Bot.HasInHand(CardId.DctGate) && !Bot.HasInSpellZone(CardId.DctGate))
            {
                AI.SelectCard(CardId.DctGate);
            }
            else if (!Bot.HasInSpellZone(CardId.DctEternalDarkness))
            {
                AI.SelectCard(CardId.DctEternalDarkness);
            }
            else
            {
                AI.SelectCard(CardId.DctZeroKing);
            }
            return true;
        }

        private bool CopernicusEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (_copernicusUsed) return false;

            if (!Bot.HasInGraveyard(CardId.Lamia))
            {
                AI.SelectCard(CardId.Lamia);
            }
            else if (!Bot.HasInGraveyard(CardId.NecroSlime))
            {
                AI.SelectCard(CardId.NecroSlime);
            }
            else if (!Bot.HasInGraveyard(CardId.Gryphon))
            {
                AI.SelectCard(CardId.Gryphon);
            }
            else if (!Bot.HasInGraveyard(CardId.LanceSoldier))
            {
                AI.SelectCard(CardId.LanceSoldier);
            }
            else
            {
                AI.SelectCard(CardId.SwirlSlime);
            }

            _copernicusUsed = true;
            return true;
        }

        private bool GryphonEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_gryphonSSUsed) return false;
                if (IsSpecialSummonBlocked()) return false;
                // Special Summons itself from hand in DEF if we control D/D
                if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasSetcode(0xaf)))
                {
                    _gryphonSSUsed = true;
                    return true;
                }
            }

            if (Card.Location == CardLocation.MonsterZone && !_gryphonSearchUsed && !Card.IsDisabled())
            {
                // Revived from GY -> Search D/D card!
                AI.SelectCard(CardId.SwirlSlime, CardId.DctGate, CardId.Kepler, CardId.Copernicus, CardId.ScaleSurveyor);
                _gryphonSearchUsed = true;
                return true;
            }
            return false;
        }

        private bool ScaleSurveyorEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_scaleSurveyorSSUsed) return false;
                if (IsSpecialSummonBlocked()) return false;
                // SS if control D/D Pendulum card
                if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Pendulum) && c.HasSetcode(0xaf))
                    || Util.GetPZone(0, 0) != null || Util.GetPZone(0, 1) != null)
                {
                    _scaleSurveyorSSUsed = true;
                    return true;
                }
            }

            if (Card.Location == CardLocation.MonsterZone)
            {
                // Can become Level 4 on summon
                return true;
            }

            if (Card.Location == CardLocation.Grave || Card.Location == CardLocation.Extra)
            {
                if (_scaleSurveyorBounceUsed) return false;
                // Bounce a D/D Pendulum card back to hand
                var bounceTarget = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.HasType(CardType.Pendulum) && c.HasSetcode(0xaf) && !IsAceCard(c));
                if (bounceTarget != null)
                {
                    AI.SelectCard(bounceTarget);
                    _scaleSurveyorBounceUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool CountSurveyorEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // If control another D/D -> bounce 1 card in opponent S/T zone
                var enemyST = Enemy.GetSpells().FirstOrDefault(c => c != null);
                if (enemyST != null)
                {
                    AI.SelectCard(enemyST);
                    return true;
                }
            }
            return false;
        }

        private bool LanceSoldierEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                if (_lanceSoldierGYUsed) return false;
                if (IsSpecialSummonBlocked()) return false;
                // Destroy 1 Dark Contract to SS itself
                var contract = Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.DctGate, CardId.DctZeroKing));
                if (contract != null)
                {
                    AI.SelectCard(contract);
                    _lanceSoldierGYUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool LamiaReviveEffect()
        {
            if (Card.Location != CardLocation.Grave && Card.Location != CardLocation.Hand) return false;
            if (_lamiaReviveUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            // Cost: Send 1 D/D or Dark Contract to GY (do not send Ace cards or active Eternal Darkness)
            var cost = Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.DctGate, CardId.DctZeroKing));
            if (cost == null)
                cost = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.Kepler) && !IsAceCard(c));
            if (cost == null)
                cost = Bot.Hand.FirstOrDefault(c => c != null && c != Card && c.HasSetcode(0xaf));

            if (cost != null)
            {
                AI.SelectCard(cost);
                _lamiaReviveUsed = true;
                return true;
            }
            return false;
        }

        private bool ChaosKingApocalypseEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Duel.Player == 1) // During opponent turn
            {
                var spellsToPop = Bot.GetSpells().Where(c => c != null && c.IsFaceup() && c.IsCode(CardId.DctGate, CardId.DctZeroKing)).Take(2).ToList();
                if (spellsToPop.Count == 2)
                {
                    AI.SelectCard(spellsToPop);
                    return true;
                }
            }
            return false;
        }

        // Extra Deck Trigger Revivals
        private bool FlameKingGenghisReviveEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled() || _genghisReviveUsed) return false;

            // Target in GY: Gryphon (searches!), Copernicus (foolish!), Lamia (tuner), Siegfried, Caesar
            var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCode(CardId.Gryphon))
                ?? Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCode(CardId.Copernicus))
                ?? Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCode(CardId.Lamia))
                ?? Bot.Graveyard.FirstOrDefault(c => c != null && c.HasSetcode(0xaf) && !IsAceCard(c));

            if (target != null)
            {
                AI.SelectCard(target);
                _genghisReviveUsed = true;
                return true;
            }
            return false;
        }

        private bool FlameHighKingGenghisReviveEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled() || _highGenghisReviveUsed) return false;

            var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCode(CardId.Gryphon))
                ?? Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCode(CardId.Copernicus))
                ?? Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCode(CardId.Lamia))
                ?? Bot.Graveyard.FirstOrDefault(c => c != null && c.HasSetcode(0xaf) && !IsAceCard(c));

            if (target != null)
            {
                AI.SelectCard(target);
                _highGenghisReviveUsed = true;
                return true;
            }
            return false;
        }

        private bool ClovisReviveEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled() || _clovisReviveUsed) return false;

            // Special Summon 1 banished D/D monster or from GY
            var banishedTarget = Bot.Banished.FirstOrDefault(c => c != null && c.IsMonster() && c.HasSetcode(0xaf));
            if (banishedTarget != null)
            {
                AI.SelectCard(banishedTarget);
                _clovisReviveUsed = true;
                return true;
            }

            var gyTarget = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && c.HasSetcode(0xaf) && !IsAceCard(c));
            if (gyTarget != null)
            {
                AI.SelectCard(gyTarget);
                _clovisReviveUsed = true;
                return true;
            }
            return false;
        }

        private bool TellGYEffect()
        {
            if (_tellGYUsed) return false;
            // Send 1 D/D or Dark Contract from Deck to GY (Foolish Burial)
            if (!Bot.HasInGraveyard(CardId.Lamia))
            {
                AI.SelectCard(CardId.Lamia);
            }
            else if (!Bot.HasInGraveyard(CardId.NecroSlime))
            {
                AI.SelectCard(CardId.NecroSlime);
            }
            else if (!Bot.HasInGraveyard(CardId.LanceSoldier))
            {
                AI.SelectCard(CardId.LanceSoldier);
            }
            else
            {
                AI.SelectCard(CardId.Gryphon, CardId.SwirlSlime);
            }

            _tellGYUsed = true;
            return true;
        }

        private bool CaesarGYEffect()
        {
            if (_caesarGYUsed) return false;
            // Add 1 Dark Contract from Deck to Hand
            if (!Bot.HasInHand(CardId.DctGate) && !Bot.HasInSpellZone(CardId.DctGate))
            {
                AI.SelectCard(CardId.DctGate);
            }
            else if (!Bot.HasInSpellZone(CardId.DctEternalDarkness))
            {
                AI.SelectCard(CardId.DctEternalDarkness);
            }
            else
            {
                AI.SelectCard(CardId.DctZeroKing);
            }

            _caesarGYUsed = true;
            return true;
        }

        private bool SolomonSearchEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled() || _solomonSearchUsed) return false;
            AI.SelectCard(CardId.SwirlSlime, CardId.Gryphon, CardId.DctGate, CardId.Copernicus);
            _solomonSearchUsed = true;
            return true;
        }

        private bool DarkArmageddonPopEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            var oppMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            if (oppMonsters.Count > 0)
            {
                AI.SelectCard(oppMonsters);
                return true;
            }
            return false;
        }

        private bool GilgameshEffect()
        {
            if (_gilgameshUsed) return false;
            if (Bot.LifePoints <= 1000) return false;
            if (!HasAvailablePZone()) return false;

            // Gilgamesh places 2 D/D Pendulum monsters with different names in Pendulum Zones!
            AI.SelectCard(new[] {
                CardId.ZeroDoomQueenMachinex,
                CardId.CountSurveyor,
                CardId.Copernicus,
                CardId.Gryphon
            });
            AI.SelectNextCard(new[] {
                CardId.ScaleSurveyor,
                CardId.Kepler
            });

            _gilgameshUsed = true;
            return true;
        }

        private bool SkyKingZeusRagnarokExtraPendEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled() || _zeusRagnarokExtraPendUsed) return false;
            // Pop an unused card to gain extra Pendulum Summon
            var popTarget = Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.DctGate, CardId.DctZeroKing));
            if (popTarget != null)
            {
                AI.SelectCard(popTarget);
                _zeusRagnarokExtraPendUsed = true;
                return true;
            }
            return false;
        }

        // ==========================================
        // TIER 7: Extra Deck & Pendulum Summons
        // ==========================================

        private bool PendulumSummonCheck()
        {
            if (IsSpecialSummonBlocked()) return false;
            // Allow Pendulum Summon if we have both scales placed
            return HasBothPZonesFilled();
        }

        private bool FlameKingGenghisSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool FlameHighKingGenghisSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool AlfredDivineSageSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool GilgameshSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (_gilgameshUsed) return false;

            // Need 2 D/D monsters on field, none of which should be High Caesar / Siegfried / Deus Machinex
            var mats = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasSetcode(0xaf) && !IsAceCard(c)).ToList();
            return mats.Count >= 2;
        }

        private bool SkyKingZeusRagnarokSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            // Never summon Zeus Ragnarok before Deus Machinex is already established!
            if (!Bot.HasInMonstersZone(CardId.DeusMachinex)) return false;
            var mats = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasSetcode(0xaf) && !IsAceCard(c)).ToList();
            return mats.Count >= 3;
        }

        private bool ClovisSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var nonAce = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c)).ToList();
            bool hasTuner = nonAce.Any(c => c.HasType(CardType.Tuner));
            bool hasNonTuner = nonAce.Any(c => !c.HasType(CardType.Tuner));
            return hasTuner && hasNonTuner;
        }

        private bool SiegfriedSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var nonAce = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c)).ToList();
            bool hasTuner = nonAce.Any(c => c.HasType(CardType.Tuner));
            bool hasNonTuner = nonAce.Any(c => !c.HasType(CardType.Tuner));
            return hasTuner && hasNonTuner;
        }

        private bool WaveKingCaesarSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (GetRemainingCount(CardId.WaveKingCaesarXyz) == 0) return false;
            int lv4Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 4 && c.HasSetcode(0xaf) && !IsAceCard(c));
            return lv4Count >= 2;
        }

        private bool WiseKingSolomonSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (GetRemainingCount(CardId.WiseKingSolomon) == 0) return false;
            int lv4Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 4 && c.HasSetcode(0xaf) && !IsAceCard(c));
            return lv4Count >= 2;
        }

        private bool MarksmanKingTellSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (GetRemainingCount(CardId.MarksmanKingTell) == 0) return false;
            var caesar = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.WaveKingCaesarXyz));
            if (caesar != null)
            {
                AI.SelectCard(caesar);
                return true;
            }
            return false;
        }

        private bool HighCaesarSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (GetRemainingCount(CardId.WaveHighKingCaesar) == 0) return false;
            int lv6Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 6 && !IsAceCard(c));
            return lv6Count >= 2;
        }

        private bool DarkArmageddonSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (GetRemainingCount(CardId.SuperDoomKingDarkArmageddon) == 0) return false;
            int lv8Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 8 && c.HasSetcode(0xaf) && !IsAceCard(c));
            return lv8Count >= 2;
        }

        private bool DeusMachinexSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (GetRemainingCount(CardId.DeusMachinex) == 0) return false;
            if (Bot.HasInMonstersZone(CardId.DeusMachinex)) return false;

            // Overlay directly on top of Tell, Caesar, Solomon, or Gilgamesh
            var target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && 
                (c.IsCode(CardId.MarksmanKingTell, CardId.WaveKingCaesarXyz, CardId.WiseKingSolomon) ||
                 (c.IsCode(CardId.AbyssKingGilgamesh) && _gilgameshUsed)));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }

            // Fallback: any non-ace D/D/D monster on field
            target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.HasSetcode(0x10af) && !IsAceCard(c));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }

            return false;
        }

        private bool ArcCrisisSummon()
        {
            if (IsSpecialSummonBlocked() || _arcCrisisSSUsed) return false;

            // Arc Crisis requires 1 Fusion, 1 Synchro, 1 Xyz, 1 Pendulum from field/GY
            var allCards = Bot.GetMonsters().Concat(Bot.Graveyard.Where(c => c != null && c.IsMonster())).ToList();
            bool hasFusion = allCards.Any(c => c.HasType(CardType.Fusion) && c.HasRace(CardRace.Fiend));
            bool hasSynchro = allCards.Any(c => c.HasType(CardType.Synchro) && c.HasRace(CardRace.Fiend));
            bool hasXyz = allCards.Any(c => c.HasType(CardType.Xyz) && c.HasRace(CardRace.Fiend));
            bool hasPend = allCards.Any(c => c.HasType(CardType.Pendulum) && c.HasRace(CardRace.Fiend));

            if (hasFusion && hasSynchro && hasXyz && hasPend)
            {
                _arcCrisisSSUsed = true;
                return true;
            }
            return false;
        }

        // ==========================================
        // TIER 8: Spells/Traps Setup
        // ==========================================

        private bool DctEternalDarknessEffect()
        {
            if (Bot.LifePoints <= 1500) return false;
            if (_eternalDarknessUsed) return false;

            // Only activate if we actually have 2 P-Scales and at least 1 D/D monster to protect!
            if (!HasBothPZonesFilled()) return false;
            if (!Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasSetcode(0xaf))) return false;

            bool edAlreadyOnField = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.DctEternalDarkness) && c != Card);
            if (edAlreadyOnField) return false;

            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                _eternalDarknessUsed = true;
                return true;
            }
            return false;
        }

        private bool EternalDarknessSetEffect()
        {
            if (Bot.LifePoints <= 1500) return false;
            bool edActiveOnField = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.DctEternalDarkness));
            if (edActiveOnField) return false;
            return true;
        }

        protected override int GetMaterialSacrificePriority(ClientCard c)
        {
            if (c == null) return 999999;
            if (c.Controller == 1) return 0; // opponent card

            // Absolute boss monsters - protect them!
            if (c.IsCode(CardId.WaveHighKingCaesar, CardId.DeusMachinex, CardId.CursedKingSiegfried))
                return 50000;
            if (c.IsCode(CardId.FirstKingClovis, CardId.SuperDoomKingDarkArmageddon, CardId.DimensionalKingArcCrisis))
                return 40000;

            // Xyz bridges ready to rank up into High Caesar or Machinex
            if (c.IsCode(CardId.MarksmanKingTell, CardId.WaveKingCaesarXyz, CardId.WiseKingSolomon))
                return 50;

            // Intermediate combo pieces
            if (c.IsCode(CardId.AbyssKingGilgamesh, CardId.FlameKingGenghis, CardId.FlameHighKingGenghis))
                return 150;

            if (c.Location == CardLocation.Grave) return 20;
            if (c.Location == CardLocation.Hand) return 40;

            return base.GetMaterialSacrificePriority(c);
        }

        protected override bool IsBoardStrongEnough()
        {
            bool hasCaesar = Bot.HasInMonstersZone(CardId.WaveHighKingCaesar);
            bool hasMachinex = Bot.HasInMonstersZone(CardId.DeusMachinex);
            bool hasSiegfried = Bot.HasInMonstersZone(CardId.CursedKingSiegfried);
            int bossCount = (hasCaesar ? 1 : 0) + (hasMachinex ? 1 : 0) + (hasSiegfried ? 1 : 0);
            return bossCount >= 2 || (hasMachinex && hasCaesar);
        }

        protected override bool ShouldStopExtending()
        {
            if (Duel.Turn == 1)
                return IsBoardStrongEnough();
            return base.ShouldStopExtending();
        }

        // ==========================================
        // Hook Overrides (OnSelectEffectYn, OnSelectYesNo)
        // ==========================================

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            if (card == null) return null;
            if (card.IsCode(CardId.AbyssKingGilgamesh)) return true;
            if (card.IsCode(CardId.FlameKingGenghis)) return true;
            if (card.IsCode(CardId.FlameHighKingGenghis)) return true;
            if (card.IsCode(CardId.FirstKingClovis)) return true;
            if (card.IsCode(CardId.MarksmanKingTell)) return true;
            if (card.IsCode(CardId.WaveKingCaesarXyz)) return true;
            if (card.IsCode(CardId.WiseKingSolomon)) return true;
            if (card.IsCode(CardId.ZeroDoomQueenMachinex)) return true;
            if (card.IsCode(CardId.Gryphon)) return true;
            if (card.IsCode(CardId.Copernicus)) return true;
            if (card.IsCode(CardId.Kepler)) return true;
            if (card.IsCode(CardId.ScaleSurveyor)) return true;
            return base.OnSelectEffectYn(card, desc);
        }

        public override bool OnSelectYesNo(long desc)
        {
            return true;
        }

        // ==========================================
        // Material & Hint Selection Handlers
        // ==========================================

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // Hint 510: TOFIELD (Place continuous Dark Contract / Field)
            if (hint == 510)
            {
                var preferred = cards.OrderByDescending(c =>
                {
                    if (c == null) return -1;
                    if (c.IsCode(CardId.DctEternalDarkness) && !Bot.HasInSpellZone(CardId.DctEternalDarkness)) return 1000;
                    if (c.IsCode(CardId.DctGate) && !Bot.HasInSpellZone(CardId.DctGate)) return 900;
                    if (c.IsCode(CardId.DctZeroKing) && !Bot.HasInSpellZone(CardId.DctZeroKing)) return 800;
                    return 100;
                }).ToList();
                return preferred.Take(max).ToList();
            }

            // Gilgamesh P-Zone placement from deck (only when triggered by Gilgamesh or min/max == 2 from Deck)
            ClientCard lastChain = Util.GetLastChainCard();
            bool isGilgameshTrigger = (lastChain != null && lastChain.IsCode(CardId.AbyssKingGilgamesh)) || (hint == 0 && max == 2);
            if (isGilgameshTrigger && cards.Any(c => c != null && c.Location == CardLocation.Deck && c.IsCode(CardId.ZeroDoomQueenMachinex, CardId.ScaleSurveyor, CardId.Kepler, CardId.CountSurveyor, CardId.Copernicus, CardId.Gryphon)))
            {
                var p0 = Util.GetPZone(0, 0);
                var p1 = Util.GetPZone(0, 1);
                bool hasLowScale = (p0 != null && (p0.IsCode(CardId.ZeroDoomQueenMachinex, CardId.CountSurveyor, CardId.Copernicus, CardId.Gryphon)))
                                || (p1 != null && (p1.IsCode(CardId.ZeroDoomQueenMachinex, CardId.CountSurveyor, CardId.Copernicus, CardId.Gryphon)));
                bool hasHighScale = (p0 != null && (p0.IsCode(CardId.ScaleSurveyor, CardId.Kepler)))
                                 || (p1 != null && (p1.IsCode(CardId.ScaleSurveyor, CardId.Kepler)));

                if (hasLowScale && !hasHighScale)
                {
                    var high = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.ScaleSurveyor, CardId.Kepler));
                    if (high != null) return new[] { high };
                }
                else if (!hasLowScale && hasHighScale)
                {
                    var low = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.ZeroDoomQueenMachinex, CardId.CountSurveyor, CardId.Copernicus));
                    if (low != null) return new[] { low };
                }
                else
                {
                    var low = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.ZeroDoomQueenMachinex, CardId.CountSurveyor, CardId.Copernicus));
                    var high = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.ScaleSurveyor, CardId.Kepler) && c != low);
                    var res = new List<ClientCard>();
                    if (low != null) res.Add(low);
                    if (high != null && res.Count < max) res.Add(high);
                    if (res.Count >= min) return res;
                }
            }

            // Hint 506: ATOHAND (Search from Deck/GY)
            if (hint == 506)
            {
                var preferred = cards.OrderByDescending(c =>
                {
                    if (c == null) return -1;
                    if (c.IsCode(CardId.SwirlSlime) && !Bot.HasInHand(CardId.SwirlSlime)) return 1000;
                    if (c.IsCode(CardId.DctGate) && !Bot.HasInHand(CardId.DctGate) && !Bot.HasInSpellZone(CardId.DctGate)) return 950;
                    if (c.IsCode(CardId.Gryphon) && !Bot.HasInHand(CardId.Gryphon)) return 900;
                    if (c.IsCode(CardId.Copernicus) && !Bot.HasInHand(CardId.Copernicus)) return 850;
                    if (c.IsCode(CardId.Kepler) && !Bot.HasInHand(CardId.Kepler)) return 800;
                    if (c.IsCode(CardId.DctEternalDarkness) && !Bot.HasInSpellZone(CardId.DctEternalDarkness)) return 750;
                    if (c.IsCode(CardId.Lamia)) return 700;
                    if (c.IsCode(CardId.NecroSlime)) return 650;
                    if (c.IsCode(CardId.ScaleSurveyor)) return 600;
                    return 100;
                }).ToList();
                return preferred.Take(max).ToList();
            }

            // Hint 509: SPSUMMON (Special Summon target)
            if (hint == 509)
            {
                var preferred = cards.OrderByDescending(c =>
                {
                    if (c == null) return -1;
                    // Fusion target priorities
                    if (c.IsCode(CardId.FlameKingGenghis) && !Bot.HasInMonstersZone(CardId.FlameKingGenghis)) return 1000;
                    if (c.IsCode(CardId.Gryphon) && !_gryphonSearchUsed) return 980;
                    if (c.IsCode(CardId.Copernicus) && !_copernicusUsed) return 960;
                    if (c.IsCode(CardId.WaveHighKingCaesar)) return 940;
                    if (c.IsCode(CardId.CursedKingSiegfried)) return 920;
                    if (c.IsCode(CardId.DeusMachinex)) return 900;
                    if (c.IsCode(CardId.FlameHighKingGenghis)) return 880;
                    if (c.IsCode(CardId.AlfredDivineSage)) return 860;
                    if (c.IsCode(CardId.Lamia)) return 840;
                    if (c.IsCode(CardId.ScaleSurveyor)) return 820;
                    return c.Attack;
                }).ToList();
                return preferred.Take(max).ToList();
            }

            // Hint 504 / 501: TOGRAVE / DISCARD (Send to GY — Foolish Burial)
            if (hint == 504 || hint == 501)
            {
                var preferred = cards.OrderByDescending(c =>
                {
                    if (c == null) return -1;
                    if (c.IsCode(CardId.Lamia) && !Bot.HasInGraveyard(CardId.Lamia)) return 1000;
                    if (c.IsCode(CardId.NecroSlime) && !Bot.HasInGraveyard(CardId.NecroSlime)) return 950;
                    if (c.IsCode(CardId.Gryphon) && !Bot.HasInGraveyard(CardId.Gryphon)) return 900;
                    if (c.IsCode(CardId.LanceSoldier) && !Bot.HasInGraveyard(CardId.LanceSoldier)) return 850;
                    if (c.IsCode(CardId.SwirlSlime)) return 800;
                    if (c.IsCode(CardId.CountSurveyor)) return 750;
                    return 100;
                }).ToList();
                return preferred.Take(max).ToList();
            }

            // Hint 502: DESTROY / POP (Pop target)
            if (hint == 502)
            {
                var sorted = cards.OrderByDescending(c =>
                {
                    if (c == null) return -999;
                    int score = (c.Controller == 1) ? 10000 : 0;
                    if (c.Controller == 1)
                    {
                        if (c.IsMonster()) score += (c.IsFaceup() ? c.Attack : 1500);
                        if (c.IsSpell() || c.IsTrap()) score += (c.IsFaceup() ? 2500 : 2000);
                    }
                    else
                    {
                        // Friendly target for destruction (e.g. Zero King, Lance Soldier)
                        if (c.IsCode(CardId.DctGate, CardId.DctZeroKing)) score = 500;
                        if (c.IsCode(CardId.Kepler)) score = 400;
                        if (IsAceCard(c)) score = -10000;
                    }
                    return score;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // Hint 533: COST (Protect Ace cards)
            if (hint == 533)
            {
                var safe = cards.Where(c => c != null && !IsAceCard(c) && !c.IsCode(CardId.DctEternalDarkness)).ToList();
                if (safe.Count >= min) return safe.Take(max).ToList();
            }

            // Protect Ace cards on field
            if (cards.Any(c => c != null && c.Controller == 0 && c.Location == CardLocation.MonsterZone && IsAceCard(c)))
            {
                var safe = cards.Where(c => c == null || c.Controller != 0 || c.Location != CardLocation.MonsterZone || !IsAceCard(c)).ToList();
                if (safe.Count >= min) return safe.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override IList<ClientCard> OnSelectFusionMaterial(IList<ClientCard> cards, int min, int max)
        {
            var sorted = cards.OrderBy(c =>
            {
                if (c == null) return 999;
                if (c.Controller == 0 && IsAceCard(c)) return 10000;
                if (c.Location == CardLocation.Grave) return 10;
                if (c.Location == CardLocation.Hand) return 20;
                if (c.Location == CardLocation.MonsterZone) return 30;
                return 100;
            }).ToList();
            return sorted.Take(max).ToList();
        }

        public override IList<ClientCard> OnSelectSynchroMaterial(IList<ClientCard> cards, int sum, int min, int max)
        {
            var sorted = cards.OrderBy(c =>
            {
                if (c == null) return 999;
                if (c.Controller == 0 && IsAceCard(c)) return 10000;
                if (c.IsCode(CardId.Lamia)) return 10;
                if (c.IsCode(DDDMonsters)) return 20;
                if (c.IsCode(HandTraps)) return 800;
                return 100;
            }).ToList();
            return sorted.Take(max).ToList();
        }

        public override IList<ClientCard> OnSelectXyzMaterial(IList<ClientCard> cards, int min, int max)
        {
            var sorted = cards.OrderBy(c =>
            {
                if (c == null) return 999;
                if (c.Controller == 0 && IsAceCard(c)) return 10000;
                if (c.IsCode(CardId.MarksmanKingTell)) return 5;
                if (c.IsCode(CardId.WaveKingCaesarXyz)) return 10;
                if (c.IsCode(CardId.AbyssKingGilgamesh)) return 15;
                if (c.IsCode(CardId.FlameKingGenghis)) return 20;
                if (c.IsCode(DDDMonsters)) return 30;
                return 100;
            }).ToList();
            return sorted.Take(max).ToList();
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            int[] defMonsters = {
                CardId.Copernicus, CardId.Lamia, CardId.SwirlSlime, CardId.NecroSlime, CardId.ScaleSurveyor
            };

            if (defMonsters.Contains(cardId) && positions.Contains(CardPosition.FaceUpDefence))
                return CardPosition.FaceUpDefence;

            return base.OnSelectPosition(cardId, positions);
        }

        public override int OnSelectOption(IList<long> options)
        {
            for (int i = 0; i < options.Count; i++)
            {
                long cardId = options[i] >> 20;
                if (cardId == 0 && Card != null) cardId = Card.Id;
                long optIndex = options[i] & 0xfffff;

                if (cardId == CardId.SwirlSlime && optIndex == 0) return i;
                if (cardId == CardId.NecroSlime && optIndex == 0) return i;
                if (cardId == CardId.DeusMachinex && optIndex == 0) return i;
            }
            return 0;
        }

        public override int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
            if (location == CardLocation.MonsterZone)
            {
                // Extra Monster Zone for Gilgamesh Link-2
                if (cardId == CardId.AbyssKingGilgamesh)
                {
                    int emz1 = 1 << 5;
                    int emz2 = 1 << 6;
                    if ((available & emz1) > 0) return emz1;
                    if ((available & emz2) > 0) return emz2;
                }

                // Place High Caesar / Siegfried / Deus Machinex in central zones
                int centerZone = 1 << 2;
                if ((available & centerZone) > 0)
                {
                    if (cardId == CardId.WaveHighKingCaesar || cardId == CardId.CursedKingSiegfried || cardId == CardId.DeusMachinex)
                        return centerZone;
                }
                int nonCenterAvailable = available & ~(1 << 2);
                if (nonCenterAvailable > 0)
                    return nonCenterAvailable;
            }
            return base.OnSelectPlace(cardId, player, location, available);
        }

        public override bool OnPreBattleBetween(ClientCard attacker, ClientCard defender)
        {
            if (attacker == null) return false;
            if (defender == null) return true;
            return base.OnPreBattleBetween(attacker, defender);
        }
    }
}

