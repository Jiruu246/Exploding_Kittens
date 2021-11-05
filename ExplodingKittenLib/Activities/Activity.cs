using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplodingKittenLib.Activities
{
    public enum ActivityType
    {
        Draw,
        PlayCard,
        GetBoom,
        EndMatch
    }
    [Serializable]
    public abstract class Activity
    {
        //public bool Finish { get; set; }
        public ActivityType Type { get; set; }
        public int Player { get; set; }
        public Activity(ActivityType type, int player)
        {
            //Finish = false;
            Type = type;
            Player = player;
        }

        public abstract string Description();
    }
}
