using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kozel.Trumpnesses {
    public class Trumpness {
        private Queue<Card> deck;
        private List<Player> players;
        public virtual string Name { get { return "Simple"; } }

        public CardSuit TrumpSuit { get; internal set; }

        public Trumpness(Queue<Card> deck, List<Player> players) {
            this.deck = deck;
            this.players = players;
        }

        public void Start() {
            Player trumpedPlayer = players.Find(p => { return p.Trumped; });
            Card card = deck.Dequeue();
            int cardCount = 0;

            while (card.IsTrump) {
                trumpedPlayer.AddCard(card);
                card = deck.Dequeue();
                cardCount++;
            }
            trumpedPlayer.AddCard(card);
            card.IsTrump = true;
            this.TrumpSuit = card.Suit;
            int trumpedPlayerIndex = players.IndexOf(trumpedPlayer);
            int playerIndex = trumpedPlayerIndex == 3 ? 0 : trumpedPlayerIndex + 1;
            for (int i = 0; i < cardCount; i++) {
                for (int j = 0; j < 3; j++) {
                    players[playerIndex].AddCard(deck.Dequeue());
                    playerIndex = playerIndex == 3 ? 0 : playerIndex + 1;
                }
            }
            while ((card = deck.Dequeue()) != null) {
                players[playerIndex].AddCard(card);
                playerIndex = playerIndex == 3 ? 0 : playerIndex + 1;
            }

        }
    }
}
