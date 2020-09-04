using Engine.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using static Engine.Framework.Api;

namespace Engine.Network.Protocol
{

    public interface IDelegator
    {
        long UID { get; }
        void Delegate(long from, long to, Engine.Framework.ISerializable serializable);
        void Delegate<T, R>(Engine.Framework.Layer.Task task, long to, T msg, AsyncCallback<R> callback, Engine.Framework.AsyncCallback fallback = null)
           where T : global::Google.Protobuf.IMessage<T>
           where R : global::Google.Protobuf.IMessage<R>;
        void Delegate<T>(Engine.Framework.Layer.Task task, long to, T msg, AsyncCallback<T> callback, Engine.Framework.AsyncCallback fallback = null)
           where T : global::Google.Protobuf.IMessage<T>;
        void Delegate<T>(long from, long to, T msg)
            where T : global::Google.Protobuf.IMessage<T>;
        void Delegate(long from, long to, int code, MemoryStream stream);
        //void Attach(Engine.Framework.IDelegatable from);
        //void Detach(Engine.Framework.IDelegatable from);
        long GetSequence();

    }



    public class Delegator<D> : Engine.Framework.Scheduler, IDelegator where D : Delegator<D>.IDelegatable, new()
    {
        public interface IDelegatable
        {
            void OnConnect(IDelegator delegator, MemoryStream stream);
            //void OnDisconnect(IDelegator delegator);
            void OnDelegate(Notifier notifier, int code, MemoryStream stream);
        }

        public Delegator()
        //: base(Engine.Framework.Api.Singleton<Engine.Framework.Api.ScheduleLayer>.Instance)
        {
        }


        DateTime lastHeartBeat = DateTime.UtcNow.AddSeconds(10);
        protected override void OnSchedule(long deltaTicks)
        {
            if (string.IsNullOrEmpty(this.Ip) == true)
            {
                if (lastHeartBeat < DateTime.UtcNow && Api.DelegatorHeartBeat == true)
                {
                    Console.WriteLine($"Delegator<{typeof(D).FullName}> LastHeartBeat = {lastHeartBeat.ConvertTimeFromUtc("Korea Standard Time")} Disconnect!");
                    Protocol.Disconnect();
                }
                return;
            }

            if (Protocol.IsEstablish() == true)
            {
                var size = sizeof(int) + sizeof(long) + sizeof(long) + sizeof(long) +
                       sizeof(int) + sizeof(long);

                var header = new MemoryStream(size);
                BinaryWriter binaryWriter = new BinaryWriter(header);
                binaryWriter.Write(size);
                binaryWriter.Write((long)ulong.MinValue); //from
                binaryWriter.Write((long)ulong.MinValue); // to
                binaryWriter.Write((long)ulong.MinValue);           // seq
                binaryWriter.Write((int)1);          // code
                binaryWriter.Write((long)UID);          // res
                binaryWriter.Flush();
                header.Seek(0, SeekOrigin.Begin);
                Protocol.Write(header);
            }

        }

        protected override void OnClose()
        {
            Disconnect();
            Remove(Id);
            Console.WriteLine($"OnClose Delegator<{typeof(D).FullName}> From {Engine.Framework.Api.UInt32ToIPAddress((uint)UID)} at {DateTime.UtcNow.ConvertTimeFromUtc("Korea Standard Time")}");
        }

        public Engine.Network.Protocol.Tcp.AsyncDisconnectCallback OnDisconnect { get; set; }
        public Engine.Network.Protocol.Tcp.AsyncConnectCallback OnConnect { get; set; }

        //myc0058 test code
        protected Engine.Network.Protocol.Tcp Protocol = new Tcp() { UseCompress = false };
        new public long UID { get { return base.UID; } set { base.UID = value; } }
        private static ConcurrentDictionary<long, Delegator<D>> delegators = new ConcurrentDictionary<long, Delegator<D>>();
        public long Id { get; private set; }

        public static Delegator<D> Create()
        {
            var d = new Delegator<D>();
            d.Id = d.UID;
            return d;
        }

