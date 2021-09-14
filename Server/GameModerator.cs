using System;
using System.Text;
using System.Net.Sockets;
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
                    Socket Client = _network.Listen();

                    if (Client != null)
                    {
                        Player player = _playerGroup.AddPlayer(Client);

                        _network.SendSingle(Client, player.Position); // send the player position

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
                _network.CloseClient(player.ClientSK, _playerGroup);
                ResendPosition(); // when someone disconnect, resend the position
            }
        }

        public void ResendPosition()
        {
            foreach (Player p in _playerGroup.PlayerList)
            {
                _network.SendSingle(p.ClientSK, p.Position);
            }
        }
    }
}
