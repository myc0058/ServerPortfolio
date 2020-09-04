using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Override.Lobby
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Schema.Protobuf.Api.StartUp();
            Basis.Metadata.Api.StartUp();
            Engine.Network.Api.Binder = Schema.Protobuf.Api.Bind;
            //Basis.Metadata.Api.StartUp();

            //System.Threading.ThreadPool.SetMinThreads(8, 8);
            //System.Threading.ThreadPool.SetMaxThreads(16, 16);

            Engine.Framework.Api.StartUp();
            Engine.Network.Api.StartUp();
            Engine.Database.Api.StartUp();

                       

            Application.Lobby.Api.StartUp();
            bool exit = false;
            while (exit == false)
            {
                var cmd = Console.ReadLine();
                //cmd = cmd.ToLower();
                switch (cmd)
                {
                    case "exit":
                        exit = true;
                        break;
                }
            }


            //Application.Lobby.Api.CleanUp();
            Engine.Database.Api.CleanUp();
            Engine.Network.Api.CleanUp();
            Engine.Framework.Api.CleanUp();
            //Basis.Metadata.Api.CleanUp();
            //Schema.Protobuf.Api.CleanUp();
        }
    }
}