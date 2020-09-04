using Engine.Database;
using Engine.Framework;
using System;
using dbms = Engine.Database.Management;

namespace Application.Synchronize.Entities {
	public partial class Lobby {

        public void OnMessage(Engine.Network.Protocol.Delegator<Delegatables.Lobby>.Notifier notifier, global::Schema.Protobuf.Message.Administrator.SyncTime msg)
        {
            msg.RemoteTime = DateTime.UtcNow.Ticks;
            notifier.Notify(msg);
        }
    }
}
