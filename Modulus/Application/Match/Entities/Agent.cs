using System;
using System.Collections.Generic;
using System.Text;
using static Engine.Framework.Api;

namespace Application.Match.Entities
{
    public partial class Agent : Engine.Framework.Layer.Task
    {
        public class Layer : Engine.Framework.Layer { }
        public Agent() : base(Singleton<Layer>.Instance)
        {

        }
    }
}
