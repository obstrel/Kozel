using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public SimpleTrumpness(Queue<Card> deck, ObservableCollection<Player> players) : base(deck, players) { }

        protected override void TrumpPlayer() {
            Card card = deck.Dequeue();

            while (card.IsTrump) {
                TrumpedPlayer.AddCard(card);
                card = deck.Dequeue();
            }
            TrumpedPlayer.AddCard(card);
            card.IsTrump = true;
            SetTrumpCards(card.Suit);
        }


    }
}
