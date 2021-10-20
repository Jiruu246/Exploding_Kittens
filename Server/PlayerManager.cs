using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class PlayerManager
    {
        private PlayerGroup _players;

        public PlayerManager(PlayerGroup players)
        {
            _players = players;
        }
    }
}
