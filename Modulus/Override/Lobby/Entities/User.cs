using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Override.Lobby.Entities
{
    [Engine.Framework.Attributes.Override]
    public partial class User : Application.Lobby.Entities.User
    {
        public static void Override()
        {
            Engine.Framework.New<Application.Lobby.Entities.User>.Override<User>();
        }


        new public void OnMessage(Engine.Framework.INotifier notifier, global::Schema.Protobuf.Message.Authentication.Login msg)
        {
            Console.WriteLine("Overrided OnMessage Login");
            notifier.Response(msg);
        }
    }
}
