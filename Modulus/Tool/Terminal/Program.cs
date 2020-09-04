using Engine.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Engine.Framework.Api;

namespace Tool.Terminal
{
    class Program
    {

        public static bool Exit = false;
        static void Main(string[] args)
        {
            Engine.Framework.Api.StartUp();
            Engine.Network.Api.StartUp();

            Singleton<Engine.Network.Protocol.Terminal>.Instance.OnCommand = Api.Command.OnMessage;
            Singleton<Engine.Network.Protocol.Terminal>.Instance.OnMessage = Api.Command.OnMessage;
            Singleton<Engine.Network.Protocol.Terminal>.Instance.OnConnect = () => {

                //long address = Singleton<Engine.Network.Protocol.Terminal>.Instance.RemoteEndPoint.ToInt64Address();
                //var ip = Engine.Framework.Api.Int64ToIPAddress(address);
                //Engine.Network.Api.Terminal.CurrentTerminal = $"{ip}@Agent Modulus$ ";

            };
            Singleton<Engine.Network.Protocol.Terminal>.Instance.Connect("Agent", "52.231.88.191", 7881);
            //Singleton<Engine.Network.Protocol.Terminal>.Instance.Connect("Agent", "127.0.0.1", 7881);

            Console.WriteLine("/************************************/");
            Console.WriteLine("/*         궁금하면 help or ?        */");
            Console.WriteLine("/************************************/");

            var list = new List<Engine.Network.Protocol.Terminal.Callback>();
            list.Add(Api.Command.OnCommand);

            Engine.Network.Api.Terminal.Run(list).Wait();


        }
    }
}
