using UnoDos.Cards.Interfaces;
using UnoDos.Decks.Interfaces;
using UnoDos.Players.Interfaces;

namespace UnoDos.Players.Entities
{
    public class CPU : Player, ICPU
    {
        public IDeck LoseTwoCards(IDeck currentDeck)
        {
            if (Cards.Count < 2)
            {
                currentDeck.DeckOfCards.Add(Cards[0]);
                Cards = new List<ICard>();
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    Random _Random = new Random();
                    int _Index = _Random.Next(Cards.Count());
                    ICard _CardToLose = Cards[_Index];
                    Cards.Remove(_CardToLose);
                    currentDeck.DeckOfCards.Add(_CardToLose);
                }
            }
            IsLoseTwoCardPlayed = false;
            return currentDeck;
        }

        //Method for CPU to play a card after the list of playable cards has been generated
        public IDeck PlayCardCPU(IDeck currentDeck)
        {
            PossibleCards(currentDeck.LastCardPlayed);

            if (PlayableCards.Count > 0)
            {
                //Select a random playable card
                Random _Random = new Random();
                int _Index = _Random.Next(PlayableCards.Count());
                ICard _PlayedCard = PlayableCards[_Index];
                HasCPUPlayedCard = true;
                //Remove card from hand and play it on deck
                return PlayCard(_PlayedCard, currentDeck);
            }
            else
            {
                DrawCard(currentDeck);
                HasCPUPlayedCard = false;
            }

            return currentDeck;
        }

        //Method to generate a list of the cards the CPU could play
        private List<ICard> PossibleCards(ICard shownCard)
        {
            PlayableCards = new List<ICard>();
            //For each card the CPU has
            foreach (ICard _Card in Cards)
            {
                //Calls the parent Player().CanPLayCard() method
                if (CanPlayCard(_Card, shownCard))
                {
                    //And adds card to list if playable
                    PlayableCards.Add(_Card);
                }
            }
            return PlayableCards;
        }

        public bool HasCPUPlayedCard { get; private set; }
        public List<ICard> PlayableCards { get; private set; }
    }
}