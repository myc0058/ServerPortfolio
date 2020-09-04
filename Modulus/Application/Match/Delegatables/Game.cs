using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Engine.Framework.Api;

namespace Application.Match.Delegatables
{
    public class Game : Engine.Network.Protocol.Delegator<Game>.IDelegatable
    {
        public void OnConnect(Engine.Network.Protocol.IDelegator delegator, MemoryStream stream)
        {

        }

        public void OnDelegate(Engine.Network.Protocol.Delegator<Game>.Notifier notifier, int code, MemoryStream stream)
        {
            var callback = Engine.Network.Api.Binder(Singleton<Entities.Game>.Instance, notifier, code, stream);
            Singleton<Entities.Game>.Instance.PostMessage(callback);
        }
    }
}
