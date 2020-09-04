using System;
using static Engine.Framework.Api;
namespace Schema.Protobuf {

	public static partial class Api {
		public delegate void RuntimeBindException(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e, dynamic handler, dynamic notifier, Type mag);
		public delegate void RuntimeBinderInternalCompilerException(Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e, dynamic handler, Type mag);
		public static RuntimeBindException RuntimeBindExceptionCallback = (e, handler, notifier, msg) => {
			Logger.Info(string.Format("'{0}' has not handler for '{1}''{2}'", handler.GetType(), notifier.GetType(), msg));
		};
		public static RuntimeBinderInternalCompilerException RuntimeBinderInternalCompilerExceptionCallback = (e, handler, msg) => {
			Logger.Info(string.Format("RuntimeBinderInternalCompilerException from '{0}'", msg));
			Logger.Info(e);
		};
		private delegate global::Google.Protobuf.IMessage Deserializer(global::System.IO.MemoryStream stream);
		private delegate global::Engine.Framework.AsyncCallback Binder(dynamic handler, dynamic notifier, global::System.IO.Stream stream);
		private static global::System.Collections.Generic.Dictionary<int, Deserializer> deserilizer = new System.Collections.Generic.Dictionary<int, Deserializer>();
		private static global::System.Collections.Generic.Dictionary<int, Binder> Binders = new global::System.Collections.Generic.Dictionary<int, Binder>();
		private static global::System.Collections.Generic.Dictionary<int, Type> types = new global::System.Collections.Generic.Dictionary<int, Type>();
		static public void StartUp() {
			Engine.Framework.Id<Schema.Protobuf.Message.Administrator.TerminalCommand>.Value = 0x00010001;  // 65537
			Engine.Framework.Id<Schema.Protobuf.Message.Administrator.ConnectedAgentInfo>.Value = 0x00010002;  // 65538
			Engine.Framework.Id<Schema.Protobuf.Message.Administrator.Exit>.Value = 0x00010003;  // 65539
			Engine.Framework.Id<Schema.Protobuf.Message.Administrator.GameServerState>.Value = 0x00010004;  // 65540
			Engine.Framework.Id<Schema.Protobuf.Message.Administrator.SyncTime>.Value = 0x00010005;  // 65541
			Engine.Framework.Id<Schema.Protobuf.Message.Administrator.GetUniqueKeySeed>.Value = 0x00010006;  // 65542
			Engine.Framework.Id<Schema.Protobuf.Message.Common.GameResult>.Value = 0x00020001;  // 131073
			Engine.Framework.Id<Schema.Protobuf.Message.Common.MatchingInfo>.Value = 0x00020002;  // 131074
			Engine.Framework.Id<Schema.Protobuf.Message.Common.Vector2>.Value = 0x00020003;  // 131075
			Engine.Framework.Id<Schema.Protobuf.Message.Common.Vector3>.Value = 0x00020004;  // 131076
			Engine.Framework.Id<Schema.Protobuf.Message.Common.Character>.Value = 0x00020005;  // 131077
			Engine.Framework.Id<Schema.Protobuf.Message.Common.PlaneMap>.Value = 0x00020006;  // 131078
			Engine.Framework.Id<Schema.Protobuf.Message.Authentication.Encript>.Value = 0x00040001;  // 262145
			Engine.Framework.Id<Schema.Protobuf.Message.Authentication.Login>.Value = 0x00040002;  // 262146
			Engine.Framework.Id<Schema.Protobuf.Message.Authentication.Logout>.Value = 0x00040003;  // 262147
			Engine.Framework.Id<Schema.Protobuf.Message.Game.BeginMatching>.Value = 0x00050001;  // 327681
			Engine.Framework.Id<Schema.Protobuf.Message.Game.CancelMatching>.Value = 0x00050002;  // 327682
			Engine.Framework.Id<Schema.Protobuf.Message.Game.EndMatching>.Value = 0x00050003;  // 327683
			Engine.Framework.Id<Schema.Protobuf.Message.Game.EnterRoom>.Value = 0x00050004;  // 327684
			Engine.Framework.Id<Schema.Protobuf.Message.Game.LeaveRoom>.Value = 0x00050005;  // 327685
			Engine.Framework.Id<Schema.Protobuf.Message.Game.EnterCharacter>.Value = 0x00050006;  // 327686
			Engine.Framework.Id<Schema.Protobuf.Message.Game.LeaveCharacter>.Value = 0x00050007;  // 327687
			Engine.Framework.Id<Schema.Protobuf.Message.Game.Damage>.Value = 0x00050008;  // 327688
			Engine.Framework.Id<Schema.Protobuf.Message.Game.Die>.Value = 0x00050009;  // 327689
			Engine.Framework.Id<Schema.Protobuf.Message.Game.MyGameResult>.Value = 0x0005000A;  // 327690
			Engine.Framework.Id<Schema.Protobuf.Message.Game.FinishGame>.Value = 0x0005000B;  // 327691
			Engine.Framework.Id<Schema.Protobuf.Message.Game.Chat>.Value = 0x0005000C;  // 327692
			Engine.Framework.Id<Schema.Protobuf.Message.Game.SyncTime>.Value = 0x0005000D;  // 327693
			Engine.Framework.Id<Schema.Protobuf.Message.Game.Move>.Value = 0x0005000E;  // 327694
			Engine.Framework.Id<Schema.Protobuf.Message.Game.RTT>.Value = 0x0005000F;  // 327695
			Engine.Framework.Id<Schema.Protobuf.Message.Game.World>.Value = 0x00050010;  // 327696
			Engine.Framework.Id<Schema.Protobuf.Message.Lobby.Logout>.Value = 0x00060001;  // 393217

			Binders.Add(65537, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Administrator.TerminalCommand.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(65537, (stream) => {
				var msg = Schema.Protobuf.Message.Administrator.TerminalCommand.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(65537, typeof(Schema.Protobuf.Message.Administrator.TerminalCommand));
			Binders.Add(65538, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Administrator.ConnectedAgentInfo.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(65538, (stream) => {
				var msg = Schema.Protobuf.Message.Administrator.ConnectedAgentInfo.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(65538, typeof(Schema.Protobuf.Message.Administrator.ConnectedAgentInfo));
			Binders.Add(65539, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Administrator.Exit.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(65539, (stream) => {
				var msg = Schema.Protobuf.Message.Administrator.Exit.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(65539, typeof(Schema.Protobuf.Message.Administrator.Exit));
			Binders.Add(65540, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Administrator.GameServerState.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(65540, (stream) => {
				var msg = Schema.Protobuf.Message.Administrator.GameServerState.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(65540, typeof(Schema.Protobuf.Message.Administrator.GameServerState));
			Binders.Add(65541, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Administrator.SyncTime.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(65541, (stream) => {
				var msg = Schema.Protobuf.Message.Administrator.SyncTime.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(65541, typeof(Schema.Protobuf.Message.Administrator.SyncTime));
			Binders.Add(65542, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Administrator.GetUniqueKeySeed.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(65542, (stream) => {
				var msg = Schema.Protobuf.Message.Administrator.GetUniqueKeySeed.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(65542, typeof(Schema.Protobuf.Message.Administrator.GetUniqueKeySeed));
			Binders.Add(131073, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Common.GameResult.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(131073, (stream) => {
				var msg = Schema.Protobuf.Message.Common.GameResult.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(131073, typeof(Schema.Protobuf.Message.Common.GameResult));
			Binders.Add(131074, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Common.MatchingInfo.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(131074, (stream) => {
				var msg = Schema.Protobuf.Message.Common.MatchingInfo.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(131074, typeof(Schema.Protobuf.Message.Common.MatchingInfo));
			Binders.Add(131075, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Common.Vector2.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(131075, (stream) => {
				var msg = Schema.Protobuf.Message.Common.Vector2.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(131075, typeof(Schema.Protobuf.Message.Common.Vector2));
			Binders.Add(131076, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Common.Vector3.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(131076, (stream) => {
				var msg = Schema.Protobuf.Message.Common.Vector3.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(131076, typeof(Schema.Protobuf.Message.Common.Vector3));
			Binders.Add(131077, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Common.Character.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(131077, (stream) => {
				var msg = Schema.Protobuf.Message.Common.Character.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(131077, typeof(Schema.Protobuf.Message.Common.Character));
			Binders.Add(131078, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Common.PlaneMap.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(131078, (stream) => {
				var msg = Schema.Protobuf.Message.Common.PlaneMap.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(131078, typeof(Schema.Protobuf.Message.Common.PlaneMap));
			Binders.Add(262145, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Authentication.Encript.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(262145, (stream) => {
				var msg = Schema.Protobuf.Message.Authentication.Encript.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(262145, typeof(Schema.Protobuf.Message.Authentication.Encript));
			Binders.Add(262146, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Authentication.Login.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(262146, (stream) => {
				var msg = Schema.Protobuf.Message.Authentication.Login.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(262146, typeof(Schema.Protobuf.Message.Authentication.Login));
			Binders.Add(262147, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Authentication.Logout.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(262147, (stream) => {
				var msg = Schema.Protobuf.Message.Authentication.Logout.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(262147, typeof(Schema.Protobuf.Message.Authentication.Logout));
			Binders.Add(327681, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.BeginMatching.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(327681, (stream) => {
				var msg = Schema.Protobuf.Message.Game.BeginMatching.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(327681, typeof(Schema.Protobuf.Message.Game.BeginMatching));
			Binders.Add(327682, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.CancelMatching.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(327682, (stream) => {
				var msg = Schema.Protobuf.Message.Game.CancelMatching.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(327682, typeof(Schema.Protobuf.Message.Game.CancelMatching));
			Binders.Add(327683, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.EndMatching.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(327683, (stream) => {
				var msg = Schema.Protobuf.Message.Game.EndMatching.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(327683, typeof(Schema.Protobuf.Message.Game.EndMatching));
			Binders.Add(327684, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.EnterRoom.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(327684, (stream) => {
				var msg = Schema.Protobuf.Message.Game.EnterRoom.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(327684, typeof(Schema.Protobuf.Message.Game.EnterRoom));
			Binders.Add(327685, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.LeaveRoom.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(327685, (stream) => {
				var msg = Schema.Protobuf.Message.Game.LeaveRoom.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(327685, typeof(Schema.Protobuf.Message.Game.LeaveRoom));
			Binders.Add(327686, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.EnterCharacter.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(327686, (stream) => {
				var msg = Schema.Protobuf.Message.Game.EnterCharacter.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(327686, typeof(Schema.Protobuf.Message.Game.EnterCharacter));
			Binders.Add(327687, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.LeaveCharacter.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(327687, (stream) => {
				var msg = Schema.Protobuf.Message.Game.LeaveCharacter.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(327687, typeof(Schema.Protobuf.Message.Game.LeaveCharacter));
			Binders.Add(327688, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.Damage.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(327688, (stream) => {
				var msg = Schema.Protobuf.Message.Game.Damage.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(327688, typeof(Schema.Protobuf.Message.Game.Damage));
			Binders.Add(327689, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.Die.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(327689, (stream) => {
				var msg = Schema.Protobuf.Message.Game.Die.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(327689, typeof(Schema.Protobuf.Message.Game.Die));
			Binders.Add(327690, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.MyGameResult.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(327690, (stream) => {
				var msg = Schema.Protobuf.Message.Game.MyGameResult.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(327690, typeof(Schema.Protobuf.Message.Game.MyGameResult));
			Binders.Add(327691, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.FinishGame.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(327691, (stream) => {
				var msg = Schema.Protobuf.Message.Game.FinishGame.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(327691, typeof(Schema.Protobuf.Message.Game.FinishGame));
			Binders.Add(327692, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.Chat.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(327692, (stream) => {
				var msg = Schema.Protobuf.Message.Game.Chat.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(327692, typeof(Schema.Protobuf.Message.Game.Chat));
			Binders.Add(327693, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.SyncTime.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(327693, (stream) => {
				var msg = Schema.Protobuf.Message.Game.SyncTime.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(327693, typeof(Schema.Protobuf.Message.Game.SyncTime));
			Binders.Add(327694, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.Move.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(327694, (stream) => {
				var msg = Schema.Protobuf.Message.Game.Move.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(327694, typeof(Schema.Protobuf.Message.Game.Move));
			Binders.Add(327695, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.RTT.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(327695, (stream) => {
				var msg = Schema.Protobuf.Message.Game.RTT.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(327695, typeof(Schema.Protobuf.Message.Game.RTT));
			Binders.Add(327696, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Game.World.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(327696, (stream) => {
				var msg = Schema.Protobuf.Message.Game.World.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(327696, typeof(Schema.Protobuf.Message.Game.World));
			Binders.Add(393217, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Lobby.Logout.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(393217, (stream) => {
				var msg = Schema.Protobuf.Message.Lobby.Logout.Parser.ParseFrom(stream);
				return msg;
			});
			types.Add(393217, typeof(Schema.Protobuf.Message.Lobby.Logout));
		}

		public static global::Engine.Framework.AsyncCallback Bind(dynamic handler, dynamic notifier, int code, global::System.IO.Stream stream) {

			Binder binder = null;
			if (Binders.TryGetValue(code, out binder) == false) return () => { Console.WriteLine($"Can not find code {code} binder."); };

			return binder(handler, notifier, stream);

		}
		public static global::Google.Protobuf.IMessage Deserialize(int code, global::System.IO.MemoryStream stream) {

			if (deserilizer.TryGetValue(code, out Deserializer callback) == true) {
				return callback(stream);
			}
			return null;
		}
		public static Type CodeToType(int code) {

			if (types.TryGetValue(code, out Type type) == true) {
				return type;
			}
			return null;
		}
	}
}
