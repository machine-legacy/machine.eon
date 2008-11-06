using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class AssemblyKey : NodeKey
  {
    public static readonly AssemblyKey None = new AssemblyKey(String.Empty, String.Empty);
    public static readonly AssemblyKey Any = new AssemblyKey("*", "*");
    private readonly string _name;
    private readonly string _fullName;

    public string Name
    {
      get { return _name; }
    }

    public string FullName
    {
      get { return _fullName; }
    }

    public AssemblyKey(string name)
      : this(name, name)
    {
    }

    public AssemblyKey(string name, string fullName)
    {
      _name = name;
      _fullName = fullName;
    }

    public override bool Equals(object obj)
    {
      if (obj is AssemblyKey)
      {
        if (ReferenceEquals(Any, obj) || ReferenceEquals(Any, this)) return true;
        return ((AssemblyKey)obj).Name.Equals(this.Name);
      }
      return false;
    }

    public static bool operator ==(AssemblyKey n1, AssemblyKey n2)
    {
      return Equals(n1, n2);
    }

    public static bool operator !=(AssemblyKey n1, AssemblyKey n2)
    {
      return !Equals(n1, n2);
    }

    public override Int32 GetHashCode()
    {
      return _name.GetHashCode();
    }

    public override string ToString()
    {
      return "Assembly<" + _name + ">";
    }
  }
}