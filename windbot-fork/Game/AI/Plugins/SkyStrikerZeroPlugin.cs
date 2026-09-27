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
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: SkyStrikerZeroPlugin
    //  Decouples Domain Rules, Strategy, and Material Evaluation
    //  for Sky Striker Ace = Zero Tournament Strategy
    // ═══════════════════════════════════════════════════════════════
    public class SkyStrikerZeroPlugin : DeckPluginBase
    {
        private readonly SkyStrikerZeroExecutor _exec;

        public override string DeckName => "SkyStrikerZero";

        public SkyStrikerZeroStrategy StrategyImpl { get; }
        public SkyStrikerZeroMaterialEvaluator MaterialImpl { get; }
        public SkyStrikerZeroThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public SkyStrikerZeroPlugin(SkyStrikerZeroExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new SkyStrikerZeroStrategy(exec);
            MaterialImpl = new SkyStrikerZeroMaterialEvaluator(exec);
            ThreatImpl = new SkyStrikerZeroThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class SkyStrikerZeroStrategy : IDeckStrategy
    {
        private readonly SkyStrikerZeroExecutor _exec;
        public SkyStrikerZeroStrategy(SkyStrikerZeroExecutor exec) => _exec = exec;

        public void Reset() { }

        public int CountSpellsInGrave(ClientField bot) =>
            bot.Graveyard.Count(c => c != null && c.IsSpell());

        public bool HasThreeSpellsInGrave(ClientField bot) =>
            CountSpellsInGrave(bot) >= 3;

        public bool HasMainMonsterZoneOccupied(ClientField bot) =>
            bot.GetMonsters().Any(c => c != null && c.Sequence < 5);

        public bool CanActivateSkyStrikerSpell(ClientField bot) =>
            !HasMainMonsterZoneOccupied(bot);

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            int[] priorities = {
                SkyStrikerZeroExecutor.CardId.Zero,
                SkyStrikerZeroExecutor.CardId.Kagari,
                SkyStrikerZeroExecutor.CardId.Shizuku,
                SkyStrikerZeroExecutor.CardId.Hayate,
                SkyStrikerZeroExecutor.CardId.Kaina,
                SkyStrikerZeroExecutor.CardId.SPLittleKnight,
                SkyStrikerZeroExecutor.CardId.Azalea,
                SkyStrikerZeroExecutor.CardId.Zeke,
                SkyStrikerZeroExecutor.CardId.Raye,
                SkyStrikerZeroExecutor.CardId.Roze
            };

            foreach (int id in priorities)
            {
                var match = candidates.FirstOrDefault(c => c != null && c.IsCode(id));
                if (match != null) return match;
            }

            return candidates.FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            bool hasMonster = _exec.Bot.Hand.Any(c => c != null && c.IsMonster() && (c.Id == SkyStrikerZeroExecutor.CardId.Raye || c.Id == SkyStrikerZeroExecutor.CardId.Roze))
                              || _exec.Bot.GetMonsters().Any(c => c != null);

            int[] priorities;
            if (!hasMonster)
            {
                priorities = new[] {
                    SkyStrikerZeroExecutor.CardId.Engage,
                    SkyStrikerZeroExecutor.CardId.Linkage,
                    SkyStrikerZeroExecutor.CardId.Raye,
                    SkyStrikerZeroExecutor.CardId.HornetDrones,
                    SkyStrikerZeroExecutor.CardId.Roze,
                    SkyStrikerZeroExecutor.CardId.WidowAnchor
                };
            }
            else
            {
                priorities = new[] {
                    SkyStrikerZeroExecutor.CardId.Engage,
                    SkyStrikerZeroExecutor.CardId.Linkage,
                    SkyStrikerZeroExecutor.CardId.WidowAnchor,
                    SkyStrikerZeroExecutor.CardId.Lemnisgate,
                    SkyStrikerZeroExecutor.CardId.ForbiddenCrown,
                    SkyStrikerZeroExecutor.CardId.HornetDrones,
                    SkyStrikerZeroExecutor.CardId.TheFallenAndTheVirtuous,
                    SkyStrikerZeroExecutor.CardId.Raye
                };
            }

            foreach (int id in priorities)
            {
                var match = candidates.FirstOrDefault(c => c != null && c.IsCode(id));
                if (match != null) return match;
            }

            return candidates.FirstOrDefault();
        }
    }

    public class SkyStrikerZeroMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly SkyStrikerZeroExecutor _exec;
        public SkyStrikerZeroMaterialEvaluator(SkyStrikerZeroExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 999;

            // Stolen enemy monster via Widow Anchor: Use FIRST!
            if (card.Controller == 1 || (card.EquipCards != null && card.EquipCards.Any()))
                return 0;

            // Hornet Drone Token: Perfect disposable material
            if (card.IsCode(SkyStrikerZeroExecutor.CardId.Token))
                return 5;

            // Roze on field: High disposal priority
            if (card.IsCode(SkyStrikerZeroExecutor.CardId.Roze))
                return 20;

            // Kagari, Hayate, Kaina whose on-field trigger resolved
            if (card.IsCode(SkyStrikerZeroExecutor.CardId.Kagari) ||
                card.IsCode(SkyStrikerZeroExecutor.CardId.Hayate) ||
                card.IsCode(SkyStrikerZeroExecutor.CardId.Kaina))
                return 40;

            // Raye: Floats from GY, but keep on field if possible
            if (card.IsCode(SkyStrikerZeroExecutor.CardId.Raye))
                return 50;

            // Shizuku: Valuable End Phase search
            if (card.IsCode(SkyStrikerZeroExecutor.CardId.Shizuku))
                return 70;

            // Protected boss monsters: Never link away generically
            if (card.IsCode(SkyStrikerZeroExecutor.CardId.Zero) ||
                card.IsCode(SkyStrikerZeroExecutor.CardId.SPLittleKnight) ||
                card.IsCode(SkyStrikerZeroExecutor.CardId.SuperStarslayerTYPHON))
                return 999;

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

            // Priority for Radiant Typhoon Vision discard or card cost
            int[] discardPriority = {
                SkyStrikerZeroExecutor.CardId.Lemnisgate, // Has GY trigger
                SkyStrikerZeroExecutor.CardId.TheFallenAndTheVirtuous,
                SkyStrikerZeroExecutor.CardId.MysticalSpaceTyphoon,
                SkyStrikerZeroExecutor.CardId.Roze,
                SkyStrikerZeroExecutor.CardId.Raye,
                SkyStrikerZeroExecutor.CardId.PotOfDesires
            };

            foreach (int id in discardPriority)
            {
                var match = candidates.FirstOrDefault(c => c != null && c.IsCode(id));
                if (match != null) return match;
            }

            // Quick-Play Spells for Radiant Typhoon Vision
            var qp = candidates.FirstOrDefault(c => c != null && c.IsSpell() && c.HasType(CardType.QuickPlay));
            if (qp != null) return qp;

            return candidates.OrderBy(c => c.Attack).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(GetMaterialCost).FirstOrDefault();
        }
    }

    public class SkyStrikerZeroThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly SkyStrikerZeroExecutor _exec;
        public SkyStrikerZeroThreatEvaluator(SkyStrikerZeroExecutor exec) => _exec = exec;

        private static readonly int[] HeavyFloodgates = {
            82732705, // Skill Drain
            58921041, // Mystic Mine
            39239728, // Anti-Spell Fragrance
            73125233, // Imperial Order
            99510761, // Vanity's Emptiness
            40605147, // Solemn Strike
            41420027  // Solemn Judgment
        };

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null) return 0;
            if (HeavyFloodgates.Contains(card.Id)) return 100;
            if (card.IsMonster() && card.IsFaceup() && card.Attack >= 2500) return 70;
            if (card.IsMonster() && card.IsFaceup() && !card.IsDisabled()) return 50;
            return 10;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null) return false;
            return HeavyFloodgates.Contains(card.Id) || (card.IsMonster() && card.Attack >= 3000);
        }
    }
}
