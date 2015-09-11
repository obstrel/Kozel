using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kozel.Trumpnesses {
    public class SimpleTrumpness : TrumpnessBase {

        public override string Name {
            get {
                return "Simple";
            }
        }

        public SimpleTrumpness(Queue<Card> deck, List<Player> players) : base(deck, players) { }

        protected override void TrumpPlayer(Player player) {
            Card card = deck.Dequeue();
            int cardCount = 0;

            while (card.IsTrump) {
                player.AddCard(card);
                card = deck.Dequeue();
                cardCount++;
            }
            player.AddCard(card);
            card.IsTrump = true;
            SetTrumpCards(card.Suit);
        }


    }
}
