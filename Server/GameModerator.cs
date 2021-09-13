using System;
using System.Text;
using System.Threading;
using ExplodingKittenLib;

namespace Server
{
    class GameModerator
    {
        private ServerNetwork _network;
        private PlayerGroup _playerGroup;

        public GameModerator(ServerNetwork network, PlayerGroup players)
        {
            _network = network;
            _playerGroup = players;
        }

        public void Execute(object data)
        {
            switch (data.GetType().Name)
            {
                case "Int32":
                    Console.WriteLine((int)data);
                    break;
                case "String":
                    Console.WriteLine((string)data);
                    break;
            }
        }

        public void Listen()
        {
            while (true)
            {
                if (!_playerGroup.MaxPlayer())
                {
                    Player player = _network.Listen();
                    if (player != null)
                    {
                        Thread recieve = new Thread(() => { Receive(player); });
                        recieve.IsBackground = true;
                        recieve.Start();
                    }
                }

            }
        }

        public void Receive(Player player)
        {
            try
            {
                while (true)
                {
                    Execute(_network.GetData(player.ClientSK));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _network.CloseClient(player.ClientSK);
            }
        }
    }
}
