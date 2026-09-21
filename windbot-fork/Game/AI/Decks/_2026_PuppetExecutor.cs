// ============================================================
// CARD AUDIT โ€” 2026_Puppet
// | Card Name              | Type      | OPT? | Cost        | Effect Summary                        | Activate When                    | NEVER When                        |
// |------------------------|-----------|------|-------------|---------------------------------------|----------------------------------|-----------------------------------|
// | Imsety, Glory of Horus | Monster   | HOPT | Discard 2   | Search Sarcophagus + Draw 1           | Have Sarc in Deck + discardable  | Discarding key starter/no target  |
// | Hapi, Guidance of Horus| Monster   | HOPT | None        | SS from GY with Sarc / GY recycle     | Sarc on field                    | SS blocked                        |
// | King's Sarcophagus     | Cont.Spell| 4/T  | Discard 1   | Send Horus from Deck to GY            | Need Horus in GY                 | No discard / Hapi already in GY   |
// | Mansion of Dread Dolls | Field     | HOPT | Detach 1    | Search GP on activate / SS GP to opp  | Setup starter / Burn loop        | No GP in Deck / No Xyz to detach  |
// | Little Soldiers         | Monster   | HOPT | None/Banish | Dump GP -> change Lv / GY Lv+4 boost  | Normal/SS / Need Rank 8 mod      | SS blocked                        |
// | Rouge Doll             | Monster   | HOPT | Reveal Xyz  | SS self + SS GP from deck / GY recover| Hand extender / Rank 8 setup     | Locked / No Xyz to reveal         |
// | Bisque Doll            | Monster   | OPT  | Discard GP  | SS self from hand / GY target protect | Hand extender / Protect GP       | No GP in hand / Already protected |
// | Fiendish Knight        | Monster   | HOPT | Target GY   | SS target to owner + SS self          | Hand extender / Field presence   | No target in GY                   |
// | Cattle Scream          | Monster   | HOPT | Detach 1    | SS self from hand/GY / Opp SS trigger | Detach from Xyz to extend        | No Xyz on field                   |
// | Terror Baby            | Monster   | None | Banish GY   | Revive GP on NS / GY response protect | Normal summon / Protect combo    | No GP in GY                       |
// | Argent Chaos Force     | Spell/RUM | OPT* | Target R8   | Rank-Up into Fanatix / GY recycle     | Have Fantasix/R8 / R5+ SS trigger| No CXyz in Extra Deck             |
// | Fantasix Machinix      | Xyz (R8)  | HOPT | Detach 1    | Search RUM + Extra NS / GY revive+RUM | Have RUM in Deck / GP Xyz SS'd   | No material / RUM depleted        |
// | Fanatix Machinix       | CXyz (R9) | HOPT | Detach 1    | Search Puppet Trap / SS to opp & Burn | On SS / Opp field burn trigger   | No trap in Deck / Burn not lethal |
// | Chimera Doll           | Link-2    | HOPT | None        | Search or SS GP from deck             | Need GP extender                 | Locked out of Extra              |
// | Service Puppet Play    | Trap      | HOPT | None/Banish | Take control of opp monster / GY SS   | Opp has monsters / GY revive     | No GP Xyz on field                |
// ============================================================
// ACE CARDS:
//   Primary: CXyz Gimmick Puppet Fanatix Machinix (3685372) โ€” 3100 ATK, Search Trap, SS to Opp & Destroy Burn
//   Secondary: Gimmick Puppet Fantasix Machinix (76290637) โ€” Search RUM Argent Chaos Force, Extra NS, GY Revival
//   Tertiary: Gimmick Puppet Chimera Doll (97520532), Number C15 Giant Hunter (33776843), Number 15 Giant Grinder (88120966)
// COMBO STARTERS:
//   1. Imsety, Glory of Horus (84941194) -> Search Sarcophagus -> Dump Hapi -> Overlay Rank 8 Fantasix
//   2. Mansion of the Dreadful Dolls (36890111) -> Search Little Soldiers / Rouge Doll -> Rank 8 Fantasix
//   3. Gimmick Puppet Little Soldiers (36436372) -> Dump Rouge Doll (Lv8) -> Become Lv8 -> Rank 8 Fantasix
//   4. Gimmick Puppet Rouge Doll (63825486) -> Reveal Fanatix -> SS Rouge + Bisque/Cattle (Lv8) -> Rank 8
// WIN CONDITION:
//   Establish Fantasix Machinix -> Detach to search Argent Chaos Force -> Rank up into CXyz Fanatix Machinix ->
//   Search Service Puppet Play -> Detach to SS high ATK monster to opponent field -> Trigger Fanatix destroy & burn ->
//   Attack for OTK or pass with multiple disruptions (Service Puppet Play + Fanatix Quick Pop + Sarcophagus Protection).
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
    [Deck("2026_Puppet", "2026_Puppet")]
    public class _2026_PuppetExecutor : ModernExecutor
    {
        public class CardId
        {
            // Horus Engine
            public const int ImsetyHorus = 84941194;
            public const int HapiHorus = 47330808;
            public const int KingsSarcophagus = 16528181;

            // Gimmick Puppet Main Deck
            public const int GPCattleScream = 99229085;
            public const int GPBisqueDoll = 79086452;
            public const int GPRougeDoll = 63825486;
            public const int GPFiendishKnight = 4145915;
            public const int GPTerrorBaby = 43598843;
            public const int GPLittleSoldiers = 36436372;

            // Spells & Traps
            public const int CondolencePuppet = 57093995;
            public const int Terraforming = 73628505;
            public const int ArgentChaosForce = 94220427;
            public const int MansionDreadfulDolls = 36890111;
            public const int ServicePuppetPlay = 36400569;

            // Staples & Hand Traps
            public const int AshBlossom = 14558127;
            public const int MaxxC = 23434538;
            public const int MulcharmyFuwalos = 42141493;
            public const int CalledByTheGrave = 24224830;
            public const int CrossoutDesignator = 65681983;
            public const int TripleTacticsTalent = 25311006;
            public const int InfiniteImpermanence = 10045474;
            public const int DominusImpulse = 40366667;

            // Extra Deck
            public const int GPDarkStrings = 69170557;
            public const int GPFanatixMachinix = 3685372;
            public const int GPGiantHunter = 33776843;
            public const int GPStrings = 75433814;
            public const int GPFantasixMachinix = 76290637;
            public const int GPGiantGrinder = 88120966;
            public const int GPGigantesDoll = 7593748;
            public const int SPLittleKnight = 29301450;
            public const int GPChimeraDoll = 97520532;
        }

        private static readonly int[] AceCardIds = {
            CardId.GPFanatixMachinix,
            CardId.GPFantasixMachinix,
            CardId.GPChimeraDoll,
            CardId.GPGiantGrinder,
            CardId.GPGiantHunter,
            CardId.SPLittleKnight
        };

        private static readonly int[] GPMonsters = {
            CardId.GPCattleScream,
            CardId.GPBisqueDoll,
            CardId.GPRougeDoll,
            CardId.GPFiendishKnight,
            CardId.GPTerrorBaby,
            CardId.GPLittleSoldiers
        };

        // Per-turn activation trackers
        private bool _imsetyUsed = false;
        private bool _sarcDumpUsed = false;
        private bool _mansionHandUsed = false;
        private bool _mansionFieldUsed = false;
        private bool _soldiersDumpUsed = false;
        private bool _soldiersGYUsed = false;
        private bool _rougeHandUsed = false;
        private bool _bisqueHandUsed = false;
        private bool _bisqueGYUsed = false;
        private bool _fiendishHandUsed = false;
        private bool _cattleHandGYUsed = false;
        private bool _fantasixSearchUsed = false;
        private bool _fanatixSearchUsed = false;
        private bool _fanatixSSOppUsed = false;
        private bool _fanatixBurnUsed = false;
        private bool _condolenceUsed = false;
        private bool _gpExtraLocked = false;

        public _2026_PuppetExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            HeuristicGuard.RegisterAceCards(AceCardIds);
            ResourcePlan.RegisterAceCards(AceCardIds);

            // โ”€โ”€ Combo Router Setup โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Horus-Fantasix-Fanatix-Line",
                RequiredCards = new List<int> { CardId.ImsetyHorus },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.ImsetyHorus, ActionType = ExecutorType.Activate, Description = "Imsety search King's Sarcophagus" },
                    new() { CardId = CardId.KingsSarcophagus, ActionType = ExecutorType.Activate, Description = "Activate King's Sarcophagus" },
                    new() { CardId = CardId.GPFantasixMachinix, ActionType = ExecutorType.SpSummon, Description = "Overlay Horus into Fantasix Machinix" },
                    new() { CardId = CardId.GPFantasixMachinix, ActionType = ExecutorType.Activate, Description = "Fantasix search Argent Chaos Force" },
                    new() { CardId = CardId.ArgentChaosForce, ActionType = ExecutorType.Activate, Description = "Rank-Up into Fanatix Machinix" }
                },
                EndBoardScore = 95
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Mansion-LittleSoldiers-Line",
                RequiredCards = new List<int> { CardId.MansionDreadfulDolls },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.MansionDreadfulDolls, ActionType = ExecutorType.Activate, Description = "Activate Mansion -> search Little Soldiers" },
                    new() { CardId = CardId.GPLittleSoldiers, ActionType = ExecutorType.Summon, Description = "Normal Summon Little Soldiers" },
                    new() { CardId = CardId.GPLittleSoldiers, ActionType = ExecutorType.Activate, Description = "Dump Rouge Doll -> Level 8" },
                    new() { CardId = CardId.GPFantasixMachinix, ActionType = ExecutorType.SpSummon, Description = "Xyz into Fantasix Machinix" }
                },
                EndBoardScore = 90
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "RougeDoll-Extender-Line",
                RequiredCards = new List<int> { CardId.GPRougeDoll },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.GPRougeDoll, ActionType = ExecutorType.Activate, Description = "Reveal Xyz -> SS Rouge + GP from Deck" },
                    new() { CardId = CardId.GPFantasixMachinix, ActionType = ExecutorType.SpSummon, Description = "Rank 8 Xyz into Fantasix" }
                },
                EndBoardScore = 85
            });

            BaitPlanner.RegisterComboStarters(CardId.ImsetyHorus, CardId.MansionDreadfulDolls, CardId.Terraforming, CardId.GPLittleSoldiers);
            BaitPlanner.RegisterBaitCards(CardId.CondolencePuppet, CardId.TripleTacticsTalent);
            ChainAdvisor.RegisterHighValueTargets(CardId.ImsetyHorus, CardId.MansionDreadfulDolls, CardId.GPFantasixMachinix, CardId.ArgentChaosForce);

            // โ•โ•โ•โ• TIER 1: Hand Traps & Fast Negates โ•โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCActivate);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshActivate);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, FuwalosEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.CrossoutDesignator, CrossoutDesignatorEffect);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, DefaultInfiniteImpermanence);
            AddExecutor(ExecutorType.Activate, CardId.DominusImpulse, DominusEffect);

            // โ•โ•โ•โ• TIER 2: Fast Quick Effects & Removal โ•โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.GPFanatixMachinix, FanatixEffect);
            AddExecutor(ExecutorType.Activate, CardId.ServicePuppetPlay, ServicePuppetEffect);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, LittleKnightEffect);
            AddExecutor(ExecutorType.Activate, CardId.GPGiantHunter, GiantHunterEffect);
            AddExecutor(ExecutorType.Activate, CardId.GPGiantGrinder, GiantGrinderEffect);

            // โ•โ•โ•โ• TIER 3: Field & Starter Spells โ•โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.Terraforming, TerraformingEffect);
            AddExecutor(ExecutorType.Activate, CardId.MansionDreadfulDolls, MansionEffect);
            AddExecutor(ExecutorType.Activate, CardId.CondolencePuppet, CondolenceEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TalentEffect);

            // โ•โ•โ•โ• TIER 4: Horus Engine Plays โ•โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.ImsetyHorus, ImsetyEffect);
            AddExecutor(ExecutorType.Activate, CardId.KingsSarcophagus, SarcophagusEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.ImsetyHorus, HorusSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.HapiHorus, HorusSummon);

            // โ•โ•โ•โ• TIER 5: Gimmick Puppet Hand Special Summons & Ignitions โ•โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.GPRougeDoll, RougeDollEffect);
            AddExecutor(ExecutorType.Activate, CardId.GPBisqueDoll, BisqueDollEffect);
            AddExecutor(ExecutorType.Activate, CardId.GPFiendishKnight, FiendishKnightEffect);
            AddExecutor(ExecutorType.Activate, CardId.GPCattleScream, CattleScreamEffect);

            // โ•โ•โ•โ• TIER 6: Normal Summons & On-Summon Triggers โ•โ•โ•โ•
            AddExecutor(ExecutorType.Summon, CardId.GPLittleSoldiers, LittleSoldiersSummon);
            AddExecutor(ExecutorType.Activate, CardId.GPLittleSoldiers, LittleSoldiersEffect);
            AddExecutor(ExecutorType.Summon, CardId.GPTerrorBaby, TerrorBabySummon);
            AddExecutor(ExecutorType.Activate, CardId.GPTerrorBaby, TerrorBabyEffect);
            AddExecutor(ExecutorType.Summon, CardId.GPFiendishKnight, GenericGPSummon);

            // โ•โ•โ•โ• TIER 7: Rank-Up-Magic & Extra Deck Summons โ•โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.ArgentChaosForce, ArgentChaosForceEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.GPFantasixMachinix, FantasixSummon);
            AddExecutor(ExecutorType.Activate, CardId.GPFantasixMachinix, FantasixEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.GPFanatixMachinix, FanatixSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.GPChimeraDoll, ChimeraDollSummon);
            AddExecutor(ExecutorType.Activate, CardId.GPChimeraDoll, ChimeraDollEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.GPGiantGrinder, GiantGrinderSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.GPGiantHunter, GiantHunterSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.GPDarkStrings);
            AddExecutor(ExecutorType.SpSummon, CardId.GPStrings);
            AddExecutor(ExecutorType.SpSummon, CardId.GPGigantesDoll);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, LinkSummonCheck);

            // โ•โ•โ•โ• TIER 8: Traps & Repositioning โ•โ•โ•โ•
            AddExecutor(ExecutorType.SpellSet, CardId.ServicePuppetPlay);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, () => Util.IsTurn1OrMain2());
            AddExecutor(ExecutorType.SpellSet, CardId.DominusImpulse, () => Util.IsTurn1OrMain2());
            AddExecutor(ExecutorType.SpellSet, SpellSetFiltered);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _imsetyUsed = false;
            _sarcDumpUsed = false;
            _mansionHandUsed = false;
            _mansionFieldUsed = false;
            _soldiersDumpUsed = false;
            _soldiersGYUsed = false;
            _rougeHandUsed = false;
            _bisqueHandUsed = false;
            _bisqueGYUsed = false;
            _fiendishHandUsed = false;
            _cattleHandGYUsed = false;
            _fantasixSearchUsed = false;
            _fanatixSearchUsed = false;
            _fanatixSSOppUsed = false;
            _fanatixBurnUsed = false;
            _condolenceUsed = false;
            _gpExtraLocked = false;
        }

        public override bool OnSelectHand()
        {
            return true; // Go first for Fantasix -> Fanatix OTK / Burn / Lock setup
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Contains(card.Id);
        }

        protected override bool IsBoardStrongEnough()
        {
            if (Bot.HasInMonstersZone(CardId.GPFanatixMachinix) && (Bot.HasInSpellZone(CardId.ServicePuppetPlay) || Bot.HasInHand(CardId.ServicePuppetPlay)))
                return true;
            if (Bot.HasInMonstersZone(CardId.GPFanatixMachinix) && Bot.HasInSpellZone(CardId.KingsSarcophagus))
                return true;
            if (Bot.HasInMonstersZone(CardId.GPFantasixMachinix) && Bot.HasInHand(CardId.ArgentChaosForce))
                return false; // Still need to rank up
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (Bot.HasInMonstersZone(CardId.GPFanatixMachinix))
            {
                if (CanDealLethal() || OpponentHasActiveNegator())
                    return true;
            }
            return base.ShouldStopExtending();
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง 1. Smart Discard Engine (Protect Starters & Key Spells)
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private int GetDiscardScore(ClientCard card)
        {
            if (card == null) return 9999;
            // Best Discards (wants to be in GY or triggers immediately)
            if (card.IsCode(CardId.GPCattleScream)) return 10;
            if (card.IsCode(CardId.HapiHorus)) return 20;
            if (card.IsCode(CardId.GPBisqueDoll)) return 30;
            if (card.IsCode(CardId.GPTerrorBaby)) return 40;
            if (card.IsCode(CardId.GPRougeDoll)) return 50;
            if (card.IsCode(CardId.GPFiendishKnight)) return 60;

            // Extra copies of cards already on field or in hand
            if (card.IsCode(CardId.MansionDreadfulDolls) && Bot.HasInSpellZone(CardId.MansionDreadfulDolls)) return 70;
            if (card.IsCode(CardId.KingsSarcophagus) && Bot.HasInSpellZone(CardId.KingsSarcophagus)) return 75;
            if (card.IsCode(CardId.ArgentChaosForce) && (Bot.HasInHand(CardId.ArgentChaosForce) || Bot.HasInGraveyard(CardId.ArgentChaosForce))) return 80;
            if (card.IsCode(CardId.ImsetyHorus) && Bot.HasInSpellZone(CardId.KingsSarcophagus)) return 90;

            // Medium Discards (generic / staples if no other choice)
            if (card.IsCode(CardId.TripleTacticsTalent)) return 300;
            if (card.IsCode(CardId.CalledByTheGrave, CardId.CrossoutDesignator)) return 400;
            if (card.IsCode(CardId.AshBlossom, CardId.MaxxC, CardId.MulcharmyFuwalos, CardId.DominusImpulse)) return 600;

            // PROTECTED CARDS (NEVER discard if preventable)
            if (card.IsCode(CardId.GPLittleSoldiers)) return 800;
            if (card.IsCode(CardId.Terraforming)) return 900;
            if (card.IsCode(CardId.MansionDreadfulDolls)) return 1000;
            if (card.IsCode(CardId.ArgentChaosForce)) return 1100;
            if (card.IsCode(CardId.KingsSarcophagus)) return 1200;

            return 500;
        }

        private ClientCard SelectBestDiscard(IEnumerable<ClientCard> pool, params int[] excludedIds)
        {
            var candidates = pool.Where(c => c != null && !excludedIds.Contains(c.Id)).ToList();
            if (candidates.Count == 0) return null;
            return candidates.OrderBy(c => GetDiscardScore(c)).FirstOrDefault();
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            // When Ranking up Fantasix into Fanatix, Fantasix MUST be allowed!
            if (c.IsCode(CardId.GPFantasixMachinix))
            {
                if (Card != null && (Card.Id == CardId.ArgentChaosForce || Card.Id == CardId.GPFanatixMachinix))
                    return 10; // Top priority to be chosen as Rank-Up material!
                return 900;
            }

            if (IsAceCard(c))
            {
                if (c.Location == CardLocation.MonsterZone && c.IsFaceup())
                    return int.MaxValue;
                return 900;
            }

            if (c.IsCode(CardId.AshBlossom, CardId.MaxxC, CardId.MulcharmyFuwalos))
                return 800;

            // Preferred Xyz materials
            if (c.IsCode(CardId.GPCattleScream, CardId.GPBisqueDoll, CardId.GPRougeDoll))
                return 50;
            if (c.IsCode(CardId.GPLittleSoldiers) && c.Level == 8)
                return 60;
            if (c.IsCode(CardId.ImsetyHorus, CardId.HapiHorus))
                return 70;

            return 100;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง 2. OnSelectCard & Targeting Overrides
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Fanatix Machinix: destroy target on opponent's field (Trigger or Hint 502)
            if ((LastChainCard != null && LastChainCard.Id == CardId.GPFanatixMachinix) || hint == 502)
            {
                var oppMonsters = cards.Where(c => c != null && c.Controller == 1 && c.Location == CardLocation.MonsterZone)
                    .OrderByDescending(c => c.Attack)
                    .ToList();
                if (oppMonsters.Count > 0)
                {
                    DecisionTracer.TraceSelect("OnSelectCard", "Fanatix targeting highest ATK opponent monster to destroy and burn", oppMonsters.First());
                    return new List<ClientCard> { oppMonsters.First() };
                }
            }

            // Rouge Doll Extra Deck reveal
            if (Card != null && Card.Id == CardId.GPRougeDoll && cards.Any(c => c.Location == CardLocation.Extra))
            {
                var extraGP = cards.FirstOrDefault(c => c.Id == CardId.GPFanatixMachinix || c.Id == CardId.GPFantasixMachinix || c.Id == CardId.GPGiantGrinder);
                if (extraGP != null)
                {
                    DecisionTracer.TraceSelect("OnSelectCard", "Rouge Doll revealing Extra Deck Xyz", extraGP);
                    return new List<ClientCard> { extraGP };
                }
            }

            // Hint 505 / 506: Deck search selections & additions to hand
            if (hint == 505 || hint == 506)
            {
                if (Card != null && Card.Id == CardId.GPFantasixMachinix)
                {
                    var rum = cards.FirstOrDefault(c => c.Id == CardId.ArgentChaosForce);
                    if (rum != null) return new List<ClientCard> { rum };
                }

                if (Card != null && Card.Id == CardId.GPFanatixMachinix)
                {
                    var trap = cards.FirstOrDefault(c => c.Id == CardId.ServicePuppetPlay);
                    if (trap != null) return new List<ClientCard> { trap };
                }

                if (Card != null && (Card.Id == CardId.MansionDreadfulDolls || Card.Id == CardId.Terraforming))
                {
                    var preferred = cards.Where(c => c.IsCode(CardId.GPLittleSoldiers, CardId.GPRougeDoll, CardId.GPFiendishKnight, CardId.GPCattleScream))
                        .OrderBy(c => c.Id == CardId.GPLittleSoldiers ? 1 : (c.Id == CardId.GPRougeDoll ? 2 : 3))
                        .FirstOrDefault();
                    if (preferred != null) return new List<ClientCard> { preferred };
                }
            }

            // Hint 508: Send to GY / Dump from Deck
            if (hint == 508)
            {
                // Little Soldiers: Dump Lv8 (Rouge Doll / Cattle Scream / Bisque Doll) so Little Soldiers becomes Lv8
                if (Card != null && Card.Id == CardId.GPLittleSoldiers)
                {
                    var lv8Target = cards.FirstOrDefault(c => c.Id == CardId.GPRougeDoll)
                                 ?? cards.FirstOrDefault(c => c.Id == CardId.GPCattleScream)
                                 ?? cards.FirstOrDefault(c => c.Id == CardId.GPBisqueDoll);
                    if (lv8Target != null) return new List<ClientCard> { lv8Target };
                }

                // King's Sarcophagus: Dump Hapi
                if (Card != null && Card.Id == CardId.KingsSarcophagus)
                {
                    var hapi = cards.FirstOrDefault(c => c.Id == CardId.HapiHorus);
                    if (hapi != null) return new List<ClientCard> { hapi };
                }

                // Condolence Puppet: Send different GP monsters from deck to GY
                if (Card != null && Card.Id == CardId.CondolencePuppet)
                {
                    var sorted = cards.OrderBy(c =>
                    {
                        if (c.Id == CardId.GPRougeDoll) return 1;
                        if (c.Id == CardId.GPCattleScream) return 2;
                        if (c.Id == CardId.GPBisqueDoll) return 3;
                        if (c.Id == CardId.GPTerrorBaby) return 4;
                        return 10;
                    }).ToList();
                    return Util.CheckSelectCount(sorted, cards, min, max);
                }
            }

            // Hint 509: Special Summon from Deck / Extra Deck
            if (hint == 509)
            {
                // Argent Chaos Force: Rank-Up into Fanatix / Giant Hunter from Extra Deck
                if (Card != null && Card.Id == CardId.ArgentChaosForce)
                {
                    var fanatix = cards.FirstOrDefault(c => c.Id == CardId.GPFanatixMachinix)
                               ?? cards.FirstOrDefault(c => c.Id == CardId.GPGiantHunter)
                               ?? cards.FirstOrDefault(c => c.Id == CardId.GPDarkStrings);
                    if (fanatix != null) return new List<ClientCard> { fanatix };
                }

                var deckCards = cards.Where(c => c != null && c.Location == CardLocation.Deck).ToList();
                if (deckCards.Count > 0)
                {
                    var preferred = deckCards.OrderBy(c => {
                        if (c.Id == CardId.GPCattleScream) return 1;
                        if (c.Id == CardId.GPBisqueDoll) return 2;
                        if (c.Id == CardId.GPRougeDoll) return 3;
                        if (c.Id == CardId.GPLittleSoldiers) return 4;
                        return 10;
                    }).ToList();
                    return Util.CheckSelectCount(preferred, cards, min, max);
                }
            }

            // Xyz Material selection (Hint 511/512/513/519/533)
            if (hint == 511 || hint == 512 || hint == 513 || hint == 519 || hint == 533)
            {
                var sorted = cards.Where(c => c != null)
                    .OrderBy(c => GetMaterialPriority(c))
                    .ToList();
                return Util.CheckSelectCount(sorted, cards, min, max);
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override IList<ClientCard> OnSelectXyzMaterial(IList<ClientCard> cards, int min, int max)
        {
            var sorted = cards.Where(c => c != null)
                .OrderBy(c => GetMaterialPriority(c))
                .ToList();
            return Util.CheckSelectCount(sorted, cards, min, max);
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง 3. Hand Traps & Fast Reactives
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool MaxxCActivate()
        {
            if (!SmartHandTrapChain()) return false;
            if (Duel.LastChainPlayer == 0) return false;
            return DefaultMaxxC();
        }

        private bool AshActivate()
        {
            if (!SmartHandTrapChain()) return false;
            if (Duel.LastChainPlayer == 0) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool FuwalosEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1 && Bot.GetMonsterCount() == 0 && Bot.GetSpellCount() == 0;
        }

        private bool CalledByTheGraveEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                ClientCard target = Util.GetLastChainCard();
                if (target != null && (target.Location == CardLocation.Grave || target.Location == CardLocation.Hand))
                {
                    return DefaultCalledByTheGrave();
                }
            }
            return false;
        }

        private bool CrossoutDesignatorEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (Duel.LastChainPlayer == 1)
            {
                ClientCard lastChain = Util.GetLastChainCard();
                if (lastChain != null && lastChain.Controller == 1 && lastChain.IsMonster())
                {
                    int code = lastChain.Id;
                    int alias = lastChain.Alias;
                    if (alias != 0 && alias - code < 10) code = alias;
                    if (code != 0 && GetRemainingCount(code) > 0)
                    {
                        AI.SelectAnnounceID(code);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool DominusEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.LastChainPlayer == 1;
        }

        private bool TalentEffect()
        {
            if (Duel.Player == 0 && (Enemy.GetMonsterCount() > 0 || Duel.LastChainPlayer == 1))
            {
                // Prioritize Draw 2 on turn 1, or take control of problem monster going 2nd
                if (Enemy.GetMonsterCount() >= 1 && Duel.Turn > 1)
                {
                    AI.SelectOption(1); // Take control
                    return true;
                }
                AI.SelectOption(0); // Draw 2
                return true;
            }
            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง 4. Boss Effects (Fanatix / Fantasix / Giant Grinder)
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool FanatixEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Trigger 1: When Special Summoned -> Search Service Puppet Play
                if (!_fanatixSearchUsed && GetRemainingCount(CardId.ServicePuppetPlay) > 0)
                {
                    _fanatixSearchUsed = true;
                    DecisionTracer.TraceActivate("FanatixEffect", "Search Service Puppet Play");
                    AI.SelectCard(CardId.ServicePuppetPlay);
                    return true;
                }

                // Trigger 2: When monster SS'd to opponent's field -> Destroy & Burn
                if (!_fanatixBurnUsed)
                {
                    var oppMonster = Enemy.MonsterZone.Where(c => c != null && c.IsFaceup()).OrderByDescending(c => c.Attack).FirstOrDefault();
                    if (oppMonster != null)
                    {
                        _fanatixBurnUsed = true;
                        AI.SelectCard(oppMonster);
                        DecisionTracer.TraceActivate("FanatixEffect", $"Destroy and burn opponent monster {oppMonster.Name}");
                        return true;
                    }
                }

                // Ignition Effect: Detach 1 -> SS monster from GY to opponent field (burn setup)
                // Only do this if Fanatix can immediately burn it and opponent has room
                if (!_fanatixSSOppUsed && !_fanatixBurnUsed && Card.HasXyzMaterial() && Duel.Phase == DuelPhase.Main1 && Enemy.GetMonsterCount() < 5)
                {
                    var gyTarget = Bot.Graveyard.Concat(Enemy.Graveyard)
                        .Where(c => c != null && c.IsMonster() && c.IsCanRevive())
                        .OrderByDescending(c => c.Attack)
                        .FirstOrDefault();
                    if (gyTarget != null)
                    {
                        _fanatixSSOppUsed = true;
                        AI.SelectCard(gyTarget);
                        DecisionTracer.TraceActivate("FanatixEffect", $"SS {gyTarget.Name} to opponent's field for Fanatix burn loop");
                        return true;
                    }
                }
            }
            return false;
        }

        private bool FantasixEffect()
        {
            // Search RUM Argent Chaos Force
            if (Card.Location == CardLocation.MonsterZone && Card.HasXyzMaterial() && !_fantasixSearchUsed)
            {
                if (GetRemainingCount(CardId.ArgentChaosForce) > 0 && !Bot.HasInHand(CardId.ArgentChaosForce))
                {
                    _fantasixSearchUsed = true;
                    AI.SelectCard(CardId.ArgentChaosForce);
                    DecisionTracer.TraceActivate("FantasixEffect", "Detach material to search Argent Chaos Force");
                    return true;
                }
            }

            // GY Trigger: When GP Xyz is SS'd -> Revive Fantasix & recycle RUM
            if (Card.Location == CardLocation.Grave)
            {
                DecisionTracer.TraceActivate("FantasixEffect", "Reviving Fantasix from GY and recycling RUM");
                return true;
            }

            return false;
        }

        private bool ArgentChaosForceEffect()
        {
            // Target Fantasix Machinix to Rank-Up into CXyz Fanatix Machinix
            var fantasix = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.GPFantasixMachinix);
            if (fantasix != null)
            {
                AI.SelectCard(fantasix);
                AI.SelectNextCard(CardId.GPFanatixMachinix);
                DecisionTracer.TraceActivate("ArgentChaosForce", "Ranking up Fantasix Machinix into CXyz Fanatix Machinix");
                return true;
            }

            // Fallback: Rank-Up Giant Grinder into Giant Hunter or Dark Strings
            var rank8 = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Level == 8 && c.HasType(CardType.Xyz));
            if (rank8 != null)
            {
                AI.SelectCard(rank8);
                AI.SelectNextCard(CardId.GPGiantHunter, CardId.GPDarkStrings, CardId.GPFanatixMachinix);
                return true;
            }

            // GY Trigger: When Rank 5+ Xyz is SS'd -> Add to Hand
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }

            return false;
        }

        private bool ServicePuppetEffect()
        {
            if (Card.Location == CardLocation.SpellZone && Card.IsFacedown())
            {
                int freeZones = 5 - Bot.GetMonsterCount();
                if (freeZones <= 0) return false;

                int gpXyzCount = Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && c.HasType(CardType.Xyz) && IsGimmickPuppet(c));
                int takeCount = Math.Min(gpXyzCount, freeZones);
                var oppMonsters = Enemy.MonsterZone.Where(c => c != null && c.IsFaceup()).OrderByDescending(c => c.Attack).Take(takeCount).ToList();

                if (oppMonsters.Count > 0 && takeCount > 0)
                {
                    AI.SelectCard(oppMonsters);
                    DecisionTracer.TraceActivate("ServicePuppetPlay", $"Taking control of {oppMonsters.Count} opponent monster(s)");
                    return true;
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Banish from GY to SS Xyz from either GY (requires controlling a GP Xyz)
                bool hasGPXyz = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Xyz) && IsGimmickPuppet(c));
                if (!hasGPXyz) return false;

                var xyzTarget = Bot.Graveyard.Concat(Enemy.Graveyard)
                    .FirstOrDefault(c => c != null && c.HasType(CardType.Xyz) && c.IsCanRevive());
                if (xyzTarget != null)
                {
                    AI.SelectCard(xyzTarget);
                    return true;
                }
            }
            return false;
        }

        private bool GiantGrinderEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || !Card.HasXyzMaterial()) return false;
            var target = Enemy.MonsterZone.Where(c => c != null && c.IsFaceup()).OrderByDescending(c => c.Attack).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool GiantHunterEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || !Card.HasXyzMaterial()) return false;
            var target = Enemy.MonsterZone.Where(c => c != null && c.IsFaceup()).OrderByDescending(c => c.Attack).FirstOrDefault()
                ?? Enemy.SpellZone.FirstOrDefault(c => c != null && c.IsFaceup());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool LittleKnightEffect()
        {
            var target = Util.GetProblematicEnemyCard() ?? Enemy.MonsterZone.Where(c => c != null && c.IsFaceup()).OrderByDescending(c => c.Attack).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง 5. Spells & Search
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool TerraformingEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Bot.HasInSpellZone(CardId.MansionDreadfulDolls) || Bot.HasInHand(CardId.MansionDreadfulDolls))
                return false;
            return GetRemainingCount(CardId.MansionDreadfulDolls) > 0;
        }

        private bool MansionEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (Bot.HasInSpellZone(CardId.MansionDreadfulDolls)) return false;
                if (_mansionHandUsed) return false;
                _mansionHandUsed = true;
                DecisionTracer.TraceActivate("MansionEffect", "Activate Mansion from hand to search GP starter");
                return true;
            }

            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                if (_mansionFieldUsed) return false;
                // Field Ignition: Detach 1 from Xyz -> SS GP monster from GY to opponent's field in Def (burn setup)
                // Only if Fanatix can burn it and opponent has monster zone space!
                if (!_fanatixBurnUsed && Bot.HasInMonstersZone(CardId.GPFanatixMachinix) && Enemy.GetMonsterCount() < 5)
                {
                    bool hasXyzWithMat = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Xyz) && c.Overlays.Count > 0);
                    var gpInGY = Bot.Graveyard.FirstOrDefault(c => c != null && GPMonsters.Contains(c.Id) && c.IsCanRevive());

                    if (hasXyzWithMat && gpInGY != null)
                    {
                        _mansionFieldUsed = true;
                        AI.SelectCard(gpInGY);
                        DecisionTracer.TraceActivate("MansionEffect", $"Detach to SS {gpInGY.Name} to opponent field for Fanatix trigger");
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CondolenceEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.Hand && !_condolenceUsed)
            {
                _condolenceUsed = true;
                AI.SelectCard(CardId.GPRougeDoll, CardId.GPCattleScream, CardId.GPBisqueDoll);
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                var target = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Race == (int)CardRace.Machine && c.HasType(CardType.Xyz));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง 6. Horus Engine
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool ImsetyEffect()
        {
            if (_imsetyUsed) return false;
            if (Card.Location == CardLocation.Hand)
            {
                // Discard Imsety + 1 best discard
                var discard = SelectBestDiscard(Bot.Hand, CardId.ImsetyHorus, CardId.KingsSarcophagus);
                if (discard != null && GetRemainingCount(CardId.KingsSarcophagus) > 0)
                {
                    _imsetyUsed = true;
                    AI.SelectCard(discard);
                    DecisionTracer.TraceActivate("ImsetyEffect", $"Discarding {discard.Name} to search King's Sarcophagus");
                    return true;
                }
            }
            return false;
        }

        private bool SarcophagusEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (Bot.HasInSpellZone(CardId.KingsSarcophagus)) return false;
                return true;
            }
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                if (_sarcDumpUsed) return false;
                // Discard 1 card to send Hapi from Deck to GY
                var discard = SelectBestDiscard(Bot.Hand, CardId.KingsSarcophagus);
                if (discard != null && GetRemainingCount(CardId.HapiHorus) > 0 && !Bot.HasInGraveyard(CardId.HapiHorus))
                {
                    _sarcDumpUsed = true;
                    AI.SelectCard(discard);
                    AI.SelectNextCard(CardId.HapiHorus);
                    DecisionTracer.TraceActivate("SarcophagusEffect", $"Discarding {discard.Name} to send Hapi to GY");
                    return true;
                }
            }
            return false;
        }

        private bool HorusSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.HasInSpellZone(CardId.KingsSarcophagus);
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง 7. Gimmick Puppets Summons & Hand Ignitions
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool RougeDollEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (_rougeHandUsed) return false;

            if (Card.Location == CardLocation.Hand)
            {
                // Reveal GP Xyz in Extra Deck -> SS Rouge + GP from Deck
                _rougeHandUsed = true;
                _gpExtraLocked = true;
                AI.SelectCard(CardId.GPFanatixMachinix, CardId.GPFantasixMachinix, CardId.GPGiantGrinder);
                AI.SelectNextCard(CardId.GPCattleScream, CardId.GPBisqueDoll, CardId.GPRougeDoll);
                DecisionTracer.TraceActivate("RougeDollEffect", "Reveal Xyz and SS Rouge + Lv8 GP from deck");
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Sent to GY except from hand -> Add back to hand
                return true;
            }
            return false;
        }

        private bool BisqueDollEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Card.Location == CardLocation.Hand && !_bisqueHandUsed)
            {
                var discard = SelectBestDiscard(Bot.Hand.Where(c => GPMonsters.Contains(c.Id)), CardId.GPBisqueDoll);
                if (discard != null)
                {
                    _bisqueHandUsed = true;
                    AI.SelectCard(discard);
                    DecisionTracer.TraceActivate("BisqueDollEffect", $"Discarding {discard.Name} to SS Bisque Doll (Lv8)");
                    return true;
                }
            }
            if (Card.Location == CardLocation.Grave && !_bisqueGYUsed && Duel.Player == 0)
            {
                _bisqueGYUsed = true;
                DecisionTracer.TraceActivate("BisqueDollEffect", "Banish Bisque from GY for targeting protection");
                return true;
            }
            return false;
        }

        private bool FiendishKnightEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Card.Location == CardLocation.Hand && !_fiendishHandUsed)
            {
                // Target 1 GP monster in our GY OR 1 monster in opp GY
                var target = Bot.Graveyard.Where(c => c != null && c.IsMonster() && IsGimmickPuppet(c) && c.IsCanRevive())
                    .Concat(Enemy.Graveyard.Where(c => c != null && c.IsMonster() && c.IsCanRevive()))
                    .FirstOrDefault();
                if (target != null)
                {
                    _fiendishHandUsed = true;
                    _gpExtraLocked = true;
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("FiendishKnightEffect", $"Targeting {target.Name} in GY to SS Fiendish Knight");
                    return true;
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }
            return false;
        }

        private bool CattleScreamEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (_cattleHandGYUsed) return false;

            if ((Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave))
            {
                bool hasXyzWithMat = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Xyz) && c.Overlays.Count > 0);
                if (hasXyzWithMat)
                {
                    _cattleHandGYUsed = true;
                    DecisionTracer.TraceActivate("CattleScreamEffect", "Detach material to SS Cattle Scream (Lv8)");
                    return true;
                }
            }
            return false;
        }

        private bool LittleSoldiersSummon()
        {
            return true;
        }

        private bool LittleSoldiersEffect()
        {
            if (Card.Location == CardLocation.MonsterZone && !_soldiersDumpUsed)
            {
                _soldiersDumpUsed = true;
                // Dump Rouge Doll or Cattle Scream (Lv8) to make Little Soldiers Level 8 for Rank 8 overlay!
                AI.SelectCard(CardId.GPRougeDoll, CardId.GPCattleScream, CardId.GPBisqueDoll);
                DecisionTracer.TraceActivate("LittleSoldiersEffect", "Dump Lv8 GP to make Little Soldiers Level 8");
                return true;
            }
            if (Card.Location == CardLocation.Grave && !_soldiersGYUsed)
            {
                var targets = Bot.MonsterZone.Where(c => c != null && c.IsFaceup() && GPMonsters.Contains(c.Id) && c.Level == 4).ToList();
                if (targets.Count > 0)
                {
                    _soldiersGYUsed = true;
                    AI.SelectCard(targets);
                    DecisionTracer.TraceActivate("LittleSoldiersEffect", "Banish from GY to boost Level by 4");
                    return true;
                }
            }
            return false;
        }

        private bool TerrorBabySummon()
        {
            return Bot.Graveyard.Any(c => c != null && GPMonsters.Contains(c.Id) && c.IsCanRevive());
        }

        private bool TerrorBabyEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                var target = Bot.Graveyard.Where(c => c != null && GPMonsters.Contains(c.Id) && c.IsCanRevive())
                    .OrderByDescending(c => c.Level)
                    .FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            if (Card.Location == CardLocation.Grave && Duel.Player == 0)
            {
                DecisionTracer.TraceActivate("TerrorBabyEffect", "Banish from GY for response protection");
                return true;
            }
            return false;
        }

        private bool GenericGPSummon()
        {
            return true;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง 8. Extra Deck Overlay & Link
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool FantasixSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            int lv8Count = Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && c.Level == 8);
            return lv8Count >= 2;
        }

        private bool FanatixSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            // Can overlay with 3 Lv9s or via Argent Chaos Force
            int lv9Count = Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && c.Level == 9);
            return lv9Count >= 3;
        }

        private bool ChimeraDollSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldAvoidGenericExtraDeckSummon(2)) return false;
            return Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && c.Race == (int)CardRace.Machine) >= 2;
        }

        private bool ChimeraDollEffect()
        {
            _gpExtraLocked = true;
            AI.SelectCard(CardId.GPRougeDoll, CardId.GPLittleSoldiers, CardId.GPTerrorBaby);
            return true;
        }

        private bool GiantGrinderSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && c.Level == 8) >= 2;
        }

        private bool GiantHunterSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && c.Level == 9) >= 3;
        }

        private bool LinkSummonCheck()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (_gpExtraLocked) return false; // Locked to GP / Machine Xyz
            if (ShouldAvoidGenericExtraDeckSummon(2)) return false;
            return Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && !IsAceCard(c)) >= 2;
        }

        private bool SpellSetFiltered()
        {
            return Duel.Phase == DuelPhase.Main2 || !Main.CanBattlePhase;
        }

        private bool IsGimmickPuppet(ClientCard c)
        {
            if (c == null) return false;
            return GPMonsters.Contains(c.Id)
                || c.Id == CardId.GPFanatixMachinix
                || c.Id == CardId.GPFantasixMachinix
                || c.Id == CardId.GPChimeraDoll
                || c.Id == CardId.GPGiantGrinder
                || c.Id == CardId.GPGiantHunter
                || c.Id == CardId.GPDarkStrings
                || c.Id == CardId.GPStrings
                || c.Id == CardId.GPGigantesDoll;
        }
    }

    [Deck("Expert_2026_Puppet", "2026_Puppet")]
    public class ExpertPuppetExecutor : _2026_PuppetExecutor
    {
        private string _duelId;
        public ExpertPuppetExecutor(GameAI ai, Duel duel) : base(ai, duel)
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
