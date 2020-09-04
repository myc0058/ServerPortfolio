using Application.Game.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Engine.Framework.Api;

namespace Application.Game.Delegatables.Lobby
{

    public class User : Engine.Network.Protocol.Delegator<User>.IDelegatable
    {
        public Engine.Network.Protocol.Delegator<User> Delegator { get; set; }

        public void OnConnect(Engine.Network.Protocol.IDelegator delegator, MemoryStream stream)
        {

        }

        public void OnDelegate(Engine.Network.Protocol.Delegator<User>.Notifier notifier, int code, MemoryStream stream)
        {
            //myc0058 실제로 매칭이 된 방인지를 EndMatching을 받아서 저장 하고 검증한다.
            var room = Singleton<Engine.Framework.Container<long, Entities.Room>>.Instance.GetOrCreate(notifier.To, (instance) =>
            {
                instance.UID = notifier.To;
                instance.Run(Room.SnapShotInterval);
            });

            var callback = Engine.Network.Api.Binder(room, notifier, code, stream);
            room.PostMessage(callback);

        }
    }


}
