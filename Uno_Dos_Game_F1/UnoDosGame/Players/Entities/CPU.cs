using UnoDos.Cards.Entities;
using UnoDos.Cards.Enums;
using UnoDos.Decks.Entities;
 
namespace UnoDos.Players.Entities
{
    public class CPU : Player
    {
        //Method to generate a list of the cards the CPU could play
        private List<Card> PossibleCards(Card shownCard)
        {
            List<Card> __PlayableCards;
            //For each card the CPU has
            foreach (Card _Card in Cards)
            {
                //Calls the parent Player().CanPLayCard() method
                if (CPU.CanPlayCard())
                {
                    //And adds card to list if playable
                    __PlayableCards.add(__Card);
                }
            }
            return __PlayableCards;
        }

        //Method for CPU to play a card after the list of playable cards has been generated
        public override Deck PlayCard(List<Card> playableCards, Card shownCard, Deck currentDeck)
        {
            //Select a random playable card
            Random rnd = new Random();
            int index = rnd.Next(playableCards.Length);
            Card playedCard = playableCards.get(index);
            
            //Remove card from hand and play it on deck
            Cards.Remove(playedCard);
            currentDeck.PlayedCards.Add(playedCard);
            
            return currentDeck;
        }

    }
}