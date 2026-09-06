using System;
using YGOSharp.OCGWrapper.Enums;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_Solfachord", "2026_Solfachord")]
    public class _2026_SolfachordExecutor : ModernExecutor
    {
        public class CardId
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
            public const int SolfachordSolfegia = 84693918; // Custom Card
            public const int DoSolfachordCutia = 55226153;
            public const int SolfachordPrimoa = 11688916;  // Custom Card

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
            public const int WPFancyBall = 4993187;          // Custom Card
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

        public _2026_SolfachordExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Register Ace Cards to protect from being material or tributed
            HeuristicGuard.RegisterAceCards(
                CardId.GranSolfachordCoolia,
                CardId.UnderworldGoddessOfTheClosedWorld,
                CardId.AccesscodeTalker,
                CardId.SPLittleKnight,
                CardId.ArktosXIIChronochasmVaylantz,
                CardId.KnightmareGryphon,
                CardId.WPFancyBall,
                CardId.DoSolfachordCoolia
            );
            ResourcePlan.RegisterAceCards(
                CardId.GranSolfachordCoolia,
                CardId.UnderworldGoddessOfTheClosedWorld,
                CardId.AccesscodeTalker,
                CardId.SPLittleKnight,
                CardId.ArktosXIIChronochasmVaylantz,
                CardId.KnightmareGryphon,
                CardId.WPFancyBall,
                CardId.DoSolfachordCoolia
            );

            // ── Combo Router: Sequencing ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Standard-Setup",
                RequiredCards = new List<int> { CardId.PiriReisMap, CardId.PsyFrameDriver },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.PiriReisMap, ActionType = ExecutorType.Activate, Description = "Play CardId.PiriReisMap" },
                    new() { CardId = CardId.PsyFrameDriver, ActionType = ExecutorType.Activate, Description = "Extend with CardId.PsyFrameDriver" }
                },
                EndBoardScore = 80
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Cutia-Scale-Path",
                RequiredCards = new List<int> { CardId.DoSolfachordCutia },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.DoSolfachordCutia, ActionType = ExecutorType.Activate, Description = "Set Cutia as high scale" },
                    new() { CardId = CardId.DoSolfachordCoolia, ActionType = ExecutorType.Activate, Description = "Set Coolia as low scale" },
                    new() { CardId = CardId.GranSolfachordCoolia, ActionType = ExecutorType.SpSummon, Description = "Pendulum Summon into Gran Coolia" }
                },
                EndBoardScore = 85
            });

            // ── Bait Planner ──
            BaitPlanner.RegisterComboStarters(CardId.PiriReisMap);
            BaitPlanner.RegisterBaitCards(CardId.PiriReisMap);

            // ── Chain Advisor ──
            ChainAdvisor.RegisterHighValueTargets(CardId.PiriReisMap, CardId.SPLittleKnight);

            // Register Mulcharmy Fuwalos & Purulia hand traps
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyPurulia, MulcharmyPuruliaEffect);

            // Register PSY-Framegear Gamma + Driver
            AddExecutor(ExecutorType.Activate, CardId.PsyFramegearGamma, PsyFramegearGammaEffect);

            // Register Herald of Orange Light
            AddExecutor(ExecutorType.Activate, CardId.HeraldOfOrangeLight, HeraldOfOrangeLightEffect);

            // Register Ash Blossom & Maxx "C"
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAndJoyousSpring, () => SmartHandTrapChain() && DefaultAshBlossomAndJoyousSpring());
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, () => SmartHandTrapChain() && DefaultMaxxC());

            // Register Called by the Grave
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);

            // Register Piri Reis Map & Triple Tactics Talent
            AddExecutor(ExecutorType.Activate, CardId.PiriReisMap, PiriReisMapEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentEffect);

            // Register W:P Fancy Ball
            AddExecutor(ExecutorType.Activate, CardId.WPFancyBall, WPFancyBallNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.WPFancyBall, WPFancyBallQuickLinkEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.WPFancyBall, WPFancyBallSpSummon);

            // Register Solfachord Solfegia
            AddExecutor(ExecutorType.Activate, CardId.SolfachordSolfegia, SolfachordSolfegiaScaleNegateEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.SolfachordSolfegia, SolfachordSolfegiaHandSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SolfachordSolfegia, SolfachordSolfegiaMZoneSummon);
            AddExecutor(ExecutorType.Activate, CardId.SolfachordSolfegia, ScaleActivate);

            // Register Solfachord Primoa
            AddExecutor(ExecutorType.Activate, CardId.SolfachordPrimoa, SolfachordPrimoaPZoneBounce);
            AddExecutor(ExecutorType.Activate, CardId.SolfachordPrimoa, SolfachordPrimoaSummonSearch);
            AddExecutor(ExecutorType.Activate, CardId.SolfachordPrimoa, SolfachordPrimoaLinkRecycle);
            AddExecutor(ExecutorType.Activate, CardId.SolfachordPrimoa, ScaleActivate);

            // Register Solfachord Happiness
            AddExecutor(ExecutorType.Activate, CardId.SolfachordHappiness, SolfachordHappinessActivate);

            // Register general summon and scale setting executors
            AddExecutor(ExecutorType.Activate, CardId.DoSolfachordCoolia, ScaleActivate);
            AddExecutor(ExecutorType.Activate, CardId.DoSolfachordCoolia, DoSolfachordCooliaMonsterEffect);
            AddExecutor(ExecutorType.Activate, CardId.DoSolfachordCutia, ScaleActivate);
            AddExecutor(ExecutorType.Activate, CardId.DoSolfachordCutia, DoSolfachordCutiaEffect);
            AddExecutor(ExecutorType.Summon, CardId.DoSolfachordCutia);
            AddExecutor(ExecutorType.SpSummon, CardId.DoSolfachordCutia);

            AddExecutor(ExecutorType.Summon, CardId.SolfachordPrimoa);
            AddExecutor(ExecutorType.SpSummon, CardId.SolfachordPrimoa, SolfachordPrimoaSpSummon);

            AddExecutor(ExecutorType.Summon, CardId.SolfachordSolfegia);

            AddExecutor(ExecutorType.SpSummon, CardId.ReSolfachordDreamia);
            AddExecutor(ExecutorType.Activate, CardId.ReSolfachordDreamia, ScaleActivate);

            AddExecutor(ExecutorType.Activate, CardId.SolSolfachordGracia, ScaleActivate);
            AddExecutor(ExecutorType.Activate, CardId.SolSolfachordGracia, SolSolfachordGraciaMonsterEffect);
            AddExecutor(ExecutorType.Summon, CardId.SolSolfachordGracia);
            AddExecutor(ExecutorType.SpSummon, CardId.SolSolfachordGracia);
            AddExecutor(ExecutorType.Activate, CardId.DisablasterTheNegationFortress, ScaleActivate);

            AddExecutor(ExecutorType.Activate, CardId.SolfachordHarmonia, SolfachordHarmoniaEffect);

            // Extra Deck monster summons & effects
            AddExecutor(ExecutorType.SpSummon, CardId.HeavymetalfoesElectrumite, ElectrumiteSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.HeavymetalfoesElectrumite, ElectrumiteEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.BeyondThePendulum, BeyondThePendulumSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.BeyondThePendulum, BeyondThePendulumEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.ExceedThePendulum, ExceedThePendulumSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.ExceedThePendulum, ExceedThePendulumEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.GranSolfachordCoolia, GranSolfachordCooliaSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.GranSolfachordCoolia, GranSolfachordCooliaEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.IPMasquerena, IPMasquerenaSpSummon);

            AddExecutor(ExecutorType.SpSummon, CardId.ArktosXIIChronochasmVaylantz, ArktosXIISpSummon);
            AddExecutor(ExecutorType.Activate, CardId.ArktosXIIChronochasmVaylantz, VaylantzBossRemovalEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.VaylantzGenesisGrandDuke, GenesisGrandDukeSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.VaylantzGenesisGrandDuke, VaylantzBossRemovalEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, AccesscodeSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.AccesscodeTalker, AccesscodeEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.Linkuriboh, LinkuribohSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ArtemisTheMagistusMoonMaiden, ArtemisSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmareGryphon, KnightmareGryphonSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.KnightmareGryphon, KnightmareGryphonEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.UnderworldGoddessOfTheClosedWorld, UnderworldGoddessSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.UnderworldGoddessOfTheClosedWorld, UnderworldGoddessEffect);

            // Vaylantz starter combo activations/summons
            AddExecutor(ExecutorType.Activate, CardId.ShinonomeTheVaylantzPriestess, VaylantzActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.ShinonomeTheVaylantzPriestess);
            AddExecutor(ExecutorType.Summon, CardId.ShinonomeTheVaylantzPriestess);

            AddExecutor(ExecutorType.Activate, CardId.HojoTheVaylantzWarrior, VaylantzActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.HojoTheVaylantzWarrior);

            AddExecutor(ExecutorType.Activate, CardId.NazukiTheVaylantzNinja, VaylantzActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.NazukiTheVaylantzNinja);

            AddExecutor(ExecutorType.Activate, CardId.VaylantzMadMarquess, VaylantzActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.VaylantzMadMarquess);

            AddExecutor(ExecutorType.Activate, CardId.VaylantzVoltageViscount, VaylantzActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.VaylantzVoltageViscount);

            AddExecutor(ExecutorType.Activate, CardId.VaylantzBusterBaron, VaylantzActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.VaylantzBusterBaron);

            AddExecutor(ExecutorType.Activate, CardId.SaionTheVaylantzArcher, VaylantzActivate);

            AddExecutor(ExecutorType.Activate, CardId.VaylantzWakeningSoloActivation, VaylantzWakeningEffect);
            AddExecutor(ExecutorType.Activate, CardId.VaylantzWorldShinraBansho, VaylantzWorldEffect);
            AddExecutor(ExecutorType.Activate, CardId.VaylantzWorldKonigWissen, VaylantzWorldEffect);

            // Default spell sets/repos
            AddExecutor(ExecutorType.SpellSet, DefaultSpellSet);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // Solfachord Pendulum control — prefer going first for scale setup + boss monsters
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            
            if (ShouldGoBreakBoard)
            {
                // Solfachord going second: prioritize pendulum summon for board-breaking
            }
        }

        private long StringId(int cardId, int optionIndex)
        {
            return ((long)cardId << 4) + optionIndex;
        }

        public bool MulcharmyFuwalosEffect()
        {
            if (!SmartHandTrapChain()) return false;
            bool controlsOtherCards = Bot.GetMonsters().Any(m => m != null && m.Id != CardId.MulcharmyFuwalos && m.Id != CardId.MulcharmyPurulia) ||
                                       Bot.GetSpells().Any(s => s != null);
            if (controlsOtherCards) return false;

            return (Duel.LastChainPlayer == 1 || Duel.LastSummonPlayer == 1) &&
                   Duel.Player == 1 &&
                   (Bot.HasInHand(CardId.MulcharmyFuwalos) || Bot.HasInMonstersZone(CardId.MulcharmyFuwalos));
        }

        public bool MulcharmyPuruliaEffect()
        {
            if (!SmartHandTrapChain()) return false;
            bool controlsOtherCards = Bot.GetMonsters().Any(m => m != null && m.Id != CardId.MulcharmyFuwalos && m.Id != CardId.MulcharmyPurulia) ||
                                       Bot.GetSpells().Any(s => s != null);
            if (controlsOtherCards) return false;

            return (Duel.LastChainPlayer == 1 || Duel.LastSummonPlayer == 1) &&
                   Duel.Player == 1 &&
                   Bot.HasInHand(CardId.MulcharmyPurulia);
        }

        public bool PsyFramegearGammaEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            if (Bot.GetMonsterCount() > 0) return false;
            if (!Bot.HasInHand(CardId.PsyFramegearGamma)) return false;

            var lastChainCard = Util.GetLastChainCard();
            if (lastChainCard == null || !lastChainCard.IsMonster()) return false;

            // Ensure Driver is available in Deck/Hand/GY
            bool driverAvailable = Bot.HasInHand(CardId.PsyFrameDriver) ||
                                   Bot.Graveyard.Any(c => c.Id == CardId.PsyFrameDriver) ||
                                   GetRemainingCount(CardId.PsyFrameDriver) > 0;
            if (!driverAvailable) return false;

            return true;
        }

        public bool HeraldOfOrangeLightEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;

            var lastChainCard = Util.GetLastChainCard();
            if (lastChainCard != null && !lastChainCard.IsMonster()) return false;

            var activating = (Card != null && Card.Id == CardId.HeraldOfOrangeLight && Bot.Hand.Contains(Card))
                ? Card
                : Bot.Hand.FirstOrDefault(c => c.Id == CardId.HeraldOfOrangeLight);

            if (activating == null) return false;

            return Bot.Hand.Any(card => card != activating &&
                                        card.IsMonster() &&
                                        (card.Race == (int)CardRace.Fairy || card.HasSetcode(0x15f)));
        }

        public bool WPFancyBallNegateEffect()
        {
            var lastChainCard = Util.GetLastChainCard();
            return Duel.LastChainPlayer == 1 &&
                   Bot.GetMonsters().Any(m => m.IsCode(CardId.WPFancyBall) && m.IsSpecialSummoned) &&
                   lastChainCard != null &&
                   lastChainCard.IsMonster() &&
                   (lastChainCard.Location == CardLocation.MonsterZone || lastChainCard.Location == CardLocation.Grave);
        }

        public bool WPFancyBallQuickLinkEffect()
        {
            if (Duel.Player != 1 || !Duel.IsMainPhase()) return false;
            if (!Bot.HasInMonstersZone(CardId.WPFancyBall)) return false;

            bool canMakeSPLittleKnight = Bot.HasInExtra(CardId.SPLittleKnight) && 
                (Bot.GetMonsters().Any(m => m != null && m != Card && m.HasType(CardType.Effect)) ||
                 Enemy.GetMonsters().Any(m => m != null && m.HasType(CardType.Effect) && m.HasType(CardType.Link) && m.LinkCount <= 2));

            return canMakeSPLittleKnight || Bot.GetMonsterCount() >= 2;
        }

        public bool SolfachordSolfegiaScaleNegateEffect()
        {
            return Duel.LastChainPlayer == 1 &&
                   (Card == null || Card.Location == CardLocation.SpellZone) &&
                   Bot.GetMonsters().Any(m => m.IsCode(CardId.GranSolfachordCoolia) && m.IsFaceup());
        }

        public bool SolfachordSolfegiaHandSpSummon()
        {
            return (Card == null || Card.Location == CardLocation.Hand) &&
                   Bot.GetMonsters().All(m => m.HasSetcode(0x15f));
        }

        public bool SolfachordSolfegiaMZoneSummon()
        {
            return (Card == null || Card.Location == CardLocation.MonsterZone) &&
                   (Bot.Hand.Any(c => c.HasSetcode(0x15f) && c.Id != CardId.SolfachordSolfegia) ||
                    Bot.Graveyard.Any(c => c.HasSetcode(0x15f) && c.IsMonster()) ||
                    HasRemainingCardWithSetcode(0x15f));
        }

        public bool SolfachordPrimoaPZoneBounce()
        {
            return (Card == null || Card.Location == CardLocation.SpellZone) &&
                   Bot.SpellZone.Any(c => c != null && c.HasSetcode(0x15f) && IsPZone(c));
        }

        public bool SolfachordPrimoaSummonSearch()
        {
            if (ShouldSkipCombo()) return false;
            return (Card == null || Card.Location == CardLocation.MonsterZone) &&
                   HasRemainingMonsterWithSetcode(0x15f);
        }

        public bool SolfachordPrimoaLinkRecycle()
        {
            return (Card == null || Card.Location == CardLocation.MonsterZone) &&
                   (Bot.Graveyard.Any(c => c.HasSetcode(0x15f) && c.IsMonster()) ||
                    Bot.ExtraDeck.Any(c => c.HasSetcode(0x15f) && c.IsFaceup()));
        }

        public bool SolfachordHappinessActivate()
        {
            if (ShouldSkipCombo()) return false;
            if (Duel.Player != 0 || !Duel.IsMainPhase()) return false;

            // Option 1: Discard 1, search 2 (requires another card in hand)
            bool canSearch = Bot.Hand.Count >= 2 && HasRemainingMonsterWithSetcode(0x15f);

            // Option 3: Special Summon 2 from PZones
            int pZoneCount = Bot.SpellZone.Count(c => c != null && IsPZone(c));
            bool canSummon = pZoneCount >= 2 && Bot.GetMonsterCount() <= 3;

            // Option 2: Additional Pendulum Summon (always selectable but requires valid PSummon setup)
            bool canExtraPSummon = pZoneCount > 0;

            return canSearch || canSummon || canExtraPSummon;
        }

        public bool SolfachordHarmoniaEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.Hand)
                return Duel.Player == 0 && Duel.IsMainPhase() && !Bot.HasInSpellZone(CardId.SolfachordHarmonia, faceUp: true);

            return Duel.Player == 0 && Duel.IsMainPhase() &&
                   (Bot.ExtraDeck.Any(c => c != null && c.IsFaceup() && c.HasSetcode(0x15f)) ||
                    Bot.Graveyard.Any(c => c != null && c.HasSetcode(0x15f)));
        }

        public bool VaylantzWakeningEffect()
        {
            return Duel.Player == 0 && Duel.IsMainPhase() &&
                   (Bot.MonsterZone[1] == null || Bot.MonsterZone[3] == null) &&
                   Bot.Hand.Any(c => c != null && IsVaylantz(c.Id));
        }

        public bool VaylantzWorldEffect()
        {
            if (Card.Location == CardLocation.Hand && Bot.HasInSpellZone(Card.Id, faceUp: true)) return false;
            return Duel.Player == 0 && Duel.IsMainPhase();
        }

        public override int OnSelectOption(IList<long> options)
        {
            for (int i = 0; i < options.Count; ++i)
            {
                long option = options[i];

                // Solfachord Happiness
                if (option == StringId(CardId.SolfachordHappiness, 1))
                {
                    // Discard 1, search 2. High priority.
                    if (Bot.Hand.Count >= 1) return i;
                }
                if (option == StringId(CardId.SolfachordHappiness, 3))
                {
                    // Special Summon 2 from PZones
                    int pZoneCount = Bot.SpellZone.Count(c => c != null && IsPZone(c));
                    if (pZoneCount >= 2 && Bot.GetMonsterCount() <= 3) return i;
                }
                if (option == StringId(CardId.SolfachordHappiness, 2))
                {
                    // Additional Pendulum Summon
                    return i;
                }

                // Triple Tactics Talent
                if (option == StringId(CardId.TripleTacticsTalent, 0)) // Draw 2
                {
                    if (Bot.GetHandCount() <= 2) return i;
                }
                if (option == StringId(CardId.TripleTacticsTalent, 1)) // Take Control
                {
                    if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= 2500)) return i;
                }
                if (option == StringId(CardId.TripleTacticsTalent, 2)) // Hand Rip
                {
                    return i;
                }
            }
            return base.OnSelectOption(options);
        }

        public override bool OnSelectYesNo(long desc)
        {
            if (desc == StringId(CardId.SolfachordSolfegia, 2))
            {
                // PZone negation
                return true;
            }
            if (desc == StringId(CardId.SolfachordSolfegia, 3))
            {
                // MZone Tribute itself option
                bool hasGyOrExtra = Bot.Graveyard.Any(c => c.HasSetcode(0x15f) && c.IsMonster()) ||
                                    Bot.ExtraDeck.Any(c => c.HasSetcode(0x15f) && c.IsFaceup());
                if (hasGyOrExtra) return true;

                if (Bot.GetMonsterCount() >= 5) return true;

                return false;
            }
            return base.OnSelectYesNo(desc);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return null;

            // Hint 500 / 505: Cost / Tribute selection (Protect scales and bosses)
            if (hint == 500 || hint == 505)
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (IsAceCard(c)) return 900;
                    if (IsPZone(c)) return 800;
                    return 100;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // Hint 501 / 549: Removal / Disruption Target Selection
            if (hint == 501 || hint == 549)
            {
                var oppCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (oppCards.Count > 0)
                {
                    var sortedOpp = oppCards.OrderByDescending(c => {
                        if (c.IsSpell() && c.IsFaceup() && (c.HasType(CardType.Continuous) || c.HasType(CardType.Field))) return 4000;
                        if (c.IsMonster() && c.IsFaceup() && !c.IsDisabled()) return 3000 + c.Attack;
                        if (c.IsFacedown()) return 2000;
                        return 1000;
                    }).ToList();
                    return sortedOpp.Take(max).ToList();
                }
            }

            // Hint 511 / 512 / 513 / 533 / 508 / 504: Material Selection (Protect Ace cards & active scales)
            if (hint == 511 || hint == 512 || hint == 513 || hint == 533 || hint == 508 || hint == 504)
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (IsAceCard(c)) return 900;
                    if (IsPZone(c)) return 800;
                    if (c.IsCode(CardId.MulcharmyFuwalos) || c.IsCode(CardId.MulcharmyPurulia)) return 700;
                    return 100;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            if (Card != null && Card.Id == CardId.SolfachordSolfegia)
            {
                var sorted = cards.OrderBy(c => {
                    if (c.IsCode(CardId.DoSolfachordCoolia)) return 1;
                    if (c.IsCode(CardId.DoSolfachordCutia)) return 2;
                    if (c.IsCode(CardId.SolfachordPrimoa)) return 3;
                    if (c.HasSetcode(0x15f)) return 4;
                    return 100;
                }).ToList();
                var result = new List<ClientCard>();
                for (int i = 0; i < Math.Min(max, sorted.Count); ++i)
                {
                    result.Add(sorted[i]);
                }
                return result;
            }

            bool isDeck = cards.All(c => c.Location == CardLocation.Deck);
            bool isPZone = cards.All(c => IsPZone(c));
            bool isGyOrExtra = cards.All(c => c.Location == CardLocation.Grave || c.Location == CardLocation.Extra);

            // S:P Little Knight target selection
            if (Card != null && Card.IsCode(CardId.SPLittleKnight))
            {
                if (max == 2)
                {
                    var enemyMonsters = cards.Where(c => c.Controller == 1 && c.Location == CardLocation.MonsterZone).OrderByDescending(c => c.Attack).ToList();
                    var myMonsters = cards.Where(c => c.Controller == 0 && c.Location == CardLocation.MonsterZone).OrderBy(c => c.Attack).ToList();

                    if (enemyMonsters.Any() && myMonsters.Any())
                    {
                        return new List<ClientCard> { myMonsters.First(), enemyMonsters.First() };
                    }
                }

                var fieldTargets = cards.Where(c => c.Controller == 1 && c.Location == CardLocation.MonsterZone).OrderByDescending(c => c.Attack).ToList();
                var gyOrSpellTargets = cards.Where(c => c.Controller == 1 && (c.Location == CardLocation.Grave || c.Location == CardLocation.SpellZone)).ToList();

                if (fieldTargets.Any()) return new List<ClientCard> { fieldTargets.First() };
                if (gyOrSpellTargets.Any()) return new List<ClientCard> { gyOrSpellTargets.First() };
            }

            // GranSolfachord Coolia negation targets selection
            if (Card != null && Card.IsCode(CardId.GranSolfachordCoolia))
            {
                var targets = cards.Where(c => c.Controller == 1 && c.IsFaceup() && !c.IsDisabled()).OrderBy(c => {
                    if (c.HasType(CardType.Link) || c.HasType(CardType.Xyz) || c.HasType(CardType.Synchro) || c.HasType(CardType.Fusion)) return 1;
                    if (c.IsMonster()) return 2;
                    if (c.HasType(CardType.Continuous) || c.HasType(CardType.Field) || c.HasType(CardType.Equip)) return 3;
                    return 100;
                }).ToList();

                var result = new List<ClientCard>();
                for (int i = 0; i < Math.Min(max, targets.Count); ++i)
                {
                    result.Add(targets[i]);
                }
                return result;
            }

            // Vaylantz movement / summon target selections
            if (Card != null && (Card.IsCode(CardId.VaylantzBusterBaron) || Card.IsCode(CardId.NazukiTheVaylantzNinja) || Card.IsCode(CardId.VaylantzWorldShinraBansho) || Card.IsCode(CardId.VaylantzWorldKonigWissen)))
            {
                var shinonome = cards.FirstOrDefault(c => c.IsCode(CardId.ShinonomeTheVaylantzPriestess) && c.Location == CardLocation.MonsterZone);
                if (shinonome != null)
                {
                    return new List<ClientCard> { shinonome };
                }
            }

            // Electrumite destroy target selection (destroy our own unimportant cards)
            if (Card != null && Card.IsCode(CardId.HeavymetalfoesElectrumite) && cards.All(c => c.Controller == 0))
            {
                var sorted = cards.OrderBy(c => {
                    if (c.IsCode(CardId.VaylantzWorldShinraBansho)) return 1;
                    if (c.IsCode(CardId.VaylantzWorldKonigWissen)) return 2;
                    if (IsVaylantz(c.Id) && c.Location == CardLocation.SpellZone) return 3;
                    if (c.HasSetcode(0x15f) && c.Location == CardLocation.SpellZone && c.Id != CardId.DoSolfachordCutia && c.Id != CardId.SolfachordSolfegia) return 4;
                    if (c.IsCode(CardId.DoSolfachordCutia)) return 10;
                    if (c.IsCode(CardId.SolfachordSolfegia)) return 11;
                    return 100;
                }).ToList();
                var result = new List<ClientCard>();
                for (int i = 0; i < Math.Min(max, sorted.Count); ++i)
                {
                    result.Add(sorted[i]);
                }
                return result;
            }

            if (isDeck)
            {
                // Heavymetalfoes Electrumite deck dump target: Solfachord Primoa is highest priority
                if (Card != null && Card.IsCode(CardId.HeavymetalfoesElectrumite))
                {
                    var sorted = cards.OrderBy(c => {
                        if (c.IsCode(CardId.SolfachordPrimoa)) return 1;
                        if (c.IsCode(CardId.DoSolfachordCutia)) return 2;
                        if (c.IsCode(CardId.SolfachordSolfegia)) return 3;
                        if (c.IsCode(CardId.DoSolfachordCoolia)) return 4;
                        if (c.IsCode(CardId.ReSolfachordDreamia)) return 5;
                        return 100;
                    }).ToList();
                    var result = new List<ClientCard>();
                    for (int i = 0; i < Math.Min(max, sorted.Count); ++i)
                    {
                        result.Add(sorted[i]);
                    }
                    return result;
                }

                // Beyond the Pendulum search target (prioritize finding low vs high scales)
                if (Card != null && Card.IsCode(CardId.BeyondThePendulum))
                {
                    bool hasLow = Bot.Hand.Any(c => GetScale(c.Id) <= 1 && GetScale(c.Id) >= 0) ||
                                  Bot.SpellZone.Any(c => c != null && (c.Sequence == 6 || c.Sequence == 7) && GetScale(c.Id) <= 1);
                    bool hasHigh = Bot.Hand.Any(c => GetScale(c.Id) >= 7) ||
                                   Bot.SpellZone.Any(c => c != null && (c.Sequence == 6 || c.Sequence == 7) && GetScale(c.Id) >= 7);

                    var sorted = cards.OrderBy(c => {
                        if (!hasLow)
                        {
                            if (c.IsCode(CardId.ShinonomeTheVaylantzPriestess)) return 1;
                            if (c.IsCode(CardId.SolfachordPrimoa)) return 2;
                            if (c.IsCode(CardId.DoSolfachordCoolia)) return 3;
                        }
                        else if (!hasHigh)
                        {
                            if (c.IsCode(CardId.DoSolfachordCutia)) return 1;
                            if (c.IsCode(CardId.SolfachordSolfegia)) return 2;
                            if (c.IsCode(CardId.ReSolfachordDreamia)) return 3;
                        }
                        if (c.IsCode(CardId.DoSolfachordCutia)) return 10;
                        if (c.IsCode(CardId.SolfachordPrimoa)) return 11;
                        if (c.IsCode(CardId.ShinonomeTheVaylantzPriestess)) return 12;
                        if (c.IsCode(CardId.SolfachordSolfegia)) return 13;
                        return 100;
                    }).ToList();
                    var result = new List<ClientCard>();
                    for (int i = 0; i < Math.Min(max, sorted.Count); ++i)
                    {
                        result.Add(sorted[i]);
                    }
                    return result;
                }

                // Default search from Deck: dynamic scale based priority
                {
                    bool hasLowScale = Bot.Hand.Any(c => GetScale(c.Id) <= 1 && GetScale(c.Id) >= 0) ||
                                       Bot.SpellZone.Any(c => c != null && (c.Sequence == 6 || c.Sequence == 7) && GetScale(c.Id) <= 1 && GetScale(c.Id) >= 0);
                    bool hasHighScale = Bot.Hand.Any(c => GetScale(c.Id) >= 7) ||
                                        Bot.SpellZone.Any(c => c != null && (c.Sequence == 6 || c.Sequence == 7) && GetScale(c.Id) >= 7);

                    var sorted = cards.OrderBy(c => {
                        int scale = GetScale(c.Id);
                        if (!hasLowScale && hasHighScale && scale >= 0 && scale <= 1)
                        {
                            if (c.IsCode(CardId.DoSolfachordCoolia)) return 1;
                            if (c.IsCode(CardId.SolfachordPrimoa)) return 2;
                            if (c.IsCode(CardId.ShinonomeTheVaylantzPriestess)) return 3;
                            return 4;
                        }
                        if (!hasHighScale && hasLowScale && scale >= 7)
                        {
                            if (c.IsCode(CardId.DoSolfachordCutia)) return 1;
                            if (c.IsCode(CardId.SolfachordSolfegia)) return 2;
                            if (c.IsCode(CardId.ReSolfachordDreamia)) return 3;
                            return 4;
                        }
                        if (c.IsCode(CardId.DoSolfachordCutia)) return 10;
                        if (c.IsCode(CardId.DoSolfachordCoolia)) return 11;
                        if (c.IsCode(CardId.SolfachordSolfegia)) return 12;
                        if (c.IsCode(CardId.SolfachordPrimoa)) return 13;
                        if (c.IsCode(CardId.SolfachordHappiness)) return 14;
                        if (c.IsCode(CardId.ReSolfachordDreamia)) return 15;
                        if (c.IsCode(CardId.SolfachordHarmonia)) return 16;
                        return 100;
                    }).ToList();

                    var result = new List<ClientCard>();
                    for (int i = 0; i < Math.Min(max, sorted.Count); ++i)
                    {
                        result.Add(sorted[i]);
                    }
                    return result;
                }
            }

            if (isPZone)
            {
                if (max == 1)
                {
                    // Bounce scale priority: Cutia > Solfegia > Coolia > Dreamia
                    var sorted = cards.OrderBy(c => {
                        if (c.IsCode(CardId.DoSolfachordCutia)) return 1;
                        if (c.IsCode(CardId.SolfachordSolfegia)) return 2;
                        if (c.IsCode(CardId.DoSolfachordCoolia)) return 3;
                        if (c.IsCode(CardId.ReSolfachordDreamia)) return 4;
                        if (c.IsCode(CardId.SolfachordPrimoa)) return 5;
                        return 100;
                    }).ToList();

                    var result = new List<ClientCard>();
                    for (int i = 0; i < Math.Min(max, sorted.Count); ++i)
                    {
                        result.Add(sorted[i]);
                    }
                    return result;
                }
                else
                {
                    // Summon from PZone priority: Coolia > Solfegia > Primoa > Dreamia > Cutia
                    var sorted = cards.OrderBy(c => {
                        if (c.IsCode(CardId.DoSolfachordCoolia)) return 1;
                        if (c.IsCode(CardId.SolfachordSolfegia)) return 2;
                        if (c.IsCode(CardId.SolfachordPrimoa)) return 3;
                        if (c.IsCode(CardId.ReSolfachordDreamia)) return 4;
                        if (c.IsCode(CardId.DoSolfachordCutia)) return 5;
                        return 100;
                    }).ToList();

                    var result = new List<ClientCard>();
                    for (int i = 0; i < Math.Min(max, sorted.Count); ++i)
                    {
                        result.Add(sorted[i]);
                    }
                    return result;
                }
            }

            if (isGyOrExtra)
            {
                // Heavymetalfoes Electrumite Extra Deck retrieve target: Solfachord Primoa is highest priority
                if (Card != null && Card.IsCode(CardId.HeavymetalfoesElectrumite))
                {
                    var sorted = cards.OrderBy(c => {
                        if (c.IsCode(CardId.SolfachordPrimoa)) return 1;
                        if (c.IsCode(CardId.SolfachordSolfegia)) return 2;
                        if (c.IsCode(CardId.DoSolfachordCutia)) return 3;
                        if (c.IsCode(CardId.DoSolfachordCoolia)) return 4;
                        if (c.IsCode(CardId.ReSolfachordDreamia)) return 5;
                        return 100;
                    }).ToList();
                    var result = new List<ClientCard>();
                    for (int i = 0; i < Math.Min(max, sorted.Count); ++i)
                    {
                        result.Add(sorted[i]);
                    }
                    return result;
                }

                // Retrieving priority: Cutia > Solfegia > Coolia > Primoa > Dreamia
                {
                    var sorted = cards.OrderBy(c => {
                        if (c.IsCode(CardId.DoSolfachordCutia)) return 1;
                        if (c.IsCode(CardId.SolfachordSolfegia)) return 2;
                        if (c.IsCode(CardId.DoSolfachordCoolia)) return 3;
                        if (c.IsCode(CardId.SolfachordPrimoa)) return 4;
                        if (c.IsCode(CardId.ReSolfachordDreamia)) return 5;
                        return 100;
                    }).ToList();

                    var result = new List<ClientCard>();
                    for (int i = 0; i < Math.Min(max, sorted.Count); ++i)
                    {
                        result.Add(sorted[i]);
                    }
                    return result;
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.IsCode(CardId.GranSolfachordCoolia) ||
                   card.IsCode(CardId.AccesscodeTalker) ||
                   card.IsCode(CardId.ArktosXIIChronochasmVaylantz) ||
                   card.IsCode(CardId.VaylantzGenesisGrandDuke) ||
                   card.IsCode(CardId.UnderworldGoddessOfTheClosedWorld) ||
                   card.IsCode(CardId.KnightmareGryphon) ||
                   card.IsCode(CardId.WPFancyBall) ||
                   card.IsCode(CardId.ExceedThePendulum) ||
                   card.IsCode(CardId.HeavymetalfoesElectrumite) ||
                   card.IsCode(CardId.SPLittleKnight) ||
                   card.IsCode(CardId.BeyondThePendulum) ||
                   card.IsCode(CardId.IPMasquerena);
        }

        protected override bool IsBoardStrongEnough()
        {
            // Gran Coolia on field = boss pendulum established
            if (Bot.HasInMonstersZone(CardId.GranSolfachordCoolia))
                return true;
            // Accesscode Talker = strong enough for OTK
            if (Bot.HasInMonstersZone(CardId.AccesscodeTalker))
                return true;
            // Underworld Goddess = ultimate board lock
            if (Bot.HasInMonstersZone(CardId.UnderworldGoddessOfTheClosedWorld))
                return true;
            // 3+ monsters with scales set = sufficient pendulum board
            if (Bot.GetMonsterCount() >= 3)
            {
                int pZoneCount = Bot.SpellZone.Count(c => c != null && IsPZone(c));
                if (pZoneCount >= 2) return true;
            }
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            // Stop once we have Gran Coolia or Accesscode on board
            if (Bot.HasInMonstersZone(CardId.GranSolfachordCoolia)
                || Bot.HasInMonstersZone(CardId.AccesscodeTalker)
                || Bot.HasInMonstersZone(CardId.UnderworldGoddessOfTheClosedWorld))
                return base.ShouldStopExtending();
            return false;
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.IsCode(CardId.AshBlossomAndJoyousSpring) || c.IsCode(CardId.MulcharmyFuwalos) || c.IsCode(CardId.MulcharmyPurulia) || c.IsCode(CardId.MaxxC))
                return 800;
            if (c.IsCode(CardId.SolfachordSolfegia)) return 50;
            if (c.IsCode(CardId.SolfachordPrimoa)) return 40;
            if (c.IsCode(CardId.WPFancyBall)) return 30;
            if (c.IsCode(CardId.ReSolfachordDreamia)) return 10;
            if (c.IsCode(CardId.DoSolfachordCutia)) return 5;
            return 100;
        }

        // --- Custom Helpers & Extra Deck Summon Conditions ---

        private bool IsVaylantz(int cardId)
        {
            return cardId == CardId.ShinonomeTheVaylantzPriestess ||
                   cardId == CardId.VaylantzBusterBaron ||
                   cardId == CardId.HojoTheVaylantzWarrior ||
                   cardId == CardId.NazukiTheVaylantzNinja ||
                   cardId == CardId.VaylantzMadMarquess ||
                   cardId == CardId.VaylantzVoltageViscount ||
                   cardId == CardId.SaionTheVaylantzArcher;
        }

        private int GetScale(int cardId)
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
                case CardId.ShinonomeTheVaylantzPriestess: return 1;
                case CardId.HojoTheVaylantzWarrior: return 4;
                case CardId.NazukiTheVaylantzNinja: return 3;
                case CardId.VaylantzMadMarquess: return 2;
                case CardId.VaylantzVoltageViscount: return 5;
                case CardId.VaylantzBusterBaron: return 2;
                case CardId.SaionTheVaylantzArcher: return 3;
                default:
                    return -1;
            }
        }

        public bool ScaleActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;

            ClientCard leftScale = Util.GetPZone(0, 0);
            ClientCard rightScale = Util.GetPZone(0, 1);

            if (leftScale != null && rightScale != null) return false;

            int myScale = GetScale(Card.Id);
            if (myScale == -1) return false;

            if (leftScale == null && rightScale == null)
            {
                // Vaylantz monster: place if we can summon it to MMZ column 1 or column 3
                if (IsVaylantz(Card.Id))
                {
                    return Bot.MonsterZone[1] == null || Bot.MonsterZone[3] == null;
                }

                // Solfachord monster: place if we have partner scale, Harmonia, or Happiness
                bool hasLowScaleInHand = Bot.Hand.Any(c => c != Card && GetScale(c.Id) <= 1 && GetScale(c.Id) >= 0);
                bool hasHighScaleInHand = Bot.Hand.Any(c => c != Card && GetScale(c.Id) >= 7);

                if (myScale <= 1 && hasHighScaleInHand) return true;
                if (myScale >= 7 && hasLowScaleInHand) return true;

                if (Bot.HasInSpellZone(CardId.SolfachordHarmonia) || Bot.HasInHand(CardId.SolfachordHappiness)) return true;

                return false;
            }
            else
            {
                ClientCard existingCard = leftScale ?? rightScale;
                int existingScale = GetScale(existingCard.Id);

                // Check for valid low+high scale Pendulum range
                if (existingScale <= 1 && myScale >= 7) return true;
                if (existingScale >= 7 && myScale <= 1) return true;

                // Vaylantz monster: can always place if its column is empty
                if (IsVaylantz(Card.Id))
                {
                    if (leftScale == null && Bot.MonsterZone[1] == null) return true;
                    if (rightScale == null && Bot.MonsterZone[3] == null) return true;
                }

                return false;
            }
        }

        public bool VaylantzActivate()
        {
            if (IsPZone(Card))
            {
                int targetCol = Card.Sequence == 6 || Card.Sequence == 0 ? 1 : 3;
                return Bot.MonsterZone[targetCol] == null;
            }
            if (Card.Location == CardLocation.Hand)
            {
                return ScaleActivate();
            }
            return Card.Location == CardLocation.MonsterZone;
        }

        public bool ElectrumiteSpSummon()
        {
            return Bot.GetMonsters().Count(m => m.HasType(CardType.Pendulum)) >= 2 &&
                   !Bot.HasInMonstersZone(CardId.HeavymetalfoesElectrumite);
        }

        public bool ElectrumiteEffect()
        {
            return Card.Location == CardLocation.MonsterZone;
        }

        public bool BeyondThePendulumSpSummon()
        {
            int effectMonsters = Bot.GetMonsters().Count(m => m.HasType(CardType.Effect));
            bool hasPendulum = Bot.GetMonsters().Any(m => m.HasType(CardType.Pendulum));
            bool hasFullScales = Util.GetPZone(0, 0) != null && Util.GetPZone(0, 1) != null;
            return effectMonsters >= 2 && hasPendulum && !hasFullScales &&
                   !Bot.HasInMonstersZone(CardId.BeyondThePendulum);
        }

        public bool BeyondThePendulumEffect()
        {
            return Card.Location == CardLocation.MonsterZone;
        }

        public bool ExceedThePendulumSpSummon()
        {
            return Bot.GetMonsters().Count(m => m.HasType(CardType.Effect)) >= 2 &&
                   Bot.ExtraDeck.Any(m => m.IsFaceup() && m.HasType(CardType.Pendulum)) &&
                   !Bot.HasInMonstersZone(CardId.ExceedThePendulum);
        }

        public bool ExceedThePendulumEffect()
        {
            return Card.Location == CardLocation.MonsterZone;
        }

        public bool GranSolfachordCooliaSpSummon()
        {
            return Bot.GetMonsters().Count() >= 3 &&
                   Bot.SpellZone.Any(c => c != null && c.HasSetcode(0x15f) && IsPZone(c)) &&
                   !Bot.HasInMonstersZone(CardId.GranSolfachordCoolia);
        }

        public bool WPFancyBallSpSummon()
        {
            return Bot.GetMonsters().Count() >= 2 &&
                   !Bot.HasInMonstersZone(CardId.WPFancyBall);
        }

        public bool SPLittleKnightSpSummon()
        {
            return Enemy.GetMonsterCount() > 0 && Bot.GetMonsters().Count() >= 2 &&
                   !Bot.HasInMonstersZone(CardId.SPLittleKnight);
        }

        public bool SPLittleKnightEffect()
        {
            return true;
        }

        public bool IPMasquerenaSpSummon()
        {
            return Bot.GetMonsters().Count() >= 2 &&
                   !Bot.HasInMonstersZone(CardId.IPMasquerena);
        }

        public bool ArktosXIISpSummon()
        {
            return Bot.GetMonsters().Count(m => IsVaylantz(m.Id) && m.Level >= 5) >= 2;
        }

        public bool GenesisGrandDukeSpSummon()
        {
            return Bot.GetMonsters().Any(m => IsVaylantz(m.Id) && m.Level >= 5);
        }

        public bool AccesscodeSpSummon()
        {
            return !Util.IsTurn1OrMain2() && Bot.GetMonsters().Any(m => m.LinkCount >= 3) &&
                   (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0);
        }

        public bool AccesscodeEffect()
        {
            return Card.Location == CardLocation.MonsterZone && Duel.Player == 0 && Enemy.GetFieldCount() > 0;
        }

        public bool LinkuribohSpSummon()
        {
            return Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Level == 1 && c.Id != CardId.Linkuriboh);
        }

        public bool ArtemisSpSummon()
        {
            return Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id == CardId.ShinonomeTheVaylantzPriestess);
        }

        public bool KnightmareGryphonSpSummon()
        {
            return Bot.GetMonsterCount() >= 3 && !Bot.HasInMonstersZone(CardId.KnightmareGryphon);
        }

        public bool KnightmareGryphonEffect()
        {
            return Card.Location == CardLocation.MonsterZone && Duel.Player == 0 &&
                   Bot.Graveyard.Any(c => c != null && (c.IsSpell() || c.IsTrap()));
        }

        public bool UnderworldGoddessSpSummon()
        {
            return Bot.GetMonsterCount() >= 4 && Util.GetProblematicEnemyCard() != null;
        }

        public bool UnderworldGoddessEffect()
        {
            return Card.Location == CardLocation.MonsterZone && Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled());
        }

        public bool VaylantzBossRemovalEffect()
        {
            return Card.Location == CardLocation.MonsterZone && Duel.Player == 0 && Enemy.GetFieldCount() > 0;
        }

        public bool GranSolfachordCooliaEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                var lastChainCard = Util.GetLastChainCard();
                if (lastChainCard != null)
                {
                    bool hasOddScaleInExtra = Bot.ExtraDeck.Any(c => c.IsFaceup() && c.HasType(CardType.Pendulum) && GetScale(c.Id) > 0 && (GetScale(c.Id) % 2 != 0));
                    if (hasOddScaleInExtra) return true;
                }
            }

            bool opponentHasActiveCard = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled()) ||
                                         Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && !c.IsDisabled());

            return opponentHasActiveCard;
        }

        public bool DoSolfachordCutiaEffect()
        {
            return Card.Location == CardLocation.MonsterZone && HasRemainingMonsterWithSetcode(0x15f);
        }

        public bool DoSolfachordCooliaMonsterEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Chain Quick Effect: destroy opponent's activated monster
            if (Duel.LastChainPlayer == 1)
            {
                return true;
            }

            // Ignition Effect: negate opponent's faceup monsters
            if (Duel.Player == 0 && Duel.IsMainPhase())
            {
                var targets = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup() && !m.IsDisabled() && !m.IsShouldNotBeTarget()).ToList();
                if (targets.Count > 0)
                {
                    AI.SelectCard(targets);
                    return true;
                }
            }
            return false;
        }

        public bool SolSolfachordGraciaMonsterEffect()
        {
            return Card.Location == CardLocation.MonsterZone;
        }

        public bool SolfachordPrimoaSpSummon()
        {
            return Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.HasSetcode(0x15f));
        }

        public bool PiriReisMapEffect()
        {
            if (Duel.Player != 0 || Duel.Phase != DuelPhase.Main1) return false;
            if (Bot.LifePoints <= 2000) return false;
            if (Bot.HasInHand(CardId.SolfachordPrimoa)) return false;

            if (GetRemainingCount(CardId.SolfachordPrimoa) > 0)
            {
                AI.SelectCard(CardId.SolfachordPrimoa);
                return true;
            }
            return false;
        }

        public bool TripleTacticsTalentEffect()
        {
            return Duel.Player == 0 && Duel.LastChainPlayer == 1;
        }

        public bool IsPZone(ClientCard card)
        {
            if (card == null) return false;
            if (Duel.IsNewRule)
            {
                return card.Location == CardLocation.SpellZone && (card.Sequence == 0 || card.Sequence == 4);
            }
            else
            {
                return card.Location == CardLocation.SpellZone && (card.Sequence == 6 || card.Sequence == 7);
            }
        }
    }

    [Deck("Expert_2026_Solfachord", "2026_Solfachord")]
    public class ExpertSolfachordExecutor : _2026_SolfachordExecutor
    {
        private string _duelId;
        public ExpertSolfachordExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            _duelId = $"duel_{Guid.NewGuid():N}";
            // [REMOVED-AI-TRAINING] ExpertDataLogger.EnsureInitialized(ExpertDataLogger.FindProjectRoot());
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
