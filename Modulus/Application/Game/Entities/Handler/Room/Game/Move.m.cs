using Engine.Framework;
using dbms = Engine.Database.Management;
using System.Threading.Tasks;
using Schema.Protobuf.Message.Enums;
using System;

namespace Application.Game.Entities
{
    public partial class Room
    {
        public void OnMessage(Engine.Network.Protocol.Delegator<Delegatables.Lobby.User>.Notifier notifier, global::Schema.Protobuf.Message.Game.Move msg)
        {
            Characters.TryGetValue(msg.Idx, out var character);

            if (character == null) { return; }

            character?.Move(msg);
        }
    }
}
