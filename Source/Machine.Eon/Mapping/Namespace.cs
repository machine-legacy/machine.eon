using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Namespace : Node, INamespace, IHaveIndirectUses
  {
    private readonly Assembly _assembly;
    private readonly NamespaceKey _key;
    private readonly List<Type> _types = new List<Type>();

    public Assembly Assembly
    {
      get { return _assembly; }
    }

    public NamespaceKey Key
    {
      get { return _key; }
    }

    public IEnumerable<Type> Types
    {
      get { return _types; }
    }

    public Namespace(Assembly assembly, NamespaceKey key)
    {
      _assembly = assembly;
      _key = key;
    }

    public Type FindOrCreateType(TypeKey key)
    {
      if (!key.Namespace.Equals(_key)) throw new ArgumentException("name");
      Type type = FindType(key);
      if (type != null)
      {
        return type;
      }
      type = new Type(this, key);
      _types.Add(type);
      return type;
    }

    private Type FindType(TypeKey key)
    {
      foreach (Type type in _types)
      {
        if (type.Key.Equals(key))
        {
          return type;
        }
      }
      return null;
    }

    public override string ToString()
    {
      return _key.ToString();
    }

    public IndirectUses IndirectlyUses
    {
      get { return UsageSet.MakeFrom(_types).CreateIndirectUses(); }
    }
  }
}