using System;

namespace Kozel
{
    public class PlayerEventArgs : EventArgs
    {
        public PlayerEventArgs(Player player)
        {
            Player = player;
        }

        public Player Player { get; private set; }
    }
}