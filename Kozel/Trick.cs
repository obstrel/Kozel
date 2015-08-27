using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kozel {
    public class Trick {
        public Team Owner { get; set; }
        public List<Card> Cards {
            get {
                return new List<Card>(moves.Keys);
            }
        }

        private Dictionary<Card, Player> moves = new Dictionary<Card, Player>(4);
        private bool HasTrump {
            get {
                return moves.Keys.Any(c => { return c.IsTrump; });
            }
        }

        public void AddMove(Card card, Player player) {
            moves.Add(card, player);
        }

        public Player GetTrickOwner() {
            if(HasTrump)
                return moves[moves.Keys.Max()];

            return moves[moves.Keys.Where(c => { return c.Suit == moves.First().Key.Suit; }).Max()];
        }
    }
}
