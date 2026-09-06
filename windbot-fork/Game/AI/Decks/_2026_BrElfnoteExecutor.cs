using System;
using YGOSharp.OCGWrapper.Enums;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_BrElfnote", "2026_BrElfnote")]
    public class _2026_BrElfnoteExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck
            public const int ElfnoteLucina = 13597785;
            public const int ElfnoteTinia = 59581480;
            public const int ElfnoteRegina = 56651978;
            public const int ElfnoteFortuna = 85976588;
            public const int ElfnotePowerPatron = 12375297;
            public const int MediusThePure = 97556336;
            public const int PowerPatronShadowSpiritJunordo = 10266279;
            public const int FallenOfTheWhiteDragon = 73819701;
            public const int IncredibleEcclesiaTheVirtuous = 55273562;
            public const int FallenOfAlbaz = 68468459;
            public const int AshBlossom = 14558127;
            public const int MaxxC = 23434538;
            public const int MulcharmyFuwalos = 42141493;
            public const int MulcharmyPurulia = 84192580;
            public const int DrollAndLockBird = 94145021;
            public const int EffectVeiler = 97268402;
            public const int FidraulisHarmonia = 70088809;
            public const int FydraulisHarmonia = 70088809; // Alias for consistency
            public const int ElfnotesWelcomeHome = 64491754;
            public const int TheFallenAndTheVirtuous = 30271097;
            public const int CalledByTheGrave = 24224830;
            public const int ElfnotesRhapsodiaOfMadness = 24092792;

            // Extra Deck
            public const int GoldenCloudBeastMalong = 93125329;
            public const int JunoraThePowerPatronOfTuning = 5914858;
            public const int InfernalStrikeFighter = 66122213; // Alias for the card ID
            public const int BlackRoseDragon = 73580471;
            public const int ElfnoteSeraphimStrelitzia = 42302563;
            public const int PSYFramelordOmega = 74586817;
            public const int EcclesiaAndTheDarkDragon = 78397661;
            public const int BaronneDeFleur = 84815190;
            public const int PsychicEndPunisher = 60465049;
            public const int DespianLuluwalilith = 53971455;
            public const int AlbionTheBrandedDragon = 87746184;
            public const int RindbrummTheStrikingDragon = 51409648;
            public const int TheDragonThatDevoursTheDogma = 76666602;
            public const int SprindTheIrondashDragon = 1906812;
            public const int MirrorjadeTheIcebladeDragon = 44146295;

            // Side Deck
            public const int BystialMagnamhut = 33854624;
            public const int GhostBelle = 73642296;
            public const int GhostOgre = 59438930;
            public const int HarpiesFeatherDuster = 18144506;
            public const int HeavyStorm = 19613556;
            public const int IllusionGate = 33017964;
            public const int DimensionalBarrier = 83326048;
            public const int SolemnJudgment = 41420027;
            public const int SolemnWarning = 84749824;
            public const int BlueEyesChaosMAXDragon = 55410871;
            public const int AzureEyesSilverDragon = 40908371;
            public const int Number38HopeHarbinger = 63767246;

            // Opponent Cards (for lock checks)
            public const int DarkMagicianTheDragonKnight = 41721210;
            public const int EternalSoul = 48680970;
            public const int AlternativeWhiteDragon = 38517737;
            public const int MagicianNavigation = 7922915;
            public const int DarkMagician = 46986414;
            public const int DarkMagicalCircle = 47222536;
        }

        private static readonly int[] ElfnoteCards = {
            CardId.ElfnoteLucina,
            CardId.ElfnoteTinia,
            CardId.ElfnoteRegina,
            CardId.ElfnoteFortuna,
            CardId.ElfnotePowerPatron,
            CardId.PowerPatronShadowSpiritJunordo,
            CardId.ElfnotesWelcomeHome,
            CardId.ElfnotesRhapsodiaOfMadness
        };

        private static readonly int[] BossMonsters = {
            CardId.BaronneDeFleur,
            CardId.PsychicEndPunisher,
            CardId.MirrorjadeTheIcebladeDragon,
            CardId.DespianLuluwalilith,
            CardId.AlbionTheBrandedDragon,
            CardId.RindbrummTheStrikingDragon,
            CardId.ElfnoteSeraphimStrelitzia,
            CardId.EcclesiaAndTheDarkDragon,
            CardId.JunoraThePowerPatronOfTuning
        };

        // Once-per-turn & state flags
        private bool _welcomeHomeUsed = false;
        private bool _rhapsodiaUsed = false;
        private bool _harmoniaUsed = false;
        private bool _fallenVirtuousUsed = false;
        private bool _number38NegateUsed = false;

        public override bool OnSelectHand()
        {
            // Elfnote Synchro control โ€” prefer going first to set up Baronne + disruption
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _welcomeHomeUsed = false;
            _rhapsodiaUsed = false;
            _harmoniaUsed = false;
            _fallenVirtuousUsed = false;
            _number38NegateUsed = false;

            // โ”€โ”€ Going-Second BreakBoard: prioritize disruption over combo โ”€โ”€
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
            }
        }

        public override void OnChaining(int player, ClientCard card)
        {
            base.OnChaining(player, card);
            if (player == 1 && card != null && card.IsCode(CardId.Number38HopeHarbinger))
            {
                _number38NegateUsed = true;
            }
        }

        // FieldGuard inherited: IsSpecialSummonBlocked, CanDealLethal, CanOTK,
        // ShouldSkipCombo, NeedsBoardPresence, IsInGrindGame, EnemyHasKnownNegate โ’ inherited

        protected override bool IsBoardStrongEnough()
        {
            int disruptionCount = 0;
            if (Bot.HasInMonstersZone(CardId.BaronneDeFleur)) disruptionCount += 2;
            if (Bot.HasInMonstersZone(CardId.MirrorjadeTheIcebladeDragon)) disruptionCount += 2;
            if (Bot.HasInMonstersZone(CardId.DespianLuluwalilith)) disruptionCount++;
            if (Bot.HasInMonstersZone(CardId.PsychicEndPunisher)) disruptionCount += 2;
            if (Bot.HasInMonstersZone(CardId.RindbrummTheStrikingDragon)) disruptionCount++;
            if (Bot.HasInMonstersZone(CardId.GoldenCloudBeastMalong)) disruptionCount++;
            if (Bot.HasInMonstersZone(CardId.ElfnoteSeraphimStrelitzia)) disruptionCount++;
            if (Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.DimensionalBarrier))) disruptionCount += 2;
            if (Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.SolemnJudgment))) disruptionCount += 2;
            if (Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.SolemnWarning))) disruptionCount += 2;
            if (Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.ElfnotesRhapsodiaOfMadness))) disruptionCount++;
            if (Bot.HasInHand(CardId.AshBlossom) || Bot.HasInHand(CardId.EffectVeiler) || Bot.HasInHand(CardId.MaxxC)) disruptionCount++;
            return disruptionCount >= 3;
        }

        protected override bool ShouldStopExtending()
        {
            if (IsBoardStrongEnough())
                return base.ShouldStopExtending();
            return false;
        }

        private bool OpponentHasThreateningMonster()
        {
            foreach (ClientCard c in Enemy.MonsterZone)
            {
                if (c == null || !c.IsFaceup() || c.IsDisabled()) continue;
                if (c.Attack >= 2500 || c.IsFloodgate())
                    return true;
            }
            return false;
        }

        public _2026_BrElfnoteExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Layer 2: Register boss monsters for HeuristicGuard violation detection
            HeuristicGuard.RegisterAceCards(
                CardId.BaronneDeFleur, CardId.PsychicEndPunisher,
                CardId.MirrorjadeTheIcebladeDragon, CardId.DespianLuluwalilith,
                CardId.AlbionTheBrandedDragon, CardId.RindbrummTheStrikingDragon,
                CardId.ElfnoteSeraphimStrelitzia, CardId.EcclesiaAndTheDarkDragon,
                CardId.JunoraThePowerPatronOfTuning
            );

            // โ”€โ”€ Combo Router: Sequencing โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Regina-Baronne-Play",
                RequiredCards = new List<int> { CardId.ElfnoteRegina },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.ElfnoteRegina, ActionType = ExecutorType.Activate, Description = "Activate Regina from hand" },
                    new() { CardId = CardId.ElfnoteRegina, ActionType = ExecutorType.Activate, Description = "Activate Regina center zone effect" },
                    new() { CardId = CardId.ElfnotePowerPatron, ActionType = ExecutorType.Activate, Description = "Activate Power Patron level mod + synchro" },
                    new() { CardId = CardId.BaronneDeFleur, ActionType = ExecutorType.SpSummon, Description = "Synchro Summon Baronne" }
                },
                EndBoardScore = 85
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Lucina-To-Baronne",
                RequiredCards = new List<int> { CardId.ElfnoteLucina },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.ElfnoteLucina, ActionType = ExecutorType.Summon, Description = "Normal Summon Lucina" },
                    new() { CardId = CardId.ElfnoteLucina, ActionType = ExecutorType.Activate, Description = "Activate Lucina search" },
                    new() { CardId = CardId.ElfnoteRegina, ActionType = ExecutorType.Activate, Description = "Activate Regina from hand" },
                    new() { CardId = CardId.ElfnoteRegina, ActionType = ExecutorType.Activate, Description = "Activate Regina center zone effect" },
                    new() { CardId = CardId.ElfnotePowerPatron, ActionType = ExecutorType.Activate, Description = "Activate Power Patron level mod + synchro" },
                    new() { CardId = CardId.BaronneDeFleur, ActionType = ExecutorType.SpSummon, Description = "Synchro Summon Baronne" }
                },
                EndBoardScore = 80
            });

            // โ”€โ”€ Bait Planner โ”€โ”€
            BaitPlanner.RegisterComboStarters(CardId.ElfnoteRegina, CardId.ElfnotePowerPatron);
            BaitPlanner.RegisterBaitCards(CardId.ElfnoteLucina, CardId.ElfnoteTinia, CardId.ElfnoteFortuna, CardId.MediusThePure);

            // โ”€โ”€ Chain Advisor โ”€โ”€
            ChainAdvisor.RegisterHighValueTargets(CardId.ElfnoteRegina, CardId.ElfnotePowerPatron);

            // Hand Traps & Reactive Hand Effects
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, () => SmartHandTrapChain() && DefaultMaxxC());
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyPurulia, MulcharmyEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelle, GhostBelleEffect);
            AddExecutor(ExecutorType.Activate, CardId.GhostOgre, GhostOgreEffect);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, () => SmartHandTrapChain() && DefaultEffectVeiler());
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);

            // Fydraulis Harmonia (activated from hand during opponent's turn as a disruption)
            AddExecutor(ExecutorType.Activate, CardId.FidraulisHarmonia, FidraulisHarmoniaEffect);

            // Boss Monster Quick Effects
            AddExecutor(ExecutorType.Activate, CardId.BaronneDeFleur, BaronneDeFleurEffect);
            AddExecutor(ExecutorType.Activate, CardId.MirrorjadeTheIcebladeDragon, MirrorjadeTheIcebladeDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.RindbrummTheStrikingDragon, RindbrummTheStrikingDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.PsychicEndPunisher, PsychicEndPunisherEffect);

            // Spells (Board Breakers)
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, DefaultHarpiesFeatherDusterFirst);
            AddExecutor(ExecutorType.Activate, CardId.HeavyStorm, DefaultHeavyStorm);
            AddExecutor(ExecutorType.Activate, CardId.IllusionGate, IllusionGateEffect);

            // Engine Spells
            AddExecutor(ExecutorType.Activate, CardId.ElfnotesWelcomeHome, ElfnotesWelcomeHomeEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheFallenAndTheVirtuous, TheFallenAndTheVirtuousEffect);

            // Special Summons from Hand (Chainless Summon Conditions)
            AddExecutor(ExecutorType.SpSummon, CardId.ElfnoteLucina, ElfnoteLucinaSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ElfnoteTinia, ElfnoteTiniaSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ElfnoteFortuna, ElfnoteFortunaSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.IncredibleEcclesiaTheVirtuous, IncredibleEcclesiaTheVirtuousSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.FallenOfTheWhiteDragon, FallenOfTheWhiteDragonSpSummon);

            // Monster Effects (Activated from hand/field)
            AddExecutor(ExecutorType.Activate, CardId.MediusThePure, MediusThePureEffect);
            AddExecutor(ExecutorType.Activate, CardId.IncredibleEcclesiaTheVirtuous, IncredibleEcclesiaTheVirtuousEffect);
            AddExecutor(ExecutorType.Activate, CardId.FallenOfAlbaz, FallenOfAlbazEffect);
            AddExecutor(ExecutorType.Activate, CardId.FallenOfTheWhiteDragon, FallenOfTheWhiteDragonFieldEffect);
            
            AddExecutor(ExecutorType.Activate, CardId.ElfnoteLucina, ElfnoteLucinaEffect);
            AddExecutor(ExecutorType.Activate, CardId.ElfnoteTinia, ElfnoteTiniaEffect);
            AddExecutor(ExecutorType.Activate, CardId.ElfnoteFortuna, ElfnoteFortunaEffect);
            AddExecutor(ExecutorType.Activate, CardId.ElfnoteRegina, ElfnoteReginaEffect);
            AddExecutor(ExecutorType.Activate, CardId.ElfnotePowerPatron, ElfnotePowerPatronEffect);
            AddExecutor(ExecutorType.Activate, CardId.PowerPatronShadowSpiritJunordo, PowerPatronShadowSpiritJunordoEffect);

            // Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.MediusThePure);
            AddExecutor(ExecutorType.Summon, CardId.ElfnoteLucina);
            AddExecutor(ExecutorType.Summon, CardId.ElfnoteTinia);
            AddExecutor(ExecutorType.Summon, CardId.ElfnoteFortuna);
            AddExecutor(ExecutorType.Summon, CardId.IncredibleEcclesiaTheVirtuous);
            AddExecutor(ExecutorType.Summon, CardId.FallenOfAlbaz);
            AddExecutor(ExecutorType.Summon, CardId.ElfnotePowerPatron);
            AddExecutor(ExecutorType.Summon, CardId.ElfnoteRegina);
            AddExecutor(ExecutorType.Summon, CardId.PowerPatronShadowSpiritJunordo);

            // Extra Deck Synchro Summoning Priority
            AddExecutor(ExecutorType.SpSummon, CardId.BaronneDeFleur, BaronneSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.PsychicEndPunisher, PsychicEndPunisherSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.JunoraThePowerPatronOfTuning);
            AddExecutor(ExecutorType.SpSummon, CardId.DespianLuluwalilith);
            AddExecutor(ExecutorType.SpSummon, CardId.ElfnoteSeraphimStrelitzia, ElfnoteSeraphimStrelitziaSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.EcclesiaAndTheDarkDragon);
            AddExecutor(ExecutorType.SpSummon, CardId.GoldenCloudBeastMalong, GoldenCloudBeastMalongSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.PSYFramelordOmega);
            AddExecutor(ExecutorType.SpSummon, CardId.BlackRoseDragon, BlackRoseDragonSpSummon);

            // Fallen of the White Dragon (summon from hand)
            AddExecutor(ExecutorType.Activate, CardId.FallenOfTheWhiteDragon, FallenOfTheWhiteDragonHandEffect);

            // Extra Deck Fusion Summoning (Usually initiated by card effects)
            AddExecutor(ExecutorType.SpSummon, CardId.MirrorjadeTheIcebladeDragon);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);

            // Fydraulis Harmonia (activated from hand during opponent's turn as a disruption)
            AddExecutor(ExecutorType.Activate, CardId.FidraulisHarmonia, FidraulisHarmoniaEffect);

            // Boss Monster Quick Effects
            AddExecutor(ExecutorType.Activate, CardId.BaronneDeFleur, BaronneDeFleurEffect);
            AddExecutor(ExecutorType.Activate, CardId.MirrorjadeTheIcebladeDragon, MirrorjadeTheIcebladeDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.RindbrummTheStrikingDragon, RindbrummTheStrikingDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.PsychicEndPunisher, PsychicEndPunisherEffect);

            // Spells (Board Breakers)
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, DefaultHarpiesFeatherDusterFirst);
            AddExecutor(ExecutorType.Activate, CardId.HeavyStorm, DefaultHeavyStorm);
            AddExecutor(ExecutorType.Activate, CardId.IllusionGate, IllusionGateEffect);

            // Engine Spells
            AddExecutor(ExecutorType.Activate, CardId.ElfnotesWelcomeHome, ElfnotesWelcomeHomeEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheFallenAndTheVirtuous, TheFallenAndTheVirtuousEffect);

            // Special Summons from Hand (Chainless Summon Conditions)
            AddExecutor(ExecutorType.SpSummon, CardId.ElfnoteLucina, ElfnoteLucinaSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ElfnoteTinia, ElfnoteTiniaSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ElfnoteFortuna, ElfnoteFortunaSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.IncredibleEcclesiaTheVirtuous, IncredibleEcclesiaTheVirtuousSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.FallenOfTheWhiteDragon, FallenOfTheWhiteDragonSpSummon);

            // Monster Effects (Activated from hand/field)
            AddExecutor(ExecutorType.Activate, CardId.MediusThePure, MediusThePureEffect);
            AddExecutor(ExecutorType.Activate, CardId.IncredibleEcclesiaTheVirtuous, IncredibleEcclesiaTheVirtuousEffect);
            AddExecutor(ExecutorType.Activate, CardId.FallenOfAlbaz, FallenOfAlbazEffect);
            AddExecutor(ExecutorType.Activate, CardId.FallenOfTheWhiteDragon, FallenOfTheWhiteDragonFieldEffect);
            
            AddExecutor(ExecutorType.Activate, CardId.ElfnoteLucina, ElfnoteLucinaEffect);
            AddExecutor(ExecutorType.Activate, CardId.ElfnoteTinia, ElfnoteTiniaEffect);
            AddExecutor(ExecutorType.Activate, CardId.ElfnoteFortuna, ElfnoteFortunaEffect);
            AddExecutor(ExecutorType.Activate, CardId.ElfnoteRegina, ElfnoteReginaEffect);
            AddExecutor(ExecutorType.Activate, CardId.ElfnotePowerPatron, ElfnotePowerPatronEffect);
            AddExecutor(ExecutorType.Activate, CardId.PowerPatronShadowSpiritJunordo, PowerPatronShadowSpiritJunordoEffect);

            // Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.MediusThePure);
            AddExecutor(ExecutorType.Summon, CardId.ElfnoteLucina);
            AddExecutor(ExecutorType.Summon, CardId.ElfnoteTinia);
            AddExecutor(ExecutorType.Summon, CardId.ElfnoteFortuna);
            AddExecutor(ExecutorType.Summon, CardId.IncredibleEcclesiaTheVirtuous);
            AddExecutor(ExecutorType.Summon, CardId.FallenOfAlbaz);
            AddExecutor(ExecutorType.Summon, CardId.ElfnotePowerPatron);
            AddExecutor(ExecutorType.Summon, CardId.ElfnoteRegina);
            AddExecutor(ExecutorType.Summon, CardId.PowerPatronShadowSpiritJunordo);

            // Extra Deck Synchro Summoning Priority
            AddExecutor(ExecutorType.SpSummon, CardId.BaronneDeFleur, BaronneSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.PsychicEndPunisher, PsychicEndPunisherSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.JunoraThePowerPatronOfTuning);
            AddExecutor(ExecutorType.SpSummon, CardId.DespianLuluwalilith);
            AddExecutor(ExecutorType.SpSummon, CardId.ElfnoteSeraphimStrelitzia, ElfnoteSeraphimStrelitziaSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.EcclesiaAndTheDarkDragon);
            AddExecutor(ExecutorType.SpSummon, CardId.GoldenCloudBeastMalong, GoldenCloudBeastMalongSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.PSYFramelordOmega);
            AddExecutor(ExecutorType.SpSummon, CardId.BlackRoseDragon, BlackRoseDragonSpSummon);

            // Fallen of the White Dragon (summon from hand)
            AddExecutor(ExecutorType.Activate, CardId.FallenOfTheWhiteDragon, FallenOfTheWhiteDragonHandEffect);

            // Extra Deck Fusion Summoning (Usually initiated by card effects)
            AddExecutor(ExecutorType.SpSummon, CardId.MirrorjadeTheIcebladeDragon);
            AddExecutor(ExecutorType.SpSummon, CardId.AlbionTheBrandedDragon);
            AddExecutor(ExecutorType.SpSummon, CardId.RindbrummTheStrikingDragon);
            AddExecutor(ExecutorType.SpSummon, CardId.TheDragonThatDevoursTheDogma);
            AddExecutor(ExecutorType.SpSummon, CardId.SprindTheIrondashDragon);

            // Extra Deck Monster Effects
            AddExecutor(ExecutorType.Activate, CardId.ElfnoteSeraphimStrelitzia, ElfnoteSeraphimStrelitziaEffect);
            AddExecutor(ExecutorType.Activate, CardId.EcclesiaAndTheDarkDragon, EcclesiaAndTheDarkDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.DespianLuluwalilith, DespianLuluwalilithEffect);
            AddExecutor(ExecutorType.Activate, CardId.GoldenCloudBeastMalong, GoldenCloudBeastMalongEffect);
            AddExecutor(ExecutorType.Activate, CardId.PSYFramelordOmega, PSYFramelordOmegaEffect);
            AddExecutor(ExecutorType.Activate, CardId.JunoraThePowerPatronOfTuning, JunoraEffect);
            
            AddExecutor(ExecutorType.Activate, CardId.AlbionTheBrandedDragon, AlbionTheBrandedDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheDragonThatDevoursTheDogma, TheDragonThatDevoursTheDogmaEffect);
            AddExecutor(ExecutorType.Activate, CardId.SprindTheIrondashDragon, SprindEffect);

            // Traps & Continuous Spells (Set/Activate)
            AddExecutor(ExecutorType.SpellSet, CardId.DimensionalBarrier);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnWarning);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnJudgment);
            AddExecutor(ExecutorType.SpellSet, CardId.ElfnotesRhapsodiaOfMadness);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave);

            AddExecutor(ExecutorType.Activate, CardId.DimensionalBarrier, DefaultDimensionalBarrier);
            AddExecutor(ExecutorType.Activate, CardId.SolemnWarning, DefaultSolemnWarning);
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, DefaultSolemnJudgment);
            AddExecutor(ExecutorType.Activate, CardId.ElfnotesRhapsodiaOfMadness, ElfnotesRhapsodiaOfMadnessEffect);

            // Side Deck Bystial
            AddExecutor(ExecutorType.SpSummon, CardId.BystialMagnamhut, BystialMagnamhutSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.BystialMagnamhut, BystialMagnamhutEffect);

            // Reposition / Battle
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        private bool IsTargetable(ClientCard card)
        {
            if (card == null) return false;
            
            // Chaos MAX is always untargetable by opponent's card effects
            if (card.IsCode(CardId.BlueEyesChaosMAXDragon)) return false;

            // Dragon monsters are untargetable if opponent controls Azure-Eyes Silver Dragon
            if (card.HasRace(CardRace.Dragon) && Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.AzureEyesSilverDragon)))
                return false;

            return true;
        }

        protected override bool IsViableEffectTarget(ClientCard card)
        {
            if (card == null) return false;
            if (!IsTargetable(card)) return false;

            // If opponent controls active Eternal Soul
            bool isEternalSoulActive = Enemy.GetSpells().Any(s => s != null && s.IsFaceup() && s.IsCode(CardId.EternalSoul) && !s.IsDisabled());
            if (isEternalSoulActive)
            {
                // Dark Magician and Dark Magician the Dragon Knight are unaffected by card effects
                if (card.IsCode(CardId.DarkMagician) || card.IsCode(CardId.DarkMagicianTheDragonKnight))
                {
                    return false;
                }
            }

            // If opponent controls active Dark Magician the Dragon Knight
            bool hasDragonKnight = Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.DarkMagicianTheDragonKnight) && !m.IsDisabled());
            if (hasDragonKnight && (card.IsSpell() || card.IsTrap()))
            {
                return false;
            }

            return true;
        }

        private bool IsCenterZoneEmpty()
        {
            return Bot.MonsterZone[2] == null;
        }

        private bool ElfnoteLucinaSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            if (IsBoardStrongEnough() && !IsInGrindGame()) return false;
            if (IsCenterZoneEmpty())
            {
                AI.SelectPosition(CardPosition.FaceUpDefence);
                return true;
            }
            return false;
        }

        private bool DrollAndLockBirdEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1 && Duel.LastChainPlayer == 1;
        }

        private bool ElfnoteTiniaSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            if (IsBoardStrongEnough() && !IsInGrindGame()) return false;
            if (IsCenterZoneEmpty())
            {
                AI.SelectPosition(CardPosition.FaceUpDefence);
                return true;
            }
            return false;
        }

        private bool ElfnoteFortunaSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            if (IsBoardStrongEnough() && !IsInGrindGame()) return false;
            if (IsCenterZoneEmpty())
            {
                AI.SelectPosition(CardPosition.FaceUpDefence);
                return true;
            }
            return false;
        }

        private bool IncredibleEcclesiaTheVirtuousSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            if (IsBoardStrongEnough() && !IsInGrindGame()) return false;
            AI.SelectPosition(CardPosition.FaceUpDefence);
            return true;
        }

        private bool ElfnoteSeraphimStrelitziaSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            if (IsBoardStrongEnough() && !IsInGrindGame()) return false;
            AI.SelectPosition(CardPosition.FaceUpDefence);
            return true;
        }

        private bool GoldenCloudBeastMalongSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            AI.SelectPosition(CardPosition.FaceUpDefence);
            return true;
        }

        private bool ElfnoteLucinaEffect()
        {
            if (OpponentHasActiveNegator(Card)) return false;
            if (EnemyHasKnownNegate()) return false;

            // Main Phase effect: Add 1 Elfnote monster from Deck to hand (except Lucina)
            if (Card != null && Card.Location == CardLocation.MonsterZone && Duel.Player == 0)
            {
                if (ShouldSkipCombo()) return false;
                if (IsBoardStrongEnough() && !IsInGrindGame()) return false;
                AI.SelectCard(new[] {
                    CardId.ElfnoteRegina,
                    CardId.ElfnoteTinia,
                    CardId.ElfnoteFortuna,
                    CardId.ElfnotePowerPatron
                });
                return true;
            }
            // Opponent's turn: Bounce Level 6 or lower opponent monster
            if (Duel.Player == 1)
            {
                ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.Level <= 6 && c.Level > 0 && c.IsFaceup() && IsViableEffectTarget(c));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
                return false;
            }
            return true;
        }

        private bool ElfnoteTiniaEffect()
        {
            if (OpponentHasActiveNegator(Card)) return false;
            if (EnemyHasKnownNegate()) return false;

            // Main Phase effect: Place 1 Elfnote Continuous Spell face-up on field
            if (Card != null && Card.Location == CardLocation.MonsterZone && Duel.Player == 0)
            {
                if (ShouldSkipCombo()) return false;
                if (IsBoardStrongEnough() && !IsInGrindGame()) return false;
                AI.SelectCard(CardId.ElfnotesWelcomeHome);
                return true;
            }
            // Opponent's turn: Banish 1 random card from opponent's hand until End Phase
            if (Duel.Player == 1)
            {
                return Enemy.Hand.Count > 0;
            }
            return true;
        }

        private bool ElfnoteFortunaEffect()
        {
            if (OpponentHasActiveNegator(Card)) return false;
            if (EnemyHasKnownNegate()) return false;

            // Main Phase effect: Place 1 Elfnote Continuous Trap face-up on field
            if (Card != null && Card.Location == CardLocation.MonsterZone && Duel.Player == 0)
            {
                if (ShouldSkipCombo()) return false;
                if (IsBoardStrongEnough() && !IsInGrindGame()) return false;
                AI.SelectCard(CardId.ElfnotesRhapsodiaOfMadness);
                return true;
            }
            // Opponent's turn: Bounce face-up opponent's Spell/Trap
            if (Duel.Player == 1)
            {
                var spells = Enemy.GetSpells().Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c)).ToList();
                if (spells.Count > 0)
                {
                    ClientCard target = spells.OrderBy(c => {
                        if (c.IsCode(CardId.EternalSoul)) return 0;
                        if (c.IsCode(CardId.DarkMagicalCircle)) return 1;
                        return 2;
                    }).First();

                    AI.SelectCard(target);
                    return true;
                }
                return false;
            }
            return true;
        }

        private bool ElfnoteReginaEffect()
        {
            if (OpponentHasActiveNegator(Card))
            {
                DecisionTracer.TraceSkip("ReginaEffect", "Opponent has active negator");
                return false;
            }
            if (EnemyHasKnownNegate()) return false;

            // Hand activation: Send another Elfnote card from hand/field to GY; Special Summon from hand
            if (Card != null && Card.Location == CardLocation.Hand)
            {
                if (IsSpecialSummonBlocked()) return false;
                if (ShouldSkipCombo()) return false;
                if (IsBoardStrongEnough() && !IsInGrindGame()) return false;

                // Bait check!
                var bait = GetBaitIfNeeded(Card);
                if (bait != null) return false;
                DecisionTracer.Trace("ReginaEffect", "Evaluating hand activation โ€” searching cost card...");
                ClientCard costCard = null;
                
                // 1. Center Zone monster (Zone 2) to free it for Regina
                ClientCard centerMonster = Bot.MonsterZone[2];
                if (centerMonster != null && centerMonster.IsCode(ElfnoteCards))
                {
                    costCard = centerMonster;
                    DecisionTracer.TraceSelect("ReginaEffect", "COST (step1: center zone)", costCard);
                }

                // 2. Power Patron in hand (discarding triggers its GY search)
                if (costCard == null)
                {
                    costCard = Bot.Hand.FirstOrDefault(c => c != null && c.IsCode(CardId.ElfnotePowerPatron));
                    if (costCard != null) DecisionTracer.TraceSelect("ReginaEffect", "COST (step2: PowerPatron hand)", costCard);
                }

                // 3. Other Elfnote monsters in hand (excluding Regina unless it's a duplicate)
                if (costCard == null)
                {
                    costCard = Bot.Hand.FirstOrDefault(c => c != null && c != Card && c.IsMonster() && c.IsCode(ElfnoteCards));
                    if (costCard != null) DecisionTracer.TraceSelect("ReginaEffect", "COST (step3: Elfnote hand)", costCard);
                }

                // 4. Other Elfnote monsters on field
                if (costCard == null)
                {
                    costCard = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsCode(ElfnoteCards));
                    if (costCard != null) DecisionTracer.TraceSelect("ReginaEffect", "COST (step4: Elfnote field)", costCard);
                }

                // 5. Duplicate Spells/Traps in hand (we have more than 1 Welcome Home or Rhapsodia, or already have one active on field)
                if (costCard == null)
                {
                    costCard = Bot.Hand.FirstOrDefault(c => c != null && c.IsCode(CardId.ElfnotesWelcomeHome, CardId.ElfnotesRhapsodiaOfMadness) &&
                        (Bot.Hand.Count(h => h.IsCode(c.Id)) > 1 || Bot.HasInSpellZone(c.Id)));
                    if (costCard != null) DecisionTracer.TraceSelect("ReginaEffect", "COST (step5: dup spell/trap)", costCard);
                }

                // 6. Fallback: Any Elfnote card in hand
                if (costCard == null)
                {
                    costCard = Bot.Hand.FirstOrDefault(c => c != null && c != Card && c.IsCode(ElfnoteCards));
                    if (costCard != null) DecisionTracer.TraceSelect("ReginaEffect", "COST (step6: any Elfnote hand)", costCard);
                }

                // 7. Fallback: Any Elfnote card on field (Spells/Traps)
                if (costCard == null)
                {
                    costCard = Bot.GetSpells().FirstOrDefault(c => c != null && c.IsCode(ElfnoteCards));
                    if (costCard != null) DecisionTracer.TraceSelect("ReginaEffect", "COST (step7: Elfnote spell field)", costCard);
                }

                if (costCard != null)
                {
                    DecisionTracer.TraceActivate("ReginaEffect", $"Hand activate with cost: {costCard.Name ?? "?"} ({costCard.Id})");
                    AI.SelectCard(costCard);
                    AI.SelectPosition(CardPosition.FaceUpDefence);
                    return true;
                }
                DecisionTracer.TraceSkip("ReginaEffect", "No valid Elfnote cost found in hand/field");
                return false;
            }
            // Center zone effect: Special Summon 1 Elfnote monster from Deck
            if (Card != null && Card.Location == CardLocation.MonsterZone && Card.Sequence == 2)
            {
                if (IsSpecialSummonBlocked()) return false;
                if (ShouldSkipCombo()) return false;
                DecisionTracer.TraceActivate("ReginaEffect", "Center zone effect โ€” summoning from Deck");
                AI.SelectCard(new[] {
                    CardId.ElfnotePowerPatron,
                    CardId.ElfnoteLucina,
                    CardId.ElfnoteTinia,
                    CardId.ElfnoteFortuna
                });
                AI.SelectPosition(CardPosition.FaceUpDefence);
                return true;
            }
            return true;
        }

        private bool ElfnotePowerPatronEffect()
        {
            if (Card != null && Card.Location == CardLocation.MonsterZone && OpponentHasActiveNegator(Card))
            {
                DecisionTracer.TraceSkip("PowerPatronEffect", "Opponent has active negator");
                return false;
            }
            if (EnemyHasKnownNegate()) return false;

            if (CanOTK())
            {
                DecisionTracer.TraceSkip("PowerPatronEffect", "Can OTK โ€” attack first, synchro in MP2");
                return false;
            }
            // Main Phase (Quick Effect): Target center zone monster; increase Level by 3 and Synchro Summon
            if (Card != null && Card.Location == CardLocation.MonsterZone)
            {
                if (IsSpecialSummonBlocked()) return false;
                if (ShouldSkipCombo()) return false;
                ClientCard centerMonster = Bot.MonsterZone[2];
                if (centerMonster == null)
                {
                    DecisionTracer.TraceSkip("PowerPatronEffect", "No center zone monster");
                    return false;
                }
                
                DecisionTracer.TraceSelect("PowerPatronEffect", "Synchro target (center)", centerMonster);
                AI.SelectCard(centerMonster);
                
                // Select the optimal Synchro monster from the Extra Deck based on resulting levels
                if (ShouldPrioritizePEP())
                {
                    DecisionTracer.Trace("PowerPatronEffect", "Priority: PsychicEndPunisher (LP advantage)");
                    AI.SelectNextCard(new[] {
                        CardId.PsychicEndPunisher,
                        CardId.BaronneDeFleur,
                        CardId.JunoraThePowerPatronOfTuning,
                        CardId.DespianLuluwalilith,
                        CardId.ElfnoteSeraphimStrelitzia,
                        CardId.EcclesiaAndTheDarkDragon,
                        CardId.PSYFramelordOmega
                    });
                }
                else
                {
                    DecisionTracer.Trace("PowerPatronEffect", "Priority: BaronneDeFleur (negate)");
                    AI.SelectNextCard(new[] {
                        CardId.BaronneDeFleur,
                        CardId.JunoraThePowerPatronOfTuning,
                        CardId.PsychicEndPunisher,
                        CardId.DespianLuluwalilith,
                        CardId.ElfnoteSeraphimStrelitzia,
                        CardId.EcclesiaAndTheDarkDragon,
                        CardId.PSYFramelordOmega
                    });
                }
                return true;
            }
            // GY effect: Add 1 Elfnote card from Deck to hand
            if (Card.Location == CardLocation.Grave)
            {
                if (ShouldSkipCombo()) return false;
                AI.SelectCard(new[] {
                    CardId.ElfnotesWelcomeHome,
                    CardId.ElfnoteRegina,
                    CardId.ElfnoteLucina,
                    CardId.ElfnoteTinia,
                    CardId.ElfnoteFortuna
                });
                return true;
            }
            return true;
        }

        private bool PowerPatronShadowSpiritJunordoEffect()
        {
            // Pendulum effect: Discard 1 to draw 2
            if (Card.Location == CardLocation.SpellZone)
            {
                AI.SelectCard(new[] {
                    CardId.ElfnotePowerPatron,
                    CardId.MediusThePure,
                    CardId.FallenOfAlbaz,
                    CardId.IncredibleEcclesiaTheVirtuous
                });
                return true;
            }
            // Monster effect: Banish top 3 cards face-down; destroy this and summon Junora
            if (Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(CardId.JunoraThePowerPatronOfTuning);
                return true;
            }
            return true;
        }

        private bool MediusThePureEffect()
        {
            if (OpponentHasActiveNegator(Card)) return false;
            if (EnemyHasKnownNegate()) return false;

            // Normal/Special Summoned: Add/Special Summon 1 Power Patron monster from Deck
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (ShouldSkipCombo()) return false;
                AI.SelectCard(CardId.ElfnotePowerPatron);
                AI.SelectPosition(CardPosition.FaceUpDefence);
                return true;
            }
            // GY effect: Shuffle 1 monster from hand/field into Deck; Special Summon this card
            if (Card.Location == CardLocation.Grave)
            {
                if (IsSpecialSummonBlocked()) return false;
                if (ShouldSkipCombo()) return false;
                if (IsBoardStrongEnough() && !IsInGrindGame()) return false;
                ClientCard shuffleTarget = Bot.Hand.FirstOrDefault(c => c != null && c.IsMonster() && c.IsCode(CardId.EffectVeiler, CardId.DrollAndLockBird, CardId.MaxxC));
                if (shuffleTarget == null)
                {
                    shuffleTarget = Bot.Hand.FirstOrDefault(c => c != null && c.IsMonster() && c != Card);
                }
                
                if (shuffleTarget != null)
                {
                    AI.SelectCard(shuffleTarget);
                    AI.SelectPosition(CardPosition.FaceUpDefence);
                    return true;
                }
                return false;
            }
            return true;
        }

        private bool FallenOfTheWhiteDragonSpSummon()
        {
            // Cost: Send Albaz-mentioning monster from Extra Deck to GY
            // Prioritize Albion for its End Phase search
            AI.SelectCard(new[] {
                CardId.AlbionTheBrandedDragon,
                CardId.RindbrummTheStrikingDragon,
                CardId.SprindTheIrondashDragon,
                CardId.TheDragonThatDevoursTheDogma
            });
            return true;
        }

        private bool FallenOfTheWhiteDragonHandEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            // Hand activation to summon itself
            if (Card != null && Card.Location == CardLocation.Hand)
            {
                DecisionTracer.Trace("FallenWhiteDragonHand", "Evaluating lock-avoidance logic...");

                // 1. If we already control one of our Level 10/11 boss monsters, the lock is fine.
                if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.BaronneDeFleur, CardId.PsychicEndPunisher, CardId.JunoraThePowerPatronOfTuning)))
                {
                    DecisionTracer.TraceActivate("FallenWhiteDragonHand", "Already have boss on field โ€” lock OK");
                    AI.SelectCard(new[] {
                        CardId.AlbionTheBrandedDragon,
                        CardId.RindbrummTheStrikingDragon,
                        CardId.SprindTheIrondashDragon
                    });
                    return true;
                }

                // 2. If we have Incredible Ecclesia in hand or on field, we should NOT use the hand effect.
                if (Bot.Hand.Any(c => c != null && c.IsCode(CardId.IncredibleEcclesiaTheVirtuous)) ||
                    Bot.GetMonsters().Any(c => c != null && c.IsCode(CardId.IncredibleEcclesiaTheVirtuous)))
                {
                    DecisionTracer.TraceSkip("FallenWhiteDragonHand", "Ecclesia available โ€” use her to avoid lock");
                    return false;
                }

                // 3. Check if we have any other viable Level 10/11 Synchro play.
                bool hasLevel6Elfnote = Bot.Hand.Any(c => c != null && c.IsCode(CardId.ElfnoteLucina, CardId.ElfnoteTinia, CardId.ElfnoteRegina, CardId.ElfnoteFortuna)) ||
                                        Bot.GetMonsters().Any(c => c != null && c.IsCode(CardId.ElfnoteLucina, CardId.ElfnoteTinia, CardId.ElfnoteRegina, CardId.ElfnoteFortuna));

                bool hasTuner = Bot.Hand.Any(c => c != null && c.IsCode(CardId.ElfnotePowerPatron, CardId.IncredibleEcclesiaTheVirtuous)) ||
                                Bot.GetMonsters().Any(c => c != null && c.IsCode(CardId.ElfnotePowerPatron, CardId.IncredibleEcclesiaTheVirtuous));

                bool hasTunerOrSearcher = hasTuner || Bot.Hand.Any(c => c != null && c.IsCode(CardId.MediusThePure)) ||
                                          Bot.GetMonsters().Any(c => c != null && c.IsCode(CardId.MediusThePure));

                if (hasLevel6Elfnote && hasTunerOrSearcher)
                {
                    DecisionTracer.TraceSkip("FallenWhiteDragonHand", $"Synchro play available: Lv6Elfnote={hasLevel6Elfnote}, Tuner={hasTuner}, Searcher={hasTunerOrSearcher}");
                    return false;
                }

                // If we have Regina in hand and another Elfnote card in hand/field, Regina can initiate the combo. Don't lock.
                bool hasReginaInHand = Bot.Hand.Any(c => c != null && c.IsCode(CardId.ElfnoteRegina));
                int elfnoteCount = Bot.Hand.Count(c => c != null && ElfnoteCards.Contains(c.Id)) +
                                   Bot.GetMonsters().Count(c => c != null && ElfnoteCards.Contains(c.Id)) +
                                   Bot.GetSpells().Count(c => c != null && ElfnoteCards.Contains(c.Id));
                if (hasReginaInHand && elfnoteCount >= 2)
                {
                    DecisionTracer.TraceSkip("FallenWhiteDragonHand", $"Regina combo available: elfnoteCount={elfnoteCount}");
                    return false;
                }

                // If we have Elfnotes Welcome Home active on field or in hand, and we can pay its cost
                bool hasWelcomeHome = Bot.HasInSpellZone(CardId.ElfnotesWelcomeHome) || Bot.Hand.Any(c => c != null && c.IsCode(CardId.ElfnotesWelcomeHome));
                if (hasWelcomeHome && !_welcomeHomeUsed && GetWelcomeHomeCostCard() != null)
                {
                    DecisionTracer.TraceSkip("FallenWhiteDragonHand", "WelcomeHome available โ€” don't lock");
                    return false;
                }

                // Otherwise, we have no other viable Level 10/11 play, so it's safe to activate.
                DecisionTracer.TraceActivate("FallenWhiteDragonHand", "No alternative Synchro play โ€” activating hand effect");
                AI.SelectCard(new[] {
                    CardId.AlbionTheBrandedDragon,
                    CardId.RindbrummTheStrikingDragon,
                    CardId.SprindTheIrondashDragon
                });
                return true;
            }
            return false;
        }

        private bool FallenOfTheWhiteDragonFieldEffect()
        {
            // Normal/Special Summoned: Special Summon 1 Ecclesia monster from hand/Deck/GY
            if (Card != null && Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(CardId.IncredibleEcclesiaTheVirtuous);
                AI.SelectPosition(CardPosition.FaceUpDefence);
                return true;
            }
            return false;
        }

        private bool OpponentHasAlbazFusionTarget()
        {
            foreach (var c in Enemy.GetMonsters())
            {
                if (c == null || !c.IsFaceup()) continue;
                if (c.HasAttribute(CardAttribute.Light)) return true;
                if (c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link)) return true;
                if (c.HasRace(CardRace.Beast) || c.HasRace(CardRace.BestWarrior) || c.HasRace(CardRace.WindBeast)) return true;
                if (c.Level >= 8) return true;
                if (c.IsSpecialSummoned && c.HasType(CardType.Effect)) return true;
            }
            return false;
        }

        private bool IncredibleEcclesiaTheVirtuousEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (EnemyHasKnownNegate()) return false;
            // Tribute this card; Special Summon 1 Fallen of Albaz or Fallen of the White Dragon
            if (Bot.Hand.Count > 0 && OpponentHasAlbazFusionTarget())
            {
                AI.SelectCard(new[] {
                    CardId.FallenOfAlbaz,
                    CardId.FallenOfTheWhiteDragon
                });
            }
            else
            {
                AI.SelectCard(new[] {
                    CardId.FallenOfTheWhiteDragon,
                    CardId.FallenOfAlbaz
                });
            }
            AI.SelectPosition(CardPosition.FaceUpDefence);
            return true;
        }

        private bool FallenOfAlbazEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (EnemyHasKnownNegate()) return false;
            // Fusion Summon using opponent's monsters. Discard 1 card as cost.
            AI.SelectCard(new[] {
                CardId.ElfnotePowerPatron,
                CardId.MediusThePure,
                CardId.MaxxC,
                CardId.EffectVeiler
            });
            return true;
        }

        private bool OpponentHasActiveNumber38()
        {
            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.Number38HopeHarbinger) && !c.IsDisabled()) && !_number38NegateUsed;
        }

        private bool CanActivateTheFallenAndTheVirtuous()
        {
            if (_fallenVirtuousUsed) return false;
            if (!Bot.Hand.Any(c => c != null && c.IsCode(CardId.TheFallenAndTheVirtuous))) return false;

            ClientCard destroyTarget = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && IsTargetable(c));
            if (destroyTarget == null)
            {
                destroyTarget = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && IsTargetable(c));
            }

            int[] validSummonTargets = {
                CardId.FallenOfAlbaz,
                CardId.FallenOfTheWhiteDragon,
                CardId.IncredibleEcclesiaTheVirtuous
            };
            ClientCard spTarget = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCode(validSummonTargets));

            return destroyTarget != null || spTarget != null;
        }

        private bool CanActivateJunordoPendulum()
        {
            if (!Bot.Hand.Any(c => c != null && c.IsCode(CardId.PowerPatronShadowSpiritJunordo))) return false;
            return Bot.GetSpellCount() < 5;
        }

        private bool HasBaitSpell()
        {
            return CanActivateTheFallenAndTheVirtuous() || CanActivateJunordoPendulum();
        }

        private ClientCard GetWelcomeHomeCostCard()
        {
            var elfnotesInDeck = Bot.Deck.Where(c => c != null && c.IsMonster() && ElfnoteCards.Contains(c.Id)).ToList();
            if (elfnotesInDeck.Count == 0) return null;

            // 1. Hand traps in hand (Veiler, Maxx C)
            ClientCard costCard = Bot.Hand.FirstOrDefault(c => c != null && c.IsMonster() && c.IsCode(CardId.EffectVeiler, CardId.MaxxC) && elfnotesInDeck.Any(d => d.Attribute != c.Attribute));
            
            // 2. Any other monster in hand
            if (costCard == null)
                costCard = Bot.Hand.FirstOrDefault(c => c != null && c.IsMonster() && elfnotesInDeck.Any(d => d.Attribute != c.Attribute));
            
            // 3. Field: non-ace materials/extenders
            if (costCard == null)
                costCard = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsCode(CardId.MediusThePure, CardId.ElfnotePowerPatron, CardId.PowerPatronShadowSpiritJunordo) && elfnotesInDeck.Any(d => d.Attribute != c.Attribute));
            
            // 4. Field: other Elfnotes
            if (costCard == null)
                costCard = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsCode(ElfnoteCards) && elfnotesInDeck.Any(d => d.Attribute != c.Attribute));
            
            // 5. Field: any non-boss monsters
            if (costCard == null)
                costCard = Bot.GetMonsters().FirstOrDefault(c => c != null && !IsAceCard(c) && elfnotesInDeck.Any(d => d.Attribute != c.Attribute));

            return costCard;
        }

        private ClientCard GetRhapsodiaCostCard()
        {
            var elfnotesInGY = Bot.Graveyard.Where(c => c != null && c.IsMonster() && ElfnoteCards.Contains(c.Id) && c.IsCanRevive()).ToList();
            if (elfnotesInGY.Count == 0) return null;

            // 1. Hand traps in hand (Veiler, Maxx C)
            ClientCard costCard = Bot.Hand.FirstOrDefault(c => c != null && c.IsMonster() && c.IsCode(CardId.EffectVeiler, CardId.MaxxC) && elfnotesInGY.Any(g => g.Attribute != c.Attribute));
            
            // 2. Any other monster in hand
            if (costCard == null)
                costCard = Bot.Hand.FirstOrDefault(c => c != null && c.IsMonster() && elfnotesInGY.Any(g => g.Attribute != c.Attribute));
            
            // 3. Field: non-ace materials/extenders
            if (costCard == null)
                costCard = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsCode(CardId.MediusThePure, CardId.ElfnotePowerPatron, CardId.PowerPatronShadowSpiritJunordo) && elfnotesInGY.Any(g => g.Attribute != c.Attribute));
            
            // 4. Field: other Elfnotes
            if (costCard == null)
                costCard = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsCode(ElfnoteCards) && elfnotesInGY.Any(g => g.Attribute != c.Attribute));
            
            // 5. Field: any non-boss monsters
            if (costCard == null)
                costCard = Bot.GetMonsters().FirstOrDefault(c => c != null && !IsAceCard(c) && elfnotesInGY.Any(g => g.Attribute != c.Attribute));

            return costCard;
        }

        private bool ElfnotesWelcomeHomeEffect()
        {
            if (EnemyHasKnownNegate()) return false;
            if (Card.Location == CardLocation.Hand && Bot.HasInSpellZone(CardId.ElfnotesWelcomeHome)) return false;

            if (OpponentHasActiveNumber38() && HasBaitSpell())
            {
                return false; // Yield to bait spells
            }

            if (Card.Location == CardLocation.Hand) return true; // Place continuous spell on field first

            if (_welcomeHomeUsed) return false;

            if (Card != null && Card.Location == CardLocation.SpellZone && !Card.IsFacedown())
            {
                ClientCard costCard = GetWelcomeHomeCostCard();
                if (costCard != null)
                {
                    var elfnotesInDeck = Bot.Deck.Where(c => c != null && c.IsMonster() && ElfnoteCards.Contains(c.Id) && c.Attribute != costCard.Attribute).ToList();
                    if (elfnotesInDeck.Count > 0)
                    {
                        AI.SelectCard(costCard);
                        int[] preferredTargets = {
                            CardId.ElfnoteRegina,
                            CardId.ElfnoteLucina,
                            CardId.ElfnoteTinia,
                            CardId.ElfnoteFortuna,
                            CardId.ElfnotePowerPatron
                        };
                        var orderedTargets = elfnotesInDeck.OrderBy(c => {
                            int idx = System.Array.IndexOf(preferredTargets, c.Id);
                            return idx >= 0 ? idx : 999;
                        }).Select(c => c.Id).ToArray();

                        AI.SelectNextCard(orderedTargets);
                        _welcomeHomeUsed = true;
                        return true;
                    }
                }
                return false;
            }
            return false;
        }

        private bool ElfnotesRhapsodiaOfMadnessEffect()
        {
            if (EnemyHasKnownNegate()) return false;
            if (Card.Location == CardLocation.Hand) return false;

            if (_rhapsodiaUsed) return false;

            if (Card != null && Card.Location == CardLocation.SpellZone)
            {
                ClientCard costCard = GetRhapsodiaCostCard();
                if (costCard != null)
                {
                    var elfnotesInGY = Bot.Graveyard.Where(c => c != null && c.IsMonster() && ElfnoteCards.Contains(c.Id) && c.Attribute != costCard.Attribute && c.IsCanRevive()).ToList();
                    if (elfnotesInGY.Count > 0)
                    {
                        AI.SelectCard(costCard);
                        int[] preferredTargets = {
                            CardId.ElfnoteRegina,
                            CardId.ElfnoteLucina,
                            CardId.ElfnoteTinia,
                            CardId.ElfnoteFortuna,
                            CardId.ElfnotePowerPatron
                        };
                        var orderedTargets = elfnotesInGY.OrderBy(c => {
                            int idx = System.Array.IndexOf(preferredTargets, c.Id);
                            return idx >= 0 ? idx : 999;
                        }).Select(c => c.Id).ToArray();

                        AI.SelectNextCard(orderedTargets);
                        AI.SelectPosition(CardPosition.FaceUpDefence);
                        _rhapsodiaUsed = true;
                        return true;
                    }
                }
                return false;
            }
            return false;
        }

        private bool OpponentHasActiveSpellNegator()
        {
            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.Number38HopeHarbinger) && !c.IsDisabled()) && !_number38NegateUsed)
                return true;
            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.BaronneDeFleur) && !c.IsDisabled()))
                return true;
            return false;
        }

        private bool TheFallenAndTheVirtuousEffect()
        {
            if (EnemyHasKnownNegate()) return false;
            if (_fallenVirtuousUsed) return false;

            if (OpponentHasActiveSpellNegator() && Duel.Phase == DuelPhase.End)
            {
                return false;
            }

            // Check if we have a valid opponent card to destroy
            var monsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c)).ToList();
            var spells = Enemy.GetSpells().Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c)).ToList();

            ClientCard destroyTarget = null;
            if (spells.Any(c => c.IsCode(CardId.EternalSoul)))
            {
                destroyTarget = spells.First(c => c.IsCode(CardId.EternalSoul));
            }
            else if (spells.Any(c => c.IsCode(CardId.DarkMagicalCircle)))
            {
                destroyTarget = spells.First(c => c.IsCode(CardId.DarkMagicalCircle));
            }
            else if (spells.Count > 0)
            {
                destroyTarget = spells.First();
            }
            else if (monsters.Count > 0)
            {
                destroyTarget = monsters.First();
            }

            // Check if we have a valid target in GY to Special Summon
            int[] validSummonTargets = {
                CardId.FallenOfAlbaz,
                CardId.FallenOfTheWhiteDragon,
                CardId.IncredibleEcclesiaTheVirtuous
            };
            ClientCard spTarget = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCode(validSummonTargets));

            // If we can do neither, do not activate
            if (destroyTarget == null && spTarget == null)
            {
                return false;
            }

            _fallenVirtuousUsed = true;

            // If we have an enemy card to destroy, we prefer destroying it
            if (destroyTarget != null)
            {
                AI.SelectCard(new[] {
                    CardId.AlbionTheBrandedDragon,
                    CardId.RindbrummTheStrikingDragon,
                    CardId.TheDragonThatDevoursTheDogma,
                    CardId.SprindTheIrondashDragon
                });
                AI.SelectNextCard(destroyTarget);
                return true;
            }

            // Otherwise, Special Summon
            if (spTarget != null)
            {
                AI.SelectCard(spTarget);
                AI.SelectPosition(CardPosition.FaceUpDefence);
                return true;
            }

            return false;
        }

        private bool IllusionGateEffect()
        {
            // Illusion Gate โ€” generic combo extender, check game state
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.MonsterZone && Card.IsDisabled()) return false;
            return true;
        }

        private bool ElfnoteSeraphimStrelitziaEffect()
        {
            // Special Summon 1 Level 6 or lower Elfnote from hand or GY
            AI.SelectCard(new[] {
                CardId.ElfnoteRegina,
                CardId.ElfnoteLucina,
                CardId.ElfnoteTinia,
                CardId.ElfnoteFortuna,
                CardId.ElfnotePowerPatron
            });
            AI.SelectPosition(CardPosition.FaceUpDefence);
            return true;
        }

        private bool EcclesiaAndTheDarkDragonEffect()
        {
            // Quick Effect during Main Phase: Banish this card; Special Summon 1 Fallen of Albaz or card mentioning it
            if (Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(new[] {
                    CardId.FallenOfTheWhiteDragon,
                    CardId.IncredibleEcclesiaTheVirtuous,
                    CardId.FallenOfAlbaz
                });
                AI.SelectPosition(CardPosition.FaceUpDefence);
                return true;
            }
            // GY effect: shuffle 1 Level 8 Fusion from GY/banished and 1 card on field into Deck
            if (Card.Location == CardLocation.Grave)
            {
                ClientCard fusionTarget = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCode(CardId.AlbionTheBrandedDragon, CardId.MirrorjadeTheIcebladeDragon));
                
                var monsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c)).ToList();
                var spells = Enemy.GetSpells().Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c)).ToList();

                ClientCard fieldTarget = null;
                if (spells.Any(c => c.IsCode(CardId.EternalSoul)))
                {
                    fieldTarget = spells.First(c => c.IsCode(CardId.EternalSoul));
                }
                else if (spells.Any(c => c.IsCode(CardId.DarkMagicalCircle)))
                {
                    fieldTarget = spells.First(c => c.IsCode(CardId.DarkMagicalCircle));
                }
                else if (spells.Count > 0)
                {
                    fieldTarget = spells.First();
                }
                else if (monsters.Count > 0)
                {
                    fieldTarget = monsters.First();
                }

                if (fusionTarget != null && fieldTarget != null)
                {
                    AI.SelectCard(fusionTarget);
                    AI.SelectNextCard(fieldTarget);
                    return true;
                }
                return false;
            }
            return true;
        }


        private bool ShouldPrioritizePEP()
        {
            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.Attack >= 3000 || c.IsCode(CardId.BlueEyesChaosMAXDragon)));
        }

        private bool IsSpecialSummonable(int cardId)
        {
            if (Main == null || Main.SpecialSummonableCards == null) return false;
            return Main.SpecialSummonableCards.Any(c => c != null && c.Id == cardId);
        }

        private bool PsychicEndPunisherSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            // Do not summon PEP on Turn 1, because LP is equal and it has no immunity.
            if (Duel.Turn == 1)
            {
                return false;
            }

            // Always summon if we can OTK
            if (CanOTK())
            {
                return true;
            }

            // Always summon if opponent has a high-ATK threat or Chaos MAX
            if (ShouldPrioritizePEP())
            {
                return true;
            }

            // Always summon if opponent has the DM lock
            bool hasDMLock = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.DarkMagicianTheDragonKnight)) &&
                             Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.EternalSoul));
            if (hasDMLock)
            {
                return true;
            }

            // Summon PEP if our LP is lower than opponent's LP (immune to activated effects)
            if (Bot.LifePoints < Enemy.LifePoints)
            {
                return true;
            }

            // If we have no other options and opponent has no monsters/threats, summon to push beater on field
            if (Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0)
            {
                return true;
            }

            return true;
        }

        private bool BaronneSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            if (IsBoardStrongEnough() && !IsInGrindGame()) return false;
            if (Bot.HasInExtra(CardId.PsychicEndPunisher))
            {
                // ONLY yield to PEP if PEP is actually special summonable right now
                if (IsSpecialSummonable(CardId.PsychicEndPunisher))
                {
                    // Yield to PEP if we can OTK
                    if (CanOTK())
                    {
                        return false;
                    }

                    // Yield to PEP if opponent has a high-ATK threat or Chaos MAX
                    if (ShouldPrioritizePEP())
                    {
                        return false;
                    }

                    // Yield to PEP if opponent has the Dark Magician the Dragon Knight + Eternal Soul lock
                    bool hasDMLock = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.DarkMagicianTheDragonKnight)) &&
                                     Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.EternalSoul));
                    if (hasDMLock)
                    {
                        return false;
                    }

                    // Yield to PEP if our LP is lower than opponent's LP (so PEP gets its effect immunity)
                    if (Bot.LifePoints < Enemy.LifePoints)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool BaronneShouldNegate()
        {
            ClientCard lastChainCard = Util.GetLastChainCard();
            if (lastChainCard == null)
            {
                return Duel.LastChainPlayer == 1;
            }

            if (lastChainCard.Controller == 0)
                return false;

            // Whitelist of minor triggers we should NOT waste Baronne's negate on
            int[] negateBlacklist = {
                79814787, // The White Stone of Legend
                71039903, // The White Stone of Ancients
                8240199,  // Sage with Eyes of Blue
                7084129,  // Magicians' Rod
                40908371, // Azure-Eyes Silver Dragon
            };

            if (lastChainCard.IsCode(negateBlacklist))
            {
                return false;
            }

            return true;
        }

        private bool BaronneDeFleurEffect()
        {
            if (Duel.Phase == DuelPhase.Standby && Duel.CurrentChain.Count == 0)
            {
                return false;
            }

            if (Duel.CurrentChain.Count > 0)
            {
                return BaronneShouldNegate();
            }

            // Find all viable targets on opponent's field
            List<ClientCard> targets = new List<ClientCard>();
            targets.AddRange(Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c)));
            targets.AddRange(Enemy.GetSpells().Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c)));

            if (targets.Count > 0)
            {
                // Prioritize: Eternal Soul -> Dark Magical Circle -> Boss/Other Spells/Traps -> Monsters
                ClientCard bestTarget = targets.OrderBy(c => {
                    if (c.IsCode(CardId.EternalSoul)) return 0;
                    if (c.IsCode(CardId.DarkMagicalCircle)) return 1;
                    if (c.IsSpell() || c.IsTrap()) return 2;
                    return 3;
                }).First();

                AI.SelectCard(bestTarget);
                return true;
            }
            return false;
        }

        private bool PsychicEndPunisherEffect()
        {
            // Banish 1 opponent card and 1 our card
            List<ClientCard> targets = new List<ClientCard>();
            targets.AddRange(Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c)));
            targets.AddRange(Enemy.GetSpells().Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c)));

            ClientCard target = null;
            if (targets.Count > 0)
            {
                target = targets.OrderBy(c => {
                    if (c.IsCode(CardId.EternalSoul)) return 0;
                    if (c.IsCode(CardId.DarkMagicalCircle)) return 1;
                    if (c.IsSpell() || c.IsTrap()) return 2;
                    return 3;
                }).First();
            }

            if (target != null)
            {
                ClientCard ourCost = Bot.GetSpells().FirstOrDefault(c => c != null && c.IsCode(CardId.ElfnotesWelcomeHome, CardId.ElfnotesRhapsodiaOfMadness));
                if (ourCost == null)
                {
                    ourCost = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsCode(CardId.MediusThePure, CardId.ElfnotePowerPatron));
                }

                // DO NOT banish Psychic End Punisher itself
                if (ourCost == null)
                {
                    return false;
                }

                if (ourCost != null)
                {
                    AI.SelectCard(ourCost);
                    AI.SelectNextCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool GoldenCloudBeastMalongEffect()
        {
            // If sent to GY: Target face-up card opponent controls; return to hand
            List<ClientCard> targets = new List<ClientCard>();
            targets.AddRange(Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c)));
            targets.AddRange(Enemy.GetSpells().Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c)));

            if (targets.Count > 0)
            {
                ClientCard target = targets.OrderBy(c => {
                    if (c.IsCode(CardId.EternalSoul)) return 0;
                    if (c.IsCode(CardId.DarkMagicalCircle)) return 1;
                    if (c.IsSpell() || c.IsTrap()) return 2;
                    return 3;
                }).First();

                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool PSYFramelordOmegaEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Hand-rip: only in opponent's main phase, or our Main Phase 2
                if (Enemy.Hand.Count > 0)
                {
                    if (Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
                    {
                        return true;
                    }
                    if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main2)
                    {
                        return true;
                    }
                }
                return false;
            }

            if (Card.Location == CardLocation.Grave)
            {
                // GY shuffle: Target opponent's key card to disrupt them
                int[] targetList = {
                    CardId.DarkMagician,
                    71039903 // The White Stone of Ancients
                };
                ClientCard target = Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsCode(targetList));
                if (target == null)
                {
                    target = Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsMonster());
                }
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }

            return false;
        }

        private bool MirrorjadeTheIcebladeDragonEffect()
        {
            // Banish 1 monster on the field by sending 1 Albaz monster from Extra Deck to GY
            // Mirrorjade does not target, so we bypass targetability check (e.g. for Chaos MAX/Azure-Eyes)
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.BlueEyesChaosMAXDragon));
            if (target == null)
            {
                target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.AzureEyesSilverDragon));
            }
            if (target == null)
            {
                target = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).OrderByDescending(c => c.Attack).FirstOrDefault();
            }

            if (target != null)
            {
                AI.SelectCard(new[] {
                    CardId.AlbionTheBrandedDragon,
                    CardId.RindbrummTheStrikingDragon,
                    CardId.SprindTheIrondashDragon
                });
                AI.SelectNextCard(target);
                return true;
            }
            return false;
        }

        private bool AlbionTheBrandedDragonEffect()
        {
            // Fusion summon by banishing materials from hand, field, and/or GY
            return true;
        }

        private bool RindbrummTheStrikingDragonEffect()
        {
            if (Card != null && Card.Location == CardLocation.MonsterZone)
            {
                ClientCard lastChainCard = Util.GetLastChainCard();
                bool isMonsterEffect = false;
                if (lastChainCard != null)
                {
                    if (lastChainCard.Controller == 0)
                        return false;
                    if (lastChainCard.Controller == 1 && lastChainCard.IsMonster())
                        isMonsterEffect = true;
                }
                else
                {
                    if (Duel.LastChainPlayer == 1)
                        isMonsterEffect = true;
                }

                if (isMonsterEffect)
                {
                    // Negate activated Extra Deck monster effect and bounce
                    ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && IsViableEffectTarget(c));
                    if (target == null)
                    {
                        target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup());
                    }
                    if (target != null)
                    {
                        AI.SelectCard(target);
                    }
                    return true;
                }
                return false;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Special summon Albaz or this card
                AI.SelectCard(new[] {
                    CardId.FallenOfAlbaz,
                    CardId.RindbrummTheStrikingDragon
                });
                return true;
            }
            return true;
        }

        private bool FidraulisHarmoniaEffect()
        {
            if (_harmoniaUsed) return false;
            _harmoniaUsed = true;

            // Reveal 5 Synchro monsters
            AI.SelectCard(new[] {
                CardId.GoldenCloudBeastMalong,
                CardId.DespianLuluwalilith,
                CardId.ElfnoteSeraphimStrelitzia,
                CardId.EcclesiaAndTheDarkDragon,
                CardId.BaronneDeFleur,
                CardId.PSYFramelordOmega,
                CardId.PsychicEndPunisher,
                CardId.JunoraThePowerPatronOfTuning,
                CardId.BlackRoseDragon
            });
            
            // If 3+ revealed, send 1 to GY:
            // Send Malong to bounce if there are cards to target, otherwise Luluwalilith for End Phase Ecclesia summon.
            if (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0)
            {
                AI.SelectNextCard(CardId.GoldenCloudBeastMalong, CardId.DespianLuluwalilith);
            }
            else
            {
                AI.SelectNextCard(CardId.DespianLuluwalilith, CardId.GoldenCloudBeastMalong);
            }

            // Queue the 3rd selection (destruction target) if we reveal 5 Synchros
            if (Enemy.GetMonsterCount() > 0)
            {
                ClientCard destroyTarget = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).OrderByDescending(c => c.Attack).FirstOrDefault();
                if (destroyTarget != null)
                {
                    AI.SelectNextCard(destroyTarget);
                }
            }

            return true;
        }

        private bool BlackRoseDragonSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            // Prevent self-board wipe: do not summon if we control other boss monsters
            if (Bot.HasInMonstersZone(CardId.BaronneDeFleur) || 
                Bot.HasInMonstersZone(CardId.MirrorjadeTheIcebladeDragon) ||
                Bot.HasInMonstersZone(CardId.DespianLuluwalilith))
            {
                return false;
            }

            int ourCards = Bot.GetMonsterCount() + Bot.GetSpellCount();
            int enemyCards = Enemy.GetMonsterCount() + Enemy.GetSpellCount();
            
            // Only summon if opponent has established a board of at least 3 cards and we are behind
            return enemyCards > ourCards && enemyCards >= 3;
        }

        private bool BystialMagnamhutSpSummon()
        {
            return true;
        }

        private bool BystialMagnamhutEffect()
        {
            if (Card != null && Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(CardId.BystialMagnamhut);
                return true;
            }
            return true;
        }
        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.IsCode(CardId.MirrorjadeTheIcebladeDragon) ||
                   card.IsCode(CardId.BaronneDeFleur) ||
                   card.IsCode(CardId.DespianLuluwalilith) ||
                   card.IsCode(CardId.ElfnoteSeraphimStrelitzia) ||
                   card.IsCode(CardId.PsychicEndPunisher) ||
                   card.IsCode(CardId.PSYFramelordOmega) ||
                   card.IsCode(CardId.FallenOfTheWhiteDragon) ||
                   card.IsCode(CardId.IncredibleEcclesiaTheVirtuous) ||
                   card.IsCode(CardId.FallenOfAlbaz);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.IsCode(CardId.AshBlossom, CardId.MaxxC, CardId.EffectVeiler, CardId.DrollAndLockBird, CardId.GhostBelle, CardId.GhostOgre))
                return 800;
            if (c.IsCode(CardId.ElfnoteLucina, CardId.ElfnoteTinia, CardId.ElfnoteRegina, CardId.ElfnoteFortuna, CardId.ElfnotePowerPatron))
                return 100;
            return 200;
        }

        public override int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
            if (location == CardLocation.MonsterZone)
            {
                // If it is Regina or one of our Boss Monsters, we want it in the Center Zone (Zone 2)
                if (cardId == CardId.ElfnoteRegina || BossMonsters.Contains((int)cardId))
                {
                    int centerZone = 1 << 2;
                    if ((available & centerZone) > 0)
                        return centerZone;
                }
                else
                {
                    // For other monsters, avoid the Center Zone (Zone 2) if possible
                    int nonCenterAvailable = available & ~(1 << 2);
                    if (nonCenterAvailable > 0)
                        return nonCenterAvailable;
                }
            }
            return base.OnSelectPlace(cardId, player, location, available);
        }

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0)
                return 0;

            for (int i = 0; i < options.Count; i++)
            {
                long cardId = options[i] >> 20;
                if (cardId == 0 && Card != null)
                {
                    cardId = Card.Id;
                }
                long optIndex = options[i] & 0xfffff;
                
                // Medius the Pure: Option 0: Add to Hand, Option 1: Special Summon.
                // We prefer Special Summon if we have space on the field.
                if (cardId == CardId.MediusThePure && optIndex == 1)
                {
                    if (Bot.GetMonsterCount() < 5)
                    {
                        return i;
                    }
                }
            }

            for (int i = 0; i < options.Count; i++)
            {
                long cardId = options[i] >> 20;
                if (cardId == 0 && Card != null)
                {
                    cardId = Card.Id;
                }
                long optIndex = options[i] & 0xfffff;

                // The Fallen and the Virtuous:
                // Option 0: Send Albaz monster from Extra Deck to GY, destroy 1 card
                // Option 1: Special Summon 1 monster from GY
                if (cardId == CardId.TheFallenAndTheVirtuous)
                {
                    bool hasEnemyCard = Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
                    if (hasEnemyCard && optIndex == 0)
                    {
                        return i; // Destroy
                    }
                    if (!hasEnemyCard && optIndex == 1)
                    {
                        return i; // Special Summon from GY
                    }
                }
            }

            return 0;
        }

        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain()) return false;
            ClientCard lastChainCard = Util.GetLastChainCard();
            if (lastChainCard != null)
            {
                if (lastChainCard.Controller == 0)
                    return false;
            }
            else
            {
                if (Duel.LastChainPlayer != 1)
                    return false;
            }

            int[] ignoreList = {
                CardId.CalledByTheGrave,
                CardId.AshBlossom
            };
            if (lastChainCard != null && lastChainCard.IsCode(ignoreList))
                return false;

            return true;
        }

        private bool GhostBelleEffect()
        {
            if (!SmartHandTrapChain()) return false;
            ClientCard lastChainCard = Util.GetLastChainCard();
            if (lastChainCard != null)
            {
                if (lastChainCard.Controller == 0)
                    return false;
            }
            else
            {
                if (Duel.LastChainPlayer != 1)
                    return false;
            }
            return true;
        }

        private bool GhostOgreEffect()
        {
            if (!SmartHandTrapChain()) return false;
            ClientCard lastChainCard = Util.GetLastChainCard();
            if (lastChainCard != null)
            {
                if (lastChainCard.Controller == 0)
                    return false;
                if (lastChainCard.IsDisabled())
                    return false;
            }
            else
            {
                if (Duel.LastChainPlayer != 1)
                    return false;
            }
            return true;
        }

        private bool CalledByTheGraveEffect()
        {
            ClientCard lastChainCard = Util.GetLastChainCard();
            if (lastChainCard != null && lastChainCard.Controller == 1)
            {
                int[] reactiveBanishWhitelist = {
                    CardId.MaxxC,
                    CardId.AshBlossom,
                    CardId.GhostBelle,
                    CardId.GhostOgre,
                    CardId.EffectVeiler,
                    CardId.DrollAndLockBird,
                    CardId.MulcharmyFuwalos,
                    CardId.MulcharmyPurulia,
                    71039903, // The White Stone of Ancients
                    CardId.DarkMagician,
                    CardId.DarkMagicianTheDragonKnight,
                    CardId.AlternativeWhiteDragon,
                    CardId.BlueEyesChaosMAXDragon,
                    40908371, // Azure-Eyes Silver Dragon
                    63767246  // Number 38
                };

                if (lastChainCard.IsMonster() && reactiveBanishWhitelist.Contains(lastChainCard.Id))
                {
                    ClientCard target = Enemy.Graveyard.FirstOrDefault(c => c != null && c.Id == lastChainCard.Id);
                    if (target != null)
                    {
                        AI.SelectCard(target);
                        return true;
                    }
                }
                
                // Banish Dark Magician in response to Eternal Soul or Navigation
                if (lastChainCard.IsTrap() && (lastChainCard.IsCode(CardId.EternalSoul) || lastChainCard.IsCode(CardId.MagicianNavigation)))
                {
                    ClientCard dm = Enemy.Graveyard.FirstOrDefault(c => c != null && c.Id == CardId.DarkMagician);
                    if (dm != null)
                    {
                        AI.SelectCard(dm);
                        return true;
                    }
                }
            }

            // Only activate Called by the Grave on our turn as Chain Link 1 to banish a GY threat
            if (Duel.Player == 0 && Duel.CurrentChain.Count == 0)
            {
                int[] targetList =
                {
                    71039903, // WhiteStoneOfAncients
                    CardId.DarkMagician,
                    CardId.MaxxC,
                    CardId.AshBlossom,
                    CardId.EffectVeiler,
                    CardId.GhostBelle,
                    CardId.GhostOgre,
                    CardId.DrollAndLockBird,
                    CardId.MulcharmyFuwalos,
                    CardId.MulcharmyPurulia,
                    59822133, // BlueEyesSpiritDragon
                    7084129   // MagiciansRod
                };

                foreach (int id in targetList)
                {
                    ClientCard target = Enemy.Graveyard.FirstOrDefault(c => c != null && c.Id == id);
                    if (target != null)
                    {
                        AI.SelectCard(target);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool MulcharmyEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }



        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            int[] lowStatMonsters = {
                CardId.FidraulisHarmonia,
                CardId.ElfnotePowerPatron,
                CardId.ElfnoteLucina,
                CardId.ElfnoteTinia,
                CardId.ElfnoteFortuna,
                CardId.ElfnoteRegina,
                CardId.PowerPatronShadowSpiritJunordo,
                CardId.MediusThePure,
                CardId.AshBlossom,
                CardId.MaxxC,
                CardId.DrollAndLockBird,
                CardId.EffectVeiler,
                CardId.MulcharmyFuwalos,
                CardId.MulcharmyPurulia
            };

            if (lowStatMonsters.Contains(cardId) && positions.Contains(CardPosition.FaceUpDefence))
            {
                return CardPosition.FaceUpDefence;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Fidraulis Harmonia GY send selection (hint 504)
            if (Card != null && Card.Id == CardId.FidraulisHarmonia && hint == 504)
            {
                bool hasEnemyCards = Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
                if (hasEnemyCards)
                {
                    var malong = cards.FirstOrDefault(c => c != null && c.Id == CardId.GoldenCloudBeastMalong);
                    if (malong != null) return new[] { malong };
                }
                var lulu = cards.FirstOrDefault(c => c != null && c.Id == CardId.DespianLuluwalilith);
                if (lulu != null) return new[] { lulu };
            }

            // 1. Link Material Selection โ€” Protect Ace Cards (hint 533)
            if (hint == 533)
            {
                var safe = cards.Where(c => c != null && !IsAceCard(c)).ToList();
                if (safe.Count >= min) return safe.Take(max).ToList();
            }

            // 2. Destruction target selection (hint 502)
            if (hint == 502)
            {
                var sorted = cards.OrderByDescending(c => {
                    if (c == null) return -999;
                    int score = (c.Controller == 1) ? 10000 : 0;
                    if (c.IsMonster())
                    {
                        if (c.IsFaceup() && !c.IsDisabled())
                        {
                            if (c.Attack >= 2500 && c.HasType(CardType.Effect)) return score + 5000;
                        }
                        return score + c.Attack;
                    }
                    else if (c.IsSpell() || c.IsTrap())
                    {
                        if (c.IsFaceup()) return score + 2000;
                        return score + 100;
                    }
                    return score;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // 3. Special Summon / Revival selection (hint 509)
            if (hint == 509)
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    int controllerScore = (c.Controller == 0) ? 0 : 1000;
                    int bossScore = IsAceCard(c) ? 100 : 0;
                    return controllerScore + bossScore;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // 4. Attack target selection (hint 549)
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

            // 5. Bystial Magnamhut summon cost (or other banish costs)
            if (Card != null && (Card.Id == CardId.BystialMagnamhut || Card.Id == CardId.FallenOfTheWhiteDragon))
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (c.Controller == 1) return 1; // Banish opponent's cards first
                    if (c.Location == CardLocation.Grave)
                    {
                        if (IsAceCard(c)) return 100;
                        if (c.Id == CardId.FallenOfAlbaz || c.Id == CardId.IncredibleEcclesiaTheVirtuous) return 80;
                        return 10; // Discard/banish non-essential GY cards
                    }
                    return 200;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // Protect our Ace cards from generic selections
            if (cards.Any(c => c != null && c.Controller == 0 && c.Location == CardLocation.MonsterZone && IsAceCard(c)))
            {
                var safeCards = cards.Where(c => c == null || c.Controller != 0 || c.Location != CardLocation.MonsterZone || !IsAceCard(c)).ToList();
                if (safeCards.Count >= min)
                    return safeCards.Take(max).ToList();
            }

            // 6. Negate target selection (hint 575 = HINTMSG_NEGATE)
            if (hint == 575)
            {
                var opponentCards = cards.Where(c => c != null && c.Controller == 1 && c.IsFaceup() && !c.IsDisabled() && !IsUnaffectedByEffects(c)).ToList();
                if (opponentCards.Count > 0)
                {
                    var sorted = opponentCards.OrderBy(c => {
                        if (c.IsCode(CardId.EternalSoul)) return 0;
                        if (c.IsCode(CardId.DarkMagicalCircle)) return 1;
                        if (c.IsCode(CardId.Number38HopeHarbinger)) return 2;
                        if (c.IsCode(CardId.DarkMagicianTheDragonKnight)) return 3;
                        if (c.IsCode(CardId.AzureEyesSilverDragon)) return 4;
                        if (c.IsCode(CardId.BlueEyesChaosMAXDragon)) return 5;
                        if (c.IsSpell() || c.IsTrap()) return 6;
                        return 7;
                    }).ToList();
                    return sorted.Take(max).ToList();
                }

                var ourCards = cards.Where(c => c != null && c.Controller == 0 && c.IsFaceup()).ToList();
                if (ourCards.Count > 0)
                {
                    var sortedOur = ourCards.OrderBy(c => {
                        if (c.IsSpell() || c.IsTrap()) return 0;
                        if (c.IsCode(CardId.ElfnotePowerPatron, CardId.MediusThePure)) return 1;
                        return 2;
                    }).ToList();
                    return sortedOur.Take(max).ToList();
                }
            }

            // 7. Bounce target selection (hint 505 = HINTMSG_RTOHAND)
            if (hint == 505)
            {
                var opponentMonsters = cards.Where(c => c != null && c.Controller == 1 && c.IsFaceup() && !IsUnaffectedByEffects(c)).ToList();
                if (opponentMonsters.Count > 0)
                {
                    var sorted = opponentMonsters.OrderByDescending(c => {
                        if (c.IsCode(CardId.BlueEyesChaosMAXDragon)) return 9999;
                        if (c.IsCode(CardId.DarkMagicianTheDragonKnight)) return 9000;
                        if (c.IsCode(CardId.AzureEyesSilverDragon)) return 8000;
                        return c.Attack;
                    }).ToList();
                    return sorted.Take(max).ToList();
                }

                var ourMonsters = cards.Where(c => c != null && c.Controller == 0 && c.IsFaceup()).ToList();
                if (ourMonsters.Count > 0)
                {
                    var sortedOur = ourMonsters.OrderBy(c => {
                        if (c.IsCode(CardId.IncredibleEcclesiaTheVirtuous)) return 0;
                        if (c.IsCode(CardId.FallenOfAlbaz)) return 1;
                        if (c.IsCode(CardId.MediusThePure, CardId.ElfnotePowerPatron)) return 2;
                        if (IsAceCard(c)) return 100;
                        return 10;
                    }).ToList();
                    return sortedOur.Take(max).ToList();
                }
            }

            if (Card == null) return base.OnSelectCard(cards, min, max, hint, cancelable);

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override IList<ClientCard> OnSelectFusionMaterial(IList<ClientCard> cards, int min, int max)
        {
            var sorted = cards.OrderBy(c => {
                if (c == null) return 999;
                if (c.Controller == 0 && IsAceCard(c))
                {
                    return 10000;
                }
                if (c.Controller == 1)
                {
                    if (c.Location == CardLocation.MonsterZone) return 10;
                    return 20;
                }
                if (c.Location == CardLocation.Grave)
                {
                    if (c.Id == CardId.FallenOfAlbaz) return 30;
                    return 40;
                }
                if (c.Location == CardLocation.Hand)
                {
                    if (c.IsMonster()) return 50;
                    return 60;
                }
                return 100;
            }).ToList();
            
            return sorted.Take(max).ToList();
        }

        public override IList<ClientCard> OnSelectSynchroMaterial(IList<ClientCard> cards, int sum, int min, int max)
        {
            var sorted = cards.OrderBy(c => {
                if (c == null) return 999;
                if (c.Controller == 0 && IsAceCard(c))
                {
                    return 10000;
                }
                if (c.Id == CardId.ElfnotePowerPatron) return 10;
                if (c.IsCode(ElfnoteCards)) return 20;
                return 100;
            }).ToList();
            
            return sorted.Take(max).ToList();
        }

        private bool ShouldHoldAttack(ClientCard attacker, ClientCard defender)
        {
            if (attacker == null) return false;

            // If we have PEP on field and PEP's effect immunity is active (our LP <= opponent's LP)
            if (Bot.HasInMonstersZone(CardId.PsychicEndPunisher) && Bot.LifePoints <= Enemy.LifePoints)
            {
                // If we can OTK, don't hold back!
                if (CanOTK()) return false;

                // Calculate damage from this attack
                int damage = 0;
                if (defender == null)
                {
                    damage = attacker.Attack;
                }
                else if (defender.IsAttack())
                {
                    damage = Math.Max(0, attacker.Attack - defender.Attack);
                }

                // If this attack would make opponent's LP lower than ours (breaking PEP's immunity)
                if (Enemy.LifePoints - damage < Bot.LifePoints)
                {
                    // If defender is a threat we are destroying by battle, it is fine to proceed
                    if (defender != null && (defender.Attack >= 2500 || 
                        defender.IsCode(CardId.BlueEyesChaosMAXDragon, CardId.DarkMagicianTheDragonKnight, CardId.AlternativeWhiteDragon)))
                    {
                        return false;
                    }
                    
                    // Otherwise, hold the attack to preserve PEP's immunity!
                    return true;
                }
            }
            return false;
        }

        public override bool OnPreBattleBetween(ClientCard attacker, ClientCard defender)
        {
            if (ShouldHoldAttack(attacker, defender))
            {
                return false;
            }
            return base.OnPreBattleBetween(attacker, defender);
        }

        protected override bool OpponentHasActiveNegator(ClientCard ourCard = null)
        {
            foreach (var c in Enemy.GetMonsters())
            {
                if (c == null || !c.IsFaceup() || c.IsDisabled()) continue;

                // Crystal Wing Synchro Dragon (only negates level 5+ monster effects on the field)
                if (c.IsCode(50954680) && ourCard != null && ourCard.Location == CardLocation.MonsterZone && ourCard.Level >= 5 && ourCard.IsMonster())
                {
                    return true;
                }

                // Baronne de Fleur (negates any activation)
                if (c.IsCode(CardId.BaronneDeFleur))
                {
                    return true;
                }

                // Apollousa, Bow of the Goddess (negates any monster effect activation)
                if (c.IsCode(42815418) && c.Attack >= 800 && ourCard != null && ourCard.IsMonster())
                {
                    return true;
                }
            }
            return false;
        }

        public override bool OnSelectYesNo(long desc)
        {
            // Despian Luluwalilith negation prompt: (53971455 << 4) + 2 = 863543282
            if (desc == ((long)CardId.DespianLuluwalilith << 4) + 2)
            {
                // Only negate if opponent has a negatable face-up card on field
                bool opponentHasNegatable = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled() && !IsUnaffectedByEffects(c)) ||
                                            Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && !c.IsDisabled());
                return opponentHasNegatable;
            }
            return base.OnSelectYesNo(desc);
        }

        private bool IsUnaffectedByEffects(ClientCard card)
        {
            if (card == null) return false;
            bool isEternalSoulActive = Enemy.GetSpells().Any(s => s != null && s.IsFaceup() && s.IsCode(CardId.EternalSoul) && !s.IsDisabled());
            if (isEternalSoulActive && (card.IsCode(CardId.DarkMagician) || card.IsCode(CardId.DarkMagicianTheDragonKnight)))
            {
                return true;
            }
            return false;
        }

        private bool DespianLuluwalilithEffect()
        {
            return true;
        }

        private bool JunoraEffect()
        {
            return true;
        }

        private bool TheDragonThatDevoursTheDogmaEffect()
        {
            return Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2);
        }

        private bool SprindEffect()
        {
            if (Card == null) return false;
            if (Card.Location != CardLocation.MonsterZone) return true;

            int botCol = GetCardColumn(Card);
            if (botCol < 0) return false;

            return Enemy.GetMonsters().Concat(Enemy.GetSpells())
                .Any(c => c != null && c.IsFaceup() && GetCardColumn(c) == botCol);
        }

        // Map a card's Sequence to a physical column index (0..4) from Bot's perspective.
        // Returns -1 for cards outside Monster/Spell zones (or unsupported sequences).
        private static int GetCardColumn(ClientCard c)
        {
            if (c == null) return -1;
            int seq = c.Sequence;
            bool isBot = c.Controller == 0;
            if (c.Location == CardLocation.MonsterZone)
            {
                if (seq >= 0 && seq <= 4) return isBot ? seq : (4 - seq);
                if (seq == 5) return isBot ? 1 : 3; // Extra Monster Zone (left from owner's POV)
                if (seq == 6) return isBot ? 3 : 1; // Extra Monster Zone (right from owner's POV)
            }
            else if (c.Location == CardLocation.SpellZone)
            {
                if (seq >= 0 && seq <= 4) return isBot ? seq : (4 - seq);
            }
            return -1;
        }
    }
}
