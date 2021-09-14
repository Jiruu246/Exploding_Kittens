using System;
using System.Net;
using System.Net.Sockets;
using ExplodingKittenLib;

namespace Server
{
    class ServerNetwork : Network
    {
        private IPEndPoint _ClientIP;
        private Socket _Server;

        protected ServerNetwork() : base()
        {
            GenerateAddress();
            Connect();
        }

        public static ServerNetwork GetInstance()
        {
            if (_network == null)
            {
                _network = new ServerNetwork();
            }

            return _network as ServerNetwork;
        }

        protected override void GenerateAddress()
        {
            _ClientIP = new IPEndPoint(IPAddress.Any, 5555);
            _Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        }

        public override bool Connect()
        {
            try
            {
                _Server.Bind(_ClientIP);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("cannot bind IP address");
                return false;
            }
        }
        //Thread Listen = new Thread(() => {   // create thread for listen purpose
        public Socket Listen()
        {
            try
            {
                //while (true) //listen for many client
                //{
                    //if (!_players.MaxPlayer())
                    //{
                _Server.Listen(1);
                Socket client = _Server.Accept();
                return client;

                //
                //Player p = _players.AddPlayer(client); // can put this out side !!!!!!!!!
                //SendSingle(client, p.Position); // send somthing to confirm connection
                //return p;
                        //Thread recieve = new Thread(Receive); // create thread for client
                        //recieve.IsBackground = true;
                        //recieve.Start(client);
                    //}
                //}
            }
            catch (Exception e)
            {   //some client close => loop error ???
                Console.WriteLine(e);
                Console.WriteLine("disconnect bug");
                GenerateAddress();
                return null;
            }
        }
            //});
            //Listen.IsBackground = true;
            //Listen.Start();
        //}

        public override void Close()
        {
            _Server.Close();
        }

        public void SendSingle(Socket client, object data)
        {
            client.Send(Serialize(data));
        }

        public void SendMulti(object data, PlayerGroup players)
        {
            foreach(Player player in players.PlayerList)
            {
                SendSingle(player.ClientSK, data);
                Console.WriteLine("finish sending2");
            }
        }

        /*public Receive(Object obj)
        {
            Socket client = obj as Socket;

            try
            {
                while (true) // Always listen for receiving message
                {
                    GetData(client);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                CloseClient(client);
            }
        }*/

        /*private byte[] Serialize(object obj)
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

        public object GetData(Socket client)
        {
            byte[] data = new byte[2048];
            client.Receive(data);
            object message = (object)Deserialize(data);
            return message;
        }*/

        public void CloseClient(Socket client, PlayerGroup players)
        {
            for (int i = 0; i < players.PlayerList.Count; i++) // singleton?? passing the list
            {
                Player player = players.PlayerList[i];
                if (player.ClientSK == client)
                    players.RemovePlayer(player);
            }
            client.Close();
        }
    }
}
