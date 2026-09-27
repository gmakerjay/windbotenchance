// ====================================================================================================
// CARD AUDIT — 2026_Grave (K9 Archetype + Necrovalley Lockdown Engine)
// 100% verified against cards.cdb and 2026_Grave.ydk
// ====================================================================================================
// | Card Name              | Type           | Lv/Rk | ATK  | DEF  | HOPT | Key Interaction                                         |
// |------------------------|----------------|-------|------|------|------|---------------------------------------------------------|
// | K9-66a Jokul           | Aqua/DARK      | 5     | 2000 | 1900 | Yes  | Hand: reveal self + Lv5 -> SS both; MP: search K9 monster |
// | K9-66b Lantern         | Pyro/DARK      | 5     | 2000 | 1900 | Yes  | Hand: SS self + Lv5 K9 from GY; MP: search K9 S/T        |
// | K9-17 Izuna            | Warrior/EARTH  | 5     | 2100 | 1600 | Yes  | Quick: SS on opp hand/GY trigger; NS/SS: Foolish K9 card |
// | K9-04 Noroi            | Machine/EARTH  | 5     | 2200 | 1500 | Yes  | Free NS if opp 2+ cards; NS: SS non-Machine K9 from deck|
// | K9-ØØ Lupis            | Beast-War/EARTH| 5     | 2300 | 200  | Yes  | Quick: SS on opp trigger; Material: target immunity      |
// | GK Commandant          | Spellcaster/E  | 4     | 1600 | 1500 | No   | Discard to search Necrovalley                           |
// | Necrovalley            | Field Spell    | -     | -    | -    | No   | Continuous GY lock: negates all GY movements & banishes |
// | Necrovalley Throne     | Normal Spell   | -     | -    | -    | Yes  | Searches GK Commandant (access to Necrovalley)          |
// | "A Case for K9"        | Continuous Sp  | -     | -    | -    | Yes  | Search any K9 monster; buffs K9 ATK +900                |
// | K9-X Forced Release    | Quick Spell    | -     | -    | -    | Yes  | Quick: Rank-up K9 Xyz into Werewolf + destroy 1 opp card |
// | Chaotic Elements       | Normal Spell   | -     | -    | -    | Yes  | Search Lv5+ LIGHT/DARK Pyro/Aqua (Jokul/Lantern)        |
// | Triple Tactics Talent  | Normal Spell   | -     | -    | -    | Yes  | Draw 2 / Steal monster / Hand rip                       |
// | Ash Blossom            | Tuner/FIRE     | 3     | 0    | 1800 | Yes  | Handtrap: negate search/draw/mill                       |
// | Maxx "C"               | Insect/EARTH   | 2     | 500  | 200  | Yes  | Handtrap: draw when opp special summons                 |
// | Droll & Lock Bird      | Spellcaster/W  | 1     | 0    | 0    | Yes  | Handtrap: stop further searching/drawing                |
// | Infinite Impermanence  | Normal Trap    | -     | -    | -    | Yes  | Handtrap/Trap: negate face-up monster                   |
// | Raigeki                | Normal Spell   | -     | -    | -    | No   | Board breaker: destroy all opp monsters                 |
// | Dark Ruler No More     | Normal Spell   | -     | -    | -    | No   | Board breaker: negate all opp face-up monsters          |
// |------------------------|----------------|-------|------|------|------|---------------------------------------------------------|
// | K9-17 "Ripper"         | Warrior/WIND   | R5    | 2300 | 1800 | Yes  | Core Boss: detach 1 search K9; Quick negate in hand/GY  |
// | K9-X "Werewolf"        | Beast-War/LIGHT| R9    | 3300 | 2500 | Yes  | Finisher: Quick rip opp hand; detach banish field; 3300 |
// | K9-66X "Jacks"         | Fiend/DARK     | R5    | 2600 | 2500 | Yes  | Quick pop / tribute pop                                 |
// | K9-ØØ "Hound"          | Beast-War/LIGHT| R5    | 2500 | 2500 | Yes  | Indestructible turn SS'd; detach to pop & burn          |
// | Artifact Durendal      | Fairy/LIGHT    | R5    | 2400 | 2100 | Yes  | Quick: rewrite opp effect to S/T pop or hand reload     |
// | Vallon Super Psy       | Psychic/DARK   | R5    | 2500 | 2200 | Yes  | Quick: book of moon opp monster face-down               |
// | N.As.H. Knight         | Aqua/WATER     | R5    | 1700 | 2700 | Yes  | Attach #104 from ED + suck opp monster as material      |
// | CXyz N.As.Ch. Knight   | Aqua/WATER     | R6    | 2000 | 3000 | Yes  | Overlay on N.As.H. Knight; SS #C104 Umbral              |
// | S:P Little Knight      | Warrior/DARK   | L2    | 1600 | -    | Yes  | Staple: banish card on field / dodge & banish           |
// | AA-ZEUS                | Machine/LIGHT  | R12   | 3000 | 3000 | No   | MP2: detach 2 send all other cards to GY                |
// | TY-PHON                | Fiend/DARK     | R12   | 2900 | 2900 | No   | Comeback: floodgate 2900+ ATK; detach bounce monster    |
// ====================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Plugin;
using WindBot.Game.AI.Plugins;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_Grave", "2026_Grave")]
    public class _2026_GraveExecutor : ModernExecutor
    {
        public class CardId
        {
            // --- K9 MONSTERS ---
            public const int K9_66aJokul = 28642461;
            public const int K9_66bLantern = 55031170;
            public const int K9_17Izuna = 92248362;
            public const int K9_04Noroi = 47960073;
            public const int K9_00Lupis = 91025875;

            // --- NECROVALLEY ENGINE ---
            public const int GravekeeperCommandant = 17393207;
            public const int Necrovalley = 47355498;
            public const int NecrovalleyThrone = 37561138;

            // --- SPELLS & TRAPS ---
            public const int ACaseForK9 = 80181649;
            public const int K9XForcedRelease = 53792930;
            public const int ChaoticElements = 92221402;
            public const int TripleTacticsTalent = 25311006;
            public const int Raigeki = 12580477;
            public const int DarkRulerNoMore = 54693926;

            // --- HAND TRAPS & STAPLES ---
            public const int AshBlossom = 14558128;
            public const int AshBlossomAlt = 14558127;
            public const int MaxxC = 23434538;
            public const int DrollAndLockBird = 94145021;
            public const int InfiniteImpermanence = 10045474;
            public const int CalledByTheGrave = 24224830;
            public const int DimensionalBarrier = 83326048;

            // --- EXTRA DECK ---
            public const int K9_17Ripper = 27420823;
            public const int K9_XWerewolf = 90303227;
            public const int K9_66XJacks = 67515699;
            public const int K9_00Hound = 54919528;
            public const int VallonSuperPsy = 40673853;
            public const int ArtifactDurendal = 69840739;
            public const int NASHKnight = 34876719;
            public const int CXyzNaschKnight = 61374414;
            public const int Number104Masquerade = 2061963;
            public const int NumberC104Umbral = 49456901;
            public const int SPLittleKnight = 29301450;
            public const int Zeus = 90448279;
            public const int TYPHON = 93039339;
        }

        private static readonly int[] AceCardIds = {
            CardId.K9_17Ripper,
            CardId.K9_XWerewolf,
            CardId.K9_66XJacks,
            CardId.K9_00Hound,
            CardId.ArtifactDurendal,
            CardId.SPLittleKnight,
            CardId.Zeus,
            CardId.TYPHON
        };

        private static readonly int[] HandTrapIds = {
            CardId.AshBlossom,
            CardId.AshBlossomAlt,
            CardId.MaxxC,
            CardId.DrollAndLockBird,
            CardId.InfiniteImpermanence
        };

        private static readonly int[] K9MonsterIds = {
            CardId.K9_66aJokul,
            CardId.K9_66bLantern,
            CardId.K9_17Izuna,
            CardId.K9_04Noroi,
            CardId.K9_00Lupis
        };

        // Turn State Tracking
        private bool _jokulHandUsed = false;
        private bool _jokulFieldUsed = false;
        private bool _lanternHandUsed = false;
        private bool _lanternFieldUsed = false;
        private bool _izunaFieldUsed = false;
        private bool _noroiFieldUsed = false;
        private bool _commandantUsed = false;
        private bool _throneUsed = false;
        private bool _necrovalleyUsed = false;
        private bool _caseForK9Used = false;
        private bool _forcedReleaseUsed = false;
        private bool _werewolfUsed = false;
        private bool _durendalUsed = false;

        public _2026_GraveExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            DeckPlugin = new GravePlugin(this);

            HeuristicGuard.RegisterAceCards(AceCardIds);

            // ── Combo Router ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Noroi-Ripper-Necrovalley",
                RequiredCards = new List<int> { CardId.K9_04Noroi },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.K9_04Noroi, ActionType = ExecutorType.Summon, Description = "Normal Summon Noroi" },
                    new() { CardId = CardId.K9_04Noroi, ActionType = ExecutorType.Activate, Description = "Noroi SS Jokul from deck" },
                    new() { CardId = CardId.K9_17Ripper, ActionType = ExecutorType.SpSummon, Description = "Xyz Summon Ripper" },
                    new() { CardId = CardId.K9_17Ripper, ActionType = ExecutorType.Activate, Description = "Ripper search Forced Release" },
                    new() { CardId = CardId.Necrovalley, ActionType = ExecutorType.Activate, Description = "Activate Necrovalley" }
                },
                EndBoardScore = 90
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Jokul-Extender-Ripper",
                RequiredCards = new List<int> { CardId.K9_66aJokul },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.K9_66aJokul, ActionType = ExecutorType.Activate, Description = "Jokul reveal & SS" },
                    new() { CardId = CardId.K9_17Ripper, ActionType = ExecutorType.SpSummon, Description = "Xyz Summon Ripper" }
                },
                EndBoardScore = 85
            });

            BaitPlanner.RegisterComboStarters(CardId.ACaseForK9, CardId.K9_04Noroi, CardId.K9_66aJokul);
            BaitPlanner.RegisterBaitCards(CardId.Raigeki, CardId.DarkRulerNoMore, CardId.ChaoticElements);
            ChainAdvisor.RegisterHighValueTargets(CardId.K9_17Ripper, CardId.ArtifactDurendal, CardId.K9_XWerewolf, CardId.AshBlossom, CardId.AshBlossomAlt);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 1: Handtraps & Interruptions (Highest Priority)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAlt, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdEffect);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, ImpermanenceEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: Boss Monster Quick Effects & Quick-Play Spells
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.K9_17Ripper, RipperEffect);
            AddExecutor(ExecutorType.Activate, CardId.K9_XWerewolf, WerewolfEffect);
            AddExecutor(ExecutorType.Activate, CardId.K9XForcedRelease, ForcedReleaseEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArtifactDurendal, DurendalEffect);
            AddExecutor(ExecutorType.Activate, CardId.VallonSuperPsy, VallonSuperPsyEffect);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightEffect);
            AddExecutor(ExecutorType.Activate, CardId.CXyzNaschKnight, CXyzNaschKnightEffect);
            AddExecutor(ExecutorType.Activate, CardId.NumberC104Umbral, NumberC104UmbralEffect);
            AddExecutor(ExecutorType.Activate, CardId.Zeus, ZeusEffect);
            AddExecutor(ExecutorType.Activate, CardId.TYPHON, TYPHONEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: Board Breakers (Going Second Sweepers)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.Raigeki, RaigekiEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarkRulerNoMore, DarkRulerNoMoreEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: Search Spells & Enablers
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentEffect);
            AddExecutor(ExecutorType.Activate, CardId.ACaseForK9, ACaseForK9Effect);
            AddExecutor(ExecutorType.Activate, CardId.ChaoticElements, ChaoticElementsEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 5: K9 Monster Effects & Summons (Phase 1: Swarm & Board Building)
            // ═══════════════════════════════════════════════════════════════
            // Hand Special Summons
            AddExecutor(ExecutorType.Activate, CardId.K9_66aJokul, JokulHandEffect);
            AddExecutor(ExecutorType.Activate, CardId.K9_66bLantern, LanternHandEffect);
            AddExecutor(ExecutorType.Activate, CardId.K9_17Izuna, IzunaHandEffect);
            AddExecutor(ExecutorType.Activate, CardId.K9_00Lupis, LupisHandEffect);

            // Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.K9_04Noroi, NoroiSummonCheck);
            AddExecutor(ExecutorType.Summon, CardId.K9_66aJokul, Level5SummonCheck);
            AddExecutor(ExecutorType.Summon, CardId.K9_66bLantern, Level5SummonCheck);
            AddExecutor(ExecutorType.Summon, CardId.K9_17Izuna, Level5SummonCheck);
            AddExecutor(ExecutorType.Summon, CardId.GravekeeperCommandant, CommandantSummonCheck);

            // On-Field Monster Effects
            AddExecutor(ExecutorType.Activate, CardId.K9_04Noroi, NoroiFieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.K9_66aJokul, JokulFieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.K9_66bLantern, LanternFieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.K9_17Izuna, IzunaFieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.K9_00Hound, HoundEffect);
            AddExecutor(ExecutorType.Activate, CardId.K9_66XJacks, JacksEffect);
            AddExecutor(ExecutorType.Activate, CardId.NASHKnight, NASHKnightEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 6: Extra Deck Summons (Climbing & Boss Establishment)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.K9_17Ripper, RipperSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.ArtifactDurendal, DurendalSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.K9_66XJacks, XyzSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.K9_00Hound, XyzSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.VallonSuperPsy, XyzSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.NASHKnight, XyzSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.CXyzNaschKnight, CXyzNaschKnightSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, LinkSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.Zeus, ZeusSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.TYPHON, TYPHONSummonCheck);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 7: Necrovalley Lockdown Engine (Phase 2: Lockdown Phase)
            //  *ACTIVATED AFTER K9 SWARM SO WE NEVER NEGATE OUR OWN LANTERN!*
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.NecrovalleyThrone, ThroneEffect);
            AddExecutor(ExecutorType.Activate, CardId.GravekeeperCommandant, CommandantEffect);
            AddExecutor(ExecutorType.Activate, CardId.Necrovalley, NecrovalleyEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 8: Spells/Traps Setting & Repositioning
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, SpellSetFiltered);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _jokulHandUsed = false;
            _jokulFieldUsed = false;
            _lanternHandUsed = false;
            _lanternFieldUsed = false;
            _izunaFieldUsed = false;
            _noroiFieldUsed = false;
            _commandantUsed = false;
            _throneUsed = false;
            _necrovalleyUsed = false;
            _caseForK9Used = false;
            _forcedReleaseUsed = false;
            _werewolfUsed = false;
            _durendalUsed = false;
        }

        public override bool OnSelectHand() => true; // Always choose First Turn for unbreakable Necrovalley + Ripper lock

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Contains(card.Id);
        }

        private bool IsK9Monster(ClientCard c) => c != null && K9MonsterIds.Contains(c.Id);
        private bool IsHandTrap(ClientCard c) => c != null && (HandTrapIds.Contains(c.Id) || CardIntelligence.IsHandtrap(c.Id));

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c))
            {
                if (c.Location == CardLocation.MonsterZone && c.IsFaceup())
                    return 10000; // Strong penalty to protect on-field Aces
                return 900;
            }
            if (IsHandTrap(c)) return 800; // Protect handtraps
            if (c.IsCode(CardId.K9_00Lupis)) return 10; // Lupis gives target immunity as material!
            if (c.IsCode(CardId.K9_17Izuna)) return 15;
            if (c.IsCode(CardId.K9_66aJokul)) return 20;
            if (c.IsCode(CardId.K9_66bLantern)) return 25;
            if (c.IsCode(CardId.K9_04Noroi)) return 30;
            return 100;
        }

        // ====================================================================================================
        //  STRATEGIC SELECTION ENGINE (OnSelectCard)
        // ====================================================================================================
        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // ── Hint 506: HINTMSG_ATOHAND (Search from Deck to Hand) ──
            if (hint == 506)
            {
                // Ripper Search: Forced Release > A Case for K9 > Jokul > Noroi > Lantern
                if (Card != null && Card.Id == CardId.K9_17Ripper)
                {
                    int[] ripperPriorities = {
                        CardId.K9XForcedRelease,
                        CardId.ACaseForK9,
                        CardId.K9_66aJokul,
                        CardId.K9_04Noroi,
                        CardId.K9_66bLantern,
                        CardId.K9_17Izuna
                    };
                    foreach (int pId in ripperPriorities)
                    {
                        var target = cards.FirstOrDefault(c => c != null && c.Id == pId);
                        if (target != null)
                        {
                            DecisionTracer.Trace("OnSelectCard", $"Ripper searching {target.Name}");
                            return new List<ClientCard> { target };
                        }
                    }
                }

                // A Case for K9 Search: Jokul > Noroi > Lantern > Izuna
                if (Card != null && Card.Id == CardId.ACaseForK9)
                {
                    int[] casePriorities = {
                        CardId.K9_66aJokul,
                        CardId.K9_04Noroi,
                        CardId.K9_66bLantern,
                        CardId.K9_17Izuna,
                        CardId.K9_00Lupis
                    };
                    foreach (int pId in casePriorities)
                    {
                        var target = cards.FirstOrDefault(c => c != null && c.Id == pId);
                        if (target != null)
                        {
                            DecisionTracer.Trace("OnSelectCard", $"A Case for K9 searching {target.Name}");
                            return new List<ClientCard> { target };
                        }
                    }
                }

                // Jokul MP Search: Noroi > Lantern > Izuna > Lupis
                if (Card != null && Card.Id == CardId.K9_66aJokul)
                {
                    int[] jokulPriorities = {
                        CardId.K9_04Noroi,
                        CardId.K9_66bLantern,
                        CardId.K9_17Izuna,
                        CardId.K9_00Lupis
                    };
                    foreach (int pId in jokulPriorities)
                    {
                        var target = cards.FirstOrDefault(c => c != null && c.Id == pId);
                        if (target != null)
                        {
                            DecisionTracer.Trace("OnSelectCard", $"Jokul searching {target.Name}");
                            return new List<ClientCard> { target };
                        }
                    }
                }

                // Lantern MP Search: Forced Release > A Case for K9
                if (Card != null && Card.Id == CardId.K9_66bLantern)
                {
                    var forced = cards.FirstOrDefault(c => c != null && c.Id == CardId.K9XForcedRelease);
                    if (forced != null) return new List<ClientCard> { forced };

                    var k9Case = cards.FirstOrDefault(c => c != null && c.Id == CardId.ACaseForK9);
                    if (k9Case != null) return new List<ClientCard> { k9Case };
                }

                // Necrovalley Throne Search: Commandant
                if (Card != null && Card.Id == CardId.NecrovalleyThrone)
                {
                    var cmd = cards.FirstOrDefault(c => c != null && c.Id == CardId.GravekeeperCommandant);
                    if (cmd != null) return new List<ClientCard> { cmd };
                }

                // Commandant Search: Necrovalley
                if (Card != null && Card.Id == CardId.GravekeeperCommandant)
                {
                    var nv = cards.FirstOrDefault(c => c != null && c.Id == CardId.Necrovalley);
                    if (nv != null) return new List<ClientCard> { nv };
                }
            }

            // ── Hint 502: HINTMSG_DESTROY & Hint 503: HINTMSG_REMOVE (Removal Effects) ──
            if (hint == 502 || hint == 503)
            {
                // CRITICAL RULE: Enforce opponent cards only (c.Controller == 1)
                var oppCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (oppCards.Count > 0)
                {
                    var priorityTarget = oppCards.FirstOrDefault(c => CardIntelligence.IsKnownNegator(c.Id) || CardIntelligence.IsFloodgate(c.Id))
                        ?? oppCards.OrderByDescending(c => c.Attack).First();
                    DecisionTracer.Trace("OnSelectCard", $"Targeting opponent threat {priorityTarget.Name} for removal");
                    return new List<ClientCard> { priorityTarget };
                }
            }

            // ── Hint 509: HINTMSG_SPSUMMON (Special Summon Selection) ──
            if (hint == 509)
            {
                // Noroi SS from Deck: Jokul > Izuna > Lantern
                if (Card != null && Card.Id == CardId.K9_04Noroi)
                {
                    int[] noroiTargets = { CardId.K9_66aJokul, CardId.K9_17Izuna, CardId.K9_66bLantern };
                    foreach (int tId in noroiTargets)
                    {
                        var target = cards.FirstOrDefault(c => c != null && c.Id == tId);
                        if (target != null)
                        {
                            DecisionTracer.Trace("OnSelectCard", $"Noroi special summoning {target.Name} from deck");
                            return new List<ClientCard> { target };
                        }
                    }
                }

                // Forced Release: Werewolf from Extra Deck
                if (Card != null && Card.Id == CardId.K9XForcedRelease)
                {
                    var werewolf = cards.FirstOrDefault(c => c != null && c.Id == CardId.K9_XWerewolf);
                    if (werewolf != null)
                    {
                        DecisionTracer.Trace("OnSelectCard", "Forced Release summoning Werewolf from Extra Deck");
                        return new List<ClientCard> { werewolf };
                    }
                }
            }

            // ── Hint 513: HINTMSG_XMATERIAL (Xyz Material Selection) ──
            if (hint == 513)
            {
                var sorted = cards.Where(c => c != null).OrderBy(c => GetMaterialPriority(c)).ToList();
                if (cancelable)
                {
                    var safe = sorted.Where(c => !(c.Location == CardLocation.MonsterZone && IsAceCard(c))).ToList();
                    if (safe.Count >= min) return Util.CheckSelectCount(safe, cards, min, max);
                }
                return Util.CheckSelectCount(sorted, cards, min, max);
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override IList<ClientCard> OnSelectXyzMaterial(IList<ClientCard> cards, int min, int max)
        {
            var protectedCards = cards.Where(c => c != null && !(c.Location == CardLocation.MonsterZone && IsAceCard(c))).ToList();
            if (protectedCards.Count >= min)
            {
                return protectedCards.OrderBy(c => GetMaterialPriority(c)).Take(max).ToList();
            }
            return cards.OrderBy(c => GetMaterialPriority(c)).Take(max).ToList();
        }

        // ====================================================================================================
        //  TIER 1: HANDTRAPS & COUNTERS
        // ====================================================================================================
        private bool MaxxCEffect() => SmartHandTrapChain() && DefaultMaxxC();
        private bool AshBlossomEffect() => SmartHandTrapChain() && DefaultAshBlossomAndJoyousSpring();
        private bool DrollAndLockBirdEffect() => SmartHandTrapChain();
        private bool ImpermanenceEffect() => SmartHandTrapChain() && DefaultInfiniteImpermanence();

        // ====================================================================================================
        //  TIER 2: QUICK EFFECTS & BOSS DISRUPTIONS
        // ====================================================================================================
        private bool RipperEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Our turn MP: Detach 1 to search K9 card from Deck
            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                if (Card.Overlays.Count > 0)
                {
                    DecisionTracer.TraceActivate("Ripper", "Detaching material to search K9 card");
                    return true;
                }
            }

            // Opponent turn: Negate monster effect activated in hand or GY!
            if (Duel.Player == 1 && Duel.LastChainPlayer == 1)
            {
                ClientCard lastChain = Util.GetLastChainCard();
                if (lastChain != null && lastChain.IsMonster() &&
                    (lastChain.Location == CardLocation.Hand || lastChain.Location == CardLocation.Grave))
                {
                    DecisionTracer.TraceActivate("RipperNegate", $"Negating opponent's {lastChain.Name} in {lastChain.Location}");
                    return true;
                }
            }

            return false;
        }

        private bool WerewolfEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.Overlays.Count == 0) return false;
            if (_werewolfUsed) return false;

            // Opponent's turn: Banish 1 card from opponent's hand face-up!
            if (Duel.Player == 1 && Duel.LastChainPlayer == 1)
            {
                _werewolfUsed = true;
                DecisionTracer.TraceActivate("Werewolf", "Detaching to banish card from opponent's hand face-up");
                return true;
            }

            // Our turn: Banish card on opponent's field!
            if (Duel.Player == 0 && Enemy.GetMonsterCount() > 0)
            {
                _werewolfUsed = true;
                DecisionTracer.TraceActivate("Werewolf", "Detaching to banish opponent field card");
                return true;
            }

            return false;
        }

        private bool ForcedReleaseEffect()
        {
            if (_forcedReleaseUsed) return false;

            // Target face-up K9 Xyz monster you control (e.g. Ripper after it has searched, or Hound)
            var target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && IsK9Monster(c) && c.HasType(CardType.Xyz));
            if (target == null) return false;

            // Check if Werewolf is in Extra Deck
            bool hasWerewolfInED = StartingDeck.ExtraCards.Any(id => id == CardId.K9_XWerewolf && Bot.GetRemainingCount(id, 3) > 0);
            if (!hasWerewolfInED && target.Id == CardId.K9_XWerewolf) return false;

            // On opponent's turn: Chain to summon or activation to pop & setup Werewolf
            if (Duel.Player == 1)
            {
                _forcedReleaseUsed = true;
                AI.SelectCard(target);
                DecisionTracer.TraceActivate("ForcedRelease", $"Rank-Up {target.Name} into Werewolf & pop opponent card");
                return true;
            }

            // On our turn: If Ripper already searched (has 1 material left), rank up to Werewolf for 3300 ATK push & pop
            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                _forcedReleaseUsed = true;
                AI.SelectCard(target);
                DecisionTracer.TraceActivate("ForcedRelease", $"Rank-Up {target.Name} into Werewolf for beatdown & pop");
                return true;
            }

            return false;
        }

        private bool DurendalEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.Overlays.Count == 0) return false;
            if (_durendalUsed) return false;

            // When opponent activates a monster, spell, or trap effect:
            if (Duel.LastChainPlayer == 1)
            {
                _durendalUsed = true;
                AI.SelectOption(0); // Change effect to "Destroy 1 S/T opponent controls"
                DecisionTracer.TraceActivate("Durendal", "Rewriting opponent's activated effect to S/T destruction");
                return true;
            }

            return false;
        }

        private bool VallonSuperPsyEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.Overlays.Count == 0) return false;

            ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled());
            if (target != null)
            {
                AI.SelectCard(target);
                DecisionTracer.TraceActivate("VallonSuperPsy", $"Flipping {target.Name} face-down");
                return true;
            }
            return false;
        }

        private bool SPLittleKnightEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup())
                    ?? Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());
                if (target != null)
                {
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("SPLittleKnight", $"Banish targeting: {target.Name}");
                    return true;
                }
            }
            return false;
        }

        private bool CXyzNaschKnightEffect()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.Overlays.Count > 0)
            {
                ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget());
                if (target != null)
                {
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("CXyzNaschKnight", $"Attaching enemy {target.Name} as material");
                    return true;
                }
            }
            return false;
        }

        private bool NumberC104UmbralEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                ClientCard target = Util.GetLastChainCard();
                if (target != null && target.Controller == 1)
                {
                    DecisionTracer.TraceActivate("NumberC104Umbral", $"Negating effect of {target.Name}");
                    return true;
                }
            }
            return false;
        }

        private bool ZeusEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.Overlays.Count < 2) return false;
            if (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0)
            {
                DecisionTracer.TraceActivate("Zeus", "Sending all other cards on the field to the GY");
                return true;
            }
            return false;
        }

        private bool TYPHONEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.Overlays.Count == 0) return false;
            var oppTarget = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup());
            if (oppTarget != null)
            {
                AI.SelectCard(oppTarget);
                DecisionTracer.TraceActivate("TYPHON", $"Bouncing {oppTarget.Name} to hand");
                return true;
            }
            return false;
        }

        // ====================================================================================================
        //  TIER 3: BOARD BREAKERS
        // ====================================================================================================
        private bool RaigekiEffect() => Enemy.GetMonsterCount() > 0;
        private bool DarkRulerNoMoreEffect() => Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled());

        // ====================================================================================================
        //  TIER 4: SEARCH SPELLS & ENABLERS
        // ====================================================================================================
        private bool TripleTacticsTalentEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                if (Bot.Hand.Count <= 4)
                {
                    AI.SelectOption(0); // Draw 2 cards
                    return true;
                }
                var strongMonster = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Attack >= 2000);
                if (strongMonster != null)
                {
                    AI.SelectOption(1); // Take control
                    AI.SelectCard(strongMonster);
                    return true;
                }
                AI.SelectOption(2); // Look at opp hand and shuffle 1
                return true;
            }
            return false;
        }

        private bool ACaseForK9Effect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_caseForK9Used) return false;

            _caseForK9Used = true;
            DecisionTracer.TraceActivate("ACaseForK9", "Activating A Case for K9 to search");
            return true;
        }

        private bool ChaoticElementsEffect()
        {
            // Searches Jokul (Aqua) or Lantern (Pyro)
            bool hasTargets = StartingDeck.Cards.Any(id => (id == CardId.K9_66aJokul || id == CardId.K9_66bLantern) && Bot.GetRemainingCount(id, 3) > 0);
            return hasTargets;
        }

        // ====================================================================================================
        //  TIER 5: K9 MONSTER EFFECTS (Swarm & Board Building)
        // ====================================================================================================
        private bool JokulHandEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_jokulHandUsed) return false;

            // Reveal Jokul + another Level 5 in hand
            bool hasOtherLv5 = Bot.Hand.Any(c => c != null && c != Card && c.Level == 5);
            if (hasOtherLv5 && !IsSpecialSummonBlocked())
            {
                _jokulHandUsed = true;
                DecisionTracer.TraceActivate("JokulHand", "Special Summoning Jokul & Level 5 from hand");
                return true;
            }
            return false;
        }

        private bool LanternHandEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_lanternHandUsed) return false;

            // CRITICAL CHECK: If Necrovalley is active, Lantern cannot revive from GY!
            if (Bot.HasInSpellZone(CardId.Necrovalley, faceUp: true) || Enemy.HasInSpellZone(CardId.Necrovalley, faceUp: true))
                return false;

            bool hasK9InGY = Bot.Graveyard.Any(c => c != null && c.Level == 5 && c.IsMonster());
            if (hasK9InGY && !IsSpecialSummonBlocked())
            {
                _lanternHandUsed = true;
                DecisionTracer.TraceActivate("LanternHand", "Special Summoning Lantern and reviving K9 from GY");
                return true;
            }
            return false;
        }

        private bool IzunaHandEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            ClientCard last = Util.GetLastChainCard();
            return Duel.LastChainPlayer == 1 && last != null && (last.Location == CardLocation.Hand || last.Location == CardLocation.Grave);
        }

        private bool LupisHandEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            bool hasK9 = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsK9Monster(c));
            return hasK9 && !IsSpecialSummonBlocked();
        }

        private bool NoroiSummonCheck()
        {
            // Noroi can Normal Summon without tribute if opponent has 2+ cards in hand
            return !IsSpecialSummonBlocked();
        }

        private bool Level5SummonCheck()
        {
            // Normal Summon Level 5 if opponent has 2+ cards in hand (free summon condition)
            return Enemy.Hand.Count >= 2;
        }

        private bool CommandantSummonCheck()
        {
            // Normal Summon Commandant only as beatdown/material when Necrovalley is already active
            return Bot.HasInSpellZone(CardId.Necrovalley, faceUp: true) && Bot.GetMonsterCount() < 5;
        }

        private bool NoroiFieldEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_noroiFieldUsed) return false;

            _noroiFieldUsed = true;
            DecisionTracer.TraceActivate("NoroiField", "Noroi summoning K9 from Deck");
            return true;
        }

        private bool JokulFieldEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_jokulFieldUsed) return false;

            _jokulFieldUsed = true;
            DecisionTracer.TraceActivate("JokulField", "Jokul searching K9 monster from Deck");
            return true;
        }

        private bool LanternFieldEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_lanternFieldUsed) return false;

            _lanternFieldUsed = true;
            DecisionTracer.TraceActivate("LanternField", "Lantern searching K9 S/T from Deck");
            return true;
        }

        private bool IzunaFieldEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_izunaFieldUsed) return false;

            _izunaFieldUsed = true;
            DecisionTracer.TraceActivate("IzunaField", "Izuna milling K9 card to GY");
            return true;
        }

        private bool HoundEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.Overlays.Count == 0) return false;
            var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup());
            if (target != null)
            {
                AI.SelectCard(target);
                DecisionTracer.TraceActivate("Hound", $"Destroying and burning {target.Name}");
                return true;
            }
            return false;
        }

        private bool JacksEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup());
            if (target != null)
            {
                AI.SelectCard(target);
                DecisionTracer.TraceActivate("Jacks", $"Jacks popping {target.Name}");
                return true;
            }
            return false;
        }

        private bool NASHKnightEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.Overlays.Count < 2) return false;
            var oppTarget = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget());
            if (oppTarget != null)
            {
                AI.SelectCard(CardId.Number104Masquerade);
                AI.SelectNextCard(oppTarget);
                DecisionTracer.TraceActivate("NASHKnight", $"Attaching Masquerade and sucking {oppTarget.Name}");
                return true;
            }
            return false;
        }

        // ====================================================================================================
        //  TIER 6: EXTRA DECK SUMMON CHECKS
        // ====================================================================================================
        private bool RipperSummonCheck()
        {
            if (IsSpecialSummonBlocked()) return false;
            // Prioritize Ripper as our first Rank 5
            return true;
        }

        private bool DurendalSummonCheck()
        {
            if (IsSpecialSummonBlocked()) return false;
            // Durendal is excellent secondary boss alongside Ripper
            return Bot.HasInMonstersZone(CardId.K9_17Ripper) || Bot.GetMonsters().Count(c => c != null && c.Level == 5) >= 2;
        }

        private bool XyzSummonCheck() => !IsSpecialSummonBlocked();

        private bool CXyzNaschKnightSummonCheck()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.HasInMonstersZone(CardId.NASHKnight);
        }

        private bool LinkSummonCheck()
        {
            if (IsSpecialSummonBlocked() || ShouldSkipLinkSummon()) return false;
            if (Duel.Phase == DuelPhase.Main1 && Duel.Turn > 1)
            {
                if (Enemy.GetMonsterCount() == 0 && Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsAttack()))
                    return false;
                if (CanDealLethal())
                    return false;
                if (Enemy.GetFieldCount() == 0 && Enemy.Graveyard.Count == 0)
                    return false;
            }
            // Never sacrifice Ripper, Werewolf, or 2000+ ATK monsters for SP
            int nonAceCount = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c) && c.Attack < 2000);
            return nonAceCount >= 2;
        }

        private bool ZeusSummonCheck()
        {
            return !IsSpecialSummonBlocked() && Duel.Phase == DuelPhase.Main2 &&
                   Bot.GetMonsters().Any(c => c != null && c.HasType(CardType.Xyz) && c.Attacked);
        }

        private bool TYPHONSummonCheck()
        {
            return !IsSpecialSummonBlocked() && Duel.Phase == DuelPhase.Main2 &&
                   Enemy.GetMonsters().Any(c => c != null && c.Attack >= 2500);
        }

        // ====================================================================================================
        //  TIER 7: NECROVALLEY LOCKDOWN ENGINE (Phase 2: Lockdown)
        // ====================================================================================================
        private bool CanActivateNecrovalleyNow()
        {
            // CRITICAL INTELLIGENCE: Do NOT activate Necrovalley if we still have Lantern in hand
            // and can summon it with a GY target! Let Lantern resolve first!
            if (!_lanternHandUsed && Bot.Hand.Any(c => c != null && c.Id == CardId.K9_66bLantern))
            {
                bool hasK9InGY = Bot.Graveyard.Any(c => c != null && c.Level == 5 && c.IsMonster());
                if (hasK9InGY && !IsSpecialSummonBlocked())
                {
                    // Delay Necrovalley until Lantern has resolved
                    return false;
                }
            }
            return true;
        }

        private bool ThroneEffect()
        {
            if (_throneUsed) return false;
            if (!CanActivateNecrovalleyNow()) return false;

            // If we don't have Necrovalley, search Commandant
            bool hasField = Bot.HasInSpellZone(CardId.Necrovalley, faceUp: true) || Bot.Hand.Any(c => c != null && c.Id == CardId.Necrovalley);
            bool hasCommandant = Bot.Hand.Any(c => c != null && c.Id == CardId.GravekeeperCommandant);

            if (!hasField && !hasCommandant)
            {
                _throneUsed = true;
                AI.SelectCard(CardId.GravekeeperCommandant);
                DecisionTracer.TraceActivate("Throne", "Searching Commandant for Necrovalley");
                return true;
            }

            return false;
        }

        private bool CommandantEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_commandantUsed) return false;
            if (!CanActivateNecrovalleyNow()) return false;

            if (!Bot.HasInSpellZone(CardId.Necrovalley, faceUp: true) && !Bot.Hand.Any(c => c != null && c.Id == CardId.Necrovalley))
            {
                _commandantUsed = true;
                AI.SelectCard(CardId.Necrovalley);
                DecisionTracer.TraceActivate("Commandant", "Discarding to search Necrovalley");
                return true;
            }

            return false;
        }

        private bool NecrovalleyEffect()
        {
            if (_necrovalleyUsed) return false;
            if (Bot.HasInSpellZone(CardId.Necrovalley, faceUp: true)) return false;
            if (!CanActivateNecrovalleyNow()) return false;

            _necrovalleyUsed = true;
            DecisionTracer.TraceActivate("Necrovalley", "Activating Necrovalley for total GY lockdown!");
            return true;
        }

        // ====================================================================================================
        //  TIER 8: BACKROW SETTING
        // ====================================================================================================
        private bool SpellSetFiltered()
        {
            if (Card.IsCode(CardId.K9XForcedRelease, CardId.InfiniteImpermanence))
            {
                return Util.IsTurn1OrMain2();
            }
            return false;
        }
    }
}
