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
    // DECOUPLED DOMAIN PLUGIN ARCHITECTURE: YummyPlugin
    // Implements Strategy, MaterialEvaluator, and ThreatEvaluator
    // ═══════════════════════════════════════════════════════════════════════════
    public class YummyPlugin : DeckPluginBase
    {
        private readonly _2026_YummyExecutor _exec;

        public override string DeckName => "2026_Yummy";

        public YummyStrategy StrategyImpl { get; }
        public YummyMaterialEvaluator MaterialImpl { get; }
        public YummyThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public YummyPlugin(_2026_YummyExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new YummyStrategy(exec);
            MaterialImpl = new YummyMaterialEvaluator(exec);
            ThreatImpl = new YummyThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class YummyStrategy : IDeckStrategy
    {
        private readonly _2026_YummyExecutor _exec;
        public YummyStrategy(_2026_YummyExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Opponent Turn: Tag-out or Spright Elf quick revival
            if (_exec.Duel.Player == 1)
            {
                // Spright Elf reviving Synchro Boss
                var synchroBoss = candidates
                    .Where(c => c != null && c.IsFaceup() && (c.Id == _2026_YummyExecutor.CardId.CookyYummyWay || c.Id == _2026_YummyExecutor.CardId.CupsyYummyWay))
                    .OrderByDescending(c => c.Id == _2026_YummyExecutor.CardId.CookyYummyWay ? 100 : 80)
                    .FirstOrDefault();
                if (synchroBoss != null) return synchroBoss;

                // Tag-out SS from GY (triggers on-summon effects):
                // 1) Cooky Yummy: Destroys 1 monster when SS by Synchro
                var cooky = candidates.FirstOrDefault(c => c != null && c.Id == _2026_YummyExecutor.CardId.CookyYummy);
                if (cooky != null && _exec.Enemy.GetMonsterCount() > 0) return cooky;

                // 2) Marshmao: Places Acroquey / Mignon from Deck
                var marshmao = candidates.FirstOrDefault(c => c != null && c.Id == _2026_YummyExecutor.CardId.MarshmaoYummy);
                if (marshmao != null) return marshmao;

                // 3) Lollipo: Banishes 1 card from opponent GY
                var lollipo = candidates.FirstOrDefault(c => c != null && c.Id == _2026_YummyExecutor.CardId.LollipoYummy);
                if (lollipo != null && _exec.Enemy.Graveyard.Count > 0) return lollipo;

                // 4) Cupsy: Draws 1 card
                var cupsy = candidates.FirstOrDefault(c => c != null && c.Id == _2026_YummyExecutor.CardId.CupsyYummy);
                if (cupsy != null) return cupsy;
            }

            // Our Turn: Mignon / Spright Elf revival
            if (_exec.Duel.Player == 0)
            {
                var synchroTarget = candidates
                    .Where(c => c != null && (c.Id == _2026_YummyExecutor.CardId.CupsyYummyWay || c.Id == _2026_YummyExecutor.CardId.CookyYummyWay))
                    .OrderByDescending(c => c.Id == _2026_YummyExecutor.CardId.CupsyYummyWay ? 100 : 80)
                    .FirstOrDefault();
                if (synchroTarget != null) return synchroTarget;

                var mainYummy = candidates
                    .Where(c => c != null && _2026_YummyExecutor.YummyMonsters.Contains(c.Id))
                    .OrderByDescending(c => c.Id == _2026_YummyExecutor.CardId.MarshmaoYummy ? 100 :
                                           (c.Id == _2026_YummyExecutor.CardId.CupsyYummy ? 80 :
                                           (c.Id == _2026_YummyExecutor.CardId.CookyYummy ? 60 : 40)))
                    .FirstOrDefault();
                if (mainYummy != null) return mainYummy;
            }

            return candidates.FirstOrDefault(c => c != null);
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Illusion of Chaos context: Always search Magicians' Souls
            if (context != null && context.IsCode(_2026_YummyExecutor.CardId.IllusionOfChaos))
            {
                var souls = candidates.FirstOrDefault(c => c != null && c.Id == _2026_YummyExecutor.CardId.MagiciansSouls);
                if (souls != null) return souls;
            }

            // Piri Reis Map context: Search 0-ATK Lv1 starter
            if (context != null && context.IsCode(_2026_YummyExecutor.CardId.PiriReisMap))
            {
                var cupsy = candidates.FirstOrDefault(c => c != null && c.Id == _2026_YummyExecutor.CardId.CupsyYummy);
                if (cupsy != null) return cupsy;

                var marshmao = candidates.FirstOrDefault(c => c != null && c.Id == _2026_YummyExecutor.CardId.MarshmaoYummy);
                if (marshmao != null) return marshmao;
            }

            // In-archetype Cupsy Yummy on-summon search:
            // 1st: Yummy Surprise (Trap disruption) if not already held or set
            if (!_exec.Bot.HasInSpellZone(_2026_YummyExecutor.CardId.YummySurprise) && !_exec.Bot.HasInHand(_2026_YummyExecutor.CardId.YummySurprise))
            {
                var trap = candidates.FirstOrDefault(c => c != null && c.Id == _2026_YummyExecutor.CardId.YummySurprise);
                if (trap != null) return trap;
            }

            // 2nd: Extenders not yet held in hand
            var unheldCooky = candidates.FirstOrDefault(c => c != null && c.Id == _2026_YummyExecutor.CardId.CookyYummy && !_exec.Bot.HasInHand(_2026_YummyExecutor.CardId.CookyYummy));
            if (unheldCooky != null) return unheldCooky;

            var unheldLollipo = candidates.FirstOrDefault(c => c != null && c.Id == _2026_YummyExecutor.CardId.LollipoYummy && !_exec.Bot.HasInHand(_2026_YummyExecutor.CardId.LollipoYummy));
            if (unheldLollipo != null) return unheldLollipo;

            var unheldMarshmao = candidates.FirstOrDefault(c => c != null && c.Id == _2026_YummyExecutor.CardId.MarshmaoYummy && !_exec.Bot.HasInHand(_2026_YummyExecutor.CardId.MarshmaoYummy));
            if (unheldMarshmao != null) return unheldMarshmao;

            // 3rd: Field Spells if not active
            if (!_exec.Bot.HasInSpellZone(_2026_YummyExecutor.CardId.YummyusmentMignon))
            {
                var mignon = candidates.FirstOrDefault(c => c != null && c.Id == _2026_YummyExecutor.CardId.YummyusmentMignon);
                if (mignon != null) return mignon;
            }

            return candidates.FirstOrDefault(c => c != null);
        }
    }

    public class YummyMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly _2026_YummyExecutor _exec;
        public YummyMaterialEvaluator(_2026_YummyExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 999;

            // Ace monsters on field: protect from being sent to GY
            if (_exec.IsAceCard(card))
            {
                if (card.Location == CardLocation.MonsterZone && card.IsFaceup())
                {
                    if (card.IsCode(_2026_YummyExecutor.CardId.SprightElf,
                                    _2026_YummyExecutor.CardId.HeraldOfTheArcLight,
                                    _2026_YummyExecutor.CardId.SPLittleKnight,
                                    _2026_YummyExecutor.CardId.DivineArsenalAAZEUSSkyThunder))
                        return 10000;

                    // Level 2 Synchros can be used as Link Material for Spright Elf if Elf is not on field
                    if (_2026_YummyExecutor.YummySynchros.Contains(card.Id))
                    {
                        if (!_exec.Bot.HasInMonstersZone(_2026_YummyExecutor.CardId.SprightElf))
                            return 250;
                        return 8000;
                    }
                }
                return 900;
            }

            // Handtraps: keep on hand
            if (card.IsCode(_2026_YummyExecutor.CardId.AshBlossomAndJoyousSpring,
                            _2026_YummyExecutor.CardId.GhostBelleAndHauntedMansion,
                            _2026_YummyExecutor.CardId.EffectVeiler,
                            _2026_YummyExecutor.CardId.MaxxC,
                            _2026_YummyExecutor.CardId.DrollAndLockBird,
                            _2026_YummyExecutor.CardId.MulcharmyFuwalos))
                return 800;

            // Low-cost / Fodder:
            if (card.IsCode(_2026_YummyExecutor.CardId.YummySnatchy)) return 20;
            if (card.IsCode(_2026_YummyExecutor.CardId.Sangan)) return 25;
            if (card.IsCode(_2026_YummyExecutor.CardId.MagiciansSouls, _2026_YummyExecutor.CardId.JesterConfit)) return 30;
            if (card.IsCode(52340445, 26077387)) return 10; // Tokens
            if (_2026_YummyExecutor.YummyMonsters.Contains(card.Id)) return 100;

            return 50;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.Where(c => c != null).OrderBy(GetMaterialCost).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;

            return candidates.Where(c => c != null)
                .OrderBy(c => {
                    if (c.IsCode(_2026_YummyExecutor.CardId.IllusionOfChaos)) return 5;
                    if (c.IsCode(_2026_YummyExecutor.CardId.PiriReisMap) && (_exec.Bot.LifePoints <= 4000 || _exec.Bot.GetMonsterCount() > 0)) return 10;
                    if (c.IsCode(_2026_YummyExecutor.CardId.JesterConfit) && _exec.Bot.GetMonsterCount() > 0) return 15;
                    if (c.IsCode(_2026_YummyExecutor.CardId.YummyusmentMignon) && _exec.Bot.HasInSpellZone(_2026_YummyExecutor.CardId.YummyusmentMignon)) return 18;
                    if (_2026_YummyExecutor.YummyMonsters.Contains(c.Id) && _exec.Bot.Hand.Count(h => h.Id == c.Id) > 1) return 20;
                    if (c.IsCode(_2026_YummyExecutor.CardId.TripleTacticsTalent) && _exec.Duel.Player == 1) return 22;
                    if (c.IsCode(_2026_YummyExecutor.CardId.CookyYummy) && _exec.CookyHandSSUsed) return 25;
                    if (c.IsCode(_2026_YummyExecutor.CardId.LollipoYummy) && _exec.LollipoHandSSUsed) return 30;
                    if (c.IsCode(_2026_YummyExecutor.CardId.EffectVeiler, _2026_YummyExecutor.CardId.GhostBelleAndHauntedMansion)) return 60;
                    if (c.IsCode(_2026_YummyExecutor.CardId.AshBlossomAndJoyousSpring, _2026_YummyExecutor.CardId.MaxxC)) return 90;
                    return 50;
                })
                .FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            return candidates?.OrderBy(GetMaterialCost).FirstOrDefault();
        }
    }

    public class YummyThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly _2026_YummyExecutor _exec;
        public YummyThreatEvaluator(_2026_YummyExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null || card.Controller == 0) return 0;

            int score = 0;
            // Floodgates / Continuous Threats
            if (card.IsCode(48680970, 48770333)) score += 25000; // Eternal Soul, True Light
            if (card.IsCode(82732705)) score += 20000; // Skill Drain
            if (card.IsCode(41420027)) score += 18000; // Solemn Judgment
            if (card.IsCode(31444249)) score += 15000; // There Can Be Only One
            if (card.IsCode(90448279)) score += 15000; // AA-ZEUS
            if (card.IsCode(21887175)) score += 12000; // Avramax

            if (card.IsMonster())
            {
                if (card.IsFaceup() && !card.IsDisabled())
                {
                    if (card.Attack >= 2500) score += 5000;
                    if (card.HasType(CardType.Fusion) || card.HasType(CardType.Synchro) || card.HasType(CardType.Xyz) || card.HasType(CardType.Link)) score += 3000;
                }
                score += card.Attack;
            }

            if (card.IsSpell() || card.IsTrap())
            {
                if (card.IsFaceup() && (card.HasType(CardType.Continuous) || card.HasType(CardType.Field))) score += 6000;
                else score += 2000;
            }

            return score;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null || card.Controller == 0) return false;
            if (card.IsCode(48680970, 48770333, 82732705, 31444249, 90448279)) return true;
            if (card.IsMonster() && card.IsAttack() && card.Attack >= _exec.Bot.LifePoints) return true;
            return false;
        }
    }
}
