using System;
using System.Collections.Generic;
using System.Text;
using ExplodingKittenLib;
using ExplodingKittenLib.Cards;
using System.Net.Sockets;

namespace Server
{
    class PlayerGroup
    {
        private List<Player> _players;
        private int _max;

        public PlayerGroup()
        {
            _max = 5;
            _players = new List<Player>();
        }

        public bool MaxPlayer()
        {
            return _players.Count == _max;
        }

        public Player AddPlayer(Socket sk)
        {
            Player player = new Player();
            player.ClientSK = sk;
            _players.Add(player);
            //get position after add
            player.Position = GetPosition(player);
            return player;
        }

        public void ResetTurn()
        {
            foreach(Player player in _players)
            {
                if (!player.Explode)
                {
                    player.Turn = 1;
                }
                else
                {
                    player.Turn = 0;
                }
            }
        }

        public void RemovePlayerAt(int i)
        {
            _players.RemoveAt(i);
            // rework the position
            foreach(Player p in _players)
            {
                p.Position = GetPosition(p);
            }
        }

        public int GetPosition(Player player)
        {
            return _players.IndexOf(player);
        }

        public int NumOfPlayer
        {
            get
            {
                return _players.Count;
            }
        }

        public List<Player> PlayerList
        {
            get
            {
                return _players;
            }
        }

        public int NumOfSurvival
        {
            
            get
            {
                int i = 0;
                foreach(Player p in _players)
                {
                    if(p.Explode == false)
                    {
                        i++;
                    }
                }
                return i;
            }
        }

        public Player GetWinner()
        {
            if(NumOfSurvival == 1)
            {
                foreach(Player p in _players)
                {
                    if(p.Explode == false)
                    {
                        return p;
                    }
                }
            }
            return null;
        }

        public Player GetPlayerAt(int i)
        {
            return _players[i];
        }

        public void GivePlayerData(int playerpos, _Card card)
        {
            _players[playerpos].GetCard(card);
        }

        public bool HasPlayer(Player player)
        {
            return _players.Contains(player);
        }
    }
}
