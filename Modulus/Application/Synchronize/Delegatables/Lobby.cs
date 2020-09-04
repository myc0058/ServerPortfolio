using Basis.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Engine.Framework.Api;

namespace Application.Synchronize.Delegatables
{
    public partial class Lobby : Engine.Network.Protocol.Delegator<Lobby>.IDelegatable
    {
        public void OnConnect(Engine.Network.Protocol.IDelegator delegator, MemoryStream stream)
        {
            StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
        }

        public void OnDelegate(Engine.Network.Protocol.Delegator<Lobby>.Notifier notifier, int code, MemoryStream stream)
        {
            var lobby = Singleton<Engine.Framework.Container<long, Entities.Lobby>>.Instance.GetOrCreate(notifier.To, (instance) => {
                instance.UID = notifier.To;
            });
            var callback = Engine.Network.Api.Binder(lobby, notifier, code, stream);
            lobby.PostMessage(callback);
        }
    }
}
