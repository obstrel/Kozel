using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kozel
{
    public enum CardSuit
    {
        Diamond = 1,
        Heart = 16,
        Spade = 31,
        Club = 46
    }

    public enum CardValue { 
        Six = -3,
        Seven = -2,
        Eight = -1,
        Nine = 0,
        King = 4,
        Ten = 10,
        Ace = 11,
        Jack = 2,
        Queen = 3,
    }

    public class Card : IComparable
    {
        private CardSuit suit;
        private CardValue value;
        private bool isTrump;

        public CardSuit Suit { get { return suit; } }
        public CardValue Value { get { return value; } }

        public bool IsTrump
        {
            get
            {
                return IsPermanentTrump() || isTrump;
            }

            set
            {
                isTrump = value;
            }
        }

        public override int GetHashCode()
        {
            return Suit.GetHashCode() + Value.GetHashCode() + GetTrumpValue();
        }

        public Card(CardSuit aSuite, CardValue aValue)
        {
            suit = aSuite;
            value = aValue;
        }

        private int GetTrumpValue()
        {
            return IsTrump ? IsPermanentTrump() ? isShoha() ? 1000 : Value.GetHashCode() * 60 : 58 : 0;
        }

        private bool isShoha()
        {
            return (Suit == CardSuit.Club && Value == CardValue.Six);
        }

        private bool IsPermanentTrump()
        {
            return Value == CardValue.Jack || Value == CardValue.Queen || isShoha();
        }

        int IComparable.CompareTo(object obj)
        {
            return this.GetHashCode() > obj.GetHashCode() ? 1 : this.GetHashCode() == obj.GetHashCode() ? 0 : -1;
        }
    }
}
