// ============================================================
// CARD AUDIT — 2026_MagistusFairy
// Verified against cards.cdb and 2026_MagistusFairy.ydk
// | Card Name                          | Type         | OPT? | Effect Summary                                              |
// |------------------------------------|--------------|------|-------------------------------------------------------------|
// | Fairy Tail - Luna (86937530)       | Monster Lv4  | SOPT | NS: Search 1850 ATK Spellcaster. Quick: bounce self + opp.  |
// | Fairy Tail - Snow (55623480)       | Monster Lv4  | None | NS/SS: Book of Moon opp. Quick in GY: banish 7 to SS self.  |
// | Fairy Tail - Matchlille (19144622) | Monster Lv4  | HOPT | SS if 1850 ATK on field. NS/SS: Search Fairy Tail S/T.      |
// |                                    |              |      | Field: Pay 500 LP -> turn opp monster into "Fairy Prince".  |
// | Danger!? Tsuchinoko? (99745551)    | Monster Lv3  | HOPT | Hand reveal -> discard/SS + draw. GY discard -> SS self.    |
// | Spoon, the Seal of Magistus        | Monster Lv4  | HOPT | Hand: Discard -> Search Magistus monster.                   |
// |   (42544773)                       |              |      | GY: Banish -> Equip Magistus from ED/GY to our monster.     |
// | Crowley, the Gifted (875572)       | Monster Lv4  | HOPT | Hand: SS when added to hand. NS/SS: Fusion Magistus/Invoked.|
// | Zoroa, the Magistus of Flame       | Monster Lv4  | HOPT | NS/SS: Equip Magistus from ED -> SS Lv4 Spellcaster from GY.|
// |   (36099130)                       | (Tuner)      |      |                                                             |
// | Rilliona, the Magistus (72498838)  | Monster Lv4  | HOPT | NS/SS: Search Magistus S/T. GY: Banish -> Equip from GY.    |
// | Witchcrafter Genni (64756282)      | Monster Lv1  | HOPT | GY: Banish self + Witchcrafter spell (Lacrima) to copy.     |
// | Regulus, Prince of Endymion        | Monster Lv4  | HOPT | Hand: Reveal Spellcaster -> SS self. SS: Search Empire.     |
// |   (96228804)                       |              |      |                                                             |
// | Fairy Tail Ball (56725612)         | Cont. Spell  | HOPT | On activate: Search Fairy Tail. When opp SS: negate & Prince|
// | Tales of Fairy Tail (82119326)     | Equip Spell  | HOPT | Fuse Spellcaster using hand/field + opp "Fairy Prince"!     |
// |                                    |              |      | GY: Equip to Fairy Tail + Normal Summon 1850 ATK.           |
// | Fairy Tail Long Long Ago(19326613) | Quick Spell  | HOPT | SS up to 1 LIGHT Fairy Tail each from Hand/GY/Banish.       |
// | Verre Magic - Lacrima (73664385)   | Quick Spell  | HOPT | Dump Snow/Spell to GY; or SS Magistus when opp activates.   |
// | Endymion Empire (34041788)         | Cont. Spell  | HOPT | Search Regulus; SS Spellcaster from hand if opp has monster.|
// | Forbidden Crown (98829635)         | Quick Spell  | HOPT | Freeze & negate 1 face-up monster on field completely.      |
// | Super Polymerization (48130397)    | Quick Spell  | None | Fuse opp monsters into Garura or Mudragon.                  |
// | Chronicler of Fairy Tail (4026187) | Fusion Lv4   | HOPT | Fused with Fairy Prince -> WIPE OPP BOARD + burn 500 each!  |
// |                                    |              |      | Quick: Banish Fairy Tail from Hand/GY -> Omni-negate & pop. |
// | Weaver of Fairy Tails (78021082)   | Fusion Lv4   | HOPT | Main: SS Fairy Tail from deck. Non-EARTH SS: negate & Prince|
// | Zoroa Verethragna (37260677)       | Fusion Lv8   | HOPT | Equip steal opp monster. Quick: Send Magistus -> Negate+pop.|
// | Invoked Mechaba (75286621)         | Fusion Lv9   | SOPT | Quick: Discard same card type -> Omni-negate & banish.     |
// | Selene Queen (45819647)           | Link-3       | SOPT | Spell counters -> Quick: SS Spellcaster from hand/GY.       |
// | Four Charmers (27519978)          | Link-4       | HOPT | Quick in Main: SS 2 monsters from GY for lethal push.       |
// | Artemis Moon Maiden (34755994)     | Link-1       | HOPT | While equipped: Search Magistus monster.                    |
// | Fairy Tail - Wickat (27632520)     | Xyz Rk4      | HOPT | Detach -> Dump Fairy Tails. LIGHT Fairy Tails unaffected.   |
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_MagistusFairy", "2026_MagistusFairy")]
    public class _2026_MagistusFairyExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int FairyTailLuna = 86937530;
            public const int FairyTailSnow = 55623480;
            public const int FairyTailMatchlille = 19144622;
            public const int DangerTsuchinoko = 99745551;
            public const int SpoonTheSealOfMagistus = 42544773;
            public const int CrowleyTheGiftedOfMagistus = 875572;
            public const int ZoroaTheMagistusOfFlame = 36099130;
            public const int RillionaTheMagistusOfVerre = 72498838;
            public const int WitchcrafterGenni = 64756282;
            public const int RegulusThePrinceOfEndymion = 96228804;

            // Handtraps
            public const int MulcharmyFuwalos = 42141493;
            public const int DrollAndLockBird = 94145021;
            public const int AshBlossom = 14558128; // Deck uses alt-art 14558128

            // Main Deck Spells
            public const int FairyTailLongLongAgo = 19326613;
            public const int TalesOfFairyTail = 82119326;
            public const int FairyTailBall = 56725612;
            public const int VerreMagicLacrimaOfLight = 73664385;
            public const int EndymionEmpire = 34041788;
            public const int ForbiddenCrown = 98829635;
            public const int SuperPolymerization = 48130397;
            public const int InstantFusion = 1845204;
            public const int CalledByTheGrave = 24224830;

            // Extra Deck
            public const int WeaverOfFairyTails = 78021082;
            public const int ChroniclerOfFairyTailTales = 4026187;
            public const int ZoroaTheMagistusVerethragna = 37260677;
            public const int MagistusChorozo = 66532962;
            public const int InvokedMechaba = 75286621;
            public const int GaruraWingsOfResonantLife = 11765832;
            public const int MudragonOfTheSwamp = 54757758;
            public const int SummonSorceress = 61665245;
            public const int SeleneQueenOfTheMasterMagicians = 45819647;
            public const int FourCharmersInProfusion = 27519978;
            public const int ArtemisTheMagistusMoonMaiden = 34755994;
            public const int EndymionTheCrescentMagistus = 20714553;
            public const int FairyTailWiccat = 27632520;

            // Fairy Prince Token / Alias ID in cards.cdb
            public const int FairyPrince = 10000120;
        }

        private bool _lunaSearched = false;
        private bool _snowSummonedThisTurn = false;
        private bool _matchlilleEffectUsed = false;
        private bool _talesFusedThisTurn = false;

        private static readonly HashSet<int> HandTraps = new HashSet<int>
        {
            CardId.MulcharmyFuwalos,
            CardId.DrollAndLockBird,
            CardId.AshBlossom
        };

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.IsCode(
                CardId.ChroniclerOfFairyTailTales,
                CardId.InvokedMechaba,
                CardId.ZoroaTheMagistusVerethragna,
                CardId.SeleneQueenOfTheMasterMagicians,
                CardId.FourCharmersInProfusion,
                CardId.WeaverOfFairyTails
            ) || base.IsAceCard(card);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.IsCode(CardId.ZoroaTheMagistusOfFlame) && HasEquippedMagistus(c)) return 800;
            if (c.IsCode(CardId.DangerTsuchinoko, CardId.WitchcrafterGenni)) return 10;
            if (c.IsCode(CardId.FairyTailSnow, CardId.FairyTailMatchlille)) return 30;
            if (c.IsCode(CardId.CrowleyTheGiftedOfMagistus, CardId.SpoonTheSealOfMagistus)) return 40;
            return 100;
        }

        protected override bool IsBoardStrongEnough()
        {
            int bossCount = Bot.GetMonsters().Count(m => m != null && m.IsFaceup() && IsAceCard(m));
            if (bossCount >= 2) return true;
            if (Bot.HasInMonstersZone(CardId.InvokedMechaba) && Bot.HasInMonstersZone(CardId.ChroniclerOfFairyTailTales)) return true;
            if (Bot.HasInMonstersZone(CardId.FourCharmersInProfusion)) return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (Bot.HasInMonstersZone(CardId.FourCharmersInProfusion) ||
                (Bot.HasInMonstersZone(CardId.InvokedMechaba) && Bot.HasInMonstersZone(CardId.ChroniclerOfFairyTailTales)))
            {
                return base.ShouldStopExtending();
            }
            return false;
        }

        public _2026_MagistusFairyExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Register Ace Cards
            HeuristicGuard.RegisterAceCards(
                CardId.ChroniclerOfFairyTailTales,
                CardId.InvokedMechaba,
                CardId.ZoroaTheMagistusVerethragna,
                CardId.SeleneQueenOfTheMasterMagicians,
                CardId.FourCharmersInProfusion,
                CardId.WeaverOfFairyTails
            );

            // Combo Router: Sequencing
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Zoroa-Artemis-Crowley-Mechaba",
                RequiredCards = new List<int> { CardId.ZoroaTheMagistusOfFlame },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.ZoroaTheMagistusOfFlame, ActionType = ExecutorType.Summon, Description = "NS Zoroa" },
                    new() { CardId = CardId.ZoroaTheMagistusOfFlame, ActionType = ExecutorType.Activate, Description = "Equip Artemis" },
                    new() { CardId = CardId.ArtemisTheMagistusMoonMaiden, ActionType = ExecutorType.Activate, Description = "Artemis search Crowley" },
                    new() { CardId = CardId.CrowleyTheGiftedOfMagistus, ActionType = ExecutorType.Activate, Description = "Crowley SS self" },
                    new() { CardId = CardId.CrowleyTheGiftedOfMagistus, ActionType = ExecutorType.Activate, Description = "Crowley fuse Mechaba" }
                },
                EndBoardScore = 90
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "FairyPrince-Tales-BoardWipe",
                RequiredCards = new List<int> { CardId.FairyTailMatchlille, CardId.TalesOfFairyTail },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.FairyTailMatchlille, ActionType = ExecutorType.Summon, Description = "Summon Matchlille" },
                    new() { CardId = CardId.FairyTailMatchlille, ActionType = ExecutorType.Activate, Description = "Turn opp to Fairy Prince" },
                    new() { CardId = CardId.TalesOfFairyTail, ActionType = ExecutorType.Activate, Description = "Fuse with Fairy Prince into Chronicler" }
                },
                EndBoardScore = 95
            });

            // Bait Planner
            BaitPlanner.RegisterComboStarters(
                CardId.ZoroaTheMagistusOfFlame,
                CardId.SpoonTheSealOfMagistus,
                CardId.FairyTailBall,
                CardId.FairyTailLuna,
                CardId.RillionaTheMagistusOfVerre
            );
            BaitPlanner.RegisterBaitCards(
                CardId.DangerTsuchinoko,
                CardId.EndymionEmpire,
                CardId.SpoonTheSealOfMagistus
            );

            // Chain Advisor
            ChainAdvisor.RegisterHighValueTargets(
                CardId.InvokedMechaba,
                CardId.ChroniclerOfFairyTailTales,
                CardId.ZoroaTheMagistusVerethragna,
                CardId.ZoroaTheMagistusOfFlame
            );

            // ═════════════════════════════════════════════════════════════
            // TIER 1: Omni-Negations, Disruptions & Handtraps
            // ═════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdEffect);

            // Boss Field Quick Negates
            AddExecutor(ExecutorType.Activate, CardId.InvokedMechaba, MechabaEffect);
            AddExecutor(ExecutorType.Activate, CardId.ChroniclerOfFairyTailTales, ChroniclerEffect);
            AddExecutor(ExecutorType.Activate, CardId.ZoroaTheMagistusVerethragna, VerethragnaEffect);

            // Board Breaking & Fast Disruptions
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationEffect);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenCrown, ForbiddenCrownEffect);
            AddExecutor(ExecutorType.Activate, CardId.FairyTailLuna, LunaEffect);
            AddExecutor(ExecutorType.Activate, CardId.FairyTailSnow, SnowFieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.FairyTailSnow, SnowGyEffect);

            // ═════════════════════════════════════════════════════════════
            // TIER 2: Main Combos, Spells & Field Actions
            // ═════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.InstantFusion, InstantFusionEffect);
            AddExecutor(ExecutorType.Activate, CardId.EndymionEmpire, EndymionEmpireEffect);
            AddExecutor(ExecutorType.Activate, CardId.FairyTailBall, FairyTailBallEffect);
            AddExecutor(ExecutorType.Activate, CardId.TalesOfFairyTail, TalesOfFairyTailEffect);
            AddExecutor(ExecutorType.Activate, CardId.FairyTailLongLongAgo, FairyTailLongLongAgoEffect);
            AddExecutor(ExecutorType.Activate, CardId.VerreMagicLacrimaOfLight, LacrimaEffect);

            // ═════════════════════════════════════════════════════════════
            // TIER 3: Starters, Extenders & Monster Summons
            // ═════════════════════════════════════════════════════════════
            // Danger!? Tsuchinoko? (Hand activation to discard & SS, or GY trigger)
            AddExecutor(ExecutorType.Activate, CardId.DangerTsuchinoko, DangerTsuchinokoEffect);

            // Spoon: Hand discard search / GY banish equip
            AddExecutor(ExecutorType.Activate, CardId.SpoonTheSealOfMagistus, SpoonEffect);

            // Regulus: Hand reveal SS / search Empire on SS
            AddExecutor(ExecutorType.Activate, CardId.RegulusThePrinceOfEndymion, RegulusEffect);

            // Fairy Tail - Matchlille: Hand/GY SS if 1850 ATK on field / search S/T / Prince rename
            AddExecutor(ExecutorType.Activate, CardId.FairyTailMatchlille, MatchlilleEffect);

            // Crowley: Hand SS when added / NS/SS Fusion summon
            AddExecutor(ExecutorType.Activate, CardId.CrowleyTheGiftedOfMagistus, CrowleyEffect);

            // Normal Summons in Strategic Priority Order
            AddExecutor(ExecutorType.Summon, CardId.ZoroaTheMagistusOfFlame, ZoroaSummon);
            AddExecutor(ExecutorType.Activate, CardId.ZoroaTheMagistusOfFlame, ZoroaEffect);

            AddExecutor(ExecutorType.Summon, CardId.FairyTailLuna, LunaSummon);
            AddExecutor(ExecutorType.Summon, CardId.FairyTailMatchlille, MatchlilleSummon);
            AddExecutor(ExecutorType.Summon, CardId.RillionaTheMagistusOfVerre, RillionaSummon);
            AddExecutor(ExecutorType.Activate, CardId.RillionaTheMagistusOfVerre, RillionaEffect);

            AddExecutor(ExecutorType.Summon, CardId.CrowleyTheGiftedOfMagistus, CrowleySummon);
            AddExecutor(ExecutorType.Summon, CardId.SpoonTheSealOfMagistus, SpoonSummon);
            AddExecutor(ExecutorType.Summon, CardId.RegulusThePrinceOfEndymion, RegulusSummon);

            // Artemis equip search in S/T zone
            AddExecutor(ExecutorType.Activate, CardId.ArtemisTheMagistusMoonMaiden, ArtemisEffect);

            // Witchcrafter Genni GY copy effect
            AddExecutor(ExecutorType.Activate, CardId.WitchcrafterGenni, GenniEffect);

            // ═════════════════════════════════════════════════════════════
            // TIER 4: Extra Deck Extenders & Finishers
            // ═════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.ArtemisTheMagistusMoonMaiden, ArtemisSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.FairyTailWiccat, WickatSummon);
            AddExecutor(ExecutorType.Activate, CardId.FairyTailWiccat, WickatEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.WeaverOfFairyTails, WeaverSummon);
            AddExecutor(ExecutorType.Activate, CardId.WeaverOfFairyTails, WeaverEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.ChroniclerOfFairyTailTales, ChroniclerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.InvokedMechaba, MechabaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ZoroaTheMagistusVerethragna, VerethragnaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.MagistusChorozo, ChorozoSummon);
            AddExecutor(ExecutorType.Activate, CardId.MagistusChorozo, ChorozoEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.EndymionTheCrescentMagistus, CrescentSummon);
            AddExecutor(ExecutorType.Activate, CardId.EndymionTheCrescentMagistus, CrescentEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.SeleneQueenOfTheMasterMagicians, SeleneSummon);
            AddExecutor(ExecutorType.Activate, CardId.SeleneQueenOfTheMasterMagicians, SeleneEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.SummonSorceress, SummonSorceressSummon);
            AddExecutor(ExecutorType.Activate, CardId.SummonSorceress, SummonSorceressEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.FourCharmersInProfusion, FourCharmersSummon);
            AddExecutor(ExecutorType.Activate, CardId.FourCharmersInProfusion, FourCharmersEffect);

            // Standard fallback sets & battle repos
            AddExecutor(ExecutorType.SpellSet, DefaultSpellSet);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override bool OnSelectHand()
        {
            return true; // Go first to set up Mechaba / Chronicler / Snow control board
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _lunaSearched = false;
            _snowSummonedThisTurn = false;
            _matchlilleEffectUsed = false;
            _talesFusedThisTurn = false;
        }

        private bool HasEquippedMagistus(ClientCard card)
        {
            if (card == null || card.Location != CardLocation.MonsterZone) return false;
            return Bot.GetSpells().Any(c => c != null && c.IsFaceup() &&
                c.IsCode(CardId.ArtemisTheMagistusMoonMaiden, CardId.ZoroaTheMagistusVerethragna,
                         CardId.EndymionTheCrescentMagistus, CardId.MagistusChorozo,
                         CardId.TalesOfFairyTail));
        }

        private IList<ClientCard> SortMaterials(IList<ClientCard> cards, int max)
        {
            var sorted = cards.OrderBy(c => {
                if (c == null) return 9999;
                if (c.Controller == 0)
                {
                    // Highest protection
                    if (IsAceCard(c)) return 50000;
                    if (c.IsCode(CardId.ZoroaTheMagistusOfFlame) && HasEquippedMagistus(c)) return 40000;
                    if (HandTraps.Contains(c.Id)) return 8000;
                    if (c.IsCode(CardId.ZoroaTheMagistusOfFlame)) return 5000;
                    if (c.IsCode(CardId.FairyTailLuna) && Duel.Player == 1) return 4000;
                    if (c.IsCode(CardId.RillionaTheMagistusOfVerre)) return 3000;

                    // Prefer to use
                    if (c.Level == 0 || !c.HasType(CardType.Effect)) return 1;
                    if (c.IsCode(CardId.WitchcrafterGenni)) return 5;
                    if (c.IsCode(CardId.DangerTsuchinoko)) return 10;
                    if (c.IsCode(CardId.FairyTailSnow)) return 20;
                    if (c.IsCode(CardId.FairyTailMatchlille)) return 30;
                    if (c.IsCode(CardId.CrowleyTheGiftedOfMagistus)) return 40;
                    if (c.IsCode(CardId.SpoonTheSealOfMagistus)) return 50;
                    if (c.IsCode(CardId.RegulusThePrinceOfEndymion)) return 60;
                    if (c.IsCode(CardId.FairyTailLuna) && Duel.Player == 0) return 70;
                }
                return 100;
            }).ToList();
            return sorted.Take(max).ToList();
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // ── Hint 533: Fusion Material Selection ──
            // Always prioritize opponent's "Fairy Prince" when fusing with Tales of Fairy Tail!
            if (hint == 533)
            {
                var oppPrince = cards.Where(c => c != null && c.Controller == 1 &&
                    (c.IsCode(CardId.FairyPrince) || c.Name == "Fairy Prince")).ToList();
                var oppOthers = cards.Where(c => c != null && c.Controller == 1 &&
                    !c.IsCode(CardId.FairyPrince) && c.Name != "Fairy Prince").ToList();
                var ourNonAces = cards.Where(c => c != null && c.Controller == 0 && !IsAceCard(c))
                    .OrderBy(GetMaterialPriority).ToList();
                var ourAces = cards.Where(c => c != null && c.Controller == 0 && IsAceCard(c)).ToList();

                var candidateList = oppPrince.Concat(oppOthers).Concat(ourNonAces).Concat(ourAces).ToList();
                if (candidateList.Count >= min)
                {
                    return candidateList.Take(Math.Max(min, Math.Min(max, candidateList.Count))).ToList();
                }
            }

            // ── Hint 501: Discard Selection ──
            // Prioritize Snow (GY active), Genni (GY copy), Danger, duplicate spells
            if (hint == 501)
            {
                var sortedDiscard = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (c.IsCode(CardId.FairyTailSnow)) return 1;
                    if (c.IsCode(CardId.WitchcrafterGenni)) return 2;
                    if (c.IsCode(CardId.DangerTsuchinoko)) return 3;
                    if (c.IsCode(CardId.TalesOfFairyTail)) return 4;
                    if (c.IsCode(CardId.FairyTailLongLongAgo)) return 5;
                    if (Bot.Hand.Count(h => h != null && h.Id == c.Id) > 1) return 10;
                    if (HandTraps.Contains(c.Id)) return 80;
                    return 50;
                }).ToList();
                if (sortedDiscard.Count >= min)
                {
                    return sortedDiscard.Take(Math.Max(min, Math.Min(max, sortedDiscard.Count))).ToList();
                }
            }

            // ── Hint 504: Banish Selection (Snow 7-card cost or GY banish) ──
            if (hint == 504)
            {
                var sortedBanish = cards.OrderBy(c => {
                    if (c == null) return 99999;
                    // Banish spent Spells/Traps in GY first
                    if (c.Location == CardLocation.Grave && !c.IsMonster()) return 10;
                    // Banish low-impact monsters in GY
                    if (c.Location == CardLocation.Grave)
                    {
                        if (c.IsCode(CardId.DangerTsuchinoko, CardId.WitchcrafterGenni)) return 20;
                        if (HandTraps.Contains(c.Id)) return 30;
                        if (c.IsCode(CardId.SpoonTheSealOfMagistus, CardId.CrowleyTheGiftedOfMagistus)) return 40;
                        if (c.IsCode(CardId.RillionaTheMagistusOfVerre, CardId.RegulusThePrinceOfEndymion)) return 50;
                        if (IsAceCard(c)) return 5000;
                        return 100;
                    }
                    // Avoid banishing from hand or field
                    if (c.Location == CardLocation.Hand) return 10000;
                    if (c.Location == CardLocation.MonsterZone || c.Location == CardLocation.SpellZone)
                    {
                        if (IsAceCard(c)) return 99999;
                        return 20000;
                    }
                    return 500;
                }).ToList();

                if (sortedBanish.Count >= min)
                {
                    return sortedBanish.Take(Math.Max(min, Math.Min(max, sortedBanish.Count))).ToList();
                }
            }

            // ── Hint 508: Send to Graveyard Selection (Lacrima, Wickat) ──
            if (hint == 508)
            {
                var dumpPriority = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (c.IsCode(CardId.FairyTailSnow)) return 1;
                    if (c.IsCode(CardId.TalesOfFairyTail)) return 2;
                    if (c.IsCode(CardId.FairyTailLongLongAgo)) return 3;
                    if (c.IsCode(CardId.FairyTailMatchlille)) return 4;
                    if (c.IsCode(CardId.WitchcrafterGenni)) return 5;
                    return 50;
                }).ToList();
                if (dumpPriority.Count >= min)
                {
                    return dumpPriority.Take(Math.Max(min, Math.Min(max, dumpPriority.Count))).ToList();
                }
            }

            // ── Hint 505: Add to Hand (Searcher) ──
            if (hint == 505)
            {
                var searchPriority = cards.OrderBy(c => {
                    if (c == null) return 999;
                    // Magistus Search Targets
                    if (c.IsCode(CardId.ZoroaTheMagistusOfFlame) && !Bot.HasInHand(CardId.ZoroaTheMagistusOfFlame)) return 1;
                    if (c.IsCode(CardId.CrowleyTheGiftedOfMagistus) && !Bot.HasInHand(CardId.CrowleyTheGiftedOfMagistus)) return 2;
                    if (c.IsCode(CardId.SpoonTheSealOfMagistus) && !Bot.HasInHand(CardId.SpoonTheSealOfMagistus)) return 3;
                    // Fairy Tail Search Targets
                    if (c.IsCode(CardId.TalesOfFairyTail) && !Bot.HasInHand(CardId.TalesOfFairyTail)) return 4;
                    if (c.IsCode(CardId.FairyTailMatchlille) && !Bot.HasInHand(CardId.FairyTailMatchlille)) return 5;
                    if (c.IsCode(CardId.FairyTailBall) && !Bot.HasInHand(CardId.FairyTailBall)) return 6;
                    if (c.IsCode(CardId.FairyTailLuna) && !Bot.HasInHand(CardId.FairyTailLuna)) return 7;
                    if (c.IsCode(CardId.FairyTailSnow) && !Bot.HasInHand(CardId.FairyTailSnow)) return 8;
                    // Endymion Targets
                    if (c.IsCode(CardId.EndymionEmpire) && !Bot.HasInHand(CardId.EndymionEmpire)) return 9;
                    if (c.IsCode(CardId.RegulusThePrinceOfEndymion) && !Bot.HasInHand(CardId.RegulusThePrinceOfEndymion)) return 10;
                    return 50;
                }).ToList();
                if (searchPriority.Count >= min)
                {
                    return searchPriority.Take(Math.Max(min, Math.Min(max, searchPriority.Count))).ToList();
                }
            }

            // ── Hint 502 / 503 / 551 / 552: Target/Destroy/Negate Opponent Cards ──
            if (hint == 502 || hint == 503 || hint == 551 || hint == 552 || hint == 572 || hint == 575)
            {
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).OrderByDescending(c => {
                    if (c.IsMonster())
                    {
                        int score = c.Attack;
                        if (c.IsExtraCard()) score += 2000;
                        if (!c.IsDisabled()) score += 1000;
                        return score;
                    }
                    return 500;
                }).ToList();
                if (enemyCards.Count >= min)
                {
                    return enemyCards.Take(Math.Max(min, Math.Min(max, enemyCards.Count))).ToList();
                }
            }

            // ── Hint 509: Special Summon Selection ──
            if (hint == 509)
            {
                var sortedSpSummon = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (IsAceCard(c)) return 1;
                    if (c.IsCode(CardId.ZoroaTheMagistusOfFlame)) return 2;
                    if (c.IsCode(CardId.FairyTailLuna)) return 3;
                    if (c.IsCode(CardId.FairyTailMatchlille)) return 4;
                    if (c.IsCode(CardId.CrowleyTheGiftedOfMagistus)) return 5;
                    if (c.IsCode(CardId.FairyTailSnow)) return 6;
                    return 50;
                }).ToList();
                if (sortedSpSummon.Count >= min)
                {
                    return sortedSpSummon.Take(Math.Max(min, Math.Min(max, sortedSpSummon.Count))).ToList();
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override IList<ClientCard> OnSelectXyzMaterial(IList<ClientCard> cards, int min, int max)
        {
            return SortMaterials(cards, max);
        }

        public override IList<ClientCard> OnSelectSynchroMaterial(IList<ClientCard> cards, int sum, int min, int max)
        {
            return SortMaterials(cards, max);
        }

        public override IList<ClientCard> OnSelectLinkMaterial(IList<ClientCard> cards, int min, int max)
        {
            if (max == 1) // Artemis Link-1
            {
                int[] magistusIds = {
                    CardId.SpoonTheSealOfMagistus,
                    CardId.CrowleyTheGiftedOfMagistus,
                    CardId.RillionaTheMagistusOfVerre,
                    CardId.ZoroaTheMagistusOfFlame
                };
                foreach (int mid in magistusIds)
                {
                    var mat = cards.FirstOrDefault(c => c != null && c.IsCode(mid) && !HasEquippedMagistus(c));
                    if (mat != null) return new[] { mat };
                }
            }
            return SortMaterials(cards, max);
        }

        public override int OnSelectOption(IList<long> options)
        {
            // Default to option 0 (Search effects / Main effects)
            return 0;
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            int[] defMonsters = {
                CardId.RillionaTheMagistusOfVerre,
                CardId.CrowleyTheGiftedOfMagistus,
                CardId.SpoonTheSealOfMagistus,
                CardId.WitchcrafterGenni,
                CardId.AshBlossom,
                CardId.MulcharmyFuwalos,
                CardId.DrollAndLockBird
            };

            if (defMonsters.Contains(cardId) && positions.Contains(CardPosition.FaceUpDefence))
            {
                return CardPosition.FaceUpDefence;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        // ═════════════════════════════════════════════════════════════
        // TIER 1: Omni-Negates & Disruptions
        // ═════════════════════════════════════════════════════════════

        private bool CalledByTheGraveEffect()
        {
            return DefaultCalledByTheGrave();
        }

        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (Util.GetLastChainCard() == null || Util.GetLastChainCard().Controller != 1) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool MulcharmyFuwalosEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }

        private bool DrollAndLockBirdEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1;
        }

        private bool MechabaEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            var lastCard = Util.GetLastChainCard();
            if (lastCard == null || lastCard.Controller != 1) return false;

            // Must discard same card type (Monster, Spell, Trap) from hand
            CardType requiredType = CardType.Monster;
            if (lastCard.IsSpell()) requiredType = CardType.Spell;
            else if (lastCard.IsTrap()) requiredType = CardType.Trap;

            var discardCandidate = Bot.Hand.FirstOrDefault(c => c != null && c.HasType(requiredType));
            if (discardCandidate != null)
            {
                AI.SelectCard(discardCandidate);
                return true;
            }
            return false;
        }

        private bool ChroniclerEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            var lastCard = Util.GetLastChainCard();
            if (lastCard == null || lastCard.Controller != 1) return false;

            // Quick effect: Banish 1 Fairy Tail card from hand or GY (except Fairy Tail Tales)
            var banishTarget = Bot.Hand.Concat(Bot.Graveyard)
                .FirstOrDefault(c => c != null &&
                    c.IsCode(CardId.FairyTailLuna, CardId.FairyTailSnow, CardId.FairyTailMatchlille,
                             CardId.FairyTailBall, CardId.FairyTailLongLongAgo, CardId.FairyTailWiccat));

            if (banishTarget != null)
            {
                AI.SelectCard(banishTarget);
                return true;
            }
            return false;
        }

        private bool VerethragnaEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;

            // Effect 1: Equip steal opp monster or from GY
            if (ActivateDescription == Util.GetStringId(CardId.ZoroaTheMagistusVerethragna, 0) ||
                Duel.LastChainPlayer != 1)
            {
                var oppTarget = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && IsViableEffectTarget(m))
                    ?? Bot.Graveyard.FirstOrDefault(m => m != null && m.IsMonster());
                if (oppTarget != null)
                {
                    AI.SelectCard(oppTarget);
                    return true;
                }
            }

            // Effect 2: Quick monster negate -> send face-up Magistus monster card to GY
            var lastCard = Util.GetLastChainCard();
            if (lastCard != null && lastCard.Controller == 1 && lastCard.IsMonster())
            {
                var magistusCost = Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() &&
                    c.IsCode(CardId.ArtemisTheMagistusMoonMaiden, CardId.EndymionTheCrescentMagistus))
                    ?? Bot.GetMonsters().FirstOrDefault(m => m != null && m != Card && m.IsFaceup() &&
                        m.IsCode(CardId.ZoroaTheMagistusOfFlame, CardId.RillionaTheMagistusOfVerre, CardId.CrowleyTheGiftedOfMagistus));

                if (magistusCost != null)
                {
                    AI.SelectCard(magistusCost);
                    return true;
                }
            }
            return false;
        }

        private bool ForbiddenCrownEffect()
        {
            if (Card.Location != CardLocation.Hand && Card.Location != CardLocation.SpellZone) return false;

            if (Duel.LastChainPlayer == 1)
            {
                var lastCard = Util.GetLastChainCard();
                if (lastCard != null && lastCard.Controller == 1 && lastCard.IsMonster() && !lastCard.IsDisabled() && lastCard.IsFaceup())
                {
                    AI.SelectCard(lastCard);
                    return true;
                }
            }

            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                var target = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && !m.IsDisabled() && (m.IsMonsterDangerous() || m.IsExtraCard()));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool SuperPolymerizationEffect()
        {
            if (Bot.Hand.Count < (Card.Location == CardLocation.Hand ? 2 : 1)) return false;

            var allMonsters = Bot.GetMonsters().Concat(Enemy.GetMonsters()).Where(c => c != null && c.IsFaceup()).ToList();
            if (allMonsters.Count < 2) return false;

            bool canGarura = false;
            for (int i = 0; i < allMonsters.Count; i++)
            {
                for (int j = i + 1; j < allMonsters.Count; j++)
                {
                    if (allMonsters[i].Race == allMonsters[j].Race &&
                        allMonsters[i].Attribute == allMonsters[j].Attribute &&
                        allMonsters[i].Id != allMonsters[j].Id)
                    {
                        canGarura = true;
                        break;
                    }
                }
                if (canGarura) break;
            }

            bool canMudragon = false;
            for (int i = 0; i < allMonsters.Count; i++)
            {
                for (int j = i + 1; j < allMonsters.Count; j++)
                {
                    if (allMonsters[i].Attribute == allMonsters[j].Attribute &&
                        allMonsters[i].Race != allMonsters[j].Race)
                    {
                        canMudragon = true;
                        break;
                    }
                }
                if (canMudragon) break;
            }

            if (canGarura || canMudragon)
            {
                AI.SelectCard(new[] {
                    CardId.FairyTailSnow,
                    CardId.WitchcrafterGenni,
                    CardId.DangerTsuchinoko,
                    CardId.AshBlossom,
                    CardId.DrollAndLockBird
                });
                AI.SelectNextCard(new[] {
                    CardId.GaruraWingsOfResonantLife,
                    CardId.MudragonOfTheSwamp
                });
                return true;
            }
            return false;
        }

        private bool LunaEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Search effect on NS
            if (ActivateDescription == Util.GetStringId(CardId.FairyTailLuna, 0))
            {
                if (!_lunaSearched)
                {
                    _lunaSearched = true;
                    AI.SelectCard(CardId.FairyTailMatchlille, CardId.FairyTailSnow, CardId.FairyTailLuna);
                    return true;
                }
            }

            // Quick bounce effect
            if (Duel.Player == 1 || Duel.Phase == DuelPhase.BattleStart)
            {
                var target = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && IsViableEffectTarget(m) &&
                    (m.Attack >= 1850 || m.IsExtraCard()));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool SnowFieldEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            var target = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && IsViableEffectTarget(m));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool SnowGyEffect()
        {
            if (Card.Location != CardLocation.Grave || _snowSummonedThisTurn) return false;
            if (IsSpecialSummonBlocked()) return false;

            // Check if we have at least 7 cards to banish
            int totalPool = (Bot.Graveyard.Count - 1) + Bot.Hand.Count +
                Bot.GetMonsters().Count(m => m != null && !IsAceCard(m)) +
                Bot.GetSpells().Count(s => s != null && s.IsCode(CardId.InstantFusion, CardId.VerreMagicLacrimaOfLight));

            if (totalPool < 7) return false;

            bool oppTurn = Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Main2);
            bool endPhase = Duel.Phase == DuelPhase.End;
            bool lethalPush = Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2) && Bot.GetMonsterCount() < 4;

            if (oppTurn || endPhase || lethalPush)
            {
                _snowSummonedThisTurn = true;
                return true;
            }
            return false;
        }

        // ═════════════════════════════════════════════════════════════
        // TIER 2: Spells & Field Actions
        // ═════════════════════════════════════════════════════════════

        private bool InstantFusionEffect()
        {
            if (IsSpecialSummonBlocked() || Bot.LifePoints <= 1000) return false;
            AI.SelectCard(CardId.ChroniclerOfFairyTailTales, CardId.MudragonOfTheSwamp);
            return true;
        }

        private bool EndymionEmpireEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (Bot.HasInSpellZone(CardId.EndymionEmpire)) return false;
                AI.SelectCard(CardId.RegulusThePrinceOfEndymion);
                return true;
            }
            return true;
        }

        private bool FairyTailBallEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (Bot.HasInSpellZone(CardId.FairyTailBall)) return false;
                AI.SelectCard(CardId.FairyTailMatchlille, CardId.TalesOfFairyTail, CardId.FairyTailLuna, CardId.FairyTailSnow);
                return true;
            }

            // On-field trigger: Negate opponent SS monster and rename to Fairy Prince
            var oppTarget = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && !m.IsDisabled() && IsViableEffectTarget(m));
            if (oppTarget != null)
            {
                AI.SelectCard(oppTarget);
                return true;
            }
            return false;
        }

        private bool TalesOfFairyTailEffect()
        {
            // Equip from hand to 1850 ATK Spellcaster
            if (Card.Location == CardLocation.Hand)
            {
                var equipTarget = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() &&
                    m.HasRace(CardRace.SpellCaster) && (m.Attack == 1850 || m.IsCode(CardId.ZoroaTheMagistusOfFlame)));
                if (equipTarget != null)
                {
                    AI.SelectCard(equipTarget);
                    return true;
                }
                return false;
            }

            // On-field Fusion effect: Fuse using hand/field + opponent's Fairy Prince!
            if (Card.Location == CardLocation.SpellZone && !_talesFusedThisTurn)
            {
                if (IsSpecialSummonBlocked()) return false;
                _talesFusedThisTurn = true;
                AI.SelectCard(CardId.ChroniclerOfFairyTailTales, CardId.WeaverOfFairyTails, CardId.ZoroaTheMagistusVerethragna);
                return true;
            }

            // GY effect: Equip to Fairy Tail monster + Normal Summon 1850 ATK Spellcaster
            if (Card.Location == CardLocation.Grave)
            {
                var ftMonster = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() &&
                    m.IsCode(CardId.FairyTailLuna, CardId.FairyTailMatchlille, CardId.FairyTailSnow,
                             CardId.WeaverOfFairyTails, CardId.ChroniclerOfFairyTailTales));
                if (ftMonster != null)
                {
                    AI.SelectCard(ftMonster);
                    return true;
                }
            }
            return false;
        }

        private bool FairyTailLongLongAgoEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            // SS up to 1 LIGHT Fairy Tail each from Hand, GY, Banishment
            return true;
        }

        private bool LacrimaEffect()
        {
            // Mode 1: Send Spellcaster (Snow) from Deck to GY
            if (Bot.GetMonsters().Any(m => m != null && m.IsFaceup() &&
                m.IsCode(CardId.ZoroaTheMagistusOfFlame, CardId.RillionaTheMagistusOfVerre,
                         CardId.CrowleyTheGiftedOfMagistus, CardId.SpoonTheSealOfMagistus,
                         CardId.WitchcrafterGenni)))
            {
                AI.SelectCard(CardId.FairyTailSnow, CardId.TalesOfFairyTail, CardId.FairyTailLongLongAgo);
                return true;
            }

            // Mode 2: Opponent activates effect -> SS Magistus from Deck
            if (Duel.LastChainPlayer == 1)
            {
                AI.SelectCard(CardId.ZoroaTheMagistusOfFlame, CardId.RillionaTheMagistusOfVerre);
                return true;
            }
            return false;
        }

        // ═════════════════════════════════════════════════════════════
        // TIER 3: Starters, Extenders & Normal Summons
        // ═════════════════════════════════════════════════════════════

        private bool DangerTsuchinokoEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            // Activate in hand to reveal & discard, or in GY when discarded
            return Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave;
        }

        private bool SpoonEffect()
        {
            // Hand: Discard to search Magistus monster
            if (Card.Location == CardLocation.Hand)
            {
                if (GetRemainingCount(CardId.ZoroaTheMagistusOfFlame) > 0 && !Bot.HasInHand(CardId.ZoroaTheMagistusOfFlame))
                    AI.SelectCard(CardId.ZoroaTheMagistusOfFlame);
                else if (GetRemainingCount(CardId.CrowleyTheGiftedOfMagistus) > 0 && !Bot.HasInHand(CardId.CrowleyTheGiftedOfMagistus))
                    AI.SelectCard(CardId.CrowleyTheGiftedOfMagistus);
                else
                    AI.SelectCard(CardId.RillionaTheMagistusOfVerre);
                return true;
            }

            // GY: Banish to equip Magistus from ED/GY to our monster
            if (Card.Location == CardLocation.Grave)
            {
                var targetMonster = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() &&
                    m.IsCode(CardId.ZoroaTheMagistusOfFlame, CardId.RillionaTheMagistusOfVerre, CardId.CrowleyTheGiftedOfMagistus));
                if (targetMonster != null)
                {
                    AI.SelectCard(targetMonster);
                    AI.SelectNextCard(CardId.ArtemisTheMagistusMoonMaiden, CardId.ZoroaTheMagistusVerethragna);
                    return true;
                }
            }
            return false;
        }

        private bool RegulusEffect()
        {
            // Hand: Reveal 1 other Spellcaster to Special Summon
            if (Card.Location == CardLocation.Hand)
            {
                if (IsSpecialSummonBlocked()) return false;
                var revealSpellcaster = Bot.Hand.FirstOrDefault(c => c != Card && c.HasRace(CardRace.SpellCaster));
                if (revealSpellcaster != null && Bot.LifePoints > revealSpellcaster.Level * 300)
                {
                    AI.SelectCard(revealSpellcaster);
                    return true;
                }
                return false;
            }

            // On SS: Search Endymion Empire
            if (Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(CardId.EndymionEmpire);
                return true;
            }
            return false;
        }

        private bool MatchlilleEffect()
        {
            // Hand/GY: Special Summon if 1850 original ATK on field
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave)
            {
                if (IsSpecialSummonBlocked()) return false;
                bool has1850 = Bot.GetMonsters().Concat(Enemy.GetMonsters()).Any(m => m != null && m.IsFaceup() &&
                    m.HasRace(CardRace.SpellCaster) && (m.Attack == 1850 ||
                    m.IsCode(CardId.FairyTailLuna, CardId.FairyTailSnow, CardId.FairyTailMatchlille,
                             CardId.WeaverOfFairyTails, CardId.ChroniclerOfFairyTailTales,
                             CardId.SeleneQueenOfTheMasterMagicians, CardId.FourCharmersInProfusion,
                             CardId.FairyTailWiccat, CardId.EndymionTheCrescentMagistus)));
                if (has1850) return true;
                return false;
            }

            // On NS/SS: Search Fairy Tail S/T
            if (ActivateDescription == Util.GetStringId(CardId.FairyTailMatchlille, 1) ||
                (Card.Location == CardLocation.MonsterZone && !_matchlilleEffectUsed))
            {
                _matchlilleEffectUsed = true;
                if (!Bot.HasInHand(CardId.TalesOfFairyTail) && GetRemainingCount(CardId.TalesOfFairyTail) > 0)
                    AI.SelectCard(CardId.TalesOfFairyTail);
                else if (!Bot.HasInHand(CardId.FairyTailBall) && GetRemainingCount(CardId.FairyTailBall) > 0)
                    AI.SelectCard(CardId.FairyTailBall);
                else
                    AI.SelectCard(CardId.FairyTailLongLongAgo);
                return true;
            }

            // Field: Pay 500 LP -> make 1 opp Effect monster become "Fairy Prince"
            if (Bot.LifePoints > 500)
            {
                var oppTarget = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() &&
                    !m.IsCode(CardId.FairyPrince) && m.Name != "Fairy Prince" && IsViableEffectTarget(m));
                if (oppTarget != null)
                {
                    AI.SelectCard(oppTarget);
                    return true;
                }
            }
            return false;
        }

        private bool CrowleyEffect()
        {
            // Hand: Trigger SS when added to hand
            if (Card.Location == CardLocation.Hand)
            {
                return !IsSpecialSummonBlocked();
            }

            // On NS/SS: Fusion Summon Magistus or Invoked monster
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (IsSpecialSummonBlocked()) return false;
                // Crowley on field counts as "Aleister the Invoker"!
                // Fuses Invoked Mechaba with any LIGHT monster in Hand or Field
                bool hasLight = Bot.Hand.Concat(Bot.GetMonsters()).Any(c => c != Card && c.HasAttribute(CardAttribute.Light));
                if (hasLight)
                {
                    AI.SelectCard(CardId.InvokedMechaba, CardId.ZoroaTheMagistusVerethragna, CardId.WeaverOfFairyTails);
                    return true;
                }
                AI.SelectCard(CardId.ZoroaTheMagistusVerethragna, CardId.MagistusChorozo, CardId.WeaverOfFairyTails);
                return true;
            }
            return false;
        }

        private bool ZoroaSummon()
        {
            return true;
        }

        private bool ZoroaEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Effect 0: Equip Magistus from Extra Deck
            if (ActivateDescription == Util.GetStringId(CardId.ZoroaTheMagistusOfFlame, 0))
            {
                AI.SelectCard(CardId.ArtemisTheMagistusMoonMaiden, CardId.ZoroaTheMagistusVerethragna, CardId.EndymionTheCrescentMagistus);
                return true;
            }

            // Effect 1: SS Level 4 Spellcaster from hand/GY
            if (ActivateDescription == Util.GetStringId(CardId.ZoroaTheMagistusOfFlame, 1))
            {
                var candidates = Bot.Hand.Concat(Bot.Graveyard)
                    .Where(c => c != null && c.Level == 4 && c.HasRace(CardRace.SpellCaster) && c.Id != CardId.ZoroaTheMagistusOfFlame)
                    .ToList();
                if (candidates.Count > 0)
                {
                    AI.SelectCard(candidates.OrderByDescending(c => c.Attack).ToList());
                    return true;
                }
            }
            return false;
        }

        private bool LunaSummon()
        {
            return true;
        }

        private bool MatchlilleSummon()
        {
            return true;
        }

        private bool RillionaSummon()
        {
            return true;
        }

        private bool RillionaEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(CardId.VerreMagicLacrimaOfLight);
                return true;
            }

            if (Card.Location == CardLocation.Grave)
            {
                var target = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() &&
                    m.IsCode(CardId.ZoroaTheMagistusOfFlame, CardId.CrowleyTheGiftedOfMagistus, CardId.SpoonTheSealOfMagistus));
                if (target != null)
                {
                    AI.SelectCard(target);
                    AI.SelectNextCard(CardId.ArtemisTheMagistusMoonMaiden, CardId.ZoroaTheMagistusVerethragna);
                    return true;
                }
            }
            return false;
        }

        private bool CrowleySummon()
        {
            return true;
        }

        private bool SpoonSummon()
        {
            return true;
        }

        private bool RegulusSummon()
        {
            return true;
        }

        private bool ArtemisEffect()
        {
            // In SpellZone: Search Magistus monster
            if (Card.Location == CardLocation.SpellZone)
            {
                AI.SelectCard(CardId.CrowleyTheGiftedOfMagistus, CardId.ZoroaTheMagistusOfFlame, CardId.SpoonTheSealOfMagistus);
                return true;
            }

            // In MonsterZone: Equip itself to another Magistus monster
            if (Card.Location == CardLocation.MonsterZone)
            {
                var otherMagistus = Bot.GetMonsters().FirstOrDefault(m => m != null && m != Card && m.IsFaceup() &&
                    m.IsCode(CardId.ZoroaTheMagistusOfFlame, CardId.RillionaTheMagistusOfVerre, CardId.CrowleyTheGiftedOfMagistus));
                if (otherMagistus != null)
                {
                    AI.SelectCard(otherMagistus);
                    return true;
                }
            }
            return false;
        }

        private bool GenniEffect()
        {
            // GY: Banish Genni + Lacrima to dump another card
            if (Card.Location == CardLocation.Grave)
            {
                var lacrimaInGy = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCode(CardId.VerreMagicLacrimaOfLight));
                if (lacrimaInGy != null)
                {
                    AI.SelectCard(lacrimaInGy);
                    return true;
                }
            }
            return false;
        }

        // ═════════════════════════════════════════════════════════════
        // TIER 4: Extra Deck Extenders & Finishers
        // ═════════════════════════════════════════════════════════════

        private bool ArtemisSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            // Needs 1 Level 4 or lower Magistus (Spoon, Crowley, Rilliona, or unequipped Zoroa)
            var mat = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() &&
                m.IsCode(CardId.SpoonTheSealOfMagistus, CardId.CrowleyTheGiftedOfMagistus,
                         CardId.RillionaTheMagistusOfVerre, CardId.WitchcrafterGenni) && !HasEquippedMagistus(m));
            if (mat != null)
            {
                AI.SelectCard(mat);
                return true;
            }
            return false;
        }

        private bool WickatSummon()
        {
            if (IsSpecialSummonBlocked() || ShouldSkipLinkSummon()) return false;
            int expendableLv4 = Bot.GetMonsters().Count(m => m != null && m.IsFaceup() && m.Level == 4 &&
                m.HasRace(CardRace.SpellCaster) && !IsAceCard(m) && !HasEquippedMagistus(m));
            return expendableLv4 >= 2;
        }

        private bool WickatEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(CardId.FairyTailSnow, CardId.TalesOfFairyTail, CardId.FairyTailLongLongAgo);
                return true;
            }
            return false;
        }

        private bool WeaverSummon()
        {
            return !IsSpecialSummonBlocked();
        }

        private bool WeaverEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            // SS 1 Fairy Tail from Deck or banishment
            AI.SelectCard(CardId.FairyTailLuna, CardId.FairyTailMatchlille, CardId.FairyTailSnow);
            return true;
        }

        private bool ChroniclerSummon()
        {
            return !IsSpecialSummonBlocked();
        }

        private bool MechabaSummon()
        {
            return !IsSpecialSummonBlocked();
        }

        private bool VerethragnaSummon()
        {
            return !IsSpecialSummonBlocked();
        }

        private bool ChorozoSummon()
        {
            return !IsSpecialSummonBlocked();
        }

        private bool ChorozoEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            // Cancel attack + bounce attacking monster
            var attacking = Enemy.BattlingMonster ?? Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup());
            if (attacking != null)
            {
                AI.SelectCard(attacking);
                return true;
            }
            return false;
        }

        private bool CrescentSummon()
        {
            return !IsSpecialSummonBlocked();
        }

        private bool CrescentEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            return true;
        }

        private bool SeleneSummon()
        {
            if (IsSpecialSummonBlocked() || ShouldSkipLinkSummon()) return false;
            int mats = Bot.GetMonsters().Count(m => m != null && m.IsFaceup() && !IsAceCard(m) && !HasEquippedMagistus(m));
            return mats >= 3;
        }

        private bool SeleneEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            // SS Spellcaster from hand/GY
            AI.SelectCard(CardId.ZoroaTheMagistusOfFlame, CardId.FairyTailLuna, CardId.CrowleyTheGiftedOfMagistus, CardId.FairyTailSnow);
            return true;
        }

        private bool SummonSorceressSummon()
        {
            return false; // Disabled to prevent giving opponent free monsters (Anti-Advantage Gate)
        }

        private bool SummonSorceressEffect()
        {
            return false; // Anti-Advantage Gate: Never give opponent free cards
        }

        private bool FourCharmersSummon()
        {
            if (IsSpecialSummonBlocked() || ShouldSkipLinkSummon()) return false;
            // Only summon if we can make lethal push or strong board
            int mats = Bot.GetMonsters().Count(m => m != null && m.IsFaceup() && !IsAceCard(m) && !HasEquippedMagistus(m));
            return mats >= 4;
        }

        private bool FourCharmersEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            // SS 2 monsters from GY
            AI.SelectCard(CardId.ZoroaTheMagistusOfFlame, CardId.FairyTailLuna, CardId.CrowleyTheGiftedOfMagistus, CardId.FairyTailSnow);
            return true;
        }
    }
}
