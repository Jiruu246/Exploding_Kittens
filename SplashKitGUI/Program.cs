using System;
using SplashKitSDK;
using Client;

namespace SplashKitGUI
{
    class Program
    {
        static void Main(string[] args)
        {
            new SplashKitAdapter();
            ClientProgram game = new ClientProgram();
            game.Start();
        }
    }
}
