using Schema.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Schema.Protobuf
{
    public class Room
    {
        public class Character
        {
            public long Idx { get; set; }
            public Vector3 Position { get; set; }
            public Vector3 Direction { get; set; }

            public float Speed { get; set; }

            public Character(Schema.Protobuf.Message.Common.Character Character)
            {
                this.Idx = Character.Idx;
                this.Position = new Vector3()
                {
                    x = Character.Position.X,
                    y = Character.Position.Y,
                    z = Character.Position.Z
                };
                this.Direction = new Vector3()
                {
                    x = Character.Direction.X,
                    y = Character.Direction.Y,
                    z = Character.Direction.Z
                };
                this.Speed = Character.Speed;
            }
        }

        public long GameServerID { get; set; } = 0;

        public long RoomId { get; set; } = 0;

        public Unity.Room UIRoomComp { get; set; } = null;

        public Dictionary<long, Character> Characters { get; set; } = new Dictionary<long, Character>();

        public void Clear()
        {
            GameServerID = 0;
            RoomId = 0;
            Characters.Clear();
            UIRoomComp = null;
        }

        public bool IsValid()
        {
            return GameServerID != 0;
        }
    }

}