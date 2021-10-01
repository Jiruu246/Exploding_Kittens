using System;
using System.Collections.Generic;
using System.Text;
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
                Screen.GetInstance.ChangeState(new ScreenLobby());
            }
        }
    }
}
