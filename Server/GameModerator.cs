using System;
using System.Text;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using ExplodingKittenLib;
using ExplodingKittenLib.Cards;

namespace Server
{
    class GameModerator
    {
        private ServerNetwork _network;
        private PlayerGroup _playerGroup;
        private Deck _drawPile;
        private Deck _discardPile;

        public GameModerator(ServerNetwork network, PlayerGroup players)
        {
            _network = network;
            _playerGroup = players;
            RegisterCards();
            _drawPile = new Deck();
            _discardPile = new Deck();
            CreateDeck(10);
        }

        public void Execute(object data)
        {
            switch (data.GetType().Name)
            {
                case "Int32":
                    Console.WriteLine((int)data);
                    break;
                case "String":
                    Console.WriteLine((string)data);
                    break;
            }
        }

        public void StringProcess(string text) //probably will delete
        {

        }

        public void Listen()
        {
            while (true)
            {
                if (!_playerGroup.MaxPlayer())
                {
                    Socket Client = _network.Listen();

                    if (Client != null)
                    {
                        Player player = _playerGroup.AddPlayer(Client);

                        _network.SendSingle(Client, player.Position); // send the player position

                        Thread recieve = new Thread(() => { Receive(player); });
                        recieve.IsBackground = true;
                        recieve.Start();

                    }
                }

            }
        }

        private void Receive(Player player)
        {
            try
            {
                while (true)
                {
                    Execute(_network.GetData(player.ClientSK));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _network.CloseClient(player.ClientSK, _playerGroup);
                ResendPosition(); // when someone disconnect, resend the position
            }
        }

        private void ResendPosition()
        {
            foreach (Player p in _playerGroup.PlayerList)
            {
                _network.SendSingle(p.ClientSK, p.Position);
            }
        }

        private Deck CreateDeck(int num)
        {
            Deck deck = new Deck();
            deck.Merge(BaseSettup());

            Array value = CardType.GetValues(typeof(CardType));
            Random random = new Random();

            for(int i = 0; i < num - _playerGroup.NumOfPlayer*2 + 1; i++)
            {
                CardType card = (CardType)value.GetValue(random.Next(2, value.Length));
                deck.AddCard(_Card.CreateCard(card));
            }

            deck.Shuffle();

            return deck;
        }

        private Deck BaseSettup()
        {
            Deck deck = new Deck();
            int numplayer = _playerGroup.NumOfPlayer;
            for (int i = 0; i < numplayer; i++)
            {
                deck.AddCard(_Card.CreateCard(CardType.Exploding));
            }
            for (int i = 0; i < numplayer - 1; i++)
            {
                deck.AddCard(_Card.CreateCard(CardType.Defuse));
            }

            return deck;
        }

        private void RegisterCards()
        {
            _Card.RegisterCard(CardType.Exploding, typeof(ExplodingCard));
            _Card.RegisterCard(CardType.Defuse, typeof(DefuseCard));
            _Card.RegisterCard(CardType.Skip, typeof(SkipCard));
            _Card.RegisterCard(CardType.Cattermelon, typeof(CattermelonCard));
        }
    }
}
