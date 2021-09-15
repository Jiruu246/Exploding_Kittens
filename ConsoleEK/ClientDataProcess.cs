using System;
using System.Threading;
using ExplodingKittenLib;

namespace Client
{
    class ClientDataProcess //facade pattern
    {
        private ClientNetwork _network = ClientNetwork.GetInstance();
        private Player _player;

        public ClientDataProcess(Player player)
        {
            _player = player;
        }
        public void Execute(object data)
        {
            switch (data.GetType().Name)
            {
                case "Int32":
                    SetPlayerPosition((int)data);
                    Console.WriteLine(_player.Position);
                    break;
                case "String": // maybe will delete later
                    Console.WriteLine((string)data);
                    break;
                case "Deck":
                    MergeDeck((Deck)data);
                    break;
            }


        }

        public bool Connect()
        {
            bool conn = _network.Connect();

            _player.ClientSK = _network.Socket; // save the socket to the player object

            Thread listen = new Thread(Receive); // when connect establish a listen thread immidiately
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
                    Execute(_network.GetData(_player.ClientSK));
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

        private void MergeDeck(Deck deck)
        {
            _player.Deck.Merge(deck);
        }
    }
}
