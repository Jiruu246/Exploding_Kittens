using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args) // use this for console application
        {
          
            ClientGame game = new ClientGame();

            while (true)
            {
                game.StartConsole();
            }
        }
    }
}
