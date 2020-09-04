using Engine.Database;
using Engine.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using static Engine.Framework.Api;
using dbms = Engine.Database.Management;

namespace Application.StandAlone
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            Application.Game.Api.StandAlone = true;
            Application.Lobby.Api.StandAlone = true;
            Application.Match.Api.StandAlone = true;
            Application.Synchronize.Api.StandAlone = true;
            Application.Agent.Api.StandAlone = true;
            Application.Agent.Api.IsServer = true;
            Application.Agent.Api.ServerIp = "127.0.0.1";
            

            Application.Lobby.Api.HeartBeatInterval = 600;
            Schema.Protobuf.Api.StartUp();


            Basis.Metadata.Api.ServerType = Schema.Protobuf.CSharp.Enums.EServer.StandAlone;
            Basis.Metadata.Api.StartUp(args);

            Engine.Network.Api.Binder = Schema.Protobuf.Api.Bind;


            //System.Threading.ThreadPool.SetMinThreads(8, 8);
            //System.Threading.ThreadPool.SetMaxThreads(16, 16);

            Engine.Network.Api.DelegatorHeartBeat = false;
            Engine.Framework.Api.StartUp();
            Engine.Network.Api.StartUp();
            Engine.Database.Api.StartUp();
            
            
            Engine.Database.Management.Driver.AddSession("DynamoDB", new Engine.Database.Management.Amazon.DynamoDB());


            Application.Synchronize.Api.StartUp();
            Application.Agent.Api.StartUp();
            Application.Game.Api.StartUp();
            Application.Lobby.Api.StartUp();
            Application.Match.Api.StartUp();

            try
            {
                Application.Synchronize.AdminWebService.Run();
            }
            catch
            {
                Logger.Error("AdminWebService is not working. Execute Visual Studio with Admin Account or... ");
                Logger.Error("netsh http add urlacl url = \"http://*:5281/\" user = everyone");
            }

            var list = new List<Engine.Network.Protocol.Terminal.Callback>();
            list.Add(Application.Game.Api.Command.OnCommand);
            list.Add(Application.Synchronize.Api.Command.OnCommand);
            list.Add(Application.Match.Api.Command.OnCommand);
            list.Add(Command.OnCommand);
            list.Add(Basis.Metadata.Api.OnCommand);
            list.Add(Application.Agent.Api.Command.Client.OnCommand);
            Engine.Network.Api.Terminal.Run(list).Wait();

            Engine.Database.Api.CleanUp();
            Engine.Network.Api.CleanUp();
            Engine.Framework.Api.CleanUp();

            
        }
    }
}
