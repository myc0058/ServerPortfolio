using System;
using Engine.Network.Protocol;

namespace Application.Lobby.Entities
{
    public partial class User
    {
        public void OnMessage(Protocol.Client notifier, global::Schema.Protobuf.Message.Lobby.Logout msg)
        {
            msg.Idx = Account.Idx;
            if (MyMatchingInfo.RoomID > 0)
            {
                MyMatchingInfo.Delegator?.Delegate(UID, MyMatchingInfo.RoomID, msg);
                Delegatables.Match.User.Instance.Delegate(UID, UID, msg);
                MyMatchingInfo.Clear();
            }
            else
            {
                notifier.Response(msg);
            }

            Engine.Framework.Api.Logger.Info($"Logout Id : {Account.Id}");
        }

        public void OnMessage(Delegator<Delegatables.Game.Room>.Notifier notifier, global::Schema.Protobuf.Message.Lobby.Logout msg)
        {
            if (MyMatchingInfo.RoomID > 0)
            {
                MyMatchingInfo.Clear();
                Client?.Notify(msg);
            }
            else
            {
                Client?.Notify(msg);
            }

            Engine.Framework.Api.Logger.Info($"Logout Id : {Account.Id}");
        }
    }
}
