using System;
using System.Text;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using ExplodingKittenLib;
using ExplodingKittenLib.Cards;
using ExplodingKittenLib.Numbers;

namespace Server
{
    class GameModerator
    {
        private ServerNetwork _network = ServerNetwork.GetInstance();
        private PlayerGroup _playerGroup;
        private CardProcessor _cardProc;
        private RequestProcessor _reqProc;
        private Deck _drawPile;
        private Deck _discardPile;

        private Player _Winner; // this could keep
        private bool _start; // this could keep

        private int _currentP;
        private ManageTurn _currentTurn;

        private int _direction;


        public GameModerator()
        {
            ResetGame();
            RegisterCards();
        }

        /// <summary>
        /// tempo
        /// </summary>
        public void ShowDrawPile()
        {
            foreach(_Card c in _drawPile.CardList)
            {
                Console.WriteLine(c);
            }
        }

        public void ShowDisPile()
        {
            foreach (_Card c in _discardPile.CardList)
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
        ///


        /// <summary>
        /// this need to change maybe
        /// </summary>
        /// <param name="data"></param>
        /// <param name="player"></param>
        public void Execute(object data, Player player)
        {

            if (data is Numbers)
            {
                CardPosition pos = data as CardPosition;
                _currentTurn.BombPosition = pos.Get;
            }
            else if (data is _Card)
            {
                //shouldnt do this out here

                _cardProc.Process((_Card)data, player);
            }
            else if (data is Requests)
            {
                _reqProc.Process((Requests)data, player);
            }

        }

        public void CreateGame(Player player)
        {
            if (player.RoomMaster && _playerGroup.NumOfPlayer > 1)
            {
                SettupGame(20);

                _start = true;
                Thread newgame = new Thread(StartGame);
                newgame.IsBackground = true;
                newgame.Start();
            }
        }

        public void Listen()
        {
            while (true)
            {
                if (!_playerGroup.MaxPlayer())
                {
                    Socket Client = _network.Listen();

                    if (_start)
                    {
                        _network.SendSingle(Client, "The game already start");
                        Client.Close();
                    }
                    else if (Client != null)
                    {
                        try
                        {
                            Player player = _playerGroup.AddPlayer(Client);

                            _network.SendSingle(Client, player.GetPosition); // send the player position object

                            Thread recieve = new Thread(() => { Receive(player); });
                            recieve.IsBackground = true;
                            recieve.Start();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            Console.WriteLine("disconnect bug");
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
                while (!player.Explode) //should be put at other place???
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
                _network.SendSingle(p.ClientSK, p.GetPosition);
            }
        }

        private void SettupGame(int numofcard)
        {
            _drawPile = new Deck(_playerGroup.NumOfPlayer, numofcard);

            foreach (Player player in _playerGroup.PlayerList)
            {
                Deck deck = new Deck();
                deck.AddCard(_Card.CreateCard(CardType.Defuse));
                for(int i = 0; i < 4; i++) //each player start with 4 card and a defuse
                {
                    deck.AddCard(_Card.GetRandom());
                }

                player.Deck = deck;
                _network.SendSingle(player.ClientSK, deck);
            }

        }

        public Player Winner
        {
            get
            {
                return _Winner;
            }

            set
            {
                _Winner = value;
            }
        }


        private void StartGame()
        {
            try
            {
                while (Winner == null)
                {
                    ResetTurn(); //exploded player will have 0 turn
                    for(int i = 0; i < _playerGroup.NumOfPlayer; i += Direction)
                    {
                        _currentP = i;
                        Player player =_playerGroup.GetPlayerAt(i);
                        
                        while (player.Turn > 0)
                        {

                            _currentTurn = new ManageTurn(player, _drawPile, _discardPile);

                            _network.SendSingle(player.ClientSK, Requests.YourTurn);

                            while (!_currentTurn.EndTurn)
                            {
                                /// AFK detect
                                if(_playerGroup.NumOfPlayer == 0)/// if everyone afk
                                {
                                    ResetGame();
                                    return;
                                }
                                if (_playerGroup.NumOfPlayer == 1) 
                                {
                                    player = _playerGroup.GetPlayerAt(0);
                                    Winner = player;
                                    break;
                                }
                                else if (!_playerGroup.HasPlayer(player))
                                {
                                    if (i == _playerGroup.NumOfPlayer)
                                    {
                                        break;
                                    }
                                    player = _playerGroup.PlayerList[i];
                                    _network.SendSingle(player.ClientSK, Requests.YourTurn);
                                }
                                ///

                                _currentTurn.Busy();
                            }

                            ReduceTurn(player);

                            if(_playerGroup.NumOfSurvival == 1)
                            {
                                Winner = _playerGroup.GetWinner();
                                break;
                            }
                        }

                        if(Winner != null)
                        {
                            break;
                        }
                    }

                }

                _network.SendSingle(Winner.ClientSK, "you winnnnn!!!!!!!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("disconnect error");
                ResetGame();
            }

        }


        public void GiveTopCard(Player player) //change return type
        {
            _Card card = _drawPile.Pop();
            SyncSending(player, card);

            CheckBomb(card);
        }

        public void GiveBottomCard(Player player)
        {
            _Card card = _drawPile.PopBottom();
            SyncSending(player, card);

            CheckBomb(card);

        }
        public int CurrentPlayer
        {
            get
            {
                return _currentP;
            }
        }

        public int Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                if(value == 1 || value == -1)
                {
                    _direction = value;
                }
            }
        }

        public void ChageDirection()
        {
            Direction *= -1;
            ResetTurn();
        }

        public void ReduceTurn(Player player)
        {
            player.Turn--;
            _network.SendSingle(player.ClientSK, player.GetTurn);
        }

        /// <summary>
        /// Reset alive player turn
        /// </summary>
        public void ResetTurn()
        {
            _playerGroup.ResetTurn();
            _network.SendMulti(new Turn(1), _playerGroup);

        }

        private void SyncSending(Player player, _Card card)
        {
            _playerGroup.GivePlayerData(player.Position, card);
            _network.SendSingle(player.ClientSK, card);

            //return card is ExplodingCard;
        }

        private void CheckBomb(_Card card)
        {
            if (card is ExplodingCard)
            {
                _currentTurn.DrawBomb = true;
            }
            else
            {
                _currentTurn.EndTurn = true;
            }
        }

        public void DefuseCurrentTurn()
        {
            _currentTurn.BombDefuse = true;
        }



        public void SendDeny(Player player)
        {
            _network.SendSingle(player.ClientSK, "Deny");
        }

        public void ResetGame()
        {
            _Winner = null;
            _start = false;
            _playerGroup = new PlayerGroup();
            _reqProc = new RequestProcessor(this);
            _cardProc = new CardProcessor(this);
            _discardPile = new Deck();
            _direction = 1;
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
