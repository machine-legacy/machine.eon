using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class NamespaceName
  {
    public static readonly NamespaceName None = new NamespaceName(String.Empty);
    private readonly string _name;

    public string Name
    {
      get { return _name; }
    }

    public NamespaceName(string name)
    {
      _name = name;
    }

    public override bool Equals(object obj)
    {
      if (obj is NamespaceName)
      {
        return ((NamespaceName)obj).Name.Equals(this.Name);
      }
      return false;
    }

    public override Int32 GetHashCode()
    {
      return _name.GetHashCode();
    }

    public override string ToString()
    {
      return _name;
    }
  }
}