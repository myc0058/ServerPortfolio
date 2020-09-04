using System;
using Engine.Network.Protocol;
using static Engine.Framework.Api;

namespace Application.Match.Entities
{
    public partial class Synchronize
    {
        public void OnMessage(Delegator<Delegatables.Synchronize>.Notifier notifier, global::Schema.Protobuf.Message.Administrator.SyncTime msg)
        {
            msg.RecvLocalTime = DateTime.UtcNow.Ticks;

            long RTT = (msg.RecvLocalTime - msg.SendLocalTime);
            if (RTT >= 50 * 10000) return;

            Singleton<Application.Common.Scheduler.SyncTime>.Instance.SyncCount++;
            Singleton<Application.Common.Scheduler.SyncTime>.Instance.SyncList.Add(msg);
            Logger.Info($"recv synctime : {msg.SendLocalTime / 10000} {msg.RemoteTime / 10000} {msg.RecvLocalTime / 10000} {(msg.RecvLocalTime - msg.SendLocalTime)}");
        }
    }
}
