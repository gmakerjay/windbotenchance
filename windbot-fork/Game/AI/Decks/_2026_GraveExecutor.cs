// ============================================================
// CARD AUDIT — 2026_Grave
// | Card Name           | Type    | OPT? | Cost      | Effect              | Activate When        | NEVER When           |
// |---------------------|---------|------|-----------|---------------------|----------------------|----------------------|
// | GK Commandant       | Monster | None | Discard   | Search Necrovalley  | In hand to get field | Necrovalley active   |
// | GK Chief            | Monster | None | None      | Extra Summon + revive| Summon to swarm      | SS blocked           |
// | GK Shaman           | Monster | None | None      | GK protection       | Summon to protect    | Already active       |
// | Necrovalley         | Spell   | None | None      | GY floodgate lock   | Main Phase to lock   | Already active       |
// | K9-66a Jokul        | Monster | HOPT | None      | SS self + search K9 | Hand has Level 5 K9  | No target in deck    |
// ACE CARDS: Primary: Gravekeeper's Chief / Secondary: K9-X Werewolf, Jacks, Hound, Ripper
// COMBO STARTERS: 1. Gravekeeper's Commandant 2. Necrovalley Throne 3. K9-66a Jokul
// CHOKEPOINTS: Ash Blossom on Commandant or Throne
// WIN CONDITION: Establish Necrovalley GY lock, then use K9 special summons to build Xyz bosses (Ripper, Werewolf, Jacks).
// GOING 1ST END BOARD: Necrovalley + K9-17 Ripper + Gravekeeper's Shaman
// GOING 2ND GAMEPLAN: Clear board with Raigeki / Dark Ruler, then setup Necrovalley and push damage.
// ============================================================

