using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using ExplodingKittenLib.Cards;

namespace ExplodingKittenLib
{
    public class Player
    {
        private Deck _deck;
        private Socket _clientSK;
        private int _position;
        private bool _roomMaster;
        private int _turn;
        private bool _explode;

        public Player()
        {
            _deck = new Deck();
            _roomMaster = false;
            _turn = 1;
            _explode = false;
        }

        public int Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                if (_position == 0)
                    _roomMaster = true;
                else
                    _roomMaster = false;
            }
        }

        public bool RoomMaster
        {
            get
            {
                return _roomMaster;
            }
        }
        public Socket ClientSK
        {
            get
            {
                return _clientSK;
            }

            set
            {
                _clientSK = value;
            }
        }
        public Deck Deck
        {
            get
            {
                return _deck;
            }

            set
            {
                _deck = value;
            }
        }

        public int Turn
        {
            get
            {
                return _turn;
            }

            set
            {
                _turn = value;
            }
        }

        public bool Explode
        {
            get
            {
                return _explode;
            }

            set
            {
                _explode = value;
            }
        }

        public void GetCard(_Card card)
        {
            _deck.AddCard(card);
        }

    }
}
