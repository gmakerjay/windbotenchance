using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.DecisionEngine
{
    /// <summary>
    /// Analyzes the tactical and strategic threat level of opponent cards on the board.
    /// Rather than just looking at raw ATK, it evaluates threat based on
    /// Expected Resource Swing, active floodgates, and disruption potential.
    /// </summary>
    public class ThreatAnalyzer
    {
        private readonly OpponentProfiler _profiler;

        // Floodgates that completely block play (threat = 100)
        private static readonly HashSet<int> HardFloodgates = new HashSet<int>
        {
            42009836,  // Fossil Dyna Pachycephalo
            41855169,   // Jowgen the Spiritualist
            19261966,  // El Shaddoll Winda
            47084486,  // Vanity's Fiend
            82732705,  // Skill Drain
            5851097,   // Vanity's Emptiness
            35059553,   // Kaiser Colosseum
        };

        // Key combo starters / high resource swing generators
        private static readonly Dictionary<int, double> HighResourceSwingCards = new Dictionary<int, double>
        {
            { 25311006, 95 },  // Triple Tactics Talent (Resource Swing: +2 draw / take control)
            { 84211599, 90 },  // Pot of Prosperity (Resource Swing: search top 6)
            { 73628505, 80 },  // Terraforming (Resource Swing: +1 search)
            { 44335251, 85 },  // Souleating Oviraptor (Resource Swing: +1 search / summon)
            { 14558127, 88 },  // Ash Blossom (Resource Swing: -1 negate)
            // Snake-Eye cards
            { 9674034,  92 },  // Snake-Eye Ash (Resource Swing: +2 searches/summons)
            { 15778492, 85 },  // Gaming Gamer GG
        };

        public ThreatAnalyzer(OpponentProfiler profiler)
        {
            _profiler = profiler;
        }

        /// <summary>
        /// Evaluates and returns a threat score (0 to 100) for a given opponent card.
        /// </summary>
        public double GetThreatScore(ClientCard card)
        {
            if (card == null) return 0;

            // 1. Check Hard Floodgates (Highest priority threat)
            if ((HardFloodgates.Contains(card.Id) || CardIntelligence.IsFloodgateMonster(card.Id) || CardIntelligence.IsFloodgateSpellTrap(card.Id))
                && card.IsFaceup() && !card.IsDisabled())
            {
                return 100.0;
            }

            // 1.5. Check Known Negators & Disruptions
            if ((CardIntelligence.IsKnownNegator(card.Id) || CardIntelligence.IsKnownNegator(card.GetNonAltartCode()))
                && card.IsFaceup() && !card.IsDisabled())
            {
                return 98.0;
            }

            // 1.8. Check High-Threat Chokepoints
            if (CardIntelligence.IsHighThreatChokepoint(card.Id) || CardIntelligence.IsHighThreatChokepoint(card.GetNonAltartCode()))
            {
                return 92.0;
            }

            // 2. Check known high resource swing cards
            if (HighResourceSwingCards.TryGetValue(card.Id, out double score))
            {
                if (card.IsFaceup() && !card.IsDisabled())
                {
                    return score;
                }
            }

            // 3. Face-down backrow evaluation (highly dependent on opponent deck profile)
            if (card.Location == CardLocation.SpellZone && card.IsFacedown())
            {
                if (_profiler != null && _profiler.IsControlDeck())
                {
                    // Against control decks (e.g. Altergeist, Eldlich), backrow is extremely dangerous
                    return 75.0; 
                }
                return 40.0; // default backrow threat
            }

            // 4. Monster threats based on Extra Deck status or stats
            if (card.Location == CardLocation.MonsterZone && card.IsFaceup())
            {
                double baseThreat = 30.0;

                // Extra Deck monsters have high utility / active effects
                if (card.IsExtraCard())
                {
                    baseThreat += 25.0;
                }

                // Add ATK scaling (high ATK = higher combat threat)
                baseThreat += Math.Min(35.0, card.Attack / 100.0);

                // If disabled, its active threat drops but still retains combat value
                if (card.IsDisabled())
                {
                    baseThreat *= 0.4;
                }

                return Math.Min(95.0, baseThreat);
            }

            return 10.0; // low base threat for GY/banished cards
        }

        /// <summary>
        /// [Core v0.094] Threat of an ACTIVATION (chain link) rather than of a card sitting on the board.
        /// GetThreatScore() rates a Normal Spell / monster effect from hand / GY effect at ~10 because of its
        /// location, which made the decision engine "hold" handtraps against real starters. This reads the
        /// card text generically (search / Special Summon from Deck-Extra-GY / draw) and returns
        /// max(board threat, activation threat).
        /// </summary>
        public double GetActivationThreatScore(ClientCard card)
        {
            if (card == null) return 0;
            double boardThreat = GetThreatScore(card);

            double activation = 40.0;
            try
            {
                if (CardTextSemantics.IsEngineEffect(card)) activation += 30.0;   // starter / extender engine
            }
            catch { }
            if (card.IsExtraCard()) activation += 10.0;
            if (card.HasType(CardType.Spell) && !card.HasType(CardType.Continuous)
                && !card.HasType(CardType.Field) && !card.HasType(CardType.Equip))
                activation += 5.0; // one-shot spell from hand = deliberate play
            if (card.IsDisabled()) activation *= 0.5;

            return Math.Min(100.0, Math.Max(boardThreat, activation));
        }

        /// <summary>
        /// Find the highest threat monster currently on the opponent's board.
        /// </summary>
        public ClientCard GetHighestThreatMonster(IEnumerable<ClientCard> monsters)
        {
            ClientCard highest = null;
            double maxThreat = -1.0;

            foreach (var m in monsters)
            {
                if (m == null) continue;
                double t = GetThreatScore(m);
                if (t > maxThreat)
                {
                    maxThreat = t;
                    highest = m;
                }
            }

            return highest;
        }
    }
}
