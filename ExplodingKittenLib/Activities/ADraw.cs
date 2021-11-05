using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplodingKittenLib.Activities
{
    [Serializable]
    public class ADraw : Activity
    {
        public int NumOfDrawCard { get; set; }
        public ADraw(int player, int num) : base(ActivityType.Draw, player)
        {
            NumOfDrawCard = num;
        }

        public override string Description()
        {
            return "Player " + (Player + 1).ToString() + " just take a card from the Draw Pile";
        }

    }
}
