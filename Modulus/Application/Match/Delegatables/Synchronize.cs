using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Engine.Framework.Api;

namespace Application.Match.Delegatables
{
    public class Synchronize : Engine.Network.Protocol.Delegator<Synchronize>.IDelegatable
    {
        public static Engine.Network.Protocol.Delegator<Synchronize> Instance { get; private set; } = new Engine.Network.Protocol.Delegator<Synchronize>();

        public void OnConnect(Engine.Network.Protocol.IDelegator delegator, MemoryStream stream)
        {

        }

        public void OnDelegate(Engine.Network.Protocol.Delegator<Synchronize>.Notifier notifier, int code, MemoryStream stream)
        {

            //var user = Singleton<Engine.Framework.Container<long, Entities.User>>.Instance.GetOrCreate(notifier.To, (instance) => {

            //    instance.UID = notifier.To;
            //    instance.Lobby = notifier.UID;

            //});
            var callback = Engine.Network.Api.Binder(Singleton<Entities.Synchronize>.Instance, notifier, code, stream);
            Singleton<Entities.Synchronize>.Instance.PostMessage(callback);
        }
    }
}
