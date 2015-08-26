using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kozel
{
    public class Round
    {
        private CardSuit? trumpSuit = null;
        private List<Card> cards = new List<Card>(4);


        public CardSuit TrumpSuit
        {
            get
            {
                if (trumpSuit == null)
                {
                    throw new ArgumentOutOfRangeException("TrumpSuit");
                }
                return (CardSuit)trumpSuit;
            }

            set
            {
                trumpSuit = value;
            }
        }

        public void AddCard(Card card)
        {
            cards.Add(card);
        }

        public void SetTrumpCards(List<Player> players)
        {
            foreach (Player player in players)
            {
                foreach (Card card in player.GetCards())
                {
                    card.IsTrump = card.Suit == TrumpSuit;
                }
            }
        }

    }
}
