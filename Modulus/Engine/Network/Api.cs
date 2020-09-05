using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using static Engine.Framework.Api;

namespace Engine.Network
{
    public static partial class Api
    {
        public static ushort TERMINAL_PORT { get; set; } = 5882;
        public static ushort GATEWAY_PORT { get; set; } = 6882;

        public class ProtobufParser<T> where T : Google.Protobuf.IMessage<T>
        {
#pragma warning disable RECS0108 // 제네릭 형식의 정적 필드에 대해 경고합니다.
            internal static readonly System.Reflection.PropertyInfo field = typeof(T).GetProperty("Parser", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
#pragma warning restore RECS0108 // 제네릭 형식의 정적 필드에 대해 경고합니다.
            public static readonly Google.Protobuf.MessageParser<T> Parser = (Google.Protobuf.MessageParser<T>)field.GetValue(null);
            public Google.Protobuf.MessageParser GetParser()
            {
                return Parser;
            }
        }

        public delegate global::Engine.Framework.AsyncCallback Bind(dynamic handler, dynamic notifier, int code, global::System.IO.Stream stream);

        public static Bind Binder = (dynamic handler, dynamic notifier, int code, global::System.IO.Stream stream) => 
        {
            Console.WriteLine("Did not set rpc binder. Are you missing? 'Engine.Network.Api.Binder = delegate'");
            return () => { };
        };

        static Dictionary<ushort, Socket> listeners = new Dictionary<ushort, Socket>();
        static Dictionary<ushort, ListenCallback> listenCallbacks = new Dictionary<ushort, ListenCallback>();
        public delegate void ListenCallback();

        public delegate void OnDisconnectCallback();
        static internal ConcurrentQueue<Engine.Network.Protocol.Tcp.AsyncDisconnectCallback> OnDisconnectCallbacks = new ConcurrentQueue<Protocol.Tcp.AsyncDisconnectCallback>();


        static internal Socket Acceptor(ushort port)
        {

            if (Engine.Network.Api.IsOpen == false) return null;
            Socket socket;
            if (listeners.TryGetValue(port, out socket) == true)
            {
                return socket;
            }

            socket = new Socket(AddressFamily.InterNetwork,
              SocketType.Stream, ProtocolType.Tcp);

            //IPAddress hostIP = Dns.Resolve(IPAddress.Any.ToString()).AddressList[0];
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, port);

            try
            {
                socket.Bind(ep);
                socket.Listen(1024);

                listeners.Remove(port);
                listeners.Add(port, socket);
            }
            catch
            {
                //try
                //{
                //    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                //    listeners.Remove(port);
                //    listeners.Add(port, socket);
                //}
                //catch
                {
                    throw;
                }
            }
            return socket;
        }

        static public void Listen(ushort port)
        {

            if (Engine.Network.Api.IsOpen == false) return;
            ListenCallback callback = null;
            if (listenCallbacks.TryGetValue(port, out callback) == true)
            {
                callback();
            }

        }

        private static bool isOpen = false;

        internal static bool IsOpen
        {
            get { return isOpen; }
        }

        static public string PublicIp { get; private set; }
        static public string PrivateIp { get; private set; }
        public static long Idx { get; set; }
        public static bool DelegatorHeartBeat { get; set; } = true;

        static public void StartUp()
        {
            if (isOpen == true) { return; }

            String strHostName = Dns.GetHostName();
            IPHostEntry iphostentry = Dns.GetHostEntry(strHostName);

            foreach (IPAddress ipaddress in iphostentry.AddressList)
            {
                if (ipaddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    PrivateIp = ipaddress.ToString();
                }
            }
            PublicIp = GetPublicIp();
            Idx = IPAddressToUInt32(PrivateIp);

            Layer.Instance = new Layer();
            Framework.Api.Add(Layer.Instance);
            isOpen = true;
            Console.WriteLine("Engine.Network.Api.StartUp");
        }
        static public void CleanUp()
        {
            isOpen = false;
        }

        static public bool Listen(ushort port, ListenCallback heartbeat)
        {

            return Listen(port, 128, heartbeat);

        }

        static public bool Listen(ushort port, ushort backlog, ListenCallback callback)
        {

            if (listenCallbacks.ContainsKey(port) == true)
            {
                return false;
            }
            listenCallbacks.Add(port, callback);

            for (int i = 0; i < backlog; ++i)
            {
                callback();
            }
            return true;

        }

        class Layer : Engine.Framework.Layer
        {
            internal static Layer Instance = null;
            public override int OnUpdate()
            {
                return Api.Update();
            }
        }

        static public int Update()
        {

            int count = OnDisconnectCallbacks.Count;

            Engine.Network.Protocol.Tcp.AsyncDisconnectCallback callback;
            for (int i = 0; i < count; ++i)
            {

                if (OnDisconnectCallbacks.TryDequeue(out callback))
                {

                    if (callback != null)
                    {
                        try
                        {
                            callback();
                        }
                        catch (Exception)
                        {

                        }
                    }
                }

            }

            return OnDisconnectCallbacks.Count;

        }


        static public Google.Protobuf.ByteString ToByteStringWithCode<T>(this T msg) where T : Google.Protobuf.IMessage<T>
        {
            return ToByteString(Engine.Framework.Id<T>.Value, msg);
        }

        static public Google.Protobuf.ByteString ToByteString<T>(int code, T msg) where T : Google.Protobuf.IMessage
        {
            var stream = new MemoryStream(4096);
            using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                bw.Write(code);
                using (var co = new Google.Protobuf.CodedOutputStream(stream, true))
                {
                    msg.WriteTo(co);
                }

                bw.Seek(0, SeekOrigin.Begin);
                return Google.Protobuf.ByteString.FromStream(stream);
            }
                
        }

        static public string GetPublicIp()
        {

            while (true)
            {
                try
                {
                    string result = new WebClient().DownloadString("https://checkip.amazonaws.com/");
                    result = result.Replace("\n", "");
                    Logger.Info($"My Public Ip : {result}");
                    return result;
                }
                catch
                {
                    Logger.Error($"Cannot Public IP");
                    continue;
                }
            }
        }
    }
}
