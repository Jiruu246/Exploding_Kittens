using System;
using ExplodingKittenLib.Activities;
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

        protected void Init()
        {
            _instance = this;
        }

        public abstract void CreateWindow(int width, int height);
        public abstract void LoadAssets();
        public abstract void Update();
        public abstract void Draw();
        public abstract void DrawCard(_Card card);
        public abstract void DrawActivity(Activity activity);
        public abstract void DrawGameDeck(int NumOfDrawCard, _Card card);
        public abstract void DrawPlayerInLobby(PlayerInfo playerinf);
        public abstract void DrawPlayerInGame(PlayerInfo playerInfo);
        public abstract void DrawBombWarning();
        public abstract void DrawChoosingIndex(int index);
        public abstract void ButtonDown();
        public abstract bool ExitGame();
    }
}
