using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplodingKittenLib.Cards
{
    [Serializable]
    public class ReverseCard : _Card, IActivatable
    {
        public List<Actions> Action { get; }

        public ReverseCard() : base()
        {
            Action = new List<Actions>();
            Action.Add(Actions.Reverse);
            Action.Add(Actions.Skip);
        }

        public List<Actions> Activate()
        {
            return Action;
        }
    }
}
