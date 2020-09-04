using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Engine.Framework.Api;
using static Engine.Network.Protocol.Terminal;

namespace Engine.Network
{
    public static partial class Api
    {
        public static partial class Terminal
        {
            
            public static void Notify(Engine.Framework.INotifier notifier, string message, bool newLine = true)
            {
                if (notifier == null) { return; }
                Protocol.Terminal.Message msg = new Protocol.Terminal.Message();
                msg.Type = Protocol.Terminal.Message.EType.Notify;

                msg.NewLine = newLine;
                msg.Command = message;

                notifier.Notify(msg);
            }

            public static void Complete(Engine.Framework.INotifier notifier, string message)
            {
                if (notifier == null) { return; }
                Protocol.Terminal.Message msg = new Protocol.Terminal.Message();
                msg.Type = Protocol.Terminal.Message.EType.Complete;
                msg.NewLine = true;
                msg.Command = message;
                notifier.Notify(msg);
            }

            public static void Error(Engine.Framework.INotifier notifier, string message, bool newLine = true)
            {
                if (notifier == null) { return; }
                Protocol.Terminal.Message msg = new Protocol.Terminal.Message();
                msg.Type = Protocol.Terminal.Message.EType.Error;

                msg.NewLine = newLine;
                msg.Command = message;

                notifier.Notify(msg);
            }

            public static void Command(Engine.Framework.INotifier notifier, string message)
            {
                if (notifier == null) { return; }
                Protocol.Terminal.Message msg = new Protocol.Terminal.Message();
                msg.Type = Protocol.Terminal.Message.EType.Command;
                msg.Command = message;
                notifier.Notify(msg);
            }

            public static void Command(Protocol.Terminal to, string message)
            {
                if (to == null) { return; }
                Protocol.Terminal.Message msg = new Protocol.Terminal.Message();
                msg.Type = Protocol.Terminal.Message.EType.Command;
                msg.Command = message;
                to.Notify(msg);
            }



            public static string CurrentTerminal = "127.0.0.1@~ standalone ";
            public static DateTime IsCommandable { get; set; } = DateTime.MinValue;
            public static bool Service { get; set; } = false;

            public static bool Exit = false;

            public static void Listen(List<Callback> commands, Callback message = null, ushort port = 5882)
            {
                async Task<bool> ProcessCommand(Engine.Framework.INotifier notifier, Protocol.Terminal.Message msg)
                {
                    try
                    {
                        foreach (var e in commands)
                        {
                            var ret = await e.Invoke(notifier, msg);
                            if (ret == true) { return true; }
                        }

                        Error(notifier, $"Unknown command '{msg.Command}'");

                    }
                    catch (Exception e)
                    {
                        Notify(notifier, "catch Exception");
                        Error(notifier, e.Message);
                        return false;
                    }
                    return false;
                }

                Engine.Network.Api.Listen(port, () =>
                {
                    var tcp = new Protocol.Terminal();
                    tcp.OnCommand = ProcessCommand;
                    tcp.OnMessage = message;
                    tcp.Accept(port);
                });
            }

            public static async Task Run(List<Callback> callbacks)
            {
                async Task<bool> ProcessCommand(Engine.Framework.INotifier notifier, Protocol.Terminal.Message msg)
                {
                    try
                    {
                        foreach (var e in callbacks)
                        {
                            var ret = await e.Invoke(notifier, msg);
                            if (ret == true) { return true; }
                        }

                        Error(notifier, $"Unknown command '{msg.Command}'");

                    }
                    catch (Exception e)
                    {
                        Notify(notifier, "catch Exception");
                        Error(notifier, e.Message);
                        return false;
                    }
                    return false;
                }

                await Task.Run(async () => {

                    while (Exit == false)
                    {
                        if (Service == true) { System.Threading.Thread.Sleep(100000); continue; }
                        if (Api.Terminal.IsCommandable > DateTime.UtcNow)
                        {
                            System.Threading.Thread.Sleep(1);
                            continue;
                        }

                        var tokens = Api.Terminal.CurrentTerminal.Split('@');
                        if (tokens != null && tokens.Length > 1)
                        {
                            Console.Write(tokens[0]);
                            Console.Write("@");
                            
                            tokens = tokens[1].Split(' ');

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(tokens[0]);
                            Console.Write(" ");
                            Console.ResetColor();
                            Console.Write(tokens[1]);
                            Console.Write(" ");
                        }
                        else
                        {
                            Console.Write(Api.Terminal.CurrentTerminal);
                        }
                        
                        var cmd = Console.ReadLine();

                        if (string.IsNullOrEmpty(cmd)) {
                            continue;
                        }

                        if (cmd.ToLower() == "quit")
                        {
                            break;
                        }

                        try
                        {
                            var msg = new Protocol.Terminal.Message();
                            msg.Type = Protocol.Terminal.Message.EType.Command;
                            msg.Command = cmd;
                            await ProcessCommand(Singleton<ConsoleNotifier>.Instance, msg);
                        }
                        catch
                        {

                        }
                    }

                });
            }

          
        }
    }
}
