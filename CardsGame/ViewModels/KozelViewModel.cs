using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kozel;

namespace CardsGame.ViewModels {
    public class KozelViewModel : INotifyPropertyChanged {
        Kozel.KozelGame kozelGame;

        public string Title { get { return Kozel.KozelGame.Name; } }
        public Player Player1 { get { return kozelGame.Players[0]; } }

        public event PropertyChangedEventHandler PropertyChanged;

        public KozelViewModel() {
            kozelGame = new Kozel.KozelGame();
        }

    }
}
