using System;


namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            PlayerGroup players = new PlayerGroup();

            ServerNetwork network = new ServerNetwork(players);

            GameModerator game = new GameModerator(network, players);

            while (true)
            {
                game.Update();
            }
        }
    }
}