        public static Delegator<D> Create(long id)
        {
            if (delegators.ContainsKey(id) == true)
            {
                delegators.TryGetValue(id, out Delegator<D> d);
                return d;
            }
            else
            {
                var d = new Delegator<D>();
                d.Id = id;
                delegators.TryAdd(id, d);
                return d;
            }
        }

        public static ICollection<long> Keys
        {
            get
            {
                return delegators.Keys;
            }
        }

        public static Delegator<D>[] Values
        {
            get
            {
                return delegators.Values.ToArray();
            }
        }


        public static int Count
        {
            get
            {
                return delegators.Count;
            }
        }

        public static Delegator<D> Get(long id)
        {
            delegators.TryGetValue(id, out Delegator<D> d);
            return d;
        }

        public static Delegator<D> Add(long id, Delegator<D> delegator)
        {
            delegators.TryAdd(id, delegator);
            return delegator;
        }

        public static Delegator<D> Remove(long id)
        {
            delegators.TryRemove(id, out Delegator<D> delegator);
            return delegator;
        }

        private delegate void ResponseCallback(global::System.IO.Stream stream);
        private delegate void ResponseFallback();
        private ConcurrentDictionary<long, (ResponseCallback, ResponseFallback)> waitResponse = new ConcurrentDictionary<long, (ResponseCallback, ResponseFallback)>();

        public class Serializer : Engine.Framework.ISerializable
        {
            public void Serialize(Stream output)
            {
                output.Write(BitConverter.GetBytes(Length), 0, 4);
                output.Write(BitConverter.GetBytes(From), 0, 8);
                output.Write(BitConverter.GetBytes(To), 0, 8);
                output.Write(BitConverter.GetBytes(Sequence), 0, 8);
                output.Write(BitConverter.GetBytes(Code), 0, 4);
                output.Write(BitConverter.GetBytes(Responsable), 0, 8);
                Message.CopyTo(output);
            }
            public int Length
            {
                get
                {
                    if (length == 0)
                    {
                        length = (int)Message.Length + sizeof(int) + sizeof(long) + sizeof(long) + sizeof(int) + sizeof(long) + sizeof(long);
                    }
                    return length;
                }
            }

            protected int length { get; set; }
            public Stream Message;
            public int Code;
            public long From;
            public long To;
            public long Sequence;
            public long Responsable;
        }

        public class Serializer<T> : Engine.Framework.ISerializable
        {
            public void Serialize(Stream output)
            {

                var proto = Message as Google.Protobuf.IMessage;
                output.Write(BitConverter.GetBytes(Length), 0, 4);
                output.Write(BitConverter.GetBytes(From), 0, 8);
                output.Write(BitConverter.GetBytes(To), 0, 8);
                output.Write(BitConverter.GetBytes(Sequence), 0, 8);
                output.Write(BitConverter.GetBytes(Engine.Framework.Id<T>.Value), 0, 4);
                output.Write(BitConverter.GetBytes(Responsable), 0, 8);

                proto.Serialize(output, true);
               
            }

            public int Length
            {
                get
                {
                    if (length == 0)
                    {
                        var proto = Message as Google.Protobuf.IMessage;
                        length = proto.CalculateSize() + sizeof(int) + sizeof(long) + sizeof(long) + sizeof(int) + sizeof(long) + sizeof(long);
                    }
                    return length;
                }
            }
            protected int length { get; set; }
            public T Message;
            public long From;
            public long To;
            public long Sequence;
            public long Responsable;
        }

        public void Disconnect()
        {
            lock (this)
            {
                Protocol.Disconnect();
            }
        }

        public class Notifier : Engine.Framework.INotifier
        {
            public void Response<T>(T msg)
            {
                if (Responsable == 0) {
                  //  Console.WriteLine($"Delegator Notifier Response But Responsable == {Responsable}, {typeof(T)}");
                    return;
                }
                lock (Delegator)
                {
                    Delegator.Delegate(To, From, new Serializer<T>() { Message = msg, From = To, To = From, Sequence = 0, Responsable = Responsable } as ISerializable);
                }
                
            }

