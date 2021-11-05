using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplodingKittenLib.Cards
{
    [Serializable]
    public class ShuffleCard : _Card, IActivatable
    {
         public List<Actions> Action { get; }

        public ShuffleCard() : base()
        {
            Action = new List<Actions>();
            Action.Add(Actions.Shuffle);
        }

        public List<Actions> Activate()
        {
            return Action;
        }
    }
}
