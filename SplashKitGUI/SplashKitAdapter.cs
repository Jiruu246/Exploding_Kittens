using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client;
using SplashKitSDK;
using SplashKitGUI.Screens;
using ExplodingKittenLib.Cards;

namespace SplashKitGUI
{
    public class SplashKitAdapter : UIAdapter
    {
        public SplashKitAdapter() : base()
        {

        }

        public override void CreateWindow(int width, int height)
        {
            new Window("Exploding Kittens", width, height);
        }

        public override void LoadAssets()
        {
            SplashKit.LoadResourceBundle("Assets", "Assets.txt");
        }

        public override void Update()
        {
            Screen.GetInstance.Update();
        }

        public override void Draw()
        {
            SplashKit.ClearScreen(Color.White);
            ///SplashKit.FillRectangle(Color.Blue, 50, 50, 100, 100);
            Screen.GetInstance.Draw();
            SplashKit.RefreshScreen();
        }

        public override void DrawCard(_Card card)
        {
            double X = card.X;
            double Y = card.Y;
            string cardname = card.GetCardname().ToString();

            if (card.Selected)
            {
                SplashKit.FillRectangle(Color.White, X - 2, Y - 2, 260 + 4, 280 + 4);
            }

            if (cardname != null)
            {
                //SplashKit.DrawBitmap(cardname, X, Y);
                SplashKit.DrawBitmap("DefuseCard", X, Y);
            }
            else
            {
                SplashKit.DrawBitmap("backside", X, Y);
            }


        }

        public override void ButtonDown()
        {
            SplashKit.ProcessEvents();
            Screen.GetInstance.ButtonDown();
        }

        public override bool ExitGame()
        {
            return SplashKit.WindowCloseRequested("Exploding Kittens");
        }

    }
}
