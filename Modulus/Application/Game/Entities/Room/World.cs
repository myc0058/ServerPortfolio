using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using static Engine.Framework.Api;

namespace Application.Game.Entities
{
    public partial class Room
    {
        public class World
        {
            public Dictionary<long, Character> Characters { get; set; } = null;
            public PlaneMap Map { get; set; } = null;
            public long Seq { get; set; } = 0;
            public long DeltaTicks { get; set; } = 0;

            public Schema.Protobuf.Message.Game.World ToPacket()
            {
                var result = new Schema.Protobuf.Message.Game.World();
                foreach (var item in Characters)
                {
                    result.Characters.Add(item.Value.ToPacket());
                }
                result.Map = Map.ToPacket();
                result.DeltaTicks = this.DeltaTicks;
                result.Seq = this.Seq;

                return result;
            }
        }

        private SortedDictionary<long, World> snapShots = new SortedDictionary<long, World>();
        private Stopwatch swSnapShot = new Stopwatch();

        public static int SnapShotInterval { get; set; } = 10;
        public static long SnapShotSeq = 1;

        public void StoreAndSendSnapShot(long deltaTicks)
        {

            if (swSnapShot.IsRunning == false)
            {
                swSnapShot.Start();
            }

            if (swSnapShot.Elapsed.TotalMilliseconds < 50)
            {
                return;
            }

            long elapsedTicks = swSnapShot.ElapsedTicks;

            swSnapShot.Restart();

            World world = new World();
            world.Characters = Characters.ToDictionary(entry => entry.Key,
                                                       entry => (Character)entry.Value.Clone());
            world.Map = new PlaneMap()
            {
                Center = Map.Center,
                Width = Map.Width,
            };
            world.DeltaTicks = elapsedTicks;
            world.Seq = SnapShotSeq;

            snapShots.Add(world.Seq, world);
            SnapShotSeq++;

            var worldPacket = world.ToPacket();

            foreach(var item in Users)
            {
                item.Value.Delegator.Delegate(UID, item.Key, worldPacket);
            }

            Logger.Info($"Store Snap Shot!! {elapsedTicks / 10000}");
        }

    }
}
