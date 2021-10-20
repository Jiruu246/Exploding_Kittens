using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client;
using SplashKitSDK;
using SplashKitGUI.Screens;
using ExplodingKittenLib;
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
            SplashKit.RefreshScreen(60);
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

        public override void DrawPlayerInLobby(PlayerInfo playerinf)
        {
            int x = 350;
            int y = 60;
            foreach(Player p in playerinf.Players)
            {
                SplashKit.DrawBitmap("idvholder", x, y);
                SplashKit.DrawBitmap("avatar" + (p.Position + 1), x + 20, y + 15);
                SplashKit.DrawText("Player " + (p.Position + 1), Color.White, "lobby_player_text", 50,  x + 180, y + 25);
                if(p.Position == playerinf.MyPos)
                {
                    SplashKit.DrawText("(You)", Color.LightGray, "lobby_player_text", 40, x + 200, y + 80);
                }
                y += 160;
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
