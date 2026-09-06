// =========================================================================================
// CARD AUDIT โ€” 2026_AncientG
// | Card Name                | Type            | OPT? | Cost            | Effect                                                     | Activate When                             | NEVER When                        |
// | :----------------------- | :-------------: | :--: | :-------------: | :--------------------------------------------------------- | :---------------------------------------- | :-------------------------------- |
// | Geartown                 | Field           | No   | None            | Special Summons "Ancient Gear" from Deck/Hand/GY on destroy | Destructive pop available (MST/Storm)     | Replacement pop / Chain Link 2+   |
// | Skill Drain              | Continuous Trap | No   | 1000 LP         | Negates all face-up monster effects on field               | Enemy combos / we have beatsticks        | LP <= 1000                         |
// | Beast King Barbaros      | Monster         | No   | None            | Summon without tribute (1900 ATK). 3-tribute wipes field  | Normal summon under Skill Drain / Chalice | Opponent high ATK without negate  |
// | Fusilier Dragon          | Monster         | No   | None            | Summon without tribute (1400 ATK). Full 2800 ATK under SD | Normal summon under Skill Drain / Chalice | No negate setup                   |
// | Machina Fortress         | Monster         | No   | Discard Lvl 8+  | SS from Hand/GY. Pops enemy card when destroyed by battle  | Machine discard available                 | Discarding core combo pieces      |
// | Machina Gearframe        | Monster         | OPT  | None            | Searches Machina monster on Normal Summon / equips to M.F. | Main Phase to start Machina engine        | SS/Effect blocked                 |
// | Ancient Gear Gadjiltron  | Monster         | No   | Tribute         | 3000 ATK Boss. Stops opponent S/T during battle            | SS via Geartown / Tribute under Geartown  | Under SS block                    |
// | Mystical Space Typhoon   | Quick-Play      | No   | None            | Destroys target S/T card                                  | Destroy own Geartown / enemy backrow      | Chain Link 2+ on Geartown         |
// | Forbidden Chalice        | Quick-Play      | No   | None            | Negates monster effect, +400 ATK                          | Chain negate enemy / boost own beatsticks | Negating own active boss effects  |
// | Call of the Haunted      | Continuous Trap | No   | None            | SS monster from GY in ATK position                         | Opponent End Phase / Battle Phase block   | Main Phase exposing to board wipe |
// | Trade-In                 | Spell           | No   | Discard Lvl 8   | Draws 2 cards                                              | Dig for Skill Drain/Geartown              | No Lvl 8 targets in hand           |
// | Terraforming             | Spell           | No   | None            | Searches Field Spell from Deck                             | Main Phase to fetch Geartown              | Lethal confirmed / Search deferred|
// =========================================================================================
// ACE CARDS: Primary: Ancient Gear Gadjiltron Dragon / Secondary: Machina Fortress, Beast King Barbaros Ur
// COMBO STARTERS: 1. Geartown + MST/Heavy Storm  2. Machina Gearframe -> Fortress  3. Skill Drain + Barbaros/Fusilier
// CHOKEPOINTS: Geartown destroyed as Chain Link 2+ (misses timing). Skill Drain negated/destroyed.
// =========================================================================================

