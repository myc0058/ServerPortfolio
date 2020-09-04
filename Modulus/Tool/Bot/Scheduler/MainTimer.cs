﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool.Bot.Entities;
using static Engine.Framework.Api;

namespace Tool.Bot.Scheduler
{
    public class MainTimer : Engine.Framework.Scheduler
    {
        public class Layer : Engine.Framework.Layer { }
        public MainTimer() : base(Singleton<Layer>.Instance) { }

        protected override void OnSchedule(long deltaTicks)
        {
            Protocol.BotClient.UpdateAll();
        }
    }
}
