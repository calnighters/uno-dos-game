using UnoDos.Cards.Enums;
using UnoDos.Cards.Interfaces;

namespace UnoDos.Cards.Entities
{
    public class Card : ICard
    {
        public Card CreateCard(int cardID, CardColour colour, CardType cardType, int cardScore)
        {
            return new() { CardID = cardID, Colour = colour, TypeOfCard = cardType, CardScore = cardScore };
        }

        public int CardID { get; set; }

        public int CardScore { get; set; }
        public CardColour Colour { get; set; }
        public CardType TypeOfCard { get; set; }

        // add an override toString method
        public string toString()
        {
            return Colour + " " + TypeOfCard + " " + CardScore;
        }
    }
}