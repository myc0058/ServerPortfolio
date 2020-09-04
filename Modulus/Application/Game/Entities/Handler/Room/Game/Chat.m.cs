using Engine.Framework;
using dbms = Engine.Database.Management;
using System.Threading.Tasks;
using Schema.Protobuf.Message.Enums;

namespace Application.Game.Entities
{
    public partial class Room
    {
        public void OnMessage(Engine.Network.Protocol.Delegator<Delegatables.Lobby.User>.Notifier notifier, global::Schema.Protobuf.Message.Game.Chat msg)
        {
            foreach(var item in Users)
            {
                item.Value.Delegator?.Delegate(UID, item.Key, msg);
            }
        }
    }
}
