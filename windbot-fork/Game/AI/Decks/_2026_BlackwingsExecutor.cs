// ============================================================
// CARD AUDIT โ€” 2026_Blackwings
// | Card Name           | Type    | OPT? | Cost      | Effect              | Activate When        | NEVER When           |
// |---------------------|---------|------|-----------|---------------------|----------------------|----------------------|
// | Simoon the Poison   | Monster | HOPT | Banish BW | Place Whirlwind+NS  | Hand starter         | Already normal sum.  |
// | Sudri the Phantom   | Monster | HOPT | None      | Search Blackwing    | Normal summon        | SS blocked           |
// | Vata the Emblem     | Monster | HOPT | None      | Send materials deck | Hand extender        | BWD not in Extra Deck|
// | Zephyros the Elite  | Monster | Duel | Bounce +40| SS self from GY     | GY extender          | No face-up card      |
// | Black Whirlwind     | Spell   | None | None      | Search BW on Normal | NS of Blackwing      | Hand size too large  |
// | Black-Winged Assault| Monster | None | Banish 2  | Burn / negate       | Banish from field/GY | SS blocked           |
// ACE CARDS: Primary: Black-Winged Assault Dragon / Secondary: Bystial Dis Pater, Blackwing Full Armor Master
// COMBO STARTERS: 1. Blackwing - Simoon the Poison Wind 2. Blackwing - Sudri the Phantom Glimmer 3. Black Whirlwind
// CHOKEPOINTS: Ash Blossom on Simoon or Sudri search
// WIN CONDITION: Summon multiple copies of Black-Winged Assault Dragon and Bystial Dis Pater to control the field with negates and burn.
// GOING 1ST END BOARD: Black-Winged Assault Dragon x2 + Bystial Dis Pater + Hot Red Dragon Archfiend Abyss
// GOING 2ND GAMEPLAN: Clear board using Bystial banish and Synchro summons, then attack for lethal.
// ============================================================

