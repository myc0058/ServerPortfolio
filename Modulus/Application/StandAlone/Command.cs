using Engine.Database;
using Engine.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Engine.Framework.Api;
using dbms = Engine.Database.Management;

namespace Application.StandAlone
{
    public static class Command
    {
        private static void UnitTest(string[] cmd)
        {
            if (cmd.Length < 2) { return; }

            var type = Type.GetType($"Application.StandAlone.UnitTest.{cmd[1]}");

            if (type == null)
            {
                Console.WriteLine($"Can't find Application.StandAlone.UnitTest.{cmd[1]}");
                return;
            }

            var method = type.GetMethod("UnitTest", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            if (method == null)
            {
                Console.WriteLine($"Can't find Application.StandAlone.UnitTest.{cmd[1]}.UnitTest()");
                return;
            }

            try
            {
                method.Invoke(null, null);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception Application.StandAlone.UnitTest.{cmd[1]}.UnitTest()");
                Console.WriteLine(e);
            }



        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public static async Task<bool> OnCommand(Engine.Framework.INotifier notifier, Engine.Network.Protocol.Terminal.Message msg)
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            string cmd = msg.Command;
            switch (cmd)
            {
                case string b when b.StartsWith("dc"):
                    {
                        Application.Lobby.Protocol.Client.DisconnectOne();
                    }
                    break;
                case string b when b.StartsWith("ping"):
                    {
                        var tokens = cmd.Split(' ');
                        Application.Lobby.Api.HeartBeatInterval = tokens[1].ToInt32();
                    }
                    break;
                case string b when b.StartsWith("test"):

                    Console.WriteLine($"unittest {cmd} begin");

                    UnitTest(cmd.Split(' '));

                    Console.WriteLine($"unittest {cmd} end");
                    break;
                case "exit":
                case string b when b.ToLower().StartsWith("exit"):
                    Engine.Network.Api.Terminal.Notify(notifier, "ok.");
                    Engine.Network.Api.Terminal.Exit = true;
                    break;
            }

            return false;
        }


        public static void Execute(string cmd)
        {
        }
    }
}
