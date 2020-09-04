using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Engine.Framework.Api;

namespace Application.Match.Delegatables
{
    public partial class Lobby
    {
        public class User : Engine.Network.Protocol.Delegator<User>.IDelegatable
        {
            public void OnConnect(Engine.Network.Protocol.IDelegator delegator, MemoryStream stream)
            {

            }

            public void OnDelegate(Engine.Network.Protocol.Delegator<User>.Notifier notifier, int code, MemoryStream stream)
            {
                var user = Singleton<Engine.Framework.Container<long, Entities.User>>.Instance.GetOrCreate(notifier.To, (instance) =>
                {
                    instance.UID = notifier.To;
                    instance.LobbyUID = notifier.UID;
                });
                var callback = Engine.Network.Api.Binder(user, notifier, code, stream);
                user.PostMessage(callback);
            }
        }
    }
}
