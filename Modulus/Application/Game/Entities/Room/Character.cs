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
        public class Character : ICloneable
        {
            public long Idx { get; set; } = 0;
            public Vector3 Position { get; set; } = Vector3.Zero;
            public Vector3 Direction { get; set; } = Vector3.Zero;
            public float Speed { get; set; } = 1.0f;

            private User user = null;

            private long lastMoveTick = 0;

            public Character(User user)
            {
                this.user = user;
            }
            
            public void Move(Schema.Protobuf.Message.Game.Move move)
            {
                var now = DateTime.UtcNow.Ticks - user.AvgRTT / 2;

                //움직일당시 포지션 저장하기
                Position = new Vector3(move.Position.X, move.Position.Y, move.Position.Z);

                //이전 움직임 계산하기
                Position += Direction * (now - lastMoveTick) / 10000 / 1000 * Speed;

                //현재 움직임 저장하기
                Direction = new Vector3(move.Direction.X, move.Direction.Y, move.Direction.Z);
                Speed = move.Speed;

                lastMoveTick = now;
            }

            public void Update()
            {
                var now = DateTime.UtcNow.Ticks - (user.AvgRTT / 2);

                //이전 움직임 계산하기
                Position += Direction * (now - lastMoveTick) / 10000 / 1000 * Speed;

                lastMoveTick = now;
            }

            public object Clone()
            {
                return new Character(this.user)
                {
                    Idx = this.Idx,
                    Position = this.Position,
                    Direction = this.Direction,
                    Speed = this.Speed,
                };
            }

            public Schema.Protobuf.Message.Common.Character ToPacket()
            {
                var character = new Schema.Protobuf.Message.Common.Character();
                character.Idx = Idx;
                character.Position = new Schema.Protobuf.Message.Common.Vector3()
                {
                    X = this.Position.X,
                    Y = this.Position.Y,
                    Z = this.Position.Z
                };
                character.Direction = new Schema.Protobuf.Message.Common.Vector3()
                {
                    X = this.Direction.X,
                    Y = this.Direction.Y,
                    Z = this.Direction.Z
                };
                character.Speed = this.Speed;
                return character;
            }
        }

        public Dictionary<long, Character> Characters = new Dictionary<long, Character>();
    }
}
