using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class NamespaceName : NodeName
  {
    public static readonly NamespaceName None = new NamespaceName(AssemblyName.None, String.Empty);
    private readonly AssemblyName _assemblyName;
    private readonly string _name;

    public AssemblyName AssemblyName
    {
      get { return _assemblyName; }
    }

    public string Name
    {
      get { return _name; }
    }

    public NamespaceName(AssemblyName assemblyName)
      : this(assemblyName, String.Empty)
    {
    }

    public NamespaceName(AssemblyName assemblyName, string name)
    {
      _assemblyName = assemblyName;
      _name = name;
    }

    public override bool Equals(object obj)
    {
      if (obj is NamespaceName)
      {
        return ((NamespaceName)obj).AssemblyName.Equals(this.AssemblyName) && ((NamespaceName)obj).Name.Equals(this.Name);
      }
      return false;
    }

    public static bool operator ==(NamespaceName n1, NamespaceName n2)
    {
      return Equals(n1, n2);
    }

    public static bool operator !=(NamespaceName n1, NamespaceName n2)
    {
      return !Equals(n1, n2);
    }

    public override Int32 GetHashCode()
    {
      return _assemblyName.GetHashCode() ^ _name.GetHashCode();
    }

    public override string ToString()
    {
      if (String.IsNullOrEmpty(_name))
      {
        return "Ns<Null>";
      }
      return _name;
    }
  }
}