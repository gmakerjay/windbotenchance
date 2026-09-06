// ============================================================
// CARD AUDIT — 2026_Spright (Spright Frog / Nimble Engine)
// ============================================================
// | Card Name                | Type       | OPT? | HOPT? | Cost | Effect Summary                             | Activate When                              | NEVER Activate When                          |
// |--------------------------|------------|------|-------|------|--------------------------------------------|--------------------------------------------|----------------------------------------------|
// | Spright Blue             | Monster L2 | Yes  | Yes   | None | SS from Hand; Search Spright Monster       | Control Lv/Rk 2 / On SS                    | Blue search already used                     |
// | Spright Jet              | Monster L2 | Yes  | Yes   | None | SS from Hand; Search Spright S/T           | Control Lv/Rk 2 / On SS                    | Jet search already used                      |
// | Spright Red              | Monster L2 | Yes  | Yes   | Trib | SS from Hand; Negate & Destroy Monster Eff | Control Lv/Lk 2 / Opp Monster Effect       | No tribute available                         |
// | Spright Carrot           | Monster L2 | Yes  | Yes   | Trib | SS from Hand; Negate & Destroy Spell/Trap  | Control Lv/Lk 2 / Opp Spell/Trap Activate  | No tribute available                         |
// | Spright Pixies           | Monster L2 | Yes  | Yes   | Send | SS from Hand; Handtrap boost ATK by opp ATK| Damage Calculation of Lv/Rk/Lk 2 monster  | Already higher ATK without boost             |
// | Spright Starter          | QuickSp    | Yes  | Yes   | LP   | SS Spright from Deck (Lock Lv/Rk/Lk 2)    | Need Starter / Blue search / Extender      | Already locked out or out of Deck targets    |
// | Spright Smashers         | QuickSp    | Yes  | Yes   | Banish 1 Spright S/T | Banish 1 Lv/Rk/Lk 2 + 1 Opp card | Opp controls high threat card / Quick Rem  | No Spright in hand/GY to banish              |
// | Spright Gamma Burst      | QuickSp    | Yes  | Yes   | None | All Lv/Rk/Lk 2 gain +1400 ATK/DEF (OTK)    | Battle Phase lethal push / Damage step     | No monsters to buff                          |
// | Spright Double Cross     | Trap       | Yes  | Yes   | None | Steal Opp Monster / Revive / Attach Mat    | Opp summons boss / Main Phase disruption   | No valid target / Zones full                 |
// | Swap Frog                | Monster L2 | No   | No    | Disc | Send Lv2 Aqua to GY; Extra Frog Normal Sum | Hand/Field setup; Send Ronin/Dupe/Swap     | Already used Extra Normal Summon             |
// | Dupe Frog                | Monster L2 | No   | No    | None | Search Frog on GY send; Attack redirection | Sent from field to GY / On field           | Missing timing                               |
// | Ronintoadin              | Monster L2 | No   | No    | Banish Frog | SS from GY                           | In GY, Frog in GY available                | No Frogs in GY to banish                     |
// | Nimble Beaver            | Monster L2 | No   | No    | None | On NS: SS Nimble from Deck/GY              | On Normal Summon                           | No Nimble targets left                       |
// | Nimble Angler            | Monster L2 | Yes  | Yes   | None | Sent to GY from Hand/Deck: SS 2 Nimbles    | Dumped by Sprind or Droplet/Twin           | No Beaver in deck                            |
// | Gigantic Spright         | Xyz R2     | Yes  | Yes   | Det 1| 3200 ATK with Link mat; SS Lv2 from Deck  | Main Phase combo start; Locks both to Lv2  | Already used HOPT                            |
// | Spright Elf              | Link 2     | Yes  | Yes   | None | Target protection; Quick Revive Lv/Rk/Lk 2 | Main Phase (Our Turn/Opp Turn) Quick Revive| Already used HOPT                            |
// | Spright Sprind           | Link 2     | Yes  | Yes   | Det 1| On Link: Dump Lv2; On SS: Quick Bounce 1   | On Link Summon / Opp SS monster            | Already used HOPT                            |
// | Toadally Awesome         | Xyz R2     | Yes  | Yes   | Trib | Omni-Negate + Destroy + SET; Standby SS Frog| Opp activates ANY card; Standby Phase      | No Aqua to tribute                           |
// | Cat Shark                | Xyz R2     | No   | No    | Det 1| Double Rank 2 Xyz ATK (Gigantic 6400 ATK)  | Battle Phase OTK / High DEF stall          | No Rank 2 to target                          |
// | Onibimaru Soul Sweeper   | Xyz R2     | Yes  | Yes   | Det 1| Banish opp monster until next End Phase    | Opp monster removal                        | No materials left                            |
// | I:P Masquerena           | Link 2     | Yes  | Yes   | None | Opponent Turn Quick Link Summon (Unicorn)  | Opponent Main Phase                        | No other materials on field                  |
// | Knightmare Unicorn       | Link 3     | Yes  | Yes   | Disc | On Link: Spin 1 card into Deck             | Opponent high-threat card on field         | Hand empty                                   |
// | AA-ZEUS                  | Xyz R12    | No   | No    | Det 2| Send all other cards on field to GY        | After Xyz battled; Opponent heavy board    | Board already clear                          |
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_Spright", "2026_Spright")]
    public class _2026_SprightExecutor : ModernExecutor
    {
        public class CardId
        {
            // Spright Core Main Deck
            public const int SprightBlue = 76145933;
            public const int SprightJet = 13533678;
            public const int SprightRed = 75922381;
            public const int SprightCarrot = 2311090;
            public const int SprightPixies = 49928686;
            public const int SprightStarter = 15443125;
            public const int SprightSmashers = 88836438;
            public const int SprightGammaBurst = 42431833;
            public const int SprightDoubleCross = 68250822;

            // Frog Engine
            public const int SwapFrog = 9126351;
            public const int DupeFrog = 46239604;
            public const int Ronintoadin = 1357146;

            // Nimble Engine
            public const int NimbleBeaver = 68353324;
            public const int NimbleAngler = 88686573;

            // Handtraps & Staples
            public const int MaxxC = 23434538;
            public const int AshBlossom = 14558127;
            public const int GhostOgre = 59438930;
            public const int CalledByTheGrave = 24224830;
            public const int CrossoutDesignator = 65681983;
            public const int ForbiddenDroplet = 24299458;
            public const int PotOfProsperity = 84211599;
            public const int HarpiesFeatherDuster = 18144506;

            // Extra Deck Bosses
            public const int GiganticSpright = 54498517;
            public const int SprightElf = 27381364;
            public const int SprightSprind = 72329844;
            public const int ToadallyAwesome = 90809975;
            public const int CatShark = 84224627;
            public const int OnibimaruSoulSweeper = 9486959;
            public const int NinjaShadowMosquito = 32453837;
            public const int MannequinCat = 54191698;
            public const int IPMasquerena = 65741786;
            public const int KnightmareUnicorn = 38342335;
            public const int DownerdMagician = 72167543;
            public const int Zeus = 90448279;
        }

        // Standard OCG Hint IDs
        private const long HINT_SELECT_FACEUP = 500;
        private const long HINT_SELECT_TOGRAVE = 501;
        private const long HINT_SELECT_DESTROY = 502;
        private const long HINT_SELECT_DISCARD = 504;
        private const long HINT_SELECT_TOHAND = 506;
        private const long HINT_SELECT_SPSUMMON = 509;
        private const long HINT_SELECT_BANISH = 512;
        private const long HINT_SELECT_SET = 514;
        private const long HINT_SELECT_DETACH = 519;

        // Turn-state tracking
        private bool _sprightBlueSearchUsed = false;
        private bool _sprightJetSearchUsed = false;
        private bool _sprightStarterUsed = false;
        private bool _sprightSmashersUsed = false;
        private bool _sprightGammaBurstUsed = false;
        private bool _sprightDoubleCrossUsed = false;
        private bool _giganticSprightUsed = false;
        private bool _sprightElfReviveUsed = false;
        private bool _sprightSprindDumpUsed = false;
        private bool _sprightSprindBounceUsed = false;
        private bool _prosperityUsedThisTurn = false;
        private bool _maxxCActivatedThisTurn = false;

        public _2026_SprightExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // ── 1. Register Ace Cards in ResourcePlanner ──
            ResourcePlan.RegisterAceCards(
                CardId.ToadallyAwesome,
                CardId.SprightElf,
                CardId.GiganticSpright,
                CardId.SprightRed,
                CardId.SprightCarrot,
                CardId.OnibimaruSoulSweeper,
                CardId.CatShark,
                CardId.IPMasquerena,
                CardId.KnightmareUnicorn,
                CardId.Zeus
            );

            // ── 2. Register Starters and Baits in BaitPlanner ──
            BaitPlanner.RegisterComboStarters(
                CardId.NimbleBeaver,
                CardId.SwapFrog,
                CardId.SprightStarter,
                CardId.SprightBlue,
                CardId.SprightJet,
                CardId.PotOfProsperity
            );
            BaitPlanner.RegisterBaitCards(
                CardId.HarpiesFeatherDuster,
                CardId.PotOfProsperity,
                CardId.ForbiddenDroplet
            );

            // ── 3. Register Combo Lines in ComboRouter ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Spright-Frog-Toadally-FullBoard",
                RequiredCards = new List<int> { CardId.NimbleBeaver, CardId.SprightBlue },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.NimbleBeaver, ActionType = ExecutorType.Summon, Description = "Normal Summon Nimble Beaver -> SS Nimble Beaver from Deck" },
                    new() { CardId = CardId.SprightBlue, ActionType = ExecutorType.SpSummon, Description = "Special Summon Spright Blue -> Search Spright Jet" },
                    new() { CardId = CardId.SprightJet, ActionType = ExecutorType.SpSummon, Description = "Special Summon Spright Jet -> Search Spright Starter / Smashers" },
                    new() { CardId = CardId.GiganticSpright, ActionType = ExecutorType.SpSummon, Description = "Xyz Summon Gigantic Spright -> SS Swap Frog from Deck" },
                    new() { CardId = CardId.SwapFrog, ActionType = ExecutorType.Activate, Description = "Swap Frog dumps Ronintoadin to GY" },
                    new() { CardId = CardId.SprightElf, ActionType = ExecutorType.SpSummon, Description = "Link Summon Spright Elf -> Revive Swap Frog" },
                    new() { CardId = CardId.Ronintoadin, ActionType = ExecutorType.Activate, Description = "Ronintoadin banishes Frog to revive" },
                    new() { CardId = CardId.ToadallyAwesome, ActionType = ExecutorType.SpSummon, Description = "Xyz Summon Toadally Awesome (Omni-Negate)" }
                },
                FallbackLineName = "Spright-Gigantic-Red-Line"
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Spright-Gigantic-Red-Line",
                RequiredCards = new List<int> { CardId.SprightStarter },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.SprightStarter, ActionType = ExecutorType.Activate, Description = "Activate Spright Starter -> SS Spright Blue" },
                    new() { CardId = CardId.SprightBlue, ActionType = ExecutorType.Activate, Description = "Spright Blue searches Spright Jet" },
                    new() { CardId = CardId.SprightJet, ActionType = ExecutorType.SpSummon, Description = "Special Summon Spright Jet -> Search Spright Smashers" },
                    new() { CardId = CardId.GiganticSpright, ActionType = ExecutorType.SpSummon, Description = "Xyz Summon Gigantic Spright -> SS Spright Red from Deck" },
                    new() { CardId = CardId.SprightElf, ActionType = ExecutorType.SpSummon, Description = "Link Summon Spright Elf -> Revive Spright Blue" }
                }
            });

            // ═══════════════════════════════════════════════════════════════
            //  EXECUTORS PIPELINE (Tiered Priority)
            // ═══════════════════════════════════════════════════════════════

            // ── Tier 0: Quick Negates, Handtraps & Disruptions ──
            AddExecutor(ExecutorType.Activate, CardId.ToadallyAwesome, ToadallyAwesomeNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.SprightRed, SprightRedNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.SprightCarrot, SprightCarrotNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.GhostOgre, GhostOgreEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.CrossoutDesignator, CrossoutDesignatorEffect);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletEffect);
            AddExecutor(ExecutorType.Activate, CardId.SprightSmashers, SprightSmashersEffect);
            AddExecutor(ExecutorType.Activate, CardId.SprightDoubleCross, SprightDoubleCrossEffect);
            AddExecutor(ExecutorType.Activate, CardId.SprightSprind, SprightSprindBounceEffect);
            AddExecutor(ExecutorType.Activate, CardId.SprightElf, SprightElfQuickReviveEffect);
            AddExecutor(ExecutorType.Activate, CardId.IPMasquerena, IPMasquerenaEffect);
            AddExecutor(ExecutorType.Activate, CardId.SprightPixies, SprightPixiesEffect);

            // ── Tier 1: Board Breakers ──
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, HarpiesFeatherDusterEffect);

            // ── Tier 2: Search, Setup Spells & Inherent Hand Summons (PRIORITIZED BEFORE EXTRA DECK) ──
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, PotOfProsperityEffect);
            AddExecutor(ExecutorType.Activate, CardId.SprightStarter, SprightStarterEffect);

            // Inherent Spright Special Summons from Hand
            AddExecutor(ExecutorType.SpSummon, CardId.SprightBlue, SprightBlueHandSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SprightBlue, SprightBlueSearchEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.SprightJet, SprightJetHandSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SprightJet, SprightJetSearchEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.SprightRed, SprightRedHandSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SprightCarrot, SprightCarrotHandSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SprightPixies, SprightPixiesHandSpSummon);

            // ── Tier 3: Frog & Nimble Normal Summons & Triggers ──
            AddExecutor(ExecutorType.Summon, CardId.NimbleBeaver, NimbleBeaverSummon);
            AddExecutor(ExecutorType.Activate, CardId.NimbleBeaver, NimbleBeaverEffect);
            AddExecutor(ExecutorType.Activate, CardId.NimbleAngler, NimbleAnglerEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.SwapFrog, SwapFrogSpSummonHand);
            AddExecutor(ExecutorType.Summon, CardId.SwapFrog, SwapFrogSummon);
            AddExecutor(ExecutorType.Activate, CardId.SwapFrog, SwapFrogTriggerEffect);
            AddExecutor(ExecutorType.Activate, CardId.Ronintoadin, RonintoadinReviveEffect);
            AddExecutor(ExecutorType.Activate, CardId.DupeFrog, DupeFrogSearchEffect);

            // ── Tier 4: Primary Extra Deck Engine (Gigantic -> Sprind -> Elf -> Toadally) ──
            AddExecutor(ExecutorType.SpSummon, CardId.GiganticSpright, GiganticSprightSummon);
            AddExecutor(ExecutorType.Activate, CardId.GiganticSpright, GiganticSprightEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.SprightSprind, SprightSprindSummon);
            AddExecutor(ExecutorType.Activate, CardId.SprightSprind, SprightSprindDumpEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.SprightElf, SprightElfSummon);
            AddExecutor(ExecutorType.Activate, CardId.SprightElf, SprightElfMainReviveEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.ToadallyAwesome, ToadallyAwesomeSummon);
            AddExecutor(ExecutorType.Activate, CardId.ToadallyAwesome, ToadallyAwesomeStandbyEffect);

            // ── Tier 5: Auxiliary Extra Deck Bosses ──
            AddExecutor(ExecutorType.SpSummon, CardId.OnibimaruSoulSweeper, OnibimaruSummon);
            AddExecutor(ExecutorType.Activate, CardId.OnibimaruSoulSweeper, OnibimaruEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.CatShark, CatSharkSummon);
            AddExecutor(ExecutorType.Activate, CardId.CatShark, CatSharkEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.IPMasquerena, IPMasquerenaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmareUnicorn, KnightmareUnicornSummon);
            AddExecutor(ExecutorType.Activate, CardId.KnightmareUnicorn, KnightmareUnicornEffect);

            // ── Tier 6: Downerd Magician & AA-ZEUS ──
            AddExecutor(ExecutorType.SpSummon, CardId.DownerdMagician, DownerdMagicianSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Zeus, ZeusSummon);
            AddExecutor(ExecutorType.Activate, CardId.Zeus, ZeusEffect);

            // ── Tier 7: OTK Enhancer ──
            AddExecutor(ExecutorType.Activate, CardId.SprightGammaBurst, SprightGammaBurstEffect);

            // ── Tier 8: Spell & Trap Sets ──
            AddExecutor(ExecutorType.SpellSet, CardId.SprightDoubleCross, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.SprightSmashers, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.SprightStarter, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.CrossoutDesignator, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet, DefaultSpellSet);

            // ── Tier 9: Monster Reposition ──
            AddExecutor(ExecutorType.Repos, MonsterReposOverride);
        }

        public override bool OnSelectHand() => true;

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _sprightBlueSearchUsed = false;
            _sprightJetSearchUsed = false;
            _sprightStarterUsed = false;
            _sprightSmashersUsed = false;
            _sprightGammaBurstUsed = false;
            _sprightDoubleCrossUsed = false;
            _giganticSprightUsed = false;
            _sprightElfReviveUsed = false;
            _sprightSprindDumpUsed = false;
            _sprightSprindBounceUsed = false;
            _prosperityUsedThisTurn = false;
            _maxxCActivatedThisTurn = false;
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.ToadallyAwesome
                || card.Id == CardId.SprightElf
                || card.Id == CardId.GiganticSpright
                || card.Id == CardId.SprightRed
                || card.Id == CardId.SprightCarrot
                || card.Id == CardId.OnibimaruSoulSweeper
                || card.Id == CardId.CatShark
                || card.Id == CardId.IPMasquerena
                || card.Id == CardId.KnightmareUnicorn
                || card.Id == CardId.Zeus;
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (c.Controller == 1) return 1; // Opponent monsters first!
            if (c.Id == CardId.NimbleAngler) return 2;
            if (c.Id == CardId.NimbleBeaver) return 3;
            if (c.Id == CardId.Ronintoadin) return 4;
            if (c.Id == CardId.DupeFrog) return 5;
            if (c.Id == CardId.SwapFrog) return 6;
            if (c.Id == CardId.SprightPixies) return 7;
            if (c.Id == CardId.SprightJet && _sprightJetSearchUsed) return 8;
            if (c.Id == CardId.SprightBlue && _sprightBlueSearchUsed) return 9;
            if (c.Id == CardId.SprightCarrot) return 20;
            if (c.Id == CardId.SprightRed) return 25;
            if (c.Id == CardId.SprightSprind) return 30;
            if (c.Id == CardId.GiganticSpright) return 40;
            if (c.Id == CardId.SprightElf) return 50;
            if (c.Id == CardId.ToadallyAwesome) return 900; // Strictly protect Toadally Awesome!
            return base.GetMaterialPriority(c);
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 0: QUICK NEGATES, HANDTRAPS & DISRUPTIONS
        // ═══════════════════════════════════════════════════════════════

        private bool ToadallyAwesomeNegateEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Duel.LastChainPlayer != 1) return false;

            // Select Aqua monster to tribute: prioritize Swap Frog / Dupe Frog / Ronintoadin / Toadally itself
            var tribute = Bot.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && (c.Race == (int)CardRace.Aqua || c.Id == CardId.SwapFrog || c.Id == CardId.DupeFrog || c.Id == CardId.Ronintoadin || c.Id == CardId.ToadallyAwesome))
                .OrderBy(c => c.Id == CardId.Ronintoadin ? 1 :
                              c.Id == CardId.DupeFrog ? 2 :
                              c.Id == CardId.SwapFrog ? 3 :
                              c.Id == CardId.ToadallyAwesome ? 10 : 5)
                .FirstOrDefault();

            if (tribute != null)
            {
                AI.SelectCard(tribute);
                return true;
            }
            return false;
        }

        private bool SprightRedNegateEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Duel.LastChainPlayer != 1) return false;

            var lastCard = LastChainCard;
            if (lastCard == null || !lastCard.IsMonster()) return false;

            // Select Level/Rank/Link 2 monster to tribute (Rank/Link 2 gives Destroy effect as bonus)
            var tribute = Bot.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && c != Card && (c.Level == 2 || c.Rank == 2 || (c.HasType(CardType.Link) && c.LinkCount == 2)))
                .OrderBy(c => c.Id == CardId.NimbleBeaver ? 1 :
                              c.Id == CardId.NimbleAngler ? 2 :
                              c.Id == CardId.Ronintoadin ? 3 :
                              c.Id == CardId.SwapFrog ? 4 :
                              c.Id == CardId.SprightJet ? 5 :
                              c.Id == CardId.SprightBlue ? 6 :
                              c.Id == CardId.SprightCarrot ? 7 :
                              c.Id == CardId.GiganticSpright ? 8 :
                              c.Id == CardId.SprightSprind ? 9 : 20)
                .FirstOrDefault();

            if (tribute != null)
            {
                AI.SelectCard(tribute);
                return true;
            }
            return false;
        }

        private bool SprightCarrotNegateEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Duel.LastChainPlayer != 1) return false;

            var lastCard = LastChainCard;
            if (lastCard == null || (!lastCard.HasType(CardType.Spell) && !lastCard.HasType(CardType.Trap))) return false;

            var tribute = Bot.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && c != Card && (c.Level == 2 || c.Rank == 2 || (c.HasType(CardType.Link) && c.LinkCount == 2)))
                .OrderBy(c => c.Id == CardId.NimbleBeaver ? 1 :
                              c.Id == CardId.Ronintoadin ? 2 :
                              c.Id == CardId.SwapFrog ? 3 :
                              c.Id == CardId.SprightJet ? 4 :
                              c.Id == CardId.SprightBlue ? 5 :
                              c.Id == CardId.GiganticSpright ? 6 :
                              c.Id == CardId.SprightSprind ? 7 : 20)
                .FirstOrDefault();

            if (tribute != null)
            {
                AI.SelectCard(tribute);
                return true;
            }
            return false;
        }

        private bool GhostOgreEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            var lastCard = LastChainCard;
            if (lastCard != null && (lastCard.Location == CardLocation.MonsterZone || lastCard.Location == CardLocation.SpellZone))
            {
                return true;
            }
            return false;
        }

        private bool AshBlossomEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool MaxxCEffect()
        {
            if (_maxxCActivatedThisTurn) return false;
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                _maxxCActivatedThisTurn = true;
                return true;
            }
            return DefaultMaxxC();
        }

        private bool CalledByTheGraveEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                return DefaultCalledByTheGrave();
            }
            return false;
        }

        private bool CrossoutDesignatorEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                var lastCard = LastChainCard;
                if (lastCard != null && Bot.Deck.Any(c => c != null && c.Id == lastCard.Id))
                {
                    AI.SelectCard(lastCard.Id);
                    return true;
                }
            }
            return false;
        }

        private bool ForbiddenDropletEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                var chainCard = LastChainCard;
                if (chainCard != null && chainCard.IsMonster() && chainCard.IsFaceup() && !chainCard.IsDisabled())
                    return true;
            }
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep || Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
            {
                var oppEffectMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsDisabled() && c.HasType(CardType.Effect)).ToList();
                if (oppEffectMonsters.Count > 0 && (Bot.Hand.Count > 1 || Bot.GetMonsterCount() > 2))
                    return true;
            }
            return false;
        }

        private bool SprightSmashersEffect()
        {
            if (_sprightSmashersUsed) return false;

            bool hasSprightCostInHandOrGY = Bot.Hand.Any(c => c != null && c != Card && IsSprightCard(c.Id))
                                        || Bot.Graveyard.Any(c => c != null && IsSprightCard(c.Id));

            if (!hasSprightCostInHandOrGY) return false;

            bool hasLv2OnField = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.Level == 2 || c.Rank == 2 || (c.HasType(CardType.Link) && c.LinkCount == 2)));
            if (!hasLv2OnField) return false;

            var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup()) ?? Enemy.GetSpells().FirstOrDefault(c => c != null);
            if (target != null)
            {
                _sprightSmashersUsed = true;
                return true;
            }
            return false;
        }

        private bool SprightDoubleCrossEffect()
        {
            if (_sprightDoubleCrossUsed) return false;

            bool hasRank2 = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Rank == 2);
            bool hasElf = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id == CardId.SprightElf);

            if (hasRank2 || hasElf)
            {
                var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup()) ?? Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsMonster());
                if (target != null)
                {
                    _sprightDoubleCrossUsed = true;
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool SprightSprindBounceEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_sprightSprindBounceUsed) return false;

            var xyzWithMat = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.HasType(CardType.Xyz) && c.Overlays.Count > 0);
            if (xyzWithMat == null) return false;

            var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup());
            if (target != null)
            {
                _sprightSprindBounceUsed = true;
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool SprightElfQuickReviveEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_sprightElfReviveUsed) return false;
            if (Duel.Player != 1) return false; // Opponent turn Quick Revive

            // Prioritize reviving Toadally Awesome, Spright Red, or Swap Frog
            var target = Bot.Graveyard
                .Where(c => c != null && c.IsMonster() && c.IsCanRevive() && (c.Level == 2 || c.Rank == 2 || (c.HasType(CardType.Link) && c.LinkCount == 2)))
                .OrderBy(c => c.Id == CardId.ToadallyAwesome ? 1 :
                              c.Id == CardId.SprightRed ? 2 :
                              c.Id == CardId.SprightCarrot ? 3 :
                              c.Id == CardId.SwapFrog ? 4 :
                              c.Id == CardId.SprightBlue ? 5 : 10)
                .FirstOrDefault();

            if (target != null)
            {
                _sprightElfReviveUsed = true;
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool IPMasquerenaEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Duel.Player != 1) return false; // Opponent turn Link summon

            if (Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 1 && Bot.GetMonsterCount() >= 2)
            {
                return true;
            }
            return false;
        }

        private bool SprightPixiesEffect()
        {
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep || Duel.Phase == DuelPhase.Damage)
            {
                var botMon = Bot.BattlingMonster;
                var oppMon = Enemy.BattlingMonster;
                if (botMon != null && oppMon != null && (botMon.Level == 2 || botMon.Rank == 2 || (botMon.HasType(CardType.Link) && botMon.LinkCount == 2)))
                {
                    if (botMon.Attack <= oppMon.Attack || oppMon.Attack >= 1500)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 1 & 2: BOARD BREAKERS, SEARCH & SETUP
        // ═══════════════════════════════════════════════════════════════

        private bool HarpiesFeatherDusterEffect()
        {
            return Enemy.GetSpellCount() > 0;
        }

        private bool PotOfProsperityEffect()
        {
            if (_prosperityUsedThisTurn) return false;
            _prosperityUsedThisTurn = true;
            return true;
        }

        private bool SprightStarterEffect()
        {
            if (_sprightStarterUsed) return false;
            _sprightStarterUsed = true;
            return true;
        }

        private bool SprightBlueSearchEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_sprightBlueSearchUsed) return false;
                _sprightBlueSearchUsed = true;
                return true;
            }
            return false;
        }

        private bool SprightJetSearchEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_sprightJetSearchUsed) return false;
                _sprightJetSearchUsed = true;
                return true;
            }
            return false;
        }

        private bool NimbleAnglerEffect() => true;

        // ═══════════════════════════════════════════════════════════════
        //  TIER 3 & 4: FROG / NIMBLE STARTERS & INHERENT SUMMONS
        // ═══════════════════════════════════════════════════════════════

        private bool NimbleBeaverSummon() => true;

        private bool NimbleBeaverEffect() => true;

        private bool SwapFrogSpSummonHand()
        {
            var waterCost = Bot.Hand.FirstOrDefault(c => c != null && c != Card && c.Attribute == (int)CardAttribute.Water);
            if (waterCost != null)
            {
                AI.SelectCard(waterCost);
                return true;
            }
            return false;
        }

        private bool SwapFrogSummon() => true;

        private bool SwapFrogTriggerEffect() => true;

        private bool RonintoadinReviveEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                var frogCost = Bot.Graveyard.FirstOrDefault(c => c != null && c != Card && (c.Id == CardId.SwapFrog || c.Id == CardId.DupeFrog));
                if (frogCost != null)
                {
                    AI.SelectCard(frogCost);
                    return true;
                }
            }
            return false;
        }

        private bool DupeFrogSearchEffect() => true;

        private bool SprightBlueHandSpSummon()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.Level == 2 || c.Rank == 2));
        }

        private bool SprightJetHandSpSummon()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.Level == 2 || c.Rank == 2));
        }

        private bool SprightRedHandSpSummon()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.Level == 2 || (c.HasType(CardType.Link) && c.LinkCount == 2)));
        }

        private bool SprightCarrotHandSpSummon()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.Level == 2 || (c.HasType(CardType.Link) && c.LinkCount == 2)));
        }

        private bool SprightPixiesHandSpSummon()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.Level == 2 || c.Rank == 2));
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 4: EXTRA DECK SUMMONS & BOSSES
        // ═══════════════════════════════════════════════════════════════

        private bool GiganticSprightSummon()
        {
            if (_giganticSprightUsed) return false;
            int lv2Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && (c.Level == 2 || (c.HasType(CardType.Link) && c.LinkCount == 2)) && !IsAceCard(c));
            return lv2Count >= 2;
        }

        private bool GiganticSprightEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_giganticSprightUsed) return false;
            _giganticSprightUsed = true;
            return true;
        }

        private bool SprightSprindSummon()
        {
            if (_sprightSprindDumpUsed) return false;
            int lv2Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            return lv2Count >= 2 && Bot.Deck.Any(c => c != null && c.Id == CardId.NimbleAngler);
        }

        private bool SprightSprindDumpEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_sprightSprindDumpUsed) return false;
            _sprightSprindDumpUsed = true;
            return true;
        }

        private bool SprightElfSummon()
        {
            int monstersCount = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !c.IsCode(CardId.ToadallyAwesome));
            bool hasLvRkLk2 = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.Level == 2 || c.Rank == 2 || (c.HasType(CardType.Link) && c.LinkCount == 2)));
            return monstersCount >= 2 && hasLvRkLk2 && !Bot.HasInMonstersZone(CardId.SprightElf);
        }

        private bool SprightElfMainReviveEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_sprightElfReviveUsed) return false;
            if (Duel.Player != 0) return false; // Our Turn Revive

            var target = Bot.Graveyard
                .Where(c => c != null && c.IsMonster() && c.IsCanRevive() && (c.Level == 2 || c.Rank == 2 || (c.HasType(CardType.Link) && c.LinkCount == 2)))
                .OrderBy(c => c.Id == CardId.SwapFrog ? 1 :
                              c.Id == CardId.SprightBlue && !_sprightBlueSearchUsed ? 2 :
                              c.Id == CardId.SprightJet && !_sprightJetSearchUsed ? 3 :
                              c.Id == CardId.SprightRed ? 4 :
                              c.Id == CardId.SprightCarrot ? 5 :
                              c.Id == CardId.NimbleBeaver ? 6 : 10)
                .FirstOrDefault();

            if (target != null)
            {
                _sprightElfReviveUsed = true;
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool ToadallyAwesomeSummon()
        {
            int aquaLv2Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 2 && (c.Id == CardId.SwapFrog || c.Id == CardId.DupeFrog || c.Id == CardId.Ronintoadin));
            return aquaLv2Count >= 2;
        }

        private bool ToadallyAwesomeStandbyEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            return true;
        }

        private bool OnibimaruSummon()
        {
            int lv2Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 2 && !IsAceCard(c));
            return lv2Count >= 2 && Enemy.GetMonsterCount() > 0;
        }

        private bool OnibimaruEffect()
        {
            var target = Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool CatSharkSummon()
        {
            int lv2Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 2 && !IsAceCard(c));
            return lv2Count >= 2 && Bot.HasInMonstersZone(CardId.GiganticSpright) && Duel.Phase == DuelPhase.Main1;
        }

        private bool CatSharkEffect()
        {
            var target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.GiganticSpright);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool IPMasquerenaSummon()
        {
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            return mats >= 2 && !Bot.HasInMonstersZone(CardId.IPMasquerena) && Duel.Turn == 1;
        }

        private bool KnightmareUnicornSummon()
        {
            return Enemy.GetMonsterCount() + Enemy.GetSpellCount() > 0 && Bot.GetMonsterCount() >= 3;
        }

        private bool KnightmareUnicornEffect()
        {
            var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup()) ?? Enemy.GetSpells().FirstOrDefault(c => c != null);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool DownerdMagicianSummon()
        {
            return Duel.Phase == DuelPhase.Main2 && Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Rank <= 3 && c.HasType(CardType.Xyz));
        }

        private bool ZeusSummon()
        {
            return Duel.Phase == DuelPhase.Main2 && Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Xyz));
        }

        private bool ZeusEffect()
        {
            if (Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 2)
            {
                return true;
            }
            return false;
        }

        private bool SprightGammaBurstEffect()
        {
            if (_sprightGammaBurstUsed) return false;
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep || Duel.Phase == DuelPhase.Main1)
            {
                int lv2Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && (c.Level == 2 || c.Rank == 2 || (c.HasType(CardType.Link) && c.LinkCount == 2)));
                if (lv2Count >= 2)
                {
                    _sprightGammaBurstUsed = true;
                    return true;
                }
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TACTICAL DECISION OVERRIDES (OnSelectCard / Position / Option)
        // ═══════════════════════════════════════════════════════════════

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // CRITICAL: Extra Deck Banish for Pot of Prosperity
            // STRICT RULE: PROTECT SPRIGHT ELF, GIGANTIC SPRIGHT & TOADALLY AWESOME!
            if (cards.All(c => c.Location == CardLocation.Extra))
            {
                var safeBanishList = cards
                    .Where(c => c != null && c.Id != CardId.SprightElf && c.Id != CardId.GiganticSpright && c.Id != CardId.ToadallyAwesome)
                    .OrderBy(c => c.Id == CardId.NinjaShadowMosquito ? 1 :
                                  c.Id == CardId.MannequinCat ? 2 :
                                  c.Id == CardId.KnightmareUnicorn ? 3 :
                                  c.Id == CardId.DownerdMagician ? 4 :
                                  c.Id == CardId.Zeus ? 5 :
                                  c.Id == CardId.CatShark ? 6 :
                                  c.Id == CardId.OnibimaruSoulSweeper ? 7 : 10)
                    .Take(max).ToList();

                if (safeBanishList.Count >= min)
                    return safeBanishList;
            }

            // Ronintoadin GY Banish Cost Selection
            if (LastChainCard != null && LastChainCard.Id == CardId.Ronintoadin)
            {
                var frogCost = cards.Where(c => c != null && (c.Id == CardId.SwapFrog || c.Id == CardId.DupeFrog)).Take(max).ToList();
                if (frogCost.Count >= min) return frogCost;
            }

            // Gigantic Spright Special Summon from Deck
            if (LastChainCard != null && LastChainCard.Id == CardId.GiganticSpright)
            {
                if (hint == HINT_SELECT_DETACH)
                {
                    return SelectPreferredCard(cards, min, max, CardId.NimbleBeaver, CardId.NimbleAngler, CardId.SprightJet, CardId.SprightBlue, CardId.SwapFrog);
                }
                if (hint == HINT_SELECT_SPSUMMON || hint == 0)
                {
                    if (!Bot.HasInMonstersZone(CardId.ToadallyAwesome))
                    {
                        return SelectPreferredCard(cards, min, max,
                            CardId.SwapFrog,
                            CardId.SprightBlue,
                            CardId.SprightJet,
                            CardId.SprightRed,
                            CardId.SprightCarrot);
                    }
                    else
                    {
                        return SelectPreferredCard(cards, min, max,
                            CardId.SprightBlue,
                            CardId.SprightJet,
                            CardId.SprightRed,
                            CardId.SprightCarrot,
                            CardId.SwapFrog);
                    }
                }
            }

            // Spright Blue Search Trigger
            if (LastChainCard != null && LastChainCard.Id == CardId.SprightBlue && hint == HINT_SELECT_TOHAND)
            {
                return SelectPreferredCard(cards, min, max,
                    CardId.SprightJet,
                    CardId.SprightRed,
                    CardId.SprightCarrot,
                    CardId.SprightPixies);
            }

            // Spright Jet Search Trigger
            if (LastChainCard != null && LastChainCard.Id == CardId.SprightJet && hint == HINT_SELECT_TOHAND)
            {
                return SelectPreferredCard(cards, min, max,
                    CardId.SprightStarter,
                    CardId.SprightSmashers,
                    CardId.SprightDoubleCross,
                    CardId.SprightGammaBurst);
            }

            // Spright Starter Special Summon from Deck
            if (LastChainCard != null && LastChainCard.Id == CardId.SprightStarter && (hint == HINT_SELECT_SPSUMMON || hint == 0))
            {
                return SelectPreferredCard(cards, min, max,
                    CardId.SprightBlue,
                    CardId.SprightJet,
                    CardId.SprightRed,
                    CardId.SprightCarrot);
            }

            // Spright Sprind Dump Trigger
            if (LastChainCard != null && LastChainCard.Id == CardId.SprightSprind && (hint == HINT_SELECT_TOGRAVE || hint == 0))
            {
                return SelectPreferredCard(cards, min, max,
                    CardId.NimbleAngler,
                    CardId.Ronintoadin,
                    CardId.SwapFrog,
                    CardId.DupeFrog);
            }

            // Nimble Beaver Normal Summon Trigger
            if (LastChainCard != null && LastChainCard.Id == CardId.NimbleBeaver && (hint == HINT_SELECT_SPSUMMON || hint == 0))
            {
                return SelectPreferredCard(cards, min, max,
                    CardId.NimbleBeaver,
                    CardId.NimbleAngler);
            }

            // Nimble Angler GY Trigger
            if (LastChainCard != null && LastChainCard.Id == CardId.NimbleAngler && (hint == HINT_SELECT_SPSUMMON || hint == 0))
            {
                return SelectPreferredCard(cards, min, max,
                    CardId.NimbleBeaver,
                    CardId.NimbleBeaver);
            }

            // Swap Frog Summon Dump Trigger
            if (LastChainCard != null && LastChainCard.Id == CardId.SwapFrog && (hint == HINT_SELECT_TOGRAVE || hint == 0))
            {
                return SelectPreferredCard(cards, min, max,
                    CardId.Ronintoadin,
                    CardId.DupeFrog,
                    CardId.SwapFrog);
            }

            // Toadally Awesome Standby Phase Summon
            if (LastChainCard != null && LastChainCard.Id == CardId.ToadallyAwesome)
            {
                if (hint == HINT_SELECT_DETACH)
                {
                    return SelectPreferredCard(cards, min, max, CardId.SwapFrog, CardId.DupeFrog, CardId.Ronintoadin);
                }
                if (hint == HINT_SELECT_SPSUMMON)
                {
                    return SelectPreferredCard(cards, min, max, CardId.DupeFrog, CardId.SwapFrog);
                }
                if (hint == HINT_SELECT_TOHAND)
                {
                    return SelectPreferredCard(cards, min, max, CardId.ToadallyAwesome, CardId.SwapFrog, CardId.DupeFrog);
                }
            }

            // Spright Smashers banish cost from hand/GY
            if (LastChainCard != null && LastChainCard.Id == CardId.SprightSmashers)
            {
                if (hint == HINT_SELECT_BANISH || hint == 0)
                {
                    var sprightCost = cards.Where(c => c != null && (c.Location == CardLocation.Hand || c.Location == CardLocation.Grave) && IsSprightCard(c.Id)).Take(max).ToList();
                    if (sprightCost.Count >= min) return sprightCost;

                    var ourMon = cards.Where(c => c != null && c.Controller == 0).OrderBy(c => GetMaterialPriority(c)).Take(1).ToList();
                    var oppCard = cards.Where(c => c != null && c.Controller == 1).Take(1).ToList();
                    var combined = ourMon.Concat(oppCard).ToList();
                    if (combined.Count >= min) return combined;
                }
            }

            // Pot of Prosperity Excavated Cards Selection
            if (LastChainCard != null && LastChainCard.Id == CardId.PotOfProsperity && hint == HINT_SELECT_TOHAND)
            {
                return SelectPreferredCard(cards, min, max,
                    CardId.SprightStarter,
                    CardId.NimbleBeaver,
                    CardId.SwapFrog,
                    CardId.SprightBlue,
                    CardId.SprightJet,
                    CardId.CalledByTheGrave,
                    CardId.CrossoutDesignator,
                    CardId.SprightSmashers,
                    CardId.SprightDoubleCross,
                    CardId.ForbiddenDroplet,
                    CardId.AshBlossom,
                    CardId.MaxxC);
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.SprightRed || cardId == CardId.SprightCarrot || cardId == CardId.SwapFrog || cardId == CardId.DupeFrog || cardId == CardId.Ronintoadin || cardId == CardId.CatShark)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            }
            if (cardId == CardId.GiganticSpright || cardId == CardId.ToadallyAwesome || cardId == CardId.OnibimaruSoulSweeper || cardId == CardId.Zeus || cardId == CardId.KnightmareUnicorn)
            {
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }
            return base.OnSelectPosition(cardId, positions);
        }

        private bool IsSprightCard(int id)
        {
            return id == CardId.SprightBlue
                || id == CardId.SprightJet
                || id == CardId.SprightRed
                || id == CardId.SprightCarrot
                || id == CardId.SprightPixies
                || id == CardId.SprightStarter
                || id == CardId.SprightSmashers
                || id == CardId.SprightGammaBurst
                || id == CardId.SprightDoubleCross
                || id == CardId.GiganticSpright
                || id == CardId.SprightElf
                || id == CardId.SprightSprind;
        }

        private IList<ClientCard> SelectPreferredCard(IList<ClientCard> cards, int min, int max, params int[] preferredIds)
        {
            var result = new List<ClientCard>();
            foreach (int id in preferredIds)
            {
                var matches = cards.Where(c => c != null && c.Id == id && !result.Contains(c)).ToList();
                foreach (var m in matches)
                {
                    result.Add(m);
                    if (result.Count >= max) break;
                }
                if (result.Count >= max) break;
            }
            if (result.Count < min)
            {
                foreach (var card in cards)
                {
                    if (card != null && !result.Contains(card))
                    {
                        result.Add(card);
                        if (result.Count >= min) break;
                    }
                }
            }
            return result;
        }

        private bool MonsterReposOverride()
        {
            if (Card == null) return false;
            if (IsAceCard(Card))
            {
                if (Card.Id == CardId.ToadallyAwesome || Card.Id == CardId.GiganticSpright || Card.Id == CardId.OnibimaruSoulSweeper || Card.Id == CardId.Zeus)
                {
                    if (Card.IsAttack()) return false;
                    return true;
                }
                if (Card.Id == CardId.CatShark)
                {
                    if (Card.IsDefense()) return false;
                    return true;
                }
            }

            if (Card.Id == CardId.DupeFrog || Card.Id == CardId.Ronintoadin)
            {
                if (Card.IsDefense()) return false;
                return true;
            }

            bool enemyEmpty = Enemy.GetMonsterCount() == 0;
            if (Card.IsAttack())
            {
                if (!enemyEmpty && Card.Attack < 1200) return true;
            }
            else
            {
                if (enemyEmpty || Card.Defense < Card.Attack) return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  BATTLE & ATTACK LOGIC
        // ═══════════════════════════════════════════════════════════════

        public override BattlePhaseAction OnBattle(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            if (attackers == null || attackers.Count == 0)
                return null;

            // Direct attack scenario
            if (defenders == null || defenders.Count == 0)
            {
                var directAttacker = attackers
                    .Where(c => c != null && c.IsFaceup() && c.IsAttack() && c.Attack > 0)
                    .OrderBy(c => IsAceCard(c) ? 0 : 1)
                    .ThenByDescending(c => c.Attack)
                    .FirstOrDefault();

                if (directAttacker != null)
                {
                    return AI.Attack(directAttacker, null);
                }
            }

            // Combat against defenders
            foreach (var attacker in attackers.Where(c => c != null && c.IsFaceup() && c.IsAttack()).OrderByDescending(c => c.Attack))
            {
                foreach (var defender in defenders.Where(d => d != null))
                {
                    if (defender.IsFaceup() && defender.IsAttack() && attacker.Attack > defender.Attack)
                        return AI.Attack(attacker, defender);
                    if (defender.IsFaceup() && defender.IsDefense() && attacker.Attack > defender.Defense)
                        return AI.Attack(attacker, defender);
                    if (defender.IsFacedown() && attacker.Attack >= 1600)
                        return AI.Attack(attacker, defender);
                }
            }

            return null;
        }
    }
}
