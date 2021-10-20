using System;
using System.Collections.Generic;
using System.Text;
using Client;
using SplashKitSDK;

namespace SplashKitGUI.Screens
{
    class ScreenMenu : ScreenState
    {


        public ScreenMenu()
        {

        }
        public void Update()
        {

        }

        public void Draw()
        {
            SplashKit.DrawBitmap("menu", 0, 0);
            SplashKit.DrawBitmap("startbttn", 650, 200);
        }

        public void ButtonDown()
        {
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                if(Screen.GetInstance.MouseInZone(650, 200, 299, 136))
                {
                    //connect after press start
                    //ClientGame.GetInstance.Connect();
                    Screen.GetInstance.ChangeState(new ScreenLobby());
                }
            }
        }
    }
}