            public void Notify<T>(T msg)
            {
                lock (Delegator)
                {
                    Delegator.Delegate(To, From, new Serializer<T>() { Message = msg, From = To, To = From, Sequence = Delegator.GetSequence() } as ISerializable);
                }
            }

            public void Response(int code, Stream stream)
            {
                lock (Delegator)
                {
                    Delegator.Delegate(To, From, new Serializer() { Message = stream, From = To, To = From, Code = code, Sequence = 0, Responsable = Responsable });
                }
            }


            public IDelegator Delegator;
            public long UID;
            internal protected long Responsable;
            public long From;
            public long To;
        }

        public MemoryStream ConnectStream { get; set; } = new MemoryStream();
        private bool IsConnected = false;
        protected void onConnect(bool ret)
        {
            if (ret == true)
            {
                Console.WriteLine($"Delegator<{typeof(D).FullName}> Connected To {Protocol.IP} at {DateTime.UtcNow.ConvertTimeFromUtc("Asia/Seoul")}");
                lastHeartBeat = DateTime.UtcNow;
                IsConnected = true;
                lock (this)
                {

                    var size = sizeof(int) + sizeof(long) + sizeof(long) + sizeof(long) +
                        sizeof(uint) + sizeof(long) + sizeof(int) + (int)ConnectStream.Length;

                    var tokens = Ip.Split(',');

                    if (tokens.Length > 1)
                    {
                        var header = new MemoryStream(10);
                        BinaryWriter binaryWriter = new BinaryWriter(header);
                        binaryWriter.Write((int)10);
                        binaryWriter.Write((uint)Engine.Framework.Api.IPAddressToUInt32(tokens[1]));
                        binaryWriter.Write(Port);
                        binaryWriter.Flush();
                        header.Seek(0, SeekOrigin.Begin);
                        Protocol.Write(header);
                    }

                    {
                        var header = new MemoryStream(size);
                        BinaryWriter binaryWriter = new BinaryWriter(header);
                        binaryWriter.Write(size);
                        binaryWriter.Write((long)ulong.MinValue); //from
                        binaryWriter.Write((long)ulong.MinValue); // to
                        binaryWriter.Write(recvSequence);           // seq
                        binaryWriter.Write(uint.MinValue);          // code
                        binaryWriter.Write((long)UID);          // res
                        binaryWriter.Write((int)ConnectStream.Length);
                        binaryWriter.Write(ConnectStream.GetBuffer(), 0, (int)ConnectStream.Length);
                                                                //binaryWriter.Write((int)bytes.Length);
                                                                //binaryWriter.Write(bytes, 0, bytes.Length);
                                                                //binaryWriter.Write(port);

                        binaryWriter.Flush();
                        header.Seek(0, SeekOrigin.Begin);
                        Protocol.Write(header);
                        OnConnect?.Invoke(ret);
                    }

                }
                Run(3000);
            }
            else
            {
                Console.WriteLine($"Delegator<{typeof(D).FullName}> Connect Fail {Protocol.IP} at {DateTime.UtcNow.ConvertTimeFromUtc("Asia/Seoul")}");
            }
        }

        public void Delegate<T>(Engine.Framework.Layer.Task task, long to, T msg, Engine.Framework.AsyncCallback<T> callback, Engine.Framework.AsyncCallback fallback = null)
            where T : global::Google.Protobuf.IMessage<T>
        {

            lock (this)
            {
                ResponseCallback responder = (cis) =>
                {
                    T ret = Api.ProtobufParser<T>.Parser.ParseFrom(cis);
                    task.PostMessage(() => { callback(ret); });
                };


                ResponseFallback responseFallback = null;
                if (fallback != null)
                {
                    responseFallback = () => { task.PostMessage(fallback); };
                }

                var serialzable = new Serializer<T>();
                serialzable.From = task.UID;
                serialzable.To = to;
                serialzable.Message = msg;
                serialzable.Sequence = GetSequence();
                serialzable.Responsable = GetSequence();
                if (waitResponse.TryAdd(serialzable.Responsable, (responder, responseFallback)) == false)
                {
                    Console.WriteLine($"{this.GetType()} waitResponse Add Fail. msg : {msg.GetType()}, json : {msg.ToJson()}");
                    responseFallback?.Invoke();
                    return;
                }

                if (Protocol.Write(serialzable) == false)
                {

                    Console.WriteLine($"{this.GetType()} Write Fail. msg : {msg.GetType()}, json : {msg.ToJson()}");
                    waitResponse.TryRemove(serialzable.Responsable, out (ResponseCallback, ResponseFallback) failed);
                    failed.Item2?.Invoke();
                    return;
                }
            }
        }

