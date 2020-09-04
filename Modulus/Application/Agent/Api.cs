using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using Engine.Framework;
using Engine.Network.Protocol;
using static Engine.Framework.Api;

namespace Application.Agent
{
    public static partial class Api
    {
        public static Terminal Modulus { get; internal set; }
        static ConcurrentDictionary<long, Engine.Network.Protocol.Terminal> waitCommands = new ConcurrentDictionary<long, Terminal>();
        public static ushort Port { get; set; } = 7880;
        public static ushort AdminPort { get; set; } = 7881;
        public static string ServerIp { get; set; } = string.Empty;
        public static bool IsServer { get; set; } = false;
        public static bool StandAlone { get; set; } = false;

        public static void StartUp()
        {

            if (IsServer == true)
            {
                Console.WriteLine($"--------------- Agent Server StartUp --------------");
                Engine.Network.Protocol.Delegator<Delegatables.Server>.Listen((ushort)(Port + 1000));
                Engine.Network.Protocol.Delegator<Delegatables.Admin>.Listen((ushort)(AdminPort + 1000));

                try
                {
                    AdminWebService();
                }
                catch
                {
                    Logger.Error("AdminWebService is not working. Execute Visual Studio with Admin Account or... ");
                    Logger.Error("netsh http add urlacl url = \"http://*:5282/\" user = everyone");
                }
            }

            if (string.IsNullOrEmpty(ServerIp) == false)
            {
                Console.WriteLine($"--------------- Agent Client StartUp --------------");

                var delegator = Singleton<Engine.Network.Protocol.Delegator<Delegatables.Client>>.Instance;
                delegator.UID = Engine.Network.Api.Idx;
                var info = new Schema.Protobuf.Message.Administrator.ConnectedAgentInfo();
                
                foreach (var e in Basis.Metadata.Api.ServerTypes)
                {
                    info.ServerTypes.Add(e.ToString());
                }
                
                info.Address = Engine.Network.Api.Idx;

                uint ip = (uint)info.Address;
                Engine.Framework.Api.Logger.Info($"{info.ToJson()} - Ip : {Engine.Framework.Api.UInt32ToIPAddress(ip)}");


                info.UID = delegator.UID;
                delegator.ConnectStream = info.ToMemoryStream();
                delegator.Connect(ServerIp, (ushort)(Port + 1000));
                Engine.Network.Protocol.Delegator<Delegatables.Process>.Listen((ushort)(Port + 2000));

                Logger.Info($"Agent Address : {info.Address}");
            }
        }

        static HttpListener listener = new HttpListener();
        public static void AdminWebService()
        {
            listener.Prefixes.Add(string.Format("http://*:{0}/", 5282));
            listener.Start();
            void Response(HttpListenerContext ctx)
            {
                string[] codes = ctx.Request.RawUrl.Split('/');

                int code = codes[codes.Length - 1].ToInt32();

                try
                {
                    var web = Engine.Framework.Api.Singleton<Entities.Admin>.Instance;

                    var callback = Schema.Protobuf.Api.Bind(web, new Entities.Admin.Notifier() { Context = ctx }, code, ctx.Request.InputStream);

                    web.PostMessage(callback);

                    
                }
                catch
                {
                    ctx.Response.ContentType = "application/json";
                    ctx.Response.StatusCode = 500;
                    ctx.Response.Close();
                }
            }


            ThreadPool.QueueUserWorkItem((o) =>
            {
                Engine.Framework.Api.Logger.Info("HttpListen  Started...." + 5282);
                try
                {
                    while (listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                Response(ctx);

                            }
                            catch (Exception e)
                            {
                                Engine.Framework.Api.Logger.Info(e);

                            } // suppress any exceptions
                                finally
                            {
                                    // always close the stream
                                }
                        }, listener.GetContext());
                    }
                }
                catch { } // suppress any exceptions
                });


        }
    }

}
