using Engine.Framework;
using dbms = Engine.Database.Management;
using System.Threading.Tasks;
using Engine.Network.Protocol;

namespace Application.Lobby.Entities
{
    public partial class User
    {
        public void OnMessage(Delegator<Delegatables.Game.Room>.Notifier notifier, global::Schema.Protobuf.Message.Game.EnterCharacter msg)
        {
            Client?.Notify(msg);
        }
    }
}
