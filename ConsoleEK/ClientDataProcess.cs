using System;
using System.Threading;
using ExplodingKittenLib;
using ExplodingKittenLib.Cards;
using ExplodingKittenLib.Activities;

namespace Client
{
    class ClientDataProcess
    {
        private Player _player;
        private Deck _deck;
        private ClientRequestProcess _reqProc;
        //private int _currentTurn;

        public ClientDataProcess(Player player)
        {
            _player = player;
            _deck = player.Deck;
            _reqProc = new ClientRequestProcess();
        }
        public void Execute(object data)
        {
            if (data is MatchInfo)
            {
                Extract((MatchInfo)data);
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
                Console.WriteLine("this is a card wwooooo");
                GetCard((_Card)data);
                if(data is ExplodingCard)
                {
                    ClientGame.GetInstance.GetBomb = true;
                }
            }
            else if (data is Requests)
            {
                _reqProc.Execute((Requests)data);
            }
            else if(data is Activity)
            {
                if(data is ADraw)
                {
                    ADraw act = data as ADraw;
                    ClientGame.GetInstance.NumOfDrawCard = act.NumOfDrawCard;
                }
                else if(data is APlayCard)
                {
                    APlayCard act = data as APlayCard;
                    ClientGame.GetInstance.DCard = act.Card;
                    //maybe dont need this
                    if(act.Card is DefuseCard)
                    {
                        PlayerInfo.GetInstance.PlayerDefuseBoom(act.Player);
                    }
                }
                else if(data is AGetBoom)
                {
                    AGetBoom act = data as AGetBoom;
                    PlayerInfo.GetInstance.PlayerGetBoom(act.Player);
                }
                else if(data is AEndMatch)
                {
                    AEndMatch act = data as AEndMatch;
                    ClientGame.GetInstance.Winner = act.Player;
                }
                
                ClientGame.GetInstance.Activity = (Activity)data;
            }
        }

        public void SetPlayerPosition(int position)
        {
            _player.Position = position;
        }
        public void SetPlayerTurn(int turns)
        {
            _player.Turn = turns;
        }

        private void MergeDeck(Deck deck)
        {
            _deck.Merge(deck);
        }

        private void GetCard(_Card card)
        {
            _deck.AddCard(card);
        }

        private void Extract(MatchInfo info)
        {
            _player.Position = (info.MyPos >= 0)? info.MyPos : _player.Position;
            _player.Turn = (info.pTurn[_player.Position] >= 0)? info.pTurn[_player.Position] : _player.Turn;
            ClientGame.GetInstance.CurrentTurn = (info.CurrentTurn >= 0)? info.CurrentTurn : ClientGame.GetInstance.CurrentTurn;
            ClientGame.GetInstance.NumOfDrawCard = (info.NumOfDrawCard >= 0) ? info.NumOfDrawCard : ClientGame.GetInstance.NumOfDrawCard;
            PlayerInfo.GetInstance.UpdatePlayer(info);
            _player.Explode = PlayerInfo.GetInstance.Players[_player.Position].Explode;
            ClientGame.GetInstance.GetBomb = false;
        }
    }
}
