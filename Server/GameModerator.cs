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
        private ServerNetwork _network = ServerNetwork.GetInstance;
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
            _network.SendMulti(data, _playerGroup);
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
        }

        public void CreateGame()
        {
            if (_playerGroup.NumOfPlayer > 1)
            {
                _game.NewGame(_playerGroup, 20);
                foreach (Player p in _playerGroup.PlayerList)
                {
                    _network.SendSingle(p.ClientSK, Requests.Start);
                }
                
                Thread newgame = new Thread(_game.StartGame);
                newgame.IsBackground = true;
                newgame.Start();
            }
        }

        public void ListenForPlayer()
        {

            while (true)
            {
                if (!_playerGroup.MaxPlayer())
                {

                    Socket Client = _network.Listen();

                    if (_game.Start)
                    {
                        _network.SendSingle(Client, "The game already start"); //change it to request
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
                                _network.SendSingle(player.ClientSK, data);
                            }

                            Thread recieve = new Thread(() => { Receive(newplayer); });
                            recieve.IsBackground = true;
                            recieve.Start();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            _network.GenerateAddress();
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
                    Execute(_network.GetData(player.ClientSK), player);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Receive error!!!!!!");
                _network.CloseClient(player.ClientSK, _playerGroup);
                ResendPosition(); // when someone disconnect, resend the position
            }
        }

        private void ResendPosition()
        {
            foreach (Player p in _playerGroup.PlayerList)
            {
                MatchInfo data = new MatchInfo(_playerGroup.PlayerList, p.Position);
                _network.SendSingle(p.ClientSK, data);
            }
        }
        public void SendDeny(Player player)
        {
            _network.SendSingle(player.ClientSK, "Deny");
        }

        //private void SetupGame(int numofcard)
        //{
        //    _drawPile = new Deck(_playerGroup.NumOfPlayer, numofcard);

        //    foreach (Player player in _playerGroup.PlayerList)
        //    {
        //        //each player start with 4 card and a defuse
        //        Deck deck = new Deck();
        //        deck.AddCard(_Card.CreateCard(CardType.DefuseCard));
        //        for (int i = 0; i < 4; i++)
        //        {
        //            deck.AddCard(_Card.GetRandom());
        //        }
        //        player.Deck = deck;
        //        _network.SendSingle(player.ClientSK, deck);
        //    }
        //}

        //public player winner
        //{
        //    get
        //    {
        //        return _winner;
        //    }

        //    set
        //    {
        //        _winner = value;
        //    }
        //}


        //private void StartGame()
        //{
        //    //try
        //    //{
        //    //    while (_game.Winner == null)
        //    //    {
        //    //        ResetTurn(); //exploded player will have 0 turn
        //    //        for(int i = 0; i < _playerGroup.NumOfPlayer; i += Direction)
        //    //        {
        //    //            _currentP = i;
        //    //            Player player =_playerGroup.GetPlayerAt(i);
                        
        //    //            while (player.Turn > 0)
        //    //            {

        //    //                _currentTurn = new ManageTurn(player, _game.DrawPile, _game.DiscardPile);

        //    //                //throw new NotImplementedException();
        //    //                //_network.SendMulti(new CurrentTurn(_currentP), _playerGroup);

        //    //                while (!_currentTurn.EndTurn)
        //    //                {
        //    //                    /// AFK detect
        //    //                    if(_playerGroup.NumOfPlayer == 0)/// if everyone afk
        //    //                    {
        //    //                        ResetGame();
        //    //                        return;
        //    //                    }
        //    //                    if (_playerGroup.NumOfPlayer == 1) 
        //    //                    {
        //    //                        player = _playerGroup.GetPlayerAt(0);
        //    //                        _game.Winner = player;
        //    //                        break;
        //    //                    }
        //    //                    else if (!_playerGroup.HasPlayer(player))
        //    //                    {
        //    //                        if (i == _playerGroup.NumOfPlayer)
        //    //                        {
        //    //                            break;
        //    //                        }
        //    //                        player = _playerGroup.PlayerList[i];
        //    //                        //throw new NotImplementedException();
        //    //                        //_network.SendMulti(new CurrentTurn(_currentP), _playerGroup);
        //    //                    }
        //    //                    ///

        //    //                    _currentTurn.Busy();
        //    //                }

        //    //                ReduceTurn(player);

        //    //                if(_playerGroup.NumOfSurvival == 1)
        //    //                {
        //    //                    _game.Winner = _playerGroup.GetWinner();
        //    //                    break;
        //    //                }
        //    //            }

        //    //            if(_game.Winner != null)
        //    //            {
        //    //                break;
        //    //            }
        //    //        }

        //    //    }

        //    //    _network.SendSingle(_game.Winner.ClientSK, "you winnnnn!!!!!!!");
        //    //    ResetGame();
        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    Console.WriteLine(e);
        //    //    Console.WriteLine("disconnect error");
        //    //    ResetGame();
        //    //}

        //}


        //public void GiveTopCard(Player player)
        //{
        //    _Card card = _game.DrawPile.Pop();
        //    SyncSending(player, card);

        //    CheckBomb(card);
        //}

        //public void GiveBottomCard(Player player)
        //{
        //    _Card card = _game.DrawPile.PopBottom();
        //    SyncSending(player, card);

        //    CheckBomb(card);
        //}

        //public int CurrentPlayer
        //{
        //    get
        //    {
        //        return _currentP;
        //    }
        //}


        //public void ChageDirection()
        //{
        //    Direction *= -1;
        //    ResetTurn();
        //}

        //public void ReduceTurn(Player player)
        //{
        //    player.Turn--;
        //    //throw new NotImplementedException();
        //    //_network.SendSingle(player.ClientSK, player.GetTurn);
        //}

        /// <summary>
        /// Reset alive player turn
        /// </summary>
        //public void ResetTurn()
        //{
        //    _playerGroup.ResetTurn();
        //    //throw new NotImplementedException();
        //    //_network.SendMulti(new Turn(1), _playerGroup);
        //}



        //private void CheckBomb(_Card card)
        //{
        //    if (card is ExplodingCard)
        //    {
        //        _currentTurn.DrawBomb = true;
        //    }
        //    else
        //    {
        //        _currentTurn.Stop();
        //    }
        //}

        //public void DefuseCurrentTurn()
        //{
        //    _currentTurn.BombDefuse = true;
        //}


        //public void ResetGame()
        //{
        //    //_Winner = null;
        //    //_start = false;
        //    //_playerGroup = new PlayerGroup();
        //    //_discardPile = new Deck();
        //    //_cardProc = new CardProcessor(this, _drawPile, _discardPile);
        //    //_direction = 1;
        //}


        //private void RegisterCards() // should this be here ??
        //{
        //    _Card.RegisterCard(CardType.ExplodingCard, typeof(ExplodingCard));
        //    _Card.RegisterCard(CardType.DefuseCard, typeof(DefuseCard));
        //    _Card.RegisterCard(CardType.SkipCard, typeof(SkipCard));
        //    _Card.RegisterCard(CardType.CattermelonCard, typeof(CattermelonCard));
        //    _Card.RegisterCard(CardType.NopeCard, typeof(NopeCard));
        //}
    }
}
