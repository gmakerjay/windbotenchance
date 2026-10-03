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

            // Starters & Extenders
            public const int ScrapRecycler = 4334811;
            public const int ArmageddonKnight = 28985331;
            public const int DarkGrepher = 14536035;
            public const int ReinforcementOfTheArmy = 32807846;
            public const int FoolishBurial = 81439174;

            // Spells & Traps
            public const int OrcustratedBabel = 90351981;
            public const int OrcustratedReturn = 26845680;
            public const int OrcustCrescendo = 703897;
            public const int ForbiddenDroplet = 24299458;
            public const int CalledByTheGrave = 24224830;
            public const int CrossoutDesignator = 65681983;
            public const int TripleTacticsTalent = 25311006;
            public const int InfiniteImpermanence = 10045474;

            // Handtraps
            public const int AshBlossom = 14558128;
            public const int AshBlossomAlt = 14558127;
            public const int GhostBelle = 73642296;

            // Extra Deck
            public const int Galatea = 30741503;
            public const int GalateaI = 48835607;
            public const int Dingirsu = 93854893;
            public const int DingirsuAlt = 93854894;
            public const int Longirsu = 76145142;
            public const int Enlilgirsu = 74820316;
            public const int IPMasquerena = 65741786;
            public const int SPLittleKnight = 29301450;
            public const int AccesscodeTalker = 86066372;
            public const int KnightmarePhoenix = 2857636;
            public const int KnightmareCerberus = 75452921;
            public const int DharcTheDarkCharmer = 8264361;
            public const int TYPHON = 93039339;
            public const int BarricadeborgBlocker = 13117073;
        }

        private static readonly int[] AceCardIds = new[]
        {
            CardId.Dingirsu,
            CardId.DingirsuAlt,
            CardId.Galatea,
            CardId.Longirsu,
            CardId.Enlilgirsu,
            CardId.AccesscodeTalker,
            CardId.SPLittleKnight,
            CardId.IPMasquerena
        };

        private readonly OrcustPlugin _plugin;
        internal OrcustPlugin Plugin => _plugin;
        internal OrcustStrategy Strategy => _plugin.OrcustStrat;
        internal OrcustThreatEvaluator ThreatEvaluator => _plugin.OrcustThreat;
        internal OrcustMaterialScorer MaterialScorer => _plugin.OrcustMat;
        internal OrcustBoardAssessor BoardAssessor => _plugin.BoardAssessor;

        public OrcustWCQExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            _plugin = new OrcustPlugin(this);
            DeckPlugin = _plugin;

            // ═══════════════════════════════════════════════════════════════
            //  AI ENHANCEMENT MODULE REGISTRATIONS (2026+)
            // ═══════════════════════════════════════════════════════════════
            // 1. HeuristicGuard & ResourcePlan Ace Protection
            HeuristicGuard.RegisterAceCards(AceCardIds);
            ResourcePlan.RegisterAceCards(AceCardIds);

            // 2. BaitPlanner: Register safe bait cards to draw out opponent handtraps
            BaitPlanner.RegisterBaitCards(
                CardId.ReinforcementOfTheArmy,
                CardId.OrcustratedReturn,
                CardId.TripleTacticsTalent,
                CardId.FoolishBurial
            );

            // 3. ComboRouter: Register authentic Orcust play lines
            RegisterComboRoutes();

            // ═══════════════════════════════════════════════════════════════
            //  TIER 0: QUICK NEGATES, INTERRUPTIONS & COUNTER TRAPS
            // ═══════════════════════════════════════════════════════════════
            // Orcust Crescendo: Counter Trap Omni-Negate & Banish (Requires Orcust Link on field)
            AddExecutor(ExecutorType.Activate, CardId.OrcustCrescendo, ShouldCrescendoActivate);

            // Dingirsu: Continuous / Trigger detach protection
            AddExecutor(ExecutorType.Activate, CardId.Dingirsu, ShouldDingirsuActivate);
            AddExecutor(ExecutorType.Activate, CardId.DingirsuAlt, ShouldDingirsuActivate);

            // S:P Little Knight: Quick Banish 2 monsters until EP
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, ShouldSPLittleKnightActivate);

            // I:P Masquerena: Quick Link on opponent turn
            AddExecutor(ExecutorType.Activate, CardId.IPMasquerena, ShouldIPMasquerenaActivate);

            // Longirsu: Quick Send 1 linked monster to GY
            AddExecutor(ExecutorType.Activate, CardId.Longirsu, ShouldLongirsuActivate);

            // Enlilgirsu: Quick take control / GY send
            AddExecutor(ExecutorType.Activate, CardId.Enlilgirsu, ShouldEnlilgirsuActivate);

            // Handtraps & Quick Defenses
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAlt, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelle, DefaultGhostBelleAndHauntedMansion);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.CrossoutDesignator, ShouldCrossoutActivate);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, DefaultInfiniteImpermanence);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 1: BOARD BREAKERS & REMOVAL
            // ═══════════════════════════════════════════════════════════════
            // Forbidden Droplet: Send expendables to negate enemy monsters
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ShouldForbiddenDropletActivate);

            // Triple Tactics Talent
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, ShouldTTTActivate);

            // TY-PHON Sky Crisis
            AddExecutor(ExecutorType.SpSummon, CardId.TYPHON, ShouldTYPHONSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TYPHON, ShouldTYPHONActivate);

            // Knightmare Phoenix & Cerberus
            AddExecutor(ExecutorType.Activate, CardId.KnightmarePhoenix, ShouldKnightmarePhoenixActivate);
            AddExecutor(ExecutorType.Activate, CardId.KnightmareCerberus, ShouldKnightmareCerberusActivate);

            // Accesscode Talker: Pop enemy cards
            AddExecutor(ExecutorType.Activate, CardId.AccesscodeTalker, ShouldAccesscodeActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: SEARCHERS & SPELL STARTERS
            // ═══════════════════════════════════════════════════════════════
            // Reinforcement of the Army
            AddExecutor(ExecutorType.Activate, CardId.ReinforcementOfTheArmy, ShouldRotAActivate);

            // Foolish Burial
            AddExecutor(ExecutorType.Activate, CardId.FoolishBurial, ShouldFoolishBurialActivate);

            // Orcustrated Return: Send Orcust from hand/field to draw 2
            AddExecutor(ExecutorType.Activate, CardId.OrcustratedReturn, ShouldOrcustratedReturnActivate);

            // Field Spell: Orcustrated Babel
            AddExecutor(ExecutorType.Activate, CardId.OrcustratedBabel, ShouldBabelActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: SPECIAL SUMMONS FROM HAND (EXTENDERS)
            // ═══════════════════════════════════════════════════════════════
            // Dark Grepher: Discard Lv5+ DARK (WorldWand / Knightmare) to SS
            AddExecutor(ExecutorType.SpSummon, CardId.DarkGrepher, ShouldDarkGrepherSpSummon);

            // World Crown: SS to a zone a Link Monster points to
            AddExecutor(ExecutorType.SpSummon, CardId.WorldCrown, ShouldWorldCrownSpSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: NORMAL SUMMONS (STARTERS)
            // ═══════════════════════════════════════════════════════════════
            // 1. Girsu (Top starter: dumps Harp + makes Token)
            AddExecutor(ExecutorType.Summon, CardId.Girsu, ShouldGirsuSummon);
            AddExecutor(ExecutorType.Activate, CardId.Girsu, ShouldGirsuActivate);

            // 2. Scrap Recycler (1-card starter: dumps Machine)
            AddExecutor(ExecutorType.Summon, CardId.ScrapRecycler, ShouldScrapRecyclerSummon);
            AddExecutor(ExecutorType.Activate, CardId.ScrapRecycler, ShouldScrapRecyclerActivate);

            // 3. Armageddon Knight (1-card starter: dumps DARK)
            AddExecutor(ExecutorType.Summon, CardId.ArmageddonKnight, ShouldArmageddonKnightSummon);
            AddExecutor(ExecutorType.Activate, CardId.ArmageddonKnight, ShouldArmageddonKnightActivate);

            // 4. Dark Grepher (Normal summon fallback)
            AddExecutor(ExecutorType.Summon, CardId.DarkGrepher, ShouldDarkGrepherSummon);
            AddExecutor(ExecutorType.Activate, CardId.DarkGrepher, ShouldDarkGrepherActivate);

            // 5. Fallback Orcust Normal Summons (to make Galatea-i Link-1 bridge)
            AddExecutor(ExecutorType.Summon, CardId.OrcustHarpHorror, ShouldHarpHorrorSummon);
            AddExecutor(ExecutorType.Summon, CardId.OrcustCymbalSkeleton, ShouldCymbalSkeletonSummon);
            AddExecutor(ExecutorType.Summon, CardId.OrcustBrassBombard, ShouldBrassBombardSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 5: GY TRIGGERS & EXTENDERS
            // ═══════════════════════════════════════════════════════════════
            // Galatea: MUST ACTIVATE BEFORE DINGIRSU OVERLAY!
            AddExecutor(ExecutorType.Activate, CardId.Galatea, ShouldGalateaActivate);

            // Orcust Harp Horror: Banish from GY -> Special Summon from Deck
            AddExecutor(ExecutorType.Activate, CardId.OrcustHarpHorror, ShouldHarpHorrorGYActivate);

            // Orcust Knightmare: Banish from GY -> Foolish DARK Machine from Deck
            AddExecutor(ExecutorType.Activate, CardId.OrcustKnightmare, ShouldKnightmareGYActivate);

            // World Wand: Banish from GY -> Special Summon banished Orcust
            AddExecutor(ExecutorType.Activate, CardId.WorldWand, ShouldWorldWandGYActivate);

            // Orcust Brass Bombard: Banish from GY -> Special Summon Orcust from hand
            AddExecutor(ExecutorType.Activate, CardId.OrcustBrassBombard, ShouldBrassBombardGYActivate);

            // Galatea-i: Field search / GY revive
            AddExecutor(ExecutorType.Activate, CardId.GalateaI, ShouldGalateaIActivate);

            // Orcust Cymbal Skeleton: Banish from GY -> Special Summon Dingirsu!
            AddExecutor(ExecutorType.Activate, CardId.OrcustCymbalSkeleton, ShouldCymbalSkeletonGYActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 6: EXTRA DECK SUMMONS
            // ═══════════════════════════════════════════════════════════════
            // 1. Galatea-i (Link-1): ONLY if we have exactly 1 non-link Orcust on field needing GY bridge
            AddExecutor(ExecutorType.SpSummon, CardId.GalateaI, ShouldGalateaISpSummon);

            // 2. Galatea (Link-2): Primary combo engine
            AddExecutor(ExecutorType.SpSummon, CardId.Galatea, ShouldGalateaSpSummon);

            // 3. Dingirsu (Rank 8): Overlay onto Galatea or Longirsu (NEVER Galatea-i!)
            AddExecutor(ExecutorType.SpSummon, CardId.Dingirsu, ShouldDingirsuSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DingirsuAlt, ShouldDingirsuSpSummon);

            // 4. Longirsu (Link-3): Use Dingirsu + Cymbal to send Dingirsu to GY & hold Orcust Link for Crescendo!
            AddExecutor(ExecutorType.SpSummon, CardId.Longirsu, ShouldLongirsuSpSummon);

            // 5. I:P Masquerena: Link-2 for opponent turn disruption
            AddExecutor(ExecutorType.SpSummon, CardId.IPMasquerena, ShouldIPMasquerenaSpSummon);

            // 6. Knightmare Phoenix & Cerberus: Problem removals / Discard outlets
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmarePhoenix, ShouldKnightmarePhoenixSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmareCerberus, ShouldKnightmareCerberusSpSummon);

            // 7. Dharc the Dark Charmer
            AddExecutor(ExecutorType.SpSummon, CardId.DharcTheDarkCharmer, ShouldDharcSpSummon);

            // 8. S:P Little Knight
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, ShouldSPLittleKnightSpSummon);

            // 9. Enlilgirsu (Link-4)
            AddExecutor(ExecutorType.SpSummon, CardId.Enlilgirsu, ShouldEnlilgirsuSpSummon);

            // 10. Accesscode Talker (Link-4 OTK Finisher)
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, ShouldAccesscodeSpSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 7: SPELL / TRAP SETS & REPOS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, CardId.OrcustCrescendo);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, ShouldImpermanenceSet);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet, ShouldDropletSet);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, ShouldCalledBySet);
            AddExecutor(ExecutorType.SpellSet, CardId.CrossoutDesignator, ShouldCrossoutSet);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        private void RegisterComboRoutes()
        {
            // Route 1: Girsu 1-card line
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Girsu-FullCombo",
                RequiredCards = new List<int> { CardId.Girsu },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.Girsu, ActionType = ExecutorType.Summon, Description = "Normal Summon Girsu" },
                    new() { CardId = CardId.Girsu, ActionType = ExecutorType.Activate, Description = "Foolish Harp Horror & Spawn Token" },
                    new() { CardId = CardId.Galatea, ActionType = ExecutorType.SpSummon, Description = "Link Summon Galatea (Link-2)" },
                    new() { CardId = CardId.OrcustHarpHorror, ActionType = ExecutorType.Activate, Description = "Harp Horror SS Cymbal Skeleton" },
                    new() { CardId = CardId.Galatea, ActionType = ExecutorType.Activate, Description = "Galatea recycle Harp -> Set Crescendo" },
                    new() { CardId = CardId.Dingirsu, ActionType = ExecutorType.SpSummon, Description = "Overlay Dingirsu over Galatea" }
                },
                EndBoardScore = 90
            });

            // Route 2: Scrap Recycler 1-card line
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Scrap-Recycler-Starter",
                RequiredCards = new List<int> { CardId.ScrapRecycler },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.ScrapRecycler, ActionType = ExecutorType.Summon, Description = "Normal Summon Scrap Recycler" },
                    new() { CardId = CardId.ScrapRecycler, ActionType = ExecutorType.Activate, Description = "Dump Harp Horror" },
                    new() { CardId = CardId.OrcustHarpHorror, ActionType = ExecutorType.Activate, Description = "Harp Horror SS Girsu" },
                    new() { CardId = CardId.Galatea, ActionType = ExecutorType.SpSummon, Description = "Link Summon Galatea" }
                },
                EndBoardScore = 85
            });

            // Route 3: Armageddon Knight 1-card line
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Armageddon-Knight-Starter",
                RequiredCards = new List<int> { CardId.ArmageddonKnight },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.ArmageddonKnight, ActionType = ExecutorType.Summon, Description = "Normal Summon Armageddon Knight" },
                    new() { CardId = CardId.ArmageddonKnight, ActionType = ExecutorType.Activate, Description = "Dump Harp Horror" },
                    new() { CardId = CardId.OrcustHarpHorror, ActionType = ExecutorType.Activate, Description = "Harp Horror SS Cymbal Skeleton" },
                    new() { CardId = CardId.Galatea, ActionType = ExecutorType.SpSummon, Description = "Link Summon Galatea" }
                },
                EndBoardScore = 85
            });

            // Route 4: Dark Grepher Unbricking line
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "DarkGrepher-Unbricker",
                RequiredCards = new List<int> { CardId.DarkGrepher },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.DarkGrepher, ActionType = ExecutorType.Summon, Description = "Summon Dark Grepher" },
                    new() { CardId = CardId.DarkGrepher, ActionType = ExecutorType.Activate, Description = "Discard DARK -> Dump Harp Horror" }
                },
                EndBoardScore = 80
            });
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _plugin.ResetTurnState();
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
            return true;
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
                return Bot.ExtraDeck.Any(c => c.Id == CardId.SPLittleKnight || c.Id == CardId.KnightmarePhoenix);
            }
            return false;
        }

        private bool ShouldLongirsuActivate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_plugin.OrcustStrat.LongirsuUsed) return false;

            int banishedMachines = Bot.Banished.Count(c => c != null && (c.Race & (int)CardRace.Machine) != 0);
            if (banishedMachines >= 2 && Enemy.GetMonsterCount() > 0)
            {
                _plugin.OrcustStrat.LongirsuUsed = true;
                return true;
            }
            return false;
        }

        private bool ShouldEnlilgirsuActivate()
        {
            if (Card.Location == CardLocation.MonsterZone && !_plugin.OrcustStrat.EnlilgirsuHandUsed)
            {
                int banishedCount = Bot.Banished.Count(c => OrcustStrategy.IsOrcustCard(c) || OrcustStrategy.IsWorldLegacyCard(c));
                if (banishedCount >= 1 && Enemy.GetMonsterCount() > 0)
                {
                    _plugin.OrcustStrat.EnlilgirsuHandUsed = true;
                    return true;
                }
            }
            if (Card.Location == CardLocation.Grave && !_plugin.OrcustStrat.EnlilgirsuGYUsed)
            {
                _plugin.OrcustStrat.EnlilgirsuGYUsed = true;
                return true;
            }
            return false;
        }

        private bool ShouldCrossoutActivate()
        {
            return Duel.LastChainPlayer == 1;
        }
        #endregion

        #region Tier 1: Board Breakers
        private bool ShouldForbiddenDropletActivate()
        {
            // NEVER activate if we already chained an interruption to this action (Double Negate / Self-Chain)
            if (Duel.LastChainPlayer == 0) return false;

            if (Enemy.GetMonsterCount() == 0) return false;
            var oppEffectMonsters = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup() && !m.IsDisabled()).ToList();
            if (!oppEffectMonsters.Any()) return false;

            // Only activate if we have expendable cards to send (from Hand or expendable monsters)
            // STRICT RULE: NEVER send Protected Bosses (Dingirsu, Galatea, etc.) or Key Backrow (Babel, Crescendo)
            int sendableHand = Bot.Hand.Count(c => c != Card && !_plugin.OrcustMat.IsProtectedBoss(c));
            int sendableMonsters = Bot.GetMonsters().Count(m => m != null && m.IsFaceup() && !_plugin.OrcustMat.IsProtectedBoss(m) && !m.HasType(CardType.Token));
            int sendableSpells = Bot.GetSpells().Count(s => s != null && s != Card && s.IsFaceup() && s.Id != CardId.OrcustratedBabel && s.Id != CardId.OrcustCrescendo);

            return (sendableHand + sendableMonsters + sendableSpells) >= 1;
        }

        private bool ShouldTTTActivate()
        {
            if (_plugin.OrcustStrat.TripleTacticsTalentUsed) return false;
            _plugin.OrcustStrat.TripleTacticsTalentUsed = true;
            return true;
        }

        private bool ShouldTYPHONSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.TYPHON)) return false;
            return Enemy.GetMonsters().Any(m => m != null && m.Attack >= 2800);
        }

        private bool ShouldTYPHONActivate()
        {
            return Enemy.GetMonsterCount() > 0;
        }

        private bool ShouldAccesscodeActivate()
        {
            return Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
        }

        private bool ShouldKnightmarePhoenixActivate()
        {
            return Enemy.GetSpellCount() > 0 && Bot.Hand.Any(c => c != Card);
        }

        private bool ShouldKnightmareCerberusActivate()
        {
            return Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.HasType(CardType.SpSummon)) && Bot.Hand.Any(c => c != Card);
        }
        #endregion

        #region Tier 2: Searchers & Spells
        private bool ShouldRotAActivate()
        {
            if (_plugin.OrcustStrat.RotAUsed) return false;
            _plugin.OrcustStrat.RotAUsed = true;
            return true;
        }

        private bool ShouldFoolishBurialActivate()
        {
            return true;
        }

        private bool ShouldOrcustratedReturnActivate()
        {
            if (_plugin.OrcustStrat.OrcustratedReturnUsed) return false;
            // Prefer sending from hand; if sending from field, MUST be an expendable non-boss
            bool hasHandTarget = Bot.Hand.Any(c => c != Card && (OrcustStrategy.IsOrcustCard(c) || OrcustStrategy.IsWorldLegacyCard(c)));
            bool hasFieldTarget = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && !_plugin.OrcustMat.IsProtectedBoss(m) && (OrcustStrategy.IsOrcustCard(m) || OrcustStrategy.IsWorldLegacyCard(m)));

            if (hasHandTarget || hasFieldTarget)
            {
                _plugin.OrcustStrat.OrcustratedReturnUsed = true;
                return true;
            }
            return false;
        }

        private bool ShouldBabelActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return !_plugin.OrcustStrat.HasBabelOnField();
            }
            if (Card.Location == CardLocation.Grave)
            {
                return Bot.Hand.Count >= 2 && !_plugin.OrcustStrat.HasBabelOnField();
            }
            return false;
        }
        #endregion

        #region Tier 3: Special Summons from Hand
        private bool ShouldDarkGrepherSpSummon()
        {
            if (_plugin.OrcustStrat.DarkGrepherSpSummonUsed) return false;
            // Can discard 1 Lv5+ DARK monster (WorldWand / Knightmare)
            bool hasLv5Dark = Bot.Hand.Any(c => c != Card && (c.Attribute & (int)CardAttribute.Dark) != 0 && c.Level >= 5);
            if (hasLv5Dark)
            {
                _plugin.OrcustStrat.DarkGrepherSpSummonUsed = true;
                return true;
            }
            return false;
        }

        private bool ShouldWorldCrownSpSummon()
        {
            return Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.HasType(CardType.Link));
        }
        #endregion

        #region Tier 4: Normal Summons
        private bool ShouldGirsuSummon()
        {
            return !_plugin.OrcustStrat.GirsuSummonUsed;
        }

        private bool ShouldGirsuActivate()
        {
            // Effect 1: Dump Orcust from Deck to GY
            if (ActivateDescription == -1 || Card.Location == CardLocation.MonsterZone)
            {
                _plugin.OrcustStrat.GirsuSummonUsed = true;
                return true;
            }
            // Effect 2: Spawn Tokens to both fields
            if (Bot.GetMonsterCount() <= 1 && !_plugin.OrcustStrat.GirsuTokenUsed)
            {
                _plugin.OrcustStrat.GirsuTokenUsed = true;
                return true;
            }
            return true;
        }

        private bool ShouldScrapRecyclerSummon()
        {
            return Bot.GetMonsterCount() == 0 && !_plugin.OrcustStrat.GirsuSummonUsed;
        }

        private bool ShouldScrapRecyclerActivate()
        {
            _plugin.OrcustStrat.ScrapRecyclerUsed = true;
            return true;
        }

        private bool ShouldArmageddonKnightSummon()
        {
            return Bot.GetMonsterCount() == 0 && !_plugin.OrcustStrat.GirsuSummonUsed && !_plugin.OrcustStrat.ScrapRecyclerUsed;
        }

        private bool ShouldArmageddonKnightActivate()
        {
            _plugin.OrcustStrat.ArmageddonKnightUsed = true;
            return true;
        }

        private bool ShouldDarkGrepherSummon()
        {
            return Bot.GetMonsterCount() == 0 && !_plugin.OrcustStrat.GirsuSummonUsed && !_plugin.OrcustStrat.ScrapRecyclerUsed && !_plugin.OrcustStrat.ArmageddonKnightUsed;
        }

        private bool ShouldDarkGrepherActivate()
        {
            if (_plugin.OrcustStrat.DarkGrepherDumpUsed) return false;
            // Ignition effect: discard 1 DARK monster to send 1 DARK from deck to GY
            bool hasDarkInHand = Bot.Hand.Any(c => c != Card && (c.Attribute & (int)CardAttribute.Dark) != 0);
            if (hasDarkInHand)
            {
                _plugin.OrcustStrat.DarkGrepherDumpUsed = true;
                return true;
            }
            return false;
        }

        private bool ShouldHarpHorrorSummon()
        {
            return Bot.GetMonsterCount() == 0 && !Bot.HasInHand(CardId.Girsu) && !Bot.HasInHand(CardId.ScrapRecycler) && !Bot.HasInHand(CardId.ArmageddonKnight);
        }

        private bool ShouldCymbalSkeletonSummon()
        {
            return Bot.GetMonsterCount() == 0 && !Bot.HasInHand(CardId.Girsu) && !Bot.HasInHand(CardId.ScrapRecycler) && !Bot.HasInHand(CardId.ArmageddonKnight) && !Bot.HasInHand(CardId.OrcustHarpHorror);
        }

        private bool ShouldBrassBombardSummon()
        {
            return Bot.GetMonsterCount() == 0 && !Bot.HasInHand(CardId.Girsu) && !Bot.HasInHand(CardId.ScrapRecycler) && !Bot.HasInHand(CardId.ArmageddonKnight) && !Bot.HasInHand(CardId.OrcustHarpHorror) && !Bot.HasInHand(CardId.OrcustCymbalSkeleton);
        }
        #endregion

        #region Tier 5: GY Extenders & Ignition
        private bool ShouldGalateaActivate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_plugin.OrcustStrat.GalateaUsed) return false;

            // Target 1 banished Machine to shuffle into deck -> Set Orcust S/T
            bool hasBanishedMachine = Bot.Banished.Any(c => c != null && (c.Race & (int)CardRace.Machine) != 0);
            if (hasBanishedMachine)
            {
                _plugin.OrcustStrat.GalateaUsed = true;
                return true;
            }
            return false;
        }

        private bool ShouldHarpHorrorGYActivate()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_plugin.OrcustStrat.HarpHorrorUsed) return false;

            if (Duel.Player == 1 && !_plugin.OrcustStrat.HasBabelOnField()) return false;

            _plugin.OrcustStrat.HarpHorrorUsed = true;
            _plugin.OrcustStrat.IsDarkLockedThisTurn = true;
            return true;
        }

        private bool ShouldKnightmareGYActivate()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_plugin.OrcustStrat.OrcustKnightmareUsed) return false;

            if (Duel.Player == 1 && !_plugin.OrcustStrat.HasBabelOnField()) return false;

            var target = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup()) ?? Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup());
            if (target != null)
            {
                AI.SelectCard(target);
                _plugin.OrcustStrat.OrcustKnightmareUsed = true;
                _plugin.OrcustStrat.IsDarkLockedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool ShouldWorldWandGYActivate()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_plugin.OrcustStrat.WorldWandUsed) return false;

            bool hasBanishedOrcust = Bot.Banished.Any(c => OrcustStrategy.IsOrcustMonster(c));
            if (hasBanishedOrcust)
            {
                _plugin.OrcustStrat.WorldWandUsed = true;
                _plugin.OrcustStrat.IsDarkLockedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool ShouldBrassBombardGYActivate()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_plugin.OrcustStrat.BrassBombardUsed) return false;

            bool hasOrcustInHand = Bot.Hand.Any(c => OrcustStrategy.IsOrcustMonster(c));
            if (hasOrcustInHand)
            {
                _plugin.OrcustStrat.BrassBombardUsed = true;
                _plugin.OrcustStrat.IsDarkLockedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool ShouldGalateaIActivate()
        {
            // Effect 1: Discard 1 -> Search Babel or World Legacy
            if (Card.Location == CardLocation.MonsterZone && !_plugin.OrcustStrat.GalateaISearchUsed)
            {
                if (Bot.Hand.Count >= 1 && !_plugin.OrcustStrat.HasBabelOnField())
                {
                    _plugin.OrcustStrat.GalateaISearchUsed = true;
                    return true;
                }
            }

            // Effect 2: GY effect: Banish 1 Orcust to Special Summon self (Provides Orcust Link for Crescendo!)
            if (Card.Location == CardLocation.Grave && !_plugin.OrcustStrat.GalateaIGYUsed)
            {
                bool canBanish = Bot.Graveyard.Any(c => c != Card && OrcustStrategy.IsOrcustCard(c) && c.Id != CardId.OrcustCymbalSkeleton && c.Id != CardId.Dingirsu && c.Id != CardId.DingirsuAlt);
                if (canBanish && !Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.HasType(CardType.Link) && OrcustStrategy.IsOrcustCard(m)))
                {
                    _plugin.OrcustStrat.GalateaIGYUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool ShouldCymbalSkeletonGYActivate()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_plugin.OrcustStrat.CymbalSkeletonUsed) return false;

            if (Duel.Player == 1 && !_plugin.OrcustStrat.HasBabelOnField()) return false;

            // Opponent turn with Babel: Wait for opponent to commit card/summon before reviving Dingirsu to send!
            if (Duel.Player == 1 && _plugin.OrcustStrat.HasBabelOnField())
            {
                bool oppThreat = Enemy.GetMonsterCount() > 0 || Duel.LastChainPlayer == 1;
                if (!oppThreat) return false;
            }

            bool hasTarget = Bot.Graveyard.Any(c => c != Card && (c.Id == CardId.Dingirsu || c.Id == CardId.DingirsuAlt || OrcustStrategy.IsOrcustMonster(c)));
            if (hasTarget)
            {
                _plugin.OrcustStrat.CymbalSkeletonUsed = true;
                _plugin.OrcustStrat.IsDarkLockedThisTurn = true;
                return true;
            }
            return false;
        }
        #endregion

        #region Tier 6: Extra Deck Summons
        private bool ShouldGalateaISpSummon()
        {
            // NEVER summon Galatea-i if we already have an Ace Boss or Link on field!
            if (Bot.HasInMonstersZone(CardId.Dingirsu) ||
                Bot.HasInMonstersZone(CardId.DingirsuAlt) ||
                Bot.HasInMonstersZone(CardId.Galatea) ||
                Bot.HasInMonstersZone(CardId.GalateaI) ||
                Bot.HasInMonstersZone(CardId.Longirsu))
            {
                return false;
            }

            // ONLY summon Galatea-i if we have exactly 1 main-deck expendable monster needing a GY bridge
            if (Bot.GetMonsterCount() != 1) return false;

            var material = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() &&
                m.Level > 0 &&
                !m.HasType(CardType.Link | CardType.Xyz | CardType.Fusion | CardType.Synchro) &&
                (OrcustStrategy.IsOrcustMonster(m) || OrcustStrategy.IsWorldLegacyCard(m)) &&
                !_plugin.OrcustMat.IsProtectedBoss(m));

            if (material != null)
            {
                AI.SelectCard(material);
                return true;
            }
            return false;
        }

        private bool ShouldGalateaSpSummon()
        {
            // STRICT RULE: Only 1 Galatea on field. NEVER link away Galatea for another Galatea!
            if (Bot.HasInMonstersZone(CardId.Galatea)) return false;

            var expendables = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && !_plugin.OrcustMat.IsProtectedBoss(m)).ToList();
            bool hasOrcust = expendables.Any(m => OrcustStrategy.IsOrcustMonster(m));
            if (expendables.Count >= 2 && hasOrcust)
            {
                AI.SelectCard(expendables);
                return true;
            }
            return false;
        }

        private bool ShouldDingirsuSpSummon()
        {
            if (_plugin.OrcustStrat.DingirsuSummonedThisTurn) return false;
            if (Bot.HasInMonstersZone(CardId.Dingirsu) || Bot.HasInMonstersZone(CardId.DingirsuAlt)) return false;

            // ONLY overlay onto Galatea, Longirsu, or Enlilgirsu — NEVER Galatea-i!
            var orcustLink = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() &&
                (m.Id == CardId.Galatea || m.Id == CardId.Longirsu || m.Id == CardId.Enlilgirsu));

            if (orcustLink != null)
            {
                // CRITICAL SAFETY: If overlaying on Galatea, ensure Galatea has already activated her effect
                // OR we have no banished Machine to shuffle and need Dingirsu now
                if (orcustLink.Id == CardId.Galatea && !_plugin.OrcustStrat.GalateaUsed)
                {
                    bool hasBanishedMachine = Bot.Banished.Any(c => c != null && (c.Race & (int)CardRace.Machine) != 0);
                    if (hasBanishedMachine)
                    {
                        return false; // Let Galatea activate first!
                    }
                }

                AI.SelectCard(orcustLink);
                _plugin.OrcustStrat.DingirsuSummonedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool ShouldLongirsuSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.Longirsu)) return false;

            // NEVER sacrifice Dingirsu or Galatea!
            // Only summon Longirsu if we have 3+ expendable monsters on field, and one is Orcust
            var expendables = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && !_plugin.OrcustMat.IsProtectedBoss(m)).ToList();
            bool hasOrcust = expendables.Any(m => OrcustStrategy.IsOrcustMonster(m));
            if (expendables.Count >= 3 && hasOrcust)
            {
                AI.SelectCard(expendables);
                return true;
            }

            return false;
        }

        private bool ShouldIPMasquerenaSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.IPMasquerena)) return false;

            // Only make Masquerena on Turn 1 if we have 2+ EXPENDABLE non-link monsters
            var expendables = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && !m.HasType(CardType.Link) && !_plugin.OrcustMat.IsProtectedBoss(m)).ToList();
            if (expendables.Count >= 2 && Duel.Turn == 1)
            {
                AI.SelectCard(expendables);
                return true;
            }
            return false;
        }

        private bool ShouldKnightmarePhoenixSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.KnightmarePhoenix)) return false;
            var expendables = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && !_plugin.OrcustMat.IsProtectedBoss(m)).ToList();
            if (expendables.Count >= 2 && Enemy.GetSpellCount() > 0 && Bot.Hand.Any(c => c != Card))
            {
                AI.SelectCard(expendables);
                return true;
            }
            return false;
        }

        private bool ShouldKnightmareCerberusSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.KnightmareCerberus)) return false;
            var expendables = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && !_plugin.OrcustMat.IsProtectedBoss(m)).ToList();
            if (expendables.Count >= 2 && Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.HasType(CardType.SpSummon)) && Bot.Hand.Any(c => c != Card))
            {
                AI.SelectCard(expendables);
                return true;
            }
            return false;
        }

        private bool ShouldDharcSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.DharcTheDarkCharmer)) return false;
            var expendables = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && !_plugin.OrcustMat.IsProtectedBoss(m)).ToList();
            bool enemyHasDarkInGY = Enemy.Graveyard.Any(c => (c.Attribute & (int)CardAttribute.Dark) != 0);
            if (expendables.Count >= 2 && enemyHasDarkInGY)
            {
                AI.SelectCard(expendables);
                return true;
            }
            return false;
        }

        private bool ShouldSPLittleKnightSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.SPLittleKnight)) return false;
            var expendables = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && !_plugin.OrcustMat.IsProtectedBoss(m)).ToList();
            if (expendables.Count >= 2 && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0 || Duel.Player == 1))
            {
                AI.SelectCard(expendables);
                return true;
            }
            return false;
        }

        private bool ShouldEnlilgirsuSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.Enlilgirsu)) return false;
            var expendables = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && !_plugin.OrcustMat.IsProtectedBoss(m)).ToList();
            bool hasOrcustLink = expendables.Any(m => m.HasType(CardType.Link) && OrcustStrategy.IsOrcustCard(m));
            if (expendables.Count >= 3 && hasOrcustLink && Enemy.GetMonsterCount() > 0)
            {
                AI.SelectCard(expendables);
                return true;
            }
            return false;
        }

        private bool ShouldAccesscodeSpSummon()
        {
            // Accesscode Talker requires 2+ Effect Monsters
            // NEVER use Dingirsu as material!
            var link3or2 = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.HasType(CardType.Link) && m.LinkMarker >= 2 && !_plugin.OrcustMat.IsProtectedBoss(m));
            // If no expendable Link-2/3, allow upgrading Longirsu or Little Knight ONLY if going for game
            if (link3or2 == null && (Enemy.LifePoints <= 5300 || _plugin.BoardAssessor.HasLethalOnBoard()))
            {
                link3or2 = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.HasType(CardType.Link) && m.LinkMarker >= 2 && m.Id != CardId.Dingirsu && m.Id != CardId.DingirsuAlt);
            }
            if (link3or2 == null) return false;

            var otherExpendables = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && m != link3or2 && !_plugin.OrcustMat.IsProtectedBoss(m)).ToList();
            if (!otherExpendables.Any()) return false;

            if (Enemy.GetMonsterCount() > 0 || Enemy.LifePoints <= 5300)
            {
                AI.SelectCard(new[] { link3or2, otherExpendables[0] });
                return true;
            }
            return false;
        }
        #endregion

        #region Tier 7: Backrow Setting
        private bool ShouldImpermanenceSet()
        {
            return Duel.Player == 0 && Duel.Phase == DuelPhase.Main2;
        }

        private bool ShouldDropletSet()
        {
            return Duel.Player == 0;
        }

        private bool ShouldCalledBySet()
        {
            return Duel.Player == 0;
        }

        private bool ShouldCrossoutSet()
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
                var nonBoss = cards.Where(c => !_plugin.OrcustMat.IsProtectedBoss(c)).ToList();
                if (!nonBoss.Any())
                {
                    if (cancelable) return new List<ClientCard>();
                    nonBoss = cards.ToList();
                }
                var sorted = nonBoss.OrderByDescending(c => _plugin.OrcustMat.ScoreTributeOrCostMaterial(c)).ToList();
                return sorted.Take(min).ToList();
            }

            // Hint 501: Discard
            if (hint == 501)
            {
                var nonBoss = cards.Where(c => !_plugin.OrcustMat.IsProtectedBoss(c)).ToList();
                if (!nonBoss.Any())
                {
                    if (cancelable) return new List<ClientCard>();
                    nonBoss = cards.ToList();
                }
                var sorted = nonBoss.OrderByDescending(c => _plugin.OrcustMat.ScoreDiscardMaterial(c)).ToList();
                return sorted.Take(min).ToList();
            }

            // Hint 502: Destroy / Hint 503: Remove / Hint 505: Return to Hand
            if (hint == 502 || hint == 503 || hint == 505)
            {
                var oppCards = cards.Where(c => c.Controller == 1).ToList();
                if (oppCards.Any())
                {
                    var sorted = oppCards.OrderByDescending(c => _plugin.OrcustThreat.EvaluateTargetPriority(c)).ToList();
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
                    var sorted = oppCards.OrderByDescending(c => _plugin.OrcustThreat.EvaluateTargetPriority(c)).ToList();
                    return sorted.Take(Math.Min(max, sorted.Count)).ToList();
                }
                if (cancelable)
                {
                    return new List<ClientCard>();
                }
            }

            // Hint 504: Send to GY (Dingirsu non-targeting send OR Foolish / Starter dumping OR Droplet / Cost send)
            if (hint == 504)
            {
                var oppCards = cards.Where(c => c.Controller == 1).ToList();
                if (oppCards.Any())
                {
                    var sorted = oppCards.OrderByDescending(c => _plugin.OrcustThreat.EvaluateTargetPriority(c)).ToList();
                    return sorted.Take(min).ToList();
                }

                // Check if this is Deck dumping (e.g. Foolish Burial, Scrap Recycler, Armageddon Knight, Girsu)
                bool isDeckDump = cards.Any(c => c.Location == CardLocation.Deck);
                if (isDeckDump)
                {
                    // Starter dumping priority (Deck -> GY)
                    var selected = new List<ClientCard>();
                    var harp = cards.FirstOrDefault(c => c.Id == CardId.OrcustHarpHorror && !_plugin.OrcustStrat.HarpHorrorUsed);
                    if (harp != null) selected.Add(harp);

                    var knightmare = cards.FirstOrDefault(c => c.Id == CardId.OrcustKnightmare && !_plugin.OrcustStrat.OrcustKnightmareUsed);
                    if (knightmare != null && !selected.Contains(knightmare)) selected.Add(knightmare);

                    var wand = cards.FirstOrDefault(c => c.Id == CardId.WorldWand && !_plugin.OrcustStrat.WorldWandUsed);
                    if (wand != null && !selected.Contains(wand)) selected.Add(wand);

                    var cymbal = cards.FirstOrDefault(c => c.Id == CardId.OrcustCymbalSkeleton && !_plugin.OrcustStrat.CymbalSkeletonUsed);
                    if (cymbal != null && !selected.Contains(cymbal)) selected.Add(cymbal);

                    var brass = cards.FirstOrDefault(c => c.Id == CardId.OrcustBrassBombard && !_plugin.OrcustStrat.BrassBombardUsed);
                    if (brass != null && !selected.Contains(brass)) selected.Add(brass);

                    var girsu = cards.FirstOrDefault(c => c.Id == CardId.Girsu);
                    if (girsu != null && !selected.Contains(girsu)) selected.Add(girsu);

                    foreach (var c in cards)
                    {
                        if (selected.Count >= min) break;
                        if (!selected.Contains(c)) selected.Add(c);
                    }
                    return selected.Take(min).ToList();
                }

                // Otherwise, this is sending cards from Field or Hand as COST (e.g. Forbidden Droplet, Orcustrated Return, etc.)
                // STRICT RULE: NEVER SEND PROTECTED BOSSES (Dingirsu, Galatea, Longirsu, Accesscode, Little Knight, etc.)!
                var validCostCards = cards.Where(c => !_plugin.OrcustMat.IsProtectedBoss(c)).ToList();
                if (!validCostCards.Any())
                {
                    if (cancelable) return new List<ClientCard>();
                    validCostCards = cards.ToList();
                }

                var costSorted = validCostCards.OrderByDescending(c =>
                {
                    if (c.Location == CardLocation.Hand)
                    {
                        return _plugin.OrcustMat.ScoreDiscardMaterial(c);
                    }
                    else if (c.Location == CardLocation.MonsterZone)
                    {
                        return _plugin.OrcustMat.ScoreTributeOrCostMaterial(c);
                    }
                    else if (c.Location == CardLocation.SpellZone)
                    {
                        if (c.Id == CardId.OrcustratedBabel || c.Id == CardId.OrcustCrescendo) return -9999;
                        return 500;
                    }
                    return 100;
                }).ToList();

                return costSorted.Take(min).ToList();
            }

            // Hint 506: Add to Hand / Search
            if (hint == 506)
            {
                var selected = new List<ClientCard>();

                // RotA search priority: ArmageddonKnight > DarkGrepher
                var arma = cards.FirstOrDefault(c => c.Id == CardId.ArmageddonKnight && !_plugin.OrcustStrat.ArmageddonKnightUsed && Bot.GetMonsterCount() == 0);
                if (arma != null) selected.Add(arma);

                var grepher = cards.FirstOrDefault(c => c.Id == CardId.DarkGrepher);
                if (grepher != null && !selected.Contains(grepher)) selected.Add(grepher);

                // Galatea / Galatea-i search priority:
                if (!Bot.SpellZone.Any(s => s != null && s.Id == CardId.OrcustCrescendo))
                {
                    var crescendo = cards.FirstOrDefault(c => c.Id == CardId.OrcustCrescendo);
                    if (crescendo != null && !selected.Contains(crescendo)) selected.Add(crescendo);
                }

                if (!_plugin.OrcustStrat.HasBabelOnField() && !Bot.HasInHand(CardId.OrcustratedBabel))
                {
                    var babel = cards.FirstOrDefault(c => c.Id == CardId.OrcustratedBabel);
                    if (babel != null && !selected.Contains(babel)) selected.Add(babel);
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

                var harp = cards.FirstOrDefault(c => c.Id == CardId.OrcustHarpHorror);
                if (harp != null) selected.Add(harp);

                var wand = cards.FirstOrDefault(c => c.Id == CardId.WorldWand);
                if (wand != null && !selected.Contains(wand)) selected.Add(wand);

                var brass = cards.FirstOrDefault(c => c.Id == CardId.OrcustBrassBombard);
                if (brass != null && !selected.Contains(brass)) selected.Add(brass);

                var cymbal = cards.FirstOrDefault(c => c.Id == CardId.OrcustCymbalSkeleton);
                if (cymbal != null && !selected.Contains(cymbal)) selected.Add(cymbal);

                var galateaI = cards.FirstOrDefault(c => c.Id == CardId.GalateaI);
                if (galateaI != null && !selected.Contains(galateaI)) selected.Add(galateaI);

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
                var dingirsu = cards.FirstOrDefault(c => c.Id == CardId.Dingirsu || c.Id == CardId.DingirsuAlt);
                if (dingirsu != null) selected.Add(dingirsu);

                var galatea = cards.FirstOrDefault(c => c.Id == CardId.Galatea);
                if (galatea != null && !selected.Contains(galatea)) selected.Add(galatea);

                // Harp Horror deck summon priority:
                if (Bot.GetMonsterCount() == 0)
                {
                    var girsu = cards.FirstOrDefault(c => c.Id == CardId.Girsu);
                    if (girsu != null && !selected.Contains(girsu)) selected.Add(girsu);
                }

                var cymbal = cards.FirstOrDefault(c => c.Id == CardId.OrcustCymbalSkeleton);
                if (cymbal != null && !selected.Contains(cymbal)) selected.Add(cymbal);

                var knightmare = cards.FirstOrDefault(c => c.Id == CardId.OrcustKnightmare);
                if (knightmare != null && !selected.Contains(knightmare)) selected.Add(knightmare);

                var brass = cards.FirstOrDefault(c => c.Id == CardId.OrcustBrassBombard);
                if (brass != null && !selected.Contains(brass)) selected.Add(brass);

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

                var wand = cards.FirstOrDefault(c => c.Id == CardId.WorldWand);
                if (wand != null && !selected.Contains(wand)) selected.Add(wand);

                foreach (var c in cards)
                {
                    if (selected.Count >= min) break;
                    if (!selected.Contains(c)) selected.Add(c);
                }
                if (selected.Any()) return selected.Take(min).ToList();

                var sorted = cards.OrderByDescending(c => _plugin.OrcustMat.ScoreTributeOrCostMaterial(c)).ToList();
                return sorted.Take(min).ToList();
            }

            // Hint 533: Link Material
            if (hint == 533)
            {
                var nonBoss = cards.Where(c => !_plugin.OrcustMat.IsProtectedBoss(c)).ToList();
                if (!nonBoss.Any())
                {
                    if (cancelable) return new List<ClientCard>();
                    nonBoss = cards.ToList();
                }
                var sorted = nonBoss.OrderByDescending(c => _plugin.OrcustMat.ScoreTributeOrCostMaterial(c)).ToList();
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
                var sorted = oppCardsGeneric.OrderByDescending(c => _plugin.OrcustThreat.EvaluateTargetPriority(c)).ToList();
                return sorted.Take(Math.Min(max, sorted.Count)).ToList();
            }

            // Protect our own boss monsters in any unexpected selection fallback
            var ourCardsGeneric = cards.Where(c => c.Controller == 0).ToList();
            if (ourCardsGeneric.Any())
            {
                var nonBoss = ourCardsGeneric.Where(c => !_plugin.OrcustMat.IsProtectedBoss(c)).ToList();
                if (nonBoss.Count >= min)
                {
                    var sorted = nonBoss.OrderByDescending(c => _plugin.OrcustMat.ScoreTributeOrCostMaterial(c)).ToList();
                    return sorted.Take(min).ToList();
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override int OnSelectOption(IList<long> options)
        {
            // Dingirsu On-Summon options:
            // Option 0: Send 1 card your opponent controls to the GY
            // Option 1: Attach 1 of your banished Machine monsters to this card as material
            long ding0 = Util.GetStringId(CardId.Dingirsu, 0);
            long ding1 = Util.GetStringId(CardId.Dingirsu, 1);

            if (options.Contains(ding0) && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0))
            {
                return options.IndexOf(ding0);
            }
            if (options.Contains(ding1) && Bot.Banished.Any(c => c != null && (c.Race & (int)CardRace.Machine) != 0))
            {
                return options.IndexOf(ding1);
            }
            if (options.Contains(ding0))
            {
                return options.IndexOf(ding0);
            }

            // Triple Tactics Talent options:
            // Option 0: Draw 2 cards
            // Option 1: Take control of 1 monster opponent controls until End Phase
            // Option 2: Look at opponent's hand and choose 1 card to shuffle into Deck
            long ttt0 = Util.GetStringId(CardId.TripleTacticsTalent, 0);
            long ttt1 = Util.GetStringId(CardId.TripleTacticsTalent, 1);
            long ttt2 = Util.GetStringId(CardId.TripleTacticsTalent, 2);

            if (options.Contains(ttt1) && Enemy.GetMonsterCount() > 0 && Duel.Player == 0 && Duel.Phase == DuelPhase.Main1)
            {
                return options.IndexOf(ttt1); // Take control of opponent's boss!
            }
            if (options.Contains(ttt0))
            {
                return options.IndexOf(ttt0); // Draw 2 cards!
            }

            return base.OnSelectOption(options);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.Dingirsu || cardId == CardId.DingirsuAlt)
            {
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }
            if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;

            return base.OnSelectPosition(cardId, positions);
        }
        #endregion
    }
}
