using Application.Match.Scheduler;
using System;
using static Application.Match.Scheduler.MatchMaker;
using static Engine.Framework.Api;

namespace Application.Match.Entities
{
    public partial class Game
    {
        public void OnMessage(Engine.Network.Protocol.Delegator<Application.Match.Delegatables.Game>.Notifier notifier, global::Schema.Protobuf.Message.Administrator.GameServerState msg)
        {
            Singleton<Scheduler.MatchMaker>.Instance.AddOrUpdateGameServerState(msg);
            Logger.Info("Match Recv GameServer State");
        }
    }
}
