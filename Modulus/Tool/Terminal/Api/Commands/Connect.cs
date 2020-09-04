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
            public static bool Connect(Engine.Framework.INotifier notifer, Engine.Network.Protocol.Terminal.Message msg)
            {

                var tokens = msg.Command.Split(' ');
                if (tokens.Length < 2)
                {
                    return false;
                }
           
                Engine.Network.Api.Terminal.Complete(notifer, "ok.");
                return true;
            }
        }

    }

}
