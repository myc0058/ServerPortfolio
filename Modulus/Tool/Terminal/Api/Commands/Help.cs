using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Engine.Framework.Api;

namespace Tool.Terminal
{
    public static partial class Api
    {
        public static partial class Command
        {
            public static bool Help(Engine.Framework.INotifier notifer, Engine.Network.Protocol.Terminal.Message msg)
            {

                using (var stream = typeof(Tool.Terminal.Program).Assembly.GetManifestResourceStream("Tool.Terminal.Help.txt"))
                {
                    var sr = new StreamReader(stream);
                    var txt = sr.ReadToEnd();
                    Engine.Framework.Api.Logger.Info(txt);
                }

                return true;
            }
        }

    }

}