        protected long sequence = 0;
        public long GetSequence() {
            {
                return Interlocked.Increment(ref sequence);
            }
        }

        public void Delegate<T, R>(Engine.Framework.Layer.Task task, long to, T msg, Engine.Framework.AsyncCallback<R> callback, Engine.Framework.AsyncCallback fallback = null) 
            where T : global::Google.Protobuf.IMessage<T>
            where R : global::Google.Protobuf.IMessage<R>
        {

            lock (this)
            {
                ResponseCallback responder = (cis) =>
                {
                    var ret = Api.ProtobufParser<R>.Parser.ParseFrom(cis);
                    task.PostMessage(() => { callback(ret); });
                };
                ResponseFallback responseFallback = null;
                
                if (fallback != null)
                {
                    responseFallback = () => { task.PostMessage(fallback); };
                }


                var serialzable = new Serializer<T>();
                serialzable.From = task.UID;
                serialzable.To = to;
                serialzable.Message = msg;
                serialzable.Sequence = GetSequence();
                serialzable.Responsable = GetSequence();

                if (waitResponse.TryAdd(serialzable.Responsable, (responder, responseFallback)) == false)
                {
                    Console.WriteLine($"{this.GetType()} waitResponse Add Fail. msg : {msg.GetType()}, json : {msg.ToJson()}");
                    responseFallback?.Invoke();
                    return;
                }

                if (Protocol.Write(serialzable) == false)
                {
                    Console.WriteLine($"{this.GetType()} Write Fail. msg : {msg.GetType()}, json : {msg.ToJson()}");
                    waitResponse.TryRemove(serialzable.Responsable, out (ResponseCallback, ResponseFallback) failed);
                    failed.Item2?.Invoke();
                    return;
                }
                
            }

        }

        public void Delegate(long from, long to, int code, MemoryStream stream, Engine.Framework.AsyncCallback<Stream> callback, Engine.Framework.AsyncCallback fallback = null)
        {
            lock (this)
            {

                ResponseCallback responder = (cis) =>
                {
                    callback(cis);
                };
                ResponseFallback responseFallback = () => { fallback?.Invoke(); };


                
                var header = new MemoryStream(sizeof(int) + sizeof(long) + +sizeof(long) + sizeof(long) + sizeof(int) + sizeof(long));
                BinaryWriter binaryWriter = new BinaryWriter(header);
                binaryWriter.Write((int)stream.Length + sizeof(int) + sizeof(long) + sizeof(long) + sizeof(long) + sizeof(int) + sizeof(long));
                binaryWriter.Write(from);
                binaryWriter.Write(to);
                binaryWriter.Write(GetSequence());
                binaryWriter.Write(code);

                var response = GetSequence();

                binaryWriter.Write((long)response);

                waitResponse.TryAdd(response, (responder, responseFallback));
                if (Protocol.Write(header, stream) == false)
                {
                    waitResponse.TryRemove(response, out (ResponseCallback, ResponseFallback) failed);
                    failed.Item2?.Invoke();
                }

            }
        }

        public virtual void Delegate<T>(long from, long to, T msg)
            where T : global::Google.Protobuf.IMessage<T>
        {
            lock (this)
            {
                var serialzable = new Serializer<T>();
                serialzable.From = from;
                serialzable.To = to;
                serialzable.Message = msg;
                serialzable.Sequence = GetSequence();
                Protocol.Write(serialzable);
            }
        }

