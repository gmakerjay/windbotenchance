// ============================================================================
// PENDULUM MAGICIAN EXECUTOR — SUPREME KING Z-ARC & SYNCHRO/XYZ/LINK AI
// ============================================================================
// Architecture: ModernExecutor + Decoupled Domain Plugin (Layer 3)
// Features:
//   - Autonomous Scale Management via ScaleResolver (Scale 1 with Scale 8)
//   - Harmonizing Magician 1-Card Engine (Level 8 Synchro / Rank 4 Xyz)
//   - Timestar Magician + Time Pendulumgraph Non-Targeting Removal Engine
//   - Electrumite + Astrograph Sorcerer Advantage Loop
//   - Odd-Eyes Absolute Dragon -> Odd-Eyes Vortex Dragon Omni-Negate
//   - Supreme King Z-ARC Complete Board Wipe & 4-Dragon Fusion
//   - Anti-Pattern Guards: Never place Harmonizing in scale, check Allure DARKs
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Plugin;
using WindBot.Game.AI.Plugins;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("PendulumMagician", "PendulumMagician")]
    [Deck("SupremeMagician", "PendulumMagician")]
    public class PendulumMagicianExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Monsters — Magicians & Performapals
            public const int AstrographSorcerer = 76794549;
            public const int ChronographSorcerer = 12289247;
            public const int TimegazerMagician = 20409757;
            public const int PerformapalSkullcrobatJoker = 40318957;
            public const int WisdomEyeMagician = 72714461;
            public const int HarmonizingMagician = 73941492;
            public const int DoubleIrisMagician = 49684352;
            public const int PurplePoisonMagician = 48461764;
            public const int BlackFangMagician = 75672051;
            public const int OafdragonMagician = 14920218;
            public const int WhiteWingMagician = 11067666;
            public const int DragonpitMagician = 51531505;
            public const int PerformapalCelestialMagician = 58092907;
            public const int LusterPendulum = 92746535;

            // Handtraps & Staples
            public const int AshBlossom = 14558127;
            public const int MaxxC = 23434538;
            public const int InfiniteImpermanence = 10045474;
            public const int CalledByTheGrave = 24224830;
            public const int HarpiesFeatherDuster = 18144507;
            public const int PotOfDesires = 35261759;
            public const int AllureOfDarkness = 1475311;

            // Spells & Traps — Magician Engine
            public const int DuelistAlliance = 37469904;
            public const int PendulumCall = 53208660;
            public const int StarPendulumgraph = 74850403;
            public const int TimePendulumgraph = 1344018;

            // Legacy / Support Cards
            public const int TuningMagician = 54941203;
            public const int MagiciansSouls = 97631303;
            public const int ArtifactScythe = 20292186;
            public const int ArtifactDagda = 7480763;
            public const int CrystronHalqifibrax = 50588353;
            public const int TGWonderMagician = 98558751;
            public const int FormulaSynchron = 50091196;
            public const int Linkuriboh = 41999284;

            // Extra Deck Bosses
            public const int SupremeKingZARC = 13331639;
            public const int TimestarMagician = 47349116;
            public const int AbyssDweller = 21044178;
            public const int TornadoDragon = 6983839;
            public const int SupremeKingDragonDarkRebellion = 42160203;
            public const int SupremeKingDragonClearWing = 70771599;
            public const int IgnisterProminence = 18239909;
            public const int BaronneDeFleur = 84815190;
            public const int BorreloadSavageDragon = 27548199;
            public const int Number41Bagooska = 90590303;
            public const int AccesscodeTalker = 86066372;
            public const int HeavymetalfoesElectrumite = 24094258;
            public const int BeyondThePendulum = 22125101;
            public const int SPLittleKnight = 29301450;
            public const int Apollousa = 4280258;
        }

        // Audited OCGCore Hint Constants
        private const long HINT_DISCARD = 501;
        private const long HINT_DESTROY = 502;
        private const long HINT_REMOVE = 503;
        private const long HINT_TOGRAVE = 504;
        private const long HINT_RTOHAND = 505;
        private const long HINT_ATOHAND = 506;
        private const long HINT_SPSUMMON = 509;

        // Turn Tracking State
        private bool _electrumiteSendUsed = false;
        private bool _electrumitePopUsed = false;
        private bool _wisdomEyeUsed = false;
        private bool _pendulumCallUsed = false;
        private bool _duelistAllianceUsed = false;
        private bool _timestarSearchUsed = false;
        private bool _baronneNegateUsed = false;
        private bool _baronnePopUsed = false;
        private bool _timePendulumgraphUsed = false;
        private bool _hasPendulumSummoned = false;
        private bool _astrographSSUsed = false;
        private bool _zarcSummoned = false;
        public bool NormalSummonUsed { get; set; } = false;

        public PendulumMagicianExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // 0. Install Decoupled Domain Plugin Architecture (Layer 3)
            DeckPlugin = new PendulumMagicianPlugin(this);

            // 1. Register Modules
            RegisterHelperModules();
            RegisterComboLines();
            RegisterExecutors();
        }

        private void RegisterHelperModules()
        {
            // Layer 2 Central Core Ace Card Protection
            ResourcePlan.RegisterAceCards(
                CardId.SupremeKingZARC,
                CardId.BaronneDeFleur,
                CardId.BorreloadSavageDragon,
                CardId.AccesscodeTalker,
                CardId.Apollousa,
                CardId.Number41Bagooska,
                CardId.TornadoDragon,
                CardId.TimestarMagician,
                CardId.SupremeKingDragonClearWing,
                CardId.SupremeKingDragonDarkRebellion
            );
            HeuristicGuard.RegisterAceCards(
                CardId.SupremeKingZARC,
                CardId.BaronneDeFleur,
                CardId.BorreloadSavageDragon,
                CardId.AccesscodeTalker,
                CardId.Apollousa,
                CardId.Number41Bagooska,
                CardId.TornadoDragon,
                CardId.TimestarMagician,
                CardId.SupremeKingDragonClearWing,
                CardId.SupremeKingDragonDarkRebellion
            );

            // Layer 2 Handtrap Bait & Starters
            BaitPlanner.RegisterComboStarters(
                CardId.PerformapalSkullcrobatJoker,
                CardId.WisdomEyeMagician,
                CardId.HarmonizingMagician,
                CardId.DuelistAlliance
            );
            BaitPlanner.RegisterBaitCards(
                CardId.AllureOfDarkness,
                CardId.PotOfDesires,
                CardId.PendulumCall
            );

            // Layer 2 High Value Chain Targets
            ChainAdvisor.RegisterHighValueTargets(
                CardId.HeavymetalfoesElectrumite,
                CardId.BeyondThePendulum,
                CardId.BaronneDeFleur,
                CardId.BorreloadSavageDragon,
                CardId.TimestarMagician,
                CardId.AccesscodeTalker
            );
        }

        private void RegisterComboLines()
        {
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Magician-Harmonizing-Baronne",
                RequiredCards = new List<int> { CardId.HarmonizingMagician },
                EndBoardScore = 98,
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.HarmonizingMagician, ActionType = ExecutorType.SpSummon, Description = "Pendulum Summon Harmonizing from Hand" },
                    new() { CardId = CardId.HarmonizingMagician, ActionType = ExecutorType.Activate, Description = "Special Summon Oafdragon from Deck" },
                    new() { CardId = CardId.BaronneDeFleur, ActionType = ExecutorType.SpSummon, Description = "Synchro Summon Baronne de Fleur (Omni-Negate)" }
                }
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Magician-Electrumite-Astrograph-Loop",
                RequiredCards = new List<int> { CardId.HeavymetalfoesElectrumite },
                EndBoardScore = 95,
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.HeavymetalfoesElectrumite, ActionType = ExecutorType.SpSummon, Description = "Link Summon Electrumite" },
                    new() { CardId = CardId.HeavymetalfoesElectrumite, ActionType = ExecutorType.Activate, Description = "Send Astrograph to Extra Deck" },
                    new() { CardId = CardId.AstrographSorcerer, ActionType = ExecutorType.Activate, Description = "Add Astrograph & Special Summon" }
                }
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Magician-Harmonizing-BorreloadSavage",
                RequiredCards = new List<int> { CardId.HarmonizingMagician },
                EndBoardScore = 94,
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.HarmonizingMagician, ActionType = ExecutorType.SpSummon, Description = "Pendulum Summon Harmonizing from Hand" },
                    new() { CardId = CardId.HarmonizingMagician, ActionType = ExecutorType.Activate, Description = "Special Summon Level 4 Magician from Deck" },
                    new() { CardId = CardId.BorreloadSavageDragon, ActionType = ExecutorType.SpSummon, Description = "Synchro Summon Borreload Savage Dragon (2x Omni-Negate)" }
                }
            });
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _electrumiteSendUsed = false;
            _electrumitePopUsed = false;
            _wisdomEyeUsed = false;
            _pendulumCallUsed = false;
            _duelistAllianceUsed = false;
            _timestarSearchUsed = false;
            _baronneNegateUsed = false;
            _baronnePopUsed = false;
            _timePendulumgraphUsed = false;
            _hasPendulumSummoned = false;
            _astrographSSUsed = false;
            _zarcSummoned = false;
            NormalSummonUsed = false;

            DeckPlugin?.ResetTurnState();
        }

        public override bool OnSelectHand()
        {
            // Pendulum Magicians thrive going first to establish multiple negates & Time Pendulumgraph
            return true;
        }

        // ============================================================================
        // EXECUTOR PIPELINE
        // ============================================================================
        private void RegisterExecutors()
        {
            // ── Tier 0: Quick Negations & Handtraps ──
            AddExecutor(ExecutorType.Activate, CardId.BorreloadSavageDragon, BorreloadSavageEffect);
            AddExecutor(ExecutorType.Activate, CardId.BaronneDeFleur, BaronneNegate);
            AddExecutor(ExecutorType.Activate, CardId.Apollousa, ApollousaNegate);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomActivate);
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCActivate);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, ImpermActivate);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveActivate);
            AddExecutor(ExecutorType.Activate, CardId.TornadoDragon, TornadoDragonActivate);
            AddExecutor(ExecutorType.Activate, CardId.TimePendulumgraph, TimePendulumgraphActivate);
            AddExecutor(ExecutorType.Activate, CardId.AbyssDweller, AbyssDwellerActivate);

            // ── Tier 1: Board Breakers (Going 2nd & Ignition Pops) ──
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, HarpiesFeatherDusterActivate);
            AddExecutor(ExecutorType.Activate, CardId.SupremeKingDragonClearWing, SKClearWingBoardWipe);
            AddExecutor(ExecutorType.Activate, CardId.SupremeKingDragonDarkRebellion, SKDarkRebellionOTK);
            AddExecutor(ExecutorType.Activate, CardId.IgnisterProminence, IgnisterActivate);
            AddExecutor(ExecutorType.Activate, CardId.PurplePoisonMagician, PurplePoisonMonsterPop);
            AddExecutor(ExecutorType.Activate, CardId.BlackFangMagician, BlackFangMonsterRevive);
            AddExecutor(ExecutorType.Activate, CardId.BaronneDeFleur, BaronnePopCard);

            // ── Tier 2: Supreme King Z-ARC Summoning (Ultimate Wipe) ──
            AddExecutor(ExecutorType.Activate, CardId.AstrographSorcerer, AstrographZARCSummon);
            AddExecutor(ExecutorType.Activate, CardId.ChronographSorcerer, ChronographZARCSummon);
            AddExecutor(ExecutorType.Activate, CardId.SupremeKingZARC, ZARCOnSummonWipe);

            // ── Tier 3: Draw & Search Engines (Pre-Pendulum) ──
            AddExecutor(ExecutorType.Activate, CardId.AllureOfDarkness, AllureOfDarknessActivate);
            AddExecutor(ExecutorType.Activate, CardId.PotOfDesires, PotOfDesiresActivate);

            // STRICT SEQUENCE: Wisdom-Eye pop MUST trigger BEFORE Pendulum Call!
            AddExecutor(ExecutorType.Activate, CardId.WisdomEyeMagician, WisdomEyePendulumEffect);
            AddExecutor(ExecutorType.Summon, CardId.PerformapalSkullcrobatJoker, SkullcrobatSummon);
            AddExecutor(ExecutorType.Activate, CardId.PerformapalSkullcrobatJoker, SkullcrobatActivate);
            AddExecutor(ExecutorType.Activate, CardId.DuelistAlliance, DuelistAllianceActivate);
            AddExecutor(ExecutorType.Activate, CardId.PendulumCall, PendulumCallActivate);
            AddExecutor(ExecutorType.Activate, CardId.StarPendulumgraph, StarPendulumgraphActivate);

            // ── Tier 4: Scale Setup ──
            AddExecutor(ExecutorType.Activate, CardId.DoubleIrisMagician, ScalePlacementEffect);
            AddExecutor(ExecutorType.Activate, CardId.PurplePoisonMagician, ScalePlacementEffect);
            AddExecutor(ExecutorType.Activate, CardId.BlackFangMagician, ScalePlacementEffect);
            AddExecutor(ExecutorType.Activate, CardId.WhiteWingMagician, ScalePlacementEffect);
            AddExecutor(ExecutorType.Activate, CardId.OafdragonMagician, ScalePlacementEffect);
            AddExecutor(ExecutorType.Activate, CardId.DragonpitMagician, ScalePlacementEffect);
            AddExecutor(ExecutorType.Activate, CardId.ChronographSorcerer, ScalePlacementEffect);
            AddExecutor(ExecutorType.Activate, CardId.AstrographSorcerer, ScalePlacementEffect);
            AddExecutor(ExecutorType.Activate, CardId.TimegazerMagician, ScalePlacementEffect);

            // ── Tier 5: Pre-Pendulum Link Climbs (Electrumite & Beyond) ──
            AddExecutor(ExecutorType.SpSummon, CardId.HeavymetalfoesElectrumite, ElectrumiteSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.HeavymetalfoesElectrumite, ElectrumiteActivate);
            AddExecutor(ExecutorType.Activate, CardId.AstrographSorcerer, AstrographHandSS);
            AddExecutor(ExecutorType.Activate, CardId.ChronographSorcerer, ChronographHandSS);
            AddExecutor(ExecutorType.SpSummon, CardId.BeyondThePendulum, BeyondThePendulumSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.BeyondThePendulum, BeyondThePendulumActivate);

            // ── Tier 6: The Pendulum Summon ──
            AddExecutor(ExecutorType.SpSummon, ExecutePendulumSummon);

            // ── Tier 7: Post-Pendulum Extra Deck Combinations ──
            AddExecutor(ExecutorType.Activate, CardId.HarmonizingMagician, HarmonizingOnSummon);
            AddExecutor(ExecutorType.Activate, CardId.AstrographSorcerer, AstrographHandSS);
            AddExecutor(ExecutorType.Activate, CardId.ChronographSorcerer, ChronographHandSS);

            // 1. Synchro Summons First (consumes Tuner while available!)
            AddExecutor(ExecutorType.SpSummon, CardId.BaronneDeFleur, BaronneSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.BorreloadSavageDragon, BorreloadSavageSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SupremeKingDragonClearWing, SKClearWingSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.IgnisterProminence, IgnisterSpSummon);

            // 2. Post-Pendulum Electrumite Loop (if not summoned pre-pendulum!)
            AddExecutor(ExecutorType.SpSummon, CardId.HeavymetalfoesElectrumite, ElectrumitePostPendulumSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.HeavymetalfoesElectrumite, ElectrumiteActivate);

            // 3. Xyz Summons (using remaining Level 4s)
            AddExecutor(ExecutorType.SpSummon, CardId.TornadoDragon, TornadoDragonSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AbyssDweller, AbyssDwellerSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.TimestarMagician, TimestarSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TimestarMagician, TimestarActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.Number41Bagooska, BagooskaSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SupremeKingDragonDarkRebellion, SKDarkRebellionSpSummon);

            // 4. Link Finishers & Controls
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, AccesscodeSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.AccesscodeTalker, AccesscodeActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.Apollousa, ApollousaSpSummon);

            // ── Tier 8: Setting S/T & Fallback Normal Summons ──
            AddExecutor(ExecutorType.SpellSet, CardId.TimePendulumgraph);
            AddExecutor(ExecutorType.SpellSet, CardId.StarPendulumgraph);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, ImpermSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, CalledByTheGraveSet);

            AddExecutor(ExecutorType.Summon, FallbackNormalSummon);
            AddExecutor(ExecutorType.Repos, SmartMonsterRepos);
        }

        // ============================================================================
        // HELPER QUERIES & CHECKS
        // ============================================================================
        public bool HasDragonInGraveyard(int id)
        {
            return Bot.Graveyard.Any(c => c.Id == id);
        }

        public bool HasLowScaleInHandOrPZone()
        {
            var left = Util.GetPZone(0, 0);
            var right = Util.GetPZone(0, 1);
            if ((left != null && left.LScale <= 3) || (right != null && right.RScale <= 3)) return true;
            return Bot.Hand.Any(c => c.HasType(CardType.Pendulum) && c.LScale <= 3);
        }

        public bool HasHighScaleInHandOrPZone()
        {
            var left = Util.GetPZone(0, 0);
            var right = Util.GetPZone(0, 1);
            if ((left != null && left.LScale >= 8) || (right != null && right.RScale >= 8)) return true;
            return Bot.Hand.Any(c => c.HasType(CardType.Pendulum) && c.LScale >= 8);
        }

        public bool HasFullScales()
        {
            var left = Util.GetPZone(0, 0);
            var right = Util.GetPZone(0, 1);
            return left != null && right != null && left.LScale != right.RScale;
        }

        private bool IsChainAlreadyNeutralized()
        {
            ClientCard last = LastChainCard;
            if (last == null || last.Controller != 1) return true;
            if (last.IsDisabled()) return true;
            return false;
        }

        private bool CanSummonZARC()
        {
            // Requires 4 Dragon Magicians across Hand, Field, and GY:
            // 1. Pendulum Dragon: Double Iris Magician
            // 2. Synchro Dragon: White Wing Magician
            // 3. Xyz Dragon: Black Fang Magician
            // 4. Fusion Dragon: Purple Poison Magician
            bool hasPendulum = HasDragonAnywhere(CardId.DoubleIrisMagician);
            bool hasSynchro = HasDragonAnywhere(CardId.WhiteWingMagician);
            bool hasXyz = HasDragonAnywhere(CardId.BlackFangMagician);
            bool hasFusion = HasDragonAnywhere(CardId.PurplePoisonMagician);

            return hasPendulum && hasSynchro && hasXyz && hasFusion;
        }

        private bool HasDragonAnywhere(int id)
        {
            return Bot.Hand.Any(c => c.Id == id) ||
                   Bot.GetMonsters().Any(c => c.Id == id) ||
                   Bot.GetSpells().Any(c => c.Id == id) ||
                   Bot.Graveyard.Any(c => c.Id == id);
        }

        // ============================================================================
        // QUICK NEGATES & DISRUPTIONS
        // ============================================================================
        private bool BorreloadSavageEffect()
        {
            if (Duel.CurrentChain.Count == 0)
            {
                // Equip effect on summon: prioritize highest rating Link in GY
                AI.SelectCard(new[] { CardId.HeavymetalfoesElectrumite, CardId.BeyondThePendulum, CardId.SPLittleKnight });
                return true;
            }
            else
            {
                // Quick Effect Omni-Negate
                if (IsChainAlreadyNeutralized()) return false;
                ClientCard last = LastChainCard;
                if (last == null || last.Controller != 1) return false;
                return true;
            }
        }

        private bool BaronneNegate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_baronneNegateUsed) return false;
            if (IsChainAlreadyNeutralized()) return false;

            _baronneNegateUsed = true;
            return true;
        }

        private bool ApollousaNegate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.Attack < 800) return false;
            if (IsChainAlreadyNeutralized()) return false;

            ClientCard last = LastChainCard;
            if (last == null || !last.IsMonster() || last.Controller != 1) return false;

            return true;
        }

        private bool AshBlossomActivate()
        {
            if (IsChainAlreadyNeutralized()) return false;
            ClientCard last = LastChainCard;
            if (last == null || last.Controller != 1) return false;
            return CardIntelligence.IsHighThreatChokepoint(last.Id) || Duel.Player == 1;
        }

        private bool MaxxCActivate()
        {
            if (Duel.Player == 0) return false; // Handtrap during opponent turn
            if (Duel.LastChainPlayer == 0) return false; // No self-chain
            return true;
        }

        private bool ImpermActivate()
        {
            if (IsChainAlreadyNeutralized()) return false;
            ClientCard last = LastChainCard;
            if (last == null || !last.IsMonster() || last.Controller != 1) return false;
            return true;
        }

        private bool CalledByTheGraveActivate()
        {
            if (IsChainAlreadyNeutralized()) return false;
            ClientCard last = LastChainCard;
            if (last == null || last.Controller != 1) return false;
            // Target monster in opponent's GY
            return Enemy.Graveyard.Any(c => c.IsMonster() && c.Id == last.Id);
        }

        private bool TimePendulumgraphActivate()
        {
            if (_timePendulumgraphUsed) return false;

            // Must have friendly Magician card to target
            bool hasFriendlyTarget = Bot.GetMonsters().Concat(Bot.GetSpells())
                .Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Pendulum));
            if (!hasFriendlyTarget) return false;

            // Immediate reaction: If opponent has Eternal Soul (48680970) face-up, POP IT NOW!
            if (Enemy.GetSpells().Any(s => s != null && s.IsFaceup() && s.Id == 48680970))
            {
                _timePendulumgraphUsed = true;
                return true;
            }

            // During opponent turn or going 2nd to break board
            if (Duel.Player == 1 || Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0)
            {
                if (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0)
                {
                    _timePendulumgraphUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool AbyssDwellerActivate()
        {
            // Activate during opponent turn or going 2nd if opponent has GY effects
            if (Duel.Player == 1 && Card.Location == CardLocation.MonsterZone && Card.Overlays.Count > 0)
                return true;
            return false;
        }

        private bool TornadoDragonActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.Overlays.Count == 0) return false;

            // Absolute #1: Pop opponent's Eternal Soul (48680970) immediately!
            if (Enemy.GetSpells().Any(s => s != null && s.IsFaceup() && s.Id == 48680970))
                return true;

            // Pop any opponent continuous/field/floodgate spells or traps
            if (Enemy.GetSpells().Any(s => s != null && s.IsFaceup() && (CardIntelligence.IsFloodgate(s.Id) || s.HasType(CardType.Continuous) || s.HasType(CardType.Field))))
                return true;

            // During opponent's turn, pop any face-up or set backrow
            if (Duel.Player == 1 && Enemy.GetSpellCount() > 0)
                return true;

            // Main Phase 2 before end
            if (Duel.Phase == DuelPhase.Main2 && Enemy.GetSpellCount() > 0)
                return true;

            return false;
        }

        // ============================================================================
        // BOARD BREAKERS
        // ============================================================================
        private bool HarpiesFeatherDusterActivate()
        {
            return Enemy.GetSpellCount() > 0;
        }

        private bool SKClearWingBoardWipe()
        {
            // Destroys all face-up monsters opponent controls on summon
            return Enemy.GetMonsters().Any(m => m.IsFaceup());
        }

        private bool SKDarkRebellionOTK()
        {
            // Target face-up opponent monster to reduce ATK to 0 and absorb ATK
            return Enemy.GetMonsters().Any(m => m.IsFaceup() && m.Attack > 0);
        }

        private bool IgnisterActivate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            // Shuffle 1 card without targeting!
            return Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
        }

        private bool PurplePoisonMonsterPop()
        {
            // When destroyed: pop 1 face-up card
            return Enemy.GetMonsters().Concat(Enemy.GetSpells()).Any(c => c != null && c.IsFaceup());
        }

        private bool BlackFangMonsterRevive()
        {
            // When destroyed: revive 1 DARK Spellcaster from GY
            return Bot.Graveyard.Any(c => c.IsMonster() && c.HasRace(CardRace.SpellCaster) && c.HasAttribute(CardAttribute.Dark));
        }

        private bool BaronnePopCard()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_baronnePopUsed) return false;
            // Target 1 card on field to destroy
            if (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0)
            {
                _baronnePopUsed = true;
                return true;
            }
            return false;
        }

        // ============================================================================
        // Z-ARC SUMMONING
        // ============================================================================
        private bool AstrographZARCSummon()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_zarcSummoned) return false;

            if (CanSummonZARC())
            {
                _zarcSummoned = true;
                return true;
            }
            return false;
        }

        private bool ChronographZARCSummon()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_zarcSummoned) return false;

            if (CanSummonZARC())
            {
                _zarcSummoned = true;
                return true;
            }
            return false;
        }

        private bool ZARCOnSummonWipe()
        {
            // Destroy all cards opponent controls
            return true;
        }

        // ============================================================================
        // DRAW & PRE-PENDULUM CONSISTENCY
        // ============================================================================
        private bool AllureOfDarknessActivate()
        {
            // Anti-Brick Guard: STRICTLY require at least 1 DARK monster in hand!
            return Bot.Hand.Count(c => c.IsMonster() && c.HasAttribute(CardAttribute.Dark)) >= 1;
        }

        private bool PotOfDesiresActivate()
        {
            // Safe draw 2
            return Bot.Deck.Count >= 15;
        }

        private bool WisdomEyePendulumEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Place in scale if either scale is empty
                return Util.GetPZone(0, 0) == null || Util.GetPZone(0, 1) == null;
            }

            if (Card.Location == CardLocation.SpellZone)
            {
                if (_wisdomEyeUsed) return false;
                // Requires another Magician / Performapal in the other P-Zone
                ClientCard otherScale = Card.Sequence == 0 ? Util.GetPZone(0, 1) : Util.GetPZone(0, 0);
                if (otherScale != null)
                {
                    _wisdomEyeUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool SkullcrobatSummon()
        {
            // Prime Normal Summon starter
            NormalSummonUsed = true;
            return true;
        }

        private bool SkullcrobatActivate()
        {
            // On Normal Summon: search Magician / Performapal / Odd-Eyes
            return true;
        }

        private bool DuelistAllianceActivate()
        {
            if (_duelistAllianceUsed) return false;
            // Requires card in Pendulum Zone
            if (Util.GetPZone(0, 0) == null && Util.GetPZone(0, 1) == null) return false;

            _duelistAllianceUsed = true;
            return true;
        }

        private bool PendulumCallActivate()
        {
            if (_pendulumCallUsed) return false;
            if (Bot.Hand.Count < 2) return false;

            // If we have Wisdom-Eye in scale, let Wisdom-Eye pop first!
            if (Bot.GetSpells().Any(s => s != null && s.IsFaceup() && s.Id == CardId.WisdomEyeMagician && !_wisdomEyeUsed))
            {
                return false;
            }

            _pendulumCallUsed = true;
            return true;
        }

        private bool StarPendulumgraphActivate()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                // Activate Continuous Spell
                return Bot.GetSpellCount() < 5;
            }
            // Trigger effect when Magician card leaves zone
            return true;
        }

        // ============================================================================
        // SCALE PLACEMENT LOGIC
        // ============================================================================
        private bool ScalePlacementEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;

            // Check if scales already complete
            if (HasFullScales()) return false;

            var left = Util.GetPZone(0, 0);
            var right = Util.GetPZone(0, 1);

            // Never place Harmonizing in scale if we have alternatives
            if (Card.Id == CardId.HarmonizingMagician)
            {
                var otherHigh = Bot.Hand.FirstOrDefault(c => c.HasType(CardType.Pendulum) && c.LScale >= 8 && c.Id != CardId.HarmonizingMagician);
                if (otherHigh != null) return false;
            }

            // If left is empty and card is low scale
            if (left == null && (Card.LScale <= 3 || right != null && right.RScale >= 8))
            {
                return true;
            }

            // If right is empty and card is high scale
            if (right == null && (Card.LScale >= 8 || left != null && left.LScale <= 3))
            {
                return true;
            }

            return left == null || right == null;
        }

        // ============================================================================
        // PRE-PENDULUM LINK EXTENDERS
        // ============================================================================
        private bool ElectrumiteSpSummon()
        {
            if (_electrumiteSendUsed) return false;
            // 2 Pendulum Monsters
            var pendMonsters = Bot.GetMonsters().Where(m => m.IsFaceup() && m.HasType(CardType.Pendulum) && !IsAceCard(m)).ToList();
            return pendMonsters.Count >= 2;
        }

        private bool ElectrumiteActivate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Effect 1: Send Astrograph Sorcerer to Extra Deck
            if (!_electrumiteSendUsed)
            {
                _electrumiteSendUsed = true;
                AI.SelectCard(CardId.AstrographSorcerer);
                return true;
            }

            // Effect 2: Destroy 1 card to add Astrograph to hand
            if (!_electrumitePopUsed)
            {
                ClientCard popTarget = (DeckPlugin?.ScaleResolver as PendulumMagicianScaleResolver)?.PickScalePopTarget();
                if (popTarget != null)
                {
                    _electrumitePopUsed = true;
                    AI.SelectCard(popTarget);
                    AI.SelectNextCard(CardId.AstrographSorcerer);
                    return true;
                }
            }

            // Effect 3: Draw 1 card when scale leaves field
            return true;
        }

        private bool AstrographHandSS()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_astrographSSUsed) return false;

            // Triggers when a card is destroyed (e.g. by Electrumite or Wisdom-Eye)
            if (Bot.GetMonsterCount() < 5)
            {
                _astrographSSUsed = true;
                return true;
            }
            return false;
        }

        private bool ChronographHandSS()
        {
            if (Card.Location != CardLocation.Hand) return false;
            return Bot.GetMonsterCount() < 5;
        }

        private bool BeyondThePendulumSpSummon()
        {
            // Only summon Beyond if we don't have full scales and need to search
            if (HasFullScales()) return false;
            var pends = Bot.GetMonsters().Where(m => m.IsFaceup() && m.HasType(CardType.Pendulum) && !IsAceCard(m)).ToList();
            return pends.Count >= 2;
        }

        private bool BeyondThePendulumActivate()
        {
            // Pay 1200 LP to search missing scale
            return true;
        }

        // ============================================================================
        // PENDULUM SUMMON
        // ============================================================================
        private bool ExecutePendulumSummon()
        {
            _hasPendulumSummoned = true;
            return true;
        }

        // ============================================================================
        // POST-PENDULUM COMBOS
        // ============================================================================
        private bool HarmonizingOnSummon()
        {
            // When Pendulum Summoned from Hand: Special Summon 1 Magician from Deck
            return true;
        }

        private bool BaronneSpSummon()
        {
            if (Bot.GetMonsters().Any(m => m.Id == CardId.BaronneDeFleur)) return false;
            // Level 10 Synchro (Level 4 Tuner Harmonizing + Level 6 non-Tuner Oafdragon)
            return Bot.GetMonsters().Any(m => m.IsFaceup() && m.IsTuner() && m.Level == 4) &&
                   Bot.GetMonsters().Any(m => m.IsFaceup() && !m.IsTuner() && m.Level == 6);
        }

        private bool BorreloadSavageSpSummon()
        {
            if (Bot.GetMonsters().Any(m => m.Id == CardId.BorreloadSavageDragon)) return false;
            // Requires 1 Tuner + 1+ non-Tuner monsters
            // In Pendulum Magicians: Harmonizing (4 Tuner) + Level 4 non-Tuner Magician = Level 8
            // Must have a Link monster in GY to equip!
            bool hasLinkInGY = Bot.Graveyard.Any(c => c.HasType(CardType.Link));
            if (!hasLinkInGY) return false;

            return Bot.GetMonsters().Any(m => m.IsFaceup() && m.IsTuner() && m.Level == 4) &&
                   Bot.GetMonsters().Any(m => m.IsFaceup() && !m.IsTuner() && m.Level == 4 && m.HasType(CardType.Pendulum));
        }

        private bool SKClearWingSpSummon()
        {
            // 1 Tuner + 1+ non-Tuner DARK Pendulum Monsters (Level 8)
            // Harmonizing (4 Tuner) + Level 4 non-Tuner = Level 8
            // Prefer when breaking board going 2nd
            if (Duel.Player == 0 && Enemy.GetMonsterCount() > 0)
            {
                return Bot.GetMonsters().Any(m => m.IsFaceup() && m.IsTuner() && m.Level == 4) &&
                       Bot.GetMonsters().Any(m => m.IsFaceup() && !m.IsTuner() && m.Level == 4);
            }
            return false;
        }

        private bool IgnisterSpSummon()
        {
            // Level 8 Synchro (1 Tuner + 1+ non-Tuner Pendulum Monsters)
            return Bot.GetMonsters().Any(m => m.IsFaceup() && m.IsTuner() && m.Level == 4) &&
                   Bot.GetMonsters().Any(m => m.IsFaceup() && !m.IsTuner() && m.Level == 4 && m.HasType(CardType.Pendulum));
        }

        private bool ElectrumitePostPendulumSpSummon()
        {
            if (_electrumiteSendUsed) return false;
            if (Bot.GetMonsters().Any(m => m.Id == CardId.HeavymetalfoesElectrumite)) return false;

            // Use 2 non-Tuner Pendulum monsters so we preserve Harmonizing for Synchro!
            bool hasTuner = Bot.GetMonsters().Any(m => m.IsFaceup() && m.IsTuner());
            List<ClientCard> pends;
            if (hasTuner)
            {
                pends = Bot.GetMonsters().Where(m => m.IsFaceup() && m.HasType(CardType.Pendulum) && !m.IsTuner() && !IsAceCard(m)).ToList();
            }
            else
            {
                pends = Bot.GetMonsters().Where(m => m.IsFaceup() && m.HasType(CardType.Pendulum) && !IsAceCard(m)).ToList();
            }

            return pends.Count >= 2;
        }

        private bool TimestarSpSummon()
        {
            if (_timestarSearchUsed) return false;
            if (Bot.GetMonsters().Any(m => m.Id == CardId.TimestarMagician)) return false;
            // 2 Level 4 Magician Pendulum Monsters
            var lv4s = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 4 && m.HasType(CardType.Pendulum) && !IsAceCard(m)).ToList();
            return lv4s.Count >= 2;
        }

        private bool TimestarActivate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_timestarSearchUsed) return false;

            _timestarSearchUsed = true;
            return true;
        }

        private bool TornadoDragonSpSummon()
        {
            if (Bot.GetMonsters().Any(m => m.Id == CardId.TornadoDragon)) return false;
            var lv4s = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 4 && !IsAceCard(m)).ToList();
            if (lv4s.Count < 2) return false;

            // Summon if opponent has Spells/Traps or against backrow decks
            return Enemy.GetSpellCount() > 0 || Enemy.GetSpells().Any(s => s != null && s.Id == 48680970);
        }

        private bool AbyssDwellerSpSummon()
        {
            if (Bot.GetMonsters().Any(m => m.Id == CardId.AbyssDweller)) return false;
            var lv4s = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 4 && !IsAceCard(m)).ToList();
            return lv4s.Count >= 2 && (Duel.Turn == 1 || Enemy.Graveyard.Count >= 2);
        }

        private bool BagooskaSpSummon()
        {
            if (Bot.GetMonsters().Any(m => m.Id == CardId.Number41Bagooska)) return false;
            var lv4s = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 4 && !IsAceCard(m)).ToList();
            if (lv4s.Count < 2) return false;

            // Defense floodgate when Turn 1 or stalling
            return Duel.Turn == 1 || Enemy.GetMonsterCount() >= 2;
        }

        private bool SKDarkRebellionSpSummon()
        {
            // 2 Level 4 DARK Pendulum Monsters (OTK Machine)
            if (Duel.Turn > 1 && Enemy.GetMonsterCount() > 0)
            {
                var lv4s = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 4 && m.HasAttribute(CardAttribute.Dark) && !IsAceCard(m)).ToList();
                return lv4s.Count >= 2;
            }
            return false;
        }

        private bool AccesscodeSpSummon()
        {
            if (Duel.Turn == 1) return false; // Finisher for Turn 2+
            var linkMonsters = Bot.GetMonsters().Where(m => m.IsFaceup() && m.HasType(CardType.Link)).ToList();
            if (linkMonsters.Count == 0) return false;

            var nonAceMats = Bot.GetMonsters().Where(m => m.IsFaceup() && !IsAceCard(m)).ToList();
            return nonAceMats.Count >= 2;
        }

        private bool AccesscodeActivate()
        {
            // Boost ATK / Banish to pop opponent card
            return true;
        }

        private bool SPLittleKnightSpSummon()
        {
            // Main Phase 2 or when clearing threat
            if (Duel.Phase == DuelPhase.Main2 || Enemy.GetMonsterCount() > 0)
            {
                var mats = Bot.GetMonsters().Where(m => m.IsFaceup() && !IsAceCard(m) && m.Attack < 2000).ToList();
                return mats.Count >= 2;
            }
            return false;
        }

        private bool SPLittleKnightActivate()
        {
            // Banish 1 card from opponent field or GY
            return true;
        }

        private bool ApollousaSpSummon()
        {
            // Multi-negate boss if we have surplus monsters
            var mats = Bot.GetMonsters().Where(m => m.IsFaceup() && !IsAceCard(m)).ToList();
            return mats.Count >= 3;
        }

        // ============================================================================
        // SETTING S/T & REPOS
        // ============================================================================
        private bool ImpermSpellSet()
        {
            return Card.Location == CardLocation.Hand && Bot.GetSpellCount() < 5;
        }

        private bool CalledByTheGraveSet()
        {
            return Card.Location == CardLocation.Hand && Bot.GetSpellCount() < 5;
        }

        private bool FallbackNormalSummon()
        {
            if (NormalSummonUsed) return false;
            // Normal summon Skullcrobat or low ATK fodder if normal summon unused
            if (Card.Id == CardId.PerformapalSkullcrobatJoker ||
                (Card.Attack <= 1500 && Bot.GetMonsterCount() < 5))
            {
                NormalSummonUsed = true;
                return true;
            }
            return false;
        }

        private bool SmartMonsterRepos()
        {
            if (Card.IsAttack() && Card.Attack == 0) return true;
            if (Card.IsDefense() && Card.Attack >= 2000) return true;
            return false;
        }

        // ============================================================================
        // CARD & TARGET SELECTION OVERRIDES
        // ============================================================================
        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards != null && cards.Count > 0)
            {
                // 1. Deck Search (HINTMSG_ATOHAND = 506)
                if (hint == HINT_ATOHAND || cards.All(c => c.Location == CardLocation.Deck))
                {
                    var target = DeckPlugin?.Strategy?.PickSearchTarget(cards, Card);
                    if (target != null)
                    {
                        var result = new List<ClientCard> { target };
                        var remaining = cards.Where(c => c != target).Take(max - 1);
                        result.AddRange(remaining);
                        return result;
                    }
                }

                // 2. Special Summon (HINTMSG_SPSUMMON = 509)
                if (hint == HINT_SPSUMMON)
                {
                    var target = DeckPlugin?.Strategy?.PickSpecialSummonTarget(cards);
                    if (target != null)
                    {
                        var result = new List<ClientCard> { target };
                        var remaining = cards.Where(c => c != target).Take(max - 1);
                        result.AddRange(remaining);
                        return result;
                    }
                }

                // 3. Discard Cost (HINTMSG_DISCARD = 501)
                if (hint == HINT_DISCARD)
                {
                    var target = DeckPlugin?.MaterialEvaluator?.PickDiscardTarget(cards, min);
                    if (target != null)
                    {
                        var result = new List<ClientCard> { target };
                        var remaining = cards.Where(c => c != target).Take(max - 1);
                        result.AddRange(remaining);
                        return result;
                    }
                }

                // 4. Destruction / Removal (HINTMSG_DESTROY = 502, HINTMSG_REMOVE = 503, HINTMSG_TOGRAVE = 504)
                if (hint == HINT_DESTROY || hint == HINT_REMOVE || hint == HINT_TOGRAVE)
                {
                    // If Timestar replacement: send Dragon Magician from Deck to GY
                    if (cards.All(c => c.Location == CardLocation.Deck))
                    {
                        var sub = DeckPlugin?.MaterialEvaluator?.PickDestructionSubstitute(cards, min);
                        if (sub != null) return new List<ClientCard> { sub };
                    }

                    // Self-pop targeting (when all candidates are friendly, e.g. Electrumite or Ignister cost)
                    if (cards.All(c => c.Controller == 0))
                    {
                        var selfPop = (DeckPlugin?.ScaleResolver as PendulumMagicianScaleResolver)?.PickScalePopTarget();
                        if (selfPop != null && cards.Contains(selfPop))
                        {
                            return new List<ClientCard> { selfPop };
                        }
                    }

                    // Enemy cards targeting
                    var enemyTargets = cards.Where(c => c.Controller == 1).ToList();
                    if (enemyTargets.Count >= min)
                    {
                        bool enemyHasEternalSoul = Enemy.GetSpells().Any(s => s != null && s.IsFaceup() && s.Id == 48680970);

                        return enemyTargets.OrderByDescending(c =>
                        {
                            // 1. Eternal Soul: Destroying this card automatically wipes all opponent monsters!
                            if (c.Id == 48680970) return 999999;

                            // 2. Dark Magical Circle: Banishing removal engine
                            if (c.Id == 47222536) return 80000;

                            // 3. Floodgates & Negators
                            if (CardIntelligence.IsFloodgate(c.Id)) return 70000;
                            if (CardIntelligence.IsKnownNegator(c.Id)) return 60000;

                            // 4. Immune checks: Never target cards immune to targeting or card effects!
                            if (CardIntelligence.IsTargetImmune(c)) return -5000;

                            // If Eternal Soul is active, Dark Magician monsters are unaffected by card effects!
                            if (enemyHasEternalSoul && (c.Id == 46986414 || c.Name?.Contains("Dark Magician") == true))
                                return -10000; // Do NOT target Dark Magician while Eternal Soul protects it!

                            // 5. Threat score via CardIntelligence
                            return CardIntelligence.GetCardThreatScore(c, hint);
                        }).Take(max).ToList();
                    }

                    // Fallback self-pop
                    var fallbackSelfPop = (DeckPlugin?.ScaleResolver as PendulumMagicianScaleResolver)?.PickScalePopTarget();
                    if (fallbackSelfPop != null && cards.Contains(fallbackSelfPop))
                    {
                        return new List<ClientCard> { fallbackSelfPop };
                    }
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            // Rule 16: ALWAYS reject opponent optional effects!
            if (card != null && card.Controller == 1) return false;

            // Accept self optional triggers
            return true;
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (positions == null || positions.Count == 0) return CardPosition.FaceUpAttack;
            if (positions.Count == 1) return positions[0];

            // Rule 14: Number 41: Bagooska MUST always be summoned in FaceUpDefence!
            if (cardId == CardId.Number41Bagooska)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            }

            var cardData = YGOSharp.OCGWrapper.NamedCard.Get(cardId);
            if (cardData != null)
            {
                if (cardData.HasType(CardType.Link)) return CardPosition.FaceUpAttack;

                // 0 ATK monsters or Handtraps -> Defense
                if (cardData.Attack == 0 || CardIntelligence.IsHandtrap(cardId))
                {
                    if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                    if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                }

                // Harmonizing target (effects negated, defense position specified)
                if (positions.Contains(CardPosition.FaceUpDefence) && cardData.Attack < 1800)
                {
                    return CardPosition.FaceUpDefence;
                }

                // Bosses & High ATK -> Attack
                if (cardData.Attack >= 1800 && positions.Contains(CardPosition.FaceUpAttack))
                {
                    return CardPosition.FaceUpAttack;
                }
            }

            return base.OnSelectPosition(cardId, positions);
        }
    }
}
