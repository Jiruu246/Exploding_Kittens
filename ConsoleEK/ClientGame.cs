using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ExplodingKittenLib;
using ExplodingKittenLib.Cards;
using ExplodingKittenLib.Numbers;

namespace Client
{
    public class ClientGame //the game engine
    {
        private string prom;
        private Player _player;
        private ClientDataProcess _process;
        private List<int> _playernumcards;
        private Deck _deck; //maybe its nescessary
        private int _currentTurn;

        public ClientGame()
        {
            _currentTurn = 0;
            _player = new Player();
            _deck = _player.Deck;
            _process = new ClientDataProcess(_player);

            /// test gui
            _Card.RegisterCard(CardType.ExplodingCard, typeof(ExplodingCard));
            _Card.RegisterCard(CardType.DefuseCard, typeof(DefuseCard));
            _Card.RegisterCard(CardType.SkipCard, typeof(SkipCard));
            _Card.RegisterCard(CardType.CattermelonCard, typeof(CattermelonCard));
            _Card.RegisterCard(CardType.NopeCard, typeof(NopeCard));
            for (int i = 0; i < 9; i++)
            {
                _deck.AddCard(_Card.GetRandom());
            }
            ///

        }

        /// <summary>
        /// for console app
        /// </summary>
        public void StartConsole()
        {
            prom = Console.ReadLine();

            List<string> command = new List<string>(prom.Split(' '));

            switch (command[0])
            {
                case "close":
                    _process.Close();
                    break;
                case "send":
                    _process.Send(command[1]);
                    break;
                case "connect":
                    _process.Connect();
                    break;
                case "pos":
                    Console.WriteLine(_player.Position);
                    break;
                case "start":
                    _process.Send(Requests.Start);
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
                    if (_player.Position == _process.CurrentTurn || playcard is NopeCard)
                    {
                        //everytime play a defuse always stop to ask where to put back (GUI)
                        if(playcard is DefuseCard)
                        {
                            _deck.Pop();
                        }
                        _process.Send(playcard);
                        _deck.RemoveCardAt(int.Parse(command[1]));
                    }
                    else
                    {
                        Console.WriteLine("its not your turn");
                    }
                    break;
                case "draw":
                    _process.Send(Requests.Draw);
                    break;
                case "cardpos":
                    _process.Send(new CardPosition(int.Parse(command[1])));
                    break;
            }

        }


        /// <summary>
        /// for GUI for init the game
        /// </summary>
        public void StartGame()
        {
            throw new NotImplementedException();
        }

        /// length of screen 1600
        public void Update()
        {
            if (_deck.DeckNotEmpty)
            {
                if(_deck.NumOfCard < 10)
                {
                    float gap = 800 / _deck.NumOfCard;
                    float xpos = 300;
                    foreach(_Card card in _deck.CardList)
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
                else if(_deck.NumOfCard < 20)
                {

                }
            }

        }

        public void Draw()
        {
            ///draw the whole deck
            foreach(_Card card in _deck.CardList)
            {
                UIAdapter.GetInstance.DrawCard(card);
            }
        }

        public void ButtonDown()
        {
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
    }
}
