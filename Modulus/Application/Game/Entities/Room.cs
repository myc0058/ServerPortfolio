using Engine.Network.Protocol;
using Schema.Protobuf.Message.Game;
using System;using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using static Engine.Framework.Api;

namespace Application.Game.Entities{
    public partial class Room : Engine.Framework.Scheduler    {        public class Layer : Engine.Framework.Layer { }
        public Room() : base(Engine.Framework.Api.Singleton<Layer>.Instance)
        {

        }

        protected override void OnException(Exception e)
        {
            Console.WriteLine(e);        }

        public static Room Get(long id)        {            return Singleton<Engine.Framework.Container<long, Room>>.Instance.Get(id);
        }

        internal static void Remove(long id)        {            Singleton<Engine.Framework.Container<long, Room>>.Instance.Pop(id);        }

        public static int Count()
        {
            return Singleton<Engine.Framework.Container<long, Room>>.Instance.Count;
        }

        public static Room Create(long uid)
        {
            var room = Engine.Framework.New<Entities.Room>.Instantiate;
            room.UID = uid;
            Singleton<Engine.Framework.Container<long, Room>>.Instance.Add(uid, room);
            return room;
        }

        public static Room GetOrCreate(long id)        {            Room room = Get(id);            if (room == null)
            {
                room = Create(id);
            }            return room;        }

        protected override void OnSchedule(long deltaTicks)
        {
            
            Simulate(deltaTicks);

            StoreAndSendSnapShot(deltaTicks);

            SendRTT(deltaTicks);

            /*
            AutoStartFinish();
            */
        }

        private void AutoStartFinish()
        {
            switch (State)
            {
                case RoomState.Ready://auto start
                    {
                        TimeSpan diff = DateTime.UtcNow - CreateTime;
                        if (diff.TotalSeconds >= 5)
                        {
                            StartTime = DateTime.UtcNow;
                            State = RoomState.Playing;
                        }
                    }
                    break;
                case RoomState.Playing://auto finish
                    {
                        TimeSpan diff = DateTime.UtcNow - StartTime;
                        if (diff.TotalSeconds >= 5)
                        {
                            FinishTime = DateTime.UtcNow;
                            State = RoomState.Finish;
                            Close();
                        }
                    }
                    break;

            }
        }

        new public void Close()
        {
            if (Singleton<Room>.Instance != this)
            {
                base.Close();
            }
            else
            {
                if (UID != 0)
                {
                    Console.WriteLine($"Singleton Room UID != 0. UID : {UID}");
                }
            }
        }

        protected override void OnClose()
        {
            {
                foreach (var e in Users)
                {
                    if (e.Value.SendGameResult == false)
                    {
                        e.Value.Delegator?.Delegate(UID, e.Key, e.Value.MyGameResult);
                        e.Value.SendGameResult = true;
                    }
                }
            }

            {
                FinishGame msg = new FinishGame();
                msg.ResponseCode = Schema.Protobuf.Message.Enums.ResponseCode.Success;
                var arrUserPlayData = Users.ToArray();

                foreach (var item in arrUserPlayData)
                {
                    msg.Datas.Add(item.Value.MyGameResult.Data);
                }

                foreach (var e in Users)
                {
                    e.Value.Delegator?.Delegate(UID, e.Key, msg);
                }
            }
            

            Entities.Room.Remove(UID);
            Users.Clear();
        }

        public enum RoomState
        {
            None = -1,
            Ready = 0,
            Playing = 1,
            Finish = 2,
        }

        public long FirstTicks = DateTime.MinValue.Ticks;

        public DateTime CreateTime = DateTime.UtcNow;
        public DateTime StartTime = DateTime.MinValue;
        public DateTime FinishTime = DateTime.MinValue;

        public RoomState State { get; set; } = RoomState.Ready;
    }
}
