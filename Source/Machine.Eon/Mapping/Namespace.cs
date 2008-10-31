using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Namespace : Node, INamespace, IHaveIndirectUses
  {
    private readonly Assembly _assembly;
    private readonly NamespaceKey _key;
    private readonly List<Type> _types = new List<Type>();
    private readonly List<Type> _genericParameterTypes = new List<Type>();

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

    public Type FindType(TypeKey key)
    {
      foreach (Type type in _genericParameterTypes)
      {
        if (type.Key.Equals(key)) return type;
      }
      foreach (Type type in _types)
      {
        if (type.Key.Equals(key)) return type;
      }
      return null;
    }

    public Type AddType(TypeKey key)
    {
      Type type = new Type(this, key);
      if (key is GenericParameterTypeKey)
      {
        _genericParameterTypes.Add(type);
      }
      else
      {
        _types.Add(type);
      }
      return type;
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