using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Engine.Framework.Api;

namespace Application.Lobby.Scheduler
{
    public class HeartBeat : Engine.Framework.Scheduler
    {
        public HeartBeat() : base(Singleton<Engine.Framework.Layer>.Instance) { }
        protected override void OnSchedule(long deltaTicks)
        {
            Protocol.Client.HeartBeat();
        }
    }
}