using System;
using YGOSharp.OCGWrapper.Enums;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_AncientG", "2026_AncientG")]
    public class _2026_AncientGExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck
            public const int MachinaForce = 58054262;
            public const int TradeIn = 38120068;
            public const int MachinaFortress = 5556499;
            public const int LimiterRemoval = 23171610;
            public const int InterdimensionalMatterTransporter = 36261276;
            public const int Terraforming = 73628505;
            public const int CompulsoryEvacuationDevice = 94192409;
            public const int MirrorForce = 44095762;
            public const int MachinaGearframe = 42940404;
            public const int SkillDrain = 82732705;
            public const int Geartown = 37694547;
            public const int CallOfTheHaunted = 97077563;
            public const int BeastMachineKingBarbarosUr = 19028307;
            public const int BeastKingBarbaros = 78651105;
            public const int FutureVisions = 87902575;
            public const int MausoleumOfTheEmperor = 80921533;
            public const int FusilierDragonTheDualModeBeast = 51632798;
            public const int GiantTrunade = 42703248;
            public const int HeavyStorm = 19613556;
            public const int AncientGearGadjiltronDragon = 50933533;
            public const int MysticalSpaceTyphoon = 5318639;
            public const int ForbiddenChalice = 25789292;
            public const int Minefieldriller = 24419823;

            // Extra Deck
            public const int ChimeratechFortressDragon = 79229522;
            public const int RedDragonArchfiend = 70902743;
            public const int ColossalFighter = 23693634;
            public const int ThoughtRulerArchfiend = 70780151;
            public const int StardustDragon = 44508094;
            public const int BlackRoseDragon = 73580471;
            public const int XSaberUrbellum = 80108118;
            public const int GoyoGuardian = 7391448;
            public const int GaiaKnightTheForceOfEarth = 97204936;
            public const int IronChainDragon = 19974580;
            public const int BrionacDragonOfTheIceBarrier = 50321796;
            public const int FlamvellUruquizas = 53714009;
            public const int MagicalAndroid = 43385557;
            public const int AllyOfJusticeCatastor = 26593852;

            // Side
            public const int CyberDragon = 70095154;
            public const int DDCrow = 24508238;
            public const int BrainControl = 87910978;
            public const int SolemnJudgment = 41420027;
            public const int LightImprisoningMirror = 53341729;
            public const int ShadowImprisoningMirror = 99735427;
            public const int PhoenixWingWindBlast = 63356631;
            public const int DustTornado = 60082869;
            public const int BottomlessTrapHole = 29401950;
            public const int StarlightRoad = 58120309;

            // Altergeist cards
            public const int Hexstia = 1508649;
            public const int Silquitous = 89538537;
            public const int Multifaker = 42790071;
            public const int Marionetter = 53143898;
            public const int Meluseek = 25533642;

            // Negators & Key Matchup Cards
            public const int Number38 = 63767246;
            public const int BaronneDeFleur = 84815190;
            public const int BorreloadSavageDragon = 57793869;
            public const int CyberDragonInfinity = 10443957;
            public const int CrystalWingSynchroDragon = 50954680;
            public const int EvolzarDolkka = 31801517;
            public const int AzureEyesSilverDragon = 40908371;
            public const int BlueEyesSpiritDragon = 59822133;
            public const int AlternativeWhiteDragon = 38517737;
            public const int DragonSpiritOfWhite = 45467446;
            public const int BlueEyesWhiteDragon = 89631139;
            public const int AltergeistProtocol = 27541563;
            public const int AltergeistSpoofing = 53936268;
            public const int AltergeistManifestation = 35146019;
            public const int SecretVillage = 68462976;
        }

        private static readonly int[] BossMonsters = {
            CardId.AncientGearGadjiltronDragon,
            CardId.BeastMachineKingBarbarosUr,
            CardId.BeastKingBarbaros,
            CardId.MachinaFortress,
            CardId.MachinaForce
        };

        private bool _limiterRemovalUsed = false;
        private bool _heavyStormUsed = false;
        private bool _giantTrunadeUsed = false;
        private bool _normalSummonedThisTurn = false;
        private int _spellsActivatedThisTurn = 0;

        public _2026_AncientGExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            HeuristicGuard.RegisterAceCards(
                CardId.AncientGearGadjiltronDragon,
                CardId.BeastMachineKingBarbarosUr,
                CardId.BeastKingBarbaros,
                CardId.MachinaFortress,
                CardId.MachinaForce
            );
            ResourcePlan.RegisterAceCards(BossMonsters);

            // โ”€โ”€ Combo Router: Sequencing โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Geartown-MST",
                RequiredCards = new List<int> { CardId.Geartown, CardId.MysticalSpaceTyphoon },
                FallbackLineName = "AncientG-SkillDrain-Fallback",
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.Geartown, ActionType = ExecutorType.Activate, Description = "Play Geartown" },
                    new() { CardId = CardId.MysticalSpaceTyphoon, ActionType = ExecutorType.Activate, Description = "MST own Geartown" }
                },
                EndBoardScore = 80
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Geartown-HeavyStorm",
                RequiredCards = new List<int> { CardId.Geartown, CardId.HeavyStorm },
                FallbackLineName = "AncientG-SkillDrain-Fallback",
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.Geartown, ActionType = ExecutorType.Activate, Description = "Play Geartown" },
                    new() { CardId = CardId.HeavyStorm, ActionType = ExecutorType.Activate, Description = "Heavy Storm own Geartown" }
                },
                EndBoardScore = 85
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Machina-Gearframe-Fortress",
                RequiredCards = new List<int> { CardId.MachinaGearframe },
                FallbackLineName = "AncientG-SkillDrain-Fallback",
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.MachinaGearframe, ActionType = ExecutorType.Summon, Description = "Summon Gearframe" },
                    new() { CardId = CardId.MachinaGearframe, ActionType = ExecutorType.Activate, Description = "Search Machina Fortress" },
                    new() { CardId = CardId.MachinaFortress, ActionType = ExecutorType.SpSummon, Description = "Special Summon Fortress" }
                },
                EndBoardScore = 75
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "AncientG-SkillDrain-Fallback",
                RequiredCards = new List<int> { CardId.SkillDrain, CardId.BeastKingBarbaros },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.SkillDrain, ActionType = ExecutorType.Activate, Description = "Activate Skill Drain" },
                    new() { CardId = CardId.BeastKingBarbaros, ActionType = ExecutorType.Summon, Description = "Summon Barbaros as 3000 beater" }
                },
                EndBoardScore = 70,
                Condition = () => Bot.Hand.Any(c => c != null && c.Id == CardId.SkillDrain) && Bot.Hand.Any(c => c != null && c.Id == CardId.BeastKingBarbaros)
            });

            // โ”€โ”€ Bait Planner โ”€โ”€
            BaitPlanner.RegisterComboStarters(CardId.Geartown);
            BaitPlanner.RegisterBaitCards(CardId.Terraforming, CardId.MachinaGearframe);

            // โ”€โ”€ Chain Advisor โ”€โ”€
            ChainAdvisor.RegisterHighValueTargets(CardId.Geartown, CardId.SkillDrain);

            // 1. Spells/Traps Removal & Disruption (MST, Heavy Storm, Giant Trunade, Forbidden Chalice)
            AddExecutor(ExecutorType.Activate, CardId.HeavyStorm, HeavyStormEffect);
            AddExecutor(ExecutorType.Activate, CardId.GiantTrunade, GiantTrunadeEffect);
            AddExecutor(ExecutorType.Activate, CardId.MysticalSpaceTyphoon, MstEffect);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenChalice, ForbiddenChaliceEffect);

            // 2. Continuous Traps / Floodgates (Skill Drain)
            AddExecutor(ExecutorType.Activate, CardId.SkillDrain, SkillDrainEffect);

            // 3. Trade-In Draw Power
            AddExecutor(ExecutorType.Activate, CardId.TradeIn, TradeInEffect);

            // 4. Searching Spells (Terraforming)
            AddExecutor(ExecutorType.Activate, CardId.Terraforming, TerraformingEffect);

            // 5. Field Spells activation (Geartown destruction combo is handled here)
            AddExecutor(ExecutorType.Activate, CardId.Geartown, GeartownEffect);
            AddExecutor(ExecutorType.Activate, CardId.FutureVisions, FutureVisionsEffect);
            AddExecutor(ExecutorType.Activate, CardId.MausoleumOfTheEmperor, MausoleumEffect);

            // 6. Monster Trigger & Ignition Effects
            AddExecutor(ExecutorType.Activate, CardId.MachinaGearframe, MachinaGearframeEffect);
            AddExecutor(ExecutorType.Activate, CardId.Minefieldriller, MinefieldrillerEffect);
            AddExecutor(ExecutorType.Activate, CardId.AncientGearGadjiltronDragon, GadjiltronEffect);

            // 7. Special Summons (Fortress, Barbaros Ur, Contact Fusion Chimeratech)
            AddExecutor(ExecutorType.SpSummon, CardId.MachinaFortress, MachinaFortressSummon);
            AddExecutor(ExecutorType.Activate, CardId.MachinaFortress, MachinaFortressEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.BeastMachineKingBarbarosUr, BarbarosUrSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ChimeratechFortressDragon, ChimeratechSummon);

            // 8. Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.MachinaGearframe, GearframeSummon);
            AddExecutor(ExecutorType.Summon, CardId.BeastKingBarbaros, BarbarosSummon);
            AddExecutor(ExecutorType.Summon, CardId.FusilierDragonTheDualModeBeast, FusilierSummon);
            AddExecutor(ExecutorType.Summon, CardId.Minefieldriller);
            AddExecutor(ExecutorType.Summon, CardId.AncientGearGadjiltronDragon, GadjiltronTributeSummon);

            // 9. Reborn / Special Summon Traps
            AddExecutor(ExecutorType.Activate, CardId.CallOfTheHaunted, CallOfTheHauntedEffect);

            // 10. Combat Spells (Limiter Removal)
            AddExecutor(ExecutorType.Activate, CardId.LimiterRemoval, LimiterRemovalEffect);

            // 11. Defensive Traps (Mirror Force, Compulsory, Interdimensional)
            AddExecutor(ExecutorType.Activate, CardId.MirrorForce, MirrorForceEffect);
            AddExecutor(ExecutorType.Activate, CardId.CompulsoryEvacuationDevice, CompulsoryEvacuationDeviceEffect);
            AddExecutor(ExecutorType.Activate, CardId.InterdimensionalMatterTransporter, InterdimensionalMatterTransporterEffect);
            AddExecutor(ExecutorType.Activate, CardId.BottomlessTrapHole, DefaultUniqueTrap);
            AddExecutor(ExecutorType.Activate, CardId.StarlightRoad, DefaultTrap);
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, DefaultSolemnJudgment);

            // 12. Set Traps (MP2/Deferrable)
            AddExecutor(ExecutorType.SpellSet, CardId.SkillDrain);
            AddExecutor(ExecutorType.SpellSet, CardId.MirrorForce);
            AddExecutor(ExecutorType.SpellSet, CardId.CompulsoryEvacuationDevice);
            AddExecutor(ExecutorType.SpellSet, CardId.InterdimensionalMatterTransporter);
            AddExecutor(ExecutorType.SpellSet, CardId.CallOfTheHaunted);
            AddExecutor(ExecutorType.SpellSet, CardId.MysticalSpaceTyphoon);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenChalice);
            AddExecutor(ExecutorType.SpellSet, CardId.BottomlessTrapHole);
            AddExecutor(ExecutorType.SpellSet, CardId.StarlightRoad);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnJudgment);

            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // Beatdown deck with Skill Drain / Geartown combo -> prefer going first to set up
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _limiterRemovalUsed = false;
            _heavyStormUsed = false;
            _giantTrunadeUsed = false;
            _normalSummonedThisTurn = false;
            _spellsActivatedThisTurn = 0;

            // Clean up expired or missing Azure-Eyes references
            var activeAzureEyes = Enemy.GetMonsters().Concat(Bot.GetMonsters())
                .Where(m => m != null && m.IsFaceup() && m.IsCode(CardId.AzureEyesSilverDragon))
                .ToHashSet();
            var keysToRemove = _azureEyesSummonTurns.Keys.Where(k => !activeAzureEyes.Contains(k)).ToList();
            foreach (var k in keysToRemove)
            {
                _azureEyesSummonTurns.Remove(k);
            }

            // โ”€โ”€ Going-Second BreakBoard: prioritize disruption over combo โ”€โ”€
            if (ShouldGoBreakBoard)
            {
                _heavyStormUsed = false;
                _giantTrunadeUsed = false;
            }
        }

        public override MainPhaseAction OnSelectIdleCmd(MainPhase main)
        {
            var action = base.OnSelectIdleCmd(main);
            if (action != null && action.Action == MainPhaseAction.MainAction.Summon)
            {
                _normalSummonedThisTurn = true;
            }
            return action;
        }

        // --- Core Combos and Activations ---

        private bool OpponentHasSpellNegator()
        {
            foreach (var c in Enemy.GetMonsters())
            {
                if (c == null || !c.IsFaceup() || c.IsDisabled()) continue;
                if (c.IsCode(CardId.Number38, CardId.BaronneDeFleur, CardId.BorreloadSavageDragon,
                             CardId.CyberDragonInfinity, CardId.Hexstia, 17330115))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsSkillDrainActive()
        {
            return Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.SkillDrain) && !c.IsDisabled())
                || Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.SkillDrain) && !c.IsDisabled());
        }

        private bool IsMonsterEffectNegated(ClientCard monster)
        {
            if (monster == null) return false;
            if (monster.IsDisabled()) return true;
            if (monster.Location == CardLocation.MonsterZone && monster.IsFaceup() && IsSkillDrainActive()) return true;
            return false;
        }

        private bool OpponentHasGyOrGenericNegator()
        {
            foreach (var c in Enemy.GetMonsters())
            {
                if (c == null || !c.IsFaceup() || c.IsDisabled()) continue;
                if (c.IsCode(CardId.BlueEyesSpiritDragon, CardId.BaronneDeFleur, 
                             CardId.BorreloadSavageDragon, CardId.CyberDragonInfinity))
                {
                    return true;
                }
            }
            return false;
        }

        private bool HeavyStormEffect()
        {
            if (_heavyStormUsed) return false;
            if (OpponentHasSpellNegator() && _spellsActivatedThisTurn == 0) return false;

            // 1. Skill Drain Guard: Don't blow up our own active/set Skill Drain unless we can deal lethal
            bool skillDrainActive = Bot.GetSpells().Any(c => c != null && c.IsCode(CardId.SkillDrain));
            if (skillDrainActive && !CanDealLethal())
                return false;

            int enemyST = Enemy.GetSpellCount();
            int ourST = Bot.GetSpellCountWithoutField();
            bool geartownOnField = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.Geartown));

            // Verify Gadjiltron Dragon target exists in Deck, Hand, or GY
            bool targetExists = Bot.GetRemainingCount(CardId.AncientGearGadjiltronDragon, 3) > 0
                || Bot.Hand.Any(c => c != null && c.IsCode(CardId.AncientGearGadjiltronDragon))
                || Bot.Graveyard.Any(c => c != null && c.IsCode(CardId.AncientGearGadjiltronDragon));

            if (geartownOnField)
            {
                if (targetExists && !OpponentHasGyOrGenericNegator())
                {
                    // If we have another Field Spell in hand to overwrite Geartown for free, 
                    // and the opponent has no backrow, do NOT use Heavy Storm.
                    bool canOverwrite = Bot.HasInHand(CardId.Geartown)
                        || Bot.HasInHand(CardId.FutureVisions)
                        || Bot.HasInHand(CardId.MausoleumOfTheEmperor);

                    if (canOverwrite && enemyST == 0)
                        return false;

                    // If we have valuable set traps and the opponent has no backrow, do NOT use Heavy Storm.
                    if (enemyST == 0 && ourST > 1)
                        return false;

                    _heavyStormUsed = true;
                    _spellsActivatedThisTurn++;
                    return true;
                }
            }
            else
            {
                // DEFER: If we have Geartown in hand (or can search it) and a Gadjiltron is available, wait for Geartown to be activated first!
                bool canSearchGeartown = Bot.HasInHand(CardId.Terraforming) && Bot.GetRemainingCount(CardId.Geartown, 3) > 0;
                bool hasOrCanSearchGeartown = Bot.HasInHand(CardId.Geartown) || canSearchGeartown;
                if (hasOrCanSearchGeartown && targetExists)
                {
                    return false;
                }

                // If no Geartown, only use Heavy Storm to clear opponent's backrow
                // It's worth it if:
                // - Opponent has 2+ S/T cards
                // - Opponent has 1 S/T card and we have 0 set S/T cards
                // - Opponent has a face-up floodgate / continuous S/T that is hindering us
                bool opponentHasFloodgate = Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && 
                    (c.IsFloodgate() || c.HasType(CardType.Continuous) || c.HasType(CardType.Field)));

                if (enemyST >= 2 || (enemyST >= 1 && ourST == 0) || opponentHasFloodgate)
                {
                    _heavyStormUsed = true;
                    _spellsActivatedThisTurn++;
                    return true;
                }
            }

            return false;
        }

        private bool GiantTrunadeEffect()
        {
            if (_giantTrunadeUsed) return false;
            if (Duel.Phase != DuelPhase.Main1) return false;

            if (Enemy.GetSpellCount() >= 1)
            {
                _giantTrunadeUsed = true;
                _spellsActivatedThisTurn++;
                return true;
            }
            return false;
        }

        private bool MstEffect()
        {
            if (Duel.CurrentChain.Count > 0)
            {
                if (Duel.LastChainPlayer == 1)
                {
                    var lastCard = Util.GetLastChainCard();
                    if (lastCard != null && lastCard.Controller == 1 && (lastCard.IsSpell() || lastCard.IsTrap()) &&
                        (lastCard.HasType(CardType.Continuous) || lastCard.HasType(CardType.Field) || lastCard.HasType(CardType.Equip)) &&
                        IsViableEffectTarget(lastCard))
                    {
                        _spellsActivatedThisTurn++;
                        AI.SelectCard(lastCard);
                        return true;
                    }
                }
                return false;
            }

            // 2. Destroy face-up continuous threat cards of the opponent immediately
            var faceupThreat = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && 
                (c.IsFloodgate() || c.HasType(CardType.Continuous) || c.HasType(CardType.Field) ||
                 c.IsCode(CardId.AltergeistSpoofing, CardId.AltergeistProtocol, CardId.SecretVillage, CardId.AltergeistManifestation)) &&
                IsViableEffectTarget(c));
            if (faceupThreat != null)
            {
                _spellsActivatedThisTurn++;
                AI.SelectCard(faceupThreat);
                return true;
            }

            // 3. Destroy our own face-up Geartown to summon Gadjiltron Dragon (Chain Link 1 only)
            bool geartownOnField = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.Geartown));
            bool gadjiltronAvailable = Bot.GetRemainingCount(CardId.AncientGearGadjiltronDragon, 3) > 0
                || Bot.Hand.Any(c => c != null && c.IsCode(CardId.AncientGearGadjiltronDragon))
                || Bot.Graveyard.Any(c => c != null && c.IsCode(CardId.AncientGearGadjiltronDragon));

            if (geartownOnField && gadjiltronAvailable && Duel.CurrentChain.Count == 0 && !OpponentHasGyOrGenericNegator())
            {
                if (OpponentHasSpellNegator() && _spellsActivatedThisTurn == 0)
                {
                    // Block destroying own Geartown if it would get negated
                }
                else
                {
                    ClientCard geartown = Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.Geartown));
                    if (geartown != null)
                    {
                        _spellsActivatedThisTurn++;
                        AI.SelectCard(geartown);
                        return true;
                    }
                }
            }

            // 4. Blind-pop opponent backrow in opponent's End Phase
            bool isOpponentEndPhase = (Duel.Player == 1 && Duel.Phase == DuelPhase.End);
            if (isOpponentEndPhase)
            {
                var target = Util.GetBestEnemySpell();
                if (target != null)
                {
                    _spellsActivatedThisTurn++;
                    AI.SelectCard(target);
                    return true;
                }
            }

            // 5. Hold MST if we have Geartown in hand (or can search it) to prepare combo on our turn
            bool canSearchGeartown = Bot.HasInHand(CardId.Terraforming) && Bot.GetRemainingCount(CardId.Geartown, 3) > 0;
            bool hasOrCanSearchGeartown = Bot.HasInHand(CardId.Geartown) || canSearchGeartown;
            if (hasOrCanSearchGeartown && gadjiltronAvailable)
            {
                return false;
            }

            // 6. Otherwise, pop opponent S/T
            var fallbackTarget = Util.GetBestEnemySpell();
            if (fallbackTarget != null)
            {
                _spellsActivatedThisTurn++;
                AI.SelectCard(fallbackTarget);
                return true;
            }

            return false;
        }

        private bool ForbiddenChaliceEffect()
        {
            if (Card.Location != CardLocation.Hand && Card.Location != CardLocation.SpellZone) return false;

            // 1. Negate opponent monster effect on chain
            if (Duel.LastChainPlayer == 1 && Util.GetLastChainCard() != null && Util.GetLastChainCard().Controller == 1 && Util.GetLastChainCard().IsMonster())
            {
                var target = Util.GetLastChainCard();
                if (target != null && IsViableEffectTarget(target) && !IsMonsterEffectNegated(target))
                {
                    _spellsActivatedThisTurn++;
                    AI.SelectCard(target);
                    return true;
                }
            }

            // 2. Proactively negate opponent disruptor monsters in our Main Phase
            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                var target = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && !m.IsDisabled() &&
                    (m.IsCode(CardId.Silquitous, CardId.Hexstia, CardId.Multifaker, 
                             CardId.Number38, CardId.BaronneDeFleur, CardId.BorreloadSavageDragon,
                             CardId.CyberDragonInfinity, CardId.CrystalWingSynchroDragon, CardId.EvolzarDolkka,
                             CardId.BlueEyesSpiritDragon) || 
                      m.IsFloodgate() || m.IsMonsterDangerous()) &&
                    IsViableEffectTarget(m));
                if (target != null)
                {
                    _spellsActivatedThisTurn++;
                    AI.SelectCard(target);
                    return true;
                }
            }

            // 3. Negate our own normal summoned Barbaros/Fusilier to restore/boost their ATK (Main Phase or Battle Phase when attacking/defending)
            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle))
            {
                bool skillDrainActive = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.SkillDrain));
                if (skillDrainActive && Duel.Phase == DuelPhase.Main1)
                    return false;

                var target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() &&
                    ((c.IsCode(CardId.BeastKingBarbaros) && c.Attack == 1900) ||
                     (c.IsCode(CardId.FusilierDragonTheDualModeBeast) && c.Attack == 1400)));

                if (target != null)
                {
                    if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle)
                    {
                        if (Bot.BattlingMonster != null && Bot.BattlingMonster.Equals(target))
                        {
                            _spellsActivatedThisTurn++;
                            AI.SelectCard(target);
                            return true;
                        }
                    }
                    else
                    {
                        _spellsActivatedThisTurn++;
                        AI.SelectCard(target);
                        return true;
                    }
                }
            }

            return false;
        }

        private bool SkillDrainEffect()
        {
            if (Bot.LifePoints <= 1000) return false;

            bool active = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.SkillDrain) && c != Card);
            if (active) return false;

            // If it is face-down on field, restrict when we activate it:
            if (Card.Location == CardLocation.SpellZone && Card.IsFacedown())
            {
                // 0. Chain to cards that target this card or destroy/return backrow
                if (DefaultOnBecomeTarget())
                {
                    return true;
                }

                if (Duel.LastChainPlayer == 1 && Util.GetLastChainCard() != null)
                {
                    var lastCard = Util.GetLastChainCard();
                    if (lastCard.Controller == 1 && lastCard.IsCode(CardId.HeavyStorm, CardId.GiantTrunade, 18144506))
                    {
                        return true;
                    }
                }

                // 1. Chain to opponent's monster effect activation
                if (Duel.LastChainPlayer == 1 && Util.GetLastChainCard() != null && Util.GetLastChainCard().Controller == 1 && Util.GetLastChainCard().IsMonster())
                {
                    return true;
                }

                // 2. Activate in response to a monster being summoned by the opponent that is not a vanilla monster
                bool opponentHasActiveMonsters = Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && !m.IsDisabled() && m.HasType(CardType.Effect));
                if (opponentHasActiveMonsters && Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2 || Duel.Phase == DuelPhase.Battle))
                {
                    return true;
                }

                // 3. Activate on our turn if we have normal summoned Barbaros/Fusilier and want to restore/boost their ATK
                if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
                {
                    bool weHaveBeatsticks = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() &&
                        ((c.IsCode(CardId.BeastKingBarbaros) && c.Attack == 1900) ||
                         (c.IsCode(CardId.FusilierDragonTheDualModeBeast) && c.Attack == 1400)));
                    if (weHaveBeatsticks)
                    {
                        return true;
                    }
                }

                return false;
            }

            // Delay activating Skill Drain in Main Phase 1 if we have MachinaGearframe in hand and can still normal summon it
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1 && Bot.HasInHand(CardId.MachinaGearframe) && !_normalSummonedThisTurn)
            {
                // BUT do NOT delay if the opponent has active disruptors (Silquitous, Hexstia, Multifaker) or dangerous face-up monsters
                bool opponentHasActiveMonsters = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled() &&
                    (c.IsCode(CardId.Silquitous, CardId.Hexstia, CardId.Multifaker) || c.IsMonsterDangerous()));
                if (opponentHasActiveMonsters)
                {
                    return true;
                }
                return false;
            }

            return true;
        }

        private bool TradeInEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (IsMain1SearchDeferred()) return false;
            if (OpponentHasSpellNegator() && _spellsActivatedThisTurn == 0) return false;

            ClientCard target = null;
            if (Bot.HasInHand(CardId.AncientGearGadjiltronDragon))
            {
                target = Bot.Hand.FirstOrDefault(c => c != null && c.IsCode(CardId.AncientGearGadjiltronDragon));
            }
            else if (Bot.HasInHand(CardId.BeastMachineKingBarbarosUr))
            {
                target = Bot.Hand.FirstOrDefault(c => c != null && c.IsCode(CardId.BeastMachineKingBarbarosUr));
            }
            else if (Bot.HasInHand(CardId.BeastKingBarbaros))
            {
                target = Bot.Hand.FirstOrDefault(c => c != null && c.IsCode(CardId.BeastKingBarbaros));
            }

            if (target != null)
            {
                _spellsActivatedThisTurn++;
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool TerraformingEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (IsMain1SearchDeferred()) return false;
            _spellsActivatedThisTurn++;
            // Choose the best Field Spell based on hand state
            if (!Bot.HasInHand(CardId.Geartown) && !Bot.HasInSpellZone(CardId.Geartown))
            {
                AI.SelectCard(CardId.Geartown);
            }
            else if (!Bot.HasInHand(CardId.FutureVisions) && !Bot.HasInSpellZone(CardId.FutureVisions))
            {
                AI.SelectCard(CardId.FutureVisions);
            }
            else
            {
                AI.SelectCard(CardId.MausoleumOfTheEmperor);
            }
            return true;
        }

        private bool GeartownEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.Hand)
            {
                // Play it if we have no Field Spell on the field (overwriting wastes Geartown under modern rules).
                bool anyFieldActive = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Field));
                if (!anyFieldActive)
                {
                    _spellsActivatedThisTurn++;
                    return true;
                }
                return false;
            }

            if (Card.Location == CardLocation.Grave)
            {
                if (IsSpecialSummonBlocked()) return false;
                // Triggered in GY. Summon Gadjiltron Dragon.
                AI.SelectCard(CardId.AncientGearGadjiltronDragon);
                return true;
            }
            return false;
        }

        private bool FutureVisionsEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (CanDealLethal()) return false;

                // If we have Geartown in hand and no field spell on field, prioritize playing Geartown first.
                if (Bot.Hand.Any(c => c != null && c.IsCode(CardId.Geartown)) && !Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Field)))
                {
                    return false;
                }

                // Do not play Future Visions if the opponent already has a strong board (ATK >= 2000 or 2+ monsters)
                // because it won't affect their existing monsters, but will banish our normal summoned defenders.
                int enemyMonsters = Enemy.GetMonsters().Count(c => c != null && c.IsFaceup());
                int enemyMaxAtk = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup()) ? Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).Max(c => c.Attack) : 0;
                if (enemyMonsters >= 2 || enemyMaxAtk >= 2000)
                {
                    return false;
                }

                // Do not play Future Visions if we have Skill Drain active or set, because we want our normal summoned beatsticks to stay on field.
                bool skillDrainReady = Bot.GetSpells().Any(c => c != null && (c.IsFaceup() || c.IsFacedown()) && c.IsCode(CardId.SkillDrain))
                    || Bot.HasInHand(CardId.SkillDrain);
                if (skillDrainReady)
                {
                    return false;
                }

                // Do not overwrite an existing Field Spell
                bool anyFieldActive = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Field));
                if (!anyFieldActive)
                {
                    _spellsActivatedThisTurn++;
                    return true;
                }
                return false;
            }
            return false;
        }

        private bool MausoleumEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Do not overwrite an existing Field Spell (including Geartown, as overwriting doesn't trigger destruction under modern rules)
                bool anyFieldActive = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Field));
                if (anyFieldActive)
                {
                    return false;
                }

                // We must have Gadjiltron in hand to utilize Mausoleum
                bool hasGadjiltronInHand = Bot.Hand.Any(c => c != null && c.IsCode(CardId.AncientGearGadjiltronDragon));
                if (!hasGadjiltronInHand)
                {
                    return false;
                }

                // If we have Geartown in hand, prioritize playing Geartown first
                if (Bot.Hand.Any(c => c != null && c.IsCode(CardId.Geartown)))
                {
                    return false;
                }

                _spellsActivatedThisTurn++;
                return true;
            }

            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                // ONLY pay LP for Gadjiltron Dragon, NEVER for Barbaros (which can be normal summoned for free)
                bool hasGadjiltron = Bot.Hand.Any(c => c != null && c.IsCode(CardId.AncientGearGadjiltronDragon));
                if (hasGadjiltron && Bot.LifePoints > 2000)
                {
                    return true;
                }
            }
            return false;
        }

        private bool MachinaGearframeEffect()
        {
            if (ActivateDescription == Util.GetStringId(CardId.MachinaGearframe, 1))
            {
                if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup())
                {
                    // Equip to Fortress
                    if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
                    if (CanDealLethal()) return false; // Don't equip if we have lethal

                    bool skillDrainActive = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.SkillDrain));
                    if (skillDrainActive) return false;

                    ClientCard fortress = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.MachinaFortress));
                    if (fortress != null && Card.EquipTarget == null)
                    {
                        AI.SelectCard(fortress);
                        return true;
                    }
                }
                return false;
            }

            // Search on Summon (Option 0)
            if (!Bot.HasInHand(CardId.MachinaFortress))
            {
                AI.SelectCard(CardId.MachinaFortress);
            }
            else
            {
                AI.SelectCard(CardId.MachinaForce);
            }
            return true;
        }

        private bool MinefieldrillerEffect()
        {
            AI.SelectCard(CardId.Geartown, CardId.FutureVisions, CardId.MausoleumOfTheEmperor);
            return true;
        }

        private bool GadjiltronEffect()
        {
            // Ancient Gear Gadjiltron Dragon has no active ignition effects on field.
            return false;
        }

        private bool MachinaFortressSummon()
        {
            if (IsSpecialSummonBlocked()) return false;

            if (Card.Location == CardLocation.Grave)
            {
                bool hasLevel8Plus = Bot.Hand.Any(c => c != null && c.IsMonster() && c.HasRace(CardRace.Machine) && c.Level >= 8);
                if (hasLevel8Plus) return true;

                int totalLevel = Bot.Hand.Where(c => c != null && c.IsMonster() && c.HasRace(CardRace.Machine)).Sum(c => c.Level);
                if (totalLevel >= 8) return true;
            }

            if (Card.Location == CardLocation.Hand)
            {
                bool hasOtherMachine = Bot.Hand.Any(c => c != null && c != Card && c.IsMonster() && c.HasRace(CardRace.Machine));
                if (hasOtherMachine) return true;

                bool hasLevel8Plus = Bot.Hand.Any(c => c != null && c.IsMonster() && c.HasRace(CardRace.Machine) && c.Level >= 8 && c != Card);
                if (hasLevel8Plus) return true;
            }

            return false;
        }

        private bool MachinaFortressEffect()
        {
            // Quick effect to destroy a card on opponent field when destroyed, or look at hand.
            // Target opponent's best cards.
            var target = Util.GetBestEnemyCard();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool BarbarosUrSummon()
        {
            if (IsSpecialSummonBlocked()) return false;

            // If Skill Drain is active, prefer keeping Beast King Barbaros in hand to normal summon it!
            bool skillDrainActive = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.SkillDrain));
            if (skillDrainActive && Bot.HasInHand(CardId.BeastKingBarbaros))
            {
                // Only allow summon if we have Beast King Barbaros in GY to banish (not in hand)
                bool hasBarbarosInGy = Bot.Graveyard.Any(c => c != null && c.IsCode(CardId.BeastKingBarbaros));
                if (!hasBarbarosInGy) return false;
            }

            // Optimize Barbaros Ur Summon: return false if it requires banishing BOTH materials from hand and hand is low (< 4)
            bool noMachinesOnFieldOrGy = !Bot.GetMonsters().Concat(Bot.Graveyard)
                .Any(c => c != null && c.IsMonster() && c.HasRace(CardRace.Machine) && c.Id != CardId.BeastMachineKingBarbarosUr);

            bool noBeastWarriorsOnFieldOrGy = !Bot.GetMonsters().Concat(Bot.Graveyard)
                .Any(c => c != null && c.IsMonster() && c.HasRace(CardRace.BestWarrior));

            if (noMachinesOnFieldOrGy && noBeastWarriorsOnFieldOrGy)
            {
                if (Bot.Hand.Count < 4 && !CanDealLethal())
                {
                    return false;
                }
            }

            var machines = Bot.Hand.Concat(Bot.GetMonsters()).Concat(Bot.Graveyard)
                .Where(c => c != null && c.IsMonster() && c.HasRace(CardRace.Machine) && c.Id != CardId.BeastMachineKingBarbarosUr)
                .ToList();

            var beastWarriors = Bot.Hand.Concat(Bot.GetMonsters()).Concat(Bot.Graveyard)
                .Where(c => c != null && c.IsMonster() && c.HasRace(CardRace.BestWarrior))
                .ToList();

            if (machines.Count < 1 || beastWarriors.Count < 1) return false;

            // Check if we are forced to banish a field Ace card
            bool mustBanishMachineFromField = !Bot.Hand.Concat(Bot.Graveyard).Any(c => c != null && c.IsMonster() && c.HasRace(CardRace.Machine) && c.Id != CardId.BeastMachineKingBarbarosUr);
            bool mustBanishBeastWarriorFromField = !Bot.Hand.Concat(Bot.Graveyard).Any(c => c != null && c.IsMonster() && c.HasRace(CardRace.BestWarrior));

            if (mustBanishMachineFromField)
            {
                var fieldMachines = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Machine)).ToList();
                if (fieldMachines.All(c => IsAceCard(c)))
                {
                    bool allowed = false;
                    foreach (var mat in fieldMachines)
                    {
                        var res = ResourcePlan.EvaluateAceUsage(
                            card: mat,
                            hasLethalIfUsed: CanDealLethal(),
                            isOnlyAnswerToThreat: OpponentHasActiveNegator(),
                            haveAlternateWinCon: false
                        );
                        if (res.allowed) { allowed = true; break; }
                    }
                    if (!allowed) return false;
                }
            }

            if (mustBanishBeastWarriorFromField)
            {
                var fieldBeastWarriors = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasRace(CardRace.BestWarrior)).ToList();
                if (fieldBeastWarriors.All(c => IsAceCard(c)))
                {
                    bool allowed = false;
                    foreach (var mat in fieldBeastWarriors)
                    {
                        var res = ResourcePlan.EvaluateAceUsage(
                            card: mat,
                            hasLethalIfUsed: CanDealLethal(),
                            isOnlyAnswerToThreat: OpponentHasActiveNegator(),
                            haveAlternateWinCon: false
                        );
                        if (res.allowed) { allowed = true; break; }
                    }
                    if (!allowed) return false;
                }
            }

            return true;
        }

        private bool ChimeratechSummon()
        {
            if (IsSpecialSummonBlocked()) return false;

            bool cyberDragonOnField = Bot.GetMonsters().Concat(Enemy.GetMonsters()).Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.CyberDragon));
            if (!cyberDragonOnField) return false;

            // Check if there are opponent machines we can use first
            bool opponentMachines = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Machine) && !c.IsCode(CardId.CyberDragon));
            if (opponentMachines) return true;

            // Otherwise, we have to use our own machines. Check if any are Ace cards
            var ourMachines = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Machine) && !c.IsCode(CardId.CyberDragon)).ToList();
            if (!ourMachines.Any()) return false;

            bool hasNonAce = ourMachines.Any(c => !IsAceCard(c));
            if (hasNonAce) return true;

            // Forced to use our own Ace machine
            foreach (var mat in ourMachines.Where(c => IsAceCard(c)))
            {
                var res = ResourcePlan.EvaluateAceUsage(
                    card: mat,
                    hasLethalIfUsed: CanDealLethal(),
                    isOnlyAnswerToThreat: OpponentHasActiveNegator(),
                    haveAlternateWinCon: false
                );
                if (res.allowed)
                {
                    try
                    {
                        AI?.Log(LogLevel.Info, $"[ACE-ALLOW] Allowing Chimeratech summon by sacrificing valuable material {mat.Id}: {res.reason}");
                    }
                    catch { }
                    return true;
                }
            }

            return false;
        }

        private bool GearframeSummon()
        {
            // If Skill Drain is active, prefer summoning Barbaros or Fusilier first (since they are massive beatsticks under Skill Drain)
            bool skillDrainActive = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.SkillDrain));
            if (skillDrainActive && (Bot.HasInHand(CardId.BeastKingBarbaros) || Bot.HasInHand(CardId.FusilierDragonTheDualModeBeast)))
            {
                return false;
            }
            return true;
        }

        private bool BarbarosSummon()
        {
            // Always legal to normal summon without tribute as a 1900 beatstick (or 3000 under Skill Drain)
            return true;
        }

        private bool FusilierSummon()
        {
            // Always legal to normal summon without tribute as a 1400 beatstick (or 2800 under Skill Drain)
            return true;
        }

        private bool GadjiltronTributeSummon()
        {
            bool skillDrainActive = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.SkillDrain));

            // Tribute summon Gadjiltron if Geartown or Mausoleum allows it.
            bool geartownOnField = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.Geartown));
            bool mausoleumOnField = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.MausoleumOfTheEmperor));

            if (mausoleumOnField && Bot.LifePoints > 2000) return true;

            int requiredTributes = geartownOnField ? 1 : 2;
            if (Bot.GetMonsterCount() < requiredTributes) return false;

            if (skillDrainActive && !geartownOnField) return false;

            // Check if we are forced to tribute any Ace cards
            var ownMonsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            var nonAces = ownMonsters.Where(c => !IsAceCard(c)).ToList();
            if (nonAces.Count < requiredTributes)
            {
                var valuableTributes = ownMonsters.Where(c => IsAceCard(c)).ToList();
                bool allowed = false;
                foreach (var mat in valuableTributes)
                {
                    var res = ResourcePlan.EvaluateAceUsage(
                        card: mat,
                        hasLethalIfUsed: CanDealLethal(),
                        isOnlyAnswerToThreat: OpponentHasActiveNegator(),
                        haveAlternateWinCon: false
                    );
                    if (res.allowed)
                    {
                        allowed = true;
                        break;
                    }
                }
                if (!allowed) return false;
            }

            return true;
        }

        private bool CallOfTheHauntedEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Card.Location != CardLocation.SpellZone || !Card.IsFacedown()) return false;

            // Only activate during opponent's turn (End Phase or Battle Phase) or our own Battle Phase for lethal.
            bool opponentTurn = Duel.Player == 1;
            bool isEndPhase = Duel.Phase == DuelPhase.End;
            bool isBattle = Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle;
            bool isTargeted = DefaultOnBecomeTarget();

            bool isBackrowRemoval = false;
            if (Duel.LastChainPlayer == 1 && Util.GetLastChainCard() != null)
            {
                var lastCard = Util.GetLastChainCard();
                if (lastCard.Controller == 1 && lastCard.IsCode(CardId.HeavyStorm, CardId.GiantTrunade, 18144506))
                {
                    isBackrowRemoval = true;
                }
            }

            if (isTargeted || isBackrowRemoval || (opponentTurn && (isEndPhase || isBattle)) || (!opponentTurn && isBattle))
            {
                var targets = new[] {
                    CardId.AncientGearGadjiltronDragon,
                    CardId.BeastKingBarbaros,
                    CardId.BeastMachineKingBarbarosUr,
                    CardId.MachinaFortress
                };

                var available = Bot.Graveyard.Where(c => c != null && c.IsMonster() && c.IsCanRevive() && targets.Contains(c.Id)).ToList();
                if (available.Count > 0)
                {
                    var sorted = available.OrderByDescending(c => {
                        if (c.IsCode(CardId.AncientGearGadjiltronDragon)) return 1000;
                        if (c.IsCode(CardId.MachinaFortress)) return 900;
                        if (c.IsCode(CardId.BeastMachineKingBarbarosUr)) return 800;
                        if (c.IsCode(CardId.BeastKingBarbaros)) return 700;
                        return c.Attack;
                    }).ToList();
                    AI.SelectCard(sorted.First());
                    return true;
                }
            }
            return false;
        }

        private bool LimiterRemovalEffect()
        {
            if (_limiterRemovalUsed) return false;
            if (Duel.Phase != DuelPhase.BattleStart && Duel.Phase != DuelPhase.Battle && Duel.Phase != DuelPhase.Damage) return false;

            if (CanDealLethal())
            {
                _limiterRemovalUsed = true;
                _spellsActivatedThisTurn++;
                return true;
            }

            // Restrict Limiter Removal if the opponent has Return of the Dragon Lords in GY and a Dragon on field
            bool enemyHasReturnInGraveyard = Enemy.Graveyard.Any(c => c != null && c.IsCode(6853254));
            bool enemyHasDragonOnField = Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.HasRace(CardRace.Dragon));
            if (enemyHasReturnInGraveyard && enemyHasDragonOnField)
            {
                if (Bot.BattlingMonster != null && Enemy.BattlingMonster != null)
                {
                    int ourAtk = Bot.BattlingMonster.Attack;
                    int enemyPower = Enemy.BattlingMonster.GetDefensePower();
                    if (ourAtk > enemyPower)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            // Battle protection / combat trick
            if (Bot.BattlingMonster != null && Bot.BattlingMonster.IsFaceup() && Bot.BattlingMonster.HasRace(CardRace.Machine) && Enemy.BattlingMonster != null)
            {
                int ourAtk = Bot.BattlingMonster.Attack;
                int enemyPower = Enemy.BattlingMonster.GetDefensePower();

                if (ourAtk <= enemyPower && (ourAtk * 2) > enemyPower)
                {
                    _limiterRemovalUsed = true;
                    _spellsActivatedThisTurn++;
                    return true;
                }
            }

            return false;
        }

        private bool MirrorForceEffect()
        {
            // Standard Mirror Force logic - allow activation during any opponent phase prompt (e.g. BattleStart or Battle)
            return Duel.Player == 1;
        }

        private bool CompulsoryEvacuationDeviceEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                var lastCard = Util.GetLastChainCard();
                if (lastCard != null && lastCard.IsMonster() && lastCard.Controller == 1 && IsMonsterEffectNegated(lastCard))
                {
                    // Do not chain to already negated monster activation to prevent lifting negation
                    return false;
                }
            }

            // Secret Village Lockbreaker: If locked by Secret Village, bounce their Spellcaster immediately to unlock our spells!
            bool secretVillageActive = Bot.GetSpells().Concat(Enemy.GetSpells()).Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.SecretVillage));
            if (secretVillageActive)
            {
                var spellcaster = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.HasRace(CardRace.SpellCaster) && IsViableEffectTarget(c));
                if (spellcaster != null)
                {
                    AI.SelectCard(spellcaster);
                    return true;
                }
            }

            // Tier 0: Altergeist Control Key Monsters (Highest Priority)
            int[] altergeistPriority = {
                CardId.Hexstia,
                CardId.Silquitous,
                CardId.Multifaker,
                CardId.Marionetter,
                CardId.Meluseek
            };

            foreach (int id in altergeistPriority)
            {
                var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(id) && IsViableEffectTarget(c));
                if (target != null)
                {
                    if (Duel.LastChainPlayer == 1 || (Duel.Player == 1 && (
                        Duel.Phase == DuelPhase.Main1 || 
                        Duel.Phase == DuelPhase.Main2 || 
                        Duel.Phase == DuelPhase.BattleStart || 
                        Duel.Phase == DuelPhase.Battle)))
                    {
                        AI.SelectCard(target);
                        return true;
                    }
                }
            }

            // Tier 0.5: Blue-Eyes Threat Monsters (Alternative White Dragon, Dragon Spirit of White)
            int[] blueEyesPriority = {
                CardId.AlternativeWhiteDragon,
                CardId.DragonSpiritOfWhite
            };

            foreach (int id in blueEyesPriority)
            {
                var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(id) && IsViableEffectTarget(c));
                if (target != null)
                {
                    if (Duel.LastChainPlayer == 1 || (Duel.Player == 1 && (
                        Duel.Phase == DuelPhase.Main1 || 
                        Duel.Phase == DuelPhase.Main2 || 
                        Duel.Phase == DuelPhase.BattleStart || 
                        Duel.Phase == DuelPhase.Battle)))
                    {
                        AI.SelectCard(target);
                        return true;
                    }
                }
            }

            // Tier 1: Extra Deck Monsters (Fusion, Link, XYZ, Synchro)
            // Note: We skip Blue-Eyes Spirit Dragon because targeting it will just force it to tag out for free Azure-Eyes.
            var extraTargets = Enemy.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && c.IsExtraCard() && !c.IsCode(CardId.BlueEyesSpiritDragon) && IsViableEffectTarget(c))
                .OrderByDescending(c => c.Attack)
                .ToList();

            if (extraTargets.Any())
            {
                if (Duel.LastChainPlayer == 1 || (Duel.Player == 1 && (
                    Duel.Phase == DuelPhase.Main1 || 
                    Duel.Phase == DuelPhase.Main2 || 
                    Duel.Phase == DuelPhase.BattleStart || 
                    Duel.Phase == DuelPhase.Battle)))
                {
                    AI.SelectCard(extraTargets.First());
                    return true;
                }
            }

            // Tier 2: Tuners / Combo Starters (prevent Synchro/Link summon)
            var tunerTargets = Enemy.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && c.IsTuner() && IsViableEffectTarget(c))
                .OrderByDescending(c => c.Attack)
                .ToList();

            if (tunerTargets.Any())
            {
                if (Duel.LastChainPlayer == 1 || (Duel.Player == 1 && (
                    Duel.Phase == DuelPhase.Main1 || 
                    Duel.Phase == DuelPhase.Main2 || 
                    Duel.Phase == DuelPhase.BattleStart || 
                    Duel.Phase == DuelPhase.Battle)))
                {
                    AI.SelectCard(tunerTargets.First());
                    return true;
                }
            }

            // Tier 3: Problematic / High ATK Monsters
            var highAtkTargets = Enemy.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && c.Attack >= 2500 && IsViableEffectTarget(c))
                .OrderByDescending(c => c.Attack)
                .ToList();

            if (highAtkTargets.Any())
            {
                if (Duel.LastChainPlayer == 1 || (Duel.Player == 1 && (
                    Duel.Phase == DuelPhase.Main1 || 
                    Duel.Phase == DuelPhase.Main2 || 
                    Duel.Phase == DuelPhase.BattleStart || 
                    Duel.Phase == DuelPhase.Battle)))
                {
                    AI.SelectCard(highAtkTargets.First());
                    return true;
                }
            }

            // Tier 4: Self-Preservation / Chain Target (if Compulsory itself is about to be destroyed)
            bool isBackrowRemoval = false;
            if (Duel.LastChainPlayer == 1 && Util.GetLastChainCard() != null)
            {
                var lastCard = Util.GetLastChainCard();
                if (lastCard.Controller == 1 && lastCard.IsCode(CardId.HeavyStorm, CardId.GiantTrunade, 18144506))
                {
                    isBackrowRemoval = true;
                }
            }

            if (DefaultOnBecomeTarget() || isBackrowRemoval)
            {
                var fallbackTarget = Enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c))
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();

                if (fallbackTarget != null)
                {
                    AI.SelectCard(fallbackTarget);
                    return true;
                }
            }

            return false;
        }

        private bool InterdimensionalMatterTransporterEffect()
        {
            if (Duel.LastChainPlayer == 1 && Util.GetLastChainCard() != null && Util.GetLastChainCard().Controller == 1)
            {
                var ourTarget = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && Util.IsChainTarget(c));
                if (ourTarget != null)
                {
                    AI.SelectCard(ourTarget);
                    return true;
                }
            }
            return false;
        }

        // --- Overrides and Handlers ---

        private Dictionary<ClientCard, int> _azureEyesSummonTurns = new Dictionary<ClientCard, int>();

        protected override bool IsTargetImmune(ClientCard card)
        {
            if (card == null) return false;

            // 1. Chaos MAX Dragon target immunity
            if (card.IsCode(39701395))
                return true;

            // 2. Custom Azure-Eyes Silver Dragon target immunity logic
            if (card.HasRace(CardRace.Dragon))
            {
                var azureEyesList = Enemy.GetMonsters().Concat(Bot.GetMonsters())
                    .Where(m => m != null && m.IsFaceup() && m.IsCode(CardId.AzureEyesSilverDragon))
                    .ToList();

                bool isProtected = false;
                foreach (var ae in azureEyesList)
                {
                    // If Azure-Eyes is currently chaining its summon effect, protection is NOT active yet.
                    bool isSummonChaining = Duel.CurrentChain.Any(cl => cl != null && cl.IsCode(CardId.AzureEyesSilverDragon));
                    if (isSummonChaining)
                        continue;

                    if (!_azureEyesSummonTurns.ContainsKey(ae))
                    {
                        _azureEyesSummonTurns[ae] = Duel.Turn;
                    }

                    int summonTurn = _azureEyesSummonTurns[ae];
                    if (Duel.Turn <= summonTurn + 1)
                    {
                        isProtected = true;
                        break;
                    }
                }

                if (isProtected)
                    return true;
            }

            // 3. Eternal Soul checks (reproduced from ModernExecutor)
            const int EternalSoul = 48682656;
            const int DarkMagician = 46986414;
            const int DarkMagicianTheDragonKnight = 1395963;

            bool isEternalSoulActive = Enemy.GetSpells().Concat(Bot.GetSpells()).Any(s =>
                s != null && s.IsFaceup() && s.IsCode(EternalSoul) && !s.IsDisabled());
            if (isEternalSoulActive &&
                (card.IsCode(DarkMagician) || card.IsCode(DarkMagicianTheDragonKnight)))
                return true;

            return false;
        }

        protected override bool IsBoardStrongEnough()
        {
            bool hasGadjiltron = Bot.HasInMonstersZone(CardId.AncientGearGadjiltronDragon);
            bool hasFortress = Bot.HasInMonstersZone(CardId.MachinaFortress);
            bool hasSkillDrain = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.SkillDrain));
            int setTraps = Bot.GetSpells().Count(c => c != null && c.IsFacedown());

            if ((hasGadjiltron || hasFortress) && (hasSkillDrain || setTraps >= 2)) return true;

            int score = 0;
            if (hasSkillDrain) score += 2;

            foreach (var m in Bot.GetMonsters())
            {
                if (m == null || !m.IsFaceup() || m.IsDisabled()) continue;
                if (m.Attack >= 2500) score += 2;
                else if (m.Attack >= 1800) score += 1;
            }

            score += setTraps;
            return score >= 4;
        }

        protected override bool ShouldStopExtending()
        {
            if (IsBoardStrongEnough()) return true;
            return base.ShouldStopExtending();
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return BossMonsters.Contains(card.Id);
        }

        public override bool ShouldAllowSpSummon(ClientCard card)
        {
            if (!base.ShouldAllowSpSummon(card)) return false;
            if (card == null) return true;

            // Extra Deck summons (Synchro, etc.)
            if (card.HasType(CardType.Synchro))
            {
                if (!CanSummonWithoutValuableMaterials(card.Id))
                {
                    var activeAces = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && IsAceCard(c)).ToList();
                    bool allowed = false;
                    string reason = "";
                    foreach (var mat in activeAces)
                    {
                        var res = ResourcePlan.EvaluateAceUsage(
                            card: mat,
                            hasLethalIfUsed: CanDealLethal(),
                            isOnlyAnswerToThreat: OpponentHasActiveNegator(),
                            haveAlternateWinCon: false
                        );
                        if (res.allowed)
                        {
                            allowed = true;
                            reason = res.reason;
                            break;
                        }
                    }
                    if (!allowed)
                    {
                        DecisionTracer.TraceSkip("ShouldAllowSpSummon", $"Summoning Synchro {card.Name} is not safe (would consume Ace card(s) as material)");
                        return false;
                    }
                    else
                    {
                        try
                        {
                            AI?.Log(LogLevel.Info, $"[ACE-ALLOW] Allowing Synchro summon of {card.Id} by sacrificing valuable material: {reason}");
                        }
                        catch { }
                    }
                }
            }

            return true;
        }

        private bool CanSummonWithoutValuableMaterials(int targetCardId)
        {
            var ownMonsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            var availableMaterials = ownMonsters.Where(c => !IsAceCard(c)).ToList();

            int requiredCount = 2; // Default for Synchros
            return availableMaterials.Count >= requiredCount;
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.IsCode(CardId.Minefieldriller)) return 10;
            if (c.IsCode(CardId.FusilierDragonTheDualModeBeast) && c.Attack == 1400) return 20;
            if (c.IsCode(CardId.BeastKingBarbaros) && c.Attack == 1900) return 30;
            if (c.IsCode(CardId.MachinaGearframe)) return 40;
            return base.GetMaterialPriority(c);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // 1. Machina Fortress discard cost or general discard/cost (hint 533)
            if (hint == 533)
            {
                var sorted = cards.OrderByDescending(c => {
                    if (c == null) return -999;
                    if (c.IsCode(CardId.MachinaForce)) return 1000;
                    if (c.IsCode(CardId.AncientGearGadjiltronDragon)) return 500;
                    if (c.IsCode(CardId.BeastMachineKingBarbarosUr)) return 400;
                    if (c.IsCode(CardId.MachinaFortress)) return 300;
                    return 0;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // 2. Destroy targets (hint 502)
            if (hint == 502)
            {
                // If we want to destroy Geartown for combo
                bool wantDestroyGeartown = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.Geartown));
                if (wantDestroyGeartown)
                {
                    var geartown = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.Geartown) && c.Controller == 0);
                    if (geartown != null)
                    {
                        return new[] { geartown };
                    }
                }
            }

            // 3. Material selection for Chimeratech Fortress Dragon (Contact Fusion)
            // Send opponent machines first!
            bool isChimeratechOption = cards.Any(c => c != null && c.Controller == 1 && c.HasRace(CardRace.Machine));
            if (isChimeratechOption)
            {
                var sortedChimeratech = cards.OrderByDescending(c => {
                    if (c == null) return -999;
                    return (c.Controller == 1) ? 1000 : 0;
                }).ToList();
                return sortedChimeratech.Take(max).ToList();
            }

            // 4. Banish cost for Barbaros Ur (banish from GY first, then Hand, then field)
            bool isBanishTarget = Card != null && Card.IsCode(CardId.BeastMachineKingBarbarosUr) 
                && cards.Any(c => c != null && (c.Location == CardLocation.Grave || c.Location == CardLocation.Hand || c.Location == CardLocation.MonsterZone));
            if (isBanishTarget)
            {
                var sortedBanish = cards.OrderBy(c => {
                    if (c == null) return 999;
                    int score = 0;
                    if (c.Location == CardLocation.Grave) score += 10;
                    if (c.Location == CardLocation.Hand) score += 100;
                    if (c.Location == CardLocation.MonsterZone) score += 1000;
                    if (IsAceCard(c)) score += 5000;
                    return score;
                }).ToList();
                return sortedBanish.Take(max).ToList();
            }

            // 5. Tribute selection for Tribute Summon (hint 503)
            if (hint == 503)
            {
                var sortedTribute = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (c.Location == CardLocation.MonsterZone)
                    {
                        if (c.IsCode(CardId.Minefieldriller)) return 10; // Best tribute target (retrieves field spells!)
                        if (c.IsCode(CardId.FusilierDragonTheDualModeBeast) && c.Attack == 1400) return 20; // Great tribute
                        if (c.IsCode(CardId.BeastKingBarbaros) && c.Attack == 1900) return 30; // Good tribute
                        if (c.IsCode(CardId.MachinaGearframe)) return 45; // Okay tribute
                        if (IsAceCard(c)) return 900; // Keep boss monsters!
                        return 100;
                    }
                    return 200;
                }).ToList();
                return sortedTribute.Take(max).ToList();
            }

            // 6. Call of the Haunted target sorting
            if (Card != null && Card.IsCode(CardId.CallOfTheHaunted))
            {
                var sortedCoth = cards.OrderByDescending(c => {
                    if (c == null) return -999;
                    if (c.IsCode(CardId.AncientGearGadjiltronDragon)) return 1000;
                    if (c.IsCode(CardId.MachinaFortress)) return 900;
                    if (c.IsCode(CardId.BeastMachineKingBarbarosUr)) return 800;
                    if (c.IsCode(CardId.BeastKingBarbaros)) return 700;
                    if (c.IsCode(CardId.FusilierDragonTheDualModeBeast)) return 600;
                    return c.Attack;
                }).ToList();
                return sortedCoth.Take(max).ToList();
            }

            // 7. Attack target selection (hint 549)
            if (hint == 549)
            {
                ClientCard attacker = Bot.BattlingMonster;
                if (attacker != null)
                {
                    int predictedAtk = GetPredictedAttackPower(attacker);
                    var beatable = cards.Where(c => c != null && c.IsFaceup() && c.Location == CardLocation.MonsterZone &&
                        (c.IsAttack() ? predictedAtk >= c.Attack : predictedAtk >= c.Defense)).ToList();

                    if (beatable.Any())
                    {
                        var sortedBeatable = beatable.OrderBy(c => c.IsAttack() ? c.Attack : c.Defense + 10000).ToList();
                        return new[] { sortedBeatable.First() };
                    }

                    var fallback = cards.Where(c => c != null && c.Location == CardLocation.MonsterZone)
                        .OrderBy(c => c.IsFaceup() ? (c.IsAttack() ? c.Attack : c.Defense) : 0)
                        .FirstOrDefault();
                    if (fallback != null)
                    {
                        return new[] { fallback };
                    }
                }
                else
                {
                    int ourBestAtk = Bot.GetMonsters()
                        .Where(c => c != null && c.IsFaceup() && c.IsAttack())
                        .Select(c => GetPredictedAttackPower(c))
                        .DefaultIfEmpty(0).Max();

                    var beatable = cards.Where(c => c != null && c.IsFaceup() && c.Location == CardLocation.MonsterZone &&
                        (c.IsAttack() ? c.Attack <= ourBestAtk : c.Defense <= ourBestAtk)).ToList();

                    if (beatable.Any())
                    {
                        return new[] { beatable.OrderByDescending(c => c.Attack).First() };
                    }

                    var fallback = cards.Where(c => c != null && c.Location == CardLocation.MonsterZone)
                        .OrderBy(c => c.IsFaceup() ? (c.IsAttack() ? c.Attack : c.Defense) : 0)
                        .FirstOrDefault();
                    if (fallback != null)
                    {
                        return new[] { fallback };
                    }
                }
            }

            // 8. Default opponent target removal
            var oppCards = cards.Where(c => c != null && c.Controller == 1).ToList();
            if (oppCards.Count >= min)
            {
                var sortedOpp = oppCards.OrderByDescending(c => {
                    if (c.IsMonster()) return c.Attack;
                    if (c.IsSpell() || c.IsTrap()) return c.IsFaceup() ? 2000 : 100;
                    return 0;
                }).ToList();
                return sortedOpp.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override int OnSelectOption(IList<long> options)
        {
            for (int i = 0; i < options.Count; i++)
            {
                long cardId = options[i] >> 20;
                if (cardId == 0 && Card != null) cardId = Card.Id;
                long optIndex = options[i] & 0xfffff;

                if (cardId == CardId.MausoleumOfTheEmperor)
                {
                    bool hasGadjiltron = Bot.Hand.Any(c => c != null && c.IsCode(CardId.AncientGearGadjiltronDragon));
                    if (hasGadjiltron && optIndex == 1)
                    {
                        return i; // Pay 2000 LP
                    }
                }
            }
            return 0;
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.Minefieldriller && positions.Contains(CardPosition.FaceUpDefence))
                return CardPosition.FaceUpDefence;
            return base.OnSelectPosition(cardId, positions);
        }

        private int GetPredictedAttackPower(ClientCard attacker)
        {
            if (attacker == null) return 0;
            int power = attacker.Attack;

            // 1. Limiter Removal prediction
            bool hasLimiterRemoval = Bot.HasInHand(CardId.LimiterRemoval) || 
                                     Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.LimiterRemoval));
            if (hasLimiterRemoval && !_limiterRemovalUsed && attacker.HasRace(CardRace.Machine))
            {
                power = power * 2;
            }

            // 2. Forbidden Chalice prediction
            bool hasChalice = Bot.HasInHand(CardId.ForbiddenChalice) || 
                              Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.ForbiddenChalice));
            if (hasChalice)
            {
                int chalicePower = attacker.Attack;
                if (attacker.IsCode(CardId.BeastKingBarbaros) && attacker.Attack == 1900)
                {
                    chalicePower = 3400;
                }
                else if (attacker.IsCode(CardId.FusilierDragonTheDualModeBeast) && attacker.Attack == 1400)
                {
                    chalicePower = 3200;
                }
                else
                {
                    chalicePower += 400;
                }

                if (chalicePower > power)
                {
                    power = chalicePower;
                }
            }

            return power;
        }

        public override bool OnPreBattleBetween(ClientCard attacker, ClientCard defender)
        {
            // Boost attacker power dynamically to help DefaultExecutor's OnSelectAttackTarget choose correct battles.
            if (attacker.RealPower <= defender.RealPower)
            {
                int predicted = GetPredictedAttackPower(attacker);
                if (predicted > defender.RealPower)
                {
                    attacker.RealPower = predicted;
                }
            }

            if (!base.OnPreBattleBetween(attacker, defender)) return false;

            // Prevent suicide attack (our ATK <= opponent's ATK/DEF) if we have no Skill Drain active 
            // and the opponent has active negators that can stop our GY triggers (like Minefieldriller or Fortress).
            if (attacker.RealPower <= defender.RealPower)
            {
                bool skillDrainActive = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.SkillDrain));
                if (!skillDrainActive)
                {
                    bool opponentHasNegator = Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.AltergeistProtocol))
                        || OpponentHasGyOrGenericNegator();
                    if (opponentHasNegator)
                    {
                        return false; // Cancel the attack
                    }
                }
            }

            // Prevent bad Machina Fortress suicides
            if (attacker.IsCode(CardId.MachinaFortress) && attacker.RealPower <= defender.RealPower)
            {
                bool protectedByReturn = defender.HasRace(CardRace.Dragon) && Enemy.Graveyard.Any(c => c != null && c.IsCode(6853254));
                if (IsTargetImmune(defender) || protectedByReturn)
                {
                    return false; // Cancel the attack
                }
            }

            return true;
        }
    }

    [Deck("Expert_2026_AncientG", "2026_AncientG")]
    public class ExpertAncientGExecutor : _2026_AncientGExecutor
    {
        private string _duelId;
        public ExpertAncientGExecutor(GameAI ai, Duel duel) : base(ai, duel)
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
