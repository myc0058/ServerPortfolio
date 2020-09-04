using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Engine.Framework.Api;

namespace Application.Synchronize.Delegatables
{
    public class Game : Engine.Network.Protocol.Delegator<Game>.IDelegatable
    {
        public static Engine.Network.Protocol.Delegator<Game> Instance { get; internal set; } = Singleton<Engine.Network.Protocol.Delegator<Game>>.Instance;

        public void OnConnect(Engine.Network.Protocol.IDelegator delegator, MemoryStream stream)
        {
        }
        public void OnDelegate(Engine.Network.Protocol.Delegator<Game>.Notifier notifier, int code, MemoryStream stream)
        {
            var client = Singleton<Entities.Game>.Instance;
            Engine.Framework.AsyncCallback callback = null;
            callback = Engine.Network.Api.Binder(client, notifier, code, stream);
            client?.PostMessage(callback);
        }
    }
}

