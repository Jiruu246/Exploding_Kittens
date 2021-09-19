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

        public CardProcessor(GameModerator gameModerator)
        {
            _gameMod = gameModerator;
        }

        public void Process(_Card card, Player player)
        {
            player.RemoveCard(card);
            //_discardPile.AddCard((_Card)data);

            IActivatable Acard = card as IActivatable;
            Execute(Acard.Activate(), player);
        }

        public void Execute(List<Actions> actions, Player player)
        {
            foreach(Actions action in actions)
            {
                switch (action)
                {
                    case Actions.Defuse:
                        player.Deck.Pop();
                        _gameMod.DefuseCurrentTurn();
                        break;
                }
            }
        }
    }
}
