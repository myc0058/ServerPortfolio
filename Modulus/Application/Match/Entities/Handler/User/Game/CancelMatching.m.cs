using Engine.Framework;
using dbms = Engine.Database.Management;
using System.Threading.Tasks;
using static Engine.Framework.Api;
using Schema.Protobuf.Message.Enums;

namespace Application.Match.Entities
{
    public partial class User
    {
        public void OnMessage(Engine.Network.Protocol.Delegator<Delegatables.Lobby.User>.Notifier notifier, global::Schema.Protobuf.Message.Game.CancelMatching msg)
        {
            Singleton<Scheduler.MatchMaker>.Instance.CancelMatching(this);
            msg.ResponseCode = ResponseCode.Success;
            notifier.Response(msg);
        }
    }
}
