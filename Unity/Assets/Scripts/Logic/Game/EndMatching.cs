using Schema.Protobuf.Message.Game;
using UnityEngine;

namespace Schema.Protobuf
{
    public partial class User
    {
        public override void OnMessage(INotifier notifier, EndMatching msg)
        {
            base.OnMessage(notifier, msg);

            Debug.Log($"Receive {msg.GetType()} msg => " + msg.ToString());

            var enterRoom = new EnterRoom();

            Client?.Notify(enterRoom);
        }
    }
}

