using Engine.Framework;
using dbms = Engine.Database.Management;
using System.Threading.Tasks;

namespace Application.Lobby.Entities
{
    public partial class User
    {
        public void OnMessage(Protocol.Client notifier, global::Schema.Protobuf.Message.Authentication.Login msg)
        {
            Engine.Framework.Api.Logger.Info($"Login Id : {msg.Id}");

            msg.Idx = msg.Id.GetHashCode();
            Account.Idx = msg.Idx;
            Account.Id = msg.Id;

            UID = Account.Idx;

            var client = Client;
            if (client != null && client.IsClosed() == false)
            {
                Entities.User.Add(this);
            }

            notifier.Response(msg);
        }
    }
}
