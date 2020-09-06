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

            Application.Lobby.Api.HeartBeatInterval = 600;
            Schema.Protobuf.Api.StartUp();

            Engine.Network.Api.Binder = Schema.Protobuf.Api.Bind;

            Engine.Network.Api.DelegatorHeartBeat = false;
            Engine.Framework.Api.StartUp();
            Engine.Network.Api.StartUp();
            Engine.Database.Api.StartUp();
            
            Application.Synchronize.Api.StartUp();
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
            Engine.Network.Api.Terminal.Run(list).Wait();

            Engine.Database.Api.CleanUp();
            Engine.Network.Api.CleanUp();
            Engine.Framework.Api.CleanUp();

            
        }
    }
}
