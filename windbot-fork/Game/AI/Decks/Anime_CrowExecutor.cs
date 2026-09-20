// ============================================================================
// CARD AUDIT — Anime_Crow (Crow Hogan's Ultimate Blackwing Synchro & Burn Army)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Ash Blossom & Joyous Spring        | Monster L3 T | Yes  | Yes   | Discard | Handtrap: Negate deck search/dump/SS          | Opponent activates deck-interacting effect    | Bot's own turn without threat               |
// | Infinite Impermanence              | Trap Normal  | Yes  | Yes   | None    | Negate 1 face-up monster; column S/T negate   | Opponent monster activates or dangerous boss  | Target already negated                      |
// | Called by the Grave                | Spell Quick  | Yes  | Yes   | Target  | Banish monster in opp GY and negate effects   | Opp activates handtrap or dangerous GY effect | No target in opp GY                         |
// | Harpie's Feather Duster            | Spell Normal | No   | No    | None    | Destroy all Spells and Traps opponent controls| Opponent controls 1+ Spell/Trap cards         | Opponent controls 0 Spells/Traps            |
// | Allure of Darkness                 | Spell Normal | No   | No    | Draw/Ban| Draw 2 cards, then banish 1 DARK monster      | Main Phase 1 draw engine                      | No DARK monsters in hand                    |
// | Black Whirlwind                    | Spell Cont   | No   | No    | None    | When Blackwing Normal Summoned: Search lower ATK| Main Phase 1 setup before Normal Summon       | Already 3 on field                          |
// | Foolish Burial                     | Spell Normal | No   | No    | None    | Send 1 monster from Deck to GY (Zephyros)     | Main Phase 1 setup extender                   | Zephyros already in GY                      |
// | Monster Reborn                     | Spell Normal | No   | No    | None    | Special Summon 1 monster from either GY       | Main Phase 1 extend Synchro materials or boss | Both GYs empty                              |
// | Blackbird Close                    | Trap Counter | Yes  | Yes   | Send mon| Handtrap Counter: Negate mon eff + SS BWD     | Opponent activates monster effect on field    | No Blackwing to send / target already neg   |
// | Blackwing - Simoon the Poison Wind | Monster L6   | Yes  | Yes   | Banish  | Banish Blackwing -> Place Whirlwind & NS free | In hand with another Blackwing                | Already used this turn                      |
// | Blackwing - Sudri the Phantom Glim | Monster L4   | Yes  | Yes   | None    | On NS: Search Blackwing card; tribute for Tkn | Normal Summon (primary starter)               | Already used this turn                      |
// | Blackwing - Shamal the Sandstorm   | Monster L4 T | Yes  | Yes   | Discard | Discard to place Whirlwind / recycle          | In hand; setup Whirlwind                      | Already used this turn                      |
// | Blackwing - Zephyros the Elite     | Monster L4   | Yes  | Yes   | Bounce  | Bounce face-up card to SS from GY (take 400 dmg)| In GY; bounce Whirlwind/used card to extend   | In hand                                     |
// | Blackwing - Bora the Spear         | Monster L4   | No   | No    | None    | SS from hand if control Blackwing; piercing   | Control Blackwing; need material              | No Blackwing on field                       |
// | Blackwing - Kris the Crack of Dawn | Monster L4   | Yes  | No    | None    | SS from hand if control Blackwing; destruct im| Control Blackwing; need material              | No Blackwing on field                       |
// | Blackwing - Gale the Whirlwind     | Monster L3 T | No   | No    | Target  | SS from hand if control Blackwing; halve ATK  | In hand or field; halve opp monster ATK       | Opponent controls 0 monsters                |
// | Blackwing - Harmattan the Dust     | Monster L2   | Yes  | No    | Target  | SS from hand; increase Level by target Level  | In hand; adjust level for Synchro climb       | No Blackwing on field                       |
// | Blackwing - Oroshi the Squall      | Monster L1 T | Yes  | No    | None    | SS from hand; change battle position on Synchro| In hand; need Level 1 Tuner                   | No Blackwing on field                       |
// | Blackwing Full Armor Master        | Synchro L10  | Yes  | Yes   | None    | 3000 ATK Tower; unaffected; steal enemy mon   | Boss push; steal enemy monster                | Points to friendly monsters                 |
// | Black-Winged Assault Dragon        | Synchro L10  | Yes  | Yes   | Tribute | 3200 ATK; 700 burn per opp mon eff; board nuke| Opponent active turns; high pressure          | Opponent controls 0 cards for nuke          |
// | Blackwing - Boreastorm the Wicked  | Synchro L6 T | Yes  | Yes   | Send mon| Synchro Tuner: dump Blackwing & copy Level    | Synchro Summoned; setup GY & climb Level 10   | No Blackwings in deck                       |
// | Blackwing - Nothung the Starlight  | Synchro L6   | Yes  | Yes   | None    | 800 burn + 800 debuff + extra Normal Summon   | Synchro Summoned; grant extra NS              | Already used this turn                      |
// | Assault Blackwing - Raikiri        | Synchro L7   | Yes  | Yes   | None    | Destroy cards up to other Blackwings you ctrl | Opponent controls cards; board wipe           | Opponent controls 0 cards                   |
// | Blackwing Tamer - Obsidian Hawk Joe| Synchro L7   | Yes  | Yes   | Target  | Revive L5+ Winged Beast; redirect attack/eff  | GY has L5+ Synchro (Full Armor / Assault)     | GY empty                                    |
// | Assault Blackwing - Onimaru        | Synchro L12  | No   | No    | None    | 6000 ATK attack push; indestructible          | Battle Phase finisher                         | No battle                                   |
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
    [Deck("Anime_Crow", "Anime_Crow")]
    public class Anime_CrowExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int SudriThePhantomGlimmer = 70465810;
            public const int SimoonThePoisonWind = 81470373;
            public const int ShamalTheSandstorm = 8571567;
            public const int ZephyrosTheElite = 14785765;
            public const int BoraTheSpear = 49003716;
            public const int KrisTheCrackOfDawn = 81105204;
            public const int GaleTheWhirlwind = 2009101;
            public const int HarmattanTheDust = 77152542;
            public const int OroshiTheSquall = 73652465;
            public const int AshBlossom = 14558127;

            // Spells
            public const int BlackWhirlwind = 91351370;
            public const int AllureOfDarkness = 1475311;
            public const int CalledByTheGrave = 24224830;
            public const int FoolishBurial = 81439173;
            public const int MonsterReborn = 83764718;
            public const int HarpiesFeatherDuster = 18144506;

            // Traps
            public const int BlackbirdClose = 80254726;
            public const int InfiniteImpermanence = 10045474;

            // Extra Deck
            public const int FullArmorMaster = 54082269;
            public const int BlackWingedAssaultDragon = 73218989;
            public const int BoreastormTheWickedWind = 10602628;
            public const int NothungTheStarlight = 95040215;
            public const int RaikiriTheRainShower = 16051717;
            public const int ObsidianHawkJoe = 81983656;
            public const int ChidoriTheRainSprinkling = 23338098;
            public const int OnimaruTheDivineThunder = 80773359;
            public const int BlackWingedDragon = 9012916;
        }

        public Anime_CrowExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            RegisterComboLines();
            RegisterExecutors();
        }

        private void RegisterComboLines()
        {
            // ── Line 1: Simoon Starter Line ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Blackwing-Simoon-Line",
                RequiredCards = new List<int> { CardId.SimoonThePoisonWind },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.SimoonThePoisonWind, ActionType = ExecutorType.Activate, Description = "Simoon places Whirlwind & Normal Summons self" },
                    new() { CardId = CardId.BlackWhirlwind, ActionType = ExecutorType.Activate, Description = "Whirlwind searches Sudri" },
                    new() { CardId = CardId.SudriThePhantomGlimmer, ActionType = ExecutorType.Summon, Description = "Normal Summon Sudri -> Search Shamal or Harmattan" }
                },
                FallbackLineName = "Blackwing-Sudri-Line"
            });

            // ── Line 2: Sudri Starter Line ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Blackwing-Sudri-Line",
                RequiredCards = new List<int> { CardId.SudriThePhantomGlimmer },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.SudriThePhantomGlimmer, ActionType = ExecutorType.Summon, Description = "Normal Summon Sudri" },
                    new() { CardId = CardId.SudriThePhantomGlimmer, ActionType = ExecutorType.Activate, Description = "Sudri search Blackbird Close or Harmattan" }
                }
            });
        }

        private void RegisterExecutors()
        {
            // ═══════════════════════════════════════════════════════════════
            //  TIER 0: COUNTER TRAPS, QUICK DISRUPTIONS & HANDTRAPS
            // ═══════════════════════════════════════════════════════════════

            // Blackbird Close — Handtrap Counter: Negates monster effect & cheats out Black-Winged Dragon!
            AddExecutor(ExecutorType.Activate, CardId.BlackbirdClose, BlackbirdCloseActivate);

            // Black-Winged Assault Dragon — Quick field nuke when 4+ counters
            AddExecutor(ExecutorType.Activate, CardId.BlackWingedAssaultDragon, AssaultDragonNukeActivate);

            // Full Armor Master — Steal enemy monster with Wedge counter
            AddExecutor(ExecutorType.Activate, CardId.FullArmorMaster, FullArmorMasterStealActivate);

            // Handtraps
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveActivate);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomActivate);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, InfiniteImpermanenceActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 1: MAIN PHASE SPELLS & DRAW/SEARCH
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, HarpiesFeatherDusterActivate);
            AddExecutor(ExecutorType.Activate, CardId.BlackWhirlwind, BlackWhirlwindActivate);
            AddExecutor(ExecutorType.Activate, CardId.AllureOfDarkness, AllureOfDarknessActivate);
            AddExecutor(ExecutorType.Activate, CardId.FoolishBurial, FoolishBurialActivate);
            AddExecutor(ExecutorType.Activate, CardId.MonsterReborn, MonsterRebornActivate);

            // Simoon hand activation
            AddExecutor(ExecutorType.Activate, CardId.SimoonThePoisonWind, SimoonActivate);
            AddExecutor(ExecutorType.Activate, CardId.ShamalTheSandstorm, ShamalActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: NORMAL SUMMON STARTERS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Summon, CardId.SudriThePhantomGlimmer, SudriSummon);
            AddExecutor(ExecutorType.Activate, CardId.SudriThePhantomGlimmer, SudriEffect);

            AddExecutor(ExecutorType.Summon, CardId.SimoonThePoisonWind, SimoonSummon);
            AddExecutor(ExecutorType.Summon, CardId.GaleTheWhirlwind, GaleSummon);
            AddExecutor(ExecutorType.Activate, CardId.GaleTheWhirlwind, GaleEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: SPECIAL SUMMON EXTENDERS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.BoraTheSpear, BoraSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.KrisTheCrackOfDawn, KrisSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.GaleTheWhirlwind, GaleSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.HarmattanTheDust, HarmattanSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.HarmattanTheDust, HarmattanEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.OroshiTheSquall, OroshiSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.ZephyrosTheElite, ZephyrosEffect);

            // Fallback Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.BoraTheSpear, FallbackNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.KrisTheCrackOfDawn, FallbackNormalSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: EXTRA DECK SYNCHRO PROGRESSION & BOARD REMOVAL
            // ═══════════════════════════════════════════════════════════════

            // Level 6 Boreastorm (Synchro Tuner: Foolish & Copy Level)
            AddExecutor(ExecutorType.SpSummon, CardId.BoreastormTheWickedWind, BoreastormSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.BoreastormTheWickedWind, BoreastormEffect);

            // Level 6 Nothung (800 burn + debuff + extra NS)
            AddExecutor(ExecutorType.SpSummon, CardId.NothungTheStarlight, NothungSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.NothungTheStarlight, NothungEffect);

            // Level 7 Raikiri (Board wipe)
            AddExecutor(ExecutorType.SpSummon, CardId.RaikiriTheRainShower, RaikiriSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.RaikiriTheRainShower, RaikiriEffect);

            // Level 7 Obsidian Hawk Joe (Revive L5+ Winged Beast)
            AddExecutor(ExecutorType.SpSummon, CardId.ObsidianHawkJoe, HawkJoeSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.ObsidianHawkJoe, HawkJoeEffect);

            // Level 10 Full Armor Master (Tower boss)
            AddExecutor(ExecutorType.SpSummon, CardId.FullArmorMaster, FullArmorMasterSpSummon);

            // Level 10 Black-Winged Assault Dragon (3200 ATK burn boss)
            AddExecutor(ExecutorType.SpSummon, CardId.BlackWingedAssaultDragon, AssaultDragonSpSummon);

            // Level 12 Onimaru (6000 ATK attack push)
            AddExecutor(ExecutorType.SpSummon, CardId.OnimaruTheDivineThunder, OnimaruSpSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 5: TRAP SETTING & REPOSITIONING
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, CardId.BlackbirdClose, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, SpellSetStrategy);

            AddExecutor(ExecutorType.Repos, RepositionStrategy);
        }

        // ═══════════════════════════════════════════════════════════════
        //  DISRUPTIONS & COUNTERS
        // ═══════════════════════════════════════════════════════════════

        private bool BlackbirdCloseActivate()
        {
            // Negate opponent monster effect
            ClientCard lastCard = LastChainCard;
            if (lastCard != null && lastCard.Controller == 1)
            {
                ClientCard fodder = Bot.GetMonsters()
                    .Where(m => m.IsFaceup() && m.HasSetcode(0x33) && !m.HasType(CardType.Synchro))
                    .OrderBy(m => m.Attack)
                    .FirstOrDefault()
                    ?? Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.HasSetcode(0x33));

                if (fodder != null)
                {
                    AI.SelectCard(fodder);
                    return true;
                }
            }
            return false;
        }

        private bool AssaultDragonNukeActivate()
        {
            // Board wipe if opponent controls 2+ cards
            return Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 2;
        }

        private bool FullArmorMasterStealActivate()
        {
            // Steal 1 opponent monster with a Wedge Counter
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
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
        //  SPELLS & SEARCHERS
        // ═══════════════════════════════════════════════════════════════

        private bool HarpiesFeatherDusterActivate()
        {
            return Enemy.GetSpellCount() > 0;
        }

        private bool BlackWhirlwindActivate()
        {
            return true;
        }

        private bool AllureOfDarknessActivate()
        {
            return Bot.Hand.Any(c => c.IsMonster() && c.HasAttribute(CardAttribute.Dark));
        }

        private bool FoolishBurialActivate()
        {
            AI.SelectCard(CardId.ZephyrosTheElite, CardId.ShamalTheSandstorm);
            return true;
        }

        private bool MonsterRebornActivate()
        {
            ClientCard target = Bot.Graveyard
                .Where(c => c.IsMonster() && (c.HasType(CardType.Synchro) || c.HasSetcode(0x33)))
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool SimoonActivate()
        {
            // Banish 1 Blackwing from hand
            ClientCard fodder = Bot.Hand.FirstOrDefault(c => c.HasSetcode(0x33) && c != Card && c.Id != CardId.SudriThePhantomGlimmer);
            if (fodder != null)
            {
                AI.SelectCard(fodder);
                return true;
            }
            return false;
        }

        private bool ShamalActivate()
        {
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  MONSTERS & SUMMONS
        // ═══════════════════════════════════════════════════════════════

        private bool SudriSummon()
        {
            return true;
        }

        private bool SudriEffect()
        {
            // Search Blackbird Close or Shamal/Harmattan
            if (!Bot.HasInHand(CardId.BlackbirdClose))
            {
                AI.SelectCard(CardId.BlackbirdClose);
            }
            else
            {
                AI.SelectCard(CardId.HarmattanTheDust, CardId.BoraTheSpear);
            }
            return true;
        }

        private bool SimoonSummon()
        {
            return true;
        }

        private bool GaleSummon()
        {
            return true;
        }

        private bool GaleEffect()
        {
            ClientCard target = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool BoraSpSummon()
        {
            return Bot.GetMonsters().Any(m => m.HasSetcode(0x33));
        }

        private bool KrisSpSummon()
        {
            return Bot.GetMonsters().Any(m => m.HasSetcode(0x33));
        }

        private bool GaleSpSummon()
        {
            return Bot.GetMonsters().Any(m => m.HasSetcode(0x33));
        }

        private bool HarmattanSpSummon()
        {
            return Bot.GetMonsters().Any(m => m.HasSetcode(0x33));
        }

        private bool HarmattanEffect()
        {
            // Target Blackwing to copy level
            ClientCard target = Bot.GetMonsters().FirstOrDefault(m => m != Card && m.HasSetcode(0x33));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool OroshiSpSummon()
        {
            return Bot.GetMonsters().Any(m => m.HasSetcode(0x33));
        }

        private bool ZephyrosEffect()
        {
            // Bounce Black Whirlwind or a monster
            ClientCard target = Bot.SpellZone.FirstOrDefault(s => s != null && s.IsFaceup() && s.Id == CardId.BlackWhirlwind)
                             ?? Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.BoraTheSpear);

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool FallbackNormalSummon()
        {
            return Bot.GetMonsterCount() < 2;
        }

        // ═══════════════════════════════════════════════════════════════
        //  EXTRA DECK SYNCHRO & REMOVAL
        // ═══════════════════════════════════════════════════════════════

        private bool BoreastormSpSummon()
        {
            return true;
        }

        private bool BoreastormEffect()
        {
            // Dump Blackwing to copy level
            AI.SelectCard(CardId.ZephyrosTheElite, CardId.SudriThePhantomGlimmer, CardId.HarmattanTheDust);
            return true;
        }

        private bool NothungSpSummon()
        {
            return true;
        }

        private bool NothungEffect()
        {
            ClientCard target = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
            }
            return true;
        }

        private bool RaikiriSpSummon()
        {
            return true;
        }

        private bool RaikiriEffect()
        {
            var targets = Enemy.GetMonsters().Concat(Enemy.GetSpells()).ToList();
            if (targets.Count > 0)
            {
                AI.SelectCard(targets);
                return true;
            }
            return false;
        }

        private bool HawkJoeSpSummon()
        {
            return true;
        }

        private bool HawkJoeEffect()
        {
            ClientCard target = Bot.Graveyard
                .Where(c => c.IsMonster() && c.HasType(CardType.Synchro) && c.Level >= 5)
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool FullArmorMasterSpSummon()
        {
            return true;
        }

        private bool AssaultDragonSpSummon()
        {
            return true;
        }

        private bool OnimaruSpSummon()
        {
            return true;
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
            if (Card.Attack < 1500 && Card.IsAttack()) return true;
            if (Card.Attack >= 2000 && Card.IsDefense()) return true;
            return false;
        }

        public override bool OnSelectYesNo(long desc)
        {
            if (Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0) return false;
            return base.OnSelectYesNo(desc);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards != null && cards.Count > 0)
            {
                var preferred = cards.Where(c => c.Id == CardId.SudriThePhantomGlimmer ||
                                                c.Id == CardId.SimoonThePoisonWind ||
                                                c.Id == CardId.HarmattanTheDust ||
                                                c.Id == CardId.BlackbirdClose).ToList();
                if (preferred.Count >= min)
                {
                    return preferred.Take(max).ToList();
                }
            }
            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
