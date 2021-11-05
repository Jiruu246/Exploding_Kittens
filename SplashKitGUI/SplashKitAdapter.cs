using System;
using Client;
using SplashKitSDK;
using SplashKitGUI.Screens;
using ExplodingKittenLib.Activities;
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
            string cardname = card.GetCardname();

            //if (card.Selected)
            //{
            //    SplashKit.DrawBitmap("selected", X - 20, Y - 19);
            //}

            if (card.FaceUp)
            {
                SplashKit.DrawBitmap(cardname, X, Y);
                //SplashKit.DrawBitmap("DefuseCard", X, Y);
            }
            else
            {
                SplashKit.DrawBitmap("backside", X, Y);
            }
        }

        public override void DrawActivity(Activity activity)
        {
            SplashKit.DrawText(activity.Description(), Color.White, "main_font", 25, 450, 480);
        }

        public override void DrawGameDeck(int NumOfDrawCard, _Card Dcard)
        {
            SplashKit.DrawText(NumOfDrawCard.ToString(), Color.White, "main_font", 75, 360, 250);
            SplashKit.DrawText("Cards", Color.RGBColor(192, 0, 33), "main_font", 20, 370, 330);

            if(Dcard != null)
            {
                //SplashKit.DrawBitmap("DefuseCard", 1000, 210);
                SplashKit.DrawBitmap(Dcard.GetCardname(), 1000, 210);
            }
        }

        public override void DrawPlayerInLobby(PlayerInfo playerinf)
        {
            int x = 350;
            int y = 60;
            foreach(OPlayer p in playerinf.Players)
            {
                SplashKit.DrawBitmap("idvholder", x, y);
                SplashKit.DrawBitmap("avatar" + (p.Position + 1), x + 20, y + 15);
                SplashKit.DrawText("Player " + (p.Position + 1), Color.White, "main_font", 50,  x + 180, y + 25);
                if(p.Position == playerinf.MyPos)
                {
                    SplashKit.DrawText("(You)", Color.LightGray, "main_font", 40, x + 200, y + 80);
                }
                y += 160;
            }
        }

        public override void DrawPlayerInGame(PlayerInfo playerInfo)
        {
            int x = 50;
            int y = 30;
            foreach(OPlayer p in playerInfo.Players)
            {
                if(p.Position == playerInfo.MyPos)
                {
                    if (!p.Explode)
                    {
                        SplashKit.DrawBitmap("avatar" + (p.Position + 1), 80, 360);
                    }
                    else
                    {
                        SplashKit.DrawBitmap("dead", 80, 360);
                    }
                    SplashKit.DrawText("Player " + (p.Position + 1) + " (You)", Color.White, "main_font", 40, 20, 490);

                    if(ClientGame.GetInstance.CurrentTurn == p.Position)
                    {
                        SplashKit.FillTriangle(Color.White, 120, 320, 160, 320, 140, 335);
                        SplashKit.DrawText("Your Turn x" + p.Turn.ToString(), Color.White, "main_font", 35, 30, 280);
                    }
                }
                else
                {
                    if (!p.Explode)
                    {
                        SplashKit.DrawBitmap("avatar" + (p.Position + 1), x, y);
                    }
                    else
                    {
                        SplashKit.DrawBitmap("dead", x, y);
                    }
                    SplashKit.DrawText("Player " + (p.Position + 1), Color.White, "main_font", 30, x, y + 100);
                    SplashKit.DrawBitmap("oppcard", x + 140, y + 10);
                    SplashKit.DrawText(p.NumOfCard.ToString(), Color.Black, "main_font", 28, x + 260, y + 85);
                    if (p.GetBoom)
                    {
                        SplashKit.DrawBitmap("boom", x + 170, y + 15);
                    }
                    if (ClientGame.GetInstance.CurrentTurn == p.Position)
                    {
                        SplashKit.FillTriangle(Color.White, x + 50, y + 145, x + 70, y + 145, x + 60, y + 135);
                        SplashKit.DrawText("Turn x" + p.Turn.ToString(), Color.White, "main_font", 25, x + 10, y + 150);
                    }
                    x += 400;
                }
            }
        }

        public override void DrawBombWarning()
        {
            SplashKit.FillRectangle(Color.RGBColor(34,34,59), 0, 50, 1600, 400);
            SplashKit.DrawText("Oh no! You Got A BOMB!!", Color.White, "main_font", 50, 500, 100);
            SplashKit.DrawText("Quick, use a DEFUSE before it's too late!!!", Color.White, "main_font", 50, 300, 190);
            SplashKit.DrawBitmap("boom", 700, 300);
        }

        public override void DrawChoosingIndex(int index)
        {
            SplashKit.FillRectangle(Color.RGBColor(34, 34, 59), 0, 50, 1600, 400);
            SplashKit.DrawText("Where do you want to put it back?", Color.White, "main_font", 50, 30, 200);
            SplashKit.DrawBitmap("indexbttn", 900, 100);
            SplashKit.DrawText("TOP", Color.White, "main_font", 50, 1000, 120);
            SplashKit.DrawBitmap("indexselect", 900, 200);
            SplashKit.DrawText(index.ToString(), Color.Black, "main_font", 35, 1050, 220);
            SplashKit.DrawBitmap("okbttn", 1250, 200);
            SplashKit.DrawBitmap("indexbttn", 900, 300);
            SplashKit.DrawText("BOTTOM", Color.White, "main_font", 50, 950, 320);
            SplashKit.DrawBitmap("indexbttn", 500, 300);
            SplashKit.DrawText("RANDOM", Color.White, "main_font", 50, 550, 320);


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
