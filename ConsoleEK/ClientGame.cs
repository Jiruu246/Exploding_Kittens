using System;
using System.Collections.Generic;
using System.Text;
using ExplodingKittenLib;
using ExplodingKittenLib.Cards;
using ExplodingKittenLib.Numbers;

namespace Client
{
    class ClientGame //the game engine
    {
        private string prom;
        private Player _player;
        private ClientDataProcess _process;
        private List<int> _playernumcards;
        private Deck _deck; //maybe its nescessary
        private bool _ready;

        public ClientGame()
        {
            _player = new Player();
            _deck = _player.Deck;
            _ready = false;
            _process = new ClientDataProcess(_player);

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
                    _process.Send(_deck.CardList[int.Parse(command[1])]);
                    _deck.RemoveCardAt(int.Parse(command[1]));
                    break;
                case "draw":
                    _process.Send(Requests.Draw);
                    break;
                case "cardpos":
                    _process.Send(new CardPosition(int.Parse(command[1])));
                    break;
            }

        }
    }
}
