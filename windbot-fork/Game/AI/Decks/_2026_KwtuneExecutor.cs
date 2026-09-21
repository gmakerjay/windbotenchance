using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    // ====================================================================================================
    // CARD AUDIT — WCParisKewlTune & 2026_Kwtune (Kewl Tune Archetype)
    // 100% verified against cards.cdb and WCParisKewlTune.ydk
    // ====================================================================================================
    // | Card Name              | Type           | Lv | ATK  | DEF  | HOPT | Hand-Sync | Key Interaction                                |
    // |------------------------|----------------|----|------|------|------|-----------|------------------------------------------------|
    // | Kewl Tune Cue          | Tuner/Light    | 3  | 900  | 1900 | Yes  | Yes       | NS: SS Tuner from Deck. GY: excavate 2 banish 1 |
    // | Kewl Tune Rotary       | Tuner/Light    | 1  | 100  | 800  | Yes  | Yes       | Hand: extra NS Tuner. GY: search KT S/T / spin |
    // | Kewl Tune Reco         | Tuner/Fire     | 3  | 1500 | 800  | Yes  | Yes       | NS/SS: search non-Lv3 KT. GY: destroy opp S/T  |
    // | Kewl Tune Clip         | Tuner/Dark     | 2  | 800  | 1000 | Yes  | Yes       | Quick: SS on opp turn + Synchro. GY: banish ED |
    // | Kewl Tune Mix          | Tuner/Water    | 2  | 100  | 2000 | Yes  | Yes       | NS/SS: search non-Lv2 KT. GY: destroy monster  |
    // | Starjunk Synchron      | Tuner/Dark     | 3  | 1300 | 500  | Yes  | No        | SS from hand if Warrior/Synchron controlled    |
    // | Jet Synchron           | Tuner/Fire     | 1  | 500  | 0    | Yes  | No        | GY: discard 1 to SS this card                  |
    // | Fidraulis Harmonia     | Tuner/Dark     | 7  | 2500 | 2000 | No   | No        | Hand Quick: reveal 5 ED Synchros -> SS + dump  |
    // | JJ "Kewl Tune"         | Field Spell    | 0  | 0    | 0    | Yes  | No        | Extra NS + tribute Tuner to search/SS KT       |
    // | Kewl Tune Synchro      | Quick Spell    | 0  | 0    | 0    | 2x   | No        | Search KT card + immediate Synchro Summon      |
    // | Duelist Genesis        | Normal Spell   | 0  | 0    | 0    | Yes  | No        | If Tuner on field/GY: search "Synchro" S/T     |
    // | Synchro Overtake       | Normal Spell   | 0  | 0    | 0    | Yes  | No        | Reveal ED Synchro -> SS named material (Mix/Rec)|
    // | Triple Tactics Talent  | Normal Spell   | 0  | 0    | 0    | Yes  | No        | Draw 2 / Steal monster / Hand rip              |
    // | Called by the Grave    | Quick Spell    | 0  | 0    | 0    | No   | No        | Banish & negate monster in opp GY              |
    // | Mannadium Reframing    | Counter Trap   | 0  | 0    | 0    | Yes  | No        | Control Synchro: negate & destroy (Visas link) |
    // | Synchro Emergency      | Normal Trap    | 0  | 0    | 0    | Yes  | No        | Hand activatable going 2nd; GY: Synchro summon |
    // |------------------------|----------------|----|------|------|------|-----------|------------------------------------------------|
    // | Track Maker (Synchro)  | Tuner/Light    | 4  | 0    | 2500 | Yes  | Yes       | On SS: search KT card. GY: bounce opp card     |
    // | Kewl Tune Crackle      | Tuner/Dark     | 5  | 800  | 2000 | Yes  | No        | On SS: banish opp ED. GY: self-revives + banish|
    // | Kewl Tune RS           | Tuner/Dark     | 5  | 1700 | 2400 | Yes  | No        | Quick (MP): banish Tuner in GY -> negate faceup|
    // | Kewl Tune Remix        | Tuner/Dark     | 5  | 1500 | 2000 | Yes  | No        | Quick (opp turn): tribute -> recover 2 + Synchro|
    // | Loudness War (Synchro) | Tuner/Light    | 6  | 0    | 3000 | Yes  | No        | Protects Tuners; Quick: copy GY trigger        |
    // | Malong (Synchro)       | Tuner/Light    | 6  | 2200 | 1000 | Yes  | No        | GY: bounce face-up opp card (Fidraulis synergy)|
    // | Zalen the Shackled     | Tuner/Dark     | 7  | 2800 | 2100 | Yes  | No        | Quick: negate & destroy chain link 1 or 2      |
    // | Wind Pegasus @Ignister | Non-Tuner/Wind | 7  | 2300 | 1500 | Yes  | No        | S/T destruction; GY: spin opp card when card pop|
    // | Visas Amritara         | Tuner/Light    | 8  | 2500 | 2100 | Yes  | No        | On SS: search Mannadium Reframing!             |
    // | Enigmaster Packbit     | Non-Tuner/Water| 8  | 2900 | 2500 | Yes  | No        | Place monster in S/T zone as Continuous Trap   |
    // | Kewl Tune Back 2 Back  | Tuner/Dark     | 10 | 3200 | 0    | 2x   | No        | Tuners attack twice; Quick: SS Tuner + Synchro |
    // | Despian Luluwalilith   | Non-Tuner/Light| 12 | 2500 | 2500 | Yes  | No        | ED exit: +500 ATK all + negate 1; End Phase SS |
    // ====================================================================================================

    [Deck("2026_Kwtune", "2026_Kwtune")]
    public class _2026_KwtuneExecutor : ModernExecutor
    {
        public class CardId
        {
            // --- MAIN DECK ARCHETYPE MONSTERS ---
            public const int KewlTuneReco = 89392810;
            public const int KewlTuneCue = 16387555;
            public const int KewlTuneRotary = 17209452;
            public const int KewlTuneClip = 43904702;
            public const int KewlTuneMix = 16509007;
            public const int StarjunkSynchron = 13021682;
            public const int JetSynchron = 9742784;
            public const int FidraulisHarmonia = 70088809;

            // --- STANDARD HAND TRAPS ---
            public const int AshBlossom = 14558128;
            public const int GhostOgre = 59438930;
            public const int GhostBelle = 73642296;
            public const int DrollAndLockBird = 94145021;
            public const int EffectVeiler = 97268402;
            public const int InfiniteImpermanence = 10045474;

            // --- MAIN DECK SPELLS & TRAPS ---
            public const int JJKewlTune = 14442329;
            public const int KewlTuneSynchro = 78058681;
            public const int DuelistGenesis = 97474300;
            public const int SynchroOvertake = 99243014;
            public const int TripleTacticsTalent = 25311006;
            public const int CalledByTheGrave = 24224830;
            public const int SynchroEmergency = 49415281;
            public const int MannadiumReframing = 18158393;
            public const int HarpiesFeatherDuster = 18144507;
            public const int LightningStorm = 14532163;
            public const int PotOfProsperity = 84211599;

            // --- EXTRA DECK SYNCHROS ---
            public const int KewlTuneTrackMaker = 42781164;
            public const int KewlTuneCrackle = 39576656;
            public const int KewlTuneRS = 15665977;
            public const int KewlTuneRemix = 88170262;
            public const int KewlTuneLoudnessWar = 41069676;
            public const int GoldenCloudBeastMalong = 93125329;
            public const int ZalenTheShackledDragon = 4891376;
            public const int WindPegasusIgnister = 98506199;
            public const int VisasAmritara = 821049;
            public const int EnigmasterPackbit = 72444406;
            public const int KewlTuneBack2Back = 65961304;
            public const int DespianLuluwalilith = 53971455;
            public const int ChaosAngel = 22850702; // legacy support for 2026_Kwtune

            // --- SIDE DECK / LEGACY CARDS ---
            public const int MulcharmyFuwalos = 42141493;
            public const int MulcharmyPurulia = 84192580;
            public const int PotOfSloth = 98476659;
            public const int IllusionGate = 33017964;
        }

        // --- Fast Archetype Lookups ---
        private static readonly int[] KewlTuneMainMonsters = {
            CardId.KewlTuneReco, CardId.KewlTuneCue, CardId.KewlTuneRotary,
            CardId.KewlTuneClip, CardId.KewlTuneMix
        };

        private static readonly int[] KewlTuneSynchros = {
            CardId.KewlTuneTrackMaker, CardId.KewlTuneCrackle, CardId.KewlTuneRemix,
            CardId.KewlTuneRS, CardId.KewlTuneLoudnessWar, CardId.KewlTuneBack2Back
        };

        private static readonly int[] HandTrapIds = {
            CardId.AshBlossom, CardId.GhostOgre, CardId.GhostBelle,
            CardId.EffectVeiler, CardId.DrollAndLockBird, CardId.MulcharmyFuwalos, CardId.MulcharmyPurulia
        };

        private static readonly int[] AceCardIds = {
            CardId.KewlTuneBack2Back,
            CardId.KewlTuneLoudnessWar,
            CardId.VisasAmritara,
            CardId.ZalenTheShackledDragon,
            CardId.ChaosAngel,
            CardId.DespianLuluwalilith
        };

        public override bool IsAceCard(ClientCard card) => card != null && AceCardIds.Any(id => card.IsCode(id));
        private bool IsHandTrap(ClientCard c) => c != null && (HandTrapIds.Contains(c.Id) || CardIntelligence.IsHandtrap(c.Id));
        private bool IsKewlTuneMonster(ClientCard c) => c != null && KewlTuneMainMonsters.Contains(c.Id);
        private bool IsKewlTuneSynchro(ClientCard c) => c != null && KewlTuneSynchros.Contains(c.Id);

        // ====================================================================================================
        //  OPT FLAGS — Tracked per turn
        // ====================================================================================================
        private bool _potUsed;
        private bool _synchroOvertakeUsed;
        private bool _fieldUsed;
        private bool _rotaryHandUsed;
        private bool _genesisUsed;
        private bool _cueNSUsed;
        private bool _recoSummonSearchUsed;
        private bool _mixSummonSearchUsed;
        private bool _loudnessWarEffectUsed;
        private bool _rsNegateUsed;
        private bool _remixTributeUsed;
        private int _back2BackUsedCount;
        private bool _trackMakerSearchUsed;
        private int _kewlTuneSynchroUseCount;
        private bool _tunerOnlyLock;
        private bool _hasOpponentActivatedMonsterEffect;
        private bool _jetSynchronUsed;

        // ====================================================================================================
        //  PRIORITIES
        // ====================================================================================================

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (IsHandTrap(c)) return 850;
            if (c.Id == CardId.FidraulisHarmonia) return 700;

            // Main Deck Tuners: prefer using those with beneficial GY triggers first
            switch (c.Id)
            {
                case CardId.KewlTuneClip: return 10;
                case CardId.KewlTuneRotary: return 15;
                case CardId.KewlTuneMix: return 20;
                case CardId.KewlTuneReco: return 25;
                case CardId.JetSynchron: return 28;
                case CardId.StarjunkSynchron: return 29;
                case CardId.KewlTuneCue: return 30;
                case CardId.GoldenCloudBeastMalong: return 45;
                case CardId.KewlTuneTrackMaker: return 50;
                case CardId.KewlTuneCrackle: return 55;
                case CardId.KewlTuneRemix: return 60;
                case CardId.KewlTuneRS: return 65;
                case CardId.WindPegasusIgnister: return 70;
                case CardId.KewlTuneLoudnessWar: return 75;
                default: return 100;
            }
        }

        private int GetNormalSummonPriority(ClientCard c)
        {
            if (c == null) return 99;
            switch (c.Id)
            {
                case CardId.KewlTuneCue: return 1;     // NS -> SS from deck (Main Starter)
                case CardId.KewlTuneMix: return 2;     // NS -> Search non-Lv2 KT
                case CardId.KewlTuneReco: return 3;    // NS -> Search non-Lv3 KT
                case CardId.KewlTuneRotary: return 4;  // NS -> Lv1 Tuner fodder
                case CardId.JetSynchron: return 5;
                case CardId.KewlTuneClip: return 6;
                default: return 99;
            }
        }

        private int GetHandSyncPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsHandTrap(c)) return 950; // NEVER discard handtraps for Hand-Sync
            if (c.Id == CardId.FidraulisHarmonia) return 900;

            switch (c.Id)
            {
                case CardId.KewlTuneClip: return 10;   // Banish opp Extra Deck
                case CardId.KewlTuneRotary: return 15; // Spin opp GY / search S/T
                case CardId.KewlTuneMix: return 20;    // Destroy opp monster
                case CardId.KewlTuneReco: return 25;   // Destroy opp S/T
                case CardId.JetSynchron: return 28;    // Can self-revive later
                case CardId.KewlTuneCue: return 30;    // Excavate & banish
                default: return 100;
            }
        }

        // ====================================================================================================
        //  CONSTRUCTOR — Registered in Strategic Execution Order
        // ====================================================================================================
        public _2026_KwtuneExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // Combo Router: Register Core Routing Lines
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Cue-TrackMaker-Setup",
                RequiredCards = new List<int> { CardId.KewlTuneCue },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.KewlTuneCue, ActionType = ExecutorType.Summon, Description = "Normal Summon Cue" },
                    new() { CardId = CardId.KewlTuneCue, ActionType = ExecutorType.Activate, Description = "Cue SS Tuner from Deck" },
                    new() { CardId = CardId.KewlTuneTrackMaker, ActionType = ExecutorType.SpSummon, Description = "Synchro TrackMaker" },
                    new() { CardId = CardId.KewlTuneTrackMaker, ActionType = ExecutorType.Activate, Description = "TrackMaker Search" }
                },
                EndBoardScore = 90
            });

            // Bait Planner
            BaitPlanner.RegisterComboStarters(CardId.KewlTuneCue, CardId.KewlTuneSynchro, CardId.KewlTuneTrackMaker);
            BaitPlanner.RegisterBaitCards(CardId.PotOfProsperity, CardId.DuelistGenesis, CardId.KewlTuneReco);

            // Chain Advisor
            ChainAdvisor.RegisterHighValueTargets(CardId.KewlTuneCue, CardId.EffectVeiler, CardId.AshBlossom, CardId.MannadiumReframing);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 1: Handtraps, Negations & Counter Traps (Highest Priority)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.MannadiumReframing, MannadiumReframingEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelle, GhostBelleEffect);
            AddExecutor(ExecutorType.Activate, CardId.GhostOgre, GhostOgreEffect);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, EffectVeilerEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyPurulia, MulcharmyPuruliaEffect);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, ImpermanenceEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: Quick Effects on Opponent's Turn (Disruption Bosses)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.FidraulisHarmonia, FidraulisHarmoniaEffect);
            AddExecutor(ExecutorType.Activate, CardId.ZalenTheShackledDragon, ZalenEffect);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneBack2Back, Back2BackEffect);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneLoudnessWar, LoudnessWarEffect);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneRS, RSEffect);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneRemix, RemixEffect);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneClip, KewlTuneClipEffect);
            AddExecutor(ExecutorType.Activate, CardId.DespianLuluwalilith, DespianLuluwalilithEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: Board Breakers (Going Second Sweepers)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, HarpiesFeatherDusterEffect);
            AddExecutor(ExecutorType.Activate, CardId.LightningStorm, LightningStormEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: Search Spells & Hand Correction
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentEffect);
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, PotOfProsperityEffect);
            AddExecutor(ExecutorType.Activate, CardId.DuelistGenesis, DuelistGenesisEffect);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneSynchro, KewlTuneSynchroEffect);
            AddExecutor(ExecutorType.Activate, CardId.SynchroOvertake, SynchroOvertakeEffect);
            AddExecutor(ExecutorType.Activate, CardId.JJKewlTune, JJKewlTuneEffect);
            AddExecutor(ExecutorType.Activate, CardId.SynchroEmergency, SynchroEmergencyEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 5: Monster Effects (On-Summon & GY Triggers)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneCue, KewlTuneCueEffect);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneReco, KewlTuneRecoEffect);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneMix, KewlTuneMixEffect);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneRotary, KewlTuneRotaryEffect);
            AddExecutor(ExecutorType.Activate, CardId.GoldenCloudBeastMalong, MalongEffect);
            AddExecutor(ExecutorType.Activate, CardId.WindPegasusIgnister, WindPegasusEffect);
            AddExecutor(ExecutorType.Activate, CardId.EnigmasterPackbit, PackbitEffect);
            AddExecutor(ExecutorType.Activate, CardId.StarjunkSynchron, StarjunkSynchronEffect);
            AddExecutor(ExecutorType.Activate, CardId.JetSynchron, JetSynchronEffect);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneTrackMaker, TrackMakerActivateEffect);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneCrackle, CrackleEffect);
            AddExecutor(ExecutorType.Activate, CardId.VisasAmritara, VisasAmritaraEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 6: Extra Deck Synchro Summons (Strategic Climbing)
            // ═══════════════════════════════════════════════════════════════
            // Step 1: Bridge Synchro (Level 4 Track Maker)
            AddExecutor(ExecutorType.SpSummon, CardId.KewlTuneTrackMaker, TrackMakerSpSummon);

            // Step 2: Level 5 Synchros (RS for negate, Crackle for ED strip, Remix for ladder)
            AddExecutor(ExecutorType.SpSummon, CardId.KewlTuneRS, RSSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.KewlTuneCrackle, CrackleSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.KewlTuneRemix, RemixSpSummon);

            // Step 3: Level 6 Synchros (Loudness War wall/protection, Malong bounce)
            AddExecutor(ExecutorType.SpSummon, CardId.KewlTuneLoudnessWar, LoudnessWarSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.GoldenCloudBeastMalong, MalongSpSummon);

            // Step 4: Level 7 Synchros (Zalen omni-negate, Wind Pegasus S/T clear)
            AddExecutor(ExecutorType.SpSummon, CardId.ZalenTheShackledDragon, ZalenSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.WindPegasusIgnister, WindPegasusSpSummon);

            // Step 5: Level 8 Synchros (Visas Amritara searches Mannadium Reframing!)
            AddExecutor(ExecutorType.SpSummon, CardId.VisasAmritara, VisasAmritaraSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.EnigmasterPackbit, PackbitSpSummon);

            // Step 6: Boss Synchros (Level 10 Back 2 Back, Chaos Angel)
            AddExecutor(ExecutorType.SpSummon, CardId.KewlTuneBack2Back, Back2BackSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ChaosAngel, ChaosAngelSpSummon);

            // Step 7: Level 12 Synchro (Despian Luluwalilith)
            AddExecutor(ExecutorType.SpSummon, CardId.DespianLuluwalilith, LuluwalilithSpSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 7: Normal Summons (Combo Starters)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Summon, CardId.KewlTuneCue, NormalSummonCue);
            AddExecutor(ExecutorType.Summon, CardId.KewlTuneMix, NormalSummonMix);
            AddExecutor(ExecutorType.Summon, CardId.KewlTuneReco, NormalSummonReco);
            AddExecutor(ExecutorType.Summon, CardId.KewlTuneRotary, NormalSummonRotary);
            AddExecutor(ExecutorType.Summon, CardId.JetSynchron, NormalSummonJet);
            AddExecutor(ExecutorType.Summon, CardId.KewlTuneClip, NormalSummonClip);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 8: Set Spells/Traps & Safe Repos
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, CardId.MannadiumReframing);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave);
            AddExecutor(ExecutorType.SpellSet, CardId.SynchroEmergency);
            AddExecutor(ExecutorType.Repos, MonsterReposLogic);
        }

        // ====================================================================================================
        //  TURN LIFECYCLE
        // ====================================================================================================

        public override bool OnSelectHand() => true; // Always choose to go first

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _potUsed = false;
            _synchroOvertakeUsed = false;
            _fieldUsed = false;
            _rotaryHandUsed = false;
            _genesisUsed = false;
            _cueNSUsed = false;
            _recoSummonSearchUsed = false;
            _mixSummonSearchUsed = false;
            _loudnessWarEffectUsed = false;
            _rsNegateUsed = false;
            _remixTributeUsed = false;
            _back2BackUsedCount = 0;
            _trackMakerSearchUsed = false;
            _kewlTuneSynchroUseCount = 0;
            _tunerOnlyLock = false;
            _hasOpponentActivatedMonsterEffect = false;
            _jetSynchronUsed = false;
        }

        public override void OnChaining(int player, ClientCard card)
        {
            if (player == 1 && card != null && card.IsMonster() && Duel.Player == 0)
                _hasOpponentActivatedMonsterEffect = true;
            base.OnChaining(player, card);
        }

        // ====================================================================================================
        //  STRATEGIC EVALUATION HELPERS
        // ====================================================================================================

        protected override bool CanDealLethal()
        {
            if (Scorer != null && Scorer.HasLethal()) return true;

            int total = 0;
            bool hasBack2Back = Bot.HasInMonstersZone(CardId.KewlTuneBack2Back);

            foreach (var c in Bot.MonsterZone)
            {
                if (c == null || !c.IsFaceup() || !c.IsAttack()) continue;
                total += c.Attack;
                // Back2Back grants all Tuners a second attack
                if (hasBack2Back && c.HasType(CardType.Tuner))
                    total += c.Attack;
            }
            return total >= Enemy.LifePoints;
        }

        private bool OpponentHasActiveThreat()
        {
            if (Util.GetProblematicEnemyMonster() != null) return true;
            if (CardIntelligence.OpponentHasActiveNegator(Enemy)) return true;
            if (CardIntelligence.IsSpecialSummonBlocked(Enemy)) return true;
            return Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && (c.Attack >= 2400 || CardIntelligence.IsFloodgateMonster(c.Id) || CardIntelligence.IsKnownNegator(c.Id)));
        }

        private ClientCard GetHighestPriorityEnemyTarget(bool requireFaceup = true, bool spellTrapOnly = false)
        {
            if (spellTrapOnly)
            {
                // Look for floodgate spells/traps first
                var floodgate = Enemy.SpellZone.FirstOrDefault(s => s != null && s.IsFaceup() && CardIntelligence.IsFloodgateSpellTrap(s.Id));
                if (floodgate != null) return floodgate;

                var problemSpell = Util.GetProblematicEnemySpell();
                if (problemSpell != null) return problemSpell;

                return Enemy.SpellZone.FirstOrDefault(s => s != null && s.IsFacedown())
                    ?? Enemy.SpellZone.FirstOrDefault(s => s != null);
            }

            // Monster / general target
            var negator = Enemy.MonsterZone.FirstOrDefault(m => m != null && m.IsFaceup() && !m.IsDisabled() && CardIntelligence.IsKnownNegator(m.Id));
            if (negator != null && !CardIntelligence.IsTargetImmune(negator)) return negator;

            var floodgateMon = Enemy.MonsterZone.FirstOrDefault(m => m != null && m.IsFaceup() && !m.IsDisabled() && CardIntelligence.IsFloodgateMonster(m.Id));
            if (floodgateMon != null && !CardIntelligence.IsTargetImmune(floodgateMon)) return floodgateMon;

            var problem = Util.GetProblematicEnemyMonster(0, true);
            if (problem != null && !CardIntelligence.IsTargetImmune(problem)) return problem;

            return Enemy.MonsterZone.Where(m => m != null && m.IsFaceup() && !CardIntelligence.IsTargetImmune(m))
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();
        }

        // ====================================================================================================
        //  TIER 1: HAND TRAPS & COUNTER TRAPS
        // ====================================================================================================

        private bool MannadiumReframingEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;
            // Requires controlling a Synchro monster
            if (!Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Synchro))) return false;
            return true;
        }

        private bool CalledByTheGraveEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;
            ClientCard last = Util.GetLastChainCard();

            // Opponent activating monster handtrap -> banish it from their GY
            if (last != null && last.IsMonster() && last.Location == CardLocation.Hand)
            {
                AI.SelectCard(last.Id);
                return true;
            }

            // Opponent activating effect from GY or chaining on field
            if (last != null && last.IsMonster())
            {
                var gyTarget = Enemy.Graveyard.FirstOrDefault(c => c != null && c.Id == last.Id);
                if (gyTarget != null)
                {
                    AI.SelectCard(gyTarget);
                    return true;
                }
            }

            // Or target any key threat in opponent GY
            var threat = Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && CardIntelligence.IsKnownNegator(c.Id));
            if (threat != null)
            {
                AI.SelectCard(threat);
                return true;
            }

            return false;
        }

        private bool AshBlossomEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool GhostBelleEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;
            return DefaultGhostBelleAndHauntedMansion();
        }

        private bool GhostOgreEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;
            ClientCard last = Util.GetLastChainCard();
            if (last == null || last.Controller != 1 || !last.IsFaceup()) return false;

            if (last.Location == CardLocation.MonsterZone) return true;
            if (last.Location == CardLocation.SpellZone && (last.HasType(CardType.Continuous) || last.HasType(CardType.Field) || last.HasType(CardType.Equip)))
                return true;

            return false;
        }

        private bool EffectVeilerEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.Player != 1 || Duel.LastChainPlayer != 1) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;

            ClientCard target = GetHighestPriorityEnemyTarget();
            if (target != null && !target.IsDisabled())
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool DrollEffect()
        {
            return Duel.Player == 1 && Duel.LastChainPlayer == 1;
        }

        private bool MulcharmyFuwalosEffect()
        {
            return Duel.Player == 1 && Bot.GetMonsterCount() == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2);
        }

        private bool MulcharmyPuruliaEffect()
        {
            return Duel.Player == 1 && Bot.GetMonsterCount() == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2);
        }

        private bool ImpermanenceEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;

            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                var target = GetHighestPriorityEnemyTarget();
                if (target != null && !target.IsDisabled())
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return DefaultInfiniteImpermanence();
        }

        // ====================================================================================================
        //  TIER 2: QUICK EFFECTS (Opponent Turn & Disruption)
        // ====================================================================================================

        private bool FidraulisHarmoniaEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (Duel.Player != 1) return false;
            if (LastChainCard == null || LastChainCard.Controller != 1 || !LastChainCard.IsMonster()) return false;

            // Needs at least 1 Synchro in Extra Deck to reveal
            int edSynchros = Bot.ExtraDeck.Count(c => c != null && c.HasType(CardType.Synchro));
            return edSynchros >= 1;
        }

        private bool ZalenEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (Duel.LastChainPlayer != 1) return false;
            if (Duel.CurrentChain.Count < 1) return false;
            return true;
        }

        private bool Back2BackEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (_back2BackUsedCount >= 2) return false;
            if (Duel.LastChainPlayer != 1) return false;

            ClientCard last = Util.GetLastChainCard();
            if (last == null || !last.IsMonster()) return false;

            bool hasTarget = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.HasType(CardType.Tuner) && c.Level != 10 && c.IsCanRevive())
                          || Bot.Banished.Any(c => c != null && c.IsMonster() && c.HasType(CardType.Tuner) && c.Level != 10);
            if (!hasTarget) return false;

            _back2BackUsedCount++;
            return true;
        }

        private bool LoudnessWarEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (_loudnessWarEffectUsed) return false;
            if (Duel.LastChainPlayer != 1) return false;

            bool hasGYTarget = Bot.Graveyard.Any(c => c != null && IsKewlTuneMonster(c));
            if (!hasGYTarget) return false;

            _loudnessWarEffectUsed = true;
            return true;
        }

        private bool RSEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (_rsNegateUsed) return false;

            bool hasGYTuner = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.HasType(CardType.Tuner));
            if (!hasGYTuner) return false;

            ClientCard target = GetHighestPriorityEnemyTarget();
            if (target == null)
                target = Enemy.SpellZone.FirstOrDefault(c => c != null && c.IsFaceup());
            if (target == null) return false;

            AI.SelectCard(target);
            _rsNegateUsed = true;
            return true;
        }

        private bool RemixEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (_remixTributeUsed) return false;
            if (Duel.Player != 1) return false;

            int gyNonSynchroTuners = Bot.Graveyard.Count(c => c != null && c.IsMonster() && c.HasType(CardType.Tuner) && !c.HasType(CardType.Synchro));
            if (gyNonSynchroTuners < 2) return false;

            _remixTributeUsed = true;
            return true;
        }

        private bool KewlTuneClipEffect()
        {
            return Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2);
        }

        private bool DespianLuluwalilithEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                var target = GetHighestPriorityEnemyTarget();
                if (target != null) AI.SelectCard(target);
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                return true; // End Phase trigger
            }
            return false;
        }

        // ====================================================================================================
        //  TIER 3: BOARD BREAKERS
        // ====================================================================================================

        private bool HarpiesFeatherDusterEffect()
        {
            return Enemy.GetSpellCount() > 0;
        }

        private bool LightningStormEffect()
        {
            int faceupBot = Bot.MonsterZone.Count(c => c != null && c.IsFaceup()) + Bot.SpellZone.Count(c => c != null && c.IsFaceup());
            if (faceupBot > 0) return false;

            if (Enemy.GetSpellCount() >= 2)
            {
                AI.SelectOption(1); // destroy S/T
                return true;
            }
            if (Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsAttack()))
            {
                AI.SelectOption(0); // destroy attack monsters
                return true;
            }
            if (Enemy.GetSpellCount() >= 1)
            {
                AI.SelectOption(1);
                return true;
            }
            return false;
        }

        // ====================================================================================================
        //  TIER 4: SEARCH & ENABLER SPELLS
        // ====================================================================================================

        private bool TripleTacticsTalentEffect()
        {
            if (Duel.Player != 0 || !_hasOpponentActivatedMonsterEffect) return false;

            if (Bot.GetHandCount() <= 2)
            {
                AI.SelectOption(0); // Draw 2
                return true;
            }
            if (Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Attack >= 2500 && !CardIntelligence.IsTargetImmune(c)))
            {
                AI.SelectOption(1); // Take control
                return true;
            }
            AI.SelectOption(2); // Hand rip
            return true;
        }

        private bool PotOfProsperityEffect()
        {
            if (_potUsed || Bot.ExtraDeck.Count < 3) return false;
            _potUsed = true;
            return true;
        }

        private bool DuelistGenesisEffect()
        {
            if (_genesisUsed) return false;
            bool hasTuner = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Tuner))
                         || Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.HasType(CardType.Tuner));
            if (!hasTuner) return false;

            _genesisUsed = true;
            return true;
        }

        private bool KewlTuneSynchroEffect()
        {
            if (_kewlTuneSynchroUseCount >= 2) return false;
            _kewlTuneSynchroUseCount++;
            _tunerOnlyLock = true;
            return true;
        }

        private bool SynchroOvertakeEffect()
        {
            if (_synchroOvertakeUsed) return false;
            _synchroOvertakeUsed = true;
            return true;
        }

        private bool JJKewlTuneEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Play field spell
                return !Bot.HasInSpellZone(CardId.JJKewlTune);
            }
            if (Card.Location == CardLocation.SpellZone)
            {
                if (_fieldUsed) return false;
                // Tribute 1 Tuner to search or SS 1 Kewl Tune monster from Deck
                bool hasFodder = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Tuner) && !IsAceCard(c));
                if (!hasFodder) return false;

                _fieldUsed = true;
                _tunerOnlyLock = true;
                return true;
            }
            return false;
        }

        private bool SynchroEmergencyEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Only activatable from hand if only opponent controls monster
                if (Bot.GetMonsterCount() > 0 || Enemy.GetMonsterCount() == 0) return false;
                return Bot.Hand.Any(c => c != null && c.IsMonster() && c != Card);
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Banish to immediately Synchro Summon
                return true;
            }
            return false;
        }

        // ====================================================================================================
        //  TIER 5: MONSTER EFFECTS
        // ====================================================================================================

        private bool KewlTuneCueEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_cueNSUsed) return false;
                _cueNSUsed = true;
                _tunerOnlyLock = true;
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                return true; // Excavate 2 & banish 1
            }
            return false;
        }

        private bool KewlTuneRecoEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_recoSummonSearchUsed) return false;
                _recoSummonSearchUsed = true;
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                return Enemy.GetSpellCount() > 0;
            }
            return false;
        }

        private bool KewlTuneMixEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_mixSummonSearchUsed) return false;
                _mixSummonSearchUsed = true;
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                return Enemy.GetMonsterCount() > 0;
            }
            return false;
        }

        private bool KewlTuneRotaryEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_rotaryHandUsed) return false;
                bool hasOtherTuner = Bot.Hand.Any(c => c != null && c != Card && c.HasType(CardType.Tuner) && !IsHandTrap(c));
                if (!hasOtherTuner) return false;

                _rotaryHandUsed = true;
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }
            return false;
        }

        private bool MalongEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                var target = GetHighestPriorityEnemyTarget();
                if (target != null) AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool WindPegasusEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                return Enemy.GetSpellCount() > 0;
            }
            if (Card.Location == CardLocation.Grave)
            {
                var target = GetHighestPriorityEnemyTarget(false);
                if (target != null) AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool PackbitEffect()
        {
            if (Card.Location == CardLocation.MonsterZone || Card.Location == CardLocation.Grave)
            {
                return Bot.GetHandCount() > 0;
            }
            if (Card.Location == CardLocation.SpellZone)
            {
                return true; // Special Summon from S/T zone
            }
            return false;
        }

        private bool StarjunkSynchronEffect()
        {
            return Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && (c.IsCode(CardId.KewlTuneMix) || c.HasRace(CardRace.Warrior)));
        }

        private bool JetSynchronEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                if (_jetSynchronUsed || Bot.GetHandCount() <= 0) return false;
                _jetSynchronUsed = true;
                return true;
            }
            return true;
        }

        private bool TrackMakerActivateEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_trackMakerSearchUsed) return false;
                _trackMakerSearchUsed = true;
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Bounce 1 opp card
                var target = GetHighestPriorityEnemyTarget(false);
                if (target != null) AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool CrackleEffect()
        {
            if (Card.Location == CardLocation.MonsterZone) return true;
            if (Card.Location == CardLocation.Grave) return true; // Self-revive from GY
            return false;
        }

        private bool VisasAmritaraEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // On-Summon search Mannadium Reframing
                return true;
            }
            return false;
        }

        // ====================================================================================================
        //  TIER 6: SYNCHRO SUMMONS
        // ====================================================================================================

        private bool TrackMakerSpSummon() => true;
        private bool RSSpSummon() => true;
        private bool CrackleSpSummon() => true;
        private bool RemixSpSummon() => true;
        private bool LoudnessWarSpSummon() => true;
        private bool MalongSpSummon() => true;
        private bool ZalenSpSummon() => true; // Zalen IS a Tuner! Never block with _tunerOnlyLock!
        private bool WindPegasusSpSummon() => !_tunerOnlyLock;
        private bool VisasAmritaraSpSummon() => true;
        private bool PackbitSpSummon() => !_tunerOnlyLock;
        private bool Back2BackSpSummon() => true;
        private bool ChaosAngelSpSummon() => !_tunerOnlyLock && OpponentHasActiveThreat();
        private bool LuluwalilithSpSummon() => !_tunerOnlyLock;

        // ====================================================================================================
        //  TIER 7: NORMAL SUMMONS
        // ====================================================================================================

        private bool NormalSummonCue() => true;
        private bool NormalSummonMix() => true;
        private bool NormalSummonReco() => true;
        private bool NormalSummonRotary() => true;
        private bool NormalSummonJet() => true;
        private bool NormalSummonClip() => true;

        // ====================================================================================================
        //  CARD SELECTION LOGIC (Hint Handlers & Targeted Interactions)
        // ====================================================================================================

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // ───── Hint 512: Synchro Material Selection (Including Hand-Sync from Hand) ─────
            if (hint == 512)
            {
                var fieldCards = cards.Where(c => c != null && c.Location == CardLocation.MonsterZone).ToList();
                var handCards = cards.Where(c => c != null && c.Location == CardLocation.Hand).ToList();

                // 1. Preferred field materials (Tuner fodder with beneficial GY triggers first, protect Aces)
                var preferredField = fieldCards.Where(c => !IsAceCard(c))
                    .OrderBy(c => GetMaterialPriority(c))
                    .ToList();

                // 2. Hand-Sync: at most 1 KewlTune monster from hand (never handtraps/aces)
                var preferredHand = handCards.Where(c => !IsHandTrap(c) && !IsAceCard(c) && IsKewlTuneMonster(c))
                    .OrderBy(c => GetHandSyncPriority(c))
                    .Take(1)
                    .ToList();

                // 3. Combine: field materials first, then at most 1 hand-sync material
                var combined = preferredField.Concat(preferredHand).ToList();

                // If combined is insufficient to satisfy min, add remaining field cards (including Aces if forced)
                if (combined.Count < min)
                {
                    var remainingField = fieldCards.Where(c => !combined.Contains(c))
                        .OrderBy(c => GetMaterialPriority(c));
                    combined.AddRange(remainingField);
                }

                if (combined.Count >= min)
                    return Util.CheckSelectCount(combined, cards, min, max);
            }

            // ───── Hint 508: Send to GY / Dump from Extra Deck or Hand ─────
            if (hint == 508)
            {
                // Fidraulis Harmonia dumping from Extra Deck:
                var extraSynchros = cards.Where(c => c != null && c.Location == CardLocation.Extra).ToList();
                if (extraSynchros.Count > 0)
                {
                    var dumpTarget = extraSynchros.OrderByDescending(c =>
                    {
                        if (c.Id == CardId.GoldenCloudBeastMalong) return 100;
                        if (c.Id == CardId.WindPegasusIgnister) return 90;
                        if (c.Id == CardId.DespianLuluwalilith) return 80;
                        if (c.Id == CardId.KewlTuneCrackle) return 70;
                        return 10;
                    }).FirstOrDefault();
                    if (dumpTarget != null) return new List<ClientCard> { dumpTarget };
                }

                // If discarding from hand (e.g. Jet Synchron cost)
                var handPool = cards.Where(c => c != null && c.Location == CardLocation.Hand).ToList();
                if (handPool.Count > 0)
                {
                    var discardTarget = handPool.Where(c => !IsHandTrap(c) && !IsAceCard(c))
                        .OrderBy(c => GetMaterialPriority(c))
                        .FirstOrDefault() ?? handPool.First();
                    return new List<ClientCard> { discardTarget };
                }
            }

            // ───── Hint 502: Destroy Target ─────
            if (hint == 502)
            {
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyCards.Count > 0)
                {
                    var prioritized = enemyCards.OrderByDescending(c =>
                    {
                        if (CardIntelligence.IsFloodgate(c.Id)) return 10000;
                        if (CardIntelligence.IsKnownNegator(c.Id)) return 9000;
                        if (c.IsMonster() && c.IsFaceup() && c.Attack >= 2500) return 8000;
                        if (c.HasType(CardType.Continuous) && c.IsFaceup()) return 7000;
                        if (c.IsMonster() && c.IsFaceup()) return c.Attack;
                        if (c.IsFacedown()) return 500;
                        return 100;
                    }).ToList();
                    return Util.CheckSelectCount(prioritized, cards, min, max);
                }

                // If forced to select our own card (self-pop e.g. Visas Amritara)
                var ourCards = cards.Where(c => c != null && c.Controller == 0).ToList();
                if (ourCards.Count > 0)
                {
                    var prioritized = ourCards.OrderBy(c =>
                    {
                        if (IsAceCard(c)) return 99999;
                        if (c.Id == CardId.GoldenCloudBeastMalong) return 1; // triggers Malong GY bounce!
                        if (c.Id == CardId.KewlTuneCrackle) return 2;       // triggers Crackle GY revive!
                        if (IsKewlTuneMonster(c)) return 10 + GetMaterialPriority(c);
                        return 100;
                    }).ToList();
                    return Util.CheckSelectCount(prioritized, cards, min, max);
                }
            }

            // ───── Hint 504: Banish Target ─────
            if (hint == 504)
            {
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyCards.Count > 0)
                {
                    var prioritized = enemyCards.OrderByDescending(c =>
                    {
                        if (CardIntelligence.IsFloodgate(c.Id)) return 10000;
                        if (CardIntelligence.IsKnownNegator(c.Id)) return 9000;
                        if (c.IsMonster() && c.IsFaceup()) return c.Attack;
                        return 100;
                    }).ToList();
                    return Util.CheckSelectCount(prioritized, cards, min, max);
                }

                // If banishing from our GY for cost (e.g. RS or Loudness War)
                var ourGY = cards.Where(c => c != null && c.Controller == 0 && c.Location == CardLocation.Grave).ToList();
                if (ourGY.Count > 0)
                {
                    var prioritized = ourGY.OrderBy(c => GetMaterialPriority(c)).ToList();
                    return Util.CheckSelectCount(prioritized, cards, min, max);
                }
            }

            // ───── Hint 509: Special Summon Target ─────
            if (hint == 509)
            {
                var candidates = cards.Where(c => c != null && c.IsMonster()).ToList();
                if (candidates.Count > 0)
                {
                    // Prioritize Deck candidates over Hand if Deck options exist
                    bool hasDeck = candidates.Any(c => c.Location == CardLocation.Deck);
                    var pool = hasDeck ? candidates.Where(c => c.Location == CardLocation.Deck).ToList() : candidates;

                    var sorted = pool.OrderByDescending(c =>
                    {
                        if (IsAceCard(c)) return 6000 + c.Attack;
                        if (IsKewlTuneSynchro(c)) return 4000 + c.Attack;
                        if (c.Id == CardId.KewlTuneRotary && Bot.MonsterZone.Any(m => m != null && m.IsCode(CardId.KewlTuneCue))) return 5000; // Cue + Rotary = Lv 4 TrackMaker
                        if (c.Id == CardId.KewlTuneMix) return 3000;
                        if (c.Id == CardId.KewlTuneReco) return 2500;
                        return c.Attack;
                    }).ToList();

                    if (sorted.Count >= min)
                        return Util.CheckSelectCount(sorted, cards, min, max);
                }
            }

            // ───── Hint 506 / 505: Search / Add to Hand ─────
            if (hint == 506 || hint == 505)
            {
                var searchable = cards.Where(c => c != null).ToList();
                if (searchable.Count > 0)
                {
                    var sorted = searchable.OrderByDescending(c =>
                    {
                        if (c.Id == CardId.MannadiumReframing && !Bot.HasInHand(CardId.MannadiumReframing) && !Bot.HasInSpellZone(CardId.MannadiumReframing)) return 200;
                        if (c.Id == CardId.KewlTuneSynchro && !Bot.HasInHand(CardId.KewlTuneSynchro)) return 150;
                        if (c.Id == CardId.JJKewlTune && !Bot.HasInHand(CardId.JJKewlTune) && !Bot.HasInSpellZone(CardId.JJKewlTune)) return 140;
                        if (c.Id == CardId.KewlTuneCue && !Bot.HasInHand(CardId.KewlTuneCue)) return 130;
                        if (c.Id == CardId.KewlTuneReco && !Bot.HasInHand(CardId.KewlTuneReco)) return 120;
                        if (c.Id == CardId.KewlTuneMix && !Bot.HasInHand(CardId.KewlTuneMix)) return 110;
                        if (c.Id == CardId.CalledByTheGrave && !Bot.HasInHand(CardId.CalledByTheGrave)) return 100;
                        return 10;
                    }).ToList();
                    return Util.CheckSelectCount(sorted, cards, min, max);
                }
            }

            // ───── Card-Specific Selections ─────
            if (Card != null)
            {
                // Synchro Overtake: Reveal named material
                if (Card.Id == CardId.SynchroOvertake)
                {
                    var extraSynchros = cards.Where(c => c.Location == CardLocation.Extra).ToList();
                    if (extraSynchros.Count > 0)
                    {
                        ClientCard target = extraSynchros.FirstOrDefault(c => c.Id == CardId.KewlTuneRemix)
                                         ?? extraSynchros.FirstOrDefault(c => c.Id == CardId.KewlTuneRS)
                                         ?? extraSynchros.FirstOrDefault(c => c.Id == CardId.KewlTuneCrackle)
                                         ?? extraSynchros.FirstOrDefault();
                        if (target != null) return new List<ClientCard> { target };
                    }

                    ClientCard monster = cards.FirstOrDefault(c => c.Id == CardId.KewlTuneMix)
                                      ?? cards.FirstOrDefault(c => c.Id == CardId.KewlTuneReco)
                                      ?? cards.FirstOrDefault(c => c.Id == CardId.KewlTuneClip);
                    if (monster != null) return new List<ClientCard> { monster };
                }

                // Fidraulis Harmonia: Reveal 5 Extra Deck Synchros, then dump Malong / WindPegasus / Luluwalilith, or destroy opponent monster
                bool isHarmonia = (Card != null && Card.Id == CardId.FidraulisHarmonia)
                               || (LastChainCard != null && LastChainCard.Id == CardId.FidraulisHarmonia);
                if (isHarmonia)
                {
                    if (cards.All(c => c.Location == CardLocation.Extra))
                    {
                        var preferred = cards.OrderByDescending(c =>
                        {
                            if (c.Id == CardId.GoldenCloudBeastMalong) return 100; // triggers bounce
                            if (c.Id == CardId.WindPegasusIgnister) return 90;     // triggers spin on destroy
                            if (c.Id == CardId.DespianLuluwalilith) return 80;     // triggers End Phase SS
                            if (c.Id == CardId.KewlTuneCrackle) return 70;         // triggers self-revive
                            return 10;
                        }).ToList();

                        if (min == 1 && max == 1)
                        {
                            // Dump 1 Synchro to GY (Hint 508 / Effect 2)
                            return new List<ClientCard> { preferred.First() };
                        }
                        return preferred.Take(max).ToList();
                    }

                    // Destroy 1 monster opponent controls (Hint 502 / Effect 3)
                    var oppMonsters = cards.Where(c => c != null && c.Controller == 1 && c.Location == CardLocation.MonsterZone)
                        .OrderByDescending(c => c.Attack).ToList();
                    if (oppMonsters.Count > 0)
                    {
                        return new List<ClientCard> { oppMonsters.First() };
                    }
                }

                // Visas Amritara: Search Mannadium Reframing
                if (Card.Id == CardId.VisasAmritara)
                {
                    var reframing = cards.FirstOrDefault(c => c.Id == CardId.MannadiumReframing);
                    if (reframing != null) return new List<ClientCard> { reframing };
                }

                // Duelist Genesis: Search Kewl Tune Synchro
                if (Card.Id == CardId.DuelistGenesis)
                {
                    var ktSynchro = cards.FirstOrDefault(c => c.Id == CardId.KewlTuneSynchro);
                    if (ktSynchro != null) return new List<ClientCard> { ktSynchro };

                    var emergency = cards.FirstOrDefault(c => c.Id == CardId.SynchroEmergency);
                    if (emergency != null) return new List<ClientCard> { emergency };
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override IList<ClientCard> OnSelectSynchroMaterial(IList<ClientCard> cards, int sum, int min, int max)
        {
            // Protect handtraps and Ace cards strictly
            var safe = cards.Where(c => c != null && !IsHandTrap(c) && !IsAceCard(c))
                .OrderBy(c => GetMaterialPriority(c))
                .ToList();
            if (safe.Count >= min)
                return Util.CheckSelectCount(safe, cards, min, max);

            var noAce = cards.Where(c => c != null && !IsAceCard(c))
                .OrderBy(c => GetMaterialPriority(c))
                .ToList();
            if (noAce.Count >= min)
                return Util.CheckSelectCount(noAce, cards, min, max);

            return base.OnSelectSynchroMaterial(cards, sum, min, max);
        }

        public override int OnSelectOption(IList<long> options)
        {
            // Rotary GY effect: option 0 = spin opp GY, option 1 = look at hand & search KT S/T
            if (Card != null && Card.IsCode(CardId.KewlTuneRotary) && options.Count >= 2)
            {
                return 1; // search S/T
            }

            // Back2Back: option 0 = add to hand, option 1 = Special Summon
            if (Card != null && Card.IsCode(CardId.KewlTuneBack2Back) && options.Count >= 2)
            {
                return 1; // Special Summon for follow-up Synchro
            }

            // Triple Tactics Talent: option 0 = draw 2, 1 = steal, 2 = rip hand
            if (Card != null && Card.IsCode(CardId.TripleTacticsTalent) && options.Count >= 3)
            {
                if (Bot.GetHandCount() <= 2) return 0;
                if (Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Attack >= 2500)) return 1;
                return 2;
            }

            return 0;
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Low ATK combo pieces in defense
            if ((cardId == CardId.KewlTuneRotary || cardId == CardId.KewlTuneClip || cardId == CardId.KewlTuneTrackMaker || cardId == CardId.KewlTuneLoudnessWar)
                && positions.Contains(CardPosition.FaceUpDefence))
                return CardPosition.FaceUpDefence;

            // Attackers in attack
            if ((cardId == CardId.KewlTuneBack2Back || cardId == CardId.ZalenTheShackledDragon || cardId == CardId.FidraulisHarmonia || cardId == CardId.VisasAmritara)
                && positions.Contains(CardPosition.FaceUpAttack))
                return CardPosition.FaceUpAttack;

            return base.OnSelectPosition(cardId, positions);
        }

        // ====================================================================================================
        //  BATTLE & MONSTER REPOSITION
        // ====================================================================================================

        public override ClientCard OnSelectAttacker(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            // Back2Back attacks first (double attack enabled)
            ClientCard back2back = attackers.FirstOrDefault(c => c != null && c.IsCode(CardId.KewlTuneBack2Back) && !c.Attacked);
            if (back2back != null) return back2back;

            var sorted = attackers.Where(c => c != null && !c.Attacked && c.Attack > 0)
                .OrderByDescending(c => c.Attack).ToList();
            return sorted.Count > 0 ? sorted.First() : base.OnSelectAttacker(attackers, defenders);
        }

        public override BattlePhaseAction OnSelectAttackTarget(ClientCard attacker, IList<ClientCard> defenders)
        {
            if (defenders.Count == 0) return AI.Attack(attacker, null);

            int GetDefVal(ClientCard c) => c == null ? 0 : (c.IsDefense() ? c.Defense : c.Attack);

            var beatable = defenders.Where(d => d != null && attacker.Attack > GetDefVal(d) && !CardIntelligence.IsDangerousBattleTarget(d, attacker))
                .OrderByDescending(d => GetDefVal(d)).ToList();
            if (beatable.Count > 0) return AI.Attack(attacker, beatable.First());

            return null; // Don't suicide
        }

        private bool MonsterReposLogic()
        {
            bool enemyEmpty = Enemy.GetMonsterCount() == 0;

            foreach (ClientCard monster in Bot.GetMonsters())
            {
                if (monster == null || !monster.IsFaceup() || IsAceCard(monster)) continue;

                if (monster.IsAttack())
                {
                    if (!enemyEmpty && monster.Attack <= 1000 && monster.Defense > 0)
                    {
                        bool stronger = Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsAttack() && c.Attack >= monster.Attack);
                        if (stronger) return true;
                    }
                }
                else
                {
                    if (enemyEmpty && monster.Attack > 0) return true;
                    if (monster.Attack >= 2000 && !Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Attack >= monster.Attack))
                        return true;
                }
            }
            return false;
        }
    }

    // ====================================================================================================
    //  WCParisKewlTune EXECUTOR & EXPERT WRAPPERS
    // ====================================================================================================
    [Deck("WCParisKewlTune", "WCParisKewlTune")]
    public class WCParisKewlTuneExecutor : _2026_KwtuneExecutor
    {
        public WCParisKewlTuneExecutor(GameAI ai, Duel duel) : base(ai, duel) { }
    }

    [Deck("Expert_WCParisKewlTune", "WCParisKewlTune")]
    public class ExpertWCParisKewlTuneExecutor : _2026_KwtuneExecutor
    {
        public ExpertWCParisKewlTuneExecutor(GameAI ai, Duel duel) : base(ai, duel) { }
    }

    [Deck("Expert_2026_Kwtune", "2026_Kwtune")]
    public class ExpertKwtuneExecutor : _2026_KwtuneExecutor
    {
        public ExpertKwtuneExecutor(GameAI ai, Duel duel) : base(ai, duel) { }
    }
}
