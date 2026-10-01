using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Plugins;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    // ══════════════════════════════════════════════════════════════════════════════════
    // CARD AUDIT — Thunder Dragon (Banish Combo & Floodgate Contact Fusion Engine)
    // ══════════════════════════════════════════════════════════════════════════════════
    // | Card Name                     | Type        | OPT? | HOPT? | Cost     | Effect Summary                                | Activate When                                |
    // |-------------------------------|-------------|------|-------|----------|-----------------------------------------------|----------------------------------------------|
    // | Thunder Dragon (31786629)     | Monster L5  | No   | No    | Discard  | Add up to 2 "Thunder Dragon" from Deck        | Turn on Colossus / deck thin / Titan pop     |
    // | Thunder Dragondark (56713174) | Monster L5  | Yes  | Yes   | Discard  | Discard: search Dragondark. Banish/GY: search | Turn on Colossus / search any Thunder Dragon |
    // | Thunder Dragonhawk (83107873) | Monster L6  | Yes  | Yes   | Discard  | Discard: revive banished/GY TD. Banish: mull  | Extender / boss reviver                      |
    // | Thunder Dragonroar (29596581) | Monster L6  | Yes  | Yes   | Discard  | Discard: salvage TD. Banish/GY: SS from deck  | Extender / deck summoner                     |
    // | Thunder Dragonmatrix (20318029)| Monster L1 | Yes  | Yes   | Discard  | Quick: +500 ATK. Banish/GY: search Matrix     | Turn on Colossus / Titan pop trigger / Link1 |
    // | Thunder Dragonduo (55591586)  | Monster L8  | Yes  | Yes   | Banish   | SS by banishing 1 LIGHT & 1 DARK from GY     | High ATK beatstick / search on kill          |
    // | Aloof Lupine (92998610)       | Monster L4  | Yes  | Yes   | Banish H | NS: banish 1 from hand & 1 same type from deck| Premier Banish Starter                       |
    // | Batteryman Solar (44586426)   | Monster L4  | Yes  | Yes   | None     | Send Thunder from Deck to GY; SS token on TD  | Mill Roar/Dark/Matrix; Link fodder           |
    // | Chaos Dragon Levianeer        | Monster L8  | Yes  | Yes   | Banish 3 | Banish 3 L/D from GY: pop 2 / hand rip / rev  | Board breaker / game ender                   |
    // | Thunder Dragon Colossus       | Fusion L8   | No   | No    | Tribute  | 1-Card Contact Fusion; Search Floodgate       | Turn 1 Boss #1 / search lock                 |
    // | Thunder Dragon Titan          | Fusion L10  | No   | No    | 3 Shuffle| Quick pop when Thunder activates in hand      | Turn 1 Boss #2 / Quick removal               |
    // | Thunder Dragon Fusion         | Spell Normal| Yes  | Yes   | Shuffle 3| Shuffles 3 from GY/field/banish -> Fusion TD  | Main combo extender; GY banish searches TD   |
    // | Gold Sarcophagus              | Spell Normal| Yes  | Yes   | None     | Banish 1 from deck face-up                   | Instant trigger Dark/Roar                    |
    // | Allure of Darkness            | Spell Normal| No   | No    | Banish D | Draw 2, banish 1 DARK from hand              | Draw 2 & trigger Dark/Roar                   |
    // ══════════════════════════════════════════════════════════════════════════════════

    [Deck("ThunderDragon", "ThunderDragon")]
    public class ThunderDragonExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Deck Monsters
            public const int ThunderDragon = 31786629;
            public const int ThunderDragondark = 56713174;
            public const int ThunderDragonhawk = 83107873;
            public const int ThunderDragonroar = 29596581;
            public const int ThunderDragonmatrix = 20318029;
            public const int ThunderDragonduo = 55591586;
            public const int AloofLupine = 92998610;
            public const int BatterymanSolar = 44586426;
            public const int ChaosDragonLevianeer = 55878038;
            public const int AshBlossom = 14558127;
            public const int EffectVeiler = 97268402;

            // Spells & Traps
            public const int ThunderDragonFusion = 95238394;
            public const int GoldSarcophagus = 75500286;
            public const int AllureOfDarkness = 1475311;
            public const int HarpiesFeatherDuster = 18144506;
            public const int DarkRulerNoMore = 54693926;
            public const int TripleTacticsTalent = 25311006;
            public const int MonsterReborn = 83764718;
            public const int InfiniteImpermanence = 10045474;

            // Extra Deck
            public const int ThunderDragonColossus = 15291624;
            public const int ThunderDragonTitan = 41685633;
            public const int CrossSheep = 50277355;
            public const int SomeSummerSummoner = 38406364;
            public const int IPMasquerena = 65741786;
            public const int SPLittleKnight = 29301450;
            public const int KnightmarePhoenix = 2857636;
            public const int KnightmareUnicorn = 38342335;
            public const int AccesscodeTalker = 86066372;
            public const int RelinquishedAnima = 94259633;
            public const int Linkuriboh = 41999284;
            public const int HopeHarbinger = 63767246;

            // Tokens
            public const int BatterymanToken = 44586427;
        }

        private readonly ThunderDragonPlugin _plugin;
        private bool _thunderEffectActivatedInHandThisTurn;

        public ThunderDragonExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            DeckPlugin = _plugin = new ThunderDragonPlugin(this);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 0: EMERGENCY COUNTERS & HANDTRAPS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, ImpermanenceCondition);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomCondition);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, EffectVeilerCondition);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 1: BOARD BREAKERS & GOING-SECOND ADVANTAGE
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, FeatherDusterCondition);
            AddExecutor(ExecutorType.Activate, CardId.DarkRulerNoMore, DarkRulerNoMoreCondition);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsCondition);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 2: HAND THINNING & BANISH ENABLERS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.GoldSarcophagus, GoldSarcophagusCondition);
            AddExecutor(ExecutorType.Activate, CardId.AllureOfDarkness, AllureOfDarknessCondition);
            AddExecutor(ExecutorType.Activate, CardId.ThunderDragon, ThunderDragonPitchCondition);
            AddExecutor(ExecutorType.Activate, CardId.ThunderDragondark, DragondarkPitchCondition);
            AddExecutor(ExecutorType.Activate, CardId.ThunderDragonmatrix, MatrixQuickCondition);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 3: NORMAL SUMMON STARTERS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Summon, CardId.AloofLupine, AloofLupineSummonCondition);
            AddExecutor(ExecutorType.Activate, CardId.AloofLupine, AloofLupineActivateCondition);

            AddExecutor(ExecutorType.Summon, CardId.BatterymanSolar, SolarSummonCondition);
            AddExecutor(ExecutorType.Activate, CardId.BatterymanSolar, SolarActivateCondition);

            AddExecutor(ExecutorType.Summon, CardId.ThunderDragonmatrix, MatrixNormalSummonCondition);
            AddExecutor(ExecutorType.Summon, CardId.ThunderDragondark, DarkNormalSummonCondition);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 4: EXTENDERS & GY SALVAGE
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.ThunderDragonhawk, HawkHandReviveCondition);
            AddExecutor(ExecutorType.Activate, CardId.ThunderDragonroar, RoarHandSalvageCondition);
            AddExecutor(ExecutorType.Activate, CardId.MonsterReborn, MonsterRebornCondition);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 5: BANISH TRIGGERS & FIELD->GY TRIGGERS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.ThunderDragondark, DragondarkBanishOrGYCondition);
            AddExecutor(ExecutorType.Activate, CardId.ThunderDragonroar, RoarBanishOrGYCondition);
            AddExecutor(ExecutorType.Activate, CardId.ThunderDragonhawk, HawkBanishMulliganCondition);
            AddExecutor(ExecutorType.Activate, CardId.ThunderDragonmatrix, MatrixBanishOrGYCondition);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 6: EXTRA DECK PREPARATION & LINK SETUP
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.CrossSheep, CrossSheepSummonCondition);
            AddExecutor(ExecutorType.Activate, CardId.CrossSheep, CrossSheepActivateCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.Linkuriboh, LinkuribohSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.RelinquishedAnima, RelinquishedAnimaSummonCondition);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 7: BOSS SUMMONS (COLOSSUS & TITAN)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.ThunderDragonColossus, ColossusContactSummonCondition);
            AddExecutor(ExecutorType.Activate, CardId.ThunderDragonFusion, ThunderDragonFusionCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.ThunderDragonTitan, TitanContactSummonCondition);
            AddExecutor(ExecutorType.Activate, CardId.ThunderDragonTitan, TitanPopCondition);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 8: CHAOS EXTENDERS & LEVEL 8 XYZ
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.ThunderDragonduo, DuoSummonCondition);
            AddExecutor(ExecutorType.Activate, CardId.ThunderDragonduo, DuoActivateCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.ChaosDragonLevianeer, LevianeerSummonCondition);
            AddExecutor(ExecutorType.Activate, CardId.ChaosDragonLevianeer, LevianeerActivateCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.HopeHarbinger, HopeHarbingerSummonCondition);
            AddExecutor(ExecutorType.Activate, CardId.HopeHarbinger, HopeHarbingerNegateCondition);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 9: UTILITY LINK CLIMBS (S:P, UNICORN, ACCESSCODE)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmarePhoenix, PhoenixSummonCondition);
            AddExecutor(ExecutorType.Activate, CardId.KnightmarePhoenix, PhoenixActivateCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmareUnicorn, UnicornSummonCondition);
            AddExecutor(ExecutorType.Activate, CardId.KnightmareUnicorn, UnicornActivateCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSummonCondition);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightActivateCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, AccesscodeSummonCondition);
            AddExecutor(ExecutorType.Activate, CardId.AccesscodeTalker, AccesscodeActivateCondition);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 10: BACKROW SETTING
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, ImpermSetCondition);
        }

        public override bool OnSelectHand() => true; // Go first to establish Colossus search floodgate!

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _thunderEffectActivatedInHandThisTurn = false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  EXECUTION CONDITIONS
        // ═══════════════════════════════════════════════════════════════

        private bool IsChainAlreadyNeutralized()
        {
            ClientCard last = LastChainCard;
            if (last == null || last.Controller != 1) return true;
            if (last.IsDisabled()) return true;
            return false;
        }

        private bool ImpermanenceCondition()
        {
            if (IsChainAlreadyNeutralized()) return false;
            return DefaultInfiniteImpermanence();
        }

        private bool AshBlossomCondition()
        {
            if (IsChainAlreadyNeutralized()) return false;
            return SmartHandTrapChain() && DefaultAshBlossomAndJoyousSpring();
        }

        private bool EffectVeilerCondition()
        {
            if (IsChainAlreadyNeutralized()) return false;
            return DefaultEffectVeiler();
        }

        private bool FeatherDusterCondition()
        {
            return Enemy.GetSpellCount() > 0;
        }

        private bool DarkRulerNoMoreCondition()
        {
            return Enemy.GetMonsters().Any(c => c.IsFaceup() && !c.IsDisabled() && (c.HasType(CardType.Effect) || c.Attack >= 2000));
        }

        private bool TripleTacticsCondition()
        {
            return true;
        }

        private bool GoldSarcophagusCondition()
        {
            // Banish Dark or Roar from Deck
            return GetRemainingCount(CardId.ThunderDragonroar) > 0 || GetRemainingCount(CardId.ThunderDragondark) > 0;
        }

        private bool AllureOfDarknessCondition()
        {
            // Safe Allure check: Must have at least 1 DARK monster in hand so we don't discard entire hand!
            return Bot.Hand.Any(c => c != Card && c.HasAttribute(CardAttribute.Dark));
        }

        private bool ThunderDragonPitchCondition()
        {
            if (Card.Location != CardLocation.Hand) return false;
            _thunderEffectActivatedInHandThisTurn = true;
            return GetRemainingCount(CardId.ThunderDragon) > 0;
        }

        private bool DragondarkPitchCondition()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // Activate Dragondark from hand to enable Colossus contact summon if not already enabled,
            // or if we have duplicates in hand
            if (!_thunderEffectActivatedInHandThisTurn || Bot.Hand.Count(c => c.Id == CardId.ThunderDragondark) > 1)
            {
                _thunderEffectActivatedInHandThisTurn = true;
                return true;
            }
            return false;
        }

        private bool MatrixQuickCondition()
        {
            if (Card.Location != CardLocation.Hand) return false;

            // 1. If Titan is on field and enemy has cards, activate Matrix to trigger Titan's Quick Pop!
            if (Bot.HasInMonstersZone(CardId.ThunderDragonTitan) && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0))
            {
                _thunderEffectActivatedInHandThisTurn = true;
                return true;
            }

            // 2. If no Thunder effect used in hand yet, and we control a Thunder monster, activate to enable Colossus!
            if (!_thunderEffectActivatedInHandThisTurn && Bot.GetMonsters().Any(c => c.HasRace(CardRace.Thunder)))
            {
                _thunderEffectActivatedInHandThisTurn = true;
                return true;
            }

            // 3. During Battle Phase, buff our attacker
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep)
            {
                _thunderEffectActivatedInHandThisTurn = true;
                return true;
            }

            return false;
        }

        private bool AloofLupineSummonCondition()
        {
            // Lupine requires a Thunder monster in hand to banish
            return Bot.Hand.Any(c => c != Card && c.HasRace(CardRace.Thunder));
        }

        private bool AloofLupineActivateCondition()
        {
            return true;
        }

        private bool SolarSummonCondition()
        {
            return true;
        }

        private bool SolarActivateCondition()
        {
            return true;
        }

        private bool MatrixNormalSummonCondition()
        {
            // Only normal summon Matrix if we have no Lupine or Solar
            if (Bot.HasInHand(CardId.AloofLupine) || Bot.HasInHand(CardId.BatterymanSolar))
                return false;
            return true;
        }

        private bool DarkNormalSummonCondition()
        {
            // Normal summon Dragondark as last resort body
            return Bot.GetMonsterCount() == 0 &&
                   !Bot.HasInHand(CardId.AloofLupine) &&
                   !Bot.HasInHand(CardId.BatterymanSolar) &&
                   !Bot.HasInHand(CardId.ThunderDragonmatrix);
        }

        private bool HawkHandReviveCondition()
        {
            if (Card.Location != CardLocation.Hand) return false;

            // Revive target must exist in GY or Banished
            bool hasTarget = Bot.Graveyard.Any(c => IsThunderDragonName(c.Id)) ||
                             Bot.Banished.Any(c => IsThunderDragonName(c.Id));

            if (hasTarget)
            {
                _thunderEffectActivatedInHandThisTurn = true;
                return true;
            }
            return false;
        }

        private bool RoarHandSalvageCondition()
        {
            if (Card.Location != CardLocation.Hand) return false;

            // If we don't have Hawk or Fusion and have target in GY/Banish
            bool hasTarget = Bot.Graveyard.Any(c => IsThunderDragonName(c.Id)) ||
                             Bot.Banished.Any(c => IsThunderDragonName(c.Id));

            if (hasTarget && (!_thunderEffectActivatedInHandThisTurn || !Bot.HasInHand(CardId.ThunderDragonhawk)))
            {
                _thunderEffectActivatedInHandThisTurn = true;
                return true;
            }
            return false;
        }

        private bool MonsterRebornCondition()
        {
            return Bot.Graveyard.Any(c => c.Id == CardId.ThunderDragonColossus ||
                                          c.Id == CardId.ThunderDragonTitan ||
                                          c.Id == CardId.ChaosDragonLevianeer ||
                                          c.Id == CardId.ThunderDragonduo);
        }

        private bool DragondarkBanishOrGYCondition()
        {
            return Card.Location != CardLocation.Hand;
        }

        private bool RoarBanishOrGYCondition()
        {
            return Card.Location != CardLocation.Hand;
        }

        private bool HawkBanishMulliganCondition()
        {
            if (Card.Location == CardLocation.Hand) return false;
            // Only mulligan if we have duplicate cards in hand
            return Bot.Hand.GroupBy(c => c.Id).Any(g => g.Count() > 1);
        }

        private bool MatrixBanishOrGYCondition()
        {
            return Card.Location != CardLocation.Hand;
        }

        private bool CrossSheepSummonCondition()
        {
            // Only make Cross-Sheep if we have 2 fodder monsters and a way to summon Colossus or Titan
            if (Bot.GetMonsters().Count(c => _plugin.MaterialEvaluator.GetMaterialCost(c) < 1000) >= 2)
            {
                return _thunderEffectActivatedInHandThisTurn ||
                       Bot.HasInHand(CardId.ThunderDragonFusion) ||
                       Bot.Graveyard.Any(c => c.Id == CardId.ThunderDragonFusion);
            }
            return false;
        }

        private bool CrossSheepActivateCondition()
        {
            return true;
        }

        private bool LinkuribohSummonCondition()
        {
            // Link off Matrix or Batteryman Token
            var fodder = Bot.GetMonsters().FirstOrDefault(c => c.Id == CardId.ThunderDragonmatrix || c.Id == CardId.BatterymanToken);
            return fodder != null && Bot.GetMonsterCount() < 5;
        }

        private bool RelinquishedAnimaSummonCondition()
        {
            // Check if opponent has monster in front of EMZ
            var fodder = Bot.GetMonsters().FirstOrDefault(c => c.Id == CardId.ThunderDragonmatrix);
            return fodder != null && Enemy.GetMonsters().Any(c => c.IsFaceup());
        }

        private bool ColossusContactSummonCondition()
        {
            // Rule: Thunder monster effect must have activated in hand this turn!
            if (!_thunderEffectActivatedInHandThisTurn) return false;

            // Maximum 2 Colossus on field
            if (Bot.GetMonsters().Count(c => c.Id == CardId.ThunderDragonColossus) >= 2) return false;

            // Must have a Thunder Effect non-Fusion Monster on field to tribute
            var tributeCandidates = Bot.GetMonsters().Where(c =>
                c.HasRace(CardRace.Thunder) &&
                c.HasType(CardType.Effect) &&
                !c.HasType(CardType.Fusion)
            ).ToList();

            if (tributeCandidates.Count == 0) return false;

            // Select best tribute (prefer Matrix > Dark > Roar > Solar > Duo)
            var bestTribute = tributeCandidates.OrderBy(c => _plugin.MaterialEvaluator.GetMaterialCost(c)).FirstOrDefault();
            if (bestTribute != null)
            {
                AI.SelectCard(bestTribute);
                return true;
            }

            return false;
        }

        private bool ThunderDragonFusionCondition()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                // Count available Thunders in GY, Banished, and non-Boss field
                int availableThunders = Bot.Graveyard.Count(c => c.HasRace(CardRace.Thunder)) +
                                       Bot.Banished.Count(c => c.HasRace(CardRace.Thunder)) +
                                       Bot.GetMonsters().Count(c => c.HasRace(CardRace.Thunder) && _plugin.MaterialEvaluator.GetMaterialCost(c) < 10000);

                return availableThunders >= 3;
            }

            // GY effect: Banish to search Thunder monster from deck
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }

            return false;
        }

        private bool TitanContactSummonCondition()
        {
            // Titan contact summon requires banishing 1 Thunder from hand AND 1 Thunder Fusion from field
            // Only do this if we have excess Colossus and need 3200 ATK beatstick
            if (Bot.GetMonsters().Count(c => c.Id == CardId.ThunderDragonColossus) >= 2 &&
                Bot.Hand.Any(c => c.HasRace(CardRace.Thunder)))
            {
                return true;
            }
            return false;
        }

        private bool TitanPopCondition()
        {
            // HARD RULE (Target Verification Safeguard):
            // Only activate if opponent has cards to destroy!
            if (Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0)
                return false;

            return true;
        }

        private bool DuoSummonCondition()
        {
            if (Bot.GetMonsters().Count(c => c.Id == CardId.ThunderDragonduo) > 0) return false;
            // Requires 1 LIGHT and 1 DARK in GY
            bool hasLight = Bot.Graveyard.Any(c => c.HasAttribute(CardAttribute.Light));
            bool hasDark = Bot.Graveyard.Any(c => c.HasAttribute(CardAttribute.Dark));
            return hasLight && hasDark;
        }

        private bool DuoActivateCondition()
        {
            return true;
        }

        private bool LevianeerSummonCondition()
        {
            int ldCount = Bot.Graveyard.Count(c => c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark));
            return ldCount >= 3;
        }

        private bool LevianeerActivateCondition()
        {
            return true;
        }

        private bool HopeHarbingerSummonCondition()
        {
            // Overlay 2 Level 8 monsters (Duo + Levianeer)
            var lv8s = Bot.GetMonsters().Where(c => c.Level == 8 && c.Id != CardId.ThunderDragonColossus).ToList();
            return lv8s.Count >= 2;
        }

        private bool HopeHarbingerNegateCondition()
        {
            return DefaultTrap();
        }

        private bool PhoenixSummonCondition()
        {
            return Enemy.GetSpellCount() > 0 &&
                   Bot.GetMonsters().Count(c => _plugin.MaterialEvaluator.GetMaterialCost(c) < 1000) >= 2;
        }

        private bool PhoenixActivateCondition()
        {
            return Enemy.GetSpellCount() > 0;
        }

        private bool UnicornSummonCondition()
        {
            return (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0) &&
                   Bot.GetMonsters().Count(c => _plugin.MaterialEvaluator.GetMaterialCost(c) < 1000) >= 3;
        }

        private bool UnicornActivateCondition()
        {
            return Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
        }

        private bool SPLittleKnightSummonCondition()
        {
            // Rule DEFAULT 2: Do NOT summon S:P in MP1 if enemy field is empty
            if (Duel.Phase == DuelPhase.Main1 && Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0)
                return false;

            // Only make S:P if enemy has threats or in MP2
            var fodders = Bot.GetMonsters().Where(c => _plugin.MaterialEvaluator.GetMaterialCost(c) < 5000).ToList();
            return fodders.Count >= 2 && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0 || Duel.Phase == DuelPhase.Main2);
        }

        private bool SPLittleKnightActivateCondition()
        {
            return true;
        }

        private bool AccesscodeSummonCondition()
        {
            // OTK finisher in MP1 or board breaker
            return Duel.Turn > 1 && Bot.GetMonsters().Any(c => c.HasType(CardType.Link) && c.LinkCount >= 2);
        }

        private bool AccesscodeActivateCondition()
        {
            return true;
        }

        private bool ImpermSetCondition()
        {
            // Only set in Main Phase 2 if going first, or if we have another copy in hand
            return Duel.Phase == DuelPhase.Main2 || Bot.Hand.Count(c => c.Id == CardId.InfiniteImpermanence) > 1;
        }

        // ═══════════════════════════════════════════════════════════════
        //  AI INTERACTION OVERRIDES
        // ═══════════════════════════════════════════════════════════════

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // Delegate to Plugin logic first
            var pluginResult = _plugin.SelectCardLogic(cards, min, max, hint, cancelable);
            if (pluginResult != null)
                return pluginResult;

            // 1. Thunder Dragon Fusion material selection (select 3 Thunders, prefer Banished & GY over Field)
            if (cards.Any(c => c.HasRace(CardRace.Thunder)) && min <= 3 && max >= 3)
            {
                var sortedThunders = cards.Where(c => c.HasRace(CardRace.Thunder))
                    .OrderBy(c => c.Location == CardLocation.MonsterZone ? 100 : 0)
                    .ThenBy(c => c.Id == CardId.ThunderDragonColossus || c.Id == CardId.ThunderDragonTitan ? 50 : 0)
                    .Take(max)
                    .ToList();
                if (sortedThunders.Count >= min)
                    return sortedThunders;
            }

            // 2. Colossus Tribute selection
            if (cards.All(c => c.Location == CardLocation.MonsterZone && c.Controller == 0))
            {
                var sorted = cards.OrderBy(c => _plugin.MaterialEvaluator.GetMaterialCost(c)).ToList();
                return sorted.Take(min).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            // HARD RULE: Enemy effect prompts default to false!
            if (card != null && card.Controller == 1)
                return false;

            // Colossus / Titan destruction protection: ALWAYS say yes to survive!
            if (card != null && (card.Id == CardId.ThunderDragonColossus || card.Id == CardId.ThunderDragonTitan))
                return true;

            return base.OnSelectEffectYn(card, desc);
        }

        public override int OnSelectOption(IList<long> options)
        {
            // Triple Tactics Talent options:
            // 0: Draw 2 cards (preferred if hand is low)
            // 1: Take control of 1 opponent monster (if opponent has strong monster)
            // 2: Look at opponent hand and shuffle 1 card
            if (options.Count == 3)
            {
                if (Enemy.GetMonsters().Any(c => c.Attack >= 2500) && Bot.GetMonsterCount() < 5)
                    return 1; // Take control
                return 0; // Draw 2
            }

            return base.OnSelectOption(options);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Roar SS from deck must be in DEF
            if (cardId == CardId.ThunderDragonroar && positions.Contains(CardPosition.FaceUpDefence))
                return CardPosition.FaceUpDefence;

            // Bosses attack
            if (cardId == CardId.ThunderDragonColossus || cardId == CardId.ThunderDragonTitan || cardId == CardId.ThunderDragonduo)
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
            // When summoning Fusion monsters while Cross-Sheep is on field:
            // Prefer summoning to a zone Cross-Sheep points to (bottom-left or bottom-right of Cross-Sheep)
            if (cardId == CardId.ThunderDragonColossus || cardId == CardId.ThunderDragonTitan)
            {
                var sheep = Bot.GetMonsters().FirstOrDefault(c => c.Id == CardId.CrossSheep);
                if (sheep != null)
                {
                    // Zone under EMZ 5: Zone 0 or Zone 2
                    // Zone under EMZ 6: Zone 2 or Zone 4
                    int zone0 = 0x1, zone2 = 0x4, zone4 = 0x10;
                    if ((available & zone2) != 0) return zone2;
                    if ((available & zone0) != 0) return zone0;
                    if ((available & zone4) != 0) return zone4;
                }
            }

            return base.OnSelectPlace(cardId, player, location, available);
        }

        private static bool IsThunderDragonName(long id)
        {
            return id == CardId.ThunderDragon ||
                   id == CardId.ThunderDragondark ||
                   id == CardId.ThunderDragonhawk ||
                   id == CardId.ThunderDragonroar ||
                   id == CardId.ThunderDragonmatrix ||
                   id == CardId.ThunderDragonduo ||
                   id == CardId.ThunderDragonColossus ||
                   id == CardId.ThunderDragonTitan;
        }
    }
}
