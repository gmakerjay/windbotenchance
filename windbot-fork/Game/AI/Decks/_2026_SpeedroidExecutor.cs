// ============================================================
// CARD AUDIT โ€” 2026_Speedroid
// | Card Name           | Type    | OPT? | Cost      | Effect              | Activate When        | NEVER When           |
// |---------------------|---------|------|-----------|---------------------|----------------------|----------------------|
// | Speedroid Terrortop | Monster | HOPT | None      | SS itself, search   | Early turn setup     | Already control card |
// | Speedroid Taketomborg| Monster | HOPT | Tribute   | SS Speedroid Tuner  | Extend synchro combos| Wind lock violates   |
// ACE CARDS: Primary: Crystal Wing Synchro Dragon / Secondary: Hi-Speedroid Kitedrake
// COMBO STARTERS: 1. Speedroid Terrortop 2. Speedroid Marble Machine
// CHOKEPOINTS: Taketomborg effect negated
// WIN CONDITION: High-level Synchro plays (Crystal Wing, Baron, Kitedrake) to dominate board
// GOING 1ST END BOARD: Crystal Wing Synchro Dragon + Baronne de Fleur
// GOING 2ND GAMEPLAN: Clear board with Kitedrake, then push for game with high ATK Synchros
// ============================================================

// ============================================================
// COMBO DRAFT โ€” 2026_Speedroid
// ============================================================
// === COMBO LINE 1: Standard Synchro Play (Starter: Terrortop) ===
// HAND REQUIRED: Speedroid Terrortop + Speedroid Taketomborg
// STEP 1: Special Summon Speedroid Terrortop from hand (if control no monsters)
// STEP 2: Terrortop Effect: Add Taketomborg to hand
// STEP 3: Special Summon Taketomborg from hand
// STEP 4: Tribute Taketomborg to Special Summon a Tuner (e.g. Red-Eyed Dice)
// STEP 5: Synchro Summon Crystal Wing Synchro Dragon
// END BOARD: Crystal Wing on field
// === COMBO LINE 2: Going 2nd Board Wipe ===
// STEP 1: Special Summon/Normal Summon starters
// STEP 2: Synchro Summon Hi-Speedroid Kitedrake
// STEP 3: Kitedrake Effect: Negate all other card effects or destroy all cards on field
// ============================================================

