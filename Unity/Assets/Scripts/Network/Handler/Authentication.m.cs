using System;
namespace Schema.Protobuf.Message.Authentication {
	public sealed partial class Encript { }
	public sealed partial class Login { }
	public sealed partial class Logout { }
}
namespace Schema.Protobuf {
	public partial class Handler {
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Authentication.Encript msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Authentication.Login msg) {}
		public virtual void OnMessage(INotifier notifier, Schema.Protobuf.Message.Authentication.Logout msg) {}
	}
}
