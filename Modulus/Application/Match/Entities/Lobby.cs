using Engine.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Engine.Framework.Api;

namespace Application.Match.Entities
{
    public partial class Lobby : Engine.Framework.Layer.Task
    {
        public class Layer : Engine.Framework.Layer { }
        public Lobby() : base(Singleton<Layer>.Instance)
        {


        }
    }
}
