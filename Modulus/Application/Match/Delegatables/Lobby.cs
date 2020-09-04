using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Engine.Framework.Api;

namespace Application.Match.Delegatables
{
    public partial class Lobby : Engine.Network.Protocol.Delegator<Lobby>.IDelegatable
    {
        public void OnConnect(Engine.Network.Protocol.IDelegator delegator, MemoryStream stream)
        {

        }

        public void OnDelegate(Engine.Network.Protocol.Delegator<Lobby>.Notifier notifier, int code, MemoryStream stream)
        {

            //var user = Singleton<Engine.Framework.Container<long, Entities.User>>.Instance.GetOrCreate(notifier.To, (instance) => {

            //    instance.UID = notifier.To;
            //    instance.Lobby = notifier.UID;

            //});
            var callback = Engine.Network.Api.Binder(Singleton<Entities.Lobby>.Instance, notifier, code, stream);
            Singleton<Entities.Lobby>.Instance.PostMessage(callback);
        }
    }
}
