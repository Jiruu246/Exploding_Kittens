using System;
using System.Collections.Generic;

namespace ExplodingKittenLib.Cards
{
    [Serializable]
    public class DefuseCard : _Card, IActivatable
    {
        public List<Actions> Action { get;}
        public DefuseCard() : base()
        {
            Action = new List<Actions>();
            Action.Add(Actions.Defuse);
        }

        public List<Actions> Activate()
        {
            return Action;
        }
    }
}
