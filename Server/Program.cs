using System;
using System.Collections.Generic;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {

            ServerNetwork network = ServerNetwork.GetInstance;

            GameModerator game = new GameModerator();

            Thread Listen = new Thread(game.ListenForPlayer);
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
                        game.Send(command[1]);
                        break;
                    case "drawpile":
                        game.ShowDrawPile();
                        break;
                    case "dispile":
                        game.ShowDisPile();
                        break;
                    case "playerdeck":
                        game.ShowPlayerDeck(int.Parse(command[1]));
                        break;
                }

            }
        }
    }
}
