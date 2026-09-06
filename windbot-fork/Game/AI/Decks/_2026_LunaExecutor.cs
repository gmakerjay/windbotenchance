using YGOSharp.OCGWrapper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    // ==========================================
    // 2026_Luna (Lunalight)
    // ==========================================
    [Deck("2026_Luna", "2026_Luna")]
    public class _2026_LunaExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int GoldLeo = 8379983;
            public const int SilverHound = 35763582;
            public const int BlackSheep = 11317977;
            public const int KaleidoChick = 35618217;
            public const int YellowMarten = 50546208;
            public const int EmeraldBird = 14152693;
            public const int Wolf = 47705572;
            public const int Tiger = 83190280;
            public const int TriBrigadeFraktall = 87209160;

            // Hand Traps & Disruptions
            public const int MulcharmyFuwalos = 42141493;
            public const int MulcharmyPurulia = 84192580;
            public const int AshBlossom = 14558128;
            public const int AshBlossomAlt = 14558127;
            public const int DrollAndLockBird = 94145021;

            // Spells & Traps
            public const int LunalightFusion = 87931906;
            public const int LunalightMasquerade = 2344618;
            public const int LunaLightPerfume = 48444114;
            public const int Tenki = 57103969;
            public const int Polymerization = 24094653;
            public const int ApexPolymerization = 44886582;
            public const int ForbiddenDroplet = 24299458;
            public const int CrossoutDesignator = 65681983;
            public const int TripleTacticsTalent = 25311006;
            public const int FoolishBurial = 81439174;
            public const int CalledByTheGrave = 24224830;
            public const int DominusImpulse = 40366667;

            // Extra Deck
            public const int LigerDancer = 54701958;
            public const int LeoDancer = 24550676;
            public const int PerfumeDancer = 81196066;
            public const int PantherDancer = 97165977;
            public const int SabreDancer = 88753594;
            public const int TigerKing = 96381979;
            public const int Dugares = 66011101;
            public const int Nyarla = 8809344;
            public const int Bagooska = 90590304;
            public const int Almiraj = 60303245;
            public const int SpLittleKnight = 29301450;
            public const int GravityController = 23656668;

            // Side/Other
            public const int ArtifactLancea = 34267821;
            public const int HarpiesFeatherDuster = 18144507;
            public const int HarpiesFeatherDusterAlt = 18144506;
            public const int LightningStorm = 14532163;
            public const int HeavyPolymerization = 58570206;
            public const int DarkRulerNoMore = 54693926;
            public const int TripleTacticsThrust = 35269904;
            public const int FoolishBurialGoods = 35726888;
            public const int MaskOfRestrict = 29549364;
            public const int SerenadeDance = 13935001;
        }

        private static readonly int[] AceCardIds = {
            CardId.LigerDancer,
            CardId.LeoDancer,
            CardId.SabreDancer,
            CardId.PantherDancer
        };

        // Once per turn state flags
        private bool _goldLeoSummonUsed;
        private bool _perfumeDancerBounceUsed;
        private bool _perfumeDancerGyUsed;
        private bool _martenGyUsed;
        private bool _martenBounceUsed;
        private bool _chickGraveUsed;
        private bool _chickSendUsed;
        private bool _emeraldSummonUsed;
        private bool _emeraldGraveUsed;
        private bool _perfumeSearchUsed;
        private bool _apexPolyUsed;
        private bool _heavyPolyUsed;
        private bool _fraktallUsed;

        public _2026_LunaExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // Register Ace Cards to protect them from being used suboptimally
            ResourcePlan.RegisterAceCards(AceCardIds);

            // โ”€โ”€ Combo Router: Sequencing โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Standard-Setup",
                RequiredCards = new List<int> { CardId.GoldLeo, CardId.BlackSheep },
                FallbackLineName = "Luna-Tenki-Fallback",
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.GoldLeo, ActionType = ExecutorType.Activate, Description = "Play CardId.GoldLeo" },
                    new() { CardId = CardId.BlackSheep, ActionType = ExecutorType.Activate, Description = "Extend with CardId.BlackSheep" }
                },
                EndBoardScore = 80
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Luna-Tenki-Fallback",
                RequiredCards = new List<int> { CardId.Tenki },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.Tenki, ActionType = ExecutorType.Activate, Description = "Activate Tenki to search Tiger" }
                },
                EndBoardScore = 60,
                Condition = () => Bot.Hand.Any(c => c != null && c.Id == CardId.Tenki)
            });

            // โ”€โ”€ Bait Planner โ”€โ”€
            BaitPlanner.RegisterComboStarters(CardId.GoldLeo, CardId.SilverHound);
            BaitPlanner.RegisterBaitCards(CardId.SilverHound);

            // โ”€โ”€ Chain Advisor โ”€โ”€
            ChainAdvisor.RegisterHighValueTargets(CardId.GoldLeo, CardId.SilverHound, CardId.SpLittleKnight);

            // โ•โ•โ• TIER 1: Hand Traps & Chain Negations โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyPurulia, MulcharmyEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAlt, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.CrossoutDesignator, CrossoutDesignatorEffect);
            AddExecutor(ExecutorType.Activate, CardId.DominusImpulse, DominusImpulseEffect);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletEffect);

            // โ•โ•โ• TIER 2: Board Breakers โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, DefaultHarpiesFeatherDusterFirst);
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDusterAlt, DefaultHarpiesFeatherDusterFirst);
            AddExecutor(ExecutorType.Activate, CardId.LightningStorm);
            AddExecutor(ExecutorType.Activate, CardId.DarkRulerNoMore);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentEffect);

            // โ•โ•โ• TIER 3: Searchers & Setup Spells โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.Tenki, TenkiEffect);
            AddExecutor(ExecutorType.Activate, CardId.FoolishBurial, FoolishBurialEffect);
            AddExecutor(ExecutorType.Activate, CardId.FoolishBurialGoods, FoolishBurialGoodsEffect);
            AddExecutor(ExecutorType.Activate, CardId.LunaLightPerfume, LunaLightPerfumeEffect);

            // โ•โ•โ• TIER 4: Main Archetype Engines โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.TriBrigadeFraktall, TriBrigadeFraktallEffect);
            AddExecutor(ExecutorType.Activate, CardId.BlackSheep, BlackSheepEffect);
            AddExecutor(ExecutorType.Activate, CardId.LunalightMasquerade, LunalightMasqueradeEffect);
            AddExecutor(ExecutorType.Activate, CardId.Tiger, TigerPendulumEffect);
            AddExecutor(ExecutorType.Activate, CardId.Wolf, WolfPendulumEffect);

            // โ•โ•โ• TIER 5: Main Deck Monster Summons & Effects โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.GoldLeo, GoldLeoEffect);

            AddExecutor(ExecutorType.Activate, CardId.SilverHound, SilverHoundEffect);

            AddExecutor(ExecutorType.Activate, CardId.KaleidoChick, KaleidoChickEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.YellowMarten, YellowMartenSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.YellowMarten, YellowMartenEffect);

            AddExecutor(ExecutorType.Activate, CardId.EmeraldBird, EmeraldBirdEffect);

            // โ•โ•โ• TIER 6: Polymerization & Fusion Summons โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.ApexPolymerization, ApexPolymerizationEffect);
            AddExecutor(ExecutorType.Activate, CardId.HeavyPolymerization, HeavyPolymerizationEffect);
            AddExecutor(ExecutorType.Activate, CardId.LunalightFusion, LunalightFusionEffect);
            AddExecutor(ExecutorType.Activate, CardId.Polymerization, LunalightFusionEffect);

            // โ•โ•โ• TIER 7: Extra Deck Summons & Trigger Effects โ•โ•โ•
            AddExecutor(ExecutorType.SpSummon, CardId.LigerDancer, LigerDancerSummon);
            AddExecutor(ExecutorType.Activate, CardId.LigerDancer, LigerDancerEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.LeoDancer, LeoDancerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.PantherDancer, PantherDancerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SabreDancer, SabreDancerSummon);

            AddExecutor(ExecutorType.SpSummon, CardId.PerfumeDancer, PerfumeDancerSummon);
            AddExecutor(ExecutorType.Activate, CardId.PerfumeDancer, PerfumeDancerEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.Dugares, DugaresSummon);
            AddExecutor(ExecutorType.Activate, CardId.Dugares, DugaresEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.TigerKing, TigerKingSummon);
            AddExecutor(ExecutorType.Activate, CardId.TigerKing, TigerKingEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.Bagooska, BagooskaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Almiraj, AlmirajSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SpLittleKnight, SpLittleKnightSummon);
            AddExecutor(ExecutorType.Activate, CardId.SpLittleKnight);
            AddExecutor(ExecutorType.SpSummon, CardId.GravityController, GravityControllerSummon);

            // โ•โ•โ• TIER 8: Fallback Summons & Sets โ•โ•โ•
            AddExecutor(ExecutorType.Summon, FallbackNormalSummon);
            AddExecutor(ExecutorType.SpellSet, SetTrapCondition);
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // Lunalight is an OTK deck โ€” prefer going second
            return false;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _goldLeoSummonUsed = false;
            _perfumeDancerBounceUsed = false;
            _perfumeDancerGyUsed = false;
            _martenGyUsed = false;
            _martenBounceUsed = false;
            _chickGraveUsed = false;
            _chickSendUsed = false;
            _emeraldSummonUsed = false;
            _emeraldGraveUsed = false;
            _perfumeSearchUsed = false;
            _apexPolyUsed = false;
            
            if (ShouldGoBreakBoard)
            {
                // Going second Lunalight: prioritize Panther Dancer OTK
                _apexPolyUsed = false;
                _heavyPolyUsed = false;
            }
        }

        protected override bool IsBoardStrongEnough()
        {
            int count = 0;
            if (Bot.HasInMonstersZone(CardId.LigerDancer)) count += 3;
            if (Bot.HasInMonstersZone(CardId.LeoDancer)) count += 2;
            if (Bot.HasInMonstersZone(CardId.SpLittleKnight)) count++;
            if (Bot.HasInMonstersZone(CardId.Bagooska)) count += 2;
            if (Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.DominusImpulse))) count++;
            if (Bot.HasInHand(CardId.AshBlossom) || Bot.HasInHand(CardId.AshBlossomAlt)) count++;
            return count >= 3;
        }

        protected override bool ShouldStopExtending()
        {
            if (IsBoardStrongEnough())
                return base.ShouldStopExtending();
            return false;
        }

        public override bool ShouldAllowSpSummon(ClientCard card)
        {
            if (!base.ShouldAllowSpSummon(card)) return false;
            if (card == null) return true;

            if (card.HasType(CardType.Link | CardType.Xyz))
            {
                var activeAces = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && IsAceCard(c)).ToList();
                if (activeAces.Count > 0)
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
                        DecisionTracer.TraceSkip("ShouldAllowSpSummon", $"Summoning {card.Name} is not safe (would consume Ace card(s))");
                        return false;
                    }
                }
            }
            return true;
        }

        private bool IsLunalightCard(int id)
        {
            return id == CardId.GoldLeo || id == CardId.SilverHound || id == CardId.BlackSheep ||
                   id == CardId.KaleidoChick || id == CardId.YellowMarten || id == CardId.EmeraldBird ||
                   id == CardId.Wolf || id == CardId.Tiger || id == CardId.LunalightFusion ||
                   id == CardId.LunalightMasquerade || id == CardId.LunaLightPerfume ||
                   id == CardId.LigerDancer || id == CardId.LeoDancer || id == CardId.PerfumeDancer ||
                   id == CardId.PantherDancer || id == CardId.SabreDancer || id == CardId.SerenadeDance;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ• TIER 1 & 2: Disruptions & Staples โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool MulcharmyEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }

        private bool DrollAndLockBirdEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1 && Duel.LastChainPlayer == 1;
        }

        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard == null || LastChainCard.Controller != 1) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool CalledByTheGraveEffect()
        {
            if (LastChainCard == null || LastChainCard.Controller != 1) return false;
            return DefaultCalledByTheGrave();
        }

        private bool DominusImpulseEffect()
        {
            if (Card.Location == CardLocation.Hand) return false;
            if (LastChainCard == null || LastChainCard.Controller != 1) return false;
            return true;
        }

        private bool CrossoutDesignatorEffect()
        {
            // NEVER chain to own cards โ€” Crossout crashes engine when mis-timed
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard == null || LastChainCard.Controller != 1) return false;

            // Resolve alias (alt-art handling) โ€” critical for engine compatibility
            int code = LastChainCard.Id;
            int alias = LastChainCard.Alias;
            if (alias != 0 && alias - code < 10) code = alias;
            if (code == 0) return false;

            // Only negate monster effects (Crossout can't negate S/T effects practically)
            if (!LastChainCard.IsMonster()) return false;

            // Verify card is still in deck to avoid engine crash on resolve
            if (GetRemainingCount(code) > 0)
            {
                AI.SelectAnnounceID(code);
                return true;
            }
            return false;
        }

        private bool ForbiddenDropletEffect()
        {
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1)
            {
                var targets = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsDisabled() && c.HasType(CardType.Effect)).ToList();
                if (targets.Count > 0)
                {
                    return true;
                }
            }

            if (LastChainCard != null && LastChainCard.Controller == 1)
            {
                var targets = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsDisabled()).ToList();
                if (targets.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private bool TripleTacticsTalentEffect()
        {
            if (Enemy.GetMonsterCount() > 0 && _isGoingSecond)
            {
                AI.SelectOption(1);
            }
            else
            {
                AI.SelectOption(0);
            }
            return true;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ• TIER 3 & 4: Search & Setup Spells โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool TenkiEffect()
        {
            if (ShouldSkipCombo()) return false;
            bool hasTiger = Bot.Hand.Any(c => c != null && c.IsCode(CardId.Tiger)) || Bot.HasInSpellZone(CardId.Tiger);
            if (!hasTiger)
            {
                AI.SelectCard(CardId.Tiger, CardId.KaleidoChick, CardId.GoldLeo, CardId.YellowMarten);
            }
            else
            {
                AI.SelectCard(CardId.KaleidoChick, CardId.GoldLeo, CardId.YellowMarten, CardId.SilverHound);
            }
            return true;
        }

        private bool FoolishBurialEffect()
        {
            AI.SelectCard(CardId.SilverHound, CardId.YellowMarten, CardId.EmeraldBird);
            return true;
        }

        private bool FoolishBurialGoodsEffect()
        {
            AI.SelectCard(CardId.SerenadeDance, CardId.LunaLightPerfume);
            return true;
        }

        private bool LunaLightPerfumeEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.Hand)
            {
                var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCanRevive() && IsLunalightCard(c.Id));
                if (target != null)
                {
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave && !_perfumeSearchUsed)
            {
                if (Bot.Hand.Count > 0)
                {
                    _perfumeSearchUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool TriBrigadeFraktallEffect()
        {
            if (_fraktallUsed) return false;
            AI.SelectCard(CardId.Tiger, CardId.BlackSheep);
            _fraktallUsed = true;
            return true;
        }

        private bool BlackSheepEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                bool hasPoly = Bot.Hand.Any(c => c != null && c.IsCode(CardId.Polymerization, CardId.LunalightFusion, CardId.HeavyPolymerization));
                if (!hasPoly)
                {
                    AI.SelectOption(0); // Search Polymerization from Deck
                }
                else
                {
                    AI.SelectOption(1); // Add Lunalight from GY
                }
                return true;
            }
            return false;
        }

        private bool LunalightMasqueradeEffect()
        {
            if (Card.Location == CardLocation.Hand || Card.IsFacedown())
            {
                return true;
            }
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                // Can activate ignition effect to dump if activated this turn
                return StartingDeck.Cards.Any(id => { var c = YGOSharp.OCGWrapper.NamedCard.Get(id); return c != null && IsLunalightCard(id) && c.HasType(CardType.Monster) && GetRemainingCount(id) > 0; });
            }
            return false;
        }

        private bool TigerPendulumEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Always place Tiger in scale if we don't have it scaled
                return !Bot.HasInSpellZone(CardId.Tiger);
            }
            if (Card.Location == CardLocation.SpellZone)
            {
                var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCanRevive() && IsLunalightCard(c.Id));
                if (target != null)
                {
                    AI.SelectCard(target.Id);
                    return true;
                }
            }
            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ• TIER 5: Main Deck Monsters โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool GoldLeoEffect()
        {
            if (_goldLeoSummonUsed) return false;
            
            // Search-and-discard triggers on summon
            if (Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(CardId.Tiger, CardId.KaleidoChick, CardId.YellowMarten, CardId.Wolf);
                _goldLeoSummonUsed = true;
                return true;
            }
            return false;
        }

        private bool SilverHoundEffect()
        {
            // Negate Spell/Trap activation (Quick Effect in GY)
            if (Card.Location == CardLocation.Grave && LastChainCard != null && LastChainCard.Controller == 1)
            {
                if (LastChainCard.IsSpell() || LastChainCard.IsTrap())
                {
                    bool hasFusionInGy = Bot.Graveyard.Any(c => c != null && c.HasType(CardType.Fusion) && IsLunalightCard(c.Id));
                    if (hasFusionInGy)
                    {
                        return true;
                    }
                }
            }

            // Special Summon 1 Lunalight from Deck (Trigger in GY when sent there by card effect)
            if (Card.Location == CardLocation.Grave)
            {
                AI.SelectCard(CardId.KaleidoChick, CardId.GoldLeo, CardId.YellowMarten, CardId.EmeraldBird, CardId.BlackSheep);
                return true;
            }

            return false;
        }

        private bool KaleidoChickEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_chickSendUsed) return false;
                bool hasMaterialsForLeo = Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && IsLunalightCard(c.Id)) >= 2;
                if (hasMaterialsForLeo && GetRemainingCount(CardId.LeoDancer) > 0)
                {
                    AI.SelectCard(CardId.PantherDancer);
                }
                else
                {
                    AI.SelectCard(CardId.YellowMarten, CardId.SerenadeDance, CardId.BlackSheep);
                }
                _chickSendUsed = true;
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                if (_chickGraveUsed) return false;
                _chickGraveUsed = true;
                return true;
            }
            return false;
        }

        private bool YellowMartenSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (_martenBounceUsed) return false;
            
            ClientCard target = Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.Tiger));
            if (target == null)
            {
                target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && IsLunalightCard(c.Id) && !IsAceCard(c));
            }
            
            if (target != null)
            {
                AI.SelectCard(target.Id);
                _martenBounceUsed = true;
                return true;
            }
            return false;
        }

        private bool YellowMartenEffect()
        {
            if (Card.Location == CardLocation.Grave && !_martenGyUsed)
            {
                AI.SelectCard(CardId.LunaLightPerfume, CardId.LunalightFusion, CardId.SerenadeDance);
                _martenGyUsed = true;
                return true;
            }
            return false;
        }

        private bool EmeraldBirdEffect()
        {
            if (Card.Location == CardLocation.MonsterZone && !_emeraldSummonUsed)
            {
                if (Bot.Hand.Count > 0)
                {
                    AI.SelectCard(CardId.YellowMarten, CardId.SerenadeDance, CardId.BlackSheep);
                    _emeraldSummonUsed = true;
                    return true;
                }
            }
            if (Card.Location == CardLocation.Grave && !_emeraldGraveUsed)
            {
                var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCanRevive() && IsLunalightCard(c.Id) && c.Level <= 4);
                if (target != null)
                {
                    AI.SelectCard(target.Id);
                    _emeraldGraveUsed = true;
                    return true;
                }
            }
            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ• TIER 6: Fusion Spells โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool ApexPolymerizationEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.LifePoints <= 2000) return false;
            if (_apexPolyUsed) return false;

            var target = Bot.MonsterZone.GetMonsters().FirstOrDefault(c => 
                c != null && c.IsFaceup() && c.IsCode(CardId.KaleidoChick, CardId.GoldLeo, CardId.SilverHound, CardId.YellowMarten, CardId.EmeraldBird));
            
            if (target != null)
            {
                AI.SelectCard(target.Id);
                AI.SelectOption(1); // Always send to GY to set up Graveyard Fusions
                _apexPolyUsed = true;
                return true;
            }
            return false;
        }

        private bool HeavyPolymerizationEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.LifePoints <= 3000) return false; // Avoid suicide
            if (_heavyPolyUsed) return false;
            
            if (Enemy.GetMonsterCount() > 0)
            {
                _heavyPolyUsed = true;
                return true;
            }
            return false;
        }

        private bool CanFusionSummonSafely(int targetId, bool isLunalightFusion, bool isWolf)
        {
            var locations = new List<ClientCard>();
            
            if (isWolf)
            {
                locations.AddRange(Bot.MonsterZone.GetMonsters().Where(c => c != null && IsLunalightCard(c.Id)));
                locations.AddRange(Bot.Graveyard.Where(c => c != null && IsLunalightCard(c.Id)));
            }
            else
            {
                locations.AddRange(Bot.Hand.Where(c => c != null && IsLunalightCard(c.Id)));
                locations.AddRange(Bot.MonsterZone.GetMonsters().Where(c => c != null && IsLunalightCard(c.Id)));
            }

            bool canUseDeckExtra = isLunalightFusion && Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsSpecialSummoned && (c.HasType(CardType.Fusion) || c.HasType(CardType.Xyz) || c.HasType(CardType.Synchro) || c.HasType(CardType.Link)));
            
            var safeMats = locations.Where(c => {
                if (c == null) return false;
                if (IsAceCard(c))
                {
                    if (targetId == CardId.LeoDancer && c.IsCode(CardId.PantherDancer)) return true;
                    if (targetId == CardId.LigerDancer && c.IsCode(CardId.LeoDancer)) return true;
                    if (targetId == CardId.PantherDancer && c.IsCode(CardId.PerfumeDancer)) return true;
                    return false;
                }
                // Avoid using hand traps as fusion materials unless desperate
                if (c.Id == CardId.AshBlossom || c.Id == CardId.AshBlossomAlt || c.Id == CardId.DrollAndLockBird || c.Id == CardId.MulcharmyFuwalos || c.Id == CardId.MulcharmyPurulia)
                    return false;
                return true;
            }).ToList();

            int count = safeMats.Count;
            if (canUseDeckExtra) count += 1;

            if (targetId == CardId.LeoDancer)
            {
                bool hasPanther = safeMats.Any(c => c.IsCode(CardId.PantherDancer)) || (canUseDeckExtra && GetRemainingCount(CardId.PantherDancer) > 0);
                return hasPanther && count >= 3;
            }
            if (targetId == CardId.LigerDancer)
            {
                bool hasLeo = safeMats.Any(c => c.IsCode(CardId.LeoDancer)) || (canUseDeckExtra && GetRemainingCount(CardId.LeoDancer) > 0);
                return hasLeo && count >= 4;
            }
            if (targetId == CardId.PantherDancer)
            {
                bool hasPerfume = safeMats.Any(c => c.IsCode(CardId.PerfumeDancer)) || (canUseDeckExtra && GetRemainingCount(CardId.PerfumeDancer) > 0);
                return hasPerfume && count >= 2;
            }
            if (targetId == CardId.SabreDancer)
            {
                return count >= 3;
            }
            if (targetId == CardId.PerfumeDancer)
            {
                return count >= 2;
            }

            return false;
        }

        private bool WolfPendulumEffect()
        {
            if (IsSpecialSummonBlocked()) return false;

            if (Card.Location == CardLocation.Hand)
            {
                if (Bot.HasInSpellZone(CardId.Wolf)) return false;
                if (CanFusionSummonSafely(CardId.LigerDancer, false, true) ||
                    CanFusionSummonSafely(CardId.LeoDancer, false, true) ||
                    CanFusionSummonSafely(CardId.PantherDancer, false, true) ||
                    CanFusionSummonSafely(CardId.SabreDancer, false, true) ||
                    CanFusionSummonSafely(CardId.PerfumeDancer, false, true))
                {
                    return true;
                }
                return false;
            }
            
            if (Card.Location == CardLocation.SpellZone)
            {
                if (CanFusionSummonSafely(CardId.LigerDancer, false, true) && GetRemainingCount(CardId.LigerDancer) > 0)
                {
                    AI.SelectCard(CardId.LigerDancer);
                    return true;
                }
                if (CanFusionSummonSafely(CardId.LeoDancer, false, true) && GetRemainingCount(CardId.LeoDancer) > 0)
                {
                    AI.SelectCard(CardId.LeoDancer);
                    return true;
                }
                if (CanFusionSummonSafely(CardId.PantherDancer, false, true) && GetRemainingCount(CardId.PantherDancer) > 0)
                {
                    AI.SelectCard(CardId.PantherDancer);
                    return true;
                }
                if (CanFusionSummonSafely(CardId.SabreDancer, false, true) && GetRemainingCount(CardId.SabreDancer) > 0)
                {
                    AI.SelectCard(CardId.SabreDancer);
                    return true;
                }
                if (CanFusionSummonSafely(CardId.PerfumeDancer, false, true) && GetRemainingCount(CardId.PerfumeDancer) > 0)
                {
                    AI.SelectCard(CardId.PerfumeDancer);
                    return true;
                }
            }
            
            return false;
        }

        private bool LunalightFusionEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            
            bool isLunalightFusion = Card.IsCode(CardId.LunalightFusion);
            
            if (CanFusionSummonSafely(CardId.LigerDancer, isLunalightFusion, false) && GetRemainingCount(CardId.LigerDancer) > 0)
            {
                AI.SelectCard(CardId.LigerDancer);
                return true;
            }
            if (CanFusionSummonSafely(CardId.LeoDancer, isLunalightFusion, false) && GetRemainingCount(CardId.LeoDancer) > 0)
            {
                AI.SelectCard(CardId.LeoDancer);
                return true;
            }
            if (CanFusionSummonSafely(CardId.PantherDancer, isLunalightFusion, false) && GetRemainingCount(CardId.PantherDancer) > 0)
            {
                AI.SelectCard(CardId.PantherDancer);
                return true;
            }
            if (CanFusionSummonSafely(CardId.SabreDancer, isLunalightFusion, false) && GetRemainingCount(CardId.SabreDancer) > 0)
            {
                AI.SelectCard(CardId.SabreDancer);
                return true;
            }
            if (CanFusionSummonSafely(CardId.PerfumeDancer, isLunalightFusion, false) && GetRemainingCount(CardId.PerfumeDancer) > 0)
            {
                AI.SelectCard(CardId.PerfumeDancer);
                return true;
            }
            
            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ• TIER 7: Extra Deck Monsters โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool LigerDancerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool LigerDancerEffect()
        {
            if (Enemy.GetMonsters().Any(c => c != null && c.IsSpecialSummoned))
            {
                AI.SelectCard(CardId.SabreDancer, CardId.PerfumeDancer, CardId.PantherDancer);
                return true;
            }
            return false;
        }

        private bool LeoDancerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool PantherDancerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool SabreDancerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool PerfumeDancerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool PerfumeDancerEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_perfumeDancerBounceUsed) return false;
                
                ClientCard target = Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.Tiger));
                if (target == null)
                {
                    target = Bot.MonsterZone.GetMonsters().FirstOrDefault(c => c != null && c != Card && IsLunalightCard(c.Id) && !IsAceCard(c));
                }

                if (target != null && Bot.Hand.Any(c => c != null && IsLunalightCard(c.Id)))
                {
                    AI.SelectCard(target.Id);
                    var summon = Bot.Hand.FirstOrDefault(c => c != null && IsLunalightCard(c.Id));
                    if (summon != null)
                    {
                        AI.SelectNextCard(summon.Id);
                    }
                    _perfumeDancerBounceUsed = true;
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (_perfumeDancerGyUsed) return false;
                if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2) && Enemy.GetMonsterCount() > 0)
                {
                    _perfumeDancerGyUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool TigerKingSummon()
        {
            return !Bot.HasInSpellZone(CardId.Tenki) && Bot.GetRemainingCount(CardId.Tenki, 1) > 0;
        }

        private bool TigerKingEffect()
        {
            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled() && !c.HasRace(CardRace.BestWarrior));
        }

        private bool DugaresSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.Graveyard.Any(c => c != null && c.IsCanRevive() && IsLunalightCard(c.Id));
        }

        private bool DugaresEffect()
        {
            var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCanRevive() && IsLunalightCard(c.Id));
            if (target != null)
            {
                AI.SelectOption(0); // Option 1 (index 0) Special Summon from GY
                AI.SelectCard(target.Id);
                return true;
            }
            return false;
        }

        private bool BagooskaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if ((Duel.Turn == 1 || Duel.Phase == DuelPhase.Main2) && Duel.Player == 0)
            {
                AI.SelectPosition(CardPosition.FaceUpDefence);
                return true;
            }
            return false;
        }

        private bool AlmirajSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            
            // Only summon Almiraj if we have a way to revive the material (Tiger or Perfume)
            bool hasRevival = Bot.Hand.Any(c => c != null && c.IsCode(CardId.LunaLightPerfume, CardId.Tiger)) ||
                              Bot.HasInSpellZone(CardId.Tiger);
            if (!hasRevival) return false;

            var mat = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.BlackSheep, CardId.KaleidoChick));
            if (mat != null)
            {
                AI.SelectCard(mat.Id);
                return true;
            }
            return false;
        }

        private bool SpLittleKnightSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            
            // Only summon if we have at least 2 non-Ace monsters to use as materials
            int nonAceCount = Bot.MonsterZone.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            if (nonAceCount < 2) return false;
            
            // Do not link away our monsters in Main Phase 1 of our turn (prefer fusions)
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1) return false;
            
            return Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
        }

        private bool GravityControllerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var emzCard = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Location == CardLocation.MonsterZone && (c.Sequence == 5 || c.Sequence == 6));
            if (emzCard != null && IsLunalightCard(emzCard.Id) && !IsAceCard(emzCard))
            {
                AI.SelectCard(emzCard.Id);
                return true;
            }
            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ• Fallbacks & Helpers โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool SetTrapCondition() => Util.IsTurn1OrMain2();

        private bool FallbackNormalSummon()
        {
            if (Card != null && (Card.Id == CardId.AshBlossom || Card.Id == CardId.AshBlossomAlt || 
                                 Card.Id == CardId.DrollAndLockBird || Card.Id == CardId.MulcharmyFuwalos || 
                                 Card.Id == CardId.MulcharmyPurulia))
                return false;
            return true;
        }

        private bool IsSafeToAttack(ClientCard attacker)
        {
            foreach (ClientCard enemy in Enemy.MonsterZone)
            {
                if (enemy == null || !enemy.IsFaceup()) continue;
                if (enemy.IsAttack() && enemy.Attack >= attacker.Attack) return false;
                if (enemy.IsDefense() && attacker.Attack <= enemy.Defense) return false;
            }
            return true;
        }

        private bool IsSafeToDefend(ClientCard monster)
        {
            foreach (ClientCard enemy in Enemy.MonsterZone)
            {
                if (enemy == null || !enemy.IsFaceup()) continue;
                if (enemy.Attack > monster.Defense) return false;
            }
            return true;
        }

        private bool MonsterRepos()
        {
            if (Card == null) return false;
            if (IsAceCard(Card))
            {
                if (Card.IsDefense()) return true; // Switch to ATK
                return false;
            }
            
            bool enemyEmpty = Enemy.GetMonsterCount() == 0;
            
            // Luna main deck monsters have very low ATK (100-1400) โ€” keep in DEF
            if (Card.Attack <= 1400 && Card.IsAttack())
                return true; // Switch to DEF
            
            if (Card.IsAttack())
            {
                if (!enemyEmpty && !IsSafeToAttack(Card) && IsSafeToDefend(Card))
                    return true;
            }
            else
            {
                if (enemyEmpty && Card.Attack >= 1500)
                    return true;
            }
            return false;
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Ace boss monsters go to ATK (Liger, Leo, Sabre, Panther Dancers)
            if ((cardId == CardId.LigerDancer || cardId == CardId.LeoDancer 
                || cardId == CardId.SabreDancer || cardId == CardId.PantherDancer
                || cardId == CardId.PerfumeDancer || cardId == CardId.TigerKing
                || cardId == CardId.Dugares || cardId == CardId.Bagooska
                || cardId == CardId.SpLittleKnight || cardId == CardId.Nyarla)
                && positions.Contains(CardPosition.FaceUpAttack))
                return CardPosition.FaceUpAttack;
            
            // Low ATK main deck monsters โ€” stay in DEF for safety
            if (positions.Contains(CardPosition.FaceUpDefence))
                return CardPosition.FaceUpDefence;
            
            return base.OnSelectPosition(cardId, positions);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.Id == CardId.AshBlossom || c.Id == CardId.AshBlossomAlt || c.Id == CardId.DrollAndLockBird || c.Id == CardId.MulcharmyFuwalos || c.Id == CardId.MulcharmyPurulia) return 800;
            if (c.Id == CardId.Wolf || c.Id == CardId.Tiger) return 150;
            if (c.Id == CardId.BlackSheep) return 100;
            return 50;
        }

        public override IList<ClientCard> OnSelectFusionMaterial(IList<ClientCard> cards, int min, int max)
        {
            var sorted = cards.OrderBy(GetFusionMaterialScore).ToList();
            DecisionTracer.Trace("OnSelectFusionMaterial", $"min: {min}, max: {max}, selected: {string.Join(", ", sorted.Take(max).Select(c => c.Name ?? c.Id.ToString()))}");
            return sorted.Take(max).ToList();
        }

        public override IList<ClientCard> OnSelectXyzMaterial(IList<ClientCard> cards, int min, int max)
        {
            var sorted = cards.OrderBy(c => {
                if (c == null) return 999;
                if (IsAceCard(c)) return 900;
                if (c.Id == CardId.AshBlossom || c.Id == CardId.AshBlossomAlt || c.Id == CardId.DrollAndLockBird || c.Id == CardId.MulcharmyFuwalos || c.Id == CardId.MulcharmyPurulia) return 800;
                if (c.Id == CardId.YellowMarten) return 10;
                if (c.Id == CardId.EmeraldBird) return 20;
                if (c.Id == CardId.KaleidoChick) return 30;
                return 100;
            }).ToList();
            DecisionTracer.Trace("OnSelectXyzMaterial", $"min: {min}, max: {max}, selected: {string.Join(", ", sorted.Take(max).Select(c => c.Name ?? c.Id.ToString()))}");
            return sorted.Take(max).ToList();
        }

        private int GetFusionMaterialScore(ClientCard c)
        {
            if (c == null) return 999;
            if (c.Controller == 1) return 10; // Super Poly / Lunalight Fusion from opponent's field/deck
            
            if (c.Id == CardId.AshBlossom || c.Id == CardId.AshBlossomAlt || c.Id == CardId.DrollAndLockBird || c.Id == CardId.MulcharmyFuwalos || c.Id == CardId.MulcharmyPurulia)
                return 800;

            if (c.Location == CardLocation.Grave)
            {
                if (c.Id == CardId.YellowMarten) return 20;
                if (c.Id == CardId.EmeraldBird) return 21;
                if (c.Id == CardId.KaleidoChick) return 22;
                if (c.Id == CardId.BlackSheep) return 23;
                return 30;
            }

            if (c.Location == CardLocation.Hand)
            {
                if (c.Id == CardId.BlackSheep) return 40;
                if (c.Id == CardId.GoldLeo || c.Id == CardId.SilverHound) return 45;
                if (c.Id == CardId.EmeraldBird) return 50;
                if (c.Id == CardId.YellowMarten) return 55;
                if (c.Id == CardId.KaleidoChick) return 60;
                return 70;
            }

            if (c.Location == CardLocation.MonsterZone)
            {
                if (IsAceCard(c)) return 500;
                if (c.Id == CardId.BlackSheep) return 100;
                if (c.Id == CardId.EmeraldBird) return 110;
                if (c.Id == CardId.KaleidoChick) return 120;
                if (c.Id == CardId.YellowMarten) return 130;
                return 150;
            }

            return 200;
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            DecisionTracer.Trace("OnSelectCard", $"hint: {hint}, min: {min}, max: {max}");

            if (hint == 533) // Link material
            {
                var sorted = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }
            if (hint == 511) // Fusion material
            {
                var sorted = cards.OrderBy(GetFusionMaterialScore).ToList();
                return sorted.Take(max).ToList();
            }
            if (hint == 513) // Xyz material
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (IsAceCard(c)) return 900;
                    if (c.Id == CardId.AshBlossom || c.Id == CardId.AshBlossomAlt || c.Id == CardId.DrollAndLockBird || c.Id == CardId.MulcharmyFuwalos || c.Id == CardId.MulcharmyPurulia) return 800;
                    if (c.Id == CardId.YellowMarten) return 10;
                    if (c.Id == CardId.EmeraldBird) return 20;
                    if (c.Id == CardId.KaleidoChick) return 30;
                    return 100;
                }).ToList();
                return sorted.Take(max).ToList();
            }
            if (hint == 509) // Special Summon / Revival
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    int controllerScore = (c.Controller == 0) ? 0 : 1000;
                    int cardScore = 100;
                    if (c.IsCode(CardId.LeoDancer)) cardScore = 1;
                    else if (c.IsCode(CardId.PantherDancer)) cardScore = 2;
                    else if (c.IsCode(CardId.SabreDancer)) cardScore = 3;
                    else if (c.IsCode(CardId.LigerDancer)) cardScore = 4;
                    else if (c.IsCode(CardId.PerfumeDancer)) cardScore = 5;
                    else if (c.IsCode(CardId.TigerKing)) cardScore = 6;
                    else if (c.IsCode(CardId.KaleidoChick)) cardScore = 7;
                    else if (c.IsCode(CardId.Tiger)) cardScore = 8;
                    else if (c.IsCode(CardId.YellowMarten)) cardScore = 9;
                    else if (c.IsCode(CardId.EmeraldBird)) cardScore = 10;
                    return controllerScore + cardScore;
                }).ToList();
                return sorted.Take(max).ToList();
            }
            if (hint == 502) // Destroy
            {
                var sorted = cards.OrderByDescending(c => {
                    if (c == null) return -999;
                    int score = (c.Controller == 1) ? 10000 : 0;
                    if (c.IsMonster())
                    {
                        if (c.IsFaceup() && !c.IsDisabled())
                        {
                            if (c.Attack >= 2500 && c.HasType(CardType.Effect)) return score + 5000;
                        }
                        return score + c.Attack;
                    }
                    else if (c.IsSpell() || c.IsTrap())
                    {
                        if (c.IsFaceup())
                        {
                            if (c.HasType(CardType.Continuous) || c.HasType(CardType.Field)) return score + 2000;
                            return score + 500;
                        }
                        return score + 100;
                    }
                    return score;
                }).ToList();
                return sorted.Take(max).ToList();
            }
            if (hint == 506) // Search
            {
                return SelectPreferred(cards, min, max, CardId.Tiger, CardId.KaleidoChick, CardId.YellowMarten, CardId.GoldLeo, CardId.SilverHound, CardId.LunalightFusion);
            }

            if (Card == null) return base.OnSelectCard(cards, min, max, hint, cancelable);

            // Card-specific overrides
            if (Card.Id == CardId.LunaLightPerfume)
            {
                if (cards.Any(c => c != null && c.Location == CardLocation.Hand && c.Controller == 0))
                {
                    return SelectPreferred(cards, min, max, CardId.SerenadeDance, CardId.YellowMarten, CardId.EmeraldBird, CardId.BlackSheep, CardId.SilverHound, CardId.GoldLeo);
                }
                if (cards.Any(c => c != null && c.Location == CardLocation.Grave && c.Controller == 0))
                {
                    return SelectPreferred(cards, min, max, CardId.KaleidoChick, CardId.YellowMarten, CardId.EmeraldBird, CardId.Tiger, CardId.GoldLeo, CardId.SilverHound, CardId.BlackSheep);
                }
                if (cards.Any(c => c != null && c.Location == CardLocation.Deck && c.Controller == 0))
                {
                    return SelectPreferred(cards, min, max, CardId.Tiger, CardId.KaleidoChick, CardId.YellowMarten, CardId.GoldLeo, CardId.SilverHound, CardId.Wolf);
                }
            }

            if (Card.Id == CardId.BlackSheep)
            {
                if (cards.Any(c => c != null && c.IsCode(CardId.BlackSheep) && c.Location == CardLocation.Hand))
                {
                    return cards.Where(c => c != null && c.IsCode(CardId.BlackSheep)).Take(max).ToList();
                }
                if (cards.Any(c => c != null && c.Location == CardLocation.Grave && c.Controller == 0))
                {
                    return SelectPreferred(cards, min, max, CardId.Tiger, CardId.KaleidoChick, CardId.YellowMarten, CardId.EmeraldBird, CardId.GoldLeo, CardId.SilverHound);
                }
            }

            if (Card.Id == CardId.LunalightMasquerade)
            {
                if (cards.Any(c => c != null && c.Location == CardLocation.Deck && c.Controller == 0))
                {
                    return SelectPreferred(cards, min, max, CardId.SilverHound, CardId.YellowMarten, CardId.SerenadeDance, CardId.EmeraldBird, CardId.BlackSheep, CardId.KaleidoChick);
                }
            }

            if (Card.Id == CardId.YellowMarten)
            {
                if (cards.Any(c => c != null && (c.Location == CardLocation.MonsterZone || c.Location == CardLocation.SpellZone)))
                {
                    return SelectPreferred(cards, min, max, CardId.Tiger, CardId.GoldLeo, CardId.SilverHound, CardId.EmeraldBird, CardId.BlackSheep);
                }
            }

            if (Card.Id == CardId.EmeraldBird)
            {
                if (cards.Any(c => c != null && c.Location == CardLocation.Hand && c.Controller == 0))
                {
                    return SelectPreferred(cards, min, max, CardId.SerenadeDance, CardId.YellowMarten, CardId.BlackSheep, CardId.GoldLeo, CardId.SilverHound);
                }
                if (cards.Any(c => c != null && c.Location == CardLocation.Grave && c.Controller == 0))
                {
                    return SelectPreferred(cards, min, max, CardId.KaleidoChick, CardId.YellowMarten, CardId.BlackSheep, CardId.GoldLeo, CardId.SilverHound);
                }
            }

            if (Card.Id == CardId.GoldLeo)
            {
                if (cards.Any(c => c != null && c.Location == CardLocation.Hand && c.Controller == 0))
                {
                    return SelectPreferred(cards, min, max, CardId.SerenadeDance, CardId.YellowMarten, CardId.SilverHound, CardId.EmeraldBird, CardId.BlackSheep);
                }
                if (cards.Any(c => c != null && c.Location == CardLocation.Grave && c.Controller == 0))
                {
                    return SelectPreferred(cards, min, max, CardId.Tiger, CardId.KaleidoChick, CardId.BlackSheep, CardId.GoldLeo, CardId.YellowMarten);
                }
            }

            if (Card.Id == CardId.ForbiddenDroplet)
            {
                if (cards.Any(c => c != null && c.Controller == 0 && (c.Location == CardLocation.Hand || c.Location == CardLocation.SpellZone || c.Location == CardLocation.MonsterZone)))
                {
                    return SelectPreferred(cards, min, max, CardId.Tenki, CardId.SerenadeDance, CardId.LunalightMasquerade, CardId.LunaLightPerfume);
                }
                if (cards.Any(c => c != null && c.Controller == 1 && c.Location == CardLocation.MonsterZone))
                {
                    // Prioritize negators like Blue-Eyes Spirit Dragon (59822133) and Hope Harbinger (63767246)
                    var preferredTargets = SelectPreferred(cards, min, max, 59822133, 63767246, 40908371);
                    if (preferredTargets.Count > 0)
                    {
                        return preferredTargets;
                    }
                    return cards.Where(c => c != null && c.IsFaceup() && !c.IsDisabled()).OrderByDescending(c => c.Attack).Take(max).ToList();
                }
            }

            if (Card.Id == CardId.ApexPolymerization)
            {
                if (cards.Any(c => c != null && c.Location == CardLocation.MonsterZone && c.Controller == 0))
                {
                    return SelectPreferred(cards, min, max, CardId.KaleidoChick, CardId.GoldLeo, CardId.SilverHound, CardId.YellowMarten, CardId.EmeraldBird);
                }
                if (cards.Any(c => c != null && c.Location == CardLocation.Extra && c.Controller == 0))
                {
                    return SelectPreferred(cards, min, max, CardId.LeoDancer, CardId.PantherDancer, CardId.SabreDancer, CardId.PerfumeDancer);
                }
            }

            if (Card.Id == CardId.LigerDancer)
            {
                return SelectPreferred(cards, min, max, CardId.SabreDancer, CardId.PerfumeDancer, CardId.PantherDancer);
            }

            if (Card != null && (Card.Id == CardId.LunalightFusion || Card.Id == CardId.Polymerization || Card.Id == CardId.HeavyPolymerization))
            {
                if (cards.Any(c => c != null && c.Location == CardLocation.Extra && c.Controller == 0))
                {
                    return SelectPreferred(cards, min, max, CardId.LigerDancer, CardId.LeoDancer, CardId.PantherDancer, CardId.SabreDancer, CardId.PerfumeDancer);
                }
                if (cards.Any(c => c != null && c.Location == CardLocation.Deck && c.Controller == 0))
                {
                    return SelectPreferred(cards, min, max, CardId.YellowMarten, CardId.SerenadeDance, CardId.SilverHound, CardId.EmeraldBird, CardId.BlackSheep);
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        private IList<ClientCard> SelectPreferred(IList<ClientCard> cards, int min, int max, params int[] preferredIds)
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

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.IsCode(CardId.LigerDancer, CardId.LeoDancer, CardId.SabreDancer, CardId.PantherDancer);
        }
    }
}
