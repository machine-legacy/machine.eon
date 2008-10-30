using System;
using System.Collections.Generic;
using System.Linq;

using Machine.Eon.Mapping;

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

      QueryRoot qr = mapper.ToQueryRoot();

      var types = from type in qr.Types
                  where type.Name.FullName.StartsWith("Machine")
                  select type
                  ;
      
      foreach (Type type in types)
      {
        _log.Info(type);
      }
      
      var getters = from type in qr.Types
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

      _log.Info("Types used by Namespace Machine.Eon.Console");

      var usedByConsole = from ns in qr.NamespacesNamed("Machine.Eon.Console")
                          from usage in ns.IndirectlyUses.Types
                          select usage
                          ;
      
      foreach (Type used in usedByConsole)
      {
        _log.Info(used);
      }

      _log.Info("Types that have Node as a BaseType");

      var nodeTypes = from type in qr.Types
                      where type.BaseType != null && type.BaseType.Name.Equals(new TypeName(AssemblyName.Any, "Machine.Eon.Mapping.Node"))
                      select type
                      ;
      
      foreach (Type type in nodeTypes)
      {
        _log.Info(type);
      }

      _log.Info("Types that are INode's");

      var inodeTypes = from type in qr.Types
                       where type.IsA(new TypeName(AssemblyName.Any, "Machine.Eon.Mapping.INode")) && !type.IsInterface
                       select type
                       ;
      
      foreach (Type type in inodeTypes)
      {
        _log.Info(type);
      }

      _log.Info("Types that use System.String's");

      var systemString = (from type in qr.Types where type.Name.FullName.Equals("System.String") select type).FirstOrDefault();

      var usesString = from type in qr.Types
                       where type.IndirectlyUses.Types.Contains(systemString)
                       select type
                       ;
      
      foreach (Type type in usesString)
      {
        _log.Info(type);
      }
    }
  }
}
