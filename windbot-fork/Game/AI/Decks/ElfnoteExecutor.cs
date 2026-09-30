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
    [Deck("Elfnote", "Elfnote")]
    [Deck("ElfnotePowerPatron", "Elfnote")]
    [Deck("2026_Elfnote", "Elfnote")]
    public class ElfnoteExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Deck Monsters - Elfnote, Power Patron, Theorealize, Ars Magna
            public const int ElfnoteLucina = 13597785;
            public const int ElfnoteRegina = 56651978;
            public const int ElfnoteTinia = 59581480;
            public const int ElfnoteFortuna = 85976588;
            public const int ElfnotePowerPatron = 12375297;
            public const int PowerPatronShadowSpiritJunordo = 10266279;
            public const int TheorealizeMedius = 90875418;
            public const int MediusThePure = 97556336;
            public const int ArsMagnaOfInfinityAndFinity = 36270527;
            public const int ArsMagnaOfPurificationAndCorruption = 62368221;
            public const int JetSynchron = 9742784;

            // Handtraps
            public const int AshBlossom = 14558128;
            public const int AshBlossomAlt = 14558127;
            public const int GhostOgre = 59438931;
            public const int MaxxC = 23434538;
            public const int NibiruThePrimalBeing = 27204313;
            public const int MulcharmyFuwalos = 42141493;

            // Spells
            public const int UnleashedPowerPatronPortalTerminus = 25661743;
            public const int ElfnotesWelcomeHome = 64491754;
            public const int TheorealizePastLull = 36709484;
            public const int ArsMagnaCitrinitas = 37279096;
            public const int TripleTacticsTalent = 25311006;

            // Traps
            public const int ElfnotesRhapsodiaOfMadness = 24092792;
            public const int InfiniteImpermanence = 10045474;
            public const int SolemnJudgment = 41420027;

            // Extra Deck
            public const int ArtmageDiactorus = 27184601;
            public const int JunoraThePowerPatronOfTuning = 5914858;
            public const int ElfnoteJunePride = 5559570;
            public const int ElfnoteSeraphimStrelitzia = 42302563;
            public const int BaronneDeFleur = 84815190;
            public const int CrystalWingSynchroDragon = 50954680;
            public const int PSYFramelordOmega = 74586817;
            public const int ChaosAngel = 22850702;
            public const int FADawnDragster = 33158448;
            public const int BlackRoseDragon = 73580472;
            public const int BlackRoseDragonAlt = 73580471;
            public const int AccelSynchron = 37675907;
            public const int RavenousCrocodragonArchethys = 87188910;
            public const int PurificationPowerPatron = 31822037;
            public const int MedicuriusThePowerPatronOfIllusions = 4063756;
            public const int SPLittleKnight = 29301451;
        }

        private static readonly int[] BossMonsters = {
            CardId.ArtmageDiactorus,
            CardId.BaronneDeFleur,
            CardId.JunoraThePowerPatronOfTuning,
            CardId.ElfnoteJunePride,
            CardId.MedicuriusThePowerPatronOfIllusions,
            CardId.CrystalWingSynchroDragon,
            CardId.ChaosAngel,
            CardId.PSYFramelordOmega,
            CardId.FADawnDragster,
            CardId.RavenousCrocodragonArchethys,
            CardId.ElfnoteSeraphimStrelitzia,
            CardId.SPLittleKnight
        };

        private static readonly int[] ElfnoteCenterMonsters = {
            CardId.ElfnoteJunePride,
            CardId.ElfnoteLucina,
            CardId.ElfnoteRegina,
            CardId.ElfnoteTinia,
            CardId.ElfnoteFortuna,
            CardId.ElfnoteSeraphimStrelitzia
        };

        internal ElfnotePlugin Plugin { get; private set; }
        public ClientCard CurrentCard => Card;

        private bool _junePrideSwarmedThisTurn = false;
        private bool _lucinaSearchUsed = false;
        private bool _reginaSummonUsed = false;
        private bool _tiniaPlacedUsed = false;
        private bool _fortunaPlacedUsed = false;
        private bool _powerPatronModulatedUsed = false;
        private bool _terminusUsed = false;
        private bool _junordoUsed = false;
        private bool _theorealizeMediusUsed = false;
        private bool _arsMagnaInfinityUsed = false;
        private bool _arsMagnaPurificationUsed = false;
        private bool _citrinitasUsed = false;

        public ElfnoteExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            Plugin = new ElfnotePlugin(this);
            DeckPlugin = Plugin;

            RegisterHelperModules();
            RegisterExecutors();
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _junePrideSwarmedThisTurn = false;
            _lucinaSearchUsed = false;
            _reginaSummonUsed = false;
            _tiniaPlacedUsed = false;
            _fortunaPlacedUsed = false;
            _powerPatronModulatedUsed = false;
            _terminusUsed = false;
            _junordoUsed = false;
            _theorealizeMediusUsed = false;
            _arsMagnaInfinityUsed = false;
            _arsMagnaPurificationUsed = false;
            _citrinitasUsed = false;
            Plugin?.ResetTurnState();
        }

        public override bool OnSelectHand()
        {
            // Elfnote sets up an impregnable Center Zone castle on Turn 1
            return true;
        }

        private void RegisterHelperModules()
        {
            // 1. Ace Card Protection
            ResourcePlan.RegisterAceCards(BossMonsters);
            HeuristicGuard.RegisterAceCards(BossMonsters);

            // 2. Bait & Combo Starters
            BaitPlanner.RegisterComboStarters(
                CardId.ElfnoteLucina,
                CardId.UnleashedPowerPatronPortalTerminus,
                CardId.ElfnotesWelcomeHome,
                CardId.MediusThePure,
                CardId.TheorealizeMedius,
                CardId.PowerPatronShadowSpiritJunordo,
                CardId.ArsMagnaOfInfinityAndFinity
            );
            BaitPlanner.RegisterBaitCards(
                CardId.TheorealizePastLull,
                CardId.TripleTacticsTalent,
                CardId.ArsMagnaCitrinitas
            );

            // 3. Negators & Chain Disruption
            ChainAdvisor.RegisterHighValueTargets(
                CardId.SolemnJudgment,
                CardId.ArtmageDiactorus,
                CardId.BaronneDeFleur,
                CardId.CrystalWingSynchroDragon,
                CardId.FADawnDragster,
                CardId.ElfnotesRhapsodiaOfMadness,
                CardId.InfiniteImpermanence,
                CardId.AshBlossom,
                CardId.AshBlossomAlt,
                CardId.GhostOgre
            );
        }

        private void RegisterExecutors()
        {
            // ═══════════════════════════════════════════════════════════════
            // TIER 0: COUNTER-TRAPS, QUICK NEGATORS & HANDTRAPS
            // ═══════════════════════════════════════════════════════════════

            // Solemn Judgment: Protect Center Zone and negate opponent critical cards
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, DefaultSolemnJudgment);

            // Maxx "C": Draw on special summons
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, DefaultMaxxC);

            // Mulcharmy Fuwalos: Quick Effect from hand when we control no cards
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosActivate);

            // Infinite Impermanence: Handtrap or set trap monster negate
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, DefaultInfiniteImpermanence);

            // Ash Blossom: High-priority search/draw denial
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAlt, DefaultAshBlossomAndJoyousSpring);

            // Ghost Ogre & Snow Rabbit: Destroy card that activates on field
            AddExecutor(ExecutorType.Activate, CardId.GhostOgre, DefaultGhostOgreAndSnowRabbit);

            // Nibiru, the Primal Being: Board wipe if enemy summoned 5+ monsters
            AddExecutor(ExecutorType.Activate, CardId.NibiruThePrimalBeing, DefaultNibiru);

            // Baronne de Fleur: Omni-negate activation
            AddExecutor(ExecutorType.Activate, CardId.BaronneDeFleur, BaronneNegate);

            // Artmage Diactorus: Omni-negate on-field card/effect when 3+ races on field
            AddExecutor(ExecutorType.Activate, CardId.ArtmageDiactorus, DiactorusNegate);

            // Crystal Wing Synchro Dragon: Negate monster effect activation and destroy
            AddExecutor(ExecutorType.Activate, CardId.CrystalWingSynchroDragon, CrystalWingEffect);

            // F.A. Dawn Dragster: Spell/Trap negate
            AddExecutor(ExecutorType.Activate, CardId.FADawnDragster, DawnDragsterEffect);

            // Elfnotes: Rhapsodia of Madness: Negate face-up card if we control a Synchro Monster
            AddExecutor(ExecutorType.Activate, CardId.ElfnotesRhapsodiaOfMadness, RhapsodiaActivate);

            // Medicurius the Power Patron of Illusions: Total Board Banish Quick Effect
            AddExecutor(ExecutorType.Activate, CardId.MedicuriusThePowerPatronOfIllusions, MedicuriusBanishAll);

            // Junora: Quick Effect column material lockout
            AddExecutor(ExecutorType.Activate, CardId.JunoraThePowerPatronOfTuning, JunoraActivate);

            // S:P Little Knight: Quick Effect banish dodge / disruption
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightEffect);

            // Ravenous Crocodragon Archethys: Draw on summon & Quick pop
            AddExecutor(ExecutorType.Activate, CardId.RavenousCrocodragonArchethys, CrocodragonEffect);

            // ═══════════════════════════════════════════════════════════════
            // TIER 1: REMOVAL & ADVANTAGE SPELLS / IGNITIONS
            // ═══════════════════════════════════════════════════════════════

            // Triple Tactics Talent: Draw 2 or Take Control
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentActivate);

            // Baronne de Fleur: MP1 ignition target destroy
            AddExecutor(ExecutorType.Activate, CardId.BaronneDeFleur, BaronnePop);

            // Unleashed Power Patron Portal - Terminus: Dump Medicurius/Junora -> Add Tuner or Junordo
            AddExecutor(ExecutorType.Activate, CardId.UnleashedPowerPatronPortalTerminus, TerminusActivate);

            // Theorealize Past Lull: SS Medius from Deck
            AddExecutor(ExecutorType.Activate, CardId.TheorealizePastLull, TheorealizeActivate);

            // Ars Magna - "Citrinitas": Continuous Spell -> Search Medius or Ars Magna
            AddExecutor(ExecutorType.Activate, CardId.ArsMagnaCitrinitas, CitrinitasActivate);

            // Ars Magna of Infinity and Finity: Hand banish to search Purification
            AddExecutor(ExecutorType.Activate, CardId.ArsMagnaOfInfinityAndFinity, ArsMagnaInfinityHand);

            // Ars Magna of Purification and Corruption: Hand banish to search Citrinitas
            AddExecutor(ExecutorType.Activate, CardId.ArsMagnaOfPurificationAndCorruption, ArsMagnaPurificationHand);

            // Elfnotes: Welcome Home: Send fodder -> SS Elfnote with different attribute
            AddExecutor(ExecutorType.Activate, CardId.ElfnotesWelcomeHome, WelcomeHomeActivate);

            // PSY-Framelord Omega: Banish self and random opp hand card
            AddExecutor(ExecutorType.Activate, CardId.PSYFramelordOmega, OmegaRip);

            // ═══════════════════════════════════════════════════════════════
            // TIER 2: POWER PATRON & THEOREALIZE STARTERS / SUMMONS
            // ═══════════════════════════════════════════════════════════════

            // Junordo (Monster ignition): Banish 3 face-down -> SS Junora -> Board-wide negate!
            AddExecutor(ExecutorType.Activate, CardId.PowerPatronShadowSpiritJunordo, JunordoActivate);
            AddExecutor(ExecutorType.Summon, CardId.PowerPatronShadowSpiritJunordo, JunordoSummon);

            // Theorealize Medius: SS from hand when Power Patron on field, search Past Lull; on banish cheat Diactorus!
            AddExecutor(ExecutorType.Activate, CardId.TheorealizeMedius, TheorealizeMediusActivate);

            // Medius the Pure: NS/SS -> SS or search Power Patron
            AddExecutor(ExecutorType.Summon, CardId.MediusThePure, MediusSummon);
            AddExecutor(ExecutorType.Activate, CardId.MediusThePure, MediusActivate);

            // Jet Synchron: Normal Summon or GY revive
            AddExecutor(ExecutorType.Summon, CardId.JetSynchron, JetSynchronSummon);
            AddExecutor(ExecutorType.Activate, CardId.JetSynchron, JetSynchronActivate);

            // Ars Magna field banish effects (Infinity banish monster, Purification banish spells)
            AddExecutor(ExecutorType.Activate, CardId.ArsMagnaOfInfinityAndFinity, ArsMagnaInfinityField);
            AddExecutor(ExecutorType.Activate, CardId.ArsMagnaOfPurificationAndCorruption, ArsMagnaPurificationField);

            // ═══════════════════════════════════════════════════════════════
            // TIER 3: ELFNOTE MAIN-DECK SUMMONS & MODULATIONS
            // ═══════════════════════════════════════════════════════════════

            // Lucina: SS from hand to Center Zone, then search Regina/Tuner
            AddExecutor(ExecutorType.SpSummon, CardId.ElfnoteLucina, LucinaSpSummon);
            AddExecutor(ExecutorType.Summon, CardId.ElfnoteLucina, LucinaNormalSummon);
            AddExecutor(ExecutorType.Activate, CardId.ElfnoteLucina, LucinaActivate);

            // Regina: Quick Effect in hand -> send Elfnote fodder -> SS to Center -> SS Tuner from Deck!
            AddExecutor(ExecutorType.Activate, CardId.ElfnoteRegina, ReginaActivate);
            AddExecutor(ExecutorType.Summon, CardId.ElfnoteRegina, ReginaNormalSummon);

            // Tinia: SS from hand to Center Zone -> Place Continuous Spell
            AddExecutor(ExecutorType.SpSummon, CardId.ElfnoteTinia, TiniaSpSummon);
            AddExecutor(ExecutorType.Summon, CardId.ElfnoteTinia, TiniaNormalSummon);
            AddExecutor(ExecutorType.Activate, CardId.ElfnoteTinia, TiniaActivate);

            // Fortuna: SS from hand to Center Zone -> Place Continuous Trap
            AddExecutor(ExecutorType.SpSummon, CardId.ElfnoteFortuna, FortunaSpSummon);
            AddExecutor(ExecutorType.Summon, CardId.ElfnoteFortuna, FortunaNormalSummon);
            AddExecutor(ExecutorType.Activate, CardId.ElfnoteFortuna, FortunaActivate);

            // Elfnote Power Patron: Quick modulation (+3 Level) & immediate Synchro!
            AddExecutor(ExecutorType.Summon, CardId.ElfnotePowerPatron, PowerPatronSummon);
            AddExecutor(ExecutorType.Activate, CardId.ElfnotePowerPatron, PowerPatronActivate);

            // ═══════════════════════════════════════════════════════════════
            // TIER 4: EXTRA DECK SYNCHRO & LINK CLIMBS
            // ═══════════════════════════════════════════════════════════════

            // June Pride: Center Zone boss -> Tag-out swarm!
            AddExecutor(ExecutorType.SpSummon, CardId.ElfnoteJunePride, SynchroJunePride);
            AddExecutor(ExecutorType.Activate, CardId.ElfnoteJunePride, JunePrideTagOut);

            // Baronne de Fleur (Lv 10)
            AddExecutor(ExecutorType.SpSummon, CardId.BaronneDeFleur, SynchroBaronne);

            // Chaos Angel (Lv 10)
            AddExecutor(ExecutorType.SpSummon, CardId.ChaosAngel, SynchroChaosAngel);

            // Ravenous Crocodragon Archethys (Lv 9)
            AddExecutor(ExecutorType.SpSummon, CardId.RavenousCrocodragonArchethys, SynchroCrocodragon);

            // Crystal Wing Synchro Dragon (Lv 8)
            AddExecutor(ExecutorType.SpSummon, CardId.CrystalWingSynchroDragon, SynchroCrystalWing);

            // PSY-Framelord Omega (Lv 8)
            AddExecutor(ExecutorType.SpSummon, CardId.PSYFramelordOmega, SynchroOmega);

            // Elfnote Seraphim Strelitzia (Lv 7)
            AddExecutor(ExecutorType.SpSummon, CardId.ElfnoteSeraphimStrelitzia, SynchroStrelitzia);
            AddExecutor(ExecutorType.Activate, CardId.ElfnoteSeraphimStrelitzia, StrelitziaActivate);

            // F.A. Dawn Dragster (Lv 7)
            AddExecutor(ExecutorType.SpSummon, CardId.FADawnDragster, SynchroDawnDragster);

            // Black Rose Dragon (Lv 7 board wipe going second)
            AddExecutor(ExecutorType.SpSummon, CardId.BlackRoseDragon, SynchroBlackRose);
            AddExecutor(ExecutorType.SpSummon, CardId.BlackRoseDragonAlt, SynchroBlackRose);
            AddExecutor(ExecutorType.Activate, CardId.BlackRoseDragon, BlackRoseBoardWipe);
            AddExecutor(ExecutorType.Activate, CardId.BlackRoseDragonAlt, BlackRoseBoardWipe);

            // Accel Synchron (Lv 5 Tuner)
            AddExecutor(ExecutorType.SpSummon, CardId.AccelSynchron, SynchroAccelSynchron);
            AddExecutor(ExecutorType.Activate, CardId.AccelSynchron, AccelSynchronActivate);

            // Medicurius the Power Patron of Illusions (Link-3)
            AddExecutor(ExecutorType.SpSummon, CardId.MedicuriusThePowerPatronOfIllusions, LinkMedicurius);

            // S:P Little Knight (Link-2)
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, LinkSPLittleKnight);

            // Purification Power Patron (Link-2)
            AddExecutor(ExecutorType.SpSummon, CardId.PurificationPowerPatron, LinkPurification);
            AddExecutor(ExecutorType.Activate, CardId.PurificationPowerPatron, PurificationActivate);

            // ═══════════════════════════════════════════════════════════════
            // TIER 5: SET SP/TRAPS & BATTLE REPOSITIONING
            // ═══════════════════════════════════════════════════════════════

            AddExecutor(ExecutorType.SpellSet, CardId.SolemnJudgment);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence);
            AddExecutor(ExecutorType.SpellSet, CardId.ElfnotesRhapsodiaOfMadness);

            AddExecutor(ExecutorType.Repos, SmartMonsterRepos);
        }

        // ═══════════════════════════════════════════════════════════════
        // TIER 0: NEGATIONS & QUICK DISRUPTIONS
        // ═══════════════════════════════════════════════════════════════

        private bool BaronneNegate()
        {
            if (Duel.LastChainPlayer != 1) return false;
            return true;
        }

        private bool BaronnePop()
        {
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
            var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && IsTargetable(c))
                      ?? Enemy.GetSpells().FirstOrDefault(c => c != null && IsTargetable(c));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool DiactorusNegate()
        {
            if (Duel.LastChainPlayer != 1) return false;
            // Check if we control 3+ different races
            var races = Bot.GetMonsters().Where(c => c != null && c.IsFaceup()).Select(c => c.Race).Distinct().Count();
            return races >= 3;
        }

        private bool CrystalWingEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            return true;
        }

        private bool DawnDragsterEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            return true;
        }

        private bool RhapsodiaActivate()
        {
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                // Activated trigger on field: revive Elfnote and negate face-up card
                var enemyTarget = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled() && IsTargetable(c))
                               ?? Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled() && IsTargetable(c));
                if (enemyTarget != null)
                {
                    AI.SelectCard(enemyTarget);
                    return true;
                }
                return false;
            }

            // Activating from hand or face-down
            return true;
        }

        private bool MedicuriusBanishAll()
        {
            if (Duel.Player == 0) return false; // Opponent turn only
            // Condition e3: linked group count == 3
            if (Card == null || Card.Location != CardLocation.MonsterZone) return false;
            if (Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 2 || Duel.Phase == DuelPhase.BattleStart)
            {
                return true;
            }
            return false;
        }

        private bool JunoraActivate()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup())
            {
                // Lock opponent material in same column as Elfnote
                return Duel.Player == 1 && Enemy.GetMonsterCount() > 0;
            }
            // On summon negate all face-up opp cards
            return true;
        }

        private bool SPLittleKnightEffect()
        {
            // On summon banish 1 enemy card
            var banishTarget = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && IsTargetable(c))
                            ?? Enemy.GetSpells().FirstOrDefault(c => c != null && IsTargetable(c))
                            ?? Enemy.Graveyard.FirstOrDefault(c => c != null && CardIntelligence.IsHighThreatChokepoint(c.Id));
            if (banishTarget != null)
            {
                AI.SelectCard(banishTarget);
                return true;
            }

            // Quick effect dodge
            if (Duel.LastChainPlayer == 1)
            {
                var enemyCard = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && IsTargetable(c));
                if (enemyCard != null)
                {
                    AI.SelectCard(new[] { Card, enemyCard });
                    return true;
                }
            }

            return false;
        }

        private bool CrocodragonEffect()
        {
            // Draw on summon
            if (Card.Location == CardLocation.MonsterZone && Duel.LastChainPlayer == 0)
                return true;

            // Quick pop if enemy activates or in battle
            if (Duel.LastChainPlayer == 1 && Bot.Hand.Count >= 2)
            {
                var popTarget = Enemy.GetMonsters().FirstOrDefault(c => c != null && IsTargetable(c))
                             ?? Enemy.GetSpells().FirstOrDefault(c => c != null && IsTargetable(c));
                if (popTarget != null)
                {
                    AI.SelectCard(popTarget);
                    return true;
                }
            }
            return false;
        }

        private bool MulcharmyFuwalosActivate()
        {
            // Can only activate if we control no cards
            return Bot.GetMonsterCount() == 0 && Bot.GetSpellCount() == 0 && Duel.Player == 1;
        }

        // ═══════════════════════════════════════════════════════════════
        // TIER 1: ADVANTAGE SPELLS & FIELD IGNITIONS
        // ═══════════════════════════════════════════════════════════════

        private bool TripleTacticsTalentActivate()
        {
            if (Duel.Player != 0) return false;
            if (Enemy.GetMonsterCount() >= 1 && Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= 2500))
            {
                AI.SelectOption(1);
                return true;
            }
            if (Bot.Hand.Count <= 4)
            {
                AI.SelectOption(0);
                return true;
            }
            AI.SelectOption(2);
            return true;
        }

        private bool TerminusActivate()
        {
            if (_terminusUsed) return false;
            _terminusUsed = true;
            return true;
        }

        private bool TheorealizeActivate()
        {
            // Special summon Medius the Pure from Deck
            return Bot.GetMonsterCount() < 5;
        }

        private bool CitrinitasActivate()
        {
            if (_citrinitasUsed) return false;
            _citrinitasUsed = true;
            return true;
        }

        private bool ArsMagnaInfinityHand()
        {
            if (_arsMagnaInfinityUsed) return false;
            _arsMagnaInfinityUsed = true;
            return true;
        }

        private bool ArsMagnaPurificationHand()
        {
            if (_arsMagnaPurificationUsed) return false;
            _arsMagnaPurificationUsed = true;
            return true;
        }

        private bool WelcomeHomeActivate()
        {
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                // Send fodder to SS Elfnote with different attribute
                return Bot.GetMonsterCount() >= 1 && Bot.GetMonsterCount() < 5;
            }
            return true;
        }

        private bool OmegaRip()
        {
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
            return Enemy.Hand.Count > 0;
        }

        // ═══════════════════════════════════════════════════════════════
        // TIER 2: POWER PATRON & THEOREALIZE SUMMON ENGINE
        // ═══════════════════════════════════════════════════════════════

        private bool JunordoActivate()
        {
            if (_junordoUsed) return false;
            // Banish top 3 face-down -> SS Junora from Extra Deck!
            _junordoUsed = true;
            return true;
        }

        private bool JunordoSummon()
        {
            return Bot.GetMonsterCount() < 5 && !_junordoUsed;
        }

        private bool TheorealizeMediusActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_theorealizeMediusUsed) return false;
                // SS self if Power Patron on field
                bool hasPowerPatron = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.IsCode(CardId.PurificationPowerPatron) || c.IsCode(CardId.MediusThePure) || c.IsCode(CardId.ElfnotePowerPatron) || c.IsCode(CardId.JunoraThePowerPatronOfTuning) || c.IsCode(CardId.MedicuriusThePowerPatronOfIllusions)));
                if (hasPowerPatron && Bot.GetMonsterCount() < 5)
                {
                    _theorealizeMediusUsed = true;
                    return true;
                }
                return false;
            }

            // On-field trigger: when monster banished face-up -> banish self to SS Diactorus!
            return true;
        }

        private bool MediusSummon()
        {
            return Bot.GetMonsterCount() < 5;
        }

        private bool MediusActivate()
        {
            return true;
        }

        private bool JetSynchronSummon()
        {
            return Bot.GetMonsterCount() < 5 && IsCenterZoneFree();
        }

        private bool JetSynchronActivate()
        {
            // GY revive
            return Card.Location == CardLocation.Grave && Bot.GetMonsterCount() < 5 && Bot.Hand.Count >= 1;
        }

        private bool ArsMagnaInfinityField()
        {
            // Banish 1 monster if we control Power Patron Link
            bool hasLink = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.IsCode(CardId.PurificationPowerPatron) || c.IsCode(CardId.MedicuriusThePowerPatronOfIllusions)));
            if (!hasLink) return false;

            var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && IsTargetable(c));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool ArsMagnaPurificationField()
        {
            // Banish Spells/Traps up to number of Power Patron Link
            int linkCount = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && (c.IsCode(CardId.PurificationPowerPatron) || c.IsCode(CardId.MedicuriusThePowerPatronOfIllusions)));
            if (linkCount == 0) return false;

            var targets = Enemy.GetSpells().Where(c => c != null && IsTargetable(c)).Take(linkCount).ToList();
            if (targets.Count > 0)
            {
                AI.SelectCard(targets);
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        // TIER 3: ELFNOTE MAIN DECK MONSTERS
        // ═══════════════════════════════════════════════════════════════

        private bool LucinaSpSummon()
        {
            return IsCenterZoneFree();
        }

        private bool LucinaNormalSummon()
        {
            // Level 6 monster: do not tribute our monsters to Normal Summon
            return false;
        }

        private bool LucinaActivate()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup())
            {
                // In opponent's turn: swap with center and bounce Lv 6 or lower monster
                if (Duel.Player == 1)
                {
                    var bounceTarget = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Level > 0 && c.Level <= 6 && IsTargetable(c));
                    if (bounceTarget != null)
                    {
                        AI.SelectCard(bounceTarget);
                        return true;
                    }
                    return false;
                }

                // In our turn: search Elfnote monster
                if (!_lucinaSearchUsed)
                {
                    _lucinaSearchUsed = true;
                    return true;
                }
            }
            return true;
        }

        private bool ReginaActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_reginaSummonUsed) return false;
                // Discard 1 other Elfnote -> SS to Center
                bool hasFodder = Bot.Hand.Any(c => c != null && c != Card && IsElfnote(c))
                              || Bot.GetMonsters().Any(c => c != null && c.Sequence != 2 && IsElfnote(c));
                if (hasFodder && (IsCenterZoneFree() || Bot.GetMonsterCount() < 5))
                {
                    _reginaSummonUsed = true;
                    return true;
                }
                return false;
            }

            // If summoned to Center Zone -> SS Elfnote from Deck!
            if (Card.Location == CardLocation.MonsterZone && Card.Sequence == 2)
            {
                return true;
            }

            // If sent to GY as Synchro Material -> add self back to hand!
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }

            return true;
        }

        private bool ReginaNormalSummon()
        {
            // Level 6 monster: do not tribute our monsters
            return false;
        }

        private bool TiniaSpSummon()
        {
            return IsCenterZoneFree();
        }

        private bool TiniaNormalSummon()
        {
            // Level 6 monster: do not tribute our monsters
            return false;
        }

        private bool TiniaActivate()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup())
            {
                if (Duel.Player == 1)
                {
                    // Opponent turn: banish 1 random card from opp hand until EP
                    return Enemy.Hand.Count > 0;
                }
                if (!_tiniaPlacedUsed)
                {
                    _tiniaPlacedUsed = true;
                    return true;
                }
            }
            return true;
        }

        private bool FortunaSpSummon()
        {
            return IsCenterZoneFree();
        }

        private bool FortunaNormalSummon()
        {
            // Level 6 monster: do not tribute our monsters
            return false;
        }

        private bool FortunaActivate()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup())
            {
                if (Duel.Player == 1)
                {
                    // Bounce face-up Spell/Trap
                    var bounce = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && IsTargetable(c));
                    if (bounce != null)
                    {
                        AI.SelectCard(bounce);
                        return true;
                    }
                    return false;
                }
                if (!_fortunaPlacedUsed)
                {
                    _fortunaPlacedUsed = true;
                    return true;
                }
            }
            return true;
        }

        private bool PowerPatronSummon()
        {
            return Bot.GetMonsterCount() < 5;
        }

        private bool PowerPatronActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_powerPatronModulatedUsed) return false;
                var center = GetCenterMonster();
                if (center != null && center.IsFaceup() && center.Level >= 6)
                {
                    _powerPatronModulatedUsed = true;
                    AI.SelectCard(center);
                    return true;
                }
                return false;
            }

            // Sent to GY as Synchro Material -> Search ANY Elfnote card!
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }

            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        // TIER 4: EXTRA DECK SYNCHRO & LINK CLIMBS
        // ═══════════════════════════════════════════════════════════════

        private bool SynchroJunePride()
        {
            return true;
        }

        private bool JunePrideTagOut()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.Sequence != 2) return false;
            if (_junePrideSwarmedThisTurn) return false;

            // In our turn: tag out if we need to swarm for links or after attacking
            if (Duel.Player == 0)
            {
                if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Main2 || Bot.GetMonsterCount() <= 2)
                {
                    _junePrideSwarmedThisTurn = true;
                    return true;
                }
            }

            // In opponent's turn: tag out when opponent enters Battle Phase or targets June Pride
            if (Duel.Player == 1)
            {
                if (Duel.Phase == DuelPhase.BattleStart || Duel.LastChainPlayer == 1)
                {
                    _junePrideSwarmedThisTurn = true;
                    return true;
                }
            }

            return false;
        }

        private bool SynchroBaronne()
        {
            return true;
        }

        private bool SynchroChaosAngel()
        {
            return true;
        }

        private bool SynchroCrocodragon()
        {
            return true;
        }

        private bool SynchroCrystalWing()
        {
            return true;
        }

        private bool SynchroOmega()
        {
            return true;
        }

        private bool SynchroStrelitzia()
        {
            return true;
        }

        private bool StrelitziaActivate()
        {
            // SS Lv 6 or lower Elfnote from hand or GY
            return Bot.GetMonsterCount() < 5;
        }

        private bool SynchroDawnDragster()
        {
            return true;
        }

        private bool SynchroBlackRose()
        {
            return _isGoingSecond && Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 2;
        }

        private bool BlackRoseBoardWipe()
        {
            return Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 2;
        }

        private bool SynchroAccelSynchron()
        {
            return true;
        }

        private bool AccelSynchronActivate()
        {
            // Dump Jet Synchron from Deck to modulate level
            return true;
        }

        private bool LinkMedicurius()
        {
            // Need 2+ monsters including Fusion, Synchro, Link
            bool hasExtra = Bot.GetMonsters().Any(c => c != null && (c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Link)));
            return hasExtra && Bot.GetMonsterCount() >= 3;
        }

        private bool LinkSPLittleKnight()
        {
            return Bot.GetMonsterCount() >= 2 && Enemy.GetMonsterCount() > 0;
        }

        private bool LinkPurification()
        {
            return Bot.GetMonsterCount() >= 3 && !HasAceOnBoard();
        }

        private bool PurificationActivate()
        {
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        // REPOSITIONING & UTILITIES
        // ═══════════════════════════════════════════════════════════════

        private bool SmartMonsterRepos()
        {
            if (Card == null) return false;

            // 0 ATK monsters or Handtraps accidentally in Attack -> switch to Defense
            if (Card.IsAttack() && (Card.Attack == 0 || (Card.Defense > Card.Attack && Card.Defense >= 1800)))
                return true;

            // High ATK bosses in Defense -> switch to Attack to deal lethal
            if (Card.IsDefense() && Card.Attack > Card.Defense && Card.Attack >= 1800 && Duel.Turn > 1)
                return true;

            return DefaultMonsterRepos();
        }

        private bool IsCenterZoneFree()
        {
            var center = GetCenterMonster();
            return center == null;
        }

        private ClientCard GetCenterMonster()
        {
            return Bot.GetMonsters().FirstOrDefault(c => c != null && c.Sequence == 2);
        }

        private bool IsElfnote(ClientCard card)
        {
            if (card == null) return false;
            return card.IsCode(
                CardId.ElfnoteLucina,
                CardId.ElfnoteRegina,
                CardId.ElfnoteTinia,
                CardId.ElfnoteFortuna,
                CardId.ElfnotePowerPatron,
                CardId.PowerPatronShadowSpiritJunordo,
                CardId.ElfnoteJunePride,
                CardId.ElfnoteSeraphimStrelitzia,
                CardId.ElfnotesWelcomeHome,
                CardId.ElfnotesRhapsodiaOfMadness
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

        // ═══════════════════════════════════════════════════════════════
        // CALLBACK OVERRIDES (ZONE PLACEMENT, TARGETS, OPTIONS)
        // ═══════════════════════════════════════════════════════════════

        public override int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
            if (location == CardLocation.MonsterZone)
            {
                long actualId = (cardId != 0) ? cardId : (Card != null ? Card.Id : 0);
                int centerZone = 1 << 2; // Zone 2 (Center Main Monster Zone)

                // Key Elfnote monsters MUST take Center Zone if available
                if (ElfnoteCenterMonsters.Contains((int)actualId))
                {
                    if ((available & centerZone) > 0)
                        return centerZone;
                }
                else
                {
                    // For other monsters (Tuner, Fodder, non-Elfnote Synchros), preserve Center Zone!
                    int nonCenterAvailable = available & ~centerZone;
                    if (nonCenterAvailable > 0)
                        return nonCenterAvailable;
                }
            }

            return base.OnSelectPlace(cardId, player, location, available);
        }

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0) return 0;

            for (int i = 0; i < options.Count; i++)
            {
                long cardId = options[i] >> 4;
                if (cardId == 0 && Card != null) cardId = Card.Id;
                long optIndex = options[i] & 0xf;

                // Theorealize Medius:
                // Option 1 or stringid 4: Banish self -> Special Summon Diactorus from Extra Deck!
                // Option 0 or stringid 3: Add Ars Magna card
                if (cardId == CardId.TheorealizeMedius)
                {
                    if (optIndex == 1 || optIndex == 4) return i;
                }

                // Medius the Pure: Option 1: Special Summon, Option 0: Add to Hand
                if (cardId == CardId.MediusThePure)
                {
                    if (optIndex == 1 && Bot.GetMonsterCount() < 5) return i;
                    if (optIndex == 0) return i;
                }

                // Triple Tactics Talent:
                // Option 0: Draw 2 cards
                // Option 1: Take control of 1 monster
                // Option 2: Hand shuffle
                if (cardId == CardId.TripleTacticsTalent)
                {
                    bool hasHighAtkEnemy = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= 2500 && IsTargetable(c));
                    if (hasHighAtkEnemy && optIndex == 1 && Bot.GetMonsterCount() < 5)
                        return i;
                    if (optIndex == 0)
                        return i;
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

                // 1. Handtraps (Ash 0/1800, etc.) or 0 ATK -> Defense
                if (cardData.Attack == 0 || CardIntelligence.IsHandtrap(cardId))
                {
                    if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                    if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                }

                // 2. High DEF / Low ATK walls -> Defense
                if (cardData.Defense > cardData.Attack && cardData.Attack < 1800)
                {
                    if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                    if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                }

                // 3. Bosses & Attackers (ATK >= 1800) -> Attack
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
                var target = Plugin.Strategy.PickSpecialSummonTarget(cards);
                if (target != null)
                {
                    return new List<ClientCard> { target };
                }
            }

            // 3. Hint 501 / 504: DISCARD / TOGRAVE
            if (hint == 501 || hint == 504)
            {
                // Check if this is Terminus sending from Extra Deck
                if (Card != null && Card.IsCode(CardId.UnleashedPowerPatronPortalTerminus))
                {
                    var dumpTarget = Plugin.StrategyImpl.PickDumpTarget(cards, Card);
                    if (dumpTarget != null)
                    {
                        return new List<ClientCard> { dumpTarget };
                    }
                }

                var discardTarget = Plugin.MaterialEvaluator.PickDiscardTarget(cards, min);
                if (discardTarget != null)
                {
                    return new List<ClientCard> { discardTarget };
                }
            }

            // 4. Hint 551: TARGET (Center monster for Power Patron level boost)
            if (hint == 551 && Card != null && Card.IsCode(CardId.ElfnotePowerPatron))
            {
                var center = cards.FirstOrDefault(c => c != null && c.Sequence == 2);
                if (center != null)
                {
                    return new List<ClientCard> { center };
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
