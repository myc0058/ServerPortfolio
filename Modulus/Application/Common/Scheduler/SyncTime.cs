using Engine.Network.Protocol;
using System;
using System.Collections.Generic;
using static Engine.Framework.Api;

namespace Application.Common.Scheduler
{
    public class SyncTime : Engine.Framework.Scheduler
    {
        public IDelegator Delegator { get; set; } = null;

        public long TimeOffset { get; set; } = 0;

        public int SyncCount = 0;
        public List<Schema.Protobuf.Message.Administrator.SyncTime> SyncList = new List<Schema.Protobuf.Message.Administrator.SyncTime>();
        public SyncTime() : base(Singleton<Engine.Framework.Layer>.Instance) { }
        
        protected override void OnSchedule(long deltaTicks)
        {
            if (SyncCount >= 100)
            {
                List<long> diffTimes = new List<long>();

                this.Stop();

                long offsetTotal = 0;
                int validSyncCount = 0;

                SyncList.Sort((Schema.Protobuf.Message.Administrator.SyncTime x, Schema.Protobuf.Message.Administrator.SyncTime y) =>
                {
                    long RTT = (x.RecvLocalTime - x.SendLocalTime);
                    long localTime = x.SendLocalTime + RTT / 2;
                    long xOffset = x.RemoteTime - localTime;

                    RTT = (y.RecvLocalTime - y.SendLocalTime);
                    localTime = y.SendLocalTime + RTT / 2;
                    long yOffset = y.RemoteTime - localTime;

                    if (xOffset > yOffset)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                });

                SyncList.RemoveRange(0, 30);
                SyncList.RemoveRange(SyncList.Count - 30, 30);

                foreach (var item in SyncList)
                {
                    long RTT = (item.RecvLocalTime - item.SendLocalTime);
                    long localTime = item.SendLocalTime + RTT / 2;

                    diffTimes.Add(item.RemoteTime - localTime);
                    offsetTotal += item.RemoteTime - localTime;
                    validSyncCount++;
                }
                TimeOffset = offsetTotal / validSyncCount;
                Console.WriteLine($"SyncTime : {new TimeSpan(TimeOffset).TotalMilliseconds}");
                return;
            }

            if (Delegator != null)
            {
                var msg = new Schema.Protobuf.Message.Administrator.SyncTime();
                msg.SendLocalTime = DateTime.UtcNow.Ticks;

                // 아래꺼보다 이게 빠르다. 바꾸지 말자
                Delegator.Delegate(0, 0, msg);

                /*
                Delegator.Delegate(this, 0, msg, (ret) =>
                {
                    ret.RecvLocalTime = DateTime.UtcNow.Ticks;
                    Singleton<Application.Common.Scheduler.SyncTime>.Instance.SyncCount++;
                    Singleton<Application.Common.Scheduler.SyncTime>.Instance.SyncList.Add(ret);
                },
                () =>
                {
                    Logger.Error($"Send SyncTime Fail");
                });
                */
            }
        }

        public DateTime ServerDateTime
        {
            get
            {
                return DateTime.UtcNow.AddTicks(TimeOffset);
            }
        }

        public long ServerTicks
        {
            get
            {
                return DateTime.UtcNow.Ticks + TimeOffset;
            }
        }
    }
}
