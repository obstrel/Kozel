using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardsGame.ViewModels {
    public class KozelViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Title { get { return "Kozel Game"; } }
    }
}
