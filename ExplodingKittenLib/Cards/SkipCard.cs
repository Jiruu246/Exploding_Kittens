using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplodingKittenLib.Cards
{
    [Serializable]
    public class SkipCard : _Card, IActivatable
    {
        public List<Actions> Action { get; }

        public SkipCard() : base()
        {
            Action = new List<Actions>();
            Action.Add(Actions.Skip);
        }

        public List<Actions> Activate()
        {
            return Action;
        }

    }
}
