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
    // ═══════════════════════════════════════════════════════════════
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: MadolchePlugin
    //  Non-Targeting Spin & Continuous Resource Recursion Engine
    // ═══════════════════════════════════════════════════════════════
    public class MadolchePlugin : DeckPluginBase
    {
        private readonly MadolcheExecutor _exec;

        public override string DeckName => "Madolche";

        public MadolcheStrategy StrategyImpl { get; }
        public MadolcheMaterialEvaluator MaterialImpl { get; }
        public MadolcheThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public MadolchePlugin(MadolcheExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new MadolcheStrategy(exec);
            MaterialImpl = new MadolcheMaterialEvaluator(exec);
            ThreatImpl = new MadolcheThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class MadolcheStrategy : IDeckStrategy
    {
        private readonly MadolcheExecutor _exec;

        public MadolcheStrategy(MadolcheExecutor exec) => _exec = exec;

        public void Reset() { }

        public bool IsGyCleanForPetingcessoeur()
        {
            return _exec.Bot.Graveyard.Count(c => c.IsMonster()) == 0;
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Context 1: Messengelato (Spell / Trap Search)
            if (context != null && context.Id == MadolcheExecutor.CardId.MadolcheMessengelato)
            {
                // 1. Salon (Extra Normal Summon + Auto-Set on recycle)
                var salon = candidates.FirstOrDefault(c => c.Id == MadolcheExecutor.CardId.MadolcheSalon);
                if (salon != null && !_exec.Bot.HasInSpellZone(MadolcheExecutor.CardId.MadolcheSalon) &&
                    !_exec.Bot.HasInHand(MadolcheExecutor.CardId.MadolcheSalon))
                    return salon;

                // 2. Chateau (+500 ATK + Monsters return to Hand instead of Deck)
                var chateau = candidates.FirstOrDefault(c => c.Id == MadolcheExecutor.CardId.MadolcheChateau);
                if (chateau != null && !_exec.Bot.HasInSpellZone(MadolcheExecutor.CardId.MadolcheChateau) &&
                    !_exec.Bot.HasInHand(MadolcheExecutor.CardId.MadolcheChateau))
                    return chateau;

                // 3. Promenade (Omni-Negate Trap)
                var promenade = candidates.FirstOrDefault(c => c.Id == MadolcheExecutor.CardId.MadolchePromenade);
                if (promenade != null && !_exec.Bot.HasInSpellZone(MadolcheExecutor.CardId.MadolchePromenade))
                    return promenade;

                // 4. Ticket (Free Special Summon from deck on bounce)
                var ticket = candidates.FirstOrDefault(c => c.Id == MadolcheExecutor.CardId.MadolcheTicket);
                if (ticket != null && !_exec.Bot.HasInSpellZone(MadolcheExecutor.CardId.MadolcheTicket))
                    return ticket;
            }

            // Context 2: Magileine / Meowcaroons (Monster Search)
            if (context != null && (context.Id == MadolcheExecutor.CardId.MadolcheMagileine ||
                                   context.Id == MadolcheExecutor.CardId.MadolcheMiniMeowcaroons))
            {
                // If GY is clean or can be cleaned, Petingcessoeur is supreme starter
                if (IsGyCleanForPetingcessoeur())
                {
                    var peting = candidates.FirstOrDefault(c => c.Id == MadolcheExecutor.CardId.MadolchePetingcessoeur);
                    if (peting != null && !_exec.Bot.HasInHand(MadolcheExecutor.CardId.MadolchePetingcessoeur))
                        return peting;
                }

                // Anjelly (tributes herself to summon from deck)
                var anjelly = candidates.FirstOrDefault(c => c.Id == MadolcheExecutor.CardId.MadolcheAnjelly);
                if (anjelly != null && !_exec.Bot.HasInHand(MadolcheExecutor.CardId.MadolcheAnjelly))
                    return anjelly;

                // Hootcake (if monsters in GY to banish)
                var hootcake = candidates.FirstOrDefault(c => c.Id == MadolcheExecutor.CardId.MadolcheHootcake);
                if (hootcake != null && _exec.Bot.Graveyard.Any(c => c.IsMonster()))
                    return hootcake;
            }

            // Context 3: Petingcessoeur special summon from deck
            if (context != null && context.Id == MadolcheExecutor.CardId.MadolchePetingcessoeur)
            {
                // Puddingcess enables Chocolat-a-la-Mode full detachment trigger!
                var pudding = candidates.FirstOrDefault(c => c.Id == MadolcheExecutor.CardId.MadolchePuddingcess);
                if (pudding != null && !_exec.Bot.HasInMonstersZone(MadolcheExecutor.CardId.MadolchePuddingcess))
                    return pudding;

                var anjelly = candidates.FirstOrDefault(c => c.Id == MadolcheExecutor.CardId.MadolcheAnjelly);
                if (anjelly != null) return anjelly;

                var mess = candidates.FirstOrDefault(c => c.Id == MadolcheExecutor.CardId.MadolcheMessengelato);
                if (mess != null) return mess;
            }

            return candidates[0];
        }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Priority for Xyz / Link / Revival:
            var tiarafraise = candidates.FirstOrDefault(c => c.Id == MadolcheExecutor.CardId.MadolcheQueenTiarafraise);
            if (tiarafraise != null) return tiarafraise;

            var glassouffle = candidates.FirstOrDefault(c => c.Id == MadolcheExecutor.CardId.MadolcheTeacherGlassouffle);
            if (glassouffle != null) return glassouffle;

            var tiaramisu = candidates.FirstOrDefault(c => c.Id == MadolcheExecutor.CardId.MadolcheQueenTiaramisu);
            if (tiaramisu != null) return tiaramisu;

            var chocolat = candidates.FirstOrDefault(c => c.Id == MadolcheExecutor.CardId.MadolchePuddingcessChocolatALaMode);
            if (chocolat != null) return chocolat;

            var sistart = candidates.FirstOrDefault(c => c.Id == MadolcheExecutor.CardId.MadolcheFreshSistart);
            if (sistart != null) return sistart;

            return candidates.FirstOrDefault(c => c != null);
        }

        public bool PrioritizeSpSummonFirst => IsGyCleanForPetingcessoeur() &&
                                               _exec.Bot.HasInHand(MadolcheExecutor.CardId.MadolchePetingcessoeur);

        public bool CanExecuteTurn1Combo => _exec.Bot.HasInHand(MadolcheExecutor.CardId.MadolchePetingcessoeur) ||
                                            _exec.Bot.HasInHand(MadolcheExecutor.CardId.MadolcheAnjelly) ||
                                            _exec.Bot.HasInHand(MadolcheExecutor.CardId.MadolcheMagileine);

        public bool CanExecuteTurn2Combo => true;
        public int AssessBoardState() => _exec.Bot.GetMonsters().Count(m => m.IsFaceup());
    }

    public class MadolcheMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly MadolcheExecutor _exec;

        public MadolcheMaterialEvaluator(MadolcheExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 999;

            // Ace Xyz / Link Bosses are priceless
            if (card.Id == MadolcheExecutor.CardId.MadolcheQueenTiarafraise) return 50000;
            if (card.Id == MadolcheExecutor.CardId.MadolcheTeacherGlassouffle) return 30000;
            if (card.Id == MadolcheExecutor.CardId.MadolcheFreshSistart) return 20000;
            if (card.Id == MadolcheExecutor.CardId.MadolchePuddingcessChocolatALaMode) return 15000;

            // Messengelato on field after searching is prime material for Link/Xyz
            if (card.Id == MadolcheExecutor.CardId.MadolcheMessengelato) return 50;
            if (card.Id == MadolcheExecutor.CardId.MadolchePuddingcess) return 80;
            if (card.Id == MadolcheExecutor.CardId.MadolcheMagileine) return 100;
            if (card.Id == MadolcheExecutor.CardId.MadolcheHootcake) return 150;

            return 200;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.Where(c => c != null).OrderBy(c => GetMaterialCost(c)).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }
    }

    public class MadolcheThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly MadolcheExecutor _exec;

        public MadolcheThreatEvaluator(MadolcheExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null || card.Controller == 0) return 0;

            int score = 0;
            int id = card.Id;

            // Mass Backrow Removals threatening Chateau / Salon / Promenade
            if (id == 18144506 || id == 43898403 || id == 57728570 || id == 12580477)
                score += 15000;

            if (CardIntelligence.IsFloodgate(id)) score += 8000;
            if (CardIntelligence.IsKnownNegator(id)) score += 7000;

            // High ATK or Untargetable monsters (prime targets for Tiaramisu / Tiarafraise non-target spin!)
            if (card.Attack >= 2800) score += 5000;
            if (card.IsFaceup()) score += 2000;

            return score;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null || card.Controller == 0) return false;
            int id = card.Id;
            return id == 18144506 || id == 43898403 || id == 57728570 || card.Attack >= 3000;
        }
    }
}
