// ============================================================
// CARD AUDIT โ€” 2026 Red Dragon
// ============================================================
// | Card Name                    | Type    | ATK/DEF | LV | Role              |
// |------------------------------|---------|---------|----|--------------------|
// | Soul Resonator               | Tuner   | 500/200 | 3  | Starter โ€” search   |
// | Vision Resonator             | Tuner   | 400/400 | 2  | Extender โ€” SS      |
// | Crimson Resonator            | Tuner   | 800/300 | 2  | Starter โ€” search   |
// | Synkron Resonator            | Tuner   | 100/100 | 1  | Extender โ€” SS from GY |
// | Darkness Resonator           | Tuner   | 1300/300| 3  | Extender           |
// | Bone Archfiend               | Non-Tun | 1800/0  | 4  | Bridge โ€” SS via banish |
// | Red Lotus King, Flame Crime  | Non-Tun | 1700/300| 3  | Extender โ€” revive   |
// | Obsessive Uvualoop            | Non-Tun | 1200/1800|4 | Bridge โ€” send to GY |
// | Fiend Piece Golem            | Non-Tun | 2100/0  | 5  | Extender โ€” draw     |
// | Power Vice Dragon            | Non-Tun | 2000/2400|5 | Beater              |
// | Chaos Dragon Levianeer       | Non-Tun | 3000/0  | 8  | Board breaker       |
// | Skull Meister                | Hand Trap|1700/400| 4 | GY negation         |
// | Mulcharmy Fuwalos            | Hand Trap| 100/600| 4 | Hand trap โ€” draw    |
// | Resonator Call               | Spell   | -       | -  | Search Resonator    |
// | Crimson Gaia                 | Field   | -       | -  | Search + buff       |
// | Void Apocalypse              | Trap    | -       | -  | Pop + search        |
// | Crimson Call                 | QuickSp | -       | -  | SS from GY          |
// | Cosmic Cyclone               | QuickSp | -       | -  | Banish backrow      |
// | Book of Lunar Eclipse        | QuickSp | -       | -  | Flip face-down      |
// | Red Zone                     | Contrap | -       | -  | Negate attack/effect|
// | Red Reign                    | Trap    | -       | -  | Burn + negate       |
// | The Ruler's Rumbling         | Trap    | -       | -  | Banish all monsters |
// | Scarlet Security             | Trap    | -       | -  | Protect from destruction|
// |-----------------------------|---------|---------|----|---------------------|
// EXTRA DECK:
// | Red Supernova Dragon         | Synchro | 4000/3000|12| ACE โ€” omni-negate   |
// | Red Nova Dragon              | Synchro | 3500/3000|12| Board-wipe on SS    |
// | Hot Red Dragon Archfiend Abyss|Synchro | 3200/2500|9| Quick negate        |
// | Bystial Dis Pater            | Synchro | 3500/3500|10| Revive + negate     |
// | The Crimson King             | Synchro | 3000/2500|8| Extender + negate   |
// | Scarred Dragon Archfiend     | Synchro | 3000/2500|8| Recursion engine    |
// | Red Dragon Archfiend         | Synchro | 3000/2000|8| Classic boss        |
// | Crimson Blade Dragon         | Synchro | 2400/2600|7| Extender โ€” reborn   |
// | Kuibelt the Blade Dragon     | Synchro | 2500/1900|7| Quick destroy       |
// | Zalen the Shackled Dragon    | Synchro | 2800/2100|7| Multi-attack        |
// | Red Rising Dragon            | Synchro | 2100/1600|6| ACE โ€” revive bridge  |
// | Red Hypernova Dragon         | Synchro | 4500/3000|12| Mass banish         |
// | Storm-Bane Dragon Destorbim  | Synchro | 3000/2800|11| Banish field        |
// ============================================================
// ACE CARDS:
//   Primary: Red Supernova Dragon (4000 omni-negate)
//   Secondary: Red Rising Dragon (600 bridge), The Crimson King (800 extender)
// COMBO STARTERS:
//   1. Resonator Call โ’ Soul Resonator โ’ search Bone Archfiend
//   2. Crimson Resonator โ’ search Resonator from deck
//   3. Crimson Gaia โ’ search Resonator + buff
// WIN CONDITION: Red Supernova Dragon + Dis Pater + backrow = 2-3 disruptions
// GOING 1ST END BOARD: Red Supernova + Red Zone/Crimson Call set = strong defense
// GOING 2ND GAMEPLAN: Break board via Dis Pater banish + Red Nova nuke
// CHOKEPOINTS: Crimson Resonator normal โ’ Ash = combo stopped
// ============================================================
// COMBO DRAFT โ€” 2026 Red Dragon / Resonator
// ============================================================
// === COMBO LINE 1: Standard 1-Card Soul Resonator ===
// HAND: Soul Resonator
// STEP 1: NS Soul Resonator โ’ search Bone Archfiend
// STEP 2: SS Bone Archfiend by banishing Soul Resonator
// STEP 3: Synchro: Bone Archfiend(L4) + Soul Resonator(GY, via... actually banish for cost)
// Actually: NS Soul Resonator โ’ search Bone Archfiend
// Activate Crimson Gaia โ’ search another Resonator
// SS Bone Archfiend (banish Resonator from field/GY)
// Synchro: Bone Archfiend + Tuner = Red Rising Dragon (L6)
// Red Rising Dragon: SS Tuner from GY
// Synchro: Red Rising Dragon + Tuner = The Crimson King (L8)
// The Crimson King: SS Tuner from GY
// Synchro: The Crimson King + 2 Tuners = Red Supernova Dragon (L12)
// END BOARD: Red Supernova Dragon + Red Zone/Crimson Call
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
    [Deck("2026_RedDragon", "2026_RedDragon")]
    public class _2026_RedDragonExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck โ€” Resonator Tuners
            public const int SoulResonator = 62991792;
            public const int VisionResonator = 98396890;
            public const int CrimsonResonator = 34761841;
            public const int SynkronResonator = 77360173;
            public const int DarknessResonator = 83445539;

            // Main Deck โ€” Non-Tuners
            public const int BoneArchfiend = 25784595;
            public const int RedLotusKingFlameCrime = 19299793;
            public const int ObsessiveUvualoop = 98806751;
            public const int FiendPieceGolem = 56838842;
            public const int ChaosDragonLevianeer = 55878038;
            public const int PowerViceDragon = 19434243;

            // Hand Traps
            public const int SkullMeister = 67750322;
            public const int MulcharmyFuwalos = 42141493;

            // Spells
            public const int ResonatorCall = 23008320;
            public const int CrimsonGaia = 98173209;
            public const int VoidApocalypse = 7337976;
            public const int CrimsonCall = 99398682;
            public const int CosmicCyclone = 8267140;
            public const int BookOfLunarEclipse = 31834488;

            // Traps
            public const int RedZone = 50056656;
            public const int RedReign = 5376159;
            public const int TheRulersRumbling = 17269895;
            public const int ScarletSecurity = 50215517;

            // Extra Deck โ€” Synchro Boss Monsters
            public const int RedSupernovaDragon = 99585850;
            public const int RedNovaDragon = 97489701;
            public const int HotRedAbyss = 9753964;
            public const int BystialDisPater = 27572350;
            public const int TheCrimsonKing = 67809530;
            public const int ScarredDragonArchfiend = 87451661;
            public const int RedDragonArchfiend = 70902743;
            public const int CrimsonBladeDragon = 3294539;
            public const int KuibeltBladeDragon = 87837090;
            public const int ZalenShackledDragon = 4891376;
            public const int RedRisingDragon = 66141736;
            public const int RedHypernovaDragon = 30698243;
            public const int StormBaneDragonDestorbim = 94641726;

            // Side Deck
            public const int DDCrow = 24508238;
            public const int ThunderKingKaiju = 48770333;
            public const int JizukiruKaiju = 63941210;
            public const int TwinTwisters = 43898403;
            public const int InterruptedKaijuSlumber = 99330325;
            public const int GravityCollapse = 7811875;
        }

        // โ”€โ”€ Ace/Boss monsters โ”€โ”€
        private static readonly int[] AceCardIds = {
            CardId.RedSupernovaDragon,
            CardId.RedNovaDragon,
            CardId.HotRedAbyss,
            CardId.BystialDisPater,
            CardId.TheCrimsonKing,
            CardId.RedRisingDragon,
            CardId.RedHypernovaDragon,
            CardId.StormBaneDragonDestorbim,
        };

        // โ”€โ”€ All Resonator Tuners โ”€โ”€
        private static readonly int[] ResonatorCards = {
            CardId.SoulResonator,
            CardId.VisionResonator,
            CardId.CrimsonResonator,
            CardId.SynkronResonator,
            CardId.DarknessResonator,
        };

        // โ”€โ”€ Hand traps for protection โ”€โ”€
        private static readonly int[] HandTraps = {
            CardId.SkullMeister,
            CardId.MulcharmyFuwalos,
        };

        // โ”€โ”€ OPT Flags โ”€โ”€
        private bool _boneArchfiendUsed = false;
        private bool _crimsonGaiaUsed = false;
        private bool _voidApocalypseUsed = false;
        private bool _crimsonCallUsed = false;
        private bool _redZoneUsed = false;
        private bool _theCrimsonKingOnSummonUsed = false;
        private bool _scarredDragonUsed = false;
        private bool _redLotusUsed = false;
        private bool _redReignUsed = false;
        private bool _rumblingUsed = false;
        private bool _resonatorCallUsed = false;
        private bool _crimsonResonatorSearchUsed = false;
        private bool _soulResonatorSearchUsed = false;
        private bool _levianeerUsed = false;

        public _2026_RedDragonExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Register Ace Cards to protect them from being used suboptimally
            ResourcePlan.RegisterAceCards(AceCardIds);

            // โ”€โ”€ Combo Router โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Resonator-Call-Soul",
                RequiredCards = new List<int> { CardId.ResonatorCall, CardId.BoneArchfiend },
                FallbackLineName = "Crimson-Gaia-Fallback",
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.ResonatorCall, ActionType = ExecutorType.Activate, Description = "Search Soul Resonator" },
                    new() { CardId = CardId.BoneArchfiend, ActionType = ExecutorType.Activate, Description = "SS Bone Archfiend" },
                },
                EndBoardScore = 80
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Crimson-Gaia-Fallback",
                RequiredCards = new List<int> { CardId.CrimsonGaia },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.CrimsonGaia, ActionType = ExecutorType.Activate, Description = "Search Resonator" },
                },
                EndBoardScore = 70,
                Condition = () => Bot.Hand.Any(c => c != null && c.Id == CardId.CrimsonGaia)
            });

            BaitPlanner.RegisterComboStarters(CardId.ResonatorCall, CardId.SoulResonator, CardId.CrimsonResonator);
            BaitPlanner.RegisterBaitCards(CardId.BookOfLunarEclipse, CardId.CosmicCyclone);
            ChainAdvisor.RegisterHighValueTargets(CardId.ResonatorCall, CardId.SoulResonator, CardId.CrimsonResonator);

            // TIER 1: Hand Traps
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyActivate);
            AddExecutor(ExecutorType.Activate, CardId.SkullMeister, SkullMeisterActivate);

            // TIER 2: Boss Quick Effects
            AddExecutor(ExecutorType.Activate, CardId.RedSupernovaDragon, RedSupernovaActivate);
            AddExecutor(ExecutorType.Activate, CardId.HotRedAbyss, HotRedAbyssActivate);
            AddExecutor(ExecutorType.Activate, CardId.BystialDisPater, DisPaterActivate);
            AddExecutor(ExecutorType.Activate, CardId.TheCrimsonKing, TheCrimsonKingActivate);
            AddExecutor(ExecutorType.Activate, CardId.KuibeltBladeDragon, KuibeltActivate);
            AddExecutor(ExecutorType.Activate, CardId.StormBaneDragonDestorbim, StormBaneActivate);
            AddExecutor(ExecutorType.Activate, CardId.ScarredDragonArchfiend, ScarredDragonActivate);
            AddExecutor(ExecutorType.Activate, CardId.RedNovaDragon, RedNovaActivate);
            AddExecutor(ExecutorType.Activate, CardId.RedHypernovaDragon, RedHypernovaActivate);
            AddExecutor(ExecutorType.Activate, CardId.RedDragonArchfiend, RedDragonArchfiendActivate);

            // TIER 3: Spells โ€” Setup & Disruption
            AddExecutor(ExecutorType.Activate, CardId.ResonatorCall, ResonatorCallActivate);
            AddExecutor(ExecutorType.Activate, CardId.CrimsonGaia, CrimsonGaiaActivate);
            AddExecutor(ExecutorType.Activate, CardId.CosmicCyclone, CosmicCycloneActivate);
            AddExecutor(ExecutorType.Activate, CardId.BookOfLunarEclipse, BookOfLunarEclipseActivate);
            AddExecutor(ExecutorType.Activate, CardId.CrimsonCall, CrimsonCallActivate);
            AddExecutor(ExecutorType.Activate, CardId.VoidApocalypse, VoidApocalypseActivate);

            // TIER 4: Monster Effects โ€” Field
            AddExecutor(ExecutorType.Activate, CardId.SoulResonator, SoulResonatorSearch);
            AddExecutor(ExecutorType.Activate, CardId.CrimsonResonator, CrimsonResonatorSearch);
            AddExecutor(ExecutorType.Activate, CardId.VisionResonator, VisionResonatorSS);
            AddExecutor(ExecutorType.Activate, CardId.SynkronResonator, SynkronResonatorSS);
            AddExecutor(ExecutorType.Activate, CardId.BoneArchfiend, BoneArchfiendActivate);
            AddExecutor(ExecutorType.Activate, CardId.RedLotusKingFlameCrime, RedLotusActivate);
            AddExecutor(ExecutorType.Activate, CardId.ObsessiveUvualoop, ObsessiveUvualoopActivate);
            AddExecutor(ExecutorType.Activate, CardId.ChaosDragonLevianeer, ChaosLevianeerActivate);
            AddExecutor(ExecutorType.Activate, CardId.FiendPieceGolem, FiendPieceGolemActivate);
            AddExecutor(ExecutorType.Activate, CardId.RedRisingDragon, RedRisingDragonEffect);

            // TIER 5: Traps
            AddExecutor(ExecutorType.Activate, CardId.RedZone, RedZoneActivate);
            AddExecutor(ExecutorType.Activate, CardId.RedReign, RedReignActivate);
            AddExecutor(ExecutorType.Activate, CardId.TheRulersRumbling, RumblingActivate);
            AddExecutor(ExecutorType.SpellSet, CardId.RedZone, DefensiveSetCheck);
            AddExecutor(ExecutorType.SpellSet, CardId.RedReign, DefensiveSetCheck);
            AddExecutor(ExecutorType.SpellSet, CardId.TheRulersRumbling, DefensiveSetCheck);
            AddExecutor(ExecutorType.SpellSet, CardId.ScarletSecurity, DefensiveSetCheck);

            // TIER 6: Normal & Special Summons
            AddExecutor(ExecutorType.Summon, CardId.SoulResonator, SimpleSummon);
            AddExecutor(ExecutorType.Summon, CardId.CrimsonResonator, SimpleSummon);
            AddExecutor(ExecutorType.Summon, CardId.DarknessResonator, SimpleSummon);
            AddExecutor(ExecutorType.Summon, CardId.BoneArchfiend, SimpleSummon);
            AddExecutor(ExecutorType.Summon, CardId.ObsessiveUvualoop, SimpleSummon);
            AddExecutor(ExecutorType.Summon, CardId.RedLotusKingFlameCrime, SimpleSummon);
            AddExecutor(ExecutorType.Summon, CardId.FiendPieceGolem, SimpleSummon);
            AddExecutor(ExecutorType.Summon, CardId.PowerViceDragon, SimpleSummon);

            // TIER 7: Extra Deck โ€” Synchro Summons (ordered by priority)
            AddExecutor(ExecutorType.SpSummon, CardId.RedSupernovaDragon, RedSupernovaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.RedHypernovaDragon, RedHypernovaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.RedNovaDragon, RedNovaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.BystialDisPater, DisPaterSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.TheCrimsonKing, TheCrimsonKingSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.HotRedAbyss, HotRedAbyssSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ScarredDragonArchfiend, ScarredDragonSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.RedDragonArchfiend, SimpleSynchroSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.StormBaneDragonDestorbim, StormBaneSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CrimsonBladeDragon, CrimsonBladeSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.KuibeltBladeDragon, SimpleSynchroSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ZalenShackledDragon, SimpleSynchroSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.RedRisingDragon, RedRisingSummon);

            // TIER 8: Sets & Repos
            AddExecutor(ExecutorType.SpellSet, CardId.CosmicCyclone, DefensiveSetCheck);
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand() => true; // Go first โ€” Synchro setup

        private bool OpponentHasActiveNegator()
        {
            return Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && !c.IsDisabled() &&
                (c.IsFloodgate() || c.Attack >= 2500 || c.HasType(CardType.Link) || c.HasType(CardType.Synchro)));
        }

        public override bool ShouldAllowSpSummon(ClientCard card)
        {
            if (!base.ShouldAllowSpSummon(card)) return false;
            if (card == null) return true;

            // Extra Deck summons (excluding the Ace card itself on its initial summon)
            // If the summon would consume any face-up Ace card currently on the field as material, evaluate it.
            var activeAces = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && IsAceCard(c)).ToList();
            if (activeAces.Count > 0)
            {
                // Synchro/Link summons that require materials from field
                int reqCount = card.HasType(CardType.Link) ? card.LinkCount : 2;
                var nonAceMats = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c)).ToList();
                if (nonAceMats.Count < reqCount)
                {
                    bool allowed = false;
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
                            break;
                        }
                    }
                    if (!allowed)
                    {
                        DecisionTracer.TraceSkip("ShouldAllowSpSummon", $"Summoning {card.Name} is not safe (would consume Ace card(s) as material)");
                        return false;
                    }
                }
            }

            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            ResetOPTFlags();
            
            if (ShouldGoBreakBoard)
            {
                // Going second Red Dragon: prioritize board-breaking (Levianeer banish, Rumbling nuke)
                _levianeerUsed = false;
                _rumblingUsed = false;
            }
        }

        private void ResetOPTFlags()
        {
            _boneArchfiendUsed = false;
            _crimsonGaiaUsed = false;
            _voidApocalypseUsed = false;
            _crimsonCallUsed = false;
            _redZoneUsed = false;
            _theCrimsonKingOnSummonUsed = false;
            _scarredDragonUsed = false;
            _redLotusUsed = false;
            _redReignUsed = false;
            _rumblingUsed = false;
            _resonatorCallUsed = false;
            _crimsonResonatorSearchUsed = false;
            _soulResonatorSearchUsed = false;
            _levianeerUsed = false;
        }

        public override void OnChaining(int player, ClientCard card)
        {
            base.OnChaining(player, card);
            if (player == 0 && card != null)
            {
                if (card.Id == CardId.SoulResonator) _soulResonatorSearchUsed = true;
                if (card.Id == CardId.CrimsonResonator) _crimsonResonatorSearchUsed = true;
                if (card.Id == CardId.BoneArchfiend) _boneArchfiendUsed = true;
                if (card.Id == CardId.ResonatorCall) _resonatorCallUsed = true;
                if (card.Id == CardId.CrimsonGaia) _crimsonGaiaUsed = true;
            }
        }

        // โ•โ•โ• Ace & Priority โ•โ•โ•

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Contains(card.Id);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.IsCode(HandTraps)) return 800;
            // Protect Resonators โ€” they enable synchro plays
            if (c.IsCode(ResonatorCards)) return 500;
            // Bone Archfiend is key bridge โ€” protect last copy
            if (c.IsCode(CardId.BoneArchfiend) && GetRemainingCount(CardId.BoneArchfiend) <= 1) return 400;
            return 100;
        }

        protected override bool IsBoardStrongEnough()
        {
            if (Bot.HasInMonstersZone(CardId.RedSupernovaDragon))
                return true;
            if (Bot.HasInMonstersZone(CardId.RedNovaDragon) && Bot.HasInMonstersZone(CardId.HotRedAbyss))
                return true;
            if (Bot.HasInMonstersZone(CardId.BystialDisPater) && Bot.HasInMonstersZone(CardId.TheCrimsonKing))
                return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (IsBoardStrongEnough())
                return base.ShouldStopExtending();
            return false;
        }

        // โ•โ•โ• TIER 1: Hand Traps โ•โ•โ•

        private bool MulcharmyActivate()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }

        private bool SkullMeisterActivate()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;
            if (LastChainCard == null) return false;
            // Skull Meister negates GY effects โ€” chain when opponent activates in GY
            return LastChainCard.Location == CardLocation.Grave;
        }

        // โ•โ•โ• TIER 2: Boss Quick Effects โ•โ•โ•

        private bool RedSupernovaActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (Duel.LastChainPlayer != 1) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            // Omni-negate โ€” can negate any activation
            return true;
        }

        private bool HotRedAbyssActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (Duel.LastChainPlayer != 1) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (LastChainCard != null && LastChainCard.IsSpell())
            {
                AI.SelectCard(LastChainCard);
                return true;
            }
            return false;
        }

        private bool DisPaterActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (Duel.LastChainPlayer != 1) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (LastChainCard != null && LastChainCard.IsMonster())
            {
                // Negate + banish monster effect
                AI.SelectCard(LastChainCard);
                return true;
            }
            return false;
        }

        private bool TheCrimsonKingActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            // On-summon: SS 1 Resonator from GY
            if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
            {
                if (_theCrimsonKingOnSummonUsed) return false;
                bool hasTarget = Bot.Graveyard.Any(c => c != null && c.IsCode(ResonatorCards) && c.IsCanRevive());
                if (hasTarget)
                {
                    _theCrimsonKingOnSummonUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool KuibeltActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (Duel.Player != 0) return false;
            // Quick effect: destroy 1 face-up card on field
            var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool StormBaneActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            // On summon: banish 1 card from opponent's field
            var target = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                .FirstOrDefault(c => c != null && c.IsFaceup());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool ScarredDragonActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (_scarredDragonUsed) return false;
            if (Duel.Phase != DuelPhase.Standby) return false;
            // Standby Phase: SS 1 Dragon Fiend from GY
            bool hasTarget = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.HasRace(CardRace.Dragon) && c.HasRace(CardRace.Fiend));
            if (!hasTarget) return false;
            _scarredDragonUsed = true;
            return true;
        }

        private bool RedNovaActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            // On summon: destroy all opponent cards
            if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
            {
                return Enemy.GetMonsterCount() + Enemy.GetSpellCount() > 0;
            }
            return false;
        }

        private bool RedHypernovaActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (Duel.LastChainPlayer != 1) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            // Quick effect: banish all monsters opponent controls and in their GY
            if (Enemy.GetMonsterCount() > 0 || Enemy.Graveyard.Any(c => c != null && c.IsMonster()))
                return true;
            return false;
        }

        private bool RedDragonArchfiendActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            // End Phase: destroy all monsters in Attack Position that you control
            if (Duel.Phase == DuelPhase.End)
            {
                var attackMonsters = Bot.GetMonsters().Where(c => c != null && c.IsAttack() && c.Id != Card.Id).ToList();
                return attackMonsters.Count > 0;
            }
            return false;
        }

        // โ•โ•โ• TIER 3: Spells โ•โ•โ•

        private bool ResonatorCallActivate()
        {
            if (_resonatorCallUsed) return false;
            if (ShouldSkipCombo()) return false;
            if (Card.Location != CardLocation.Hand) return false;

            // Search priority: Soul Resonator > Crimson Resonator > Vision Resonator > Synkron
            if (GetRemainingCount(CardId.SoulResonator) > 0 && !Bot.HasInHand(CardId.SoulResonator))
                AI.SelectCard(CardId.SoulResonator);
            else if (GetRemainingCount(CardId.CrimsonResonator) > 0 && !Bot.HasInHand(CardId.CrimsonResonator))
                AI.SelectCard(CardId.CrimsonResonator);
            else if (GetRemainingCount(CardId.VisionResonator) > 0 && !Bot.HasInHand(CardId.VisionResonator))
                AI.SelectCard(CardId.VisionResonator);
            else if (GetRemainingCount(CardId.SynkronResonator) > 0 && !Bot.HasInHand(CardId.SynkronResonator))
                AI.SelectCard(CardId.SynkronResonator);
            else
                return false;

            _resonatorCallUsed = true;
            return true;
        }

        private bool CrimsonGaiaActivate()
        {
            if (_crimsonGaiaUsed) return false;
            if (ShouldSkipCombo()) return false;

            if (Card.Location == CardLocation.Hand)
            {
                // Search priority: if we don't have Soul/Crimson Resonator, search one
                if (!Bot.HasInHand(CardId.SoulResonator) && GetRemainingCount(CardId.SoulResonator) > 0)
                    AI.SelectCard(CardId.SoulResonator);
                else if (GetRemainingCount(CardId.CrimsonResonator) > 0 && !Bot.HasInHand(CardId.CrimsonResonator))
                    AI.SelectCard(CardId.CrimsonResonator);
                else if (!Bot.HasInHand(CardId.VisionResonator) && GetRemainingCount(CardId.VisionResonator) > 0)
                    AI.SelectCard(CardId.VisionResonator);
                else if (!Bot.HasInHand(CardId.SynkronResonator) && GetRemainingCount(CardId.SynkronResonator) > 0)
                    AI.SelectCard(CardId.SynkronResonator);
                else
                    return false; // Already have all Resonators

                _crimsonGaiaUsed = true;
                return true;
            }
            return true; // Already on field โ€” ATK boost passive
        }

        private bool CosmicCycloneActivate()
        {
            if (ShouldSkipCombo()) return false;
            // Target enemy backrow
            var target = Util.GetBestEnemySpell();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            // Fallback: pop our own Crimson Gaia if opponent has no targets (to search)
            var ownGaia = Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.CrimsonGaia));
            if (ownGaia != null && GetRemainingCount(CardId.CrimsonGaia) > 0)
            {
                AI.SelectCard(ownGaia);
                return true;
            }
            return false;
        }

        private bool BookOfLunarEclipseActivate()
        {
            if (ShouldSkipCombo()) return false;
            if (Duel.LastChainPlayer != 1) return false;
            if (LastChainCard == null || LastChainCard.Controller == 0) return false;
            // Flip 1 face-up monster face-down
            var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool CrimsonCallActivate()
        {
            if (_crimsonCallUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (Card.Location == CardLocation.Hand)
            {
                // SS 1 Resonator or Archfiend from GY
                bool hasTarget = Bot.Graveyard.Any(c => c != null && c.IsMonster() &&
                    (c.IsCode(ResonatorCards) || c.IsCode(CardId.BoneArchfiend)));
                if (hasTarget)
                {
                    _crimsonCallUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool VoidApocalypseActivate()
        {
            if (_voidApocalypseUsed) return false;
            if (Card.Location == CardLocation.SpellZone && Card.IsFacedown())
            {
                // Pop 1 monster we control, search Field Spell
                var popTarget = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() &&
                    (c.IsCode(ResonatorCards) || c.IsCode(CardId.ObsessiveUvualoop)));
                if (popTarget != null && GetRemainingCount(CardId.CrimsonGaia) > 0)
                {
                    AI.SelectCard(popTarget);
                    AI.SelectCard(CardId.CrimsonGaia);
                    _voidApocalypseUsed = true;
                    return true;
                }
            }
            return false;
        }

        // โ•โ•โ• TIER 4: Monster Effects โ•โ•โ•

        private bool SoulResonatorSearch()
        {
            if (_soulResonatorSearchUsed) return false;
            if (ShouldSkipCombo()) return false;
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;

            // Search Bone Archfiend first for combo extension
            if (GetRemainingCount(CardId.BoneArchfiend) > 0 && !Bot.HasInHand(CardId.BoneArchfiend))
            {
                AI.SelectCard(CardId.BoneArchfiend);
            }
            else if (GetRemainingCount(CardId.RedLotusKingFlameCrime) > 0 && !Bot.HasInHand(CardId.RedLotusKingFlameCrime))
            {
                AI.SelectCard(CardId.RedLotusKingFlameCrime);
            }
            else if (GetRemainingCount(CardId.ObsessiveUvualoop) > 0 && !Bot.HasInHand(CardId.ObsessiveUvualoop))
            {
                AI.SelectCard(CardId.ObsessiveUvualoop);
            }
            else if (GetRemainingCount(CardId.PowerViceDragon) > 0 && !Bot.HasInHand(CardId.PowerViceDragon))
            {
                AI.SelectCard(CardId.PowerViceDragon);
            }
            else
                return false;

            _soulResonatorSearchUsed = true;
            return true;
        }

        private bool CrimsonResonatorSearch()
        {
            if (_crimsonResonatorSearchUsed) return false;
            if (ShouldSkipCombo()) return false;
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;

            // Search another Resonator from deck
            if (!Bot.HasInHand(CardId.SoulResonator) && GetRemainingCount(CardId.SoulResonator) > 0)
                AI.SelectCard(CardId.SoulResonator);
            else if (!Bot.HasInHand(CardId.VisionResonator) && GetRemainingCount(CardId.VisionResonator) > 0)
                AI.SelectCard(CardId.VisionResonator);
            else if (!Bot.HasInHand(CardId.SynkronResonator) && GetRemainingCount(CardId.SynkronResonator) > 0)
                AI.SelectCard(CardId.SynkronResonator);
            else
                return false;

            _crimsonResonatorSearchUsed = true;
            return true;
        }

        private bool VisionResonatorSS()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            // Can SS itself if we control a L5+ Dragon/Fiend
            bool hasLevel5Plus = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Level >= 5 &&
                (c.HasRace(CardRace.Dragon) || c.HasRace(CardRace.Fiend)));
            return hasLevel5Plus;
        }

        private bool SynkronResonatorSS()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            // Can SS from GY if a Synchro is on field
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Synchro));
        }

        private bool BoneArchfiendActivate()
        {
            if (ShouldSkipCombo()) return false;

            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave)
            {
                if (_boneArchfiendUsed) return false;
                if (IsSpecialSummonBlocked()) return false;

                // Can SS by sending 1 other card from hand or field to GY
                bool hasSendTarget = Bot.Hand.Any(c => c != null && c != Card)
                    || Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id != CardId.BoneArchfiend);

                if (!hasSendTarget) return false;

                _boneArchfiendUsed = true;
                return true;
            }
            else if (Card.Location == CardLocation.MonsterZone && !Card.IsDisabled())
            {
                // Field effect: manipulate level by sending a Fiend Tuner from deck
                bool hasTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsTuner() && c.HasRace(CardRace.Fiend));
                bool hasTargetInDeck = GetRemainingCount(CardId.CrimsonResonator) > 0 || GetRemainingCount(CardId.SynkronResonator) > 0 || GetRemainingCount(CardId.VisionResonator) > 0;
                
                if (hasTuner && hasTargetInDeck)
                {
                    var fieldTuner = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsTuner() && c.HasRace(CardRace.Fiend));
                    if (fieldTuner != null)
                    {
                        AI.SelectCard(fieldTuner);
                        
                        if (GetRemainingCount(CardId.CrimsonResonator) > 0)
                            AI.SelectNextCard(CardId.CrimsonResonator);
                        else if (GetRemainingCount(CardId.VisionResonator) > 0)
                            AI.SelectNextCard(CardId.VisionResonator);
                        else
                            AI.SelectNextCard(CardId.SynkronResonator);
                        
                        AI.SelectOption(1); // Usually option 1 is decrease level
                        return true;
                    }
                }
            }
            return false;
        }

        private bool RedLotusActivate()
        {
            if (_redLotusUsed) return false;
            
            if (Card.Location == CardLocation.Hand)
            {
                if (IsSpecialSummonBlocked()) return false;
                // SS from hand if we control a Fiend Tuner
                bool hasFiendTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsTuner() && c.HasRace(CardRace.Fiend));
                if (hasFiendTuner)
                {
                    _redLotusUsed = true;
                    return true;
                }
            }
            else if (Card.Location == CardLocation.MonsterZone && !Card.IsDisabled())
            {
                if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
                
                // Send 1 Normal Trap from Deck/Field to GY
                bool hasTrapInDeck = GetRemainingCount(CardId.RedZone) > 0 || GetRemainingCount(CardId.RedReign) > 0 || GetRemainingCount(CardId.TheRulersRumbling) > 0;
                if (hasTrapInDeck)
                {
                    AI.SelectCard(CardId.RedReign, CardId.RedZone, CardId.TheRulersRumbling);
                    _redLotusUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool ObsessiveUvualoopActivate()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave)
            {
                if (Card.Location == CardLocation.Hand && IsSpecialSummonBlocked()) return false;
                
                // Banish 1 Synchro from GY to SS from hand or add to hand from GY
                bool hasSynchroInGY = Bot.Graveyard.Any(c => c != null && c.HasType(CardType.Synchro));
                if (hasSynchroInGY)
                {
                    // Select lowest level/priority synchro in GY
                    var target = Bot.Graveyard.Where(c => c != null && c.HasType(CardType.Synchro)).OrderByDescending(c => GetMaterialPriority(c)).FirstOrDefault();
                    if (target != null)
                    {
                        AI.SelectCard(target);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ChaosLevianeerActivate()
        {
            if (_levianeerUsed) return false;
            if (Card.Location != CardLocation.Hand) return false;
            if (IsSpecialSummonBlocked()) return false;
            // SS by banishing 1 LIGHT + 1 DARK from GY
            bool hasLight = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.HasAttribute(CardAttribute.Light));
            bool hasDark = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.HasAttribute(CardAttribute.Dark));
            if (!hasLight || !hasDark) return false;

            _levianeerUsed = true;
            return true;
        }

        private bool FiendPieceGolemActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (Duel.Phase != DuelPhase.End) return false;
            // End Phase: search 1 Resonator if sent to GY this turn
            return true;
        }

        private bool RedRisingDragonEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            // On summon: SS 1 Resonator from GY
            bool hasTarget = Bot.Graveyard.Any(c => c != null && c.IsCode(ResonatorCards) && c.IsCanRevive());
            return hasTarget;
        }

        // โ•โ•โ• TIER 5: Traps โ•โ•โ•

        private bool RedZoneActivate()
        {
            if (_redZoneUsed) return false;
            if (Card.Location != CardLocation.SpellZone || Card.IsFacedown()) return false;
            if (Duel.LastChainPlayer != 1) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            // Negate attack or effect targeting our monster
            if (Duel.Phase == DuelPhase.Battle && LastChainCard != null && LastChainCard.IsMonster())
            {
                _redZoneUsed = true;
                return true;
            }
            return false;
        }

        private bool RedReignActivate()
        {
            if (_redReignUsed) return false;
            if (Card.Location != CardLocation.SpellZone || Card.IsFacedown()) return false;
            if (Duel.LastChainPlayer != 1) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            // Negate + burn โ€” chain to opponent's monster effect
            if (LastChainCard != null && LastChainCard.IsMonster())
            {
                _redReignUsed = true;
                return true;
            }
            return false;
        }

        private bool RumblingActivate()
        {
            if (_rumblingUsed) return false;
            if (Card.Location != CardLocation.SpellZone || Card.IsFacedown()) return false;
            if (Duel.LastChainPlayer != 1) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            // Banish all monsters on field โ€” use as last resort
            if (Enemy.GetMonsterCount() >= 3 && Bot.GetMonsterCount() <= Enemy.GetMonsterCount())
            {
                _rumblingUsed = true;
                return true;
            }
            return false;
        }

        // โ•โ•โ• Normal Summon Conditions โ•โ•โ•

        private bool SimpleSummon()
        {
            if (ShouldSkipCombo()) return false;
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        // โ•โ•โ• Extra Deck โ€” Synchro Summon Conditions โ•โ•โ•

        private bool RedSupernovaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            // LV12: 3 Tuners + 1+ non-Tuner Synchro โ€” check field
            int tunerCount = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.IsTuner());
            int synchroCount = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.HasType(CardType.Synchro));
            if (tunerCount >= 3 && synchroCount >= 1)
            {
                // Check we have enough total levels for LV12
                int totalLevel = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.HasType(CardType.Xyz) && !c.HasType(CardType.Link))
                    .Sum(c => c.Level);
                if (totalLevel >= 12)
                    return true;
            }
            return false;
        }

        private bool RedHypernovaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            // LV12: 3 Tuners + 1+ non-Tuner Synchro
            int tunerCount = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.IsTuner());
            int synchroCount = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.HasType(CardType.Synchro));
            if (tunerCount >= 3 && synchroCount >= 1)
            {
                int totalLevel = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.HasType(CardType.Xyz) && !c.HasType(CardType.Link))
                    .Sum(c => c.Level);
                if (totalLevel >= 12)
                    return true;
            }
            return false;
        }

        private bool RedNovaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            // LV12: 1 Tuner + 1+ non-Tuner Dragon
            bool hasTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsTuner());
            bool hasDragon = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsTuner() && c.HasRace(CardRace.Dragon));
            if (hasTuner && hasDragon)
            {
                int totalLevel = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.HasType(CardType.Xyz))
                    .Sum(c => c.Level);
                if (totalLevel >= 12)
                    return true;
            }
            return false;
        }

        private bool DisPaterSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            // LV10: 1 Tuner + 1+ non-Tuner 
            bool hasTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsTuner());
            bool hasNonTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsTuner());
            if (hasTuner && hasNonTuner)
            {
                int totalLevel = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.HasType(CardType.Xyz))
                    .Sum(c => c.Level);
                if (totalLevel >= 10)
                    return true;
            }
            return false;
        }

        private bool TheCrimsonKingSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            // LV8: 1 Tuner + 1+ non-Tuner Fiend
            bool hasTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsTuner());
            bool hasFiend = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsTuner() && c.HasRace(CardRace.Fiend));
            if (hasTuner && hasFiend)
            {
                int totalLevel = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.HasType(CardType.Xyz))
                    .Sum(c => c.Level);
                if (totalLevel >= 8)
                    return true;
            }
            return false;
        }

        private bool HotRedAbyssSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            // LV9: 1 Tuner + 1+ non-Tuner Dragon
            bool hasTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsTuner());
            bool hasDragon = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsTuner() && c.HasRace(CardRace.Dragon));
            if (hasTuner && hasDragon)
            {
                int totalLevel = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.HasType(CardType.Xyz))
                    .Sum(c => c.Level);
                if (totalLevel >= 9)
                    return true;
            }
            return false;
        }

        private bool ScarredDragonSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            // LV8: needs 1 Tuner + 1+ non-Tuner Fiend
            bool hasTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsTuner());
            bool hasNonTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsTuner() && c.HasRace(CardRace.Fiend));
            if (hasTuner && hasNonTuner)
            {
                int totalLevel = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.HasType(CardType.Xyz))
                    .Sum(c => c.Level);
                if (totalLevel >= 8)
                    return true;
            }
            return false;
        }

        private bool SimpleSynchroSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            bool hasTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsTuner());
            bool hasNonTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsTuner());
            return hasTuner && hasNonTuner;
        }

        private bool StormBaneSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            // LV11: use when opponent controls cards to banish
            bool hasTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsTuner());
            bool hasNonTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsTuner());
            if (hasTuner && hasNonTuner)
            {
                int totalLevel = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.HasType(CardType.Xyz))
                    .Sum(c => c.Level);
                if (totalLevel >= 11 && Enemy.GetMonsterCount() > 0)
                    return true;
            }
            return false;
        }

        private bool CrimsonBladeSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            // LV7: for extension โ€” can SS itself back from GY
            bool hasTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsTuner());
            bool hasNonTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsTuner());
            if (hasTuner && hasNonTuner)
            {
                int totalLevel = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.HasType(CardType.Xyz))
                    .Sum(c => c.Level);
                if (totalLevel >= 7)
                    return true;
            }
            return false;
        }

        private bool RedRisingSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            // LV6: the key bridge summon โ€” requires 1 Tuner + 1+ non-Tuner
            bool hasTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsTuner());
            bool hasNonTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsTuner());
            if (hasTuner && hasNonTuner)
            {
                int totalLevel = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.HasType(CardType.Xyz))
                    .Sum(c => c.Level);
                if (totalLevel >= 6)
                    return true;
            }
            return false;
        }

        // โ•โ•โ• Utility Methods โ•โ•โ•

        private bool DefensiveSetCheck()
        {
            if (Duel.Player != 0) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
            if (Bot.GetSpellCountWithoutField() >= 5) return false;
            // Set in MP2 primarily
            if (Duel.Phase == DuelPhase.Main2) return true;
            // Only set in MP1 if we have no other actions
            if (Main != null && (Main.ActivableCards.Count > 0 || Main.SummonableCards.Count > 0
                || Main.SpecialSummonableCards.Count > 0))
                return false;
            return true;
        }

        // โ•โ•โ• OnSelectCard โ•โ•โ•

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Fusion/Synchro/Xyz material selection
            if (hint == 511 || hint == 533 || hint == 502 || hint == 504)
            {
                var sorted = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }

            // Search selection โ€” Resonator Call, Crimson Gaia search
            if (hint == 506)
            {
                var preferred = new List<ClientCard>();
                // Bose Archfiend first for combo
                var boneArchfiend = cards.FirstOrDefault(c => c != null && c.Id == CardId.BoneArchfiend);
                if (boneArchfiend != null && !Bot.HasInHand(CardId.BoneArchfiend))
                    preferred.Add(boneArchfiend);
                if (preferred.Count < max)
                {
                    var soulRes = cards.FirstOrDefault(c => c != null && c.Id == CardId.SoulResonator);
                    if (soulRes != null) preferred.Add(soulRes);
                }
                if (preferred.Count >= min)
                    return preferred.Take(max).ToList();
            }

            // Special Summon selection
            if (hint == 509)
            {
                var bossFirst = cards.Where(c => c != null && AceCardIds.Contains(c.Id)).ToList();
                if (bossFirst.Count >= min) return bossFirst.Take(max).ToList();
                var resonators = cards.Where(c => c != null && c.IsCode(ResonatorCards)).ToList();
                if (resonators.Count >= min) return resonators.Take(max).ToList();
            }

            // Bone Archfiend send cost or other hand/field costs
            if (hint == 502 || hint == 504 || hint == 500)
            {
                if (cards.Any(c => c.Location == CardLocation.Hand || c.Location == CardLocation.MonsterZone))
                {
                    // Prefer sending duplicate setup spells or extra tuners from hand
                    var handSpells = cards.Where(c => c != null && c.Location == CardLocation.Hand && (c.IsCode(CardId.CrimsonGaia) || c.IsCode(CardId.ResonatorCall))).ToList();
                    if (handSpells.Count >= min) return handSpells.Take(max).ToList();
                    
                    var uselessMonsters = cards.Where(c => c != null && c.Location == CardLocation.Hand && (c.IsCode(CardId.ObsessiveUvualoop) || c.IsCode(CardId.RedLotusKingFlameCrime) || c.IsCode(CardId.SynkronResonator))).ToList();
                    if (uselessMonsters.Count >= min) return uselessMonsters.Take(max).ToList();

                    var resonators = cards.Where(c => c != null && c.Location == CardLocation.Hand && c.IsCode(ResonatorCards)).ToList();
                    if (resonators.Count >= min) return resonators.Take(max).ToList();
                }
            }

            if (Card == null) return base.OnSelectCard(cards, min, max, hint, cancelable);
            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override IList<ClientCard> OnSelectSynchroMaterial(IList<ClientCard> cards, int sum, int min, int max)
        {
            var sorted = cards.Where(c => c != null)
                .OrderBy(c => GetMaterialPriority(c))
                .ToList();

            var safe = sorted.Where(c => !IsAceCard(c)).ToList();
            if (safe.Count >= min)
                return Util.CheckSelectCount(safe, cards, min, max);

            // If forced to use an Ace card, at least use the lowest priority ones
            return Util.CheckSelectCount(sorted, cards, min, max);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            YGOSharp.OCGWrapper.NamedCard cardData = YGOSharp.OCGWrapper.NamedCard.Get(cardId);
            if (cardData != null)
            {
                if (cardData.Attack < 1500 && positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence;
                if (cardData.Attack >= 1500 && positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }
            
            if (AceCardIds.Contains(cardId) && positions.Contains(CardPosition.FaceUpAttack))
                return CardPosition.FaceUpAttack;
            if (cardId == CardId.SoulResonator && positions.Contains(CardPosition.FaceUpDefence))
                return CardPosition.FaceUpDefence;
            return base.OnSelectPosition(cardId, positions);
        }

        // โ•โ•โ• Repos โ•โ•โ•

        private bool MonsterRepos()
        {
            if (Card == null) return false;
            if (IsAceCard(Card))
            {
                if (Card.IsDefense()) return true;
                return false;
            }
            
            bool enemyEmpty = Enemy.GetMonsterCount() == 0;
            int enemyHighestATK = 0;
            if (!enemyEmpty)
            {
                var faceUpEnemies = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup());
                if (faceUpEnemies.Any())
                    enemyHighestATK = faceUpEnemies.Max(c => c.Attack);
            }

            if (Card.IsAttack())
            {
                if (!enemyEmpty && Card.Attack <= enemyHighestATK && Card.Attack < 2500)
                    return true;
                    
                if (!enemyEmpty && Card.Attack <= 1000 && Card.Defense > 0)
                    return true;
            }
            else
            {
                if (enemyEmpty || Card.Attack > enemyHighestATK || Card.Attack >= 2500)
                    return true;
            }
            return false;
        }
    }
}
