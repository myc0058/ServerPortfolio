using Engine.Network.Protocol;
using Schema.Protobuf.Message.Game;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Application.Game.Entities
{
    public partial class Room
    {
        public class User
        {
            public long Idx { get; set; } = 0;

            public IDelegator Delegator { get; set; } = null;
            public MyGameResult MyGameResult = new MyGameResult()
            {
                ResponseCode = Schema.Protobuf.Message.Enums.ResponseCode.Success,
                Data = null,
            };

            public bool SendGameResult { get; set; } = false;

            public Room Room { get; set; } = null;

            #region RTT
            private Dictionary<long, long> rttSendHistory = new Dictionary<long, long>();
            private long rttSeq = 0;

            public Queue<long> RTTs = new Queue<long>();

            public long AvgRTT { get; set; } = 0;
            public void SendRTT()
            {
                var rtt = new RTT();
                rtt.Idx = Idx;
                rtt.Seq = rttSeq;
                rttSeq++;
                Delegator.Delegate(Room.UID, Idx, rtt);
                rttSendHistory.Add(rttSeq, DateTime.UtcNow.Ticks);
            }

            public void RecvRTT(RTT msg)
            {
                rttSendHistory.TryGetValue(msg.Seq, out var sendTicks);
                if (sendTicks == 0) { return; }

                rttSendHistory.Remove(msg.Seq);

                long rtt = DateTime.UtcNow.Ticks - sendTicks;
                RTTs.Enqueue(rtt);

                while(RTTs.Count > 10)
                {
                    RTTs.Dequeue();
                }

                AvgRTT = 0;
                foreach (var item in RTTs)
                {
                    AvgRTT += item;
                }
                AvgRTT = AvgRTT / RTTs.Count;
            }
            #endregion
        }

        public Dictionary<long, User> Users = new Dictionary<long, User>();
    }
}
