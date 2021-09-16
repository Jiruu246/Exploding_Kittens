using System;
using System.Collections.Generic;
using System.Linq;


namespace ExplodingKittenLib.Cards
{
    public enum CardType
    {
        Exploding,
        Defuse,
        Skip,
        Cattermelon
    }

    public enum Actions
    {
        Skip
    }
    [Serializable]
    public abstract class _Card // factory pattern?
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

        public static _Card GetRandom()
        {
            Array value = CardType.GetValues(typeof(CardType));
            Random random = new Random();
            CardType card = (CardType)value.GetValue(random.Next(2, value.Length));

            return CreateCard(card);
        }
    }
}
