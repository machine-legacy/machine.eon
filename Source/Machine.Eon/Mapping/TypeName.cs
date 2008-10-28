using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class GenericParameterTypeName : TypeName
  {
    public GenericParameterTypeName(AssemblyName assemblyName, string name)
      : base(assemblyName, name)
    {
    }
  }
  public class TypeName : NodeName
  {
    public static readonly TypeName None = new TypeName(AssemblyName.None, String.Empty);
    private readonly AssemblyName _assemblyName;
    private readonly string _fullName;

    public AssemblyName AssemblyName
    {
      get { return _assemblyName; }
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

    public NamespaceName Namespace
    {
      get
      {
        if (_fullName.LastIndexOf('.') < 0)
        {
          return NamespaceName.None;
        }
        return new NamespaceName(_fullName.Substring(0, _fullName.LastIndexOf('.')));
      }
    }

    public TypeName(AssemblyName assemblyName, string name)
    {
      _assemblyName = assemblyName;
      _fullName = name;
    }

    public override bool Equals(object obj)
    {
      if (obj is TypeName)
      {
        return ((TypeName)obj).FullName.Equals(this.FullName);
      }
      return false;
    }

    public static bool operator ==(TypeName n1, TypeName n2)
    {
      return Equals(n1, n2);
    }

    public static bool operator !=(TypeName n1, TypeName n2)
    {
      return !Equals(n1, n2);
    }

    public override Int32 GetHashCode()
    {
      return _fullName.GetHashCode();
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