using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client;
using SplashKitSDK;

namespace SplashKitGUI.Screens
{
    public class ScreenLobby : ScreenState
    {
        //Rectangle rect = SplashKit.RectangleFrom(650, 60, 350, 80);
        //string name = "unknow";
        Font font = new Font("input", "Minecraft.ttf");
        
        public ScreenLobby()
        {

        }
        public void Update()
        {
            if (ClientGame.GetInstance.GameStart)
            {
                Screen.GetInstance.ChangeState(new ScreenGame());
            }
            /*
            if (!SplashKit.ReadingText())
            {
                if (SplashKit.TextEntryCancelled())
                {
                    name = "unknow";
                }
                else
                {
                    name = SplashKit.TextInput();
                }
            }
            */
 
        }

        public void Draw()
        {
            SplashKit.DrawBitmap("lobby", 0, 0);
            //SplashKit.DrawBitmap("namePlaceholder", 600, 30);

            /*draw the name holder
            if (SplashKit.ReadingText())
            {
                SplashKit.DrawCollectedText(Color.White, font, 40, SplashKit.OptionDefaults());
            }
            else
            {
                SplashKit.DrawText(name, Color.White, font, 40, 650, 60);
            }*/

            //draw the room
            if (!ClientGame.GetInstance.GameFound)
            {
                SplashKit.DrawBitmap("findmatch", 650, 400);
            }
            else
            {
                SplashKit.DrawBitmap("playerholder", 320, 20);
                ClientGame.GetInstance.DrawPlayerInLobby();
                if(ClientGame.GetInstance.PlayerPos == 0)
                {
                    SplashKit.DrawBitmap("startMatch", 1250, 750);
                }
            }
        }

        public void ButtonDown()
        {
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                if (!ClientGame.GetInstance.GameFound)
                {
                    /*if (Screen.GetInstance.MouseInZone(600, 30, 400, 100))
                    {
                        SplashKit.StartReadingText(rect, name);
                    }*/
                    if(Screen.GetInstance.MouseInZone(650,400, 300, 134))
                    {
                        ClientGame.GetInstance.FindMatch();
                    }
                }
                else
                {
                    if (Screen.GetInstance.MouseInZone(1250, 750, 200, 86))
                    {
                        ClientGame.GetInstance.StartMatch();
                    }
                }
            }
            /*
            else if (SplashKit.KeyTyped(KeyCode.ReturnKey))
            {
                ClientGame.GetInstance.SetName(SplashKit.TextInput());
            }
            */

        }
    }
}
