using Engine.Framework;
using dbms = Engine.Database.Management;
using System.Threading.Tasks;
using Schema.Protobuf.Message.Enums;

namespace Application.Lobby.Entities
{
    public partial class User
    {
        public void OnMessage(Protocol.Client notifier, global::Schema.Protobuf.Message.Game.RTT msg)
        {
            MyMatchingInfo.Delegator?.Delegate(UID, MyMatchingInfo.RoomID, msg);
        }

        public void OnMessage(Engine.Network.Protocol.Delegator<Delegatables.Game.Room>.Notifier notifier, global::Schema.Protobuf.Message.Game.RTT msg)
        {
            Client?.Notify(msg);
        }
    }
}
