using UnoDos.Cards.Entities;
using UnoDos.Cards.Enums;
using UnoDos.Decks.Entities;

namespace UnoDos.Players.Entities
{
    public class Player
    {
        private const int DRAW_CARD_ONE = 1;
        private const string INVALID_COLOUR_ERROR = "Card colour {0} is an invalid card! Please play a card with this colour: {1}";
        private const string INVALID_NUMBER_ERROR = "Card number {0} is an invalid card! Please play a card with a value of +1 or -1 of the last card value, which is {1}!";

        private List<string> __Errors;

        private bool CanPlayCard(Card playedCard, Card shownCard)
        {
            if (playedCard.Colour == CardColour.Wild)
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
                    if (playedCard.Colour != shownCard.Colour)
                    {
                        Errors.Add(string.Format(INVALID_COLOUR_ERROR, playedCard.Colour.ToString(), shownCard.Colour.ToString()));
                        return false;
                    }
                    if(playedCard.CardScore - shownCard.CardScore != -1 && playedCard.CardScore - shownCard.CardScore != 1)
                    {
                        Errors.Add(string.Format(INVALID_NUMBER_ERROR, playedCard.CardScore.ToString(), shownCard.CardScore.ToString()));
                        return false;
                    }
                    return true;
                case CardType.LoseTwo:
                case CardType.SwapDeck:
                case CardType.Switch:
                    if (playedCard.Colour != shownCard.Colour)
                    {
                        Errors.Add(string.Format(INVALID_COLOUR_ERROR, playedCard.Colour.ToString(), shownCard.Colour.ToString()));
                        return false;
                    }
                    return true;
            }

            return false;
        }

        public Deck DrawCard(Deck currentDeck)
        {
            Cards.Add(currentDeck.DrawCards(DRAW_CARD_ONE).SingleOrDefault());
            return currentDeck;
        }

        public Deck PlayCard(Card playedCard, Card shownCard, Deck currentDeck)
        {
            if (CanPlayCard(playedCard, shownCard))
            {
                Cards.Remove(playedCard);
                currentDeck.PlayedCards.Add(playedCard);
            }
            return currentDeck;
        }

        public List<string> ViewCards()
        {
            List<string> _CardToString = new List<string>();
            Cards.ForEach(card => _CardToString.Add(card.toString()));
            return _CardToString;
        }

        public List<Card> Cards { get; set; }
        public List<string> Errors => __Errors = __Errors ?? new List<string>();
        public string PlayerName { get; set; }
    }
}
