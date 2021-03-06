﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kozel.AI;
using System.Collections.ObjectModel;

namespace Kozel {
    public class KozelGame {
        public static readonly string Name = "Kozel";

        private Team team1;
        private Team team2;
        private ObservableCollection<Player> players;
        private List<Game> games = new List<Game>();

        public Team Team1 { get { return team1; } }
        public Team Team2 { get { return team2; } }
        public bool IsFinished { get { return Team1.GameScore >= 12 || Team2.GameScore >= 12; } }
        public Game CurrentGame { get { return games.Last(); } }
        public ObservableCollection<Player> Players { get { return players; } }

        public event EventHandler GameStarted;
        public event EventHandler<PlayerMadeMoveEventArgs> PlayerMadeMove;
        public event EventHandler<PlayerEventArgs> ActivePlayerChanged;
        public event EventHandler<PlayerEventArgs> CardsResorted;
        public event EventHandler<RoundFinishedEventArgs> RoundFinished;
        public event EventHandler<PlayerEventArgs> RoundStarted;
        public event EventHandler<GameFinishedEventArgs> GameFinished;



        public KozelGame() {
            players = new ObservableCollection<Player>() { new Player(new StupidAI()), new Player(new StupidAI()), new Player(), new Player(new StupidAI()) };
            team1 = new Team(players[0], players[2]);
            team2 = new Team(players[1], players[3]);
            foreach (Player player in players) {
                player.PlayerMadeMove += Player_PlayerMadeMove;
                player.CardsResorted += Player_CardsResorted;
            }
        }



        public void Start() {
            StartNewGame();
        }

        public void StartNewGame() {
            Game game = new Game(players);
            Team1.ClearTricks();
            Team2.ClearTricks();
            game.GameFinished += Game_GameFinished;
            game.ActivePlayerChanged += Game_ActivePlayerChanged;
            game.RoundFinished += Game_RoundFinished;
            game.RoundStarted += Game_RoundStarted;
            games.Add(game);
            game.Start();

            if (GameStarted != null)
                GameStarted(this, new EventArgs());
        }

        private void Game_RoundStarted(object sender, PlayerEventArgs e) {
            if (RoundStarted != null) {
                RoundStarted(this, e);
            }
        }

        private void Game_ActivePlayerChanged(object sender, PlayerEventArgs e) {
            if (ActivePlayerChanged != null) {
                ActivePlayerChanged(this, e);
            }
        }

        private void Player_CardsResorted(object sender, PlayerEventArgs e) {
            if (CardsResorted != null) {
                CardsResorted(this, e);
            }
        }

        private void Player_PlayerMadeMove(object sender, PlayerMadeMoveEventArgs e) {
            if (PlayerMadeMove != null)
                PlayerMadeMove(this, e);
        }

        private void Game_RoundFinished(object sender, RoundFinishedEventArgs e) {
            if (RoundFinished != null) {
                RoundFinished(this, e);
            }
        }

        private void Game_GameFinished(object sender, GameFinishedEventArgs e) {
            if (GameFinished != null) {
                GameFinished(this, e);
            }
            if (!IsFinished) {
                StartNewGame();
            }
        }
    }
}
