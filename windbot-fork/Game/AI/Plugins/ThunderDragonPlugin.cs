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
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: ThunderDragonPlugin
    //  Thunder Dragon Banish Combo & Floodgate Control Engine
    // ═══════════════════════════════════════════════════════════════
    public class ThunderDragonPlugin : DeckPluginBase
    {
        private readonly ThunderDragonExecutor _exec;

        public override string DeckName => "ThunderDragon";

        public ThunderDragonStrategy StrategyImpl { get; }
        public ThunderDragonMaterialEvaluator MaterialImpl { get; }
        public ThunderDragonThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public ThunderDragonPlugin(ThunderDragonExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new ThunderDragonStrategy(exec);
            MaterialImpl = new ThunderDragonMaterialEvaluator(exec);
            ThreatImpl = new ThunderDragonThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }

        public IList<ClientCard> SelectCardLogic(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return null;

            // 1. Hint 502 = DESTROY, 503 = REMOVE, 504 = TOGRAVE, 505 = RTOHAND, 507 = TODECK
            // HARD RULE: Always target opponent cards first!
            var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
            if (enemyCards.Count >= min && (hint == 502 || hint == 503 || hint == 504 || hint == 505 || hint == 507))
            {
                var sorted = enemyCards.OrderByDescending(c => ThreatImpl.EvaluateThreatScore(c)).ToList();
                return sorted.Take(Math.Min(max, sorted.Count)).ToList();
            }

            // 2. Hint 503 = HINTMSG_REMOVE (Our cards: Hand banish / Deck banish / GY banish cost)
            if (hint == 503 || cards.All(c => c.Controller == 0))
            {
                // 2a. Aloof Lupine / Allure of Darkness banish from HAND
                if (cards.All(c => c.Location == CardLocation.Hand))
                {
                    var handTarget = StrategyImpl.PickHandBanishTarget(cards);
                    if (handTarget != null && min <= 1 && 1 <= max)
                        return new List<ClientCard> { handTarget };
                }

                // 2b. Aloof Lupine / Gold Sarcophagus banish from DECK
                if (cards.All(c => c.Location == CardLocation.Deck))
                {
                    var deckTarget = StrategyImpl.PickAloofDeckBanishTarget(cards);
                    if (deckTarget != null && min <= 1 && 1 <= max)
                        return new List<ClientCard> { deckTarget };
                }

                // 2c. Colossus / Titan / Duo / Levianeer banish from GRAVE
                if (cards.All(c => c.Location == CardLocation.Grave))
                {
                    var sortedGy = cards.OrderBy(c => MaterialImpl.GetMaterialCost(c)).ToList();
                    if (sortedGy.Count >= min)
                        return sortedGy.Take(min).ToList();
                }
            }

            // 3. Hint 504 = HINTMSG_TOGRAVE (Our cards: Batteryman Solar mill from Deck)
            if (hint == 504 && cards.All(c => c.Location == CardLocation.Deck))
            {
                var millTarget = StrategyImpl.PickSolarMillTarget(cards);
                if (millTarget != null && min <= 1 && 1 <= max)
                    return new List<ClientCard> { millTarget };
            }

            // 4. Hint 506 = HINTMSG_ATOHAND (Deck / GY Search)
            if (hint == 506 || cards.All(c => c.Location == CardLocation.Deck))
            {
                var target = StrategyImpl.PickSearchTarget(cards, null);
                if (target != null && min <= 1 && 1 <= max)
                    return new List<ClientCard> { target };
            }

            // 5. Hint 509 = HINTMSG_SPSUMMON (Special Summon target)
            if (hint == 509)
            {
                var target = StrategyImpl.PickSpecialSummonTarget(cards);
                if (target != null && min <= 1 && 1 <= max)
                    return new List<ClientCard> { target };
            }

            // 6. Hint 507 = HINTMSG_TODECK (Hawk Mulligan from hand)
            if (hint == 507 && cards.All(c => c.Location == CardLocation.Hand))
            {
                var duplicates = cards.GroupBy(c => c.Id).Where(g => g.Count() > 1).Select(g => g.First()).ToList();
                if (duplicates.Count >= min)
                    return duplicates.Take(min).ToList();

                var toDeck = cards.OrderBy(c => MaterialImpl.GetMaterialCost(c)).Take(min).ToList();
                if (toDeck.Count >= min)
                    return toDeck;
            }

            // 7. Hint 500 = HINTMSG_RELEASE (Tribute for Colossus or tribute summon)
            if (hint == 500)
            {
                var sorted = MaterialImpl.SortMaterials(cards, min);
                if (sorted != null && sorted.Count >= min)
                    return sorted.Take(min).ToList();
            }

            // 8. Hint 501 = HINTMSG_DISCARD or hand cost
            if (hint == 501 || cards.All(c => c.Location == CardLocation.Hand))
            {
                var discard = MaterialImpl.PickDiscardTarget(cards, min);
                if (discard != null)
                    return new List<ClientCard> { discard };
            }

            return null;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  STRATEGY IMPLEMENTATION
    // ═══════════════════════════════════════════════════════════════
    public class ThunderDragonStrategy : IDeckStrategy
    {
        private readonly ThunderDragonExecutor _exec;

        public ThunderDragonStrategy(ThunderDragonExecutor exec)
        {
            _exec = exec;
        }

        public void Reset()
        {
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Thunder Dragon pitch: searches duplicate Thunder Dragon
            var td = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragon);
            if (td != null && candidates.All(c => c.Id == ThunderDragonExecutor.CardId.ThunderDragon))
                return td;

            // Thunder Dragonmatrix search: searches duplicate Thunder Dragonmatrix
            var matrix = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragonmatrix);
            if (matrix != null && candidates.All(c => c.Id == ThunderDragonExecutor.CardId.ThunderDragonmatrix))
                return matrix;

            // Dragondark search (can search ANY Thunder Dragon card):
            // 1. Thunder Dragon Fusion (if we don't have it and have GY/banish fuel >= 3)
            int gyBanishThunders = _exec.Bot.Graveyard.Count(IsThunder) + _exec.Bot.Banished.Count(IsThunder);
            if (!_exec.Bot.HasInHand(ThunderDragonExecutor.CardId.ThunderDragonFusion) &&
                !_exec.Bot.HasInSpellZone(ThunderDragonExecutor.CardId.ThunderDragonFusion) &&
                gyBanishThunders >= 3)
            {
                var fusion = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragonFusion);
                if (fusion != null) return fusion;
            }

            // 2. Thunder Dragonhawk (premium extender / reviver)
            if (!_exec.Bot.HasInHand(ThunderDragonExecutor.CardId.ThunderDragonhawk) && gyBanishThunders >= 1)
            {
                var hawk = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragonhawk);
                if (hawk != null) return hawk;
            }

            // 3. Thunder Dragonroar (deck summoner / recycler)
            if (!_exec.Bot.HasInHand(ThunderDragonExecutor.CardId.ThunderDragonroar))
            {
                var roar = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragonroar);
                if (roar != null) return roar;
            }

            // 4. Thunder Dragonduo (high ATK boss & banish beatstick)
            if (!_exec.Bot.HasInHand(ThunderDragonExecutor.CardId.ThunderDragonduo) &&
                !_exec.Bot.HasInMonstersZone(ThunderDragonExecutor.CardId.ThunderDragonduo))
            {
                var duo = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragonduo);
                if (duo != null) return duo;
            }

            // 5. Thunder Dragonmatrix (hand activator for Titan / Colossus)
            if (matrix != null) return matrix;

            // 6. Thunder Dragon
            if (td != null) return td;

            return candidates.FirstOrDefault();
        }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Reviving from GY/Banish (Hawk / Cross-Sheep / Monster Reborn)
            // Priority: Colossus > Titan > Levianeer > Roar > Dark > Duo > Solar > Matrix
            var colossus = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragonColossus);
            if (colossus != null) return colossus;

            var titan = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragonTitan);
            if (titan != null) return titan;

            var levi = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ChaosDragonLevianeer);
            if (levi != null) return levi;

            // Summon from Deck via Roar:
            // 1. Dark (great tribute for Colossus or link fodder, triggers search when sent to GY!)
            var dark = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragondark);
            if (dark != null) return dark;

            // 2. Matrix (level 1 for Linkuriboh/Anima, searches matrix on send)
            var matrix = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragonmatrix);
            if (matrix != null) return matrix;

            // 3. Roar
            var roar = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragonroar);
            if (roar != null) return roar;

            // 4. Solar (if summoned via Cross-Sheep)
            var solar = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.BatterymanSolar);
            if (solar != null) return solar;

            return candidates.FirstOrDefault();
        }

        public ClientCard PickHandBanishTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Prioritize hand cards that trigger upon being banished:
            // Dragondark (searches TD card) > Dragonroar (SS from deck) > Matrix (searches Matrix) > Hawk > Thunder Dragon > Duo > Solar
            var dark = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragondark);
            if (dark != null) return dark;

            var roar = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragonroar);
            if (roar != null) return roar;

            var matrix = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragonmatrix);
            if (matrix != null) return matrix;

            var hawk = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragonhawk);
            if (hawk != null) return hawk;

            var td = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragon);
            if (td != null) return td;

            return candidates.FirstOrDefault(c => c != null && IsThunder(c)) ?? candidates.FirstOrDefault();
        }

        public ClientCard PickSolarMillTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Batteryman Solar send to GY from Deck:
            // 1. Roar (can be revived by Hawk, or banished from GY by Duo/Colossus)
            // 2. Dark (can be revived by Hawk, or banished from GY)
            // 3. Matrix (can be banished, or adds another Matrix)
            var roar = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragonroar);
            if (roar != null) return roar;

            var dark = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragondark);
            if (dark != null) return dark;

            var matrix = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragonmatrix);
            if (matrix != null) return matrix;

            var td = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragon);
            if (td != null) return td;

            return candidates.FirstOrDefault(c => c != null && IsThunder(c));
        }

        public ClientCard PickAloofDeckBanishTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // If we banished Dark from hand, banish Roar from Deck to get SS from Deck!
            // If we banished Roar from hand, banish Dark from Deck to get search!
            // If we have neither, Dark > Roar > Matrix > Hawk
            var roar = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragonroar);
            var dark = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragondark);
            var matrix = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragonmatrix);

            if (roar != null) return roar;
            if (dark != null) return dark;
            if (matrix != null) return matrix;

            return candidates.FirstOrDefault(c => c != null && IsThunder(c));
        }

        private static bool IsThunder(ClientCard c)
        {
            if (c == null) return false;
            return c.HasRace(CardRace.Thunder);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  MATERIAL EVALUATOR
    // ═══════════════════════════════════════════════════════════════
    public class ThunderDragonMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly ThunderDragonExecutor _exec;

        public ThunderDragonMaterialEvaluator(ThunderDragonExecutor exec)
        {
            _exec = exec;
        }

        public int GetMaterialCost(ClientCard c)
        {
            if (c == null) return 0;

            // Never sacrifice or link off key boss monsters unless necessary
            if (c.Id == ThunderDragonExecutor.CardId.ThunderDragonTitan) return 20000;
            if (c.Id == ThunderDragonExecutor.CardId.ThunderDragonColossus) return 15000;
            if (c.Id == ThunderDragonExecutor.CardId.HopeHarbinger) return 14000;
            if (c.Id == ThunderDragonExecutor.CardId.AccesscodeTalker) return 12000;
            if (c.Id == ThunderDragonExecutor.CardId.ChaosDragonLevianeer) return 10000;
            if (c.Id == ThunderDragonExecutor.CardId.ThunderDragonduo) return 8000;

            // Mid-tier Link monsters
            if (c.Id == ThunderDragonExecutor.CardId.SPLittleKnight) return 6000;
            if (c.Id == ThunderDragonExecutor.CardId.IPMasquerena) return 5000;
            if (c.Id == ThunderDragonExecutor.CardId.KnightmareUnicorn) return 5000;

            // Low-cost / optimal fodders
            if (c.Id == ThunderDragonExecutor.CardId.BatterymanToken) return 10;
            if (c.Id == ThunderDragonExecutor.CardId.ThunderDragonmatrix) return 100; // Triggers search on GY/banish!
            if (c.Id == ThunderDragonExecutor.CardId.ThunderDragondark) return 150;   // Triggers search on field->GY!
            if (c.Id == ThunderDragonExecutor.CardId.ThunderDragonroar) return 200;   // Triggers SS on field->GY!
            if (c.Id == ThunderDragonExecutor.CardId.Linkuriboh) return 250;
            if (c.Id == ThunderDragonExecutor.CardId.RelinquishedAnima) return 250;
            if (c.Id == ThunderDragonExecutor.CardId.CrossSheep) return 300;
            if (c.Id == ThunderDragonExecutor.CardId.AloofLupine) return 350;
            if (c.Id == ThunderDragonExecutor.CardId.SomeSummerSummoner) return 400;
            if (c.Id == ThunderDragonExecutor.CardId.BatterymanSolar) return 450;
            if (c.Id == ThunderDragonExecutor.CardId.ThunderDragon) return 500;

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

            // Discard priority: duplicate cards, or cards that benefit in GY
            var dupDark = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragondark && _exec.Bot.Hand.Count(h => h.Id == c.Id) > 1);
            if (dupDark != null) return dupDark;

            var dupMatrix = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragonmatrix && _exec.Bot.Hand.Count(h => h.Id == c.Id) > 1);
            if (dupMatrix != null) return dupMatrix;

            var matrix = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragonmatrix);
            if (matrix != null) return matrix;

            var dark = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragondark);
            if (dark != null) return dark;

            var roar = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragonroar);
            if (roar != null) return roar;

            var hawk = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragonhawk);
            if (hawk != null) return hawk;

            return candidates.OrderBy(GetMaterialCost).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Banish substitute from GY for Colossus / Titan protection:
            // Prioritize cards that trigger when banished!
            var dark = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragondark);
            if (dark != null) return dark;

            var roar = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragonroar);
            if (roar != null) return roar;

            var matrix = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragonmatrix);
            if (matrix != null) return matrix;

            var hawk = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragonhawk);
            if (hawk != null) return hawk;

            var td = candidates.FirstOrDefault(c => c != null && c.Id == ThunderDragonExecutor.CardId.ThunderDragon);
            if (td != null) return td;

            return candidates.FirstOrDefault();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  THREAT EVALUATOR
    // ═══════════════════════════════════════════════════════════════
    public class ThunderDragonThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly ThunderDragonExecutor _exec;

        public ThunderDragonThreatEvaluator(ThunderDragonExecutor exec)
        {
            _exec = exec;
        }

        public int EvaluateThreatScore(ClientCard c)
        {
            if (c == null) return 0;
            return CardIntelligence.GetCardThreatScore(c);
        }

        public bool IsEmergencyThreat(ClientCard c)
        {
            if (c == null) return false;
            return CardIntelligence.IsKnownNegator(c.Id) || CardIntelligence.GetCardThreatScore(c) >= 9000;
        }
    }
}
