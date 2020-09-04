using System;
namespace Schema.Protobuf.Message.Administrator {
	public sealed partial class TerminalCommand { }
	public sealed partial class ConnectedAgentInfo { }
	public sealed partial class Exit { }
	public sealed partial class GameServerState { }
}
namespace Schema.Protobuf {
	public partial class Handler {
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Administrator.TerminalCommand msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Administrator.ConnectedAgentInfo msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Administrator.Exit msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Administrator.GameServerState msg) {}
	}
}
