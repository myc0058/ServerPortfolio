using System;
using System.IO;

namespace Tool.MockDedicateServer.Entities {
	public partial class Game {
		public void OnMessage(Engine.Framework.INotifier notifier, global::Schema.Protobuf.Message.Action.Logout msg) {

            Schema.Protobuf.Message.Action.EndGame endGame = new Schema.Protobuf.Message.Action.EndGame();

            endGame.Idx = msg.Idx;
            endGame.Mode = Api.BattleMode;
            endGame.Rank = 0;
            MemoryStream stream = new MemoryStream();
            Protocol.Client.SerializeProtobufTo(stream, 0, endGame);
            Client.Write(stream);
        }

        internal void OnDisconnect()
        {
            Client?.Disconnect();
            Client = null;
            Environment.Exit(0);
        }
    }
}
