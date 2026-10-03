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
    // ═══════════════════════════════════════════════════════════════════════════
    // DECOUPLED DOMAIN PLUGIN ARCHITECTURE: Darklord2Plugin
    // Implements Strategy, MaterialEvaluator, ThreatEvaluator
    // ═══════════════════════════════════════════════════════════════════════════
    public class Darklord2Plugin : DeckPluginBase
    {
        private readonly Darklord2Executor _exec;

        public override string DeckName => "Darklord 2";

        public Darklord2Strategy StrategyImpl { get; }
        public Darklord2MaterialEvaluator MaterialImpl { get; }
        public Darklord2ThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public Darklord2Plugin(Darklord2Executor exec)
        {
            _exec = exec;
            StrategyImpl = new Darklord2Strategy(exec);
            MaterialImpl = new Darklord2MaterialEvaluator(exec);
            ThreatImpl = new Darklord2ThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class Darklord2Strategy : IDeckStrategy
    {
        private readonly Darklord2Executor _exec;

        public Darklord2Strategy(Darklord2Executor exec)
        {
            _exec = exec;
        }

        public void Reset()
        {
        }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. The First Darklord Quick Effect: Revive 1 Fairy from GY in DEF
            if (candidates.Any(c => c.Location == CardLocation.Grave))
            {
                var eveningstar = candidates.FirstOrDefault(c => c.Id == Darklord2Executor.CardId.DarklordEveningstar);
                if (eveningstar != null) return eveningstar;

                var morningstar = candidates.FirstOrDefault(c => c.Id == Darklord2Executor.CardId.DarklordMorningstar);
                if (morningstar != null) return morningstar;

                var ixchel = candidates.FirstOrDefault(c => c.Id == Darklord2Executor.CardId.DarklordIxchel);
                if (ixchel != null) return ixchel;

                var djehuty = candidates.FirstOrDefault(c => c.Id == Darklord2Executor.CardId.DarklordDjehuty);
                if (djehuty != null) return djehuty;
            }

            // 2. Darklord Djehuty On Summon effect: Special Summon 1 Darklord from Deck in DEF
            if (candidates.Any(c => c.Location == CardLocation.Deck))
            {
                // If we have no Gulgolet, summon Gulgolet (spawns 2 tokens)
                if (!_exec.Bot.HasInMonstersZone(Darklord2Executor.CardId.DarklordGulgolet))
                {
                    var gulgolet = candidates.FirstOrDefault(c => c.Id == Darklord2Executor.CardId.DarklordGulgolet);
                    if (gulgolet != null) return gulgolet;
                }

                var ixchel = candidates.FirstOrDefault(c => c.Id == Darklord2Executor.CardId.DarklordIxchel);
                if (ixchel != null) return ixchel;

                var tezcatlipoca = candidates.FirstOrDefault(c => c.Id == Darklord2Executor.CardId.DarklordTezcatlipoca);
                if (tezcatlipoca != null) return tezcatlipoca;
            }

            // 3. Darklord Contact: Revive Darklord from GY
            var firstDarklord = candidates.FirstOrDefault(c => c.Id == Darklord2Executor.CardId.TheFirstDarklord);
            if (firstDarklord != null) return firstDarklord;

            return candidates.OrderByDescending(c => c.Attack).FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Banishment of the Darklords (Search any Darklord card)
            if (context != null && context.Id == Darklord2Executor.CardId.BanishmentOfTheDarklords)
            {
                // If we don't have a Fusion Spell and have materials -> search Darklord Dance
                if (!_exec.Bot.HasInHand(Darklord2Executor.CardId.DarklordDance) && !_exec.Bot.HasInHand(Darklord2Executor.CardId.ApexPolymerization))
                {
                    var dance = candidates.FirstOrDefault(c => c.Id == Darklord2Executor.CardId.DarklordDance);
                    if (dance != null) return dance;
                }

                // If no Djehuty, search Djehuty (Starter: SS from deck!)
                if (!_exec.Bot.HasInHand(Darklord2Executor.CardId.DarklordDjehuty) && !_exec.Bot.HasInMonstersZone(Darklord2Executor.CardId.DarklordDjehuty))
                {
                    var djehuty = candidates.FirstOrDefault(c => c.Id == Darklord2Executor.CardId.DarklordDjehuty);
                    if (djehuty != null) return djehuty;
                }

                // If no Ixchel, search Ixchel (Draw 2!)
                if (!_exec.Bot.HasInHand(Darklord2Executor.CardId.DarklordIxchel))
                {
                    var ixchel = candidates.FirstOrDefault(c => c.Id == Darklord2Executor.CardId.DarklordIxchel);
                    if (ixchel != null) return ixchel;
                }

                // Search Morningstar (Fusion material for full field wipe on The First Darklord!)
                var morningstar = candidates.FirstOrDefault(c => c.Id == Darklord2Executor.CardId.DarklordMorningstar && !_exec.Bot.HasInHand(Darklord2Executor.CardId.DarklordMorningstar));
                if (morningstar != null) return morningstar;

                // Search The Sanctified Darklord or Darklord Rebellion
                var sanctified = candidates.FirstOrDefault(c => c.Id == Darklord2Executor.CardId.TheSanctifiedDarklord);
                if (sanctified != null) return sanctified;
            }

            // 2. Djehuty GY effect / Gulgolet GY effect: Search Darklord/Forbidden card
            if (context != null && (context.Id == Darklord2Executor.CardId.DarklordDjehuty || context.Id == Darklord2Executor.CardId.DarklordGulgolet))
            {
                var banishment = candidates.FirstOrDefault(c => c.Id == Darklord2Executor.CardId.BanishmentOfTheDarklords);
                if (banishment != null) return banishment;

                var dance = candidates.FirstOrDefault(c => c.Id == Darklord2Executor.CardId.DarklordDance && !_exec.Bot.HasInHand(Darklord2Executor.CardId.DarklordDance));
                if (dance != null) return dance;

                var contact = candidates.FirstOrDefault(c => c.Id == Darklord2Executor.CardId.DarklordContact);
                if (contact != null) return contact;

                var droplet = candidates.FirstOrDefault(c => c.Id == Darklord2Executor.CardId.ForbiddenDroplet);
                if (droplet != null) return droplet;
            }

            return candidates.FirstOrDefault();
        }
    }

    public class Darklord2MaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly Darklord2Executor _exec;

        public Darklord2MaterialEvaluator(Darklord2Executor exec)
        {
            _exec = exec;
        }

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 0;
            if (card.Id == Darklord2Executor.CardId.TheFirstDarklord) return 10000;
            if (card.Id == Darklord2Executor.CardId.DarklordEveningstar) return 8000;
            if (card.Id == Darklord2Executor.CardId.DarklordMorningstar) return 6000;
            if (card.Id == Darklord2Executor.CardId.DarklordIxchel) return 4000;
            if (card.Id == Darklord2Executor.CardId.DarklordTezcatlipoca) return 3000;
            if (card.Id == Darklord2Executor.CardId.DarklordDjehuty) return 2000;
            if (card.Id == Darklord2Executor.CardId.DarklordGulgolet) return 1500;
            if (card.HasType(CardType.Token)) return 100;
            return 1000;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.OrderBy(GetMaterialCost).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Priority 1: Gulgolet (When sent to GY: searches Darklord/Forbidden card!)
            var gulgolet = candidates.FirstOrDefault(c => c.Id == Darklord2Executor.CardId.DarklordGulgolet);
            if (gulgolet != null) return gulgolet;

            // Priority 2: Duplicate Darklord cards
            var duplicate = candidates.FirstOrDefault(c => candidates.Count(x => x.Id == c.Id) > 1);
            if (duplicate != null) return duplicate;

            // Priority 3: Tezcatlipoca
            var tezcatlipoca = candidates.FirstOrDefault(c => c.Id == Darklord2Executor.CardId.DarklordTezcatlipoca);
            if (tezcatlipoca != null) return tezcatlipoca;

            // Priority 4: Djehuty (has GY effect to banish self and search!)
            var djehuty = candidates.FirstOrDefault(c => c.Id == Darklord2Executor.CardId.DarklordDjehuty);
            if (djehuty != null) return djehuty;

            return candidates
                .Where(c => c.Id != Darklord2Executor.CardId.TheFirstDarklord && c.Id != Darklord2Executor.CardId.DarklordEveningstar)
                .OrderBy(GetMaterialCost)
                .FirstOrDefault() ?? candidates.First();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(GetMaterialCost).FirstOrDefault();
        }
    }

    public class Darklord2ThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly Darklord2Executor _exec;

        public Darklord2ThreatEvaluator(Darklord2Executor exec)
        {
            _exec = exec;
        }

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null || card.Controller == 0) return 0;
            int score = 0;
            if (card.IsFaceup())
            {
                if (card.HasType(CardType.Monster))
                {
                    if (card.Attack >= 3000) score += 40;
                    else if (card.Attack >= 2500) score += 30;
                    if (card.HasType(CardType.Effect)) score += 20;
                }
                if (card.HasType(CardType.Continuous) || card.HasType(CardType.Field)) score += 25;
            }
            else
            {
                if (card.IsFacedown() && (card.IsSpell() || card.IsTrap())) score += 35;
            }
            return score;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null || card.Controller == 0) return false;
            return EvaluateThreatScore(card) >= 50;
        }
    }
}
