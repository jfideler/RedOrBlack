using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedOrBlack
{
    public class Card
    {
        public int Id { get; set; }
        public CardDeck.Suits Suit { get;set; }
        public bool IsFace { get; set; }
        public bool IsBlack { get; set; }
        public bool IsOdd { get; set; }
        public bool IsMale { get; set; }
        public int Value { get; set; }
        public string DescriptionShort { get; set; }
               
        public string DescriptionLong
        {
            get
            {
                return DescriptionShort + " of " + this.Suit.ToString();
            }
        }
    }
}
