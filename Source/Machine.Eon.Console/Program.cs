using System;
using System.Collections.Generic;

using Machine.Eon.Mapping;
using Machine.Eon.Mapping.Repositories.Impl;
using Machine.Eon.Querying;

namespace Machine.Eon.Console
{
  public class Program
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(Program));

    public static void Main(string[] args)
    {
      log4net.Config.XmlConfigurator.Configure();

      Condition condition1 = new AnyNamespaceCondition();
      Condition condition2 = new SpecificNamespaceCondition(new NamespaceName(AssemblyName.None, ""));

      Mapper mapper = new Mapper();
      mapper.Include(typeof(Program).Assembly.Location);
      mapper.Include(typeof(Mapper).Assembly.Location);

      foreach (Assembly assembly in Storage.InMemory.Assemblies.Values)
      {
        _log.Info("Assembly: " + assembly);
        foreach (Namespace ns in assembly.Namespaces)
        {
          foreach (Machine.Eon.Mapping.Type type in ns.Types)
          {
            _log.Info("  Type: " + type);
            foreach (Member member in type.Members)
            {
              _log.Info("    Member: " + member);
            }
          }
        }
      }

      System.Console.WriteLine("DONE");
      System.Console.ReadKey();
    }
  }
}
