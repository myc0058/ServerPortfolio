using Schema.Protobuf.Message.Administrator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Schema.Protobuf
{
    public partial class User
    {
        public override void OnMessage(INotifier notifier, SyncTime msg)
        {
            base.OnMessage(notifier, msg);

            msg.RecvLocalTime = DateTime.UtcNow.Ticks;

            long RTT = (msg.RecvLocalTime - msg.SendLocalTime);

            if (RTT >= 50 * 10000) return;

            long localTime = msg.SendLocalTime + RTT / 2;

            var offset = msg.RemoteTime - localTime;
            addTimeOffset(offset);

            //UnityEngine.Debug.Log($"Receive {msg.GetType()} msg => " + msg.ToString());
        }

        private long timeOffset = 0;
        private List<long> timeOffsetList = new List<long>();
        public bool KnowServerTime { get; set; } = false;

        private void addTimeOffset(long offset)
        {
            timeOffsetList.Add(offset);
            if (timeOffsetList.Count > 100)
            {
                timeOffsetList.RemoveAt(0);
            }

            if (timeOffsetList.Count >= 100)
            {
                List<long> tempList = new List<long>();
                tempList.AddRange(timeOffsetList);
                tempList.Sort((long x, long y) =>
                {
                    if (x > y)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                });

                tempList.RemoveRange(0, 30);
                tempList.RemoveRange(tempList.Count - 30, 30);

                long offsetTotal = 0;

                foreach (var item in tempList)
                {
                    offsetTotal += item;
                }

                timeOffset = offsetTotal / tempList.Count;

                KnowServerTime = true;

                UnityEngine.Debug.Log($"SyncTime : {timeOffset}");
            }
        }

        private Stopwatch swSyncTime = new Stopwatch();

        public void CheckAndSendSyncTime()
        {
            if (swSyncTime.IsRunning == false)
            {
                swSyncTime.Start();
            }

            if (KnowServerTime == true)
            {
                return;
            }
            else
            {
                if (swSyncTime.ElapsedMilliseconds > 100)
                {
                    var msg = new Schema.Protobuf.Message.Administrator.SyncTime();
                    msg.SendLocalTime = DateTime.UtcNow.Ticks;
                    Client?.Notify(msg);
                    swSyncTime.Restart();
                }
            }
        }

        public DateTime ServerDateTime
        {
            get
            {
                if (KnowServerTime == false)
                    return DateTime.MinValue;
                else
                    return DateTime.UtcNow.AddTicks(timeOffset);
            }
        }

        public long ServerTicks
        {
            get
            {
                if (KnowServerTime == false)
                    return 0;
                else
                    return DateTime.UtcNow.Ticks + timeOffset;
            }
        }
    }
}