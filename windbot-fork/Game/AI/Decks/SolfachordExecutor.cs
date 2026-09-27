using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_Solfachord", "2026_Solfachord")]
    public class _2026_SolfachordExecutor : ModernExecutor
    {
        public static class CardId
        {
            // --- Main Deck Monsters ---
            public const int PsyFrameDriver = 49036338;
            public const int MulcharmyFuwalos = 42141493;
            public const int MulcharmyPurulia = 84192580;
            public const int AshBlossomAndJoyousSpring = 14558127;
            public const int PsyFramegearGamma = 38814750;
            public const int MaxxC = 23434538;
            public const int HeraldOfOrangeLight = 17266660;
            public const int DoSolfachordCoolia = 80776622;
            public const int HojoTheVaylantzWarrior = 88919365;
            public const int NazukiTheVaylantzNinja = 41525660;
            public const int VaylantzMadMarquess = 87897777;
            public const int DisablasterTheNegationFortress = 58707981;
            public const int VaylantzVoltageViscount = 41802073;
            public const int SaionTheVaylantzArcher = 15130912;
            public const int VaylantzBusterBaron = 14418464;
            public const int ReSolfachordDreamia = 92610868;
            public const int ShinonomeTheVaylantzPriestess = 49131917;
            public const int SolfachordSolfegia = 84693918; // Custom Card (Scale 9)
            public const int DoSolfachordCutia = 55226153;  // Scale 8
            public const int SolfachordPrimoa = 11688916;   // Custom Card (Scale 0)

            // --- Main Deck Spells ---
            public const int SolfachordHappiness = 93481594; // Custom Card
            public const int VaylantzWakeningSoloActivation = 60095092;
            public const int CalledByTheGrave = 24224830;
            public const int SolfachordHarmonia = 29650040;
            public const int VaylantzWorldKonigWissen = 75952542;
            public const int VaylantzWorldShinraBansho = 49568943;

            // --- Extra Deck Monsters ---
            public const int ArktosXIIChronochasmVaylantz = 50687050;
            public const int VaylantzGenesisGrandDuke = 76075139;
            public const int UnderworldGoddessOfTheClosedWorld = 98127546;
            public const int KnightmareGryphon = 65330383;
            public const int AccesscodeTalker = 86066372;
            public const int GranSolfachordCoolia = 84521924;
            public const int WPFancyBall = 4993187;
            public const int ExceedThePendulum = 92812851;
            public const int HeavymetalfoesElectrumite = 24094258;
            public const int SPLittleKnight = 29301450;
            public const int BeyondThePendulum = 22125101;
            public const int IPMasquerena = 65741786;
            public const int ArtemisTheMagistusMoonMaiden = 34755994;
            public const int Linkuriboh = 41999284;

            // --- Side Deck Cards ---
            public const int NibiruThePrimalBeing = 27204311;
            public const int DrollAndLockBird = 94145021;
            public const int SolSolfachordGracia = 91598270;
            public const int PiriReisMap = 33907039;
            public const int TripleTacticsTalent = 25311006;
        }

        private const int SetCodeSolfachord = 0x164;
        private const int SetCodeVaylantz = 0x17e;

        private bool _hasOpponentActivatedMonsterEffect = false;
        private bool _hasPendulumSummoned = false;

        public _2026_SolfachordExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Register True Ace Cards (protected from being material or tributed)
            // Note: Electrumite & BeyondThePendulum are combo stepping-stones, not terminal aces
            HeuristicGuard.RegisterAceCards(
                CardId.GranSolfachordCoolia,
                CardId.UnderworldGoddessOfTheClosedWorld,
                CardId.AccesscodeTalker,
                CardId.ArktosXIIChronochasmVaylantz,
                CardId.KnightmareGryphon,
                CardId.WPFancyBall,
                CardId.DoSolfachordCoolia,
                CardId.SPLittleKnight
            );
            ResourcePlan.RegisterAceCards(
                CardId.GranSolfachordCoolia,
                CardId.UnderworldGoddessOfTheClosedWorld,
                CardId.AccesscodeTalker,
                CardId.ArktosXIIChronochasmVaylantz,
                CardId.KnightmareGryphon,
                CardId.WPFancyBall,
                CardId.DoSolfachordCoolia,
                CardId.SPLittleKnight
            );

            // ── Bait Planner ──
            BaitPlanner.RegisterComboStarters(CardId.PiriReisMap, CardId.DoSolfachordCutia, CardId.SolfachordPrimoa);
            BaitPlanner.RegisterBaitCards(CardId.PiriReisMap, CardId.VaylantzWakeningSoloActivation, CardId.VaylantzWorldShinraBansho, CardId.VaylantzWorldKonigWissen);

            // ── Chain Advisor ──
            ChainAdvisor.RegisterHighValueTargets(CardId.PiriReisMap, CardId.HeavymetalfoesElectrumite, CardId.BeyondThePendulum, CardId.GranSolfachordCoolia);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 1: COUNTERS & IMMEDIATE HANDTRAPS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.HeraldOfOrangeLight, HeraldOfOrangeLightEffect);
            AddExecutor(ExecutorType.Activate, CardId.PsyFramegearGamma, PsyFramegearGammaEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAndJoyousSpring, () => SmartHandTrapChain() && DefaultAshBlossomAndJoyousSpring());
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, () => SmartHandTrapChain() && DefaultMaxxC());
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyPurulia, MulcharmyPuruliaEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: ON-FIELD QUICK DISRUPTIONS & BOSS NEGATES
            // ═══════════════════════════════════════════════════════════════
            // GranSolfachord Coolia: Quick Effect negation from PZone (Odd scale) & on-summon faceup negation
            AddExecutor(ExecutorType.Activate, CardId.GranSolfachordCoolia, GranSolfachordCooliaEffect);

            // DoSolfachord Coolia: Quick destroy activated monster on field / ignition negate
            AddExecutor(ExecutorType.Activate, CardId.DoSolfachordCoolia, DoSolfachordCooliaMonsterEffect);

            // W:P Fancy Ball: Quick Negate (field/GY) & Quick Link in opp Main Phase
            AddExecutor(ExecutorType.Activate, CardId.WPFancyBall, WPFancyBallNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.WPFancyBall, WPFancyBallQuickLinkEffect);

            // S:P Little Knight: Quick banish / on-summon banish
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightEffect);

            // Arktos XII: Quick position switch / on-move destroy
            AddExecutor(ExecutorType.Activate, CardId.ArktosXIIChronochasmVaylantz, ArktosXIIEffect);

            // Vaylantz Genesis Grand Duke: S&T bounce and burn
            AddExecutor(ExecutorType.Activate, CardId.VaylantzGenesisGrandDuke, GenesisGrandDukeEffect);

            // Underworld Goddess & Knightmare Gryphon
            AddExecutor(ExecutorType.Activate, CardId.UnderworldGoddessOfTheClosedWorld, UnderworldGoddessEffect);
            AddExecutor(ExecutorType.Activate, CardId.KnightmareGryphon, KnightmareGryphonEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: BOARD BREAKERS & TACTICS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: SEARCH SPELLS & PRE-SUMMON SETUP
            // ═══════════════════════════════════════════════════════════════
            // Piri Reis Map: start of MP1, searches Primoa (0 ATK)
            AddExecutor(ExecutorType.Activate, CardId.PiriReisMap, PiriReisMapEffect);

            // Solfachord Happiness: Flexible search / extra PSummon / PZone SS
            AddExecutor(ExecutorType.Activate, CardId.SolfachordHappiness, SolfachordHappinessActivate);

            // Solfachord Harmonia: Field spell search from Extra / pop opp card
            AddExecutor(ExecutorType.Activate, CardId.SolfachordHarmonia, SolfachordHarmoniaEffect);

            // Vaylantz World Field Spells
            AddExecutor(ExecutorType.Activate, CardId.VaylantzWorldShinraBansho, VaylantzWorldShinraBanshoEffect);
            AddExecutor(ExecutorType.Activate, CardId.VaylantzWorldKonigWissen, VaylantzWorldKonigWissenEffect);

            // Vaylantz Wakening: place Vaylantz in PZone / GY move effect
            AddExecutor(ExecutorType.Activate, CardId.VaylantzWakeningSoloActivation, VaylantzWakeningEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 5: NORMAL SUMMONS & MONSTER EFFECTS (Before Vaylantz lock)
            // ═══════════════════════════════════════════════════════════════
            // Normal summon Cutia -> search
            AddExecutor(ExecutorType.Summon, CardId.DoSolfachordCutia);
            AddExecutor(ExecutorType.Activate, CardId.DoSolfachordCutia, DoSolfachordCutiaEffect);

            // Normal summon Primoa -> search
            AddExecutor(ExecutorType.Summon, CardId.SolfachordPrimoa);
            AddExecutor(ExecutorType.Activate, CardId.SolfachordPrimoa, SolfachordPrimoaEffect);

            // Hand Special Summons for Solfachords
            AddExecutor(ExecutorType.SpSummon, CardId.SolfachordSolfegia, SolfachordSolfegiaHandSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SolfachordSolfegia, SolfachordSolfegiaEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.ReSolfachordDreamia, ReSolfachordDreamiaSpSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 6: SCALE ACTIVATIONS & PENDULUM SUMMONING
            // ═══════════════════════════════════════════════════════════════
            // High scales (Solfegia 9, Cutia 8, Dreamia 7)
            AddExecutor(ExecutorType.Activate, CardId.SolfachordSolfegia, ScaleActivate);
            AddExecutor(ExecutorType.Activate, CardId.DoSolfachordCutia, ScaleActivate);
            AddExecutor(ExecutorType.Activate, CardId.ReSolfachordDreamia, ScaleActivate);

            // Low scales (Coolia 1, Primoa 0, Vaylantz 1)
            AddExecutor(ExecutorType.Activate, CardId.DoSolfachordCoolia, ScaleActivate);
            AddExecutor(ExecutorType.Activate, CardId.SolfachordPrimoa, ScaleActivate);
            AddExecutor(ExecutorType.Activate, CardId.ShinonomeTheVaylantzPriestess, ScaleActivate);
            AddExecutor(ExecutorType.Activate, CardId.VaylantzBusterBaron, ScaleActivate);
            AddExecutor(ExecutorType.Activate, CardId.NazukiTheVaylantzNinja, ScaleActivate);
            AddExecutor(ExecutorType.Activate, CardId.VaylantzMadMarquess, ScaleActivate);
            AddExecutor(ExecutorType.Activate, CardId.VaylantzVoltageViscount, ScaleActivate);
            AddExecutor(ExecutorType.Activate, CardId.HojoTheVaylantzWarrior, ScaleActivate);
            AddExecutor(ExecutorType.Activate, CardId.SaionTheVaylantzArcher, ScaleActivate);
            AddExecutor(ExecutorType.Activate, CardId.DisablasterTheNegationFortress, ScaleActivate);

            // Vaylantz PZone Special Summon to same column
            AddExecutor(ExecutorType.Activate, CardId.ShinonomeTheVaylantzPriestess, VaylantzPZoneSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.VaylantzBusterBaron, VaylantzPZoneSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.NazukiTheVaylantzNinja, VaylantzPZoneSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.VaylantzMadMarquess, VaylantzPZoneSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.VaylantzVoltageViscount, VaylantzPZoneSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.HojoTheVaylantzWarrior, VaylantzPZoneSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SaionTheVaylantzArcher, VaylantzPZoneSpSummon);

            // Vaylantz MMZ monster effects (movement, searches)
            AddExecutor(ExecutorType.Activate, CardId.ShinonomeTheVaylantzPriestess, ShinonomeMonsterEffect);
            AddExecutor(ExecutorType.Activate, CardId.VaylantzBusterBaron, VaylantzMovementEffect);
            AddExecutor(ExecutorType.Activate, CardId.NazukiTheVaylantzNinja, VaylantzMovementEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 7: EXTRA DECK LINK & FUSION COMBOS
            // ═══════════════════════════════════════════════════════════════
            // Link-2 Bridge: Heavymetalfoes Electrumite
            AddExecutor(ExecutorType.SpSummon, CardId.HeavymetalfoesElectrumite, ElectrumiteSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.HeavymetalfoesElectrumite, ElectrumiteEffect);

            // Link-2 Bridge: Beyond the Pendulum (STRICT: ONLY BEFORE PENDULUM SUMMON)
            AddExecutor(ExecutorType.SpSummon, CardId.BeyondThePendulum, BeyondThePendulumSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.BeyondThePendulum, BeyondThePendulumEffect);

            // Link-3 Bridge: Exceed the Pendulum
            AddExecutor(ExecutorType.SpSummon, CardId.ExceedThePendulum, ExceedThePendulumSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.ExceedThePendulum, ExceedThePendulumEffect);

            // Vaylantz Fusions: Arktos XII & Grand Duke
            AddExecutor(ExecutorType.SpSummon, CardId.ArktosXIIChronochasmVaylantz, ArktosXIISpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.VaylantzGenesisGrandDuke, GenesisGrandDukeSpSummon);

            // Link-3 Boss: GranSolfachord Coolia
            AddExecutor(ExecutorType.SpSummon, CardId.GranSolfachordCoolia, GranSolfachordCooliaSpSummon);

            // Link-3 Boss: W:P Fancy Ball
            AddExecutor(ExecutorType.SpSummon, CardId.WPFancyBall, WPFancyBallSpSummon);

            // Link-2 Disruption: S:P Little Knight
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSpSummon);

            // Link-2 Disruption: I:P Masquerena
            AddExecutor(ExecutorType.SpSummon, CardId.IPMasquerena, IPMasquerenaSpSummon);

            // Link-4 Bosses: Accesscode Talker (OTK) & Knightmare Gryphon (Lock)
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, AccesscodeSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.AccesscodeTalker, AccesscodeEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmareGryphon, KnightmareGryphonSpSummon);

            // Link-5 Boss: Underworld Goddess of the Closed World
            AddExecutor(ExecutorType.SpSummon, CardId.UnderworldGoddessOfTheClosedWorld, UnderworldGoddessSpSummon);

            // Utility Links: Artemis (unclogs Shinonome), Linkuriboh (Level 1)
            AddExecutor(ExecutorType.SpSummon, CardId.ArtemisTheMagistusMoonMaiden, ArtemisSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Linkuriboh, LinkuribohSpSummon);

            // Fallback Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.ShinonomeTheVaylantzPriestess, FallbackNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.SolfachordSolfegia, FallbackNormalSummon);

            // Spells sets & battle positions
            AddExecutor(ExecutorType.SpellSet, DefaultSpellSet);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override bool OnSelectHand() => true; // Solfachord Pendulum prefer going first

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _hasOpponentActivatedMonsterEffect = false;
            _hasPendulumSummoned = false;
        }

        public override IList<ClientCard> OnSelectPendulumSummon(IList<ClientCard> cards, int max)
        {
            _hasPendulumSummoned = true;
            if (cards == null || cards.Count == 0) return null;
            var sorted = cards.OrderBy(c => {
                if (c == null) return 999;
                if (c.IsCode(CardId.DoSolfachordCutia)) return 1;
                if (c.IsCode(CardId.SolfachordPrimoa)) return 2;
                if (c.IsCode(CardId.DoSolfachordCoolia)) return 3;
                if (c.IsCode(CardId.SolfachordSolfegia)) return 4;
                if (c.IsCode(CardId.ReSolfachordDreamia)) return 5;
                if (IsVaylantz(c.Id)) return 10;
                return 50;
            }).ToList();
            return sorted.Take(max).ToList();
        }

        public override void OnChaining(int player, ClientCard card)
        {
            if (player == 1 && card != null && card.IsMonster() && Duel.Player == 0 && Duel.IsMainPhase())
            {
                _hasOpponentActivatedMonsterEffect = true;
            }
            base.OnChaining(player, card);
        }

        private static long StringId(int cardId, int optionIndex) => ((long)cardId << 4) + optionIndex;

        // ═══════════════════════════════════════════════════════════════
        //  SCALE & PZONE INTELLIGENCE
        // ═══════════════════════════════════════════════════════════════
        public static int GetScale(int cardId)
        {
            switch (cardId)
            {
                case CardId.SolfachordSolfegia: return 9;
                case CardId.DoSolfachordCutia: return 8;
                case CardId.ReSolfachordDreamia: return 7;
                case CardId.DisablasterTheNegationFortress: return 5;
                case CardId.SolSolfachordGracia: return 4;
                case CardId.DoSolfachordCoolia: return 1;
                case CardId.SolfachordPrimoa: return 0;
                // All Main Deck Vaylantz are Scale 1
                case CardId.ShinonomeTheVaylantzPriestess:
                case CardId.HojoTheVaylantzWarrior:
                case CardId.NazukiTheVaylantzNinja:
                case CardId.VaylantzMadMarquess:
                case CardId.VaylantzVoltageViscount:
                case CardId.VaylantzBusterBaron:
                case CardId.SaionTheVaylantzArcher:
                    return 1;
                // Extra Deck Vaylantz Fusions
                case CardId.ArktosXIIChronochasmVaylantz: return 12;
                case CardId.VaylantzGenesisGrandDuke: return 10;
                default:
                    return -1;
            }
        }

        public int GetScale(ClientCard card)
        {
            if (card == null) return -1;
            if (card.LScale > 0) return card.LScale;
            if (card.RScale > 0) return card.RScale;
            return GetScale(card.Id);
        }

        public bool IsPZone(ClientCard card)
        {
            if (card == null || card.Location != CardLocation.SpellZone) return false;
            if (!card.HasType(CardType.Pendulum)) return false;
            if (Duel.IsNewRule)
                return card.Sequence == 0 || card.Sequence == 4;
            return card.Sequence == 6 || card.Sequence == 7;
        }

        public ClientCard GetLeftScaleCard()
        {
            ClientCard c = Util.GetPZone(0, 0);
            return IsPZone(c) ? c : null;
        }

        public ClientCard GetRightScaleCard()
        {
            ClientCard c = Util.GetPZone(0, 1);
            return IsPZone(c) ? c : null;
        }

        public bool HasFullScales() => GetLeftScaleCard() != null && GetRightScaleCard() != null;

        public bool HasOddScaleInPZone()
        {
            var left = GetLeftScaleCard();
            var right = GetRightScaleCard();
            int s1 = left != null ? GetScale(left) : -1;
            int s2 = right != null ? GetScale(right) : -1;
            return (s1 > 0 && s1 % 2 != 0) || (s2 > 0 && s2 % 2 != 0);
        }

        public bool HasEvenScaleInPZone()
        {
            var left = GetLeftScaleCard();
            var right = GetRightScaleCard();
            int s1 = left != null ? GetScale(left) : -1;
            int s2 = right != null ? GetScale(right) : -1;
            return (s1 >= 0 && s1 % 2 == 0) || (s2 >= 0 && s2 % 2 == 0);
        }

        public int GetHighestPZoneScale()
        {
            var left = GetLeftScaleCard();
            var right = GetRightScaleCard();
            int s1 = left != null ? GetScale(left) : 0;
            int s2 = right != null ? GetScale(right) : 0;
            return Math.Max(s1, s2);
        }

        public static bool IsSolfachord(ClientCard card) => card != null && card.HasSetcode(SetCodeSolfachord);
        public static bool IsVaylantz(ClientCard card) => card != null && card.HasSetcode(SetCodeVaylantz);
        public static bool IsVaylantz(int cardId)
        {
            return cardId == CardId.ShinonomeTheVaylantzPriestess ||
                   cardId == CardId.VaylantzBusterBaron ||
                   cardId == CardId.HojoTheVaylantzWarrior ||
                   cardId == CardId.NazukiTheVaylantzNinja ||
                   cardId == CardId.VaylantzMadMarquess ||
                   cardId == CardId.VaylantzVoltageViscount ||
                   cardId == CardId.SaionTheVaylantzArcher ||
                   cardId == CardId.ArktosXIIChronochasmVaylantz ||
                   cardId == CardId.VaylantzGenesisGrandDuke;
        }

        // ═══════════════════════════════════════════════════════════════
        //  HANDTRAP & DISRUPTION HANDLERS
        // ═══════════════════════════════════════════════════════════════
        public bool MulcharmyFuwalosEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (Bot.GetMonsterCount() > 0 || Bot.GetSpellCount() > 0) return false;
            return Duel.Player == 1 && Bot.HasInHand(CardId.MulcharmyFuwalos);
        }

        public bool MulcharmyPuruliaEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (Bot.GetMonsterCount() > 0 || Bot.GetSpellCount() > 0) return false;
            return Duel.Player == 1 && Bot.HasInHand(CardId.MulcharmyPurulia);
        }

        public bool PsyFramegearGammaEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            if (Bot.GetMonsterCount() > 0) return false;
            if (!Bot.HasInHand(CardId.PsyFramegearGamma)) return false;

            var lastChain = Util.GetLastChainCard();
            if (lastChain == null || !lastChain.IsMonster()) return false;

            bool driverAvailable = Bot.HasInHand(CardId.PsyFrameDriver) ||
                                   Bot.Graveyard.Any(c => c.Id == CardId.PsyFrameDriver) ||
                                   GetRemainingCount(CardId.PsyFrameDriver) > 0;
            return driverAvailable;
        }

        public bool HeraldOfOrangeLightEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            var lastChain = Util.GetLastChainCard();
            if (lastChain != null && !lastChain.IsMonster()) return false;

            var activating = (Card != null && Card.Id == CardId.HeraldOfOrangeLight && Bot.Hand.Contains(Card))
                ? Card
                : Bot.Hand.FirstOrDefault(c => c.Id == CardId.HeraldOfOrangeLight);
            if (activating == null) return false;

            return Bot.Hand.Any(c => c != activating && c.IsMonster() && (c.Race == (int)CardRace.Fairy || IsSolfachord(c)));
        }

        // ═══════════════════════════════════════════════════════════════
        //  BOARD DISRUPTIONS
        // ═══════════════════════════════════════════════════════════════
        public bool GranSolfachordCooliaEffect()
        {
            // Effect 1: Quick Negate when opponent activates monster effect (SS odd scale from PZone to zone pointed to)
            if (Duel.LastChainPlayer == 1)
            {
                var lastChain = Util.GetLastChainCard();
                if (lastChain != null && lastChain.IsMonster())
                {
                    bool hasOddScale = HasOddScaleInPZone();
                    // Gran Coolia points to zones below her (bottom-left, bottom, bottom-right)
                    bool hasEmptyZone = Bot.GetMonsterCount() < 5;
                    if (hasOddScale && hasEmptyZone) return true;
                }
            }

            // Effect 2: On Link Summon / Ignition negate opponent faceup card
            if (Card.Location == CardLocation.MonsterZone && Duel.Player == 0 && Duel.IsMainPhase())
            {
                return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled() && !CardIntelligence.IsTargetImmune(c)) ||
                       Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && !c.IsDisabled() && !CardIntelligence.IsTargetImmune(c));
            }

            return false;
        }

        public bool DoSolfachordCooliaMonsterEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Quick effect: Destroy opponent monster activated on field if ATK <= max PZone scale x 300
            if (Duel.LastChainPlayer == 1)
            {
                var lastChain = Util.GetLastChainCard();
                if (lastChain != null && lastChain.Location == CardLocation.MonsterZone && lastChain.Controller == 1)
                {
                    int threshold = GetHighestPZoneScale() * 300;
                    if (lastChain.Attack <= threshold) return true;
                }
            }

            // Ignition: Negate faceup cards up to number of PZone scales
            if (Duel.Player == 0 && Duel.IsMainPhase())
            {
                int pCount = (GetLeftScaleCard() != null ? 1 : 0) + (GetRightScaleCard() != null ? 1 : 0);
                if (pCount > 0)
                {
                    return Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && !m.IsDisabled() && !CardIntelligence.IsTargetImmune(m));
                }
            }
            return false;
        }

        public bool WPFancyBallNegateEffect()
        {
            if (Card == null || Card.Location != CardLocation.MonsterZone) return false;
            if (ActivateDescription != -1 && ActivateDescription != 0 && ActivateDescription != StringId(CardId.WPFancyBall, 0)) return false;
            if (Duel.LastChainPlayer != 1) return false;

            var lastChain = Util.GetLastChainCard();
            if (lastChain == null || !lastChain.IsMonster()) return false;
            return lastChain.Location == CardLocation.MonsterZone || lastChain.Location == CardLocation.Grave;
        }

        public bool WPFancyBallQuickLinkEffect()
        {
            if (Card == null || Card.Location != CardLocation.MonsterZone) return false;
            if (ActivateDescription != -1 && ActivateDescription != 0 && ActivateDescription != StringId(CardId.WPFancyBall, 1)) return false;
            if (Duel.Player != 1 || !Duel.IsMainPhase()) return false;

            // Can make Knightmare Gryphon (Link-4) using Fancy Ball (Link-3) + 1 monster
            // Or S:P Little Knight (Link-2)
            bool canMakeGryphon = Bot.HasInExtra(CardId.KnightmareGryphon) && Bot.GetMonsters().Count(m => m != Card) >= 1;
            bool canStealOpp = Enemy.GetMonsters().Any(m => m != null && m.HasType(CardType.Link) && m.LinkCount <= 2);
            return canMakeGryphon || canStealOpp || Bot.GetMonsterCount() >= 2;
        }

        public bool SPLittleKnightEffect() => true;

        public bool ArktosXIIEffect()
        {
            if (Card == null || Card.Location != CardLocation.MonsterZone) return false;
            // Option 2: Destroy 1 card on field if enemy has targets
            if (Enemy.GetFieldCount() > 0) return true;
            // Option 1: Position switch during enemy turn
            return Duel.Player == 1;
        }

        public bool GenesisGrandDukeEffect()
        {
            return Card.Location == CardLocation.MonsterZone && Enemy.GetSpellCount() > 0;
        }

        public bool UnderworldGoddessEffect()
        {
            return Card.Location == CardLocation.MonsterZone && Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled());
        }

        public bool KnightmareGryphonEffect()
        {
            return Card.Location == CardLocation.MonsterZone && Duel.Player == 0 &&
                   Bot.Graveyard.Any(c => c != null && (c.IsSpell() || c.IsTrap()));
        }

        public bool TripleTacticsTalentEffect()
        {
            return Duel.Player == 0 && Duel.IsMainPhase() && _hasOpponentActivatedMonsterEffect;
        }

        // ═══════════════════════════════════════════════════════════════
        //  SEARCH & FIELD SPELLS
        // ═══════════════════════════════════════════════════════════════
        public bool PiriReisMapEffect()
        {
            if (Duel.Player != 0 || Duel.Phase != DuelPhase.Main1) return false;
            if (Bot.LifePoints <= 2000) return false;
            if (Bot.HasInHand(CardId.SolfachordPrimoa)) return false;
            return GetRemainingCount(CardId.SolfachordPrimoa) > 0;
        }

        public bool SolfachordHappinessActivate()
        {
            if (ShouldSkipCombo()) return false;
            if (Duel.Player != 0 || !Duel.IsMainPhase()) return false;

            // Option 1: Discard 1, Search 2 scales (requires another card in hand)
            bool canSearch = Bot.Hand.Count(c => c != Card) >= 1 && HasRemainingMonsterWithSetcode(SetCodeSolfachord);

            // Option 3: Special Summon 2 from PZones
            bool canSSFromPZone = HasFullScales() && Bot.GetMonsterCount() <= 3;

            // Option 2: Additional Pendulum Summon
            bool canExtraPSummon = HasFullScales() && _hasPendulumSummoned;

            return canSearch || canSSFromPZone || canExtraPSummon;
        }

        public bool SolfachordHarmoniaEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.Hand)
                return Duel.Player == 0 && Duel.IsMainPhase() && !Bot.HasInSpellZone(CardId.SolfachordHarmonia, faceUp: true);

            return Duel.Player == 0 && Duel.IsMainPhase() &&
                   (Bot.ExtraDeck.Any(c => c != null && c.IsFaceup() && IsSolfachord(c)) ||
                    (Enemy.GetFieldCount() > 0 && HasFullScales()));
        }

        public bool VaylantzWorldShinraBanshoEffect()
        {
            if (Card.Location == CardLocation.Hand && Bot.HasInSpellZone(Card.Id, faceUp: true)) return false;
            return Duel.Player == 0 && Duel.IsMainPhase();
        }

        public bool VaylantzWorldKonigWissenEffect()
        {
            if (Card.Location == CardLocation.Hand && Bot.HasInSpellZone(Card.Id, faceUp: true)) return false;
            return Duel.Player == 0 && Duel.IsMainPhase();
        }

        public bool VaylantzWakeningEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Place 1 Vaylantz into PZone from deck
                return Duel.Player == 0 && Duel.IsMainPhase() &&
                       (!HasFullScales() || Bot.MonsterZone[1] == null || Bot.MonsterZone[3] == null);
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Move 1 Vaylantz to adjacent zone
                return Duel.Player == 0 && Duel.IsMainPhase() &&
                       Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && IsVaylantz(m.Id));
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  SOLFACHORD STARTERS & EXTENDERS
        // ═══════════════════════════════════════════════════════════════
        public bool DoSolfachordCutiaEffect()
        {
            return Card.Location == CardLocation.MonsterZone && HasRemainingMonsterWithSetcode(SetCodeSolfachord);
        }

        public bool SolfachordPrimoaEffect()
        {
            return Card.Location == CardLocation.MonsterZone && HasRemainingCardWithSetcode(SetCodeSolfachord);
        }

        public bool SolfachordSolfegiaHandSpSummon()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // SS if we control only Solfachords (or no non-Solfachords)
            return !Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && !IsSolfachord(m));
        }

        public bool SolfachordSolfegiaEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // SS 1 Solfachord from hand or tribute self to SS from GY/Extra
                return Bot.Hand.Any(c => c != null && IsSolfachord(c) && c.Id != CardId.SolfachordSolfegia) ||
                       Bot.Graveyard.Any(c => c != null && IsSolfachord(c) && c.IsMonster()) ||
                       Bot.ExtraDeck.Any(c => c != null && c.IsFaceup() && IsSolfachord(c));
            }
            return false;
        }

        public bool ReSolfachordDreamiaSpSummon()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // SS from hand if we control a Solfachord in PZone
            var left = GetLeftScaleCard();
            var right = GetRightScaleCard();
            return (left != null && IsSolfachord(left)) || (right != null && IsSolfachord(right));
        }

        // ═══════════════════════════════════════════════════════════════
        //  SCALE PLACEMENT & VAYLANTZ PZONE SUMMON
        // ═══════════════════════════════════════════════════════════════
        public bool ScaleActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (HasFullScales()) return false;

            int myScale = GetScale(Card.Id);
            if (myScale < 0) return false;

            var leftScale = GetLeftScaleCard();
            var rightScale = GetRightScaleCard();

            // Case 1: No scales active yet
            if (leftScale == null && rightScale == null)
            {
                // Vaylantz monster: can place if column 1 or column 3 is open for its SS
                if (IsVaylantz(Card.Id))
                {
                    return Bot.MonsterZone[1] == null || Bot.MonsterZone[3] == null;
                }

                // Solfachord: Place if we have matching partner scale or searcher in hand
                bool hasLowInHand = Bot.Hand.Any(c => c != Card && GetScale(c.Id) <= 1 && GetScale(c.Id) >= 0);
                bool hasHighInHand = Bot.Hand.Any(c => c != Card && GetScale(c.Id) >= 7);

                if (myScale <= 1 && (hasHighInHand || Bot.HasInHand(CardId.SolfachordHappiness))) return true;
                if (myScale >= 7 && (hasLowInHand || Bot.HasInHand(CardId.SolfachordHappiness))) return true;
                if (Bot.HasInSpellZone(CardId.SolfachordHarmonia) || Bot.HasInHand(CardId.SolfachordHarmonia)) return true;

                return false;
            }

            // Case 2: One scale already active
            ClientCard existing = leftScale ?? rightScale;
            int existingScale = GetScale(existing);

            // Establish valid low-high scale range
            if (existingScale <= 1 && myScale >= 7) return true;
            if (existingScale >= 7 && myScale <= 1) return true;

            // Vaylantz monster: can place if its target column is free to immediately SS
            if (IsVaylantz(Card.Id))
            {
                if (leftScale == null && Bot.MonsterZone[1] == null) return true;
                if (rightScale == null && Bot.MonsterZone[3] == null) return true;
            }

            return false;
        }

        public bool VaylantzPZoneSpSummon()
        {
            if (!IsPZone(Card)) return false;
            int targetCol = (Card.Sequence == 0 || Card.Sequence == 6) ? 1 : 3;
            return Bot.MonsterZone[targetCol] == null;
        }

        public bool ShinonomeMonsterEffect()
        {
            return Card.Location == CardLocation.MonsterZone && Duel.Player == 0 && Duel.IsMainPhase();
        }

        public bool VaylantzMovementEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Duel.Player != 0 || !Duel.IsMainPhase()) return false;
            // Buster Baron / Nazuki: target Shinonome to move her and trigger her search!
            return Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Id == CardId.ShinonomeTheVaylantzPriestess);
        }

        public bool FallbackNormalSummon()
        {
            return Duel.Player == 0 && Duel.IsMainPhase() && Bot.GetMonsterCount() < 5;
        }

        // ═══════════════════════════════════════════════════════════════
        //  EXTRA DECK SUMMONS & BRIDGES
        // ═══════════════════════════════════════════════════════════════
        public bool ElectrumiteSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.HeavymetalfoesElectrumite)) return false;
            return Bot.GetMonsters().Count(m => m != null && m.IsFaceup() && m.HasType(CardType.Pendulum)) >= 2;
        }

        public bool ElectrumiteEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            // On summon: dump to Extra Deck
            // Ignition: pop a card to add back from Extra Deck
            bool hasCardToPop = Bot.SpellZone.Any(c => c != null && (c.Id == CardId.VaylantzWorldShinraBansho || c.Id == CardId.VaylantzWorldKonigWissen || (IsPZone(c) && HasFullScales() && _hasPendulumSummoned)));
            bool hasExtraToRetrieve = Bot.ExtraDeck.Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Pendulum));
            return hasExtraToRetrieve || hasCardToPop;
        }

        public bool BeyondThePendulumSpSummon()
        {
            // CRITICAL SAFETY GATE: NEVER SUMMON BEYOND THE PENDULUM IF ALREADY PENDULUM SUMMONED
            if (_hasPendulumSummoned) return false;
            if (Bot.HasInMonstersZone(CardId.BeyondThePendulum)) return false;

            int effectMonsters = Bot.GetMonsters().Count(m => m != null && m.IsFaceup() && m.HasType(CardType.Effect));
            bool hasPendulum = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.HasType(CardType.Pendulum));
            if (effectMonsters < 2 || !hasPendulum) return false;

            // Only summon if we need to search a scale to enable Pendulum Summoning
            return !HasFullScales() || Bot.Hand.Count(c => c.HasType(CardType.Pendulum)) == 0;
        }

        public bool BeyondThePendulumEffect()
        {
            // Must have LP and haven't Pendulum Summoned yet
            return Card.Location == CardLocation.MonsterZone && !_hasPendulumSummoned && Bot.LifePoints > 1200;
        }

        public bool ExceedThePendulumSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.ExceedThePendulum)) return false;
            return Bot.GetMonsters().Count(m => m != null && m.IsFaceup() && m.HasType(CardType.Effect)) >= 2 &&
                   Bot.ExtraDeck.Any(m => m != null && m.IsFaceup() && m.HasType(CardType.Pendulum));
        }

        public bool ExceedThePendulumEffect() => Card.Location == CardLocation.MonsterZone;

        public bool ArktosXIISpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.ArktosXIIChronochasmVaylantz)) return false;
            return Bot.GetMonsters().Count(m => m != null && m.IsFaceup() && IsVaylantz(m.Id) && m.Level >= 5) >= 2;
        }

        public bool GenesisGrandDukeSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.VaylantzGenesisGrandDuke)) return false;
            // Must tribute 1 Level 5+ Vaylantz in same column as EMZ (column 1 or column 3)
            return (Bot.MonsterZone[1] != null && IsVaylantz(Bot.MonsterZone[1].Id) && Bot.MonsterZone[1].Level >= 5) ||
                   (Bot.MonsterZone[3] != null && IsVaylantz(Bot.MonsterZone[3].Id) && Bot.MonsterZone[3].Level >= 5);
        }

        public bool GranSolfachordCooliaSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.GranSolfachordCoolia)) return false;
            int totalMonsters = Bot.GetMonsters().Count(m => m != null && m.IsFaceup());
            bool hasPendulum = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.HasType(CardType.Pendulum));
            return totalMonsters >= 3 && hasPendulum;
        }

        public bool WPFancyBallSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.WPFancyBall)) return false;
            return Bot.GetMonsters().Count(m => m != null && m.IsFaceup() && m.HasType(CardType.Effect)) >= 2;
        }

        public bool SPLittleKnightSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.SPLittleKnight)) return false;
            bool enemyHasTarget = Enemy.GetFieldCount() > 0 || Enemy.Graveyard.Count > 0;
            return enemyHasTarget && Bot.GetMonsters().Count(m => m != null && m.IsFaceup() && m.HasType(CardType.Effect)) >= 2;
        }

        public bool IPMasquerenaSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.IPMasquerena)) return false;
            return Util.IsTurn1OrMain2() && Bot.GetMonsters().Count(m => m != null && m.IsFaceup()) >= 2;
        }

        public bool AccesscodeSpSummon()
        {
            if (Util.IsTurn1OrMain2()) return false;
            bool hasHighLink = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.LinkCount >= 3);
            return hasHighLink && Enemy.GetFieldCount() > 0;
        }

        public bool AccesscodeEffect()
        {
            return Card.Location == CardLocation.MonsterZone && Duel.Player == 0 && Enemy.GetFieldCount() > 0;
        }

        public bool KnightmareGryphonSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.KnightmareGryphon)) return false;
            return Bot.GetMonsterCount() >= 3;
        }

        public bool UnderworldGoddessSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.UnderworldGoddessOfTheClosedWorld)) return false;
            return Bot.GetMonsterCount() >= 4 && Enemy.GetMonsterCount() > 0;
        }

        public bool LinkuribohSpSummon()
        {
            return Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Level == 1 && c.Id != CardId.Linkuriboh && !IsAceCard(c));
        }

        public bool ArtemisSpSummon()
        {
            return Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id == CardId.ShinonomeTheVaylantzPriestess);
        }

        // ═══════════════════════════════════════════════════════════════
        //  ENGINE CALLBACKS: OPTIONS & YES/NO
        // ═══════════════════════════════════════════════════════════════
        public override int OnSelectOption(IList<long> options)
        {
            for (int i = 0; i < options.Count; ++i)
            {
                long opt = options[i];

                // Solfachord Happiness
                if (opt == StringId(CardId.SolfachordHappiness, 1)) // Discard 1, search 2
                {
                    if (Bot.Hand.Count(c => c.Id != CardId.SolfachordHappiness) >= 1) return i;
                }
                if (opt == StringId(CardId.SolfachordHappiness, 3)) // SS 2 from PZones
                {
                    if (HasFullScales() && Bot.GetMonsterCount() <= 3) return i;
                }
                if (opt == StringId(CardId.SolfachordHappiness, 2)) // Additional PSummon
                {
                    if (_hasPendulumSummoned) return i;
                }

                // Solfachord Harmonia
                if (opt == StringId(CardId.SolfachordHarmonia, 2)) // Destroy 1 opp card
                {
                    if (Enemy.GetFieldCount() > 0) return i;
                }
                if (opt == StringId(CardId.SolfachordHarmonia, 0)) // Add face-up Solfachord from Extra to hand
                {
                    if (Bot.ExtraDeck.Any(c => c != null && c.IsFaceup() && IsSolfachord(c))) return i;
                }
                if (opt == StringId(CardId.SolfachordHarmonia, 1)) // Scale adjustment
                {
                    return i;
                }

                // Arktos XII
                if (opt == StringId(CardId.ArktosXIIChronochasmVaylantz, 2)) // Destroy 1 card on field
                {
                    if (Enemy.GetFieldCount() > 0) return i;
                }
                if (opt == StringId(CardId.ArktosXIIChronochasmVaylantz, 1)) // Switch 2 monsters
                {
                    return i;
                }

                // Vaylantz Genesis Grand Duke
                if (opt == StringId(CardId.VaylantzGenesisGrandDuke, 1)) // Bounce S&T to hand
                {
                    return i;
                }

                // Shinonome
                if (opt == StringId(CardId.ShinonomeTheVaylantzPriestess, 1)) return i; // Search Spell
                if (opt == StringId(CardId.ShinonomeTheVaylantzPriestess, 2)) return i; // Search Monster

                // Triple Tactics Talent
                if (opt == StringId(CardId.TripleTacticsTalent, 0)) // Draw 2
                {
                    if (Bot.GetHandCount() <= 2) return i;
                }
                if (opt == StringId(CardId.TripleTacticsTalent, 1)) // Take Control
                {
                    if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= 2500 && !CardIntelligence.IsTargetImmune(c))) return i;
                }
                if (opt == StringId(CardId.TripleTacticsTalent, 2)) // Hand Rip
                {
                    return i;
                }
            }
            return base.OnSelectOption(options);
        }

        public override bool OnSelectYesNo(long desc)
        {
            // Solfegia PZone Negate
            if (desc == StringId(CardId.SolfachordSolfegia, 2)) return true;

            // Solfegia MZone Tribute
            if (desc == StringId(CardId.SolfachordSolfegia, 3))
            {
                bool hasGyOrExtra = Bot.Graveyard.Any(c => IsSolfachord(c) && c.IsMonster()) ||
                                    Bot.ExtraDeck.Any(c => IsSolfachord(c) && c.IsFaceup());
                return hasGyOrExtra || Bot.GetMonsterCount() >= 5;
            }

            // Solfachord Happiness: Special Summon from hand
            if (desc == StringId(CardId.SolfachordHappiness, 4)) return true;

            // Gran Coolia: Negate monster effect activation
            if (desc == StringId(CardId.GranSolfachordCoolia, 0)) return true;

            // Gran Coolia: Dump even scale to Extra Deck
            if (desc == StringId(CardId.GranSolfachordCoolia, 1)) return true;

            return base.OnSelectYesNo(desc);
        }

        // ═══════════════════════════════════════════════════════════════
        //  OCGCORE HINT-BASED CARD SELECTION ENGINE
        // ═══════════════════════════════════════════════════════════════
        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return null;

            // ── Hint 500: Tribute / Release ──
            if (hint == 500)
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (IsAceCard(c)) return 900;
                    if (IsPZone(c)) return 800;
                    if (c.Id == CardId.SolfachordSolfegia) return 10;
                    if (IsVaylantz(c.Id) && c.Level >= 5) return 20; // Grand Duke / Arktos tribute
                    return 50;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // ── Hint 501: Discard (Cost / Effect) ──
            if (hint == 501)
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (c.Id == CardId.PsyFrameDriver) return 1;
                    if (Bot.Hand.Count(h => h.Id == c.Id) > 1) return 2; // Duplicates
                    if (IsVaylantz(c.Id) && c.Location == CardLocation.Hand) return 5;
                    if (c.Id == CardId.HeraldOfOrangeLight || c.Id == CardId.AshBlossomAndJoyousSpring || c.Id == CardId.MaxxC) return 500;
                    if (c.Id == CardId.DoSolfachordCutia || c.Id == CardId.SolfachordPrimoa || c.Id == CardId.SolfachordHappiness) return 900;
                    return 100;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // ── Hint 502 / 503 / 575: Removal & Negation Targets ──
            if (hint == 502 || hint == 503 || hint == 575)
            {
                // If Electrumite popping bot's own card
                if (Card != null && Card.IsCode(CardId.HeavymetalfoesElectrumite) && cards.All(c => c.Controller == 0))
                {
                    var sortedOwn = cards.OrderBy(c => {
                        if (c.IsCode(CardId.VaylantzWorldShinraBansho)) return 1;
                        if (c.IsCode(CardId.VaylantzWorldKonigWissen)) return 2;
                        if (IsVaylantz(c.Id) && c.Location == CardLocation.SpellZone) return 3;
                        if (IsPZone(c) && !IsAceCard(c) && _hasPendulumSummoned) return 5;
                        if (IsAceCard(c)) return 999;
                        return 100;
                    }).ToList();
                    return sortedOwn.Take(max).ToList();
                }

                // Opponent targets
                var oppCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (oppCards.Count > 0)
                {
                    var sortedOpp = oppCards.OrderByDescending(c => {
                        if (CardIntelligence.IsTargetImmune(c)) return -1;
                        if (c.IsSpell() && c.IsFaceup() && (c.HasType(CardType.Continuous) || c.HasType(CardType.Field))) return 5000;
                        if (c.IsMonster() && c.IsFaceup() && !c.IsDisabled()) return 4000 + c.Attack;
                        if (c.IsFacedown()) return 2000;
                        return 1000;
                    }).ToList();
                    return sortedOpp.Take(max).ToList();
                }
            }

            // ── Hint 505: Return to Hand (Bounce) ──
            if (hint == 505)
            {
                // If Primoa bouncing own scale to hand
                var ownPZone = cards.Where(c => c != null && c.Controller == 0 && IsPZone(c)).ToList();
                if (ownPZone.Count > 0)
                {
                    var sortedPZone = ownPZone.OrderBy(c => {
                        if (c.IsCode(CardId.DoSolfachordCutia)) return 1; // Reuse Cutia search next turn
                        if (c.IsCode(CardId.SolfachordSolfegia)) return 2;
                        if (c.IsCode(CardId.DoSolfachordCoolia)) return 3;
                        return 10;
                    }).ToList();
                    return sortedPZone.Take(max).ToList();
                }

                // Opponent cards (Grand Duke bounce)
                var oppCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (oppCards.Count > 0)
                {
                    var sortedOpp = oppCards.OrderByDescending(c => c.Location == CardLocation.SpellZone ? 2000 : 1000).ToList();
                    return sortedOpp.Take(max).ToList();
                }
            }

            // ── Hint 506: Add to Hand (Search / Recovery) ──
            if (hint == 506 || cards.All(c => c.Location == CardLocation.Deck))
            {
                bool hasLow = (GetLeftScaleCard() != null && GetScale(GetLeftScaleCard()) <= 1) ||
                              (GetRightScaleCard() != null && GetScale(GetRightScaleCard()) <= 1) ||
                              Bot.Hand.Any(c => GetScale(c.Id) <= 1 && GetScale(c.Id) >= 0);

                bool hasHigh = (GetLeftScaleCard() != null && GetScale(GetLeftScaleCard()) >= 7) ||
                               (GetRightScaleCard() != null && GetScale(GetRightScaleCard()) >= 7) ||
                               Bot.Hand.Any(c => GetScale(c.Id) >= 7);

                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;

                    // If Electrumite dumping to Extra Deck
                    if (Card != null && Card.IsCode(CardId.HeavymetalfoesElectrumite) && c.Location == CardLocation.Deck)
                    {
                        if (c.IsCode(CardId.SolfachordPrimoa)) return 1;
                        if (c.IsCode(CardId.DoSolfachordCutia)) return 2;
                        if (c.IsCode(CardId.SolfachordSolfegia)) return 3;
                        if (c.IsCode(CardId.DoSolfachordCoolia)) return 4;
                    }

                    // Scale balancing
                    if (!hasLow && GetScale(c.Id) <= 1 && GetScale(c.Id) >= 0)
                    {
                        if (c.IsCode(CardId.DoSolfachordCoolia)) return 10;
                        if (c.IsCode(CardId.SolfachordPrimoa)) return 11;
                    }
                    if (!hasHigh && GetScale(c.Id) >= 7)
                    {
                        if (c.IsCode(CardId.SolfachordSolfegia)) return 10;
                        if (c.IsCode(CardId.DoSolfachordCutia)) return 11;
                        if (c.IsCode(CardId.ReSolfachordDreamia)) return 12;
                    }

                    // General Solfachord search
                    if (c.IsCode(CardId.SolfachordHappiness)) return 20;
                    if (c.IsCode(CardId.DoSolfachordCutia)) return 21;
                    if (c.IsCode(CardId.SolfachordPrimoa)) return 22;
                    if (c.IsCode(CardId.SolfachordSolfegia)) return 23;
                    if (c.IsCode(CardId.DoSolfachordCoolia)) return 24;
                    if (c.IsCode(CardId.SolfachordHarmonia)) return 25;

                    // Vaylantz search
                    if (c.IsCode(CardId.VaylantzWakeningSoloActivation)) return 30;
                    if (c.IsCode(CardId.ShinonomeTheVaylantzPriestess)) return 31;
                    if (c.IsCode(CardId.VaylantzBusterBaron)) return 32;

                    return 100;
                }).ToList();

                return sorted.Take(max).ToList();
            }

            // ── Hint 509: Special Summon Selection ──
            if (hint == 509)
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (c.IsCode(CardId.DoSolfachordCutia)) return 1;
                    if (c.IsCode(CardId.SolfachordPrimoa)) return 2;
                    if (c.IsCode(CardId.DoSolfachordCoolia)) return 3;
                    if (c.IsCode(CardId.SolfachordSolfegia)) return 4;
                    if (c.IsCode(CardId.ReSolfachordDreamia)) return 5;
                    return 50;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // ── Hint 511 / 512 / 513 / 533: Material Selection ──
            if (hint == 511 || hint == 512 || hint == 513 || hint == 533)
            {
                var sorted = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 950;
            if (IsPZone(c)) return 900;
            if (c.IsCode(CardId.HeraldOfOrangeLight) || c.IsCode(CardId.AshBlossomAndJoyousSpring) || c.IsCode(CardId.MaxxC)) return 800;

            // Low level combo pieces are best materials
            if (c.IsCode(CardId.VaylantzBusterBaron) || c.IsCode(CardId.SaionTheVaylantzArcher)) return 10;
            if (c.IsCode(CardId.ShinonomeTheVaylantzPriestess)) return 15;
            if (c.IsCode(CardId.ReSolfachordDreamia)) return 20;
            if (c.IsCode(CardId.SolfachordSolfegia)) return 25;
            if (c.IsCode(CardId.SolfachordPrimoa)) return 30;
            if (c.IsCode(CardId.DoSolfachordCutia)) return 35;

            // Stepping-stone Link monsters
            if (c.IsCode(CardId.HeavymetalfoesElectrumite) || c.IsCode(CardId.BeyondThePendulum)) return 100;

            return 50;
        }

        // ═══════════════════════════════════════════════════════════════
        //  ZONE PLACEMENT & COMBAT POSITION
        // ═══════════════════════════════════════════════════════════════
        public override int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
            if (location == CardLocation.MonsterZone)
            {
                // 1. Extra Monster Zone for Extra Deck Link / Fusion summons
                if (cardId == CardId.HeavymetalfoesElectrumite ||
                    cardId == CardId.BeyondThePendulum ||
                    cardId == CardId.GranSolfachordCoolia ||
                    cardId == CardId.WPFancyBall ||
                    cardId == CardId.KnightmareGryphon ||
                    cardId == CardId.AccesscodeTalker ||
                    cardId == CardId.UnderworldGoddessOfTheClosedWorld ||
                    cardId == CardId.SPLittleKnight ||
                    cardId == CardId.IPMasquerena ||
                    cardId == CardId.ArtemisTheMagistusMoonMaiden ||
                    cardId == CardId.Linkuriboh)
                {
                    int emz1 = 1 << 5;
                    int emz2 = 1 << 6;
                    if ((available & emz1) > 0 && Bot.MonsterZone[5] == null) return emz1;
                    if ((available & emz2) > 0 && Bot.MonsterZone[6] == null) return emz2;
                }

                // 2. Vaylantz PZone Special Summon: must go to same column
                if (IsVaylantz((int)cardId))
                {
                    var pLeft = GetLeftScaleCard();
                    if (pLeft != null && pLeft.Id == cardId && (available & (1 << 1)) > 0)
                        return 1 << 1;
                    var pRight = GetRightScaleCard();
                    if (pRight != null && pRight.Id == cardId && (available & (1 << 3)) > 0)
                        return 1 << 3;
                }

                // 3. Vaylantz Movement (Shinonome): move to adjacent column
                if (cardId == CardId.ShinonomeTheVaylantzPriestess)
                {
                    if (Bot.MonsterZone[1] != null && Bot.MonsterZone[1].Id == CardId.ShinonomeTheVaylantzPriestess)
                    {
                        if ((available & (1 << 0)) > 0) return 1 << 0;
                        if ((available & (1 << 2)) > 0) return 1 << 2;
                    }
                    if (Bot.MonsterZone[3] != null && Bot.MonsterZone[3].Id == CardId.ShinonomeTheVaylantzPriestess)
                    {
                        if ((available & (1 << 2)) > 0) return 1 << 2;
                        if ((available & (1 << 4)) > 0) return 1 << 4;
                    }
                }

                // 4. Default: Prefer center column or non-EMZ columns to leave EMZ lines open
                if ((available & (1 << 2)) > 0) return 1 << 2;
                if ((available & (1 << 0)) > 0) return 1 << 0;
                if ((available & (1 << 4)) > 0) return 1 << 4;
            }
            return base.OnSelectPlace(cardId, player, location, available);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (positions.Contains(CardPosition.FaceUpDefence))
            {
                // Fragile low-ATK cards should always be placed in Defense
                if (cardId == CardId.SolfachordPrimoa ||
                    cardId == CardId.DoSolfachordCutia ||
                    cardId == CardId.ShinonomeTheVaylantzPriestess ||
                    cardId == CardId.AshBlossomAndJoyousSpring ||
                    cardId == CardId.MaxxC ||
                    cardId == CardId.HeraldOfOrangeLight ||
                    cardId == CardId.PsyFramegearGamma ||
                    cardId == CardId.MulcharmyFuwalos ||
                    cardId == CardId.MulcharmyPurulia ||
                    cardId == CardId.VaylantzBusterBaron ||
                    cardId == CardId.SaionTheVaylantzArcher)
                {
                    return CardPosition.FaceUpDefence;
                }
            }
            return base.OnSelectPosition(cardId, positions);
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.IsCode(CardId.GranSolfachordCoolia) ||
                   card.IsCode(CardId.AccesscodeTalker) ||
                   card.IsCode(CardId.ArktosXIIChronochasmVaylantz) ||
                   card.IsCode(CardId.UnderworldGoddessOfTheClosedWorld) ||
                   card.IsCode(CardId.KnightmareGryphon) ||
                   card.IsCode(CardId.WPFancyBall) ||
                   card.IsCode(CardId.DoSolfachordCoolia) ||
                   card.IsCode(CardId.SPLittleKnight);
        }

        protected override bool IsBoardStrongEnough()
        {
            if (Bot.HasInMonstersZone(CardId.GranSolfachordCoolia)) return true;
            if (Bot.HasInMonstersZone(CardId.AccesscodeTalker)) return true;
            if (Bot.HasInMonstersZone(CardId.UnderworldGoddessOfTheClosedWorld)) return true;
            if (Bot.GetMonsterCount() >= 3 && HasFullScales()) return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (Bot.HasInMonstersZone(CardId.GranSolfachordCoolia) ||
                Bot.HasInMonstersZone(CardId.AccesscodeTalker) ||
                Bot.HasInMonstersZone(CardId.UnderworldGoddessOfTheClosedWorld))
            {
                return base.ShouldStopExtending();
            }
            return false;
        }
    }

    [Deck("Expert_2026_Solfachord", "2026_Solfachord")]
    public class ExpertSolfachordExecutor : _2026_SolfachordExecutor
    {
        public ExpertSolfachordExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
        }
    }
}
