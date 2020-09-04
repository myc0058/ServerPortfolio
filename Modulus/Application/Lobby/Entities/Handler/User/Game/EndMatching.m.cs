using Engine.Framework;
using dbms = Engine.Database.Management;
using System.Threading.Tasks;

namespace Application.Lobby.Entities
{
    public partial class User
    {
        public void OnMessage(Engine.Network.Protocol.Delegator<Delegatables.Match.User>.Notifier notifier, global::Schema.Protobuf.Message.Game.EndMatching msg)
        {

            MyMatchingInfo.Delegator = Engine.Network.Protocol.Delegator<Delegatables.Game.Room>.Get(msg.MatchingInfo.LobbyDelegatorID);
            MyMatchingInfo.GameServerID = msg.MatchingInfo.GameServerId;
            MyMatchingInfo.RoomID = msg.MatchingInfo.RoomId;
            Client?.Notify(msg);
        }
    }
}
