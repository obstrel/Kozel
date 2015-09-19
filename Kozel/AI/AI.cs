using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kozel.AI {
    public interface IAI {
        Player Player { get; set; }
        Trick Trick { get; set; }

        Card FindBestCard(List<Card> cards);
    }

    public class StupidAI : IAI {
        public Player Player { get; set; }
        public Trick Trick { get; set; }

        public StupidAI() : base() {

        }

        public Card FindBestCard(List<Card> cards) {
            List<Card> validCards = cards.FindAll(c => { return Player.CanThrowCard(Trick, c); });
            Random r = new Random();
            return validCards[r.Next(validCards.Count - 1)];
        }
    }
}
