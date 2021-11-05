using System;
using System.Collections.Generic;
using System.Threading;
using ExplodingKittenLib;
using ExplodingKittenLib.Cards;
using ExplodingKittenLib.Activities;

namespace Client
{
    public class ClientGame
    {
        private static ClientGame _game;
        private string prom;
        private Player _player;
        private ClientDataProcess _process;
        private Deck _deck; 
        public Activity Activity;
        public _Card DCard { get; set; }
        public int NumOfDrawCard { get; set; }
        public int CurrentTurn;
        public bool GameStart { get; set; }
        public bool GameFound { get; set; }
        public bool GetBomb { get; set; }
        public bool ChoosingIndex { get; set; }
        public int Winner { get; set; }
        public int Index { get; set; }
        private ClientGame()
        {
            GameFound = false;
            GameStart = false;
            GetBomb = false;
            ChoosingIndex = false;
            CurrentTurn = 0;
            _player = new Player();
            _deck = _player.Deck;
            Winner = -1;
            _process = new ClientDataProcess(_player);
            Index = 1;

            //_Card.RegisterCard(CardType.ExplodingCard, typeof(ExplodingCard));
            //_Card.RegisterCard(CardType.DefuseCard, typeof(DefuseCard));
            //_Card.RegisterCard(CardType.SkipCard, typeof(SkipCard));
            //_Card.RegisterCard(CardType.CattermelonCard, typeof(CattermelonCard));
            //_Card.RegisterCard(CardType.NopeCard, typeof(NopeCard));
            //for (int i = 0; i < 9; i++)
            //{
            //    _deck.AddCard(_Card.GetRandom());
            //}
        }

        public static ClientGame GetInstance
        {
            get
            {
                if(_game == null)
                {
                    _game = new ClientGame();
                }
                return _game;
            }
        }

        //Console Game
        public void RunConsole()
        {
            prom = Console.ReadLine();

            List<string> command = new List<string>(prom.Split(' '));

            switch (command[0])
            {
                case "close":
                    ClientNetwork.GetInstance.Close();
                    break;
                case "send":
                    ClientNetwork.GetInstance.Send(command[1]);
                    break;
                case "connect":
                    Connect();
                    break;
                case "pos":
                    Console.WriteLine(_player.Position);
                    break;
                case "start":
                    StartMatch();
                    break;
                case "mydeck":
                    foreach(_Card card in _deck.CardList)
                    {
                        Console.WriteLine(card);
                    }
                    break;
                case "playcard":
                    //check for turn here
                    _Card playcard = _deck.CardList[int.Parse(command[1])];
                    if (_player.Position == CurrentTurn || playcard is NopeCard)
                    {
                        //everytime play a defuse always stop to ask where to put back (GUI)
                        if(playcard is DefuseCard)
                        {
                            _deck.Pop();
                        }
                        ClientNetwork.GetInstance.Send(playcard);
                        _deck.RemoveCardAt(int.Parse(command[1]));
                    }
                    else
                    {
                        Console.WriteLine("its not your turn");
                    }
                    break;
                case "draw":
                    DrawACard();
                    break;
                case "cardpos":
                    //_process.Send(new CardPosition(int.Parse(command[1])));
                    break;
            }

        }


        //GUI GAME
        private bool Connect()
        {
            bool conn = ClientNetwork.GetInstance.Connect();

            _player.ClientSK = ClientNetwork.GetInstance.Socket; // save the socket to the player object

            Thread listen = new Thread(Receive); // when connect establish a listen thread immidiately
            listen.IsBackground = true;
            listen.Start();

            return conn;
        }

        public void Receive()
        {
            try
            {
                while (true)
                {
                    object data = ClientNetwork.GetInstance.GetData(_player.ClientSK);
                    Thread execute = new Thread(() =>
                    {
                        _process.Execute(data);
                    });
                    execute.IsBackground = true;
                    execute.Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ClientNetwork.GetInstance.Close();
            }
        }
        public bool FindMatch()
        {
            bool result = Connect();
            if (result)
            {
                GameFound = true;
                Console.WriteLine("game found");
            }
            return result;
        }
        public void StartMatch()
        {
            ClientNetwork.GetInstance.Send(Requests.Start);
        }

        public void DrawACard()
        {
            if(CurrentTurn == _player.Position && !GetBomb && !ChoosingIndex && !_player.Explode)
            {
                ClientNetwork.GetInstance.Send(Requests.Draw);
            }
        }

        public void PutBackAt(int i)
        {
            ClientNetwork.GetInstance.Send(i);
            ChoosingIndex = false;
        }

        /// length of screen 1600
        public void UpdateInGame()
        {
            //update the hand
            if (_deck.DeckNotEmpty)
            {
                float gap;
                float xpos;
                if(_deck.NumOfCard < 10)
                {
                    gap = 800 / _deck.NumOfCard;
                    xpos = 300;
                }
                else if(_deck.NumOfCard < 20)
                {
                    gap = 1000 / _deck.NumOfCard;
                    xpos = 200;
                }
                else
                {
                    gap = 1200;
                    xpos = 100;
                }

                foreach (_Card card in _deck.CardList)
                {
                    card.X = xpos;
                    card.Y = 610;
                    xpos += gap;

                    if (card.Selected)
                    {
                        card.Y = 530;
                    }
                }
            }

            if (Index > NumOfDrawCard)
            {
                Index = 1;
            }
            else if (Index <= 0)
            {
                Index = NumOfDrawCard;
            }
        }

        public void DrawInGame()
        {
            //draw the whole hand
            foreach(_Card card in _deck.CardList)
            {
                UIAdapter.GetInstance.DrawCard(card);
            }
            //draw the activity
            if(Activity != null)
            {
                UIAdapter.GetInstance.DrawActivity(Activity);
            }

            //draw the game deck
            UIAdapter.GetInstance.DrawGameDeck(NumOfDrawCard, DCard);

            //draw the player
            UIAdapter.GetInstance.DrawPlayerInGame(PlayerInfo.GetInstance);

            if (GetBomb)
            {
                UIAdapter.GetInstance.DrawBombWarning();
            }
            else if (ChoosingIndex)
            {
                UIAdapter.GetInstance.DrawChoosingIndex(Index);
            }
        }

        public void DrawPlayerInLobby()
        {
            UIAdapter.GetInstance.DrawPlayerInLobby(PlayerInfo.GetInstance);
        }

        public int PlayerPos
        {
            get
            {
                return _player.Position;
            }
        }
        public void ChooseCardAt(double x, double y)
        {
            for(int i = _deck.NumOfCard - 1; i >= 0; i--)
            {
                _Card card = _deck.GetCardAt(i);
                if (card.IsAt(x, y))
                {
                    card.Selected = !card.Selected;
                    return;
                }
            }
        }
        public void PlayCard()
        {
            if (!_player.Explode && !ChoosingIndex)
            {
                int selected = 0;
                _Card[] playCards = new _Card[5];
                foreach(_Card c in _deck.CardList)
                {
                    if (c.Selected)
                    {
                        playCards[selected] = c;
                        selected += 1;
                    }
                }

                if(selected == 1)
                {
                    if (_player.Position == CurrentTurn || playCards[0] is NopeCard)
                    {
                        if (playCards[0] is DefuseCard)
                        {
                            if (GetBomb)
                            {
                                _deck.Pop();
                                GetBomb = false;
                                ChoosingIndex = true;
                            }
                            else
                            {
                                return;
                            }
                        }
                        ClientNetwork.GetInstance.Send(playCards[0]);
                        _deck.RemoveCard(playCards[0]);
                    }
                }
            }
        }
    }
}
