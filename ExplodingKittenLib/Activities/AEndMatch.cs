using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplodingKittenLib.Activities
{

    [Serializable]
    public class AEndMatch : Activity
    {
        public AEndMatch(int player) : base(ActivityType.EndMatch, player)
        {

        }

        public override string Description()
        {
            return "Match End!!! Player " + (Player + 1).ToString() + " is the winner";
        }
    }

}
