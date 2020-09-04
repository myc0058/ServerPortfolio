using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Engine.Framework.Api;

namespace Application.Lobby.Delegatables
{
    public partial class Synchronize
    {
        public class User : Engine.Network.Protocol.Delegator<User>.IDelegatable
        {
            public static Engine.Network.Protocol.Delegator<User> Instance { get; private set; } = new Engine.Network.Protocol.Delegator<User>();

            public class Null : Engine.Framework.INotifier
            {
                public Null(Engine.Network.Protocol.Delegator<User>.Notifier notifier)
                {
                    Notifier = notifier;
                }
                public void Response<T>(T msg)
                {
                    Notifier.Response(msg);
                }
                public void Notify<T>(T msg)
                {
                    Notifier.Notify(msg);
                }
                protected Engine.Network.Protocol.Delegator<User>.Notifier Notifier;
                public long To => Notifier.To;
            }
            
            public void OnConnect(Engine.Network.Protocol.IDelegator delegator, MemoryStream stream)
            {
            }
            public void OnDelegate(Engine.Network.Protocol.Delegator<User>.Notifier notifier, int code, MemoryStream stream)
            {
                var user = Entities.User.Get(notifier.To);
                Engine.Framework.AsyncCallback callback = null;
                if (user == null)
                {
                    user = Singleton<Entities.User>.Instance;
                    callback = Engine.Network.Api.Binder(user, new Null(notifier), code, stream);
                }
                else
                {
                    callback = Engine.Network.Api.Binder(user, notifier, code, stream);
                }
                user.PostMessage(callback);
            }
        }
    }
    
}
