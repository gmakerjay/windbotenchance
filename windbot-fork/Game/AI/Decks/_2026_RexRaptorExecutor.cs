// ============================================================
// CARD AUDIT โ€” 2026_RexRaptor
// | Card Name           | Type    | OPT? | Cost      | Effect              | Activate When        | NEVER When           |
// |---------------------|---------|------|-----------|---------------------|----------------------|----------------------|
// | Souleating Oviraptor| Monster | HOPT | None      | Search Dino / pop   | Normal/Special summon| SS blocked           |
// | Miscellaneousaurus  | Monster | HOPT | Discard   | Protect / SS deck   | Main Phase to protect| Non-Main Phase       |
// | Babycerasaurus      | Monster | None | None      | SS level <=4 deck   | Destroyed            | None                 |
// | Petiteranodon       | Monster | None | None      | SS level >=4 deck   | Destroyed            | None                 |
// | Ultimate Conductor  | Monster | None | Banish 2  | Boss / Book / Attack| Break board/OTK      | SS blocked           |
// | Overtex Qoatlus     | Monster | HOPT | Banish 5  | Negate Spell/Trap   | Negate / search Pill | Own card chain       |
// ACE CARDS: Primary: Ultimate Conductor Tyranno / Secondary: Evolzar Laggia, Evolzar Dolkka
// COMBO STARTERS: 1. Souleating Oviraptor 2. Miscellaneousaurus 3. Fossil Dig
// CHOKEPOINTS: Ash Blossom on Oviraptor search
// WIN CONDITION: Summon Ultimate Conductor Tyranno for OTK, or lock opponent with Evolzar Laggia/Dolkka.
// GOING 1ST END BOARD: Evolzar Laggia + Evolzar Dolkka
// GOING 2ND GAMEPLAN: Board break with UCT/Pankratops and OTK.
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
    [Deck("2026_RexRaptor", "2026_RexRaptor")]
    public class _2026_RexRaptorExecutor : ModernExecutor
    {
        public class CardId
        {
            // Dinosaur Main Deck
            public const int SouleatingOviraptor = 44335251;
            public const int Miscellaneousaurus = 38572779;
            public const int Babycerasaurus = 36042004;
            public const int Petiteranodon = 82946847;
            public const int UltimateConductorTyranno = 18940556;
            public const int OvertexQoatlus = 41782653;
            public const int GiantRex = 80280944;
            public const int DinowrestlerPankratops = 82385847;

            // Evol Main Deck
            public const int EvoltileNajasho = 88095331;
            public const int EvoltileWestlo = 81873903;
            public const int EvolsaurVulcano = 54266211;
            public const int EvolsaurCerato = 80651316;

            // Spells & Traps
            public const int FossilDig = 47325505;
            public const int EvoDiversity = 88760522;
            public const int EvoForce = 5338223;
            public const int LostWorld = 17228908;
            public const int Terraforming = 73628505;
            public const int DoubleEvolutionPill = 38179121;
            public const int EvoSingularity = 74100225;

            // Hand Traps & Staples
            public const int DrollAndLockBird = 94145021;
            public const int Nibiru = 27204311;
            public const int DarkRulerNoMore = 54693926;
            public const int CosmicCyclone = 8267140;
            public const int EvenlyMatched = 15693423;

            // Extra Deck
            public const int EvolzarDolkka = 42752141;
            public const int EvolzarLaggia = 74294676;
            public const int EvolzarSolda = 18511599;
            public const int AbyssDweller = 21044178;
            public const int TornadoDragon = 6983839;
            public const int Castel = 82633039;
            public const int EvilswarmExcitonKnight = 46772449;
            public const int Linkuriboh = 41999284;
            public const int KnightmarePhoenix = 2857636;
            public const int KnightmareUnicorn = 38342335;
        }

        private static readonly int[] AceCardIds = {
            CardId.UltimateConductorTyranno,
            CardId.OvertexQoatlus,
            CardId.EvolzarLaggia,
            CardId.EvolzarDolkka,
            CardId.EvolzarSolda,
            CardId.Castel,
            CardId.KnightmareUnicorn
        };

        private bool _oviraptorSummoned = false;
        private bool _oviraptorSearchUsed = false;
        private bool _oviraptorPopUsed = false;
        private bool _pillUsed = false;
        private bool _singularityUsed = false;
        private bool _miscHandUsed = false;

        public _2026_RexRaptorExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            HeuristicGuard.RegisterAceCards(AceCardIds);

            // โ”€โ”€ Combo Router โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Oviraptor-Baby-Laggia",
                RequiredCards = new List<int> { CardId.SouleatingOviraptor },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.SouleatingOviraptor, ActionType = ExecutorType.Summon, Description = "NS Oviraptor โ’ search Misc" },
                    new() { CardId = CardId.Miscellaneousaurus, ActionType = ExecutorType.Activate, Description = "Discard Misc for protection" },
                    new() { CardId = CardId.SouleatingOviraptor, ActionType = ExecutorType.Activate, Description = "Oviraptor target Baby โ’ SS Vulcano" },
                    new() { CardId = CardId.EvolzarLaggia, ActionType = ExecutorType.SpSummon, Description = "Overlay Vulcano + Oviraptor โ’ Laggia" }
                },
                EndBoardScore = 85
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "FossilDig-Oviraptor-UCT",
                RequiredCards = new List<int> { CardId.FossilDig },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.FossilDig, ActionType = ExecutorType.Activate, Description = "Fossil Dig โ’ search Oviraptor" },
                    new() { CardId = CardId.SouleatingOviraptor, ActionType = ExecutorType.Summon, Description = "NS Oviraptor โ’ search UCT" },
                    new() { CardId = CardId.UltimateConductorTyranno, ActionType = ExecutorType.SpSummon, Description = "Banish 2 Dinos โ’ SS UCT" }
                },
                EndBoardScore = 90
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Going2nd-Pankratops-UCT-OTK",
                RequiredCards = new List<int> { CardId.DinowrestlerPankratops, CardId.DoubleEvolutionPill },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.DinowrestlerPankratops, ActionType = ExecutorType.SpSummon, Description = "SS Pankratops to break board" },
                    new() { CardId = CardId.DoubleEvolutionPill, ActionType = ExecutorType.Activate, Description = "Pill โ’ SS UCT" },
                    new() { CardId = CardId.UltimateConductorTyranno, ActionType = ExecutorType.Activate, Description = "UCT book all monsters & OTK push" }
                },
                EndBoardScore = 95
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Evo-Najasho-Force-Line",
                RequiredCards = new List<int> { CardId.EvoltileNajasho, CardId.EvoForce },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.EvoltileNajasho, ActionType = ExecutorType.Summon, Description = "NS Najasho" },
                    new() { CardId = CardId.EvoForce, ActionType = ExecutorType.Activate, Description = "Evo-Force pop Najasho โ’ SS Vulcano + Cerato" },
                    new() { CardId = CardId.EvolzarDolkka, ActionType = ExecutorType.SpSummon, Description = "Overlay โ’ Evolzar Dolkka" }
                },
                EndBoardScore = 80
            });

            BaitPlanner.RegisterComboStarters(CardId.SouleatingOviraptor, CardId.FossilDig, CardId.EvoDiversity);
            BaitPlanner.RegisterBaitCards(CardId.CosmicCyclone);
            ChainAdvisor.RegisterHighValueTargets(CardId.UltimateConductorTyranno, CardId.EvolzarLaggia, CardId.EvolzarDolkka);

            // Priority 1: Hand Traps & Reactives
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollEffect);
            AddExecutor(ExecutorType.Activate, CardId.CosmicCyclone, CosmicCycloneEffect);

            // Priority 2: Board Breakers
            AddExecutor(ExecutorType.Activate, CardId.DarkRulerNoMore, DarkRulerEffect);
            AddExecutor(ExecutorType.Activate, CardId.EvenlyMatched, EvenlyMatchedEffect);

            // Priority 3: Boss Quick Effects
            AddExecutor(ExecutorType.Activate, CardId.UltimateConductorTyranno, UltimateConductorEffect);
            AddExecutor(ExecutorType.Activate, CardId.EvolzarLaggia, EvolzarLaggiaEffect);
            AddExecutor(ExecutorType.Activate, CardId.EvolzarDolkka, EvolzarDolkkaEffect);
            AddExecutor(ExecutorType.Activate, CardId.OvertexQoatlus, OvertexEffect);
            AddExecutor(ExecutorType.Activate, CardId.DinowrestlerPankratops, PankratopsEffect);

            // Priority 4: Field / Search Spells
            AddExecutor(ExecutorType.Activate, CardId.Terraforming, TerraformingEffect);
            AddExecutor(ExecutorType.Activate, CardId.LostWorld, LostWorldEffect);
            AddExecutor(ExecutorType.Activate, CardId.FossilDig, FossilDigEffect);
            AddExecutor(ExecutorType.Activate, CardId.EvoDiversity, EvoDiversityEffect);

            // Priority 5: Main Combos & Special Summons
            AddExecutor(ExecutorType.Activate, CardId.Miscellaneousaurus, MiscellaneousaurusEffect);
            AddExecutor(ExecutorType.Summon, CardId.SouleatingOviraptor, OviraptorSummon);
            AddExecutor(ExecutorType.Activate, CardId.SouleatingOviraptor, OviraptorEffect);
            AddExecutor(ExecutorType.Summon, CardId.EvoltileNajasho, NajashoSummon);
            AddExecutor(ExecutorType.Activate, CardId.EvoForce, EvoForceEffect);

            // Priority 6: Pill Summon & Bosses
            AddExecutor(ExecutorType.Activate, CardId.DoubleEvolutionPill, DoubleEvolutionPillEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.UltimateConductorTyranno, UltimateConductorSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DinowrestlerPankratops, PankratopsSummon);

            // Priority 7: Extenders & Fallback Summons
            AddExecutor(ExecutorType.Summon, CardId.EvolsaurCerato);
            AddExecutor(ExecutorType.Summon, CardId.EvolsaurVulcano);
            AddExecutor(ExecutorType.Summon, CardId.GiantRex);
            AddExecutor(ExecutorType.Summon, CardId.Babycerasaurus);

            // Priority 8: Extra Deck overlay / summons
            AddExecutor(ExecutorType.SpSummon, CardId.EvolzarLaggia, EvolzarLaggiaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.EvolzarDolkka, EvolzarDolkkaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.EvolzarSolda, EvolzarSoldaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Castel, CastelSummon);
            AddExecutor(ExecutorType.Activate, CardId.Castel, CastelEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.AbyssDweller, AbyssDwellerSummon);
            AddExecutor(ExecutorType.Activate, CardId.AbyssDweller, AbyssDwellerEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.TornadoDragon, TornadoDragonSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.EvilswarmExcitonKnight, ExcitonSummon);
            AddExecutor(ExecutorType.Activate, CardId.EvilswarmExcitonKnight, ExcitonEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmarePhoenix, KnightmarePhoenixSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmareUnicorn, KnightmareUnicornSummon);

            // Priority 9: Trap cards & Repos
            AddExecutor(ExecutorType.Activate, CardId.EvoSingularity, EvoSingularityEffect);
            AddExecutor(ExecutorType.SpellSet, CardId.EvoSingularity);
            AddExecutor(ExecutorType.SpellSet, SpellSetFiltered);

            AddExecutor(ExecutorType.Repos, CardId.EvoltileWestlo);
            AddExecutor(ExecutorType.Activate, CardId.EvoltileWestlo);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _oviraptorSummoned = false;
            _oviraptorSearchUsed = false;
            _oviraptorPopUsed = false;
            _pillUsed = false;
            _singularityUsed = false;
            _miscHandUsed = false;
        }

        public override bool OnSelectHand() => false; // Prefer 2nd for UCT OTK

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
            if (c.IsCode(CardId.Babycerasaurus, CardId.Petiteranodon))
                return 800;
            if (c.IsCode(CardId.DrollAndLockBird))
                return 800;
            return 100;
        }

        public override IList<ClientCard> OnSelectLinkMaterial(IList<ClientCard> cards, int min, int max)
        {
            var withoutFieldAces = cards.Where(c => c != null && !(c.Location == CardLocation.MonsterZone && IsAceCard(c))).ToList();
            if (withoutFieldAces.Count >= min)
            {
                var sorted = withoutFieldAces.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }
            var allSorted = cards.Where(c => c != null).OrderBy(c => GetMaterialPriority(c)).ToList();
            return allSorted.Take(max).ToList();
        }

        public override IList<ClientCard> OnSelectXyzMaterial(IList<ClientCard> cards, int min, int max)
        {
            var withoutFieldAces = cards.Where(c => c != null && !(c.Location == CardLocation.MonsterZone && IsAceCard(c))).ToList();
            if (withoutFieldAces.Count >= min)
            {
                var sorted = withoutFieldAces.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }
            var allSorted = cards.Where(c => c != null).OrderBy(c => GetMaterialPriority(c)).ToList();
            return allSorted.Take(max).ToList();
        }

        public int GetBanishPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (c.Id == CardId.GiantRex) return 0;
            if (c.Id == CardId.Babycerasaurus || c.Id == CardId.Petiteranodon) return 10;
            if (c.Id == CardId.EvolsaurVulcano || c.Id == CardId.EvolsaurCerato) return 20;
            if (c.Id == CardId.Miscellaneousaurus) return 50;
            if (IsAceCard(c)) return 900;
            return 100;
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (LastChainCard != null && LastChainCard.Id == CardId.SouleatingOviraptor && hint == 509)
            {
                var target = cards.FirstOrDefault(c => c.Id == CardId.Miscellaneousaurus)
                    ?? cards.FirstOrDefault(c => c.Id == CardId.GiantRex)
                    ?? cards.FirstOrDefault(c => c.Id == CardId.Babycerasaurus)
                    ?? cards.FirstOrDefault();
                if (target != null)
                {
                    return new List<ClientCard> { target };
                }
            }

            if (cards.Count > 0 && cards.All(c => c != null && c.Location == CardLocation.Deck))
            {
                var target = cards.FirstOrDefault(c => c.Id == CardId.Babycerasaurus)
                    ?? cards.FirstOrDefault(c => c.Id == CardId.Petiteranodon);
                if (target != null)
                {
                    return new List<ClientCard> { target };
                }
            }

            if (hint == 509)
            {
                var deckCards = cards.Where(c => c != null && c.Location == CardLocation.Deck).ToList();
                if (deckCards.Count > 0 && cards.Any(c => c.Location != CardLocation.Deck))
                    return Util.CheckSelectCount(deckCards, cards, min, max);
            }

            if (hint == 511 || hint == 512 || hint == 513 || hint == 533)
            {
                var sorted = cards.Where(c => c != null)
                    .OrderBy(c => GetMaterialPriority(c))
                    .ToList();

                if (cancelable)
                {
                    var nonFieldAces = sorted.Where(c => !(c.Location == CardLocation.MonsterZone && IsAceCard(c))).ToList();
                    if (nonFieldAces.Count < min) return null;
                    return Util.CheckSelectCount(nonFieldAces, cards, min, max);
                }
                return Util.CheckSelectCount(sorted, cards, min, max);
            }

            if (hint == 508 || hint == 504)
            {
                var sorted = cards.Where(c => c != null)
                    .OrderBy(c => GetBanishPriority(c))
                    .ToList();
                return Util.CheckSelectCount(sorted, cards, min, max);
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        private bool SpellSetFiltered()
        {
            return Duel.Phase == DuelPhase.Main2 || !Main.CanBattlePhase;
        }

        private bool DrollEffect() => Duel.LastChainPlayer == 1;

        private bool CosmicCycloneEffect()
        {
            ClientCard target = Enemy.SpellZone.FirstOrDefault(c => c != null && c.IsFaceup() && !IsTargetImmune(c));
            if (target == null) target = Enemy.SpellZone.FirstOrDefault(c => c != null && !IsTargetImmune(c));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool DarkRulerEffect()
        {
            return Enemy.GetMonsterCount() >= 2 || Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= 2500);
        }

        private bool EvenlyMatchedEffect()
        {
            return Bot.GetFieldCount() <= 1 && Enemy.GetFieldCount() >= 3;
        }

        private bool TerraformingEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (IsMain1SearchDeferred()) return false;
            if (Bot.HasInSpellZone(CardId.LostWorld) || Bot.Hand.Any(c => c.Id == CardId.LostWorld))
                return false;
            return GetRemainingCount(CardId.LostWorld) > 0;
        }

        private bool LostWorldEffect()
        {
            if (Bot.HasInSpellZone(CardId.LostWorld)) return false;
            return true;
        }

        private bool FossilDigEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (IsMain1SearchDeferred()) return false;
            int target = CardId.SouleatingOviraptor;
            if (Bot.Hand.Any(c => c.Id == CardId.SouleatingOviraptor) || _oviraptorSummoned)
            {
                int gyDinos = Bot.Graveyard.Count(c => c.IsMonster() && c.Race == (int)CardRace.Dinosaur);
                if (gyDinos >= 2 && GetRemainingCount(CardId.UltimateConductorTyranno) > 0
                    && !Bot.Hand.Any(c => c.Id == CardId.UltimateConductorTyranno))
                {
                    target = CardId.UltimateConductorTyranno;
                }
                else
                {
                    target = CardId.Miscellaneousaurus;
                    if (Bot.Hand.Any(c => c.Id == CardId.Miscellaneousaurus))
                        target = CardId.Babycerasaurus;
                }
            }
            if (GetRemainingCount(target) > 0)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool EvoDiversityEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (IsMain1SearchDeferred()) return false;
            int target = CardId.EvoltileNajasho;
            if (Bot.Hand.Any(c => c.Id == CardId.EvoltileNajasho) || !Bot.Hand.Any(c => c.Id == CardId.EvoForce))
            {
                target = CardId.EvoltileWestlo;
            }
            if (GetRemainingCount(target) > 0)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool OviraptorSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            _oviraptorSummoned = true;
            return true;
        }

        private bool OviraptorEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (!_oviraptorSearchUsed)
                {
                    int searchTarget = CardId.Miscellaneousaurus;
                    if (Bot.Hand.Any(c => c.Id == CardId.Miscellaneousaurus) || _miscHandUsed)
                    {
                        int gyDinos = Bot.Graveyard.Count(c => c.IsMonster() && c.Race == (int)CardRace.Dinosaur);
                        if (gyDinos >= 1 && GetRemainingCount(CardId.UltimateConductorTyranno) > 0
                            && !Bot.Hand.Any(c => c.Id == CardId.UltimateConductorTyranno))
                        {
                            searchTarget = CardId.UltimateConductorTyranno;
                        }
                        else
                        {
                            searchTarget = CardId.Babycerasaurus;
                        }
                    }
                    _oviraptorSearchUsed = true;
                    AI.SelectOption(0);
                    AI.SelectCard(searchTarget);
                    return true;
                }

                if (!_oviraptorPopUsed)
                {
                    ClientCard target = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && (c.Id == CardId.Babycerasaurus || c.Id == CardId.Petiteranodon));
                    if (target == null)
                    {
                        target = Enemy.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.HasType(CardType.Token) && c.Race == (int)CardRace.Dinosaur);
                    }
                    if (target == null)
                    {
                        target = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Id != CardId.SouleatingOviraptor && c.Level <= 4 && !IsAceCard(c));
                    }

                    bool hasRevivable = Bot.Graveyard.Any(c => c.IsMonster() && c.Race == (int)CardRace.Dinosaur && c.IsCanRevive());

                    if (target != null && hasRevivable)
                    {
                        _oviraptorPopUsed = true;
                        AI.SelectCard(target);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool MiscellaneousaurusEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_miscHandUsed) return false;
                if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
                {
                    _miscHandUsed = true;
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (IsSpecialSummonBlocked()) return false;
                int dinoCount = Bot.Graveyard.Count(c => c.IsMonster() && c.Race == (int)CardRace.Dinosaur);
                
                bool hasOviraptor = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id == CardId.SouleatingOviraptor && !_oviraptorPopUsed);
                if (hasOviraptor && dinoCount >= 1 && GetRemainingCount(CardId.Babycerasaurus) > 0)
                {
                    AI.SelectCard(CardId.Babycerasaurus);
                    return true;
                }

                if (dinoCount >= 7 && GetRemainingCount(CardId.OvertexQoatlus) > 0)
                {
                    AI.SelectCard(CardId.OvertexQoatlus);
                    return true;
                }

                if (dinoCount >= 4)
                {
                    if (GetRemainingCount(CardId.SouleatingOviraptor) > 0 && !Bot.MonsterZone.Any(c => c != null && c.Id == CardId.SouleatingOviraptor))
                    {
                        AI.SelectCard(CardId.SouleatingOviraptor);
                        return true;
                    }
                    if (GetRemainingCount(CardId.GiantRex) > 0)
                    {
                        AI.SelectCard(CardId.GiantRex);
                        return true;
                    }
                }

                if (dinoCount >= 1 && GetRemainingCount(CardId.Babycerasaurus) > 0)
                {
                    AI.SelectCard(CardId.Babycerasaurus);
                    return true;
                }
                if (dinoCount >= 1 && GetRemainingCount(CardId.Petiteranodon) > 0)
                {
                    AI.SelectCard(CardId.Petiteranodon);
                    return true;
                }
            }
            return false;
        }

        private bool NajashoSummon()
        {
            return Bot.Hand.Any(c => c.Id == CardId.EvoForce);
        }

        private bool EvoForceEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            ClientCard target = Bot.MonsterZone.FirstOrDefault(c => c != null && c.Id == CardId.EvoltileNajasho);
            if (target != null)
            {
                AI.SelectCard(target);
                AI.SelectNextCard(CardId.EvolsaurVulcano, CardId.EvolsaurCerato);
                return true;
            }
            return false;
        }

        private bool DoubleEvolutionPillEffect()
        {
            if (IsSpecialSummonBlocked() || _pillUsed) return false;

            bool hasDino = Bot.Graveyard.Any(c => c.IsMonster() && c.Race == (int)CardRace.Dinosaur)
                           || Bot.Hand.Any(c => c.IsMonster() && c.Race == (int)CardRace.Dinosaur && c.Id != CardId.UltimateConductorTyranno);
            bool hasNonDino = Bot.Graveyard.Any(c => c.IsMonster() && c.Race != (int)CardRace.Dinosaur)
                              || Bot.Hand.Any(c => c.IsMonster() && c.Race != (int)CardRace.Dinosaur);

            if (hasDino && hasNonDino)
            {
                _pillUsed = true;
                int target = CardId.UltimateConductorTyranno;
                if (GetRemainingCount(CardId.UltimateConductorTyranno) == 0)
                    target = CardId.OvertexQoatlus;
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool UltimateConductorSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            int gyDinos = Bot.Graveyard.Count(c => c.IsMonster() && c.Race == (int)CardRace.Dinosaur);
            return gyDinos >= 2;
        }

        private bool UltimateConductorEffect()
        {
            if (Enemy.GetMonsterCount() > 0)
            {
                ClientCard target = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup()
                    && (c.Id == CardId.Babycerasaurus || c.Id == CardId.Petiteranodon));
                if (target == null)
                    target = Bot.Hand.FirstOrDefault(c => c.Id == CardId.Babycerasaurus || c.Id == CardId.Petiteranodon);
                if (target == null)
                    target = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && !IsAceCard(c) && c.Id != CardId.UltimateConductorTyranno);
                if (target == null)
                    target = Bot.Hand.FirstOrDefault(c => c.IsMonster() && c.Id != CardId.UltimateConductorTyranno);
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool OvertexEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                ClientCard lastChain = Util.GetLastChainCard();
                if (lastChain != null && lastChain.Controller == 1 && (lastChain.IsSpell() || lastChain.IsTrap()))
                {
                    ClientCard pop = Bot.MonsterZone.FirstOrDefault(c => c != null && (c.Id == CardId.Babycerasaurus || c.Id == CardId.Petiteranodon));
                    if (pop == null) pop = Bot.Hand.FirstOrDefault(c => c.Id == CardId.Babycerasaurus || c.Id == CardId.Petiteranodon);
                    if (pop == null) pop = Bot.MonsterZone.FirstOrDefault(c => c != null && !IsAceCard(c) && c.Id != CardId.OvertexQoatlus);
                    if (pop != null)
                    {
                        AI.SelectCard(pop);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool PankratopsSummon()
        {
            return Bot.GetMonsterCount() < Enemy.GetMonsterCount();
        }

        private bool PankratopsEffect()
        {
            ClientCard target = Enemy.MonsterZone
                .Where(c => c != null && c.IsFaceup() && !IsTargetImmune(c))
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();
            if (target == null)
                target = Enemy.SpellZone.FirstOrDefault(c => c != null && c.IsFaceup());
            if (target == null)
                target = Enemy.SpellZone.FirstOrDefault(c => c != null);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool EvolzarLaggiaEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                ClientCard lastChain = Util.GetLastChainCard();
                if (lastChain != null && lastChain.Controller == 1)
                    return Card.Overlays.Count >= 2;
            }
            return false;
        }

        private bool EvolzarDolkkaEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                ClientCard lastChain = Util.GetLastChainCard();
                if (lastChain != null && lastChain.Controller == 1 && lastChain.IsMonster())
                    return Card.Overlays.Count >= 1;
            }
            return false;
        }

        private bool EvolzarLaggiaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && !IsAceCard(c) && c.Level == 4 && c.Race == (int)CardRace.Dinosaur) >= 2;
        }

        private bool EvolzarDolkkaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && !IsAceCard(c) && c.Level == 4 && c.Race == (int)CardRace.Dinosaur) >= 2;
        }

        private bool EvolzarSoldaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && !IsAceCard(c) && c.Level == 6) >= 2;
        }

        private bool CastelSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && !IsAceCard(c) && c.Level == 4) >= 2
                   && Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= 2000);
        }

        private bool CastelEffect()
        {
            ClientCard target = Enemy.MonsterZone
                .Where(c => c != null && c.IsFaceup() && !IsTargetImmune(c))
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool AbyssDwellerEffect()
        {
            if (Duel.Player == 1 || Duel.LastChainPlayer == 1)
                return Card.Overlays.Count >= 1;
            return false;
        }

        private bool ExcitonSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && !IsAceCard(c) && c.Level == 4) >= 2
                   && (Enemy.GetFieldCount() > Bot.GetFieldCount() + 1);
        }

        private bool ExcitonEffect()
        {
            return Enemy.GetFieldCount() > Bot.GetFieldCount();
        }

        private bool EvoSingularityEffect()
        {
            if (IsSpecialSummonBlocked() || _singularityUsed) return false;
            ClientCard tile = Bot.Graveyard.FirstOrDefault(c => c.IsMonster() && (c.Id == CardId.EvoltileNajasho || c.Id == CardId.EvoltileWestlo));
            ClientCard saur = Bot.Graveyard.FirstOrDefault(c => c.IsMonster() && (c.Id == CardId.EvolsaurVulcano || c.Id == CardId.EvolsaurCerato));

            if (tile != null && saur != null)
            {
                _singularityUsed = true;
                AI.SelectCard(new[] { tile, saur });
                int choice = CardId.EvolzarLaggia;
                if (Bot.MonsterZone.Any(c => c != null && c.Id == CardId.EvolzarLaggia))
                    choice = CardId.EvolzarDolkka;
                AI.SelectNextCard(choice);
                return true;
            }
            return false;
        }

        private bool AbyssDwellerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldAvoidGenericExtraDeckSummon(2)) return false;
            return Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && !IsAceCard(c) && c.Level == 4) >= 2;
        }

        private bool TornadoDragonSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldAvoidGenericExtraDeckSummon(2)) return false;
            return Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && !IsAceCard(c) && c.Level == 4) >= 2;
        }

        private bool KnightmarePhoenixSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldAvoidGenericExtraDeckSummon(2)) return false;
            return Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && !IsAceCard(c)) >= 2;
        }

        private bool KnightmareUnicornSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldAvoidGenericExtraDeckSummon(3)) return false;
            return Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && !IsAceCard(c)) >= 2;
        }
    }

    [Deck("Expert_2026_RexRaptor", "2026_RexRaptor")]
    public class ExpertRexRaptorExecutor : _2026_RexRaptorExecutor
    {
        private string _duelId;
        public ExpertRexRaptorExecutor(GameAI ai, Duel duel) : base(ai, duel)
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
