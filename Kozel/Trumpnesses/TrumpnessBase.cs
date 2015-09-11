using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kozel.Trumpnesses {

    abstract public class TrumpnessBase {
        protected Queue<Card> deck;
        protected List<Player> players;

        abstract public string Name { get; }

        abstract protected void TrumpPlayer(Player player);

        public TrumpnessBase(Queue<Card> deck, List<Player> players) {
            this.deck = deck;
            this.players = players;
        }

        public virtual void Start() {
            Player trumpedPlayer = players.Find(p => { return p.Trumped; });
            TrumpPlayer(trumpedPlayer);
            DealRestCards(trumpedPlayer);
        }

        protected void DealRestCards(Player trumpedPlayer) {
            int trumpedPlayerIndex = players.IndexOf(trumpedPlayer);
            int playerIndex = trumpedPlayerIndex == 3 ? 0 : trumpedPlayerIndex + 1;
            
            for (int i = 0; i < trumpedPlayer.Cards.Count; i++) {
                for (int j = 0; j < 3; j++) {
                    players[playerIndex].AddCard(deck.Dequeue());
                    playerIndex = playerIndex == 3 ? 0 : playerIndex + 1;
                }
            }
            Card card;
            while ((card = deck.Dequeue()) != null) {
                players[playerIndex].AddCard(card);
                playerIndex = playerIndex == 3 ? 0 : playerIndex + 1;
            }
        }

        protected void SetTrumpCards(CardSuit trumpSuit) {
            foreach (Card card in deck) {
                card.IsTrump = card.Suit == trumpSuit;
            }
        }


    }
}
