using UnoDos.Cards.Interfaces;

namespace UnoDos.Decks.Interfaces
{
    public interface IDeck
    {
        ICard CardCreator { get; }
        List<ICard> DeckOfCards { get; }
        List<ICard> PlayedCards { get; }

        void CreateDeck();
        ICard DrawCard();
        List<ICard> DrawCards(int count);
        ICard DrawInitialCard();
        void Reshuffle();
        void Shuffle();
    }
}