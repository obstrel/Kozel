using System;

namespace Kozel {
    public class ShohaCatchQueenEventArgs : EventArgs {
        public Player ShohaOwner { get; private set; }

        public ShohaCatchQueenEventArgs(Player ShohaOwner) {
            this.ShohaOwner = ShohaOwner;
        }
    }
}