using System;
using System.Collections.Generic;
using System.Linq;


namespace ExplodingKittenLib.Cards
{
    public enum CardType
    {
        ExplodingCard,
        DefuseCard,
        SkipCard,
        CattermelonCard,
        NopeCard
    }

    public enum Actions
    {
        Defuse,
        Skip,
        Nope
    }

    [Serializable]
    public abstract class _Card 
    {
        private static Dictionary<CardType, Type> _CardClassRegistry = new Dictionary<CardType, Type>();
        protected CardType _cardName;
        protected Guid _id;
        protected bool _faceup;
        public double X { get; set; }
        public double Y { get; set; }
        public bool Selected { get; set; }

        public _Card()
        {
            _cardName = GetKey(GetType());
            _id = Guid.NewGuid();
            _faceup = false;
            Selected = false;
        }

        public CardType? GetCardname()
        {
            if (_faceup)
            {
                return _cardName;
            }
            else
            {
                return null;
            }
        }

        public void Flip()
        {
            _faceup = !_faceup;
        }

        public Guid ID
        {
            get
            {
                return _id;
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

        private CardType GetKey(Type t)
        {
            return _CardClassRegistry.FirstOrDefault(entry =>
            EqualityComparer<Type>.Default.Equals(entry.Value, t)).Key;
        }

        public static _Card GetRandom()
        {
            Array value = Enum.GetValues(typeof(CardType));
            Random random = new Random();
            CardType card = (CardType)value.GetValue(random.Next(2, value.Length));

            return CreateCard(card);
        }
        
        /// <summary>
        /// For GUI
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public bool IsAt(double x, double y)
        {
            return ((x >= X && x <= X + 260) && (y >= Y && y <= Y + 280));
        }
    }
}
