﻿using System;
using System.Threading;
using ExplodingKittenLib;
using ExplodingKittenLib.Cards;

namespace Client
{
    class ClientDataProcess //facade pattern
    {
        private ClientNetwork _network = ClientNetwork.GetInstance();
        private Player _player;
        private Deck _deck;
        private ClientRequestProcess _reqProc;

        public ClientDataProcess(Player player)
        {
            _player = player;
            _deck = _player.Deck;
            _reqProc = new ClientRequestProcess();
        }
        public void Execute(object data)
        {
            if (data is int)
            {
                Console.WriteLine((int)data);
            }
            else if (data is String)
            {
                Console.WriteLine((string)data);
            }
            else if(data is Deck)
            {
                MergeDeck((Deck)data);
            }
            else if (data is _Card)
            {
                Console.WriteLine("this is a card wwooooo"); //just get a card
                GetCard((_Card)data);
            }
            else if (data is Requests)
            {
                //put it in the request processor
                _reqProc.Execute((Requests)data);
            }


        }

        public bool Connect()
        {
            bool conn = _network.Connect();

            _player.ClientSK = _network.Socket; // save the socket to the player object

            Thread listen = new Thread(Receive); // when connect establish a listen thread immidiately
            listen.IsBackground = true;
            listen.Start();

            return conn;
        }

        public void Close()
        {
            _network.Close();
        }

        public void Send(object data)
        {
            _network.Send(data);
        }

        public void Receive()
        {
            try
            {
                while (true)
                {
                    Execute(_network.GetData(_player.ClientSK));
                }
            }
            catch
            {
                _network.Close();
            }
        }

        private void SetPlayerPosition(int position)
        {
            _player.Position = position;
        }

        private void MergeDeck(Deck deck)
        {
            _deck.Merge(deck);
        }

        private void GetCard(_Card card)
        {
            _deck.AddCard(card);
        }
    }
}
