using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kozel.AI;

namespace Kozel {
    public class Player {
        List<Card> cards = new List<Card>(8);
        IAI ai;

        public List<Card> Cards { get { return cards; } }
        public bool Trumped { get; set; }
        public Team Team { get; internal set; }
        public bool IsAI { get { return ai != null; } }

        public event EventHandler<PlayerMadeMoveEventArgs> PlayerMadeMove;
        public event EventHandler<PlayerEventArgs> CardsResorted;

        public Player() : base() {
        }

        public Player(IAI ai) : this() {
            this.ai = ai;
            this.ai.Player = this;
        }

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

        public bool CanThrowCard(Trick trick, Card card) {

            if (ai != null && ai.Trick == null) {
                ai.Trick = trick;
            }

            if (trick.Cards.Count == 0) {
                if (!Team.Trumped && Cards.Exists(c => { return !c.IsTrump; }) && Cards.Exists(c => { return !c.IsTrump; }) && card.IsTrump) {
                    return false;
                }
                return true;
            }
            if (trick.Cards[0].IsTrump) {
                if (Cards.Exists(c => { return c.IsTrump; }) && !card.IsTrump) {
                    return false;
                }
                return true;
            }
            if (Cards.Exists(c => { return c.Suit == trick.Cards[0].Suit && !c.IsTrump; }) && (card.Suit != trick.Cards[0].Suit || card.IsTrump)) {
                return false;
            }
            return true;
        }

        private Card GetBestCard(CardSuit suit) {
            if(ai == null) {
                throw new ArgumentOutOfRangeException("AI is null!");
            }
            return ai.FindBestCard(Cards);
        }

    }
}
