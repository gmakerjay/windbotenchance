using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Plugin;
using WindBot.Game.AI.Plugins;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    // ====================================================================================================
    // CARD AUDIT — GrenMaju (Danger! Kaiju Level 8 Turbo & Rank 8 Numeron Plan B)
    // 100% verified against cards.cdb and GrenMaju.ydk
    // Category: Special (Exotic Win Condition, Unconventional High-Power Mechanics)
    // ====================================================================================================
    // | Card Name                     | Type           | Lv/Rk | ATK  | DEF  | Key Interaction                                         |
    // |-------------------------------|----------------|-------|------|------|---------------------------------------------------------|
    // | Gren Maju Da Eiza             | Fiend/FIRE     | 3     | ?    | ?    | Gains 400 ATK/DEF for EACH banished card (10,000+ ATK)  |
    // | Gizmek Orochi                 | Machine/DARK   | 8     | 2450 | 2450 | Quick SS by banishing 8; banish 3 ED to pop monster     |
    // | Eater of Millions             | Fiend/LIGHT    | 1     | ?    | ?    | SS by banishing 5 ED; banishes enemy monster face-down  |
    // | Danger! Bigfoot!              | Beast/DARK     | 8     | 3000 | 0    | Hand reveal SS/draw; pops face-up enemy card on discard |
    // | Danger! Thunderbird!          | Winged-Beast/D | 8     | 2800 | 2400 | Hand reveal SS/draw; pops Set enemy card on discard     |
    // | Alpha, Master of Beasts       | Beast/EARTH    | 8     | 3000 | 2500 | Free SS going 2nd; non-targeting bounce of enemy faceup |
    // | Lava Golem                    | Fiend/FIRE     | 8     | 3000 | 2500 | Tributes 2 enemy monsters for cost (unnegatable wipe)   |
    // | Gameciel, Sea Turtle Kaiju    | Aqua/WATER     | 8     | 2200 | 3000 | Tributes 1 enemy boss monster (lowest ATK target)       |
    // | Dogoran, Mad Flame Kaiju      | Dinosaur/FIRE  | 8     | 3000 | 1200 | Tributes 1 enemy monster or summons to bot side (Lv 8)  |
    // | Necroface                     | Zombie/DARK    | 4     | 1200 | 1800 | NS recycles all banished; banish mills 5 each player    |
    // | Trade-In                      | Normal Spell   | -     | -    | -    | Discard Lv 8 -> draw 2 (triggers Danger pops cleanly!)  |
    // | Pot of Desires                | Normal Spell   | -     | -    | -    | Banishes 10 cards face-down to draw 2 (+4000 to Maju)   |
    // | Number 97: Draglubion         | Dragon/DARK    | Rk 8  | 3000 | 3000 | Plan B: summons Numeron Dragon from ED + attaches Hope  |
    // | Number 100: Numeron Dragon    | Dragon/LIGHT   | Rk 1  | 0    | 0    | Plan B: 9,000-13,000 ATK nuclear OTK without Gren Maju  |
    // | Number 38: Hope Harbinger     | Dragon/LIGHT   | Rk 8  | 3800 | 2800 | Spell negate + attack redirect                          |
    // | Dingirsu, Orcust Evening Star | Machine/DARK   | Rk 8  | 2600 | 2100 | Non-targeting send to GY + destruction protection       |
    // | Super Polymerization          | Quick-Play Sp  | -     | -    | -    | Unrespondable board fuse using enemy monsters           |
    // | Raigeki                       | Normal Spell   | -     | -    | -    | Destroys all monsters opponent controls                 |
    // | Harpie's Feather Duster       | Normal Spell   | -     | -    | -    | Destroys all Spells/Traps opponent controls             |
    // | Dimensional Fissure           | Continuous Sp  | -     | -    | -    | Banishes all monsters sent to GY                        |
    // | Macro Cosmos                  | Continuous Tr  | -     | -    | -    | Banishes all cards sent to GY                           |
    // ====================================================================================================

    [Deck("GrenMaju", "GrenMaju")]
    public class GrenMajuExecutor : ModernExecutor
    {
        public class CardId
        {
            // Monsters
            public const int GrenMajuDaEiza = 36584821;
            public const int GizmekOrochi = 71197066;
            public const int EaterOfMillions = 63845230;
            public const int DangerBigfoot = 43316238;
            public const int DangerThunderbird = 90807199;
            public const int AlphaTheMasterOfBeasts = 73304257;
            public const int LavaGolem = 102380;
            public const int GamecielTheSeaTurtleKaiju = 55063751;
            public const int DogoranTheMadFlameKaiju = 93332803;
            public const int Necroface = 28297833;

            // Spells
            public const int TradeIn = 38120068;
            public const int PotOfDesires = 35261759;
            public const int InterruptedKaijuSlumber = 99330325;
            public const int Raigeki = 12580477;
            public const int HarpiesFeatherDuster = 18144506;
            public const int SuperPolymerization = 48130397;
            public const int DimensionalFissure = 81674782;

            // Traps
            public const int MacroCosmos = 30241314;
            public const int SolemnJudgment = 41420027;

            // Extra Deck
            public const int Draglubion = 28400508;
            public const int NumeronDragon = 57314798;
            public const int HopeHarbinger = 63767246;
            public const int Dingirsu = 93854893;
            public const int AAZeus = 90448279;
            public const int TYPHON = 93039339;
            public const int Garura = 11765832;
            public const int MudragonOfTheSwamp = 54757758;
            public const int StarvingVenomFusionDragon = 41209827;
            public const int EarthGolemIgnister = 62111090;
            public const int PredaplantDragostapelia = 69946549;
            public const int SPLittleKnight = 29301450;
        }

        public readonly GrenMajuPlugin Plugin;
        private bool _normalSummonUsed = false;
        private bool _numeronPumped = false;

        private static readonly HashSet<int> KaijuIds = new HashSet<int>
        {
            CardId.GamecielTheSeaTurtleKaiju,
            CardId.DogoranTheMadFlameKaiju,
            63941210, // Jizukiru, the Star Destroying Kaiju
            29726552, // Kumongous, the Sticky String Kaiju
            36956512, // Gadarla, the Mystery Dust Kaiju
            28674152, // Radian, the Multidimensional Kaiju
            48770333  // Thunder King, the Lightningstrike Kaiju
        };

        private static bool IsKaijuCard(int id) => KaijuIds.Contains(id);

        public override void OnNewTurn()
        {
            _normalSummonUsed = false;
            _numeronPumped = false;
            base.OnNewTurn();
        }

        public GrenMajuExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            Plugin = new GrenMajuPlugin(this);
            DeckPlugin = Plugin;

            // Register Ace Bosses that must never be sacrificed
            HeuristicGuard.RegisterAceCards(
                CardId.GrenMajuDaEiza,
                CardId.NumeronDragon,
                CardId.Draglubion,
                CardId.HopeHarbinger,
                CardId.Dingirsu,
                CardId.GizmekOrochi
            );

            // ==========================================
            // PRIORITY 1: COUNTER TRAPS
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, SolemnJudgmentEffect);

            // ==========================================
            // PRIORITY 2: MASS BOARD BREAKERS
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, FeatherDusterEffect);
            AddExecutor(ExecutorType.Activate, CardId.Raigeki, RaigekiEffect);
            AddExecutor(ExecutorType.Activate, CardId.InterruptedKaijuSlumber, KaijuSlumberEffect);
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolyEffect);

            // ==========================================
            // PRIORITY 3: KAIJU & LAVA GOLEM TRIBUTES
            // ==========================================
            AddExecutor(ExecutorType.SpSummon, CardId.LavaGolem, LavaGolemSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.GamecielTheSeaTurtleKaiju, GamecielSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DogoranTheMadFlameKaiju, DogoranSummon);

            // ==========================================
            // PRIORITY 4: ALPHA, THE MASTER OF BEASTS
            // ==========================================
            AddExecutor(ExecutorType.SpSummon, CardId.AlphaTheMasterOfBeasts, AlphaSummon);
            AddExecutor(ExecutorType.Activate, CardId.AlphaTheMasterOfBeasts, AlphaEffect);

            // ==========================================
            // PRIORITY 5: TARGETED DRAW & HAND SCULPTING (TRADE-IN & DANGER!)
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.TradeIn, TradeInEffect);
            // Danger! Hand reveal effects
            AddExecutor(ExecutorType.Activate, CardId.DangerBigfoot, DangerHandEffect);
            AddExecutor(ExecutorType.Activate, CardId.DangerThunderbird, DangerHandEffect);

            // ==========================================
            // PRIORITY 6: CONTINUOUS BANISH FLOODGATES
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.DimensionalFissure, DimensionalFissureEffect);
            AddExecutor(ExecutorType.Activate, CardId.MacroCosmos, MacroCosmosEffect);

            // ==========================================
            // PRIORITY 7: BANISH ENGINES (POT OF DESIRES & OROCHI & EATER)
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.PotOfDesires, PotOfDesiresEffect);
            AddExecutor(ExecutorType.Activate, CardId.GizmekOrochi, GizmekOrochiEffect);
            AddExecutor(ExecutorType.Activate, CardId.EaterOfMillions, EaterOfMillionsEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.EaterOfMillions, EaterOfMillionsSummon);

            // ==========================================
            // PRIORITY 8: PLAN B RANK 8 XYZ ENGINE (NUMERON DRAGON OTK)
            // ==========================================
            AddExecutor(ExecutorType.SpSummon, CardId.Draglubion, DraglubionSummon);
            AddExecutor(ExecutorType.Activate, CardId.Draglubion, DraglubionEffect);
            AddExecutor(ExecutorType.Activate, CardId.NumeronDragon, NumeronDragonEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Dingirsu, DingirsuSummon);
            AddExecutor(ExecutorType.Activate, CardId.Dingirsu, DingirsuEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.HopeHarbinger, HopeHarbingerSummon);
            AddExecutor(ExecutorType.Activate, CardId.HopeHarbinger, HopeHarbingerEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.TYPHON, TyphonSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AAZeus, ZeusSummon);

            // ==========================================
            // PRIORITY 9: FINISHER NORMAL SUMMONS
            // ==========================================
            // 9.1 Gren Maju: Nuclear OTK Finisher
            AddExecutor(ExecutorType.Summon, CardId.GrenMajuDaEiza, GrenMajuSummon);
            // 9.2 Necroface: Deck-Out Recovery Safeguard
            AddExecutor(ExecutorType.Summon, CardId.Necroface, NecrofaceSummon);

            // ==========================================
            // PRIORITY 10: SET BACKROW
            // ==========================================
            AddExecutor(ExecutorType.SpellSet, SpellSetPriority);
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        // =========================================================================
        // COUNTER TRAP HANDLERS
        // =========================================================================
        private bool SolemnJudgmentEffect()
        {
            if (Bot.LifePoints <= 1000) return false;
            if (LastChainCard != null && Duel.LastChainPlayer == 1)
            {
                if (LastChainCard.IsSpell() || LastChainCard.IsTrap()) return true;
                if (LastChainCard.IsMonster() && Duel.SummoningCards.Count > 0) return true;
            }
            return Duel.LastChainPlayer == 1;
        }

        // =========================================================================
        // BOARD BREAKER HANDLERS
        // =========================================================================
        private bool FeatherDusterEffect()
        {
            return Enemy.GetSpells().Any(s => s != null);
        }

        private bool RaigekiEffect()
        {
            return Enemy.GetMonsters().Any(m => m != null && m.IsFaceup());
        }

        private bool KaijuSlumberEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // Banish from GY to search a Kaiju
                AI.SelectCard(CardId.GamecielTheSeaTurtleKaiju, CardId.DogoranTheMadFlameKaiju);
                return true;
            }

            if (Card.Location == CardLocation.Hand)
            {
                // Activate on field: wipe board & summon Dogoran to bot, Gameciel to enemy
                if (Enemy.GetMonsterCount() >= 1 || (Bot.GetMonsterCount() == 0 && Bot.Deck.Count >= 2))
                {
                    AI.SelectCard(CardId.DogoranTheMadFlameKaiju);
                    AI.SelectNextCard(CardId.GamecielTheSeaTurtleKaiju);
                    return true;
                }
            }

            return false;
        }

        private bool SuperPolyEffect()
        {
            var enemyMonsters = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup()).ToList();
            if (enemyMonsters.Count < 2) return false;
            return Bot.Hand.Count >= 1; // Needs 1 discard
        }

        // =========================================================================
        // KAIJU & LAVA GOLEM TRIBUTE HANDLERS
        // =========================================================================
        private bool LavaGolemSummon()
        {
            // Tributes 2 enemy monsters to opponent's side!
            var enemyMonsters = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup()).ToList();
            if (enemyMonsters.Count < 2) return false;

            var targets = Plugin.StrategyImpl.PickKaijuTributeTargets(enemyMonsters, 2);
            if (targets.Count >= 2)
            {
                AI.SelectCard(targets);
            }

            _normalSummonUsed = true; // Lava Golem consumes Normal Summon procedure
            return true;
        }

        private bool GamecielSummon()
        {
            // Tributes 1 enemy monster to opponent's field
            var enemyMonsters = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup()).ToList();
            if (enemyMonsters.Count == 0) return false;

            var target = Plugin.StrategyImpl.PickKaijuTributeTargets(enemyMonsters, 1).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
            }

            return enemyMonsters.Any(m => m.Attack >= 2000 || m.IsExtraCard() || CardIntelligence.IsKnownNegator(m.Id) || CardIntelligence.IsFloodgate(m.Id));
        }

        private bool DogoranSummon()
        {
            // If opponent already controls a Kaiju (e.g. Gameciel or Slumber), Special Summon Dogoran to OUR field!
            if (Enemy.GetMonsters().Any(m => IsKaijuCard(m.Id) || m.Id == CardId.GamecielTheSeaTurtleKaiju || m.Id == CardId.DogoranTheMadFlameKaiju))
            {
                return true;
            }

            // Otherwise, if opponent has a high threat monster and we don't have Gameciel
            var enemyMonsters = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup()).ToList();
            if (enemyMonsters.Count > 0 && !Bot.HasInHand(CardId.GamecielTheSeaTurtleKaiju))
            {
                var target = Plugin.StrategyImpl.PickKaijuTributeTargets(enemyMonsters, 1).FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                }
                return enemyMonsters.Any(m => m.Attack >= 2500 || CardIntelligence.IsKnownNegator(m.Id));
            }

            return false;
        }

        // =========================================================================
        // ALPHA, THE MASTER OF BEASTS
        // =========================================================================
        private bool AlphaSummon()
        {
            if (Enemy.GetMonsterCount() == 0) return false;
            int botTotalAtk = Bot.GetMonsters().Sum(m => m.Attack);
            int enemyTotalAtk = Enemy.GetMonsters().Sum(m => m.Attack);
            return enemyTotalAtk > botTotalAtk;
        }

        private bool AlphaEffect()
        {
            // Non-targeting bounce!
            var enemyFaceup = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup()).ToList();
            if (enemyFaceup.Count == 0) return false;

            var lv8Count = Bot.GetMonsters().Count(m => m != null && m.IsFaceup() && m.Level == 8);
            // If we have exactly 2 Level 8 monsters and can make Draglubion for lethal, keep Alpha as material!
            if (lv8Count == 2 && Bot.ExtraDeck.Any(c => c.Id == CardId.NumeronDragon) && Duel.Phase == DuelPhase.Main1)
            {
                // Only bounce if the enemy monster prevents our attack (e.g. Negator / Floodgate)
                return enemyFaceup.Any(m => CardIntelligence.IsKnownNegator(m.Id) || CardIntelligence.IsFloodgate(m.Id));
            }

            // Only bounce if enemy controls a high threat monster (>= 2500 ATK or known negator / extra deck)
            return enemyFaceup.Any(m => m.Attack >= 2500 || CardIntelligence.IsKnownNegator(m.Id) || m.IsExtraCard());
        }

        // =========================================================================
        // TARGETED DRAW & HAND SCULPTING (TRADE-IN & DANGER!)
        // =========================================================================
        private bool TradeInEffect()
        {
            var lv8Monsters = Bot.Hand.Where(c => c != null && c.Level == 8).ToList();
            if (lv8Monsters.Count == 0) return false;

            var discard = Plugin.MaterialImpl.PickDiscardTarget(lv8Monsters);
            if (discard != null)
            {
                AI.SelectCard(discard);
                return true;
            }

            return false;
        }

        private bool DangerHandEffect()
        {
            // Case 1: Hand activation (reveal and discard random card)
            if (Card.Location == CardLocation.Hand)
            {
                return true;
            }

            // Case 2: GY / Banished trigger on discard!
            if (Card.Location == CardLocation.Grave || Card.Location == CardLocation.Removed)
            {
                if (Card.Id == CardId.DangerBigfoot)
                {
                    // Target 1 face-up card opponent controls
                    var faceup = Enemy.GetMonsters().Concat(Enemy.GetSpells()).Where(c => c != null && c.IsFaceup()).ToList();
                    if (faceup.Count == 0) return false;

                    var target = Plugin.StrategyImpl.PickDangerDestroyTarget(faceup, false);
                    if (target != null)
                    {
                        AI.SelectCard(target);
                        return true;
                    }
                }
                else if (Card.Id == CardId.DangerThunderbird)
                {
                    // Target 1 Set card opponent controls
                    var setCards = Enemy.GetMonsters().Concat(Enemy.GetSpells()).Where(c => c != null && c.IsFacedown()).ToList();
                    if (setCards.Count == 0) return false;

                    var target = Plugin.StrategyImpl.PickDangerDestroyTarget(setCards, true);
                    if (target != null)
                    {
                        AI.SelectCard(target);
                        return true;
                    }
                }
            }

            return false;
        }

        private bool DimensionalFissureEffect()
        {
            if (Bot.HasInSpellZone(CardId.DimensionalFissure)) return false;
            return true;
        }

        private bool MacroCosmosEffect()
        {
            if (Bot.HasInSpellZone(CardId.MacroCosmos)) return false;
            if (Duel.Player == 1) return true;
            return Duel.Phase == DuelPhase.End;
        }

        // =========================================================================
        // BANISH ENGINE HANDLERS
        // =========================================================================
        private bool PotOfDesiresEffect()
        {
            // Banishes 10 cards face-down to draw 2 (+4000 to Maju!)
            // Safe margin: only activate if deck has at least 15 cards
            return Bot.Deck.Count >= 15;
        }

        private bool GizmekOrochiEffect()
        {
            // Case 1: Hand or Grave -> Quick Effect Special Summon!
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave)
            {
                if (Bot.HasInMonstersZone(CardId.GizmekOrochi)) return false;
                if (Bot.Deck.Count < 10) return false; // Safety margin against deck out

                // In our turn: summon in Main Phase 1
                if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1) return true;
                // In enemy's turn: summon during Battle or End Phase as blocker/beater
                if (Duel.Player == 1 && (Duel.Phase >= DuelPhase.BattleStart || Duel.Phase == DuelPhase.End)) return true;

                return false;
            }

            // Case 2: On Field -> Banish 3 ED cards face-down to pop 1 face-up monster!
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (Bot.ExtraDeck.Count < 3) return false;
                var targets = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup()).ToList();
                if (targets.Count == 0) return false;

                AI.SelectCard(targets.OrderByDescending(m => m.Attack).ToList());
                return true;
            }

            return false;
        }

        private bool EaterOfMillionsSummon()
        {
            // Banish 5 ED cards face-down to Special Summon
            if (Bot.ExtraDeck.Count < 5) return false;
            if (Bot.HasInMonstersZone(CardId.EaterOfMillions)) return false;
            if (Bot.GetMonstersInMainZone().Count >= 5) return false;

            // Preselect 5 fodder cards from Extra Deck (preserving Rank 8 engine)
            var fodder = Plugin.StrategyImpl.PickExtraDeckBanishTargets(Bot.ExtraDeck.ToList(), 5);
            if (fodder.Count >= 5)
            {
                AI.SelectCard(fodder);
            }

            return true;
        }

        private bool EaterOfMillionsEffect()
        {
            // When battling: banish battling enemy monster face-down at start of damage step!
            return true;
        }

        // =========================================================================
        // PLAN B RANK 8 XYZ ENGINE (NUMERON DRAGON OTK)
        // =========================================================================
        private bool DraglubionSummon()
        {
            // 1. MUST be Main Phase 1 to declare attack and OTK! (Draglubion oath prevents attacks from anyone except Numeron Dragon)
            if (Duel.Phase != DuelPhase.Main1) return false;

            // 2. Need 2 Level 8 monsters on field
            var lv8Monsters = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && m.Level == 8).ToList();
            if (lv8Monsters.Count < 2) return false;

            // 3. Ensure Numeron Dragon is available in Extra Deck
            if (!Bot.ExtraDeck.Any(c => c.Id == CardId.NumeronDragon)) return false;

            // 4. Ensure Hope Harbinger is available in Extra Deck or Grave to attach as material
            if (!Bot.ExtraDeck.Any(c => c.Id == CardId.HopeHarbinger) && !Bot.Graveyard.Any(c => c.Id == CardId.HopeHarbinger)) return false;

            AI.SelectPosition(CardPosition.FaceUpAttack);
            return true;
        }

        private bool DraglubionEffect()
        {
            // Detach 1 -> SS Numeron Dragon and attach Hope Harbinger as material!
            AI.SelectCard(CardId.NumeronDragon, CardId.HopeHarbinger);
            AI.SelectNextCard(CardId.NumeronDragon);
            AI.SelectPosition(CardPosition.FaceUpAttack);
            return true;
        }

        private bool NumeronDragonEffect()
        {
            // Detach 1 -> Gains 1000 ATK per combined Rank on field (Rank 8 + Rank 1 = 9000 ATK minimum!)
            _numeronPumped = true;
            return true;
        }

        private bool DingirsuSummon()
        {
            var lv8Monsters = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && m.Level == 8).ToList();
            if (lv8Monsters.Count < 2) return false;

            // Summon Dingirsu if opponent controls untargetable/indestructible threat
            var enemyCards = Enemy.GetMonsters().Concat(Enemy.GetSpells()).Where(c => c != null && c.IsFaceup()).ToList();
            return enemyCards.Any(c => c.Attack >= 2500 || CardIntelligence.IsKnownNegator(c.Id) || CardIntelligence.IsFloodgate(c.Id));
        }

        private bool DingirsuEffect()
        {
            // Send 1 card opponent controls to GY without targeting!
            AI.SelectOption(0); // Option 0 = Send 1 card opponent controls to the GY
            var enemyCards = Enemy.GetMonsters().Concat(Enemy.GetSpells()).Where(c => c != null).ToList();
            if (enemyCards.Count > 0)
            {
                var target = Plugin.StrategyImpl.PickDingirsuTarget(enemyCards);
                if (target != null)
                {
                    AI.SelectCard(target);
                }
            }
            return true;
        }

        private bool HopeHarbingerSummon()
        {
            var lv8Monsters = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && m.Level == 8).ToList();
            if (lv8Monsters.Count < 2) return false;

            // Summon Hope Harbinger if going first or setting up spell negation
            return Duel.Player == 0 && Duel.Phase == DuelPhase.Main2;
        }

        private bool HopeHarbingerEffect()
        {
            // Negate spell
            return Duel.LastChainPlayer == 1;
        }

        private bool TyphonSummon()
        {
            if (Bot.GetMonsterCount() == 0) return false;
            return Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Attack >= 3000);
        }

        private bool ZeusSummon()
        {
            return Duel.Phase == DuelPhase.Main2;
        }

        // =========================================================================
        // NORMAL SUMMONS
        // =========================================================================
        private bool GrenMajuSummon()
        {
            if (_normalSummonUsed) return false;

            int banishedCount = Bot.Banished.Count + Enemy.Banished.Count;
            int estimatedAtk = banishedCount * 400;

            // Summon if Gren Maju will have 2000+ ATK
            if (estimatedAtk >= 2000)
            {
                _normalSummonUsed = true;
                return true;
            }

            // If board is empty and we have no other monsters to summon
            if (Bot.GetMonsterCount() == 0)
            {
                _normalSummonUsed = true;
                return true;
            }

            return false;
        }

        private bool NecrofaceSummon()
        {
            if (_normalSummonUsed) return false;

            // Emergency recycle: only Normal Summon Necroface if deck is critically low (<= 4 cards)
            // or all 3 Gren Maju are banished and we need to recycle them!
            bool deckOutDanger = Bot.Deck.Count <= 4;
            bool majusAllBanished = Bot.Banished.Count(c => c.Id == CardId.GrenMajuDaEiza) >= 3;

            if (deckOutDanger || (majusAllBanished && Bot.GetMonsterCount() == 0))
            {
                _normalSummonUsed = true;
                return true;
            }

            return false;
        }

        private bool SpellSetPriority()
        {
            if (Card.Id == CardId.PotOfDesires ||
                Card.Id == CardId.TradeIn ||
                Card.Id == CardId.InterruptedKaijuSlumber ||
                Card.Id == CardId.Raigeki ||
                Card.Id == CardId.HarpiesFeatherDuster)
            {
                return false;
            }

            return Card.IsTrap() || Card.Id == CardId.DimensionalFissure || Card.Id == CardId.SuperPolymerization;
        }

        private bool MonsterRepos()
        {
            if (Card == null) return false;

            // If any of our key offensive beaters somehow ended up in Defense Position, switch to Attack immediately!
            if (Card.IsDefense() && (Card.Id == CardId.NumeronDragon ||
                                     Card.Id == CardId.GrenMajuDaEiza ||
                                     Card.Id == CardId.EaterOfMillions ||
                                     Card.Id == CardId.DangerBigfoot ||
                                     Card.Id == CardId.DangerThunderbird ||
                                     Card.Id == CardId.AlphaTheMasterOfBeasts ||
                                     Card.Id == CardId.DogoranTheMadFlameKaiju ||
                                     Card.Id == CardId.Draglubion ||
                                     Card.Id == CardId.Dingirsu ||
                                     Card.Id == CardId.GizmekOrochi))
            {
                return true; // Reposition to FaceUpAttack
            }

            if (Card.IsDefense() && Card.Attack >= 2000) return true;
            return false;
        }

        // =========================================================================
        // COMBAT OVERRIDES (Eater of Millions & Numeron Dragon & Gren Maju Calculations)
        // =========================================================================
        public override bool OnPreBattleBetween(ClientCard attacker, ClientCard defender)
        {
            if (attacker == null) return base.OnPreBattleBetween(attacker, defender);

            // Eater of Millions banishes battling monster face-down at start of damage step!
            if (attacker.IsCode(CardId.EaterOfMillions) && !attacker.IsDisabled())
            {
                attacker.RealPower = 9999;
                return true;
            }

            // Number 100: Numeron Dragon (9,000 to 13,000+ ATK!)
            if (attacker.IsCode(CardId.NumeronDragon))
            {
                attacker.RealPower = Math.Max(attacker.Attack, 9000);
                return true;
            }

            // Gren Maju Da Eiza dynamically calculates ATK from all banished cards
            if (attacker.IsCode(CardId.GrenMajuDaEiza))
            {
                int banished = Bot.Banished.Count + Enemy.Banished.Count;
                attacker.RealPower = banished * 400;
                return attacker.RealPower >= (defender?.GetDefensePower() ?? 0);
            }

            return base.OnPreBattleBetween(attacker, defender);
        }

        public override ClientCard OnSelectAttacker(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            if (attackers == null || attackers.Count == 0) return null;

            // 1. Eater of Millions: attacks first to banish enemy boss without taking damage!
            var eater = attackers.FirstOrDefault(a => a != null && a.IsCode(CardId.EaterOfMillions) && !a.IsDisabled());
            if (eater != null && defenders != null && defenders.Count > 0)
            {
                return eater;
            }

            // 2. Numeron Dragon: 9000-13000 ATK game-ending strike!
            var numeron = attackers.FirstOrDefault(a => a != null && a.IsCode(CardId.NumeronDragon));
            if (numeron != null)
            {
                return numeron;
            }

            // 3. Gren Maju Da Eiza: huge lethal swing!
            var maju = attackers.FirstOrDefault(a => a != null && a.IsCode(CardId.GrenMajuDaEiza));
            if (maju != null)
            {
                int power = (Bot.Banished.Count + Enemy.Banished.Count) * 400;
                if (power >= 3000) return maju;
            }

            // 4. Alpha (3000 ATK)
            var alpha = attackers.FirstOrDefault(a => a != null && a.IsCode(CardId.AlphaTheMasterOfBeasts));
            if (alpha != null) return alpha;

            // 5. Dogoran (3000 ATK) / Bigfoot (3000 ATK)
            var beater = attackers.FirstOrDefault(a => a != null && (a.IsCode(CardId.DogoranTheMadFlameKaiju) || a.IsCode(CardId.DangerBigfoot)));
            if (beater != null) return beater;

            // 6. Thunderbird (2800 ATK) / Dingirsu (2600 ATK)
            var beater2 = attackers.FirstOrDefault(a => a != null && (a.IsCode(CardId.DangerThunderbird) || a.IsCode(CardId.Dingirsu)));
            if (beater2 != null) return beater2;

            // 7. Gizmek Orochi (2450 ATK)
            var orochi = attackers.FirstOrDefault(a => a != null && a.IsCode(CardId.GizmekOrochi));
            if (orochi != null) return orochi;

            return base.OnSelectAttacker(attackers, defenders);
        }

        // =========================================================================
        // POSITION SELECTION OVERRIDES (Dynamic ATK Finishers MUST ALWAYS Attack)
        // =========================================================================
        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // CRITICAL: Dynamic ATK Finishers and Offensive Beaters MUST ALWAYS be summoned in Attack Position!
            // When monsters with 0 or variable base ATK (e.g. Numeron Dragon, Gren Maju, Eater of Millions, Bigfoot)
            // are summoned, placing them in Defense Position leaves them unable to attack, and with 0 DEF they are destroyed next turn!
            if (cardId == CardId.NumeronDragon ||
                cardId == CardId.GrenMajuDaEiza ||
                cardId == CardId.EaterOfMillions ||
                cardId == CardId.DangerBigfoot ||
                cardId == CardId.DangerThunderbird ||
                cardId == CardId.AlphaTheMasterOfBeasts ||
                cardId == CardId.DogoranTheMadFlameKaiju ||
                cardId == CardId.GizmekOrochi ||
                cardId == CardId.Draglubion ||
                cardId == CardId.Dingirsu ||
                cardId == CardId.HopeHarbinger ||
                cardId == CardId.Garura ||
                cardId == CardId.MudragonOfTheSwamp ||
                cardId == CardId.StarvingVenomFusionDragon)
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }

            // Kaiju and Lava Golem given to the opponent MUST ALWAYS be placed in Attack Position!
            // This ensures our 9000-13000 ATK Numeron Dragon or Gren Maju can punch through them for lethal battle damage!
            if (cardId == CardId.GamecielTheSeaTurtleKaiju || cardId == CardId.LavaGolem)
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        // =========================================================================
        // SELECTION OVERRIDES (Routing through Decoupled Plugin)
        // =========================================================================
        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return base.OnSelectCard(cards, min, max, hint, cancelable);

            // 1. Kaiju / Lava Golem Tribute Target Selection
            if (hint == 500) // HINTMSG_RELEASE
            {
                AI.SelectPosition(CardPosition.FaceUpAttack);
                var tributes = Plugin.StrategyImpl.PickKaijuTributeTargets(cards, min);
                if (tributes.Count >= min) return tributes;
            }

            // 2. Discard Target Selection (Trade-In)
            if (hint == 501) // HINTMSG_DISCARD
            {
                var discardTarget = Plugin.MaterialImpl.PickDiscardTarget(cards, min);
                if (discardTarget != null) return new[] { discardTarget };
            }

            // 3. Destruction Targets (Danger! Bigfoot / Thunderbird / Raigeki)
            if (hint == 502) // HINTMSG_DESTROY
            {
                bool targetSet = cards.Any(c => c != null && c.Controller == 1 && c.IsFacedown());
                var popTarget = Plugin.StrategyImpl.PickDangerDestroyTarget(cards, targetSet);
                if (popTarget != null) return new[] { popTarget };
            }

            // 4. Banish Targets (Eater of Millions / Orochi Extra Deck cost vs Enemy Removal)
            if (hint == 503) // HINTMSG_REMOVE
            {
                // If selecting from Extra Deck: preserve Rank 8 Plan B combo!
                if (cards.All(c => c != null && c.Location == CardLocation.Extra))
                {
                    var extraBanish = Plugin.StrategyImpl.PickExtraDeckBanishTargets(cards, min);
                    if (extraBanish.Count >= min) return extraBanish;
                }

                // If removing enemy card (Eater of Millions combat effect)
                var enemyTarget = cards.FirstOrDefault(c => c != null && c.Controller == 1);
                if (enemyTarget != null) return new[] { enemyTarget };
            }

            // 5. Send to GY (Dingirsu non-targeting removal)
            if (hint == 504) // HINTMSG_TOGRAVE
            {
                var enemyTarget = cards.FirstOrDefault(c => c != null && c.Controller == 1);
                if (enemyTarget != null) return new[] { enemyTarget };
            }

            // 6. Search target (Kaiju Slumber search)
            if (hint == 506) // HINTMSG_ATOHAND
            {
                var searchTarget = Plugin.StrategyImpl.PickSearchTarget(cards, Card);
                if (searchTarget != null) return new[] { searchTarget };
            }

            // 7. Return to Hand (Alpha, the Master of Beasts)
            if (hint == 507) // HINTMSG_RTOHAND
            {
                var alphaTarget = Plugin.StrategyImpl.PickAlphaBounceTarget(cards);
                if (alphaTarget != null) return new[] { alphaTarget };
            }

            // 8. Draglubion Number Dragon Overlays (2 dragons from ED)
            if (cards.Any(c => c.Id == CardId.NumeronDragon || c.Id == CardId.HopeHarbinger))
            {
                if (min == 2 && max == 2)
                {
                    var dragons = Plugin.StrategyImpl.PickDraglubionOverlayTargets(cards);
                    if (dragons.Count >= min) return dragons.Take(max).ToList();
                }
                if (min == 1)
                {
                    // Draglubion selecting 1 card to Special Summon
                    var numeron = cards.FirstOrDefault(c => c.Id == CardId.NumeronDragon);
                    if (numeron != null)
                    {
                        AI.SelectPosition(CardPosition.FaceUpAttack);
                        return new[] { numeron };
                    }
                }
            }

            // Default fallback: prioritize enemy targets when removal is expected
            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override IList<ClientCard> OnSelectXyzMaterial(IList<ClientCard> cards, int min, int max)
        {
            if (cards == null || cards.Count == 0) return base.OnSelectXyzMaterial(cards, min, max);

            // Prioritize Level 8 monsters that have already used their effects or are pure beaters
            return cards
                .Where(c => c != null && c.Level == 8)
                .OrderBy(c => c.Attack)
                .Take(max)
                .ToList();
        }
    }
}
