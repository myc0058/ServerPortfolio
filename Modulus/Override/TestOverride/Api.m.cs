using System;
namespace Schema.Protobuf {

	public static partial class Api {
		public delegate void RuntimeBindException(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e, dynamic handler, Type mag);
		public delegate void RuntimeBinderInternalCompilerException(Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e, dynamic handler, Type mag);
		public static RuntimeBindException RuntimeBindExceptionCallback = (e, handler, msg) => {
			Console.WriteLine(string.Format("'{0}' has not handler for '{1}'", handler.GetType(), msg));
		};
		public static RuntimeBinderInternalCompilerException RuntimeBinderInternalCompilerExceptionCallback = (e, handler, msg) => {
			Console.WriteLine(string.Format("RuntimeBinderInternalCompilerException from '{0}'", msg));
			Console.WriteLine(e);
		};
		private delegate global::Google.Protobuf.IMessage Deserializer(global::System.IO.MemoryStream stream);
		private delegate global::Engine.Framework.Layer.Task.Callback Binder(dynamic handler, Engine.Framework.INotifier notifier, global::System.IO.Stream stream);
		private static global::System.Collections.Generic.Dictionary<int, Deserializer> deserilizer = new System.Collections.Generic.Dictionary<int, Deserializer>();
		private static global::System.Collections.Generic.Dictionary<int, Binder> Binders = new global::System.Collections.Generic.Dictionary<int, Binder>();
		static public void StartUp() {
			Engine.Framework.Id<Schema.Protobuf.Message.Action.BeginMatching>.Value = 0x00010001;  // 65537
			Engine.Framework.Id<Schema.Protobuf.Message.Action.EndMatching>.Value = 0x00010002;  // 65538
			Engine.Framework.Id<Schema.Protobuf.Message.Action.SelectCharacter>.Value = 0x00010003;  // 65539
			Engine.Framework.Id<Schema.Protobuf.Message.Action.Ready>.Value = 0x00010004;  // 65540
			Engine.Framework.Id<Schema.Protobuf.Message.Action.EnterBattle>.Value = 0x00010005;  // 65541
			Engine.Framework.Id<Schema.Protobuf.Message.Action.Logout>.Value = 0x00010006;  // 65542
			Engine.Framework.Id<Schema.Protobuf.Message.Action.CancelMatching>.Value = 0x00010007;  // 65543
			Engine.Framework.Id<Schema.Protobuf.Message.Action.TestAddPacket>.Value = 0x00010008;  // 65544
			Engine.Framework.Id<Schema.Protobuf.Message.Authentication.Encript>.Value = 0x00020001;  // 131073
			Engine.Framework.Id<Schema.Protobuf.Message.Authentication.Login>.Value = 0x00020002;  // 131074
			Engine.Framework.Id<Schema.Protobuf.Message.Authentication.Logout>.Value = 0x00020003;  // 131075
			Engine.Framework.Id<Schema.Protobuf.Data>.Value = 0x00030001;  // 196609
			Engine.Framework.Id<Schema.Protobuf.Message.Shop.Open>.Value = 0x00070001;  // 458753
			Engine.Framework.Id<Schema.Protobuf.Message.Shop.Buy>.Value = 0x00070002;  // 458754
			Engine.Framework.Id<Schema.Protobuf.Message.Synchronize.Chat>.Value = 0x00080001;  // 524289

			Binders.Add(65537, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Action.BeginMatching.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(65537, (stream) => {
				var msg = Schema.Protobuf.Message.Action.BeginMatching.Parser.ParseFrom(stream);
				return msg;
			});
			Binders.Add(65538, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Action.EndMatching.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(65538, (stream) => {
				var msg = Schema.Protobuf.Message.Action.EndMatching.Parser.ParseFrom(stream);
				return msg;
			});
			Binders.Add(65539, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Action.SelectCharacter.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(65539, (stream) => {
				var msg = Schema.Protobuf.Message.Action.SelectCharacter.Parser.ParseFrom(stream);
				return msg;
			});
			Binders.Add(65540, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Action.Ready.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(65540, (stream) => {
				var msg = Schema.Protobuf.Message.Action.Ready.Parser.ParseFrom(stream);
				return msg;
			});
			Binders.Add(65541, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Action.EnterBattle.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(65541, (stream) => {
				var msg = Schema.Protobuf.Message.Action.EnterBattle.Parser.ParseFrom(stream);
				return msg;
			});
			Binders.Add(65542, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Action.Logout.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(65542, (stream) => {
				var msg = Schema.Protobuf.Message.Action.Logout.Parser.ParseFrom(stream);
				return msg;
			});
			Binders.Add(65543, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Action.CancelMatching.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(65543, (stream) => {
				var msg = Schema.Protobuf.Message.Action.CancelMatching.Parser.ParseFrom(stream);
				return msg;
			});
			Binders.Add(65544, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Action.TestAddPacket.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(65544, (stream) => {
				var msg = Schema.Protobuf.Message.Action.TestAddPacket.Parser.ParseFrom(stream);
				return msg;
			});
			Binders.Add(131073, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Authentication.Encript.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(131073, (stream) => {
				var msg = Schema.Protobuf.Message.Authentication.Encript.Parser.ParseFrom(stream);
				return msg;
			});
			Binders.Add(131074, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Authentication.Login.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(131074, (stream) => {
				var msg = Schema.Protobuf.Message.Authentication.Login.Parser.ParseFrom(stream);
				return msg;
			});
			Binders.Add(131075, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Authentication.Logout.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(131075, (stream) => {
				var msg = Schema.Protobuf.Message.Authentication.Logout.Parser.ParseFrom(stream);
				return msg;
			});
			Binders.Add(196609, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Data.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(196609, (stream) => {
				var msg = Schema.Protobuf.Data.Parser.ParseFrom(stream);
				return msg;
			});
			Binders.Add(458753, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Shop.Open.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(458753, (stream) => {
				var msg = Schema.Protobuf.Message.Shop.Open.Parser.ParseFrom(stream);
				return msg;
			});
			Binders.Add(458754, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Shop.Buy.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(458754, (stream) => {
				var msg = Schema.Protobuf.Message.Shop.Buy.Parser.ParseFrom(stream);
				return msg;
			});
			Binders.Add(524289, (handler, notifier, stream) =>
			{
				var msg = Schema.Protobuf.Message.Synchronize.Chat.Parser.ParseFrom(stream);
				return () => {
					try { handler.OnMessage(notifier, msg); }
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {
						RuntimeBindExceptionCallback(e, handler, msg.GetType());
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {
						RuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());
					}
					catch { throw; }
				};
			});
			deserilizer.Add(524289, (stream) => {
				var msg = Schema.Protobuf.Message.Synchronize.Chat.Parser.ParseFrom(stream);
				return msg;
			});
		}

		public static global::Engine.Framework.Layer.Task.Callback Bind(dynamic handler, global::Engine.Framework.INotifier notifier, int code, global::System.IO.Stream stream) {

			Binder binder = null;
			if (Binders.TryGetValue(code, out binder) == false) return () => { Console.WriteLine(string.Format("Code '{0}' is not have binder.", code)); };

			return binder(handler, notifier, stream);

		}
		public static global::Google.Protobuf.IMessage Deserialize(int code, global::System.IO.MemoryStream stream) {

			if (deserilizer.TryGetValue(code, out Deserializer callback) == true) {
				return callback(stream);
			}
			return null;
		}
	}
}
