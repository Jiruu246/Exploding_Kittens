using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args) // use this for console application
        {
          

            while (true)
            {
                ClientGame.GetInstance.RunConsole();
            }
        }
    }
}
