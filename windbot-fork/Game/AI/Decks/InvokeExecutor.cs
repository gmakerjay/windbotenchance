// ============================================================
// CARD AUDIT เนโฌโ€ 2026_Invoke
// ============================================================
// Card Name                      | Type      | OPT? | Effect Summary                      | Condition
// -------------------------------+-----------+------+-------------------------------------+--------------------------------
// Magical Meltdown               | Field Sp  | Yes  | Search Aleister                     | Activate to get Aleister
// Aleister the Invoker           | Monster   | Yes  | Normal Search / GY buff             | Normal Summon helper
// Aleister the Reminiscent       | Monster   | Yes  | Special summon & buff / search      | Spec Summon Spellcaster present
// Aiwass the Spirit of the Law   | Monster   | Yes  | Search Aleister / GY revive         | Search/Fusion Helper
// Virakam the Artificial Spirit  | Monster   | Yes  | Special summon & Set / negate       | Protect Fusions
// Spellbook Magician of Prophecy | Monster   | Yes  | Search Spellbook card               | Secrets/Knowledge search
// Spellbook of Secrets           | Spell     | Yes  | Search Spellbook card               | Secrets/Knowledge search
// Spellbook of Knowledge         | Spell     | Yes  | Send Spellcaster to draw 2          | Draw Engine
// Invocation                     | Spell     | Yes  | Fusion summon / GY recycle          | Main Fusion tool
// Invocation "Sword"             | Spell     | Yes  | Fusion / GY recycle                 | Main Fusion tool
// Spirit Sword Aiwass            | Spell     | Yes  | Spec summon Aiwass / dump           | Turn 1 setup
// Magical Name - Rosa Mundi      | Spell     | Yes  | Spec summon Invoked from Extra      | Quick Spec summon
// ============================================================
// ACE CARDS:
// - Primary:   Invoked Mechaba เนโฌโ€ Spell/Trap/Monster Negation
// - Secondary: Invoked Transcendence Aeon เนโฌโ€ Board removal & attribute modification
// ============================================================

