using System;
namespace Schema.Protobuf {

	public interface INotifier
	{
		void Response<T>(T msg) where T : global::Google.Protobuf.IMessage;
		void Notify<T>(T msg) where T : global::Google.Protobuf.IMessage;
	}
	public delegate void Callback();
	public static partial class Api {
		public static Handler HandlerInstance = null;

		public static class Id<T> { public static int Value; }
		private delegate Callback Binder(INotifier notifier, global::System.IO.Stream stream);
		private static global::System.Collections.Generic.Dictionary<int, Binder> Binders = new global::System.Collections.Generic.Dictionary<int, Binder>();
		static public void StartUp() {
			Id<Schema.Protobuf.Message.Administrator.TerminalCommand>.Value = 0x00010001;  // 65537
			Id<Schema.Protobuf.Message.Administrator.ConnectedAgentInfo>.Value = 0x00010002;  // 65538
			Id<Schema.Protobuf.Message.Administrator.Exit>.Value = 0x00010003;  // 65539
			Id<Schema.Protobuf.Message.Administrator.GameServerState>.Value = 0x00010004;  // 65540
			Id<Schema.Protobuf.Message.Authentication.Encript>.Value = 0x00020001;  // 131073
			Id<Schema.Protobuf.Message.Authentication.Login>.Value = 0x00020002;  // 131074
			Id<Schema.Protobuf.Message.Authentication.Logout>.Value = 0x00020003;  // 131075
			Id<Schema.Protobuf.Message.Common.GameResult>.Value = 0x00030001;  // 196609
			Id<Schema.Protobuf.Message.Common.MatchingInfo>.Value = 0x00030002;  // 196610
			Id<Schema.Protobuf.Message.Game.BeginMatching>.Value = 0x00050001;  // 327681
			Id<Schema.Protobuf.Message.Game.CancelMatching>.Value = 0x00050002;  // 327682
			Id<Schema.Protobuf.Message.Game.EndMatching>.Value = 0x00050003;  // 327683
			Id<Schema.Protobuf.Message.Game.EnterRoom>.Value = 0x00050004;  // 327684
			Id<Schema.Protobuf.Message.Game.MyGameResult>.Value = 0x00050005;  // 327685
			Id<Schema.Protobuf.Message.Game.FinishGame>.Value = 0x00050006;  // 327686
			Id<Schema.Protobuf.Message.Game.Chat>.Value = 0x00050007;  // 327687
			Id<Schema.Protobuf.Message.Lobby.Logout>.Value = 0x00060001;  // 393217

			Binders.Add(65537, (notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Administrator.TerminalCommand.Parser.ParseFrom(stream);
				return () => { HandlerInstance.OnMessage(notifier, msg); };
			});
			Binders.Add(65538, (notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Administrator.ConnectedAgentInfo.Parser.ParseFrom(stream);
				return () => { HandlerInstance.OnMessage(notifier, msg); };
			});
			Binders.Add(65539, (notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Administrator.Exit.Parser.ParseFrom(stream);
				return () => { HandlerInstance.OnMessage(notifier, msg); };
			});
			Binders.Add(65540, (notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Administrator.GameServerState.Parser.ParseFrom(stream);
				return () => { HandlerInstance.OnMessage(notifier, msg); };
			});
			Binders.Add(131073, (notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Authentication.Encript.Parser.ParseFrom(stream);
				return () => { HandlerInstance.OnMessage(notifier, msg); };
			});
			Binders.Add(131074, (notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Authentication.Login.Parser.ParseFrom(stream);
				return () => { HandlerInstance.OnMessage(notifier, msg); };
			});
			Binders.Add(131075, (notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Authentication.Logout.Parser.ParseFrom(stream);
				return () => { HandlerInstance.OnMessage(notifier, msg); };
			});
			Binders.Add(196609, (notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Common.GameResult.Parser.ParseFrom(stream);
				return () => { HandlerInstance.OnMessage(notifier, msg); };
			});
			Binders.Add(196610, (notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Common.MatchingInfo.Parser.ParseFrom(stream);
				return () => { HandlerInstance.OnMessage(notifier, msg); };
			});
			Binders.Add(327681, (notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.BeginMatching.Parser.ParseFrom(stream);
				return () => { HandlerInstance.OnMessage(notifier, msg); };
			});
			Binders.Add(327682, (notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.CancelMatching.Parser.ParseFrom(stream);
				return () => { HandlerInstance.OnMessage(notifier, msg); };
			});
			Binders.Add(327683, (notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.EndMatching.Parser.ParseFrom(stream);
				return () => { HandlerInstance.OnMessage(notifier, msg); };
			});
			Binders.Add(327684, (notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.EnterRoom.Parser.ParseFrom(stream);
				return () => { HandlerInstance.OnMessage(notifier, msg); };
			});
			Binders.Add(327685, (notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.MyGameResult.Parser.ParseFrom(stream);
				return () => { HandlerInstance.OnMessage(notifier, msg); };
			});
			Binders.Add(327686, (notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.FinishGame.Parser.ParseFrom(stream);
				return () => { HandlerInstance.OnMessage(notifier, msg); };
			});
			Binders.Add(327687, (notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.Chat.Parser.ParseFrom(stream);
				return () => { HandlerInstance.OnMessage(notifier, msg); };
			});
			Binders.Add(393217, (notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Lobby.Logout.Parser.ParseFrom(stream);
				return () => { HandlerInstance.OnMessage(notifier, msg); };
			});
		}

		public static Callback Bind(INotifier notifier, int code, global::System.IO.Stream stream) {

			Binder binder = null;
			if (Binders.TryGetValue(code, out binder) == false) return () => { };

			return binder(notifier, stream);

		}
	}
}
