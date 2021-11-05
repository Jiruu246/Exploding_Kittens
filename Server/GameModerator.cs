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
        //private ServerNetwork _network = ServerNetwork.GetInstance;
        private PlayerGroup _playerGroup;
        private Game _game;
        private RequestProcessor _reqProc;

        //private CardProcessor _cardProc;
        //private Deck _drawPile;
        //private Deck _discardPile;
        //private Player _Winner; // this could keep
        //private bool _start; // this could keep
        //private int _currentP;
        //private ManageTurn _currentTurn;
        //private int _direction;

        public GameModerator()
        {
            _playerGroup = new PlayerGroup();
            _game = new Game();
            _reqProc = new RequestProcessor(this, _game);
            //ResetGame();
            //RegisterCards();
        }

        /// <summary>
        /// tempo for testing
        /// </summary>
        public void ShowDrawPile()
        {
            foreach(_Card c in _game.DrawPile.CardList)
            {
                Console.WriteLine(c);
            }
        }

        public void ShowDisPile()
        {
            foreach (_Card c in _game.DiscardPile.CardList)
            {
                Console.WriteLine(c);
            }
        }

        public void ShowPlayerDeck(int i)
        {
            Player p = _playerGroup.GetPlayerAt(i);

            foreach (_Card c in p.Deck.CardList)
            {
                Console.WriteLine(c);
            }
        }

        public void Send(string data)
        {
            ServerNetwork.GetInstance.SendMulti(data, _playerGroup);
        }
        ///


        /// <summary>
        /// this need to change maybe
        /// </summary>
        /// <param name="data"></param>
        /// <param name="player"></param>
        public void Execute(object data, Player player)
        {
            if (data is _Card)
            {
                Thread cardProc = new Thread(() => 
                { 
                _game.ProcessCard((_Card)data, player);
                });
                cardProc.IsBackground = true;
                cardProc.Start();
            }
            else if (data is Requests)
            {
                _reqProc.Process((Requests)data, player);
            }
            else if(data is string)
            {
                Console.WriteLine((string)data);
            }
            else if(data is int)
            {
                _game.PutBackCard(player, (int)data);
            }
        }

        public void ListenForPlayer()
        {

            while (true)
            {
                if (!_playerGroup.MaxPlayer())
                {

                    Socket Client = ServerNetwork.GetInstance.Listen();

                    if (_game.Start)
                    {
                        ServerNetwork.GetInstance.SendSingle(Client, "The game already start"); //change it to request
                        Client.Close();
                    }
                    else if (Client != null)
                    {
                        try
                        {
                            Player newplayer =_playerGroup.AddPlayer(Client);

                            foreach(Player player in _playerGroup.PlayerList)
                            {
                                MatchInfo data = new MatchInfo(_playerGroup.PlayerList, player.Position);
                                ServerNetwork.GetInstance.SendSingle(player.ClientSK, data);
                            }

                            Thread recieve = new Thread(() => { Receive(newplayer); });
                            recieve.IsBackground = true;
                            recieve.Start();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            ServerNetwork.GetInstance.GenerateAddress();
                        }

                    }
                }
            }
        }


        private void Receive(Player player)
        {
            try
            {
                while (!player.Explode)
                {
                    Execute(ServerNetwork.GetInstance.GetData(player.ClientSK), player);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Receive error!!!!!!");
                ServerNetwork.GetInstance.CloseClient(player.ClientSK, _playerGroup);
                ResendPosition(); // when someone disconnect, resend the position
            }
        }

        private void ResendPosition()
        {
            foreach (Player p in _playerGroup.PlayerList)
            {
                MatchInfo data = new MatchInfo(_playerGroup.PlayerList, p.Position);
                ServerNetwork.GetInstance.SendSingle(p.ClientSK, data);
            }
        }
        public void CreateGame()
        {
            if (_playerGroup.NumOfPlayer > 1)
            {
                _game.NewGame(_playerGroup, 20);
                foreach (Player p in _playerGroup.PlayerList)
                {
                    ServerNetwork.GetInstance.SendSingle(p.ClientSK, Requests.Start);
                }

                Thread.Sleep(250);

                Thread newgame = new Thread(_game.StartGame);
                newgame.IsBackground = true;
                newgame.Start();

            }
        }

    }
}
