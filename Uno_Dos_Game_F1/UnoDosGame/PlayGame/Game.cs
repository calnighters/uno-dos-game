using UnoDos.Cards.Entities;
using UnoDos.Cards.Interfaces;
using UnoDos.Decks.Entities;
using UnoDos.Players.Entities;

namespace UnoDos.PlayGame
{
    public class Game
    {
        private const int DRAW_CARD_OPTION = 3;
        private const int INITIAL_DRAW_COUNT = 10;
        private const int PLAY_CARD_OPTION = 2;

        private CPU __CPU;
        private Deck __Deck;
        private Player __Player;

        private void CPUTurn()
        {
            if (Player.Cards.Count > 0)
            {
                __Deck = CPU.PlayCardCPU(Deck, Deck.PlayedCards.Last());
                DisplayHasCPUPlayedCardMessage();
                while (CPU.IsResetCardPlayed)
                {
                    CPU.IsResetCardPlayed = false;
                    DisplayHasCPUPlayedCardMessage();
                    Console.WriteLine(string.Format("\nCPU Plays {0}", Deck.PlayedCards.Last().ToString()));
                }
                if (CPU.IsLoseTwoCardPlayed)
                {
                    __Deck = CPU.LoseTwoCards(Deck);
                }

                if (CPU.IsSwapDeckPlayed)
                {
                    KeyValuePair<List<ICard>, List<ICard>> _SwappedCards = CPU.SwapCards(CPU.Cards, Player.Cards);
                    CPU.Cards = _SwappedCards.Key;
                    Player.Cards = _SwappedCards.Value;
                }
            }
        }

        private int CheckCardSelectedIsValid(string _SelectedCardInput)
        {
            bool _IsSelectedCardInputValid = int.TryParse(_SelectedCardInput, out int _SelectedCardPosition);

            if (!_IsSelectedCardInputValid)
            {
                Console.WriteLine("\nPlease type in your card in a valid format! eg: To use the first card in your hand, you type 1, for the second card, you type 2:");
                _SelectedCardPosition = 0;
            }

            if (_SelectedCardPosition > Player.Cards.Count || _SelectedCardPosition < 1)
            {
                Console.WriteLine("\nThe card you want to play is out of range of the cards in your hand! Please try again:");
                _SelectedCardPosition = 0;
            }
            return _SelectedCardPosition;
        }

        public void DisplayDrawnCard()
        {
            Player.Cards.Add(__Deck.DrawCard());
            Console.WriteLine(string.Format("\nThe card you drew was: {0}", __Player.Cards.Last().ToString()));
        }

        public void DisplayPlayerHand()
        {
            Console.WriteLine("\nHere are the current cards in your hand: \n");
            for (int i = 0; i < Player.Cards.Count; i++)
            {
                Console.Write($"({i + 1}) {Player.Cards[i].ToString()} ");
            }
        }

        private void DisplayHasCPUPlayedCardMessage()
        {
            if (CPU.HasCPUPlayedCard)
            {
                Console.WriteLine(string.Format("\nCPU Plays {0}", Deck.PlayedCards.Last().ToString()));
            }
            else
            {
                Console.WriteLine(string.Format("\nCPU did not have any playable cards!"));
            }
        }

        public void InitialiseGame()
        {
            Deck.CreateDeck();
            Deck.Shuffle();
            Player.Cards = Deck.DrawCards(INITIAL_DRAW_COUNT);
            CPU.Cards = Deck.DrawCards(INITIAL_DRAW_COUNT);
            Deck.PlayedCards.Add(Deck.DrawInitialCard());
        }

        private void PlayerTurn()
        {
            PlayerOptions();

            while (Player.IsResetCardPlayed)
            {
                Player.IsResetCardPlayed = false;
                Console.WriteLine("\nYou played a reset card! Take another turn!");
                PlayerOptions();
            }
            if (Player.IsLoseTwoCardPlayed)
            {
                PlayerSelectTwoCardsToLose();
            }

            if (Player.IsSwapDeckPlayed)
            {
                KeyValuePair<List<ICard>, List<ICard>> _SwappedCards = Player.SwapCards(Player.Cards, CPU.Cards);
                Player.Cards = _SwappedCards.Key;
                CPU.Cards = _SwappedCards.Value;
            }
        }

