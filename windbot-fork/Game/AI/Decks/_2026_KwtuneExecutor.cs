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
    // CARD AUDIT โ€” 2026_Kwtune (Kewl Tune)
    // ============================================================
    // | Card Name           | Type   | Lv | ATK  | DEF  | OPT? | HOPT? | Hand-Sync | GY Trigger (as Synchro Material)          |
    // |---------------------|--------|----|------|------|------|-------|-----------|-------------------------------------------|
    // | Kewl Tune Reco      | Tuner  | 3  | 1500 | 800  | Yes  | Yes   | Yes       | Destroy opp S/T                           |
    // | Kewl Tune Cue       | Tuner  | 3  | 900  | 1900 | Yes  | Yes   | Yes       | Excavate 2 from opp deck, banish 1        |
    // | Kewl Tune Rotary    | Tuner  | 1  | 100  | 800  | Yes  | Yes   | Yes       | Search KT spell/trap OR add tuner         |
    // | Kewl Tune Clip      | Tuner  | 1  | 0    | 0    | Yes  | Yes   | Yes       | (Opp turn only) Banish from opp ED        |
    // | Kewl Tune Mix       | Tuner  | 2  | 100  | 2000 | Yes  | Yes   | Yes       | Destroy opp monster                       |
    // | Fidraulis Harmonia  | Tuner  | 7  | 2500 | 2000 | No   | No    | No        | (Not archetype)                           |
    // |---------------------|--------|----|------|------|------|-------|-----------|-------------------------------------------|
    // | TrackMaker (Synchro)| Tuner  | 4  | 0    | 2500 | Yes  | Yes   | Yes       | Bounce 1 opp card                         |
    // | Crackle (Synchro)   | Tuner  | 5  | 800  | 2000 | Yes  | Yes   | No        | Requires Clip as material                 |
    // | Remix (Synchro)     | Tuner  | 5  | 1500 | 2000 | Yes  | Yes   | No        | Requires Mix as material                  |
    // | RS (Synchro)        | Tuner  | 5  | 1700 | 2400 | Yes  | Yes   | No        | Requires Reco as material                 |
    // | Loudness War (Syn)  | Tuner  | 6  | 0    | 3000 | Yes  | Yes   | No        | 2+ Tuners; protects other tuners          |
    // | Back 2 Back (Syn)   | Tuner  | 10 | 3200 | 0    | 2x   | Yes   | No        | 1 Tuner + 1+ Synchro Monsters             |
    // ============================================================
    // ACE CARDS:
    //   Primary  : Back 2 Back (3200 ATK, double attack, Quick Effect SS+Synchro 2x/turn)
    //   Secondary: Chaos Angel (3500 ATK, banish on SS, protection based on materials)
    //   Tertiary : Loudness War (0/3000 DEF wall, protects tuners, copy GY triggers)
    //
    // COMBO STARTERS:
    //   1. Cue (NS โ’ SS tuner from deck โ’ Hand-Sync โ’ TrackMaker)
    //   2. KT Synchro spell (search + immediate Synchro)
    //   3. JJ KewlTune (extra NS + tribute-search)
    //   4. Reco/Mix (NS โ’ search KT monster + Hand-Sync)
    //
    // IDEAL COMBO:
    //   NS Cue โ’ SS Rotary from deck โ’ Hand-Sync Lv1 from hand
    //   โ’ Synchro TrackMaker (Lv4) โ’ search KT Synchro
    //   โ’ Activate KT Synchro โ’ search Reco + Synchro RS/Crackle/Remix (Lv5)
    //   โ’ Use RS + TrackMaker โ’ Synchro Back2Back (Lv10) or LoudnessWar (Lv6)
    //
    // CHOKEPOINTS:
    //   Step 1: Cue NS effect โ’ Ash Blossom kills combo
    //   Step 2: TrackMaker search โ’ Ash kills follow-up
    //   Step 3: KT Synchro spell โ’ can be negated by Naturia Beast
    // ============================================================

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
            public const int FidraulisHarmonia = 70088809;

            // --- STANDARD HAND TRAPS ---
            public const int AshBlossom = 14558127;
            public const int GhostOgre = 59438930;
            public const int GhostBelle = 73642296;
            public const int EffectVeiler = 97268402;
            public const int InfiniteImpermanence = 10045474;

            // --- MAIN DECK SPELLS ---
            public const int JJKewlTune = 14442329;
            public const int KewlTuneSynchro = 78058681;
            public const int DuelistGenesis = 97474300;
            public const int SynchroOvertake = 99243014;
            public const int PotOfProsperity = 84211599;
            public const int TripleTacticsTalent = 25311006;
            public const int CalledByTheGrave = 24224830;

            // --- EXTRA DECK SYNCHROS ---
            public const int KewlTuneTrackMaker = 42781164;
            public const int KewlTuneCrackle = 39576656;
            public const int KewlTuneRemix = 88170262;
            public const int KewlTuneRS = 15665977;
            public const int KewlTuneLoudnessWar = 41069676;
            public const int KewlTuneBack2Back = 65961304;
            public const int ZalenTheShackledDragon = 4891376;
            public const int ChaosAngel = 22850702;
            public const int VisasAmritara = 821049;
            public const int GoldenCloudBeastMalong = 93125329;
            public const int WindPegasusIgnister = 98506199;
            public const int EnigmasterPackbit = 72444406;

            // --- SIDE DECK CARDS ---
            public const int MulcharmyFuwalos = 42141493;
            public const int PSYFrameDriver = 49036338;
            public const int PSYFramegearDelta = 74203495;
            public const int PSYFramegearGamma = 38814750;
            public const int DrollAndLockBird = 94145021;
            public const int HarpiesFeatherDuster = 18144506;
            public const int SolemnWarning = 84749824;
        }

        // --- Archetype card sets for quick lookups ---
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
            CardId.EffectVeiler, CardId.MulcharmyFuwalos, CardId.DrollAndLockBird,
            CardId.PSYFramegearGamma, CardId.PSYFramegearDelta
        };

        // --- Floodgate cards (critical threats to prioritize) ---
        private static readonly int[] OpponentFloodgateCards = {
            42009023, 7902349, 15397015, 19261966, 82732047,
            30241314, 81674782, 51452091, 4280258, 84815190,
            10443957, 47060154, 33198837, 99916754
        };

        // --- Ace Card Protection ---
        private static readonly int[] AceCardIds = {
            CardId.KewlTuneBack2Back,
            CardId.ChaosAngel,
            CardId.KewlTuneLoudnessWar
        };
        public override bool IsAceCard(ClientCard card) => card != null && AceCardIds.Any(id => card.IsCode(id));
        private bool IsHandTrap(ClientCard c) => c != null && HandTrapIds.Contains(c.Id);
        private bool IsKewlTuneMonster(ClientCard c) => c != null && KewlTuneMainMonsters.Contains(c.Id);
        private bool IsKewlTuneSynchro(ClientCard c) => c != null && KewlTuneSynchros.Contains(c.Id);

        // ====================================================================================================
        //  OPT FLAGS โ€” reset every turn
        // ====================================================================================================
        private bool _potUsed;
        private bool _synchroOvertakeUsed;
        private bool _fieldUsed;                // JJ Kewl Tune tribute effect
        private bool _rotaryHandUsed;           // Rotary reveal-from-hand effect
        private bool _genesisUsed;
        private bool _cueNSUsed;                // Cue on-Normal-Summon SS effect
        private bool _recoSummonSearchUsed;     // Reco on-summon search
        private bool _mixSummonSearchUsed;      // Mix on-summon search
        private bool _loudnessWarEffectUsed;    // Loudness War GY banish quick effect
        private bool _rsNegateUsed;             // RS negate effect
        private bool _remixTributeUsed;         // Remix tribute quick effect
        private int _back2BackUsedCount;        // Back2Back can be used TWICE per turn
        private bool _trackMakerSearchUsed;     // TrackMaker on-SS search
        private int _kewlTuneSynchroUseCount;   // KT Synchro spell can be used 2x per turn
        private bool _tunerOnlyLock;            // SS restricted to Tuners only this turn
        private bool _hasOpponentActivatedMonsterEffect;
        private int _synchroSummonsThisTurn;

        /// <summary>
        /// Priority for using a tuner as Synchro Material (lower = use first).
        /// Hand traps should NEVER be used as Synchro Material voluntarily.
        /// </summary>
        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            // Ace cards โ€” NEVER use as material
            if (IsAceCard(c)) return 900;
            // Hand traps โ€” avoid at all costs
            if (IsHandTrap(c)) return 800;
            // Fidraulis โ€” keep for Quick Effect disruption
            if (c.Id == CardId.FidraulisHarmonia) return 700;
            // PSY-Frame Driver โ€” expendable non-tuner
            if (c.Id == CardId.PSYFrameDriver) return 5;
            // Kwtune monsters by expendability
            switch (c.Id)
            {
                case CardId.KewlTuneClip: return 10;      // Lv1, effect is opp-turn only
                case CardId.KewlTuneRotary: return 15;     // Lv1, useful but replaceable
                case CardId.KewlTuneMix: return 20;        // Lv2, has GY trigger (destroy monster)
                case CardId.KewlTuneReco: return 25;       // Lv3, has GY trigger (destroy S/T)
                case CardId.KewlTuneCue: return 30;        // Lv3, has GY trigger (excavate)
                // Synchros โ€” prefer using lower-value ones
                case CardId.KewlTuneTrackMaker: return 50;
                case CardId.KewlTuneCrackle: return 55;
                case CardId.KewlTuneRemix: return 60;
                case CardId.KewlTuneRS: return 65;
                case CardId.KewlTuneLoudnessWar: return 70;
                case CardId.GoldenCloudBeastMalong: return 45;
                default: return 100;
            }
        }

        /// <summary>
        /// Priority for Normal Summon (lower = summon first).
        /// Cue is the strongest combo starter.
        /// </summary>
        private int GetNormalSummonPriority(ClientCard c)
        {
            if (c == null) return 99;
            switch (c.Id)
            {
                case CardId.KewlTuneCue: return 1;     // SS tuner from deck โ’ instant Synchro
                case CardId.KewlTuneMix: return 2;      // Search on summon + Lv2 for varied Synchro
                case CardId.KewlTuneReco: return 3;     // Search on summon + Lv3
                case CardId.KewlTuneRotary: return 4;   // Lv1, can reveal for extra NS
                case CardId.KewlTuneClip: return 5;     // Lv1, effect is opp-turn only
                default: return 99;
            }
        }

        /// <summary>
        /// Priority for selecting a tuner from hand for Hand-Sync.
        /// We want to use the tuner that gives us the best GY trigger.
        /// </summary>
        private int GetHandSyncPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsHandTrap(c)) return 900;  // NEVER use hand traps
            if (c.Id == CardId.FidraulisHarmonia) return 800;
            switch (c.Id)
            {
                // Prefer tuners whose GY triggers match the board state
                case CardId.KewlTuneClip: return 10;    // Banish from opp ED โ€” always useful
                case CardId.KewlTuneRotary: return 15;   // Search on GY send
                case CardId.KewlTuneMix: return 20;      // Destroy opp monster
                case CardId.KewlTuneReco: return 25;     // Destroy opp S/T
                case CardId.KewlTuneCue: return 30;      // Excavate โ€” mild disruption
                default: return 100;
            }
        }

        // ====================================================================================================
        //  CONSTRUCTOR โ€” Executor Registration Order
        // ====================================================================================================

        public _2026_KwtuneExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // โ”€โ”€ Combo Router: Sequencing โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Standard-Setup",
                RequiredCards = new List<int> { CardId.KewlTuneCue, CardId.KewlTuneRotary },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.KewlTuneCue, ActionType = ExecutorType.Activate, Description = "Play CardId.KewlTuneCue" },
                    new() { CardId = CardId.KewlTuneRotary, ActionType = ExecutorType.Activate, Description = "Extend with CardId.KewlTuneRotary" }
                },
                EndBoardScore = 80
            });

            // โ”€โ”€ Bait Planner โ”€โ”€
            BaitPlanner.RegisterComboStarters(CardId.KewlTuneCue, CardId.KewlTuneReco, CardId.KewlTuneSynchro, CardId.KewlTuneTrackMaker);
            BaitPlanner.RegisterBaitCards(CardId.PotOfProsperity, CardId.KewlTuneReco);

            // โ”€โ”€ Chain Advisor โ”€โ”€
            ChainAdvisor.RegisterHighValueTargets(CardId.KewlTuneCue, CardId.EffectVeiler, CardId.AshBlossom);

            // โ•โ•โ• TIER 1: Hand Traps & Negation (opponent-reactive, highest priority) โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollEffect);
            AddExecutor(ExecutorType.Activate, CardId.PSYFramegearGamma, GammaEffect);
            AddExecutor(ExecutorType.Activate, CardId.PSYFramegearDelta, DeltaEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.GhostOgre, GhostOgreEffect);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelle, GhostBelleEffect);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, EffectVeilerEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, ImpermanenceEffect);

            // โ•โ•โ• TIER 2: Quick Effects on opponent's turn (Boss disruption) โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.FidraulisHarmonia, FidraulisHarmoniaEffect);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneLoudnessWar, LoudnessWarEffect);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneBack2Back, Back2BackEffect);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneRS, RSEffect);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneRemix, RemixEffect);
            AddExecutor(ExecutorType.Activate, CardId.ZalenTheShackledDragon, ZalenEffect);

            // โ•โ•โ• TIER 3: Search Spells โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, PotOfProsperityEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentEffect);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneSynchro, KewlTuneSynchroEffect);
            AddExecutor(ExecutorType.Activate, CardId.SynchroOvertake, SynchroOvertakeEffect);
            AddExecutor(ExecutorType.Activate, CardId.DuelistGenesis, DuelistGenesisEffect);
            AddExecutor(ExecutorType.Activate, CardId.JJKewlTune, JJKewlTuneEffect);

            // โ•โ•โ• TIER 4: Monster Effects (on-field / on-summon) โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneCue, KewlTuneCueEffect);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneReco, KewlTuneRecoEffect);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneMix, KewlTuneMixEffect);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneRotary, KewlTuneRotaryEffect);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneClip, KewlTuneClipEffect);

            // โ•โ•โ• TIER 5: Extra Deck Synchros (ordered by combo sequence) โ•โ•โ•
            // Step 1: TrackMaker (Lv4 โ€” the bridge)
            AddExecutor(ExecutorType.SpSummon, CardId.KewlTuneTrackMaker, TrackMakerSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneTrackMaker, TrackMakerActivateEffect);

            // Step 2: Level 5s (RS for disruption, Crackle for ED banish, Remix for setup)
            AddExecutor(ExecutorType.SpSummon, CardId.KewlTuneRS, RSSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.KewlTuneCrackle, CrackleSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.KewlTuneRemix, RemixSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.KewlTuneCrackle, CrackleEffect);

            // Step 3: Level 6 (Loudness War โ€” wall + protect + copy GY triggers)
            AddExecutor(ExecutorType.SpSummon, CardId.KewlTuneLoudnessWar, LoudnessWarSpSummon);

            // Step 4: Generic utility Synchros
            AddExecutor(ExecutorType.SpSummon, CardId.GoldenCloudBeastMalong, GenericSynchroSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.WindPegasusIgnister, GenericSynchroSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.ZalenTheShackledDragon, ZalenSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.VisasAmritara, GenericSynchroSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.EnigmasterPackbit, GenericSynchroSummonCheck);

            // Step 5: Boss monsters (Lv10)
            AddExecutor(ExecutorType.SpSummon, CardId.KewlTuneBack2Back, Back2BackSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ChaosAngel, ChaosAngelSpSummon);

            // โ•โ•โ• TIER 6: Normal Summons (combo starters) โ•โ•โ•
            AddExecutor(ExecutorType.Summon, CardId.KewlTuneCue, NormalSummonCue);
            AddExecutor(ExecutorType.Summon, CardId.KewlTuneMix, NormalSummonMix);
            AddExecutor(ExecutorType.Summon, CardId.KewlTuneReco, NormalSummonReco);
            AddExecutor(ExecutorType.Summon, CardId.KewlTuneRotary, NormalSummonRotary);
            AddExecutor(ExecutorType.Summon, CardId.KewlTuneClip, NormalSummonClip);

            // โ•โ•โ• TIER 7: Set Spells/Traps & Repos โ•โ•โ•
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnWarning);
            AddExecutor(ExecutorType.Repos, MonsterReposLogic);
        }

        // ====================================================================================================
        //  TURN LIFECYCLE
        // ====================================================================================================

        public override bool OnSelectHand()
        {
            // Kwtune is a synchro combo/control deck โ€” prefer going first to set up Back2Back + disrupt
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
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
            _synchroSummonsThisTurn = 0;

            // โ”€โ”€ Going-Second BreakBoard: prioritize disruption over combo โ”€โ”€
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
            }
        }

        public override void OnChaining(int player, ClientCard card)
        {
            if (player == 1 && card != null && card.IsMonster() && Duel.Player == 0)
                _hasOpponentActivatedMonsterEffect = true;
            base.OnChaining(player, card);
        }

        // ====================================================================================================
        //  BOARD ANALYSIS HELPERS
        // ====================================================================================================

        protected override bool CanDealLethal()
        {
            if (Scorer != null && Scorer.HasLethal()) return true;
            int total = 0;
            bool hasBack2Back = false;
            foreach (var c in Bot.MonsterZone)
            {
                if (c == null || !c.IsFaceup() || !c.IsAttack()) continue;
                if (c.IsCode(CardId.KewlTuneBack2Back) && !c.IsDisabled())
                    hasBack2Back = true;
                total += c.Attack;
            }
            // Back2Back allows all Tuners to attack twice
            if (hasBack2Back)
            {
                int tunerAtk = 0;
                foreach (var c in Bot.MonsterZone)
                {
                    if (c == null || !c.IsFaceup() || !c.IsAttack()) continue;
                    if (c.HasType(CardType.Tuner)) tunerAtk += c.Attack;
                }
                total += tunerAtk; // second attack damage
            }
            return total >= Enemy.LifePoints;
        }

        private bool IsOpponentThreatening()
        {
            if (Util.GetProblematicEnemyMonster() != null) return true;
            if (Enemy.GetMonsterCount() > 0)
            {
                if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup()
                    && (OpponentFloodgateCards.Contains(c.Id) || c.Attack >= 2400)))
                    return true;
            }
            if (Enemy.GetSpellCount() >= 2) return true;
            return false;
        }

        private bool CanActivateSpellTrap()
        {
            if (Card == null) return true;
            if (Card.IsSpell() && Enemy.HasInMonstersZone(33198837)) return false; // Naturia Beast
            return true;
        }

        /// <summary>
        /// Count how many Kwtune tuners are available for Hand-Sync in hand (excluding the current card being evaluated).
        /// </summary>
        private int CountHandSyncPartnersInHand()
        {
            return Bot.Hand.Count(c => c != null && c != Card
                && c.HasType(CardType.Tuner)
                && IsKewlTuneMonster(c));
        }

        /// <summary>
        /// Check if we have materials for a specific level Synchro on field+hand.
        /// </summary>
        private bool HasMaterialsForLevel(int targetLevel)
        {
            // With Hand-Sync, we need: 1 Tuner on field + 1 Tuner in hand whose levels sum = targetLevel
            foreach (var fieldMon in Bot.GetMonsters())
            {
                if (fieldMon == null || !fieldMon.IsFaceup() || !fieldMon.HasType(CardType.Tuner)) continue;
                int needed = targetLevel - fieldMon.Level;
                if (needed <= 0) continue;
                foreach (var handMon in Bot.Hand)
                {
                    if (handMon == null || !handMon.HasType(CardType.Tuner)) continue;
                    if (IsHandTrap(handMon)) continue; // don't count hand traps
                    if (handMon.Level == needed) return true;
                }
            }
            // Also check 2 tuners on field summing to level
            var tuners = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasType(CardType.Tuner)).ToList();
            for (int i = 0; i < tuners.Count; i++)
                for (int j = i + 1; j < tuners.Count; j++)
                    if (tuners[i].Level + tuners[j].Level == targetLevel) return true;
            return false;
        }

        private ClientCard GetHighestPriorityMonsterTarget()
        {
            ClientCard problematic = Util.GetProblematicEnemyMonster(0, true);
            if (problematic != null) return problematic;
            return Enemy.GetMonsters()
                .Where(c => c != null && c.IsFaceup())
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();
        }

        private ClientCard GetHighestPriorityBackrowTarget()
        {
            ClientCard problematic = Util.GetProblematicEnemySpell();
            if (problematic != null) return problematic;
            // Floodgate face-up
            foreach (var spell in Enemy.GetSpells())
                if (spell != null && spell.IsFaceup() && OpponentFloodgateCards.Contains(spell.Id))
                    return spell;
            // Face-down S/T first, then any S/T
            return Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFacedown())
                ?? Enemy.GetSpells().FirstOrDefault(c => c != null);
        }

        /// <summary>
        /// Determine if we should skip combo because we can already win this turn.
        /// </summary>
        private bool ShouldSkipComboForLethal()
        {
            return Duel.Phase == DuelPhase.Main1 && CanDealLethal();
        }

        // --- Board State Evaluation ---

        protected override bool IsBoardStrongEnough()
        {
            if (Bot.HasInMonstersZone(CardId.KewlTuneBack2Back)) return true;
            if (Bot.HasInMonstersZone(CardId.ChaosAngel)) return true;
            if (Bot.HasInMonstersZone(CardId.KewlTuneLoudnessWar)) return true;
            // TrackMaker + a Level 5 Synchro is a solid field
            if (Bot.HasInMonstersZone(CardId.KewlTuneTrackMaker) && Bot.GetMonsterCount() >= 2) return true;
            return false;
        }

        protected override bool IsInGrindGame()
        {
            // Limited resources โ€” need to be conservative
            if (Bot.GetHandCount() <= 1 && Bot.GetMonsterCount() <= 1 && Bot.Graveyard.Count(c => c != null && IsKewlTuneMonster(c)) <= 2)
                return true;
            return false;
        }

        protected override bool NeedsBoardPresence()
        {
            int ourFaceup = Bot.MonsterZone.Count(c => c != null && c.IsFaceup());
            int enemyMonsters = Enemy.GetMonsterCount();
            return ourFaceup == 0 || (ourFaceup <= 1 && enemyMonsters >= 2);
        }

        /// <summary>
        /// Board-state-aware Synchro priority: which Synchro monster to aim for.
        /// </summary>
        private enum SynchroPriority
        {
            Back2Back,     // Lv10 โ€” game finisher (double attack, Quick Effect SS+Synchro)
            ChaosAngel,    // Lv10 โ€” threat removal (banish on SS)
            LoudnessWar,   // Lv6 โ€” protection for tuners + copy GY triggers
            RS,            // Lv5 โ€” Quick Effect negate
            Crackle,       // Lv5 โ€” ED disruption (banish from opp ED)
            Remix,         // Lv5 โ€” GY recovery (tribute to recover 2 tuners)
            TrackMaker,    // Lv4 โ€” bridge to search KT Synchro spell
            Zalen,         // Lv7 โ€” counter-negate
            Generic        // Other utility Synchros
        }

        private SynchroPriority GetPreferredSynchro()
        {
            // Game-ending priority: Back2Back for double attack
            if (Duel.MainPhase != null && Duel.MainPhase.CanBattlePhase)
            {
                int totalAtk = Bot.MonsterZone.Where(c => c != null && c.IsFaceup() && c.IsAttack() && !c.Attacked)
                    .Sum(c => c.Attack);
                // Back2Back at 3200 + any other monster's ATK
                if (totalAtk + 3200 >= Enemy.LifePoints)
                    return SynchroPriority.Back2Back;

                // Chaos Angel at 3500
                if (totalAtk + 3500 >= Enemy.LifePoints)
                    return SynchroPriority.ChaosAngel;
            }

            // Need protection for our tuners โ’ Loudness War
            int tunerCount = Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && c.HasType(CardType.Tuner));
            if (tunerCount >= 2 && OpponentHasThreateningMonster())
                return SynchroPriority.LoudnessWar;

            // Opponent has threatening monster โ’ RS for negate or Chaos Angel for banish
            if (OpponentHasThreateningMonster())
            {
                // If we need Quick Effect negate on opponent's turn
                if (Duel.Turn >= 2 || Duel.Player == 1)
                    return SynchroPriority.RS;
            }

            // Opponent has cards in Extra Deck โ’ Crackle for ED disruption
            if (Enemy.ExtraDeck.Count >= 3 && !_tunerOnlyLock)
                return SynchroPriority.Crackle;

            // Need GY recovery โ’ Remix (but less priority)
            if (IsInGrindGame() && Bot.Graveyard.Count(c => c != null && IsKewlTuneMonster(c)) >= 2)
                return SynchroPriority.Remix;

            // Opponent has big board โ’ Chaos Angel
            if (Enemy.GetMonsterCount() >= 2 && !_tunerOnlyLock)
                return SynchroPriority.ChaosAngel;

            // Opponent chain-heavy โ’ Zalen
            if (_hasOpponentActivatedMonsterEffect && Duel.Player == 0 && !_tunerOnlyLock)
                return SynchroPriority.Zalen;

            // Default: TrackMaker as bridge (search KT Synchro)
            if (HasMaterialsForLevel(4))
                return SynchroPriority.TrackMaker;

            return SynchroPriority.Generic;
        }

        private bool OpponentHasThreateningMonster()
        {
            return Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && !c.IsDisabled() &&
                (OpponentFloodgateCards.Contains(c.Id) || c.Attack >= 2400 ||
                 c.HasType(CardType.Link) || c.HasType(CardType.Synchro)));
        }

        // ====================================================================================================
        //  HAND TRAPS
        // ====================================================================================================

        private bool MulcharmyFuwalosEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1 && Bot.GetMonsterCount() == 0
                && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2);
        }

        private bool DrollEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1;
        }

        private bool GammaEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (Bot.GetMonsterCount() > 0 || Duel.LastChainPlayer != 1) return false;
            ClientCard last = Util.GetLastChainCard();
            return last != null && last.IsMonster();
        }

        private bool DeltaEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (Bot.GetMonsterCount() > 0 || Duel.LastChainPlayer != 1) return false;
            ClientCard last = Util.GetLastChainCard();
            return last != null && last.IsSpell();
        }

        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain()) return false;
            // Self-harm check: never negate our own chain
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;
            ClientCard last = Util.GetLastChainCard();
            if (last == null) return false;
            // Try the built-in Ash logic first (checks if effect involves deck)
            if (DefaultAshBlossomAndJoyousSpring()) return true;
            return false;
        }

        private bool GhostOgreEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;
            ClientCard last = Util.GetLastChainCard();
            if (last == null) return false;
            // Ghost Ogre: destroy face-up card that activated its effect on field
            // DON'T activate vs Normal/Quick spells (they go to GY before resolution)
            if (last.Controller == 1 && last.IsFaceup())
            {
                if (last.Location == CardLocation.MonsterZone) return true;
                // For spells: only activate against Continuous, Field, Equip, Pendulum
                if (last.Location == CardLocation.SpellZone
                    && (last.HasType(CardType.Continuous) || last.HasType(CardType.Field)
                        || last.HasType(CardType.Equip) || last.HasType(CardType.Pendulum)))
                    return true;
            }
            return false;
        }

        private bool GhostBelleEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;
            ClientCard last = Util.GetLastChainCard();
            if (last == null) return false;

            // Custom whitelist of common cards that interact with the GY that the default checker might miss
            int[] gyInteractionIds = {
                83764718, // Monster Reborn
                97077563, // Call of the Haunted
                30012506, // A-Assault Core (adds Union from GY - real ID)
                15622650, // A-Assault Core (adds Union from GY)
                60846090, // Union Scramble (banish to add from GY)
                71039903, // The White Stone of Ancients (banish to add Blue-Eyes from GY)
                6853254,  // Return of the Dragon Lords (SS from GY)
                87025064, // Silver's Cry (SS from GY)
                24224830, // Called by the Grave (banish from GY)
                62325062, // Bystial Druiswurm (banish from GY)
                94250255, // Bystial Magnamhut (banish from GY)
                9600109,  // Bystial Saronir (banish from GY)
                17209452, // Kewl Tune Rotary (GY search/add Tuner from GY)
                16509007  // Kewl Tune Mix (on-summon search can choose from GY)
            };

            return DefaultGhostBelleAndHauntedMansion() || gyInteractionIds.Contains(last.Id);
        }

        private bool EffectVeilerEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.Player != 1 || Duel.LastChainPlayer != 1) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
            ClientCard last = Util.GetLastChainCard();
            if (last != null && last.IsMonster() && last.Controller == 1
                && last.Location == CardLocation.MonsterZone && !last.IsDisabled())
            {
                AI.SelectCard(last);
                return true;
            }
            return false;
        }

        private bool CalledByTheGraveEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;
            ClientCard last = Util.GetLastChainCard();
            // Called By: banish from GY to negate monster with same name
            if (last != null && last.IsMonster() && last.Location == CardLocation.Hand)
            {
                // Opponent using hand trap โ€” banish from their GY
                AI.SelectCard(last.Id);
                return true;
            }
            // Also check opponent GY for the chaining monster
            if (last != null && last.IsMonster())
            {
                var gyTarget = Enemy.Graveyard.FirstOrDefault(c => c != null && c.Id == last.Id);
                if (gyTarget != null)
                {
                    AI.SelectCard(gyTarget);
                    return true;
                }
            }
            return false;
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

        private bool ImpermanenceEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (!CanActivateSpellTrap()) return false;

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

        // ====================================================================================================
        //  QUICK EFFECTS ON OPPONENT'S TURN (TIER 2)
        // ====================================================================================================

        private bool FidraulisHarmoniaEffect()
        {
            // Quick Effect: When opponent activates monster effect on field
            // Reveal this card + up to 5 Synchros in ED
            // 2+ revealed: SS this card
            // 4+ revealed: Send 1 revealed Synchro to GY
            // 6 revealed: Destroy 1 opp monster
            if (Card.Location != CardLocation.Hand) return false;
            if (Duel.Player != 1) return false;
            if (LastChainCard == null || LastChainCard.Controller != 1 || !LastChainCard.IsMonster()) return false;

            // Count available Synchros in ED
            int synchroCount = Bot.ExtraDeck.Count(c => c != null && c.HasType(CardType.Synchro));
            if (synchroCount < 1) return false; // need at least 2 total (Fidraulis + 1 Synchro)

            // Only activate if opponent has a meaningful board (don't waste on minor effects)
            if (Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() <= 1) return false;

            return true;
        }

        private bool LoudnessWarEffect()
        {
            // Quick Effect: When opponent activates card/effect, banish 1 Kwtune from GY
            // โ’ copy that monster's GY trigger (sent as Synchro Material)
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_loudnessWarEffectUsed) return false;
            if (Duel.LastChainPlayer != 1) return false;

            // Check if we have a useful Kwtune monster in GY to banish
            bool hasUsefulGYTarget = Bot.Graveyard.Any(c => c != null && IsKewlTuneMonster(c));
            if (!hasUsefulGYTarget) return false;

            _loudnessWarEffectUsed = true;
            return true;
        }

        private bool Back2BackEffect()
        {
            // Quick Effect: When opponent activates monster effect
            // โ’ Target 1 Tuner in GY/banishment (not Lv10), add to hand or SS
            // โ’ Then can Synchro Summon. Twice per turn!
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_back2BackUsedCount >= 2) return false;
            if (Duel.LastChainPlayer != 1) return false;

            ClientCard lastCard = Util.GetLastChainCard();
            if (lastCard == null || !lastCard.IsMonster()) return false;

            // Check if we have a revivable Tuner in GY or banishment (not Lv10)
            bool hasTarget = Bot.Graveyard.Any(c => c != null && c.IsMonster()
                && c.HasType(CardType.Tuner) && c.Level != 10 && c.IsCanRevive())
                || Bot.Banished.Any(c => c != null && c.IsMonster()
                && c.HasType(CardType.Tuner) && c.Level != 10);
            if (!hasTarget) return false;

            _back2BackUsedCount++;
            return true;
        }

        private bool RSEffect()
        {
            // Quick Effect (Main Phase): Banish 1 Tuner from GY โ’ negate 1 face-up card
            if (Card.Location != CardLocation.MonsterZone) return false; // Must be on field
            if (Card.IsDisabled()) return false;
            if (_rsNegateUsed) return false;

            // Need a Tuner in GY to banish
            bool hasGYTuner = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.HasType(CardType.Tuner));
            if (!hasGYTuner) return false;

            // Need a face-up card on opponent's field to negate
            ClientCard target = GetHighestPriorityMonsterTarget();
            if (target == null)
                target = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());
            if (target == null) return false;

            AI.SelectCard(target);
            _rsNegateUsed = true;
            return true;
        }

        private bool RemixEffect()
        {
            // Quick Effect (Opponent's turn): Tribute this card
            // โ’ Choose 2 non-Synchro Tuners in GY, add 1 to hand, SS the other
            // โ’ Then can Synchro Summon
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_remixTributeUsed) return false;
            if (Duel.Player != 1) return false;

            // Need 2+ non-Synchro Tuners in GY
            var gyTuners = Bot.Graveyard.Where(c => c != null && c.IsMonster()
                && c.HasType(CardType.Tuner) && !c.HasType(CardType.Synchro)).ToList();
            if (gyTuners.Count < 2) return false;

            _remixTributeUsed = true;
            return true;
        }

        private bool ZalenEffect()
        {
            // Quick Effect: When a card/effect is activated in response to another
            // โ’ Negate the first or second effect and destroy that card
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (Duel.LastChainPlayer != 1) return false;

            // Only activate when there's a chain to interact with
            if (Duel.CurrentChain.Count < 1) return false;
            return true;
        }

        // ====================================================================================================
        //  SEARCH SPELLS
        // ====================================================================================================

        private bool PotOfProsperityEffect()
        {
            if (_potUsed) return false;
            if (ShouldSkipComboForLethal()) return false;
            // Need at least some ED cards to banish
            if (Bot.ExtraDeck.Count < 3) return false;
            _potUsed = true;
            return true;
        }

        private bool TripleTacticsTalentEffect()
        {
            if (Duel.Player != 0 || !_hasOpponentActivatedMonsterEffect) return false;
            if (Bot.GetHandCount() <= 2)
            {
                AI.SelectOption(0); // Draw 2
                return true;
            }
            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= 2500))
            {
                AI.SelectOption(1); // Take Control
                return true;
            }
            AI.SelectOption(2); // Hand Rip
            return true;
        }

        private bool KewlTuneSynchroEffect()
        {
            // KT Synchro: Search Kwtune card + immediately Synchro Summon Tuner Synchro
            // Can be used 2x per turn. Locks SS to Tuners only.
            if (_kewlTuneSynchroUseCount >= 2) return false;
            if (ShouldSkipComboForLethal()) return false;
            if (!CanActivateSpellTrap()) return false;

            _kewlTuneSynchroUseCount++;
            _tunerOnlyLock = true;
            return true;
        }

        private bool SynchroOvertakeEffect()
        {
            if (_synchroOvertakeUsed) return false;
            if (ShouldSkipComboForLethal()) return false;
            // Check we have targets in deck
            if (Bot.GetRemainingCount(CardId.KewlTuneReco, 3) == 0
                && Bot.GetRemainingCount(CardId.KewlTuneCue, 3) == 0
                && Bot.GetRemainingCount(CardId.KewlTuneMix, 3) == 0
                && Bot.GetRemainingCount(CardId.KewlTuneRotary, 3) == 0
                && Bot.GetRemainingCount(CardId.KewlTuneClip, 1) == 0)
                return false;
            _synchroOvertakeUsed = true;
            return true;
        }

        private bool DuelistGenesisEffect()
        {
            if (_genesisUsed) return false;
            if (ShouldSkipComboForLethal()) return false;
            // Need a Tuner and a Synchro in GY
            bool hasTuner = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.HasType(CardType.Tuner));
            bool hasSynchro = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.HasType(CardType.Synchro));
            if (!hasTuner || !hasSynchro) return false;
            _genesisUsed = true;
            AI.SelectCard(CardId.KewlTuneSynchro);
            return true;
        }

        private bool JJKewlTuneEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Activate as Field Spell
                if (Bot.HasInSpellZone(CardId.JJKewlTune)) return false;
                return true;
            }
            // On-field effect: Tribute 1 Tuner โ’ search/SS Kwtune from deck
            if (_fieldUsed) return false;
            if (ShouldSkipComboForLethal()) return false;
            // Need a tuner on field to tribute (prefer expendable ones)
            if (!Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Tuner) && !IsAceCard(c)))
                return false;
            // Need targets in deck
            if (Bot.GetRemainingCount(CardId.KewlTuneReco, 3) == 0
                && Bot.GetRemainingCount(CardId.KewlTuneCue, 3) == 0
                && Bot.GetRemainingCount(CardId.KewlTuneMix, 2) == 0)
                return false;
            _fieldUsed = true;
            _tunerOnlyLock = true;
            return true;
        }

        // ====================================================================================================
        //  NORMAL SUMMONS
        // ====================================================================================================

        private bool NormalSummonCue()
        {
            if (ShouldSkipComboForLethal()) return false;
            if (_cueNSUsed) return false;
            // Cue NS effect: SS 1 Tuner from hand/deck/GY (not Cue) โ’ locks to Tuner SS only
            // Best combo starter โ€” always summon if possible
            // Check we have a tuner in hand for Hand-Sync (or in deck for Cue's SS effect)
            bool hasTunerForSync = Bot.Hand.Any(c => c != null && c != Card
                && c.HasType(CardType.Tuner) && !IsHandTrap(c));
            bool hasTunerInDeck = Bot.GetRemainingCount(CardId.KewlTuneRotary, 3) > 0
                || Bot.GetRemainingCount(CardId.KewlTuneClip, 1) > 0
                || Bot.GetRemainingCount(CardId.KewlTuneReco, 3) > 0
                || Bot.GetRemainingCount(CardId.KewlTuneMix, 2) > 0;
            bool hasTunerInGY = Bot.Graveyard.Any(c => c != null && IsKewlTuneMonster(c));

            return hasTunerForSync || hasTunerInDeck || hasTunerInGY;
        }

        private bool NormalSummonMix()
        {
            if (ShouldSkipComboForLethal()) return false;
            // Mix NS effect: Add 1 Kwtune (not Lv2) from deck/GY to hand
            // Good if we have another tuner for Hand-Sync
            bool hasTunerPartner = Bot.Hand.Any(c => c != null && c != Card
                && c.HasType(CardType.Tuner) && !IsHandTrap(c));
            return hasTunerPartner || Bot.GetMonsterCount() == 0;
        }

        private bool NormalSummonReco()
        {
            if (ShouldSkipComboForLethal()) return false;
            // Reco NS effect: Add 1 Kwtune (not Lv3) from deck/GY to hand
            bool hasTunerPartner = Bot.Hand.Any(c => c != null && c != Card
                && c.HasType(CardType.Tuner) && !IsHandTrap(c));
            return hasTunerPartner || Bot.GetMonsterCount() == 0;
        }

        private bool NormalSummonRotary()
        {
            if (ShouldSkipComboForLethal()) return false;
            // Rotary: Lv1, can reveal from hand for extra NS
            return Bot.GetMonsterCount() == 0
                || Bot.Hand.Any(c => c != null && c != Card && c.HasType(CardType.Tuner) && !IsHandTrap(c));
        }

        private bool NormalSummonClip()
        {
            if (ShouldSkipComboForLethal()) return false;
            // Clip is last resort โ€” effect is opponent-turn only
            return Bot.GetMonsterCount() == 0
                && Bot.Hand.Any(c => c != null && c != Card && c.HasType(CardType.Tuner) && !IsHandTrap(c));
        }

        // ====================================================================================================
        //  MONSTER EFFECTS (ON-FIELD / ON-SUMMON)
        // ====================================================================================================

        private bool KewlTuneCueEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_cueNSUsed) return false;
                _cueNSUsed = true;
                _tunerOnlyLock = true;
                // SS from deck/hand/GY: prefer Rotary (Lv1 for flexible Synchro) > Clip > others
                AI.SelectCard(CardId.KewlTuneRotary, CardId.KewlTuneClip,
                    CardId.KewlTuneMix, CardId.KewlTuneReco);
                return true;
            }
            // GY trigger: excavate 2 from opp deck, banish 1
            // Only activate if opponent has cards in deck (min 2)
            if (Enemy.Deck.Count < 2) return false;
            return true;
        }

        private bool KewlTuneRecoEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_recoSummonSearchUsed) return false;
                _recoSummonSearchUsed = true;
                // Search Kwtune not Lv3 โ’ get Mix(Lv2), Clip(Lv1), Rotary(Lv1)
                // Prefer cards we need for combo
                if (!Bot.HasInHand(CardId.KewlTuneMix) && Bot.GetRemainingCount(CardId.KewlTuneMix, 2) > 0)
                    AI.SelectCard(CardId.KewlTuneMix);
                else if (!Bot.HasInHand(CardId.KewlTuneClip) && Bot.GetRemainingCount(CardId.KewlTuneClip, 1) > 0)
                    AI.SelectCard(CardId.KewlTuneClip);
                else
                    AI.SelectCard(CardId.KewlTuneRotary, CardId.KewlTuneMix, CardId.KewlTuneClip);
                return true;
            }
            // GY trigger: Destroy 1 opp S/T
            // Only activate if opponent has valid S/T targets
            if (Enemy.GetSpells().Any(c => c != null && c.IsFaceup())) return true;
            if (Enemy.GetSpells().Any(c => c != null && c.IsFacedown())) return true;
            return false;
        }

        private bool KewlTuneMixEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_mixSummonSearchUsed) return false;
                _mixSummonSearchUsed = true;
                // Search Kwtune not Lv2 โ’ get Reco(Lv3), Cue(Lv3), Clip(Lv1), Rotary(Lv1)
                if (!Bot.HasInHand(CardId.KewlTuneReco) && Bot.GetRemainingCount(CardId.KewlTuneReco, 3) > 0)
                    AI.SelectCard(CardId.KewlTuneReco);
                else if (!Bot.HasInHand(CardId.KewlTuneCue) && Bot.GetRemainingCount(CardId.KewlTuneCue, 3) > 0)
                    AI.SelectCard(CardId.KewlTuneCue);
                else
                    AI.SelectCard(CardId.KewlTuneReco, CardId.KewlTuneCue, CardId.KewlTuneRotary, CardId.KewlTuneClip);
                return true;
            }
            // GY trigger: Destroy 1 opp monster
            // Only activate if opponent has valid monster target
            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup())) return true;
            return false;
        }

        private bool KewlTuneRotaryEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.Hand)
            {
                // Hand effect: Reveal this + 1 other Tuner in hand โ’ extra Normal Summon
                if (_rotaryHandUsed) return false;
                if (!Bot.Hand.Any(c => c != null && c != Card && c.HasType(CardType.Tuner) && !IsHandTrap(c)))
                    return false;
                _rotaryHandUsed = true;
                return true;
            }
            // GY trigger: Search KT spell/trap or add Tuner
            // Only activate if we need the search target
            bool needKTSynchro = !Bot.HasInHand(CardId.KewlTuneSynchro) && !Bot.SpellZone.Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.KewlTuneSynchro));
            bool needTuner = !Bot.HasInHand(CardId.KewlTuneCue) && !Bot.HasInHand(CardId.KewlTuneReco) &&
                             !Bot.HasInHand(CardId.KewlTuneMix);
            if (!needKTSynchro && !needTuner) return false;

            // Check if there are targets remaining in deck
            bool hasTarget = GetRemainingCount(CardId.KewlTuneSynchro) > 0 ||
                             GetRemainingCount(CardId.KewlTuneCue) > 0 ||
                             GetRemainingCount(CardId.KewlTuneReco) > 0 ||
                             GetRemainingCount(CardId.KewlTuneMix) > 0;
            return hasTarget;
        }

        private bool KewlTuneClipEffect()
        {
            if (ShouldSkipCombo()) return false;
            // Clip effect: only activatable during opponent's Main Phase
            // Banish from opponent's ED
            return Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2);
        }

        // ====================================================================================================
        //  EXTRA DECK SYNCHROS โ€” SUMMON CONDITIONS
        // ====================================================================================================

        private bool TrackMakerSpSummon()
        {
            // Lv4 Synchro: 1 "Kewl Tune" Tuner + 1+ Tuners
            // The bridge card โ€” search on SS, bounce on GY send
            if (ShouldSkipComboForLethal()) return false;
            SynchroPriority preferred = GetPreferredSynchro();
            // TrackMaker is the bridge โ€” always good for advancing combo
            _synchroSummonsThisTurn++;
            return true;
        }

        private bool TrackMakerActivateEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_trackMakerSearchUsed) return false;
                _trackMakerSearchUsed = true;
                // Search a Kwtune card โ€” prioritize KT Synchro spell if don't have it
                if (!Bot.HasInHand(CardId.KewlTuneSynchro)
                    && Bot.GetRemainingCount(CardId.KewlTuneSynchro, 3) > 0)
                    AI.SelectCard(CardId.KewlTuneSynchro);
                else if (!Bot.HasInSpellZone(CardId.JJKewlTune) && !Bot.HasInHand(CardId.JJKewlTune)
                    && Bot.GetRemainingCount(CardId.JJKewlTune, 1) > 0)
                    AI.SelectCard(CardId.JJKewlTune);
                else
                    AI.SelectCard(CardId.KewlTuneSynchro, CardId.JJKewlTune,
                        CardId.KewlTuneReco, CardId.KewlTuneCue);
                return true;
            }
            // GY trigger: Bounce 1 opp card
            // Only activate if opponent has valid target
            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup())) return true;
            if (Enemy.GetSpells().Any(c => c != null && c.IsFaceup())) return true;
            return false;
        }

        private bool RSSpSummon()
        {
            // Lv5 Synchro: "Kewl Tune Reco" + 1+ Tuners
            // Has Quick Effect negate โ€” good for disruption
            if (ShouldSkipComboForLethal()) return false;
            // Prioritize RS if we need negate (opponent has threats)
            SynchroPriority preferred = GetPreferredSynchro();
            if (preferred != SynchroPriority.RS && preferred != SynchroPriority.Back2Back && preferred != SynchroPriority.LoudnessWar)
            {
                // Don't waste materials on RS if we need a different Synchro
                if (IsBoardStrongEnough() && !OpponentHasThreateningMonster()) return false;
            }
            _synchroSummonsThisTurn++;
            return true;
        }

        private bool CrackleSpSummon()
        {
            // Lv5 Synchro: "Kewl Tune Clip" + 1+ Tuners
            // On SS: look at opp ED, banish 1 card (disrupts ED strategies)
            if (ShouldSkipComboForLethal()) return false;
            SynchroPriority preferred = GetPreferredSynchro();
            // Only summon Crackle if preferred or ED disruption is useful
            if (preferred != SynchroPriority.Crackle && preferred != SynchroPriority.Back2Back)
            {
                // Crackle is useful when opponent relies on Extra Deck
                if (Enemy.ExtraDeck.Count < 3) return false;
            }
            _synchroSummonsThisTurn++;
            return true;
        }

        private bool CrackleEffect()
        {
            if (ShouldSkipCombo()) return false;
            // On Synchro Summon: Banish from opp ED โ€” always beneficial (on field)
            if (Card.Location == CardLocation.MonsterZone) return true;
            // GY self-revive effect: SS this card when sent to GY as Synchro Summoned card
            // Only activate if we can use it for further Synchro or as material
            if (Card.Location == CardLocation.Grave)
            {
                // Revive only if we have another tuner on field to Synchro with
                if (Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Tuner) && c != Card))
                    return true;
                // Or if we need board presence
                if (NeedsBoardPresence()) return true;
                return false;
            }
            return false;
        }

        private bool RemixSpSummon()
        {
            // Lv5 Synchro: "Kewl Tune Mix" + 1+ Tuners
            // Has Quick Effect tribute โ’ recover 2 GY tuners + Synchro
            if (ShouldSkipComboForLethal()) return false;
            // Only summon Remix if we need GY recovery (grind game) or we're aiming for Back2Back
            SynchroPriority preferred = GetPreferredSynchro();
            if (preferred != SynchroPriority.Remix && preferred != SynchroPriority.Back2Back && preferred != SynchroPriority.LoudnessWar)
            {
                if (!IsInGrindGame()) return false;
            }
            _synchroSummonsThisTurn++;
            return true;
        }

        private bool LoudnessWarSpSummon()
        {
            // Lv6 Synchro: 2+ Tuners
            // Protects other Tuners from effects + Quick Effect copy GY triggers
            if (ShouldSkipComboForLethal()) return false;
            SynchroPriority preferred = GetPreferredSynchro();
            // Loudness War is best when we have many tuners to protect
            if (preferred != SynchroPriority.LoudnessWar && preferred != SynchroPriority.Back2Back)
            {
                if (!OpponentHasThreateningMonster()) return false;
            }
            // Check available tuners on field AND hand (Hand-Sync can use hand tuners too)
            int fieldTuners = Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && c.HasType(CardType.Tuner));
            int handTuners = Bot.Hand.Count(c => c != null && c.HasType(CardType.Tuner) && !IsHandTrap(c) && IsKewlTuneMonster(c));
            if (fieldTuners + handTuners < 2) return false;
            _synchroSummonsThisTurn++;
            return true;
        }

        private bool ZalenSpSummon()
        {
            // Lv7 Synchro: 1 Tuner + 1+ Synchro Monsters
            // Counter-negate โ€” good against chain-heavy opponents
            if (_tunerOnlyLock) return false;
            if (ShouldSkipComboForLethal()) return false;
            if (!IsOpponentThreatening() && Enemy.GetMonsterCount() < 1) return false;
            _synchroSummonsThisTurn++;
            return true;
        }

        private bool Back2BackSpSummon()
        {
            // Lv10 Synchro: 1 Tuner + 1+ Synchro Monsters
            // Primary boss โ€” double attack + Quick Effect SS tuner + Synchro (2x/turn)
            if (ShouldSkipComboForLethal()) return false;
            // Only summon Back2Back when preferred or going for game
            SynchroPriority preferred = GetPreferredSynchro();
            if (preferred != SynchroPriority.Back2Back && preferred != SynchroPriority.ChaosAngel)
            {
                if (!IsOpponentThreatening() && !NeedsBoardPresence())
                    return false;
            }
            _synchroSummonsThisTurn++;
            return true;
        }

        private bool ChaosAngelSpSummon()
        {
            // Lv10 Synchro: 1 Tuner + 1 LIGHT/DARK non-Tuner (or treat as Tuner)
            // Banish on SS + protection based on material attributes
            if (_tunerOnlyLock) return false;
            if (ShouldSkipComboForLethal()) return false;
            // Only summon when opponent has threats worth banishing
            if (!IsOpponentThreatening()) return false;
            _synchroSummonsThisTurn++;
            return true;
        }

        private bool GenericSynchroSummonCheck()
        {
            if (ShouldSkipComboForLethal()) return false;
            if (_tunerOnlyLock && Card != null && !Card.HasType(CardType.Tuner)) return false;
            // Only summon generic Synchros when preferred is Generic or we need board presence
            SynchroPriority preferred = GetPreferredSynchro();
            if (preferred != SynchroPriority.Generic && preferred != SynchroPriority.TrackMaker)
            {
                if (!NeedsBoardPresence() && !IsInGrindGame()) return false;
            }
            return true;
        }

        // ====================================================================================================
        //  OnSelectCard โ€” COMPREHENSIVE CARD SELECTION HANDLER
        // ====================================================================================================

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // ===== HANDLE HINT-BASED SELECTIONS BEFORE CHECKING Card =====
            // Some hints (502, 509, 512, 533) can fire when Card == null

            // hint 512: Synchro Material selection from hand (Hand-Sync)
            if (hint == 512)
            {
                // Sort by Hand-Sync priority: prefer Kwtune monsters, NEVER hand traps
                var handCards = cards.Where(c => c != null && c.Location == CardLocation.Hand).ToList();
                if (handCards.Count > 0)
                {
                    var sorted = handCards.OrderBy(c => GetHandSyncPriority(c)).ToList();
                    // Only select Kwtune monsters, never hand traps
                    var kwtuneMats = sorted.Where(c => !IsHandTrap(c) && !IsAceCard(c)).ToList();
                    if (kwtuneMats.Count >= min)
                        return Util.CheckSelectCount(kwtuneMats, cards, min, max);
                }
                // If selecting field materials for Synchro
                var fieldCards = cards.Where(c => c != null && c.Location == CardLocation.MonsterZone).ToList();
                if (fieldCards.Count > 0)
                {
                    var sorted = fieldCards.OrderBy(c => GetMaterialPriority(c)).ToList();
                    return Util.CheckSelectCount(sorted, cards, min, max);
                }
            }

            // hint 533: Link Material โ€” protect Ace cards (shouldn't normally happen but safety)
            if (hint == 533)
            {
                var safe = cards.Where(c => c != null && !IsAceCard(c)).ToList();
                if (safe.Count >= min) return Util.CheckSelectCount(safe, cards, min, max);
            }

            // hint 502: Destroy target โ€” choose wisely
            if (hint == 502)
            {
                // If we're choosing enemy cards to destroy
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyCards.Count > 0)
                {
                    var prioritized = enemyCards.OrderByDescending(c =>
                    {
                        if (OpponentFloodgateCards.Contains(c.Id)) return 10000;
                        if (c.IsMonster() && c.IsFaceup() && !c.IsDisabled() && c.Attack >= 2500) return 8000;
                        if (c.HasType(CardType.Continuous) && c.IsFaceup()) return 7000;
                        if (c.IsMonster() && c.IsFaceup()) return c.Attack;
                        if (c.IsFacedown()) return 500;
                        return 100;
                    }).ToList();
                    return Util.CheckSelectCount(prioritized, cards, min, max);
                }

                // If we are forced to choose our own cards to destroy (self-pop)
                var ourCards = cards.Where(c => c != null && c.Controller == 0).ToList();
                if (ourCards.Count > 0)
                {
                    var prioritized = ourCards.OrderBy(c =>
                    {
                        if (IsAceCard(c)) return 99999; // NEVER destroy Ace cards if possible
                        if (c.Id == CardId.PSYFrameDriver) return 1; // best target to tribute/destroy
                        if (IsHandTrap(c)) return 5;
                        if (IsKewlTuneMonster(c)) return 10 + GetMaterialPriority(c);
                        if (c.IsFacedown()) return 50;
                        return 100;
                    }).ToList();
                    return Util.CheckSelectCount(prioritized, cards, min, max);
                }
            }

            // hint 509: Special Summon target โ€” prefer best monsters
            // CRITICAL: Filter by location! Cue SS from Deck, but engine may offer Hand+Deck.
            // Selecting a Hand card for a Deck-SS effect causes a CRASH.
            if (hint == 509)
            {
                var revivable = cards.Where(c => c != null && c.IsMonster()).ToList();
                if (revivable.Count > 0)
                {
                    // If options come from mixed locations (Deck+Hand), prefer Deck-only
                    bool hasDeck = revivable.Any(c => c.Location == CardLocation.Deck);
                    bool hasGrave = revivable.Any(c => c.Location == CardLocation.Grave);
                    var filtered = revivable;
                    if (hasDeck && revivable.Any(c => c.Location == CardLocation.Hand))
                        filtered = revivable.Where(c => c.Location == CardLocation.Deck).ToList();

                    // Prefer Kwtune combo pieces, then by ATK
                    var sorted = filtered.OrderByDescending(c =>
                    {
                        if (IsAceCard(c)) return 5000 + c.Attack;
                        if (IsKewlTuneSynchro(c)) return 3000 + c.Attack;
                        if (IsKewlTuneMonster(c)) return 2000;
                        return c.Attack;
                    }).ToList();
                    if (sorted.Count >= min)
                        return Util.CheckSelectCount(sorted, cards, min, max);
                }
            }

            // hint 506: Add to hand (search) โ€” prefer combo pieces we're missing
            if (hint == 506)
            {
                var searchable = cards.Where(c => c != null).ToList();
                if (searchable.Count > 0)
                {
                    var sorted = searchable.OrderByDescending(c =>
                    {
                        // Prefer cards we don't have
                        if (c.Id == CardId.KewlTuneSynchro && !Bot.HasInHand(CardId.KewlTuneSynchro)) return 100;
                        if (c.Id == CardId.JJKewlTune && !Bot.HasInHand(CardId.JJKewlTune) && !Bot.HasInSpellZone(CardId.JJKewlTune)) return 90;
                        if (c.Id == CardId.KewlTuneCue && !Bot.HasInHand(CardId.KewlTuneCue)) return 80;
                        if (c.Id == CardId.KewlTuneReco && !Bot.HasInHand(CardId.KewlTuneReco)) return 70;
                        if (c.Id == CardId.KewlTuneMix && !Bot.HasInHand(CardId.KewlTuneMix)) return 60;
                        if (IsKewlTuneMonster(c)) return 50;
                        return 10;
                    }).ToList();
                    return Util.CheckSelectCount(sorted, cards, min, max);
                }
            }

            // ===== CARD-SPECIFIC SELECTIONS (Card must be non-null) =====
            if (Card == null) return base.OnSelectCard(cards, min, max, hint, cancelable);

            // --- Pot of Prosperity: banish from Extra Deck ---
            if (Card.Id == CardId.PotOfProsperity && cards.All(c => c.Location == CardLocation.Extra))
            {
                // Banish generic/duplicate Synchros, keep key ones
                var priority = new[] {
                    CardId.WindPegasusIgnister, CardId.EnigmasterPackbit,
                    CardId.GoldenCloudBeastMalong, CardId.VisasAmritara,
                    CardId.ZalenTheShackledDragon,
                    CardId.KewlTuneRemix, CardId.KewlTuneTrackMaker
                };
                var result = new List<ClientCard>();
                foreach (int id in priority)
                    foreach (var c in cards.Where(c2 => c2.Id == id && !result.Contains(c2)))
                        if (result.Count < max) result.Add(c);
                if (result.Count >= min) return result.Take(max).ToList();
            }

            // --- Synchro Overtake: reveal Synchro from ED, then get material ---
            if (Card.Id == CardId.SynchroOvertake)
            {
                var extraSynchros = cards.Where(c => c.Location == CardLocation.Extra).ToList();
                if (extraSynchros.Count > 0)
                {
                    // Prefer RS (has Reco as named material) โ’ search Reco
                    ClientCard target = extraSynchros.FirstOrDefault(c => c.Id == CardId.KewlTuneRS)
                                     ?? extraSynchros.FirstOrDefault(c => c.Id == CardId.KewlTuneCrackle)
                                     ?? extraSynchros.FirstOrDefault(c => c.Id == CardId.KewlTuneRemix)
                                     ?? extraSynchros.FirstOrDefault();
                    if (target != null) return new List<ClientCard> { target };
                }
                // Select material target from deck/GY
                ClientCard monster = cards.FirstOrDefault(c => c.Id == CardId.KewlTuneReco)
                                  ?? cards.FirstOrDefault(c => c.Id == CardId.KewlTuneClip)
                                  ?? cards.FirstOrDefault(c => c.Id == CardId.KewlTuneMix)
                                  ?? cards.FirstOrDefault(c => c.Id == CardId.KewlTuneRotary);
                if (monster != null) return new List<ClientCard> { monster };
            }

            // --- KT Synchro spell: search target ---
            if (Card.Id == CardId.KewlTuneSynchro)
            {
                // Search: prefer JJ if we don't have it, then combo pieces
                if (!Bot.HasInSpellZone(CardId.JJKewlTune) && !Bot.HasInHand(CardId.JJKewlTune))
                {
                    var jj = cards.FirstOrDefault(c => c.Id == CardId.JJKewlTune);
                    if (jj != null) return new List<ClientCard> { jj };
                }
                // Otherwise search combo monsters
                var searchTarget = cards.FirstOrDefault(c => c.Id == CardId.KewlTuneCue && !Bot.HasInHand(CardId.KewlTuneCue))
                    ?? cards.FirstOrDefault(c => c.Id == CardId.KewlTuneReco && !Bot.HasInHand(CardId.KewlTuneReco))
                    ?? cards.FirstOrDefault(c => c.Id == CardId.KewlTuneMix && !Bot.HasInHand(CardId.KewlTuneMix))
                    ?? cards.FirstOrDefault(c => IsKewlTuneMonster(c));
                if (searchTarget != null) return new List<ClientCard> { searchTarget };
            }

            // --- JJ KewlTune: tribute lowest priority tuner ---
            if (Card.Id == CardId.JJKewlTune)
            {
                var tuners = cards.Where(c => c.Location == CardLocation.MonsterZone
                    && c.HasType(CardType.Tuner) && !IsAceCard(c)).ToList();
                if (tuners.Count > 0)
                    return new List<ClientCard> { tuners.OrderBy(c => GetMaterialPriority(c)).First() };
            }

            // --- Fidraulis Harmonia: reveal Synchros from ED ---
            if (Card.Id == CardId.FidraulisHarmonia && cards.All(c => c.Location == CardLocation.Extra))
            {
                // Reveal as many as possible to maximize effects (2+=SS, 4+=dump, 6=destroy)
                return cards.Take(max).ToList();
            }

            // --- RS: select tuner from GY to banish for negate ---
            if (Card.Id == CardId.KewlTuneRS)
            {
                // For GY banish cost: prefer expendable tuners
                var gyTuners = cards.Where(c => c.HasType(CardType.Tuner) && c.Location == CardLocation.Grave).ToList();
                if (gyTuners.Count > 0)
                {
                    var best = gyTuners.OrderBy(c => GetMaterialPriority(c)).First();
                    return new List<ClientCard> { best };
                }
                // For negate target
                if (cards.Any(c => c.Controller == 1))
                {
                    var target = GetHighestPriorityMonsterTarget()
                        ?? cards.FirstOrDefault(c => c.Controller == 1 && c.IsFaceup());
                    if (target != null && cards.Contains(target))
                        return new List<ClientCard> { target };
                }
            }

            // --- Duelist Genesis: select Synchro+Tuner from GY ---
            if (Card.Id == CardId.DuelistGenesis)
            {
                // Select a Synchro from GY to banish
                if (cards.All(c => c.Location == CardLocation.Grave))
                {
                    var synchros = cards.Where(c => c.HasType(CardType.Synchro)).ToList();
                    if (synchros.Count > 0)
                    {
                        // Banish least useful Synchro
                        var target = synchros.OrderBy(c => GetMaterialPriority(c)).First();
                        return new List<ClientCard> { target };
                    }
                }
            }

            // --- Loudness War: select Kwtune monster from GY to banish + copy effect ---
            if (Card.Id == CardId.KewlTuneLoudnessWar)
            {
                var gyKwtune = cards.Where(c => c != null && IsKewlTuneMonster(c) && c.Location == CardLocation.Grave).ToList();
                if (gyKwtune.Count > 0)
                {
                    ClientCard target = null;
                    // Choose based on which GY trigger is most useful right now
                    if (Enemy.GetMonsterCount() > 0)
                        target = gyKwtune.FirstOrDefault(c => c.Id == CardId.KewlTuneMix);       // Destroy monster
                    if (target == null && Enemy.GetSpellCount() > 0)
                        target = gyKwtune.FirstOrDefault(c => c.Id == CardId.KewlTuneReco);      // Destroy S/T
                    if (target == null)
                        target = gyKwtune.FirstOrDefault(c => c.Id == CardId.KewlTuneCue)         // Excavate/banish
                              ?? gyKwtune.FirstOrDefault(c => c.Id == CardId.KewlTuneClip)        // Banish ED
                              ?? gyKwtune.FirstOrDefault(c => c.Id == CardId.KewlTuneRotary)      // Search
                              ?? gyKwtune.First();
                    if (target != null) return new List<ClientCard> { target };
                }
            }

            // --- Back2Back: select Tuner from GY/banishment to add/SS ---
            if (Card.Id == CardId.KewlTuneBack2Back)
            {
                var tuners = cards.Where(c => c != null && c.IsMonster()
                    && c.HasType(CardType.Tuner) && c.Level != 10).ToList();
                if (tuners.Count > 0)
                {
                    // Prefer Synchro Tuners (for Synchro climb) > main deck Tuners with effects
                    var best = tuners.OrderByDescending(c =>
                    {
                        if (IsKewlTuneSynchro(c)) return 3000 + c.Level * 100;
                        if (IsKewlTuneMonster(c)) return 2000;
                        return c.Attack;
                    }).First();
                    AI.SelectCard(best);
                    return new List<ClientCard> { best };
                }
            }

            // --- Remix: select 2 non-Synchro Tuners from GY ---
            if (Card.Id == CardId.KewlTuneRemix)
            {
                var gyTuners = cards.Where(c => c != null && c.IsMonster()
                    && c.HasType(CardType.Tuner) && !c.HasType(CardType.Synchro)
                    && c.Location == CardLocation.Grave).ToList();
                if (gyTuners.Count >= 2)
                {
                    // Select 2 best โ€” prefer combo pieces
                    var sorted = gyTuners.OrderByDescending(c =>
                    {
                        if (c.Id == CardId.KewlTuneCue) return 100;
                        if (c.Id == CardId.KewlTuneReco) return 90;
                        if (c.Id == CardId.KewlTuneMix) return 80;
                        if (c.Id == CardId.KewlTuneRotary) return 70;
                        return 10;
                    }).Take(max).ToList();
                    return Util.CheckSelectCount(sorted, cards, min, max);
                }
            }

            // --- Generic Kwtune card selection by priority ---
            if (IsKewlTuneMonster(Card) || Card.Id == CardId.KewlTuneBack2Back)
            {
                // Sort by tuner combo priority
                var sorted = cards.OrderBy(c => GetNormalSummonPriority(c)).ToList();
                if (sorted.Count >= min) return Util.CheckSelectCount(sorted, cards, min, max);
            }

            // --- Generic enemy targeting โ€” floodgate first ---
            if (cards.Any(c => c.Controller == 1))
            {
                var enemy = cards.FirstOrDefault(c => OpponentFloodgateCards.Contains(c.Id) && c.Controller == 1)
                         ?? cards.FirstOrDefault(c => c.Controller == 1 && c.IsMonster() && c.IsFaceup())
                         ?? cards.FirstOrDefault(c => c.Controller == 1 && c.Location == CardLocation.SpellZone && c.IsFaceup())
                         ?? cards.FirstOrDefault(c => c.Controller == 1);
                if (enemy != null) return new List<ClientCard> { enemy };
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        // ====================================================================================================
        //  SYNCHRO MATERIAL SELECTION โ€” PROTECT HAND TRAPS
        // ====================================================================================================

        public override IList<ClientCard> OnSelectSynchroMaterial(IList<ClientCard> cards, int sum, int min, int max)
        {
            // Sort by material priority: use expendable Kwtune monsters first
            // NEVER use hand traps or Ace cards as material
            var sorted = cards.Where(c => c != null)
                .OrderBy(c => GetMaterialPriority(c))
                .ToList();

            // Try to find a valid combination that avoids hand traps entirely
            var safe = sorted.Where(c => !IsHandTrap(c) && !IsAceCard(c)).ToList();
            if (safe.Count >= min)
                return Util.CheckSelectCount(safe, cards, min, max);

            // Fallback: at least avoid Ace cards
            var noAce = sorted.Where(c => !IsAceCard(c)).ToList();
            if (noAce.Count >= min)
                return Util.CheckSelectCount(noAce, cards, min, max);

            return base.OnSelectSynchroMaterial(cards, sum, min, max);
        }

        // ====================================================================================================
        //  OTHER OVERRIDES
        // ====================================================================================================

        public override int OnSelectOption(IList<long> options)
        {
            // Rotary GY trigger: option 0 = add Tuner, option 1 = search KT spell/trap
            if (Card != null && Card.IsCode(CardId.KewlTuneRotary) && options.Count >= 2)
            {
                // Prefer search spell/trap if we need KT Synchro
                if (!Bot.HasInHand(CardId.KewlTuneSynchro) && Bot.GetRemainingCount(CardId.KewlTuneSynchro, 3) > 0)
                    return 1; // search spell
                return 0; // add tuner (more flexible)
            }

            // Back2Back: option 0 = add to hand, option 1 = Special Summon
            if (Card != null && Card.IsCode(CardId.KewlTuneBack2Back) && options.Count >= 2)
            {
                // Prefer SS if we can then Synchro climb
                return 1; // SS for Synchro follow-up
            }

            return 0;
        }

        public override bool OnSelectYesNo(long desc) => true;

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // DEF for low-ATK utility monsters
            if ((cardId == CardId.KewlTuneRotary || cardId == CardId.KewlTuneClip)
                && positions.Contains(CardPosition.FaceUpDefence))
                return CardPosition.FaceUpDefence;
            // ATK for boss monsters
            if ((cardId == CardId.KewlTuneBack2Back || cardId == CardId.ChaosAngel
                || cardId == CardId.KewlTuneRS || cardId == CardId.FidraulisHarmonia)
                && positions.Contains(CardPosition.FaceUpAttack))
                return CardPosition.FaceUpAttack;
            // DEF for wall monsters
            if ((cardId == CardId.KewlTuneLoudnessWar || cardId == CardId.KewlTuneTrackMaker)
                && positions.Contains(CardPosition.FaceUpDefence))
                return CardPosition.FaceUpDefence;
            return base.OnSelectPosition(cardId, positions);
        }

        public override int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
            if (player == 0 && location == CardLocation.MonsterZone)
            {
                // Center position for Back2Back (for protection/visibility)
                if (cardId == CardId.KewlTuneBack2Back)
                {
                    if ((available & 0x4) > 0) return 0x4;  // center
                    if ((available & 0x2) > 0) return 0x2;
                    if ((available & 0x8) > 0) return 0x8;
                }
                // Edges for Remix (so it doesn't block middle)
                if (cardId == CardId.KewlTuneRemix)
                {
                    if ((available & 0x1) > 0) return 0x1;
                    if ((available & 0x10) > 0) return 0x10;
                }
            }
            return base.OnSelectPlace(cardId, player, location, available);
        }

        // ====================================================================================================
        //  BATTLE PHASE
        // ====================================================================================================

        public override ClientCard OnSelectAttacker(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            // Back2Back attacks first (highest ATK, double attack)
            ClientCard back2back = attackers.FirstOrDefault(c => c != null
                && c.IsCode(CardId.KewlTuneBack2Back) && !c.Attacked);
            if (back2back != null) return back2back;

            // Then highest ATK monsters
            var sorted = attackers.Where(c => c != null && !c.Attacked && c.Attack > 0)
                .OrderByDescending(c => c.Attack).ToList();
            return sorted.Count > 0 ? sorted.First() : base.OnSelectAttacker(attackers, defenders);
        }

        public override BattlePhaseAction OnSelectAttackTarget(ClientCard attacker, IList<ClientCard> defenders)
        {
            if (defenders.Count == 0) return AI.Attack(attacker, null);

            int GetDefenseValue(ClientCard c)
            {
                if (c == null) return 0;
                return c.IsDefense() ? c.Defense : c.Attack;
            }

            // Attack monsters we can destroy (highest value first for max damage)
            var defeatable = defenders.Where(d => attacker.Attack > GetDefenseValue(d))
                .OrderByDescending(d => GetDefenseValue(d)).ToList();
            if (defeatable.Count > 0) return AI.Attack(attacker, defeatable.First());

            // Direct attack if no defenders can block
            if (defenders.All(d => d == null)) return AI.Attack(attacker, null);

            // Trade only if we're ahead on LP
            var equal = defenders.Where(d => attacker.Attack == GetDefenseValue(d) && d.IsAttack()).ToList();
            if (equal.Count > 0 && Bot.LifePoints > Enemy.LifePoints)
                return AI.Attack(attacker, equal.First());

            return null; // Don't attack into something stronger
        }

        public override bool OnPreBattleBetween(ClientCard attacker, ClientCard defender)
        {
            // Don't attack with low-ATK utility monsters into face-down defenders
            if (attacker.Attack <= 500 && defender != null && defender.IsFacedown())
                return false;

            // Safety: don't attack if defender ATK > attacker ATK (face-up ATK position)
            if (defender != null && defender.IsFaceup() && defender.IsAttack()
                && defender.Attack >= attacker.Attack)
                return false;

            return base.OnPreBattleBetween(attacker, defender);
        }

        // ====================================================================================================
        //  MONSTER REPOS โ€” Safe positioning
        // ====================================================================================================

        private bool MonsterReposLogic()
        {
            // Custom repos to protect our monsters
            bool enemyEmpty = Enemy.GetMonsterCount() == 0;

            foreach (ClientCard monster in Bot.GetMonsters())
            {
                if (monster == null || !monster.IsFaceup()) continue;
                if (IsAceCard(monster)) continue; // Don't change Ace position

                if (monster.IsAttack())
                {
                    // Switch to DEF if unsafe
                    if (!enemyEmpty && monster.Attack <= 1000)
                    {
                        bool enemyStronger = Enemy.GetMonsters().Any(c => c != null
                            && c.IsFaceup() && c.IsAttack() && c.Attack >= monster.Attack);
                        if (enemyStronger && monster.Defense > 0) return true;
                    }
                }
                else // defense
                {
                    // Switch to ATK if safe
                    if (enemyEmpty && monster.Attack > 0) return true;
                    if (monster.Attack >= 2000 && !Enemy.GetMonsters().Any(c => c != null
                        && c.IsFaceup() && c.Attack >= monster.Attack))
                        return true;
                }
            }
            return false;
        }
    }

    // ====================================================================================================
    //  EXPERT MODE WRAPPER
    // ====================================================================================================
    [Deck("Expert_2026_Kwtune", "2026_Kwtune")]
    public class ExpertKwtuneExecutor : _2026_KwtuneExecutor
    {
        private string _duelId;
        public ExpertKwtuneExecutor(GameAI ai, Duel duel) : base(ai, duel)
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

    // ====================================================================================================
    //  NEURAL MODE WRAPPER
    // ====================================================================================================
}
