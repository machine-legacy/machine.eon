using System;
using System.Collections.Generic;

using Machine.Eon.Mapping;

namespace Machine.Eon.Console
{
  public class Selector
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(Querier));

    public SelectResult Select(QueryResult qr)
    {
      List<Node> nodes = new List<Node>();
      return new SelectResult(nodes);
    }
  }
  public class SelectResult
  {
    private readonly ICollection<Node> _nodes;

    public ICollection<Node> Nodes
    {
      get { return _nodes; }
    }

    public SelectResult(ICollection<Node> nodes)
    {
      _nodes = nodes;
    }
  }
}