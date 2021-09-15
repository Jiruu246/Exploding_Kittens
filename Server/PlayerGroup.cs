using System;
using System.Collections.Generic;
using System.Text;
using ExplodingKittenLib;
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

        public void RemovePlayer(Player player)
        {
            _players.Remove(player);
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
    }
}
