using Engine.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Engine.Framework.Api;

namespace Engine.Network.Protocol
{
    public class Terminal : Engine.Framework.Scheduler, Engine.Framework.INotifier
    {
        public class Message
        {
            public enum EType
            {
                Ping = 0,
                Command,
                Notify,
                Error,
                Complete,
            };
            public EType Type { get; set; }
            public string Command { get; set; }
            public bool NewLine { get; set; } = true;
            public long Address { get; set; }
            public long UID { get; set; }
                 
        }

        public class DelegateNotifier : Engine.Framework.INotifier
        {
            public DelegateNotifier(long uid)
            {
                FromUID = uid;
                ToUID = Engine.Framework.Api.UniqueKey;
                HeartBeat = DateTime.UtcNow.AddSeconds(15);
                delegateNotifiers.TryAdd(ToUID, this);
                Protocol.Terminal.ClearDelegateNotifier();
            }
            public void Response<T>(T msg)
            {
                HeartBeat = DateTime.UtcNow.AddSeconds(15);
                (msg as Message).UID = FromUID;
                From?.Response(msg);
                Protocol.Terminal.ClearDelegateNotifier();

            }
            public void Notify<T>(T msg)
            {
                HeartBeat = DateTime.UtcNow.AddSeconds(15);
                (msg as Message).UID = ToUID;
                To?.Notify(msg);
                Protocol.Terminal.ClearDelegateNotifier();
            }
            public long FromUID { get; set; }
            public long ToUID { get; set; }
            public Engine.Framework.INotifier To { get; set; }
            public Engine.Framework.INotifier From { get; set; }
            public DateTime HeartBeat { get; set; }
        }
        public class ConsoleNotifier : Engine.Framework.INotifier
        {
            public static ConsoleNotifier Instance
            {
                get
                {
                    return Singleton<ConsoleNotifier>.Instance;
                }
            }
            public void Response<T>(T msg)
            {
                var message = msg as Protocol.Terminal.Message;
                if (message.NewLine == true)
                {
                    Engine.Framework.Api.Logger.Info(message.Command);
                }
                else
                {
                    Console.Write(message.Command);
                }

            }
            public void Notify<T>(T msg)
            {
                var message = msg as Protocol.Terminal.Message;
                if (message.NewLine == true)
                {
                    Engine.Framework.Api.Logger.Info(message.Command);
                }
                else
                {
                    Console.Write(message.Command);
                }
            }
        }
        public class DefaultNotifier : Engine.Framework.INotifier
        {
            public void Response<T>(T msg)
            {
                var message = msg as Protocol.Terminal.Message;
                message.UID = UID;
                if (message.Type == Message.EType.Command)
                {
                    Engine.Framework.Api.Logger.Info($"DefaultNotifier Response Command. Not Allow Command in Response. '{message.Command}'");
                    message.Type = Message.EType.Notify;
                }
                From.Response(msg);
            }
            public void Notify<T>(T msg)
            {
                var message = msg as Protocol.Terminal.Message;
                message.UID = UID;
                From.Notify(msg);
            }
            public long UID { get; set; }
            public Engine.Framework.INotifier From { get; set; }
        }

        public class StringNotifier : Engine.Framework.INotifier
        {
            public string Buffer { get; set; }

            public void Notify<T>(T msg)
            {
                var message = msg as Protocol.Terminal.Message;
                Buffer += message.Command;
            }

            public void Response<T>(T msg)
            {
                var message = msg as Protocol.Terminal.Message;
                Buffer += message.Command;
            }
        }

        public static void ClearDelegateNotifier()
        {
            foreach (var e in delegateNotifiers)
            {
                if (e.Value.HeartBeat < DateTime.UtcNow)
                {
       //             delegateNotifiers.TryRemove(e.Key, out DelegateNotifier output);
                }
            }
        }
        internal static ConcurrentDictionary<long, DelegateNotifier> delegateNotifiers = new ConcurrentDictionary<long, DelegateNotifier>();

        //public static async Task<bool> DelegateCommand(Engine.Framework.INotifier, Message msg)
        //{

        //    return true;
        //}

        Network.Protocol.Tcp protocol = new Network.Protocol.Tcp();
        public void Response<T>(T msg)
        {
            Message data = msg as Message;
            if (data == null) { return; }
            var stream = new MemoryStream(1024);

            using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                byte[] bytes = data.Command.GetBytes();
                bw.Write((ushort)(sizeof(ushort) + sizeof(int) + sizeof(bool) + sizeof(long) + sizeof(long) + bytes.Length));
                bw.Write((int)data.Type);
                bw.Write(data.NewLine);
                bw.Write(data.UID);
                bw.Write(data.Address);
                bw.Write(bytes, 0, bytes.Length);
            }

