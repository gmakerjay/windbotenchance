using YGOSharp.OCGWrapper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    // ============================================================
    // CARD AUDIT — 2026_Doomz
    // ============================================================
    // | Card Name                                 | Type    | OPT? | Cost            | Effect Summary                                | Activate When                     | NEVER Activate When                    |
    // |-------------------------------------------|---------|------|-----------------|-----------------------------------------------|-----------------------------------|----------------------------------------|
    // | DoomZ VII Seven - Elara (54265980)        | Monster | Yes  | None            | Summon/destroy: Set 1 DoomZ Spell/Trap        | Summoned or destroyed             | No Spell/Trap left in Deck             |
    // |                                           |         | Yes  | None (Quick)    | While equipped: SS Rank 4 Diactorus           | Equipped on field                 | Special Summon blocked                 |
    // | DoomZ V Five - Amalthe (28877382)         | Monster | Yes  | None            | Summon/destroy: Add 1 DoomZ monster           | Summoned or destroyed             | No monster left in Deck                |
    // |                                           |         | Yes  | None            | Hand: SS if control DoomZ                     | Have DoomZ on field               | Special Summon blocked                 |
    // |                                           |         | Yes  | None (Quick)    | While equipped: SS Rank 4 Diactorus           | Equipped on field                 | Special Summon blocked                 |
    // | DoomZ XII Zero - Drastea (92472273)       | Monster | Yes  | Pop 1 DoomZ card| Hand: SS itself, then equip 1 Equip from Deck | DoomZ card on field               | No Equip left in Deck                  |
    // |                                           |         | Yes  | None (Quick)    | While equipped: SS Rank 8 Drastrius           | Equipped on field                 | Special Summon blocked                 |
    // | Power Patron DoomZ (81650695)             | Monster | Yes  | None            | Hand: SS if control DoomZ/Patron              | Have DoomZ/Patron on field        | Special Summon blocked                 |
    // |                                           |         | Yes  | None            | Target own monster: overlay into DoomZ Xyz    | Level 4/8 on field                | Special Summon blocked                 |
    // |                                           |         | Yes  | None            | Destroyed: search 1 DoomZ card                | Destroyed                         | No DoomZ left in Deck                  |
    // | Medius the Pure (97556336)                | Monster | Yes  | None            | Summon: search/SS 1 Power Patron monster      | Normal or Special Summoned        | No Power Patron in Deck                |
    // |                                           |         | Yes  | Shuffle monster | GY: SS itself (banish when leaves)            | Monster in hand/field to shuffle  | Special Summon blocked                 |
    // | Vidrium ... Chaos Extermination (70488851)| Monster | Yes  | Return F/S/X    | SS itself from ED/GY; banish GYs face-down    | F/S/X in GY/field to return       | Special Summon blocked                 |
    // | Power Patron Shadow Machine Zegredo (43871165)| Monster| Yes| Banish 3 FD     | SS Jupiter from ED, attach 1 from hand        | Main Phase                        | Special Summon blocked                 |
    // |                                           |         | Yes  | Pop itself + card| Pendulum: destroy itself + card -> pop enemy  | Main Phase                        | No enemy card to destroy               |
    // | DoomZ Change (38007649)                   | Spell   | Yes  | None            | Destroy 1 DoomZ card in hand/Deck/field       | Main Phase                        | No DoomZ to destroy                    |
    // | DoomZ Raiders (32442017)                  | Trap    | Yes  | Pop 1 DoomZ     | Continuous: destroy 1 DoomZ -> search/SS DoomZ| Main Phase                        | Extra Deck locked to Xyz               |
    // | DoomZ Command "D.O.O.M.D.U.R.G." (68831625)| Spell   | Yes  | Pop own card    | Equipped gains direct attack + ATK boost      | Equipped on field                 | None                                   |
    // | DoomZ Command "A.D.R.A.S.T.E.I.A." (84054556)| Spell  | Yes  | None            | Pop equipped -> SS DoomZ from hand/GY/banish  | Equipped on field                 | None                                   |
    // | Null Power Patron Realm - Vidria (51669847)| Spell  | Yes  | Banish 1 hand FD| Reveal Patron in ED -> search/SS its mention  | Main Phase                        | No card in hand to banish              |
    // | Unleashed Power Patron Portal - Terminus (25661743)| Spell| Yes| None          | Send Power Patron from Deck/ED -> search Fairy| Main Phase                        | No DARK Fairy left in Deck             |
    // | The Fallen & The Virtuous (30271097)      | Spell   | Yes  | Send Albaz ED   | Send Albaz from ED -> pop 1 face-up card      | Main Phase                        | No face-up card on field               |
    // | DoomZ Destruction (80320877)              | Trap    | Yes  | None            | Equip to DoomZ Xyz -> search floodgate        | Main Phase                        | No DoomZ Xyz on field                  |
    // ============================================================
    // ACE CARDS: Primary: Jupiter the Power Patron of Destruction / Secondary: DoomZ XII End - Drastrius / Tertiary: DoomZ XIII Over - Graflario
    // COMBO STARTERS: 1. Medius the Pure 2. Null Power Patron Realm - Vidria 3. DoomZ Change
    // CHOKEPOINTS: Medius Summon negated = Zegredo search fails
    // ============================================================

    [Deck("2026_Doomz", "2026_Doomz")]
    public class _2026_DoomzExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Custom Cards
            public const int DoomZVIISevenElara = 54265980;
            public const int DoomZXIIZeroDrastea = 92472273;
            public const int DoomZVFiveAmalthe = 28877382;
            public const int PowerPatronDoomZ = 81650695;
            public const int MediusThePure = 97556336;
            public const int VidriumThePowerPatronOfChaosExtermination = 70488851;
            public const int PowerPatronShadowMachineZegredo = 43871165;
            public const int DoomZChange = 38007649;
            public const int DoomZRaiders = 32442017;
            public const int DoomZCommandDOOMDURG = 68831625;
            public const int DoomZCommandADRASTEIA = 84054556;
            public const int NullPowerPatronRealmVidria = 51669847;
            public const int UnleashedPowerPatronPortalTerminus = 25661743;
            public const int TheFallenAndTheVirtuous = 30271097;
            public const int DoomZDestruction = 80320877;

            // Extra Deck Custom Cards
            public const int DoomZBreakDiactorus = 31010081;
            public const int DoomZXIIEndDrastrius = 95626382;
            public const int DoomZXIIIOverGraflario = 65848113;
            public const int JupiterThePowerPatronOfDestruction = 68231287;

            // Other Deck Cards (Staples / Support)
            public const int BystialMagnamhut = 33854624;
            public const int MulcharmyFuwalos = 42141493;
            public const int AshBlossom = 14558128;
            public const int DrollAndLockBird = 94145021;
            public const int GhostBelle = 73642296;
            public const int CalledByTheGrave = 24224830;
            public const int VarudrasTheFinalBringerOfTheEndTimes = 70636044;
            public const int GiganticChampionSargas = 11132674;
            public const int SpringansMerrymaker = 48285768;
            public const int NumberF0UtopicFutureZexal = 41522092;
            public const int EcclesiaAndTheDarkDragon = 78397661;
            public const int AlbionTheBrandedDragon = 87746184;
            public const int ClockworkKnight = 41739381;
        }

        // Once per turn trackers
        private bool _mediusUsed = false;
        private bool _mediusGYUsed = false;
        private bool _zegredoUsed = false;
        private bool _zegredoPendUsed = false;
        private bool _zegredoPendActivateUsed = false;
        private bool _doomzChangeUsed = false;
        private bool _doomzRaidersUsed = false;
        private bool _adrasteiaGYUsed = false;
        private bool _terminusUsed = false;
        private bool _fallenUsed = false;
        private bool _vidriaUsed = false;
        private bool _jupiterUsed = false;
        private bool _drastriusUsed = false;
        private bool _graflarioUsed = false;
        private bool _elaraQuickUsed = false;
        private bool _amaltheQuickUsed = false;
        private bool _drasteaQuickUsed = false;
        private bool _drasteaHandUsed = false;
        private bool _elaraTriggerUsed = false;
        private bool _amaltheTriggerUsed = false;
        private bool _patronDoomzTriggerUsed = false;
        private bool _changeGYUsed = false;
        private bool _vidriumPendUsed = false;

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.JupiterThePowerPatronOfDestruction
                || card.Id == CardId.DoomZXIIEndDrastrius
                || card.Id == CardId.DoomZXIIIOverGraflario
                || card.Id == CardId.VarudrasTheFinalBringerOfTheEndTimes
                || card.Id == CardId.NumberF0UtopicFutureZexal;
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.Id == CardId.AshBlossom || c.Id == CardId.GhostBelle || c.Id == CardId.DrollAndLockBird || c.Id == CardId.MulcharmyFuwalos)
                return 800;
            return base.GetMaterialPriority(c);
        }

        protected override bool IsBoardStrongEnough()
        {
            // Jupiter on field with overlay materials = complete board
            if (Bot.HasInMonstersZone(CardId.JupiterThePowerPatronOfDestruction))
                return true;
            // Drastrius + any other ace = strong enough
            if (Bot.HasInMonstersZone(CardId.DoomZXIIEndDrastrius) && Bot.GetMonsterCount() >= 2)
                return true;
            // Graflario on field = sufficient for going-first
            if (Bot.HasInMonstersZone(CardId.DoomZXIIIOverGraflario))
                return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            // Stop once we have Jupiter or Drastrius — don't waste resources
            bool hasAceBoss = Bot.HasInMonstersZone(CardId.JupiterThePowerPatronOfDestruction)
                || Bot.HasInMonstersZone(CardId.DoomZXIIEndDrastrius)
                || Bot.HasInMonstersZone(CardId.DoomZXIIIOverGraflario);
            if (!hasAceBoss) return false;
            return base.ShouldStopExtending();
        }

        public _2026_DoomzExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Heuristic protections
            HeuristicGuard.RegisterAceCards(
                CardId.JupiterThePowerPatronOfDestruction,
                CardId.DoomZXIIEndDrastrius,
                CardId.DoomZXIIIOverGraflario,
                CardId.VarudrasTheFinalBringerOfTheEndTimes,
                CardId.NumberF0UtopicFutureZexal
            );
            // ── Combo Router: Sequencing ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Standard-Setup",
                RequiredCards = new List<int> { CardId.MediusThePure, CardId.DoomZXIIZeroDrastea },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.MediusThePure, ActionType = ExecutorType.Activate, Description = "Play CardId.MediusThePure" },
                    new() { CardId = CardId.DoomZXIIZeroDrastea, ActionType = ExecutorType.Activate, Description = "Extend with CardId.DoomZXIIZeroDrastea" }
                },
                EndBoardScore = 80
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Vidria-To-Jupiter",
                RequiredCards = new List<int> { CardId.NullPowerPatronRealmVidria },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.NullPowerPatronRealmVidria, ActionType = ExecutorType.Activate, Description = "Activate Vidria to search" },
                    new() { CardId = CardId.PowerPatronShadowMachineZegredo, ActionType = ExecutorType.Activate, Description = "Zegredo summons Jupiter" },
                    new() { CardId = CardId.JupiterThePowerPatronOfDestruction, ActionType = ExecutorType.SpSummon, Description = "Summon Jupiter" }
                },
                EndBoardScore = 75
            });

            // ── Bait Planner ──
            BaitPlanner.RegisterComboStarters(CardId.MediusThePure, CardId.DoomZVFiveAmalthe, CardId.NullPowerPatronRealmVidria, CardId.PowerPatronDoomZ, CardId.DoomZVIISevenElara, CardId.DoomZChange);
            BaitPlanner.RegisterBaitCards(CardId.DoomZVFiveAmalthe);

            // ── Chain Advisor ──
            ChainAdvisor.RegisterHighValueTargets(CardId.MediusThePure, CardId.PowerPatronShadowMachineZegredo);


            // TIER 1: Hand Traps (negates opponent, never ourselves)
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelle, GhostBelleEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);

            // TIER 2: Quick Effects / Negations / Boss Disruptions
            AddExecutor(ExecutorType.Activate, CardId.DoomZXIIEndDrastrius, DrastriusNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.VarudrasTheFinalBringerOfTheEndTimes);
            AddExecutor(ExecutorType.Activate, CardId.DoomZVIISevenElara, ElaraQuickEffect);
            AddExecutor(ExecutorType.Activate, CardId.DoomZVFiveAmalthe, AmaltheQuickEffect);
            AddExecutor(ExecutorType.Activate, CardId.DoomZXIIZeroDrastea, DrasteaQuickEffect);

            // TIER 3: Fusion / SS / Spell Setup
            AddExecutor(ExecutorType.Activate, CardId.TheFallenAndTheVirtuous, TheFallenAndTheVirtuousEffect);
            AddExecutor(ExecutorType.Activate, CardId.AlbionTheBrandedDragon, AlbionEffect);

            // TIER 4: Search & Field Spells
            AddExecutor(ExecutorType.Activate, CardId.NullPowerPatronRealmVidria, NullPowerPatronRealmEffect);
            AddExecutor(ExecutorType.Activate, CardId.UnleashedPowerPatronPortalTerminus, TerminusEffect);

            // TIER 5: Monster Effects (Hand/GY / Ignition / Trigger)
            AddExecutor(ExecutorType.Activate, CardId.MediusThePure, MediusSummonEffect);
            AddExecutor(ExecutorType.Activate, CardId.MediusThePure, MediusGYEffect);
            
            // Pre-Summon: SS from hand (before Normal Summon)
            AddExecutor(ExecutorType.Activate, CardId.DoomZXIIZeroDrastea, DrasteaHandSummon);
            AddExecutor(ExecutorType.Activate, CardId.DoomZVFiveAmalthe, AmaltheHandSSEffect);
            
            // Pendulum Zone activations from hand
            AddExecutor(ExecutorType.Activate, CardId.PowerPatronShadowMachineZegredo, ZegredoPendulumActivate);
            AddExecutor(ExecutorType.Activate, CardId.VidriumThePowerPatronOfChaosExtermination, VidriumPendulumActivate);
            
            // Monster trigger effects (Summoned or Destroyed)
            AddExecutor(ExecutorType.Activate, CardId.DoomZVIISevenElara, ElaraTriggerEffect);
            AddExecutor(ExecutorType.Activate, CardId.DoomZVFiveAmalthe, AmaltheTriggerEffect);
            AddExecutor(ExecutorType.Activate, CardId.PowerPatronDoomZ, PowerPatronDoomZDestroyedEffect);
            AddExecutor(ExecutorType.Activate, CardId.DoomZChange, DoomZChangeGYEffect);

            AddExecutor(ExecutorType.Activate, CardId.PowerPatronShadowMachineZegredo, ZegredoMonsterEffect);
            AddExecutor(ExecutorType.Activate, CardId.PowerPatronShadowMachineZegredo, ZegredoPendulumEffect);
            AddExecutor(ExecutorType.Activate, CardId.DoomZChange, DoomZChangeEffect);
            AddExecutor(ExecutorType.Activate, CardId.DoomZRaiders, DoomZRaidersEffect);
            AddExecutor(ExecutorType.Activate, CardId.PowerPatronDoomZ, PowerPatronDoomZFieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.DoomZXIIIOverGraflario, GraflarioEffect);
            AddExecutor(ExecutorType.Activate, CardId.JupiterThePowerPatronOfDestruction, JupiterEffect);
            AddExecutor(ExecutorType.Activate, CardId.DoomZCommandADRASTEIA, AdrasteiaGYEffect);
            AddExecutor(ExecutorType.Activate, CardId.DoomZDestruction, DoomZDestructionEffect);

            // TIER 6: Special Summons & Normal Summons
            AddExecutor(ExecutorType.SpSummon, CardId.VidriumThePowerPatronOfChaosExtermination, VidriumSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DoomZVFiveAmalthe); // SS from hand
            
            // Normal Summons (with conditions)
            AddExecutor(ExecutorType.Summon, CardId.MediusThePure, ShouldSummonMedius);
            AddExecutor(ExecutorType.Summon, CardId.DoomZVIISevenElara, ShouldSummonElara);
            AddExecutor(ExecutorType.Summon, CardId.DoomZVFiveAmalthe, ShouldSummonAmalthe);
            AddExecutor(ExecutorType.Summon, CardId.PowerPatronShadowMachineZegredo, ShouldSummonZegredo);
            // Patron DoomZ should NOT be Normal Summoned — SS from hand is better

            // Fallback Normal Summon
            AddExecutor(ExecutorType.Summon, CardId.MediusThePure);
            AddExecutor(ExecutorType.Summon, CardId.DoomZVFiveAmalthe);
            AddExecutor(ExecutorType.Summon, CardId.DoomZVIISevenElara);
            AddExecutor(ExecutorType.Summon, CardId.PowerPatronShadowMachineZegredo);
            AddExecutor(ExecutorType.Summon, CardId.PowerPatronDoomZ);

            // TIER 7: Extra Deck Summons
            AddExecutor(ExecutorType.SpSummon, CardId.JupiterThePowerPatronOfDestruction);
            AddExecutor(ExecutorType.SpSummon, CardId.DoomZXIIIOverGraflario, GraflarioSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DoomZXIIEndDrastrius);
            AddExecutor(ExecutorType.SpSummon, CardId.DoomZBreakDiactorus);
            AddExecutor(ExecutorType.SpSummon, CardId.VarudrasTheFinalBringerOfTheEndTimes);
            AddExecutor(ExecutorType.SpSummon, CardId.GiganticChampionSargas);
            AddExecutor(ExecutorType.SpSummon, CardId.SpringansMerrymaker);
            AddExecutor(ExecutorType.SpSummon, CardId.NumberF0UtopicFutureZexal);

            // Extra Deck trigger / activation
            AddExecutor(ExecutorType.Activate, CardId.DoomZBreakDiactorus);

            // TIER 8: Sets & Repos
            AddExecutor(ExecutorType.SpellSet, CardId.TheFallenAndTheVirtuous);
            AddExecutor(ExecutorType.SpellSet, CardId.DoomZRaiders);
            AddExecutor(ExecutorType.SpellSet, CardId.DoomZDestruction);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave);

            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // Control deck — strongly prefer going first to establish floodgates/xyz setups
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _mediusUsed = false;
            _mediusGYUsed = false;
            _zegredoUsed = false;
            _zegredoPendUsed = false;
            _zegredoPendActivateUsed = false;
            _doomzChangeUsed = false;
            _doomzRaidersUsed = false;
            _adrasteiaGYUsed = false;
            _terminusUsed = false;
            _fallenUsed = false;
            _vidriaUsed = false;
            _jupiterUsed = false;
            _drastriusUsed = false;
            _graflarioUsed = false;
            _elaraQuickUsed = false;
            _amaltheQuickUsed = false;
            _drasteaQuickUsed = false;
            _drasteaHandUsed = false;
            _elaraTriggerUsed = false;
            _amaltheTriggerUsed = false;
            _patronDoomzTriggerUsed = false;
            _changeGYUsed = false;
            _vidriumPendUsed = false;

            // ── Going-Second BreakBoard: prioritize disruption over combo ──
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
            }
        }

        private int GetFreeMonsterZoneCount()
        {
            int count = 0;
            for (int i = 0; i < 5; ++i)
            {
                if (Bot.MonsterZone[i] == null) count++;
            }
            return count;
        }

        private int GetHandPriority(ClientCard c)
        {
            if (c == null) return 999;
            // Higher value = KEEP (protect), Lower value = DISCARD/BANISH first
            // Hand traps: low priority (banish first as cost)
            if (c.Id == CardId.AshBlossom || c.Id == CardId.GhostBelle || c.Id == CardId.DrollAndLockBird || c.Id == CardId.MulcharmyFuwalos) return 50;
            if (c.Id == CardId.CalledByTheGrave) return 100;
            // Non-engine: discard early
            if (c.Id == CardId.BystialMagnamhut) return 150;
            // Engine pieces: KEEP (high priority)
            if (c.Id == CardId.MediusThePure) return 900;
            if (c.Id == CardId.PowerPatronShadowMachineZegredo) return 850;
            if (c.Id == CardId.DoomZXIIZeroDrastea) return 800;
            if (c.Id == CardId.DoomZVIISevenElara) return 700;
            if (c.Id == CardId.DoomZVFiveAmalthe) return 600;
            if (c.Id == CardId.PowerPatronDoomZ) return 500;
            if (c.Id == CardId.DoomZChange) return 450;
            return 200;
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
                if (card.IsFaceup() && !card.IsDisabled() && (card.Id == 84815190 || card.Id == 4280258 || card.Id == 50954680))
                    score += 5000;
            }
            else if (card.IsSpell() || card.IsTrap())
            {
                score += 500;
                if (card.IsFaceup()) score += 1000;
            }
            return score;
        }

        private bool IsSafeToAttack(ClientCard attacker)
        {
            foreach (ClientCard enemy in Enemy.MonsterZone)
            {
                if (enemy == null || !enemy.IsFaceup()) continue;
                if (!enemy.IsDisabled())
                {
                    if (enemy.Id == 21887175 && attacker.IsSpecialSummoned) return false;
                    if (enemy.Id == 50954680 && attacker.Level >= 5) return false;
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

        private bool MonsterRepos()
        {
            if (Card == null) return false;
            bool enemyEmpty = Enemy.GetMonsterCount() == 0;
            if (Card.IsAttack())
            {
                if (!enemyEmpty && !IsSafeToAttack(Card) && IsSafeToDefend(Card)) return true;
            }
            else
            {
                if (enemyEmpty || IsSafeToAttack(Card)) return true;
            }
            return false;
        }

        // ==========================================
        //  TIER 1: Hand Traps & Negations
        // ==========================================

        private bool MulcharmyEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }

        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.LastChainPlayer == 1 && DefaultAshBlossomAndJoyousSpring();
        }

        private bool GhostBelleEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.LastChainPlayer == 1 && DefaultGhostBelleAndHauntedMansion();
        }

        private bool DrollAndLockBirdEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.Player == 1 && Duel.LastChainPlayer == 1;
        }

        private bool CalledByTheGraveEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;

            bool opponentHandTrapChained = LastChainCard != null && 
                (LastChainCard.Id == CardId.AshBlossom || 
                 LastChainCard.Id == CardId.GhostBelle || 
                 LastChainCard.Id == CardId.DrollAndLockBird);

            if (opponentHandTrapChained)
            {
                return DefaultCalledByTheGrave();
            }

            if (LastChainCard != null && LastChainCard.Location == CardLocation.Grave)
            {
                return DefaultCalledByTheGrave();
            }

            return false;
        }

        // ==========================================
        //  TIER 2: Quick Effects and Disruptions
        // ==========================================

        private bool DrastriusNegateEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_drastriusUsed) return false;
            if (LastChainCard == null || LastChainCard.Controller != 1 || !LastChainCard.IsMonster()) return false;
            if (Card.Overlays.Count == 0) return false;

            _drastriusUsed = true;
            return true;
        }

        private bool ElaraQuickEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.EquipCards.Count == 0) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (_elaraQuickUsed) return false;

            if (Duel.Player == 1)
            {
                if (Duel.LastChainPlayer == 1 || Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
                {
                    _elaraQuickUsed = true;
                    return true;
                }
                return false;
            }

            bool hasUnusedEquipIgnition = Card.EquipCards.Any(eq => eq.Id == CardId.DoomZCommandADRASTEIA && !_adrasteiaGYUsed);
            if (hasUnusedEquipIgnition) return false;

            _elaraQuickUsed = true;
            return true;
        }

        private bool AmaltheQuickEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.EquipCards.Count == 0) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (_amaltheQuickUsed) return false;

            if (Duel.Player == 1)
            {
                if (Duel.LastChainPlayer == 1 || Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
                {
                    _amaltheQuickUsed = true;
                    return true;
                }
                return false;
            }

            bool hasUnusedEquipIgnition = Card.EquipCards.Any(eq => eq.Id == CardId.DoomZCommandADRASTEIA && !_adrasteiaGYUsed);
            if (hasUnusedEquipIgnition) return false;

            _amaltheQuickUsed = true;
            return true;
        }

        private bool DrasteaQuickEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.EquipCards.Count == 0) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (_drasteaQuickUsed) return false;

            if (Duel.Player == 1)
            {
                if (Duel.LastChainPlayer == 1 || Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
                {
                    _drasteaQuickUsed = true;
                    return true;
                }
                return false;
            }

            bool hasUnusedEquipIgnition = Card.EquipCards.Any(eq => eq.Id == CardId.DoomZCommandADRASTEIA && !_adrasteiaGYUsed);
            if (hasUnusedEquipIgnition) return false;

            _drasteaQuickUsed = true;
            return true;
        }

        // ==========================================
        //  TIER 3 & 4: Spells & Searches
        // ==========================================

        private bool TheFallenAndTheVirtuousEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_fallenUsed) return false;

                bool hasAlbion = GetRemainingCount(CardId.AlbionTheBrandedDragon) > 0;
                bool hasFaceupTarget = Enemy.GetMonsters().Concat(Enemy.GetSpells()).Any(c => c != null && c.IsFaceup());
                bool hasEcclesia = Bot.GetMonsters().Concat(Bot.Graveyard).Any(c => c != null && c.Id == CardId.EcclesiaAndTheDarkDragon);
                bool hasGYTarget = Bot.Graveyard.Concat(Enemy.Graveyard).Any(c => c != null && c.IsMonster() && c.IsCanRevive());

                if ((hasAlbion && hasFaceupTarget) || (hasEcclesia && hasGYTarget))
                {
                    _fallenUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool AlbionEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            return Duel.Phase == DuelPhase.End;
        }

        private bool NullPowerPatronRealmEffect()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                if (_vidriaUsed) return false;
                if (Bot.Hand.Count <= 1 && Card.Location == CardLocation.Hand) return false;
                if (Bot.Hand.Count == 0 && Card.Location == CardLocation.SpellZone) return false;

                bool hasJupiter = GetRemainingCount(CardId.JupiterThePowerPatronOfDestruction) > 0;
                if (!hasJupiter) return false;

                _vidriaUsed = true;
                return true;
            }
            return false;
        }

        private bool TerminusEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_terminusUsed) return false;

                bool hasSendTarget = HasRemainingMonsterWithSetcode(0x1c6);
                if (!hasSendTarget) return false;

                // Dark Fairy monster search target — attribute/race not queryable from StartingDeck in this fork
                bool hasSearchTarget = true; // Let engine handle target validation
                if (!hasSearchTarget) return false;

                _terminusUsed = true;
                return true;
            }
            return false;
        }

        // ==========================================
        //  TIER 5: Monster Effects (Ignition & Trigger)
        // ==========================================

        private bool MediusSummonEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_mediusUsed) return false;

            _mediusUsed = true;
            return true;
        }

        private bool MediusGYEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (_mediusGYUsed) return false;

            var targets = Bot.Hand.Concat(Bot.GetMonsters().Where(c => c != null && c.IsFaceup()))
                .Where(c => c != null && c.IsMonster() && !IsAceCard(c))
                .ToList();
            if (targets.Count == 0) return false;

            _mediusGYUsed = true;
            return true;
        }

        private bool DrasteaHandSummon()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (_drasteaHandUsed) return false;

            bool hasDoomzOnField = Bot.GetMonsters().Concat(Bot.GetSpells())
                .Any(c => c != null && c.IsFaceup() && c.HasSetcode(0x1cb) && c != Card);
            if (!hasDoomzOnField) return false;

            _drasteaHandUsed = true;
            return true;
        }

        private bool ZegredoPendulumActivate()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location != CardLocation.Hand) return false;
            if (_zegredoPendActivateUsed) return false;

            bool hasMonster = Bot.GetMonsters().Any(c => c != null && c.IsFaceup());
            if (!hasMonster) return false;

            _zegredoPendActivateUsed = true;
            return true;
        }

        private bool VidriumPendulumActivate()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location != CardLocation.Hand) return false;
            if (_vidriumPendUsed) return false;

            bool hasSummonTarget = Bot.Hand.Concat(Bot.Graveyard)
                .Any(c => c != null && c.IsMonster() && c.HasSetcode(0x1c6) && c.Id != CardId.VidriumThePowerPatronOfChaosExtermination);
            if (!hasSummonTarget) return false;

            _vidriumPendUsed = true;
            return true;
        }

        private bool ElaraTriggerEffect()
        {
            if (Card.Location != CardLocation.MonsterZone && Card.Location != CardLocation.Grave && Card.Location != CardLocation.Removed) return false;
            if (_elaraTriggerUsed) return false;

            bool hasTarget = HasRemainingCardWithSetcode(0x1cb);
            if (!hasTarget) return false;

            _elaraTriggerUsed = true;
            return true;
        }

        private bool AmaltheTriggerEffect()
        {
            if (Card.Location != CardLocation.MonsterZone && Card.Location != CardLocation.Grave && Card.Location != CardLocation.Removed) return false;
            if (_amaltheTriggerUsed) return false;

            bool hasTarget = HasRemainingMonsterWithSetcode(0x1cb);
            if (!hasTarget) return false;

            _amaltheTriggerUsed = true;
            return true;
        }

        private bool PowerPatronDoomZDestroyedEffect()
        {
            if (Card.Location != CardLocation.Grave && Card.Location != CardLocation.Removed) return false;
            if (_patronDoomzTriggerUsed) return false;

            bool hasTarget = HasRemainingCardWithSetcode(0x1cb);
            if (!hasTarget) return false;

            _patronDoomzTriggerUsed = true;
            return true;
        }

        private bool DoomZChangeGYEffect()
        {
            if (Card.Location != CardLocation.Grave && Card.Location != CardLocation.Removed) return false;
            if (_changeGYUsed) return false;

            bool hasTarget = Bot.Graveyard.Any(c => c.HasSetcode(0x1cb) && c.Id != CardId.DoomZChange);
            if (!hasTarget) return false;

            _changeGYUsed = true;
            return true;
        }

        private bool ZegredoMonsterEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (_zegredoUsed) return false;
            if (Bot.Deck.Count < 3) return false;

            bool hasJupiter = GetRemainingCount(CardId.JupiterThePowerPatronOfDestruction) > 0;
            if (!hasJupiter) return false;

            _zegredoUsed = true;
            return true;
        }

        private bool ZegredoPendulumEffect()
        {
            if (Card.Location != CardLocation.SpellZone) return false;
            if (_zegredoPendUsed) return false;

            var targets = Bot.Hand.Concat(Bot.GetMonsters())
                .Where(c => c != null && c != Card && c.IsMonster() && (c.HasSetcode(0x1cb) || c.HasSetcode(0x1c6)))
                .ToList();
            if (targets.Count == 0) return false;

            bool oppHasCard = Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
            // Prefer destroying Elara/Amalthe (trigger search/set on destroy)
            bool hasTriggerTarget = targets.Any(c => c.Id == CardId.DoomZVIISevenElara && !_elaraTriggerUsed)
                || targets.Any(c => c.Id == CardId.DoomZVFiveAmalthe && !_amaltheTriggerUsed);

            if (!oppHasCard && !hasTriggerTarget) return false;

            _zegredoPendUsed = true;
            return true;
        }

        private bool DoomZChangeEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_doomzChangeUsed) return false;

                var targets = Bot.Hand.Concat(Bot.GetMonsters()).Concat(Bot.Deck)
                    .Where(c => c != null && c.HasSetcode(0x1cb) && c.Id != CardId.DoomZChange)
                    .ToList();
                if (targets.Count == 0) return false;

                _doomzChangeUsed = true;
                return true;
            }
            return false;
        }

        private bool DoomZRaidersEffect()
        {
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                if (_doomzRaidersUsed) return false;

                var targets = Bot.Hand.Concat(Bot.GetMonsters())
                    .Where(c => c != null && c.HasSetcode(0x1cb) && c != Card)
                    .ToList();
                if (targets.Count == 0) return false;

                bool hasDeckTarget = HasRemainingMonsterWithSetcode(0x1cb);
                if (!hasDeckTarget) return false;

                // Raiders locks Extra Deck to Xyz only — don't activate if we still need non-Xyz ED
                bool needsNonXyzED = GetRemainingCount(CardId.EcclesiaAndTheDarkDragon) > 0 || GetRemainingCount(CardId.AlbionTheBrandedDragon) > 0;
                // If we have Fallen/Virtuous in hand and need Albion — don't lock
                bool hasFallenInHand = Bot.Hand.Any(c => c != null && c.Id == CardId.TheFallenAndTheVirtuous);
                if (needsNonXyzED && hasFallenInHand) return false;

                _doomzRaidersUsed = true;
                return true;
            }
            return false;
        }

        private bool PowerPatronDoomZFieldEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (IsSpecialSummonBlocked()) return false;

            var targets = Bot.GetMonsters()
                .Where(c => c != null && c != Card && c.IsFaceup() && c.IsMonster() && c.Level > 0)
                .ToList();
            
            bool hasLevel4Target = targets.Any(c => c.Level == 4) && GetRemainingCount(CardId.DoomZBreakDiactorus) > 0;
            bool hasLevel8Target = targets.Any(c => c.Level == 8) && GetRemainingCount(CardId.DoomZXIIEndDrastrius) > 0;
            bool hasLevel10Target = targets.Any(c => c.Level == 10) && GetRemainingCount(CardId.JupiterThePowerPatronOfDestruction) > 0;

            if (!hasLevel4Target && !hasLevel8Target && !hasLevel10Target) return false;

            return true;
        }

        private bool GraflarioEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_graflarioUsed) return false;
            if (Card.Overlays.Count == 0) return false;

            bool hasSearchTarget = HasRemainingCardWithSetcode(0x1cb);
            if (!hasSearchTarget) return false;

            _graflarioUsed = true;
            return true;
        }

        private bool JupiterEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_jupiterUsed) return false;
            if (Card.Overlays.Count == 0) return false;

            bool hasGYTarget = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.HasSetcode(0x1cb) && c.IsCanRevive());
            if (!hasGYTarget) return false;

            _jupiterUsed = true;
            return true;
        }

        private bool AdrasteiaGYEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_adrasteiaGYUsed) return false;

            bool hasMonster = Bot.GetMonsters().Any(c => c != null && c.IsFaceup());
            if (!hasMonster) return false;

            _adrasteiaGYUsed = true;
            return true;
        }

        private bool DoomZDestructionEffect()
        {
            if (Card.Location != CardLocation.SpellZone || !Card.IsFacedown()) return false;

            bool hasXyz = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Xyz) && c.HasSetcode(0x1cb));
            if (!hasXyz) return false;

            // Proactively activate during opponent's Standby Phase or when they chain/activate something
            if (Duel.Player == 1)
            {
                if (Duel.Phase == DuelPhase.Draw || Duel.Phase == DuelPhase.Standby) return true;
                if (Duel.LastChainPlayer == 1) return true;
            }

            // Also allow activation at the end of our turn to prepare
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.End) return true;

            return false;
        }

        // ==========================================
        //  Amalthe Hand SS
        // ==========================================

        private bool AmaltheHandSSEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (IsSpecialSummonBlocked()) return false;

            // SS from hand if we control a DoomZ card
            bool hasDoomzOnField = Bot.GetMonsters().Concat(Bot.GetSpells())
                .Any(c => c != null && c.IsFaceup() && c.HasSetcode(0x1cb));
            if (!hasDoomzOnField) return false;

            // Check free zone
            if (GetFreeMonsterZoneCount() == 0) return false;

            return true;
        }

        // ==========================================
        //  Normal Summon Conditions
        // ==========================================

        private bool ShouldSummonMedius()
        {
            // Medius is THE best Normal Summon — search/SS 1 Power Patron on summon
            bool hasTarget = HasRemainingMonsterWithSetcode(0x1c6);
            return hasTarget;
        }

        private bool ShouldSummonElara()
        {
            // Elara: Set 1 DoomZ Spell/Trap from Deck when summoned
            if (_elaraTriggerUsed) return false;
            bool hasTarget = HasRemainingCardWithSetcode(0x1cb);
            return hasTarget;
        }

        private bool ShouldSummonAmalthe()
        {
            // Amalthe: Add 1 DoomZ monster from Deck when summoned
            if (_amaltheTriggerUsed) return false;
            bool hasTarget = HasRemainingMonsterWithSetcode(0x1cb);
            return hasTarget;
        }

        private bool ShouldSummonZegredo()
        {
            // Zegredo: SS Jupiter from ED (banish 3 face-down from Deck)
            // Only NS as backup if Medius is not available
            bool hasMedius = Bot.Hand.Any(c => c != null && c.Id == CardId.MediusThePure);
            if (hasMedius) return false; // Don't NS Zegredo if Medius is in hand

            bool hasJupiter = GetRemainingCount(CardId.JupiterThePowerPatronOfDestruction) > 0;
            return hasJupiter && Bot.Deck.Count >= 3;
        }

        // ==========================================
        //  TIER 6 & 7: Summons
        // ==========================================

        private bool VidriumSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            
            // Only summon if going second (to disrupt opponent GY) or to close the game
            if (!_isGoingSecond && Bot.GetMonsterCount() > 0) return false;
            
            // Ensure we don't disrupt our own setup if we have a healthy GY of DoomZ and Jupiter still has materials
            bool hasActiveJupiter = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id == CardId.JupiterThePowerPatronOfDestruction && c.Overlays.Count > 0);
            if (hasActiveJupiter && Bot.Graveyard.Any(c => c.HasSetcode(0x1cb))) return false;

            return true;
        }

        private bool GraflarioSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            bool hasXyz = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Xyz) && c.HasSetcode(0x1cb));
            return hasXyz;
        }

        // ==========================================
        //  Selection overrides (OnSelectCard & OnSelectOption)
        // ==========================================

        public override int OnSelectOption(IList<long> options)
        {
            if (LastChainCard != null)
            {
                if (LastChainCard.Id == CardId.MediusThePure || LastChainCard.Id == CardId.NullPowerPatronRealmVidria)
                {
                    if (options.Contains(1) && GetFreeMonsterZoneCount() > 0)
                    {
                        return 1;
                    }
                    return 0;
                }

                if (LastChainCard.Id == CardId.TheFallenAndTheVirtuous)
                {
                    bool hasEcclesia = Bot.GetMonsters().Concat(Bot.Graveyard).Any(c => c != null && c.Id == CardId.EcclesiaAndTheDarkDragon);
                    bool hasGYTarget = Bot.Graveyard.Concat(Enemy.Graveyard).Any(c => c != null && c.IsMonster() && c.IsCanRevive() && !IsAceCard(c));
                    if (hasEcclesia && hasGYTarget && GetFreeMonsterZoneCount() > 0 && options.Contains(1))
                    {
                        return 1;
                    }
                    return 0;
                }
            }
            return base.OnSelectOption(options);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (hint == 509)
            {
                bool hasDeck = cards.Any(c => c.Location == CardLocation.Deck);
                if (hasDeck && cards.Any(c => c.Location == CardLocation.Hand))
                {
                    var targets = cards.Where(c => c.Location == CardLocation.Deck).ToList();
                    if (targets.Count >= min) return targets.Take(max).ToList();
                }
            }

            if (LastChainCard != null)
            {
                if (LastChainCard.Id == CardId.PowerPatronShadowMachineZegredo && hint == 509)
                {
                    var target = cards.OrderBy(c => GetHandPriority(c)).FirstOrDefault();
                    if (target != null) return new[] { target };
                }

                if (LastChainCard.Id == CardId.PowerPatronShadowMachineZegredo && hint == 502)
                {
                    // Prefer destroying Elara/Amalthe WITH unused triggers (get free search/set)
                    if (!_elaraTriggerUsed)
                    {
                        var elara = cards.FirstOrDefault(c => c.Id == CardId.DoomZVIISevenElara);
                        if (elara != null) return new[] { elara };
                    }
                    if (!_amaltheTriggerUsed)
                    {
                        var amalthe = cards.FirstOrDefault(c => c.Id == CardId.DoomZVFiveAmalthe);
                        if (amalthe != null) return new[] { amalthe };
                    }
                    // Fallback: any Elara/Amalthe (even used triggers — still bodies)
                    var preferred = cards.Where(c => c.Id == CardId.DoomZVIISevenElara || c.Id == CardId.DoomZVFiveAmalthe).ToList();
                    if (preferred.Count > 0) return new[] { preferred[0] };
                    
                    var oppCards = cards.Where(c => c.Controller == 1).ToList();
                    if (oppCards.Count > 0)
                    {
                        return new[] { oppCards.OrderByDescending(c => GetThreatScore(c)).First() };
                    }
                }

                if (LastChainCard.Id == CardId.DoomZChange && hint == 502)
                {
                    var deckCards = cards.Where(c => c.Location == CardLocation.Deck).ToList();
                    if (deckCards.Count > 0)
                    {
                        if (!_amaltheTriggerUsed)
                        {
                            var amalthe = deckCards.FirstOrDefault(c => c.Id == CardId.DoomZVFiveAmalthe);
                            if (amalthe != null) return new[] { amalthe };
                        }
                        if (!_elaraTriggerUsed)
                        {
                            var elara = deckCards.FirstOrDefault(c => c.Id == CardId.DoomZVIISevenElara);
                            if (elara != null) return new[] { elara };
                        }
                        return new[] { deckCards[0] };
                    }
                    var handCards = cards.Where(c => c.Location == CardLocation.Hand).ToList();
                    if (handCards.Count > 0)
                    {
                        if (!_amaltheTriggerUsed)
                        {
                            var amalthe = handCards.FirstOrDefault(c => c.Id == CardId.DoomZVFiveAmalthe);
                            if (amalthe != null) return new[] { amalthe };
                        }
                        if (!_elaraTriggerUsed)
                        {
                            var elara = handCards.FirstOrDefault(c => c.Id == CardId.DoomZVIISevenElara);
                            if (elara != null) return new[] { elara };
                        }
                        return new[] { handCards[0] };
                    }
                }

                if (LastChainCard.Id == CardId.DoomZRaiders && hint == 502)
                {
                    var preferred = cards.Where(c => c.Id == CardId.DoomZVFiveAmalthe || c.Id == CardId.DoomZVIISevenElara).ToList();
                    if (preferred.Count > 0) return new[] { preferred[0] };
                    
                    var oppMonsters = cards.Where(c => c.Controller == 1 && c.IsMonster()).ToList();
                    if (oppMonsters.Count > 0)
                    {
                        return new[] { oppMonsters.OrderByDescending(c => GetThreatScore(c)).First() };
                    }
                }

                if (LastChainCard.Id == CardId.PowerPatronDoomZ && hint == 501)
                {
                    var l10 = cards.Where(c => c.Level == 10).ToList();
                    if (l10.Count > 0) return new[] { l10[0] };
                    var l8 = cards.Where(c => c.Level == 8).ToList();
                    if (l8.Count > 0) return new[] { l8[0] };
                    var l4 = cards.Where(c => c.Level == 4).ToList();
                    if (l4.Count > 0) return new[] { l4[0] };
                }

                if ((LastChainCard.Id == CardId.DoomZVIISevenElara || LastChainCard.Id == CardId.DoomZVFiveAmalthe) && hint == 509)
                {
                    var target = cards.FirstOrDefault(c => c.Id == CardId.DoomZBreakDiactorus);
                    if (target != null) return new[] { target };
                }

                if (LastChainCard.Id == CardId.DoomZXIIZeroDrastea && hint == 509)
                {
                    var target = cards.FirstOrDefault(c => c.Id == CardId.DoomZXIIEndDrastrius);
                    if (target != null) return new[] { target };
                }

                if (LastChainCard.Id == CardId.DoomZXIIEndDrastrius)
                {
                    var oppMonsters = cards.Where(c => c.Controller == 1 && c.IsFaceup()).ToList();
                    if (oppMonsters.Count > 0)
                    {
                        return new[] { oppMonsters.OrderByDescending(c => c.Attack).First() };
                    }
                }

                if (LastChainCard.Id == CardId.DoomZXIIIOverGraflario && hint == 502)
                {
                    var oppCards = cards.Where(c => c.Controller == 1).ToList();
                    if (oppCards.Count > 0)
                    {
                        return new[] { oppCards.OrderByDescending(c => GetThreatScore(c)).First() };
                    }
                    var self = cards.FirstOrDefault(c => c.Id == CardId.DoomZXIIIOverGraflario);
                    if (self != null) return new[] { self };
                }

                if (LastChainCard.Id == CardId.JupiterThePowerPatronOfDestruction)
                {
                    if (hint == 509)
                    {
                        var target = cards.FirstOrDefault(c => c.IsMonster() && c.HasSetcode(0x1cb) && c.IsCanRevive());
                        if (target != null) return new[] { target };
                    }
                    if (hint == 502)
                    {
                        var oppCards = cards.Where(c => c.Controller == 1).ToList();
                        if (oppCards.Count > 0)
                        {
                            return new[] { oppCards.OrderByDescending(c => GetThreatScore(c)).First() };
                        }
                    }
                }

                if (LastChainCard.Id == CardId.NullPowerPatronRealmVidria)
                {
                    if (hint == 514)
                    {
                        var jupiter = cards.FirstOrDefault(c => c.Id == CardId.JupiterThePowerPatronOfDestruction);
                        if (jupiter != null) return new[] { jupiter };
                    }
                    if (hint == 502)
                    {
                        var best = cards.OrderBy(c => GetHandPriority(c)).FirstOrDefault();
                        if (best != null) return new[] { best };
                    }
                }

                if (LastChainCard.Id == CardId.TheFallenAndTheVirtuous)
                {
                    if (hint == 507)
                    {
                        var albion = cards.FirstOrDefault(c => c.Id == CardId.AlbionTheBrandedDragon);
                        if (albion != null) return new[] { albion };
                        var ecclesia = cards.FirstOrDefault(c => c.Id == CardId.EcclesiaAndTheDarkDragon);
                        if (ecclesia != null) return new[] { ecclesia };
                    }
                    if (hint == 509 || hint == 0)
                    {
                        var bosses = cards.Where(c => IsAceCard(c) || c.Id == CardId.PowerPatronShadowMachineZegredo).ToList();
                        if (bosses.Count > 0) return new[] { bosses[0] };
                        
                        var oppBosses = cards.Where(c => c.Controller == 1 && c.Attack >= 2500).ToList();
                        if (oppBosses.Count > 0) return new[] { oppBosses[0] };
                        
                        return new[] { cards[0] };
                    }
                }

                if (LastChainCard.Id == CardId.AlbionTheBrandedDragon)
                {
                    var target = cards.FirstOrDefault(c => c.Id == CardId.TheFallenAndTheVirtuous);
                    if (target != null) return new[] { target };
                }

                if (LastChainCard.Id == CardId.DoomZDestruction)
                {
                    var target = cards.FirstOrDefault(c => c.IsFaceup() && c.HasType(CardType.Xyz) && c.HasSetcode(0x1cb));
                    if (target != null) return new[] { target };
                }
            }

            if (Card == null)
            {
                return base.OnSelectCard(cards, min, max, hint, cancelable);
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.MediusThePure || cardId == CardId.PowerPatronShadowMachineZegredo ||
                cardId == CardId.DoomZVFiveAmalthe || cardId == CardId.DoomZVIISevenElara)
            {
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }
            return base.OnSelectPosition(cardId, positions);
        }
    }

    [Deck("Expert_2026_Doomz", "2026_Doomz")]
    public class ExpertDoomzExecutor : _2026_DoomzExecutor
    {
        private string _duelId;
        public ExpertDoomzExecutor(GameAI ai, Duel duel) : base(ai, duel)
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

    [Deck("Neural_2026_Doomz", "2026_Doomz")]
    public class NeuralDoomzExecutor : _2026_DoomzExecutor
    {
        public NeuralDoomzExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
        }
    }
}
