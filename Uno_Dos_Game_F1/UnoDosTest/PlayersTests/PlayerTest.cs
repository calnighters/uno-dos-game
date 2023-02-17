using UnoDos.Cards.Entities;
using UnoDos.Cards.Enums;
using UnoDos.Cards.Interfaces;
using UnoDos.Decks.Entities;
using UnoDos.Decks.Interfaces;
using UnoDos.Players.Entities;
using UnoDos.Players.Interfaces;

namespace UnoDosTest.PlayersTests
{
    [TestClass]
    public class PlayerTest
    {
        private const string EXPECTED_COLOUR_ERROR = "Card colour {0} is an invalid card! Please play a card with this colour: Orange";
        private const string EXPECTED_VALUE_ERROR = "Card number {0} is an invalid card! Please play a card with a value of +1 or -1 of the last card value, which is 5!";
        private const int INITIAL_DRAW_COUNT = 10;

        IPlayer __Player;
        IDeck __Deck;

        [TestInitialize]
        public void Initialise()
        {
            __Player = new Player();
            __Deck = new Deck();
            __Deck.CreateDeck();
            __Player.Cards = __Deck.DrawCards(INITIAL_DRAW_COUNT);
            __Deck.PlayedCards.Add(new Card()
            {
                CardID = 5,
                CardScore = 5,
                Colour = CardColour.Orange,
                TypeOfCard = CardType.Five
            });
        }
        private void AssertCardIsNotPlayed(int errorCount, string lastErrorMessage, int expectedPlayerCardCount, int cardsPlayed)
        {
            Assert.AreEqual(cardsPlayed, __Deck.PlayedCards.Count);
            Assert.AreEqual(errorCount, __Player.Errors.Count);
            Assert.AreEqual(lastErrorMessage, __Player.Errors[errorCount - 1]);
            Assert.AreEqual(expectedPlayerCardCount, __Player.Cards.Count);
        }

        private void AssertCardIsPlayed(ICard expectedPlayedCard, ICard actualPlayedCard, int cardsPlayed)
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
        public void Player_PlayCard_ShouldReturnNoErrorWhenDifferentColour()
        {
            ICard _PurpleFour = new Card()
            {
                CardID = 1000,
                CardScore = 4,
                Colour = CardColour.Purple,
                TypeOfCard = CardType.Four
            };

            ICard _PurpleSwitch = new Card()
            {
                CardID = 1001,
                CardScore = 20,
                Colour = CardColour.Purple,
                TypeOfCard = CardType.Reset
            };

            __Player.Cards.Add(_PurpleFour);
            __Player.Cards.Add(_PurpleSwitch);

            __Deck = __Player.PlayCard(_PurpleFour, __Deck);

            AssertCardIsPlayed(_PurpleFour, __Deck.PlayedCards.Last(), 2);

            __Deck = __Player.PlayCard(_PurpleSwitch, __Deck);

            AssertCardIsPlayed(_PurpleSwitch, __Deck.PlayedCards.Last(), 3);
        }

        [TestMethod]
        public void Player_PlayCard_ShouldReturnErrorWhenInvalidValue()
        {
            ICard _OrangeThree = new Card()
            {
                CardID = 1000,
                CardScore = 3,
                Colour = CardColour.Orange,
                TypeOfCard = CardType.Three
            };

            ICard _OrangeNine = new Card()
            {
                CardID = 1001,
                CardScore = 9,
                Colour = CardColour.Orange,
                TypeOfCard = CardType.Nine
            };

            __Player.Cards.Add(_OrangeThree);
            __Player.Cards.Add(_OrangeNine);

            __Deck = __Player.PlayCard(_OrangeThree, __Deck);

            AssertCardIsNotPlayed(1, string.Format(EXPECTED_VALUE_ERROR, 3), 12, 1);

            __Deck = __Player.PlayCard(_OrangeNine, __Deck);

            AssertCardIsNotPlayed(2, string.Format(EXPECTED_VALUE_ERROR, 9), 12, 1);
        }

        [TestMethod]
        public void Player_PlayCard_ShouldReturnErrorWhenInvalidColour()
        {
            ICard _OrangeThree = new Card()
            {
                CardID = 1000,
                CardScore = 3,
                Colour = CardColour.Purple,
                TypeOfCard = CardType.LoseTwo
            };

            ICard _OrangeNine = new Card()
            {
                CardID = 1001,
                CardScore = 9,
                Colour = CardColour.Pink,
                TypeOfCard = CardType.SwapDeck
            };

            __Player.Cards.Add(_OrangeThree);
            __Player.Cards.Add(_OrangeNine);

            __Deck = __Player.PlayCard(_OrangeThree, __Deck);

            AssertCardIsNotPlayed(1, string.Format(EXPECTED_COLOUR_ERROR, CardColour.Purple), 12, 1);

            __Deck = __Player.PlayCard(_OrangeNine, __Deck);

            AssertCardIsNotPlayed(2, string.Format(EXPECTED_COLOUR_ERROR, CardColour.Pink), 12, 1);
        }

