using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDeck
{
    public class Deck : Card
    {
		public List<Card> deck;


		public List<Card> CreateDeck()
		{
			deck = new List<Card>();
			/*
			List<int> values = new List<int>()
			{
				0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 20, 20, 20, 50, 50 ,50
			};

			List<CardColor> colors = new List<CardColor>();

			int index = 0;

			foreach (CardColor color in Enum.GetValues(typeof(CardColor)))
            {
				if (color != CardColor.Wild)
                {
                    //colors[index++] = color;	

					for (int i = 0; i < 9; i++)
                    {
						colors[index++] = color;
						colors[index++] = color;
                    }

					for (int i = 0; i < 3; i++)
                    {
						colors[index++] = color;
						colors[index++] = color;
					}
                }

                else
                {
					for (int i = 0; i < 4; i++)
                    {
						colors[index++] = color;
                    }
                }
            }

			for (int i = 0; i < 19; i++)
            {
				deck.Add(new Card() { Color = colors[i], 
					                  Value = (CardValue)values[i], 
					                  Score = values[i] });
            }
			*/
			foreach (CardColor color in Enum.GetValues(typeof(CardColor)))
			{
				if (color != CardColor.Wild)
				{
					foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
					{
						switch (value)
						{
							case CardValue.One:
							case CardValue.Two:
							case CardValue.Three:
							case CardValue.Four:
							case CardValue.Five:
							case CardValue.Six:
							case CardValue.Seven:
							case CardValue.Eight:
							case CardValue.Nine:
								//Two copies of each color from 1 to 9
								deck.Add(new Card() { Color = color, Value = value, Score = (int)value });
								deck.Add(new Card() { Color = color, Value = value, Score = (int)value });
								break;
							case CardValue.Switch:
							case CardValue.LoseTwo:
							case CardValue.SwapDeck:
								//Add two copies Switch and LoseTwo and SwapDeck for each Color
								deck.Add(new Card() { Color = color, Value = value, Score = 20 });
								deck.Add(new Card() { Color = color, Value = value, Score = 20 });
								break;
							//Add one copy of zero for each color
							case CardValue.Zero:
								deck.Add(new Card() { Color = color, Value = value, Score = 0 });
								break;
						}
					}

				}
				else //Create the wild cards (these will be see through cards hence will have no color)
				{
					//Add four wild cards
					for (int i = 0; i <= 4; i++)
					{
						deck.Add(new Card() { Color = color, Value = CardValue.WildCard, Score = 50 });
					}
				}

			}

			return deck;
		}

		public List<Card> getCards
		{
			get { return deck; }
		}

		// Shuffle Card Method
		public void Shuffle()
		{
			Random random = new Random();

			List<Card> cards = deck;

			for (int i = cards.Count - 1; i >= 0; --i)// may need to check this for condition
			{
				int j = random.Next(i + 1);
				Card temp = cards[i];
				cards[i] = cards[j];
				cards[j] = temp;

			}

		}

		// Draw Card Method
		public List<Card> DrawCards(int count)
		{
			if (count <= 0)
			{
				throw new ArgumentOutOfRangeException("count", "count must be greater than 0");
			}

			if (count > deck.Count)
			{
				throw new InvalidOperationException("Not enough cards in deck");
			}

			List<Card> drawnCards = deck.Take(count).ToList();

			//Remove the drawn cards from the draw pile
			deck.RemoveAll(card => drawnCards.Contains(card));
			return drawnCards;

		}
	}
}
