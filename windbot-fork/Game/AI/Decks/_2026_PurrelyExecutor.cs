// =========================================================================================
// CARD AUDIT — 2026_Purrely (Championship Tier-1 Metagame Engine)
// | Card Name                          | Type    | OPT? | Cost      | Effect Summary                                                   |
// | :--------------------------------- | :-----: | :--: | :-------: | :--------------------------------------------------------------- |
// | Purrely (25550531)                 | Monster | No   | None      | NS/SS: Excavate top 3, add 1 Purrely S/T. Reveal Quick-Play -> Xyz|
// | Purrelyly (79933029)               | Monster | HOPT | None      | NS/SS: Search 1 non-Quick-Play Purrely. Target GY Quick-Play -> Xyz|
// | Stray Purrely Street (20212491)    | Spell   | HOPT | None      | Field: Target protection. End Phase attach Quick-Play from Deck/GY|
// | My Friend Purrely (56700100)       | Spell   | HOPT | 500 LP    | Search 1 of 3 revealed Purrely cards. Recovery 3 Quick-Plays from GY|
// | Purrely Delicious Memory (55584558)| Spell   | No   | Discard 1 | Battle protect monster. SS Level 1 Purrely from Deck. Xyz: +300 ATK/DEF per mat|
// | Purrely Pretty Memory (29599813)   | Spell   | No   | Discard 1 | +1000 LP each. SS Level 1 Purrely from Deck. Xyz: Send 1, attach opp card|
// | Purrely Happy Memory (82105704)    | Spell   | No   | Discard 1 | Effect protect 1x. SS Level 1 Purrely from Deck. Xyz: Multi-attack monsters|
// | Purrely Sleepy Memory (21347668)   | Spell   | No   | Discard 1 | Damage becomes 0. SS Level 1 Purrely from Deck. Xyz: Draw 1 in opp Standby|
// | Purrelyeap!? (82983267)            | Trap    | HOPT | None      | Quick Xyz rank up Purrely Xyz. GY: shuffle 3 Purrely from GY to deck|
// | Epurrely Plump (24434049)          | Xyz     | OPT  | None      | Attach up to 2 Spells/Traps from GYs. Quick-Play trigger: attach + banish|
// | Epurrely Noir (62592805)           | Xyz     | OPT  | Discard 1 | Bounce 1-2 opp cards. Quick-Play trigger: attach + Set Trap from Deck|
// | Epurrely Beauty (98049934)         | Xyz     | OPT  | None      | Negate 1 opp monster. Quick-Play trigger: attach + change position|
// | Epurrely Happiness (52645235)      | Xyz     | OPT  | None      | Search Purrely + halve ATK on battle. Quick-Play trigger: attach + bounce S/T|
// | Expurrely Noir (83827392)          | Xyz     | No*  | Detach 2  | Tower immunity at 5+ mats. Quick spin 1 opp card/GY to bottom of Deck|
// | Divine Arsenal AA-ZEUS (90448279)  | Xyz     | No*  | Detach 2  | Send all other cards on field to GY (Quick Effect)|
// | TYPHON Sky Crisis (93039339)       | Xyz     | OPT  | Detach 1  | 3000+ ATK floodgate + bounce 1 monster to hand|
// =========================================================================================
// ACE CARDS: Primary: Expurrely Noir / Secondary: Divine Arsenal AA-ZEUS, Epurrely Beauty, Epurrely Noir, Epurrely Plump
// COMBO STARTERS: 1. Purrelyly / Purrely 2. My Friend Purrely 3. Quick-Play Memory Spells
// WIN CONDITION: 5-7 Material Expurrely Noir (Tower Immune 2800+ DEF Fortress + Multi-Spin to Deck Bottom + Standby Draw 2-3)
// GOING 1ST END BOARD: Expurrely Noir (5+ mats, DEF pos) + My Friend + Street + Set Purrelyeap + Hand Traps
// GOING 2ND GAMEPLAN: Board Breakers -> Epurrely Happiness / Plump Multi-Attack OTK -> Downerd Magician -> AA-ZEUS Board Wipe
// =========================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using static WindBot.Game.AI.ComboRouter;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_Purrely", "2026_Purrely")]
    public class _2026_PurrelyExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int Purrelyly = 79933029;
            public const int Purrely = 25550531;

            // Hand Traps & Disruption
            public const int EffectVeiler = 97268402;
            public const int GhostBelleAndHauntedMansion = 73642296;
            public const int MaxxC = 23434538;
            public const int AshBlossomAndJoyousSpring = 14558127;
            public const int DrollAndLockBird = 94145021;
            public const int NibiruThePrimalBeing = 27204311;
            public const int ArtifactLancea = 34267821;

            // Spells - Purrely Core
            public const int PurrelyDeliciousMemory = 55584558;
            public const int PurrelyHappyMemory = 82105704;
            public const int PurrelyPrettyMemory = 29599813;
            public const int PurrelySleepyMemory = 21347668;
            public const int StrayPurrelyStreet = 20212491;
            public const int MyFriendPurrely = 56700100;

            // Spells - Generic & Board Breakers
            public const int PotOfDesires = 35261759;
            public const int CalledByTheGrave = 24224830;
            public const int TripleTacticsThrust = 35269904;
            public const int TripleTacticsTalent = 25311006;
            public const int RadiantTyphoonVision = 20508881;
            public const int HarpiesFeatherDuster = 18144507;
            public const int LightningStorm = 14532163;
            public const int ForbiddenDroplet = 24299458;

            // Traps
            public const int Purrelyeap = 82983267;
            public const int SolemnJudgment = 41420027;
            public const int RivalryOfWarlords = 90846359;
            public const int RedReboot = 23002292;
            public const int DimensionalBarrier = 83326048;

            // Extra Deck
            public const int ExpurrelyNoir = 83827392;
            public const int EpurrelyNoir = 62592805;
            public const int EpurrelyPlump = 24434049;
            public const int EpurrelyBeauty = 98049934;
            public const int EpurrelyHappiness = 52645235;
            public const int DivineArsenalAAZEUSSkyThunder = 90448279;
            public const int DownerdMagician = 72167543;
            public const int KikinagashiFucho = 27240101;
            public const int SuperStarslayerTYPHONSkyCrisis = 93039339;
            public const int SPLittleKnight = 29301450;
            public const int Linkuriboh = 41999284;
        }

        private static readonly int[] BossMonsters = {
            CardId.ExpurrelyNoir,
            CardId.DivineArsenalAAZEUSSkyThunder,
            CardId.SuperStarslayerTYPHONSkyCrisis,
            CardId.EpurrelyNoir,
            CardId.EpurrelyBeauty,
            CardId.EpurrelyPlump,
            CardId.EpurrelyHappiness,
            CardId.DownerdMagician,
            CardId.SPLittleKnight
        };

        private static readonly int[] MemorySpells = {
            CardId.PurrelyDeliciousMemory,
            CardId.PurrelySleepyMemory,
            CardId.PurrelyPrettyMemory,
            CardId.PurrelyHappyMemory
        };

        private static readonly int[] PurrelyXyz = {
            CardId.ExpurrelyNoir,
            CardId.EpurrelyNoir,
            CardId.EpurrelyPlump,
            CardId.EpurrelyBeauty,
            CardId.EpurrelyHappiness
        };

        // Turn Tracking & OPT Flags
        private bool _normalSummonUsed = false;
        private bool _myFriendSearchUsed = false;
        private bool _purrelyExcavateUsed = false;
        private bool _purrelyXyzUsed = false;
        private bool _purrelylySearchUsed = false;
        private bool _purrelylyXyzUsed = false;
        private int _plumpAttachCount = 0;
        private bool _noirBounceUsed = false;
        private bool _beautyNegateUsed = false;
        private bool _purrelyeapUsed = false;
        private bool _xyzBattledThisTurn = false;
        private int _handTrapsUsedThisTurn = 0;

        public override bool IsAceCard(ClientCard card)
        {
            return card != null && BossMonsters.Contains(card.Id);
        }

        protected override bool IsBoardStrongEnough()
        {
            var noir = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.ExpurrelyNoir));
            if (noir != null && GetOverlayCount(noir) >= 5) return true;

            int disr = CountDisruptions();
            if (disr >= 3) return true;

            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (Duel.Turn == 1 || (Duel.Player == 0 && Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0))
            {
                var noir = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.ExpurrelyNoir));
                if (noir != null && GetOverlayCount(noir) >= 5) return true;
            }

            if (CanDealLethal()) return true;

            return false;
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c))
            {
                if (c.Location == CardLocation.MonsterZone && c.IsFaceup())
                {
                    if (c.IsCode(CardId.ExpurrelyNoir)) return 10000;
                    if (c.IsCode(CardId.DivineArsenalAAZEUSSkyThunder, CardId.SPLittleKnight, CardId.SuperStarslayerTYPHONSkyCrisis)) return 9500;
                    if (c.IsCode(CardId.DownerdMagician)) return 9000;
                    if (c.Rank == 2)
                    {
                        if (GetOverlayCount(c) >= 5) return 100; // Ready to rank up into Expurrely Noir!
                        if (GetOverlayCount(c) >= 3) return 8000;
                        return 6000;
                    }
                }
                return 900;
            }
            if (c.IsCode(CardId.AshBlossomAndJoyousSpring, CardId.GhostBelleAndHauntedMansion, CardId.EffectVeiler, CardId.MaxxC, CardId.DrollAndLockBird))
                return 800;
            if (c.IsCode(CardId.Purrely, CardId.Purrelyly))
            {
                if (c.Location == CardLocation.MonsterZone && c.IsFaceup() && !c.IsDisabled())
                    return 5000;
                return 200;
            }
            return 100;
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

        public override IList<ClientCard> OnSelectXyzMaterial(IList<ClientCard> cards, int min, int max)
        {
            // If picking Rank 2 for Expurrely Noir: pick the Rank 2 with >= 5 materials!
            var rank2With5 = cards.Where(c => c != null && c.Rank == 2 && GetOverlayCount(c) >= 5).OrderByDescending(c => GetOverlayCount(c)).FirstOrDefault();
            if (rank2With5 != null)
            {
                return new[] { rank2With5 };
            }

            var protectedCards = cards.Where(c => c != null && !(c.Location == CardLocation.MonsterZone && IsAceCard(c) && c.Rank != 2)).ToList();
            if (protectedCards.Count >= min)
            {
                var sorted = protectedCards.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }
            return cards.OrderBy(c => GetMaterialPriority(c)).Take(max).ToList();
        }

        public _2026_PurrelyExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            HeuristicGuard.RegisterAceCards(BossMonsters);
            ResourcePlan.RegisterAceCards(BossMonsters);
            BaitPlanner.RegisterComboStarters(CardId.MyFriendPurrely, CardId.StrayPurrelyStreet, CardId.Purrelyly, CardId.Purrely);
            ChainAdvisor.RegisterHighValueTargets(CardId.ExpurrelyNoir, CardId.EpurrelyPlump, CardId.EpurrelyHappiness, CardId.MyFriendPurrely);

            ComboRouter.RegisterLine(new ComboLine
            {
                Name = "Purrely-Turn1-ExpurrelyNoir",
                RequiredCards = new List<int> { CardId.Purrelyly, CardId.Purrely, CardId.MyFriendPurrely },
                EndBoardScore = 95,
                Steps = new List<ComboStep>
                {
                    new() { CardId = CardId.MyFriendPurrely, ActionType = ExecutorType.Activate, Description = "Activate My Friend", Optional = true },
                    new() { CardId = CardId.StrayPurrelyStreet, ActionType = ExecutorType.Activate, Description = "Activate Street", Optional = true },
                    new() { CardId = CardId.Purrelyly, ActionType = ExecutorType.Summon, Description = "Normal Summon Purrelyly", Optional = true },
                    new() { CardId = CardId.Purrelyly, ActionType = ExecutorType.Activate, Description = "Purrelyly Search / Xyz", Optional = true },
                    new() { CardId = CardId.Purrely, ActionType = ExecutorType.Summon, Description = "Normal Summon Purrely", Optional = true },
                    new() { CardId = CardId.Purrely, ActionType = ExecutorType.Activate, Description = "Purrely Excavate / Xyz", Optional = true },
                    new() { CardId = CardId.EpurrelyPlump, ActionType = ExecutorType.Activate, Description = "Plump Attach GY Spells", Optional = true },
                    new() { CardId = CardId.PurrelyDeliciousMemory, ActionType = ExecutorType.Activate, Description = "Quick-Play Delicious to feed Plump", Optional = true },
                    new() { CardId = CardId.PurrelySleepyMemory, ActionType = ExecutorType.Activate, Description = "Quick-Play Sleepy to feed Plump", Optional = true },
                    new() { CardId = CardId.PurrelyPrettyMemory, ActionType = ExecutorType.Activate, Description = "Quick-Play Pretty to feed Plump", Optional = true },
                    new() { CardId = CardId.PurrelyHappyMemory, ActionType = ExecutorType.Activate, Description = "Quick-Play Happy to feed Plump", Optional = true },
                    new() { CardId = CardId.ExpurrelyNoir, ActionType = ExecutorType.SpSummon, Description = "Rank-up into Expurrely Noir 5+ Mats", Optional = true },
                    new() { CardId = CardId.Purrelyeap, ActionType = ExecutorType.SpellSet, Description = "Set Purrelyeap!?", Optional = true },
                }
            });

            // ============================================================
            // TIER 1: Hand Traps & Reactive Disruptions
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.ArtifactLancea, LanceaCondition);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCCondition);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAndJoyousSpring, AshCondition);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelleAndHauntedMansion, GhostBelleCondition);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, EffectVeilerCondition);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollCondition);
            AddExecutor(ExecutorType.Activate, CardId.NibiruThePrimalBeing, NibiruCondition);

            // ============================================================
            // TIER 2: Boss Monster Rank-up (5+ Mats) & Quick Effects
            // ============================================================
            AddExecutor(ExecutorType.SpSummon, CardId.ExpurrelyNoir, ExpurrelyNoirSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Purrelyeap, PurrelyeapEffect);
            AddExecutor(ExecutorType.Activate, CardId.ExpurrelyNoir, ExpurrelyNoirEffect);
            AddExecutor(ExecutorType.Activate, CardId.DivineArsenalAAZEUSSkyThunder, ZeusEffect);
            AddExecutor(ExecutorType.Activate, CardId.SuperStarslayerTYPHONSkyCrisis, TyphonEffect);
            AddExecutor(ExecutorType.Activate, CardId.EpurrelyBeauty, EpurrelyBeautyEffect);
            AddExecutor(ExecutorType.Activate, CardId.EpurrelyNoir, EpurrelyNoirEffect);
            AddExecutor(ExecutorType.Activate, CardId.EpurrelyPlump, EpurrelyPlumpEffect);
            AddExecutor(ExecutorType.Activate, CardId.EpurrelyHappiness, EpurrelyHappinessEffect);

            // ============================================================
            // TIER 3: Board Breakers (Going 2nd)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, HarpiesFeatherDusterEffect);
            AddExecutor(ExecutorType.Activate, CardId.LightningStorm, LightningStormEffect);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsThrust, TripleTacticsThrustEffect);
            AddExecutor(ExecutorType.Activate, CardId.RadiantTyphoonVision, RadiantTyphoonVisionEffect);
            AddExecutor(ExecutorType.Activate, CardId.PotOfDesires, PotOfDesiresEffect);

            // ============================================================
            // TIER 4: Setup Field & Search Spells (Continuous/Field)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.StrayPurrelyStreet, StrayPurrelyStreetEffect);
            AddExecutor(ExecutorType.Activate, CardId.MyFriendPurrely, MyFriendPurrelyEffect);

            // ============================================================
            // TIER 5: NORMAL SUMMON STARTERS FIRST! (Establish monster to reveal Quick-Play!)
            // ============================================================
            AddExecutor(ExecutorType.Summon, CardId.EffectVeiler, EffectVeilerNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.Purrelyly, PurrelylyNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.Purrely, PurrelyNormalSummon);

            // ============================================================
            // ============================================================
            // TIER 6: Monster Field Effects (Excavate, Search & Reveal Xyz)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.Purrely, PurrelyEffect);
            AddExecutor(ExecutorType.Activate, CardId.Purrelyly, PurrelylyEffect);

            // ============================================================
            // TIER 6.5: Rank 2 Xyz Summons (Overlay 2 Level 1 Purrelys into Plump/Beauty)
            // Prioritize standard Xyz over Link Summons!
            // ============================================================
            AddExecutor(ExecutorType.SpSummon, CardId.EpurrelyPlump, EpurrelyPlumpSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.EpurrelyNoir, EpurrelyNoirSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.EpurrelyBeauty, EpurrelyBeautySpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.EpurrelyHappiness, EpurrelyHappinessSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Purrelyly, PurrelySpSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.Purrely, PurrelySpSummonCheck);

            // ============================================================
            // TIER 7: Extra Deck Summons (TYPHON / Downerd / AA-ZEUS / S:P Little Knight / Linkuriboh)
            // ============================================================
            AddExecutor(ExecutorType.SpSummon, CardId.SuperStarslayerTYPHONSkyCrisis, TyphonSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DownerdMagician, DownerdMagicianSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DivineArsenalAAZEUSSkyThunder, ZeusSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Linkuriboh, LinkuribohSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Linkuriboh, LinkuribohEffect);

            // ============================================================
            // TIER 7.5: Set Purrelyeap!? & Traps IMMEDIATELY before Quick-Play Spells!
            // Must be BEFORE Tier 8 so Purrelyeap!? is protected from discard costs!
            // ============================================================
            AddExecutor(ExecutorType.SpellSet, CardId.Purrelyeap, PurrelyeapSetCondition);
            AddExecutor(ExecutorType.SpellSet, CardId.DimensionalBarrier);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnJudgment);
            AddExecutor(ExecutorType.SpellSet, CardId.RivalryOfWarlords);

            // ============================================================
            // TIER 8: Quick-Play Memory Spells (Feed Plump / SS Starter if needed)
            // Prioritize Sleepy & Pretty (Non-targeting) so empty fields can SS starter!
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.PurrelySleepyMemory, PurrelySleepyMemoryEffect);
            AddExecutor(ExecutorType.Activate, CardId.PurrelyPrettyMemory, PurrelyPrettyMemoryEffect);
            AddExecutor(ExecutorType.Activate, CardId.PurrelyDeliciousMemory, PurrelyDeliciousMemoryEffect);
            AddExecutor(ExecutorType.Activate, CardId.PurrelyHappyMemory, PurrelyHappyMemoryEffect);

            // ============================================================
            // TIER 10: Counter Traps & Setting Backrow
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, DefaultSolemnJudgment);
            AddExecutor(ExecutorType.Activate, CardId.RivalryOfWarlords, RivalryEffect);
            AddExecutor(ExecutorType.Activate, CardId.RedReboot, RedRebootEffect);
            AddExecutor(ExecutorType.Activate, CardId.DimensionalBarrier, DimensionalBarrierEffect);

            // Repositioning
            AddExecutor(ExecutorType.Repos, CustomMonsterRepos);
        }

        public override bool OnSelectHand() => true; // Always choose to go first

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _normalSummonUsed = false;
            _myFriendSearchUsed = false;
            _purrelyExcavateUsed = false;
            _purrelyXyzUsed = false;
            _purrelylySearchUsed = false;
            _purrelylyXyzUsed = false;
            _plumpAttachCount = 0;
            _noirBounceUsed = false;
            _beautyNegateUsed = false;
            _purrelyeapUsed = false;
            _xyzBattledThisTurn = false;
            _handTrapsUsedThisTurn = 0;
        }

        // ============================================================
        // HELPER METHODS
        // ============================================================

        private int GetOverlayCount(ClientCard card)
        {
            return card?.Overlays?.Count ?? 0;
        }

        private bool HasPurrelyXyzOnField()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && PurrelyXyz.Contains(c.Id));
        }

        private ClientCard GetBestDiscardCard()
        {
            return Bot.Hand
                .Where(c => c != null && c != Card)
                .OrderBy(c => {
                    // Lowest score = best to discard
                    if (c.IsCode(CardId.RadiantTyphoonVision)) return 5;
                    // Duplicate Memory Spells if we have multiple
                    if (MemorySpells.Contains(c.Id) && Bot.Hand.Count(h => h.Id == c.Id) > 1) return 10;
                    // Generic spells that Plump can attach from GY
                    if (c.IsCode(CardId.PurrelyHappyMemory)) return 15;
                    if (c.IsCode(CardId.PurrelyPrettyMemory)) return 18;
                    if (c.IsCode(CardId.PurrelySleepyMemory)) return 22;
                    if (c.IsCode(CardId.PurrelyDeliciousMemory)) return 25;
                    // Duplicate spells/traps
                    if (c.IsCode(CardId.StrayPurrelyStreet) && Bot.HasInSpellZone(CardId.StrayPurrelyStreet)) return 30;
                    if (c.IsCode(CardId.MyFriendPurrely) && Bot.HasInSpellZone(CardId.MyFriendPurrely)) return 32;
                    if (c.IsCode(CardId.Purrelyeap) && (Bot.HasInSpellZone(CardId.Purrelyeap) || Bot.Hand.Count(h => h.Id == CardId.Purrelyeap) > 1)) return 35;
                    // Duplicate monsters
                    if (c.IsCode(CardId.Purrely, CardId.Purrelyly) && Bot.Hand.Count(h => c.IsCode(h.Id)) > 1) return 40;
                    if (c.IsCode(CardId.Purrelyeap)) return 45;
                    if (c.IsCode(CardId.Purrely, CardId.Purrelyly)) return 50;
                    if (c.IsCode(CardId.ArtifactLancea, CardId.DrollAndLockBird)) return 60;
                    if (c.IsCode(CardId.GhostBelleAndHauntedMansion, CardId.EffectVeiler)) return 75;
                    if (c.IsCode(CardId.AshBlossomAndJoyousSpring, CardId.MaxxC)) return 95;
                    if (c.IsCode(CardId.MyFriendPurrely, CardId.StrayPurrelyStreet)) return 85;
                    return 50;
                })
                .FirstOrDefault();
        }

        private bool OpponentHasActiveNegator()
        {
            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled() &&
                (c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link) || c.Attack >= 2500));
        }

        // ============================================================
        // HAND TRAP IMPLEMENTATIONS
        // ============================================================

        private bool LanceaCondition()
        {
            if (Duel.Player != 1) return false;
            // Activate floodgate in Draw or Standby Phase on opponent's turn to prevent ALL banishing!
            if (Duel.Phase == DuelPhase.Draw || Duel.Phase == DuelPhase.Standby || Duel.Phase == DuelPhase.Main1)
            {
                DecisionTracer.TraceActivate("ArtifactLancea", "Blocking opponent banishing for entire turn");
                return true;
            }
            return false;
        }

        private bool MaxxCCondition()
        {
            if (Duel.Player == 0) return false;
            if (Duel.Phase == DuelPhase.Draw || Duel.Phase == DuelPhase.Standby) return false;
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

            var lastCard = Util.GetLastChainCard();
            if (lastCard == null || lastCard.Controller != 1) return false;

            // Never ash Upstart Goblin, Macro Cosmos, or Danger! hand effects
            int[] ignoreList = { 70368879, 30241314, 60600126 };
            if (lastCard.IsCode(ignoreList)) return false;
            if (lastCard.HasSetcode(0x11e) && lastCard.Location == CardLocation.Hand) return false;

            _handTrapsUsedThisTurn++;
            DecisionTracer.TraceActivate("AshBlossom", $"Negating opponent effect {lastCard.Name}");
            return true;
        }

        private bool GhostBelleCondition()
        {
            if (Util.GetLastChainCard()?.Controller == 0) return false;
            if (_handTrapsUsedThisTurn >= 2) return false;

            var lastCard = Util.GetLastChainCard();
            if (lastCard == null || lastCard.Controller != 1) return false;

            _handTrapsUsedThisTurn++;
            DecisionTracer.TraceActivate("GhostBelle", $"Negating opponent GY interaction {lastCard.Name}");
            return true;
        }

        private bool EffectVeilerCondition()
        {
            if (Duel.Player == 0) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
            if (_handTrapsUsedThisTurn >= 2) return false;

            // 1. Reactive chain: Negate active monster effect on field
            if (Duel.LastChainPlayer == 1)
            {
                var chainCard = Util.GetLastChainCard();
                if (chainCard != null && chainCard.Controller == 1 && chainCard.Location == CardLocation.MonsterZone && !chainCard.IsDisabled() && !chainCard.IsShouldNotBeTarget())
                {
                    _handTrapsUsedThisTurn++;
                    AI.SelectCard(chainCard);
                    DecisionTracer.TraceActivate("EffectVeiler", $"Chaining Effect Veiler negation to {chainCard.Name}");
                    return true;
                }
            }

            if (DefaultEffectVeiler())
            {
                _handTrapsUsedThisTurn++;
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

        private bool NibiruCondition()
        {
            return DefaultNibiru();
        }

        // ============================================================
        // BOARD BREAKERS & STAPLES
        // ============================================================

        private bool HarpiesFeatherDusterEffect()
        {
            if (Duel.Player != 0) return false;
            if (Enemy.GetSpellCount() >= 1)
            {
                DecisionTracer.TraceActivate("HarpiesFeatherDuster", $"Clearing {Enemy.GetSpellCount()} enemy backrows");
                return true;
            }
            return false;
        }

        private bool LightningStormEffect()
        {
            if (Duel.Player != 0) return false;
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup()) || Bot.GetSpells().Any(c => c != null && c.IsFaceup()))
                return false;

            if (Enemy.GetSpellCount() >= 2)
            {
                AI.SelectOption(1); // Destroy Spells
                DecisionTracer.TraceActivate("LightningStorm", "Destroying enemy spells/traps");
                return true;
            }
            if (Enemy.GetMonsterCount() >= 1)
            {
                AI.SelectOption(0); // Destroy Monsters
                DecisionTracer.TraceActivate("LightningStorm", "Destroying enemy attack position monsters");
                return true;
            }
            if (Enemy.GetSpellCount() > 0)
            {
                AI.SelectOption(1);
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
                DecisionTracer.TraceActivate("ForbiddenDroplet", "Chaining negation to opponent");
                return true;
            }
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1)
            {
                var enemyBoss = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled() && (c.Attack >= 2500 || OpponentHasActiveNegator()));
                if (enemyBoss != null)
                {
                    var sendTarget = GetBestDiscardCard();
                    if (sendTarget != null)
                    {
                        AI.SelectCard(sendTarget);
                        DecisionTracer.TraceActivate("ForbiddenDroplet", $"Negating enemy boss {enemyBoss.Name}");
                        return true;
                    }
                }
            }
            return false;
        }

        private bool TripleTacticsTalentEffect()
        {
            if (Duel.Player != 0) return false;
            if (Enemy.GetMonsterCount() >= 1 && CanDealLethal())
            {
                AI.SelectOption(1); // Take control
                return true;
            }
            if (Bot.Hand.Count <= 3)
            {
                AI.SelectOption(0); // Draw 2
                return true;
            }
            AI.SelectOption(2); // Look at hand
            return true;
        }

        private bool TripleTacticsThrustEffect()
        {
            if (Duel.Player != 0) return false;
            AI.SelectCard(new[] {
                CardId.MyFriendPurrely,
                CardId.PurrelyDeliciousMemory,
                CardId.PurrelySleepyMemory,
                CardId.Purrelyeap
            });
            return true;
        }

        private bool RadiantTyphoonVisionEffect()
        {
            if (Duel.Player != 0) return false;
            var target = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && (c.HasType(CardType.Field) || c.HasType(CardType.Continuous) || c.IsTrap())) ??
                         Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup()) ??
                         Enemy.GetSpells().FirstOrDefault(c => c != null);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool PotOfDesiresEffect()
        {
            if (Duel.Player != 0) return false;
            bool hasStarter = Bot.Hand.Any(c => c != null && c.IsCode(CardId.Purrely, CardId.Purrelyly, CardId.MyFriendPurrely));
            if (!hasStarter || Bot.Hand.Count <= 3)
            {
                DecisionTracer.TraceActivate("PotOfDesires", "Drawing 2 cards to unbrick hand");
                return true;
            }
            return false;
        }

        // ============================================================
        // CORE PURRELY SPELLS
        // ============================================================

        private bool StrayPurrelyStreetEffect()
        {
            // End Phase Attach Effect (Description 1)
            if (ActivateDescription == Util.GetStringId(CardId.StrayPurrelyStreet, 1))
            {
                var targetXyz = Bot.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && PurrelyXyz.Contains(c.Id))
                    .OrderByDescending(c => c.IsCode(CardId.ExpurrelyNoir) ? 1000 : GetOverlayCount(c))
                    .FirstOrDefault();

                if (targetXyz != null)
                {
                    AI.SelectCard(targetXyz);
                    DecisionTracer.TraceActivate("StrayPurrelyStreet", $"Attaching Memory from Deck/GY to {targetXyz.Name}");
                    return true;
                }
                return false;
            }

            // Normal Activation from Hand
            if (Card.Location == CardLocation.Hand)
            {
                if (Bot.HasInSpellZone(CardId.StrayPurrelyStreet)) return false;
                if (Duel.Player != 0) return false;
                DecisionTracer.TraceActivate("StrayPurrelyStreet", "Activating Field Spell for targeting protection");
                return true;
            }

            // Float Trigger (Description 0)
            if (ActivateDescription == Util.GetStringId(CardId.StrayPurrelyStreet, 0) || ActivateDescription == -1)
            {
                if (IsSpecialSummonBlocked()) return false;
                AI.SelectCard(new[] { CardId.Purrelyly, CardId.Purrely });
                return true;
            }

            return false;
        }

        private bool MyFriendPurrelyEffect()
        {
            // Effect 0: Ignition Search
            if (ActivateDescription == Util.GetStringId(CardId.MyFriendPurrely, 0) || (Card.Location == CardLocation.SpellZone && ActivateDescription == -1))
            {
                if (_myFriendSearchUsed) return false;
                if (Bot.LifePoints <= 500) return false;
                if (Bot.Deck.Count <= 4) return false;
                _myFriendSearchUsed = true;

                // Priority: Delicious > Sleepy > Pretty
                if (!Bot.Hand.Any(h => MemorySpells.Contains(h.Id)))
                {
                    AI.SelectCard(new[] {
                        CardId.PurrelyDeliciousMemory,
                        CardId.PurrelySleepyMemory,
                        CardId.PurrelyPrettyMemory
                    });
                }
                else
                {
                    AI.SelectCard(new[] {
                        CardId.PurrelyDeliciousMemory,
                        CardId.Purrelyly,
                        CardId.Purrely,
                        CardId.PurrelySleepyMemory,
                        CardId.PurrelyPrettyMemory
                    });
                }
                DecisionTracer.TraceActivate("MyFriendPurrely", "Searching Purrely core card");
                return true;
            }

            // Normal Activation from Hand
            if (Card.Location == CardLocation.Hand)
            {
                if (Bot.HasInSpellZone(CardId.MyFriendPurrely)) return false;
                if (Duel.Player != 0) return false;
                DecisionTracer.TraceActivate("MyFriendPurrely", "Activating Continuous Spell");
                return true;
            }

            // Effect 1: GY Float Recovery
            if (ActivateDescription == Util.GetStringId(CardId.MyFriendPurrely, 1))
            {
                AI.SelectCard(new[] {
                    CardId.PurrelyDeliciousMemory,
                    CardId.PurrelySleepyMemory,
                    CardId.PurrelyPrettyMemory,
                    CardId.PurrelyHappyMemory
                });
                DecisionTracer.TraceActivate("MyFriendPurrely", "Recovering 3 Quick-Play Memories from GY");
                return true;
            }

            return false;
        }

        // ============================================================
        // PURRELY MONSTER SUMMONS & EFFECTS
        // ============================================================

        private bool EffectVeilerNormalSummon()
        {
            if (_normalSummonUsed) return false;
            if (Duel.Player != 0) return false;
            // Out to Secret Village of the Spellcasters! If opponent has Secret Village face-up
            // and we have no Spellcaster, normal summon Effect Veiler so we control a Spellcaster,
            // immediately breaking Secret Village's lockdown on all of our Spells!
            if (Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(68462976)) && !Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasRace(CardRace.SpellCaster)))
            {
                _normalSummonUsed = true;
                DecisionTracer.TraceActivate("EffectVeilerNormalSummon", "Normal Summoning Effect Veiler to shatter Secret Village lock!");
                return true;
            }
            return false;
        }

        private bool PurrelylyNormalSummon()
        {
            if (_normalSummonUsed) return false;
            if (Duel.Player != 0) return false;
            _normalSummonUsed = true;
            DecisionTracer.TraceActivate("PurrelylyNormalSummon", "Normal Summoning Purrelyly (Starter #1)");
            return true;
        }

        private bool PurrelyNormalSummon()
        {
            if (_normalSummonUsed) return false;
            if (Duel.Player != 0) return false;
            _normalSummonUsed = true;
            DecisionTracer.TraceActivate("PurrelyNormalSummon", "Normal Summoning Purrely (Starter #2)");
            return true;
        }

        private bool PurrelySpSummonCheck()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool PurrelylyEffect()
        {
            // Effect 0: Search non-Quick-Play Purrely card from Deck (ON-SUMMON TRIGGER ONLY)
            if (ActivateDescription == Util.GetStringId(CardId.Purrelyly, 0) || (Duel.CurrentChain.Count > 0 && !_purrelylySearchUsed))
            {
                if (_purrelylySearchUsed) return false;
                _purrelylySearchUsed = true;

                // Priority: My Friend > Purrelyeap > Stray Street > Purrely
                if (!Bot.HasInSpellZone(CardId.MyFriendPurrely) && !Bot.HasInHand(CardId.MyFriendPurrely) && Bot.GetRemainingCount(CardId.MyFriendPurrely, 3) > 0)
                    AI.SelectCard(CardId.MyFriendPurrely);
                else if (!Bot.HasInSpellZone(CardId.Purrelyeap) && !Bot.HasInHand(CardId.Purrelyeap) && Bot.GetRemainingCount(CardId.Purrelyeap, 2) > 0)
                    AI.SelectCard(CardId.Purrelyeap);
                else if (!Bot.HasInSpellZone(CardId.StrayPurrelyStreet) && !Bot.HasInHand(CardId.StrayPurrelyStreet) && Bot.GetRemainingCount(CardId.StrayPurrelyStreet, 3) > 0)
                    AI.SelectCard(CardId.StrayPurrelyStreet);
                else
                    AI.SelectCard(CardId.Purrely);

                DecisionTracer.TraceActivate("Purrelyly", "Searching non-Quick-Play Purrely card");
                return true;
            }

            // Effect 1: Target GY Quick-Play -> Xyz Summon (IGNITION EFFECT IN IDLE)
            if (ActivateDescription == Util.GetStringId(CardId.Purrelyly, 1) || (Duel.CurrentChain.Count == 0 && ActivateDescription == -1))
            {
                if (_purrelylyXyzUsed) return false;
                if (IsSpecialSummonBlocked()) return false;

                var memoryInGY = Bot.Graveyard
                    .Where(c => c != null && MemorySpells.Contains(c.Id))
                    .OrderBy(c => {
                        // Plump is our #1 target for climbing to 5-material Noir!
                        if (c.Id == CardId.PurrelyDeliciousMemory && Bot.ExtraDeck.Any(e => e.IsCode(CardId.EpurrelyPlump))) return 1;
                        if (c.Id == CardId.PurrelySleepyMemory && Bot.ExtraDeck.Any(e => e.IsCode(CardId.EpurrelyNoir))) return 2;
                        if (c.Id == CardId.PurrelyPrettyMemory && Bot.ExtraDeck.Any(e => e.IsCode(CardId.EpurrelyBeauty))) return 3;
                        return 4;
                    })
                    .FirstOrDefault();

                if (memoryInGY != null)
                {
                    _purrelylyXyzUsed = true;
                    AI.SelectCard(memoryInGY);
                    DecisionTracer.TraceActivate("Purrelyly", $"Targeting {memoryInGY.Name} in GY to Xyz Summon Rank 2");
                    return true;
                }
            }

            return false;
        }

        private bool PurrelyEffect()
        {
            // Effect 0: Excavate top 3 cards (ON-SUMMON TRIGGER ONLY)
            if (ActivateDescription == Util.GetStringId(CardId.Purrely, 0) || (Duel.CurrentChain.Count > 0 && !_purrelyExcavateUsed))
            {
                if (_purrelyExcavateUsed) return false;
                if (Bot.Deck.Count <= 3) return false;
                _purrelyExcavateUsed = true;
                AI.SelectCard(new[] {
                    CardId.MyFriendPurrely,
                    CardId.StrayPurrelyStreet,
                    CardId.PurrelyDeliciousMemory,
                    CardId.PurrelySleepyMemory,
                    CardId.PurrelyPrettyMemory,
                    CardId.PurrelyHappyMemory,
                    CardId.Purrelyeap
                });
                DecisionTracer.TraceActivate("Purrely", "Excavating top 3 cards for Purrely S/T");
                return true;
            }

            // Effect 1: Reveal Quick-Play in Hand -> Xyz Summon (IGNITION EFFECT IN IDLE)
            if (ActivateDescription == Util.GetStringId(CardId.Purrely, 1) || (Duel.CurrentChain.Count == 0 && ActivateDescription == -1))
            {
                if (_purrelyXyzUsed) return false;
                if (IsSpecialSummonBlocked()) return false;

                // Priority: Delicious (Plump) > Sleepy (Noir) > Pretty (Beauty) > Happy (Happiness)
                var memoryInHand = Bot.Hand
                    .Where(c => c != null && MemorySpells.Contains(c.Id))
                    .OrderBy(c => {
                        if (c.Id == CardId.PurrelyDeliciousMemory && Bot.ExtraDeck.Any(e => e.IsCode(CardId.EpurrelyPlump))) return 1;
                        if (c.Id == CardId.PurrelySleepyMemory && Bot.ExtraDeck.Any(e => e.IsCode(CardId.EpurrelyNoir))) return 2;
                        if (c.Id == CardId.PurrelyPrettyMemory && Bot.ExtraDeck.Any(e => e.IsCode(CardId.EpurrelyBeauty))) return 3;
                        return 4;
                    })
                    .FirstOrDefault();

                if (memoryInHand != null)
                {
                    _purrelyXyzUsed = true;
                    AI.SelectCard(memoryInHand);
                    DecisionTracer.TraceActivate("Purrely", $"Revealing {memoryInHand.Name} to Xyz Summon Rank 2");
                    return true;
                }
            }

            return false;
        }

        // ============================================================
        // QUICK-PLAY MEMORY SPELLS ACTIVATION
        // ============================================================

        private bool ShouldActivateMemoryInHand()
        {
            if (Duel.Player != 0) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2 && Duel.Phase != DuelPhase.Battle) return false;
            if (Duel.CurrentChain.Count > 0) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.Deck.Count <= 4) return false; // Prevent deck out!

            // STOP EXTENDING: If we ALREADY control Expurrely Noir with 5+ materials, we are done!
            var noir = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.ExpurrelyNoir));
            if (noir != null && GetOverlayCount(noir) >= 5) return false;

            // RULE 1: If we have a Normal Summonable starter in hand and haven't Normal Summoned yet, Normal Summon first!
            if (!_normalSummonUsed && Bot.Hand.Any(h => h.IsCode(CardId.Purrely, CardId.Purrelyly)))
                return false;

            // RULE 2: If we have a Rank 2 Purrely (e.g. Plump) on field with < 5 materials: attach and feed!
            var rank2 = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Rank == 2 && PurrelyXyz.Contains(c.Id));
            if (rank2 != null && GetOverlayCount(rank2) < 5)
            {
                return true;
            }

            // RULE 3: If we have Purrelyly on field and NO Quick-Play in GY, activate one so Purrelyly can target it!
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.Purrelyly)) &&
                !Bot.Graveyard.Any(c => c != null && MemorySpells.Contains(c.Id)))
            {
                return true;
            }

            // RULE 4: If we have NO Purrely monsters on field, activate to SS Purrelyly/Purrely from Deck!
            bool hasPurrelyMon = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.IsCode(CardId.Purrely, CardId.Purrelyly) || PurrelyXyz.Contains(c.Id)));
            if (!hasPurrelyMon && Bot.Hand.Count >= 2)
            {
                return true;
            }

            return false;
        }

        private bool PurrelyDeliciousMemoryEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (!ShouldActivateMemoryInHand()) return false;

                // Target OUR monster first to give it battle immunity, then enemy monsters!
                var target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup()) ??
                             Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup());
                if (target == null) return false;

                AI.SelectCard(target);
                DecisionTracer.TraceActivate("PurrelyDeliciousMemory", $"Targeting {target.Name} for battle immunity & feeding Plump / SS Purrely");
                return true;
            }
            return false;
        }

        private bool PurrelySleepyMemoryEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (!ShouldActivateMemoryInHand()) return false;

                DecisionTracer.TraceActivate("PurrelySleepyMemory", "Activating Sleepy Memory to SS Purrely from Deck / feed Plump");
                return true;
            }
            return false;
        }

        private bool PurrelyPrettyMemoryEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (!ShouldActivateMemoryInHand()) return false;

                DecisionTracer.TraceActivate("PurrelyPrettyMemory", "Activating Pretty Memory to SS Purrely from Deck / feed Plump");
                return true;
            }
            return false;
        }

        private bool PurrelyHappyMemoryEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (!ShouldActivateMemoryInHand()) return false;

                // Target OUR card first to give it effect destruction immunity!
                var target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup()) ??
                             Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup()) ??
                             Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup()) ??
                             Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());
                if (target == null) return false;

                AI.SelectCard(target);
                DecisionTracer.TraceActivate("PurrelyHappyMemory", $"Targeting {target.Name} for effect destruction immunity");
                return true;
            }
            return false;
        }

        // ============================================================
        // EXTRA DECK MONSTERS — EFFECTS & SUMMONS
        // ============================================================

        private bool ExpurrelyNoirSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var rank2With5Mats = Bot.GetMonsters()
                .FirstOrDefault(c => c != null && c.IsFaceup() && c.HasType(CardType.Xyz) && c.Rank == 2 && GetOverlayCount(c) >= 5);

            return rank2With5Mats != null;
        }

        private bool TyphonSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            // NEVER summon Typhon if we control ANY Purrely Xyz monster!
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && PurrelyXyz.Contains(c.Id))) return false;
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.ExpurrelyNoir))) return false;

            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= 3000);
        }

        private bool TyphonEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (GetOverlayCount(Card) < 1) return false;

            var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Attack >= 2500) ??
                         Enemy.GetMonsters().FirstOrDefault(c => c != null);
            if (target != null)
            {
                AI.SelectCard(target);
                DecisionTracer.TraceActivate("TYPHON", $"Bouncing enemy monster {target.Name} to hand");
                return true;
            }
            return false;
        }

        private bool DownerdMagicianSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Duel.Phase != DuelPhase.Main2) return false;
            // CRITICAL: An Xyz monster MUST have battled this turn to summon Zeus in Main 2!
            if (!_xyzBattledThisTurn) return false;
            // CRITICAL: Never summon Downerd if we control Expurrely Noir!
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.ExpurrelyNoir))) return false;

            // If opponent controls Secret Village of the Spellcasters, Downerd Magician is a Spellcaster!
            // Overlaying Downerd Magician over any Rank 2 instantly breaks Secret Village's spell lock!
            if (Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(68462976)))
            {
                var rank2Village = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Rank == 2);
                if (rank2Village != null) return true;
            }

            var rank2 = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Rank == 2);
            if (rank2 == null || GetOverlayCount(rank2) >= 4) return false;
            if (!Bot.ExtraDeck.Any(e => e.IsCode(CardId.DivineArsenalAAZEUSSkyThunder))) return false;
            if (Enemy.GetMonsterCount() + Enemy.GetSpellCount() < 2) return false;

            return true;
        }

        private bool ZeusSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Duel.Phase != DuelPhase.Main2) return false;
            if (!_xyzBattledThisTurn) return false;
            var noir = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.ExpurrelyNoir));
            if (noir != null && GetOverlayCount(noir) >= 5) return false;

            // Always summon Zeus if opponent controls Avramax or 2+ cards!
            if (Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(21887175))) return true;
            return Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 2;
        }

        private bool ZeusEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (GetOverlayCount(Card) < 2) return false;
            if (Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 2 || Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.Attack >= 2500 || c.IsCode(21887175))))
            {
                DecisionTracer.TraceActivate("AA-ZEUS", "Wiping entire field with Zeus Quick Effect!");
                return true;
            }
            return false;
        }

        private bool SPLittleKnightSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            // CRITICAL: NEVER sacrifice ANY Purrely Xyz monster or Expurrely Noir for S:P Little Knight!
            // Purrely Xyz monsters are our core win condition and stepping stones to Noir!
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (PurrelyXyz.Contains(c.Id) || c.IsCode(CardId.ExpurrelyNoir)))) return false;

            bool oppHasFloodgate = Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && 
                (c.IsCode(68462976, 27541563, 61740673, 99745551, 82732705) || c.HasType(CardType.Continuous) || c.HasType(CardType.Field)));
            bool oppHasBoss = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.IsCode(21887175) || c.Attack >= 2500));

            // Only allow using NON-Xyz monsters (e.g. Linkuriboh, Effect Veiler, or babies AFTER their effects are used)
            int eligibleMaterials = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !PurrelyXyz.Contains(c.Id) && !c.IsCode(CardId.ExpurrelyNoir));
            return eligibleMaterials >= 2 && (oppHasFloodgate || oppHasBoss);
        }

        private bool SPLittleKnightEffect()
        {
            // Effect 0: On Link Summon banish targeting (out Secret Village / Protocol / Avramax!)
            var target = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(68462976)) ??
                         Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && (c.IsCode(27541563, 61740673, 48680970, 48770333, 66399653))) ??
                         Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget() && (c.IsCode(21887175) || c.Attack >= 2500 || c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link))) ??
                         Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup()) ??
                         Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget()) ??
                         Enemy.Graveyard.FirstOrDefault(c => c != null && (c.IsCode(46986414, 89631139) || c.Attack >= 2000));

            if (target != null)
            {
                AI.SelectCard(target);
                DecisionTracer.TraceActivate("SPLittleKnight", $"Banish targeting {target.Name}");
                return true;
            }

            // Effect 1: Quick Effect banish self + 1 monster
            if (Duel.LastChainPlayer == 1)
            {
                var enemyMon = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget());
                if (enemyMon != null)
                {
                    AI.SelectCard(Card);
                    AI.SelectNextCard(enemyMon);
                    DecisionTracer.TraceActivate("SPLittleKnight", $"Quick effect banishing self and {enemyMon.Name}");
                    return true;
                }
            }
            return false;
        }

        private bool LinkuribohSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            // Never summon Linkuriboh if we have Expurrely Noir or an established Purrely Xyz
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.IsCode(CardId.ExpurrelyNoir) || (PurrelyXyz.Contains(c.Id) && GetOverlayCount(c) >= 2)))) return false;

            // Preferred: Use EffectVeiler or non-Purrely Level 1 (e.g. from normal summon to clear Secret Village)
            var nonPurrelyLv1 = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Level == 1 && !c.IsCode(CardId.Purrely, CardId.Purrelyly));
            if (nonPurrelyLv1 != null)
            {
                AI.SelectCard(nonPurrelyLv1);
                return true;
            }

            // DO NOT sacrifice Purrely or Purrelyly if we can overlay or have Purrelyeap!
            bool hasMemory = Bot.Hand.Any(c => c != null && MemorySpells.Contains(c.Id));
            bool hasPurrelyeap = Bot.Hand.Any(c => c != null && c.IsCode(CardId.Purrelyeap)) || Bot.GetSpells().Any(c => c != null && c.IsCode(CardId.Purrelyeap));
            if (hasMemory || hasPurrelyeap) return false;

            // Only allow if Purrely/Purrelyly is already used, we have at least 2 monsters, and we are preparing for S:P Little Knight
            var purrelyMon = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Level == 1 && c.IsCode(CardId.Purrely, CardId.Purrelyly));
            if (purrelyMon != null && Bot.GetMonsterCount() >= 2 && Enemy.GetMonsterCount() > 0)
            {
                AI.SelectCard(purrelyMon);
                return true;
            }

            return false;
        }

        private bool LinkuribohEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (Duel.Phase == DuelPhase.Battle && Duel.Player == 1)
                {
                    DecisionTracer.TraceActivate("Linkuriboh", "Tributing self to reduce attack to 0");
                    return true;
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                // CRITICAL FIX: NEVER tribute Purrely or Purrelyly! Only tribute non-Purrely Level 1 monsters (e.g. Effect Veiler, tokens)
                var lv1 = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Level == 1 && !IsAceCard(c) && !c.IsCode(CardId.Purrely, CardId.Purrelyly));
                if (lv1 != null && (Duel.Player == 1 || Bot.GetMonsterCount() <= 2))
                {
                    AI.SelectCard(lv1);
                    DecisionTracer.TraceActivate("Linkuriboh", $"Reviving Linkuriboh by tributing {lv1.Name}");
                    return true;
                }
            }
            return false;
        }

        private bool ExpurrelyNoirEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            int mats = GetOverlayCount(Card);
            if (mats < 2) return false;

            // Prevent infinite self-chaining
            if (Duel.LastChainPlayer == 0 && Util.GetLastChainCard()?.Id == CardId.ExpurrelyNoir) return false;

            // Check if opponent has Eternal Soul active -> Dark Magicians are immune!
            bool eternalSoulActive = Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(48680970));

            // Helper to get best opponent target on field or GY
            ClientCard GetBestOpponentTarget()
            {
                // #1: Eternal Soul / True Light -> spinning to bottom of deck triggers wipe of all opp monsters!
                var wipeSpell = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(48680970, 48770333) && !c.IsShouldNotBeTarget());
                if (wipeSpell != null) return wipeSpell;

                // #2: Face-up continuous spells / traps / floodgates (Dark Magical Circle, Skill Drain, etc.)
                var floodgate = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && (c.HasType(CardType.Continuous) || c.HasType(CardType.Field) || c.IsTrap()) && !c.IsShouldNotBeTarget());
                if (floodgate != null) return floodgate;

                // #3: Extra Deck Boss Monsters (Fusion, Synchro, Xyz, Link)
                var extraBoss = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget() &&
                            (!eternalSoulActive || !c.IsCode(46986414)) &&
                            (c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link)));
                if (extraBoss != null) return extraBoss;

                // #4: High-ATK threat monsters (ATK >= 2500)
                var bigThreat = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget() && (!eternalSoulActive || !c.IsCode(46986414)))
                            .OrderByDescending(c => c.Attack).FirstOrDefault(c => c.Attack >= 2500);
                if (bigThreat != null) return bigThreat;

                // #5: Any face-up monster (that is not immune)
                var anyFaceupMon = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget() && (!eternalSoulActive || !c.IsCode(46986414)))
                            .OrderByDescending(c => c.Attack).FirstOrDefault();
                if (anyFaceupMon != null) return anyFaceupMon;

                // #6: Opponent GY boss / recurrer (e.g. Dark Magician in GY, ABC piece, or ATK >= 2000)
                var gyThreat = Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && (c.IsCode(46986414) || c.Attack >= 2000));
                if (gyThreat != null) return gyThreat;

                // #7: Face-down Spells/Traps
                var setSpell = Enemy.GetSpells().FirstOrDefault(c => c != null && !c.IsShouldNotBeTarget());
                if (setSpell != null) return setSpell;

                return Enemy.GetMonsters().FirstOrDefault(c => c != null && !c.IsShouldNotBeTarget());
            }

            // OPPONENT'S TURN (Duel.Player == 1): CHAMPIONSHIP QUICK DISRUPTIONS
            if (Duel.Player == 1)
            {
                // 1. Reactive Chain: Opponent activated a card or effect
                if (Duel.LastChainPlayer == 1)
                {
                    var chainCard = Util.GetLastChainCard();
                    ClientCard target = null;
                    // Expurrely Noir can ONLY target cards opponent controls on field or in GY (NOT in hand!)
                    if (chainCard != null && chainCard.Controller == 1 && chainCard.Location != CardLocation.Hand && !chainCard.IsShouldNotBeTarget())
                    {
                        if (!eternalSoulActive || !chainCard.IsCode(46986414))
                            target = chainCard;
                    }
                    if (target == null)
                    {
                        target = GetBestOpponentTarget();
                    }

                    if (target != null)
                    {
                        bool isBoardWipeTarget = target.IsCode(48680970, 48770333);
                        bool inBattleDanger = target.IsMonster() && (target.Attack >= Card.Attack && target.Attack >= Card.Defense);
                        bool isBossThreat = target.IsMonster() && (target.Attack >= 2500 || target.HasType(CardType.Fusion) || target.HasType(CardType.Synchro) || target.HasType(CardType.Xyz) || target.HasType(CardType.Link));
                        bool isKeyBackrow = target.IsCode(47222536, 66399653, 53936268, 61740673, 68462976, 27541563) || (target.HasType(CardType.Field) || target.HasType(CardType.Continuous) || (target.IsFaceup() && target.IsTrap()));

                        // If mats >= 7: ALWAYS spin! (Keeps 5+ mats for full tower immunity!)
                        // If mats is 5 or 6: PROTECT TOWER IMMUNITY! NEVER drop below 5 mats unless battle danger or board wipe!
                        // If mats < 5: we DO NOT have tower immunity anyway -> spin any threat to disrupt opponent!
                        bool shouldSpin = (mats >= 7 && (isBossThreat || isKeyBackrow || target.IsMonster())) ||
                                          isBoardWipeTarget || inBattleDanger ||
                                          (mats < 5 && (isBossThreat || isKeyBackrow || target.IsMonster()));

                        if (shouldSpin)
                        {
                            AI.SelectCard(target);
                            DecisionTracer.TraceActivate("ExpurrelyNoir", $"Chaining Quick spin to {target.Name} (Mats: {mats})");
                            return true;
                        }
                    }
                }

                // 2. Battle Phase Protection: Spin any attacker threatening Noir or lethal!
                if (Duel.Phase == DuelPhase.Battle)
                {
                    var battleThreat = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsAttack() && !c.IsShouldNotBeTarget() && (!eternalSoulActive || !c.IsCode(46986414)));
                    if (battleThreat != null && (battleThreat.Attack >= Card.Attack || battleThreat.Attack >= Card.Defense || mats >= 7 || mats < 5))
                    {
                        AI.SelectCard(battleThreat);
                        DecisionTracer.TraceActivate("ExpurrelyNoir", $"Battle Phase spin on {battleThreat.Name}");
                        return true;
                    }
                }

                // 3. Main Phase Proactive Interruption:
                if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
                {
                    var target = GetBestOpponentTarget();
                    if (target != null)
                    {
                        bool isBoardWipeTarget = target.IsCode(48680970, 48770333);
                        bool isBossThreat = (target.IsMonster() && (target.Attack >= 2500 || target.HasType(CardType.Fusion) || target.HasType(CardType.Synchro) || target.HasType(CardType.Xyz) || target.HasType(CardType.Link)));
                        bool isKeyBackrow = target.IsCode(47222536, 66399653, 53936268, 61740673, 68462976, 27541563) || (target.HasType(CardType.Field) || target.HasType(CardType.Continuous) || (target.IsFaceup() && target.IsTrap()));
                        bool inBattleDanger = target.IsMonster() && (target.Attack >= Card.Attack && target.Attack >= Card.Defense);

                        // If mats >= 7: Proactively spin boss threats or key backrow!
                        // If mats is 5 or 6: PROTECT TOWER IMMUNITY! ONLY spin if battle danger or board wipe!
                        // If mats < 5: Spin any threat to disrupt!
                        bool shouldSpin = isBoardWipeTarget || inBattleDanger ||
                            (mats >= 7 && (isBossThreat || isKeyBackrow)) ||
                            (mats < 5 && (target.IsMonster() || isKeyBackrow));

                        if (shouldSpin)
                        {
                            AI.SelectCard(target);
                            DecisionTracer.TraceActivate("ExpurrelyNoir", $"Main Phase proactive spin on {target.Name} (Mats: {mats})");
                            return true;
                        }
                    }
                }
            }

            // OUR TURN (Duel.Player == 0): Push for lethal & remove roadblocks
            if (Duel.Player == 0)
            {
                var target = GetBestOpponentTarget();
                if (target != null)
                {
                    bool isBoardWipeTarget = target.IsCode(48680970, 48770333);
                    bool inBattleDanger = target.IsMonster() && (target.Attack >= Card.Attack || target.Defense >= Card.Attack);
                    bool isBossThreat = (target.IsMonster() && (target.Attack >= 2000 || inBattleDanger || target.HasType(CardType.Fusion) || target.HasType(CardType.Synchro) || target.HasType(CardType.Xyz) || target.HasType(CardType.Link)));
                    bool isKeyBackrow = target.IsCode(47222536, 66399653, 53936268, 61740673, 68462976, 27541563) || (target.HasType(CardType.Field) || target.HasType(CardType.Continuous) || (target.IsFaceup() && target.IsTrap()));

                    // On our turn:
                    // Always spin board wipes, battle roadblocks (monsters we can't beat), boss threats, and key backrows!
                    bool shouldSpin = isBoardWipeTarget || CanDealLethal() || isBossThreat || isKeyBackrow || inBattleDanger ||
                        (mats >= 7 && target.IsMonster()) ||
                        (mats < 5 && target.IsMonster());

                    if (shouldSpin)
                    {
                        AI.SelectCard(target);
                        DecisionTracer.TraceActivate("ExpurrelyNoir", $"Our Turn spin on {target.Name} (Mats: {mats})");
                        return true;
                    }
                }
            }

            return false;
        }

        private bool EpurrelyPlumpEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Trigger 1: When a Purrely Quick-Play Spell is activated -> Attach it from field to Plump!
            if (Duel.CurrentChain.Count > 0)
            {
                var lastCard = Util.GetLastChainCard();
                if (lastCard != null && lastCard.Controller == 0 && MemorySpells.Contains(lastCard.Id))
                {
                    DecisionTracer.TraceActivate("EpurrelyPlump", "Attaching activated Quick-Play Spell to Plump!");
                    return true;
                }
            }

            // Effect 0: Ignition / Quick Effect to attach up to 2 Spells/Traps from GYs (Once per turn!)
            if (Duel.CurrentChain.Count == 0 || ActivateDescription == Util.GetStringId(CardId.EpurrelyPlump, 0))
            {
                // If Plump already has 5+ materials, DO NOT activate ignition effect; rank up to Expurrely Noir directly!
                if (GetOverlayCount(Card) >= 5) return false;
                if (_plumpAttachCount >= 1) return false;

                var gySpells = Bot.Graveyard.Concat(Enemy.Graveyard)
                    .Where(c => c != null && (c.IsSpell() || c.IsTrap()))
                    .OrderByDescending(c => {
                        if (c.Id == CardId.PurrelyDeliciousMemory) return 100; // ATK/DEF boost per material!
                        if (c.Id == CardId.PurrelySleepyMemory) return 90;     // Draw 1 per material!
                        if (MemorySpells.Contains(c.Id)) return 80;
                        return 10;
                    })
                    .Take(2)
                    .ToList();

                if (gySpells.Count > 0)
                {
                    _plumpAttachCount++;
                    AI.SelectCard(gySpells);
                    DecisionTracer.TraceActivate("EpurrelyPlump", $"Attaching {gySpells.Count} Spells/Traps from GY to Plump (Overlays: {GetOverlayCount(Card)})");
                    return true;
                }
            }

            return false;
        }

        private bool EpurrelyNoirEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Trigger 1: When a Purrely Quick-Play Spell is activated -> Attach it + Set Purrely Trap from Deck!
            // CRITICAL: Only check Trigger 1 during an active chain (Duel.CurrentChain.Count > 0), NEVER in IDLE!
            if (Duel.CurrentChain.Count > 0 && (ActivateDescription == Util.GetStringId(CardId.EpurrelyNoir, 1) || (Duel.LastChainPlayer == 0 && MemorySpells.Contains(Util.GetLastChainCard()?.Id ?? 0))))
            {
                DecisionTracer.TraceActivate("EpurrelyNoir", "Attaching activated Quick-Play + Setting Purrelyeap from Deck!");
                return true;
            }

            // Effect 0: Discard 1 to bounce 1-2 opponent cards
            if (ActivateDescription == Util.GetStringId(CardId.EpurrelyNoir, 0) || ActivateDescription == -1 || Duel.CurrentChain.Count == 0)
            {
                if (_noirBounceUsed) return false;
                if (Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0) return false;

                var discard = GetBestDiscardCard();
                if (discard == null && Bot.Hand.Count == 0) return false;

                var target = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && (c.HasType(CardType.Continuous) || c.HasType(CardType.Field) || c.IsTrap())) ??
                             Enemy.GetMonsters().Concat(Enemy.GetSpells())
                                .Where(c => c != null && !c.IsShouldNotBeTarget())
                                .OrderByDescending(c => c.IsMonster() && c.IsFaceup() ? c.Attack : 1000)
                                .FirstOrDefault();

                if (target != null && discard != null)
                {
                    _noirBounceUsed = true;
                    AI.SelectCard(discard);
                    AI.SelectNextCard(target);
                    DecisionTracer.TraceActivate("EpurrelyNoir", $"Discarding to bounce {target.Name}");
                    return true;
                }
            }

            return false;
        }

        private bool EpurrelyBeautyEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Trigger 1: Attach activated Quick-Play + change battle position
            // CRITICAL: Only check Trigger 1 during an active chain (Duel.CurrentChain.Count > 0), NEVER in IDLE!
            if (Duel.CurrentChain.Count > 0 && (ActivateDescription == Util.GetStringId(CardId.EpurrelyBeauty, 1) || (Duel.LastChainPlayer == 0 && MemorySpells.Contains(Util.GetLastChainCard()?.Id ?? 0))))
            {
                var oppAtk = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsAttack());
                if (oppAtk != null) AI.SelectCard(oppAtk);
                DecisionTracer.TraceActivate("EpurrelyBeauty", "Attaching activated Quick-Play to Beauty");
                return true;
            }

            // Effect 0: Negate 1 opponent monster (Quick Effect if has Pretty Memory as material!)
            if (ActivateDescription == Util.GetStringId(CardId.EpurrelyBeauty, 0) || ActivateDescription == -1)
            {
                if (_beautyNegateUsed) return false;

                // If opponent just activated a monster effect on field, negate it!
                if (Duel.LastChainPlayer == 1 && Util.GetLastChainCard()?.Location == CardLocation.MonsterZone)
                {
                    var chainCard = Util.GetLastChainCard();
                    if (chainCard != null && !chainCard.IsDisabled() && !chainCard.IsShouldNotBeTarget())
                    {
                        _beautyNegateUsed = true;
                        AI.SelectCard(chainCard);
                        DecisionTracer.TraceActivate("EpurrelyBeauty", $"Chaining negation to {chainCard.Name}");
                        return true;
                    }
                }

                // Proactive negation on opponent's turn
                var target = Enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && !c.IsDisabled() && !c.IsShouldNotBeTarget())
                    .OrderByDescending(c => c.Attack + (c.HasType(CardType.Effect) ? 3000 : 0))
                    .FirstOrDefault();

                if (target != null)
                {
                    _beautyNegateUsed = true;
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("EpurrelyBeauty", $"Negating opponent monster {target.Name}");
                    return true;
                }
            }

            return false;
        }

        private bool EpurrelyHappinessEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Trigger 1: Attach Quick-Play + bounce S/T
            if (ActivateDescription == Util.GetStringId(CardId.EpurrelyHappiness, 1))
            {
                var oppSpell = Enemy.GetSpells().FirstOrDefault(c => c != null);
                if (oppSpell != null) AI.SelectCard(oppSpell);
                return true;
            }

            // Effect 0: Damage Step search + Halve ATK
            if (ActivateDescription == Util.GetStringId(CardId.EpurrelyHappiness, 0) || ActivateDescription == -1)
            {
                AI.SelectCard(new[] {
                    CardId.PurrelyDeliciousMemory,
                    CardId.PurrelyHappyMemory,
                    CardId.MyFriendPurrely
                });
                return true;
            }

            return false;
        }

        private bool PurrelyeapEffect()
        {
            if (_purrelyeapUsed) return false;

            // Trap on field activation (Description 0)
            if (Card.Location == CardLocation.SpellZone)
            {
                var rank2Xyz = Bot.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && c.Rank == 2 && PurrelyXyz.Contains(c.Id))
                    .OrderByDescending(c => GetOverlayCount(c))
                    .FirstOrDefault();

                if (rank2Xyz != null)
                {
                    int mats = GetOverlayCount(rank2Xyz);

                    // OUR TURN (Duel.Player == 0):
                    if (Duel.Player == 0)
                    {
                        // In Battle Phase: if our rank 2 has attacked or can't beat enemy, rank up into Noir to attack again!
                        if (Duel.Phase == DuelPhase.Battle)
                        {
                            _purrelyeapUsed = true;
                            AI.SelectCard(rank2Xyz);
                            DecisionTracer.TraceActivate("Purrelyeap", $"Battle Phase ranking up {rank2Xyz.Name} into Noir for game!");
                            return true;
                        }
                        if (Duel.Phase == DuelPhase.End)
                        {
                            _purrelyeapUsed = true;
                            AI.SelectCard(rank2Xyz);
                            DecisionTracer.TraceActivate("Purrelyeap", $"End Phase ranking up {rank2Xyz.Name} into Noir before turn pass");
                            return true;
                        }
                        return false;
                    }

                    // OPPONENT'S TURN (Duel.Player == 1):
                    // 1. Chained to backrow removal or targeting danger -> CHAIN IMMEDIATELY!
                    bool isChainedToDanger = false;
                    if (Duel.LastChainPlayer == 1)
                    {
                        var lastCard = Util.GetLastChainCard();
                        if (lastCard != null)
                        {
                            if (lastCard.IsCode(2314238, 18144507, 53582587, 43898403, 73580471, 98338152, 12580477, 47222536) ||
                                (Duel.ChainTargets != null && (Duel.ChainTargets.Contains(Card) || Duel.ChainTargets.Contains(rank2Xyz))) ||
                                (Duel.LastChainTargets != null && (Duel.LastChainTargets.Contains(Card) || Duel.LastChainTargets.Contains(rank2Xyz))))
                            {
                                isChainedToDanger = true;
                            }
                        }
                    }

                    // On Opponent's turn: Rank up immediately if chained to danger, or in battle phase, or opponent controls any monster, or opponent activated anything, or end phase!
                    // Rank 2 monsters on opponent's turn have zero protection. Turning them into a 2800 DEF Expurrely Noir with Quick spin is strictly optimal!
                    bool shouldRankUp = isChainedToDanger ||
                                        (Duel.Phase == DuelPhase.Battle) ||
                                        (Duel.Phase == DuelPhase.End) ||
                                        (Enemy.GetMonsterCount() > 0) ||
                                        (Duel.LastChainPlayer == 1) ||
                                        (Duel.CurrentChain.Count > 0);

                    if (shouldRankUp)
                    {
                        _purrelyeapUsed = true;
                        AI.SelectCard(rank2Xyz);
                        DecisionTracer.TraceActivate("Purrelyeap", $"Ranking up {rank2Xyz.Name} ({mats} mats) into Expurrely Noir via Purrelyeap!?");
                        return true;
                    }
                }
            }

            // GY Banish to shuffle 3 Purrelys into Deck (Description 1)
            if (Card.Location == CardLocation.Grave)
            {
                var gyPurrelys = Bot.Graveyard.Where(c => c != null && (c.IsCode(CardId.Purrely, CardId.Purrelyly) || PurrelyXyz.Contains(c.Id))).Take(3).ToList();
                if (gyPurrelys.Count >= 3)
                {
                    AI.SelectCard(gyPurrelys);
                    DecisionTracer.TraceActivate("Purrelyeap", "Recycling Purrelys from GY to Deck");
                    return true;
                }
            }

            return false;
        }

        private bool PurrelyeapSetCondition()
        {
            return true;
        }

        private bool EpurrelyPlumpSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool EpurrelyNoirSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool EpurrelyBeautySpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool EpurrelyHappinessSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        // ============================================================
        // TRAP CARDS & RESPONSES
        // ============================================================

        private bool RivalryEffect()
        {
            if (Duel.Player != 1) return false;
            if (Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.RivalryOfWarlords))) return false;
            return Enemy.GetMonsterCount() >= 2;
        }

        private bool RedRebootEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            return Util.GetLastChainCard()?.IsTrap() == true;
        }

        private bool DimensionalBarrierEffect()
        {
            if (Duel.Player != 1) return false;
            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Xyz))) { AI.SelectOption(3); return true; }
            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Fusion))) { AI.SelectOption(0); return true; }
            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Synchro))) { AI.SelectOption(1); return true; }
            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Link))) { AI.SelectOption(4); return true; }
            return false;
        }

        // ============================================================
        // REPOSITIONING
        // ============================================================

        private bool CustomMonsterRepos()
        {
            // In Main 2: Switch lower-ATK monsters to Defense for safety ONLY if DEF > ATK
            if (Duel.Phase == DuelPhase.Main2)
            {
                if (Card.IsAttack() && Card.Defense > Card.Attack) return true;
                return false;
            }

            // In Main 1 on our turn: We want our beaters in Attack mode!
            if (Duel.Phase == DuelPhase.Main1 && Duel.Player == 0 && Duel.Turn > 1)
            {
                var strongestEnemy = Enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup())
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();

                if (Card.IsDefense())
                {
                    // If we can deal lethal, always switch to attack!
                    if (CanDealLethal()) return true;

                    // Small baby monsters (Purrely / Purrelyly / handtraps) stay in DEF for safety
                    if (Card.IsCode(CardId.Purrely, CardId.Purrelyly) || Card.Attack < 1000)
                    {
                        return false;
                    }

                    // For Expurrely Noir and all Xyz/Boss beaters:
                    // If opponent has no face-up monsters, SWITCH TO ATTACK to deal direct damage!
                    if (strongestEnemy == null && Card.Attack > 0)
                    {
                        return true;
                    }

                    // If our monster can destroy the opponent's strongest monster in battle, SWITCH TO ATTACK!
                    if (strongestEnemy != null && Card.Attack > strongestEnemy.Attack)
                    {
                        return true;
                    }

                    // Expurrely Noir with 5+ materials is unaffected by effects; if safe, attack!
                    if (Card.IsCode(CardId.ExpurrelyNoir) && (strongestEnemy == null || Card.Attack >= strongestEnemy.Attack))
                    {
                        return true;
                    }

                    return false;
                }
                else if (Card.IsAttack())
                {
                    // Small monsters (Purrely / Purrelyly with 100-300 ATK) should NOT be in Attack mode
                    if ((Card.IsCode(CardId.Purrely, CardId.Purrelyly) || Card.Attack < 1000) && !CanDealLethal())
                    {
                        return true; // Switch small babies to Defense
                    }

                    // Only switch to DEF if opponent has a stronger monster and our DEF is higher to shield LP
                    if (strongestEnemy != null && Card.Attack < strongestEnemy.Attack && Card.Defense > Card.Attack)
                    {
                        return true;
                    }
                }
                return false;
            }

            return DefaultMonsterRepos();
        }

        public override ClientCard OnSelectAttacker(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            // CRITICAL: Never attack Mekk-Knight Crusadia Avramax (21887175) with Special Summoned monsters!
            // Avramax gains ATK equal to the opponent's monster during damage calculation.
            var hasAvramax = defenders.Any(d => d != null && d.IsCode(21887175));
            if (hasAvramax)
            {
                var nonAvramaxDefenders = defenders.Where(d => d != null && !d.IsCode(21887175)).ToList();
                if (nonAvramaxDefenders.Count == 0) return null;
                defenders = nonAvramaxDefenders;
            }

            // Filter out 0-500 ATK baby monsters (Purrely, Purrelyly) from suiciding into enemy monsters
            var effectiveAttackers = attackers.Where(c => {
                if (c == null) return false;
                if (c.Attack <= 500 && defenders.Count > 0) return false;
                // Noir shouldn't attack only if ALL defenders are stronger (cannot beat over any)
                if (c.IsCode(CardId.ExpurrelyNoir) && defenders.Count > 0 && defenders.All(d => d != null && d.IsFaceup() && (d.IsAttack() ? d.Attack >= c.Attack : d.Defense >= c.Attack))) return false;
                return true;
            }).ToList();

            ClientCard selected = null;
            if (effectiveAttackers.Count > 0)
            {
                selected = base.OnSelectAttacker(effectiveAttackers, defenders);
            }
            else if (defenders.Count == 0)
            {
                selected = base.OnSelectAttacker(attackers, defenders);
            }

            if (selected != null && selected.HasType(CardType.Xyz))
            {
                _xyzBattledThisTurn = true;
            }
            return selected;
        }

        // ============================================================
        // COMPLETE HOOK OVERRIDES (OnSelectYesNo, OnSelectEffectYn, OnSelectCard, OnSelectOption, OnSelectPosition)
        // ============================================================

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            if (card != null)
            {
                // Always activate Purrelyly search on summon
                if (card.IsCode(CardId.Purrelyly))
                {
                    _purrelylySearchUsed = true;
                    return true;
                }
                // Always activate Purrely excavation on summon
                if (card.IsCode(CardId.Purrely)) return true;
                // Always activate Plump attach trigger when Quick-Play activated
                if (card.IsCode(CardId.EpurrelyPlump)) return true;
                // Always activate Noir set trap trigger when Quick-Play activated
                if (card.IsCode(CardId.EpurrelyNoir)) return true;
                // Always activate Beauty position change trigger
                if (card.IsCode(CardId.EpurrelyBeauty)) return true;
                // Always activate Happiness bounce trigger
                if (card.IsCode(CardId.EpurrelyHappiness)) return true;
                // Always activate Sleepy Memory draw trigger in Standby Phase
                if (card.IsCode(CardId.PurrelySleepyMemory)) return true;
                // Always activate My Friend Purrely / Stray Purrely Street float triggers
                if (card.IsCode(CardId.MyFriendPurrely, CardId.StrayPurrelyStreet)) return true;
            }
            return true;
        }

        public override bool OnSelectYesNo(long desc)
        {
            // 1. Plump optional banish: "Banish 1 monster on the field until the End Phase?"
            if (desc == Util.GetStringId(CardId.EpurrelyPlump, 2))
            {
                // Banish opponent face-up monster if one exists!
                return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget());
            }

            // 2. Memory Spells: "Discard 1 card, and if you do, Special Summon 1 Level 1 Purrely from Deck?"
            // Delicious (index 1), Happy (index 1), Pretty (index 2), Sleepy (index 3)
            if (desc == Util.GetStringId(CardId.PurrelyDeliciousMemory, 1) ||
                desc == Util.GetStringId(CardId.PurrelyHappyMemory, 1) ||
                desc == Util.GetStringId(CardId.PurrelyPrettyMemory, 2) ||
                desc == Util.GetStringId(CardId.PurrelySleepyMemory, 3))
            {
                // If we already control Expurrely Noir: NEVER discard!
                if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.ExpurrelyNoir)))
                    return false;

                // If we control a Purrely Xyz (e.g. Plump): DO NOT discard! Plump already attaches the spell from field!
                if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && PurrelyXyz.Contains(c.Id)))
                    return false;

                // If we have NO Purrely monsters on field: Discard only to establish our Starter!
                bool hasPurrelyMonsterOnField = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.IsCode(CardId.Purrely, CardId.Purrelyly) || PurrelyXyz.Contains(c.Id)));
                if (!hasPurrelyMonsterOnField)
                {
                    return Bot.Hand.Count >= 2;
                }

                // If we already control Purrely or Purrelyly on field:
                // Conserve our hand cards so we have spells to reveal/attach!
                return false;
            }

            // 3. Epurrely Noir setting Purrely Trap from Deck:
            if (desc == Util.GetStringId(CardId.EpurrelyNoir, 2))
            {
                return Bot.GetRemainingCount(CardId.Purrelyeap, 2) > 0;
            }

            // 4. Epurrely Beauty changing battle position of opponent monster:
            if (desc == Util.GetStringId(CardId.EpurrelyBeauty, 2))
            {
                return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsAttack());
            }

            return true;
        }

        public override int OnSelectOption(IList<long> options)
        {
            return base.OnSelectOption(options);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Hint 526: HINTMSG_CONFIRM (Purrely revealing Quick-Play from hand to Xyz)
            if (hint == 526)
            {
                var qp = cards.Where(c => c != null && MemorySpells.Contains(c.Id))
                    .OrderBy(c => {
                        // Plump (Delicious) is #1 priority
                        if (c.Id == CardId.PurrelyDeliciousMemory && Bot.ExtraDeck.Any(e => e.IsCode(CardId.EpurrelyPlump))) return 1;
                        if (c.Id == CardId.PurrelySleepyMemory && Bot.ExtraDeck.Any(e => e.IsCode(CardId.EpurrelyNoir))) return 2;
                        if (c.Id == CardId.PurrelyPrettyMemory && Bot.ExtraDeck.Any(e => e.IsCode(CardId.EpurrelyBeauty))) return 3;
                        if (c.Id == CardId.PurrelyHappyMemory && Bot.ExtraDeck.Any(e => e.IsCode(CardId.EpurrelyHappiness))) return 4;
                        return 10;
                    }).ToList();
                if (qp.Count > 0) return qp.Take(max).ToList();
            }

            // Hint 551: HINTMSG_TARGET (Purrelyly targeting GY / Purrelyeap targeting field / Protection targets)
            if (hint == 551)
            {
                // Case A: Purrelyly targeting GY Quick-Play
                if (cards.All(c => c.Location == CardLocation.Grave && MemorySpells.Contains(c.Id)))
                {
                    var bestGY = cards.OrderBy(c => {
                        if (c.Id == CardId.PurrelyDeliciousMemory && Bot.ExtraDeck.Any(e => e.IsCode(CardId.EpurrelyPlump))) return 1;
                        if (c.Id == CardId.PurrelySleepyMemory && Bot.ExtraDeck.Any(e => e.IsCode(CardId.EpurrelyNoir))) return 2;
                        if (c.Id == CardId.PurrelyPrettyMemory && Bot.ExtraDeck.Any(e => e.IsCode(CardId.EpurrelyBeauty))) return 3;
                        return 4;
                    }).ToList();
                    return bestGY.Take(max).ToList();
                }

                // Case B: Purrelyeap!? targeting friendly Rank 2 Xyz on field
                if (cards.Any(c => c != null && c.Location == CardLocation.MonsterZone && c.Controller == 0 && c.Rank == 2))
                {
                    var bestRank2 = cards.Where(c => c != null && c.Location == CardLocation.MonsterZone && c.Controller == 0 && c.Rank == 2)
                                         .OrderByDescending(c => GetOverlayCount(c))
                                         .FirstOrDefault();
                    if (bestRank2 != null) return new[] { bestRank2 };
                }

                // Case C: Delicious / Happy Memory targeting a monster for protection:
                var ourMon = cards.FirstOrDefault(c => c != null && c.Controller == 0 && c.IsFaceup());
                if (ourMon != null && Bot.BattlingMonster == null && Duel.CurrentChain.Count > 0)
                {
                    var lastChain = Util.GetLastChainCard();
                    if (lastChain != null && lastChain.IsCode(CardId.PurrelyDeliciousMemory, CardId.PurrelyHappyMemory))
                        return new[] { ourMon };
                }

                // Case D: General removal / disruption targeting opponent card:
                var oppTarget = cards.Where(c => c != null && c.Controller == 1 && !c.IsShouldNotBeTarget())
                    .OrderByDescending(c => {
                        if (c.IsCode(48680970, 48770333)) return 50000;
                        if (c.IsSpell() || c.IsTrap()) return c.IsFaceup() ? 20000 : 10000;
                        if (c.IsMonster())
                        {
                            int s = (c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link)) ? 25000 : 15000;
                            return s + c.Attack;
                        }
                        return 0;
                    }).FirstOrDefault();
                if (oppTarget != null) return new[] { oppTarget };
            }

            // ONLY select a Rank 2 on FIELD if explicitly resolving Purrelyeap OR performing an Xyz overlay!
            if (min == 1 && (hint == 513 || Util.GetLastChainCard()?.Id == CardId.Purrelyeap) &&
                cards.Any(c => c != null && c.Location == CardLocation.MonsterZone && c.Controller == 0 && c.Rank == 2))
            {
                var bestRank2 = cards.Where(c => c != null && c.Location == CardLocation.MonsterZone && c.Controller == 0 && c.Rank == 2)
                                     .OrderByDescending(c => GetOverlayCount(c))
                                     .FirstOrDefault();
                if (bestRank2 != null)
                {
                    return new[] { bestRank2 };
                }
            }

            // CRITICAL: NEVER select Purrely Xyz monsters or Expurrely Noir as Link/Fusion material or Tribute!
            if (hint == 0 || hint == 500 || hint == 504 || hint == 505)
            {
                var nonXyz = cards.Where(c => c != null && !PurrelyXyz.Contains(c.Id) && !c.IsCode(CardId.ExpurrelyNoir)).ToList();
                if (nonXyz.Count >= min)
                {
                    cards = nonXyz;
                }
            }

            // Hint 503: Banish (e.g. Plump optional banish trigger)
            if (hint == 503)
            {
                var oppMonsters = cards.Where(c => c != null && (c.Controller == 1 || Enemy.GetMonsters().Contains(c)) && c.IsFaceup() && !c.IsShouldNotBeTarget())
                                       .OrderByDescending(c => c.Attack)
                                       .ToList();
                if (oppMonsters.Count > 0)
                {
                    return oppMonsters.Take(max).ToList();
                }
                if (cancelable) return null;
                // PROTECT PURRELY XYZ: NEVER BANISH OUR OWN XYZ MONSTERS!
                var safeTargets = cards.Where(c => c != null && !PurrelyXyz.Contains(c.Id) && c.Id != CardId.ExpurrelyNoir)
                                       .OrderBy(c => c.Attack)
                                       .ToList();
                if (safeTargets.Count > 0) return safeTargets.Take(max).ToList();
            }

            // Hint 502 / 507: Destruction / Removal Target / Spin to Deck Target
            if (hint == 502 || hint == 507)
            {
                bool eternalSoulActive = Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(48680970));
                var sorted = cards.OrderByDescending(c => {
                    if (c == null) return -999;
                    int score = 0;
                    // Opponent cards vs our own
                    if (c.Controller == 1) score += 20000;
                    else score -= 20000;

                    // FIELD targets have MUCH higher priority than GY targets!
                    if (c.Location == CardLocation.MonsterZone || c.Location == CardLocation.SpellZone)
                        score += 30000;
                    else if (c.Location == CardLocation.Grave)
                        score += 5000;

                    if (c.IsSpell() || c.IsTrap())
                    {
                        // Eternal Soul / True Light on FIELD: Board wipe when spun! Absolute #1 priority!
                        if (c.IsCode(48680970, 48770333))
                        {
                            if (c.Location == CardLocation.SpellZone && c.IsFaceup())
                                return score + 50000;
                            // If in GY, low priority (do not help opponent recycle it!)
                            return score - 10000;
                        }
                        if (c.Location == CardLocation.SpellZone)
                        {
                            if (c.IsFaceup()) score += 10000; // Continuous/Field/Equip spells
                            else score += 3000; // Set backrow
                        }
                        return score;
                    }
                    if (c.IsMonster())
                    {
                        // Dark Magician on field under Eternal Soul is immune to card effects! NEVER TARGET!
                        if (eternalSoulActive && c.IsCode(46986414) && c.Location == CardLocation.MonsterZone)
                            return -50000;

                        if (c.Location == CardLocation.MonsterZone)
                        {
                            if (c.IsFaceup() && !c.IsDisabled()) score += 15000;
                            if (c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link))
                                score += 8000;
                            return score + c.Attack;
                        }
                        else if (c.Location == CardLocation.Grave)
                        {
                            // In GY: prioritize key GY recurrers (ABC pieces, Dark Magician, Blue-Eyes)
                            if (c.IsCode(46986414, 89631139, 99785935, 65877963, 77411244)) score += 5000;
                            return score + c.Attack / 2;
                        }
                    }
                    return score;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // Hint 506: Search / Add to hand / Reveal selection
            if (hint == 506)
            {
                // My Friend Purrely 3-card reveal:
                if (min == 3 && max == 3)
                {
                    // 100% guarantee a Quick-Play Memory spell by revealing ONLY Quick-Play Memory spells!
                    var deliciousCopies = cards.Where(c => c != null && c.Id == CardId.PurrelyDeliciousMemory).ToList();
                    if (deliciousCopies.Count >= 3)
                    {
                        return deliciousCopies.Take(3).ToList();
                    }

                    var memSpells = cards.Where(c => c != null && MemorySpells.Contains(c.Id))
                        .OrderByDescending(c => {
                            if (c.Id == CardId.PurrelyDeliciousMemory) return 100;
                            if (c.Id == CardId.PurrelySleepyMemory) return 90;
                            if (c.Id == CardId.PurrelyPrettyMemory) return 80;
                            if (c.Id == CardId.PurrelyHappyMemory) return 70;
                            return 10;
                        }).ToList();

                    if (memSpells.Count >= 3)
                    {
                        return memSpells.Take(3).ToList();
                    }
                }

                bool hasMonsterStarter = Bot.GetMonsterCount() > 0 || Bot.Hand.Any(h => h.IsCode(CardId.Purrely, CardId.Purrelyly));
                var sorted = cards.OrderByDescending(c => {
                    if (c == null) return -999;
                    // If we have NO monsters on field or in hand: WE MUST SEARCH A MONSTER STARTER FIRST!
                    if (!hasMonsterStarter)
                    {
                        if (c.Id == CardId.Purrelyly) return 1000;
                        if (c.Id == CardId.Purrely) return 900;
                        if (c.Id == CardId.PurrelyDeliciousMemory) return 800;
                        if (c.Id == CardId.PurrelySleepyMemory) return 700;
                        if (c.Id == CardId.PurrelyPrettyMemory) return 600;
                        if (c.Id == CardId.MyFriendPurrely) return 500;
                        return 10;
                    }

                    // Standard search priority with starter already secured:
                    if (c.Id == CardId.MyFriendPurrely && !Bot.HasInSpellZone(CardId.MyFriendPurrely) && !Bot.HasInHand(CardId.MyFriendPurrely)) return 100;
                    if (c.Id == CardId.PurrelyDeliciousMemory && !Bot.HasInHand(CardId.PurrelyDeliciousMemory)) return 95;
                    if (c.Id == CardId.Purrelyeap && !Bot.HasInSpellZone(CardId.Purrelyeap) && !Bot.HasInHand(CardId.Purrelyeap)) return 90;
                    if (c.Id == CardId.PurrelySleepyMemory) return 85;
                    if (c.Id == CardId.StrayPurrelyStreet && !Bot.HasInSpellZone(CardId.StrayPurrelyStreet) && !Bot.HasInHand(CardId.StrayPurrelyStreet)) return 80;
                    if (c.Id == CardId.Purrelyly && !Bot.HasInHand(CardId.Purrelyly)) return 75;
                    if (c.Id == CardId.Purrely) return 70;
                    if (c.Id == CardId.PurrelyPrettyMemory) return 60;
                    if (c.Id == CardId.PurrelyHappyMemory) return 50;
                    return 10;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // Hint 508 / 501 / 505 / 577: Discard / Send to GY cost
            if (hint == 508 || hint == 501 || hint == 505 || hint == 577)
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (c.IsCode(CardId.RadiantTyphoonVision)) return 5;
                    // Duplicates of spells
                    if (MemorySpells.Contains(c.Id) && cards.Count(h => h.Id == c.Id) > 1) return 10;
                    if (c.IsCode(CardId.StrayPurrelyStreet) && Bot.HasInSpellZone(CardId.StrayPurrelyStreet)) return 15;
                    if (c.IsCode(CardId.MyFriendPurrely) && Bot.HasInSpellZone(CardId.MyFriendPurrely)) return 18;
                    // Extra copies of normal summons if already used
                    if (c.IsCode(CardId.Purrelyly) && (_normalSummonUsed || _purrelylySearchUsed)) return 20;
                    if (c.IsCode(CardId.Purrely) && (_normalSummonUsed || _purrelyExcavateUsed)) return 22;
                    if (c.IsCode(CardId.Purrely, CardId.Purrelyly) && cards.Count(h => c.IsCode(h.Id)) > 1) return 25;

                    // Single memory spells
                    if (c.IsCode(CardId.PurrelyHappyMemory)) return 30;
                    if (c.IsCode(CardId.PurrelyPrettyMemory)) return 35;
                    if (c.IsCode(CardId.PurrelySleepyMemory)) return 40;
                    if (c.IsCode(CardId.PurrelyDeliciousMemory)) return 45;

                    // Handtraps
                    if (c.IsCode(CardId.EffectVeiler, CardId.GhostBelleAndHauntedMansion)) return 60;
                    if (c.IsCode(CardId.AshBlossomAndJoyousSpring, CardId.MaxxC)) return 80;

                    // Purrelyeap duplicates
                    if (c.IsCode(CardId.Purrelyeap) && (Bot.HasInSpellZone(CardId.Purrelyeap) || cards.Count(h => h.Id == CardId.Purrelyeap) > 1)) return 35;

                    // PROTECT UNIQUE PURRELYEAP AT ALL COSTS:
                    if (c.IsCode(CardId.Purrelyeap)) return 800; // NEVER discard our only Purrelyeap!

                    if (IsAceCard(c)) return 900;
                    return 50;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // Hint 509: Special Summon from Deck / Extra / GY
            if (hint == 509)
            {
                var sorted = cards.OrderByDescending(c => {
                    if (c == null) return -999;
                    // Extra Deck Xyz Rank 2 priority
                    if (c.Id == CardId.ExpurrelyNoir) return 2000;
                    if (c.Id == CardId.SuperStarslayerTYPHONSkyCrisis) return 1600;
                    if (c.Id == CardId.DivineArsenalAAZEUSSkyThunder) return 1500;
                    if (c.Id == CardId.DownerdMagician) return 1200;
                    if (c.Id == CardId.EpurrelyPlump) return 1000;
                    if (c.Id == CardId.EpurrelyNoir) return 900;
                    if (c.Id == CardId.EpurrelyBeauty) return 800;
                    if (c.Id == CardId.EpurrelyHappiness) return 700;
                    // Main deck starters: If Purrelyly is already on field or used, ALWAYS summon non-OPT Purrely!
                    bool purrelylyActive = _purrelylySearchUsed || _normalSummonUsed || Bot.GetMonsters().Any(m => m != null && m.IsCode(CardId.Purrelyly));
                    if (c.Id == CardId.Purrely) return purrelylyActive ? 150 : 90;
                    if (c.Id == CardId.Purrelyly) return purrelylyActive ? 20 : 100;
                    return 10;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // Hint 513: Attach materials from GY (Plump)
            if (hint == 513 || (cards.Count > 0 && cards.All(c => c.Location == CardLocation.Grave && (c.IsSpell() || c.IsTrap()))))
            {
                var sorted = cards.OrderByDescending(c => {
                    if (c == null) return -999;
                    if (c.Id == CardId.PurrelyDeliciousMemory) return 100;
                    if (c.Id == CardId.PurrelySleepyMemory) return 90;
                    if (MemorySpells.Contains(c.Id)) return 80;
                    if (c.IsSpell() || c.IsTrap()) return 50;
                    return 10;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // Hint 519: Detach Xyz Material cost (Protect Sleepy & Delicious!)
            if (hint == 519)
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (c.IsCode(CardId.Purrely, CardId.Purrelyly)) return 5;
                    if (!MemorySpells.Contains(c.Id) && !PurrelyXyz.Contains(c.Id)) return 10;
                    if (c.IsCode(CardId.PurrelyHappyMemory)) return 20;
                    if (c.IsCode(CardId.PurrelyPrettyMemory)) return 30;
                    if (PurrelyXyz.Contains(c.Id)) return 40;
                    // PROTECT Delicious Memory (Stat buff) & Sleepy Memory (Draw engine)!
                    if (c.IsCode(CardId.PurrelyDeliciousMemory)) return 80;
                    if (c.IsCode(CardId.PurrelySleepyMemory)) return 100;
                    return 50;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // Protect Ace Cards in materials
            if (hint == 511 || hint == 512 || hint == 533)
            {
                var sorted = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
                if (cancelable)
                {
                    var nonFieldAces = sorted.Where(c => !(c.Location == CardLocation.MonsterZone && IsAceCard(c) && c.Rank != 2)).ToList();
                    if (nonFieldAces.Count < min) return null;
                    return Util.CheckSelectCount(nonFieldAces, cards, min, max);
                }
                return Util.CheckSelectCount(sorted, cards, min, max);
            }

            // HINTMSG_ATTACKTARGET = 549 (Battle Target Selection)
            if (hint == 549)
            {
                var attacker = Bot.BattlingMonster;
                int attackerAtk = attacker?.Attack ?? 0;

                // Priority #1: Monsters with combat tricks / honest effects (Apprentice Illusion Magician)
                var combatThreat = cards.FirstOrDefault(c => c != null && c.IsFaceup() && c.Location == CardLocation.MonsterZone && c.IsCode(30603688) && c.Attack < attackerAtk);
                if (combatThreat != null)
                {
                    return new[] { combatThreat };
                }

                var beatable = cards.Where(c => c != null && c.IsFaceup() && c.Location == CardLocation.MonsterZone
                    && (c.IsAttack() ? c.Attack < attackerAtk : c.Defense < attackerAtk)).ToList();
                if (beatable.Count >= min)
                    return beatable.OrderByDescending(c => c.Attack).Take(max).ToList();

                if (cancelable) return null; // Cancel attack replay instead of suiciding!

                var weakest = cards.Where(c => c != null && c.IsFaceup() && c.Location == CardLocation.MonsterZone)
                    .OrderBy(c => c.Attack).Take(max).ToList();
                if (weakest.Count >= min) return weakest;
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (positions.Contains(CardPosition.FaceUpAttack))
            {
                // On our turn (Turn > 1, Main 1), offensive extra deck summons enter in Attack:
                if (Duel.Player == 0 && Duel.Turn > 1 && Duel.Phase == DuelPhase.Main1)
                {
                    if (cardId == CardId.ExpurrelyNoir ||
                        cardId == CardId.DivineArsenalAAZEUSSkyThunder ||
                        cardId == CardId.SuperStarslayerTYPHONSkyCrisis || cardId == CardId.DownerdMagician ||
                        cardId == CardId.EpurrelyHappiness || cardId == CardId.EpurrelyBeauty)
                    {
                        return CardPosition.FaceUpAttack;
                    }
                }
            }

            // Defense Position for safety:
            if (positions.Contains(CardPosition.FaceUpDefence))
            {
                // Expurrely Noir: On Turn 1 (or outside our Main 1) enter in Defense for 2800+ DEF fortress!
                if (cardId == CardId.ExpurrelyNoir)
                {
                    if (Duel.Turn == 1 || Duel.Phase != DuelPhase.Main1)
                        return CardPosition.FaceUpDefence;
                }

                int[] defenseMonsters = {
                    CardId.EpurrelyPlump,
                    CardId.EpurrelyNoir,
                    CardId.Purrely,
                    CardId.Purrelyly,
                    CardId.MaxxC,
                    CardId.EffectVeiler,
                    CardId.GhostBelleAndHauntedMansion,
                    CardId.DrollAndLockBird,
                    CardId.ArtifactLancea
                };
                if (defenseMonsters.Contains(cardId))
                    return CardPosition.FaceUpDefence;
            }

            return base.OnSelectPosition(cardId, positions);
        }
    }
}
