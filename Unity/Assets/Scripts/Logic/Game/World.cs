using Schema.Protobuf.Message.Game;
using UnityEngine;

namespace Schema.Protobuf
{
    public partial class User
    {
        public override void OnMessage(INotifier notifier, World msg)
        {
            base.OnMessage(notifier, msg);

            Room.UIRoomComp.RecvWorld(msg);
            //Debug.Log($"Receive {msg.GetType()} msg => " + msg.ToString());
        }
    }
}

