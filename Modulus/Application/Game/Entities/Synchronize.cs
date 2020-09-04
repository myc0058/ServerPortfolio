using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Game.Entities
{
    public partial class Synchronize : Engine.Framework.Layer.Task
    {
        public class Layer : Engine.Framework.Layer { }
        public Synchronize() : base(Engine.Framework.Api.Singleton<Layer>.Instance) { }
    }
}
