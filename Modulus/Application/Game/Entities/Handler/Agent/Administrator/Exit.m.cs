using System;
using System.Threading.Tasks;

namespace Application.Game.Entities
{
    public partial class Agent
    {
        public void OnMessage(Engine.Framework.INotifier notifier, global::Schema.Protobuf.Message.Administrator.Exit msg)
        {
            Environment.Exit(0);
        }
    }
}