        [TestMethod]
        public void Player_PlayCard_ShouldReturnPlayedCardWhenCardPlusOrMinusOneScore()
        {
            ICard _OrangeFour = new Card()
            {
                CardID = 1000,
                CardScore = 4,
                Colour = CardColour.Orange,
                TypeOfCard = CardType.Four
            };

            ICard _OrangeSix = new Card()
            {
                CardID = 1001,
                CardScore = 5,
                Colour = CardColour.Orange,
                TypeOfCard = CardType.Five
            };

            __Player.Cards.Add(_OrangeFour);
            __Player.Cards.Add(_OrangeSix);

            __Deck = __Player.PlayCard(_OrangeFour, __Deck);

            AssertCardIsPlayed(_OrangeFour, __Deck.PlayedCards.Last(), 2);

            __Deck = __Player.PlayCard(_OrangeSix, __Deck);

            AssertCardIsPlayed(_OrangeSix, __Deck.PlayedCards.Last(), 3);
            Assert.AreEqual(INITIAL_DRAW_COUNT, __Player.Cards.Count);
        }

        [TestMethod]
        public void Player_PlayCard_ShouldReturnPlayedCardWhenSpecialCard()
        {
            ICard _SwitchCard = new Card()
            {
                CardID = 1000,
                CardScore = 20,
                Colour = CardColour.Orange,
                TypeOfCard = CardType.Reset
            };

            ICard _SwapDeckCard = new Card()
            {
                CardID = 1001,
                CardScore = 20,
                Colour = CardColour.Orange,
                TypeOfCard = CardType.SwapDeck
            };

            ICard _LoseTwoCard = new Card()
            {
                CardID = 1002,
                CardScore = 20,
                Colour = CardColour.Orange,
                TypeOfCard = CardType.LoseTwo
            };

            __Player.Cards.Add(_SwitchCard);
            __Player.Cards.Add(_SwapDeckCard);

            __Deck = __Player.PlayCard(_SwitchCard, __Deck);


            AssertCardIsPlayed(_SwitchCard, __Deck.PlayedCards.Last(), 2);

            __Deck = __Player.PlayCard(_SwapDeckCard, __Deck);

            AssertCardIsPlayed(_SwapDeckCard, __Deck.PlayedCards.Last(), 3);

            __Deck = __Player.PlayCard(_LoseTwoCard, __Deck);

            AssertCardIsPlayed(_LoseTwoCard, __Deck.PlayedCards.Last(), 4);
            Assert.AreEqual(INITIAL_DRAW_COUNT, __Player.Cards.Count);
        }

        [TestMethod]
        public void Player_PlayCard_ShouldReturnPlayedCardWhenWildcard()
        {
            ICard _Wildcard = new Card()
            {
                CardID = 1000,
                CardScore = 50,
                Colour = CardColour.SeeThrough,
                TypeOfCard = CardType.SeeThrough
            };

            __Player.Cards.Add(_Wildcard);

            __Deck = __Player.PlayCard(_Wildcard, __Deck);

            AssertCardIsPlayed(_Wildcard, __Deck.PlayedCards.Last(), 2);
            Assert.AreEqual(INITIAL_DRAW_COUNT, __Player.Cards.Count);
        }

        [TestMethod]
        public void Player_LoseTwo_ShouldRemoveAllCardsFromPlayerHand()
        {
            int _OriginalDeckSize = __Deck.DeckOfCards.Count;

            List<ICard> _CardsToLose = new List<ICard>
            {
                __Player.Cards[0],
            };
            __Player.Cards = new List<ICard>()
            {
                _CardsToLose[0]
            };

            __Deck = __Player.LoseTwoCards(_CardsToLose, __Deck);

            Assert.AreEqual(0, __Player.Cards.Count);

            Assert.AreEqual(_OriginalDeckSize + _CardsToLose.Count, __Deck.DeckOfCards.Count);
            Assert.AreEqual(__Deck.DeckOfCards.Last(), _CardsToLose[0]);
        }

        [TestMethod]
        public void Player_LoseTwo_ShouldRemoveTwoCardsFromPlayerHand()
        {
            int _OriginalDeckSize = __Deck.DeckOfCards.Count;

            List<ICard> _CardsToLose = new List<ICard>
            {
                __Player.Cards[0],
                __Player.Cards[1],
            };

            __Deck = __Player.LoseTwoCards(_CardsToLose, __Deck);

            Assert.AreEqual(INITIAL_DRAW_COUNT - _CardsToLose.Count, __Player.Cards.Count);
            Assert.AreNotEqual(_CardsToLose[0], __Player.Cards[0]);
            Assert.AreNotEqual(_CardsToLose[1], __Player.Cards[1]);

            Assert.AreEqual(_OriginalDeckSize + _CardsToLose.Count, __Deck.DeckOfCards.Count);
            Assert.AreEqual(__Deck.DeckOfCards[__Deck.DeckOfCards.Count - 2], _CardsToLose[0]);
            Assert.AreEqual(__Deck.DeckOfCards.Last(), _CardsToLose[1]);
        }

        [TestMethod]
        public void Player_SwapCards_ShouldReplaceExistingHandWithNewHand()
        {
            ICPU _CPU = new CPU();
            _CPU.Cards = __Deck.DrawCards(9);

            KeyValuePair<List<ICard>, List<ICard>> _UnswappedCards = new KeyValuePair<List<ICard>, List<ICard>>(__Player.Cards, _CPU.Cards);

            KeyValuePair<List<ICard>, List<ICard>> _SwappedCards = __Player.SwapCards(_UnswappedCards);

            Assert.AreNotEqual(_UnswappedCards.Key.Count, _SwappedCards.Key.Count);
            Assert.AreNotEqual(_UnswappedCards.Key[0], _SwappedCards.Key[0]);
            Assert.AreEqual(_UnswappedCards.Value.Count, _SwappedCards.Key.Count);
            Assert.AreEqual(_UnswappedCards.Value[0], _SwappedCards.Key[0]);
        }
    }
}
