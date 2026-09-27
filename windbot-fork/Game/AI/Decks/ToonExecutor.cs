using YGOSharp.OCGWrapper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    // ============================================================
    // CARD AUDIT โ€” 2026_Toon (OCG WCQ Guangzhou 1st Place)
    // ============================================================
    // | Card Name                        | Type       | OPT?   | Effect Summary                         | Activate When                         | NEVER When                              |
    // |----------------------------------|------------|--------|----------------------------------------|---------------------------------------|-----------------------------------------|
    // | Blue-Eyes Toon Dragon            | Monster    | No     | Toon 3000 ATK beater, direct attack    | Toon World on field                   | No Toon World                           |
    // | Toon Dark Magician              | Monster    | 1/turn | SS Toon from deck OR set Toon S/T      | Toon World on field, standby/MP       | No Toon World / no targets in deck      |
    // | Funny Dark Rabbit               | Monster    | 1/turn | Extra NS Toon + search Field/Cont Spell| After NS/SS                           | Already used / no targets               |
    // | Comic Cat                       | Monster    | HOPT   | Tribute 1 (opp if TW) โ’ SS Toon from deck| Main Phase, has tribute target      | No Toon World / no SS targets           |
    // | Evil Box                        | Monster    | HOPT   | SS self if TW + search Toon Trap       | Control Toon World                    | No Toon World / hand empty              |
    // | Dark-Eyes Illusionist FM        | Monster    | HOPT   | Discard โ’ Place Mind Scan OR revive Toon| Hand, need Mind Scan or GY recovery  | No targets / hand โค1                    |
    // | Toon Mermaid                    | Monster    | No     | SS self if TW (tribute-free Toon)      | Toon World on field                   | No Toon World / SS blocked              |
    // | Toon World the Perfect World    | Field Spell| 3/turn | Search any Toon/mention card from deck  | Field Zone empty or need search       | Already have face-up copy               |
    // | Toon Bookmark                   | Spell      | HOPT   | Search Toon card + protect Toon World  | Need search / protect field spell     | No targets in deck                      |
    // | Toon Table of Contents          | Spell      | No     | Search any "Toon" card                 | Always (thin deck)                    | No Toon cards in deck                   |
    // | Mind Scan                       | Cont Spell | HOPT   | Reveal opp hand + negate by name       | Toon card in field/GY                 | No Toon cards anywhere                  |
    // | Toon Terror                     | Trap       | HOPT   | Counter Trap: negate + destroy         | Opponent activates, control Toon mon  | No Toon monster on field                |
    // | Dominus Impulse                 | Trap       | HOPT   | SS from deck on opponent's SS          | Opponent SS, we have valid target     | No targets in deck                      |
    // | Blue-Eyes Toon Ultimate Dragon  | Fusion     | 1/turn | Contact Fusion: direct ATK, GY recycle | Have BETD + 2 Toon mons on field/hand | Materials not available                 |
    // ============================================================
    // ACE CARDS:
    //   Primary  : Blue-Eyes Toon Ultimate Dragon (4500 ATK direct attack boss)
    //   Secondary: Toon Dark Magician (recurring SS engine from deck)
    //   Tertiary : Blue-Eyes Toon Dragon (3000 ATK direct attacker)
    // COMBO STARTERS: Toon Table of Contents โ’ Toon Bookmark โ’ Toon World the Perfect World
    // CHOKEPOINTS: Toon World the Perfect World search (Ash = lose engine) โ’ Funny Dark Rabbit NS (Veiler/Imperm)
    // WIN CONDITION: Direct attack with Toon monsters bypassing opponent's field
    // GOING 1ST END BOARD: Toon World + Toon monster + Mind Scan (hand reveal + name negate) + Toon Terror set
    // GOING 2ND GAMEPLAN: Deploy Toon World โ’ SS Toon beaters โ’ direct attack for OTK
    // ============================================================

    // ============================================================
    // COMBO DRAFT โ€” 2026_Toon
    // ============================================================
    //
    // === COMBO LINE 1: Standard Setup (Starter: Toon Table / Toon Bookmark) ===
    // HAND REQUIRED: [Toon Table of Contents] or [Toon Bookmark] + any 1 card
    // STEP 1: Activate Toon Table of Contents
    //   โ’ Search Toon Bookmark (or Perfect World if have Bookmark)
    // STEP 2: Activate Toon Bookmark
    //   โ’ Search Toon World the Perfect World
    // STEP 3: Activate Toon World the Perfect World (Field Spell)
    //   โ’ Search x3: Funny Dark Rabbit, Comic Cat, Toon Dark Magician, etc.
    //   โ’ CHOKEPOINT: เธ–เนเธฒ Ash โ’ เนเธกเนเธกเธต search โ’ เธ•เนเธญเธ NS Funny Dark Rabbit เธ•เธฃเธเน
    // STEP 4: NS Funny Dark Rabbit
    //   โ’ Effect: Search Perfect World (if not have) / another Toon Spell
    //   โ’ Bonus: Extra NS for Toon monster
    // STEP 5: Extra NS Toon Mermaid / Comic Cat
    // STEP 6: Comic Cat Quick Effect: Tribute enemy โ’ SS Toon Dark Magician from deck
    // END BOARD: Toon World + 2-3 Toon monsters + Mind Scan + Toon Terror set
    //
    // === COMBO LINE 2: Going 2nd OTK ===
    // PRIORITY: Get Toon World โ’ SS Toon beaters โ’ direct attack
    // STEP 1: Activate field spell / search chain
    // STEP 2: SS Blue-Eyes Toon Dragon (direct 3000)
    // STEP 3: SS other Toon monsters (direct attack all)
    // STEP 4: Contact Fuse into Blue-Eyes Toon Ultimate Dragon if needed (4500 direct)
    // LETHAL CHECK: 8000 LP โ’ need 3000+3000+2000 or 4500+3500 direct
    //
    // === COMBO LINE 3: Grind / Fallback ===
    // Use Toon Bookmark protection to keep Toon World alive
    // Recycle with Blue-Eyes Toon Ultimate Dragon GY effect
    // Mind Scan to negate key opponent cards by name
    // ============================================================

    [Deck("2026_Toon", "2026_Toon")]
    public class _2026_ToonExecutor : ModernExecutor
    {
        public class CardId
        {
            // === Main Deck Toon Monsters ===
            public const int BlueEyesToonDragon = 53183600;
            public const int ToonDarkMagician = 21296502;
            public const int FunnyDarkRabbit = 100455001;
            public const int ComicCat = 100455002;
            public const int EvilBox = 100455003;
            public const int DarkEyesIllusionist = 100455004;
            public const int ToonMermaid = 65458948;

            // === Hand Traps ===
            public const int MulcharmyFuwalos = 42141493;
            public const int MulcharmyPurulia = 84192580;
            public const int GhostOgre = 59438930;
            public const int AshBlossom = 14558127;
            public const int MaxxC = 23434538;
            public const int DrollAndLockBird = 94145021;
            public const int PSYFramegearDelta = 74203495;
            public const int PSYFramegearGamma = 38814750;
            public const int PSYFrameDriver = 49036338;

            // === Spells ===
            public const int ToonWorldPerfect = 100455006;
            public const int ToonBookmark = 91500017;
            public const int ToonTableOfContents = 89997728;
            public const int MindScan = 100455007;
            public const int TripleTacticsTalent = 25311006;
            public const int Terraforming = 73628505;
            public const int CalledByTheGrave = 24224830;
            public const int DominusImpulse = 40366667;

            // === Traps ===
            public const int ToonTerror = 53094821;

            // === Extra Deck ===
            public const int BlueEyesToonUltimateDragon = 100455005;
            public const int MagistusChorozo = 66532962;
            public const int DarkMagicianOfDestruction = 59400890;
            public const int DragunityKnightLuin = 12496261;
            public const int BaronneDeFleur = 84815190;
            public const int Number60Dugares = 66011101;
            public const int RelinquishedAnima = 94259633;
            public const int CrossSheep = 50277355;
            public const int Lahamu = 53904087;
            public const int SPLittleKnight = 29301450;
            public const int WPFancyBall = 4993187;
            public const int SeleneQueenMasterMagicians = 45819647;
            public const int FourCharmers = 27519978;
            public const int DragunityLordGeorgius = 70522875;
        }

        // === Card Groups ===
        private static readonly int[] AceCardIds = {
            CardId.BlueEyesToonUltimateDragon,
            CardId.BlueEyesToonDragon,
            CardId.ToonDarkMagician
        };

        private static readonly int[] ToonMonsters = {
            CardId.BlueEyesToonDragon, CardId.ToonDarkMagician,
            CardId.FunnyDarkRabbit, CardId.ComicCat,
            CardId.EvilBox, CardId.ToonMermaid
        };

        // Monsters that are expendable as material (low value after using effect)
        private static readonly int[] ExpendableToonMonsters = {
            CardId.ToonMermaid, CardId.EvilBox, CardId.FunnyDarkRabbit, CardId.ComicCat
        };

        private static readonly int[] HandTraps = {
            CardId.AshBlossom, CardId.GhostOgre, CardId.MaxxC,
            CardId.DrollAndLockBird, CardId.MulcharmyFuwalos,
            CardId.MulcharmyPurulia, CardId.PSYFramegearGamma,
            CardId.PSYFramegearDelta
        };

        // Extra Deck monsters that should not be used as material
        private static readonly int[] ExtraDeckAces = {
            CardId.BlueEyesToonUltimateDragon, CardId.BaronneDeFleur, CardId.SPLittleKnight
        };

        // === OPT Flags ===
        private bool _funnyDarkRabbitSearchUsed;
        private bool _comicCatUsed;
        private bool _evilBoxSSUsed;
        private bool _evilBoxGYUsed;
        private bool _darkEyesUsed;
        private bool _toonBookmarkSearchUsed;
        private bool _mindScanNegateUsed;
        private bool _toonTerrorUsed;
        private int _perfectWorldSearchCount;
        private int _handTrapsUsedThisTurn;
        private bool _toonDarkMagicianUsed;

        // === Constructor ===
        public _2026_ToonExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            HeuristicGuard.RegisterAceCards(
                CardId.BlueEyesToonUltimateDragon,
                CardId.BlueEyesToonDragon,
                CardId.ToonDarkMagician
            );
            // โ”€โ”€ Combo Router: Sequencing โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Standard-Setup",
                RequiredCards = new List<int> { CardId.BlueEyesToonDragon, CardId.ToonDarkMagician },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.BlueEyesToonDragon, ActionType = ExecutorType.Activate, Description = "Play CardId.BlueEyesToonDragon" },
                    new() { CardId = CardId.ToonDarkMagician, ActionType = ExecutorType.Activate, Description = "Extend with CardId.ToonDarkMagician" }
                },
                EndBoardScore = 80
            });

            // โ”€โ”€ Bait Planner โ”€โ”€
            BaitPlanner.RegisterComboStarters(CardId.BlueEyesToonDragon, CardId.ToonTableOfContents, CardId.ToonBookmark, CardId.ToonWorldPerfect, CardId.CalledByTheGrave);
            BaitPlanner.RegisterBaitCards(CardId.Terraforming, CardId.ToonTableOfContents);

            // โ”€โ”€ Chain Advisor โ”€โ”€
            ChainAdvisor.RegisterHighValueTargets(CardId.BlueEyesToonDragon, CardId.ToonWorldPerfect, CardId.CalledByTheGrave, CardId.AshBlossom, CardId.FunnyDarkRabbit, CardId.ToonDarkMagician);


            // โ•โ•โ• TIER 1: Hand Traps โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, () => SmartHandTrapChain() && DefaultMaxxC());
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyPurulia, MulcharmyEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshEffect);
            AddExecutor(ExecutorType.Activate, CardId.GhostOgre, GhostOgreEffect);
            AddExecutor(ExecutorType.Activate, CardId.PSYFramegearGamma, PSYGammaEffect);
            AddExecutor(ExecutorType.Activate, CardId.PSYFramegearDelta, PSYDeltaEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);

            // โ•โ•โ• TIER 2: Quick-Effect Disruption โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.MindScan, MindScanEffect);
            AddExecutor(ExecutorType.Activate, CardId.ToonTerror, ToonTerrorEffect);
            AddExecutor(ExecutorType.Activate, CardId.ComicCat, ComicCatEffect);
            AddExecutor(ExecutorType.Activate, CardId.EvilBox, EvilBoxGYEffect);

            // โ•โ•โ• TIER 3: Search Chain (Thinning) โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.ToonTableOfContents, ToonTableEffect);
            AddExecutor(ExecutorType.Activate, CardId.ToonBookmark, ToonBookmarkEffect);
            AddExecutor(ExecutorType.Activate, CardId.Terraforming, TerraformingEffect);

            // โ•โ•โ• TIER 4: Field Spell โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.ToonWorldPerfect, ToonWorldPerfectEffect);

            // โ•โ•โ• TIER 5: Monster Effects (Setup) โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.FunnyDarkRabbit, FunnyDarkRabbitEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarkEyesIllusionist, DarkEyesEffect);
            AddExecutor(ExecutorType.Activate, CardId.ToonDarkMagician, ToonDarkMagicianEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentEffect);

            // โ•โ•โ• TIER 6: Special Summons โ•โ•โ•
            AddExecutor(ExecutorType.SpSummon, CardId.BlueEyesToonUltimateDragon, BlueEyesToonUltDragonSS);
            AddExecutor(ExecutorType.SpSummon, CardId.EvilBox, EvilBoxSSEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.ToonMermaid, ToonMermaidSS);

            // โ•โ•โ• TIER 7: Normal Summons โ•โ•โ•
            AddExecutor(ExecutorType.Summon, CardId.FunnyDarkRabbit, FunnyDarkRabbitSummon);
            AddExecutor(ExecutorType.Summon, CardId.ComicCat, ComicCatSummon);
            AddExecutor(ExecutorType.Summon, CardId.EvilBox, EvilBoxSummon);
            AddExecutor(ExecutorType.Summon, CardId.ToonMermaid, ToonMermaidSummon);

            // โ•โ•โ• TIER 7.5: Extra Deck Summons (GATED โ€” only when needed) โ•โ•โ•
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.BaronneDeFleur, BaronneSummon);
            AddExecutor(ExecutorType.Activate, CardId.BaronneDeFleur, BaronneEffect);

            // โ•โ•โ• TIER 8: Set Backrow โ•โ•โ•
            AddExecutor(ExecutorType.SpellSet, CardId.ToonTerror, SetBackrowCondition);
            AddExecutor(ExecutorType.SpellSet, CardId.DominusImpulse, SetBackrowCondition);
            AddExecutor(ExecutorType.Activate, CardId.DominusImpulse, DominusImpulseEffect);

            // โ•โ•โ• TIER 9: Repos โ•โ•โ•
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        // SECTION: Turn Management & State
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        public override bool OnSelectHand() => true; // Go first to setup Toon World

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _funnyDarkRabbitSearchUsed = false;
            _comicCatUsed = false;
            _evilBoxSSUsed = false;
            _evilBoxGYUsed = false;
            _darkEyesUsed = false;
            _toonBookmarkSearchUsed = false;
            _mindScanNegateUsed = false;
            _toonTerrorUsed = false;
            _perfectWorldSearchCount = 0;
            _handTrapsUsedThisTurn = 0;
            _toonDarkMagicianUsed = false;

            // โ”€โ”€ Going-Second BreakBoard: prioritize disruption over combo โ”€โ”€
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
            }
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        // SECTION: Strategic Assessment (OTK, Board Strength, Recovery)
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        /// <summary>
        /// Calculate total direct attack damage available from Toon monsters.
        /// Toon monsters with Toon World can attack directly (bypass enemy monsters).
        /// </summary>
        private int GetTotalDirectAttackDamage()
        {
            if (!HasToonWorldOnField()) return 0;
            bool enemyHasToon = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && IsToonMonster(c.Id));
            if (enemyHasToon) return 0; // Can't direct attack if enemy controls Toon

            return Bot.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && IsToonMonster(c.Id) && c.IsAttack())
                .Sum(c => c.Attack);
        }

        /// <summary>
        /// Check if we can OTK with current board state (direct attacks).
        /// Used to prevent wasteful plays like unnecessary Extra Deck summons.
        /// </summary>
        private bool CanOTKWithDirectAttack()
        {
            return GetTotalDirectAttackDamage() >= Enemy.LifePoints;
        }

        /// <summary>
        /// Check if we are in recovery mode (lost Toon World, low resources).
        /// When in recovery, AI should focus on rebuilding rather than extending.
        /// </summary>
        private bool IsInRecoveryMode()
        {
            if (HasToonWorldOnField()) return false;

            // No Toon World and can't get one = recovery mode
            bool canGetToonWorld = Bot.HasInHand(CardId.ToonWorldPerfect) ||
                                    GetRemainingCount(CardId.ToonWorldPerfect) > 0 ||
                                    Bot.HasInHand(CardId.Terraforming) ||
                                    Bot.HasInHand(CardId.ToonBookmark) ||
                                    Bot.HasInHand(CardId.ToonTableOfContents);
            if (!canGetToonWorld) return true;

            // Toon World not on field but we can still get it โ€” partial recovery
            return false;
        }

        /// <summary>
        /// Count how many non-ace monsters we have on field that can be used as material.
        /// Prevents the AI from cannibalizing its own boss monsters.
        /// </summary>
        private int CountExpendableFieldMonsters()
        {
            return Bot.GetMonsters()
                .Count(c => c != null && c.IsFaceup() && !IsAceCard(c) && !ExtraDeckAces.Contains(c.Id));
        }

        protected override bool IsBoardStrongEnough()
        {
            // [FIX 5] If we can OTK with direct attacks, board is absolutely strong enough
            if (CanOTKWithDirectAttack())
                return true;

            // If BETUD is on field, that's our win condition โ€” don't overextend
            if (Bot.HasInMonstersZone(CardId.BlueEyesToonUltimateDragon))
                return true;

            bool hasToonWorld = HasToonWorldOnField();
            int toonMonCount = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && IsToonMonster(c.Id));
            bool hasMindScan = Bot.HasInSpellZone(CardId.MindScan);
            bool hasToonTerror = Bot.GetSpells().Any(c => c != null && c.IsFacedown() &&
                c.IsCode(CardId.ToonTerror));

            // Standard end board: Toon World + 2 Toon mons + disruption
            if (hasToonWorld && toonMonCount >= 2 && (hasMindScan || hasToonTerror))
                return true;

            // [FIX 5] If Toon World + high direct ATK (>= 50% enemy LP), strong enough going 2nd
            if (hasToonWorld && _isGoingSecond)
            {
                int directDmg = GetTotalDirectAttackDamage();
                if (directDmg >= Enemy.LifePoints * 0.6)
                    return true;
            }

            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (IsBoardStrongEnough())
                return base.ShouldStopExtending();
            return false;
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Contains(card.Id);
        }

        /// <summary>
        /// [FIX 3] Material priority for material selection.
        /// LOWER value = MORE likely to be chosen as material.
        /// Ace cards get very high values so they are NEVER chosen unless forced.
        /// </summary>
        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;

            // Enemy monsters โ€” always prefer to use as material (tribute/remove)
            if (c.Location == CardLocation.MonsterZone && c.Owner == 1) return 1;

            // === NEVER use as material (absolute protection) ===
            // Blue-Eyes Toon Ultimate Dragon โ€” the boss, NEVER sacrifice
            if (c.IsCode(CardId.BlueEyesToonUltimateDragon)) return 999;
            // Blue-Eyes Toon Dragon โ€” primary beater, NEVER sacrifice for non-BETUD plays
            if (c.IsCode(CardId.BlueEyesToonDragon)) return 950;
            // Toon Dark Magician โ€” recurring engine, protect
            if (c.IsCode(CardId.ToonDarkMagician)) return 900;
            // Baronne de Fleur / S:P โ€” Extra Deck bosses, protect
            if (c.IsCode(CardId.BaronneDeFleur)) return 980;
            if (c.IsCode(CardId.SPLittleKnight)) return 970;

            // Hand traps โ€” never use as on-field material
            if (HandTraps.Contains(c.Id)) return 850;

            // === Expendable material (prefer to use) ===
            // Toon Mermaid โ€” vanilla body, expendable
            if (c.IsCode(CardId.ToonMermaid)) return 30;
            // Evil Box โ€” expendable after using SS + search effect
            if (c.IsCode(CardId.EvilBox)) return 40;
            // Comic Cat โ€” expendable after using tribute effect
            if (c.IsCode(CardId.ComicCat)) return 50;
            // Funny Dark Rabbit โ€” somewhat expendable after NS + search
            if (c.IsCode(CardId.FunnyDarkRabbit)) return 60;

            // PSY-Frame Driver โ€” completely useless, best material
            if (c.IsCode(CardId.PSYFrameDriver)) return 5;

            // Default
            return 200;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        // SECTION: Utility Methods
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        private bool HasToonWorldOnField()
        {
            // Toon World the Perfect World becomes "Toon World" in the field zone
            return Bot.HasInSpellZone(CardId.ToonWorldPerfect, true);
        }

        private bool IsToonMonster(int id)
        {
            return ToonMonsters.Contains(id) || id == CardId.BlueEyesToonUltimateDragon;
        }

        private bool HasToonMonsterOnField()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsToonMonster(c.Id));
        }

        /// <summary>
        /// Check if a specific monster's effect has already been used this turn,
        /// making it safe to use as material.
        /// </summary>
        private bool IsMonsterEffectSpent(ClientCard card)
        {
            if (card == null) return true;
            if (card.IsCode(CardId.FunnyDarkRabbit) && _funnyDarkRabbitSearchUsed) return true;
            if (card.IsCode(CardId.ComicCat) && _comicCatUsed) return true;
            if (card.IsCode(CardId.EvilBox) && _evilBoxGYUsed) return true;
            if (card.IsCode(CardId.ToonDarkMagician) && _toonDarkMagicianUsed) return true;
            if (card.IsCode(CardId.ToonMermaid)) return true; // No OPT effect, always expendable
            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        // SECTION: Hand Trap Effects
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        private bool MulcharmyEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }

        private bool DrollEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1 && Duel.LastChainPlayer == 1;
        }

        private bool AshEffect()
        {
            if (!SmartHandTrapChain()) return false;
            var last = Util.GetLastChainCard();
            if (last == null || last.Controller != 1) return false;
            if (_handTrapsUsedThisTurn >= 2) return false;
            _handTrapsUsedThisTurn++;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool GhostOgreEffect()
        {
            if (!SmartHandTrapChain()) return false;
            var last = Util.GetLastChainCard();
            if (last == null || last.Controller != 1) return false;
            if (_handTrapsUsedThisTurn >= 2) return false;
            // Ghost Ogre only works on Continuous/Field/Equip/Monsters
            if (last.IsSpell() && !last.HasType(CardType.Continuous)
                && !last.HasType(CardType.Field) && !last.HasType(CardType.Equip))
            {
                if (!last.IsMonster()) return false;
            }
            _handTrapsUsedThisTurn++;
            return true;
        }

        private bool PSYGammaEffect()
        {
            if (!SmartHandTrapChain()) return false;
            var last = Util.GetLastChainCard();
            if (last == null || last.Controller != 1) return false;
            // Gamma requires no monsters on our field
            if (Bot.GetMonsterCount() > 0) return false;
            if (_handTrapsUsedThisTurn >= 2) return false;
            _handTrapsUsedThisTurn++;
            return true;
        }

        private bool PSYDeltaEffect()
        {
            if (!SmartHandTrapChain()) return false;
            var last = Util.GetLastChainCard();
            if (last == null || last.Controller != 1) return false;
            if (Bot.GetMonsterCount() > 0) return false;
            if (_handTrapsUsedThisTurn >= 2) return false;
            _handTrapsUsedThisTurn++;
            return true;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        // SECTION: Search Chain (Thinning Engine)
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        /// <summary>
        /// Toon Table of Contents โ€” [FIX 8] Context-aware search priorities.
        /// Going 1st: prioritize setup (Bookmark โ’ Perfect World โ’ FDR)
        /// Going 2nd: prioritize beaters (Bookmark โ’ PW โ’ BETD if have PW)
        /// Recovery: prioritize getting Toon World back
        /// </summary>
        private bool ToonTableEffect()
        {
            if (ShouldSkipCombo()) return false;
            // [FIX 4] Don't waste searches if we can already OTK
            if (CanOTKWithDirectAttack()) return false;

            // Recovery mode: prioritize getting Toon World
            if (IsInRecoveryMode())
            {
                if (GetRemainingCount(CardId.ToonWorldPerfect) > 0 &&
                    !Bot.HasInHand(CardId.ToonWorldPerfect))
                {
                    AI.SelectCard(CardId.ToonWorldPerfect);
                }
                else if (GetRemainingCount(CardId.ToonBookmark) > 0 &&
                         !Bot.HasInHand(CardId.ToonBookmark))
                {
                    AI.SelectCard(CardId.ToonBookmark);
                }
                else
                {
                    AI.SelectCard(CardId.ToonBookmark, CardId.ToonWorldPerfect,
                        CardId.FunnyDarkRabbit, CardId.ComicCat);
                }
                DecisionTracer.TraceActivate("ToonTableEffect", "recovery: searching Toon World path");
                return true;
            }

            // Normal search priority: thin deck into key pieces
            if (!Bot.HasInHand(CardId.ToonBookmark) &&
                GetRemainingCount(CardId.ToonBookmark) > 0)
            {
                AI.SelectCard(CardId.ToonBookmark);
            }
            else if (!Bot.HasInHand(CardId.ToonWorldPerfect) && !HasToonWorldOnField() &&
                     GetRemainingCount(CardId.ToonWorldPerfect) > 0)
            {
                AI.SelectCard(CardId.ToonWorldPerfect);
            }
            // [FIX 8] Going 2nd: search BETD early for OTK
            else if (_isGoingSecond && !Bot.HasInHand(CardId.BlueEyesToonDragon) &&
                     GetRemainingCount(CardId.BlueEyesToonDragon) > 0 && HasToonWorldOnField())
            {
                AI.SelectCard(CardId.BlueEyesToonDragon);
            }
            else if (!Bot.HasInHand(CardId.FunnyDarkRabbit) &&
                     GetRemainingCount(CardId.FunnyDarkRabbit) > 0)
            {
                AI.SelectCard(CardId.FunnyDarkRabbit);
            }
            else if (GetRemainingCount(CardId.ToonTableOfContents) > 0)
            {
                // Chain into another Toon Table to thin further
                AI.SelectCard(CardId.ToonTableOfContents);
            }
            else
            {
                AI.SelectCard(CardId.ComicCat, CardId.EvilBox, CardId.ToonMermaid,
                    CardId.BlueEyesToonDragon, CardId.ToonDarkMagician);
            }

            DecisionTracer.TraceActivate("ToonTableEffect", "thinning deck");
            return true;
        }

        /// <summary>
        /// Toon Bookmark โ€” [FIX 8] Context-aware search + GY protection.
        /// </summary>
        private bool ToonBookmarkEffect()
        {
            if (_toonBookmarkSearchUsed && Card.Location == CardLocation.Hand) return false;
            if (ShouldSkipCombo() && Card.Location == CardLocation.Hand) return false;
            // [FIX 4] Don't waste searches if we can already OTK
            if (CanOTKWithDirectAttack() && Card.Location == CardLocation.Hand) return false;

            // From hand: search Toon World the Perfect World or another key Toon card
            if (Card.Location == CardLocation.Hand)
            {
                if (!HasToonWorldOnField() && !Bot.HasInHand(CardId.ToonWorldPerfect) &&
                    GetRemainingCount(CardId.ToonWorldPerfect) > 0)
                {
                    AI.SelectCard(CardId.ToonWorldPerfect);
                }
                // [FIX 8] Going 2nd: prioritize BETD if Toon World is up
                else if (_isGoingSecond && HasToonWorldOnField() &&
                         !Bot.HasInHand(CardId.BlueEyesToonDragon) &&
                         !Bot.HasInMonstersZone(CardId.BlueEyesToonDragon) &&
                         GetRemainingCount(CardId.BlueEyesToonDragon) > 0)
                {
                    AI.SelectCard(CardId.BlueEyesToonDragon);
                }
                else if (!Bot.HasInHand(CardId.FunnyDarkRabbit) &&
                         GetRemainingCount(CardId.FunnyDarkRabbit) > 0)
                {
                    AI.SelectCard(CardId.FunnyDarkRabbit);
                }
                // [FIX 8] Going 1st: prioritize disruption tools
                else if (!_isGoingSecond && !Bot.HasInHand(CardId.EvilBox) &&
                         GetRemainingCount(CardId.EvilBox) > 0)
                {
                    AI.SelectCard(CardId.EvilBox); // Will search Toon Terror
                }
                else
                {
                    AI.SelectCard(CardId.ComicCat, CardId.EvilBox, CardId.ToonDarkMagician,
                        CardId.BlueEyesToonDragon);
                }
                _toonBookmarkSearchUsed = true;
                DecisionTracer.TraceActivate("ToonBookmarkEffect", "searching Toon card");
                return true;
            }

            // From GY: protect Toon World from destruction (banish self to save)
            if (Card.Location == CardLocation.Grave)
            {
                // Only activate GY effect to protect Toon World when it's threatened
                return true;
            }

            return false;
        }

        /// <summary>
        /// Terraforming โ€” search Perfect World if we don't have field spell.
        /// </summary>
        private bool TerraformingEffect()
        {
            if (HasToonWorldOnField()) return false;
            if (Bot.HasInHand(CardId.ToonWorldPerfect)) return false;
            if (GetRemainingCount(CardId.ToonWorldPerfect) == 0) return false;
            AI.SelectCard(CardId.ToonWorldPerfect);
            return true;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        // SECTION: Toon World the Perfect World (Field Spell)
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        /// <summary>
        /// [FIX 8] Context-aware search priorities based on game state.
        /// </summary>
        private bool ToonWorldPerfectEffect()
        {
            // Activation: place on field
            if (Card.Location == CardLocation.Hand)
            {
                if (HasToonWorldOnField()) return false;
                DecisionTracer.TraceActivate("ToonWorldPerfect", "activating field spell");
                return true;
            }

            // Search effect (up to 3 per turn)
            if (Card.Location == CardLocation.SpellZone && _perfectWorldSearchCount < 3)
            {
                if (ShouldSkipCombo() && _perfectWorldSearchCount >= 1) return false;
                // [FIX 4] Don't burn searches if we can already OTK
                if (CanOTKWithDirectAttack() && _perfectWorldSearchCount >= 1) return false;

                // [FIX 8] Adaptive search priority based on going 1st/2nd + game state
                if (_isGoingSecond)
                {
                    // Going 2nd: OTK focus โ€” beaters first, then combo
                    SelectPerfectWorldSearchGoing2nd();
                }
                else
                {
                    // Going 1st: setup focus โ€” combo + disruption
                    SelectPerfectWorldSearchGoing1st();
                }

                _perfectWorldSearchCount++;
                DecisionTracer.TraceActivate("ToonWorldPerfect",
                    $"search #{_perfectWorldSearchCount}");
                return true;
            }

            return false;
        }

        private void SelectPerfectWorldSearchGoing1st()
        {
            // Going 1st: Funny Dark Rabbit โ’ Comic Cat โ’ Evil Box โ’ Toon Dark Magician โ’ disruption
            if (!Bot.HasInHand(CardId.FunnyDarkRabbit) && !Bot.HasInMonstersZone(CardId.FunnyDarkRabbit) &&
                GetRemainingCount(CardId.FunnyDarkRabbit) > 0)
            {
                AI.SelectCard(CardId.FunnyDarkRabbit);
            }
            else if (!Bot.HasInHand(CardId.ComicCat) && !Bot.HasInMonstersZone(CardId.ComicCat) &&
                     GetRemainingCount(CardId.ComicCat) > 0)
            {
                AI.SelectCard(CardId.ComicCat);
            }
            else if (!Bot.HasInHand(CardId.EvilBox) &&
                     GetRemainingCount(CardId.EvilBox) > 0)
            {
                AI.SelectCard(CardId.EvilBox);
            }
            else if (!Bot.HasInHand(CardId.ToonDarkMagician) &&
                     GetRemainingCount(CardId.ToonDarkMagician) > 0)
            {
                AI.SelectCard(CardId.ToonDarkMagician);
            }
            else if (!Bot.HasInHand(CardId.ToonBookmark) &&
                     GetRemainingCount(CardId.ToonBookmark) > 0)
            {
                AI.SelectCard(CardId.ToonBookmark);
            }
            else
            {
                AI.SelectCard(CardId.ToonMermaid, CardId.MindScan, CardId.ToonTerror);
            }
        }

        private void SelectPerfectWorldSearchGoing2nd()
        {
            // Going 2nd: Funny Dark Rabbit (combo) โ’ Blue-Eyes Toon Dragon (OTK) โ’ Comic Cat โ’ rest
            if (!Bot.HasInHand(CardId.FunnyDarkRabbit) && !Bot.HasInMonstersZone(CardId.FunnyDarkRabbit) &&
                GetRemainingCount(CardId.FunnyDarkRabbit) > 0)
            {
                AI.SelectCard(CardId.FunnyDarkRabbit);
            }
            else if (!Bot.HasInHand(CardId.BlueEyesToonDragon) &&
                     !Bot.HasInMonstersZone(CardId.BlueEyesToonDragon) &&
                     GetRemainingCount(CardId.BlueEyesToonDragon) > 0)
            {
                AI.SelectCard(CardId.BlueEyesToonDragon);
            }
            else if (!Bot.HasInHand(CardId.ComicCat) && !Bot.HasInMonstersZone(CardId.ComicCat) &&
                     GetRemainingCount(CardId.ComicCat) > 0)
            {
                AI.SelectCard(CardId.ComicCat);
            }
            else if (!Bot.HasInHand(CardId.ToonDarkMagician) &&
                     GetRemainingCount(CardId.ToonDarkMagician) > 0)
            {
                AI.SelectCard(CardId.ToonDarkMagician);
            }
            else if (!Bot.HasInHand(CardId.EvilBox) &&
                     GetRemainingCount(CardId.EvilBox) > 0)
            {
                AI.SelectCard(CardId.EvilBox);
            }
            else
            {
                AI.SelectCard(CardId.ToonMermaid, CardId.ToonBookmark, CardId.MindScan);
            }
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        // SECTION: Funny Dark Rabbit
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        private bool FunnyDarkRabbitSummon()
        {
            if (ShouldSkipCombo()) return false;
            // [FIX 4] Don't NS if we can OTK already
            if (CanOTKWithDirectAttack()) return false;
            // Priority NS: Funny Dark Rabbit gives extra NS + search
            DecisionTracer.TraceActivate("FunnyDarkRabbitSummon", "NS for extra NS + search");
            return true;
        }

        private bool FunnyDarkRabbitEffect()
        {
            // Search effect: Field or Continuous Spell (Toon)
            if (_funnyDarkRabbitSearchUsed) return false;

            if (!HasToonWorldOnField() && !Bot.HasInHand(CardId.ToonWorldPerfect) &&
                GetRemainingCount(CardId.ToonWorldPerfect) > 0)
            {
                AI.SelectCard(CardId.ToonWorldPerfect);
            }
            else if (!Bot.HasInSpellZone(CardId.MindScan) && !Bot.HasInHand(CardId.MindScan) &&
                     GetRemainingCount(CardId.MindScan) > 0)
            {
                AI.SelectCard(CardId.MindScan);
            }
            else
            {
                // Default: grab Perfect World if available
                AI.SelectCard(CardId.ToonWorldPerfect, CardId.MindScan);
            }

            _funnyDarkRabbitSearchUsed = true;
            DecisionTracer.TraceActivate("FunnyDarkRabbitEffect", "searching Field/Continuous spell");
            return true;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        // SECTION: Comic Cat
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        private bool ComicCatSummon()
        {
            if (ShouldSkipCombo()) return false;
            if (Bot.HasInMonstersZone(CardId.ComicCat)) return false;
            // [FIX 4] Don't NS if we can OTK already
            if (CanOTKWithDirectAttack()) return false;
            return true;
        }

        private bool ComicCatEffect()
        {
            if (_comicCatUsed) return false;
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;

            // Need a Toon monster to SS from hand/deck
            bool hasTargetInDeck = GetRemainingCount(CardId.ToonDarkMagician) > 0 ||
                                   GetRemainingCount(CardId.BlueEyesToonDragon) > 0 ||
                                   GetRemainingCount(CardId.ToonMermaid) > 0;
            bool hasTargetInHand = Bot.Hand.Any(c => c != null && c.IsMonster() && IsToonMonster(c.Id));

            if (!hasTargetInDeck && !hasTargetInHand) return false;

            // Tribute priority: opponent's monster if Toon World is up
            if (HasToonWorldOnField())
            {
                var oppTarget = Enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup())
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();
                if (oppTarget != null)
                {
                    AI.SelectCard(oppTarget);
                }
                else
                {
                    // Tribute our own expendable monster (NEVER tribute aces!)
                    var ownTarget = Bot.GetMonsters()
                        .Where(c => c != null && c.IsFaceup() && !IsAceCard(c) &&
                               c.Id != CardId.ComicCat && !ExtraDeckAces.Contains(c.Id))
                        .OrderBy(c => c.Attack)
                        .FirstOrDefault();
                    if (ownTarget == null) return false;
                    AI.SelectCard(ownTarget);
                }
            }
            else
            {
                // No Toon World: can only tribute own monsters
                var ownTarget = Bot.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && !IsAceCard(c) &&
                           c.Id != CardId.ComicCat && !ExtraDeckAces.Contains(c.Id))
                    .OrderBy(c => c.Attack)
                    .FirstOrDefault();
                if (ownTarget == null) return false;
                AI.SelectCard(ownTarget);
            }

            // SS target priority
            if (GetRemainingCount(CardId.BlueEyesToonDragon) > 0 && _isGoingSecond)
            {
                AI.SelectCard(CardId.BlueEyesToonDragon);
            }
            else if (GetRemainingCount(CardId.ToonDarkMagician) > 0)
            {
                AI.SelectCard(CardId.ToonDarkMagician);
            }
            else
            {
                AI.SelectCard(CardId.BlueEyesToonDragon, CardId.ToonMermaid, CardId.EvilBox);
            }

            _comicCatUsed = true;
            DecisionTracer.TraceActivate("ComicCatEffect", "tribute โ’ SS Toon from deck");
            return true;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        // SECTION: Evil Box
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        private bool EvilBoxSSEffect()
        {
            if (_evilBoxSSUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (!HasToonWorldOnField()) return false;
            if (Card.Location != CardLocation.Hand) return false;
            // [FIX 4] Don't SS if we can OTK already (save resources)
            if (CanOTKWithDirectAttack()) return false;

            _evilBoxSSUsed = true;
            // Will automatically search Toon Trap
            if (GetRemainingCount(CardId.ToonTerror) > 0)
                AI.SelectCard(CardId.ToonTerror);

            DecisionTracer.TraceActivate("EvilBoxSSEffect", "SS self + search Toon Trap");
            return true;
        }

        private bool EvilBoxSummon()
        {
            if (ShouldSkipCombo()) return false;
            // [FIX 4] Don't NS if we can OTK already
            if (CanOTKWithDirectAttack()) return false;
            return !HasToonWorldOnField(); // Only NS Evil Box if no Toon World (can't SS)
        }

        private bool EvilBoxGYEffect()
        {
            // Quick Effect from field: target 1 card in GY โ’ bottom of deck
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_evilBoxGYUsed) return false;
            if (Card.IsDisabled()) return false;

            // Use to recycle or deny opponent's GY plays
            var oppGYTarget = Enemy.Graveyard
                .Where(c => c != null && c.IsMonster())
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();

            if (oppGYTarget != null)
            {
                AI.SelectCard(oppGYTarget);
                _evilBoxGYUsed = true;
                DecisionTracer.TraceActivate("EvilBoxGYEffect", $"bottom-decking opponent's {oppGYTarget.Name}");
                return true;
            }
            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        // SECTION: Dark-Eyes Illusionist Faceless Mage
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        private bool DarkEyesEffect()
        {
            if (_darkEyesUsed) return false;
            if (Card.Location != CardLocation.Hand) return false;
            if (Bot.Hand.Count <= 1) return false; // Don't discard last card

            // Option 1: Place Mind Scan if we don't have it
            if (!Bot.HasInSpellZone(CardId.MindScan) &&
                (Bot.HasInHand(CardId.MindScan) || GetRemainingCount(CardId.MindScan) > 0))
            {
                AI.SelectOption(0); // Place Mind Scan
                _darkEyesUsed = true;
                DecisionTracer.TraceActivate("DarkEyesEffect", "placing Mind Scan");
                return true;
            }

            // Option 2: Revive Toon monster from GY
            var reviveTarget = Bot.Graveyard
                .Where(c => c != null && c.IsMonster() && IsToonMonster(c.Id) && c.IsCanRevive())
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();
            if (reviveTarget != null && !IsSpecialSummonBlocked())
            {
                AI.SelectOption(1); // Revive
                AI.SelectCard(reviveTarget);
                _darkEyesUsed = true;
                DecisionTracer.TraceActivate("DarkEyesEffect", $"reviving {reviveTarget.Name}");
                return true;
            }

            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        // SECTION: Toon Dark Magician
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        private bool ToonDarkMagicianEffect()
        {
            if (_toonDarkMagicianUsed) return false;
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (!HasToonWorldOnField()) return false;

            // SS Toon from deck or Set Toon S/T from deck
            bool hasToonInDeck = GetRemainingCount(CardId.BlueEyesToonDragon) > 0 ||
                                 GetRemainingCount(CardId.ToonMermaid) > 0;

            if (hasToonInDeck && !IsSpecialSummonBlocked())
            {
                // [FIX 4] If we can OTK already, set disruption instead of SS more
                if (CanOTKWithDirectAttack())
                {
                    AI.SelectOption(1); // Set Toon S/T
                    AI.SelectCard(CardId.ToonTerror, CardId.ToonBookmark);
                }
                else
                {
                    AI.SelectOption(0); // SS Toon from deck
                    if (_isGoingSecond && GetRemainingCount(CardId.BlueEyesToonDragon) > 0)
                        AI.SelectCard(CardId.BlueEyesToonDragon);
                    else
                        AI.SelectCard(CardId.ToonMermaid, CardId.BlueEyesToonDragon, CardId.EvilBox);
                }
            }
            else
            {
                AI.SelectOption(1); // Set Toon S/T
                AI.SelectCard(CardId.ToonTerror, CardId.ToonBookmark);
            }

            _toonDarkMagicianUsed = true;
            DecisionTracer.TraceActivate("ToonDarkMagicianEffect", "SS/Set from deck");
            return true;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        // SECTION: Toon Mermaid
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        private bool ToonMermaidSS()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (!HasToonWorldOnField()) return false;
            if (Card.Location != CardLocation.Hand) return false;
            // [FIX 4] Don't SS if we can OTK already (save field space)
            if (CanOTKWithDirectAttack()) return false;
            return true;
        }

        private bool ToonMermaidSummon()
        {
            if (ShouldSkipCombo()) return false;
            // [FIX 4] Don't NS if we can OTK already
            if (CanOTKWithDirectAttack()) return false;
            return !HasToonWorldOnField(); // Only NS if can't SS
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        // SECTION: Mind Scan
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        private bool MindScanEffect()
        {
            // Activate from hand: place on field
            if (Card.Location == CardLocation.Hand)
            {
                bool hasToonAnywhere = HasToonMonsterOnField() ||
                    Bot.Graveyard.Any(c => c != null && IsToonMonster(c.Id));
                return hasToonAnywhere;
            }

            // Negate effect on field
            if (Card.Location == CardLocation.SpellZone && !_mindScanNegateUsed)
            {
                if (Duel.CurrentChain.Count > 0) return false;

                bool hasToonMonOnField = HasToonMonsterOnField();
                bool hasToonSpell = Bot.GetSpells().Any(c => c != null && c.IsFaceup() &&
                    (c.IsCode(CardId.ToonWorldPerfect) || c.IsCode(CardId.ToonBookmark) || c.IsCode(CardId.MindScan)));
                bool hasToonInGY = Bot.Graveyard.Any(c => c != null && IsToonMonster(c.Id));
                bool hasToonSpellInGY = Bot.Graveyard.Any(c => c != null &&
                    (c.IsCode(CardId.ToonWorldPerfect) || c.IsCode(CardId.ToonBookmark) || c.IsCode(CardId.MindScan)));

                if ((hasToonMonOnField || hasToonInGY) && (hasToonSpell || hasToonSpellInGY))
                {
                    int targetId = FindMindScanTarget();
                    _mindScanNegateUsed = true;
                    AI.SelectAnnounceID(targetId);
                    DecisionTracer.TraceActivate("MindScanEffect", $"proactively negating card ID {targetId}");
                    return true;
                }
            }
            return false;
        }

        private int FindMindScanTarget()
        {
            // Cards we want to proactively negate if they are in opponent's hand:
            int[] threatIds = {
                4031928,  // Change of Heart
                25311006, // Triple Tactics Talent
                24224830, // Called by the Grave
                95365081, // Hecahands Ibtel
                20415050, // The Hidden Hecahands
                58143852, // Nightmare Apprentice
                45951104, // Bot Herder
                32759190, // Hecahands Yadel
                21637502, // Hecahands Breus
                95132593, // Hecahands Gaigas
                57809669, // Hecahands Tartaros
                42141493, // Mulcharmy Fuwalos
                84192580, // Mulcharmy Purulia
                94145021  // Droll & Lock Bird
            };

            foreach (int id in threatIds)
            {
                if (Enemy.Hand.Any(c => c != null && c.Id == id))
                {
                    return id;
                }
            }

            // Any other known card in enemy hand
            var anyKnown = Enemy.Hand.FirstOrDefault(c => c != null && c.Id != 0);
            if (anyKnown != null) return anyKnown.Id;

            // Fallback: negate Ash Blossom
            return CardId.AshBlossom;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        // SECTION: Toon Terror
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        private bool ToonTerrorEffect()
        {
            if (_toonTerrorUsed) return false;
            var last = Util.GetLastChainCard();
            if (last == null || last.Controller != 1) return false;
            if (!HasToonMonsterOnField()) return false;

            _toonTerrorUsed = true;
            DecisionTracer.TraceActivate("ToonTerrorEffect", $"counter-trap negating {last.Name}");
            return true;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        // SECTION: Blue-Eyes Toon Ultimate Dragon (Contact Fusion)
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        /// <summary>
        /// [FIX 3] Contact Fusion with intelligent material selection.
        /// Only fuse when we have expendable monsters and it's worth it.
        /// </summary>
        private bool BlueEyesToonUltDragonSS()
        {
            if (IsSpecialSummonBlocked()) return false;

            // [FIX 4] If we can OTK without BETUD, don't contact fuse
            // (contact fuse removes our attackers from the field)
            if (CanOTKWithDirectAttack()) return false;

            // Requires BETD + 2 Toon monsters on field/hand/GY โ’ shuffle to deck
            bool hasBETD = Bot.HasInMonstersZone(CardId.BlueEyesToonDragon) ||
                           Bot.HasInHand(CardId.BlueEyesToonDragon) ||
                           Bot.Graveyard.Any(c => c != null && c.IsCode(CardId.BlueEyesToonDragon));

            if (!hasBETD) return false;

            // Count available Toon monsters (excluding BETD itself which is one of the materials)
            var availableToons = new List<ClientCard>();

            // Field monsters
            availableToons.AddRange(Bot.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && IsToonMonster(c.Id) &&
                       !c.IsCode(CardId.BlueEyesToonDragon)));

            // Hand monsters
            availableToons.AddRange(Bot.Hand
                .Where(c => c != null && IsToonMonster(c.Id) &&
                       !c.IsCode(CardId.BlueEyesToonDragon)));

            // GY monsters
            availableToons.AddRange(Bot.Graveyard
                .Where(c => c != null && IsToonMonster(c.Id) &&
                       !c.IsCode(CardId.BlueEyesToonDragon)));

            // Need at least 2 other Toon monsters as materials
            if (availableToons.Count < 2) return false;

            // [FIX 3] Prefer expendable materials โ€” sort by priority
            // Use effect-spent and low-value monsters first
            var sortedMaterials = availableToons
                .OrderBy(c => IsMonsterEffectSpent(c) ? 0 : 1) // Effect spent first
                .ThenBy(c => ExpendableToonMonsters.Contains(c.Id) ? 0 : 1) // Expendable first
                .ThenBy(c => c.Attack) // Lower ATK first
                .ToList();

            // Select BETD + 2 best expendable materials
            // (The engine will handle the actual selection, but we signal preference)
            var bestMaterials = sortedMaterials.Take(2).ToList();

            // [FIX 3] SAFETY: Don't fuse if we'd have to use Toon Dark Magician as material
            // and its effect hasn't been used yet (it's our recurring engine)
            bool wouldUseTDM = bestMaterials.Any(c => c.IsCode(CardId.ToonDarkMagician) && !_toonDarkMagicianUsed);
            if (wouldUseTDM && bestMaterials.Count == 2)
            {
                // Check if there's no alternative โ€” if TDM is the only option, skip fusion
                var nonTDM = sortedMaterials.Where(c => !c.IsCode(CardId.ToonDarkMagician)).ToList();
                if (nonTDM.Count < 2) return false;
            }

            DecisionTracer.TraceActivate("BlueEyesToonUltDragonSS",
                $"contact fusion โ€” materials: {string.Join(", ", bestMaterials.Select(c => c.Name ?? c.Id.ToString()))}");
            return true;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        // SECTION: Extra Deck Monster Summons (GATED)
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        /// <summary>
        /// [FIX 1] S:P Little Knight โ€” only SS when there's a real threat to banish
        /// AND we have expendable material. NEVER eat ace cards.
        /// </summary>
        private bool SPLittleKnightSummon()
        {
            // MUST have a problematic enemy card to justify summoning
            var target = Util.GetProblematicEnemyCard();
            if (target == null) return false;

            // [FIX 4] If we can OTK with direct attacks, don't waste monsters on S:P
            if (CanOTKWithDirectAttack()) return false;

            // [FIX 1] Need at least 2 expendable (non-ace) monsters as material
            int expendableCount = CountExpendableFieldMonsters();
            if (expendableCount < 2) return false;

            // [FIX 1] Additional check: don't SS S:P if we would lose more ATK than we gain
            int totalToonATK = GetTotalDirectAttackDamage();
            // If our Toon direct attack potential is > 4000, S:P is almost never worth it
            if (totalToonATK > 4000) return false;

            DecisionTracer.TraceActivate("SPLittleKnightSummon",
                $"summoning to banish {target.Name} โ€” {expendableCount} expendable monsters available");
            return true;
        }

        /// <summary>
        /// [FIX 2] Baronne de Fleur โ€” only SS when we need omni-negate
        /// AND we have expendable material AND no Toon Terror set.
        /// </summary>
        private bool BaronneSummon()
        {
            // [FIX 4] If we can OTK with direct attacks, don't waste monsters
            if (CanOTKWithDirectAttack()) return false;

            // Don't summon Baronne if we already have disruption (Toon Terror or Mind Scan)
            bool hasToonTerror = Bot.GetSpells().Any(c => c != null && c.IsFacedown() &&
                c.IsCode(CardId.ToonTerror));
            bool hasMindScan = Bot.HasInSpellZone(CardId.MindScan);
            if (hasToonTerror && hasMindScan) return false;

            // Need expendable material โ€” never use ace cards
            int expendableCount = CountExpendableFieldMonsters();
            if (expendableCount < 2) return false;

            // [FIX 2] Don't summon Baronne if BETUD is on field (we'd rather keep board)
            if (Bot.HasInMonstersZone(CardId.BlueEyesToonUltimateDragon)) return false;

            DecisionTracer.TraceActivate("BaronneSummon", "summoning for omni-negate disruption");
            return true;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        // SECTION: Triple Tactics Talent
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        private bool TripleTacticsTalentEffect()
        {
            if (Enemy.GetMonsterCount() > 0)
                AI.SelectOption(1); // Steal
            else
                AI.SelectOption(0); // Draw 2
            return true;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        // SECTION: S:P Little Knight & Baronne Effects
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        private bool SPLittleKnightEffect()
        {
            var target = Util.GetProblematicEnemyCard();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool BaronneEffect()
        {
            var last = Util.GetLastChainCard();
            if (last == null || last.Controller != 1) return false;
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            return true;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        // SECTION: Dominus Impulse
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        private bool DominusImpulseEffect()
        {
            // Only activate from field to avoid severe LIGHT/DARK lock drawback when activated from hand
            if (Card.Location == CardLocation.Hand) return false;

            var last = Util.GetLastChainCard();
            if (last == null || last.Controller != 1) return false;

            return true;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        // SECTION: Backrow, Repos, Battle
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        private bool SetBackrowCondition() => Util.IsTurn1OrMain2();

        private bool MonsterRepos()
        {
            if (Card == null) return false;

            // Toon monsters can attack directly when Toon World is up (and opponent has no Toon monster), so prefer ATK position
            if (HasToonWorldOnField() && IsToonMonster(Card.Id))
            {
                bool enemyHasToon = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && IsToonMonster(c.Id));
                if (!enemyHasToon)
                {
                    if (Card.IsDefense())
                        return true;
                    return false;
                }
            }

            if (IsAceCard(Card))
            {
                // Keep ace cards in their best position
                if (Card.IsDefense() && Card.Attack >= Card.Defense)
                    return true;
                return false;
            }

            bool enemyEmpty = Enemy.GetMonsterCount() == 0;
            if (Card.IsAttack())
            {
                if (!enemyEmpty)
                {
                    int maxEnemyAtk = Enemy.GetMonsters()
                        .Where(c => c != null && c.IsFaceup())
                        .Select(c => c.Attack)
                        .DefaultIfEmpty(0).Max();
                    // If enemy has a stronger monster and we cannot attack directly, flip to defense
                    if (Card.Attack < maxEnemyAtk && !IsToonMonster(Card.Id))
                        return true;
                }
            }
            else if (Card.IsDefense())
            {
                if (enemyEmpty || IsToonMonster(Card.Id))
                {
                    // If we can attack or enemy is empty, flip to attack
                    if (Card.Attack >= Card.Defense)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Battle Phase: Toon Direct Attack logic.
        /// </summary>
        public override bool OnPreBattleBetween(ClientCard attacker, ClientCard defender)
        {
            // Toon monsters can attack directly when Toon World is up
            // and opponent doesn't control a Toon monster
            if (HasToonWorldOnField() && IsToonMonster(attacker.Id))
            {
                bool enemyHasToon = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && IsToonMonster(c.Id));
                if (!enemyHasToon)
                {
                    // Toon monsters prefer direct attacks
                    attacker.RealPower = attacker.Attack;
                    return true;
                }
            }

            if (defender != null && defender.IsFaceup() && !defender.IsDisabled())
            {
                if (defender.IsAttack() && defender.Attack > attacker.Attack)
                    return false;
            }
            return base.OnPreBattleBetween(attacker, defender);
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        // SECTION: OnSelectYesNo / OnSelectEffectYn
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        public override bool OnSelectYesNo(long desc)
        {
            if (desc == ((long)CardId.ToonWorldPerfect << 16) + 1)
            {
                ClientCard last = Util.GetLastChainCard();
                if (last == null || last.Controller != 1)
                    return false;

                // 1. If it targets any of our Toon monsters or our Toon World, it's a threat!
                bool isTargeted = last.TargetCards.Any(c => c != null && c.Controller == 0 && 
                    ((c.Location == CardLocation.MonsterZone && IsToonMonster(c.Id)) ||
                     (c.Location == CardLocation.SpellZone && c.Id == CardId.ToonWorldPerfect)));
                if (isTargeted)
                {
                    DecisionTracer.TraceActivate("ToonWorldPerfectBanish", $"Dodging target effect from opponent's {last.Name}");
                    return true;
                }

                // 2. If it is a known non-targeting removal or threat card:
                int[] nonTargetThreats = {
                    26412047, // Raigeki
                    53129443, // Dark Hole
                    14532163, // Lightning Storm
                    18144506, // Harpie's Feather Duster
                    15693423, // Evenly Matched
                    27204311, // Nibiru
                    90448279, // Zeus
                    25311006, // Triple Tactics Talent
                    48130397, // Super Polymerization
                    86066372, // Accesscode Talker
                    44095762, // Mirror Force
                    53582587, // Torrential Tribute
                    35218707, // Book of Eclipse
                    33017964  // Illusion Gate
                };

                if (nonTargetThreats.Contains(last.Id))
                {
                    DecisionTracer.TraceActivate("ToonWorldPerfectBanish", $"Dodging non-targeting threat: {last.Name}");
                    return true;
                }

                DecisionTracer.TraceActivate("ToonWorldPerfectBanish", $"Declining banish for non-threatening opponent card: {last.Name}");
                return false;
            }

            return base.OnSelectYesNo(desc);
        }

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            if (desc == ((long)CardId.ToonWorldPerfect << 16) + 1)
            {
                return OnSelectYesNo(desc);
            }
            return base.OnSelectEffectYn(card, desc);
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        // SECTION: OnSelectCard โ€” [FIX 7] Material Protection
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        /// <summary>
        /// [FIX 7] Intelligent card selection with ace protection.
        /// For material hints (511/512/513/533): sort by GetMaterialPriority,
        /// ensuring ace cards are ALWAYS last to be selected.
        /// </summary>
        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Hint 506: Search โ’ pick missing combo pieces
            if (hint == 506)
            {
                var toonCards = cards.Where(c => c != null && (IsToonMonster(c.Id) ||
                    c.IsCode(CardId.ToonWorldPerfect, CardId.ToonBookmark, CardId.MindScan, CardId.ToonTerror)))
                    .ToList();
                if (toonCards.Count > 0)
                    return Util.CheckSelectCount(toonCards, cards, min, max);
            }

            // Hint 509: Special Summon โ’ filter by location
            if (hint == 509)
            {
                var deckCards = cards.Where(c => c != null && c.Location == CardLocation.Deck).ToList();
                if (deckCards.Count > 0 && cards.Any(c => c.Location != CardLocation.Deck))
                    return Util.CheckSelectCount(deckCards, cards, min, max);
            }

            // Hint 511/512/513/533: Material selection (Fusion/Link/Synchro/Xyz)
            // [FIX 7] Sort by GetMaterialPriority โ€” lowest = selected first (expendable)
            //         Ace cards have highest priority values โ’ selected LAST (protected)
            if (hint == 511 || hint == 512 || hint == 513 || hint == 533)
            {
                var sorted = cards.Where(c => c != null)
                    .OrderBy(c => GetMaterialPriority(c))
                    .ToList();

                // [FIX 7] SAFETY: If all available materials are ace cards, try to cancel
                if (cancelable)
                {
                    var nonAceMaterials = sorted.Where(c => !IsAceCard(c) && !ExtraDeckAces.Contains(c.Id)).ToList();
                    if (nonAceMaterials.Count < min)
                    {
                        // Would be forced to use ace cards โ€” cancel if possible
                        DecisionTracer.TraceActivate("OnSelectCard",
                            "CANCELLED: would be forced to use ace cards as material");
                        return null;
                    }
                }

                return Util.CheckSelectCount(sorted, cards, min, max);
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
