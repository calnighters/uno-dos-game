using UnoDos.Cards.Interfaces;

namespace UnoDos.Decks.Interfaces
{
    public interface IDeck
    {
        List<ICard> DeckOfCards { get; }
        ICard LastCardPlayed { get; }
        List<ICard> PlayedCards { get; }

        void CreateDeck();
        ICard DrawCard();
        List<ICard> DrawCards(int count);
        ICard DrawInitialCard();
        void Shuffle();
    }
}