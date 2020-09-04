using static Engine.Framework.Api;

namespace Application.Match.Entities
{
    public partial class Synchronize : Engine.Framework.Layer.Task
    {
        public class Layer : Engine.Framework.Layer { }
        public Synchronize() : base(Singleton<Layer>.Instance)
        {


        }
    }
}