using YGOSharp.OCGWrapper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_Invoke", "2026_Invoke")]
    public class _2026_InvokeExecutor : ModernExecutor
    {
        public class CardId
        {
            public const int InfiniteImpermanence = 10045474;
            public const int InvokedElysium = 11270236;
            public const int GaruraWingsOfResonantLife = 11765832;
            public const int InvokedPurgatrio = 12307878;
            public const int InvokedCaliga = 13529466;
            public const int AshBlossom = 14558127;
            public const int MaxxC = 23434538;
            public const int SpellbookMagicianOfProphecy = 14824019;
            public const int SpellbookOfKnowledge = 23314220;
            public const int GravityController = 23656668;
            public const int CalledByTheGrave = 24224830;
            public const int ForbiddenDroplet = 24299458;
            public const int ArtemisTheMagistusMoonMaiden = 34755994;
            public const int InvokedMagistusOmega = 38423248;
            public const int MagicalMeltdown = 47679935;
            public const int SuperPolymerization = 48130397;
            public const int InvokedRaidjin = 49513164;
            public const int GozenMatch = 53334471;
            public const int MudragonOfTheSwamp = 54757758;
            public const int SecretVillageOfTheSpellcasters = 68462976;
            public const int Terraforming = 73628505;
            public const int GhostBelle = 73642296;
            public const int Invocation = 74063034;
            public const int InvokedMechaba = 75286621;
            public const int InvokedCocytus = 85908279;
            public const int AleisterTheInvoker = 86120751;
            public const int SpellbookOfSecrets = 89739383;
            public const int AleisterTheInvokerOfMadness = 97973962;

            // 2026 Support
            public const int AleisterTheReminiscent = 101305015;
            public const int AiwassTheSpiritOfTheLaw = 101305016;
            public const int VirakamTheArtificialSpirit = 101305017;
            public const int InvokedSorath = 101305030;
            public const int InvokedBabalon = 101305031;
            public const int InvokedOkeanos = 101305032;
            public const int InvokedTranscendenceAeon = 101305033;
            public const int InvocationSword = 101305053;
            public const int SpiritSwordAiwass = 101305054;
            public const int MagicalNameRosaMundi = 101305070;
        }

        private bool _reminiscentUsed = false;
        private bool _aiwassSearchUsed = false;
        private bool _virakamUsed = false;
        private bool _invocationSwordUsed = false;
        private bool _spiritSwordUsed = false;
        private bool _secretsUsed = false;
        private bool _knowledgeUsed = false;

        private static readonly int[] AceCardIds = {
            CardId.InvokedMechaba,
            CardId.InvokedTranscendenceAeon,
            CardId.InvokedElysium,
            CardId.InvokedSorath,
            CardId.InvokedBabalon
        };



        private static readonly int[] AltergeistCardIds = {
            25533642, // Meluseek
            89538537, // Silquitous
            42790071, // Multifaker
            53143898, // Marionetter
            52927340, // Kunquery
            49725936, // Hexstia
            18528996, // Primebanshee
            85289965, // Protocol
            41999284, // Manifestation
            94259633  // Pookuery
        };

        private bool IsAltergeistCard(ClientCard card)
        {
            return card != null && AltergeistCardIds.Contains(card.Id);
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Contains(card.Id);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.IsCode(CardId.AshBlossom, CardId.MaxxC, CardId.GhostBelle))
                return 800;
            if (c.IsCode(CardId.ArtemisTheMagistusMoonMaiden))
                return 100;
            if (c.IsCode(CardId.AleisterTheInvoker))
                return 110;
            if (c.IsCode(CardId.AleisterTheReminiscent))
                return 120;
            return 200;
        }

        private bool EnemyHasActiveProtocol()
        {
            return Enemy.GetSpells().Any(c => c != null
                && c.IsFaceup()
                && !c.IsDisabled()
                && c.IsCode(85289965));
        }

        public _2026_InvokeExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            ResourcePlan.RegisterAceCards(AceCardIds);
            HeuristicGuard.RegisterAceCards(AceCardIds);
            // เนโ€โฌเนโ€โฌ Combo Router: Sequencing เนโ€โฌเนโ€โฌ
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Standard-Setup",
                RequiredCards = new List<int> { CardId.InfiniteImpermanence, CardId.GaruraWingsOfResonantLife },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.InfiniteImpermanence, ActionType = ExecutorType.Activate, Description = "Play CardId.InfiniteImpermanence" },
                    new() { CardId = CardId.GaruraWingsOfResonantLife, ActionType = ExecutorType.Activate, Description = "Extend with CardId.GaruraWingsOfResonantLife" }
                },
                EndBoardScore = 80
            });

            // เนโ€โฌเนโ€โฌ Bait Planner เนโ€โฌเนโ€โฌ
            BaitPlanner.RegisterComboStarters(CardId.InfiniteImpermanence, CardId.InvokedElysium);
            BaitPlanner.RegisterBaitCards(CardId.Terraforming, CardId.InvokedElysium);

            // เนโ€โฌเนโ€โฌ Chain Advisor เนโ€โฌเนโ€โฌ
            ChainAdvisor.RegisterHighValueTargets(CardId.InfiniteImpermanence, CardId.InvokedElysium);

            // #1 Hand traps & Negations (Interactions) - Highest priority
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, () => SmartHandTrapChain() && DefaultMaxxC());
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEmergency);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelle, () => SmartHandTrapChain() && DefaultDontChainMyself());
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, InfiniteImpermanenceEffect);
            AddExecutor(ExecutorType.Activate, CardId.VirakamTheArtificialSpirit, VirakamNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.AleisterTheInvoker, AleisterBuffEffect);

            // #2 Quick Effects of Boss Monsters (High priority, response-ready on opponent turn)
            AddExecutor(ExecutorType.Activate, CardId.InvokedMechaba, MechabaNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.InvokedTranscendenceAeon, TranscendenceAeonEffect);
            AddExecutor(ExecutorType.Activate, CardId.InvokedSorath, SorathEffect);
            AddExecutor(ExecutorType.Activate, CardId.InvokedBabalon, BabalonEffect);

            // #3 Opponent board reactions
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletEffect);
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationEffect);

            // #4 Protection & Field Setup (Meltdown MUST be first!)
            AddExecutor(ExecutorType.Activate, CardId.MagicalMeltdown, MagicalMeltdownEffect);
            AddExecutor(ExecutorType.Activate, CardId.Terraforming, TerraformingEffect);
            AddExecutor(ExecutorType.Activate, CardId.SpiritSwordAiwass, SpiritSwordAiwassEffect);
            AddExecutor(ExecutorType.Activate, CardId.AiwassTheSpiritOfTheLaw, AiwassSearchEffect);

            // #5 Searches & Setup
            AddExecutor(ExecutorType.Activate, CardId.SpellbookOfSecrets, SpellbookOfSecretsEffect);
            AddExecutor(ExecutorType.Summon, CardId.SpellbookMagicianOfProphecy, SpellbookMagicianSummon);
            AddExecutor(ExecutorType.Activate, CardId.SpellbookMagicianOfProphecy, SpellbookMagicianEffect);
            AddExecutor(ExecutorType.Activate, CardId.AleisterTheReminiscent, ReminiscentSummonEffect);
            AddExecutor(ExecutorType.Summon, CardId.AleisterTheInvoker, AleisterSummon);
            AddExecutor(ExecutorType.Summon, CardId.AleisterTheReminiscent, AleisterReminiscentSummon);
            AddExecutor(ExecutorType.Activate, CardId.AleisterTheInvoker, AleisterSearchEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.ArtemisTheMagistusMoonMaiden, ArtemisSummon);
            AddExecutor(ExecutorType.Activate, CardId.SpellbookOfKnowledge, SpellbookOfKnowledgeEffect);

            // #6 Fusion & Summon Spells
            AddExecutor(ExecutorType.Activate, CardId.Invocation, InvocationEffect);
            AddExecutor(ExecutorType.Activate, CardId.InvocationSword, InvocationSwordEffect);
            AddExecutor(ExecutorType.Activate, CardId.MagicalNameRosaMundi, RosaMundiEffect);

            // #7 Extra Deck & Boss Plays
            AddExecutor(ExecutorType.SpSummon, CardId.GravityController, GravityControllerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AleisterTheInvokerOfMadness, InvokerOfMadnessSummon);
            AddExecutor(ExecutorType.Activate, CardId.InvokedOkeanos, OkeanosEffect);

            // #8 Fusion Extra Deck summons (board-state-aware placement)
            AddExecutor(ExecutorType.SpSummon, CardId.InvokedMechaba, InvokedSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.InvokedTranscendenceAeon, InvokedSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.InvokedSorath, InvokedSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.InvokedBabalon, InvokedSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.InvokedOkeanos, InvokedSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.InvokedPurgatrio, InvokedSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.InvokedRaidjin, InvokedSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.InvokedCaliga, InvokedSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.InvokedElysium, InvokedSummonCondition);

            // #9 Sets & repos
            AddExecutor(ExecutorType.SpellSet, CardId.GozenMatch, SetTrapCondition);
            AddExecutor(ExecutorType.SpellSet, SetImpermanenceCondition);
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // Invoke/Dogmatika control เนโฌโ€ prefer going first to set up Mechaba + backrow
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _reminiscentUsed = false;
            _aiwassSearchUsed = false;
            _virakamUsed = false;
            _invocationSwordUsed = false;
            _spiritSwordUsed = false;
            _secretsUsed = false;
            _knowledgeUsed = false;

            // เนโ€โฌเนโ€โฌ Going-Second BreakBoard: prioritize disruption over combo เนโ€โฌเนโ€โฌ
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
            }
        }



        private bool SetTrapCondition() => Util.IsTurn1OrMain2();

        private bool SetImpermanenceCondition()
        {
            return Card.Id == CardId.InfiniteImpermanence && Bot.GetMonsterCount() == 0;
        }

        private bool IsSafeToAttack(ClientCard attacker)
        {
            // Okeanos can enable direct attacks
            bool canDirect = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.InvokedOkeanos));
            if (canDirect) return true;

            foreach (ClientCard enemy in Enemy.MonsterZone)
            {
                if (enemy == null || !enemy.IsFaceup()) continue;
                if (enemy.IsAttack() && enemy.Attack >= attacker.Attack) return false;
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
            foreach (ClientCard monster in Bot.GetMonsters())
            {
                if (monster == null) continue;
                bool enemyEmpty = Enemy.GetMonsterCount() == 0;
                if (monster.IsAttack())
                {
                    if (!enemyEmpty && !IsSafeToAttack(monster) && IsSafeToDefend(monster))
                        return true;
                }
                else
                {
                    if (enemyEmpty || IsSafeToAttack(monster))
                        return true;
                }
            }
            return false;
        }

        // FieldGuard inherited: IsSpecialSummonBlocked, CanDealLethal, CanOTK,
        // ShouldSkipCombo, NeedsBoardPresence, IsInGrindGame, EnemyHasKnownNegate เนยโ€ inherited

        protected override bool IsBoardStrongEnough()
        {
            if (Bot.HasInMonstersZone(CardId.InvokedMechaba)) return true;
            if (Bot.HasInMonstersZone(CardId.InvokedTranscendenceAeon)) return true;
            if (Bot.HasInMonstersZone(CardId.InvokedOkeanos) && Bot.GetMonsterCount() >= 2) return true;
            if (Bot.HasInMonstersZone(CardId.InvokedBabalon) || Bot.HasInMonstersZone(CardId.InvokedSorath)) return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (IsBoardStrongEnough())
                return base.ShouldStopExtending();
            return false;
        }

        private bool OpponentHasThreateningMonster()
        {
            return Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && !c.IsDisabled() &&
                (c.IsFloodgate() || c.Attack >= 2500 || c.HasType(CardType.Link) || c.HasType(CardType.Synchro)));
        }

        private enum FusionType
        {
            Mechaba,       // Negation เนโฌโ€ default control
            Purgatrio,     // OTK เนโฌโ€ multi-attack
            Raidjin,       // Flip face-down เนโฌโ€ big monster removal
            Caliga,        // Lock เนโฌโ€ limit opponent to 1 monster effect
            Okeanos,       // Direct attacks เนโฌโ€ finish game
            Elysium,       // Board wipe เนโฌโ€ reset opponent field
            Sorath,        // Revive เนโฌโ€ grind/extend
            Babalon,       // Set Virakam/RosaMundi เนโฌโ€ setup
            Transcendence  // Altergeist counter / attribute change
        }

        private FusionType GetPreferredFusionTarget()
        {
            // OTK priority: if we can attack for game
            if (Duel.MainPhase != null && Duel.MainPhase.CanBattlePhase)
            {
                int totalAtk = Bot.MonsterZone.Where(c => c != null && c.IsFaceup() && c.IsAttack() && !c.Attacked)
                    .Sum(c => c.Attack);
                
                // Purgatrio gets 300 ATK per opponent monster
                int enemyCount = Enemy.GetMonsterCount();
                int purgatrioAtk = 2300 + (enemyCount * 300);
                
                if (totalAtk + purgatrioAtk >= Enemy.LifePoints && enemyCount >= 1)
                    return FusionType.Purgatrio;
            }

            // Okeanos: if we can enable direct attacks for game
            if (Enemy.GetMonsterCount() >= 2)
            {
                int totalAtk = Bot.MonsterZone.Where(c => c != null && c.IsFaceup() && c.IsAttack() && !c.Attacked)
                    .Sum(c => c.Attack);
                if (totalAtk >= Enemy.LifePoints) return FusionType.Okeanos;
            }

            // Transendence: if opponent has Altergeist or backrow-heavy
            if (Enemy.GetSpells().Count(c => c != null && c.IsFaceup()) >= 2)
                return FusionType.Transcendence;

            // Elysium: if opponent has many monsters
            if (Enemy.GetMonsterCount() >= 3)
                return FusionType.Elysium;

            // Caliga: if opponent has many monster effects and we have advantage
            if (Enemy.GetMonsterCount() >= 2 && Bot.GetHandCount() >= 3)
                return FusionType.Caliga;

            // Raidjin: if opponent has a big threatening monster
            if (OpponentHasThreateningMonster())
                return FusionType.Raidjin;

            // Sorath: if we're in grind and need extension
            if (IsInGrindGame() && Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.Level <= 6 && c.IsCanRevive()))
                return FusionType.Sorath;

            // Babalon: if we need setup (search Virakam/RosaMundi)
            if (Bot.GetMonsterCount() <= 1 && !IsBoardStrongEnough())
                return FusionType.Babalon;

            // Default: Mechaba for negate
            return FusionType.Mechaba;
        }

        private int GetFusionCardId(FusionType type)
        {
            switch (type)
            {
                case FusionType.Mechaba: return CardId.InvokedMechaba;
                case FusionType.Purgatrio: return CardId.InvokedPurgatrio;
                case FusionType.Raidjin: return CardId.InvokedRaidjin;
                case FusionType.Caliga: return CardId.InvokedCaliga;
                case FusionType.Okeanos: return CardId.InvokedOkeanos;
                case FusionType.Elysium: return CardId.InvokedElysium;
                case FusionType.Sorath: return CardId.InvokedSorath;
                case FusionType.Babalon: return CardId.InvokedBabalon;
                case FusionType.Transcendence: return CardId.InvokedTranscendenceAeon;
                default: return CardId.InvokedMechaba;
            }
        }

        // Returns a priority list with the preferred fusion first, then reasonable fallbacks
        private int[] GetFusionPriorityList(FusionType preferred)
        {
            // Build base fallback list
            int[] allFusions = {
                CardId.InvokedMechaba,
                CardId.InvokedTranscendenceAeon,
                CardId.InvokedPurgatrio,
                CardId.InvokedSorath,
                CardId.InvokedBabalon,
                CardId.InvokedRaidjin,
                CardId.InvokedCaliga,
                CardId.InvokedOkeanos,
                CardId.InvokedElysium
            };

            // Put preferred first, then all others as fallbacks
            var list = new List<int>(allFusions.Length);
            list.Add(GetFusionCardId(preferred));
            foreach (int id in allFusions)
            {
                if (id != GetFusionCardId(preferred))
                    list.Add(id);
            }
            return list.ToArray();
        }

        private bool AshBlossomEmergency()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Bot.GetMonsterCount() == 0 && Bot.GetHandCount() <= 2 && Duel.Player == 0)
                return true;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private ClientCard GetPreemptiveImpermTarget()
        {
            int[] threatIds = {
                21522601, // Witchcrafter Madame Verre
                84523092, // Witchcrafter Haine
                1561110,  // ABC-Dragon Buster
                4280258,  // Apollousa, Bow of the Goddess
                10443957, // Cyber Dragon Infinity
                84815190, // Baronne de Fleur
                1508649   // Altergeist Hexstia
            };

            return Enemy.MonsterZone.GetMonsters().FirstOrDefault(c => 
                c != null && c.IsFaceup() && !c.IsDisabled() && 
                threatIds.Contains(c.Id) && 
                !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget());
        }

        private bool InfiniteImpermanenceEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;

            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                var target = GetPreemptiveImpermTarget();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }

            return DefaultInfiniteImpermanence();
        }

        private bool VirakamNegateEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // spec summon from hand
                bool hasAleister = Bot.MonsterZone.Any(c => c != null && c.IsCode(CardId.AleisterTheInvoker, CardId.AleisterTheReminiscent)) ||
                                   Bot.Graveyard.Any(c2 => c2 != null && c2.IsCode(CardId.AleisterTheInvoker, CardId.AleisterTheReminiscent));
                return hasAleister && !_virakamUsed;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (LastChainCard == null || LastChainCard.Controller == 0) return false;
                // If opponent has active Protocol, don't try to negate Altergeist cards activated on their field
                if (EnemyHasActiveProtocol() && IsAltergeistCard(LastChainCard) && (LastChainCard.Location == CardLocation.MonsterZone || LastChainCard.Location == CardLocation.SpellZone))
                {
                    return false;
                }

                // Resource management: don't waste Virakam negate on trivial effects
                // Virakam is a one-time negate เนโฌโ€ save for game-changing effects
                bool isTrivial = false;

                // Don't negate normal spell/trap activations like Pot of Greed equivalents
                if (LastChainCard.HasType(CardType.Normal) && !LastChainCard.HasType(CardType.Monster))
                {
                    if (!Util.IsChainTarget(Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && IsAceCard(c))))
                        isTrivial = true;
                }

                // Save Virakam if we have Mechaba available (Mechaba is better)
                if (Bot.HasInMonstersZone(CardId.InvokedMechaba) && !isTrivial)
                {
                    // Let Mechaba handle it
                    if (!LastChainCard.HasType(CardType.Monster) || LastChainCard.Attack < 3000)
                        isTrivial = true;
                }

                if (isTrivial) return false;

                return true;
            }
            return true;
        }

        private bool AleisterBuffEffect()
        {
            return Card.Location == CardLocation.Hand && 
                   Duel.Phase > DuelPhase.Main1 && Duel.Phase < DuelPhase.Main2 &&
                   Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsExtraCard());
        }

        private bool MechabaNegateEffect()
        {
            if (LastChainCard == null || LastChainCard.Controller == 0) return false;

            // If opponent has active Protocol, don't try to negate Altergeist cards activated on their field
            if (EnemyHasActiveProtocol() && IsAltergeistCard(LastChainCard) && (LastChainCard.Location == CardLocation.MonsterZone || LastChainCard.Location == CardLocation.SpellZone))
            {
                return false;
            }

            // Resource management: don't waste Mechaba on trivial effects
            // Save for: monster effects (most dangerous), search spells, field spells
            bool isTrivial = false;

            // Opponent normal summon effect (like Aleister's search) เนโฌโ€ save negate
            if (LastChainCard.HasType(CardType.Monster))
            {
                // Always negate monster effects except low-impact normal summons
                if (LastChainCard.Attack <= 1500 && !OpponentHasThreateningMonster())
                {
                    // Check if opponent is just setting up เนโฌโ€ let them, save negate
                    if (Duel.Turn <= 2 && Bot.GetHandCount() >= 3) isTrivial = true;
                }
            }
            else if (LastChainCard.HasType(CardType.Spell))
            {
                // Negate key spells but save for monster negations
                if (LastChainCard.HasType(CardType.QuickPlay) || LastChainCard.HasType(CardType.Normal))
                {
                    // Only negate if it's a removal/burn spell targeting our cards
                    if (!Util.IsChainTarget(Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup())))
                    {
                        // If it's a generic search/utility spell and we have board advantage, save resource
                        if (IsBoardStrongEnough() && Bot.GetHandCount() >= 2)
                            isTrivial = true;
                    }
                }
            }
            else if (LastChainCard.HasType(CardType.Trap))
            {
                // Always negate traps (they're harder to play around)
                // But save if we're low on resources
                if (IsInGrindGame() && !LastChainCard.HasType(CardType.Counter)) isTrivial = true;
            }

            if (isTrivial) return false;

            return true;
        }

        // --- #2 OPPONENT REACTIONS ---
        private bool ForbiddenDropletEffect()
        {
            var target = Util.GetProblematicEnemyCard();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool SuperPolymerizationEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Enemy.GetMonsterCount() >= 1;
        }

        // --- #3 SEARCHES & SETUP ---
        private bool MagicalMeltdownEffect()
        {
            if (ShouldSkipCombo()) return false;
            return !Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.MagicalMeltdown)) &&
                   !Bot.SpellZone.Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.MagicalMeltdown));
        }

        private bool TerraformingEffect()
        {
            if (ShouldSkipCombo()) return false;
            return !Bot.HasInHand(CardId.MagicalMeltdown) && 
                   !Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.MagicalMeltdown)) &&
                   !Bot.SpellZone.Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.MagicalMeltdown));
        }

        private bool SpellbookOfSecretsEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_secretsUsed) return false;
            
            if (!Bot.HasInHand(CardId.SpellbookOfKnowledge))
            {
                AI.SelectCard(CardId.SpellbookOfKnowledge);
            }
            else
            {
                AI.SelectCard(CardId.SpellbookMagicianOfProphecy);
            }
            _secretsUsed = true;
            return true;
        }

        private bool SpellbookMagicianSummon()
        {
            if (ShouldSkipCombo()) return false;

            // Don't summon if we already have Aleister in hand (Aleister is the priority normal summon)
            if (Bot.HasInHand(CardId.AleisterTheInvoker)) return false;

            // Summon Spellbook Magician if we can use Knowledge to draw 2
            // Need: Spellbook Magician on field + Knowledge in hand
            bool hasKnowledge = Bot.HasInHand(CardId.SpellbookOfKnowledge);
            bool hasSecrets = Bot.HasInHand(CardId.SpellbookOfSecrets) || 
                              Bot.SpellZone.Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.SpellbookOfSecrets));

            // Only summon if we have Knowledge ready or Secrets to search Knowledge
            if (!hasKnowledge && !hasSecrets && GetRemainingCount(CardId.SpellbookOfSecrets) == 0) return false;

            return true;
        }

        private bool AiwassSearchEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_aiwassSearchUsed) return false;
            
            if (Card.Location == CardLocation.Hand)
            {
                bool hasAleister = Bot.HasInHand(CardId.AleisterTheInvoker) || Bot.HasInHand(CardId.AleisterTheReminiscent);
                bool hasSearch = Bot.HasInHand(CardId.MagicalMeltdown) || Bot.HasInHand(CardId.Terraforming) || Bot.HasInHand(CardId.SpiritSwordAiwass);
                if (hasAleister || hasSearch) return false;
                
                AI.SelectCard(CardId.AleisterTheInvoker, CardId.AleisterTheReminiscent);
                _aiwassSearchUsed = true;
                return true;
            }
            
            if (Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(CardId.AleisterTheInvoker, CardId.AleisterTheReminiscent);
                _aiwassSearchUsed = true;
                return true;
            }

            if (Card.Location == CardLocation.Grave)
            {
                // Aiwass GY effect: banish to search Aleister
                // Only activate if we don't already have Aleister available
                bool hasAleister = Bot.HasInHand(CardId.AleisterTheInvoker) || Bot.HasInHand(CardId.AleisterTheReminiscent) ||
                                   Bot.MonsterZone.Any(c => c != null && c.IsCode(CardId.AleisterTheInvoker, CardId.AleisterTheReminiscent)) ||
                                   Bot.Graveyard.Any(c => c != null && c.IsCode(CardId.AleisterTheInvoker, CardId.AleisterTheReminiscent));
                if (hasAleister) return false;

                // Check if there's an Aleister in deck to search
                bool hasTargetInDeck = GetRemainingCount(CardId.AleisterTheInvoker) > 0 || GetRemainingCount(CardId.AleisterTheReminiscent) > 0;
                return hasTargetInDeck;
            }
            return false;
        }

        private bool AleisterReminiscentSummon()
        {
            if (ShouldSkipCombo()) return false;
            return !Bot.HasInHand(CardId.AleisterTheInvoker);
        }

        private bool ReminiscentSummonEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_reminiscentUsed) return false;
            
            if (Card.Location == CardLocation.Hand)
            {
                var target = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && (c.HasRace(CardRace.SpellCaster) || c.HasType(CardType.Fusion)));
                if (target != null)
                {
                    AI.SelectCard(target);
                    _reminiscentUsed = true;
                    return true;
                }
            }
            return true;
        }

        private bool AleisterSummon()
        {
            if (ShouldSkipCombo()) return false;
            return true;
        }

        private bool AleisterSearchEffect()
        {
            if (ShouldSkipCombo()) return false;
            return true;
        }

        private bool SpiritSwordAiwassEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_spiritSwordUsed) return false;
            
            bool hasAleister = Bot.HasInHand(CardId.AleisterTheInvoker) || 
                               Bot.HasInHand(CardId.AleisterTheReminiscent) ||
                               Bot.MonsterZone.Any(c => c != null && c.IsCode(CardId.AleisterTheInvoker, CardId.AleisterTheReminiscent)) ||
                               Bot.Graveyard.Any(c => c != null && c.IsCode(CardId.AleisterTheInvoker, CardId.AleisterTheReminiscent));
                               
            bool hasMeltdown = Bot.HasInHand(CardId.MagicalMeltdown) || 
                               Bot.HasInHand(CardId.Terraforming) ||
                               Bot.SpellZone.Any(c => c != null && c.IsCode(CardId.MagicalMeltdown));

            if (!hasAleister && !hasMeltdown)
            {
                AI.SelectOption(0); // Summon Aiwass to search
            }
            else
            {
                AI.SelectOption(1); // Send Aleister to GY
            }
            _spiritSwordUsed = true;
            return true;
        }

        private bool SpellbookOfKnowledgeEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_knowledgeUsed) return false;

            // Don't use Knowledge if we still need Aleister on field for fusion
            // Check ALL fusion methods including RosaMundi and SuperPoly
            bool needAleisterForFusion = !Bot.HasInHand(CardId.Invocation) && 
                                         !Bot.HasInHand(CardId.InvocationSword) &&
                                         !Bot.HasInHand(CardId.MagicalNameRosaMundi) &&
                                         !Bot.HasInHand(CardId.SuperPolymerization) &&
                                         !Bot.SpellZone.Any(c => c != null && c.IsFaceup() && 
                                            (c.IsCode(CardId.Invocation) || c.IsCode(CardId.InvocationSword)));
            bool hasAleisterOnField = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && 
                (c.IsCode(CardId.AleisterTheInvoker) || c.IsCode(CardId.AleisterTheReminiscent) || c.IsCode(CardId.ArtemisTheMagistusMoonMaiden)));

            // Prefer to send Spellbook Magician or Artemis over Aleister
            var target = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.SpellbookMagicianOfProphecy)) ??
                         Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.ArtemisTheMagistusMoonMaiden));

            // Only use Aleister for Knowledge if we have another way to fusion
            if (target == null && needAleisterForFusion && hasAleisterOnField)
                return false; // Save Aleister for fusion

            if (target == null)
            {
                target = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.AleisterTheInvoker)) ??
                         Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsCode(CardId.AleisterTheReminiscent) && c.HasRace(CardRace.SpellCaster) && c.HasRace(CardRace.Zombie) == false);
            }
                         
            if (target != null)
            {
                AI.SelectCard(target);
                _knowledgeUsed = true;
                return true;
            }
            return false;
        }

        // --- #4 FUSION SPELLS ---
        private bool InvocationEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.Hand)
            {
                // Board-state-aware fusion target selection with fallback priority list
                FusionType preferred = GetPreferredFusionTarget();
                int[] fusionPriority = GetFusionPriorityList(preferred);
                if (fusionPriority.Length == 1)
                    AI.SelectCard(fusionPriority[0]);
                else
                    AI.SelectCard(fusionPriority);
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // GY effect: only activate if we have materials and need the fusion
                bool hasFieldMaterials = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && !IsAceCard(c)) ||
                                        Enemy.MonsterZone.Any(c => c != null && c.IsFaceup());
                if (!hasFieldMaterials) return false;

                if (ShouldSkipCombo()) return false;

                FusionType preferred = GetPreferredFusionTarget();
                AI.SelectCard(GetFusionPriorityList(preferred));
                return true;
            }
            return true;
        }

        private bool InvocationSwordEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.Hand)
            {
                // Board-state-aware fusion target selection with fallback priority list
                FusionType preferred = GetPreferredFusionTarget();
                AI.SelectCard(GetFusionPriorityList(preferred));
                return true;
            }
            if (!_invocationSwordUsed)
            {
                AI.SelectCard(CardId.AleisterTheInvoker, CardId.Invocation, CardId.AleisterTheReminiscent);
                _invocationSwordUsed = true;
                return true;
            }
            return false;
        }

        private bool RosaMundiEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;

            // Board-state-aware fusion target selection with fallback priority list
            FusionType preferred = GetPreferredFusionTarget();
            AI.SelectCard(GetFusionPriorityList(preferred));
            return true;
        }

        // --- #5 EXTRA DECK ---
        private bool ArtemisSummon()
        {
            if (ShouldSkipCombo()) return false;
            return Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.AleisterTheInvoker, CardId.AleisterTheReminiscent));
        }

        private bool GravityControllerSummon()
        {
            if (ShouldSkipCombo()) return false;
            return Bot.MonsterZone.Any(c => c != null && c.Sequence >= 5 && c.IsMonster());
        }

        private bool InvokerOfMadnessSummon()
        {
            if (ShouldSkipCombo()) return false;

            // Invoker of Madness locks you into Fusion Summons from Extra Deck for the rest of the turn
            // Only summon if we have Invocation ready or can still fusion afterwards
            bool hasInvocation = Bot.HasInHand(CardId.Invocation) || Bot.HasInHand(CardId.InvocationSword) ||
                                 Bot.HasInHand(CardId.SuperPolymerization) || Bot.HasInHand(CardId.MagicalNameRosaMundi) ||
                                 Bot.SpellZone.Any(c => c != null && c.IsFaceup() && 
                                    (c.IsCode(CardId.Invocation) || c.IsCode(CardId.InvocationSword)));
            bool hasTargetInDeck = GetRemainingCount(CardId.Invocation) > 0 || GetRemainingCount(CardId.InvocationSword) > 0;

            if (!hasInvocation && !hasTargetInDeck && !Bot.HasInHand(CardId.MagicalNameRosaMundi))
                return false; // Would lock us out with no fusion summon possible

            int spellcasters = Bot.MonsterZone.Count(c => c != null && c.IsFaceup());
            return spellcasters >= 2;
        }

        private bool TranscendenceAeonEffect()
        {
            // Only activate if opponent has meaningful targets
            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup())) return true;
            if (Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && 
                (c.IsCode(85289965) || c.HasType(CardType.Continuous) || c.HasType(CardType.Field))))
                return true;
            return false;
        }

        private bool SorathEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && c.Level <= 6 && c.IsCanRevive()) ??
                             Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && c.Level <= 6 && c.IsCanRevive());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
                // Only activate without target if we need board presence
                if (NeedsBoardPresence()) return true;
                return false;
            }
            return true;
        }

        private bool BabalonEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Only activate if we need to search Virakam or RosaMundi
                bool needVirakam = !Bot.HasInHand(CardId.VirakamTheArtificialSpirit) && 
                    !Bot.MonsterZone.Any(c => c != null && c.IsCode(CardId.VirakamTheArtificialSpirit));
                bool needRosaMundi = !Bot.HasInHand(CardId.MagicalNameRosaMundi) && 
                    !Bot.SpellZone.Any(c => c != null && c.IsCode(CardId.MagicalNameRosaMundi));

                if (needVirakam || needRosaMundi)
                {
                    AI.SelectCard(CardId.VirakamTheArtificialSpirit, CardId.MagicalNameRosaMundi);
                    return true;
                }
                return false;
            }
            return true;
        }

        private bool OkeanosEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                var target = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.IsExtraCard());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
                return false;
            }
            return true;
        }

        // --- #7 CARD SELECTION HELPER ---
        private IList<ClientCard> SelectPreferred(
            IList<ClientCard> cards, int min, int max, params int[] preferredIds)
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

        // --- #6 SELECT CARD STRATEGIES & HINTS ---
        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // 1. Link Material Selection เนโฌโ€ Protect Ace Cards (hint 533)
            if (hint == 533)
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (IsAceCard(c)) return 100;
                    if (c.IsCode(CardId.ArtemisTheMagistusMoonMaiden)) return 1;
                    if (c.IsCode(CardId.AleisterTheInvoker)) return 2;
                    if (c.IsCode(CardId.AleisterTheReminiscent)) return 3;
                    return 10;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // 2. Fusion Material Selection เนโฌโ€ Protect Ace Cards (hint 511)
            if (hint == 511)
            {
                var sorted = cards.OrderBy(GetFusionMaterialScore).ToList();
                return sorted.Take(max).ToList();
            }

            // 3. Special Summon / Revival selection (hint 509)
            if (hint == 509)
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    int controllerScore = (c.Controller == 0) ? 0 : 1000;
                    int cardScore = 100;
                    if (c.IsCode(CardId.InvokedMechaba)) cardScore = 1;
                    else if (c.IsCode(CardId.InvokedTranscendenceAeon)) cardScore = 2;
                    else if (c.IsCode(CardId.InvokedBabalon)) cardScore = 3;
                    else if (c.IsCode(CardId.InvokedSorath)) cardScore = 4;
                    else if (c.IsCode(CardId.InvokedOkeanos)) cardScore = 5;
                    else if (c.IsCode(CardId.VirakamTheArtificialSpirit)) cardScore = 6;
                    else if (c.IsCode(CardId.AleisterTheReminiscent)) cardScore = 7;
                    else if (c.IsCode(CardId.AleisterTheInvoker)) cardScore = 8;
                    else if (c.IsCode(CardId.SpellbookMagicianOfProphecy)) cardScore = 9;
                    else if (c.IsCode(CardId.ArtemisTheMagistusMoonMaiden)) cardScore = 10;
                    return controllerScore + cardScore;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // 4. Destruction target selection (hint 502)
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
                            if (c.IsCode(49725936)) return score + 4500; // Hexstia
                            if (c.IsCode(89538537)) return score + 4000; // Silquitous
                            if (c.IsCode(42790071)) return score + 3500; // Multifaker
                        }
                        return score + c.Attack;
                    }
                    else if (c.IsSpell() || c.IsTrap())
                    {
                        if (c.IsFaceup())
                        {
                            if (c.IsCode(85289965)) return score + 4800; // Altergeist Protocol
                            if (c.IsCode(41999284)) return score + 3000; // Manifestation
                            if (c.HasType(CardType.Continuous) || c.HasType(CardType.Field)) return score + 2000;
                            return score + 500;
                        }
                        return score + 100;
                    }
                    return score;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            if (Card == null) return base.OnSelectCard(cards, min, max, hint, cancelable);

            // Card-specific checks
            if (Card.Id == CardId.VirakamTheArtificialSpirit)
            {
                bool hasInvocation = Bot.HasInHand(CardId.Invocation) || Bot.HasInHand(CardId.InvocationSword) ||
                                     Bot.SpellZone.Any(c => c != null && c.IsCode(CardId.Invocation, CardId.InvocationSword));
                if (!hasInvocation)
                {
                    return SelectPreferred(cards, min, max, CardId.Invocation, CardId.InvocationSword, CardId.MagicalMeltdown, CardId.SpiritSwordAiwass);
                }
                return SelectPreferred(cards, min, max, CardId.MagicalNameRosaMundi, CardId.SpiritSwordAiwass, CardId.MagicalMeltdown, CardId.Invocation, CardId.InvocationSword);
            }

            if (Card.Id == CardId.AleisterTheReminiscent)
            {
                if (cards.Any(c => c != null && c.Location == CardLocation.Extra))
                {
                    return SelectPreferred(cards, min, max,
                        CardId.InvokedMechaba,
                        CardId.InvokedTranscendenceAeon,
                        CardId.InvokedBabalon,
                        CardId.InvokedSorath,
                        CardId.InvokedOkeanos,
                        CardId.InvokedPurgatrio,
                        CardId.InvokedRaidjin,
                        CardId.InvokedCaliga,
                        CardId.InvokedElysium
                    );
                }
                if (cards.Any(c => c != null && c.Location == CardLocation.Deck))
                {
                    bool hasInvocation = Bot.HasInHand(CardId.Invocation) || Bot.HasInHand(CardId.InvocationSword);
                    if (!hasInvocation)
                    {
                        return SelectPreferred(cards, min, max, CardId.Invocation, CardId.InvocationSword, CardId.MagicalMeltdown, CardId.SpiritSwordAiwass);
                    }
                    return SelectPreferred(cards, min, max, CardId.MagicalNameRosaMundi, CardId.SpiritSwordAiwass, CardId.MagicalMeltdown, CardId.Invocation, CardId.InvocationSword);
                }
                var ourSpellcasters = cards.Where(c => c != null && c.Controller == 0 && c.IsFaceup() && (c.HasType(CardType.Fusion) || c.HasRace(CardRace.SpellCaster))).ToList();
                if (ourSpellcasters.Count >= min) return ourSpellcasters.Take(max).ToList();
            }

            if (Card.Id == CardId.AiwassTheSpiritOfTheLaw)
            {
                if (cards.Any(c => c != null && c.Location == CardLocation.Deck))
                {
                    return SelectPreferred(cards, min, max, CardId.AleisterTheInvoker, CardId.AleisterTheReminiscent);
                }
                if (cards.Any(c => c != null && c.Location == CardLocation.Extra))
                {
                    return SelectPreferred(cards, min, max,
                        CardId.InvokedMechaba,
                        CardId.InvokedTranscendenceAeon,
                        CardId.InvokedSorath,
                        CardId.InvokedBabalon,
                        CardId.InvokedOkeanos,
                        CardId.InvokedPurgatrio,
                        CardId.InvokedRaidjin,
                        CardId.InvokedCaliga,
                        CardId.InvokedElysium
                    );
                }
            }

            if (Card.Id == CardId.InvokedSorath)
            {
                return cards.OrderBy(c => {
                    if (c.Controller == 0)
                    {
                        if (c.Id == CardId.AleisterTheInvoker) return 1;
                        if (c.Id == CardId.AleisterTheReminiscent) return 2;
                        if (c.Id == CardId.SpellbookMagicianOfProphecy) return 3;
                        return 4;
                    }
                    return 5;
                }).Take(max).ToList();
            }

            if (Card.Id == CardId.InvokedBabalon)
            {
                if (cards.Any(c => c != null && c.Location == CardLocation.Deck))
                {
                    return SelectPreferred(cards, min, max, CardId.VirakamTheArtificialSpirit, CardId.MagicalNameRosaMundi);
                }
                if (cards.Any(c => c != null && c.Location == CardLocation.Grave))
                {
                    var oppGrave = cards.Where(c => c != null && c.Controller == 1 && c.Location == CardLocation.Grave).ToList();
                    var preferredOpp = oppGrave.OrderBy(c => {
                        if (c.IsCode(42790071)) return 1; // Altergeist Multifaker
                        if (c.IsCode(25533642)) return 2; // Altergeist Meluseek
                        if (c.IsCode(89538537)) return 3; // Altergeist Silquitous
                        if (c.IsCode(53143898)) return 4; // Altergeist Marionetter
                        return 5;
                    }).ToList();
                    if (preferredOpp.Count >= min) return preferredOpp.Take(max).ToList();
                    return cards.OrderBy(c => c.Controller == 0 ? 1 : 2).Take(max).ToList();
                }
                if (cards.Any(c => c != null && c.Location == CardLocation.MonsterZone && c.Controller == 0))
                {
                    var ourFusions = cards.Where(c => c != null && c.Controller == 0 && c.HasType(CardType.Fusion)).OrderByDescending(c => c.Attack).ToList();
                    if (ourFusions.Count >= min) return ourFusions.Take(max).ToList();
                }
            }

            if (Card.Id == CardId.InvokedOkeanos)
            {
                var ourFusions = cards.Where(c => c != null && c.Controller == 0 && c.HasType(CardType.Fusion)).OrderByDescending(c => c.Attack).ToList();
                if (ourFusions.Count >= min) return ourFusions.Take(max).ToList();
            }

            if (Card.Id == CardId.InvokedTranscendenceAeon)
            {
                if (cards.Any(c => c != null && c.Location == CardLocation.Extra))
                {
                    return SelectPreferred(cards, min, max, 49725936, 18528996);
                }
                var oppCards = cards.Where(c => c != null && c.Controller == 1).OrderBy(c => {
                    if (c.IsCode(85289965)) return 1;
                    if (c.IsCode(53936268)) return 2;
                    if (c.Location == CardLocation.MonsterZone && c.IsFaceup()) return 3;
                    if (c.Location == CardLocation.SpellZone && c.IsFaceup()) return 4;
                    return 5;
                }).ToList();
                if (oppCards.Count >= min) return oppCards.Take(max).ToList();
            }

            if (Card.Id == CardId.Invocation)
            {
                if (cards.Any(c => c != null && c.Location == CardLocation.Removed))
                {
                    return SelectPreferred(cards, min, max, CardId.AleisterTheInvoker, CardId.AleisterTheReminiscent);
                }
            }

            if (Card.Id == CardId.InvocationSword)
            {
                return SelectPreferred(cards, min, max, CardId.AleisterTheInvoker, CardId.Invocation, CardId.AleisterTheReminiscent);
            }

            if (Card.Id == CardId.MagicalNameRosaMundi)
            {
                if (cards.Any(c => c != null && c.Location == CardLocation.Extra))
                {
                    return SelectPreferred(cards, min, max, CardId.InvokedMechaba, CardId.InvokedTranscendenceAeon, CardId.InvokedSorath, CardId.InvokedBabalon);
                }
                if (cards.Any(c => c != null && c.Location == CardLocation.Removed))
                {
                    return SelectPreferred(cards, min, max, CardId.InvokedMechaba, CardId.InvokedTranscendenceAeon, CardId.InvokedSorath, CardId.InvokedBabalon);
                }
                return SelectPreferred(cards, min, max, CardId.AleisterTheInvoker, CardId.AleisterTheReminiscent, CardId.InvokedOkeanos, CardId.InvokedCaliga);
            }

            if (Card.Id == CardId.InvokedMechaba)
            {
                return cards.OrderBy(c => {
                    if (c.Id == CardId.AiwassTheSpiritOfTheLaw) return 1;
                    if (c.Id == CardId.InvocationSword) return 2;
                    if (c.Id == CardId.MagicalNameRosaMundi) return 3;
                    if (c.Id == CardId.AleisterTheReminiscent) return 4;
                    if (c.Id == CardId.AleisterTheInvoker) return 100;
                    if (c.Id == CardId.Invocation) return 101;
                    return 10;
                }).Take(max).ToList();
            }

            if (Card.Id == CardId.SpellbookOfKnowledge)
            {
                var targets = cards.OrderBy(c => {
                    if (c.Id == CardId.SpellbookMagicianOfProphecy) return 1;
                    if (c.Id == CardId.ArtemisTheMagistusMoonMaiden) return 2;
                    if (c.Id == CardId.AleisterTheInvoker) return 3;
                    if (c.Id == CardId.AleisterTheReminiscent) return 4;
                    return 5;
                }).ToList();
                return targets.Take(max).ToList();
            }

            if (Card.Id == CardId.ForbiddenDroplet)
            {
                var discards = cards.OrderBy(c => {
                    if (c.IsSpell() && !c.IsCode(CardId.MagicalMeltdown, CardId.Invocation)) return 1;
                    if (c.IsMonster() && c.Id == CardId.SpellbookMagicianOfProphecy) return 2;
                    return 3;
                }).ToList();
                return discards.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        private int GetFusionMaterialScore(ClientCard c)
        {
            if (c == null) return 0;
            
            // Protect our own Ace cards on field/GY
            if (c.Controller == 0 && IsAceCard(c))
            {
                return 10000;
            }
            
            // Opponent's cards in GY (best target to banish/disrupt)
            if (c.Controller == 1 && c.Location == CardLocation.Grave)
            {
                if (c.IsCode(42790071)) return 10; // Multifaker
                if (c.IsCode(25533642)) return 11; // Meluseek
                if (c.IsCode(89538537)) return 12; // Silquitous
                if (c.IsCode(53143898)) return 13; // Marionetter
                if (c.IsCode(49725936)) return 14; // Hexstia
                return 20;
            }
            
            // Opponent's cards on field
            if (c.Controller == 1 && c.Location == CardLocation.MonsterZone)
            {
                return 30;
            }
            
            // Our own cards in GY
            if (c.Controller == 0 && c.Location == CardLocation.Grave)
            {
                if (c.IsCode(CardId.AleisterTheInvoker, CardId.AleisterTheReminiscent)) return 40;
                return 50;
            }
            
            // Our own cards in hand/field (preserve if possible)
            if (c.Controller == 0)
            {
                if (c.Location == CardLocation.Hand)
                {
                    if (c.IsCode(CardId.AshBlossom, CardId.GhostBelle, CardId.MaxxC, CardId.InfiniteImpermanence)) return 80;
                    return 70;
                }
                if (c.Location == CardLocation.MonsterZone)
                {
                    return 90;
                }
            }
            
            return 100;
        }



        public override IList<ClientCard> OnSelectFusionMaterial(IList<ClientCard> cards, int min, int max)
        {
            var sorted = cards.OrderBy(GetFusionMaterialScore).ToList();
            return sorted.Take(max).ToList();
        }

        private bool InvokedSummonCondition()
        {
            // All Invoked fusion summons are handled via Invocation/InvocationSword/RosaMundi.
            // This SpSummon executor is a backup for game's auto-summon mechanics.
            // Check that the fusion target is appropriate based on board state.
            if (Card == null) return true;

            FusionType preferred = GetPreferredFusionTarget();
            int preferredId = GetFusionCardId(preferred);

            // If the game wants us to summon the preferred fusion, allow it
            if (Card.IsCode(preferredId)) return true;

            // Also allow alternative fusion if preferred is not available
            // E.g. if Invocation selects Mechaba but materials only allow Purgatrio
            return true;
        }



        private bool SpellbookMagicianEffect()
        {
            if (ShouldSkipCombo()) return false;
            return GetRemainingCount(CardId.SpellbookOfSecrets) > 0 || GetRemainingCount(CardId.SpellbookOfKnowledge) > 0;
        }
    }

    [Deck("Expert_2026_Invoke", "2026_Invoke")]
    public class ExpertInvokeExecutor : _2026_InvokeExecutor
    {
        private string _duelId;
        public ExpertInvokeExecutor(GameAI ai, Duel duel) : base(ai, duel)
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
