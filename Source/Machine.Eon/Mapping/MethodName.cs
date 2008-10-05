using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class MethodName
  {
    public static readonly MethodName None = new MethodName(TypeName.None, String.Empty);
    private readonly TypeName _typeName;
    private readonly string _name;

    public TypeName TypeName
    {
      get { return _typeName; }
    }

    public string Name
    {
      get { return _name; }
    }

    public MethodName(TypeName typeName, string name)
    {
      _typeName = typeName;
      _name = name;
    }

    public override bool Equals(object obj)
    {
      MethodName other = obj as MethodName;
      if (other != null)
      {
        return other.TypeName.Equals(this.TypeName) && other.Name.Equals(this.Name);
      }
      return false;
    }

    public override Int32 GetHashCode()
    {
      return _typeName.GetHashCode() ^ _name.GetHashCode();
    }

    public override string ToString()
    {
      if (String.IsNullOrEmpty(_name))
      {
        return "Method<Null>";
      }
      return _name;
    }
  }
}