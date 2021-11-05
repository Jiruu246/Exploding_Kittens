using System;
using System.Collections.Generic;


namespace ExplodingKittenLib
{
    [Serializable]
    public class MatchInfo
    {
        public List<int> pPos;

        public List<int> pTurn;

        public List<int> pNumOfCard;

        public List<bool> pExplode;

        public int MyPos;
        public int CurrentTurn { get; set; }
        public int NumOfDrawCard { get; set; }

        public MatchInfo(List<Player> players, int playerPos, int curTurn = -1, int numOfDraw = -1)
        {
            pPos = new List<int>();
            pTurn = new List<int>();
            pNumOfCard = new List<int>();
            pExplode = new List<bool>();

            MyPos = playerPos;
            CurrentTurn = curTurn;
            NumOfDrawCard = numOfDraw;
            foreach (Player player in players)
            {
                pPos.Add(player.Position);
                pTurn.Add(player.Turn);
                pNumOfCard.Add(player.NumOfCard);
                pExplode.Add(player.Explode);
            }
        }

    }
}
