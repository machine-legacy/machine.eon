using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class GenericParameterTypeName : TypeKey
  {
    public GenericParameterTypeName(AssemblyKey assemblyKey, string name)
      : base(assemblyKey, name)
    {
    }
  }
  public class TypeKey : NodeKey
  {
    public static readonly TypeKey None = new TypeKey(AssemblyKey.None, String.Empty);
    public static readonly TypeKey Any = new TypeKey(AssemblyKey.Any, "*");
    private readonly NamespaceKey _namespaceKey;
    private readonly string _fullName;

    public AssemblyKey AssemblyKey
    {
      get { return _namespaceKey.AssemblyKey; }
    }

    public string FullName
    {
      get { return _fullName; }
    }

    public string Name
    {
      get
      {
        if (_fullName.LastIndexOf('.') < 0)
        {
          return _fullName;
        }
        return _fullName.Substring(_fullName.LastIndexOf('.') + 1);
      }
    }

    public NamespaceKey Namespace
    {
      get { return _namespaceKey; }
    }

    public TypeKey(AssemblyKey assemblyKey, string name)
    {
      if (name.LastIndexOf('.') > 0)
      {
        _namespaceKey = new NamespaceKey(assemblyKey, name.Substring(0, name.LastIndexOf('.')));
      }
      else
      {
        _namespaceKey = new NamespaceKey(assemblyKey);
      }
      _fullName = name;
    }

    public override bool Equals(object obj)
    {
      if (obj is TypeKey)
      {
        if (ReferenceEquals(Any, obj) || ReferenceEquals(Any, this)) return true;
        return ((TypeKey)obj).Name.Equals(this.Name) && ((TypeKey)obj).Namespace.Equals(this.Namespace);
      }
      return false;
    }

    public static bool operator ==(TypeKey n1, TypeKey n2)
    {
      return Equals(n1, n2);
    }

    public static bool operator !=(TypeKey n1, TypeKey n2)
    {
      return !Equals(n1, n2);
    }

    public override Int32 GetHashCode()
    {
      return _fullName.GetHashCode() ^ _namespaceKey.GetHashCode();
    }

    public override string ToString()
    {
      if (String.IsNullOrEmpty(_fullName))
      {
        return "Type<Null>";
      }
      return "Type<" + _fullName + ">";
    }
  }
}