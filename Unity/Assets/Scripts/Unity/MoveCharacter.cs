using Schema.Protobuf;
using Schema.Protobuf.Message.Common;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity
{
    public class MoveCharacter : MonoBehaviour
    {
        public float Speed { get; set; } = 1.0f;

        UnityEngine.Vector3 moveDirection = UnityEngine.Vector3.zero;
        UnityEngine.Vector3 oldMoveDirection = UnityEngine.Vector3.zero;

        public long Idx { get; set; } = -1;

        public User User { get; set; } = null;

        public Room Room { get; set; } = null;

        void Start()
        {
        }

        void Update()
        {
            if (Idx != DropdownEvent.SelectedIdx)
            {
                return;
            }

            UnityEngine.Vector3 direction = UnityEngine.Vector3.zero;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                direction += new UnityEngine.Vector3(0, 0, 1);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                direction += new UnityEngine.Vector3(0, 0, -1);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                direction += new UnityEngine.Vector3(-1, 0, 0);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                direction += new UnityEngine.Vector3(1, 0, 0);
            }

            direction = direction.normalized;
            moveDirection = direction * Speed;

            //moveDirection.y -= gravity;
            gameObject.transform.localPosition += moveDirection * Time.deltaTime;

            if (oldMoveDirection != moveDirection)
            {
                var msg = new Schema.Protobuf.Message.Game.Move();
                msg.Idx = User.Idx;
                msg.Position = gameObject.transform.localPosition.ToPacket();
                msg.Direction = direction.ToPacket();
                msg.Speed = Speed;
                msg.Ticks = User.ServerTicks;
                User.Client?.Notify(msg);
            }

            oldMoveDirection = moveDirection;
        }

        public void Move(Room.Interporation interporation)
        {
            /* 내위치 보정 코드 넣어야 함
            Position = character.Position.ToUnityVector3();
            Direction = character.Direction.ToUnityVector3();
            Speed = character.Speed;
            */
        }
    }

}