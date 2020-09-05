using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Application.Lobby
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            Application.Lobby.Api.StandAlone = false;
            Schema.Protobuf.Api.StartUp();
            
            Console.WriteLine($"StartUp Lobby. Version - {Path.GetFileName(Directory.GetCurrentDirectory())}");

            Engine.Network.Api.Binder = Schema.Protobuf.Api.Bind;

            Engine.Framework.Api.StartUp();
            Engine.Network.Api.StartUp();

            Engine.Database.Api.StartUp();
            Engine.Database.Management.Driver.AddSession("DynamoDB", new Engine.Database.Management.Amazon.DynamoDB());

            Api.StartUp();

            bool exit = false;
            while (exit == false)
            {
                var cmd = Console.ReadLine();
                switch (cmd)
                {
                    case "exit":
                        Delegatables.Synchronize.Instance.Delegate(Api.Idx, Api.Idx, new Schema.Protobuf.Message.Authentication.Logout());
                        System.Threading.Thread.Sleep(3000);
                        exit = true;
                        break;
                }
            }


            Engine.Database.Api.CleanUp();
            Engine.Network.Api.CleanUp();
            Engine.Framework.Api.CleanUp();
        }
    }
}
