// ============================================================================
// CARD AUDIT — Watenpai (Watt + Tenpai Dragon + Kaiju OTK Synergy)
// ============================================================================
// | Card Name                          | Level/Type     | Role                                        | Activation Trigger / Strategy                           |
// |------------------------------------|----------------|---------------------------------------------|---------------------------------------------------------|
// | Wattcobra                          | L4 Thunder     | Direct Attacker (1000 ATK) + Searcher       | Direct attack -> Search Wattuna to hand                 |
// | Wattuna                            | L4 Thunder T   | Direct Attacker (800 ATK) + Synchro Enabler | Battle damage -> SS self; direct damage -> SS Wattkyuki |
// | Wattdragonfly                      | L2 Thunder     | Floater Wall (900 ATK)                      | Destroyed by opp -> SS Watt from Deck                   |
// | Brohunder                          | L4 Thunder     | Normal Summon Searcher                      | NS -> Search Wattcobra or Wattuna                       |
// | Dora Dora                          | L1 Fire Dragon | Normal Summon Searcher                      | NS -> Search Paidra or Chundra                          |
// | Thunder King Kaiju                 | L9 Thunder     | 3300 ATK Behemoth / Board Remover           | Kaiju Slumber target (given to bot!)                    |
// | Kumongous / Radian Kaiju           | L7 Kaiju       | 2400 / 2800 ATK Fodder                      | Given to opponent via Kaiju Slumber                     |
// | Interrupted Kaiju Slumber          | Normal Spell   | Board Wipe + 2x Kaiju SS                    | Clears enemy monsters -> Bot gets 3300 Thunder King     |
// | Tenpai Dragon Paidra               | L3 Fire Dragon | Searcher / Synchro Material                 | NS/SS -> Search Sangen Summoning/Kaimen                 |
// | Tenpai Dragon Chundra              | L4 Fire Tuner  | Tuner / Battle Swarmer                      | SS from hand; on battle SS Fadra; BP Quick Synchro      |
// | Tenpai Dragon Fadra                | L3 Fire Dragon | Extender / Battle Reviver                   | Revive Paidra/Chundra; BP Quick Synchro                 |
// | Sangen Summoning                   | Field Spell    | Blanket MP1 Immunity + Search + BP 2x ATK   | Trident pops this -> Trident becomes 6000 ATK x 3!      |
// | Wattkyuki                          | L8 Thunder S   | Direct Attacker (1600 ATK) + Synchro Swarm  | Direct damage -> Shuffle GY/field -> SS Watthydra       |
// | Watthydra                          | L7 Thunder S   | Direct Attacker (1500 ATK) + Deck Banish    | Direct damage -> Banish 1 card from deck                |
// | Trident Dragion                    | L10 Dragon S   | OTK Finisher (6000 ATK x 3 = 18,000!)       | BP: Pops Summoning -> 18,000 Damage OTK                 |
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.DecisionEngine;
using WindBot.Game.AI.Plugin;
using WindBot.Game.AI.Plugins;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("Watenpai", "Watenpai")]
    public class WatenpaiExecutor : ModernExecutor
    {
        public class CardId
        {
            // Watt & Thunder Engine
            public const int Wattcobra = 88205593;
            public const int Wattuna = 10113611;
            public const int Wattdragonfly = 97885363;
            public const int Brohunder = 27217742;

            // Kaiju Engine
            public const int ThunderKingKaiju = 48770333;
            public const int RadianKaiju = 28674152;
            public const int KumongousKaiju = 29726552;
            public const int InterruptedKaijuSlumber = 99330325;

            // Dragon & Tenpai Engine
            public const int DoraDora = 11590299;
            public const int Blaster = 53804307;
            public const int TenpaiDragonPaidra = 39931513;
            public const int TenpaiDragonChundra = 91810826;
            public const int TenpaiDragonFadra = 65326118;
            public const int TenpaiDragonGenroku = 23657016;
            public const int SangenSummoning = 30336082;
            public const int SangenKaimen = 66730191;

            // Spells & Board Breakers
            public const int Raigeki = 12580477;
            public const int DarkHole = 53129443;
            public const int HeyTrunade = 64697431;
            public const int TwinTwisters = 43898403;
            public const int ForbiddenChalice = 25789292;
            public const int CalledByTheGrave = 24224830;

            // Extra Deck
            public const int Wattkyuki = 67752972;
            public const int Watthydra = 29765339;
            public const int SangenpaiBidentDragion = 82570174;
            public const int SangenpaiTranscendentDragion = 18969888;
            public const int TridentDragion = 39402797;
            public const int BlackRoseDragon = 73580471;
            public const int KuibeltTheBladeDragon = 87837090;
            public const int HieraticSeal = 24361622;
            public const int SuperStarslayerTYPHON = 93039339;
            public const int Garura = 11765832;
            public const int Mudragon = 54757758;
            public const int SPLittleKnight = 38342335;
        }

        public WatenpaiPlugin Plugin { get; }

        private bool _summoningSearchUsed = false;
        private bool _paidraSearchUsed = false;
        private bool _chundraDeckSpUsed = false;
        private bool _fadraGyReviveUsed = false;
        private bool _wattunaSpFromHandUsed = false;
        private bool _wattunaSynchroUsed = false;
        private bool _wattkyukiSynchroUsed = false;

        public WatenpaiExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            Plugin = new WatenpaiPlugin(this);
            DeckPlugin = Plugin;

            BaitPlanner.RegisterComboStarters(CardId.Wattcobra, CardId.TenpaiDragonPaidra, CardId.InterruptedKaijuSlumber);
            BaitPlanner.RegisterBaitCards(CardId.Raigeki, CardId.DarkHole, CardId.HeyTrunade, CardId.TwinTwisters);

            RegisterComboLines();
            RegisterExecutors();
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _summoningSearchUsed = false;
            _paidraSearchUsed = false;
            _chundraDeckSpUsed = false;
            _fadraGyReviveUsed = false;
            _wattunaSpFromHandUsed = false;
            _wattunaSynchroUsed = false;
            _wattkyukiSynchroUsed = false;
            Plugin.ResetTurnState();
        }

        private bool IsGoingFirstTurn()
        {
            return Duel.Turn == 1 && Duel.Player == 0;
        }

        private void RegisterComboLines()
        {
            // ── Line 1: Watt Direct Attack Burst Line ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Watenpai-Watt-Direct-Burst",
                RequiredCards = new List<int> { CardId.Wattcobra },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.Wattcobra, ActionType = ExecutorType.Summon, Description = "Normal Summon Wattcobra" },
                    new() { CardId = CardId.Wattcobra, ActionType = ExecutorType.Activate, Description = "Direct Attack -> Search Wattuna" },
                    new() { CardId = CardId.Wattuna, ActionType = ExecutorType.Activate, Description = "Special Summon Wattuna from Hand on Battle Damage" }
                }
            });

            // ── Line 2: Kaiju Slumber Board Wipe Line ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Watenpai-Kaiju-Slumber-Breaker",
                RequiredCards = new List<int> { CardId.InterruptedKaijuSlumber },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.InterruptedKaijuSlumber, ActionType = ExecutorType.Activate, Description = "Destroy all monsters, summon Kaijus" }
                }
            });
        }

        private void RegisterExecutors()
        {
            // ═══════════════════════════════════════════════════════════════
            //  TIER 0: QUICK DISRUPTIONS & COUNTERS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveActivate);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenChalice, ForbiddenChaliceActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 1: BOARD BREAKERS (MAIN PHASE 1)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.HeyTrunade, HeyTrunadeActivate);
            AddExecutor(ExecutorType.Activate, CardId.TwinTwisters, TwinTwistersActivate);
            AddExecutor(ExecutorType.Activate, CardId.Raigeki, RaigekiActivate);
            AddExecutor(ExecutorType.Activate, CardId.DarkHole, DarkHoleActivate);
            AddExecutor(ExecutorType.Activate, CardId.InterruptedKaijuSlumber, SlumberActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: FIELD SPELLS & SEARCHERS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.SangenSummoning, SangenSummoningActivate);
            AddExecutor(ExecutorType.Activate, CardId.SangenKaimen, SangenKaimenActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: WATT & TENPAI MONSTERS
            // ═══════════════════════════════════════════════════════════════
            // Wattuna in hand: Trigger when battle damage inflicted to opponent
            AddExecutor(ExecutorType.Activate, CardId.Wattuna, WattunaEffect);

            // Normal Summons: Priority to Wattcobra (direct attack search) > Brohunder > Dora Dora > Paidra
            AddExecutor(ExecutorType.Summon, CardId.Wattcobra, WattcobraSummon);
            AddExecutor(ExecutorType.Activate, CardId.Wattcobra, WattcobraEffect);

            AddExecutor(ExecutorType.Summon, CardId.Brohunder, BrohunderSummon);
            AddExecutor(ExecutorType.Activate, CardId.Brohunder, BrohunderEffect);

            AddExecutor(ExecutorType.Summon, CardId.DoraDora, DoraDoraSummon);
            AddExecutor(ExecutorType.Activate, CardId.DoraDora, DoraDoraEffect);

            AddExecutor(ExecutorType.Summon, CardId.TenpaiDragonPaidra, PaidraSummon);
            AddExecutor(ExecutorType.Activate, CardId.TenpaiDragonPaidra, PaidraEffect);

            AddExecutor(ExecutorType.Activate, CardId.TenpaiDragonChundra, ChundraEffect);
            AddExecutor(ExecutorType.Summon, CardId.TenpaiDragonChundra, ChundraSummon);

            AddExecutor(ExecutorType.Summon, CardId.TenpaiDragonFadra, FadraSummon);
            AddExecutor(ExecutorType.Activate, CardId.TenpaiDragonFadra, FadraEffect);

            AddExecutor(ExecutorType.Activate, CardId.TenpaiDragonGenroku, GenrokuEffect);

            AddExecutor(ExecutorType.Summon, CardId.Wattdragonfly, DragonflySummon);
            AddExecutor(ExecutorType.Activate, CardId.Wattdragonfly, DragonflyEffect);

            AddExecutor(ExecutorType.Summon, FallbackSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: EXTRA DECK (WATT & TENPAI DUAL LADDER)
            // ═══════════════════════════════════════════════════════════════
            // Wattkyuki & Watthydra effects upon direct damage
            AddExecutor(ExecutorType.Activate, CardId.Wattkyuki, WattkyukiEffect);
            AddExecutor(ExecutorType.Activate, CardId.Watthydra, WatthydraEffect);

            // Extra Deck Synchro Summons
            AddExecutor(ExecutorType.SpSummon, CardId.HieraticSeal, HieraticSealSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.HieraticSeal, HieraticSealEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.BlackRoseDragon, BlackRoseSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.BlackRoseDragon, BlackRoseEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.SangenpaiBidentDragion, BidentSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SangenpaiBidentDragion, BidentEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.TridentDragion, TridentSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TridentDragion, TridentEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.SangenpaiTranscendentDragion, TranscendentSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SangenpaiTranscendentDragion, TranscendentEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.KuibeltTheBladeDragon, KuibeltSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.KuibeltTheBladeDragon, KuibeltEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSpSummon);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 5: SETS & REPOSITIONING
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenChalice, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.TwinTwisters, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.SangenKaimen, SpellSetStrategy);

            AddExecutor(ExecutorType.Repos, RepositionStrategy);
        }

        // ═══════════════════════════════════════════════════════════════
        //  DISRUPTIONS & BOARD BREAKERS
        // ═══════════════════════════════════════════════════════════════

        private bool CalledByTheGraveActivate()
        {
            ClientCard lastCard = LastChainCard;
            if (lastCard != null && lastCard.Controller == 1)
            {
                ClientCard target = Enemy.Graveyard.FirstOrDefault(c => c.IsMonster() && c.Name == lastCard.Name);
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool ForbiddenChaliceActivate()
        {
            ClientCard target = Enemy.GetMonsters()
                .Where(m => m != null && m.IsFaceup() && !m.IsDisabled() && !m.IsShouldNotBeTarget())
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool HeyTrunadeActivate()
        {
            return Enemy.GetSpellCount() >= 1;
        }

        private bool TwinTwistersActivate()
        {
            if (Enemy.GetSpellCount() == 0) return false;
            var fodder = Plugin.MaterialImpl.PickDiscardTarget(Bot.Hand.Where(c => c != Card).ToList());
            if (fodder != null)
            {
                var oppSpells = Enemy.GetSpells().Where(s => !s.IsShouldNotBeTarget()).Take(2).ToList();
                if (oppSpells.Count > 0)
                {
                    AI.SelectCard(fodder);
                    AI.SelectNextCard(oppSpells);
                    return true;
                }
            }
            return false;
        }

        private bool RaigekiActivate()
        {
            return Enemy.GetMonsterCount() >= 1;
        }

        private bool DarkHoleActivate()
        {
            if (Bot.GetMonsterCount() == 0 && Enemy.GetMonsterCount() >= 1) return true;
            return Enemy.GetMonsterCount() >= 2 && Enemy.GetMonsterCount() > Bot.GetMonsterCount();
        }

        private bool SlumberActivate()
        {
            return Enemy.GetMonsterCount() >= 1;
        }

        // ═══════════════════════════════════════════════════════════════
        //  FIELD SPELLS & SEARCHERS
        // ═══════════════════════════════════════════════════════════════

        private bool SangenSummoningActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return !Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.SangenSummoning);
            }
            else if (Card.Location == CardLocation.SpellZone)
            {
                if (_summoningSearchUsed) return false;
                _summoningSearchUsed = true;
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // Double ATK of Dragon Synchro (Trident Dragion -> 6000 ATK!)
                ClientCard target = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.Id == CardId.TridentDragion)
                                 ?? Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.HasType(CardType.Synchro) && m.HasRace(CardRace.Dragon));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return true;
        }

        private bool SangenKaimenActivate()
        {
            if (Duel.Phase == DuelPhase.Battle) return true;
            AI.SelectOption(0);
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  WATT & TENPAI MONSTERS
        // ═══════════════════════════════════════════════════════════════

        private bool WattunaEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Trigger when damage inflicted to opponent: Special Summon Wattuna from hand!
                if (!_wattunaSpFromHandUsed)
                {
                    _wattunaSpFromHandUsed = true;
                    return true;
                }
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                // Trigger when Wattuna inflicts direct battle damage:
                // Release self + 1 non-tuner to Special Summon Wattkyuki (Level 8) or Watthydra (Level 7) from Extra Deck!
                if (!_wattunaSynchroUsed)
                {
                    _wattunaSynchroUsed = true;
                    ClientCard nonTuner = Bot.GetMonsters().FirstOrDefault(m => m != Card && m.IsFaceup() && !m.HasType(CardType.Tuner))
                                       ?? Bot.Hand.FirstOrDefault(m => m.IsMonster() && !m.HasType(CardType.Tuner));
                    if (nonTuner != null)
                    {
                        AI.SelectCard(Card);
                        AI.SelectNextCard(nonTuner);
                        return true;
                    }
                }
            }
            return true;
        }

        private bool WattcobraSummon()
        {
            return true;
        }

        private bool WattcobraEffect()
        {
            // Direct battle damage search: Add Wattuna > Wattcobra
            AI.SelectCard(CardId.Wattuna, CardId.Wattcobra, CardId.Wattdragonfly);
            return true;
        }

        private bool BrohunderSummon()
        {
            return true;
        }

        private bool BrohunderEffect()
        {
            AI.SelectCard(CardId.Wattcobra, CardId.Wattuna, CardId.ThunderKingKaiju);
            return true;
        }

        private bool DoraDoraSummon()
        {
            return true;
        }

        private bool DoraDoraEffect()
        {
            AI.SelectCard(CardId.TenpaiDragonPaidra, CardId.TenpaiDragonChundra, CardId.TenpaiDragonFadra);
            return true;
        }

        private bool PaidraSummon()
        {
            return true;
        }

        private bool PaidraEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (Duel.Phase == DuelPhase.Battle) return true; // Quick Synchro
                if (_paidraSearchUsed) return false;
                _paidraSearchUsed = true;
                return true;
            }
            return true;
        }

        private bool ChundraSummon()
        {
            return true;
        }

        private bool ChundraEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasAttribute(CardAttribute.Fire) && m.HasRace(CardRace.Dragon));
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                if (Duel.Phase == DuelPhase.Battle)
                {
                    if (!_chundraDeckSpUsed)
                    {
                        _chundraDeckSpUsed = true;
                        AI.SelectCard(CardId.TenpaiDragonFadra, CardId.TenpaiDragonPaidra);
                        return true;
                    }
                    return true;
                }
            }
            return true;
        }

        private bool FadraSummon()
        {
            return true;
        }

        private bool FadraEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (!_fadraGyReviveUsed)
                {
                    _fadraGyReviveUsed = true;
                    ClientCard revTarget = Bot.Graveyard.Where(c => c.IsMonster() && c.HasAttribute(CardAttribute.Fire) && c.HasRace(CardRace.Dragon))
                        .OrderByDescending(c => c.Attack)
                        .FirstOrDefault();
                    if (revTarget != null)
                    {
                        AI.SelectCard(revTarget);
                        return true;
                    }
                }
                return true;
            }
            return true;
        }

        private bool GenrokuEffect()
        {
            if (Card.Location == CardLocation.Hand) return true;
            if (Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(CardId.TenpaiDragonChundra, CardId.TenpaiDragonPaidra);
                return true;
            }
            return true;
        }

        private bool DragonflySummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool DragonflyEffect()
        {
            AI.SelectCard(CardId.Wattcobra, CardId.Wattuna);
            return true;
        }

        private bool FallbackSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        // ═══════════════════════════════════════════════════════════════
        //  EXTRA DECK SYNCHROS
        // ═══════════════════════════════════════════════════════════════

        private bool WattkyukiEffect()
        {
            // On direct damage: Shuffle Wattuna from GY + 1 Thunder monster from field -> SS Watthydra from Extra Deck!
            if (!_wattkyukiSynchroUsed)
            {
                _wattkyukiSynchroUsed = true;
                ClientCard gyTuner = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.Wattuna);
                ClientCard fieldThunder = Bot.GetMonsters().FirstOrDefault(m => m != Card && m.IsFaceup() && m.HasRace(CardRace.Thunder));
                if (gyTuner != null && fieldThunder != null)
                {
                    AI.SelectCard(gyTuner);
                    AI.SelectNextCard(fieldThunder);
                    AI.SelectThirdCard(CardId.Watthydra);
                    return true;
                }
            }
            return true;
        }

        private bool WatthydraEffect()
        {
            // On direct damage: Banish 1 card from deck
            return true;
        }

        private bool HieraticSealSpSummon()
        {
            return IsGoingFirstTurn() && Bot.GetMonsters().Count(m => m.IsFaceup() && m.HasRace(CardRace.Dragon)) >= 2;
        }

        private bool HieraticSealEffect()
        {
            if (Duel.Player == 1)
            {
                ClientCard oppTarget = GetBestRemovalTarget(onlyFaceup: true, canBeTarget: true);
                if (oppTarget != null)
                {
                    AI.SelectCard(Card);
                    AI.SelectNextCard(oppTarget);
                    return true;
                }
            }
            AI.SelectCard(CardId.TenpaiDragonPaidra, CardId.TenpaiDragonGenroku);
            return true;
        }

        private bool BlackRoseSpSummon()
        {
            return !IsGoingFirstTurn() && Duel.Phase == DuelPhase.Main1 && Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 3;
        }

        private bool BlackRoseEffect()
        {
            return true;
        }

        private bool BidentSpSummon()
        {
            return true;
        }

        private bool BidentEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                ClientCard paidra = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.TenpaiDragonPaidra)
                                 ?? Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.TenpaiDragonFadra);
                if (paidra != null)
                {
                    AI.SelectCard(paidra);
                    return true;
                }
            }
            return true;
        }

        private bool TridentSpSummon()
        {
            if (IsGoingFirstTurn()) return false;
            return Duel.Phase == DuelPhase.Battle || Duel.Phase == DuelPhase.Main1;
        }

        private bool TridentEffect()
        {
            var popTargets = new List<ClientCard>();
            ClientCard fieldSpell = Bot.GetSpells().FirstOrDefault(s => s != null && s.IsFaceup() && s.Id == CardId.SangenSummoning);
            if (fieldSpell != null) popTargets.Add(fieldSpell);

            ClientCard secondTarget = Bot.GetMonsters().FirstOrDefault(m => m != Card && m.Id != CardId.SangenpaiBidentDragion);
            if (secondTarget != null) popTargets.Add(secondTarget);

            if (popTargets.Count > 0)
            {
                AI.SelectCard(popTargets);
                return true;
            }
            return true;
        }

        private bool TranscendentSpSummon()
        {
            return true;
        }

        private bool TranscendentEffect()
        {
            return true;
        }

        private bool KuibeltSpSummon()
        {
            return Enemy.GetMonsterCount() + Enemy.GetSpellCount() > 0;
        }

        private bool KuibeltEffect()
        {
            ClientCard oppTarget = GetBestRemovalTarget(onlyFaceup: false, canBeTarget: true);
            if (oppTarget != null)
            {
                AI.SelectCard(oppTarget);
                return true;
            }
            return false;
        }

        private bool SPLittleKnightSpSummon()
        {
            return Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 1 && Bot.GetMonsters().Count(m => m.IsFaceup()) >= 2;
        }

        // ═══════════════════════════════════════════════════════════════
        //  HEURISTICS & SELECTION
        // ═══════════════════════════════════════════════════════════════

        private bool SpellSetStrategy()
        {
            if (Card.IsTrap()) return true;
            if (Card.IsSpell() && Card.HasType(CardType.QuickPlay))
            {
                return Duel.Phase == DuelPhase.Main2 || IsGoingFirstTurn();
            }
            return false;
        }

        private bool RepositionStrategy()
        {
            if (Card == null) return false;
            if (Card.IsAttack() && Card.Attack == 0) return true;
            if (Card.IsDefense() && Card.Attack >= 1000 && Duel.Phase == DuelPhase.Main1 && Duel.Player == 0) return true;
            return false;
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.Wattdragonfly && positions.Contains(CardPosition.FaceUpDefence))
            {
                return CardPosition.FaceUpDefence;
            }
            if (positions.Contains(CardPosition.FaceUpAttack))
            {
                return CardPosition.FaceUpAttack;
            }
            return base.OnSelectPosition(cardId, positions);
        }

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0) return base.OnSelectOption(options);
            if (Card != null && Card.Id == CardId.SangenKaimen) return 0;
            return base.OnSelectOption(options);
        }

        public override bool OnSelectYesNo(long desc)
        {
            // Sangen Summoning destroyed in BP: double ATK
            if (desc == Util.GetStringId(CardId.SangenSummoning, 1)) return true;
            if (desc == Util.GetStringId(CardId.TridentDragion, 0)) return true;
            if (desc == Util.GetStringId(CardId.Wattuna, 0)) return true;
            if (desc == Util.GetStringId(CardId.Wattuna, 1)) return true;
            if (desc == Util.GetStringId(CardId.Wattkyuki, 0)) return true;
            if (desc == Util.GetStringId(CardId.BlackRoseDragon, 0)) return Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 2;
            return base.OnSelectYesNo(desc);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards != null && cards.Count > 0)
            {
                // Deck search
                if (hint == 506 || cards.All(c => c.Location == CardLocation.Deck))
                {
                    var searchTarget = Plugin.StrategyImpl.PickSearchTarget(cards, Card);
                    if (searchTarget != null) return new List<ClientCard> { searchTarget };
                }

                // Discard
                if (hint == 501 || (cards.All(c => c.Location == CardLocation.Hand) && !cancelable))
                {
                    var discardTarget = Plugin.MaterialImpl.PickDiscardTarget(cards, min);
                    if (discardTarget != null) return new List<ClientCard> { discardTarget };
                }

                // Destruction (Trident vs Enemy)
                if (hint == 502 || hint == 503 || hint == 504)
                {
                    if (Card != null && Card.Id == CardId.TridentDragion && cards.Any(c => c.Controller == 0))
                    {
                        var field = cards.FirstOrDefault(c => c.Id == CardId.SangenSummoning);
                        if (field != null) return new List<ClientCard> { field };
                    }
                    var enemyTargets = cards.Where(c => c.Controller == 1).OrderByDescending(c => Scorer.ThreatScore(c)).ToList();
                    if (enemyTargets.Count >= min)
                    {
                        return enemyTargets.Take(Math.Min(max, enemyTargets.Count)).ToList();
                    }
                }

                // Special Summon
                if (hint == 509)
                {
                    var spTarget = Plugin.StrategyImpl.PickSpecialSummonTarget(cards);
                    if (spTarget != null) return new List<ClientCard> { spTarget };
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
