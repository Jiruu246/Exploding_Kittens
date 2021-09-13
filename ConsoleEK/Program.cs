using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args) // this will be the main program that control every thing
        {
          

            Menu menu = new Menu();

            ClientGame game = new ClientGame();

            while (true)
            {
                game.Update();
            }
        }
    }
}
