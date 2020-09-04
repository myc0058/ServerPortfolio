using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool.MockDedicateServer.Protocol;
using Tool.MockDedicateServer.Utils;

namespace Tool.MockDedicateServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Schema.Protobuf.Api.StartUp();
            Basis.Metadata.Api.StartUp(args);
            Engine.Network.Api.Binder = Schema.Protobuf.Api.Bind;

            Engine.Framework.Api.StartUp();
            Engine.Network.Api.StartUp();

            Api.StartUp();

            Api.WriteDebugLog("Mock Dedicate Server Start!!");
            Api.WriteDebugLog("============ Execute Params Begin");
            for(int i = 0; i < args.Length; i++)
            {
                if (i == 0)
                {
                    string mapName = args[i];
                    int indexQ = mapName.IndexOf('?');

                    if (indexQ >= -1)
                    {
                        mapName = mapName.Substring(0, indexQ);
                    }
                    Api.SetModeByMapname(mapName);
                    Console.Title = Api.BattleMode.ToString();
                }
                else
                {
                    string[] argElement = args[i].Split('=');

                    if (argElement.Length >= 2 && argElement[0] == "-port")
                        Api.ListenPortForClient = int.Parse(argElement[1]);
                }

                Api.WriteDebugLog(args[i]);
            }

            Api.WriteDebugLog("============ Execute Params End");

            if (Api.ListenPortForClient == 0)
            {
                Engine.Framework.Api.Logger.Info("No -port Execute Params Press Exit.");
                Console.ReadLine();
                return;
            }

            Client.Instance.Connect(Config.Instance.GameServerIP, Config.Instance.GameServerPort);
            Api.WriteDebugLog("Connecting to GameServer...");

            bool exit = false;
            while (exit == false)
            {
                var cmd = Console.ReadLine();
                //cmd = cmd.ToLower();
                switch (cmd)
                {
                    case "resend":
                        Client.Instance.ResendLastPacket();
                        break;
                    case "exit":
                        exit = true;
                        break;
                }
            }
        }
    }
}
