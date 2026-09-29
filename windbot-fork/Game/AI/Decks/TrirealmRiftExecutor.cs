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
            // Main Deck Monsters
            public const int TrirealmRiftOfEmptinessGehenna = 100458031; // Lv 1 DARK (Free SS / Search 2 Monsters)
            public const int TrirealmRiftOfSkySheol = 100458032;        // Lv 2 LIGHT (SS with Trirealm / Search 2 S/T)
            public const int TrirealmRiftOfBlueTuonela = 100458033;     // Lv 3 WATER (SS if outnumbered / SS Lv 5+)
            public const int TrirealmRiftOfScarletNaraka = 100458034;   // Lv 4 FIRE (SS on enemy SS / Quick SS Lv 5+)
            public const int SkyThunderTrirealmRiftYomi = 100458035;    // Lv 5 WIND (Core Boss / Hand Search / Quick Negate)
            public const int BurialSummitTrirealmRiftHelheim = 100458036; // Lv 6 EARTH (Hand SS Boss / Full Field Protection)
            public const int MadTempestTrirealmRiftPloutonion = 100458037; // Lv 7 WATER (Hand Draw 2 / Quick Spin)
            public const int TrirealmRiftDarkness = 100458038;          // Lv 8 DARK (3000 ATK Finisher / Quick Pop / Draw Skip)
            public const int GizmekOrochi = 71197066;                   // Lv 8 DARK (Quick SS Banish 8 face-down / Pop monster)

            // OTK Finishers & Deck Reset Engine
            public const int GrenMajuDaEiza = 36584821;                // Lv 3 FIRE Fiend (Supreme Finisher: ATK = Banished x 400)
            public const int EaterOfMillions = 63845230;               // Lv 1 LIGHT Fiend (Banish 5 Extra Deck -> SS / Banishes battling monster)
            public const int Necroface = 28297833;                     // Lv 4 DARK Zombie (NS: Shuffles ALL banished cards into deck!)
            public const int Nyannyan = 8736823;                       // Lv 3 EARTH Psychic (When banished: Shuffles 1 banished card to deck!)
            public const int RadianKaiju = 28674152;                   // Lv 7 DARK Fiend (Kaiju: Tributes enemy boss / 100% Small World bridge)

            // Spells & Traps
            public const int SmallWorld = 89558743;                    // Normal Spell (Search Gren Maju / Starter 100%)
            public const int ForbiddenDroplet = 24299458;              // Quick-Play Spell (Sends cards, negates all enemy monsters & halves ATK)
            public const int HarpieFeatherDuster = 18144506;           // Normal Spell (Destroys all enemy S/T -> wipes Eternal Soul)
            public const int LightningStorm = 14532163;                // Normal Spell (Wipes enemy attack monsters or backrow)
            public const int MonsterReborn = 83764718;                 // Normal Spell (Revive Gren Maju or Bosses)
            public const int TrirealmRiftTerritoryValvols = 100458039; // Field Spell (Banish 5 / Search 1 / Turn Lockout)
            public const int TrirealmRiftGospel = 100458040;          // Continuous Spell (Banish 5 / Double NS / Morph Lv 4-)
            public const int Raigeki = 12580477;                      // Normal Spell (Destroys all opponent monsters -> clears way for Gren Maju)
            public const int TrirealmRiftJudgment = 100458041;        // Counter Trap (Summon Intercept / ATK 0)
            public const int PotOfDesires = 35261759;
            public const int PotOfExtravagance = 49238328;             // Normal Spell (Banish 6 Extra Deck face-down -> Draw 2! +2400 ATK fuel)
            public const int CalledByTheGrave = 24224830;              // Quick-Play Spell (Banish 1 enemy GY monster & negate / protects Gren Maju)
            public const int Terraforming = 73628505;
            public const int InfiniteImpermanence = 10045474;

            // Extra Deck
            public const int UnderworldGoddess = 98127546;             // Link 5 (Uses 1 enemy monster / Negates all enemy monsters / Shuts GY revive)
            public const int Dingirsu = 93854893;                      // Rank 8 (Non-target send 1 enemy card to GY / Detach to protect)
            public const int HopeHarbinger = 63767246;                 // Rank 8 (Spell negate / Attach as material / Attack redirect)
            public const int TopologicZeroboros = 66403530;            // Link 4 (+200 ATK per banished = 9,000 ATK, Field Wipe Banish)
            public const int AccesscodeTalker = 86066372;
            public const int KnightmareUnicorn = 38342335;
            public const int KnightmarePhoenix = 2857636;
            public const int SPLittleKnight = 29301450;
            public const int IPMasquerena = 65741786;
            public const int AbyssDweller = 21044178;
            public const int Number41Bagooska = 90590303;
            public const int DivineArsenalAAZEUS = 90448279;
            public const int SuperStarslayerTYPHON = 93039339;
            public const int Linkuriboh = 41999284;
            public const int RelinquishedAnima = 94259633;
            public const int DharcTheDarkCharmer = 8264361;
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

        public TrirealmRiftExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // 0. Install Decoupled Domain Plugin
            DeckPlugin = new TrirealmRiftPlugin(this);
            Plugin = (TrirealmRiftPlugin)DeckPlugin;

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 1: QUICK COUNTERS & HANDTRAPS (Chain Priority)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, OnCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.SkyThunderTrirealmRiftYomi, OnYomiQuickNegate);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, OnForbiddenDroplet);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, DefaultInfiniteImpermanence);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 2: BOARD BREAKERS & OPPONENT TURN DISRUPTIONS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.PotOfExtravagance, OnPotOfExtravagance); // MUST activate first at start of MP1!
            AddExecutor(ExecutorType.SpSummon, CardId.RadianKaiju, OnRadianKaijuSummon);
            AddExecutor(ExecutorType.Activate, CardId.HarpieFeatherDuster, OnHarpieFeatherDuster);
            AddExecutor(ExecutorType.Activate, CardId.LightningStorm, OnLightningStorm);
            AddExecutor(ExecutorType.Activate, CardId.Raigeki, OnRaigeki);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, OnForbiddenDroplet);
            AddExecutor(ExecutorType.Activate, CardId.MadTempestTrirealmRiftPloutonion, OnPloutonionQuickSpin);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftDarkness, OnDarknessQuickDestroy);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfScarletNaraka, OnNarakaOpponentSummon);
            AddExecutor(ExecutorType.Activate, CardId.GizmekOrochi, OnGizmekOrochiQuick);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, OnSPLittleKnightActivate);
            AddExecutor(ExecutorType.Activate, CardId.IPMasquerena, OnIPMasquerenaActivate);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 3: ON-SUMMON TRIGGER EFFECTS (All Monsters Banish Top Cards!)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfEmptinessGehenna, OnSummonBanishTrigger);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfSkySheol, OnSummonBanishTrigger);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfBlueTuonela, OnSummonBanishTrigger);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfScarletNaraka, OnSummonBanishTrigger);
            AddExecutor(ExecutorType.Activate, CardId.SkyThunderTrirealmRiftYomi, OnSummonBanishTrigger);
            AddExecutor(ExecutorType.Activate, CardId.BurialSummitTrirealmRiftHelheim, OnSummonBanishTrigger);
            AddExecutor(ExecutorType.Activate, CardId.MadTempestTrirealmRiftPloutonion, OnSummonBanishTrigger);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftDarkness, OnSummonBanishTrigger);
            AddExecutor(ExecutorType.Activate, CardId.Necroface, OnNecrofaceActivate);
            AddExecutor(ExecutorType.Activate, CardId.Nyannyan);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 4: SEARCH ENGINES & BANISH ACCELERATORS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.Terraforming, OnTerraforming);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftTerritoryValvols, OnValvolsHandActivate);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftGospel, OnGospelHandActivate);
            AddExecutor(ExecutorType.Activate, CardId.SmallWorld, OnSmallWorld);
            AddExecutor(ExecutorType.Activate, CardId.MadTempestTrirealmRiftPloutonion, OnPloutonionHandDraw);
            AddExecutor(ExecutorType.Activate, CardId.SkyThunderTrirealmRiftYomi, OnYomiHandSearch);
            AddExecutor(ExecutorType.Activate, CardId.GizmekOrochi, OnGizmekOrochiSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.EaterOfMillions, OnEaterOfMillionsSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.EaterOfMillions);
            AddExecutor(ExecutorType.Activate, CardId.MonsterReborn, OnMonsterReborn);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 5: MAIN PHASE STARTERS, EXTENDERS & SEARCHES
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfEmptinessGehenna, OnGehennaHandSummon);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfEmptinessGehenna, OnGehennaFieldSearch);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfSkySheol, OnSheolHandSummon);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfSkySheol, OnSheolFieldSearch);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfBlueTuonela, OnTuonelaHandSummon);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfBlueTuonela, OnTuonelaFieldSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftTerritoryValvols, OnValvolsFieldSearch);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftGospel, OnGospelFieldMorph);
            AddExecutor(ExecutorType.Activate, CardId.BurialSummitTrirealmRiftHelheim, OnHelheimQuickSummon);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfScarletNaraka, OnNarakaHandSummon);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 6: NORMAL SUMMONS (Smart Gren Maju Timing & Necroface Reset)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Summon, CardId.Necroface, OnNecrofaceSummon);
            AddExecutor(ExecutorType.Summon, CardId.GrenMajuDaEiza, OnGrenMajuSummon);
            AddExecutor(ExecutorType.Summon, CardId.TrirealmRiftDarkness, OnDarknessSummon);
            AddExecutor(ExecutorType.Summon, CardId.TrirealmRiftOfScarletNaraka, OnGeneralNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.TrirealmRiftOfSkySheol, OnGeneralNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.TrirealmRiftOfEmptinessGehenna, OnGeneralNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.TrirealmRiftOfBlueTuonela, OnGeneralNormalSummon);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 7: EXTRA DECK TOOLBOX & FINISHERS (Turn 2+ Only)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.Dingirsu, OnDingirsuSummon);
            AddExecutor(ExecutorType.Activate, CardId.Dingirsu);
            AddExecutor(ExecutorType.SpSummon, CardId.HopeHarbinger, OnHopeHarbingerSummon);
            AddExecutor(ExecutorType.Activate, CardId.HopeHarbinger);
            AddExecutor(ExecutorType.SpSummon, CardId.UnderworldGoddess, OnUnderworldGoddessSummon);
            AddExecutor(ExecutorType.Activate, CardId.UnderworldGoddess);
            AddExecutor(ExecutorType.SpSummon, CardId.TopologicZeroboros, OnZeroborosSummon);
            AddExecutor(ExecutorType.Activate, CardId.TopologicZeroboros);
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, OnAccesscodeSummon);
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
            AddExecutor(ExecutorType.SpSummon, CardId.AbyssDweller, OnAbyssDwellerSummon);
            AddExecutor(ExecutorType.Activate, CardId.AbyssDweller);
            AddExecutor(ExecutorType.SpSummon, CardId.IPMasquerena, OnIPMasquerenaSummon);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 8: SPELL & TRAP BACKROW PLACEMENT
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 9: REPOSITIONING
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // Blind-Second / Board-Breaking OTK deck:
            // Going second allows drawing the 6th card, breaking opponent's board with Duster/Darkness/Droplet,
            // and delivering an 8,000-14,000+ ATK lethal OTK strike with Gren Maju in the Battle Phase!
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
        }

        // ═══════════════════════════════════════════════════════════════
        // DISRUPTIONS & COMBOS
        // ═══════════════════════════════════════════════════════════════

        private bool OnHarpieFeatherDuster()
        {
            // Blow away opponent backrows (destroys Eternal Soul, Traps, Floodgates)
            return Duel.Phase == DuelPhase.Main1 && Enemy.GetSpellCount() > 0;
        }

        private bool OnLightningStorm()
        {
            if (Duel.Phase != DuelPhase.Main1) return false;
            // Lightning storm requires no face-up cards
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup()) || Bot.GetSpells().Any(c => c != null && c.IsFaceup())) return false;
            return Enemy.GetSpellCount() > 0 || Enemy.GetMonsters().Any(c => c != null && c.IsAttack());
        }

        private bool OnRaigeki()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // Destroy all monsters opponent controls -> direct path for lethal Gren Maju
            return Duel.Phase == DuelPhase.Main1 && Enemy.GetMonsterCount() > 0;
        }

        private bool OnForbiddenDroplet()
        {
            if (Duel.LastChainPlayer == 0) return false;

            // If we have Pot of Extravagance in hand, and it's Main Phase 1:
            // ALWAYS let Pot of Extravagance activate FIRST at the start of Main Phase 1!
            if (Duel.Phase == DuelPhase.Main1 && Bot.HasInHand(CardId.PotOfExtravagance) && Bot.ExtraDeck.Count >= 6)
            {
                return false;
            }

            var enemyMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsDisabled() && c.HasType(CardType.Effect)).ToList();
            if (enemyMonsters.Count == 0) return false;

            int totalCostAvailable = Bot.Hand.Count(c => c != null && c != Card && !c.IsCode(CardId.GrenMajuDaEiza))
                + Bot.GetMonsters().Count(c => c != null && !c.IsCode(CardId.GrenMajuDaEiza))
                + Bot.GetSpells().Count(c => c != null && c != Card && (c.IsCode(CardId.TrirealmRiftTerritoryValvols) || c.IsCode(CardId.TrirealmRiftGospel)));
            if (totalCostAvailable == 0) return false;

            if (Duel.Player != 0)
            {
                if (Duel.LastChainPlayer == 1) return true;
                if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle)
                {
                    return enemyMonsters.Any(c => c.Attack >= 2000);
                }
            }
            else
            {
                if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.BattleStart)
                {
                    return enemyMonsters.Any(c => c.Attack >= 2000 
                        || CardIntelligence.IsFloodgate(c.Id) 
                        || CardIntelligence.IsKnownNegator(c.Id)
                        || c.IsShouldNotBeTarget());
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
                    || c.Id == 7084129  // Magicians' Souls
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

        private bool OnPotOfExtravagance()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (Duel.Phase != DuelPhase.Main1) return false;
            if (Bot.ExtraDeck.Count < 6) return false;

            // Extravagance MUST activate at the start of Main Phase 1 before other actions!
            // Banishes 6 cards face-down from Extra Deck (+2400 ATK fuel) and draws 2 cards!
            return true;
        }

        private bool OnSmallWorld()
        {
            if (Card.Location != CardLocation.Hand) return false;

            // 1. If Gren Maju is not in hand, Small World guarantees Gren Maju via bridges!
            if (!Bot.HasInHand(CardId.GrenMajuDaEiza))
            {
                return Bot.Hand.Any(c => c != null && c.IsMonster() && c != Card);
            }

            // 2. If Gren Maju is in hand, check if opponent has a high-threat boss that we can Kaiju
            bool enemyHasBoss = Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && (m.Attack >= 2500 || CardIntelligence.IsKnownNegator(m.Id) || CardIntelligence.IsFloodgate(m.Id)));
            if (enemyHasBoss && !Bot.HasInHand(CardId.RadianKaiju))
            {
                return Bot.Hand.Count(c => c != null && c.IsMonster() && c != Card) >= 2;
            }

            // 3. Check if we need Gizmek Orochi (instant 8 banish fuel + 2450 ATK beatstick)
            if (!Bot.HasInHand(CardId.GizmekOrochi) && !Bot.HasInGraveyard(CardId.GizmekOrochi) && Bot.Deck.Count >= 10)
            {
                return Bot.Hand.Count(c => c != null && c.IsMonster() && c != Card) >= 2;
            }

            // 4. Check if we need a key starter (Gehenna / Darkness)
            bool needsStarter = Bot.GetMonsterCount() == 0 && !Bot.Hand.Any(c => c != null && (c.IsCode(CardId.TrirealmRiftOfEmptinessGehenna) || c.IsCode(CardId.TrirealmRiftOfSkySheol) || c.IsCode(CardId.TrirealmRiftDarkness)));
            return needsStarter && Bot.Hand.Count(c => c != null && c.IsMonster() && c != Card) >= 2;
        }

        private bool OnMonsterReborn()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (Bot.GetMonsterCount() >= 5) return false;

            // 1. Revive Gren Maju from Graveyard (resurrects with full lethal ATK!)
            if (Bot.HasInGraveyard(CardId.GrenMajuDaEiza))
            {
                AI.SelectCard(CardId.GrenMajuDaEiza);
                return true;
            }

            // 2. Revive Bosses (Darkness, Gizmek Orochi, Helheim, Yomi)
            var boss = Bot.Graveyard.FirstOrDefault(c => c != null && (
                c.IsCode(CardId.TrirealmRiftDarkness)
                || c.IsCode(CardId.GizmekOrochi)
                || c.IsCode(CardId.BurialSummitTrirealmRiftHelheim)
                || c.IsCode(CardId.SkyThunderTrirealmRiftYomi)
            ));
            if (boss != null)
            {
                AI.SelectCard(boss);
                return true;
            }

            // 3. Steal high ATK enemy boss
            var enemyBoss = Enemy.Graveyard.Where(c => c != null && c.IsMonster() && c.Attack >= 2500)
                .OrderByDescending(c => c.Attack).FirstOrDefault();
            if (enemyBoss != null)
            {
                AI.SelectCard(enemyBoss);
                return true;
            }

            return false;
        }

        private bool OnTrirealmJudgment()
        {
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Damage)
            {
                return Bot.Deck.Count == 0;
            }

            if (Duel.LastSummonPlayer == 1 || Duel.LastChainPlayer == 1)
            {
                return Bot.Banished.Any(c => c != null && c.IsFacedown() && IsTrirealmMonster(c));
            }

            return false;
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
            int facedownCount = Bot.Banished.Count(c => c != null && c.IsFacedown());
            if (facedownCount < 20) return false;

            // Target Eternal Soul (48680970), Circle, high-threat monster, or backrow
            return Enemy.GetMonsters().Any(c => c != null && !c.IsShouldNotBeTarget())
                || Enemy.GetSpells().Any(c => c != null && !c.IsShouldNotBeTarget());
        }

        private bool OnNarakaOpponentSummon()
        {
            if (Card.Location == CardLocation.MonsterZone && Duel.Player != 0)
            {
                return Bot.Banished.Any(c => c != null && c.IsFacedown() && IsTrirealmMonster(c) && c.Level >= 5);
            }
            return false;
        }

        private bool OnGizmekOrochiQuick()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget() && (c.Attack >= 2000 || CardIntelligence.IsFloodgate(c.Id)));
            }
            return false;
        }

        private bool OnSummonBanishTrigger()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Darkness banishes 8 cards face-down.
            // If Gren Maju is NOT secured (not in hand/field/GY) and Small World is not in hand:
            // Do not risk banishing all our Gren Majus!
            if (Card.IsCode(CardId.TrirealmRiftDarkness))
            {
                bool majuSecured = Bot.HasInHand(CardId.GrenMajuDaEiza)
                    || Bot.HasInGraveyard(CardId.GrenMajuDaEiza)
                    || Bot.GetMonsters().Any(c => c != null && c.IsCode(CardId.GrenMajuDaEiza))
                    || Bot.HasInHand(CardId.SmallWorld);

                if (!majuSecured && Bot.Banished.Count < 15) return false;
            }

            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        // BANISH ACCELERATORS & DRAW ENGINES
        // ═══════════════════════════════════════════════════════════════

        private bool OnEaterOfMillionsSpSummon()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (Bot.GetMonsterCount() >= 5) return false;
            if (Bot.ExtraDeck.Count < 5) return false;

            // Eater removes enemy monsters face-down or pumps Gren Maju by +2000 ATK
            if (Enemy.GetMonsterCount() > 0) return true;
            if (Bot.HasInHand(CardId.GrenMajuDaEiza) || Bot.GetMonsters().Any(c => c != null && c.IsCode(CardId.GrenMajuDaEiza))) return true;

            bool hasNormalStarters = Bot.Hand.Any(c => c != null && (
                c.IsCode(CardId.TrirealmRiftOfEmptinessGehenna)
                || c.IsCode(CardId.TrirealmRiftOfSkySheol)
                || c.IsCode(CardId.TrirealmRiftGospel)));

            return !hasNormalStarters;
        }

        private bool OnPotOfDesires()
        {
            if (Bot.Deck.Count < 12) return false;

            // 1. If Gren Maju is already secured in hand, Desires gives +4000 ATK fuel & +5000 LP!
            if (Bot.HasInHand(CardId.GrenMajuDaEiza))
            {
                return true;
            }

            // 2. If Small World is in hand, use Small World first to secure Gren Maju safely
            if (Bot.HasInHand(CardId.SmallWorld))
            {
                return false;
            }

            // 3. Prioritize starters first
            bool hasSafeStarters = Bot.Hand.Any(c => c != null && (
                c.IsCode(CardId.TrirealmRiftOfEmptinessGehenna)
                || c.IsCode(CardId.TrirealmRiftOfSkySheol)
                || c.IsCode(CardId.TrirealmRiftTerritoryValvols)
                || c.IsCode(CardId.Terraforming)
                || c.IsCode(CardId.TrirealmRiftGospel)));

            if (hasSafeStarters && (Bot.GetMonsterCount() == 0 || !_valvolsSearchedThisTurn))
            {
                return false;
            }

            return true;
        }

        private bool OnTerraforming()
        {
            return !Bot.Hand.Any(c => c != null && c.IsCode(CardId.TrirealmRiftTerritoryValvols))
                && !Bot.SpellZone.Any(c => c != null && c.IsCode(CardId.TrirealmRiftTerritoryValvols));
        }

        private bool OnGizmekOrochiSpSummon()
        {
            if ((Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave) && !_gizmekSummonedThisTurn)
            {
                if (Bot.Deck.Count < 10 || Bot.GetMonsterCount() >= 5) return false;

                // 1. If Gren Maju is in hand, summoning Gizmek banishes 8 cards (+3200 ATK fuel & 2450 beatstick!)
                if (Bot.HasInHand(CardId.GrenMajuDaEiza))
                {
                    _gizmekSummonedThisTurn = true;
                    return true;
                }

                // 2. If Small World in hand and NO Gren Maju yet, hold Gizmek to search Gren Maju first
                if (Bot.HasInHand(CardId.SmallWorld) && !Bot.HasInHand(CardId.GrenMajuDaEiza))
                {
                    return false;
                }

                if (Duel.Turn == 1 && Bot.GetMonsterCount() < 2)
                {
                    bool hasStarters = Bot.Hand.Any(c => c != null && (
                        c.IsCode(CardId.TrirealmRiftOfEmptinessGehenna)
                        || c.IsCode(CardId.TrirealmRiftOfSkySheol)
                        || c.IsCode(CardId.TrirealmRiftGospel)));
                    if (hasStarters) return false;
                }

                _gizmekSummonedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool OnValvolsHandActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return Bot.Deck.Count + Bot.Graveyard.Count >= 5;
            }
            return false;
        }

        private bool OnGospelHandActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
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

        // ═══════════════════════════════════════════════════════════════
        // MAIN PHASE STARTERS, EXTENDERS & SEARCHES
        // ═══════════════════════════════════════════════════════════════

        private bool OnGehennaHandSummon()
        {
            return Card.Location == CardLocation.Hand && Bot.GetMonsterCount() == 0;
        }

        private bool OnGehennaFieldSearch()
        {
            if (Card.Location == CardLocation.MonsterZone && !_gehennaSearchedThisTurn && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                // Do NOT lock ourselves into Trirealm if we still have crucial non-Trirealm spells/effects to use!
                if (Bot.HasInHand(CardId.PotOfExtravagance) && Bot.ExtraDeck.Count >= 6) return false;
                if (Bot.HasInHand(CardId.SmallWorld)) return false;
                if (Bot.HasInHand(CardId.GizmekOrochi) && Bot.Deck.Count >= 10 && !_gizmekSummonedThisTurn) return false;
                if (Bot.HasInHand(CardId.Raigeki) && Enemy.GetMonsterCount() > 0) return false;
                if (Bot.HasInHand(CardId.HarpieFeatherDuster) && Enemy.GetSpellCount() > 0) return false;
                if (Bot.HasInHand(CardId.LightningStorm) && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0)) return false;

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
                // Do NOT lock ourselves into Trirealm if we still have crucial non-Trirealm spells/effects to use!
                if (Bot.HasInHand(CardId.PotOfExtravagance) && Bot.ExtraDeck.Count >= 6) return false;
                if (Bot.HasInHand(CardId.SmallWorld)) return false;
                if (Bot.HasInHand(CardId.GizmekOrochi) && Bot.Deck.Count >= 10 && !_gizmekSummonedThisTurn) return false;
                if (Bot.HasInHand(CardId.Raigeki) && Enemy.GetMonsterCount() > 0) return false;
                if (Bot.HasInHand(CardId.HarpieFeatherDuster) && Enemy.GetSpellCount() > 0) return false;
                if (Bot.HasInHand(CardId.LightningStorm) && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0)) return false;

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
                bool hasBanishedBoss = Bot.Banished.Any(c => c != null && c.IsFacedown() && IsTrirealmMonster(c) && c.Level >= 5);
                if (lv4Fodder != null && hasBanishedBoss)
                {
                    _gospelMorphedThisTurn = true;
                    return true;
                }
            }
            return false;
        }

        private bool OnHelheimQuickSummon()
        {
            if (Card.Location == CardLocation.Hand && !_helheimSummonedThisTurn)
            {
                bool hasBanishedTarget = Bot.Banished.Any(c => c != null && c.IsFacedown() && IsTrirealmMonster(c) && c.Level != 6);
                if (hasBanishedTarget && Bot.GetMonsterCount() < 5)
                {
                    _helheimSummonedThisTurn = true;
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
        // NORMAL SUMMONS & FINISHERS
        // ═══════════════════════════════════════════════════════════════

        private bool OnNecrofaceSummon()
        {
            // Emergency Deck Reset: if deck is critically low (<= 5 cards) and we have banished cards
            if (Bot.Deck.Count <= 5 && Bot.Banished.Count >= 10) return true;

            // Or if Gren Maju is NOT in hand, and our banished pool is high, but we need to recycle
            if (!Bot.HasInHand(CardId.GrenMajuDaEiza) && Bot.Deck.Count <= 8 && Bot.Banished.Count >= 20) return true;

            return false;
        }

        private bool OnNecrofaceActivate()
        {
            // 1. If Normal Summon trigger (shuffles all banished cards back into Deck)
            if (Card.Location == CardLocation.MonsterZone)
            {
                // If Gren Maju is on the field, do NOT wipe its ATK down to 0!
                if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.GrenMajuDaEiza)))
                {
                    return false;
                }
                return true;
            }

            // 2. If Banished trigger (each player banishes top 5 cards)
            // If Bot is dangerously close to deckout (<= 5 cards), do not banish ourselves to death!
            if (Card.Location == CardLocation.Removed)
            {
                if (Enemy.Deck.Count <= 5 && Enemy.Deck.Count > 0) return true; // Can deck opponent out!
                if (Bot.Deck.Count <= 5) return false;
            }

            return true;
        }

        private bool OnRadianKaijuSummon()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (Duel.Turn == 1) return false;

            bool enemyHasEternalSoul = Enemy.GetSpells().Any(s => s != null && s.IsFaceup() && s.IsCode(48680970));

            // PRIORITY 1: Known Negators & Spell/Effect Interrupters (Hope Harbinger, Infinity, Crystal Wing, etc.)
            // Tributing these is mandatory to allow our spells and searchers to resolve!
            var dangerousEnemy = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && !m.IsDisabled() && (
                m.Id == 63767246 // Hope Harbinger (Spell Negator)
                || m.Id == 10443957 // Cyber Dragon Infinity (Omninegator)
                || m.Id == 50954680 // Crystal Wing Synchro Dragon
                || m.Id == 41721210 // Dark Magician the Dragon Knight
                || m.Id == 1561110  // ABC-Dragon Buster
                || m.Id == 59822133 // Blue-Eyes Spirit Dragon
                || CardIntelligence.IsKnownNegator(m.Id)
                || CardIntelligence.IsFloodgate(m.Id)
            ));

            // PRIORITY 2: If no negator, look for big bosses (>= 2800 ATK, but NOT Dark Magician if Eternal Soul is active)
            if (dangerousEnemy == null)
            {
                dangerousEnemy = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup() && (
                    m.Attack >= 2800 && (!m.IsCode(46986414) || !enemyHasEternalSoul)
                )).OrderByDescending(m => m.Attack).FirstOrDefault();
            }

            if (dangerousEnemy == null) return false;

            // If we have Raigeki or Lightning Storm in hand, and enemy does not negate spells:
            // Let Raigeki/Lightning Storm wipe the board first without feeding them a 2800 Kaiju!
            bool enemyNegatesSpells = Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && !m.IsDisabled() && (
                m.Id == 63767246 || m.Id == 10443957 || CardIntelligence.IsKnownNegator(m.Id)
            ));

            if (!enemyNegatesSpells)
            {
                if (Bot.HasInHand(CardId.Raigeki) && Duel.Phase == DuelPhase.Main1)
                    return false;
                if (Bot.HasInHand(CardId.LightningStorm) && Duel.Phase == DuelPhase.Main1 &&
                    !Bot.GetMonsters().Any(c => c != null && c.IsFaceup()) && !Bot.GetSpells().Any(c => c != null && c.IsFaceup()))
                    return false;
            }

            // Kaiju Safety Check: Ensure we have a way to eliminate or attack over the 2800 ATK Kaiju this turn,
            // OR the target is an active negator/floodgate that MUST be tributed to unbrick our plays!
            bool isChokepointNegator = CardIntelligence.IsKnownNegator(dangerousEnemy.Id) 
                || CardIntelligence.IsFloodgate(dangerousEnemy.Id)
                || dangerousEnemy.Id == 63767246 || dangerousEnemy.Id == 10443957 || dangerousEnemy.Id == 50954680;

            bool canRemoveKaiju = isChokepointNegator
                || Bot.HasInHand(CardId.Raigeki)
                || Bot.HasInHand(CardId.EaterOfMillions) || Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.EaterOfMillions))
                || Bot.HasInHand(CardId.TrirealmRiftDarkness) || Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.TrirealmRiftDarkness))
                || Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.GizmekOrochi))
                || ((Bot.HasInHand(CardId.GrenMajuDaEiza) || Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.GrenMajuDaEiza))) && Bot.Banished.Count >= 8);

            if (!canRemoveKaiju) return false;

            AI.SelectCard(dangerousEnemy);
            return true;
        }

        private bool OnDarknessSummon()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // Never sacrifice Gren Maju for Darkness
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.GrenMajuDaEiza))) return false;
            int lowMats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level <= 4 && !c.IsCode(CardId.GrenMajuDaEiza));
            return lowMats >= 2 && Bot.Deck.Count >= 8 && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0);
        }

        private bool OnGeneralNormalSummon()
        {
            // 1. If Gospel is face-up in SpellZone, we have an EXTRA Normal Summon for a Trirealm monster!
            // We can freely summon our Trirealm monster AND save the regular summon for Gren Maju!
            bool hasGospelExtraSummon = Bot.SpellZone.Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.TrirealmRiftGospel));
            if (hasGospelExtraSummon)
            {
                return true;
            }

            // 2. If Gren Maju is in hand:
            // STRICT RULE: NEVER steal the Normal Summon from Gren Maju!
            if (Bot.HasInHand(CardId.GrenMajuDaEiza))
            {
                return false;
            }

            // 3. If Small World is in hand and we have monster fodder, reserve Normal Summon for Gren Maju!
            if (Bot.HasInHand(CardId.SmallWorld) && Bot.Hand.Count(c => c != null && c.IsMonster() && c != Card) >= 1)
            {
                return false;
            }

            return true;
        }

        private bool OnGrenMajuSummon()
        {
            if (Card.Location != CardLocation.Hand) return false;

            // STRICT RULE: On Turn 1, NEVER summon Gren Maju!
            // Bot cannot attack on Turn 1, and summoning Maju leaves it exposed to removal without doing damage!
            if (Duel.Turn == 1) return false;

            // Don't Normal Summon Gren Maju in Main Phase 2
            if (Duel.Phase == DuelPhase.Main2) return false;

            // If we have banish engines in hand that should run first to maximize ATK, let them run first!
            if (Bot.HasInHand(CardId.PotOfExtravagance) && Bot.ExtraDeck.Count >= 6)
                return false;
            if (Bot.HasInHand(CardId.TrirealmRiftTerritoryValvols) && Bot.Deck.Count + Bot.Graveyard.Count >= 5)
                return false;
            if (Bot.HasInHand(CardId.TrirealmRiftGospel) && Bot.Deck.Count + Bot.Graveyard.Count >= 5 && !Bot.SpellZone.Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.TrirealmRiftGospel)))
                return false;
            if (Bot.HasInHand(CardId.GizmekOrochi) && Bot.Deck.Count >= 10 && !_gizmekSummonedThisTurn)
                return false;

            // CRITICAL SUICIDE GUARD:
            // Calculate Gren Maju's actual ATK
            int majuAtk = Bot.Banished.Count * 400;
            int enemyBestAtk = Util.GetBestAttack(Enemy);

            // If enemy has monsters and Gren Maju cannot beat the strongest enemy monster:
            if (Enemy.GetMonsterCount() > 0 && majuAtk <= enemyBestAtk)
            {
                // Check if we have starters/extenders in hand that can banish more cards or remove the threat first
                bool hasStarters = Bot.Hand.Any(c => c != null && c != Card && (
                    c.IsCode(CardId.TrirealmRiftOfEmptinessGehenna)
                    || c.IsCode(CardId.TrirealmRiftOfSkySheol)
                    || c.IsCode(CardId.TrirealmRiftOfBlueTuonela)
                    || c.IsCode(CardId.SkyThunderTrirealmRiftYomi)
                    || c.IsCode(CardId.TrirealmRiftTerritoryValvols)
                    || c.IsCode(CardId.TrirealmRiftGospel)
                    || c.IsCode(CardId.EaterOfMillions)
                    || c.IsCode(CardId.SmallWorld)
                    || c.IsCode(CardId.Raigeki)
                    || c.IsCode(CardId.LightningStorm)
                    || c.IsCode(CardId.ForbiddenDroplet)
                ));

                if (hasStarters) return false; // Do NOT throw Gren Maju away! Play starters or removal first!

                // Even if no starters, if Gren Maju has less than 2000 ATK, summoning it in Attack position into bigger monsters is suicide!
                if (majuAtk < 2000 && enemyBestAtk >= 2000) return false;
            }

            // Always summon Gren Maju to attack / deliver lethal OTK!
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        // EXTRA DECK SUMMONS (Turn 2+ Only, NEVER SACRIFICE GREN MAJU!)
        // ═══════════════════════════════════════════════════════════════

        private bool OnDingirsuSummon()
        {
            // Dingirsu requires 2 Level 8s (Gizmek Orochi, Trirealm Darkness)
            int lv8Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 8 && !c.IsCode(CardId.GrenMajuDaEiza));
            if (lv8Count < 2) return false;
            return Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
        }

        private bool OnHopeHarbingerSummon()
        {
            int lv8Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 8 && !c.IsCode(CardId.GrenMajuDaEiza));
            if (lv8Count < 2) return false;
            return !Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.HopeHarbinger));
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
            // Topologic Zeroboros: Banish entire field + 8,000+ ATK!
            if (Duel.Turn == 1 || Util.IsTurn1OrMain2()) return false;
            // Never sacrifice Gren Maju for Zeroboros if Gren Maju already has big ATK!
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.GrenMajuDaEiza))) return false;
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !c.IsCode(CardId.GrenMajuDaEiza));
            return mats >= 3 && (Enemy.GetMonsterCount() >= 2 || Enemy.GetSpellCount() >= 2);
        }

        private bool OnAccesscodeSummon()
        {
            // STRICT RULE: Never summon Accesscode on Turn 1 or Main 2!
            if (Duel.Turn == 1 || Util.IsTurn1OrMain2()) return false;
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.GrenMajuDaEiza))) return false;
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !c.IsCode(CardId.GrenMajuDaEiza));
            return mats >= 3;
        }

        private bool OnKnightmareUnicornSummon()
        {
            // Do not summon Unicorn in Main 2 or if Gren Maju is already present!
            if (Duel.Turn == 1 || Util.IsTurn1OrMain2()) return false;
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.GrenMajuDaEiza))) return false;
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !c.IsCode(CardId.GrenMajuDaEiza) && !c.IsCode(CardId.GizmekOrochi) && !c.IsCode(CardId.TrirealmRiftDarkness));
            return mats >= 3 && Enemy.GetMonsterCount() > 0;
        }

        private bool OnKnightmarePhoenixSummon()
        {
            // Do not summon Phoenix in Main 2 or if Gren Maju is already present!
            if (Util.IsTurn1OrMain2()) return false;
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.GrenMajuDaEiza))) return false;
            int lowMats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level <= 4 && !c.IsCode(CardId.GrenMajuDaEiza));
            return lowMats >= 2 && Enemy.GetSpellCount() > 0;
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

        private bool OnTyphonSummon()
        {
            // TY-PHON locks the player out of ANY further Normal or Special Summons!
            // Therefore, TY-PHON should strictly be summoned in Main Phase 2 to secure the board
            // or when Bot has no more summons and needs to silence 3000+ ATK bosses!
            if (Duel.Phase != DuelPhase.Main2) return false;
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.GrenMajuDaEiza))) return false;
            return Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && (m.Attack >= 2500 || m.IsExtraCard()));
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
            // Do not sacrifice Gren Maju or high-ATK bosses for S:P!
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.GrenMajuDaEiza))) return false;
            int lowMats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level <= 4 && !c.IsCode(CardId.GrenMajuDaEiza));
            return lowMats >= 2 && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0);
        }

        private bool OnSPLittleKnightActivate()
        {
            return Card.Location == CardLocation.MonsterZone;
        }

        private bool OnIPMasquerenaSummon()
        {
            if (Duel.Turn != 1) return false;
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !c.IsCode(CardId.GrenMajuDaEiza));
            return mats >= 2;
        }

        private bool OnIPMasquerenaActivate()
        {
            return Duel.Player != 0 && Card.Location == CardLocation.MonsterZone;
        }

        private bool OnBagooskaSummon()
        {
            if (Duel.Turn != 1) return false;
            int lv4Mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 4 && !c.IsCode(CardId.GrenMajuDaEiza));
            return lv4Mats >= 2;
        }

        private bool OnAbyssDwellerSummon()
        {
            if (Duel.Turn != 1) return false;
            int lv4Mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 4 && !c.IsCode(CardId.GrenMajuDaEiza));
            return lv4Mats >= 2;
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            if (card.IsCode(CardId.GrenMajuDaEiza)) return true;
            if (card.IsCode(CardId.GizmekOrochi)) return true;
            if (card.IsCode(CardId.TrirealmRiftDarkness)) return true;
            if (card.IsCode(CardId.Dingirsu)) return true;
            if (card.IsCode(CardId.UnderworldGoddess)) return true;
            if (card.IsCode(CardId.TopologicZeroboros)) return true;
            if (card.IsCode(CardId.SuperStarslayerTYPHON)) return true;
            if (card.IsCode(CardId.DivineArsenalAAZEUS)) return true;
            return base.IsAceCard(card);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (c.IsCode(CardId.GrenMajuDaEiza)) return 99999;
            if (c.IsCode(CardId.UnderworldGoddess)) return 9000;
            if (c.IsCode(CardId.Dingirsu)) return 9000;
            if (c.IsCode(CardId.TopologicZeroboros)) return 9000;
            if (c.IsCode(CardId.TrirealmRiftDarkness)) return 5000;
            if (c.IsCode(CardId.GizmekOrochi)) return 5000;
            if (c.IsCode(CardId.SkyThunderTrirealmRiftYomi)) return 3000;
            if (c.IsCode(CardId.BurialSummitTrirealmRiftHelheim)) return 3000;
            return base.GetMaterialPriority(c);
        }

        protected override ActionPriority ClassifyAction(ClientCard card, ExecutorType actionType)
        {
            if (card != null && (card.IsCode(CardId.TrirealmRiftTerritoryValvols) || card.IsCode(CardId.TrirealmRiftGospel)))
            {
                return ActionPriority.ComboStarter;
            }
            return base.ClassifyAction(card, actionType);
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
        // CALLBACK OVERRIDES
        // ═══════════════════════════════════════════════════════════════

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (positions == null || positions.Count == 0) return CardPosition.FaceUpAttack;
            if (positions.Count == 1) return positions[0];

            // 0. Gren Maju Da Eiza: ALWAYS FaceUpAttack on Turn 2+ to execute OTK!
            if (cardId == CardId.GrenMajuDaEiza)
            {
                if (Duel.Turn > 1 && positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
                if (positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence;
            }

            // 1. High DEF walls & Handtraps -> Defense
            if (cardId == CardId.SkyThunderTrirealmRiftYomi // 1000 ATK / 2500 DEF
                || cardId == CardId.TrirealmRiftOfBlueTuonela // 300 ATK / 2000 DEF
                || cardId == CardId.TrirealmRiftOfEmptinessGehenna // 100 ATK / 1000 DEF
                || cardId == CardId.Number41Bagooska)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
            }

            // 2. High ATK Bosses -> Attack
            if (cardId == CardId.TrirealmRiftDarkness // 3000 ATK
                || cardId == CardId.TopologicZeroboros // 8000+ ATK
                || cardId == CardId.BurialSummitTrirealmRiftHelheim // 2400 ATK
                || cardId == CardId.GizmekOrochi // 2450 ATK
                || cardId == CardId.MadTempestTrirealmRiftPloutonion // 2100 ATK
                || cardId == CardId.TrirealmRiftOfScarletNaraka // 1700 ATK
                || cardId == CardId.AccesscodeTalker)
            {
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override ClientCard OnSelectAttacker(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            if (attackers == null || attackers.Count == 0) return null;

            // 1. If enemy has monsters, attack with Eater of Millions first to banish enemy monsters face-down!
            if (defenders != null && defenders.Count > 0)
            {
                var eater = attackers.FirstOrDefault(c => c != null && c.IsCode(CardId.EaterOfMillions) && !c.IsDisabled());
                if (eater != null) return eater;
            }

            // 2. Bait Linkuriboh (41999284) with a non-Maju monster!
            // Linkuriboh tributes itself to drop the attacker's ATK to 0.
            // If Gren Maju attacks into Linkuriboh, its OTK is ruined. Attack with non-Maju first!
            bool enemyHasLinkuriboh = defenders != null && defenders.Any(d => d != null && d.IsCode(CardId.Linkuriboh) && !d.IsDisabled());
            if (enemyHasLinkuriboh && attackers.Count > 1)
            {
                var nonMaju = attackers.FirstOrDefault(c => c != null && !c.IsCode(CardId.GrenMajuDaEiza) && !c.IsCode(CardId.EaterOfMillions));
                if (nonMaju != null) return nonMaju;
            }

            // 3. If opponent has face-down backrow and we have non-Maju attackers, bait backrow first!
            bool oppHasBackrow = Enemy.GetSpells().Any(s => s != null && s.IsFacedown());
            if (oppHasBackrow && attackers.Count > 1)
            {
                var baitAttacker = attackers.FirstOrDefault(c => c != null && !c.IsCode(CardId.GrenMajuDaEiza) && c.Attack >= 1500);
                if (baitAttacker != null) return baitAttacker;
            }

            // 4. If enemy field is empty or all enemies cleared, strike with highest ATK first for OTK
            if (defenders == null || defenders.Count == 0)
            {
                return attackers.Where(c => c != null)
                                .OrderByDescending(c => c.IsCode(CardId.GrenMajuDaEiza) ? Bot.Banished.Count * 400 : c.Attack)
                                .FirstOrDefault();
            }

            return base.OnSelectAttacker(attackers, defenders);
        }

        public override BattlePhaseAction OnSelectAttackTarget(ClientCard attacker, IList<ClientCard> defenders)
        {
            if (attacker == null) return null;

            // 1. If attacker is Eater of Millions, it can attack any monster to banish it face-down (except Crystal Wing which negates)
            if (attacker.IsCode(CardId.EaterOfMillions) && !attacker.IsDisabled() && defenders != null && defenders.Count > 0)
            {
                var validDefenders = defenders.Where(d => d != null && !(d.Id == 50954680 && !d.IsDisabled())).ToList();
                if (validDefenders.Count > 0)
                {
                    var bestTarget = validDefenders.OrderByDescending(d => d.Attack).FirstOrDefault();
                    if (bestTarget != null) return AI.Attack(attacker, bestTarget);
                }
            }

            // 2. Dynamic RealPower for Gren Maju
            if (attacker.IsCode(CardId.GrenMajuDaEiza))
            {
                attacker.RealPower = Bot.Banished.Count * 400;
            }
            else
            {
                attacker.RealPower = attacker.Attack;
            }

            // 3. Attack safe targets
            if (defenders != null && defenders.Count > 0)
            {
                foreach (ClientCard defender in defenders)
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

            // 4. Direct attack if allowed
            if (attacker.CanDirectAttack && (defenders == null || defenders.Count == 0))
            {
                return AI.Attack(attacker, null);
            }

            return null;
        }

        public override bool OnPreBattleBetween(ClientCard attacker, ClientCard defender)
        {
            if (attacker == null || defender == null) return false;

            // 1. Eater of Millions banishes battling monster at start of Damage Step
            if (attacker.IsCode(CardId.EaterOfMillions) && !attacker.IsDisabled())
            {
                // Never attack into Crystal Wing Synchro Dragon (50954680) because it negates and destroys!
                if (defender.Id == 50954680 && !defender.IsDisabled()) return false;
                attacker.RealPower = 9999;
                return true;
            }

            // 2. Correct RealPower for Gren Maju
            if (attacker.IsCode(CardId.GrenMajuDaEiza))
            {
                attacker.RealPower = Math.Max(attacker.Attack, Bot.Banished.Count * 400);
            }
            else
            {
                attacker.RealPower = attacker.Attack;
            }

            // 3. Absolute Suicide Prevention:
            // Account for Apprentice Illusion Magician (30603688) combat trick (+2000 ATK to DARK Spellcaster)
            bool enemyHasApprentice = Enemy.HasInMonstersZone(30603688) || Enemy.HasInHand(30603688);
            if (enemyHasApprentice && defender.IsCode(46986414)) // Dark Magician
            {
                if (defender.Attack + 2000 >= attacker.RealPower)
                {
                    return false; // Do not crash into Dark Magician while Apprentice is available!
                }
            }

            // If defender is in attack position, and defender ATK >= attacker RealPower: NEVER attack!
            if (defender.IsAttack() && defender.Attack >= attacker.RealPower)
            {
                return false;
            }

            // If defender is in defense position, and defender DEF > attacker RealPower: do NOT crash
            if (defender.IsDefense() && defender.Defense > attacker.RealPower)
            {
                return false;
            }

            // Never take battle damage that would reduce Bot's LP to 0
            if (defender.IsAttack() && (defender.Attack - attacker.RealPower) >= Bot.LifePoints)
            {
                return false;
            }

            return base.OnPreBattleBetween(attacker, defender);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return new List<ClientCard>();

            // 0.0 Radian Kaiju Tribute Selection (Tribute opponent's biggest threat)
            if ((Card != null && Card.IsCode(CardId.RadianKaiju)) || (LastChainCard != null && LastChainCard.IsCode(CardId.RadianKaiju)))
            {
                if (cards.Any(c => c.Controller == 1))
                {
                    var target = cards.FirstOrDefault(c => c.Id == 63767246) // Hope Harbinger
                              ?? cards.FirstOrDefault(c => c.Id == 10443957) // Cyber Dragon Infinity
                              ?? cards.FirstOrDefault(c => c.Id == 50954680) // Crystal Wing
                              ?? cards.FirstOrDefault(c => c.Id == 41721210) // Dragon Knight
                              ?? cards.FirstOrDefault(c => c.Id == 1561110)  // ABC-Dragon Buster
                              ?? cards.FirstOrDefault(c => c.Id == 59822133) // Blue-Eyes Spirit Dragon
                              ?? cards.FirstOrDefault(c => CardIntelligence.IsKnownNegator(c.Id) || CardIntelligence.IsFloodgate(c.Id))
                              ?? cards.OrderByDescending(c => c.Attack).FirstOrDefault();
                    if (target != null) return new List<ClientCard> { target };
                }
            }

            // 0. Nyannyan Trigger (When banished: shuffle 1 banished card to deck)
            if (Card != null && Card.IsCode(CardId.Nyannyan))
            {
                // Absolute Priority: Recycle banished Gren Maju back to deck!
                var grenMajuTarget = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.GrenMajuDaEiza));
                if (grenMajuTarget != null)
                {
                    return new List<ClientCard> { grenMajuTarget };
                }

                // Or recycle key Field / Spells / Darkness
                var keySpell = cards.FirstOrDefault(c => c != null && (c.IsCode(CardId.TrirealmRiftTerritoryValvols) || c.IsCode(CardId.TrirealmRiftGospel) || c.IsCode(CardId.TrirealmRiftDarkness)));
                if (keySpell != null)
                {
                    return new List<ClientCard> { keySpell };
                }
            }

            // 0.1 Eater of Millions Banish Selection (5 cards strictly from Extra Deck)
            if (Card != null && Card.IsCode(CardId.EaterOfMillions))
            {
                var extraDeckCandidates = cards.Where(c => c != null && c.Location == CardLocation.Extra).ToList();
                if (extraDeckCandidates.Count >= min)
                {
                    int[] sacrificePriority = new int[]
                    {
                        CardId.DharcTheDarkCharmer,
                        CardId.RelinquishedAnima,
                        CardId.Linkuriboh,
                        CardId.Number41Bagooska,
                        CardId.AbyssDweller,
                        CardId.KnightmarePhoenix,
                        CardId.KnightmareUnicorn,
                        CardId.IPMasquerena,
                        CardId.SPLittleKnight,
                        CardId.SuperStarslayerTYPHON,
                        CardId.DivineArsenalAAZEUS,
                        CardId.AccesscodeTalker,
                        CardId.Dingirsu,
                        CardId.UnderworldGoddess,
                        CardId.TopologicZeroboros
                    };

                    var selected = new List<ClientCard>();
                    foreach (int id in sacrificePriority)
                    {
                        var match = extraDeckCandidates.FirstOrDefault(c => c != null && c.IsCode(id) && !selected.Contains(c));
                        if (match != null)
                        {
                            selected.Add(match);
                            if (selected.Count == min) return selected;
                        }
                    }
                    foreach (var c in extraDeckCandidates)
                    {
                        if (!selected.Contains(c))
                        {
                            selected.Add(c);
                            if (selected.Count == min) return selected;
                        }
                    }
                    if (selected.Count >= min) return selected.Take(min).ToList();
                }
            }

            // 0.2 Forbidden Droplet Selection
            if (Card != null && Card.IsCode(CardId.ForbiddenDroplet))
            {
                // Cost selection
                if (cards.All(c => c.Location == CardLocation.Hand || c.Location == CardLocation.MonsterZone || c.Location == CardLocation.SpellZone))
                {
                    int enemyThreatCount = Enemy.GetMonsters().Count(c => c != null && c.IsFaceup() && !c.IsDisabled() && c.HasType(CardType.Effect));
                    int targetCostCount = Math.Clamp(enemyThreatCount, min, max);

                    var costs = new List<ClientCard>();
                    int[] preferredCostIds = new int[]
                    {
                        CardId.GizmekOrochi, // Loves GY!
                        CardId.Nyannyan,     // Triggers when banished / fine in GY
                        CardId.TrirealmRiftGospel,
                        CardId.TrirealmRiftTerritoryValvols,
                        CardId.SmallWorld,
                        CardId.HarpieFeatherDuster,
                        CardId.Raigeki,
                        CardId.Terraforming,
                        CardId.TrirealmRiftOfScarletNaraka,
                        CardId.TrirealmRiftOfBlueTuonela,
                        CardId.TrirealmRiftOfSkySheol,
                        CardId.TrirealmRiftOfEmptinessGehenna
                    };

                    foreach (int id in preferredCostIds)
                    {
                        var matches = cards.Where(c => c != null && c.IsCode(id) && !c.IsCode(CardId.GrenMajuDaEiza) && !costs.Contains(c)).ToList();
                        foreach (var m in matches)
                        {
                            costs.Add(m);
                            if (costs.Count == targetCostCount) return costs;
                        }
                    }
                    foreach (var c in cards)
                    {
                        if (c != null && !c.IsCode(CardId.GrenMajuDaEiza) && !costs.Contains(c))
                        {
                            costs.Add(c);
                            if (costs.Count == targetCostCount) return costs;
                        }
                    }
                    if (costs.Count >= min) return costs;
                    return cards.Take(min).ToList();
                }

                // Enemy Monster Negate targeting
                if (cards.All(c => c.Controller == 1))
                {
                    var orderedEnemies = cards.OrderByDescending(c => c.Id == 10443957 || c.Id == 50954680 || c.Id == 84815190 || c.Id == 4280258 || c.Id == 27548199)
                                              .ThenByDescending(c => CardIntelligence.IsKnownNegator(c.Id) || CardIntelligence.IsFloodgate(c.Id))
                                              .ThenByDescending(c => c.Attack)
                                              .ToList();
                    return orderedEnemies.Take(Math.Min(max, orderedEnemies.Count)).ToList();
                }
            }

            // 0.3 Dingirsu Send to GY Selection (Non-targeting removal)
            if (Card != null && Card.IsCode(CardId.Dingirsu) && cards.Any(c => c.Controller == 1))
            {
                var bestTarget = Plugin.ThreatImpl.PickBestRemovalTarget(cards);
                if (bestTarget != null) return new List<ClientCard> { bestTarget };
            }

            // 0.4 Underworld Goddess Material Selection (Steal enemy monster)
            if (Card != null && Card.IsCode(CardId.UnderworldGoddess) && cards.Any(c => c.Controller == 1))
            {
                var enemyMonster = cards.FirstOrDefault(c => c != null && c.Controller == 1);
                if (enemyMonster != null && min == 1) return new List<ClientCard> { enemyMonster };
            }

            // 0.5 Knightmare Phoenix Target Selection (Destroy opponent's Spell/Trap, prioritize Eternal Soul)
            if (((Card != null && Card.IsCode(CardId.KnightmarePhoenix)) || (LastChainCard != null && LastChainCard.IsCode(CardId.KnightmarePhoenix))) && cards.Any(c => c.Controller == 1))
            {
                var eternalSoul = cards.FirstOrDefault(c => c != null && c.IsCode(48680970));
                if (eternalSoul != null) return new List<ClientCard> { eternalSoul };

                var bestSpell = cards.OrderByDescending(c => CardIntelligence.IsFloodgate(c.Id))
                                     .ThenByDescending(c => c.IsFaceup())
                                     .FirstOrDefault();
                if (bestSpell != null) return new List<ClientCard> { bestSpell };
            }

            // 0.6 Knightmare Unicorn Target Selection (Shuffle opponent's card, prioritize Eternal Soul/Threats)
            if (((Card != null && Card.IsCode(CardId.KnightmareUnicorn)) || (LastChainCard != null && LastChainCard.IsCode(CardId.KnightmareUnicorn))) && cards.Any(c => c.Controller == 1))
            {
                var eternalSoul = cards.FirstOrDefault(c => c != null && c.IsCode(48680970));
                if (eternalSoul != null) return new List<ClientCard> { eternalSoul };

                var bestTarget = Plugin.ThreatImpl.PickBestRemovalTarget(cards);
                if (bestTarget != null) return new List<ClientCard> { bestTarget };
            }

            // 0.15 Called by the Grave Target Selection (Target enemy GY monster)
            if (Card != null && Card.IsCode(CardId.CalledByTheGrave))
            {
                var keyTarget = cards.FirstOrDefault(c => c != null && (
                    CardIntelligence.IsHandtrap(c.Id)
                    || c.Id == 71039903 // White Stone of Ancients
                    || c.Id == 79814787 // White Stone of Legend
                    || c.Id == 89631139 // Blue-Eyes White Dragon
                    || c.Id == 46986414 // Dark Magician
                    || c.Id == 7084129  // Magicians' Souls
                    || CardIntelligence.IsKnownNegator(c.Id)
                )) ?? cards.FirstOrDefault();
                if (keyTarget != null) return new List<ClientCard> { keyTarget };
            }

            // 0.16 Nyannyan Shuffle Banished Target (Recover Gren Maju / Orochi / Territory)
            if (Card != null && Card.IsCode(CardId.Nyannyan))
            {
                var target = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.GrenMajuDaEiza))
                    ?? cards.FirstOrDefault(c => c != null && c.IsCode(CardId.GizmekOrochi))
                    ?? cards.FirstOrDefault(c => c != null && c.IsCode(CardId.TrirealmRiftTerritoryValvols))
                    ?? cards.FirstOrDefault(c => c != null && !c.IsCode(CardId.Nyannyan));
                if (target != null) return new List<ClientCard> { target };
            }

            // 1. Small World Resolution
            if ((Card != null && Card.IsCode(CardId.SmallWorld)) || (LastChainCard != null && LastChainCard.IsCode(CardId.SmallWorld)))
            {
                // Step A: From Hand (reveal starter/fodder)
                if (cards.All(c => c.Location == CardLocation.Hand))
                {
                    // Strictly reveal a monster that has a 100% bridge to Gren Maju (Trirealm, Gizmek, Necroface, Nyannyan)
                    // Do NOT reveal Eater of Millions or Radian Kaiju (they cannot bridge to Gren Maju!)
                    var preferredReveal = cards.FirstOrDefault(c => c != null && !c.IsCode(CardId.GrenMajuDaEiza)
                                                                             && !c.IsCode(CardId.EaterOfMillions)
                                                                             && !c.IsCode(CardId.RadianKaiju))
                                       ?? cards.FirstOrDefault(c => c != null && !c.IsCode(CardId.GrenMajuDaEiza))
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

                    // If Bot already has Gren Maju in hand, check what else we need in Step C:
                    if (Bot.HasInHand(CardId.GrenMajuDaEiza))
                    {
                        bool enemyHasDangerousBoss = Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && (
                            m.Id == 50954680 || m.Id == 41721210 ||
                            CardIntelligence.IsKnownNegator(m.Id) || CardIntelligence.IsFloodgate(m.Id) ||
                            m.Attack >= 2800
                        ));
                        if (enemyHasDangerousBoss && !Bot.HasInHand(CardId.RadianKaiju))
                        {
                            var kaijuTarget = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.RadianKaiju));
                            if (kaijuTarget != null) return new List<ClientCard> { kaijuTarget };
                        }

                        if (!Bot.HasInHand(CardId.GizmekOrochi) && !Bot.HasInGraveyard(CardId.GizmekOrochi) && Bot.Deck.Count >= 10)
                        {
                            var orochiTarget = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.GizmekOrochi));
                            if (orochiTarget != null) return new List<ClientCard> { orochiTarget };
                        }
                    }

                    // Otherwise, this is STEP B (BRIDGE SELECTION):
                    // Priority 1: Naraka (FIRE / Psychic / Lv 4) bridges ALL Trirealm monsters, Necroface, and Nyannyan to Gren Maju!
                    var narakaBridge = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.TrirealmRiftOfScarletNaraka));
                    if (narakaBridge != null) return new List<ClientCard> { narakaBridge };

                    // Priority 2: Tuonela (WATER / Psychic / Lv 3) bridges Naraka and Sheol to Gren Maju (shares Lv 3 with Maju)!
                    var tuonelaBridge = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.TrirealmRiftOfBlueTuonela));
                    if (tuonelaBridge != null) return new List<ClientCard> { tuonelaBridge };

                    // Priority 3: Radian Kaiju (DARK / Fiend / Lv 7) bridges Gizmek Orochi and Darkness to Gren Maju (shares Fiend with Maju)!
                    var radianBridge = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.RadianKaiju));
                    if (radianBridge != null) return new List<ClientCard> { radianBridge };

                    // Priority 4: Nyannyan (EARTH / Psychic / Lv 3) bridges Trirealm monsters to Gren Maju (shares Lv 3 with Maju)!
                    var nyannyanBridge = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.Nyannyan));
                    if (nyannyanBridge != null) return new List<ClientCard> { nyannyanBridge };

                    // Fallback to Gren Maju if present, or first candidate
                    if (targetGrenMaju != null) return new List<ClientCard> { targetGrenMaju };
                    return new List<ClientCard> { cards.First() };
                }
            }

            // 2. Monster Reborn Selection
            if (Card != null && Card.IsCode(CardId.MonsterReborn))
            {
                var grenMaju = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.GrenMajuDaEiza));
                if (grenMaju != null) return new List<ClientCard> { grenMaju };

                var darkness = cards.FirstOrDefault(c => c != null && c.IsCode(CardId.TrirealmRiftDarkness));
                if (darkness != null) return new List<ClientCard> { darkness };

                var boss = cards.FirstOrDefault(c => c != null && (c.IsCode(CardId.SkyThunderTrirealmRiftYomi) || c.IsCode(CardId.BurialSummitTrirealmRiftHelheim) || c.IsCode(CardId.GizmekOrochi)));
                if (boss != null) return new List<ClientCard> { boss };
            }

            // 3. Darkness Quick Destroy Targeting
            if (Card != null && Card.IsCode(CardId.TrirealmRiftDarkness))
            {
                var bestTarget = Plugin.ThreatImpl.PickBestRemovalTarget(cards);
                if (bestTarget != null) return new List<ClientCard> { bestTarget };
            }

            // 4. Hint 506: ATOHAND (Search target from banished)
            if (hint == 506)
            {
                var targets = Plugin.StrategyImpl.PickSearchTargets(cards, Card, max);
                if (targets != null && targets.Count >= min)
                {
                    return targets;
                }
            }

            // 5. Hint 509: SPSUMMON (Special Summon target from banished)
            if (hint == 509)
            {
                var target = Plugin.StrategyImpl.PickSpecialSummonTarget(cards);
                if (target != null)
                {
                    return new List<ClientCard> { target };
                }
            }

            // 6. Hint 500 / 501 / 504: RELEASE / DISCARD / TOGRAVE (NEVER SACRIFICE OR DISCARD GREN MAJU!)
            if (hint == 500 || hint == 501 || hint == 504)
            {
                var nonGrenCards = cards.Where(c => c != null && !c.IsCode(CardId.GrenMajuDaEiza)).ToList();
                if (nonGrenCards.Count >= min)
                {
                    var discardTargets = Plugin.MaterialImpl.PickDiscardTargets(nonGrenCards, min);
                    if (discardTargets != null && discardTargets.Count >= min)
                    {
                        return discardTargets;
                    }
                    return nonGrenCards.Take(min).ToList();
                }
            }

            // 7. Hint 533: Material selection for Link / Xyz / Synchro (NEVER SACRIFICE GREN MAJU)
            if (hint == 533)
            {
                var safeMaterials = cards.Where(c => c != null && !c.IsCode(CardId.GrenMajuDaEiza)).ToList();
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

            if ((Card != null && Card.IsCode(CardId.LightningStorm)) || (LastChainCard != null && LastChainCard.IsCode(CardId.LightningStorm)))
            {
                // Option 0: Destroy all Attack monsters, Option 1: Destroy all Spells/Traps
                if (Enemy.GetSpellCount() > 0 && options.Count > 1) return 1;
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
