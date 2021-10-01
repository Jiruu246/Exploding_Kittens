using System;
using System.Net;
using System.Net.Sockets;
using ExplodingKittenLib;

namespace Client
{
    public class ClientNetwork : Network //singleton
    {
        private IPEndPoint _ServerIP;
        private Socket _client;

        protected ClientNetwork() : base()
        {
            GenerateAddress();
        }

        public static ClientNetwork GetInstance()
        {
            if (_network == null)
            {
                _network = new ClientNetwork();
            }

            return _network as ClientNetwork;
        }

        public override void GenerateAddress()
        {
            _ServerIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        }

        public override bool Connect()
        {

            try
            {
                _client.Connect(_ServerIP);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("cannot connect");
                return false;
            }

        }

        public override void Close()
        {
            _client.Close();
        }

        public void Send(object data)
        {

            try
            {
                if(data != null)
                    _client.Send(Serialize(data));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Cant send data");
            }
        }

        public Socket Socket
        {
            get
            {
                return _client;
            }
        }

    }
}
