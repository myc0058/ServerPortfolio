using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity
{
    public static class Extensions
    {
        public static Schema.Protobuf.Message.Common.Vector3 ToPacket(this UnityEngine.Vector3 value)
        {
            var result = new Schema.Protobuf.Message.Common.Vector3()
            {
                X = value.x,
                Y = value.y,
                Z = value.z,
            };
            
            return result;

        }

        public static UnityEngine.Vector3 ToUnityVector3(this Schema.Protobuf.Message.Common.Vector3 value)
        {
            var result = new UnityEngine.Vector3()
            {
                x = value.X,
                y = value.Y,
                z = value.Z,
            };

            return result;

        }
    }
}
