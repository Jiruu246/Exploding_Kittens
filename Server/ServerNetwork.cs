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

        public override void GenerateAddress()
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
        public Socket Listen()
        {
            try
            {
                _Server.Listen(1);
                Socket client = _Server.Accept();
                return client;
            }
            catch (Exception e)
            {   
                Console.WriteLine(e);
                return null;
            }
        }

        public override void Close()
        {
            _Server.Close();
        }

        public void SendSingle(Socket client, object data)
        {
            if(data != null)
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

        public void CloseClient(Socket client, PlayerGroup players)
        {
            for (int i = 0; i < players.PlayerList.Count; i++) // singleton?? passing the list
            {
                Player player = players.GetPlayerAt(i);
                if (player.ClientSK == client)
                {
                    players.RemovePlayerAt(i);
                    Console.WriteLine("remove success");
                }
            }
            client.Close();
        }
    }
}
