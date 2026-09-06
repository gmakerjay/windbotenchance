using YGOSharp.OCGWrapper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    // ====================================================================================================
    // CARD AUDIT — GOD-01 (Modern Egyptian God Engine: Slifer, Obelisk, Ra, Slimes, Soul Crossing, Super Poly, Droplet)
    // | Card Name                     | Type      | OPT? | Cost        | Effect                     | Activate When            | NEVER When           |
    // |-------------------------------|-----------|------|-------------|----------------------------|--------------------------|----------------------|
    // | Slifer the Sky Dragon         | Monster   | No   | 3 Tributes  | +1000/card, -2000 on opp SS| In hand, 3 tributes ready| Tributes < 3         |
    // | Obelisk the Tormentor         | Monster   | No   | 3 Tributes  | 4000 ATK, Trib 2 wipe board| In hand, 3 tributes ready| Tributes < 3         |
    // | The Winged Dragon of Ra       | Monster   | No   | 3 Tributes  | Pay LP->ATK, 1000 pop mon  | In hand, 3 tributes ready| Tributes < 3         |
    // | Ra - Sphere Mode              | Monster   | No   | 3 Opp Tribs | SS to opp field / SS 4k Ra | Opp has 3+ mons / On field| Opp mons < 3         |
    // | Ra - Immortal Phoenix         | Monster   | No   | 1000 LP     | 4000 Unaffected, Send GY   | Ra sent to GY / Main Phase| Already on field     |
    // | Guardian Slime                | Monster   | HOPT | None        | SS on damage, Trib->EG Slm | Damage taken / On field  | Zone full            |
    // | Reactor Slime                 | Monster   | HOPT | None        | SS 2 Slime Tokens, Set MRS | Main Phase / Battle Phase| Cannot SS            |
    // | Metal Reflect Slime           | Trap/Mon  | No   | None        | SS Lv10 0 ATK 3000 DEF Mon | Opp turn / Set on field  | Zone full            |
    // | Egyptian God Slime            | Fusion/Mon| No   | Trib 1 Slm  | Counts as 3 Tributes, 3kDEF| Extra Deck, Slime on field| No Slime on field    |
    // | Soul Crossing                 | QuickSp   | HOPT | None        | Trib 1-3 Opp Mons for God  | Opp has monsters / Hand  | No God ready         |
    // | Ancient Chant                 | Spell     | HOPT | None        | Search Ra, +1 Trib Sum, ATK| Main Phase               | Already used OPT     |
    // | The True Sun God              | ContSpell | HOPT | None        | Search Ra/Support, Dump GY | Main Phase               | Already active       |
    // | Millennium Revelation         | ContSpell | HOPT | Discard God | Search Monster Reborn, Ra  | Hand has God / Main Phase| Already used OPT     |
    // | The Breaking Ruin God         | QuickSp   | HOPT | None        | SS Obelisk from Hand/GY imm| Obelisk in Hand/GY       | No Obelisk in Hand/GY|
    // | The Revived Sky God           | Trap      | HOPT | None        | SS Slifer from GY + draw 6 | Slifer in GY             | Slifer not in GY     |
    // | Soul Energy MAX!!!            | Trap      | HOPT | Tribute 2   | Wipe opp field + 4000 burn | Obelisk on field         | Monsters < 2         |
    // | Fist of Fate                  | QuickSp   | HOPT | None        | Negate mon + pop all S/T   | Obelisk on field         | No Obelisk on field  |
    // | Super Polymerization          | QuickSp   | No   | Discard 1   | Uncounterable Fusion       | Opp has 2+ valid monsters| No target in Extra   |
    // | Forbidden Droplet             | QuickSp   | No   | Send cards  | Uncounterable Negate & Half| Opp effect mon on field  | No cost available    |
    // | Mound of the Bound Creator    | FieldSp   | No   | None        | Lv10+ Target/Dest immunity | Main Phase               | Already active       |
    // | Foolish Burial                | Spell     | No   | None        | Send Slime/God from Deck   | Main Phase               | GY target redundant  |
    // | Pot of Prosperity             | Spell     | HOPT | Banish 3-6  | Excavate 3-6, add 1        | Main Phase               | Already used OPT     |
    // | Harpie's Feather Duster       | Spell     | No   | None        | Destroy all opp S/T        | Opp has S/T              | Opp S/T == 0         |
    // | Ash Blossom & Joyous Spring   | Monster   | HOPT | Discard     | Negate deck search/ss/dump | Opponent searches/draws  | Chain already blocked|
    // | Maxx "C"                      | Monster   | HOPT | Discard     | Draw each time opp SS      | Opponent turn / Main     | Already active       |
    // | Called by the Grave           | QuickSp   | HOPT | None        | Banish & negate GY monster | Opponent activates GY/HT | No enemy target      |
    // | Gustav Max                    | Xyz       | HOPT | Detach 1    | 2000 Burn Damage           | 2x Lv10 monsters on field| Lethal / MP2         |
    // | Juggernaut Liebe              | Xyz       | No   | Detach 1    | 6000 ATK Multi-Attacker    | Overlay on Gustav/Dora   | Battle Phase lethal  |
    // | AA-ZEUS                       | Xyz       | No   | Detach 2    | Send all other cards to GY | After Xyz battled        | Board already clear  |
    // ====================================================================================================

    [Deck("GOD-01", "GOD-01")]
    public class _2026_God01Executor : ModernExecutor
    {
        public class CardId
        {
            // Egyptian God Bosses
            public const int ObeliskTheTormentor = 10000002;
            public const int TheWingedDragonOfRaImmortalPhoenix = 10000090;
            public const int TheWingedDragonOfRa = 10000010;
            public const int SliferTheSkyDragon = 10000022;
            public const int TheWingedDragonOfRaSphereMode = 10000080;

            // Slime Tribute Engine
            public const int GuardianSlime = 15771991;
            public const int ReactorSlime = 79387392;
            public const int MetalReflectSlime = 26905245;
            public const int SlimeToken = 79387393;
            public const int EgyptianGodSlime = 42166000;

            // God Support Spells & Traps
            public const int SoulCrossing = 5253985;
            public const int AncientChant = 78665705;
            public const int TheTrueSunGod = 11587414;
            public const int MillenniumRevelation = 41044418;
            public const int TheBreakingRuinGod = 85182315;
            public const int TheRevivedSkyGod = 59094601;
            public const int SoulEnergyMax = 79339613;
            public const int FistOfFate = 79868386;
            public const int MoundOfTheBoundCreator = 269012;
            public const int MonsterReborn = 83764718;

            // Board Breakers & Staples
            public const int SuperPolymerization = 48130397;
            public const int ForbiddenDroplet = 24299458;
            public const int FoolishBurial = 81439173;
            public const int AshBlossom = 14558127;
            public const int MaxxC = 23434538;
            public const int CalledByTheGrave = 24224830;
            public const int PotOfProsperity = 84211599;
            public const int HarpiesFeatherDuster = 18144506;
            public const int LightningStorm = 14532163;
            public const int DarkRulerNoMore = 54693926;

            // Extra Deck Fusion & Xyz Bosses
            public const int MudragonOfTheSwamp = 54757758;
            public const int StarvingVenomFusionDragon = 41209827;
            public const int PredaplantDragostapelia = 69946549;
            public const int GustavMax = 56910167;
            public const int JuggernautLiebe = 26096328;
            public const int SuperDora = 49032236;
            public const int Zeus = 90448279;
            public const int KnightmarePhoenix = 2857636;
            public const int KnightmareUnicorn = 38342335;
            public const int Linkuriboh = 41999284;
        }

        // Standard OCG Hint IDs
        private const long HINT_SELECT_FACEUP = 500;
        private const long HINT_SELECT_TOGRAVE = 501;
        private const long HINT_SELECT_DESTROY = 502;
        private const long HINT_SELECT_DISCARD = 504;
        private const long HINT_SELECT_TOHAND = 506;
        private const long HINT_SELECT_SPSUMMON = 509;
        private const long HINT_SELECT_FUSION_MAT = 511;
        private const long HINT_SELECT_BANISH = 512;
        private const long HINT_SELECT_SET = 514;
        private const long HINT_SELECT_DETACH = 519;

        private bool _ancientChantUsedThisTurn = false;
        private bool _trueSunGodUsedThisTurn = false;
        private bool _prosperityUsedThisTurn = false;
        private bool _soulCrossingUsedThisTurn = false;
        private bool _breakingRuinGodUsedThisTurn = false;
        private bool _revivedSkyGodUsedThisTurn = false;
        private bool _millenniumRevelationUsedThisTurn = false;
        private bool _maxxCActivatedThisTurn = false;
        private bool _superPolyUsedThisTurn = false;

        public _2026_God01Executor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // ── 1. Register Ace Cards in ResourcePlanner ──
            ResourcePlan.RegisterAceCards(
                CardId.SliferTheSkyDragon,
                CardId.ObeliskTheTormentor,
                CardId.TheWingedDragonOfRaImmortalPhoenix,
                CardId.TheWingedDragonOfRa,
                CardId.TheWingedDragonOfRaSphereMode,
                CardId.EgyptianGodSlime,
                CardId.StarvingVenomFusionDragon,
                CardId.PredaplantDragostapelia,
                CardId.MudragonOfTheSwamp,
                CardId.JuggernautLiebe,
                CardId.GustavMax,
                CardId.SuperDora,
                CardId.Zeus
            );

            // ── 2. Register Starters and Baits in BaitPlanner ──
            BaitPlanner.RegisterComboStarters(
                CardId.PotOfProsperity,
                CardId.FoolishBurial,
                CardId.TheTrueSunGod,
                CardId.AncientChant,
                CardId.MillenniumRevelation,
                CardId.GuardianSlime,
                CardId.ReactorSlime,
                CardId.SoulCrossing,
                CardId.SuperPolymerization,
                CardId.TheBreakingRuinGod
            );
            BaitPlanner.RegisterBaitCards(
                CardId.HarpiesFeatherDuster,
                CardId.MoundOfTheBoundCreator
            );

            // ── 3. Register Combo Lines ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Slime-EgyptianGodSlime-GodSummon",
                RequiredCards = new List<int> { CardId.ReactorSlime, CardId.SliferTheSkyDragon },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.ReactorSlime, ActionType = ExecutorType.Summon, Description = "Normal Summon Reactor Slime" },
                    new() { CardId = CardId.EgyptianGodSlime, ActionType = ExecutorType.SpSummon, Description = "Special Summon Egyptian God Slime (3 Tributes)" },
                    new() { CardId = CardId.SliferTheSkyDragon, ActionType = ExecutorType.Summon, Description = "Tribute Summon Slifer using Egyptian God Slime" }
                },
                FallbackLineName = "Ancient-Chant-Line"
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Ancient-Chant-Ra-Boost",
                RequiredCards = new List<int> { CardId.AncientChant, CardId.ReactorSlime },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.AncientChant, ActionType = ExecutorType.Activate, Description = "Search Ra & gain extra Tribute Summon" },
                    new() { CardId = CardId.ReactorSlime, ActionType = ExecutorType.Summon, Description = "Normal Summon Reactor Slime" },
                    new() { CardId = CardId.EgyptianGodSlime, ActionType = ExecutorType.SpSummon, Description = "Special Summon Egyptian God Slime" },
                    new() { CardId = CardId.TheWingedDragonOfRa, ActionType = ExecutorType.Summon, Description = "Tribute Summon Ra with boosted ATK" }
                },
                FallbackLineName = "Breaking-Ruin-Line"
            });

            // ═══════════════════════════════════════════════════════════════
            //  EXECUTORS PIPELINE (Tiered Priority)
            // ═══════════════════════════════════════════════════════════════

            // ── Tier 0: Quick Negates, Handtraps & Reactive Disruptions ──
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletEffect);
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationEffect);
            AddExecutor(ExecutorType.Activate, CardId.FistOfFate, FistOfFateEffect);
            AddExecutor(ExecutorType.Activate, CardId.SoulEnergyMax, SoulEnergyMaxEffect);
            AddExecutor(ExecutorType.Activate, CardId.SoulCrossing, SoulCrossingEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheRevivedSkyGod, RevivedSkyGodEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheBreakingRuinGod, BreakingRuinGodEffect);
            AddExecutor(ExecutorType.Activate, CardId.GuardianSlime, GuardianSlimeDamageEffect);
            AddExecutor(ExecutorType.Activate, CardId.MetalReflectSlime, MetalReflectSlimeEffect);

            // ── Tier 1: Board Breakers (Harpie's Feather Duster, Lightning Storm, Dark Ruler, Sphere Mode) ──
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, HarpiesDusterEffect);
            AddExecutor(ExecutorType.Activate, CardId.LightningStorm, LightningStormEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarkRulerNoMore, DarkRulerNoMoreEffect);
            AddExecutor(ExecutorType.Summon, CardId.TheWingedDragonOfRaSphereMode, SphereModeOpponentSummon);
            AddExecutor(ExecutorType.Activate, CardId.TheWingedDragonOfRaSphereMode, SphereModeOurEffect);

            // ── Tier 2: Setup Spells, Search & Consistency ──
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, PotOfProsperityEffect);
            AddExecutor(ExecutorType.Activate, CardId.FoolishBurial, FoolishBurialEffect);
            AddExecutor(ExecutorType.Activate, CardId.MoundOfTheBoundCreator, MoundEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheTrueSunGod, TrueSunGodEffect);
            AddExecutor(ExecutorType.Activate, CardId.MillenniumRevelation, MillenniumRevelationEffect);
            AddExecutor(ExecutorType.Activate, CardId.AncientChant, AncientChantEffect);
            AddExecutor(ExecutorType.Activate, CardId.MonsterReborn, MonsterRebornEffect);

            // ── Tier 3: Extra Deck 3-Tribute Body: Egyptian God Slime ──
            AddExecutor(ExecutorType.SpSummon, CardId.EgyptianGodSlime, EgyptianGodSlimeSummon);

            // ── Tier 3.5: Rank 10 / Rank 11 Xyz OTK Finisher (Gustav Max -> Juggernaut Liebe) ──
            AddExecutor(ExecutorType.SpSummon, CardId.GustavMax, GustavMaxSummon);
            AddExecutor(ExecutorType.Activate, CardId.GustavMax, GustavMaxEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.JuggernautLiebe, JuggernautLiebeSummon);
            AddExecutor(ExecutorType.Activate, CardId.JuggernautLiebe, JuggernautLiebeEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.SuperDora, SuperDoraSummon);
            AddExecutor(ExecutorType.Activate, CardId.SuperDora, SuperDoraEffect);

            // ── Tier 4: God Ignition & Trigger Effects ──
            AddExecutor(ExecutorType.Activate, CardId.StarvingVenomFusionDragon, StarvingVenomEffect);
            AddExecutor(ExecutorType.Activate, CardId.PredaplantDragostapelia, DragostapeliaEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheWingedDragonOfRaImmortalPhoenix, ImmortalPhoenixEffect);
            AddExecutor(ExecutorType.Activate, CardId.ObeliskTheTormentor, ObeliskEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheWingedDragonOfRa, RaEffect);

            // ── Tier 5: Tribute Engine Starters ──
            AddExecutor(ExecutorType.Summon, CardId.ReactorSlime, ReactorSlimeSummon);
            AddExecutor(ExecutorType.Activate, CardId.ReactorSlime, ReactorSlimeEffect);
            AddExecutor(ExecutorType.MonsterSet, CardId.GuardianSlime, DefensiveMonsterSet);

            // ── Tier 6: Egyptian God Tribute Summons ──
            AddExecutor(ExecutorType.Summon, CardId.SliferTheSkyDragon, SliferSummon);
            AddExecutor(ExecutorType.Summon, CardId.ObeliskTheTormentor, ObeliskSummon);
            AddExecutor(ExecutorType.Summon, CardId.TheWingedDragonOfRa, RaSummon);

            // ── Tier 7: Extra Deck Utility (Zeus & Link Monsters) ──
            AddExecutor(ExecutorType.SpSummon, CardId.Zeus, ZeusSummon);
            AddExecutor(ExecutorType.Activate, CardId.Zeus, ZeusEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Linkuriboh, LinkuribohSummon);
            AddExecutor(ExecutorType.Activate, CardId.Linkuriboh, LinkuribohEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmarePhoenix, KnightmarePhoenixSummon);
            AddExecutor(ExecutorType.Activate, CardId.KnightmarePhoenix, KnightmarePhoenixEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmareUnicorn, KnightmareUnicornSummon);
            AddExecutor(ExecutorType.Activate, CardId.KnightmareUnicorn, KnightmareUnicornEffect);

            // ── Tier 8: Spell / Trap Sets ──
            AddExecutor(ExecutorType.SpellSet, CardId.MetalReflectSlime, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.SuperPolymerization, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.SoulCrossing, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.TheRevivedSkyGod, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.TheBreakingRuinGod, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.SoulEnergyMax, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.FistOfFate, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, DefaultSpellSet);

            // ── Tier 9: Monster Reposition ──
            AddExecutor(ExecutorType.Repos, MonsterReposOverride);
        }

        public override bool OnSelectHand()
        {
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _ancientChantUsedThisTurn = false;
            _trueSunGodUsedThisTurn = false;
            _prosperityUsedThisTurn = false;
            _soulCrossingUsedThisTurn = false;
            _breakingRuinGodUsedThisTurn = false;
            _revivedSkyGodUsedThisTurn = false;
            _millenniumRevelationUsedThisTurn = false;
            _maxxCActivatedThisTurn = false;
            _superPolyUsedThisTurn = false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  ACE CARD & TRIBUTE PRIORITY
        // ═══════════════════════════════════════════════════════════════

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.SliferTheSkyDragon
                || card.Id == CardId.ObeliskTheTormentor
                || card.Id == CardId.TheWingedDragonOfRaImmortalPhoenix
                || card.Id == CardId.TheWingedDragonOfRa
                || card.Id == CardId.TheWingedDragonOfRaSphereMode
                || card.Id == CardId.EgyptianGodSlime
                || card.Id == CardId.StarvingVenomFusionDragon
                || card.Id == CardId.PredaplantDragostapelia
                || card.Id == CardId.MudragonOfTheSwamp
                || card.Id == CardId.JuggernautLiebe
                || card.Id == CardId.GustavMax
                || card.Id == CardId.SuperDora
                || card.Id == CardId.Zeus;
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (c.Controller == 1) return 1; // Always tribute opponent's monsters first!
            if (c.Id == CardId.SlimeToken) return 2; // Tokens first
            if (c.Id == CardId.EgyptianGodSlime) return 3; // Egyptian God Slime counts as 3 tributes!
            if (c.Id == CardId.GuardianSlime) return 4;
            if (c.Id == CardId.MetalReflectSlime) return 5;
            if (c.Id == CardId.ReactorSlime) return 6;
            if (c.Id == CardId.Linkuriboh) return 7;
            if (IsAceCard(c)) return 900; // Protect true Gods
            return base.GetMaterialPriority(c);
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 0: REACTIVE DISRUPTIONS, DROPLET & HANDTRAPS
        // ═══════════════════════════════════════════════════════════════

        private bool ForbiddenDropletEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                var chainCard = LastChainCard;
                if (chainCard != null && chainCard.IsMonster() && chainCard.IsFaceup() && !chainCard.IsDisabled())
                    return true;
            }
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep || Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
            {
                var oppEffectMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsDisabled() && c.HasType(CardType.Effect)).ToList();
                if (oppEffectMonsters.Count > 0 && Bot.Hand.Count > 1)
                    return true;
            }
            return false;
        }

        private bool SuperPolymerizationEffect()
        {
            if (_superPolyUsedThisTurn) return false;

            // Check if opponent controls 2+ face-up monsters that can be fused
            var oppMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsDisabled()).ToList();
            if (oppMonsters.Count >= 2)
            {
                int discardable = Bot.Hand.Count(c => c != null && c.Id != CardId.SuperPolymerization);
                if (discardable >= 1)
                {
                    _superPolyUsedThisTurn = true;
                    return true;
                }
            }
            return false;
        }

        private bool StarvingVenomEffect()
        {
            return true;
        }

        private bool DragostapeliaEffect()
        {
            var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled() && c.HasType(CardType.Effect));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool MaxxCEffect()
        {
            if (_maxxCActivatedThisTurn) return false;
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                _maxxCActivatedThisTurn = true;
                return true;
            }
            return DefaultMaxxC();
        }

        private bool AshBlossomEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            ClientCard lastChain = Util.GetLastChainCard();
            if (lastChain == null) return false;
            return true;
        }

        private bool CalledByTheGraveEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                return DefaultCalledByTheGrave();
            }
            return false;
        }

        private bool FistOfFateEffect()
        {
            if (!Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id == CardId.ObeliskTheTormentor))
                return false;

            var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled() && c.HasType(CardType.Effect));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool SoulEnergyMaxEffect()
        {
            if (Card.Location == CardLocation.SpellZone && Card.IsFacedown())
            {
                bool hasObelisk = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id == CardId.ObeliskTheTormentor);
                int otherMons = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Id != CardId.ObeliskTheTormentor);
                if (hasObelisk && otherMons >= 2 && Enemy.GetMonsterCount() > 0)
                {
                    return true;
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                if (!Bot.Hand.Any(c => c != null && c.Id == CardId.ObeliskTheTormentor))
                {
                    AI.SelectCard(CardId.ObeliskTheTormentor);
                    return true;
                }
            }
            return false;
        }

        private bool SoulCrossingEffect()
        {
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2)
                return false;

            if (_soulCrossingUsedThisTurn) return false;

            bool hasGodInHand = Bot.Hand.Any(c => c != null && (c.Id == CardId.SliferTheSkyDragon || c.Id == CardId.ObeliskTheTormentor || c.Id == CardId.TheWingedDragonOfRa));
            if (hasGodInHand)
            {
                int totalAvailable = Bot.GetMonsterCount() + Enemy.GetMonsterCount();
                if (Bot.GetMonsters().Any(c => c != null && c.Id == CardId.EgyptianGodSlime))
                {
                    _soulCrossingUsedThisTurn = true;
                    return true;
                }
                if (totalAvailable >= 3)
                {
                    _soulCrossingUsedThisTurn = true;
                    return true;
                }
                if (Enemy.GetMonsterCount() >= 1 && totalAvailable >= 2)
                {
                    _soulCrossingUsedThisTurn = true;
                    return true;
                }
            }
            return false;
        }

        private bool RevivedSkyGodEffect()
        {
            if (Card.Location == CardLocation.SpellZone && Card.IsFacedown())
            {
                if (!_revivedSkyGodUsedThisTurn && Bot.Graveyard.Any(c => c != null && c.Id == CardId.SliferTheSkyDragon))
                {
                    _revivedSkyGodUsedThisTurn = true;
                    return true;
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }
            return false;
        }

        private bool BreakingRuinGodEffect()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                if (!_breakingRuinGodUsedThisTurn)
                {
                    bool hasObelisk = Bot.Hand.Any(c => c != null && c.Id == CardId.ObeliskTheTormentor)
                                   || Bot.Graveyard.Any(c => c != null && c.Id == CardId.ObeliskTheTormentor);
                    if (hasObelisk)
                    {
                        _breakingRuinGodUsedThisTurn = true;
                        AI.SelectCard(CardId.ObeliskTheTormentor);
                        return true;
                    }
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                if (Enemy.Graveyard.Count(c => c != null && c.IsMonster()) >= 2)
                {
                    return true;
                }
            }
            return false;
        }

        private bool GuardianSlimeDamageEffect()
        {
            if (Card.Location == CardLocation.Hand) return true;
            if (Card.Location == CardLocation.MonsterZone) return true;
            if (Card.Location == CardLocation.Grave)
            {
                AI.SelectCard(CardId.SoulCrossing, CardId.AncientChant, CardId.TheTrueSunGod, CardId.MillenniumRevelation);
                return true;
            }
            return false;
        }

        private bool MetalReflectSlimeEffect()
        {
            if (Bot.GetMonsterCount() < 5)
            {
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 1 & 2: BOARD BREAKERS, SEARCH & SETUP
        // ═══════════════════════════════════════════════════════════════

        private bool HarpiesDusterEffect()
        {
            return Enemy.GetSpellCount() > 0;
        }

        private bool PotOfProsperityEffect()
        {
            if (_prosperityUsedThisTurn) return false;
            _prosperityUsedThisTurn = true;
            return true;
        }

        private bool FoolishBurialEffect()
        {
            if (Bot.Deck.Any(c => c != null && c.Id == CardId.GuardianSlime))
            {
                AI.SelectCard(CardId.GuardianSlime);
                return true;
            }
            if (Bot.Deck.Any(c => c != null && c.Id == CardId.TheWingedDragonOfRaImmortalPhoenix))
            {
                AI.SelectCard(CardId.TheWingedDragonOfRaImmortalPhoenix);
                return true;
            }
            if (Bot.Deck.Any(c => c != null && c.Id == CardId.SliferTheSkyDragon))
            {
                AI.SelectCard(CardId.SliferTheSkyDragon);
                return true;
            }
            return false;
        }

        private bool MillenniumRevelationEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_millenniumRevelationUsedThisTurn) return false;
                _millenniumRevelationUsedThisTurn = true;
                return true;
            }

            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                bool hasGodInHand = Bot.Hand.Any(c => c != null && (c.Id == CardId.SliferTheSkyDragon || c.Id == CardId.ObeliskTheTormentor || c.Id == CardId.TheWingedDragonOfRa));
                bool hasRebornInDeck = Bot.Deck.Any(c => c != null && c.Id == CardId.MonsterReborn) || Bot.Graveyard.Any(c => c != null && c.Id == CardId.MonsterReborn);
                if (hasGodInHand && hasRebornInDeck)
                {
                    return true;
                }

                if (Bot.Hand.Any(c => c != null && c.Id == CardId.MonsterReborn) && Bot.Graveyard.Any(c => c != null && c.Id == CardId.TheWingedDragonOfRa))
                {
                    return true;
                }
            }
            return false;
        }

        private bool MoundEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                AI.SelectCard(CardId.SliferTheSkyDragon, CardId.ObeliskTheTormentor, CardId.TheWingedDragonOfRaSphereMode, CardId.TheWingedDragonOfRa);
                return true;
            }
            return !Bot.HasInSpellZone(CardId.MoundOfTheBoundCreator);
        }

        private bool TrueSunGodEffect()
        {
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                if (Bot.Deck.Any(c => c != null && c.Id == CardId.TheWingedDragonOfRaImmortalPhoenix)
                    && Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id == CardId.TheWingedDragonOfRa))
                {
                    AI.SelectCard(CardId.TheWingedDragonOfRaImmortalPhoenix);
                    return true;
                }
                return false;
            }

            if (!_trueSunGodUsedThisTurn)
            {
                _trueSunGodUsedThisTurn = true;
                AI.SelectCard(CardId.AncientChant, CardId.GuardianSlime, CardId.ReactorSlime, CardId.SoulCrossing, CardId.MillenniumRevelation, CardId.TheWingedDragonOfRa);
                return true;
            }
            return !Bot.HasInSpellZone(CardId.TheTrueSunGod);
        }

        private bool AncientChantEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }

            if (!_ancientChantUsedThisTurn)
            {
                _ancientChantUsedThisTurn = true;
                AI.SelectCard(CardId.TheWingedDragonOfRa);
                return true;
            }
            return false;
        }

        private bool MonsterRebornEffect()
        {
            var target = Bot.Graveyard.FirstOrDefault(c => c != null && (c.Id == CardId.SliferTheSkyDragon || c.Id == CardId.ObeliskTheTormentor || c.Id == CardId.EgyptianGodSlime || c.Id == CardId.GuardianSlime || c.Id == CardId.TheWingedDragonOfRa));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 3 & 4: EGYPTIAN GOD SLIME & GOD MONSTERS
        // ═══════════════════════════════════════════════════════════════

        private bool EgyptianGodSlimeSummon()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.Id == CardId.GuardianSlime || c.Id == CardId.MetalReflectSlime));
        }

        private bool SphereModeOpponentSummon()
        {
            return Enemy.GetMonsterCount() >= 3;
        }

        private bool SphereModeOurEffect()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.Controller == 0)
            {
                AI.SelectCard(CardId.TheWingedDragonOfRa);
                return true;
            }
            return false;
        }

        private bool ImmortalPhoenixEffect()
        {
            if (Card.Location == CardLocation.Grave) return true;

            if (Card.Location == CardLocation.MonsterZone)
            {
                if (Bot.LifePoints > 1000 && Enemy.GetMonsterCount() > 0)
                {
                    var target = Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup());
                    if (target != null)
                    {
                        AI.SelectCard(target);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ObeliskEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                int otherMonsters = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c != Card && !IsAceCard(c));
                if (otherMonsters >= 2 && Enemy.GetMonsterCount() >= 1)
                {
                    return true;
                }
            }
            return false;
        }

        private bool RaEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (LastChainCard != null && LastChainCard.Id == CardId.TheWingedDragonOfRa && Card.Attack <= 0)
                {
                    if (Bot.HasInSpellZone(CardId.MoundOfTheBoundCreator) || Enemy.GetMonsterCount() == 0)
                    {
                        return true;
                    }
                    return false;
                }

                if (Enemy.GetMonsterCount() > 0 && Bot.LifePoints > 1000)
                {
                    var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup());
                    if (target != null)
                    {
                        AI.SelectCard(target);
                        return true;
                    }
                }
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 5 & 6: TRIBUTE ENGINE & GOD SUMMONS
        // ═══════════════════════════════════════════════════════════════

        private bool ReactorSlimeSummon() => true;

        private bool ReactorSlimeEffect()
        {
            if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
            {
                return Bot.GetMonsterCount() <= 3;
            }
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep)
            {
                return true;
            }
            return false;
        }

        private bool DefensiveMonsterSet()
        {
            return Bot.GetMonsterCount() < 4;
        }

        private bool SliferSummon()
        {
            int tributes = GetAvailableTributeCount();
            return tributes >= 3;
        }

        private bool ObeliskSummon()
        {
            int tributes = GetAvailableTributeCount();
            return tributes >= 3;
        }

        private bool RaSummon()
        {
            int tributes = GetAvailableTributeCount();
            return tributes >= 3;
        }

        private int GetAvailableTributeCount()
        {
            int count = 0;
            foreach (var m in Bot.GetMonsters().Where(c => c != null && c.IsFaceup()))
            {
                if (m.Id == CardId.EgyptianGodSlime) count += 3;
                else count += 1;
            }
            return count;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 3.5 & 7: EXTRA DECK BOSSES
        // ═══════════════════════════════════════════════════════════════

        private bool GustavMaxSummon()
        {
            int lv10Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 10 && !c.IsCode(CardId.SliferTheSkyDragon));
            return lv10Count >= 2;
        }

        private bool GustavMaxEffect()
        {
            return true;
        }

        private bool JuggernautLiebeSummon()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.Id == CardId.GustavMax || c.Id == CardId.SuperDora));
        }

        private bool JuggernautLiebeEffect()
        {
            return Duel.Phase == DuelPhase.Main1;
        }

        private bool SuperDoraSummon()
        {
            int lv10Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 10);
            return lv10Count >= 2 && Duel.Turn == 1;
        }

        private bool SuperDoraEffect()
        {
            var target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && IsAceCard(c));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool ZeusSummon()
        {
            return true;
        }

        private bool ZeusEffect()
        {
            if (Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 2)
            {
                return true;
            }
            return false;
        }

        private bool LinkuribohSummon()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id == CardId.SlimeToken);
        }

        private bool LinkuribohEffect()
        {
            return true;
        }

        private bool KnightmarePhoenixSummon()
        {
            return Enemy.GetSpellCount() > 0 && Bot.GetMonsterCount() >= 2;
        }

        private bool KnightmarePhoenixEffect()
        {
            var target = Enemy.GetSpells().FirstOrDefault(c => c != null);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool KnightmareUnicornSummon()
        {
            return Enemy.GetMonsterCount() + Enemy.GetSpellCount() > 0 && Bot.GetMonsterCount() >= 3;
        }

        private bool KnightmareUnicornEffect()
        {
            var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup()) ?? Enemy.GetSpells().FirstOrDefault(c => c != null);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TACTICAL DECISION OVERRIDES (OnSelectCard / OnSelectTribute / Position / Option)
        // ═══════════════════════════════════════════════════════════════

        public override IList<ClientCard> OnSelectTribute(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            var sorted = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
            var result = new List<ClientCard>();

            foreach (var c in sorted)
            {
                if (IsAceCard(c) && c.Id != CardId.EgyptianGodSlime && result.Count >= min) continue;
                result.Add(c);
                if (result.Count >= max) break;
            }

            if (result.Count < min)
            {
                foreach (var c in sorted)
                {
                    if (!result.Contains(c))
                    {
                        result.Add(c);
                        if (result.Count >= min) break;
                    }
                }
            }
            return result;
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // CRITICAL: Extra Deck Banish for Pot of Prosperity / Extravagance
            // STRICT RULE: NEVER BANISH EGYPTIAN GOD SLIME!
            if (cards.All(c => c.Location == CardLocation.Extra))
            {
                var safeBanishList = cards
                    .Where(c => c != null && c.Id != CardId.EgyptianGodSlime)
                    .OrderBy(c => c.Id == CardId.Linkuriboh ? 1 :
                                  c.Id == CardId.KnightmarePhoenix ? 2 :
                                  c.Id == CardId.KnightmareUnicorn ? 3 :
                                  c.Id == CardId.SuperDora ? 4 :
                                  c.Id == CardId.Zeus ? 5 :
                                  c.Id == CardId.GustavMax ? 6 : 10)
                    .Take(max).ToList();

                if (safeBanishList.Count >= min)
                    return safeBanishList;
            }

            // Forbidden Droplet cost & target selection
            if (LastChainCard != null && LastChainCard.Id == CardId.ForbiddenDroplet)
            {
                if (hint == HINT_SELECT_TOGRAVE || hint == HINT_SELECT_DISCARD)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.SlimeToken,
                        CardId.GuardianSlime,
                        CardId.TheWingedDragonOfRaImmortalPhoenix,
                        CardId.SliferTheSkyDragon,
                        CardId.ObeliskTheTormentor,
                        CardId.TheWingedDragonOfRa,
                        CardId.MetalReflectSlime);
                }
                var oppTargets = cards.Where(c => c != null && c.Controller == 1 && c.IsFaceup() && !c.IsDisabled()).OrderByDescending(c => c.Attack).Take(max).ToList();
                if (oppTargets.Count >= min) return oppTargets;
            }

            // Super Polymerization material selection: STRICTLY OPPONENT MONSTERS FIRST!
            if (LastChainCard != null && LastChainCard.Id == CardId.SuperPolymerization)
            {
                if (hint == HINT_SELECT_DISCARD)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.GuardianSlime,
                        CardId.TheWingedDragonOfRaImmortalPhoenix,
                        CardId.SliferTheSkyDragon,
                        CardId.ObeliskTheTormentor,
                        CardId.TheWingedDragonOfRa,
                        CardId.MetalReflectSlime);
                }
                if (hint == HINT_SELECT_FUSION_MAT || hint == 0)
                {
                    var oppMats = cards.Where(c => c != null && c.Controller == 1).OrderByDescending(c => c.Attack).Take(max).ToList();
                    if (oppMats.Count >= min) return oppMats;
                }
            }

            // Pot of Prosperity Excavated Cards Selection
            if (LastChainCard != null && LastChainCard.Id == CardId.PotOfProsperity && hint == HINT_SELECT_TOHAND)
            {
                return SelectPreferredCard(cards, min, max,
                    CardId.MoundOfTheBoundCreator,
                    CardId.TheTrueSunGod,
                    CardId.SoulCrossing,
                    CardId.SuperPolymerization,
                    CardId.ForbiddenDroplet,
                    CardId.AncientChant,
                    CardId.ReactorSlime,
                    CardId.GuardianSlime,
                    CardId.SliferTheSkyDragon,
                    CardId.ObeliskTheTormentor,
                    CardId.TheBreakingRuinGod,
                    CardId.TheRevivedSkyGod,
                    CardId.FoolishBurial);
            }

            // Millennium Revelation discard cost
            if (LastChainCard != null && LastChainCard.Id == CardId.MillenniumRevelation && hint == HINT_SELECT_DISCARD)
            {
                return SelectPreferredCard(cards, min, max,
                    CardId.SliferTheSkyDragon,
                    CardId.ObeliskTheTormentor,
                    CardId.TheWingedDragonOfRa);
            }

            // Egyptian God Slime material selection (tribute Guardian Slime or Metal Reflect Slime)
            if (LastChainCard != null && LastChainCard.Id == CardId.EgyptianGodSlime)
            {
                return SelectPreferredCard(cards, min, max, CardId.GuardianSlime, CardId.MetalReflectSlime, CardId.ReactorSlime);
            }

            // Soul Crossing Tribute selection: STRICTLY OPPONENT MONSTERS FIRST!
            if (LastChainCard != null && LastChainCard.Id == CardId.SoulCrossing)
            {
                var oppTargets = cards.Where(c => c != null && c.Controller == 1).OrderByDescending(c => c.Attack).Take(max).ToList();
                if (oppTargets.Count >= min) return oppTargets;
            }

            // The Revived Sky God search / summon
            if (LastChainCard != null && LastChainCard.Id == CardId.TheRevivedSkyGod)
            {
                return SelectPreferredCard(cards, min, max, CardId.SliferTheSkyDragon, CardId.MonsterReborn);
            }

            // The Breaking Ruin God search / summon
            if (LastChainCard != null && LastChainCard.Id == CardId.TheBreakingRuinGod)
            {
                return SelectPreferredCard(cards, min, max, CardId.ObeliskTheTormentor);
            }

            // The True Sun God search
            if (LastChainCard != null && LastChainCard.Id == CardId.TheTrueSunGod)
            {
                return SelectPreferredCard(cards, min, max,
                    CardId.AncientChant,
                    CardId.GuardianSlime,
                    CardId.ReactorSlime,
                    CardId.SoulCrossing,
                    CardId.MillenniumRevelation,
                    CardId.TheWingedDragonOfRa);
            }

            // Ancient Chant search
            if (LastChainCard != null && LastChainCard.Id == CardId.AncientChant)
            {
                return SelectPreferredCard(cards, min, max, CardId.TheWingedDragonOfRa);
            }

            // Mound of the Bound Creator search on destruction
            if (LastChainCard != null && LastChainCard.Id == CardId.MoundOfTheBoundCreator)
            {
                return SelectPreferredCard(cards, min, max,
                    CardId.SliferTheSkyDragon,
                    CardId.ObeliskTheTormentor,
                    CardId.TheWingedDragonOfRaSphereMode,
                    CardId.TheWingedDragonOfRa);
            }

            // Sphere Mode Tribute on our field
            if (LastChainCard != null && LastChainCard.Id == CardId.TheWingedDragonOfRaSphereMode)
            {
                return SelectPreferredCard(cards, min, max, CardId.TheWingedDragonOfRa);
            }

            // Ra 1000 LP Pop Target Selection: STRICTLY ENEMY MONSTERS ONLY
            if (LastChainCard != null && LastChainCard.Id == CardId.TheWingedDragonOfRa)
            {
                var enemyTargets = cards.Where(c => c != null && c.Controller == 1).OrderByDescending(c => c.Attack).Take(max).ToList();
                if (enemyTargets.Count > 0)
                    return enemyTargets;
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.GuardianSlime || cardId == CardId.ReactorSlime || cardId == CardId.SlimeToken || cardId == CardId.MetalReflectSlime)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
            }
            if (cardId == CardId.SliferTheSkyDragon || cardId == CardId.ObeliskTheTormentor || cardId == CardId.TheWingedDragonOfRa || cardId == CardId.TheWingedDragonOfRaImmortalPhoenix || cardId == CardId.JuggernautLiebe || cardId == CardId.GustavMax || cardId == CardId.Zeus || cardId == CardId.StarvingVenomFusionDragon || cardId == CardId.PredaplantDragostapelia)
            {
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }
            if (cardId == CardId.EgyptianGodSlime || cardId == CardId.SuperDora || cardId == CardId.MudragonOfTheSwamp)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }
            return base.OnSelectPosition(cardId, positions);
        }

        private IList<ClientCard> SelectPreferredCard(IList<ClientCard> cards, int min, int max, params int[] preferredIds)
        {
            var result = new List<ClientCard>();
            foreach (int id in preferredIds)
            {
                var matches = cards.Where(c => c != null && c.Id == id && !result.Contains(c)).ToList();
                foreach (var m in matches)
                {
                    result.Add(m);
                    if (result.Count >= max) break;
                }
                if (result.Count >= max) break;
            }
            if (result.Count < min)
            {
                foreach (var card in cards)
                {
                    if (card != null && !result.Contains(card))
                    {
                        result.Add(card);
                        if (result.Count >= min) break;
                    }
                }
            }
            return result;
        }

        private bool MonsterReposOverride()
        {
            if (Card == null) return false;
            if (IsAceCard(Card))
            {
                if (Card.Id == CardId.EgyptianGodSlime || Card.Id == CardId.SuperDora || Card.Id == CardId.MudragonOfTheSwamp)
                {
                    if (Card.IsDefense()) return false;
                    return true;
                }
                if (Card.IsAttack()) return false;
                return true;
            }

            if (Card.Id == CardId.GuardianSlime || Card.Id == CardId.ReactorSlime || Card.Id == CardId.MetalReflectSlime || Card.Id == CardId.SlimeToken)
            {
                if (Card.IsDefense()) return false;
                return true;
            }

            bool enemyEmpty = Enemy.GetMonsterCount() == 0;
            if (Card.IsAttack())
            {
                if (!enemyEmpty && Card.Attack < 1500) return true;
            }
            else
            {
                if (enemyEmpty || Card.Defense < Card.Attack) return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  BATTLE & ATTACK LOGIC
        // ═══════════════════════════════════════════════════════════════

        public override BattlePhaseAction OnBattle(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            if (attackers == null || attackers.Count == 0)
                return null;

            // 1. Direct attack scenario
            if (defenders == null || defenders.Count == 0)
            {
                var directAttacker = attackers
                    .Where(c => c != null && c.IsFaceup() && c.IsAttack() && c.Attack > 0)
                    .OrderBy(c => IsAceCard(c) ? 0 : 1)
                    .ThenByDescending(c => c.Attack)
                    .FirstOrDefault();

                if (directAttacker != null)
                {
                    return AI.Attack(directAttacker, null);
                }
            }

            // 2. Combat against defenders
            foreach (var attacker in attackers.Where(c => c != null && c.IsFaceup() && c.IsAttack()).OrderByDescending(c => c.Attack))
            {
                foreach (var defender in defenders.Where(d => d != null))
                {
                    if (defender.IsFaceup() && defender.IsAttack() && attacker.Attack > defender.Attack)
                        return AI.Attack(attacker, defender);
                    if (defender.IsFaceup() && defender.IsDefense() && attacker.Attack > defender.Defense)
                        return AI.Attack(attacker, defender);
                    if (defender.IsFacedown() && attacker.Attack >= 2000)
                        return AI.Attack(attacker, defender);
                }
            }

            return null;
        }
        // ═══════════════════════════════════════════════════════════════
        //  BOARD BREAKER EFFECTS (Going 2nd)
        // ═══════════════════════════════════════════════════════════════

        private bool LightningStormEffect()
        {
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup()) ||
                Bot.GetSpells().Any(c => c != null && c.IsFaceup()))
                return false;

            if (Enemy.GetMonsterCount() >= 2)
            {
                AI.SelectOption(0);
                return true;
            }
            if (Enemy.GetSpellCount() >= 2)
            {
                AI.SelectOption(1);
                return true;
            }
            if (Enemy.GetMonsterCount() > 0)
            {
                AI.SelectOption(0);
                return true;
            }
            if (Enemy.GetSpellCount() > 0)
            {
                AI.SelectOption(1);
                return true;
            }
            return false;
        }

        private bool DarkRulerNoMoreEffect()
        {
            return Enemy.GetMonsterCount() >= 2;
        }
    }
}
