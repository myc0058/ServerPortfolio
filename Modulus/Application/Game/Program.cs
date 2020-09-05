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
            Schema.Protobuf.Api.StartUp();
            Engine.Network.Api.Binder = Schema.Protobuf.Api.Bind;

            Engine.Framework.Api.Logger.Info($"StartUp Game. Version - {Path.GetFileName(Directory.GetCurrentDirectory())}");

            Engine.Framework.Api.StartUp();
            Engine.Network.Api.StartUp();
            Engine.Database.Api.StartUp();

            Api.StartUp();
            
            Engine.Framework.Api.Logger.Info("----------------- Game Server CleanUp -----------------");
     

            Engine.Database.Api.CleanUp();
            Engine.Network.Api.CleanUp();
            Engine.Framework.Api.CleanUp();
        }

       
    }
}
