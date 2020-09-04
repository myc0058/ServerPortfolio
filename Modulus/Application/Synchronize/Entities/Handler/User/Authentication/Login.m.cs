using System;

namespace Application.Synchronize.Entities {
	public partial class User {
		public void OnMessage(Engine.Network.Protocol.Delegator<Delegatables.Lobby.User>.Notifier notifier, global::Schema.Protobuf.Message.Authentication.Login msg) {
        }
	}
}