        public void Delegate(long from, long to, int code, MemoryStream stream)
        {
            lock (this)
            {
                var header = new MemoryStream(sizeof(int) + sizeof(long) + +sizeof(long) + sizeof(long) + sizeof(int) + sizeof(long));
                BinaryWriter binaryWriter = new BinaryWriter(header);
                binaryWriter.Write((int)stream.Length + sizeof(int) + sizeof(long) + sizeof(long) + sizeof(long) + sizeof(int) + sizeof(long));
                binaryWriter.Write(from);
                binaryWriter.Write(to);
                binaryWriter.Write(GetSequence());
                binaryWriter.Write(code);
                binaryWriter.Write((long)ulong.MinValue);
                Protocol.Write(header, stream);
            }
        }

        public void Delegate(long from, long to, Engine.Framework.ISerializable serializable)
        {
            lock (this)
            {
                Protocol.Write(serializable);
            }
            
        }

        public static void Event(long delegatable, long to, int code, MemoryStream stream)
        {
            lock (delegators)
            {
                foreach (var e in delegators.Values)
                {
                    e.Delegate(delegatable, to, code, new MemoryStream(stream.ToArray()));
                }
            }

        }


        virtual public void OnDelegate(long seq, long res, long from, long to, int code, MemoryStream stream)
        {
            if (seq == 0)
            {
                if (waitResponse.TryRemove(res, out (ResponseCallback, ResponseFallback) responder) == true)
                {
                    responder.Item1?.Invoke(stream);
                    return;
                }
            }

            var notifier = new Notifier();
            notifier.Delegator = this;
            notifier.UID = UID;
            notifier.From = from;
            notifier.To = to;
            notifier.Responsable = res;

            Singleton<D>.Instance.OnDelegate(notifier, code, stream);

        }

        virtual protected void onAccept(bool ret)
        {
            if (ret == true)
            {
                Console.WriteLine($"Delegator<{typeof(D).FullName}> Accepted {Protocol.RemoteEndPoint.GetIp()} at {DateTime.UtcNow.ConvertTimeFromUtc("Korea Standard Time")}");
                IsConnected = true;
                recvSequence = 0;
                lastHeartBeat = DateTime.UtcNow.AddSeconds(15);
                Protocol.RecvBufferSize = 1024 * 1000 * 10;
                Run(3000);
            }
        }

        public void Connect(string ip, ushort port)
        {
            this.Ip = ip;
            this.Port = port;

            if (Protocol.IsClosed() == false) { return; }
            Protocol.OnConnect = onConnect;
            Protocol.OnRead = onRead;
            Protocol.OnDisconnect = onDisconnect;
            Protocol.RecvBufferSize = 1024 * 1000 * 10;
            
            var tokens = ip.Split(',');
            Protocol.Connect(tokens[0], port);

        }

        public string Ip { get; set; } = string.Empty;
        public ushort Port = 0;
        public void Accept(ushort port)
        {
            Protocol.OnRead = onRead;
            Protocol.OnDisconnect = onDisconnect;
            Protocol.OnAccept = onAccept;
            Protocol.Accept(port);
            
        }

        public static void Listen(ushort port)
        {
            Engine.Network.Api.Listen(port, () => {
                new Engine.Network.Protocol.Delegator<D>().Accept(port);
            });
        }


