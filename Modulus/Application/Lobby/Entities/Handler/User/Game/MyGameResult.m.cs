using Engine.Framework;
using dbms = Engine.Database.Management;
using System.Threading.Tasks;

namespace Application.Lobby.Entities
{
    public partial class User
    {
        public void OnMessage(Engine.Network.Protocol.Delegator<Delegatables.Game.Room>.Notifier notifier, global::Schema.Protobuf.Message.Game.MyGameResult msg)
        {
            MyMatchingInfo.Delegator = null;
            MyMatchingInfo.RoomID = 0;

            Client?.Notify(msg);
        }
    }
}
