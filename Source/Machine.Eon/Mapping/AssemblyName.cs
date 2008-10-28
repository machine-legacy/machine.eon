using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class AssemblyName
  {
    public static readonly AssemblyName None = new AssemblyName(String.Empty);
    private readonly string _name;

    public string Name
    {
      get { return _name; }
    }

    public AssemblyName(string name)
    {
      _name = name;
    }

    public override bool Equals(object obj)
    {
      if (obj is AssemblyName)
      {
        return ((AssemblyName)obj).Name.Equals(this.Name);
      }
      return false;
    }

    public static bool operator ==(AssemblyName n1, AssemblyName n2)
    {
      return Equals(n1, n2);
    }

    public static bool operator !=(AssemblyName n1, AssemblyName n2)
    {
      return !Equals(n1, n2);
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