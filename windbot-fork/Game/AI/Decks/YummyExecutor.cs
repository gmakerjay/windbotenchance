// =========================================================================================
// CARD AUDIT — 2026_Yummy (Championship Tier-1 Metagame Engine)
// 100% verified against cards.cdb and 2026_Yummy.ydk
// | Card Name                          | Type    | OPT? | Cost   | Effect Summary                                                   |
// | :--------------------------------- | :-----: | :--: | :----: | :--------------------------------------------------------------- |
// | Marshmao☆Yummy (10966439)          | Monster | HOPT | None   | Lv1 Non-Tuner. Free SS if LIGHT Beast. S/T from GY or Deck (if Synchro SS)|
// | Cupsy☆Yummy (31425736)             | Monster | HOPT | None   | Lv1 Non-Tuner. Free SS if Link-1/Synchro-2. Search Yummy card (Draw 1 if Synchro SS)|
// | Cooky☆Yummy (68810435)             | Monster | HOPT | None   | Lv1 Non-Tuner. Free SS if Link-1/Synchro-2. -1000 ATK (Destroy 1 if Synchro SS)|
// | Lollipo☆Yummy (4215180)            | Monster | HOPT | None   | Lv1 Non-Tuner. Free SS if Link-1/Synchro-2. Shuffle GY (Banish 1 if Synchro SS)|
// | Yummyusment☆Mignon (66975205)      | Field   | HOPT | None   | Field: +500 ATK per LIGHT Beast on field. Revive Lv1 Yummy if Link-1 on field|
// | Yummyusment★Acroquey (93360904)    | Field   | HOPT | None   | Pop 1 card when LIGHT Beast Synchro SS. SS Yummy from deck if card leaves field|
// | Yummy☆Surprise (29369059)          | Trap    | HOPT | None   | 1) Bounce 2 LIGHT Beasts + 2 opp cards; 2) SS Yummy from GY; 3) Recycle Field|
// | Yummy★Snatchy (30581601)           | Link-1  | HOPT | 100 LP | Link-1 (1 LIGHT Beast). Places Mignon/Acroquey. Quick Synchro in MP/opp BP|
// | Cupsy★Yummy Way (31603289)         | Synchro | HOPT | Discard| Lv2 Synchro (treats Link-1 as Lv1 Tuner). Search 2 Yummies. Tag-out to SS 2|
// | Cooky★Yummy Way (67098897)         | Synchro | HOPT | None   | Lv2 Synchro (treats Link-1 as Lv1 Tuner). Book of Moon x2. Tag-out to SS 2|
// | Lollipo★Yummy Way (93192592)       | Synchro | HOPT | None   | Lv2 Synchro. Revive 2 Yummies. Tag-out to SS 2               |
// | Spright Elf (27381364)             | Link-2  | HOPT | None   | Target protect linked monsters. Quick revive Level/Rank/Link 2   |
// | S:P Little Knight (29301450)       | Link-2  | HOPT | None   | Banish 1 on summon; Quick banish 2 monsters until End Phase       |
// | Herald of the Arc Light (79606837) | Synchro | No   | Tribute| Level 4 Boss (Omni-negate tribute)                               |
// | Lyrilusc Nightingale (48608796)    | Xyz Rk1 | None | None   | Direct attack, detach for team destruction/damage immunity       |
// | Divine Arsenal AA-ZEUS (90448279)  | Xyz Rk12| Quick| Detach2| Quick field wipe: sends all other cards to GY                     |
// =========================================================================================

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
    [Deck("2026_Yummy", "2026_Yummy")]
    public class _2026_YummyExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Deck — Yummy Archetype
            public const int MarshmaoYummy = 10966439;
            public const int CupsyYummy = 31425736;
            public const int CookyYummy = 68810435;
            public const int LollipoYummy = 4215180;
            public const int YummyusmentMignon = 66975205;
            public const int YummyusmentAcroquey = 93360904;
            public const int YummySurprise = 29369059;

            // Main Deck — Staples & Hand Traps
            public const int NibiruThePrimalBeing = 27204311;
            public const int MulcharmyFuwalos = 42141493;
            public const int MulcharmyPurulia = 84192580;
            public const int MaxxC = 23434538;
            public const int AshBlossomAndJoyousSpring = 14558127;
            public const int GhostBelleAndHauntedMansion = 73642296;
            public const int DrollAndLockBird = 94145021;
            public const int EffectVeiler = 97268402;
            public const int GhostOgreAndSnowRabbit = 59438930;
            public const int DominusPurge = 97045737;
            public const int InfiniteImpermanence = 10045474;

            // Extenders & Generic Spells
            public const int Sangan = 26202165;
            public const int MagiciansSouls = 97631303;
            public const int IllusionOfChaos = 12266229;
            public const int JesterConfit = 8487449;
            public const int PiriReisMap = 33907039;
            public const int SkyStrikerMechaHornetDrones = 52340444;
            public const int CalledByTheGrave = 24224830;
            public const int CrossoutDesignator = 65681983;
            public const int TripleTacticsTalent = 25311006;
            public const int ForbiddenDroplet = 24299458;
            public const int HarpiesFeatherDuster = 18144507;
            public const int LightningStorm = 14532163;
            public const int CosmicCyclone = 8267140;

            // Extra Deck
            public const int SprightElf = 27381364;
            public const int SPLittleKnight = 29301450;
            public const int HeraldOfTheArcLight = 79606837;
            public const int CupsyYummyWay = 31603289;
            public const int CookyYummyWay = 67098897;
            public const int LollipoYummyWay = 93192592;
            public const int YummySnatchy = 30581601;
            public const int RelinquishedAnima = 29479265;
            public const int Linkuriboh = 41999284;
            public const int LinkSpider = 98978921;
            public const int SalamangreatAlmiraj = 60303245;
            public const int SkyStrikerAceKagari = 63288573;
            public const int LyriluscAssembledNightingale = 48608796;
            public const int DivineArsenalAAZEUSSkyThunder = 90448279;

            // Side Techs
            public const int SantaClaws = 46565218;
            public const int BookOfEclipse = 35480699;
            public const int MistakenArrest = 4227096;
            public const int DimensionalBarrier = 83326048;
            public const int EvenlyMatched = 15693423;
        }

        public static readonly int[] BossMonsters = {
            CardId.SprightElf,
            CardId.SPLittleKnight,
            CardId.CupsyYummyWay,
            CardId.CookyYummyWay,
            CardId.LollipoYummyWay,
            CardId.HeraldOfTheArcLight,
            CardId.DivineArsenalAAZEUSSkyThunder
        };

        public static readonly int[] YummyMonsters = {
            CardId.MarshmaoYummy,
            CardId.CupsyYummy,
            CardId.CookyYummy,
            CardId.LollipoYummy
        };

        public static readonly int[] YummySynchros = {
            CardId.CupsyYummyWay,
            CardId.CookyYummyWay,
            CardId.LollipoYummyWay
        };

        // Turn tracking flags
        public bool NormalSummonUsed { get; private set; }
        public bool MarshmaoHandSSUsed { get; private set; }
        public bool MarshmaoEffectUsed { get; private set; }
        public bool CupsHandSSUsed { get; private set; }
        public bool CupsySearchUsed { get; private set; }
        public bool CookyHandSSUsed { get; private set; }
        public bool CookyEffectUsed { get; private set; }
        public bool LollipoHandSSUsed { get; private set; }
        public bool LollipoEffectUsed { get; private set; }
        public bool MignonFieldReviveUsed { get; private set; }
        public bool SnatchyPlaceUsed { get; private set; }
        public bool CupsyWaySearchUsed { get; private set; }
        public bool SprightElfReviveUsed { get; private set; }
        public bool SurpriseUsed { get; private set; }
        public int SurpriseOurBounceCount { get; set; }
        public int SurpriseOppBounceCount { get; set; }
        public int HandTrapsUsedThisTurn { get; private set; }

        public override bool IsAceCard(ClientCard card)
        {
            return card != null && BossMonsters.Contains(card.Id);
        }

        protected override bool IsBoardStrongEnough()
        {
            bool hasElf = Bot.HasInMonstersZone(CardId.SprightElf);
            bool hasSynchro = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && YummySynchros.Contains(c.Id));
            bool hasTrap = Bot.HasInSpellZone(CardId.YummySurprise);
            bool hasHerald = Bot.HasInMonstersZone(CardId.HeraldOfTheArcLight);
            bool hasSP = Bot.HasInMonstersZone(CardId.SPLittleKnight);

            if (hasElf && hasSynchro && hasTrap) return true;
            if (hasElf && hasHerald) return true;
            if (hasElf && hasSynchro && hasSP) return true;
            if (CountDisruptions() >= 3) return true;

            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (Duel.Turn == 1 || (Duel.Player == 0 && Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0))
            {
                bool hasElf = Bot.HasInMonstersZone(CardId.SprightElf);
                bool hasSynchro = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && YummySynchros.Contains(c.Id));
                bool hasHerald = Bot.HasInMonstersZone(CardId.HeraldOfTheArcLight);

                if (hasElf && hasSynchro && hasHerald) return true;
            }

            if (CanDealLethal()) return true;
            return false;
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            return DeckPlugin?.MaterialEvaluator?.GetMaterialCost(c) ?? 50;
        }

        public override IList<ClientCard> OnSelectLinkMaterial(IList<ClientCard> cards, int min, int max)
        {
            // Selecting materials for Spright Elf: requires 1 Level/Rank/Link 2 monster!
            var lv2Monster = cards.FirstOrDefault(c => c != null && (c.Level == 2 || c.Rank == 2 || (c.HasType(CardType.Link) && c.LinkCount == 2)));
            if (lv2Monster != null && min == 2)
            {
                var otherCards = cards.Where(c => c != null && c != lv2Monster)
                                      .OrderBy(GetMaterialPriority)
                                      .Take(1)
                                      .ToList();
                if (otherCards.Count > 0)
                {
                    return new[] { lv2Monster, otherCards[0] };
                }
            }

            var protectedCards = cards.Where(c => c != null && !(c.Location == CardLocation.MonsterZone && IsAceCard(c))).ToList();
            if (protectedCards.Count >= min)
            {
                return protectedCards.OrderBy(GetMaterialPriority).Take(max).ToList();
            }
            return cards.OrderBy(GetMaterialPriority).Take(max).ToList();
        }

        public override IList<ClientCard> OnSelectSynchroMaterial(IList<ClientCard> cards, int sum, int min, int max)
        {
            var sorted = cards.Where(c => c != null).OrderBy(GetMaterialPriority).ToList();
            var safe = sorted.Where(c => !c.IsCode(CardId.HeraldOfTheArcLight, CardId.SprightElf)).ToList();
            if (safe.Count >= min)
                return Util.CheckSelectCount(safe, cards, min, max);

            return base.OnSelectSynchroMaterial(cards, sum, min, max);
        }

        public _2026_YummyExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // 🔒 Decoupled Domain Plugin Architecture
            DeckPlugin = new YummyPlugin(this);

            HeuristicGuard.RegisterAceCards(BossMonsters);
            ResourcePlan.RegisterAceCards(BossMonsters);

            // ── Combo Router Routes ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Yummy-Snatchy-CupsyWay-Elf-Line",
                RequiredCards = new List<int> { CardId.MarshmaoYummy },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.MarshmaoYummy, ActionType = ExecutorType.Summon, Description = "Summon Marshmao" },
                    new() { CardId = CardId.YummySnatchy, ActionType = ExecutorType.SpSummon, Description = "Link Yummy Snatchy" },
                    new() { CardId = CardId.YummySnatchy, ActionType = ExecutorType.Activate, Description = "Place Mignon" },
                    new() { CardId = CardId.YummyusmentMignon, ActionType = ExecutorType.Activate, Description = "Revive Marshmao" },
                    new() { CardId = CardId.CupsyYummyWay, ActionType = ExecutorType.SpSummon, Description = "Synchro Cupsy Way" },
                    new() { CardId = CardId.CupsyYummyWay, ActionType = ExecutorType.Activate, Description = "Search 2 Yummies" },
                    new() { CardId = CardId.SprightElf, ActionType = ExecutorType.SpSummon, Description = "Link Spright Elf" }
                },
                EndBoardScore = 95
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Cupsy-Snatchy-CupsyWay-Elf-Line",
                RequiredCards = new List<int> { CardId.CupsyYummy },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.CupsyYummy, ActionType = ExecutorType.Summon, Description = "Summon Cupsy" },
                    new() { CardId = CardId.YummySnatchy, ActionType = ExecutorType.SpSummon, Description = "Link Yummy Snatchy" },
                    new() { CardId = CardId.YummySnatchy, ActionType = ExecutorType.Activate, Description = "Place Mignon" },
                    new() { CardId = CardId.YummyusmentMignon, ActionType = ExecutorType.Activate, Description = "Revive Cupsy" },
                    new() { CardId = CardId.CupsyYummyWay, ActionType = ExecutorType.SpSummon, Description = "Synchro Cupsy Way" },
                    new() { CardId = CardId.CupsyYummyWay, ActionType = ExecutorType.Activate, Description = "Search 2 Yummies" },
                    new() { CardId = CardId.SprightElf, ActionType = ExecutorType.SpSummon, Description = "Link Spright Elf" }
                },
                EndBoardScore = 95
            });

            BaitPlanner.RegisterComboStarters(CardId.MarshmaoYummy, CardId.CupsyYummy, CardId.CookyYummy, CardId.LollipoYummy, CardId.PiriReisMap);
            ChainAdvisor.RegisterHighValueTargets(CardId.SprightElf, CardId.HeraldOfTheArcLight, CardId.CupsyYummyWay, CardId.SPLittleKnight);

            // ============================================================
            // TIER 1: Hand Traps & Reactive Disruptions (Both Turns)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCCondition);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAndJoyousSpring, AshCondition);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelleAndHauntedMansion, GhostBelleCondition);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, EffectVeilerCondition);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, ImpermanenceCondition);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollCondition);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyCondition);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyPurulia, MulcharmyCondition);
            AddExecutor(ExecutorType.Activate, CardId.GhostOgreAndSnowRabbit, DefaultGhostOgreAndSnowRabbit);
            AddExecutor(ExecutorType.Activate, CardId.NibiruThePrimalBeing, NibiruCondition);
            AddExecutor(ExecutorType.Activate, CardId.DominusPurge, DominusPurgeCondition);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.CrossoutDesignator, CrossoutDesignatorEffect);

            // ============================================================
            // TIER 2: Board Breakers (Going 2nd Priority)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, HarpiesFeatherDusterEffect);
            AddExecutor(ExecutorType.Activate, CardId.LightningStorm, LightningStormEffect);
            AddExecutor(ExecutorType.Activate, CardId.CosmicCyclone, CosmicCycloneEffect);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentEffect);
            AddExecutor(ExecutorType.Activate, CardId.DivineArsenalAAZEUSSkyThunder, ZeusEffect);

            // ============================================================
            // TIER 3: Boss Monster Quick Effects & Tag-Outs (Opponent Turn & Chains)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.HeraldOfTheArcLight, HeraldOfTheArcLightEffect);
            AddExecutor(ExecutorType.Activate, CardId.CupsyYummyWay, CupsyYummyWayEffect);
            AddExecutor(ExecutorType.Activate, CardId.CookyYummyWay, CookyYummyWayEffect);
            AddExecutor(ExecutorType.Activate, CardId.LollipoYummyWay, LollipoYummyWayEffect);
            AddExecutor(ExecutorType.Activate, CardId.SprightElf, SprightElfEffect);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightEffect);
            AddExecutor(ExecutorType.Activate, CardId.YummySnatchy, YummySnatchyEffect);
            AddExecutor(ExecutorType.Activate, CardId.YummySurprise, YummySurpriseEffect);
            AddExecutor(ExecutorType.Activate, CardId.LyriluscAssembledNightingale, NightingaleEffect);

            // ============================================================
            // TIER 4: Setup Spells & Field Spell Ignition
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.PiriReisMap, PiriReisMapEffect);
            AddExecutor(ExecutorType.Activate, CardId.YummyusmentMignon, YummyusmentMignonEffect);
            AddExecutor(ExecutorType.Activate, CardId.YummyusmentAcroquey, YummyusmentAcroqueyEffect);
            AddExecutor(ExecutorType.Activate, CardId.IllusionOfChaos, IllusionOfChaosEffect);
            AddExecutor(ExecutorType.Activate, CardId.SkyStrikerMechaHornetDrones, HornetDronesEffect);
            AddExecutor(ExecutorType.Activate, CardId.MagiciansSouls, MagiciansSoulsEffect);

            // ============================================================
            // TIER 5: Monster Effects (Field / Search / On-Summon Triggers)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.MarshmaoYummy, MarshmaoYummyEffect);
            AddExecutor(ExecutorType.Activate, CardId.CupsyYummy, CupsyYummyEffect);
            AddExecutor(ExecutorType.Activate, CardId.CookyYummy, CookyYummyEffect);
            AddExecutor(ExecutorType.Activate, CardId.LollipoYummy, LollipoYummyEffect);
            AddExecutor(ExecutorType.Activate, CardId.RelinquishedAnima, RelinquishedAnimaEffect);
            AddExecutor(ExecutorType.Activate, CardId.SkyStrikerAceKagari, KagariEffect);

            // ============================================================
            // TIER 6: Normal Summons (Starters)
            // ============================================================
            AddExecutor(ExecutorType.Summon, CardId.CupsyYummy, CupsyNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.MarshmaoYummy, MarshmaoNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.CookyYummy, YummyGenericNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.LollipoYummy, YummyGenericNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.Sangan, SanganNormalSummon);

            // ============================================================
            // TIER 7: Special Summons (Hand Extenders & Free Inherent SS)
            // ============================================================
            AddExecutor(ExecutorType.SpSummon, CardId.MarshmaoYummy, MarshmaoSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CupsyYummy, YummyHandSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CookyYummy, YummyHandSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.LollipoYummy, YummyHandSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.JesterConfit, JesterConfitSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.MagiciansSouls, MagiciansSoulsSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SantaClaws, SantaClawsSpSummon);

            // ============================================================
            // TIER 8: Extra Deck Summons (Snatchy -> Synchro Lv2 -> Spright Elf / S:P / Zeus)
            // ============================================================
            AddExecutor(ExecutorType.SpSummon, CardId.YummySnatchy, YummySnatchySpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CupsyYummyWay, CupsyYummyWaySpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CookyYummyWay, CookyYummyWaySpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.LollipoYummyWay, LollipoYummyWaySpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SprightElf, SprightElfSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.HeraldOfTheArcLight, HeraldSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.LyriluscAssembledNightingale, NightingaleSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DivineArsenalAAZEUSSkyThunder, ZeusSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.RelinquishedAnima, RelinquishedAnimaSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SalamangreatAlmiraj, AlmirajSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SkyStrikerAceKagari, KagariSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.LinkSpider, LinkSpiderSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Linkuriboh, LinkuribohSpSummon);

            // ============================================================
            // TIER 9: Setting Traps & Cleanup
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.BookOfEclipse, BookOfEclipseEffect);
            AddExecutor(ExecutorType.Activate, CardId.MistakenArrest, MistakenArrestEffect);
            AddExecutor(ExecutorType.Activate, CardId.DimensionalBarrier, DimensionalBarrierEffect);
            AddExecutor(ExecutorType.Activate, CardId.EvenlyMatched, EvenlyMatchedEffect);

            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, ImpermanenceSetCondition);
            AddExecutor(ExecutorType.SpellSet, CardId.DominusPurge);
            AddExecutor(ExecutorType.SpellSet, CardId.YummySurprise, YummySurpriseSetCondition);
            AddExecutor(ExecutorType.SpellSet, CardId.DimensionalBarrier);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave);
            AddExecutor(ExecutorType.SpellSet, CardId.CrossoutDesignator);

            AddExecutor(ExecutorType.Repos, YummyMonsterRepos);
        }

        public override bool OnSelectHand() => true;

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            NormalSummonUsed = false;
            MarshmaoHandSSUsed = false;
            MarshmaoEffectUsed = false;
            CupsHandSSUsed = false;
            CupsySearchUsed = false;
            CookyHandSSUsed = false;
            CookyEffectUsed = false;
            LollipoHandSSUsed = false;
            LollipoEffectUsed = false;
            MignonFieldReviveUsed = false;
            SnatchyPlaceUsed = false;
            CupsyWaySearchUsed = false;
            SprightElfReviveUsed = false;
            SurpriseUsed = false;
            SurpriseOurBounceCount = 0;
            SurpriseOppBounceCount = 0;
            HandTrapsUsedThisTurn = 0;
        }

        // ============================================================
        // HELPER METHODS
        // ============================================================

        private bool HasLink1OnField()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Link) && c.LinkCount == 1);
        }

        private bool HasLevel2SynchroOnField()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Synchro) && c.Level == 2);
        }

        // ============================================================
        // HAND TRAPS & REACTIVE DISRUPTIONS
        // ============================================================

        private bool MaxxCCondition()
        {
            if (Duel.Player == 0) return false;
            if (HandTrapsUsedThisTurn >= 2) return false;
            if (DefaultMaxxC())
            {
                HandTrapsUsedThisTurn++;
                DecisionTracer.TraceActivate("MaxxC", "Opponent starting SS chain");
                return true;
            }
            return false;
        }

        private bool AshCondition()
        {
            if (Util.GetLastChainCard()?.Controller == 0) return false;
            if (HandTrapsUsedThisTurn >= 2) return false;

            var lastCard = Util.GetLastChainCard();
            if (lastCard == null || lastCard.Controller != 1) return false;

            int[] ignoreList = { 70368879, 30241314, 60600126 };
            if (lastCard.IsCode(ignoreList)) return false;
            if (lastCard.HasSetcode(0x11e) && lastCard.Location == CardLocation.Hand) return false;

            HandTrapsUsedThisTurn++;
            DecisionTracer.TraceActivate("AshBlossom", $"Negating search/SS from deck: {lastCard.Name}");
            return true;
        }

        private bool GhostBelleCondition()
        {
            if (Util.GetLastChainCard()?.Controller == 0) return false;
            if (HandTrapsUsedThisTurn >= 2) return false;

            var lastCard = Util.GetLastChainCard();
            if (lastCard == null || lastCard.Controller != 1) return false;

            HandTrapsUsedThisTurn++;
            DecisionTracer.TraceActivate("GhostBelle", $"Negating GY interaction: {lastCard.Name}");
            return true;
        }

        private bool EffectVeilerCondition()
        {
            if (Duel.Player == 0) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
            if (Util.GetLastChainCard()?.Controller == 0) return false;
            if (HandTrapsUsedThisTurn >= 2) return false;

            if (Duel.LastChainPlayer == 1)
            {
                var chainCard = Util.GetLastChainCard();
                if (chainCard != null && chainCard.Controller == 1 && chainCard.Location == CardLocation.MonsterZone && !chainCard.IsDisabled() && !chainCard.IsShouldNotBeTarget())
                {
                    HandTrapsUsedThisTurn++;
                    AI.SelectCard(chainCard);
                    DecisionTracer.TraceActivate("EffectVeiler", $"Chaining Effect Veiler negation to {chainCard.Name}");
                    return true;
                }
            }
            if (DefaultEffectVeiler())
            {
                HandTrapsUsedThisTurn++;
                DecisionTracer.TraceActivate("EffectVeiler", "Negating opponent monster");
                return true;
            }
            return false;
        }

        private bool ImpermanenceCondition()
        {
            if (Util.GetLastChainCard()?.Controller == 0) return false;
            if (Duel.LastChainPlayer == 1)
            {
                var chainCard = Util.GetLastChainCard();
                if (chainCard != null && chainCard.Controller == 1 && chainCard.Location == CardLocation.MonsterZone && !chainCard.IsDisabled() && !chainCard.IsShouldNotBeTarget())
                {
                    AI.SelectCard(chainCard);
                    return true;
                }
            }
            return DefaultInfiniteImpermanence();
        }

        private bool ImpermanenceSetCondition()
        {
            if (Duel.Turn == 1 || Duel.Phase == DuelPhase.Main2) return true;
            return Bot.GetMonsterCount() > 0;
        }

        private bool DrollCondition()
        {
            if (Duel.Player == 0) return false;
            if (Util.GetLastChainCard()?.Controller == 0) return false;
            if (Duel.LastChainPlayer == 1)
            {
                DecisionTracer.TraceActivate("DrollAndLockBird", "Shutting down opponent multi-search");
                return true;
            }
            return false;
        }

        private bool MulcharmyCondition()
        {
            if (Duel.Player == 0) return false;
            if (Util.GetLastChainCard()?.Controller == 0) return false;
            if (Bot.GetMonsterCount() == 0 && Duel.LastChainPlayer == 1)
            {
                DecisionTracer.TraceActivate("Mulcharmy", "Chaining Mulcharmy draw trigger");
                return true;
            }
            return false;
        }

        private bool NibiruCondition() => DefaultNibiru();

        private bool DominusPurgeCondition()
        {
            if (Util.GetLastChainCard()?.Controller == 0) return false;
            return SmartHandTrapChain();
        }

        private bool CrossoutDesignatorEffect()
        {
            if (!SmartHandTrapChain()) return false;
            ClientCard lastChainCard = Util.GetLastChainCard();
            if (lastChainCard == null || lastChainCard.Controller == 0) return false;

            int code = lastChainCard.Id;
            int alias = lastChainCard.Alias;
            if (alias != 0 && alias - code < 10) code = alias;
            if (code == 0) return false;

            if (GetRemainingCount(code) > 0)
            {
                AI.SelectAnnounceID(code);
                DecisionTracer.TraceActivate("CrossoutDesignator", $"Calling {lastChainCard.Name} to negate");
                return true;
            }
            return false;
        }

        // ============================================================
        // BOARD BREAKERS
        // ============================================================

        private bool HarpiesFeatherDusterEffect()
        {
            if (Duel.Player != 0) return false;
            return Enemy.GetSpellCount() >= 1;
        }

        private bool LightningStormEffect()
        {
            if (Duel.Player != 0) return false;
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup()) || Bot.GetSpells().Any(c => c != null && c.IsFaceup()))
                return false;

            if (Enemy.GetSpellCount() >= 2) { AI.SelectOption(1); return true; }
            if (Enemy.GetMonsterCount() >= 1) { AI.SelectOption(0); return true; }
            if (Enemy.GetSpellCount() > 0) { AI.SelectOption(1); return true; }
            return false;
        }

        private bool CosmicCycloneEffect()
        {
            if (Bot.LifePoints <= 1000) return false;
            if (Enemy.GetSpellCount() == 0) return false;
            var target = Enemy.GetSpells()
                .Where(c => c != null && !c.IsShouldNotBeTarget())
                .OrderByDescending(c => c.IsFaceup() && (c.HasType(CardType.Continuous) || c.HasType(CardType.Field)) ? 100 : 50)
                .FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool ForbiddenDropletEffect()
        {
            if (Duel.LastChainPlayer == 1 && Util.GetLastChainCard()?.Controller == 1)
            {
                var sendTarget = DeckPlugin?.MaterialEvaluator?.PickDiscardTarget(Bot.Hand);
                if (sendTarget != null) AI.SelectCard(sendTarget);
                return true;
            }
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1)
            {
                var enemyBoss = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled() && (c.Attack >= 2500 || OpponentHasActiveNegator()));
                if (enemyBoss != null)
                {
                    var sendTarget = DeckPlugin?.MaterialEvaluator?.PickDiscardTarget(Bot.Hand);
                    if (sendTarget != null) AI.SelectCard(sendTarget);
                    AI.SelectNextCard(enemyBoss);
                    return true;
                }
            }
            return false;
        }

        private bool TripleTacticsTalentEffect()
        {
            if (Duel.Player != 0) return false;
            if (CanDealLethal() && Enemy.GetMonsterCount() >= 1) { AI.SelectOption(1); return true; } // Steal monster
            if (Bot.Hand.Count <= 3) { AI.SelectOption(0); return true; } // Draw 2
            AI.SelectOption(2); // Look at opp hand and shuffle 1
            return true;
        }

        private bool ZeusEffect()
        {
            if (Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0) return false;
            // Send all other cards on the field to the GY
            int oppField = Enemy.GetMonsterCount() + Enemy.GetSpellCount();
            int ourField = Bot.GetMonsterCount() + Bot.GetSpellCount() - 1; // excluding Zeus
            if (oppField >= 2 || oppField > ourField)
            {
                DecisionTracer.TraceActivate("DivineArsenalAAZEUS", "Wiping field with Zeus");
                return true;
            }
            return false;
        }

        // ============================================================
        // SETUP SPELLS & ENGINES
        // ============================================================

        private bool IllusionOfChaosEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (Bot.HasInHand(CardId.PiriReisMap) && Bot.LifePoints > 4000 && Duel.Phase == DuelPhase.Main1 && Bot.GetMonsterCount() == 0 && Bot.GetSpellCount() == 0)
                    return false;

                if (Bot.GetRemainingCount(CardId.MagiciansSouls, 1) > 0)
                {
                    AI.SelectCard(CardId.MagiciansSouls);
                    DecisionTracer.TraceActivate("IllusionOfChaos", "Searching Magicians' Souls");
                    return true;
                }
            }
            return false;
        }

        private bool PiriReisMapEffect()
        {
            if (Duel.Player != 0) return false;
            if (Duel.Phase != DuelPhase.Main1) return false;
            if (Bot.LifePoints <= 4000) return false;

            if (Bot.GetRemainingCount(CardId.CupsyYummy, 3) > 0)
            {
                AI.SelectCard(CardId.CupsyYummy);
                DecisionTracer.TraceActivate("PiriReisMap", "Searching Cupsy Yummy starter");
                return true;
            }
            if (Bot.GetRemainingCount(CardId.MarshmaoYummy, 3) > 0)
            {
                AI.SelectCard(CardId.MarshmaoYummy);
                return true;
            }
            if (Bot.GetRemainingCount(CardId.MagiciansSouls, 1) > 0)
            {
                AI.SelectCard(CardId.MagiciansSouls);
                return true;
            }
            if (Bot.GetRemainingCount(CardId.JesterConfit, 1) > 0)
            {
                AI.SelectCard(CardId.JesterConfit);
                return true;
            }
            return false;
        }

        private bool HornetDronesEffect()
        {
            if (Duel.Player != 0) return false;
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool MagiciansSoulsEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (IsSpecialSummonBlocked()) return false;
                AI.SelectCard(CardId.IllusionOfChaos);
                DecisionTracer.TraceActivate("MagiciansSouls", "Sending Illusion to SS Souls");
                return true;
            }

            if (Card.Location == CardLocation.MonsterZone)
            {
                if (Bot.Hand.Count <= 1) return false;
                var sendTargets = Bot.Hand.Where(c => c != null && (c.IsCode(CardId.PiriReisMap) || c.IsCode(CardId.IllusionOfChaos))).Take(2).ToList();
                if (sendTargets.Count > 0)
                {
                    AI.SelectCard(sendTargets);
                    return true;
                }
            }
            return false;
        }

        private bool YummyusmentMignonEffect()
        {
            // Effect on field: Revive Level 1 Yummy if control Link-1
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                if (MignonFieldReviveUsed) return false;
                if (!HasLink1OnField()) return false;
                if (IsSpecialSummonBlocked()) return false;

                var gyYummy = Bot.Graveyard
                    .Where(c => c != null && YummyMonsters.Contains(c.Id) && c.IsCanRevive())
                    .OrderByDescending(c => c.Id == CardId.MarshmaoYummy ? 100 : (c.Id == CardId.CupsyYummy ? 80 : (c.Id == CardId.CookyYummy ? 60 : 50)))
                    .FirstOrDefault();

                if (gyYummy != null)
                {
                    MignonFieldReviveUsed = true;
                    AI.SelectCard(gyYummy);
                    DecisionTracer.TraceActivate("YummyusmentMignon", $"Reviving {gyYummy.Name} from GY");
                    return true;
                }
            }

            // Activation from Hand
            if (Card.Location == CardLocation.Hand)
            {
                if (Bot.HasInSpellZone(CardId.YummyusmentMignon)) return false;
                if (Bot.Hand.Any(c => c != null && YummyMonsters.Contains(c.Id)) && !NormalSummonUsed)
                {
                    if (HasLink1OnField())
                    {
                        DecisionTracer.TraceActivate("YummyusmentMignon", "Activating Field Spell from hand");
                        return true;
                    }
                    return false;
                }
                DecisionTracer.TraceActivate("YummyusmentMignon", "Activating Field Spell from hand");
                return true;
            }

            // GY Recycle
            if (Card.Location == CardLocation.Grave)
            {
                var gyYummies = Bot.Graveyard.Where(c => c != null && YummyMonsters.Contains(c.Id)).Take(2).ToList();
                if (gyYummies.Count >= 2)
                {
                    AI.SelectCard(gyYummies);
                    return true;
                }
            }

            return false;
        }

        private bool YummyusmentAcroqueyEffect()
        {
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                // Trigger when LIGHT Beast Synchro is SS: destroy 1 opp card
                var target = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                    .Where(c => c != null && !c.IsShouldNotBeTarget())
                    .OrderByDescending(c => c.IsCode(48680970, 48770333) ? 20000 : (c.IsMonster() ? c.Attack : 5000))
                    .FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("YummyusmentAcroquey", $"Destroying {target.Name}");
                    return true;
                }
            }

            // Activation from Hand
            if (Card.Location == CardLocation.Hand)
            {
                if (Bot.HasInSpellZone(CardId.YummyusmentAcroquey)) return false;
                return true;
            }

            return false;
        }

        // ============================================================
        // YUMMY MONSTER EFFECTS & SUMMONS
        // ============================================================

        private bool CupsyNormalSummon()
        {
            if (NormalSummonUsed) return false;
            NormalSummonUsed = true;
            DecisionTracer.TraceActivate("CupsyNormalSummon", "Normal Summoning Cupsy");
            return true;
        }

        private bool MarshmaoNormalSummon()
        {
            if (NormalSummonUsed) return false;
            NormalSummonUsed = true;
            DecisionTracer.TraceActivate("MarshmaoNormalSummon", "Normal Summoning Marshmao");
            return true;
        }

        private bool YummyGenericNormalSummon()
        {
            if (NormalSummonUsed) return false;
            NormalSummonUsed = true;
            return true;
        }

        private bool SanganNormalSummon()
        {
            if (NormalSummonUsed) return false;
            if (Bot.Hand.Any(c => c != null && YummyMonsters.Contains(c.Id))) return false;
            NormalSummonUsed = true;
            return true;
        }

        private bool MarshmaoSpSummon()
        {
            if (MarshmaoHandSSUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            bool validField = Bot.GetMonsterCount() == 0 ||
                Bot.GetMonsters().All(c => c == null || (c.IsFaceup() && c.HasAttribute(CardAttribute.Light) && c.HasRace(CardRace.Beast)));

            if (validField)
            {
                MarshmaoHandSSUsed = true;
                DecisionTracer.TraceActivate("MarshmaoSpSummon", "Special Summoning Marshmao from hand");
                return true;
            }
            return false;
        }

        private bool YummyHandSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (HasLink1OnField() || HasLevel2SynchroOnField())
            {
                DecisionTracer.TraceActivate("YummyHandSpSummon", $"Special Summoning {Card.Name} from hand");
                return true;
            }
            return false;
        }

        private bool MarshmaoYummyEffect()
        {
            if (Card.Location == CardLocation.Hand) return MarshmaoSpSummon();

            if (Card.Location == CardLocation.MonsterZone)
            {
                if (MarshmaoEffectUsed) return false;
                MarshmaoEffectUsed = true;

                AI.SelectCard(new[] {
                    CardId.YummyusmentAcroquey,
                    CardId.YummySurprise,
                    CardId.YummyusmentMignon
                });
                DecisionTracer.TraceActivate("MarshmaoYummy", "Resolving Marshmao search/place S/T");
                return true;
            }

            return false;
        }

        private bool CupsyYummyEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (CupsHandSSUsed) return false;
                if (IsSpecialSummonBlocked()) return false;
                if (HasLink1OnField() || HasLevel2SynchroOnField())
                {
                    CupsHandSSUsed = true;
                    DecisionTracer.TraceActivate("CupsyYummy", "Special Summoning Cupsy from hand");
                    return true;
                }
                return false;
            }

            if (Card.Location == CardLocation.MonsterZone)
            {
                if (CupsySearchUsed) return false;
                CupsySearchUsed = true;

                AI.SelectCard(new[] {
                    CardId.YummySurprise,
                    CardId.CookyYummy,
                    CardId.LollipoYummy,
                    CardId.MarshmaoYummy,
                    CardId.YummyusmentMignon
                });
                DecisionTracer.TraceActivate("CupsyYummy", "Searching Yummy card from Deck");
                return true;
            }

            return false;
        }

        private bool CookyYummyEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (CookyHandSSUsed) return false;
                if (IsSpecialSummonBlocked()) return false;
                if (HasLink1OnField() || HasLevel2SynchroOnField())
                {
                    CookyHandSSUsed = true;
                    DecisionTracer.TraceActivate("CookyYummy", "Special Summoning Cooky from hand");
                    return true;
                }
                return false;
            }

            if (Card.Location == CardLocation.MonsterZone)
            {
                if (CookyEffectUsed) return false;
                var target = Enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget())
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();

                if (target != null)
                {
                    CookyEffectUsed = true;
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("CookyYummy", $"Targeting {target.Name} to reduce ATK/destroy");
                    return true;
                }
            }

            return false;
        }

        private bool LollipoYummyEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (LollipoHandSSUsed) return false;
                if (IsSpecialSummonBlocked()) return false;
                if (HasLink1OnField() || HasLevel2SynchroOnField())
                {
                    LollipoHandSSUsed = true;
                    DecisionTracer.TraceActivate("LollipoYummy", "Special Summoning Lollipo from hand");
                    return true;
                }
                return false;
            }

            if (Card.Location == CardLocation.MonsterZone)
            {
                if (LollipoEffectUsed) return false;
                var gyTarget = Enemy.Graveyard
                    .OrderByDescending(c => c.IsMonster() ? 100 : (c.IsSpell() ? 50 : 20))
                    .FirstOrDefault();

                if (gyTarget != null)
                {
                    LollipoEffectUsed = true;
                    AI.SelectCard(gyTarget);
                    DecisionTracer.TraceActivate("LollipoYummy", $"Targeting {gyTarget.Name} in opponent GY");
                    return true;
                }
            }

            return false;
        }

        private bool JesterConfitSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool MagiciansSoulsSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool SantaClawsSpSummon()
        {
            var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && (c.Attack >= 2500 || OpponentHasActiveNegator()));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // ============================================================
        // EXTRA DECK SUMMONS & SYNCHRO CLIMBING
        // ============================================================

        private bool YummySnatchySpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;

            var material = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !IsAceCard(c) &&
                c.Level <= 4 && c.HasAttribute(CardAttribute.Light) && c.HasRace(CardRace.Beast));

            if (material != null)
            {
                AI.SelectCard(material);
                DecisionTracer.TraceActivate("YummySnatchySpSummon", $"Link Summoning Snatchy using {material.Name}");
                return true;
            }
            return false;
        }

        private bool YummySnatchyEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // 1. Placing Mignon or Acroquey (Trigger effect on SS):
            if (Bot.HasInSpellZone(CardId.YummyusmentMignon))
                SnatchyPlaceUsed = true;

            if (!SnatchyPlaceUsed)
            {
                SnatchyPlaceUsed = true;
                AI.SelectCard(new[] { CardId.YummyusmentMignon, CardId.YummyusmentAcroquey });
                DecisionTracer.TraceActivate("YummySnatchy", "Placing Yummy Field Spell from Deck");
                return true;
            }

            // 2. Quick Synchro Summon on our turn:
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1)
            {
                bool hasLv1Yummy = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Level == 1 && c.Id != CardId.YummySnatchy);
                bool alreadyHasSynchro = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && YummySynchros.Contains(c.Id));
                if (hasLv1Yummy && !alreadyHasSynchro)
                {
                    DecisionTracer.TraceActivate("YummySnatchy", "Synchro Summoning Cupsy Way on our turn!");
                    return true;
                }
                return false;
            }

            // 3. Quick Synchro Summon on opponent's turn:
            if (Duel.Player == 1)
            {
                bool hasNonTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Level == 1 && c.Id != CardId.YummySnatchy);
                if (hasNonTuner)
                {
                    DecisionTracer.TraceActivate("YummySnatchy", "Quick Synchro summoning during opponent turn!");
                    return true;
                }
            }

            return false;
        }

        private bool CupsyYummyWaySpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            bool hasSnatchy = Bot.HasInMonstersZone(CardId.YummySnatchy);
            bool hasLv1Yummy = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Level == 1 && c.Id != CardId.YummySnatchy);

            if (hasSnatchy && hasLv1Yummy)
            {
                DecisionTracer.TraceActivate("CupsyYummyWaySpSummon", "Synchro Summoning Cupsy Way using Snatchy (Lv1 Tuner) + Lv1 Yummy");
                return true;
            }

            bool hasTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Tuner) && c.Level == 1);
            bool hasNonTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.HasType(CardType.Tuner) && c.Level == 1);
            return hasTuner && hasNonTuner;
        }

        private bool CookyYummyWaySpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Duel.Player == 0 && (Duel.Turn > 1 || Enemy.GetMonsterCount() > 0))
            {
                bool hasSnatchy = Bot.HasInMonstersZone(CardId.YummySnatchy);
                bool hasLv1 = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Level == 1 && c.Id != CardId.YummySnatchy);
                return hasSnatchy && hasLv1;
            }
            return false;
        }

        private bool LollipoYummyWaySpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.Graveyard.Count(c => c != null && YummyMonsters.Contains(c.Id)) >= 2 && !HasLevel2SynchroOnField())
            {
                bool hasSnatchy = Bot.HasInMonstersZone(CardId.YummySnatchy);
                bool hasLv1 = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Level == 1 && c.Id != CardId.YummySnatchy);
                return hasSnatchy && hasLv1;
            }
            return false;
        }

        private bool CupsyYummyWayEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Opponent's Turn: Quick Tag-Out (EVENT_CHAINING, rp == 1 - tp)
            if (Duel.Player == 1)
            {
                if (Duel.LastChainPlayer != 1) return false;
                if (!Bot.Graveyard.Any(c => c != null && YummyMonsters.Contains(c.Id) && c.IsCanRevive()))
                    return false;

                var chainCard = Util.GetLastChainCard();
                bool isHarmless = chainCard != null && (chainCard.IsCode(70368879) || chainCard.IsCode(49238328));
                if (isHarmless) return false;

                AI.SelectCard(new[] {
                    CardId.CookyYummy,     // Destroys 1 monster when SS by Synchro!
                    CardId.MarshmaoYummy,  // Places Acroquey/Mignon
                    CardId.LollipoYummy,   // Banishes 1 GY card when SS by Synchro!
                    CardId.CupsyYummy      // Draws 1
                });
                DecisionTracer.TraceActivate("CupsyYummyWay", "Tagging out to SS 2 Yummies from GY in response to opponent!");
                return true;
            }

            // Our Turn: Search 2 DISTINCT Yummies and discard 1
            if (!CupsyWaySearchUsed && Duel.Player == 0)
            {
                CupsyWaySearchUsed = true;
                DecisionTracer.TraceActivate("CupsyYummyWay", "Searching 2 Yummy monsters from Deck and discarding 1");
                return true;
            }

            return false;
        }

        private bool CookyYummyWayEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Opponent's Turn: Quick Tag-Out
            if (Duel.Player == 1)
            {
                if (Duel.LastChainPlayer == 1)
                {
                    if (!Bot.Graveyard.Any(c => c != null && YummyMonsters.Contains(c.Id) && c.IsCanRevive()))
                        return false;

                    var chainCard = Util.GetLastChainCard();
                    bool isHarmless = chainCard != null && (chainCard.IsCode(70368879) || chainCard.IsCode(49238328));
                    if (!isHarmless)
                    {
                        AI.SelectCard(new[] {
                            CardId.CookyYummy,
                            CardId.MarshmaoYummy,
                            CardId.LollipoYummy,
                            CardId.CupsyYummy
                        });
                        DecisionTracer.TraceActivate("CookyYummyWay", "Tagging out to SS 2 Yummies from GY!");
                        return true;
                    }
                }
                return false;
            }

            // Face-down Book of Moon x2 on summon
            var oppMonsters = Enemy.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget())
                .OrderByDescending(c => c.Attack)
                .Take(2)
                .ToList();

            if (oppMonsters.Count > 0)
            {
                AI.SelectCard(oppMonsters);
                DecisionTracer.TraceActivate("CookyYummyWay", $"Flipping {oppMonsters.Count} monsters face-down");
                return true;
            }

            return false;
        }

        private bool LollipoYummyWayEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            if (Duel.Player == 1)
            {
                if (Duel.LastChainPlayer != 1) return false;
                AI.SelectCard(new[] { CardId.CookyYummy, CardId.MarshmaoYummy, CardId.LollipoYummy, CardId.CupsyYummy });
                DecisionTracer.TraceActivate("LollipoYummyWay", "Tagging out to SS 2 Yummies from GY!");
                return true;
            }

            if (Bot.Graveyard.Count(c => c != null && YummyMonsters.Contains(c.Id)) >= 2)
            {
                AI.SelectCard(new[] { CardId.MarshmaoYummy, CardId.CupsyYummy, CardId.CookyYummy });
                return true;
            }

            return false;
        }

        private bool SprightElfSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            if (Bot.HasInMonstersZone(CardId.SprightElf)) return false;

            bool hasLv2 = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.Level == 2 || c.Rank == 2 || (c.HasType(CardType.Link) && c.LinkCount == 2)));
            int otherMonsters = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !c.IsCode(CardId.SprightElf, CardId.HeraldOfTheArcLight, CardId.SPLittleKnight));

            if (hasLv2 && otherMonsters >= 2)
            {
                DecisionTracer.TraceActivate("SprightElfSpSummon", "Link Summoning Spright Elf");
                return true;
            }

            return false;
        }

        private bool SprightElfEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (SprightElfReviveUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            if (Duel.Player == 1)
            {
                bool isMainOrBattle = Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Battle || Duel.Phase == DuelPhase.Main2;
                bool isChaining = Duel.LastChainPlayer == 1;

                if (!isMainOrBattle && !isChaining) return false;

                var targetSynchro = Bot.Graveyard
                    .Where(c => c != null && c.IsMonster() && c.IsCanRevive() && YummySynchros.Contains(c.Id))
                    .OrderByDescending(c => c.Id == CardId.CookyYummyWay ? 100 : (c.Id == CardId.CupsyYummyWay ? 80 : 50))
                    .FirstOrDefault();

                if (targetSynchro != null)
                {
                    SprightElfReviveUsed = true;
                    AI.SelectCard(targetSynchro);
                    DecisionTracer.TraceActivate("SprightElf", $"Quick reviving {targetSynchro.Name} on opponent turn!");
                    return true;
                }
            }

            if (Duel.Player == 0)
            {
                var target = Bot.Graveyard
                    .Where(c => c != null && c.IsMonster() && c.IsCanRevive() &&
                        (c.Level == 2 || c.Rank == 2 || (c.HasType(CardType.Link) && c.LinkCount == 2)))
                    .OrderByDescending(c => YummySynchros.Contains(c.Id) ? 100 : 10)
                    .FirstOrDefault();

                if (target != null)
                {
                    SprightElfReviveUsed = true;
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("SprightElf", $"Reviving {target.Name} from GY");
                    return true;
                }
            }

            return false;
        }

        private bool SPLittleKnightSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            if (Bot.HasInMonstersZone(CardId.SPLittleKnight)) return false;

            // In MP1, avoid summoning S:P if enemy board is empty and we can attack directly
            if (Duel.Turn > 1 && Duel.Phase == DuelPhase.Main1 && Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0)
                return false;

            int effectMonsters = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c) && c.HasType(CardType.Effect));
            return effectMonsters >= 2;
        }

        private bool SPLittleKnightEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Trigger on summon: banish 1 card on field or in either GY
            if (Duel.CurrentChain.Count == 0)
            {
                var target = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                    .Where(c => c != null && !c.IsShouldNotBeTarget())
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("SPLittleKnight", $"Banish on summon: {target.Name}");
                    return true;
                }
            }

            // Quick effect: banish 2 face-up monsters until End Phase
            if (Duel.LastChainPlayer == 1)
            {
                var oppTarget = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget());
                if (oppTarget != null)
                {
                    AI.SelectCard(new[] { Card, oppTarget });
                    DecisionTracer.TraceActivate("SPLittleKnight", $"Quick banishing S:P and {oppTarget.Name}");
                    return true;
                }
            }

            return false;
        }

        private bool HeraldSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldStopExtending()) return false;

            var tuners = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasType(CardType.Tuner) && !IsAceCard(c)).ToList();
            var nonTuners = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.HasType(CardType.Tuner) && !IsAceCard(c)).ToList();

            foreach (var t in tuners)
            {
                foreach (var nt in nonTuners)
                {
                    if (t.Level > 0 && nt.Level > 0 && t.Level + nt.Level == 4)
                    {
                        AI.SelectCard(new[] { t, nt });
                        DecisionTracer.TraceActivate("HeraldSpSummon", $"Synchro Herald of the Arc Light using {t.Name} (Lv{t.Level}) + {nt.Name} (Lv{nt.Level})");
                        return true;
                    }
                }
            }
            return false;
        }

        private bool HeraldOfTheArcLightEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Duel.LastChainPlayer == 1 && Util.GetLastChainCard()?.Controller == 1)
            {
                DecisionTracer.TraceActivate("HeraldOfTheArcLight", "Tributing Herald to negate and destroy!");
                return true;
            }
            return false;
        }

        private bool NightingaleSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.HasInMonstersZone(CardId.LyriluscAssembledNightingale)) return false;

            // Only make Nightingale going 2nd to attack directly and overlay Zeus in MP2!
            if (Duel.Turn == 1) return false;

            var lv1s = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.Level == 1 && !IsAceCard(c)).Take(2).ToList();
            if (lv1s.Count >= 2)
            {
                AI.SelectCard(lv1s);
                DecisionTracer.TraceActivate("NightingaleSpSummon", "Xyz Summoning Nightingale");
                return true;
            }
            return false;
        }

        private bool NightingaleEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            // Detach 1 to make monsters immune to destruction and take 0 battle damage
            if (Duel.Player == 1 || Duel.Phase == DuelPhase.Battle)
            {
                DecisionTracer.TraceActivate("Nightingale", "Activating protection detach");
                return true;
            }
            return false;
        }

        private bool ZeusSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Duel.Phase != DuelPhase.Main2) return false;

            var nightingale = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.LyriluscAssembledNightingale));
            if (nightingale != null)
            {
                AI.SelectCard(nightingale);
                DecisionTracer.TraceActivate("ZeusSpSummon", "Overlaying Zeus on Nightingale in MP2!");
                return true;
            }
            return false;
        }

        private bool RelinquishedAnimaSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;

            // Check if opponent has a monster in the column Anima can point to
            var material = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Level == 1 && !c.HasType(CardType.Token) && !IsAceCard(c) && !YummyMonsters.Contains(c.Id));
            if (material != null && Enemy.GetMonsterCount() > 0)
            {
                AI.SelectCard(material);
                return true;
            }
            return false;
        }

        private bool RelinquishedAnimaEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget());
            if (target != null)
            {
                AI.SelectCard(target);
                DecisionTracer.TraceActivate("RelinquishedAnima", $"Equipping {target.Name}");
                return true;
            }
            return false;
        }

        private bool YummySurpriseEffect()
        {
            if (SurpriseUsed) return false;
            if (Card.Location != CardLocation.SpellZone) return false;

            if ((Duel.Phase == DuelPhase.Draw || Duel.Phase == DuelPhase.Standby) && Duel.LastChainPlayer != 1)
                return false;

            bool eternalSoulActive = Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(48680970));

            var allBeasts = Bot.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Light) && c.HasRace(CardRace.Beast))
                .ToList();

            var oppCards = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                .Where(c => c != null && !c.IsShouldNotBeTarget() && (!eternalSoulActive || !c.IsCode(46986414)))
                .OrderByDescending(c => {
                    if (c.IsCode(48680970, 48770333)) return 20000;
                    int s = 0;
                    if (c.IsSpell() || c.IsTrap())
                    {
                        if (c.IsFaceup() && (c.HasType(CardType.Continuous) || c.HasType(CardType.Field))) s += 15000;
                        else s += 5000;
                    }
                    else if (c.IsMonster())
                    {
                        if (c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link)) s += 12000;
                        if (c.Attack >= 2500) s += 10000;
                        s += c.Attack;
                    }
                    return s;
                })
                .ToList();

            var mainDeckBeasts = allBeasts
                .Where(c => !YummySynchros.Contains(c.Id) && c.Id != CardId.YummySnatchy)
                .OrderBy(c => c.Attack)
                .ToList();

            // OPTION 0: Bounce 2 LIGHT Beasts + 2 Opponent cards
            bool canUseBounce = allBeasts.Count >= 2 && oppCards.Count >= 2;
            if (canUseBounce && Duel.Player == 1)
            {
                bool hasSynchroOnField = allBeasts.Any(c => YummySynchros.Contains(c.Id));
                bool isBattleOrEnd = Duel.Phase == DuelPhase.Battle || Duel.Phase == DuelPhase.End;
                bool opponentCommitted = oppCards.Any(c => c.IsCode(48680970, 48770333) || (c.IsMonster() && c.Attack >= 2500));

                if (mainDeckBeasts.Count >= 2 || !hasSynchroOnField || isBattleOrEnd || opponentCommitted || Duel.LastChainPlayer == 1)
                {
                    var selectedOurBeasts = mainDeckBeasts.Take(2).ToList();
                    if (selectedOurBeasts.Count < 2)
                    {
                        selectedOurBeasts = allBeasts.OrderBy(c => YummySynchros.Contains(c.Id) ? 100 : 10).Take(2).ToList();
                    }

                    var selectedOpp = oppCards.Take(2).ToList();
                    if (selectedOurBeasts.Count == 2 && selectedOpp.Count == 2)
                    {
                        SurpriseUsed = true;
                        SurpriseOurBounceCount = 0;
                        SurpriseOppBounceCount = 0;
                        AI.SelectOption(0);
                        AI.SelectCard(selectedOurBeasts.Concat(selectedOpp).ToList());
                        DecisionTracer.TraceActivate("YummySurprise", "Bouncing 2 beasts and 2 opponent cards!");
                        return true;
                    }
                }
            }

            // OPTION 1: Special Summon 1 Yummy from GY or hand
            if (!IsSpecialSummonBlocked() && Bot.GetMonsterCount() < 5)
            {
                var gyYummy = Bot.Graveyard.Where(c => c != null && YummyMonsters.Contains(c.Id) && c.IsCanRevive())
                    .OrderByDescending(c => {
                        if (c.Id == CardId.CookyYummy && Enemy.GetMonsterCount() > 0) return 100;
                        if (c.Id == CardId.MarshmaoYummy) return 90;
                        if (c.Id == CardId.LollipoYummy && Enemy.Graveyard.Count > 0) return 80;
                        if (c.Id == CardId.CupsyYummy) return 70;
                        return 10;
                    })
                    .FirstOrDefault();

                if (gyYummy != null)
                {
                    if (Duel.Player == 0 || Duel.Phase == DuelPhase.Battle || Duel.Phase == DuelPhase.End || Enemy.GetMonsterCount() > 0)
                    {
                        SurpriseUsed = true;
                        AI.SelectOption(1);
                        AI.SelectCard(gyYummy);
                        DecisionTracer.TraceActivate("YummySurprise", $"Special Summoning {gyYummy.Name} from GY!");
                        return true;
                    }
                }
            }

            return false;
        }

        private bool YummySurpriseSetCondition()
        {
            if (Duel.Turn == 1 || Duel.Phase == DuelPhase.Main2) return true;
            bool canAttack = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsAttack() && c.Attack > 0);
            return !canAttack;
        }

        private bool LinkSpiderSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            var normalToken = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.HasType(CardType.Normal));
            if (normalToken != null)
            {
                AI.SelectCard(normalToken);
                return true;
            }
            return false;
        }

        private bool LinkuribohSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            var level1 = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Level == 1 && !IsAceCard(c) && !YummyMonsters.Contains(c.Id));
            if (level1 != null)
            {
                AI.SelectCard(level1);
                return true;
            }
            return false;
        }

        private bool AlmirajSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            if (HasLink1OnField()) return false;

            var sangan = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.Sangan) && !c.IsSpecialSummoned);
            if (sangan != null)
            {
                AI.SelectCard(sangan);
                DecisionTracer.TraceActivate("AlmirajSpSummon", "Link Summoning Almiraj using Sangan");
                return true;
            }

            var normalSummonedLowAtk = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() &&
                !IsAceCard(c) && c.Attack <= 1000 && !c.IsSpecialSummoned &&
                !c.HasType(CardType.Link));

            if (normalSummonedLowAtk != null && !Bot.HasInMonstersZone(CardId.YummySnatchy))
            {
                AI.SelectCard(normalSummonedLowAtk);
                DecisionTracer.TraceActivate("AlmirajSpSummon", $"Link Summoning Almiraj using {normalSummonedLowAtk.Name}");
                return true;
            }

            return false;
        }

        private bool KagariSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;

            var token = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && (c.IsCode(52340445) || c.IsCode(26077387)));
            if (token != null)
            {
                AI.SelectCard(token);
                DecisionTracer.TraceActivate("KagariSpSummon", "Link Summoning Kagari using Sky Striker token");
                return true;
            }

            return false;
        }

        private bool KagariEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            var drones = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCode(CardId.SkyStrikerMechaHornetDrones));
            if (drones != null)
            {
                AI.SelectCard(drones);
                return true;
            }
            return true;
        }

        // ============================================================
        // SIDE TECHS & TRAPS
        // ============================================================

        private bool BookOfEclipseEffect()
        {
            if (Duel.Player != 1) return false;
            return Enemy.GetMonsters().Count(c => c != null && c.IsFaceup()) >= 2;
        }

        private bool MistakenArrestEffect()
        {
            if (Duel.Player != 1) return false;
            return true;
        }

        private bool DimensionalBarrierEffect()
        {
            if (Duel.Player != 1) return false;
            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Link))) { AI.SelectOption(4); return true; }
            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Synchro))) { AI.SelectOption(1); return true; }
            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Xyz))) { AI.SelectOption(3); return true; }
            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Fusion))) { AI.SelectOption(0); return true; }
            return false;
        }

        private bool EvenlyMatchedEffect()
        {
            if (Duel.Player != 1) return false;
            int enemyField = Enemy.GetMonsterCount() + Enemy.GetSpellCount();
            int ourField = Bot.GetMonsterCount() + Bot.GetSpellCount();
            return enemyField >= 3 && enemyField > ourField + 1;
        }

        // ============================================================
        // CALLBACKS & PROTOCOLS
        // ============================================================

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            // 🔒 Rule 6: Hostile Prompt Safeguard — Always refuse opponent prompts!
            if (card != null && card.Controller == 1) return false;

            if (card != null && card.Controller == 0)
            {
                if (card.IsCode(CardId.YummySnatchy)) return true; // Always place Field Spell!
                if (card.IsCode(CardId.CupsyYummyWay, CardId.LollipoYummyWay)) return true;
                if (card.IsCode(CardId.CookyYummyWay))
                {
                    // Book of Moon on summon - only activate if opp has face-up target
                    if (desc == Util.GetStringId(CardId.CookyYummyWay, 0))
                    {
                        return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget());
                    }
                    // Tag-Out quick effect - tag out when opp acts if we have GY targets
                    if (desc == Util.GetStringId(CardId.CookyYummyWay, 1))
                    {
                        return Duel.LastChainPlayer == 1 && Bot.Graveyard.Any(c => c != null && YummyMonsters.Contains(c.Id) && c.IsCanRevive());
                    }
                    return true;
                }
                if (card.IsCode(CardId.MarshmaoYummy, CardId.CupsyYummy, CardId.CookyYummy, CardId.LollipoYummy)) return true;
                if (card.IsCode(CardId.HeraldOfTheArcLight, CardId.SPLittleKnight, CardId.SkyStrikerAceKagari)) return true;
                if (card.IsCode(CardId.YummyusmentAcroquey)) return true;
            }

            return base.OnSelectEffectYn(card, desc);
        }

        public override int OnSelectOption(IList<long> options)
        {
            if (Card != null)
            {
                // Cooky Yummy when SS by Synchro:
                // Option 0: -1000 ATK (Stringid 2)
                // Option 1: Destroy that monster (Stringid 3)
                long cookyDestroy = Util.GetStringId(CardId.CookyYummy, 3);
                if (Card.IsCode(CardId.CookyYummy) && options.Contains(cookyDestroy))
                {
                    DecisionTracer.Trace("OnSelectOption", "Cooky Yummy: Selecting option to destroy target!");
                    return options.IndexOf(cookyDestroy);
                }

                // Lollipo Yummy: Option 3 is Banish it
                long lollipoBanish = Util.GetStringId(CardId.LollipoYummy, 3);
                if (Card.IsCode(CardId.LollipoYummy) && options.Contains(lollipoBanish))
                {
                    return options.IndexOf(lollipoBanish);
                }

                // Yummy Surprise: Stringid 1: Bounce 2 beasts + 2 opp cards; Stringid 2: SS 1 Yummy
                long surpriseBounce = Util.GetStringId(CardId.YummySurprise, 1);
                long surpriseSp = Util.GetStringId(CardId.YummySurprise, 2);

                if (Card.IsCode(CardId.YummySurprise))
                {
                    if (options.Contains(surpriseBounce))
                    {
                        int ourBeasts = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Light) && c.HasRace(CardRace.Beast));
                        int oppCards = Enemy.GetMonsterCount() + Enemy.GetSpellCount();
                        if (ourBeasts >= 2 && oppCards >= 2 && Duel.Player == 1)
                        {
                            return options.IndexOf(surpriseBounce);
                        }
                    }
                    if (options.Contains(surpriseSp))
                    {
                        return options.IndexOf(surpriseSp);
                    }
                }
            }

            return base.OnSelectOption(options);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // HINTMSG_DISCARD = 501
            if (hint == 501)
            {
                var bestDiscard = DeckPlugin?.MaterialEvaluator?.PickDiscardTarget(cards, min);
                if (bestDiscard != null) return new[] { bestDiscard };
            }

            // HINTMSG_TARGET = 551, HINTMSG_FACEUP = 575, HINTMSG_POSCHANGE = 528, HINTMSG_FACEDOWN = 561
            if (hint == 551 || hint == 575 || hint == 528 || hint == 561)
            {
                if (hint == 561 || hint == 528)
                {
                    var oppFaceup = cards.Where(c => c != null && c.Controller == 1 && c.IsFaceup()).OrderByDescending(c => c.Attack).ToList();
                    if (oppFaceup.Count >= min) return oppFaceup.Take(max).ToList();
                    if (cancelable) return new List<ClientCard>();
                }
                bool eternalSoulActive = Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(48680970));
                var sorted = cards.OrderByDescending(c => {
                    if (c == null) return -999;
                    int score = (c.Controller == 1) ? 10000 : 0;
                    if (c.IsSpell() || c.IsTrap())
                    {
                        if (c.IsCode(48680970, 48770333)) return score + 25000;
                        if (c.IsFaceup())
                        {
                            if (c.HasType(CardType.Continuous) || c.HasType(CardType.Field)) score += 6000;
                            else score += 2000;
                        }
                        else score += 1000;
                        return score;
                    }
                    if (c.IsMonster())
                    {
                        if (eternalSoulActive && c.IsCode(46986414)) return -50000;
                        if (c.IsFaceup() && !c.IsDisabled())
                        {
                            if (c.Attack >= 2500) score += 5000;
                            if (c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link)) score += 2000;
                        }
                        return score + c.Attack;
                    }
                    return score;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // HINTMSG_REMOVE = 503 / 504 (Banishing cards from opp GY, e.g. Lollipo target)
            if (hint == 503 || hint == 504)
            {
                var oppGyTarget = cards.Where(c => c != null && c.Controller == 1)
                    .OrderByDescending(c => {
                        if (c.IsCode(48680970, 48770333)) return 100000;
                        if (c.IsMonster())
                        {
                            if (c.IsCode(30012506, 77411244, 23893227)) return 50000;
                            if (c.IsCode(46986414, 89631139)) return 40000;
                            if (c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link)) return 30000;
                            return c.Attack;
                        }
                        return 10;
                    })
                    .FirstOrDefault();

                if (oppGyTarget != null) return new[] { oppGyTarget };
            }

            // HINTMSG_TODECK = 507 (Return/place on deck, e.g. Illusion of Chaos return to deck)
            if (hint == 507)
            {
                var sorted = cards.OrderByDescending(c => {
                    if (c == null) return -999;
                    if (c.Controller == 1) return 20000;

                    if (c.IsCode(CardId.IllusionOfChaos)) return 10000;
                    if (Bot.Hand.Count(h => h.Id == c.Id) > 1) return 8000;
                    if (c.IsCode(CardId.JesterConfit)) return 5000;
                    if (c.IsCode(CardId.TripleTacticsTalent) && Duel.Player == 1) return 4000;
                    if (c.IsCode(CardId.PiriReisMap) && (Bot.LifePoints <= 4000 || Bot.GetMonsterCount() > 0)) return 3000;
                    if (c.IsCode(CardId.EffectVeiler, CardId.GhostBelleAndHauntedMansion)) return 1000;

                    if (YummyMonsters.Contains(c.Id))
                    {
                        if (Bot.Hand.Count(h => h.Id == c.Id) <= 1) return -5000;
                        return 2000;
                    }
                    return 500;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // HINTMSG_RTOHAND = 505 (Bounce target selection, e.g. Yummy Surprise)
            if (hint == 505)
            {
                bool eternalSoulActive = Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(48680970));
                var oppCards = cards.Where(c => c != null && c.Controller == 1).OrderByDescending(c => {
                    if (c.IsCode(48680970, 48770333)) return 200000;
                    if (eternalSoulActive && c.IsCode(46986414)) return -500000;
                    int score = 50000;
                    if (c.IsSpell() || c.IsTrap())
                    {
                        if (c.IsFaceup() && (c.HasType(CardType.Continuous) || c.HasType(CardType.Field))) score += 30000;
                        else score += 10000;
                    }
                    if (c.IsMonster())
                    {
                        if (c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link)) score += 25000;
                        if (c.Attack >= 2500) score += 20000;
                        score += c.Attack;
                    }
                    return score;
                }).ToList();

                var ourCards = cards.Where(c => c != null && c.Controller == 0).OrderBy(c => {
                    if (c.Id == CardId.SprightElf) return 100000;
                    if (YummySynchros.Contains(c.Id)) return 50000;
                    if (c.Id == CardId.YummySnatchy) return 20000;
                    if (c.Id == CardId.CupsyYummy) return 10;
                    if (c.Id == CardId.MarshmaoYummy) return 20;
                    if (c.Id == CardId.CookyYummy) return 30;
                    if (c.Id == CardId.LollipoYummy) return 40;
                    return 100;
                }).ToList();

                if (cards.All(c => c.Controller == 0)) return ourCards.Take(max).ToList();
                if (cards.All(c => c.Controller == 1)) return oppCards.Take(max).ToList();

                if (max >= 4 && min >= 4)
                {
                    var doubleBounce = ourCards.Take(2).Concat(oppCards.Take(2)).ToList();
                    if (doubleBounce.Count >= min) return doubleBounce;
                }

                if (SurpriseOurBounceCount < 2 && ourCards.Count > 0)
                {
                    SurpriseOurBounceCount++;
                    return ourCards.Take(max).ToList();
                }
                else if (SurpriseOppBounceCount < 2 && oppCards.Count > 0)
                {
                    SurpriseOppBounceCount++;
                    return oppCards.Take(max).ToList();
                }

                return ourCards.Concat(oppCards).Take(max).ToList();
            }

            // HINTMSG_DESTROY = 502 (Cooky pop monster target)
            if (hint == 502)
            {
                var oppTarget = cards.Where(c => c != null && c.Controller == 1 && c.IsFaceup() && !c.IsShouldNotBeTarget())
                    .OrderByDescending(c => {
                        int s = 0;
                        if (c.IsMonster())
                        {
                            if (c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link)) s += 10000;
                            if (c.Attack >= 2500) s += 8000;
                            s += c.Attack;
                        }
                        return s;
                    })
                    .FirstOrDefault();

                if (oppTarget != null) return new[] { oppTarget };
            }

            // HINTMSG_ATOHAND = 506 (Searching from Deck)
            if (hint == 506)
            {
                // When selecting 2 cards (Cupsy Way), select 2 DISTINCT Yummy monsters!
                if (max == 2)
                {
                    var selected = new List<ClientCard>();
                    var candidates = cards.Where(c => c != null).ToList();

                    var cooky = candidates.FirstOrDefault(c => c.Id == CardId.CookyYummy && !Bot.HasInHand(CardId.CookyYummy));
                    if (cooky != null) { selected.Add(cooky); candidates.Remove(cooky); }

                    var lollipo = candidates.FirstOrDefault(c => c.Id == CardId.LollipoYummy && !Bot.HasInHand(CardId.LollipoYummy));
                    if (lollipo != null && selected.Count < 2) { selected.Add(lollipo); candidates.Remove(lollipo); }

                    var marshmao = candidates.FirstOrDefault(c => c.Id == CardId.MarshmaoYummy && !Bot.HasInHand(CardId.MarshmaoYummy));
                    if (marshmao != null && selected.Count < 2) { selected.Add(marshmao); candidates.Remove(marshmao); }

                    var cupsy = candidates.FirstOrDefault(c => c.Id == CardId.CupsyYummy && !Bot.HasInHand(CardId.CupsyYummy));
                    if (cupsy != null && selected.Count < 2) { selected.Add(cupsy); candidates.Remove(cupsy); }

                    while (selected.Count < max && candidates.Count > 0)
                    {
                        var next = candidates.FirstOrDefault(c => !selected.Any(s => s.Id == c.Id)) ?? candidates.First();
                        selected.Add(next);
                        candidates.Remove(next);
                    }

                    if (selected.Count >= min) return selected;
                }

                // Single search: delegate to DeckPlugin.Strategy
                var searchTarget = DeckPlugin?.Strategy?.PickSearchTarget(cards, Card);
                if (searchTarget != null) return new[] { searchTarget };
            }

            // HINTMSG_TOFIELD = 527 / HINTMSG_SET = 510 (Snatchy placing Field Spell)
            if (hint == 527 || hint == 510)
            {
                var mignon = cards.FirstOrDefault(c => c != null && c.Id == CardId.YummyusmentMignon && !Bot.HasInSpellZone(CardId.YummyusmentMignon));
                if (mignon != null) return new[] { mignon };

                var acroquey = cards.FirstOrDefault(c => c != null && c.Id == CardId.YummyusmentAcroquey);
                if (acroquey != null) return new[] { acroquey };

                var fallback = cards.FirstOrDefault(c => c != null && (c.Id == CardId.YummyusmentMignon || c.Id == CardId.YummyusmentAcroquey));
                if (fallback != null) return new[] { fallback };
            }

            // HINTMSG_SPSUMMON = 509 (Tag-out / Revival / Quick Synchro)
            if (hint == 509)
            {
                var ssTarget = DeckPlugin?.Strategy?.PickSpecialSummonTarget(cards);
                if (ssTarget != null) return new[] { ssTarget };
            }

            // HINTMSG_ATTACKTARGET = 549 (Battle Target Selection)
            if (hint == 549)
            {
                var attacker = Bot.BattlingMonster;
                int attackerAtk = attacker?.Attack ?? 0;

                var beatable = cards.Where(c => c != null && c.IsFaceup() && c.Location == CardLocation.MonsterZone
                    && (c.IsAttack() ? c.Attack < attackerAtk : c.Defense < attackerAtk)).ToList();
                if (beatable.Count >= min)
                    return beatable.OrderByDescending(c => c.Attack).Take(max).ToList();

                if (cancelable) return null;

                var weakest = cards.Where(c => c != null && c.IsFaceup() && c.Location == CardLocation.MonsterZone)
                    .OrderBy(c => c.Attack).Take(max).ToList();
                if (weakest.Count >= min) return weakest;
            }

            // Materials selection (Protect Ace cards)
            if (hint == 511 || hint == 512 || hint == 513 || hint == 533 || hint == 508 || hint == 504)
            {
                var sorted = DeckPlugin?.MaterialEvaluator?.SortMaterials(cards, min) ?? cards.OrderBy(GetMaterialPriority).ToList();
                if (cancelable)
                {
                    var nonFieldAces = sorted.Where(c => !(c.Location == CardLocation.MonsterZone && IsAceCard(c))).ToList();
                    if (nonFieldAces.Count < min) return null;
                    return Util.CheckSelectCount(nonFieldAces, cards, min, max);
                }
                return Util.CheckSelectCount(sorted, cards, min, max);
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        private bool YummyMonsterRepos()
        {
            if (Card == null || !Card.IsFaceup()) return false;

            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Battle))
            {
                if (Card.IsDefense())
                {
                    bool hasFieldSpell = Bot.HasInSpellZone(CardId.YummyusmentMignon, true);
                    int beastCount = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Light) && c.HasRace(CardRace.Beast));
                    int effectiveAtk = Card.Attack + (hasFieldSpell && Card.HasAttribute(CardAttribute.Light) && Card.HasRace(CardRace.Beast) ? beastCount * 500 : 0);

                    var strongestEnemy = Enemy.GetMonsters()
                        .Where(c => c != null && c.IsFaceup())
                        .OrderByDescending(c => c.Attack)
                        .FirstOrDefault();

                    if (CanDealLethal() ||
                        (Enemy.GetMonsterCount() == 0 && (effectiveAtk > 0 || Card.Attack > 0)) ||
                        (strongestEnemy != null && (effectiveAtk > strongestEnemy.Attack || Card.Attack > strongestEnemy.Attack)))
                    {
                        return true;
                    }
                }
            }

            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main2)
            {
                if (Card.IsAttack() && Card.Defense > Card.Attack)
                {
                    return true;
                }
            }

            return DefaultMonsterRepos();
        }

        public override ClientCard OnSelectAttacker(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            var hasAvramax = defenders.Any(d => d != null && d.IsCode(21887175));
            if (hasAvramax)
            {
                var nonAvramaxDefenders = defenders.Where(d => d != null && !d.IsCode(21887175)).ToList();
                if (nonAvramaxDefenders.Count == 0) return null;
                defenders = nonAvramaxDefenders;
            }

            if (defenders.Count == 0)
            {
                var directAttackers = attackers.Where(c => c != null && c.Attack > 0).ToList();
                if (directAttackers.Count > 0)
                    return base.OnSelectAttacker(directAttackers, defenders);
            }

            var safeAttackers = attackers.Where(c => c != null && (c.Attack >= 800 || CanDealLethal())).ToList();
            if (safeAttackers.Count > 0)
            {
                return base.OnSelectAttacker(safeAttackers, defenders);
            }
            return base.OnSelectAttacker(attackers, defenders);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            int[] pureUtilityMonsters = {
                CardId.MaxxC, CardId.EffectVeiler,
                CardId.Sangan, CardId.JesterConfit,
                CardId.DrollAndLockBird, CardId.MulcharmyFuwalos,
                CardId.MulcharmyPurulia, CardId.RelinquishedAnima
            };

            if (pureUtilityMonsters.Contains(cardId) && positions.Contains(CardPosition.FaceUpDefence))
            {
                return CardPosition.FaceUpDefence;
            }

            // On opponent's turn, ANY monster SS should enter in Defense for maximum protection
            if (Duel.Player == 1 && positions.Contains(CardPosition.FaceUpDefence))
            {
                return CardPosition.FaceUpDefence;
            }

            // In Turn 1, all non-boss monsters enter in Defense
            if (Duel.Turn == 1 && positions.Contains(CardPosition.FaceUpDefence))
            {
                if (YummyMonsters.Contains(cardId))
                {
                    return CardPosition.FaceUpDefence;
                }
            }

            // Combat turn (Turn > 1, Main 1): Bosses and attackers enter in Attack
            if (Duel.Player == 0 && Duel.Turn > 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Battle))
            {
                if (BossMonsters.Contains(cardId) || YummyMonsters.Contains(cardId) || CanDealLethal())
                {
                    if (positions.Contains(CardPosition.FaceUpAttack))
                        return CardPosition.FaceUpAttack;
                }
            }

            if (positions.Contains(CardPosition.FaceUpDefence))
            {
                return CardPosition.FaceUpDefence;
            }

            return base.OnSelectPosition(cardId, positions);
        }
    }
}
