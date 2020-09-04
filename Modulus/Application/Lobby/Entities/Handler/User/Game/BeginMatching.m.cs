using Engine.Framework;
using dbms = Engine.Database.Management;
using System.Threading.Tasks;
using Schema.Protobuf.Message.Enums;

namespace Application.Lobby.Entities
{
    public partial class User
    {
        public void OnMessage(Protocol.Client notifier, global::Schema.Protobuf.Message.Game.BeginMatching msg)
        {
            Delegatables.Match.User.Instance.Delegate(this, UID, msg, (ret) =>
            {
                notifier.Response(ret);
            },
            () => {
                msg.ResponseCode = ResponseCode.Fail;
                notifier.Response(msg);
            });
        }
    }
}
