using System;
using System.Threading;
using ExplodingKittenLib;
using ExplodingKittenLib.Cards;
using ExplodingKittenLib.Activities;

namespace Server
{
    class Game
    {
        public bool Start { get; set; }
        public Player Winner { get; set; }
        public Deck DrawPile;
        public Deck DiscardPile;

        private PlayerGroup _players;
        private int _direction;
        public int CurrentPlayer { get; set; }
        private CardProcessor _cardProc;
        private ManageTurn _Turn;

        public Game()
        {
            ResetGame();
            RegisterCards();
        }

        public void ProcessCard(_Card card, Player player)
        {
            _cardProc.Process(card, player, _players);
        }

        public void ResetGame()
        {
            Winner = null;
            Start = false;
            DiscardPile = new Deck();
            _cardProc = new CardProcessor(this);
            _direction = 1;
            CurrentPlayer = -1;
        }

        public void NewGame(PlayerGroup players, int numofcard)//setup game
        {
            _players = players;
            Start = true;
            DrawPile = new Deck(players.NumOfPlayer, numofcard);
            DiscardPile = new Deck();

            foreach (Player player in players.PlayerList)
            {
                //each player start with 4 card and a defuse
                Deck deck = new Deck();
                deck.AddCard(_Card.CreateCard(CardType.DefuseCard));
                for (int i = 0; i < 4; i++)
                {
                    //deck.AddCard(_Card.GetRandom());
                    deck.AddCard(_Card.CreateCard(CardType.ReverseCard));
                }
                foreach(_Card c in deck.CardList)
                {
                    c.Flip();
                }
                player.Deck = deck;
                ServerNetwork.GetInstance.SendSingle(player.ClientSK, deck);
            }
        }

        public void StartGame()
        {
            try
            {
                while (Winner == null)
                {
                    CurrentPlayer = GetNextPlayer();
                    
                    Player player = _players.GetPlayerAt(CurrentPlayer);

                    if (!player.Explode)
                    {
                        player.Turn += 1;

                        while (player.Turn > 0)
                        {
                            Console.WriteLine("current player: " + CurrentPlayer.ToString());

                            _Turn = new ManageTurn(player, DrawPile, DiscardPile);

                            foreach (Player p in _players.PlayerList)
                            {
                                MatchInfo matchInfo = new MatchInfo(_players.PlayerList, p.Position, CurrentPlayer, DrawPile.NumOfCard);
                                ServerNetwork.GetInstance.SendSingle(p.ClientSK, matchInfo);
                            }

                            ServerNetwork.GetInstance.SendSingle(player.ClientSK, Requests.YourTurn);

                            Thread StartTurn = new Thread(_Turn.Busy);
                            StartTurn.IsBackground = true;
                            StartTurn.Start();

                            while (!_Turn.EndTurn)
                            {
                                /// AFK detect
                                if (_players.NumOfPlayer == 0)/// if everyone afk
                                {
                                    ResetGame();
                                    return;
                                }
                                if (_players.NumOfPlayer == 1)
                                {
                                    player = _players.GetPlayerAt(0);
                                    Winner = player;
                                    _Turn.EmergencyStop();
                                    break;
                                }
                                else if (!_players.HasPlayer(player))
                                {
                                    if(Direction == 1)
                                    {
                                        if (CurrentPlayer == _players.NumOfPlayer)
                                        {
                                            break;
                                        }
                                        player = _players.GetPlayerAt(CurrentPlayer);
                                    }
                                    else
                                    {
                                        if(CurrentPlayer == 0)
                                        {
                                            player = _players.GetPlayerAt(_players.NumOfPlayer - 1);
                                        }
                                        player = _players.GetPlayerAt(CurrentPlayer - 1);
                                    }
                                }
                                ///
                            }
                            player.Turn--;

                            if (_players.NumOfSurvival == 1)
                            {
                                Winner = _players.GetWinner();
                                break;
                            }
                        }

                        player.Turn = 0;
                    }

                }

                ServerNetwork.GetInstance.SendMulti(new AEndMatch(Winner.Position), _players);
                ResetGame();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("disconnect error");
                ResetGame();
            }
        }

        public int GetNextPlayer()
        {
            CurrentPlayer += Direction;

            if(0 <= CurrentPlayer && CurrentPlayer < _players.NumOfPlayer)
            {
                return CurrentPlayer;
            }
            else
            {
                return (Direction == 1) ? 0 : _players.NumOfPlayer - 1;
            }
        }

        public void EndCurrneTurn()
        {
            _Turn.Stop();
        }


        public void PutBackCard(Player p, int i)
        {
            _Card card = p.Deck.Pop();
            card.Flip();
            if(i < 0 || i > DrawPile.NumOfCard)
            {
                i = 0;
            }

            DrawPile.AddCardAt(i, card);
            _Turn.PositionSend = true;
        }

        public void GiveTopCard(Player player)
        {
            _Card card = DrawPile.Pop();
            SyncSending(player, card);
            ServerNetwork.GetInstance.SendMulti(new ADraw(player.Position, DrawPile.NumOfCard), _players);
            CheckBomb(card);
        }

        public void GiveBottomCard(Player player)
        {
            _Card card = DrawPile.PopBottom();
            SyncSending(player, card);

            CheckBomb(card);
        }

        private void SyncSending(Player player, _Card card)
        {
            if(card != null)
            {
                card.Flip();
                _players.GivePlayerData(player.Position, card);
                ServerNetwork.GetInstance.SendSingle(player.ClientSK, card);
            }
        }

        public void ChangeDirection()
        {
            Direction *= -1;
        }

        public int Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                if (value == 1 || value == -1)
                {
                    _direction = value;
                }
            }
        }

        private void CheckBomb(_Card card)
        {
            if (card is ExplodingCard)
            {
                ServerNetwork.GetInstance.SendMulti(new AGetBoom(CurrentPlayer), _players);
                _Turn.DrawBomb = true;
            }
            else
            {
                _Turn.Stop();
            }
        }

        public void DefuseCurrentTurn()
        {
            _Turn.BombDefuse = true;
        }

        private void RegisterCards()
        {
            _Card.RegisterCard(CardType.ExplodingCard, typeof(ExplodingCard));
            _Card.RegisterCard(CardType.DefuseCard, typeof(DefuseCard));
            _Card.RegisterCard(CardType.SkipCard, typeof(SkipCard));
            _Card.RegisterCard(CardType.CattermelonCard, typeof(CattermelonCard));
            _Card.RegisterCard(CardType.NopeCard, typeof(NopeCard));
            _Card.RegisterCard(CardType.ShuffleCard, typeof(ShuffleCard));
            _Card.RegisterCard(CardType.DrawFromBottomCard, typeof(DrawFromBottomCard));
            _Card.RegisterCard(CardType.ReverseCard, typeof(ReverseCard));
        }
    }
}
