using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Plugin;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("Magical Cylinder", "MagicalCylinder")]
    [Deck("MagicalCylinder", "MagicalCylinder")]
    public class MagicalCylinderExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Core Reflect Traps
            public const int MagicCylinder = 62279055;
            public const int MagicalCylinders = 15943341;
            public const int DimensionWall = 67095270;
            public const int BattleMania = 31245780;
            public const int RingOfDestruction = 83555666;

            // Searchers & Consistency Traps
            public const int TrapTrick = 80101899;

            // Counter Fortress Traps
            public const int SolemnJudgment = 41420027;
            public const int SolemnStrike = 40605147;
            public const int DarkBribe = 77538567;
            public const int WakingTheDragon = 10813327;

            // Monsters
            public const int LordOfTheHeavenlyPrison = 9822220;
            public const int LilithLadyOfLament = 23898021;
            public const int Wannabee = 3248469;
            public const int JizukiruTheStarDestroyingKaiju = 63941210;
            public const int DogoranTheMadFlameKaiju = 93332803;

            // Spells
            public const int PotOfDuality = 98645731;
            public const int PotOfProsperity = 84211599;
            public const int InterruptedKaijuSlumber = 99330325;

            // Extra Deck
            public const int SuperStarslayerTYPHONSkyCrisis = 93039339;
            public const int DivineArsenalAAZEUS = 90448279;
            public const int SPLittleKnight = 29301450;
            public const int KnightmarePhoenix = 2857636;
            public const int KnightmareUnicorn = 38342335;
            public const int AbyssDweller = 21044178;
            public const int Bagooska = 90590303;
            public const int GustavMax = 56910167;
            public const int JuggernautLiebe = 26096328;
            public const int AccesscodeTalker = 86066372;
            public const int RaidraptorUltimateFalcon = 86221741;
            public const int TheLastWarriorFromAnotherPlanet = 86099788;
            public const int BaronneDeFleur = 84815190;
        }

        // Standard OCG Hint IDs
        private const long HINT_SELECT_DESTROY = 502;
        private const long HINT_SELECT_REMOVE = 503;
        private const long HINT_SELECT_ATOHAND = 506;
        private const long HINT_SELECT_SPSUMMON = 509;
        private const long HINT_SELECT_SET = 510;

        private bool _heavenlyPrisonRevealed = false;
        private bool _battleManiaUsed = false;
        private bool _lilithUsed = false;
        private bool _dualityUsed = false;
        private bool _prosperityUsed = false;

        public MagicalCylinderPlugin Plugin { get; }

        public MagicalCylinderExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            Plugin = new MagicalCylinderPlugin(this);
            DeckPlugin = Plugin;

            ResourcePlan.RegisterAceCards(
                CardId.LordOfTheHeavenlyPrison,
                CardId.GustavMax,
                CardId.JuggernautLiebe,
                CardId.SuperStarslayerTYPHONSkyCrisis,
                CardId.RaidraptorUltimateFalcon,
                CardId.TheLastWarriorFromAnotherPlanet,
                CardId.BaronneDeFleur
            );

            BaitPlanner.RegisterComboStarters(
                CardId.PotOfDuality,
                CardId.PotOfProsperity
            );

            // ═══════════════════════════════════════════════════════════════
            //  EXECUTORS PIPELINE (Tiered Priority Execution)
            // ═══════════════════════════════════════════════════════════════

            // ── Tier 0: Counter Traps & Backrow Defense (Spell Speed 3 & Floating Mines) ──
            AddExecutor(ExecutorType.Activate, CardId.WakingTheDragon);
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, SolemnJudgmentEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarkBribe, DarkBribeEffect);
            AddExecutor(ExecutorType.Activate, CardId.SolemnStrike, SolemnStrikeEffect);

            // ── Tier 1: Double Damage Reflect & Attack Interceptions (The Core Strike) ──
            AddExecutor(ExecutorType.Activate, CardId.MagicalCylinders, MagicalCylindersEffect);
            AddExecutor(ExecutorType.Activate, CardId.MagicCylinder, MagicCylinderEffect);
            AddExecutor(ExecutorType.Activate, CardId.DimensionWall, DimensionWallEffect);
            AddExecutor(ExecutorType.Activate, CardId.RingOfDestruction, RingOfDestructionEffect);

            // ── Tier 2: Forced Attack Triggers & Kaiju Invasions ──
            AddExecutor(ExecutorType.Activate, CardId.BattleMania, BattleManiaEffect);
            AddExecutor(ExecutorType.Activate, CardId.InterruptedKaijuSlumber, KaijuSlumberEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.JizukiruTheStarDestroyingKaiju, KaijuSummonEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.DogoranTheMadFlameKaiju, KaijuSummonEffect);

            // ── Tier 3: Trap Trick & Search Acceleration ──
            AddExecutor(ExecutorType.Activate, CardId.TrapTrick, TrapTrickEffect);
            AddExecutor(ExecutorType.Activate, CardId.LilithLadyOfLament, LilithEffect);
            AddExecutor(ExecutorType.Activate, CardId.LordOfTheHeavenlyPrison, HeavenlyPrisonEffect);
            AddExecutor(ExecutorType.Activate, CardId.Wannabee, WannabeeEffect);

            // ── Tier 4: Draw & Excavation Spells ──
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, PotOfProsperityEffect);
            AddExecutor(ExecutorType.Activate, CardId.PotOfDuality, PotOfDualityEffect);

            // ── Tier 5: Normal Summons ──
            AddExecutor(ExecutorType.Summon, CardId.LilithLadyOfLament, LilithSummon);
            AddExecutor(ExecutorType.Summon, CardId.Wannabee, WannabeeSummon);

            // ── Tier 6: Extra Deck Finishers (Rank 10 from Lord of Heavenly Prison & Waking Payoffs) ──
            AddExecutor(ExecutorType.SpSummon, CardId.GustavMax, GustavMaxSummon);
            AddExecutor(ExecutorType.Activate, CardId.GustavMax, GustavMaxEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.JuggernautLiebe);
            AddExecutor(ExecutorType.Activate, CardId.JuggernautLiebe);
            AddExecutor(ExecutorType.SpSummon, CardId.SuperStarslayerTYPHONSkyCrisis);
            AddExecutor(ExecutorType.Activate, CardId.BaronneDeFleur);
            AddExecutor(ExecutorType.Activate, CardId.RaidraptorUltimateFalcon);

            // ── Tier 7: Spell & Trap Sets ──
            AddExecutor(ExecutorType.SpellSet, CardId.WakingTheDragon, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.MagicalCylinders, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.MagicCylinder, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.DimensionWall, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.BattleMania, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.TrapTrick, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnJudgment, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.DarkBribe, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnStrike, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.RingOfDestruction, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet);

            // Smart Repos
            AddExecutor(ExecutorType.Repos, SmartMonsterRepos);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _heavenlyPrisonRevealed = false;
            _battleManiaUsed = false;
            _lilithUsed = false;
            _dualityUsed = false;
            _prosperityUsed = false;
        }

        public override bool OnSelectHand()
        {
            // Prefer going first to set up the Fortress of Reflect Traps
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  ACTIVATION & COMBAT LOGIC
        // ═══════════════════════════════════════════════════════════════

        private bool SolemnJudgmentEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 1)
            {
                // Always intercept mass backrow wipes!
                if (IsMassBackrowWipe(LastChainCard)) return true;

                // Counter dangerous Spells / Traps
                if (LastChainCard.IsSpell() || LastChainCard.IsTrap()) return true;

                // Counter dangerous Boss Monster effects
                if (LastChainCard.IsMonster() && LastChainCard.Attack >= 2500) return true;
            }

            // Negate Special Summon of High-Impact monsters
            if (Duel.LastSummonPlayer == 1)
            {
                var summoned = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault();
                if (summoned != null && (summoned.Attack >= 2800 || summoned.Level >= 8 || summoned.Rank >= 8 || summoned.LinkCount >= 3))
                    return true;
            }

            return false;
        }

        private bool DarkBribeEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 1)
            {
                // Protect our set backrow from destruction / removal
                if (LastChainCard.IsSpell() || LastChainCard.IsTrap())
                {
                    return true;
                }
            }
            return false;
        }

        private bool SolemnStrikeEffect()
        {
            if (Bot.LifePoints <= 1500) return false;

            if (LastChainCard != null && LastChainCard.Controller == 1 && LastChainCard.IsMonster())
            {
                // Negate dangerous monster effect
                return true;
            }

            if (Duel.LastSummonPlayer == 1)
            {
                var summoned = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault();
                if (summoned != null && summoned.Attack >= 2400)
                    return true;
            }

            return false;
        }

        private bool MagicalCylindersEffect()
        {
            // 1. Graveyard Effect: Banish to DOUBLE Magic Cylinder's damage!
            if (Card.Location == CardLocation.Grave)
            {
                // Always double the damage when Magic Cylinder resolves!
                return true;
            }

            // 2. Field Activation: Set 1 "Magic Cylinder" from Deck or GY
            if (Card.Location == CardLocation.SpellZone)
            {
                // Set directly to field if we have room
                return Bot.GetSpellCount() < 5;
            }

            return false;
        }

        private bool MagicCylinderEffect()
        {
            // Only triggers when opponent declares an attack
            if (Duel.Phase != DuelPhase.BattleStart && Duel.Phase != DuelPhase.BattleStep && Duel.Phase != DuelPhase.Battle)
                return false;

            // HARD RULE: Do not chain Magic Cylinder or Dimension Wall if one is already active in current chain!
            // "negate the attack, and if you do, inflict damage": if attack is already negated, 0 damage!
            if (Duel.CurrentChain.Any(c => c.IsCode(CardId.MagicCylinder, CardId.DimensionWall)))
                return false;

            ClientCard attacker = Enemy.BattlingMonster;
            if (attacker == null) return false;

            // Do not waste Magic Cylinder on zero or low ATK (unless opponent is nearly dead)
            if (attacker.Attack <= 500 && Enemy.LifePoints > 1000)
                return false;

            // If we have Magical Cylinders in GY, the damage is DOUBLED!
            // 3000 ATK -> 6000 damage; 4000 ATK -> 8000 OTK!
            return true;
        }

        private bool DimensionWallEffect()
        {
            // Triggers when opponent declares an attack
            if (Duel.Phase != DuelPhase.BattleStart && Duel.Phase != DuelPhase.BattleStep && Duel.Phase != DuelPhase.Battle)
                return false;

            // HARD RULE: Do not chain Dimension Wall or Magic Cylinder if one is already active in current chain!
            if (Duel.CurrentChain.Any(c => c.IsCode(CardId.MagicCylinder, CardId.DimensionWall)))
                return false;

            ClientCard attacker = Enemy.BattlingMonster;
            if (attacker == null) return false;

            // Dimension Wall reflects battle damage (Non-Targeting!)
            // Highly effective if attacker has high ATK
            return attacker.Attack >= 1500 || attacker.Attack >= Enemy.LifePoints;
        }

        private bool RingOfDestructionEffect()
        {
            if (Enemy.GetMonsterCount() == 0) return false;

            // Target face-up opponent monster whose ATK <= opponent's LP and will not kill us
            var target = Enemy.GetMonsters()
                .Where(m => m != null && m.IsFaceup() && m.Attack <= Enemy.LifePoints && m.Attack < Bot.LifePoints)
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();

            if (target != null && target.Attack >= 2000)
            {
                return true;
            }

            return false;
        }

        private bool BattleManiaEffect()
        {
            // Activate during opponent's Standby Phase!
            if (Duel.Player == 1 && Duel.Phase == DuelPhase.Standby)
            {
                if (_battleManiaUsed) return false;

                // Only activate if opponent controls monsters and we have reflect traps set!
                bool hasReflectTrap = Bot.GetSpells().Any(s => s != null && s.IsFacedown() &&
                    (s.IsCode(CardId.MagicCylinder) || s.IsCode(CardId.DimensionWall) || s.IsCode(CardId.MagicalCylinders)));

                if (Enemy.GetMonsterCount() > 0 && hasReflectTrap)
                {
                    _battleManiaUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool KaijuSlumberEffect()
        {
            if (Enemy.GetMonsterCount() >= 2 && Bot.GetMonsterCount() == 0)
            {
                return true;
            }
            return false;
        }

        private bool KaijuSummonEffect()
        {
            // Summon Kaiju to opponent's field by tributing their biggest threat!
            if (Enemy.GetMonsterCount() > 0)
            {
                var biggestThreat = Enemy.GetMonsters()
                    .OrderByDescending(m => CardIntelligence.IsFloodgate(m.Id) ? 10000 : 0)
                    .ThenByDescending(m => CardIntelligence.IsKnownNegator(m.Id) ? 8000 : 0)
                    .ThenByDescending(m => m.Attack)
                    .FirstOrDefault();

                if (biggestThreat != null && (biggestThreat.Attack >= 2500 || CardIntelligence.IsKnownNegator(biggestThreat.Id)))
                {
                    return true;
                }
            }
            return false;
        }

        private bool TrapTrickEffect()
        {
            // Activate during opponent's turn to fetch key traps
            if (Duel.Player == 1)
            {
                // If in Standby Phase and opponent has monsters, fetch Battle Mania!
                if (Duel.Phase == DuelPhase.Standby && Enemy.GetMonsterCount() > 0 && !_battleManiaUsed)
                {
                    return true;
                }

                // If in Main Phase or Battle Phase, fetch Magical Cylinders or Dimension Wall
                if (Duel.IsMainPhase() || Duel.Phase == DuelPhase.BattleStart)
                {
                    return true;
                }
            }
            return false;
        }

        private bool LilithEffect()
        {
            if (_lilithUsed) return false;

            // Quick effect: Tribute 1 DARK monster (itself) to reveal 3 Normal Traps from deck and set 1!
            if (Bot.GetSpellCount() < 5)
            {
                _lilithUsed = true;
                return true;
            }
            return false;
        }

        private bool HeavenlyPrisonEffect()
        {
            // 1. Reveal in hand during Main Phase to protect all set cards from destruction!
            if (Card.Location == CardLocation.Hand && Duel.IsMainPhase())
            {
                if (!_heavenlyPrisonRevealed && Bot.GetSpells().Any(s => s != null && s.IsFacedown()))
                {
                    _heavenlyPrisonRevealed = true;
                    return true;
                }
            }

            // 2. Special summon when a set card is activated!
            if (Card.Location == CardLocation.Hand && _heavenlyPrisonRevealed)
            {
                return true;
            }

            return false;
        }

        private bool WannabeeEffect()
        {
            // End Phase in hand: send to GY to excavate 5 cards and set 1 Trap!
            if (Card.Location == CardLocation.Hand && Duel.Phase == DuelPhase.End)
            {
                return Bot.GetSpellCount() < 5;
            }
            return false;
        }

        private bool PotOfProsperityEffect()
        {
            if (_prosperityUsed || _dualityUsed) return false;
            _prosperityUsed = true;
            return true;
        }

        private bool PotOfDualityEffect()
        {
            if (_dualityUsed) return false;
            _dualityUsed = true;
            return true;
        }

        private bool LilithSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool WannabeeSummon()
        {
            return Bot.GetMonsterCount() == 0 && !Bot.HasInHand(CardId.LilithLadyOfLament);
        }

        private bool GustavMaxSummon()
        {
            // Summon Gustav Max using two Level 10 Lord of the Heavenly Prisons!
            return Bot.GetMonsters().Count(m => m != null && m.IsFaceup() && m.Level == 10) >= 2;
        }

        private bool GustavMaxEffect()
        {
            // Detach 1 material to burn 2000 damage!
            return true;
        }

        private bool SmartMonsterRepos()
        {
            if (Card == null) return false;

            // Wannabee / 0 ATK walls in Attack -> Defense!
            if (Card.IsAttack() && Card.Attack == 0) return true;

            // Lord of Heavenly Prison (3000 ATK) in Defense during our turn -> Attack!
            if (Card.IsDefense() && Card.Attack >= 3000 && Duel.Player == 0 && Duel.Phase == DuelPhase.Main1)
            {
                return true;
            }

            return false;
        }

        private static bool IsMassBackrowWipe(ClientCard card)
        {
            if (card == null) return false;
            int id = card.Id;
            return id == 18144506   // Harpie's Feather Duster
                || id == 18144507   // Harpie's Feather Duster (Alt)
                || id == 14532163   // Lightning Storm
                || id == 15693423   // Evenly Matched
                || id == 43898403   // Twin Twisters
                || id == 19613556   // Heavy Storm
                || id == 8267140    // Cosmic Cyclone
                || id == 23002292;  // Red Reboot
        }

        // ═══════════════════════════════════════════════════════════════
        //  CARD & TARGET SELECTION OVERRIDES (OnSelectCard)
        // ═══════════════════════════════════════════════════════════════

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return base.OnSelectCard(cards, min, max, hint, cancelable);

            // 0. Select Card to Special Summon (Waking the Dragon / Extra Deck)
            if (hint == HINT_SELECT_SPSUMMON)
            {
                var wakingTargets = new List<int>
                {
                    CardId.TheLastWarriorFromAnotherPlanet,
                    CardId.RaidraptorUltimateFalcon,
                    CardId.BaronneDeFleur,
                    CardId.SuperStarslayerTYPHONSkyCrisis,
                    CardId.GustavMax
                };

                var sorted = cards.OrderBy(c => {
                    int idx = wakingTargets.IndexOf(c.Id);
                    return idx >= 0 ? idx : 999;
                }).ToList();

                int targetCount = Math.Max(min, 1);
                targetCount = Math.Min(targetCount, max);
                targetCount = Math.Min(targetCount, sorted.Count);

                if (targetCount >= min)
                    return sorted.Take(targetCount).ToList();
            }

            // 1. Select Card to Set from Deck (Magical Cylinders / Trap Trick / Lord of Heavenly Prison / Lilith)
            if (hint == HINT_SELECT_SET)
            {
                var priorityOrder = new List<int>
                {
                    CardId.MagicalCylinders,
                    CardId.MagicCylinder,
                    CardId.DimensionWall,
                    CardId.WakingTheDragon,
                    CardId.BattleMania,
                    CardId.TrapTrick,
                    CardId.SolemnJudgment,
                    CardId.DarkBribe,
                    CardId.SolemnStrike,
                    CardId.RingOfDestruction
                };

                // If min == 3 (Lilith, Lady of Lament reveals 3 normal traps)
                // If we have 3 copies of Magical Cylinders, revealing all 3 guarantees opponent picks it!
                if (min == 3)
                {
                    foreach (int id in priorityOrder)
                    {
                        var matching = cards.Where(c => c.Id == id).ToList();
                        if (matching.Count >= 3)
                        {
                            return matching.Take(3).ToList();
                        }
                    }
                }

                // General sorted pick by priority
                var sorted = cards.OrderBy(c => {
                    int idx = priorityOrder.IndexOf(c.Id);
                    return idx >= 0 ? idx : 999;
                }).ToList();

                int targetCount = Math.Max(min, 1);
                targetCount = Math.Min(targetCount, max);
                targetCount = Math.Min(targetCount, sorted.Count);

                if (targetCount >= min)
                    return sorted.Take(targetCount).ToList();
            }

            // 2. Select Card to Add to Hand (Duality / Prosperity / Searchers)
            if (hint == HINT_SELECT_ATOHAND)
            {
                var priorityHand = new List<int>
                {
                    CardId.LordOfTheHeavenlyPrison,
                    CardId.MagicalCylinders,
                    CardId.MagicCylinder,
                    CardId.TrapTrick,
                    CardId.WakingTheDragon,
                    CardId.SolemnJudgment,
                    CardId.DarkBribe,
                    CardId.SolemnStrike,
                    CardId.BattleMania,
                    CardId.DimensionWall,
                    CardId.RingOfDestruction
                };

                var sorted = cards.OrderBy(c => {
                    int idx = priorityHand.IndexOf(c.Id);
                    return idx >= 0 ? idx : 999;
                }).ToList();

                int targetCount = Math.Max(min, 1);
                targetCount = Math.Min(targetCount, max);
                targetCount = Math.Min(targetCount, sorted.Count);

                if (targetCount >= min)
                    return sorted.Take(targetCount).ToList();
            }

            // 3. Select Card to Destroy / Remove
            if (hint == HINT_SELECT_DESTROY || hint == HINT_SELECT_REMOVE)
            {
                var enemyTarget = cards.Where(c => c.Controller == 1)
                    .OrderByDescending(c => CardIntelligence.IsFloodgate(c.Id) ? 10000 : 0)
                    .ThenByDescending(c => CardIntelligence.IsKnownNegator(c.Id) ? 8000 : 0)
                    .ThenByDescending(c => c.Attack)
                    .ToList();

                if (enemyTarget.Count >= min)
                {
                    int targetCount = Math.Min(max, enemyTarget.Count);
                    return enemyTarget.Take(targetCount).ToList();
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            // When Magical Cylinders in GY prompts: "Banish this card to double the damage?", ALWAYS YES!
            if (card != null && card.Controller == 0 && card.Id == CardId.MagicalCylinders)
            {
                return true;
            }
            return base.OnSelectEffectYn(card, desc);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Lord of Heavenly Prison -> Prefer Defense Position initially (3000 DEF wall)
            if (cardId == CardId.LordOfTheHeavenlyPrison)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                if (positions.Contains(CardPosition.Defence)) return CardPosition.Defence;
            }

            // Wannabee -> Defense
            if (cardId == CardId.Wannabee)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
            }

            return base.OnSelectPosition(cardId, positions);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: MagicalCylinderPlugin
    // ═══════════════════════════════════════════════════════════════
    public class MagicalCylinderPlugin : DeckPluginBase
    {
        private readonly MagicalCylinderExecutor _exec;

        public override string DeckName => "Magical Cylinder";

        public override IDeckStrategy Strategy { get; }
        public override IDeckMaterialEvaluator MaterialEvaluator { get; }
        public override IDeckThreatEvaluator ThreatEvaluator { get; }

        public MagicalCylinderPlugin(MagicalCylinderExecutor exec)
        {
            _exec = exec;
            Strategy = new MagicalCylinderStrategy(exec);
            MaterialEvaluator = new MagicalCylinderMaterialEvaluator(exec);
            ThreatEvaluator = new MagicalCylinderThreatEvaluator(exec);
        }
    }

    public class MagicalCylinderStrategy : IDeckStrategy
    {
        private readonly MagicalCylinderExecutor _exec;
        public MagicalCylinderStrategy(MagicalCylinderExecutor exec) => _exec = exec;
        public void Reset() { }
        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates) => candidates.FirstOrDefault();
        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context) => candidates.FirstOrDefault();
    }

    public class MagicalCylinderMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly MagicalCylinderExecutor _exec;
        public MagicalCylinderMaterialEvaluator(MagicalCylinderExecutor exec) => _exec = exec;
        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 999;
            if (card.Id == MagicalCylinderExecutor.CardId.LordOfTheHeavenlyPrison) return 10000;
            return 100;
        }
        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            return candidates.OrderBy(GetMaterialCost).ToList();
        }
        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            return candidates.OrderBy(GetMaterialCost).FirstOrDefault();
        }
        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            return candidates.FirstOrDefault();
        }
    }

    public class MagicalCylinderThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly MagicalCylinderExecutor _exec;
        public MagicalCylinderThreatEvaluator(MagicalCylinderExecutor exec) => _exec = exec;
        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null || card.Controller == 0) return 0;
            // Backrow mass removal is extreme threat
            if (card.Id == 18144506 || card.Id == 18144507 || card.Id == 14532163 || card.Id == 15693423 ||
                card.Id == 43898403 || card.Id == 19613556 || card.Id == 23002292)
                return 15000;
            return card.Attack >= 2500 ? 5000 : 2000;
        }
        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null || card.Controller == 0) return false;
            return card.Id == 18144506 || card.Id == 18144507 || card.Id == 14532163 || card.Id == 15693423 ||
                   card.Id == 43898403 || card.Id == 19613556 || card.Id == 23002292;
        }
    }
}
