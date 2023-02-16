using UnoDos.Cards.Entities;
using UnoDos.Cards.Enums;
using UnoDos.Decks.Entities;
using UnoDos.Players.Entities;


namespace UnoDosTest.CPUTests
{
    [TestClass]
    public class CPUTest
    {
        private const int INITIAL_DRAW_COUNT = 10;

        Player __Player;
        Deck __Deck;
        CPU __CPU;

        [TestInitialize]
        public void Initialise()
        {
            __Player = new Player();
            __Deck = new Deck();
            __Deck.CreateDeck();
            __Player.Cards = __Deck.DrawCards(INITIAL_DRAW_COUNT);
            __Deck.PlayedCards.Add(new()
            {
                CardID = 5,
                CardScore = 5,
                Colour = CardColour.Orange,
                TypeOfCard = CardType.Five
            });
            __CPU = new CPU();
            __CPU.Cards = __Deck.DrawCards(INITIAL_DRAW_COUNT);
        }

        [TestMethod]
        public void CPUInitializationTest()
        {
            Initialise();
            Assert.AreEqual(10, __CPU.Cards.Count);
        }

        [TestMethod]
        public void PlayCardTest()
        {
            Initialise();
            Assert.AreEqual(10, __CPU.Cards.Count);

            __CPU.playCard(__CPU.playableCards);
            Assert.AreEqual(9, __CPU.Cards.Count);
        }
    }
}
