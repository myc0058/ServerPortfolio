using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Framework;

namespace Application.Game
{
    public static partial class Api
    {
        public static partial class Command
        {
            public static async Task<bool> OnCommand(Engine.Framework.INotifier notifer, Engine.Network.Protocol.Terminal.Message msg)
            {
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
