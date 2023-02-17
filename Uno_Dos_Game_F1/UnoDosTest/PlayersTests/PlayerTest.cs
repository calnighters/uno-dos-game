using UnoDos.Cards.Entities;
using UnoDos.Cards.Enums;
using UnoDos.Decks.Entities;
using UnoDos.Players.Entities;

namespace UnoDosTest.PlayersTests
{
    [TestClass]
    public class PlayerTest
    {
        private const string EXPECTED_COLOUR_ERROR = "Card colour Purple is an invalid card! Please play a card with this colour: Orange";
        private const string EXPECTED_VALUE_ERROR = "Card number {0} is an invalid card! Please play a card with a value of +1 or -1 of the last card value, which is 5!";
        private const int INITIAL_DRAW_COUNT = 10;

        Player __Player;
        Deck __Deck;

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
        }
        private void AssertCardIsNotPlayed(int errorCount, string lastErrorMessage, int expectedPlayerCardCount)
        {
            Assert.AreEqual(1, __Deck.PlayedCards.Count);
            Assert.AreEqual(errorCount, __Player.Errors.Count);
            Assert.AreEqual(lastErrorMessage, __Player.Errors[errorCount - 1]);
            Assert.AreEqual(expectedPlayerCardCount, __Player.Cards.Count);
        }

        private void AssertCardIsPlayed(Card expectedPlayedCard, Card actualPlayedCard, int cardsPlayed)
        {
            Assert.AreEqual(cardsPlayed, __Deck.PlayedCards.Count);
            Assert.AreEqual(expectedPlayedCard.CardID, actualPlayedCard.CardID);
            Assert.AreEqual(expectedPlayedCard.Colour, actualPlayedCard.Colour);
            Assert.AreEqual(expectedPlayedCard.CardScore, actualPlayedCard.CardScore);
            Assert.AreEqual(expectedPlayedCard.TypeOfCard, actualPlayedCard.TypeOfCard);
        }

