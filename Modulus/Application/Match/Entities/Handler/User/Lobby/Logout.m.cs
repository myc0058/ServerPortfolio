using System;

namespace Application.Match.Entities
{
    public partial class User
    {
        public void OnMessage(Engine.Network.Protocol.Delegator<Delegatables.Lobby.User>.Notifier notifier, global::Schema.Protobuf.Message.Lobby.Logout msg)
        {
            //CancelMatch();
        }
    }
}
