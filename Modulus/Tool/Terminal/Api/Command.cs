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
            public static async Task<bool> OnMessage(Engine.Framework.INotifier notifer, Engine.Network.Protocol.Terminal.Message msg)
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

            public static async Task<bool> OnCommand(Engine.Framework.INotifier notifer, Engine.Network.Protocol.Terminal.Message msg)
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

                        if (cmd.StartsWith("agents"))
                        {
                            watchAgents = true;
                            agents.Clear();
                        }
                        msg.Address = CurrentAgent;
                        msg.UID = Engine.Framework.Api.UniqueKey;
                        Singleton<Engine.Network.Protocol.Terminal>.Instance.Notify(msg);
                        Engine.Framework.Api.Logger.Info("");
                        Engine.Network.Api.Terminal.IsCommandable = DateTime.UtcNow.AddSeconds(10);

                        //if (Current == null)
                        //{
                        //    Engine.Framework.Api.Logger.Info("CurrentTerminal is null");
                        //    return true;
                        //}

                        //if (Current.IsConnected == false)
                        //{
                        //    Engine.Framework.Api.Logger.Info("Not Connected");
                        //    return true;
                        //}

                        //Engine.Network.Api.Terminal.IsCommand = false;
                        //Current.Notify(msg);
                        return true;
                }
                return false;
            }
        }
        
    }
    
}
