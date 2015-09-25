using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using Kozel;

namespace CardsGame {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private static readonly Brush InactivePlayerPanelBrush = Brushes.White;

//        Kozel.KozelGame game = new Kozel.KozelGame();

//        private KozelGame Game { get { return game; } }
        private Panel[] PlayerPanels;
        private Panel[] TablePlayerPanels;

        public MainWindow() {
            InitializeComponent();
            this.DataContext = new ViewModels.KozelViewModel();
            TableCanvas.Background = new ImageBrush() { ImageSource = new BitmapImage((new Uri("Images\\Изображение.jpg", UriKind.Relative))) };
//            Title = Kozel.KozelGame.Name;
//            Game.GameStarted += Game_GameStarted;
//            PlayerPanels = new Panel[4] { Player1, Player2, Player3, Player4 };
            TablePlayerPanels = new Panel[4] { TablePlayer1, TablePlayer2, TablePlayer3, TablePlayer4 };
        }

        private void Game_ActivePlayerChanged(object sender, PlayerEventArgs e) {
            if (e.Player != null) {
                ActivatePlayer(e.Player);
            }
        }

        private void Game_PlayerMadeMove(object sender, PlayerMadeMoveEventArgs e) {
            DeactivatePlayer(e.Player);
        }


        private Panel FindPlayerPanelByPlayer(Player player) {
            return PlayerPanels.Single(p => (p.Tag as Player) == player);
        }

        private void Game_GameStarted(object sender, EventArgs e) {
            ShowCards();
            InitTablePanel();
            //lTrumpSuit.Content = Game.CurrentGame.ActiveRound.TrumpSuit;
            //ActivatePlayer(Game.CurrentGame.ActiveRound.ActivePlayer);
            //game.PlayerMadeMove += Game_PlayerMadeMove;
            //game.ActivePlayerChanged += Game_ActivePlayerChanged;
            //game.CardsResorted += Game_CardsResorted;
            //game.RoundFinished += Game_RoundFinished;
            //game.RoundStarted += Game_RoundStarted;
            //game.GameFinished += Game_GameFinished;
        }

        private void ClearTable() {
            foreach (Panel panel in TablePlayerPanels) {
                panel.Children.Clear();
            }
        }

        private void ClearPlayerPanels() {
            foreach(Panel panel in PlayerPanels) {
                panel.Children.Clear();
                panel.Background = InactivePlayerPanelBrush;
            }
        }

        private void RefreshStat(Player lastRoundWinner) {
            lLastWinner.Content = lastRoundWinner.ToString();
            //lScoreTeam1.Content = Game.Team1.Score;
            //lScoreTeam2.Content = Game.Team2.Score;
            //lGameScoreTeam1.Content = Game.Team1.GameScore;
            //lGameScoreTeam2.Content = Game.Team2.GameScore;

        }

        private void Game_GameFinished(object sender, GameFinishedEventArgs e) {
            ClearTable();
            ClearPlayerPanels();
            RefreshStat(e.LastRoundWinner);
        }

        private void Game_RoundStarted(object sender, PlayerEventArgs e) {
            ActivatePlayer(e.Player);
        }

        private void Game_RoundFinished(object sender, RoundFinishedEventArgs e) {
            ClearTable();
            RefreshStat(e.LastRoundWinner);
        }

        private void InitTablePanel() {
            //TablePlayer1.Tag = game.Team1.Player1;
            //TablePlayer2.Tag = game.Team2.Player1;
            //TablePlayer3.Tag = game.Team1.Player2;
            //TablePlayer4.Tag = game.Team2.Player2;
        }

        private void DeactivatePlayer(Player player) {
            DeactivePlayer(FindPlayerPanelByPlayer(player));
        }

        private void DeactivePlayer(Panel player) {
            player.Background = InactivePlayerPanelBrush;
            foreach (UIElement el in player.Children) {
                (el as Label).BorderBrush = Brushes.Black;
                el.MouseMove -= Label_MouseMove;
            }
        }

        private void ActivatePlayer(Player player) {
            ActivatePlayer(FindPlayerPanelByPlayer(player));
        }

        private void ActivatePlayer(Panel player) {
            player.Background = Brushes.AntiqueWhite;
            foreach (UIElement el in player.Children) {
                //if (Game.CurrentGame.ActiveRound.CanThrowCard(player.Tag as Player, (el as Label).Tag as Card)) {
                //    (el as Label).BorderBrush = Brushes.Red;
                //    el.MouseMove += Label_MouseMove;
                //}
            }
        }

        private void ShowCards() {
            //ShowPlayerCards(Player1);
            //ShowPlayerCards(Player2);
            //ShowPlayerCards(Player3);
            //ShowPlayerCards(Player4);
        }

        private void ShowPlayerCards(Panel panel) {
            Player player = panel.Tag as Player;
//            panel.Tag = player;
            foreach (Card card in player.Cards) {
                Label label = CreateLabel(card);
                panel.Children.Add(label);
            }

        }

        private Label CreateLabel(Card card) {
            Label label = new Label();
            label.Content = card.Value + " " + card.Suit;
            label.BorderThickness = new Thickness(1, 1, 1, 1);
            label.BorderBrush = Brushes.Black;
            label.Tag = card;
            //            label.MouseMove += Label_MouseMove;
            label.FontSize = 8;

            return label;
        }


        //private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    MouseDown = true;
        //}

        private void Label_MouseMove(object sender, MouseEventArgs e) {
            Label l = sender as Label;
            if (l != null && e.LeftButton == MouseButtonState.Pressed) {
                DragDrop.DoDragDrop(l, l, DragDropEffects.Move);
            }
        }

        private void Image_Drop(object sender, DragEventArgs e) {
            object o = e.Data.GetData("System.Windows.Controls.Label", false);
            Label l = o as Label;

            if (l != null) {
                {
                    (l.Parent as Panel).Children.Remove(l);
                    //Panel panel = FindTablePanelByPlayer(Game.CurrentGame.ActiveRound.ActivePlayer);
                    //panel.Children.Add(l);
                    //Game.CurrentGame.ActiveRound.NextMove(l.Tag as Card);
                }
            }

        }

        private Panel FindTablePanelByPlayer(Player activePlayer) {
            foreach (Panel panel in new List<Panel> { TablePlayer1, TablePlayer2, TablePlayer3, TablePlayer4 }) {
                if (panel.Tag == activePlayer) {
                    return panel;
                }
            }
            return null;
        }
    }
}
