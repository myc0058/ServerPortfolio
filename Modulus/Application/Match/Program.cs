using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Engine.Framework.Api;
using dbms = Engine.Database.Management;

namespace Application.Match
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            //List<Entities.User> users = new List<Entities.User>();
            //for (int i = 0; i < 10; ++i)
            //{
            //    var user = new Entities.User();
            //    user.Group = i % 3;

            //    user.UID = i;
            //    users.Add(user);
            //}

            //Application.Match.Scheduler.MatchMakers.Trio.Add(null, users);


            
            Schema.Protobuf.Api.StartUp();
            Engine.Network.Api.Binder = Schema.Protobuf.Api.Bind;
            Basis.Metadata.Api.StartUp(args);

            Engine.Framework.Api.Logger.Info($"StartUp Match. Version - {Path.GetFileName(Directory.GetCurrentDirectory())}");

            Engine.Framework.Api.StartUp();
            Engine.Database.Api.StartUp();
            Engine.Network.Api.StartUp();
            Engine.Database.Management.Driver.AddSession("DynamoDB", new Engine.Database.Management.Amazon.DynamoDB());


            Api.StartUp();

            Engine.Framework.Api.Logger.Info("----------------- Matching Server CleanUp -----------------");
            //Engine.Network.Api.Terminal.Listen(new List<Engine.Network.Protocol.Terminal.Callback>() { Api.Command.OnCommand });
            //Engine.Network.Api.Terminal.Run(new List<Engine.Network.Protocol.Terminal.Callback>() { Api.Command.OnCommand }).Wait();

         
            Engine.Database.Api.CleanUp();
            Engine.Network.Api.CleanUp();
            Engine.Framework.Api.CleanUp();
        }
    }
}
