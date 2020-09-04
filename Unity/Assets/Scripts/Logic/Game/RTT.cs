using Schema.Protobuf.Message.Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Schema.Protobuf
{
    public partial class User
    {
        public override void OnMessage(INotifier notifier, RTT msg)
        {
            base.OnMessage(notifier, msg);

            notifier.Notify(msg);

            //UnityEngine.Debug.Log($"Receive {msg.GetType()} msg => " + msg.ToString());
        }
    }
}

