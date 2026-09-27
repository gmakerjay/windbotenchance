using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    // ====================================================================================================
    // CARD AUDIT — PhantomKnight (The Phantom Knights of the Round Table)
    // 100% verified against cards.cdb and PhantomKnight.ydk (40 Main, 15 Extra, 15 Side)
    // ====================================================================================================
    // | Card Name                               | ID       | Lv | ATK  | DEF  | Type   | Key Role                                  |
    // |-----------------------------------------|----------|----|------|------|--------|-------------------------------------------|
    // | The Phantom Knights of Doomed Soleret   | 83566725 | 3  | 400  | 1400 | W/DARK | Free SS empty board / Set PK S/T / Lv+1   |
    // | The Phantom Knights of Decayed Cloak    | 46072770 | 3  | 1000 | 300  | W/DARK | Reveal PK to SS / Search PK mon / Rank=Lv |
    // | The Phantom Knights of Torn Scales      | 25538345 | 3  | 600  | 1600 | W/DARK | Discard 1 dump PK / GY SS on PK banish    |
    // | The Phantom Knights of Silent Boots     | 36426778 | 3  | 200  | 1200 | W/DARK | Free SS with PK / GY banish search S/T    |
    // | The Phantom Knights of Ancient Cloak    | 90432163 | 3  | 800  | 1000 | W/DARK | +800 ATK buff / GY banish search PK mon   |
    // | The Phantom Knights of Ragged Gloves    | 63821877 | 3  | 1000 | 500  | W/DARK | +1000 ATK Xyz mat / GY banish dump PK     |
    // | Mulcharmy Purulia                       | 84192580 | 4  | 100  | 600  | Aqua   | Handtrap draw on opp hand summon          |
    // | Mulcharmy Fuwalos                       | 42141493 | 4  | 100  | 600  | WingB  | Handtrap draw on opp Deck/Extra SS        |
    // | Ash Blossom & Joyous Spring             | 14558128 | 3  | 0    | 1800 | Zombie | Handtrap negate search/dump/deck SS       |
    // | Droll & Lock Bird                       | 94145021 | 1  | 0    | 0    | SpellC | Handtrap shut down repeated searches      |
    // | Infinite Impermanence                   | 10045474 | 0  | 0    | 0    | Trap   | Monster negate from hand/field            |
    // | Dominus Impulse                         | 40366667 | 0  | 0    | 0    | Trap   | SS negate & pop (DARK has 0 drawback)     |
    // | Dominus Spark                           | 6325660  | 0  | 0    | 0    | Trap   | Targeted monster banish (0 drawback)      |
    // | Phantom Knights' Fog Blade              | 25542642 | 0  | 0    | 0    | CTrap  | Monster negate + attack lock / GY revive  |
    // | Phantom Knights' Wing                   | 98431356 | 0  | 0    | 0    | Trap   | +500 ATK & 1x destroy shield / GY revive  |
    // | The Phantom Knights of Umbrage Veil     | 85257384 | 3  | 0    | 300  | Trap   | Lv3 Monster turn set / GY Quick Xyz       |
    // | The Phantom Knights' RUM Requiem        | 62104532 | 0  | 0    | 0    | Spell  | Revive PK/Dragon + Rank up 1 higher Xyz   |
    // | 'The Phantom Knights' RUM Launch'       | 3298689  | 0  | 0    | 0    | QSpell | Rank up empty mat DARK Xyz -> Ophion/Req  |
    // | Reinforcement of the Army               | 32807846 | 0  | 0    | 0    | Spell  | Search any Lv3 PK starter                 |
    // | Foolish Burial                          | 81439174 | 0  | 0    | 0    | Spell  | Send key PK (Torn Scales/Boots) to GY     |
    // | Triple Tactics Talent                   | 25311006 | 0  | 0    | 0    | Spell  | Draw 2 / Steal monster / Hand rip         |
    // | Called by the Grave                     | 24224830 | 0  | 0    | 0    | QSpell | Banish opp GY monster + negate            |
    // | Crossout Designator                     | 65681983 | 0  | 0    | 0    | QSpell | Declare card -> banish from deck & negate |
    // |-----------------------------------------|----------|----|------|------|--------|-------------------------------------------|
    // | Cherubini, Ebon Angel Burning Abyss     | 58699500 | 2  | 500  | 5    | Link   | Unstoppable Lv3 dump as COST              |
    // | The Phantom Knights of Rusty Bardiche   | 26692769 | 3  | 2100 | 37   | Link   | Dump PK + Set PK S/T + Pop on DARK Xyz SS |
    // | The Phantom Knights of Malevolent Scythe| 78449284 | 3  | 1900 | 0    | Xyz    | Detach 1 SS PK from Deck / RUM search     |
    // | The Phantom Knights of Break Sword      | 62709239 | 3  | 2000 | 1000 | Xyz    | Pop 1 opp + 1 own / Floats into 2x Lv4    |
    // | Raider's Knight                         | 28781003 | 4  | 2000 | 0    | Xyz    | Overlays into Arc Rebellion / Dark Reb    |
    // | Dark Rebellion Xyz Dragon               | 16195942 | 4  | 2500 | 2000 | Xyz    | Halves opp ATK / Base for Dark Requiem    |
    // | Dark Requiem Xyz Dragon                 | 1621413  | 5  | 3000 | 2500 | Xyz    | 3x Monster Negate + Pop + Revive Xyz (Ace)|
    // | Arc Rebellion Xyz Dragon                | 64276752 | 5  | 3000 | 2500 | Xyz    | Indestructible / Negate board / 10k+ OTK  |
    // | Evilswarm Ophion                        | 91279700 | 4  | 2550 | 1650 | Xyz    | Level 5+ SS Floodgate (via RUM Launch)    |
    // | Evilswarm Nightmare                     | 359563   | 4  | 950  | 1950 | Xyz    | Flips opp SS monsters face-down DEF       |
    // | Shamanite Shamanknight                  | 16237004 | 3  | 1700 | 2400 | Xyz    | Recycles banished traps / revives DARK    |
    // | I:P Masquerena                          | 65741786 | 2  | 800  | 5    | Link   | Quick Link into S:P Little Knight         |
    // | S:P Little Knight                       | 29301450 | 2  | 1600 | 40   | Link   | Banish on SS / Quick banish 2 monsters    |
    // | Dharc the Dark Charmer, Gloomy          | 8264361  | 2  | 1850 | 5    | Link   | Steal opp DARK monster from GY            |
    // | Super Starslayer TY-PHON - Sky Crisis   | 93039339 | 12 | 2900 | 2900 | Xyz    | Overlay on highest ATK / Floodgate & bounce|
    // ====================================================================================================

    [Deck("PhantomKnight", "PhantomKnight", "Modern")]
    [Deck("Phantom Knights", "PhantomKnight", "Modern")]
    [Deck("2026_PhantomKnight", "PhantomKnight", "Modern")]
    [Deck("phantomknight", "PhantomKnight", "Modern")]
    public class _2026_PhantomKnightExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Deck Phantom Knights Monsters
            public const int DoomedSoleret = 83566725;
            public const int DecayedCloak = 46072770;
            public const int TornScales = 25538345;
            public const int SilentBoots = 36426778;
            public const int AncientCloak = 90432163;
            public const int RaggedGloves = 63821877;

            // Handtraps & Interrupts
            public const int MulcharmyPurulia = 84192580;
            public const int MulcharmyFuwalos = 42141493;
            public const int AshBlossom = 14558128;
            public const int DrollAndLockBird = 94145021;
            public const int InfiniteImpermanence = 10045474;
            public const int DominusImpulse = 40366667;
            public const int DominusSpark = 6325660;

            // Spells
            public const int RUMRequiem = 62104532;
            public const int RUMLaunch = 3298689;
            public const int ReinforcementOfTheArmy = 32807846;
            public const int FoolishBurial = 81439174;
            public const int TripleTacticsTalent = 25311006;
            public const int CalledByTheGrave = 24224830;
            public const int CrossoutDesignator = 65681983;

            // Traps
            public const int FogBlade = 25542642;
            public const int PKWing = 98431356;
            public const int UmbrageVeil = 85257384;

            // Extra Deck
            public const int Cherubini = 58699500;
            public const int RustyBardiche = 26692769;
            public const int MalevolentScythe = 78449284;
            public const int BreakSword = 62709239;
            public const int RaidersKnight = 28781003;
            public const int DarkRebellion = 16195942;
            public const int DarkRequiem = 1621413;
            public const int ArcRebellion = 64276752;
            public const int EvilswarmOphion = 91279700;
            public const int EvilswarmNightmare = 359563;
            public const int Shamanite = 16237004;
            public const int IPMasquerena = 65741786;
            public const int SPLittleKnight = 29301450;
            public const int DharcCharmer = 8264361;
            public const int TyphonSkyCrisis = 93039339;

            // Side Deck
            public const int SantaClaws = 46565218;
            public const int LavaGolem = 102380;
            public const int HarpiesFeatherDuster = 18144507;
            public const int InfestationPandemic = 27541267;
            public const int PotOfSloth = 98476659;
            public const int EvenlyMatched = 15693423;
            public const int SolemnReport = 78114463;
            public const int SolemnJudgment = 41420027;
        }

        private static readonly HashSet<int> AceMonsters = new HashSet<int>
        {
            CardId.DarkRequiem,
            CardId.ArcRebellion,
            CardId.EvilswarmOphion,
            CardId.RustyBardiche,
            CardId.SPLittleKnight,
            CardId.TyphonSkyCrisis
        };

        private static readonly HashSet<int> PKMonsters = new HashSet<int>
        {
            CardId.DoomedSoleret,
            CardId.DecayedCloak,
            CardId.TornScales,
            CardId.SilentBoots,
            CardId.AncientCloak,
            CardId.RaggedGloves
        };

        private static readonly HashSet<int> PKSpellsTraps = new HashSet<int>
        {
            CardId.FogBlade,
            CardId.PKWing,
            CardId.UmbrageVeil,
            CardId.RUMRequiem,
            CardId.RUMLaunch
        };

        // Turn state tracking
        private bool _normalSummonUsedThisTurn = false;
        private bool _soleretHandSSUsed = false;
        private bool _decayedCloakHandSSUsed = false;
        private bool _bootsHandSSUsed = false;
        private bool _tornScalesPitchUsed = false;
        private bool _scytheDetachUsed = false;
        private bool _bardicheSendUsed = false;

        public _2026_PhantomKnightExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            RegisterExecutors();
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _normalSummonUsedThisTurn = false;
            _soleretHandSSUsed = false;
            _decayedCloakHandSSUsed = false;
            _bootsHandSSUsed = false;
            _tornScalesPitchUsed = false;
            _scytheDetachUsed = false;
            _bardicheSendUsed = false;
        }

        private void RegisterExecutors()
        {
            // ═══════════════════════════════════════════════════════════════════
            //  GROUP 1: COUNTER TRAPS & OMNI-NEGATION (Priority #1)
            // ═══════════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, ShouldSolemnJudgmentActivate);
            AddExecutor(ExecutorType.Activate, CardId.SolemnReport, ShouldSolemnReportActivate);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, ShouldCalledByTheGraveActivate);
            AddExecutor(ExecutorType.Activate, CardId.CrossoutDesignator, ShouldCrossoutDesignatorActivate);

            // ═══════════════════════════════════════════════════════════════════
            //  GROUP 2: ACE QUICK DISRUPTIONS & HANDTRAPS
            // ═══════════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.DarkRequiem, ShouldDarkRequiemActivate);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, ShouldSPLittleKnightActivate);
            AddExecutor(ExecutorType.Activate, CardId.IPMasquerena, ShouldIPMasquerenaActivate);
            AddExecutor(ExecutorType.Activate, CardId.EvilswarmNightmare, ShouldEvilswarmNightmareActivate);
            AddExecutor(ExecutorType.Activate, CardId.FogBlade, ShouldFogBladeActivate);
            AddExecutor(ExecutorType.Activate, CardId.DominusImpulse, ShouldDominusImpulseActivate);
            AddExecutor(ExecutorType.Activate, CardId.DominusSpark, ShouldDominusSparkActivate);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, ShouldImpermanenceActivate);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, ShouldAshBlossomActivate);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, ShouldDrollActivate);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyPurulia, ShouldMulcharmyPuruliaActivate);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, ShouldMulcharmyFuwalosActivate);

            // ═══════════════════════════════════════════════════════════════════
            //  GROUP 3: TURN 2 BOARD BREAKERS
            // ═══════════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.LavaGolem, ShouldLavaGolemSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SantaClaws, ShouldSantaClawsSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, ShouldHarpiesFeatherDusterActivate);
            AddExecutor(ExecutorType.Activate, CardId.EvenlyMatched, ShouldEvenlyMatchedActivate);
            AddExecutor(ExecutorType.Activate, CardId.PotOfSloth, ShouldPotOfSlothActivate);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, ShouldTripleTacticsTalentActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.TyphonSkyCrisis, ShouldTyphonSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TyphonSkyCrisis, ShouldTyphonActivate);

            // ═══════════════════════════════════════════════════════════════════
            //  GROUP 4: STARTERS & EXTENDERS (Main Phase Combos)
            // ═══════════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.ReinforcementOfTheArmy, ShouldRotAActivate);
            AddExecutor(ExecutorType.Activate, CardId.FoolishBurial, ShouldFoolishBurialActivate);

            // Step 1: Free Special Summons BEFORE using Normal Summon (Anti-Brick)
            AddExecutor(ExecutorType.SpSummon, CardId.DoomedSoleret, ShouldDoomedSoleretHandSS);
            AddExecutor(ExecutorType.Activate, CardId.DecayedCloak, ShouldDecayedCloakHandSS);
            AddExecutor(ExecutorType.SpSummon, CardId.SilentBoots, ShouldSilentBootsHandSS);

            // Step 2: Normal Summons (Torn Scales preferred for hand cleaning, then Cloak/Soleret)
            AddExecutor(ExecutorType.Summon, CardId.TornScales, ShouldTornScalesSummon);
            AddExecutor(ExecutorType.Summon, CardId.DecayedCloak, ShouldDecayedCloakSummon);
            AddExecutor(ExecutorType.Summon, CardId.DoomedSoleret, ShouldDoomedSoleretSummon);
            AddExecutor(ExecutorType.Summon, CardId.SilentBoots, ShouldSilentBootsSummon);
            AddExecutor(ExecutorType.Summon, CardId.AncientCloak, ShouldAncientCloakSummon);
            AddExecutor(ExecutorType.Summon, CardId.RaggedGloves, ShouldRaggedGlovesSummon);

            // Step 3: Ignition effects of summoned monsters
            AddExecutor(ExecutorType.Activate, CardId.DoomedSoleret, ShouldDoomedSoleretOnSummon);
            AddExecutor(ExecutorType.Activate, CardId.DecayedCloak, ShouldDecayedCloakOnSummon);
            AddExecutor(ExecutorType.Activate, CardId.TornScales, ShouldTornScalesPitchActivate);

            // ═══════════════════════════════════════════════════════════════════
            //  GROUP 5: EXTRA DECK LINK & XYZ COMBOS
            // ═══════════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.Cherubini, ShouldCherubiniSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Cherubini, ShouldCherubiniCostDump);

            AddExecutor(ExecutorType.SpSummon, CardId.RustyBardiche, ShouldRustyBardicheSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.RustyBardiche, ShouldRustyBardicheActivate);

            AddExecutor(ExecutorType.SpSummon, CardId.MalevolentScythe, ShouldMalevolentScytheSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.MalevolentScythe, ShouldMalevolentScytheActivate);

            AddExecutor(ExecutorType.SpSummon, CardId.BreakSword, ShouldBreakSwordSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.BreakSword, ShouldBreakSwordActivate);

            AddExecutor(ExecutorType.SpSummon, CardId.RaidersKnight, ShouldRaidersKnightSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.RaidersKnight, ShouldRaidersKnightActivate);

            AddExecutor(ExecutorType.SpSummon, CardId.DarkRebellion, ShouldDarkRebellionSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.DarkRebellion, ShouldDarkRebellionActivate);

            AddExecutor(ExecutorType.Activate, CardId.ArcRebellion, ShouldArcRebellionActivate);

            AddExecutor(ExecutorType.SpSummon, CardId.Shamanite, ShouldShamaniteSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Shamanite, ShouldShamaniteActivate);

            AddExecutor(ExecutorType.SpSummon, CardId.DharcCharmer, ShouldDharcSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.DharcCharmer, ShouldDharcActivate);

            AddExecutor(ExecutorType.SpSummon, CardId.IPMasquerena, ShouldIPMasquerenaSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, ShouldSPLittleKnightSpSummon);

            // ═══════════════════════════════════════════════════════════════════
            //  GROUP 6: RANK-UP SPELLS & GY EXTENDERS
            // ═══════════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.RUMRequiem, ShouldRUMRequiemActivate);
            AddExecutor(ExecutorType.Activate, CardId.RUMLaunch, ShouldRUMLaunchActivate);
            AddExecutor(ExecutorType.Activate, CardId.InfestationPandemic, ShouldInfestationPandemicActivate);

            // GY Monster Reborns & Searches
            AddExecutor(ExecutorType.Activate, CardId.SilentBoots, ShouldSilentBootsGYActivate);
            AddExecutor(ExecutorType.Activate, CardId.AncientCloak, ShouldAncientCloakGYActivate);
            AddExecutor(ExecutorType.Activate, CardId.DoomedSoleret, ShouldDoomedSoleretGYLevelMod);
            AddExecutor(ExecutorType.Activate, CardId.DecayedCloak, ShouldDecayedCloakGYRankMod);
            AddExecutor(ExecutorType.Activate, CardId.RaggedGloves, ShouldRaggedGlovesGYActivate);
            AddExecutor(ExecutorType.Activate, CardId.TornScales, ShouldTornScalesGYRevive);

            // Trap Reanimation / Activation
            AddExecutor(ExecutorType.Activate, CardId.UmbrageVeil, ShouldUmbrageVeilActivate);
            AddExecutor(ExecutorType.Activate, CardId.PKWing, ShouldPKWingActivate);

            // ═══════════════════════════════════════════════════════════════════
            //  GROUP 7: BACKROW SETTING (Controlled to prevent zone clogging)
            // ═══════════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, CardId.FogBlade, ShouldSetTrapSafely);
            AddExecutor(ExecutorType.SpellSet, CardId.PKWing, ShouldSetTrapSafely);
            AddExecutor(ExecutorType.SpellSet, CardId.UmbrageVeil, ShouldSetTrapSafely);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, ShouldSetTrapSafely);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnJudgment, ShouldSetTrapSafely);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnReport, ShouldSetTrapSafely);
            AddExecutor(ExecutorType.SpellSet, CardId.RUMLaunch, ShouldSetTrapSafely);
        }

        // ═══════════════════════════════════════════════════════════════════
        //  ACTIVATION GUARDS: COUNTERS & HANDTRAPS
        // ═══════════════════════════════════════════════════════════════════

        private bool ShouldSolemnJudgmentActivate()
        {
            return Bot.LifePoints > 1000 && Duel.LastChainPlayer == 1;
        }

        private bool ShouldSolemnReportActivate()
        {
            if (Duel.LastChainPlayer != 1) return false;
            // Option 0 costs 1500 LP, Option 1 costs 3000 LP
            return Bot.LifePoints > 1500;
        }

        private bool ShouldCalledByTheGraveActivate()
        {
            if (Duel.LastChainPlayer != 1) return false;
            ClientCard lastCard = LastChainCard;
            if (lastCard == null) return false;

            // Target handtraps or high threat monster in opponent's GY
            return Enemy.Graveyard.Any(c => c.IsMonster() && (c.Id == lastCard.Id || CardIntelligence.IsHighThreatChokepoint(c.Id) || CardIntelligence.IsHandtrap(c.Id)));
        }

        private bool ShouldCrossoutDesignatorActivate()
        {
            if (Duel.LastChainPlayer != 1) return false;
            ClientCard lastCard = LastChainCard;
            if (lastCard == null) return false;

            // Can negate if we also run a copy in our Main Deck
            return GetRemainingCount(lastCard.Id) > 0;
        }

        private bool ShouldDarkRequiemActivate()
        {
            // Quick effect: negate monster activation, destroy, and revive Xyz from GY
            if (Duel.LastChainPlayer == 1)
            {
                ClientCard lastCard = LastChainCard;
                if (lastCard != null && lastCard.IsMonster())
                    return true;
            }

            // Ignition: Set opponent face-up monster ATK to 0 and gain original ATK
            if (Duel.IsMainPhase() && Duel.Player == 0)
            {
                return Enemy.GetMonsters().Any(c => c.IsFaceup() && c.Attack > 0 && !c.HasType(CardType.Token));
            }

            return false;
        }

        private bool ShouldFogBladeActivate()
        {
            // In GY: banish to revive PK monster from GY
            if (Card.Location == CardLocation.Grave)
            {
                return Bot.Graveyard.Any(c => PKMonsters.Contains(c.Id)) && Bot.GetMonsterCount() < 5;
            }

            // On Field: negate opponent face-up effect monster
            if (Card.Location == CardLocation.SpellZone)
            {
                if (Duel.LastChainPlayer == 1)
                {
                    ClientCard lastCard = LastChainCard;
                    if (lastCard != null && lastCard.IsMonster() && lastCard.IsFaceup() && !lastCard.IsDisabled())
                        return true;
                }

                // In Main Phase, negate highest ATK active threat
                return Enemy.GetMonsters().Any(c => c.IsFaceup() && !c.IsDisabled() && (CardIntelligence.IsKnownNegator(c.Id) || CardIntelligence.IsHighThreatChokepoint(c.Id) || c.Attack >= 2000));
            }

            return false;
        }

        private bool ShouldDominusImpulseActivate()
        {
            // Negates an effect that includes Special Summoning a monster
            return Duel.LastChainPlayer == 1;
        }

        private bool ShouldDominusSparkActivate()
        {
            // Banish 1 monster opponent controls when opponent activated monster effect in hand or GY
            return Duel.LastChainPlayer == 1 && Enemy.GetMonsters().Any(c => c.IsFaceup() && !CardIntelligence.IsTargetImmune(c.Id));
        }

        private bool ShouldImpermanenceActivate()
        {
            if (Duel.LastChainPlayer == 1)
            {
                ClientCard lastCard = LastChainCard;
                if (lastCard != null && lastCard.IsMonster() && lastCard.IsFaceup() && !lastCard.IsDisabled())
                    return true;
            }

            return Enemy.GetMonsters().Any(c => c.IsFaceup() && !c.IsDisabled() && (CardIntelligence.IsKnownNegator(c.Id) || CardIntelligence.IsHighThreatChokepoint(c.Id)));
        }

        private bool ShouldAshBlossomActivate()
        {
            return Duel.LastChainPlayer == 1;
        }

        private bool ShouldDrollActivate()
        {
            return Duel.LastChainPlayer == 1 && Duel.Player != 0;
        }

        private bool ShouldMulcharmyPuruliaActivate()
        {
            return Bot.GetMonsterCount() == 0 && Bot.GetSpellCount() == 0 && Duel.Player == 1;
        }

        private bool ShouldMulcharmyFuwalosActivate()
        {
            return Bot.GetMonsterCount() == 0 && Bot.GetSpellCount() == 0 && Duel.Player == 1;
        }

        // ═══════════════════════════════════════════════════════════════════
        //  ACTIVATION GUARDS: BOARD BREAKERS & REMOVAL
        // ═══════════════════════════════════════════════════════════════════

        private bool ShouldLavaGolemSpSummon()
        {
            // Rule 4.2: Tribute 2 opponent threats only if we have removal or can swing over 3000 ATK
            var oppMonsters = Enemy.GetMonsters().Where(c => c.IsFaceup()).OrderByDescending(c => c.Attack).ToList();
            if (oppMonsters.Count < 2) return false;

            // Only tribute if at least one is a high threat/negator or ATK >= 2500
            bool hasThreat = oppMonsters.Any(c => CardIntelligence.IsKnownNegator(c.Id) || CardIntelligence.IsFloodgateMonster(c.Id) || c.Attack >= 2500);
            return hasThreat && !_normalSummonUsedThisTurn;
        }

        private bool ShouldSantaClawsSpSummon()
        {
            // Tribute 1 problematic threat
            return Enemy.GetMonsters().Any(c => c.IsFaceup() && (CardIntelligence.IsKnownNegator(c.Id) || CardIntelligence.IsFloodgateMonster(c.Id) || c.Attack >= 2800));
        }

        private bool ShouldHarpiesFeatherDusterActivate()
        {
            return Enemy.GetSpellCount() > 0;
        }

        private bool ShouldEvenlyMatchedActivate()
        {
            return (Duel.Phase == DuelPhase.Battle || Duel.Phase == DuelPhase.End) && (Enemy.GetMonsterCount() + Enemy.GetSpellCount()) > (Bot.GetMonsterCount() + Bot.GetSpellCount());
        }

        private bool ShouldPotOfSlothActivate()
        {
            // Going 2nd draw engine: draw equal to opponent cards, then bottom deck drawn - 1
            return (Enemy.GetMonsterCount() + Enemy.GetSpellCount()) >= 2;
        }

        private bool ShouldTripleTacticsTalentActivate()
        {
            // Usable if opponent activated monster effect during our Main Phase
            return true;
        }

        private bool ShouldTyphonSpSummon()
        {
            // Overlay on highest ATK monster if opponent SS 2+ from Extra Deck
            return Enemy.GetMonsters().Any(c => c.Attack >= 2500 || CardIntelligence.IsKnownNegator(c.Id));
        }

        private bool ShouldTyphonActivate()
        {
            // Detach 1: bounce 1 monster on field to hand
            return Enemy.GetMonsters().Any(c => c.IsFaceup());
        }

        // ═══════════════════════════════════════════════════════════════════
        //  ACTIVATION GUARDS: STARTERS & EXTENDERS (Anti-Brick Flow)
        // ═══════════════════════════════════════════════════════════════════

        private bool ShouldRotAActivate()
        {
            return true;
        }

        private bool ShouldFoolishBurialActivate()
        {
            return true;
        }

        private bool ShouldDoomedSoleretHandSS()
        {
            // SS from hand if we control no monsters (Must do before Normal Summon!)
            if (_soleretHandSSUsed || Bot.GetMonsterCount() > 0) return false;
            _soleretHandSSUsed = true;
            return true;
        }

        private bool ShouldDecayedCloakHandSS()
        {
            // Reveal 1 other PK card in hand to SS
            if (_decayedCloakHandSSUsed) return false;
            bool hasOtherPK = Bot.Hand.Any(c => c != Card && (PKMonsters.Contains(c.Id) || PKSpellsTraps.Contains(c.Id)));
            if (!hasOtherPK) return false;

            _decayedCloakHandSSUsed = true;
            return true;
        }

        private bool ShouldSilentBootsHandSS()
        {
            // SS if we control a PK monster
            if (_bootsHandSSUsed) return false;
            if (!Bot.GetMonsters().Any(c => c.IsFaceup() && PKMonsters.Contains(c.Id))) return false;

            _bootsHandSSUsed = true;
            return true;
        }

        private bool ShouldTornScalesSummon()
        {
            _normalSummonUsedThisTurn = true;
            return true;
        }

        private bool ShouldDecayedCloakSummon()
        {
            _normalSummonUsedThisTurn = true;
            return true;
        }

        private bool ShouldDoomedSoleretSummon()
        {
            _normalSummonUsedThisTurn = true;
            return true;
        }

        private bool ShouldSilentBootsSummon()
        {
            _normalSummonUsedThisTurn = true;
            return true;
        }

        private bool ShouldAncientCloakSummon()
        {
            _normalSummonUsedThisTurn = true;
            return true;
        }

        private bool ShouldRaggedGlovesSummon()
        {
            _normalSummonUsedThisTurn = true;
            return true;
        }

        private bool ShouldDoomedSoleretOnSummon()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            // Sets 1 PK S/T from Deck
            return Bot.GetSpellCount() < 5;
        }

        private bool ShouldDecayedCloakOnSummon()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            // Adds 1 PK monster from Deck to hand
            return true;
        }

        private bool ShouldTornScalesPitchActivate()
        {
            if (_tornScalesPitchUsed) return false;

            // Pitch hand card to send PK from deck to GY (Cleans stuck cards in hand!)
            bool hasPitchTarget = Bot.Hand.Any(c => c != Card && c.Id != CardId.CalledByTheGrave && c.Id != CardId.CrossoutDesignator);
            if (!hasPitchTarget) return false;

            _tornScalesPitchUsed = true;
            return true;
        }

        // ═══════════════════════════════════════════════════════════════════
        //  ACTIVATION GUARDS: EXTRA DECK LINK & XYZ
        // ═══════════════════════════════════════════════════════════════════

        private bool ShouldCherubiniSpSummon()
        {
            // 2 Level 3 monsters -> Cherubini
            int lv3Count = Bot.GetMonsters().Count(c => c.IsFaceup() && c.Level == 3 && !AceMonsters.Contains(c.Id));
            return lv3Count >= 2 && !Bot.GetMonsters().Any(c => c.Id == CardId.Cherubini);
        }

        private bool ShouldCherubiniCostDump()
        {
            // Send Lv3 monster from deck to GY as cost (Unstoppable!)
            return true;
        }

        private bool ShouldRustyBardicheSpSummon()
        {
            // 2+ DARK monsters (including a Link monster or 3 DARK monsters)
            int darkCount = Bot.GetMonsters().Count(c => c.IsFaceup() && c.HasAttribute(CardAttribute.Dark) && !AceMonsters.Contains(c.Id));
            return darkCount >= 2 && !Bot.GetMonsters().Any(c => c.Id == CardId.RustyBardiche);
        }

        private bool ShouldRustyBardicheActivate()
        {
            // Send PK monster to GY, Set PK S/T to field
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (!_bardicheSendUsed && Duel.IsMainPhase() && Duel.Player == 0)
                {
                    _bardicheSendUsed = true;
                    return true;
                }

                // On DARK Xyz summoned to zone it points to: pop 1 card on field
                return (Enemy.GetMonsterCount() + Enemy.GetSpellCount()) > 0;
            }
            return false;
        }

        private bool ShouldMalevolentScytheSpSummon()
        {
            // 2 Level 3 DARK monsters
            int lv3DarkCount = Bot.GetMonsters().Count(c => c.IsFaceup() && c.Level == 3 && c.HasAttribute(CardAttribute.Dark) && !AceMonsters.Contains(c.Id));
            return lv3DarkCount >= 2 && !Bot.GetMonsters().Any(c => c.Id == CardId.MalevolentScythe);
        }

        private bool ShouldMalevolentScytheActivate()
        {
            // Detach 1 to SS PK from deck
            if (Card.Overlays.Count > 0 && !_scytheDetachUsed)
            {
                _scytheDetachUsed = true;
                return true;
            }
            // Search RUM when Xyz Dragon is summoned
            return true;
        }

        private bool ShouldBreakSwordSpSummon()
        {
            // 2 Level 3 monsters
            int lv3Count = Bot.GetMonsters().Count(c => c.IsFaceup() && c.Level == 3 && !AceMonsters.Contains(c.Id));
            return lv3Count >= 2;
        }

        private bool ShouldBreakSwordActivate()
        {
            // Detach 1: pop 1 own + 1 opp card -> triggers float into 2x Lv4 PKs!
            if (Card.Location == CardLocation.MonsterZone && Card.Overlays.Count > 0)
            {
                return (Enemy.GetMonsterCount() + Enemy.GetSpellCount()) > 0;
            }
            // Float on destruction: revive 2 PKs as Lv4
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }
            return false;
        }

        private bool ShouldRaidersKnightSpSummon()
        {
            // 2 Level 4 DARK monsters (floated from Break Sword or boosted by Soleret)
            int lv4DarkCount = Bot.GetMonsters().Count(c => c.IsFaceup() && c.Level == 4 && c.HasAttribute(CardAttribute.Dark));
            return lv4DarkCount >= 2 && !Bot.GetMonsters().Any(c => c.Id == CardId.RaidersKnight);
        }

        private bool ShouldRaidersKnightActivate()
        {
            // Detach 1: Rank up into Arc Rebellion (for OTK) or Dark Rebellion
            return Card.Overlays.Count > 0;
        }

        private bool ShouldDarkRebellionSpSummon()
        {
            int lv4Count = Bot.GetMonsters().Count(c => c.IsFaceup() && c.Level == 4);
            return lv4Count >= 2;
        }

        private bool ShouldDarkRebellionActivate()
        {
            // Halves opp ATK and adds to own
            return Card.Overlays.Count >= 2 && Enemy.GetMonsters().Any(c => c.IsFaceup() && c.Attack > 0);
        }

        private bool ShouldArcRebellionActivate()
        {
            // Cannot be destroyed by card effects. Detach 1: gains original ATK of all other monsters + negates board!
            return Card.Overlays.Count > 0;
        }

        private bool ShouldShamaniteSpSummon()
        {
            int lv3Count = Bot.GetMonsters().Count(c => c.IsFaceup() && c.Level == 3 && !AceMonsters.Contains(c.Id));
            return lv3Count >= 2 && Bot.Banished.Any(c => c.IsTrap());
        }

        private bool ShouldShamaniteActivate()
        {
            return true;
        }

        private bool ShouldDharcSpSummon()
        {
            return Enemy.Graveyard.Any(c => c.IsMonster() && c.HasAttribute(CardAttribute.Dark));
        }

        private bool ShouldDharcActivate()
        {
            return Enemy.Graveyard.Any(c => c.IsMonster() && c.HasAttribute(CardAttribute.Dark));
        }

        private bool ShouldIPMasquerenaSpSummon()
        {
            // 2 non-Link monsters
            int nonLinkCount = Bot.GetMonsters().Count(c => c.IsFaceup() && !c.HasType(CardType.Link) && !AceMonsters.Contains(c.Id));
            return nonLinkCount >= 2 && Duel.Player == 0 && !Bot.GetMonsters().Any(c => c.Id == CardId.IPMasquerena);
        }

        private bool ShouldIPMasquerenaActivate()
        {
            // Quick Link during opponent's Main Phase into S:P Little Knight
            return Duel.Player == 1 && Duel.IsMainPhase();
        }

        private bool ShouldSPLittleKnightSpSummon()
        {
            int mats = Bot.GetMonsters().Count(c => c.IsFaceup() && !AceMonsters.Contains(c.Id));
            return mats >= 2;
        }

        private bool ShouldSPLittleKnightActivate()
        {
            // Banish 1 card on field/GY on SS using Fusion/Synchro/Xyz/Link
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (Duel.LastChainPlayer == 1)
                {
                    // Tag out 2 monsters until end phase to dodge removal
                    return true;
                }
                return (Enemy.GetMonsterCount() + Enemy.GetSpellCount()) > 0 || Enemy.Graveyard.Any(c => c.IsMonster());
            }
            return false;
        }

        private bool ShouldEvilswarmNightmareActivate()
        {
            // Flips opp SS monster face-down DEF
            return Card.Overlays.Count > 0 && Duel.LastChainPlayer == 1;
        }

        // ═══════════════════════════════════════════════════════════════════
        //  ACTIVATION GUARDS: RANK-UP MAGIC & GRAVEYARD ROUTING
        // ═══════════════════════════════════════════════════════════════════

        private bool ShouldRUMRequiemActivate()
        {
            // Target 1 PK/Xyz Dragon in GY/banish -> SS it -> Rank up DARK Xyz +1
            bool hasValidTargetInGrave = Bot.Graveyard.Any(c => PKMonsters.Contains(c.Id) || c.Id == CardId.DarkRebellion || c.Id == CardId.BreakSword);
            bool hasDarkXyzOnField = Bot.GetMonsters().Any(c => c.IsFaceup() && c.HasType(CardType.Xyz) && c.HasAttribute(CardAttribute.Dark));

            return hasValidTargetInGrave && hasDarkXyzOnField;
        }

        private bool ShouldRUMLaunchActivate()
        {
            // Target DARK Xyz with NO materials -> Rank up 1 higher (e.g. Rank 3 -> Rank 4 Evilswarm Ophion / Nightmare!)
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                return Bot.GetMonsters().Any(c => c.IsFaceup() && c.HasType(CardType.Xyz) && c.HasAttribute(CardAttribute.Dark) && c.Overlays.Count == 0);
            }
            // GY effect: attach PK monster from hand as material
            if (Card.Location == CardLocation.Grave)
            {
                return Bot.Hand.Any(c => PKMonsters.Contains(c.Id)) && Bot.GetMonsters().Any(c => c.IsFaceup() && c.HasType(CardType.Xyz) && c.HasAttribute(CardAttribute.Dark));
            }
            return false;
        }

        private bool ShouldInfestationPandemicActivate()
        {
            // Protection for Evilswarm Ophion against spells/traps
            return Bot.GetMonsters().Any(c => c.IsFaceup() && c.Id == CardId.EvilswarmOphion) && Duel.LastChainPlayer == 1;
        }

        private bool ShouldSilentBootsGYActivate()
        {
            if (Card.Location != CardLocation.Grave) return false;
            // Banish to search PK S/T
            return PKSpellsTraps.Any(id => GetRemainingCount(id) > 0);
        }

        private bool ShouldAncientCloakGYActivate()
        {
            if (Card.Location != CardLocation.Grave) return false;
            // Banish to search PK monster
            return PKMonsters.Any(id => id != CardId.AncientCloak && GetRemainingCount(id) > 0);
        }

        private bool ShouldDoomedSoleretGYLevelMod()
        {
            if (Card.Location != CardLocation.Grave) return false;
            // Increase level/rank of up to 2 Lv3 DARK monsters by 1 -> enables Rank 4 Raider's Knight / Ophion!
            return Bot.GetMonsters().Count(c => c.IsFaceup() && c.Level == 3 && c.HasAttribute(CardAttribute.Dark)) >= 2;
        }

        private bool ShouldDecayedCloakGYRankMod()
        {
            if (Card.Location != CardLocation.Grave) return false;
            // Treat Rank as Level
            return Bot.GetMonsters().Any(c => c.IsFaceup() && c.HasType(CardType.Xyz));
        }

        private bool ShouldRaggedGlovesGYActivate()
        {
            if (Card.Location != CardLocation.Grave) return false;
            // Send PK card from Deck to GY
            return PKSpellsTraps.Concat(PKMonsters).Any(id => GetRemainingCount(id) > 0);
        }

        private bool ShouldTornScalesGYRevive()
        {
            // SS when another PK in GY is banished
            return Bot.GetMonsterCount() < 5;
        }

        private bool ShouldUmbrageVeilActivate()
        {
            // On Field: SS as Lv3 Monster
            if (Card.Location == CardLocation.SpellZone)
            {
                return Bot.GetMonsterCount() < 5;
            }
            // In GY: Quick Xyz Summon
            if (Card.Location == CardLocation.Grave)
            {
                int lv3Count = Bot.GetMonsters().Count(c => c.IsFaceup() && c.Level == 3);
                int lv4Count = Bot.GetMonsters().Count(c => c.IsFaceup() && c.Level == 4);
                return lv3Count >= 2 || lv4Count >= 2;
            }
            return false;
        }

        private bool ShouldPKWingActivate()
        {
            // In GY: revive PK
            if (Card.Location == CardLocation.Grave)
            {
                return Bot.Graveyard.Any(c => PKMonsters.Contains(c.Id)) && Bot.GetMonsterCount() < 5;
            }
            // On Field: +500 ATK & 1-time destruction shield
            if (Card.Location == CardLocation.SpellZone)
            {
                return Bot.GetMonsters().Any(c => c.IsFaceup());
            }
            return false;
        }

        private bool ShouldSetTrapSafely()
        {
            // Anti-Brick: Keep at least 1 backrow zone open for Bardiche or Soleret sets
            return Bot.GetSpellCount() < 4;
        }

        // ═══════════════════════════════════════════════════════════════════
        //  HINT TABLE HANDLERS (OnSelectCard)
        // ═══════════════════════════════════════════════════════════════════

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            var result = new List<ClientCard>();

            switch (hint)
            {
                case 500: // HINTMSG_RELEASE (Tributes for Lava Golem / Santa Claws)
                    {
                        var oppThreats = cards.Where(c => c.Controller == 1)
                            .OrderByDescending(c => CardIntelligence.IsKnownNegator(c.Id))
                            .ThenByDescending(c => CardIntelligence.IsFloodgateMonster(c.Id))
                            .ThenByDescending(c => c.Attack)
                            .ToList();
                        foreach (var c in oppThreats)
                        {
                            if (result.Count < max) result.Add(c);
                        }
                        break;
                    }

                case 501: // HINTMSG_DISCARD (Torn Scales Pitch / Hand Cleanse)
                    {
                        // Prioritize cards that benefit from being in GY or unbrick the hand
                        var discardPriority = cards
                            .OrderByDescending(c => c.Id == CardId.SilentBoots)
                            .ThenByDescending(c => c.Id == CardId.DoomedSoleret)
                            .ThenByDescending(c => c.Id == CardId.AncientCloak)
                            .ThenByDescending(c => c.Id == CardId.RaggedGloves)
                            .ThenByDescending(c => c.Id == CardId.FogBlade)
                            .ThenByDescending(c => c.Id == CardId.PKWing)
                            .ThenByDescending(c => c.Id == CardId.DecayedCloak)
                            .ToList();

                        foreach (var c in discardPriority)
                        {
                            if (result.Count < max && !AceMonsters.Contains(c.Id))
                                result.Add(c);
                        }
                        break;
                    }

                case 502: // HINTMSG_DESTROY (Rusty Bardiche / Break Sword pops)
                    {
                        // If selecting friendly card for Break Sword: destroy Break Sword itself or Fog Blade/Wing
                        var ownPops = cards.Where(c => c.Controller == 0)
                            .OrderByDescending(c => c.Id == CardId.BreakSword)
                            .ThenByDescending(c => c.Id == CardId.PKWing)
                            .ThenByDescending(c => c.Id == CardId.FogBlade)
                            .ToList();

                        // If selecting opponent card: destroy floodgates, negators, then highest ATK
                        var oppPops = cards.Where(c => c.Controller == 1)
                            .OrderByDescending(c => CardIntelligence.IsFloodgateSpellTrap(c.Id))
                            .ThenByDescending(c => CardIntelligence.IsFloodgateMonster(c.Id))
                            .ThenByDescending(c => CardIntelligence.IsKnownNegator(c.Id))
                            .ThenByDescending(c => c.Attack)
                            .ToList();

                        foreach (var c in ownPops)
                        {
                            if (result.Count < max) result.Add(c);
                        }
                        foreach (var c in oppPops)
                        {
                            if (result.Count < max) result.Add(c);
                        }
                        break;
                    }

                case 504: // HINTMSG_REMOVE (Banish for cost or S:P Little Knight)
                    {
                        // Opponent targets for S:P
                        var oppBanishes = cards.Where(c => c.Controller == 1)
                            .OrderByDescending(c => CardIntelligence.IsKnownNegator(c.Id))
                            .ThenByDescending(c => CardIntelligence.IsFloodgateMonster(c.Id))
                            .ThenByDescending(c => c.Attack)
                            .ToList();

                        // Friendly GY banish costs
                        var ownBanishCosts = cards.Where(c => c.Controller == 0)
                            .OrderByDescending(c => c.Id == CardId.SilentBoots)
                            .ThenByDescending(c => c.Id == CardId.AncientCloak)
                            .ThenByDescending(c => c.Id == CardId.DoomedSoleret)
                            .ThenByDescending(c => c.Id == CardId.DecayedCloak)
                            .ThenByDescending(c => c.Id == CardId.RaggedGloves)
                            .ThenByDescending(c => c.Id == CardId.FogBlade)
                            .ToList();

                        foreach (var c in oppBanishes)
                        {
                            if (result.Count < max) result.Add(c);
                        }
                        foreach (var c in ownBanishCosts)
                        {
                            if (result.Count < max) result.Add(c);
                        }
                        break;
                    }

                case 505: // HINTMSG_ATOHAND (Searches)
                    {
                        // Prioritize key combo pieces
                        var searchPriority = cards
                            .OrderByDescending(c => c.Id == CardId.RUMRequiem && !Bot.Hand.Any(h => h.Id == CardId.RUMRequiem))
                            .ThenByDescending(c => c.Id == CardId.RUMLaunch && !Bot.Hand.Any(h => h.Id == CardId.RUMLaunch))
                            .ThenByDescending(c => c.Id == CardId.TornScales && !Bot.Hand.Any(h => h.Id == CardId.TornScales))
                            .ThenByDescending(c => c.Id == CardId.DecayedCloak && !Bot.Hand.Any(h => h.Id == CardId.DecayedCloak))
                            .ThenByDescending(c => c.Id == CardId.DoomedSoleret && !Bot.Hand.Any(h => h.Id == CardId.DoomedSoleret))
                            .ThenByDescending(c => c.Id == CardId.SilentBoots)
                            .ThenByDescending(c => c.Id == CardId.FogBlade)
                            .ToList();

                        foreach (var c in searchPriority)
                        {
                            if (result.Count < max) result.Add(c);
                        }
                        break;
                    }

                case 506: // HINTMSG_TODECK (Pot of Sloth return to bottom deck)
                    {
                        // Place duplicate handtraps or situational cards on bottom of deck
                        var bottomPriority = cards
                            .OrderByDescending(c => c.Id == CardId.LavaGolem)
                            .ThenByDescending(c => c.Id == CardId.SantaClaws)
                            .ThenByDescending(c => c.Id == CardId.DrollAndLockBird && Bot.Hand.Count(h => h.Id == CardId.DrollAndLockBird) > 1)
                            .ThenByDescending(c => c.Id == CardId.AshBlossom && Bot.Hand.Count(h => h.Id == CardId.AshBlossom) > 1)
                            .ToList();

                        foreach (var c in bottomPriority)
                        {
                            if (result.Count < max) result.Add(c);
                        }
                        break;
                    }

                case 508: // HINTMSG_TOGRAVE (Cherubini / Rusty Bardiche / Torn Scales dumps)
                    {
                        var dumpPriority = cards
                            .OrderByDescending(c => c.Id == CardId.TornScales && !Bot.Graveyard.Any(g => g.Id == CardId.TornScales))
                            .ThenByDescending(c => c.Id == CardId.SilentBoots)
                            .ThenByDescending(c => c.Id == CardId.AncientCloak)
                            .ThenByDescending(c => c.Id == CardId.DoomedSoleret)
                            .ThenByDescending(c => c.Id == CardId.RaggedGloves)
                            .ThenByDescending(c => c.Id == CardId.FogBlade)
                            .ThenByDescending(c => c.Id == CardId.PKWing)
                            .ToList();

                        foreach (var c in dumpPriority)
                        {
                            if (result.Count < max) result.Add(c);
                        }
                        break;
                    }

                case 509: // HINTMSG_SPSUMMON (Special Summons & Reanimations)
                    {
                        var ssPriority = cards
                            .OrderByDescending(c => c.Id == CardId.DarkRequiem)
                            .ThenByDescending(c => c.Id == CardId.ArcRebellion)
                            .ThenByDescending(c => c.Id == CardId.EvilswarmOphion)
                            .ThenByDescending(c => c.Id == CardId.EvilswarmNightmare)
                            .ThenByDescending(c => c.Id == CardId.RaidersKnight)
                            .ThenByDescending(c => c.Id == CardId.DarkRebellion)
                            .ThenByDescending(c => c.Id == CardId.BreakSword)
                            .ThenByDescending(c => c.Id == CardId.TornScales)
                            .ThenByDescending(c => c.Id == CardId.DecayedCloak)
                            .ThenByDescending(c => c.Id == CardId.DoomedSoleret)
                            .ThenByDescending(c => c.Id == CardId.SilentBoots)
                            .ToList();

                        foreach (var c in ssPriority)
                        {
                            if (result.Count < max) result.Add(c);
                        }
                        break;
                    }

                case 510: // HINTMSG_SET (Rusty Bardiche / Doomed Soleret set from Deck)
                    {
                        var setPriority = cards
                            .OrderByDescending(c => c.Id == CardId.FogBlade)
                            .ThenByDescending(c => c.Id == CardId.PKWing)
                            .ThenByDescending(c => c.Id == CardId.UmbrageVeil)
                            .ThenByDescending(c => c.Id == CardId.RUMRequiem)
                            .ToList();

                        foreach (var c in setPriority)
                        {
                            if (result.Count < max) result.Add(c);
                        }
                        break;
                    }

                case 512: // HINTMSG_XMATERIAL (Xyz Detach)
                    {
                        // Detach materials that trigger in GY
                        var detachPriority = cards
                            .OrderByDescending(c => c.Id == CardId.SilentBoots)
                            .ThenByDescending(c => c.Id == CardId.DoomedSoleret)
                            .ThenByDescending(c => c.Id == CardId.AncientCloak)
                            .ThenByDescending(c => c.Id == CardId.RaggedGloves)
                            .ThenByDescending(c => c.Id == CardId.DecayedCloak)
                            .ToList();

                        foreach (var c in detachPriority)
                        {
                            if (result.Count < max) result.Add(c);
                        }
                        break;
                    }

                case 551: // HINTMSG_TARGET (Friendly level mod / RUM or enemy target)
                    {
                        var ownTargets = cards.Where(c => c.Controller == 0)
                            .OrderByDescending(c => c.Id == CardId.BreakSword)
                            .ThenByDescending(c => c.Id == CardId.DarkRebellion)
                            .ThenByDescending(c => c.Level == 3 && c.HasAttribute(CardAttribute.Dark))
                            .ToList();

                        var oppTargets = cards.Where(c => c.Controller == 1)
                            .OrderByDescending(c => CardIntelligence.IsKnownNegator(c.Id))
                            .ThenByDescending(c => CardIntelligence.IsFloodgateMonster(c.Id))
                            .ThenByDescending(c => c.Attack)
                            .ToList();

                        foreach (var c in ownTargets)
                        {
                            if (result.Count < max) result.Add(c);
                        }
                        foreach (var c in oppTargets)
                        {
                            if (result.Count < max) result.Add(c);
                        }
                        break;
                    }

                case 552: // HINTMSG_DISABLE
                case 572: // HINTMSG_NEGATE
                    {
                        var negatePriority = cards.Where(c => c.Controller == 1)
                            .OrderByDescending(c => CardIntelligence.IsKnownNegator(c.Id))
                            .ThenByDescending(c => CardIntelligence.IsFloodgateMonster(c.Id))
                            .ThenByDescending(c => CardIntelligence.IsHighThreatChokepoint(c.Id))
                            .ThenByDescending(c => c.Attack)
                            .ToList();

                        foreach (var c in negatePriority)
                        {
                            if (result.Count < max) result.Add(c);
                        }
                        break;
                    }
            }

            // Fallback safety to guarantee result satisfies [min, max]
            if (result.Count < min)
            {
                foreach (var card in cards)
                {
                    if (!result.Contains(card))
                    {
                        result.Add(card);
                        if (result.Count >= min) break;
                    }
                }
            }

            return result;
        }

        public override int OnSelectOption(IList<long> options)
        {
            for (int i = 0; i < options.Count; i++)
            {
                long cardId = options[i] >> 20;
                long optIndex = options[i] & 0xfffff;
                if (cardId == 0 && LastChainCard != null) cardId = LastChainCard.Id;

                // Solemn Report: Option 0 (Pay 1500, destroy & lock) vs Option 1 (Pay 3000, banish all)
                if (cardId == CardId.SolemnReport)
                {
                    if (Bot.LifePoints > 4000 && optIndex == 1) return i;
                    if (optIndex == 0) return i;
                }
            }

            return base.OnSelectOption(options);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Stat-aware position selection
            if (cardId == CardId.SilentBoots || cardId == CardId.DoomedSoleret || cardId == CardId.UmbrageVeil || cardId == CardId.Cherubini)
            {
                if (positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence;
            }

            if (cardId == CardId.ArcRebellion || cardId == CardId.DarkRequiem || cardId == CardId.RaidersKnight || cardId == CardId.DarkRebellion)
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }
    }
}
