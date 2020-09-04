using Schema.Protobuf.Message.Game;
using UnityEngine;

namespace Schema.Protobuf
{
    public partial class User
    {
        public override void OnMessage(INotifier notifier, LeaveRoom msg)
        {
            base.OnMessage(notifier, msg);

            Room.Clear();

            Debug.Log($"Receive {msg.GetType()} msg => " + msg.ToString());
        }
    }
}

