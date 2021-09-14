using System;
using System.Net;
using System.Net.Sockets;
using ExplodingKittenLib;

namespace Client
{
    public class ClientNetwork : Network
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

        protected override void GenerateAddress()
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

            //Thread listen = new Thread(Receive);
            //listen.IsBackground = true;
            //listen.Start();
        }

        public override void Close()
        {
            _client.Close();
        }

        public void Send(object data)
        {

            try
            {
                //if (data as string != null) // test for sending string
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

        //public void Receive()
        //{
        //   try
        //    {
        //        while (true)
        //        {
        //            GetData();
        //        }
        //    }
        //    catch
        //    {
        //        Close();
        //    }
        //}

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
            Console.WriteLine(message.GetType().Name);
            return message;
        }*/
    }
}
