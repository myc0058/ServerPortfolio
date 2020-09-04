using Engine.Framework;
using dbms = Engine.Database.Management;
using System.Threading.Tasks;
using Schema.Protobuf.Message.Enums;

namespace Application.Lobby.Entities
{
    public partial class User
    {
        public void OnMessage(Engine.Network.Protocol.Delegator<Delegatables.Game.Room>.Notifier notifier, global::Schema.Protobuf.Message.Game.World msg)
        {
            Client?.Notify(msg);
        }
    }
}
