using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using static Engine.Framework.Api;

namespace Application.Game.Delegatables
{
    public class Agent : Engine.Network.Protocol.Delegator<Agent>.IDelegatable
    {
        public static Engine.Network.Protocol.Delegator<Agent> Instance { get; internal set; } = Singleton<Engine.Network.Protocol.Delegator<Agent>>.Instance;

        public void OnConnect(Engine.Network.Protocol.IDelegator delegator, MemoryStream stream)
        {
        }
        public void OnDelegate(Engine.Network.Protocol.Delegator<Agent>.Notifier notifier, int code, MemoryStream stream)
        {
            var client = Singleton<Entities.Agent>.Instance;
            Engine.Framework.AsyncCallback callback = null;
            callback = Engine.Network.Api.Binder(client, notifier, code, stream);
            client?.PostMessage(callback);
        }
    }
}
