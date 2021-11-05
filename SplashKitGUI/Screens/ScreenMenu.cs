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
            SplashKit.DrawBitmap("startbttn", 750, 600);
            SplashKit.DrawBitmap("quitbttn", 750, 750);
        }

        public void ButtonDown()
        {
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                if(Screen.GetInstance.MouseInZone(750, 600, 193, 94))
                {
                    Screen.GetInstance.ChangeState(new ScreenLobby());
                }
                else if(Screen.GetInstance.MouseInZone(750, 750, 193, 94))
                {
                    SplashKit.CloseAllWindows();
                }
            }
        }
    }
}
