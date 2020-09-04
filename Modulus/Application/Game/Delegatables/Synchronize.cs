using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Engine.Framework.Api;

namespace Application.Game.Delegatables
{
    public class Synchronize : Engine.Network.Protocol.Delegator<Synchronize>.IDelegatable
    {
        public static Engine.Network.Protocol.Delegator<Synchronize> Instance { get; internal set; } = Singleton<Engine.Network.Protocol.Delegator<Synchronize>>.Instance;

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
