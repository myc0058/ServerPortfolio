using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Match
{
    public static partial class Api
    {
        public static partial class Command
        {
            public static async Task<bool> OnCommand(Engine.Framework.INotifier notifer, Engine.Network.Protocol.Terminal.Message msg)
            {
                if (string.IsNullOrEmpty(msg.Command)) { return true; }

                var tokens = msg.Command.Split(' ');

                switch (msg.Command.ToLower())
                {
                    default:
                        return false;
                }
            }

        }
    }

}
