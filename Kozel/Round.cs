﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kozel {
    public class Round {

        private Trick trick = new Trick();
        private ObservableCollection<Player> players;
        private int activePlayer = 0;
        private bool finishing = false;

        public Trick Trick {  get { return trick; } }

        public CardSuit TrumpSuit {
            get {
                List<Card> cards = new List<Card>();
                players.ToList().ForEach(p => { cards.AddRange(p.Cards); });
                Card trumpCard = cards.First(c => { return !c.IsPermanentTrump() && c.IsTrump; });
                if (trumpCard == null) {
                    throw new ArgumentOutOfRangeException("TrumpSuit is null!");
                }
                return trumpCard.Suit;
            }
        }

        public Player ActivePlayer { get { return players[activePlayer]; } }

        public ObservableCollection<Player> Players {
            get { return players; }
        }

        public event EventHandler<PlayerEventArgs> ActivePlayerChanged;
        public event EventHandler<RoundFinishedEventArgs> RoundFinished;
        public event EventHandler<PlayerEventArgs> RoundStarted;
        public event EventHandler<ShohaCatchQueenEventArgs> ShohaCatchQueen;

        public Round(ObservableCollection<Player> players) {
            this.players = players;
            this.trick.ShohaCatchQueen += Trick_ShohaCatchQueen;
        }

        private void Trick_ShohaCatchQueen(object sender, ShohaCatchQueenEventArgs e) {
            finishing = true;
            if(ShohaCatchQueen != null) {
                ShohaCatchQueen(this, e);
            }
        }



        public void Start(Player startRoundPlayer) {
            if(startRoundPlayer == null) {
                activePlayer = GetTrumpedPlayerIndex();
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
            return Players.ToList().FindIndex(p => { return p == startRoundPlayer; });
        }


        public void NextMove(Card card) {
            card = card == null ? ActivePlayer.ThrowCard() : ActivePlayer.ThrowCard(card);
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
                else {
                    if(ActivePlayer.IsAI) {
                        NextMove();
                    }
                }
            }
        }


        #region PRIVATE METHODS ----------------------------------------------------------------------------------------

        private void NextMove() {
            NextMove(null);
        }

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
        private int GetTrumpedPlayerIndex() {
            if(Players.ToList().Exists(p => { return p.Trumped; })) {
                return Players.ToList().FindIndex(p => { return p.Trumped; });
            }
            return 0;
        }

        public bool CanThrowCard(Player player, Card card) {
            return player.CanThrowCard(trick, card);
        }



        #endregion ---------------------------------------------------------------------------------------
    }
}
