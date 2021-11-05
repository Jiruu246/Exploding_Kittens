using System;
using System.Collections.Generic;
using System.Text;
using ExplodingKittenLib;

namespace Client
{
    public class ClientRequestProcess
    {
        public ClientRequestProcess()
        {

        }

        public void Execute(Requests requests)
        {
            switch (requests)
            {
                case Requests.YourTurn:
                    //
                    break;
                case Requests.Start:
                    ClientGame.GetInstance.GameStart = true;
                    break;
            }
        }
    }
}
