using System;
using System.Collections.Generic;
using System.Linq;
using WindBot.Game;
using YgoAiPlatform.Core;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    /// <summary>
    /// Maps WindBot's ClientCard/ClientField objects to the core platform's
    /// GameState, BoardState, OpponentState, and DatasetSample models.
    /// </summary>
    public static class ClientCardMapper
    {
        /// <summary>
        /// Convert a single ClientCard to a structured CardInfo.
        /// </summary>
        public static CardInfo ToCardInfo(ClientCard card)
        {
            if (card == null)
            {
                return new CardInfo();
            }

            return new CardInfo
            {
                Id = card.Id,
                Name = card.Name ?? string.Empty,
                Attack = card.Attack,
                Defense = card.Defense,
                Level = card.Level > 0 ? card.Level : card.Rank,
                CardType = card.Type,
                Position = FormatPosition(card.Position),
                Location = card.Location.ToString(),
                LinkMarkers = card.LinkMarker,
                Counters = card.Overlays != null ? new List<int>(card.Overlays) : new List<int>()
            };
        }

        /// <summary>
        /// Build a BoardState from the bot player's ClientField.
        /// </summary>
        public static BoardState BuildBoardState(ClientField player)
        {
            var monsters = player.GetMonsters();
            var spells = player.GetSpells();
            var fieldCards = new List<ClientCard>();
            fieldCards.AddRange(monsters);
            fieldCards.AddRange(spells);

            var board = new BoardState
            {
                // String lists for backward compatibility
                Hand = player.Hand
                    .Where(c => c != null)
                    .Select(c => c.Name ?? $"id:{c.Id}")
                    .ToList(),
                Field = fieldCards
                    .Select(c => c.Name ?? $"id:{c.Id}")
                    .ToList(),
                Graveyard = player.Graveyard
                    .Where(c => c != null)
                    .Select(c => c.Name ?? $"id:{c.Id}")
                    .ToList(),
                Banished = player.Banished
                    .Where(c => c != null)
                    .Select(c => c.Name ?? $"id:{c.Id}")
                    .ToList(),
                Lp = player.LifePoints,

                // Structured CardInfo lists
                HandCards = player.Hand
                    .Where(c => c != null)
                    .Select(ToCardInfo)
                    .ToList(),
                FieldCards = fieldCards
                    .Select(ToCardInfo)
                    .ToList(),
                GraveyardCards = player.Graveyard
                    .Where(c => c != null)
                    .Select(ToCardInfo)
                    .ToList(),
                BanishedCards = player.Banished
                    .Where(c => c != null)
                    .Select(ToCardInfo)
                    .ToList(),
                ExtraDeckCards = player.ExtraDeck
                    .Where(c => c != null)
                    .Select(ToCardInfo)
                    .ToList()
            };

            return board;
        }

        /// <summary>
        /// <summary>
        /// Build an OpponentState from the enemy's ClientField.
        /// </summary>
        public static OpponentState BuildOpponentState(ClientField enemy, bool opponentChokepointActive = false)
        {
            var monsters = enemy.GetMonsters();
            var spells = enemy.GetSpells();
            var fieldCards = new List<ClientCard>();
            fieldCards.AddRange(monsters);
            fieldCards.AddRange(spells);

            return new OpponentState
            {
                HandCount = enemy.Hand.Count(c => c != null),
                Field = fieldCards
                    .Select(c => c.IsFacedown() ? "?Set" : (c.Name ?? $"id:{c.Id}"))
                    .ToList(),
                FieldCards = fieldCards
                    .Select(ToCardInfo)
                    .ToList(),
                Graveyard = enemy.Graveyard
                    .Where(c => c != null)
                    .Select(c => c.Name ?? $"id:{c.Id}")
                    .ToList(),
                GraveyardCards = enemy.Graveyard
                    .Where(c => c != null)
                    .Select(ToCardInfo)
                    .ToList(),
                BanishedCards = enemy.Banished
                    .Where(c => c != null)
                    .Select(ToCardInfo)
                    .ToList(),
                Lp = enemy.LifePoints,
                OpponentChokepointActive = opponentChokepointActive
            };
        }

        /// <summary>
        /// Build a complete GameState from bot and enemy fields.
        /// </summary>
        public static GameState BuildGameState(ClientField bot, ClientField enemy)
        {
            var botMonsters = bot.GetMonsters();
            var botSpells = bot.GetSpells();
            var botFieldCards = new List<ClientCard>();
            botFieldCards.AddRange(botMonsters);
            botFieldCards.AddRange(botSpells);

            var enemyMonsters = enemy.GetMonsters();
            var enemySpells = enemy.GetSpells();
            var enemyFieldCards = new List<ClientCard>();
            enemyFieldCards.AddRange(enemyMonsters);
            enemyFieldCards.AddRange(enemySpells);

            return new GameState
            {
                Hand = bot.Hand
                    .Where(c => c != null)
                    .Select(c => c.Name ?? $"id:{c.Id}")
                    .ToList(),
                Field = botFieldCards
                    .Select(c => c.Name ?? $"id:{c.Id}")
                    .ToList(),
                Graveyard = bot.Graveyard
                    .Where(c => c != null)
                    .Select(c => c.Name ?? $"id:{c.Id}")
                    .ToList(),
                Banished = bot.Banished
                    .Where(c => c != null)
                    .Select(c => c.Name ?? $"id:{c.Id}")
                    .ToList(),
                LifePoints = bot.LifePoints,
                OpponentLifePoints = enemy.LifePoints,
                OpponentHandCount = enemy.Hand.Count(c => c != null),
                OpponentField = enemyFieldCards
                    .Select(c => c.IsFacedown() ? "?Set" : (c.Name ?? $"id:{c.Id}"))
                    .ToList()
            };
        }

        /// <summary>
        /// Build a complete DatasetSample with real game data from a decision point.
        /// </summary>
        public static DatasetSample BuildDatasetSample(
            string duelId, int turn, string phase,
            ClientField bot, ClientField enemy,
            List<(string ActionName, string CardName, double Score)> candidates,
            string chosenAction, string chosenCardName, double chosenScore,
            List<string> reasoning,
            bool opponentChokepointActive = false)
        {
            var boardState = BuildBoardState(bot);
            var opponentState = BuildOpponentState(enemy, opponentChokepointActive);

            var legalActions = candidates
                .Select(c => $"{NormalizeActionName(c.ActionName)}:{c.CardName}")
                .ToList();

            chosenAction = NormalizeActionName(chosenAction);

            return new DatasetSample
            {
                DuelId = duelId,
                Turn = turn,
                Phase = phase,
                BoardState = boardState,
                OpponentState = opponentState,
                LegalActions = legalActions,
                ChosenAction = $"{chosenAction}:{chosenCardName}",
                ActionReason = reasoning ?? new List<string>(),
                BoardScore = chosenScore,
                Reward = 0.0,
                Result = null
            };
        }

        private static string NormalizeActionName(string actionName)
        {
            if (string.Equals(actionName, "SetMonster", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(actionName, "SetSpell", StringComparison.OrdinalIgnoreCase))
            {
                return "Set";
            }

            return actionName;
        }

        /// <summary>
        /// Convert the integer Position bitmask to a human-readable string.
        /// </summary>
        private static string FormatPosition(int position)
        {
            // CardPosition values are bitmask flags
            const int FaceUpAttack = 0x1;
            const int FaceDownAttack = 0x2;
            const int FaceUpDefence = 0x4;
            const int FaceDownDefence = 0x8;

            if ((position & FaceUpAttack) != 0) return "FaceUpAttack";
            if ((position & FaceDownAttack) != 0) return "FaceDownAttack";
            if ((position & FaceUpDefence) != 0) return "FaceUpDefence";
            if ((position & FaceDownDefence) != 0) return "FaceDownDefence";
            return "Unknown";
        }
    }
}
