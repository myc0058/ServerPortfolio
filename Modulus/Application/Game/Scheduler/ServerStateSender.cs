using Application.Game.Entities;
using Schema.Protobuf.Message.Administrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Engine.Framework.Api;

namespace Application.Game.Scheduler
{
    public class ServerStateSender : Engine.Framework.Scheduler
    {
        public ServerStateSender() : base(Singleton<Engine.Framework.Layer>.Instance) { }
        protected override void OnSchedule(long deltaTicks)
        {
            SendServerState();
        }

        public void SendServerState()
        {
            GameServerState msg = new GameServerState();
            msg.GameServerID = Api.Idx;
            msg.LobbyDelegatorID = Engine.Framework.Api.AddressToInt64(Api.LobbyGameIp, Api.LobbyGamePort);
            msg.RunningGameCount = Room.Count();
            Delegatables.Match.Instance.Delegate(Api.Idx, 0, msg);
        }
    }
}
