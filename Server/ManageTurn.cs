using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using ExplodingKittenLib;
using ExplodingKittenLib.Cards;
namespace Server
{
    class ManageTurn
    {
        public bool EndTurn;
        private bool _emergency;
        private bool _drawBomb;
        private bool _bombDefuse;
        private Player _player;


        /// <summary>
        /// consider to put it here
        /// </summary>
        private Deck _drawPile;
        private Deck _discardPile;

        public bool PositionSend;


        public ManageTurn(Player player, Deck drawPile, Deck discardPile)
        {
            PositionSend = false;
            _player = player;
            _drawPile = drawPile;
            _discardPile = discardPile;
            EndTurn = false;
            _emergency = false;
            _drawBomb = false;
            _bombDefuse = false;
        }

        public void Busy()
        {
            while (!EndTurn)
            {
                if (_player.Turn == 0)
                {
                    EndTurn = true;
                }
                if (DrawBomb)
                {
                    _bombDefuse = false;
                    bool bombremoval = BombCountDown();
                    if (bombremoval)
                    {
                        while (!_emergency)
                        {
                            if(PositionSend)
                            {
                                //_drawPile.AddCardAt(_bombPosition, _Card.CreateCard(CardType.ExplodingCard));
                                break;
                            }
                        }
                    }
                    else
                    {
                        _player.Explode = true;
                        _player.Turn = 0;
                        _player.Deck.Pop();
                        _discardPile.Merge(_player.Deck);
                        _player.Deck = new Deck();
                    }

                    EndTurn = true;
                }
            }
        }

        public void Stop()
        {
            EndTurn = true;
        }

        public void EmergencyStop()
        {
            _emergency = true;
            EndTurn = true;
        }

        private bool BombCountDown()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine(i); //remove later
                if (BombDefuse || _emergency)
                {
                    return true;
                }
            }

            return false;
        }

        public bool DrawBomb
        {
            get
            {
                return _drawBomb;
            }
            set
            {
                _drawBomb = value;
            }
        }

        public bool BombDefuse
        {
            get
            {
                return _bombDefuse;
            }
            set
            {
                _bombDefuse = value;
            }
        }

    }
}
 