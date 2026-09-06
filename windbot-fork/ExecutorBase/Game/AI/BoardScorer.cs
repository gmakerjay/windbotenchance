using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot.Game;

namespace WindBot.Game.AI
{
    /// <summary>
    /// BoardScorer — centralized combat/threat/lethal calculation engine.
    /// Pure calculation — no mutable state. All methods are static or take explicit parameters.
    /// Replaces duplicated CanDealLethal(), GetBestEnemyCard(), combat math across executors.
    /// 
    /// Architecture:
    ///   Executor.cs +1 line (public BoardScorer Scorer)
    ///   DefaultExecutor.OnNewTurn() → Scorer.Reset()
    ///   All executors → this.Scorer.ThreatScore(enemyMonster) etc.
    /// </summary>
    public class BoardScorer
    {
        // ═══════════════════════════════════════
        //  CACHED STATE (reset each turn)
        // ═══════════════════════════════════════

        private ClientField _bot;
        private ClientField _enemy;
        private Duel _duel;

        // Decision-level Cache (cleared at the start of any new decision step)
        private int? _cachedMaxDamage;
        private bool? _cachedHasLethal;
        private Dictionary<int, bool> _cachedHasLethalAfterRemoval = new Dictionary<int, bool>();


        public void ClearCache()
        {
            _cachedMaxDamage = null;
            _cachedHasLethal = null;
            _cachedHasLethalAfterRemoval.Clear();
        }

        /// <summary>Call from DefaultExecutor.OnNewTurn() to refresh references.</summary>
        public void Reset(ClientField bot, ClientField enemy, Duel duel = null)
        {
            _bot = bot;
            _enemy = enemy;
            _duel = duel;
            ClearCache();
        }

        // ═══════════════════════════════════════
        //  THREAT SCORING — how dangerous is a card?
        // ═══════════════════════════════════════

        /// <summary>
        /// Compute threat score for a single enemy card. Higher = more dangerous.
        /// Factors: ATK, card type (Extra Deck = scarier), floodgate status, negation potential.
        /// </summary>
        public int ThreatScore(ClientCard card)
        {
            if (card == null) return 0;
            int score = 0;

            // Base: ATK power
            score += card.Attack / 100;

            // Extra Deck monsters are usually boss monsters with dangerous effects
            if (card.HasType(CardType.Fusion)) score += 30;
            if (card.HasType(CardType.Synchro)) score += 35;
            if (card.HasType(CardType.Xyz)) score += 30;
            if (card.HasType(CardType.Link)) score += 25;
            if (card.HasType(CardType.Ritual)) score += 25;

            // High ATK bonus
            if (card.Attack >= 3000) score += 20;
            else if (card.Attack >= 2500) score += 10;

            // Floodgate detection — these cards define the game state
            if (card.IsFloodgate() || CardIntelligence.IsFloodgate(card.Id)) score += 50;

            // Known negators and disruptions (Baronne, Apollousa, Savage, Infinity, etc.)
            if (!card.IsDisabled() && CardIntelligence.IsKnownNegator(card.Id)) score += 45;

            // High threat chokepoints (Union Hangar, Circle, Eternal Soul, Multifaker, etc.)
            if (CardIntelligence.IsHighThreatChokepoint(card.Id)) score += 35;

            // Dangerous battle effects (honest, utopia lightning, etc.)
            if (card.IsMonsterDangerous()) score += 40;

            // Invincible in battle (can't be destroyed)
            if (card.IsMonsterInvincible()) score += 15;

            // Has negation or disruption potential (Extra Deck monsters with effects)
            if (!card.IsDisabled() && (card.HasType(CardType.Fusion) || card.HasType(CardType.Synchro)
                || card.HasType(CardType.Xyz) || card.HasType(CardType.Link)))
                score += 15;

            // Face-up continuous / field spells provide permanent advantage
            if (card.IsFaceup() && (card.HasType(CardType.Continuous) || card.HasType(CardType.Field)))
                score += 25;

            // Face-down unknown card — treat as moderate threat
            if (card.IsFacedown()) score = 20;

            return score;
        }