            protocol.Write(stream);
        }
        public void Notify<T>(T msg)
        {
            Message data = msg as Message;
            if (data == null) { return; }
            var stream = new MemoryStream(1024);
            using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                byte[] bytes = data.Command.GetBytes();
                bw.Write((ushort)(sizeof(ushort) + sizeof(int) + sizeof(bool) + sizeof(long) + sizeof(long) + bytes.Length));
                bw.Write((int)data.Type);
                bw.Write(data.NewLine);
                bw.Write(data.UID);
                bw.Write(data.Address);
                bw.Write(bytes, 0, bytes.Length);
            }
            protocol.Write(stream);

        }



        public class Layer : Engine.Framework.Layer { }

        public void Disconnect() => protocol?.Disconnect();

        protected override void OnSchedule(long deltaTicks)
        {
            if (protocol.IsClosed()) { return; }

            // ping
            if (string.IsNullOrEmpty(IP) == true) { return; }
            var stream = new MemoryStream(1024);


            using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                byte[] bytes = "ping".GetBytes();
                bw.Write((ushort)(sizeof(ushort) + sizeof(int) + sizeof(bool) + bytes.Length));
                bw.Write((int)0);
                bw.Write(true);
                bw.Write(bytes, 0, bytes.Length);
            }

            protocol.Write(stream);

        }

        public EndPoint RemoteEndPoint => protocol.RemoteEndPoint;

        public static Terminal Get(string name)
        {
            return Singleton<Engine.Framework.Container<string, Engine.Network.Protocol.Terminal>>.Instance.Get(name);
        }

        private static ConcurrentDictionary<long, Terminal> clients = new ConcurrentDictionary<long, Terminal>();

        public static Terminal Get(long uid)
        {
            clients.TryGetValue(uid, out Terminal e);
            return e;
        }

        internal void Accept(ushort port)
        {
            protocol.Accept(port);
            protocol.OnAccept = (ret) => {

                if (ret == true)
                {
                    UID = protocol.RemoteEndPoint.ToInt64Address();
                    clients.TryRemove(UID, out Terminal older);
                    clients.TryAdd(UID, this);
                }

            };

            protocol.OnDisconnect = () =>
            {
                clients.TryRemove(UID, out Terminal older);
            };

            if (OnMessage == null)
            {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
                OnMessage = async (notifier, msg) =>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
                {
                    if (delegateNotifiers.TryGetValue(msg.UID, out DelegateNotifier @delegate) == true)
                    {
                        @delegate?.Response(msg);
                    }
                    else
                    {
                        Singleton<ConsoleNotifier>.Instance.Response(msg);
                    }
                    return true;
                };
            }
        }

        public void Connect(string name, string ip, ushort port = 5882)
        {
            Name = name;
            Singleton<Engine.Framework.Container<string, Engine.Network.Protocol.Terminal>>.Instance.Add(name.ToLower(), this);
            protocol.Connect(ip, port);
            if (OnMessage == null)
            {
                OnMessage = async (notifier, msg) =>
                {
                    if (delegateNotifiers.TryGetValue(msg.UID, out DelegateNotifier @delegate) == true)
                    {
                        @delegate?.Response(msg);
                    }
                    else
                    {
                        Singleton<ConsoleNotifier>.Instance.Response(msg);
                    }
                    return true;
                };
            }
            Run(8000);
        }

        public string Name { get; private set; }
        public bool IsConnected => !protocol.IsClosed();

        public Terminal() : base(Singleton<Layer>.Instance)
        {
            protocol.OnConnect = (ret) =>
            {
                if (ret == true)
                {
                    OnConnect?.Invoke();
                    return;
                }



            };

            protocol.OnDisconnect = () => {


                if (protocol.IP != "127.0.0.1")
                {
                    Engine.Framework.Api.Logger.Info($"OnDisconnect Terminal Retry {protocol.IP}");
                }

                OnDisconnect?.Invoke();
                if (string.IsNullOrEmpty(protocol.IP)) { return; }
                

                Task.Run(() => {
                    var ip = protocol.IP;
                    var port = protocol.Port;
                    Task.Delay(1000).Wait();
                    protocol.Connect(protocol.IP, protocol.Port);
                });

            };
            protocol.OnRead = onRead;
        }

        public string IP => protocol.IP;
        public ushort Port => protocol.Port;

        public delegate void CallbackCompletion();

        public CallbackCompletion OnConnect { get; set; }
        public CallbackCompletion OnDisconnect { get; set; }

        public delegate Task<bool> Callback(Engine.Framework.INotifier notifier, Message msg);

        public Callback OnCommand { get; set; }
        public Callback OnMessage { get; set; }

        
        static readonly int NewLineOffset = sizeof(ushort) + sizeof(int);
        static readonly int UIDOffset = sizeof(ushort) + sizeof(int) + sizeof(bool);
        static readonly int AddressOffset = sizeof(ushort) + sizeof(int) + sizeof(bool) + sizeof(long);
        static readonly int CommandOffset = sizeof(ushort) + sizeof(int) + sizeof(bool) + sizeof(long) + sizeof(long);

        private int onRead(MemoryStream transferred)
        {
            int offset = 0;
            byte[] buffer = transferred.GetBuffer();
            while ((transferred.Length - offset) > sizeof(ushort))
            {

                ushort size = BitConverter.ToUInt16(buffer, offset);
                if (size > transferred.Length - offset)
                {
                    break;
                }


                if (BitConverter.ToInt32(buffer, 2) == 0)
                {
                    offset += size;
                    continue;
                }

                try
                {
                    Message msg = new Message();
                    msg.Type = (Message.EType)BitConverter.ToInt32(buffer, offset + 2);
                    msg.NewLine = BitConverter.ToBoolean(buffer, offset + NewLineOffset);
                    msg.Command = Encoding.UTF8.GetString(buffer, offset + CommandOffset, size - CommandOffset);
                    msg.UID = BitConverter.ToInt64(buffer, offset + UIDOffset);
                    msg.Address = BitConverter.ToInt64(buffer, offset + AddressOffset);

                    var notifier = new Engine.Network.Protocol.Terminal.DefaultNotifier();
                    notifier.UID = msg.UID;
                    notifier.From = this;

                    if (msg.Type == Message.EType.Command)
                    {
                        PostMessage(() => { OnCommand?.Invoke(notifier, msg); });
                    }
                    else
                    {
                        OnMessage?.Invoke(this, msg);
                    }
                }
                catch (Exception e)
                {
                    Engine.Framework.Api.Logger.Info(e);
                    protocol.Disconnect();
                }

                offset += size;
            }


            transferred.Seek(offset, SeekOrigin.Begin);
            return 0;
        }

    }
}
