using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kozel;
using System.Windows.Input;
using CardsGame.Commands;

namespace CardsGame.ViewModels {
    public class KozelViewModel : INotifyPropertyChanged {
        Kozel.KozelGame kozelGame;

        public string Title { get { return Kozel.KozelGame.Name; } }
        public Player Player1 { get { return kozelGame.Players[0]; } }
        public Player Player2 { get { return kozelGame.Players[1]; } }
        public Player Player3 { get { return kozelGame.Players[2]; } }
        public Player Player4 { get { return kozelGame.Players[3]; } }

        public event PropertyChangedEventHandler PropertyChanged;

        //       private DelegateCommand startGameCommand;

        public KozelViewModel() {
            kozelGame = new Kozel.KozelGame();
        }

        private ICommand newGame;
        public ICommand NewGame {
            get {
                if (newGame == null) {
                    newGame = new DelegateCommand(
                        delegate () {
                            kozelGame.Start();
                        }
                    );
                }
                return newGame;
            }
        }

    }
}
