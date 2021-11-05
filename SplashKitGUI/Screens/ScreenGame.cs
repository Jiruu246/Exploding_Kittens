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
            if(ClientGame.GetInstance.Winner >= 0)
            {
                Screen.GetInstance.ChangeState(new ScreenEnd());
            }
            ClientGame.GetInstance.UpdateInGame();

        }

        public void Draw()
        {
            SplashKit.DrawBitmap("gamebackground", 0, 0);
            SplashKit.DrawBitmap("drawPile", 550, 230);
            SplashKit.DrawBitmap("drawbttn", 300, 350);
            ClientGame.GetInstance.DrawInGame();
            SplashKit.DrawBitmap("playCard", 1300, 460);

        }

        public void ButtonDown()
        {
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                double X = SplashKit.MousePosition().X;
                double Y = SplashKit.MousePosition().Y;
                if(Y > 530)
                {
                    ClientGame.GetInstance.ChooseCardAt(X, Y);
                }
                else
                {
                    if(Screen.GetInstance.MouseInZone(300, 350, 193, 94))
                    {
                        ClientGame.GetInstance.DrawACard();
                    }
                    else if (Screen.GetInstance.MouseInZone(1300, 460, 200, 63))
                    {
                        ClientGame.GetInstance.PlayCard();
                    }
                    else if (ClientGame.GetInstance.ChoosingIndex)
                    {

                        if (Screen.GetInstance.MouseInZone(900,100,320,85))
                        {
                            ClientGame.GetInstance.PutBackAt(ClientGame.GetInstance.NumOfDrawCard);
                        }
                        else if (Screen.GetInstance.MouseInZone(900,200,75,85))
                        {
                            ClientGame.GetInstance.Index -= 1;
                        }
                        else if (Screen.GetInstance.MouseInZone(1149,200,75,85))
                        {
                            ClientGame.GetInstance.Index += 1;
                        }
                        else if (Screen.GetInstance.MouseInZone(900,300,320,85))
                        {
                            ClientGame.GetInstance.PutBackAt(0);
                        }
                        else if (Screen.GetInstance.MouseInZone(550,320,200,60))
                        {
                            Random rnd = new Random();
                            int number = rnd.Next(0, ClientGame.GetInstance.NumOfDrawCard);
                            ClientGame.GetInstance.PutBackAt(number);
                        }
                        else if (Screen.GetInstance.MouseInZone(1250, 200, 85, 85))
                        {
                            ClientGame.GetInstance.PutBackAt(ClientGame.GetInstance.NumOfDrawCard + 1 - ClientGame.GetInstance.Index);
                        }
                    }
                }
            }
        }
    }
}
