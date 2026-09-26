// ============================================================================
// DrytronTourExecutor.cs — Drytron Machine Ritual & Rank 1 Xyz Engine
// Archetype: Drytron Meteonis DA Draconids / Mu Beta Fafnir / Lyrilusc / ZEUS
// Standard: Decoupled Domain Plugin Architecture (Layer 3 in SKILL.md)
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
    [Deck("DrytronTour")]
    public class DrytronTourExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Ritual Monsters
            public const int DrytronMeteonisDADraconids = 56863746;

            // Main Deck Drytrons
            public const int DrytronDeltaAltais = 22420202;
            public const int DrytronGammaEltanin = 60037599;
            public const int DrytronZetaAldhibah = 96026108;
            public const int DrytronAlphaThuban = 97148796;
            public const int DrytronBetaRastaban = 33543890;
            public const int DrytronNuII = 22435424;
            public const int UltimateBrightKnightUrsatronAlpha = 14959144;

            // Other Monsters
            public const int GadgetGamer = 64487132;
            public const int DDCrow = 24508238;

            // Spells
            public const int DrytronNova = 94187078;
            public const int MeteonisDrytron = 22398665;
            public const int DrytronFafnir = 58793369;
            public const int CyberEmergency = 60600126;
            public const int GordianSlicer = 66236707;
            public const int JackInTheHand = 51697825;
            public const int DarkRulerNoMore = 54693926;
            public const int PairBearScare = 21501961;

            // Extra Deck
            public const int DrytronMuBetaFafnir = 1174075;
            public const int GalaxyEyesAntimatterDragon = 92517928;
            public const int GalaxyEyesFullArmorPhotonDragon = 39030163;
            public const int NumberF0UtopicFuture = 65305468;
            public const int NumberF0UtopicFutureZexal = 41522092;
            public const int FullArmoredUtopicRayLancer = 1269512;
            public const int CherubidamnIrisfiel = 64626565;
            public const int LyriluscEnsemblueRobin = 72971064;
            public const int LyriluscPromenadeThrush = 19369609;
            public const int LyriluscRecitalStarling = 8491961;
            public const int LyriluscAssembledNightingale = 48608796;
            public const int SylvanPrincessprite = 33909817;
            public const int KikinagashiFucho = 27240101;
            public const int DownerdMagician = 72167543;
            public const int DivineArsenalAAZEUSSkyThunder = 90448279;
        }

        // Domain Plugin Coordinator (Layer 3)
        internal DrytronTourPlugin Plugin { get; private set; }

        public ClientCard CurrentLastChainCard => LastChainCard;

        public DrytronTourExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            Plugin = new DrytronTourPlugin(this);

            // Register Ace Cards
            ResourcePlan.RegisterAceCards(
                CardId.DrytronMeteonisDADraconids,
                CardId.DrytronMuBetaFafnir,
                CardId.LyriluscEnsemblueRobin,
                CardId.DivineArsenalAAZEUSSkyThunder
            );

            BaitPlanner.RegisterComboStarters(
                CardId.DrytronFafnir,
                CardId.CyberEmergency,
                CardId.DrytronNova,
                CardId.JackInTheHand,
                CardId.GadgetGamer
            );

            ChainAdvisor.RegisterHighValueTargets(
                CardId.DrytronMeteonisDADraconids,
                CardId.DrytronMuBetaFafnir,
                CardId.MeteonisDrytron
            );

            // ═══════════════════════════════════════════════════════════════
            //  EXECUTOR PIPELINE
            // ═══════════════════════════════════════════════════════════════

            // ── Tier 0: Quick Negations & Handtraps ──
            AddExecutor(ExecutorType.Activate, CardId.DrytronMeteonisDADraconids, DADraconidsNegate);
            AddExecutor(ExecutorType.Activate, CardId.DrytronMuBetaFafnir, MuBetaNegate);
            AddExecutor(ExecutorType.Activate, CardId.LyriluscEnsemblueRobin, RobinBounce);
            AddExecutor(ExecutorType.Activate, CardId.DDCrow, DDCrowActivate);

            // ── Tier 1: Board Breakers (Going 2nd) ──
            AddExecutor(ExecutorType.Activate, CardId.DarkRulerNoMore, DarkRulerNoMoreActivate);
            AddExecutor(ExecutorType.Activate, CardId.GordianSlicer, GordianSlicerActivate);
            AddExecutor(ExecutorType.Activate, CardId.DivineArsenalAAZEUSSkyThunder, ZeusActivate);

            // ── Tier 2: Search & Engine Starters ──
            AddExecutor(ExecutorType.Activate, CardId.DrytronFafnir, DrytronFafnirActivate);
            AddExecutor(ExecutorType.Activate, CardId.CyberEmergency, CyberEmergencyActivate);
            AddExecutor(ExecutorType.Activate, CardId.DrytronNova, DrytronNovaActivate);
            AddExecutor(ExecutorType.Activate, CardId.JackInTheHand, JackInTheHandActivate);
            AddExecutor(ExecutorType.Summon, CardId.GadgetGamer, GadgetGamerSummon);
            AddExecutor(ExecutorType.Activate, CardId.GadgetGamer, GadgetGamerActivate);

            // ── Tier 3: Drytron Main Deck Ignition Extenders ──
            AddExecutor(ExecutorType.Activate, CardId.DrytronAlphaThuban, DrytronAlphaActivate);
            AddExecutor(ExecutorType.Activate, CardId.DrytronZetaAldhibah, DrytronZetaActivate);
            AddExecutor(ExecutorType.Activate, CardId.DrytronNuII, DrytronNuActivate);
            AddExecutor(ExecutorType.Activate, CardId.DrytronGammaEltanin, DrytronGammaActivate);
            AddExecutor(ExecutorType.Activate, CardId.DrytronDeltaAltais, DrytronDeltaActivate);
            AddExecutor(ExecutorType.Activate, CardId.DrytronBetaRastaban, DrytronBetaActivate);

            // ── Tier 4: Extra Deck Climbs (Mu Beta Fafnir & Rank 1s) ──
            AddExecutor(ExecutorType.SpSummon, CardId.DrytronMuBetaFafnir, MuBetaSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.DrytronMuBetaFafnir, MuBetaMill);

            // ── Tier 5: Ritual Summon Execution ──
            AddExecutor(ExecutorType.Activate, CardId.MeteonisDrytron, MeteonisDrytronActivate);

            // ── Tier 6: Secondary Xyz Monsters & AA-ZEUS Chain ──
            AddExecutor(ExecutorType.SpSummon, CardId.LyriluscEnsemblueRobin, RobinSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.LyriluscAssembledNightingale, NightingaleSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.LyriluscAssembledNightingale, NightingaleActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.DownerdMagician, DownerdSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DivineArsenalAAZEUSSkyThunder, ZeusSpSummon);

            // ── Tier 7: Desperation MonsterSet ──
            AddExecutor(ExecutorType.MonsterSet, DesperationMonsterSet);

            // ── Tier 8: Smart Monster Repositioning ──
            AddExecutor(ExecutorType.Repos, SmartMonsterRepos);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            Plugin.ResetTurnState();
        }

        public override bool OnSelectHand()
        {
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  ACTIVATION & COMBOS
        // ═══════════════════════════════════════════════════════════════

        private bool DADraconidsNegate()
        {
            ClientCard last = LastChainCard;
            if (last == null || last.Controller != 1 || !last.IsMonster()) return false;
            if (last.IsDisabled()) return false;
            // Requires Drytron in GY with total ATK >= opponent monster original ATK
            int targetAtk = last.Attack;
            var gyDrytrons = Bot.Graveyard.Where(c => c.HasSetcode(0x154) && c.IsMonster()).ToList();
            return gyDrytrons.Sum(c => c.Attack) >= targetAtk;
        }

        private bool MuBetaNegate()
        {
            // Negates Spell/Trap card or effect activation by detaching 1 material while controlling Machine Ritual
            ClientCard last = LastChainCard;
            if (last == null || last.Controller != 1 || last.IsMonster()) return false;
            if (last.IsDisabled()) return false;
            bool hasRitual = Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasType(CardType.Ritual) && m.HasRace(CardRace.Machine));
            return hasRitual && Card.Overlays.Count > 0;
        }

        private bool RobinBounce()
        {
            ClientCard last = LastChainCard;
            if (last != null && last.Controller == 1 && last.IsMonster()) return true;
            return Card.Overlays.Count > 0 && Enemy.GetMonsters().Any(m => m.IsFaceup() && m.Attack >= 1800);
        }

        private bool DDCrowActivate()
        {
            var gyTargets = Enemy.Graveyard.Where(c => CardIntelligence.IsHighThreatChokepoint(c.Id) || c.IsMonster()).ToList();
            return gyTargets.Count > 0;
        }

        private bool DarkRulerNoMoreActivate()
        {
            return Enemy.GetMonsters().Any(m => m.IsFaceup() && (CardIntelligence.IsKnownNegator(m.Id) || m.Attack >= 2500));
        }

        private bool GordianSlicerActivate()
        {
            return (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2) && Enemy.GetMonsterCount() > 0;
        }

        private bool ZeusActivate()
        {
            // Quick Effect: detach 2 to send all other cards on field to GY
            if (Card.Overlays.Count < 2) return false;
            return Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 2;
        }

        private bool DrytronFafnirActivate()
        {
            return true;
        }

        private bool CyberEmergencyActivate()
        {
            return true;
        }

        private bool DrytronNovaActivate()
        {
            return true;
        }

        private bool JackInTheHandActivate()
        {
            return true;
        }

        private bool GadgetGamerSummon()
        {
            return true;
        }

        private bool GadgetGamerActivate()
        {
            return true;
        }

        private bool DrytronAlphaActivate()
        {
            if (Plugin.AlphaUsed) return false;
            return Plugin.TributeManager.CanTributeForDrytron(Card);
        }

        private bool DrytronZetaActivate()
        {
            if (Plugin.ZetaUsed) return false;
            return Plugin.TributeManager.CanTributeForDrytron(Card);
        }

        private bool DrytronNuActivate()
        {
            if (Plugin.NuUsed) return false;
            return Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasSetcode(0x154));
        }

        private bool DrytronGammaActivate()
        {
            if (Plugin.GammaUsed) return false;
            bool hasGyDrytron = Bot.Graveyard.Any(c => c.HasSetcode(0x154) && c.IsMonster() && c.Attack == 2000);
            return hasGyDrytron && Plugin.TributeManager.CanTributeForDrytron(Card);
        }

        private bool DrytronDeltaActivate()
        {
            if (Plugin.DeltaUsed) return false;
            return Plugin.TributeManager.CanTributeForDrytron(Card);
        }

        private bool DrytronBetaActivate()
        {
            if (Plugin.BetaUsed) return false;
            return Bot.Banished.Any(c => c.HasSetcode(0x154)) && Plugin.TributeManager.CanTributeForDrytron(Card);
        }

        private bool MuBetaSpSummon()
        {
            if (Bot.GetMonsters().Any(m => m.Id == CardId.DrytronMuBetaFafnir)) return false;
            var lv1s = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 1 && !IsAceCard(m)).ToList();
            return lv1s.Count >= 2;
        }

        private bool MuBetaMill()
        {
            // Mill Zeta if need Ritual Spell, else Alpha, else Gamma
            return true;
        }

        private bool MeteonisDrytronActivate()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // GY effect: target Drytron on field, reduce ATK by 1000, add to hand
                return Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasSetcode(0x154) && m.Attack >= 1000) &&
                       !Bot.HasInHand(CardId.MeteonisDrytron);
            }

            // Hand activation: Ritual Summon
            return Plugin.RitualAdvisor.CanRitualSummon();
        }

        private bool RobinSpSummon()
        {
            if (Bot.GetMonsters().Any(m => m.Id == CardId.LyriluscEnsemblueRobin)) return false;
            var lv1s = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 1 && !IsAceCard(m)).ToList();
            return lv1s.Count >= 2;
        }

        private bool NightingaleSpSummon()
        {
            if (Duel.Turn <= 1) return false;
            var lv1s = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 1 && !IsAceCard(m)).ToList();
            return lv1s.Count >= 2;
        }

        private bool NightingaleActivate()
        {
            return true;
        }

        private bool DownerdSpSummon()
        {
            // Overlay onto Nightingale in MP2
            if (Duel.Phase != DuelPhase.Main2) return false;
            return Bot.GetMonsters().Any(m => m.IsFaceup() && m.Id == CardId.LyriluscAssembledNightingale);
        }

        private bool ZeusSpSummon()
        {
            if (Duel.Phase != DuelPhase.Main2) return false;
            return Bot.GetMonsters().Any(m => m.IsFaceup() && (m.Id == CardId.DownerdMagician || m.Id == CardId.LyriluscAssembledNightingale));
        }

        private bool DesperationMonsterSet()
        {
            if (Bot.GetMonsterCount() > 0) return false;
            if (Duel.Turn <= 1) return false;
            return Enemy.GetMonsters().Any(m => m.IsFaceup() && m.IsAttack() && m.Attack >= 1500);
        }

        private bool SmartMonsterRepos()
        {
            if (Card == null) return false;
            if (Card.IsAttack() && (Card.Attack == 0 || (Card.Defense > Card.Attack && Card.Defense >= 1800)))
                return true;
            if (Card.IsDefense() && Card.Attack > Card.Defense && Card.Attack >= 1800 && Duel.Turn > 1)
                return true;
            return DefaultMonsterRepos();
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (positions == null || positions.Count == 0) return CardPosition.FaceUpAttack;
            if (positions.Count == 1) return positions[0];

            var cardData = YGOSharp.OCGWrapper.NamedCard.Get(cardId);
            if (cardData != null)
            {
                if (cardData.HasType(CardType.Link))
                    return CardPosition.FaceUpAttack;

                // 1. Handtraps (0/1800, Veiler 0/0) or 0 ATK -> 100% Defense
                if (cardData.Attack == 0 || CardIntelligence.IsHandtrap(cardId))
                {
                    if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                    if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                }

                // 2. High DEF / Wall (DEF > ATK and ATK < 1800) -> Defense
                if (cardData.Defense > cardData.Attack && cardData.Attack < 1800)
                {
                    if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                    if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                }

                // 3. Boss / High ATK (ATK >= 1800) -> Attack
                if (cardData.Attack >= 1800 && positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // Drytron tribute selection (hint 500 = HINTMSG_RELEASE): prioritize hand cards over on-field boss
            if (hint == 500)
            {
                var handTributes = cards.Where(c => c.Location == CardLocation.Hand && !IsAceCard(c)).ToList();
                if (handTributes.Count >= min) return handTributes.Take(max).ToList();

                var fieldTributes = cards.Where(c => c.Location == CardLocation.MonsterZone && !IsAceCard(c)).ToList();
                if (fieldTributes.Count >= min) return fieldTributes.Take(max).ToList();
            }

            // Search priority (hint 506 = HINTMSG_ATOHAND)
            if (hint == 506 && min <= 1 && 1 <= max)
            {
                // If don't have Ritual Spell -> Meteonis Drytron
                if (!Bot.HasInHand(CardId.MeteonisDrytron) && !Bot.HasInGraveyard(CardId.MeteonisDrytron))
                {
                    var meteonis = cards.FirstOrDefault(c => c.Id == CardId.MeteonisDrytron);
                    if (meteonis != null) return new List<ClientCard> { meteonis };
                }

                // If don't have Ritual Boss -> DA Draconids
                if (!Bot.HasInHand(CardId.DrytronMeteonisDADraconids))
                {
                    var draconids = cards.FirstOrDefault(c => c.Id == CardId.DrytronMeteonisDADraconids);
                    if (draconids != null) return new List<ClientCard> { draconids };
                }

                // Drytron Alpha > Zeta > Nu II > Gamma
                var alpha = cards.FirstOrDefault(c => c.Id == CardId.DrytronAlphaThuban);
                if (alpha != null && (Plugin?.AlphaUsed != true)) return new List<ClientCard> { alpha };

                var zeta = cards.FirstOrDefault(c => c.Id == CardId.DrytronZetaAldhibah);
                if (zeta != null && (Plugin?.ZetaUsed != true)) return new List<ClientCard> { zeta };

                var nu = cards.FirstOrDefault(c => c.Id == CardId.DrytronNuII);
                if (nu != null && (Plugin?.NuUsed != true)) return new List<ClientCard> { nu };
            }

            // Send to GY (hint 504 = HINTMSG_TOGRAVE, e.g. Mu Beta Fafnir mill)
            if (hint == 504 && min <= 1 && 1 <= max)
            {
                // Mill Zeta if missing ritual spell, Alpha if missing boss, Nu II if missing extender
                var millZeta = cards.FirstOrDefault(c => c.Id == CardId.DrytronZetaAldhibah);
                if (millZeta != null && (Plugin?.ZetaUsed != true)) return new List<ClientCard> { millZeta };

                var millAlpha = cards.FirstOrDefault(c => c.Id == CardId.DrytronAlphaThuban);
                if (millAlpha != null && (Plugin?.AlphaUsed != true)) return new List<ClientCard> { millAlpha };

                var millGamma = cards.FirstOrDefault(c => c.Id == CardId.DrytronGammaEltanin);
                if (millGamma != null && (Plugin?.GammaUsed != true)) return new List<ClientCard> { millGamma };
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  MASTER DECK PLUGIN: DrytronTourPlugin
    // ═══════════════════════════════════════════════════════════════
    internal class DrytronTourPlugin
    {
        private readonly DrytronTourExecutor _exec;

        public bool AlphaUsed { get; set; }
        public bool ZetaUsed { get; set; }
        public bool NuUsed { get; set; }
        public bool GammaUsed { get; set; }
        public bool DeltaUsed { get; set; }
        public bool BetaUsed { get; set; }

        public DrytronStrategy Strategy { get; }
        public DrytronTributeManager TributeManager { get; }
        public DrytronRitualAdvisor RitualAdvisor { get; }
        public DrytronMaterialScorer MaterialScorer { get; }
        public DrytronBoardAssessor BoardAssessor { get; }

        public DrytronTourPlugin(DrytronTourExecutor exec)
        {
            _exec = exec;
            Strategy = new DrytronStrategy(exec);
            TributeManager = new DrytronTributeManager(exec);
            RitualAdvisor = new DrytronRitualAdvisor(exec);
            MaterialScorer = new DrytronMaterialScorer(exec);
            BoardAssessor = new DrytronBoardAssessor(exec);
        }

        public void ResetTurnState()
        {
            AlphaUsed = false;
            ZetaUsed = false;
            NuUsed = false;
            GammaUsed = false;
            DeltaUsed = false;
            BetaUsed = false;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN HELPER: DrytronStrategy
    // ═══════════════════════════════════════════════════════════════
    internal class DrytronStrategy
    {
        private readonly DrytronTourExecutor _exec;

        public DrytronStrategy(DrytronTourExecutor exec)
        {
            _exec = exec;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN HELPER: DrytronTributeManager
    // ═══════════════════════════════════════════════════════════════
    internal class DrytronTributeManager
    {
        private readonly DrytronTourExecutor _exec;

        public DrytronTributeManager(DrytronTourExecutor exec)
        {
            _exec = exec;
        }

        public bool CanTributeForDrytron(ClientCard activator)
        {
            // Can tribute another Drytron or Ritual monster from hand or field
            var tributes = _exec.Bot.Hand.Concat(_exec.Bot.GetMonsters())
                .Where(c => c != activator && (c.HasSetcode(0x154) || c.HasType(CardType.Ritual)) && !_exec.IsAceCard(c))
                .ToList();

            return tributes.Count > 0;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN HELPER: DrytronRitualAdvisor
    // ═══════════════════════════════════════════════════════════════
    internal class DrytronRitualAdvisor
    {
        private readonly DrytronTourExecutor _exec;

        public DrytronRitualAdvisor(DrytronTourExecutor exec)
        {
            _exec = exec;
        }

        public bool CanRitualSummon()
        {
            bool hasDraconids = _exec.Bot.HasInHand(DrytronTourExecutor.CardId.DrytronMeteonisDADraconids) ||
                                _exec.Bot.HasInGraveyard(DrytronTourExecutor.CardId.DrytronMeteonisDADraconids);

            if (!hasDraconids) return false;

            // DA Draconids requires 5000 ATK tribute
            // Mu Beta with 2+ materials can tribute its materials directly
            var muBeta = _exec.Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.Id == DrytronTourExecutor.CardId.DrytronMuBetaFafnir);
            int availableAtk = 0;

            if (muBeta != null)
            {
                availableAtk += muBeta.Overlays.Count * 2000;
            }

            var fieldMachines = _exec.Bot.GetMonsters().Where(m => m != muBeta && m.IsFaceup() && m.HasRace(CardRace.Machine) && !_exec.IsAceCard(m)).ToList();
            availableAtk += fieldMachines.Sum(m => m.Attack);

            var handMachines = _exec.Bot.Hand.Where(c => c.HasRace(CardRace.Machine) && c.Id != DrytronTourExecutor.CardId.DrytronMeteonisDADraconids).ToList();
            availableAtk += handMachines.Sum(c => c.Attack);

            return availableAtk >= 4000;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN HELPER: DrytronMaterialScorer
    // ═══════════════════════════════════════════════════════════════
    internal class DrytronMaterialScorer
    {
        private readonly DrytronTourExecutor _exec;

        public DrytronMaterialScorer(DrytronTourExecutor exec)
        {
            _exec = exec;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN HELPER: DrytronBoardAssessor
    // ═══════════════════════════════════════════════════════════════
    internal class DrytronBoardAssessor
    {
        private readonly DrytronTourExecutor _exec;

        public DrytronBoardAssessor(DrytronTourExecutor exec)
        {
            _exec = exec;
        }
    }
}
