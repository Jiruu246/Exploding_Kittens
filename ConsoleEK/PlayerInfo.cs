using System;
using System.Collections.Generic;
using System.Text;
using ExplodingKittenLib;

namespace Client
{
    public class PlayerInfo
    {
        private static PlayerInfo instance;
        private List<OPlayer> _players;
        
        public int MyPos { get; set; }

        private PlayerInfo()
        {
            _players = new List<OPlayer>();
        }

        public static PlayerInfo GetInstance
        {
            get
            {
                if(instance == null)
                {
                    instance = new PlayerInfo();
                }

                return instance;
            }
        }

        public List<OPlayer> Players
        {
            get
            {
                return _players;
            }
        }


        public void PlayerGetBoom(int player)
        {
            _players[player].GetBoom = true;
        }

        public void PlayerDefuseBoom(int player)
        {
            _players[player].GetBoom = false;
        }

        public void UpdatePlayer(MatchInfo info)
        {
            MyPos = info.MyPos;
            _players = new List<OPlayer>();
            for(int i = 0; i < info.pPos.Count; i++)
            {
                OPlayer player = new OPlayer();
                player.Position = CheckData(player.Position, info.pPos[i]);
                player.Turn = CheckData(player.Turn, info.pTurn[i]);
                player.NumOfCard = CheckData(player.NumOfCard, info.pNumOfCard[i]);
                player.Explode = info.pExplode[i];
                _players.Add(player);
            }
        }

        private int CheckData(int target, int input)
        {
            return (input >= 0) ? input : target;
        }
    }
}
