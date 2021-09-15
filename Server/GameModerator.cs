using System;
using System.Text;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using ExplodingKittenLib;
using ExplodingKittenLib.Cards;
using ExplodingKittenLib.Request;

namespace Server
{
    class GameModerator
    {
        private ServerNetwork _network = ServerNetwork.GetInstance();
        private PlayerGroup _playerGroup;
        private Deck _drawPile;
        private Deck _discardPile;
        private bool _start;
        private int _currentP;

        public GameModerator(PlayerGroup players)
        {
            _start = false;
            _playerGroup = players;
            RegisterCards();
            _discardPile = new Deck();
        }

        public void Execute(object data, Player player)
        {

            if (data is int)
            {
                Console.WriteLine((int)data);
            }
            else if(data is String)
            {
                StringProcess((string)data, player);
            }
            else if (data is _Card)
            {
                Console.WriteLine("this is a card wwooooo");
            }

        }

        public void StringProcess(string text, Player player) //probably will delete
        {
            switch (text)
            {
                case "start":
                    if (player.RoomMaster && _playerGroup.NumOfPlayer > 1)
                    {
                        _start = true;
                        Thread newgame = new Thread(StartGame);
                        newgame.IsBackground = true;
                        newgame.Start();
                    }
                    break;
            }
        }

        public void Listen()
        {
            while (true)
            {
                if (!_start)
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
        }

        private void Receive(Player player)
        {
            try
            {
                while (true)
                {
                    Execute(_network.GetData(player.ClientSK), player);
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

        private void SettupGame(int numofcard)
        {
            _drawPile = new Deck(_playerGroup.NumOfPlayer, numofcard);

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

        private void StartGame()
        {
            SettupGame(20);

            while (_playerGroup.NumOfPlayer > 1)
            {
                _playerGroup.ResetTurn();
                for(int i = 0; i < _playerGroup.NumOfPlayer; i++)
                {
                    _currentP = i;
                    Player player = _playerGroup.PlayerList[i];
                    if (!player.Explode)
                    {
                        while (player.Turn != 0)
                        {
                            _network.SendSingle(player.ClientSK, "Your turn");

                            while (playernotdraw)
                            {
                                if (player.Turn == 0)
                                    break;
                            }

                            player.Turn = -1;
                        }
                    }
                }
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
