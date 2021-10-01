using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using ExplodingKittenLib.Cards;
using ExplodingKittenLib.Numbers;

namespace ExplodingKittenLib
{
    public class Player
    {
        private Deck _deck;
        private Socket _clientSK;
        private Position _position;
        private bool _roomMaster;
        private Turn _turn;
        private bool _explode;

        public Player()
        {
            _position = new Position(0);
            _deck = new Deck();
            _roomMaster = false;
            _turn = new Turn(1);
            _explode = false;
        }

        public int Position
        {
            get
            {
                return _position.Get;
            }
            set
            {
                _position.Get = value;
                if (_position.Get == 0)
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
                return _turn.Get;
            }

            set
            {
                _turn.Get = value;
            }
        }

        public Turn GetTurn
        {
            get
            {
                return _turn;
            }
        }

        public Position GetPosition
        {
            get
            {
                return _position;
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

        public void RemoveCard(_Card card)
        {
            _deck.RemoveCard(card);
        }

    }
}
