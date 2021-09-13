﻿using System;
using System.Collections.Generic;
using System.Text;
using ExplodingKittenLib;

namespace Client
{
    class ClientGame //the game engine
    {
        private string prom;
        private Player _player;
        private ClientDataProcess _process;
        private List<int> _playernumcards;
        private Deck _deck;
        private bool _ready;

        public ClientGame()
        {
            _player = new Player();
            _ready = false;
            _process = new ClientDataProcess(_player, _deck);

        }
        public void Update()
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
                    _ready = _process.Connect();
                    break;
            }

        }
    }
}
