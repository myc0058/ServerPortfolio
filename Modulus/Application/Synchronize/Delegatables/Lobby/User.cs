using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Synchronize.Delegatables
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
                var user = Entities.User.Get(notifier.To);
                var callback = Engine.Network.Api.Binder(user, notifier, code, stream);
                user.PostMessage(callback);
            }
        }
    }
}
