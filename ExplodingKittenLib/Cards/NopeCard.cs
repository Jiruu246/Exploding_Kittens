using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplodingKittenLib.Cards
{
    [Serializable]
    public class NopeCard : _Card, IActivatable
    {
        public List<Actions> Action { get; } 
        public NopeCard(): base()
        {
            Action = new List<Actions>();
            Action.Add(Actions.Nope);
        }

        public List<Actions> Activate()
        {
            return Action;
        }
    }
}
