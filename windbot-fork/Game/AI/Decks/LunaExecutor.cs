using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Plugins;

namespace WindBot.Game.AI.Decks
{
    // ============================================================
    // CARD AUDIT — Luna (Lunalight Fusion Turbo & OTK)
    // ============================================================
    // | Card Name           | Type       | OPT? | Effect Summary                                                   |
    // |---------------------|------------|------|------------------------------------------------------------------|
    // | Lunalight Gold Leo  | Monster    | Yes  | On NS/SS search Lunalight & discard 1; GY: add back sent Lunalight|
    // | Lunalight Silver Hnd| Monster    | Yes  | Sent to GY by effect -> SS Lunalight from Deck; GY: Quick negate S/T|
    // | Lunalight KaleidoChk| Monster    | Yes  | Dump Lunalight from Deck/Extra to copy name; GY: add Poly from GY|
    // | Lunalight Yellow Mrt| Monster    | Yes  | Bounce Tiger/Lunalight on field to SS from hand/GY; GY: search S/T|
    // | Lunalight EmeraldBrd| Monster    | Yes  | On NS/SS discard Lunalight to draw 1; GY: SS Lv4 or lower from GY |
    // | Lunalight BlackSheep| Monster    | Yes  | Discard to search Poly or recycle GY; Fusion mat: add from GY/Extra|
    // | Lunalight Tiger     | Pendulum   | Soft | Scale 5: Revive 1 Lunalight from GY (Bounce with Marten to reuse!)|
    // | Lunalight Wolf      | Pendulum   | Soft | Scale 1: Miracle Fusion from field and GY!                      |
    // | Tri-Brigade Fraktall| Monster    | Yes  | Discard to dump Silver Hound -> triggers SS from Deck!           |
    // | Luna Light Perfume  | Spell      | Yes  | Monster Reborn for Lunalight; GY: banish & discard to search mon |
    // | Fire Formation Tenki| Spell      | Yes  | Search Level 4 or lower Beast-Warrior (Gold Leo, Tiger, Chick)   |
    // | Apex Polymerization | Spell      | Oath | Pay 2000 LP: send Lv3/4 Lunalight -> SS Fusion boss from Extra!  |
    // | Polymerization      | Spell      | No   | Standard fusion from hand and field                              |
    // | Lunalight Fusion    | Spell      | Yes  | Fusion summon; if opp has Extra Deck mon, dump from Deck/Extra   |
    // | Lunalight Masquerade| Continuous | Yes  | Dump Lunalight from Deck to GY; recycle Poly from GY             |
    // | Forbidden Droplet   | QuickSpell | No   | Send cards from field/hand to negate enemy monsters              |
    // | Lunalight Perfume D | Fusion Lv6 | Yes  | 2 Lunalights: on summon search Perfume; bounce Tiger to SS mon   |
    // | Lunalight Panther D | Fusion Lv8 | Yes  | Double attack each monster; fodder for Leo Dancer                |
    // | Lunalight Leo Dancer| Fusion Lv10| Yes  | 3500 ATK, untargetable, indestructible, double attack, SS wipe   |
    // | Lunalight Liger D   | Fusion Lv11| Yes  | 3800 ATK, immune, double attack, Quick SS board wipe             |
    // | Lunalight Sabre D   | Fusion Lv9 | Yes  | 3000+ ATK, untargetable, gains ATK, GY: +3000 ATK boost          |
    // | Number 41: Bagooska | Xyz Rank 4 | Yes  | Turn 1 floodgate (MUST ALWAYS BE FACE-UP DEFENSE)                |
    // ============================================================

    [Deck("Luna", "Luna")]
    [Deck("Lunalight", "Luna")]
    public class LunaExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int GoldLeo = 8379983;
            public const int SilverHound = 35763582;
            public const int BlackSheep = 11317977;
            public const int KaleidoChick = 35618217;
            public const int YellowMarten = 50546208;
            public const int EmeraldBird = 14152693;
            public const int Wolf = 47705572;
            public const int Tiger = 83190280;
            public const int TriBrigadeFraktall = 87209160;

            // Hand Traps & Disruptions
            public const int MulcharmyFuwalos = 42141493;
            public const int MulcharmyPurulia = 84192580;
            public const int AshBlossom = 14558128;
            public const int AshBlossomAlt = 14558127;
            public const int DrollAndLockBird = 94145021;

            // Spells & Traps
            public const int LunalightFusion = 87931906;
            public const int LunalightMasquerade = 2344618;
            public const int LunaLightPerfume = 48444114;
            public const int Tenki = 57103969;
            public const int Polymerization = 24094653;
            public const int ApexPolymerization = 44886582;
            public const int ForbiddenDroplet = 24299458;
            public const int CrossoutDesignator = 65681983;
            public const int TripleTacticsTalent = 25311006;
            public const int FoolishBurial = 81439174;
            public const int CalledByTheGrave = 24224830;
            public const int DominusImpulse = 40366667;
            public const int InfiniteImpermanence = 10045474;

