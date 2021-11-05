using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ExplodingKittenLib;
using ExplodingKittenLib.Cards;
using ExplodingKittenLib.Activities;

namespace Server
{
    class CardProcessor
    {
        private Game _game;
        private int _currentSender;
        private bool _nopeSend;
        private bool _processing;

        public CardProcessor(Game game)
        {
            _nopeSend = false;
            _processing = false;
            _currentSender = -1;
            _game = game;
        }

        public void Process(_Card card, Player player, PlayerGroup players)
        {
            _currentSender = player.Position;
            //if(card is IActivatable)
            //{
            //    if(card is NopeCard)
            //    {
            //        ServerNetwork.GetInstance.SendMulti(new APlayCard(player.Position, card), players);
            //        player.RemoveCard(card);
            //        _disPile.AddCard(card);
            //        IActivatable Acard = card as IActivatable;
            //        Execute(Acard.Activate(), player);
            //    }
            //    else if(_game.CurrentPlayer == _currentSender)
            //    {
            //        ServerNetwork.GetInstance.SendMulti(new APlayCard(player.Position, card), players);
            //        player.RemoveCard(card);
            //        _disPile.AddCard(card);

            //        if (card is DefuseCard || !GetNope())
            //        {
            //            IActivatable Acard = card as IActivatable;
            //            Execute(Acard.Activate(), player);
            //        }
            //    }
            //    else
            //    {
            //        ServerNetwork.GetInstance.SendSingle(player.ClientSK, Requests.Deny);
            //        //return the card if it's not their turn
            //        ServerNetwork.GetInstance.SendSingle(player.ClientSK, card);
            //        return;
            //    }
            //}
            if(card is NopeCard)
            {
                ServerNetwork.GetInstance.SendMulti(new APlayCard(player.Position, card), players);
                player.RemoveCard(card);
                _game.DiscardPile.AddCard(card);
                IActivatable Acard = card as IActivatable;
                Execute(Acard.Activate(), player);
            }
            else if(_currentSender == _game.CurrentPlayer)
            {
                if (_processing)
                {
                    //return the card if the previous one is still processing
                    ServerNetwork.GetInstance.SendSingle(player.ClientSK, card);
                    return;
                }

                ServerNetwork.GetInstance.SendMulti(new APlayCard(player.Position, card), players);
                if(card is IActivatable)
                {
                    player.RemoveCard(card);
                    _game.DiscardPile.AddCard(card);

                    if (card is DefuseCard || !GetNope())
                    {
                        IActivatable Acard = card as IActivatable;
                        Execute(Acard.Activate(), player);
                        _processing = false;
                    }
                }
            }
            else
            {
                //return the card if it's not their turn 
                ServerNetwork.GetInstance.SendSingle(player.ClientSK, card);
                return;
            }

        }

        private bool GetNope()
        {
            _processing = true;
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
                        _game.DefuseCurrentTurn();
                        break;
                    case Actions.Nope:
                        _nopeSend = true;
                        break;
                    case Actions.Skip:
                        _game.EndCurrneTurn();
                        break;
                    case Actions.Shuffle:
                        _game.DrawPile.Shuffle();
                        break;
                    case Actions.DrawFromBottom:
                        _game.GiveBottomCard(player);
                        break;
                    case Actions.Reverse:
                        _game.ChangeDirection();
                        break;
                }
            }
        }
    }
}
