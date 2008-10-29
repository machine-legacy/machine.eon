using System;
using System.Collections.Generic;

using Machine.Eon.Mapping;
using Machine.Eon.Mapping.Repositories.Impl;
using Machine.Eon.Querying;

namespace Machine.Eon.Console
{
  public class Querier
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(Querier));

    public QueryResult FindAll(Query query)
    {
      List<Node> nodes = new List<Node>();
      foreach (Assembly assembly in Storage.InMemory.Assemblies.Values)
      {
        if (query.Assemblies.Matches(assembly))
        {
          nodes.Add(assembly);
        }
        foreach (Namespace ns in assembly.Namespaces)
        {
          if (query.Namespaces.Matches(ns))
          {
            nodes.Add(ns);
          }
          foreach (Machine.Eon.Mapping.Type type in ns.Types)
          {
            if (query.Types.Matches(type))
            {
              nodes.Add(type);
            }
            foreach (Member member in type.Members)
            {
              if (query.Members.Matches(member))
              {
                nodes.Add(member);
              }
            }
          }
        }
      }
      return new QueryResult(nodes);
    }
  }
  public class QueryResult
  {
    private readonly ICollection<Node> _nodes;

    public ICollection<Node> Nodes
    {
      get { return _nodes; }
    }

    public QueryResult(ICollection<Node> nodes)
    {
      _nodes = nodes;
    }
  }
}