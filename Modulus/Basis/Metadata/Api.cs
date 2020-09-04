using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basis.Metadata
{
    public static class Api
    {

        public async static Task<bool> OnCommand(Engine.Framework.INotifier notifier, Engine.Network.Protocol.Terminal.Message msg)
        {
            if (msg.Command.ToLower().StartsWith("metadata"))
            {
                Engine.Framework.Attributes.Metadata.StartUp();//.Wait();
                return true;
            }
            return false;
        }

        public static Schema.Protobuf.CSharp.Enums.EServer ServerType { get; set; } = Schema.Protobuf.CSharp.Enums.EServer.StandAlone;
        public static List<Schema.Protobuf.CSharp.Enums.EServer> ServerTypes { get; set; } = new List<Schema.Protobuf.CSharp.Enums.EServer>();


        public static bool ReadMetadata { get; set; } = true;
        public static bool Silence { get; set; } = false;
        public static string Version { get; set; } = "0.0.0.0";
        public static void StartUp(string[] args)
        {
            foreach (var e in args)
            {
                switch (e)
                {
                    case string b when b.ToLower().StartsWith("metadata="):
                        {
                            var tokens = e.Split('=');
                            Version = tokens[1];
                        }

                        break;
                    case string b when b.ToLower().StartsWith("agents="):
                        {
                            var tokens = e.Split('=');
                            tokens = tokens[1].Split(',');

                            foreach (var t in tokens)
                            {
                                try
                                {
                                    var type = Enum.Parse<Schema.Protobuf.CSharp.Enums.EServer>(t, true);
                                    Basis.Metadata.Api.ServerTypes.Remove(type);
                                    Basis.Metadata.Api.ServerTypes.Add(type);
                                }
                                catch
                                {

                                }
                            }

                        }
                        break;
                    case string b when b.ToLower().StartsWith("service"):
                        Engine.Network.Api.Terminal.Service = true;
                        continue;
                    case string b when b.ToLower().StartsWith("nocommand"):
                        Engine.Network.Api.Terminal.Service = true;
                        continue;
                    case string b when b.ToLower().StartsWith("silence"):
                        Silence = true;
                        continue;
                    default:
                        break;
                }
            }


            if (Engine.Network.Api.Terminal.Service == true || Basis.Metadata.Api.Silence == true)
            {
                Engine.Framework.Api.Silence = true;
            }

            Engine.Framework.Api.Logger.Info($"ServerType : {Api.ServerType.ToString()}");
            
            Engine.Framework.Api.Config = new FileStream("./Config.xml", FileMode.Open);

            if (ReadMetadata == true)
            {
                if (ServerType == Schema.Protobuf.CSharp.Enums.EServer.Game ||
                ServerType == Schema.Protobuf.CSharp.Enums.EServer.Lobby ||
                ServerType == Schema.Protobuf.CSharp.Enums.EServer.Match ||
                ServerType == Schema.Protobuf.CSharp.Enums.EServer.Synchronize ||
                ServerType == Schema.Protobuf.CSharp.Enums.EServer.StandAlone)

                {
                    Engine.Framework.Attributes.Metadata.Version = $"{Version}";
                    Engine.Framework.Attributes.Metadata.StartUp();//.Wait();
                }
            }

        }
    }
}
