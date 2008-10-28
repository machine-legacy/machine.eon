using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class PropertyName : MemberName
  {
    public static readonly PropertyName None = new PropertyName(TypeName.None, String.Empty);

    public PropertyName(TypeName typeName, string name)
      : base(typeName, name)
    {
    }
  }
  public class MethodName : MemberName
  {
    public static readonly MethodName None = new MethodName(TypeName.None, String.Empty);

    public MethodName(TypeName typeName, string name)
      : base(typeName, name)
    {
    }
  }
  public abstract class MemberName : NodeName
  {
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

    protected MemberName(TypeName typeName, string name)
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

    public static bool operator ==(MemberName n1, MemberName n2)
    {
      return Equals(n1, n2);
    }

    public static bool operator !=(MemberName n1, MemberName n2)
    {
      return !Equals(n1, n2);
    }

    public override Int32 GetHashCode()
    {
      return _typeName.GetHashCode() ^ _name.GetHashCode();
    }

    public override string ToString()
    {
      if (String.IsNullOrEmpty(_name))
      {
        return "Member<Null>";
      }
      return "Member<" + _typeName + "->" + _name + ">";
    }
  }
}