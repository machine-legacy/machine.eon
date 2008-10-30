using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping.Repositories.Impl
{
  public class AssemblyRepository : IAssemblyRepository
  {
    private readonly Dictionary<AssemblyName, Assembly> _assemblies = new Dictionary<AssemblyName, Assembly>();

    public Assembly FindAssembly(AssemblyName name)
    {
      if (!_assemblies.ContainsKey(name))
      {
        return null;
      }
      return _assemblies[name];
    }

    public IEnumerable<Assembly> FindAll()
    {
      return _assemblies.Values;
    }

    public void SaveAssembly(Assembly assembly)
    {
      _assemblies[assembly.Name] = assembly;
    }
  }
}