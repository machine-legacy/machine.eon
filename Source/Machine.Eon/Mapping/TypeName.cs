using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class TypeName
  {
    public static readonly TypeName None = new TypeName(String.Empty);
    private readonly string _name;

    public string Name
    {
      get { return _name; }
    }

    public NamespaceName Namespace
    {
      get
      {
        if (_name.LastIndexOf('.') < 0)
        {
          return NamespaceName.None;
        }
        return new NamespaceName(_name.Substring(0, _name.LastIndexOf('.') - 1));
      }
    }

    public TypeName(string name)
    {
      _name = name;
    }

    public override bool Equals(object obj)
    {
      if (obj is TypeName)
      {
        return ((TypeName)obj).Name.Equals(this.Name);
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
      return _name.GetHashCode();
    }

    public override string ToString()
    {
      if (String.IsNullOrEmpty(_name))
      {
        return "Type<Null>";
      }
      return _name;
    }
  }
}