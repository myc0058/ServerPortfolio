using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Engine.Framework.Api;

namespace Application.Synchronize
{
    public static partial class Api
    {
        public static partial class Command
        {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            public static async Task<bool> OnCommand(Engine.Framework.INotifier notifer, Engine.Network.Protocol.Terminal.Message msg)
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            {
                Engine.Framework.Api.Logger.Info($"Command : {msg.Command}");
                try
                {
                    var backup = msg.Command;
                    var cmd = msg.Command.ToLower();
                    switch (cmd)
                    {
                        case string b when b.ToLower().StartsWith("exit"):
                            {
                                Engine.Network.Api.Terminal.Notify(notifer, "ok.");
                                Engine.Network.Api.Terminal.Exit = true;
                            }
                            return true;
                        default:
                            return false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return true;
                }
            }
        }

    }
}
