using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Schema.Protobuf.Message.Administrator;
using Engine.Network.Protocol;
using Engine.Framework;
using static Engine.Framework.Api;

namespace Application.Agent.Delegatables
{
    public class Process : Engine.Network.Protocol.Delegator<Process>.IDelegatable
    {
        private static Dictionary<long, (ConnectedAgentInfo, IDelegator)> delegators = new Dictionary<long, (ConnectedAgentInfo, IDelegator)>();

        public void OnConnect(Engine.Network.Protocol.IDelegator delegator, MemoryStream stream)
        {
            var info = new ConnectedAgentInfo();
            info.MergeFrom(stream);

            lock (delegators)
            {
                Engine.Framework.Api.Logger.Info($"OnConnect Agent Process Delegator.");
                Engine.Framework.Api.Logger.Info($"{info.ToJson()}");
                delegators.Remove(delegator.UID);
                delegators.Add(delegator.UID, (info, delegator));
            }
        }

        public void OnDisconnect(IDelegator delegator)
        {
            lock (delegators)
            {
                delegators.Remove(delegator.UID);
            }
        }

        public static IDelegator Get(string type)
        {
            lock (delegators)
            {
                foreach (var e in delegators)
                {
                    if (e.Value.Item1.ServerTypes.Contains(type) == true)
                    {
                        return e.Value.Item2;
                    }
                }
            }
            return null;
        }

        public void OnDelegate(Engine.Network.Protocol.Delegator<Process>.Notifier notifier, int code, MemoryStream stream)
        {
            var callback = Engine.Network.Api.Binder(Singleton<Entities.Process>.Instance, notifier, code, stream);
            Singleton<Entities.Process>.Instance.PostMessage(callback);
        }
    }
}
