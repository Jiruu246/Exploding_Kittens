using System;
using System.Collections.Generic;
using System.Text;

namespace SplashKitGUI.Screens
{
    public interface ScreenState
    {
        void Update();
        void Draw();
        void ButtonDown();
    }
}
