using System;
namespace Schema.Protobuf.Message.Common {
	public sealed partial class GameResult { }
	public sealed partial class MatchingInfo { }
	public sealed partial class Vector2 { }
	public sealed partial class Vector3 { }
	public sealed partial class Character { }
	public sealed partial class PlaneMap { }
}
namespace Schema.Protobuf {
	public partial class Handler {
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Common.GameResult msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Common.MatchingInfo msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Common.Vector2 msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Common.Vector3 msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Common.Character msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Common.PlaneMap msg) {}
	}
}
