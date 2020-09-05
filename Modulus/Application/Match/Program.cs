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
            
            Schema.Protobuf.Api.StartUp();
            Engine.Network.Api.Binder = Schema.Protobuf.Api.Bind;
            
            Engine.Framework.Api.Logger.Info($"StartUp Match. Version - {Path.GetFileName(Directory.GetCurrentDirectory())}");

            Engine.Framework.Api.StartUp();
            Engine.Database.Api.StartUp();
            Engine.Network.Api.StartUp();

            Api.StartUp();

            Engine.Framework.Api.Logger.Info("----------------- Matching Server CleanUp -----------------");
            
            Engine.Database.Api.CleanUp();
            Engine.Network.Api.CleanUp();
            Engine.Framework.Api.CleanUp();
        }
    }
}
