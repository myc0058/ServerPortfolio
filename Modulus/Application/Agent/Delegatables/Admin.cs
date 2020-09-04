using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Application.Agent.Delegatables
{
    public class Admin : Engine.Network.Protocol.Delegator<Admin>.IDelegatable
    {
        public void OnConnect(Engine.Network.Protocol.IDelegator delegator, MemoryStream stream)
        { 
        
        }

        public void OnDelegate(Engine.Network.Protocol.Delegator<Admin>.Notifier notifier, int code, MemoryStream stream)
        {

        }
    }
}
