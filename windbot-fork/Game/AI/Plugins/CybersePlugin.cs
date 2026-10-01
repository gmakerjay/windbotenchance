using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Decks;
using WindBot.Game.AI.Plugin;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Plugins
{
    public class CybersePlugin : DeckPluginBase
    {
        private readonly CyberseExecutor _exec;

        public override string DeckName => "Cyberse";

        public CyberseStrategy StrategyImpl { get; }
        public CyberseMaterialEvaluator MaterialImpl { get; }
        public CyberseThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public CybersePlugin(CyberseExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new CyberseStrategy(exec);
            MaterialImpl = new CyberseMaterialEvaluator(exec);
            ThreatImpl = new CyberseThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class CyberseStrategy : IDeckStrategy
    {
        private readonly CyberseExecutor _exec;
        public CyberseStrategy(CyberseExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Transcode Talker GY Revival:
            // Revive Splash Mage (Link-2) or Update Jammer to immediately climb into Accesscode Talker!
            var splash = candidates.FirstOrDefault(c => c.Id == CyberseExecutor.CardId.SplashMage);
            if (splash != null) return splash;

            var jammer = candidates.FirstOrDefault(c => c.Id == CyberseExecutor.CardId.UpdateJammer);
            if (jammer != null) return jammer;

            var transcode = candidates.FirstOrDefault(c => c.Id == CyberseExecutor.CardId.TranscodeTalker);
            if (transcode != null) return transcode;

            var decode = candidates.FirstOrDefault(c => c.Id == CyberseExecutor.CardId.DecodeTalker);
            if (decode != null) return decode;

            var gadget = candidates.FirstOrDefault(c => c.Id == CyberseExecutor.CardId.CyberseGadget);
            if (gadget != null) return gadget;

            var debug = candidates.FirstOrDefault(c => c.Id == CyberseExecutor.CardId.LadyDebug);
            if (debug != null) return debug;

            var balancer = candidates.FirstOrDefault(c => c.Id == CyberseExecutor.CardId.BalancerLord);
            if (balancer != null) return balancer;

            var dot = candidates.FirstOrDefault(c => c.Id == CyberseExecutor.CardId.DotScaper);
            if (dot != null) return dot;

            var bitron = candidates.FirstOrDefault(c => c.Id == CyberseExecutor.CardId.Bitron);
            if (bitron != null) return bitron;

            return candidates.OrderByDescending(c => c.Attack).FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard contextCard)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Lady Debug search (Lv3 or lower Cyberse):
            var balancer = candidates.FirstOrDefault(c => c.Id == CyberseExecutor.CardId.BalancerLord);
            if (balancer != null && !_exec.BalancerLordUsed) return balancer;

            var dot = candidates.FirstOrDefault(c => c.Id == CyberseExecutor.CardId.DotScaper);
            if (dot != null) return dot;

            var draconnet = candidates.FirstOrDefault(c => c.Id == CyberseExecutor.CardId.Draconnet);
            if (draconnet != null) return draconnet;

            // Cynet Mining search (Lv4 or lower Cyberse):
            var debug = candidates.FirstOrDefault(c => c.Id == CyberseExecutor.CardId.LadyDebug);
            if (debug != null) return debug;

            var gadget = candidates.FirstOrDefault(c => c.Id == CyberseExecutor.CardId.CyberseGadget);
            if (gadget != null) return gadget;

            var linkslayer = candidates.FirstOrDefault(c => c.Id == CyberseExecutor.CardId.Linkslayer);
            if (linkslayer != null) return linkslayer;

            var rom = candidates.FirstOrDefault(c => c.Id == CyberseExecutor.CardId.ROMCloudia);
            if (rom != null) return rom;

            return candidates.OrderByDescending(c => c.Attack).FirstOrDefault();
        }
    }

    public class CyberseMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly CyberseExecutor _exec;
        public CyberseMaterialEvaluator(CyberseExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard c)
        {
            if (c == null) return 0;

            // Absolute Finisher: NEVER use Accesscode Talker as material
            if (c.Id == CyberseExecutor.CardId.AccesscodeTalker) return 99999;

            // If we are summoning Accesscode Talker (CurrentExecutingCard.Id == AccesscodeTalker),
            // Transcode Talker and Splash Mage have VERY LOW material cost so they are picked!
            if (_exec.CurrentExecutingCard != null && _exec.CurrentExecutingCard.Id == CyberseExecutor.CardId.AccesscodeTalker)
            {
                if (c.Id == CyberseExecutor.CardId.TranscodeTalker) return 10;
                if (c.Id == CyberseExecutor.CardId.SplashMage) return 20;
                if (c.Id == CyberseExecutor.CardId.UpdateJammer) return 15;
            }

            // If we are summoning Transcode Talker:
            if (_exec.CurrentExecutingCard != null && _exec.CurrentExecutingCard.Id == CyberseExecutor.CardId.TranscodeTalker)
            {
                if (c.Id == CyberseExecutor.CardId.SplashMage) return 10;
                if (c.Id == CyberseExecutor.CardId.UpdateJammer) return 15;
                if (c.Id == CyberseExecutor.CardId.Honeybot) return 25;
            }

            // Normal Boss Protection
            if (c.Id == CyberseExecutor.CardId.DecodeTalker) return 9000;
            if (c.Id == CyberseExecutor.CardId.TriGateWizard) return 8500;
            if (c.Id == CyberseExecutor.CardId.EncodeTalker) return 8000;
            if (c.Id == CyberseExecutor.CardId.TranscodeTalker) return 4000;
            if (c.Id == CyberseExecutor.CardId.Honeybot) return 3000;
            if (c.Id == CyberseExecutor.CardId.DualAssembloom) return 2500;

            // Link-2 Stepping Stones
            if (c.Id == CyberseExecutor.CardId.SplashMage) return 300;
            if (c.Id == CyberseExecutor.CardId.UpdateJammer) return 200;
            if (c.Id == CyberseExecutor.CardId.BinarySorceress) return 200;

            // Main Deck Monsters are Link Fodder!
            if (c.Id == CyberseExecutor.CardId.Linkslayer) return 100;
            if (c.Id == CyberseExecutor.CardId.BootStagguard) return 90;
            if (c.Id == CyberseExecutor.CardId.LadyDebug) return 80;
            if (c.Id == CyberseExecutor.CardId.CyberseGadget) return 80;
            if (c.Id == CyberseExecutor.CardId.ROMCloudia) return 70;
            if (c.Id == CyberseExecutor.CardId.BalancerLord) return 60;
            if (c.Id == CyberseExecutor.CardId.Backlinker) return 50;
            if (c.Id == CyberseExecutor.CardId.Draconnet) return 40;
            if (c.Id == CyberseExecutor.CardId.LinkSpider) return 30;
            if (c.Id == CyberseExecutor.CardId.Bitron) return 20;
            if (c.Id == CyberseExecutor.CardId.DotScaper) return 15;
            if (c.Id == CyberseExecutor.CardId.GadgetToken) return 10;

            return 50;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.OrderBy(GetMaterialCost).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;

            var dot = candidates.FirstOrDefault(c => c.Id == CyberseExecutor.CardId.DotScaper);
            if (dot != null) return dot;

            var assem = candidates.FirstOrDefault(c => c.Id == CyberseExecutor.CardId.DualAssembloom);
            if (assem != null) return assem;

            var bitron = candidates.FirstOrDefault(c => c.Id == CyberseExecutor.CardId.Bitron);
            if (bitron != null) return bitron;

            var duplicates = candidates.GroupBy(c => c.Id).Where(g => g.Count() > 1).Select(g => g.First()).FirstOrDefault();
            if (duplicates != null && duplicates.Id != CyberseExecutor.CardId.AshBlossom) return duplicates;

            return candidates.OrderBy(GetMaterialCost).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(GetMaterialCost).FirstOrDefault();
        }
    }

    public class CyberseThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly CyberseExecutor _exec;
        public CyberseThreatEvaluator(CyberseExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard c)
        {
            if (c == null) return 0;
            int score = 0;

            if (c.Attack >= 3000) score += 40;
            else if (c.Attack >= 2500) score += 25;

            if (c.IsFaceup())
            {
                if (c.HasType(CardType.Continuous) || c.HasType(CardType.Field)) score += 30;
                if (c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link)) score += 20;
            }

            return score;
        }

        public bool IsEmergencyThreat(ClientCard c)
        {
            if (c == null) return false;
            return c.Attack >= 3000 || (c.IsFaceup() && (c.Id == 1561110 || c.Id == 50954680 || c.Id == 63767246));
        }
    }
}
