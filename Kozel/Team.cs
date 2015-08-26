using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kozel
{
   public class Team
    {
        private Player player1;
        private Player player2;

        public Player Player1
        {
            get { return player1; }
        }

        public Player Player2
        {
            get { return player2; }
        }

        public Team(Player aPlayer1, Player aPlayer2)
        {
            player1 = aPlayer1;
            player2 = aPlayer2;
        }
    }
}
