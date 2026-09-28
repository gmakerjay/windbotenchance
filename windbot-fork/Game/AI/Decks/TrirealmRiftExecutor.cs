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

            // Staples / Handtraps
            public const int AshBlossom = 14558127;
            public const int AshBlossomAlt = 14558128;
            public const int InfiniteImpermanence = 10045474;
            public const int CalledByTheGrave = 24224830;
            public const int CalledByTheGraveAlt = 24224831;
            public const int PotOfDesires = 35261759;
            public const int Terraforming = 73628505;

            // Spells & Traps
            public const int TrirealmRiftTerritoryValvols = 100458039; // Field Spell (Banish 5 / Search 1 / Turn Lockout)
            public const int TrirealmRiftGospel = 100458040;          // Continuous Spell (Banish 5 / Double NS / Morph Lv 4-)
            public const int TrirealmRiftJudgment = 100458041;        // Counter Trap (Summon Intercept / ATK 0)

            // Extra Deck
            public const int SPLittleKnight = 29301450;
            public const int IPMasquerena = 65741786;
            public const int KnightmarePhoenix = 2857636;
            public const int KnightmareUnicorn = 38342335;
            public const int AccesscodeTalker = 86066372;
            public const int AbyssDweller = 21044178;
            public const int Number41Bagooska = 90590303;
            public const int DivineArsenalAAZEUS = 90448279;
            public const int SuperStarslayerTYPHON = 93039339;
            public const int Linkuriboh = 41999284;
            public const int RelinquishedAnima = 94259633;
            public const int DharcTheDarkCharmer = 8264361;
            public const int LynaTheLightCharmer = 9839945;
            public const int HiitaTheFireCharmer = 48815792;
            public const int WynnTheWindCharmer = 30674956;
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
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftJudgment, OnTrirealmJudgment);
            AddExecutor(ExecutorType.Activate, CardId.SkyThunderTrirealmRiftYomi, OnYomiQuickNegate);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGraveAlt, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAlt, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, DefaultInfiniteImpermanence);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 2: OPPONENT TURN DISRUPTIONS & QUICK REMOVALS
            // ═══════════════════════════════════════════════════════════════
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

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 4: BANISH ACCELERATORS & DRAW ENGINES (Fill Banished Pool!)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.PotOfDesires, OnPotOfDesires);
            AddExecutor(ExecutorType.Activate, CardId.Terraforming, OnTerraforming);
            AddExecutor(ExecutorType.Activate, CardId.GizmekOrochi, OnGizmekOrochiSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftTerritoryValvols, OnValvolsHandActivate);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftGospel, OnGospelHandActivate);
            AddExecutor(ExecutorType.Activate, CardId.MadTempestTrirealmRiftPloutonion, OnPloutonionHandDraw);
            AddExecutor(ExecutorType.Activate, CardId.SkyThunderTrirealmRiftYomi, OnYomiHandSearch);

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
            //  PHASE 6: NORMAL SUMMONS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Summon, CardId.TrirealmRiftOfScarletNaraka);
            AddExecutor(ExecutorType.Summon, CardId.TrirealmRiftOfSkySheol);
            AddExecutor(ExecutorType.Summon, CardId.TrirealmRiftOfEmptinessGehenna);
            AddExecutor(ExecutorType.Summon, CardId.TrirealmRiftOfBlueTuonela);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 7: EXTRA DECK TOOLBOX (Only when Xenolock is not active)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, OnAccesscodeSummon);
            AddExecutor(ExecutorType.Activate, CardId.AccesscodeTalker);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmareUnicorn, OnKnightmareUnicornSummon);
            AddExecutor(ExecutorType.Activate, CardId.KnightmareUnicorn);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, OnSPLittleKnightSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Number41Bagooska, OnBagooskaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AbyssDweller, OnAbyssDwellerSummon);
            AddExecutor(ExecutorType.Activate, CardId.AbyssDweller);
            AddExecutor(ExecutorType.SpSummon, CardId.IPMasquerena, OnIPMasquerenaSummon);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 8: SPELL & TRAP BACKROW PLACEMENT
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, CardId.TrirealmRiftJudgment);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGraveAlt);

            // ═══════════════════════════════════════════════════════════════
            //  PHASE 9: REPOSITIONING
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // First turn preferred to set up face-down banish pool and Yomi negate
            return true;
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

        private bool OnTrirealmJudgment()
        {
            // Counter Trap: when opponent normal or special summons a monster OR in damage step
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Damage)
            {
                return Bot.Deck.Count == 0;
            }

            // Must respond to opponent summoning
            if (Duel.LastSummonPlayer == 1 || Duel.LastChainPlayer == 1)
            {
                // Check if we have face-down banished Trirealm monsters
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

            // Check if we have a face-down banished Trirealm Rift monster with the same attribute
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
            // In MonsterZone: Quick Destroy 1 face-up monster on field
            if (Card.Location == CardLocation.MonsterZone)
            {
                return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget() && (c.Attack >= 2000 || CardIntelligence.IsFloodgate(c.Id)));
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        // ON-SUMMON TRIGGER EFFECTS (All Monsters)
        // ═══════════════════════════════════════════════════════════════
        private bool OnSummonBanishTrigger()
        {
            // Triggers automatically upon Normal or Special Summon
            // Always activate to mill & feed face-down banished pool!
            if (Card.Location == CardLocation.MonsterZone)
            {
                // In chain responding to summon
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        // BANISH ACCELERATORS & DRAW ENGINES
        // ═══════════════════════════════════════════════════════════════

        private bool OnPotOfDesires()
        {
            return Bot.Deck.Count >= 12;
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
                if (Bot.Deck.Count >= 12 && Bot.GetMonsterCount() < 5)
                {
                    _gizmekSummonedThisTurn = true;
                    return true;
                }
            }
            return false;
        }

        private bool OnValvolsHandActivate()
        {
            // Activate Field Spell from hand by banishing 5 cards face-down
            if (Card.Location == CardLocation.Hand)
            {
                return Bot.Deck.Count + Bot.Graveyard.Count >= 5;
            }
            return false;
        }

        private bool OnGospelHandActivate()
        {
            // Activate Continuous Spell from hand by banishing 5 cards face-down
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
        // EXTRA DECK SUMMONS (Only when not locked)
        // ═══════════════════════════════════════════════════════════════

        private bool OnAccesscodeSummon()
        {
            return Util.IsTurn1OrMain2() && Bot.GetMonsterCount() >= 3;
        }

        private bool OnKnightmareUnicornSummon()
        {
            return Enemy.GetMonsterCount() > 0 && Bot.GetMonsterCount() >= 3;
        }

        private bool OnSPLittleKnightSummon()
        {
            return Bot.GetMonsterCount() >= 2 && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0);
        }

        private bool OnSPLittleKnightActivate()
        {
            return Card.Location == CardLocation.MonsterZone;
        }

        private bool OnIPMasquerenaSummon()
        {
            return Duel.Turn == 1 && Bot.GetMonsterCount() >= 2;
        }

        private bool OnIPMasquerenaActivate()
        {
            return Duel.Player != 0 && Card.Location == CardLocation.MonsterZone;
        }

        private bool OnBagooskaSummon()
        {
            return Duel.Turn == 1 && Bot.GetMonsters().Count(c => c != null && c.Level == 4) >= 2;
        }

        private bool OnAbyssDwellerSummon()
        {
            return Duel.Turn == 1 && Bot.GetMonsters().Count(c => c != null && c.Level == 4) >= 2;
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

            // 1. High DEF walls & Handtraps -> Defense
            if (cardId == CardId.SkyThunderTrirealmRiftYomi // 1000 ATK / 2500 DEF
                || cardId == CardId.TrirealmRiftOfBlueTuonela // 300 ATK / 2000 DEF
                || cardId == CardId.TrirealmRiftOfEmptinessGehenna // 100 ATK / 1000 DEF
                || cardId == CardId.AshBlossom
                || cardId == CardId.AshBlossomAlt
                || cardId == CardId.Number41Bagooska)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
            }

            // 2. High ATK Bosses -> Attack
            if (cardId == CardId.TrirealmRiftDarkness // 3000 ATK
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

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return new List<ClientCard>();

            // 1. Hint 506: ATOHAND (Search target from banished)
            if (hint == 506)
            {
                var targets = Plugin.StrategyImpl.PickSearchTargets(cards, Card, max);
                if (targets != null && targets.Count >= min)
                {
                    return targets;
                }
            }

            // 2. Hint 509: SPSUMMON (Special Summon target from banished)
            if (hint == 509)
            {
                var target = Plugin.StrategyImpl.PickSpecialSummonTarget(cards);
                if (target != null)
                {
                    return new List<ClientCard> { target };
                }
            }

            // 3. Hint 501 / 504: DISCARD / TOGRAVE
            if (hint == 501 || hint == 504)
            {
                var discardTarget = Plugin.MaterialImpl.PickDiscardTarget(cards, min);
                if (discardTarget != null)
                {
                    return new List<ClientCard> { discardTarget };
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0) return 0;

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
