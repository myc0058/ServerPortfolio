using Engine.Framework;
using Engine.Network.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using static Engine.Framework.Api;
using dbms = Engine.Database.Management;


namespace Application.Game.Entities {
	public partial class Room {

        public void OnMessage(Delegator<Delegatables.Lobby.User>.Notifier notifier, global::Schema.Protobuf.Message.Lobby.Logout msg)
        {
            LeaveRoom(msg.Idx);
        }
    }
}
