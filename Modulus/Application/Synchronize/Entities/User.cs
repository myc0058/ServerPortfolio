using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Engine.Framework.Api;

namespace Application.Synchronize.Entities
{
    public partial class User : Engine.Framework.Layer.Task
    {
        public class Layer : Engine.Framework.Layer { }
        public User() : base(Singleton<Layer>.Instance)
        {
            //Group = Data.Group.Get(Engine.Framework.Api.UniqueKey);
        }

        new public void Close()
        {
            Pop(UID);
            base.Close();
        }

        protected override void OnClose()
        {
            base.OnClose();
        }

        public static User Get(long idx)
        {
            User user = Singleton<Engine.Framework.Container<long, User>>.Instance.GetOrCreate(idx, (value) =>
            {
                value.UID = idx;
            });
            return user;
        }
        public static User Pop(long idx)
        {
            return Singleton<Engine.Framework.Container<long, User>>.Instance.Pop(idx);
        }

        public static bool Exist(long idx)
        {
            User user = Singleton<Engine.Framework.Container<long, User>>.Instance.Get(idx);
            return user != null;
        }
        
    }
}
