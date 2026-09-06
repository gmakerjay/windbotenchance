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
            42009023,  // Fossil Dyna Pachycephalo
            7902349,   // Jowgen the Spiritualist
            19261966,  // El Shaddoll Winda
            78193831,  // Vanity's Fiend
            82732705,  // Skill Drain
            5851097,   // Vanity's Emptiness
            4514109,   // Kaiser Colosseum
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
            { 4611341,  92 },  // Snake-Eye Ash (Resource Swing: +2 searches/summons)
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
            if (HardFloodgates.Contains(card.Id) && card.IsFaceup() && !card.IsDisabled())
            {
                return 100.0;
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
