// ============================================================================
// CARD AUDIT — ADML (Azamina Dark Magician Light and Darkness Ritual)
// ============================================================================
// | Card Name                                    | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |----------------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Red-Eyes Dark Dragoon                        | Fusion L8    | Yes  | No    | Discard | Quick Omni-Negate + pop/burn; untarget/indestr| Quick negate opp card; MP1 pop monster       | Already negated this turn                   |
// | Azamina Ilia Silvia                          | Fusion L6    | Yes  | Yes   | Tribute | Quick Omni-Negate by tributing self           | Quick negate opp card / high threat          | Already negated this turn                   |
// | Illusion of Chaos                            | Ritual L7    | Yes  | Yes   | Bounce  | Hand: search Souls/Rod; Field: Quick Negate   | Hand: search starter; Field: negate monster  | Hand: already searched; Field: no DM in GY  |
// | Black Chaos                                  | Effect L8 SS | Yes  | Yes   | Discard | Hand: place Mind Shuffle face-up; Banish 2 opp| Hand: place Mind Shuffle; Field: banish 2 opp| Already have Mind Shuffle on field          |
// | Magician of Dark Chaos - Black Chaos         | Ritual L8    | Yes  | Yes   | None    | S/T protect; On SS: add spell; Banish opp f-d | On SS: recur spell; Quick/Ignition: banish opp| Opponent controls 0 cards                   |
// | Black Luster Soldier - Light and Darkness    | Ritual L8    | Yes  | Yes   | None    | Untargetable by non-target; banish opp; 4500  | On SS: banish opp card; Battle: double attack | Opponent controls 0 cards                   |
// | WANTED: Seeker of Sinful Spoils              | Spell Quick  | Yes  | Yes   | None    | Hand: Search Diabellstar; GY: recycle & draw  | Main Phase 1: search Diabellstar / draw 1    | Already used this turn                      |
// | Diabellstar the Black Witch                  | Monster L7   | Yes  | Yes   | Send 1  | Hand SS; On SS: Set Deception from deck       | Main Phase 1: SS self & set Deception        | Already on field                            |
// | Deception of the Sinful Spoils               | Spell Cont   | Yes  | Yes   | Tribute | Search The Hallowed Azamina                   | Main Phase 1: search Azamina spell           | Already used search this turn               |
// | The Hallowed Azamina                         | Spell Normal | Yes  | Yes   | Send SS | Fusion Summon Azamina Silvia / Mu; GY recycle | Main Phase 1: summon Silvia (Omni-Negate)    | No Sinful Spoils card to send               |
// | Mind Shuffle                                 | Trap Cont    | Yes  | Yes   | None    | Search Ritual monster; Tag-out Lv7+ to Boss SS| Opp turn: tag out Lv7+ for MagChaos/BLS      | No valid Lv7+ monster on field              |
// | The Gaze of Timaeus                          | Spell Quick  | Yes  | Yes   | None    | Fusion Summon Dragoon using DM in field/GY    | Main Phase 1: summon Dragoon                 | No DM in field or GY                        |
// | Dark Magical Curtain                         | Spell Quick  | Yes  | Yes   | None    | SS Dark Magician from deck & search Gaze      | Main Phase 1: bring out DM & search Gaze     | Already used this turn                      |
// | Magicians' Souls                             | Monster L1   | Yes  | Yes   | Send Lv6| Dump DM -> SS self; send spells draw up to 2  | Main Phase 1: setup DM in GY & draw          | Already used this turn                      |
// | Magician's Rod                               | Monster L3   | Yes  | Yes   | None    | On NS: search Gaze of Timaeus / Curtain       | Normal Summon starter                        | No targets in deck                          |
// | Ragged Records of Rites                      | Spell Normal | Yes  | Yes   | None    | Reveal Ritual Spell -> search Ritual Monster  | Main Phase 1: search Black Chaos/Griffoh/BLS | Deck has no Ritual Spell/Monster            |
// | Light and Darkness Ritual                    | Spell Ritual | Yes  | Yes   | Tribute | Ritual Summon Magician of Dark Chaos / BLS    | Main Phase 1: Ritual Summon Boss             | No valid tribute in field/hand/GY           |
// | Griffoh                                      | Monster L1   | Yes  | Yes   | Discard | Quick: set Mind Shuffle; Counts as entire Lv8 | Hand: set Mind Shuffle; GY: full tribute     | Already have Mind Shuffle on field          |
// | Skull Archfiend of Chaos                     | Monster L6   | Yes  | Yes   | None    | GY send: dump Ritual Spell & search Ritual mon| Cost fodder / Souls dump target              | Already used this turn                      |
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
    [Deck("ADML", "ADML")]
    public class ADMLExecutor : ModernExecutor
    {
        public class CardId
        {
            // ── Main Deck Monsters ──
            public const int BlackLusterSoldierLightDarkness = 70405001;
            public const int MagicianOfDarkChaosBlackChaos = 44001993;
            public const int IllusionOfChaos = 12266229;
            public const int DarkMagician = 46986414;
            public const int BlackChaos = 98684220;
            public const int Griffoh = 97462632;
            public const int SkullArchfiendOfChaos = 24088928;
            public const int MagiciansRod = 7084129;
            public const int MagiciansSouls = 97631303;
            public const int DiabellstarTheBlackWitch = 72270339;
            public const int MulcharmyFuwalos = 42141493;
            public const int MulcharmyPurulia = 84192580;
            public const int AshBlossom = 14558128;
            public const int AshBlossomAlt = 14558127;
            public const int DrollAndLockBird = 94145021;

            // ── Main Deck Spells & Traps ──
            public const int LightAndDarknessRitual = 33599853;
            public const int DarkMagicalCurtain = 41350417;
            public const int TheGazeOfTimaeus = 22283204;
            public const int TheHallowedAzamina = 94845588;
            public const int DeceptionOfTheSinfulSpoils = 66328392;
            public const int WantedSeekerOfSinfulSpoils = 80845034;
            public const int RaggedRecordsOfRites = 24461358;
            public const int CalledByTheGrave = 24224830;
            public const int MindShuffle = 24749710;

            // ── Extra Deck ──
            public const int DarkMagicianOfDestruction = 59400890;
            public const int RedEyesDarkDragoon = 37818794;
            public const int AzaminaMuRcielago = 73391962;
            public const int AzaminaIliaSilvia = 46396218;
            public const int RelinquishedAnima = 94259633;
            public const int FirewallDragon = 5043010;
            public const int ProtectcodeTalker = 58036229;
            public const int SeleneQueenOfTheMasterMagicians = 45819647;
            public const int CyberseContractWitch = 37458564;
            public const int ZennasDeceivingDollMaidens = 7594154;
            public const int FourCharmersInProfusion = 27519978;
            public const int WPFancyBall = 4993187;
            public const int IPMasquerena = 65741786;
            public const int SPLittleKnight = 29301450;
            public const int CrossSheep = 50277355;
        }

        // Track last searched card from Illusion of Chaos to NEVER return it to deck
        private int _lastIllusionSearchedId = 0;

        public ADMLExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            RegisterComboLines();
            RegisterExecutors();
        }

        private void RegisterComboLines()
        {
            // ── Line 1: Azamina Early Silvia Anti-Nibiru Line ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "ADML-Azamina-Starter",
                RequiredCards = new List<int> { CardId.WantedSeekerOfSinfulSpoils },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.WantedSeekerOfSinfulSpoils, ActionType = ExecutorType.Activate, Description = "Search Diabellstar the Black Witch" },
                    new() { CardId = CardId.DiabellstarTheBlackWitch, ActionType = ExecutorType.SpSummon, Description = "Special Summon Diabellstar" },
                    new() { CardId = CardId.DiabellstarTheBlackWitch, ActionType = ExecutorType.Activate, Description = "Set Deception of the Sinful Spoils" },
                    new() { CardId = CardId.DeceptionOfTheSinfulSpoils, ActionType = ExecutorType.Activate, Description = "Search The Hallowed Azamina" },
                    new() { CardId = CardId.TheHallowedAzamina, ActionType = ExecutorType.Activate, Description = "Fusion Summon Azamina Ilia Silvia (Omni-Negate)" }
                },
                FallbackLineName = "ADML-Ritual-Trap-Starter"
            });

            // ── Line 2: Ritual Mind Shuffle Trap Setup Line ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "ADML-Ritual-Trap-Starter",
                RequiredCards = new List<int> { CardId.BlackChaos },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.BlackChaos, ActionType = ExecutorType.Activate, Description = "Discard Black Chaos to place Mind Shuffle face-up" }
                },
                FallbackLineName = "ADML-DarkMagician-Starter"
            });

            // ── Line 3: Dark Magician & Dragoon Setup Line ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "ADML-DarkMagician-Starter",
                RequiredCards = new List<int> { CardId.IllusionOfChaos },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.IllusionOfChaos, ActionType = ExecutorType.Activate, Description = "Reveal Illusion of Chaos to search Magicians' Souls" },
                    new() { CardId = CardId.MagiciansSouls, ActionType = ExecutorType.Activate, Description = "Dump Dark Magician and Special Summon Souls" }
                }
            });
        }

        private void RegisterExecutors()
        {
            // ═══════════════════════════════════════════════════════════════
            //  TIER 0: COUNTER TRAPS, HANDTRAPS & HARD DISRUPTIONS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveActivate);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomActivate);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAlt, AshBlossomActivate);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdActivate);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosActivate);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyPurulia, MulcharmyPuruliaActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 1: QUICK OMNI-NEGATES & BOSS DISRUPTIONS
            // ═══════════════════════════════════════════════════════════════
            // Red-Eyes Dark Dragoon: Quick Omni-Negate + Destroy + 1000 ATK
            AddExecutor(ExecutorType.Activate, CardId.RedEyesDarkDragoon, DragoonActivate);

            // Azamina Ilia Silvia: Quick Omni-Negate by tributing self
            AddExecutor(ExecutorType.Activate, CardId.AzaminaIliaSilvia, SilviaActivate);

            // Illusion of Chaos: Field Quick Negate (bounces self to hand, summons Dark Magician from GY)
            AddExecutor(ExecutorType.Activate, CardId.IllusionOfChaos, IllusionOfChaosFieldNegate);

            // W:P Fancy Ball: Quick Monster Negate (banish self till End Phase)
            AddExecutor(ExecutorType.Activate, CardId.WPFancyBall, WPFancyBallActivate);

            // Mind Shuffle: Quick Tag-Out in opponent turn (bounce Lv7+ to summon MagChaos / BLS / Black Chaos)
            AddExecutor(ExecutorType.Activate, CardId.MindShuffle, MindShuffleActivate);

            // Magician of Dark Chaos - Black Chaos: Quick/Ignition Banish opponent card face-down!
            AddExecutor(ExecutorType.Activate, CardId.MagicianOfDarkChaosBlackChaos, MagicianOfDarkChaosActivate);

            // Black Chaos: Field Ignition - Banish 2 opponent cards!
            AddExecutor(ExecutorType.Activate, CardId.BlackChaos, BlackChaosFieldBanishActivate);

            // Black Luster Soldier - Soldier of Light and Darkness: Banish on summon & battle effect
            AddExecutor(ExecutorType.Activate, CardId.BlackLusterSoldierLightDarkness, BLSSoldierActivate);

            // S:P Little Knight: Quick temporary banish of 2 monsters
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightDisruptActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: SEARCHERS, STARTERS & COMBO ENGINES
            // ═══════════════════════════════════════════════════════════════
            // Illusion of Chaos: Reveal to search Souls or Rod, return redundant card
            AddExecutor(ExecutorType.Activate, CardId.IllusionOfChaos, IllusionOfChaosHandActivate);

            // WANTED: Seeker of Sinful Spoils (Search Diabellstar / Draw 1 from GY)
            AddExecutor(ExecutorType.Activate, CardId.WantedSeekerOfSinfulSpoils, WantedActivate);

            // Diabellstar the Black Witch: SpSummon from hand, then Set Deception from deck
            AddExecutor(ExecutorType.SpSummon, CardId.DiabellstarTheBlackWitch, DiabellstarSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.DiabellstarTheBlackWitch, DiabellstarEffect);

            // Deception of the Sinful Spoils: Place on field & tribute monster to search The Hallowed Azamina
            AddExecutor(ExecutorType.Activate, CardId.DeceptionOfTheSinfulSpoils, DeceptionActivate);

            // Magicians' Souls: Hand dump Lv6+ Spellcaster to SS self / DM; Field dump spells to draw
            AddExecutor(ExecutorType.Activate, CardId.MagiciansSouls, MagiciansSoulsActivate);

            // Magician's Rod: Normal Summon search The Gaze of Timaeus or Curtain
            AddExecutor(ExecutorType.Summon, CardId.MagiciansRod, MagiciansRodSummon);
            AddExecutor(ExecutorType.Activate, CardId.MagiciansRod, MagiciansRodActivate);

            // Ragged Records of Rites: Reveal Ritual Spell to search Black Chaos / Griffoh / Skull Archfiend
            AddExecutor(ExecutorType.Activate, CardId.RaggedRecordsOfRites, RaggedRecordsActivate);

            // Black Chaos: Hand discard to place Mind Shuffle face-up on field
            AddExecutor(ExecutorType.Activate, CardId.BlackChaos, BlackChaosHandActivate);

            // Griffoh: Hand discard to set Mind Shuffle (can activate this turn)
            AddExecutor(ExecutorType.Activate, CardId.Griffoh, GriffohActivate);

            // Skull Archfiend of Chaos: GY send trigger (dump Ritual Spell & search Ritual Monster)
            AddExecutor(ExecutorType.Activate, CardId.SkullArchfiendOfChaos, SkullArchfiendActivate);

            // ── Cross-Sheep Combo Bridge Enabler ──
            // Intelligently summon Cross-Sheep BEFORE Fusion/Ritual when we have spent fodder on field
            // AND an upcoming Fusion or Ritual play, so the incoming boss lands in Cross-Sheep's pointed zone!
            AddExecutor(ExecutorType.SpSummon, CardId.CrossSheep, CrossSheepSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.CrossSheep, CrossSheepActivate);

            // The Hallowed Azamina: Send Deception to Fusion Summon Azamina Silvia / Mu
            AddExecutor(ExecutorType.Activate, CardId.TheHallowedAzamina, TheHallowedAzaminaActivate);

            // Azamina Mu Rcielago: Search Azamina or Sinful Spoils on Fusion Summon
            AddExecutor(ExecutorType.Activate, CardId.AzaminaMuRcielago, AzaminaMuActivate);

            // Dark Magical Curtain: Special Summon Dark Magician from deck & search The Gaze of Timaeus
            AddExecutor(ExecutorType.Activate, CardId.DarkMagicalCurtain, DarkMagicalCurtainActivate);

            // The Gaze of Timaeus: Target Dark Magician to Fusion Summon Red-Eyes Dark Dragoon!
            AddExecutor(ExecutorType.Activate, CardId.TheGazeOfTimaeus, TheGazeOfTimaeusActivate);

            // Dark Magician of Destruction: Contact Fusion Lv6+ DARK Spellcaster & search DM card
            AddExecutor(ExecutorType.SpSummon, CardId.DarkMagicianOfDestruction, DarkMagicianOfDestructionSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.DarkMagicianOfDestruction, DarkMagicianOfDestructionActivate);

            // Light and Darkness Ritual: Ritual Summon Magician of Dark Chaos / BLS
            AddExecutor(ExecutorType.Activate, CardId.LightAndDarknessRitual, LightAndDarknessRitualActivate);

            // Special Summons for Main Bosses
            AddExecutor(ExecutorType.SpSummon, CardId.BlackChaos, BlackChaosSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SkullArchfiendOfChaos, SkullArchfiendSpSummon);

            // Fallback Normal Summon
            AddExecutor(ExecutorType.Summon, FallbackNormalSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: EXTRA DECK UTILITY & LINK EXTENDERS
            // ═══════════════════════════════════════════════════════════════
            // Relinquished Anima: Link-1 using Level 1 (Souls/Griffoh) to absorb monster
            AddExecutor(ExecutorType.SpSummon, CardId.RelinquishedAnima, RelinquishedAnimaSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.RelinquishedAnima, RelinquishedAnimaActivate);

            // Cyberse Contract Witch: Send spell to search Ritual Monster
            AddExecutor(ExecutorType.SpSummon, CardId.CyberseContractWitch, CyberseContractWitchSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.CyberseContractWitch, CyberseContractWitchActivate);

            // Selene, Queen of the Master Magicians: Revive Spellcaster
            AddExecutor(ExecutorType.SpSummon, CardId.SeleneQueenOfTheMasterMagicians, SeleneSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SeleneQueenOfTheMasterMagicians, SeleneActivate);

            // S:P Little Knight: Link-2 removal
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightOnSummonActivate);

            // W:P Fancy Ball: Link-3
            AddExecutor(ExecutorType.SpSummon, CardId.WPFancyBall, WPFancyBallSpSummon);

            // I:P Masquerena: Link-2 setup for opponent turn
            AddExecutor(ExecutorType.SpSummon, CardId.IPMasquerena, IPMasquerenaSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.IPMasquerena, IPMasquerenaActivate);

            // Four Charmers in Profusion & Protectcode & Firewall
            AddExecutor(ExecutorType.SpSummon, CardId.FourCharmersInProfusion, FourCharmersSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.FourCharmersInProfusion, FourCharmersActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: SPELL/TRAP SETTING & REPOSITIONING
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, CardId.MindShuffle, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.TheGazeOfTimaeus, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.DarkMagicalCurtain, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, SpellSetStrategy);

            AddExecutor(ExecutorType.Repos, RepositionStrategy);
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 0 & 1: HANDTRAPS & DISRUPTIONS
        // ═══════════════════════════════════════════════════════════════

        private bool CalledByTheGraveActivate()
        {
            ClientCard lastCard = LastChainCard;
            if (lastCard != null && lastCard.Controller == 1)
            {
                ClientCard target = Enemy.Graveyard.FirstOrDefault(c => c.IsMonster() && c.Name == lastCard.Name);
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            if (Duel.Player == 1)
            {
                ClientCard target = Enemy.Graveyard.Where(c => c.IsMonster()).OrderByDescending(c => c.Attack).FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool AshBlossomActivate()
        {
            ClientCard lastCard = LastChainCard;
            return lastCard != null && lastCard.Controller == 1;
        }

        private bool DrollAndLockBirdActivate()
        {
            return Duel.Player == 1;
        }

        private bool MulcharmyFuwalosActivate()
        {
            // Activate Fuwalos only when we control 0 cards (card text condition)
            return Bot.GetFieldCount() == 0 && Duel.Player == 1;
        }

        private bool MulcharmyPuruliaActivate()
        {
            // Activate Purulia only when we control 0 cards (card text condition)
            return Bot.GetFieldCount() == 0 && Duel.Player == 1;
        }

        private bool DragoonActivate()
        {
            ClientCard lastCard = LastChainCard;
            // Case 1: Quick Effect Omni-Negate
            if (lastCard != null && lastCard.Controller == 1)
            {
                // Discard lowest utility card from hand (Skull Archfiend triggers on sent to GY!)
                ClientCard discardFodder = Bot.Hand.FirstOrDefault(c => c.Id == CardId.SkullArchfiendOfChaos)
                                        ?? Bot.Hand.FirstOrDefault(c => c.Id != CardId.TheGazeOfTimaeus &&
                                                                        c.Id != CardId.LightAndDarknessRitual &&
                                                                        c.Id != CardId.MagicianOfDarkChaosBlackChaos &&
                                                                        c.Id != CardId.BlackLusterSoldierLightDarkness)
                                        ?? Bot.Hand.FirstOrDefault();

                if (discardFodder != null)
                {
                    AI.SelectCard(discardFodder);
                    return true;
                }
            }

            // Case 2: Main Phase Monster Destruction & Burn
            if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
            {
                ClientCard oppTarget = Enemy.GetMonsters()
                    .Where(m => m.IsFaceup() && !CardIntelligence.IsTargetImmune(m.Id))
                    .OrderByDescending(m => m.Attack)
                    .FirstOrDefault();

                if (oppTarget != null)
                {
                    AI.SelectCard(oppTarget);
                    return true;
                }
            }

            return false;
        }

        private bool SilviaActivate()
        {
            ClientCard lastCard = LastChainCard;
            // Omni-Negate when opponent activates card/effect
            return lastCard != null && lastCard.Controller == 1;
        }

        private bool IllusionOfChaosFieldNegate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            ClientCard lastCard = LastChainCard;
            // Field Quick Effect: Negate opponent monster effect, bounce self to hand, summon DM from GY
            if (lastCard != null && lastCard.Controller == 1 && lastCard.IsMonster())
            {
                bool hasDmInGy = Bot.Graveyard.Any(c => c.Id == CardId.DarkMagician);
                return hasDmInGy;
            }
            return false;
        }

        private bool WPFancyBallActivate()
        {
            ClientCard lastCard = LastChainCard;
            // Negate opponent monster effect on field or in GY
            return lastCard != null && lastCard.Controller == 1 && lastCard.IsMonster();
        }

        private bool MindShuffleActivate()
        {
            // Case 1: Opponent activates an effect -> Tag-out Lv7+ monster for Ritual Boss ignoring summoning condition!
            ClientCard lastCard = LastChainCard;
            if (lastCard != null && lastCard.Controller == 1)
            {
                ClientCard bounceTarget = Bot.GetMonsters()
                    .Where(m => m.IsFaceup() && m.Level >= 7 && m.Id != CardId.RedEyesDarkDragoon)
                    .FirstOrDefault();

                ClientCard bossInHand = Bot.Hand.FirstOrDefault(c => c.Id == CardId.MagicianOfDarkChaosBlackChaos ||
                                                                    c.Id == CardId.BlackLusterSoldierLightDarkness ||
                                                                    c.Id == CardId.BlackChaos);

                if (bounceTarget != null && bossInHand != null && bounceTarget.Name != bossInHand.Name)
                {
                    AI.SelectCard(bounceTarget);
                    AI.SelectNextCard(bossInHand);
                    return true;
                }
            }

            // Case 2: Turn Search (Either player's turn): Add Ritual monster to hand, then discard 1
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                // Prefer adding MagChaos or BLS or Black Chaos
                AI.SelectCard(CardId.MagicianOfDarkChaosBlackChaos, CardId.BlackLusterSoldierLightDarkness, CardId.BlackChaos, CardId.Griffoh);
                // Discard fodder
                ClientCard discardFodder = Bot.Hand.FirstOrDefault(c => c.Id == CardId.SkullArchfiendOfChaos)
                                        ?? Bot.Hand.FirstOrDefault(c => c.Id == CardId.Griffoh)
                                        ?? Bot.Hand.FirstOrDefault(c => c.Id != CardId.TheGazeOfTimaeus && c.Id != CardId.MindShuffle);
                if (discardFodder != null)
                {
                    AI.SelectNextCard(discardFodder);
                }
                return true;
            }

            return false;
        }

        private bool MagicianOfDarkChaosActivate()
        {
            // Case 1: On Special Summon: Recur 1 Spell from GY to hand
            if (Card.Location == CardLocation.MonsterZone)
            {
                ClientCard spellRecur = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.TheGazeOfTimaeus)
                                     ?? Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.TheHallowedAzamina)
                                     ?? Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.WantedSeekerOfSinfulSpoils)
                                     ?? Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.CalledByTheGrave)
                                     ?? Bot.Graveyard.FirstOrDefault(c => c.IsSpell());

                if (spellRecur != null)
                {
                    AI.SelectCard(spellRecur);
                }
            }

            // Case 2: Quick/Ignition: Banish 1 card opponent controls face-down!
            if (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0)
            {
                ClientCard oppTarget = Enemy.GetMonsters().Where(m => !CardIntelligence.IsTargetImmune(m.Id)).OrderByDescending(m => m.Attack).FirstOrDefault()
                                    ?? Enemy.GetSpells().FirstOrDefault();
                if (oppTarget != null)
                {
                    AI.SelectCard(oppTarget);
                    return true;
                }
            }
            return true;
        }

        private bool BlackChaosFieldBanishActivate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            // Banish 2 cards opponent controls
            var oppCards = Enemy.GetMonsters().Concat(Enemy.GetSpells()).ToList();
            if (oppCards.Count > 0)
            {
                AI.SelectCard(oppCards);
                return true;
            }
            return false;
        }

        private bool BLSSoldierActivate()
        {
            // On Special Summon: Banish 1 card opponent controls
            ClientCard oppTarget = Enemy.GetMonsters().Where(m => !CardIntelligence.IsTargetImmune(m.Id)).OrderByDescending(m => m.Attack).FirstOrDefault()
                                ?? Enemy.GetSpells().FirstOrDefault();
            if (oppTarget != null)
            {
                AI.SelectCard(oppTarget);
                return true;
            }
            return true;
        }

        private bool SPLittleKnightDisruptActivate()
        {
            ClientCard lastCard = LastChainCard;
            if (lastCard != null && lastCard.Controller == 1)
            {
                ClientCard oppTarget = Enemy.GetMonsters().Where(m => m.IsFaceup()).OrderByDescending(m => m.Attack).FirstOrDefault();
                ClientCard myTarget = Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.SPLittleKnight)
                                   ?? Bot.GetMonsters().FirstOrDefault(m => m.Id != CardId.RedEyesDarkDragoon);
                if (oppTarget != null && myTarget != null)
                {
                    AI.SelectCard(myTarget);
                    AI.SelectNextCard(oppTarget);
                    return true;
                }
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 2: SEARCHERS, STARTERS & ENGINE SPELLS
        // ═══════════════════════════════════════════════════════════════

        private bool WantedActivate()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                AI.SelectCard(CardId.DiabellstarTheBlackWitch);
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // In GY: Recycle Sinful Spoils S/T to bottom of deck & Draw 1
                ClientCard recycleTarget = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.DeceptionOfTheSinfulSpoils);
                if (recycleTarget != null)
                {
                    AI.SelectCard(recycleTarget);
                    return true;
                }
            }
            return false;
        }

        private bool DiabellstarSpSummon()
        {
            // Send fodder from hand or field (Skull Archfiend triggers GY search!)
            ClientCard fodder = Bot.Hand.FirstOrDefault(c => c.Id == CardId.SkullArchfiendOfChaos)
                             ?? Bot.Hand.FirstOrDefault(c => c.Id == CardId.DarkMagician)
                             ?? Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.MagiciansRod)
                             ?? Bot.Hand.FirstOrDefault(c => c.Id == CardId.RaggedRecordsOfRites)
                             ?? Bot.Hand.FirstOrDefault(c => c.Id != CardId.TheGazeOfTimaeus &&
                                                             c.Id != CardId.DiabellstarTheBlackWitch &&
                                                             c.Id != CardId.WantedSeekerOfSinfulSpoils);

            if (fodder != null)
            {
                AI.SelectCard(fodder);
                return true;
            }
            return false;
        }

        private bool DiabellstarEffect()
        {
            // On summon: Set Deception of the Sinful Spoils directly from deck
            AI.SelectCard(CardId.DeceptionOfTheSinfulSpoils);
            return true;
        }

        private bool DeceptionActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return !Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.DeceptionOfTheSinfulSpoils);
            }

            // Continuous Spell Ignition: Tribute 1 monster -> Add 1 Azamina card (The Hallowed Azamina)
            ClientCard tribute = Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.MagiciansRod)
                              ?? Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.DiabellstarTheBlackWitch)
                              ?? Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.MagiciansSouls)
                              ?? Bot.Hand.FirstOrDefault(c => c.Id == CardId.Griffoh)
                              ?? Bot.Hand.FirstOrDefault(c => c.Id == CardId.SkullArchfiendOfChaos);

            if (tribute != null)
            {
                AI.SelectCard(tribute);
                AI.SelectNextCard(CardId.TheHallowedAzamina);
                return true;
            }
            return false;
        }

        private bool TheHallowedAzaminaActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Reveal Azamina Ilia Silvia (Omni-Negate) or Mu Rcielago (Searcher)
                if (!Bot.HasInMonstersZone(CardId.AzaminaIliaSilvia))
                {
                    AI.SelectCard(CardId.AzaminaIliaSilvia);
                }
                else
                {
                    AI.SelectCard(CardId.AzaminaMuRcielago);
                }

                // Send Deception of the Sinful Spoils from field/hand as cost
                ClientCard sinfulSpoilsCost = Bot.SpellZone.FirstOrDefault(s => s != null && s.IsFaceup() && s.Id == CardId.DeceptionOfTheSinfulSpoils)
                                           ?? Bot.Hand.FirstOrDefault(c => c.Id == CardId.DeceptionOfTheSinfulSpoils);

                if (sinfulSpoilsCost != null)
                {
                    AI.SelectNextCard(sinfulSpoilsCost);
                }
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // Recycle Azamina monster from GY/field to hand
                ClientCard recycle = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.AzaminaIliaSilvia || c.Id == CardId.AzaminaMuRcielago);
                if (recycle != null)
                {
                    AI.SelectCard(recycle);
                    return true;
                }
            }
            return false;
        }

        private bool AzaminaMuActivate()
        {
            // On Fusion Summon: Search Azamina or Sinful Spoils card
            AI.SelectCard(CardId.WantedSeekerOfSinfulSpoils, CardId.TheHallowedAzamina, CardId.DeceptionOfTheSinfulSpoils);
            return true;
        }

        private bool RaggedRecordsActivate()
        {
            // Reveal Light and Darkness Ritual -> Search Ritual monster
            AI.SelectCard(CardId.LightAndDarknessRitual);
            AI.SelectNextCard(CardId.BlackChaos, CardId.Griffoh, CardId.SkullArchfiendOfChaos, CardId.MagicianOfDarkChaosBlackChaos, CardId.BlackLusterSoldierLightDarkness);
            return true;
        }

        private bool BlackChaosHandActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // Discard self to place Mind Shuffle face-up on field
            bool hasMindShuffle = Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.MindShuffle);
            return !hasMindShuffle;
        }

        private bool GriffohActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // Set Mind Shuffle from deck (can activate this turn)
            bool hasMindShuffle = Bot.SpellZone.Any(s => s != null && s.Id == CardId.MindShuffle) || Bot.HasInHand(CardId.MindShuffle);
            if (!hasMindShuffle)
            {
                AI.SelectCard(CardId.MindShuffle);
                return true;
            }
            return false;
        }

        private bool IllusionOfChaosHandActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // Hand effect: Reveal -> Search Magicians' Souls or Rod, return redundant card to top of deck
            int targetId = !Bot.HasInHand(CardId.MagiciansSouls) && !Bot.HasInMonstersZone(CardId.MagiciansSouls)
                ? CardId.MagiciansSouls
                : CardId.MagiciansRod;

            _lastIllusionSearchedId = targetId;
            AI.SelectCard(targetId);

            // Select card to place back on top of deck: redundant card, never the searched card!
            ClientCard returnCard = Bot.Hand.FirstOrDefault(c => c.Id != targetId &&
                                                                c.Id != CardId.IllusionOfChaos &&
                                                                c.Id != CardId.TheGazeOfTimaeus &&
                                                                c.Id != CardId.WantedSeekerOfSinfulSpoils &&
                                                                (c.Id == CardId.DarkMagician || c.Id == CardId.Griffoh || c.Id == CardId.RaggedRecordsOfRites))
                                 ?? Bot.Hand.FirstOrDefault(c => c.Id != targetId && c.Id != CardId.IllusionOfChaos);

            if (returnCard != null)
            {
                AI.SelectNextCard(returnCard);
            }
            return true;
        }

        private bool MagiciansSoulsActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Dump Dark Magician or Skull Archfiend (triggers on sent to GY!) or Diabellstar
                int dumpTarget = !Bot.Graveyard.Any(c => c.Id == CardId.DarkMagician)
                    ? CardId.DarkMagician
                    : (!Bot.Graveyard.Any(c => c.Id == CardId.SkullArchfiendOfChaos) ? CardId.SkullArchfiendOfChaos : CardId.DiabellstarTheBlackWitch);

                AI.SelectCard(dumpTarget);

                // If we have Timaeus in hand and no DM on field, summon DM directly to field for Dragoon!
                if (Bot.HasInHand(CardId.TheGazeOfTimaeus) && !Bot.GetMonsters().Any(m => m.IsFaceup() && m.Id == CardId.DarkMagician) && dumpTarget == CardId.DarkMagician)
                {
                    AI.SelectOption(1);
                    AI.SelectNextCard(CardId.DarkMagician);
                    return true;
                }

                AI.SelectOption(0);
                return true;
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                // Send up to 2 Spells/Traps to draw
                bool canSendDeception = Bot.HasInHand(CardId.TheHallowedAzamina) ||
                                       Bot.HasInMonstersZone(CardId.AzaminaIliaSilvia) ||
                                       Bot.HasInMonstersZone(CardId.AzaminaMuRcielago);

                var deadSpells = Bot.SpellZone.Where(s => s != null && s.IsFaceup() &&
                                                         ((s.Id == CardId.DeceptionOfTheSinfulSpoils && canSendDeception) ||
                                                           s.Id == CardId.WantedSeekerOfSinfulSpoils)).Take(2).ToList();
                if (deadSpells.Count > 0)
                {
                    AI.SelectCard(deadSpells);
                    return true;
                }
            }
            return false;
        }

        private bool SkullArchfiendActivate()
        {
            // On sent to GY: Dump Light and Darkness Ritual & Search Ritual Monster
            AI.SelectCard(CardId.LightAndDarknessRitual);
            AI.SelectNextCard(CardId.MagicianOfDarkChaosBlackChaos, CardId.BlackLusterSoldierLightDarkness, CardId.BlackChaos);
            return true;
        }

        private bool DarkMagicalCurtainActivate()
        {
            // Special Summon Dark Magician from deck -> Search The Gaze of Timaeus
            AI.SelectCard(CardId.DarkMagician);
            AI.SelectNextCard(CardId.TheGazeOfTimaeus);
            return true;
        }

        private bool TheGazeOfTimaeusActivate()
        {
            // Target Dark Magician on field -> Fusion Summon Dragoon!
            ClientCard dmTarget = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.Id == CardId.DarkMagician);

            if (dmTarget != null)
            {
                AI.SelectCard(dmTarget);
                AI.SelectNextCard(CardId.RedEyesDarkDragoon);
                return true;
            }
            return false;
        }

        private bool DarkMagicianOfDestructionSpSummon()
        {
            // Contact Fusion by banishing Lv6+ DARK Spellcaster (Diabellstar or DM)
            ClientCard mat = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.Level >= 6 &&
                                                                   m.Attribute == (int)CardAttribute.Dark &&
                                                                   m.Id != CardId.RedEyesDarkDragoon &&
                                                                   m.Id != CardId.MagicianOfDarkChaosBlackChaos);
            if (mat != null && !Bot.HasInMonstersZone(CardId.DarkMagicianOfDestruction))
            {
                AI.SelectCard(mat);
                return true;
            }
            return false;
        }

        private bool DarkMagicianOfDestructionActivate()
        {
            // Search Dark Magician card: The Gaze of Timaeus or Dark Magical Curtain
            AI.SelectCard(CardId.TheGazeOfTimaeus, CardId.DarkMagicalCurtain, CardId.DarkMagician);
            return true;
        }

        private bool LightAndDarknessRitualActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Select Ritual Boss to summon: MagChaos or BLS
                int bossId = !Bot.HasInMonstersZone(CardId.MagicianOfDarkChaosBlackChaos)
                    ? CardId.MagicianOfDarkChaosBlackChaos
                    : CardId.BlackLusterSoldierLightDarkness;

                AI.SelectCard(bossId);

                // Tribute priority: Griffoh in GY counts as ENTIRE tribute!
                ClientCard griffohGy = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.Griffoh);
                if (griffohGy != null)
                {
                    AI.SelectNextCard(griffohGy);
                }
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // Add self and Ritual card from GY to hand
                ClientCard ritCard = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.BlackChaos ||
                                                                      c.Id == CardId.Griffoh ||
                                                                      c.Id == CardId.SkullArchfiendOfChaos);
                if (ritCard != null)
                {
                    AI.SelectCard(ritCard);
                    return true;
                }
            }
            return false;
        }

        private bool MagiciansRodSummon()
        {
            return true;
        }

        private bool MagiciansRodActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Normal Summon search: The Gaze of Timaeus or Dark Magical Curtain
                AI.SelectCard(CardId.TheGazeOfTimaeus, CardId.DarkMagicalCurtain);
                return true;
            }
            return false;
        }

        private bool BlackChaosSpSummon()
        {
            // Shuffle Ritual monster from GY/hand to deck
            ClientCard ritMat = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.IllusionOfChaos ||
                                                                 c.Id == CardId.MagicianOfDarkChaosBlackChaos ||
                                                                 c.Id == CardId.BlackLusterSoldierLightDarkness)
                             ?? Bot.Hand.FirstOrDefault(c => c.Id == CardId.IllusionOfChaos);

            if (ritMat != null)
            {
                AI.SelectCard(ritMat);
                return true;
            }
            return false;
        }

        private bool SkullArchfiendSpSummon()
        {
            // Shuffle 3 cards in GY/banished to deck bottom
            var candidates = Bot.Graveyard.Where(c => c.Id != CardId.DarkMagician && c.Id != CardId.TheGazeOfTimaeus).Take(3).ToList();
            if (candidates.Count >= 3)
            {
                AI.SelectCard(candidates);
                return true;
            }
            return false;
        }

        private bool FallbackNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 3: EXTRA DECK UTILITY & LINKS
        // ═══════════════════════════════════════════════════════════════

        private bool RelinquishedAnimaSpSummon()
        {
            // Only summon if opponent has a monster in the column Anima points to
            ClientCard fodder = Bot.GetMonsters().FirstOrDefault(m => m.Level == 1 && (m.Id == CardId.MagiciansSouls || m.Id == CardId.Griffoh));
            return fodder != null && Enemy.GetMonsterCount() > 0;
        }

        private bool RelinquishedAnimaActivate()
        {
            ClientCard oppTarget = Enemy.GetMonsters().Where(m => m.IsFaceup() && !CardIntelligence.IsTargetImmune(m.Id)).OrderByDescending(m => m.Attack).FirstOrDefault();
            if (oppTarget != null)
            {
                AI.SelectCard(oppTarget);
                return true;
            }
            return false;
        }

        private bool CanTriggerCrossSheepThisTurn()
        {
            // Cross-Sheep triggers when a Fusion or Ritual monster is Special Summoned to a zone it points to.
            // Check if a Fusion or Ritual play is actively ready this turn:
            bool hasFusionReady = (Bot.HasInHand(CardId.TheHallowedAzamina) && (Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.DeceptionOfTheSinfulSpoils) || Bot.HasInHand(CardId.DeceptionOfTheSinfulSpoils) || Bot.HasInHand(CardId.WantedSeekerOfSinfulSpoils)))
                               || (Bot.HasInHand(CardId.TheGazeOfTimaeus) && (Bot.GetMonsters().Any(m => m.IsFaceup() && m.Id == CardId.DarkMagician) || Bot.HasInHand(CardId.DarkMagicalCurtain)));

            bool hasRitualReady = Bot.HasInHand(CardId.LightAndDarknessRitual) && (Bot.HasInHand(CardId.MagicianOfDarkChaosBlackChaos) || Bot.HasInHand(CardId.BlackLusterSoldierLightDarkness));

            var validMats = Bot.GetMonsters().Where(m =>
                m.Id != CardId.RedEyesDarkDragoon &&
                m.Id != CardId.AzaminaIliaSilvia &&
                m.Id != CardId.MagicianOfDarkChaosBlackChaos &&
                m.Id != CardId.BlackLusterSoldierLightDarkness &&
                m.Id != CardId.BlackChaos &&
                !m.HasType(CardType.Link)
            ).ToList();

            // Revive target exists in GY or is currently on field and will be sent to GY as Cross-Sheep material:
            bool hasReviveTarget = Bot.Graveyard.Any(c => c.Id == CardId.MagiciansSouls || c.Id == CardId.MagiciansRod || c.Id == CardId.Griffoh)
                                || validMats.Any(m => m.Level <= 4 && (m.Id == CardId.MagiciansSouls || m.Id == CardId.MagiciansRod || m.Id == CardId.Griffoh));

            if (hasFusionReady && hasReviveTarget) return true;
            if (hasRitualReady) return true;

            return false;
        }

        private bool CrossSheepSpSummon()
        {
            // Do not summon Cross-Sheep if it cannot be actively triggered this turn
            if (!CanTriggerCrossSheepThisTurn())
                return false;

            // Only use spent fodder / non-boss monsters as materials
            // NEVER sacrifice Ace Bosses or active Omni-Negates!
            var validMats = Bot.GetMonsters().Where(m =>
                m.Id != CardId.RedEyesDarkDragoon &&
                m.Id != CardId.AzaminaIliaSilvia &&
                m.Id != CardId.MagicianOfDarkChaosBlackChaos &&
                m.Id != CardId.BlackLusterSoldierLightDarkness &&
                m.Id != CardId.BlackChaos &&
                !m.HasType(CardType.Link)
            ).ToList();

            if (validMats.Count < 2)
                return false;

            // Must have different names (Cross-Sheep requirement: 2 monsters with different names)
            if (validMats.Select(m => m.Id).Distinct().Count() < 2)
                return false;

            // Material Value Evaluation:
            // Prefer low-ATK spent fodder (Souls 0 ATK, Rod 1600 ATK, Griffoh 300 ATK)
            var lowAtkMats = validMats.Where(m => m.Attack < 2000).ToList();
            if (lowAtkMats.Count >= 2 && lowAtkMats.Select(m => m.Id).Distinct().Count() >= 2)
            {
                var chosen = lowAtkMats.GroupBy(m => m.Id).Select(g => g.First()).OrderBy(m => m.Attack).Take(2).ToList();
                AI.SelectCard(chosen);
                return true;
            }

            // If we only have 1 low-ATK monster and 1 beater (e.g. Souls + Diabellstar):
            // Only summon if we have Selene in Extra Deck to immediately revive the beater back!
            if (Bot.ExtraDeck.Any(c => c.Id == CardId.SeleneQueenOfTheMasterMagicians) && lowAtkMats.Count >= 1)
            {
                var chosen = validMats.GroupBy(m => m.Id).Select(g => g.First()).OrderBy(m => m.Attack).Take(2).ToList();
                if (chosen.Count >= 2)
                {
                    AI.SelectCard(chosen);
                    return true;
                }
            }

            return false;
        }

        private bool CrossSheepActivate()
        {
            // Case 1: Fusion Summon trigger -> Revive Level 4 or lower from GY (Souls, Rod, Griffoh)
            ClientCard revive = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.MagiciansSouls)
                             ?? Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.MagiciansRod)
                             ?? Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.Griffoh);
            if (revive != null)
            {
                AI.SelectCard(revive);
            }
            return true;
        }

        private bool CyberseContractWitchSpSummon()
        {
            // 2 monsters with same Type (e.g. 2 Spellcasters)
            var spellcasters = Bot.GetMonsters().Where(m => m.Race == (int)CardRace.SpellCaster &&
                                                           m.Id != CardId.RedEyesDarkDragoon &&
                                                           m.Id != CardId.AzaminaIliaSilvia &&
                                                           m.Id != CardId.MagicianOfDarkChaosBlackChaos).ToList();
            return spellcasters.Count >= 2;
        }

        private bool CyberseContractWitchActivate()
        {
            // Send 1 spell from hand/field to search Ritual monster
            ClientCard spellCost = Bot.Hand.FirstOrDefault(c => c.IsSpell() && c.Id != CardId.TheGazeOfTimaeus && c.Id != CardId.LightAndDarknessRitual)
                                ?? Bot.SpellZone.FirstOrDefault(s => s != null && s.IsFaceup() && s.Id == CardId.DeceptionOfTheSinfulSpoils);
            if (spellCost != null)
            {
                AI.SelectCard(spellCost);
                AI.SelectNextCard(CardId.MagicianOfDarkChaosBlackChaos, CardId.BlackLusterSoldierLightDarkness, CardId.IllusionOfChaos);
                return true;
            }
            return false;
        }

        private bool SeleneSpSummon()
        {
            // Selene (Link-3) requires 2+ monsters including a Spellcaster, Link rating = 3
            // Cross-Sheep (Link-2) + Magicians' Souls (Level 1 Spellcaster) = Link-3!
            var validMats = Bot.GetMonsters().Where(m =>
                m.Id != CardId.RedEyesDarkDragoon &&
                m.Id != CardId.AzaminaIliaSilvia &&
                m.Id != CardId.MagicianOfDarkChaosBlackChaos &&
                m.Id != CardId.BlackLusterSoldierLightDarkness
            ).ToList();

            if (validMats.Count < 2) return false;

            int totalRating = validMats.Sum(m => m.HasType(CardType.Link) ? m.LinkMarker : 1);
            if (totalRating < 3) return false;

            bool hasSpellcaster = validMats.Any(m => m.Race == (int)CardRace.SpellCaster);
            if (!hasSpellcaster) return false;

            // Make sure we have a valuable Spellcaster in GY to revive!
            bool hasGyRevive = Bot.Graveyard.Any(c => c.Race == (int)CardRace.SpellCaster &&
                                                     (c.Id == CardId.DarkMagician ||
                                                      c.Id == CardId.DiabellstarTheBlackWitch ||
                                                      c.Id == CardId.BlackChaos ||
                                                      c.Id == CardId.MagicianOfDarkChaosBlackChaos));

            if (!hasGyRevive) return false;

            var chosen = validMats.OrderBy(m => m.Attack).Take(2).ToList();
            AI.SelectCard(chosen);
            return true;
        }

        private bool SeleneActivate()
        {
            // Revive Spellcaster from hand or GY
            ClientCard target = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.DarkMagician)
                             ?? Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.DiabellstarTheBlackWitch)
                             ?? Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.BlackChaos);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool SPLittleKnightSpSummon()
        {
            // Never sacrifice Ace bosses
            var mats = Bot.GetMonsters().Where(m => m.Id != CardId.RedEyesDarkDragoon &&
                                                   m.Id != CardId.AzaminaIliaSilvia &&
                                                   m.Id != CardId.MagicianOfDarkChaosBlackChaos &&
                                                   m.Id != CardId.BlackLusterSoldierLightDarkness).ToList();
            return mats.Count >= 2 && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0);
        }

        private bool SPLittleKnightOnSummonActivate()
        {
            ClientCard oppTarget = Enemy.GetMonsters().Where(m => !CardIntelligence.IsTargetImmune(m.Id)).OrderByDescending(m => m.Attack).FirstOrDefault()
                                ?? Enemy.GetSpells().FirstOrDefault()
                                ?? Enemy.Graveyard.OrderByDescending(c => c.Attack).FirstOrDefault();
            if (oppTarget != null)
            {
                AI.SelectCard(oppTarget);
                return true;
            }
            return false;
        }

        private bool WPFancyBallSpSummon()
        {
            var mats = Bot.GetMonsters().Where(m => m.Id != CardId.RedEyesDarkDragoon &&
                                                   m.Id != CardId.AzaminaIliaSilvia &&
                                                   m.Id != CardId.MagicianOfDarkChaosBlackChaos).ToList();
            return mats.Count >= 3;
        }

        private bool IPMasquerenaSpSummon()
        {
            return Duel.Phase == DuelPhase.Main2 && Bot.GetMonsterCount() >= 2;
        }

        private bool IPMasquerenaActivate()
        {
            return Duel.Player == 1;
        }

        private bool FourCharmersSpSummon()
        {
            return Duel.Phase == DuelPhase.Main2 && Bot.GetMonsterCount() >= 4;
        }

        private bool FourCharmersActivate()
        {
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 4: SPELL SETTING & REPOSITIONING
        // ═══════════════════════════════════════════════════════════════

        private bool SpellSetStrategy()
        {
            if (Card.IsTrap()) return true;
            if (Card.IsSpell() && Card.HasType(CardType.QuickPlay))
            {
                return Duel.Phase == DuelPhase.Main2;
            }
            return false;
        }

        private bool RepositionStrategy()
        {
            if (Card.Attack < 1500 && Card.IsAttack()) return true;
            if (Card.Attack >= 2000 && Card.IsDefense()) return true;
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  OVERRIDDEN CALLBACKS & UNIVERSAL GUARDS
        // ═══════════════════════════════════════════════════════════════

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards != null && cards.Count > 0)
            {
                // 1. Deck Search (Hint 506 = HINTMSG_ATOHAND or all candidates from Deck)
                if (hint == 506 || cards.All(c => c.Location == CardLocation.Deck))
                {
                    var preferred = cards.OrderBy(c =>
                    {
                        if (c.Id == CardId.WantedSeekerOfSinfulSpoils) return 1;
                        if (c.Id == CardId.DiabellstarTheBlackWitch) return 2;
                        if (c.Id == CardId.IllusionOfChaos) return 3;
                        if (c.Id == CardId.MagiciansSouls) return 4;
                        if (c.Id == CardId.TheGazeOfTimaeus) return 5;
                        if (c.Id == CardId.BlackChaos) return 6;
                        if (c.Id == CardId.MindShuffle) return 7;
                        if (c.Id == CardId.LightAndDarknessRitual) return 8;
                        if (c.Id == CardId.MagicianOfDarkChaosBlackChaos) return 9;
                        if (c.Id == CardId.BlackLusterSoldierLightDarkness) return 10;
                        if (c.Id == CardId.DarkMagician) return 11;
                        return 99;
                    }).ToList();

                    if (preferred.Count >= min)
                    {
                        return preferred.Take(max).ToList();
                    }
                }

                // 2. Removal / Banish / Destroy (Hint 502, 503, 504, 505, 507): ALWAYS target ENEMY cards!
                if (hint == 502 || hint == 503 || hint == 504 || hint == 505 || hint == 507)
                {
                    var enemyTargets = cards.Where(c => c.Controller == 1 && !CardIntelligence.IsTargetImmune(c.Id)).ToList();
                    if (enemyTargets.Count >= min)
                    {
                        return enemyTargets.OrderByDescending(c => c.Attack).Take(max).ToList();
                    }
                }

                // 3. Placing card back on deck for Illusion of Chaos
                if (cards.All(c => c.Location == CardLocation.Hand) && _lastIllusionSearchedId > 0)
                {
                    var unneededCards = cards.Where(c => c.Id != _lastIllusionSearchedId).ToList();
                    if (unneededCards.Count >= min)
                    {
                        return unneededCards.Take(max).ToList();
                    }
                }

                // 4. Ritual Tribute selection: prefer Griffoh in GY (full Lv8 tribute) or monsters in GY before field
                if (hint == 500 || hint == 516)
                {
                    var griffoh = cards.FirstOrDefault(c => c.Id == CardId.Griffoh && c.Location == CardLocation.Grave);
                    if (griffoh != null)
                    {
                        return new List<ClientCard> { griffoh };
                    }
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
            // Rule 11 Compliance: Reserve Extra Monster Zone (0x20) exclusively for Link monsters
            int mmz = available & 0x1F;
            if (mmz != 0)
            {
                bool isLink = cardId == CardId.RelinquishedAnima ||
                              cardId == CardId.FirewallDragon ||
                              cardId == CardId.ProtectcodeTalker ||
                              cardId == CardId.SeleneQueenOfTheMasterMagicians ||
                              cardId == CardId.CyberseContractWitch ||
                              cardId == CardId.ZennasDeceivingDollMaidens ||
                              cardId == CardId.FourCharmersInProfusion ||
                              cardId == CardId.WPFancyBall ||
                              cardId == CardId.IPMasquerena ||
                              cardId == CardId.SPLittleKnight ||
                              cardId == CardId.CrossSheep;

                if (!isLink)
                {
                    // If Cross-Sheep is on field, prioritize the zones Cross-Sheep points to
                    ClientCard crossSheep = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.Id == CardId.CrossSheep);
                    if (crossSheep != null)
                    {
                        int pointedZones = crossSheep.GetLinkedZones() & 0x1F;
                        int match = mmz & pointedZones;
                        if (match != 0)
                        {
                            int[] preference = { 0x4, 0x1, 0x10, 0x2, 0x8 };
                            foreach (int z in preference)
                            {
                                if ((match & z) != 0)
                                    return z;
                            }
                        }
                    }

                    return base.OnSelectPlace(cardId, player, location, mmz);
                }
            }
            return base.OnSelectPlace(cardId, player, location, available);
        }

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            // Anti-Pattern Rule 14: Never accept opponent's effect offers blindly
            if (card != null && card.Controller == 1)
            {
                return false;
            }
            return base.OnSelectEffectYn(card, desc);
        }
    }
}
