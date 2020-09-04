using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Engine.Framework;
using Engine.Network.Protocol;
using Schema.Protobuf.Message.Administrator;
using System.Linq;
using static Engine.Framework.Api;

namespace Application.Agent.Delegatables
{
    public class Server : Engine.Network.Protocol.Delegator<Server>.IDelegatable
    {
        private static Dictionary<long, (ConnectedAgentInfo, IDelegator)> delegators = new Dictionary<long, (ConnectedAgentInfo, IDelegator)>();


        internal static List<IDelegator> Get(string type)
        {
            List<IDelegator> list = new List<IDelegator>();
            lock (delegators)
            {
                foreach ((ConnectedAgentInfo Info, IDelegator Delegator) e in delegators.Values)
                {
                    if (e.Info.ServerTypes.Contains(type) == false) { continue; }
                    list.Add(e.Delegator);
                }
            }
            return list;
        }

        internal static IDelegator Get(string type, long address)
        {
            lock (delegators)
            {
                foreach ((ConnectedAgentInfo Info, IDelegator Delegator) e in delegators.Values)
                {
                    if (e.Info.ServerTypes.Contains(type) == false) { continue; }
                    if (e.Info.Address != address) { continue; }
                    return e.Delegator;
                }
            }
            return null;
        }

        internal static IDelegator Get(long address)
        {
            lock (delegators)
            {
                foreach ((ConnectedAgentInfo Info, IDelegator Delegator) e in delegators.Values)
                {
                    if (e.Info.Address == address)
                    {
                        return e.Delegator;
                    }
                }
            }
            Logger.Info($"Can't find Agent Address {address}");
            return null;
        }
     
        public static (ConnectedAgentInfo, IDelegator)[] Delegators
        {
            get {

                lock (delegators)
                {
                    return delegators.Values.ToArray();
                }
                
            }
        }

        public void OnConnect(IDelegator delegator, MemoryStream stream)
        {
            var info = new ConnectedAgentInfo();
            info.MergeFrom(stream);

            lock (delegators)
            {
                delegators.Remove(info.Address);
                delegators.Add(info.Address, (info, delegator));

                uint ip = (uint)info.Address;

                Engine.Framework.Api.Logger.Info($"OnConnect Agent Delegator.");
                Engine.Framework.Api.Logger.Info($"{info.ToJson()} - Ip : {Engine.Framework.Api.UInt32ToIPAddress(ip)}");

            }
        
        }

        public void OnDelegate(Engine.Network.Protocol.Delegator<Server>.Notifier notifier, int code, MemoryStream stream)
        {
            var callback = Engine.Network.Api.Binder(Singleton<Entities.Server>.Instance, notifier, code, stream);
            Singleton<Entities.Server>.Instance.PostMessage(callback);
        }
    }
}
