using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExplodingKittenLib.Cards;

namespace ExplodingKittenLib
{
    public interface IActivatable
    {
        List<Actions> Activate();
    }
}