        public void PlayGame()
        {
            Console.WriteLine("Welcome to Uno Dos!");
            Console.WriteLine("\nPlease enter your desired username to continue...");
            Player.PlayerName = Console.ReadLine();

            InitialiseGame();
            while (Player.Cards.Count > 0 && CPU.Cards.Count > 0)
            {
                PlayerTurn();
                CPUTurn();
            }

            if (Player.Cards.Count == 0)
            {
                Console.WriteLine("Congratulations! You won!");
            }
            if(CPU.Cards.Count == 0)
            {
                Console.WriteLine("Unlucky! CPU Won!");
            }
        }

        private void PlayerOptions()
        {
            int _SelectedOption = 0;

            while (_SelectedOption != PLAY_CARD_OPTION && _SelectedOption != DRAW_CARD_OPTION)
            {
                ShowLastCardPlayedToString();
                Console.WriteLine(string.Format("\nIt's your turn {0}! \n\nYou have {1} cards remaining. \n\nWould you like to: \n1) View your cards. \n2) Play a card. \n3) Draw a card", __Player.PlayerName, __Player.Cards.Count));
                int.TryParse(Console.ReadLine(), out _SelectedOption);

                switch (_SelectedOption)
                {
                    case 1:
                        DisplayPlayerHand();
                        break;
                    case 2:
                        List<bool> _PlayableCards = new();
                        Player.Cards.ForEach(card =>
                        {
                            _PlayableCards.Add(Player.CanPlayCard(card, __Deck.PlayedCards.Last()));
                        });

                        if (_PlayableCards.All(cardCanBePlayed => !cardCanBePlayed))
                        {
                            Console.WriteLine("\nNo Cards in your hand can be played, drawing a card for you...");
                            DisplayDrawnCard();
                        }
                        else
                        {
                            DisplayPlayerHand();
                            ShowLastCardPlayedToString();
                            Console.WriteLine("\n\nPlease Select a card from your hand. You can do this by typing in the index of the card you want to play. eg: To play the first card in your hand, you type 1 :");
                            bool _CardPlayed = false;
                            while (!_CardPlayed)
                            {
                                _CardPlayed = TryPlayCard(Console.ReadLine());
                            }
                        }
                        break;
                    case 3:
                        DisplayDrawnCard();
                        break;
                    default:
                        Console.WriteLine("\nPlease select a valid option!");
                        break;
                }
            }
        }

        private void PlayerSelectTwoCardsToLose()
        {
            int _CardsSelected = 0;
            List<ICard> _CardsToRemove = new List<ICard>();
            Console.Clear();
            Console.WriteLine("\nYou played a Lose Two card! Please select two cards to lose");
            DisplayPlayerHand();
            while (_CardsSelected < 2)
            {
                Console.WriteLine(string.Format("\nCard {0}:", _CardsSelected + 1));
                int _SelectedCardPosition = CheckCardSelectedIsValid(Console.ReadLine());
                if (_SelectedCardPosition > 0)
                {
                    _CardsToRemove.Add(Player.Cards[_SelectedCardPosition - 1]);
                    _CardsSelected++;
                }
            }

            __Deck = Player.LoseTwoCards(_CardsToRemove, Deck);
        }

        private void ShowLastCardPlayedToString()
        {
            Console.WriteLine(string.Format("\n\nThe current card showing on the table is {0}", __Deck.PlayedCards.Last().ToString()));
        }

        private bool TryPlayCard(string _SelectedCardInput)
        {
            int _SelectedCardPosition = CheckCardSelectedIsValid(_SelectedCardInput);

            if (_SelectedCardPosition > 0)
            {
                ICard _SelectedCard = Player.Cards[_SelectedCardPosition - 1];

                if (!Player.CanPlayCard(_SelectedCard, Deck.PlayedCards.Last()))
                {
                    Console.WriteLine("\nInvalid Card... Please select another card:");
                }
                else
                {
                    __Deck = Player.PlayCard(_SelectedCard, Deck);
                    return true;
                }
            }

            return false;
        }


        private CPU CPU => __CPU = __CPU ?? new CPU();
        private Deck Deck => __Deck = __Deck ?? new Deck();
        private Player Player => __Player = __Player ?? new Player();
    }
}
