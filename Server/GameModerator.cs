using System;
using System.Collections.Generic;
using System.Text;
using ExplodingKittenLib;

namespace Server
{
    class GameModerator
    {
        private string prom; //temp 
        private ServerNetwork _network;
        private PlayerGroup _playerGroup;

        public GameModerator(ServerNetwork network, PlayerGroup players)
        {
            _network = network;
            _playerGroup = players;
        }
        public void Update()
        {
            prom = Console.ReadLine();

            List<string> command = new List<string>(prom.Split(' '));

            switch (command[0])
            {
                case "close":
                    _network.Close();
                    break;
                case "send":
                    _network.SendMulti(command[1]);
                    break;
            }
        }
    }
}
