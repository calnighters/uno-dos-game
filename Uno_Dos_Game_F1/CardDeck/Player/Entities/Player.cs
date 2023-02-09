using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoDos.Cards.Entities;
using UnoDos.Cards.Enums;
using UnoDos.Decks.Entities;

namespace UnoDos.Player.Entities
{
    public class Player
    {
        private const int DRAW_CARD_ONE = 1;

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
                    if (playedCard.Colour == shownCard.Colour
                        && (playedCard.CardScore - shownCard.CardScore == -1
                            || playedCard.CardScore - shownCard.CardScore == 1))
                    {
                        return true;
                    }
                    break;
                case CardType.LoseTwo:
                case CardType.SwapDeck:
                case CardType.Switch:
                    if (playedCard.Colour == shownCard.Colour)
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }

        public Deck DrawCard(Deck currentDeck)
        {
            Cards.Add(currentDeck.DrawCards(DRAW_CARD_ONE).SingleOrDefault());
            return currentDeck;
        }

        public Card PlayCard(Card playedCard, Card shownCard)
        {
            if (CanPlayCard(playedCard, shownCard))
            {
                Cards.Remove(playedCard);
                return playedCard;
            }
            return new Card();
        }

        public List<string> ViewCards()
        {
            List<string> _CardToString = new List<string>();
            Cards.ForEach(card => _CardToString.Add(card.toString()));
            return _CardToString;
        }

        public List<Card> Cards { get; set; }
        public string PlayerName { get; set; }


    }
}
