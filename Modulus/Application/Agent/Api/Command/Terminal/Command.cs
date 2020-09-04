using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Agent
{
    public static partial class Api
    {
        public static partial class Command
        {
            public static partial class Terminal
            {
                public static async Task<bool> OnCommand(Engine.Framework.INotifier notifer, Engine.Network.Protocol.Terminal.Message msg)
                {
                    switch (msg.Command.ToLower())
                    {
                        default:
                            return false;
                    }
                }
            }

        }
    }


}