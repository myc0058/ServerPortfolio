using Engine.Database;
using Engine.Framework;
using System;
using System.Threading;

namespace Application.Synchronize.Entities {
	public partial class Lobby {

        public void OnMessage(Engine.Network.Protocol.Delegator<Delegatables.Lobby>.Notifier notifier, global::Schema.Protobuf.Message.Administrator.GetUniqueKeySeed msg)
        {
            msg.Seed = Api.GetUniqueKeySeed();
            notifier.Response(msg);
        }
    }
}
