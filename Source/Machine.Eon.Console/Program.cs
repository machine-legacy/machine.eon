using System;
using System.Collections.Generic;
using System.Linq;

using Machine.Eon.Mapping;
using Machine.Eon.Mapping.Repositories.Impl;

using Type = Machine.Eon.Mapping.Type;

namespace Machine.Eon.Console
{
  public class Program
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(Program));

    public static void Main(string[] args)
    {
      log4net.Config.XmlConfigurator.Configure();

      Mapper mapper = new Mapper();
      mapper.Include(typeof(Program).Assembly.Location);
      mapper.Include(typeof(Mapper).Assembly.Location);

      ICollection<Assembly> assemblies = Storage.InMemory.Assemblies.Values;

      var types = from a in assemblies 
                  from ns in a.Namespaces
                  from type in ns.Types
                  where type.Name.FullName.StartsWith("Machine")
                  select type
                  ;
      
      foreach (Type type in types)
      {
        _log.Info(type);
      }
      
      var getters = from a in assemblies 
                    from ns in a.Namespaces
                    from type in ns.Types
                    from method in type.Methods
                    where
                      type.Name.FullName.StartsWith("Machine")
                      &&
                      method.IsGetter
                    select method
                    ;
      
      foreach (Method setter in getters)
      {
        _log.Info(setter);
      }
    }
  }
}
