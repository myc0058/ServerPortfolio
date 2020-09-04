using Engine.Database;
using Engine.Framework;
using System;
using dbms = Engine.Database.Management;

namespace Application.Synchronize.Entities {
	public partial class Match {

        public void OnMessage(Engine.Network.Protocol.Delegator<Delegatables.Match>.Notifier notifier, global::Schema.Protobuf.Message.Administrator.SyncTime msg)
        {
            msg.RemoteTime = DateTime.UtcNow.Ticks;
            notifier.Notify(msg);
            //Engine.Framework.Api.Logger.Info("Synchronize Recv SyncTime");
        }
    }
}
