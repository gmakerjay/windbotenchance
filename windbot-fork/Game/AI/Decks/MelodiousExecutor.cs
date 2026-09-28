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
    [Deck("Melodious", "Melodious")]
    public class MelodiousExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Deck Monsters
            public const int RefrainTheMelodiousSongstress = 64881644;
            public const int CoupletTheMelodiousSongstress = 90276649;
            public const int SopranoTheMelodiousSongstress = 62895219;
            public const int SonataTheMelodiousDiva = 76990617;
            public const int AriaTheMelodiousDiva = 40502912;
            public const int ElegyTheMelodiousDiva = 79514956;
            public const int ShopinaTheMelodiousMaestra = 5908650;
            public const int ScoreTheMelodiousDiva = 41767843;
            public const int TamtamTheMelodiousDiva = 79757784;
            public const int AshBlossom = 14558127;
            public const int AshBlossomAlt = 14558128;
            public const int EffectVeiler = 97268402;
            public const int EffectVeilerAlt1 = 97268403;
            public const int EffectVeilerAlt2 = 97268404;

            // Spells
            public const int Ostinato = 9113513;
            public const int FirstMovementSolo = 44256816;
            public const int MelodiousConcerto = 31458630;
            public const int Polymerization = 24094653;
            public const int PotOfProsperity = 84211599;
            public const int CalledByTheGrave = 24224830;
            public const int CalledByTheGraveAlt = 24224831;
            public const int CrossoutDesignator = 65681983;
            public const int ForbiddenDroplet = 24299458;
            public const int TripleTacticsTalent = 25311006;
            public const int TripleTacticsThrust = 35269904;
            public const int MonsterReborn = 83764718;
            public const int VingolfsBlessing = 75493362;

            // Traps
            public const int InfiniteImpermanence = 10045474;
            public const int SolemnStrike = 40605147;
            public const int SolemnJudgment = 41420027;

            // Extra Deck
            public const int FloweringEtoileTheMelodiousMagnificat = 83793721;
            public const int BachaTheMelodiousMaestra = 56208713;
            public const int SchubertaTheMelodiousMaestra = 57594700;
            public const int BloomDivaTheMelodiousChoir = 84988419;
            public const int BloomHarmonistTheMelodiousComposer = 34974462;
            public const int HeraldOfMirageLights = 46935289;
            public const int SPLittleKnight = 29301450;
            public const int IPMasquerena = 65741786;
            public const int Apollousa = 4280258;
            public const int SuperStarslayerTYPHON = 93039339;
            public const int AbyssDweller = 21044178;
        }

        private static readonly int[] BossMonsters = {
            CardId.FloweringEtoileTheMelodiousMagnificat,
            CardId.BachaTheMelodiousMaestra,
            CardId.BloomDivaTheMelodiousChoir,
            CardId.SchubertaTheMelodiousMaestra,
            CardId.BloomHarmonistTheMelodiousComposer,
            CardId.Apollousa,
            CardId.SPLittleKnight,
            CardId.HeraldOfMirageLights,
            CardId.SuperStarslayerTYPHON,
            CardId.AbyssDweller
        };

        internal MelodiousPlugin Plugin { get; private set; }

        private bool _ostinatoUsedThisTurn = false;
        private bool _soloUsedThisTurn = false;
        private bool _refrainSearchedThisTurn = false;
        private bool _coupletSearchedThisTurn = false;
        private bool _bloomHarmonistUsedThisTurn = false;
        private bool _floweringEtoileUsedThisTurn = false;
        private bool _schubertaUsedThisTurn = false;

        public MelodiousExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            Plugin = new MelodiousPlugin(this);
            DeckPlugin = Plugin;

            RegisterHelperModules();
            RegisterExecutors();
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _ostinatoUsedThisTurn = false;
            _soloUsedThisTurn = false;
            _refrainSearchedThisTurn = false;
            _coupletSearchedThisTurn = false;
            _bloomHarmonistUsedThisTurn = false;
            _floweringEtoileUsedThisTurn = false;
            _schubertaUsedThisTurn = false;
            Plugin?.ResetTurnState();
        }

        public override bool OnSelectHand()
        {
            // Melodious builds formidable board going 1st
            return true;
        }

        private void RegisterHelperModules()
        {
            // 1. Ace Card Protection
            ResourcePlan.RegisterAceCards(BossMonsters);
            HeuristicGuard.RegisterAceCards(BossMonsters);

            // 2. Combo Starters & Bait
            BaitPlanner.RegisterComboStarters(
                CardId.Ostinato,
                CardId.FirstMovementSolo,
                CardId.RefrainTheMelodiousSongstress,
                CardId.PotOfProsperity
            );
            BaitPlanner.RegisterBaitCards(
                CardId.CrossoutDesignator,
                CardId.CalledByTheGrave
            );

            // 3. Negators & High Value Targets
            ChainAdvisor.RegisterHighValueTargets(
                CardId.FloweringEtoileTheMelodiousMagnificat,
                CardId.SolemnJudgment,
                CardId.SolemnStrike,
                CardId.InfiniteImpermanence,
                CardId.AshBlossom,
                CardId.CalledByTheGrave,
                CardId.CrossoutDesignator,
                CardId.HeraldOfMirageLights,
                CardId.Apollousa
            );
        }

        private void RegisterExecutors()
        {
            // ===============================================================
            // TIER 0: COUNTER & QUICK NEGATORS / HANDTRAPS
            // ===============================================================

            // Called by the Grave: Negate opponent Handtraps or GY Bosses
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGraveAlt, DefaultCalledByTheGrave);

            // Crossout Designator: Declare Ash / Imperm / Veiler
            AddExecutor(ExecutorType.Activate, CardId.CrossoutDesignator, DefaultCrossoutDesignator);

            // Solemn Judgment: Counterboard sweepers or key summons
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, SolemnJudgmentActivate);

            // Solemn Strike: Negate monster effect activations or Special Summons
            AddExecutor(ExecutorType.Activate, CardId.SolemnStrike, SolemnStrikeActivate);

            // Infinite Impermanence: Handtrap or set trap negate
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, DefaultInfiniteImpermanence);

            // Ash Blossom: Universal search/draw denial
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAlt, DefaultAshBlossomAndJoyousSpring);

            // Effect Veiler: Target face-up monster negate during Main Phase
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, DefaultEffectVeiler);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeilerAlt1, DefaultEffectVeiler);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeilerAlt2, DefaultEffectVeiler);

            // Forbidden Droplet: Discard spells/monsters to negate opponent monsters
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletActivate);

            // Herald of Mirage Lights: Send Fairy from hand to GY to negate Spell/Trap
            AddExecutor(ExecutorType.Activate, CardId.HeraldOfMirageLights, HeraldOfMirageLightsActivate);

            // Score the Melodious Diva: Send from hand to drop battling opponent monster ATK to 0
            AddExecutor(ExecutorType.Activate, CardId.ScoreTheMelodiousDiva, ScoreActivate);

            // ===============================================================
            // TIER 0.5: BOSS DISRUPTION & QUICK BOUNCES
            // ===============================================================

            // Flowering Etoile the Melodious Magnificat: Quick Effect Banish & Bounce!
            AddExecutor(ExecutorType.Activate, CardId.FloweringEtoileTheMelodiousMagnificat, FloweringEtoileActivate);

            // Schuberta the Melodious Maestra: Quick Effect Banish up to 3 cards in GYs!
            AddExecutor(ExecutorType.Activate, CardId.SchubertaTheMelodiousMaestra, SchubertaActivate);

            // Bloom Diva the Melodious Choir: Combat pop & burn
            AddExecutor(ExecutorType.Activate, CardId.BloomDivaTheMelodiousChoir, BloomDivaActivate);

            // ===============================================================
            // TIER 1: COMBO STARTERS & FUSION SPELLS
            // ===============================================================

            // Pot of Prosperity: Excavate to dig for Ostinato or Refrain
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, PotOfProsperityActivate);

            // Triple Tactics Talent: Draw 2 or Steal monster if opponent activated monster effect
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentActivate);

            // Triple Tactics Thrust: Search Normal Spell/Trap if opponent activated monster effect
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsThrust, TripleTacticsThrustActivate);

            // Ostinato: God-tier 1-card Fusion from Deck into Bacha!
            AddExecutor(ExecutorType.Activate, CardId.Ostinato, OstinatoActivate);

            // 1st Movement Solo: Special Summon Refrain from Deck while no monsters on field!
            AddExecutor(ExecutorType.Activate, CardId.FirstMovementSolo, FirstMovementSoloActivate);

            // Melodious Concerto: In-archetype Fusion Summon using field/hand/Pendulum Zone
            AddExecutor(ExecutorType.Activate, CardId.MelodiousConcerto, MelodiousConcertoActivate);

            // Polymerization: Standard Fusion Summon
            AddExecutor(ExecutorType.Activate, CardId.Polymerization, PolymerizationActivate);

            // Monster Reborn: Revive key boss or combo piece
            AddExecutor(ExecutorType.Activate, CardId.MonsterReborn, MonsterRebornActivate);

            // ===============================================================
            // TIER 2: MONSTER COMBO ENGINES (REFRAIN, COUPLET, BACHA, BLOOM HARMONIST)
            // ===============================================================

            // Refrain the Melodious Songstress: Normal Summon -> Search Melodious Monster
            AddExecutor(ExecutorType.Summon, CardId.RefrainTheMelodiousSongstress, RefrainSummon);
            AddExecutor(ExecutorType.Activate, CardId.RefrainTheMelodiousSongstress, RefrainActivate);

            // Couplet the Melodious Songstress: Scale activation -> Search Melodious Spell/Trap
            AddExecutor(ExecutorType.Activate, CardId.CoupletTheMelodiousSongstress, CoupletActivate);

            // Bacha the Melodious Maestra: Special Summon Melodious from Deck on Fusion; float on sent to GY
            AddExecutor(ExecutorType.Activate, CardId.BachaTheMelodiousMaestra, BachaActivate);

            // Bloom Harmonist the Melodious Composer: Link-2 Summon & Summon 2 Melodious from Deck
            AddExecutor(ExecutorType.SpSummon, CardId.BloomHarmonistTheMelodiousComposer, BloomHarmonistSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.BloomHarmonistTheMelodiousComposer, BloomHarmonistActivate);

            // Soprano the Melodious Songstress: Recover Melodious from GY; Fuse on field
            AddExecutor(ExecutorType.Summon, CardId.SopranoTheMelodiousSongstress, SopranoSummon);
            AddExecutor(ExecutorType.Activate, CardId.SopranoTheMelodiousSongstress, SopranoActivate);

            // Sonata the Melodious Diva: Special Summon from hand if Melodious on field
            AddExecutor(ExecutorType.SpSummon, CardId.SonataTheMelodiousDiva, SonataSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SonataTheMelodiousDiva, SonataActivate);

            // Tamtam the Melodious Diva: Search Polymerization on Special Summon
            AddExecutor(ExecutorType.Activate, CardId.TamtamTheMelodiousDiva, TamtamActivate);

            // Aria the Melodious Diva: Normal Summon if needed
            AddExecutor(ExecutorType.Summon, CardId.AriaTheMelodiousDiva, AriaSummon);

            // Elegy the Melodious Diva: Normal Summon if needed
            AddExecutor(ExecutorType.Summon, CardId.ElegyTheMelodiousDiva, ElegySummon);

            // ===============================================================
            // TIER 2.5: EXTRA DECK BOSSES & UTILITIES
            // ===============================================================

            // Flowering Etoile the Melodious Magnificat (Fusion Summon)
            AddExecutor(ExecutorType.SpSummon, CardId.FloweringEtoileTheMelodiousMagnificat, FloweringEtoileSpSummon);

            // Bacha the Melodious Maestra (Fusion Summon)
            AddExecutor(ExecutorType.SpSummon, CardId.BachaTheMelodiousMaestra, BachaSpSummon);

            // Schuberta the Melodious Maestra (Fusion Summon)
            AddExecutor(ExecutorType.SpSummon, CardId.SchubertaTheMelodiousMaestra, SchubertaSpSummon);

            // Bloom Diva the Melodious Choir (Fusion Summon)
            AddExecutor(ExecutorType.SpSummon, CardId.BloomDivaTheMelodiousChoir, BloomDivaSpSummon);

            // Herald of Mirage Lights (Link-2)
            AddExecutor(ExecutorType.SpSummon, CardId.HeraldOfMirageLights, HeraldOfMirageLightsSpSummon);

            // S:P Little Knight (Link-2)
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightActivate);

            // I:P Masquerena (Link-2)
            AddExecutor(ExecutorType.SpSummon, CardId.IPMasquerena, IPMasquerenaSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.IPMasquerena, IPMasquerenaActivate);

            // Apollousa, Bow of the Goddess (Link-3/4)
            AddExecutor(ExecutorType.SpSummon, CardId.Apollousa, ApollousaSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Apollousa, ApollousaActivate);

            // Super Starslayer TY-PHON - Sky Crisis (Rank 12 Xyz Emergency)
            AddExecutor(ExecutorType.SpSummon, CardId.SuperStarslayerTYPHON, TYPHONSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SuperStarslayerTYPHON, TYPHONActivate);

            // Abyss Dweller (Rank 4 Xyz)
            AddExecutor(ExecutorType.SpSummon, CardId.AbyssDweller, AbyssDwellerSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.AbyssDweller, AbyssDwellerActivate);

            // ===============================================================
            // TIER 3: TRAP SETTING & POSITION CONTROL
            // ===============================================================

            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnJudgment);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnStrike);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGraveAlt);
            AddExecutor(ExecutorType.SpellSet, CardId.CrossoutDesignator);

            AddExecutor(ExecutorType.Repos, SmartMonsterRepos);
        }

        // ===============================================================
        // EXECUTOR IMPLEMENTATIONS
        // ===============================================================

        private bool ForbiddenDropletActivate()
        {
            if (Card == null) return false;
            var oppMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsDisabled() && IsTargetable(c)).ToList();
            if (oppMonsters.Count == 0) return false;
            int sendableCount = Bot.Hand.Count(c => c != null && c != Card && !Plugin.MaterialImpl.IsHighValueMaterial(c))
                + Bot.GetMonsters().Count(c => c != null && !Plugin.MaterialImpl.IsHighValueMaterial(c));
            return sendableCount > 0;
        }

        private bool SolemnJudgmentActivate()
        {
            if (Duel.LastChainPlayer == 1)
            {
                // Pay half LP to negate Spell/Trap card or monster Summon
                return Bot.LifePoints > 1000;
            }
            return false;
        }

        private bool SolemnStrikeActivate()
        {
            if (Duel.LastChainPlayer == 1)
            {
                // Pay 1500 LP to negate monster Special Summon or monster effect
                return Bot.LifePoints > 1500;
            }
            return false;
        }

        private bool HeraldOfMirageLightsActivate()
        {
            if (Duel.LastChainPlayer == 1)
            {
                // Negate Spell/Trap by sending Fairy monster from hand to GY
                return Bot.Hand.Any(c => c != null && c.HasRace(CardRace.Fairy) && !Plugin.MaterialImpl.IsHighValueMaterial(c));
            }
            return false;
        }

        private bool ScoreActivate()
        {
            // Drop opponent battling monster ATK to 0
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep)
            {
                ClientCard battlingBot = Bot.BattlingMonster;
                ClientCard battlingEnemy = Enemy.BattlingMonster;
                if (battlingBot != null && battlingEnemy != null && IsMelodious(battlingBot))
                {
                    return battlingEnemy.Attack > battlingBot.Attack;
                }
            }
            return false;
        }

        private bool FloweringEtoileActivate()
        {
            if (Card == null) return false;

            // Effect 1: Floating on leaves field -> Special Summon Melodious from Deck/Extra
            if (Card.Location == CardLocation.Grave || Card.Location == CardLocation.Removed)
            {
                return true;
            }

            // Effect 2: Quick Effect Banish & Bounce
            if (_floweringEtoileUsedThisTurn) return false;

            var enemyFaceups = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            enemyFaceups.AddRange(Enemy.GetSpells().Where(c => c != null && c.IsFaceup()));

            if (enemyFaceups.Count == 0) return false;

            // Activate on opponent's turn or in response to opponent action
            bool shouldDisrupt = (Duel.Turn % 2 == 0) || (Duel.LastChainPlayer == 1) || (_isGoingSecond && Duel.Phase == DuelPhase.Main1);
            if (shouldDisrupt)
            {
                _floweringEtoileUsedThisTurn = true;
                return true;
            }

            return false;
        }

        private bool SchubertaActivate()
        {
            if (Card == null || _schubertaUsedThisTurn) return false;

            // Target up to 3 cards in GYs to banish and gain ATK
            var targets = Enemy.Graveyard.Where(c => c != null).ToList();
            if (targets.Count > 0)
            {
                bool shouldBanish = (Duel.Turn % 2 == 0) || (Duel.Phase == DuelPhase.BattleStep && Enemy.BattlingMonster != null) || Enemy.Graveyard.Count >= 2;
                if (shouldBanish)
                {
                    _schubertaUsedThisTurn = true;
                    return true;
                }
            }

            return false;
        }

        private bool BloomDivaActivate()
        {
            // Combat pop and burn
            return true;
        }

        private bool PotOfProsperityActivate()
        {
            // Excavate 3 or 6 to search starter
            return Bot.GetMonsterCount() == 0 || !Bot.Hand.Any(c => c != null && (c.IsCode(CardId.Ostinato) || c.IsCode(CardId.RefrainTheMelodiousSongstress)));
        }

        private bool TripleTacticsTalentActivate()
        {
            // Draw 2 cards by default
            return true;
        }

        private bool TripleTacticsThrustActivate()
        {
            // Search Normal Spell or Trap
            return true;
        }

        private bool OstinatoActivate()
        {
            if (_ostinatoUsedThisTurn) return false;
            // Activate when field has no monsters
            if (Bot.GetMonsterCount() == 0)
            {
                _ostinatoUsedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool FirstMovementSoloActivate()
        {
            if (_soloUsedThisTurn) return false;
            // Activate when field has no monsters
            if (Bot.GetMonsterCount() == 0)
            {
                _soloUsedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool MelodiousConcertoActivate()
        {
            if (Card == null) return false;

            // GY Trigger: Shuffle to bottom of deck to draw 1 card
            if (Card.Location == CardLocation.Grave)
                return true;

            // Hand/Field Activation: Fuse into Flowering Etoile, Bacha, or Schuberta
            int fairyCount = Bot.Hand.Count(c => c != null && c.HasRace(CardRace.Fairy))
                + Bot.GetMonsters().Count(c => c != null && c.HasRace(CardRace.Fairy))
                + Bot.SpellZone.Count(c => c != null && (c.IsCode(CardId.RefrainTheMelodiousSongstress) || c.IsCode(CardId.CoupletTheMelodiousSongstress)));

            return fairyCount >= 2;
        }

        private bool PolymerizationActivate()
        {
            int melodiousCount = Bot.Hand.Count(c => c != null && IsMelodious(c))
                + Bot.GetMonsters().Count(c => c != null && IsMelodious(c));
            return melodiousCount >= 2;
        }

        private bool MonsterRebornActivate()
        {
            var target = Bot.Graveyard.FirstOrDefault(c => c != null && (
                c.IsCode(CardId.FloweringEtoileTheMelodiousMagnificat) ||
                c.IsCode(CardId.BachaTheMelodiousMaestra) ||
                c.IsCode(CardId.RefrainTheMelodiousSongstress) ||
                c.IsCode(CardId.SopranoTheMelodiousSongstress)
            ));
            return target != null;
        }

        private bool RefrainSummon()
        {
            // Normal Summon Refrain to trigger search
            return true;
        }

        private bool RefrainActivate()
        {
            if (Card == null) return false;

            // Trigger 1: In Extra Deck face-up -> Place in Pendulum Zone
            if (Card.Location == CardLocation.Extra)
            {
                return true;
            }

            // Trigger 2: Pendulum Scale Effect -> Send Melodious to GY to boost Fusion ATK
            if (Card.Location == CardLocation.SpellZone)
            {
                return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Fusion) && IsMelodious(c));
            }

            // Trigger 3: Monster Effect on Normal/Special Summon -> Search Melodious monster
            if (!_refrainSearchedThisTurn)
            {
                _refrainSearchedThisTurn = true;
                return true;
            }

            return false;
        }

        private bool CoupletActivate()
        {
            if (Card == null) return false;

            // Trigger 1: In Extra Deck face-up -> Place in Pendulum Zone
            if (Card.Location == CardLocation.Extra)
            {
                return true;
            }

            // Trigger 2: Pendulum Scale Effect -> Search Melodious Spell/Trap
            if (Card.Location == CardLocation.SpellZone)
            {
                if (!_coupletSearchedThisTurn)
                {
                    _coupletSearchedThisTurn = true;
                    return true;
                }
            }

            // Trigger 3: In Hand -> Place as Scale or Special Summon
            if (Card.Location == CardLocation.Hand)
            {
                // Place into scale if we have an open spell zone
                if (Bot.SpellZone.Count(c => c != null && c.IsFaceup() && (c.IsCode(CardId.CoupletTheMelodiousSongstress) || c.IsCode(CardId.RefrainTheMelodiousSongstress))) < 2)
                {
                    return true;
                }
            }

            return true;
        }

        private bool BachaActivate()
        {
            if (Card == null) return false;

            // Trigger 1: Sent to GY -> Special Summon Melodious from GY (except Bacha)
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }

            // Trigger 2: Special Summoned -> Special Summon Melodious from Deck
            return true;
        }

        private bool BloomHarmonistSpSummon()
        {
            if (_bloomHarmonistUsedThisTurn) return false;
            // Link Summon Bloom Harmonist using 2 Fairy monsters
            int fairyMonsters = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Fairy) && !Plugin.MaterialImpl.IsHighValueMaterial(c));
            return fairyMonsters >= 2 && Bot.Hand.Count > 0;
        }

        private bool BloomHarmonistActivate()
        {
            if (_bloomHarmonistUsedThisTurn) return false;
            // Discard 1 card to Special Summon 2 Melodious with different levels from Deck
            if (Bot.Hand.Count > 0)
            {
                _bloomHarmonistUsedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool SopranoSummon()
        {
            // Normal Summon Soprano if have Melodious monsters on field to fuse
            return Bot.GetMonsters().Any(c => c != null && IsMelodious(c));
        }

        private bool SopranoActivate()
        {
            if (Card == null) return false;

            // Trigger 1: Special Summoned -> Target Melodious in GY to add to hand
            if (Bot.Graveyard.Any(c => c != null && IsMelodious(c) && !c.IsCode(CardId.SopranoTheMelodiousSongstress)))
            {
                return true;
            }

            // Trigger 2: Field Fusion Ignition Effect
            int fieldMelodious = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && IsMelodious(c));
            return fieldMelodious >= 2;
        }

        private bool SonataSpSummon()
        {
            // Special Summon from hand if Melodious on field
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsMelodious(c)) && Bot.GetMonsterCount() < 5;
        }

        private bool SonataActivate()
        {
            return true;
        }

        private bool TamtamActivate()
        {
            // Search Polymerization
            return true;
        }

        private bool AriaSummon()
        {
            // Normal Summon Aria only if no other plays
            return Bot.GetMonsterCount() == 0;
        }

        private bool ElegySummon()
        {
            // Elegy is level 5, usually Special Summoned from Deck
            return false;
        }

        // ===============================================================
        // EXTRA DECK SUMMONS
        // ===============================================================

        private bool FloweringEtoileSpSummon()
        {
            return true;
        }

        private bool BachaSpSummon()
        {
            return true;
        }

        private bool SchubertaSpSummon()
        {
            return true;
        }

        private bool BloomDivaSpSummon()
        {
            return true;
        }

        private bool HeraldOfMirageLightsSpSummon()
        {
            int fairies = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Fairy) && !Plugin.MaterialImpl.IsHighValueMaterial(c));
            return fairies >= 2 && !HasAceOnBoard();
        }

        private bool SPLittleKnightSpSummon()
        {
            // Link Summon S:P Little Knight if opponent has cards to banish
            int monsters = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !Plugin.MaterialImpl.IsHighValueMaterial(c));
            return monsters >= 2 && Enemy.GetMonsterCount() > 0;
        }

        private bool SPLittleKnightActivate()
        {
            ClientCard oppTarget = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault()
                                ?? Enemy.GetSpells().FirstOrDefault();
            if (oppTarget != null)
            {
                AI.SelectCard(oppTarget);
                return true;
            }
            return false;
        }

        private bool IPMasquerenaSpSummon()
        {
            int nonLinks = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !c.HasType(CardType.Link) && !Plugin.MaterialImpl.IsHighValueMaterial(c));
            return nonLinks >= 2 && !HasAceOnBoard();
        }

        private bool IPMasquerenaActivate()
        {
            return Duel.LastChainPlayer == 1 || Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.BattleStart;
        }

        private bool ApollousaSpSummon()
        {
            int materials = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !Plugin.MaterialImpl.IsHighValueMaterial(c));
            return materials >= 3;
        }

        private bool ApollousaActivate()
        {
            return Duel.LastChainPlayer == 1;
        }

        private bool TYPHONSpSummon()
        {
            // Emergency Xyz if opponent controls 3000+ ATK or Extra Deck monster
            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.Attack >= 3000 || c.IsExtraCard()));
        }

        private bool TYPHONActivate()
        {
            return true;
        }

        private bool AbyssDwellerSpSummon()
        {
            int lv4s = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 4 && !Plugin.MaterialImpl.IsHighValueMaterial(c));
            return lv4s >= 2 && Enemy.Graveyard.Count >= 3;
        }

        private bool AbyssDwellerActivate()
        {
            return Duel.Phase == DuelPhase.Main1 || Duel.LastChainPlayer == 1;
        }

        // ===============================================================
        // REPOSITIONING & UTILITIES
        // ===============================================================

        private bool SmartMonsterRepos()
        {
            if (Card == null) return false;

            // 0 ATK monsters or Handtraps in Attack -> switch to Defense
            if (Card.IsAttack() && (Card.Attack == 0 || (Card.Defense > Card.Attack && Card.Defense >= 1800)))
                return true;

            // High ATK bosses in Defense -> switch to Attack to deal damage
            if (Card.IsDefense() && Card.Attack > Card.Defense && Card.Attack >= 1800 && Duel.Turn > 1)
                return true;

            return DefaultMonsterRepos();
        }

        private bool IsMelodious(ClientCard card)
        {
            if (card == null) return false;
            return card.IsCode(
                CardId.RefrainTheMelodiousSongstress,
                CardId.CoupletTheMelodiousSongstress,
                CardId.SopranoTheMelodiousSongstress,
                CardId.SonataTheMelodiousDiva,
                CardId.AriaTheMelodiousDiva,
                CardId.ElegyTheMelodiousDiva,
                CardId.ShopinaTheMelodiousMaestra,
                CardId.ScoreTheMelodiousDiva,
                CardId.TamtamTheMelodiousDiva,
                CardId.FloweringEtoileTheMelodiousMagnificat,
                CardId.BachaTheMelodiousMaestra,
                CardId.SchubertaTheMelodiousMaestra,
                CardId.BloomDivaTheMelodiousChoir,
                CardId.BloomHarmonistTheMelodiousComposer
            );
        }

        private bool HasAceOnBoard()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && BossMonsters.Contains(c.Id));
        }

        private bool IsTargetable(ClientCard card)
        {
            if (card == null) return false;
            return !card.IsShouldNotBeTarget();
        }

        // ===============================================================
        // CALLBACK OVERRIDES
        // ===============================================================

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0) return 0;

            for (int i = 0; i < options.Count; i++)
            {
                long cardId = options[i] >> 4;
                if (cardId == 0 && Card != null) cardId = Card.Id;
                long optIndex = options[i] & 0xf;

                // Triple Tactics Talent: Option 0 = Draw 2 cards
                if (cardId == CardId.TripleTacticsTalent)
                {
                    if (optIndex == 0) return i;
                }
            }

            return base.OnSelectOption(options);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (positions == null || positions.Count == 0) return CardPosition.FaceUpAttack;
            if (positions.Count == 1) return positions[0];

            var cardData = YGOSharp.OCGWrapper.NamedCard.Get(cardId);
            if (cardData != null)
            {
                if (cardData.HasType(CardType.Link))
                    return CardPosition.FaceUpAttack;

                // Handtraps / 0 ATK -> Defense
                if (cardData.Attack == 0 || CardIntelligence.IsHandtrap(cardId))
                {
                    if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                    if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                }

                // Aria & Elegy lock pieces: Defense is safe
                if (cardId == CardId.AriaTheMelodiousDiva || cardId == CardId.ElegyTheMelodiousDiva)
                {
                    if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                }

                // High ATK -> Attack
                if (cardData.Attack >= 1800 && positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return new List<ClientCard>();

            // 1. Hint 506: ATOHAND (Search target)
            if (hint == 506)
            {
                var target = Plugin.Strategy.PickSearchTarget(cards, Card);
                if (target != null)
                {
                    return new List<ClientCard> { target };
                }
            }

            // 2. Hint 509: SPSUMMON (Special Summon target)
            if (hint == 509)
            {
                // Bloom Harmonist dual summon from deck
                if (Card != null && Card.IsCode(CardId.BloomHarmonistTheMelodiousComposer) && min >= 2)
                {
                    var pair = Plugin.StrategyImpl.PickCards(cards, min, Card);
                    if (pair != null && pair.Count >= min)
                        return pair;
                }

                var target = Plugin.Strategy.PickSpecialSummonTarget(cards);
                if (target != null)
                {
                    return new List<ClientCard> { target };
                }
            }

            // 3. Hint 501 / 504: DISCARD / TOGRAVE
            if (hint == 501 || hint == 504)
            {
                // Bloom Harmonist discard cost: discard least important card
                var discardTarget = cards
                    .OrderBy(c => Plugin.MaterialImpl.ScoreMaterial(c))
                    .FirstOrDefault(c => !Plugin.MaterialImpl.IsHighValueMaterial(c));

                if (discardTarget != null)
                {
                    return new List<ClientCard> { discardTarget };
                }
            }

            // 4. Flowering Etoile Banish Target: banish self or floaters, avoid Aria/Elegy lock
            if (Card != null && Card.IsCode(CardId.FloweringEtoileTheMelodiousMagnificat) && hint == 503)
            {
                var banishTargets = cards
                    .Where(c => c != null && IsMelodious(c) && !Plugin.MaterialImpl.IsHighValueMaterial(c))
                    .Take(max)
                    .ToList();

                if (banishTargets.Count >= min)
                    return banishTargets;
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
