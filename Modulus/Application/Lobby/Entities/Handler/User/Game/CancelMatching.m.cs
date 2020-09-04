using Engine.Framework;
using dbms = Engine.Database.Management;
using System.Threading.Tasks;

namespace Application.Lobby.Entities
{
    public partial class User
    {
        public void OnMessage(Protocol.Client notifier, global::Schema.Protobuf.Message.Game.CancelMatching msg)
        {
        }
    }
}
