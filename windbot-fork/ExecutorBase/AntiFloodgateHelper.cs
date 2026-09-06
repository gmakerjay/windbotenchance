using System;
using System.Collections.Generic;
using System.Linq;
using WindBot.Game;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI
{
    /// <summary>
    /// Shared utility class for 2026 Executors.
    /// Consolidates floodgate detection, threat assessment, board evaluation, and combat safety
    /// patterns that were previously duplicated across 16+ executor files.
    /// 
    /// Usage: Call static methods from executor logic, passing Bot, Enemy, Duel as needed.
    /// 
    /// Patterns consolidated:
    ///   - IsSpecialSummonBlocked() — floodgate monster/spell detection (12+ executors)
    ///   - OpponentHasThreateningMonster() — threat assessment (10+ executors)
    ///   - EnemyHasKnownNegate() — negate monster detection (8+ executors)
    ///   - IsInGrindGame() — resource poverty check (12+ executors)
    ///   - NeedsBoardPresence() — board state urgency (12+ executors)
    ///   - CanDealLethal() — lethal detection (12+ executors)
    ///   - AreSpellsNegatedOrDisabled() — spell lock detection
    ///   - IsSafeToAttack() / IsSafeToDefend() — combat safety (10+ executors)
    ///   - NegateMonsterIds — shared negate monster ID list (8+ executors)
    ///   - FloodgateMonsterIds / FloodgateSpellIds — floodgate ID lists
    /// </summary>
    public static class AntiFloodgateHelper
    {
        // ═══════════════════════════════════════════════════════════════
        //  SHARED CONSTANT ARRAYS
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Common negate monsters that stop plays. Used by EnemyHasKnownNegate() and threat assessment.
        /// Consolidated from 8+ executor files.
        /// </summary>
        public static readonly int[] NegateMonsterIds = {
            4280258,   // Apollousa, Bow of the Goddess
            84815190,  // Baronne de Fleur
            10443957,  // Cyber Dragon Infinity
            86066372,  // Herald of Ultimateness
            31801517,  // Evolzar Dolkka
            57793869,  // Borreload Savage Dragon
            17330115,  // Hot Red Dragon Archfiend Abyss
            21522601,  // Witchcrafter Madame Verre
            84523092,  // Witchcrafter Haine
            1508649,   // Altergeist Hexstia
            1561110,   // ABC-Dragon Buster
            63767246,  // Number 38: Hope Harbinger Dragon Titanic Galaxy
        };

        /// <summary>
        /// Floodgate monsters that block Special Summons. Used by IsSpecialSummonBlocked().
        /// Consolidated from 12+ executor files.
        /// </summary>
        public static readonly int[] FloodgateMonsterIds = {
            78193831,  // Vanity's Fiend
            67922702,  // Fossil Dyna Pachycephalo
            96015934,  // Jowgen the Spiritualist
            14212200,  // Archlord Kristya
            86325573,  // Barrier Statue of the Stormwinds
            3717252,   // Vanity's Ruler
            85359414,  // El Shaddoll Winda
            87294958,  // Known SS-blocking floodgate (used in original executors)
        };

        /// <summary>
        /// Floodgate spells/traps that block Special Summons. Used by IsSpecialSummonBlocked().
        /// Consolidated from 12+ executor files.
        /// </summary>
        public static readonly int[] FloodgateSpellIds = {
            5851097,   // Vanity's Emptiness
            4514109,   // Summon Limit
            22046459,  // Gozen Match
            34487429,  // Rivalry of Warlords
            2429943,   // Anti-Spell Fragrance
        };

        /// <summary>
        /// Spell/Trap negate monsters used by EnemyHasOncePerTurnSpellNegator().
        /// Consolidated from Branded executor.
        /// </summary>
        public static readonly int[] SpellNegatorIds = {
            63767246,  // Unknown Negator (generic)
            84815190,  // Baronne de Fleur
            27548133,  // Borreload Savage Dragon
            9753964,   // Hot Red Dragon Archfiend Abyss
        };

        /// <summary>
        /// Preemptive Infinite Impermanence targets — high-value monsters worth negating.
        /// Consolidated from multiple executors.
        /// </summary>
        public static readonly int[] PreemptiveImpermTargetIds = {
            21522601,  // Witchcrafter Madame Verre
            84523092,  // Witchcrafter Haine
            1561110,   // ABC-Dragon Buster
            4280258,   // Apollousa, Bow of the Goddess
            10443957,  // Cyber Dragon Infinity
            84815190,  // Baronne de Fleur
            1508649,   // Altergeist Hexstia
        };

        // ═══════════════════════════════════════════════════════════════
        //  FLOODGATE DETECTION
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Checks if Special Summons are currently blocked by floodgate monsters or spells.
        /// Checks both opponent AND our own field (Symmetric floodgates like Skill Drain affect both).
        /// Consolidated from 12+ executor files.
        /// </summary>
        public static bool IsSpecialSummonBlocked(ClientField bot, ClientField enemy)
        {
            if (CardIntelligence.IsSpecialSummonBlocked(enemy, bot)) return true;

            // Check monsters
            var allMonsters = enemy.GetMonsters().Concat(bot.GetMonsters())
                .Where(c => c != null && c.IsFaceup() && !c.IsDisabled());

            foreach (var card in allMonsters)
            {
                // Built-in floodgate check
                if (card.IsFloodgate() || CardIntelligence.IsFloodgateMonster(card.Id)) return true;
                // Known floodgate monster IDs
                if (FloodgateMonsterIds.Contains(card.Id)) return true;
                // El Shaddoll Winda & Bagooska checks
                if (card.IsCode(19261966, 85359414, 26273196)) return true;
            }

            // Check spells/traps
            var allSpells = enemy.GetSpells().Concat(bot.GetSpells())
                .Where(c => c != null && c.IsFaceup() && !c.IsDisabled());

            foreach (var card in allSpells)
            {
                if (CardIntelligence.IsFloodgateSpellTrap(card.Id) || FloodgateSpellIds.Contains(card.Id)) return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if our spells are currently negated or disabled by opponent floodgates.
        /// Consolidated from Branded executor.
        /// 
        /// Optional activatingCardLocation: if provided and Anti-Spell Fragrance is active,
        /// only return true when the activating card is in hand (matching Anti-Spell behavior).
        /// If null, always return true when Anti-Spell is active.
        /// </summary>
        public static bool AreSpellsNegatedOrDisabled(ClientField bot, ClientField enemy, CardLocation? activatingCardLocation = null)
        {
            // Imperial Order check
            bool orderActive = bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(61740673) && !c.IsDisabled())
                || enemy.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(61740673) && !c.IsDisabled());
            if (orderActive) return true;

            // Naturia Beast check
            bool natBeast = enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(33198837) && !c.IsDisabled());
            if (natBeast) return true;

            // Secret Village of the Spellcasters check
            bool enemyHasVillage = enemy.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(68462976) && !c.IsDisabled());
            if (enemyHasVillage)
            {
                bool enemyHasSpellcaster = enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Race == (int)CardRace.SpellCaster);
                bool weHaveSpellcaster = bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Race == (int)CardRace.SpellCaster);
                if (enemyHasSpellcaster && !weHaveSpellcaster) return true;
            }

            // Anti-Spell Fragrance check — only blocks cards activated from hand
            bool antiSpell = bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(58921041) && !c.IsDisabled())
                || enemy.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(58921041) && !c.IsDisabled());
            if (antiSpell)
            {
                // If caller provided a card location, only block hand activations (Anti-Spell behavior)
                if (activatingCardLocation == null || activatingCardLocation.Value == CardLocation.Hand)
                    return true;
                // Card is on field — Anti-Spell Fragrance does NOT block field-activated spells
                return false;
            }

            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  THREAT ASSESSMENT
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Checks if the opponent controls a threatening monster (floodgate, high ATK, or extra deck boss).
        /// Consolidated from 10+ executor files.
        /// 
        /// Threshold variant: use threshold parameter to adjust ATK cutoff (default 2500).
        /// Most executors use 2500-3000; 2500 is the safer default.
        /// </summary>
        public static bool OpponentHasThreateningMonster(ClientField enemy, int atkThreshold = 2500)
        {
            return enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && !c.IsDisabled() &&
                (c.IsFloodgate() || CardIntelligence.IsFloodgate(c.Id) || c.Attack >= atkThreshold || c.HasType(CardType.Link) || c.HasType(CardType.Synchro)));
        }

        /// <summary>
        /// Checks if the opponent controls a negate monster.
        /// Consolidated from 8+ executor files.
        /// </summary>
        public static bool EnemyHasKnownNegate(ClientField enemy)
        {
            return enemy.GetMonsters().Any(c => c != null
                && c.IsFaceup()
                && !c.IsDisabled()
                && (NegateMonsterIds.Contains(c.Id) || CardIntelligence.IsKnownNegator(c.Id)));
        }

        /// <summary>
        /// Checks if the opponent has a once-per-turn spell negate (for baiting logic).
        /// Consolidated from Branded executor.
        /// </summary>
        public static bool EnemyHasOncePerTurnSpellNegator(ClientField enemy)
        {
            return enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled() && SpellNegatorIds.Contains(c.Id));
        }

        /// <summary>
        /// Gets the best preemptive Impermanence target from opponent's field.
        /// Consolidated from multiple executors.
        /// Returns null if no valid target exists.
        /// </summary>
        public static ClientCard GetPreemptiveImpermTarget(ClientField enemy)
        {
            return enemy.GetMonsters().FirstOrDefault(c =>
                c != null && c.IsFaceup() && !c.IsDisabled() &&
                PreemptiveImpermTargetIds.Contains(c.Id) &&
                !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget());
        }

        // ═══════════════════════════════════════════════════════════════
        //  BOARD STATE EVALUATION
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Checks if the bot is in a resource-poor "grind game" state.
        /// Consolidated from 12+ executor files.
        /// 
        /// Threshold variant: use thresholds to adjust sensitivity.
        /// </summary>
        public static bool IsInGrindGame(ClientField bot, int handThreshold = 1, int monsterThreshold = 1)
        {
            return bot.GetHandCount() <= handThreshold && bot.GetMonsterCount() <= monsterThreshold;
        }

        /// <summary>
        /// Checks if the bot needs to establish board presence urgently.
        /// Consolidated from 12+ executor files.
        /// </summary>
        public static bool NeedsBoardPresence(ClientField bot, ClientField enemy)
        {
            int ourFaceup = bot.MonsterZone.Count(c => c != null && c.IsFaceup());
            int enemyMonsters = enemy.GetMonsterCount();
            return ourFaceup == 0 || (ourFaceup <= 1 && enemyMonsters >= 2);
        }

        /// <summary>
        /// Checks if the bot can deal lethal damage this turn.
        /// Consolidated from 12+ executor files.
        /// </summary>
        public static bool CanDealLethal(ClientField bot, ClientField enemy, BoardScorer scorer = null)
        {
            if (scorer != null && scorer.HasLethal()) return true;
            int totalATK = 0;
            foreach (var m in bot.GetMonsters())
                if (m != null && m.IsFaceup() && m.IsAttack() && !m.Attacked)
                    totalATK += m.Attack;
            if (enemy.GetMonsterCount() == 0 && totalATK >= enemy.LifePoints) return true;
            return false;
        }

        /// <summary>
        /// Checks if the bot should skip combo plays and go straight to battle (lethal available).
        /// Consolidated from 12+ executor files.
        /// </summary>
        public static bool ShouldSkipCombo(ClientField bot, ClientField enemy, Duel duel, BoardScorer scorer = null)
        {
            return duel.Phase == DuelPhase.Main1 && CanDealLethal(bot, enemy, scorer);
        }

        // ═══════════════════════════════════════════════════════════════
        //  COMBAT SAFETY
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Checks if an attacker can safely attack into the opponent's board.
        /// Consolidated from 10+ executor files.
        /// 
        /// Extra threat IDs can be provided for deck-specific immunity checks (e.g., Avramax, Crystal Wing).
        /// </summary>
        public static bool IsSafeToAttack(ClientCard attacker, ClientField enemy, params int[] extraThreatIds)
        {
            if (attacker == null) return false;
            foreach (ClientCard enemyCard in enemy.MonsterZone)
            {
                if (enemyCard == null || !enemyCard.IsFaceup()) continue;
                if (!enemyCard.IsDisabled())
                {
                    // Avramax: cannot be targeted by special summoned monster's attack
                    if (enemyCard.Id == 21887175 && attacker.IsSpecialSummoned) return false;
                    // Crystal Wing: destroys monsters Level 5+
                    if (enemyCard.Id == 50954680 && attacker.Level >= 5) return false;
                    // Custom extra threats
                    if (extraThreatIds.Length > 0 && extraThreatIds.Contains(enemyCard.Id)) return false;
                }
                if (enemyCard.IsAttack())
                {
                    if (enemyCard.Attack > attacker.Attack) return false;
                }
                if (enemyCard.IsDefense() && attacker.Attack <= enemyCard.Defense) return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if a monster in defense position is safe from opponent attacks.
        /// Consolidated from 10+ executor files.
        /// </summary>
        public static bool IsSafeToDefend(ClientCard monster, ClientField enemy)
        {
            if (monster == null) return false;
            foreach (ClientCard enemyCard in enemy.MonsterZone)
            {
                if (enemyCard == null || !enemyCard.IsFaceup()) continue;
                if (enemyCard.Attack > monster.Defense) return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if a card is an enemy threat worth targeting for destruction/banish.
        /// Used by Super Poly, board breakers, and removal effects.
        /// Consolidated from multiple executors.
        /// </summary>
        public static bool IsEnemyCardAThreat(ClientCard card, ClientField bot)
        {
            if (card == null || !card.IsFaceup()) return false;
            if (card.IsSpell() || card.IsTrap())
            {
                // Any face-up spell/trap is potentially threatening
                if (bot.GetMonsterCount() > 0 || bot.GetSpellCount() > 0) return true;
            }
            if (card.IsMonster())
            {
                // Extra deck monsters and high-ATK monsters are always threats
                if (card.IsExtraCard() || card.Attack >= 2000) return true;
                // Floodgates and effect monsters with 1500+ ATK are threats
                if (card.IsFloodgate() || card.Attack >= 1500) return true;
                return false;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  UTILITY — ACE CARD / MATERIAL PROTECTION
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Filters a card list to exclude Ace cards, preferring non-Ace cards for materials/tributes.
        /// Useful in OnSelectCard for hint-based selections.
        /// Consolidated from multiple executors.
        /// </summary>
        public static IList<ClientCard> FilterAceCards(IList<ClientCard> cards, Func<ClientCard, bool> isAceCard, int min)
        {
            var safe = cards.Where(c => c != null && !isAceCard(c)).ToList();
            if (safe.Count >= min) return safe;
            return cards.ToList();
        }

        /// <summary>
        /// Selects preferred cards by ID priority from a list.
        /// Consolidated from 12+ executor files.
        /// </summary>
        public static IList<ClientCard> SelectPreferred(IList<ClientCard> cards, int min, int max, params int[] preferredIds)
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
    }
}
