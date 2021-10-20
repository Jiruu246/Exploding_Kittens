using System;
using System.Collections.Generic;
using System.Text;
using ExplodingKittenLib;

namespace Server
{
    class RequestProcessor
    {
        GameModerator _gameMod;
        Game _game;
        int _currentSender;

        public RequestProcessor(GameModerator gameMod, Game game)
        {
            _currentSender = -1;
            _gameMod = gameMod;
            _game = game;
        }
        public void Process(Requests request, Player player)
        {
            _currentSender = player.Position;
            switch (request)
            {
                case Requests.Start:
                    if (player.RoomMaster)
                    {
                    _gameMod.CreateGame();
                    }
                    break;
                case Requests.Draw:
                    if (_game.CurrentPlayer == _currentSender)
                    {
                        _game.GiveTopCard(player);
                    }
                    else
                        _gameMod.SendDeny(player);
                    break;
            }
        }

    }
}
