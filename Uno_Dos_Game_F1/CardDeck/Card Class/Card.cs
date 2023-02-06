using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDeck
{
    public class Card
    {
		public enum CardColor
		{
			//Name of colors
			//getters and setters for the colors
			Pink, Green, Orange, Purple, Wild

			//private static final List<Color> colors = Enum.GetValues(typeof(Color));
		}

		public enum CardValue
		{
			Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Switch, LoseTwo, SwapDeck, WildCard
		}

		public CardColor Color { get; set; }
		public CardValue Value { get; set; }
		public int Score { get; set; }

		// add an override toString method
		public String toString()
        {
			return Color + " " + Value + " " + Score;
        }
	}
}
