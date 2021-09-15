using System;
using System.Text;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using ExplodingKittenLib;
using ExplodingKittenLib.Cards;

namespace Server
{
    class GameModerator
    {
        private ServerNetwork _network = ServerNetwork.GetInstance();
        private PlayerGroup _playerGroup;
        private Deck _drawPile;
        private Deck _discardPile;

        public GameModerator(PlayerGroup players)
        {
            _playerGroup = players;
            RegisterCards();
            _discardPile = new Deck();
        }

        public void Execute(object data)
        {
            switch (data.GetType().Name)
            {
                case "Int32":
                    Console.WriteLine((int)data);
                    break;
                case "String":
                    StringProcess((string)data);
                    break;
            }
        }

        public void StringProcess(string text) //probably will delete
        {
            switch (text)
            {
                case "start":
                    SettupGame();
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

        private void Receive(Player player)
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

        private void ResendPosition()
        {
            foreach (Player p in _playerGroup.PlayerList)
            {
                _network.SendSingle(p.ClientSK, p.Position);
            }
        }

        private void SettupGame()
        {
            _drawPile = new Deck(_playerGroup.NumOfPlayer, 20);

            foreach (Player player in _playerGroup.PlayerList)
            {
                Deck deck = new Deck();
                deck.AddCard(_Card.CreateCard(CardType.Defuse));
                for(int i = 0; i < 4; i++)
                {
                    deck.AddCard(_Card.GetRandom());
                }

                player.Deck = deck;
                _network.SendSingle(player.ClientSK, deck);
            }

        }


        private void RegisterCards()
        {
            _Card.RegisterCard(CardType.Exploding, typeof(ExplodingCard));
            _Card.RegisterCard(CardType.Defuse, typeof(DefuseCard));
            _Card.RegisterCard(CardType.Skip, typeof(SkipCard));
            _Card.RegisterCard(CardType.Cattermelon, typeof(CattermelonCard));
        }
    }
}
