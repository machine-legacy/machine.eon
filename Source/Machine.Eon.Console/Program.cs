using System;
using System.Collections.Generic;

using Machine.Eon.Mapping;
using Machine.Eon.Mapping.Repositories.Impl;

namespace Machine.Eon.Console
{
  public class Program
  {
    public static void Main(string[] args)
    {
      Mapper mapper = new Mapper();
      mapper.Include(typeof(Program).Assembly.Location);
      mapper.Include(typeof(Mapper).Assembly.Location);

      foreach (Assembly assembly in Storage.InMemory.Assemblies.Values)
      {
        System.Console.WriteLine("Assembly: " + assembly);
      }

      System.Console.WriteLine("DONE");
      System.Console.ReadKey();
    }
  }
}
