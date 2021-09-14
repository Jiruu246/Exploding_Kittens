﻿using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ExplodingKittenLib;

namespace ExplodingKittenLib
{
    public abstract class Network
    {
        public Network()
        {

        }

        protected abstract void GenerateAddress();

        public abstract bool Connect(); // test protect

        public abstract void Close();

        protected virtual byte[] Serialize(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, obj);

            return stream.ToArray();
        }

        protected virtual object Deserialize(byte[] data)
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
        }
    }
}
