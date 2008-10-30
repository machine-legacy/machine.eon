using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class IndirectUses : IEnumerable<RelativeUsage>
  {
    private readonly List<RelativeUsage> _usages = new List<RelativeUsage>();
    private readonly Dictionary<Node, RelativeUsage> _byNode = new Dictionary<Node, RelativeUsage>();

    public UsageSet Nodes
    {
      get
      {
        UsageSet set = new UsageSet();
        foreach (RelativeUsage usage in _usages)
        {
          set.Add(usage.Node);
        }
        return set;
      }
    }

    public void Add(Int32 depth, Node node)
    {
      if (_byNode.ContainsKey(node))
      {
        return;
      }
      RelativeUsage usage = new RelativeUsage(depth, node);
      _byNode[node] = usage;
      _usages.Add(usage);
    }

    public IEnumerable<RelativeUsage> Properties
    {
      get { return OfType<Property>(); }
    }

    public IEnumerable<RelativeUsage> Fields
    {
      get { return OfType<Field>(); }
    }

    public IEnumerable<RelativeUsage> Methods
    {
      get { return OfType<Method>(); }
    }

    public IEnumerable<RelativeUsage> Types
    {
      get { return OfType<Type>(); }
    }

    private IEnumerable<RelativeUsage> OfType<T>() where T : Node
    {
      foreach (RelativeUsage usage in _usages)
      {
        if (usage.Node is T)
        {
          yield return usage;
        }
      }
    }

    #region IEnumerable<RelativeUsage> Members
    public IEnumerator<RelativeUsage> GetEnumerator()
    {
      return _usages.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return _usages.GetEnumerator();
    }
    #endregion
  }
  public static class UsageMappings
  {
    public static IEnumerable<Type> ExtractTypes(this IEnumerable<RelativeUsage> usages) 
    {
      foreach (RelativeUsage usage in usages)
      {
        yield return (Type)usage.Node;
      }
    }
    
    public static IEnumerable<Method> ExtractMethods(this IEnumerable<RelativeUsage> usages) 
    {
      foreach (RelativeUsage usage in usages)
      {
        yield return (Method)usage.Node;
      }
    }
  }
}