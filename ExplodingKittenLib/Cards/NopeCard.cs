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
        public NopeCard(): base() { }

        public List<Actions> Activate()
        {
            List<Actions> actions = new List<Actions>();
            actions.Add(Actions.Nope);
            return actions;
        }
    }
}
