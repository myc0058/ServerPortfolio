using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Engine.Database.Attributes
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
  public class Query : Attribute
  {
    private Type provider;
    public Type Provider { get { return provider; } }

    public Query(Type provider = null) {
      this.provider = provider;

    }

    static public void StartUp() {

      var classes = (from asm in AppDomain.CurrentDomain.GetAssemblies()
                     from type in asm.GetTypes()
                     where type.IsClass
                     select type);


      foreach (var c in classes) {

        

        foreach (var attribute in c.GetCustomAttributes(false)) {


          var query = attribute as Engine.Database.Attributes.Query;

          if (query != null) {

            Engine.Framework.Api.Logger.Info("Resist DB - " + query.provider + " " + c.Namespace + ":" + c.Name);

						//Engine.Database.Management.Provider driver = null;
						//if (query.provider == typeof(Engine.Database.Management.Relational.MySql)) {
						//	driver = Engine.Database.Api.Db(query.provider);
						//}


            //if (driver == null) { return; }

						//foreach (var m in c.GetMethods()) {

              
						//	var ma = m.GetCustomAttribute(typeof(Engine.Database.Attributes.Query)) as Engine.Database.Attributes.Query;
						//	if (ma == null) { continue; }

						//	string procedure = (c.Name + "." + m.Name);
						//	var callback = (Engine.Database.Management.Provider.ProcedureOld)Delegate.CreateDelegate(typeof(Engine.Database.Management.Provider.ProcedureOld), m);
						//	Engine.Database.Management.Provider.Add(procedure, callback);

						//}
          }

        }
      }

    }
  }
}
