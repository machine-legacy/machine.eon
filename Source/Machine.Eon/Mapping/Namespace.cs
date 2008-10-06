using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Namespace
  {
    private readonly NamespaceName _name;
    private readonly List<Type> _types = new List<Type>();

    public NamespaceName Name
    {
      get { return _name; }
    }

    public IEnumerable<Type> Types
    {
      get { return _types; }
    }

    public Namespace(NamespaceName name)
    {
      _name = name;
    }

    public Type FindOrCreateType(TypeName name)
    {
      if (!name.Namespace.Equals(_name)) throw new ArgumentException("name");
      foreach (Type type in _types)
      {
        if (type.Name.Equals(name))
        {
          return type;
        }
      }
      Type newType = new Type(name);
      _types.Add(newType);
      return newType;
    }
  }
}