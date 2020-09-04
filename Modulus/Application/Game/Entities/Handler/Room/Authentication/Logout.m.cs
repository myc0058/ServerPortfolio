using System;
using Engine.Network.Protocol;

namespace Application.Game.Entities {
	public partial class Room {
		public void OnMessage(Delegator<Delegatables.Lobby.User>.Notifier notifier, global::Schema.Protobuf.Message.Authentication.Logout msg)
		{
			LeaveRoom(msg.Idx);
        }
	}
}
