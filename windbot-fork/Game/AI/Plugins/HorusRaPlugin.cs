using System.Collections.Generic;
using System.Linq;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Decks;
using WindBot.Game.AI.Plugin;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Plugins
{
    // ============================================================================
    // DECOUPLED DOMAIN PLUGIN ARCHITECTURE: HorusRaPlugin
    // ============================================================================
    public class HorusRaPlugin : DeckPluginBase
    {
        private readonly HorusRaExecutor _exec;

        public override string DeckName => "HorusRa";

        public HorusRaStrategy StrategyImpl { get; }
        public HorusRaMaterialEvaluator MaterialImpl { get; }
        public HorusRaThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public HorusRaPlugin(HorusRaExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new HorusRaStrategy(exec);
            MaterialImpl = new HorusRaMaterialEvaluator(exec);
            ThreatImpl = new HorusRaThreatEvaluator(exec);
        }
    }

    public class HorusRaStrategy : IDeckStrategy
    {
        private readonly HorusRaExecutor _exec;

        public HorusRaStrategy(HorusRaExecutor exec) => _exec = exec;

        public void Reset()
        {
        }

        public int GetTurnPlayRoute()
        {
            return _exec.Duel.Turn == 1 ? 1 : 2;
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context = null)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. King's Sarcophagus (if not in hand or on field)
            if (!_exec.Bot.HasInSpellZone(HorusRaExecutor.CardId.KingsSarcophagus) &&
                !_exec.Bot.HasInHand(HorusRaExecutor.CardId.KingsSarcophagus))
            {
                var sarc = candidates.FirstOrDefault(c => c != null && c.Id == HorusRaExecutor.CardId.KingsSarcophagus);
                if (sarc != null) return sarc;
            }

            // 2. The True Sun God (Engine starter)
            if (!_exec.Bot.HasInSpellZone(HorusRaExecutor.CardId.TheTrueSunGod) &&
                !_exec.Bot.HasInHand(HorusRaExecutor.CardId.TheTrueSunGod))
            {
                var trueSun = candidates.FirstOrDefault(c => c != null && c.Id == HorusRaExecutor.CardId.TheTrueSunGod);
                if (trueSun != null) return trueSun;
            }

            // 3. The Sun God Leading Down into Darkness (Dumps Ra + searches Sun God)
            if (!_exec.Bot.HasInSpellZone(HorusRaExecutor.CardId.TheSunGodLeadingDownIntoDarkness) &&
                !_exec.Bot.HasInHand(HorusRaExecutor.CardId.TheSunGodLeadingDownIntoDarkness))
            {
                var darkLeading = candidates.FirstOrDefault(c => c != null && c.Id == HorusRaExecutor.CardId.TheSunGodLeadingDownIntoDarkness);
                if (darkLeading != null) return darkLeading;
            }

            // 4. The Immortal Sun God (Revives Ra + cheats out Ra Darkness Fusion)
            var immortal = candidates.FirstOrDefault(c => c != null && c.Id == HorusRaExecutor.CardId.TheImmortalSunGod);
            if (immortal != null && !_exec.Bot.HasInHand(HorusRaExecutor.CardId.TheImmortalSunGod)) return immortal;

            // 5. Helpoemer Chanter (Special summons self + searches Sun God card)
            var helpoemer = candidates.FirstOrDefault(c => c != null && c.Id == HorusRaExecutor.CardId.HelpoemerChanter);
            if (helpoemer != null && !_exec.Bot.HasInHand(HorusRaExecutor.CardId.HelpoemerChanter)) return helpoemer;

            // 6. Makyura Destructor (GY trigger enables traps)
            var makyura = candidates.FirstOrDefault(c => c != null && c.Id == HorusRaExecutor.CardId.MakyuraDestructor);
            if (makyura != null) return makyura;

            // 7. Imsety, Glory of Horus
            var imsety = candidates.FirstOrDefault(c => c != null && c.Id == HorusRaExecutor.CardId.ImsetyGloryOfHorus);
            if (imsety != null && !_exec.Bot.HasInHand(HorusRaExecutor.CardId.ImsetyGloryOfHorus)) return imsety;

            // 8. Traps: Sun God Domination / Egyptian God Slime Protector
            var domination = candidates.FirstOrDefault(c => c != null && c.Id == HorusRaExecutor.CardId.SunGodDomination);
            if (domination != null && !_exec.Bot.HasInHand(HorusRaExecutor.CardId.SunGodDomination) && !_exec.Bot.HasInSpellZone(HorusRaExecutor.CardId.SunGodDomination)) return domination;

            var slime = candidates.FirstOrDefault(c => c != null && c.Id == HorusRaExecutor.CardId.EgyptianGodSlimeProtector);
            if (slime != null) return slime;

            // 9. The Winged Dragon of Ra
            var ra = candidates.FirstOrDefault(c => c != null && c.Id == HorusRaExecutor.CardId.TheWingedDragonOfRa);
            if (ra != null) return ra;

            return candidates.FirstOrDefault(c => c != null);
        }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Fusion Ace: Ra Sun God of Darkness
            var darkRa = candidates.FirstOrDefault(c => c != null && c.Id == HorusRaExecutor.CardId.RaSunGodOfDarkness);
            if (darkRa != null) return darkRa;

            // 2. The Winged Dragon of Ra
            var ra = candidates.FirstOrDefault(c => c != null && c.Id == HorusRaExecutor.CardId.TheWingedDragonOfRa);
            if (ra != null) return ra;

            // 3. Horus Core Revives
            var imsety = candidates.FirstOrDefault(c => c != null && c.Id == HorusRaExecutor.CardId.ImsetyGloryOfHorus);
            if (imsety != null) return imsety;

            var hapi = candidates.FirstOrDefault(c => c != null && c.Id == HorusRaExecutor.CardId.HapiGuidanceOfHorus);
            if (hapi != null) return hapi;

            var duamutef = candidates.FirstOrDefault(c => c != null && c.Id == HorusRaExecutor.CardId.DuamutefBlessingOfHorus);
            if (duamutef != null) return duamutef;

            var qebeh = candidates.FirstOrDefault(c => c != null && c.Id == HorusRaExecutor.CardId.QebehsenuefProtectionOfHorus);
            if (qebeh != null) return qebeh;

            // 4. Rank 8 Xyz Bosses
            var coach = candidates.FirstOrDefault(c => c != null && c.Id == HorusRaExecutor.CardId.CoachKingGiantrainer);
            if (coach != null) return coach;

            var photonLord = candidates.FirstOrDefault(c => c != null && c.Id == HorusRaExecutor.CardId.Number90GalaxyEyesPhotonLord);
            if (photonLord != null) return photonLord;

            var hope = candidates.FirstOrDefault(c => c != null && c.Id == HorusRaExecutor.CardId.Number38HopeHarbinger);
            if (hope != null) return hope;

            return candidates.FirstOrDefault(c => c != null);
        }
    }

    public class HorusRaMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly HorusRaExecutor _exec;

        public HorusRaMaterialEvaluator(HorusRaExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 999;

            // Safeguard Bosses from ever being used as Extra Deck material or tribute
            if (card.Id == HorusRaExecutor.CardId.RaSunGodOfDarkness) return 60000;
            if (card.Id == HorusRaExecutor.CardId.TheWingedDragonOfRa) return 40000;
            if (card.Id == HorusRaExecutor.CardId.Number90GalaxyEyesPhotonLord) return 45000;
            if (card.Id == HorusRaExecutor.CardId.Number38HopeHarbinger) return 45000;
            if (card.Id == HorusRaExecutor.CardId.CoachKingGiantrainer) return 35000;
            if (card.Id == HorusRaExecutor.CardId.DingirsuTheOrcust) return 30000;
            if (card.Id == HorusRaExecutor.CardId.Number68Sanaphond) return 30000;

            if (_exec.IsAceCard(card))
                return 25000;

            // High-power monsters on field (ATK >= 2000) should attack rather than be linked away into lower ATK
            if (card.Location == CardLocation.MonsterZone && card.Attack >= 2000)
                return 5000;

            // Discard priority from hand (Hint 501 / Costs):
            // Makyura triggers in GY!
            if (card.Location == CardLocation.Hand && card.Id == HorusRaExecutor.CardId.MakyuraDestructor)
                return 10;

            // Horus pieces in hand love being in GY so King's Sarcophagus can revive them freely
            if (card.Location == CardLocation.Hand &&
                (card.Id == HorusRaExecutor.CardId.HapiGuidanceOfHorus ||
                 card.Id == HorusRaExecutor.CardId.DuamutefBlessingOfHorus ||
                 card.Id == HorusRaExecutor.CardId.QebehsenuefProtectionOfHorus))
                return 15;

            // Ra in hand wants to be in GY for The Immortal Sun God
            if (card.Location == CardLocation.Hand && card.Id == HorusRaExecutor.CardId.TheWingedDragonOfRa)
                return 20;

            // Helpoemer or Gil Garth discard fodder
            if (card.Location == CardLocation.Hand &&
                (card.Id == HorusRaExecutor.CardId.HelpoemerChanter ||
                 card.Id == HorusRaExecutor.CardId.GilGarthTerrorMachine ||
                 card.Id == HorusRaExecutor.CardId.ViserDesShock))
                return 25;

            // Duplicate spells in hand
            if (card.Location == CardLocation.Hand &&
                _exec.Bot.Hand.Count(c => c != null && c.Id == card.Id) > 1)
                return 30;

            // General cards in hand evaluated with DeckProbability replacement odds
            if (card.Location == CardLocation.Hand)
            {
                int remainingInDeck = _exec.Bot.Deck.Count(c => c != null && c.Id == card.Id);
                // Irreplaceable one-of card with 0 remaining in deck
                if (remainingInDeck == 0)
                    return 2500;

                // Probability of redrawing this card
                double drawOdds = DeckProbability.AtLeastOne(_exec.Bot.Deck.Count, remainingInDeck, 1);
                // Higher drawOdds -> safer to discard -> lower cost
                int dynamicCost = 150 - (int)(drawOdds * 60.0);
                return System.Math.Max(40, dynamicCost);
            }

            return 100;
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
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }
    }

    public class HorusRaThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly HorusRaExecutor _exec;

        public HorusRaThreatEvaluator(HorusRaExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null) return 0;
            int score = 0;

            if (card.HasType(CardType.Monster))
            {
                if (card.Attack >= 3000) score += 3500;
                else if (card.Attack >= 2000) score += 2000;
                if (card.HasType(CardType.Fusion | CardType.Synchro | CardType.Xyz | CardType.Link)) score += 2500;
                if (card.IsFaceup()) score += 1000;
            }

            if (card.HasType(CardType.Spell | CardType.Trap))
            {
                // Board wipe cards
                if (card.Id == 18144506 || card.Id == 12580477 || card.Id == 15693423 || card.Id == 53582587) // Feather Duster, Lightning Storm, Evenly Matched, Torrential
                    score += 6000;
                else if (card.HasType(CardType.Continuous | CardType.Field))
                    score += 2500;
                else
                    score += 1500;
            }

            return score;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null) return false;
            return EvaluateThreatScore(card) >= 4000;
        }

        public ClientCard PickHighestThreat(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderByDescending(c => EvaluateThreatScore(c)).FirstOrDefault();
        }
    }
}
