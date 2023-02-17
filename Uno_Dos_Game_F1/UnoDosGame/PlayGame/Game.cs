using UnoDos.Cards.Interfaces;
using UnoDos.Decks.Entities;
using UnoDos.Decks.Interfaces;
using UnoDos.Players.Entities;
using UnoDos.Players.Interfaces;

namespace UnoDos.PlayGame
{
    public class Game
    {
        private const int DRAW_CARD_OPTION = 3;
        private const int END_GAME_OPTION = 3;
        private const int INITIAL_DRAW_COUNT = 10;
        private const int PLAY_CARD_OPTION = 2;

        private ICPU __CPU;
        private IDeck __Deck;
        private IPlayer __Player;

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

        private void CPUTurn()
        {
            if (Player.Cards.Count > 0)
            {
                __Deck = CPU.PlayCardCPU(Deck);
                DisplayHasCPUPlayedCardMessage();
                while (CPU.IsResetCardPlayed && CPU.Cards.Count > 0)
                {
                    __Deck = CPU.PlayCardCPU(Deck);
                    CPU.IsResetCardPlayed = false;
                    DisplayHasCPUPlayedCardMessage();
                }
                if (CPU.IsLoseTwoCardPlayed)
                {
                    __Deck = CPU.LoseTwoCards(Deck);
                }

                if (CPU.IsSwapDeckPlayed && CPU.Cards.Count > 0)
                {
                    KeyValuePair<List<ICard>, List<ICard>> _SwappedCards = CPU.SwapCards(new KeyValuePair<List<ICard>, List<ICard>>(CPU.Cards, Player.Cards));
                    CPU.Cards = _SwappedCards.Key;
                    Player.Cards = _SwappedCards.Value;
                }
            }
        }

        private void DisplayDrawnCard()
        {
            Player.Cards.Add(__Deck.DrawCard());
            Console.WriteLine(string.Format("\nThe card you drew was: {0}", __Player.Cards.Last().ToString()));
        }

        private void DisplayHasCPUPlayedCardMessage()
        {
            if (CPU.HasCPUPlayedCard)
            {
                Console.WriteLine(string.Format("\nCPU Plays {0}", Deck.LastCardPlayed.ToString()));
            }
            else
            {
                Console.WriteLine(string.Format("\nCPU did not have any playable cards!"));
            }
        }

        private void DisplayPlayerHand()
        {
            Console.WriteLine("\nHere are the current cards in your hand: \n");
            for (int i = 0; i < Player.Cards.Count; i++)
            {
                Console.Write($"({i + 1}) {Player.Cards[i].ToString()} ");
            }
        }

        private void DisplayRules()
        {
            Console.Clear();
            Console.WriteLine("Here are the rules for the game!");
            Console.WriteLine("\nThis game is called Uno Dos, it is very similar to Uno but with a few caveats.");
            Console.WriteLine("\n1) Players will receive a set of 10 cards at the start of the game and so will the CPU");
            Console.WriteLine("\n2) If the current showing card on the table is numbered, players can only play their numbered cards if it is +1 or -1 of the current number shown, regardless of colour");
            Console.WriteLine("\n3) Special cards can only be played if they are the same colour as the card shown on the table. You can only use a different colour if the special card is the same as what has just been played");
            Console.WriteLine("\n4) The See Through card is an exception to this, it can be played at anytime. Once played, it'll take the colour of the card on the table");
            Console.WriteLine("\n5) If a special card is the current card showing on the table then you can play any numbered card as long as it is the same colour of the special card");
            Console.WriteLine("\n6) The Reset card allows the player to take another turn");
            Console.WriteLine("\n7) The Lose Two card allows the player to select any two cards they would like to get rid of and then places them two cards to the bottom of the card deck");
            Console.WriteLine("\n8) The Swap Deck card swaps the player's cards with the CPU's cards");
            Console.WriteLine("\n9) Compete to get rid of all your cards before the CPU does to win!");
            Console.WriteLine("\n\nPress enter to return to the main menu...");
            Console.ReadLine();
            Console.Clear();
        }

        private void InitialiseGame()
        {
            __Deck = new Deck();
            __Player = new Player();
            __CPU = new CPU();
            Deck.CreateDeck();
            Deck.Shuffle();
            Player.Cards = Deck.DrawCards(INITIAL_DRAW_COUNT);
            CPU.Cards = Deck.DrawCards(INITIAL_DRAW_COUNT);
            Deck.PlayedCards.Add(Deck.DrawInitialCard());
        }

