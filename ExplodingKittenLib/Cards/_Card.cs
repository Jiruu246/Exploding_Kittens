using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplodingKittenLib.Cards
{
    public enum CardType
    {
        Exploding,
        Defuse,
        Skip,
        Cattermelon
    }
    public abstract class _Card
    {
        private static Dictionary<CardType, Type> _CardClassRegistry = new Dictionary<CardType, Type>();
        protected CardType _cardName;

        public _Card()
        {
            _cardName = GetKey(GetType());
        }

        public CardType Cardname
        {
            get
            {
                return _cardName;
            }
        }

        public static void RegisterCard(CardType name, Type t)
        {
            _CardClassRegistry[name] = t;
        }

        public static _Card CreateCard(CardType name)
        {
            return (_Card)Activator.CreateInstance(_CardClassRegistry[name]);
        }

        public static CardType GetKey(Type t)
        {
            return _CardClassRegistry.FirstOrDefault(entry =>
            EqualityComparer<Type>.Default.Equals(entry.Value, t)).Key;
        }
    }
}
