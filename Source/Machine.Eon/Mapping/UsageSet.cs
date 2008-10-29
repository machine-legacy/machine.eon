using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class UsageSet
  {
    private readonly List<Usage> _usages = new List<Usage>();

    public IEnumerable<Node> All
    {
      get
      {
        foreach (Type node in this.Types) yield return node;
        foreach (Property node in this.Properties) yield return node;
        foreach (Method node in this.Methods) yield return node;
      }
    }

    public IEnumerable<Type> Types
    {
      get { return NodesOfType<Type, TypeName>(); }
    }

    public IEnumerable<Property> Properties
    {
      get { return NodesOfType<Property, PropertyName>(); }
    }

    public IEnumerable<Method> Methods
    {
      get { return NodesOfType<Method, MethodName>(); }
    }

    public void Add(Node node)
    {
      Add(node.Usage());
    }

    private void AddAll(IEnumerable<Usage> usages)
    {
      foreach (Usage usage in usages)
      {
        Add(usage);
      }
    }

    private void AddAll(UsageSet set)
    {
      AddAll(set._usages);
    }

    private void Add(Usage usage)
    {
      if (_usages.Contains(usage))
      {
        return;
      }
      _usages.Add(usage);
    }

    private IEnumerable<T> OfType<T>() where T : Usage
    {
      foreach (Usage usage in _usages)
      {
        if (usage is T)
        {
          yield return (T)usage;
        }
      }
    }

    private IEnumerable<TNode> NodesOfType<TNode, TName>()
      where TNode : Node
      where TName : NodeName
    {
      foreach (UsageByName<TNode, TName> usage in OfType<UsageByName<TNode, TName>>())
      {
        if (usage != null)
        {
          yield return usage.Node;
        }
      }
    }

    public static UsageSet Union(params UsageSet[] sets)
    {
      UsageSet union = new UsageSet();
      foreach (UsageSet set in sets)
      {
        union.AddAll(set);
      }
      return union;
    }

    public static UsageSet Union<T>(IEnumerable<T> uses) where T : IHaveUses
    {
      List<UsageSet> sets = new List<UsageSet>();
      foreach (T hasUses in uses)
      {
        sets.Add(hasUses.Uses);
      }
      return Union(sets.ToArray());
    }
  }
}