// ============================================================
// COMBO DRAFT โ€” 2026_Blackwings
// ============================================================
// === COMBO LINE 1: Simoon Starter (Starter: Simoon) ===
// HAND REQUIRED: Simoon the Poison Wind + any Blackwing
// STEP 1: Activate Simoon hand effect, banish the other Blackwing.
//   โ’ Effect: Place Black Whirlwind from deck to field, and Normal Summon Simoon.
// STEP 2: Black Whirlwind triggers: Search Blackwing - Sudri the Phantom Glimmer.
// STEP 3: Normal Summon Sudri using the extra summon.
//   โ’ Sudri triggers: Search Blackwing - Vata the Emblem of Wandering.
//   โ’ Black Whirlwind triggers: Search Blackwing - Shamal the Sandstorm.
// STEP 4: Vata effect: Special Summon itself from hand.
// STEP 5: Vata effect: Send Vata + Level 6 Blackwing from deck to GY โ’ SS Black-Winged Dragon.
// STEP 6: Banish Black-Winged Dragon + Boreastorm (Tuner) from GY โ’ SS Black-Winged Assault Dragon.
// END BOARD: Black-Winged Assault Dragon (burn + pop)
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
    [Deck("2026_Blackwings", "2026_Blackwings")]
    public class _2026_BlackwingsExecutor : ModernExecutor
    {
        public class CardId
        {
            // Blackwings Main Deck
            public const int BWSimoon = 81470373;
            public const int BWSudri = 70465810;
            public const int BWVata = 71187462;
            public const int BWZephyros = 14785765;
            public const int BWShamal = 8571567;
            public const int BWSharnga = 54594017;
            public const int BWChinook = 34976176;
            public const int BWHarmattan = 77152542;
            public const int BWOroshi = 73652465;
            public const int BWKunai = 70456282;
            public const int BWZonda = 7459919;

            // Bystials & Extenders
            public const int AssaultSynchron = 77202120;
            public const int BystialLubellion = 32731036;
            public const int BystialDruiswurm = 6637331;
            public const int BystialMagnamhut = 33854624;
            public const int BystialBaldrake = 72656408;

            // Spells & Traps
            public const int BlackWhirlwind = 91351370;
            public const int BlackFeatherWhirlwind = 7602800;
            public const int BrandedRegained = 34090915;
            public const int SmallWorld = 89558743;
            public const int PotOfProsperity = 84211599;
            public const int CalledByTheGrave = 24224830;
            public const int BWTwinShadow = 69973414;

            // Hand Traps
            public const int AshBlossom = 14558127;
            public const int DrollAndLockBird = 94145021;
            public const int InfiniteImpermanence = 10045474;

            // Extra Deck
            public const int BWAssaultDragon = 73218989;
            public const int BlackWingedDragon = 9012916;
            public const int BWFullArmorMaster = 54082269;
            public const int BWBoreastorm = 10602628;
            public const int BWNothung = 95040215;
            public const int RaidraptorWiseStrix = 36429703;
            public const int SPLittleKnight = 29301450;
            public const int ZalenShackledDragon = 4891376;
            public const int DracoBerserker = 5041348;
            public const int HotRedDragonAbyss = 9753964;
            public const int ChaosAngel = 22850702;
            public const int BystialDisPater = 27572350;
        }

        private static readonly int[] AceCardIds = {
            CardId.BWAssaultDragon,
            CardId.BystialDisPater,
            CardId.BWFullArmorMaster,
            CardId.HotRedDragonAbyss,
            CardId.ChaosAngel,
            CardId.SPLittleKnight,
            CardId.DracoBerserker
        };

        private bool _simoonUsed = false;
        private bool _sudriUsed = false;
        private bool _vataUsed = false;
        private bool _shamalUsed = false;
        private bool _twinShadowUsed = false;
        private bool _zephyrosUsed = false;
        private bool _prosperityUsed = false;
        private bool _smallWorldUsed = false;

        private enum ComboPhase
        {
            Init,
            Searching,
            Summoning,
            Synchroing,
            Finalizing,
            BoardComplete,
            OTK
        }
        private ComboPhase _currentPhase = ComboPhase.Init;

        private static readonly int[] ExtraDeckExtortion = {
            87602890,   // Zaborg, The Mega Monarch
            95679145,   // Maximus Dragma
            82734805,   // Infernoid Tierra
            86062400,   // Xyz Avenger
            63737050,   // Ryu Okami
        };

        private static readonly int[] BoardWipes = {
            12580477,   // Raigeki
            53129443,   // Dark Hole
            14532163,   // Lightning Storm
            99330325,   // Interrupted Kaiju Slumber
            15693423,   // Evenly Matched
        };

        public _2026_BlackwingsExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            HeuristicGuard.RegisterAceCards(AceCardIds);

            // Combo Router setup
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Simoon-Whirlwind-Combo",
                RequiredCards = new List<int> { CardId.BWSimoon },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.BWSimoon, ActionType = ExecutorType.Activate, Description = "Simoon banish and place Whirlwind" }
                },
                EndBoardScore = 95
            });

            BaitPlanner.RegisterComboStarters(CardId.BWSimoon, CardId.BWSudri, CardId.BlackWhirlwind);
            BaitPlanner.RegisterBaitCards(CardId.PotOfProsperity);

            // ===== Priority 1: Hand Traps & Staples =====
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, DefaultInfiniteImpermanence);
            AddExecutor(ExecutorType.Activate, CardId.BWChinook, ChinookEffect);

            // ===== Priority 2: Boss Quick Effects =====
            AddExecutor(ExecutorType.Activate, CardId.BWAssaultDragon, AssaultDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.BWFullArmorMaster, FullArmorMasterEffect);
            AddExecutor(ExecutorType.Activate, CardId.HotRedDragonAbyss, HotRedAbyssEffect);
            AddExecutor(ExecutorType.Activate, CardId.ChaosAngel, ChaosAngelEffect);
            AddExecutor(ExecutorType.Activate, CardId.BystialDisPater, DisPaterEffect);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, LittleKnightEffect);
            AddExecutor(ExecutorType.Activate, CardId.BWSharnga, SharngaEffect);

            // ===== Priority 3: Search Spells & Field Setup =====
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, ProsperityEffect);
            AddExecutor(ExecutorType.Activate, CardId.SmallWorld, SmallWorldEffect);
            AddExecutor(ExecutorType.Activate, CardId.BlackWhirlwind, BlackWhirlwindEffect);
            AddExecutor(ExecutorType.Activate, CardId.BlackFeatherWhirlwind, BlackFeatherWhirlwindEffect);
            AddExecutor(ExecutorType.Activate, CardId.BrandedRegained, BrandedRegainedEffect);

            // ===== Priority 4: Main combos & Summons =====
            AddExecutor(ExecutorType.Activate, CardId.BWSimoon, SimoonEffect);
            AddExecutor(ExecutorType.Summon, CardId.BWSudri, SudriSummon);
            AddExecutor(ExecutorType.Activate, CardId.BWSudri, SudriEffect);
            AddExecutor(ExecutorType.Activate, CardId.BWShamal, ShamalEffect);
            AddExecutor(ExecutorType.Summon, CardId.BWSimoon, SimoonNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.BWShamal, FallbackNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.BWChinook, FallbackNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.BWSharnga, FallbackNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.BWHarmattan, FallbackNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.BWOroshi, FallbackNormalSummon);

            // ===== Priority 5: Tuners & Extenders =====
            AddExecutor(ExecutorType.SpSummon, CardId.BWVata, VataSummon);
            AddExecutor(ExecutorType.Activate, CardId.BWVata, VataEffect);
            AddExecutor(ExecutorType.Activate, CardId.BWZephyros, ZephyrosEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.BWHarmattan, HarmattanSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.BWOroshi, OroshiSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AssaultSynchron, AssaultSynchronSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.BWSharnga, SharngaSummon);

            // ===== Priority 6: Bystials plays =====
            AddExecutor(ExecutorType.Activate, CardId.BystialLubellion, LubellionEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.BystialMagnamhut, BystialSummon);
            AddExecutor(ExecutorType.Activate, CardId.BystialMagnamhut, BystialMagnamhutEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.BystialDruiswurm, BystialSummon);
            AddExecutor(ExecutorType.Activate, CardId.BystialDruiswurm, BystialDruiswurmEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.BystialBaldrake, BystialSummon);
            AddExecutor(ExecutorType.Activate, CardId.BystialBaldrake, BystialBaldrakeEffect);

            // ===== Priority 7: Extra Deck Synchro summons =====
            AddExecutor(ExecutorType.SpSummon, CardId.BWBoreastorm, BoreastormSummon);
            AddExecutor(ExecutorType.Activate, CardId.BWBoreastorm, BoreastormEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.BWNothung, NothungSummon);
            AddExecutor(ExecutorType.Activate, CardId.BWNothung, NothungEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.BlackWingedDragon, BlackWingedDragonSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.BWAssaultDragon, AssaultDragonSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.BystialDisPater, DisPaterSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.BWFullArmorMaster, FullArmorSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.HotRedDragonAbyss, HotRedSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ChaosAngel);
            AddExecutor(ExecutorType.SpSummon, CardId.DracoBerserker);
            AddExecutor(ExecutorType.SpSummon, CardId.RaidraptorWiseStrix, WiseStrixSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, LinkSummonCheck);

            // ===== Priority 8: Trap cards & Repos =====
            AddExecutor(ExecutorType.Activate, CardId.BWTwinShadow, TwinShadowEffect);
            AddExecutor(ExecutorType.SpellSet, CardId.BWTwinShadow);
            AddExecutor(ExecutorType.SpellSet, SpellSetFiltered);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _simoonUsed = false;
            _sudriUsed = false;
            _vataUsed = false;
            _shamalUsed = false;
            _twinShadowUsed = false;
            _prosperityUsed = false;
            _smallWorldUsed = false;
            
            _currentPhase = ComboPhase.Init;
            if (_isGoingSecond && Bot.GetMonsterCount() == 0)
                _currentPhase = ComboPhase.OTK;
            // Zephyros is once per duel โ€” don't reset

            // โ”€โ”€ Going-Second BreakBoard: prioritize disruption over combo โ”€โ”€
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
            }
        }

        public override bool OnSelectHand()
        {
            return true; // Go first to set up Assault Dragon + negation board
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Contains(card.Id);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c))
            {
                if (c.Location == CardLocation.MonsterZone && c.IsFaceup())
                    return int.MaxValue;
                return 900;
            }
            if (c.IsCode(CardId.AshBlossom, CardId.DrollAndLockBird))
                return 800;
            // Tuners are moderately valuable
            if (c.IsTuner()) return 500;
            return 100;
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // โ”€โ”€ CASE 1: Extra Deck Mill Protection (Zaborg, Maximus Dragma) โ”€โ”€
            if (Util.GetLastChainCard() != null && Util.GetLastChainCard().IsCode(ExtraDeckExtortion))
            {
                return SelectExtraDeckToMill(cards, min, max);
            }

            // โ”€โ”€ CASE 2: Evenly Matched Protection โ”€โ”€
            if (Util.GetLastChainCard() != null && Util.GetLastChainCard().IsCode(15693423) && Duel.LastChainPlayer != 0)
            {
                var keepIds = new HashSet<int> { 
                    CardId.BWAssaultDragon,
                    CardId.BystialDisPater,
                    CardId.BWFullArmorMaster,
                    CardId.HotRedDragonAbyss,
                    CardId.ChaosAngel
                };
                var result = cards.Where(c => c != null && !keepIds.Contains(c.Id)).Take(max).ToList();
                if (result.Count >= min)
                    return Util.CheckSelectCount(result, cards, min, max);
            }

            // โ”€โ”€ CASE 3: Small World Selection โ”€โ”€
            if (Util.GetLastChainCard() != null && Util.GetLastChainCard().Id == CardId.SmallWorld)
            {
                if (cards.All(c => c != null && c.Location == CardLocation.Hand))
                {
                    var target = cards.FirstOrDefault(c => c.Id == CardId.BWSharnga)
                        ?? cards.FirstOrDefault(c => c.Id == CardId.BWChinook)
                        ?? cards.FirstOrDefault(c => c.Id == CardId.BystialBaldrake)
                        ?? cards.FirstOrDefault(c => c.Id == CardId.BWZephyros)
                        ?? cards.FirstOrDefault(c => c.Id == CardId.BWVata)
                        ?? cards.FirstOrDefault(c => c.Id != CardId.BWSimoon && c.Id != CardId.BWSudri && c.Id != CardId.AshBlossom && c.Id != CardId.DrollAndLockBird)
                        ?? cards.FirstOrDefault();
                    if (target != null) return new List<ClientCard> { target };
                }
                else if (cards.All(c => c != null && c.Location == CardLocation.Deck))
                {
                    if (hint == 506) // Step 3: Add to hand!
                    {
                        var target = cards.FirstOrDefault(c => c.Id == CardId.BWSimoon)
                            ?? cards.FirstOrDefault(c => c.Id == CardId.BWSudri)
                            ?? cards.FirstOrDefault(c => c.Id == CardId.BWVata)
                            ?? cards.FirstOrDefault(c => c.Id == CardId.BWShamal)
                            ?? cards.FirstOrDefault();
                        if (target != null) return new List<ClientCard> { target };
                    }
                    else // Step 2: Choose the bridge (hint == 502 / other)!
                    {
                        var target = cards.FirstOrDefault(c => c.Id == CardId.BystialDruiswurm)
                            ?? cards.FirstOrDefault(c => c.Id == CardId.BystialMagnamhut)
                            ?? cards.FirstOrDefault(c => c.Id == CardId.BystialBaldrake)
                            ?? cards.FirstOrDefault(c => c.Id == CardId.AshBlossom)
                            ?? cards.FirstOrDefault(c => c.Id == CardId.BWShamal)
                            ?? cards.FirstOrDefault();
                        if (target != null) return new List<ClientCard> { target };
                    }
                }
            }

            // โ”€โ”€ CASE 4: Vata Deck Send Selection โ”€โ”€
            if (Util.GetLastChainCard() != null && Util.GetLastChainCard().Id == CardId.BWVata)
            {
                var selected = cards.Where(c => c != null && c.Selected).ToList();
                int selectedSum = selected.Sum(c => c.Level);
                
                if (selectedSum == 8)
                {
                    return new List<ClientCard>(); // Finish selection
                }
                
                var zonda = cards.FirstOrDefault(c => c != null && c.Id == CardId.BWZonda);
                var twoCard = cards.FirstOrDefault(c => c != null && c.Id == CardId.BWChinook)
                    ?? cards.FirstOrDefault(c => c != null && c.Id == CardId.BWHarmattan);
                    
                if (zonda != null && twoCard != null)
                {
                    if (!zonda.Selected)
                        return new List<ClientCard> { zonda };
                    if (!twoCard.Selected)
                        return new List<ClientCard> { twoCard };
                }
                
                var shamals = cards.Where(c => c != null && c.Id == CardId.BWShamal).ToList();
                if (shamals.Count >= 2)
                {
                    var unselectedShamal = shamals.FirstOrDefault(c => !c.Selected);
                    if (unselectedShamal != null)
                        return new List<ClientCard> { unselectedShamal };
                }
                
                var shamal = cards.FirstOrDefault(c => c != null && c.Id == CardId.BWShamal);
                var sudri = cards.FirstOrDefault(c => c != null && c.Id == CardId.BWSudri);
                if (shamal != null && sudri != null)
                {
                    if (!shamal.Selected)
                        return new List<ClientCard> { shamal };
                    if (!sudri.Selected)
                        return new List<ClientCard> { sudri };
                }
            }

            // โ”€โ”€ CASE 5: Search from Deck โ”€โ”€
            if (hint == 509)
            {
                var deckCards = cards.Where(c => c != null && c.Location == CardLocation.Deck).ToList();
                if (deckCards.Count > 0 && cards.Any(c => c.Location != CardLocation.Deck))
                    return Util.CheckSelectCount(deckCards, cards, min, max);
            }

            // โ”€โ”€ CASE 6: Material selection โ”€โ”€
            if (hint == 511 || hint == 512 || hint == 513 || hint == 533)
            {
                var sorted = cards.Where(c => c != null)
                    .OrderBy(c => GetMaterialPriority(c))
                    .ToList();
                return Util.CheckSelectCount(sorted, cards, min, max);
            }

            // โ”€โ”€ CASE 7: Discard / Removal โ”€โ”€
            if (hint == 501 || hint == 502 || hint == 503 || hint == 504 || hint == 505 || hint == 508)
            {
                var sorted = cards.Where(c => c != null)
                    .OrderBy(c => GetMaterialPriority(c))
                    .ToList();
                return Util.CheckSelectCount(sorted, cards, min, max);
            }

            // โ”€โ”€ CASE 8: Banish Selection (Bystials / Called by / etc.) โ”€โ”€
            if (hint == 502 || hint == 517 || (Util.GetLastChainCard() != null && 
                (Util.GetLastChainCard().Id == CardId.BystialDruiswurm || 
                 Util.GetLastChainCard().Id == CardId.BystialMagnamhut || 
                 Util.GetLastChainCard().Id == CardId.BystialBaldrake ||
                 Util.GetLastChainCard().Id == CardId.CalledByTheGrave)))
            {
                var opponentCards = cards.Where(c => c != null && c.Controller == 1)
                    .OrderBy(c => GetBanishPriority(c))
                    .ToList();
                if (opponentCards.Count > 0)
                {
                    return Util.CheckSelectCount(opponentCards, cards, min, max);
                }
                
                var ownCards = cards.Where(c => c != null && c.Controller == 0)
                    .OrderBy(c => GetBanishPriority(c))
                    .ToList();
                if (ownCards.Count > 0)
                {
                    return Util.CheckSelectCount(ownCards, cards, min, max);
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        private IList<ClientCard> SelectExtraDeckToMill(IList<ClientCard> cards, int min, int max)
        {
            var preferred = new List<int> {
                CardId.RaidraptorWiseStrix,
                CardId.BlackWingedDragon,
                CardId.BWNothung,
                CardId.BWBoreastorm,
                CardId.DracoBerserker,
                CardId.ZalenShackledDragon
            };
            
            var result = new List<ClientCard>();
            foreach (int id in preferred)
            {
                var match = cards.FirstOrDefault(c => c != null && c.Id == id);
                if (match != null)
                {
                    result.Add(match);
                    if (result.Count >= max) break;
                }
            }
            
            if (result.Count < min)
            {
                var remaining = cards.Where(c => !result.Contains(c))
                    .OrderBy(c => GetMaterialPriority(c))
                    .ToList();
                foreach (var c in remaining)
                {
                    result.Add(c);
                    if (result.Count >= max) break;
                }
            }
            
            return Util.CheckSelectCount(result, cards, min, max);
        }

        public override IList<ClientCard> OnSelectXyzMaterial(IList<ClientCard> cards, int min, int max)
        {
            var sorted = cards.Where(c => c != null)
                .OrderBy(c => GetMaterialPriority(c))
                .ToList();
            return Util.CheckSelectCount(sorted, cards, min, max);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (AceCardIds.Contains(cardId))
                return CardPosition.FaceUpAttack;

            if (cardId == CardId.BWSudri || cardId == CardId.BWVata || cardId == CardId.BWShamal || cardId == CardId.BWZephyros || cardId == CardId.BWHarmattan || cardId == CardId.BWOroshi)
            {
                if (positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence;
            }
            return base.OnSelectPosition(cardId, positions);
        }

        public override bool OnPreBattleBetween(ClientCard attacker, ClientCard defender)
        {
            if (attacker != null && !IsAceCard(attacker) && attacker.Attack < 2000)
            {
                if (defender != null && attacker.Attack <= defender.GetDefensePower())
                    return false;
            }
            return base.OnPreBattleBetween(attacker, defender);
        }

        private bool LinkSummonCheck()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldAvoidGenericExtraDeckSummon(2)) return false;
            // Only summon S:P Little Knight if there is an opponent target to banish, or we're not on turn 1
            return Bot.MonsterZone.Count(c => c != null && c.IsFaceup()) >= 2 &&
                   (Enemy.MonsterZone.Count(c => c != null && c.IsFaceup()) > 0 || Duel.Turn > 1);
        }

        private bool WiseStrixSummonCheck()
        {
            if (IsSpecialSummonBlocked()) return false;
            // Only summon Wise Strix if we have at least 3 monsters and at least 1 tuner on field
            bool hasTuner = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsTuner());
            return Bot.MonsterZone.Count(c => c != null && c.IsFaceup()) >= 3 && hasTuner;
        }

        private bool SpellSetFiltered()
        {
            if (Card == null) return false;
            if (!Card.IsTrap() && Card.Id != CardId.CalledByTheGrave) return false;
            return Duel.Phase == DuelPhase.Main2 || !Main.CanBattlePhase;
        }

        // --- Staples & Hand Traps ---
        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain(
                CardId.BWSudri,                          // Blackwings chokepoint
                CardId.BWSimoon,                         // Blackwings chokepoint
                8240199,                                 // Blue-Eyes Sage
                48800175,                                // Blue-Eyes Melody
                24382602,                                // Blue-Eyes Mausoleum
                41620959,                                // Dragon Shrine
                39701395,                                // Cards of Consonance
                35261759,                                // Pot of Desires
                38120068,                                // Trade-In
                84211599,                                // Pot of Prosperity
                35269904,                                // Pot of Extravagance
                71039903,                                // The White Stone of Ancients
                79814787,                                // The White Stone of Legend
                53143898,                                // Altergeist Marionetter
                42790071,                                // Altergeist Multifaker
                53936268,                                // Altergeist Spoofing
                66399653,                                // ABC Union Hangar
                89917387,                                // ABC Union Driver
                04367828,                                // Branded Fusion
                73628505                                 // Terraforming
            )) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool DrollEffect()
        {
            return Duel.LastChainPlayer == 1;
        }

        private bool CalledByTheGraveEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                ClientCard lastChain = Util.GetLastChainCard();
                if (lastChain == null) return false;
                
                if (lastChain.Location == CardLocation.Grave || lastChain.Location == CardLocation.Hand)
                {
                    AI.SelectCard(lastChain);
                    return true;
                }
                
                if (lastChain.Location == CardLocation.MonsterZone)
                {
                    ClientCard target = Enemy.Graveyard.FirstOrDefault(c => c.Id == lastChain.Id);
                    if (target != null)
                    {
                        AI.SelectCard(target);
                        return true;
                    }
                }
            }
            
            if (Duel.LastChainPlayer != 1)
            {
                ClientCard target = Enemy.Graveyard.FirstOrDefault(c => c.Id == 71039903); // The White Stone of Ancients
                if (target == null)
                    target = Enemy.Graveyard.FirstOrDefault(c => c.Id == 38517737); // Alternative
                if (target == null)
                    target = Enemy.Graveyard.FirstOrDefault(c => c.Id == 89631139); // Blue-Eyes White Dragon
                    
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            
            return false;
        }

        // --- Boss Quick Effects ---
        private bool FullArmorMasterEffect()
        {
            // Place Wedge Counters or negate
            if (Duel.LastChainPlayer == 1)
                return true;
            // Also use proactively to place counters
            if (Enemy.GetMonsterCount() > 0 && Duel.Player == 0)
            {
                ClientCard target = Enemy.MonsterZone
                    .Where(c => c != null && c.IsFaceup())
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

        private bool HotRedAbyssEffect()
        {
            // Negate Spell/Trap activation
            if (Duel.LastChainPlayer == 1)
            {
                ClientCard lastChain = Util.GetLastChainCard();
                if (lastChain != null && lastChain.Controller == 1 && (lastChain.IsSpell() || lastChain.IsTrap()))
                    return true;
            }
            return false;
        }

        private bool ChaosAngelEffect()
        {
            // Banish 1 face-up card
            if (Duel.LastChainPlayer == 1 || Enemy.GetMonsterCount() > 0)
            {
                ClientCard target = Enemy.MonsterZone
                    .Where(c => c != null && c.IsFaceup())
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

        private bool LittleKnightEffect()
        {
            ClientCard target = Enemy.MonsterZone
                .Where(c => c != null && c.IsFaceup())
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();
            if (target == null)
                target = Enemy.SpellZone.FirstOrDefault(c => c != null && c.IsFaceup());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // --- Search Spells ---
        private bool ProsperityEffect()
        {
            if (_prosperityUsed) return false;
            _prosperityUsed = true;
            return true;
        }

        private bool SmallWorldEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_smallWorldUsed) return false;
            _smallWorldUsed = true;
            return true;
        }

        private bool BlackWhirlwindEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.Hand && Bot.HasInSpellZone(CardId.BlackWhirlwind))
                return false;
            return true;
        }

        private bool BlackFeatherWhirlwindEffect()
        {
            if (Card.Location == CardLocation.Hand && Bot.HasInSpellZone(CardId.BlackFeatherWhirlwind))
                return false;
            return true;
        }

        private bool BrandedRegainedEffect()
        {
            if (Card.Location == CardLocation.Hand && Bot.HasInSpellZone(CardId.BrandedRegained))
                return false;
            return true;
        }

        // --- Blackwing Actions ---
        private bool SimoonEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_simoonUsed) return false;
            if (Card.Location == CardLocation.Hand)
            {
                // Banish 1 BW (any Winged Beast) to place Black Whirlwind and Normal Summon
                // Prefer banishing lower-value BW
                ClientCard cost = Bot.Hand
                    .Where(c => c != Card && c.IsMonster() && c.Race == (int)CardRace.WindBeast)
                    .OrderBy(c => c.Attack)
                    .FirstOrDefault();
                if (cost != null)
                {
                    _simoonUsed = true;
                    AI.SelectCard(cost);
                    return true;
                }
            }
            return false;
        }

        private bool SimoonNormalSummon()
        {
            // Fallback normal summon if Simoon effect wasn't used
            if (_simoonUsed) return false;
            return !Bot.MonsterZone.Any(c => c != null && c.IsFaceup());
        }

        private bool SudriSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool SudriEffect()
        {
            if (_sudriUsed) return false;
            _sudriUsed = true;
            // Search priority: Vata for combo extension > Shamal > Harmattan
            if (!Bot.Hand.Any(c => c.Id == CardId.BWVata) && Bot.GetRemainingCount(CardId.BWVata, 3) > 0)
                AI.SelectCard(CardId.BWVata, CardId.BWShamal, CardId.BWHarmattan);
            else
                AI.SelectCard(CardId.BWShamal, CardId.BWHarmattan, CardId.BWVata);
            return true;
        }

        private bool ShamalEffect()
        {
            if (_shamalUsed) return false;
            _shamalUsed = true;
            return true;
        }

        private bool VataSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id != CardId.BWVata);
        }

        private bool VataEffect()
        {
            if (_vataUsed) return false;
            _vataUsed = true;
            // Send Vata + non-tuner to GY, SS Black-Winged Dragon
            AI.SelectCard(CardId.BWChinook, CardId.BWShamal, CardId.BWZonda, CardId.BWSharnga);
            return true;
        }

        private bool ZephyrosEffect()
        {
            if (IsSpecialSummonBlocked() || _zephyrosUsed) return false;
            // Bounce face-up card to SS from GY โ€” prefer bouncing something we can replay
            ClientCard target = Bot.SpellZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.BlackWhirlwind);
            if (target == null)
                target = Bot.SpellZone.FirstOrDefault(c => c != null && c.IsFaceup());
            if (target != null)
            {
                _zephyrosUsed = true;
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // --- Extender Summons ---
        private bool HarmattanSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id != CardId.BWHarmattan);
        }

        private bool OroshiSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id != CardId.BWOroshi);
        }

        private bool AssaultSynchronSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.Graveyard.Any(c => c.IsMonster() && c.IsTuner());
        }

        // --- Bystials ---
        private bool LubellionEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                AI.SelectCard(CardId.BystialMagnamhut, CardId.BystialDruiswurm);
                return true;
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                return true;
            }
            return false;
        }

        private bool BystialSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            bool hasLightDark = Bot.Graveyard.Any(c => c.Attribute == (int)CardAttribute.Light || c.Attribute == (int)CardAttribute.Dark)
                                || Enemy.Graveyard.Any(c => c.Attribute == (int)CardAttribute.Light || c.Attribute == (int)CardAttribute.Dark);
            return hasLightDark;
        }

        private bool BystialMagnamhutEffect()
        {
            // Search Dragon from deck
            AI.SelectCard(CardId.BystialDruiswurm, CardId.BystialLubellion, CardId.BystialBaldrake);
            return true;
        }

        private bool BystialDruiswurmEffect()
        {
            // Send opponent's S/T to GY
            ClientCard target = Enemy.SpellZone
                .Where(c => c != null && c.IsFaceup())
                .FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return true; // Activate even without target for board presence
        }

        private bool BystialBaldrakeEffect()
        {
            // Banish from field
            ClientCard target = Enemy.MonsterZone
                .Where(c => c != null && c.IsFaceup())
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool ChinookEffect()
        {
            // Negate opponent's face-up monster
            ClientCard target = Enemy.MonsterZone
                .Where(c => c != null && c.IsFaceup() && !c.IsDisabled() && 
                            (c.Id == 59822133 || c.Id == 63767246 || c.Attack >= 2500))
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();
                
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool SharngaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            bool has2000 = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Attack >= 2000)
                           || Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Attack >= 2000);
            return has2000;
        }

        private bool SharngaEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                bool has2000 = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Attack >= 2000)
                               || Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Attack >= 2000);
                if (!has2000) return false;
                
                ClientCard target = Enemy.MonsterZone
                    .Where(c => c != null && c.IsFaceup())
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();
                if (target == null)
                    target = Enemy.SpellZone.FirstOrDefault(c => c != null && c.IsFaceup());
                    
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private int GetBanishPriority(ClientCard c)
        {
            if (c == null) return 999;
            
            if (c.Controller == 1)
            {
                if (c.Id == 71039903) return 10;  // The White Stone of Ancients
                if (c.Id == 79814787) return 11;  // The White Stone of Legend
                if (c.Id == 38517737) return 12;  // Blue-Eyes Alternative
                if (c.IsMonster()) return 20;
                return 30;
            }
            else
            {
                if (c.Id == CardId.BWSharnga) return 100;
                if (c.Id == CardId.BWChinook) return 110;
                if (c.Id == CardId.BWHarmattan) return 120;
                if (c.Id == CardId.BWOroshi) return 130;
                if (c.Id == CardId.BWZephyros) return 200;
                if (c.Id == CardId.BWVata) return 210;
                if (c.Id == CardId.BWSudri) return 220;
                if (c.Id == CardId.BWBoreastorm) return 300;
                if (c.Id == CardId.BlackWingedDragon) return 310;
                return 400;
            }
        }

        // --- Synchro & Extra Deck ---
        private bool BoreastormSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsTuner())
                   && Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && !c.IsTuner());
        }

        private bool BoreastormEffect()
        {
            AI.SelectCard(CardId.BWSharnga, CardId.BWOroshi, CardId.BWChinook);
            return true;
        }

        private bool NothungSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsTuner())
                   && Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && !c.IsTuner());
        }

        private bool NothungEffect()
        {
            ClientCard target = Enemy.MonsterZone
                .Where(c => c != null && c.IsFaceup())
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            // 800 burn is always good even without a target
            return true;
        }

        private bool BlackWingedDragonSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsTuner())
                   && Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && !c.IsTuner());
        }

        private bool AssaultDragonSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            bool hasBWD = Bot.Graveyard.Any(c => c.Id == CardId.BlackWingedDragon)
                          || Bot.MonsterZone.Any(c => c != null && c.Id == CardId.BlackWingedDragon);
            bool hasTuner = Bot.Graveyard.Any(c => c.Id == CardId.BWBoreastorm)
                            || Bot.MonsterZone.Any(c => c != null && c.Id == CardId.BWBoreastorm);
            return hasBWD && hasTuner;
        }

        private bool AssaultDragonEffect()
        {
            // Quick effect: burn + pop
            if (Duel.LastChainPlayer == 1)
                return true;
            // Proactive: pop opponent's strongest monster during either turn
            if (Enemy.GetMonsterCount() > 0)
            {
                ClientCard target = Enemy.MonsterZone
                    .Where(c => c != null && c.IsFaceup())
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

        private bool DisPaterSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.MonsterZone.Count(c => c != null && c.IsFaceup()) >= 2;
        }

        private bool DisPaterEffect()
        {
            // Revive banished LIGHT/DARK monster or banish opponent card
            if (Duel.LastChainPlayer == 1)
            {
                // Banish opponent's card
                ClientCard target = Enemy.MonsterZone
                    .Where(c => c != null && c.IsFaceup())
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            // Retrieve banished monster
            ClientCard revive = Bot.Graveyard.FirstOrDefault(c => c.IsMonster() && c.IsCanRevive());
            if (revive != null)
            {
                AI.SelectCard(revive);
                return true;
            }
            return false;
        }

        private bool FullArmorSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsTuner())
                   && Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && !c.IsTuner());
        }

        private bool HotRedSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsTuner())
                   && Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && !c.IsTuner());
        }

        // --- Twin Shadow ---
        private bool TwinShadowEffect()
        {
            if (_twinShadowUsed || IsSpecialSummonBlocked()) return false;
            
            var windBeasts = Bot.Graveyard.Concat(Bot.Banished)
                .Where(c => c != null && c.Race == (int)CardRace.WindBeast)
                .ToList();
                
            bool hasTuner = windBeasts.Any(c => c.IsTuner());
            bool hasNonTuner = windBeasts.Any(c => !c.IsTuner());
            
            if (hasTuner && hasNonTuner)
            {
                _twinShadowUsed = true;
                AI.SelectCard(
                    CardId.BWFullArmorMaster,
                    CardId.BWAssaultDragon,
                    CardId.BlackWingedDragon,
                    CardId.BWBoreastorm,
                    CardId.BWNothung
                );
                return true;
            }
            return false;
        }

        private bool FallbackNormalSummon()
        {
            if (Bot.MonsterZone.Any(c => c != null && c.IsFaceup())) return false;
            return true;
        }

        protected override bool IsBoardStrongEnough()
        {
            bool hasAssaultDragon = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id == CardId.BWAssaultDragon);
            bool hasOtherBoss = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && 
                (c.Id == CardId.BystialDisPater || c.Id == CardId.HotRedDragonAbyss || c.Id == CardId.ChaosAngel || c.Id == CardId.BWFullArmorMaster));
            int backrowCount = Bot.SpellZone.Count(c => c != null && c.IsFacedown());
            
            if (hasAssaultDragon && (hasOtherBoss || backrowCount >= 1))
                return true;
                
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (IsBoardStrongEnough() && _currentPhase == ComboPhase.Synchroing)
            {
                AI?.Log(LogLevel.Info, "[COMBO-STOP] โ“ Board sufficient (Assault Dragon + negate) โ€” stop extending");
                return true;
            }

            int ownSummons = Brain?.OwnSummons ?? 0;
            if (ownSummons >= 5)
            {
                AI?.Log(LogLevel.Info, $"[NIBIRU-CHECK] โ  {ownSummons} summons โ€” stop to avoid Nibiru");
                _currentPhase = ComboPhase.BoardComplete;
                return true;
            }

            return base.ShouldStopExtending();
        }
    }

    [Deck("Expert_2026_Blackwings", "2026_Blackwings")]
    public class ExpertBlackwingsExecutor : _2026_BlackwingsExecutor
    {
        private string _duelId;
        public ExpertBlackwingsExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            _duelId = $"duel_{Guid.NewGuid():N}";
            // [REMOVED-AI-TRAINING] ExpertDataLogger.EnsureInitialized(ExpertDataLogger.FindProjectRoot());
        }
        public override MainPhaseAction OnSelectIdleCmd(MainPhase main)
        {
            var action = base.OnSelectIdleCmd(main);
            // [REMOVED-AI-TRAINING] ExpertDataLogger.LogMainPhaseDecision(main, action, Bot, Enemy, _duelId, Duel.Turn);
            return action;
        }
        public override BattlePhaseAction OnBattle(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            var action = base.OnBattle(attackers, defenders);
            // [REMOVED-AI-TRAINING] ExpertDataLogger.LogBattleDecision(attackers, defenders, action, Bot, Enemy, _duelId, Duel.Turn);
            return action;
        }
    }
}
