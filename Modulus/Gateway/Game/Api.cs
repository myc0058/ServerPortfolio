using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Game
{
    public static partial class Api
    {

        public static void StartUp()
        {
            Engine.Network.Api.Listen(5881, 128, () => {
                var link = new Engine.Network.Protocol.Gateway.Link();
            });
        }
    }
}