// ============================================================
// COMBO DRAFT — 2026_Grave
// ============================================================
// === COMBO LINE 1: Necrovalley Setup (Starter: Commandant / Throne) ===
// HAND REQUIRED: Commandant or Throne
// STEP 1: If hand has Throne, activate Throne → Search Commandant
// STEP 2: Discard Commandant → Search Necrovalley
// STEP 3: Activate Necrovalley
// END BOARD: Necrovalley active
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
    [Deck("2026_Grave", "2026_Grave")]
    public class _2026_GraveExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck - Gravekeeper
            public const int GravekeeperCommandant = 17393207;
            public const int GravekeeperChief = 62473983;
            public const int GravekeeperShaman = 58139128;
            public const int Necrovalley = 47355498;
            public const int NecrovalleyThrone = 37561138;
            public const int DarkRenewal = 9287078;

            // Main Deck - K9
            public const int K9_66aJokul = 28642461;
            public const int K9_66bLantern = 55031170;
            public const int K9_00Lupis = 91025875;
            public const int K9_17Izuna = 92248362;
            public const int K9_04Noroi = 47960073;

            // Hand Traps & Staples
            public const int DrollAndLockBird = 94145021;
            public const int MaxxC = 23434538;
            public const int AshBlossom = 14558127;
            public const int Raigeki = 12580477;
            public const int DarkRulerNoMore = 54693926;
            public const int TripleTacticsTalent = 25311006;
            public const int TripleTacticsThrust = 35269904;
            public const int CalledByTheGrave = 24224830;
            public const int DimensionalBarrier = 83326048;
            public const int ACaseForK9 = 80181649;
            public const int ChaoticElements = 92221402;
            public const int IllusionGate = 33017964;
            public const int K9XForcedRelease = 53792930;

            // Extra Deck
            public const int Number104Masquerade = 2061963;
            public const int NumberC104Umbral = 49456901;
            public const int InfinitrackRiverStormer = 24701066;
            public const int NASHKnight = 34876719;
            public const int ImperialPrincessQuinquery = 29510428;
            public const int K9_17Ripper = 27420823;
            public const int K9_66XJacks = 67515699;
            public const int K9_00Hound = 54919528;
            public const int VallonSuperPsy = 40673853;
            public const int CXyzNaschKnight = 61374414;
            public const int K9_XWerewolf = 90303227;
            public const int Zeus = 90448279;
            public const int TYPHON = 93039339;
            public const int SPLittleKnight = 29301450;
        }

        private static readonly int[] AceCardIds = {
            CardId.GravekeeperChief,
            CardId.GravekeeperShaman,
            CardId.K9_XWerewolf,
            CardId.K9_17Ripper,
            CardId.K9_66XJacks,
            CardId.K9_00Hound,
            CardId.SPLittleKnight
        };

        private bool _commandantUsed = false;
        private bool _throneUsed = false;
        private bool _necrovalleyUsed = false;
        private bool _jokulUsed = false;
        private bool _lanternUsed = false;
        private bool _lupisUsed = false;

        // ═══════════════════════════════════════
        //  HAND MANAGEMENT HELPERS
        // ═══════════════════════════════════════
        private bool HandNearFull() => Bot.Hand.Count >= 5;

        private bool SearchWouldOverflow()
        {
            if (!HandNearFull()) return false;
            // If we have monsters on field to Xyz away, we can manage hand size
            if (Bot.GetMonsterCount() >= 2) return false;
            return true;
        }

        public _2026_GraveExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            HeuristicGuard.RegisterAceCards(AceCardIds);

            // ── Combo Router ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Necrovalley-Setup",
                RequiredCards = new List<int> { CardId.GravekeeperCommandant },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.GravekeeperCommandant, ActionType = ExecutorType.Activate, Description = "Discard Commandant to search" },
                    new() { CardId = CardId.Necrovalley, ActionType = ExecutorType.Activate, Description = "Activate Necrovalley" }
                },
                EndBoardScore = 65
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "K9-Xyz-Rush",
                RequiredCards = new List<int> { CardId.K9_66aJokul },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.K9_66aJokul, ActionType = ExecutorType.Activate, Description = "SS Jokul + search K9" },
                    new() { CardId = CardId.K9_17Ripper, ActionType = ExecutorType.SpSummon, Description = "Xyz Summon Ripper" }
                },
                EndBoardScore = 75
            });

            BaitPlanner.RegisterComboStarters(CardId.NecrovalleyThrone, CardId.GravekeeperCommandant);
            BaitPlanner.RegisterBaitCards(CardId.Raigeki, CardId.DarkRulerNoMore);

            // ===== Priority 1: Hand Traps & Reactives =====
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsThrust, TripleTacticsThrustEffect);

            // ===== Priority 2: Board Breakers =====
            AddExecutor(ExecutorType.Activate, CardId.Raigeki, RaigekiEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarkRulerNoMore, DarkRulerNoMoreEffect);

            // ===== Priority 3: Boss Monster Quick Effects =====
            AddExecutor(ExecutorType.Activate, CardId.K9_17Ripper, RipperEffect);
            AddExecutor(ExecutorType.Activate, CardId.K9_XWerewolf);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightEffect);
            AddExecutor(ExecutorType.Activate, CardId.TYPHON);
            AddExecutor(ExecutorType.Activate, CardId.NumberC104Umbral, NumberC104UmbralEffect);
            AddExecutor(ExecutorType.Activate, CardId.CXyzNaschKnight, CXyzNaschKnightEffect);
            AddExecutor(ExecutorType.Activate, CardId.VallonSuperPsy, VallonSuperPsyEffect);
            AddExecutor(ExecutorType.Activate, CardId.InfinitrackRiverStormer, InfinitrackRiverStormerEffect);
            AddExecutor(ExecutorType.Activate, CardId.ImperialPrincessQuinquery, ImperialPrincessQuinqueryEffect);

            // ===== Priority 4: Search & Setup Spells =====
            AddExecutor(ExecutorType.Activate, CardId.NecrovalleyThrone, ThroneEffect);
            AddExecutor(ExecutorType.Activate, CardId.Necrovalley, NecrovalleyEffect);
            AddExecutor(ExecutorType.Activate, CardId.ChaoticElements, ChaoticElementsEffect);
            AddExecutor(ExecutorType.Activate, CardId.IllusionGate, IllusionGateEffect);
            // ===== Priority 5: Monster Effects (Hand / Field) =====
            AddExecutor(ExecutorType.Activate, CardId.GravekeeperCommandant, CommandantEffect);
            AddExecutor(ExecutorType.Activate, CardId.K9_66aJokul, JokulHandEffect);
            AddExecutor(ExecutorType.Activate, CardId.K9_66bLantern, LanternHandEffect);
            AddExecutor(ExecutorType.Activate, CardId.K9_17Izuna, IzunaHandEffect);
            AddExecutor(ExecutorType.Activate, CardId.GravekeeperChief, ChiefEffect);
            AddExecutor(ExecutorType.Activate, CardId.GravekeeperShaman, ShamanEffect);
            AddExecutor(ExecutorType.Activate, CardId.K9_66aJokul, JokulFieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.K9_66bLantern, LanternFieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.K9_17Izuna, IzunaFieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.K9_00Lupis, LupisEffect);

            // ===== Priority 6: Special Summons (K9 / Extra) =====
            AddExecutor(ExecutorType.SpSummon, CardId.K9_66aJokul, K9SpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.K9_66bLantern, K9SpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.K9_17Izuna, K9SpSummon);

            // ===== Priority 7: Normal Summons =====
            AddExecutor(ExecutorType.Summon, CardId.GravekeeperChief);
            AddExecutor(ExecutorType.Summon, CardId.GravekeeperCommandant, NormalSummonCommandant);
            AddExecutor(ExecutorType.Summon, CardId.GravekeeperShaman);
            AddExecutor(ExecutorType.Summon, CardId.K9_66aJokul);
            AddExecutor(ExecutorType.Summon, CardId.K9_04Noroi);

            // ===== Monster Field Effects (after Normal Summon) =====
            AddExecutor(ExecutorType.Activate, CardId.K9_04Noroi, NoroiFieldEffect);

            // ===== Priority 8: Extra Deck Summons =====
            AddExecutor(ExecutorType.SpSummon, CardId.K9_17Ripper, XyzSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.K9_66XJacks, XyzSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.K9_00Hound, XyzSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.K9_XWerewolf, XyzSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.NASHKnight, XyzSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.Number104Masquerade, XyzSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.NumberC104Umbral, XyzSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.CXyzNaschKnight, XyzSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.VallonSuperPsy, XyzSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.InfinitrackRiverStormer, XyzSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.ImperialPrincessQuinquery, XyzSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, LinkSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.Zeus, ZeusSummon);

            // ===== Priority 9: Traps & Repos =====
            AddExecutor(ExecutorType.Activate, CardId.DarkRenewal, DarkRenewalEffect);
            AddExecutor(ExecutorType.Activate, CardId.DimensionalBarrier, DimensionalBarrierEffect);
            AddExecutor(ExecutorType.Activate, CardId.K9XForcedRelease, ForcedReleaseEffect);
            AddExecutor(ExecutorType.SpellSet, SpellSetFiltered);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _commandantUsed = false;
            _throneUsed = false;
            _necrovalleyUsed = false;
            _jokulUsed = false;
            _lanternUsed = false;
            _lupisUsed = false;

            if (ShouldGoBreakBoard)
            {
                // Reset Necrovalley activation for going-second: prioritize board breaking over lock
                _necrovalleyUsed = false;
            }
        }

        public override bool OnSelectHand()
        {
            return true; // Go First to setup Necrovalley lock
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Contains(card.Id);
        }

        protected override bool IsBoardStrongEnough()
        {
            // Necrovalley + K9 Xyz boss = complete lock board
            bool hasNecrovalley = Bot.HasInSpellZone(CardId.Necrovalley, faceUp: true);
            bool hasXyzBoss = Bot.HasInMonstersZone(CardId.K9_17Ripper)
                || Bot.HasInMonstersZone(CardId.K9_XWerewolf)
                || Bot.HasInMonstersZone(CardId.K9_66XJacks);
            if (hasNecrovalley && hasXyzBoss)
                return true;
            // Necrovalley + Shaman = sufficient stall board
            if (hasNecrovalley && Bot.HasInMonstersZone(CardId.GravekeeperShaman))
                return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            // Stop extending once we have a K9 Xyz boss on field
            bool hasXyzBoss = Bot.HasInMonstersZone(CardId.K9_17Ripper)
                || Bot.HasInMonstersZone(CardId.K9_XWerewolf)
                || Bot.HasInMonstersZone(CardId.K9_66XJacks);
            if (!hasXyzBoss) return false;
            return base.ShouldStopExtending();
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c))
            {
                // Ace cards on the field are protected from being used as Link/Xyz/Synchro material
                if (c.Location == CardLocation.MonsterZone && c.IsFaceup())
                    return int.MaxValue;
                return 900; // Protect Chief, Werewolf, SP, etc.
            }
            if (c.IsCode(CardId.AshBlossom, CardId.MaxxC, CardId.DrollAndLockBird))
                return 800; // Protect hand traps
            return 100;
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Hint 509: Special Summon -> Filter by Location (prefer Deck over Hand/Grave to avoid crashes)
            if (hint == 509)
            {
                var deckCards = cards.Where(c => c != null && c.Location == CardLocation.Deck).ToList();
                if (deckCards.Count > 0 && cards.Any(c => c.Location != CardLocation.Deck))
                {
                    DecisionTracer.Trace("OnSelectCard", "Filtering hint 509 Special Summon to Deck location");
                    return Util.CheckSelectCount(deckCards, cards, min, max);
                }
            }

            // Hints 511/512/513/533: Material selection
            if (hint == 511 || hint == 512 || hint == 513 || hint == 533)
            {
                var sorted = cards.Where(c => c != null)
                    .OrderBy(c => GetMaterialPriority(c))
                    .ToList();

                if (cancelable)
                {
                    // Protect field Ace cards and hand traps from being used as material
                    var protectedCards = sorted.Where(c => !(c.Location == CardLocation.MonsterZone && IsAceCard(c))).ToList();
                    if (protectedCards.Count < min)
                    {
                        DecisionTracer.Trace("OnSelectCard", "Cancelling material selection to protect field Ace cards");
                        return null; // Cancel to protect Aces
                    }
                    return Util.CheckSelectCount(protectedCards, cards, min, max);
                }
                return Util.CheckSelectCount(sorted, cards, min, max);
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override IList<ClientCard> OnSelectXyzMaterial(IList<ClientCard> cards, int min, int max)
        {
            // Protect Ace cards and hand traps from being used as Xyz material
            var protectedCards = cards.Where(c => c != null && !(c.Location == CardLocation.MonsterZone && IsAceCard(c))).ToList();
            if (protectedCards.Count >= min)
            {
                var sorted = protectedCards.OrderBy(c => GetMaterialPriority(c)).ToList();
                DecisionTracer.Trace("OnSelectXyzMaterial", $"Using {sorted.Count} non-Ace monsters for Xyz material");
                return sorted.Take(max).ToList();
            }

            // Not enough non-Ace — include Ace cards but log warning
            var allSorted = cards.Where(c => c != null).OrderBy(c => GetMaterialPriority(c)).ToList();
            var fieldAces = allSorted.Where(c => c.Location == CardLocation.MonsterZone && IsAceCard(c)).ToList();
            if (fieldAces.Any())
            {
                DecisionTracer.Trace("OnSelectXyzMaterial", $"WARNING: Forced to use field Ace card {fieldAces.First().Name} as Xyz material");
            }
            return allSorted.Take(max).ToList();
        }

        public override IList<ClientCard> OnSelectLinkMaterial(IList<ClientCard> cards, int min, int max)
        {
            // Protect Ace cards and hand traps from being used as Link material
            var protectedCards = cards.Where(c => c != null && !(c.Location == CardLocation.MonsterZone && IsAceCard(c))).ToList();
            if (protectedCards.Count >= min)
            {
                var sorted = protectedCards.OrderBy(c => GetMaterialPriority(c)).ToList();
                DecisionTracer.Trace("OnSelectLinkMaterial", $"Using {sorted.Count} non-Ace monsters for Link material");
                return sorted.Take(max).ToList();
            }

            // Not enough non-Ace — include Ace cards but log warning
            var allSorted = cards.Where(c => c != null).OrderBy(c => GetMaterialPriority(c)).ToList();
            var fieldAces = allSorted.Where(c => c.Location == CardLocation.MonsterZone && IsAceCard(c)).ToList();
            if (fieldAces.Any())
            {
                DecisionTracer.Trace("OnSelectLinkMaterial", $"WARNING: Forced to use field Ace card {fieldAces.First().Name} as Link material");
            }
            return allSorted.Take(max).ToList();
        }

        public override bool ShouldAllowSpSummon(ClientCard card)
        {
            if (!base.ShouldAllowSpSummon(card)) return false;
            if (card == null) return true;

            bool isExtraDeckSummon = card.HasType(CardType.Link) || card.HasType(CardType.Fusion)
                || card.HasType(CardType.Synchro) || card.HasType(CardType.Xyz);
            if (!isExtraDeckSummon) return true;

            // Link Summons
            if (card.HasType(CardType.Link))
            {
                int reqRating = card.Level; // Level is Link Rating for Link monsters
                if (!IsMaterialSelectionSafeForLink(reqRating))
                {
                    DecisionTracer.TraceSkip("ShouldAllowSpSummon", $"Summoning {card.Name} is not safe (would consume Ace card(s) as Link material)");
                    return false;
                }
            }

            // Xyz Summons
            if (card.HasType(CardType.Xyz))
            {
                if (card.Id == CardId.Zeus || card.Id == CardId.TYPHON)
                    return true;

                int rank = card.Level; // Level represents Rank for Xyz in ED
                int reqCount = 2; // Default required materials
                if (card.Id == CardId.Number104Masquerade) reqCount = 3;
                else if (card.Id == CardId.NumberC104Umbral) reqCount = 4;
                else if (card.Id == CardId.CXyzNaschKnight) reqCount = 3;

                if (!IsMaterialSelectionSafeForXyz(rank, reqCount))
                {
                    DecisionTracer.TraceSkip("ShouldAllowSpSummon", $"Summoning {card.Name} is not safe (would consume Ace card(s) as Xyz material)");
                    return false;
                }
            }

            return true;
        }

        private bool IsMaterialSelectionSafeForLink(int requiredRating)
        {
            var faceUpNonAceMonsters = Bot.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && !IsAceCard(c))
                .ToList();

            int availableRating = 0;
            foreach (var m in faceUpNonAceMonsters)
            {
                if (m.HasType(CardType.Link))
                    availableRating += m.Level;
                else
                    availableRating += 1;
            }

            return faceUpNonAceMonsters.Count >= requiredRating && availableRating >= requiredRating;
        }

        private bool IsMaterialSelectionSafeForXyz(int rank, int reqCount)
        {
            int nonAceMatchingLevelCount = Bot.GetMonsters()
                .Count(c => c != null && c.IsFaceup() && c.Level == rank && !IsAceCard(c));

            return nonAceMatchingLevelCount >= reqCount;
        }

        private bool XyzSummonCheck()
        {
            bool cond = !IsSpecialSummonBlocked();
            if (!cond)
                DecisionTracer.TraceSkip("XyzSummonCheck", "Special Summon blocked by floodgate");
            return cond;
        }

        private bool LinkSummonCheck()
        {
            if (IsSpecialSummonBlocked())
            {
                DecisionTracer.TraceSkip("LinkSummonCheck", "Special Summon blocked by floodgate");
                return false;
            }
            if (ShouldSkipLinkSummon()) return false;
            return true;
        }

        private bool MaxxCEffect()
        {
            bool cond = SmartHandTrapChain() && DefaultMaxxC();
            if (cond)
                DecisionTracer.TraceActivate("MaxxC", "Activating Maxx C");
            return cond;
        }

        private bool DrollAndLockBirdEffect()
        {
            bool cond = SmartHandTrapChain();
            if (cond)
                DecisionTracer.TraceActivate("DrollAndLockBird", "Activating Droll & Lock Bird");
            return cond;
        }

        private bool CalledByTheGraveEffect()
        {
            ClientCard target = Util.GetLastChainCard();
            if (target != null && target.Controller == 1 && target.IsMonster())
            {
                ClientCard targetGrave = Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsCode(target.Id));
                if (targetGrave != null)
                {
                    // Note: Called by the Grave banishes from GY, which is NEGATED if Necrovalley is active!
                    if (Bot.HasInSpellZone(CardId.Necrovalley, faceUp: true) || Enemy.HasInSpellZone(CardId.Necrovalley, faceUp: true))
                    {
                        DecisionTracer.TraceSkip("CalledByTheGrave", "Necrovalley is active; GY movements blocked");
                        return false; // Skip because Necrovalley blocks it
                    }
                    
                    AI.SelectCard(targetGrave);
                    DecisionTracer.TraceActivate("CalledByTheGrave", $"Banish targeting: {targetGrave.Name}");
                    return true;
                }
            }
            return false;
        }

        private bool TripleTacticsTalentEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                // [HAND-FLOW] TTT Draw 2 adds net +2 to hand.
                // Skip if hand is near-full to avoid overflow.
                if (SearchWouldOverflow())
                {
                    DecisionTracer.TraceSkip("TripleTacticsTalent", "Hand near full — skipping draw to avoid overflow");
                    return false;
                }
                AI.SelectOption(0); // Draw 2
                DecisionTracer.TraceActivate("TripleTacticsTalent", "Drawing 2 cards");
                return true;
            }
            return false;
        }

        private bool TripleTacticsThrustEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                AI.SelectCard(new[] {
                    CardId.DimensionalBarrier,
                    CardId.DarkRenewal,
                    CardId.IllusionGate,
                    CardId.Raigeki
                });
                DecisionTracer.TraceActivate("TripleTacticsThrust", "Searching Normal Spell/Trap");
                return true;
            }
            return false;
        }

        private bool RaigekiEffect()
        {
            bool cond = Enemy.GetMonsterCount() > 0;
            if (cond)
                DecisionTracer.TraceActivate("Raigeki", "Activating Raigeki to clear field");
            return cond;
        }

        private bool DarkRulerNoMoreEffect()
        {
            bool cond = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled());
            if (cond)
                DecisionTracer.TraceActivate("DarkRulerNoMore", "Activating Dark Ruler No More");
            return cond;
        }

        private bool RipperEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                // Our turn Main Phase: Ignition effect — detach 1 material, search 1 "K9" card from Deck
                // The card selection will be prompted at resolution via OnSelectCard
                DecisionTracer.TraceActivate("K9Ripper", "Searching K9 card from deck");
                return true;
            }
            else if (Duel.Player == 1 && Duel.LastChainPlayer == 1)
            {
                // Opponent's turn: Quick effect — detach 1 material, negate opponent monster effect activated in hand or GY
                ClientCard lastChain = Util.GetLastChainCard();
                if (lastChain != null && lastChain.IsMonster() &&
                    (lastChain.Location == CardLocation.Hand || lastChain.Location == CardLocation.Grave))
                {
                    DecisionTracer.TraceActivate("K9RipperNegate", $"Negating opponent's {lastChain.Name} in {lastChain.Location}");
                    return true;
                }
            }
            return false;
        }

        private bool SPLittleKnightEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null);
                if (target != null)
                {
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("SPLittleKnight", $"Banish targeting: {target.Id}");
                    return true;
                }
            }
            return false;
        }

        private bool ThroneEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_throneUsed) return false;

            // [HAND-FLOW] Throne adds Commandant from deck to hand (net +1).
            // Skip if hand is near-full to avoid overflow.
            if (SearchWouldOverflow())
            {
                DecisionTracer.TraceSkip("ThroneEffect", "Hand near full — skipping search to avoid overflow");
                return false;
            }

            // Check there is a target in the deck to search
            if (GetRemainingCount(CardId.GravekeeperCommandant) == 0)
            {
                DecisionTracer.TraceSkip("ThroneEffect", "No Gravekeeper Commandant left in deck");
                return false;
            }
            // Search Gravekeeper's Commandant
            AI.SelectCard(CardId.GravekeeperCommandant);
            _throneUsed = true;
            DecisionTracer.TraceActivate("NecrovalleyThrone", "Searching Gravekeeper Commandant");
            return true;
        }

        private bool NecrovalleyEffect()
        {
            if (_necrovalleyUsed) return false;
            if (Bot.HasInSpellZone(CardId.Necrovalley, faceUp: true)) return false;
            _necrovalleyUsed = true;
            DecisionTracer.TraceActivate("Necrovalley", "Activating Necrovalley field spell");
            return true;
        }

        private bool CommandantEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_commandantUsed) return false;
            if (Card.Location == CardLocation.Hand)
            {
                // Discard itself to search Necrovalley — net: -1 (discard) + 1 (search) = 0
                // Always safe, doesn't cause overflow.
                if (!Bot.HasInSpellZone(CardId.Necrovalley, faceUp: true))
                {
                    AI.SelectCard(CardId.Necrovalley);
                    _commandantUsed = true;
                    DecisionTracer.TraceActivate("GravekeeperCommandant", "Discarding to search Necrovalley");
                    return true;
                }
            }
            return false;
        }

        private bool JokulHandEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_jokulUsed) return false;
            bool cond = Bot.Hand.Any(c => c != null && c.Level == 5 && c.Id != CardId.K9_66aJokul);
            if (cond)
            {
                _jokulUsed = true;
                DecisionTracer.TraceActivate("JokulHand", "Summoning Jokul from hand by revealing Lvl 5 K9");
            }
            return cond;
        }

        private bool LanternHandEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_lanternUsed) return false;
            if (Bot.HasInSpellZone(CardId.Necrovalley, faceUp: true) || Enemy.HasInSpellZone(CardId.Necrovalley, faceUp: true))
            {
                DecisionTracer.TraceSkip("LanternHand", "Necrovalley blocks GY movements");
                return false;
            }
            bool cond = Bot.Graveyard.Any(c => c != null && c.Level == 5);
            if (cond)
            {
                _lanternUsed = true;
                DecisionTracer.TraceActivate("LanternHand", "Summoning Lantern from hand by reviving Lvl 5 K9");
            }
            return cond;
        }

        private bool IzunaHandEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // SpSummon on opponent hand/GY effect trigger
            ClientCard last = Util.GetLastChainCard();
            bool cond = Duel.LastChainPlayer == 1 && last != null && (last.Location == CardLocation.Hand || last.Location == CardLocation.Grave);
            if (cond)
                DecisionTracer.TraceActivate("IzunaHand", "Summoning Izuna on opponent hand/GY effect");
            return cond;
        }

        private bool ChiefEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Target Gravekeeper in GY to revive (Note: Chief's card text allows it under Necrovalley)
                var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCode(CardId.GravekeeperCommandant, CardId.GravekeeperShaman));
                if (target != null)
                {
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("GravekeeperChief", $"Reviving Gravekeeper {target.Name} from GY");
                    return true;
                }
            }
            return false;
        }

        private bool ShamanEffect()
        {
            DecisionTracer.TraceActivate("GravekeeperShaman", "Gravekeeper Shaman active on field");
            return true;
        }

        private bool JokulFieldEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(CardId.K9_17Izuna);
                DecisionTracer.TraceActivate("JokulField", "Searching K9 monster from deck");
                return true;
            }
            return false;
        }

        private bool LanternFieldEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(CardId.ACaseForK9);
                DecisionTracer.TraceActivate("LanternField", "Searching A Case for K9");
                return true;
            }
            return false;
        }

        private bool IzunaFieldEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(CardId.K9_66bLantern);
                DecisionTracer.TraceActivate("IzunaField", "Sending K9 monster from deck to GY");
                return true;
            }
            return false;
        }

        private bool IsK9Card(int id)
        {
            return id == CardId.K9_66aJokul ||
                   id == CardId.K9_66bLantern ||
                   id == CardId.K9_00Lupis ||
                   id == CardId.K9_17Izuna ||
                   id == CardId.K9_04Noroi ||
                   id == CardId.K9_17Ripper ||
                   id == CardId.K9_66XJacks ||
                   id == CardId.K9_00Hound ||
                   id == CardId.K9_XWerewolf;
        }

        private bool LupisEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                bool hasK9 = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsK9Card(c.Id));
                if (hasK9 && !IsSpecialSummonBlocked())
                {
                    DecisionTracer.TraceActivate("LupisHand", "Special Summoning Lupis from hand");
                    return true;
                }
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                if (_lupisUsed) return false;
                var target = Bot.GetMonsters().FirstOrDefault(c => c != null && c != Card && c.IsFaceup() && IsK9Card(c.Id));
                if (target != null)
                {
                    AI.SelectCard(target);
                    _lupisUsed = true;
                    DecisionTracer.TraceActivate("LupisField", $"Modulating level of {target.Name}");
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
                    DecisionTracer.TraceActivate("NumberC104Umbral", $"Negating monster effect of {target.Name} and halving LP");
                    return true;
                }
            }
            return false;
        }

        private bool CXyzNaschKnightEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
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

        private bool VallonSuperPsyEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup());
                if (target != null)
                {
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("VallonSuperPsy", $"Activating Vallon Super Psy effect on {target.Name}");
                    return true;
                }
            }
            return false;
        }

        private bool InfinitrackRiverStormerEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                DecisionTracer.TraceActivate("InfinitrackRiverStormer", "Activating River Stormer search effect");
                return true;
            }
            return false;
        }

        private bool ImperialPrincessQuinqueryEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                bool necrovalley = Bot.HasInSpellZone(CardId.Necrovalley, faceUp: true) || Enemy.HasInSpellZone(CardId.Necrovalley, faceUp: true);
                var target = Bot.Hand.FirstOrDefault(c => c != null && c.Level == 5 && c.IsMonster());
                if (target == null && !necrovalley)
                {
                    target = Bot.Graveyard.FirstOrDefault(c => c != null && c.Level == 5 && c.IsMonster());
                }

                if (target != null && !IsSpecialSummonBlocked())
                {
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("ImperialPrincessQuinquery", $"Reviving/summoning {target.Name}");
                    return true;
                }
            }
            return false;
        }

        private bool K9SpSummon()
        {
            bool cond = !IsSpecialSummonBlocked();
            if (!cond)
                DecisionTracer.TraceSkip("K9SpSummon", "Special Summon blocked by floodgate");
            return cond;
        }

        private bool NormalSummonCommandant()
        {
            bool cond = Bot.HasInSpellZone(CardId.Necrovalley, faceUp: true) && Bot.Hand.Count > 1;
            if (cond)
                DecisionTracer.TraceActivate("NormalSummonCommandant", "Normal Summoning Commandant under Necrovalley");
            return cond;
        }

        private bool ZeusSummon()
        {
            bool cond = !IsSpecialSummonBlocked() && Duel.Phase == DuelPhase.Main2 && Bot.GetMonsters().Any(c => c != null && c.HasType(CardType.Xyz) && c.Attacked);
            if (cond)
                DecisionTracer.TraceActivate("ZeusSummon", "Summoning AA-ZEUS in Main Phase 2");
            return cond;
        }

        private bool DarkRenewalEffect()
        {
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup());
            if (target != null)
            {
                var send = Bot.GetMonsters().FirstOrDefault(c => c != null && c.Race == (int)CardRace.SpellCaster);
                if (send != null)
                {
                    AI.SelectCard(send);
                    AI.SelectNextCard(target);
                    DecisionTracer.TraceActivate("DarkRenewal", $"Sending {send.Name} to take down {target.Name}");
                    return true;
                }
            }
            return false;
        }

        private bool DimensionalBarrierEffect()
        {
            AI.SelectOption(0); // Declare Fusion
            DecisionTracer.TraceActivate("DimensionalBarrier", "Activating Dimensional Barrier declaring Fusion");
            return true;
        }

        private bool IllusionGateEffect()
        {
            if (ShouldSkipCombo()) return false;
            // Illusion Gate: Pay half LP, destroy ALL monsters your opponent controls,
            // then optionally Special Summon 1 monster from opponent's GY.
            // (No targeting at activation — destruction is not targetted.)

            if (Bot.LifePoints < 2000)
            {
                DecisionTracer.TraceSkip("IllusionGate", "LP too low to safely pay half");
                return false;
            }

            if (Enemy.GetMonsterCount() == 0)
            {
                DecisionTracer.TraceSkip("IllusionGate", "No opponent monsters to destroy");
                return false;
            }

            // The effect doesn't target at activation — it destroys all and optionally summons from opponent GY
            // The GY summon prompt (if any) will be handled by the default prompt handlers.
            DecisionTracer.TraceActivate("IllusionGate", "Destroying all opponent monsters");
            return true;
        }

        private bool ChaoticElementsEffect()
        {
            // Chaotic Elements: Add 1 Level 5+ LIGHT/DARK Pyro or Aqua from Deck or GY to hand.
            // If 3+ Pyro/Aqua in GY, can also destroy 1 card on field.

            // Check for valid targets in Deck (use StartingDeck + Bot.GetRemainingCount)
            bool hasDeckTarget = false;
            foreach (int deckId in StartingDeck.Cards)
            {
                if (deckId == CardId.GravekeeperCommandant) continue; // skip Commandant
                int remaining = Bot.GetRemainingCount(deckId, 3);
                if (remaining > 0 && !Bot.Hand.Any(c => c != null && c.Id == deckId))
                {
                    hasDeckTarget = true;
                    break;
                }
            }

            bool hasGyTarget = Bot.Graveyard.Any(c => c != null && c.Level >= 5 && c.IsMonster() &&
                (c.Attribute == (int)CardAttribute.Light || c.Attribute == (int)CardAttribute.Dark) &&
                (c.Race == (int)CardRace.Pyro || c.Race == (int)CardRace.Aqua));

            if (!hasDeckTarget && !hasGyTarget)
            {
                DecisionTracer.TraceSkip("ChaoticElements", "No valid Level 5+ LIGHT/DARK Pyro/Aqua target in Deck or GY");
                return false;
            }

            // Prefer Deck targets over GY targets
            if (hasDeckTarget)
            {
                // If deck has targets and GY has 3+ Pyro/Aqua, we can also destroy a card
                int pyroAquaInGY = Bot.Graveyard.Count(c => c != null && c.IsMonster() &&
                    (c.Race == (int)CardRace.Pyro || c.Race == (int)CardRace.Aqua));
                if (pyroAquaInGY >= 3 && Enemy.GetMonsters().Any(c => c != null && c.IsFaceup()))
                {
                    AI.SelectOption(1); // Option to destroy
                    var destroyTarget = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup());
                    if (destroyTarget != null)
                    {
                        AI.SelectNextCard(destroyTarget);
                        DecisionTracer.TraceActivate("ChaoticElements", $"Adding from Deck, then destroying {destroyTarget.Name}");
                        return true;
                    }
                }
                DecisionTracer.TraceActivate("ChaoticElements", "Adding Level 5+ LIGHT/DARK Pyro/Aqua from Deck");
                return true;
            }

            DecisionTracer.TraceActivate("ChaoticElements", "Adding Level 5+ LIGHT/DARK Pyro/Aqua from GY");
            return true;
        }

        private bool NoroiFieldEffect()
        {
            // Noroi Ignition Effect (Main Phase only): Tribute 1 face-up K9 monster to look at opponent's hand
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;

            // Need a face-up K9 monster on field (other than Noroi itself) to tribute
            var tributeTarget = Bot.GetMonsters().FirstOrDefault(c => c != null && c != Card && c.IsFaceup() && IsK9Card(c.Id));
            if (tributeTarget == null)
            {
                DecisionTracer.TraceSkip("NoroiField", "No face-up K9 monster to tribute");
                return false;
            }

            DecisionTracer.TraceActivate("NoroiField", $"Tributing {tributeTarget.Name} to look at opponent's hand");
            return true;
        }

        private bool ForcedReleaseEffect()
        {
            // K9-X Forced Release: Target 1 face-up "K9" Xyz monster you control;
            // Special Summon 1 different "K9" Xyz monster from ED using target as material.
            // Then optionally destroy 1 card opponent controls.

            // Check we have a face-up K9 Xyz monster on field
            ClientCard target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && IsK9Card(c.Id) && c.HasType(CardType.Xyz));
            if (target == null)
            {
                DecisionTracer.TraceSkip("ForcedRelease", "No face-up K9 Xyz monster to target");
                return false;
            }

            // Check we have a different K9 Xyz in Extra Deck to summon
            bool hasDifferentK9Xyz = false;
            foreach (int deckId in StartingDeck.ExtraCards)
            {
                if (IsK9Card(deckId) && deckId != target.Id && Bot.GetRemainingCount(deckId, 3) > 0)
                {
                    hasDifferentK9Xyz = true;
                    break;
                }
            }
            if (!hasDifferentK9Xyz)
            {
                DecisionTracer.TraceSkip("ForcedRelease", "No different K9 Xyz in Extra Deck to swap into");
                return false;
            }

            AI.SelectCard(target);

            // Optionally destroy 1 opponent card if we can
            var opponentTarget = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup())
                ?? Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());
            if (opponentTarget != null)
            {
                AI.SelectNextCard(opponentTarget);
                DecisionTracer.TraceActivate("ForcedRelease", $"Swapping {target.Name} to different K9 Xyz, destroying {opponentTarget.Name}");
            }
            else
            {
                DecisionTracer.TraceActivate("ForcedRelease", $"Swapping {target.Name} to different K9 Xyz");
            }
            return true;
        }

        private bool SpellSetFiltered()
        {
            if (Card.IsCode(CardId.DimensionalBarrier, CardId.DarkRenewal, CardId.Necrovalley, CardId.K9XForcedRelease))
            {
                bool cond = SetBackrowCondition();
                if (cond)
                    DecisionTracer.TraceActivate("SpellSetFiltered", $"Setting {Card.Name}");
                return cond;
            }
            return false;
        }

        private bool SetBackrowCondition() => Util.IsTurn1OrMain2();
    }
}
