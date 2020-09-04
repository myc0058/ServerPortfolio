using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using static Schema.Protobuf.Api;

namespace Schema.Protobuf
{
    public class Client : Tcp, INotifier
    {
        void INotifier.Response<T>(T msg)
        {
            var serializer = new Serializer<T>();
            serializer.Message = msg;
            base.Write(serializer);
        }

        void INotifier.Notify<T>(T msg)
        {
            var serializer = new Serializer<T>();
            serializer.Message = msg;
            base.Write(serializer);
        }

        public static void SerializeProtobufTo<T>(Stream output, long sequence, T msg)
        {
            var proto = msg as Google.Protobuf.IMessage;

            output.Write(BitConverter.GetBytes(proto.CalculateSize() + sizeof(int) + sizeof(int) + sizeof(long)), 0, 4);
            output.Write(BitConverter.GetBytes(Api.Id<T>.Value), 0, 4);
            output.Write(BitConverter.GetBytes(sequence), 0, 8);
            proto.Serialize(output, true);

        }

        public class Serializer<T> : ISerializable
        {
            public void Serialize(Stream output)
            {
                SerializeProtobufTo<T>(output, Sequence, Message);
            }
            public int Length
            {
                get
                {
                    if (length == 0)
                    {
                        var proto = Message as Google.Protobuf.IMessage;
                        length = proto.CalculateSize() + sizeof(int) + sizeof(int) + sizeof(long);
                    }
                    return length;
                }
            }
            protected int length { get; set; }
            public T Message;
            public long Sequence { get; set; }
        }

        public Client()
        {
            OnRead = onRead;
            OnDisconnect = onDisconnect;

            clientList.Add(this);

            UseCompress = true;
        }

        virtual protected void onDisconnect()
        {
            lock (clientList)
            {
                clientList.Remove(this);
            }
        }

        protected override void Defragment(MemoryStream transferred)
        {
            var buffer = transferred.GetBuffer();

            int blockSize = 0;
            int readBytes = 0;

            while (transferred.Length - readBytes > sizeof(int))
            {
                blockSize = BitConverter.ToInt32(buffer, readBytes);
                if (blockSize < 1 || blockSize > RecvBufferSize)
                {
                    transferred.Seek(transferred.Length, SeekOrigin.Begin);
                    Disconnect();
                    return;
                }

                if (blockSize + readBytes > transferred.Length) { break; }

                Stream stream = new MemoryStream(buffer, readBytes + 8, blockSize - 8, true, true);
                readBytes += blockSize;


                CryptoStream csEncrypt = null;
                if (aesAlg != null)
                {
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    csEncrypt = new CryptoStream(stream, decryptor, CryptoStreamMode.Read);
                    stream = csEncrypt;
                }

                GZipStream compressionStream = null;
                if (UseCompress)
                {
                    compressionStream = new GZipStream(stream, CompressionMode.Decompress, true);
                    stream = compressionStream;
                }

                MemoryStream result = new MemoryStream();

                stream.CopyTo(result);
                result.Seek(0, SeekOrigin.Begin);

                try
                {
                    OnRead?.Invoke(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("OnRead Exception " + e);
                }


            }

            transferred.Seek(readBytes, SeekOrigin.Begin);

        }

        private int onRead(MemoryStream transferred)
        {
            int offset = 0;
            byte[] buffer = transferred.GetBuffer();
            while ((transferred.Length - offset) > sizeof(int))
            {

                int size = BitConverter.ToInt32(buffer, offset);

                if (size < 1 || size > RecvBufferSize)
                {
                    transferred.Seek(transferred.Length, SeekOrigin.Begin);
                    Disconnect();
                    return 0;
                }

                if (size > transferred.Length - offset)
                {
                    break;
                }


                int code = BitConverter.ToInt32(buffer, offset + 4);
                long seq = BitConverter.ToInt64(buffer, offset + 8);

                using (MemoryStream msg = new MemoryStream(buffer, offset + 16, size - 16, true, true))
                {
                    var callback = Schema.Protobuf.Api.Bind(this, code, msg);

                    msgCallbacks.Enqueue(callback);
                }

                offset += size;
            }

            transferred.Seek(offset, SeekOrigin.Begin);
            return 0;

        }

        public static void DisconnectAll()
        {
            lock (clientList)
            {
                foreach (var client in clientList.ToList())
                {
                    client.Disconnect();
                }
            }
        }

        protected static List<Client> clientList = new List<Client>();

        public ConcurrentQueue<Callback> msgCallbacks = new ConcurrentQueue<Callback>();
    }
}
