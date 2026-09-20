// ============================================================================
// CARD AUDIT — Anime_Bruno (Bruno / Antinomy's Ultimate Delta Accel Synchro T.G.)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Ash Blossom & Joyous Spring        | Monster L3 T | Yes  | Yes   | Discard | Handtrap: Negate deck search/dump/SS          | Opponent activates deck-interacting effect    | Bot's own turn without threat               |
// | Infinite Impermanence              | Trap Normal  | Yes  | Yes   | None    | Negate 1 face-up monster; column S/T negate   | Opponent monster activates or dangerous boss  | Target already negated                      |
// | Called by the Grave                | Spell Quick  | Yes  | Yes   | Target  | Banish monster in opp GY and negate effects   | Opp activates handtrap or dangerous GY effect | No target in opp GY                         |
// | Harpie's Feather Duster            | Spell Normal | No   | No    | None    | Destroy all Spells and Traps opponent controls| Opponent controls 1+ Spell/Trap cards         | Opponent controls 0 Spells/Traps            |
// | Bonfire                            | Spell Normal | Yes  | Yes   | None    | Add 1 Level 4 or lower Pyro monster from Deck | Main Phase: Search T.G. Rocket Salamander     | Already have Salamander or used this turn   |
// | One for One                        | Spell Normal | Yes  | Yes   | Send mon| Special Summon Level 1 monster from Deck      | Main Phase: SS T.G. Rocket Salamander         | No monsters to discard / Salamander in hand |
// | T.G. All Clear                     | Spell Cont   | Yes  | Yes   | Destroy | Extra NS; pop 1 T.G. on field/hand to search  | Main Phase: gain extra NS, pop to search      | Already used pop search this turn           |
// | T.G. Limiter Removal               | Spell Quick  | Yes  | Yes   | Discard | Discard 1; add 2 T.G. monsters from Deck      | Main Phase: search starters/extenders         | Hand empty (cannot discard)                 |
// | T.G. Close                         | Trap Counter | Yes  | Yes   | None    | Omni-negate (Monster/Spell/Trap) + GY reset   | Opponent activates card/eff & control T.G.    | No Machine T.G. on field                    |
// | T.G. Rocket Salamander             | Monster L1   | Yes  | Yes   | Tribute | Tribute T.G. -> SS Screw Serpent; GY revive   | Main Phase starter; tribute self -> SS Serpent| Already used this turn                      |
// | T.G. Screw Serpent                 | Monster L4 T | Yes  | Yes   | Target  | On NS/SS: Revive L<=4 T.G.; GY modulate level | On Summon revive Salamander/Tank Grub; extend | No targets in GY                            |
// | T.G. Warwolf                       | Monster L3   | Yes  | Yes   | None    | SS from hand when L<=4 monster Special Summon | When any L<=4 monster is Special Summoned     | Field full                                  |
// | T.G. Striker                       | Monster L2 T | Yes  | Yes   | None    | SS from hand if opp controls monster & bot 0  | Opponent controls monster, bot controls none  | Bot controls monster                        |
// | T.G. Tank Grub                     | Monster L1 T | Yes  | Yes   | None    | Treat as non-tuner; on Synchro send SS Token  | Sent as Synchro Material -> spawn Level 1 Tkn | Field full                                  |
// | T.G. Gear Zombie                   | Monster L1 T | Yes  | Yes   | Target  | Lower friendly T.G. ATK by 1000 -> SS from hand| Control T.G. with >= 1000 ATK or extender need| No T.G. on field                            |
// | T.G. Booster Raptor                | Monster L1   | Yes  | Yes   | None    | SS from hand if control T.G. monster          | Control T.G. monster on field                 | No T.G. on field                            |
// | T.G. Drill Fish                    | Monster L1   | Yes  | Yes   | None    | SS if control only T.G.; direct attack; pop mon| Control only T.G. monsters                   | Control non-T.G. monster                    |
// | T.G. Rush Rhino                    | Monster L4   | No   | No    | None    | +400 ATK on attack; Beast fodder for Trident  | Material for Trident Launcher / Synchro Lv 5  | No need                                     |
// | T.G. Trident Launcher              | Link-3       | Yes  | Yes   | None    | Link Summon: SS 3 T.G. (Hand, Deck, GY)       | Link Summoned with targets in Hand, Deck, GY  | Zones full / no targets                     |
// | T.G. Mighty Striker                | Synchro L2 T | Yes  | Yes   | None    | On Synchro: Search All Clear/Close; GY foolish| Synchro Summoned; search T.G. Spell/Trap      | Already used this turn                      |
// | T.G. Over Dragonar                 | Synchro L5   | Yes  | Yes   | None    | On Synchro: Revive all T.G. from GY; float drw| Synchro Summoned with T.G. in GY             | GY empty                                    |
// | T.G. Star Guardian                 | Synchro L5 T | Yes  | Yes   | None    | On SS: Add T.G. from GY; SS T.G. from hand    | Special Summoned; recycle and extend          | No T.G. in GY / hand                        |
// | T.G. Hyper Librarian               | Synchro L5   | No   | No    | None    | Draw 1 card when a monster is Synchro Summoned| Synchro climb setup                           | Deck near empty                             |
// | T.G. Wonder Magician               | Synchro L5 T | Yes  | Yes   | Target  | On Synchro: Destroy 1 opp Spell/Trap; quick S | Synchro Summoned; pop opp backrow             | Opponent has no Spells/Traps                |
// | T.G. Recipro Dragonfly             | Synchro L2   | Yes  | Yes   | Target  | Send T.G. Synchro to GY -> revive its materials| De-synchro Dragonar to re-swarm materials     | No valid materials                          |
// | T.G. Power Gladiator               | Synchro L5   | No   | No    | None    | Piercing damage; draw 1 card when destroyed   | Level 5 Synchro stepping stone                | No need                                     |
// | T.G. Blade Blaster                 | Synchro L10  | Yes  | Yes   | Discard | Accel Synchro 3300 ATK; negate targeting S/T  | Level 10 boss attacker                        | Discard empty                               |
// | Shooting Star Dragon T.G. EX       | Synchro L10  | Yes  | Yes   | Banish  | Accel Synchro 3300 ATK; negate targeting eff  | Opponent targets bot monster or attacks       | Already negated                             |
// | T.G. Glaive Blaster                | Synchro L12  | Yes  | Yes   | Target  | Delta Accel 4000 ATK; Quick banish Extra Deck | Opp Extra Deck monster summoned / on field    | Already banished max times                  |
// | T.G. Halberd Cannon                | Synchro L12  | Yes  | Yes   | None    | Delta Accel 4000 ATK; Negate monster summon   | Opponent attempts monster summon              | Already negated this turn                   |
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
    [Deck("Anime_Bruno", "Anime_Bruno")]
    public class Anime_BrunoExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int TGRocketSalamander = 77392987;
            public const int TGScrewSerpent = 11234702;
            public const int TGWarwolf = 293542;
            public const int TGStriker = 1315120;
            public const int TGTankGrub = 74627016;
            public const int TGGearZombie = 94350039;
            public const int TGBoosterRaptor = 48633301;
            public const int TGDrillFish = 30348744;
            public const int TGRushRhino = 36687247;
            public const int AshBlossom = 14558127;
            public const int InfiniteImpermanence = 10045474;

            // Spells
            public const int TGAllClear = 54573517;
            public const int TGLimiterRemoval = 28189908;
            public const int Bonfire = 85106525;
            public const int OneForOne = 2295440;
            public const int CalledByTheGrave = 24224830;
            public const int HarpiesFeatherDuster = 18144506;

            // Traps
            public const int TGClose = 2339825;

            // Extra Deck
            public const int TGTridentLauncher = 50750868;
            public const int TGMightyStriker = 32480825;
            public const int TGOverDragonar = 68989420;
            public const int TGStarGuardian = 99937842;
            public const int TGHyperLibrarian = 90953320;
            public const int TGWonderMagician = 98558751;
            public const int TGReciproDragonfly = 62560742;
            public const int TGPowerGladiator = 24943456;
            public const int TGBladeBlaster = 51447164;
            public const int ShootingStarDragonTGEX = 63180841;
            public const int TGGlaiveBlaster = 95973569;
            public const int TGHalberdCannon = 97836203;

            // Tokens
            public const int TGToken = 74627017;
        }

        public Anime_BrunoExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            RegisterComboLines();
            RegisterExecutors();
        }

        private void RegisterComboLines()
        {
            // ── Line 1: 1-Card Rocket Salamander Full Board ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "TG-Salamander-FullBoard",
                RequiredCards = new List<int> { CardId.TGRocketSalamander },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.TGRocketSalamander, ActionType = ExecutorType.Summon, Description = "Normal Summon Rocket Salamander" },
                    new() { CardId = CardId.TGRocketSalamander, ActionType = ExecutorType.Activate, Description = "Tribute Salamander -> Special Summon Screw Serpent from Deck" },
                    new() { CardId = CardId.TGScrewSerpent, ActionType = ExecutorType.Activate, Description = "Screw Serpent revives Salamander from GY" },
                    new() { CardId = CardId.TGOverDragonar, ActionType = ExecutorType.SpSummon, Description = "Synchro Summon Over Dragonar (Lv 4 + Lv 1 = Lv 5)" },
                    new() { CardId = CardId.TGOverDragonar, ActionType = ExecutorType.Activate, Description = "Over Dragonar revives all T.G. monsters from GY" }
                },
                FallbackLineName = "TG-Extender-Climb"
            });

            // ── Line 2: Bonfire / Searcher Starter ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "TG-Bonfire-Starter",
                RequiredCards = new List<int> { CardId.Bonfire },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.Bonfire, ActionType = ExecutorType.Activate, Description = "Bonfire adds Rocket Salamander to hand" },
                    new() { CardId = CardId.TGRocketSalamander, ActionType = ExecutorType.Summon, Description = "Normal Summon Rocket Salamander" }
                },
                FallbackLineName = "TG-Extender-Climb"
            });

            // ── Line 3: Extender Synchro Climb ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "TG-Extender-Climb",
                RequiredCards = new List<int> { CardId.TGScrewSerpent },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.TGScrewSerpent, ActionType = ExecutorType.Summon, Description = "Normal Summon Screw Serpent" },
                    new() { CardId = CardId.TGScrewSerpent, ActionType = ExecutorType.Activate, Description = "Screw Serpent revives T.G. from GY" }
                }
            });
        }

        private void RegisterExecutors()
        {
            // ═══════════════════════════════════════════════════════════════
            //  TIER 0: COUNTER TRAPS, QUICK DISRUPTIONS & HANDTRAPS
            // ═══════════════════════════════════════════════════════════════

            // T.G. Close — Omni-negate Counter Trap
            AddExecutor(ExecutorType.Activate, CardId.TGClose, TGCloseActivate);

            // T.G. Halberd Cannon — Summon Negate
            AddExecutor(ExecutorType.Activate, CardId.TGHalberdCannon, TGHalberdCannonNegateActivate);

            // T.G. Glaive Blaster — Quick Extra Deck Monster Banish & Monster Steal
            AddExecutor(ExecutorType.Activate, CardId.TGGlaiveBlaster, TGGlaiveBlasterActivate);

            // Shooting Star Dragon T.G. EX — Target Protection Negate & Attack Negate
            AddExecutor(ExecutorType.Activate, CardId.ShootingStarDragonTGEX, ShootingStarTGEXActivate);

            // Handtraps
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveActivate);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomActivate);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, InfiniteImpermanenceActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 1: MAIN PHASE SPELLS & BOARD CLEAR
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, HarpiesFeatherDusterActivate);
            AddExecutor(ExecutorType.Activate, CardId.Bonfire, BonfireActivate);
            AddExecutor(ExecutorType.Activate, CardId.OneForOne, OneForOneActivate);
            AddExecutor(ExecutorType.Activate, CardId.TGLimiterRemoval, TGLimiterRemovalActivate);
            AddExecutor(ExecutorType.Activate, CardId.TGAllClear, TGAllClearActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: STARTER NORMAL SUMMONS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Summon, CardId.TGRocketSalamander, TGRocketSalamanderSummon);
            AddExecutor(ExecutorType.Activate, CardId.TGRocketSalamander, TGRocketSalamanderEffect);

            AddExecutor(ExecutorType.Summon, CardId.TGScrewSerpent, TGScrewSerpentSummon);
            AddExecutor(ExecutorType.Activate, CardId.TGScrewSerpent, TGScrewSerpentEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: HAND EXTENDER SPECIAL SUMMONS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.TGStriker, TGStrikerSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TGWarwolf, TGWarwolfEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.TGBoosterRaptor, TGBoosterRaptorSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.TGDrillFish, TGDrillFishSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TGGearZombie, TGGearZombieEffect);
            AddExecutor(ExecutorType.Activate, CardId.TGTankGrub, TGTankGrubEffect);

            // Fallback Normal Summons if no starter
            AddExecutor(ExecutorType.Summon, CardId.TGTankGrub, FallbackNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.TGWarwolf, FallbackNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.TGRushRhino, FallbackNormalSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: EXTRA DECK SYNCHRO & LINK PROGRESSIONS
            // ═══════════════════════════════════════════════════════════════

            // Synchro Summon Over Dragonar (Revives all T.G.)
            AddExecutor(ExecutorType.SpSummon, CardId.TGOverDragonar, TGOverDragonarSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TGOverDragonar, TGOverDragonarEffect);

            // Synchro Summon Mighty Striker (Searches All Clear / Close)
            AddExecutor(ExecutorType.SpSummon, CardId.TGMightyStriker, TGMightyStrikerSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TGMightyStriker, TGMightyStrikerEffect);

            // Link Summon Trident Launcher (Summons 3 T.G.)
            AddExecutor(ExecutorType.SpSummon, CardId.TGTridentLauncher, TGTridentLauncherSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TGTridentLauncher, TGTridentLauncherEffect);

            // Synchro Summon Star Guardian (Add from GY & SS from hand)
            AddExecutor(ExecutorType.SpSummon, CardId.TGStarGuardian, TGStarGuardianSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TGStarGuardian, TGStarGuardianEffect);

            // Synchro Summon Hyper Librarian (Draw engine)
            AddExecutor(ExecutorType.SpSummon, CardId.TGHyperLibrarian, TGHyperLibrarianSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TGHyperLibrarian, TGHyperLibrarianEffect);

            // Synchro Summon Wonder Magician (Pop S/T)
            AddExecutor(ExecutorType.SpSummon, CardId.TGWonderMagician, TGWonderMagicianSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TGWonderMagician, TGWonderMagicianEffect);

            // Synchro Summon Power Gladiator & Recipro Dragonfly
            AddExecutor(ExecutorType.SpSummon, CardId.TGPowerGladiator, TGPowerGladiatorSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.TGReciproDragonfly, TGReciproDragonflySpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TGReciproDragonfly, TGReciproDragonflyEffect);

            // Delta Accel & Accel Bosses
            AddExecutor(ExecutorType.SpSummon, CardId.TGGlaiveBlaster, TGGlaiveBlasterSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.TGHalberdCannon, TGHalberdCannonSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ShootingStarDragonTGEX, ShootingStarTGEXSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.TGBladeBlaster, TGBladeBlasterSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TGBladeBlaster, TGBladeBlasterEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 5: SPELL & TRAP SETTING / REPOSITIONING
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, CardId.TGClose, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.TGLimiterRemoval, SpellSetStrategy);

            AddExecutor(ExecutorType.Repos, RepositionStrategy);
        }

        // ═══════════════════════════════════════════════════════════════
        //  DISRUPTIONS & HANDTRAPS
        // ═══════════════════════════════════════════════════════════════

        private bool TGCloseActivate()
        {
            // Counter Trap: Negates Monster, Spell, or Trap activation while controlling Machine T.G.
            // Also triggers GY reset when T.G. Synchro is banished.
            if (Card.Location == CardLocation.Grave)
            {
                // GY effect to re-set itself
                return true;
            }

            if (Card.Location == CardLocation.SpellZone)
            {
                // Check if opponent is activating an effect
                ClientCard lastCard = LastChainCard;
                if (lastCard != null && lastCard.Controller == 1)
                {
                    // Ensure we control a Machine T.G. monster
                    bool hasMachineTG = Bot.GetMonsters().Any(m => m.IsFaceup() && (m.HasRace(CardRace.Machine) || Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.TGAllClear)));
                    if (hasMachineTG)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool TGHalberdCannonNegateActivate()
        {
            // Negate monster summon and destroy it
            return Duel.LastSummonPlayer == 1;
        }

        private bool TGGlaiveBlasterActivate()
        {
            // Effect 1 (Quick): Target 1 monster Special Summoned from Extra Deck; banish it.
            // Effect 2 (Trigger): When monster is banished face-up; Special Summon it to your field!
            if (Card.Location == CardLocation.MonsterZone)
            {
                // If trigger to summon banished monster:
                ClientCard bestBanished = Enemy.Banished.Concat(Bot.Banished)
                    .Where(c => c != null && c.IsMonster())
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();

                if (bestBanished != null)
                {
                    AI.SelectCard(bestBanished);
                    return true;
                }

                // Quick banish: Target opponent monster from Extra Deck
                ClientCard target = Enemy.GetMonsters()
                    .Where(m => m.IsFaceup() && (m.HasType(CardType.Fusion) || m.HasType(CardType.Synchro) || m.HasType(CardType.Xyz) || m.HasType(CardType.Link)))
                    .OrderByDescending(m => m.Attack)
                    .FirstOrDefault();

                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool ShootingStarTGEXActivate()
        {
            // 1. Negate effect targeting monster(s) we control by banishing 1 Tuner from GY
            // 2. Negate opponent attack
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle)
            {
                return true;
            }

            ClientCard lastCard = LastChainCard;
            if (lastCard != null && lastCard.Controller == 1)
            {
                ClientCard tunerInGrave = Bot.Graveyard.FirstOrDefault(c => c.HasType(CardType.Tuner));
                if (tunerInGrave != null)
                {
                    AI.SelectCard(tunerInGrave);
                    return true;
                }
            }
            return false;
        }

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
                ClientCard target = Enemy.Graveyard
                    .Where(c => c.IsMonster())
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();
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
            if (Duel.Player == 0 && !OpponentHasActiveNegator())
            {
                // Save Ash for enemy turn unless high threat
                return false;
            }

            ClientCard lastCard = LastChainCard;
            return lastCard != null && lastCard.Controller == 1;
        }

        private bool InfiniteImpermanenceActivate()
        {
            ClientCard target = Enemy.GetMonsters()
                .Where(m => m.IsFaceup() && !m.IsDisabled() && m.HasType(CardType.Effect))
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  SPELLS & DRAW/SEARCH ENGINE
        // ═══════════════════════════════════════════════════════════════

        private bool HarpiesFeatherDusterActivate()
        {
            return Enemy.GetSpellCount() > 0;
        }

        private bool BonfireActivate()
        {
            // Search Rocket Salamander (Pyro)
            if (!Bot.Hand.Any(c => c.Id == CardId.TGRocketSalamander))
            {
                AI.SelectCard(CardId.TGRocketSalamander);
                return true;
            }
            return false;
        }

        private bool OneForOneActivate()
        {
            // Discard 1 monster to Special Summon Rocket Salamander (Level 1) or Tank Grub
            ClientCard discard = Bot.Hand
                .Where(c => c.IsMonster() && c.Id != CardId.TGRocketSalamander && c.Id != CardId.TGScrewSerpent)
                .OrderBy(c => c.Attack)
                .FirstOrDefault();

            if (discard != null)
            {
                AI.SelectCard(discard);
                AI.SelectNextCard(CardId.TGRocketSalamander, CardId.TGTankGrub);
                return true;
            }
            return false;
        }

        private bool TGLimiterRemovalActivate()
        {
            // Discard 1; add 2 T.G. monsters from Deck with different names
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                ClientCard discard = Bot.Hand
                    .Where(c => c != Card)
                    .OrderBy(c => c.IsSpell() ? 1 : 0)
                    .FirstOrDefault();

                if (discard != null)
                {
                    AI.SelectCard(discard);
                    AI.SelectNextCard(CardId.TGRocketSalamander, CardId.TGScrewSerpent, CardId.TGWarwolf, CardId.TGStriker);
                    return true;
                }
            }

            // GY effect: shuffle 1 T.G. into deck or add to hand
            if (Card.Location == CardLocation.Grave)
            {
                ClientCard target = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.TGRocketSalamander || c.Id == CardId.TGScrewSerpent);
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool TGAllClearActivate()
        {
            // 1. Activate from hand to field
            if (Card.Location == CardLocation.Hand)
            {
                return !Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.TGAllClear);
            }

            // 2. On field: Destroy 1 T.G. monster to search 1 T.G. monster
            if (Card.Location == CardLocation.SpellZone)
            {
                ClientCard fodder = Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.TGToken || m.Id == CardId.TGTankGrub || m.Id == CardId.TGRushRhino)
                                 ?? Bot.Hand.FirstOrDefault(c => c.IsMonster() && c.Id != CardId.TGRocketSalamander && c.Id != CardId.TGScrewSerpent);

                if (fodder != null)
                {
                    AI.SelectCard(fodder);
                    AI.SelectNextCard(CardId.TGRocketSalamander, CardId.TGScrewSerpent, CardId.TGWarwolf);
                    return true;
                }
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  STARTERS & MONSTER EFFECTS
        // ═══════════════════════════════════════════════════════════════

        private bool TGRocketSalamanderSummon()
        {
            return true;
        }

        private bool TGRocketSalamanderEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Tribute self to Special Summon Screw Serpent from Deck!
                AI.SelectCard(Card);
                AI.SelectNextCard(CardId.TGScrewSerpent);
                return true;
            }
            return false;
        }

        private bool TGScrewSerpentSummon()
        {
            return true;
        }

        private bool TGScrewSerpentEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Revive Level 4 or lower T.G. from GY
                ClientCard target = Bot.Graveyard
                    .Where(c => c.Level <= 4 && c.Id != CardId.TGScrewSerpent)
                    .OrderByDescending(c => c.Id == CardId.TGRocketSalamander ? 10 : c.Level)
                    .FirstOrDefault();

                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool TGStrikerSpSummon()
        {
            return Enemy.GetMonsterCount() > 0 && Bot.GetMonsterCount() == 0;
        }

        private bool TGWarwolfEffect()
        {
            // Hand effect when L<=4 monster Special Summoned
            return Card.Location == CardLocation.Hand;
        }

        private bool TGBoosterRaptorSpSummon()
        {
            return Bot.GetMonsters().Any(m => m.IsFaceup());
        }

        private bool TGDrillFishSpSummon()
        {
            return Bot.GetMonsterCount() > 0 && Bot.GetMonsters().All(m => m.IsFaceup());
        }

        private bool TGGearZombieEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                ClientCard target = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.Attack >= 1000)
                                 ?? Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool TGTankGrubEffect()
        {
            // Trigger on sent as Synchro material -> spawn Level 1 Token
            return true;
        }

        private bool FallbackNormalSummon()
        {
            return Bot.GetMonsterCount() < 2;
        }

        // ═══════════════════════════════════════════════════════════════
        //  EXTRA DECK PROGRESSIONS
        // ═══════════════════════════════════════════════════════════════

        private bool TGOverDragonarSpSummon()
        {
            // Primary combo engine: Level 5 Synchro
            return true;
        }

        private bool TGOverDragonarEffect()
        {
            // Special Summon any number of T.G. monsters from GY!
            if (Card.Location == CardLocation.MonsterZone)
            {
                var targets = Bot.Graveyard
                    .Where(c => c.IsMonster())
                    .OrderByDescending(c => c.Attack)
                    .Take(5)
                    .ToList();

                if (targets.Count > 0)
                {
                    AI.SelectCard(targets);
                    return true;
                }
            }
            return true; // Draw 1 on destroy
        }

        private bool TGMightyStrikerSpSummon()
        {
            // Level 2 Synchro Tuner
            return true;
        }

        private bool TGMightyStrikerEffect()
        {
            // On Synchro: Add T.G. Spell/Trap (All Clear or Close)
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (!Bot.HasInSpellZone(CardId.TGAllClear) && !Bot.HasInHand(CardId.TGAllClear))
                {
                    AI.SelectCard(CardId.TGAllClear);
                }
                else
                {
                    AI.SelectCard(CardId.TGClose);
                }
                return true;
            }

            // On sent to GY: Foolish 1 T.G.
            if (Card.Location == CardLocation.Grave)
            {
                AI.SelectCard(CardId.TGScrewSerpent, CardId.TGTankGrub, CardId.TGRocketSalamander);
                return true;
            }
            return false;
        }

        private bool TGTridentLauncherSpSummon()
        {
            // Link-3: Requires 2+ Effect Monsters including a T.G. Tuner
            return true;
        }

        private bool TGTridentLauncherEffect()
        {
            // Special Summon 3 T.G. monsters: 1 Hand, 1 Deck, 1 GY
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Deck target
                AI.SelectCard(CardId.TGScrewSerpent, CardId.TGRocketSalamander, CardId.TGWarwolf, CardId.TGTankGrub);
                return true;
            }
            return false;
        }

        private bool TGStarGuardianSpSummon()
        {
            return true;
        }

        private bool TGStarGuardianEffect()
        {
            // 1. Add T.G. from GY to hand
            // 2. Special Summon T.G. from hand
            if (Card.Location == CardLocation.MonsterZone)
            {
                ClientCard gyTarget = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.TGScrewSerpent || c.Id == CardId.TGRocketSalamander);
                if (gyTarget != null)
                {
                    AI.SelectCard(gyTarget);
                }

                ClientCard handTarget = Bot.Hand.FirstOrDefault(c => c.IsMonster());
                if (handTarget != null)
                {
                    AI.SelectNextCard(handTarget);
                }
                return true;
            }
            return false;
        }

        private bool TGHyperLibrarianSpSummon()
        {
            return true;
        }

        private bool TGHyperLibrarianEffect()
        {
            // Draw 1 card on Synchro Summon
            return true;
        }

        private bool TGWonderMagicianSpSummon()
        {
            return true;
        }

        private bool TGWonderMagicianEffect()
        {
            // Destroy 1 opponent Spell/Trap
            ClientCard target = Enemy.GetSpells().FirstOrDefault(s => s.IsFaceup()) ?? Enemy.GetSpells().FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return true;
        }

        private bool TGReciproDragonflySpSummon()
        {
            return false; // situational de-synchro
        }

        private bool TGReciproDragonflyEffect()
        {
            return false;
        }

        private bool TGPowerGladiatorSpSummon()
        {
            return true;
        }

        private bool TGGlaiveBlasterSpSummon()
        {
            // Level 12 Delta Accel Boss (4000 ATK)
            return true;
        }

        private bool TGHalberdCannonSpSummon()
        {
            // Level 12 Delta Accel Boss (4000 ATK)
            return true;
        }

        private bool ShootingStarTGEXSpSummon()
        {
            // Level 10 Accel Boss (3300 ATK)
            return true;
        }

        private bool TGBladeBlasterSpSummon()
        {
            // Level 10 Accel Boss (3300 ATK)
            return true;
        }

        private bool TGBladeBlasterEffect()
        {
            // Negate S/T targeting this card by sending 1 card from hand
            return Bot.Hand.Count > 0;
        }

        // ═══════════════════════════════════════════════════════════════
        //  HEURISTICS & CARD SELECTION
        // ═══════════════════════════════════════════════════════════════

        private bool SpellSetStrategy()
        {
            if (Card.IsTrap()) return true;
            if (Card.IsSpell() && Card.HasType(CardType.QuickPlay))
            {
                return Duel.Phase == DuelPhase.Main2 || Bot.GetMonsterCount() > 0;
            }
            return false;
        }

        private bool RepositionStrategy()
        {
            if (Card.Attack < 1500 && Card.IsAttack())
            {
                return true;
            }
            if (Card.Attack >= 2000 && Card.IsDefense())
            {
                return true;
            }
            return false;
        }

        public override bool OnSelectYesNo(long desc)
        {
            // Guard against empty field triggers
            if (Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0)
            {
                return false;
            }
            return base.OnSelectYesNo(desc);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Prioritize best targets during prompts
            if (cards != null && cards.Count > 0)
            {
                var preferred = cards.Where(c => c.Id == CardId.TGScrewSerpent ||
                                                c.Id == CardId.TGRocketSalamander ||
                                                c.Id == CardId.TGClose ||
                                                c.Id == CardId.TGAllClear ||
                                                c.Id == CardId.TGWarwolf).ToList();
                if (preferred.Count >= min)
                {
                    return preferred.Take(max).ToList();
                }
            }
            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