            // Extra Deck
            public const int LigerDancer = 54701958;
            public const int LeoDancer = 24550676;
            public const int PerfumeDancer = 81196066;
            public const int PantherDancer = 97165977;
            public const int SabreDancer = 88753594;
            public const int TigerKing = 96381979;
            public const int Dugares = 66011101;
            public const int Nyarla = 8809344;
            public const int Bagooska = 90590304;
            public const int Almiraj = 60303245;
            public const int SpLittleKnight = 29301450;
            public const int GravityController = 23656668;
        }

        public static readonly int[] AceCardIds = {
            CardId.LigerDancer,
            CardId.LeoDancer,
            CardId.PerfumeDancer,
            CardId.SabreDancer,
            CardId.PantherDancer
        };

        // Once per turn state flags
        private bool _goldLeoSummonUsed;
        private bool _goldLeoGyRetrievalUsed;
        private bool _perfumeDancerSearchUsed;
        private bool _perfumeDancerBounceUsed;
        private bool _perfumeDancerGyUsed;
        private bool _martenGyUsed;
        private bool _martenBounceUsed;
        private bool _chickSendUsed;
        private bool _chickGraveUsed;
        private bool _emeraldSummonUsed;
        private bool _emeraldGraveUsed;
        private bool _perfumeSearchUsed;
        private bool _apexPolyUsed;
        private bool _fraktallUsed;
        private bool _silverHoundSummonUsed;
        private bool _silverHoundGyUsed;
        private bool _masqueradeUsed;

        public bool HasNormalSummonedThisTurn { get; private set; }
        public bool CanDealLethalCheck() => CanDealLethal();

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Contains(card.Id);
        }

        public bool IsLunalightCard(int id)
        {
            return id == CardId.GoldLeo || id == CardId.SilverHound || id == CardId.BlackSheep ||
                   id == CardId.KaleidoChick || id == CardId.YellowMarten || id == CardId.EmeraldBird ||
                   id == CardId.Wolf || id == CardId.Tiger || id == CardId.LunalightFusion ||
                   id == CardId.LunalightMasquerade || id == CardId.LunaLightPerfume ||
                   id == CardId.LigerDancer || id == CardId.LeoDancer || id == CardId.PerfumeDancer ||
                   id == CardId.PantherDancer || id == CardId.SabreDancer;
        }

        public LunaExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // Connect Decoupled Domain Plugin
            DeckPlugin = new LunaPlugin(this);

            // Register Ace Cards in Core modules
            ResourcePlan.RegisterAceCards(AceCardIds);
            HeuristicGuard.RegisterAceCards(AceCardIds);

            // Combo Router: Authentic Lunalight Sequencing
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "GoldLeo-Starter",
                RequiredCards = new List<int> { CardId.GoldLeo },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.GoldLeo, ActionType = ExecutorType.Summon, Description = "Normal Summon Gold Leo" },
                    new() { CardId = CardId.GoldLeo, ActionType = ExecutorType.Activate, Description = "Gold Leo search Tiger & discard" }
                },
                EndBoardScore = 85,
                Condition = () => Bot.Hand.Any(c => c != null && c.Id == CardId.GoldLeo) && !HasNormalSummonedThisTurn
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Tenki-Starter",
                RequiredCards = new List<int> { CardId.Tenki },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.Tenki, ActionType = ExecutorType.Activate, Description = "Activate Tenki to search Gold Leo / Tiger" }
                },
                EndBoardScore = 80,
                Condition = () => Bot.Hand.Any(c => c != null && c.Id == CardId.Tenki)
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Fraktall-Starter",
                RequiredCards = new List<int> { CardId.TriBrigadeFraktall },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.TriBrigadeFraktall, ActionType = ExecutorType.Activate, Description = "Discard Fraktall to dump Silver Hound" }
                },
                EndBoardScore = 75,
                Condition = () => Bot.Hand.Any(c => c != null && c.Id == CardId.TriBrigadeFraktall)
            });

            // Bait Planner
            BaitPlanner.RegisterComboStarters(CardId.GoldLeo, CardId.Tenki, CardId.KaleidoChick, CardId.TriBrigadeFraktall);
            BaitPlanner.RegisterBaitCards(CardId.Tenki, CardId.FoolishBurial, CardId.TripleTacticsTalent);

            // Chain Advisor
            ChainAdvisor.RegisterHighValueTargets(CardId.GoldLeo, CardId.KaleidoChick, CardId.Tiger, CardId.Wolf, CardId.PerfumeDancer);

            // Register Optional Field Removal Cards
            RegisterOptionalFieldRemovalCards(CardId.LigerDancer, CardId.SpLittleKnight);

            // ============================================================
            // TIER 1: Hand Traps & Fast Chains
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyPurulia, MulcharmyPuruliaEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAlt, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.CrossoutDesignator, CrossoutDesignatorEffect);
            AddExecutor(ExecutorType.Activate, CardId.DominusImpulse, DominusImpulseEffect);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, InfiniteImpermanenceEffect);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, () => false);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletEffect);

            // ============================================================
            // TIER 2: Board Breakers
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentEffect);

            // ============================================================
            // TIER 3: Searchers & Setup Spells (Activate Before Normal Summon)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.Tenki, TenkiEffect);
            AddExecutor(ExecutorType.Activate, CardId.FoolishBurial, FoolishBurialEffect);
            AddExecutor(ExecutorType.Activate, CardId.TriBrigadeFraktall, TriBrigadeFraktallEffect);
            AddExecutor(ExecutorType.Activate, CardId.BlackSheep, BlackSheepEffect);
            AddExecutor(ExecutorType.Activate, CardId.LunalightMasquerade, LunalightMasqueradeEffect);

            // ============================================================
            // TIER 4: Normal & Special Summons
            // ============================================================
            AddExecutor(ExecutorType.SpSummon, CardId.GoldLeo, GoldLeoSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SilverHound, SilverHoundSpSummon);
            AddExecutor(ExecutorType.Summon, CardId.KaleidoChick, KaleidoChickSummon);
            AddExecutor(ExecutorType.Summon, CardId.GoldLeo, GoldLeoSummon);
            AddExecutor(ExecutorType.Summon, CardId.EmeraldBird, EmeraldBirdSummon);
            AddExecutor(ExecutorType.Summon, CardId.BlackSheep, BlackSheepSummon);
            AddExecutor(ExecutorType.Summon, CardId.SilverHound, SilverHoundSummon);

            // ============================================================
            // TIER 5: Pendulum Scales & Monster Ignition / Revives
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.Tiger, TigerPendulumEffect);
            AddExecutor(ExecutorType.Activate, CardId.GoldLeo, GoldLeoEffect);
            AddExecutor(ExecutorType.Activate, CardId.SilverHound, SilverHoundEffect);
            AddExecutor(ExecutorType.Activate, CardId.KaleidoChick, KaleidoChickEffect);
            AddExecutor(ExecutorType.Activate, CardId.YellowMarten, YellowMartenEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.YellowMarten, YellowMartenSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.EmeraldBird, EmeraldBirdEffect);
            AddExecutor(ExecutorType.Activate, CardId.PerfumeDancer, PerfumeDancerEffect);
            AddExecutor(ExecutorType.Activate, CardId.LunaLightPerfume, LunaLightPerfumeEffect);

            // ============================================================
            // TIER 7: Fusion Spells (Apex Poly, Lunalight Fusion, Polymerization, Wolf)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.ApexPolymerization, ApexPolymerizationEffect);
            AddExecutor(ExecutorType.Activate, CardId.LunalightFusion, LunalightFusionEffect);
            AddExecutor(ExecutorType.Activate, CardId.Polymerization, PolymerizationEffect);
            AddExecutor(ExecutorType.Activate, CardId.Wolf, WolfPendulumEffect);

            // ============================================================
            // TIER 8: Extra Deck Summons
            // ============================================================
            AddExecutor(ExecutorType.SpSummon, CardId.PerfumeDancer, PerfumeDancerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.LeoDancer, LeoDancerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.LigerDancer, LigerDancerSummon);
            AddExecutor(ExecutorType.Activate, CardId.LigerDancer, LigerDancerEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.SabreDancer, SabreDancerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.PantherDancer, PantherDancerSummon);

            // Rank 4 & Links
            AddExecutor(ExecutorType.SpSummon, CardId.TigerKing, TigerKingSummon);
            AddExecutor(ExecutorType.Activate, CardId.TigerKing, TigerKingEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Dugares, DugaresSummon);
            AddExecutor(ExecutorType.Activate, CardId.Dugares, DugaresEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Bagooska, BagooskaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SpLittleKnight, SpLittleKnightSummon);
            AddExecutor(ExecutorType.Activate, CardId.SpLittleKnight);
            AddExecutor(ExecutorType.SpSummon, CardId.GravityController, GravityControllerSummon);

            // ============================================================
            // TIER 9: Fallbacks & Sets
            // ============================================================
            AddExecutor(ExecutorType.SpellSet, SetTrapCondition);
            AddExecutor(ExecutorType.Repos, MonsterRepos);
            AddExecutor(ExecutorType.Summon, FallbackNormalSummon);
        }

        public override bool OnSelectHand()
        {
            // Lunalight is an aggressive OTK deck — prefer going second
            return false;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _goldLeoSummonUsed = false;
            _goldLeoGyRetrievalUsed = false;
            _perfumeDancerSearchUsed = false;
            _perfumeDancerBounceUsed = false;
            _perfumeDancerGyUsed = false;
            _martenGyUsed = false;
            _martenBounceUsed = false;
            _chickSendUsed = false;
            _chickGraveUsed = false;
            _emeraldSummonUsed = false;
            _emeraldGraveUsed = false;
            _perfumeSearchUsed = false;
            _apexPolyUsed = false;
            _fraktallUsed = false;
            _silverHoundSummonUsed = false;
            _silverHoundGyUsed = false;
            _masqueradeUsed = false;
            HasNormalSummonedThisTurn = false;
        }

        protected override bool IsBoardStrongEnough()
        {
            int count = 0;
            if (Bot.HasInMonstersZone(CardId.LigerDancer)) count += 3;
            if (Bot.HasInMonstersZone(CardId.LeoDancer)) count += 3;
            if (Bot.HasInMonstersZone(CardId.SabreDancer)) count += 2;
            if (Bot.HasInMonstersZone(CardId.Bagooska)) count += 2;
            if (Bot.HasInMonstersZone(CardId.SpLittleKnight)) count += 1;
            if (Bot.HasInHand(CardId.AshBlossom) || Bot.HasInHand(CardId.AshBlossomAlt)) count += 1;
            if (Bot.HasInHand(CardId.InfiniteImpermanence)) count += 1;
            return count >= 3;
        }

        protected override bool ShouldStopExtending()
        {
            if (CanDealLethal()) return true;
            if (IsBoardStrongEnough() && Duel.Turn == 1) return true;
            return false;
        }

        // ============================================================
        // HAND TRAPS
        // ============================================================
        private bool MulcharmyFuwalosEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }

        private bool MulcharmyPuruliaEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }

        private bool DrollAndLockBirdEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1;
        }

        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard == null || LastChainCard.Controller != 1) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool CalledByTheGraveEffect()
        {
            if (LastChainCard == null || LastChainCard.Controller != 1) return false;
            return DefaultCalledByTheGrave();
        }

        private bool DominusImpulseEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard == null || LastChainCard.Controller != 1) return false;
            return true;
        }

        private bool CrossoutDesignatorEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard == null || LastChainCard.Controller != 1) return false;

            int code = LastChainCard.Id;
            int alias = LastChainCard.Alias;
            if (alias != 0 && alias - code < 10) code = alias;
            if (code == 0) return false;

            if (GetRemainingCount(code) > 0)
            {
                AI.SelectAnnounceID(code);
                return true;
            }
            return false;
        }

        private bool InfiniteImpermanenceEffect()
        {
            if (Util.ChainContainsCard(CardId.InfiniteImpermanence)) return false;

            // Proactively negate continuous negators/floodgates on our turn before casting Fusion spells
            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Battle))
            {
                var priorityTarget = Enemy.MonsterZone.GetMonsters().FirstOrDefault(c =>
                    c != null && c.IsFaceup() && !c.IsDisabled() &&
                    (c.IsCode(63767246, 59822133, 84013237, 21044178, 27548199, 90590304) ||
                     c.IsFloodgate() || CardIntelligence.IsKnownNegator(c.Id)));

                if (priorityTarget != null)
                {
                    AI.SelectCard(priorityTarget);
                    return true;
                }
            }

            return DefaultInfiniteImpermanence();
        }

        private bool ForbiddenDropletEffect()
        {
            // STRICT RULE: Never chain to our own card's activation!
            if (Duel.CurrentChain.Count > 0 && Duel.LastChainPlayer == 0) return false;

            var targets = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsDisabled() && c.HasType(CardType.Effect)).ToList();
            if (targets.Count == 0) return false;

            // 1. Response to enemy monster effect
            if (Duel.LastChainPlayer == 1 && LastChainCard != null && LastChainCard.Location == CardLocation.MonsterZone && !LastChainCard.IsDisabled())
            {
                return true;
            }

            // 2. Open board break in MP1 before starting combos (no chain active)
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1 && Duel.CurrentChain.Count == 0)
            {
                // Only activate if we have safe fodder (Tenki, Masquerade, or surplus cards, NOT only fusion spell/scales!)
                bool hasSafeFodder = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.Tenki, CardId.LunalightMasquerade)) ||
                                     Bot.Hand.Count(c => c != null && c.Id != CardId.ForbiddenDroplet && !c.IsCode(CardId.Polymerization, CardId.LunalightFusion, CardId.Tiger, CardId.Wolf, CardId.KaleidoChick)) >= 1;
                return hasSafeFodder;
            }

            // 3. Battle Phase
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Battle && Duel.CurrentChain.Count == 0)
            {
                return true;
            }

            return false;
        }

        private bool TripleTacticsTalentEffect()
        {
            if (Enemy.GetMonsterCount() > 0 && _isGoingSecond)
            {
                AI.SelectOption(1); // Steal monster for fusion / lethal
            }
            else
            {
                AI.SelectOption(0); // Draw 2
            }
            return true;
        }

        // ============================================================
        // SEARCHERS & SETUP SPELLS
        // ============================================================
        private bool TenkiEffect()
        {
            if (ShouldSkipCombo()) return false;
            return true;
        }

        private bool FoolishBurialEffect()
        {
            if (ShouldSkipCombo()) return false;
            AI.SelectCard(CardId.SilverHound, CardId.YellowMarten, CardId.EmeraldBird);
            return true;
        }

        private bool TriBrigadeFraktallEffect()
        {
            if (_fraktallUsed) return false;
            if (ShouldSkipCombo()) return false;
            _fraktallUsed = true;
            return true;
        }

        private bool BlackSheepEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                bool hasPoly = Bot.Hand.Any(c => c != null && c.IsCode(CardId.Polymerization, CardId.LunalightFusion, CardId.ApexPolymerization));
                if (!hasPoly && Bot.GetRemainingCount(CardId.Polymerization, 1) > 0)
                {
                    AI.SelectOption(0); // Search Polymerization from Deck
                }
                else
                {
                    AI.SelectOption(1); // Add Lunalight from GY
                }
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Trigger when sent to GY as fusion material -> recycle Lunalight!
                return true;
            }
            return false;
        }

        private bool LunalightMasqueradeEffect()
        {
            if (Card.Location == CardLocation.Hand || Card.IsFacedown())
            {
                return true;
            }
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup() && !_masqueradeUsed)
            {
                _masqueradeUsed = true;
                return true;
            }
            return false;
        }

        // ============================================================
        // NORMAL SUMMONS
        // ============================================================
        private bool GoldLeoSummon()
        {
            if (HasNormalSummonedThisTurn) return false;
            HasNormalSummonedThisTurn = true;
            return true;
        }
        private bool KaleidoChickSummon()
        {
            if (HasNormalSummonedThisTurn) return false;
            HasNormalSummonedThisTurn = true;
            return true;
        }
        private bool EmeraldBirdSummon()
        {
            if (HasNormalSummonedThisTurn) return false;
            HasNormalSummonedThisTurn = true;
            return true;
        }
        private bool BlackSheepSummon()
        {
            if (HasNormalSummonedThisTurn) return false;
            if (!Bot.Hand.Any(c => c != null && c.IsCode(CardId.Polymerization, CardId.LunalightFusion))) return false;
            HasNormalSummonedThisTurn = true;
            return true;
        }
        private bool SilverHoundSummon()
        {
            if (HasNormalSummonedThisTurn) return false;
            HasNormalSummonedThisTurn = true;
            return true;
        }

        private bool GoldLeoSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.MonsterZone.GetMonsters().Any(c => c != null && c.IsFaceup() && IsLunalightCard(c.Id));
        }

        private bool SilverHoundSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.MonsterZone.GetMonsters().Any(c => c != null && c.IsFaceup() && IsLunalightCard(c.Id));
        }

        // ============================================================
        // MONSTER EFFECTS
        // ============================================================
        private bool GoldLeoEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (!_goldLeoSummonUsed)
                {
                    _goldLeoSummonUsed = true;
                    return true;
                }
                if (!_goldLeoGyRetrievalUsed)
                {
                    _goldLeoGyRetrievalUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool SilverHoundEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;

            // Effect 0: Sent to GY by effect -> Special Summon 1 Lunalight from Deck!
            // Stringid 0 = Deck Special Summon
            if (ActivateDescription == Util.GetStringId(CardId.SilverHound, 0) || 
                (ActivateDescription == -1 && !_silverHoundGyUsed))
            {
                if (!_silverHoundGyUsed && !IsSpecialSummonBlocked() && Bot.GetMonsterCount() < 5)
                {
                    _silverHoundGyUsed = true;
                    return true;
                }
                return false;
            }

            // Effect 1: Quick Negate of on-field Spell/Trap by banishing Silver Hound + 1 Fusion
            // Stringid 1 = Spell/Trap Negate
            if (ActivateDescription == Util.GetStringId(CardId.SilverHound, 1))
            {
                // STRICT RULE: NEVER negate our own cards! Opponent ONLY!
                if (Duel.LastChainPlayer != 1) return false;
                ClientCard currentChainCard = Duel.GetCurrentChainCard();
                if (currentChainCard == null || currentChainCard.Controller != 1) return false;
                if (!currentChainCard.IsSpell() && !currentChainCard.IsTrap()) return false;

                // Check that we have a Lunalight Fusion in GY that is NOT our only Panther Dancer (if needed for Leo)
                bool hasFodderFusion = Bot.Graveyard.Any(c => c != null && c.HasType(CardType.Fusion) && IsLunalightCard(c.Id) && c.Id != CardId.PantherDancer);
                if (hasFodderFusion) return true;

                // If Panther Dancer is the only one, only negate if the enemy spell is a heavy board wipe / dangerous card
                if (currentChainCard.IsFloodgate() || currentChainCard.IsCode(18144506, 27551, 12580477, 99745551))
                {
                    return Bot.Graveyard.Any(c => c != null && c.HasType(CardType.Fusion) && IsLunalightCard(c.Id));
                }
            }

            return false;
        }

        private bool KaleidoChickEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.MonsterZone && !_chickSendUsed)
            {
                _chickSendUsed = true;
                return true;
            }
            if (Card.Location == CardLocation.Grave && !_chickGraveUsed)
            {
                _chickGraveUsed = true;
                return true;
            }
            return false;
        }

        private bool YellowMartenSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (_martenBounceUsed) return false;

            // 1. Bounce Tiger in SpellZone so it can be re-scaled and used again!
            ClientCard target = Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.Tiger));
            if (target == null)
            {
                // 2. Bounce Lunalight Masquerade (Continuous Spell) - only Lunalight cards are valid!
                target = Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.LunalightMasquerade));
            }
            if (target == null && Bot.GetMonsterCount() >= 2)
            {
                // 3. Bounce a non-ace Lunalight monster ONLY if we have at least 2 monsters on field!
                target = Bot.MonsterZone.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && IsLunalightCard(c.Id) && !c.IsCode(CardId.YellowMarten) && !IsAceCard(c));
            }

            if (target != null)
            {
                AI.SelectCard(target.Id);
                _martenBounceUsed = true;
                return true;
            }
            return false;
        }

        private bool YellowMartenEffect()
        {
            if (Card.Location == CardLocation.Grave && !_martenGyUsed)
            {
                _martenGyUsed = true;
                return true;
            }
            return false;
        }

        private bool EmeraldBirdEffect()
        {
            if (Card.Location == CardLocation.MonsterZone && !_emeraldSummonUsed)
            {
                if (Bot.Hand.Count > 0)
                {
                    _emeraldSummonUsed = true;
                    return true;
                }
            }
            if (Card.Location == CardLocation.Grave && !_emeraldGraveUsed)
            {
                _emeraldGraveUsed = true;
                return true;
            }
            return false;
        }

        private bool LunaLightPerfumeEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.Hand)
            {
                return Bot.Graveyard.Any(c => c != null && c.IsCanRevive() && IsLunalightCard(c.Id));
            }
            else if (Card.Location == CardLocation.Grave && !_perfumeSearchUsed)
            {
                if (Bot.Hand.Count > 0)
                {
                    _perfumeSearchUsed = true;
                    return true;
                }
            }
            return false;
        }

        // ============================================================
        // PENDULUM EFFECTS
        // ============================================================
        private bool TigerPendulumEffect()
        {
            // Place in scale if in hand and we don't have Tiger scaled
            if (Card.Location == CardLocation.Hand)
            {
                return !Bot.HasInSpellZone(CardId.Tiger);
            }

            // In SpellZone: Revive a Lunalight from GY!
            if (Card.Location == CardLocation.SpellZone)
            {
                return Bot.Graveyard.Any(c => c != null && c.IsCanRevive() && IsLunalightCard(c.Id));
            }
            return false;
        }

        private bool WolfPendulumEffect()
        {
            if (IsSpecialSummonBlocked()) return false;

            // Place Wolf in scale if in hand and we are ready for Miracle Fusion
            if (Card.Location == CardLocation.Hand)
            {
                if (Bot.HasInSpellZone(CardId.Wolf)) return false;
                int totalMats = Bot.MonsterZone.GetMonsters().Count(c => c != null && IsLunalightCard(c.Id)) +
                                Bot.Graveyard.Count(c => c != null && IsLunalightCard(c.Id));
                return totalMats >= 2;
            }

            // In SpellZone: Miracle Fusion activation
            if (Card.Location == CardLocation.SpellZone)
            {
                int totalMats = Bot.MonsterZone.GetMonsters().Count(c => c != null && IsLunalightCard(c.Id)) +
                                Bot.Graveyard.Count(c => c != null && IsLunalightCard(c.Id));
                return totalMats >= 2;
            }
            return false;
        }

        private bool PerfumeDancerEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // 1. Fusion Summon Trigger: Search Luna Light Perfume from Deck!
                if ((ActivateDescription == Util.GetStringId(CardId.PerfumeDancer, 0) || ActivateDescription == -1) &&
                    !_perfumeDancerSearchUsed && Bot.GetRemainingCount(CardId.LunaLightPerfume, 1) > 0)
                {
                    _perfumeDancerSearchUsed = true;
                    return true;
                }

                // 2. Field Ignition: Bounce Tiger/Lunalight on field to SS Lunalight from hand
                if ((ActivateDescription == Util.GetStringId(CardId.PerfumeDancer, 1) || ActivateDescription == -1) &&
                    !_perfumeDancerBounceUsed)
                {
                    ClientCard target = Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.Tiger));
                    if (target == null)
                    {
                        target = Bot.MonsterZone.GetMonsters().FirstOrDefault(c => c != null && c != Card && IsLunalightCard(c.Id) && !IsAceCard(c));
                    }

                    if (target != null && Bot.Hand.Any(c => c != null && IsLunalightCard(c.Id)))
                    {
                        AI.SelectCard(target.Id);
                        _perfumeDancerBounceUsed = true;
                        return true;
                    }
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // 3. GY Ignition: Banish self to debuff all opponent monsters ATK by their DEF
                if ((ActivateDescription == Util.GetStringId(CardId.PerfumeDancer, 2) || ActivateDescription == -1) &&
                    !_perfumeDancerGyUsed && Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2) && Enemy.GetMonsterCount() > 0)
                {
                    _perfumeDancerGyUsed = true;
                    return true;
                }
            }
            return false;
        }

        // ============================================================
        // FUSION SPELLS
        // ============================================================
        private bool ApexPolymerizationEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            // LP Safety check: costs 2000 LP
            if (Bot.LifePoints <= 2000) return false;
            if (_apexPolyUsed) return false;

            // Target Lv3 or Lv4 Lunalight on field -> Special Summons Lv6/8/9/10/11 Fusion!
            var target = Bot.MonsterZone.GetMonsters().FirstOrDefault(c =>
                c != null && c.IsFaceup() && !c.HasType(CardType.Xyz | CardType.Link) && (c.Level == 3 || c.Level == 4) && IsLunalightCard(c.Id));

            if (target != null)
            {
                AI.SelectCard(target);
                _apexPolyUsed = true;
                return true;
            }
            return false;
        }

        private bool LunalightFusionEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            bool oppHasExtra = Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && (c.HasType(CardType.Fusion | CardType.Xyz | CardType.Synchro | CardType.Link)));
            if (oppHasExtra)
            {
                return true;
            }
            int availableNonAceMats = Bot.Hand.Count(c => c != null && IsLunalightCard(c.Id)) +
                                      Bot.MonsterZone.GetMonsters().Count(c => c != null && IsLunalightCard(c.Id) && !IsAceCard(c));
            return availableNonAceMats >= 2;
        }

        private bool PolymerizationEffect()
        {
            if (IsSpecialSummonBlocked()) return false;

            int availableNonAceMats = Bot.Hand.Count(c => c != null && IsLunalightCard(c.Id)) +
                                      Bot.MonsterZone.GetMonsters().Count(c => c != null && IsLunalightCard(c.Id) && !IsAceCard(c));

            // If we don't have Perfume Dancer on field, 2 non-ace materials make Perfume Dancer
            if (!Bot.HasInMonstersZone(CardId.PerfumeDancer))
            {
                return availableNonAceMats >= 2;
            }

            // If Perfume Dancer is already on field, only activate Poly if we can make a higher Boss:
            bool hasPantherInGyOrField = Bot.Graveyard.Any(c => c != null && c.Id == CardId.PantherDancer) ||
                                          Bot.MonsterZone.GetMonsters().Any(c => c != null && c.Id == CardId.PantherDancer);
            bool hasChickOnField = Bot.MonsterZone.GetMonsters().Any(c => c != null && c.Id == CardId.KaleidoChick);

            // Leo Dancer requires Panther (or Chick copying it) + 2 Lunalights
            if (hasPantherInGyOrField || hasChickOnField)
            {
                return availableNonAceMats >= 2;
            }

            // Sabre Dancer requires 3 Lunalights
            if (availableNonAceMats >= 3) return true;

            return false;
        }

        // ============================================================
        // EXTRA DECK SUMMONS
        // ============================================================
        private bool PerfumeDancerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            // Never summon Perfume Dancer if we already have one on field!
            if (Bot.HasInMonstersZone(CardId.PerfumeDancer)) return false;
            return true;
        }

        private bool LeoDancerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool LigerDancerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool LigerDancerEffect()
        {
            // Wipe all opponent's Special Summoned monsters!
            return Enemy.GetMonsters().Any(c => c != null && c.IsSpecialSummoned);
        }

        private bool PantherDancerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool SabreDancerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool TigerKingSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            int lvl4BeastWarriors = Bot.MonsterZone.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 4 && c.HasRace(CardRace.BestWarrior) && !IsAceCard(c));
            if (lvl4BeastWarriors < 2) return false;
            return !Bot.HasInSpellZone(CardId.Tenki) && Bot.GetRemainingCount(CardId.Tenki, 1) > 0;
        }

        private bool TigerKingEffect()
        {
            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled() && !c.HasRace(CardRace.BestWarrior));
        }

        private bool DugaresSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            int lvl4s = Bot.MonsterZone.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 4 && !IsAceCard(c));
            if (lvl4s < 2) return false;
            return Bot.Graveyard.Any(c => c != null && c.IsCanRevive() && IsLunalightCard(c.Id));
        }

        private bool DugaresEffect()
        {
            AI.SelectOption(0); // Option 0: Special Summon from GY
            return true;
        }

        private bool BagooskaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            int lvl4s = Bot.MonsterZone.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 4 && !IsAceCard(c));
            if (lvl4s < 2) return false;
            // RULE #12: Bagooska MUST ALWAYS BE DEFENSE!
            if ((Duel.Turn == 1 || Duel.Phase == DuelPhase.Main2) && Duel.Player == 0)
            {
                AI.SelectPosition(CardPosition.FaceUpDefence);
                return true;
            }
            return false;
        }

        private bool SpLittleKnightSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            int fodderCount = Bot.MonsterZone.GetMonsters().Count(c => c != null && c.IsFaceup() && (!IsAceCard(c) || c.Owner == 1));
            if (fodderCount < 2) return false;
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1 && !_isGoingSecond) return false;
            return Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
        }

        private bool GravityControllerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var emzCard = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Location == CardLocation.MonsterZone && (c.Sequence == 5 || c.Sequence == 6));
            if (emzCard != null && IsLunalightCard(emzCard.Id) && !IsAceCard(emzCard))
            {
                AI.SelectCard(emzCard.Id);
                return true;
            }
            return false;
        }

        private bool SetTrapCondition() => Util.IsTurn1OrMain2();

        private bool FallbackNormalSummon()
        {
            if (HasNormalSummonedThisTurn) return false;
            var nonHandtrap = Bot.Hand.FirstOrDefault(c => c != null && c.IsMonster() && c.Level <= 4 &&
                !c.IsCode(CardId.AshBlossom, CardId.AshBlossomAlt, CardId.DrollAndLockBird, CardId.MulcharmyFuwalos, CardId.MulcharmyPurulia, CardId.Tiger, CardId.Wolf));
            if (nonHandtrap != null)
            {
                HasNormalSummonedThisTurn = true;
                AI.SelectCard(nonHandtrap);
                return true;
            }
            return false;
        }

        private bool MonsterRepos()
        {
            if (Card == null) return false;
            // CRITICAL RULE #12: Never switch Bagooska to Attack!
            if (Card.IsCode(CardId.Bagooska)) return false;

            if (IsAceCard(Card))
            {
                if (Card.IsDefense()) return true; // Switch to Attack
                return false;
            }

            bool enemyEmpty = Enemy.GetMonsterCount() == 0;
            if (Card.Attack <= 1400 && Card.IsAttack()) return true; // Switch to DEF

            if (Card.IsDefense() && enemyEmpty && Card.Attack >= 1500)
                return true;

            return false;
        }

        // ============================================================
        // ON SELECT POSITION — STRICT RULE #12 COMPLIANCE
        // ============================================================
        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // CRITICAL USER RULE #12: Bagooska MUST ALWAYS BE FaceUpDefence!
            if (cardId == CardId.Bagooska)
            {
                if (positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence;
            }

            // Boss attackers go to FaceUpAttack
            if ((cardId == CardId.LigerDancer || cardId == CardId.LeoDancer
                || cardId == CardId.SabreDancer || cardId == CardId.PantherDancer
                || cardId == CardId.PerfumeDancer || cardId == CardId.TigerKing
                || cardId == CardId.Dugares || cardId == CardId.SpLittleKnight)
                && positions.Contains(CardPosition.FaceUpAttack))
            {
                return CardPosition.FaceUpAttack;
            }

            // Main deck low-ATK monsters stay in DEF for safety
            if (positions.Contains(CardPosition.FaceUpDefence))
                return CardPosition.FaceUpDefence;

            return base.OnSelectPosition(cardId, positions);
        }

        public override int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
            if (player == 0 && location == CardLocation.SpellZone)
            {
                // In Lunalight, we need Pendulum Zones (z0=0x1 and z4=0x10) for Tiger and Wolf!
                // So normal Spells/Traps/Continuous cards MUST prioritize middle zones (z2=0x4, z1=0x2, z3=0x8) first!
                int middleAvailable = available & (0x4 | 0x2 | 0x8);
                if (middleAvailable > 0)
                {
                    if ((middleAvailable & 0x4) > 0) return 0x4; // center
                    if ((middleAvailable & 0x2) > 0) return 0x2; // left center
                    if ((middleAvailable & 0x8) > 0) return 0x8; // right center
                }
            }
            return base.OnSelectPlace(cardId, player, location, available);
        }

        // ============================================================
        // ON SELECT CARD — DELEGATE TO PLUGIN & STRICT ENEMY DESTROY
        // ============================================================
        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // Wolf Miracle Fusion Materials (from our field/GY)
            if (Card != null && Card.Id == CardId.Wolf && (hint == 503 || hint == 511))
            {
                var sorted = DeckPlugin?.MaterialEvaluator?.SortMaterials(cards, min);
                if (sorted != null && sorted.Count >= min)
                {
                    return sorted.Take(max).ToList();
                }
            }

            // Mandatory Rule #1 & Skill 3.3: Removal / Destruction must target ENEMY ONLY!
            if (hint == 502 || hint == 503)
            {
                var enemyTargets = cards.Where(c => c != null && c.Controller == 1)
                    .OrderByDescending(c => {
                        int score = c.Attack;
                        if (c.IsFaceup())
                        {
                            if (c.IsFloodgate()) score += 10000;
                            if (c.IsExtraCard()) score += 5000;
                        }
                        return score;
                    }).ToList();

                if (enemyTargets.Count >= min)
                    return enemyTargets.Take(max).ToList();
            }

            // Forbidden Droplet targets & costs
            if (Card != null && Card.IsCode(CardId.ForbiddenDroplet))
            {
                if (cards.Any(c => c != null && c.Controller == 0))
                {
                    // Selecting cost from our field / hand:
                    // 1. Tenki on field (already searched)
                    var tenki = cards.FirstOrDefault(c => c != null && c.Location == CardLocation.SpellZone && c.IsCode(CardId.Tenki));
                    if (tenki != null) return new List<ClientCard> { tenki };

                    // 2. Lunalight Masquerade on field
                    var masquerade = cards.FirstOrDefault(c => c != null && c.Location == CardLocation.SpellZone && c.IsCode(CardId.LunalightMasquerade));
                    if (masquerade != null) return new List<ClientCard> { masquerade };

                    // 3. Silver Hound in Hand (triggers SS from deck!)
                    var hound = cards.FirstOrDefault(c => c != null && c.Location == CardLocation.Hand && c.IsCode(CardId.SilverHound));
                    if (hound != null) return new List<ClientCard> { hound };

                    // 4. Yellow Marten in Hand (triggers S/T search!)
                    var marten = cards.FirstOrDefault(c => c != null && c.Location == CardLocation.Hand && c.IsCode(CardId.YellowMarten));
                    if (marten != null) return new List<ClientCard> { marten };

                    // 5. Emerald Bird in Hand
                    var bird = cards.FirstOrDefault(c => c != null && c.Location == CardLocation.Hand && c.IsCode(CardId.EmeraldBird));
                    if (bird != null) return new List<ClientCard> { bird };

                    // Fallback safe fodder (exclude core fusion spells and scales!)
                    var safeFodder = cards.Where(c => c != null && c.Controller == 0 &&
                        !c.IsCode(CardId.Polymerization, CardId.LunalightFusion, CardId.Tiger, CardId.Wolf, CardId.KaleidoChick))
                        .OrderBy(c => c.Location == CardLocation.Hand ? 0 : 1)
                        .FirstOrDefault();
                    if (safeFodder != null) return new List<ClientCard> { safeFodder };
                }
                else if (cards.Any(c => c != null && c.Controller == 1))
                {
                    // Selecting enemy monster to negate
                    var target = cards.Where(c => c != null && c.Controller == 1 && c.IsFaceup() && !c.IsDisabled())
                        .OrderByDescending(c => {
                            int score = c.Attack;
                            if (c.IsFloodgate()) score += 10000;
                            if (c.IsExtraCard()) score += 5000;
                            return score;
                        }).FirstOrDefault();

                    if (target != null) return new List<ClientCard> { target };
                }
            }

            // 3. Deck Search (hint 506 = HINTMSG_ATOHAND) or all in Deck
            if (hint == 506 || (hint == 0 && cards.All(c => c.Location == CardLocation.Deck)))
            {
                var target = DeckPlugin?.Strategy?.PickSearchTarget(cards, Card);
                if (target != null)
                {
                    var result = new List<ClientCard> { target };
                    result.AddRange(cards.Where(c => c != target).Take(max - 1));
                    return result;
                }
            }

            // 4. Special Summon (hint 509 = HINTMSG_SPSUMMON)
            if (hint == 509)
            {
                var target = DeckPlugin?.Strategy?.PickSpecialSummonTarget(cards);
                if (target != null)
                {
                    var result = new List<ClientCard> { target };
                    result.AddRange(cards.Where(c => c != target).Take(max - 1));
                    return result;
                }
            }

            // 5. Send to Grave (hint 504 = HINTMSG_TOGRAVE)
            if (hint == 504)
            {
                var target = DeckPlugin?.Strategy?.PickFoolishGraveTarget(cards, Card);
                if (target != null)
                {
                    var result = new List<ClientCard> { target };
                    result.AddRange(cards.Where(c => c != target).Take(max - 1));
                    return result;
                }
            }

            // 6. Discard Cost (hint 501 = HINTMSG_DISCARD)
            if (hint == 501)
            {
                var target = DeckPlugin?.MaterialEvaluator?.PickDiscardTarget(cards, min);
                if (target != null)
                {
                    var result = new List<ClientCard> { target };
                    result.AddRange(cards.Where(c => c != target).Take(max - 1));
                    return result;
                }
            }

            // 7. Fusion / Xyz Materials (hint 511, 513, 533, 570)
            if (hint == 511 || hint == 513 || hint == 533 || hint == 570)
            {
                var sorted = DeckPlugin?.MaterialEvaluator?.SortMaterials(cards, min);
                if (sorted != null && sorted.Count >= min)
                {
                    return sorted.Take(max).ToList();
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        // ============================================================
        // ON SELECT OPTION & EFFECT YN
        // ============================================================
        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count <= 1)
                return 0;

            // 1. Apex Polymerization: Option 0 = Fusion Summon; Option 1 = Send to GY
            if (Card != null && Card.Id == CardId.ApexPolymerization)
            {
                return 0; // Always Fusion Summon
            }

            // 2. Black Sheep:
            // Option 0: Add Polymerization to hand
            // Option 1: Add Lunalight monster from GY to hand
            if (Card != null && Card.Id == CardId.BlackSheep)
            {
                bool hasPoly = Bot.Hand.Any(c => c != null && c.IsCode(CardId.Polymerization, CardId.LunalightFusion, CardId.ApexPolymerization));
                if (!hasPoly && Bot.GetRemainingCount(CardId.Polymerization, 1) > 0)
                    return 0;
                return Math.Min(1, options.Count - 1);
            }

            // 3. Triple Tactics Talent:
            // Option 0: Draw 2 cards
            // Option 1: Take control of opponent monster until End Phase
            // Option 2: Look at opponent hand and shuffle 1
            if (Card != null && Card.Id == CardId.TripleTacticsTalent)
            {
                if (Enemy.GetMonsterCount() > 0 && _isGoingSecond && options.Count > 1)
                {
                    var stealable = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup() && !m.IsShouldNotBeTarget()).ToList();
                    if (stealable.Any(m => m.IsFloodgate() || m.Attack >= 2500 || m.IsExtraCard()))
                        return 1; // Steal high-threat or boss monster
                }
                return 0; // Draw 2 cards (safe advantage)
            }

            // 4. Number 60: Dugares the Timeless:
            // Option 0: Draw 2, discard 1
            // Option 1: Special Summon 1 monster from GY in DEF
            // Option 2: Double ATK
            if (Card != null && Card.Id == CardId.Dugares)
            {
                if (Bot.Graveyard.Any(c => c != null && c.IsCanRevive() && IsLunalightCard(c.Id)) && options.Count > 1)
                    return 1; // Revive
                return 0; // Draw 2
            }

            int baseChoice = base.OnSelectOption(options);
            if (baseChoice >= 0 && baseChoice < options.Count)
                return baseChoice;

            return 0;
        }

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            if (card == null) return null;

            // RULE #14: Never accept opponent-initiated optional prompts
            if (card.Controller == 1) return false;

            // Always accept beneficial Lunalight effects
            if (IsLunalightCard(card.Id))
            {
                return true;
            }

            return base.OnSelectEffectYn(card, desc);
        }
    }

    [Deck("Expert_2026_Luna", "2026_Luna")]
    public class ExpertLunaExecutor : LunaExecutor
    {
        public ExpertLunaExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
        }
    }

    // Backward-compatibility class
    [Deck("2026_Luna_Compat", "2026_Luna")]
    public class _2026_LunaExecutor : LunaExecutor
    {
        public _2026_LunaExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
        }
    }
}
