using System;
using System.Collections.Generic;


namespace ExplodingKittenLib
{
    [Serializable]
    public class MatchInfo
    {
        public List<int> pPos;

        public List<int> pNumOfCard;

        public List<bool> pExplode;

        public int MyPos;
        public int CurrentTurn { get; set; }
        public int NumOfDrawCard { get; set; }
        public MatchInfo(List<Player> players, int playerPos) : this(players)
        {
            MyPos = playerPos;
        }
        public MatchInfo(List<Player> players)
        {
            pPos = new List<int>();

            MyPos = -1;
            CurrentTurn = -1;
            NumOfDrawCard = -1;
            foreach (Player player in players)
            {
                pPos.Add(player.Position);
            }
        }

        public void UpdateTurn(int currentTurn, int playerTurn)
        {
            CurrentTurn = currentTurn;
        }

        public void UpdateNumOfCard(int numOfDrawCard)
        {
            NumOfDrawCard = numOfDrawCard;
        }

        /*public void GetMyData(Player player)
        {
            if(_currP > 0)
            {
                IObservable Iplayer = Players[_currP];
                Console.WriteLine("found match");
                player.Position = (Iplayer.Position > 0) ? Iplayer.Position : player.Position;
                player.Turn = (Iplayer.Turn > 0) ? Iplayer.Turn : player.Turn;
            }
        }*/
    }
}
