using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplodingKittenLib
{
    [Serializable]
    public enum Requests
    {
        Start,
        YourTurn,
        Draw,
        Explode,
        Defuse,
        YouWin
    }
}
