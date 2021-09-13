using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ExplodingKittenLib;

namespace Server
{
    class ServerNetwork
    {
        private IPEndPoint _ClientIP;
        private Socket _Server;
        private PlayerGroup _players;

        public ServerNetwork(PlayerGroup players)
        {
            _players = players;
            _ClientIP = new IPEndPoint(IPAddress.Any, 5555);
            _Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            Connect();
        }

        private void Connect()
        {
            try
            {
                _Server.Bind(_ClientIP); 
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("cannot bind IP address");
                return;
            }

            Thread Listen = new Thread(() => {   // create thread for listen purpose
                try
                {
                    while (true) //listen for many client
                    {
                        if (!_players.MaxPlayer())
                        {
                            _Server.Listen(1);
                            Socket client = _Server.Accept(); 
                            Player p = _players.AddPlayer(client); 

                            //SendSingle(client, p.Position); // send somthing to confirm connection

                            Thread recieve = new Thread(Receive); // create thread for client
                            recieve.IsBackground = true;
                            recieve.Start(client);
                        }
                    }
                }
                catch (Exception e)
                {   //some client close => loop error ???
                    Console.WriteLine(e);
                    Console.WriteLine("disconnect bug");
                    _ClientIP = new IPEndPoint(IPAddress.Any, 5555); //any IP from client
                    _Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                }
            });
            Listen.IsBackground = true;
            Listen.Start();
        }

        public void Close()
        {
            _Server.Close();
        }

        public void SendSingle(Socket client, object data)
        {
            client.Send(Serialize(data));
        }

        public void SendMulti(object data)
        {
            foreach(Player player in _players.PlayerList)
            {
                SendSingle(player.ClientSK, data);
                Console.WriteLine("finish sending2");

            }
        }

        public void Receive(Object obj)
        {
            Socket client = obj as Socket;

            try
            {
                while (true) // Always listen for receiving message
                {
                    byte[] data = new byte[1024 * 5000];
                    client.Receive(data);

                    object message = (Object)Deserialize(data); // deserialize data

                    Console.WriteLine(message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                for (int i = 0; i < _players.PlayerList.Count; i++)
                {
                    Player player = _players.PlayerList[i];
                    if (player.ClientSK == client)
                        _players.RemovePlayer(player);
                }
                client.Close();
            }
        }

        private byte[] Serialize(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, obj);

            return stream.ToArray();
        }

        private object Deserialize(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();

            return formatter.Deserialize(stream);
        }
    }
}
