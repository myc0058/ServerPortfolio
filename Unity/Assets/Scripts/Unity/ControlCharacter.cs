using Schema.Protobuf;
using Schema.Protobuf.Message.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unity
{
    public class ControlCharacter : MonoBehaviour
    {
        public UnityEngine.Vector3 Position 
        { 
            get
            {
                return gameObject.transform.localPosition;
            }
            set
            {
                gameObject.transform.localPosition = value;
            }
        }

        public Room Room { get; set; } = null;

        public long DelTaTicks { get; set; } = 0;
        public UnityEngine.Vector3 Target { get; set; } = UnityEngine.Vector3.zero;

        private SortedDictionary<long, Room.Interporation> interporations = new SortedDictionary<long, Room.Interporation>();

        private long nowSeq = 0;
        private Room.Interporation nowInterporation = null;
        private long accDeltaTicks = 0;

        private bool changeNowInterporation(long deltaTicks)
        {
            if (interporations.Count < 1)
            {
                return false;
            }

            nowInterporation = interporations.First().Value;
            nowSeq = interporations.First().Key;
            interporations.Remove(nowSeq);
            accDeltaTicks = deltaTicks;
            this.Position = nowInterporation.orgPosition;

            if (accDeltaTicks > nowInterporation.deltaTicks)
            {
                changeNowInterporation(accDeltaTicks - nowInterporation.deltaTicks);
            }

            return true;
        }

        void Awake()
        {
        }

        void Update()
        {
            if (nowInterporation == null)
            {
                if (interporations.Count < 1)
                {
                    return;
                }

                changeNowInterporation(0);
                return;
            }

            accDeltaTicks += (long)(Time.deltaTime * 1000 * 10000);

            float percent = (float)accDeltaTicks / nowInterporation.deltaTicks;
            if (percent > 1.0f)
            {
                changeNowInterporation(accDeltaTicks - nowInterporation.deltaTicks);
                return;
            }

            this.Position = UnityEngine.Vector3.Lerp(nowInterporation.orgPosition, nowInterporation.targetPosition, percent);
        }

        internal void Move(Room.Interporation interporation)
        {
            interporations.Add(interporation.nowSeq, interporation);
        }
    }

}