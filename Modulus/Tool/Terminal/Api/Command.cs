using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Engine.Framework.Api;

namespace Tool.Terminal
{
    public static partial class Api
    {

        public static Engine.Network.Protocol.Terminal Current = null;
        public static partial class Command
        {
            private static bool watchAgents { get; set; } = false;
            static ConcurrentDictionary<string, string> agents = new ConcurrentDictionary<string, string>();
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            public static async Task<bool> OnMessage(Engine.Framework.INotifier notifer, Engine.Network.Protocol.Terminal.Message msg)
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            {

                if (msg.Type == Engine.Network.Protocol.Terminal.Message.EType.Complete || msg.Type == Engine.Network.Protocol.Terminal.Message.EType.Error)
                {
                    watchAgents = false;

                    if (msg.Type == Engine.Network.Protocol.Terminal.Message.EType.Error)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Engine.Framework.Api.Logger.Info(msg.Command);
                        Console.ResetColor();
                    }
                    else
                    {
                        Engine.Framework.Api.Logger.Info(msg.Command);
                    }
                    
                    Engine.Network.Api.Terminal.IsCommandable = DateTime.MinValue;
                    return true;
                }

                if (watchAgents == true)
                {
                    var tokens = msg.Command.Split('$');
                    if (tokens != null && tokens.Length > 1)
                    {
                        tokens = tokens[0].Split('@');
                        if (tokens != null && tokens.Length == 2)
                        {
                            agents.TryAdd(tokens[0], tokens[1].Split(' ')[0]);
                            Engine.Network.Api.Terminal.CurrentTerminal = AgentToIPAddress();
                        }
                    }

                    tokens = msg.Command.Split('#');
                    if (tokens != null && tokens.Length > 1)
                    {
                        Console.Write(tokens[0]);

                        if (tokens[1].StartsWith("On") == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        
                        Console.Write(tokens[1]);

                        if (tokens[2] == "N/A")
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Engine.Framework.Api.Logger.Info(tokens[2]);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Engine.Framework.Api.Logger.Info(tokens[2]);
                        }
                        Console.ResetColor();

                        return true;
                    }
                }
                Engine.Network.Api.Terminal.IsCommandable = DateTime.UtcNow.AddSeconds(15);
                if (msg.NewLine == true)
                {
                    Engine.Framework.Api.Logger.Info(msg.Command);
                }
                else
                {
                    Console.Write(msg.Command);
                }
                return true;
            }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            public static async Task<bool> OnCommand(Engine.Framework.INotifier notifer, Engine.Network.Protocol.Terminal.Message msg)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
            {
                if (string.IsNullOrEmpty(msg.Command)) { return true; }

                var tokens = msg.Command.Split(' ');

                var cmd = msg.Command.ToLower();

                switch (cmd)
                {
                    case string b when b.StartsWith("switch"):
                        return Switch(notifer, msg);

                    case string b when b.StartsWith("exit"):
                        {
                            Engine.Network.Api.Terminal.Exit = true;
                        }
                        return true;
                    case string b when b.StartsWith("help"):
                    case string a when a.StartsWith("?"):
                        Help(notifer, msg);
                        return true;
                    default:
                        return false;
                }
            }
        }
        
    }
    
}
