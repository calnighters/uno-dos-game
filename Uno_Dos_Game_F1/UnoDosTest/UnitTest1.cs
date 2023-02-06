using CardDeck;
using static CardDeck.Card;

namespace UnoDosTest
{
    [TestClass]
    public class DeckTest
    {
        [TestMethod]
        public void DeckInitializationTest()
        {
            Deck deck = new Deck();
            List<Card> cardDeck = deck.CreateDeck();
            Assert.AreEqual(105, cardDeck.Count);
        }

        [TestMethod]
        public void ShuffleTest()
        {
            Deck deck = new Deck();
            List<Card> cardDeck = deck.CreateDeck();
            List<Card> cardsBeforeShuffle = deck.getCards.ToList();

            deck.Shuffle();

            List<Card> cardsAfterShuffle = deck.getCards.ToList();

            // Both decks should not be equal in order to be completely shuffled
            Assert.AreNotEqual(cardsBeforeShuffle, cardsAfterShuffle);
        }

        [TestMethod]
        public void DrawCardsTest()
        {
            Deck deck = new Deck();
            List<Card> cardDeck = deck.CreateDeck();
            int count = 7;
            List<Card> drawnCards = deck.DrawCards(count);

            Assert.AreEqual(drawnCards.Count, count);
            Assert.AreEqual(105 - count, deck.getCards.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DrawCardsExceptionTest()
        {
            Deck deck = new Deck();
            List<Card> cardDeck = deck.CreateDeck();
            int count = 120;
            List<Card> drawnCards = deck.DrawCards(count);
        }
    }
}