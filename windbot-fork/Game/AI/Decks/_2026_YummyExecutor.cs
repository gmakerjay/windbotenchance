// =========================================================================================
// CARD AUDIT — 2026_Yummy (Championship Tier-1 Metagame Engine)
// | Card Name                          | Type    | OPT? | Cost   | Effect Summary                                                   |
// | :--------------------------------- | :-----: | :--: | :----: | :--------------------------------------------------------------- |
// | Marshmao☆Yummy (10966439)          | Monster | HOPT | None   | Lv1 Non-Tuner. Free SS if LIGHT Beast. S/T from GY or Deck (if Synchro SS)|
// | Cupsy☆Yummy (31425736)             | Monster | HOPT | None   | Lv1 Non-Tuner. Free SS if Link-1/Synchro-2. Search Yummy card (Draw 1 if Synchro SS)|
// | Cooky☆Yummy (68810435)             | Monster | HOPT | None   | Lv1 Non-Tuner. Free SS if Link-1/Synchro-2. -1000 ATK (Destroy 1 if Synchro SS)|
// | Lollipo☆Yummy (4215180)            | Monster | HOPT | None   | Lv1 Non-Tuner. Free SS if Link-1/Synchro-2. Shuffle GY (Banish 1 if Synchro SS)|
// | Yummyusment☆Mignon (66975205)      | Spell   | HOPT | None   | Field: +500 ATK per LIGHT Beast on field. Revive Lv1 Yummy if Link-1 on field|
// | Yummy☆Surprise (29369059)          | Trap    | HOPT | None   | 1) Bounce 2 LIGHT Beasts + 2 opp cards; 2) SS Yummy from GY; 3) Recycle Field|
// | Yummy★Snatchy (30581601)           | Link-1  | HOPT | 100 LP | Link-1 (1 LIGHT Beast). Places Mignon from Deck. Quick Synchro in opp turn|
// | Cupsy★Yummy Way (31603289)         | Synchro | HOPT | Discard| Lv2 Synchro (treats Link-1 as Lv1 Tuner). Search 2 Yummies. Tag-out to SS 2|
// | Cooky★Yummy Way (67098897)         | Synchro | HOPT | None   | Lv2 Synchro (treats Link-1 as Lv1 Tuner). Book of Moon x2. Tag-out to SS 2|
// | Lollipo★Yummy Way (93192592)       | Synchro | HOPT | None   | Lv2 Synchro. Revive 2 Yummies. Tag-out to SS 2               |
// | Spright Elf (27381364)             | Link-2  | HOPT | None   | Target protect linked monsters. Quick revive Level/Rank/Link 2   |
// | Borreload Savage Dragon (27548199) | Synchro | HOPT | None   | Level 8 Boss (situational with Handtrap/Tuner). Omni-negate       |
// | Herald of the Arc Light (79606837) | Synchro | No   | Tribute| Level 4 Boss (situational with Handtrap/Tuner). Omni-negate       |
// =========================================================================================
// ACE CARDS: Spright Elf, Cupsy★Yummy Way, Cooky★Yummy Way, Lollipo★Yummy Way, Borreload Savage Dragon, Herald of the Arc Light
// PRIMARY COMBO: 1 Yummy -> Link Snatchy (Place Mignon) -> Mignon revives Yummy -> Synchro Cupsy Way (Lv2) -> Search Cooky + Lollipo -> Hand SS Cooky + Lollipo -> Link Spright Elf -> Set Yummy☆Surprise
// OPPONENT DISRUPTIONS: Tag-out Cupsy Way (Pop 1 with Cooky + Banish 1 with Lollipo) + Spright Elf revives Cooky Way (Book of Moon x2) + Yummy☆Surprise (Bounce 2) -> 6-7 Disruptions Total!
// =========================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_Yummy", "2026_Yummy")]
    public class _2026_YummyExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck — Yummy Archetype
            public const int MarshmaoYummy = 10966439;
            public const int CupsyYummy = 31425736;
            public const int CookyYummy = 68810435;
            public const int LollipoYummy = 4215180;
            public const int YummyusmentMignon = 66975205;
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
            public const int BorreloadSavageDragon = 27548199;
            public const int SprightElf = 27381364;
            public const int HeraldOfTheArcLight = 79606837;
            public const int CupsyYummyWay = 31603289;
            public const int CookyYummyWay = 67098897;
            public const int LollipoYummyWay = 93192592;
            public const int YummySnatchy = 30581601;
            public const int MartialMetalMarcher = 81846453;
            public const int CupidPitch = 21915012;
            public const int KewlTuneRS = 15665977;
            public const int CrystronHalqifibrax = 50588353;
            public const int Linkuriboh = 41999284;
            public const int LinkSpider = 98978921;
            public const int SalamangreatAlmiraj = 60303245;
            public const int SkyStrikerAceKagari = 63288573;

            // Side Techs
            public const int SantaClaws = 46565218;
            public const int BookOfEclipse = 35480699;
            public const int YummyusmentAcroquey = 93360904;
            public const int MistakenArrest = 4227096;
            public const int DimensionalBarrier = 83326048;
            public const int EvenlyMatched = 15693423;
        }

        private static readonly int[] BossMonsters = {
            CardId.SprightElf,
            CardId.CupsyYummyWay,
            CardId.CookyYummyWay,
            CardId.LollipoYummyWay,
            CardId.BorreloadSavageDragon,
            CardId.HeraldOfTheArcLight
        };

        private static readonly int[] YummyMonsters = {
            CardId.MarshmaoYummy,
            CardId.CupsyYummy,
            CardId.CookyYummy,
            CardId.LollipoYummy
        };

        private static readonly int[] YummySynchros = {
            CardId.CupsyYummyWay,
            CardId.CookyYummyWay,
            CardId.LollipoYummyWay
        };

        // Turn tracking flags
        private bool _normalSummonUsed = false;
        private bool _marshmaoHandSSUsed = false;
        private bool _marshmaoEffectUsed = false;
        private bool _cupsHandSSUsed = false;
        private bool _cupsySearchUsed = false;
        private bool _cookyHandSSUsed = false;
        private bool _cookyEffectUsed = false;
        private bool _lollipoHandSSUsed = false;
        private bool _lollipoEffectUsed = false;
        private bool _mignonFieldReviveUsed = false;
        private bool _snatchyPlaceUsed = false;
        private bool _cupsyWaySearchUsed = false;
        private bool _sprightElfReviveUsed = false;
        private bool _borreloadNegateUsed = false;
        private bool _surpriseUsed = false;
        private int _handTrapsUsedThisTurn = 0;

        public override bool IsAceCard(ClientCard card)
        {
            return card != null && BossMonsters.Contains(card.Id);
        }

        protected override bool IsBoardStrongEnough()
        {
            bool hasElf = Bot.HasInMonstersZone(CardId.SprightElf);
            bool hasSynchro = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && YummySynchros.Contains(c.Id));
            bool hasTrap = Bot.HasInSpellZone(CardId.YummySurprise);

            if (hasElf && hasSynchro && hasTrap) return true;
            if (hasElf && hasSynchro) return true;
            if (hasSynchro && hasTrap) return true;

            int disr = CountDisruptions();
            if (disr >= 3) return true;

            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            // If we have our established optimal end board going 1st, stop to conserve resources
            if (Duel.Turn == 1 || (Duel.Player == 0 && Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0))
            {
                if (Bot.HasInMonstersZone(CardId.SprightElf) &&
                    Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && YummySynchros.Contains(c.Id)) &&
                    Bot.HasInSpellZone(CardId.YummySurprise))
                {
                    return true;
                }
            }

            // OTK check
            if (CanDealLethal())
            {
                return true;
            }

            return false;
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c))
            {
                if (c.Location == CardLocation.MonsterZone && c.IsFaceup())
                {
                    if (c.IsCode(CardId.SprightElf, CardId.BorreloadSavageDragon, CardId.HeraldOfTheArcLight))
                        return 10000;
                    if (YummySynchros.Contains(c.Id))
                    {
                        // Allow Cupsy Way to be used for Spright Elf only if we already searched and have follow-up
                        if (!Bot.HasInMonstersZone(CardId.SprightElf) && _cupsyWaySearchUsed)
                            return 250;
                        return 8000;
                    }
                }
                return 900;
            }
            if (c.IsCode(CardId.AshBlossomAndJoyousSpring, CardId.GhostBelleAndHauntedMansion, CardId.EffectVeiler, CardId.MaxxC, CardId.DrollAndLockBird))
                return 800;
            if (c.IsCode(CardId.YummySnatchy)) return 20;
            if (c.IsCode(CardId.Sangan, CardId.MagiciansSouls, CardId.JesterConfit)) return 30;
            if (c.IsCode(CardId.CookyYummy, CardId.LollipoYummy, CardId.MarshmaoYummy, CardId.CupsyYummy)) return 100;
            return 50;
        }

        public override IList<ClientCard> OnSelectLinkMaterial(IList<ClientCard> cards, int min, int max)
        {
            var protectedCards = cards.Where(c => c != null && !(c.Location == CardLocation.MonsterZone && IsAceCard(c))).ToList();
            if (protectedCards.Count >= min)
            {
                var sorted = protectedCards.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }
            return cards.OrderBy(c => GetMaterialPriority(c)).Take(max).ToList();
        }

        public override IList<ClientCard> OnSelectSynchroMaterial(IList<ClientCard> cards, int sum, int min, int max)
        {
            var protectedCards = cards.Where(c => c != null && !(c.Location == CardLocation.MonsterZone && IsAceCard(c))).ToList();
            if (protectedCards.Count >= min)
            {
                var sorted = protectedCards.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }
            return base.OnSelectSynchroMaterial(cards, sum, min, max);
        }

        public _2026_YummyExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            HeuristicGuard.RegisterAceCards(BossMonsters);

            // ============================================================
            // TIER 1: Hand Traps & Reactive Disruptions (Both Turns)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCCondition);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAndJoyousSpring, AshCondition);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelleAndHauntedMansion, GhostBelleCondition);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, EffectVeilerCondition);
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

            // ============================================================
            // TIER 3: Boss Monster Quick Effects & Tag-Outs (Opponent Turn & Chains)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.BorreloadSavageDragon, BorreloadSavageEffect);
            AddExecutor(ExecutorType.Activate, CardId.HeraldOfTheArcLight, HeraldOfTheArcLightEffect);
            AddExecutor(ExecutorType.Activate, CardId.CupsyYummyWay, CupsyYummyWayEffect);
            AddExecutor(ExecutorType.Activate, CardId.CookyYummyWay, CookyYummyWayEffect);
            AddExecutor(ExecutorType.Activate, CardId.LollipoYummyWay, LollipoYummyWayEffect);
            AddExecutor(ExecutorType.Activate, CardId.SprightElf, SprightElfEffect);
            AddExecutor(ExecutorType.Activate, CardId.YummySnatchy, YummySnatchyEffect);
            AddExecutor(ExecutorType.Activate, CardId.YummySurprise, YummySurpriseEffect);

            // ============================================================
            // TIER 4: Setup Spells & Field Spell Ignition
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.YummyusmentMignon, YummyusmentMignonEffect);
            AddExecutor(ExecutorType.Activate, CardId.IllusionOfChaos, IllusionOfChaosEffect);
            AddExecutor(ExecutorType.Activate, CardId.PiriReisMap, PiriReisMapEffect);
            AddExecutor(ExecutorType.Activate, CardId.SkyStrikerMechaHornetDrones, HornetDronesEffect);
            AddExecutor(ExecutorType.Activate, CardId.MagiciansSouls, MagiciansSoulsEffect);

            // ============================================================
            // TIER 5: Monster Effects (Field / Search / On-Summon Triggers)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.MarshmaoYummy, MarshmaoYummyEffect);
            AddExecutor(ExecutorType.Activate, CardId.CupsyYummy, CupsyYummyEffect);
            AddExecutor(ExecutorType.Activate, CardId.CookyYummy, CookyYummyEffect);
            AddExecutor(ExecutorType.Activate, CardId.LollipoYummy, LollipoYummyEffect);
            AddExecutor(ExecutorType.Activate, CardId.MartialMetalMarcher, MartialMetalMarcherEffect);
            AddExecutor(ExecutorType.Activate, CardId.CupidPitch, CupidPitchEffect);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneRS, KewlTuneRSEffect);

            // ============================================================
            // TIER 6: Normal Summons (Starters)
            // ============================================================
            AddExecutor(ExecutorType.Summon, CardId.MarshmaoYummy, MarshmaoNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.CupsyYummy, CupsyNormalSummon);
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
            // TIER 8: Extra Deck Summons (Snatchy -> Synchro Lv2 -> Spright Elf)
            // ============================================================
            AddExecutor(ExecutorType.SpSummon, CardId.YummySnatchy, YummySnatchySpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CupsyYummyWay, CupsyYummyWaySpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CookyYummyWay, CookyYummyWaySpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.LollipoYummyWay, LollipoYummyWaySpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SprightElf, SprightElfSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.MartialMetalMarcher, SynchroSpSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.CupidPitch, SynchroSpSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.HeraldOfTheArcLight, HeraldSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.BorreloadSavageDragon, BorreloadSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.KewlTuneRS, SynchroSpSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.CrystronHalqifibrax, LinkSpSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.LinkSpider, LinkSpiderSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Linkuriboh, LinkuribohSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SalamangreatAlmiraj, LinkSpSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.SkyStrikerAceKagari, LinkSpSummonCheck);

            // ============================================================
            // TIER 9: Setting Traps & Cleanup
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.BookOfEclipse, BookOfEclipseEffect);
            AddExecutor(ExecutorType.Activate, CardId.YummyusmentAcroquey, YummyusmentAcroqueyEffect);
            AddExecutor(ExecutorType.Activate, CardId.MistakenArrest, MistakenArrestEffect);
            AddExecutor(ExecutorType.Activate, CardId.DimensionalBarrier, DimensionalBarrierEffect);
            AddExecutor(ExecutorType.Activate, CardId.EvenlyMatched, EvenlyMatchedEffect);

            AddExecutor(ExecutorType.SpellSet, CardId.YummySurprise, YummySurpriseSetCondition);
            AddExecutor(ExecutorType.SpellSet, CardId.DimensionalBarrier);

            AddExecutor(ExecutorType.Repos, YummyMonsterRepos);
        }

        public override bool OnSelectHand() => true;

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _normalSummonUsed = false;
            _marshmaoHandSSUsed = false;
            _marshmaoEffectUsed = false;
            _cupsHandSSUsed = false;
            _cupsySearchUsed = false;
            _cookyHandSSUsed = false;
            _cookyEffectUsed = false;
            _lollipoHandSSUsed = false;
            _lollipoEffectUsed = false;
            _mignonFieldReviveUsed = false;
            _snatchyPlaceUsed = false;
            _cupsyWaySearchUsed = false;
            _sprightElfReviveUsed = false;
            _borreloadNegateUsed = false;
            _surpriseUsed = false;
            _handTrapsUsedThisTurn = 0;
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

        private ClientCard GetBestDiscardCard()
        {
            return Bot.Hand
                .Where(c => c != null && c != Card)
                .OrderBy(c => {
                    if (c.IsCode(CardId.IllusionOfChaos)) return 5;
                    if (c.IsCode(CardId.PiriReisMap) && (Bot.LifePoints <= 4000 || Bot.GetMonsterCount() > 0)) return 10;
                    if (c.IsCode(CardId.JesterConfit) && Bot.GetMonsterCount() > 0) return 15;
                    if (c.IsCode(CardId.YummyusmentMignon) && Bot.HasInSpellZone(CardId.YummyusmentMignon)) return 18;
                    if (YummyMonsters.Contains(c.Id) && Bot.Hand.Count(h => h.Id == c.Id) > 1) return 20;
                    if (c.IsCode(CardId.TripleTacticsTalent) && Duel.Player == 1) return 22;
                    if (c.IsCode(CardId.CookyYummy) && _cookyHandSSUsed) return 25;
                    if (c.IsCode(CardId.LollipoYummy) && _lollipoHandSSUsed) return 30;
                    if (c.IsCode(CardId.EffectVeiler, CardId.GhostBelleAndHauntedMansion)) return 60;
                    if (c.IsCode(CardId.AshBlossomAndJoyousSpring, CardId.MaxxC)) return 90;
                    return 50;
                })
                .FirstOrDefault();
        }

        // ============================================================
        // HAND TRAPS & REACTIVE DISRUPTIONS
        // ============================================================

        private bool MaxxCCondition()
        {
            if (Duel.Player == 0) return false;
            if (_handTrapsUsedThisTurn >= 2) return false;
            if (DefaultMaxxC())
            {
                _handTrapsUsedThisTurn++;
                DecisionTracer.TraceActivate("MaxxC", "Opponent starting SS chain");
                return true;
            }
            return false;
        }

        private bool AshCondition()
        {
            if (Util.GetLastChainCard()?.Controller == 0) return false;
            if (_handTrapsUsedThisTurn >= 2) return false;
            if (DefaultAshBlossomAndJoyousSpring())
            {
                _handTrapsUsedThisTurn++;
                DecisionTracer.TraceActivate("AshBlossom", "Negating search/SS from deck");
                return true;
            }
            return false;
        }

        private bool GhostBelleCondition()
        {
            if (Util.GetLastChainCard()?.Controller == 0) return false;
            if (_handTrapsUsedThisTurn >= 2) return false;
            if (DefaultGhostBelleAndHauntedMansion())
            {
                _handTrapsUsedThisTurn++;
                DecisionTracer.TraceActivate("GhostBelle", "Negating GY interaction");
                return true;
            }
            return false;
        }

        private bool EffectVeilerCondition()
        {
            if (Duel.Player == 0) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
            if (Util.GetLastChainCard()?.Controller == 0) return false;
            if (_handTrapsUsedThisTurn >= 2) return false;
            if (DefaultEffectVeiler())
            {
                _handTrapsUsedThisTurn++;
                DecisionTracer.TraceActivate("EffectVeiler", "Negating opponent monster");
                return true;
            }
            return false;
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

        private bool NibiruCondition()
        {
            if (!SmartHandTrapChain()) return false;
            if (Enemy.GetMonsterCount() >= 3 || Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Attack >= 2500))
            {
                DecisionTracer.TraceActivate("Nibiru", "Wiping large board");
                return true;
            }
            return false;
        }

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
            if (Enemy.GetSpellCount() >= 1)
            {
                DecisionTracer.TraceActivate("HarpiesFeatherDuster", $"Clearing {Enemy.GetSpellCount()} backrows");
                return true;
            }
            return false;
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
                var sendTarget = GetBestDiscardCard();
                if (sendTarget != null) AI.SelectCard(sendTarget);
                return true;
            }
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1)
            {
                var enemyBoss = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled() && (c.Attack >= 2500 || OpponentHasActiveNegator()));
                if (enemyBoss != null)
                {
                    var sendTarget = GetBestDiscardCard();
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
            if (CanDealLethal() && Enemy.GetMonsterCount() >= 1) { AI.SelectOption(1); return true; } // Steal monster for game
            if (Bot.Hand.Count <= 3) { AI.SelectOption(0); return true; } // Draw 2
            AI.SelectOption(2); // Look at opponent's hand and shuffle 1
            return true;
        }

        // ============================================================
        // SETUP SPELLS & ENGINES
        // ============================================================

        private bool IllusionOfChaosEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
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
            if (Bot.LifePoints <= 4000) return false;
            if (Bot.GetRemainingCount(CardId.CupsyYummy, 3) > 0)
            {
                AI.SelectCard(CardId.CupsyYummy);
                DecisionTracer.TraceActivate("PiriReisMap", "Searching Cupsy Yummy");
                return true;
            }
            if (Bot.GetRemainingCount(CardId.MagiciansSouls, 1) > 0)
            {
                AI.SelectCard(CardId.MagiciansSouls);
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
                if (_mignonFieldReviveUsed) return false;
                if (!HasLink1OnField()) return false;
                if (IsSpecialSummonBlocked()) return false;

                var gyYummy = Bot.Graveyard
                    .Where(c => c != null && YummyMonsters.Contains(c.Id) && c.IsCanRevive())
                    .OrderByDescending(c => c.Id == CardId.MarshmaoYummy ? 100 : (c.Id == CardId.CupsyYummy ? 80 : (c.Id == CardId.CookyYummy ? 60 : 50)))
                    .FirstOrDefault();

                if (gyYummy != null)
                {
                    _mignonFieldReviveUsed = true;
                    AI.SelectCard(gyYummy);
                    DecisionTracer.TraceActivate("YummyusmentMignon", $"Reviving {gyYummy.Name} from GY");
                    return true;
                }
            }

            // Activation from Hand
            if (Card.Location == CardLocation.Hand)
            {
                if (Bot.HasInSpellZone(CardId.YummyusmentMignon)) return false;
                if (Bot.Hand.Any(c => c != null && YummyMonsters.Contains(c.Id)) && !_normalSummonUsed)
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

            // GY Recycle (Effect 1)
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

        // ============================================================
        // YUMMY MONSTER EFFECTS & SUMMONS
        // ============================================================

        private bool MarshmaoNormalSummon()
        {
            if (_normalSummonUsed) return false;
            _normalSummonUsed = true;
            DecisionTracer.TraceActivate("MarshmaoNormalSummon", "Normal Summoning Marshmao");
            return true;
        }

        private bool CupsyNormalSummon()
        {
            if (_normalSummonUsed) return false;
            _normalSummonUsed = true;
            DecisionTracer.TraceActivate("CupsyNormalSummon", "Normal Summoning Cupsy");
            return true;
        }

        private bool YummyGenericNormalSummon()
        {
            if (_normalSummonUsed) return false;
            _normalSummonUsed = true;
            return true;
        }

        private bool SanganNormalSummon()
        {
            if (_normalSummonUsed) return false;
            if (Bot.Hand.Any(c => c != null && YummyMonsters.Contains(c.Id))) return false;
            _normalSummonUsed = true;
            return true;
        }

        private bool MarshmaoSpSummon()
        {
            if (_marshmaoHandSSUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            bool validField = Bot.GetMonsterCount() == 0 ||
                Bot.GetMonsters().All(c => c == null || (c.IsFaceup() && c.HasAttribute(CardAttribute.Light) && c.HasRace(CardRace.Beast)));

            if (validField)
            {
                _marshmaoHandSSUsed = true;
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
            if (Card.Location == CardLocation.Hand)
            {
                return MarshmaoSpSummon();
            }

            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_marshmaoEffectUsed) return false;
                _marshmaoEffectUsed = true;

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
                if (_cupsHandSSUsed) return false;
                if (IsSpecialSummonBlocked()) return false;
                if (HasLink1OnField() || HasLevel2SynchroOnField())
                {
                    _cupsHandSSUsed = true;
                    DecisionTracer.TraceActivate("CupsyYummy", "Special Summoning Cupsy from hand");
                    return true;
                }
                return false;
            }

            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_cupsySearchUsed) return false;
                _cupsySearchUsed = true;

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
                if (_cookyHandSSUsed) return false;
                if (IsSpecialSummonBlocked()) return false;
                if (HasLink1OnField() || HasLevel2SynchroOnField())
                {
                    _cookyHandSSUsed = true;
                    DecisionTracer.TraceActivate("CookyYummy", "Special Summoning Cooky from hand");
                    return true;
                }
                return false;
            }

            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_cookyEffectUsed) return false;
                var target = Enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget())
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();

                if (target != null)
                {
                    _cookyEffectUsed = true;
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
                if (_lollipoHandSSUsed) return false;
                if (IsSpecialSummonBlocked()) return false;
                if (HasLink1OnField() || HasLevel2SynchroOnField())
                {
                    _lollipoHandSSUsed = true;
                    DecisionTracer.TraceActivate("LollipoYummy", "Special Summoning Lollipo from hand");
                    return true;
                }
                return false;
            }

            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_lollipoEffectUsed) return false;
                var gyTarget = Enemy.Graveyard
                    .OrderByDescending(c => c.IsMonster() ? 100 : (c.IsSpell() ? 50 : 20))
                    .FirstOrDefault();

                if (gyTarget != null)
                {
                    _lollipoEffectUsed = true;
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

            // In our turn or on summon: ALWAYS trigger to place Mignon from Deck!
            if (!_snatchyPlaceUsed && Duel.Player == 0)
            {
                _snatchyPlaceUsed = true;
                AI.SelectCard(CardId.YummyusmentMignon);
                DecisionTracer.TraceActivate("YummySnatchy", "Placing Yummyusment Mignon from Deck");
                return true;
            }

            // In opponent's turn: Quick Synchro Summon
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

            int nonAceCount = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            return nonAceCount >= 2;
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

            // Opponent's Turn: Quick Tag-Out (Wait for opponent to commit / chain, or Main Phase with targets)
            if (Duel.Player == 1)
            {
                bool opponentHasFieldPresence = Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0 || Enemy.Graveyard.Count > 0;
                bool isMainOrBattle = Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Battle || Duel.Phase == DuelPhase.Main2;
                bool isChaining = Duel.LastChainPlayer == 1;

                if ((opponentHasFieldPresence && isMainOrBattle) || isChaining)
                {
                    AI.SelectCard(new[] {
                        CardId.CookyYummy,     // Destroy 1 monster
                        CardId.LollipoYummy,   // Banish 1 GY card
                        CardId.MarshmaoYummy,  // Place Acroquey / Surprise
                        CardId.CupsyYummy      // Draw 1
                    });
                    DecisionTracer.TraceActivate("CupsyYummyWay", "Tagging out to SS 2 Yummies from GY!");
                    return true;
                }
                return false;
            }

            // Our Turn: Search 2 DISTINCT Yummies and discard 1
            if (!_cupsyWaySearchUsed && Duel.Player == 0)
            {
                _cupsyWaySearchUsed = true;
                DecisionTracer.TraceActivate("CupsyYummyWay", "Searching 2 Yummy monsters from Deck and discarding 1");
                return true;
            }

            return false;
        }

        private bool CookyYummyWayEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            if (Duel.Player == 1)
            {
                bool isMainOrBattle = Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Battle || Duel.Phase == DuelPhase.Main2;
                bool isChaining = Duel.LastChainPlayer == 1;

                if (isMainOrBattle || isChaining)
                {
                    AI.SelectCard(new[] {
                        CardId.CookyYummy,
                        CardId.LollipoYummy,
                        CardId.MarshmaoYummy,
                        CardId.CupsyYummy
                    });
                    DecisionTracer.TraceActivate("CookyYummyWay", "Tagging out to SS 2 Yummies from GY!");
                    return true;
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
                AI.SelectCard(new[] { CardId.CookyYummy, CardId.LollipoYummy, CardId.MarshmaoYummy });
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

            int nonAceBeasts = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            bool hasSynchro2 = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && YummySynchros.Contains(c.Id));

            if (hasSynchro2 && nonAceBeasts >= 1)
            {
                DecisionTracer.TraceActivate("SprightElfSpSummon", "Link Summoning Spright Elf");
                return true;
            }
            if (nonAceBeasts >= 2)
            {
                return true;
            }

            return false;
        }

        private bool SprightElfEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_sprightElfReviveUsed) return false;
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
                    _sprightElfReviveUsed = true;
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
                    _sprightElfReviveUsed = true;
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("SprightElf", $"Reviving {target.Name} from GY");
                    return true;
                }
            }

            return false;
        }

        private bool BorreloadSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            bool hasLinkInGY = Bot.Graveyard.Any(c => c != null && c.HasType(CardType.Link));
            if (!hasLinkInGY) return false;

            bool hasTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Tuner));
            bool hasNonTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.HasType(CardType.Tuner));
            return hasTuner && hasNonTuner;
        }

        private bool BorreloadSavageEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            if (Duel.LastChainPlayer == 1 && !_borreloadNegateUsed)
            {
                _borreloadNegateUsed = true;
                DecisionTracer.TraceActivate("BorreloadSavageDragon", "Omni-negating opponent activation!");
                return true;
            }

            if (Duel.CurrentChain.Count == 0)
            {
                var linkTarget = Bot.Graveyard
                    .Where(c => c != null && c.HasType(CardType.Link))
                    .OrderByDescending(c => c.LinkCount)
                    .FirstOrDefault();

                if (linkTarget != null)
                {
                    AI.SelectCard(linkTarget);
                    DecisionTracer.TraceActivate("BorreloadSavageDragon", $"Equipping Link monster {linkTarget.Name} from GY");
                    return true;
                }
            }

            return false;
        }

        private bool HeraldSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            bool hasTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Tuner));
            bool hasNonTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.HasType(CardType.Tuner));
            return hasTuner && hasNonTuner;
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

        private bool MartialMetalMarcherEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            var tuner = Bot.Graveyard.FirstOrDefault(c => c != null && c.HasType(CardType.Tuner) && c.IsCanRevive());
            if (tuner != null)
            {
                AI.SelectCard(tuner);
                return true;
            }
            return false;
        }

        private bool CupidPitchEffect()
        {
            AI.SelectCard(new[] { CardId.MarshmaoYummy, CardId.CupsyYummy, CardId.LollipoYummy, CardId.CookyYummy });
            return true;
        }

        private bool KewlTuneRSEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool YummySurpriseEffect()
        {
            if (_surpriseUsed) return false;
            if (Card.Location != CardLocation.SpellZone) return false;

            if (Duel.Player == 1 && Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 1)
            {
                var ownBeasts = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Light) && c.HasRace(CardRace.Beast)).Take(2).ToList();
                var oppCards = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                    .Where(c => c != null && !c.IsShouldNotBeTarget())
                    .OrderByDescending(c => c.IsMonster() && c.Attack >= 2000 ? 100 : (c.IsSpell() && (c.HasType(CardType.Field) || c.HasType(CardType.Continuous)) ? 80 : 30))
                    .Take(2)
                    .ToList();

                if (ownBeasts.Count >= 1 && oppCards.Count >= 1)
                {
                    _surpriseUsed = true;
                    AI.SelectOption(0);
                    AI.SelectCard(ownBeasts.Concat(oppCards).ToList());
                    DecisionTracer.TraceActivate("YummySurprise", "Bouncing cards on both fields!");
                    return true;
                }
            }

            if (IsSpecialSummonBlocked()) return false;
            var gyYummy = Bot.Graveyard.FirstOrDefault(c => c != null && YummyMonsters.Contains(c.Id) && c.IsCanRevive());
            if (gyYummy != null)
            {
                _surpriseUsed = true;
                AI.SelectOption(1);
                AI.SelectCard(gyYummy);
                return true;
            }

            return false;
        }

        private bool YummySurpriseSetCondition()
        {
            if (!Util.IsTurn1OrMain2()) return false;
            return true;
        }

        private bool LinkSpiderSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            var normalToken = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.HasType(CardType.Normal));
            return normalToken != null;
        }

        private bool LinkuribohSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            var level1 = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Level == 1 && !IsAceCard(c) && !YummyMonsters.Contains(c.Id));
            return level1 != null;
        }

        private bool LinkSpSummonCheck()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            int nonAceCount = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            return nonAceCount >= 2;
        }

        private bool SynchroSpSummonCheck()
        {
            if (IsSpecialSummonBlocked()) return false;
            int nonAceCount = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            return nonAceCount >= 2;
        }

        // ============================================================
        // SIDE TECHS & TRAPS
        // ============================================================

        private bool BookOfEclipseEffect()
        {
            if (Duel.Player != 1) return false;
            return Enemy.GetMonsters().Count(c => c != null && c.IsFaceup()) >= 2;
        }

        private bool YummyusmentAcroqueyEffect()
        {
            if (Duel.Player != 1) return false;
            return Enemy.GetMonsterCount() > 0;
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
        // OnSelectCard Override (Smart Search, Targeting & Discard)
        // ============================================================

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // HINTMSG_DISCARD = 501
            if (hint == 501)
            {
                var bestDiscard = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (c.IsCode(CardId.IllusionOfChaos)) return 5;
                    if (c.IsCode(CardId.PiriReisMap) && (Bot.LifePoints <= 4000 || Bot.GetMonsterCount() > 0)) return 10;
                    if (c.IsCode(CardId.JesterConfit)) return 15;
                    if (c.IsCode(CardId.TripleTacticsTalent) && Duel.Player == 1) return 18;
                    if (YummyMonsters.Contains(c.Id) && Bot.Hand.Count(h => h.Id == c.Id) > 1) return 20;
                    if (c.IsCode(CardId.CookyYummy) && _cookyHandSSUsed) return 25;
                    if (c.IsCode(CardId.LollipoYummy) && _lollipoHandSSUsed) return 30;
                    if (c.IsCode(CardId.EffectVeiler, CardId.GhostBelleAndHauntedMansion)) return 60;
                    if (c.IsCode(CardId.AshBlossomAndJoyousSpring, CardId.MaxxC)) return 90;
                    return 50;
                }).Take(max).ToList();
                return bestDiscard;
            }

            // HINTMSG_DESTROY = 502, HINTMSG_TARGET = 551, HINTMSG_FACEUP = 575, HINTMSG_POSCHANGE = 518
            if (hint == 502 || hint == 551 || hint == 575 || hint == 518)
            {
                var sorted = cards.OrderByDescending(c => {
                    if (c == null) return -999;
                    int score = (c.Controller == 1) ? 10000 : 0;
                    if (c.IsMonster())
                    {
                        if (c.IsFaceup() && !c.IsDisabled())
                        {
                            if (c.Attack >= 2500) score += 5000;
                            if (c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link)) score += 2000;
                        }
                        return score + c.Attack;
                    }
                    if (c.IsSpell() || c.IsTrap())
                    {
                        if (c.IsFaceup())
                        {
                            if (c.HasType(CardType.Continuous) || c.HasType(CardType.Field)) score += 6000;
                            else score += 2000;
                        }
                        else score += 1000;
                        return score;
                    }
                    return score;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // HINTMSG_REMOVE = 504 (Banishing cards, e.g. Lollipo target)
            if (hint == 504)
            {
                var sorted = cards.OrderByDescending(c => {
                    if (c == null) return -999;
                    int score = (c.Controller == 1) ? 10000 : 0;
                    if (c.Location == CardLocation.Grave)
                    {
                        if (c.IsMonster())
                        {
                            if (c.Attack >= 2000) score += 3000;
                            if (c.HasType(CardType.Effect)) score += 2000;
                        }
                        if (c.IsSpell() || c.IsTrap()) score += 1500;
                    }
                    return score;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // HINTMSG_RTOHAND = 505 / 507 (Bounce target selection, e.g. Yummy Surprise)
            if (hint == 505 || hint == 507)
            {
                var oppCards = cards.Where(c => c != null && c.Controller == 1).OrderByDescending(c => {
                    int score = 0;
                    if (c.IsMonster())
                    {
                        if (c.Attack >= 2500) score += 5000;
                        score += c.Attack;
                    }
                    if (c.IsSpell() || c.IsTrap())
                    {
                        if (c.IsFaceup() && (c.HasType(CardType.Continuous) || c.HasType(CardType.Field))) score += 6000;
                        else score += 2000;
                    }
                    return score;
                }).ToList();

                var ourCards = cards.Where(c => c != null && c.Controller == 0).OrderBy(c => {
                    if (IsAceCard(c)) return 10000;
                    if (c.Id == CardId.CupsyYummy) return 10;
                    if (c.Id == CardId.MarshmaoYummy) return 20;
                    if (c.Id == CardId.CookyYummy) return 30;
                    if (c.Id == CardId.LollipoYummy) return 40;
                    return 100;
                }).ToList();

                var combined = new List<ClientCard>();
                combined.AddRange(ourCards);
                combined.AddRange(oppCards);
                if (combined.Count >= min)
                    return combined.Take(max).ToList();
            }

            // HINTMSG_ATOHAND = 506 (Searching from Deck)
            if (hint == 506)
            {
                // When selecting 2 cards (e.g. Cupsy Way), select 2 DISTINCT Yummy monsters!
                if (max == 2)
                {
                    var selected = new List<ClientCard>();
                    var candidates = cards.Where(c => c != null).ToList();

                    // 1st: Cooky☆Yummy (if not in hand)
                    var cooky = candidates.FirstOrDefault(c => c.Id == CardId.CookyYummy && !Bot.HasInHand(CardId.CookyYummy));
                    if (cooky != null) { selected.Add(cooky); candidates.Remove(cooky); }

                    // 2nd: Lollipo☆Yummy (if not in hand)
                    var lollipo = candidates.FirstOrDefault(c => c.Id == CardId.LollipoYummy && !Bot.HasInHand(CardId.LollipoYummy));
                    if (lollipo != null && selected.Count < 2) { selected.Add(lollipo); candidates.Remove(lollipo); }

                    // 3rd: Marshmao☆Yummy
                    var marshmao = candidates.FirstOrDefault(c => c.Id == CardId.MarshmaoYummy && !Bot.HasInHand(CardId.MarshmaoYummy));
                    if (marshmao != null && selected.Count < 2) { selected.Add(marshmao); candidates.Remove(marshmao); }

                    // 4th: Cupsy☆Yummy
                    var cupsy = candidates.FirstOrDefault(c => c.Id == CardId.CupsyYummy && !Bot.HasInHand(CardId.CupsyYummy));
                    if (cupsy != null && selected.Count < 2) { selected.Add(cupsy); candidates.Remove(cupsy); }

                    // Fill remaining slots with distinct IDs if possible
                    while (selected.Count < max && candidates.Count > 0)
                    {
                        var next = candidates.FirstOrDefault(c => !selected.Any(s => s.Id == c.Id)) ?? candidates.First();
                        selected.Add(next);
                        candidates.Remove(next);
                    }

                    if (selected.Count >= min) return selected;
                }

                // Single card search (e.g. Cupsy on-summon or Piri Reis Map)
                var singleSorted = cards.OrderByDescending(c => {
                    if (c == null) return -999;
                    if (c.Id == CardId.YummySurprise && !Bot.HasInSpellZone(CardId.YummySurprise)) return 100;
                    if (c.Id == CardId.CookyYummy && !Bot.HasInHand(CardId.CookyYummy)) return 90;
                    if (c.Id == CardId.LollipoYummy && !Bot.HasInHand(CardId.LollipoYummy)) return 80;
                    if (c.Id == CardId.MarshmaoYummy && !Bot.HasInHand(CardId.MarshmaoYummy)) return 70;
                    if (c.Id == CardId.CupsyYummy && !Bot.HasInHand(CardId.CupsyYummy)) return 60;
                    if (c.Id == CardId.YummyusmentMignon && !Bot.HasInSpellZone(CardId.YummyusmentMignon)) return 50;
                    return 10;
                }).ToList();
                return singleSorted.Take(max).ToList();
            }

            // HINTMSG_TOFIELD = 510 (Snatchy placing Field Spell)
            if (hint == 510)
            {
                var mignon = cards.FirstOrDefault(c => c != null && c.Id == CardId.YummyusmentMignon);
                if (mignon != null) return new[] { mignon };
            }

            // HINTMSG_SPSUMMON = 509 (Tag-out / Revival)
            if (hint == 509)
            {
                if (Duel.Player == 1) // Opponent turn tag-out revival
                {
                    var selected = new List<ClientCard>();
                    var candidates = cards.Where(c => c != null).ToList();

                    // Cooky destroys 1 monster
                    var cooky = candidates.FirstOrDefault(c => c.Id == CardId.CookyYummy);
                    if (cooky != null) { selected.Add(cooky); candidates.Remove(cooky); }

                    // Lollipo banishes 1 GY card
                    var lollipo = candidates.FirstOrDefault(c => c.Id == CardId.LollipoYummy);
                    if (lollipo != null && selected.Count < max) { selected.Add(lollipo); candidates.Remove(lollipo); }

                    // Marshmao places S/T from deck
                    var marshmao = candidates.FirstOrDefault(c => c.Id == CardId.MarshmaoYummy);
                    if (marshmao != null && selected.Count < max) { selected.Add(marshmao); candidates.Remove(marshmao); }

                    // Synchro bosses (Cooky Way / Cupsy Way) for Spright Elf
                    var synchro = candidates.FirstOrDefault(c => YummySynchros.Contains(c.Id));
                    if (synchro != null && selected.Count < max) { selected.Add(synchro); candidates.Remove(synchro); }

                    while (selected.Count < max && candidates.Count > 0)
                    {
                        var next = candidates.First();
                        selected.Add(next);
                        candidates.Remove(next);
                    }

                    if (selected.Count >= min) return selected;
                }
            }

            // HINTMSG_ATTACKTARGET = 549 (Battle Target Selection)
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

            // Materials selection (Protect Ace cards)
            if (hint == 511 || hint == 512 || hint == 513 || hint == 533 || hint == 508 || hint == 504)
            {
                var sorted = cards.Where(c => c != null)
                    .OrderBy(c => GetMaterialPriority(c))
                    .ToList();

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

            // In MP1 or Battle Phase: Switch to Attack if we can deal damage or have high ATK
            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Battle))
            {
                if (Card.IsDefense())
                {
                    bool hasFieldSpell = Bot.HasInSpellZone(CardId.YummyusmentMignon, true);
                    int beastCount = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Light) && c.HasRace(CardRace.Beast));
                    int effectiveAtk = Card.Attack + (hasFieldSpell && Card.HasAttribute(CardAttribute.Light) && Card.HasRace(CardRace.Beast) ? beastCount * 500 : 0);

                    // If we have >= 1000 ATK or enemy has no monsters (direct attack) or our ATK beats enemy
                    if (effectiveAtk >= 1000 || Enemy.GetMonsterCount() == 0 || !Util.IsAllEnemyBetter(true))
                    {
                        return true;
                    }
                }
            }

            // In MP2: If we have low ATK and opponent has stronger monsters, switch to Defense
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main2)
            {
                if (Card.IsAttack() && Util.IsAllEnemyBetter(true) && Card.Attack < 1500)
                {
                    return true;
                }
            }

            return DefaultMonsterRepos();
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Pure utility / 0 ATK non-combat cards (hand traps etc.)
            int[] pureUtilityMonsters = {
                CardId.MaxxC, CardId.EffectVeiler,
                CardId.Sangan, CardId.JesterConfit,
                CardId.DrollAndLockBird, CardId.MulcharmyFuwalos,
                CardId.MulcharmyPurulia
            };

            if (pureUtilityMonsters.Contains(cardId) && positions.Contains(CardPosition.FaceUpDefence))
            {
                return CardPosition.FaceUpDefence;
            }

            // In Turn 1 (First turn of the duel with no battle phase), non-boss Yummies can set up safely in Defense
            if (Duel.Turn == 1 && positions.Contains(CardPosition.FaceUpDefence))
            {
                if (YummyMonsters.Contains(cardId))
                {
                    return CardPosition.FaceUpDefence;
                }
            }

            // In Turn > 1 or when we can attack / when Field Spell Mignon is active:
            // If Field Spell Mignon is on field or we are in our turn (Main 1 / Battle), summon in FaceUpAttack!
            bool hasFieldSpell = Bot.HasInSpellZone(CardId.YummyusmentMignon, true);
            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Battle))
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                {
                    return CardPosition.FaceUpAttack;
                }
            }

            if (hasFieldSpell && positions.Contains(CardPosition.FaceUpAttack))
            {
                return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }
    }
}

