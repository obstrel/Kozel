using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kozel {
    public class Round {
        private CardSuit? trumpSuit = null;
        private Trick trick = new Trick();
        private List<Team> teams;
        private List<Player> players;
        private int activePlayer = 0;

        public CardSuit TrumpSuit {
            get {
                if (trumpSuit == null) {
                    throw new ArgumentOutOfRangeException("TrumpSuit is null!");
                }
                return (CardSuit)trumpSuit;
            }

            set {
                trumpSuit = value;
            }
        }

        public Player ActivePlayer { get { return players[activePlayer]; } }
        public Team Team1 { get { return teams[0]; } }
        public Team Team2 { get { return teams[1]; } }

        public List<Player> Players {
            get {
                if (players == null)
                    players = new List<Player>(4) { Team1.Player1, Team2.Player1, Team1.Player2, Team2.Player2 };
                return players;
            }
        }

        public event EventHandler<PlayerEventArgs> ActivePlayerChanged;

        public Round(Team team1, Team team2) {
            teams = new List<Team>(2) { team1, team2 };
        }

        public bool CanThrowCard(Player player, Card card) {
            if(trick.Cards.Count == 0) {
                return true;
            }
            if(trick.Cards[0].IsTrump) { 
                if(player.Cards.Exists(c => { return c.IsTrump; }) && !card.IsTrump) {
                    return false;
                }
                return true;
            }
            if(player.Cards.Exists(c => { return c.Suit == trick.Cards[0].Suit && !c.IsTrump; }) && (card.Suit != trick.Cards[0].Suit || card.IsTrump)) {
                return false;
            }
            return true;
        }

        public Team Start() {
            Queue<Card> deck = new Queue<Card>(32);
            FillDeck(deck);

            TrumpSuit = CardSuit.Diamond;
            DealCards(deck, Players);

            activePlayer = GetActivePlayer();
            SetTrumpCardsAndInitPlayers(Players);
            SortCards();

            return teams[0];
        }

        public void SetTrumpCardsAndInitPlayers(List<Player> players) {
            foreach (Player player in players) {
                foreach (Card card in player.Cards) {
                    card.IsTrump = card.Suit == TrumpSuit;
                }
                player.Round = this;
            }
        }

        public void NextMove(Card card) {
            ActivePlayer.ThrowCard(card);
            AddCardToTrick(ActivePlayer, card);
            if (activePlayer < 3) {
                activePlayer++;
            }
            else {
                activePlayer = 0;
            }
            if (ActivePlayerChanged != null) {
                ActivePlayerChanged(this, new PlayerEventArgs(ActivePlayer));
            }
        }



        #region PRIVATE METHODS ----------------------------------------------------------------------------------------

        private void SetTrickOwner() {
            //Trick.Cards.
        }

        private void AddCardToTrick(Player player, Card card) {
            trick.AddMove(card, player);
        }

        /// <summary>
        /// 
        /// </summary>
        private void SortCards() {
            foreach (Player p in players) {
                p.SortCards();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int GetActivePlayer() {
            for (int i = 0; i < 4; i++) {
                if (players[i].Cards.Any(c => { return c.Suit == CardSuit.Diamond && c.Value == CardValue.Six; }))
                    return i;
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deck"></param>
        /// <param name="players"></param>
        private void DealCards(Queue<Card> deck, List<Player> players) {
            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 4; j++) {
                    players[j].AddCard(deck.Dequeue());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deck"></param>
        private void FillDeck(Queue<Card> deck) {
            List<Card> cardList = new List<Card> {
                new Card(CardSuit.Diamond, CardValue.Six), new Card(CardSuit.Heart, CardValue.Six), new Card(CardSuit.Spade, CardValue.Six), new Card(CardSuit.Club, CardValue.Six),
                new Card(CardSuit.Diamond, CardValue.Eight), new Card(CardSuit.Heart, CardValue.Eight), new Card(CardSuit.Spade, CardValue.Eight), new Card(CardSuit.Club, CardValue.Eight),
                new Card(CardSuit.Diamond, CardValue.Nine), new Card(CardSuit.Heart, CardValue.Nine), new Card(CardSuit.Spade, CardValue.Nine), new Card(CardSuit.Club, CardValue.Nine),
                new Card(CardSuit.Diamond, CardValue.King), new Card(CardSuit.Heart, CardValue.King), new Card(CardSuit.Spade, CardValue.King), new Card(CardSuit.Club, CardValue.King),
                new Card(CardSuit.Diamond, CardValue.Ten), new Card(CardSuit.Heart, CardValue.Ten), new Card(CardSuit.Spade, CardValue.Ten), new Card(CardSuit.Club, CardValue.Ten),
                new Card(CardSuit.Diamond, CardValue.Ace), new Card(CardSuit.Heart, CardValue.Ace), new Card(CardSuit.Spade, CardValue.Ace), new Card(CardSuit.Club, CardValue.Ace),
                new Card(CardSuit.Diamond, CardValue.Jack), new Card(CardSuit.Heart, CardValue.Jack), new Card(CardSuit.Spade, CardValue.Jack), new Card(CardSuit.Club, CardValue.Jack),
                new Card(CardSuit.Diamond, CardValue.Queen), new Card(CardSuit.Heart, CardValue.Queen), new Card(CardSuit.Spade, CardValue.Queen), new Card(CardSuit.Club, CardValue.Queen),
            };

            Random rnd = new Random();

            while (cardList.Count > 0) {
                int index = rnd.Next(cardList.Count - 1);
                deck.Enqueue(cardList[index]);
                cardList.RemoveAt(index);
            }

        }
        #endregion ---------------------------------------------------------------------------------------
    }
}
