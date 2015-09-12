using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kozel.Trumpnesses {

    abstract public class TrumpnessBase {
        protected Player TrumpedPlayer { get { return players.Find(p => { return p.Trumped; }); } }

        protected Queue<Card> deck;
        protected List<Player> players;

        abstract public string Name { get; }

        abstract protected void TrumpPlayer();

        public TrumpnessBase(Queue<Card> deck, List<Player> players) {
            this.deck = deck;
            this.players = players;
        }

        public virtual void Start() {
            TrumpPlayer();
            DealCardsAfterTrump();
            DealRestCards();
        }

        protected virtual void DealRestCards() {
            int playerIndex = GetPlayerIndexForDealRestCards();

            Card card;
            while (deck.Count > 0) {
                card = deck.Dequeue();
                players[playerIndex].AddCard(card);
                playerIndex = playerIndex == 3 ? 0 : playerIndex + 1;
            }
        }

        protected virtual int GetPlayerIndexForDealRestCards() {
            return players.IndexOf(TrumpedPlayer);
        }

        protected virtual void DealCardsAfterTrump() {
            int trumpedPlayerIndex = players.IndexOf(TrumpedPlayer);
            int playerIndex = trumpedPlayerIndex == 3 ? 0 : trumpedPlayerIndex + 1;
            for (int i = 0; i < TrumpedPlayer.Cards.Count; i++) {
                for (int j = 0; j < 3; j++) {
                    if (players[playerIndex] != TrumpedPlayer) {
                        players[playerIndex].AddCard(deck.Dequeue());
                    }
                    playerIndex = playerIndex == 3 ? 0 : playerIndex + 1;
                }
            }
        }

        protected void SetTrumpCards(CardSuit trumpSuit) {
            foreach (Card card in deck) {
                card.IsTrump = card.Suit == trumpSuit;
            }
        }


    }
}
