using Schema.Protobuf.Message.Game;
using UnityEngine;

namespace Schema.Protobuf
{
    public partial class User
    {
        public override void OnMessage(INotifier notifier, EnterCharacter msg)
        {
            base.OnMessage(notifier, msg);

            Debug.Log($"Receive {msg.GetType()} msg => " + msg.ToString());

            var roomComp = Unity.Room.GetRoomComponent(Idx.ToString());

            if (Room.RoomId == msg.RoomId)
            {
                Room.Characters.Remove(msg.Character.Idx);
                var character = new Room.Character(msg.Character);
                Room.Characters.Add(msg.Character.Idx, character);
                roomComp.AddCharacter(this, character);
            }
        }
    }
}

