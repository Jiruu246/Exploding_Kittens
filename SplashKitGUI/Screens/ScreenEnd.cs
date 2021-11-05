
using Client;
using SplashKitSDK;

namespace SplashKitGUI.Screens
{
    public class ScreenEnd : ScreenState
    {
        public ScreenEnd()
        {

        }

        public void Update()
        {

        }
        public void Draw()
        {
            SplashKit.DrawBitmap("endmatchbg", 0, 0);
            SplashKit.DrawBitmap("avatar" + (ClientGame.GetInstance.Winner + 1), 780, 300);
            SplashKit.DrawText("Player " + (ClientGame.GetInstance.Winner + 1), Color.White, "main_font", 40, 770, 450);
        }
        public void ButtonDown()
        {

        }
    }
}
