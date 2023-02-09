using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoDos.Decks.Entities;
using UnoDos.Players.Entities;

namespace UnoDos.PlayGame
{
    public class Game
    {
        private const int INITIAL_DRAW_COUNT = 10;

        private Deck __Deck;
        private Deck __PlayedCards;
        private Player __Player;

        public Game() { }

        public void InitialiseGame()
        {
            __Deck = new Deck();
            __Player = new Player();

            __Deck.CreateDeck();
            __Player.Cards = __Deck.DrawCards(INITIAL_DRAW_COUNT);
        }

        public void PlayGame()
        {
            Console.WriteLine("Welcome to Uno Dos!");
            Console.WriteLine("\nPlease press any key to continue...");
            Console.ReadLine();
        }
    }
}
