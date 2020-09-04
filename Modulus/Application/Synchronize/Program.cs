using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Synchronize
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.Synchronize.Api.StandAlone = false;
            Schema.Protobuf.Api.StartUp();
            Basis.Metadata.Api.StartUp(args);
            Engine.Network.Api.Binder = Schema.Protobuf.Api.Bind;

            Engine.Framework.Api.StartUp();
            Engine.Network.Api.StartUp();
            Engine.Database.Api.StartUp();
            Engine.Database.Management.Driver.AddSession("DynamoDB", new Engine.Database.Management.Amazon.DynamoDB());
            Api.StartUp();

            var list = new List<Engine.Network.Protocol.Terminal.Callback>();
            //list.Add(Api.Command.OnCommand);

            //Engine.Network.Api.Terminal.Listen(new List<Engine.Network.Protocol.Terminal.Callback>() { Api.Command.OnCommand });
            Engine.Network.Api.Terminal.Run(list).Wait();

            Console.WriteLine("----------------- Synchronize Server CleanUp -----------------");
            Engine.Database.Api.CleanUp();
            Engine.Network.Api.CleanUp();
            Engine.Framework.Api.CleanUp();

        }
    }
}
