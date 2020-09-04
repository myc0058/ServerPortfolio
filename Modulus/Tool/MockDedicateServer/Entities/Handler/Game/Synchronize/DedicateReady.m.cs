using System;

namespace Tool.MockDedicateServer.Entities {
	public partial class Game {
		public void OnMessage(Engine.Framework.INotifier notifier, global::Schema.Protobuf.Message.Synchronize.DedicateReady msg) {
            LastReceivedPacket[Engine.Framework.Id<Schema.Protobuf.Message.Synchronize.DedicateReady>.Value] = msg;
		}
	}
}
