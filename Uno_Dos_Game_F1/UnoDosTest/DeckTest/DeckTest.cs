using UnoDos.Cards.Entities;
using UnoDos.Decks.Entities;

namespace UnoDosTest.DeckTest
{
    [TestClass]
    public class DeckTest
    {
        private Deck __Deck;

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
            int count = 7;
            List<Card> drawnCards = __Deck.DrawCards(count);

            Assert.AreEqual(drawnCards.Count, count);
            Assert.AreEqual(105 - count, __Deck.DeckOfCards.Count);
        }

        [TestMethod]
        public void ShuffleTest()
        {
            CreateDeck();
            List<Card> cardsBeforeShuffle = __Deck.DeckOfCards.ToList();

            __Deck.Shuffle();

            List<Card> cardsAfterShuffle = __Deck.DeckOfCards.ToList();

            // Both decks should not be equal in order to be completely shuffled
            Assert.AreNotEqual(cardsBeforeShuffle, cardsAfterShuffle);
        }
    }
}