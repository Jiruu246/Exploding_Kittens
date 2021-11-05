
using System.Collections.Generic;
using ExplodingKittenLib.Cards;

namespace ExplodingKittenLib
{
    public interface IActivatable
    {
        List<Actions> Action { get;}
        List<Actions> Activate();
    }
}
