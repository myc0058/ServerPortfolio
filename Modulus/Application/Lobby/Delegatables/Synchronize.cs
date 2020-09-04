using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Engine.Framework.Api;

namespace Application.Lobby.Delegatables
{
    public partial class Synchronize : Engine.Network.Protocol.Delegator<Synchronize>.IDelegatable
    {
        public static Engine.Network.Protocol.Delegator<Synchronize> Instance = new Engine.Network.Protocol.Delegator<Synchronize>();
        public void OnConnect(Engine.Network.Protocol.IDelegator delegator, MemoryStream stream)
        {
        }
        public void OnDelegate(Engine.Network.Protocol.Delegator<Synchronize>.Notifier notifier, int code, MemoryStream stream)
        {
            var synchronize = Singleton<Entities.Synchronize>.Instance;
            var callback = Engine.Network.Api.Binder(synchronize, notifier, code, stream);

            synchronize.PostMessage(callback);

        }
    }
}
