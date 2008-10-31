using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class NamespaceKey : NodeKey
  {
    public static readonly NamespaceKey None = new NamespaceKey(AssemblyKey.None, String.Empty);
    public static readonly NamespaceKey Any = new NamespaceKey(AssemblyKey.Any, "*");
    private readonly AssemblyKey _assemblyKey;
    private readonly string _name;

    public AssemblyKey AssemblyKey
    {
      get { return _assemblyKey; }
    }

    public string Name
    {
      get { return _name; }
    }

    public NamespaceKey(AssemblyKey assemblyKey)
      : this(assemblyKey, String.Empty)
    {
    }

    public NamespaceKey(AssemblyKey assemblyKey, string name)
    {
      _assemblyKey = assemblyKey;
      _name = name;
    }

    public override bool Equals(object obj)
    {
      if (obj is NamespaceKey)
      {
        if (ReferenceEquals(Any, obj) || ReferenceEquals(Any, this)) return true;
        return ((NamespaceKey)obj).AssemblyKey.Equals(this.AssemblyKey) && ((NamespaceKey)obj).Name.Equals(this.Name);
      }
      return false;
    }

    public static bool operator ==(NamespaceKey n1, NamespaceKey n2)
    {
      return Equals(n1, n2);
    }

    public static bool operator !=(NamespaceKey n1, NamespaceKey n2)
    {
      return !Equals(n1, n2);
    }

    public override Int32 GetHashCode()
    {
      return _assemblyKey.GetHashCode() ^ _name.GetHashCode();
    }

    public override string ToString()
    {
      if (String.IsNullOrEmpty(_name))
      {
        return "Ns<Null>";
      }
      return "Ns<" + _name + ">";
    }
  }
}