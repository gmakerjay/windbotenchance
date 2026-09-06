using YGOSharp.OCGWrapper.Enums;
using YGOSharp.OCGWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    // ====================================================================================================
    //  2026_RyuGe โ€” RYU-GE STRATEGY EXECUTOR
    // ====================================================================================================
    //
    // เธเธฒเธกเน€เธ”เนเธ: 2026_RyuGe
    // เธเธณเธญเธเธดเธเธฒเธขเธเธฅเธขเธธเธ—เธเนเนเธฅเธฐเธเธฒเธฃเธ—เธณเธเธฒเธเธซเธฅเธฑเธ (Strategy Overview):
    //  1. Multi-Realm Control (เธเธฒเธฃเธเธฒเธเน€เธงเธ—เธกเธเธ•เธฃเนเธ•เนเธญเน€เธเธทเนเธญเธเธซเธฅเธฑเธ):
    //     - Dino Domains: เน€เธเธดเนเธกเธเธฅเธฑเธเนเธเธกเธ•เธตเนเธฅเธฐเธเธฑเธ”เธเธงเธฒเธเน€เธญเธเน€เธเธเธ•เนเธกเธญเธเธชเน€เธ•เธญเธฃเนเธ—เธตเนเธกเธตเธเธฅเธฑเธเนเธเธกเธ•เธตเธเนเธญเธขเธเธงเนเธฒ
    //     - Sea Spires: เธเธเธเนเธญเธเธเธฒเธฃเธ•เนเธญเธชเธนเนเนเธฅเธฐเธเนเธฒเธกเธชเธดเนเธเธเธตเธ”เธเธงเธฒเธเนเธ”เธขเธเธฒเธฃเน€เธ”เนเธเธเธฒเธฃเนเธ”เธเธนเนเธ•เนเธญเธชเธนเนเธเธฅเธฑเธเธเธถเนเธเธกเธทเธญ
    //     - Wyrm Winds: เธเธงเธเธเธธเธกเธชเธธเธชเธฒเธเนเธ”เธขเนเธเธเธเธฒเธฃเนเธ”เธจเธฑเธ•เธฃเธนเธ—เธตเนเธเธฐเธ•เธเธฅเธเธชเธธเธชเธฒเธ เนเธฅเธฐเธฅเธ” ATK เธจเธฑเธ•เธฃเธนเน€เธซเธฅเธทเธญ 0
    //  2. Ritual/Pendulum Swarm Engine:
    //     - Sosei Ryu-Ge Mistva: เธญเธฑเธเน€เธเธดเธเธเธดเน€เธจเธฉเธเธฒเธ ED เน€เธกเธทเนเธญเธกเธตเธเธฒเธฃเธ—เธณเธฅเธฒเธขเน€เธเธดเธ”เธเธถเนเธเน€เธเธทเนเธญเธเนเธงเธขเธเธฒเธเน€เธงเธ—เธกเธเธ•เธฃเนเธ•เนเธญเน€เธเธทเนเธญเธ
    //     - Tensei Ryu-Ge Anva: เธญเธฑเธเน€เธเธดเธเนเธ”เธขเธชเนเธเธ•เธฑเธงเธญเธทเนเธ เธขเธดเธเธเธฒเธฃเนเธ”เธเธฑเนเธเธ•เธฃเธเธเนเธฒเธก เนเธฅเธฐเธเธฒเธเน€เธงเธ—เธกเธเธ•เธฃเนเธ•เนเธญเน€เธเธทเนเธญเธเน€เธกเธทเนเธญเธ–เธนเธเธ—เธณเธฅเธฒเธข
    //  3. Rank 10 Boss Monsters:
    //     - Varudras: เธเธญเธชเนเธซเธเนเธเธญเธขเธเธฑเธ”เธเธงเธฒเธเนเธฅเธฐเธฅเธเธเธฒเธฃเนเธ”เธเธนเนเธ•เนเธญเธชเธนเน (Omni-Negate)
    //
    // ====================================================================================================

    // ==========================================
    // 2026_RyuGe
    // ==========================================
    // ============================================================
    // CARD AUDIT โ€” 2026_RyuGe
    // ============================================================
    // Card Name                   | Type       | OPT? | Effect Summary                            | Activate When                             | NEVER Activate When                          |
    // ----------------------------|------------|------|-------------------------------------------|-------------------------------------------|----------------------------------------------|
    // Genro Ryu-Ge Hakva          | Monster    | Yes  | Hand: search Wyrms; Field: SS from GY     | Need Wyrm search / GY setup               | Already used / no targets                   |
    // Kairo Ryu-Ge Emva          | Monster    | Yes  | Hand: search SeaSpires; Field: bounce hand| Need SeaSpires / opp hand > 0             | Already used / no targets                   |
    // Kyoro Ryu-Ge Kaiva         | Monster    | Yes  | Hand: search DinoDomains; Field: destroy  | Need DinoDomains / enemy cards to destroy | Already used / no enemy cards               |
    // Sosei Ryu-Ge Mistva        | Monster    | Yes  | Place in PZone; SS from ED when destroyed | Open PZone / field has Lv10               | Used already / no Lv10 target               |
    // Tensei Ryu-Ge Anva         | Monster    | Yes  | SS by tribute; PZone bounce + destroy     | Have tribute fodder / enemy cards         | Used already / no tribute                   |
    // Ryu-Ge Rising              | Spell      | Yes  | Search RyuGe; GY banish to add from GY    | Need RyuGe search / have RyuGe in GY      | Already used this turn / no targets         |
    // Ryu-Ge War Zone            | Spell      | Yes  | Search RyuGe; SS RyuGe Pendulum from ED   | Need RyuGe search / have Pendulum in ED   | Already face-up / used                      |
    // Dino Domains               | Spell      | No   | Negate low-ATK monster effects            | Have Dino Lv10 + RyuGe on field           | No Dino Lv10 / no RyuGe pendulum            |
    // Sea Spires                 | Spell      | No   | Bounce opponent card when attacked        | Have SeaSerpent Lv10 + RyuGe on field     | No SeaSerpent Lv10 / no enemy cards         |
    // Wyrm Winds                 | Spell      | No   | Banish opponent cards from GY; Reduce ATK | Have Wyrm Lv10 + RyuGe on field           | No Wyrm Lv10 / no enemy monsters            |
    // ============================================================
    // ACE CARDS:
    //   Primary  : Varudras โ€” omni-negate + destroy boss
    //   Secondary: Sosei Ryu-Ge Mistva โ€” recursion + Pendulum scale
    //   Tertiary : Tensei Ryu-Ge Anva โ€” bounce + destroy engine
    // ============================================================
    // STRATEGY:
    //   Turn 1: Rising โ’ search Anva โ’ Hakva/Emva/Kaiva โ’ set Realms
    //   Turn 2: Varudras Xyz + Realm control + Mistva recursion
    //   Engine: Lv10 engines + Realms for multi-realm control
    // ============================================================
    [Deck("2026_RyuGe", "2026_RyuGe")]
    public class _2026_RyuGeExecutor : ModernExecutor
    {
        public class CardId
        {
            // --- MAIN DECK MONSTERS ---
            public const int DivinerOfTheHerald = 92919429;
            public const int AshBlossom = 14558127;
            public const int GenroRyuGeHakva = 56499179;
            public const int TenseiRyuGeAnva = 56322832;
            public const int DrollAndLockBird = 94145021;
            public const int MaxxC = 23434538;
            public const int KairoRyuGeEmva = 20904475;
            public const int KyoroRyuGeKaiva = 93509766;
            public const int Nibiru = 27204311;
            public const int SoseiRyuGeMistva = 92487128;

            // --- MAIN DECK SPELLS/TRAPS ---
            public const int RyuGeRising = 29882827;
            public const int RyuGeWarZone = 55276522;
            public const int SuperPolymerization = 48130397;
            public const int TheMelodyOfAwakeningDragon = 48800175;
            public const int CalledByTheGrave = 24224830;
            public const int RyuGeRealmDinoDomains = 82661630;
            public const int RyuGeRealmSeaSpires = 28669235;
            public const int RyuGeRealmWyrmWinds = 55154344;
            public const int CrossoutDesignator = 65681983;
            public const int InfiniteImpermanence = 10045474;

            // --- EXTRA DECK FUSIONS & OTHERS ---
            public const int ElderEntityNtss = 80532587;
            public const int MudragonOfTheSwamp = 54757758;
            public const int Garura = 11765832;
            public const int EarthGolemIgnister = 62111090;
            public const int KhaosStarsourceDragon = 72578374;
            public const int StarvingVenom = 41209827;
            public const int HeraldOfTheArcLight = 79606837;
            public const int TranscendosaurusGlaciasaurus = 94130731;
            public const int Varudras = 70636044;
            public const int NumberXX = 21858819;
            public const int GalaxyDestroyer = 66523544;
            public const int Zeus = 90448279;
            public const int TyPhon = 93039339;
            public const int SPLittleKnight = 29301450;
        }

        // --- GOING FIRST/SECOND TRACKING ---

        // --- OPT FLAGS & ENGINE STATE ---
        private bool _warZoneActivatedThisTurn = false;
        private bool _risingActivatedThisTurn = false;
        private bool _mistvaScalePlaced = false;
        private bool _mistvaPZoneEffectUsed = false;
        private bool _mistvaMonsterUsed = false;
        private bool _anvaMonsterUsed = false;
        private bool _anvaPZoneUsed = false;
        private bool _hakvaHandUsed = false;
        private bool _hakvaFieldUsed = false;
        private bool _emvaHandUsed = false;
        private bool _emvaFieldUsed = false;
        private bool _kaivaHandUsed = false;
        private bool _kaivaFieldUsed = false;
        private bool _divinerUsedThisTurn = false;

        // FieldGuard inherited: IsSpecialSummonBlocked, CanDealLethal, CanOTK,
        // ShouldSkipCombo, NeedsBoardPresence, IsInGrindGame, EnemyHasKnownNegate โ’ inherited

        protected override bool IsBoardStrongEnough()
        {
            // Deck-specific: Varudras + Realm = strong control
            if (Bot.HasInMonstersZone(CardId.Varudras) && Bot.GetSpells().Any(c => c != null && c.IsFaceup() &&
                (c.Id == CardId.RyuGeRealmDinoDomains || c.Id == CardId.RyuGeRealmSeaSpires || c.Id == CardId.RyuGeRealmWyrmWinds)))
                return true;
            if (Bot.HasInMonstersZone(CardId.TenseiRyuGeAnva) && Bot.GetSpellCount() >= 2)
                return true;
            if (Bot.HasInMonstersZone(CardId.SoseiRyuGeMistva) && Bot.GetSpells().Any(c => c != null && c.IsFaceup() &&
                (c.Id == CardId.RyuGeRealmDinoDomains || c.Id == CardId.RyuGeRealmSeaSpires || c.Id == CardId.RyuGeRealmWyrmWinds)))
                return true;
            int realmCount = Bot.GetSpells().Count(c => c != null && c.IsFaceup() &&
                (c.Id == CardId.RyuGeRealmDinoDomains || c.Id == CardId.RyuGeRealmSeaSpires || c.Id == CardId.RyuGeRealmWyrmWinds));
            if (realmCount >= 2 && Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Level >= 10))
                return true;
            return base.IsBoardStrongEnough();
        }

        private bool OpponentHasThreateningMonster()
        {
            return Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && !c.IsDisabled() &&
                (c.IsFloodgate() || c.Attack >= 3000 || c.HasType(CardType.Link) || c.HasType(CardType.Synchro)));
        }

        private static readonly int[] AceCardIds = {
            CardId.Varudras,
            CardId.SoseiRyuGeMistva,
            CardId.TenseiRyuGeAnva
        };

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Contains(card.Id);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.Id == CardId.AshBlossom || c.Id == CardId.DrollAndLockBird || c.Id == CardId.MaxxC) return 800;
            if (c.Id == CardId.GenroRyuGeHakva) return 100;
            if (c.Id == CardId.KairoRyuGeEmva) return 110;
            if (c.Id == CardId.KyoroRyuGeKaiva) return 120;
            return base.GetMaterialPriority(c);
        }

        public _2026_RyuGeExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // โ”€โ”€ Combo Router: Sequencing โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Standard-Setup",
                RequiredCards = new List<int> { CardId.DivinerOfTheHerald, CardId.GenroRyuGeHakva },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.DivinerOfTheHerald, ActionType = ExecutorType.Activate, Description = "Play CardId.DivinerOfTheHerald" },
                    new() { CardId = CardId.GenroRyuGeHakva, ActionType = ExecutorType.Activate, Description = "Extend with CardId.GenroRyuGeHakva" }
                },
                EndBoardScore = 80
            });

            // โ”€โ”€ Bait Planner โ”€โ”€
            BaitPlanner.RegisterComboStarters(CardId.DivinerOfTheHerald, CardId.AshBlossom);
            BaitPlanner.RegisterBaitCards(CardId.AshBlossom);

            // โ”€โ”€ Chain Advisor โ”€โ”€
            ChainAdvisor.RegisterHighValueTargets(CardId.DivinerOfTheHerald, CardId.AshBlossom, CardId.InfiniteImpermanence, CardId.SPLittleKnight);

            // --- GOING FIRST/SECOND STRATEGY ---
            _isGoingSecond = (Duel.Turn > 1);

            // --- TIER 1: Hand Traps & Counter Measures ---
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, () => SmartHandTrapChain() && DefaultMaxxC());
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, () => SmartHandTrapChain() && DefaultAshBlossomAndJoyousSpring());
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdEffect);
            AddExecutor(ExecutorType.Activate, CardId.Nibiru, NibiruEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.CrossoutDesignator, CrossoutDesignatorEffect);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, () => SmartHandTrapChain() && DefaultInfiniteImpermanence());

            // --- TIER 2: Boss Monster Quick Effects & Negates ---
            AddExecutor(ExecutorType.Activate, CardId.Varudras, VarudrasEffect);
            AddExecutor(ExecutorType.Activate, CardId.TenseiRyuGeAnva, AnvaPZoneEffect);
            AddExecutor(ExecutorType.Activate, CardId.RyuGeRealmDinoDomains, DinoDomainsNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.RyuGeRealmSeaSpires, SeaSpiresBounceEffect);
            AddExecutor(ExecutorType.Activate, CardId.RyuGeRealmWyrmWinds, WyrmWindsATKReduceEffect);

            // --- TIER 3: Extenders, Board Breakers & Searches ---
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheMelodyOfAwakeningDragon, MelodyEffect);
            AddExecutor(ExecutorType.Activate, CardId.RyuGeWarZone, WarZoneSS);
            AddExecutor(ExecutorType.Activate, CardId.RyuGeWarZone, WarZoneEffect);
            AddExecutor(ExecutorType.Activate, CardId.RyuGeRising, RyuGeRisingEffect);
            AddExecutor(ExecutorType.Activate, CardId.SoseiRyuGeMistva, MistvaPZoneEffect);
            AddExecutor(ExecutorType.Activate, CardId.SoseiRyuGeMistva, MistvaMonsterEffect);

            // --- TIER 4: Normal Summons & Archetype Summoning Effects ---
            AddExecutor(ExecutorType.Activate, CardId.TenseiRyuGeAnva, AnvaSummonOrEffect);
            AddExecutor(ExecutorType.Summon, CardId.DivinerOfTheHerald, DivinerSummon);
            AddExecutor(ExecutorType.Activate, CardId.DivinerOfTheHerald, DivinerEffect);
            AddExecutor(ExecutorType.Activate, CardId.HeraldOfTheArcLight);
            AddExecutor(ExecutorType.Activate, CardId.ElderEntityNtss);
            AddExecutor(ExecutorType.Activate, CardId.Garura);
            
            AddExecutor(ExecutorType.Activate, CardId.GenroRyuGeHakva, HakvaBanishSS);
            AddExecutor(ExecutorType.Activate, CardId.GenroRyuGeHakva, HakvaFieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.GenroRyuGeHakva, HakvaHandEffect);

            AddExecutor(ExecutorType.Activate, CardId.KairoRyuGeEmva, EmvaGYSS);
            AddExecutor(ExecutorType.Activate, CardId.KairoRyuGeEmva, EmvaFieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.KairoRyuGeEmva, EmvaHandEffect);

            AddExecutor(ExecutorType.Activate, CardId.KyoroRyuGeKaiva, KaivaHandSS);
            AddExecutor(ExecutorType.Activate, CardId.KyoroRyuGeKaiva, KaivaFieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.KyoroRyuGeKaiva, KaivaHandEffect);

            // --- TIER 5: Realms Activations ---
            AddExecutor(ExecutorType.Activate, CardId.RyuGeRealmDinoDomains, DinoDomainsEffect);
            AddExecutor(ExecutorType.Activate, CardId.RyuGeRealmSeaSpires, SeaSpiresEffect);
            AddExecutor(ExecutorType.Activate, CardId.RyuGeRealmWyrmWinds, WyrmWindsEffect);

            // --- TIER 6: Extra Deck Summons ---
            AddExecutor(ExecutorType.SpSummon, CardId.Varudras, VarudrasSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.GalaxyDestroyer, GalaxyDestroyerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Zeus, ZeusSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.TyPhon, TyPhonSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSummon);

            // --- TIER 7: Spell Sets & Monster Positioning ---
            AddExecutor(ExecutorType.SpellSet, RyuGeSpellSet);
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // RyuGe is a combo/control deck โ€” prefer going first to set up Realms + Varudras
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _warZoneActivatedThisTurn = false;
            _risingActivatedThisTurn = false;
            _mistvaScalePlaced = false;
            _mistvaPZoneEffectUsed = false;
            _mistvaMonsterUsed = false;
            _anvaMonsterUsed = false;
            _anvaPZoneUsed = false;
            _hakvaHandUsed = false;
            _hakvaFieldUsed = false;
            _emvaHandUsed = false;
            _emvaFieldUsed = false;
            _kaivaHandUsed = false;
            _kaivaFieldUsed = false;
            _divinerUsedThisTurn = false;

            // ── Going-Second BreakBoard: prioritize disruption over combo ──
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
            }
        }

        public override void OnChaining(int player, ClientCard card)
        {
            base.OnChaining(player, card);
            if (player == 0 && card != null)
            {
                if (card.Id == CardId.RyuGeWarZone) _warZoneActivatedThisTurn = true;
                if (card.Id == CardId.RyuGeRising) _risingActivatedThisTurn = true;
                if (card.Id == CardId.SoseiRyuGeMistva)
                {
                    if (card.Location == CardLocation.Hand) _mistvaScalePlaced = true;
                    else if (card.Location == CardLocation.SpellZone) _mistvaPZoneEffectUsed = true;
                    else _mistvaMonsterUsed = true;
                }
                if (card.Id == CardId.TenseiRyuGeAnva)
                {
                    if (card.Location == CardLocation.Hand) _anvaMonsterUsed = true;
                    else if (card.Location == CardLocation.SpellZone) _anvaPZoneUsed = true;
                    else _anvaMonsterUsed = true;
                }
                if (card.Id == CardId.GenroRyuGeHakva)
                {
                    if (card.Location == CardLocation.Hand) _hakvaHandUsed = true;
                    else _hakvaFieldUsed = true;
                }
                if (card.Id == CardId.KairoRyuGeEmva)
                {
                    if (card.Location == CardLocation.Hand) _emvaHandUsed = true;
                    else _emvaFieldUsed = true;
                }
                if (card.Id == CardId.KyoroRyuGeKaiva)
                {
                    if (card.Location == CardLocation.Hand) _kaivaHandUsed = true;
                    else _kaivaFieldUsed = true;
                }
                if (card.Id == CardId.DivinerOfTheHerald) _divinerUsedThisTurn = true;
            }
        }

        private long StringId(int cardId, int optionIndex)
        {
            return ((long)cardId << 4) + optionIndex;
        }

        public override bool OnSelectYesNo(long desc)
        {
            // If opponent has no cards on field, do NOT say yes to optional destruction effects!
            if (Enemy.GetFieldCount() == 0)
            {
                var active = Card ?? LastChainCard;
                if (active != null && (active.Id == CardId.Varudras || active.Id == CardId.TenseiRyuGeAnva || active.Id == CardId.SoseiRyuGeMistva))
                    return false;
            }

            // Ryu-Ge Rising: place Mistva from hand into face-up Extra Deck
            if (desc == StringId(CardId.RyuGeRising, 2))
            {
                return Bot.HasInHand(CardId.SoseiRyuGeMistva);
            }

            if (desc == StringId(CardId.RyuGeRising, 0) || 
                desc == StringId(CardId.RyuGeRising, 1))
            {
                bool hasWarZone = Bot.HasInHand(CardId.RyuGeWarZone) || Bot.HasInSpellZone(CardId.RyuGeWarZone, faceUp: true);
                return hasWarZone;
            }
            return true;
        }

        // --- STRATEGIC SITUATIONAL CHECKS ---

        private bool DrollAndLockBirdEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (Duel.Player == 1 && LastChainCard != null && LastChainCard.Controller == 1)
            {
                if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.DrollAndLockBird))
                    return false;
                return true;
            }
            return false;
        }

        private bool NibiruEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                if (Enemy.GetMonsterCount() >= 2 || Enemy.GetMonsters().Any(c => c.Attack >= 2500))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsSameCard(int id1, int id2)
        {
            if (id1 == id2) return true;
            var data1 = NamedCard.Get(id1);
            var data2 = NamedCard.Get(id2);
            int alias1 = data1 != null ? data1.Alias : 0;
            int alias2 = data2 != null ? data2.Alias : 0;
            int code1 = alias1 != 0 ? alias1 : id1;
            int code2 = alias2 != 0 ? alias2 : id2;
            return code1 == code2;
        }

        private bool CrossoutDesignatorEffect()
        {
            // NEVER chain to own cards โ€” Crossout crashes engine when mis-timed
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard == null || LastChainCard.Controller != 1) return false;

            // Only negate monster effects
            if (!LastChainCard.IsMonster()) return false;
            
            int matchId = 0;
            foreach (int id in StartingDeck.Cards)
            {
                if (IsSameCard(id, LastChainCard.Id))
                {
                    matchId = id;
                    break;
                }
            }
            
            if (matchId != 0 && GetRemainingCount(matchId) > 0)
            {
                AI.SelectAnnounceID(matchId);
                return true;
            }
            return false;
        }

        private bool VarudrasEffect()
        {
            // Omni-negate: always negate opponent card or effect activation!
            if (LastChainCard != null && LastChainCard.Controller == 1)
            {
                return true;
            }
            // Detach to destroy: when opponent controls cards on field
            if (Card != null && Card.Overlays.Count >= 1)
            {
                return Enemy.GetFieldCount() > 0;
            }
            return false;
        }

        private bool AnvaPZoneEffect()
        {
            if (Card == null || Card.Location != CardLocation.SpellZone) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (_anvaPZoneUsed) return false;
            // Only activate if enemy has meaningful cards to bounce/destroy
            if (Enemy.GetFieldCount() == 0) return false;
            return true;
        }

        private bool DinoDomainsNegateEffect()
        {
            if (LastChainCard == null || LastChainCard.Controller != 1 || !LastChainCard.IsMonster()) return false;
            var dino = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && 
                ((c.Level >= 10 && c.HasRace(CardRace.Dinosaur)) || c.Id == CardId.TenseiRyuGeAnva || c.Id == CardId.SoseiRyuGeMistva));
            if (dino == null) return false;
            if (LastChainCard.Attack > dino.Attack) return false;
            
            bool hasSpellToCost = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && (c.Id == CardId.RyuGeRealmDinoDomains || c.Id == CardId.RyuGeRealmSeaSpires || c.Id == CardId.RyuGeRealmWyrmWinds));
            return hasSpellToCost;
        }

        private bool SeaSpiresBounceEffect()
        {
            if (Duel.Player != 1) return false;
            var seaSerpent = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && 
                ((c.Level >= 10 && c.HasRace(CardRace.SeaSerpent)) || c.Id == CardId.TenseiRyuGeAnva || c.Id == CardId.SoseiRyuGeMistva));
            if (seaSerpent == null) return false;
            if (Enemy.GetFieldCount() == 0) return false;
            
            bool hasSpellToCost = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && (c.Id == CardId.RyuGeRealmDinoDomains || c.Id == CardId.RyuGeRealmSeaSpires || c.Id == CardId.RyuGeRealmWyrmWinds));
            return hasSpellToCost;
        }

        private bool WyrmWindsATKReduceEffect()
        {
            if (Duel.Player != 1) return false;
            var wyrm = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && 
                ((c.Level >= 10 && c.HasRace(CardRace.Wyrm)) || c.Id == CardId.TenseiRyuGeAnva || c.Id == CardId.SoseiRyuGeMistva));
            if (wyrm == null) return false;
            if (!Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack > 0)) return false;
            
            bool hasSpellToCost = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && (c.Id == CardId.RyuGeRealmDinoDomains || c.Id == CardId.RyuGeRealmSeaSpires || c.Id == CardId.RyuGeRealmWyrmWinds));
            return hasSpellToCost;
        }

        private bool SuperPolymerizationEffect()
        {
            if (Bot.Hand.Count < 1) return false;
            var opponentMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            var myMonsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            var allMonsters = opponentMonsters.Concat(myMonsters).ToList();

            if (Bot.HasInExtra(CardId.StarvingVenom))
            {
                var darkMonsters = allMonsters.Where(c => c.HasAttribute(CardAttribute.Dark)).ToList();
                if (darkMonsters.Count >= 2 && darkMonsters.Any(c => c.Controller == 1)) return true;
            }
            if (Bot.HasInExtra(CardId.MudragonOfTheSwamp))
            {
                foreach (var attr in Enum.GetValues(typeof(CardAttribute)).Cast<CardAttribute>())
                {
                    var matching = allMonsters.Where(c => c.HasAttribute(attr)).ToList();
                    if (matching.Count >= 2 && matching.Any(c => c.Controller == 1))
                    {
                        var races = matching.Select(c => c.Race).Distinct().ToList();
                        if (races.Count >= 2) return true;
                    }
                }
            }
            if (Bot.HasInExtra(CardId.Garura))
            {
                foreach (var attr in Enum.GetValues(typeof(CardAttribute)).Cast<CardAttribute>())
                {
                    foreach (var race in Enum.GetValues(typeof(CardRace)).Cast<CardRace>())
                    {
                        var matching = allMonsters.Where(c => c.HasAttribute(attr) && c.HasRace(race)).ToList();
                        if (matching.Count >= 2 && matching.Any(c => c.Controller == 1))
                        {
                            var names = matching.Select(c => c.Name).Distinct().ToList();
                            if (names.Count >= 2) return true;
                        }
                    }
                }
            }
            if (Bot.HasInExtra(CardId.EarthGolemIgnister))
            {
                bool hasCyberse = allMonsters.Any(c => c.HasRace(CardRace.Cyberse));
                bool hasLink = allMonsters.Any(c => c.HasType(CardType.Link));
                if (hasCyberse && hasLink && (opponentMonsters.Any(c => c.HasRace(CardRace.Cyberse)) || opponentMonsters.Any(c => c.HasType(CardType.Link)))) return true;
            }
            return false;
        }

        private bool MelodyEffect()
        {
            if (Bot.Hand.Count < 2) return false;
            // Melody of Awakening Dragon searches Dragon monsters with 3000+ ATK & <=2500 DEF (Anva and Mistva)
            bool needsAnva = !Bot.HasInHand(CardId.TenseiRyuGeAnva) && Bot.GetRemainingCount(CardId.TenseiRyuGeAnva, 3) > 0;
            bool needsMistva = !Bot.HasInHand(CardId.SoseiRyuGeMistva) && Bot.GetRemainingCount(CardId.SoseiRyuGeMistva, 3) > 0;
            return needsAnva || needsMistva;
        }

        private bool WarZoneEffect()
        {
            if (Card == null || Card.Location != CardLocation.Hand) return false;
            if (_warZoneActivatedThisTurn) return false;
            if (Bot.HasInSpellZone(CardId.RyuGeWarZone, faceUp: true)) return false;
            if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.RyuGeWarZone)) return false;
            return true;
        }

        private bool RyuGeRisingEffect()
        {
            if (Card == null) return false;
            if (Card.Location == CardLocation.Hand)
            {
                if (Duel.Player == 1) return false; // Can't activate from hand on opponent's turn
                if (Bot.HasInSpellZone(CardId.RyuGeRising, faceUp: true)) return false;
                if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.RyuGeRising)) return false;
                return !_risingActivatedThisTurn;
            }
            if (Card.Location == CardLocation.Grave || Card.Location == CardLocation.SpellZone)
            {
                // GY/Field banish effect (Quick Effect): can be activated during either player's Main Phase
                if (Duel.Player == 1 && Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2)
                    return false;
                
                // Make sure we have a Ryu-Ge monster in GY to banish
                bool hasGyMonster = Bot.Graveyard.Any(c => c != null && 
                    (c.Id == CardId.GenroRyuGeHakva || c.Id == CardId.KairoRyuGeEmva || 
                     c.Id == CardId.KyoroRyuGeKaiva || c.Id == CardId.SoseiRyuGeMistva || 
                     c.Id == CardId.TenseiRyuGeAnva));
                
                return hasGyMonster;
            }
            return false;
        }

        private bool MistvaPZoneEffect()
        {
            if (Card == null) return false;
            if (Card.Location == CardLocation.SpellZone)
            {
                if (_mistvaPZoneEffectUsed) return false;
                // PZone Effect: Search non-Pendulum RyuGe card (War Zone / Rising / Kaiva) and destroy self to Extra Deck!
                return true;
            }
            if (Card.Location == CardLocation.Hand)
            {
                if (_mistvaScalePlaced) return false;
                // Place into PZone if open
                return Util.GetPZone(0, 0) == null || Util.GetPZone(0, 1) == null;
            }
            return false;
        }

        private bool MistvaMonsterEffect()
        {
            if (Card == null) return false;
            if (_mistvaMonsterUsed) return false;
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Field effect: destroy 1 card we control -> SS 1 Ryu-Ge monster from Deck!
                int ourFieldCount = Bot.GetMonsterCount() + Bot.GetSpellCount();
                return ourFieldCount >= 2;
            }
            if (Card.Location == CardLocation.Extra)
            {
                // Extra Deck Trigger: when a monster was destroyed, tribute 1 Level 10 Ryu-Ge on field -> Special Summon Mistva -> destroy up to 2 cards to place Continuous Spells!
                bool hasLvl10Tribute = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Level == 10 && c.Id != CardId.SoseiRyuGeMistva);
                return hasLvl10Tribute;
            }
            return false;
        }

        private bool DivinerSummon()
        {
            if (_divinerUsedThisTurn) return false;
            if (IsSpecialSummonBlocked()) return false;
            return (GetRemainingCount(CardId.HeraldOfTheArcLight) > 0 || GetRemainingCount(CardId.ElderEntityNtss) > 0 || GetRemainingCount(CardId.Garura) > 0);
        }

        private bool DivinerEffect()
        {
            if (_divinerUsedThisTurn) return false;
            return true;
        }

        private bool HakvaBanishSS()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool HakvaFieldEffect()
        {
            if (Card == null || Card.Location != CardLocation.MonsterZone) return false;
            if (_hakvaFieldUsed) return false;
            if (!Bot.HasInSpellZone(CardId.RyuGeRealmWyrmWinds, faceUp: true)) return false;
            return true;
        }

        private bool HakvaHandEffect()
        {
            if (Card == null || Card.Location != CardLocation.Hand) return false;
            if (_hakvaHandUsed) return false;
            return Bot.GetRemainingCount(CardId.RyuGeRealmWyrmWinds, 1) > 0;
        }

        private bool EmvaGYSS()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool EmvaFieldEffect()
        {
            if (Card == null || Card.Location != CardLocation.MonsterZone) return false;
            if (_emvaFieldUsed) return false;
            if (!Bot.HasInSpellZone(CardId.RyuGeRealmSeaSpires, faceUp: true) || Enemy.GetHandCount() == 0) return false;
            return Enemy.GetHandCount() >= 2;
        }

        private bool EmvaHandEffect()
        {
            if (Card == null || Card.Location != CardLocation.Hand) return false;
            if (_emvaHandUsed) return false;
            return Bot.GetRemainingCount(CardId.RyuGeRealmSeaSpires, 1) > 0;
        }

        private bool KaivaHandSS()
        {
            if (Card == null || Card.Location != CardLocation.Hand) return false;
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool KaivaFieldEffect()
        {
            if (Card == null || Card.Location != CardLocation.MonsterZone) return false;
            if (_kaivaFieldUsed) return false;
            if (!Bot.HasInSpellZone(CardId.RyuGeRealmDinoDomains, faceUp: true)) return false;
            if (Enemy.GetFieldCount() == 0) return false;
            return true;
        }

        private bool KaivaHandEffect()
        {
            if (Card == null || Card.Location != CardLocation.Hand) return false;
            if (_kaivaHandUsed) return false;
            return Bot.GetRemainingCount(CardId.RyuGeRealmDinoDomains, 1) > 0;
        }

        private bool AnvaSummonOrEffect()
        {
            if (Card == null) return false;
            if (Card.Location == CardLocation.Hand)
            {
                if (_anvaMonsterUsed) return false;
                if (IsSpecialSummonBlocked()) return false;
                bool hasPreferredTribute = Bot.Hand.Any(c => c != null && c != Card && (c.Id == CardId.GenroRyuGeHakva || c.Id == CardId.KairoRyuGeEmva || c.Id == CardId.KyoroRyuGeKaiva || c.Id == CardId.TenseiRyuGeAnva))
                    || Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id != CardId.TenseiRyuGeAnva);
                bool hasMistvaTribute = Bot.Hand.Any(c => c != null && c.Id == CardId.SoseiRyuGeMistva);
                return hasPreferredTribute || hasMistvaTribute;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                // On-summon destroy effect: ONLY activate if enemy controls cards to destroy! NEVER destroy our own cards!
                return Enemy.GetFieldCount() > 0;
            }
            // Trigger effects when destroyed (from Extra/GY/Banished): place 1 Realm face-up on field
            return true;
        }

        private bool HasRyuGePendulumOnField()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && 
                (c.Id == CardId.SoseiRyuGeMistva || c.Id == CardId.TenseiRyuGeAnva));
        }

        private bool DinoDomainsEffect()
        {
            if (Card == null || Card.Location != CardLocation.Hand) return false;
            return !Bot.HasInSpellZone(CardId.RyuGeRealmDinoDomains, faceUp: true);
        }

        private bool SeaSpiresEffect()
        {
            if (Card == null || Card.Location != CardLocation.Hand) return false;
            return !Bot.HasInSpellZone(CardId.RyuGeRealmSeaSpires, faceUp: true);
        }

        private bool WyrmWindsEffect()
        {
            if (Card == null || Card.Location != CardLocation.Hand) return false;
            return !Bot.HasInSpellZone(CardId.RyuGeRealmWyrmWinds, faceUp: true);
        }

        private bool WarZoneSS()
        {
            if (Card == null || Card.Location != CardLocation.SpellZone) return false;
            if (IsSpecialSummonBlocked()) return false;
            // Activate 3-monster Special Summon if Sosei Ryu-Ge Mistva is face-up in Extra Deck
            return Bot.ExtraDeck.Any(c => c != null && c.Id == CardId.SoseiRyuGeMistva && c.IsFaceup());
        }

        private bool VarudrasSummon()
        {
            int lvl10Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 10 && !c.HasType(CardType.Xyz));
            if (lvl10Count < 2) return false;
            // Always prioritize summoning Varudras if not already on field!
            return !Bot.HasInMonstersZone(CardId.Varudras);
        }

        private bool GalaxyDestroyerSummon()
        {
            int lvl10Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 10);
            if (lvl10Count < 3) return false;
            if (Enemy.GetSpellCount() < 2) return false;
            // Only use 3 materials if we really need the board wipe
            if (IsBoardStrongEnough() && !OpponentHasThreateningMonster()) return false;
            return true;
        }

        private bool ZeusSummon()
        {
            // Only summon Zeus in Main Phase 2 after a battle
            if (Duel.Phase != DuelPhase.Main2) return false;
            // Only overlay on an Xyz monster if it has no materials left, or if the opponent has a dangerous board
            var xyz = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.HasType(CardType.Xyz) && c.Attacked);
            if (xyz == null) return false;
            if (xyz.Id == CardId.Varudras && xyz.Overlays.Count > 0 && Enemy.GetFieldCount() < 2)
                return false;
            return true;
        }

        private bool TyPhonSummon()
        {
            // Do not summon Ty-Phon if we control a face-up Ace monster (like Varudras, Mistva, Anva)
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsAceCard(c)))
                return false;
            return Util.GetProblematicEnemyMonster(0, true) != null;
        }

        private bool SPLittleKnightSummon()
        {
            // Do not link away our Ace monsters (Varudras, Mistva, Anva) if they are active on the field
            int nonAceCount = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            if (nonAceCount < 2 && Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsAceCard(c)))
                return false;

            // Only summon if there is a problematic enemy card to banish
            return Util.GetProblematicEnemyCard(0, true) != null;
        }

        // --- COMBAT & MOVEMENT CONTROL ---

        private bool IsSafeToAttack(ClientCard attacker)
        {
            foreach (ClientCard enemy in Enemy.MonsterZone)
            {
                if (enemy == null || !enemy.IsFaceup()) continue;
                if (!enemy.IsDisabled())
                {
                    if (enemy.Id == 21887175 && attacker.IsSpecialSummoned) return false;
                    if (enemy.Id == 50954680 && attacker.Level >= 5) return false;
                }
                if (enemy.IsAttack())
                {
                    if (enemy.Attack > attacker.Attack) return false;
                    if (enemy.Attack == attacker.Attack && Bot.GetMonsterCount() < Enemy.GetMonsterCount()) return false;
                }
                if (enemy.IsDefense() && attacker.Attack <= enemy.Defense) return false;
            }
            return true;
        }

        private bool IsSafeToDefend(ClientCard monster)
        {
            foreach (ClientCard enemy in Enemy.MonsterZone)
            {
                if (enemy == null || !enemy.IsFaceup()) continue;
                if (enemy.Attack > monster.Defense) return false;
            }
            return true;
        }

        private bool MonsterRepos()
        {
            if (Card == null) return false;
            bool enemyEmpty = Enemy.GetMonsterCount() == 0;
            if (Card.IsAttack())
            {
                if (!enemyEmpty && !IsSafeToAttack(Card) && IsSafeToDefend(Card)) return true;
            }
            else
            {
                if (enemyEmpty || IsSafeToAttack(Card)) return true;
            }
            return false;
        }

        private bool RyuGeSpellSet()
        {
            if (Card == null) return false;
            Logger.WriteTraceLine($"[RyuGeSpellSet Debug] Card: {Card.Name} ({Card.Id}) | Type: 0x{Card.Type:X} | IsContinuous: {Card.HasType(CardType.Continuous)} | IsTrap: {Card.IsTrap()} | IsQuickPlay: {Card.HasType(CardType.QuickPlay)}");
            // Never set Continuous Spells (like Ryu-Ge Rising or the Realms) from hand.
            // They should be activated directly.
            if (Card.HasType(CardType.Continuous)) return false;
            // Do not set Super Polymerization if we don't have enough cards in hand to pay the cost.
            if (Card.Id == CardId.SuperPolymerization && Bot.Hand.Count < 2) return false;
            
            return (Card.IsTrap() || Card.HasType(CardType.QuickPlay)) 
                && Bot.GetSpellCountWithoutField() < 4;
        }

        // --- SELECT TARGETS & STRATEGIC PRIORITIES ---

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // ===== HINT-BASED SELECTIONS (Ace Protection) =====

            // hint 533 = HINTMSG_LMATERIAL โ€” Link Material: protect Ace Cards
            if (hint == 533)
            {
                var safe = cards.Where(c => c != null && !IsAceCard(c)).ToList();
                if (safe.Count >= min) return safe.Take(max).ToList();
            }

            // hint 502 = HINTMSG_DESTROY โ€” destroy target selection
            if (hint == 502)
            {
                // Prefer opponent's cards first
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyCards.Count > 0)
                {
                    var sorted = enemyCards.OrderByDescending(c => {
                        if (c.IsMonster()) return c.RealPower;
                        return 1000;
                    }).ToList();
                    return sorted.Take(Math.Min(max, sorted.Count)).ToList();
                }
                // Our cards: protect Ace, destroy expendable first
                var nonAce = cards.Where(c => c != null && !IsAceCard(c))
                    .OrderBy(c => {
                        if (c.Id == CardId.TenseiRyuGeAnva) return 1;
                        if (c.Id == CardId.RyuGeRising) return 2;
                        if (c.Id == CardId.SoseiRyuGeMistva) return 3;
                        if (c.IsSpell() && !c.HasType(CardType.Continuous) && !c.HasType(CardType.Field)) return 4;
                        if (c.Id == CardId.RyuGeWarZone) return 5;
                        return 100;
                    }).ToList();
                if (nonAce.Count >= min) return nonAce.Take(max).ToList();
            }

            // hint 509 = HINTMSG_SPSUMMON — Special Summon target
            if (hint == 509)
            {
                // War Zone / Multi-zone SS
                var banishedHakva = cards.FirstOrDefault(c => c.Location == CardLocation.Removed && c.Id == CardId.GenroRyuGeHakva);
                if (banishedHakva != null && cards.All(c => c.Location == CardLocation.Removed))
                    return new List<ClientCard> { banishedHakva };

                var gyEmva = cards.FirstOrDefault(c => c.Location == CardLocation.Grave && c.Id == CardId.KairoRyuGeEmva);
                if (gyEmva != null && cards.All(c => c.Location == CardLocation.Grave))
                    return new List<ClientCard> { gyEmva };

                var deckKaiva = cards.FirstOrDefault(c => c.Location == CardLocation.Deck && c.Id == CardId.KyoroRyuGeKaiva);
                if (deckKaiva != null && cards.All(c => c.Location == CardLocation.Deck))
                    return new List<ClientCard> { deckKaiva };

                var aceMonsters = cards.Where(c => c != null && IsAceCard(c) && c.IsCanRevive()).ToList();
                if (aceMonsters.Count >= min) return aceMonsters.Take(max).ToList();

                var ryuGeLvl10 = cards.Where(c => c.Level == 10 && 
                    (c.Id == CardId.GenroRyuGeHakva || c.Id == CardId.KairoRyuGeEmva || 
                     c.Id == CardId.KyoroRyuGeKaiva || c.Id == CardId.TenseiRyuGeAnva || 
                     c.Id == CardId.SoseiRyuGeMistva)).ToList();
                if (ryuGeLvl10.Count >= min)
                {
                    var sorted = ryuGeLvl10.OrderBy(c => GetMaterialPriority(c)).ToList();
                    return sorted.Take(Math.Min(max, sorted.Count)).ToList();
                }

                bool hasDeck = cards.Any(c => c.Location == CardLocation.Deck);
                if (hasDeck && cards.Any(c => c.Location == CardLocation.Hand))
                {
                    var deckTargets = cards.Where(c => c.Location == CardLocation.Deck).ToList();
                    if (deckTargets.Count >= min) return deckTargets.Take(max).ToList();
                }
            }

            // hint 530 = HINTMSG_REMOVE (Banish)
            if (hint == 530)
            {
                // From War Zone: prefer banishing Hakva (so Hakva can SS from banishment)
                var hakva = cards.FirstOrDefault(c => c.Id == CardId.GenroRyuGeHakva);
                if (hakva != null) return new List<ClientCard> { hakva };
                var sorted = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(Math.Min(max, sorted.Count)).ToList();
            }

            // hint 549 = HINTMSG_ATTACKTARGET — Battle Phase attack target
            if (hint == 549)
            {
                int ourBestAtk = Bot.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && c.IsAttack())
                    .Select(c => c.Attack)
                    .DefaultIfEmpty(0).Max();
                var beatable = cards.Where(c => c != null && c.IsFaceup() && c.Location == CardLocation.MonsterZone
                    && (c.IsAttack() ? c.Attack < ourBestAtk : c.Defense < ourBestAtk)).ToList();
                if (beatable.Count >= min) return beatable.OrderByDescending(c => c.Attack).Take(max).ToList();
                var validTargets = cards.Where(c => c != null && c.IsFaceup() && c.Location == CardLocation.MonsterZone).ToList();
                if (validTargets.Count >= min) return validTargets.Take(max).ToList();
            }

            // hint 500 = HINTMSG_RELEASE (Tribute)
            if (hint == 500)
            {
                // Prefer Emva (revives from GY) > Hakva > Kaiva > duplicate Anva > Mistva
                var emva = cards.FirstOrDefault(c => c.Id == CardId.KairoRyuGeEmva);
                if (emva != null) return new List<ClientCard> { emva };
                var hakva = cards.FirstOrDefault(c => c.Id == CardId.GenroRyuGeHakva);
                if (hakva != null) return new List<ClientCard> { hakva };
                var kaiva = cards.FirstOrDefault(c => c.Id == CardId.KyoroRyuGeKaiva);
                if (kaiva != null) return new List<ClientCard> { kaiva };
                var dupAnva = cards.FirstOrDefault(c => c.Id == CardId.TenseiRyuGeAnva && c != Card);
                if (dupAnva != null) return new List<ClientCard> { dupAnva };
                var mistva = cards.FirstOrDefault(c => c.Id == CardId.SoseiRyuGeMistva);
                if (mistva != null) return new List<ClientCard> { mistva };
                var otherNonAce = cards.FirstOrDefault(c => !IsAceCard(c));
                if (otherNonAce != null) return new List<ClientCard> { otherNonAce };
                return cards.Take(max).ToList();
            }

            var activeCard = Card ?? LastChainCard;

            // Ryu-Ge Rising placement selection: Send Mistva to face-up Extra Deck if available. NEVER send Anva!
            if (activeCard != null && activeCard.Id == CardId.RyuGeRising && cards.All(c => c.Location == CardLocation.Hand))
            {
                var mistva = cards.FirstOrDefault(c => c.Id == CardId.SoseiRyuGeMistva);
                if (mistva != null) return new List<ClientCard> { mistva };
                
                if (cancelable) return new List<ClientCard>();

                var safe = cards.FirstOrDefault(c => c.Id != CardId.TenseiRyuGeAnva);
                if (safe != null) return new List<ClientCard> { safe };
                
                return cards.Take(max).ToList();
            }

            if (hint == 533 || hint == 513 || hint == 512)
            {
                var sorted = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(Math.Min(max, sorted.Count)).ToList();
            }

            if (hint == 511) // Fusion Material (Super Poly)
            {
                var sorted = cards.OrderByDescending(c => c.Controller == 1 ? 1 : 0).ToList();
                return sorted.Take(Math.Min(max, sorted.Count)).ToList();
            }

            if (hint == 501) // Discard
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    
                    bool isDuplicate = Bot.Hand.Count(x => x.Id == c.Id) > 1;
                    
                    if (isDuplicate)
                    {
                        if (c.Id == CardId.KairoRyuGeEmva) return 2;
                        if (c.Id == CardId.GenroRyuGeHakva) return 4;
                        if (c.Id == CardId.KyoroRyuGeKaiva) return 5;
                        if (c.IsSpell() || c.IsTrap()) return 8;
                        if (c.Id == CardId.SoseiRyuGeMistva) return 300;
                        if (c.Id == CardId.TenseiRyuGeAnva) return 301;
                        return 9;
                    }
                    
                    // Single copies: prefer Emva since it revives from GY
                    if (c.Id == CardId.KairoRyuGeEmva) return 1;
                    if (c.Id == CardId.KyoroRyuGeKaiva)
                    {
                        bool hasDino = Bot.HasInHand(CardId.RyuGeRealmDinoDomains) || Bot.HasInSpellZone(CardId.RyuGeRealmDinoDomains);
                        return hasDino ? 10 : 25;
                    }
                    if (c.Id == CardId.GenroRyuGeHakva)
                    {
                        bool hasWyrm = Bot.HasInHand(CardId.RyuGeRealmWyrmWinds) || Bot.HasInSpellZone(CardId.RyuGeRealmWyrmWinds);
                        return hasWyrm ? 11 : 26;
                    }
                    if (c.Id == CardId.SoseiRyuGeMistva) return 300; // NEVER discard Mistva!
                    if (c.Id == CardId.TenseiRyuGeAnva) return 301; // NEVER discard Anva!
                    
                    if (c.Id == CardId.DrollAndLockBird) return 30;
                    if (c.Id == CardId.Nibiru) return 31;
                    
                    if (c.Id == CardId.AshBlossom) return 100;
                    if (c.Id == CardId.MaxxC) return 101;
                    if (c.Id == CardId.DivinerOfTheHerald) return 102;
                    
                    if (c.Id == CardId.CalledByTheGrave) return 200;
                    if (c.Id == CardId.CrossoutDesignator) return 201;
                    if (c.Id == CardId.InfiniteImpermanence) return 202;
                    if (c.Id == CardId.SuperPolymerization) return 250;
                    
                    return 50;
                }).ToList();
                return sorted.Take(Math.Min(max, sorted.Count)).ToList();
            }

            if (hint == 506) // Search / Add to hand
            {
                // Melody of Awakening Dragon or multi-search: add 1 Anva + 1 Mistva
                if (max >= 2)
                {
                    var res = new List<ClientCard>();
                    var anva = cards.FirstOrDefault(c => c.Id == CardId.TenseiRyuGeAnva);
                    if (anva != null) res.Add(anva);
                    var mistva = cards.FirstOrDefault(c => c.Id == CardId.SoseiRyuGeMistva);
                    if (mistva != null) res.Add(mistva);
                    if (res.Count < max)
                    {
                        foreach (var c in cards)
                        {
                            if (!res.Contains(c))
                            {
                                res.Add(c);
                                if (res.Count >= max) break;
                            }
                        }
                    }
                    if (res.Count >= min) return res;
                }

                // If from Mistva PZone: add War Zone first!
                if (activeCard != null && activeCard.Id == CardId.SoseiRyuGeMistva)
                {
                    var warZone = cards.FirstOrDefault(c => c.Id == CardId.RyuGeWarZone);
                    if (warZone != null && !Bot.HasInHandOrInSpellZone(CardId.RyuGeWarZone))
                        return new List<ClientCard> { warZone };
                    var rising = cards.FirstOrDefault(c => c.Id == CardId.RyuGeRising);
                    if (rising != null && !Bot.HasInHand(CardId.RyuGeRising))
                        return new List<ClientCard> { rising };
                    var kaiva = cards.FirstOrDefault(c => c.Id == CardId.KyoroRyuGeKaiva);
                    if (kaiva != null) return new List<ClientCard> { kaiva };
                }

                if (cards.Any(c => c.Id == CardId.RyuGeWarZone) && !Bot.HasInHandOrInSpellZone(CardId.RyuGeWarZone))
                {
                    return cards.Where(c => c.Id == CardId.RyuGeWarZone).Take(max).ToList();
                }
                if (cards.Any(c => c.Id == CardId.RyuGeRising) && !Bot.HasInHand(CardId.RyuGeRising))
                {
                    return cards.Where(c => c.Id == CardId.RyuGeRising).Take(max).ToList();
                }
                if (cards.Any(c => c.Id == CardId.TenseiRyuGeAnva) && !Bot.HasInHand(CardId.TenseiRyuGeAnva) && !Bot.HasInMonstersZone(CardId.TenseiRyuGeAnva))
                {
                    return cards.Where(c => c.Id == CardId.TenseiRyuGeAnva).Take(max).ToList();
                }
                bool hasMistvaInEDSearch = Bot.ExtraDeck.Any(c => c != null && c.Id == CardId.SoseiRyuGeMistva && c.IsFaceup());
                if (cards.Any(c => c.Id == CardId.SoseiRyuGeMistva) && !Bot.HasInHand(CardId.SoseiRyuGeMistva) && !hasMistvaInEDSearch)
                {
                    return cards.Where(c => c.Id == CardId.SoseiRyuGeMistva).Take(max).ToList();
                }
                if (cards.Any(c => c.Id == CardId.RyuGeRealmWyrmWinds) && !Bot.HasInHandOrInSpellZone(CardId.RyuGeRealmWyrmWinds))
                {
                    return cards.Where(c => c.Id == CardId.RyuGeRealmWyrmWinds).Take(max).ToList();
                }
                if (cards.Any(c => c.Id == CardId.RyuGeRealmSeaSpires) && !Bot.HasInHandOrInSpellZone(CardId.RyuGeRealmSeaSpires))
                {
                    return cards.Where(c => c.Id == CardId.RyuGeRealmSeaSpires).Take(max).ToList();
                }
                if (cards.Any(c => c.Id == CardId.RyuGeRealmDinoDomains) && !Bot.HasInHandOrInSpellZone(CardId.RyuGeRealmDinoDomains))
                {
                    return cards.Where(c => c.Id == CardId.RyuGeRealmDinoDomains).Take(max).ToList();
                }
                var nonPendulum = cards.Where(c => c.Id == CardId.KyoroRyuGeKaiva || c.Id == CardId.GenroRyuGeHakva || c.Id == CardId.KairoRyuGeEmva).ToList();
                if (nonPendulum.Count > 0)
                {
                    var kaiva = nonPendulum.FirstOrDefault(c => c.Id == CardId.KyoroRyuGeKaiva);
                    if (kaiva != null) return new List<ClientCard> { kaiva };
                    var hakva = nonPendulum.FirstOrDefault(c => c.Id == CardId.GenroRyuGeHakva);
                    if (hakva != null) return new List<ClientCard> { hakva };
                    return new List<ClientCard> { nonPendulum[0] };
                }
            }

            if (hint == 504) // Send to GY
            {
                if (cards.Any(c => c.Id == CardId.KairoRyuGeEmva))
                {
                    return cards.Where(c => c.Id == CardId.KairoRyuGeEmva).Take(max).ToList();
                }
                if (cards.Any(c => c.Id == CardId.HeraldOfTheArcLight))
                {
                    return cards.Where(c => c.Id == CardId.HeraldOfTheArcLight).Take(max).ToList();
                }
                if (cards.Any(c => c.Id == CardId.Garura))
                {
                    return cards.Where(c => c.Id == CardId.Garura).Take(max).ToList();
                }
                if (cards.Any(c => c.Id == CardId.ElderEntityNtss))
                {
                    return cards.Where(c => c.Id == CardId.ElderEntityNtss).Take(max).ToList();
                }
                var sorted = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(Math.Min(max, sorted.Count)).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        // ===== Fusion Material: protect Ace =====
        public override IList<ClientCard> OnSelectFusionMaterial(IList<ClientCard> cards, int min, int max)
        {
            // Super Polymerization: use opponent monsters first
            if (Card != null && Card.Id == CardId.SuperPolymerization)
            {
                var oppMonsters = cards.Where(c => c != null
                    && c.Controller == 1 && c.Location == CardLocation.MonsterZone).ToList();
                if (oppMonsters.Count >= min) return oppMonsters.Take(max).ToList();
            }

            // Regular: hand > field expendable > field non-Ace
            var result = new List<ClientCard>();
            var handMats = cards.Where(c => c != null && c.Location == CardLocation.Hand).ToList();
            var safeField = cards.Where(c => c != null
                && c.Location == CardLocation.MonsterZone && !IsAceCard(c)).ToList();
            foreach (var c in handMats) { result.Add(c); if (result.Count >= max) break; }
            if (result.Count < max)
                foreach (var c in safeField) { result.Add(c); if (result.Count >= max) break; }
            if (result.Count >= min) return result;

            return base.OnSelectFusionMaterial(cards, min, max);
        }

        public override int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
            if (player == 0 && location == CardLocation.SpellZone)
            {
                // If it is not a Pendulum monster being placed in the Pendulum Zone,
                // try to avoid the leftmost and rightmost zones (z0 and z4) to keep them clear for Pendulum scales.
                if (cardId != CardId.SoseiRyuGeMistva && cardId != CardId.TenseiRyuGeAnva)
                {
                    int middleZones = available & (Zones.z1 | Zones.z2 | Zones.z3);
                    if (middleZones != 0)
                    {
                        // Prefer z2 (middle), then z1, then z3
                        if ((middleZones & Zones.z2) != 0) return Zones.z2;
                        if ((middleZones & Zones.z1) != 0) return Zones.z1;
                        if ((middleZones & Zones.z3) != 0) return Zones.z3;
                    }
                }
            }
            return base.OnSelectPlace(cardId, player, location, available);
        }
    }

    [Deck("Expert_2026_RyuGe", "2026_RyuGe")]
    public class ExpertRyuGeExecutor : _2026_RyuGeExecutor
    {
        private string _duelId;
        public ExpertRyuGeExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            _duelId = $"duel_{Guid.NewGuid():N}";
            // [REMOVED-AI-TRAINING] ExpertDataLogger.EnsureInitialized(ExpertDataLogger.FindProjectRoot(), "2026_RyuGe");
        }
        public override MainPhaseAction OnSelectIdleCmd(MainPhase main)
        {
            var action = base.OnSelectIdleCmd(main);
            // [REMOVED-AI-TRAINING] ExpertDataLogger.LogMainPhaseDecision(main, action, Bot, Enemy, _duelId, Duel.Turn);
            return action;
        }
        public override BattlePhaseAction OnBattle(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            var action = base.OnBattle(attackers, defenders);
            // [REMOVED-AI-TRAINING] ExpertDataLogger.LogBattleDecision(attackers, defenders, action, Bot, Enemy, _duelId, Duel.Turn);
            return action;
        }
    }
}
