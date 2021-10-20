using System;
using System.Collections.Generic;
using System.Text;
using ExplodingKittenLib;

namespace Client
{
    public class PlayerInfo
    {
        private static PlayerInfo instance;
        private List<Player> _players;

        public int MyPos { get; set; }

        private PlayerInfo()
        {
            _players = new List<Player>();
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

        public List<Player> Players
        {
            get
            {
                return _players;
            }
        }

        public void UpdatePlayer(MatchInfo info)
        {
            MyPos = info.MyPos;
            _players = new List<Player>();
            foreach(int p in info.pPos)
            {
                Player player = new Player();
                player.Position = p;
                _players.Add(player);
            }
        }
    }
}
