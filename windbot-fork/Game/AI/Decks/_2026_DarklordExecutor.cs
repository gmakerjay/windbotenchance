using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    // ============================================================
    // CARD AUDIT — 2026_Darklord
    // ============================================================
    // | Card Name                                      | Type    | OPT? | Cost               | Effect Summary                                | Activate When                     | NEVER Activate When                    |
    // |------------------------------------------------|---------|------|--------------------|-----------------------------------------------|-----------------------------------|----------------------------------------|
    // | Darklord Morningstar (25451652)                | Monster | Yes  | None               | Tribute Summon: SS Darklords from H/D equal to| Tribute Summoned, opp has face-up | SS blocked / no Darklord left          |
    // |                                                |         |      |                    | opp's face-up Effect monsters                 | Effect Monster                    |                                        |
    // |                                                |         | Yes  | None               | Ignition: mill equal to face-up Darklords,    | Main Phase                        | No face-up Darklord on field           |
    // |                                                |         |      |                    | gain 500 LP per Darklord milled               |                                   |                                        |
    // |                                                |   CANNOT BE SPECIAL SUMMONED                       | Also cannot be revived from GY                |                                   |                                        |
    // | Darklord Ixchel (52840267)                     | Monster | Yes  | Discard self+DL    | Draw 2 cards                                  | Hand, have another Darklord       | No other Darklord to discard           |
    // |                                                |         | Yes  | 1000 LP            | Quick: copy effect of 1 Darklord S/T from GY | Main Phase/opp turn                | No Darklord S/T target in GY           |
    // | Darklord Gulgolet (84031359)                   | Monster | Yes  | None               | SS: 2 Darklord Tokens (Fairy-lock rest of turn)| When Special Summoned             | SS blocked / no zones                  |
    // |                                                |         | Yes  | None               | Sent to GY: search Darklord/Forbidden QP      | When sent to GY (cost/effect)     | No target in Deck                      |
    // | Darklord Djehuty (10426067)                    | Monster | Yes  | None               | Normal/SS: SS 1 Darklord from Deck in DEF     | Summoned/Special Summoned         | SS blocked / no Darklord in Deck       |
    // |                                                |         | Yes  | Banish self (GY)   | GY: add Darklord/Forbidden QP to hand         | Have DARK Fairy Fusion face-up    | No Darklord S/T target in GY           |
    // | Vidolium (70488851)                            | Monster | Yes  | Return F/S/X       | SS from ED/GY by returning F/S/X from field/GY| Have F/S/X on field/GY            | No F/S/X to return                     |
    // |                                                |         | Yes  | None               | Pend: destroy self + SS Power Patron          | In Pendulum Zone                  | No Power Patron target                 |
    // | Herald of Orange Light (17266660)              | Monster | Yes  | Discard self+Fairy | Hand: negate monster effect activation         | Opponent activates monster effect | Self-chain                             |
    // | Mulcharmy Fuwalos (42141493)                   | Monster | Yes  | None               | Hand: draw when opp SSs                       | Opponent's turn, no field         | We have cards on field                 |
    // | Ash Blossom (14558128)                         | Monster | Yes  | None               | Hand: negate search/SS/mill from Deck          | Opponent uses such effect         | Self-chain                             |
    // | Banishment of the Darklords (87112784)         | Spell   | Yes  | None               | Search any Darklord card except itself        | Main Phase                        | No Darklord card left in Deck          |
    // | Darklord Contact (14517422)                    | Spell   | Yes  | None               | SS 1 Darklord from GY in DEF                  | Main Phase, have target in GY     | No Darklord in GY                      |
    // | Darklord Dance (99941223)                      | Spell   | Yes  | Banish materials   | Fusion Summon DARK Fairy Fusion (+1000 ATK)   | Main Phase, have materials        | SS blocked / no valid Fusion target    |
    // | Darklord Rebellion (50501121)                  | Spell   | Yes  | Send Darklord H/F  | Destroy 1 card on field                       | Main Phase, have Darklord to send | No target on field                     |
    // | Sanctified Darklord (48152161)                 | Trap    | Yes  | Send Darklord H/F  | Negate 1 face-up Effect Monster, gain its ATK | Opponent has effect monster       | No Darklord to send                    |
    // | Terminus Portal (25661743)                     | Spell   | Yes  | None               | Send Power Patron Deck/ED -> search DARK Fairy| Main Phase, have Power Patron    | No Power Patron in Deck/ED / no Fairy  |
    // | Forbidden Droplet (24299458)                   | Spell   | Yes  | Send H/F multiples | Negate ATK/effects of opp monsters            | Opponent has face-up effect mons  | No cards to send as cost               |
    // | Forbidden Crown (98829635)                     | Spell   | Yes  | None               | Negate face-up monster; it's invincible       | Any face-up monster               | No valid target                         |
    // | Super Polymerization (48130397)                | Spell   | Yes  | Discard 1          | Fusion using opponent's monsters as material  | Opponent has face-up monsters     | No valid Fusion target in ED           |
    // | Foolish Burial (81439174)                      | Spell   | No   | None               | Send 1 monster from Deck to GY                | Main Phase, have useful send      | No useful monster in Deck              |
    // | Dominus Impulse (40366667)                     | Trap    | Yes  | None               | Hand: negate SS effect; locks LIGHT/EARTH/WIND| Opponent SS effect                | Self-chain                             |
    // | Darklord Eveningstar (10136446)                | Fusion  | Yes  | None               | Set 1 Darklord S + 1 Darklord T from Deck     | Fusion Summoned on field          | No Darklord S/T left in Deck           |
    // |                                                |         | Yes  | 1000 LP            | Quick: copy Darklord S/T from GY              | Main Phase/opp turn                | No Darklord S/T target in GY           |
    // | The First Darklord (4167084)                   | Fusion  | Yes  | None               | If Morningstar used: destroy all opp cards    | Fusion Summoned w/ Morningstar    | Morningstar not used                   |
    // |                                                |         | Yes  | 1000 LP            | Quick: SS 1 Fairy from hand/GY in DEF         | Main Phase (Quick)                | SS blocked / no Fairy target           |
    // | Condemned Darklord (35306215)                  | Link-2  | Yes  | Discard 1          | Search or send 1 Darklord monster             | Main Phase, have card to discard  | No Darklord monster in Deck            |
    // |                                                |         | Const| Banish 2 GY Fairies| Tribute Summon 2-tribute Fairy w/o tribute   | Have 2 Fairies in GY              | Less than 2 Fairies in GY              |
    // | Super Starslayer TY-PHON (93039339)            | Xyz R12 | Yes  | Detach 1           | Return 1 monster on field to hand             | Main Phase, have material         | No monster on field                    |
    // | Herald of Mirage Lights (46935289)             | Link-2  | Yes  | Discard 1 Fairy    | Negate S/T activation                         | Opponent activates S/T            | Self-chain                             |
    // | Protector of The Agents - Moon (90290572)       | Link-2  | Yes  | Tribute 1 Fairy    | Destroy 1 opponent card                       | Main Phase, have Fairy to tribute  | No opponent target                     |
    // | Rahamu (53904087)                              | Link-2  | Yes  | None               | Once: Normal Summon L5+ without Tribute       | Main Phase, have L5+ in hand      | No L5+ monster in hand                 |
    // ============================================================
    // ACE CARDS: Primary: The First Darklord / Secondary: Darklord Eveningstar / Tertiary: Darklord Morningstar, Super Starslayer TY-PHON
    // COMBO STARTERS: 1. Banishment of the Darklords 2. Darklord Djehuty 3. Terminus Portal 4. Darklord Contact 5. Darklord Ixchel
    // CHOKEPOINTS: Banishment search negated = engine stalls. Djehuty SS negated = no extension.
    // DOMINUS IMPULSE WARNING: Hand trap locks into LIGHT/EARTH/WIND (conflicts with DARK Fairy) -> NEVER activate from hand on our turn!
    // WIN CONDITION: Establish The First Darklord (5000 ATK, blanket targeting protection) + Darklord Eveningstar (4000 ATK) with Set Sanctified + Rebellion, swarm using Morningstar / First Darklord, then OTK with massive Fairy attacks.
    // GOING 1ST END BOARD: The First Darklord + Darklord Eveningstar + Set Sanctified + Set Rebellion + GY Copy ready (5-7 disruptions).
    // GOING 2ND GAMEPLAN: Break board with Super Polymerization / Droplet / First Darklord board wipe, then push for game with 3000-5000 ATK bodies.
    // ============================================================

    [Deck("2026_Darklord", "2026_Darklord")]
    public class _2026_DarklordExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck — Darklord Archetype
            public const int DarklordMorningstar = 25451652;
            public const int DarklordIxchel = 52840267;
            public const int DarklordGulgolet = 84031359;
            public const int DarklordDjehuty = 10426067;
            public const int BanishmentOfTheDarklords = 87112784;
            public const int DarklordDance = 99941223;
            public const int DarklordContact = 14517422;
            public const int DarklordRebellion = 50501121;
            public const int TheSanctifiedDarklord = 48152161;

            // Main Deck — Tech / Engine
            public const int VidriumThePowerPatronOfChaosExtermination = 70488851;
            public const int UnleashedPowerPatronPortalTerminus = 25661743;
            public const int HeraldOfOrangeLight = 17266660;
            public const int MulcharmyFuwalos = 42141493;
            public const int AshBlossom = 14558128;
            public const int ForbiddenDroplet = 24299458;
            public const int ForbiddenCrown = 98829635;
            public const int SuperPolymerization = 48130397;
            public const int FoolishBurial = 81439174;
            public const int DominusImpulse = 40366667;

            // Extra Deck — Darklord
            public const int DarklordEveningstar = 10136446;
            public const int TheFirstDarklord = 4167084;
            public const int CondemnedDarklord = 35306215;

            // Extra Deck — Generic Tech
            public const int SuperStarslayerTYPHON = 93039339;
            public const int MudragonOfTheSwamp = 54757758;
            public const int GaruraWingsOfResonantLife = 11765832;
            public const int SeaMonsterOfTheseus = 96334243;
            public const int SecreterionDragon = 89851827;
            public const int TheDragonThatDevoursTheDogma = 76666602;
            public const int HeraldOfMirageLights = 46935289;
            public const int LahamuTheMessengerOfSacredScripture = 53904087;
            public const int ProtectorOfTheAgentsMoon = 90290572;

            // Side Deck — referenced for discard priority
            public const int DrollAndLockBird = 94145021;
            public const int CalledByTheGrave = 24224830;
        }

        /// <summary>
        /// Cards that CANNOT be Special Summoned (Morningstar is Tribute Summon only).
        /// </summary>
        private static readonly HashSet<int> CannotBeSpecialSummoned = new HashSet<int>
        {
            CardId.DarklordMorningstar
        };

        private static readonly int[] HandTraps = {
            CardId.AshBlossom,
            CardId.MulcharmyFuwalos,
            CardId.DrollAndLockBird,
            CardId.CalledByTheGrave,
            CardId.HeraldOfOrangeLight,
            CardId.DominusImpulse
        };

        // Dominus Impulse lock: if activated from hand, we can only SS LIGHT/EARTH/WIND
        private bool _dominusImpulseActivatedThisTurnFromHand = false;

        // Once-per-turn trackers
        private bool _morningstarSummonUsed = false;
        private bool _morningstarMillUsed = false;
        private bool _ixchelDrawUsed = false;
        private bool _ixchelCopyUsed = false;
        private bool _djehutySummonUsed = false;
        private bool _djehutyGYUsed = false;
        private bool _gulgoletTokensUsed = false;
        private bool _gulgoletSearchUsed = false;
        private bool _danceUsed = false;
        private bool _banishmentUsed = false;
        private bool _contactUsed = false;
        private bool _rebellionUsed = false;
        private bool _sanctifiedUsed = false;
        private bool _terminusUsed = false;
        private bool _crownUsed = false;
        private bool _superPolyUsed = false;
        private bool _foolishUsed = false;
        private bool _vidriumGYUsed = false;
        private bool _eveningstarSetUsed = false;
        private bool _eveningstarCopyUsed = false;
        private bool _firstDarklordSSUsed = false;
        private bool _condemnedSearchUsed = false;
        private bool _heraldMirageNegateUsed = false;
        private bool _moonDestroyUsed = false;
        private bool _lahamuFreeSummonUsed = false;
        private bool _lahamuQuickSummonUsed = false;
        private bool _typhonBounceUsed = false;

        public override bool OnSelectHand()
        {
            // Darklord strongly prefers going first to establish The First Darklord / Eveningstar + backrow
            return true;
        }

        public override void OnChaining(int player, ClientCard card)
        {
            if (card != null && card.Id == CardId.DominusImpulse && card.Controller == 0 && card.Location == CardLocation.Hand)
            {
                _dominusImpulseActivatedThisTurnFromHand = true;
            }
            base.OnChaining(player, card);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _morningstarSummonUsed = false;
            _morningstarMillUsed = false;
            _ixchelDrawUsed = false;
            _ixchelCopyUsed = false;
            _djehutySummonUsed = false;
            _djehutyGYUsed = false;
            _gulgoletTokensUsed = false;
            _gulgoletSearchUsed = false;
            _danceUsed = false;
            _banishmentUsed = false;
            _contactUsed = false;
            _rebellionUsed = false;
            _sanctifiedUsed = false;
            _terminusUsed = false;
            _crownUsed = false;
            _superPolyUsed = false;
            _foolishUsed = false;
            _vidriumGYUsed = false;
            _eveningstarSetUsed = false;
            _eveningstarCopyUsed = false;
            _firstDarklordSSUsed = false;
            _condemnedSearchUsed = false;
            _heraldMirageNegateUsed = false;
            _moonDestroyUsed = false;
            _lahamuFreeSummonUsed = false;
            _lahamuQuickSummonUsed = false;
            _typhonBounceUsed = false;
            _dominusImpulseActivatedThisTurnFromHand = false;

            if (ShouldGoBreakBoard)
            {
                _superPolyUsed = false;
            }
        }

        private bool IsDominusImpulseLocked()
        {
            return _dominusImpulseActivatedThisTurnFromHand;
        }

        public _2026_DarklordExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Register Ace Cards for HeuristicGuard
            HeuristicGuard.RegisterAceCards(
                CardId.TheFirstDarklord,
                CardId.DarklordEveningstar,
                CardId.DarklordMorningstar,
                CardId.SuperStarslayerTYPHON
            );

            // ── Combo Router: 4 Strategic Lines ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Djehuty-Gulgolet-Fusion-Setup",
                RequiredCards = new List<int> { CardId.DarklordDjehuty, CardId.BanishmentOfTheDarklords },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.BanishmentOfTheDarklords, ActionType = ExecutorType.Activate, Description = "Search Darklord Djehuty / Dance" },
                    new() { CardId = CardId.DarklordDjehuty, ActionType = ExecutorType.Summon, Description = "Normal Summon Djehuty -> SS Gulgolet -> 2 Tokens" },
                    new() { CardId = CardId.CondemnedDarklord, ActionType = ExecutorType.SpSummon, Description = "Link Summon Condemned -> Gulgolet search Dance" },
                    new() { CardId = CardId.DarklordDance, ActionType = ExecutorType.Activate, Description = "Fusion Summon Darklord Eveningstar / The First Darklord" }
                },
                EndBoardScore = 95
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Morningstar-Tribute-BoardWipe",
                RequiredCards = new List<int> { CardId.DarklordMorningstar, CardId.CondemnedDarklord },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.CondemnedDarklord, ActionType = ExecutorType.SpSummon, Description = "Link Summon Condemned Darklord" },
                    new() { CardId = CardId.DarklordMorningstar, ActionType = ExecutorType.Summon, Description = "Tribute Summon Morningstar via Condemned GY banish" },
                    new() { CardId = CardId.DarklordDance, ActionType = ExecutorType.Activate, Description = "Fusion Summon The First Darklord -> Board Wipe!" }
                },
                EndBoardScore = 90
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "SuperPoly-FirstDarklord-OTK",
                RequiredCards = new List<int> { CardId.SuperPolymerization },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.SuperPolymerization, ActionType = ExecutorType.Activate, Description = "Board Break with Super Poly" },
                    new() { CardId = CardId.DarklordDance, ActionType = ExecutorType.Activate, Description = "Fusion Summon The First Darklord (5000 ATK)" }
                },
                EndBoardScore = 85
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Ixchel-Contact-GrindControl",
                RequiredCards = new List<int> { CardId.DarklordIxchel, CardId.DarklordContact },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.DarklordIxchel, ActionType = ExecutorType.Activate, Description = "Discard Ixchel + Darklord to draw 2" },
                    new() { CardId = CardId.DarklordContact, ActionType = ExecutorType.Activate, Description = "Revive Ixchel in DEF for GY copying" }
                },
                EndBoardScore = 75
            });

            // ── Bait Planner ──
            BaitPlanner.RegisterComboStarters(CardId.BanishmentOfTheDarklords, CardId.DarklordDjehuty, CardId.UnleashedPowerPatronPortalTerminus, CardId.DarklordMorningstar, CardId.DarklordIxchel);
            BaitPlanner.RegisterBaitCards(CardId.UnleashedPowerPatronPortalTerminus, CardId.FoolishBurial);

            // ── Chain Advisor ──
            ChainAdvisor.RegisterHighValueTargets(CardId.BanishmentOfTheDarklords, CardId.DarklordDjehuty, CardId.DarklordDance, CardId.CondemnedDarklord);

            // ============================================================
            // EXECUTOR PIPELINE
            // ============================================================

            // TIER 1: Hand Traps & Negations (never chain to ourselves)
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.DominusImpulse, DominusImpulseEffect);
            AddExecutor(ExecutorType.Activate, CardId.HeraldOfOrangeLight, HeraldOfOrangeLightEffect);

            // TIER 2: Quick Effects & Disruptions (Enemy Turn / Chain Responses)
            AddExecutor(ExecutorType.Activate, CardId.DarklordEveningstar, EveningstarCopyEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarklordIxchel, IxchelCopyEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheFirstDarklord, FirstDarklordQuickAndWipeEffect);
            AddExecutor(ExecutorType.Activate, CardId.HeraldOfMirageLights, HeraldMirageLightsEffect);
            AddExecutor(ExecutorType.Activate, CardId.ProtectorOfTheAgentsMoon, MoonDestroyEffect);
            AddExecutor(ExecutorType.Activate, CardId.LahamuTheMessengerOfSacredScripture, LahamuQuickSummonEffect);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletEffect);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenCrown, ForbiddenCrownEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheSanctifiedDarklord, SanctifiedDarklordEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarklordRebellion, DarklordRebellionEffect);

            // TIER 3: Pre-Summon Starters, Draw Effects & Revivals
            AddExecutor(ExecutorType.Activate, CardId.DarklordIxchel, IxchelDrawEffect);
            AddExecutor(ExecutorType.Activate, CardId.BanishmentOfTheDarklords, BanishmentEffect);
            AddExecutor(ExecutorType.Activate, CardId.UnleashedPowerPatronPortalTerminus, TerminusEffect);
            AddExecutor(ExecutorType.Activate, CardId.FoolishBurial, FoolishBurialEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarklordContact, DarklordContactEffect);
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationEffect);

            // TIER 4: Normal Summons (Engine Starters & Tributes)
            AddExecutor(ExecutorType.Summon, CardId.DarklordDjehuty, ShouldSummonDjehuty);
            AddExecutor(ExecutorType.Summon, CardId.DarklordMorningstar, ShouldSummonMorningstar);
            AddExecutor(ExecutorType.Summon, CardId.DarklordIxchel, ShouldSummonIxchel);

            // TIER 5: Monster Trigger Effects on Summon / Sent to GY
            AddExecutor(ExecutorType.Activate, CardId.DarklordDjehuty, DjehutySummonEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarklordGulgolet, GulgoletTokensEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarklordGulgolet, GulgoletSearchEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarklordEveningstar, EveningstarSetEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarklordMorningstar, MorningstarSummonEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarklordMorningstar, MorningstarMillEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarklordDjehuty, DjehutyGYEffect);

            // TIER 6: Key Extra Deck Summons & Primary Fusion
            AddExecutor(ExecutorType.SpSummon, CardId.CondemnedDarklord, CondemnedDarklordSummon);
            AddExecutor(ExecutorType.Activate, CardId.CondemnedDarklord, CondemnedDarklordEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarklordDance, DarklordDanceEffect);
            AddExecutor(ExecutorType.Activate, CardId.VidriumThePowerPatronOfChaosExtermination, VidriumGYEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.SuperStarslayerTYPHON, TyphonSummon);
            AddExecutor(ExecutorType.Activate, CardId.SuperStarslayerTYPHON, TyphonBounceEffect);

            // Secondary Extra Deck Summons (only when Dance has been used or cannot be played)
            AddExecutor(ExecutorType.SpSummon, CardId.HeraldOfMirageLights, HeraldMirageLightsSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ProtectorOfTheAgentsMoon, MoonSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.LahamuTheMessengerOfSacredScripture, LahamuSummon);
            AddExecutor(ExecutorType.Activate, CardId.LahamuTheMessengerOfSacredScripture, LahamuFreeSummonEffect);

            // TIER 7: Fallback Normal Summons (Djehuty ONLY — NEVER Handtraps)
            AddExecutor(ExecutorType.Summon, CardId.DarklordDjehuty);

            // TIER 8: Backrow Sets & Repos
            AddExecutor(ExecutorType.SpellSet, CardId.TheSanctifiedDarklord);
            AddExecutor(ExecutorType.SpellSet, CardId.DarklordRebellion);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenCrown);
            AddExecutor(ExecutorType.SpellSet, CardId.DarklordDance);
            AddExecutor(ExecutorType.SpellSet, CardId.DarklordContact);
            AddExecutor(ExecutorType.SpellSet, CardId.BanishmentOfTheDarklords);

            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        // ==========================================
        //  TIER 1: Hand Traps & Negations
        // ==========================================

        private bool MulcharmyEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (IsDominusImpulseLocked()) return false;
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }

        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.LastChainPlayer == 1 && DefaultAshBlossomAndJoyousSpring();
        }

        private bool DominusImpulseEffect()
        {
            // CRITICAL: NEVER activate Dominus Impulse from hand during our turn (locks us into LIGHT/EARTH/WIND)
            if (Card.Location == CardLocation.Hand && Duel.Player == 0) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.LastChainPlayer == 1)
            {
                DecisionTracer.TraceActivate("DominusImpulseEffect", "Negating opponent Special Summon effect via Dominus Impulse");
                return true;
            }
            return false;
        }

        private bool HeraldOfOrangeLightEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (IsDominusImpulseLocked()) return false;
            if (Duel.LastChainPlayer == 1)
            {
                bool hasFairy = Bot.Hand.Any(c => c != null && c != Card && c.HasRace(CardRace.Fairy));
                if (!hasFairy) return false;
                DecisionTracer.TraceActivate("HeraldOfOrangeLightEffect", "Negating monster effect activation via Herald of Orange Light");
                return true;
            }
            return false;
        }

        // ==========================================
        //  TIER 2: Quick Effects & Disruptions
        // ==========================================

        private bool EveningstarCopyEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (ActivateDescription != -1 && ActivateDescription == Util.GetStringId(CardId.DarklordEveningstar, 0)) return false;
            if (_eveningstarCopyUsed) return false;
            if (Bot.LifePoints <= 1000) return false;

            var gyTargets = Bot.Graveyard.Where(c => c != null && c.HasSetcode(0xef) && (c.IsSpell() || c.IsTrap())).ToList();
            if (gyTargets.Count == 0) return false;

            if (Duel.Player == 1)
            {
                bool oppHasMonsterTarget = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect) && !c.IsDisabled());
                bool oppHasAnyCard = Enemy.GetMonsterCount() > 0 || Enemy.GetSpells().Count > 0;

                bool hasSanctified = gyTargets.Any(c => c.Id == CardId.TheSanctifiedDarklord);
                bool hasRebellion = gyTargets.Any(c => c.Id == CardId.DarklordRebellion);

                if (hasSanctified && oppHasMonsterTarget)
                {
                    _eveningstarCopyUsed = true;
                    DecisionTracer.TraceActivate("EveningstarCopyEffect", "Opponent turn: copying Sanctified from GY");
                    return true;
                }

                if (hasRebellion && oppHasAnyCard)
                {
                    _eveningstarCopyUsed = true;
                    DecisionTracer.TraceActivate("EveningstarCopyEffect", "Opponent turn: copying Rebellion from GY");
                    return true;
                }

                return false;
            }

            if (gyTargets.Any(c => c.Id == CardId.BanishmentOfTheDarklords && !_banishmentUsed) ||
                gyTargets.Any(c => c.Id == CardId.DarklordDance && !_danceUsed) ||
                gyTargets.Any(c => c.Id == CardId.DarklordContact && !_contactUsed) ||
                gyTargets.Any(c => c.Id == CardId.DarklordRebellion && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpells().Count > 0)))
            {
                _eveningstarCopyUsed = true;
                DecisionTracer.TraceActivate("EveningstarCopyEffect", "Own turn: copying Darklord S/T from GY");
                return true;
            }

            return false;
        }

        private bool IxchelCopyEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_ixchelCopyUsed) return false;
            if (Bot.LifePoints <= 1000) return false;

            var gyTargets = Bot.Graveyard.Where(c => c != null && c.HasSetcode(0xef) && (c.IsSpell() || c.IsTrap())).ToList();
            if (gyTargets.Count == 0) return false;

            if (Duel.Player == 1)
            {
                bool oppHasMonsterTarget = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect) && !c.IsDisabled());
                bool oppHasAnyCard = Enemy.GetMonsterCount() > 0 || Enemy.GetSpells().Count > 0;

                bool hasSanctified = gyTargets.Any(c => c.Id == CardId.TheSanctifiedDarklord);
                bool hasRebellion = gyTargets.Any(c => c.Id == CardId.DarklordRebellion);

                if (hasSanctified && oppHasMonsterTarget)
                {
                    _ixchelCopyUsed = true;
                    DecisionTracer.TraceActivate("IxchelCopyEffect", "Opponent turn: copying Sanctified from GY");
                    return true;
                }

                if (hasRebellion && oppHasAnyCard)
                {
                    _ixchelCopyUsed = true;
                    DecisionTracer.TraceActivate("IxchelCopyEffect", "Opponent turn: copying Rebellion from GY");
                    return true;
                }

                return false;
            }

            if (gyTargets.Any(c => c.Id == CardId.BanishmentOfTheDarklords && !_banishmentUsed) ||
                gyTargets.Any(c => c.Id == CardId.DarklordDance && !_danceUsed) ||
                gyTargets.Any(c => c.Id == CardId.DarklordContact && !_contactUsed) ||
                gyTargets.Any(c => c.Id == CardId.DarklordRebellion && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpells().Count > 0)))
            {
                _ixchelCopyUsed = true;
                DecisionTracer.TraceActivate("IxchelCopyEffect", "Own turn: copying Darklord S/T from GY");
                return true;
            }

            return false;
        }

        private bool FirstDarklordQuickAndWipeEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;

            if (ActivateDescription != -1 && ActivateDescription == Util.GetStringId(CardId.TheFirstDarklord, 0))
            {
                DecisionTracer.TraceActivate("FirstDarklordQuickAndWipeEffect", "The First Darklord FULL BOARD WIPE!");
                return true;
            }

            if (ActivateDescription == -1 || ActivateDescription == Util.GetStringId(CardId.TheFirstDarklord, 1))
            {
                if (!Duel.IsMainPhase()) return false;
                if (_firstDarklordSSUsed) return false;
                if (Bot.LifePoints <= 1000) return false;
                if (IsSpecialSummonBlocked()) return false;

                bool hasTarget = Bot.Hand.Any(c => c != null && c.HasRace(CardRace.Fairy) && !CannotBeSpecialSummoned.Contains(c.Id))
                    || Bot.Graveyard.Any(c => c != null && c.HasRace(CardRace.Fairy) && c.IsCanRevive() && !CannotBeSpecialSummoned.Contains(c.Id));
                if (!hasTarget) return false;

                _firstDarklordSSUsed = true;
                DecisionTracer.TraceActivate("FirstDarklordQuickAndWipeEffect", "The First Darklord: Quick SS Fairy from Hand/GY in DEF");
                return true;
            }

            return false;
        }

        private bool HeraldMirageLightsEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (IsDominusImpulseLocked()) return false;
            if (_heraldMirageNegateUsed) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;

            bool hasFairy = Bot.Hand.Any(c => c != null && c.HasRace(CardRace.Fairy) && !IsAceCard(c));
            if (!hasFairy) return false;

            _heraldMirageNegateUsed = true;
            DecisionTracer.TraceActivate("HeraldMirageLightsEffect", "Negating S/T activation via Herald of Mirage Lights");
            return true;
        }

        private bool MoonDestroyEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (IsDominusImpulseLocked()) return false;
            if (_moonDestroyUsed) return false;

            bool hasFairy = Bot.GetMonsters().Any(c => c != null && c != Card && c.IsFaceup() && c.HasRace(CardRace.Fairy) && !IsAceCard(c));
            if (!hasFairy) return false;

            bool hasTarget = Enemy.GetSpells().Any(c => c != null && IsViableEffectTarget(c))
                || Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && IsViableEffectTarget(c));
            if (!hasTarget) return false;

            _moonDestroyUsed = true;
            DecisionTracer.TraceActivate("MoonDestroyEffect", "Tributing Fairy to destroy opponent card via Moon");
            return true;
        }

        private bool LahamuQuickSummonEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (ActivateDescription != -1 && ActivateDescription != Util.GetStringId(CardId.LahamuTheMessengerOfSacredScripture, 1)) return false;
            if (_lahamuQuickSummonUsed) return false;
            if (!Duel.IsMainPhase()) return false;

            bool hasMonster = Bot.Hand.Any(c => c != null && c.IsMonster() && c.Level >= 5 && c.HasAttribute(CardAttribute.Dark));
            if (!hasMonster) return false;

            _lahamuQuickSummonUsed = true;
            DecisionTracer.TraceActivate("LahamuQuickSummonEffect", "Lahamu Quick Normal Summon Level 5+ DARK monster");
            return true;
        }

        private bool ForbiddenDropletEffect()
        {
            ClientCard lastChain = Util.GetLastChainCard();
            if (lastChain != null && lastChain.Controller == 0) return false;

            var oppMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect) && !c.IsDisabled()).ToList();
            if (oppMonsters.Count == 0) return false;

            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1 && Duel.LastChainPlayer != 1)
            {
                bool hasThreat = oppMonsters.Any(c => c.IsMonsterDangerous() || c.Attack >= 2500);
                if (!hasThreat && Bot.Hand.Count <= 2) return false;
            }

            int availableForCost = Bot.Hand.Count(c => c != null && c != Card && !IsAceCard(c) && !HandTraps.Contains(c.Id) && c.Id != CardId.DarklordDance && c.Id != CardId.BanishmentOfTheDarklords)
                + Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c) && (c.HasType(CardType.Token) || c.Id == CardId.DarklordDjehuty))
                + Bot.GetSpells().Count(c => c != null && c.IsFaceup());
            if (availableForCost == 0) return false;

            DecisionTracer.TraceActivate("ForbiddenDropletEffect", $"Negating {oppMonsters.Count} opponent monsters via Droplet");
            return true;
        }

        private bool ForbiddenCrownEffect()
        {
            if (Card.Location != CardLocation.Hand && (Card.Location != CardLocation.SpellZone || Card.IsFacedown())) return false;
            if (_crownUsed) return false;
            ClientCard lastChain = Util.GetLastChainCard();
            if (lastChain != null && lastChain.Controller == 0) return false;

            // ONLY activate if OPPONENT has a face-up effect monster that is not disabled!
            var oppTargets = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect) && !c.IsDisabled()).ToList();
            if (oppTargets.Count == 0) return false;

            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1 && Duel.LastChainPlayer != 1)
            {
                if (!oppTargets.Any(c => c.Attack >= 2500 || c.IsMonsterDangerous())) return false;
            }

            _crownUsed = true;
            DecisionTracer.TraceActivate("ForbiddenCrownEffect", "Negating opponent monster via Forbidden Crown");
            return true;
        }

        private bool SanctifiedDarklordEffect()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && !Card.IsFacedown()) || Card.Location == CardLocation.SpellZone)
            {
                if (_sanctifiedUsed) return false;

                bool hasDarklordCost = Bot.Hand.Any(c => c != null && c != Card && c.HasSetcode(0xef) && c.IsMonster() && !IsAceCard(c))
                    || Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasSetcode(0xef) && c.IsMonster() && !IsAceCard(c));
                if (!hasDarklordCost) return false;

                bool hasTarget = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect) && !c.IsDisabled());
                if (!hasTarget) return false;

                _sanctifiedUsed = true;
                DecisionTracer.TraceActivate("SanctifiedDarklordEffect", "Negating monster effect & absorbing ATK via Sanctified");
                return true;
            }
            return false;
        }

        private bool DarklordRebellionEffect()
        {
            if (Card.Location != CardLocation.Hand && (Card.Location != CardLocation.SpellZone || Card.IsFacedown())) return false;
            if (_rebellionUsed) return false;

            bool hasDarklordCost = Bot.Hand.Any(c => c != null && c != Card && c.HasSetcode(0xef) && c.IsMonster() && !IsAceCard(c))
                || Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasSetcode(0xef) && c.IsMonster() && !IsAceCard(c));
            if (!hasDarklordCost) return false;

            bool hasTarget = Enemy.GetMonsterCount() > 0
                || Enemy.GetSpells().Any(c => c != null && c.IsFaceup())
                || Duel.LastChainPlayer == 1;
            if (!hasTarget) return false;

            _rebellionUsed = true;
            DecisionTracer.TraceActivate("DarklordRebellionEffect", "Destroying 1 card on field via Darklord Rebellion");
            return true;
        }

        // ==========================================
        //  TIER 3: Pre-Summon Starters & Draw Effects
        // ==========================================

        private bool IxchelDrawEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_ixchelDrawUsed) return false;

            bool otherDarklord = Bot.Hand.Any(c => c != null && c != Card && c.HasSetcode(0xef));
            if (!otherDarklord) return false;

            _ixchelDrawUsed = true;
            DecisionTracer.TraceActivate("IxchelDrawEffect", "Discarding Ixchel + Darklord to draw 2 cards");
            return true;
        }

        private bool BanishmentEffect()
        {
            if (Card.Location != CardLocation.Hand && (Card.Location != CardLocation.SpellZone || Card.IsFacedown())) return false;
            if (_banishmentUsed) return false;

            bool hasTarget = HasRemainingCardWithSetcode(0xef);
            if (!hasTarget) return false;

            _banishmentUsed = true;
            DecisionTracer.TraceActivate("BanishmentEffect", "Searching Darklord card from Deck");
            return true;
        }

        private bool TerminusEffect()
        {
            if (Card.Location != CardLocation.Hand && (Card.Location != CardLocation.SpellZone || Card.IsFacedown())) return false;
            if (_terminusUsed) return false;

            bool hasPowerPatron = HasRemainingMonsterWithSetcode(0x1c6);
            bool hasFairySearch = HasRemainingMonsterWithSetcode(0xef);
            if (!hasPowerPatron || !hasFairySearch) return false;

            _terminusUsed = true;
            DecisionTracer.TraceActivate("TerminusEffect", "Sending Power Patron to search DARK Fairy");
            return true;
        }

        private bool FoolishBurialEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_foolishUsed) return false;

            bool hasUsefulTarget = GetRemainingCount(CardId.DarklordGulgolet) > 0
                || GetRemainingCount(CardId.DarklordIxchel) > 0
                || GetRemainingCount(CardId.DarklordDjehuty) > 0
                || GetRemainingCount(CardId.DarklordMorningstar) > 0;
            if (!hasUsefulTarget) return false;

            _foolishUsed = true;
            DecisionTracer.TraceActivate("FoolishBurialEffect", "Foolish Burial sending Darklord to GY");
            return true;
        }

        private bool DarklordContactEffect()
        {
            if (Card.Location != CardLocation.Hand && (Card.Location != CardLocation.SpellZone || Card.IsFacedown())) return false;
            if (_contactUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            // Morningstar CANNOT be Special Summoned
            bool hasTarget = Bot.Graveyard.Any(c => c != null && c.HasSetcode(0xef) && c.IsMonster() && c.IsCanRevive()
                && !CannotBeSpecialSummoned.Contains(c.Id));
            if (!hasTarget) return false;

            _contactUsed = true;
            DecisionTracer.TraceActivate("DarklordContactEffect", "Reviving Darklord monster from GY");
            return true;
        }

        private bool SuperPolymerizationEffect()
        {
            if (Card.Location != CardLocation.Hand && (Card.Location != CardLocation.SpellZone || Card.IsFacedown())) return false;
            if (ShouldSkipCombo() && Duel.Phase == DuelPhase.Main1) return false;
            if (_superPolyUsed) return false;

            var oppMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            if (oppMonsters.Count == 0) return false;

            bool hasFusionTarget = GetRemainingCount(CardId.MudragonOfTheSwamp) > 0
                || GetRemainingCount(CardId.GaruraWingsOfResonantLife) > 0
                || GetRemainingCount(CardId.DarklordEveningstar) > 0
                || GetRemainingCount(CardId.TheFirstDarklord) > 0;
            if (!hasFusionTarget) return false;

            _superPolyUsed = true;
            DecisionTracer.TraceActivate("SuperPolymerizationEffect", "Super Polymerization board break");
            return true;
        }

        // ==========================================
        //  TIER 4: Normal Summons
        // ==========================================

        private bool ShouldSummonDjehuty()
        {
            if (IsSpecialSummonBlocked()) return false;
            return HasRemainingMonsterWithSetcode(0xef);
        }

        private bool ShouldSummonMorningstar()
        {
            bool hasCondemnedWithGYFairies = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id == CardId.CondemnedDarklord)
                && Bot.Graveyard.Count(c => c != null && c.HasRace(CardRace.Fairy)) >= 2;

            bool hasLahamu = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id == CardId.LahamuTheMessengerOfSacredScripture);

            int availableTributes = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            bool canRegularTribute = availableTributes >= 2;

            if (!hasCondemnedWithGYFairies && !hasLahamu && !canRegularTribute) return false;

            bool oppHasEffect = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect));
            if (oppHasEffect) return true;

            return hasCondemnedWithGYFairies || hasLahamu;
        }

        private bool ShouldSummonIxchel()
        {
            bool hasCondemnedWithGYFairies = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id == CardId.CondemnedDarklord)
                && Bot.Graveyard.Count(c => c != null && c.HasRace(CardRace.Fairy)) >= 2;

            bool hasLahamu = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id == CardId.LahamuTheMessengerOfSacredScripture);

            if (hasCondemnedWithGYFairies || hasLahamu) return true;

            int availableTributes = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            if (availableTributes >= 2 && Bot.GetMonsterCount() < 3) return true;

            return false;
        }

        // ==========================================
        //  TIER 5: Monster Trigger Effects
        // ==========================================

        private bool DjehutySummonEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (ActivateDescription != -1 && ActivateDescription == Util.GetStringId(CardId.DarklordDjehuty, 1)) return false;
            if (_djehutySummonUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            bool hasTarget = HasRemainingMonsterWithSetcode(0xef);
            if (!hasTarget) return false;

            _djehutySummonUsed = true;
            DecisionTracer.TraceActivate("DjehutySummonEffect", "Djehuty Special Summoning Darklord from Deck in DEF");
            return true;
        }

        private bool GulgoletTokensEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (ActivateDescription != -1 && ActivateDescription == Util.GetStringId(CardId.DarklordGulgolet, 1)) return false;
            if (_gulgoletTokensUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            _gulgoletTokensUsed = true;
            DecisionTracer.TraceActivate("GulgoletTokensEffect", "Gulgolet Special Summoning 2 Darklord Tokens");
            return true;
        }

        private bool GulgoletSearchEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_gulgoletSearchUsed) return false;

            bool hasTarget = HasRemainingCardWithSetcode(0xef) || HasRemainingCardWithSetcode(0x24);
            if (!hasTarget) return false;

            _gulgoletSearchUsed = true;
            DecisionTracer.TraceActivate("GulgoletSearchEffect", "Gulgolet GY search trigger");
            return true;
        }

        private bool EveningstarSetEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (ActivateDescription != -1 && ActivateDescription == Util.GetStringId(CardId.DarklordEveningstar, 1)) return false;
            if (_eveningstarSetUsed) return false;

            bool hasSpell = GetRemainingCount(CardId.BanishmentOfTheDarklords) > 0
                || GetRemainingCount(CardId.DarklordDance) > 0
                || GetRemainingCount(CardId.DarklordContact) > 0
                || GetRemainingCount(CardId.DarklordRebellion) > 0;
            bool hasTrap = GetRemainingCount(CardId.TheSanctifiedDarklord) > 0;
            if (!hasSpell && !hasTrap) return false;

            _eveningstarSetUsed = true;
            DecisionTracer.TraceActivate("EveningstarSetEffect", "Eveningstar setting 1 Darklord Spell + 1 Darklord Trap from Deck");
            return true;
        }

        private bool MorningstarSummonEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (ActivateDescription != -1 && ActivateDescription == Util.GetStringId(CardId.DarklordMorningstar, 1)) return false;
            if (_morningstarSummonUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            bool oppHasFaceupEffect = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect));
            if (!oppHasFaceupEffect) return false;

            bool hasDarklord = Bot.Hand.Any(c => c != null && c.HasSetcode(0xef) && c.IsMonster() && !CannotBeSpecialSummoned.Contains(c.Id))
                || HasRemainingMonsterWithSetcode(0xef);
            if (!hasDarklord) return false;

            _morningstarSummonUsed = true;
            DecisionTracer.TraceActivate("MorningstarSummonEffect", "Morningstar swarming Darklords from Hand/Deck");
            return true;
        }

        private bool MorningstarMillEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (ActivateDescription != -1 && ActivateDescription == Util.GetStringId(CardId.DarklordMorningstar, 0)) return false;
            if (_morningstarMillUsed) return false;

            bool hasDarklord = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasSetcode(0xef));
            if (!hasDarklord) return false;

            _morningstarMillUsed = true;
            DecisionTracer.TraceActivate("MorningstarMillEffect", "Morningstar ignition mill & LP gain");
            return true;
        }

        private bool DjehutyGYEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_djehutyGYUsed) return false;

            bool hasFusion = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Dark) && c.HasRace(CardRace.Fairy) && c.HasType(CardType.Fusion));
            if (!hasFusion) return false;

            bool hasTarget = Bot.Graveyard.Any(c => c != null && c != Card && (
                c.HasSetcode(0xef) || (c.HasSetcode(0x24) && c.HasType(CardType.QuickPlay))
            ));
            if (!hasTarget) return false;

            _djehutyGYUsed = true;
            DecisionTracer.TraceActivate("DjehutyGYEffect", "Djehuty GY banish to recycle Darklord / Forbidden Quick-Play");
            return true;
        }

        // ==========================================
        //  TIER 6: Link & Extra Deck Summons
        // ==========================================

        private bool CondemnedDarklordSummon()
        {
            if (IsSpecialSummonBlocked()) return false;

            var fairies = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Fairy) && !IsAceCard(c)).ToList();
            if (fairies.Count < 2) return false;

            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id == CardId.CondemnedDarklord))
                return false;

            return true;
        }

        private bool CondemnedDarklordEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_condemnedSearchUsed) return false;

            bool hasDiscard = Bot.Hand.Any(c => c != null && !IsAceCard(c) && !HandTraps.Contains(c.Id) && c.Id != CardId.DarklordDance && c.Id != CardId.BanishmentOfTheDarklords);
            if (!hasDiscard)
            {
                hasDiscard = Bot.Hand.Any(c => c != null && !IsAceCard(c) && c.Id != CardId.DarklordDance);
            }
            if (!hasDiscard) return false;

            bool hasTarget = HasRemainingMonsterWithSetcode(0xef);
            if (!hasTarget) return false;

            _condemnedSearchUsed = true;
            DecisionTracer.TraceActivate("CondemnedDarklordEffect", "Condemned Darklord searching/sending Darklord monster");
            return true;
        }

        private bool DarklordDanceEffect()
        {
            if (Card.Location != CardLocation.Hand && (Card.Location != CardLocation.SpellZone || Card.IsFacedown())) return false;
            if (_danceUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            bool hasFirstDarklord = GetRemainingCount(CardId.TheFirstDarklord) > 0;
            bool hasEveningstar = GetRemainingCount(CardId.DarklordEveningstar) > 0;
            if (!hasFirstDarklord && !hasEveningstar) return false;

            var availableDarkFairies = Bot.Hand.Where(c => c != null && c != Card && c.IsMonster() && c.HasAttribute(CardAttribute.Dark) && c.HasRace(CardRace.Fairy) && !HandTraps.Contains(c.Id))
                .Concat(Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Dark) && c.HasRace(CardRace.Fairy) && !IsAceCard(c)))
                .Concat(Bot.Graveyard.Where(c => c != null && c.IsMonster() && c.HasAttribute(CardAttribute.Dark) && c.HasRace(CardRace.Fairy)))
                .ToList();

            if (availableDarkFairies.Count < 2) return false;

            bool canSummonEveningstar = hasEveningstar && availableDarkFairies.Count(c => c.Level >= 6) >= 2;
            bool canSummonFirstDarklord = hasFirstDarklord && availableDarkFairies.Count >= 3;

            if (!canSummonEveningstar && !canSummonFirstDarklord) return false;

            _danceUsed = true;
            DecisionTracer.TraceActivate("DarklordDanceEffect", $"Activating Darklord Dance (FirstDarklord={canSummonFirstDarklord}, Eveningstar={canSummonEveningstar})");
            return true;
        }

        private bool VidriumGYEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (Card.IsDisabled()) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.GetMonsters().Count >= 5) return false;
            if (_vidriumGYUsed) return false;

            bool hasGYExtraTarget = Bot.Graveyard.Any(c => c != null && c.IsExtraCard() && c.Id != CardId.TheFirstDarklord);
            if (!hasGYExtraTarget) return false;

            _vidriumGYUsed = true;
            DecisionTracer.TraceActivate("VidriumGYEffect", "Vidrium Special Summoning itself by recycling Extra Deck monster from GY");
            return true;
        }

        private bool TyphonSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var candidates = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c) && c.Attack <= 2000).ToList();
            if (candidates.Count == 0) return false;

            bool oppHasThreat = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.Attack >= 2500 || c.IsExtraCard()));
            return oppHasThreat;
        }

        private bool TyphonBounceEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_typhonBounceUsed) return false;
            if (Card.Overlays.Count == 0) return false;

            bool hasTarget = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup());
            if (!hasTarget) return false;

            _typhonBounceUsed = true;
            DecisionTracer.TraceActivate("TyphonBounceEffect", "TY-PHON bouncing monster on field");
            return true;
        }

        private bool HeraldMirageLightsSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (IsDominusImpulseLocked()) return false;

            bool canDance = (Bot.Hand.Any(c => c != null && c.Id == CardId.DarklordDance) || Bot.GetSpells().Any(c => c != null && c.Id == CardId.DarklordDance)) && !_danceUsed;
            if (canDance) return false;

            var nonTokens = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.HasType(CardType.Token) && !IsAceCard(c) && c.Id != CardId.CondemnedDarklord).ToList();
            if (nonTokens.Count < 2) return false;

            return nonTokens.All(c => c.HasAttribute(CardAttribute.Dark)) && nonTokens.All(c => c.HasRace(CardRace.Fairy));
        }

        private bool MoonSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (IsDominusImpulseLocked()) return false;

            bool oppHasFloodgate = Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && (c.Id == 68462976 || c.HasType(CardType.Continuous) || c.HasType(CardType.Field)));

            bool canDance = (Bot.Hand.Any(c => c != null && c.Id == CardId.DarklordDance) || Bot.GetSpells().Any(c => c != null && c.Id == CardId.DarklordDance)) && !_danceUsed;
            if (canDance && !oppHasFloodgate) return false;

            var fairies = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Fairy) && !IsAceCard(c) && c.Id != CardId.CondemnedDarklord).ToList();
            return fairies.Count >= 2;
        }

        private bool LahamuSummon()
        {
            if (IsSpecialSummonBlocked()) return false;

            bool canDance = (Bot.Hand.Any(c => c != null && c.Id == CardId.DarklordDance) || Bot.GetSpells().Any(c => c != null && c.Id == CardId.DarklordDance)) && !_danceUsed;
            if (canDance) return false;

            var effects = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect) && !IsAceCard(c) && c.Id != CardId.CondemnedDarklord).ToList();
            return effects.Count >= 2;
        }

        private bool LahamuFreeSummonEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (ActivateDescription != -1 && ActivateDescription != Util.GetStringId(CardId.LahamuTheMessengerOfSacredScripture, 0)) return false;
            if (_lahamuFreeSummonUsed) return false;

            bool hasMonster = Bot.Hand.Any(c => c != null && c.IsMonster() && c.Level >= 5);
            if (!hasMonster) return false;

            _lahamuFreeSummonUsed = true;
            DecisionTracer.TraceActivate("LahamuFreeSummonEffect", "Lahamu granting 0-Tribute Normal Summon");
            return true;
        }

        // ==========================================
        //  OnSelectOption — Option overrides
        // ==========================================

        public override int OnSelectOption(IList<long> options)
        {
            if (LastChainCard != null)
            {
                // The First Darklord: Option 0 = Wipe all opp cards (if summoned w/ Morningstar), Option 1 = SS Fairy
                if (LastChainCard.Id == CardId.TheFirstDarklord)
                {
                    if (Enemy.GetMonsterCount() > 0 || Enemy.GetSpells().Count > 0)
                    {
                        if (options.Contains(0)) return 0; // Wipe!
                    }
                    if (options.Contains(1)) return 1; // SS
                    return 0;
                }

                // Darklord Morningstar: Option 0 = SS from Hand/Deck, Option 1 = Mill & recover
                if (LastChainCard.Id == CardId.DarklordMorningstar)
                {
                    bool oppHasEffect = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect));
                    if (oppHasEffect && options.Contains(0)) return 0; // Swarm!
                    if (options.Contains(1)) return 1; // Mill
                    return 0;
                }

                // Condemned Darklord: Option 0 = Add to Hand, Option 1 = Send to GY
                if (LastChainCard.Id == CardId.CondemnedDarklord)
                {
                    if (options.Contains(0)) return 0;
                    return 1;
                }

                // Darklord Djehuty: Option 0 = SS from Deck, Option 1 = GY recycle
                if (LastChainCard.Id == CardId.DarklordDjehuty)
                {
                    if (options.Contains(0)) return 0;
                    if (options.Contains(1)) return 1;
                }

                // Darklord Gulgolet: Option 0 = SS 2 Tokens, Option 1 = GY Search
                if (LastChainCard.Id == CardId.DarklordGulgolet)
                {
                    if (options.Contains(0)) return 0;
                    if (options.Contains(1)) return 1;
                }
            }
            return base.OnSelectOption(options);
        }

        // ==========================================
        //  OnSelectCard — Card Selection overrides
        // ==========================================

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // ── Priority 1: Targeting Opponent's Cards for Negate / Destroy / Bounce / Banish / ToDeck ──
            // Hint 500=FACEUP, 502=DESTROY, 503=TARGET, 505=RTOHAND/Bounce, 507=TODECK, 512=REMOVE/Banish, 575=NEGATE
            if (hint == 500 || hint == 502 || hint == 503 || hint == 505 || hint == 507 || hint == 512 || hint == 575)
            {
                var oppMonsters = cards.Where(c => c.Controller == 1 && c.IsFaceup() && !c.IsDisabled()).OrderByDescending(c => GetThreatScore(c)).ToList();
                if (oppMonsters.Count >= min) return oppMonsters.Take(max).ToList();

                var oppAllMonsters = cards.Where(c => c.Controller == 1 && c.IsFaceup()).OrderByDescending(c => GetThreatScore(c)).ToList();
                if (oppAllMonsters.Count >= min) return oppAllMonsters.Take(max).ToList();

                var oppAllCards = cards.Where(c => c.Controller == 1).OrderByDescending(c => GetThreatScore(c)).ToList();
                if (oppAllCards.Count >= min) return oppAllCards.Take(max).ToList();
            }

            // ── Priority 2: Specific Card Triggers ──
            if (LastChainCard != null)
            {
                // ============ Banishment of the Darklords search (Hint 506 / 503) ============
                if (LastChainCard.Id == CardId.BanishmentOfTheDarklords && (hint == 506 || hint == 503 || cards.All(c => c.Location == CardLocation.Deck)))
                {
                    bool hasDjehutyInHand = Bot.Hand.Any(c => c != null && c.Id == CardId.DarklordDjehuty);
                    var djehuty = cards.FirstOrDefault(c => c.Id == CardId.DarklordDjehuty);
                    if (!hasDjehutyInHand && djehuty != null) return new[] { djehuty };

                    bool hasDance = Bot.Hand.Any(c => c != null && c.Id == CardId.DarklordDance) || Bot.Graveyard.Any(c => c != null && c.Id == CardId.DarklordDance);
                    var dance = cards.FirstOrDefault(c => c.Id == CardId.DarklordDance);
                    if (!hasDance && dance != null) return new[] { dance };

                    var sanctified = cards.FirstOrDefault(c => c.Id == CardId.TheSanctifiedDarklord);
                    if (sanctified != null && !Bot.Hand.Any(c => c != null && c.Id == CardId.TheSanctifiedDarklord) && !Bot.GetSpells().Any(c => c != null && c.Id == CardId.TheSanctifiedDarklord))
                        return new[] { sanctified };

                    var rebellion = cards.FirstOrDefault(c => c.Id == CardId.DarklordRebellion);
                    if (rebellion != null && !Bot.Hand.Any(c => c != null && c.Id == CardId.DarklordRebellion) && !Bot.GetSpells().Any(c => c != null && c.Id == CardId.DarklordRebellion))
                        return new[] { rebellion };

                    bool hasContact = Bot.Hand.Any(c => c != null && c.Id == CardId.DarklordContact) || Bot.Graveyard.Any(c => c != null && c.Id == CardId.DarklordContact);
                    var contact = cards.FirstOrDefault(c => c.Id == CardId.DarklordContact);
                    if (!hasContact && contact != null) return new[] { contact };

                    var ixchel = cards.FirstOrDefault(c => c.Id == CardId.DarklordIxchel);
                    if (ixchel != null) return new[] { ixchel };

                    var morningstar = cards.FirstOrDefault(c => c.Id == CardId.DarklordMorningstar);
                    if (morningstar != null) return new[] { morningstar };

                    var targets = cards.OrderBy(c => GetSearchPriority(c.Id)).ToList();
                    if (targets.Count > 0) return new[] { targets[0] };
                }

                // ============ Terminus: Send Power Patron (Hint 501/504) & Search DARK Fairy (Hint 506/503) ============
                if (LastChainCard.Id == CardId.UnleashedPowerPatronPortalTerminus)
                {
                    if (hint == 501 || hint == 504 || (hint == 506 && cards.All(c => c.HasSetcode(0x1c6))))
                    {
                        var vidrium = cards.FirstOrDefault(c => c.Id == CardId.VidriumThePowerPatronOfChaosExtermination);
                        if (vidrium != null) return new[] { vidrium };
                        var pp = cards.FirstOrDefault(c => c.HasSetcode(0x1c6));
                        if (pp != null) return new[] { pp };
                    }

                    if (hint == 506 || hint == 503 || cards.Any(c => c.HasSetcode(0xef)))
                    {
                        bool hasDjehutyInHand = Bot.Hand.Any(c => c != null && c.Id == CardId.DarklordDjehuty);
                        var djehuty = cards.FirstOrDefault(c => c.Id == CardId.DarklordDjehuty);
                        if (!hasDjehutyInHand && djehuty != null) return new[] { djehuty };

                        var ixchel = cards.FirstOrDefault(c => c.Id == CardId.DarklordIxchel);
                        if (ixchel != null && Bot.Hand.Any(c => c != null && c.HasSetcode(0xef))) return new[] { ixchel };

                        var morningstar = cards.FirstOrDefault(c => c.Id == CardId.DarklordMorningstar);
                        if (morningstar != null && !Bot.Hand.Any(c => c != null && c.Id == CardId.DarklordMorningstar)) return new[] { morningstar };

                        var gulgolet = cards.FirstOrDefault(c => c.Id == CardId.DarklordGulgolet);
                        if (gulgolet != null) return new[] { gulgolet };

                        var darklords = cards.Where(c => c.HasSetcode(0xef)).OrderBy(c => GetSearchPriority(c.Id)).ToList();
                        if (darklords.Count > 0) return new[] { darklords[0] };
                    }
                }

                // ============ Foolish Burial ============
                if (LastChainCard.Id == CardId.FoolishBurial && (hint == 501 || cards.All(c => c.Location == CardLocation.Deck)))
                {
                    var preferred = cards.Where(c => c.Id == CardId.DarklordGulgolet).ToList();
                    if (preferred.Count > 0 && !_gulgoletSearchUsed) return new[] { preferred[0] };

                    preferred = cards.Where(c => c.Id == CardId.DarklordIxchel).ToList();
                    if (preferred.Count > 0) return new[] { preferred[0] };

                    preferred = cards.Where(c => c.Id == CardId.DarklordDjehuty).ToList();
                    if (preferred.Count > 0) return new[] { preferred[0] };

                    preferred = cards.Where(c => c.Id == CardId.DarklordMorningstar).ToList();
                    if (preferred.Count > 0) return new[] { preferred[0] };

                    var darklords = cards.Where(c => c.HasSetcode(0xef) && c.IsMonster()).ToList();
                    if (darklords.Count > 0) return new[] { darklords[0] };
                }

                // ============ Darklord Gulgolet Search (when sent to GY) ============
                if (LastChainCard.Id == CardId.DarklordGulgolet && (hint == 506 || hint == 503 || cards.All(c => c.Location == CardLocation.Deck)))
                {
                    bool hasDance = Bot.Hand.Any(c => c != null && c.Id == CardId.DarklordDance) || Bot.Graveyard.Any(c => c != null && c.Id == CardId.DarklordDance);
                    var dance = cards.FirstOrDefault(c => c.Id == CardId.DarklordDance);
                    if (!hasDance && dance != null) return new[] { dance };

                    bool hasDjehuty = Bot.Hand.Any(c => c != null && c.Id == CardId.DarklordDjehuty);
                    var djehuty = cards.FirstOrDefault(c => c.Id == CardId.DarklordDjehuty);
                    if (!hasDjehuty && djehuty != null) return new[] { djehuty };

                    bool hasContact = Bot.Hand.Any(c => c != null && c.Id == CardId.DarklordContact) || Bot.Graveyard.Any(c => c != null && c.Id == CardId.DarklordContact);
                    var contact = cards.FirstOrDefault(c => c.Id == CardId.DarklordContact);
                    if (!hasContact && contact != null) return new[] { contact };

                    var banishment = cards.FirstOrDefault(c => c.Id == CardId.BanishmentOfTheDarklords);
                    if (banishment != null && !_banishmentUsed) return new[] { banishment };

                    var morningstar = cards.FirstOrDefault(c => c.Id == CardId.DarklordMorningstar);
                    if (morningstar != null && !Bot.Hand.Any(c => c != null && c.Id == CardId.DarklordMorningstar)) return new[] { morningstar };

                    var sanctified = cards.FirstOrDefault(c => c.Id == CardId.TheSanctifiedDarklord);
                    if (sanctified != null) return new[] { sanctified };

                    var rebellion = cards.FirstOrDefault(c => c.Id == CardId.DarklordRebellion);
                    if (rebellion != null) return new[] { rebellion };

                    var droplet = cards.FirstOrDefault(c => c.Id == CardId.ForbiddenDroplet);
                    if (droplet != null) return new[] { droplet };

                    var targets = cards.OrderBy(c => GetSearchPriority(c.Id)).ToList();
                    if (targets.Count > 0) return new[] { targets[0] };
                }

                // ============ Darklord Djehuty Summon Effect (SS from Deck in DEF) ============
                if (LastChainCard.Id == CardId.DarklordDjehuty && (hint == 509 || cards.All(c => c.Location == CardLocation.Deck)))
                {
                    var gulgolet = cards.FirstOrDefault(c => c.Id == CardId.DarklordGulgolet);
                    if (gulgolet != null) return new[] { gulgolet };

                    var ixchel = cards.FirstOrDefault(c => c.Id == CardId.DarklordIxchel);
                    if (ixchel != null) return new[] { ixchel };

                    var darklords = cards.Where(c => c.HasSetcode(0xef) && !CannotBeSpecialSummoned.Contains(c.Id)).ToList();
                    if (darklords.Count > 0) return new[] { darklords[0] };
                }

                // ============ Condemned Darklord Discard Cost ============
                if (LastChainCard.Id == CardId.CondemnedDarklord && (hint == 504 || hint == 501))
                {
                    var gulgolet = cards.FirstOrDefault(c => c.Id == CardId.DarklordGulgolet);
                    if (gulgolet != null && !_gulgoletSearchUsed) return new[] { gulgolet };

                    var vidrium = cards.FirstOrDefault(c => c.Id == CardId.VidriumThePowerPatronOfChaosExtermination);
                    if (vidrium != null) return new[] { vidrium };

                    var djehuty = cards.FirstOrDefault(c => c.Id == CardId.DarklordDjehuty);
                    if (djehuty != null && Bot.Hand.Count(c => c != null && c.Id == CardId.DarklordDjehuty) > 1) return new[] { djehuty };

                    var safeDiscards = cards.Where(c => !IsAceCard(c) && !HandTraps.Contains(c.Id) && c.Id != CardId.DarklordDance && c.Id != CardId.BanishmentOfTheDarklords).ToList();
                    if (safeDiscards.Count > 0) return new[] { safeDiscards[0] };

                    var nonAces = cards.Where(c => !IsAceCard(c) && c.Id != CardId.DarklordDance).ToList();
                    if (nonAces.Count > 0) return new[] { nonAces[0] };
                }

                // ============ Condemned Darklord Search Target ============
                if (LastChainCard.Id == CardId.CondemnedDarklord && (hint == 506 || hint == 503 || cards.All(c => c.Location == CardLocation.Deck)))
                {
                    bool has2FairiesInGY = Bot.Graveyard.Count(c => c != null && c.HasRace(CardRace.Fairy)) >= 2;
                    var morningstar = cards.FirstOrDefault(c => c.Id == CardId.DarklordMorningstar);
                    if (has2FairiesInGY && morningstar != null && !Bot.Hand.Any(c => c != null && c.Id == CardId.DarklordMorningstar))
                        return new[] { morningstar };

                    var ixchel = cards.FirstOrDefault(c => c.Id == CardId.DarklordIxchel);
                    if (ixchel != null && !Bot.Hand.Any(c => c != null && c.Id == CardId.DarklordIxchel))
                        return new[] { ixchel };

                    var djehuty = cards.FirstOrDefault(c => c.Id == CardId.DarklordDjehuty);
                    if (djehuty != null && !Bot.Hand.Any(c => c != null && c.Id == CardId.DarklordDjehuty))
                        return new[] { djehuty };

                    var targets = cards.OrderBy(c => GetSearchPriority(c.Id)).ToList();
                    if (targets.Count > 0) return new[] { targets[0] };
                }

                // ============ Darklord Dance — Select Fusion Target ============
                if (LastChainCard.Id == CardId.DarklordDance && (hint == 509 || cards.All(c => c.Location == CardLocation.Extra)))
                {
                    bool hasMorningstar = Bot.Hand.Any(c => c != null && c.Id == CardId.DarklordMorningstar)
                        || Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id == CardId.DarklordMorningstar)
                        || Bot.Graveyard.Any(c => c != null && c.Id == CardId.DarklordMorningstar);

                    var first = cards.FirstOrDefault(c => c.Id == CardId.TheFirstDarklord);
                    var eveningstar = cards.FirstOrDefault(c => c.Id == CardId.DarklordEveningstar);

                    if (hasMorningstar && first != null) return new[] { first };
                    if (eveningstar != null) return new[] { eveningstar };
                    if (first != null) return new[] { first };
                }

                // ============ Darklord Dance — Banish Materials for Fusion ============
                if (LastChainCard.Id == CardId.DarklordDance && (hint == 512 || hint == 511 || hint == 500))
                {
                    var gyMaterials = cards.Where(c => c.Location == CardLocation.Grave).ToList();
                    var tokenMaterials = cards.Where(c => c.Location == CardLocation.MonsterZone && c.HasType(CardType.Token)).ToList();
                    var fieldMaterials = cards.Where(c => c.Location == CardLocation.MonsterZone && !c.HasType(CardType.Token) && !IsAceCard(c)).ToList();
                    var handMaterials = cards.Where(c => c.Location == CardLocation.Hand && !HandTraps.Contains(c.Id)).ToList();

                    var selection = new List<ClientCard>();
                    selection.AddRange(gyMaterials);
                    selection.AddRange(tokenMaterials);
                    selection.AddRange(fieldMaterials);
                    selection.AddRange(handMaterials);

                    if (selection.Count >= min) return selection.Take(max).ToList();
                    return cards.Take(max).ToList();
                }

                // ============ Darklord Eveningstar — Set 1 Spell + 1 Trap from Deck ============
                if (LastChainCard.Id == CardId.DarklordEveningstar && (hint == 514 || cards.All(c => c.Location == CardLocation.Deck)))
                {
                    var traps = cards.Where(c => c.IsTrap()).OrderBy(c =>
                    {
                        if (c.Id == CardId.TheSanctifiedDarklord) return 0;
                        return 1;
                    }).ToList();

                    var spells = cards.Where(c => c.IsSpell() && !c.IsTrap()).OrderBy(c =>
                    {
                        if (c.Id == CardId.DarklordRebellion) return 0;
                        if (c.Id == CardId.DarklordContact) return 1;
                        if (c.Id == CardId.DarklordDance) return 2;
                        if (c.Id == CardId.BanishmentOfTheDarklords) return 3;
                        return 4;
                    }).ToList();

                    var result = new List<ClientCard>();
                    if (spells.Count > 0) result.Add(spells[0]);
                    if (traps.Count > 0) result.Add(traps[0]);

                    if (result.Count >= min) return result.Take(max).ToList();
                    return cards.Take(max).ToList();
                }

                // ============ Ixchel / Eveningstar Copy — Select Darklord S/T from GY ============
                if ((LastChainCard.Id == CardId.DarklordIxchel || LastChainCard.Id == CardId.DarklordEveningstar)
                    && (hint == 551 || hint == 509 || hint == 503 || cards.All(c => c.Location == CardLocation.Grave)))
                {
                    var targets = cards.Where(c => c.HasSetcode(0xef) && (c.IsSpell() || c.IsTrap())).ToList();
                    if (targets.Count > 0)
                    {
                        if (Duel.Player == 0)
                        {
                            var banishment = targets.FirstOrDefault(c => c.Id == CardId.BanishmentOfTheDarklords);
                            if (banishment != null && !_banishmentUsed) return new[] { banishment };

                            var dance = targets.FirstOrDefault(c => c.Id == CardId.DarklordDance);
                            if (dance != null && !_danceUsed) return new[] { dance };

                            var contact = targets.FirstOrDefault(c => c.Id == CardId.DarklordContact);
                            if (contact != null && !_contactUsed) return new[] { contact };

                            var rebellion = targets.FirstOrDefault(c => c.Id == CardId.DarklordRebellion);
                            if (rebellion != null && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpells().Count > 0)) return new[] { rebellion };

                            var sanctified = targets.FirstOrDefault(c => c.Id == CardId.TheSanctifiedDarklord);
                            if (sanctified != null && Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect) && !c.IsDisabled())) return new[] { sanctified };

                            return new[] { targets[0] };
                        }
                        else
                        {
                            bool oppHasMonsterTarget = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect) && !c.IsDisabled());
                            var sanctified = targets.FirstOrDefault(c => c.Id == CardId.TheSanctifiedDarklord);
                            if (sanctified != null && oppHasMonsterTarget) return new[] { sanctified };

                            var rebellion = targets.FirstOrDefault(c => c.Id == CardId.DarklordRebellion);
                            if (rebellion != null && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpells().Count > 0)) return new[] { rebellion };

                            return new[] { targets[0] };
                        }
                    }
                }

                // ============ The First Darklord — Quick Revive Fairy from Hand/GY ============
                if (LastChainCard.Id == CardId.TheFirstDarklord && hint == 509)
                {
                    var eveningstar = cards.FirstOrDefault(c => c.Id == CardId.DarklordEveningstar);
                    if (eveningstar != null) return new[] { eveningstar };

                    var ixchel = cards.FirstOrDefault(c => c.Id == CardId.DarklordIxchel);
                    if (ixchel != null) return new[] { ixchel };

                    var gulgolet = cards.FirstOrDefault(c => c.Id == CardId.DarklordGulgolet);
                    if (gulgolet != null) return new[] { gulgolet };

                    var fairies = cards.Where(c => c.HasRace(CardRace.Fairy) && !CannotBeSpecialSummoned.Contains(c.Id)).ToList();
                    if (fairies.Count > 0) return new[] { fairies[0] };
                }

                // ============ Darklord Contact Target ============
                if (LastChainCard.Id == CardId.DarklordContact && (hint == 509 || cards.All(c => c.Location == CardLocation.Grave)))
                {
                    var first = cards.FirstOrDefault(c => c.Id == CardId.TheFirstDarklord);
                    if (first != null) return new[] { first };

                    var eveningstar = cards.FirstOrDefault(c => c.Id == CardId.DarklordEveningstar);
                    if (eveningstar != null) return new[] { eveningstar };

                    var ixchel = cards.FirstOrDefault(c => c.Id == CardId.DarklordIxchel);
                    if (ixchel != null) return new[] { ixchel };

                    var djehuty = cards.FirstOrDefault(c => c.Id == CardId.DarklordDjehuty);
                    if (djehuty != null) return new[] { djehuty };

                    var gulgolet = cards.FirstOrDefault(c => c.Id == CardId.DarklordGulgolet);
                    if (gulgolet != null) return new[] { gulgolet };

                    var darklords = cards.Where(c => c.HasSetcode(0xef) && !CannotBeSpecialSummoned.Contains(c.Id)).ToList();
                    if (darklords.Count > 0) return new[] { darklords[0] };
                }

                // ============ Ixchel Draw Discard Cost ============
                if (LastChainCard.Id == CardId.DarklordIxchel && (hint == 501 || hint == 504))
                {
                    var preferred = cards.Where(c => c.Id == CardId.DarklordGulgolet).ToList();
                    if (preferred.Count > 0) return new[] { preferred[0] };

                    preferred = cards.Where(c => c.Id == CardId.DarklordMorningstar).ToList();
                    if (preferred.Count > 0) return new[] { preferred[0] };

                    preferred = cards.Where(c => c.Id == CardId.DarklordDjehuty).ToList();
                    if (preferred.Count > 0) return new[] { preferred[0] };

                    var darklords = cards.Where(c => c.HasSetcode(0xef)).ToList();
                    if (darklords.Count > 0) return new[] { darklords[0] };
                }

                // ============ Sanctified / Rebellion Send Cost ============
                if ((LastChainCard.Id == CardId.TheSanctifiedDarklord || LastChainCard.Id == CardId.DarklordRebellion) && (hint == 501 || hint == 521 || hint == 504))
                {
                    var gulgolet = cards.FirstOrDefault(c => c.Id == CardId.DarklordGulgolet && c.Location == CardLocation.MonsterZone);
                    if (gulgolet != null) return new[] { gulgolet };

                    var djehuty = cards.FirstOrDefault(c => c.Id == CardId.DarklordDjehuty && c.Location == CardLocation.MonsterZone);
                    if (djehuty != null) return new[] { djehuty };

                    var nonAces = cards.Where(c => !IsAceCard(c)).ToList();
                    if (nonAces.Count > 0) return new[] { nonAces[0] };
                }

                // ============ Forbidden Droplet Cost Selection ============
                if (LastChainCard.Id == CardId.ForbiddenDroplet && (hint == 504 || hint == 501))
                {
                    var token = cards.FirstOrDefault(c => c.HasType(CardType.Token));
                    if (token != null) return new[] { token };

                    var djehuty = cards.FirstOrDefault(c => c.Id == CardId.DarklordDjehuty && c.Location == CardLocation.MonsterZone);
                    if (djehuty != null) return new[] { djehuty };

                    var vidrium = cards.FirstOrDefault(c => c.Id == CardId.VidriumThePowerPatronOfChaosExtermination);
                    if (vidrium != null) return new[] { vidrium };

                    var nonAces = cards.Where(c => !IsAceCard(c) && !HandTraps.Contains(c.Id) && c.Id != CardId.DarklordDance && c.Id != CardId.BanishmentOfTheDarklords).ToList();
                    if (nonAces.Count >= min) return nonAces.Take(min).ToList();
                }

                // ============ Herald of Orange Light Discard Cost ============
                if (LastChainCard.Id == CardId.HeraldOfOrangeLight && (hint == 501 || hint == 504))
                {
                    var vidrium = cards.FirstOrDefault(c => c.Id == CardId.VidriumThePowerPatronOfChaosExtermination);
                    if (vidrium != null) return new[] { vidrium };

                    var gulgolet = cards.FirstOrDefault(c => c.Id == CardId.DarklordGulgolet);
                    if (gulgolet != null) return new[] { gulgolet };

                    var extraFairy = cards.FirstOrDefault(c => !IsAceCard(c) && c.Id != CardId.HeraldOfOrangeLight);
                    if (extraFairy != null) return new[] { extraFairy };

                    var anyFairy = cards.FirstOrDefault(c => !IsAceCard(c));
                    if (anyFairy != null) return new[] { anyFairy };
                }

                // ============ Forbidden Crown & Sanctified: NEVER target Bot's own monsters ============
                if ((LastChainCard.Id == CardId.ForbiddenCrown || LastChainCard.Id == CardId.TheSanctifiedDarklord || LastChainCard.Id == CardId.ForbiddenDroplet) && (hint == 500 || hint == 503 || hint == 575))
                {
                    var oppTargets = cards.Where(c => c.Controller == 1 && c.IsFaceup() && !c.IsDisabled()).OrderByDescending(c => GetThreatScore(c)).ToList();
                    if (oppTargets.Count >= min) return oppTargets.Take(max).ToList();

                    var oppAll = cards.Where(c => c.Controller == 1 && c.IsFaceup()).OrderByDescending(c => GetThreatScore(c)).ToList();
                    if (oppAll.Count >= min) return oppAll.Take(max).ToList();

                    if (cancelable) return new List<ClientCard>();
                }

                // ============ Moon & Rebellion Target Selection (Destroy) ============
                if ((LastChainCard.Id == CardId.ProtectorOfTheAgentsMoon || LastChainCard.Id == CardId.DarklordRebellion) && (hint == 502 || hint == 503 || hint == 500))
                {
                    var fieldSpells = cards.Where(c => c.Controller == 1 && c.IsSpell() && (c.Id == 68462976 || c.HasType(CardType.Field) || c.HasType(CardType.Continuous))).ToList();
                    if (fieldSpells.Count >= min) return fieldSpells.Take(max).ToList();

                    var oppMonsters = cards.Where(c => c.Controller == 1 && c.IsFaceup() && !c.IsDisabled()).OrderByDescending(c => GetThreatScore(c)).ToList();
                    if (oppMonsters.Count >= min) return oppMonsters.Take(max).ToList();

                    var oppSpells = cards.Where(c => c.Controller == 1).ToList();
                    if (oppSpells.Count >= min) return oppSpells.Take(max).ToList();
                }

                // ============ Super Polymerization Fusion Target & Material Selection ============
                if (LastChainCard.Id == CardId.SuperPolymerization)
                {
                    if (hint == 509)
                    {
                        var first = cards.FirstOrDefault(c => c.Id == CardId.TheFirstDarklord);
                        if (first != null) return new[] { first };
                        var eveningstar = cards.FirstOrDefault(c => c.Id == CardId.DarklordEveningstar);
                        if (eveningstar != null) return new[] { eveningstar };
                        var mudragon = cards.FirstOrDefault(c => c.Id == CardId.MudragonOfTheSwamp);
                        if (mudragon != null) return new[] { mudragon };
                        var garura = cards.FirstOrDefault(c => c.Id == CardId.GaruraWingsOfResonantLife);
                        if (garura != null) return new[] { garura };
                    }
                    if (hint == 511 || hint == 512 || hint == 500 || hint == 501)
                    {
                        var oppMaterials = cards.Where(c => c.Controller == 1 && c.IsFaceup()).OrderByDescending(c => GetThreatScore(c)).ToList();
                        var botMaterials = cards.Where(c => c.Controller == 0 && !IsAceCard(c)).OrderBy(c => GetMaterialPriority(c)).ToList();

                        var selection = new List<ClientCard>();
                        selection.AddRange(oppMaterials);
                        selection.AddRange(botMaterials);

                        if (selection.Count >= min) return selection.Take(max).ToList();
                        return cards.Take(max).ToList();
                    }
                }

                // ============ Vidrium GY Target Selection ============
                if (LastChainCard.Id == CardId.VidriumThePowerPatronOfChaosExtermination && (hint == 507 || hint == 501 || hint == 503))
                {
                    var extraRecycle = cards.FirstOrDefault(c => c.Location == CardLocation.Grave && c.IsExtraCard() && c.Id != CardId.TheFirstDarklord);
                    if (extraRecycle != null) return new[] { extraRecycle };
                }

                // ============ Djehuty GY Recycle Target ============
                if (LastChainCard.Id == CardId.DarklordDjehuty && (hint == 506 || hint == 503 || cards.All(c => c.Location == CardLocation.Grave)))
                {
                    var dance = cards.FirstOrDefault(c => c.Id == CardId.DarklordDance);
                    if (dance != null && !_danceUsed) return new[] { dance };

                    var contact = cards.FirstOrDefault(c => c.Id == CardId.DarklordContact);
                    if (contact != null && !_contactUsed) return new[] { contact };

                    var banishment = cards.FirstOrDefault(c => c.Id == CardId.BanishmentOfTheDarklords);
                    if (banishment != null && !_banishmentUsed) return new[] { banishment };

                    var droplet = cards.FirstOrDefault(c => c.Id == CardId.ForbiddenDroplet);
                    if (droplet != null) return new[] { droplet };

                    var sanctified = cards.FirstOrDefault(c => c.Id == CardId.TheSanctifiedDarklord);
                    if (sanctified != null) return new[] { sanctified };

                    return new[] { cards[0] };
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        // ==========================================
        //  OnSelectPosition
        // ==========================================

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.DarklordDjehuty || cardId == CardId.DarklordIxchel ||
                cardId == CardId.DarklordMorningstar || cardId == CardId.HeraldOfOrangeLight ||
                cardId == CardId.DarklordGulgolet || cardId == CardId.TheFirstDarklord ||
                cardId == CardId.DarklordEveningstar || cardId == CardId.VidriumThePowerPatronOfChaosExtermination)
            {
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }
            return base.OnSelectPosition(cardId, positions);
        }

        // ==========================================
        //  Monster Reposition & Battle Helpers
        // ==========================================

        private bool MonsterRepos()
        {
            if (Card == null) return DefaultMonsterRepos();
            bool enemyEmpty = Enemy.GetMonsterCount() == 0;

            if (Card.IsAttack())
            {
                if (!enemyEmpty && !IsSafeToAttack(Card) && IsSafeToDefend(Card))
                    return true;
            }
            else
            {
                if (enemyEmpty || (IsSafeToAttack(Card) && !Card.HasType(CardType.Token)))
                    return true;
            }
            return false;
        }

        private bool IsSafeToAttack(ClientCard attacker)
        {
            foreach (ClientCard enemy in Enemy.MonsterZone)
            {
                if (enemy == null || !enemy.IsFaceup()) continue;
                if (!enemy.IsDisabled())
                {
                    if (enemy.Id == 50954680 && attacker.IsSpecialSummoned) return false; // Crystal Wing
                }
                if (enemy.IsAttack())
                {
                    if (enemy.Attack > attacker.Attack) return false;
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

        // ==========================================
        //  Priority Helpers
        // ==========================================

        private int GetSearchPriority(int cardId)
        {
            // Engine starters (top priority)
            if (cardId == CardId.DarklordDjehuty) return 1;
            if (cardId == CardId.DarklordDance) return 2;
            if (cardId == CardId.BanishmentOfTheDarklords) return 3;
            if (cardId == CardId.DarklordContact) return 4;
            if (cardId == CardId.DarklordIxchel) return 5;
            if (cardId == CardId.DarklordMorningstar) return 6;
            if (cardId == CardId.TheSanctifiedDarklord) return 7;
            if (cardId == CardId.DarklordRebellion) return 8;
            if (cardId == CardId.ForbiddenDroplet) return 9;
            if (cardId == CardId.DarklordGulgolet) return 10;
            return 100;
        }

        private int GetThreatScore(ClientCard card)
        {
            if (card == null) return 0;
            int score = 0;
            if (card.IsMonster())
            {
                score += 1000;
                score += card.Attack;
                if (card.IsExtraCard()) score += 2000;
                if (card.IsFaceup() && !card.IsDisabled())
                    score += 5000;
            }
            else if (card.IsSpell() || card.IsTrap())
            {
                score += 500;
                if (card.IsFaceup()) score += 1000;
            }
            return score;
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            if (card.Id == CardId.TheFirstDarklord ||
                card.Id == CardId.DarklordEveningstar ||
                card.Id == CardId.DarklordMorningstar ||
                card.Id == CardId.SuperStarslayerTYPHON)
                return true;
            return base.IsAceCard(card);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.Id == CardId.AshBlossom || c.Id == CardId.MulcharmyFuwalos || c.Id == CardId.HeraldOfOrangeLight)
                return 800;
            if (c.HasType(CardType.Token)) return 10;
            return 100;
        }
    }

    [Deck("Expert_2026_Darklord", "2026_Darklord")]
    public class ExpertDarklordExecutor : _2026_DarklordExecutor
    {
        public ExpertDarklordExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
        }
    }

    [Deck("Neural_2026_Darklord", "2026_Darklord")]
    public class NeuralDarklordExecutor : _2026_DarklordExecutor
    {
        public NeuralDarklordExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
        }
        protected override bool IsBoardStrongEnough()
        {
            int disruption = Bot.GetMonsters().Count(m => m != null && m.IsFaceup() && m.IsMonsterDangerous());
            if (disruption >= 2) return true;
            int atk = Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).Sum(m => m.Attack);
            if (atk >= 5000) return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (IsBoardStrongEnough()) return true;
            return base.ShouldStopExtending();
        }
    }
}
