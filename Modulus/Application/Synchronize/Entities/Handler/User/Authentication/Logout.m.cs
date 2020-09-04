using Engine.Database;
using Engine.Framework;
using System;
using System.Collections.Generic;
using dbms = Engine.Database.Management;

namespace Application.Synchronize.Entities {
	public partial class User {
		public void OnMessage(Engine.Network.Protocol.Delegator<Delegatables.Lobby.User>.Notifier notifier, global::Schema.Protobuf.Message.Authentication.Logout msg) {
            
            //Group.Remove(UID);
            //Close();

        }
	}
}
