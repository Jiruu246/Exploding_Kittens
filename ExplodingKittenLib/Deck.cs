using System;
using System.Collections.Generic;
using System.Text;
using ExplodingKittenLib.Cards;

namespace ExplodingKittenLib
{
    public class Deck
    {
        private List<_Card> _CardList;

        public Deck()
        {
            _CardList = new List<_Card>();
        }

        public List<_Card> CardList
        {
            get
            {
                return _CardList;
            }
        }

        public void AddCard(_Card card)
        {
            _CardList.Add(card);
        }

        public void Merge(Deck deck)
        {
            _CardList.AddRange(deck.CardList);
        }

        public void Shuffle()
        {
            Random random = new Random();

            int replace = random.Next(100, 1000);

            for(int i = 0; i < replace; i++)
            {
                int A = random.Next(0, _CardList.Count);
                int B = random.Next(0, _CardList.Count);

                _Card a = _CardList[A];
                _Card b = _CardList[B];
                _Card c = _CardList[A];

                a = b;
                b = c;

                _CardList[A] = a;
                _CardList[B] = b;

            }

        }
    }
}