        [TestMethod]
        public void Player_DrawCard_ShouldAddOneCardToPlayerCardCount()
        {
            int _InitialPlayerCardCount = __Player.Cards.Count;
            int _InitialDeckCardCount = __Deck.DeckOfCards.Count;
            __Player.DrawCard(__Deck);

            Assert.AreEqual(_InitialDeckCardCount - 1, __Deck.DeckOfCards.Count);
            Assert.AreEqual(_InitialPlayerCardCount + 1, __Player.Cards.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Player_DrawCard_ShouldError()
        {
            __Deck = new Deck();
            __Player.DrawCard(__Deck);
        }

        [TestMethod]
        public void Player_PlayCard_ShouldReturnErrorWhenInvalidColour()
        {
            Card _PurpleFour = new Card()
            {
                CardID = 1000,
                CardScore = 4,
                Colour = CardColour.Purple,
                TypeOfCard = CardType.Four
            };

            Card _PurpleSwitch = new Card()
            {
                CardID = 1001,
                CardScore = 20,
                Colour = CardColour.Purple,
                TypeOfCard = CardType.Reset
            };

            __Player.Cards.Add(_PurpleFour);
            __Player.Cards.Add(_PurpleSwitch);

            __Deck = __Player.PlayCard(_PurpleFour, __Deck.PlayedCards.Last(), __Deck);

            AssertCardIsNotPlayed(1, EXPECTED_COLOUR_ERROR, 12);

            __Deck = __Player.PlayCard(_PurpleSwitch, __Deck.PlayedCards.Last(), __Deck);

            AssertCardIsNotPlayed(2, EXPECTED_COLOUR_ERROR, 12);
        }

        [TestMethod]
        public void Player_PlayCard_ShouldReturnErrorWhenInvalidValue()
        {
            Card _OrangeThree = new Card()
            {
                CardID = 1000,
                CardScore = 3,
                Colour = CardColour.Orange,
                TypeOfCard = CardType.Three
            };

            Card _OrangeNine = new Card()
            {
                CardID = 1001,
                CardScore = 9,
                Colour = CardColour.Orange,
                TypeOfCard = CardType.Nine
            };

            __Player.Cards.Add(_OrangeThree);
            __Player.Cards.Add(_OrangeNine);

            __Deck = __Player.PlayCard(_OrangeThree, __Deck.PlayedCards.Last(), __Deck);

            AssertCardIsNotPlayed(1, string.Format(EXPECTED_VALUE_ERROR, 3), 12);

            __Deck = __Player.PlayCard(_OrangeNine, __Deck.PlayedCards.Last(), __Deck);

            AssertCardIsNotPlayed(2, string.Format(EXPECTED_VALUE_ERROR, 9), 12);
        }

        [TestMethod]
        public void Player_PlayCard_ShouldReturnPlayedCardWhenCardPlusOrMinusOneScore()
        {
            Card _OrangeFour = new Card()
            {
                CardID = 1000,
                CardScore = 4,
                Colour = CardColour.Orange,
                TypeOfCard = CardType.Four
            };

            Card _OrangeSix = new Card()
            {
                CardID = 1001,
                CardScore = 5,
                Colour = CardColour.Orange,
                TypeOfCard = CardType.Five
            };

            __Player.Cards.Add(_OrangeFour);
            __Player.Cards.Add(_OrangeSix);

            __Deck = __Player.PlayCard(_OrangeFour, __Deck.PlayedCards.Last(), __Deck);

            AssertCardIsPlayed(_OrangeFour, __Deck.PlayedCards.Last(), 2);

            __Deck = __Player.PlayCard(_OrangeSix, __Deck.PlayedCards.Last(), __Deck);

            AssertCardIsPlayed(_OrangeSix, __Deck.PlayedCards.Last(), 3);
            Assert.AreEqual(INITIAL_DRAW_COUNT, __Player.Cards.Count);
        }

        [TestMethod]
        public void Player_PlayCard_ShouldReturnPlayedCardWhenSpecialCard()
        {
            Card _SwitchCard = new Card()
            {
                CardID = 1000,
                CardScore = 20,
                Colour = CardColour.Orange,
                TypeOfCard = CardType.Reset
            };

            Card _SwapDeckCard = new Card()
            {
                CardID = 1001,
                CardScore = 20,
                Colour = CardColour.Orange,
                TypeOfCard = CardType.SwapDeck
            };

            Card _LoseTwoCard = new Card()
            {
                CardID = 1002,
                CardScore = 20,
                Colour = CardColour.Orange,
                TypeOfCard = CardType.LoseTwo
            };

            __Player.Cards.Add(_SwitchCard);
            __Player.Cards.Add(_SwapDeckCard);

            __Deck = __Player.PlayCard(_SwitchCard, __Deck.PlayedCards.Last(), __Deck);


            AssertCardIsPlayed(_SwitchCard, __Deck.PlayedCards.Last(), 2);

            __Deck = __Player.PlayCard(_SwapDeckCard, __Deck.PlayedCards.Last(), __Deck);

            AssertCardIsPlayed(_SwapDeckCard, __Deck.PlayedCards.Last(), 3);

            __Deck = __Player.PlayCard(_LoseTwoCard, __Deck.PlayedCards.Last(), __Deck);

            AssertCardIsPlayed(_LoseTwoCard, __Deck.PlayedCards.Last(), 4);
            Assert.AreEqual(INITIAL_DRAW_COUNT, __Player.Cards.Count);
        }

        [TestMethod]
        public void Player_PlayCard_ShouldReturnPlayedCardWhenWildcard()
        {
            Card _Wildcard = new Card()
            {
                CardID = 1000,
                CardScore = 50,
                Colour = CardColour.SeeThrough,
                TypeOfCard = CardType.SeeThrough
            };

            __Player.Cards.Add(_Wildcard);

            __Deck = __Player.PlayCard(_Wildcard, __Deck.PlayedCards.Last(), __Deck);

            AssertCardIsPlayed(_Wildcard, __Deck.PlayedCards.Last(), 2);
            Assert.AreEqual(INITIAL_DRAW_COUNT, __Player.Cards.Count);
        }
    }
}
