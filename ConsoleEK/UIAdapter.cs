using System;
using System.Collections.Generic;
using System.Text;
using ExplodingKittenLib.Cards;

namespace Client
{
    public abstract class UIAdapter
    {
        private static UIAdapter _instance;

        protected UIAdapter() => Init();

        public static UIAdapter GetInstance
        {
            get
            {
                if (_instance == null)
                    throw new NullReferenceException("No instance found");
                return _instance;
            }
        }
        /// <summary>
        /// get instance from child class ??
        /// </summary>
        protected void Init()
        {
            _instance = this;
        }

        public abstract void CreateWindow(int width, int height);
        public abstract void LoadAssets();
        public abstract void Update();
        public abstract void Draw();
        public abstract void DrawCard(_Card card);
        public abstract void DrawPlayerInLobby(PlayerInfo playerinf);
        public abstract void ButtonDown();
        public abstract bool ExitGame();
    }
}
