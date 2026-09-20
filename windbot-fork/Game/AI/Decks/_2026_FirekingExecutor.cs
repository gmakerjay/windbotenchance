using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    // ============================================================
    // CARD AUDIT โ€” 2026_Fireking
    // ============================================================
    // | Card Name                                      | Type    | OPT? | Cost               | Effect Summary                                | Activate When                     | NEVER Activate When                    |
    // |------------------------------------------------|---------|------|--------------------|-----------------------------------------------|-----------------------------------|----------------------------------------|
    // | Sacred Fire King Garunix (66431519)            | Monster | Yes  | None               | FIRE destroyed: SS itself; Main: pop 1 FIRE   | Have target in hand/deck to pop   | Hand/deck has no useful targets        |
    // | Fire King High Avatar Garunix (23015896)       | Monster | No   | None               | Standby after pop: SS itself & wipe all monsters| Standby phase trigger             | None                                   |
    // | Fire King High Avatar Kirin (02526224)         | Monster | Yes  | Destroy 1 FIRE     | Main (Quick): pop 1 FIRE, SS itself; If popped| Disruption / pop trigger / battle | No other FIRE in hand/field            |
    // |                                                |         |      |                    | SS 1 Fire King hand/GY & destroy 1 card       |                                   |                                        |
    // | Fire King Courtier Ulcanix (44455560)          | Monster | Yes  | Destroy 1 FIRE     | Summon: pop 1 FIRE, search 1 FIRE beast, lvl  | Combo starter / searcher          | No other FIRE in hand/field            |
    // |                                                |         |      |                    | change; If popped: SS Sacred Garunix from Deck|                                   |                                        |
    // | Fire King Avatar Arvata (18621798)             | Monster | Yes  | Destroy 1 FIRE     | Monster eff: negate activation, pop 1 FIRE    | Negate opponent's monster eff     | Negate ourselves / no other FIRE       |
    // | Fire King Avatar Kirin (96594609)              | Monster | Yes  | None               | FK popped: SS itself; GY: send 1 FIRE to GY   | Pop extender                      | No FIRE to send / no FK popped         |
    // | Legendary Fire King Ponix (90681088)           | Monster | Yes  | None               | FIRE popped: SS itself; Summon: search FK S/T | Primary searcher / setup          | S/T already in hand                    |
    // | Fire King Island (57554544)                    | Spell   | Yes  | Destroy 1 monster  | Main: pop 1, search FK monster; SS Winged Beast| Main Phase setup                  | Already used OPT effect                |
    // | Fire King Sanctuary (65305978)                 | Spell   | Yes  | None               | On-act: place Island; protection; Xyz summon  | Setup / protection                | Already have Island / Sanctuary on field|
    // | Fire King Sky Burn (91703676)                  | Spell   | Yes  | Target FK & opp    | Quick: pop equal FK and opponent cards        | Disruption / pop trigger          | No FK on field / no opponent cards     |
    // | Miscellaneousaurus (38572779)                  | Monster | Yes  | Banish itself/Dino | Hand: Dino unaffected; GY: banish to SS Dino   | Setup / summon Jurrac Megalo      | No Dino in deck (Megalo)               |
    // | Jurrac Megalo (93170499)                       | Monster | Yes  | Discard 2 cards    | Dino on field: SS itself; Pop: send Jurrac    | Draw engine / send Astero to GY   | Already sent Astero                    |
    // | Fossil Dig (47325505)                          | Spell   | No   | None               | Add 1 Level 6- Dino from Deck                 | Search Miscellaneousaurus / Megalo| No target in deck                      |
    // | The Fallen & The Virtuous (30271097)           | Spell   | Yes  | Send Fallen Extra  | Send Albion to GY, target face-up to destroy  | Self-pop FK or destroy enemy      | No target on field                     |
    // | Dominus Impulse (40366667)                     | Trap    | Yes  | None               | Hand-trap: negate SS effect                   | Opponent tries to SS              | Hand activation locks LIGHT/EARTH/WIND |
    // ============================================================
    // ACE CARDS: Primary: Garunix Eternity, Hyang of the Fire Kings / Secondary: Promethean Princess / Tertiary: Jurrac Astero
    // COMBO STARTERS: 1. Legendary Fire King Ponix 2. Fire King Courtier Ulcanix 3. Fossil Dig
    // CHOKEPOINTS: Ulcanix / Ponix / Fossil Dig search negated.
    // ============================================================

    [Deck("2026_Fireking", "2026_Fireking")]
    public class _2026_FirekingExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck
            public const int SacredFireKingGarunix = 66431519;
            public const int FireKingHighAvatarGarunix = 23015896;
            public const int FireKingHighAvatarKirin = 2526224;
            public const int FireKingCourtierUlcanix = 44455560;
            public const int FireKingAvatarArvata = 18621798;
            public const int FireKingAvatarKirin = 96594609;
            public const int LegendaryFireKingPonix = 90681088;
            public const int FireKingIsland = 57554544;
            public const int FireKingSanctuary = 65305978;
            public const int FireKingSkyBurn = 91703676;
            public const int Miscellaneousaurus = 38572779;
            public const int JurracMegalo = 93170499;
            public const int FossilDig = 47325505;
            public const int MulcharmyPurulia = 84192580;
            public const int AshBlossom = 14558127;
            public const int DrollAndLockBird = 94145021;
            public const int TheFallenAndTheVirtuous = 30271097;
            public const int DominusImpulse = 40366667;

            // Extra Deck
            public const int GarunixEternity = 64182380;
            public const int JurracMeteor = 17548456;
            public const int JurracAstero = 52553102;
            public const int AlbionTheBrandedDragon = 87746184;
            public const int SalamangreatRagingPhoenix = 57134592;
            public const int PrometheanPrincess = 2772337;
            public const int SPLittleKnight = 29301450;
            public const int SalamangreatSunlightWolf = 87871125;
            public const int HiitaTheFireCharmerAblaze = 48815792;
            public const int DharcTheDarkCharmerGloomy = 8264361;
            public const int Linkuriboh = 41999284;
        }

        private static readonly int[] HandTraps = {
            CardId.MulcharmyPurulia,
            CardId.AshBlossom,
            CardId.DrollAndLockBird
        };

        private bool _dominusImpulseActivatedThisTurnFromHand = false;

        public override bool OnSelectHand()
        {
            // Prefer Going First (true) to establish Sanctuary/Island and Jurrac set up
            return true;
        }

        public override void OnChaining(int player, ClientCard card)
        {
            if (card != null && card.Id == CardId.DominusImpulse && card.Controller == 0 && card.Location == CardLocation.Hand)
            {
                _dominusImpulseActivatedThisTurnFromHand = true;
            }
            base.OnChaining(player, card);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _dominusImpulseActivatedThisTurnFromHand = false;

            // โ”€โ”€ Going-Second BreakBoard: prioritize disruption over combo โ”€โ”€
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
            }
        }

        public _2026_FirekingExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Register Ace Cards to protect from material/tribute selection
            HeuristicGuard.RegisterAceCards(
                CardId.GarunixEternity,
                CardId.JurracAstero,
                CardId.JurracMeteor,
                CardId.PrometheanPrincess,
                CardId.SalamangreatRagingPhoenix,
                CardId.SPLittleKnight
            );
            ResourcePlan.RegisterAceCards(
                CardId.GarunixEternity,
                CardId.JurracAstero,
                CardId.JurracMeteor,
                CardId.PrometheanPrincess,
                CardId.SalamangreatRagingPhoenix,
                CardId.SPLittleKnight
            );

            // โ”€โ”€ Combo Router: Sequencing โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Ponix-Island-Ulcanix",
                RequiredCards = new List<int> { CardId.LegendaryFireKingPonix, CardId.FireKingIsland },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.LegendaryFireKingPonix, ActionType = ExecutorType.Summon, Description = "Summon Ponix" },
                    new() { CardId = CardId.LegendaryFireKingPonix, ActionType = ExecutorType.Activate, Description = "Ponix search Sanctuary" },
                    new() { CardId = CardId.FireKingSanctuary, ActionType = ExecutorType.Activate, Description = "Activate Sanctuary to place Island" },
                    new() { CardId = CardId.FireKingIsland, ActionType = ExecutorType.Activate, Description = "Island pop Ponix search Ulcanix" }
                },
                EndBoardScore = 85
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Ulcanix-Pop-Garunix",
                RequiredCards = new List<int> { CardId.FireKingCourtierUlcanix },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.FireKingCourtierUlcanix, ActionType = ExecutorType.Summon, Description = "Summon Ulcanix" },
                    new() { CardId = CardId.FireKingCourtierUlcanix, ActionType = ExecutorType.Activate, Description = "Ulcanix pop FIRE to SS Sacred Garunix from Deck" }
                },
                EndBoardScore = 80
            });

            // —— Bait Planner ——
            BaitPlanner.RegisterComboStarters(CardId.LegendaryFireKingPonix, CardId.FireKingCourtierUlcanix, CardId.FireKingIsland);
            BaitPlanner.RegisterBaitCards(CardId.FossilDig, CardId.FireKingSanctuary);

            // ── Chain Advisor ──
            ChainAdvisor.RegisterHighValueTargets(CardId.LegendaryFireKingPonix, CardId.FireKingCourtierUlcanix, CardId.FireKingIsland);
            RegisterOptionalFieldRemovalCards(CardId.FireKingHighAvatarKirin, CardId.FireKingIsland);

            // TIER 1: Hand Traps (do not chain to ourselves!)
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyPurulia, MulcharmyEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdEffect);
            AddExecutor(ExecutorType.Activate, CardId.DominusImpulse, DominusImpulseEffect);

            // TIER 2: Quick Effects of Bosses/Disruptions
            AddExecutor(ExecutorType.Activate, CardId.FireKingAvatarArvata, ArvataEffect);
            AddExecutor(ExecutorType.Activate, CardId.FireKingHighAvatarKirin, HighKirinEffect);
            AddExecutor(ExecutorType.Activate, CardId.FireKingSkyBurn, SkyBurnEffect);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightEffect);
            AddExecutor(ExecutorType.Activate, CardId.PrometheanPrincess, PrometheanPrincessEffect);
            AddExecutor(ExecutorType.Activate, CardId.JurracAstero, JurracAsteroGYEffect);

            // TIER 3: Fusion / SS / Spell Setup
            AddExecutor(ExecutorType.Activate, CardId.TheFallenAndTheVirtuous, FallenVirtuousEffect);
            AddExecutor(ExecutorType.Activate, CardId.FossilDig, FossilDigEffect);

            // TIER 4: Field & Search Spells
            AddExecutor(ExecutorType.Activate, CardId.FireKingSanctuary, SanctuaryEffect);
            AddExecutor(ExecutorType.Activate, CardId.FireKingIsland, IslandEffect);

            // TIER 5: Monster Effects (Hand/GY activation)
            AddExecutor(ExecutorType.Activate, CardId.Miscellaneousaurus, MiscellaneousaurusEffect);
            AddExecutor(ExecutorType.Activate, CardId.SacredFireKingGarunix, SacredGarunixEffect);
            AddExecutor(ExecutorType.Activate, CardId.LegendaryFireKingPonix, PonixEffect);
            AddExecutor(ExecutorType.Activate, CardId.FireKingCourtierUlcanix, UlcanixEffect);
            AddExecutor(ExecutorType.Activate, CardId.JurracMegalo, JurracMegaloEffect);
            AddExecutor(ExecutorType.Activate, CardId.JurracMeteor, JurracMeteorSummonEffect);
            AddExecutor(ExecutorType.Activate, CardId.SalamangreatRagingPhoenix, RagingPhoenixEffect);
            AddExecutor(ExecutorType.Activate, CardId.SalamangreatSunlightWolf, SunlightWolfEffect);
            AddExecutor(ExecutorType.Activate, CardId.HiitaTheFireCharmerAblaze, HiitaEffect);

            // TIER 6: Special Summons (Ignition) & Normal Summons
            AddExecutor(ExecutorType.SpSummon, CardId.JurracMegalo, JurracMegaloSpSummon);
            
            // Normal Summons priority
            AddExecutor(ExecutorType.Summon, CardId.FireKingCourtierUlcanix);
            AddExecutor(ExecutorType.Summon, CardId.LegendaryFireKingPonix);
            AddExecutor(ExecutorType.Summon, CardId.FireKingAvatarArvata);
            AddExecutor(ExecutorType.Summon, CardId.JurracMegalo);
            AddExecutor(ExecutorType.Summon, CardId.FireKingAvatarKirin);

            // TIER 7: Extra Deck Summons
            AddExecutor(ExecutorType.SpSummon, CardId.GarunixEternity, GarunixEternitySummon);
            AddExecutor(ExecutorType.SpSummon, CardId.PrometheanPrincess, PrometheanPrincessSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SalamangreatSunlightWolf, SunlightWolfSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.HiitaTheFireCharmerAblaze, HiitaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Linkuriboh, LinkuribohSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight);
            AddExecutor(ExecutorType.SpSummon, CardId.GarunixEternity);

            // Trigger active Extra deck effects
            AddExecutor(ExecutorType.Activate, CardId.GarunixEternity, GarunixEternityEffect);

            // TIER 8: Sets & Repos
            AddExecutor(ExecutorType.SpellSet, CardId.FireKingSkyBurn);
            AddExecutor(ExecutorType.SpellSet, CardId.TheFallenAndTheVirtuous);

            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        // ==========================================
        //  TIER 1: Hand Traps & Negations
        // ==========================================

        private bool MulcharmyEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }

        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.LastChainPlayer == 1 && DefaultAshBlossomAndJoyousSpring();
        }

        private bool DrollAndLockBirdEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (_dominusImpulseActivatedThisTurnFromHand) return false;
            return Duel.Player == 1 && Duel.LastChainPlayer == 1;
        }

        private bool DominusImpulseEffect()
        {
            if (Card.Location == CardLocation.Hand) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.LastChainPlayer == 1)
            {
                return true;
            }
            return false;
        }

        // ==========================================
        //  TIER 2: Quick Effects and Disruptions
        // ==========================================

        private bool ArvataEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (LastChainCard != null && LastChainCard.Controller == 0) return false;
                if (Duel.LastChainPlayer == 1)
                {
                    // Verify we have a FIRE monster in hand or face-up field to destroy
                    var targets = Bot.Hand.Concat(Bot.GetMonsters())
                        .Where(c => c != null && c.HasAttribute(CardAttribute.Fire) && c != Card && (c.Location != CardLocation.MonsterZone || c.IsFaceup()));
                    var sortedTargets = GetSafeDestroyTargets(targets);
                    if (sortedTargets.Count > 0)
                    {
                        AI.SelectCard(sortedTargets[0]);
                        return true;
                    }
                }
                return false;
            }
            else // GY revive trigger
            {
                if (Card.Location != CardLocation.Grave) return false;
                var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.HasAttribute(CardAttribute.Fire) && c.IsMonster() && (c.HasRace(CardRace.Beast) || c.HasRace(CardRace.BestWarrior) || c.HasRace(CardRace.WindBeast)) && c.Id != CardId.FireKingAvatarArvata && c.IsCanRevive());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
                return false;
            }
        }

        private bool HighKirinEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // In opponent's turn: only disrupt if enemy has cards
                if (Duel.Player == 1 && Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0)
                    return false;

                if (ShouldSkipCombo()) return false;
                var popTargets = Bot.Hand.Concat(Bot.GetMonsters())
                    .Where(c => c != null && c.HasAttribute(CardAttribute.Fire) && c != Card && (c.Location != CardLocation.MonsterZone || c.IsFaceup()));
                var sortedTargets = GetSafeDestroyTargets(popTargets);

                if (sortedTargets.Count > 0)
                {
                    AI.SelectCard(sortedTargets[0]);
                    return true;
                }
                return false;
            }
            return true; // GY trigger handles revival & optional pop automatically or via OnSelectCard
        }

        private bool SkyBurnEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;

            var ourFKs = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.IsCode(CardId.LegendaryFireKingPonix, CardId.FireKingCourtierUlcanix, CardId.FireKingAvatarArvata, CardId.FireKingAvatarKirin, CardId.SacredFireKingGarunix, CardId.FireKingHighAvatarKirin, CardId.FireKingHighAvatarGarunix)).ToList();
            var oppCards = Enemy.GetMonsters().Concat(Enemy.GetSpells()).Where(c => c != null && IsViableEffectTarget(c)).ToList();

            if (ourFKs.Count > 0 && oppCards.Count > 0)
            {
                var ours = ourFKs.OrderBy(c => {
                    if (c.IsCode(CardId.LegendaryFireKingPonix)) return 1;
                    if (c.IsCode(CardId.FireKingCourtierUlcanix)) return 2;
                    if (c.IsCode(CardId.FireKingAvatarArvata)) return 3;
                    return 10;
                }).First();

                var opp = oppCards.OrderByDescending(c => Scorer != null ? Scorer.ThreatScore(c) : (c.IsMonster() ? c.Attack : 1000)).First();

                AI.SelectCard(ours);
                AI.SelectNextCard(opp);
                return true;
            }
            return false;
        }

        private bool SPLittleKnightEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Enemy.GetMonsterCount() > 0 || (Enemy.GetSpellCount() > 0 && Enemy.GetSpells().Any(c => c != null && c.IsFaceup())))
            {
                return true;
            }
            if (Duel.LastChainPlayer == 1)
            {
                bool isTargetingUs = Duel.ChainTargets.Any(c => c != null && c.Controller == 0);
                if (isTargetingUs) return true;
            }
            return false;
        }

        private bool PrometheanPrincessEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.HasAttribute(CardAttribute.Fire) && c.IsMonster() && c.IsCanRevive() && !IsAceCard(c));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
                return false;
            }
            if (Card.Location == CardLocation.Grave)
            {
                var ours = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c))
                    .OrderBy(c => c.IsCode(CardId.JurracMegalo) ? 1 :
                                 c.IsCode(CardId.LegendaryFireKingPonix) ? 2 : 10).FirstOrDefault();
                var opps = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c)).ToList();
                if (ours != null && opps.Count > 0)
                {
                    AI.SelectCard(ours);
                    AI.SelectNextCard(opps.OrderByDescending(c => Scorer != null ? Scorer.ThreatScore(c) : c.Attack).First());
                    return true;
                }
            }
            return false;
        }

        private bool JurracAsteroGYEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (Duel.Player != 1) return false;
            if (IsSpecialSummonBlocked()) return false;

            bool hasOtherJurrac = Bot.Graveyard.Any(c => c != null && (c.Id == CardId.JurracMegalo || c.Id == CardId.JurracAstero || c.Id == CardId.JurracMeteor) && c != Card);
            bool hasMeteorInExtra = GetRemainingCount(CardId.JurracMeteor) > 0;

            if (hasOtherJurrac && hasMeteorInExtra)
            {
                var other = Bot.Graveyard.FirstOrDefault(c => c != null && (c.Id == CardId.JurracMegalo || c.Id == CardId.JurracAstero || c.Id == CardId.JurracMeteor) && c != Card);
                if (other != null)
                {
                    AI.SelectCard(other);
                    return true;
                }
            }
            return false;
        }

        // ==========================================
        //  TIER 3 & 4: Spells & Setup
        // ==========================================

        private bool FallenVirtuousEffect()
        {
            if (ShouldSkipCombo()) return false;
            bool hasAlbion = GetRemainingCount(CardId.AlbionTheBrandedDragon) > 0;
            if (!hasAlbion) return false;

            var opponentTargets = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                .Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c)).ToList();

            if (opponentTargets.Count > 0)
            {
                var target = opponentTargets.OrderByDescending(c => Scorer != null ? Scorer.ThreatScore(c) : (c.IsMonster() ? c.Attack : 1000)).First();
                AI.SelectCard(target);
                return true;
            }

            // Only self-pop if we have more than 1 monster to ensure we don't end up with an empty field!
            if (Bot.GetMonsterCount() > 1)
            {
                var ownTargets = Bot.GetMonsters().Concat(Bot.GetSpells())
                    .Where(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Fire) && !IsAceCard(c)).ToList();

                if (ownTargets.Count > 0)
                {
                    var target = ownTargets.OrderBy(c => {
                        if (c.IsCode(CardId.JurracMegalo)) return 1;
                        if (c.IsCode(CardId.LegendaryFireKingPonix)) return 2;
                        return 10;
                    }).First();
                    AI.SelectCard(target);
                    return true;
                }
            }

            return false;
        }

        private bool FossilDigEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Bot.GetRemainingCount(CardId.Miscellaneousaurus, 1) > 0)
            {
                AI.SelectCard(CardId.Miscellaneousaurus);
                return true;
            }
            if (Bot.GetRemainingCount(CardId.JurracMegalo, 3) > 0)
            {
                AI.SelectCard(CardId.JurracMegalo);
                return true;
            }
            return false;
        }

        private bool SanctuaryEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (Bot.HasInSpellZone(CardId.FireKingSanctuary)) return false;
                if (Bot.GetRemainingCount(CardId.FireKingIsland, 2) == 0) return false;
                return true;
            }
            if (ActivateDescription == Util.GetStringId(CardId.FireKingSanctuary, 0))
            {
                return true;
            }
            if (ActivateDescription == Util.GetStringId(CardId.FireKingSanctuary, 1))
            {
                // Xyz summon Garunix Eternity when opponent Special Summons
                if (Duel.LastChainPlayer == 1)
                {
                    var lv8s = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.Level == 8).ToList();
                    if (lv8s.Count >= 2 && GetRemainingCount(CardId.GarunixEternity) > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IslandEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (Bot.HasInSpellZone(CardId.FireKingIsland)) return false;
                // Never replace an existing Island if we control monsters and don't have Sanctuary protection!
                var currentField = Bot.SpellZone[5];
                if (currentField != null && currentField.IsCode(CardId.FireKingIsland) && Bot.GetMonsterCount() > 0 && !Bot.HasInSpellZone(CardId.FireKingSanctuary))
                    return false;
                return true;
            }

            if (ActivateDescription == Util.GetStringId(CardId.FireKingIsland, 0))
            {
                return IslandSearchEffect();
            }
            if (ActivateDescription == Util.GetStringId(CardId.FireKingIsland, 1))
            {
                return IslandSummonEffect();
            }
            return IslandSearchEffect() || IslandSummonEffect();
        }

        private bool IslandSearchEffect()
        {
            if (ShouldSkipCombo()) return false;

            var destroyTargets = Bot.Hand.Concat(Bot.GetMonsters())
                .Where(c => c != null && c.IsMonster() && (c.Location != CardLocation.MonsterZone || c.IsFaceup()));
            var sortedTargets = GetSafeDestroyTargets(destroyTargets);

            if (sortedTargets.Count == 0) return false;

            int searchId = CardId.FireKingCourtierUlcanix;
            if (Bot.GetRemainingCount(CardId.FireKingCourtierUlcanix, 3) > 0 && !Bot.HasInHand(CardId.FireKingCourtierUlcanix))
            {
                searchId = CardId.FireKingCourtierUlcanix;
            }
            else if (Bot.GetRemainingCount(CardId.SacredFireKingGarunix, 3) > 0)
            {
                searchId = CardId.SacredFireKingGarunix;
            }
            else
            {
                searchId = CardId.FireKingHighAvatarKirin;
            }

            AI.SelectCard(sortedTargets[0]);
            AI.SelectNextCard(searchId);
            return true;
        }

        private bool IslandSummonEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Bot.GetMonsterCount() > 0) return false;

            var ssTargets = Bot.Hand.Where(c => c != null && c.HasAttribute(CardAttribute.Fire) && c.HasRace(CardRace.WindBeast)).ToList();
            if (ssTargets.Count > 0)
            {
                var target = ssTargets.OrderBy(c => {
                    if (c.IsCode(CardId.FireKingCourtierUlcanix)) return 1;
                    if (c.IsCode(CardId.SacredFireKingGarunix)) return 2;
                    if (c.IsCode(CardId.LegendaryFireKingPonix)) return 3;
                    return 100;
                }).First();
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // ==========================================
        //  TIER 5: Monster Effects (Hand / GY / Field)
        // ==========================================

        private bool MiscellaneousaurusEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (Duel.Player == 0 && Duel.IsMainPhase())
                {
                    bool willPlayMegalo = Bot.HasInHand(CardId.JurracMegalo) || Bot.HasInMonstersZone(CardId.JurracMegalo) || Bot.Graveyard.Any(c => c.IsCode(CardId.JurracMegalo));
                    if (willPlayMegalo && !EnemyHasKnownNegate())
                    {
                        return true;
                    }
                }
                return false;
            }
            if (Card.Location == CardLocation.Grave)
            {
                if (IsSpecialSummonBlocked()) return false;
                if (Duel.Player == 0 && Duel.IsMainPhase())
                {
                    if (Bot.GetRemainingCount(CardId.JurracMegalo, 3) > 0)
                    {
                        AI.SelectCard(Card);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool SacredGarunixEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (ShouldSkipCombo()) return false;

                bool hasMegaloInDeck = Bot.GetRemainingCount(CardId.JurracMegalo, 3) > 0;
                if (hasMegaloInDeck)
                {
                    AI.SelectCard(CardLocation.Deck);
                    AI.SelectNextCard(CardId.JurracMegalo);
                    return true;
                }

                bool hasPonix = Bot.GetRemainingCount(CardId.LegendaryFireKingPonix, 3) > 0;
                if (hasPonix)
                {
                    AI.SelectCard(CardLocation.Deck);
                    AI.SelectNextCard(CardId.LegendaryFireKingPonix);
                    return true;
                }

                bool hasKirin = Bot.GetRemainingCount(CardId.FireKingAvatarKirin, 1) > 0;
                if (hasKirin)
                {
                    AI.SelectCard(CardLocation.Deck);
                    AI.SelectNextCard(CardId.FireKingAvatarKirin);
                    return true;
                }

                var targets = Bot.Hand.Concat(Bot.GetMonsters())
                    .Where(c => c != null && c.HasAttribute(CardAttribute.Fire) && c != Card && (c.Location != CardLocation.MonsterZone || c.IsFaceup()));
                var sortedTargets = GetSafeDestroyTargets(targets);

                if (sortedTargets.Count > 0)
                {
                    AI.SelectCard(sortedTargets[0]);
                    return true;
                }
                return false;
            }
            return true; // GY/Hand Summon trigger
        }

        private bool PonixEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                bool hasSanctuary = Bot.HasInSpellZone(CardId.FireKingSanctuary) || Bot.HasInHand(CardId.FireKingSanctuary);
                bool hasIsland = Bot.HasInSpellZone(CardId.FireKingIsland) || Bot.HasInHand(CardId.FireKingIsland);

                if (!hasSanctuary && !hasIsland && Bot.GetRemainingCount(CardId.FireKingSanctuary, 2) > 0)
                {
                    AI.SelectCard(CardId.FireKingSanctuary);
                    return true;
                }
                if (!hasIsland && Bot.GetRemainingCount(CardId.FireKingIsland, 2) > 0)
                {
                    AI.SelectCard(CardId.FireKingIsland);
                    return true;
                }
                if (Bot.GetRemainingCount(CardId.FireKingSkyBurn, 1) > 0)
                {
                    AI.SelectCard(CardId.FireKingSkyBurn);
                    return true;
                }
                return true;
            }
            return true; // GY Standby Phase addition or Hand SS trigger
        }

        private bool UlcanixEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                var destroyTargets = Bot.Hand.Concat(Bot.GetMonsters())
                    .Where(c => c != null && c.HasAttribute(CardAttribute.Fire) && c != Card && (c.Location != CardLocation.MonsterZone || c.IsFaceup()));
                var sortedTargets = GetSafeDestroyTargets(destroyTargets);

                if (sortedTargets.Count == 0) return false;

                int searchId = CardId.SacredFireKingGarunix;
                bool hasSacred = Bot.HasInHand(CardId.SacredFireKingGarunix) || Bot.HasInMonstersZone(CardId.SacredFireKingGarunix) || Bot.Graveyard.Any(c => c.IsCode(CardId.SacredFireKingGarunix));
                if (!hasSacred && Bot.GetRemainingCount(CardId.SacredFireKingGarunix, 3) > 0)
                {
                    searchId = CardId.SacredFireKingGarunix;
                }
                else if (Bot.GetRemainingCount(CardId.FireKingHighAvatarKirin, 3) > 0)
                {
                    searchId = CardId.FireKingHighAvatarKirin;
                }
                else if (Bot.GetRemainingCount(CardId.LegendaryFireKingPonix, 3) > 0)
                {
                    searchId = CardId.LegendaryFireKingPonix;
                }
                else
                {
                    searchId = CardId.FireKingAvatarArvata;
                }

                AI.SelectCard(sortedTargets[0]);
                AI.SelectNextCard(searchId);
                return true;
            }
            return true; // GY trigger
        }

        private bool JurracMegaloEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (ShouldSkipCombo()) return false;
                bool hasMegaloInHand = Bot.Hand.Any(c => c != null && c.IsCode(CardId.JurracMegalo) && c != Card);
                if (hasMegaloInHand && Bot.Hand.Count >= 2)
                {
                    return true;
                }
                return false;
            }
            return true; // GY trigger
        }

        private bool JurracMeteorSummonEffect()
        {
            var tuner = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && c.HasType(CardType.Tuner) && c.IsCanRevive());
            if (tuner != null)
            {
                AI.SelectCard(tuner);
            }
            return true;
        }

        private bool RagingPhoenixEffect()
        {
            if (Card.Location == CardLocation.Grave && Card.IsCanRevive())
            {
                return true;
            }
            return false;
        }

        private bool SunlightWolfEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.HasAttribute(CardAttribute.Fire) && c.IsMonster() && !IsAceCard(c));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool HiitaEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (IsSpecialSummonBlocked()) return false;
                var target = Enemy.Graveyard.FirstOrDefault(c => c != null && c.HasAttribute(CardAttribute.Fire) && c.IsMonster() && c.IsCanRevive());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        // ==========================================
        //  TIER 6: Special Summons & Ignitions
        // ==========================================

        private bool JurracMegaloSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Card.Location != CardLocation.Hand) return false;
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Dinosaur));
        }

        // ==========================================
        //  TIER 7: Extra Deck Summons
        // ==========================================

        private bool GarunixEternitySummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var lv8s = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.Level == 8).ToList();
            return lv8s.Count >= 2;
        }

        private bool PrometheanPrincessSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var fires = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Fire) && !IsAceCard(c)).ToList();
            return fires.Count >= 2;
        }

        private bool SunlightWolfSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var fires = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Fire) && !IsAceCard(c)).ToList();
            return fires.Count >= 2;
        }

        private bool HiitaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var fires = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Fire) && !IsAceCard(c)).ToList();
            var others = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c)).ToList();
            return fires.Count >= 1 && others.Count >= 2;
        }

        private bool LinkuribohSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var lvl1s = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.Level == 1 && (c.Id == CardId.LegendaryFireKingPonix || c.Id == CardId.JurracMegalo)).ToList();
            return lvl1s.Count > 0;
        }

        private bool GarunixEternityEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                var targets = Enemy.GetSpells().Where(c => c != null && IsViableEffectTarget(c)).ToList();
                if (targets.Count > 0)
                {
                    var target = targets.First();
                    AI.SelectCard(target);
                    return true;
                }
                return false; // Don't pop our own Spells/Traps if opponent has none!
            }
            return true; // Trigger when summoned or destroyed
        }

        // ==========================================
        //  Card Selection & Protections (OnSelectCard)
        // ==========================================

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Golden Rule: handle hints FIRST before null-safe Card checks

            // Special handling for Jurrac Megalo sending to GY from Extra Deck
            if (cards != null && cards.Any(c => c != null && c.Id == CardId.JurracAstero) && cards.Any(c => c != null && c.Id == CardId.JurracMeteor))
            {
                var astero = cards.FirstOrDefault(c => c != null && c.Id == CardId.JurracAstero);
                if (astero != null)
                {
                    return new List<ClientCard> { astero };
                }
            }

            // Hint 509: Special Summon from location
            if (hint == 509)
            {
                bool hasDeck = cards.Any(c => c.Location == CardLocation.Deck);
                if (hasDeck && cards.Any(c => c.Location == CardLocation.Hand))
                {
                    var targets = cards.Where(c => c.Location == CardLocation.Deck).ToList();
                    if (targets.Count >= min)
                    {
                        return targets.Take(max).ToList();
                    }
                }
            }

            // Discarding priority
            if (hint == 501)
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (c.IsCode(CardId.JurracMegalo)) return 10;
                    if (c.IsCode(CardId.LegendaryFireKingPonix)) return 20;
                    if (c.IsCode(CardId.SacredFireKingGarunix)) return 30;
                    if (HandTraps.Contains(c.Id)) return 100;
                    if (c.Controller == 0 && IsAceCard(c)) return 10000;
                    return 50;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // Hint 503: Destroy - if enemy cards are available, ALWAYS prioritize destroying opponent cards!
            if (hint == 503)
            {
                var enemyTargets = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyTargets.Count > 0)
                {
                    var sortedEnemy = enemyTargets.OrderByDescending(c => Scorer != null ? Scorer.ThreatScore(c) : (c.IsMonster() ? c.Attack : 1000)).ToList();
                    return sortedEnemy.Take(Math.Min(max, sortedEnemy.Count)).ToList();
                }
            }

            // Protect Aces in material/cost card selections (hints: materials, destroy, cost, target, remove, tograve)
            if (hint == 511 || hint == 512 || hint == 513 || hint == 533 || hint == 503 || hint == 501 || hint == 502 || hint == 504 || hint == 505 || hint == 508)
            {
                var safeCards = cards.Where(c => c == null || (c.Controller == 0 && !IsAceCard(c) && !HandTraps.Contains(c.Id)) || c.Controller == 1).ToList();
                if (safeCards.Count >= min)
                {
                    var sortedSafe = safeCards.OrderBy(c => {
                        if (c == null) return 999;
                        if (c.Location == CardLocation.Hand) return 10;
                        if (c.Location == CardLocation.Grave) return 20;
                        if (c.Location == CardLocation.MonsterZone) return 30;
                        return 100;
                    }).ToList();
                    return sortedSafe.Take(max).ToList();
                }
                else
                {
                    // Audit/log Ace usage if we are forced to choose an Ace card
                    var aceMonstersChosen = cards.Where(c => c != null && c.Controller == 0 && IsAceCard(c)).ToList();
                    foreach (var mat in aceMonstersChosen)
                    {
                        var res = ResourcePlan.EvaluateAceUsage(
                            card: mat,
                            hasLethalIfUsed: CanDealLethal(),
                            isOnlyAnswerToThreat: OpponentHasActiveNegator(),
                            haveAlternateWinCon: false
                        );
                        try
                        {
                            AI?.Log(LogLevel.Info, $"[ACE-AUDIT] Forced to select Ace card {mat.Id} (hint {hint}): {res.reason}");
                        }
                        catch {}
                    }

                    var sortedAll = cards.OrderBy(c => {
                        if (c == null) return 999;
                        if (c.Controller == 0 && IsAceCard(c)) return 10000;
                        if (c.Controller == 0 && HandTraps.Contains(c.Id)) return 8000;
                        if (c.Location == CardLocation.Hand) return 10;
                        if (c.Location == CardLocation.Grave) return 20;
                        if (c.Location == CardLocation.MonsterZone) return 30;
                        return 100;
                    }).ToList();
                    return sortedAll.Take(max).ToList();
                }
            }

            if (Card == null)
            {
                return base.OnSelectCard(cards, min, max, hint, cancelable);
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            if (card.IsCode(CardId.GarunixEternity) ||
                card.IsCode(CardId.JurracAstero) ||
                card.IsCode(CardId.JurracMeteor) ||
                card.IsCode(CardId.PrometheanPrincess) ||
                card.IsCode(CardId.SalamangreatRagingPhoenix) ||
                card.IsCode(CardId.SPLittleKnight))
                return true;

            return base.IsAceCard(card);
        }
        protected override bool IsBoardStrongEnough()
        {
            if (Bot.HasInMonstersZone(CardId.GarunixEternity)) return true;
            if (Bot.HasInMonstersZone(CardId.PrometheanPrincess)) return true;
            if (Bot.HasInMonstersZone(CardId.SalamangreatRagingPhoenix)) return true;
            return base.IsBoardStrongEnough();
        }
        protected override bool ShouldStopExtending()
        {
            if (Bot.HasInMonstersZone(CardId.GarunixEternity)
                || Bot.HasInMonstersZone(CardId.PrometheanPrincess))
                return base.ShouldStopExtending();
            return false;
        }

        private bool OpponentHasActiveNegator()
        {
            return Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && !c.IsDisabled() &&
                (c.IsFloodgate() || c.Attack >= 2500 || c.HasType(CardType.Link) || c.HasType(CardType.Synchro)));
        }

        public override bool ShouldAllowSpSummon(ClientCard card)
        {
            if (!base.ShouldAllowSpSummon(card)) return false;
            if (card == null) return true;

            // Extra Deck summons (excluding the Ace card itself on its initial summon)
            // If the summon would consume any face-up Ace card currently on the field as material, evaluate it.
            var activeAces = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && IsAceCard(c)).ToList();
            if (activeAces.Count > 0)
            {
                // Fusion / Link / Xyz summons that require materials from field
                int reqCount = card.HasType(CardType.Link) ? card.LinkCount : 2;
                var nonAceMats = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c)).ToList();
                if (nonAceMats.Count < reqCount)
                {
                    bool allowed = false;
                    foreach (var mat in activeAces)
                    {
                        var res = ResourcePlan.EvaluateAceUsage(
                            card: mat,
                            hasLethalIfUsed: CanDealLethal(),
                            isOnlyAnswerToThreat: OpponentHasActiveNegator(),
                            haveAlternateWinCon: false
                        );
                        if (res.allowed)
                        {
                            allowed = true;
                            break;
                        }
                    }
                    if (!allowed)
                    {
                        DecisionTracer.TraceSkip("ShouldAllowSpSummon", $"Summoning {card.Name} is not safe (would consume Ace card(s) as material)");
                        return false;
                    }
                }
            }

            return true;
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.Id == CardId.AshBlossom || c.Id == CardId.DrollAndLockBird || c.Id == CardId.MulcharmyPurulia) return 800;
            return 100;
        }

        private List<ClientCard> GetSafeDestroyTargets(IEnumerable<ClientCard> candidates)
        {
            return candidates
                .Where(c => c != null && !IsAceCard(c))
                .OrderBy(c => {
                    int score = 0;
                    if (c.Location == CardLocation.MonsterZone || c.Location == CardLocation.SpellZone)
                    {
                        score += 100000;
                        score += c.Attack;
                    }
                    if (c.IsCode(CardId.JurracMegalo)) score += 1;
                    else if (c.IsCode(CardId.LegendaryFireKingPonix)) score += 2;
                    else if (c.IsCode(CardId.SacredFireKingGarunix)) score += 3;
                    else if (c.IsCode(CardId.FireKingAvatarKirin)) score += 4;
                    else if (c.IsCode(CardId.FireKingHighAvatarKirin)) score += 5;
                    else if (c.IsCode(CardId.FireKingAvatarArvata)) score += 6;
                    else if (c.IsCode(CardId.FireKingCourtierUlcanix)) score += 7;
                    else score += 10000;
                    return score;
                }).ToList();
        }
    }
}
