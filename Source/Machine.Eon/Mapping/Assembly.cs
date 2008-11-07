using System;
using System.Collections.Generic;
using System.Linq;

namespace Machine.Eon.Mapping
{
  public class Assembly : Node, IAssembly
  {
    private readonly List<Namespace> _namespaces = new List<Namespace>();
    private readonly AssemblyKey _key;
    private bool _isDependency;

    public AssemblyKey Key
    {
      get { return _key; }
    }

    public IEnumerable<Namespace> Namespaces
    {
      get { return _namespaces; }
    }

    public IEnumerable<Type> Types
    {
      get { return from ns in this.Namespaces from type in ns.Types select type; }
    }

    public bool IsDependency
    {
      get { return _isDependency; }
      set { _isDependency = value; }
    }

    public Assembly(AssemblyKey key)
    {
      _key = key;
    }

    private Namespace FindOrCreateNamespace(NamespaceKey key)
    {
      foreach (Namespace ns in _namespaces)
      {
        if (ns.Key.Equals(key))
        {
          return ns;
        }
      }
      Namespace newNs = new Namespace(this, key);
      _namespaces.Add(newNs);
      return newNs;
    }

    public Type FindType(TypeKey key)
    {
      Namespace ns = FindOrCreateNamespace(key.Namespace);
      return ns.FindType(key);
    }

    public Type AddType(TypeKey key)
    {
      Namespace ns = FindOrCreateNamespace(key.Namespace);
      return ns.AddType(key);
    }

    public override string ToString()
    {
      return _key.ToString();
    }
  }
}
