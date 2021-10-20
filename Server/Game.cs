using System;
using System.Collections.Generic;
using System.Text;
using ExplodingKittenLib;
using ExplodingKittenLib.Cards;

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
        private ManageTurn CurrentTurn;

        public Game()
        {
            ResetGame();
            RegisterCards();
        }

        public void ProcessCard(_Card card, Player player)
        {
            _cardProc.Process(card, player);
        }

        public void ResetGame()
        {
            Winner = null;
            Start = false;
            DiscardPile = new Deck();
            _cardProc = new CardProcessor(this, DrawPile, DiscardPile);
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
                    deck.AddCard(_Card.GetRandom());
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
                    ResetTurn(); //exploded player will have 0 turn
                    for (int i = 0; i < _players.NumOfPlayer; i += Direction)
                    {
                        CurrentPlayer = i;
                        Player player = _players.GetPlayerAt(i);

                        while (player.Turn > 0)
                        {

                            CurrentTurn = new ManageTurn(player, DrawPile,  DiscardPile);

                            //throw new NotImplementedException();
                            //_network.SendMulti(new CurrentTurn(_currentP), _players);

                            while (!CurrentTurn.EndTurn)
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
                                    break;
                                }
                                else if (!_players.HasPlayer(player))
                                {
                                    if (i == _players.NumOfPlayer)
                                    {
                                        break;
                                    }
                                    player = _players.PlayerList[i];
                                    //throw new NotImplementedException();
                                    //_network.SendMulti(new CurrentTurn(_currentP), _players);
                                }
                                ///

                                CurrentTurn.Busy();
                            }

                            ReduceTurn(player);

                            if (_players.NumOfSurvival == 1)
                            {
                                Winner = _players.GetWinner();
                                break;
                            }
                        }

                        if (Winner != null)
                        {
                            break;
                        }
                    }

                }

                ServerNetwork.GetInstance.SendSingle(Winner.ClientSK, "you winnnnn!!!!!!!");
                ResetGame();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("disconnect error");
                ResetGame();
            }
        }

        public void ResetTurn()
        {
            _players.ResetTurn();
        }

        public void ReduceTurn(Player player)
        {
            player.Turn--;
            //throw new NotImplementedException();
            //_network.SendSingle(player.ClientSK, player.GetTurn);
        }

        public void GiveTopCard(Player player)
        {
            _Card card = DrawPile.Pop();
            SyncSending(player, card);

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
            _players.GivePlayerData(player.Position, card);
            ServerNetwork.GetInstance.SendSingle(player.ClientSK, card);
        }

        public void SendDeny(Player player)
        {
            ServerNetwork.GetInstance.SendSingle(player.ClientSK, "Deny");
        }

        public void ChangeDirection()
        {
            Direction *= -1;
            ResetTurn();
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
                CurrentTurn.DrawBomb = true;
            }
            else
            {
                CurrentTurn.Stop();
            }
        }

        public void DefuseCurrentTurn()
        {
            CurrentTurn.BombDefuse = true;
        }

        private void RegisterCards()
        {
            _Card.RegisterCard(CardType.ExplodingCard, typeof(ExplodingCard));
            _Card.RegisterCard(CardType.DefuseCard, typeof(DefuseCard));
            _Card.RegisterCard(CardType.SkipCard, typeof(SkipCard));
            _Card.RegisterCard(CardType.CattermelonCard, typeof(CattermelonCard));
            _Card.RegisterCard(CardType.NopeCard, typeof(NopeCard));
        }
    }
}
