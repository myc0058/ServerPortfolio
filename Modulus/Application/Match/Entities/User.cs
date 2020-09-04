using Engine.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Engine.Framework.Api;

namespace Application.Match.Entities
{
    public partial class User : Engine.Framework.Layer.Task, IComparable
    {
        public class Layer : Engine.Framework.Layer { }
        public User() : base(Singleton<Layer>.Instance)
        {
        }
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            User othreObj = obj as User;
            if (othreObj != null)
                return this.MMR.CompareTo(othreObj.MMR);
            else
                throw new ArgumentException("Object is not a User");
        }

        public long LobbyUID { get; set; }
        public int MMR { get; set; } = 0;

        
    }
}
