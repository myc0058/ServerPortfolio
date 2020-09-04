using System;
using static Tool.MockDedicateServer.Protocol.Client;

namespace Tool.MockDedicateServer.Entities {
	public partial class Game {
		public void OnMessage(Engine.Framework.INotifier notifier, global::Schema.Protobuf.Message.Synchronize.ForceEnd msg) {
            LastReceivedPacket[Engine.Framework.Id<Schema.Protobuf.Message.Synchronize.ForceEnd>.Value] = msg;
            WaitingResponses[DedicateState.Result] = false;
		}
	}
}
