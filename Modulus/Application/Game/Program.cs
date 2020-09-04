using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Engine.Database.Management;
using Engine.Framework;
using Newtonsoft.Json.Linq;
using static Engine.Framework.Api;
using NoSql = Engine.Database.Management.NoSql;

namespace Application.Game
{

    public class Program
    {
        static void Main(string[] args)
        {
            
            Application.Game.Api.StandAlone = false;
            Basis.Metadata.Api.ServerType = Schema.Protobuf.CSharp.Enums.EServer.Game;
            Schema.Protobuf.Api.StartUp();
            Basis.Metadata.Api.StartUp(args);
            Engine.Network.Api.Binder = Schema.Protobuf.Api.Bind;

            Engine.Framework.Api.Logger.Info($"StartUp Game. Version - {Path.GetFileName(Directory.GetCurrentDirectory())}");

            Engine.Framework.Api.StartUp();
            Engine.Network.Api.StartUp();
            Engine.Database.Api.StartUp();
            Engine.Database.Management.Driver.AddSession("DynamoDB", new Engine.Database.Management.Amazon.DynamoDB());


            Api.StartUp();



            //Engine.Network.Api.Terminal.Listen(new List<Engine.Network.Protocol.Terminal.Callback>() { Api.Command.OnCommand, Basis.Metadata.Api.OnCommand });
            //Engine.Network.Api.Terminal.Run(new List<Engine.Network.Protocol.Terminal.Callback>() { Api.Command.OnCommand, Basis.Metadata.Api.OnCommand }).Wait();

            
            Engine.Framework.Api.Logger.Info("----------------- Game Server CleanUp -----------------");
     

            Engine.Database.Api.CleanUp();
            Engine.Network.Api.CleanUp();
            Engine.Framework.Api.CleanUp();
        }

       
    }
}
