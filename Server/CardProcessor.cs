using System;
using System.Collections.Generic;
using System.Text;
using ExplodingKittenLib;
using ExplodingKittenLib.Cards;

namespace Server
{
    class CardProcessor
    {
        private GameModerator _gameMod;
        int _currentSender;

        public CardProcessor(GameModerator gameModerator)
        {
            _gameMod = gameModerator;
        }

        public void Process(_Card card, Player player)
        {
            _currentSender = player.Position;
            IActivatable Acard = card as IActivatable;
            Execute(Acard.Activate());
        }

        public void Execute(List<Actions> actions)
        {
            foreach(Actions action in actions)
            {
                switch (action)
                {
                    case Actions.Defuse:
                        _gameMod.DefuseCurrentTurn();
                        break;
                }
            }
        }
    }
}
