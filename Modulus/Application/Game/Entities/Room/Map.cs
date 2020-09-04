using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Application.Game.Entities
{
    public partial class Room
    {
        public class PlaneMap
        {
            public Vector3 Center { get; set; } = Vector3.Zero;

            public float Width { get; set; } = 50f;

            public float Left { get { return Center.X - Width; } }
            public float Right { get { return Center.X + Width; } }
            public float Top { get { return Center.Z + Width; } }
            public float Bottom { get { return Center.Z - Width; } }

            public Schema.Protobuf.Message.Common.PlaneMap ToPacket()
            {
                var result = new Schema.Protobuf.Message.Common.PlaneMap();
                result.Center = new Schema.Protobuf.Message.Common.Vector3()
                {
                    X = this.Center.X,
                    Y = this.Center.Y,
                    Z = this.Center.Z,
                };
                result.Width = this.Width;
                return result;
            }
        }

        private PlaneMap Map { get; set; } = new PlaneMap();

    }
}
