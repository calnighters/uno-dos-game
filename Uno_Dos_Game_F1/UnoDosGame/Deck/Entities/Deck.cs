using UnoDos.Cards.Entities;
using UnoDos.Cards.Enums;
using UnoDos.Cards.Interfaces;

namespace UnoDos.Decks.Entities
{
    public class Deck
    {
        private const int DRAW_FACE_CARD_AMOUNT = 1;
        private const int SPECIAL_CARD_VALUE = 20;
        private const int WILDCARD_VALUE = 50;

        public Deck() { }

        private ICard __CardCreator;
        private List<ICard> __DeckOfCards;
        private List<ICard> __PlayedCards;

        private void AddColourCards(CardColour colour)
        {
            DeckOfCards.Add(CardCreator.CreateCard(DeckOfCards.Count + 1, colour, CardType.One, Convert.ToInt32(CardType.One)));
            DeckOfCards.Add(CardCreator.CreateCard(DeckOfCards.Count + 1, colour, CardType.Two, Convert.ToInt32(CardType.Two)));
            DeckOfCards.Add(CardCreator.CreateCard(DeckOfCards.Count + 1, colour, CardType.Three, Convert.ToInt32(CardType.Three)));
            DeckOfCards.Add(CardCreator.CreateCard(DeckOfCards.Count + 1, colour, CardType.Four, Convert.ToInt32(CardType.Four)));
            DeckOfCards.Add(CardCreator.CreateCard(DeckOfCards.Count + 1, colour, CardType.Five, Convert.ToInt32(CardType.Five)));
            DeckOfCards.Add(CardCreator.CreateCard(DeckOfCards.Count + 1, colour, CardType.Six, Convert.ToInt32(CardType.Six)));
            DeckOfCards.Add(CardCreator.CreateCard(DeckOfCards.Count + 1, colour, CardType.Seven, Convert.ToInt32(CardType.Seven)));
            DeckOfCards.Add(CardCreator.CreateCard(DeckOfCards.Count + 1, colour, CardType.Eight, Convert.ToInt32(CardType.Eight)));
            DeckOfCards.Add(CardCreator.CreateCard(DeckOfCards.Count + 1, colour, CardType.Nine, Convert.ToInt32(CardType.Nine)));

            DeckOfCards.Add(CardCreator.CreateCard(DeckOfCards.Count + 1, colour, CardType.Reset, SPECIAL_CARD_VALUE));
            DeckOfCards.Add(CardCreator.CreateCard(DeckOfCards.Count + 1, colour, CardType.LoseTwo, SPECIAL_CARD_VALUE));
            DeckOfCards.Add(CardCreator.CreateCard(DeckOfCards.Count + 1, colour, CardType.SwapDeck, SPECIAL_CARD_VALUE));
        }

        private void AddSeeThroughCard()
        {
            DeckOfCards.Add(CardCreator.CreateCard(DeckOfCards.Count + 1, CardColour.SeeThrough, CardType.SeeThrough, WILDCARD_VALUE));
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
                        DeckOfCards.Add(CardCreator.CreateCard(DeckOfCards.Count + 1, _Colour, CardType.Zero, Convert.ToInt32(CardType.Zero)));
                        AddColourCards(_Colour);
                        AddColourCards(_Colour);
                        break;
                    case CardColour.SeeThrough:
                        AddSeeThroughCard();
                        AddSeeThroughCard();
                        AddSeeThroughCard();
                        AddSeeThroughCard();
                        AddSeeThroughCard();
                        break;
                }
            }
        }

        // Draw Card Method
        public List<ICard> DrawCards(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("count", "count must be greater than 0");
            }

            Reshuffle();

            List<ICard> drawnCards = DeckOfCards.Take(count).ToList();

            //Remove the drawn cards from the draw pile
            DeckOfCards.RemoveAll(card => drawnCards.Contains(card));
            return drawnCards;
        }

        public ICard DrawInitialCard()
        {
            ICard _DrawnCard = DeckOfCards.Where(card => !Card.SpecialCards.Contains(card.TypeOfCard)).Take(DRAW_FACE_CARD_AMOUNT).SingleOrDefault();

            //Remove the drawn cards from the draw pile
            DeckOfCards.Remove(_DrawnCard);
            return _DrawnCard;
        }

        public ICard DrawCard()
        {
            Reshuffle();
            ICard _DrawnCard = DeckOfCards.Take(DRAW_FACE_CARD_AMOUNT).SingleOrDefault();

            //Remove the drawn cards from the draw pile
            DeckOfCards.Remove(_DrawnCard);
            return _DrawnCard;
        }

        public void Reshuffle()
        {
            if(DeckOfCards.Count < 1)
            {
                __DeckOfCards = PlayedCards;
                Shuffle();
                ICard _FaceCard = DrawCard();
                __PlayedCards = new List<ICard> { _FaceCard };
            }
        }

        // Shuffle Card Method
        public void Shuffle()
        {
            Random random = new Random();

            List<ICard> cards = DeckOfCards;

            for (int i = cards.Count - 1; i >= 0; --i)// may need to check this for condition
            {
                int j = random.Next(i + 1);
                ICard temp = cards[i];
                cards[i] = cards[j];
                cards[j] = temp;

            }
        }

        public ICard CardCreator => __CardCreator = __CardCreator ?? new Card();
        public List<ICard> DeckOfCards => __DeckOfCards = __DeckOfCards ?? new();
        public List<ICard> PlayedCards => __PlayedCards = __PlayedCards ?? new();
    }
}
