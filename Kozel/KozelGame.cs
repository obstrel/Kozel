using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kozel {
    public class KozelGame {
        public static string Name { get { return "Kozel"; } }

        private Team team1;
        private Team team2;
        private int activeRound = 0;
        List<Player> players = new List<Player>(4) { new Player(), new Player(), new Player(), new Player() };
        List<Round> rounds = new List<Round>(8);

        public Team Team1 { get { return team1; } }
        public Team Team2 { get { return team2; } }
        public Round ActiveRound { get { return rounds[activeRound]; } }


        private CardSuit? GetTrumpSuit() {
            Random r = new Random();
            Array ar = Enum.GetValues(typeof(CardSuit));
            return (CardSuit)ar.GetValue((int)r.Next(3));
        }

        public event EventHandler GameStarted;
        public event EventHandler<PlayerMadeMoveEventArgs> PlayerMadeMove;
        public event EventHandler<PlayerEventArgs> ActivePlayerChanged;
        public event EventHandler<PlayerEventArgs> CardsResorted;

        public KozelGame() {
            team1 = new Team(players[0], players[2]);
            team2 = new Team(players[1], players[3]);
            for (int i = 0; i < 8; i++) {
                rounds.Add(new Round(Team1, Team2));
                rounds.Last().ActivePlayerChanged += KozelGame_ActivePlayerChanged;
            }
            foreach (Player player in players) {
                player.PlayerMadeMove += Player_PlayerMadeMove;
                player.CardsResorted += Player_CardsResorted;
            }
        }

        private void KozelGame_ActivePlayerChanged(object sender, PlayerEventArgs e) {
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


        public void Start() {
            rounds[0].Start();
            if (GameStarted != null)
                GameStarted(this, new EventArgs());
        }
    }
}
