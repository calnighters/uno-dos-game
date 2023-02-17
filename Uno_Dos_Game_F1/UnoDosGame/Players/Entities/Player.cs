using UnoDos.Cards.Entities;
using UnoDos.Cards.Enums;
using UnoDos.Cards.Interfaces;
using UnoDos.Decks.Interfaces;
using UnoDos.Players.Interfaces;

namespace UnoDos.Players.Entities
{
    public class Player : IPlayer
    {
        private const int DRAW_CARD_ONE = 1;
        private const string INVALID_COLOUR_ERROR = "Card colour {0} is an invalid card! Please play a card with this colour: {1}";
        private const string INVALID_NUMBER_ERROR = "Card number {0} is an invalid card! Please play a card with a value of +1 or -1 of the last card value, which is {1}!";

        private List<string> __Errors;

        public bool CanPlayCard(ICard playedCard, ICard shownCard)
        {
            bool _IsShownCardSpecial = Card.SpecialCards.Any(specialCard => specialCard == shownCard.TypeOfCard);
            bool _IsColourValid = playedCard.Colour == shownCard.Colour;

            if (playedCard.Colour == CardColour.SeeThrough)
            {
                return true;
            }

            switch (playedCard.TypeOfCard)
            {
                case CardType.Zero:
                case CardType.One:
                case CardType.Two:
                case CardType.Three:
                case CardType.Four:
                case CardType.Five:
                case CardType.Six:
                case CardType.Seven:
                case CardType.Eight:
                case CardType.Nine:
                    if (!_IsColourValid && _IsShownCardSpecial)
                    {
                        Errors.Add(string.Format(INVALID_COLOUR_ERROR, playedCard.Colour.ToString(), shownCard.Colour.ToString()));
                        return false;
                    }
                    if (playedCard.CardScore - shownCard.CardScore != -1
                        && playedCard.CardScore - shownCard.CardScore != 1
                        && !_IsShownCardSpecial)
                    {
                        Errors.Add(string.Format(INVALID_NUMBER_ERROR, playedCard.CardScore.ToString(), shownCard.CardScore.ToString()));
                        return false;
                    }
                    return true;
                case CardType.LoseTwo:
                case CardType.SwapDeck:
                case CardType.Reset:
                    if (!_IsColourValid && playedCard.TypeOfCard != shownCard.TypeOfCard)
                    {
                        Errors.Add(string.Format(INVALID_COLOUR_ERROR, playedCard.Colour.ToString(), shownCard.Colour.ToString()));
                        return false;
                    }
                    return true;
            }

            return false;
        }

        public IDeck DrawCard(IDeck currentDeck)
        {
            Cards.Add(currentDeck.DrawCards(DRAW_CARD_ONE).SingleOrDefault());
            return currentDeck;
        }

        public IDeck LoseTwoCards(List<ICard> cardsToRemove, IDeck currentDeck)
        {
            Cards.RemoveAll(card => cardsToRemove.Contains(card));
            currentDeck.DeckOfCards.AddRange(cardsToRemove);
            IsLoseTwoCardPlayed = false;
            return currentDeck;
        }

        public IDeck PlayCard(ICard playedCard, IDeck currentDeck)
        {
            ICard _ShownCard = currentDeck.LastCardPlayed;

            if (CanPlayCard(playedCard, _ShownCard))
            {
                switch (playedCard.TypeOfCard)
                {
                    case CardType.SeeThrough:
                        playedCard.Colour = _ShownCard.Colour;
                        break;
                    case CardType.LoseTwo:
                        IsLoseTwoCardPlayed = true;
                        break;
                    case CardType.SwapDeck:
                        IsSwapDeckPlayed = true;
                        break;
                    case CardType.Reset:
                        IsResetCardPlayed = true;
                        break;
                }

                if (playedCard.TypeOfCard == CardType.SeeThrough)
                {
                    playedCard.Colour = _ShownCard.Colour;
                }
                Cards.Remove(playedCard);
                currentDeck.PlayedCards.Add(playedCard);
            }
            return currentDeck;
        }

        public KeyValuePair<List<ICard>, List<ICard>> SwapCards(KeyValuePair<List<ICard>, List<ICard>> unswappedCards)
        {
            KeyValuePair<List<ICard>, List<ICard>> _SwappedCards = new KeyValuePair<List<ICard>, List<ICard>>(unswappedCards.Value, unswappedCards.Key);
            IsSwapDeckPlayed = false;
            return _SwappedCards;
        }

        public List<string> ViewCards()
        {
            List<string> _CardToString = new List<string>();
            Cards.ForEach(card => _CardToString.Add(card.ToString()));
            return _CardToString;
        }

        public List<ICard> Cards { get; set; }
        public List<string> Errors => __Errors = __Errors ?? new List<string>();
        public bool IsLoseTwoCardPlayed { get; protected set; }
        public bool IsResetCardPlayed { get; set; }
        public bool IsSwapDeckPlayed { get; set; }
        public string PlayerName { get; set; }
    }
}
