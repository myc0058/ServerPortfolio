using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Schema.Protobuf.Api.StartUp();
            Engine.Framework.Api.StartUp();
            Engine.Network.Api.StartUp();
            Engine.Database.Api.StartUp();


            var dummy = new Protocol.Dummy();
            dummy.Connect("127.0.0.1", 5882);

            while (true)
            {
                System.Console.ReadLine();
                dummy.Write(new Schema.Protobuf.Message.Authentication.Login());

                System.Console.ReadLine();
            }


        }
    }
}
