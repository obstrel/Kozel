using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kozel.Trumpnesses;

namespace Kozel {
    public class Game {
        private List<Player> players;
        private List<Round> rounds = new List<Round>(8);
        private int activeRound = 0;
        private Team Team1 { get { return players[0].Team; } }
        private Team Team2 { get { return players[1].Team; } }
        private Trumpness trumpness;

        public Round ActiveRound { get { return rounds[activeRound]; } }
        

        public event EventHandler<PlayerEventArgs> ActivePlayerChanged;
        public event EventHandler<RoundFinishedEventArgs> RoundFinished;
        public event EventHandler<PlayerEventArgs> RoundStarted;
        public event EventHandler<GameFinishedEventArgs> GameFinished;

        public Game(List<Player> players) {
            this.players = players;
            for (int i = 0; i < 8; i++) {
                rounds.Add(new Round(players));
                Round lastRound = rounds.Last();
                lastRound.ActivePlayerChanged += Round_ActivePlayerChanged;
                lastRound.RoundFinished += Round_RoundFinished;
                lastRound.RoundStarted += Round_RoundStarted;
                lastRound.ShohaCatchQueen += LastRound_ShohaCatchQueen;
            }
        }

        public void Start() {
            Queue<Card> deck = new Queue<Card>(32);
            FillDeck(deck);

            CardSuit trumpSuit = DealCards(deck, this.players);
            ActiveRound.Start(null, trumpSuit);
        }

        private void LastRound_ShohaCatchQueen(object sender, ShohaCatchQueenEventArgs e) {
            e.ShohaOwner.Team.GameScore += 4;
            Stop(e.ShohaOwner);
        }

        private void Round_RoundStarted(object sender, PlayerEventArgs e) {
            if (RoundStarted != null) {
                RoundStarted(this, e);
            }
        }

        private void Round_RoundFinished(object sender, RoundFinishedEventArgs e) {
            if (activeRound >= 7) {
                Team1.GameScore += Team2.Score < 30 ? Team2.Score == 0 ? 6 : 4 : Team2.Score < 60 ? 2 : 0;
                Team2.GameScore += Team1.Score < 30 ? Team1.Score == 0 ? 6 : 4 : Team1.Score < 60 ? 2 : 0;
            }
            if (RoundFinished != null) {
                RoundFinished(this, e);
            }
            if (activeRound < 7) {
                activeRound++;
                ActiveRound.Start(e.LastRoundWinner);
            }
            else {
                Stop(e.LastRoundWinner);
            }

        }

        private void Round_ActivePlayerChanged(object sender, PlayerEventArgs e) {
            if (ActivePlayerChanged != null) {
                ActivePlayerChanged(this, e);
            }
        }

        private void Stop(Player lastRoundWinner) {
            Player trumpedPlayer = players.Find(p => { return p.Trumped; });
            trumpedPlayer.Trumped = false;
            int index = players.IndexOf(trumpedPlayer);
            index = index == 3 ? 0 : index + 1;
            players.ForEach(p => { p.Cards.Clear(); });
            players[index].Trumped = true;
            if (GameFinished != null) {
                GameFinished(this, new GameFinishedEventArgs(lastRoundWinner));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deck"></param>
        /// <param name="players"></param>
        private CardSuit DealCards(Queue<Card> deck, List<Player> players) {
            CardSuit trumpSuit = CardSuit.Diamond;
            if (players.Exists(p => { return p.Trumped; })) {
                trumpness = new Trumpness(deck, players);
                trumpness.Start();
                trumpSuit = trumpness.TrumpSuit;
            }
            else {
                for (int i = 0; i < 8; i++) {
                    for (int j = 0; j < 4; j++) {
                        players[j].AddCard(deck.Dequeue());
                    }
                }
            }
            return trumpSuit;
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

    }
}
