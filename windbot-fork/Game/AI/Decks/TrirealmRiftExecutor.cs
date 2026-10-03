using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Decks;
using WindBot.Game.AI.DecisionEngine;
using WindBot.Game.AI.Plugins;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("TrirealmRift", "TrirealmRift")]
    public class TrirealmRiftExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Deck - Trirealm Rift Monsters (All 6 Attributes)
            public const int TrirealmRiftOfEmptinessGehenna = 100458031; // Lv 1 DARK (Free SS / Search 2 Monsters)
            public const int TrirealmRiftOfSkySheol = 100458032;        // Lv 2 LIGHT (SS with Trirealm / Search 2 S/T)
            public const int TrirealmRiftOfBlueTuonela = 100458033;     // Lv 3 WATER (SS if outnumbered / SS Lv 5+)
            public const int TrirealmRiftOfScarletNaraka = 100458034;   // Lv 4 FIRE (SS on enemy SS / Quick SS Lv 5+ in enemy MP / Small World Bridge!)
            public const int SkyThunderTrirealmRiftYomi = 100458035;    // Lv 5 WIND (Core Boss / Hand Search / Quick Negate)
            public const int BurialSummitTrirealmRiftHelheim = 100458036; // Lv 6 EARTH (Hand SS Boss / Full Field Protection)
            public const int MadTempestTrirealmRiftPloutonion = 100458037; // Lv 7 WATER (Hand Draw 2 / Quick Spin)
            public const int TrirealmRiftDarkness = 100458038;          // Lv 8 DARK (3000 ATK Finisher / Quick Pop / Battle Immune)

            // Main Deck - OTK Engine & Extenders
            public const int GrenMajuDaEiza = 36584821;                // Lv 3 FIRE Fiend (Supreme Finisher: ATK = Banished x 400)
            public const int GizmekOrochi = 71197066;                   // Lv 8 DARK Machine (Quick SS Banish 8 / Pop / Rank 8 mat)
            public const int GamecielKaiju = 55063751;                // Lv 8 WATER Aqua (2200 ATK Kaiju / Tributes enemy boss / Level 8 Rank 8 fuel)

            // Spells & Traps
            public const int TrirealmRiftTerritoryValvols = 100458039; // Field Spell (Banish 5 / Search 1 / Deck 0 Turn Lockout)
            public const int Terraforming = 73628505;                  // Normal Spell (Search Valvols)
            public const int TrirealmRiftGospel = 100458040;          // Continuous Spell (Banish 5 / Extra NS for Gren Maju! / Morph Lv 4-)
            public const int TrirealmRiftJudgment = 100458041;        // Continuous Trap (Summon Intercept / Opponent ATK 0)
            public const int AshBlossom = 14558127;                    // Handtrap (Negates search/draw/mill/SS from deck)
            public const int DimensionShifter = 91800273;              // Handtrap (Banishes all cards sent to GY for 2 turns!)
            public const int DrollLockBird = 94145021;                 // Handtrap (Shuts down adding/drawing from deck for the turn!)
            public const int PotOfExtravagance = 49238328;             // Normal Spell (Banish 6 Extra Deck face-down -> Draw 2! +2400 ATK fuel)
            public const int PotOfProsperity = 84211599;               // Normal Spell (Banish 6 from Extra Deck -> Excavate 6 & pick 1! +2400 ATK fuel)
            public const int PotOfDesires = 35261759;                  // Normal Spell (Banish 10 from Deck face-down -> Draw 2! +4000 ATK fuel)
            public const int SmallWorld = 89558743;                    // Normal Spell (100% Gren Maju search via Naraka)
            public const int ForbiddenLance = 27243130;                // Quick-Play Spell (-800 ATK, Immune to all Spells & Traps -> protects Gren Maju!)
            public const int HarpieFeatherDuster = 18144506;           // Normal Spell (Destroys all enemy S/T)
            public const int LightningStorm = 14532163;                // Normal Spell (Destroys all enemy S/T or all Attack monsters!)
            public const int Raigeki = 12580477;                      // Normal Spell (Destroys all opponent monsters)
            public const int DarkRulerNoMore = 54693926;               // Normal Spell (Negate all opponent monsters with NO monster response!)
            public const int CalledByTheGrave = 24224830;              // Quick-Play Spell (Banish 1 enemy GY monster & negate)
            public const int CosmicCyclone = 8267140;                  // Quick-Play Spell (Banish 1 Spell/Trap / Eternal Soul & Village wipe!)
            public const int ForbiddenDroplet = 24299458;               // Quick-Play Spell (Negate all enemy monsters without response!)
            public const int InfiniteImpermanence = 10045474;          // Normal Trap (Monster Negate from hand/field)
            public const int EvenlyMatched = 15693423;                 // Normal Trap (Face-down banish board breaker & Gren Maju / Darkness fuel!)
            public const int AccesscodeTalker = 86066372;              // Link 4 (5300 ATK Board Wipe Finisher)

            // Extra Deck - Rank 8 Toolbox & OTK Backup
            public const int Dingirsu = 93854893;                      // Rank 8 (Non-target send 1 enemy card to GY / Detach to protect cards!)
            public const int HopeHarbinger = 63767246;                 // Rank 8 (Spell negate / Attach as material / Attack redirect)
            public const int PhotonLord = 8165596;                     // Rank 8 (Quick monster effect negate)
            public const int Number97Draglubion = 28400508;            // Rank 8 (Untargetable / Detach to SS Dragon Number + attach material)
            public const int Number100NumeronDragon = 57314798;        // Rank 1 (Backup OTK: 9,000 - 17,000 ATK Lethal Strike!)
            public const int Sanaphond = 23085002;                     // Rank 8 (GY Shutdown Floodgate + Indestructible)
            public const int ChaosAngel = 22850702;                    // Synchro 10 (Non-Tuner LIGHT/DARK Synchro / Non-target banish + dual immunity)
            public const int RelinquishedAnima = 94259633;             // Link 1 (Gehenna fodder / Absorbs enemy monster)
            public const int DivineArsenalAAZEUS = 90448279;           // Rank 12 (Board Wipe in MP2)
            public const int SuperStarslayerTYPHON = 93039339;         // Rank 12 (Free 1-card summon / Disables 3000+ ATK / Bounce)
            public const int Number41Bagooska = 90590303;              // Rank 4 (Turn 1 Floodgate stall - DEFENCE ONLY)
            public const int TopologicZeroboros = 66403530;            // Link 4 (8,000+ ATK / Field Banish Wipe)
            public const int UnderworldGoddess = 98127546;             // Link 5 (Uses enemy boss as material / Field Negate)
            public const int SPLittleKnight = 29301450;                // Link 2 (Target banish / Dodge)
            public const int Linkuriboh = 41999284;                    // Link 1 (Gehenna fodder / Tributes to make enemy ATK 0!)
            public const int KnightmareUnicorn = 38342335;             // Link 3 (Spin removal)
            public const int KnightmarePhoenix = 2857636;              // Link 2 (Backrow pop)
        }

        private static readonly int[] TrirealmMonsters = {
            CardId.TrirealmRiftOfEmptinessGehenna,
            CardId.TrirealmRiftOfSkySheol,
            CardId.TrirealmRiftOfBlueTuonela,
            CardId.TrirealmRiftOfScarletNaraka,
            CardId.SkyThunderTrirealmRiftYomi,
            CardId.BurialSummitTrirealmRiftHelheim,
            CardId.MadTempestTrirealmRiftPloutonion,
            CardId.TrirealmRiftDarkness
        };

        internal TrirealmRiftPlugin Plugin { get; private set; }

        private bool _yomiSearchedThisTurn = false;
        private bool _helheimSummonedThisTurn = false;
        private bool _ploutonionDrawnThisTurn = false;
        private bool _valvolsSearchedThisTurn = false;
        private bool _gehennaSearchedThisTurn = false;
        private bool _sheolSearchedThisTurn = false;
        private bool _tuonelaSpSummonUsedThisTurn = false;
        private bool _gospelMorphedThisTurn = false;
        private bool _gizmekSummonedThisTurn = false;
        private bool _drnmUsedThisTurn = false;

        public TrirealmRiftExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // 0. Install Decoupled Domain Plugin
            DeckPlugin = new TrirealmRiftPlugin(this);
            Plugin = (TrirealmRiftPlugin)DeckPlugin;

            // ═══════════════════════════════════════════════════════════════
            //  TIER 0: QUICK COUNTERS, HANDTRAPS & DISRUPTIONS (Chain Priority)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.DimensionShifter, OnDimensionShifter);
            AddExecutor(ExecutorType.Activate, CardId.DrollLockBird, DefaultDrollAndLockBird);
            AddExecutor(ExecutorType.GoToBattlePhase, OnGoToBattlePhaseForEvenly);
            AddExecutor(ExecutorType.Activate, CardId.EvenlyMatched, OnEvenlyMatched);
            AddExecutor(ExecutorType.Activate, CardId.CosmicCyclone, OnCosmicCyclone);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, OnForbiddenDroplet);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, OnCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenLance, OnForbiddenLance);
            AddExecutor(ExecutorType.Activate, CardId.HopeHarbinger, OnHopeHarbingerActivate);
            AddExecutor(ExecutorType.Activate, CardId.PhotonLord, OnPhotonLordActivate);
            AddExecutor(ExecutorType.Activate, CardId.SkyThunderTrirealmRiftYomi, OnYomiQuickNegate);
            AddExecutor(ExecutorType.Activate, CardId.MadTempestTrirealmRiftPloutonion, OnPloutonionQuickSpin);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftDarkness, OnDarknessQuickDestroy);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftJudgment, OnTrirealmJudgment);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, DefaultInfiniteImpermanence);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfScarletNaraka, OnNarakaOpponentSummon);
            AddExecutor(ExecutorType.Activate, CardId.GizmekOrochi, OnGizmekOrochiQuick);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, OnSPLittleKnightActivate);
            AddExecutor(ExecutorType.Activate, CardId.Linkuriboh, OnLinkuribohActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 1: BOARD BREAKERS, POT ENGINES & KAIJU REMOVAL
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.DarkRulerNoMore, OnDarkRulerNoMore); // Unnegatable monster shutdown! MUST BE #1!
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, OnPotOfProsperity); // Excavate 6 cards for boss search!
            AddExecutor(ExecutorType.Activate, CardId.PotOfDesires, OnPotOfDesires); // Banish 10 face-down -> Draw 2 & +4000 ATK fuel!
            AddExecutor(ExecutorType.Activate, CardId.PotOfExtravagance, OnPotOfExtravagance);
            AddExecutor(ExecutorType.Activate, CardId.HarpieFeatherDuster, OnHarpieFeatherDuster);
            AddExecutor(ExecutorType.Activate, CardId.LightningStorm, OnLightningStorm);
            AddExecutor(ExecutorType.Activate, CardId.Raigeki, OnRaigeki);
            AddExecutor(ExecutorType.SpSummon, CardId.GamecielKaiju, OnGamecielKaijuSummon);
            AddExecutor(ExecutorType.Activate, CardId.SmallWorld, OnSmallWorld); // 100% Gren Maju search via Naraka!

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: ON-SUMMON BANISH TRIGGERS (Fuel for Gren Maju & Darkness)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfEmptinessGehenna, OnSummonBanishTrigger);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfSkySheol, OnSummonBanishTrigger);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfBlueTuonela, OnSummonBanishTrigger);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfScarletNaraka, OnSummonBanishTrigger);
            AddExecutor(ExecutorType.Activate, CardId.SkyThunderTrirealmRiftYomi, OnSummonBanishTrigger);
            AddExecutor(ExecutorType.Activate, CardId.BurialSummitTrirealmRiftHelheim, OnSummonBanishTrigger);
            AddExecutor(ExecutorType.Activate, CardId.MadTempestTrirealmRiftPloutonion, OnSummonBanishTrigger);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftDarkness, OnSummonBanishTrigger);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: SEARCH ENGINES, DRAW & BANISH ACCELERATORS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.Terraforming, OnTerraforming);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftTerritoryValvols, OnValvolsHandActivate);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftGospel, OnGospelHandActivate);
            AddExecutor(ExecutorType.Activate, CardId.MadTempestTrirealmRiftPloutonion, OnPloutonionHandDraw);
            AddExecutor(ExecutorType.Activate, CardId.SkyThunderTrirealmRiftYomi, OnYomiHandSearch);
            AddExecutor(ExecutorType.Activate, CardId.BurialSummitTrirealmRiftHelheim, OnHelheimQuickSummon);
            AddExecutor(ExecutorType.Activate, CardId.GizmekOrochi, OnGizmekOrochiSpSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: MAIN PHASE STARTERS, EXTENDERS & FIELD SEARCHES
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfEmptinessGehenna, OnGehennaHandSummon);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfEmptinessGehenna, OnGehennaFieldSearch);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfSkySheol, OnSheolHandSummon);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfSkySheol, OnSheolFieldSearch);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfBlueTuonela, OnTuonelaHandSummon);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfBlueTuonela, OnTuonelaFieldSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftTerritoryValvols, OnValvolsFieldSearch);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftGospel, OnGospelFieldMorph);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfScarletNaraka, OnNarakaHandSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 5: RANK 8 XYZ TOOLBOX, SYNCHRO & OTK BACKUP
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.HopeHarbinger, OnHopeHarbingerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.PhotonLord, OnPhotonLordSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Sanaphond, OnSanaphondSummon);
            AddExecutor(ExecutorType.Activate, CardId.Sanaphond, OnSanaphondActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.Dingirsu, OnDingirsuSummon);
            AddExecutor(ExecutorType.Activate, CardId.Dingirsu);
            AddExecutor(ExecutorType.SpSummon, CardId.ChaosAngel, OnChaosAngelSummon);
            AddExecutor(ExecutorType.Activate, CardId.ChaosAngel, OnChaosAngelActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.Number97Draglubion, OnDraglubionSummon);
            AddExecutor(ExecutorType.Activate, CardId.Number97Draglubion, OnDraglubionActivate);
            AddExecutor(ExecutorType.Activate, CardId.Number100NumeronDragon, OnNumeronDragonActivate);


            // Extra Deck Links & Sweep Finishes
            AddExecutor(ExecutorType.SpSummon, CardId.UnderworldGoddess, OnUnderworldGoddessSummon);
            AddExecutor(ExecutorType.Activate, CardId.UnderworldGoddess);
            AddExecutor(ExecutorType.SpSummon, CardId.TopologicZeroboros, OnZeroborosSummon);
            AddExecutor(ExecutorType.Activate, CardId.TopologicZeroboros);
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, OnAccesscodeTalkerSummon);
            AddExecutor(ExecutorType.Activate, CardId.AccesscodeTalker);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmareUnicorn, OnKnightmareUnicornSummon);
            AddExecutor(ExecutorType.Activate, CardId.KnightmareUnicorn, OnKnightmareUnicornActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmarePhoenix, OnKnightmarePhoenixSummon);
            AddExecutor(ExecutorType.Activate, CardId.KnightmarePhoenix, OnKnightmarePhoenixActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.SuperStarslayerTYPHON, OnTyphonSummon);
            AddExecutor(ExecutorType.Activate, CardId.SuperStarslayerTYPHON, OnTyphonActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.DivineArsenalAAZEUS, OnZeusSummon);
            AddExecutor(ExecutorType.Activate, CardId.DivineArsenalAAZEUS, OnZeusActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, OnSPLittleKnightSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Number41Bagooska, OnBagooskaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Linkuriboh, OnLinkuribohSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 6: NORMAL SUMMONS (Gren Maju, Darkness & Starters)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Summon, CardId.GrenMajuDaEiza, OnGrenMajuSummon);
            AddExecutor(ExecutorType.Summon, CardId.TrirealmRiftDarkness, OnDarknessSummon);
            AddExecutor(ExecutorType.Summon, CardId.BurialSummitTrirealmRiftHelheim, OnHelheimSummon);
            AddExecutor(ExecutorType.Summon, CardId.SkyThunderTrirealmRiftYomi, OnYomiSummon);
            AddExecutor(ExecutorType.Summon, CardId.MadTempestTrirealmRiftPloutonion, OnPloutonionSummon);
            AddExecutor(ExecutorType.Summon, CardId.TrirealmRiftOfScarletNaraka, OnGeneralNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.TrirealmRiftOfSkySheol, OnGeneralNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.TrirealmRiftOfEmptinessGehenna, OnGeneralNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.TrirealmRiftOfBlueTuonela, OnGeneralNormalSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 7: BACKROW SET & DEFENSIVE PLACEMENT
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, CardId.TrirealmRiftJudgment);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenLance);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 8: REPOSITIONING
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // Prefer Going Second:
            // 3x Dark Ruler No More, 2x Kaiju, Raigeki, Duster, and Gren Maju thrive going second with 6 cards,
            // breaking any established board and executing an unstoppable 8,000 - 14,000+ ATK OTK!
            return false;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _yomiSearchedThisTurn = false;
            _helheimSummonedThisTurn = false;
            _ploutonionDrawnThisTurn = false;
            _valvolsSearchedThisTurn = false;
            _gehennaSearchedThisTurn = false;
            _sheolSearchedThisTurn = false;
            _tuonelaSpSummonUsedThisTurn = false;
            _gospelMorphedThisTurn = false;
            _gizmekSummonedThisTurn = false;
            _drnmUsedThisTurn = false;
        }

        // ═══════════════════════════════════════════════════════════════
        // DISRUPTIONS & QUICK COUNTERS
        // ═══════════════════════════════════════════════════════════════

        private bool OnDimensionShifter()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // Activate when GY has 0 cards to banish all cards sent to GY for 2 turns!
            if (Bot.Graveyard.Count == 0)
            {
                Logger.DebugWriteLine("[DimensionShifter] Shifter activated! Locking GY and fueling banish zone.");
                return true;
            }
            return false;
        }

        private bool OnForbiddenDroplet()
        {
            if (Card.Location != CardLocation.Hand && Card.Location != CardLocation.SpellZone) return false;
            // If DRNM was already used this turn, all opponent monsters are already negated!
            if (_drnmUsedThisTurn) return false;

            // Target face-up effect monsters that are not already disabled
            var oppTargets = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup() && !m.IsDisabled() && m.HasType(CardType.Effect)).ToList();
            if (oppTargets.Count == 0) return false;

            // On our turn: activate in MP1 or Battle Phase before combos if opponent has any negators or bosses
            if (Duel.Player == 0)
            {
                if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.BattleStart && Duel.Phase != DuelPhase.Battle)
                    return false;
            }

            // Prioritize high-threat targets (Negators, quick banishers like ABC-Dragon Buster, etc.)
            var priorityTargets = oppTargets.Where(m => CardIntelligence.IsKnownNegator(m.Id) 
                || m.Id == 1561110 // ABC-Dragon Buster
                || m.Id == 10443957 // Cyber Dragon Infinity
                || m.Id == 4280258  // Apollousa
                || m.Id == 1508649  // Altergeist Hexstia
                || m.Id == 89538537 // Altergeist Silquitous
                || m.Id == 41721210 // Dark Magician the Dragon Knight
                || m.Id == 21887175 // Mekk-Knight Crusadia Avramax
                || m.Id == 50954680 // Crystal Wing
                || m.Id == 59822133 // Blue-Eyes Spirit Dragon
                || m.Attack >= 2500
            ).ToList();

            var targetsToNegate = priorityTargets.Count > 0 ? priorityTargets : oppTargets;

            // Choose expendable cards to send: field spells, extra cards in hand/field, but protect Gren Maju, Darkness & sole starter
            int normalStartersInHand = Bot.Hand.Count(c => c != null && c.IsMonster() && c.Level <= 4);
            var sendFodder = Bot.Hand.Concat(Bot.GetMonsters()).Concat(Bot.GetSpells())
                .Where(c => c != null && c != Card 
                    && !c.IsCode(CardId.GrenMajuDaEiza) 
                    && !c.IsCode(CardId.TrirealmRiftDarkness)
                    && !(c.Location == CardLocation.Hand && c.IsMonster() && c.Level <= 4 && normalStartersInHand <= 1 && Bot.GetMonsterCount() == 0)
                    && !(c.Location == CardLocation.Hand && (c.IsCode(CardId.TrirealmRiftGospel) || c.IsCode(CardId.TrirealmRiftTerritoryValvols)) && Bot.Hand.Count(h => h.IsCode(c.Id)) == 1))
                .OrderBy(c => {
                    if (c.Location == CardLocation.SpellZone) return 1;
                    if (c.IsCode(CardId.GizmekOrochi)) return 2;
                    if (c.IsCode(CardId.AshBlossom, CardId.InfiniteImpermanence)) return 3;
                    if (c.IsSpell()) return 4;
                    if (c.IsMonster() && c.Level > 4) return 5;
                    return 10;
                })
                .Take(targetsToNegate.Count)
                .ToList();

            if (sendFodder.Count > 0)
            {
                Logger.DebugWriteLine($"[ForbiddenDroplet] Activating Droplet! Sending {sendFodder.Count} cards to negate {targetsToNegate.Count} enemy threats.");
                AI.SelectCard(sendFodder);
                AI.SelectNextCard(targetsToNegate);
                return true;
            }

            return false;
        }

        private bool OnCosmicCyclone()
        {
            if (Card.Location != CardLocation.Hand && Card.Location != CardLocation.SpellZone) return false;
            if (Bot.LifePoints <= 1000) return false;

            // On our turn, NEVER proactively activate Cosmic Cyclone from hand during Draw or Standby Phase!
            // Wait for Main Phase 1 so Dark Ruler No More / Board Breakers can resolve first!
            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Draw || Duel.Phase == DuelPhase.Standby))
            {
                if (Duel.LastChainPlayer != 1) return false;
            }

            // 1. Reactive Chain (against spell/trap activation)
            if (Duel.LastChainPlayer == 1)
            {
                var chainCard = Util.GetLastChainCard();
                if (chainCard != null && (chainCard.Location == CardLocation.SpellZone || chainCard.HasType(CardType.Continuous) || chainCard.HasType(CardType.Field)))
                {
                    AI.SelectCard(chainCard);
                    return true;
                }
            }

            // 2. High Threat Floodgates / Continuous Cards
            var prioritySpells = Enemy.GetSpells().Where(s => s != null && s.IsFaceup() && (
                s.IsCode(48680970) // Eternal Soul
                || s.IsCode(68462976) // Secret Village of the Spellcasters
                || s.IsCode(27541563) // Altergeist Protocol
                || s.IsCode(66399653) // Union Hangar
                || s.IsCode(62089826) // True Light
                || s.IsCode(53936268) // Altergeist Personal Spoofing
                || s.IsCode(35146019) // Altergeist Manifestation
                || s.IsCode(47222536) // Dark Magical Circle
                || CardIntelligence.IsFloodgate(s.Id)
            )).ToList();

            if (prioritySpells.Count > 0)
            {
                AI.SelectCard(prioritySpells);
                return true;
            }

            // 3. Faceup continuous / field / trap
            var faceupSpells = Enemy.GetSpells().Where(s => s != null && s.IsFaceup()).ToList();
            if (faceupSpells.Count > 0)
            {
                AI.SelectCard(faceupSpells);
                return true;
            }

            // 4. Set backrow on our turn before combos or enemy End Phase
            var setSpells = Enemy.GetSpells().Where(s => s != null && s.IsFacedown()).ToList();
            if (setSpells.Count > 0)
            {
                if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1)
                {
                    AI.SelectCard(setSpells);
                    return true;
                }
                if (Duel.Player == 1 && Duel.Phase == DuelPhase.End)
                {
                    AI.SelectCard(setSpells);
                    return true;
                }
            }

            return false;
        }

        private bool OnCalledByTheGrave()
        {
            if (Card.Location != CardLocation.Hand && Card.Location != CardLocation.SpellZone) return false;

            // 1. Reactive Chain Negation (against Handtraps, Ash Blossom, Veiler, Droll, Ghost Ogre, etc.)
            if (Duel.LastChainPlayer == 1)
            {
                var lastCard = Util.GetLastChainCard();
                if (lastCard != null)
                {
                    int targetCode = lastCard.GetNonAltartCode();
                    bool isTargetable = CardIntelligence.IsHandtrap(lastCard.Id) || CardIntelligence.IsHandtrap(targetCode)
                        || CardIntelligence.IsKnownNegator(lastCard.Id) || CardIntelligence.IsHighThreatChokepoint(lastCard.Id)
                        || lastCard.HasType(CardType.Monster);

                    if (isTargetable)
                    {
                        var match = Enemy.Graveyard.FirstOrDefault(c => c != null && (c.IsCode(targetCode) || c.IsCode(lastCard.Id)));
                        if (match != null)
                        {
                            AI.SelectCard(match);
                            return true;
                        }
                    }
                }
            }

            // 2. Proactive GY banish (White Stone of Ancients, Legend, Blue-Eyes White Dragon, Dark Magician)
            if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Main2)
            {
                var keyEnemyTargets = Enemy.Graveyard.Where(c => c != null && c.IsMonster() && (
                    c.Id == 71039903 // White Stone of Ancients
                    || c.Id == 79814787 // White Stone of Legend
                    || c.Id == 89631139 // Blue-Eyes White Dragon
                    || c.Id == 46986414 // Dark Magician
                    || c.Id == 7084129  // Magician's Rod
                    || c.Id == 97631303 // Magicians' Souls
                    || CardIntelligence.IsKnownNegator(c.Id)
                    || CardIntelligence.IsHighThreatChokepoint(c.Id)
                )).ToList();

                if (keyEnemyTargets.Count > 0)
                {
                    AI.SelectCard(keyEnemyTargets.First());
                    return true;
                }
            }

            return false;
        }

        private bool OnForbiddenLance()
        {
            if (Card.Location != CardLocation.Hand && Card.Location != CardLocation.SpellZone) return false;

            // 1. Reactive Chain: Protect Gren Maju or boss from opponent's Spells/Traps
            ClientCard lastCard = Util.GetLastChainCard();
            if (lastCard != null && lastCard.Controller == 1 && (lastCard.IsSpell() || lastCard.IsTrap()))
            {
                var protectTarget = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.IsCode(CardId.GrenMajuDaEiza))
                    ?? Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && (m.IsCode(CardId.SkyThunderTrirealmRiftYomi) || m.IsCode(CardId.BurialSummitTrirealmRiftHelheim) || m.IsCode(CardId.TrirealmRiftDarkness)))
                    ?? Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup());
                if (protectTarget != null)
                {
                    AI.SelectCard(protectTarget);
                    return true;
                }
            }

            // 2. Battle Phase: If Gren Maju is attacking and opponent has set backrow (e.g. Mirror Force, Torrential, Compulsory)
            if ((Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep) && Duel.Player == 0)
            {
                var maju = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.IsCode(CardId.GrenMajuDaEiza));
                if (maju != null && Enemy.GetSpellCount() > 0)
                {
                    AI.SelectCard(maju);
                    return true;
                }
            }

            return false;
        }

        private bool OnHopeHarbingerActivate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            // Negate opponent Spell activation
            return Duel.LastChainPlayer == 1 && Util.GetLastChainCard() != null && Util.GetLastChainCard().IsSpell();
        }

        private bool OnPhotonLordActivate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            // Negate opponent monster effect
            return Duel.LastChainPlayer == 1 && Util.GetLastChainCard() != null && Util.GetLastChainCard().HasType(CardType.Monster);
        }

        private bool OnYomiQuickNegate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Duel.LastChainPlayer != 1) return false;

            var lastCard = Util.GetLastChainCard();
            if (lastCard == null || !lastCard.HasType(CardType.Monster)) return false;

            int enemyAttr = (int)lastCard.Attribute;
            return Bot.Banished.Any(c => c != null && c.IsFacedown() && IsTrirealmMonster(c) && ((int)c.Attribute & enemyAttr) > 0);
        }

        private bool OnPloutonionQuickSpin()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            var enemyMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget()).ToList();
            if (enemyMonsters.Count == 0) return false;

            foreach (var em in enemyMonsters)
            {
                int attr = (int)em.Attribute;
                if (Bot.Banished.Any(c => c != null && c.IsFacedown() && IsTrirealmMonster(c) && ((int)c.Attribute & attr) > 0))
                {
                    return true;
                }
            }
            return false;
        }

        private bool OnDarknessQuickDestroy()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            int facedownCount = Bot.Banished.Count(c => c != null && c.IsFacedown()) + Enemy.Banished.Count(c => c != null && c.IsFacedown());
            if (facedownCount < 20) return false;

            // Phase safeguard: Never activate in Draw or Standby Phase unless chained to an opponent effect!
            if (Duel.Phase == DuelPhase.Draw || Duel.Phase == DuelPhase.Standby)
            {
                if (Duel.LastChainPlayer != 1) return false;
            }

            // Target verification safeguard (Rule 15):
            // Check if enemy Dragon monsters are protected by Azure-Eyes (40908371)
            bool enemyDragonsProtected = Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(40908371));

            // Check if enemy Spells are protected by Dark Magician the Dragon Knight (41721210)
            bool enemySpellsProtected = Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(41721210));

            // Check if Dragon Knight itself is protected by Eternal Soul (48680970)
            bool enemyHasEternalSoul = Enemy.GetSpells().Any(s => s != null && s.IsFaceup() && s.IsCode(48680970));

            bool hasValidEnemyMonster = Enemy.GetMonsters().Any(m => m != null && !m.IsShouldNotBeTarget() &&
                (!enemyDragonsProtected || !m.HasRace(CardRace.Dragon)) &&
                (!enemyHasEternalSoul || !m.IsCode(41721210)));

            bool hasValidEnemySpell = Enemy.GetSpells().Any(s => s != null && !s.IsShouldNotBeTarget() &&
                (!enemySpellsProtected || s.IsCode(48680970))); // Eternal Soul can always be popped to wipe enemy board!

            if (!hasValidEnemyMonster && !hasValidEnemySpell) return false;

            // Priority 1: Always pop Eternal Soul if faceup on enemy field!
            if (enemyHasEternalSoul) return true;

            // On Opponent Turn: Be smart so we don't get baited by tribute-escapers (e.g. Dragon Spirit of White) or normal spells!
            if (Duel.Player != 0)
            {
                // Check if opponent has an active boss or removal threat on field
                bool enemyHasActiveThreat = Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && !m.IsDisabled() && (
                    m.Id == 38517737 // Blue-Eyes Alternative White Dragon (pop immediately before it pops us!)
                    || m.Id == 1561110 // ABC-Dragon Buster
                    || m.Id == 63767246 // Hope Harbinger
                    || m.Id == 59822133 // Blue-Eyes Spirit Dragon
                    || m.Id == 41721210 // Dark Magician the Dragon Knight
                    || m.Id == 1508649 // Altergeist Hexstia
                    || m.Id == 89538537 // Altergeist Silquitous
                    || (m.Attack >= 2500 && m.Id != 45467446) // Threatening boss (not Dragon Spirit of White escape artist)
                ));

                bool enemyHasKeySpell = Enemy.GetSpells().Any(s => s != null && s.IsFaceup() && (
                    s.Id == 48680970 // Eternal Soul
                    || s.Id == 66399653 // Union Hangar
                    || s.Id == 47222536 // Dark Magical Circle
                    || s.Id == 68462976 // Secret Village
                ));

                if (enemyHasActiveThreat || enemyHasKeySpell)
                {
                    return true;
                }

                // If in Battle Phase, pop their strongest attacker!
                if (Duel.Phase == DuelPhase.BattleStart && hasValidEnemyMonster)
                {
                    return true;
                }

                // Otherwise, hold the Quick Pop until they summon a real threat!
                return false;
            }

            // On Bot's Turn:
            // Pop before attacking in Main Phase 1, or after battle in Main Phase 2
            if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Main2)
            {
                return true;
            }

            return false;
        }

        private bool OnTrirealmJudgment()
        {
            if (Card.Location != CardLocation.SpellZone) return false;

            // Damage Calc trigger: Make opponent ATK 0 when 0 cards in deck
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Damage)
            {
                return Bot.Deck.Count == 0;
            }

            // Summon Intercept trigger: Opponent summoned a monster
            if (Duel.LastSummonPlayer == 1 && Duel.LastSummonedCards != null && Duel.LastSummonedCards.Count > 0)
            {
                var summonedCard = Duel.LastSummonedCards.LastOrDefault();
                if (summonedCard != null)
                {
                    int enemyAttr = (int)summonedCard.Attribute;
                    return Bot.Banished.Any(c => c != null && c.IsFacedown() && IsTrirealmMonster(c) && ((int)c.Attribute & enemyAttr) > 0);
                }
            }

            return false;
        }

        private bool OnNarakaOpponentSummon()
        {
            // 1. In Hand: Special Summon when opponent Special Summons!
            if (Card.Location == CardLocation.Hand)
            {
                Logger.DebugWriteLine("[Naraka] Opponent Special Summoned! Special Summoning Naraka from Hand.");
                return true;
            }

            // 2. In MonsterZone: Quick Summon Lv 5+ Trirealm boss during opponent's Main Phase!
            if (Card.Location == CardLocation.MonsterZone && Duel.Player != 0)
            {
                return Bot.Banished.Any(c => c != null && c.IsFacedown() && (
                    c.IsCode(CardId.TrirealmRiftDarkness)
                    || c.IsCode(CardId.BurialSummitTrirealmRiftHelheim)
                    || c.IsCode(CardId.MadTempestTrirealmRiftPloutonion)
                    || c.IsCode(CardId.SkyThunderTrirealmRiftYomi)
                ));
            }
            return false;
        }

        private bool OnGizmekOrochiQuick()
        {
            // 1. In Hand or Graveyard: Special Summon by banishing 8 cards from top of Deck!
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave)
            {
                if (Bot.Deck.Count < 8 || _gizmekSummonedThisTurn) return false;

                // On our turn: summon in MP1, Battle, or MP2 to provide Level 8 body, beatstick, and Gren Maju fuel!
                if (Duel.Player == 0)
                {
                    _gizmekSummonedThisTurn = true;
                    Logger.DebugWriteLine("[GizmekOrochi] Special Summoning Orochi from Hand/Grave! +3200 ATK to Gren Maju.");
                    return true;
                }
                // On opponent's turn: summon in Battle Step or End Phase as defense wall
                if (Duel.Player == 1 && (Duel.Phase == DuelPhase.BattleStep || Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.End))
                {
                    _gizmekSummonedThisTurn = true;
                    Logger.DebugWriteLine("[GizmekOrochi] Quick Summoning Orochi on opponent turn!");
                    return true;
                }
            }

            // 2. On Field: Pop 1 opponent monster by banishing 3 Extra Deck cards!
            if (Card.Location == CardLocation.MonsterZone)
            {
                return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget() && (c.Attack >= 2000 || CardIntelligence.IsFloodgate(c.Id) || CardIntelligence.IsKnownNegator(c.Id)));
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        // BOARD BREAKERS, POTS & KAIJU
        // ═══════════════════════════════════════════════════════════════

        private bool OnPotOfExtravagance()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (Duel.Phase != DuelPhase.Main1) return false;
            if (Bot.ExtraDeck.Count < 6) return false;

            // Extravagance MUST activate at the start of Main Phase 1 before other actions!
            // Banishes 6 cards face-down from Extra Deck (+2400 ATK fuel) and draws 2 cards!
            return true;
        }

        private bool OnPotOfProsperity()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (Bot.ExtraDeck.Count < 3) return false;

            // Excavates up to 6 cards to search Gren Maju, Darkness, or blowouts!
            // Banishes 6 cards from Extra Deck face-down (+2400 ATK fuel for Gren Maju)
            return true;
        }

        private bool OnHarpieFeatherDuster()
        {
            // Blow away opponent backrows (destroys Eternal Soul, Traps, Floodgates)
            return Duel.Phase == DuelPhase.Main1 && Enemy.GetSpellCount() > 0;
        }

        private bool OnLightningStorm()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (Duel.Phase != DuelPhase.Main1) return false;
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup()) || Bot.GetSpells().Any(c => c != null && c.IsFaceup())) return false;

            // Option 1: Destroy all opponent Spells/Traps (if enemy has backrow)
            if (Enemy.GetSpellCount() >= 2 || (Enemy.GetSpellCount() >= 1 && Enemy.GetMonsterCount() <= 1))
            {
                AI.SelectOption(1);
                return true;
            }

            // Option 0: Destroy all opponent Attack Position monsters
            if (Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsAttack()))
            {
                AI.SelectOption(0);
                return true;
            }

            if (Enemy.GetSpellCount() > 0)
            {
                AI.SelectOption(1);
                return true;
            }

            return false;
        }

        private bool OnDarkRulerNoMore()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // Shuts down all face-up opponent monsters with NO monster response allowed!
            bool canActivate = Duel.Phase == DuelPhase.Main1 && Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && !m.IsDisabled() && (m.Attack >= 1500 || m.HasType(CardType.Effect) || m.IsExtraCard() || CardIntelligence.IsKnownNegator(m.Id) || CardIntelligence.IsFloodgate(m.Id)));
            if (canActivate)
            {
                _drnmUsedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool OnRaigeki()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // Destroy all monsters opponent controls -> direct path for lethal Gren Maju
            return Duel.Phase == DuelPhase.Main1 && Enemy.GetMonsterCount() > 0;
        }

        private bool OnGamecielKaijuSummon()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (Duel.Turn == 1 || Duel.Phase != DuelPhase.Main1) return false;

            bool enemyAlreadyHasKaiju = Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && (m.HasSetcode(0xd3) || m.Id == CardId.GamecielKaiju || m.Id == 28674152 || m.Id == 63941210 || m.Id == 29726552));
            if (enemyAlreadyHasKaiju)
            {
                // Only summon 2nd Kaiju to OUR field if we can Rank 8 overlay immediately or push for lethal!
                bool canRank8 = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Level == 8);
                bool haveLethal = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.GrenMajuDaEiza) && m.Attack >= 4000);
                return canRank8 || haveLethal;
            }

            bool enemyHasEternalSoul = Enemy.GetSpells().Any(s => s != null && s.IsFaceup() && s.IsCode(48680970));

            // PRIORITY 1: Known Negators & Dangerous Bosses (MUST TRIBUTE!)
            var dangerousEnemy = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && !m.IsDisabled() && (
                m.Id == 21887175 // Mekk-Knight Crusadia Avramax (Untargetable / Battle Boost - MUST BE #1!)
                || (m.Id == 41721210 && enemyHasEternalSoul) // Dark Magician the Dragon Knight (breaks Eternal Soul immunity lock!)
                || m.Id == 40908371 // Azure-Eyes Silver Dragon (Protects all dragons from targeting & destruction!)
                || m.Id == 1561110  // ABC-Dragon Buster (Quick Banish)
                || m.Id == 63767246 // Hope Harbinger (Spell Negator)
                || m.Id == 10443957 // Cyber Dragon Infinity (Omninegator)
                || m.Id == 4280258  // Apollousa, Bow of the Goddess (Monster Negate)
                || m.Id == 1508649  // Altergeist Hexstia (Spell/Trap Negator)
                || m.Id == 89538537 // Altergeist Silquitous (Bounce)
                || m.Id == 50954680 // Crystal Wing Synchro Dragon
                || m.Id == 41721210 // Dark Magician the Dragon Knight
                || m.Id == 59822133 // Blue-Eyes Spirit Dragon
                || CardIntelligence.IsKnownNegator(m.Id)
                || CardIntelligence.IsFloodgate(m.Id)
            ));

            // PRIORITY 2: High ATK monsters (>= 2200 ATK, but NOT Dark Magician if Eternal Soul is active)
            if (dangerousEnemy == null)
            {
                dangerousEnemy = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup() && (
                    m.Attack >= 2200 && (!m.IsCode(46986414) || !enemyHasEternalSoul)
                )).OrderByDescending(m => m.Attack).FirstOrDefault();
            }

            // PRIORITY 3: Extra Deck monsters with ATK >= 2000
            if (dangerousEnemy == null)
            {
                dangerousEnemy = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.IsExtraCard() && m.Attack >= 2000);
            }

            // PRIORITY 4: Only tribute weaker monsters if we have lethal Gren Maju ready to attack
            if (dangerousEnemy == null)
            {
                bool haveLethalMaju = (Bot.HasInHand(CardId.GrenMajuDaEiza) || Bot.GetMonsters().Any(m => m.IsCode(CardId.GrenMajuDaEiza))) && Bot.Banished.Count >= 10;
                if (haveLethalMaju)
                {
                    dangerousEnemy = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup()).OrderByDescending(m => m.Attack).FirstOrDefault();
                }
            }

            if (dangerousEnemy == null) return false;

            // If enemy is immune to destruction (e.g. Dragon Knight + Eternal Soul, Avramax, Azure-Eyes), Raigeki won't kill it!
            bool enemyImmuneToBoardWipe = enemyHasEternalSoul || dangerousEnemy.Id == 21887175 || dangerousEnemy.Id == 40908371 || dangerousEnemy.Id == 41721210;

            // If enemy can escape or tag out (e.g. ABC-Dragon Buster tag out, Dragon Spirit of White tribute escape), Kaiju tributes first!
            bool enemyCanEscapeOrDisrupt = dangerousEnemy.Id == 1561110 || dangerousEnemy.Id == 45467446 || dangerousEnemy.Id == 89538537;

            // If we have Raigeki / Lightning Storm in hand, let them wipe first UNLESS enemy negates Spells, is immune to destruction, or can escape/tag out
            bool enemyNegatesSpells = Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && !m.IsDisabled() && (
                m.Id == 63767246 || m.Id == 10443957 || m.Id == 1508649 || CardIntelligence.IsKnownNegator(m.Id)
            ));

            if (!enemyImmuneToBoardWipe && !enemyNegatesSpells && !enemyCanEscapeOrDisrupt && (Bot.HasInHand(CardId.Raigeki) || (Bot.HasInHand(CardId.LightningStorm) && Bot.GetFieldCount() == 0)) && Duel.Phase == DuelPhase.Main1)
            {
                return false;
            }

            AI.SelectCard(dangerousEnemy);
            return true;
        }

        private bool OnSmallWorld()
        {
            if (Card.Location != CardLocation.Hand) return false;

            // 1. If Gren Maju is not in hand, Small World guarantees Gren Maju via Naraka bridge!
            if (!Bot.HasInHand(CardId.GrenMajuDaEiza))
            {
                return Bot.Hand.Any(c => c != null && c.IsMonster() && c != Card);
            }

            // 2. If opponent has a high-threat boss that we can Kaiju, search Gameciel!
            bool enemyHasBoss = Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && (m.Attack >= 2500 || CardIntelligence.IsKnownNegator(m.Id) || CardIntelligence.IsFloodgate(m.Id)));
            if (enemyHasBoss && !Bot.HasInHand(CardId.GamecielKaiju))
            {
                return Bot.Hand.Count(c => c != null && c.IsMonster() && c != Card) >= 2;
            }

            // 3. Search Helheim (hand boss that summons Darkness directly from banished!)
            if (!Bot.HasInHand(CardId.BurialSummitTrirealmRiftHelheim) && !Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.BurialSummitTrirealmRiftHelheim)))
            {
                return Bot.Hand.Count(c => c != null && c.IsMonster() && c != Card) >= 2;
            }

            // 4. Check if we need Gizmek Orochi (instant 8 banish fuel + Rank 8 material)
            if (!Bot.HasInHand(CardId.GizmekOrochi) && !Bot.HasInGraveyard(CardId.GizmekOrochi) && Bot.Deck.Count >= 12)
            {
                return Bot.Hand.Count(c => c != null && c.IsMonster() && c != Card) >= 2;
            }

            return false;
        }

        private bool OnPotOfDesires()
        {
            if (Bot.Deck.Count < 20) return false;

            // 1. If Gren Maju or Darkness is already secured in hand/field, Desires gives fuel & draws 2!
            bool hasBossSecured = Bot.HasInHand(CardId.GrenMajuDaEiza) || Bot.HasInHand(CardId.TrirealmRiftDarkness)
                || Bot.GetMonsters().Any(m => m != null && (m.IsCode(CardId.GrenMajuDaEiza) || m.IsCode(CardId.TrirealmRiftDarkness)));

            if (hasBossSecured)
            {
                return true;
            }

            // 2. If Small World or Valvols or Terraforming or Gehenna in hand, search boss first before blind 10-banish!
            if (Bot.HasInHand(CardId.SmallWorld) || Bot.HasInHand(CardId.TrirealmRiftTerritoryValvols)
                || Bot.HasInHand(CardId.Terraforming) || Bot.HasInHand(CardId.TrirealmRiftOfEmptinessGehenna))
            {
                return false;
            }

            return true;
        }

        private bool OnSummonBanishTrigger()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        // SEARCH ENGINES & EXTENDERS
        // ═══════════════════════════════════════════════════════════════

        private bool OnTerraforming()
        {
            return !Bot.Hand.Any(c => c != null && c.IsCode(CardId.TrirealmRiftTerritoryValvols))
                && !Bot.SpellZone.Any(c => c != null && c.IsCode(CardId.TrirealmRiftTerritoryValvols));
        }

        private bool OnValvolsHandActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.IsCode(CardId.TrirealmRiftTerritoryValvols))) return false;
                // Deck out safeguard: Do not activate if it would leave deck with < 2 cards unless GY covers the cost
                if (Bot.Deck.Count <= 6 && Bot.Graveyard.Count < 5) return false;
                return Bot.Deck.Count + Bot.Graveyard.Count >= 5;
            }
            return false;
        }

        private bool OnGospelHandActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.IsCode(CardId.TrirealmRiftGospel))) return false;
                // Deck out safeguard: Do not activate if it would leave deck with < 2 cards unless GY covers the cost
                if (Bot.Deck.Count <= 6 && Bot.Graveyard.Count < 5) return false;
                return Bot.Deck.Count + Bot.Graveyard.Count >= 5;
            }
            return false;
        }

        private bool OnPloutonionHandDraw()
        {
            if (Card.Location == CardLocation.Hand && !_ploutonionDrawnThisTurn)
            {
                var other = Bot.Hand.FirstOrDefault(c => c != null && c != Card && IsTrirealm(c));
                if (other != null && Bot.Deck.Count >= 2)
                {
                    _ploutonionDrawnThisTurn = true;
                    return true;
                }
            }
            return false;
        }

        private bool OnYomiHandSearch()
        {
            if (Card.Location == CardLocation.Hand && !_yomiSearchedThisTurn)
            {
                bool hasTarget = Bot.Banished.Any(c => c != null && c.IsFacedown() && IsTrirealm(c) && c.Level != 5);
                if (hasTarget)
                {
                    _yomiSearchedThisTurn = true;
                    return true;
                }
            }
            return false;
        }

        private bool OnHelheimQuickSummon()
        {
            if (Card.Location == CardLocation.Hand && !_helheimSummonedThisTurn)
            {
                if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
                // Helheim reveals itself, banishes itself face-down, and Special Summons 1 face-down banished Trirealm monster (except Lv 6)!
                bool hasBanishedTarget = Bot.Banished.Any(c => c != null && c.IsFacedown() && IsTrirealmMonster(c) && c.Level != 6);
                if (hasBanishedTarget && Bot.GetMonsterCount() < 5)
                {
                    _helheimSummonedThisTurn = true;
                    return true;
                }
            }
            return false;
        }

        private bool OnGizmekOrochiSpSummon()
        {
            if ((Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave) && !_gizmekSummonedThisTurn)
            {
                if (Duel.Turn == 1 && Duel.Player != 0) return false;
                if (Bot.Deck.Count < 10 || Bot.GetMonsterCount() >= 5) return false;

                // Gizmek Orochi banishes 8 cards from top of deck!
                // Only summon if:
                // 1) Gren Maju or Darkness is secured in hand/field
                // 2) OR we are in Battle Phase / need a 2450 body to survive
                bool hasBossSecured = Bot.HasInHand(CardId.GrenMajuDaEiza) || Bot.HasInHand(CardId.TrirealmRiftDarkness)
                    || Bot.GetMonsters().Any(m => m != null && (m.IsCode(CardId.GrenMajuDaEiza) || m.IsCode(CardId.TrirealmRiftDarkness)));

                if (hasBossSecured)
                {
                    _gizmekSummonedThisTurn = true;
                    return true;
                }

                if (Bot.HasInHand(CardId.SmallWorld) || Bot.HasInHand(CardId.TrirealmRiftTerritoryValvols)
                    || Bot.HasInHand(CardId.Terraforming) || Bot.HasInHand(CardId.TrirealmRiftOfEmptinessGehenna)
                    || Bot.HasInHand(CardId.PotOfProsperity))
                {
                    return false;
                }

                if (Enemy.GetMonsterCount() > 0 && Bot.GetMonsterCount() == 0 && Bot.Hand.Count <= 2)
                {
                    _gizmekSummonedThisTurn = true;
                    return true;
                }

                return false;
            }
            return false;
        }

        private bool ShouldDelayXenoLock()
        {
            // 1. Pot of Desires: Activate early to draw cards and fuel banished zone before lock
            if (Bot.HasInHand(CardId.PotOfDesires) && Bot.Deck.Count >= 12)
                return true;

            // 2. Raigeki: Only delay if enemy has monsters
            if (Bot.HasInHand(CardId.Raigeki) && Enemy.GetMonsterCount() > 0)
                return true;

            // 3. Harpie's Feather Duster: Only delay if enemy has Spells/Traps
            if (Bot.HasInHand(CardId.HarpieFeatherDuster) && Enemy.GetSpellCount() > 0)
                return true;

            // 4. Lightning Storm: Only delay if we control NO face-up cards and enemy has targets
            if (Bot.HasInHand(CardId.LightningStorm) && !Bot.GetMonsters().Any(c => c != null && c.IsFaceup()) && !Bot.GetSpells().Any(c => c != null && c.IsFaceup()) && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0))
                return true;

            // 5. Pot of Prosperity: Only delay if Extra Deck >= 3
            if (Bot.HasInHand(CardId.PotOfProsperity) && Bot.ExtraDeck.Count >= 3)
                return true;

            return false;
        }

        private bool OnGehennaHandSummon()
        {
            return Card.Location == CardLocation.Hand && Bot.GetMonsterCount() == 0;
        }

        private bool OnGehennaFieldSearch()
        {
            if (Card.Location == CardLocation.MonsterZone && !_gehennaSearchedThisTurn && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                if (ShouldDelayXenoLock()) return false;

                bool hasTargets = Bot.Banished.Any(c => c != null && c.IsFacedown() && IsTrirealmMonster(c) && !c.IsCode(CardId.TrirealmRiftOfEmptinessGehenna));
                if (hasTargets)
                {
                    _gehennaSearchedThisTurn = true;
                    return true;
                }
            }
            return false;
        }

        private bool OnSheolHandSummon()
        {
            return Card.Location == CardLocation.Hand
                && Bot.GetMonsters().Any(c => c != null && IsTrirealmMonster(c))
                && Bot.GetMonsterCount() < 5;
        }

        private bool OnSheolFieldSearch()
        {
            if (Card.Location == CardLocation.MonsterZone && !_sheolSearchedThisTurn && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                if (ShouldDelayXenoLock()) return false;

                bool hasTargets = Bot.Banished.Any(c => c != null && c.IsFacedown() && (c.IsCode(CardId.TrirealmRiftTerritoryValvols) || c.IsCode(CardId.TrirealmRiftGospel) || c.IsCode(CardId.TrirealmRiftJudgment)));
                if (hasTargets)
                {
                    _sheolSearchedThisTurn = true;
                    return true;
                }
            }
            return false;
        }

        private bool OnTuonelaHandSummon()
        {
            return Card.Location == CardLocation.Hand
                && Enemy.GetMonsterCount() > Bot.GetMonsterCount()
                && Bot.GetMonsterCount() < 5;
        }

        private bool OnTuonelaFieldSpSummon()
        {
            if (Card.Location == CardLocation.MonsterZone && !_tuonelaSpSummonUsedThisTurn && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                if (ShouldDelayXenoLock()) return false;

                bool hasTarget = Bot.Banished.Any(c => c != null && c.IsFacedown() && IsTrirealmMonster(c) && c.Level >= 5);
                if (hasTarget && Bot.GetMonsterCount() < 5)
                {
                    _tuonelaSpSummonUsedThisTurn = true;
                    return true;
                }
            }
            return false;
        }

        private bool OnValvolsFieldSearch()
        {
            if (Card.Location == CardLocation.SpellZone && !_valvolsSearchedThisTurn && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                bool hasTarget = Bot.Banished.Any(c => c != null && c.IsFacedown() && IsTrirealm(c));
                if (hasTarget)
                {
                    _valvolsSearchedThisTurn = true;
                    return true;
                }
            }
            return false;
        }

        private bool OnGospelFieldMorph()
        {
            if (Card.Location == CardLocation.SpellZone && !_gospelMorphedThisTurn && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                var lv4Fodder = Bot.GetMonsters().FirstOrDefault(c => c != null && IsTrirealmMonster(c) && c.Level <= 4);
                bool hasBanishedBoss = Bot.Banished.Any(c => c != null && c.IsFacedown() && (
                    c.IsCode(CardId.TrirealmRiftDarkness)
                    || c.IsCode(CardId.BurialSummitTrirealmRiftHelheim)
                    || c.IsCode(CardId.MadTempestTrirealmRiftPloutonion)
                    || c.IsCode(CardId.SkyThunderTrirealmRiftYomi)
                ));
                if (lv4Fodder != null && hasBanishedBoss)
                {
                    _gospelMorphedThisTurn = true;
                    return true;
                }
            }
            return false;
        }

        private bool OnNarakaHandSummon()
        {
            return Card.Location == CardLocation.Hand && Bot.GetMonsterCount() < 5;
        }

        // ═══════════════════════════════════════════════════════════════
        // RANK 8 XYZ TOOLBOX & OTK BACKUP (Dingirsu, Draglubion, Numeron)
        // ═══════════════════════════════════════════════════════════════

        private bool OnDingirsuSummon()
        {
            // Dingirsu requires 2 Level 8s (Gizmek Orochi, Trirealm Darkness)
            // NEVER use Gren Maju as material!
            int lv8Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 8 && !c.IsCode(CardId.GrenMajuDaEiza));
            if (lv8Count < 2) return false;

            // In Turn 2+: Summon Dingirsu to send 1 enemy card to GY (non-targeting) AND protect Gren Maju from any destruction!
            // In Turn 1: Summon Dingirsu as destruction shield
            return Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0 || Duel.Turn == 1;
        }

        private bool OnDraglubionSummon()
        {
            // Draglubion requires 2 Level 8s (Gizmek Orochi, Trirealm Darkness)
            int lv8Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 8 && !c.IsCode(CardId.GrenMajuDaEiza));
            if (lv8Count < 2) return false;

            // BACKUP PLAN: If Gren Maju is NOT on field / not in hand, Draglubion -> Numeron Dragon delivers 9,000-17,000 ATK OTK!
            bool hasGrenMajuOnField = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.GrenMajuDaEiza));
            if (!hasGrenMajuOnField && Duel.Turn > 1 && Duel.Phase == DuelPhase.Main1)
            {
                return true;
            }

            return false;
        }

        private bool OnDraglubionActivate()
        {
            return Card.Location == CardLocation.MonsterZone;
        }

        private bool OnNumeronDragonActivate()
        {
            return Card.Location == CardLocation.MonsterZone;
        }

        private bool OnHopeHarbingerSummon()
        {
            int lv8Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 8 && !c.IsCode(CardId.GrenMajuDaEiza));
            if (lv8Count < 2) return false;
            return !Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.HopeHarbinger));
        }

        private bool OnPhotonLordSummon()
        {
            int lv8Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 8 && !c.IsCode(CardId.GrenMajuDaEiza));
            if (lv8Count < 2) return false;
            return !Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.PhotonLord));
        }

        private bool OnSanaphondSummon()
        {
            int lv8Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 8 && !c.IsCode(CardId.GrenMajuDaEiza));
            if (lv8Count < 2) return false;
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.Sanaphond))) return false;
            // Summon Sanaphond when opponent has monsters in GY to shut down GY revives, or on Turn 1!
            return Enemy.Graveyard.Count(c => c != null && c.IsMonster()) >= 1 || Duel.Turn == 1;
        }

        private bool OnSanaphondActivate()
        {
            return Card.Location == CardLocation.MonsterZone;
        }

        private bool OnUnderworldGoddessSummon()
        {
            if (Duel.Turn == 1 || Util.IsTurn1OrMain2()) return false;
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.GrenMajuDaEiza))) return false;
            int botMonsters = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect) && !c.IsCode(CardId.GrenMajuDaEiza));
            int enemyMonsters = Enemy.GetMonsters().Count(c => c != null && c.IsFaceup() && !c.HasType(CardType.Token));
            return botMonsters >= 3 && enemyMonsters >= 1;
        }

        private bool OnZeroborosSummon()
        {
            if (Duel.Turn == 1 || Util.IsTurn1OrMain2()) return false;
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.GrenMajuDaEiza))) return false;
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !c.IsCode(CardId.GrenMajuDaEiza));
            return mats >= 3 && (Enemy.GetMonsterCount() >= 2 || Enemy.GetSpellCount() >= 2);
        }

        private bool OnAccesscodeTalkerSummon()
        {
            if (Util.IsTurn1OrMain2()) return false;
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.IsCode(CardId.GrenMajuDaEiza) || c.IsCode(CardId.TrirealmRiftDarkness)))) return false;
            var mats = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && !m.IsCode(CardId.GrenMajuDaEiza) && !m.IsCode(CardId.TrirealmRiftDarkness) && !m.IsCode(CardId.GamecielKaiju)).ToList();
            int linkRating = mats.Sum(m => m.HasType(CardType.Link) ? m.LinkMarker : 1);
            return mats.Count >= 2 && linkRating >= 4;
        }

        private bool OnKnightmareUnicornSummon()
        {
            if (Duel.Turn == 1 || Util.IsTurn1OrMain2()) return false;
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.GrenMajuDaEiza))) return false;
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !c.IsCode(CardId.GrenMajuDaEiza) && !c.IsCode(CardId.GizmekOrochi) && !c.IsCode(CardId.TrirealmRiftDarkness) && !c.IsCode(CardId.GamecielKaiju));
            return mats >= 3 && Enemy.GetMonsterCount() > 0;
        }

        private bool OnKnightmareUnicornActivate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            var target = Plugin.ThreatImpl.PickBestRemovalTarget(Enemy.GetMonsters().Concat(Enemy.GetSpells()).ToList());
            if (target == null) return false;

            var discard = Bot.Hand.FirstOrDefault(c => c != null && !c.IsCode(CardId.GrenMajuDaEiza));
            if (discard == null) return false;

            AI.SelectCard(discard);
            var eternalSoul = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsCode(48680970));
            AI.SelectNextCard(eternalSoul ?? target);
            return true;
        }

        private bool OnKnightmarePhoenixSummon()
        {
            if (Util.IsTurn1OrMain2()) return false;
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.IsCode(CardId.GrenMajuDaEiza) || c.IsCode(CardId.TrirealmRiftDarkness)))) return false;
            // Never sacrifice Gameciel
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.GamecielKaiju))) return false;
            // Never summon Phoenix if we already have Cosmic Cyclone, Duster, or Lightning Storm
            if (Bot.HasInHand(CardId.CosmicCyclone) || Bot.HasInHand(CardId.HarpieFeatherDuster) || Bot.HasInHand(CardId.LightningStorm)) return false;
            // Never summon Phoenix if Darkness is in hand or can be summoned from banished
            if (Bot.HasInHand(CardId.TrirealmRiftDarkness) && Bot.GetMonsterCount() >= 2) return false;
            if (Bot.Banished.Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.TrirealmRiftDarkness)) && (Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.IsCode(CardId.TrirealmRiftGospel)) || Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.TrirealmRiftOfBlueTuonela)))) return false;

            // Phoenix only has 1900 ATK. Don't summon into an enemy board that will destroy Phoenix next turn unless we pop Eternal Soul (which board-wipes them)
            bool enemyHasEternalSoul = Enemy.GetSpells().Any(s => s != null && s.IsFaceup() && s.IsCode(48680970));
            if (!enemyHasEternalSoul && Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Attack > 1900)) return false;

            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !c.IsCode(CardId.GrenMajuDaEiza) && !c.IsCode(CardId.TrirealmRiftDarkness) && !c.IsCode(CardId.GamecielKaiju));
            // Only summon Phoenix to destroy a critical floodgate or continuous spell (e.g. Eternal Soul, Imperial Order, Secret Village, Union Hangar)
            bool hasCriticalSpell = Enemy.GetSpells().Any(s => s != null && s.IsFaceup() && (s.IsCode(48680970) || s.IsCode(66399653) || s.IsCode(68462976) || s.IsCode(61740673) || s.IsCode(47222536)));
            return mats >= 2 && hasCriticalSpell && Bot.Hand.Count > 0;
        }

        private bool OnKnightmarePhoenixActivate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            var target = Util.GetBestEnemySpell();
            if (target == null) return false;

            var discard = Bot.Hand.FirstOrDefault(c => c != null && !c.IsCode(CardId.GrenMajuDaEiza));
            if (discard == null) return false;

            AI.SelectCard(discard);
            var eternalSoul = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsCode(48680970));
            AI.SelectNextCard(eternalSoul ?? target);
            return true;
        }

        private bool OnTyphonSummon()
        {
            if (Duel.Phase != DuelPhase.Main2) return false;

            bool enemyHasThreat = Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && (m.Attack >= 2500 || m.IsExtraCard() || CardIntelligence.IsKnownNegator(m.Id)));
            if (!enemyHasThreat) return false;

            var nonAceMat = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsCode(CardId.GrenMajuDaEiza) && !c.IsCode(CardId.TrirealmRiftDarkness)).OrderByDescending(c => c.Attack).FirstOrDefault();
            if (nonAceMat != null) return true;

            // If only Gren Maju or Darkness is on field, but enemy has 2+ threatening monsters that will kill us next turn, overlay into TY-PHON to survive!
            int enemyThreatCount = Enemy.GetMonsters().Count(m => m != null && m.IsFaceup() && (m.Attack >= 2500 || m.IsExtraCard() || CardIntelligence.IsKnownNegator(m.Id)));
            if (enemyThreatCount >= 2 && Bot.LifePoints <= 4000)
            {
                Logger.DebugWriteLine("[TY-PHON] Emergency overlay in MP2 to disable enemy 3000+ ATK bosses and survive!");
                return true;
            }

            return false;
        }

        private bool OnTyphonActivate()
        {
            var target = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup()).OrderByDescending(m => m.Attack).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool OnZeusSummon()
        {
            if (Duel.Phase != DuelPhase.Main2) return false;
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Xyz) && !c.IsCode(CardId.DivineArsenalAAZEUS));
        }

        private bool OnZeusActivate()
        {
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.GrenMajuDaEiza))) return false;
            return Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 2;
        }

        private bool OnSPLittleKnightSummon()
        {
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.IsCode(CardId.GrenMajuDaEiza) || c.IsCode(CardId.TrirealmRiftDarkness)))) return false;
            // If we have Darkness in hand and can summon it, do not waste monsters on S:P!
            if (Bot.HasInHand(CardId.TrirealmRiftDarkness) && Bot.GetMonsters().Count(c => c.Level <= 4 && !c.IsCode(CardId.GrenMajuDaEiza)) >= 2) return false;
            // Never summon S:P if Gospel can morph into Darkness or Tuonela can summon Darkness
            bool hasBanishedDarkness = Bot.Banished.Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.TrirealmRiftDarkness));
            if (hasBanishedDarkness && Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.IsCode(CardId.TrirealmRiftGospel)) && !_gospelMorphedThisTurn) return false;
            if (hasBanishedDarkness && Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.TrirealmRiftOfBlueTuonela)) && !_tuonelaSpSummonUsedThisTurn) return false;

            var mats = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsCode(CardId.GrenMajuDaEiza) && !c.IsCode(CardId.TrirealmRiftDarkness)).ToList();
            if (mats.Count < 2) return false;

            // Only summon S:P if at least 1 material is an Extra Deck monster (so on-summon banish triggers!)
            // OR if opponent controls a monster that cannot be destroyed by battle/effects and we need banish
            bool usesExtraMat = mats.Any(c => c.IsExtraCard());
            bool needsNonDestructionBanish = Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && (m.Id == 21887175 || m.Id == 40908371 || m.Id == 41721210 || CardIntelligence.IsKnownNegator(m.Id)));
            return usesExtraMat || needsNonDestructionBanish;
        }

        private bool OnSPLittleKnightActivate()
        {
            return Card.Location == CardLocation.MonsterZone;
        }

        private bool OnLinkuribohSummon()
        {
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.GrenMajuDaEiza))) return false;
            var gehenna = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.TrirealmRiftOfEmptinessGehenna));
            if (gehenna == null) return false;

            // 1. Do NOT link Gehenna away if we have Sheol in hand (Sheol needs a face-up Trirealm on field to SS!)
            if (Bot.HasInHand(CardId.TrirealmRiftOfSkySheol)) return false;

            // 2. Do NOT link Gehenna away if Gospel is on field or in hand (Gospel needs a Lv 4- Trirealm to morph into Darkness/Helheim!)
            bool hasGospelMorph = (Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.IsCode(CardId.TrirealmRiftGospel)) || Bot.HasInHand(CardId.TrirealmRiftGospel))
                && Bot.Banished.Any(c => c != null && c.IsFacedown() && (
                    c.IsCode(CardId.TrirealmRiftDarkness)
                    || c.IsCode(CardId.BurialSummitTrirealmRiftHelheim)
                    || c.IsCode(CardId.MadTempestTrirealmRiftPloutonion)
                    || c.IsCode(CardId.SkyThunderTrirealmRiftYomi)
                ));
            if (hasGospelMorph) return false;

            // 3. Do NOT link Gehenna if it can still search
            if (!_gehennaSearchedThisTurn && Bot.Banished.Any(c => c != null && c.IsFacedown() && IsTrirealmMonster(c) && !c.IsCode(CardId.TrirealmRiftOfEmptinessGehenna)))
                return false;

            // In Turn 1: If Gehenna has searched, convert into Linkuriboh for 0-ATK attack negate protection!
            if (Duel.Turn == 1 && _gehennaSearchedThisTurn) return true;

            // In Turn 2+, only summon Linkuriboh if Gehenna already searched or in Main 2
            return _gehennaSearchedThisTurn || Duel.Phase == DuelPhase.Main2;
        }

        private bool OnLinkuribohActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                return Duel.Phase == DuelPhase.BattleStep || Duel.Phase == DuelPhase.BattleStart;
            }
            if (Card.Location == CardLocation.Grave && Duel.Player != 0)
            {
                var fodder = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Level == 1 && !c.IsCode(CardId.GrenMajuDaEiza));
                return fodder != null;
            }
            return false;
        }

        private bool OnChaosAngelSummon()
        {
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.GrenMajuDaEiza))) return false;

            // Chaos Angel is Level 10, made using LIGHT and/or DARK monsters without a Tuner!
            // Level 8 (Darkness, Gizmek Orochi, Helheim) + Level 2 (Sheol, Tuonela)
            var faceupMonsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsCode(CardId.GrenMajuDaEiza)).ToList();
            bool hasLv8 = faceupMonsters.Any(c => c.Level == 8);
            bool hasLv2 = faceupMonsters.Any(c => c.Level == 2);

            if (hasLv8 && hasLv2)
            {
                return Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0 || Duel.Turn == 1;
            }

            return false;
        }

        private bool OnChaosAngelActivate()
        {
            return Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
        }

        private bool OnRelinquishedAnimaSummon()
        {
            return false;
        }

        private bool OnRelinquishedAnimaActivate()
        {
            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget());
        }

        private bool OnGoToBattlePhaseForEvenly()
        {
            return Bot.HasInHand(CardId.EvenlyMatched) && Duel.Phase == DuelPhase.Main1 && Duel.Turn >= 2 && Enemy.GetFieldCount() >= 2 && Bot.GetFieldCount() == 0;
        }

        private bool OnEvenlyMatched()
        {
            if (Card.Location != CardLocation.Hand && Card.Location != CardLocation.SpellZone) return false;
            return Enemy.GetFieldCount() > Bot.GetFieldCount();
        }

        private bool OnEvenlyMatchedSet()
        {
            if (Bot.GetFieldCount() == 0) return false;
            return true;
        }

        private bool OnBagooskaSummon()
        {
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle) return false;
            if (Duel.Turn != 1 && Duel.Phase != DuelPhase.Main2 && !_drnmUsedThisTurn) return false;
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.Number41Bagooska))) return false;
            int lv4Mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 4 && !c.IsCode(CardId.GrenMajuDaEiza));
            return lv4Mats >= 2;
        }

        // ═══════════════════════════════════════════════════════════════
        // NORMAL SUMMONS & GREN MAJU OTK
        // ═══════════════════════════════════════════════════════════════

        private bool OnDarknessSummon()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // Darkness banishes 8 cards on summon. Do not deck out unless 40+ face-down cards skip Draw Phase!
            if (Bot.Deck.Count <= 8 && (Bot.Banished.Count + Enemy.Banished.Count) < 35) return false;

            // Turn 2+: If Gren Maju is in hand and we can summon it for massive ATK/OTK, NEVER tribute our monsters for Darkness!
            if (Duel.Turn >= 2 && Bot.HasInHand(CardId.GrenMajuDaEiza) && Bot.Banished.Count >= 6 && !_drnmUsedThisTurn)
            {
                bool hasGospelExtraSummon = Bot.SpellZone.Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.TrirealmRiftGospel));
                var tributes = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsCode(CardId.GrenMajuDaEiza) && !c.IsCode(CardId.TrirealmRiftDarkness)).ToList();
                if (!hasGospelExtraSummon || tributes.Count < 3)
                {
                    return false;
                }
            }

            // Darkness is Level 8 (requires 2 Tributes for Normal Summon)
            // NEVER tribute Gren Maju or another Darkness!
            var safeTributes = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsCode(CardId.GrenMajuDaEiza) && !c.IsCode(CardId.TrirealmRiftDarkness)).ToList();
            if (safeTributes.Count < 2) return false;

            return true;
        }

        private bool OnHelheimSummon()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // STRICT RULE: On Turn 1, do NOT tribute summon (leaves a 2000 ATK attack target)
            if (Duel.Turn == 1) return false;
            if (Duel.Turn >= 2 && Bot.HasInHand(CardId.GrenMajuDaEiza) && Bot.Banished.Count >= 8 && !_drnmUsedThisTurn)
                return false;

            // Helheim is Level 6 (requires 1 tribute)
            var safeTributes = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsCode(CardId.GrenMajuDaEiza) && !c.IsCode(CardId.TrirealmRiftDarkness) && !c.IsCode(CardId.BurialSummitTrirealmRiftHelheim)).ToList();
            if (safeTributes.Count < 1) return false;

            return true;
        }

        private bool OnYomiSummon()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // STRICT RULE: On Turn 1, do NOT tribute summon (leaves a 1000 ATK attack target)
            if (Duel.Turn == 1) return false;
            if (Duel.Turn >= 2 && Bot.HasInHand(CardId.GrenMajuDaEiza) && Bot.Banished.Count >= 8 && !_drnmUsedThisTurn)
                return false;

            // Yomi is Level 5 (requires 1 tribute)
            var safeTributes = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsCode(CardId.GrenMajuDaEiza) && !c.IsCode(CardId.TrirealmRiftDarkness) && !c.IsCode(CardId.BurialSummitTrirealmRiftHelheim) && !c.IsCode(CardId.SkyThunderTrirealmRiftYomi)).ToList();
            if (safeTributes.Count < 1) return false;

            return true;
        }

        private bool OnPloutonionSummon()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // STRICT RULE: On Turn 1, do NOT tribute summon
            if (Duel.Turn == 1) return false;
            if (Duel.Turn >= 2 && Bot.HasInHand(CardId.GrenMajuDaEiza) && Bot.Banished.Count >= 8 && !_drnmUsedThisTurn)
                return false;

            // Ploutonion is Level 7 (requires 2 tributes)
            var safeTributes = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsCode(CardId.GrenMajuDaEiza) && !c.IsCode(CardId.TrirealmRiftDarkness) && !c.IsCode(CardId.BurialSummitTrirealmRiftHelheim)).ToList();
            if (safeTributes.Count < 2) return false;

            return true;
        }

        private bool OnGeneralNormalSummon()
        {
            // 1. If Gospel is active, we have an EXTRA Normal Summon for a Trirealm monster!
            bool hasGospelExtraSummon = Bot.SpellZone.Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.TrirealmRiftGospel));
            if (hasGospelExtraSummon)
            {
                return true;
            }

            // 2. Turn 1: NEVER hold back Normal Summon for Gren Maju! Always summon a starter to set up board / Bagooska!
            if (Duel.Turn == 1)
            {
                return true;
            }

            // 3. If DRNM was activated this turn, allow Gren Maju to summon and destroy negated enemy monsters!
            if (_drnmUsedThisTurn && Bot.HasInHand(CardId.GrenMajuDaEiza) && Bot.Banished.Count >= 6 && Enemy.GetMonsterCount() > 0)
            {
                return false; // Yield Normal Summon to Gren Maju so it can crush enemy monsters!
            }

            // 4. Turn 2+: Only reserve Normal Summon for Gren Maju if we have enough banished fuel (8+ cards = 3200+ ATK) to deal massive damage or OTK!
            if (Bot.HasInHand(CardId.GrenMajuDaEiza) && Bot.Banished.Count >= 8)
            {
                return false;
            }

            // 5. If Small World is in hand and we have 8+ banished cards:
            if (Bot.HasInHand(CardId.SmallWorld) && Bot.Banished.Count >= 8 && Bot.Hand.Count(c => c != null && c.IsMonster() && c != Card) >= 1)
            {
                return false;
            }

            return true;
        }

        private bool OnGrenMajuSummon()
        {
            if (Card.Location != CardLocation.Hand) return false;

            // STRICT RULE: On Turn 1, NEVER summon Gren Maju (keep safe in hand for Turn 2+ OTK!)
            if (Duel.Turn == 1) return false;

            // In Main Phase 2, avoid summoning Gren Maju unless we have no monsters on board
            if (Duel.Phase == DuelPhase.Main2 && Bot.GetMonsterCount() > 0) return false;

            // If DRNM was activated this turn:
            // Gren Maju can STILL battle-destroy negated enemy monsters!
            // Only hold Gren Maju if enemy has NO monsters to destroy AND we have another monster to summon.
            if (_drnmUsedThisTurn && Enemy.GetMonsterCount() == 0)
            {
                bool hasOtherSummon = Bot.Hand.Any(c => c != null && c.IsMonster() && !c.IsCode(CardId.GrenMajuDaEiza) && (c.Level <= 4 || (c.IsCode(CardId.TrirealmRiftDarkness) && Bot.GetMonsters().Count(m => !m.IsCode(CardId.GrenMajuDaEiza)) >= 2)));
                if (hasOtherSummon) return false;
            }

            int majuAtk = Bot.Banished.Count * 400;

            // NEVER summon a 0 ATK Gren Maju!
            if (majuAtk == 0) return false;

            // If Gren Maju has less than 2400 ATK (fewer than 6 banished cards), AND we have Trirealm starters in hand:
            // Let the Trirealm starters summon first! (Sheol banishes 2, Tuonela banishes 3, Naraka banishes 4, etc.)
            bool hasOtherStarter = Bot.Hand.Any(c => c != null && c.IsMonster() && c.Level <= 4 && !c.IsCode(CardId.GrenMajuDaEiza));
            if (hasOtherStarter && majuAtk < 2400)
            {
                return false;
            }

            // If opponent has an active attack-position threat that is NOT disabled and exceeds Maju's ATK:
            int enemyBestActiveAtk = Enemy.GetMonsters()
                .Where(m => m != null && m.IsFaceup() && m.IsAttack() && !m.IsDisabled())
                .Select(m => m.Attack)
                .DefaultIfEmpty(0)
                .Max();

            if (enemyBestActiveAtk > 0 && majuAtk <= enemyBestActiveAtk)
            {
                if (hasOtherStarter) return false;
                if (Bot.GetMonsterCount() > 0) return false;
            }

            return true;
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            if (card.IsCode(CardId.GrenMajuDaEiza)) return true;
            if (card.IsCode(CardId.Number100NumeronDragon)) return true;
            if (card.IsCode(CardId.GizmekOrochi)) return true;
            if (card.IsCode(CardId.TrirealmRiftDarkness)) return true;
            if (card.IsCode(CardId.Dingirsu)) return true;
            if (card.IsCode(CardId.Number97Draglubion)) return true;
            if (card.IsCode(CardId.HopeHarbinger)) return true;
            if (card.IsCode(CardId.PhotonLord)) return true;
            if (card.IsCode(CardId.UnderworldGoddess)) return true;
            if (card.IsCode(CardId.TopologicZeroboros)) return true;
            return base.IsAceCard(card);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (c.IsCode(CardId.GrenMajuDaEiza)) return 99999;
            if (c.IsCode(CardId.Number100NumeronDragon)) return 99999;
            if (c.IsCode(CardId.UnderworldGoddess)) return 9000;
            if (c.IsCode(CardId.Dingirsu)) return 9000;
            if (c.IsCode(CardId.Number97Draglubion)) return 9000;
            if (c.IsCode(CardId.HopeHarbinger)) return 9000;
            if (c.IsCode(CardId.PhotonLord)) return 9000;
            if (c.IsCode(CardId.TrirealmRiftDarkness)) return 5000;
            if (c.IsCode(CardId.GizmekOrochi)) return 5000;
            if (c.IsCode(CardId.SkyThunderTrirealmRiftYomi)) return 3000;
            if (c.IsCode(CardId.BurialSummitTrirealmRiftHelheim)) return 3000;
            return base.GetMaterialPriority(c);
        }

        private bool IsTrirealm(ClientCard card)
        {
            if (card == null) return false;
            return TrirealmMonsters.Contains(card.Id)
                || card.IsCode(CardId.TrirealmRiftTerritoryValvols)
                || card.IsCode(CardId.TrirealmRiftGospel)
                || card.IsCode(CardId.TrirealmRiftJudgment);
        }

        private bool IsTrirealmMonster(ClientCard card)
        {
            if (card == null) return false;
            return TrirealmMonsters.Contains(card.Id);
        }

        // ═══════════════════════════════════════════════════════════════
        // HOOK OVERRIDES (OnSelectPosition, OnSelectEffectYn, OnSelectCard)
        // ═══════════════════════════════════════════════════════════════

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (positions == null || positions.Count == 0) return CardPosition.FaceUpAttack;
            if (positions.Count == 1) return positions[0];

            // 0. Gren Maju Da Eiza & Numeron Dragon: ALWAYS FaceUpAttack on Turn 2+ to execute OTK!
            if (cardId == CardId.GrenMajuDaEiza || cardId == CardId.Number100NumeronDragon)
            {
                if (Duel.Turn > 1 && positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
                if (positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence;
            }

            // 1. Number 41: Bagooska MUST ALWAYS be summoned in FaceUpDefence!
            if (cardId == CardId.Number41Bagooska)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            }

            // 2. High DEF walls -> Defense
            if (cardId == CardId.SkyThunderTrirealmRiftYomi // 1000 ATK / 2500 DEF
                || cardId == CardId.TrirealmRiftOfBlueTuonela // 300 ATK / 2000 DEF
                || cardId == CardId.TrirealmRiftOfEmptinessGehenna) // 100 ATK / 1000 DEF
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            }

            // 3. High ATK Bosses -> Attack
            if (cardId == CardId.TrirealmRiftDarkness
                || cardId == CardId.ChaosAngel
                || cardId == CardId.RelinquishedAnima
                || cardId == CardId.Dingirsu
                || cardId == CardId.HopeHarbinger
                || cardId == CardId.PhotonLord
                || cardId == CardId.Number97Draglubion
                || cardId == CardId.TopologicZeroboros
                || cardId == CardId.BurialSummitTrirealmRiftHelheim
                || cardId == CardId.GizmekOrochi
                || cardId == CardId.MadTempestTrirealmRiftPloutonion
                || cardId == CardId.TrirealmRiftOfScarletNaraka
                || cardId == CardId.UnderworldGoddess)
            {
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            if (card == null) return null;

            // RULE 14: Never accept opponent's optional effects!
            if (card.Controller == 1) return false;

            // Dingirsu: Detach material to protect Gren Maju / cards from destruction!
            if (card.IsCode(CardId.Dingirsu)) return true;

            // Numeron Dragon: Detach to gain 9000-17000 ATK!
            if (card.IsCode(CardId.Number100NumeronDragon)) return true;

            // Draglubion: Detach to Special Summon Numeron Dragon!
            if (card.IsCode(CardId.Number97Draglubion)) return true;

            // Hope Harbinger / Photon Lord negations
            if (card.IsCode(CardId.HopeHarbinger) || card.IsCode(CardId.PhotonLord)) return true;

            return base.OnSelectEffectYn(card, desc);
        }

        public override bool OnSelectYesNo(long desc)
        {
            // Valvols (100458039, opt 1) / Gospel (100458040, opt 2): "Banish from GY instead of Deck?"
            if (desc == Util.GetStringId(CardId.TrirealmRiftTerritoryValvols, 1) || desc == Util.GetStringId(CardId.TrirealmRiftGospel, 2))
            {
                // Always say YES to banishing from GY instead of Deck to protect Main Deck bosses!
                return Bot.Graveyard.Count > 0;
            }

            // Reject opponent choices
            if (Duel.LastChainPlayer == 1) return false;
            return true;
        }

        public override ClientCard OnSelectAttacker(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            if (attackers == null || attackers.Count == 0) return null;

            // 1. Bait Linkuriboh (41999284) with a non-Maju monster!
            bool enemyHasLinkuriboh = defenders != null && defenders.Any(d => d != null && d.IsCode(41999284) && !d.IsDisabled());
            if (enemyHasLinkuriboh && attackers.Count > 1)
            {
                var nonMaju = attackers.FirstOrDefault(c => c != null && !c.IsCode(CardId.GrenMajuDaEiza) && !c.IsCode(CardId.Number100NumeronDragon));
                if (nonMaju != null) return nonMaju;
            }

            // 2. If opponent has face-down backrow and we have non-Maju attackers, bait backrow first!
            bool oppHasBackrow = Enemy.GetSpells().Any(s => s != null && s.IsFacedown());
            if (oppHasBackrow && attackers.Count > 1)
            {
                var baitAttacker = attackers.FirstOrDefault(c => c != null && !c.IsCode(CardId.GrenMajuDaEiza) && !c.IsCode(CardId.Number100NumeronDragon) && c.Attack >= 1500);
                if (baitAttacker != null) return baitAttacker;
            }

            // 3. Strike with highest ATK first for OTK
            if (defenders == null || defenders.Count == 0)
            {
                return attackers.Where(c => c != null)
                                .OrderByDescending(c => c.IsCode(CardId.GrenMajuDaEiza) ? Bot.Banished.Count * 400 : (c.IsCode(CardId.Number100NumeronDragon) ? 9000 : c.Attack))
                                .FirstOrDefault();
            }

            return base.OnSelectAttacker(attackers, defenders);
        }

        public override BattlePhaseAction OnSelectAttackTarget(ClientCard attacker, IList<ClientCard> defenders)
        {
            if (attacker == null) return null;

            // 1. Dynamic RealPower calculation
            if (attacker.IsCode(CardId.GrenMajuDaEiza))
            {
                attacker.RealPower = Bot.Banished.Count * 400;
            }
            else if (attacker.IsCode(CardId.Number100NumeronDragon))
            {
                attacker.RealPower = Math.Max(attacker.Attack, 9000);
            }
            else
            {
                attacker.RealPower = attacker.Attack;
            }

            // 2. Attack safe targets
            if (defenders != null && defenders.Count > 0)
            {
                var sortedDefenders = defenders.Where(d => d != null).OrderByDescending(d => {
                    int defPower = d.GetDefensePower();
                    if (defPower < 0) defPower = 3000;
                    // Lethal attack on attack-position monster wins the game immediately!
                    if (d.IsAttack() && (attacker.RealPower - d.Attack) >= Enemy.LifePoints) return 100000;
                    // Attack-position monsters deal direct LP battle damage (lower ATK = more damage dealt)
                    if (d.IsAttack()) return 50000 + (attacker.RealPower - d.Attack);
                    // High-threat floodgate or negator in defense
                    if (CardIntelligence.IsFloodgate(d.Id) || CardIntelligence.IsKnownNegator(d.Id)) return 20000;
                    // Defense monsters ordered by lowest DEF
                    return 1000 - defPower;
                }).ToList();

                foreach (ClientCard defender in sortedDefenders)
                {
                    if (defender == null) continue;
                    defender.RealPower = defender.GetDefensePower();
                    if (defender.RealPower < 0) defender.RealPower = 3000;

                    if (!OnPreBattleBetween(attacker, defender)) continue;

                    if (attacker.RealPower > defender.RealPower ||
                        (attacker.RealPower == defender.RealPower && defender.IsAttack() && Bot.GetMonsterCount() > Enemy.GetMonsterCount()))
                    {
                        return AI.Attack(attacker, defender);
                    }
                }
            }

            // 3. Direct attack if allowed
            if (attacker.CanDirectAttack && (defenders == null || defenders.Count == 0))
            {
                return AI.Attack(attacker, null);
            }

            return null;
        }

        public override bool OnPreBattleBetween(ClientCard attacker, ClientCard defender)
        {
            if (attacker == null || defender == null) return false;

            // RealPower calculation
            if (attacker.IsCode(CardId.GrenMajuDaEiza))
            {
                attacker.RealPower = Math.Max(attacker.Attack, Bot.Banished.Count * 400);
            }
            else if (attacker.IsCode(CardId.Number100NumeronDragon))
            {
                attacker.RealPower = Math.Max(attacker.Attack, 9000);
            }
            else
            {
                attacker.RealPower = attacker.Attack;
            }

            // Suicide Prevention: Dark Magician + Apprentice Illusion Magician (+2000 ATK combat trick)
            bool enemyHasApprentice = Enemy.HasInMonstersZone(30603688) || Enemy.HasInHand(30603688);
            if (enemyHasApprentice && defender.IsCode(46986414))
            {
                if (defender.Attack + 2000 >= attacker.RealPower) return false;
            }

            // Blue-Eyes Twin Burst Dragon (21296502): Cannot be destroyed by battle; banishes attacker!
            if (defender.IsCode(21296502) && !defender.IsDisabled())
            {
                // Allow attack only if it deals lethal damage to win the duel immediately!
                if (defender.IsAttack() && (attacker.RealPower - defender.Attack) >= Enemy.LifePoints) return true;
                // Otherwise protect Gren Maju from being banished by Twin Burst
                if (attacker.IsCode(CardId.GrenMajuDaEiza)) return false;
            }

            // Mekk-Knight Crusadia Avramax (21887175): Gains ATK equal to Special Summoned monster's ATK!
            if (defender.IsCode(21887175) && !defender.IsDisabled())
            {
                // Only Normal Summoned Gren Maju can safely attack Avramax!
                if (!attacker.IsCode(CardId.GrenMajuDaEiza)) return false;
            }

            if (defender.IsAttack() && defender.Attack >= attacker.RealPower) return false;
            if (defender.IsDefense() && defender.Defense > attacker.RealPower) return false;
            if (defender.IsAttack() && (defender.Attack - attacker.RealPower) >= Bot.LifePoints) return false;

            return base.OnPreBattleBetween(attacker, defender);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return new List<ClientCard>();

            // 0.0 Gameciel Kaiju Tribute Selection (Tribute opponent's biggest threat)
            if ((Card != null && Card.IsCode(CardId.GamecielKaiju)) || (LastChainCard != null && LastChainCard.IsCode(CardId.GamecielKaiju)) || (hint == 500 && cards.Any(c => c.Controller == 1)))
            {
                if (cards.Any(c => c.Controller == 1))
                {
                    int[] kaijuPriorities = {
                        21887175, // Mekk-Knight Crusadia Avramax (Untargetable / Battle Boost - MUST BE #1!)
                        41721210, // Dark Magician the Dragon Knight (Breaks Eternal Soul lock!)
                        40908371, // Azure-Eyes Silver Dragon (Protects all dragons from target & destruction)
                        1561110,  // ABC-Dragon Buster (Quick Banish)
                        4280258,  // Apollousa, Bow of the Goddess (Monster Negate)
                        63767246, // Hope Harbinger (Spell Negator)
                        10443957, // Cyber Dragon Infinity (Omninegator)
                        1508649,  // Altergeist Hexstia (Spell/Trap Negator)
                        89538537, // Altergeist Silquitous (Bounce)
                        50954680, // Crystal Wing Synchro Dragon
                        59822133  // Blue-Eyes Spirit Dragon
                    };

                    foreach (int id in kaijuPriorities)
                    {
                        var match = cards.FirstOrDefault(c => c.Controller == 1 && c.IsCode(id));
                        if (match != null) return new List<ClientCard> { match };
                    }

                    var negator = cards.FirstOrDefault(c => c.Controller == 1 && (CardIntelligence.IsKnownNegator(c.Id) || CardIntelligence.IsFloodgate(c.Id)));
                    if (negator != null) return new List<ClientCard> { negator };

                    var highestAtk = cards.Where(c => c.Controller == 1).OrderByDescending(c => c.Attack).FirstOrDefault();
                    if (highestAtk != null) return new List<ClientCard> { highestAtk };
                }
            }

            // 0.02 Cosmic Cyclone Target Selection
            if ((Card != null && Card.IsCode(CardId.CosmicCyclone)) || (LastChainCard != null && LastChainCard.IsCode(CardId.CosmicCyclone)))
            {
                if (cards.Any(c => c.Controller == 1))
                {
                    int[] cyclonePriorities = {
                        48680970, // Eternal Soul (DM - Wipes their monsters!)
                        68462976, // Secret Village of the Spellcasters (Altergeist - Frees Spells!)
                        27541563, // Altergeist Protocol
                        66399653, // Union Hangar (ABC)
                        62089826, // True Light (Blue-Eyes)
                        53936268, // Altergeist Personal Spoofing
                        35146019, // Altergeist Manifestation
                        47222536  // Dark Magical Circle
                    };

                    foreach (int id in cyclonePriorities)
                    {
                        var match = cards.FirstOrDefault(c => c.Controller == 1 && c.IsCode(id));
                        if (match != null) return new List<ClientCard> { match };
                    }

                    var faceup = cards.FirstOrDefault(c => c.Controller == 1 && c.IsFaceup());
                    if (faceup != null) return new List<ClientCard> { faceup };

                    var anyEnemyST = cards.FirstOrDefault(c => c.Controller == 1);
                    if (anyEnemyST != null) return new List<ClientCard> { anyEnemyST };
                }
            }

            // 0.04 Forbidden Droplet Selection (Cost & Target)
            if ((Card != null && Card.IsCode(CardId.ForbiddenDroplet)) || (LastChainCard != null && LastChainCard.IsCode(CardId.ForbiddenDroplet)))
            {
                if (cards.All(c => c.Controller == 0))
                {
                    // Cost selection
                    var sortedCosts = cards.OrderBy(c => {
                        if (c == null) return 999;
                        if (c.IsCode(CardId.GrenMajuDaEiza, CardId.TrirealmRiftDarkness)) return 999;
                        if (c.Location == CardLocation.Hand && (c.IsCode(CardId.TrirealmRiftGospel) || c.IsCode(CardId.TrirealmRiftTerritoryValvols)) && Bot.Hand.Count(h => h.IsCode(c.Id)) == 1) return 800;
                        if (c.Location == CardLocation.SpellZone) return 1;
                        if (c.IsCode(CardId.AshBlossom, CardId.InfiniteImpermanence)) return 2;
                        if (c.IsMonster() && c.Level <= 4) return 3;
                        if (c.IsSpell()) return 4;
                        return 10;
                    }).Take(min).ToList();
                    return sortedCosts;
                }
                if (cards.Any(c => c.Controller == 1))
                {
                    // Target selection
                    int[] dropletPrios = {
                        10443957, // Cyber Dragon Infinity
                        1561110,  // ABC-Dragon Buster
                        4280258,  // Apollousa
                        21887175, // Avramax
                        41721210, // Dragon Knight
                        1508649,  // Hexstia
                        89538537, // Silquitous
                        50954680, // Crystal Wing
                        59822133  // Blue-Eyes Spirit Dragon
                    };
                    var targets = cards.Where(c => c.Controller == 1 && c.IsFaceup() && !c.IsDisabled())
                        .OrderByDescending(c => {
                            if (dropletPrios.Contains(c.Id)) return 1000;
                            if (CardIntelligence.IsKnownNegator(c.Id)) return 500;
                            return c.Attack;
                        }).Take(min).ToList();
                    return targets;
                }
            }

            // 0.05 Pot of Prosperity Selection
            if ((Card != null && Card.IsCode(CardId.PotOfProsperity)) || (LastChainCard != null && LastChainCard.IsCode(CardId.PotOfProsperity)))
            {
                // Extra Deck banish selection (Cost)
                if (cards.All(c => c.Location == CardLocation.Extra))
                {
                    var safeExtra = cards.Where(c => c != null &&
                        !c.IsCode(CardId.Number41Bagooska) &&
                        !c.IsCode(CardId.SPLittleKnight) &&
                        !c.IsCode(CardId.SuperStarslayerTYPHON) &&
                        !c.IsCode(CardId.Dingirsu) &&
                        !c.IsCode(CardId.HopeHarbinger)
                    ).ToList();

                    if (safeExtra.Count >= min)
                    {
                        return safeExtra.Take(min).ToList();
                    }
                    return cards.OrderBy(c => c.IsCode(CardId.Number41Bagooska) || c.IsCode(CardId.SPLittleKnight) || c.IsCode(CardId.SuperStarslayerTYPHON) ? 999 : 0).Take(min).ToList();
                }

                // Excavated deck card selection (Add to hand)
                if (cards.All(c => c.Location == CardLocation.Deck))
                {
                    // High-threat backrow floodgates: Cosmic Cyclone (Eternal Soul, Secret Village, Protocol, Union Hangar)
                    if (Enemy.GetSpells().Any(s => s != null && s.IsFaceup() && (s.IsCode(48680970) || s.IsCode(68462976) || s.IsCode(27541563) || s.IsCode(66399653))))
                    {
                        var cyclone = cards.FirstOrDefault(c => c.IsCode(CardId.CosmicCyclone));
                        if (cyclone != null) return new List<ClientCard> { cyclone };
                    }

                    // Opponent monster board: Dark Ruler No More
                    if (Duel.Turn >= 2 && Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && !m.IsDisabled() && (CardIntelligence.IsKnownNegator(m.Id) || m.Attack >= 2500 || m.IsExtraCard())))
                    {
                        var drnm = cards.FirstOrDefault(c => c.IsCode(CardId.DarkRulerNoMore) && !Bot.HasInHand(CardId.DarkRulerNoMore));
                        if (drnm != null) return new List<ClientCard> { drnm };
                    }

                    if (!Bot.HasInHand(CardId.GrenMajuDaEiza))
                    {
                        var maju = cards.FirstOrDefault(c => c.IsCode(CardId.GrenMajuDaEiza));
                        if (maju != null) return new List<ClientCard> { maju };
                    }

                    // Prioritize Helheim (summons Darkness directly from banished!)
                    if (!Bot.HasInHand(CardId.BurialSummitTrirealmRiftHelheim) && !Bot.GetMonsters().Any(c => c != null && c.IsCode(CardId.BurialSummitTrirealmRiftHelheim)))
                    {
                        var helheim = cards.FirstOrDefault(c => c.IsCode(CardId.BurialSummitTrirealmRiftHelheim));
                        if (helheim != null) return new List<ClientCard> { helheim };
                    }

                    if (Enemy.GetMonsterCount() > 0)
                    {
                        var drnm = cards.FirstOrDefault(c => c.IsCode(CardId.DarkRulerNoMore) && !Bot.HasInHand(CardId.DarkRulerNoMore));
                        if (drnm != null) return new List<ClientCard> { drnm };

                        var raigeki = cards.FirstOrDefault(c => c.IsCode(CardId.Raigeki) && !Bot.HasInHand(CardId.Raigeki));
                        if (raigeki != null) return new List<ClientCard> { raigeki };

                        var kaiju = cards.FirstOrDefault(c => c.IsCode(CardId.GamecielKaiju) && !Bot.HasInHand(CardId.GamecielKaiju));
                        if (kaiju != null) return new List<ClientCard> { kaiju };
                    }

                    if (Enemy.GetSpellCount() > 0)
                    {
                        var cyclone = cards.FirstOrDefault(c => c.IsCode(CardId.CosmicCyclone) && !Bot.HasInHand(CardId.CosmicCyclone));
                        if (cyclone != null) return new List<ClientCard> { cyclone };

                        var duster = cards.FirstOrDefault(c => c.IsCode(CardId.HarpieFeatherDuster) && !Bot.HasInHand(CardId.HarpieFeatherDuster));
                        if (duster != null) return new List<ClientCard> { duster };

                        var storm = cards.FirstOrDefault(c => c.IsCode(CardId.LightningStorm) && !Bot.HasInHand(CardId.LightningStorm));
                        if (storm != null) return new List<ClientCard> { storm };
                    }

                    if (!Bot.HasInHand(CardId.TrirealmRiftTerritoryValvols) && !Bot.SpellZone.Any(s => s != null && s.IsCode(CardId.TrirealmRiftTerritoryValvols)))
                    {
                        var valvols = cards.FirstOrDefault(c => c.IsCode(CardId.TrirealmRiftTerritoryValvols));
                        if (valvols != null) return new List<ClientCard> { valvols };
                    }

                    if (!Bot.HasInHand(CardId.TrirealmRiftGospel) && !Bot.SpellZone.Any(s => s != null && s.IsCode(CardId.TrirealmRiftGospel)))
                    {
                        var gospel = cards.FirstOrDefault(c => c.IsCode(CardId.TrirealmRiftGospel));
                        if (gospel != null) return new List<ClientCard> { gospel };
                    }

                    if (!Bot.HasInHand(CardId.SmallWorld))
                    {
                        var sw = cards.FirstOrDefault(c => c.IsCode(CardId.SmallWorld));
                        if (sw != null) return new List<ClientCard> { sw };
                    }

                    if (!Bot.HasInHand(CardId.GizmekOrochi) && !Bot.HasInGraveyard(CardId.GizmekOrochi))
                    {
                        var orochi = cards.FirstOrDefault(c => c.IsCode(CardId.GizmekOrochi));
                        if (orochi != null) return new List<ClientCard> { orochi };
                    }

                    var naraka = cards.FirstOrDefault(c => c.IsCode(CardId.TrirealmRiftOfScarletNaraka));
                    if (naraka != null) return new List<ClientCard> { naraka };

                    var gehenna = cards.FirstOrDefault(c => c.IsCode(CardId.TrirealmRiftOfEmptinessGehenna));
                    if (gehenna != null) return new List<ClientCard> { gehenna };

                    return new List<ClientCard> { cards.First() };
                }
            }

            // 0.07 Gospel Field Morph Selection (Banish Lv 4- from field -> SS Lv 5+ boss from banished)
            if ((Card != null && Card.IsCode(CardId.TrirealmRiftGospel)) || (LastChainCard != null && LastChainCard.IsCode(CardId.TrirealmRiftGospel)))
            {
                // Step 1: Banish Level 4 or lower from field
                if (cards.All(c => c.Location == CardLocation.MonsterZone && c.Controller == 0))
                {
                    var fodder = cards.Where(c => c != null && IsTrirealmMonster(c) && c.Level <= 4 && !c.IsCode(CardId.GrenMajuDaEiza))
                                      .OrderBy(c => c.Attack)
                                      .FirstOrDefault();
                    if (fodder != null) return new List<ClientCard> { fodder };
                }

                // Step 2: Special Summon Level 5+ from face-down banished
                if (cards.All(c => c.Location == CardLocation.Removed && c.Controller == 0))
                {
                    var boss = cards.FirstOrDefault(c => c.IsCode(CardId.TrirealmRiftDarkness))
                            ?? cards.FirstOrDefault(c => c.IsCode(CardId.BurialSummitTrirealmRiftHelheim))
                            ?? cards.FirstOrDefault(c => c.IsCode(CardId.SkyThunderTrirealmRiftYomi))
                            ?? cards.FirstOrDefault(c => c.IsCode(CardId.MadTempestTrirealmRiftPloutonion));
                    if (boss != null) return new List<ClientCard> { boss };
                }
            }

            // 0.071 Trirealm Rift of Scarlet Naraka: Quick Summon Lv 5+ boss from face-down banished
            if (((Card != null && Card.IsCode(CardId.TrirealmRiftOfScarletNaraka)) || (LastChainCard != null && LastChainCard.IsCode(CardId.TrirealmRiftOfScarletNaraka)))
                && cards.All(c => c.Location == CardLocation.Removed && c.Controller == 0))
            {
                var boss = cards.FirstOrDefault(c => c.IsCode(CardId.TrirealmRiftDarkness))
                        ?? cards.FirstOrDefault(c => c.IsCode(CardId.BurialSummitTrirealmRiftHelheim))
                        ?? cards.FirstOrDefault(c => c.IsCode(CardId.MadTempestTrirealmRiftPloutonion))
                        ?? cards.FirstOrDefault(c => c.IsCode(CardId.SkyThunderTrirealmRiftYomi));
                if (boss != null) return new List<ClientCard> { boss };
            }

            // 0.07 Mad Tempest Trirealm Rift Ploutonion Hand Draw 2 Reveal Cost:
            if (((Card != null && Card.IsCode(CardId.MadTempestTrirealmRiftPloutonion))
                || (LastChainCard != null && LastChainCard.IsCode(CardId.MadTempestTrirealmRiftPloutonion)))
                && cards.All(c => c.Location == CardLocation.Hand && c.Controller == 0))
            {
                var target = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.TrirealmRiftDarkness))
                          ?? cards.FirstOrDefault(c => c != null && c.IsCode(CardId.TrirealmRiftOfSkySheol) && Bot.Hand.Count(h => h.IsCode(CardId.TrirealmRiftOfSkySheol)) > 1)
                          ?? cards.FirstOrDefault(c => c != null && IsTrirealmMonster(c) && !c.IsCode(CardId.GrenMajuDaEiza))
                          ?? cards.FirstOrDefault(c => c != null && IsTrirealm(c));
                if (target != null) return new List<ClientCard> { target };
            }

            // 0.075 Trirealm Rift Judgment: Special Summon or add to hand 1 face-down banished Trirealm monster
            if (((Card != null && Card.IsCode(CardId.TrirealmRiftJudgment))
                || (LastChainCard != null && LastChainCard.IsCode(CardId.TrirealmRiftJudgment)))
                && cards.All(c => c.Location == CardLocation.Removed && c.Controller == 0))
            {
                var target = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.TrirealmRiftDarkness))
                          ?? cards.FirstOrDefault(c => c != null && c.IsCode(CardId.BurialSummitTrirealmRiftHelheim))
                          ?? cards.FirstOrDefault(c => c != null && c.IsCode(CardId.MadTempestTrirealmRiftPloutonion))
                          ?? cards.FirstOrDefault(c => c != null && c.IsCode(CardId.SkyThunderTrirealmRiftYomi))
                          ?? cards.FirstOrDefault(c => c != null && IsTrirealmMonster(c));
                if (target != null) return new List<ClientCard> { target };
            }

            // 0.08 Valvols / Gospel Graveyard Banish Cost Selection (Prefer used S/T to preserve Main Deck)
            if (((Card != null && (Card.IsCode(CardId.TrirealmRiftTerritoryValvols) || Card.IsCode(CardId.TrirealmRiftGospel)))
                || (LastChainCard != null && (LastChainCard.IsCode(CardId.TrirealmRiftTerritoryValvols) || LastChainCard.IsCode(CardId.TrirealmRiftGospel))))
                && cards.All(c => c.Location == CardLocation.Grave && c.Controller == 0))
            {
                var gyOrder = cards.OrderBy(c =>
                {
                    if (c.IsCode(CardId.GrenMajuDaEiza) || c.IsCode(CardId.TrirealmRiftDarkness)) return 999;
                    if (c.IsSpell() || c.IsTrap()) return 1;
                    if (c.IsExtraCard()) return 2;
                    return 10;
                }).ToList();

                return gyOrder.Take(min).ToList();
            }

            // 0.1 Number 97: Draglubion Selection
            // Summons Number 100: Numeron Dragon, attaches Number 38: Hope Harbinger!
            if ((Card != null && Card.IsCode(CardId.Number97Draglubion)) || (LastChainCard != null && LastChainCard.IsCode(CardId.Number97Draglubion)))
            {
                // When selecting monster to Special Summon
                var numeronTarget = cards.FirstOrDefault(c => c.IsCode(CardId.Number100NumeronDragon));
                if (numeronTarget != null) return new List<ClientCard> { numeronTarget };

                // When selecting monster to attach
                var hopeTarget = cards.FirstOrDefault(c => c.IsCode(CardId.HopeHarbinger));
                if (hopeTarget != null) return new List<ClientCard> { hopeTarget };
            }

            // 0.2 Dingirsu Send to GY Selection (Non-targeting removal)
            if (Card != null && Card.IsCode(CardId.Dingirsu) && cards.Any(c => c.Controller == 1))
            {
                var bestTarget = Plugin.ThreatImpl.PickBestRemovalTarget(cards);
                if (bestTarget != null) return new List<ClientCard> { bestTarget };
            }

            // 0.3 Trirealm Rift Darkness Quick Destroy Targeting
            if ((Card != null && Card.IsCode(CardId.TrirealmRiftDarkness)) || (LastChainCard != null && LastChainCard.IsCode(CardId.TrirealmRiftDarkness)))
            {
                if (cards.Any(c => c.Controller == 1))
                {
                    var eternalSoul = cards.FirstOrDefault(c => c != null && c.Controller == 1 && c.IsCode(48680970));
                    if (eternalSoul != null) return new List<ClientCard> { eternalSoul };

                    var bestTarget = Plugin.ThreatImpl.PickBestRemovalTarget(cards);
                    if (bestTarget != null) return new List<ClientCard> { bestTarget };
                    return new List<ClientCard> { cards.First(c => c.Controller == 1) };
                }

                // Friendly fire safeguard: If forced to select our own card, sacrifice the lowest value card, NEVER Judgment or Darkness!
                var friendly = cards.Where(c => c != null && c.Controller == 0)
                                    .OrderBy(c => c.IsCode(CardId.TrirealmRiftDarkness) ? 9999 :
                                                  c.IsCode(CardId.TrirealmRiftJudgment) ? 9000 :
                                                  c.IsCode(CardId.TrirealmRiftGospel) ? 8000 :
                                                  c.IsCode(CardId.TrirealmRiftTerritoryValvols) ? 7000 :
                                                  c.Attack)
                                    .FirstOrDefault();
                if (friendly != null) return new List<ClientCard> { friendly };
            }

            // 0.31 Chaos Angel Banish Target Selection
            if (Card != null && Card.IsCode(CardId.ChaosAngel) && cards.Any(c => c.Controller == 1))
            {
                var bestTarget = Plugin.ThreatImpl.PickBestRemovalTarget(cards);
                if (bestTarget != null) return new List<ClientCard> { bestTarget };
            }

            // 0.32 Relinquished Anima Absorb Target Selection
            if (Card != null && Card.IsCode(CardId.RelinquishedAnima) && cards.Any(c => c.Controller == 1))
            {
                var bestMonster = cards.Where(c => c.Controller == 1 && c.IsMonster()).OrderByDescending(c => c.Attack).FirstOrDefault();
                if (bestMonster != null) return new List<ClientCard> { bestMonster };
            }

            // 0.4 Forbidden Lance Target Selection (Prioritize Gren Maju)
            if ((Card != null && Card.IsCode(CardId.ForbiddenLance)) || (LastChainCard != null && LastChainCard.IsCode(CardId.ForbiddenLance)))
            {
                var maju = cards.FirstOrDefault(c => c != null && c.Controller == 0 && c.IsCode(CardId.GrenMajuDaEiza));
                if (maju != null) return new List<ClientCard> { maju };

                var boss = cards.FirstOrDefault(c => c != null && c.Controller == 0 && (c.IsCode(CardId.SkyThunderTrirealmRiftYomi) || c.IsCode(CardId.BurialSummitTrirealmRiftHelheim) || c.IsCode(CardId.TrirealmRiftDarkness)))
                    ?? cards.FirstOrDefault(c => c != null && c.Controller == 0);
                if (boss != null) return new List<ClientCard> { boss };
            }

            // 0.45 Darkness Tribute Selection (Tribute low monsters, NEVER Gren Maju!)
            if (Card != null && Card.IsCode(CardId.TrirealmRiftDarkness) && cards.All(c => c.Controller == 0 && c.Location == CardLocation.MonsterZone))
            {
                var tributes = cards.Where(c => c != null && !c.IsCode(CardId.GrenMajuDaEiza) && !c.IsCode(CardId.TrirealmRiftDarkness))
                                    .OrderBy(c => Plugin.MaterialImpl.GetMaterialCost(c))
                                    .Take(min)
                                    .ToList();
                if (tributes.Count >= min) return tributes;
            }

            // 0.5 Underworld Goddess Material Selection (Steal enemy monster)
            if (Card != null && Card.IsCode(CardId.UnderworldGoddess) && cards.Any(c => c.Controller == 1))
            {
                var enemyMonster = cards.FirstOrDefault(c => c != null && c.Controller == 1);
                if (enemyMonster != null && min == 1) return new List<ClientCard> { enemyMonster };
            }

            // 0.6 Knightmare Phoenix Target Selection (Destroy opponent's Spell/Trap)
            if (((Card != null && Card.IsCode(CardId.KnightmarePhoenix)) || (LastChainCard != null && LastChainCard.IsCode(CardId.KnightmarePhoenix))) && cards.Any(c => c.Controller == 1))
            {
                var eternalSoul = cards.FirstOrDefault(c => c != null && c.IsCode(48680970));
                if (eternalSoul != null) return new List<ClientCard> { eternalSoul };

                var bestSpell = cards.OrderByDescending(c => CardIntelligence.IsFloodgate(c.Id))
                                     .ThenByDescending(c => c.IsFaceup())
                                     .FirstOrDefault();
                if (bestSpell != null) return new List<ClientCard> { bestSpell };
            }

            // 0.7 Knightmare Unicorn Target Selection (Shuffle opponent's card)
            if (((Card != null && Card.IsCode(CardId.KnightmareUnicorn)) || (LastChainCard != null && LastChainCard.IsCode(CardId.KnightmareUnicorn))) && cards.Any(c => c.Controller == 1))
            {
                var bestTarget = Plugin.ThreatImpl.PickBestRemovalTarget(cards);
                if (bestTarget != null) return new List<ClientCard> { bestTarget };
            }

            // 0.8 Called by the Grave Target Selection
            if (Card != null && Card.IsCode(CardId.CalledByTheGrave))
            {
                var keyTarget = cards.FirstOrDefault(c => c != null && (
                    CardIntelligence.IsHandtrap(c.Id)
                    || c.Id == 71039903 || c.Id == 79814787 || c.Id == 89631139 || c.Id == 46986414
                    || CardIntelligence.IsKnownNegator(c.Id)
                )) ?? cards.FirstOrDefault();
                if (keyTarget != null) return new List<ClientCard> { keyTarget };
            }

            // 1. Small World Resolution (100% Guaranteed Gren Maju via Naraka!)
            if ((Card != null && Card.IsCode(CardId.SmallWorld)) || (LastChainCard != null && LastChainCard.IsCode(CardId.SmallWorld)))
            {
                // Step A: From Hand (reveal starter/fodder)
                if (cards.All(c => c.Location == CardLocation.Hand))
                {
                    // Reveal any Trirealm monster (or Gizmek Orochi), preserve Gren Maju and Darkness
                    var preferredReveal = cards.FirstOrDefault(c => c != null && !c.IsCode(CardId.GrenMajuDaEiza) && !c.IsCode(CardId.TrirealmRiftDarkness) && !c.IsCode(CardId.GamecielKaiju))
                                       ?? cards.FirstOrDefault(c => c != null && !c.IsCode(CardId.GrenMajuDaEiza) && !c.IsCode(CardId.TrirealmRiftDarkness))
                                       ?? cards.FirstOrDefault();
                    if (preferredReveal != null) return new List<ClientCard> { preferredReveal };
                }

                // Step B / C: From Deck
                if (cards.All(c => c.Location == CardLocation.Deck))
                {
                    // Check if candidate list contains Gren Maju Da Eiza -> THIS IS STEP C (FINAL TARGET)!
                    var targetGrenMaju = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.GrenMajuDaEiza));
                    if (targetGrenMaju != null && !Bot.HasInHand(CardId.GrenMajuDaEiza))
                    {
                        return new List<ClientCard> { targetGrenMaju };
                    }

                    if (Bot.HasInHand(CardId.GrenMajuDaEiza))
                    {
                        bool enemyHasDangerousBoss = Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && (
                            m.Id == 21887175 || m.Id == 41721210 || m.Id == 40908371 || m.Id == 1561110 ||
                            m.Id == 50954680 || m.Id == 63767246 || m.Id == 10443957 ||
                            CardIntelligence.IsKnownNegator(m.Id) || CardIntelligence.IsFloodgate(m.Id) ||
                            m.Attack >= 2500
                        ));
                        if (enemyHasDangerousBoss && !Bot.HasInHand(CardId.GamecielKaiju))
                        {
                            var kaijuTarget = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.GamecielKaiju));
                            if (kaijuTarget != null) return new List<ClientCard> { kaijuTarget };
                        }

                        // Search Helheim (hand boss that summons Darkness directly from banished!)
                        if (!Bot.HasInHand(CardId.BurialSummitTrirealmRiftHelheim) && !Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.BurialSummitTrirealmRiftHelheim)))
                        {
                            var helheimTarget = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.BurialSummitTrirealmRiftHelheim));
                            if (helheimTarget != null) return new List<ClientCard> { helheimTarget };
                        }

                        if (!Bot.HasInHand(CardId.GizmekOrochi) && !Bot.HasInGraveyard(CardId.GizmekOrochi) && Bot.Deck.Count >= 10)
                        {
                            var orochiTarget = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.GizmekOrochi));
                            if (orochiTarget != null) return new List<ClientCard> { orochiTarget };
                        }
                    }

                    // Otherwise, this is STEP B (BRIDGE SELECTION):
                    // Naraka (FIRE / Psychic / Lv 4) bridges ALL Trirealm monsters to Gren Maju 100%!
                    var narakaBridge = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.TrirealmRiftOfScarletNaraka));
                    if (narakaBridge != null) return new List<ClientCard> { narakaBridge };

                    var tuonelaBridge = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.TrirealmRiftOfBlueTuonela));
                    if (tuonelaBridge != null) return new List<ClientCard> { tuonelaBridge };

                    var kaijuBridge = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.GamecielKaiju));
                    if (kaijuBridge != null) return new List<ClientCard> { kaijuBridge };

                    if (targetGrenMaju != null) return new List<ClientCard> { targetGrenMaju };
                    return new List<ClientCard> { cards.First() };
                }
            }

            // 2. Hint 506: ATOHAND (Search target from banished)
            if (hint == 506)
            {
                var targets = Plugin.StrategyImpl.PickSearchTargets(cards, Card, max);
                if (targets != null && targets.Count >= min)
                {
                    return targets;
                }
            }

            // 3. Hint 509: SPSUMMON (Special Summon target from banished)
            if (hint == 509)
            {
                var target = Plugin.StrategyImpl.PickSpecialSummonTarget(cards);
                if (target != null)
                {
                    return new List<ClientCard> { target };
                }
            }

            // 3.5 Hint 500: RELEASE / TRIBUTE SELECTION
            if (hint == 500)
            {
                var safeTributes = cards.Where(c => c != null && c.Controller == 0)
                                        .OrderBy(c => {
                                            if (c.IsCode(CardId.GrenMajuDaEiza)) return 99999;
                                            if (c.IsCode(CardId.TrirealmRiftDarkness)) return 9999;
                                            if (c.IsCode(CardId.BurialSummitTrirealmRiftHelheim)) return 8888;
                                            if (c.IsCode(CardId.SkyThunderTrirealmRiftYomi)) return 7777;
                                            return c.Attack;
                                        })
                                        .Take(min)
                                        .ToList();
                if (safeTributes.Count >= min) return safeTributes;
            }

            // 4. Hint 502 / 503 / 505 / 507: Removal targeting (STRICTLY OPPONENT'S CARDS)
            if (hint == 502 || hint == 503 || hint == 505 || hint == 507)
            {
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyCards.Count > 0)
                {
                    var target = Plugin.ThreatImpl.PickBestRemovalTarget(enemyCards);
                    if (target != null) return new List<ClientCard> { target };
                    return enemyCards.Take(min).ToList();
                }

                // 4.1 Hint 507 with only friendly cards (Yomi monster negate shuffle or Ploutonion spin cost from banished)
                if (hint == 507 && cards.All(c => c.Controller == 0))
                {
                    var safeShuffle = cards.OrderBy(c => {
                        if (c.IsCode(CardId.TrirealmRiftDarkness)) return 999;
                        if (c.IsCode(CardId.BurialSummitTrirealmRiftHelheim)) return 800;
                        if (c.IsCode(CardId.SkyThunderTrirealmRiftYomi)) return 500;
                        if (c.IsCode(CardId.TrirealmRiftOfScarletNaraka)) return 10;
                        if (c.IsCode(CardId.TrirealmRiftOfBlueTuonela)) return 20;
                        if (c.IsCode(CardId.TrirealmRiftOfSkySheol)) return 30;
                        if (c.IsCode(CardId.TrirealmRiftOfEmptinessGehenna)) return 40;
                        return 100;
                    }).FirstOrDefault();
                    if (safeShuffle != null) return new List<ClientCard> { safeShuffle };
                }

                // 4.2 Hint 503 with only friendly cards (Gospel field morph cost OR Gizmek Orochi Extra Deck banish cost)
                if (hint == 503 && cards.All(c => c.Controller == 0))
                {
                    if (cards.Any(c => c.Location == CardLocation.Extra))
                    {
                        // Gizmek Orochi Extra Deck banish cost: MUST return min cards!
                        var safeExtra = cards.OrderBy(c => {
                            if (c.IsCode(CardId.GrenMajuDaEiza) || c.IsCode(CardId.Number100NumeronDragon)) return 99999;
                            if (c.IsCode(CardId.HopeHarbinger) || c.IsCode(CardId.Dingirsu)) return 500;
                            return 100;
                        }).Take(min).ToList();
                        return safeExtra;
                    }

                    var safeBanish = cards.OrderBy(c => {
                        if (c.IsCode(CardId.GrenMajuDaEiza) || c.IsCode(CardId.TrirealmRiftDarkness)) return 99999;
                        if (c.Level >= 5) return 8000;
                        if (c.IsCode(CardId.TrirealmRiftOfEmptinessGehenna)) return 10;
                        if (c.IsCode(CardId.TrirealmRiftOfSkySheol)) return 20;
                        if (c.IsCode(CardId.TrirealmRiftOfBlueTuonela)) return 30;
                        if (c.IsCode(CardId.TrirealmRiftOfScarletNaraka)) return 40;
                        return 100;
                    }).Take(min).ToList();
                    return safeBanish;
                }
            }

            // 5. Hint 500 / 501 / 504: RELEASE / DISCARD / TOGRAVE (NEVER SACRIFICE OR DISCARD GREN MAJU OR NUMERON DRAGON!)
            if (hint == 500 || hint == 501 || hint == 504)
            {
                var safeCards = cards.Where(c => c != null && !c.IsCode(CardId.GrenMajuDaEiza) && !c.IsCode(CardId.Number100NumeronDragon)).ToList();
                if (safeCards.Count >= min)
                {
                    var discardTargets = Plugin.MaterialImpl.PickDiscardTargets(safeCards, min);
                    if (discardTargets != null && discardTargets.Count >= min)
                    {
                        return discardTargets;
                    }
                    return safeCards.Take(min).ToList();
                }
            }

            // 6. Hint 533: Material selection for Link / Xyz (NEVER SACRIFICE GREN MAJU OR NUMERON DRAGON!)
            if (hint == 533)
            {
                var safeMaterials = cards.Where(c => c != null && !c.IsCode(CardId.GrenMajuDaEiza) && !c.IsCode(CardId.Number100NumeronDragon)).ToList();
                if (safeMaterials.Count >= min)
                {
                    var ordered = safeMaterials.OrderBy(c => GetMaterialPriority(c)).ToList();
                    return ordered.Take(min).ToList();
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0) return 0;
            if (options.Count == 1) return 0;

            if ((Card != null && Card.IsCode(CardId.PotOfExtravagance)) || (LastChainCard != null && LastChainCard.IsCode(CardId.PotOfExtravagance)))
            {
                // Option 0: banish 3 (draw 1), Option 1: banish 6 (draw 2)
                if (options.Count > 1 && Bot.ExtraDeck.Count >= 6) return 1;
                return 0;
            }

            if ((Card != null && Card.IsCode(CardId.PotOfProsperity)) || (LastChainCard != null && LastChainCard.IsCode(CardId.PotOfProsperity)))
            {
                // Option 0: banish 3 (excavate 3), Option 1: banish 6 (excavate 6)
                if (options.Count > 1 && Bot.ExtraDeck.Count >= 6) return 1;
                return 0;
            }

            // Judgment: Option 0: Add to Hand, Option 1: Special Summon
            // Prioritize Special Summon if monster zone has room!
            if (Card != null && Card.IsCode(CardId.TrirealmRiftJudgment))
            {
                for (int i = 0; i < options.Count; i++)
                {
                    long optIndex = options[i] & 0xf;
                    if (optIndex == 1 && Bot.GetMonsterCount() < 5) return i;
                    if (optIndex == 0) return i;
                }
            }

            return base.OnSelectOption(options);
        }
    }
}
