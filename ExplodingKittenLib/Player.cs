using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace ExplodingKittenLib
{
    public class Player
    {
        private Deck _deck;
        private Socket _clientSK;
        private int _position;

        public Player()
        {
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

    }
}
