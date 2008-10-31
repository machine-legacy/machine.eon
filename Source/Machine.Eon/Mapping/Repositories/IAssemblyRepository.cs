using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping.Repositories
{
  public interface IAssemblyRepository
  {
    Assembly FindAssembly(AssemblyKey key);
    IEnumerable<Assembly> FindAll();
    void SaveAssembly(Assembly assembly);
  }
}