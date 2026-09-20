// ============================================================================
// CARD AUDIT — Anime_Sayer (Sayer's Ultimate Psychic Synchro Battlebox)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Ghost Ogre & Snow Rabbit           | Monster L3 T | Yes  | Yes   | Send GY | Handtrap: destroy face-up card activating eff | Opponent activates card/eff on field         | Bot controls no hand or target is immune    |
// | Mind Over Matter                   | Trap Counter | No   | No    | Tribute | Negate Normal/Sp Summon or S/T activation     | Opponent summons or activates S/T            | No Psychic monster on field to tribute       |
// | Thought Ruler Archfiend            | Synchro L8   | No   | No    | 1000 LP | Quick: Negate S/T targeting 1 Psychic; LP gain| Opponent targets Psychic with S/T; on kill   | LP < 1000                                   |
// | PSY-Framelord Omega                | Synchro L8   | Yes  | No    | None    | Quick: Banish self + 1 random opp card in hand| Opponent turn or during Main Phase           | Opponent has 0 cards in hand                |
// | Psychic Blaster Mk-II              | Synchro L9   | Yes  | Yes   | Banish  | Quick: Banish 1 monster from GY & field; heal | Main Phase, enemy monster on field to banish | No monster in GY or enemy field empty       |
// | Psychic End Punisher               | Synchro L11  | Yes  | No    | 1000 LP | Immune if LP<=opp; banish 1 mine + 1 opp card | Enemy controls card to banish; Battle Phase  | LP < 1000 or enemy field empty              |
// | Hyper Psychic Riser                | Synchro L6   | No   | No    | None    | Floodgate: monsters with ATK>2000 cannot eff  | On field, locks big threats from activating  | Bot needs to activate higher ATK monster eff|
// | HTS Psyhemuth                      | Synchro L6   | No   | No    | None    | After damage calc: banish both battling mons  | Battling dangerous opponent monster          | Battling direct or negligible enemy         |
// | Mind Castlin                       | Synchro L6   | Yes  | Yes   | None    | Switch control with target enemy monster      | Opponent controls strong face-up monster     | Enemy has 0 face-up monsters                |
// | Overmind Archfiend                 | Synchro L9   | Yes  | No    | None    | Banish 1 Psychic from GY; float all on GY send| Main Phase setup or boss beatdown            | No Psychic in GY                            |
// | Hyper Psychic Blaster              | Synchro L9   | No   | No    | None    | Piercing damage + heal LP difference          | Attacking defense position monster           | Opponent controls no defense monsters       |
// | Psychic Omnibuster                 | Synchro L7   | Yes  | Yes   | Banish  | In GY: banish to destroy 1 enemy S/T          | Enemy controls Spell/Trap                    | Enemy controls 0 Spell/Traps                |
// | Magical Android                    | Synchro L5   | No   | No    | None    | End Phase: heal 600 LP per Psychic controlled | End Phase automatic                          | None                                        |
// | Emergency Teleport                 | Spell Quick  | No   | No    | None    | Special Summon Level 3 or lower Psychic       | Need Tuner/Non-Tuner for Synchro or defense  | Deck has no Level 3 or lower Psychic        |
// | Brain Research Lab                 | Spell Field  | No   | No    | None    | Additional Normal Summon + counter instead LP | Main Phase, need extra Psychic summon        | Already activated this turn                 |
// | Terraforming                       | Spell Normal | Yes  | Yes   | None    | Search Brain Research Lab                     | Main Phase, need Field Spell                 | Brain Research Lab already in hand/field    |
// | Psychokinesis                      | Spell Normal | No   | No    | 1000 dmg| Control Psychic: destroy 1 card on field      | Opponent controls dangerous threat           | Bot controls no Psychic or opp board empty  |
// | Brain Control                      | Spell Normal | No   | No    | 800 LP  | Steal 1 opponent face-up normal summonable mon| Opponent has monster; steal for Synchro/push | Opponent has no valid monsters              |
// | Telekinetic Power Well             | Spell Quick  | No   | No    | Damage  | Special Summon any number of L2 Psychics from | Need Synchro material or board presence      | Graveyard has no Level 2 Psychics           |
// | Overdrive Teleporter               | Monster L6   | Yes  | No    | 2000 LP | Normal Summon: SS 2 Level 3 Psychics from Deck| Main Phase after Normal Summon               | Deck has < 2 Level 3 Psychics               |
// | Master Gig                         | Monster L8   | Yes  | No    | 1000 LP | Destroy opponent monsters up to Psychic count | Opponent controls monsters                   | Opponent controls 0 monsters                |
// | Armored Axon Kicker                | Monster L6   | No   | No    | None    | Normal Summon without tribute if control Psych| Need Level 6 body on field                   | Bot controls no Psychic monsters            |
// | Psychic Wheeleder                  | Monster L3 T | Yes  | Yes   | None    | SS if control Level 3; pop monster on Synchro | Control Level 3 monster; sent as Synchro mat | Already Special Summoned this turn          |
// | Psychic Tracker                    | Monster L3   | Yes  | Yes   | None    | SS if control Level 3; +600 ATK to Synchro    | Control Level 3 monster; Synchro extender    | Already Special Summoned this turn          |
// | Hushed Psychic Minister            | Monster L3   | Yes  | Yes   | None    | SS if control Psychic; GY banish search L3-   | Control Psychic monster; in GY for search    | Already used effect this turn               |
// | Serene Psychic Girl                | Monster L2 T | Yes  | Yes   | None    | SS if control Psychic; GY banish recycle GY   | Control Psychic monster; in GY for recycle   | Already used effect this turn               |
// | Krebons                            | Monster L2 T | No   | No    | 800 LP  | Negate attack targeting this card             | Targeted for attack                          | LP < 800                                    |
// | Psychic Commander                  | Monster L3 T | No   | No    | LP cost | Damage Step: reduce enemy monster ATK/DEF     | Battling enemy monster with higher ATK       | Enemy monster already weaker                |
// | Silent Psychic Wizard              | Monster L4   | No   | No    | None    | On NS: banish 1 Psychic from GY; float on send| Normal Summon; sent to GY to revive target   | Graveyard empty                             |
// | Psychic Snail                      | Monster L4   | Yes  | No    | 800 LP  | Give another Psychic monster double attack    | Battle Phase setup with high ATK Psychic     | Alone on field or LP < 800                  |
// | Psychic Jumper                     | Monster L2 T | Yes  | No    | 1000 LP | Switch control of 1 opp monster with 1 mine   | Opponent has high threat to steal            | Bot controls no other Psychic               |
// | Psi-Blocker                        | Monster L4   | Yes  | No    | None    | Declare card name: lock that card for 1 turn  | Main Phase 1: lock key enemy card/handtrap   | None                                        |
// | Mind Procedure                     | Monster L3 T | Yes  | Yes   | None    | Reveal top 5: add 1 Psychic monster to hand   | Main Phase search                            | Already used this turn                      |
// | Psychic Overload                   | Trap Normal  | No   | No    | Shuffle | Shuffle 3 Psychics from GY to deck, draw 2    | Graveyard has 3+ Psychic monsters            | Graveyard has < 3 Psychic monsters          |
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("Anime_Sayer", "Anime_Sayer")]
    public class Anime_SayerExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int MasterGig = 16191953;
            public const int OverdriveTeleporter = 1834753;
            public const int ArmoredAxonKicker = 62742651;
            public const int SilentPsychicWizard = 62950604;
            public const int PsychicSnail = 58453942;
            public const int PsiBlocker = 29417188;
            public const int PsychicCommander = 21454943;
            public const int PsychicWheeleder = 3233859;
            public const int PsychicTracker = 30227494;
            public const int HushedPsychicMinister = 36218106;
            public const int MindProcedure = 62606805;
            public const int GhostOgreAndSnowRabbit = 59438930;
            public const int Krebons = 59575539;
            public const int SerenePsychicGirl = 9213491;
            public const int PsychicJumper = 52430902;

            // Spells
            public const int EmergencyTeleport = 67723438;
            public const int BrainResearchLab = 85668449;
            public const int Terraforming = 73628505;
            public const int Psychokinesis = 32180819;
            public const int BrainControl = 87910978;
            public const int TelekineticPowerWell = 28741524;
            public const int TelekineticChargingCell = 68392533; // Fallback
            public const int PsychicSword = 92346415; // Fallback

            // Traps
            public const int MindOverMatter = 59718521;
            public const int PsychicOverload = 82633308;

            // Extra Deck
            public const int PsychicEndPunisher = 60465049;
            public const int OvermindArchfiend = 24221808;
            public const int PsychicBlasterMkII = 88139289;
            public const int HyperPsychicBlaster = 95526884;
            public const int PSYFramelordOmega = 74586817;
            public const int ThoughtRulerArchfiend = 70780151;
            public const int PsychicOmnibuster = 70659412;
            public const int PsychicLifetrancer = 45379225;
            public const int SerenePsychicSorceress = 5848934;
            public const int HTSPsyhemuth = 15028680;
            public const int HyperPsychicRiser = 99115354;
            public const int PsychicNightmare = 7582066;
            public const int MindCastlin = 12172567;
            public const int MagicalAndroid = 43385557;
        }

        // _normalSummonUsedThisTurn

        public Anime_SayerExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            RegisterExecutors();
        }

        private void RegisterExecutors()
        {
            // -------------------------------------------------------------
            // 1. High-Priority Handtraps & Quick Disruption (Enemy / Chain)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.GhostOgreAndSnowRabbit, GhostOgreActivate);
            AddExecutor(ExecutorType.Activate, CardId.MindOverMatter, MindOverMatterActivate);
            AddExecutor(ExecutorType.Activate, CardId.ThoughtRulerArchfiend, ThoughtRulerProtectActivate);
            AddExecutor(ExecutorType.Activate, CardId.PSYFramelordOmega, PSYFramelordOmegaQuickActivate);
            AddExecutor(ExecutorType.Activate, CardId.PsychicBlasterMkII, PsychicBlasterMkIIQuickActivate);
            AddExecutor(ExecutorType.Activate, CardId.PsychicEndPunisher, PsychicEndPunisherActivate);
            AddExecutor(ExecutorType.Activate, CardId.Krebons, KrebonsNegateAttack);
            AddExecutor(ExecutorType.Activate, CardId.PsychicCommander, PsychicCommanderCombatActivate);

            // -------------------------------------------------------------
            // 2. Board Breakers, Field Setup & Search Spells
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.Terraforming, TerraformingActivate);
            AddExecutor(ExecutorType.Activate, CardId.BrainResearchLab, BrainResearchLabActivate);
            AddExecutor(ExecutorType.Activate, CardId.BrainControl, BrainControlActivate);
            AddExecutor(ExecutorType.Activate, CardId.Psychokinesis, PsychokinesisActivate);
            AddExecutor(ExecutorType.Activate, CardId.MasterGig, MasterGigActivate);
            AddExecutor(ExecutorType.Activate, CardId.PsychicOmnibuster, PsychicOmnibusterGraveActivate);
            AddExecutor(ExecutorType.Activate, CardId.PsychicOverload, PsychicOverloadActivate);
            AddExecutor(ExecutorType.Activate, CardId.TelekineticPowerWell, TelekineticPowerWellActivate);

            // -------------------------------------------------------------
            // 3. Quick-Play Starter: Emergency Teleport
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.EmergencyTeleport, EmergencyTeleportActivate);

            // -------------------------------------------------------------
            // 4. Graveyard / Floating Trigger Effects
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.SilentPsychicWizard, SilentPsychicWizardEffect);
            AddExecutor(ExecutorType.Activate, CardId.PsychicWheeleder, PsychicWheelederGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.HushedPsychicMinister, HushedPsychicMinisterGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.SerenePsychicGirl, SerenePsychicGirlGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.SerenePsychicSorceress, SerenePsychicSorceressEffect);
            AddExecutor(ExecutorType.Activate, CardId.OvermindArchfiend, OvermindArchfiendEffect);
            AddExecutor(ExecutorType.Activate, CardId.HyperPsychicRiser, HyperPsychicRiserEffect);
            AddExecutor(ExecutorType.Activate, CardId.MindCastlin, MindCastlinEffect);

            // -------------------------------------------------------------
            // 5. Special Summon Extenders from Hand
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.SpSummon, CardId.PsychicWheeleder, PsychicWheelederSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.PsychicTracker, PsychicTrackerSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.HushedPsychicMinister, HushedPsychicMinisterSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SerenePsychicGirl, SerenePsychicGirlSpSummon);

            // -------------------------------------------------------------
            // 6. Normal Summons & Ignition Starters
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Summon, CardId.ArmoredAxonKicker, ArmoredAxonKickerSummon);
            AddExecutor(ExecutorType.Summon, CardId.OverdriveTeleporter, OverdriveTeleporterSummon);
            AddExecutor(ExecutorType.Activate, CardId.OverdriveTeleporter, OverdriveTeleporterActivate);
            AddExecutor(ExecutorType.Summon, CardId.SilentPsychicWizard, SilentPsychicWizardSummon);
            AddExecutor(ExecutorType.Summon, CardId.PsiBlocker, PsiBlockerSummon);
            AddExecutor(ExecutorType.Activate, CardId.PsiBlocker, PsiBlockerActivate);
            // Non-Tuners prioritized to pair with field Tuners
            AddExecutor(ExecutorType.Summon, CardId.PsychicTracker, PsychicTrackerSummon);
            AddExecutor(ExecutorType.Summon, CardId.HushedPsychicMinister, HushedPsychicMinisterSummon);
            AddExecutor(ExecutorType.Summon, CardId.PsychicSnail, PsychicSnailSummon);
            AddExecutor(ExecutorType.Activate, CardId.PsychicSnail, PsychicSnailActivate);
            // Tuners with anti-spam check
            AddExecutor(ExecutorType.Summon, CardId.PsychicWheeleder, PsychicWheelederSummon);
            AddExecutor(ExecutorType.Summon, CardId.PsychicCommander, PsychicCommanderSummon);
            AddExecutor(ExecutorType.Summon, CardId.MindProcedure, MindProcedureSummon);
            AddExecutor(ExecutorType.Activate, CardId.MindProcedure, MindProcedureActivate);
            AddExecutor(ExecutorType.Summon, CardId.SerenePsychicGirl, SerenePsychicGirlSummon);
            AddExecutor(ExecutorType.Summon, CardId.Krebons, KrebonsSummon);
            AddExecutor(ExecutorType.Summon, CardId.PsychicJumper, PsychicJumperSummon);
            AddExecutor(ExecutorType.Activate, CardId.PsychicJumper, PsychicJumperActivate);
            AddExecutor(ExecutorType.Summon, CardId.MasterGig, MasterGigSummon);

            // -------------------------------------------------------------
            // 7. Extra Deck Synchro Summons (Tiered Priority)
            // -------------------------------------------------------------
            // Finisher Tier: Level 11 OTK Boss
            AddExecutor(ExecutorType.SpSummon, CardId.PsychicEndPunisher, PsychicEndPunisherSummon);

            // Level 9 Boss Tier
            AddExecutor(ExecutorType.SpSummon, CardId.PsychicBlasterMkII, PsychicBlasterMkIISummon);
            AddExecutor(ExecutorType.SpSummon, CardId.OvermindArchfiend, OvermindArchfiendSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.HyperPsychicBlaster, HyperPsychicBlasterSummon);

            // Level 8 Core Sayer Boss Tier
            AddExecutor(ExecutorType.SpSummon, CardId.ThoughtRulerArchfiend, ThoughtRulerArchfiendSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.PSYFramelordOmega, PSYFramelordOmegaSummon);

            // Level 7 Control Tier
            AddExecutor(ExecutorType.SpSummon, CardId.PsychicOmnibuster, PsychicOmnibusterSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.PsychicLifetrancer, PsychicLifetrancerSummon);
            AddExecutor(ExecutorType.Activate, CardId.PsychicLifetrancer, PsychicLifetrancerActivate);

            // Level 6 Bridge / Disruption Tier
            AddExecutor(ExecutorType.SpSummon, CardId.HyperPsychicRiser, HyperPsychicRiserSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.HTSPsyhemuth, HTSPsyhemuthSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.MindCastlin, MindCastlinSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SerenePsychicSorceress, SerenePsychicSorceressSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.PsychicNightmare, PsychicNightmareSummon);
            AddExecutor(ExecutorType.Activate, CardId.PsychicNightmare, PsychicNightmareActivate);

            // Level 5 Sustain Tier
            AddExecutor(ExecutorType.SpSummon, CardId.MagicalAndroid, MagicalAndroidSummon);

            // -------------------------------------------------------------
            // 8. End Phase / Battle Phase Management
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.SpellSet, SpellSetStrategy);
            AddExecutor(ExecutorType.Repos, RepositionStrategy);
        }

        public override void OnNewTurn()
        {
            
            base.OnNewTurn();
        }

        // =====================================================================
        // Handtraps & Quick Disruption Callbacks
        // =====================================================================
        private bool GhostOgreActivate()
        {
            // Handtrap check: activate from hand when opponent card on field activates
            if (Card.Location != CardLocation.Hand && Card.Location != CardLocation.MonsterZone)
                return false;

            if (Duel.LastChainPlayer == 1)
            {
                ClientCard lastCard = LastChainCard;
                if (lastCard != null && lastCard.Location == CardLocation.MonsterZone || lastCard?.Location == CardLocation.SpellZone)
                {
                    return true;
                }
            }
            return false;
        }

        private bool MindOverMatterActivate()
        {
            if (Duel.LastChainPlayer != 1) return false;

            // Tribute fodder: Krebons, Girl, Token, or non-boss
            ClientCard tribute = Bot.GetMonsters().FirstOrDefault(m => m.Race == (int)CardRace.Psycho &&
                m.Id != CardId.PsychicEndPunisher &&
                m.Id != CardId.ThoughtRulerArchfiend &&
                m.Id != CardId.PSYFramelordOmega);

            if (tribute == null) tribute = Bot.GetMonsters().FirstOrDefault(m => m.Race == (int)CardRace.Psycho);

            if (tribute != null)
            {
                AI.SelectCard(tribute);
                return true;
            }
            return false;
        }

        private bool ThoughtRulerProtectActivate()
        {
            // Negate Spell/Trap targeting 1 Psychic monster
            if (Duel.LastChainPlayer == 1 && Bot.LifePoints > 1000)
            {
                return true;
            }
            return false;
        }

        private bool PSYFramelordOmegaQuickActivate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Banish self + 1 random card from opponent hand
            if (Enemy.Hand.Count > 0)
            {
                // In enemy turn, or in MP2 of our turn before passing
                if (Duel.Player == 1 || Duel.Phase == DuelPhase.Main2)
                {
                    return true;
                }
            }
            return false;
        }

        private bool PsychicBlasterMkIIQuickActivate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Quick effect during Main Phase: Banish 1 monster from GY, banish 1 face-up monster on field
            if (Bot.Graveyard.Any(c => c.IsMonster()) && Enemy.GetMonsters().Any(m => m.IsFaceup()))
            {
                ClientCard gyTarget = Bot.Graveyard.FirstOrDefault(c => c.IsMonster() && c.Id != CardId.PsychicEndPunisher);
                ClientCard fieldTarget = Enemy.GetMonsters().Where(m => m.IsFaceup()).OrderByDescending(m => m.Attack).FirstOrDefault();
                if (gyTarget != null && fieldTarget != null)
                {
                    AI.SelectCard(gyTarget);
                    AI.SelectNextCard(fieldTarget);
                    return true;
                }
            }
            return false;
        }

        private bool PsychicEndPunisherActivate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Effect 1: Banish 1 monster you control and 1 card opponent controls (Pay 1000 LP)
            if (Bot.LifePoints > 1000 && Enemy.GetMonsterCount() + Enemy.GetSpellCount() > 0)
            {
                ClientCard myFodder = Bot.GetMonsters().FirstOrDefault(m => m != Card) ?? Card;
                ClientCard oppTarget = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault();
                if (oppTarget == null) oppTarget = Enemy.GetSpells().FirstOrDefault();

                if (myFodder != null && oppTarget != null)
                {
                    AI.SelectCard(myFodder);
                    AI.SelectNextCard(oppTarget);
                    return true;
                }
            }

            // Effect 2: Gain ATK at start of Battle Phase
            if (Duel.Phase == DuelPhase.BattleStart)
            {
                return true;
            }

            return false;
        }

        private bool KrebonsNegateAttack()
        {
            // Negate attack targeting Krebons if LP > 800
            return Bot.LifePoints > 800;
        }

        private bool PsychicCommanderCombatActivate()
        {
            // Reduce opponent monster ATK/DEF by up to 500
            if (Duel.Phase == DuelPhase.Damage)
            {
                ClientCard battling = Enemy.BattlingMonster;
                if (battling != null && battling.Attack >= Card.Attack && Bot.LifePoints > 500)
                {
                    return true;
                }
            }
            return false;
        }

        // =====================================================================
        // Spells & Board Breakers Callbacks
        // =====================================================================
        private bool TerraformingActivate()
        {
            if (!Bot.HasInHand(CardId.BrainResearchLab) && !Bot.HasInSpellZone(CardId.BrainResearchLab))
            {
                AI.SelectCard(CardId.BrainResearchLab);
                return true;
            }
            return false;
        }

        private bool BrainResearchLabActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return !Bot.HasInSpellZone(CardId.BrainResearchLab);
            }
            return false;
        }

        private bool BrainControlActivate()
        {
            if (Bot.LifePoints <= 800) return false;
            var validTargets = Enemy.GetMonsters().Where(m => m.IsFaceup() && !m.IsShouldNotBeSpellTrapTarget()).OrderByDescending(m => m.Attack).ToList();
            if (validTargets.Count == 0) return false;

            int botTunerCount = Bot.GetMonsters().Count(m => m.IsFaceup() && m.IsTuner());
            int botNonTunerCount = Bot.GetMonsters().Count(m => m.IsFaceup() && !m.IsTuner());
            bool hasNonTunerInHand = Bot.Hand.Any(c => c.IsMonster() && !c.IsTuner());
            bool hasTunerInHand = Bot.Hand.Any(c => c.IsMonster() && c.IsTuner());

            foreach (var target in validTargets)
            {
                // If target is a Tuner: we MUST be able to pair it with a Non-Tuner
                if (target.IsTuner())
                {
                    // If we have no Non-Tuner on field and no Non-Tuner in hand to summon, and target cannot deal lethal, skip!
                    if (botNonTunerCount == 0 && !hasNonTunerInHand && target.Attack < 2000)
                        continue;
                }
                else
                {
                    // Target is Non-Tuner: we should have a Tuner on field or in hand to tune with it, or high ATK
                    if (botTunerCount == 0 && !hasTunerInHand && target.Attack < 2000)
                        continue;
                }

                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool PsychokinesisActivate()
        {
            if (Bot.GetMonsters().Any(m => m.Race == (int)CardRace.Psycho))
            {
                ClientCard target = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault();
                if (target == null) target = Enemy.GetSpells().FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool MasterGigActivate()
        {
            if (Bot.LifePoints > 1000 && Enemy.GetMonsterCount() > 0)
            {
                return true;
            }
            return false;
        }

        private bool PsychicOmnibusterGraveActivate()
        {
            if (Card.Location == CardLocation.Grave)
            {
                ClientCard target = Enemy.GetSpells().FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool PsychicOverloadActivate()
        {
            int psychicInGrave = Bot.Graveyard.Count(c => c.Race == (int)CardRace.Psycho && c.IsMonster());
            if (psychicInGrave >= 3)
            {
                var targets = Bot.Graveyard.Where(c => c.Race == (int)CardRace.Psycho && c.IsMonster()).Take(3).ToList();
                AI.SelectCard(targets);
                return true;
            }
            return false;
        }

        private bool TelekineticPowerWellActivate()
        {
            // Revive Level 2 Psychics from Graveyard (Krebons, Serene Psychic Girl, Psychic Jumper)
            if (Bot.Graveyard.Any(c => c.Race == (int)CardRace.Psycho && c.Level <= 2 && c.IsMonster()))
            {
                var targets = Bot.Graveyard.Where(c => c.Race == (int)CardRace.Psycho && c.Level <= 2 && c.IsMonster()).Take(2).ToList();
                AI.SelectCard(targets);
                return true;
            }
            return false;
        }

        private bool EmergencyTeleportActivate()
        {
            // Summon Level 3 or lower Psychic from Hand/Deck
            // Priority: Tuner if we have non-Tuner, Non-Tuner if we have Tuner
            bool hasTuner = Bot.GetMonsters().Any(m => m.IsTuner());
            bool hasNonTuner = Bot.GetMonsters().Any(m => !m.IsTuner());

            if (!hasTuner)
            {
                AI.SelectCard(new[] {
                    CardId.PsychicWheeleder,
                    CardId.GhostOgreAndSnowRabbit,
                    CardId.Krebons,
                    CardId.PsychicCommander,
                    CardId.SerenePsychicGirl
                });
            }
            else
            {
                AI.SelectCard(new[] {
                    CardId.PsychicTracker,
                    CardId.HushedPsychicMinister,
                    CardId.Krebons
                });
            }
            return true;
        }

        // =====================================================================
        // Graveyard & Floating Callbacks
        // =====================================================================
        private bool SilentPsychicWizardEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // On NS: Banish 1 Psychic from GY
                ClientCard target = Bot.Graveyard.FirstOrDefault(c => c.Race == (int)CardRace.Psycho && c.IsMonster());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // When sent from field to GY: SS the banished target
                return true;
            }
            return false;
        }

        private bool PsychicWheelederGraveEffect()
        {
            // On sent as Synchro material: Pop monster with less ATK than the Synchro
            ClientCard target = Enemy.GetMonsters().Where(m => m.IsFaceup()).OrderByDescending(m => m.Attack).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool HushedPsychicMinisterGraveEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // Banish to search Level 3 or lower Psychic
                AI.SelectCard(new[] {
                    CardId.PsychicWheeleder,
                    CardId.PsychicTracker,
                    CardId.GhostOgreAndSnowRabbit,
                    CardId.PsychicCommander,
                    CardId.Krebons
                });
                return true;
            }
            return false;
        }

        private bool SerenePsychicGirlGraveEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // Banish from GY: Add 1 Psychic from GY to hand
                ClientCard target = Bot.Graveyard.FirstOrDefault(c => c.Race == (int)CardRace.Psycho && c.IsMonster() && c != Card);
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool SerenePsychicSorceressEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // Banish from GY to revive 1 Psychic from GY
                ClientCard target = Bot.Graveyard.FirstOrDefault(c => c.Race == (int)CardRace.Psycho && c.IsMonster() && c != Card);
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool OvermindArchfiendEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Banish 1 Psychic from GY
                ClientCard target = Bot.Graveyard.FirstOrDefault(c => c.Race == (int)CardRace.Psycho && c.IsMonster());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // Float when sent to GY
                return true;
            }
            return false;
        }

        private bool HyperPsychicRiserEffect()
        {
            // Destroyed by opponent: recover 1 tuner + 1 non-tuner
            return true;
        }

        private bool MindCastlinEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Switch control of this card and 1 opponent face-up monster
                ClientCard target = Enemy.GetMonsters().Where(m => m.IsFaceup()).OrderByDescending(m => m.Attack).FirstOrDefault();
                if (target != null && target.Attack > Card.Attack)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        // =====================================================================
        // Special Summon Extenders
        // =====================================================================
        private bool PsychicWheelederSpSummon()
        {
            // SS if control Level 3 monster other than Psychic Wheeleder
            return Bot.GetMonsters().Any(m => m.Level == 3 && m.Id != CardId.PsychicWheeleder);
        }

        private bool PsychicTrackerSpSummon()
        {
            // SS if control Level 3 monster other than Psychic Tracker
            return Bot.GetMonsters().Any(m => m.Level == 3 && m.Id != CardId.PsychicTracker);
        }

        private bool HushedPsychicMinisterSpSummon()
        {
            // SS if control any Psychic monster
            return Bot.GetMonsters().Any(m => m.Race == (int)CardRace.Psycho);
        }

        private bool SerenePsychicGirlSpSummon()
        {
            // SS if control any Psychic monster
            return Bot.GetMonsters().Any(m => m.Race == (int)CardRace.Psycho);
        }

        // =====================================================================
        // Normal Summons & Starters
        // =====================================================================
        private bool ArmoredAxonKickerSummon()
        {
            // Normal summon without tribute if we control face-up Psychic
            return Bot.GetMonsters().Any(m => m.IsFaceup() && m.Race == (int)CardRace.Psycho);
        }

        private bool OverdriveTeleporterSummon()
        {
            // Needs 1 tribute: Ensure we have fodder on field! (Strict Anti-Pattern 3)
            if (Bot.GetMonsterCount() < 1) return false;

            // Don't tribute boss
            ClientCard fodder = Bot.GetMonsters().FirstOrDefault(m => m.Level <= 4 && !m.HasType(CardType.Synchro));
            if (fodder != null)
            {
                AI.SelectCard(fodder);
                
                return true;
            }
            return false;
        }

        private bool OverdriveTeleporterActivate()
        {
            // Pay 2000 LP to SS 2 Level 3 Psychics from Deck!
            // Need at least 1 Tuner + 1 Non-Tuner
            AI.SelectCard(new[] {
                CardId.PsychicWheeleder,
                CardId.PsychicTracker,
                CardId.PsychicCommander,
                CardId.HushedPsychicMinister
            });
            return true;
        }

        private bool SilentPsychicWizardSummon()
        {
            
            return true;
        }

        private bool PsiBlockerSummon()
        {
            
            return true;
        }

        private bool PsiBlockerActivate()
        {
            // Lock key opponent card: Ash Blossom or prominent opponent card
            AI.SelectAnnounceID(14558127); // Ash Blossom
            return true;
        }

        private bool CanNormalSummonTuner(int level)
        {
            int tunerCount = Bot.GetMonsters().Count(m => m.IsFaceup() && m.IsTuner());
            int nonTunerCount = Bot.GetMonsters().Count(m => m.IsFaceup() && !m.IsTuner());

            // 1. If board is empty, summoning a starter Tuner is fine
            if (Bot.GetMonsterCount() == 0) return true;

            // 2. If we already have a Tuner on field and NO Non-Tuners:
            if (tunerCount >= 1 && nonTunerCount == 0)
            {
                // Check if we have a Non-Tuner special summonable from hand (Tracker or Hushed Minister)
                bool hasSpecialNonTunerInHand = Bot.Hand.Any(c => c.Id == CardId.PsychicTracker || c.Id == CardId.HushedPsychicMinister);
                if (!hasSpecialNonTunerInHand)
                {
                    // DO NOT summon another Tuner to be stuck on field!
                    return false;
                }
            }

            // 3. If we already have 2 or more Tuners, strictly refuse another Tuner
            if (tunerCount >= 2) return false;

            return true;
        }

        private bool PsychicWheelederSummon()
        {
            return CanNormalSummonTuner(3);
        }

        private bool PsychicCommanderSummon()
        {
            return CanNormalSummonTuner(3);
        }

        private bool KrebonsSummon()
        {
            return CanNormalSummonTuner(2);
        }

        private bool MindProcedureSummon()
        {
            return CanNormalSummonTuner(3);
        }

        private bool MindProcedureActivate()
        {
            AI.SelectCard(new[] {
                CardId.OverdriveTeleporter,
                CardId.PsychicWheeleder,
                CardId.PsychicTracker,
                CardId.GhostOgreAndSnowRabbit,
                CardId.SilentPsychicWizard
            });
            return true;
        }

        private bool PsychicTrackerSummon()
        {
            return true;
        }

        private bool HushedPsychicMinisterSummon()
        {
            return true;
        }

        private bool SerenePsychicGirlSummon()
        {
            return CanNormalSummonTuner(2);
        }

        private bool PsychicSnailSummon()
        {
            return true;
        }

        private bool PsychicSnailActivate()
        {
            // Give double attack to highest ATK Psychic monster (other than Snail)
            ClientCard target = Bot.GetMonsters().Where(m => m != Card && m.Race == (int)CardRace.Psycho).OrderByDescending(m => m.Attack).FirstOrDefault();
            if (target != null && target.Attack >= 2400)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool PsychicJumperSummon()
        {
            return CanNormalSummonTuner(2);
        }

        private bool PsychicJumperActivate()
        {
            if (Bot.LifePoints <= 1000) return false;
            ClientCard oppMonster = Enemy.GetMonsters().Where(m => m.IsFaceup()).OrderByDescending(m => m.Attack).FirstOrDefault();
            ClientCard myFodder = Bot.GetMonsters().FirstOrDefault(m => m != Card && m.Level <= 3 && !m.HasType(CardType.Synchro));

            if (oppMonster != null && myFodder != null && oppMonster.Attack > myFodder.Attack)
            {
                AI.SelectCard(oppMonster);
                AI.SelectNextCard(myFodder);
                return true;
            }
            return false;
        }

        private bool MasterGigSummon()
        {
            // Needs 2 tributes: strictly check Bot.GetMonsterCount() >= 2
            if (Bot.GetMonsterCount() < 2) return false;

            var fodders = Bot.GetMonsters().Where(m => m.Level <= 4 && !m.HasType(CardType.Synchro)).Take(2).ToList();
            if (fodders.Count == 2)
            {
                AI.SelectCard(fodders);
                
                return true;
            }
            return false;
        }

        // =====================================================================
        // Extra Deck Synchro Summons Callbacks
        // =====================================================================
        private bool PsychicEndPunisherSummon()
        {
            // Level 11 Finisher OTK
            return true;
        }

        private bool PsychicBlasterMkIISummon()
        {
            // Level 9 Quick-banish boss
            return true;
        }

        private bool OvermindArchfiendSummon()
        {
            return true;
        }

        private bool HyperPsychicBlasterSummon()
        {
            return true;
        }

        private bool ThoughtRulerArchfiendSummon()
        {
            // Core Sayer Ace (Level 8)
            return true;
        }

        private bool PSYFramelordOmegaSummon()
        {
            // Hand rip & banish recycle (Level 8)
            return true;
        }

        private bool PsychicOmnibusterSummon()
        {
            return true;
        }

        private bool PsychicLifetrancerSummon()
        {
            return true;
        }

        private bool PsychicLifetrancerActivate()
        {
            // Banish 1 Psychic from GY to gain 1200 LP
            ClientCard target = Bot.Graveyard.FirstOrDefault(c => c.Race == (int)CardRace.Psycho && c.IsMonster());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool HyperPsychicRiserSummon()
        {
            // Level 6 Floodgate
            return true;
        }

        private bool HTSPsyhemuthSummon()
        {
            // Level 6 Combat Banish
            return true;
        }

        private bool MindCastlinSummon()
        {
            // Level 6 Control Swap
            return Enemy.GetMonsters().Any(m => m.IsFaceup() && m.Attack > 1500);
        }

        private bool SerenePsychicSorceressSummon()
        {
            return true;
        }

        private bool PsychicNightmareSummon()
        {
            return true;
        }

        private bool PsychicNightmareActivate()
        {
            // Guess card type in opponent hand: usually Monster
            AI.SelectOption(0);
            return true;
        }

        private bool MagicalAndroidSummon()
        {
            // Level 5 Sustain
            return true;
        }

        // =====================================================================
        // Spells & Reposition Strategy
        // =====================================================================
        private bool SpellSetStrategy()
        {
            // Strictly NO handtraps in MP1 (Strict Anti-Pattern 5)
            if (Card.Id == CardId.GhostOgreAndSnowRabbit) return false;

            if (Card.IsTrap()) return true;
            if (Card.IsSpell() && Card.HasType(CardType.QuickPlay))
            {
                // Quick-play spells set only in Main Phase 2
                return Duel.Phase == DuelPhase.Main2;
            }
            return false;
        }

        private bool RepositionStrategy()
        {
            // Switch low ATK attack monsters to Defense
            if (Card.Attack < 1500 && Card.IsAttack())
            {
                return true;
            }
            // Switch high ATK defense monsters to Attack
            if (Card.Attack >= 2000 && Card.IsDefense())
            {
                return true;
            }
            return false;
        }

        // =====================================================================
        // Strict Anti-Pattern & Safe Selection Overrides
        // =====================================================================
        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // 🚫 Rule 1: Hint 502 (Destroy), 503/504 (Remove/Banish), 508 (ToGrave removal):
            // MUST target ENEMY cards (Controller == 1) if available! Never target our own cards!
            if (hint == 502 || hint == 503 || hint == 504)
            {
                var enemyCards = cards.Where(c => c.Controller == 1).ToList();
                if (enemyCards.Count >= min)
                {
                    // Pick the highest threat enemy card
                    return enemyCards.OrderByDescending(c => c.Attack).Take(max).ToList();
                }
            }

            // 🚫 Rule 4: Hint 506 (Return to deck) / Tribute cost (Hint 500): Never return/tribute Ace
            if (hint == 500) // Release / Tribute
            {
                var nonAce = cards.Where(c => c.Id != CardId.PsychicEndPunisher &&
                                              c.Id != CardId.ThoughtRulerArchfiend &&
                                              c.Id != CardId.PSYFramelordOmega).ToList();
                if (nonAce.Count >= min)
                {
                    return nonAce.OrderBy(c => c.Attack).Take(min).ToList();
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
