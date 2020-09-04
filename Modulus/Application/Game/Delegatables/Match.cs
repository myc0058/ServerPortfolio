using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Engine.Framework.Api;

namespace Application.Game.Delegatables
{
    public partial class Match : Engine.Network.Protocol.Delegator<Match>.IDelegatable
    {
        public static Engine.Network.Protocol.Delegator<Match> Instance { get; internal set; } = Singleton<Engine.Network.Protocol.Delegator<Match>>.Instance;

        public void OnConnect(Engine.Network.Protocol.IDelegator delegator, MemoryStream stream)
        {

        }


        public void OnDelegate(Engine.Network.Protocol.Delegator<Match>.Notifier notifier, int code, MemoryStream stream)
        {
            var match = Singleton<Entities.Match>.Instance;
            var callback = Engine.Network.Api.Binder(match, notifier, code, stream);

            match.PostMessage(callback);

        }
    }
}
