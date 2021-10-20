using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ExplodingKittenLib;
using ExplodingKittenLib.Cards;

namespace Server
{
    class CardProcessor
    {
        private Game _game;
        private Deck _drawPile;
        private Deck _disPile;
        private int _currentSender;
        private bool _nopeSend;

        public CardProcessor(Game game, Deck drawPile, Deck disPile)
        {
            _nopeSend = false;
            _currentSender = -1;
            _game = game;
            _drawPile = drawPile;
            _disPile = disPile;
        }

        public void Process(_Card card, Player player)
        {
            _currentSender = player.Position;
            if(card is IActivatable)
            {
                if(card is NopeCard)
                {
                    player.RemoveCard(card);
                    _disPile.AddCard(card);
                    IActivatable Acard = card as IActivatable;
                    Execute(Acard.Activate(), player);
                }
                else if(_game.CurrentPlayer == _currentSender)
                {
                    player.RemoveCard(card);
                    _disPile.AddCard(card);

                    if (card is DefuseCard || !GetNope())
                    {
                        IActivatable Acard = card as IActivatable;
                        Execute(Acard.Activate(), player);
                    }
                }
                else
                {
                    _game.SendDeny(player);
                    return;
                }
            }

        }

        private bool GetNope()
        {
            for(int i = 0; i < 6; i++)
            {
                _nopeSend = false;
                Thread.Sleep(500);
                if (_nopeSend)
                {
                    _nopeSend = false;
                    return !GetNope();
                }
            }
            return false;
        }

        public void Execute(List<Actions> actions, Player player)
        {
            foreach(Actions action in actions)
            {
                switch (action)
                {
                    case Actions.Defuse:
                        player.Deck.Pop();
                        _game.DefuseCurrentTurn();
                        break;
                    case Actions.Nope:
                        _nopeSend = true;
                        break;
                    case Actions.Skip:
                        _game.ReduceTurn(player);
                        break;
                }
            }
        }
    }
}
