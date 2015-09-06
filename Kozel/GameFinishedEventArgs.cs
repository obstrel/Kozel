using System;

namespace Kozel {
    public class GameFinishedEventArgs : EventArgs {
        public GameFinishedEventArgs(Player lastRoundWinner) {
            LastRoundWinner = lastRoundWinner;
        }

        public Player LastRoundWinner { get; private set; }
    }
}