using System;
using Tool.Bot.Protocol;

namespace Tool.Bot.Entities {
	public partial class User {
		public void OnMessage(Engine.Framework.INotifier notifier, global::Schema.Protobuf.Message.Lobby.Logout msg) {

            var client = (BotClient)Client;

            LastReceivedPacket[Engine.Framework.Id<Schema.Protobuf.Message.Lobby.Logout>.Value] = msg;
            client.SetWaitingResponse(BotClient.UserState.Logout, false);

        }

        internal void OnDisconnect()
        {
            Close();
        }
    }
}
