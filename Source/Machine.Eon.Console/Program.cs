using System;
using System.Collections.Generic;

using Machine.Eon.Mapping;
using Machine.Eon.Querying;

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

      Querier querier = new Querier();
      querier.FindAll(new Query()).Print(_log);
      querier.FindAll(new Query() { Types = new NoneCondition() }).Print(_log);
    }
  }
  public static class Printers
  {
    public static void Print(this QueryResult qr, log4net.ILog log)
    {
      foreach (Node node in qr.Nodes)
      {
        log.Info(node);
      }
    }
  }
}
