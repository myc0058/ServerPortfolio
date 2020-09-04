using System;
namespace Schema.Protobuf.Message.Game {
	public sealed partial class BeginMatching { }
	public sealed partial class CancelMatching { }
	public sealed partial class EndMatching { }
	public sealed partial class EnterRoom { }
	public sealed partial class LeaveRoom { }
	public sealed partial class EnterCharacter { }
	public sealed partial class LeaveCharacter { }
	public sealed partial class Damage { }
	public sealed partial class Die { }
	public sealed partial class MyGameResult { }
	public sealed partial class FinishGame { }
	public sealed partial class Chat { }
	public sealed partial class SyncTime { }
	public sealed partial class Move { }
	public sealed partial class RTT { }
	public sealed partial class World { }
}
namespace Schema.Protobuf {
	public partial class Handler {
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Game.BeginMatching msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Game.CancelMatching msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Game.EndMatching msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Game.EnterRoom msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Game.LeaveRoom msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Game.EnterCharacter msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Game.LeaveCharacter msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Game.Damage msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Game.Die msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Game.MyGameResult msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Game.FinishGame msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Game.Chat msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Game.SyncTime msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Game.Move msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Game.RTT msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Game.World msg) {}
	}
}
