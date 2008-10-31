using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping.Repositories.Impl
{
  public class AssemblyRepository : IAssemblyRepository
  {
    private readonly Dictionary<AssemblyKey, Assembly> _assemblies = new Dictionary<AssemblyKey, Assembly>();

    public Assembly FindAssembly(AssemblyKey key)
    {
      if (!_assemblies.ContainsKey(key))
      {
        return null;
      }
      return _assemblies[key];
    }

    public IEnumerable<Assembly> FindAll()
    {
      return _assemblies.Values;
    }

    public void SaveAssembly(Assembly assembly)
    {
      _assemblies[assembly.Key] = assembly;
    }
  }
}