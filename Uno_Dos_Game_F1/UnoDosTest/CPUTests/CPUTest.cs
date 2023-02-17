using UnoDos.Cards.Entities;
using UnoDos.Cards.Enums;
using UnoDos.Cards.Interfaces;
using UnoDos.Decks.Entities;
using UnoDos.Decks.Interfaces;
using UnoDos.Players.Entities;
using UnoDos.Players.Interfaces;

namespace UnoDosTest.CPUTests
{
    [TestClass]
    public class CPUTest
    {
        private const int INITIAL_DRAW_COUNT = 10;

        IDeck __Deck;
        ICPU __CPU;

        [TestInitialize]
        public void Initialise()
        {
            __Deck = new Deck();
            __Deck.CreateDeck();
            __Deck.PlayedCards.Add(new Card()
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
            __CPU.Cards = new List<ICard>();

            ICard _ForceCardToPlay = new Card()
            {
                CardID = 6,
                CardScore = 6,
                Colour = CardColour.Orange,
                TypeOfCard = CardType.Six
            };

            __CPU.Cards.AddRange(new List<ICard> 
            {
                _ForceCardToPlay,
                new Card()
                {
                    CardID = 9,
                    CardScore = 9,
                    Colour = CardColour.Pink,
                    TypeOfCard = CardType.Nine
                }
            });

            __CPU.PlayCardCPU(__Deck);

            Assert.AreEqual(1, __CPU.Cards.Count);
            Assert.AreNotEqual(_ForceCardToPlay, __CPU.Cards[0]);
            Assert.AreEqual(_ForceCardToPlay, __Deck.PlayedCards.Last());
            Assert.IsTrue(__CPU.HasCPUPlayedCard);
        }

        [TestMethod]
        public void DrawCardTest()
        {
            Initialise();
            __CPU.Cards = new List<ICard>();

            ICard _ForceCardToPlay = new Card()
            {
                CardID = 7,
                CardScore = 7,
                Colour = CardColour.Orange,
                TypeOfCard = CardType.Seven
            };

            __CPU.Cards.AddRange(new List<ICard>
            {
                _ForceCardToPlay,
                new Card()
                {
                    CardID = 9,
                    CardScore = 9,
                    Colour = CardColour.Pink,
                    TypeOfCard = CardType.Nine
                }
            });

            __CPU.PlayCardCPU(__Deck);

            Assert.AreEqual(3, __CPU.Cards.Count);
            Assert.AreEqual(_ForceCardToPlay, __CPU.Cards[0]);
            Assert.AreNotEqual(_ForceCardToPlay, __Deck.PlayedCards.Last());
            Assert.IsFalse(__CPU.HasCPUPlayedCard);
        }

        [TestMethod]
        public void CPU_LoseTwo_ShouldRemoveAllCardsFromPlayerHand()
        {
            int _OriginalDeckSize = __Deck.DeckOfCards.Count;
            __CPU.Cards = new List<ICard>();

            ICard _CardToLose = new Card()
            {
                CardID = 6,
                CardScore = 6,
                Colour = CardColour.Orange,
                TypeOfCard = CardType.Six
            };

            __CPU.Cards.Add(_CardToLose);

            __Deck = __CPU.LoseTwoCards(__Deck);

            Assert.AreEqual(0, __CPU.Cards.Count);

            Assert.AreEqual(_OriginalDeckSize + 1, __Deck.DeckOfCards.Count);
            Assert.AreEqual(__Deck.DeckOfCards.Last(), _CardToLose);
        }

        [TestMethod]
        public void CPU_LoseTwo_ShouldRemoveTwoCardsFromPlayerHand()
        {
            int _OriginalDeckSize = __Deck.DeckOfCards.Count;

            List<ICard> _CardsToLose = new List<ICard>
            {
                __CPU.Cards[0],
                __CPU.Cards[1],
            };

            __CPU.Cards = new List<ICard>();
            __CPU.Cards.AddRange(_CardsToLose);

            __Deck = __CPU.LoseTwoCards(_CardsToLose, __Deck);

            Assert.AreEqual(0, __CPU.Cards.Count);

            Assert.AreEqual(_OriginalDeckSize + _CardsToLose.Count, __Deck.DeckOfCards.Count);
            Assert.AreEqual(__Deck.DeckOfCards[__Deck.DeckOfCards.Count - 2], _CardsToLose[0]);
            Assert.AreEqual(__Deck.DeckOfCards.Last(), _CardsToLose[1]);
        }

        [TestMethod]
        public void CPU_SwapCards_ShouldReplaceExistingHandWithNewHand()
        {
            IPlayer _Player = new Player();
            _Player.Cards = __Deck.DrawCards(9);

            KeyValuePair<List<ICard>, List<ICard>> _UnswappedCards = new KeyValuePair<List<ICard>, List<ICard>>(__CPU.Cards, _Player.Cards);

            KeyValuePair<List<ICard>, List<ICard>> _SwappedCards = __CPU.SwapCards(_UnswappedCards);

            Assert.AreNotEqual(_UnswappedCards.Key.Count, _SwappedCards.Key.Count);
            Assert.AreNotEqual(_UnswappedCards.Key[0], _SwappedCards.Key[0]);
            Assert.AreEqual(_UnswappedCards.Value.Count, _SwappedCards.Key.Count);
            Assert.AreEqual(_UnswappedCards.Value[0], _SwappedCards.Key[0]);
        }
    }
}
