using Google.Protobuf.Collections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using static Engine.Framework.Api;

namespace Application.Lobby.Entities
{
    public partial class Match : Engine.Framework.Layer.Task//, Engine.Framework.IDelegatable
    {
        public class Layer : Engine.Framework.Layer {}

        public Match() : base(Singleton<Layer>.Instance)
        {
            
        }
    }
}
