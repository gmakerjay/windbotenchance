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
    // CARD AUDIT — 2026_DarkWorld (Refined ModernExecutor v4)
    // ============================================================
    // ACE CARDS:
    //   - Primary: Grapha, Dragon Overlord of Dark World (Fusion 3200 ATK Omni-Disruption)
    //   - Secondary: D/D/D Wave High King Caesar (Rank 6 2800 ATK Double SS Negate)
    //   - Secondary: Snake-Eyes Vengeance Dragon (Synchro Lv 12 3000 ATK S/T zone lock & pop)
    //   - Secondary: Clorless, Chaos King of Dark World (Fusion 4000+ ATK Board Wipe OTK)
    //   - Secondary: Swordsoul Supreme Sovereign - Chengying / Luce the Dusk's Dark
    //   - Disruption: Groza, Tyrant of Thunder / Fabled Valkyrus / Dingirsu / Steelswarm Roach
    // COMBO STARTERS:
    //   - Dark Corridor, Genta Gateman, The Gates of Dark World, Fiendsmith's Tract, Dealings, Buio
    // CORE MECHANIC:
    //   - Discard-by-effect triggers, Ceruli opponent-forced discard triggers,
    //     Grapha/Overking GY bounce-summons, Accession/Mutiny Fusions, Gates draw engine.
    // ============================================================

    [Deck("2026_DarkWorld", "2026_DarkWorld")]
    public class _2026_DarkWorldExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int ReignBeauxOverkingOfDarkWorld = 41406613;
            public const int GraphaDragonLordOfDarkWorld = 34230233;
            public const int DiabolicaTheDraconiqueGeneral = 78077209;
            public const int ReignBeauxOverlordOfDarkWorld = 99458769;
            public const int LucentNetherlordOfDarkWorld = 34968834;
            public const int FiendsmithEngraver = 60764609;
            public const int DangerMothman = 52350806;
            public const int BeiigeVanguardOfDarkWorld = 33731070;
            public const int ZalamanderCatalyzer = 40460013;
            public const int GentaGatemanOfDarkWorld = 77895328;
            public const int BrowwHuntsmanOfDarkWorld = 79126789;
            public const int VersagoTheDestroyer = 50259460;
            public const int BuioTheDawnsLight = 19000848;
            public const int FabledRaven = 47217354;
            public const int ParlHermitOfDarkWorld = 3289027;
            public const int CeruliGuruOfDarkWorld = 7623640;

            // Spells & Traps
            public const int DarkCorridor = 98696958;
            public const int DarkWorldDealings = 74117290;
            public const int DraggedDownIntoTheGrave = 16435215;
            public const int FiendsmithsTract = 98567237;
            public const int MutinyInTheSky = 71593652;
            public const int DarkWorldAccession = 65956182;
            public const int DarkWorldPuppetry = 30284022;
            public const int DarkWorldArchives = 76672730;
            public const int TheGatesOfDarkWorld = 33017655;
            public const int DarkWorldBrainwashing = 10131855;
            public const int HarpiesFeatherDuster = 18144506;
            public const int CosmicCyclone = 8267140;

            // Extra Deck
            public const int ClorlessChaosKingOfDarkWorld = 22723778;
            public const int GraphaDragonOverlordOfDarkWorld = 39552584;
            public const int LuceTheDusksDark = 45409943;
            public const int AerialEater = 28143384;
            public const int SnakeEyesVengeanceDragon = 79415624;
            public const int SwordsoulSupremeSovereignChengying = 96633955;
            public const int FabledValkyrus = 54048462;
            public const int GrozaTyrantOfThunder = 45420955;
            public const int FabledAndwraith = 9061682;
            public const int HieraticSkyDragonOverlordOfHeliopolis = 3292267;
            public const int DingirsuTheOrcustOfTheEveningStar = 93854893;
            public const int DDDWaveHighKingCaesar = 79559912;
            public const int EvilswarmExcitonKnight = 46772449;
            public const int SteelswarmRoach = 37926346;

            // Side Deck & Extra
            public const int LavaGolem = 102380;
            public const int EvilswarmNightmare = 359563;
            public const int TwinTwisters = 43898403;
            public const int DarkWorldPunishment = 3167439;
        }

        // State Tracking per turn
        private bool _overkingSearchUsed = false;
        private bool _diabolicaUsed = false;
        private bool _engraverSearchUsed = false;
        private bool _engraverSummonUsed = false;
        private bool _catalyzerHandUsed = false;
        private bool _gentaSearchUsed = false;
        private bool _gentaBanishTriggerUsed = false;
        private bool _parlUsed = false;
        private bool _accessionFusionUsed = false;
        private bool _accessionGYUsed = false;
        private bool _mutinyFusionUsed = false;
        private bool _mutinyGYUsed = false;
        private bool _tractSearchUsed = false;
        private bool _corridorUsed = false;
        private bool _archivesDiscardUsed = false;
        private bool _puppetryUsed = false;
        private bool _puppetryGYUsed = false;
        private bool _overlordDisruptionUsed = false;
        private bool _ravenEffectUsed = false;
        private bool _snakeEyesMainUsed = false;
        private bool _valkyrusUsed = false;
        private bool _grozaUsed = false;
        private bool _luceIgnitionUsed = false;
        private bool _buioHandUsed = false;
        private bool _buioGYUsed = false;

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.GraphaDragonOverlordOfDarkWorld ||
                   card.Id == CardId.ClorlessChaosKingOfDarkWorld ||
                   card.Id == CardId.DDDWaveHighKingCaesar ||
                   card.Id == CardId.SnakeEyesVengeanceDragon ||
                   card.Id == CardId.SwordsoulSupremeSovereignChengying ||
                   card.Id == CardId.DingirsuTheOrcustOfTheEveningStar ||
                   card.Id == CardId.HieraticSkyDragonOverlordOfHeliopolis ||
                   card.Id == CardId.LuceTheDusksDark ||
                   card.Id == CardId.GrozaTyrantOfThunder ||
                   card.Id == CardId.FabledValkyrus ||
                   card.Id == CardId.FabledAndwraith ||
                   card.Id == CardId.SteelswarmRoach ||
                   card.Id == CardId.EvilswarmExcitonKnight;
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.Id == CardId.GraphaDragonLordOfDarkWorld || c.Id == CardId.ReignBeauxOverkingOfDarkWorld) return 400;
            if (c.Id == CardId.LucentNetherlordOfDarkWorld) return 200;
            return 100;
        }

        private int GetNonAceFiendsCount()
        {
            int handFiends = Bot.Hand.Count(c => c.IsMonster() && c.HasRace(CardRace.Fiend) && !IsAceCard(c));
            int fieldFiends = Bot.GetMonsters().Count(c => c.IsFaceup() && c.HasRace(CardRace.Fiend) && !IsAceCard(c));
            int gyFiends = Bot.Graveyard.Count(c => c.IsMonster() && c.HasRace(CardRace.Fiend) && !IsAceCard(c));
            return handFiends + fieldFiends + gyFiends;
        }

        public override bool OnSelectHand()
        {
            // Dark World prefers going first to establish Grapha Overlord + Caesar floodgate board
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _overkingSearchUsed = false;
            _diabolicaUsed = false;
            _engraverSearchUsed = false;
            _engraverSummonUsed = false;
            _catalyzerHandUsed = false;
            _gentaSearchUsed = false;
            _gentaBanishTriggerUsed = false;
            _parlUsed = false;
            _accessionFusionUsed = false;
            _accessionGYUsed = false;
            _mutinyFusionUsed = false;
            _mutinyGYUsed = false;
            _tractSearchUsed = false;
            _corridorUsed = false;
            _archivesDiscardUsed = false;
            _puppetryUsed = false;
            _puppetryGYUsed = false;
            _overlordDisruptionUsed = false;
            _ravenEffectUsed = false;
            _snakeEyesMainUsed = false;
            _valkyrusUsed = false;
            _grozaUsed = false;
            _luceIgnitionUsed = false;
            _buioHandUsed = false;
            _buioGYUsed = false;
        }

        public _2026_DarkWorldExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            HeuristicGuard.RegisterAceCards(
                CardId.GraphaDragonOverlordOfDarkWorld,
                CardId.ClorlessChaosKingOfDarkWorld,
                CardId.DDDWaveHighKingCaesar,
                CardId.SnakeEyesVengeanceDragon,
                CardId.SwordsoulSupremeSovereignChengying,
                CardId.DingirsuTheOrcustOfTheEveningStar,
                CardId.HieraticSkyDragonOverlordOfHeliopolis,
                CardId.LuceTheDusksDark,
                CardId.GrozaTyrantOfThunder,
                CardId.FabledValkyrus,
                CardId.FabledAndwraith,
                CardId.SteelswarmRoach,
                CardId.EvilswarmExcitonKnight
            );

            // ── Combo Router: Sequencing ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Grapha-Overlord-Lockdown",
                RequiredCards = new List<int> { CardId.TheGatesOfDarkWorld, CardId.GentaGatemanOfDarkWorld },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.GentaGatemanOfDarkWorld, ActionType = ExecutorType.Activate, Description = "Search Gates" },
                    new() { CardId = CardId.TheGatesOfDarkWorld, ActionType = ExecutorType.Activate, Description = "Activate Gates & Draw" }
                },
                EndBoardScore = 90
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Corridor-Starter",
                RequiredCards = new List<int> { CardId.DarkCorridor },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.DarkCorridor, ActionType = ExecutorType.Activate, Description = "Play Dark Corridor" }
                },
                EndBoardScore = 85
            });

            // ── Bait Planner ──
            BaitPlanner.RegisterComboStarters(
                CardId.DarkCorridor,
                CardId.GentaGatemanOfDarkWorld,
                CardId.TheGatesOfDarkWorld,
                CardId.DarkWorldAccession,
                CardId.FiendsmithsTract,
                CardId.ReignBeauxOverkingOfDarkWorld,
                CardId.BuioTheDawnsLight
            );
            BaitPlanner.RegisterBaitCards(CardId.DarkWorldDealings, CardId.DarkCorridor);

            // ── Chain Advisor ──
            ChainAdvisor.RegisterHighValueTargets(
                CardId.TheGatesOfDarkWorld,
                CardId.GraphaDragonOverlordOfDarkWorld,
                CardId.DarkWorldAccession,
                CardId.DDDWaveHighKingCaesar,
                CardId.SnakeEyesVengeanceDragon
            );

            // ══════════════════════════════════════════════════
            // TIER 1: Board Breakers, Hand Traps & Quick Negations
            // ══════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, DefaultHarpiesFeatherDusterFirst);
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, () => Enemy.GetSpellCount() > 0);
            AddExecutor(ExecutorType.Activate, CardId.CosmicCyclone, DefaultCosmicCyclone);
            AddExecutor(ExecutorType.Activate, CardId.GraphaDragonOverlordOfDarkWorld, OverlordDisruptionEffect);
            AddExecutor(ExecutorType.Activate, CardId.DDDWaveHighKingCaesar, CaesarNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.SteelswarmRoach, RoachNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.GrozaTyrantOfThunder, GrozaNegateAndDiscardEffect);
            AddExecutor(ExecutorType.Activate, CardId.SnakeEyesVengeanceDragon, SnakeEyesQuickEffect);
            AddExecutor(ExecutorType.Activate, CardId.EvilswarmExcitonKnight, ExcitonEffect);
            AddExecutor(ExecutorType.Activate, CardId.DingirsuTheOrcustOfTheEveningStar, DingirsuEffect);
            AddExecutor(ExecutorType.Activate, CardId.HieraticSkyDragonOverlordOfHeliopolis, HeliopolisEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarkWorldBrainwashing, BrainwashingNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarkWorldPunishment, PunishmentNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarkWorldPuppetry, PuppetryDisruptionEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarkWorldAccession, AccessionQuickFusionEffect);

            // ══════════════════════════════════════════════════
            // TIER 2: GY / Banish / Discard Trigger Effects
            // ══════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.GentaGatemanOfDarkWorld, GentaBanishTriggerEffect);
            AddExecutor(ExecutorType.Activate, CardId.BuioTheDawnsLight, BuioGYTriggerEffect);
            AddExecutor(ExecutorType.Activate, CardId.DiabolicaTheDraconiqueGeneral, DiabolicaEffect);
            AddExecutor(ExecutorType.Activate, CardId.ReignBeauxOverkingOfDarkWorld, OverkingDiscardEffect);
            AddExecutor(ExecutorType.Activate, CardId.GraphaDragonLordOfDarkWorld, GraphaDiscardEffect);
            AddExecutor(ExecutorType.Activate, CardId.ReignBeauxOverlordOfDarkWorld, OverlordGYEffect);
            AddExecutor(ExecutorType.Activate, CardId.LucentNetherlordOfDarkWorld, LucentGYEffect);
            AddExecutor(ExecutorType.Activate, CardId.BeiigeVanguardOfDarkWorld, BeiigeGYEffect);
            AddExecutor(ExecutorType.Activate, CardId.BrowwHuntsmanOfDarkWorld, BrowwGYEffect);
            AddExecutor(ExecutorType.Activate, CardId.ParlHermitOfDarkWorld, ParlGYEffect);
            AddExecutor(ExecutorType.Activate, CardId.CeruliGuruOfDarkWorld, CeruliGYEffect);
            AddExecutor(ExecutorType.Activate, CardId.DangerMothman, MothmanGYEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarkWorldArchives, ArchivesDrawTriggerEffect);
            AddExecutor(ExecutorType.Activate, CardId.LuceTheDusksDark, LuceCardDestroyedTriggerEffect);
            AddExecutor(ExecutorType.Activate, CardId.AerialEater, AerialEaterFusionTriggerEffect);
            AddExecutor(ExecutorType.Activate, CardId.ClorlessChaosKingOfDarkWorld, ClorlessTriggerEffect);
            AddExecutor(ExecutorType.Activate, CardId.FabledAndwraith, AndwraithTriggerEffect);

            // ══════════════════════════════════════════════════
            // TIER 2.5: High Priority Boss Revivals from GY
            // ══════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.GraphaDragonLordOfDarkWorld, GraphaGYSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ReignBeauxOverkingOfDarkWorld, OverkingGYSummon);

            // ══════════════════════════════════════════════════
            // TIER 3: Hand Ignition / Cycling Starters / On-Field Ignition
            // ══════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.LuceTheDusksDark, LuceIgnitionEffect);
            AddExecutor(ExecutorType.Activate, CardId.FabledValkyrus, ValkyrusDiscardDrawEffect);
            AddExecutor(ExecutorType.Activate, CardId.GentaGatemanOfDarkWorld, GentaSearchEffect);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithEngraver, EngraverSearchEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarkCorridor, CorridorEffect);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsTract, TractSearchEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheGatesOfDarkWorld, GatesEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarkWorldDealings, DealingsEffect);
            AddExecutor(ExecutorType.Activate, CardId.DraggedDownIntoTheGrave, DraggedDownEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarkWorldArchives, ArchivesIgnitionEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarkWorldPuppetry, PuppetryIgnitionEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarkWorldPuppetry, PuppetryGYRecycleEffect);
            AddExecutor(ExecutorType.Activate, CardId.ZalamanderCatalyzer, CatalyzerHandEffect);
            AddExecutor(ExecutorType.Activate, CardId.DangerMothman, MothmanHandEffect);
            AddExecutor(ExecutorType.Activate, CardId.BuioTheDawnsLight, BuioHandEffect);

            // ══════════════════════════════════════════════════
            // TIER 4: Fusion Spells & GY Recursion
            // ══════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.DarkWorldAccession, AccessionFusionEffect);
            AddExecutor(ExecutorType.Activate, CardId.MutinyInTheSky, MutinyFusionEffect);

            // ══════════════════════════════════════════════════
            // TIER 5: Special Summons (GY Recursion & Banish)
            // ══════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithEngraver, EngraverSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.AerialEater, AerialEaterGYSummon);

            // ══════════════════════════════════════════════════
            // TIER 6: Normal Summons & Raven Engine
            // ══════════════════════════════════════════════════
            AddExecutor(ExecutorType.Summon, CardId.FabledRaven, FabledRavenSummon);
            AddExecutor(ExecutorType.Activate, CardId.FabledRaven, FabledRavenDiscardEffect);
            AddExecutor(ExecutorType.Summon, CardId.GentaGatemanOfDarkWorld, NormalSummonNonTuner);
            AddExecutor(ExecutorType.Summon, CardId.BeiigeVanguardOfDarkWorld, NormalSummonNonTuner);
            AddExecutor(ExecutorType.Summon, CardId.DangerMothman, NormalSummonNonTuner);
            AddExecutor(ExecutorType.Summon, CardId.BrowwHuntsmanOfDarkWorld, NormalSummonNonTuner);

            // ══════════════════════════════════════════════════
            // TIER 7: Extra Deck Summons (Xyz, Synchro, Fusion)
            // ══════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.DDDWaveHighKingCaesar, CaesarSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SnakeEyesVengeanceDragon, SnakeEyesSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SwordsoulSupremeSovereignChengying, ChengyingSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DingirsuTheOrcustOfTheEveningStar, DingirsuSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.HieraticSkyDragonOverlordOfHeliopolis, HeliopolisSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.FabledValkyrus, ValkyrusSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.GrozaTyrantOfThunder, GrozaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.FabledAndwraith, AndwraithSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SteelswarmRoach, RoachSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.EvilswarmExcitonKnight, ExcitonSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.GraphaDragonOverlordOfDarkWorld, OverlordSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ClorlessChaosKingOfDarkWorld, ClorlessSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.LuceTheDusksDark, LuceSummon);

            // ══════════════════════════════════════════════════
            // TIER 8: Spell/Trap Sets & Battle Positions
            // ══════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, CardId.DarkWorldBrainwashing);
            AddExecutor(ExecutorType.SpellSet, CardId.DarkWorldPunishment);
            AddExecutor(ExecutorType.SpellSet, CardId.DarkWorldAccession, () => Duel.Phase == DuelPhase.Main2 || Duel.Turn == 1);
            AddExecutor(ExecutorType.SpellSet, CardId.DarkWorldPuppetry, () => Duel.Phase == DuelPhase.Main2 || Duel.Turn == 1);
            AddExecutor(ExecutorType.SpellSet, CardId.MutinyInTheSky, () => Duel.Phase == DuelPhase.Main2 || Duel.Turn == 1);
            AddExecutor(ExecutorType.SpellSet, CardId.CosmicCyclone, DefaultSpellSet);
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        // ==========================================
        //  TIER 1: Hand Traps & Negations
        // ==========================================

        private bool OverlordDisruptionEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (_overlordDisruptionUsed) return false;
            if (Duel.LastChainPlayer != 1) return false;
            if (Bot.Hand.Count == 0) return false;

            _overlordDisruptionUsed = true;
            return true;
        }

        private bool CaesarNegateEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (Card.Overlays.Count == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;
            return true;
        }

        private bool RoachNegateEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (Card.Overlays.Count == 0) return false;
            return true;
        }

        private bool GrozaNegateAndDiscardEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (_grozaUsed) return false;
            if (Duel.LastChainPlayer != 1) return false;

            bool hasMonsterInHand = Bot.Hand.Any(c => c.IsMonster());
            if (!hasMonsterInHand) return false;

            _grozaUsed = true;
            return true;
        }

        private bool SnakeEyesQuickEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (_snakeEyesMainUsed) return false;

            // Main Phase quick effect: place face-up monster on field or GY into S/T zone
            if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
            {
                bool oppHasTarget = Enemy.GetMonsters().Any(m => m.IsFaceup()) || Enemy.Graveyard.Any(m => m.IsMonster());
                if (oppHasTarget)
                {
                    _snakeEyesMainUsed = true;
                    return true;
                }
            }

            // Battle Phase quick effect: destroy 1 monster & 1 continuous spell
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep || Duel.Phase == DuelPhase.Damage)
            {
                bool hasSpellTarget = Bot.GetSpells().Any(s => s.IsFaceup() && s.HasType(CardType.Continuous))
                    || Enemy.GetSpells().Any(s => s.IsFaceup() && s.HasType(CardType.Continuous));
                bool hasMonster = Enemy.GetMonsterCount() > 0;
                return hasSpellTarget && hasMonster;
            }

            return false;
        }

        private bool ExcitonEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (Card.Overlays.Count == 0) return false;
            int myCount = Bot.Hand.Count + Bot.GetFieldCount();
            int oppCount = Enemy.Hand.Count + Enemy.GetFieldCount();
            return oppCount > myCount;
        }

        private bool DingirsuEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            return Enemy.GetFieldCount() > 0;
        }

        private bool HeliopolisEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (Card.Overlays.Count == 0) return false;
            return Enemy.GetFieldCount() >= 2;
        }

        private bool BrainwashingNegateEffect()
        {
            if (Card.Location == CardLocation.Hand) return false;
            if (Bot.Hand.Count < 3) return false;
            if (Duel.LastChainPlayer != 1) return false;
            bool hasDWOnField = Bot.GetMonsters().Any(c => c.IsFaceup() && c.HasSetcode(0x6));
            return hasDWOnField;
        }

        private bool PunishmentNegateEffect()
        {
            if (Card.Location == CardLocation.Hand) return false;
            if (Card.Location == CardLocation.Grave) return true; // GY destruction protection
            return Bot.Hand.Any(c => c.IsMonster() && c.HasRace(CardRace.Fiend));
        }

        private bool PuppetryDisruptionEffect()
        {
            if (_puppetryUsed) return false;
            // Quick-play banish up to 3 from opp GY in opponent's turn or during chain
            if (Card.Location == CardLocation.SpellZone && Card.IsFacedown() && Duel.Turn > 1)
            {
                bool oppHasKeyCards = Enemy.Graveyard.Any(c => c.IsMonster());
                bool hasFiendToDiscard = Bot.Hand.Any(c => c.IsMonster() && c.HasRace(CardRace.Fiend));
                if (oppHasKeyCards && hasFiendToDiscard)
                {
                    _puppetryUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool AccessionQuickFusionEffect()
        {
            if (_accessionFusionUsed) return false;
            // Fusion summon Grapha Overlord during opponent's turn
            if (Duel.LastChainPlayer == 1 && Card.Location == CardLocation.SpellZone && Card.IsFacedown())
            {
                bool hasGrapha = Bot.Hand.Any(c => c.Id == CardId.GraphaDragonLordOfDarkWorld)
                    || Bot.GetMonsters().Any(c => c.IsFaceup() && c.Id == CardId.GraphaDragonLordOfDarkWorld);
                int totalFiends = GetNonAceFiendsCount();
                if (hasGrapha && totalFiends >= 2 && GetRemainingCount(CardId.GraphaDragonOverlordOfDarkWorld) > 0)
                {
                    _accessionFusionUsed = true;
                    return true;
                }
            }
            return false;
        }

        // ==========================================
        //  TIER 2: GY/Banish/Discard Trigger Effects
        // ==========================================

        private bool GentaBanishTriggerEffect()
        {
            if (Card.Location != CardLocation.Removed) return false;
            if (_gentaBanishTriggerUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            bool controlsDW = Bot.GetMonsters().Any(c => c.IsFaceup() && c.HasSetcode(0x6))
                || Bot.GetSpells().Any(c => c.IsFaceup() && c.HasSetcode(0x6));
            if (!controlsDW) return false;

            _gentaBanishTriggerUsed = true;
            return true;
        }

        private bool BuioGYTriggerEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_buioGYUsed) return false;

            _buioGYUsed = true;
            return true; // Adds Mutiny in the Sky from Deck to Hand!
        }

        private bool DiabolicaEffect()
        {
            if (_diabolicaUsed) return false;
            _diabolicaUsed = true;
            return true;
        }

        private bool OverkingDiscardEffect()
        {
            if (_overkingSearchUsed) return false;
            _overkingSearchUsed = true;
            return true;
        }

        private bool GraphaDiscardEffect()
        {
            return true;
        }

        private bool OverlordGYEffect() { return true; }
        private bool LucentGYEffect() { return true; }
        private bool BeiigeGYEffect() { return true; }
        private bool BrowwGYEffect() { return true; }

        private bool ParlGYEffect()
        {
            if (_parlUsed) return false;
            bool hasTarget = Bot.Graveyard.Any(c => c.IsMonster() && c.HasSetcode(0x6) && c.Id != CardId.ParlHermitOfDarkWorld && c.IsCanRevive());
            if (!hasTarget) return false;
            _parlUsed = true;
            return true;
        }

        private bool CeruliGYEffect()
        {
            // Ceruli SP summons to opponent field when discarded
            return Enemy.GetMonsterCount() < 5;
        }

        private bool MothmanGYEffect() { return true; }

        private bool ArchivesDrawTriggerEffect()
        {
            if (Card.Location != CardLocation.SpellZone || !Card.IsFaceup()) return false;
            // When DW discarded by card effect: discard 1, draw 2!
            return Bot.Hand.Any(c => c.IsMonster());
        }

        private bool LuceCardDestroyedTriggerEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            return Enemy.GetFieldCount() > 0;
        }

        private bool AerialEaterFusionTriggerEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            return true; // Sends 1 Fiend from Deck to GY (Diabolica / Grapha / Overking)
        }

        private bool ClorlessTriggerEffect() { return true; }
        private bool AndwraithTriggerEffect() { return true; }

        // ==========================================
        //  TIER 3: Hand Ignition / Cycling
        // ==========================================

        private bool LuceIgnitionEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (_luceIgnitionUsed) return false;
            if (Enemy.GetFieldCount() == 0) return false;

            _luceIgnitionUsed = true;
            return true; // Send Fiend from Deck to GY & Pop 1 opponent card!
        }

        private bool ValkyrusDiscardDrawEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (_valkyrusUsed) return false;

            bool hasFiend = Bot.Hand.Any(c => c.IsMonster() && c.HasRace(CardRace.Fiend));
            if (!hasFiend) return false;

            _valkyrusUsed = true;
            return true; // Discard 1 Fiend & Draw 1 card!
        }

        private bool GentaSearchEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_gentaSearchUsed) return false;
            if (Bot.HasInSpellZone(CardId.TheGatesOfDarkWorld) || Bot.Hand.Any(c => c.Id == CardId.TheGatesOfDarkWorld))
                return false;

            _gentaSearchUsed = true;
            return true;
        }

        private bool EngraverSearchEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_engraverSearchUsed) return false;
            if (Bot.Hand.Any(c => c.Id == CardId.FiendsmithsTract))
                return false;

            _engraverSearchUsed = true;
            return true;
        }

        private bool CorridorEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_corridorUsed) return false;
            if (Bot.Hand.Count < 2) return false;

            _corridorUsed = true;
            return true;
        }

        private bool TractSearchEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_tractSearchUsed) return false;
            if (Bot.Hand.Count < 2) return false;

            _tractSearchUsed = true;
            return true;
        }

        private bool GatesEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (Bot.HasInSpellZone(CardId.TheGatesOfDarkWorld)) return false;
                return true;
            }

            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                bool hasGYFiend = Bot.Graveyard.Any(c => c.IsMonster() && c.HasRace(CardRace.Fiend));
                bool hasHandFiend = Bot.Hand.Any(c => c.IsMonster() && c.HasRace(CardRace.Fiend));
                return hasGYFiend && hasHandFiend;
            }
            return false;
        }

        private bool DealingsEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            return Bot.Hand.Any(c => c != Card && (c.HasSetcode(0x6) || c.HasRace(CardRace.Fiend)));
        }

        private bool DraggedDownEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            return Bot.Hand.Any(c => c != Card && c.HasSetcode(0x6));
        }

        private bool ArchivesIgnitionEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (Bot.HasInSpellZone(CardId.DarkWorldArchives)) return false;
                return true;
            }

            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                if (_archivesDiscardUsed) return false;
                bool hasDw = Bot.Hand.Any(c => c.IsMonster() && c.HasSetcode(0x6));
                bool hasFieldMonsters = Bot.GetMonsters().Any(c => c.IsFaceup() && c.HasSetcode(0x6));
                if (!hasDw || !hasFieldMonsters) return false;

                _archivesDiscardUsed = true;
                return true;
            }
            return true;
        }

        private bool PuppetryIgnitionEffect()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFaceup()))
            {
                if (_puppetryUsed) return false;
                bool hasFiend = Bot.Hand.Any(c => c.IsMonster() && c.HasRace(CardRace.Fiend));
                if (!hasFiend) return false;
                bool oppHasGY = Enemy.Graveyard.Count > 0;
                if (!oppHasGY) return false;

                _puppetryUsed = true;
                return true;
            }
            return false;
        }

        private bool PuppetryGYRecycleEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                if (_puppetryGYUsed) return false;
                bool hasBanishedFiend = Bot.Banished.Any(c => c.IsMonster() && c.HasRace(CardRace.Fiend));
                if (!hasBanishedFiend) return false;

                _puppetryGYUsed = true;
                return true;
            }
            return false;
        }

        private bool CatalyzerHandEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_catalyzerHandUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            bool hasOtherFiend = Bot.Hand.Any(c => c != Card && c.IsMonster() && c.HasRace(CardRace.Fiend));
            if (!hasOtherFiend) return false;

            _catalyzerHandUsed = true;
            return true;
        }

        private bool MothmanHandEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            return true;
        }

        private bool BuioHandEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_buioHandUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            // Target 1 face-up Fiend Effect Monster on field, negate its effect, SS Buio
            bool hasTarget = Bot.GetMonsters().Any(c => c.IsFaceup() && c.HasRace(CardRace.Fiend) && c.HasType(CardType.Effect) && !IsAceCard(c));
            if (!hasTarget) return false;

            _buioHandUsed = true;
            return true;
        }

        // ==========================================
        //  TIER 4: Fusion Spells & GY Recursion
        // ==========================================

        private bool AccessionFusionEffect()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                if (_accessionFusionUsed) return false;
                if (IsSpecialSummonBlocked()) return false;

                bool hasGrapha = Bot.Hand.Any(c => c.Id == CardId.GraphaDragonLordOfDarkWorld)
                    || Bot.GetMonsters().Any(c => c.IsFaceup() && c.Id == CardId.GraphaDragonLordOfDarkWorld);
                bool hasOverking = Bot.Hand.Any(c => c.Id == CardId.ReignBeauxOverkingOfDarkWorld || c.Id == CardId.ReignBeauxOverlordOfDarkWorld)
                    || Bot.GetMonsters().Any(c => c.IsFaceup() && (c.Id == CardId.ReignBeauxOverkingOfDarkWorld || c.Id == CardId.ReignBeauxOverlordOfDarkWorld));
                int totalFiends = GetNonAceFiendsCount();

                bool canSummonGrapha = hasGrapha && totalFiends >= 2 && GetRemainingCount(CardId.GraphaDragonOverlordOfDarkWorld) > 0;
                bool canSummonClorless = hasOverking && totalFiends >= 3 && GetRemainingCount(CardId.ClorlessChaosKingOfDarkWorld) > 0;
                bool canSummonGeneric = totalFiends >= 2 && (GetRemainingCount(CardId.LuceTheDusksDark) > 0 || GetRemainingCount(CardId.AerialEater) > 0);

                if (!canSummonGrapha && !canSummonClorless && !canSummonGeneric) return false;

                _accessionFusionUsed = true;
                return true;
            }

            if (Card.Location == CardLocation.Grave)
            {
                if (_accessionGYUsed) return false;
                bool hasDw = Bot.Hand.Any(c => c.IsMonster() && c.HasSetcode(0x6));
                if (!hasDw) return false;

                _accessionGYUsed = true;
                return true;
            }
            return false;
        }

        private bool MutinyFusionEffect()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                if (_mutinyFusionUsed) return false;
                if (IsSpecialSummonBlocked()) return false;

                // Mutiny in the Sky shuffles materials FROM GRAVEYARD into Deck!
                var gyMats = Bot.Graveyard.Where(c => c.IsMonster() && (c.HasRace(CardRace.Fiend) || c.HasRace(CardRace.Fairy))).ToList();
                if (gyMats.Count < 2) return false;

                bool canSummonLuce = gyMats.Count >= 3 && GetRemainingCount(CardId.LuceTheDusksDark) > 0;
                bool canSummonGrapha = gyMats.Any(c => c.Id == CardId.GraphaDragonLordOfDarkWorld) && gyMats.Any(c => c.HasAttribute(CardAttribute.Dark)) && GetRemainingCount(CardId.GraphaDragonOverlordOfDarkWorld) > 0;
                bool canSummonAerial = gyMats.GroupBy(c => c.Attribute).Any(g => g.Count() >= 2) && GetRemainingCount(CardId.AerialEater) > 0;

                if (!canSummonLuce && !canSummonGrapha && !canSummonAerial) return false;

                _mutinyFusionUsed = true;
                return true;
            }

            if (Card.Location == CardLocation.Grave)
            {
                if (_mutinyGYUsed) return false;
                // Send 1 Fiend/Fairy from hand or face-up field to GY -> Add Mutiny to hand!
                // Prioritize Hand Fiends so we don't accidentally sacrifice field bosses!
                bool hasHandFiend = Bot.Hand.Any(c => c.IsMonster() && (c.HasRace(CardRace.Fiend) || c.HasRace(CardRace.Fairy)));
                bool hasFieldNonAce = Bot.GetMonsters().Any(c => c.IsFaceup() && c.HasRace(CardRace.Fiend) && !IsAceCard(c));
                if (!hasHandFiend && !hasFieldNonAce) return false;

                _mutinyGYUsed = true;
                return true;
            }
            return false;
        }

        // ==========================================
        //  TIER 5: Special Summons (GY Recursion)
        // ==========================================

        private bool GraphaGYSummon()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (IsSpecialSummonBlocked()) return false;

            // Return face-up Dark World monster to hand (except Grapha & Aces)
            bool hasBounceTarget = Bot.GetMonsters().Any(c => c.IsFaceup() && c.HasSetcode(0x6) && c.Id != CardId.GraphaDragonLordOfDarkWorld && !IsAceCard(c));
            return hasBounceTarget;
        }

        private bool OverkingGYSummon()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (IsSpecialSummonBlocked()) return false;

            // Return Level 7 or lower Dark World monster to hand
            bool hasBounceTarget = Bot.GetMonsters().Any(c => c.IsFaceup() && c.HasSetcode(0x6) && c.Level <= 7 && !IsAceCard(c));
            return hasBounceTarget;
        }

        private bool EngraverSpSummon()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_engraverSummonUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            bool hasLightFiend = Bot.Graveyard.Concat(Bot.Banished).Any(c => c != Card && c.IsMonster() && c.HasAttribute(CardAttribute.Light) && c.HasRace(CardRace.Fiend));
            if (!hasLightFiend) return false;

            _engraverSummonUsed = true;
            return true;
        }

        private bool AerialEaterGYSummon()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (IsSpecialSummonBlocked()) return false;

            var candidates = Bot.Graveyard
                .Where(c => c != null && c.IsMonster() && c.HasRace(CardRace.Fiend) && c.Level >= 6 && c.Id != CardId.AerialEater && c.Id != CardId.GraphaDragonLordOfDarkWorld && c.Id != CardId.ReignBeauxOverkingOfDarkWorld)
                .ToList();
            if (candidates.Count < 2) return false;
            var groups = candidates.GroupBy(c => c.Attribute);
            return groups.Any(g => g.Count() >= 2);
        }

        // ==========================================
        //  TIER 6: Normal Summons & Raven Engine
        // ==========================================

        private bool FabledRavenSummon()
        {
            if (IsBoardStrongEnough()) return false;
            bool hasDw = Bot.Hand.Any(c => c.IsMonster() && c.HasSetcode(0x6));
            return hasDw;
        }

        private bool FabledRavenDiscardEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (_ravenEffectUsed) return false;

            int dwCount = Bot.Hand.Count(c => c.IsMonster() && c.HasSetcode(0x6));
            if (dwCount == 0) return false;

            _ravenEffectUsed = true;
            return true;
        }

        private bool NormalSummonNonTuner()
        {
            if (IsBoardStrongEnough()) return false;
            return Bot.GetMonsterCount() < 5;
        }

        // ==========================================
        //  TIER 7: Extra Deck Summons
        // ==========================================

        private bool CaesarSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            int lv6Count = Bot.GetMonsters().Count(c => c.IsFaceup() && c.Level == 6 && c.HasRace(CardRace.Fiend) && !IsAceCard(c));
            return lv6Count >= 2;
        }

        private bool SnakeEyesSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool DingirsuSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            int lv8Count = Bot.GetMonsters().Count(c => c.IsFaceup() && c.Level == 8 && !IsAceCard(c));
            return lv8Count >= 2 && (Enemy.GetFieldCount() > 0 || Duel.Turn == 1);
        }

        private bool HeliopolisSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            int lv8Count = Bot.GetMonsters().Count(c => c.IsFaceup() && c.Level == 8 && !IsAceCard(c));
            return lv8Count >= 2 && Enemy.GetFieldCount() >= 2;
        }

        private bool RoachSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Duel.Turn == 1 || Duel.Phase == DuelPhase.Main2)
            {
                int lv4Count = Bot.GetMonsters().Count(c => c.IsFaceup() && c.Level == 4 && !IsAceCard(c));
                return lv4Count >= 2;
            }
            return false;
        }

        private bool ExcitonSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            int myCount = Bot.Hand.Count + Bot.GetFieldCount();
            int oppCount = Enemy.Hand.Count + Enemy.GetFieldCount();
            if (oppCount > myCount)
            {
                int lv4Count = Bot.GetMonsters().Count(c => c.IsFaceup() && c.Level == 4 && !IsAceCard(c));
                return lv4Count >= 2;
            }
            return false;
        }

        private bool ChengyingSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool ValkyrusSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool GrozaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool AndwraithSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool OverlordSummon()
        {
            return !IsSpecialSummonBlocked();
        }

        private bool ClorlessSummon()
        {
            return !IsSpecialSummonBlocked();
        }

        private bool LuceSummon()
        {
            return !IsSpecialSummonBlocked();
        }

        // ==========================================
        //  Battle & Positioning
        // ==========================================

        private bool IsSafeToAttack(ClientCard attacker)
        {
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
                if (monster == null || IsAceCard(monster)) continue;
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

        // ==========================================
        //  Intelligent Discard Priority Matrix
        // ==========================================

        private int GetDiscardPriority(ClientCard c)
        {
            if (c == null) return 999;

            // Check if this discard is triggered by opponent's effect (Ceruli, Grapha Overlord changed effect, Dragged Down)
            bool isOppEffect = (Card != null && (Card.Id == CardId.CeruliGuruOfDarkWorld || Card.Controller == 1 || Duel.LastChainPlayer == 1));

            if (isOppEffect)
            {
                // Discarded by opponent card effect: Triggers catastrophic secondary effects!
                if (c.Id == CardId.ReignBeauxOverlordOfDarkWorld) return 1; // TOTAL BOARD WIPE!
                if (c.Id == CardId.ReignBeauxOverkingOfDarkWorld) return 2; // Search Lv5+ & SS from Deck!
                if (c.Id == CardId.LucentNetherlordOfDarkWorld) return 3;   // SS Lucent & SS ANY Fiend from Deck!
                if (c.Id == CardId.GraphaDragonLordOfDarkWorld) return 4;   // Pop 1 card & Steal 1 hand monster!
                if (c.Id == CardId.BrowwHuntsmanOfDarkWorld) return 5;      // Draw 2 cards!
                if (c.Id == CardId.BeiigeVanguardOfDarkWorld) return 6;     // SS self!
                if (c.Id == CardId.ParlHermitOfDarkWorld) return 7;         // SS DW from GY & discard!
                if (c.Id == CardId.DiabolicaTheDraconiqueGeneral) return 8; // Recycle Fiend!
                if (c.Id == CardId.CeruliGuruOfDarkWorld) return 9;
                return 15;
            }

            // Normal / Friendly Discards (Gates, Dealings, Corridor, Accession, Tract, Catalyzer, Raven)
            // Priority 1: Ceruli (if opponent has field space to summon and trigger opp-discard!)
            if (c.Id == CardId.CeruliGuruOfDarkWorld && Enemy.GetMonsterCount() < 5 && Bot.Hand.Any(h => h != c && (h.Id == CardId.ReignBeauxOverlordOfDarkWorld || h.Id == CardId.ReignBeauxOverkingOfDarkWorld || h.Id == CardId.LucentNetherlordOfDarkWorld || h.Id == CardId.GraphaDragonLordOfDarkWorld)))
                return 5;

            if (c.Id == CardId.BrowwHuntsmanOfDarkWorld) return 10;          // Draw 1 card
            if (c.Id == CardId.ReignBeauxOverkingOfDarkWorld) return 15;      // Search Lv5+ DW
            if (c.Id == CardId.GraphaDragonLordOfDarkWorld) return 20;        // Pop 1 opp card & load GY
            if (c.Id == CardId.LucentNetherlordOfDarkWorld) return 25;        // SS self to field
            if (c.Id == CardId.BeiigeVanguardOfDarkWorld) return 30;         // SS self to field
            if (c.Id == CardId.DiabolicaTheDraconiqueGeneral) return 35;     // Add Fiend from GY
            if (c.Id == CardId.DangerMothman) return 40;                     // Draw 1 & discard 1
            if (c.Id == CardId.GentaGatemanOfDarkWorld) return 45;           // Discard fodder
            if (c.Id == CardId.BuioTheDawnsLight) return 48;                 // Search Mutiny!
            if (c.Id == CardId.ParlHermitOfDarkWorld) return 50;             // Revive from GY
            if (c.Id == CardId.CeruliGuruOfDarkWorld) return 55;
            if (c.Id == CardId.ZalamanderCatalyzer) return 60;
            if (c.Id == CardId.FiendsmithEngraver) return 65;
            if (c.Id == CardId.FabledRaven) return 80;

            return 200;
        }

        // ==========================================
        //  Hint-Based Card Selection (OnSelectCard)
        // ==========================================

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // Special handling for Mutiny in the Sky GY Recycle effect:
            // "Send 1 Fiend/Fairy from hand or face-up field to GY; add this card to hand"
            if (Card != null && Card.Id == CardId.MutinyInTheSky && Card.Location == CardLocation.Grave)
            {
                var mutinySendPriority = cards.OrderBy(c => {
                    if (IsAceCard(c)) return 999; // NEVER sacrifice an Ace from field!
                    if (c.Location == CardLocation.Hand && c.HasSetcode(0x6)) return 1; // Discarding DW from hand triggers DW!
                    if (c.Location == CardLocation.Hand && (c.HasRace(CardRace.Fiend) || c.HasRace(CardRace.Fairy))) return 5;
                    if (c.Location == CardLocation.MonsterZone && !IsAceCard(c)) return 20;
                    return 100;
                }).ToList();
                return mutinySendPriority.Take(min).ToList();
            }

            // Extra Deck Special Summon Selection (hint 509 / Fusion / Synchro / Xyz)
            var extraCards = cards.Where(c => c != null && c.Location == CardLocation.Extra).ToList();
            if (extraCards.Count > 0)
            {
                var sortedExtra = extraCards.OrderBy(c => {
                    if (Duel.Turn == 1 || Duel.Phase == DuelPhase.Main2)
                    {
                        if (c.Id == CardId.GraphaDragonOverlordOfDarkWorld) return 1;
                        if (c.Id == CardId.DDDWaveHighKingCaesar) return 2;
                        if (c.Id == CardId.SnakeEyesVengeanceDragon) return 3;
                        if (c.Id == CardId.SteelswarmRoach) return 4;
                        if (c.Id == CardId.GrozaTyrantOfThunder) return 5;
                        if (c.Id == CardId.DingirsuTheOrcustOfTheEveningStar) return 6;
                        if (c.Id == CardId.SwordsoulSupremeSovereignChengying) return 7;
                        if (c.Id == CardId.LuceTheDusksDark) return 8;
                        if (c.Id == CardId.FabledValkyrus) return 9;
                        if (c.Id == CardId.AerialEater) return 10;
                        if (c.Id == CardId.FabledAndwraith) return 11;
                        if (c.Id == CardId.ClorlessChaosKingOfDarkWorld) return 12;
                    }
                    else // Going Second / Board Breaking OTK
                    {
                        if (Enemy.GetFieldCount() >= 2 && c.Id == CardId.ClorlessChaosKingOfDarkWorld) return 1;
                        if (c.Id == CardId.GraphaDragonOverlordOfDarkWorld) return 2;
                        if (c.Id == CardId.SnakeEyesVengeanceDragon) return 3;
                        if (c.Id == CardId.DingirsuTheOrcustOfTheEveningStar) return 4;
                        if (c.Id == CardId.DDDWaveHighKingCaesar) return 5;
                        if (c.Id == CardId.SwordsoulSupremeSovereignChengying) return 6;
                        if (c.Id == CardId.LuceTheDusksDark) return 7;
                        if (c.Id == CardId.AerialEater) return 8;
                        if (c.Id == CardId.EvilswarmExcitonKnight) return 9;
                    }
                    return 50;
                }).ToList();
                return sortedExtra.Take(min).ToList();
            }

            // Hint 575 = HINTMSG_NEGATE / Hint 504 = HINTMSG_DESTROY / Target Selection / Snake-Eyes target
            if (hint == 575 || hint == 504 || (Card != null && (Card.Id == CardId.GraphaDragonLordOfDarkWorld || Card.Id == CardId.SnakeEyesVengeanceDragon || Card.Id == CardId.LuceTheDusksDark)))
            {
                var opponentCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (opponentCards.Count > 0)
                {
                    var sorted = opponentCards.OrderByDescending(c => {
                        if (c.IsFaceup() && (c.IsSpell() || c.IsTrap()) && (c.HasType(CardType.Continuous) || c.HasType(CardType.Field))) return 5000;
                        if (c.IsMonster() && c.IsFaceup()) return c.Attack;
                        if (c.IsFacedown()) return 1500;
                        return 1000;
                    }).ToList();
                    return sorted.Take(max).ToList();
                }

                var ourCards = cards.Where(c => c != null && c.Controller == 0).ToList();
                if (ourCards.Count > 0)
                {
                    var sortedOur = ourCards.OrderBy(c => {
                        if (IsAceCard(c)) return 999;
                        if (c.IsMonster()) return c.Attack;
                        return 100;
                    }).ToList();
                    return sortedOur.Take(max).ToList();
                }
            }

            // Hint 506 = HINTMSG_ATOHAND (Searching from Deck / GY)
            if (hint == 506)
            {
                // Dark Corridor search
                if (Card != null && Card.Id == CardId.DarkCorridor)
                {
                    var corridorTargets = cards.OrderBy(c => {
                        if (!Bot.HasInSpellZone(CardId.TheGatesOfDarkWorld) && !Bot.Hand.Any(h => h.Id == CardId.TheGatesOfDarkWorld) && c.Id == CardId.GentaGatemanOfDarkWorld) return 1;
                        if (c.Id == CardId.ReignBeauxOverkingOfDarkWorld) return 2;
                        if (c.Id == CardId.GraphaDragonLordOfDarkWorld) return 3;
                        if (c.Id == CardId.BrowwHuntsmanOfDarkWorld) return 4;
                        if (c.Id == CardId.CeruliGuruOfDarkWorld) return 5;
                        if (c.Id == CardId.LucentNetherlordOfDarkWorld) return 6;
                        return 50;
                    }).ToList();
                    return corridorTargets.Take(min).ToList();
                }

                // Reign-Beaux Overking search (Lv 5+ Dark World)
                if (Card != null && Card.Id == CardId.ReignBeauxOverkingOfDarkWorld)
                {
                    var overkingTargets = cards.OrderBy(c => {
                        if (!Bot.Hand.Any(h => h.Id == CardId.GraphaDragonLordOfDarkWorld) && c.Id == CardId.GraphaDragonLordOfDarkWorld) return 1;
                        if (Bot.Hand.Any(h => h.Id == CardId.CeruliGuruOfDarkWorld) && c.Id == CardId.ReignBeauxOverlordOfDarkWorld) return 2;
                        if (c.Id == CardId.LucentNetherlordOfDarkWorld) return 3;
                        if (c.Id == CardId.DiabolicaTheDraconiqueGeneral) return 4;
                        if (c.Id == CardId.GraphaDragonLordOfDarkWorld) return 5;
                        return 50;
                    }).ToList();
                    return overkingTargets.Take(min).ToList();
                }

                // Fiendsmith's Tract search (LIGHT Fiend)
                if (Card != null && Card.Id == CardId.FiendsmithsTract)
                {
                    var tractTargets = cards.OrderBy(c => {
                        if (c.Id == CardId.FiendsmithEngraver) return 1;
                        if (c.Id == CardId.FabledRaven) return 2;
                        return 50;
                    }).ToList();
                    return tractTargets.Take(min).ToList();
                }

                // Diabolica search from GY
                if (Card != null && Card.Id == CardId.DiabolicaTheDraconiqueGeneral)
                {
                    var diabolicaTargets = cards.OrderBy(c => {
                        if (c.Id == CardId.ReignBeauxOverkingOfDarkWorld) return 1;
                        if (c.Id == CardId.GraphaDragonLordOfDarkWorld) return 2;
                        if (c.Id == CardId.BrowwHuntsmanOfDarkWorld) return 3;
                        if (c.Id == CardId.GentaGatemanOfDarkWorld) return 4;
                        if (c.Id == CardId.FiendsmithEngraver) return 5;
                        return 50;
                    }).ToList();
                    return diabolicaTargets.Take(min).ToList();
                }
            }

            // Hint 507 = HINTMSG_RTOHAND (Return to hand for Grapha / Overking revival)
            if (hint == 507 || (cards.All(c => c.Location == CardLocation.MonsterZone && c.Controller == 0) && (Card != null && (Card.Id == CardId.GraphaDragonLordOfDarkWorld || Card.Id == CardId.ReignBeauxOverkingOfDarkWorld))))
            {
                var bounceCandidates = cards.OrderBy(c => {
                    if (IsAceCard(c)) return 999;
                    if (c.Id == CardId.BrowwHuntsmanOfDarkWorld) return 1; // Can be re-discarded to draw!
                    if (c.Id == CardId.CeruliGuruOfDarkWorld) return 2;
                    if (c.Id == CardId.BeiigeVanguardOfDarkWorld) return 3;
                    if (c.Id == CardId.GentaGatemanOfDarkWorld) return 4;
                    if (c.Id == CardId.ParlHermitOfDarkWorld) return 5;
                    if (c.Id == CardId.DangerMothman) return 6;
                    if (c.Id == CardId.LucentNetherlordOfDarkWorld) return 7;
                    if (c.Id == CardId.DiabolicaTheDraconiqueGeneral) return 8;
                    return 100;
                }).ToList();
                return bounceCandidates.Take(min).ToList();
            }

            // Hint 509 = HINTMSG_SPSUMMON (Special Summon Selection)
            if (hint == 509)
            {
                // If Special Summoning from Deck (Lucent / Overking effect)
                var deckTargets = cards.Where(c => c.Location == CardLocation.Deck).OrderBy(c => {
                    if (c.Id == CardId.ReignBeauxOverkingOfDarkWorld) return 1;
                    if (c.Id == CardId.GraphaDragonLordOfDarkWorld) return 2;
                    if (c.Id == CardId.DiabolicaTheDraconiqueGeneral) return 3;
                    if (c.Id == CardId.LucentNetherlordOfDarkWorld) return 4;
                    if (c.Id == CardId.GentaGatemanOfDarkWorld) return 5;
                    if (c.Id == CardId.BeiigeVanguardOfDarkWorld) return 6;
                    if (c.Id == CardId.BrowwHuntsmanOfDarkWorld) return 7;
                    return 50;
                }).ToList();
                if (deckTargets.Count >= min)
                    return deckTargets.Take(min).ToList();

                // If Special Summoning from GY
                var gyTargets = cards.Where(c => c.Location == CardLocation.Grave).OrderBy(c => {
                    if (c.Id == CardId.GraphaDragonOverlordOfDarkWorld) return 1;
                    if (c.Id == CardId.GraphaDragonLordOfDarkWorld) return 2;
                    if (c.Id == CardId.ReignBeauxOverkingOfDarkWorld) return 3;
                    if (c.Id == CardId.LucentNetherlordOfDarkWorld) return 4;
                    return 50;
                }).ToList();
                if (gyTargets.Count >= min)
                    return gyTargets.Take(min).ToList();
            }

            // Hint 501 / Discard Selection (Gates, Dealings, Accession, Archives, Corridor, Tract, Dragged Down, Raven, Valkyrus, Groza)
            if (hint == 501 || (Card != null && (
                Card.Id == CardId.TheGatesOfDarkWorld ||
                Card.Id == CardId.DarkWorldDealings ||
                Card.Id == CardId.DarkWorldAccession ||
                Card.Id == CardId.DarkWorldArchives ||
                Card.Id == CardId.DarkCorridor ||
                Card.Id == CardId.FiendsmithsTract ||
                Card.Id == CardId.DraggedDownIntoTheGrave ||
                Card.Id == CardId.CeruliGuruOfDarkWorld ||
                Card.Id == CardId.FabledRaven ||
                Card.Id == CardId.FabledValkyrus ||
                Card.Id == CardId.GrozaTyrantOfThunder ||
                Card.Id == CardId.DarkWorldPuppetry
            )))
            {
                var discardCandidates = cards.OrderBy(GetDiscardPriority).ToList();
                return discardCandidates.Take(min).ToList();
            }

            // Hint 511 = HINTMSG_REMOVE (Banish for Cost e.g. Gates / Aerial Eater / Puppetry)
            if (hint == 511)
            {
                var removeCandidates = cards.OrderBy(c => {
                    if (IsAceCard(c)) return 999;
                    if (c.Id == CardId.GentaGatemanOfDarkWorld && c.Location == CardLocation.Grave) return 1; // Banish Genta triggers SS!
                    if (c.Id == CardId.GraphaDragonLordOfDarkWorld || c.Id == CardId.ReignBeauxOverkingOfDarkWorld) return 900; // Preserve for GY bounce
                    if (c.Location == CardLocation.Grave) return 10;
                    if (c.Location == CardLocation.Hand) return 50;
                    return 100;
                }).ToList();
                return removeCandidates.Take(min).ToList();
            }

            // Sending from Deck to GY (Luce / Aerial Eater)
            if (Card != null && (Card.Id == CardId.LuceTheDusksDark || Card.Id == CardId.AerialEater))
            {
                var sendCandidates = cards.OrderBy(c => {
                    if (c.Id == CardId.DiabolicaTheDraconiqueGeneral) return 10;
                    if (c.Id == CardId.ReignBeauxOverkingOfDarkWorld) return 20;
                    if (c.Id == CardId.GraphaDragonLordOfDarkWorld) return 30;
                    if (c.Id == CardId.GentaGatemanOfDarkWorld) return 40;
                    if (c.Id == CardId.LucentNetherlordOfDarkWorld) return 50;
                    if (c.Id == CardId.BrowwHuntsmanOfDarkWorld) return 60;
                    return 100;
                }).ToList();
                return sendCandidates.Take(min).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override IList<ClientCard> OnSelectFusionMaterial(IList<ClientCard> cards, int min, int max)
        {
            var result = new List<ClientCard>();

            var nonAceMats = cards.Where(c => c != null && !IsAceCard(c)).OrderBy(c => {
                if (c.Location == CardLocation.Hand && c.HasSetcode(0x6)) return 1; // Discarding DW triggers effects!
                if (c.Location == CardLocation.Grave) return 2;
                if (c.Location == CardLocation.Hand) return 3;
                return 4;
            }).ToList();

            foreach (var c in nonAceMats)
            {
                result.Add(c);
                if (result.Count >= max) break;
            }

            if (result.Count >= min) return result;

            var fallbackMats = cards.OrderBy(c => {
                if (IsAceCard(c)) return 900;
                if (c.Location == CardLocation.Grave) return 10;
                if (c.Location == CardLocation.Hand) return 20;
                return 30;
            }).ToList();

            return fallbackMats.Take(max).ToList();
        }

        protected override bool IsBoardStrongEnough()
        {
            int disruption = Bot.GetMonsters().Count(m => m != null && m.IsFaceup() && IsAceCard(m));
            if (disruption >= 2) return true;
            int atk = Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).Sum(m => m.Attack);
            if (atk >= 8000) return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (IsBoardStrongEnough()) return true;
            return base.ShouldStopExtending();
        }
    }
}
