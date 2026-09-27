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
    // CARD DOSSIER — GOD-01 (Modern Egyptian God Engine: Slifer, Obelisk, Ra, Slimes, Ruin God, Sky God)
    // ====================================================================================================
    // | Card Name                     | Type      | OPT? | Cost        | Effect                                  |
    // |-------------------------------|-----------|------|-------------|-----------------------------------------|
    // | Slifer the Sky Dragon         | Monster   | No   | 3 Tributes  | +1000 ATK/DEF per hand card, -2000 ATK  |
    // | Obelisk the Tormentor         | Monster   | No   | 3 Tributes  | 4000/4000, Untargetable, Trib 2 wipe    |
    // | The Winged Dragon of Ra       | Monster   | No   | 3 Tributes  | Pay LP down to 100 -> ATK, 1000 pop     |
    // | Ra - Sphere Mode              | Monster   | No   | 3 Opp Tribs | Tributes 3 opp monsters to clear board  |
    // | Ra - Immortal Phoenix         | Monster   | No   | 1000 LP     | 4000/4000 Unaffected, send 1 mon to GY  |
    // | Guardian Slime                | Monster   | HOPT | None        | SS on damage, GY send -> search Ra S/T  |
    // | Reactor Slime                 | Monster   | HOPT | None        | SS 2 Slime Tokens, BP tribute -> set MRS|
    // | Metal Reflect Slime           | Trap/Mon  | No   | None        | SS Lv10 WATER Aqua 0/3000 -> Trib EGS   |
    // | Egyptian God Slime            | Fusion/Mon| No   | Trib 1 Slm  | Counts as 3 Tributes, 3000 DEF, protects|
    // | The True Sun God              | ContSpell | HOPT | None        | Search Ra/Chant/Slime/Rev; dumps Phoenix|
    // | The Breaking Ruin God         | QuickSp   | HOPT | None        | Uncounterable SS Obelisk from Hand/GY   |
    // | Ancient Chant                 | Spell     | HOPT | None        | Search Ra, +1 Trib Summon, adds ATK     |
    // | Millennium Revelation         | ContSpell | HOPT | Discard God | Search Monster Reborn, revives Ra       |
    // | Card Advance                  | Spell     | No   | None        | Reorder top 5, +1 Tribute Summon        |
    // | Soul Crossing                 | QuickSp   | HOPT | None        | Quick Tribute 1-3 opp mons for God      |
    // | The Revived Sky God           | Trap      | HOPT | None        | Uncounterable SS Slifer from GY + draw 6|
    // | Mound of the Bound Creator    | FieldSp   | No   | None        | Lv10+ targeting & destruction immunity  |
    // | Forbidden Droplet             | QuickSp   | No   | Send cards  | Uncounterable negate & half ATK         |
    // | Super Polymerization          | QuickSp   | No   | Discard 1   | Uncounterable fusion using opp monsters |
    // | Infinite Impermanence         | Trap      | No   | None        | Column negation from hand/field         |
    // ====================================================================================================

    [Deck("GOD-01", "GOD-01")]
    public class _2026_God01Executor : ModernExecutor
    {
        public class CardId
        {
            // Egyptian Gods
            public const int SliferTheSkyDragon = 10000022;
            public const int ObeliskTheTormentor = 10000002;
            public const int TheWingedDragonOfRa = 10000010;
            public const int TheWingedDragonOfRaSphereMode = 10000080;
            public const int TheWingedDragonOfRaImmortalPhoenix = 10000090;

            // Slime Engine
            public const int GuardianSlime = 15771991;
            public const int ReactorSlime = 79387392;
            public const int SlimeToken = 79387393;
            public const int MetalReflectSlime = 26905245;
            public const int EgyptianGodSlime = 42166000;

            // God Support Spells & Traps
            public const int TheTrueSunGod = 11587414;
            public const int TheBreakingRuinGod = 85182315;
            public const int AncientChant = 78665705;
            public const int MillenniumRevelation = 41044418;
            public const int CardAdvance = 52112003;
            public const int SoulCrossing = 5253985;
            public const int TheRevivedSkyGod = 59094601;
            public const int MoundOfTheBoundCreator = 269012;
            public const int MonsterReborn = 83764718;
            public const int FoolishBurial = 81439173;

            // Staples & Board Breakers
            public const int PotOfProsperity = 84211599;
            public const int ForbiddenDroplet = 24299458;
            public const int SuperPolymerization = 48130397;
            public const int InfiniteImpermanence = 10045474;

            // Extra Deck
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

        // Standard OCG Hint IDs (SKILL.md Section 7.3)
        private const long HINTMSG_RELEASE = 500;
        private const long HINTMSG_DISCARD = 501;
        private const long HINTMSG_DESTROY = 502;
        private const long HINTMSG_REMOVE = 504;
        private const long HINTMSG_ATOHAND = 505;
        private const long HINTMSG_TODECK = 506;
        private const long HINTMSG_EQUIP = 507;
        private const long HINTMSG_TOGRAVE = 508;
        private const long HINTMSG_SPSUMMON = 509;
        private const long HINTMSG_FUSIONMATERIAL = 511;
        private const long HINTMSG_POSCHANGE = 518;
        private const long HINTMSG_XMATERIAL = 519;
        private const long HINTMSG_DISABLE = 552;
        private const long HINTMSG_NEGATE = 572;

        private bool _ancientChantUsedThisTurn = false;
        private bool _trueSunGodUsedThisTurn = false;
        private bool _prosperityUsedThisTurn = false;
        private bool _soulCrossingUsedThisTurn = false;
        private bool _breakingRuinGodUsedThisTurn = false;
        private bool _revivedSkyGodUsedThisTurn = false;
        private bool _millenniumRevelationUsedThisTurn = false;
        private bool _cardAdvanceUsedThisTurn = false;

        public _2026_God01Executor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // ── 1. Register Ace Cards in ResourcePlanner ──
            ResourcePlan.RegisterAceCards(
                CardId.SliferTheSkyDragon,
                CardId.ObeliskTheTormentor,
                CardId.TheWingedDragonOfRa,
                CardId.TheWingedDragonOfRaImmortalPhoenix,
                CardId.TheWingedDragonOfRaSphereMode,
                CardId.EgyptianGodSlime,
                CardId.JuggernautLiebe,
                CardId.GustavMax,
                CardId.SuperDora,
                CardId.Zeus,
                CardId.StarvingVenomFusionDragon,
                CardId.PredaplantDragostapelia,
                CardId.MudragonOfTheSwamp
            );

            // ── 2. Register Starters and Baits in BaitPlanner ──
            BaitPlanner.RegisterComboStarters(
                CardId.PotOfProsperity,
                CardId.TheTrueSunGod,
                CardId.CardAdvance,
                CardId.AncientChant,
                CardId.FoolishBurial,
                CardId.TheBreakingRuinGod,
                CardId.MillenniumRevelation,
                CardId.ReactorSlime
            );
            BaitPlanner.RegisterBaitCards(
                CardId.MoundOfTheBoundCreator,
                CardId.PotOfProsperity
            );

            // ── 3. Register Realistic Combo Lines ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "CardAdvance-ReactorSlime-GodTribute",
                RequiredCards = new List<int> { CardId.CardAdvance, CardId.ReactorSlime },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.CardAdvance, ActionType = ExecutorType.Activate, Description = "Reorder top 5 & gain extra Tribute Summon" },
                    new() { CardId = CardId.ReactorSlime, ActionType = ExecutorType.Summon, Description = "Normal Summon Reactor Slime" },
                    new() { CardId = CardId.ReactorSlime, ActionType = ExecutorType.Activate, Description = "Spawn 2 Slime Tokens" }
                }
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "TheBreakingRuinGod-InstantObelisk",
                RequiredCards = new List<int> { CardId.TheBreakingRuinGod, CardId.ObeliskTheTormentor },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.TheBreakingRuinGod, ActionType = ExecutorType.Activate, Description = "Uncounterably Special Summon Obelisk with immunity" }
                }
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "MillenniumRevelation-RebornRevival",
                RequiredCards = new List<int> { CardId.MillenniumRevelation },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.MillenniumRevelation, ActionType = ExecutorType.Activate, Description = "Activate Revelation & search Monster Reborn" },
                    new() { CardId = CardId.MonsterReborn, ActionType = ExecutorType.Activate, Description = "Revive Egyptian God from GY" }
                }
            });

            // ═══════════════════════════════════════════════════════════════
            //  EXECUTORS PIPELINE (Tiered Priority)
            // ═══════════════════════════════════════════════════════════════

            // ── Tier 0: Quick Disruptions & Handtraps ──
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletEffect);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, InfiniteImpermanenceEffect);
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationEffect);
            AddExecutor(ExecutorType.Activate, CardId.SoulCrossing, SoulCrossingEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheRevivedSkyGod, RevivedSkyGodEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheBreakingRuinGod, BreakingRuinGodEffect);
            AddExecutor(ExecutorType.Activate, CardId.GuardianSlime, GuardianSlimeDamageEffect);
            AddExecutor(ExecutorType.Activate, CardId.MetalReflectSlime, MetalReflectSlimeEffect);

            // ── Tier 1: Board Breaker Summons ──
            AddExecutor(ExecutorType.Summon, CardId.TheWingedDragonOfRaSphereMode, SphereModeOpponentSummon);
            AddExecutor(ExecutorType.Activate, CardId.TheWingedDragonOfRaSphereMode, SphereModeOurEffect);

            // ── Tier 2: Search, Digging & Setup Spells ──
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, PotOfProsperityEffect);
            AddExecutor(ExecutorType.Activate, CardId.CardAdvance, CardAdvanceEffect);
            AddExecutor(ExecutorType.Activate, CardId.MoundOfTheBoundCreator, MoundEffect);
            AddExecutor(ExecutorType.Activate, CardId.FoolishBurial, FoolishBurialEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheTrueSunGod, TrueSunGodEffect);
            AddExecutor(ExecutorType.Activate, CardId.MillenniumRevelation, MillenniumRevelationEffect);
            AddExecutor(ExecutorType.Activate, CardId.AncientChant, AncientChantEffect);
            AddExecutor(ExecutorType.Activate, CardId.MonsterReborn, MonsterRebornEffect);

            // ── Tier 3: 3-Tribute Extra Deck Boss: Egyptian God Slime ──
            AddExecutor(ExecutorType.SpSummon, CardId.EgyptianGodSlime, EgyptianGodSlimeSummon);

            // ── Tier 3.5: Rank 10 / Rank 11 Finishers (Gustav Max -> Liebe) ──
            AddExecutor(ExecutorType.SpSummon, CardId.GustavMax, GustavMaxSummon);
            AddExecutor(ExecutorType.Activate, CardId.GustavMax, GustavMaxEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.JuggernautLiebe, JuggernautLiebeSummon);
            AddExecutor(ExecutorType.Activate, CardId.JuggernautLiebe, JuggernautLiebeEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.SuperDora, SuperDoraSummon);
            AddExecutor(ExecutorType.Activate, CardId.SuperDora, SuperDoraEffect);

            // ── Tier 4: God Ignition & Trigger Effects ──
            AddExecutor(ExecutorType.Activate, CardId.TheWingedDragonOfRaImmortalPhoenix, ImmortalPhoenixEffect);
            AddExecutor(ExecutorType.Activate, CardId.ObeliskTheTormentor, ObeliskEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheWingedDragonOfRa, RaEffect);

            // ── Tier 5: Tribute Engine Starters ──
            AddExecutor(ExecutorType.Summon, CardId.ReactorSlime, ReactorSlimeSummon);
            AddExecutor(ExecutorType.Activate, CardId.ReactorSlime, ReactorSlimeEffect);

            // ── Tier 6: Egyptian God Tribute Summons ──
            AddExecutor(ExecutorType.Summon, CardId.SliferTheSkyDragon, SliferSummon);
            AddExecutor(ExecutorType.Summon, CardId.ObeliskTheTormentor, ObeliskSummon);
            AddExecutor(ExecutorType.Summon, CardId.TheWingedDragonOfRa, RaSummon);

            // ── Tier 7: Extra Deck Utility & Board Clears ──
            AddExecutor(ExecutorType.SpSummon, CardId.Zeus, ZeusSummon);
            AddExecutor(ExecutorType.Activate, CardId.Zeus, ZeusEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Linkuriboh, LinkuribohSummon);
            AddExecutor(ExecutorType.Activate, CardId.Linkuriboh, LinkuribohEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmarePhoenix, KnightmarePhoenixSummon);
            AddExecutor(ExecutorType.Activate, CardId.KnightmarePhoenix, KnightmarePhoenixEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmareUnicorn, KnightmareUnicornSummon);
            AddExecutor(ExecutorType.Activate, CardId.KnightmareUnicorn, KnightmareUnicornEffect);

            // ── Tier 8: Sets (Prepare Defensive Backrow for Opponent Turn) ──
            AddExecutor(ExecutorType.SpellSet, CardId.MetalReflectSlime, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.TheRevivedSkyGod, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.SoulCrossing, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.TheBreakingRuinGod, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.SuperPolymerization, DefaultSpellSet);
            AddExecutor(ExecutorType.MonsterSet, CardId.GuardianSlime, DefensiveMonsterSet);

            // ── Tier 9: Monster Reposition ──
            AddExecutor(ExecutorType.Repos, MonsterReposOverride);
        }

        public override bool OnSelectHand() => true;

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
            _cardAdvanceUsedThisTurn = false;
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.SliferTheSkyDragon
                || card.Id == CardId.ObeliskTheTormentor
                || card.Id == CardId.TheWingedDragonOfRa
                || card.Id == CardId.TheWingedDragonOfRaImmortalPhoenix
                || card.Id == CardId.TheWingedDragonOfRaSphereMode
                || card.Id == CardId.EgyptianGodSlime
                || card.Id == CardId.JuggernautLiebe
                || card.Id == CardId.GustavMax
                || card.Id == CardId.SuperDora
                || card.Id == CardId.Zeus
                || card.Id == CardId.StarvingVenomFusionDragon
                || card.Id == CardId.PredaplantDragostapelia
                || card.Id == CardId.MudragonOfTheSwamp;
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 0;
            // When Super Poly is active, strictly absorb opponent monsters
            if (LastChainCard != null && LastChainCard.Id == CardId.SuperPolymerization && c.Controller == 1)
                return 0;

            if (c.Id == CardId.SlimeToken) return 1;
            if (c.Id == CardId.MetalReflectSlime) return 5;
            if (c.Id == CardId.ReactorSlime) return 10;
            if (c.Id == CardId.GuardianSlime) return 15;

            // Egyptian God Slime is preferred when tribute summoning an Egyptian God
            if (c.Id == CardId.EgyptianGodSlime)
            {
                if (Card != null && (Card.Id == CardId.SliferTheSkyDragon || Card.Id == CardId.ObeliskTheTormentor || Card.Id == CardId.TheWingedDragonOfRa))
                    return 2;
                return 50000;
            }

            if (c.Id == CardId.GustavMax || c.Id == CardId.SuperDora) return 10000;
            if (c.Id == CardId.JuggernautLiebe || c.Id == CardId.Zeus) return 100000;

            if (c.Id == CardId.SliferTheSkyDragon || c.Id == CardId.ObeliskTheTormentor || c.Id == CardId.TheWingedDragonOfRa || c.Id == CardId.TheWingedDragonOfRaImmortalPhoenix)
                return int.MaxValue;

            return 50;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 0: QUICK DISRUPTIONS & HANDTRAPS
        // ═══════════════════════════════════════════════════════════════

        private bool ForbiddenDropletEffect()
        {
            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled() && (c.Attack >= 2000 || CardIntelligence.IsHighThreatChokepoint(c.Id) || CardIntelligence.IsKnownNegator(c.Id))))
            {
                int availableCost = 0;
                if (Bot.GetMonsters().Any(c => c != null && c.Id == CardId.SlimeToken)) availableCost++;
                if (Bot.Hand.Any(c => c != null && c.Id == CardId.GuardianSlime)) availableCost++;
                if (Bot.GetMonsters().Any(c => c != null && c.Id == CardId.MetalReflectSlime)) availableCost++;
                if (Bot.Hand.Any(c => c != null && (c.Id == CardId.TheWingedDragonOfRaImmortalPhoenix || c.Id == CardId.SliferTheSkyDragon))) availableCost++;

                return availableCost > 0;
            }
            return false;
        }

        private bool InfiniteImpermanenceEffect()
        {
            if (Card.Location == CardLocation.Hand && Bot.GetMonsterCount() > 0) return false;

            if (Duel.LastChainPlayer == 1 && Util.GetLastChainCard() != null && Util.GetLastChainCard().Location == CardLocation.MonsterZone)
            {
                var lastCard = Util.GetLastChainCard();
                var activatingTarget = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled() && c.Id == lastCard.Id);
                if (activatingTarget != null)
                {
                    AI.SelectCard(activatingTarget);
                    return true;
                }
            }

            var threat = Enemy.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && !c.IsDisabled() && (CardIntelligence.IsHighThreatChokepoint(c.Id) || CardIntelligence.IsKnownNegator(c.Id) || c.Attack >= 2500))
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();

            if (threat != null)
            {
                AI.SelectCard(threat);
                return true;
            }
            return false;
        }

        private bool SuperPolymerizationEffect()
        {
            if (Bot.Hand.Count == 0 && Card.Location != CardLocation.Hand) return false;

            int oppMonCount = Enemy.GetMonsters().Count(c => c != null && c.IsFaceup());
            if (oppMonCount < 2) return false;

            bool hasValidTarget = Bot.ExtraDeck.Any(c => c != null && (c.Id == CardId.MudragonOfTheSwamp || c.Id == CardId.StarvingVenomFusionDragon || c.Id == CardId.PredaplantDragostapelia));
            return hasValidTarget;
        }

        private bool SoulCrossingEffect()
        {
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2)
                return false;

            if (_soulCrossingUsedThisTurn) return false;

            bool hasGodInHand = Bot.Hand.Any(c => c != null && (c.Id == CardId.SliferTheSkyDragon || c.Id == CardId.ObeliskTheTormentor || c.Id == CardId.TheWingedDragonOfRa));
            if (!hasGodInHand) return false;

            bool hasEgyptianGodSlime = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id == CardId.EgyptianGodSlime);
            int totalMonsters = Bot.GetMonsterCount() + Enemy.GetMonsterCount();

            if (hasEgyptianGodSlime || totalMonsters >= 3)
            {
                // On our turn, only activate when ready to summon
                if (Duel.Player == 0)
                {
                    _soulCrossingUsedThisTurn = true;
                    return true;
                }
                // On opponent's turn, disrupt their board during their Main Phase!
                if (Duel.Player == 1 && Enemy.GetMonsterCount() >= 1)
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
                        return true;
                    }
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                if (Enemy.Graveyard.Count(c => c != null && c.IsMonster()) >= 2)
                    return true;
            }
            return false;
        }

        private bool GuardianSlimeDamageEffect()
        {
            if (Card.Location == CardLocation.Hand) return true;
            if (Card.Location == CardLocation.MonsterZone) return true;
            if (Card.Location == CardLocation.Grave) return true;
            return false;
        }

        private bool MetalReflectSlimeEffect()
        {
            if (Bot.GetMonsterCount() < 5) return true;
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 1 & 2: SETUP SPELLS, SEARCH & CONSISTENCY
        // ═══════════════════════════════════════════════════════════════

        private bool SphereModeOpponentSummon() => Enemy.GetMonsterCount() >= 3;

        private bool SphereModeOurEffect()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.Controller == 0) return true;
            return false;
        }

        private bool PotOfProsperityEffect()
        {
            if (_prosperityUsedThisTurn) return false;
            _prosperityUsedThisTurn = true;
            return true;
        }

        private bool CardAdvanceEffect()
        {
            if (_cardAdvanceUsedThisTurn) return false;
            _cardAdvanceUsedThisTurn = true;
            return true;
        }

        private bool MoundEffect()
        {
            if (Card.Location == CardLocation.Grave) return true;
            return !Bot.HasInSpellZone(CardId.MoundOfTheBoundCreator);
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

        private bool TrueSunGodEffect()
        {
            // Field ignition: dump Immortal Phoenix to GY
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
                return true;
            }
            return !Bot.HasInSpellZone(CardId.TheTrueSunGod);
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
                if (hasGodInHand && hasRebornInDeck) return true;

                if (Bot.Hand.Any(c => c != null && c.Id == CardId.MonsterReborn) && Bot.Graveyard.Any(c => c != null && c.Id == CardId.TheWingedDragonOfRa))
                    return true;
            }
            return false;
        }

        private bool AncientChantEffect()
        {
            if (Card.Location == CardLocation.Grave) return true;

            if (!_ancientChantUsedThisTurn)
            {
                _ancientChantUsedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool MonsterRebornEffect()
        {
            var target = Bot.Graveyard.FirstOrDefault(c => c != null && (c.Id == CardId.TheWingedDragonOfRa || c.Id == CardId.SliferTheSkyDragon || c.Id == CardId.ObeliskTheTormentor || c.Id == CardId.EgyptianGodSlime || c.Id == CardId.GuardianSlime));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 3: EGYPTIAN GOD SLIME (3-TRIBUTE EXTRA DECK BODY)
        // ═══════════════════════════════════════════════════════════════

        private bool EgyptianGodSlimeSummon()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.Id == CardId.GuardianSlime || c.Id == CardId.MetalReflectSlime));
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 3.5: RANK 10 / 11 FINISHERS
        // ═══════════════════════════════════════════════════════════════

        private bool GustavMaxSummon()
        {
            int lv10Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 10 && !c.IsCode(CardId.SliferTheSkyDragon));
            return lv10Count >= 2;
        }

        private bool GustavMaxEffect() => true;

        private bool JuggernautLiebeSummon()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.Id == CardId.GustavMax || c.Id == CardId.SuperDora));
        }

        private bool JuggernautLiebeEffect() => Duel.Phase == DuelPhase.Main1;

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

        // ═══════════════════════════════════════════════════════════════
        //  TIER 4: GOD IGNITIONS & TRIGGERS
        // ═══════════════════════════════════════════════════════════════

        private bool ImmortalPhoenixEffect()
        {
            if (Card.Location == CardLocation.Grave) return true;

            if (Card.Location == CardLocation.MonsterZone)
            {
                if (Bot.LifePoints > 1000 && Enemy.GetMonsterCount() > 0)
                {
                    var target = Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled());
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
                int otherFodder = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c != Card && (c.Id == CardId.SlimeToken || c.Id == CardId.ReactorSlime || c.Id == CardId.GuardianSlime));
                if (otherFodder >= 2 && Enemy.GetMonsterCount() >= 1) return true;
            }
            return false;
        }

        private bool RaEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Pay LP down to 100 ONLY when lethal is clear or Mound protects Ra
                if (Card.Attack <= 0)
                {
                    if (Bot.HasInSpellZone(CardId.MoundOfTheBoundCreator) || Enemy.GetMonsterCount() == 0)
                        return true;
                    return false;
                }

                // 1000 LP Pop
                if (Enemy.GetMonsterCount() > 0 && Bot.LifePoints > 1000)
                {
                    var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !IsDestructionImmune(c));
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
            // Main Phase token spawn: only when we can use them or need defense blockers
            if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
            {
                return Bot.GetMonsterCount() <= 3;
            }
            // Battle Phase: tribute self to set Metal Reflect Slime from Deck
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep)
            {
                return true;
            }
            return false;
        }

        private bool DefensiveMonsterSet() => Bot.GetMonsterCount() < 3;

        private bool SliferSummon() => GetAvailableTributeCount() >= 3;
        private bool ObeliskSummon() => GetAvailableTributeCount() >= 3;
        private bool RaSummon() => GetAvailableTributeCount() >= 3;

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
        //  TIER 7: EXTRA DECK UTILITY
        // ═══════════════════════════════════════════════════════════════

        private bool ZeusSummon() => true;
        private bool ZeusEffect() => Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 2;
        private bool LinkuribohSummon() => Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id == CardId.SlimeToken);
        private bool LinkuribohEffect() => true;

        private bool KnightmarePhoenixSummon() => Enemy.GetSpellCount() > 0 && Bot.GetMonsterCount() >= 2;
        private bool KnightmarePhoenixEffect()
        {
            var target = Enemy.GetSpells().FirstOrDefault(c => c != null);
            if (target != null) { AI.SelectCard(target); return true; }
            return false;
        }

        private bool KnightmareUnicornSummon() => (Enemy.GetMonsterCount() + Enemy.GetSpellCount() > 0) && Bot.GetMonsterCount() >= 3;
        private bool KnightmareUnicornEffect()
        {
            var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup()) ?? Enemy.GetSpells().FirstOrDefault(c => c != null);
            if (target != null) { AI.SelectCard(target); return true; }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  MODAL OPTION INTERCEPTION (OnSelectOption)
        // ═══════════════════════════════════════════════════════════════

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0) return 0;

            // Pot of Prosperity: Excavate 6 (index 1) if Extra Deck >= 6, otherwise 3 (index 0)
            if (LastChainCard != null && LastChainCard.Id == CardId.PotOfProsperity)
            {
                if (options.Count > 1 && Bot.ExtraDeck.Count >= 6) return 1;
                return 0;
            }

            // The True Sun God activation vs ignition
            if (LastChainCard != null && LastChainCard.Id == CardId.TheTrueSunGod)
            {
                return 0;
            }

            return base.OnSelectOption(options);
        }

        // ═══════════════════════════════════════════════════════════════
        //  TACTICAL DECISION OVERRIDES (OnSelectCard / Tribute / Position)
        // ═══════════════════════════════════════════════════════════════

        public override IList<ClientCard> OnSelectTribute(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Prioritize Egyptian God Slime (counts as 3 tributes), then tokens and fodder
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
            // CRITICAL: Extra Deck Banish for Pot of Prosperity
            // STRICT RULE: NEVER BANISH EGYPTIAN GOD SLIME!
            if (cards.All(c => c.Location == CardLocation.Extra))
            {
                var safeBanishList = cards
                    .Where(c => c != null && c.Id != CardId.EgyptianGodSlime)
                    .OrderBy(c => c.Id == CardId.Linkuriboh ? 1 :
                                  c.Id == CardId.KnightmarePhoenix ? 2 :
                                  c.Id == CardId.KnightmareUnicorn ? 3 :
                                  c.Id == CardId.MudragonOfTheSwamp ? 4 :
                                  c.Id == CardId.SuperDora ? 5 :
                                  c.Id == CardId.Zeus ? 6 :
                                  c.Id == CardId.GustavMax ? 7 : 10)
                    .Take(max).ToList();

                if (safeBanishList.Count >= min) return safeBanishList;
            }

            // Hint 500: HINTMSG_RELEASE
            if (hint == HINTMSG_RELEASE)
            {
                // Soul Crossing: Strictly opponent monsters first!
                if (LastChainCard != null && LastChainCard.Id == CardId.SoulCrossing)
                {
                    var oppTargets = cards.Where(c => c != null && c.Controller == 1).OrderByDescending(c => c.Attack).Take(max).ToList();
                    if (oppTargets.Count >= min) return oppTargets;
                }
                return OnSelectTribute(cards, min, max, hint, cancelable);
            }

            // Hint 501: HINTMSG_DISCARD
            if (hint == HINTMSG_DISCARD)
            {
                // Millennium Revelation: Discard Divine-Beast monster to search Monster Reborn
                if (LastChainCard != null && LastChainCard.Id == CardId.MillenniumRevelation)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.SliferTheSkyDragon,
                        CardId.ObeliskTheTormentor,
                        CardId.TheWingedDragonOfRa);
                }

                // Super Polymerization cost: discard Guardian Slime (triggers search!) or duplicate
                if (LastChainCard != null && LastChainCard.Id == CardId.SuperPolymerization)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.GuardianSlime,
                        CardId.TheWingedDragonOfRaImmortalPhoenix,
                        CardId.SliferTheSkyDragon,
                        CardId.ObeliskTheTormentor);
                }

                // General discard: prefer Guardian Slime (triggers search in GY)
                return SelectPreferredCard(cards, min, max,
                    CardId.GuardianSlime,
                    CardId.TheWingedDragonOfRaImmortalPhoenix,
                    CardId.SliferTheSkyDragon,
                    CardId.MetalReflectSlime);
            }

            // Hint 502: HINTMSG_DESTROY
            if (hint == HINTMSG_DESTROY)
            {
                var targets = cards.Where(c => c != null && c.Controller == 1 && !IsDestructionImmune(c))
                    .OrderByDescending(c => CardIntelligence.IsHighThreatChokepoint(c.Id) ? 100 : c.Attack)
                    .Take(max).ToList();
                if (targets.Count >= min) return targets;
            }

            // Hint 504 / 503: HINTMSG_REMOVE (Banish)
            if (hint == HINTMSG_REMOVE || hint == 503)
            {
                // The Breaking Ruin God: Banish all monsters from opponent GY
                if (LastChainCard != null && LastChainCard.Id == CardId.TheBreakingRuinGod)
                {
                    var oppGrave = cards.Where(c => c != null && c.Controller == 1).Take(max).ToList();
                    if (oppGrave.Count >= min) return oppGrave;
                }
            }

            // Hint 505: HINTMSG_ATOHAND (Search / Retrieve)
            if (hint == HINTMSG_ATOHAND)
            {
                // The True Sun God search:
                // If don't have Ra in hand/GY, search Ancient Chant (which searches Ra AND grants extra tribute summon!)
                if (LastChainCard != null && LastChainCard.Id == CardId.TheTrueSunGod)
                {
                    bool hasRa = Bot.Hand.Any(c => c != null && c.Id == CardId.TheWingedDragonOfRa) || Bot.Graveyard.Any(c => c != null && c.Id == CardId.TheWingedDragonOfRa);
                    if (!hasRa)
                    {
                        return SelectPreferredCard(cards, min, max, CardId.AncientChant, CardId.GuardianSlime, CardId.TheWingedDragonOfRa, CardId.MillenniumRevelation);
                    }
                    return SelectPreferredCard(cards, min, max, CardId.GuardianSlime, CardId.MillenniumRevelation, CardId.AncientChant, CardId.TheWingedDragonOfRa);
                }

                // Ancient Chant search: Strictly Ra
                if (LastChainCard != null && LastChainCard.Id == CardId.AncientChant)
                {
                    return SelectPreferredCard(cards, min, max, CardId.TheWingedDragonOfRa);
                }

                // Guardian Slime search on GY send:
                // Only searches Spells/Traps that mention "The Winged Dragon of Ra"
                if (LastChainCard != null && LastChainCard.Id == CardId.GuardianSlime)
                {
                    bool hasTrueSunGod = Bot.HasInSpellZone(CardId.TheTrueSunGod) || Bot.Hand.Any(c => c != null && c.Id == CardId.TheTrueSunGod);
                    if (!hasTrueSunGod)
                        return SelectPreferredCard(cards, min, max, CardId.TheTrueSunGod, CardId.AncientChant, CardId.MillenniumRevelation);
                    return SelectPreferredCard(cards, min, max, CardId.AncientChant, CardId.MillenniumRevelation, CardId.TheTrueSunGod);
                }

                // Millennium Revelation: Add Monster Reborn
                if (LastChainCard != null && LastChainCard.Id == CardId.MillenniumRevelation)
                {
                    return SelectPreferredCard(cards, min, max, CardId.MonsterReborn);
                }

                // Pot of Prosperity Excavated Cards Selection
                if (LastChainCard != null && LastChainCard.Id == CardId.PotOfProsperity)
                {
                    bool hasObelisk = Bot.Hand.Any(c => c != null && c.Id == CardId.ObeliskTheTormentor);
                    return SelectPreferredCard(cards, min, max,
                        hasObelisk ? CardId.TheBreakingRuinGod : CardId.TheTrueSunGod,
                        CardId.TheTrueSunGod,
                        CardId.TheBreakingRuinGod,
                        CardId.ReactorSlime,
                        CardId.CardAdvance,
                        CardId.AncientChant,
                        CardId.MetalReflectSlime,
                        CardId.SoulCrossing,
                        CardId.ForbiddenDroplet,
                        CardId.GuardianSlime,
                        CardId.MoundOfTheBoundCreator);
                }

                // Mound of the Bound Creator destruction search
                if (LastChainCard != null && LastChainCard.Id == CardId.MoundOfTheBoundCreator)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.SliferTheSkyDragon,
                        CardId.ObeliskTheTormentor,
                        CardId.TheWingedDragonOfRaSphereMode,
                        CardId.TheWingedDragonOfRa);
                }
            }

            // Hint 508: HINTMSG_TOGRAVE
            if (hint == HINTMSG_TOGRAVE)
            {
                // Forbidden Droplet cost
                if (LastChainCard != null && LastChainCard.Id == CardId.ForbiddenDroplet)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.SlimeToken,
                        CardId.GuardianSlime, // Triggers search!
                        CardId.MetalReflectSlime,
                        CardId.TheWingedDragonOfRaImmortalPhoenix,
                        CardId.SliferTheSkyDragon);
                }

                // The True Sun God: Dump Immortal Phoenix from Deck
                if (LastChainCard != null && LastChainCard.Id == CardId.TheTrueSunGod)
                {
                    return SelectPreferredCard(cards, min, max, CardId.TheWingedDragonOfRaImmortalPhoenix);
                }

                // Foolish Burial
                if (LastChainCard != null && LastChainCard.Id == CardId.FoolishBurial)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.GuardianSlime,
                        CardId.TheWingedDragonOfRaImmortalPhoenix,
                        CardId.SliferTheSkyDragon);
                }
            }

            // Hint 509: HINTMSG_SPSUMMON
            if (hint == HINTMSG_SPSUMMON)
            {
                // The Breaking Ruin God: Obelisk the Tormentor
                if (LastChainCard != null && LastChainCard.Id == CardId.TheBreakingRuinGod)
                {
                    return SelectPreferredCard(cards, min, max, CardId.ObeliskTheTormentor);
                }

                // The Revived Sky God: Slifer the Sky Dragon
                if (LastChainCard != null && LastChainCard.Id == CardId.TheRevivedSkyGod)
                {
                    return SelectPreferredCard(cards, min, max, CardId.SliferTheSkyDragon);
                }

                // Monster Reborn
                if (LastChainCard != null && LastChainCard.Id == CardId.MonsterReborn)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.TheWingedDragonOfRa,
                        CardId.SliferTheSkyDragon,
                        CardId.ObeliskTheTormentor,
                        CardId.EgyptianGodSlime,
                        CardId.GuardianSlime);
                }

                // Sphere Mode on our field
                if (LastChainCard != null && LastChainCard.Id == CardId.TheWingedDragonOfRaSphereMode)
                {
                    return SelectPreferredCard(cards, min, max, CardId.TheWingedDragonOfRa);
                }
            }

            // Hint 511 / 513 / 533: Fusion Materials
            if (hint == HINTMSG_FUSIONMATERIAL || hint == 513 || hint == 533)
            {
                // Egyptian God Slime: Tribute Guardian Slime or Metal Reflect Slime
                if (LastChainCard != null && LastChainCard.Id == CardId.EgyptianGodSlime)
                {
                    return SelectPreferredCard(cards, min, max, CardId.MetalReflectSlime, CardId.GuardianSlime);
                }

                // Super Polymerization: Strictly opponent monsters first!
                if (LastChainCard != null && LastChainCard.Id == CardId.SuperPolymerization)
                {
                    var oppMats = cards.Where(c => c != null && c.Controller == 1).OrderByDescending(c => c.Attack).Take(max).ToList();
                    if (oppMats.Count >= min) return oppMats;
                }
            }

            // Hint 552 / 572: Disable / Negate targets (Droplet / Impermanence)
            if (hint == HINTMSG_DISABLE || hint == HINTMSG_NEGATE)
            {
                var targets = cards.Where(c => c != null && c.Controller == 1 && c.IsFaceup() && !c.IsDisabled())
                    .OrderByDescending(c => CardIntelligence.IsHighThreatChokepoint(c.Id) ? 100 :
                                          CardIntelligence.IsKnownNegator(c.Id) ? 80 : c.Attack)
                    .Take(max).ToList();
                if (targets.Count >= min) return targets;
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

            // The Breaking Ruin God forces Obelisk into Defense Position
            if (cardId == CardId.ObeliskTheTormentor && LastChainCard != null && LastChainCard.Id == CardId.TheBreakingRuinGod)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
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

        public override BattlePhaseAction OnBattle(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            if (attackers == null || attackers.Count == 0) return null;

            // Direct attack scenario
            if (defenders == null || defenders.Count == 0)
            {
                var directAttacker = attackers
                    .Where(c => c != null && c.IsFaceup() && c.IsAttack() && c.Attack > 0)
                    .OrderBy(c => IsAceCard(c) ? 0 : 1)
                    .ThenByDescending(c => c.Attack)
                    .FirstOrDefault();

                if (directAttacker != null) return AI.Attack(directAttacker, null);
            }

            // Combat against defenders
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
    }
}
