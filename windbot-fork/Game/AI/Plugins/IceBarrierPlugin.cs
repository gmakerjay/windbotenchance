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
    // DECOUPLED DOMAIN PLUGIN ARCHITECTURE: IceBarrierPlugin
    // Implements Strategy, MaterialEvaluator, and ThreatEvaluator
    // ═══════════════════════════════════════════════════════════════════════════
    public class IceBarrierPlugin : DeckPluginBase
    {
        private readonly IceBarrierExecutor _exec;

        public override string DeckName => "IceBarrier";

        public IceBarrierStrategy StrategyImpl { get; }
        public IceBarrierMaterialEvaluator MaterialImpl { get; }
        public IceBarrierThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public IceBarrierPlugin(IceBarrierExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new IceBarrierStrategy(exec);
            MaterialImpl = new IceBarrierMaterialEvaluator(exec);
            ThreatImpl = new IceBarrierThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class IceBarrierStrategy : IDeckStrategy
    {
        private readonly IceBarrierExecutor _exec;
        public IceBarrierStrategy(IceBarrierExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Opponent Turn: Lancea Disruption or Floater
            if (_exec.Duel.Player == 1)
            {
                // When Lancea leaves the field by an opponent's card -> Summon Trishula Zero or Trishula
                var synchroFloater = candidates.FirstOrDefault(c => c.Id == IceBarrierExecutor.CardId.TrishulaZeroDragon);
                if (synchroFloater != null) return synchroFloater;

                var synchroFloater2 = candidates.FirstOrDefault(c => c.Id == IceBarrierExecutor.CardId.TrishulaDragon);
                if (synchroFloater2 != null) return synchroFloater2;

                // Lancea Quick Trigger on opponent's Special Summon:
                // Priority 1: General Raiho (Opponent must discard 1 card for every monster effect on field, or it's negated)
                var raiho = candidates.FirstOrDefault(c => c.Id == IceBarrierExecutor.CardId.GeneralRaiho);
                if (raiho != null && !_exec.Bot.HasInMonstersZone(IceBarrierExecutor.CardId.GeneralRaiho))
                    return raiho;

                // Priority 2: Georgius (Opponent cannot activate monster effects in the GY while another Ice Barrier is on field)
                var georgius = candidates.FirstOrDefault(c => c.Id == IceBarrierExecutor.CardId.GeorgiusSwordman);
                if (georgius != null && !_exec.Bot.HasInMonstersZone(IceBarrierExecutor.CardId.GeorgiusSwordman))
                    return georgius;

                // Priority 3: Medium (Opponent can only activate 1 Spell/Trap card each turn)
                var medium = candidates.FirstOrDefault(c => c.Id == IceBarrierExecutor.CardId.Medium);
                if (medium != null && !_exec.Bot.HasInMonstersZone(IceBarrierExecutor.CardId.Medium))
                    return medium;

                // Priority 4: Warlock (Both players must set Spells before activating them - Anti-Spell Fragrance)
                var warlock = candidates.FirstOrDefault(c => c.Id == IceBarrierExecutor.CardId.Warlock);
                if (warlock != null && !_exec.Bot.HasInMonstersZone(IceBarrierExecutor.CardId.Warlock))
                    return warlock;

                // Fallback to highest ATK
                return candidates.OrderByDescending(c => c.Attack).FirstOrDefault();
            }

            // Our Turn: Engine climbing and revival
            if (_exec.Duel.Player == 0)
            {
                // Freezing Chains / Prior / Georgius / Winds Over
                // 1) If we need a Tuner to Synchro climb -> Mirror Mage
                if (!_exec.Bot.GetMonsters().Any(m => m.IsFaceup() && (m.HasType(CardType.Tuner) || m.Id == IceBarrierExecutor.CardId.MirrorMage)))
                {
                    var tuner = candidates.FirstOrDefault(c => c.Id == IceBarrierExecutor.CardId.MirrorMage);
                    if (tuner != null) return tuner;
                }

                // 2) Georgius (Special Summons Level 5 or lower Ice Barrier on summon)
                var geo = candidates.FirstOrDefault(c => c.Id == IceBarrierExecutor.CardId.GeorgiusSwordman);
                if (geo != null && !_exec.Bot.HasInMonstersZone(IceBarrierExecutor.CardId.GeorgiusSwordman))
                    return geo;

                // 3) Revealer (Starter / Extender)
                var rev = candidates.FirstOrDefault(c => c.Id == IceBarrierExecutor.CardId.Revealer);
                if (rev != null) return rev;

                // 4) Speaker (Extender)
                var spk = candidates.FirstOrDefault(c => c.Id == IceBarrierExecutor.CardId.Speaker);
                if (spk != null) return spk;

                // 5) Revive Boss from GY
                var boss = candidates.FirstOrDefault(c => c.Id == IceBarrierExecutor.CardId.LanceaAncestralDragon ||
                                                          c.Id == IceBarrierExecutor.CardId.IcejadeGymirAegirine ||
                                                          c.Id == IceBarrierExecutor.CardId.SwordsoulChengying);
                if (boss != null) return boss;

                return candidates.OrderByDescending(c => c.Attack).FirstOrDefault();
            }

            return candidates.FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard contextCard)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Search priority:
            // 1. If we have no starter on field or hand -> Medallion / Revealer
            bool hasRevealer = _exec.Bot.HasInHand(IceBarrierExecutor.CardId.Revealer) || _exec.Bot.HasInMonstersZone(IceBarrierExecutor.CardId.Revealer);
            bool hasMirrorMage = _exec.Bot.HasInHand(IceBarrierExecutor.CardId.MirrorMage) || _exec.Bot.HasInMonstersZone(IceBarrierExecutor.CardId.MirrorMage);
            bool hasGeorgius = _exec.Bot.HasInHand(IceBarrierExecutor.CardId.GeorgiusSwordman) || _exec.Bot.HasInMonstersZone(IceBarrierExecutor.CardId.GeorgiusSwordman);

            if (!hasRevealer)
            {
                var rev = candidates.FirstOrDefault(c => c.Id == IceBarrierExecutor.CardId.Revealer);
                if (rev != null) return rev;
            }

            // 2. If we need Extender / Tuner
            if (!hasMirrorMage)
            {
                var mm = candidates.FirstOrDefault(c => c.Id == IceBarrierExecutor.CardId.MirrorMage);
                if (mm != null) return mm;
            }

            // 3. Georgius (Extender + GY reviver + GY lock)
            if (!hasGeorgius)
            {
                var geo = candidates.FirstOrDefault(c => c.Id == IceBarrierExecutor.CardId.GeorgiusSwordman);
                if (geo != null) return geo;
            }

            // 4. Freezing Chains (Revival Spell)
            if (!_exec.Bot.HasInHand(IceBarrierExecutor.CardId.FreezingChains) && !_exec.Bot.HasInSpellZone(IceBarrierExecutor.CardId.FreezingChains))
            {
                var fc = candidates.FirstOrDefault(c => c.Id == IceBarrierExecutor.CardId.FreezingChains);
                if (fc != null) return fc;
            }

            // 5. Speaker (Free special summon extender)
            var speaker = candidates.FirstOrDefault(c => c.Id == IceBarrierExecutor.CardId.Speaker);
            if (speaker != null && !_exec.Bot.HasInHand(IceBarrierExecutor.CardId.Speaker))
                return speaker;

            // 6. Wayne (S/T searcher)
            var wayne = candidates.FirstOrDefault(c => c.Id == IceBarrierExecutor.CardId.GeneralWayne);
            if (wayne != null && !_exec.Bot.HasInHand(IceBarrierExecutor.CardId.GeneralWayne))
                return wayne;

            // 7. Disruption Boss for Turn 2 / Followup
            var raiho = candidates.FirstOrDefault(c => c.Id == IceBarrierExecutor.CardId.GeneralRaiho);
            if (raiho != null && !_exec.Bot.HasInHand(IceBarrierExecutor.CardId.GeneralRaiho))
                return raiho;

            // 8. Gameciel Kaiju (Board breaker)
            var kaiju = candidates.FirstOrDefault(c => c.Id == IceBarrierExecutor.CardId.GamecielKaiju);
            if (kaiju != null)
                return kaiju;

            return candidates.FirstOrDefault();
        }
    }

    public class IceBarrierMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly IceBarrierExecutor _exec;
        public IceBarrierMaterialEvaluator(IceBarrierExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard c)
        {
            if (c == null) return 0;

            // 1. Tokens: Zero cost, use immediately as Synchro material
            if (c.Id == IceBarrierExecutor.CardId.IceBarrierToken || c.HasType(CardType.Token))
                return 1;

            // 2. Mirror Mage: Gains search on sent to GY -> Low cost
            if (c.Id == IceBarrierExecutor.CardId.MirrorMage)
                return 2;

            // 3. Speaker: Generates token in GY -> Low cost
            if (c.Id == IceBarrierExecutor.CardId.Speaker)
                return 3;

            // 4. Prior: Low cost
            if (c.Id == IceBarrierExecutor.CardId.Prior)
                return 4;

            // 5. Revealer: Medium cost
            if (c.Id == IceBarrierExecutor.CardId.Revealer)
                return 5;

            // 6. Wayne / Coral Dragon: Medium cost (Coral draws 1)
            if (c.Id == IceBarrierExecutor.CardId.CoralDragon)
                return 4;
            if (c.Id == IceBarrierExecutor.CardId.GeneralWayne)
                return 6;

            // 7. Georgius: Higher cost if providing GY lock
            if (c.Id == IceBarrierExecutor.CardId.GeorgiusSwordman)
                return 15;

            // 8. Floodgates & Ace Bosses: NEVER sacrifice unless absolutely no choice
            if (c.Id == IceBarrierExecutor.CardId.GeneralRaiho ||
                c.Id == IceBarrierExecutor.CardId.Medium ||
                c.Id == IceBarrierExecutor.CardId.Warlock)
                return 80;

            if (c.Id == IceBarrierExecutor.CardId.LanceaAncestralDragon ||
                c.Id == IceBarrierExecutor.CardId.TrishulaZeroDragon ||
                c.Id == IceBarrierExecutor.CardId.TrishulaDragon ||
                c.Id == IceBarrierExecutor.CardId.IcejadeGymirAegirine ||
                c.Id == IceBarrierExecutor.CardId.SwordsoulChengying ||
                c.Id == IceBarrierExecutor.CardId.AdamancipatorDragite)
                return 100;

            return 10;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.OrderBy(GetMaterialCost).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Discard priority: Cards with graveyard triggers/utility:
            // 1. Mirror Mage: Triggers search when sent to GY!
            var mm = candidates.FirstOrDefault(c => c.Id == IceBarrierExecutor.CardId.MirrorMage);
            if (mm != null) return mm;

            // 2. Speaker: Can banish itself from GY to create an Ice Barrier Token!
            var speaker = candidates.FirstOrDefault(c => c.Id == IceBarrierExecutor.CardId.Speaker);
            if (speaker != null) return speaker;

            // 3. Ice Barrier Trap: Can banish from GY to dump & search Gameciel Kaiju!
            var trap = candidates.FirstOrDefault(c => c.Id == IceBarrierExecutor.CardId.IceBarrierTrap);
            if (trap != null) return trap;

            // 4. Winds Over the Ice Barrier: Can banish from GY to retrieve an Ice Barrier!
            var winds = candidates.FirstOrDefault(c => c.Id == IceBarrierExecutor.CardId.WindsOver);
            if (winds != null) return winds;

            // 5. High-level Ice Barriers we can revive with Georgius / Prior / Freezing Chains
            var reviveTarget = candidates.FirstOrDefault(c => c.Id == IceBarrierExecutor.CardId.GeneralRaiho ||
                                                              c.Id == IceBarrierExecutor.CardId.Medium ||
                                                              c.Id == IceBarrierExecutor.CardId.GeneralWayne);
            if (reviveTarget != null && (_exec.Bot.HasInHand(IceBarrierExecutor.CardId.GeorgiusSwordman) ||
                                         _exec.Bot.HasInHand(IceBarrierExecutor.CardId.FreezingChains)))
                return reviveTarget;

            // 6. Duplicates in hand
            var dup = candidates.GroupBy(c => c.Id).FirstOrDefault(g => g.Count() > 1)?.FirstOrDefault();
            if (dup != null && dup.Id != IceBarrierExecutor.CardId.Medallion)
                return dup;

            // 7. Non-engine or low priority
            return candidates.OrderBy(GetMaterialCost).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            return candidates?.OrderBy(GetMaterialCost).FirstOrDefault();
        }
    }

    public class IceBarrierThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly IceBarrierExecutor _exec;
        public IceBarrierThreatEvaluator(IceBarrierExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard c)
        {
            if (c == null) return 0;
            int score = 0;

            // Continuous floodgates & spell wipes are extreme threats to Ice Barrier
            if (c.Id == 18144506 || c.Id == 18144507 || c.Id == 14532163) score += 90; // Harpie, Lightning Storm
            if (c.Id == 15693423) score += 95; // Evenly Matched
            if (c.Id == 82732705 || c.Id == 99745551) score += 80; // Skill Drain, D-Shifter

            if (c.IsFaceup() && c.HasType(CardType.Monster))
            {
                if (c.Attack >= 3000) score += 30;
                if (CardIntelligence.IsKnownNegator(c.Id)) score += 50;
                if (CardIntelligence.IsHighThreatChokepoint(c.Id)) score += 40;
            }

            return score;
        }

        public bool IsEmergencyThreat(ClientCard c)
        {
            if (c == null) return false;
            return EvaluateThreatScore(c) >= 70;
        }
    }
}
