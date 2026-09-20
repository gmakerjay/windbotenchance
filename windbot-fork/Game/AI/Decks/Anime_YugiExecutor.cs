// ============================================================================
// CARD AUDIT — Anime_Yugi (Yugi's Ultimate Anime Battlebox Strategy)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Black Chaos                        | Monster L8   | Yes  | Yes   | Shuffle | SS by shuffling Ritual, banish 2 opp cards    | Ritual in hand/GY, opp has cards to banish   | Bot has no Ritual in hand/GY to recycle     |
// | Dark Magician, Pharaoh's Servant   | Monster L7   | Yes  | Yes   | Reveal  | SS by reveal Spell, Set DM S/T, Quick Duster  | Spell in hand; Quick duster opp backrow >= 1 | No spells in hand; opp backrow = 0          |
// | Skull Archfiend of Chaos           | Monster L6   | Yes  | Yes   | Shuffle | SS by recycling 3 cards; dump Ritual Spell    | 3+ cards in GY/banish; search Ritual monster | GY/banish < 3                               |
// | Dark Magician Girl                 | Monster L6   | No   | No    | None    | 2000 ATK + 300 per DM/MoBC in GY              | Material for Timaeus / Dark Magician Destr   | Tributes needed for Ritual                  |
// | Detonating Kuriboh                 | Monster L1   | Yes  | Yes   | Equip   | Hand Quick: Equip to opp monster & negate     | Opp monster activates on field               | Opp activates in GY/hand                    |
// | Griffoh                            | Monster L1   | Yes  | Yes   | Discard | Hand Quick: Set Ritual S/T & activate turn    | Discard to Set Mind Shuffle/Box/Hats/Sword   | Already used Griffoh this turn              |
// | Multiplying Kuriboh!               | Monster L1   | No   | No    | Discard | Hand Quick: Special Summon Kuriboh Tokens     | Opponent Special Summons (prevent OTK)       | Field full (zones = 0)                      |
// | BLS - Soldier of Light & Darkness  | Ritual L8    | Yes  | Yes   | Tribute | Banish 1 card on SS; +1500 ATK & double atk   | Ritual Summoned via Light & Darkness Ritual  | Opponent has no cards and no battle targets |
// | Magician of Dark Chaos-Black Chaos | Ritual L8    | Yes  | Yes   | Tribute | Recover Spell from GY on SS; banish FD card   | Ritual Summoned via Light & Darkness Ritual  | GY has 0 Spells                             |
// | Illusion of Chaos                  | Ritual L7    | Yes  | Yes   | Reveal  | Reveal -> search Pharaoh's Servant/DM         | In hand; search starter monster              | Already searched this turn                  |
// | Pot of Prosperity                  | Spell Normal | Yes  | Yes   | Banish  | Excavate 3/6, add 1, banish ED cards          | Main Phase 1 early starter                   | Hand already has full combo                 |
// | Pre-Preparation of Rites           | Spell Normal | Yes  | Yes   | None    | Add Light & Darkness Ritual + Ritual monster  | Deck has Ritual Spell & Ritual Monster       | No targets left in deck                     |
// | Preparation of Rites               | Spell Normal | No   | No    | None    | Add Level 7 or lower Ritual monster + GY Spell| Hand needs Illusion of Chaos                 | No Level <= 7 Ritual in deck                |
// | Triple Tactics Talent              | Spell Normal | Yes  | Yes   | None    | Draw 2 / Steal monster / Shuffle opp hand     | Opp activated monster eff in Main Phase      | Opp did not activate monster eff in MP      |
// | Soul Servant                       | Spell Quick  | Yes  | Yes   | Banish  | Place DM card on deck top; banish GY draw    | Set top card or draw cards in Main Phase     | Deck empty or no valid targets              |
// | Dark Magical Curtain               | Spell Normal | Yes  | Yes   | None    | SS DARK Spellcaster from Dk + search DM S/T   | Bot needs DM/DMG on field                    | Opponent benefits heavily or bot locked     |
// | Forbidden Crown                    | Spell Quick  | Yes  | Yes   | None    | Unrespondable negate + freeze opp monster     | Opp threat monster on field / combo starter  | Target already negated                      |
// | Spell Shattering Sword             | Spell Quick  | Yes  | Yes   | None    | Pop all opp face-up Spells OR negate+0 ATK    | Opp has face-up Spells or dangerous monster  | Opp has no targets                          |
// | The Gaze of Timaeus                | Spell Quick  | Yes  | Yes   | Shuffle | Target DM/DMG on field/GY -> Fusion Summon   | DM/DMG available; SS Dragoon / Chimera       | No valid DM/DMG available                   |
// | Swords of Concealing Light         | Spell Cont   | No   | No    | None    | Flip all opp monsters face-down Defense       | Opponent has 2+ dangerous face-up monsters   | Opponent has 0 face-up monsters             |
// | Light and Darkness Ritual          | Spell Ritual | Yes  | Yes   | Tribute | Ritual Summon MoDC or BLS from hand/GY banish | Hand has Ritual boss; GY recursion ready     | No valid tribute / materials                |
// | Chaos Space                        | Spell Normal | Yes  | Yes   | Discard | Discard Light/Dark -> Add opposite attribute  | Hand has discardable Light/Dark              | No valid target in Deck                     |
// | Secrets of Dark Magic              | Spell Quick  | No   | No    | Tribute | Quick Fusion or Ritual using DM/DMG          | During battle / chain to dodge removal       | No valid materials                          |
// | Chaos Magical Hats                 | Spell Quick  | No   | No    | None    | Defend attack/effect, set 3 S/T as monsters  | Opp activates monster eff or normal S/T      | Bot has no DM/Ritual monster on field       |
// | Chaos Mystic Box                   | Spell Quick  | No   | No    | Bounce  | Save targeted card, pop 1, SS Ritual boss     | Opp targets bot card; pop opp card           | Opp does not target bot card                |
// | Dominus Impulse                    | Trap Normal  | Yes  | Yes   | None    | Handtrap negate of Special Summoning effect   | Opp activates card/eff that special summons  | Opp effect does not special summon          |
// | Mind Shuffle                       | Trap Cont    | Yes  | Yes   | Discard | Search Ritual monster; bounce L7+ to SS boss | In play / Set; search and dodge removal      | Already used both effects this turn         |
// | Dark Magic Talisman                | Trap Normal  | No   | No    | 2500 LP | Negate monster eff responding to DM card      | Opp chains monster effect to DM card         | No DM effect being chained                  |
// | Red-Eyes Dark Dragoon              | Fusion L8    | Yes  | Yes   | Discard | Omni-negate + pop monster & burn ATK         | Opp activates card/eff; MP monster pop       | Hand empty for negate; opp has 0 monsters   |
// | Master of Chaos                    | Fusion L8    | Yes  | Yes   | Tribute | Revive Light/Dark on SS; banish all opp mons  | On SS revive; tribute Light+Dark to wipe field| Opp has no monsters                         |
// | Dark Magician the Dragon Knight    | Fusion L8    | No   | No    | None    | Backrow cannot be targeted or destroyed       | Protect continuous backrow and sets          | Already on field                            |
// | Dark Cavalry                       | Fusion L8    | No   | No    | Discard | Discard to negate card/eff that targets      | Opp targets card on field                    | Opp does not target                         |
// | Dark Magician of Destruction       | Fusion L8    | Yes  | Yes   | Banish  | SS by banishing L6+ DARK Spellcaster; search  | Spell activated this turn; search DM piece   | No L6+ DARK Spellcaster on field            |
// | Guardian Chimera                   | Fusion L9    | Yes  | Yes   | None    | On SS: Draw cards & pop opp cards             | Fusion summoned using hand & field materials | Opp has no cards on field                   |
// | The Dark Magicians                 | Fusion L8    | Yes  | Yes   | None    | Draw card on S/T activation; floats to DM+DMG | S/T activated; draw and set Quick/Trap       | Already drawn this turn                     |
// | Linkuriboh                         | Link 1       | Yes  | Yes   | Tribute | Reduce opp attacking monster ATK to 0        | Opp monster declares attack                  | Bot has stronger monster in battle          |
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
    [Deck("Anime_Yugi", "Anime_Yugi")]
    public class Anime_YugiExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int BlackChaos = 98684220;
            public const int DarkMagicianThePharaohsServant = 88570003;
            public const int SkullArchfiendOfChaos = 24088928;
            public const int DarkMagicianGirl = 38033122;
            public const int DetonatingKuriboh = 46789706;
            public const int Griffoh = 97462632;
            public const int MultiplyingKuriboh = 14965712;
            public const int BlackLusterSoldierSoldierOfLightAndDarkness = 70405001;
            public const int MagicianOfDarkChaosBlackChaos = 44001993;
            public const int IllusionOfChaos = 12266229;

            // Spells
            public const int PotOfProsperity = 84211599;
            public const int PreparationOfRites = 96729612;
            public const int TripleTacticsTalent = 25311006;
            public const int ChaosMagicalHats = 2372506;
            public const int ChaosMysticBox = 75983808;
            public const int DarkMagicalCurtain = 41350417;
            public const int ForbiddenCrown = 98829635;
            public const int SoulServant = 23020408;
            public const int SpellShatteringSword_Old = 101402064;
            public const int SpellShatteringSword = 77456448;
            public const int TheGazeOfTimaeus = 22283204;
            public const int SwordsOfConcealingLight = 12923641;
            public const int LightAndDarknessRitual = 33599853;
            public const int PrePreparationOfRites = 13048472;
            public const int SecretsOfDarkMagic = 59514116;
            public const int ChaosSpace = 99266988;

            // Traps
            public const int DarkMagicTalisman = 71440209;
            public const int DominusImpulse = 40366667;
            public const int MindShuffle = 24749710;

            // Extra Deck
            public const int TimaeusTheUnitedMagicalDragon = 85899505;
            public const int GuardianChimera = 11321089;
            public const int DarkMagicianTheDragonKnight = 41721210;
            public const int MasterOfChaos = 85059922;
            public const int RedEyesDarkDragoon = 37818794;
            public const int DarkCavalry = 73452089;
            public const int DarkMagicianOfDestruction = 59400890;
            public const int TheDarkMagicians = 50237654;
            public const int MagiMagiMagicianGal = 10000030;
            public const int BlackLusterSoldierSoldierOfChaos = 49202162;
            public const int DayBreakerTheShiningMagicalWarrior = 91336701;
            public const int Linkuriboh = 41999284;
            public const int EbonHighMagician = 96471335;
        }

        public Anime_YugiExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Register priority rules
            RegisterExecutors();
        }

        private void RegisterExecutors()
        {
            // -------------------------------------------------------------
            // 1. High Priority Handtraps & Counter Disruption (Chain Phase)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.DominusImpulse, DominusImpulseActivate);
            AddExecutor(ExecutorType.Activate, CardId.DarkMagicTalisman, DarkMagicTalismanActivate);
            AddExecutor(ExecutorType.Activate, CardId.DetonatingKuriboh, DetonatingKuribohActivate);
            AddExecutor(ExecutorType.Activate, CardId.RedEyesDarkDragoon, RedEyesDarkDragoonNegateActivate);
            AddExecutor(ExecutorType.Activate, CardId.DarkCavalry, DarkCavalryNegateActivate);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenCrown, ForbiddenCrownActivate);
            AddExecutor(ExecutorType.Activate, CardId.ChaosMysticBox, ChaosMysticBoxActivate);
            AddExecutor(ExecutorType.Activate, CardId.ChaosMagicalHats, ChaosMagicalHatsActivate);
            AddExecutor(ExecutorType.Activate, CardId.MultiplyingKuriboh, MultiplyingKuribohActivate);
            AddExecutor(ExecutorType.Activate, CardId.Linkuriboh, LinkuribohActivate);

            // -------------------------------------------------------------
            // 2. Spell Speed 2 Quick Plays / Interruption on Enemy Turn
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.SpellShatteringSword_Old, SpellShatteringSwordActivate);
            AddExecutor(ExecutorType.Activate, CardId.SpellShatteringSword, SpellShatteringSwordActivate);
            AddExecutor(ExecutorType.Activate, CardId.Griffoh, GriffohActivate);
            AddExecutor(ExecutorType.Activate, CardId.MindShuffle, MindShuffleActivate);
            AddExecutor(ExecutorType.Activate, CardId.DarkMagicianThePharaohsServant, PharaohsServantQuickWipeActivate);
            AddExecutor(ExecutorType.Activate, CardId.IllusionOfChaos, IllusionOfChaosFieldNegateActivate);

            // -------------------------------------------------------------
            // 3. Main Phase 1 Consistency & Search Starters
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, PotOfProsperityActivate);
            AddExecutor(ExecutorType.Activate, CardId.PrePreparationOfRites, PrePreparationOfRitesActivate);
            AddExecutor(ExecutorType.Activate, CardId.PreparationOfRites, PreparationOfRitesActivate);
            AddExecutor(ExecutorType.Activate, CardId.IllusionOfChaos, IllusionOfChaosHandSearchActivate);
            AddExecutor(ExecutorType.Activate, CardId.SoulServant, SoulServantActivate);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentActivate);
            AddExecutor(ExecutorType.Activate, CardId.DarkMagicalCurtain, DarkMagicalCurtainActivate);
            AddExecutor(ExecutorType.Activate, CardId.ChaosSpace, ChaosSpaceActivate);

            // -------------------------------------------------------------
            // 4. Main Phase Board Breakers & Spells
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.SwordsOfConcealingLight, SwordsOfConcealingLightActivate);

            // -------------------------------------------------------------
            // 5. Special Summons & Combos
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.DarkMagicianThePharaohsServant, PharaohsServantHandSSActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.DarkMagicianOfDestruction, DarkMagicianOfDestructionSummon);
            AddExecutor(ExecutorType.Activate, CardId.DarkMagicianOfDestruction, DarkMagicianOfDestructionEffect);
            AddExecutor(ExecutorType.Activate, CardId.LightAndDarknessRitual, LightAndDarknessRitualActivate);
            AddExecutor(ExecutorType.Activate, CardId.TheGazeOfTimaeus, TheGazeOfTimaeusActivate);
            AddExecutor(ExecutorType.Activate, CardId.SecretsOfDarkMagic, SecretsOfDarkMagicActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.BlackChaos, BlackChaosSummon);
            AddExecutor(ExecutorType.Activate, CardId.BlackChaos, BlackChaosEffect);
            AddExecutor(ExecutorType.Activate, CardId.SkullArchfiendOfChaos, SkullArchfiendOfChaosEffect);

            // -------------------------------------------------------------
            // 6. Extra Deck & Boss Ignition Effects
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.RedEyesDarkDragoon, RedEyesDarkDragoonPopActivate);
            AddExecutor(ExecutorType.Activate, CardId.MagicianOfDarkChaosBlackChaos, MagicianOfDarkChaosEffect);
            AddExecutor(ExecutorType.Activate, CardId.BlackLusterSoldierSoldierOfLightAndDarkness, BLSSoldierOfLightAndDarknessEffect);
            AddExecutor(ExecutorType.Activate, CardId.MasterOfChaos, MasterOfChaosEffect);
            AddExecutor(ExecutorType.Activate, CardId.GuardianChimera, GuardianChimeraEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheDarkMagicians, TheDarkMagiciansEffect);
            AddExecutor(ExecutorType.Activate, CardId.TimaeusTheUnitedMagicalDragon, TimaeusDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.MagiMagiMagicianGal, MagiMagiMagicianGalActivate);
            AddExecutor(ExecutorType.Activate, CardId.DayBreakerTheShiningMagicalWarrior, DayBreakerActivate);
            AddExecutor(ExecutorType.Activate, CardId.BlackLusterSoldierSoldierOfChaos, BLSSoldierOfChaosActivate);

            // -------------------------------------------------------------
            // 7. Extra Deck Link Summons
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.SpSummon, CardId.Linkuriboh, LinkuribohSummon);

            // -------------------------------------------------------------
            // 8. Normal Summons & Sets
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Summon, CardId.Griffoh, GriffohSummon);
            AddExecutor(ExecutorType.Summon, CardId.DetonatingKuriboh, DetonatingKuribohSummon);
            AddExecutor(ExecutorType.Summon, CardId.MultiplyingKuriboh, MultiplyingKuribohSummon);
            AddExecutor(ExecutorType.Summon, CardId.SkullArchfiendOfChaos, SkullArchfiendSummon);
            AddExecutor(ExecutorType.Summon, CardId.DarkMagicianGirl, DarkMagicianGirlSummon);
            AddExecutor(ExecutorType.Summon, CardId.DarkMagicianThePharaohsServant, PharaohsServantNormalSummon);

            // -------------------------------------------------------------
            // 9. Spell & Trap Setting
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.SpellSet, SpellSetStrategy);

            // Reposition monsters safely
            AddExecutor(ExecutorType.Repos, RepositionStrategy);
        }

        // =================================================================
        // EXECUTION IMPLEMENTATIONS
        // =================================================================

        private bool DominusImpulseActivate()
        {
            // Negate opponent card/effect that includes Special Summoning
            if (Duel.LastChainPlayer != 1) return false;
            return true;
        }

        private bool DarkMagicTalismanActivate()
        {
            // Negates monster effect activated in response to Dark Magician card
            if (Duel.LastChainPlayer != 1) return false;
            return true;
        }

        private bool DetonatingKuribohActivate()
        {
            // When opponent activates monster effect on field, equip to negate
            if (Duel.LastChainPlayer != 1) return false;
            ClientCard lastChainCard = Util.GetLastChainCard();
            if (lastChainCard != null && lastChainCard.Controller == 1 && lastChainCard.Location == CardLocation.MonsterZone)
            {
                return true;
            }
            return false;
        }

        private bool RedEyesDarkDragoonNegateActivate()
        {
            // Discard 1 card to negate activation and destroy
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            if (Duel.LastChainPlayer != 1) return false;
            return true;
        }

        private bool RedEyesDarkDragoonPopActivate()
        {
            // In MP: pop opponent monster and burn
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
            return Enemy.GetMonsters().Any(m => m.IsFaceup());
        }

        private bool DarkCavalryNegateActivate()
        {
            // Negate effect that targets card on field
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            if (Duel.LastChainPlayer != 1) return false;
            return true;
        }

        private bool ForbiddenCrownActivate()
        {
            // Negate and freeze high threat face-up monster
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsDisabled() && (m.Attack >= 2000 || AntiFloodgateHelper.NegateMonsterIds.Contains(m.Id)));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            if (Duel.LastChainPlayer == 1)
            {
                ClientCard chainCard = Util.GetLastChainCard();
                if (chainCard != null && chainCard.Location == CardLocation.MonsterZone && chainCard.IsFaceup())
                {
                    AI.SelectCard(chainCard);
                    return true;
                }
            }
            return false;
        }

        private bool ChaosMysticBoxActivate()
        {
            // Quick-Play: protects targeted card, pops opponent card, SS ritual
            if (Duel.LastChainPlayer != 1) return false;
            return true;
        }

        private bool ChaosMagicalHatsActivate()
        {
            // Disrupts opponent monster effect or normal S/T
            if (Duel.LastChainPlayer != 1) return false;
            return Bot.GetMonsters().Any(m => m.IsFaceup());
        }

        private bool MultiplyingKuribohActivate()
        {
            // Special Summons defensive tokens on opponent SS
            if (Duel.LastChainPlayer != 1) return false;
            return Bot.GetMonsterCount() < 5;
        }

        private bool LinkuribohActivate()
        {
            // Reduce attacking monster ATK to 0 or revive from GY
            if (Card.Location == CardLocation.MonsterZone)
            {
                return Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle;
            }
            if (Card.Location == CardLocation.Grave)
            {
                return Bot.GetMonsters().Any(m => m.Level == 1 && m.Id != CardId.Linkuriboh);
            }
            return false;
        }

        private bool SpellShatteringSwordActivate()
        {
            // Option 1: Destroy all face-up Spells opp controls
            if (Enemy.GetSpells().Any(s => s.IsFaceup()))
            {
                AI.SelectOption(0);
                return true;
            }
            // Option 2: Drop opp monster ATK to 0 and negate
            ClientCard oppBoss = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && (m.Attack >= 2000 || !m.IsDisabled()));
            if (oppBoss != null && (Bot.HasInHand(CardId.LightAndDarknessRitual) || Bot.HasInGraveyard(CardId.LightAndDarknessRitual)))
            {
                AI.SelectOption(1);
                AI.SelectCard(oppBoss);
                return true;
            }
            return false;
        }

        private bool GriffohActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // Option 2: Set 1 Quick-Play Spell or Trap mentioning Light and Darkness Ritual from Deck!
            AI.SelectOption(1);
            // Preferred Set order: Mind Shuffle, Chaos Mystic Box, Chaos Magical Hats, Spell Shattering Sword
            AI.SelectCard(new[] {
                CardId.MindShuffle,
                CardId.ChaosMysticBox,
                CardId.ChaosMagicalHats,
                CardId.SpellShatteringSword_Old,
                CardId.SpellShatteringSword
            });
            return true;
        }

        private bool MindShuffleActivate()
        {
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                // Effect 1: Search monster mentioning Light & Darkness Ritual, discard 1
                AI.SelectCard(new[] {
                    CardId.BlackChaos,
                    CardId.MagicianOfDarkChaosBlackChaos,
                    CardId.BlackLusterSoldierSoldierOfLightAndDarkness,
                    CardId.SkullArchfiendOfChaos,
                    CardId.Griffoh
                });
                return true;
            }
            // Activate face-up or chain dodge
            return true;
        }

        private bool PharaohsServantQuickWipeActivate()
        {
            // On field: Discard 1 Spell -> Feather Duster wipe opponent Spells/Traps
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            if (Enemy.GetSpellCount() == 0) return false;
            if (!Bot.Hand.Any(c => c.IsSpell() && c.Id != CardId.LightAndDarknessRitual && c.Id != CardId.TheGazeOfTimaeus)) return false;
            return true;
        }

        private bool IllusionOfChaosFieldNegateActivate()
        {
            // On field Quick: bounce to hand, SS Dark Magician from GY, negate monster effect
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            if (Duel.LastChainPlayer != 1) return false;
            return Bot.HasInGraveyard(CardId.DarkMagicianThePharaohsServant);
        }

        private bool PotOfProsperityActivate()
        {
            if (Bot.ExtraDeck.Count < 3) return false;
            // Banish 3 or 6 non-essential Extra Deck cards
            AI.SelectOption(Bot.ExtraDeck.Count >= 6 ? 1 : 0);
            AI.SelectCard(new[] {
                CardId.DarkMagicianOfDestruction,
                CardId.DarkMagicianOfDestruction,
                CardId.DayBreakerTheShiningMagicalWarrior,
                CardId.MagiMagiMagicianGal,
                CardId.BlackLusterSoldierSoldierOfChaos,
                CardId.EbonHighMagician
            });
            return true;
        }

        private bool PrePreparationOfRitesActivate()
        {
            // Add Light & Darkness Ritual + Ritual monster mentioned on it
            AI.SelectCard(new[] {
                CardId.MagicianOfDarkChaosBlackChaos,
                CardId.BlackLusterSoldierSoldierOfLightAndDarkness
            });
            return true;
        }

        private bool PreparationOfRitesActivate()
        {
            // Add Level 7 or lower Ritual (Illusion of Chaos)
            AI.SelectCard(CardId.IllusionOfChaos);
            return true;
        }

        private bool IllusionOfChaosHandSearchActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // Reveal in hand -> Add Dark Magician or non-ritual monster mentioning it
            AI.SelectCard(CardId.DarkMagicianThePharaohsServant);
            return true;
        }

        private bool SoulServantActivate()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                // Place DM card on top of deck
                AI.SelectCard(new[] {
                    CardId.TheGazeOfTimaeus,
                    CardId.DarkMagicianThePharaohsServant,
                    CardId.SecretsOfDarkMagic,
                    CardId.DarkMagicalCurtain
                });
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Banish from GY to draw
                return Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2;
            }
            return false;
        }

        private bool TripleTacticsTalentActivate()
        {
            // If opponent activated monster effect in Main Phase
            if (Enemy.GetMonsters().Any(m => m.IsFaceup() && m.Attack >= 2500))
            {
                AI.SelectOption(1); // Take control of opponent monster
                return true;
            }
            AI.SelectOption(0); // Draw 2 cards
            return true;
        }

        private bool DarkMagicalCurtainActivate()
        {
            // SS Dark Magician / DMG from deck, search DM S/T
            AI.SelectCard(CardId.DarkMagicianThePharaohsServant);
            return true;
        }

        private bool ChaosSpaceActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Discard Light or Dark to add opposite attribute
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Shuffle banished monster to draw 1
                return true;
            }
            return false;
        }

        private bool SwordsOfConcealingLightActivate()
        {
            // Flips all opponent monsters face-down
            return Enemy.GetMonsters().Any(m => m.IsFaceup());
        }

        private bool PharaohsServantHandSSActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // Reveal 1 Spell in hand -> SS itself, Set DM Spell/Trap from Deck
            if (!Bot.Hand.Any(c => c.IsSpell() && c != Card)) return false;
            AI.SelectCard(new[] {
                CardId.TheGazeOfTimaeus,
                CardId.SecretsOfDarkMagic,
                CardId.SoulServant,
                CardId.DarkMagicTalisman
            });
            return true;
        }

        private bool DarkMagicianOfDestructionSummon()
        {
            // Alternative summon by banishing Level 6+ DARK Spellcaster
            ClientCard material = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.Level >= 6 && m.HasAttribute(CardAttribute.Dark) && m.HasRace(CardRace.SpellCaster));
            if (material != null)
            {
                AI.SelectCard(material);
                return true;
            }
            return false;
        }

        private bool DarkMagicianOfDestructionEffect()
        {
            // On SS: add DM or card mentioning it
            AI.SelectCard(new[] {
                CardId.TheGazeOfTimaeus,
                CardId.DarkMagicianThePharaohsServant,
                CardId.SecretsOfDarkMagic,
                CardId.SoulServant
            });
            return true;
        }

        private bool LightAndDarknessRitualActivate()
        {
            // Ritual Summon Magician of Dark Chaos or BLS Soldier of Light & Darkness
            AI.SelectCard(new[] {
                CardId.MagicianOfDarkChaosBlackChaos,
                CardId.BlackLusterSoldierSoldierOfLightAndDarkness
            });
            // Select Griffoh or lowest level monsters as tribute / banish
            return true;
        }

        private bool TheGazeOfTimaeusActivate()
        {
            // Target DM/DMG on field or GY -> Fusion Summon Dragoon or Dragon Knight
            AI.SelectCard(new[] {
                CardId.DarkMagicianThePharaohsServant,
                CardId.DarkMagicianGirl,
                CardId.DarkMagicianOfDestruction
            });
            // Fusion priority: Dragoon > Dark Magician the Dragon Knight > Master of Chaos > Dark Cavalry
            AI.SelectCard(new[] {
                CardId.RedEyesDarkDragoon,
                CardId.DarkMagicianTheDragonKnight,
                CardId.MasterOfChaos,
                CardId.DarkCavalry,
                CardId.TheDarkMagicians
            });
            return true;
        }

        private bool SecretsOfDarkMagicActivate()
        {
            // Fusion or Ritual summon using DM / DMG
            AI.SelectOption(0); // Fusion Summon
            AI.SelectCard(new[] {
                CardId.RedEyesDarkDragoon,
                CardId.GuardianChimera,
                CardId.MasterOfChaos,
                CardId.TheDarkMagicians
            });
            return true;
        }

        private bool BlackChaosSummon()
        {
            // Special Summon by shuffling Ritual Monster from hand or GY into deck
            ClientCard ritualInGrave = Bot.Graveyard.FirstOrDefault(c => c.HasType(CardType.Ritual) && (c.HasRace(CardRace.SpellCaster) || c.HasRace(CardRace.Warrior)));
            if (ritualInGrave != null)
            {
                AI.SelectCard(ritualInGrave);
                return true;
            }
            ClientCard ritualInHand = Bot.Hand.FirstOrDefault(c => c.HasType(CardType.Ritual) && (c.HasRace(CardRace.SpellCaster) || c.HasRace(CardRace.Warrior)));
            if (ritualInHand != null)
            {
                AI.SelectCard(ritualInHand);
                return true;
            }
            return false;
        }

        private bool BlackChaosEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Discard to place Mind Shuffle face-up on field
                return true;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Ignition: Banish 2 cards opponent controls!
                if (Enemy.GetMonsterCount() + Enemy.GetSpellCount() > 0)
                {
                    List<ClientCard> targets = Enemy.GetMonsters().Where(m => m.IsFaceup()).OrderByDescending(m => m.Attack).Concat(Enemy.GetSpells()).ToList();
                    if (targets.Count > 0)
                    {
                        AI.SelectCard(targets);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool SkullArchfiendOfChaosEffect()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave)
            {
                // Shuffle 3 cards to SS itself
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Sent to GY: Dump Light and Darkness Ritual -> search Ritual monster
                AI.SelectCard(CardId.LightAndDarknessRitual);
                AI.SelectCard(new[] {
                    CardId.MagicianOfDarkChaosBlackChaos,
                    CardId.BlackLusterSoldierSoldierOfLightAndDarkness
                });
                return true;
            }
            return false;
        }

        private bool MagicianOfDarkChaosEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            // On SS: add Spell from GY
            AI.SelectCard(new[] {
                CardId.TheGazeOfTimaeus,
                CardId.LightAndDarknessRitual,
                CardId.SoulServant,
                CardId.SecretsOfDarkMagic,
                CardId.PrePreparationOfRites
            });
            // Ignition: banish 1 opponent card face-down
            ClientCard target = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault() ?? Enemy.GetSpells().FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return true;
        }

        private bool BLSSoldierOfLightAndDarknessEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            // On SS: banish 1 card opponent controls
            ClientCard target = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault() ?? Enemy.GetSpells().FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            // Battle destroy: +1500 ATK and second attack
            return true;
        }

        private bool MasterOfChaosEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            // On SS: revive Light or Dark monster from GY
            AI.SelectCard(new[] {
                CardId.RedEyesDarkDragoon,
                CardId.MagicianOfDarkChaosBlackChaos,
                CardId.BlackLusterSoldierSoldierOfLightAndDarkness,
                CardId.DarkMagicianThePharaohsServant
            });
            // Tribute Light + Dark to banish all opponent monsters
            if (Enemy.GetMonsterCount() >= 2)
            {
                return true;
            }
            return true;
        }

        private bool GuardianChimeraEffect()
        {
            // Draws cards and destroys opponent cards on summon
            return true;
        }

        private bool TheDarkMagiciansEffect()
        {
            // Draws card and sets Quick-Play/Trap
            return true;
        }

        private bool TimaeusDragonEffect()
        {
            // Sets DM spell that can be activated this turn
            AI.SelectCard(new[] {
                CardId.TheGazeOfTimaeus,
                CardId.SecretsOfDarkMagic,
                CardId.SoulServant
            });
            return true;
        }

        private bool MagiMagiMagicianGalActivate()
        {
            // Steals opponent monster or summons from opp GY
            AI.SelectOption(0);
            ClientCard oppMonster = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault(m => m.IsFaceup());
            if (oppMonster != null)
            {
                AI.SelectCard(oppMonster);
                return true;
            }
            return false;
        }

        private bool DayBreakerActivate()
        {
            // Remove 2 counters to pop card
            ClientCard target = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault() ?? Enemy.GetSpells().FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool BLSSoldierOfChaosActivate()
        {
            // On battle destroy: banish 1 card on field or +1500 ATK
            AI.SelectOption(2); // Banish 1 card on field
            ClientCard target = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault() ?? Enemy.GetSpells().FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
            }
            return true;
        }

        private bool LinkuribohSummon()
        {
            // Link Summon Linkuriboh using Level 1 Kuriboh/Griffoh
            return Bot.GetMonsters().Any(m => m.Level == 1 && m.Id != CardId.Linkuriboh);
        }

        private bool GriffohSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool DetonatingKuribohSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool MultiplyingKuribohSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool SkullArchfiendSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool DarkMagicianGirlSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool PharaohsServantNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool SpellSetStrategy()
        {
            if (Card.IsTrap()) return true;
            if (Card.IsSpell() && Card.HasType(CardType.QuickPlay))
            {
                // Set Quick-Plays in Main 2 before ending turn if not activated
                return Duel.Phase == DuelPhase.Main2;
            }
            return false;
        }

        private bool RepositionStrategy()
        {
            // Keep defensive tokens or low ATK monsters in Defense position
            if (Card.Attack < 1500 && Card.IsAttack())
            {
                return true;
            }
            // Switch strong monsters to Attack
            if (Card.Attack >= 2000 && Card.IsDefense())
            {
                return true;
            }
            return false;
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
