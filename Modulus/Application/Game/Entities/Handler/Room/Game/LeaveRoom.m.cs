using Engine.Framework;
using dbms = Engine.Database.Management;
using System.Threading.Tasks;
using System;
using Schema.Protobuf.Message.Game;
using Schema.Protobuf.Message.Enums;
using Schema.Protobuf.Message.Common;
using System.Linq;

namespace Application.Game.Entities
{
    public partial class Room
    {
        public void OnMessage(Engine.Network.Protocol.Delegator<Delegatables.Lobby.User>.Notifier notifier, global::Schema.Protobuf.Message.Game.LeaveRoom msg)
        {
            LeaveRoom(msg.Idx);
        }

        public void LeaveRoom(long idx)
        {
            Users.Remove(idx);
            Characters.Remove(idx);
            if (Users.Count < 1)
            {
                Close();
            }
        }
    }
}
