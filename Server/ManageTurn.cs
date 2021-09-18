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
        private bool _endTurn;
        private bool _drawBomb;
        private bool _bombDefuse;
        private ServerNetwork _network = ServerNetwork.GetInstance();
        private Player _player;


        /// <summary>
        /// consider to put it here
        /// </summary>
        private Deck _drawPile;
        private Deck _discardPile;

        /// <summary>
        /// ???
        /// </summary>
        private int _bombPosition;


        public ManageTurn(Player player, Deck drawPile, Deck discardPile)
        {
            _bombPosition = -1;
            _player = player;
            _drawPile = drawPile;
            _discardPile = discardPile;
            EndTurn = false;
            _drawBomb = false;
            _bombDefuse = false;
        }

        public void Start()
        {
            _network.SendSingle(_player.ClientSK, Requests.YourTurn);

            while (!EndTurn)
            {
                if(_player.Explode == true)
                {
                    continue;
                }
                if (DrawBomb)
                {
                    bool bombremoval = BombCountDown();
                    if (bombremoval)
                    {
                        while (true)
                        {
                            if(_bombPosition > -1)
                            {
                                _drawPile.AddCardAt(_bombPosition, _Card.CreateCard(CardType.Exploding));
                                break;
                            }
                        }
                    }
                    else
                    {
                        _player.Explode = true;
                        _discardPile.Merge(_player.Deck);
                    }
                    EndTurn = true;
                }
            }
        }



        private bool BombCountDown()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine(i); //remove later
                if (BombDefuse)
                {
                    return true;
                }
            }

            return false;
        }


        public bool EndTurn
        {
            get
            {
                return _endTurn;
            }
            set
            {
                _endTurn = value;
            }
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

        public int BombPosition
        {
            get
            {
                return _bombPosition;
            }
            set
            {
                _bombPosition = value;
            }
        }
    }
}
 