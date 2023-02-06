using UnoDos.Cards.Entities;
using UnoDos.Cards.Enums;

namespace UnoDos.Cards.Interfaces
{
    public interface ICard
    {
        int CardID { get; set; }
        int CardScore { get; set; }

        Card CreateCard(int cardID, CardColour colour, CardType cardType, int cardScore);
        string toString();
    }
}