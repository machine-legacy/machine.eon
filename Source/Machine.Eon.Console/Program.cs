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

      _log.Info("Nodes used by Machine.Eon.Console");

      var consoleNs = (from ns in qr.Namespaces where ns.Name.Name.Equals("Machine.Eon.Console") select ns).FirstOrDefault();
      var indirectlyUses = consoleNs.IndirectlyUses;

      _log.Info(indirectlyUses.Types.Count());

      foreach (RelativeUsage usage in from ofType in indirectlyUses.Types orderby ofType.Depth select ofType)
      {
        _log.Info(usage);
      }

      _log.Info(indirectlyUses.Methods.Count());

      return;
      /* 
      _log.Info("Nodes used by Program");

      var programType = (from type in qr.Types where type.Name.FullName.Equals("Machine.Eon.Console.Program") select type).FirstOrDefault();
      
      foreach (Node node in programType.IndirectlyUses)
      {
        _log.Info("  " + node);
      }

      _log.Info("Types in Machine");

      var types = from type in qr.Types
                  where type.Name.FullName.StartsWith("Machine")
                  select type
                  ;
      
      foreach (Type type in types)
      {
        _log.Info("  " + type);
      }
      
      _log.Info("Getters in Machine");

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
        _log.Info("  " + setter);
      }

      _log.Info("Types used by Namespace Machine.Eon.Console");

      var usedByConsole = from ns in qr.NamespacesNamed("Machine.Eon.Console")
                          from usage in ns.IndirectlyUses.Types
                          select usage
                          ;
      
      foreach (Type type in usedByConsole)
      {
        _log.Info("  " + type);
      }

      _log.Info("Types that have Node as a BaseType");

      var nodeTypes = from type in qr.Types
                      where type.BaseType != null && type.BaseType.Name.Equals(new TypeName(AssemblyName.Any, "Machine.Eon.Mapping.Node"))
                      select type
                      ;
      
      foreach (Type type in nodeTypes)
      {
        _log.Info("  " + type);
      }

      _log.Info("Types that are INode's");

      var inodeTypes = from type in qr.Types
                       where type.IsA(new TypeName(AssemblyName.Any, "Machine.Eon.Mapping.INode")) && !type.IsInterface
                       select type
                       ;
      
      foreach (Type type in inodeTypes)
      {
        _log.Info("  " + type);
      }

      _log.Info("Types that use System.String's");

      var systemString = (from type in qr.Types where type.Name.FullName.Equals("System.String") select type).FirstOrDefault();

      var usesString = from type in qr.Types
                       where type.IndirectlyUses.Types.Contains(systemString)
                       select type
                       ;
      
      foreach (Type type in usesString)
      {
        _log.Info("  " + type);
      }

      _log.Info("Nodes that are used by Test");

      var test = (from type in qr.Types where type.Name.FullName.Equals("Machine.Eon.Console.Test") select type).FirstOrDefault();
      var uses = from node in test.IndirectlyUses select node;

      foreach (Node type in uses)
      {
        _log.Info("  " + type);
      }

      _log.Info("Types that have Attributes");

      var typesWithAttributes = from type in qr.Types where type.Attributes.Count() > 0 select type;

      foreach (Type type in typesWithAttributes)
      {
        _log.Info("  " + type);
      }

      _log.Info("Methods that have Attributes");

      var methodsWithAttributes = from node in qr.Methods where node.Attributes.Count() > 0 select node;

      foreach (Method node in methodsWithAttributes)
      {
        _log.Info("  " + node);
      }
      */
    }
  }
  public class Test
  {
    public void Test1()
    {
      Test2();
    }
    public void Test2()
    {
      Test1();
    }
  }
}
