using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping.Repositories.Impl
{
  public class AssemblyRepository
  {
    public Assembly FindAssembly(AssemblyName name)
    {
      if (!Storage.InMemory.Assemblies.ContainsKey(name))
      {
        return null;
      }
      return Storage.InMemory.Assemblies[name];
    }

    public IEnumerable<Assembly> FindAll()
    {
      return Storage.InMemory.Assemblies.Values;
    }

    public void SaveAssembly(Assembly assembly)
    {
      Storage.InMemory.Assemblies[assembly.Name] = assembly;
    }
  }
}