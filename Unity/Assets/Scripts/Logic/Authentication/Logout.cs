using Schema.Protobuf;
using Schema.Protobuf.Message.Authentication;
using UnityEngine;

namespace Schema.Protobuf
{
    public partial class User
    {
        public override void OnMessage(INotifier notifier, Logout msg)
        {
            base.OnMessage(notifier, msg);

            Debug.Log($"Receive {msg.GetType()} msg => " + msg.ToString());
        }
    }
}

