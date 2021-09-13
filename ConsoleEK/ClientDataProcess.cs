using System;
using System.Threading;
using ExplodingKittenLib;

namespace Client
{
    class ClientDataProcess
    {
        private ClientNetwork _network;
        private Player _player;
        private Deck _deck;

        public ClientDataProcess(Player p, Deck d)
        {
            _network = new ClientNetwork(p);
            _player = p;
            _deck = d;
        }
        public void Execute(object data)
        {
            switch (data.GetType().Name)
            {
                case "Int32":
                    SetPlayerPosition((int)data);
                    Console.WriteLine(_player.Position);
                    break;
                case "String":
                    Console.WriteLine((string)data);
                    break;
            }


        }

        public bool Connect()
        {
            bool conn;
            conn = _network.Connect();

            Thread listen = new Thread(Receive);
            listen.IsBackground = true;
            listen.Start();

            return conn;
        }

        public void Close()
        {
            _network.Close();
        }

        public void Send(object data)
        {
            _network.Send(data);
        }

        public void Receive()
        {
            try
            {
                while (true)
                {
                    Execute(_network.GetData());
                }
            }
            catch
            {
                _network.Close();
            }
        }

        private void SetPlayerPosition(int position)
        {
            _player.Position = position;
        }
    }
}
