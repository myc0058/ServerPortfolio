using Engine.Network.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Engine.Framework.Api;

namespace Application.Common
{
    public class SyncUtil : Engine.Framework.Scheduler
    {
        public IDelegator Delegator { get; set; } = null;

        public SyncUtil() : base(Singleton<Engine.Framework.Layer>.Instance) { }

        protected override void OnSchedule(long deltaTicks)
        {
            if (Delegator == null)
            {
                return;
            }

            var msg = new Schema.Protobuf.Message.Administrator.GetUniqueKeySeed();
            
            Delegator.Delegate(this, 0, msg, (ret) =>
            {
                Engine.Framework.Api.Offset = ret.Seed;
                Engine.Framework.Api.Logger.Info($"GetUniqueKeySeed Success Offset : {Engine.Framework.Api.Offset}");
                this.Stop();
            },
            () =>
            {
                Engine.Framework.Api.Logger.Error("GetUniqueKeySeed Response Error");
            });
        }
    }
}
