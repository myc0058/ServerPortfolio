using System;
using static Tool.MockDedicateServer.Protocol.Client;

namespace Tool.MockDedicateServer.Entities {
	public partial class Game {
		public void OnMessage(Engine.Framework.INotifier notifier, global::Schema.Protobuf.Message.Action.EnterBattle msg) {

            EnteredPlayerCount++;

            Schema.Protobuf.Message.Action.EndMatching endMatching = (Schema.Protobuf.Message.Action.EndMatching) LastReceivedPacket[Engine.Framework.Id<Schema.Protobuf.Message.Action.EndMatching>.Value];

            bool enterAllPlayer = EnteredPlayerCount >= endMatching.MatchInfo.Idxs.Count;

            ReceivedEnterBattle[msg.Passport.Idx] = msg;

            if (enterAllPlayer == true)
            {
                WaitingResponses[DedicateState.WaitingEnterAllPlayer] = false;
            }

            notifier.Response(msg);
		}
	}
}
