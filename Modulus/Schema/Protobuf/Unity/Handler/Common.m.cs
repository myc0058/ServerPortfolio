using System;
namespace Schema.Protobuf.Message.Common {
	public sealed partial class GameResult { }
	public sealed partial class MatchingInfo { }
}
namespace Schema.Protobuf {
	public partial class Handler {
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Common.GameResult msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Common.MatchingInfo msg) {}
	}
}
