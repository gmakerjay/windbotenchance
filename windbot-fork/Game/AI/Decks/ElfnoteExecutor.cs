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
    public class ElfnoteExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Deck Monsters
            public const int ElfnoteLucina = 13597785;
            public const int ElfnoteRegina = 56651978;
            public const int ElfnoteTinia = 59581480;
            public const int ElfnoteFortuna = 85976588;
            public const int ElfnotePowerPatron = 12375297;
            public const int PowerPatronShadowSpiritJunordo = 10266279;
            public const int MediusThePure = 97556336;
            public const int VidriumThePowerPatronOfChaosExtermination = 70488851;
            public const int FidraulisHarmonia = 70088809;
            public const int AshBlossom = 14558127;
            public const int AshBlossomAlt = 14558128;
            public const int BystialDruiswurm = 6637331;
            public const int BystialMagnamhut = 33854624;

            // Spells
            public const int UnleashedPowerPatronPortalTerminus = 25661743;
            public const int ElfnotesWelcomeHome = 64491754;
            public const int TheorealizePastLull = 36709484;
            public const int CalledByTheGrave = 24224830;
            public const int CalledByTheGraveAlt = 24224831;
            public const int TripleTacticsTalent = 25311006;

            // Traps
            public const int ElfnotesRhapsodiaOfMadness = 24092792;
            public const int ElfnotesAristeiaOfTrust = 50590801;
            public const int SolemnJudgment = 41420027;
            public const int InfiniteImpermanence = 10045474;
            public const int AntiSpellFragrance = 58921041;
            public const int SynchroEmergency = 49415281;

            // Extra Deck
            public const int ElfnoteJunePride = 5559570;
            public const int JunoraThePowerPatronOfTuning = 5914858;
            public const int ElfnoteSeraphimStrelitzia = 42302563;
            public const int ArmsOfGenexReturnZero = 61775475;
            public const int ChaosAngel = 22850702;
            public const int CrystalWingSynchroDragon = 50954680;
            public const int StardustDragonVictimSanctuary = 76636978;
            public const int AccelSynchroStardustDragon = 30983281;
            public const int StardustDragon = 44508094;
            public const int FADawnDragster = 33158448;
            public const int WindPegasusIgnister = 98506199;
            public const int GoldenCloudBeastMalong = 93125329;
            public const int BlackRoseDragon = 73580471;
            public const int StardustWarrior = 74892653;
            public const int PurificationPowerPatron = 31822037;
        }

        private static readonly int[] BossMonsters = {
            CardId.ElfnoteJunePride,
            CardId.JunoraThePowerPatronOfTuning,
            CardId.ArmsOfGenexReturnZero,
            CardId.CrystalWingSynchroDragon,
            CardId.ChaosAngel,
            CardId.StardustDragonVictimSanctuary,
            CardId.StardustWarrior,
            CardId.StardustDragon,
            CardId.FADawnDragster,
            CardId.ElfnoteSeraphimStrelitzia
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

        private bool _junePrideSwarmedThisTurn = false;
        private bool _lucinaSearchUsed = false;
        private bool _reginaSummonUsed = false;
        private bool _tiniaPlacedUsed = false;
        private bool _fortunaPlacedUsed = false;
        private bool _powerPatronModulatedUsed = false;
        private bool _terminusUsed = false;
        private bool _junordoUsed = false;

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
                CardId.PowerPatronShadowSpiritJunordo
            );
            BaitPlanner.RegisterBaitCards(
                CardId.TheorealizePastLull,
                CardId.TripleTacticsTalent
            );

            // 3. Negators & Chain Disruption
            ChainAdvisor.RegisterHighValueTargets(
                CardId.SolemnJudgment,
                CardId.CrystalWingSynchroDragon,
                CardId.ArmsOfGenexReturnZero,
                CardId.StardustDragonVictimSanctuary,
                CardId.FADawnDragster,
                CardId.ElfnotesAristeiaOfTrust,
                CardId.CalledByTheGrave,
                CardId.InfiniteImpermanence,
                CardId.AshBlossom
            );
        }

        private void RegisterExecutors()
        {
            // ═══════════════════════════════════════════════════════════════
            // TIER 0: COUNTER-TRAPS, QUICK NEGATORS & HANDTRAPS
            // ═══════════════════════════════════════════════════════════════

            // Solemn Judgment: Protect our Center Zone and deny opponent boss / board breaker
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, DefaultSolemnJudgment);

            // Called by the Grave: Negate opponent Handtraps (Ash, Droll, Maxx C) or GY bosses
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);

            // Infinite Impermanence: Handtrap or set trap monster negate
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, DefaultInfiniteImpermanence);

            // Ash Blossom: High-priority search/draw denial
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAlt, DefaultAshBlossomAndJoyousSpring);

            // Stardust Dragon - Victim Sanctuary: Negate opponent response and destroy!
            AddExecutor(ExecutorType.Activate, CardId.StardustDragonVictimSanctuary, VictimSanctuaryActivate);

            // Crystal Wing Synchro Dragon: Negate monster effect activation and destroy
            AddExecutor(ExecutorType.Activate, CardId.CrystalWingSynchroDragon, CrystalWingEffect);

            // Arms of Genex Return Zero: Omni-negate matching attribute in GY
            AddExecutor(ExecutorType.Activate, CardId.ArmsOfGenexReturnZero, ReturnZeroActivate);

            // F.A. Dawn Dragster: Spell/Trap negate
            AddExecutor(ExecutorType.Activate, CardId.FADawnDragster, DawnDragsterEffect);

            // Stardust Dragon: Negate destruction
            AddExecutor(ExecutorType.Activate, CardId.StardustDragon, StardustDragonEffect);

            // Elfnotes: Aristeia of Trust: Negate and destroy Spell/Trap if we control 3+ Elfnotes
            AddExecutor(ExecutorType.Activate, CardId.ElfnotesAristeiaOfTrust, AristeiaActivate);

            // Elfnotes: Rhapsodia of Madness: Negate face-up card if we control a Synchro Monster
            AddExecutor(ExecutorType.Activate, CardId.ElfnotesRhapsodiaOfMadness, RhapsodiaActivate);

            // Fidraulis Harmonia: Quick Effect from hand when opponent activates monster effect
            AddExecutor(ExecutorType.Activate, CardId.FidraulisHarmonia, HarmoniaActivate);

            // Bystials (Druiswurm, Magnamhut): Banish opponent GY LIGHT/DARK and summon
            AddExecutor(ExecutorType.Activate, CardId.BystialMagnamhut, BystialMagnamhutActivate);
            AddExecutor(ExecutorType.Activate, CardId.BystialDruiswurm, BystialDruiswurmActivate);

            // Anti-Spell Fragrance: Lock opponent Spells
            AddExecutor(ExecutorType.Activate, CardId.AntiSpellFragrance, AntiSpellActivate);

            // Synchro Emergency: Revive or Synchro Summon during battle/opponent turn
            AddExecutor(ExecutorType.Activate, CardId.SynchroEmergency, SynchroEmergencyActivate);

            // ═══════════════════════════════════════════════════════════════
            // TIER 1: REMOVAL & ADVANTAGE SPELLS
            // ═══════════════════════════════════════════════════════════════

            // Triple Tactics Talent: Draw 2 or Take Control
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentActivate);

            // Unleashed Power Patron Portal - Terminus: Dump Junora/Vidrium -> Add Junordo
            AddExecutor(ExecutorType.Activate, CardId.UnleashedPowerPatronPortalTerminus, TerminusActivate);

            // Theorealize Past Lull: SS Medius from Deck
            AddExecutor(ExecutorType.Activate, CardId.TheorealizePastLull, TheorealizeActivate);

            // Elfnotes: Welcome Home: Send fodder -> SS Elfnote with different attribute
            AddExecutor(ExecutorType.Activate, CardId.ElfnotesWelcomeHome, WelcomeHomeActivate);

            // ═══════════════════════════════════════════════════════════════
            // TIER 2: POWER PATRON & JUNORDO SUMMON ENGINE
            // ═══════════════════════════════════════════════════════════════

            // Junordo (Monster ignition): Banish 3 face-down -> SS Junora (treated as Synchro) -> Board-wide negate!
            AddExecutor(ExecutorType.Activate, CardId.PowerPatronShadowSpiritJunordo, JunordoActivate);
            AddExecutor(ExecutorType.Summon, CardId.PowerPatronShadowSpiritJunordo, JunordoSummon);

            // Junora on-summon trigger / material lockout
            AddExecutor(ExecutorType.Activate, CardId.JunoraThePowerPatronOfTuning, JunoraActivate);

            // Medius the Pure: NS/SS -> SS or search Power Patron
            AddExecutor(ExecutorType.Summon, CardId.MediusThePure, MediusSummon);
            AddExecutor(ExecutorType.Activate, CardId.MediusThePure, MediusActivate);

            // Vidrium the Power Patron of Chaos Extermination: SS from hand or GY banish removal
            AddExecutor(ExecutorType.Activate, CardId.VidriumThePowerPatronOfChaosExtermination, VidriumActivate);

            // ═══════════════════════════════════════════════════════════════
            // TIER 3: ELFNOTE CENTER-ZONE CASTLE & SWARM COMBO
            // ═══════════════════════════════════════════════════════════════

            // Elfnote June Pride: In Center Zone -> Return to ED -> SS 3 Elfnotes (Hand, Deck, GY)!
            AddExecutor(ExecutorType.Activate, CardId.ElfnoteJunePride, JunePrideActivate);

            // Elfnote Lucina: SS from hand to Center Zone (Zone 2)
            AddExecutor(ExecutorType.SpSummon, CardId.ElfnoteLucina, LucinaSpSummon);
            AddExecutor(ExecutorType.Summon, CardId.ElfnoteLucina, LucinaSummon);
            AddExecutor(ExecutorType.Activate, CardId.ElfnoteLucina, LucinaActivate);

            // Elfnote Regina: Pitch 1 Elfnote -> SS from hand, or SS from Deck when summoned to Center!
            AddExecutor(ExecutorType.Activate, CardId.ElfnoteRegina, ReginaActivate);
            AddExecutor(ExecutorType.Summon, CardId.ElfnoteRegina, ReginaSummon);

            // Elfnote Tinia: SS to Center Zone -> Place Welcome Home from Deck
            AddExecutor(ExecutorType.SpSummon, CardId.ElfnoteTinia, TiniaSpSummon);
            AddExecutor(ExecutorType.Summon, CardId.ElfnoteTinia, TiniaSummon);
            AddExecutor(ExecutorType.Activate, CardId.ElfnoteTinia, TiniaActivate);

            // Elfnote Fortuna: SS to Center Zone -> Place Continuous Trap from Deck
            AddExecutor(ExecutorType.SpSummon, CardId.ElfnoteFortuna, FortunaSpSummon);
            AddExecutor(ExecutorType.Summon, CardId.ElfnoteFortuna, FortunaSummon);
            AddExecutor(ExecutorType.Activate, CardId.ElfnoteFortuna, FortunaActivate);

            // Elfnote Power Patron: Level boost center monster by 3 -> Synchro Summon June Pride / Junora!
            AddExecutor(ExecutorType.Summon, CardId.ElfnotePowerPatron, PowerPatronSummon);
            AddExecutor(ExecutorType.Activate, CardId.ElfnotePowerPatron, PowerPatronActivate);

            // ═══════════════════════════════════════════════════════════════
            // TIER 4: EXTRA DECK SYNCHRO & LINK BOSS SUMMONS
            // ═══════════════════════════════════════════════════════════════

            // Elfnote June Pride (Lv 10)
            AddExecutor(ExecutorType.SpSummon, CardId.ElfnoteJunePride, SynchroJunePride);

            // Junora the Power Patron of Tuning (Lv 10)
            AddExecutor(ExecutorType.SpSummon, CardId.JunoraThePowerPatronOfTuning, SynchroJunora);

            // Arms of Genex Return Zero (Lv 10)
            AddExecutor(ExecutorType.SpSummon, CardId.ArmsOfGenexReturnZero, SynchroReturnZero);

            // Chaos Angel (Lv 10)
            AddExecutor(ExecutorType.SpSummon, CardId.ChaosAngel, SynchroChaosAngel);

            // Crystal Wing Synchro Dragon (Lv 8)
            AddExecutor(ExecutorType.SpSummon, CardId.CrystalWingSynchroDragon, SynchroCrystalWing);

            // Stardust Dragon - Victim Sanctuary (Lv 8)
            AddExecutor(ExecutorType.SpSummon, CardId.StardustDragonVictimSanctuary, SynchroVictimSanctuary);

            // Accel Synchro Stardust Dragon (Lv 8)
            AddExecutor(ExecutorType.SpSummon, CardId.AccelSynchroStardustDragon, SynchroAccelStardust);

            // Elfnote Seraphim Strelitzia (Lv 7)
            AddExecutor(ExecutorType.SpSummon, CardId.ElfnoteSeraphimStrelitzia, SynchroStrelitzia);
            AddExecutor(ExecutorType.Activate, CardId.ElfnoteSeraphimStrelitzia, StrelitziaActivate);

            // F.A. Dawn Dragster (Lv 7)
            AddExecutor(ExecutorType.SpSummon, CardId.FADawnDragster, SynchroDawnDragster);

            // Black Rose Dragon (Lv 7 board wipe going 2nd)
            AddExecutor(ExecutorType.SpSummon, CardId.BlackRoseDragon, SynchroBlackRose);
            AddExecutor(ExecutorType.Activate, CardId.BlackRoseDragon, BlackRoseActivate);

            // Purification Power Patron (Link 2)
            AddExecutor(ExecutorType.SpSummon, CardId.PurificationPowerPatron, LinkPurification);
            AddExecutor(ExecutorType.Activate, CardId.PurificationPowerPatron, PurificationActivate);

            // ═══════════════════════════════════════════════════════════════
            // TIER 5: TRAP SETTING & POSITION CONTROL
            // ═══════════════════════════════════════════════════════════════

            // Spell/Trap Setting
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnJudgment);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence);
            AddExecutor(ExecutorType.SpellSet, CardId.AntiSpellFragrance);
            AddExecutor(ExecutorType.SpellSet, CardId.SynchroEmergency);
            AddExecutor(ExecutorType.SpellSet, CardId.ElfnotesRhapsodiaOfMadness);
            AddExecutor(ExecutorType.SpellSet, CardId.ElfnotesAristeiaOfTrust);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave);

            // Monster Reposition
            AddExecutor(ExecutorType.Repos, SmartMonsterRepos);
        }

        // ═══════════════════════════════════════════════════════════════
        // EXECUTOR IMPLEMENTATIONS
        // ═══════════════════════════════════════════════════════════════

        private bool VictimSanctuaryActivate()
        {
            if (Card == null) return false;

            // Hand/Field effect: Negate opponent response to our activation and destroy
            if (Duel.LastChainPlayer == 1)
            {
                return true;
            }

            // GY effect: Banish if a monster was tributed to SS Stardust Dragon
            if (Card.Location == CardLocation.Grave)
            {
                return Bot.ExtraDeck.Any(c => c != null && c.IsCode(CardId.StardustDragon));
            }

            return false;
        }

        private bool ReturnZeroActivate()
        {
            // Negate opponent monster effect matching an attribute in our GY
            return Duel.LastChainPlayer == 1;
        }

        private bool CrystalWingEffect()
        {
            return LastChainCard != null && LastChainCard.Controller == 1 && LastChainCard.Location == CardLocation.MonsterZone;
        }

        private bool DawnDragsterEffect()
        {
            return LastChainCard != null && LastChainCard.Controller == 1 && (LastChainCard.IsSpell() || LastChainCard.IsTrap());
        }

        private bool StardustDragonEffect()
        {
            return LastChainCard != null && LastChainCard.Controller == 1;
        }

        private bool AristeiaActivate()
        {
            if (Card == null) return false;

            // Negate opponent Spell/Trap activation if we control 3+ Elfnotes
            if (Duel.LastChainPlayer == 1)
            {
                int elfnoteCount = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && IsElfnote(c));
                if (elfnoteCount >= 3)
                {
                    return true;
                }
            }

            return false;
        }

        private bool RhapsodiaActivate()
        {
            if (Card == null) return false;

            // Opponent turn interruption or Main Phase board negation
            if (Duel.LastChainPlayer == 1 || Duel.Turn > 1)
            {
                bool hasSynchro = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Synchro));
                bool hasEnemyFaceup = Enemy.GetMonsters().Concat(Enemy.GetSpells()).Any(c => c != null && c.IsFaceup() && !c.IsDisabled());

                if (hasSynchro && hasEnemyFaceup)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HarmoniaActivate()
        {
            if (Card == null) return false;

            // Quick effect from hand when opponent activates monster effect
            if (Card.Location == CardLocation.Hand && Duel.LastChainPlayer == 1)
            {
                int synchroCount = Bot.ExtraDeck.Count(c => c != null && c.HasType(CardType.Synchro));
                return synchroCount >= 5;
            }

            return false;
        }

        private bool BystialMagnamhutActivate()
        {
            if (Card == null) return false;
            if (Card.Location == CardLocation.Hand)
            {
                // Prioritize banishing enemy LIGHT or DARK from their GY
                return Enemy.Graveyard.Any(c => c != null && (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark)));
            }
            return true;
        }

        private bool BystialDruiswurmActivate()
        {
            if (Card == null) return false;
            if (Card.Location == CardLocation.Hand)
            {
                return Enemy.Graveyard.Any(c => c != null && (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark)));
            }
            // GY trigger: send 1 enemy Special Summoned monster to GY
            if (Card.Location == CardLocation.Grave)
            {
                return Enemy.GetMonsters().Any(c => c != null && c.IsSpecialSummoned);
            }
            return true;
        }

        private bool AntiSpellActivate()
        {
            // Activate during opponent turn or at end of our Main Phase 2
            return Duel.Player == 1 || Duel.Phase == DuelPhase.Main2 || Duel.Turn > 1;
        }

        private bool SynchroEmergencyActivate()
        {
            if (Card == null) return false;
            // Revive a key Synchro boss if our board has space
            return Bot.GetMonsterCount() < 5 && Bot.Graveyard.Any(c => c != null && BossMonsters.Contains(c.Id) && c.IsCanRevive());
        }

        private bool TripleTacticsTalentActivate()
        {
            // Allowed if opponent activated monster effect this turn
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
            if (Card == null) return false;
            if (Card.Location == CardLocation.Hand)
            {
                bool hasElfnote = Bot.GetMonsters().Concat(Bot.GetSpells()).Any(c => c != null && IsElfnote(c));
                return hasElfnote && Bot.GetMonsterCount() < 5;
            }
            return false;
        }

        private bool WelcomeHomeActivate()
        {
            if (Card == null) return false;
            // Send fodder to SS Elfnote with different attribute
            return Bot.GetMonsterCount() >= 1 && Bot.GetMonsterCount() < 5;
        }

        private bool JunordoSummon()
        {
            // We want Junordo on field to activate ignition effect (banish 3 -> SS Junora)
            return Bot.GetMonsterCount() < 5 && !_junordoUsed;
        }

        private bool JunordoActivate()
        {
            if (Card == null) return false;

            // Monster ignition: Banish 3 top deck face-down -> SS Junora from Extra Deck!
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (Bot.Deck.Count >= 3 && Bot.ExtraDeck.Any(c => c != null && c.IsCode(CardId.JunoraThePowerPatronOfTuning)))
                {
                    _junordoUsed = true;
                    return true;
                }
            }

            // GY / Extra Deck trigger: Add back Elfnote/Power Patron
            if (Card.Location == CardLocation.Extra)
            {
                return true;
            }

            return false;
        }

        private bool JunoraActivate()
        {
            // On Synchro Summon: Negates all face-up opponent cards!
            // In Monster Zone: Column material lockout
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

        private bool VidriumActivate()
        {
            if (Card == null) return false;

            // Hand SS if we control Power Patron
            if (Card.Location == CardLocation.Hand)
            {
                bool hasPowerPatron = Bot.GetMonsters().Any(c => c != null && (c.IsCode(CardId.ElfnotePowerPatron) || c.IsCode(CardId.JunoraThePowerPatronOfTuning) || c.IsCode(CardId.PowerPatronShadowSpiritJunordo) || c.IsCode(CardId.MediusThePure)));
                return hasPowerPatron && Bot.GetMonsterCount() < 5;
            }

            // GY banish removal: banish 1 enemy monster
            if (Card.Location == CardLocation.Grave)
            {
                return Enemy.GetMonsters().Any(c => c != null && IsTargetable(c));
            }

            return false;
        }

        private bool JunePrideActivate()
        {
            if (Card == null) return false;

            // While in Center Zone: Quick Effect to return to Extra Deck and SS up to 3 Elfnotes (Hand, Deck, GY)
            if (Card.Location == CardLocation.MonsterZone && Card.Sequence == 2)
            {
                // On our turn: Swarm if we have Tuner/Extenders to summon
                // On opponent turn: Swarm to trigger Lucina, Tinia, Fortuna disruption!
                if (!_junePrideSwarmedThisTurn)
                {
                    if (Duel.Player == 1 || Bot.GetMonsterCount() <= 2)
                    {
                        _junePrideSwarmedThisTurn = true;
                        return true;
                    }
                }
            }

            return false;
        }

        private bool LucinaSpSummon()
        {
            // Can SS to Center Zone (Zone 2)
            return IsCenterZoneFree();
        }

        private bool LucinaSummon()
        {
            return Bot.GetMonsterCount() < 5;
        }

        private bool LucinaActivate()
        {
            if (Card == null) return false;

            // Main Phase Ignition in Center: Search Elfnote monster
            if (Duel.IsMainPhase() && Duel.Player == 0 && Card.Sequence == 2)
            {
                if (!_lucinaSearchUsed)
                {
                    _lucinaSearchUsed = true;
                    return true;
                }
            }

            // Opponent turn Quick Effect: switch with center -> bounce Lv 6 or lower monster
            if (Duel.Player == 1)
            {
                return Enemy.GetMonsters().Any(c => c != null && c.Level > 0 && c.Level <= 6 && IsTargetable(c));
            }

            return false;
        }

        private bool ReginaSummon()
        {
            return Bot.GetMonsterCount() < 5;
        }

        private bool ReginaActivate()
        {
            if (Card == null) return false;

            // Hand Quick Effect: Pitch 1 Elfnote -> SS from hand
            if (Card.Location == CardLocation.Hand)
            {
                bool hasPitch = Bot.Hand.Concat(Bot.GetMonsters()).Concat(Bot.GetSpells()).Any(c => c != null && c != Card && IsElfnote(c));
                return hasPitch && Bot.GetMonsterCount() < 5;
            }

            // In Center Zone: SS 1 Elfnote from Deck!
            if (Card.Location == CardLocation.MonsterZone && Card.Sequence == 2)
            {
                if (!_reginaSummonUsed)
                {
                    _reginaSummonUsed = true;
                    return true;
                }
            }

            // GY trigger when used as Synchro Material: Return Regina to hand!
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }

            return false;
        }

        private bool TiniaSpSummon()
        {
            return IsCenterZoneFree();
        }

        private bool TiniaSummon()
        {
            return Bot.GetMonsterCount() < 5;
        }

        private bool TiniaActivate()
        {
            if (Card == null) return false;

            // Main Phase Ignition: Place Welcome Home from Deck
            if (Duel.IsMainPhase() && Duel.Player == 0 && Card.Sequence == 2)
            {
                if (!_tiniaPlacedUsed)
                {
                    _tiniaPlacedUsed = true;
                    return true;
                }
            }

            // Opponent turn Quick Effect: switch with center -> banish 1 random card from enemy hand
            if (Duel.Player == 1 && Enemy.Hand.Count > 0)
            {
                return true;
            }

            return false;
        }

        private bool FortunaSpSummon()
        {
            return IsCenterZoneFree();
        }

        private bool FortunaSummon()
        {
            return Bot.GetMonsterCount() < 5;
        }

        private bool FortunaActivate()
        {
            if (Card == null) return false;

            // Main Phase Ignition: Place Continuous Trap from Deck
            if (Duel.IsMainPhase() && Duel.Player == 0 && Card.Sequence == 2)
            {
                if (!_fortunaPlacedUsed)
                {
                    _fortunaPlacedUsed = true;
                    return true;
                }
            }

            // Opponent turn Quick Effect: switch with center -> return 1 face-up S/T to hand
            if (Duel.Player == 1)
            {
                return Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && IsTargetable(c));
            }

            return false;
        }

        private bool PowerPatronSummon()
        {
            return Bot.GetMonsterCount() < 5;
        }

        private bool PowerPatronActivate()
        {
            if (Card == null) return false;

            // Target monster in Center Zone -> increase Level by 3 -> immediately Synchro Summon June Pride or Junora!
            if (Card.Location == CardLocation.MonsterZone)
            {
                var centerMonster = GetCenterMonster();
                if (centerMonster != null && centerMonster.IsFaceup() && centerMonster != Card && !_powerPatronModulatedUsed)
                {
                    _powerPatronModulatedUsed = true;
                    return true;
                }
            }

            // GY trigger on Synchro material: Search ANY Elfnote card from Deck!
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }

            return false;
        }

        private bool StrelitziaActivate()
        {
            if (Card == null) return false;

            // Quick Effect during Main Phase: SS 1 Lv 6 or lower Elfnote from hand or GY + reduce levels by 3
            if (Duel.IsMainPhase())
            {
                bool hasTarget = Bot.Hand.Concat(Bot.Graveyard).Any(c => c != null && c.Level > 0 && c.Level <= 6 && IsElfnote(c));
                return hasTarget && Bot.GetMonsterCount() < 5;
            }

            return false;
        }

        private bool BlackRoseActivate()
        {
            // Only nuke if going 2nd and enemy has multiple threats
            return _isGoingSecond && Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 2;
        }

        private bool PurificationActivate()
        {
            // Link 2: Search Theorealize if Power Patron in GY
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        // SYNCHRO & LINK SUMMON LOGIC
        // ═══════════════════════════════════════════════════════════════

        private bool SynchroJunePride()
        {
            // Don't summon duplicate if already in Center Zone
            var center = GetCenterMonster();
            if (center != null && center.IsCode(CardId.ElfnoteJunePride)) return false;
            return true;
        }

        private bool SynchroJunora()
        {
            return true;
        }

        private bool SynchroReturnZero()
        {
            return true;
        }

        private bool SynchroChaosAngel()
        {
            return true;
        }

        private bool SynchroCrystalWing()
        {
            return true;
        }

        private bool SynchroVictimSanctuary()
        {
            return true;
        }

        private bool SynchroAccelStardust()
        {
            return true;
        }

        private bool SynchroStrelitzia()
        {
            return true;
        }

        private bool SynchroDawnDragster()
        {
            return true;
        }

        private bool SynchroBlackRose()
        {
            return _isGoingSecond && Enemy.GetMonsterCount() >= 2;
        }

        private bool LinkPurification()
        {
            return Bot.GetMonsterCount() >= 2 && !HasAceOnBoard();
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
                CardId.ElfnotesRhapsodiaOfMadness,
                CardId.ElfnotesAristeiaOfTrust
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
                // Link Monsters cannot be in Defense
                if (cardData.HasType(CardType.Link))
                    return CardPosition.FaceUpAttack;

                // 1. Handtraps (Ash 0/1800, etc.) or 0 ATK -> 100% Defense
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
                // Check if this is Terminus or Harmonia sending from Extra Deck
                if (Card != null && (Card.IsCode(CardId.UnleashedPowerPatronPortalTerminus) || Card.IsCode(CardId.FidraulisHarmonia)))
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
