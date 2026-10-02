using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Decks;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Plugins
{
    // ============================================================================
    // MEMENTO DOMAIN PLUGIN — Strategy, Material, Threat & Resource Evaluators
    // ============================================================================

    public class MementoPlugin
    {
        public MementoExecutor Executor { get; }
        public MementoStrategy Strategy { get; }
        public MementoMaterialEvaluator MaterialEvaluator { get; }
        public MementoThreatEvaluator ThreatEvaluator { get; }
        public MementoResourceLoop ResourceLoop { get; }
        public MementoBoardAssessor BoardAssessor { get; }

        public MementoPlugin(MementoExecutor executor)
        {
            Executor = executor;
            Strategy = new MementoStrategy(executor);
            MaterialEvaluator = new MementoMaterialEvaluator(executor);
            ThreatEvaluator = new MementoThreatEvaluator(executor);
            ResourceLoop = new MementoResourceLoop(executor);
            BoardAssessor = new MementoBoardAssessor(executor);
        }
    }

    /// <summary>
    /// Evaluates high-level strategy, combo paths, lethal checks, and stop conditions.
    /// </summary>
    public class MementoStrategy
    {
        private readonly MementoExecutor _executor;

        public MementoStrategy(MementoExecutor executor)
        {
            _executor = executor;
        }

        public bool IsLethalConfirmed()
        {
            // Combined Creation has 5000 ATK and can attack all monsters; Creation King has 3000 ATK
            int totalAttack = _executor.Bot.GetMonsters()
                .Where(m => m != null && m.IsFaceup() && m.IsAttack())
                .Sum(m => m.Attack);

            if (_executor.Enemy.GetMonsterCount() == 0 && totalAttack >= _executor.Enemy.LifePoints)
                return true;

            bool hasCombinedCreation = _executor.Bot.HasInMonstersZone(MementoExecutor.CardId.MementoalTecuhtlica);
            if (hasCombinedCreation && _executor.Bot.GetMonsterCount() == 1)
            {
                // Can attack all monsters with 5000 ATK
                int potentialDamage = 0;
                foreach (var enemyMonster in _executor.Enemy.GetMonsters().Where(m => m != null && m.IsFaceup()))
                {
                    potentialDamage += Math.Max(0, 5000 - enemyMonster.Attack);
                }
                if (potentialDamage >= _executor.Enemy.LifePoints) return true;
            }

            return false;
        }

        public bool ShouldOverextend()
        {
            // Stop expanding if end board is solid (Combined Creation + Creation King / Twin Dragon)
            return !BoardAssessorHasEndBoard();
        }

        public bool BoardAssessorHasEndBoard()
        {
            bool hasBoss = _executor.Bot.HasInMonstersZone(MementoExecutor.CardId.MementoalTecuhtlica);
            bool hasFusion = _executor.Bot.HasInMonstersZone(MementoExecutor.CardId.CreationKing) ||
                             _executor.Bot.HasInMonstersZone(MementoExecutor.CardId.TwinDragon);
            return hasBoss && hasFusion;
        }
    }

    /// <summary>
    /// Material Evaluator: Enforces strict material hierarchy.
    /// Priority: Fodder/Triggers (Shleepy, Ghattic, Akihiron, Tatsunootoshigo) -> Intermediate -> Bosses (PROTECTED).
    /// </summary>
    public class MementoMaterialEvaluator
    {
        private readonly MementoExecutor _executor;

        public MementoMaterialEvaluator(MementoExecutor executor)
        {
            _executor = executor;
        }

        /// <summary>
        /// Scores cards for destruction / tribute selection.
        /// Higher score means better card to destroy/tribute.
        /// </summary>
        public int GetDestructionPriority(ClientCard card)
        {
            if (card == null) return -9999;

            // NEVER destroy Bosses unless lethal or no other choice
            if (card.IsCode(MementoExecutor.CardId.MementoalTecuhtlica)) return -10000;
            if (card.IsCode(MementoExecutor.CardId.CreationKing)) return -8000;

            // Twin Dragon floats into Lv6 or lower Memento from GY when destroyed!
            if (card.IsCode(MementoExecutor.CardId.TwinDragon)) return 3500;

            // Horned Dragon pops 3 face-up cards when destroyed!
            if (card.IsCode(MementoExecutor.CardId.HornedDragon)) return 3000;

            // Shleepy sends Memento to GY when destroyed!
            if (card.IsCode(MementoExecutor.CardId.Shleepy)) return 2800;

            // Akihiron retrieves banished/GY Memento when destroyed!
            if (card.IsCode(MementoExecutor.CardId.Akihiron)) return 2500;

            // Ghattic already used effect on field
            if (card.IsCode(MementoExecutor.CardId.Ghattic)) return 1500;

            // Goblin, Mace, Dark Blade on field
            if (card.IsCode(MementoExecutor.CardId.Goblin) || card.IsCode(MementoExecutor.CardId.Mace)) return 1200;
            if (card.IsCode(MementoExecutor.CardId.DarkBlade)) return 1100;
            if (card.IsCode(MementoExecutor.CardId.Angwitch)) return 1000;

            return 500;
        }

        /// <summary>
        /// Scores cards for Fusion material selection.
        /// Higher score means preferred material.
        /// </summary>
        public int GetFusionMaterialScore(ClientCard card)
        {
            if (card == null) return -9999;
            if (card.Controller == 1) return 10000; // Opponent monster (Super Poly) is top priority!

            // Boss protection: never fuse Combined Creation or Creation King
            if (card.IsCode(MementoExecutor.CardId.MementoalTecuhtlica)) return -10000;
            if (card.IsCode(MementoExecutor.CardId.CreationKing)) return -8000;

            // From GY (via Mementotlan Fusion) — TOP PRIORITY for friendly materials to conserve field!
            if (card.Location == CardLocation.Grave)
            {
                // Preserve Ghattic if it hasn't revived yet
                if (card.IsCode(MementoExecutor.CardId.Ghattic) && !_executor.Bot.HasInMonstersZone(MementoExecutor.CardId.Ghattic))
                    return 200;
                return 5000;
            }

            // From Hand
            if (card.Location == CardLocation.Hand)
            {
                if (card.IsCode(MementoExecutor.CardId.Shleepy)) return 2500;
                if (card.IsCode(MementoExecutor.CardId.Ghattic)) return 2400;
                if (card.IsCode(MementoExecutor.CardId.Akihiron)) return 2300;
                return 2000;
            }

            // From Field — Preserve monsters on field whenever possible!
            if (card.Location == CardLocation.MonsterZone)
            {
                if (card.IsCode(MementoExecutor.CardId.TwinDragon)) return 1500; // Floats on destroy/material
                return 1000;
            }

            return 100;
        }
    }

    /// <summary>
    /// Threat Evaluator: Prioritizes opponent targets for Creation King Quick pop, Mace steal, Droplet, and Super Poly.
    /// </summary>
    public class MementoThreatEvaluator
    {
        private readonly MementoExecutor _executor;

        public MementoThreatEvaluator(MementoExecutor executor)
        {
            _executor = executor;
        }

        public ClientCard GetBestOpponentTarget(IEnumerable<ClientCard> candidates)
        {
            if (candidates == null) return null;

            return candidates
                .OrderByDescending(c => {
                    if (c == null) return -1;
                    int score = 0;
                    if (CardIntelligence.IsFloodgate(c.Id)) score += 10000;
                    if (CardIntelligence.IsKnownNegator(c.Id)) score += 8000;
                    if (CardIntelligence.IsHighThreatChokepoint(c.Id)) score += 6000;
                    if (c.IsMonster())
                    {
                        score += c.Attack;
                        if (c.IsFaceup() && !c.IsDisabled()) score += 2000;
                    }
                    else if (c.IsFacedown())
                    {
                        score += 3000; // Backrow threat
                    }
                    return score;
                })
                .FirstOrDefault();
        }

        public ClientCard GetBestStealTarget(IEnumerable<ClientCard> monsters)
        {
            if (monsters == null) return null;

            return monsters
                .Where(m => m != null && m.IsFaceup() && !m.IsShouldNotBeTarget())
                .OrderByDescending(m => {
                    int score = m.Attack;
                    if (CardIntelligence.IsKnownNegator(m.Id)) score += 5000;
                    if (CardIntelligence.IsFloodgate(m.Id)) score += 4000;
                    return score;
                })
                .FirstOrDefault();
        }
    }

    /// <summary>
    /// Tracks GY Memento names, Shleepy triggers, Ghattic recursion, and Combined Creation readiness.
    /// </summary>
    public class MementoResourceLoop
    {
        private readonly MementoExecutor _executor;

        public MementoResourceLoop(MementoExecutor executor)
        {
            _executor = executor;
        }

        public int GetUniqueMementoNamesInGY()
        {
            return _executor.Bot.Graveyard
                .Where(c => c != null && c.IsMonster() && IsMementoCard(c))
                .Select(c => c.Id)
                .Distinct()
                .Count();
        }

        public int GetUniqueMementoNamesInHandAndGY()
        {
            var gyNames = _executor.Bot.Graveyard
                .Where(c => c != null && c.IsMonster() && IsMementoCard(c) && c.Id != MementoExecutor.CardId.MementoalTecuhtlica)
                .Select(c => c.Id);

            var handNames = _executor.Bot.Hand
                .Where(c => c != null && c.IsMonster() && IsMementoCard(c) && c.Id != MementoExecutor.CardId.MementoalTecuhtlica)
                .Select(c => c.Id);

            return gyNames.Concat(handNames).Distinct().Count();
        }

        public bool CanSpecialSummonCombinedCreation()
        {
            // Needs 5 other Memento monsters with different names in Hand and/or GY
            bool hasInHandOrGY = _executor.Bot.HasInHand(MementoExecutor.CardId.MementoalTecuhtlica) ||
                                 _executor.Bot.HasInGraveyard(MementoExecutor.CardId.MementoalTecuhtlica);

            return hasInHandOrGY && GetUniqueMementoNamesInHandAndGY() >= 5;
        }

        public bool CanSpecialSummonHornedDragon()
        {
            // Needs 3 or more Memento monsters with different names in GY
            return _executor.Bot.HasInHand(MementoExecutor.CardId.HornedDragon) &&
                   GetUniqueMementoNamesInGY() >= 3;
        }

        public static bool IsMementoCard(ClientCard card)
        {
            if (card == null) return false;
            return card.HasSetcode(0x19a); // SET_MEMENTO in official CDB
        }
    }

    /// <summary>
    /// Assesses board state: Combined Creation presence, Goblin target protection, and Field Spell protection.
    /// </summary>
    public class MementoBoardAssessor
    {
        private readonly MementoExecutor _executor;

        public MementoBoardAssessor(MementoExecutor executor)
        {
            _executor = executor;
        }

        public bool HasCombinedCreationOnField()
        {
            return _executor.Bot.HasInMonstersZone(MementoExecutor.CardId.MementoalTecuhtlica);
        }

        public bool HasFieldSpellActive()
        {
            return _executor.Bot.HasInSpellZone(MementoExecutor.CardId.Mementomictlan);
        }

        public bool HasCreationKingOnField()
        {
            return _executor.Bot.HasInMonstersZone(MementoExecutor.CardId.CreationKing);
        }

        public bool HasTwinDragonOnField()
        {
            return _executor.Bot.HasInMonstersZone(MementoExecutor.CardId.TwinDragon);
        }
    }
}
