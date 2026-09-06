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
        private bool _plumpAttachUsed = false;
        private bool _noirBounceUsed = false;
        private bool _beautyNegateUsed = false;
        private bool _purrelyeapUsed = false;
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
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCCondition);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAndJoyousSpring, AshCondition);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelleAndHauntedMansion, GhostBelleCondition);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, EffectVeilerCondition);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollCondition);
            AddExecutor(ExecutorType.Activate, CardId.NibiruThePrimalBeing, NibiruCondition);

            // ============================================================
            // TIER 2: Boss Monster Quick Effects, Triggers & Multi-Spins
            // ============================================================
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
            AddExecutor(ExecutorType.Activate, CardId.Purrelyeap, PurrelyeapEffect);

            // ============================================================
            // TIER 5: NORMAL SUMMON STARTERS FIRST! (Establish monster to reveal Quick-Play!)
            // ============================================================
            AddExecutor(ExecutorType.Summon, CardId.Purrelyly, PurrelylyNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.Purrely, PurrelyNormalSummon);

            // ============================================================
            // TIER 6: Monster Field Effects (Excavate, Search & Reveal Xyz)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.Purrely, PurrelyEffect);
            AddExecutor(ExecutorType.Activate, CardId.Purrelyly, PurrelylyEffect);

            // ============================================================
            // TIER 7: Rank-up into Expurrely Noir (5+ Materials) / ZEUS Board Wipe / TYPHON
            // ============================================================
            AddExecutor(ExecutorType.SpSummon, CardId.ExpurrelyNoir, ExpurrelyNoirSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SuperStarslayerTYPHONSkyCrisis, TyphonSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DownerdMagician, DownerdMagicianSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DivineArsenalAAZEUSSkyThunder, ZeusSpSummon);

            // ============================================================
            // TIER 8: Quick-Play Memory Spells (Feed Plump / SS Starter if needed)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.PurrelyDeliciousMemory, PurrelyDeliciousMemoryEffect);
            AddExecutor(ExecutorType.Activate, CardId.PurrelySleepyMemory, PurrelySleepyMemoryEffect);
            AddExecutor(ExecutorType.Activate, CardId.PurrelyPrettyMemory, PurrelyPrettyMemoryEffect);
            AddExecutor(ExecutorType.Activate, CardId.PurrelyHappyMemory, PurrelyHappyMemoryEffect);

            // ============================================================
            // TIER 9: Extra Deck Summons
            // ============================================================
            AddExecutor(ExecutorType.SpSummon, CardId.EpurrelyPlump, EpurrelyPlumpSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.EpurrelyNoir, EpurrelyNoirSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.EpurrelyBeauty, EpurrelyBeautySpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.EpurrelyHappiness, EpurrelyHappinessSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Purrelyly, PurrelySpSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.Purrely, PurrelySpSummonCheck);

            // ============================================================
            // TIER 10: Counter Traps & Setting Backrow
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, DefaultSolemnJudgment);
            AddExecutor(ExecutorType.Activate, CardId.RivalryOfWarlords, RivalryEffect);
            AddExecutor(ExecutorType.Activate, CardId.RedReboot, RedRebootEffect);
            AddExecutor(ExecutorType.Activate, CardId.DimensionalBarrier, DimensionalBarrierEffect);

            AddExecutor(ExecutorType.SpellSet, CardId.Purrelyeap, PurrelyeapSetCondition);
            AddExecutor(ExecutorType.SpellSet, CardId.DimensionalBarrier);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnJudgment);
            AddExecutor(ExecutorType.SpellSet, CardId.RivalryOfWarlords);

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
            _plumpAttachUsed = false;
            _noirBounceUsed = false;
            _beautyNegateUsed = false;
            _purrelyeapUsed = false;
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
            if (DefaultAshBlossomAndJoyousSpring())
            {
                _handTrapsUsedThisTurn++;
                DecisionTracer.TraceActivate("AshBlossom", "Negating opponent search/SS from deck");
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
            if (!SmartHandTrapChain()) return false;
            if (Enemy.GetMonsterCount() >= 3 || Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Attack >= 2500))
            {
                DecisionTracer.TraceActivate("Nibiru", "Wiping opponent large board");
                return true;
            }
            return false;
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
            // NEVER banish 10 face-down in Purrely! Key 1-ofs are essential for combos.
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
            // Effect 0: Search non-Quick-Play Purrely card from Deck
            if (ActivateDescription == Util.GetStringId(CardId.Purrelyly, 0) || (ActivateDescription == -1 && !_purrelylySearchUsed))
            {
                if (_purrelylySearchUsed) return false;
                _purrelylySearchUsed = true;

                // Priority: My Friend > Stray Street > Purrelyeap > Purrely
                if (!Bot.HasInSpellZone(CardId.MyFriendPurrely) && !Bot.HasInHand(CardId.MyFriendPurrely) && Bot.GetRemainingCount(CardId.MyFriendPurrely, 3) > 0)
                    AI.SelectCard(CardId.MyFriendPurrely);
                else if (!Bot.HasInSpellZone(CardId.StrayPurrelyStreet) && !Bot.HasInHand(CardId.StrayPurrelyStreet) && Bot.GetRemainingCount(CardId.StrayPurrelyStreet, 3) > 0)
                    AI.SelectCard(CardId.StrayPurrelyStreet);
                else if (!Bot.HasInSpellZone(CardId.Purrelyeap) && !Bot.HasInHand(CardId.Purrelyeap) && Bot.GetRemainingCount(CardId.Purrelyeap, 2) > 0)
                    AI.SelectCard(CardId.Purrelyeap);
                else
                    AI.SelectCard(CardId.Purrely);

                DecisionTracer.TraceActivate("Purrelyly", "Searching non-Quick-Play Purrely card");
                return true;
            }

            // Effect 1: Target GY Quick-Play -> Xyz Summon
            if (ActivateDescription == Util.GetStringId(CardId.Purrelyly, 1) || ActivateDescription == -1)
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
            // Effect 0: Excavate top 3 cards
            if (ActivateDescription == Util.GetStringId(CardId.Purrely, 0) || (ActivateDescription == -1 && !_purrelyExcavateUsed))
            {
                if (_purrelyExcavateUsed) return false;
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

            // Effect 1: Reveal Quick-Play in Hand -> Xyz Summon
            if (ActivateDescription == Util.GetStringId(CardId.Purrely, 1) || ActivateDescription == -1)
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
            if (ShouldStopExtending()) return false;

            // RULE 1: If we have a Normal Summonable starter in hand and haven't Normal Summoned yet, DO NOT ACTIVATE MEMORY SPELLS!
            if (!_normalSummonUsed && Bot.Hand.Any(h => h.IsCode(CardId.Purrely, CardId.Purrelyly)))
                return false;

            // RULE 2: If we have a Rank 2 Purrely on field with < 5 materials: attach and feed!
            var rank2 = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Rank == 2 && PurrelyXyz.Contains(c.Id));
            if (rank2 != null && GetOverlayCount(rank2) < 5)
            {
                return true;
            }

            // RULE 3: If we have Purrelyly on field and haven't Xyz'd yet, and GY has no Memory Spells:
            var purrelyly = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.Purrelyly) && !_purrelylyXyzUsed);
            if (purrelyly != null && !Bot.Graveyard.Any(c => c != null && MemorySpells.Contains(c.Id)))
            {
                return true;
            }

            // RULE 4: If we have NO monsters on field and Normal Summon is used (or no starter in hand):
            if (Bot.GetMonsterCount() == 0 && (_normalSummonUsed || !Bot.Hand.Any(h => h.IsCode(CardId.Purrely, CardId.Purrelyly))))
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

                // Target OUR monster first to give it battle immunity!
                var target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup()) ??
                             Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup());
                if (target == null) return false;

                AI.SelectCard(target);
                DecisionTracer.TraceActivate("PurrelyDeliciousMemory", $"Targeting {target.Name} for battle immunity & feeding Plump");
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
            // Only summon Typhon if enemy has a monster with >= 3000 ATK (e.g. Hexstia, Blue-Eyes, Dark Matter)
            // and we don't have a 5+ material Noir!
            var noir = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.ExpurrelyNoir));
            if (noir != null && GetOverlayCount(noir) >= 5) return false;

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
            // ONLY summon Downerd if ZEUS is in Extra Deck and ready to overlay immediately!
            if (!Bot.ExtraDeck.Any(e => e.IsCode(CardId.DivineArsenalAAZEUSSkyThunder))) return false;
            var noir = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.ExpurrelyNoir));
            if (noir != null && GetOverlayCount(noir) >= 5) return false;
            if (Enemy.GetMonsterCount() + Enemy.GetSpellCount() < 2) return false;

            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Rank == 2);
        }

        private bool ZeusSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Duel.Phase != DuelPhase.Main2) return false;
            var noir = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.ExpurrelyNoir));
            if (noir != null && GetOverlayCount(noir) >= 5) return false;
            return Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 2;
        }

        private bool ZeusEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (GetOverlayCount(Card) < 2) return false;
            if (Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 2 || Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= 2500))
            {
                DecisionTracer.TraceActivate("AA-ZEUS", "Wiping entire field with Zeus Quick Effect!");
                return true;
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

            // CRITICAL IMMUNITY LOCK:
            // If mats is 5 or 6: detaching 2 will drop us to 3 or 4, LOSING TOWER IMMUNITY!
            // Since Noir is ALREADY immune to all opponent activated effects while mats >= 5:
            // DO NOT DETACH IF mats < 7 unless:
            // 1. We are in Battle Phase and an enemy monster with >= 3000 ATK is about to destroy us by battle!
            // 2. OR on our turn we need to spin an opponent floodgate/boss to push for lethal.
            if (mats >= 5 && mats < 7)
            {
                if (Duel.Player == 1)
                {
                    if (Duel.Phase != DuelPhase.Battle) return false; // STAY IMMUNE IN MAIN PHASE!
                    var battleThreat = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsAttack() && c.Attack >= Card.Defense && !c.IsShouldNotBeTarget());
                    if (battleThreat == null) return false; // No monster can beat our DEF, stay immune!
                    AI.SelectCard(battleThreat);
                    DecisionTracer.TraceActivate("ExpurrelyNoir", $"Emergency spin on battle threat {battleThreat.Name} before damage calculation");
                    return true;
                }
                else
                {
                    var threat = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && (c.HasType(CardType.Continuous) || c.HasType(CardType.Field) || c.IsTrap()) && !c.IsShouldNotBeTarget()) ??
                                 Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Attack >= 2500 && !c.IsShouldNotBeTarget());
                    if (threat == null) return false; // Stay immune!
                    AI.SelectCard(threat);
                    DecisionTracer.TraceActivate("ExpurrelyNoir", $"Main Phase spin on threat {threat.Name}");
                    return true;
                }
            }

            // If mats < 5 (we already lost tower immunity): only spin if we have valid field targets
            if (mats < 5)
            {
                var target = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget()) ??
                             Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget()).OrderByDescending(c => c.Attack).FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
                return false;
            }

            // If mats >= 7: WE HAVE FREE MATERIALS TO BURN! Spin opponent's best cards freely!
            if (mats >= 7)
            {
                var target = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && (c.HasType(CardType.Field) || c.HasType(CardType.Continuous) || c.IsTrap()) && !c.IsShouldNotBeTarget()) ??
                             Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget()).OrderByDescending(c => (c.IsDisabled() ? 0 : 6000) + (c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link) ? 4000 : 0) + c.Attack).FirstOrDefault() ??
                             Enemy.GetMonsters().FirstOrDefault(c => c != null && !c.IsShouldNotBeTarget()) ??
                             Enemy.GetSpells().FirstOrDefault(c => c != null && !c.IsShouldNotBeTarget()) ??
                             Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && c.Attack >= 2000);

                if (target != null)
                {
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("ExpurrelyNoir", $"Surplus spin (Mats: {mats}) targeting {target.Name}");
                    return true;
                }
            }

            return false;
        }

        private bool EpurrelyPlumpEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Trigger 1: When a Purrely Quick-Play Spell is activated -> Attach it from field to Plump!
            if (ActivateDescription == Util.GetStringId(CardId.EpurrelyPlump, 1) || (Duel.LastChainPlayer == 0 && MemorySpells.Contains(Util.GetLastChainCard()?.Id ?? 0)))
            {
                DecisionTracer.TraceActivate("EpurrelyPlump", "Attaching activated Quick-Play Spell to Plump!");
                return true;
            }

            // Effect 0: Ignition / Quick Effect to attach up to 2 Spells/Traps from GYs
            if (ActivateDescription == Util.GetStringId(CardId.EpurrelyPlump, 0) || ActivateDescription == -1)
            {
                if (_plumpAttachUsed) return false;

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
                    _plumpAttachUsed = true;
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
            if (ActivateDescription == Util.GetStringId(CardId.EpurrelyNoir, 1) || (Duel.LastChainPlayer == 0 && MemorySpells.Contains(Util.GetLastChainCard()?.Id ?? 0)))
            {
                DecisionTracer.TraceActivate("EpurrelyNoir", "Attaching activated Quick-Play + Setting Purrelyeap from Deck!");
                return true;
            }

            // Effect 0: Discard 1 to bounce 1-2 opponent cards
            if (ActivateDescription == Util.GetStringId(CardId.EpurrelyNoir, 0) || ActivateDescription == -1)
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
            if (ActivateDescription == Util.GetStringId(CardId.EpurrelyBeauty, 1) || (Duel.LastChainPlayer == 0 && MemorySpells.Contains(Util.GetLastChainCard()?.Id ?? 0)))
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
                if (Duel.Player == 0 && Duel.Phase != DuelPhase.End) return false;

                var rank2Xyz = Bot.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && c.Rank == 2 && PurrelyXyz.Contains(c.Id))
                    .OrderByDescending(c => GetOverlayCount(c))
                    .FirstOrDefault();

                if (rank2Xyz != null)
                {
                    int mats = GetOverlayCount(rank2Xyz);

                    // CRITICAL RANK-UP CONDITIONS:
                    // 1. rank2 has >= 4 materials (so Noir will have >= 5 materials and FULL TOWER IMMUNITY!)
                    // 2. OR our rank2 is being targeted by opponent's removal card (chain dodge!)
                    bool hasTowerMats = (mats >= 4);
                    bool isTargetedByEnemy = Duel.LastChainPlayer == 1 && Util.GetLastChainCard()?.Controller == 1;
                    bool shouldRankUp = hasTowerMats || isTargetedByEnemy;

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
            // If we have Purrely Happy Memory in hand and no cards on field: set trap first so Happy Memory has a target!
            if (Bot.GetMonsterCount() == 0 && Bot.GetSpellCount() == 0 && Bot.Hand.Any(h => h.IsCode(CardId.PurrelyHappyMemory)))
            {
                return true;
            }
            if (!Util.IsTurn1OrMain2()) return false;
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
            if (Card.IsCode(CardId.ExpurrelyNoir))
            {
                // If Expurrely Noir has < 3000 ATK and is in Attack position -> switch to Defense (2800+ DEF)
                if (Card.IsAttack() && Card.Attack < 3000 && !CanDealLethal())
                    return true;
                // If in Defense and has >= 3000 ATK and we can attack -> switch to Attack
                if (Card.IsDefense() && Card.Attack >= 3000 && Duel.Player == 0 && Duel.Phase == DuelPhase.Main1)
                    return true;
                return false;
            }

            if (Card.IsCode(CardId.EpurrelyPlump, CardId.EpurrelyNoir))
            {
                if (Card.IsAttack()) return true; // Switch to Defense
                return false;
            }

            return DefaultMonsterRepos();
        }

        // ============================================================
        // COMPLETE HOOK OVERRIDES (OnSelectYesNo, OnSelectEffectYn, OnSelectCard, OnSelectOption, OnSelectPosition)
        // ============================================================

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            if (card != null)
            {
                // Always activate Purrelyly search on summon
                if (card.IsCode(CardId.Purrelyly)) return true;
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
            // Plump optional banish: "Banish 1 monster on the field until the End Phase?"
            if (desc == Util.GetStringId(CardId.EpurrelyPlump, 2))
            {
                return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget());
            }

            return true;
        }

        public override int OnSelectOption(IList<long> options)
        {
            return base.OnSelectOption(options);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // If selecting Rank 2 for Expurrely Noir Xyz rank-up:
            if (cards.Any(c => c != null && c.Rank == 2 && GetOverlayCount(c) >= 5))
            {
                var bestRank2 = cards.Where(c => c != null && c.Rank == 2).OrderByDescending(c => GetOverlayCount(c)).FirstOrDefault();
                if (bestRank2 != null)
                {
                    return new[] { bestRank2 };
                }
            }

            // Hint 503: Banish (e.g. Plump optional banish trigger)
            if (hint == 503)
            {
                var oppMonsters = cards.Where(c => c != null && c.Controller == 1 && c.IsFaceup()).OrderByDescending(c => c.Attack).ToList();
                if (oppMonsters.Count > 0)
                {
                    return Util.CheckSelectCount(oppMonsters, cards, min, max);
                }
                if (cancelable) return null;
                var safeTargets = cards.Where(c => c != null && !c.IsCode(CardId.EpurrelyPlump, CardId.ExpurrelyNoir)).OrderBy(c => c.Attack).ToList();
                if (safeTargets.Count > 0) return Util.CheckSelectCount(safeTargets, cards, min, max);
            }

            // Hint 502 / 507: Destruction / Removal Target / Spin to Deck Target
            if (hint == 502 || hint == 507)
            {
                var sorted = cards.OrderByDescending(c => {
                    if (c == null) return -999;
                    int score = (c.Controller == 1) ? 10000 : 0;
                    if (c.Location == CardLocation.MonsterZone || c.Location == CardLocation.SpellZone) score += 5000;
                    if (c.IsSpell() || c.IsTrap())
                    {
                        return score + (c.IsFaceup() ? 6000 : 1000);
                    }
                    if (c.IsMonster())
                    {
                        if (c.IsFaceup() && !c.IsDisabled()) score += 5000;
                        if (c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link)) score += 3000;
                        return score + c.Attack;
                    }
                    return score;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // Hint 506: Search / Add to hand / Reveal selection
            if (hint == 506)
            {
                var sorted = cards.OrderByDescending(c => {
                    if (c == null) return -999;
                    if (c.Id == CardId.MyFriendPurrely && !Bot.HasInSpellZone(CardId.MyFriendPurrely)) return 100;
                    if (c.Id == CardId.PurrelyDeliciousMemory && !Bot.HasInHand(CardId.PurrelyDeliciousMemory)) return 90;
                    if (c.Id == CardId.PurrelySleepyMemory) return 80;
                    if (c.Id == CardId.StrayPurrelyStreet && !Bot.HasInSpellZone(CardId.StrayPurrelyStreet)) return 70;
                    if (c.Id == CardId.Purrelyly && !Bot.HasInHand(CardId.Purrelyly)) return 65;
                    if (c.Id == CardId.Purrelyeap && !Bot.HasInSpellZone(CardId.Purrelyeap) && !Bot.HasInHand(CardId.Purrelyeap)) return 60;
                    if (c.Id == CardId.Purrely) return 50;
                    if (c.Id == CardId.PurrelyPrettyMemory) return 40;
                    if (c.Id == CardId.PurrelyHappyMemory) return 30;
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
                    if (MemorySpells.Contains(c.Id) && cards.Count(h => h.Id == c.Id) > 1) return 10;
                    if (c.IsCode(CardId.PurrelyHappyMemory)) return 15;
                    if (c.IsCode(CardId.PurrelyPrettyMemory)) return 18;
                    if (c.IsCode(CardId.PurrelySleepyMemory)) return 22;
                    if (c.IsCode(CardId.PurrelyDeliciousMemory)) return 25;
                    if (c.IsCode(CardId.StrayPurrelyStreet) && Bot.HasInSpellZone(CardId.StrayPurrelyStreet)) return 30;
                    if (c.IsCode(CardId.MyFriendPurrely) && Bot.HasInSpellZone(CardId.MyFriendPurrely)) return 32;
                    if (c.IsCode(CardId.Purrelyeap) && (Bot.HasInSpellZone(CardId.Purrelyeap) || cards.Count(h => h.Id == CardId.Purrelyeap) > 1)) return 35;
                    if (c.IsCode(CardId.Purrely, CardId.Purrelyly) && cards.Count(h => c.IsCode(h.Id)) > 1) return 40;
                    if (c.IsCode(CardId.Purrelyeap)) return 45;
                    if (c.IsCode(CardId.Purrely, CardId.Purrelyly)) return 50;
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
                    // Main deck starters
                    if (c.Id == CardId.Purrelyly && !_purrelylySearchUsed) return 100;
                    if (c.Id == CardId.Purrely && !_purrelyExcavateUsed) return 90;
                    if (c.Id == CardId.Purrelyly) return 80;
                    if (c.Id == CardId.Purrely) return 70;
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

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Defense Position Monsters (High DEF / Wall / Non-attacker):
            // 1. Expurrely Noir (1100 ATK / 2800 DEF - Tower Wall!)
            // 2. Epurrely Plump (200 ATK / 2100 DEF - Wall while loading materials!)
            // 3. Epurrely Noir (1000 ATK / 1000 DEF)
            // 4. Purrely / Purrelyly (100-200 ATK/DEF)
            // 5. Hand traps
            int[] defenseMonsters = {
                CardId.ExpurrelyNoir,
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

            if (defenseMonsters.Contains(cardId) && positions.Contains(CardPosition.FaceUpDefence))
            {
                if (cardId == CardId.ExpurrelyNoir && CanDealLethal())
                {
                    return CardPosition.FaceUpAttack;
                }
                return CardPosition.FaceUpDefence;
            }

            return base.OnSelectPosition(cardId, positions);
        }
    }
}
