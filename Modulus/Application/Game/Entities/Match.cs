using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Game.Entities
{
    public partial class Match : Engine.Framework.Layer.Task
    {
        public class Layer : Engine.Framework.Layer { }
        public Match() : base(Engine.Framework.Api.Singleton<Layer>.Instance) { }
    }
}
