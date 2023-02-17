using UnoDos.Cards.Interfaces;
using UnoDos.Decks.Interfaces;

namespace UnoDos.Players.Interfaces
{
    public interface ICPU : IPlayer
    {
        bool HasCPUPlayedCard { get; }
        List<ICard> PlayableCards { get; }

        IDeck LoseTwoCards(IDeck currentDeck);
        IDeck PlayCardCPU(IDeck currentDeck, ICard shownCard);
    }
}