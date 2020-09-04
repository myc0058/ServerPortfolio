using System;
using Tool.Bot.Protocol;

namespace Tool.Bot.Entities {
	public partial class User {
		public void OnMessage(Engine.Framework.INotifier notifier, global::Schema.Protobuf.Message.Game.FinishGame msg) {
            
            LastReceivedPacket[Engine.Framework.Id<Schema.Protobuf.Message.Game.FinishGame>.Value] = msg;
            var client = (BotClient)Client;
            client.SetWaitingResponse(BotClient.UserState.ReceivedFinishGame, false);
        }
	}
}
