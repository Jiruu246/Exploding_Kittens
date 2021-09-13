using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ExplodingKittenLib;

namespace Client
{
    public class ClientNetwork
    {
        private IPEndPoint _ServerIP;
        private Player _player;

        public ClientNetwork(Player player)
        {
            _player = player;
            _ServerIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);
            _player.ClientSK = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        }

        public bool Connect()
        {

            try
            {
                _player.ClientSK.Connect(_ServerIP);
                //GetData();
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

        public void Close()
        {
            _player.ClientSK.Close();
        }

        public void Send(object data)
        {

            try
            {
                if (data as string != null) // test for sending string
                    _player.ClientSK.Send(Serialize(data));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Cant send data");
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

        public object GetData()
        {
            byte[] data = new byte[1024 * 5000];
            _player.ClientSK.Receive(data);
            object message = (object)Deserialize(data);
            Console.WriteLine(message.GetType().Name);
            return message;
        }
    }
}
