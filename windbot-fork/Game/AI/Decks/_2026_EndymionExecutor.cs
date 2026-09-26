// ============================================================================
// 2026_Endymion — DECOUPLED DECK PLUGIN AI ARCHITECTURE (v11.0 Audited)
// ============================================================================
// Deck: Endymion Spell Counter Control
// Mechanics:
//   - Dynamic Spell Counter Economy (CRITICAL, LOW, READY, COMBO READY, SURPLUS)
//   - Net Gain Arithmetic on Spell Resolution
//   - Jackal King 2-Counter Monster Negate Budget & Threat Prioritization
//   - Mighty Master Spell/Trap Quick Negate & Intelligent Bounce Recycling
//   - Mighty Master 6-Counter Board Break & OTK Engine
//   - Servant of Endymion 3-Counter Combo Progression Gate
//   - Magister of Endymion Extra Deck Swarm & Opponent Turn Quick SS
//   - Electrumite + Astrograph Loop -> Selene Queen of Master Magicians
//   - Odd-Eyes Absolute Dragon -> Gravity Controller -> Odd-Eyes Vortex Dragon
//   - 1 Negate = 1 Problem (No redundant negation stacking)
//   - Emergency Counter Breach Policy (Under Lethal or Floodgate threats)
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_Endymion", "2026_Endymion")]
    public class _2026_EndymionExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Monsters — Endymion & Mythical Beasts
            public const int EndymionMightyMaster = 3611830;
            public const int MythicalBeastMasterCerberus = 53842431;
            public const int MythicalBeastJackalKing = 27354732;
            public const int ServantOfEndymion = 92559258;
            public const int MagisterOfEndymion = 66104644;
            public const int ReflectionOfEndymion = 39000945;
            public const int MythicalBeastGaruda = 28570310;
            public const int MythicalBeastJackal = 91182675;
            public const int AstrographSorcerer = 76794549;
            public const int SpellbookMagician = 14824019;

            // Spells — Engine & Draw Power
            public const int SpellPowerMastery = 38943357;
            public const int MythicalInstitution = 94599451;
            public const int MagicalCitadel = 39910367;
            public const int SecretVillage = 68462976;
            public const int Terraforming = 73628505;
            public const int SpellbookOfSecrets = 89739383;
            public const int SpellbookOfKnowledge = 23314220;
            public const int UpstartGoblin = 70368879;
            public const int IntoTheVoid = 93946239;
            public const int PotOfDesires = 35261759;
            public const int TripleTacticsTalent = 25311006;

            // Staples & Defensives
            public const int CalledByTheGrave = 24224830;
            public const int HarpiesFeatherDuster = 18144507;
            public const int LightningStorm = 14532163;
            public const int DarkRulerNoMore = 54693926;

            // Extra Deck
            public const int HeavymetalfoesElectrumite = 24094258;
            public const int BeyondThePendulum = 22125101;
            public const int CrowleyTheFirstPropheseer = 50756327;
            public const int SeleneQueenOfTheMasterMagicians = 45819647;
            public const int ApollousaBowOfTheGoddess = 4280258;
            public const int AccesscodeTalker = 86066372;
            public const int SPLittleKnight = 29301450;
            public const int IPMasquerena = 65741786;
            public const int OddEyesAbsoluteDragon = 16691074;
            public const int OddEyesVortexDragon = 53262004;
            public const int GravityController = 23656668;
            public const int DayBreaker = 91336701;
            public const int DharcTheDarkCharmer = 8264361;
            public const int KnightmareUnicorn = 38342335;
        }

        // Audited OCGCore Hint Constants
        private const long HINT_RELEASE = 500;
        private const long HINT_DISCARD = 501;
        private const long HINT_DESTROY = 502;
        private const long HINT_REMOVE = 503;
        private const long HINT_TOGRAVE = 504;
        private const long HINT_RTOHAND = 505;
        private const long HINT_ATOHAND = 506;
        private const long HINT_SUMMON = 508;
        private const long HINT_SPSUMMON = 509;
        private const long HINT_FMATERIAL = 511;
        private const long HINT_SMATERIAL = 512;
        private const long HINT_XMATERIAL = 513;
        private const long HINT_LMATERIAL = 533;
        private const long HINT_TARGET = 551;
        private const long HINT_TOZONE = 571;
        private const long HINT_COUNTER = 572;
        private const long HINT_NEGATE = 575;

        // Counter Economy Levels
        public enum CounterLevel
        {
            Critical = 0, // 0-1
            Low = 1,      // 2-3
            Ready = 2,    // 4-5
            ComboReady = 3, // 6-7
            Surplus = 4   // 8+
        }

        // Internal Counter Tracking Engine
        private readonly Dictionary<ClientCard, int> _cardCounters = new Dictionary<ClientCard, int>();

        // Per-turn tracking
        private bool _servantUsed = false;
        private bool _magisterUsed = false;
        private bool _reflectionUsed = false;
        private bool _mightyMasterUsed = false;
        private bool _masterCerberusPZoneUsed = false;
        private bool _masterCerberusMZoneUsed = false;
        private bool _jackalKingNegateUsed = false;
        private bool _electrumiteSendUsed = false;
        private bool _electrumitePopUsed = false;
        private bool _seleneUsed = false;
        private bool _spellPowerMasteryUsed = false;
        private bool _knowledgeUsed = false;
        private bool _secretsUsed = false;
        private bool _prophecyUsed = false;
        private bool _absoluteUsed = false;
        private bool _crowleyUsed = false;
        private bool _garudaUsed = false;

        // Central Domain Plugin Coordinator (Layer 3)
        internal EndymionPlugin Plugin { get; private set; }

        public _2026_EndymionExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            Plugin = new EndymionPlugin(this);
            RegisterHelperModules();
            RegisterComboLines();
            RegisterExecutors();
        }

        private void RegisterHelperModules()
        {
            // 1. Layer 2 Central Core Ace Card Protection
            ResourcePlan.RegisterAceCards(
                CardId.EndymionMightyMaster,
                CardId.MythicalBeastJackalKing,
                CardId.OddEyesVortexDragon,
                CardId.ApollousaBowOfTheGoddess,
                CardId.AccesscodeTalker,
                CardId.SeleneQueenOfTheMasterMagicians
            );
            HeuristicGuard.RegisterAceCards(
                CardId.EndymionMightyMaster,
                CardId.MythicalBeastJackalKing,
                CardId.OddEyesVortexDragon,
                CardId.ApollousaBowOfTheGoddess,
                CardId.AccesscodeTalker,
                CardId.SeleneQueenOfTheMasterMagicians
            );

            // 2. Layer 2 Handtrap Bait & Combo Starters
            BaitPlanner.RegisterComboStarters(
                CardId.SpellPowerMastery,
                CardId.MythicalBeastMasterCerberus,
                CardId.ServantOfEndymion,
                CardId.SpellbookMagician
            );
            BaitPlanner.RegisterBaitCards(
                CardId.UpstartGoblin,
                CardId.IntoTheVoid,
                CardId.PotOfDesires,
                CardId.SpellbookOfSecrets,
                CardId.SpellbookOfKnowledge
            );

            // 3. Layer 2 High Value Chain Targets
            ChainAdvisor.RegisterHighValueTargets(
                CardId.HeavymetalfoesElectrumite,
                CardId.SeleneQueenOfTheMasterMagicians,
                CardId.BeyondThePendulum,
                CardId.ServantOfEndymion,
                CardId.OddEyesAbsoluteDragon
            );
        }

        private void RegisterComboLines()
        {
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Endymion-Servant-Jackal-Setup",
                RequiredCards = new List<int> { CardId.ServantOfEndymion },
                EndBoardScore = 90,
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.ServantOfEndymion, ActionType = ExecutorType.Activate, Description = "Place Servant in Scale" },
                    new() { CardId = CardId.ServantOfEndymion, ActionType = ExecutorType.Activate, Description = "Remove 3 counters -> Special Summon Servant + Jackal King from Deck" }
                }
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Endymion-Electrumite-Astrograph-Loop",
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
                Name = "Endymion-Absolute-Vortex-Negate",
                RequiredCards = new List<int> { CardId.OddEyesAbsoluteDragon },
                EndBoardScore = 92,
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.OddEyesAbsoluteDragon, ActionType = ExecutorType.SpSummon, Description = "Xyz Summon Absolute Dragon with 2 Lv7s in EMZ" },
                    new() { CardId = CardId.GravityController, ActionType = ExecutorType.SpSummon, Description = "Link Absolute into Gravity Controller" },
                    new() { CardId = CardId.OddEyesAbsoluteDragon, ActionType = ExecutorType.Activate, Description = "Trigger Absolute in GY -> SS Vortex Dragon" }
                }
            });
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _servantUsed = false;
            _magisterUsed = false;
            _reflectionUsed = false;
            _mightyMasterUsed = false;
            _masterCerberusPZoneUsed = false;
            _masterCerberusMZoneUsed = false;
            _jackalKingNegateUsed = false;
            _electrumiteSendUsed = false;
            _electrumitePopUsed = false;
            _seleneUsed = false;
            _spellPowerMasteryUsed = false;
            _knowledgeUsed = false;
            _secretsUsed = false;
            _prophecyUsed = false;
            _absoluteUsed = false;
            _crowleyUsed = false;
            _garudaUsed = false;

            Plugin?.ResetTurnState();
        }

        public override void OnChaining(int player, ClientCard card)
        {
            base.OnChaining(player, card);
            if (card != null && card.IsSpell())
            {
                // Each time a Spell Card is activated, place Spell Counters when that Spell resolves
                foreach (ClientCard c in Bot.GetMonsters().Concat(Bot.GetSpells()))
                {
                    if (c == null || !c.IsFaceup()) continue;
                    if (c.Id == CardId.MagicalCitadel ||
                        c.Id == CardId.ServantOfEndymion ||
                        c.Id == CardId.MagisterOfEndymion ||
                        c.Id == CardId.ReflectionOfEndymion)
                    {
                        AddCardCounters(c, 1);
                    }
                    else if ((c.Id == CardId.MythicalBeastJackalKing || c.Id == CardId.MythicalBeastMasterCerberus)
                             && c.Location == CardLocation.MonsterZone)
                    {
                        AddCardCounters(c, 2);
                    }
                }
            }
        }

        public override bool OnSelectHand()
        {
            // Endymion thrives going first to establish 3-4 negates + counter reserves
            return true;
        }

        // ============================================================================
        // COUNTER ECONOMY DELEGATION (Single Source of Truth: Plugin.CounterEconomy)
        // ============================================================================
        public int GetCardCounters(ClientCard card) => Plugin?.CounterEconomy?.GetCounters(card) ?? 0;
        public void SetCardCounters(ClientCard card, int count) => Plugin?.CounterEconomy?.SetCounters(card, count);
        public void AddCardCounters(ClientCard card, int count) => Plugin?.CounterEconomy?.AddCounters(card, count);
        public int GetTotalCountersOnField() => Plugin?.CounterEconomy?.GetTotalCountersOnField() ?? 0;
        public CounterLevel GetCounterLevel() => Plugin?.CounterEconomy?.GetCounterLevel() ?? CounterLevel.Critical;
        public bool CanSafelySpendCounters(int cost) => Plugin?.CounterEconomy?.CanSafelySpendCounters(cost) ?? false;

        private int GetReserveThreshold()
        {
            // Need at least 2 counters reserved for Jackal King monster negate
            bool hasJackalKing = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Id == CardId.MythicalBeastJackalKing);
            return hasJackalKing ? 2 : 0;
        }

        private bool IsEmergencyState()
        {
            // Threat evaluation: Opponent threatens lethal OTK or resolving a high-threat effect
            if (Duel.Player == 1)
            {
                int oppAtk = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup() && m.IsAttack()).Sum(m => m.Attack);
                if (oppAtk >= Bot.LifePoints) return true;

                ClientCard last = LastChainCard;
                if (last != null && last.Controller == 1)
                {
                    if (CardIntelligence.IsHighThreatChokepoint(last.Id) || CardIntelligence.IsFloodgate(last.Id))
                        return true;
                }
            }
            return false;
        }

        // ============================================================================
        // EXECUTOR PIPELINE REGISTRATION
        // ============================================================================
        private void RegisterExecutors()
        {
            // ── Tier 0: Quick Negations & Handtraps ──
            AddExecutor(ExecutorType.Activate, CardId.MythicalBeastJackalKing, JackalKingNegate);
            AddExecutor(ExecutorType.Activate, CardId.EndymionMightyMaster, MightyMasterNegate);
            AddExecutor(ExecutorType.Activate, CardId.OddEyesVortexDragon, VortexNegate);
            AddExecutor(ExecutorType.Activate, CardId.ApollousaBowOfTheGoddess, ApollousaNegate);
            AddExecutor(ExecutorType.Activate, CardId.MythicalBeastGaruda, GarudaBounce);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveActivate);

            // ── Tier 1: Board Breakers (Going 2nd) ──
            AddExecutor(ExecutorType.Activate, CardId.DarkRulerNoMore, DarkRulerActivate);
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, HarpiesFeatherDusterActivate);
            AddExecutor(ExecutorType.Activate, CardId.LightningStorm, LightningStormActivate);

            // ── Tier 2: Master Cerberus Search & Field Spell Setup ──
            AddExecutor(ExecutorType.Activate, CardId.MythicalBeastMasterCerberus, MasterCerberusPendulum);
            AddExecutor(ExecutorType.Activate, CardId.Terraforming, TerraformingActivate);
            AddExecutor(ExecutorType.Activate, CardId.MagicalCitadel, MagicalCitadelActivate);
            AddExecutor(ExecutorType.Activate, CardId.SecretVillage, SecretVillageActivate);
            AddExecutor(ExecutorType.Activate, CardId.MythicalInstitution, MythicalInstitutionActivate);

            // ── Tier 3: Draw Engine & Consistency Spells ──
            AddExecutor(ExecutorType.Activate, CardId.UpstartGoblin, UpstartGoblinActivate);
            AddExecutor(ExecutorType.Activate, CardId.IntoTheVoid, IntoTheVoidActivate);
            AddExecutor(ExecutorType.Activate, CardId.PotOfDesires, PotOfDesiresActivate);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentActivate);

            // ── Tier 4: Spellbook Engine ──
            AddExecutor(ExecutorType.Summon, CardId.SpellbookMagician, SpellbookMagicianSummon);
            AddExecutor(ExecutorType.Activate, CardId.SpellbookMagician, SpellbookMagicianActivate);
            AddExecutor(ExecutorType.Activate, CardId.SpellbookOfSecrets, SpellbookOfSecretsActivate);
            AddExecutor(ExecutorType.Activate, CardId.SpellbookOfKnowledge, SpellbookOfKnowledgeActivate);

            // ── Tier 5: Spell Power Mastery ──
            AddExecutor(ExecutorType.Activate, CardId.SpellPowerMastery, SpellPowerMasteryActivate);

            // ── Tier 6: Scale Placement & Pendulum Zone Activations ──
            AddExecutor(ExecutorType.Activate, CardId.ServantOfEndymion, ServantPendulumEffect);
            AddExecutor(ExecutorType.Activate, CardId.MagisterOfEndymion, MagisterPendulumEffect);
            AddExecutor(ExecutorType.Activate, CardId.ReflectionOfEndymion, ReflectionPendulumEffect);
            AddExecutor(ExecutorType.Activate, CardId.EndymionMightyMaster, MightyMasterBoardBreak);

            // ── Tier 7: Extra Deck Climbs ──
            AddExecutor(ExecutorType.SpSummon, CardId.CrowleyTheFirstPropheseer, CrowleySpSummon);
            AddExecutor(ExecutorType.Activate, CardId.CrowleyTheFirstPropheseer, CrowleyActivate);

            AddExecutor(ExecutorType.SpSummon, CardId.HeavymetalfoesElectrumite, ElectrumiteSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.HeavymetalfoesElectrumite, ElectrumiteActivate);

            AddExecutor(ExecutorType.Activate, CardId.AstrographSorcerer, AstrographActivate);

            AddExecutor(ExecutorType.SpSummon, CardId.BeyondThePendulum, BeyondThePendulumSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.BeyondThePendulum, BeyondThePendulumActivate);

            // Pendulum Summon
            AddExecutor(ExecutorType.SpSummon, ExecutePendulumSummon);

            // ── Tier 8: Post-Pendulum Extra Deck Bosses ──
            AddExecutor(ExecutorType.SpSummon, CardId.OddEyesAbsoluteDragon, AbsoluteDragonSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.GravityController, GravityControllerSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.OddEyesAbsoluteDragon, AbsoluteDragonGYEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.SeleneQueenOfTheMasterMagicians, SeleneSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SeleneQueenOfTheMasterMagicians, SeleneActivate);

            AddExecutor(ExecutorType.SpSummon, CardId.ApollousaBowOfTheGoddess, ApollousaSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, AccesscodeSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.AccesscodeTalker, AccesscodeActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightActivate);

            // ── Tier 9: Monster Zone Ignition Effects ──
            AddExecutor(ExecutorType.Activate, CardId.MythicalBeastMasterCerberus, MasterCerberusMonsterBanish);
            AddExecutor(ExecutorType.Activate, CardId.ReflectionOfEndymion, ReflectionMonsterBounce);
            AddExecutor(ExecutorType.Activate, CardId.MythicalBeastJackal, JackalMonsterEffect);

            // ── Tier 10: Fallback Normal Summons & Scale Sets ──
            AddExecutor(ExecutorType.Summon, CardId.ServantOfEndymion, FallbackSummon);
            AddExecutor(ExecutorType.Summon, CardId.MagisterOfEndymion, FallbackSummon);
            AddExecutor(ExecutorType.Summon, FallbackNormalSummon);

            AddExecutor(ExecutorType.Repos, SmartMonsterRepos);
        }

        // ============================================================================
        // QUICK DISRUPTIONS & NEGATION BUDGET
        // ============================================================================
        private bool IsChainAlreadyNeutralized()
        {
            // Rule: 1 Negate = 1 Problem. Do not double negate an already negated chain!
            ClientCard last = LastChainCard;
            if (last == null || last.Controller != 1) return true;
            if (last.IsDisabled()) return true;
            return false;
        }

        private bool JackalKingNegate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_jackalKingNegateUsed) return false;

            ClientCard last = LastChainCard;
            if (last == null || last.Controller != 1 || !last.IsMonster()) return false;
            if (IsChainAlreadyNeutralized()) return false;

            // Check if we have 2 counters available
            int totalCounters = GetTotalCountersOnField();
            if (totalCounters < 2) return false;

            // Threat evaluation
            bool isBoss = last.Attack >= 2400 || CardIntelligence.IsKnownNegator(last.Id) || CardIntelligence.IsHighThreatChokepoint(last.Id);
            bool isHighThreat = CardIntelligence.IsHighThreatChokepoint(last.Id);
            bool isEmergency = IsEmergencyState();

            if (isBoss || isHighThreat || isEmergency || GetCounterLevel() >= CounterLevel.Ready)
            {
                _jackalKingNegateUsed = true;
                return true;
            }

            return false;
        }

        private bool MightyMasterNegate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_mightyMasterUsed) return false;

            ClientCard last = LastChainCard;
            if (last == null || last.Controller != 1) return false;
            if (!last.IsSpell() && !last.IsTrap()) return false;
            if (IsChainAlreadyNeutralized()) return false;

            // Must have a card with a Spell Counter to return to hand
            ClientCard returnTarget = SelectCardToReturnForMightyMaster();
            if (returnTarget != null)
            {
                AI.SelectCard(returnTarget);
                _mightyMasterUsed = true;
                return true;
            }

            return false;
        }

        private ClientCard SelectCardToReturnForMightyMaster()
        {
            // Priority:
            // 1. Servant of Endymion that already activated its effect this turn (recyclable scale!)
            // 2. Magister of Endymion that already activated
            // 3. Reflection of Endymion
            // 4. Face-up card with counters that fulfilled its duty
            // 5. Never bounce Mighty Master itself or unspent Jackal King

            var candidates = Bot.GetMonsters().Concat(Bot.GetSpells())
                .Where(c => c != null && c.IsFaceup() && GetCardCounters(c) > 0 && c != Card)
                .ToList();

            if (candidates.Count == 0) return null;

            // Priority 1: Used Servant
            var usedServant = candidates.FirstOrDefault(c => c.Id == CardId.ServantOfEndymion && _servantUsed);
            if (usedServant != null) return usedServant;

            // Priority 2: Used Magister
            var usedMagister = candidates.FirstOrDefault(c => c.Id == CardId.MagisterOfEndymion && _magisterUsed);
            if (usedMagister != null) return usedMagister;

            // Priority 3: Reflection
            var reflection = candidates.FirstOrDefault(c => c.Id == CardId.ReflectionOfEndymion);
            if (reflection != null) return reflection;

            // Priority 4: Highest counter holder (counters transfer to Mighty Master!)
            return candidates.OrderByDescending(c => GetCardCounters(c)).FirstOrDefault();
        }

        private bool VortexNegate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            ClientCard last = LastChainCard;
            if (last == null || last.Controller != 1) return false;
            if (IsChainAlreadyNeutralized()) return false;

            // Vortex returns 1 face-up Pendulum from Extra Deck to Deck to negate
            return Bot.ExtraDeck.Any(c => c.IsFaceup() && (c.Type & (int)CardType.Pendulum) != 0);
        }

        private bool ApollousaNegate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            ClientCard last = LastChainCard;
            if (last == null || last.Controller != 1 || !last.IsMonster()) return false;
            if (IsChainAlreadyNeutralized()) return false;
            return Card.Attack >= 800;
        }

        private bool GarudaBounce()
        {
            // Hand Quick Effect: Remove 3 counters to SS self and bounce summoned opponent monster
            if (Card.Location != CardLocation.Hand || _garudaUsed) return false;
            if (!CanSafelySpendCounters(3)) return false;

            if (Duel.Player == 1 && Bot.GetMonsterCount() < 5)
            {
                _garudaUsed = true;
                return true;
            }
            return false;
        }

        private bool CalledByTheGraveActivate()
        {
            ClientCard last = LastChainCard;
            if (last != null && last.Controller == 1)
            {
                ClientCard target = Enemy.Graveyard.FirstOrDefault(c => c.IsMonster() && c.Name == last.Name);
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        // ============================================================================
        // BOARD BREAKERS (GOING SECOND)
        // ============================================================================
        private bool DarkRulerActivate()
        {
            return Duel.Player == 0 && Duel.Turn > 1 && Enemy.GetMonsters().Any(m => m.IsFaceup() && !m.IsDisabled());
        }

        private bool HarpiesFeatherDusterActivate()
        {
            return Enemy.GetSpellCount() > 0;
        }

        private bool LightningStormActivate()
        {
            if (Bot.GetMonsters().Any(m => m.IsFaceup())) return false;
            if (Enemy.GetSpellCount() >= 2)
            {
                AI.SelectOption(1); // Destroy Spells/Traps
                return true;
            }
            if (Enemy.GetMonsters().Any(m => m.IsFaceup() && m.IsAttack()))
            {
                AI.SelectOption(0); // Destroy Attack Position Monsters
                return true;
            }
            return false;
        }

        // ============================================================================
        // PENDULUM SCALE SETUP & SPELLS
        // ============================================================================
        private bool MasterCerberusPendulum()
        {
            // If other P-Zone is empty, destroy self to search Jackal King or Garuda
            if (Card.Location != CardLocation.Hand && Card.Location != CardLocation.SpellZone) return false;
            if (_masterCerberusPZoneUsed) return false;

            if (Card.Location == CardLocation.Hand)
            {
                // Only place in scale if other P-Zone is completely empty
                return Bot.SpellZone[0] == null && Bot.SpellZone[4] == null;
            }

            // In Pendulum Zone: activate to destroy and search Jackal King
            if (Card.Location == CardLocation.SpellZone)
            {
                // Can only activate if the other Pendulum Zone is empty
                ClientCard otherScale = Card.Sequence == 0 ? Bot.SpellZone[4] : Bot.SpellZone[0];
                if (otherScale != null) return false;

                _masterCerberusPZoneUsed = true;
                AI.SelectCard(CardId.MythicalBeastJackalKing, CardId.MythicalBeastGaruda);
                return true;
            }
            return false;
        }

        private bool TerraformingActivate()
        {
            if (!Bot.HasInSpellZone(CardId.MagicalCitadel) && !Bot.HasInHand(CardId.MagicalCitadel))
            {
                AI.SelectCard(CardId.MagicalCitadel);
                return true;
            }
            AI.SelectCard(CardId.SecretVillage);
            return true;
        }

        private bool MagicalCitadelActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return !Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.MagicalCitadel);
            }
            return false;
        }

        private bool SecretVillageActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                bool hasSpellcaster = Bot.GetMonsters().Any(m => m.IsFaceup() && (m.Race & (int)CardRace.SpellCaster) != 0);
                return hasSpellcaster && !Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.SecretVillage);
            }
            return false;
        }

        private bool MythicalInstitutionActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return Bot.SpellZone.Count(s => s != null) < 5;
            }
            // In SpellZone: search if we have enough counters
            if (Card.Location == CardLocation.SpellZone && GetCardCounters(Card) >= 4)
            {
                AI.SelectCard(CardId.MythicalBeastJackalKing, CardId.MythicalBeastGaruda);
                return true;
            }
            return false;
        }

        private bool UpstartGoblinActivate()
        {
            return true;
        }

        private bool IntoTheVoidActivate()
        {
            return Bot.Hand.Count >= 3;
        }

        private bool PotOfDesiresActivate()
        {
            return Bot.Deck.Count >= 15;
        }

        private bool TripleTacticsTalentActivate()
        {
            // Default draw 2
            AI.SelectOption(0);
            return true;
        }

        private bool SpellbookMagicianSummon()
        {
            return !_prophecyUsed && Bot.GetMonsterCount() < 5;
        }

        private bool SpellbookMagicianActivate()
        {
            _prophecyUsed = true;
            AI.SelectCard(CardId.SpellbookOfSecrets, CardId.SpellbookOfKnowledge);
            return true;
        }

        private bool SpellbookOfSecretsActivate()
        {
            if (_secretsUsed) return false;
            _secretsUsed = true;
            if (!Bot.HasInHand(CardId.SpellbookOfKnowledge))
            {
                AI.SelectCard(CardId.SpellbookOfKnowledge);
            }
            else
            {
                AI.SelectCard(CardId.SpellbookMagician);
            }
            return true;
        }

        private bool SpellbookOfKnowledgeActivate()
        {
            if (_knowledgeUsed) return false;
            // Send SpellbookMagician on field or Spellcaster to draw 2
            ClientCard target = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.Id == CardId.SpellbookMagician)
                             ?? Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && (m.Race & (int)CardRace.SpellCaster) != 0 && m.Level <= 4);

            if (target != null)
            {
                _knowledgeUsed = true;
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool SpellPowerMasteryActivate()
        {
            if (_spellPowerMasteryUsed) return false;
            _spellPowerMasteryUsed = true;

            // Search priority: Servant -> Mighty Master -> Magister
            if (!Bot.HasInHand(CardId.ServantOfEndymion) && !Bot.HasInSpellZone(CardId.ServantOfEndymion))
            {
                AI.SelectCard(CardId.ServantOfEndymion);
            }
            else if (!Bot.HasInHand(CardId.EndymionMightyMaster))
            {
                AI.SelectCard(CardId.EndymionMightyMaster);
            }
            else
            {
                AI.SelectCard(CardId.MagisterOfEndymion, CardId.ReflectionOfEndymion);
            }
            return true;
        }

        // ============================================================================
        // PENDULUM SCALE EFFECTS (SERVANT, MAGISTER, MIGHTY MASTER)
        // ============================================================================
        private bool ServantPendulumEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Place as Scale 2
                return Bot.SpellZone[0] == null || Bot.SpellZone[4] == null;
            }

            if (Card.Location == CardLocation.SpellZone)
            {
                if (_servantUsed) return false;
                // Requires 3 counters on self to Special Summon self + 1 from Deck
                if (GetCardCounters(Card) >= 3 && Bot.GetMonsterCount() <= 3)
                {
                    _servantUsed = true;
                    // Prefer Jackal King for monster negate, or Mighty Master
                    AI.SelectCard(CardId.MythicalBeastJackalKing, CardId.EndymionMightyMaster, CardId.MagisterOfEndymion);
                    return true;
                }
            }
            return false;
        }

        private bool MagisterPendulumEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Place as Scale 8
                return Bot.SpellZone[0] == null || Bot.SpellZone[4] == null;
            }

            if (Card.Location == CardLocation.SpellZone)
            {
                if (_magisterUsed) return false;
                // Requires 3 counters on self to Special Summon self + 1 face-up from Extra Deck
                if (GetCardCounters(Card) >= 3 && Bot.GetMonsterCount() <= 3 && Bot.ExtraDeck.Any(c => c.IsFaceup()))
                {
                    _magisterUsed = true;
                    AI.SelectCard(CardId.MythicalBeastJackalKing, CardId.EndymionMightyMaster, CardId.ServantOfEndymion);
                    return true;
                }
            }
            return false;
        }

        private bool ReflectionPendulumEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return Bot.SpellZone[0] == null || Bot.SpellZone[4] == null;
            }

            if (Card.Location == CardLocation.SpellZone)
            {
                if (_reflectionUsed) return false;
                if (GetCardCounters(Card) >= 3 && Bot.GetMonsterCount() <= 3 && Bot.Hand.Any(c => c.IsMonster()))
                {
                    _reflectionUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool MightyMasterBoardBreak()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Place as Scale 8
                return Bot.SpellZone[0] == null || Bot.SpellZone[4] == null;
            }

            if (Card.Location == CardLocation.SpellZone)
            {
                // Strict Board-Break Requirement:
                // Only board-break when opponent actually controls cards to destroy!
                // NEVER activate on Turn 1 or against empty opponent boards!
                if (Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0)
                    return false;

                // Remove 6 counters from field to SS self and pop cards
                int fieldCounters = GetTotalCountersOnField();
                if (fieldCounters >= 6 && Bot.GetMonsterCount() < 5)
                {
                    return true;
                }
            }
            return false;
        }

        // ============================================================================
        // EXTRA DECK COMBOS (ELECTRUMITE, SELENE, ABSOLUTE -> VORTEX)
        // ============================================================================
        private bool ElectrumiteSpSummon()
        {
            if (_electrumiteSendUsed) return false;
            // 2 Pendulum Monsters
            return Bot.GetMonsters().Count(m => m.IsFaceup() && (m.Type & (int)CardType.Pendulum) != 0) >= 2;
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

            // Effect 2: Destroy 1 face-up card to add Astrograph to hand
            if (!_electrumitePopUsed)
            {
                // Pop a card with lowest utility, e.g. used scale or institution
                ClientCard popTarget = Bot.SpellZone.FirstOrDefault(s => s != null && s.IsFaceup() && s.Id == CardId.MythicalInstitution)
                                    ?? Bot.SpellZone.FirstOrDefault(s => s != null && s.IsFaceup() && (s.Id == CardId.ServantOfEndymion && _servantUsed))
                                    ?? Bot.SpellZone.FirstOrDefault(s => s != null && s.IsFaceup() && (s.Id == CardId.MagisterOfEndymion && _magisterUsed))
                                    ?? Bot.SpellZone.FirstOrDefault(s => s != null && s.IsFaceup() && s != Card);

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

        private bool AstrographActivate()
        {
            // Trigger in hand when a card is destroyed (e.g. by Electrumite or Master Cerberus)
            if (Card.Location == CardLocation.Hand)
            {
                return Bot.GetMonsterCount() < 5;
            }
            return false;
        }

        private bool CrowleySpSummon()
        {
            if (_crowleyUsed) return false;
            // 2 Spellcasters
            return Bot.GetMonsters().Count(m => m.IsFaceup() && (m.Race & (int)CardRace.SpellCaster) != 0) >= 2;
        }

        private bool CrowleyActivate()
        {
            _crowleyUsed = true;
            // Reveal 3 Spellbooks
            AI.SelectCard(CardId.SpellbookOfSecrets, CardId.SpellbookOfKnowledge, CardId.SpellbookMagician);
            return true;
        }

        private bool BeyondThePendulumSpSummon()
        {
            // 2 Effect Monsters including a Pendulum Monster
            return Bot.GetMonsters().Count(m => m.IsFaceup()) >= 2 &&
                   Bot.GetMonsters().Any(m => m.IsFaceup() && (m.Type & (int)CardType.Pendulum) != 0);
        }

        private bool BeyondThePendulumActivate()
        {
            // Pay 1200 LP to search a Pendulum monster
            AI.SelectCard(CardId.EndymionMightyMaster, CardId.ServantOfEndymion, CardId.MagisterOfEndymion);
            return true;
        }

        private bool ExecutePendulumSummon()
        {
            // Perform Pendulum Summon when scales are set
            return true;
        }

        private bool AbsoluteDragonSpSummon()
        {
            if (_absoluteUsed) return false;
            // 2 Level 7 monsters (Mighty Master, Astrograph, Reflection)
            var lv7s = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 7).ToList();
            return lv7s.Count >= 2;
        }

        private bool GravityControllerSpSummon()
        {
            // Link 1 using Odd-Eyes Absolute Dragon in Extra Monster Zone!
            ClientCard emzMonster = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.Id == CardId.OddEyesAbsoluteDragon && (m.Sequence == 5 || m.Sequence == 6));
            return emzMonster != null;
        }

        private bool AbsoluteDragonGYEffect()
        {
            // When sent to GY: Special Summon Odd-Eyes Vortex Dragon!
            _absoluteUsed = true;
            AI.SelectCard(CardId.OddEyesVortexDragon);
            return true;
        }

        private bool SeleneSpSummon()
        {
            if (_seleneUsed) return false;
            // Link-3: 2+ monsters including a Spellcaster
            int spellcasters = Bot.GetMonsters().Count(m => m.IsFaceup() && (m.Race & (int)CardRace.SpellCaster) != 0);
            return spellcasters >= 1 && Bot.GetMonsterCount() >= 3;
        }

        private bool SeleneActivate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            // On summon: gains counters equal to Spells on field and in both GYs
            if (GetCardCounters(Card) == 0)
            {
                int totalSpells = Bot.Graveyard.Count(c => c.IsSpell()) + Enemy.Graveyard.Count(c => c.IsSpell()) + Bot.SpellZone.Count(s => s != null) + Enemy.SpellZone.Count(s => s != null);
                SetCardCounters(Card, totalSpells);
                return true;
            }

            // Ignition / Quick Effect: Remove 3 counters to revive Spellcaster from hand/GY
            if (!_seleneUsed && CanSafelySpendCounters(3) && Bot.GetMonsterCount() < 5)
            {
                _seleneUsed = true;
                AI.SelectCard(CardId.EndymionMightyMaster, CardId.MythicalBeastJackalKing, CardId.ReflectionOfEndymion);
                return true;
            }
            return false;
        }

        private bool ApollousaSpSummon()
        {
            // 2+ monsters with different names (except Tokens)
            var candidates = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Id != CardId.MythicalBeastJackalKing && m.Id != CardId.EndymionMightyMaster).ToList();
            return candidates.Count >= 3;
        }

        private bool AccesscodeSpSummon()
        {
            // 2+ Effect Monsters for lethal / OTK
            return Duel.Turn > 1 && Bot.GetMonsterCount() >= 3;
        }

        private bool AccesscodeActivate()
        {
            // Target Link-3/4 for ATK boost or pop cards
            return true;
        }

        private bool SPLittleKnightSpSummon()
        {
            return Duel.Phase == DuelPhase.Main2 && Bot.GetMonsterCount() >= 2;
        }

        private bool SPLittleKnightActivate()
        {
            ClientCard oppTarget = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault()
                                ?? Enemy.GetSpells().FirstOrDefault();
            if (oppTarget != null)
            {
                AI.SelectCard(oppTarget);
                return true;
            }
            return false;
        }

        // ============================================================================
        // MONSTER ZONE IGNITIONS
        // ============================================================================
        private bool MasterCerberusMonsterBanish()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_masterCerberusMZoneUsed) return false;

            // Remove 4 counters to banish 1 opponent monster
            if (CanSafelySpendCounters(4))
            {
                ClientCard oppTarget = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault();
                if (oppTarget != null)
                {
                    _masterCerberusMZoneUsed = true;
                    AI.SelectCard(oppTarget);
                    return true;
                }
            }
            return false;
        }

        private bool ReflectionMonsterBounce()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            ClientCard oppTarget = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault()
                                ?? Enemy.GetSpells().FirstOrDefault();
            if (oppTarget != null)
            {
                AI.SelectCard(oppTarget);
                return true;
            }
            return false;
        }

        private bool JackalMonsterEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (CanSafelySpendCounters(3))
            {
                AI.SelectCard(CardId.MythicalBeastJackalKing);
                return true;
            }
            return false;
        }

        private bool FallbackSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool FallbackNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        public bool SmartMonsterRepos()
        {
            if (Card == null) return false;
            // Link monsters can NEVER be placed in Defense Position
            if (Card.HasType(CardType.Link)) return false;

            // 1. 0 ATK monsters or Handtraps (e.g. 0/1800 Ghost Girls) in Attack position -> ALWAYS switch to Defense!
            if (Card.IsAttack() && (Card.Attack == 0 || CardIntelligence.IsHandtrap(Card.Id) || CardIntelligence.IsHandtrap(Card.GetNonAltartCode())))
                return true;

            // 2. High DEF / Low ATK monsters (DEF > ATK and ATK < 1800, e.g. Servant 900/1500) in Attack position -> switch to Defense
            if (Card.IsAttack() && Card.Defense > Card.Attack && Card.Attack < 1800)
            {
                if (Duel.Phase == DuelPhase.Main1 && ShouldRushAttack) return false;
                return true;
            }

            // 3. High ATK monsters in Defense position -> switch to Attack to push battle damage
            if (Card.IsDefense() && Card.Attack >= 1800 && Card.Attack >= Card.Defense)
            {
                if (!Util.IsAllEnemyBetter(true))
                    return true;
            }

            return DefaultMonsterRepos();
        }

        // ============================================================================
        // INTELLIGENT COUNTER & TARGET SELECTION
        // ============================================================================
        public override IList<int> OnSelectCounter(int type, int quantity, IList<ClientCard> cards, IList<int> counters)
        {
            return Plugin?.CounterEconomy?.SelectCounters(quantity, cards, counters)
                ?? base.OnSelectCounter(type, quantity, cards, counters);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards != null && cards.Count > 0)
            {
                // Deck search (hint 506 = HINTMSG_ATOHAND or candidates from Deck)
                if (hint == HINT_ATOHAND || cards.All(c => c.Location == CardLocation.Deck))
                {
                    var preferred = cards.Where(c =>
                        c.Id == CardId.ServantOfEndymion ||
                        c.Id == CardId.EndymionMightyMaster ||
                        c.Id == CardId.MythicalBeastJackalKing ||
                        c.Id == CardId.MagisterOfEndymion ||
                        c.Id == CardId.SpellPowerMastery
                    ).ToList();

                    if (preferred.Count >= min)
                    {
                        return preferred.Take(max).ToList();
                    }
                }

                // Special Summon targets (hint 509 = HINTMSG_SPSUMMON)
                if (hint == HINT_SPSUMMON)
                {
                    var preferredSS = cards.Where(c =>
                        c.Id == CardId.MythicalBeastJackalKing ||
                        c.Id == CardId.EndymionMightyMaster ||
                        c.Id == CardId.OddEyesVortexDragon ||
                        c.Id == CardId.AstrographSorcerer
                    ).ToList();

                    if (preferredSS.Count >= min)
                    {
                        return preferredSS.Take(max).ToList();
                    }
                }

                // Removals (hint 503 [REMOVE], hint 502 [DESTROY], hint 504 [TOGRAVE], hint 505 [RTOHAND]):
                // Target Enemy cards first!
                if (hint == HINT_REMOVE || hint == HINT_DESTROY || hint == HINT_TOGRAVE || hint == HINT_RTOHAND)
                {
                    var enemyTargets = cards.Where(c => c.Controller == 1).ToList();
                    if (enemyTargets.Count >= min)
                    {
                        return enemyTargets.OrderByDescending(c => c.Attack).Take(max).ToList();
                    }
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (positions == null || positions.Count == 0) return CardPosition.FaceUpAttack;
            if (positions.Count == 1) return positions[0];

            var cardData = YGOSharp.OCGWrapper.NamedCard.Get(cardId);
            if (cardData != null)
            {
                // Link monsters can NEVER be placed in Defense
                if (cardData.HasType(CardType.Link))
                    return CardPosition.FaceUpAttack;

                // 1. Handtraps (0/1800, Veiler 0/0) & 0 ATK monsters: ALWAYS DEFENSE!
                if (cardData.Attack == 0 || CardIntelligence.IsHandtrap(cardId))
                {
                    if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                    if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                }

                // 2. High DEF / Low ATK (DEF > ATK && ATK < 1800, e.g. Servant 900/1500) -> DEFENSE
                if (cardData.Defense > cardData.Attack && cardData.Attack < 1800)
                {
                    if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                    if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                }

                // 3. Boss / High ATK (ATK >= 1800) -> ATTACK
                if (cardData.Attack >= 1800 && positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  MASTER DECK PLUGIN: EndymionPlugin (Layer 3 Domain Helpers)
    //  Decouples Domain Rules, Strategy, and Scorer from Engine Core
    // ═══════════════════════════════════════════════════════════════
    internal class EndymionPlugin
    {
        private readonly _2026_EndymionExecutor _exec;

        public EndymionStrategy Strategy { get; }
        public EndymionCounterEconomy CounterEconomy { get; }
        public EndymionScaleResolver ScaleResolver { get; }
        public EndymionMaterialScorer MaterialScorer { get; }
        public EndymionActionScorer ActionScorer { get; }
        public EndymionBoardAssessor BoardAssessor { get; }

        public EndymionPlugin(_2026_EndymionExecutor exec)
        {
            _exec = exec;
            Strategy = new EndymionStrategy(exec);
            CounterEconomy = new EndymionCounterEconomy(exec);
            ScaleResolver = new EndymionScaleResolver(exec);
            MaterialScorer = new EndymionMaterialScorer(exec);
            ActionScorer = new EndymionActionScorer(exec, this);
            BoardAssessor = new EndymionBoardAssessor(exec);
        }

        public void ResetTurnState()
        {
            Strategy.Reset();
            CounterEconomy.Reset();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 1: EndymionStrategy
    // ═══════════════════════════════════════════════════════════════
    internal class EndymionStrategy
    {
        private readonly _2026_EndymionExecutor _exec;

        public bool ServantUsed { get; set; }
        public bool MagisterUsed { get; set; }
        public bool ReflectionUsed { get; set; }
        public bool MightyMasterUsed { get; set; }
        public bool MasterCerberusPZoneUsed { get; set; }
        public bool MasterCerberusMZoneUsed { get; set; }
        public bool JackalKingNegateUsed { get; set; }
        public bool ElectrumiteSendUsed { get; set; }
        public bool ElectrumitePopUsed { get; set; }
        public bool SeleneUsed { get; set; }
        public bool SpellPowerMasteryUsed { get; set; }
        public bool KnowledgeUsed { get; set; }
        public bool SecretsUsed { get; set; }
        public bool AbsoluteUsed { get; set; }
        public bool CrowleyUsed { get; set; }
        public bool GarudaUsed { get; set; }

        public EndymionStrategy(_2026_EndymionExecutor exec) => _exec = exec;

        public void Reset()
        {
            ServantUsed = false;
            MagisterUsed = false;
            ReflectionUsed = false;
            MightyMasterUsed = false;
            MasterCerberusPZoneUsed = false;
            MasterCerberusMZoneUsed = false;
            JackalKingNegateUsed = false;
            ElectrumiteSendUsed = false;
            ElectrumitePopUsed = false;
            SeleneUsed = false;
            SpellPowerMasteryUsed = false;
            KnowledgeUsed = false;
            SecretsUsed = false;
            AbsoluteUsed = false;
            CrowleyUsed = false;
            GarudaUsed = false;
        }

        public bool ShouldSummonElectrumite()
        {
            if (_exec.Bot.GetMonsters().Any(m => m.Id == _2026_EndymionExecutor.CardId.HeavymetalfoesElectrumite)) return false;
            var pendMonsters = _exec.Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && m.HasType(CardType.Pendulum) && !_exec.IsAceCard(m)).ToList();
            return pendMonsters.Count >= 2;
        }

        public bool ShouldSummonSelene()
        {
            if (_exec.Bot.GetMonsters().Any(m => m.Id == _2026_EndymionExecutor.CardId.SeleneQueenOfTheMasterMagicians)) return false;
            var spellcasters = _exec.Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && m.HasRace(CardRace.SpellCaster)).ToList();
            return spellcasters.Count >= 2;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 2: EndymionCounterEconomy
    // ═══════════════════════════════════════════════════════════════
    internal class EndymionCounterEconomy
    {
        private readonly _2026_EndymionExecutor _exec;
        private readonly Dictionary<ClientCard, int> _trackedCounters = new Dictionary<ClientCard, int>();

        public EndymionCounterEconomy(_2026_EndymionExecutor exec) => _exec = exec;

        public void Reset()
        {
            var validCards = new HashSet<ClientCard>(_exec.Bot.GetMonsters().Concat(_exec.Bot.GetSpells()));
            var keysToRemove = _trackedCounters.Keys.Where(k => !validCards.Contains(k)).ToList();
            foreach (var k in keysToRemove)
                _trackedCounters.Remove(k);
        }

        public int GetCounters(ClientCard card)
        {
            if (card == null) return 0;
            return _trackedCounters.TryGetValue(card, out int count) ? count : 0;
        }

        public void SetCounters(ClientCard card, int count)
        {
            if (card != null) _trackedCounters[card] = Math.Max(0, count);
        }

        public void AddCounters(ClientCard card, int count)
        {
            if (card != null) _trackedCounters[card] = GetCounters(card) + count;
        }

        public int GetTotalCountersOnField()
        {
            return _exec.Bot.GetMonsters().Concat(_exec.Bot.GetSpells())
                .Where(c => c != null && c.IsFaceup())
                .Sum(c => GetCounters(c));
        }

        public _2026_EndymionExecutor.CounterLevel GetCounterLevel()
        {
            int total = GetTotalCountersOnField();
            if (total <= 1) return _2026_EndymionExecutor.CounterLevel.Critical;
            if (total <= 3) return _2026_EndymionExecutor.CounterLevel.Low;
            if (total <= 5) return _2026_EndymionExecutor.CounterLevel.Ready;
            if (total <= 7) return _2026_EndymionExecutor.CounterLevel.ComboReady;
            return _2026_EndymionExecutor.CounterLevel.Surplus;
        }

        public bool CanSafelySpendCounters(int cost)
        {
            int total = GetTotalCountersOnField();
            if (total < cost) return false;
            // Jackal King reserve budget: Always preserve 2 counters if opponent has monsters to negate
            bool hasJackal = _exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Id == _2026_EndymionExecutor.CardId.MythicalBeastJackalKing);
            int reserve = hasJackal ? 2 : 0;
            return (total - cost) >= reserve;
        }

        public IList<int> SelectCounters(int quantity, IList<ClientCard> cards, IList<int> counters)
        {
            if (cards == null || counters == null || cards.Count != counters.Count)
                return null;

            int[] used = new int[counters.Count];
            int needed = quantity;

            // Sync tracked counters with exact OCGCore engine data
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i] != null)
                    SetCounters(cards[i], counters[i]);
            }

            // Priority 1: Magical Citadel of Endymion (Global replacement fuel)
            for (int i = 0; i < cards.Count && needed > 0; i++)
            {
                if (cards[i] != null && cards[i].Id == _2026_EndymionExecutor.CardId.MagicalCitadel)
                {
                    int take = Math.Min(counters[i], needed);
                    used[i] += take;
                    needed -= take;
                    DecisionTracer.Trace("EndymionCounterEconomy", $"Spent {take} counters from Magical Citadel");
                }
            }

            // Priority 2: Mythical Institution & Surplus continuous cards
            for (int i = 0; i < cards.Count && needed > 0; i++)
            {
                if (cards[i] != null && cards[i].Id == _2026_EndymionExecutor.CardId.MythicalInstitution)
                {
                    int take = Math.Min(counters[i] - used[i], needed);
                    used[i] += take;
                    needed -= take;
                    DecisionTracer.Trace("EndymionCounterEconomy", $"Spent {take} counters from Mythical Institution");
                }
            }

            // Priority 3: Non-negator monsters & cards with surplus counters (> 2 on Jackal King)
            for (int i = 0; i < cards.Count && needed > 0; i++)
            {
                if (cards[i] != null && 
                    cards[i].Id != _2026_EndymionExecutor.CardId.ServantOfEndymion && 
                    cards[i].Id != _2026_EndymionExecutor.CardId.MagisterOfEndymion)
                {
                    int avail = counters[i] - used[i];
                    // If Jackal King, preserve at least 2 counters for monster negate if possible
                    if (cards[i].Id == _2026_EndymionExecutor.CardId.MythicalBeastJackalKing)
                    {
                        avail = Math.Max(0, avail - 2);
                    }
                    if (avail > 0)
                    {
                        int take = Math.Min(avail, needed);
                        used[i] += take;
                        needed -= take;
                        DecisionTracer.Trace("EndymionCounterEconomy", $"Spent {take} surplus counters from {cards[i].Name ?? cards[i].Id.ToString()}");
                    }
                }
            }

            // Priority 4: Safe fallback across ANY remaining counters to strictly guarantee sum(used) == quantity
            for (int i = 0; i < cards.Count && needed > 0; i++)
            {
                int avail = counters[i] - used[i];
                if (avail > 0)
                {
                    int take = Math.Min(avail, needed);
                    used[i] += take;
                    needed -= take;
                    DecisionTracer.Trace("EndymionCounterEconomy", $"Spent {take} fallback counters from {cards[i]?.Name ?? cards[i]?.Id.ToString()}");
                }
            }

            // Update remaining internal counters
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i] != null)
                    SetCounters(cards[i], Math.Max(0, counters[i] - used[i]));
            }

            return used;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 3: EndymionScaleResolver
    // ═══════════════════════════════════════════════════════════════
    internal class EndymionScaleResolver
    {
        private readonly _2026_EndymionExecutor _exec;

        public EndymionScaleResolver(_2026_EndymionExecutor exec) => _exec = exec;

        public ClientCard PickLowScale()
        {
            // Low scale: Scale 2 (Servant, Reflection)
            return _exec.Bot.Hand.FirstOrDefault(c => 
                c.Id == _2026_EndymionExecutor.CardId.ServantOfEndymion || 
                c.Id == _2026_EndymionExecutor.CardId.ReflectionOfEndymion);
        }

        public ClientCard PickHighScale()
        {
            // High scale: Scale 8 (Magister, Mighty Master)
            return _exec.Bot.Hand.FirstOrDefault(c => 
                c.Id == _2026_EndymionExecutor.CardId.MagisterOfEndymion || 
                c.Id == _2026_EndymionExecutor.CardId.EndymionMightyMaster);
        }

        public ClientCard PickElectrumitePopTarget()
        {
            // Prioritize popping continuous cards that triggered or low-value scales to trigger Astrograph
            var target = _exec.Bot.GetSpells().FirstOrDefault(s => s != null && s.IsFaceup() && 
                (s.Id == _2026_EndymionExecutor.CardId.MagisterOfEndymion || 
                 s.Id == _2026_EndymionExecutor.CardId.MythicalInstitution));
            if (target != null) return target;

            return _exec.Bot.GetSpells().FirstOrDefault(s => s != null && s.IsFaceup() && 
                s.Id != _2026_EndymionExecutor.CardId.MagicalCitadel);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 4: EndymionMaterialScorer
    // ═══════════════════════════════════════════════════════════════
    internal class EndymionMaterialScorer
    {
        private readonly _2026_EndymionExecutor _exec;

        public EndymionMaterialScorer(_2026_EndymionExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 0;

            // Absolute Protection: End Board Ace Bosses must never be linked away casually
            if (card.Id == _2026_EndymionExecutor.CardId.EndymionMightyMaster) return 10000;
            if (card.Id == _2026_EndymionExecutor.CardId.MythicalBeastJackalKing) return 9500;
            if (card.Id == _2026_EndymionExecutor.CardId.OddEyesVortexDragon) return 9000;
            if (card.Id == _2026_EndymionExecutor.CardId.ApollousaBowOfTheGoddess) return 8500;
            if (card.Id == _2026_EndymionExecutor.CardId.AccesscodeTalker) return 8000;
            if (card.Id == _2026_EndymionExecutor.CardId.SeleneQueenOfTheMasterMagicians) return 7500;

            // Fodder Ranking: Spent low-stat monsters
            if (card.Id == _2026_EndymionExecutor.CardId.SpellbookMagician) return 10;
            if (card.Id == _2026_EndymionExecutor.CardId.ReflectionOfEndymion) return 20;
            if (card.Id == _2026_EndymionExecutor.CardId.MagisterOfEndymion) return 25;
            if (card.Id == _2026_EndymionExecutor.CardId.ServantOfEndymion) return 30;

            return 100;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates)
        {
            return candidates.OrderBy(c => GetMaterialCost(c)).ToList();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 5: EndymionActionScorer
    // ═══════════════════════════════════════════════════════════════
    internal class EndymionActionScorer
    {
        private readonly _2026_EndymionExecutor _exec;
        private readonly EndymionPlugin _plugin;

        public EndymionActionScorer(_2026_EndymionExecutor exec, EndymionPlugin plugin)
        {
            _exec = exec;
            _plugin = plugin;
        }

        public bool ShouldActivateMightyMasterWipe()
        {
            // Evaluate Mighty Master 6-counter wipe:
            // Score = Opponent Monster Count * 30 + Opponent Spell Count * 20 - 60 (Cost)
            int enemyCards = _exec.Enemy.GetMonsterCount() + _exec.Enemy.GetSpellCount();
            if (enemyCards < 2) return false;
            return _plugin.CounterEconomy.GetTotalCountersOnField() >= 6;
        }

        public bool ShouldActivateServantSummon()
        {
            // Servant 3 counters SS is highest priority combo starter
            return true;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 6: EndymionBoardAssessor
    // ═══════════════════════════════════════════════════════════════
    internal class EndymionBoardAssessor
    {
        private readonly _2026_EndymionExecutor _exec;

        public EndymionBoardAssessor(_2026_EndymionExecutor exec) => _exec = exec;

        public bool IsLethalSecured()
        {
            int botAtk = _exec.Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).Sum(m => m.Attack);
            return botAtk >= _exec.Enemy.LifePoints && _exec.Enemy.GetMonsterCount() == 0;
        }

        public bool IsOpponentBackrowDangerous()
        {
            return _exec.Enemy.GetSpellCount() >= 2;
        }
    }
}
