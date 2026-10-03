using System.Collections.Generic;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI
{
    public class CardSelector
    {
        private enum SelectType
        {
            Card,
            Cards,
            Id,
            Ids,
            Location
        }

        private SelectType _type;
        private ClientCard _card;
        private IList<ClientCard> _cards;
        private int _id;
        private IList<int> _ids;
        private CardLocation _location;

        public CardSelector(ClientCard card)
        {
            _type = SelectType.Card;
            _card = card;
        }

        public CardSelector(IList<ClientCard> cards)
        {
            _type = SelectType.Cards;
            _cards = cards;
        }

        public CardSelector(int cardId)
        {
            _type = SelectType.Id;
            _id = cardId;
        }

        public CardSelector(IList<int> ids)
        {
            _type = SelectType.Ids;
            _ids = ids;
        }

        public CardSelector(CardLocation location)
        {
            _type = SelectType.Location;
            _location = location;
        }

        /// <summary>
        /// [Core v0.094] True when the preselection names explicit ClientCard instances
        /// (AI.SelectCard(card) / AI.SelectCard(cards)) instead of IDs or a location.
        /// </summary>
        public bool IsExplicitCardSelection => _type == SelectType.Card || _type == SelectType.Cards;

        /// <summary>
        /// [Core v0.094] Return ONLY the cards from <paramref name="cards"/> that genuinely match this
        /// preselection — no padding, no trimming. Used by ModernExecutor to decide whether a queued
        /// AI.SelectCard(...) belongs to the current prompt before consuming it.
        /// </summary>
        public IList<ClientCard> Match(IList<ClientCard> cards)
        {
            var result = new List<ClientCard>();
            if (cards == null) return result;

            switch (_type)
            {
                case SelectType.Card:
                    if (_card != null && cards.Contains(_card))
                        result.Add(_card);
                    break;
                case SelectType.Cards:
                    if (_cards != null)
                        foreach (ClientCard card in _cards)
                            if (card != null && cards.Contains(card) && !result.Contains(card))
                                result.Add(card);
                    break;
                case SelectType.Id:
                    foreach (ClientCard card in cards)
                        if (card != null && card.IsCode(_id))
                            result.Add(card);
                    break;
                case SelectType.Ids:
                    if (_ids != null)
                        foreach (int id in _ids)
                            foreach (ClientCard card in cards)
                                if (card != null && card.IsCode(id) && !result.Contains(card))
                                    result.Add(card);
                    break;
                case SelectType.Location:
                    foreach (ClientCard card in cards)
                        if (card != null && card.Location == _location)
                            result.Add(card);
                    break;
            }

            return result;
        }

        public IList<ClientCard> Select(IList<ClientCard> cards, int min, int max)
        {
            IList<ClientCard> result = new List<ClientCard>();

            switch (_type)
            {
                case SelectType.Card:
                    if (cards.Contains(_card))
                        result.Add(_card);
                    break;
                case SelectType.Cards:
                    foreach (ClientCard card in _cards)
                        if (cards.Contains(card) && !result.Contains(card))
                            result.Add(card);
                    break;
                case SelectType.Id:
                    foreach (ClientCard card in cards)
                        if (card.IsCode(_id))
                            result.Add(card);
                    break;
                case SelectType.Ids:
                    foreach (int id in _ids)
                        foreach (ClientCard card in cards)
                            if (card.IsCode(id) && !result.Contains(card))
                                result.Add(card);
                    break;
                case SelectType.Location:
                    foreach (ClientCard card in cards)
                        if (card.Location == _location)
                            result.Add(card);
                    break;
            }

            if (result.Count < min)
            {
                foreach (ClientCard card in cards)
                {
                    if (!result.Contains(card))
                        result.Add(card);
                    if (result.Count >= min)
                        break;
                }
            }

            while (result.Count > max)
                result.RemoveAt(result.Count - 1);

            return result;
        }
    }
}