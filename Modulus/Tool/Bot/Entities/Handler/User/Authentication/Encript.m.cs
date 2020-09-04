using System;
using System.Security.Cryptography;
using Tool.Bot.Protocol;

namespace Tool.Bot.Entities {
	public partial class User {
		public void OnMessage(Engine.Framework.INotifier notifier, global::Schema.Protobuf.Message.Authentication.Encript msg) {

            if (msg.Key.Length > 0)
            {
                var aes = System.Security.Cryptography.Aes.Create();

                aes.Key = Convert.FromBase64String(msg.Key);
                aes.IV = Convert.FromBase64String(msg.IV);
                aes.Mode = CipherMode.ECB;

                Client.aesAlg = aes;
            }

            var client = (BotClient)Client;

            client.State = BotClient.UserState.ReceivedEncript;
            LastReceivedPacket[Engine.Framework.Id<Schema.Protobuf.Message.Authentication.Encript>.Value] = msg;
            client.SetWaitingResponse(BotClient.UserState.ReceivedEncript, false);
        }
	}
}
