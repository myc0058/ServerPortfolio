using Engine.Database;
using Engine.Framework;
using System;


namespace Application.Synchronize.Entities {
	public partial class Game {

        public void OnMessage(Engine.Network.Protocol.Delegator<Delegatables.Game>.Notifier notifier, global::Schema.Protobuf.Message.Administrator.GetUniqueKeySeed msg)
        {
            msg.Seed = Api.GetUniqueKeySeed();
            notifier.Response(msg);
        }
    }
}
