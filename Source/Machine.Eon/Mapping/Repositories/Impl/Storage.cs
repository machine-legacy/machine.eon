using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping.Repositories.Impl
{
  public class Storage
  {
    public static Storage InMemory = new Storage();
    private readonly Dictionary<AssemblyName, Assembly> _assemblies = new Dictionary<AssemblyName, Assembly>();

    public IDictionary<AssemblyName, Assembly> Assemblies
    {
      get { return _assemblies; }
    }
  }
}