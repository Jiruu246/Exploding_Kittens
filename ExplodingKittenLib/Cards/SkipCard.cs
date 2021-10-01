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
        public List<Actions> Activate()
        {
            List<Actions> actions = new List<Actions>();
            actions.Add(Actions.Skip);
            return actions;
        }
    }
}
