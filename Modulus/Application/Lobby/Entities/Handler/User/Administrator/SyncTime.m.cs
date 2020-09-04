using System;
using Application.Common.Scheduler;
using Engine.Network.Protocol;
using static Engine.Framework.Api;

namespace Application.Lobby.Entities
{
    public partial class User
    {
        public void OnMessage(Protocol.Client notifier, global::Schema.Protobuf.Message.Administrator.SyncTime msg)
        {
            msg.RemoteTime = Singleton<SyncTime>.Instance.ServerTicks;
            notifier.Notify(msg);
        }
    }
}
