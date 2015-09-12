using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kozel.Trumpnesses {
    public class NoTrumpness : TrumpnessBase {

        public override string Name {
            get {
                return "No trumpness";
            }
        }

        public NoTrumpness(Queue<Card> deck, List<Player> players) : base(deck, players) { }

        protected override void TrumpPlayer() {
            SetTrumpCards(CardSuit.Diamond);
        }

        protected override void DealCardsAfterTrump() { }

        protected override int GetPlayerIndexForDealRestCards() {
            return 0;
        }

        protected override void DealRestCards() {
            base.DealRestCards();
            SetTrumpedPlayer();
        }

        private void SetTrumpedPlayer() {
            for (int i = 0; i < 4; i++) {
                if (players[i].Cards.Exists(c => { return c.Suit == CardSuit.Diamond && c.Value == CardValue.Six; })) {
                    players[i].Trumped = true;
                    return;
                }
            }
        }
    }
}
