using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kozel {
    public class Player {
        List<Card> cards = new List<Card>(8);

        public List<Card> Cards { get { return cards; } }
        public bool Trumped { get; set; }
        public Team Team { get; internal set; }

        public event EventHandler<PlayerMadeMoveEventArgs> PlayerMadeMove;
        public event EventHandler<PlayerEventArgs> CardsResorted;

        public void AddCard(Card card) {
            cards.Add(card);
            //    SortCards();
        }

        public Card ThrowCard() {
            Card card = GetBestCard(CardSuit.Club);
            return ThrowCard(card);
        }

        public Card ThrowCard(Card card) {
            cards.Remove(card);
            if (PlayerMadeMove != null)
                PlayerMadeMove(this, new PlayerMadeMoveEventArgs(this, card));
            return card;
        }


        public void SortCards() {
            cards.Sort();
            if (CardsResorted != null) {
                CardsResorted(this, new PlayerEventArgs(this));
            }
        }

        private Card GetBestCard(CardSuit suit) {
            return cards.Find(c => c.Suit == suit);
        }

        //public IEnumerable<Card> GetCards() {
        //    return cards as IEnumerable<Card>;
        //}
    }
}
