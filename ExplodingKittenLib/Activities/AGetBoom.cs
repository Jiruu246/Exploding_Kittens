using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplodingKittenLib.Activities
{
    [Serializable]
    public class AGetBoom : Activity
    {
        public AGetBoom(int player) : base (ActivityType.GetBoom, player)
        {

        }

        public override string Description()
        {
            return "Player " + (Player + 1).ToString() + " just draw an EXPLODING CARD!!!!!!";
        }
    }
}
