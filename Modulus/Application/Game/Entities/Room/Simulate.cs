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
        private Stopwatch swSimulate = new Stopwatch();
        
        public void Simulate(long deltaTicks)
        {
            if (swSimulate.IsRunning == false)
            {
                swSimulate.Start();
            }

            if (swSimulate.ElapsedMilliseconds < 30)
            {
                return;
            }

            long elapsedTicks = swSimulate.ElapsedTicks;

            swSimulate.Restart();

            foreach(var item in Characters)
            {
                item.Value.Update();
            }
        }
    }
}
