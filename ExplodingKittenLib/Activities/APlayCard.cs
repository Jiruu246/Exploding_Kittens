using System;
using ExplodingKittenLib.Cards;

namespace ExplodingKittenLib.Activities
{
    [Serializable]
    public class APlayCard : Activity
    {
        public _Card Card { get; set; }
        public APlayCard(int player, _Card card) : base(ActivityType.PlayCard, player)
        {
            Card = card;
        }

        public override string Description()
        {
            return "Player " + (Player + 1).ToString() + " just play a " + Card.GetCardname();
        }
    }
}
