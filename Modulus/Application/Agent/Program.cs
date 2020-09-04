using Engine.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Engine.Framework.Api;
using static Engine.Network.Api.Terminal;
using static Engine.Network.Protocol.Terminal;

namespace Application.Agent
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args == null || args.Length == 0)
            {
                Console.WriteLine("dotnet Application.Agent.dll [ Lobby | Game | Sync | Match ]");
                return;
            }

            Schema.Protobuf.Api.StartUp();
            Basis.Metadata.Api.ReadMetadata = false;
            Basis.Metadata.Api.StartUp(args);
            Engine.Network.Api.Binder = Schema.Protobuf.Api.Bind;


            foreach (var e in args)
            {
                switch (e.ToLower())
                {
                    case string b when b.StartsWith("server"):
                        Api.IsServer = true;
                        continue;
                    case string b when b.StartsWith("ip"):
                        Api.ServerIp = e.Split('=')[1];
                        continue;
                    case string b when b.StartsWith("port"):
                        Api.Port = e.Split('=')[1].ToUInt16();
                        continue;
                    case string b when b.StartsWith("admin"):
                        Api.AdminPort = e.Split('=')[1].ToUInt16();
                        continue;
                    default:
                        continue;
                }
            }


            if (Api.IsServer == true)
            {
                Console.WriteLine($"StartUp Agent Server");
            }
            else
            {
                Console.WriteLine($"StartUp Agent Client");
            }
         

            Engine.Framework.Api.StartUp();
            Engine.Network.Api.StartUp();
            Engine.Database.Api.StartUp();

            Api.StartUp();

            var list = new List<Callback>();

            Engine.Network.Api.Terminal.Run(list).Wait();
            

            Engine.Database.Api.CleanUp();
            Engine.Network.Api.CleanUp();
            Engine.Framework.Api.CleanUp();
        }
    }
}
