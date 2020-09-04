using System;
using static Tool.MockDedicateServer.Protocol.Client;

namespace Tool.MockDedicateServer.Entities {
	public partial class Game {
		public void OnMessage(Engine.Framework.INotifier notifier, global::Schema.Protobuf.Message.Action.EndMatching msg) {

            for (int i = 0; i < msg.Passports.Count; i++)
            {
                msg.Passports[i].Slot = i;
            }
            notifier.Response(msg);

            LastReceivedPacket[Engine.Framework.Id<Schema.Protobuf.Message.Action.EndMatching>.Value] = msg;
            WaitingResponses[DedicateState.ReceiveEndMatching] = false;
            Console.Title += " " + msg.RoomId.ToString();
        }
	}
}
