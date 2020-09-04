using Engine.Framework;
using Engine.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Engine.Framework.Api;

namespace Tool.Terminal
{
    public static partial class Api
    {
        public static partial class Command
        {
            public static long CurrentAgent = 0;

            public static string AgentToIPAddress()
            {
                if (CurrentAgent == 0)
                {
                    return "127.0.0.1@~ Modulus$ ";
                }

                var address = Engine.Framework.Api.Int64ToIPAddress(CurrentAgent);

                if (agents.TryGetValue(address, out string name) == false)
                {
                    name = "~";
                }
                // ip@name user$
                return $"{address}@{name} Modulus$ ";

            }
            public static bool Switch(Engine.Framework.INotifier notifer, Engine.Network.Protocol.Terminal.Message msg)
            {

                var tokens = msg.Command.Split(' ');
                if (tokens.Length < 2)
                {
                    return false;
                }


                tokens = tokens[1].Split(':');
                CurrentAgent = Engine.Framework.Api.AddressToInt64(tokens[0], tokens[1].ToUInt16());


                Engine.Network.Api.Terminal.CurrentTerminal = AgentToIPAddress();
                Engine.Network.Api.Terminal.Complete(notifer, "ok.");
                return true;
            }
        }

    }
    
}
