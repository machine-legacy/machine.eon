using System;
using System.Collections.Generic;
using Machine.Eon.Mapping;

namespace Machine.Eon.Console
{
  public class Program
  {
    public static void Main(string[] args)
    {
      Mapper mapper = new Mapper();
      mapper.Include(typeof(Program).Assembly.Location);
      mapper.Include(typeof(Mapper).Assembly.Location);
      System.Console.WriteLine("DONE");
      System.Console.ReadKey();
    }
  }
}
