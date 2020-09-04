//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace Application.StandAlone
//{
//    [Engine.Framework.Attributes.Override]
//    public class SecondOverride : TestOverride
//    {
//        public static void Override()
//        {
//            Engine.Framework.New<TestOverride>.Instance = Expression.Lambda<Func<TestOverride>>(Expression.New(typeof(SecondOverride))).Compile();
//        }
//        public override void Func()
//        {
//            Engine.Framework.Api.Logger.Info("TestOverride Func");
//            new SecondOverride2().Func();
//        }

//        public void NewSecondOverride2()
//        {
//            Engine.Framework.Api.Logger.Info("NewSecondOverride2 Func");
//        }
//    }
//}
