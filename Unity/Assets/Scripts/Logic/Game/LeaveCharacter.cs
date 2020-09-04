using Schema.Protobuf.Message.Game;
using UnityEngine;

namespace Schema.Protobuf
{
    public partial class User
    {
        public override void OnMessage(INotifier notifier, LeaveCharacter msg)
        {
            base.OnMessage(notifier, msg);

            Debug.Log($"Receive {msg.GetType()} msg => " + msg.ToString());

            if (Room.RoomId == msg.RoomId)
            {
                Room.Characters.Remove(msg.Character.Idx);
            }
        }
    }
}

