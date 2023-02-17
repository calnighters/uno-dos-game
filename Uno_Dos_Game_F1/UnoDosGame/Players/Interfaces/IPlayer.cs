using UnoDos.Cards.Entities;
using UnoDos.Cards.Interfaces;
using UnoDos.Decks.Entities;

namespace UnoDos.Players.Interfaces
{
    public interface IPlayer
    {
        List<ICard> Cards { get; set; }
        List<string> Errors { get; }
        string PlayerName { get; set; }

        Deck DrawCard(Deck currentDeck);
        Deck PlayCard(ICard playedCard, Deck currentDeck);
        List<string> ViewCards();
    }
}