using System;
using System.Collections.Generic;


namespace ExplodingKittenLib.Cards
{
    [Serializable]
    public class DrawFromBottomCard : _Card, IActivatable
    {
        public List<Actions> Action { get; }
        public DrawFromBottomCard() : base()
        {
            Action = new List<Actions>();
            Action.Add(Actions.DrawFromBottom);
        }

        public List<Actions> Activate()
        {
            return Action;
        }
    }
}
