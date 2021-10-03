using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace SplashKitGUI.Screens
{
    public class ScreenLobby : ScreenState
    {
        public ScreenLobby()
        {
        }
        public void Update()
        {

        }

        public void Draw()
        {
            SplashKit.DrawBitmap("lobby", 0, 0);
        }

        public void ButtonDown()
        {
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                Screen.GetInstance.ChangeState(new ScreenGame());
            }
        }
    }
}