        virtual protected void onDisconnect()
        {

            if (IsConnected)
            {
                IsConnected = false;

                if (string.IsNullOrEmpty(Protocol.IP))
                {
                    Console.WriteLine($"Delegator<{typeof(D).FullName}> Disconnected From {Engine.Framework.Api.UInt32ToIPAddress((uint)UID)} at {DateTime.UtcNow.ConvertTimeFromUtc("Korea Standard Time")}");
                }
                else
                {
                    Console.WriteLine($"Delegator<{typeof(D).FullName}> Disconnected From {Protocol.IP} at {DateTime.UtcNow.ConvertTimeFromUtc("Korea Standard Time")}");
                }

                
            }

            var waits = waitResponse.Values.ToArray();
            waitResponse.Clear();

            foreach (var e in waits)
            {
                e.Item2?.Invoke();
            }

            if (string.IsNullOrEmpty(Ip) == false)
            {
                if (IsClose() == true) { return; }
                Task.Run(() => {
                    if (Engine.Network.Api.IsOpen == false) return;
                    Task.Delay(1000).Wait();
                    Connect(this.Ip, Port);

                });
            }
            else
            {
                Close();
                Console.WriteLine($"Close Delegator<{typeof(D).FullName}> Disconnected From {Engine.Framework.Api.UInt32ToIPAddress((uint)UID)} at {DateTime.UtcNow.ConvertTimeFromUtc("Korea Standard Time")}");
            }

            try
            {
                //Singleton<D>.Instance.OnDisconnect(this);
                OnDisconnect?.Invoke();
            }
            catch
            {

            }

        }

        protected long recvSequence = 0;
        private int onRead(MemoryStream transferred)
        {
            int offset = 0;
            byte[] buffer = transferred.GetBuffer();
            while ((transferred.Length - offset) > sizeof(int))
            {
                int size = BitConverter.ToInt32(buffer, offset);

                if (size < 1)
                {
                    Console.WriteLine($"size < 1 ,{this.GetType()}");
                    transferred.Seek(transferred.Length, SeekOrigin.Begin);
                    Protocol.Disconnect();
                    return 0;
                }

                if (size > transferred.Length - offset)
                {
                    break;
                }

                long from = BitConverter.ToInt64(buffer, offset + 4);
                long to = BitConverter.ToInt64(buffer, offset + 12);
                long seq = BitConverter.ToInt64(buffer, offset + 20);
                int code = BitConverter.ToInt32(buffer, offset + 28);
                long res = BitConverter.ToInt64(buffer, offset + 32);

                if (code == 0)
                {
                    // TODO:
                    // delegators가 락프리 알고리즘으로 이부분에 타이밍상 버그가 생길 가능성이 있다.
                    // 발생가능성?
                    //lock (delegators)
                    {
                        // listen 
                        // try merge before delegator requests
                        UID = res;

                        

                        

                        delegators.TryRemove(UID, out Delegator<D> delegator);
                        delegators.TryAdd(UID, this);
                        if (delegator != null)
                        {
                            Console.WriteLine($"Delegator Regist {UID}, {UInt32ToIPAddress((uint)UID)}, Sequence {seq} at {DateTime.UtcNow.ConvertTimeFromUtc("Korea Standard Time")}");
                            lock (delegator)
                            {
                                var tcp = delegator.Protocol;
                                sequence = delegator.sequence;
                                delegator.Protocol = Protocol;
                                //Protocol.Merge(tcp);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Delegator First Regist {UID}, {UInt32ToIPAddress((uint)UID)}, Sequence {seq} at {DateTime.UtcNow.ConvertTimeFromUtc("Korea Standard Time")}");
                            sequence = seq;
                        }

                        int streamSize = BitConverter.ToInt32(buffer, offset + 40);
                        var stream = new MemoryStream(buffer, offset + 44, streamSize);
                        Singleton<D>.Instance.OnConnect(this, stream);
                    }
                }
                else if (code == 1)
                {
                    lastHeartBeat = DateTime.UtcNow.AddSeconds(15);
                }
                else
                {
                    //if (seq > recvSequence)
                    {
                        recvSequence = seq;
                        try
                        {
                            OnDelegate(seq, res, from, to, code, new MemoryStream(buffer, offset + 40, size - 40, true, true));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Delegator {this.GetType()} OnDelegate Exception : " + e);
                        }
                    }
                    //else
                    //{
                    //    Console.WriteLine($"Delegator {this.GetType()} OnDelegate Sequence pass : {seq} > {recvSequence}, code : {code}");
                    //}
                }
                offset += size;
            }

            transferred.Seek(offset, SeekOrigin.Begin);
            return 0;
        }

        public Engine.Framework.Layer.Task Handler { get; set; }

        Engine.Network.Protocol.Tcp Tcp = new Tcp();

    }
}
