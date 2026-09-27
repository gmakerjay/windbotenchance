using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Decks;
using WindBot.Game.AI.Plugin;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Plugins
{
    // ═══════════════════════════════════════════════════════════════
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: DinomorphiaPlugin
    //  Precision Life Point Management & Trap Disruption Strategy
    // ═══════════════════════════════════════════════════════════════
    public class DinomorphiaPlugin : DeckPluginBase
    {
        private readonly DinomorphiaExecutor _exec;

        public override string DeckName => "Dinomorphia";

        public DinomorphiaStrategy StrategyImpl { get; }
        public DinomorphiaMaterialEvaluator MaterialImpl { get; }
        public DinomorphiaThreatEvaluator ThreatImpl { get; }
        public DinomorphiaLifePointManager LifePointManager { get; }
        public DinomorphiaTrapAdvisor TrapAdvisor { get; }
        public DinomorphiaBoardAssessor BoardAssessor { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public DinomorphiaPlugin(DinomorphiaExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new DinomorphiaStrategy(exec);
            MaterialImpl = new DinomorphiaMaterialEvaluator(exec);
            ThreatImpl = new DinomorphiaThreatEvaluator(exec);
            LifePointManager = new DinomorphiaLifePointManager(exec);
            TrapAdvisor = new DinomorphiaTrapAdvisor(exec);
            BoardAssessor = new DinomorphiaBoardAssessor(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class DinomorphiaStrategy : IDeckStrategy
    {
        private readonly DinomorphiaExecutor _exec;

        public DinomorphiaStrategy(DinomorphiaExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Fossil Dig / Search Priority:
            // 1. Therizia (Main normal summon / Trap setter)
            var therizia = candidates.FirstOrDefault(c => c.Id == DinomorphiaExecutor.CardId.DinomorphiaTherizia);
            if (therizia != null && !_exec.Bot.HasInHand(DinomorphiaExecutor.CardId.DinomorphiaTherizia) &&
                !_exec.Bot.HasInMonstersZone(DinomorphiaExecutor.CardId.DinomorphiaTherizia))
                return therizia;

            // 2. Miscellaneousaurus (Protection / Extender)
            var misc = candidates.FirstOrDefault(c => c.Id == DinomorphiaExecutor.CardId.Miscellaneousaurus);
            if (misc != null && !_exec.Bot.HasInHand(DinomorphiaExecutor.CardId.Miscellaneousaurus))
                return misc;

            // 3. Diplos (GY preparation)
            var diplos = candidates.FirstOrDefault(c => c.Id == DinomorphiaExecutor.CardId.DinomorphiaDiplos);
            if (diplos != null) return diplos;

            return candidates[0];
        }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Revival Priority from Graveyard (Alert / Rexterm float / Kentregina float)
            var rexterm = candidates.FirstOrDefault(c => c.Id == DinomorphiaExecutor.CardId.DinomorphiaRexterm);
            if (rexterm != null) return rexterm;

            var kent = candidates.FirstOrDefault(c => c.Id == DinomorphiaExecutor.CardId.DinomorphiaKentregina);
            if (kent != null) return kent;

            var stealth = candidates.FirstOrDefault(c => c.Id == DinomorphiaExecutor.CardId.DinomorphiaStealthbergia);
            if (stealth != null) return stealth;

            var therizia = candidates.FirstOrDefault(c => c.Id == DinomorphiaExecutor.CardId.DinomorphiaTherizia);
            if (therizia != null) return therizia;

            return candidates.FirstOrDefault(c => c != null);
        }

        public ClientCard PickTrapToSetFromDeck(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Therizia Set Priority:
            // 1. Frenzy (Turn 1 fusion from Extra + Main Deck)
            var frenzy = candidates.FirstOrDefault(c => c.Id == DinomorphiaExecutor.CardId.DinomorphiaFrenzy);
            if (frenzy != null && !_exec.Bot.HasInSpellZone(DinomorphiaExecutor.CardId.DinomorphiaFrenzy))
                return frenzy;

            // 2. Domain (Backup fusion)
            var domain = candidates.FirstOrDefault(c => c.Id == DinomorphiaExecutor.CardId.DinomorphiaDomain);
            if (domain != null && !_exec.Bot.HasInSpellZone(DinomorphiaExecutor.CardId.DinomorphiaDomain))
                return domain;

            // 3. Intact (Monster Negate + Halve Battle Damage)
            var intact = candidates.FirstOrDefault(c => c.Id == DinomorphiaExecutor.CardId.DinomorphiaIntact);
            if (intact != null && !_exec.Bot.HasInSpellZone(DinomorphiaExecutor.CardId.DinomorphiaIntact))
                return intact;

            // 4. Brute (Removal)
            var brute = candidates.FirstOrDefault(c => c.Id == DinomorphiaExecutor.CardId.DinomorphiaBrute);
            if (brute != null) return brute;

            return candidates[0];
        }

        public ClientCard PickTrapToDumpFromDeck(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Diplos Dump Priority:
            // 1. If Kentregina is on field or will be summoned, dump Frenzy/Domain to copy
            if (_exec.Bot.HasInMonstersZone(DinomorphiaExecutor.CardId.DinomorphiaKentregina))
            {
                var frenzy = candidates.FirstOrDefault(c => c.Id == DinomorphiaExecutor.CardId.DinomorphiaFrenzy);
                if (frenzy != null) return frenzy;
                var domain = candidates.FirstOrDefault(c => c.Id == DinomorphiaExecutor.CardId.DinomorphiaDomain);
                if (domain != null) return domain;
            }

            // 2. Intact or Sonic for Battle Damage protection in GY
            var intact = candidates.FirstOrDefault(c => c.Id == DinomorphiaExecutor.CardId.DinomorphiaIntact);
            if (intact != null && !_exec.Bot.Graveyard.Any(c => c.Id == DinomorphiaExecutor.CardId.DinomorphiaIntact))
                return intact;

            // 3. Frenzy for Effect Damage protection in GY
            var frenzyProt = candidates.FirstOrDefault(c => c.Id == DinomorphiaExecutor.CardId.DinomorphiaFrenzy);
            if (frenzyProt != null && !_exec.Bot.Graveyard.Any(c => c.Id == DinomorphiaExecutor.CardId.DinomorphiaFrenzy))
                return frenzyProt;

            return candidates[0];
        }

        public bool PrioritizeSpSummonFirst => false; // Normal summon Therizia / Set traps first
        public bool CanExecuteTurn1Combo => _exec.Bot.HasInHand(DinomorphiaExecutor.CardId.DinomorphiaTherizia) ||
                                            _exec.Bot.HasInHand(DinomorphiaExecutor.CardId.DinomorphiaFrenzy) ||
                                            _exec.Bot.HasInHand(DinomorphiaExecutor.CardId.FossilDig);
        public bool CanExecuteTurn2Combo => true;
        public int AssessBoardState() => _exec.Bot.GetMonsters().Count(m => m.IsFaceup());
    }

    public class DinomorphiaMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly DinomorphiaExecutor _exec;

        public DinomorphiaMaterialEvaluator(DinomorphiaExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 999;
            // Rexterm is the game-winning floodgate; never sacrifice unless absolute emergency
            if (card.Id == DinomorphiaExecutor.CardId.DinomorphiaRexterm) return 50000;
            if (card.Id == DinomorphiaExecutor.CardId.DinomorphiaKentregina) return 20000;
            if (card.Id == DinomorphiaExecutor.CardId.DinomorphiaStealthbergia) return 15000;
            if (card.Id == DinomorphiaExecutor.CardId.DinomorphiaTherizia) return 3000;
            if (card.Id == DinomorphiaExecutor.CardId.DinomorphiaDiplos) return 2000;
            return 100;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.Where(c => c != null).OrderBy(c => GetMaterialCost(c)).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            // When Sonic or Brute asks to destroy our own card, pick floater/token, never sole Rexterm
            var safeDino = candidates.FirstOrDefault(c => c != null && c.Id != DinomorphiaExecutor.CardId.DinomorphiaRexterm);
            if (safeDino != null) return safeDino;

            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }
    }

    public class DinomorphiaThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly DinomorphiaExecutor _exec;

        public DinomorphiaThreatEvaluator(DinomorphiaExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null || card.Controller == 0) return 0;

            int id = card.Id;
            // Extreme Threat: Mass Backrow Removals (Solemn Judgment must intercept)
            if (id == 18144506 || id == 43898403 || id == 57728570 || id == 23002292 || id == 12580477)
                return 15000;

            // Direct Burn cards (fatal at low LP)
            if (card.IsSpell() || card.IsTrap())
                return 7000;

            if (card.IsMonster())
            {
                // If Rexterm is active and opponent ATK >= LP, effect is already locked
                if (_exec.Plugin.BoardAssessor.HasActiveRextermLock() && card.Attack >= _exec.Bot.LifePoints)
                    return 2000; // Passive threat only in battle

                return card.Attack >= 3000 ? 8000 : 4000;
            }

            return 3000;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null || card.Controller == 0) return false;
            int id = card.Id;
            return id == 18144506 || id == 43898403 || id == 57728570 || id == 23002292 || card.Attack >= 3000;
        }
    }

    public class DinomorphiaLifePointManager
    {
        private readonly DinomorphiaExecutor _exec;

        public DinomorphiaLifePointManager(DinomorphiaExecutor exec) => _exec = exec;

        public bool IsCriticalLp => _exec.Bot.LifePoints <= 1000;
        public bool IsSuperCriticalLp => _exec.Bot.LifePoints <= 500;

        /// <summary>
        /// Under Stealthbergia (LP <= 2000), Dinomorphia traps and monster effects cost 0 LP to activate!
        /// </summary>
        public bool IsStealthbergiaActive()
        {
            return _exec.Bot.LifePoints <= 2000 &&
                   _exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && !m.IsDisabled() &&
                                                    m.Id == DinomorphiaExecutor.CardId.DinomorphiaStealthbergia);
        }

        /// <summary>
        /// Checks whether the Graveyard holds a Dinomorphia trap that negates battle damage to 0 when LP <= 2000.
        /// (Intact, Sonic, Shell)
        /// </summary>
        public bool HasBattleDamageProtectionInGrave()
        {
            return _exec.Bot.Graveyard.Any(c => c.Id == DinomorphiaExecutor.CardId.DinomorphiaIntact ||
                                                c.Id == DinomorphiaExecutor.CardId.DinomorphiaSonic ||
                                                c.Id == DinomorphiaExecutor.CardId.DinomorphiaShell);
        }

        /// <summary>
        /// Checks whether the Graveyard holds a Dinomorphia trap that negates opponent effect damage (burn) to 0 when LP <= 2000.
        /// (Frenzy, Domain, Brute, Alert)
        /// </summary>
        public bool HasEffectDamageProtectionInGrave()
        {
            return _exec.Bot.Graveyard.Any(c => c.Id == DinomorphiaExecutor.CardId.DinomorphiaFrenzy ||
                                                c.Id == DinomorphiaExecutor.CardId.DinomorphiaDomain ||
                                                c.Id == DinomorphiaExecutor.CardId.DinomorphiaBrute ||
                                                c.Id == DinomorphiaExecutor.CardId.DinomorphiaAlert);
        }

        /// <summary>
        /// Evaluates whether halving LP is strategically sound and safe.
        /// Considers:
        /// 1. Stealthbergia: free if active.
        /// 2. Solemn Strike preservation: If Strike is set, do not drop below 1500 LP without critical necessity.
        /// 3. Graveyard safety net: If dropping below 1000 LP, verify protection exists or emergency is occurring.
        /// </summary>
        public bool CanSafelyHalveLp(bool isEmergency = false)
        {
            if (IsStealthbergiaActive()) return true;

            int currentLp = _exec.Bot.LifePoints;
            int nextLp = (currentLp + 1) / 2;

            if (nextLp <= 0) return false;

            // If Solemn Strike is set on backrow, preserve at least 1500 LP unless this is an emergency
            bool isStrikeSet = _exec.Bot.GetSpells().Any(s => s != null && !s.IsFaceup() &&
                                                              s.Id == DinomorphiaExecutor.CardId.SolemnStrike);
            if (isStrikeSet && currentLp > 1500 && nextLp < 1500 && !isEmergency)
            {
                return false;
            }

            // If dropping into super-critical LP (<= 500), ensure either emergency or grave protection
            if (nextLp <= 500 && !isEmergency)
            {
                if (!HasBattleDamageProtectionInGrave() && _exec.Enemy.GetMonsterCount() > 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Evaluates whether Rexterm's Quick Effect ATK reduction should be activated.
        /// Should NOT be spammed blindly.
        /// Activates ONLY when:
        /// 1. An opponent monster is attacking and its ATK threatens our monster/LP.
        /// 2. Opponent controls unlocked monsters (ATK < LP) attempting to activate dangerous effects.
        /// 3. High-ATK threat on board that could run over Rexterm.
        /// </summary>
        public bool ShouldActivateRextermReduction(IList<ClientCard> oppMonsters)
        {
            if (oppMonsters == null || oppMonsters.Count == 0) return false;
            if (_exec.Bot.LifePoints <= 1) return false;

            // In Battle Phase:
            if (_exec.Duel.Phase == DuelPhase.BattleStart || _exec.Duel.Phase == DuelPhase.BattleStep || _exec.Duel.Phase == DuelPhase.Damage)
            {
                ClientCard attacker = _exec.Enemy.BattlingMonster;
                if (attacker != null)
                {
                    // Opponent attacking Rexterm or direct attack
                    if (attacker.Attack >= 3000 || attacker.Attack >= _exec.Bot.LifePoints)
                        return true;
                }
                if (oppMonsters.Any(m => m.Attack >= 3000))
                    return true;
            }

            // In Main Phase:
            if (_exec.Duel.IsMainPhase())
            {
                // Only if an opponent monster is currently chained on field and its ATK is less than our LP (not locked yet!)
                if (_exec.CurrentLastChainCard != null && _exec.CurrentLastChainCard.Controller == 1 &&
                    _exec.CurrentLastChainCard.Location == CardLocation.MonsterZone &&
                    _exec.CurrentLastChainCard.Attack < _exec.Bot.LifePoints)
                {
                    return true;
                }

                // If opponent controls a high-threat monster with ATK >= 3000 that could threaten battle
                if (oppMonsters.Any(m => m.Attack >= 3000 && m.Attack < _exec.Bot.LifePoints))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class DinomorphiaTrapAdvisor
    {
        private readonly DinomorphiaExecutor _exec;

        public DinomorphiaTrapAdvisor(DinomorphiaExecutor exec) => _exec = exec;

        public bool ShouldCopyFrenzy()
        {
            return !_exec.Bot.HasInMonstersZone(DinomorphiaExecutor.CardId.DinomorphiaRexterm);
        }

        public bool ShouldCopyDomain()
        {
            return !_exec.Bot.HasInMonstersZone(DinomorphiaExecutor.CardId.DinomorphiaRexterm);
        }
    }

    public class DinomorphiaBoardAssessor
    {
        private readonly DinomorphiaExecutor _exec;

        public DinomorphiaBoardAssessor(DinomorphiaExecutor exec) => _exec = exec;

        public bool HasActiveRextermLock()
        {
            return _exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && !m.IsDisabled() &&
                                                    m.Id == DinomorphiaExecutor.CardId.DinomorphiaRexterm);
        }
    }
}
