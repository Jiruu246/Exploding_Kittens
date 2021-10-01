using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    public class ClientProgram
    {

        public ClientProgram()
        {
            UIAdapter.GetInstance.CreateWindow(1600, 900);
            UIAdapter.GetInstance.LoadAssets();
        }

        public void Start()
        {
            UIAdapter GameUI = UIAdapter.GetInstance;
            while (!GameUI.ExitGame())
            {
                GameUI.Update();
                GameUI.Draw();
                GameUI.ButtonDown();
            }
        }
    }
}
