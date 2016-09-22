using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedOrBlack
{
    public class CardDeck
    {

        public CardDeck()
        {
            Cards = new List<Card>();
            int cardId = 1;
            int topSuitCard = 14;

            for (int i = 1; i < 5; i++)
            {
                CardDeck.Suits suit = (CardDeck.Suits)i;
                for (int x = 1; x < topSuitCard; x++)
                {
                    Cards.Add(AddCardToDeck(suit, x, cardId));
                    cardId++;
                }
            }

        }

        public Card SelectedCard { get; set; }

        public Stages Stage { get; set; }

        public enum Stages
        {
            Start=0,
            Color=1,
            Suit=2,
            Face=3,
            ChickOrDude=4,
            OddOrEven=5,
            OverUnder=6,
            Assertion=7,
            GameOver=8
        }

        public enum Suits
        {
            Spades = 1,
            Hearts = 2,
            Diamonds = 3,
            Clubs = 4
        }

        public enum CardValues
        {
            Ace = 1,
            Two= 2,
            Three = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Ten = 10,
            Jack = 11,
            Queen = 12,
            King = 13
        }

        public List<Card> Cards { get; set; }

        private Card AddCardToDeck(CardDeck.Suits suit, int value, int id)
        {

            Card retval = new Card { Id = id, Suit = suit, Value = value };
            retval.IsBlack = (retval.Suit == CardDeck.Suits.Spades) || (retval.Suit == CardDeck.Suits.Clubs);
            retval.IsFace = (retval.Value > 10 || retval.Value==1);
            retval.IsMale = (retval.Value == (int)CardValues.Jack || retval.Value == (int)CardValues.King);
            retval.DescriptionShort = ((CardValues)value).ToString();
            retval.IsOdd = !(retval.Value % 2 == 0);
            return retval;
        }

        public void ShuffleAndSelect()
        {
            Random random = new Random(); 
            int iDValue= random.Next(1, 52);

            this.SelectedCard = Cards.FirstOrDefault(c => c.Id == iDValue);
           // this.SelectedCard = Cards.FirstOrDefault(c => c.Id == 12);
            this.Stage = Stages.Start;
        }
    }
}
