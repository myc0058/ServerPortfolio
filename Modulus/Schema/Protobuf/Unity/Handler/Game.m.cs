using System;
namespace Schema.Protobuf.Message.Game {
	public sealed partial class BeginMatching { }
	public sealed partial class CancelMatching { }
	public sealed partial class EndMatching { }
	public sealed partial class EnterRoom { }
	public sealed partial class MyGameResult { }
	public sealed partial class FinishGame { }
	public sealed partial class Chat { }
}
namespace Schema.Protobuf {
	public partial class Handler {
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Game.BeginMatching msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Game.CancelMatching msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Game.EndMatching msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Game.EnterRoom msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Game.MyGameResult msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Game.FinishGame msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Game.Chat msg) {}
	}
}
