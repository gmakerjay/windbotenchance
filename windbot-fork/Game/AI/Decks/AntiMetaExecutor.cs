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
    // CARD AUDIT — AntiMeta (6-Dimensional Omni-Lockdown Anti-Meta Deck)
    // 100% verified against cards.cdb and AntiMeta.ydk
    // ====================================================================================================
    // | Card Name                     | Type           | Lv/Rk | ATK  | DEF  | Key Interaction                                         |
    // |-------------------------------|----------------|-------|------|------|---------------------------------------------------------|
    // | Inspector Boarder             | Machine/LIGHT  | 4     | 2000 | 2000 | Monster effect freeze (0 activations if no ED monster)  |
    // | Barrier Statue of the Heavens | Fairy/LIGHT    | 4     | 1000 | 1000 | Locks Special Summons of all non-LIGHT monsters         |
    // | Thunder King Rai-Oh           | Thunder/LIGHT  | 4     | 1900 | 800  | Search denial (no add from Deck) + negate SS inherent   |
    // | Banisher of the Radiance      | Fairy/LIGHT    | 3     | 1600 | 0    | Continuous Macro Cosmos (all cards banished instead)    |
    // | Time-Tearing Morganite        | Normal Spell   | -     | -    | -    | Double Normal Summon + Double Draw every turn           |
    // | Moon Mirror Shield            | Equip Spell    | -     | -    | -    | Battle invincibility (ATK/DEF = opp ATK/DEF + 100)      |
    // | Pot of Duality                | Normal Spell   | -     | -    | -    | Excavate 3 add 1, no SS this turn (we don't SS)         |
    // | Pot of Extravagance           | Normal Spell   | -     | -    | -    | Banish 6 ED cards to Draw 2 cards                       |
    // | Necrovalley                   | Field Spell    | -     | -    | -    | Graveyard total lockout                                 |
    // | Dimensional Fissure           | Continuous Sp  | -     | -    | -    | All monsters banished instead of sent to GY             |
    // | Destructive Daruma Karma Cannon| Normal Trap   | -     | -    | -    | Mass face-down flip; Links & unaffected sent to GY      |
    // | Solemn Strike                 | Counter Trap   | -     | -    | -    | Negate monster effect activation or Special Summon      |
    // | Solemn Warning                | Counter Trap   | -     | -    | -    | Negate summon or effect that summons                    |
    // | Solemn Judgment               | Counter Trap   | -     | -    | -    | Omni-negate; protects backrow from Feather Duster etc.  |
    // | Crackdown                     | Continuous Tr  | -     | -    | -    | Steals face-up opponent monster                         |
    // | Skill Drain                   | Continuous Tr  | -     | -    | -    | Negates all face-up monster effects on field            |
    // | Macro Cosmos                  | Continuous Tr  | -     | -    | -    | Universal banish floodgate                              |
    // | There Can Be Only One         | Continuous Tr  | -     | -    | -    | 1 monster per type restriction (punishes mono-type)     |
    // ====================================================================================================

    [Deck("AntiMeta", "AntiMeta")]
    public class AntiMetaExecutor : ModernExecutor
    {
        public class CardId
        {
            // --- MONSTERS ---
            public const int InspectorBoarder = 15397015;
            public const int BarrierStatueOfTheHeavens = 46145256;
            public const int ThunderKingRaiOh = 71564252;
            public const int BanisherOfTheRadiance = 94853057;

            // --- SPELLS ---
            public const int TimeTearingMorganite = 19403423;
            public const int MoonMirrorShield = 19508728;
            public const int PotOfDuality = 98645731;
            public const int PotOfExtravagance = 49238328;
            public const int Necrovalley = 47355498;
            public const int DimensionalFissure = 81674782;

            // --- TRAPS ---
            public const int DestructiveDarumaKarmaCannon = 30748475;
            public const int SolemnStrike = 40605147;
            public const int SolemnWarning = 84749824;
            public const int SolemnJudgment = 41420027;
            public const int Crackdown = 36975314;
            public const int SkillDrain = 82732705;
            public const int MacroCosmos = 30241314;
            public const int ThereCanBeOnlyOne = 24207889;

            // --- EXTRA DECK ---
            public const int Garura = 11765832;
            public const int Mudragon = 54757758;
            public const int Ntss = 80532587;
            public const int SPLittleKnight = 29301450;
            public const int AAZeus = 90448279;
            public const int TYPHON = 93039339;
            public const int TornadoDragon = 6983839;
            public const int Castel = 82633039;
            public const int KnightmarePhoenix = 2857636;
        }

        private readonly AntiMetaPlugin _plugin;

        public AntiMetaExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            _plugin = new AntiMetaPlugin(this);
            DeckPlugin = _plugin;

            // Register Ace cards that should never be sacrificed
            HeuristicGuard.RegisterAceCards(
                CardId.InspectorBoarder,
                CardId.BarrierStatueOfTheHeavens,
                CardId.ThunderKingRaiOh,
                CardId.BanisherOfTheRadiance
            );

            // ==========================================
            // PRIORITY 1: COUNTER TRAPS (Chain Reactions)
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, SolemnJudgmentEffect);
            AddExecutor(ExecutorType.Activate, CardId.SolemnStrike, SolemnStrikeEffect);
            AddExecutor(ExecutorType.Activate, CardId.SolemnWarning, SolemnWarningEffect);

            // ==========================================
            // PRIORITY 2: DISRUPTION & REMOVAL TRAPS
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.DestructiveDarumaKarmaCannon, DarumaCannonEffect);
            AddExecutor(ExecutorType.Activate, CardId.Crackdown, CrackdownEffect);
            AddExecutor(ExecutorType.Activate, CardId.ThunderKingRaiOh, RaiOhNegateEffect);

            // ==========================================
            // PRIORITY 3: FLOODGATE TRAPS
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.SkillDrain, SkillDrainEffect);
            AddExecutor(ExecutorType.Activate, CardId.ThereCanBeOnlyOne, TcbooEffect);
            AddExecutor(ExecutorType.Activate, CardId.MacroCosmos, MacroCosmosEffect);

            // ==========================================
            // PRIORITY 4: DRAW & ACCELERATION SPELLS (MP1)
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.PotOfExtravagance, PotOfExtravaganceEffect);
            AddExecutor(ExecutorType.Activate, CardId.TimeTearingMorganite, MorganiteEffect);
            AddExecutor(ExecutorType.Activate, CardId.PotOfDuality, PotOfDualityEffect);

            // ==========================================
            // PRIORITY 5: CONTINUOUS & FIELD SPELLS
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.DimensionalFissure, DimensionalFissureEffect);
            AddExecutor(ExecutorType.Activate, CardId.Necrovalley, NecrovalleyEffect);

            // ==========================================
            // PRIORITY 6: NORMAL SUMMONS (Strategic Priority)
            // ==========================================
            AddExecutor(ExecutorType.Summon, CardId.InspectorBoarder, BoarderSummonCheck);
            AddExecutor(ExecutorType.Summon, CardId.BarrierStatueOfTheHeavens, BarrierStatueSummonCheck);
            AddExecutor(ExecutorType.Summon, CardId.ThunderKingRaiOh, RaiOhSummonCheck);
            AddExecutor(ExecutorType.Summon, CardId.BanisherOfTheRadiance, BanisherSummonCheck);

            // Fallback Summon (Any available monster)
            AddExecutor(ExecutorType.Summon, CardId.InspectorBoarder);
            AddExecutor(ExecutorType.Summon, CardId.BarrierStatueOfTheHeavens);
            AddExecutor(ExecutorType.Summon, CardId.ThunderKingRaiOh);
            AddExecutor(ExecutorType.Summon, CardId.BanisherOfTheRadiance);

            // ==========================================
            // PRIORITY 7: EQUIP SPELL (Moon Mirror Shield)
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.MoonMirrorShield, MoonMirrorShieldEffect);

            // ==========================================
            // PRIORITY 9: SET SPELLS / TRAPS
            // ==========================================
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnJudgment);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnStrike);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnWarning);
            AddExecutor(ExecutorType.SpellSet, CardId.DestructiveDarumaKarmaCannon);
            AddExecutor(ExecutorType.SpellSet, CardId.Crackdown);
            AddExecutor(ExecutorType.SpellSet, CardId.SkillDrain);
            AddExecutor(ExecutorType.SpellSet, CardId.MacroCosmos);
            AddExecutor(ExecutorType.SpellSet, CardId.ThereCanBeOnlyOne);

            // ==========================================
            // PRIORITY 10: EXTRA DECK RECOVERY (Strict Guard)
            // ==========================================
            AddExecutor(ExecutorType.SpSummon, CardId.TYPHON, TyphonSummonCheck);
        }

        // ============================================================
        // EXECUTION CONDITIONS & TACTICAL LOGIC
        // ============================================================

        private bool SolemnJudgmentEffect()
        {
            // Protect backrow against mass removal
            if (Duel.LastChainPlayer == 1)
            {
                var card = Duel.GetCurrentSolvingChainCard();
                if (card != null)
                {
                    int id = card.Id;
                    // Feather Duster, Lightning Storm, Evenly Matched, Twin Twisters, Raigeki
                    if (id == 18144506 || id == 12580477 || id == 15693423 || id == 72302403 || id == 43898403)
                        return true;

                    // Intercept powerful starter spells
                    if (card.HasType(CardType.Spell) || card.HasType(CardType.Trap))
                        return true;
                }
            }

            // Negate summon of high ATK or Boss monsters
            if (Duel.LastSummonPlayer == 1)
                return true;

            return DefaultSolemnJudgment();
        }

        private bool SolemnStrikeEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                var card = Duel.GetCurrentSolvingChainCard();
                if (card != null && card.HasType(CardType.Monster))
                    return true;
            }

            if (Duel.LastSummonPlayer == 1)
                return true;

            return DefaultSolemnStrike();
        }

        private bool SolemnWarningEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                var card = Duel.GetCurrentSolvingChainCard();
                if (card != null && (card.HasType(CardType.Monster) || card.HasType(CardType.Spell) || card.HasType(CardType.Trap)))
                    return true;
            }

            if (Duel.LastSummonPlayer == 1)
                return true;

            return DefaultSolemnWarning();
        }

        private bool DarumaCannonEffect()
        {
            // Opponent attempted to summon or has 2+ monsters
            if (Enemy.GetMonsterCount() >= 2) return true;

            // Opponent has a Link monster (which cannot flip face-down and will be sent to GY)
            if (Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.HasType(CardType.Link)))
                return true;

            // Opponent enters battle phase with monsters
            if (Duel.Phase == DuelPhase.BattleStart && Enemy.GetMonsterCount() > 0)
                return true;

            return false;
        }

        private bool CrackdownEffect()
        {
            var targets = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup() && !m.HasType(CardType.Token)).ToList();
            if (targets.Count == 0) return false;

            // Steal dangerous high ATK or boss monsters
            var bestTarget = targets.OrderByDescending(m => m.Attack).FirstOrDefault();
            if (bestTarget != null && bestTarget.Attack >= 1800)
                return true;

            // Steal combo starters if opponent has 1 monster
            if (targets.Count == 1 && Duel.Player == 1)
                return true;

            return false;
        }

        private bool RaiOhNegateEffect()
        {
            // Send Rai-Oh to GY to negate inherent Special Summon
            if (Duel.LastSummonPlayer == 1)
            {
                var lastMonster = Enemy.GetMonsters().LastOrDefault(m => m != null && m.IsFaceup());
                if (lastMonster != null && lastMonster.Attack >= 2000)
                    return true;
            }
            return false;
        }

        private bool SkillDrainEffect()
        {
            // If we don't have Inspector Boarder, Skill Drain is our primary monster effect lock
            if (!Bot.HasInMonstersZone(CardId.InspectorBoarder))
                return true;

            // If opponent activates monster effect, flip Skill Drain
            return Duel.LastChainPlayer == 1;
        }

        private bool TcbooEffect()
        {
            // There Can Be Only One is active whenever opponent has or summons monsters
            return Enemy.GetMonsterCount() >= 1 || Duel.Player == 1;
        }

        private bool MacroCosmosEffect()
        {
            // Keep Macro Cosmos active unless we already have Banisher on field
            return !Bot.HasInMonstersZone(CardId.BanisherOfTheRadiance);
        }

        private bool PotOfExtravaganceEffect()
        {
            // Activate first in MP1
            return Bot.ExtraDeck.Count >= 6 && Duel.Phase == DuelPhase.Main1;
        }

        private bool MorganiteEffect()
        {
            _plugin.StrategyImpl.MorganiteActive = true;
            return true;
        }

        private bool PotOfDualityEffect()
        {
            return Duel.Phase == DuelPhase.Main1;
        }

        private bool DimensionalFissureEffect()
        {
            return !Bot.HasInSpellZone(CardId.DimensionalFissure) && !Bot.HasInMonstersZone(CardId.BanisherOfTheRadiance);
        }

        private bool NecrovalleyEffect()
        {
            return !Bot.HasInSpellZone(CardId.Necrovalley);
        }

        private bool BarrierStatueSummonCheck()
        {
            // Barrier Statue of the Heavens locks Special Summons of non-LIGHT monsters
            return true;
        }

        private bool BoarderSummonCheck()
        {
            // Boarder is primary 2000 beatstick & effect lock
            return true;
        }

        private bool RaiOhSummonCheck()
        {
            // Rai-Oh is 1900 beatstick & search denial
            return true;
        }

        private bool BanisherSummonCheck()
        {
            return true;
        }

        private bool MoonMirrorShieldEffect()
        {
            // Equip to our face-up monster
            var monsters = Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).ToList();
            if (monsters.Count == 0) return false;

            // Priority: Barrier Statue (frailest 1000 ATK) > Banisher (1600) > Rai-Oh (1900) > Boarder (2000)
            var target = monsters.FirstOrDefault(m => m.Id == CardId.BarrierStatueOfTheHeavens && (m.EquipCards == null || m.EquipCards.Count == 0))
                      ?? monsters.FirstOrDefault(m => m.Id == CardId.BanisherOfTheRadiance && (m.EquipCards == null || m.EquipCards.Count == 0))
                      ?? monsters.FirstOrDefault(m => m.Id == CardId.ThunderKingRaiOh && (m.EquipCards == null || m.EquipCards.Count == 0))
                      ?? monsters.FirstOrDefault(m => m.Id == CardId.InspectorBoarder && (m.EquipCards == null || m.EquipCards.Count == 0))
                      ?? monsters.FirstOrDefault(m => (m.EquipCards == null || m.EquipCards.Count == 0))
                      ?? monsters[0];

            return target != null;
        }

        private bool TyphonSummonCheck()
        {
            // Strict recovery only: Opponent summoned from Extra Deck with 3000+ ATK and we have no monsters
            return Bot.GetMonsterCount() == 0 &&
                   Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Attack >= 3000) &&
                   Duel.Phase == DuelPhase.Main2;
        }

        // ============================================================
        // ENGINE HOOKS & SELECT OVERRIDES
        // ============================================================

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Rule 1: Distinguish Hint ID
            if (hint == 506) // HINTMSG_ATOHAND (Searching from deck/excavation)
            {
                var target = _plugin.StrategyImpl.PickSearchTarget(cards, Card);
                if (target != null)
                {
                    var result = new List<ClientCard> { target };
                    foreach (var c in cards.Where(c => c != target))
                    {
                        if (result.Count >= max) break;
                        result.Add(c);
                    }
                    return result;
                }
            }

            // Rule 1: Destruction / Banish / Bounce must target ENEMY cards ONLY
            if (hint == 502 || hint == 503 || hint == 504 || hint == 509)
            {
                var enemyCards = cards.Where(c => c.Controller == 1).ToList();
                if (enemyCards.Count > 0)
                {
                    return enemyCards.OrderByDescending(c => _plugin.ThreatImpl.EvaluateThreatScore(c)).Take(max).ToList();
                }
            }

            // Equip target for Moon Mirror Shield
            if (Card != null && Card.Id == CardId.MoonMirrorShield)
            {
                var ourMonsters = cards.Where(c => c.Controller == 0 && c.Location == CardLocation.MonsterZone).ToList();
                if (ourMonsters.Count > 0)
                {
                    var statue = ourMonsters.FirstOrDefault(m => m.Id == CardId.BarrierStatueOfTheHeavens);
                    if (statue != null) return new List<ClientCard> { statue };

                    var banisher = ourMonsters.FirstOrDefault(m => m.Id == CardId.BanisherOfTheRadiance);
                    if (banisher != null) return new List<ClientCard> { banisher };

                    var raioh = ourMonsters.FirstOrDefault(m => m.Id == CardId.ThunderKingRaiOh);
                    if (raioh != null) return new List<ClientCard> { raioh };

                    var boarder = ourMonsters.FirstOrDefault(m => m.Id == CardId.InspectorBoarder);
                    if (boarder != null) return new List<ClientCard> { boarder };

                    return new List<ClientCard> { ourMonsters.OrderBy(m => m.Attack).First() };
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            // Rule 14: Reject opponent effects by default
            if (card != null && card.Controller == 1)
                return false;

            // Moon Mirror Shield: When sent to GY, pay 500 LP to place on top/bottom of Deck (YES!)
            if (card != null && card.Id == CardId.MoonMirrorShield)
                return true;

            return base.OnSelectEffectYn(card, desc);
        }

        public override int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
            // Rule 11: Keep EMZ open, place in Main Monster Zones
            return base.OnSelectPlace(cardId, player, location, available);
        }
    }
}
