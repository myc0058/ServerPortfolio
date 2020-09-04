using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Synchronize.Entities
{
    public partial class Agent : Engine.Framework.Layer.Task
    {
        public class Layer : Engine.Framework.Layer { }
        public Agent() : base(Engine.Framework.Api.Singleton<Layer>.Instance)
        {
        }
    }
}
