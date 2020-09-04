using System;
using static Engine.Framework.Api;

namespace Engine.Framework
{
    public class Scheduler : Engine.Framework.Layer.Task
    {
        public Scheduler(Layer layer) : base(layer)
        {
        }


        public Scheduler() : base(Singleton<Engine.Framework.Layer>.Instance) {
        
        }
	
        //new public bool PostMessage(Callback callback)
        //{
        //    return false;
        //}


        internal protected virtual void OnSchedule(long deltaTicks) { }

        public void Run(int millisecond)
        {
            if (Next != 0) { return; }
            DateTime now = DateTime.UtcNow;

            Next = now.AddMilliseconds(millisecond).Ticks;
            Start = now.Ticks;
            Pre = now.Ticks;

            interval = millisecond;
            layer.Schedule(this);
            IsPause = false;
        }

        public void Stop()
        {
            interval = -1;
        }

        public void Pause()
        {
            IsPause = true;
        }

        public void Resume()
        {
            IsPause = false;
        }

        internal int interval = -1;
        internal long Next = 0;
        internal long Pre = 0;
        public bool IsPause { get; set; } = false;
        public long Start = 0;
	}
}
