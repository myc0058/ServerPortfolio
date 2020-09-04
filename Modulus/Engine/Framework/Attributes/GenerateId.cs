using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GenerateId : Attribute
    {

        static public void StartUp()
        {

            var classes = (from asm in AppDomain.CurrentDomain.GetAssemblies()
                           from type in asm.GetTypes()
                           where type.IsClass
                           select type);


            foreach (var c in classes)
            {

                try
                {
                    foreach (var attribute in c.GetCustomAttributes(false))
                    {

                        var initialize = attribute as Engine.Framework.Attributes.GenerateId;
                        if (initialize != null)
                        {

                            c.GetMethod("GenerateId").Invoke(null, null);

                        }


                    }
                }
                catch
                {

                }
            }

        }
    }
}
