using System;
using System.Collections.Generic;
using System.Text;
using Client;
using SplashKitSDK;

namespace SplashKitGUI.Screens
{
    class ScreenGame : ScreenState
    {
        public ScreenGame()
        {

        }
        public void Update()
        {
            ClientGame.GetInstance.Update();
        }

        public void Draw()
        {
            SplashKit.DrawBitmap("gamebackground", 0, 0);
            ClientGame.GetInstance.Draw(); 

        }

        public void ButtonDown()
        {
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                double X = SplashKit.MousePosition().X;
                double Y = SplashKit.MousePosition().Y;

                ClientGame.GetInstance.ChooseCardAt(X, Y);
            }
        }
    }
}
