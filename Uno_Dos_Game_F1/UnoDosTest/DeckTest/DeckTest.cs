using UnoDos.Cards.Entities;
using UnoDos.Cards.Interfaces;
using UnoDos.Decks.Entities;
using UnoDos.Decks.Interfaces;

namespace UnoDosTest.DeckTest
{
    [TestClass]
    public class DeckTest
    {
        private IDeck __Deck;

        [TestInitialize]
        public void Initialize()
        {
            __Deck = new Deck();
        }

        private void CreateDeck()
        {
            __Deck.CreateDeck();
            __Deck.Shuffle();
        }

        [TestMethod]
        public void DeckInitializationTest()
        {
            CreateDeck();
            Assert.AreEqual(105, __Deck.DeckOfCards.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DrawCardsExceptionTest()
        {
            CreateDeck();
            int count = 120;
            __Deck.DrawCards(count);
        }


        [TestMethod]
        public void DrawCardsTest()
        {
            CreateDeck();
            int _Count = 7;
            List<ICard> _DrawnCards = __Deck.DrawCards(_Count);

            Assert.AreEqual(_DrawnCards.Count, _Count);
            Assert.AreEqual(105 - _Count, __Deck.DeckOfCards.Count);
        }

        [TestMethod]
        public void ReshuffleTest()
        {
            CreateDeck();
            __Deck.PlayedCards.AddRange(__Deck.DrawCards(50));
            __Deck.DrawCards(55);

            __Deck.DrawCard();
            Assert.AreEqual(48, __Deck.DeckOfCards.Count());
        }

        [TestMethod]
        public void DrawCardTest()
        {
            CreateDeck();
            __Deck.DrawCard();

            Assert.AreEqual(105 - 1, __Deck.DeckOfCards.Count);
        }

        [TestMethod]
        public void DrawInitialCardTest()
        {
            CreateDeck();
            __Deck.DrawInitialCard();

            Assert.AreEqual(105 - 1, __Deck.DeckOfCards.Count);
            Assert.AreEqual(29, __Deck.DeckOfCards.Where(card => Card.SpecialCards.Contains(card.TypeOfCard)).Count());
        }

        [TestMethod]
        public void ShuffleTest()
        {
            CreateDeck();
            List<ICard> _CardsBeforeShuffle = __Deck.DeckOfCards.ToList();

            __Deck.Shuffle();

            List<ICard> _CardsAfterShuffle = __Deck.DeckOfCards.ToList();

            // Both decks should not be equal in order to be completely shuffled
            Assert.AreNotEqual(_CardsBeforeShuffle, _CardsAfterShuffle);
        }
    }
}