using Engine.Framework;
using dbms = Engine.Database.Management;
using System.Threading.Tasks;

namespace Application.Lobby.Entities
{
    public partial class User
    {
        public void OnMessage(Protocol.Client notifier, global::Schema.Protobuf.Message.Game.EnterRoom msg)
        {
            if (MyMatchingInfo.RoomID > 0)
            {
                msg.Idx = Account.Idx;
                msg.GameServerId = MyMatchingInfo.GameServerID;
                msg.RoomId = MyMatchingInfo.RoomID;

                MyMatchingInfo.Delegator?.Delegate(this, MyMatchingInfo.RoomID, msg, (ret) =>
                {
                    MyMatchingInfo.EnterRoom = true;
                    notifier.Response(ret);
                },
                () => {
                    MyMatchingInfo.EnterRoom = false;
                    msg.ResponseCode = Schema.Protobuf.Message.Enums.ResponseCode.Fail;
                    notifier.Response(msg);
                });

            }


        }
    }
}
