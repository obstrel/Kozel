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

        public Trick Trick {  get { return trick; } }

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
        public event EventHandler<RoundFinishedEventArgs> RoundFinished;
        public event EventHandler<PlayerEventArgs> RoundStarted;

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

        public Team Start(Player startRoundPlayer) {
            TrumpSuit = CardSuit.Diamond;
            activePlayer = startRoundPlayer == null ? GetActivePlayer() : FindPlayerIndexByPlayer(startRoundPlayer);
            SetTrumpCardsAndInitPlayers(Players);
            SortCards();
            if(RoundStarted != null) {
                RoundStarted(this, new PlayerEventArgs(ActivePlayer));
            }
            return teams[0];
        }

        private int FindPlayerIndexByPlayer(Player startRoundPlayer) {
            return Players.FindIndex(p => { return p == startRoundPlayer; });
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
            if (ActivePlayerChanged != null && trick.Cards.Count != 4) {
                ActivePlayerChanged(this, new PlayerEventArgs(ActivePlayer));
            }
            if(trick.Cards.Count == 4) {
                if (ActivePlayerChanged != null) {
                    ActivePlayerChanged(this, new PlayerEventArgs(null));
                }
                SetTrickOwner();
                if (RoundFinished != null) {
                    RoundFinished(this, new RoundFinishedEventArgs(trick.GetTrickWinner(), trick.Owner));
                }
             }
        }




        #region PRIVATE METHODS ----------------------------------------------------------------------------------------

        private void SetTrickOwner() {
            Player winner = trick.GetTrickWinner();
            Trick.Owner = (Team1.Player1 == winner) || (Team1.Player2 == winner) ? Team1 : Team2;
            Trick.Owner.AddTrick(trick);
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
                if (Players[i].Cards.Any(c => { return c.Suit == CardSuit.Diamond && c.Value == CardValue.Six; }))
                    return i;
            }
            return 0;
        }

 

        #endregion ---------------------------------------------------------------------------------------
    }
}
