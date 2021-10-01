using System;
using System.Collections.Generic;
using System.Text;

namespace SplashKitGUI.Screens
{
    public class Screen
    {
        private static Screen _screen;
        private static ScreenState _state;

        private Screen(ScreenState state)
        {
            ChangeState(state);
        }

        public static Screen GetInstance
        {
            get
            {
                if(_screen == null)
                {
                    _screen = new Screen(new ScreenMenu());
                }
                if (_state == null)
                {
                    _state = new ScreenMenu();
                }
                return _screen;
            }
        }

        public void ChangeState(ScreenState state)
        {
            _state = state;
        }

        public void Update()
        {
            _state.Update();
        }

        public void Draw()
        {
            _state.Draw();
        }

        public void ButtonDown()
        {
            _state.ButtonDown();
        }
    }
}
