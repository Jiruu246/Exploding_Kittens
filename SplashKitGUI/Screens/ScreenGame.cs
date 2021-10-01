using System;
using System.Collections.Generic;
using System.Text;
using Client;
using SplashKitSDK;

namespace SplashKitGUI.Screens
{
    class ScreenGame : ScreenState
    {
        private ClientGame _game;
        public ScreenGame()
        {
            _game = new ClientGame();
        }
        public void Update()
        {
            _game.Update();
        }

        public void Draw()
        {
            SplashKit.DrawBitmap("gamebackground", 0, 0);
            _game.Draw(); 

        }

        public void ButtonDown()
        {
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                double X = SplashKit.MousePosition().X;
                double Y = SplashKit.MousePosition().Y;

                _game.ChooseCardAt(X, Y);
            }
        }
    }
}