using YGOSharp.OCGWrapper.Enums;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using System;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_Speedroid", "2026_Speedroid")]
    public class _2026_SpeedroidExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck
            public const int Fonix = 85315450;
            public const int VibrantVortex = 53927851;
            public const int Krosea = 16922142;
            public const int Meghala = 27755794;
            public const int Swen = 80538047;
            public const int Eldam = 54143349;
            public const int NobleKnightsShieldBearer = 34242278;
            public const int SpeedroidFukiModoshiPiper = 50482813;
            public const int SpeedroidDenDenDaikoDuke = 59640711;
            public const int Shiina = 12197223;
            public const int MulcharmyFuwalos = 42141493;
            public const int Chant = 67115133;
            public const int Vision = 20508881;
            public const int Manifestation = 94103142;
            public const int Ascendance = 25940932;
            public const int MysticalSpaceTyphoon = 5318639;
            public const int CalledByTheGrave = 24224830;
            public const int TheFallenAndTheVirtuous = 30271097;
            public const int ForbiddenDroplet = 24299458;
            public const int ForbiddenCrown = 98829635;
            public const int PotOfProsperity = 84211599;
            public const int Mandate = 53813120;

            // Extra Deck
            public const int Typhon = 93039339;
            public const int TotemBird = 71068247;
            public const int AlbionTheBrandedDragon = 87746184;
            public const int EcclesiaAndTheDarkDragon = 78397661;
            public const int WindPegasusIgnister = 98506199;
            public const int DragunityKnightAreadbhair = 88234821;
            public const int DragunityKnightTrident = 80159717;
            public const int DragunityKnightLuin = 12496261;
            public const int DragunityLordGeorgius = 70522875;
            public const int RubberBandShooter = 72813401;
            public const int LinkVaruroon = 39341885;
            public const int DoomEagle = 49105782;
            public const int WynnTheWindCharmerVerdant = 30674956;
            public const int Greatfly = 90512490;
        }

        // --- Once Per Turn Tracking ---
        private bool _kroseaHandUsed = false;
        private bool _kroseaSummonUsed = false;
        private bool _vibrantVortexHandUsed = false;
        private bool _vibrantVortexNegateUsed = false;
        private bool _eldamSummonUsed = false;
        private bool _swenSummonUsed = false;
        private bool _meghalaSpUsed = false;
        private bool _visionUsed = false;
        private bool _chantUsed = false;
        private bool _ascendanceUsed = false;
        private bool _mandateUsed = false;
        private bool _manifestationUsed = false;

        private bool _rubberBandShooterBanishUsed = false;
        private bool _linkVaruroonSummonUsed = false;
        private bool _linkVaruroonTrapPlaceUsed = false;
        private bool _windLocked = false;

        public _2026_SpeedroidExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Register Ace Cards to protect them from being material / tributed
            HeuristicGuard.RegisterAceCards(
                CardId.VibrantVortex,
                CardId.Fonix,
                CardId.DragunityKnightAreadbhair,
                CardId.DragunityLordGeorgius,
                CardId.LinkVaruroon,
                CardId.Typhon
            );
            ResourcePlan.RegisterAceCards(
                CardId.VibrantVortex,
                CardId.Fonix,
                CardId.DragunityKnightAreadbhair,
                CardId.DragunityLordGeorgius,
                CardId.LinkVaruroon,
                CardId.Typhon
            );
            // โ”€โ”€ Combo Router: Sequencing โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Standard-Setup",
                RequiredCards = new List<int> { CardId.SpeedroidFukiModoshiPiper, CardId.Fonix },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.SpeedroidFukiModoshiPiper, ActionType = ExecutorType.Activate, Description = "Play CardId.SpeedroidFukiModoshiPiper" },
                    new() { CardId = CardId.Fonix, ActionType = ExecutorType.Activate, Description = "Extend with CardId.Fonix" }
                },
                EndBoardScore = 80
            });

            // โ”€โ”€ Bait Planner โ”€โ”€
            BaitPlanner.RegisterComboStarters(CardId.SpeedroidFukiModoshiPiper);
            BaitPlanner.RegisterBaitCards(CardId.PotOfProsperity);

            // โ”€โ”€ Chain Advisor โ”€โ”€
            ChainAdvisor.RegisterHighValueTargets(CardId.SpeedroidFukiModoshiPiper);


            // TIER 1: Hand traps & counters
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosActivate);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletActivate);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenCrown, ForbiddenCrownActivate);

            // TIER 2: Quick Effects of Bosses & Disruptions
            AddExecutor(ExecutorType.Activate, CardId.DragunityKnightAreadbhair, AreadbhairActivate);
            AddExecutor(ExecutorType.Activate, CardId.VibrantVortex, VibrantVortexActivate);
            AddExecutor(ExecutorType.Activate, CardId.Fonix, FonixActivate);
            AddExecutor(ExecutorType.Activate, CardId.Shiina, ShiinaEffect);
            AddExecutor(ExecutorType.Activate, CardId.MysticalSpaceTyphoon, MysticalSpaceTyphoonActivate);
            AddExecutor(ExecutorType.Activate, CardId.TheFallenAndTheVirtuous, FallenAndVirtuousActivate);

            // TIER 3: Starters & Extenders
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, PotOfProsperityEffect);
            AddExecutor(ExecutorType.Activate, CardId.NobleKnightsShieldBearer, ShieldBearerActivate);
            AddExecutor(ExecutorType.Activate, CardId.SpeedroidFukiModoshiPiper, PiperActivate);
            AddExecutor(ExecutorType.Activate, CardId.SpeedroidDenDenDaikoDuke, DukeEffect);

            // TIER 4: Radiant starters summons & activations
            AddExecutor(ExecutorType.Activate, CardId.Krosea, KroseaActivate);
            AddExecutor(ExecutorType.Summon, CardId.Krosea, KroseaSummon);

            AddExecutor(ExecutorType.Activate, CardId.Eldam, EldamActivate);
            AddExecutor(ExecutorType.Summon, CardId.Eldam);

            AddExecutor(ExecutorType.Activate, CardId.Swen, SwenActivate);
            AddExecutor(ExecutorType.Summon, CardId.Swen);

            AddExecutor(ExecutorType.Activate, CardId.Meghala, MeghalaActivate);
            AddExecutor(ExecutorType.Summon, CardId.Meghala);

            // TIER 5: Spells popped triggers & Continuous traps
            AddExecutor(ExecutorType.Activate, CardId.Vision, VisionActivate);
            AddExecutor(ExecutorType.Activate, CardId.Chant, ChantActivate);
            AddExecutor(ExecutorType.Activate, CardId.Ascendance, AscendanceActivate);
            AddExecutor(ExecutorType.Activate, CardId.Manifestation, ManifestationActivate);
            AddExecutor(ExecutorType.Activate, CardId.Mandate, MandateActivate);

            // TIER 6: Extra Deck Summons
            AddExecutor(ExecutorType.SpSummon, CardId.DragunityKnightAreadbhair, () => !IsSpecialSummonBlocked());
            AddExecutor(ExecutorType.SpSummon, CardId.DragunityKnightLuin, () => !IsSpecialSummonBlocked());
            AddExecutor(ExecutorType.Activate, CardId.DragunityKnightLuin, () => true);
            AddExecutor(ExecutorType.SpSummon, CardId.DragunityKnightTrident, () => !IsSpecialSummonBlocked());
            AddExecutor(ExecutorType.Activate, CardId.DragunityKnightTrident, () => true);
            AddExecutor(ExecutorType.Activate, CardId.AlbionTheBrandedDragon, () => true);
            AddExecutor(ExecutorType.Activate, CardId.EcclesiaAndTheDarkDragon, () => true);

            AddExecutor(ExecutorType.SpSummon, CardId.RubberBandShooter, RubberBandShooterSummon);
            AddExecutor(ExecutorType.Activate, CardId.RubberBandShooter, RubberBandShooterActivate);

            AddExecutor(ExecutorType.SpSummon, CardId.DragunityLordGeorgius, DragunityLordGeorgiusSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.DragunityLordGeorgius, GeorgiusActivate);

            AddExecutor(ExecutorType.SpSummon, CardId.TotemBird, TotemBirdSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.WindPegasusIgnister, WindPegasusSummon);

            AddExecutor(ExecutorType.SpSummon, CardId.LinkVaruroon, LinkVaruroonSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.LinkVaruroon, LinkVaruroonActivate);

            AddExecutor(ExecutorType.SpSummon, CardId.DoomEagle, LinkSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.WynnTheWindCharmerVerdant, LinkSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.Greatfly, LinkSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.Typhon, TyphonSpSummon);

            // TIER 7: Sets & Repos
            AddExecutor(ExecutorType.SpellSet, CardId.MysticalSpaceTyphoon, MstSpellSetCheck);
            AddExecutor(ExecutorType.SpellSet, CardId.Vision);
            AddExecutor(ExecutorType.SpellSet, CardId.Chant);
            AddExecutor(ExecutorType.SpellSet, CardId.Ascendance);
            AddExecutor(ExecutorType.SpellSet, CardId.Manifestation);
            AddExecutor(ExecutorType.SpellSet, CardId.Mandate);

            AddExecutor(ExecutorType.Repos, MonsterReposLogic);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _kroseaHandUsed = false;
            _kroseaSummonUsed = false;
            _vibrantVortexHandUsed = false;
            _vibrantVortexNegateUsed = false;
            _eldamSummonUsed = false;
            _swenSummonUsed = false;
            _meghalaSpUsed = false;
            _visionUsed = false;
            _chantUsed = false;
            _ascendanceUsed = false;
            _mandateUsed = false;
            _manifestationUsed = false;

            _rubberBandShooterBanishUsed = false;
            _linkVaruroonSummonUsed = false;
            _linkVaruroonTrapPlaceUsed = false;
            _windLocked = false;

            // โ”€โ”€ Going-Second BreakBoard: prioritize disruption over combo โ”€โ”€
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
                // (Speedroid relies on Clear Wing / Crystal Wing for going-second pushes)
            }
        }

        private void TriggerWindLock()
        {
            _windLocked = true;
        }

        private bool HasRemainingRadiantOrMst()
        {
            return Bot.GetRemainingCount(CardId.MysticalSpaceTyphoon, 3) > 0 ||
                   Bot.GetRemainingCount(CardId.Krosea, 3) > 0 ||
                   Bot.GetRemainingCount(CardId.Eldam, 3) > 0 ||
                   Bot.GetRemainingCount(CardId.Swen, 3) > 0 ||
                   Bot.GetRemainingCount(CardId.Meghala, 3) > 0 ||
                   Bot.GetRemainingCount(CardId.VibrantVortex, 3) > 0 ||
                   Bot.GetRemainingCount(CardId.Fonix, 3) > 0 ||
                   Bot.GetRemainingCount(CardId.Vision, 3) > 0 ||
                   Bot.GetRemainingCount(CardId.Chant, 3) > 0 ||
                   Bot.GetRemainingCount(CardId.Ascendance, 3) > 0 ||
                   Bot.GetRemainingCount(CardId.Manifestation, 3) > 0 ||
                   Bot.GetRemainingCount(CardId.Mandate, 3) > 0;
        }

        // ==========================================
        //  TIER 1: Hand traps & counters
        // ==========================================

        private bool MulcharmyFuwalosActivate()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }

        private bool ForbiddenDropletActivate()
        {
            if (Duel.LastChainPlayer == 1)
            {
                var targets = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsDisabled() && !c.IsShouldNotBeTarget()).ToList();
                if (targets.Count > 0)
                {
                    var costCandidates = Bot.Hand.Concat(Bot.GetSpells())
                        .Where(c => c != null && c != Card && !IsAceCard(c)).ToList();
                    if (costCandidates.Count > 0)
                    {
                        AI.SelectCard(costCandidates.Take(1).ToList());
                        AI.SelectNextCard(targets[0]);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ForbiddenCrownActivate()
        {
            if (Duel.LastChainPlayer == 1)
            {
                var targets = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsDisabled() && !c.IsShouldNotBeTarget()).ToList();
                if (targets.Count > 0)
                {
                    AI.SelectCard(targets[0]);
                    return true;
                }
            }
            return false;
        }

        // ==========================================
        //  TIER 2: Quick Effects of Bosses & Disruptions
        // ==========================================

        private bool AreadbhairActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (Duel.LastChainPlayer == 1 && LastChainCard != null && LastChainCard.IsMonster())
                {
                    bool hasDragunityInGy = Bot.Graveyard.Any(c => c != null && 
                        (c.IsCode(CardId.DragunityLordGeorgius) || c.IsCode(CardId.DragunityKnightLuin) || c.IsCode(CardId.DragunityKnightTrident)));
                    if (hasDragunityInGy)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool VibrantVortexActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (IsSpecialSummonBlocked()) return false;
                if (_vibrantVortexHandUsed) return false;
                _vibrantVortexHandUsed = true;
                return true;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_vibrantVortexNegateUsed) return false;
                bool hasMstInGy = Bot.Graveyard.Any(c => c.IsCode(CardId.MysticalSpaceTyphoon));
                if (hasMstInGy && Duel.LastChainPlayer == 1)
                {
                    _vibrantVortexNegateUsed = true;
                    return true;
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                if (IsSpecialSummonBlocked()) return false;
                return true;
            }
            return false;
        }

        private bool FonixActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (IsSpecialSummonBlocked()) return false;
                return true;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                var targets = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                    .Where(c => c != null && !c.IsShouldNotBeTarget()).ToList();
                if (targets.Count > 0)
                {
                    AI.SelectCard(targets.Take(2).ToList());
                    return true;
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                if (IsSpecialSummonBlocked()) return false;
                return true;
            }
            return false;
        }

        private bool ShiinaEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (IsSpecialSummonBlocked()) return false;
                bool controlsWind = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Wind));
                if (!controlsWind) return false;

                var oppCards = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                    .Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget()).ToList();
                if (oppCards.Count > 0)
                {
                    var target = oppCards.OrderByDescending(c => Scorer != null ? Scorer.ThreatScore(c) : (c.IsMonster() ? c.Attack : 1000)).First();
                    AI.SelectCard(target);
                }
                else
                {
                    bool controlAce = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsAceCard(c));
                    if (controlAce) return false;

                    var ourTarget = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !IsAceCard(c));
                    if (ourTarget != null)
                    {
                        AI.SelectCard(ourTarget);
                    }
                }
                return true;
            }
            return false;
        }

        private bool MysticalSpaceTyphoonActivate()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0 && Duel.CurrentChain.Count > 0)
            {
                return false;
            }

            if (LastChainCard != null && LastChainCard.Controller == 1)
            {
                if (LastChainCard.IsSpell() || LastChainCard.IsTrap())
                {
                    bool isContinuousOrFieldOrEquip = LastChainCard.HasType(CardType.Continuous) ||
                                                      LastChainCard.HasType(CardType.Field) ||
                                                      LastChainCard.HasType(CardType.Equip);
                    if (!isContinuousOrFieldOrEquip && Duel.Phase != DuelPhase.End)
                    {
                        return false;
                    }
                }
            }

            if (Duel.CurrentChain.Count > 0 || Duel.Player == 1)
            {
                ClientCard enemyBackrow = Util.GetBestEnemySpell();
                if (enemyBackrow != null)
                {
                    if (Duel.CurrentChain.Any(c => c.IsCode(CardId.MysticalSpaceTyphoon) && Duel.ChainTargets.Contains(enemyBackrow)))
                    {
                        return false;
                    }
                    AI.SelectCard(enemyBackrow);
                    return true;
                }
            }
            else
            {
                if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
                {
                    var ourSetRadiants = Bot.GetSpells().Where(c => c != null && c.IsFacedown() &&
                        ((c.IsCode(CardId.Vision) && !_visionUsed) ||
                         (c.IsCode(CardId.Chant) && !_chantUsed) ||
                         (c.IsCode(CardId.Ascendance) && !_ascendanceUsed) ||
                         (c.IsCode(CardId.Manifestation) && !_manifestationUsed))).ToList();

                    if (ourSetRadiants.Count > 0)
                    {
                        var target = ourSetRadiants.OrderBy(c => {
                            if (c.IsCode(CardId.Vision)) return 1;
                            if (c.IsCode(CardId.Chant)) return 2;
                            if (c.IsCode(CardId.Ascendance)) return 3;
                            return 4;
                        }).First();

                        AI.SelectCard(target);
                        return true;
                    }

                    ClientCard enemyBackrow = Util.GetBestEnemySpell();
                    if (enemyBackrow != null)
                    {
                        AI.SelectCard(enemyBackrow);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool MstSpellSetCheck()
        {
            bool hasSetPopTarget = Bot.Hand.Any(c => c != null && 
                (c.IsCode(CardId.Vision) || c.IsCode(CardId.Chant) || c.IsCode(CardId.Ascendance) || c.IsCode(CardId.Manifestation)));
            if (hasSetPopTarget) return false;
            return true;
        }

        private bool FallenAndVirtuousActivate()
        {
            if (ShouldSkipCombo()) return false;

            var opponentTargets = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                .Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget()).ToList();

            bool hasAlbazExtra = GetRemainingCount(CardId.EcclesiaAndTheDarkDragon) > 0 || GetRemainingCount(CardId.AlbionTheBrandedDragon) > 0;

            if (hasAlbazExtra && opponentTargets.Count > 0)
            {
                int sendId = GetRemainingCount(CardId.EcclesiaAndTheDarkDragon) > 0 ? CardId.EcclesiaAndTheDarkDragon : CardId.AlbionTheBrandedDragon;
                var target = opponentTargets.OrderByDescending(c => Scorer != null ? Scorer.ThreatScore(c) : (c.IsMonster() ? c.Attack : 1000)).First();
                
                AI.SelectCard(sendId);
                AI.SelectNextCard(target);
                return true;
            }

            bool hasEcclesia = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.EcclesiaAndTheDarkDragon)) ||
                              Bot.Graveyard.Any(c => c != null && c.IsCode(CardId.EcclesiaAndTheDarkDragon));

            if (hasEcclesia)
            {
                var targets = Bot.Graveyard.Concat(Enemy.Graveyard)
                    .Where(c => c != null && c.IsMonster() && c.IsCanRevive() && !IsAceCard(c))
                    .OrderByDescending(c => c.Attack)
                    .ToList();
                if (targets.Count > 0)
                {
                    AI.SelectCard(targets[0]);
                    return true;
                }
            }
            return false;
        }

        // ==========================================
        //  TIER 3: Starters & Extenders
        // ==========================================

        private bool PotOfProsperityEffect()
        {
            if (ShouldSkipCombo()) return false;
            int banishCount = Bot.ExtraDeck.Count >= 6 ? 6 : 3;
            if (Bot.ExtraDeck.Count < banishCount) return false;
            
            var banishList = Bot.ExtraDeck.Where(c => c != null && 
                !c.IsCode(CardId.DragunityKnightAreadbhair) &&
                !c.IsCode(CardId.DragunityLordGeorgius) &&
                !c.IsCode(CardId.RubberBandShooter) &&
                !c.IsCode(CardId.DragunityKnightLuin)
            ).ToList();
            
            if (banishList.Count >= banishCount)
            {
                return true;
            }
            return false;
        }

        private bool ShieldBearerActivate()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.MonsterZone)
            {
                if (ShouldSkipCombo()) return false;
                bool hasTarget = Bot.GetRemainingCount(CardId.Swen, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.Eldam, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.Krosea, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.Meghala, 3) > 0;
                if (hasTarget)
                {
                    int searchId = CardId.Swen;
                    if (Bot.GetRemainingCount(CardId.Swen, 3) > 0) searchId = CardId.Swen;
                    else if (Bot.GetRemainingCount(CardId.Eldam, 3) > 0) searchId = CardId.Eldam;
                    else if (Bot.GetRemainingCount(CardId.Krosea, 3) > 0) searchId = CardId.Krosea;
                    else if (Bot.GetRemainingCount(CardId.Meghala, 3) > 0) searchId = CardId.Meghala;
                    AI.SelectCard(searchId);
                    return true;
                }
            }
            return false;
        }

        private bool PiperActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (ActivateDescription == Util.GetStringId(CardId.SpeedroidFukiModoshiPiper, 0))
                {
                    return true;
                }
                
                if (ActivateDescription == Util.GetStringId(CardId.SpeedroidFukiModoshiPiper, 1))
                {
                    bool hasUsefulWind = Bot.Hand.Any(c => c != null && c.IsMonster() &&
                        (c.IsCode(CardId.Swen) && !_swenSummonUsed ||
                         c.IsCode(CardId.Eldam) && !_eldamSummonUsed ||
                         c.IsCode(CardId.Krosea) && !_kroseaSummonUsed));
                    return hasUsefulWind;
                }
                return true;
            }
            return false;
        }

        private bool DukeEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                if (IsSpecialSummonBlocked()) return false;
                if (ShouldSkipCombo()) return false;

                bool hasPiper = Bot.Hand.Any(c => c.IsCode(CardId.SpeedroidFukiModoshiPiper)) ||
                                Bot.Graveyard.Any(c => c.IsCode(CardId.SpeedroidFukiModoshiPiper) && c.IsCanRevive());
                if (hasPiper)
                {
                    AI.SelectCard(CardId.SpeedroidFukiModoshiPiper);
                    return true;
                }
            }
            return false;
        }

        // ==========================================
        //  TIER 4: Radiant starters summons & activations
        // ==========================================

        private bool KroseaActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_kroseaHandUsed) return false;
                _kroseaHandUsed = true;
                return true;
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                if (_kroseaSummonUsed) return false;
                if (!HasRemainingRadiantOrMst()) return false;
                _kroseaSummonUsed = true;
                TriggerWindLock();
                return true;
            }
            return false;
        }

        private bool KroseaSummon()
        {
            if (Bot.GetMonsterCount() < 1) return false;
            if (Bot.GetMonsters().Count(c => c.HasAttribute(CardAttribute.Wind)) >= 2)
                return false;
            return true;
        }

        private bool EldamActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_eldamSummonUsed) return false;
                bool hasTarget = Bot.GetRemainingCount(CardId.Krosea, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.Swen, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.Meghala, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.VibrantVortex, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.Fonix, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.MysticalSpaceTyphoon, 3) > 0;
                if (!hasTarget) return false;
                _eldamSummonUsed = true;
                return true;
            }
            return false;
        }

        private bool SwenActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_swenSummonUsed) return false;
                bool hasTarget = Bot.GetRemainingCount(CardId.Vision, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.Chant, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.Ascendance, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.Manifestation, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.Mandate, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.MysticalSpaceTyphoon, 3) > 0;
                if (!hasTarget) return false;
                _swenSummonUsed = true;
                return true;
            }
            return false;
        }

        private bool MeghalaActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_meghalaSpUsed) return false;
                bool controlsWind = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Wind));
                if (controlsWind)
                {
                    _meghalaSpUsed = true;
                    return true;
                }
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                var controlledIds = Bot.GetMonsters().Where(c => c != null && c.IsFaceup()).Select(c => c.Id).ToList();
                int[] allRadiantMonsterIds = {
                    CardId.Krosea,
                    CardId.Eldam,
                    CardId.Swen,
                    CardId.Meghala,
                    CardId.VibrantVortex,
                    CardId.Fonix
                };
                bool hasTarget = false;
                foreach (int id in allRadiantMonsterIds)
                {
                    if (!controlledIds.Contains(id) && Bot.GetRemainingCount(id, 3) > 0)
                    {
                        hasTarget = true;
                        break;
                    }
                }
                if (!hasTarget) return false;
                return true;
            }
            return false;
        }

        // ==========================================
        //  TIER 5: Spells popped triggers & continuous
        // ==========================================

        private bool VisionActivate()
        {
            if (Card.Location == CardLocation.Grave)
            {
                return Bot.Hand.Count >= 1;
            }
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFaceup()))
            {
                if (_visionUsed) return false;
                _visionUsed = true;
                return true;
            }
            return false;
        }

        private bool ChantActivate()
        {
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFaceup()))
            {
                if (_chantUsed) return false;
                bool hasTarget = Bot.GetRemainingCount(CardId.Eldam, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.Swen, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.Meghala, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.Krosea, 3) > 0;
                if (!hasTarget) return false;
                _chantUsed = true;
                return true;
            }
            return false;
        }

        private bool AscendanceActivate()
        {
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFaceup()))
            {
                if (_ascendanceUsed) return false;
                bool hasTarget = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.Level <= 6 && c.HasAttribute(CardAttribute.Wind) && c.IsCanRevive());
                if (!hasTarget) return false;
                _ascendanceUsed = true;
                return true;
            }
            return false;
        }

        private bool ManifestationActivate()
        {
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFaceup()))
            {
                if (_manifestationUsed) return false;
                _manifestationUsed = true;
                return true;
            }
            return false;
        }

        private bool MandateActivate()
        {
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                if (_mandateUsed) return false;

                var quickPlays = Bot.Graveyard.Where(c => c != null && c.IsSpell() && c.HasType(CardType.QuickPlay)).ToList();
                var radiantQuickPlays = quickPlays.Where(c => c.IsCode(CardId.MysticalSpaceTyphoon) || c.IsCode(CardId.Vision) || c.IsCode(CardId.Chant) || c.IsCode(CardId.Ascendance) || c.IsCode(CardId.Manifestation)).ToList();

                if (quickPlays.Count >= 3 && radiantQuickPlays.Count >= 1)
                {
                    _mandateUsed = true;
                    var selection = new List<ClientCard>();
                    selection.Add(radiantQuickPlays[0]);
                    foreach (var card in quickPlays)
                    {
                        if (selection.Count >= 3) break;
                        if (!selection.Contains(card)) selection.Add(card);
                    }
                    AI.SelectCard(selection);
                    return true;
                }
            }

            if (Card.Location == CardLocation.SpellZone)
            {
                if (LastChainCard != null && LastChainCard.IsCode(CardId.MysticalSpaceTyphoon))
                {
                    ClientCard target = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                        .FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget());
                    if (target != null)
                    {
                        AI.SelectCard(target);
                        return true;
                    }
                }
            }

            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }
            return false;
        }

        // ==========================================
        //  TIER 6: Extra Deck Summons
        // ==========================================

        private bool RubberBandShooterSummon()
        {
            if (_rubberBandShooterBanishUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;

            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Wind) && !IsAceCard(c)).ToList();
            if (materials.Count >= 2)
            {
                bool canUseEffect = GetRemainingCount(CardId.DragunityKnightLuin) > 0 &&
                                    Bot.GetRemainingCount(CardId.SpeedroidFukiModoshiPiper, 1) > 0 &&
                                    Bot.GetRemainingCount(CardId.SpeedroidDenDenDaikoDuke, 1) > 0;
                
                if (canUseEffect || Bot.Hand.Any(c => c.IsMonster() && c.HasAttribute(CardAttribute.Wind)))
                {
                    return true;
                }
            }
            return false;
        }

        private bool RubberBandShooterActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (ActivateDescription == Util.GetStringId(CardId.RubberBandShooter, 0))
                {
                    bool hasWindInHand = Bot.Hand.Any(c => c != null && c.IsMonster() && c.HasAttribute(CardAttribute.Wind));
                    return hasWindInHand;
                }

                if (ActivateDescription == Util.GetStringId(CardId.RubberBandShooter, 1))
                {
                    bool hasLuin = GetRemainingCount(CardId.DragunityKnightLuin) > 0;
                    bool hasPiper = Bot.GetRemainingCount(CardId.SpeedroidFukiModoshiPiper, 1) > 0;
                    bool hasDuke = Bot.GetRemainingCount(CardId.SpeedroidDenDenDaikoDuke, 1) > 0;
                    if (hasLuin && hasPiper && hasDuke)
                    {
                        AI.SelectCard(CardId.DragunityKnightLuin);
                        AI.SelectNextCard(CardId.SpeedroidFukiModoshiPiper, CardId.SpeedroidDenDenDaikoDuke);
                        _rubberBandShooterBanishUsed = true; // Added tracking
                        return true;
                    }
                }
            }
            return false;
        }

        private bool DragunityLordGeorgiusSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;

            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c)).ToList();
            if (materials.Count >= 2 && materials.Any(c => c.IsTuner()))
            {
                bool hasTarget = GetRemainingCount(CardId.DragunityKnightAreadbhair) > 0 || GetRemainingCount(CardId.DragunityKnightTrident) > 0;
                if (hasTarget)
                {
                    int totalLinkRating = 0;
                    foreach (var m in materials)
                    {
                        totalLinkRating += m.HasType(CardType.Link) ? m.LinkCount : 1;
                    }
                    if (totalLinkRating >= 4)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool GeorgiusActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (GetRemainingCount(CardId.DragunityKnightAreadbhair) > 0)
                {
                    AI.SelectCard(CardId.DragunityKnightAreadbhair);
                    return true;
                }
                if (GetRemainingCount(CardId.DragunityKnightTrident) > 0)
                {
                    AI.SelectCard(CardId.DragunityKnightTrident);
                    return true;
                }
            }
            return false;
        }

        private bool TotemBirdSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;

            var lv3Winds = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.Level == 3 && c.HasAttribute(CardAttribute.Wind)).ToList();
            if (lv3Winds.Count >= 2)
            {
                AI.SelectCard(lv3Winds);
                return true;
            }
            return false;
        }

        private bool WindPegasusSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;

            var tuners = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.IsTuner()).ToList();
            var nonTuners = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsTuner()).ToList();

            bool hasLvl3Tuner = tuners.Any(c => c.Level == 3);
            bool hasLvl4NonTuner = nonTuners.Any(c => c.Level == 4);

            if (hasLvl3Tuner && hasLvl4NonTuner)
            {
                return true;
            }
            return false;
        }

        private bool LinkVaruroonSpSummon()
        {
            if (_rubberBandShooterBanishUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            if (_linkVaruroonSummonUsed) return false;

            // LinkVaruroon requires "Radiant Typhoon" monsters specifically, not just any WIND
            int[] radiantIds = { CardId.Krosea, CardId.Eldam, CardId.Swen, CardId.Meghala, CardId.VibrantVortex, CardId.Fonix };
            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && radiantIds.Contains(c.Id) && !IsAceCard(c)).ToList();
            if (materials.Count >= 2)
            {
                _linkVaruroonSummonUsed = true;
                AI.SelectCard(materials.Take(2).ToList());
                return true;
            }
            return false;
        }

        private bool LinkVaruroonActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (ActivateDescription == Util.GetStringId(CardId.LinkVaruroon, 0))
                {
                    return true;
                }

                if (ActivateDescription == Util.GetStringId(CardId.LinkVaruroon, 1))
                {
                    var enemyTargets = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget()).ToList();
                    var ourMonsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
                    
                    if (ourMonsters.Count == 1 && ourMonsters[0].IsCode(CardId.LinkVaruroon))
                    {
                        return false;
                    }

                    if (enemyTargets.Count > 0 && ourMonsters.Count >= 2)
                    {
                        return true;
                    }
                    return false;
                }

                if (ActivateDescription == Util.GetStringId(CardId.LinkVaruroon, 2))
                {
                    if (_linkVaruroonTrapPlaceUsed) return false;
                    _linkVaruroonTrapPlaceUsed = true;
                    return true;
                }
            }
            return false;
        }

        // ==========================================
        //  TIER 7: Overrides & battle Repos
        // ==========================================

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.IsCode(
                CardId.VibrantVortex,
                CardId.Fonix,
                CardId.DragunityKnightAreadbhair,
                CardId.DragunityLordGeorgius,
                CardId.LinkVaruroon,
                CardId.Typhon
            );
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return base.OnSelectCard(cards, min, max, hint, cancelable);

            // Hint 500 / 505: Tribute & Cost Selection
            if (hint == 500 || hint == 505)
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (IsAceCard(c)) return 900;
                    if (c.IsCode(CardId.SpeedroidDenDenDaikoDuke)) return 10;
                    if (c.IsCode(CardId.SpeedroidFukiModoshiPiper)) return 20;
                    return 100;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // Hint 501 / 549: Removal / Disruption Target Selection
            if (hint == 501 || hint == 549)
            {
                var oppCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (oppCards.Count > 0)
                {
                    var sortedOpp = oppCards.OrderByDescending(c => {
                        if (c.IsSpell() && c.IsFaceup() && (c.HasType(CardType.Continuous) || c.HasType(CardType.Field))) return 4000;
                        if (c.IsMonster() && c.IsFaceup() && !c.IsDisabled()) return 3000 + c.Attack;
                        if (c.IsFacedown()) return 2000;
                        return 1000;
                    }).ToList();
                    return sortedOpp.Take(max).ToList();
                }
            }

            // Hint 511 / 512 / 513 / 533 / 508 / 504: Synchro / Link / Material Selection
            if (hint == 511 || hint == 512 || hint == 513 || hint == 533 || hint == 508 || hint == 504)
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (IsAceCard(c)) return 900;
                    if (c.IsCode(CardId.MulcharmyFuwalos)) return 800;
                    if (c.IsCode(CardId.SpeedroidFukiModoshiPiper)) return 10;
                    if (c.IsCode(CardId.SpeedroidDenDenDaikoDuke)) return 20;
                    return 100;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            if (hint == 502)
            {
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyCards.Count > 0)
                {
                    var prioritized = enemyCards.OrderByDescending(c =>
                    {
                        if (c.IsMonster() && c.IsFaceup() && !c.IsDisabled() && c.Attack >= 2500) return 8000;
                        if (c.HasType(CardType.Continuous) && c.IsFaceup()) return 7000;
                        if (c.IsMonster() && c.IsFaceup()) return c.Attack;
                        return 100;
                    }).ToList();
                    return Util.CheckSelectCount(prioritized, cards, min, max);
                }

                var ourCards = cards.Where(c => c != null && c.Controller == 0).ToList();
                if (ourCards.Count > 0)
                {
                    var prioritized = ourCards.OrderBy(c =>
                    {
                        if (IsAceCard(c)) return 99999;
                        if (c.IsCode(CardId.Vision)) return 1;
                        if (c.IsCode(CardId.Chant)) return 2;
                        if (c.IsCode(CardId.Ascendance)) return 3;
                        if (c.IsCode(CardId.Manifestation)) return 4;
                        return 100;
                    }).ToList();
                    return Util.CheckSelectCount(prioritized, cards, min, max);
                }
            }

            if (hint == 509)
            {
                var revivable = cards.Where(c => c != null && c.IsMonster()).ToList();
                if (revivable.Count > 0)
                {
                    var sorted = revivable.OrderByDescending(c =>
                    {
                        if (IsAceCard(c)) return 5000 + c.Attack;
                        if (c.IsCode(CardId.SpeedroidFukiModoshiPiper)) return 4000;
                        if (c.IsCode(CardId.SpeedroidDenDenDaikoDuke)) return 3000;
                        return c.Attack;
                    }).ToList();
                    return Util.CheckSelectCount(sorted, cards, min, max);
                }
            }

            if (hint == 506)
            {
                var searchable = cards.Where(c => c != null).ToList();
                if (searchable.Count > 0)
                {
                    var sorted = searchable.OrderByDescending(c =>
                    {
                        if (c.Id == CardId.MysticalSpaceTyphoon && !Bot.HasInHand(CardId.MysticalSpaceTyphoon)) return 100;
                        if (c.Id == CardId.Swen && !Bot.HasInHand(CardId.Swen)) return 90;
                        if (c.Id == CardId.Krosea && !Bot.HasInHand(CardId.Krosea)) return 80;
                        if (c.Id == CardId.Eldam && !Bot.HasInHand(CardId.Eldam)) return 70;
                        if (c.Id == CardId.Meghala && !Bot.HasInHand(CardId.Meghala)) return 60;
                        return 10;
                    }).ToList();
                    return Util.CheckSelectCount(sorted, cards, min, max);
                }
            }

            if (Card != null && Card.Id == CardId.PotOfProsperity && cards.All(c => c.Location == CardLocation.Extra))
            {
                var priority = new[] {
                    CardId.Greatfly,
                    CardId.WynnTheWindCharmerVerdant,
                    CardId.DoomEagle,
                    CardId.DragunityKnightTrident,
                    CardId.WindPegasusIgnister,
                    CardId.TotemBird,
                    CardId.Typhon
                };
                var result = new List<ClientCard>();
                foreach (int id in priority)
                {
                    foreach (var c in cards.Where(c2 => c2.Id == id && !result.Contains(c2)))
                    {
                        if (result.Count < max) result.Add(c);
                    }
                }
                if (result.Count >= min) return result.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override IList<ClientCard> OnSelectSynchroMaterial(IList<ClientCard> cards, int sum, int min, int max)
        {
            var sorted = cards.Where(c => c != null)
                .OrderBy(c => GetMaterialPriority(c))
                .ToList();

            var safe = sorted.Where(c => !IsAceCard(c)).ToList();
            if (safe.Count >= min)
                return Util.CheckSelectCount(safe, cards, min, max);

            return base.OnSelectSynchroMaterial(cards, sum, min, max);
        }

        public override int OnSelectOption(IList<long> options)
        {
            return 0;
        }

        public override bool OnSelectYesNo(long desc) => true;

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if ((cardId == CardId.SpeedroidFukiModoshiPiper || cardId == CardId.SpeedroidDenDenDaikoDuke)
                && positions.Contains(CardPosition.FaceUpDefence))
                return CardPosition.FaceUpDefence;

            if ((cardId == CardId.DragunityKnightAreadbhair || cardId == CardId.DragunityLordGeorgius || cardId == CardId.Typhon)
                && positions.Contains(CardPosition.FaceUpAttack))
                return CardPosition.FaceUpAttack;

            return base.OnSelectPosition(cardId, positions);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.IsCode(CardId.MulcharmyFuwalos)) return 800;
            if (c.IsCode(CardId.SpeedroidFukiModoshiPiper)) return 10;
            if (c.IsCode(CardId.SpeedroidDenDenDaikoDuke)) return 20;
            if (c.IsCode(CardId.Krosea) || c.IsCode(CardId.Eldam) || c.IsCode(CardId.Swen) || c.IsCode(CardId.Meghala)) return 30;
            return 100;
        }

        public override ClientCard OnSelectAttacker(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            ClientCard areadbhair = attackers.FirstOrDefault(c => c != null
                && c.IsCode(CardId.DragunityKnightAreadbhair) && !c.Attacked);
            if (areadbhair != null) return areadbhair;

            var sorted = attackers.Where(c => c != null && !c.Attacked && c.Attack > 0)
                .OrderByDescending(c => c.Attack).ToList();
            return sorted.Count > 0 ? sorted.First() : base.OnSelectAttacker(attackers, defenders);
        }

        public override BattlePhaseAction OnSelectAttackTarget(ClientCard attacker, IList<ClientCard> defenders)
        {
            if (defenders.Count == 0) return AI.Attack(attacker, null);

            int GetDefenseValue(ClientCard c)
            {
                if (c == null) return 0;
                return c.IsDefense() ? c.Defense : c.Attack;
            }

            var defeatable = defenders.Where(d => attacker.Attack > GetDefenseValue(d))
                .OrderByDescending(d => GetDefenseValue(d)).ToList();
            if (defeatable.Count > 0) return AI.Attack(attacker, defeatable.First());

            if (defenders.All(d => d == null)) return AI.Attack(attacker, null);

            var equal = defenders.Where(d => attacker.Attack == GetDefenseValue(d) && d.IsAttack()).ToList();
            if (equal.Count > 0 && Bot.LifePoints > Enemy.LifePoints)
                return AI.Attack(attacker, equal.First());

            return null;
        }

        private bool MonsterReposLogic()
        {
            bool enemyEmpty = Enemy.GetMonsterCount() == 0;

            foreach (ClientCard monster in Bot.GetMonsters())
            {
                if (monster == null || !monster.IsFaceup()) continue;
                if (IsAceCard(monster)) continue;

                if (monster.IsAttack())
                {
                    if (!enemyEmpty && monster.Attack <= 1000)
                    {
                        bool enemyStronger = Enemy.GetMonsters().Any(c => c != null
                            && c.IsFaceup() && c.IsAttack() && c.Attack >= monster.Attack);
                        if (enemyStronger && monster.Defense > 0) return true;
                    }
                }
                else
                {
                    if (enemyEmpty && monster.Attack > 0) return true;
                    if (monster.Attack >= 2000 && !Enemy.GetMonsters().Any(c => c != null
                        && c.IsFaceup() && c.Attack >= monster.Attack))
                        return true;
                }
            }
            return false;
        }

        private bool LinkSummonCheck()
        {
            if (_rubberBandShooterBanishUsed) return false;
            return !IsSpecialSummonBlocked();
        }

        private bool TyphonSpSummon()
        {
            if (_windLocked || _rubberBandShooterBanishUsed) return false;
            return !IsSpecialSummonBlocked();
        }

        protected override bool ShouldStopExtending()
        {
            bool hasCoreBoss = Bot.HasInMonstersZone(CardId.DragunityKnightAreadbhair) ||
                               Bot.HasInMonstersZone(CardId.VibrantVortex) ||
                               Bot.HasInMonstersZone(CardId.Fonix) ||
                               Bot.HasInMonstersZone(CardId.LinkVaruroon);
            if (!hasCoreBoss) return false;

            return base.ShouldStopExtending();
        }

        protected override bool IsBoardStrongEnough()
        {
            return BoardScore() >= 15 || base.IsBoardStrongEnough();
        }
    }

    [Deck("Expert_2026_Speedroid", "2026_Speedroid")]
    public class ExpertSpeedroidExecutor : _2026_SpeedroidExecutor
    {
        private string _duelId;
        public ExpertSpeedroidExecutor(GameAI ai, Duel duel) : base(ai, duel)
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
