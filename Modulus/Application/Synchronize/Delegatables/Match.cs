using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Engine.Framework.Api;

namespace Application.Synchronize.Delegatables
{
    public partial class Match : Engine.Network.Protocol.Delegator<Match>.IDelegatable
    {
        public void OnConnect(Engine.Network.Protocol.IDelegator delegator, MemoryStream stream)
        {

        }
        public void OnDelegate(Engine.Network.Protocol.Delegator<Match>.Notifier notifier, int code, MemoryStream stream)
        {
            var client = Singleton<Entities.Match>.Instance;
            Engine.Framework.AsyncCallback callback = null;
            callback = Engine.Network.Api.Binder(client, notifier, code, stream);
            client?.PostMessage(callback);
        }
    }
}
