using static Engine.Framework.Api;

namespace Application.Match.Entities
{
    public partial class Game : Engine.Framework.Layer.Task
    {
        public class Layer : Engine.Framework.Layer { }
        public Game() : base(Singleton<Layer>.Instance)
        {


        }
    }
}
