using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Override.TestOverride
{
    [Engine.Framework.Attributes.Override]
    public class SecondOverride : Application.StandAlone.TestOverride
    {
        public static void Override()
        {
            Engine.Framework.New<Application.StandAlone.TestOverride>.Instance = Expression.Lambda<Func<Application.StandAlone.TestOverride>>(Expression.New(typeof(SecondOverride))).Compile();
        }
        public override void Func()
        {
            Console.WriteLine("SecondOverride Func");
            new SecondOverride2().Func();
        }

        public void NewSecondOverride2()
        {
            Console.WriteLine("NewSecondOverride2 Func");

            var ready = new Schema.Protobuf.Message.Action.Ready();
            Console.WriteLine(ready.GetType());


            var ta = new Schema.Protobuf.Message.Action.TestAddPacket();
            Console.WriteLine(ta.GetType());


            //new SecondOverride3().Func();
        }
    }
}
