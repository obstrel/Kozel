using System;

namespace Kozel {
    public class RoundFinishedEventArgs : EventArgs {
        public Player LastRoundWinner { get; private set; }
        public Team RoundOwner { get; private set; }

        public RoundFinishedEventArgs(Player lastRoundWinner, Team roundOwner) {
            LastRoundWinner = lastRoundWinner;
            RoundOwner = roundOwner;
        }

    }
}