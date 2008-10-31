using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class PropertyKey : MemberKey
  {
    public static readonly PropertyKey None = new PropertyKey(TypeKey.None, String.Empty);
    public static readonly PropertyKey Any = new PropertyKey(TypeKey.Any, "*");

    public PropertyKey(TypeKey typeKey, string name)
      : base(typeKey, name)
    {
    }
  }
  public class MethodKey : MemberKey
  {
    public static readonly MethodKey None = new MethodKey(TypeKey.None, String.Empty);
    public static readonly MethodKey Any = new MethodKey(TypeKey.Any, "*");

    public MethodKey(TypeKey typeKey, string name)
      : base(typeKey, name)
    {
    }
  }
  public class FieldKey : MemberKey
  {
    public static readonly FieldKey None = new FieldKey(TypeKey.None, String.Empty);
    public static readonly FieldKey Any = new FieldKey(TypeKey.Any, "*");

    public FieldKey(TypeKey typeKey, string name)
      : base(typeKey, name)
    {
    }
  }
  public abstract class MemberKey : NodeKey
  {
    private readonly TypeKey _typeKey;
    private readonly string _name;

    public TypeKey TypeKey
    {
      get { return _typeKey; }
    }

    public string Name
    {
      get { return _name; }
    }

    protected MemberKey(TypeKey typeKey, string name)
    {
      _typeKey = typeKey;
      _name = name;
    }

    public override bool Equals(object obj)
    {
      MemberKey other = obj as MemberKey;
      if (other != null)
      {
        return other.TypeKey.Equals(this.TypeKey) && other.Name.Equals(this.Name);
      }
      return false;
    }

    public static bool operator ==(MemberKey n1, MemberKey n2)
    {
      return Equals(n1, n2);
    }

    public static bool operator !=(MemberKey n1, MemberKey n2)
    {
      return !Equals(n1, n2);
    }

    public override Int32 GetHashCode()
    {
      return _typeKey.GetHashCode() ^ _name.GetHashCode();
    }

    public override string ToString()
    {
      if (String.IsNullOrEmpty(_name))
      {
        return GetType().Name.Replace("Key", "") +  "<Null>";
      }
      return GetType().Name.Replace("Key", "") + "<" + _typeKey + "->" + _name + ">";
    }
  }
}