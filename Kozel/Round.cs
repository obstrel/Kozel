using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kozel {
    public class Round {

        private Trick trick = new Trick();
        private List<Player> players;
        private int activePlayer = 0;
        private bool finishing = false;

        public Trick Trick {  get { return trick; } }

        public CardSuit TrumpSuit {
            get {
                List<Card> cards = new List<Card>();
                players.ForEach(p => { cards.AddRange(p.Cards); });
                Card trumpCard = cards.First(c => { return c.IsTrump; });
                if (trumpCard == null) {
                    throw new ArgumentOutOfRangeException("TrumpSuit is null!");
                }
                return trumpCard.Suit;
            }
        }

        public Player ActivePlayer { get { return players[activePlayer]; } }

        public List<Player> Players {
            get { return players; }
        }

        public event EventHandler<PlayerEventArgs> ActivePlayerChanged;
        public event EventHandler<RoundFinishedEventArgs> RoundFinished;
        public event EventHandler<PlayerEventArgs> RoundStarted;
        public event EventHandler<ShohaCatchQueenEventArgs> ShohaCatchQueen;

        public Round(List<Player> players) {
            this.players = players;
            this.trick.ShohaCatchQueen += Trick_ShohaCatchQueen;
        }

        private void Trick_ShohaCatchQueen(object sender, ShohaCatchQueenEventArgs e) {
            finishing = true;
            if(ShohaCatchQueen != null) {
                ShohaCatchQueen(this, e);
            }
        }

        public bool CanThrowCard(Player player, Card card) {
            if(trick.Cards.Count == 0) {
                if(!player.Team.Trumped && player.Cards.Exists(c => { return !c.IsTrump; }) && player.Cards.Exists(c => { return !c.IsTrump; }) &&  card.IsTrump) {
                    return false;
                }
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

        public void Start(Player startRoundPlayer) {
            if(startRoundPlayer == null) {
                activePlayer = GetActivePlayer();
                ActivePlayer.Trumped = true;
            }
            else {
                activePlayer = FindPlayerIndexByPlayer(startRoundPlayer);
            }
            
            SortCards();
            if(RoundStarted != null) {
                RoundStarted(this, new PlayerEventArgs(ActivePlayer));
            }
        }

        private int FindPlayerIndexByPlayer(Player startRoundPlayer) {
            return Players.FindIndex(p => { return p == startRoundPlayer; });
        }


        public void NextMove(Card card) {
            ActivePlayer.ThrowCard(card);
            AddCardToTrick(ActivePlayer, card);
            if (!finishing) {
                activePlayer = activePlayer < 3 ? activePlayer + 1 : 0;

                if (ActivePlayerChanged != null && trick.Cards.Count != 4) {
                    ActivePlayerChanged(this, new PlayerEventArgs(ActivePlayer));
                }
                if (trick.Cards.Count == 4) {
                    if (ActivePlayerChanged != null) {
                        ActivePlayerChanged(this, new PlayerEventArgs(null));
                    }
                    SetTrickOwner();
                    if (RoundFinished != null) {
                        RoundFinished(this, new RoundFinishedEventArgs(trick.GetTrickWinner(), trick.Owner));
                    }
                }
            }
        }




        #region PRIVATE METHODS ----------------------------------------------------------------------------------------

        private void SetTrickOwner() {
            Player winner = trick.GetTrickWinner();
            Trick.Owner = winner.Team;
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
            if(Players.Exists(p => { return p.Trumped; })) {
                return Players.FindIndex(p => { return p.Trumped; });
            }
            for (int i = 0; i < 4; i++) {
                if (Players[i].Cards.Any(c => { return c.Suit == CardSuit.Diamond && c.Value == CardValue.Six; }))
                    return i;
            }
            return 0;
        }

 

        #endregion ---------------------------------------------------------------------------------------
    }
}
