using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Decks;
using WindBot.Game.AI.Plugins;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("OrcustWCQ", "OrcustWCQ", "Modern")]
    [Deck("Orcust WCQ", "OrcustWCQ", "Modern")]
    [Deck("Orcust", "OrcustWCQ", "Modern")]
    public class OrcustWCQExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Deck Orcust & World Legacy
            public const int Girsu = 69811710;
            public const int OrcustHarpHorror = 57835716;
            public const int OrcustCymbalSkeleton = 21441617;
            public const int OrcustKnightmare = 4055337;
            public const int WorldWand = 93920420;
            public const int WorldCrown = 27918365;
            public const int OrcustBrassBombard = 94046012;

            // Spells & Traps
            public const int OrcustratedBabel = 90351981;
            public const int OrcustratedReturn = 26845680;
            public const int ForbiddenDroplet = 24299458;
            public const int FoolishBurialGoods = 35726888;
            public const int PotOfProsperity = 84211599;
            public const int FoolishBurial = 81439174;
            public const int DominusImpulse = 40366667;
            public const int DominusSpark = 6325660;
            public const int TheBlackGoatLaughs = 49299410;
            public const int OrcustCrescendo = 703897;
            public const int CalledByTheGrave = 24224830;
            public const int InfiniteImpermanence = 10045474;

            // Handtraps
            public const int MulcharmyFuwalos = 42141493;
            public const int MulcharmyPurulia = 84192580;
            public const int AshBlossom = 14558128;
            public const int AshBlossomAlt = 14558127;
            public const int DrollAndLockBird = 94145021;

            // Extra Deck
            public const int Galatea = 30741503;
            public const int GalateaI = 48835607;
            public const int Dingirsu = 93854893;
            public const int Longirsu = 76145142;
            public const int Enlilgirsu = 74820316;
            public const int BarricadeborgBlocker = 13117073;
            public const int AccesscodeTalker = 86066372;
            public const int ZennasDeceivingDollMaidens = 7594154;
            public const int DharcTheDarkCharmer = 8264361;
            public const int WPFancyBall = 4993187;
            public const int IPMasquerena = 65741786;
            public const int SPLittleKnight = 29301450;
            public const int TYPHON = 93039339;
        }

        private readonly OrcustPlugin _plugin;
        internal OrcustPlugin Plugin => _plugin;
        internal OrcustStrategy Strategy => _plugin.Strategy;
        internal OrcustThreatEvaluator ThreatEvaluator => _plugin.ThreatEvaluator;
        internal OrcustMaterialScorer MaterialScorer => _plugin.MaterialScorer;
        internal OrcustBoardAssessor BoardAssessor => _plugin.BoardAssessor;

        public OrcustWCQExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            _plugin = new OrcustPlugin(this);

            // Register Ace Cards in ResourcePlanner
            ResourcePlan.RegisterAceCards(
                CardId.Dingirsu,
                CardId.Galatea,
                CardId.GalateaI,
                CardId.Enlilgirsu,
                CardId.Longirsu,
                CardId.AccesscodeTalker,
                CardId.SPLittleKnight,
                CardId.IPMasquerena
            );

            // Turn lifecycle reset hook
            AddExecutor(ExecutorType.Activate, ResetTurnStateCheck);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 0: QUICK NEGATES, INTERRUPTIONS & COUNTER TRAPS
            // ═══════════════════════════════════════════════════════════════
            // Orcust Crescendo: Counter Trap Omni-Negate & Banish
            AddExecutor(ExecutorType.Activate, CardId.OrcustCrescendo, ShouldCrescendoActivate);

            // Dingirsu: Send 1 opp card to GY OR attach banished Machine / Detach to protect field
            AddExecutor(ExecutorType.Activate, CardId.Dingirsu, ShouldDingirsuActivate);

            // World Crown: Quick Tribute to negate Extra Deck Special Summoned monster effect
            AddExecutor(ExecutorType.Activate, CardId.WorldCrown, ShouldWorldCrownActivate);

            // Dominus Impulse & Spark
            AddExecutor(ExecutorType.Activate, CardId.DominusImpulse, ShouldDominusImpulseActivate);
            AddExecutor(ExecutorType.Activate, CardId.DominusSpark, ShouldDominusSparkActivate);

            // S:P Little Knight: Quick Banish 2 face-up monsters until EP
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, ShouldSPLittleKnightActivate);

            // I:P Masquerena: Quick Link into S:P Little Knight on opponent's turn
            AddExecutor(ExecutorType.Activate, CardId.IPMasquerena, ShouldIPMasquerenaActivate);

            // Longirsu: Quick Send 1 linked monster to GY (with Babel)
            AddExecutor(ExecutorType.Activate, CardId.Longirsu, ShouldLongirsuActivate);

            // Enlilgirsu: Quick take control / GY send
            AddExecutor(ExecutorType.Activate, CardId.Enlilgirsu, ShouldEnlilgirsuActivate);

            // The Black Goat Laughs
            AddExecutor(ExecutorType.Activate, CardId.TheBlackGoatLaughs, ShouldBlackGoatActivate);

            // Handtraps
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, ShouldMulcharmyActivate);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyPurulia, ShouldMulcharmyActivate);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAlt, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DefaultDrollAndLockBird);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, DefaultInfiniteImpermanence);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 1: BOARD BREAKERS & REMOVAL
            // ═══════════════════════════════════════════════════════════════
            // Forbidden Droplet: Send expendables to negate enemy monsters
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ShouldForbiddenDropletActivate);

            // TY-PHON Sky Crisis: Summon if opp summoned 2+ monsters from Extra Deck
            AddExecutor(ExecutorType.SpSummon, CardId.TYPHON, ShouldTYPHONSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TYPHON, ShouldTYPHONActivate);

            // Accesscode Talker: Pop enemy cards
            AddExecutor(ExecutorType.Activate, CardId.AccesscodeTalker, ShouldAccesscodeActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: SEARCHERS & SPELL STARTERS
            // ═══════════════════════════════════════════════════════════════
            // Foolish Burial: Dump Harp Horror > Knightmare > Wand > Cymbal
            AddExecutor(ExecutorType.Activate, CardId.FoolishBurial, ShouldFoolishBurialActivate);

            // Foolish Burial Goods: Dump Crescendo (to add DARK Machine) or Black Goat
            AddExecutor(ExecutorType.Activate, CardId.FoolishBurialGoods, ShouldFoolishBurialGoodsActivate);

            // Pot of Prosperity
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, ShouldPotOfProsperityActivate);

            // Orcustrated Return: Send Orcust to draw 2
            AddExecutor(ExecutorType.Activate, CardId.OrcustratedReturn, ShouldOrcustratedReturnActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: FIELD SPELL (Orcustrated Babel)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.OrcustratedBabel, ShouldBabelActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: NORMAL SUMMONS
            // ═══════════════════════════════════════════════════════════════
            // 1. Girsu: Foolish Orcust + Spawn Token
            AddExecutor(ExecutorType.Summon, CardId.Girsu, ShouldGirsuSummon);
            AddExecutor(ExecutorType.Activate, CardId.Girsu, ShouldGirsuActivate);

            // 2. Harp Horror (Normal summon fallback to link into Galatea-i)
            AddExecutor(ExecutorType.Summon, CardId.OrcustHarpHorror, ShouldHarpHorrorSummon);

            // 3. Cymbal Skeleton (Normal summon fallback)
            AddExecutor(ExecutorType.Summon, CardId.OrcustCymbalSkeleton, ShouldCymbalSkeletonSummon);

            // 4. Orcust Knightmare (Normal summon if desperate)
            AddExecutor(ExecutorType.Summon, CardId.OrcustKnightmare, ShouldKnightmareSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 5: GY TRIGGERS & EXTENDERS
            // ═══════════════════════════════════════════════════════════════
            // World Crown: Hand Special Summon to a zone a Link Monster points to
            AddExecutor(ExecutorType.SpSummon, CardId.WorldCrown, ShouldWorldCrownSpSummon);

            // Orcust Harp Horror: Banish from GY -> Special Summon from Deck
            AddExecutor(ExecutorType.Activate, CardId.OrcustHarpHorror, ShouldHarpHorrorGYActivate);

            // Orcust Knightmare: Banish from GY -> Foolish DARK Machine from Deck
            AddExecutor(ExecutorType.Activate, CardId.OrcustKnightmare, ShouldKnightmareGYActivate);

            // World Wand: Banish from GY -> Special Summon banished Orcust
            AddExecutor(ExecutorType.Activate, CardId.WorldWand, ShouldWorldWandGYActivate);

            // Orcust Cymbal Skeleton: Banish from GY -> Special Summon Orcust from GY (Dingirsu!)
            AddExecutor(ExecutorType.Activate, CardId.OrcustCymbalSkeleton, ShouldCymbalSkeletonGYActivate);

            // Galatea-i: Discard 1 -> Search Babel or World Legacy / GY Banish Orcust to SS self
            AddExecutor(ExecutorType.Activate, CardId.GalateaI, ShouldGalateaIActivate);

            // Galatea: Target 1 banished Machine -> Shuffle into Deck -> Set Orcust S/T
            AddExecutor(ExecutorType.Activate, CardId.Galatea, ShouldGalateaActivate);

            // Crescendo in GY: Banish self to search DARK Machine
            AddExecutor(ExecutorType.Activate, CardId.OrcustCrescendo, ShouldCrescendoGYActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 6: EXTRA DECK SUMMONS
            // ═══════════════════════════════════════════════════════════════
            // 1. Galatea-i (Link-1 using 1 Orcust or World Legacy monster)
            AddExecutor(ExecutorType.SpSummon, CardId.GalateaI, ShouldGalateaISpSummon);

            // 2. Galatea (Link-2 using 2 effect monsters including 1 Orcust)
            AddExecutor(ExecutorType.SpSummon, CardId.Galatea, ShouldGalateaSpSummon);

            // 3. Dingirsu (Overlay onto Galatea or Longirsu!)
            AddExecutor(ExecutorType.SpSummon, CardId.Dingirsu, ShouldDingirsuSpSummon);

            // 4. I:P Masquerena (Link-2 for opponent turn disruption)
            AddExecutor(ExecutorType.SpSummon, CardId.IPMasquerena, ShouldIPMasquerenaSpSummon);

            // 5. S:P Little Knight
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, ShouldSPLittleKnightSpSummon);

            // 6. Longirsu (Link-3)
            AddExecutor(ExecutorType.SpSummon, CardId.Longirsu, ShouldLongirsuSpSummon);

            // 7. Enlilgirsu (Link-4)
            AddExecutor(ExecutorType.SpSummon, CardId.Enlilgirsu, ShouldEnlilgirsuSpSummon);

            // 8. Accesscode Talker (Link-4 OTK)
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, ShouldAccesscodeSpSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 7: SPELL / TRAP SETS & REPOS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, CardId.OrcustCrescendo);
            AddExecutor(ExecutorType.SpellSet, CardId.TheBlackGoatLaughs);
            AddExecutor(ExecutorType.SpellSet, CardId.DominusImpulse);
            AddExecutor(ExecutorType.SpellSet, CardId.DominusSpark);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, ShouldImpermanenceSet);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, ShouldCalledBySet);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        private bool ResetTurnStateCheck()
        {
            _plugin.ResetTurnState();
            return false;
        }

        #region Tier 0: Quick Negates & Interruption
        private bool ShouldCrescendoActivate()
        {
            // Counter Trap: Negate & Banish when opponent activates while we control Orcust Link
            if (Card.Location == CardLocation.SpellZone && Duel.LastChainPlayer == 1)
            {
                bool hasOrcustLink = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.HasType(CardType.Link) && OrcustStrategy.IsOrcustCard(m));
                return hasOrcustLink;
            }
            return false;
        }

        private bool ShouldDingirsuActivate()
        {
            // Protection effect (continuous/trigger detach material to prevent destruction)
            if (ActivateDescription == -1 || Card.Location == CardLocation.MonsterZone)
            {
                return true;
            }
            return true;
        }

        private bool ShouldWorldCrownActivate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            // Tribute to negate an Extra Deck monster effect activated on field
            if (Duel.LastChainPlayer == 1)
            {
                var lastCard = Duel.LastChainPlayer == 1 ? Enemy.MonsterZone.FirstOrDefault(m => m != null && m.IsFaceup()) : null;
                return true;
            }
            return false;
        }

        private bool ShouldDominusImpulseActivate()
        {
            // Negate an effect that includes Special Summoning
            return Duel.LastChainPlayer == 1;
        }

        private bool ShouldDominusSparkActivate()
        {
            // Destroy 1 face-up card on field when opponent activates effect
            if (Duel.LastChainPlayer == 1)
            {
                return Enemy.GetMonsters().Concat(Enemy.GetSpells()).Any(c => c != null && c.IsFaceup());
            }
            return false;
        }

        private bool ShouldSPLittleKnightActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (Duel.LastChainPlayer == 1 || Duel.Phase == DuelPhase.Battle)
                {
                    var oppMonster = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup());
                    if (oppMonster != null)
                    {
                        AI.SelectCard(new[] { Card, oppMonster });
                        return true;
                    }
                }
            }
            return true;
        }

        private bool ShouldIPMasquerenaActivate()
        {
            if (Duel.Player == 1 && Card.Location == CardLocation.MonsterZone)
            {
                return Bot.ExtraDeck.Any(c => c.Id == CardId.SPLittleKnight);
            }
            return false;
        }

        private bool ShouldLongirsuActivate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_plugin.Strategy.LongirsuUsed) return false;

            // Target 2 banished Machines to shuffle, send 1 linked monster to GY
            int banishedMachines = Bot.Banished.Count(c => c != null && (c.Race & (int)CardRace.Machine) != 0);
            if (banishedMachines >= 2 && Enemy.GetMonsterCount() > 0)
            {
                _plugin.Strategy.LongirsuUsed = true;
                return true;
            }
            return false;
        }

        private bool ShouldEnlilgirsuActivate()
        {
            if (Card.Location == CardLocation.MonsterZone && !_plugin.Strategy.EnlilgirsuHandUsed)
            {
                int banishedCount = Bot.Banished.Count(c => OrcustStrategy.IsOrcustCard(c) || OrcustStrategy.IsWorldLegacyCard(c));
                if (banishedCount >= 1 && Enemy.GetMonsterCount() > 0)
                {
                    _plugin.Strategy.EnlilgirsuHandUsed = true;
                    return true;
                }
            }
            if (Card.Location == CardLocation.Grave && !_plugin.Strategy.EnlilgirsuGYUsed)
            {
                _plugin.Strategy.EnlilgirsuGYUsed = true;
                return true;
            }
            return false;
        }

        private bool ShouldBlackGoatActivate()
        {
            return Duel.LastChainPlayer == 1 || Duel.Player == 1;
        }

        private bool ShouldMulcharmyActivate()
        {
            return Duel.Player == 1 && Bot.GetMonsterCount() == 0;
        }
        #endregion

        #region Tier 1: Board Breakers
        private bool ShouldForbiddenDropletActivate()
        {
            if (Enemy.GetMonsterCount() == 0) return false;
            var oppEffectMonsters = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup() && !m.IsDisabled()).ToList();
            if (!oppEffectMonsters.Any()) return false;

            // Discard materials or tokens
            int sendable = Bot.Hand.Count(c => c != Card) + Bot.GetMonsters().Count(m => m != null && !_plugin.MaterialScorer.IsProtectedBoss(m));
            return sendable >= 1;
        }

        private bool ShouldTYPHONSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.TYPHON)) return false;
            return Enemy.GetMonsters().Any(m => m != null && m.Attack >= 3000);
        }

        private bool ShouldTYPHONActivate()
        {
            return Enemy.GetMonsterCount() > 0;
        }

        private bool ShouldAccesscodeActivate()
        {
            return Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
        }
        #endregion

        #region Tier 2: Searchers & Setup
        private bool ShouldFoolishBurialActivate()
        {
            return true;
        }

        private bool ShouldFoolishBurialGoodsActivate()
        {
            return true;
        }

        private bool ShouldPotOfProsperityActivate()
        {
            return true;
        }

        private bool ShouldOrcustratedReturnActivate()
        {
            if (_plugin.Strategy.OrcustratedReturnUsed) return false;
            bool hasTarget = Bot.Hand.Any(c => c != Card && (OrcustStrategy.IsOrcustCard(c) || OrcustStrategy.IsWorldLegacyCard(c))) ||
                             Bot.GetMonsters().Any(m => m != null && !_plugin.MaterialScorer.IsProtectedBoss(m) && (OrcustStrategy.IsOrcustCard(m) || OrcustStrategy.IsWorldLegacyCard(m)));

            if (hasTarget)
            {
                _plugin.Strategy.OrcustratedReturnUsed = true;
                return true;
            }
            return false;
        }
        #endregion

        #region Tier 3: Field Spell
        private bool ShouldBabelActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return !_plugin.Strategy.HasBabelOnField();
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Send 1 card from hand to add Babel back to hand
                return Bot.Hand.Count >= 2 && !_plugin.Strategy.HasBabelOnField();
            }
            return false;
        }
        #endregion

        #region Tier 4: Normal Summons
        private bool ShouldGirsuSummon()
        {
            return !_plugin.Strategy.GirsuSummonUsed;
        }

        private bool ShouldGirsuActivate()
        {
            // Effect 1: Dump Orcust from Deck to GY
            if (ActivateDescription == -1 || Card.Location == CardLocation.MonsterZone)
            {
                _plugin.Strategy.GirsuSummonUsed = true;
                return true;
            }
            // Effect 2: Spawn Tokens to both fields
            if (Bot.GetMonsterCount() <= 1 && !_plugin.Strategy.GirsuTokenUsed)
            {
                _plugin.Strategy.GirsuTokenUsed = true;
                return true;
            }
            return true;
        }

        private bool ShouldHarpHorrorSummon()
        {
            return Bot.GetMonsterCount() == 0 && !Bot.HasInHand(CardId.Girsu);
        }

        private bool ShouldCymbalSkeletonSummon()
        {
            return Bot.GetMonsterCount() == 0 && !Bot.HasInHand(CardId.Girsu) && !Bot.HasInHand(CardId.OrcustHarpHorror);
        }

        private bool ShouldKnightmareSummon()
        {
            return Bot.GetMonsterCount() == 0 && !Bot.HasInHand(CardId.Girsu) && !Bot.HasInHand(CardId.OrcustHarpHorror) && !Bot.HasInHand(CardId.OrcustCymbalSkeleton);
        }
        #endregion

        #region Tier 5: GY Extenders & Ignition
        private bool ShouldWorldCrownSpSummon()
        {
            // Special summon to a zone a Link Monster points to
            return Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.HasType(CardType.Link));
        }

        private bool ShouldHarpHorrorGYActivate()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_plugin.Strategy.HarpHorrorUsed) return false;

            // Only activate on our turn, OR on opponent turn if Babel is active
            if (Duel.Player == 1 && !_plugin.Strategy.HasBabelOnField()) return false;

            _plugin.Strategy.HarpHorrorUsed = true;
            _plugin.Strategy.IsDarkLockedThisTurn = true;
            return true;
        }

        private bool ShouldKnightmareGYActivate()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_plugin.Strategy.OrcustKnightmareUsed) return false;

            if (Duel.Player == 1 && !_plugin.Strategy.HasBabelOnField()) return false;

            // Target face-up monster on field
            var target = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup()) ?? Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup());
            if (target != null)
            {
                AI.SelectCard(target);
                _plugin.Strategy.OrcustKnightmareUsed = true;
                _plugin.Strategy.IsDarkLockedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool ShouldWorldWandGYActivate()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_plugin.Strategy.WorldWandUsed) return false;

            bool hasBanishedOrcust = Bot.Banished.Any(c => OrcustStrategy.IsOrcustMonster(c));
            if (hasBanishedOrcust)
            {
                _plugin.Strategy.WorldWandUsed = true;
                _plugin.Strategy.IsDarkLockedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool ShouldCymbalSkeletonGYActivate()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_plugin.Strategy.CymbalSkeletonUsed) return false;

            // If opponent's turn without Babel, cannot activate
            if (Duel.Player == 1 && !_plugin.Strategy.HasBabelOnField()) return false;

            // On opponent turn with Babel: Wait for opponent to summon or commit to trigger Dingirsu non-targeting removal!
            if (Duel.Player == 1 && _plugin.Strategy.HasBabelOnField())
            {
                // Activate if enemy has monsters or activated card
                bool oppThreat = Enemy.GetMonsterCount() > 0 || Duel.LastChainPlayer == 1;
                if (!oppThreat) return false;
            }

            bool hasTarget = Bot.Graveyard.Any(c => c != Card && OrcustStrategy.IsOrcustMonster(c));
            if (hasTarget)
            {
                _plugin.Strategy.CymbalSkeletonUsed = true;
                _plugin.Strategy.IsDarkLockedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool ShouldGalateaIActivate()
        {
            // Effect 1: Discard 1 -> Search Babel or World Legacy
            if (Card.Location == CardLocation.MonsterZone && !_plugin.Strategy.GalateaISearchUsed)
            {
                _plugin.Strategy.GalateaISearchUsed = true;
                return true;
            }

            // Effect 2: GY effect: Banish 1 Orcust to Special Summon self
            if (Card.Location == CardLocation.Grave && !_plugin.Strategy.GalateaIGYUsed)
            {
                bool canBanish = Bot.Graveyard.Any(c => c != Card && OrcustStrategy.IsOrcustCard(c) && c.Id != CardId.OrcustCymbalSkeleton);
                if (canBanish)
                {
                    _plugin.Strategy.GalateaIGYUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool ShouldGalateaActivate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_plugin.Strategy.GalateaUsed) return false;

            // Target 1 banished Machine to shuffle into deck -> Set Orcust S/T
            bool hasBanishedMachine = Bot.Banished.Any(c => c != null && (c.Race & (int)CardRace.Machine) != 0);
            if (hasBanishedMachine)
            {
                _plugin.Strategy.GalateaUsed = true;
                return true;
            }
            return false;
        }

        private bool ShouldCrescendoGYActivate()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_plugin.Strategy.CrescendoGYUsed) return false;

            _plugin.Strategy.CrescendoGYUsed = true;
            return true;
        }
        #endregion

        #region Tier 6: Extra Deck Summons
        private bool ShouldGalateaISpSummon()
        {
            // Link-1: 1 Orcust or World Legacy monster
            var material = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() &&
                !m.HasType(CardType.Link) &&
                (OrcustStrategy.IsOrcustMonster(m) || OrcustStrategy.IsWorldLegacyCard(m)) &&
                !_plugin.MaterialScorer.IsProtectedBoss(m));

            if (material != null)
            {
                AI.SelectCard(material);
                return true;
            }
            return false;
        }

        private bool ShouldGalateaSpSummon()
        {
            if (_plugin.Strategy.GalateaUsed) return false;

            var expendables = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && !_plugin.MaterialScorer.IsProtectedBoss(m)).ToList();
            bool hasOrcust = expendables.Any(m => OrcustStrategy.IsOrcustMonster(m));
            return expendables.Count >= 2 && hasOrcust;
        }

        private bool ShouldDingirsuSpSummon()
        {
            if (_plugin.Strategy.DingirsuSummonedThisTurn) return false;

            // Overlay onto Galatea, Galatea-i, or Longirsu!
            var orcustLink = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.HasType(CardType.Link) && OrcustStrategy.IsOrcustCard(m));
            if (orcustLink != null)
            {
                AI.SelectCard(orcustLink);
                _plugin.Strategy.DingirsuSummonedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool ShouldIPMasquerenaSpSummon()
        {
            if (Duel.Turn != 1) return false;
            if (Bot.HasInMonstersZone(CardId.IPMasquerena)) return false;

            var nonLinkExpendables = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && !m.HasType(CardType.Link) && !_plugin.MaterialScorer.IsProtectedBoss(m)).ToList();
            return nonLinkExpendables.Count >= 2;
        }

        private bool ShouldSPLittleKnightSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.SPLittleKnight)) return false;
            var expendables = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && !_plugin.MaterialScorer.IsProtectedBoss(m)).ToList();
            return expendables.Count >= 2 && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0);
        }

        private bool ShouldLongirsuSpSummon()
        {
            var expendables = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && !_plugin.MaterialScorer.IsProtectedBoss(m)).ToList();
            return expendables.Count >= 3;
        }

        private bool ShouldEnlilgirsuSpSummon()
        {
            var expendables = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && !_plugin.MaterialScorer.IsProtectedBoss(m)).ToList();
            bool hasOrcustLink = expendables.Any(m => m.HasType(CardType.Link) && OrcustStrategy.IsOrcustCard(m));
            return expendables.Count >= 3 && hasOrcustLink;
        }

        private bool ShouldAccesscodeSpSummon()
        {
            // Link-4 finisher for OTK
            var link3or2 = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.HasType(CardType.Link) && m.LinkMarker >= 2);
            var other = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m != link3or2);
            return link3or2 != null && other != null && (Enemy.GetMonsterCount() > 0 || Enemy.LifePoints <= 5300);
        }
        #endregion

        #region Tier 7: Backrow Setting
        private bool ShouldImpermanenceSet()
        {
            return Duel.Player == 0 && Duel.Phase == DuelPhase.Main2;
        }

        private bool ShouldCalledBySet()
        {
            return Duel.Player == 0;
        }
        #endregion

        #region Universal OnSelect Handlers
        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Hint 500: Release / Tribute
            if (hint == 500)
            {
                var sorted = cards.OrderByDescending(c => _plugin.MaterialScorer.ScoreTributeOrCostMaterial(c)).ToList();
                return sorted.Take(min).ToList();
            }

            // Hint 501: Discard
            if (hint == 501)
            {
                var sorted = cards.OrderByDescending(c => _plugin.MaterialScorer.ScoreDiscardMaterial(c)).ToList();
                return sorted.Take(min).ToList();
            }

            // Hint 502: Destroy / Hint 503: Remove / Hint 505: Return to Hand
            if (hint == 502 || hint == 503 || hint == 505)
            {
                var oppCards = cards.Where(c => c.Controller == 1).ToList();
                if (oppCards.Any())
                {
                    var sorted = oppCards.OrderByDescending(c => _plugin.ThreatEvaluator.EvaluateTargetPriority(c)).ToList();
                    return sorted.Take(Math.Min(max, sorted.Count)).ToList();
                }
                if (cancelable)
                {
                    return new List<ClientCard>();
                }
            }

            // Hint 572: Negate / Hint 575: Disable / Hint 550: Target / Hint 551: Effect / Hint 514: Face-up / Hint 556: Opponent
            if (hint == 572 || hint == 575 || hint == 550 || hint == 551 || hint == 514 || hint == 556)
            {
                var oppCards = cards.Where(c => c.Controller == 1).ToList();
                if (oppCards.Any())
                {
                    var sorted = oppCards.OrderByDescending(c => _plugin.ThreatEvaluator.EvaluateTargetPriority(c)).ToList();
                    return sorted.Take(Math.Min(max, sorted.Count)).ToList();
                }
                if (cancelable)
                {
                    return new List<ClientCard>();
                }
            }

            // Hint 504: Send to GY (Dingirsu non-targeting send OR Foolish effect)
            if (hint == 504)
            {
                var oppCards = cards.Where(c => c.Controller == 1).ToList();
                if (oppCards.Any())
                {
                    var sorted = oppCards.OrderByDescending(c => _plugin.ThreatEvaluator.EvaluateTargetPriority(c)).ToList();
                    return sorted.Take(min).ToList();
                }

                // Foolish dumping priority (Deck -> GY)
                var selected = new List<ClientCard>();
                var harp = cards.FirstOrDefault(c => c.Id == CardId.OrcustHarpHorror && !_plugin.Strategy.HarpHorrorUsed);
                if (harp != null) selected.Add(harp);

                var knightmare = cards.FirstOrDefault(c => c.Id == CardId.OrcustKnightmare && !_plugin.Strategy.OrcustKnightmareUsed);
                if (knightmare != null && !selected.Contains(knightmare)) selected.Add(knightmare);

                var wand = cards.FirstOrDefault(c => c.Id == CardId.WorldWand && !_plugin.Strategy.WorldWandUsed);
                if (wand != null && !selected.Contains(wand)) selected.Add(wand);

                var cymbal = cards.FirstOrDefault(c => c.Id == CardId.OrcustCymbalSkeleton && !_plugin.Strategy.CymbalSkeletonUsed);
                if (cymbal != null && !selected.Contains(cymbal)) selected.Add(cymbal);

                var crescendo = cards.FirstOrDefault(c => c.Id == CardId.OrcustCrescendo && !_plugin.Strategy.CrescendoGYUsed);
                if (crescendo != null && !selected.Contains(crescendo)) selected.Add(crescendo);

                var goat = cards.FirstOrDefault(c => c.Id == CardId.TheBlackGoatLaughs);
                if (goat != null && !selected.Contains(goat)) selected.Add(goat);

                foreach (var c in cards)
                {
                    if (selected.Count >= min) break;
                    if (!selected.Contains(c)) selected.Add(c);
                }
                return selected.Take(min).ToList();
            }

            // Hint 506: Add to Hand / Search
            if (hint == 506)
            {
                var selected = new List<ClientCard>();

                // Galatea-i / Crescendo search priority:
                if (!_plugin.Strategy.HasBabelOnField() && !Bot.HasInHand(CardId.OrcustratedBabel))
                {
                    var babel = cards.FirstOrDefault(c => c.Id == CardId.OrcustratedBabel);
                    if (babel != null) selected.Add(babel);
                }

                var girsu = cards.FirstOrDefault(c => c.Id == CardId.Girsu);
                if (girsu != null && !selected.Contains(girsu)) selected.Add(girsu);

                var harp = cards.FirstOrDefault(c => c.Id == CardId.OrcustHarpHorror);
                if (harp != null && !selected.Contains(harp)) selected.Add(harp);

                var cymbal = cards.FirstOrDefault(c => c.Id == CardId.OrcustCymbalSkeleton);
                if (cymbal != null && !selected.Contains(cymbal)) selected.Add(cymbal);

                foreach (var c in cards)
                {
                    if (selected.Count >= min) break;
                    if (!selected.Contains(c)) selected.Add(c);
                }
                return selected.Take(min).ToList();
            }

            // Hint 507: Return to Deck (Galatea shuffle banished Machine)
            if (hint == 507)
            {
                var selected = new List<ClientCard>();

                // Galatea-i, World Wand, Harp, Cymbal
                var galateaI = cards.FirstOrDefault(c => c.Id == CardId.GalateaI);
                if (galateaI != null) selected.Add(galateaI);

                var wand = cards.FirstOrDefault(c => c.Id == CardId.WorldWand);
                if (wand != null && !selected.Contains(wand)) selected.Add(wand);

                var harp = cards.FirstOrDefault(c => c.Id == CardId.OrcustHarpHorror);
                if (harp != null && !selected.Contains(harp)) selected.Add(harp);

                var cymbal = cards.FirstOrDefault(c => c.Id == CardId.OrcustCymbalSkeleton);
                if (cymbal != null && !selected.Contains(cymbal)) selected.Add(cymbal);

                foreach (var c in cards)
                {
                    if (selected.Count >= min) break;
                    if (!selected.Contains(c)) selected.Add(c);
                }
                return selected.Take(min).ToList();
            }

            // Hint 508 / 509: Special Summon
            if (hint == 508 || hint == 509)
            {
                var selected = new List<ClientCard>();

                // Cymbal Skeleton revive priority: Dingirsu!
                var dingirsu = cards.FirstOrDefault(c => c.Id == CardId.Dingirsu);
                if (dingirsu != null) selected.Add(dingirsu);

                var galatea = cards.FirstOrDefault(c => c.Id == CardId.Galatea);
                if (galatea != null && !selected.Contains(galatea)) selected.Add(galatea);

                // Harp Horror deck summon priority: Girsu > Cymbal > Knightmare
                var girsu = cards.FirstOrDefault(c => c.Id == CardId.Girsu);
                if (girsu != null && !selected.Contains(girsu)) selected.Add(girsu);

                var cymbal = cards.FirstOrDefault(c => c.Id == CardId.OrcustCymbalSkeleton);
                if (cymbal != null && !selected.Contains(cymbal)) selected.Add(cymbal);

                var knightmare = cards.FirstOrDefault(c => c.Id == CardId.OrcustKnightmare);
                if (knightmare != null && !selected.Contains(knightmare)) selected.Add(knightmare);

                foreach (var c in cards)
                {
                    if (selected.Count >= min) break;
                    if (!selected.Contains(c)) selected.Add(c);
                }
                return selected.Take(min).ToList();
            }

            // Hint 513: Xyz Material (Dingirsu on-summon attach banished Machine)
            if (hint == 513)
            {
                var selected = new List<ClientCard>();
                var cymbal = cards.FirstOrDefault(c => c.Id == CardId.OrcustCymbalSkeleton);
                if (cymbal != null) selected.Add(cymbal);

                var harp = cards.FirstOrDefault(c => c.Id == CardId.OrcustHarpHorror);
                if (harp != null && !selected.Contains(harp)) selected.Add(harp);

                foreach (var c in cards)
                {
                    if (selected.Count >= min) break;
                    if (!selected.Contains(c)) selected.Add(c);
                }
                if (selected.Any()) return selected.Take(min).ToList();

                var sorted = cards.OrderByDescending(c => _plugin.MaterialScorer.ScoreTributeOrCostMaterial(c)).ToList();
                return sorted.Take(min).ToList();
            }

            // Hint 533: Link Material
            if (hint == 533)
            {
                var sorted = cards.OrderByDescending(c => _plugin.MaterialScorer.ScoreTributeOrCostMaterial(c)).ToList();
                return sorted.Take(min).ToList();
            }

            // Universal fallback: Prioritize opponent targets whenever opponent cards are present
            var oppCardsGeneric = cards.Where(c => c.Controller == 1).ToList();
            if (oppCardsGeneric.Any() &&
                hint != 500 && // Release/Tribute
                hint != 501 && // Discard
                hint != 504 && // To Grave (if own deck dump)
                hint != 506 && // Search
                hint != 507 && // To Deck
                hint != 508 && // Summon
                hint != 509 && // SpSummon
                hint != 511 && // Fusion material
                hint != 512 && // Synchro material
                hint != 513 && // Xyz material
                hint != 533)   // Link material
            {
                var sorted = oppCardsGeneric.OrderByDescending(c => _plugin.ThreatEvaluator.EvaluateTargetPriority(c)).ToList();
                return sorted.Take(Math.Min(max, sorted.Count)).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override int OnSelectOption(IList<long> options)
        {
            // Dingirsu On-Summon options:
            // Option 0: Send 1 card your opponent controls to the GY
            // Option 1: Attach 1 of your banished Machine monsters to this card as material
            long opt0 = Util.GetStringId(CardId.Dingirsu, 0);
            long opt1 = Util.GetStringId(CardId.Dingirsu, 1);

            if (options.Contains(opt0) && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0))
            {
                return options.IndexOf(opt0);
            }
            if (options.Contains(opt1) && Bot.Banished.Any(c => c != null && (c.Race & (int)CardRace.Machine) != 0))
            {
                return options.IndexOf(opt1);
            }
            if (options.Contains(opt0))
            {
                return options.IndexOf(opt0);
            }

            return base.OnSelectOption(options);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.Dingirsu)
            {
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }
            if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;

            return base.OnSelectPosition(cardId, positions);
        }
        #endregion
    }
}