        /// <summary>
        /// Get the highest-threat enemy monster on the field.
        /// Filters out cards that shouldn't be targeted.
        /// </summary>
        public ClientCard GetHighestThreat(bool onlyFaceup = true, bool canBeTarget = true)
        {
            if (_enemy == null) return null;
            return _enemy.GetMonsters()
                .Where(c => c != null && (!onlyFaceup || c.IsFaceup()) && (!canBeTarget || !c.IsShouldNotBeTarget()))
                .OrderByDescending(c => ThreatScore(c))
                .FirstOrDefault();
        }

        /// <summary>
        /// Get the highest-threat enemy spell/trap.
        /// </summary>
        public ClientCard GetHighestThreatSpell()
        {
            if (_enemy == null) return null;
            return _enemy.GetSpells()
                .Where(c => c != null)
                .OrderByDescending(c => ThreatScore(c))
                .FirstOrDefault();
        }

        /// <summary>
        /// Combined: best removal target across monsters and spells.
        /// </summary>
        public ClientCard GetBestRemovalTarget()
        {
            var monster = GetHighestThreat();
            var spell = GetHighestThreatSpell();
            int monsterScore = ThreatScore(monster);
            int spellScore = ThreatScore(spell);
            return monsterScore >= spellScore ? monster : spell;
        }

        // ═══════════════════════════════════════
        //  COMBAT CALCULATION — accurate damage prediction
        // ═══════════════════════════════════════

