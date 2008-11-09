using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class UsageSet : IEnumerable<Node>
  {
    public static readonly UsageSet Empty = new UsageSet();
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(UsageSet));
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
      get { return NodesOfType<Type, TypeKey>(); }
    }

    public IEnumerable<Property> Properties
    {
      get { return NodesOfType<Property, PropertyKey>(); }
    }

    public IEnumerable<Method> Methods
    {
      get { return NodesOfType<Method, MethodKey>(); }
    }

    public void Add(Node node)
    {
      Add(node.CreateUsage());
    }

    public UsageSet RemoveReferencesToType(Type type)
    {
      UsageSet set = new UsageSet();
      foreach (Usage usage in _usages)
      {
        if (!usage.IsForType(type))
        {
          set.Add(usage);
        }
      }
      return set;
    }

    public IEnumerator<Node> GetEnumerator()
    {
      return this.All.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return this.All.GetEnumerator();
    }

    public override string ToString()
    {
      return "UsageSet<" + _usages.Count + ">";
    }

    public static UsageSet Union(params UsageSet[] sets)
    {
      UsageSet union = new UsageSet();
      foreach (UsageSet set in sets)
      {
        union.Add(set);
      }
      return union;
    }

    public static UsageSet MakeFrom<T>(IEnumerable<T> nodes) where T : Node
    {
      UsageSet set = new UsageSet();
      foreach (T node in nodes)
      {
        set.Add(node);
      }
      return set;
    }

    public void Add(UsageSet set)
    {
      AddAll(set._usages);
    }

    private void AddAll(IEnumerable<Usage> usages)
    {
      foreach (Usage usage in usages)
      {
        Add(usage);
      }
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
      where TNode : Node, IKeyedNode<TName>
      where TName : NodeKey
    {
      foreach (UsageByKey<TNode, TName> usage in OfType<UsageByKey<TNode, TName>>())
      {
        if (usage != null && !(usage.Node.Key is GenericParameterTypeKey))
        {
          yield return usage.Node;
        }
      }
    }
  }
}