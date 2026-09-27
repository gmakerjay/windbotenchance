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
    // | Card Name                                 | Type    | OPT? | Cost            | Effect Summary                                |
    // |-------------------------------------------|---------|------|-----------------|-----------------------------------------------|
    // | DoomZ VII Seven - Elara (54265980)        | Monster | Yes  | None            | Summon/destroy: Set 1 DoomZ Spell/Trap        |
    // |                                           |         | Yes  | None (Quick)    | While equipped: SS Rank 4 Diactorus           |
    // | DoomZ V Five - Amalthe (28877382)         | Monster | Yes  | None            | Summon/destroy: Add 1 DoomZ monster           |
    // |                                           |         | Yes  | None            | Hand: SS if control DoomZ                     |
    // |                                           |         | Yes  | None (Quick)    | While equipped: SS Rank 4 Diactorus           |
    // | DoomZ XII Zero - Drastea (92472273)       | Monster | Yes  | Pop 1 DoomZ card| Hand: SS itself, equip 1 Equip from Deck      |
    // |                                           |         | Yes  | None (Quick)    | While equipped: SS Rank 8 Drastrius           |
    // | Power Patron DoomZ (81650695)             | Monster | Yes  | None            | Hand: SS if control DoomZ/Patron              |
    // |                                           |         | Yes  | Pop 1 DoomZ/PP  | Field: Pop 1 DoomZ/Power Patron -> draw 2     |
    // |                                           |         | Yes  | None            | Destroyed: search 1 DoomZ card                |
    // | Medius the Pure (97556336)                | Monster | Yes  | None            | Summon: search/SS 1 Power Patron from Deck    |
    // |                                           |         | Yes  | Shuffle monster | GY: SS itself (banish when leaves)            |
    // | Vidrium ... Chaos Extermination (70488851)| Monster | Yes  | None            | Hand: SS if control Power Patron              |
    // |                                           |         | Yes  | Banish from GY  | GY: Banish 1 monster opponent controls        |
    // | Power Patron Shadow Machine Zegredo (43871165)| Monster| Yes| Banish 3 FD   | Monster: SS Jupiter from ED, attach 1 hand    |
    // |                                           |         | Yes  | None            | ED face-up: Search 1 DoomZ/PP Spell/Trap      |
    // |                                           |         | Yes  | Pop self + card | Pendulum: destroy self + card -> pop enemy    |
    // | DoomZ Change (38007649)                   | Spell   | Yes  | None            | Destroy 1 DoomZ in hand/Deck/field            |
    // |                                           |         | Yes  | None            | Destroyed: add 1 DoomZ from GY, SS 1 hand     |
    // | DoomZ Raiders (32442017)                  | Trap    | Yes  | Pop 1 DoomZ     | Continuous: destroy 1 DoomZ -> search/SS DoomZ|
    // |                                           |         | Yes  | None            | Destroyed: destroy 1 face-up monster on field |
    // | DoomZ Command "D.O.O.M.D.U.R.G." (68831625)| Spell   | Yes  | None            | Equip: Untargetable, direct attack + ATK boost|
    // | DoomZ Command "A.D.R.A.S.T.E.I.A." (84054556)| Spell  | Yes  | None            | Equip: Battle protect, pop -> SS DoomZ        |
    // |                                           |         | Yes  | None            | GY: re-equip to face-up monster               |
    // | Null Power Patron Realm - Vidria (51669847)| Spell  | Yes  | Banish 1 hand FD| Field: Reveal PP in ED -> search/SS mention   |
    // | Unleashed Power Patron Portal - Terminus (25661743)| Spell| Yes| None          | Send PP from Deck/ED -> search DARK Fairy     |
    // | The Fallen & The Virtuous (30271097)      | Spell   | Yes  | Send Albaz ED   | Pop 1 face-up card or revive                  |
    // | DoomZ Destruction (80320877)              | Trap    | Yes  | None            | Equip to DoomZ Xyz -> lock opponent searching |
    // ============================================================

    [Deck("2026_Doomz", "2026_Doomz")]
    public class _2026_DoomzExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Custom / Engine Cards
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
            public const int AshBlossom = 14558127; // Fixed ID typo (was 14558128)

            // Extra Deck Cards
            public const int DoomZBreakDiactorus = 31010081;
            public const int DoomZXIIEndDrastrius = 95626382;
            public const int DoomZXIIIOverGraflario = 65848113;
            public const int JupiterThePowerPatronOfDestruction = 68231287;
            public const int VarudrasTheFinalBringerOfTheEndTimes = 70636044;
            public const int GiganticChampionSargas = 11132674;
            public const int SpringansMerrymaker = 48285768;
            public const int NumberF0UtopicFutureZexal = 41522092;
            public const int EcclesiaAndTheDarkDragon = 78397661;
            public const int AlbionTheBrandedDragon = 87746184;

            // Side Deck Cards
            public const int MulcharmyFuwalos = 42141493;
            public const int CalledByTheGrave = 24224830;
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
        private bool _vidriumHandUsed = false;
        private bool _vidriumGYUsed = false;

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.JupiterThePowerPatronOfDestruction
                || card.Id == CardId.DoomZXIIEndDrastrius
                || card.Id == CardId.DoomZXIIIOverGraflario
                || card.Id == CardId.VarudrasTheFinalBringerOfTheEndTimes
                || card.Id == CardId.NumberF0UtopicFutureZexal
                || card.Id == CardId.GiganticChampionSargas
                || card.Id == CardId.DoomZBreakDiactorus;
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.Id == CardId.AshBlossom || c.Id == CardId.MulcharmyFuwalos)
                return 800;
            return base.GetMaterialPriority(c);
        }

        private bool HasLethalOnBoard()
        {
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
            if (Enemy.GetMonsterCount() > 0)
            {
                int enemyDefAtk = Enemy.GetMonsters().Sum(m => m.IsAttack() ? m.Attack : m.Defense);
                int botAtk = Bot.GetMonsters().Where(m => m.IsFaceup() && m.IsAttack()).Sum(m => m.Attack);
                return (botAtk - enemyDefAtk) >= Enemy.LifePoints;
            }
            int totalAtk = Bot.GetMonsters().Where(m => m.IsFaceup() && m.IsAttack()).Sum(m => m.Attack);
            return totalAtk >= Enemy.LifePoints;
        }

        protected override bool IsBoardStrongEnough()
        {
            if (HasLethalOnBoard()) return true;

            int disruption = 0;
            if (Bot.HasInMonstersZone(CardId.JupiterThePowerPatronOfDestruction)) disruption += 2;
            if (Bot.HasInMonstersZone(CardId.DoomZXIIEndDrastrius)) disruption += 2;
            if (Bot.HasInMonstersZone(CardId.VarudrasTheFinalBringerOfTheEndTimes)) disruption += 2;
            if (Bot.HasInMonstersZone(CardId.NumberF0UtopicFutureZexal)) disruption += 2;
            if (Bot.HasInMonstersZone(CardId.DoomZXIIIOverGraflario)) disruption++;
            if (Bot.HasInMonstersZone(CardId.GiganticChampionSargas)) disruption++;
            if (Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.DoomZDestruction))) disruption += 2;
            if (Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.DoomZRaiders))) disruption++;
            if (Bot.HasInHand(CardId.AshBlossom)) disruption++;

            return disruption >= 3;
        }

        protected override bool ShouldStopExtending()
        {
            if (HasLethalOnBoard()) return true;
            if (IsBoardStrongEnough() && !IsInGrindGame()) return true;
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
                CardId.NumberF0UtopicFutureZexal,
                CardId.GiganticChampionSargas,
                CardId.DoomZBreakDiactorus
            );

            // ── Combo Router: Sequencing ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Vidria-To-Jupiter",
                RequiredCards = new List<int> { CardId.NullPowerPatronRealmVidria },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.NullPowerPatronRealmVidria, ActionType = ExecutorType.Activate, Description = "Activate Vidria to search Zegredo" },
                    new() { CardId = CardId.PowerPatronShadowMachineZegredo, ActionType = ExecutorType.Activate, Description = "Zegredo summons Jupiter" },
                    new() { CardId = CardId.JupiterThePowerPatronOfDestruction, ActionType = ExecutorType.SpSummon, Description = "Summon Jupiter" }
                },
                EndBoardScore = 90
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Medius-To-Jupiter",
                RequiredCards = new List<int> { CardId.MediusThePure },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.MediusThePure, ActionType = ExecutorType.Summon, Description = "Normal Summon Medius" },
                    new() { CardId = CardId.MediusThePure, ActionType = ExecutorType.Activate, Description = "Medius SS Zegredo from Deck" },
                    new() { CardId = CardId.PowerPatronShadowMachineZegredo, ActionType = ExecutorType.Activate, Description = "Zegredo summons Jupiter" }
                },
                EndBoardScore = 85
            });

            // ── Bait Planner ──
            BaitPlanner.RegisterComboStarters(CardId.MediusThePure, CardId.DoomZVFiveAmalthe, CardId.NullPowerPatronRealmVidria, CardId.PowerPatronDoomZ, CardId.DoomZVIISevenElara, CardId.DoomZChange);
            BaitPlanner.RegisterBaitCards(CardId.DoomZVFiveAmalthe, CardId.PowerPatronDoomZ);

            // ── Chain Advisor ──
            ChainAdvisor.RegisterHighValueTargets(CardId.MediusThePure, CardId.PowerPatronShadowMachineZegredo, CardId.NullPowerPatronRealmVidria);

            // ── 1. Hand Traps & Negations ──
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);

            // ── 2. Boss Disruptions & Quick Effects ──
            AddExecutor(ExecutorType.Activate, CardId.DoomZXIIEndDrastrius, DrastriusNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.VarudrasTheFinalBringerOfTheEndTimes, VarudrasEffect);
            AddExecutor(ExecutorType.Activate, CardId.NumberF0UtopicFutureZexal, UtopicFutureZexalEffect);
            AddExecutor(ExecutorType.Activate, CardId.GiganticChampionSargas, SargasEffect);
            AddExecutor(ExecutorType.Activate, CardId.DoomZVIISevenElara, ElaraQuickEffect);
            AddExecutor(ExecutorType.Activate, CardId.DoomZVFiveAmalthe, AmaltheQuickEffect);
            AddExecutor(ExecutorType.Activate, CardId.DoomZXIIZeroDrastea, DrasteaQuickEffect);
            AddExecutor(ExecutorType.Activate, CardId.JupiterThePowerPatronOfDestruction, JupiterEffect);
            AddExecutor(ExecutorType.Activate, CardId.DoomZXIIIOverGraflario, GraflarioEffect);
            AddExecutor(ExecutorType.Activate, CardId.VidriumThePowerPatronOfChaosExtermination, VidriumGYEffect);

            // ── 3. Spells (Removal & Setup) ──
            AddExecutor(ExecutorType.Activate, CardId.TheFallenAndTheVirtuous, TheFallenAndTheVirtuousEffect);
            AddExecutor(ExecutorType.Activate, CardId.AlbionTheBrandedDragon, AlbionEffect);

            // ── 4. Search & Engine Spells ──
            AddExecutor(ExecutorType.Activate, CardId.NullPowerPatronRealmVidria, NullPowerPatronRealmEffect);
            AddExecutor(ExecutorType.Activate, CardId.UnleashedPowerPatronPortalTerminus, TerminusEffect);
            AddExecutor(ExecutorType.Activate, CardId.DoomZChange, DoomZChangeEffect);

            // ── 5. Monster Effects (Hand / Field / Trigger) ──
            AddExecutor(ExecutorType.Activate, CardId.MediusThePure, MediusSummonEffect);
            AddExecutor(ExecutorType.Activate, CardId.MediusThePure, MediusGYEffect);
            AddExecutor(ExecutorType.Activate, CardId.DoomZXIIZeroDrastea, DrasteaHandSummon);
            AddExecutor(ExecutorType.Activate, CardId.DoomZVFiveAmalthe, AmaltheHandSSEffect);

            // Pendulum Zone activations & effects
            AddExecutor(ExecutorType.Activate, CardId.PowerPatronShadowMachineZegredo, ZegredoPendulumActivate);
            AddExecutor(ExecutorType.Activate, CardId.PowerPatronShadowMachineZegredo, ZegredoPendulumEffect);
            AddExecutor(ExecutorType.Activate, CardId.PowerPatronShadowMachineZegredo, ZegredoMonsterEffect);

            // Monster trigger effects (Summoned or Destroyed)
            AddExecutor(ExecutorType.Activate, CardId.DoomZVIISevenElara, ElaraTriggerEffect);
            AddExecutor(ExecutorType.Activate, CardId.DoomZVFiveAmalthe, AmaltheTriggerEffect);
            AddExecutor(ExecutorType.Activate, CardId.PowerPatronDoomZ, PowerPatronDoomZDestroyedEffect);
            AddExecutor(ExecutorType.Activate, CardId.DoomZChange, DoomZChangeGYEffect);
            AddExecutor(ExecutorType.Activate, CardId.DoomZRaiders, DoomZRaidersEffect);
            AddExecutor(ExecutorType.Activate, CardId.PowerPatronDoomZ, PowerPatronDoomZFieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.DoomZCommandADRASTEIA, AdrasteiaGYEffect);
            AddExecutor(ExecutorType.Activate, CardId.DoomZDestruction, DoomZDestructionEffect);

            // ── 6. Special Summons from Hand ──
            AddExecutor(ExecutorType.SpSummon, CardId.VidriumThePowerPatronOfChaosExtermination, VidriumSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DoomZVFiveAmalthe);

            // ── 7. Normal Summons ──
            AddExecutor(ExecutorType.Summon, CardId.MediusThePure, ShouldSummonMedius);
            AddExecutor(ExecutorType.Summon, CardId.DoomZVIISevenElara, ShouldSummonElara);
            AddExecutor(ExecutorType.Summon, CardId.DoomZVFiveAmalthe, ShouldSummonAmalthe);
            AddExecutor(ExecutorType.Summon, CardId.PowerPatronShadowMachineZegredo, ShouldSummonZegredo);

            // Fallback Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.MediusThePure);
            AddExecutor(ExecutorType.Summon, CardId.DoomZVFiveAmalthe);
            AddExecutor(ExecutorType.Summon, CardId.DoomZVIISevenElara);
            AddExecutor(ExecutorType.Summon, CardId.PowerPatronShadowMachineZegredo);
            AddExecutor(ExecutorType.Summon, CardId.PowerPatronDoomZ);

            // ── 8. Extra Deck Summons ──
            AddExecutor(ExecutorType.SpSummon, CardId.NumberF0UtopicFutureZexal, UtopicFutureZexalSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.VarudrasTheFinalBringerOfTheEndTimes, VarudrasSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.GiganticChampionSargas, SargasSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.JupiterThePowerPatronOfDestruction);
            AddExecutor(ExecutorType.SpSummon, CardId.DoomZXIIEndDrastrius, DrastriusSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DoomZXIIIOverGraflario, GraflarioSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DoomZBreakDiactorus, DiactorusSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SpringansMerrymaker, MerrymakerSummon);

            // Extra Deck trigger / activation
            AddExecutor(ExecutorType.Activate, CardId.DoomZBreakDiactorus);
            AddExecutor(ExecutorType.Activate, CardId.SpringansMerrymaker);

            // ── 9. Sets & Repos ──
            AddExecutor(ExecutorType.SpellSet, CardId.DoomZDestruction);
            AddExecutor(ExecutorType.SpellSet, CardId.DoomZRaiders);
            AddExecutor(ExecutorType.SpellSet, CardId.TheFallenAndTheVirtuous);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave);

            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // Control / Xyz setup — strongly prefer going first
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
            _vidriumHandUsed = false;
            _vidriumGYUsed = false;
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
            // Higher value = KEEP, Lower value = DISCARD/BANISH cost
            if (c.Id == CardId.AshBlossom || c.Id == CardId.MulcharmyFuwalos) return 50;
            if (c.Id == CardId.CalledByTheGrave) return 100;
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
                if (card.IsFaceup() && !card.IsDisabled())
                {
                    if (card.Attack >= 2500) score += 3000;
                    if (card.HasType(CardType.Effect)) score += 2000;
                }
            }
            else if (card.IsSpell() || card.IsTrap())
            {
                score += 500;
                if (card.IsFaceup()) score += 2000;
            }
            return score;
        }

        private bool IsTargetable(ClientCard card)
        {
            if (card == null) return false;
            if (card.IsCode(55410871)) return false; // Blue-Eyes Chaos MAX Dragon
            return !card.IsShouldNotBeTarget();
        }

        protected override bool IsViableEffectTarget(ClientCard card)
        {
            if (card == null) return false;
            if (!IsTargetable(card)) return false;

            // Eternal Soul immunity check
            bool isEternalSoulActive = Enemy.GetSpells().Any(s => s != null && s.IsFaceup() && s.IsCode(48680970) && !s.IsDisabled());
            if (isEternalSoulActive && (card.IsCode(46986414) || card.IsCode(41721210)))
                return false;

            return true;
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

        private bool CalledByTheGraveEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;

            bool opponentHandTrapChained = LastChainCard != null && 
                (LastChainCard.Id == CardId.AshBlossom || 
                 LastChainCard.Id == CardId.MulcharmyFuwalos ||
                 LastChainCard.Id == 23434538 || // Maxx C
                 LastChainCard.Id == 97268402);   // Effect Veiler

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

        private bool VarudrasEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.Overlays.Count == 0) return false;
            if (LastChainCard != null && LastChainCard.Controller == 1)
            {
                return true;
            }
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Damage)
            {
                return true;
            }
            return false;
        }

        private bool UtopicFutureZexalEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.Overlays.Count == 0) return false;
            if (LastChainCard != null && LastChainCard.Controller == 1)
            {
                var target = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c)).OrderByDescending(c => c.Attack).FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool SargasEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.Overlays.Count == 0) return false;

            var oppTargets = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                .Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c))
                .OrderByDescending(c => GetThreatScore(c))
                .ToList();
            if (oppTargets.Count > 0)
            {
                AI.SelectCard(oppTargets[0]);
                return true;
            }
            return false;
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

        private bool VidriumGYEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_vidriumGYUsed) return false;

            var target = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c)).OrderByDescending(c => c.Attack).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                _vidriumGYUsed = true;
                return true;
            }
            return false;
        }

        // ==========================================
        //  TIER 3 & 4: Spells & Searches
        // ==========================================

        private bool TheFallenAndTheVirtuousEffect()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                if (_fallenUsed) return false;

                bool hasAlbion = GetRemainingCount(CardId.AlbionTheBrandedDragon) > 0;
                bool hasFaceupTarget = Enemy.GetMonsters().Concat(Enemy.GetSpells()).Any(c => c != null && c.IsFaceup() && IsViableEffectTarget(c));
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

                bool hasSendTarget = HasRemainingMonsterWithSetcode(0x1c6) || GetRemainingCount(CardId.JupiterThePowerPatronOfDestruction) > 0;
                if (!hasSendTarget) return false;

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
            if (HasLethalOnBoard()) return false;

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
            if (HasLethalOnBoard()) return false;

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
            if (HasLethalOnBoard()) return false;

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
                if (HasLethalOnBoard()) return false;

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
                if (HasLethalOnBoard()) return false;

                var targets = Bot.Hand.Concat(Bot.GetMonsters())
                    .Where(c => c != null && c.HasSetcode(0x1cb) && c != Card)
                    .ToList();
                if (targets.Count == 0) return false;

                bool hasDeckTarget = HasRemainingMonsterWithSetcode(0x1cb);
                if (!hasDeckTarget) return false;

                bool needsNonXyzED = GetRemainingCount(CardId.EcclesiaAndTheDarkDragon) > 0 || GetRemainingCount(CardId.AlbionTheBrandedDragon) > 0;
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
            // Pop 1 DoomZ or Power Patron to draw 2 cards
            var targets = Bot.GetMonsters()
                .Where(c => c != null && (c.HasSetcode(0x1cb) || c.HasSetcode(0x1c6)) && !IsAceCard(c))
                .ToList();
            if (targets.Count == 0) return false;

            AI.SelectCard(targets);
            return true;
        }

        private bool GraflarioEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_graflarioUsed) return false;
            if (Card.Overlays.Count == 0) return false;

            var oppMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c)).ToList();
            if (oppMonsters.Count == 0) return false;

            var ourDoomz = Bot.GetMonsters().Where(c => c != null && c.HasSetcode(0x1cb)).ToList();
            if (ourDoomz.Count == 0) return false;

            _graflarioUsed = true;
            return true;
        }

        private bool JupiterEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_jupiterUsed) return false;
            if (Card.Overlays.Count == 0) return false;

            var oppTargets = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                .Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c))
                .OrderByDescending(c => GetThreatScore(c))
                .ToList();
            if (oppTargets.Count > 0)
            {
                AI.SelectCard(oppTargets[0]);
                _jupiterUsed = true;
                return true;
            }

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

            if (Duel.Player == 1)
            {
                if (Duel.Phase == DuelPhase.Draw || Duel.Phase == DuelPhase.Standby) return true;
                if (Duel.LastChainPlayer == 1) return true;
            }

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
            if (HasLethalOnBoard()) return false;

            bool hasDoomzOnField = Bot.GetMonsters().Concat(Bot.GetSpells())
                .Any(c => c != null && c.IsFaceup() && c.HasSetcode(0x1cb));
            if (!hasDoomzOnField) return false;

            if (GetFreeMonsterZoneCount() == 0) return false;

            return true;
        }

        // ==========================================
        //  Normal Summon Conditions
        // ==========================================

        private bool ShouldSummonMedius()
        {
            bool hasTarget = HasRemainingMonsterWithSetcode(0x1c6);
            return hasTarget;
        }

        private bool ShouldSummonElara()
        {
            if (_elaraTriggerUsed) return false;
            bool hasTarget = HasRemainingCardWithSetcode(0x1cb);
            return hasTarget;
        }

        private bool ShouldSummonAmalthe()
        {
            if (_amaltheTriggerUsed) return false;
            bool hasTarget = HasRemainingMonsterWithSetcode(0x1cb);
            return hasTarget;
        }

        private bool ShouldSummonZegredo()
        {
            bool hasMedius = Bot.Hand.Any(c => c != null && c.Id == CardId.MediusThePure);
            if (hasMedius) return false;

            bool hasJupiter = GetRemainingCount(CardId.JupiterThePowerPatronOfDestruction) > 0;
            return hasJupiter && Bot.Deck.Count >= 3;
        }

        // ==========================================
        //  TIER 6 & 7: Summons
        // ==========================================

        private bool VidriumSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (_vidriumHandUsed) return false;
            if (HasLethalOnBoard()) return false;

            bool hasPP = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasSetcode(0x1c6));
            if (!hasPP) return false;

            _vidriumHandUsed = true;
            return true;
        }

        private bool DrastriusSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (HasLethalOnBoard()) return false;
            if (IsBoardStrongEnough() && !IsInGrindGame()) return false;
            return true;
        }

        private bool DiactorusSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (HasLethalOnBoard()) return false;
            if (IsBoardStrongEnough() && !IsInGrindGame()) return false;
            return true;
        }

        private bool GraflarioSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (HasLethalOnBoard()) return false;
            bool hasXyz = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Xyz) && c.HasSetcode(0x1cb));
            return hasXyz;
        }

        private bool VarudrasSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (HasLethalOnBoard()) return false;
            return true;
        }

        private bool SargasSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (HasLethalOnBoard()) return false;
            bool hasMerrymaker = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id == CardId.SpringansMerrymaker);
            return hasMerrymaker;
        }

        private bool MerrymakerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (HasLethalOnBoard()) return false;
            return true;
        }

        private bool UtopicFutureZexalSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (HasLethalOnBoard()) return false;
            return true;
        }

        // ==========================================
        //  Selection overrides
        // ==========================================

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0) return 0;

            for (int i = 0; i < options.Count; i++)
            {
                long cardId = options[i] >> 4;
                if (cardId == 0 && Card != null) cardId = Card.Id;
                long optIndex = options[i] & 0xf;

                // Medius the Pure: Option 0: Add to Hand, Option 1: Special Summon.
                if (cardId == CardId.MediusThePure)
                {
                    if (optIndex == 1 && GetFreeMonsterZoneCount() > 0 && !IsSpecialSummonBlocked())
                        return i;
                    if (optIndex == 0)
                        return i;
                }

                // Null Power Patron Realm - Vidria: Option 0: Add to Hand, Option 1: Special Summon.
                if (cardId == CardId.NullPowerPatronRealmVidria)
                {
                    if (optIndex == 1 && GetFreeMonsterZoneCount() > 0 && !IsSpecialSummonBlocked())
                        return i;
                    if (optIndex == 0)
                        return i;
                }

                // DoomZ Raiders: Option 0: Add to Hand, Option 1: Special Summon.
                if (cardId == CardId.DoomZRaiders)
                {
                    if (optIndex == 1 && GetFreeMonsterZoneCount() > 0 && !IsSpecialSummonBlocked())
                        return i;
                    if (optIndex == 0)
                        return i;
                }

                // The Fallen & The Virtuous:
                if (cardId == CardId.TheFallenAndTheVirtuous)
                {
                    bool hasEnemyFaceup = Enemy.GetMonsters().Concat(Enemy.GetSpells()).Any(c => c != null && c.IsFaceup() && IsTargetable(c));
                    bool hasAlbion = GetRemainingCount(CardId.AlbionTheBrandedDragon) > 0;
                    if (hasEnemyFaceup && hasAlbion && optIndex == 0)
                        return i;

                    bool hasEcclesia = Bot.GetMonsters().Concat(Bot.Graveyard).Any(c => c != null && c.Id == CardId.EcclesiaAndTheDarkDragon);
                    bool hasGYTarget = Bot.Graveyard.Concat(Enemy.Graveyard).Any(c => c != null && c.IsMonster() && c.IsCanRevive());
                    if (hasEcclesia && hasGYTarget && optIndex == 1 && GetFreeMonsterZoneCount() > 0)
                        return i;

                    if (hasEnemyFaceup && optIndex == 0)
                        return i;
                }
            }

            return base.OnSelectOption(options);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Low ATK utility monsters & tuners: FaceUpDefence
            int[] lowStatMonsters = {
                CardId.PowerPatronShadowMachineZegredo, // 300 ATK
                CardId.PowerPatronDoomZ,               // 300 ATK
                CardId.AshBlossom,                     // 0 ATK
                CardId.MulcharmyFuwalos,               // 100 ATK
                CardId.SpringansMerrymaker             // 1100 ATK / 2000 DEF
            };

            if (lowStatMonsters.Contains(cardId) && positions.Contains(CardPosition.FaceUpDefence))
            {
                return CardPosition.FaceUpDefence;
            }

            // High ATK beaters and bosses: FaceUpAttack
            int[] bossAttackMonsters = {
                CardId.JupiterThePowerPatronOfDestruction, // 3500 ATK
                CardId.DoomZXIIEndDrastrius,              // 2700 ATK
                CardId.VarudrasTheFinalBringerOfTheEndTimes, // 3000 ATK
                CardId.GiganticChampionSargas,            // 2800 ATK
                CardId.VidriumThePowerPatronOfChaosExtermination, // 5000 ATK
                CardId.DoomZXIIIOverGraflario,            // 2400 ATK
                CardId.DoomZBreakDiactorus,               // 2400 ATK
                CardId.DoomZXIIZeroDrastea,               // 2400 ATK
                CardId.MediusThePure,                     // 1800 ATK
                CardId.DoomZVFiveAmalthe,                 // 1600 ATK
                CardId.DoomZVIISevenElara                 // 1400 ATK
            };

            if (bossAttackMonsters.Contains(cardId) && positions.Contains(CardPosition.FaceUpAttack))
            {
                return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // ── Hint 506: Add to hand / Search selection (HINTMSG_ATOHAND) ──
            if (hint == 506)
            {
                var sortedSearch = cards.Where(c => c != null && c.Controller == 0).OrderBy(c => {
                    if (c.IsCode(CardId.MediusThePure)) return 0;
                    if (c.IsCode(CardId.PowerPatronShadowMachineZegredo)) return 1;
                    if (c.IsCode(CardId.DoomZVFiveAmalthe)) return 2;
                    if (c.IsCode(CardId.DoomZVIISevenElara)) return 3;
                    if (c.IsCode(CardId.NullPowerPatronRealmVidria)) return 4;
                    if (c.IsCode(CardId.DoomZChange)) return 5;
                    if (c.IsCode(CardId.DoomZXIIZeroDrastea)) return 6;
                    if (c.IsCode(CardId.PowerPatronDoomZ)) return 7;
                    if (c.IsCode(CardId.DoomZCommandADRASTEIA)) return 8;
                    if (c.IsCode(CardId.DoomZRaiders)) return 9;
                    if (c.IsCode(CardId.TheFallenAndTheVirtuous)) return 10;
                    if (c.IsCode(CardId.VidriumThePowerPatronOfChaosExtermination)) return 11;
                    return 20;
                }).ToList();
                if (sortedSearch.Count >= min)
                    return sortedSearch.Take(max).ToList();
            }

            // ── Hint 501: Discard / Send from Hand (HINTMSG_DISCARD) ──
            if (hint == 501)
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (IsAceCard(c)) return 900;
                    if (c.IsCode(CardId.DoomZCommandADRASTEIA)) return 10;
                    if (c.IsCode(CardId.DoomZChange)) return 20;
                    if (c.IsCode(CardId.VidriumThePowerPatronOfChaosExtermination)) return 25;
                    if (c.IsCode(CardId.NullPowerPatronRealmVidria, CardId.UnleashedPowerPatronPortalTerminus, CardId.TheFallenAndTheVirtuous) &&
                        Bot.Hand.Count(h => h.IsCode(c.Id)) > 1) return 30;
                    if (c.IsCode(CardId.PowerPatronDoomZ)) return 40;
                    return 50;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // ── Hint 504 / 508: Send to GY selection (HINTMSG_TOGRAVE) ──
            if (hint == 504 || hint == 508)
            {
                if (Card != null && Card.Id == CardId.UnleashedPowerPatronPortalTerminus)
                {
                    var jupiter = cards.FirstOrDefault(c => c != null && c.Id == CardId.JupiterThePowerPatronOfDestruction);
                    if (jupiter != null) return new[] { jupiter };

                    var vidrium = cards.FirstOrDefault(c => c != null && c.Id == CardId.VidriumThePowerPatronOfChaosExtermination);
                    if (vidrium != null) return new[] { vidrium };

                    var patronDoomz = cards.FirstOrDefault(c => c != null && c.Id == CardId.PowerPatronDoomZ);
                    if (patronDoomz != null) return new[] { patronDoomz };
                }

                if (Card != null && Card.Id == CardId.TheFallenAndTheVirtuous)
                {
                    var albion = cards.FirstOrDefault(c => c != null && c.Id == CardId.AlbionTheBrandedDragon);
                    if (albion != null) return new[] { albion };
                    var ecclesia = cards.FirstOrDefault(c => c != null && c.Id == CardId.EcclesiaAndTheDarkDragon);
                    if (ecclesia != null) return new[] { ecclesia };
                }

                var albionGeneral = cards.FirstOrDefault(c => c != null && c.Id == CardId.AlbionTheBrandedDragon);
                if (albionGeneral != null) return new[] { albionGeneral };
            }

            // ── Hint 502: Destruction target selection (HINTMSG_DESTROY) ──
            if (hint == 502)
            {
                bool isSelfPopCost = Card != null && (Card.Id == CardId.DoomZChange || Card.Id == CardId.PowerPatronDoomZ ||
                                                      Card.Id == CardId.DoomZXIIZeroDrastea || Card.Id == CardId.DoomZRaiders);

                if (isSelfPopCost || cards.All(c => c.Controller == 0))
                {
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
                    if (!_changeGYUsed)
                    {
                        var change = cards.FirstOrDefault(c => c.Id == CardId.DoomZChange);
                        if (change != null) return new[] { change };
                    }
                    if (!_patronDoomzTriggerUsed)
                    {
                        var patron = cards.FirstOrDefault(c => c.Id == CardId.PowerPatronDoomZ);
                        if (patron != null) return new[] { patron };
                    }
                    var preferred = cards.Where(c => c.Controller == 0 && (c.Id == CardId.DoomZVIISevenElara || c.Id == CardId.DoomZVFiveAmalthe)).ToList();
                    if (preferred.Count >= min) return preferred.Take(max).ToList();
                }

                // Opponent destruction targeting
                var opponentTargets = cards.Where(c => c != null && c.Controller == 1 && IsViableEffectTarget(c)).ToList();
                if (opponentTargets.Count >= min)
                {
                    var sorted = opponentTargets.OrderByDescending(c => GetThreatScore(c)).ToList();
                    return sorted.Take(max).ToList();
                }

                var ourSafe = cards.Where(c => c != null && c.Controller == 0 && !IsAceCard(c)).ToList();
                if (ourSafe.Count >= min) return ourSafe.Take(max).ToList();
            }

            // ── Hint 503: Banish target selection (HINTMSG_REMOVE) ──
            if (hint == 503)
            {
                var opponentTargets = cards.Where(c => c != null && c.Controller == 1 && IsViableEffectTarget(c)).ToList();
                if (opponentTargets.Count >= min)
                {
                    var sorted = opponentTargets.OrderByDescending(c => GetThreatScore(c)).ToList();
                    return sorted.Take(max).ToList();
                }

                var safeHand = cards.Where(c => c != null && c.Controller == 0).OrderBy(c => GetHandPriority(c)).ToList();
                if (safeHand.Count >= min) return safeHand.Take(max).ToList();
            }

            // ── Hint 505: Bounce target selection (HINTMSG_RTOHAND) ──
            if (hint == 505)
            {
                var opponentMonsters = cards.Where(c => c != null && c.Controller == 1 && IsViableEffectTarget(c)).ToList();
                if (opponentMonsters.Count >= min)
                {
                    var sorted = opponentMonsters.OrderByDescending(c => GetThreatScore(c)).ToList();
                    return sorted.Take(max).ToList();
                }

                var ourMonsters = cards.Where(c => c != null && c.Controller == 0 && !IsAceCard(c)).ToList();
                if (ourMonsters.Count >= min) return ourMonsters.Take(max).ToList();
            }

            // ── Hint 509: Special Summon / Revival selection (HINTMSG_SPSUMMON) ──
            if (hint == 509)
            {
                bool hasDeck = cards.Any(c => c.Location == CardLocation.Deck);
                if (hasDeck && cards.Any(c => c.Location == CardLocation.Hand))
                {
                    var deckTargets = cards.Where(c => c.Location == CardLocation.Deck).ToList();
                    if (deckTargets.Count >= min) cards = deckTargets;
                }

                if (Card != null && (Card.Id == CardId.DoomZVIISevenElara || Card.Id == CardId.DoomZVFiveAmalthe))
                {
                    var diactorus = cards.FirstOrDefault(c => c.Id == CardId.DoomZBreakDiactorus);
                    if (diactorus != null) return new[] { diactorus };
                }

                if (Card != null && Card.Id == CardId.DoomZXIIZeroDrastea)
                {
                    var drastrius = cards.FirstOrDefault(c => c.Id == CardId.DoomZXIIEndDrastrius);
                    if (drastrius != null) return new[] { drastrius };
                }

                if (Card != null && Card.Id == CardId.PowerPatronShadowMachineZegredo)
                {
                    var jupiter = cards.FirstOrDefault(c => c.Id == CardId.JupiterThePowerPatronOfDestruction);
                    if (jupiter != null) return new[] { jupiter };
                }

                var sorted = cards.OrderByDescending(c => {
                    if (c == null) return -999;
                    int score = (c.Controller == 0) ? 1000 : 500;
                    if (IsAceCard(c)) score += 5000;
                    if (c.IsCode(CardId.JupiterThePowerPatronOfDestruction, CardId.DoomZXIIEndDrastrius, CardId.VarudrasTheFinalBringerOfTheEndTimes)) score += 3000;
                    if (c.IsCode(CardId.PowerPatronShadowMachineZegredo, CardId.MediusThePure)) score += 2000;
                    if (c.IsCode(CardId.DoomZVFiveAmalthe, CardId.DoomZVIISevenElara)) score += 1500;
                    return score + c.Attack;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // ── Hint 500 / 512 / 513: Release / Material selection ──
            if (hint == 500 || hint == 512 || hint == 513)
            {
                var safe = cards.Where(c => c != null && !IsAceCard(c)).OrderBy(c => GetMaterialPriority(c)).ToList();
                if (safe.Count >= min) return safe.Take(max).ToList();
            }

            // Protect our Ace cards from generic selections
            if (cards.Any(c => c != null && c.Controller == 0 && c.Location == CardLocation.MonsterZone && IsAceCard(c)))
            {
                var safeCards = cards.Where(c => c == null || c.Controller != 0 || c.Location != CardLocation.MonsterZone || !IsAceCard(c)).ToList();
                if (safeCards.Count >= min)
                    return safeCards.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }

    [Deck("Expert_2026_Doomz", "2026_Doomz")]
    public class ExpertDoomzExecutor : _2026_DoomzExecutor
    {
        public ExpertDoomzExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
        }
    }
}
