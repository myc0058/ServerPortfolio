using System;
using System.IO;

namespace Application.Lobby.Entities {
    public partial class User
    {
        public void OnMessage(Protocol.Client notifier, global::Schema.Protobuf.Message.Authentication.Logout msg)
        {
            msg.Idx = Account.Idx;

            Engine.Framework.Api.Logger.Info($"Logout Id : {Account.Id}");

            notifier.Response(msg);
            Delegatables.Synchronize.User.Instance.Delegate(UID, UID, msg);
            Entities.User.Remove(this);
        }

        public void OnMessage(Engine.Network.Protocol.Delegator<Delegatables.Synchronize.User>.Notifier notifier, global::Schema.Protobuf.Message.Authentication.Logout msg)
        {
            Engine.Framework.Api.Logger.Info($"Logout Id : {Account.Id}");
            
            //Client.Disconnect();
            Entities.User.Remove(this);
            Client?.Notify(msg);
        }
    }
}
