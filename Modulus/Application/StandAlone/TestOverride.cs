using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.StandAlone
{
    public class TestOverride
    {
        public virtual void Func()
        {
            Engine.Framework.Api.Logger.Info("TestOverride Func");
        }
    }
}
