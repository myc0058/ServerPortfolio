using Engine.Framework;
using dbms = Engine.Database.Management;
using System.Threading.Tasks;
using Schema.Protobuf.Message.Enums;
using Engine.Network.Protocol;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace Application.Game.Entities
{
    public partial class Room
    {
        public void OnMessage(Delegator<Delegatables.Lobby.User>.Notifier notifier, global::Schema.Protobuf.Message.Game.RTT msg)
        {
            Users.TryGetValue(msg.Idx, out var user);
            if (user != null)
            {
                user.RecvRTT(msg);
            }
        }

        private Stopwatch swRTT = new Stopwatch();

        public void SendRTT(long deltaTicks)
        {
            if (swRTT.IsRunning == false)
            {
                swRTT.Start();
            }

            if (swRTT.Elapsed.TotalSeconds < 10)
            {
                return;
            }

            long elapsedTicks = swRTT.ElapsedTicks;

            swRTT.Restart();

            foreach (var item in Users)
            {
                item.Value.SendRTT();
            }
        }
    }
}
