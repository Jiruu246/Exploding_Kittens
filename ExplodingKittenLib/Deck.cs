using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExplodingKittenLib.Cards;

namespace ExplodingKittenLib
{
    [Serializable]
    public class Deck
    {
        private List<_Card> _CardList;

        public Deck()
        {
            _CardList = new List<_Card>();
        }

        public Deck(int numofp, int numofc) : this()
        {
            BaseSettup(numofp);

            for (int i = 0; i < numofc - numofp * 2 + 1; i++)
            {
                _CardList.Add(_Card.GetRandom());
            }

            Shuffle();
        }

        private void BaseSettup(int numofp)
        {
            for (int i = 0; i < numofp; i++)
            {
                _CardList.Add(_Card.CreateCard(CardType.Exploding));
            }
            for (int i = 0; i < numofp - 1; i++)
            {
                _CardList.Add(_Card.CreateCard(CardType.Defuse));
            }
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

        public void RemoveCard(_Card card)
        {
            _CardList.Remove(card);
        }
        public void AddCardAt(int i, _Card card)
        {
            _CardList.Insert(i, card);
        }
        public void RemoveCardAt(int i)
        {
            _CardList.RemoveAt(i);
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

        public bool DeckNotEmpty
        {
            get
            {
                return _CardList.Any();
            }
        }

        public _Card Pop()
        {
            if (DeckNotEmpty)
            {
                _Card card = _CardList[_CardList.Count - 1];
                _CardList.RemoveAt(_CardList.Count - 1);
                return card;
            }
            else
                return null;

        }

        public _Card PopBottom()
        {
            if (DeckNotEmpty)
            {
                _Card card = _CardList[0];
                _CardList.RemoveAt(0);
                return card;
            }
            else
                return null;
        }
    }
}
