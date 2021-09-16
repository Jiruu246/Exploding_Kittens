using System;
using System.Collections.Generic;
using System.Text;
using ExplodingKittenLib;

namespace Server
{
    class RequestProcessor
    {
        GameModerator _gameMod;
        int _currentSender;
        Requests _currnetReq;
        public RequestProcessor(GameModerator gameMod)
        {
            _currentSender = -1;
            _gameMod = gameMod;
        }
        public void Process(Requests request, Player player)
        {
            _currentSender = player.Position;
            switch (request)
            {
                case Requests.Draw:
                    if (_gameMod.CurrentPlayer == _currentSender)
                    {
                        _gameMod.PlayerDraw = true;
                        _gameMod.GiveTopCard(player);
                    }
                    else
                        _gameMod.SendDeny(player);
                    break;
            }
        }

    }
}
