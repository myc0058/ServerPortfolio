using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Engine.Framework;
using Google.Protobuf;
using static Engine.Framework.Api;

namespace Application.Lobby.Protocol
{

    public class Client : Engine.Network.Protocol.Tcp, Engine.Framework.INotifier
    {
        public virtual void Response<T>(T msg)
        {
            var serializer = new Serializer<T>();
            serializer.Message = msg;
            base.Write(serializer);
        }
        public virtual void Notify<T>(T msg)
        {
            var serializer = new Serializer<T>();
            serializer.Message = msg;
            base.Write(serializer);
        }

        public long UID { get; set; }

        protected override void flush()
        {
            if (pendings.Count == 0) { return; }
            MemoryStream output = new MemoryStream();
            output.Write(BitConverter.GetBytes((int)0), 0, 4);
            output.Write(BitConverter.GetBytes((int)0), 0, 4);
            output.Seek(8, SeekOrigin.Begin);
            CryptoStream csEncrypt = null;
            Stream stream = output;


            if (aesAlg != null)
            {
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                csEncrypt = new CryptoStream(stream, encryptor, CryptoStreamMode.Write);
                stream = csEncrypt;
            }

            GZipStream compressionStream = null;
            if (UseCompress)
            {
                compressionStream = new GZipStream(stream, CompressionMode.Compress, true);
                stream = compressionStream;
            }


            int length = 0;
            while (pendings.Count > 0 && length < RecvBufferSize)
            {
                var msg = pendings.Dequeue();
                switch (msg)
                {
                    case Engine.Framework.ISerializable serializable:
                        length += serializable.Length;
                        serializable.Serialize(stream);
                        /*
                        MemoryStream ms2 = new MemoryStream();
                        serializable.Serialize(ms2);
                        string base64 = Convert.ToBase64String(ms2.ToArray());
                        Console.WriteLine(base64);
                        */
                        break;
                    case MemoryStream ms:
                        try
                        {
                            length += (int)ms.Length;
                            ms.CopyTo(stream);
                        }
                        catch (Exception e)
                        {
                            Disconnect();
                        }
                        break;
                    case byte[] array:
                        length += array.Length;
                        stream.Write(array, 0, array.Length);
                        break;
                    default:
                        break;
                }
            }


            if (compressionStream != null)
            {
                compressionStream.Flush();
                compressionStream.Dispose();
            }

            if (csEncrypt != null)
            {
                csEncrypt.FlushFinalBlock();
            }

            if (output.Length == 2)
            {
                return;
            }

            output.Seek(0, SeekOrigin.Begin);
            output.Write(BitConverter.GetBytes((int)output.Length), 0, 4);
            output.Write(BitConverter.GetBytes((int)length), 0, 4);
            output.Seek(0, SeekOrigin.Begin);

            sendBuffer = output.ToArray();

            if (csEncrypt != null)
            {
                csEncrypt.Dispose();
            }

            output.Dispose();

            if (sendBuffer == null || sendBuffer.Length == 0)
            {
                sendBuffer = null;
                return;
            }

            socket.BeginSend(sendBuffer, 0, (int)sendBuffer.Length, SocketFlags.None, SendComplete, null);
        }

        public static void DisconnectOne()
        {
            lock (tcps)
            {
                foreach (var e in tcps)
                {
                    e.Key.Disconnect();
                    return;
                }
            }
        }

        public static void HeartBeat()
        {
            lock (tcps)
            {
                foreach (var e in tcps)
                {
                    if (e.Value < DateTime.UtcNow)
                    {
                        e.Key.Disconnect();
                    }
                }
            }
        }

        public static void SerializeProtobufTo<T>(Stream output, long sequence, T msg)
        {
            var proto = msg as Google.Protobuf.IMessage;

            output.Write(BitConverter.GetBytes(proto.CalculateSize() + sizeof(int) + sizeof(int) + sizeof(long)), 0, 4);
            output.Write(BitConverter.GetBytes(Engine.Framework.Id<T>.Value), 0, 4);
            output.Write(BitConverter.GetBytes(sequence), 0, 8);
            proto.Serialize(output, true);

        }

        public class Serializer<T> : Engine.Framework.ISerializable
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
            OnAccept = onAccept;
            OnDisconnect = onDisconnect;
        }

        private void onDisconnect()
        {
            lock (tcps)
            {
                tcps.TryRemove(this, out DateTime dateTime);
            }

            if (user != null)
            {
                var postDisconnect = user;
                user.PostMessage(() => {
                    postDisconnect.OnDisconnect();
                });
                user = null;
            }
        }

        private static ConcurrentDictionary<Client, DateTime> tcps = new ConcurrentDictionary<Client, DateTime>();

        private void onAccept(bool ret)
        {
            if (ret == false)
            {
                return;
            }
            lock (tcps)
            {
                tcps.TryAdd(this, DateTime.UtcNow.AddSeconds(Api.HeartBeatInterval));
            }

            UseCompress = true;
            var aes = System.Security.Cryptography.Aes.Create();
            var msg = new Schema.Protobuf.Message.Authentication.Encript();
            msg.Key = Convert.ToBase64String(aes.Key);
            msg.IV = Convert.ToBase64String(aes.IV);
            aes.Mode = CipherMode.ECB;
          //  aes = null;
            MemoryStream stream = new MemoryStream();
            SerializeProtobufTo(stream, 0, msg);
            base.Write(stream);

            aesAlg = aes;

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

                if (user == null)
                {
                    user = Engine.Framework.New<Entities.User>.Instantiate;
                    user.Client = this;
                    user.MyMatchingInfo.Delegator = null;
                }

                using (MemoryStream msg = new MemoryStream(buffer, offset + 16, size - 16, true, true))
                {
                    var callback = Schema.Protobuf.Api.Bind(user,
                                                        this,
                                                        code,
                                                        msg);

                    user.PostMessage(callback);
                }

                offset += size;
            }

            transferred.Seek(offset, SeekOrigin.Begin);
            return 0;

        }

        internal static void BroadCast<T>(T msg) where T : IMessage
        {
            lock (tcps)
            {
                foreach (var e in tcps.Keys)
                {
                    e.Notify(msg);
                }
            }
        }

        internal static void Ping(Client notifier)
        {
            lock (tcps)
            {
                if (tcps.ContainsKey(notifier) == false) { return; }
                tcps.AddOrUpdate(notifier, DateTime.UtcNow.AddSeconds(Api.HeartBeatInterval), (client, datetime) => {
                    return DateTime.UtcNow.AddSeconds(Api.HeartBeatInterval);
                });
            }
        }

        public Entities.User user = null;
    }

}
