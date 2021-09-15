using System;
using System.Collections.Generic;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            PlayerGroup players = new PlayerGroup();

            ServerNetwork network = ServerNetwork.GetInstance();

            GameModerator game = new GameModerator(players);

            Thread Listen = new Thread(game.Listen);
            Listen.IsBackground = true;
            Listen.Start();


            string prom; //temp 


            while (true)
            {

                prom = Console.ReadLine();

                List<string> command = new List<string>(prom.Split(' '));

                switch (command[0])
                {
                    case "close":
                        network.Close();
                        break;
                    case "send":
                        network.SendMulti(command[1], players);
                        break;
                }

            }
        }
    }
}
