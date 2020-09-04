using System;
using System.Collections.Generic;
using System.Text;
using static Engine.Framework.Api;

namespace Application.Lobby.Entities
{
    public class Agent : Engine.Framework.Layer.Task
    {
        public class Layer : Engine.Framework.Layer { }
        public Agent() : base(Singleton<Layer>.Instance)
        {

        }
    }
}
