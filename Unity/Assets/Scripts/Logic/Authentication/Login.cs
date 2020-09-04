using Schema.Protobuf;
using Schema.Protobuf.Message.Authentication;
using UnityEngine;

namespace Schema.Protobuf
{
    public partial class User
    {
        public override void OnMessage(INotifier notifier, Login msg)
        {
            base.OnMessage(notifier, msg);

            Idx = msg.Idx;
            ID = msg.Id;

            Debug.Log($"Receive {msg.GetType()} msg => " + msg.ToString());

            var beginMatching = new Schema.Protobuf.Message.Game.BeginMatching();
            Client?.Notify(beginMatching);
        }
    }
}

