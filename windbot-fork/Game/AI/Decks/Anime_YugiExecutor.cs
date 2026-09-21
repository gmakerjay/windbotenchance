// ============================================================================
// CARD AUDIT — Anime_Yugi (Yugi's Ultimate Anime Battlebox Strategy)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Dark Magician                      | Normal L7    | No   | No    | None    | 2500 ATK core beatstick / fusion material     | Primary target for Curtain, Timaeus, Secrets  | Never tribute without benefit               |
// | Dark Magician, Pharaoh's Servant   | Monster L7   | Yes  | Yes   | Reveal  | SS by reveal Spell, Set DM S/T, Quick Duster  | Spell in hand; Quick duster opp backrow >= 1 | No spells in hand; opp backrow = 0          |
// | Dark Magician Girl                 | Monster L6   | No   | No    | None    | 2000 ATK + 300 per DM/MoBC in GY              | Material for Timaeus / Dark Magician Destr   | Tributes needed for Ritual                  |
// | Skull Archfiend of Chaos           | Monster L6   | Yes  | Yes   | Shuffle | SS by recycling 3 cards; dump Ritual Spell    | 3+ cards in GY/banish; search Ritual monster | GY/banish < 3                               |
// | Black Chaos                        | Monster L8   | Yes  | Yes   | Shuffle | SS by shuffling Ritual, banish 2 opp cards    | Ritual in hand/GY, opp has cards to banish   | Bot has no Ritual in hand/GY to recycle     |
// | Magician of Dark Chaos-Black Chaos | Ritual L8    | Yes  | Yes   | Tribute | Recover Spell from GY on SS; banish FD card   | Ritual Summoned via Light & Darkness Ritual  | GY has 0 Spells                             |
// | BLS - Soldier of Light & Darkness  | Ritual L8    | Yes  | Yes   | Tribute | Banish 1 card on SS; +1500 ATK & double atk   | Ritual Summoned via Light & Darkness Ritual  | Opponent has no cards and no battle targets |
// | Illusion of Chaos                  | Ritual L7    | Yes  | Yes   | Reveal  | Reveal -> search Pharaoh's Servant/DM         | In hand; search starter monster              | Already searched this turn                  |
// | Griffoh                            | Monster L1   | Yes  | Yes   | Discard | Hand Quick: Set Ritual S/T & activate turn    | Discard to Set Mind Shuffle/Box/Hats/Sword   | Already used Griffoh this turn              |
// | Detonating Kuriboh                 | Monster L1   | Yes  | Yes   | Equip   | Hand Quick: Equip to opp monster & negate     | Opp monster activates on field               | Opp activates in GY/hand                    |
// | Multiplying Kuriboh!               | Monster L1   | No   | No    | Discard | Hand Quick: Special Summon Kuriboh Tokens     | Opponent Special Summons (prevent OTK)       | Field full (zones = 0)                      |
// | Pot of Prosperity                  | Spell Normal | Yes  | Yes   | Banish  | Excavate 3/6, add 1, banish ED cards          | Main Phase 1 early starter                   | Hand already has full combo                 |
// | Pre-Preparation of Rites           | Spell Normal | Yes  | Yes   | None    | Add Light & Darkness Ritual + Ritual monster  | Deck has Ritual Spell & Ritual Monster       | No targets left in deck                     |
// | Preparation of Rites               | Spell Normal | No   | No    | None    | Add Level 7 or lower Ritual monster + GY Spell| Hand needs Illusion of Chaos                 | No Level <= 7 Ritual in deck                |
// | Light and Darkness Ritual          | Spell Ritual | Yes  | Yes   | Tribute | Ritual Summon MoDC or BLS from hand/GY banish | Hand has Ritual boss; GY recursion ready     | No valid tribute / materials                |
// | Dark Magical Curtain               | Spell Normal | Yes  | Yes   | None    | SS DARK Spellcaster from Dk + search DM S/T   | Bot needs DM/DMG on field                    | Opponent benefits heavily or bot locked     |
// | The Gaze of Timaeus                | Spell Quick  | Yes  | Yes   | Shuffle | Target DM/DMG on field/GY -> Fusion Summon   | DM/DMG available; SS Dragoon / Chimera       | No valid DM/DMG available                   |
// | Soul Servant                       | Spell Quick  | Yes  | Yes   | Banish  | Place DM card on deck top; banish GY draw    | Set top card or draw cards in Main Phase     | Deck empty or no valid targets              |
// | Secrets of Dark Magic              | Spell Quick  | No   | No    | Tribute | Quick Fusion or Ritual using DM/DMG          | During battle / chain to dodge removal       | No valid materials                          |
// | Chaos Space                        | Spell Normal | Yes  | Yes   | Discard | Discard Light/Dark -> Add opposite attribute  | Hand has discardable Light/Dark              | No valid target in Deck                     |
// | Triple Tactics Talent              | Spell Normal | Yes  | Yes   | None    | Draw 2 / Steal monster / Shuffle opp hand     | Opp activated monster eff in Main Phase      | Opp did not activate monster eff in MP      |
// | Swords of Concealing Light         | Spell Cont   | No   | No    | None    | Flip all opp monsters face-down Defense       | Opponent has 2+ dangerous face-up monsters   | Opponent has 0 face-up monsters             |
// | Spell Shattering Sword             | Spell Quick  | Yes  | Yes   | None    | Pop all opp face-up Spells OR negate+0 ATK    | Opp has face-up Spells or dangerous monster  | Opp has no targets                          |
// | Forbidden Crown                    | Spell Quick  | Yes  | Yes   | None    | Unrespondable negate + freeze opp monster     | Opp threat monster on field / combo starter  | Target already negated                      |
// | Chaos Mystic Box                   | Spell Quick  | No   | No    | Bounce  | Save targeted card, pop 1, SS Ritual boss     | Opp targets bot card; pop opp card           | Opp does not target bot card                |
// | Chaos Magical Hats                 | Spell Quick  | No   | No    | None    | Defend attack/effect, set 3 S/T as monsters  | Opp activates monster eff or normal S/T      | Bot has no DM/Ritual monster on field       |
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
            public const int DarkMagician = 46986414;
            public const int DarkMagicianThePharaohsServant = 88570003;
            public const int DarkMagicianGirl = 38033122;
            public const int SkullArchfiendOfChaos = 24088928;
            public const int BlackChaos = 98684220;
            public const int MagicianOfDarkChaosBlackChaos = 44001993;
            public const int BlackLusterSoldierSoldierOfLightAndDarkness = 70405001;
            public const int IllusionOfChaos = 12266229;
            public const int Griffoh = 97462632;
            public const int DetonatingKuriboh = 46789706;
            public const int MultiplyingKuriboh = 14965712;

            // Spells
            public const int PotOfProsperity = 84211599;
            public const int PreparationOfRites = 96729612;
            public const int PrePreparationOfRites = 13048472;
            public const int LightAndDarknessRitual = 33599853;
            public const int DarkMagicalCurtain = 41350417;
            public const int TheGazeOfTimaeus = 22283204;
            public const int SoulServant = 23020408;
            public const int SecretsOfDarkMagic = 59514116;
            public const int ChaosSpace = 99266988;
            public const int TripleTacticsTalent = 25311006;
            public const int SwordsOfConcealingLight = 12923641;
            public const int SpellShatteringSword_Old = 101402064;
            public const int SpellShatteringSword = 77456448;
            public const int ForbiddenCrown = 98829635;
            public const int ChaosMysticBox = 75983808;
            public const int ChaosMagicalHats = 2372506;

            // Traps
            public const int DominusImpulse = 40366667;
            public const int MindShuffle = 24749710;
            public const int DarkMagicTalisman = 71440209;

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

        // State tracking
        private bool _usedIllusionSearchThisTurn = false;
        private bool _spellActivatedThisTurn = false;
        private int _lastSearchedCardId = 0;

        public Anime_YugiExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            RegisterExecutors();
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _usedIllusionSearchThisTurn = false;
            _spellActivatedThisTurn = false;
            _lastSearchedCardId = 0;
        }

        private void RegisterExecutors()
        {
            // -------------------------------------------------------------
            // 1. High Priority Counters & Quick Interruptions (Chain Phase)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.RedEyesDarkDragoon, RedEyesDarkDragoonNegateActivate);
            AddExecutor(ExecutorType.Activate, CardId.DarkCavalry, DarkCavalryNegateActivate);
            AddExecutor(ExecutorType.Activate, CardId.DominusImpulse, DominusImpulseActivate);
            AddExecutor(ExecutorType.Activate, CardId.DarkMagicTalisman, DarkMagicTalismanActivate);
            AddExecutor(ExecutorType.Activate, CardId.DetonatingKuriboh, DetonatingKuribohActivate);
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
            AddExecutor(ExecutorType.Activate, CardId.DarkMagicianThePharaohsServant, PharaohsServantQuickWipeActivate);
            AddExecutor(ExecutorType.Activate, CardId.IllusionOfChaos, IllusionOfChaosFieldNegateActivate);

            // -------------------------------------------------------------
            // 3. Main Phase 1 Starters & Search Engine (Top Priority!)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.IllusionOfChaos, IllusionOfChaosHandSearchActivate);
            AddExecutor(ExecutorType.Activate, CardId.PreparationOfRites, PreparationOfRitesActivate);
            AddExecutor(ExecutorType.Activate, CardId.PrePreparationOfRites, PrePreparationOfRitesActivate);
            AddExecutor(ExecutorType.Activate, CardId.Griffoh, GriffohActivate);
            AddExecutor(ExecutorType.Activate, CardId.BlackChaos, BlackChaosHandActivate);
            AddExecutor(ExecutorType.Activate, CardId.MindShuffle, MindShuffleActivate);

            // -------------------------------------------------------------
            // 4. Special Summons & Combo Extensions
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.DarkMagicianThePharaohsServant, PharaohsServantHandSSActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.DarkMagicianOfDestruction, DarkMagicianOfDestructionSummon);
            AddExecutor(ExecutorType.Activate, CardId.DarkMagicianOfDestruction, DarkMagicianOfDestructionEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheGazeOfTimaeus, TheGazeOfTimaeusActivate);
            AddExecutor(ExecutorType.Activate, CardId.DarkMagicalCurtain, DarkMagicalCurtainActivate);
            AddExecutor(ExecutorType.Activate, CardId.LightAndDarknessRitual, LightAndDarknessRitualActivate);
            AddExecutor(ExecutorType.Activate, CardId.SoulServant, SoulServantActivate);
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, PotOfProsperityActivate);
            AddExecutor(ExecutorType.Activate, CardId.SecretsOfDarkMagic, SecretsOfDarkMagicActivate);
            AddExecutor(ExecutorType.Activate, CardId.ChaosSpace, ChaosSpaceActivate);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentActivate);

            // -------------------------------------------------------------
            // 5. Board Breakers & Black Chaos Summons
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.SwordsOfConcealingLight, SwordsOfConcealingLightActivate);
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
            // 8. Normal Summons (Guaranteed active, no impossible tribute locks)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Summon, CardId.Griffoh, GriffohSummon);
            AddExecutor(ExecutorType.Summon, CardId.DetonatingKuriboh, DetonatingKuribohSummon);
            AddExecutor(ExecutorType.Summon, CardId.MultiplyingKuriboh, MultiplyingKuribohSummon);
            AddExecutor(ExecutorType.Summon, CardId.DarkMagicianGirl, DarkMagicianGirlSummon);
            AddExecutor(ExecutorType.Summon, CardId.SkullArchfiendOfChaos, SkullArchfiendSummon);
            AddExecutor(ExecutorType.Summon, CardId.DarkMagicianThePharaohsServant, PharaohsServantNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.DarkMagician, DarkMagicianNormalSummon);

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

        private bool RedEyesDarkDragoonNegateActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            if (Duel.LastChainPlayer != 1) return false;
            return true;
        }

        private bool DarkCavalryNegateActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            if (Duel.LastChainPlayer != 1) return false;
            return true;
        }

        private bool DominusImpulseActivate()
        {
            if (Duel.LastChainPlayer != 1) return false;
            return true;
        }

        private bool DarkMagicTalismanActivate()
        {
            if (Duel.LastChainPlayer != 1) return false;
            return true;
        }

        private bool DetonatingKuribohActivate()
        {
            if (Duel.LastChainPlayer != 1) return false;
            ClientCard lastChainCard = Util.GetLastChainCard();
            if (lastChainCard != null && lastChainCard.Controller == 1 && lastChainCard.Location == CardLocation.MonsterZone)
            {
                return true;
            }
            return false;
        }

        private bool ForbiddenCrownActivate()
        {
            // Negate and freeze high threat face-up monster
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsDisabled() && (m.Attack >= 2000 || AntiFloodgateHelper.NegateMonsterIds.Contains(m.Id)));
            if (target != null)
            {
                AI.SelectCard(target);
                _spellActivatedThisTurn = true;
                return true;
            }
            if (Duel.LastChainPlayer == 1)
            {
                ClientCard chainCard = Util.GetLastChainCard();
                if (chainCard != null && chainCard.Location == CardLocation.MonsterZone && chainCard.IsFaceup())
                {
                    AI.SelectCard(chainCard);
                    _spellActivatedThisTurn = true;
                    return true;
                }
            }
            return false;
        }

        private bool ChaosMysticBoxActivate()
        {
            if (Duel.LastChainPlayer != 1) return false;
            _spellActivatedThisTurn = true;
            return true;
        }

        private bool ChaosMagicalHatsActivate()
        {
            if (Duel.LastChainPlayer != 1) return false;
            if (Bot.GetMonsters().Any(m => m.IsFaceup()))
            {
                _spellActivatedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool MultiplyingKuribohActivate()
        {
            if (Duel.LastChainPlayer != 1) return false;
            return Bot.GetMonsterCount() < 5;
        }

        private bool LinkuribohActivate()
        {
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
            if (Enemy.GetSpells().Any(s => s.IsFaceup()))
            {
                AI.SelectOption(0);
                _spellActivatedThisTurn = true;
                return true;
            }
            ClientCard oppBoss = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && (m.Attack >= 2000 || !m.IsDisabled()));
            if (oppBoss != null && (Bot.HasInHand(CardId.LightAndDarknessRitual) || Bot.HasInGraveyard(CardId.LightAndDarknessRitual)))
            {
                AI.SelectOption(1);
                AI.SelectCard(oppBoss);
                _spellActivatedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool PharaohsServantQuickWipeActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            if (Enemy.GetSpellCount() == 0) return false;
            if (!Bot.Hand.Any(c => c.IsSpell() && c.Id != CardId.LightAndDarknessRitual && c.Id != CardId.TheGazeOfTimaeus)) return false;
            return true;
        }

        private bool IllusionOfChaosFieldNegateActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            if (Duel.LastChainPlayer != 1) return false;
            return Bot.HasInGraveyard(CardId.DarkMagicianThePharaohsServant) || Bot.HasInGraveyard(CardId.DarkMagician);
        }

        private bool IllusionOfChaosHandSearchActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_usedIllusionSearchThisTurn) return false;

            // Search starter: Pharaoh's Servant > Dark Magician > Dark Magician Girl
            _lastSearchedCardId = CardId.DarkMagicianThePharaohsServant;
            AI.SelectCard(new[] {
                CardId.DarkMagicianThePharaohsServant,
                CardId.DarkMagician,
                CardId.DarkMagicianGirl
            });
            _usedIllusionSearchThisTurn = true;
            return true;
        }

        private bool PreparationOfRitesActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            AI.SelectCard(CardId.IllusionOfChaos);
            _spellActivatedThisTurn = true;
            return true;
        }

        private bool PrePreparationOfRitesActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            AI.SelectCard(new[] {
                CardId.MagicianOfDarkChaosBlackChaos,
                CardId.BlackLusterSoldierSoldierOfLightAndDarkness
            });
            _spellActivatedThisTurn = true;
            return true;
        }

        private bool GriffohActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // Option 1: Set 1 Quick-Play Spell or Trap mentioning Light and Darkness Ritual from Deck!
            AI.SelectOption(1);
            // Preferred Set order: Mind Shuffle (if none on field), then defensive Quick-Plays
            bool hasMindShuffle = Bot.SpellZone.Any(s => s != null && s.Id == CardId.MindShuffle);
            if (!hasMindShuffle)
            {
                AI.SelectCard(CardId.MindShuffle);
            }
            else
            {
                AI.SelectCard(new[] {
                    CardId.ChaosMysticBox,
                    CardId.ChaosMagicalHats,
                    CardId.SpellShatteringSword_Old,
                    CardId.SpellShatteringSword
                });
            }
            return true;
        }

        private bool BlackChaosHandActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // Discard to place Mind Shuffle face-up from deck or GY
            bool hasMindShuffle = Bot.SpellZone.Any(s => s != null && s.Id == CardId.MindShuffle);
            if (hasMindShuffle) return false; // Don't discard if Mind Shuffle is already active
            return true;
        }

        private bool MindShuffleActivate()
        {
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                // In MP: Search monster mentioning Light & Darkness Ritual, discard 1
                AI.SelectCard(new[] {
                    CardId.BlackChaos,
                    CardId.MagicianOfDarkChaosBlackChaos,
                    CardId.BlackLusterSoldierSoldierOfLightAndDarkness,
                    CardId.SkullArchfiendOfChaos,
                    CardId.Griffoh
                });
                return true;
            }
            return true;
        }

        private bool PharaohsServantHandSSActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // Reveal 1 Spell in hand to Special Summon itself
            ClientCard spellToReveal = Bot.Hand.FirstOrDefault(c => c.IsSpell() && c != Card);
            if (spellToReveal == null) return false;

            // Hand reveal target
            AI.SelectCard(spellToReveal);
            // Deck set target
            AI.SelectCard(new[] {
                CardId.TheGazeOfTimaeus,
                CardId.SoulServant,
                CardId.SecretsOfDarkMagic,
                CardId.DarkMagicTalisman
            });
            return true;
        }

        private bool DarkMagicianOfDestructionSummon()
        {
            // Alternative summon by banishing Level 6+ DARK Spellcaster during turn a Spell was activated
            if (!_spellActivatedThisTurn && !Bot.SpellZone.Any(s => s != null) && !Bot.Graveyard.Any(c => c.IsSpell()))
            {
                // Verify if any spell was activated
                if (!_spellActivatedThisTurn) return false;
            }

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
            // On SS: add The Gaze of Timaeus or DM card
            AI.SelectCard(new[] {
                CardId.TheGazeOfTimaeus,
                CardId.SecretsOfDarkMagic,
                CardId.SoulServant,
                CardId.DarkMagicianThePharaohsServant,
                CardId.DarkMagician
            });
            return true;
        }

        private bool TheGazeOfTimaeusActivate()
        {
            // Target DM/DMG on field or GY -> Fusion Summon Dragoon or Dragon Knight
            bool hasTarget = Bot.GetMonsters().Concat(Bot.Graveyard).Any(m => m.IsFaceup() || m.Location == CardLocation.Grave &&
                (m.Id == CardId.DarkMagicianThePharaohsServant || m.Id == CardId.DarkMagician || m.Id == CardId.DarkMagicianGirl || m.Id == CardId.DarkMagicianOfDestruction));
            if (!hasTarget) return false;

            AI.SelectCard(new[] {
                CardId.DarkMagicianOfDestruction,
                CardId.DarkMagicianThePharaohsServant,
                CardId.DarkMagician,
                CardId.DarkMagicianGirl
            });

            // Extra Deck priority: Dragoon > Dragon Knight > Master of Chaos > Chimera > Cavalry
            bool hasDragoon = Bot.GetMonsters().Any(m => m.IsFaceup() && m.Id == CardId.RedEyesDarkDragoon);
            if (!hasDragoon)
            {
                AI.SelectCard(CardId.RedEyesDarkDragoon);
            }
            else
            {
                AI.SelectCard(new[] {
                    CardId.DarkMagicianTheDragonKnight,
                    CardId.MasterOfChaos,
                    CardId.GuardianChimera,
                    CardId.DarkCavalry,
                    CardId.TheDarkMagicians
                });
            }

            _spellActivatedThisTurn = true;
            return true;
        }

        private bool DarkMagicalCurtainActivate()
        {
            // Special Summons DARK Spellcaster (prioritize original Dark Magician for search!)
            AI.SelectCard(new[] {
                CardId.DarkMagician,
                CardId.DarkMagicianThePharaohsServant,
                CardId.DarkMagicianGirl
            });
            _spellActivatedThisTurn = true;
            return true;
        }

        private bool LightAndDarknessRitualActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Hand Ritual Summon
                AI.SelectCard(new[] {
                    CardId.MagicianOfDarkChaosBlackChaos,
                    CardId.BlackLusterSoldierSoldierOfLightAndDarkness
                });
                _spellActivatedThisTurn = true;
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // GY recovery: return itself + 1 card mentioning Light & Darkness Ritual
                AI.SelectCard(new[] {
                    CardId.MindShuffle,
                    CardId.Griffoh,
                    CardId.SkullArchfiendOfChaos
                });
                return true;
            }
            return false;
        }

        private bool SoulServantActivate()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                // Place DM card on top of deck
                AI.SelectCard(new[] {
                    CardId.TheGazeOfTimaeus,
                    CardId.DarkMagicianThePharaohsServant,
                    CardId.DarkMagician,
                    CardId.SecretsOfDarkMagic,
                    CardId.DarkMagicalCurtain
                });
                _spellActivatedThisTurn = true;
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Draw cards equal to DM/DMG in field/GY
                int dmCount = Bot.GetMonsters().Concat(Bot.Graveyard).Count(m => m.Id == CardId.DarkMagician || m.Id == CardId.DarkMagicianGirl || m.Id == CardId.DarkMagicianThePharaohsServant || m.Id == CardId.DarkMagicianOfDestruction);
                return dmCount > 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2);
            }
            return false;
        }

        private bool PotOfProsperityActivate()
        {
            if (Bot.ExtraDeck.Count < 3) return false;
            // Banish 3 or 6 non-essential Extra Deck cards
            AI.SelectOption(Bot.ExtraDeck.Count >= 6 ? 1 : 0);
            AI.SelectCard(new[] {
                CardId.EbonHighMagician,
                CardId.MagiMagiMagicianGal,
                CardId.DayBreakerTheShiningMagicalWarrior,
                CardId.BlackLusterSoldierSoldierOfChaos,
                CardId.DarkMagicianOfDestruction,
                CardId.DarkCavalry
            });
            _spellActivatedThisTurn = true;
            return true;
        }

        private bool SecretsOfDarkMagicActivate()
        {
            AI.SelectOption(0); // Fusion Summon
            AI.SelectCard(new[] {
                CardId.RedEyesDarkDragoon,
                CardId.GuardianChimera,
                CardId.MasterOfChaos,
                CardId.TheDarkMagicians
            });
            _spellActivatedThisTurn = true;
            return true;
        }

        private bool ChaosSpaceActivate()
        {
            _spellActivatedThisTurn = true;
            return true;
        }

        private bool TripleTacticsTalentActivate()
        {
            if (Enemy.GetMonsters().Any(m => m.IsFaceup() && m.Attack >= 2500))
            {
                AI.SelectOption(1); // Steal monster
            }
            else
            {
                AI.SelectOption(0); // Draw 2
            }
            _spellActivatedThisTurn = true;
            return true;
        }

        private bool SwordsOfConcealingLightActivate()
        {
            if (Enemy.GetMonsters().Any(m => m.IsFaceup()))
            {
                _spellActivatedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool BlackChaosSummon()
        {
            // Shuffles 1 Spellcaster/Warrior Ritual monster from GY or Hand
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
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup())
            {
                // Non-targeting double banish!
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

        private bool RedEyesDarkDragoonPopActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
            return Enemy.GetMonsters().Any(m => m.IsFaceup());
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
            ClientCard target = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault() ?? Enemy.GetSpells().FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return true;
        }

        private bool MasterOfChaosEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            AI.SelectCard(new[] {
                CardId.RedEyesDarkDragoon,
                CardId.MagicianOfDarkChaosBlackChaos,
                CardId.BlackLusterSoldierSoldierOfLightAndDarkness,
                CardId.DarkMagicianThePharaohsServant,
                CardId.DarkMagician
            });
            return true;
        }

        private bool GuardianChimeraEffect() => true;
        private bool TheDarkMagiciansEffect() => true;

        private bool TimaeusDragonEffect()
        {
            AI.SelectCard(new[] {
                CardId.TheGazeOfTimaeus,
                CardId.SecretsOfDarkMagic,
                CardId.SoulServant
            });
            return true;
        }

        private bool MagiMagiMagicianGalActivate()
        {
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

        private bool DarkMagicianGirlSummon()
        {
            // Level 6 tribute summon: requires 1 tribute on field
            if (Bot.GetMonsterCount() == 0) return false;
            // Tribute weak tokens or level 1 monsters
            ClientCard tribute = Bot.GetMonsters().FirstOrDefault(m => m.Level == 1 || m.Attack < 1500);
            if (tribute != null)
            {
                AI.SelectCard(tribute);
                return true;
            }
            return false;
        }

        private bool SkullArchfiendSummon()
        {
            // Level 6 tribute summon: requires 1 tribute on field
            if (Bot.GetMonsterCount() == 0) return false;
            ClientCard tribute = Bot.GetMonsters().FirstOrDefault(m => m.Level == 1 || m.Attack < 1500);
            if (tribute != null)
            {
                AI.SelectCard(tribute);
                return true;
            }
            return false;
        }

        private bool PharaohsServantNormalSummon()
        {
            // Level 7 tribute summon: requires 2 tributes
            if (Bot.GetMonsterCount() < 2) return false;
            List<ClientCard> tributes = Bot.GetMonsters().Where(m => m.Attack < 2000).Take(2).ToList();
            if (tributes.Count == 2)
            {
                AI.SelectCard(tributes);
                return true;
            }
            return false;
        }

        private bool DarkMagicianNormalSummon()
        {
            // Level 7 tribute summon: requires 2 tributes
            if (Bot.GetMonsterCount() < 2) return false;
            List<ClientCard> tributes = Bot.GetMonsters().Where(m => m.Attack < 2000).Take(2).ToList();
            if (tributes.Count == 2)
            {
                AI.SelectCard(tributes);
                return true;
            }
            return false;
        }

        private bool SpellSetStrategy()
        {
            // Set Traps in MP2 or keep 1 Dominus in hand as handtrap
            if (Card.Id == CardId.DominusImpulse)
            {
                // If we have multiple Dominus Impulse in hand, set 1, keep 1 in hand
                int countInHand = Bot.Hand.Count(c => c.Id == CardId.DominusImpulse);
                return countInHand > 1 || Duel.Phase == DuelPhase.Main2;
            }

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
            if (Card.Attack < 1500 && Card.IsAttack()) return true;
            if (Card.Attack >= 2000 && Card.IsDefense()) return true;
            return false;
        }

        // =================================================================
        // INTELLIGENT HOOK OVERRIDES (OnSelectCard, OnSelectOption, OnSelectPosition, OnSelectYesNo)
        // Strictly adheres to SKILL.md v10.0 Section 7.3 Hint Table
        // =================================================================

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0) return 0;

            for (int i = 0; i < options.Count; i++)
            {
                long cardId = options[i] >> 4;
                if (cardId == 0 && Card != null) cardId = Card.Id;
                long optIndex = options[i] & 0xf;

                // 1. Spell Shattering Sword (77456448 / 101402064)
                // Option 0: Destroy all face-up Spells opp controls
                // Option 1: Reveal Light and Darkness Ritual, negate monster + ATK to 0
                if (cardId == CardId.SpellShatteringSword || cardId == CardId.SpellShatteringSword_Old)
                {
                    bool hasOppFaceupSpells = Enemy.GetSpells().Any(s => s != null && s.IsFaceup());
                    if (hasOppFaceupSpells && optIndex == 0)
                    {
                        DecisionTracer.Trace("OnSelectOption", $"Spell Shattering Sword: Option {optIndex} (Wipe face-up Spells)");
                        return i;
                    }
                    if (!hasOppFaceupSpells && optIndex == 1)
                    {
                        DecisionTracer.Trace("OnSelectOption", $"Spell Shattering Sword: Option {optIndex} (Negate monster & ATK 0)");
                        return i;
                    }
                }

                // 2. Dark Magician, the Pharaoh's Servant (88570003)
                // Option 0: Destroy all monsters opponent controls
                // Option 1: Destroy all Spells/Traps opponent controls
                if (cardId == CardId.DarkMagicianThePharaohsServant)
                {
                    if (Enemy.GetMonsterCount() > 0 && optIndex == 0)
                    {
                        DecisionTracer.Trace("OnSelectOption", $"Pharaoh's Servant: Option {optIndex} (Wipe all opponent monsters)");
                        return i;
                    }
                    if (Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() > 0 && optIndex == 1)
                    {
                        DecisionTracer.Trace("OnSelectOption", $"Pharaoh's Servant: Option {optIndex} (Wipe all opponent Spells/Traps)");
                        return i;
                    }
                }

                // 3. Master of Chaos (85059922)
                // Option 0: Banish all monsters opponent controls
                // Option 1: Add 1 Spell from GY to hand
                if (cardId == CardId.MasterOfChaos)
                {
                    if (Enemy.GetMonsterCount() > 0 && optIndex == 0)
                    {
                        DecisionTracer.Trace("OnSelectOption", $"Master of Chaos: Option {optIndex} (Banish all monsters)");
                        return i;
                    }
                    if (optIndex == 1)
                    {
                        DecisionTracer.Trace("OnSelectOption", $"Master of Chaos: Option {optIndex} (Add Spell from GY)");
                        return i;
                    }
                }

                // 4. Mind Shuffle (24749710)
                // Option 0: Return card from GY to deck
                // Option 1: Shuffle hand into deck and draw
                if (cardId == CardId.MindShuffle)
                {
                    if (optIndex == 0)
                    {
                        DecisionTracer.Trace("OnSelectOption", $"Mind Shuffle: Option {optIndex} (Recycle from GY)");
                        return i;
                    }
                }

                // 5. Triple Tactics Talent (25311006)
                // Option 0: Draw 2
                // Option 1: Take control of 1 monster
                // Option 2: Look at opponent's hand & shuffle 1
                if (cardId == CardId.TripleTacticsTalent)
                {
                    bool hasHighAtkMonster = Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Attack >= 2500);
                    if (hasHighAtkMonster && optIndex == 1)
                    {
                        DecisionTracer.Trace("OnSelectOption", $"Triple Tactics Talent: Option {optIndex} (Take control of high ATK monster)");
                        return i;
                    }
                    if (optIndex == 0)
                    {
                        DecisionTracer.Trace("OnSelectOption", $"Triple Tactics Talent: Option {optIndex} (Draw 2 cards)");
                        return i;
                    }
                }

                // 6. Griffoh (97462632)
                // Option 0: Add Ritual Spell to hand
                // Option 1: Set 1 Quick-Play Spell or Trap from deck
                if (cardId == CardId.Griffoh)
                {
                    bool hasRitualSpell = Bot.Hand.Concat(Bot.Graveyard).Any(c => c != null && c.Id == CardId.LightAndDarknessRitual);
                    if (!hasRitualSpell && optIndex == 0)
                    {
                        DecisionTracer.Trace("OnSelectOption", $"Griffoh: Option {optIndex} (Add Ritual Spell to hand)");
                        return i;
                    }
                    if (optIndex == 1)
                    {
                        DecisionTracer.Trace("OnSelectOption", $"Griffoh: Option {optIndex} (Set Quick-Play/Trap from deck)");
                        return i;
                    }
                }

                // 7. Pot of Prosperity (84211599)
                // Option 0: Banish 3
                // Option 1: Banish 6
                if (cardId == CardId.PotOfProsperity)
                {
                    if (Bot.ExtraDeck.Count >= 6 && optIndex == 1) return i;
                    if (optIndex == 0) return i;
                }

                // 8. Secrets of Dark Magic (59514116)
                // Option 0: Fusion Summon
                // Option 1: Ritual Summon
                if (cardId == CardId.SecretsOfDarkMagic)
                {
                    if (optIndex == 0) return i;
                }

                // 9. Black Luster Soldier - Soldier of Chaos (49202162)
                // Option 0: Gain 1500 ATK
                // Option 1: Double Attack next turn
                // Option 2: Banish 1 card on the field
                if (cardId == CardId.BlackLusterSoldierSoldierOfChaos)
                {
                    if (Enemy.GetMonsterCount() + Enemy.GetSpellCount() > 0 && optIndex == 2) return i;
                    if (optIndex == 0) return i;
                }

                // 10. Magi Magi ☆ Magician Gal (10000030)
                // Option 0: Take control of 1 monster
                // Option 1: Special Summon 1 monster from opponent's GY
                if (cardId == CardId.MagiMagiMagicianGal)
                {
                    if (Enemy.GetMonsterCount() > 0 && optIndex == 0) return i;
                    if (optIndex == 1) return i;
                }
            }

            return base.OnSelectOption(options);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Low ATK / utility monsters -> FaceUpDefence (Stat-Aware)
            if (cardId == CardId.Griffoh || cardId == CardId.DetonatingKuriboh ||
                cardId == CardId.MultiplyingKuriboh || cardId == CardId.Linkuriboh)
            {
                if (positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence;
            }

            // High ATK beaters / Ace monsters -> FaceUpAttack
            if (cardId == CardId.RedEyesDarkDragoon || cardId == CardId.MasterOfChaos ||
                cardId == CardId.DarkMagicianTheDragonKnight || cardId == CardId.GuardianChimera ||
                cardId == CardId.DarkMagicianOfDestruction || cardId == CardId.DarkCavalry ||
                cardId == CardId.BlackChaos || cardId == CardId.BlackLusterSoldierSoldierOfLightAndDarkness ||
                cardId == CardId.BlackLusterSoldierSoldierOfChaos || cardId == CardId.DarkMagician ||
                cardId == CardId.DarkMagicianThePharaohsServant || cardId == CardId.DarkMagicianGirl)
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            // Tokens first
            if (c.HasType(CardType.Token)) return 0;
            // Level 1 Kuribohs / Griffoh
            if (c.Level == 1) return 10;
            // Monsters in Graveyard
            if (c.Location == CardLocation.Grave)
            {
                if (c.Id == CardId.DarkMagician) return 30;
                return 20;
            }
            // Monsters in Hand
            if (c.Location == CardLocation.Hand)
            {
                if (c.Id == CardId.DarkMagicianGirl) return 40;
                if (c.Id == CardId.DarkMagician) return 50;
                return 45;
            }
            // Protect Bosses on field
            if (c.Id == CardId.RedEyesDarkDragoon || c.Id == CardId.MasterOfChaos ||
                c.Id == CardId.DarkMagicianTheDragonKnight || c.Id == CardId.GuardianChimera)
                return 999;
            if (c.Id == CardId.DarkMagicianOfDestruction || c.Id == CardId.DarkCavalry)
                return 900;
            if (c.Id == CardId.DarkMagicianThePharaohsServant || c.Id == CardId.DarkMagician)
                return 800;

            return 100;
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // Hint 500: HINTMSG_RELEASE (Tribute fodder/tokens first, never sacrifice Ace)
            if (hint == 500)
            {
                var sorted = cards.OrderBy(c => c.Controller == 1 ? 0 : GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }

            // Hint 501: HINTMSG_DISCARD (Discard GY triggers or duplicates, protect starters)
            if (hint == 501)
            {
                // Discard Light and Darkness Ritual (has GY effect to recover itself + another card!)
                var ritual = cards.FirstOrDefault(c => c != null && c.Id == CardId.LightAndDarknessRitual);
                if (ritual != null) return new List<ClientCard> { ritual };

                var duplicateDM = cards.Where(c => c != null && (c.Id == CardId.DarkMagician || c.Id == CardId.DarkMagicianThePharaohsServant)).ToList();
                if (duplicateDM.Count > 1) return new List<ClientCard> { duplicateDM[0] };

                var sorted = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }

            // Hint 502: HINTMSG_DESTROY (Destroy highest threat, skip destruction immune)
            if (hint == 502)
            {
                var enemyTargets = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyTargets.Count > 0)
                {
                    var validTargets = enemyTargets.Where(c => !IsDestructionImmune(c)).ToList();
                    var pool = validTargets.Count > 0 ? validTargets : enemyTargets;
                    var sorted = pool.OrderByDescending(c => GetCardThreatScore(c)).ToList();
                    return sorted.Take(max).ToList();
                }
            }

            // Hint 504 / 503: HINTMSG_REMOVE (Banish highest threat, skip target-immune)
            if (hint == 504 || hint == 503)
            {
                var enemyTargets = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyTargets.Count > 0)
                {
                    var validTargets = enemyTargets.Where(c => IsViableEffectTarget(c)).ToList();
                    var pool = validTargets.Count > 0 ? validTargets : enemyTargets;
                    var sorted = pool.OrderByDescending(c => GetCardThreatScore(c)).ToList();
                    return sorted.Take(max).ToList();
                }

                // Self banish costs (Soul Servant in GY, Black Chaos in GY)
                var soulServantGY = cards.FirstOrDefault(c => c != null && c.Location == CardLocation.Grave && c.Id == CardId.SoulServant);
                if (soulServantGY != null) return new List<ClientCard> { soulServantGY };
            }

            // Case: Pharaoh's Servant - revealing 1 Spell in hand (Hint 526)
            if (hint == 526 && cards.All(c => c != null && c.Location == CardLocation.Hand && c.IsSpell()))
            {
                var toReveal = cards.OrderByDescending(c =>
                {
                    if (c.Id == CardId.SoulServant) return 100;
                    if (c.Id == CardId.PrePreparationOfRites) return 90;
                    if (c.Id == CardId.PreparationOfRites) return 80;
                    if (c.Id == CardId.TheGazeOfTimaeus) return 70;
                    if (c.Id == CardId.DarkMagicalCurtain) return 60;
                    if (c.Id == CardId.ForbiddenCrown) return 50;
                    if (c.Id == CardId.SwordsOfConcealingLight) return 40;
                    return 10;
                }).FirstOrDefault();

                if (toReveal != null) return new List<ClientCard> { toReveal };
            }

            // Hint 505: HINTMSG_ATOHAND / RTOHAND (Search to hand or bounce enemy threat)
            // Hint 506: HINTMSG_TODECK (Spin to deck or recycle GY materials / Illusion of Chaos place on top)
            if (hint == 505 || hint == 506 || cards.Any(c => c != null && c.Location == CardLocation.Deck))
            {
                // Sub-case: Illusion of Chaos - placing 1 card from hand on top of deck
                if (cards.All(c => c != null && c.Location == CardLocation.Hand))
                {
                    var toReturn = cards
                        .Where(c => c.Id != _lastSearchedCardId && c.Id != CardId.TheGazeOfTimaeus && c.Id != CardId.DarkMagicianThePharaohsServant)
                        .OrderByDescending(c =>
                        {
                            if (c.Id == CardId.DarkMagicalCurtain && Bot.Hand.Count(x => x.Id == CardId.DarkMagicalCurtain) > 1) return 100;
                            if (c.Id == CardId.DominusImpulse && Bot.Hand.Count(x => x.Id == CardId.DominusImpulse) > 1) return 90;
                            if (c.Id == CardId.SwordsOfConcealingLight) return 80;
                            if (c.Id == CardId.MindShuffle) return 70;
                            if (c.Id == CardId.ChaosSpace) return 60;
                            if (c.Id == CardId.BlackChaos && !Bot.Hand.Any(x => x.HasType(CardType.Ritual))) return 50;
                            return 10;
                        })
                        .FirstOrDefault();

                    if (toReturn != null) return new List<ClientCard> { toReturn };
                }

                // If bouncing or spinning enemy cards:
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyCards.Count > 0)
                {
                    var sorted = enemyCards.OrderByDescending(c => GetCardThreatScore(c)).ToList();
                    return sorted.Take(max).ToList();
                }

                // Search priority
                var searchPriority = new List<int>
                {
                    CardId.PrePreparationOfRites,
                    CardId.PreparationOfRites,
                    CardId.IllusionOfChaos,
                    CardId.TheGazeOfTimaeus,
                    CardId.Griffoh,
                    CardId.DarkMagicianThePharaohsServant,
                    CardId.DarkMagician,
                    CardId.SecretsOfDarkMagic,
                    CardId.DarkMagicalCurtain,
                    CardId.SoulServant,
                    CardId.LightAndDarknessRitual,
                    CardId.ForbiddenCrown,
                    CardId.DominusImpulse
                };

                foreach (int sid in searchPriority)
                {
                    var match = cards.FirstOrDefault(c => c != null && c.Id == sid);
                    if (match != null) return new List<ClientCard> { match };
                }
            }

            // Hint 507: HINTMSG_EQUIP (Equip to best boss monster)
            if (hint == 507)
            {
                var bosses = cards.Where(c => c != null && c.Controller == 0 && c.IsFaceup()).OrderByDescending(c => c.Attack).ToList();
                if (bosses.Count > 0) return bosses.Take(max).ToList();
            }

            // Hint 508: HINTMSG_TOGRAVE (Send combo extenders / triggers to GY)
            if (hint == 508)
            {
                var dumpPriority = new[]
                {
                    CardId.LightAndDarknessRitual,
                    CardId.SoulServant,
                    CardId.DarkMagician,
                    CardId.DarkMagicianGirl,
                    CardId.Griffoh
                };

                foreach (int did in dumpPriority)
                {
                    var match = cards.FirstOrDefault(c => c != null && c.Id == did);
                    if (match != null) return new List<ClientCard> { match };
                }
            }

            // Hint 509: HINTMSG_SPSUMMON (Special Summon Ace / Negator / Key Extender)
            if (hint == 509)
            {
                var preferred = new[]
                {
                    CardId.RedEyesDarkDragoon,
                    CardId.MasterOfChaos,
                    CardId.DarkMagicianTheDragonKnight,
                    CardId.GuardianChimera,
                    CardId.DarkMagicianOfDestruction,
                    CardId.DarkCavalry,
                    CardId.DarkMagicianThePharaohsServant,
                    CardId.DarkMagician,
                    CardId.DarkMagicianGirl
                };

                foreach (int pid in preferred)
                {
                    var match = cards.FirstOrDefault(c => c != null && c.Id == pid);
                    if (match != null) return new List<ClientCard> { match };
                }
            }

            // Hint 518: HINTMSG_POSCHANGE (Stat-Aware position change)
            if (hint == 518)
            {
                var sorted = cards.OrderBy(c => c.Attack > c.Defense ? 0 : 1).ToList();
                return sorted.Take(max).ToList();
            }

            // Hint 519: HINTMSG_XMATERIAL (Detach non-Ace / fodder materials first)
            if (hint == 519)
            {
                var sorted = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }

            // Hint 552 / 572: HINTMSG_DISABLE / NEGATE (Target key chokepoint or negator)
            if (hint == 552 || hint == 572)
            {
                var enemyTargets = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyTargets.Count > 0)
                {
                    var sorted = enemyTargets.OrderByDescending(c =>
                        CardIntelligence.IsHighThreatChokepoint(c.Id) ? 1000 :
                        CardIntelligence.IsKnownNegator(c.Id) ? 900 :
                        GetCardThreatScore(c)).ToList();
                    return sorted.Take(max).ToList();
                }
            }

            // Hint 511 / 513 / 533: Fusion, Ritual, Release materials
            if (hint == 511 || hint == 513 || hint == 533)
            {
                var sorted = cards.OrderBy(c => c.Controller == 1 ? 0 : GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override bool OnSelectYesNo(long desc)
        {
            // Always accept Dark Magical Curtain Special Summon and search
            long cardIdFromDesc4 = (desc >> 4);
            if (cardIdFromDesc4 == CardId.DarkMagicalCurtain)
            {
                return true;
            }

            return base.OnSelectYesNo(desc);
        }
    }
}
