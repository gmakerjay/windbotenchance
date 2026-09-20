// ============================================================================
// CARD AUDIT — Anime_Crow (Crow Hogan's Ultimate Blackwing Synchro & Burn Army)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Ash Blossom & Joyous Spring        | Monster L3 T | Yes  | Yes   | Discard | Handtrap: Negate deck search/dump/SS          | Opponent activates deck-interacting effect    | Bot's own turn without threat               |
// | Infinite Impermanence              | Trap Normal  | Yes  | Yes   | None    | Negate 1 face-up monster; column S/T negate   | Opponent monster activates or dangerous boss  | Target already negated; MP1 if handtrap     |
// | Called by the Grave                | Spell Quick  | Yes  | Yes   | Target  | Banish monster in opp GY and negate effects   | Opp activates handtrap or dangerous GY effect | No target in opp GY                         |
// | Harpie's Feather Duster            | Spell Normal | No   | No    | None    | Destroy all Spells and Traps opponent controls| Opponent controls 1+ Spell/Trap cards         | Opponent controls 0 Spells/Traps            |
// | Allure of Darkness                 | Spell Normal | No   | No    | Draw/Ban| Draw 2 cards, then banish 1 DARK monster      | Main Phase 1 draw engine                      | No DARK monsters in hand                    |
// | Black Whirlwind                    | Spell Cont   | No   | No    | None    | When Blackwing Normal Summoned: Search lower ATK| Main Phase 1 setup before Normal Summon       | Already 3 on field                          |
// | Black Feather Whirlwind            | Spell Cont   | Yes  | Yes   | None    | On DARK Synchro SS: Revive Blackwing/BWD; prot | Placed by Shamal or activated from hand      | Already active on field                     |
// | Foolish Burial                     | Spell Normal | No   | No    | None    | Send 1 monster from Deck to GY (Zephyros)     | Main Phase 1 setup extender                   | Zephyros already in GY                      |
// | Monster Reborn                     | Spell Normal | No   | No    | None    | Special Summon 1 monster from either GY       | Main Phase 1 extend Synchro materials or boss | Both GYs empty                              |
// | Blackbird Close                    | Trap Counter | Yes  | Yes   | Send mon| Handtrap Counter: Negate mon eff + SS BWD     | Opponent activates monster effect on field    | No Blackwing to send / target already neg   |
// | Blackwing - Simoon the Poison Wind | Monster L6   | Yes  | Yes   | Banish  | Banish Blackwing -> Place Whirlwind & NS free | In hand with another Blackwing                | Already used this turn                      |
// | Blackwing - Sudri the Phantom Glim | Monster L4   | Yes  | Yes   | None    | On NS: Search card mentioning BWD; trib token | Normal Summon (primary starter)               | Already used this turn                      |
// | Blackwing - Shamal the Sandstorm   | Monster L4 T | Yes  | Yes   | Discard | Discard to place BF Whirlwind / GY recycle    | In hand; setup BF Whirlwind                   | Already used this turn / BF Whirlwind on field|
// | Blackwing - Vata the Emblem of Wan | Monster L2 T | Yes  | Yes   | Send mon| SS from hand; send from deck for BWD          | In hand; send Zephyros+Bora for BWD           | Already used this turn                      |
// | Blackwing - Zephyros the Elite     | Monster L4   | Yes  | Yes   | Bounce  | Bounce face-up card to SS from GY (take 400 dmg)| In GY; bounce Whirlwind/Bora to extend        | In hand                                     |
// | Blackwing - Bora the Spear         | Monster L4   | No   | No    | None    | SS from hand if control Blackwing; piercing   | Control Blackwing; need material              | No Blackwing on field                       |
// | Blackwing - Kris the Crack of Dawn | Monster L4   | Yes  | No    | None    | SS from hand if control Blackwing; destruct im| Control Blackwing; need material              | No Blackwing on field                       |
// | Blackwing - Gale the Whirlwind     | Monster L3 T | No   | No    | Target  | SS from hand if control Blackwing; halve ATK  | In hand or field; halve opp monster ATK       | Opponent controls 0 monsters                |
// | Blackwing - Harmattan the Dust     | Monster L2   | Yes  | No    | Target  | SS from hand; increase Level by target Level  | In hand; adjust level for Synchro climb       | No Blackwing on field                       |
// | Blackwing - Oroshi the Squall      | Monster L1 T | Yes  | No    | None    | SS from hand; change battle position on Synchro| In hand; need Level 1 Tuner                   | No Blackwing on field                       |
// | Blackwing Full Armor Master        | Synchro L10  | Yes  | Yes   | None    | 3000 ATK Tower; unaffected; steal enemy mon   | Boss push; steal enemy monster                | Points to friendly monsters                 |
// | Black-Winged Assault Dragon        | Synchro L10  | Yes  | Yes   | Tribute | 3200 ATK; 700 burn per opp mon eff; board nuke| Opponent active turns; high pressure          | Opponent controls 0 cards for nuke          |
// | Assault Blackwing - Onimaru        | Synchro L12  | No   | No    | None    | 6000 ATK attack push; indestructible          | Battle Phase finisher                         | No battle / already lethal                  |
// | Black-Winged Dragon                | Synchro L8   | No   | No    | Counters| 2800 ATK Ace; absorb effect dmg; contact mat  | Stepping stone into Assault Dragon; beatstick | Already established board                   |
// | Assault Blackwing - Raikiri        | Synchro L7   | Yes  | Yes   | None    | Destroy cards up to other Blackwings you ctrl | Opponent controls cards; board wipe           | Opponent controls 0 cards                   |
// | Blackwing Tamer - Obsidian Hawk Joe| Synchro L7   | Yes  | Yes   | Target  | Revive L5+ Winged Beast; redirect attack/eff  | GY has L5+ Synchro (Full Armor / Assault)     | GY empty                                    |
// | Assault Blackwing - Chidori        | Synchro L7   | No   | No    | None    | 2600+ ATK booster per Blackwing in GY         | Beatstick push                                | Opponent unaffected                         |
// | Blackwing - Boreastorm the Wicked  | Synchro L6 T | Yes  | Yes   | Send mon| Synchro Tuner: dump Blackwing & copy Level    | Synchro Summoned; setup GY & climb Level 10   | No Blackwings in deck                       |
// | Blackwing - Nothung the Starlight  | Synchro L6   | Yes  | Yes   | None    | 800 burn + 800 debuff + extra Normal Summon   | Synchro Summoned; grant extra NS              | Already used this turn                      |
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
            public const int VataTheEmblemOfWandering = 71187462;
            public const int ZephyrosTheElite = 14785765;
            public const int BoraTheSpear = 49003716;
            public const int KrisTheCrackOfDawn = 81105204;
            public const int GaleTheWhirlwind = 2009101;
            public const int HarmattanTheDust = 77152542;
            public const int OroshiTheSquall = 73652465;
            public const int AshBlossom = 14558127;

            // Spells
            public const int BlackWhirlwind = 91351370;
            public const int BlackFeatherWhirlwind = 7602800;
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
            public const int OnimaruTheDivineThunder = 80773359;
            public const int BlackWingedDragon = 9012916;
            public const int RaikiriTheRainShower = 16051717;
            public const int ObsidianHawkJoe = 81983656;
            public const int ChidoriTheRainSprinkling = 23338098;
            public const int BoreastormTheWickedWind = 10602628;
            public const int NothungTheStarlight = 95040215;

            // Tokens
            public const int PhantomGlimmerToken = 70465811;
        }

        // Standard HINTMSG Constants
        private const long HINTMSG_RELEASE = 500;
        private const long HINTMSG_DISCARD = 501;
        private const long HINTMSG_DESTROY = 502;
        private const long HINTMSG_REMOVE = 504;
        private const long HINTMSG_RTOHAND = 505;
        private const long HINTMSG_ATOHAND = 506;
        private const long HINTMSG_EQUIP = 507;
        private const long HINTMSG_TOGRAVE = 508;
        private const long HINTMSG_SPSUMMON = 509;
        private const long HINTMSG_SMATERIAL = 512;
        private const long HINTMSG_XMATERIAL = 513;
        private const long HINTMSG_TARGET = 551;
        private const long HINTMSG_DISABLE = 552;
        private const long HINTMSG_NEGATE = 572;

        private static readonly int[] AceCardIds = new[]
        {
            CardId.FullArmorMaster,
            CardId.BlackWingedAssaultDragon,
            CardId.OnimaruTheDivineThunder,
            CardId.ObsidianHawkJoe,
            CardId.RaikiriTheRainShower
        };

        // Turn Tracking
        private bool _simoonUsed;
        private bool _sudriUsed;
        private bool _vataUsed;
        private bool _shamalUsed;
        private bool _zephyrosUsed;

        public Anime_CrowExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            HeuristicGuard.RegisterAceCards(AceCardIds);
            BaitPlanner.RegisterComboStarters(CardId.SimoonThePoisonWind, CardId.SudriThePhantomGlimmer, CardId.VataTheEmblemOfWandering, CardId.BlackWhirlwind);

            RegisterComboLines();
            RegisterExecutors();
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _simoonUsed = false;
            _sudriUsed = false;
            _vataUsed = false;
            _shamalUsed = false;
            _zephyrosUsed = false;
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
                    new() { CardId = CardId.SimoonThePoisonWind, ActionType = ExecutorType.Activate, Description = "Simoon banishes 1 Blackwing -> Places Black Whirlwind & Normal Summons self" },
                    new() { CardId = CardId.BlackWhirlwind, ActionType = ExecutorType.Activate, Description = "Whirlwind searches Sudri (1400 ATK < 1600)" },
                    new() { CardId = CardId.SudriThePhantomGlimmer, ActionType = ExecutorType.Summon, Description = "Normal Summon Sudri" },
                    new() { CardId = CardId.SudriThePhantomGlimmer, ActionType = ExecutorType.Activate, Description = "Sudri search Shamal or Vata" }
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
                    new() { CardId = CardId.SudriThePhantomGlimmer, ActionType = ExecutorType.Activate, Description = "Sudri search Shamal or Vata" },
                    new() { CardId = CardId.ShamalTheSandstorm, ActionType = ExecutorType.Activate, Description = "Shamal discard -> Place Black Feather Whirlwind" }
                },
                FallbackLineName = "Blackwing-Vata-Line"
            });

            // ── Line 3: Vata Dragon Line ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Blackwing-Vata-Line",
                RequiredCards = new List<int> { CardId.VataTheEmblemOfWandering },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.VataTheEmblemOfWandering, ActionType = ExecutorType.SpSummon, Description = "Special Summon Vata" },
                    new() { CardId = CardId.VataTheEmblemOfWandering, ActionType = ExecutorType.Activate, Description = "Vata send Zephyros + Bora from deck -> SS Black-Winged Dragon" }
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
            //  TIER 1: MAIN PHASE SPELLS & DRAW/SEARCH/SETUP
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, HarpiesFeatherDusterActivate);
            AddExecutor(ExecutorType.Activate, CardId.AllureOfDarkness, AllureOfDarknessActivate);
            AddExecutor(ExecutorType.Activate, CardId.FoolishBurial, FoolishBurialActivate);
            AddExecutor(ExecutorType.Activate, CardId.MonsterReborn, MonsterRebornActivate);

            // Whirlwind Spells
            AddExecutor(ExecutorType.Activate, CardId.BlackWhirlwind, BlackWhirlwindActivate);
            AddExecutor(ExecutorType.Activate, CardId.BlackFeatherWhirlwind, BlackFeatherWhirlwindActivate);

            // Shamal hand activation (Places Black Feather Whirlwind from deck)
            AddExecutor(ExecutorType.Activate, CardId.ShamalTheSandstorm, ShamalActivate);

            // Simoon hand activation (Places Black Whirlwind & Normal Summons)
            AddExecutor(ExecutorType.Activate, CardId.SimoonThePoisonWind, SimoonActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: NORMAL SUMMON STARTERS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Summon, CardId.SudriThePhantomGlimmer, SudriSummon);
            AddExecutor(ExecutorType.Activate, CardId.SudriThePhantomGlimmer, SudriEffect);

            AddExecutor(ExecutorType.Summon, CardId.SimoonThePoisonWind, SimoonSummon);
            AddExecutor(ExecutorType.Summon, CardId.VataTheEmblemOfWandering, VataSummon);
            AddExecutor(ExecutorType.Summon, CardId.GaleTheWhirlwind, GaleSummon);
            AddExecutor(ExecutorType.Activate, CardId.GaleTheWhirlwind, GaleEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: SPECIAL SUMMON EXTENDERS & GY RECOVERY
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.VataTheEmblemOfWandering, VataSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.VataTheEmblemOfWandering, VataEffect);

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
            AddExecutor(ExecutorType.Summon, CardId.ShamalTheSandstorm, FallbackNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.HarmattanTheDust, FallbackNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.OroshiTheSquall, FallbackNormalSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: EXTRA DECK SYNCHRO PROGRESSION & CONTACT SUMMONS
            // ═══════════════════════════════════════════════════════════════

            // Level 10 Full Armor Master (Tower boss - PRIORITY #1 TOWER BOSS)
            AddExecutor(ExecutorType.SpSummon, CardId.FullArmorMaster, FullArmorMasterSpSummon);

            // Level 10 Black-Winged Assault Dragon (Contact banish from field/GY or Synchro - PRIORITY #2 BURN BOSS)
            AddExecutor(ExecutorType.SpSummon, CardId.BlackWingedAssaultDragon, AssaultDragonSpSummon);

            // Level 12 Onimaru (6000 ATK attack push finisher)
            AddExecutor(ExecutorType.SpSummon, CardId.OnimaruTheDivineThunder, OnimaruSpSummon);

            // Level 8 Black-Winged Dragon (Signer Dragon Ace & Contact material)
            AddExecutor(ExecutorType.SpSummon, CardId.BlackWingedDragon, BlackWingedDragonSpSummon);

            // Level 7 Raikiri (Board wipe when opp controls cards)
            AddExecutor(ExecutorType.SpSummon, CardId.RaikiriTheRainShower, RaikiriSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.RaikiriTheRainShower, RaikiriEffect);

            // Level 7 Obsidian Hawk Joe (Revive L5+ Winged Beast)
            AddExecutor(ExecutorType.SpSummon, CardId.ObsidianHawkJoe, HawkJoeSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.ObsidianHawkJoe, HawkJoeEffect);

            // Level 7 Chidori (Damage beatstick)
            AddExecutor(ExecutorType.SpSummon, CardId.ChidoriTheRainSprinkling, ChidoriSpSummon);

            // Level 6 Boreastorm (Synchro Tuner: Foolish & Copy Level)
            AddExecutor(ExecutorType.SpSummon, CardId.BoreastormTheWickedWind, BoreastormSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.BoreastormTheWickedWind, BoreastormEffect);

            // Level 6 Nothung (800 burn + debuff + extra NS)
            AddExecutor(ExecutorType.SpSummon, CardId.NothungTheStarlight, NothungSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.NothungTheStarlight, NothungEffect);

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
            ClientCard lastCard = LastChainCard;
            if (lastCard != null && lastCard.Controller == 1)
            {
                // CRITICAL RULE 3: NEVER sacrifice Ace Bosses (Full Armor Master, Assault Dragon, Onimaru, Hawk Joe)
                ClientCard fodder = Bot.GetMonsters()
                    .Where(m => m.IsFaceup() && m.HasSetcode(0x33) && !IsCrowHighValueBoss(m))
                    .OrderBy(m => m.HasType(CardType.Synchro) ? 100 : 0) // Prefer non-Synchros
                    .ThenBy(m => m.Attack)
                    .FirstOrDefault();

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
            ClientCard target = Enemy.GetMonsters()
                .Where(m => m.IsFaceup())
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();

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
                ClientCard target = Enemy.Graveyard
                    .Where(c => c.IsMonster())
                    .OrderByDescending(c => GetThreatScore(c))
                    .ThenByDescending(c => c.Attack)
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
            ClientCard lastCard = LastChainCard;
            return lastCard != null && lastCard.Controller == 1;
        }

        private bool InfiniteImpermanenceActivate()
        {
            ClientCard target = Enemy.GetMonsters()
                .Where(m => m.IsFaceup() && !m.IsDisabled() && m.HasType(CardType.Effect))
                .OrderByDescending(c => GetThreatScore(c))
                .ThenByDescending(m => m.Attack)
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

        private bool BlackFeatherWhirlwindActivate()
        {
            // Activate continuous spell or trigger revive effect
            return true;
        }

        private bool AllureOfDarknessActivate()
        {
            return Bot.Hand.Any(c => c.IsMonster() && c.HasAttribute(CardAttribute.Dark));
        }

        private bool FoolishBurialActivate()
        {
            // Dump Zephyros for free revival, or Shamal for GY search
            if (Bot.Deck.Any(c => c.Id == CardId.ZephyrosTheElite))
            {
                AI.SelectCard(CardId.ZephyrosTheElite);
                return true;
            }
            if (Bot.Deck.Any(c => c.Id == CardId.ShamalTheSandstorm))
            {
                AI.SelectCard(CardId.ShamalTheSandstorm);
                return true;
            }
            return false;
        }

        private bool MonsterRebornActivate()
        {
            ClientCard target = Bot.Graveyard
                .Where(c => c.IsMonster() && (c.HasType(CardType.Synchro) || c.HasSetcode(0x33)))
                .OrderByDescending(c => IsCrowHighValueBoss(c) ? 1000 : 0)
                .ThenByDescending(c => c.Attack)
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
            if (_simoonUsed) return false;
            if (Card.Location == CardLocation.Hand && Bot.GetMonsterCount() == 0)
            {
                // Banish 1 other Blackwing from hand to place Black Whirlwind and NS Simoon
                ClientCard fodder = Bot.Hand.FirstOrDefault(c => c.HasSetcode(0x33) && c != Card && c.Id != CardId.SudriThePhantomGlimmer && c.Id != CardId.VataTheEmblemOfWandering)
                                 ?? Bot.Hand.FirstOrDefault(c => c.HasSetcode(0x33) && c != Card);
                if (fodder != null)
                {
                    _simoonUsed = true;
                    AI.SelectCard(fodder);
                    return true;
                }
            }
            return false;
        }

        private bool ShamalActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_shamalUsed) return false;
                // Only activate if Black Feather Whirlwind is in Deck and not already 2 on field
                if (Bot.Deck.Any(c => c.Id == CardId.BlackFeatherWhirlwind) &&
                    Bot.SpellZone.Count(s => s != null && s.IsFaceup() && s.Id == CardId.BlackFeatherWhirlwind) < 2)
                {
                    _shamalUsed = true;
                    return true;
                }
                return false;
            }
            // GY trigger: Banish to add Blackwing from GY to hand
            if (Card.Location == CardLocation.Grave)
            {
                ClientCard target = Bot.Graveyard
                    .Where(c => c.HasSetcode(0x33) && c != Card)
                    .OrderBy(c => c.Id == CardId.SudriThePhantomGlimmer ? 0 : 1)
                    .ThenBy(c => c.Id == CardId.BoraTheSpear ? 0 : 1)
                    .ThenBy(c => c.Id == CardId.GaleTheWhirlwind ? 0 : 1)
                    .FirstOrDefault();

                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  MONSTERS & SUMMONS
        // ═══════════════════════════════════════════════════════════════

        private bool SudriSummon()
        {
            // If Bot controls 0 monsters and has Simoon + another Blackwing, let Simoon activate first!
            if (Bot.GetMonsterCount() == 0 && !_simoonUsed && Bot.HasInHand(CardId.SimoonThePoisonWind) && Bot.Hand.Any(c => c.HasSetcode(0x33) && c.Id != CardId.SimoonThePoisonWind))
            {
                return false;
            }
            return true;
        }

        private bool SudriEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Effect 1 (On Normal Summon): Search card mentioning "Black-Winged Dragon"
                if (!_sudriUsed)
                {
                    _sudriUsed = true;
                    // Sudri can ONLY search cards mentioning Black-Winged Dragon!
                    AI.SelectCard(
                        CardId.ShamalTheSandstorm,
                        CardId.VataTheEmblemOfWandering,
                        CardId.BlackbirdClose,
                        CardId.BlackFeatherWhirlwind
                    );
                    return true;
                }

                // Effect 2 (Ignition): Tribute 1 monster for Level 2 Tuner Token (700 ATK)
                // Only tribute if we need a Tuner and have non-boss fodder!
                bool hasTuner = Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasType(CardType.Tuner));
                if (!hasTuner && Bot.GetMonsterCount() >= 2)
                {
                    ClientCard tributeFodder = Bot.GetMonsters()
                        .Where(m => m.IsFaceup() && !IsCrowHighValueBoss(m))
                        .OrderBy(m => m.Attack)
                        .FirstOrDefault();

                    if (tributeFodder != null)
                    {
                        AI.SelectCard(tributeFodder);
                        return true;
                    }
                }
                return false;
            }
            return true;
        }

        private bool SimoonSummon()
        {
            return true;
        }

        private bool VataSummon()
        {
            return Bot.GetMonsterCount() == 0 || FallbackNormalSummon();
        }

        private bool VataSpSummon()
        {
            return Bot.GetMonsters().Any(m => m.HasSetcode(0x33) && m.Id != CardId.VataTheEmblemOfWandering);
        }

        private bool VataEffect()
        {
            if (_vataUsed) return false;
            // Send Vata on field + non-Tuners from deck whose levels equal 8 to SS Black-Winged Dragon!
            // Must have Black-Winged Dragon in Extra Deck and Level 4 non-Tuners in deck (Zephyros, Bora, Kris, Sudri)
            if (!Bot.HasInExtra(CardId.BlackWingedDragon)) return false;

            int countL4InDeck = Bot.Deck.Count(c => c.HasSetcode(0x33) && !c.HasType(CardType.Tuner) && c.Level == 4);
            if (countL4InDeck >= 2)
            {
                _vataUsed = true;
                // Prefer sending Zephyros + Bora/Kris
                AI.SelectCard(CardId.ZephyrosTheElite, CardId.BoraTheSpear, CardId.KrisTheCrackOfDawn, CardId.SudriThePhantomGlimmer);
                return true;
            }
            return false;
        }

        private bool GaleSummon()
        {
            return true;
        }

        private bool GaleEffect()
        {
            ClientCard target = Enemy.GetMonsters()
                .Where(m => m.IsFaceup() && m.Attack > 0)
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();

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
            // Target Blackwing to copy level (prefer Level 4 to reach Level 6, or Level 6 to reach Level 8)
            ClientCard target = Bot.GetMonsters().FirstOrDefault(m => m != Card && m.HasSetcode(0x33) && m.Level == 4)
                             ?? Bot.GetMonsters().FirstOrDefault(m => m != Card && m.HasSetcode(0x33) && m.Level == 6)
                             ?? Bot.GetMonsters().FirstOrDefault(m => m != Card && m.HasSetcode(0x33));

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
            if (_zephyrosUsed) return false;
            // Bounce Black Whirlwind (can be reactivated!), Black Feather Whirlwind, or Bora (can be SS again)
            ClientCard target = Bot.SpellZone.FirstOrDefault(s => s != null && s.IsFaceup() && s.Id == CardId.BlackWhirlwind)
                             ?? Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.Id == CardId.BoraTheSpear)
                             ?? Bot.SpellZone.FirstOrDefault(s => s != null && s.IsFaceup() && s.Id == CardId.BlackFeatherWhirlwind)
                             ?? Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.HasType(CardType.Synchro) && !IsCrowHighValueBoss(m));

            if (target != null)
            {
                _zephyrosUsed = true;
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

        private static int GetThreatScore(ClientCard c)
        {
            if (c == null) return 0;
            int score = 0;
            if (CardIntelligence.IsHighThreatChokepoint(c.Id)) score += 1000;
            if (CardIntelligence.IsKnownNegator(c.Id)) score += 500;
            if (CardIntelligence.IsFloodgate(c.Id)) score += 400;
            return score;
        }

        private bool IsCrowHighValueBoss(ClientCard c)
        {
            if (c == null) return false;
            if (c.Id == CardId.FullArmorMaster || c.Id == CardId.BlackWingedAssaultDragon) return true;
            if (c.Id == CardId.OnimaruTheDivineThunder) return true;
            if (c.Id == CardId.ObsidianHawkJoe) return true;
            return false;
        }

        private bool FullArmorMasterSpSummon()
        {
            // Level 10 Tower Boss (3000 ATK, unaffected by card effects)
            if (Bot.GetMonsters().Any(m => m.Id == CardId.FullArmorMaster))
            {
                return false;
            }

            var nonBosses = Bot.GetMonsters().Where(m => !IsCrowHighValueBoss(m)).ToList();
            return nonBosses.Count >= 2;
        }

        private bool AssaultDragonSpSummon()
        {
            // Level 10 Burn & Nuke Boss (3200 ATK)
            if (Bot.GetMonsters().Any(m => m.Id == CardId.BlackWingedAssaultDragon))
            {
                return false;
            }

            // Contact Banish from field/GY: 1 Tuner Synchro + 1 Black-Winged Dragon
            bool canContact = (Bot.GetMonsters().Any(m => m.Id == CardId.BlackWingedDragon) || Bot.Graveyard.Any(c => c.Id == CardId.BlackWingedDragon))
                           && (Bot.GetMonsters().Any(m => m.HasType(CardType.Synchro) && m.HasType(CardType.Tuner)) || Bot.Graveyard.Any(c => c.HasType(CardType.Synchro) && c.HasType(CardType.Tuner)));
            if (canContact) return true;

            var nonBosses = Bot.GetMonsters().Where(m => !IsCrowHighValueBoss(m)).ToList();
            return nonBosses.Count >= 2;
        }

        private bool BlackWingedDragonSpSummon()
        {
            // Level 8 Signer Dragon (2800 ATK)
            var nonBosses = Bot.GetMonsters().Where(m => !IsCrowHighValueBoss(m)).ToList();
            return nonBosses.Count >= 2;
        }

        private bool RaikiriSpSummon()
        {
            // Level 7 Board Wipe (2600 ATK)
            // Only summon if opponent controls cards to destroy!
            if (Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0) return false;

            var nonBosses = Bot.GetMonsters().Where(m => !IsCrowHighValueBoss(m)).ToList();
            return nonBosses.Count >= 2;
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
            // Level 7 Reviver (2600 ATK)
            bool hasGyReviveTarget = Bot.Graveyard.Any(c => c.IsMonster() && c.HasType(CardType.Synchro) && c.Level >= 5);
            if (!hasGyReviveTarget && Enemy.GetMonsterCount() == 0 && Bot.GetMonsterCount() >= 2) return false;

            var nonBosses = Bot.GetMonsters().Where(m => !IsCrowHighValueBoss(m)).ToList();
            return nonBosses.Count >= 2;
        }

        private bool HawkJoeEffect()
        {
            ClientCard target = Bot.Graveyard
                .Where(c => c.IsMonster() && c.HasType(CardType.Synchro) && c.Level >= 5)
                .OrderBy(c => c.Id == CardId.FullArmorMaster ? 0 : 1)
                .ThenBy(c => c.Id == CardId.BlackWingedAssaultDragon ? 0 : 1)
                .ThenByDescending(c => c.Attack)
                .FirstOrDefault();

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool ChidoriSpSummon()
        {
            // Level 7 Beatstick (2600+ ATK)
            var nonBosses = Bot.GetMonsters().Where(m => !IsCrowHighValueBoss(m)).ToList();
            return nonBosses.Count >= 2;
        }

        private bool NothungSpSummon()
        {
            // Level 6 Extender (2400 ATK, 800 burn, extra Normal Summon)
            var mainDeckFodder = Bot.GetMonsters().Where(m => !m.HasType(CardType.Synchro)).ToList();
            return mainDeckFodder.Count >= 2;
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

        private bool BoreastormSpSummon()
        {
            // Level 6 Synchro Tuner (2400 ATK)
            var mainDeckFodder = Bot.GetMonsters().Where(m => !m.HasType(CardType.Synchro)).ToList();
            return mainDeckFodder.Count >= 2;
        }

        private bool BoreastormEffect()
        {
            // Dump Blackwing to copy level: Zephyros if in Deck, else Level 4, else Level 2
            if (Bot.Deck.Any(c => c.Id == CardId.ZephyrosTheElite))
            {
                AI.SelectCard(CardId.ZephyrosTheElite);
            }
            else
            {
                AI.SelectCard(
                    CardId.SudriThePhantomGlimmer,
                    CardId.BoraTheSpear,
                    CardId.KrisTheCrackOfDawn,
                    CardId.HarmattanTheDust,
                    CardId.VataTheEmblemOfWandering
                );
            }
            return true;
        }

        private bool OnimaruSpSummon()
        {
            // Level 12 Finisher (3000/6000 ATK)
            if (Duel.Phase != DuelPhase.Main1) return false;

            // NEVER sacrifice Full Armor Master or Assault Dragon!
            var sacrificialSynchros = Bot.GetMonsters().Where(m => m.HasType(CardType.Synchro) && !IsCrowHighValueBoss(m)).ToList();
            return sacrificialSynchros.Count >= 1 && Bot.GetMonsterCount() >= 2;
        }

        // ═══════════════════════════════════════════════════════════════
        //  HEURISTICS & CARD SELECTION
        // ═══════════════════════════════════════════════════════════════

        private bool SpellSetStrategy()
        {
            // Rule 5: NEVER set handtraps in Main Phase 1!
            if (Duel.Phase == DuelPhase.Main1)
            {
                return false;
            }

            // Blackbird Close: If we control a Blackwing Synchro or BWD, keep in hand as a handtrap!
            if (Card.Id == CardId.BlackbirdClose)
            {
                bool controlSynchroOrBwd = Bot.GetMonsters().Any(m => m.IsFaceup() && (m.HasType(CardType.Synchro) || m.Id == CardId.BlackWingedDragon));
                if (controlSynchroOrBwd)
                {
                    return false; // Keep in hand!
                }
                return true;
            }

            // Infinite Impermanence: If field empty, keep in hand as a handtrap
            if (Card.Id == CardId.InfiniteImpermanence)
            {
                if (Bot.GetMonsterCount() == 0 && Bot.GetSpellCount() == 0)
                {
                    return false;
                }
                return true;
            }

            if (Card.IsTrap()) return true;
            if (Card.IsSpell() && Card.HasType(CardType.QuickPlay))
            {
                return true;
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
            // DO NOT reject prompts merely because enemy board is empty! (Fixes Turn 1 Going First bug)
            return base.OnSelectYesNo(desc);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // ── 1. TRIBUTE / RELEASE (Hint 500) ──
            if (hint == HINTMSG_RELEASE)
            {
                var releaseOrder = cards.OrderBy(c =>
                {
                    if (IsCrowHighValueBoss(c)) return 999; // NEVER SACRIFICE BOSS
                    if (c.Id == CardId.PhantomGlimmerToken) return 0; // Token first
                    if (c.IsDisabled()) return 1;
                    return c.Attack;
                }).ToList();

                if (releaseOrder.Count >= min)
                    return releaseOrder.Take(max).ToList();
            }

            // ── 2. DISCARD COST (Hint 501) ──
            if (hint == HINTMSG_DISCARD)
            {
                var discardOrder = cards.OrderBy(c =>
                {
                    if (c.Id == CardId.ZephyrosTheElite) return 0; // Wants to be in GY
                    if (c.Id == CardId.ShamalTheSandstorm) return 1; // GY recycle
                    if (cards.Count(x => x.Id == c.Id) > 1) return 2; // Duplicates
                    if (IsCrowHighValueBoss(c)) return 999;
                    return 50;
                }).ToList();

                if (discardOrder.Count >= min)
                    return discardOrder.Take(max).ToList();
            }

            // ── 3. REMOVAL / DESTRUCTION (Hint 502, 503, 504) -> MUST target enemy cards! ──
            if (hint == HINTMSG_DESTROY || (hint == HINTMSG_REMOVE && cards.Any(c => c.Controller == 1)))
            {
                var enemyTargets = cards.Where(c => c.Controller == 1).ToList();
                if (enemyTargets.Count >= min)
                {
                    return enemyTargets
                        .OrderByDescending(c => GetThreatScore(c))
                        .ThenByDescending(c => c.Attack)
                        .Take(max)
                        .ToList();
                }
            }

            // ── 4. FRIENDLY BANISH COST (Contact Banish for Assault Dragon, Simoon, Allure) ──
            if (hint == HINTMSG_REMOVE && cards.All(c => c.Controller == 0))
            {
                // For Black-Winged Assault Dragon contact banish: Pick from GY FIRST!
                bool isAssaultContact = cards.Any(c => c.Id == CardId.BlackWingedDragon || (c.HasType(CardType.Synchro) && c.HasType(CardType.Tuner)));
                if (isAssaultContact)
                {
                    var sorted = cards.OrderBy(c =>
                    {
                        // GY materials preferred over field
                        int locationScore = (c.Location == CardLocation.Grave) ? 0 : 100;
                        if (c.Id == CardId.BoreastormTheWickedWind && c.Location == CardLocation.Grave) return locationScore + 0;
                        if (c.Id == CardId.BlackWingedDragon && c.Location == CardLocation.Grave) return locationScore + 1;
                        if (IsCrowHighValueBoss(c)) return 999;
                        return locationScore + 10;
                    }).ToList();

                    if (sorted.Count >= min)
                        return sorted.Take(max).ToList();
                }

                // For Simoon or Allure banish: Pick non-starters
                var banishFodder = cards.OrderBy(c =>
                {
                    if (c.Id == CardId.SudriThePhantomGlimmer) return 900; // Save starter
                    if (c.Id == CardId.SimoonThePoisonWind) return 800;
                    if (c.Id == CardId.VataTheEmblemOfWandering) return 700;
                    if (c.Id == CardId.GaleTheWhirlwind) return 600;
                    return c.Attack;
                }).ToList();

                if (banishFodder.Count >= min)
                    return banishFodder.Take(max).ToList();
            }

            // ── 5. BOUNCE TO HAND (Hint 505) ──
            if (hint == HINTMSG_RTOHAND)
            {
                // Enemy bounce
                var enemyTargets = cards.Where(c => c.Controller == 1).ToList();
                if (enemyTargets.Count >= min)
                {
                    return enemyTargets
                        .OrderByDescending(c => GetThreatScore(c))
                        .ThenByDescending(c => c.Attack)
                        .Take(max)
                        .ToList();
                }

                // Friendly bounce (Zephyros)
                var friendlyTargets = cards.Where(c => c.Controller == 0).OrderBy(c =>
                {
                    if (c.Id == CardId.BlackWhirlwind) return 0; // Re-activatable
                    if (c.Id == CardId.BoraTheSpear) return 1; // Re-summonable
                    if (c.Id == CardId.BlackFeatherWhirlwind) return 2;
                    if (IsCrowHighValueBoss(c)) return 999;
                    return 100;
                }).ToList();

                if (friendlyTargets.Count >= min)
                    return friendlyTargets.Take(max).ToList();
            }

            // ── 6. SEARCH / ADD TO HAND (Hint 506) ──
            if (hint == HINTMSG_ATOHAND)
            {
                var preferred = new List<int>
                {
                    CardId.SimoonThePoisonWind,
                    CardId.SudriThePhantomGlimmer,
                    CardId.VataTheEmblemOfWandering,
                    CardId.ShamalTheSandstorm,
                    CardId.BlackbirdClose,
                    CardId.GaleTheWhirlwind,
                    CardId.BoraTheSpear,
                    CardId.HarmattanTheDust,
                    CardId.OroshiTheSquall,
                    CardId.KrisTheCrackOfDawn,
                    CardId.ZephyrosTheElite,
                    CardId.BlackFeatherWhirlwind
                };

                var matches = cards.Where(c => preferred.Contains(c.Id))
                                   .OrderBy(c => preferred.IndexOf(c.Id))
                                   .ToList();

                if (matches.Count >= min)
                    return matches.Take(max).ToList();
            }

            // ── 7. SEND TO GRAVE (Hint 508 - Boreastorm / Foolish) ──
            if (hint == HINTMSG_TOGRAVE)
            {
                var dumpOrder = cards.OrderBy(c =>
                {
                    if (c.Id == CardId.ZephyrosTheElite) return 0; // Free SS from GY
                    if (c.Id == CardId.ShamalTheSandstorm) return 1; // Recycle
                    if (c.Id == CardId.SudriThePhantomGlimmer) return 2;
                    if (c.Id == CardId.BoraTheSpear) return 3;
                    if (c.Id == CardId.HarmattanTheDust) return 4;
                    return 50;
                }).ToList();

                if (dumpOrder.Count >= min)
                    return dumpOrder.Take(max).ToList();
            }

            // ── 8. SPECIAL SUMMON TARGET (Hint 509 - Black Feather Whirlwind, Hawk Joe, Reborn) ──
            if (hint == HINTMSG_SPSUMMON)
            {
                var ssOrder = cards.OrderBy(c =>
                {
                    if (c.Id == CardId.FullArmorMaster) return 0;
                    if (c.Id == CardId.BlackWingedAssaultDragon) return 1;
                    if (c.Id == CardId.BlackWingedDragon) return 2;
                    if (c.Id == CardId.RaikiriTheRainShower) return 3;
                    if (c.Id == CardId.BoreastormTheWickedWind) return 4;
                    if (c.Id == CardId.NothungTheStarlight) return 5;
                    if (c.Id == CardId.GaleTheWhirlwind) return 6;
                    if (c.Id == CardId.SudriThePhantomGlimmer) return 7;
                    if (c.Id == CardId.BoraTheSpear) return 8;
                    return 20;
                }).ToList();

                if (ssOrder.Count >= min)
                    return ssOrder.Take(max).ToList();
            }

            // ── 9. SYNCHRO MATERIAL (Hint 512) ──
            if (hint == HINTMSG_SMATERIAL || (Duel.Phase != DuelPhase.Battle && cards.All(c => c.Location == CardLocation.MonsterZone && c.Controller == 0)))
            {
                var materialOrder = cards.OrderBy(c =>
                {
                    if (c.Id == CardId.FullArmorMaster) return 999; // NEVER SACRIFICE TOWER BOSS
                    if (c.Id == CardId.BlackWingedAssaultDragon) return 998; // NEVER SACRIFICE BURN BOSS
                    if (c.Id == CardId.OnimaruTheDivineThunder) return 997; // NEVER SACRIFICE FINISHER
                    if (c.Id == CardId.ObsidianHawkJoe) return 500;
                    if (c.Id == CardId.RaikiriTheRainShower) return 400;
                    if (c.Id == CardId.BlackWingedDragon) return 300;
                    if (c.Id == CardId.NothungTheStarlight) return 200;
                    if (c.Id == CardId.BoreastormTheWickedWind) return 100;
                    if (c.Id == CardId.PhantomGlimmerToken) return 0;
                    if (c.IsDisabled()) return 1;
                    return c.Attack;
                }).ToList();

                if (materialOrder.Count >= min)
                    return materialOrder.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
