using UnoDos.Cards.Entities;
using UnoDos.Cards.Enums;
using UnoDos.Cards.Interfaces;

namespace UnoDos.Decks.Entities
{
    public class Deck
    {
        private const int SPECIAL_CARD_VALUE = 20;
        private const int WILDCARD_VALUE = 50;

        public Deck() { }

        private ICard __Card;
        private List<Card> __DeckOfCards;

        private void AddColourCards(CardColour colour)
        {
            DeckOfCards.Add(Card.CreateCard(DeckOfCards.Count + 1, colour, CardType.One, Convert.ToInt32(CardType.One)));
            DeckOfCards.Add(Card.CreateCard(DeckOfCards.Count + 1, colour, CardType.Two, Convert.ToInt32(CardType.Two)));
            DeckOfCards.Add(Card.CreateCard(DeckOfCards.Count + 1, colour, CardType.Three, Convert.ToInt32(CardType.Three)));
            DeckOfCards.Add(Card.CreateCard(DeckOfCards.Count + 1, colour, CardType.Four, Convert.ToInt32(CardType.Four)));
            DeckOfCards.Add(Card.CreateCard(DeckOfCards.Count + 1, colour, CardType.Five, Convert.ToInt32(CardType.Five)));
            DeckOfCards.Add(Card.CreateCard(DeckOfCards.Count + 1, colour, CardType.Six, Convert.ToInt32(CardType.Six)));
            DeckOfCards.Add(Card.CreateCard(DeckOfCards.Count + 1, colour, CardType.Seven, Convert.ToInt32(CardType.Seven)));
            DeckOfCards.Add(Card.CreateCard(DeckOfCards.Count + 1, colour, CardType.Eight, Convert.ToInt32(CardType.Eight)));
            DeckOfCards.Add(Card.CreateCard(DeckOfCards.Count + 1, colour, CardType.Nine, Convert.ToInt32(CardType.Nine)));

            DeckOfCards.Add(Card.CreateCard(DeckOfCards.Count + 1, colour, CardType.Switch, SPECIAL_CARD_VALUE));
            DeckOfCards.Add(Card.CreateCard(DeckOfCards.Count + 1, colour, CardType.LoseTwo, SPECIAL_CARD_VALUE));
            DeckOfCards.Add(Card.CreateCard(DeckOfCards.Count + 1, colour, CardType.SwapDeck, SPECIAL_CARD_VALUE));
        }

        private void AddWildCard()
        {
            DeckOfCards.Add(Card.CreateCard(DeckOfCards.Count + 1, CardColour.Wild, CardType.WildCard, WILDCARD_VALUE));
        }

        public void CreateDeck()
        {

            foreach (CardColour _Colour in Enum.GetValues(typeof(CardColour)))
            {
                switch (_Colour)
                {
                    case CardColour.Orange:
                    case CardColour.Pink:
                    case CardColour.Green:
                    case CardColour.Purple:
                        DeckOfCards.Add(Card.CreateCard(DeckOfCards.Count + 1, _Colour, CardType.Zero, Convert.ToInt32(CardType.Zero)));
                        AddColourCards(_Colour);
                        AddColourCards(_Colour);
                        break;
                    case CardColour.Wild:
                        AddWildCard();
                        AddWildCard();
                        AddWildCard();
                        AddWildCard();
                        AddWildCard();
                        break;
                }
            }
        }

        // Draw Card Method
        public List<Card> DrawCards(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("count", "count must be greater than 0");
            }

            if (count > DeckOfCards.Count)
            {
                throw new InvalidOperationException("Not enough cards in deck");
            }

            List<Card> drawnCards = DeckOfCards.Take(count).ToList();

            //Remove the drawn cards from the draw pile
            DeckOfCards.RemoveAll(card => drawnCards.Contains(card));
            return drawnCards;

        }

        // Shuffle Card Method
        public void Shuffle()
        {
            Random random = new Random();

            List<Card> cards = DeckOfCards;

            for (int i = cards.Count - 1; i >= 0; --i)// may need to check this for condition
            {
                int j = random.Next(i + 1);
                Card temp = cards[i];
                cards[i] = cards[j];
                cards[j] = temp;

            }

        }

        public ICard Card => __Card = __Card ?? new Card();
        public List<Card> DeckOfCards => __DeckOfCards = __DeckOfCards ?? new();
    }
}
