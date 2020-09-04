using System;
namespace Schema.Protobuf {

	public interface INotifier
	{
		void Response<T>(T msg) where T : global::Google.Protobuf.IMessage;
		void Notify<T>(T msg) where T : global::Google.Protobuf.IMessage;
	}
	public delegate void Callback();
	public static partial class Api {
		public static class Id<T> { public static int Value; }
		private delegate Callback Binder(Handler handler, INotifier notifier, global::System.IO.Stream stream);
		private static global::System.Collections.Generic.Dictionary<int, Binder> Binders = new global::System.Collections.Generic.Dictionary<int, Binder>();
		static public void StartUp() {
			Id<Schema.Protobuf.Message.Administrator.TerminalCommand>.Value = 0x00010001;  // 65537
			Id<Schema.Protobuf.Message.Administrator.ConnectedAgentInfo>.Value = 0x00010002;  // 65538
			Id<Schema.Protobuf.Message.Administrator.Exit>.Value = 0x00010003;  // 65539
			Id<Schema.Protobuf.Message.Administrator.GameServerState>.Value = 0x00010004;  // 65540
			Id<Schema.Protobuf.Message.Administrator.SyncTime>.Value = 0x00010005;  // 65541
			Id<Schema.Protobuf.Message.Administrator.GetUniqueKeySeed>.Value = 0x00010006;  // 65542
			Id<Schema.Protobuf.Message.Common.GameResult>.Value = 0x00020001;  // 131073
			Id<Schema.Protobuf.Message.Common.MatchingInfo>.Value = 0x00020002;  // 131074
			Id<Schema.Protobuf.Message.Common.Vector2>.Value = 0x00020003;  // 131075
			Id<Schema.Protobuf.Message.Common.Vector3>.Value = 0x00020004;  // 131076
			Id<Schema.Protobuf.Message.Common.Character>.Value = 0x00020005;  // 131077
			Id<Schema.Protobuf.Message.Common.PlaneMap>.Value = 0x00020006;  // 131078
			Id<Schema.Protobuf.Message.Authentication.Encript>.Value = 0x00040001;  // 262145
			Id<Schema.Protobuf.Message.Authentication.Login>.Value = 0x00040002;  // 262146
			Id<Schema.Protobuf.Message.Authentication.Logout>.Value = 0x00040003;  // 262147
			Id<Schema.Protobuf.Message.Game.BeginMatching>.Value = 0x00050001;  // 327681
			Id<Schema.Protobuf.Message.Game.CancelMatching>.Value = 0x00050002;  // 327682
			Id<Schema.Protobuf.Message.Game.EndMatching>.Value = 0x00050003;  // 327683
			Id<Schema.Protobuf.Message.Game.EnterRoom>.Value = 0x00050004;  // 327684
			Id<Schema.Protobuf.Message.Game.LeaveRoom>.Value = 0x00050005;  // 327685
			Id<Schema.Protobuf.Message.Game.EnterCharacter>.Value = 0x00050006;  // 327686
			Id<Schema.Protobuf.Message.Game.LeaveCharacter>.Value = 0x00050007;  // 327687
			Id<Schema.Protobuf.Message.Game.Damage>.Value = 0x00050008;  // 327688
			Id<Schema.Protobuf.Message.Game.Die>.Value = 0x00050009;  // 327689
			Id<Schema.Protobuf.Message.Game.MyGameResult>.Value = 0x0005000A;  // 327690
			Id<Schema.Protobuf.Message.Game.FinishGame>.Value = 0x0005000B;  // 327691
			Id<Schema.Protobuf.Message.Game.Chat>.Value = 0x0005000C;  // 327692
			Id<Schema.Protobuf.Message.Game.SyncTime>.Value = 0x0005000D;  // 327693
			Id<Schema.Protobuf.Message.Game.Move>.Value = 0x0005000E;  // 327694
			Id<Schema.Protobuf.Message.Game.RTT>.Value = 0x0005000F;  // 327695
			Id<Schema.Protobuf.Message.Game.World>.Value = 0x00050010;  // 327696
			Id<Schema.Protobuf.Message.Lobby.Logout>.Value = 0x00060001;  // 393217

			Binders.Add(65537, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Administrator.TerminalCommand.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(65538, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Administrator.ConnectedAgentInfo.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(65539, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Administrator.Exit.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(65540, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Administrator.GameServerState.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(65541, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Administrator.SyncTime.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(65542, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Administrator.GetUniqueKeySeed.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(131073, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Common.GameResult.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(131074, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Common.MatchingInfo.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(131075, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Common.Vector2.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(131076, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Common.Vector3.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(131077, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Common.Character.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(131078, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Common.PlaneMap.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(262145, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Authentication.Encript.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(262146, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Authentication.Login.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(262147, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Authentication.Logout.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(327681, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.BeginMatching.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(327682, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.CancelMatching.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(327683, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.EndMatching.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(327684, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.EnterRoom.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(327685, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.LeaveRoom.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(327686, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.EnterCharacter.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(327687, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.LeaveCharacter.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(327688, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.Damage.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(327689, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.Die.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(327690, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.MyGameResult.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(327691, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.FinishGame.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(327692, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.Chat.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(327693, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.SyncTime.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(327694, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.Move.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(327695, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.RTT.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(327696, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.World.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
			Binders.Add(393217, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Lobby.Logout.Parser.ParseFrom(stream);
				return () => { handler.OnMessage(notifier, msg); };
			});
		}

		public static Callback Bind(Handler handler, INotifier notifier, int code, global::System.IO.Stream stream) {

			Binder binder = null;
			if (Binders.TryGetValue(code, out binder) == false) return () => { };

			return binder(handler, notifier, stream);

		}
	}
}