        public void PlayGame()
        {
            int _GameOption = 0;

            Console.WriteLine("Welcome to Uno Dos!");
            Console.WriteLine("\nPlease enter your desired username to continue...");
            Player.PlayerName = Console.ReadLine();
            Console.Clear();

            while (_GameOption != END_GAME_OPTION)
            {
                Console.WriteLine("Please Select an option below: \n\n1)Play Game \n2)View Rules \n3)Exit Game ");
                int.TryParse(Console.ReadLine(), out _GameOption);
                switch (_GameOption)
                {
                    case 1:
                        Console.Clear();
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
                        if (CPU.Cards.Count == 0)
                        {
                            Console.WriteLine("Unlucky! CPU Won!");
                        }
                        break;
                    case 2:
                        DisplayRules();
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("Thanks for playing! Press enter to close the window...");
                        Console.ReadLine();
                        break;
                    default:
                        Console.WriteLine("Invalid Option, Please Select again!");
                        break;
                }
            }
            Environment.Exit(0);
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
                        Console.Clear();
                        DisplayPlayerHand();
                        break;
                    case 2:
                        Console.Clear();
                        List<bool> _PlayableCards = new();
                        Player.Cards.ForEach(card =>
                        {
                            _PlayableCards.Add(Player.CanPlayCard(card, __Deck.LastCardPlayed));
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
                            Console.WriteLine("\nPlease Select a card from your hand. You can do this by typing in the index of the card you want to play. eg: To play the first card in your hand, you type 1 :");
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

        private void PlayerTurn()
        {
            PlayerOptions();
            while (Player.IsResetCardPlayed && Player.Cards.Count > 0)
            {
                Player.IsResetCardPlayed = false;
                Console.WriteLine("\nYou played a reset card! Take another turn!");
                PlayerOptions();
            }
            if (Player.IsLoseTwoCardPlayed)
            {
                PlayerSelectTwoCardsToLose();
            }

            if (Player.IsSwapDeckPlayed  && Player.Cards.Count > 0)
            {
                KeyValuePair<List<ICard>, List<ICard>> _SwappedCards = Player.SwapCards(new KeyValuePair<List<ICard>, List<ICard>>(Player.Cards, CPU.Cards));
                Player.Cards = _SwappedCards.Key;
                CPU.Cards = _SwappedCards.Value;
            }
        }

        private void PlayerSelectTwoCardsToLose()
        {
            List<ICard> _CardsToRemove = new List<ICard>();

            if(Player.Cards.Count < 2)
            {
                Console.WriteLine("\nYou have less than two cards in your hand! This will now clear your hand!");
                _CardsToRemove = Player.Cards;
            }
            else
            {
                int _CardsSelected = 0;
                Console.Clear();
                Console.WriteLine("\nYou played a Lose Two card! Please select two cards to lose");
                DisplayPlayerHand();
                while (_CardsSelected < 2)
                {
                    Console.WriteLine(string.Format("\nCard {0}:", _CardsSelected + 1));
                    int _SelectedCardPosition = CheckCardSelectedIsValid(Console.ReadLine());
                    if (_SelectedCardPosition > 0)
                    {
                        ICard _CardToRemove = Player.Cards[_SelectedCardPosition - 1];
                        if (_CardsToRemove.Contains(_CardToRemove))
                        {
                            Console.WriteLine("\nYou have already selected this card! Please select another!");
                        }
                        else
                        {
                            _CardsToRemove.Add(Player.Cards[_SelectedCardPosition - 1]);
                            _CardsSelected++;
                        }
                    }
                }
            }

            __Deck = Player.LoseTwoCards(_CardsToRemove, Deck);
        }

        private void ShowLastCardPlayedToString()
        {
            Console.WriteLine(string.Format("\n\nThe current card showing on the table is {0}", __Deck.LastCardPlayed.ToString()));
        }

        private bool TryPlayCard(string _SelectedCardInput)
        {
            int _SelectedCardPosition = CheckCardSelectedIsValid(_SelectedCardInput);

            if (_SelectedCardPosition > 0)
            {
                ICard _SelectedCard = Player.Cards[_SelectedCardPosition - 1];

                if (!Player.CanPlayCard(_SelectedCard, Deck.LastCardPlayed))
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

        private ICPU CPU => __CPU = __CPU ?? new CPU();
        private IDeck Deck => __Deck = __Deck ?? new Deck();
        private IPlayer Player => __Player = __Player ?? new Player();
    }
}
