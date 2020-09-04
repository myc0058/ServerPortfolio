using Engine.Framework;
using dbms = Engine.Database.Management;
using System.Threading.Tasks;
using Schema.Protobuf.Message.Enums;
using static Engine.Framework.Api;

namespace Application.Match.Entities
{
    public partial class User
    {
        public void OnMessage(Engine.Network.Protocol.Delegator<Delegatables.Lobby.User>.Notifier notifier, global::Schema.Protobuf.Message.Game.BeginMatching msg)
        {
            try
            {
                if (Singleton<Scheduler.MatchMaker>.Instance.BeginMatching(this) == true)
                {
                    msg.ResponseCode = ResponseCode.Success;
                }
                else
                {
                    msg.ResponseCode = ResponseCode.Fail;
                }
            }
            catch
            {
                msg.ResponseCode = ResponseCode.Fail;
            }
            notifier.Response(msg);
        }
    }
}
