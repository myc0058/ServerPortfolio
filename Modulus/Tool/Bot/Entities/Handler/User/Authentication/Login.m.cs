using System;
using Tool.Bot.Protocol;

namespace Tool.Bot.Entities {
	public partial class User {
		public void OnMessage(Engine.Framework.INotifier notifier, global::Schema.Protobuf.Message.Authentication.Login msg) {
            Account = msg;

            var client = (BotClient)Client;

            client.Idx = msg.Idx;
            
            if (client != null && client.IsClosed() == false)
            {
                Entities.User.Add(this);
            }

            LastReceivedPacket[Engine.Framework.Id<Schema.Protobuf.Message.Authentication.Login>.Value] = msg;
            client.SetWaitingResponse(BotClient.UserState.Login, false);
        }
	}
}
