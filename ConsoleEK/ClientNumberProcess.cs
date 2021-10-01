using System;
using System.Collections.Generic;
using System.Text;
using ExplodingKittenLib.Numbers;

namespace Client
{
    class ClientNumberProcess
    {
        ClientDataProcess _process;
        public ClientNumberProcess(ClientDataProcess process)
        {
            _process = process;
        }

        public void Execute(Numbers number)
        {
            switch (number.GetType().Name)
            {
                case "Position":
                    _process.SetPlayerPosition(number.Get);
                    break;
                case "Turn":
                    _process.SetPlayerTurn(number.Get);
                    break;
                case "CurrentTurn":
                    _process.CurrentTurn = number.Get;
                    break;
            }
        }
    }
}