        /// <summary>
        /// Check if a monster has a piercing effect based on its card ID.
        /// </summary>
        public static bool IsPierce(ClientCard card)
        {
            if (card == null) return false;
            switch (card.Id)
            {
                case 55410871: // Blue-Eyes Chaos MAX Dragon (inflicts double piercing)
                case 51788412: // Chaos Ancient Gear Giant
                case 12652643: // Ultimate Ancient Gear Golem
                case 12307878: // Invoked Purgatrio
                case 2055794:  // Cyber End Dragon
                case 11954712: // Dark Driceratops
                case 27143874: // Dino Sewing
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Calculate damage our monster deals to an enemy monster.
        /// Returns 0 if our monster dies (ATK ≤ enemy ATK in attack position).
        /// Handles: attack position, defense position, piercing, invincible, dangerous.
        /// </summary>
        public int CalculateCombatDamage(ClientCard attacker, ClientCard defender)
        {
            if (attacker == null || defender == null) return 0;
            if (!attacker.IsAttack()) return 0; // Can't attack if not in attack position

            // Dangerous monster check — if defender is dangerous and not disabled, treat as unwinnable
            if (defender.IsMonsterDangerous() && defender.IsAttack() && !defender.IsDisabled())
            {
                // Exception: our attacker has a counter (e.g., El Shaddoll Construct vs Special Summoned)
                if (attacker.IsCode(20366274) && defender.IsSpecialSummoned) // El Shaddoll Construct
                    return 0; // Destroyed before damage step, no battle damage
                // Exception: Ally of Justice Catastor vs non-DARK
                if (attacker.IsCode(26593852) && !defender.HasAttribute(CardAttribute.Dark))
                    return 0; // Destroyed before damage step, no battle damage
                return 0; // Can't win this fight
            }

            // Invincible monster — can't be destroyed, but can still deal damage if in attack position
            if (defender.IsMonsterInvincible() && defender.IsAttack())
                return attacker.Attack > defender.Attack ? attacker.Attack - defender.Attack : 0;

            // Normal combat
            if (defender.IsAttack())
            {
                if (attacker.Attack > defender.Attack)
                    return attacker.Attack - defender.Attack;
                return 0; // We lose or tie
            }
            else
            {
                if (attacker.Attack > defender.Defense)
                {
                    if (attacker.IsCode(55410871)) // Blue-Eyes Chaos MAX Dragon double piercing
                        return (attacker.Attack - defender.Defense) * 2;
                    if (IsPierce(attacker))
                        return attacker.Attack - defender.Defense;
                    return 0; // No piercing damage
                }
                return 0; // Can't get over defense
            }
        }

        // ═══════════════════════════════════════
        //  LETHAL DETECTION — can we win this turn?
        // ═══════════════════════════════════════

        /// <summary>
        /// Calculate maximum possible battle damage this turn, considering all attackers and blockers.
        /// Uses simulated combat: pairs attackers with defenders optimally for maximum damage.
        /// </summary>
        public int CalculateMaxDamage(int removalTargets = 0)
        {
            if (removalTargets == 0 && _cachedMaxDamage.HasValue)
                return _cachedMaxDamage.Value;

            if (_bot == null || _enemy == null) return 0;

            var attackers = _bot.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && c.IsAttack() && !c.Attacked)
                .OrderByDescending(c => c.Attack)
                .ToList();

            var blockers = _enemy.GetMonsters()
                .Where(c => c != null)
                .OrderByDescending(c => c.IsFaceup() ? (c.IsAttack() ? c.Attack : c.Defense) : (c.Data?.Defense ?? 0))
                .Skip(removalTargets)
                .ToList();

            if (attackers.Count == 0) return 0;

            bool[] blockerDestroyed = new bool[blockers.Count];
            int totalDamage = OptimizeCombat(attackers, blockers, 0, blockerDestroyed);

            if (removalTargets == 0)
                _cachedMaxDamage = totalDamage;

            return totalDamage;
        }

        private int OptimizeCombat(List<ClientCard> attackers, List<ClientCard> blockers, int attackerIndex, bool[] blockerDestroyed)
        {
            if (attackerIndex >= attackers.Count)
                return 0;

            var attacker = attackers[attackerIndex];

            // Count remaining active blockers
            int activeBlockerCount = 0;
            for (int i = 0; i < blockers.Count; ++i)
                if (!blockerDestroyed[i]) activeBlockerCount++;

            // Early exit: if no active blockers, all remaining attackers can attack directly
            if (activeBlockerCount == 0)
            {
                int directDmg = 0;
                for (int idx = attackerIndex; idx < attackers.Count; ++idx)
                {
                    directDmg += attackers[idx].Attack;
                }
                return directDmg;
            }

            int maxDmg = 0;

            // Option 1: Attack directly if the card has CanDirectAttack capability
            if (attacker.CanDirectAttack)
            {
                int dmg = attacker.Attack + OptimizeCombat(attackers, blockers, attackerIndex + 1, blockerDestroyed);
                if (dmg > maxDmg) maxDmg = dmg;
            }

            // Option 2: Attack one of the active blockers
            for (int i = 0; i < blockers.Count; ++i)
            {
                if (blockerDestroyed[i]) continue;

                var defender = blockers[i];
                int dmg = CalculateCombatDamage(attacker, defender);

                // Determine if defender is destroyed in battle
                bool isDestroyed = false;
                if (!defender.IsMonsterInvincible() || defender.IsDisabled())
                {
                    if (defender.IsFacedown())
                    {
                        // [FIX CRITICAL-2] Use conservative default DEF for unknown face-downs.
                        // Previously defaulted to 0, causing false lethal detection when
                        // opponent had face-down monsters with unknown card data.
                        int defVal = defender.Data?.Defense ?? 2000;
                        if (attacker.Attack > defVal) isDestroyed = true;
                    }
                    else if (defender.IsAttack())
                    {
                        if (attacker.Attack > defender.Attack) isDestroyed = true;
                    }
                    else // Defense
                    {
                        if (attacker.Attack > defender.Defense) isDestroyed = true;
                    }
                }

                if (isDestroyed) blockerDestroyed[i] = true;
                
                int total = dmg + OptimizeCombat(attackers, blockers, attackerIndex + 1, blockerDestroyed);
                if (total > maxDmg) maxDmg = total;

                if (isDestroyed) blockerDestroyed[i] = false; // backtrack
            }

            // Option 3: Do not attack with this attacker
            int passDmg = OptimizeCombat(attackers, blockers, attackerIndex + 1, blockerDestroyed);
            if (passDmg > maxDmg) maxDmg = passDmg;

            return maxDmg;
        }

        /// <summary>
        /// True if we have lethal this turn through combat.
        /// Considers enemy blockers and our available attackers.
        /// </summary>
        public bool HasLethal()
        {
            if (_cachedHasLethal.HasValue) return _cachedHasLethal.Value;
            bool result = CalculateMaxDamage() >= (_enemy?.LifePoints ?? 0);
            _cachedHasLethal = result;
            return result;
        }

        /// <summary>
        /// True if we can clear the board AND have lethal.
        /// Used when deciding whether to use removal before attacking.
        /// </summary>
        public bool HasLethalAfterRemoval(int removalTargets)
        {
            if (_cachedHasLethalAfterRemoval.TryGetValue(removalTargets, out bool val))
                return val;
            bool result = CalculateMaxDamage(removalTargets) >= (_enemy?.LifePoints ?? 0);
            _cachedHasLethalAfterRemoval[removalTargets] = result;
            return result;
        }

        // ═══════════════════════════════════════
        //  BOARD ADVANTAGE — numeric score of who's winning
        // ═══════════════════════════════════════

        /// <summary>
        /// Compute a numeric advantage score. Positive = we're winning.
        /// Factors: LP difference, field presence, hand advantage, threat scores.
        /// </summary>
        public int BoardAdvantageScore()
        {
            if (_bot == null || _enemy == null) return 0;
            int score = 0;

            // LP advantage
            score += (_bot.LifePoints - _enemy.LifePoints) / 400;

            // Monster advantage
            score += (_bot.GetMonsterCount() - _enemy.GetMonsterCount()) * 10;

            // Spell/Trap advantage
            score += (_bot.GetSpellCount() - _enemy.GetSpellCount()) * 5;

            // Hand advantage
            score += (_bot.Hand.Count - _enemy.Hand.Count) * 8;

            // Threat differential (their threats vs our threats)
            int enemyThreat = _enemy.GetMonsters().Sum(c => ThreatScore(c));
            int ourThreat = _bot.GetMonsters().Sum(c => ThreatScore(c));
            score += (ourThreat - enemyThreat) / 5;

            return score;
        }

        /// <summary>
        /// True if we have a significant advantage (score >= 15).
        /// </summary>
        public bool WeAreWinning()
        {
            return BoardAdvantageScore() >= 15;
        }

        /// <summary>
        /// True if opponent has a significant advantage (score <= -15).
        /// </summary>
        public bool WeAreLosing()
        {
            return BoardAdvantageScore() <= -15;
        }

        // ═══════════════════════════════════════
        //  COMBAT SAFETY — should we attack?
        // ═══════════════════════════════════════

        /// <summary>
        /// True if this attack is safe (we win or trade favorably).
        /// </summary>
        public bool IsSafeAttack(ClientCard attacker, ClientCard defender)
        {
            if (attacker == null || defender == null) return false;
            if (!attacker.IsAttack()) return false;

            // Dangerous or invincible — not safe
            if (defender.IsMonsterDangerous() && !defender.IsDisabled())
            {
                // Check exceptions (Construct vs special summoned, Catastor vs non-DARK)
                bool canIgnoreIt = !attacker.IsDisabled() && (
                    attacker.IsCode(20366274) && defender.IsSpecialSummoned || // El Shaddoll Construct
                    attacker.IsCode(26593852) && !defender.HasAttribute(CardAttribute.Dark) // Ally of Justice Catastor
                );
                if (!canIgnoreIt)
                    return false;
            }
            if (defender.IsMonsterInvincible() && !defender.IsDisabled()) return false;

            // Facedown — usually safe (we might hit a flip effect though)
            if (defender.IsFacedown()) return true;

            // Can't lose to defense position (we don't take damage)
            if (defender.IsDefense() && attacker.Attack > defender.Defense) return true;

            // Attack position: we win
            if (defender.IsAttack() && attacker.Attack > defender.Attack) return true;

            return attacker.Attack > defender.GetDefensePower();
        }

        // ═══════════════════════════════════════
        //  REMOVAL PRIORITY — what to destroy/banish/bounce first
        // ═══════════════════════════════════════

        /// <summary>
        /// Rank removal targets by priority. Returns ordered list.
        /// Considers: floodgates → negates → high ATK → combo pieces.
        /// </summary>
        public List<ClientCard> GetRemovalPriority()
        {
            if (_enemy == null) return new List<ClientCard>();

            var targets = new List<ClientCard>();
            targets.AddRange(_enemy.GetMonsters().Where(c => c != null));
            targets.AddRange(_enemy.GetSpells().Where(c => c != null));

            return targets
                .OrderByDescending(c =>
                {
                    int priority = 0;
                    if (c.IsFloodgate()) priority += 1000;
                    if (c.HasType(CardType.Continuous) || c.HasType(CardType.Field) || c.HasType(CardType.Equip))
                        priority += 500;
                    if (c.IsMonsterDangerous()) priority += 300;
                    priority += ThreatScore(c);
                    return priority;
                })
                .ToList();
        }

        // ═══════════════════════════════════════
        //  SMART FLOW v2 — Attack Opportunity & Board Sufficiency
        // ═══════════════════════════════════════

        /// <summary>
        /// Estimate how much battle damage we'd deal if we entered Battle Phase right now.
        /// Considers all attackers vs all defenders optimally.
        /// </summary>
        public int EstimateDamageIfAttackNow()
        {
            return CalculateMaxDamage(0);
        }

        /// <summary>
        /// Board Sufficiency Score (0-100+).
        /// Measures how "complete" our board is. ≥60 = we should consider stopping extension.
        /// Factors: disruption count, attacker quality, backrow, hand traps, overextension penalty.
        /// 
        /// Going-second adjustment: score is REDUCED because we WANT to keep pushing
        /// (need more board to justify stopping when opponent might have responses).
        /// </summary>
        public int BoardSufficiencyScore()
        {
            if (_bot == null || _enemy == null) return 0;

            int score = 0;

            // Disruption: negate monsters on field
            foreach (var m in _bot.GetMonsters())
            {
                if (m == null || !m.IsFaceup() || m.IsDisabled()) continue;

                // Known negate monsters = high-quality disruption
                if (m.IsFloodgate() || (m.Attack >= 2500 && m.IsExtraCard()))
                    score += 20;
                else if (m.Attack >= 2000)
                    score += 10;
                else
                    score += 5;
            }

            // Backrow disruptions (face-down S/T)
            int backrowCount = _bot.GetSpells().Count(c => c != null && c.IsFacedown());
            score += backrowCount * 10;

            // Hand traps in hand (reduced weight — having hand traps shouldn't stop comboing)
            foreach (var c in _bot.Hand)
            {
                if (c == null) continue;
                switch (c.Id)
                {
                    case 14558127:  // Ash Blossom
                    case 23434538:  // Maxx "C"
                    case 94145021:  // Droll & Lock Bird
                    case 63845230:  // Effect Veiler
                    case 59438930:  // Ghost Ogre
                    case 73642296:  // Ghost Belle
                    case 10045474:  // Infinite Impermanence
                        score += 5; // Mild — these don't mean "board is done"
                        break;
                }
            }

            // Overextension penalty: too many monsters = Nibiru/board wipe risk
            int monsterCount = _bot.GetMonsterCount();
            if (monsterCount >= 5) score -= 25;
            else if (monsterCount >= 4) score -= 10;

            // Low hand penalty: if hand is nearly empty, board needs to be stronger
            if (_bot.Hand.Count <= 1) score -= 10;

            // ── Going-second penalty: we WANT to keep pushing, not sit on half a board ──
            // Reduce score so the bot needs MORE board presence to justify stopping
            if (_duel != null && _duel.Turn >= 2 && _duel.Player == 0)
            {
                score -= 15;

                // If enemy still has threats, penalize even more — we need to deal with them
                if (_enemy.GetMonsterCount() > 0)
                    score -= 10;
            }

            return score;
        }

        /// <summary>
        /// Attack Opportunity Score (0-100+).
        /// Measures how good the current moment is for attacking.
        /// ≥60 = "should attack before doing more combos"
        /// ≥100 = "definitely attack now, opponent board is wide open"
        /// 
        /// Tuned for balanced aggression:
        ///   - Backrow penalty is mild (not scared of 1-2 face-downs)
        ///   - Lethal overrides backrow fear entirely
        ///   - Going-second gets aggression bonus
        /// </summary>
        public int AttackOpportunityScore()
        {
            if (_bot == null || _enemy == null) return 0;

            int score = 0;
            int enemyMonsters = _enemy.GetMonsterCount();
            int ourAttackers = _bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.IsAttack() && c.Attack > 0);

            if (ourAttackers == 0) return 0; // Can't attack

            // Calculate total ATK for damage estimates
            int totalATK = 0;
            foreach (var m in _bot.GetMonsters())
            {
                if (m != null && m.IsFaceup() && m.IsAttack())
                    totalATK += m.Attack;
            }

            // ── Lethal override: if we can kill, backrow doesn't matter ──
            // Even a Mirror Force only saves them 1 turn — we should try
            if (enemyMonsters == 0 && totalATK >= _enemy.LifePoints)
                return 150; // Maximum aggression — go for the kill

            // ── Empty board = massive opportunity ──
            if (enemyMonsters == 0)
            {
                score += 100;
                if (totalATK >= _enemy.LifePoints / 2) score += 20; // Half HP+
            }
            // ── All enemies in defense = good opportunity ──
            else if (_enemy.GetMonsters().All(c => c == null || c.IsDefense() || c.IsFacedown()))
            {
                score += 50;
            }
            // ── We can beat all their monsters ──
            else
            {
                int ourBestATK = _bot.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && c.IsAttack())
                    .Select(c => c.Attack)
                    .DefaultIfEmpty(0)  // [FIX MINOR-3] Prevent crash on empty sequence
                    .Max();
                int enemyBestPower = _enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup())
                    .Select(c => c.IsAttack() ? c.Attack : c.Defense)
                    .DefaultIfEmpty(0)
                    .Max();

                if (ourBestATK > enemyBestPower) score += 30;
            }

            // ── Going-second aggression bonus ──
            // When going second, we WANT to push damage — be braver
            if (_duel != null && _duel.Turn >= 2 && _duel.Player == 0)
                score += 20;

            // ── Backrow risk: mild penalty ──
            // Don't be too scared of backrow — the reward of attacking often outweighs risk
            // Only penalize if we have non-lethal damage (lethal already returned above)
            int enemyBackrow = _enemy.GetSpells().Count(c => c != null && c.IsFacedown());
            score -= enemyBackrow * 8;

            // ── Dangerous monsters reduce opportunity (but don't panic) ──
            foreach (var m in _enemy.GetMonsters())
            {
                if (m != null && m.IsFaceup() && !m.IsDisabled())
                {
                    if (m.IsMonsterDangerous()) score -= 25;
                    if (m.IsMonsterInvincible()) score -= 15;
                }
            }

            return score;
        }
    }
}
