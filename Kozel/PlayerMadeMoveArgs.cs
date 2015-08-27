using System;

namespace Kozel {
    public class PlayerEventArgs : EventArgs {
        public PlayerEventArgs(Player player) {
            Player = player;
        }

        public Player Player { get; private set; }
    }

    public class PlayerMadeMoveEventArgs : EventArgs {
        public PlayerMadeMoveEventArgs(Player player, Card card) {
            Player = player;
            Card = card;
        }

        public Player Player { get; private set; }
        public Card Card { get; private set; }
    }
}