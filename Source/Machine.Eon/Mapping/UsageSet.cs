using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class UsageSet : IEnumerable<Node>
  {
    public static readonly UsageSet Empty = new UsageSet();
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
      Add(node.CreateUsage());
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
      where TNode : Node, INodeNamed<TName>
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

    public static UsageSet MakeFrom<T>(IEnumerable<T> nodes) where T : Node
    {
      UsageSet set = new UsageSet();
      foreach (T node in nodes)
      {
        set.Add(node);
      }
      return set;
    }

    public IndirectUses CreateIndirectUses()
    {
      List<Node> visited = new List<Node>();
      IndirectUses set = new IndirectUses();
      AddIndirectUses(0, visited, set);
      return set;
    }

    private void AddIndirectUses(Int32 depth, ICollection<Node> visited, IndirectUses set)
    {
      string prefix = new string(' ', depth * 4);
      // log4net.LogManager.GetLogger("DEBUG").Info(prefix + "In: " + depth + " Uses: " + _usages.Count);
      foreach (Type node in this.Types)
      {
        if (!visited.Contains(node))
        {
          log4net.LogManager.GetLogger("DEBUG").Info(prefix + "  Adding: " + node);
          visited.Add(node);
          set.Add(depth, node);
          node.DirectUsesAttributesInterfacesAndMethods.AddIndirectUses(depth + 1, visited, set);
        }
        else
        {
          // log4net.LogManager.GetLogger("DEBUG").Info(prefix + "  Already visited: " + node);
        }
      }
      foreach (Method node in this.Methods)
      {
        if (!visited.Contains(node))
        {
          log4net.LogManager.GetLogger("DEBUG").Info(prefix + "  Adding: " + node);
          visited.Add(node);
          set.Add(depth, node);
          node.DirectlyUses.AddIndirectUses(depth + 1, visited, set);
        }
        else
        {
          // log4net.LogManager.GetLogger("DEBUG").Info(prefix + "  Already visited: " + node);
        }
      }
    }

    #region IEnumerable<Node> Members
    public IEnumerator<Node> GetEnumerator()
    {
      return this.All.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return this.All.GetEnumerator();
    }
    #endregion

    public override string ToString()
    {
      return "UsageSet<" + _usages.Count + ">";
    }
  }
}