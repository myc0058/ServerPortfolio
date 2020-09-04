using System;
namespace Schema.Protobuf.Message.Lobby {
	public sealed partial class Logout { }
}
namespace Schema.Protobuf {
	public partial class Handler {
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Lobby.Logout msg) {}
	}
}
