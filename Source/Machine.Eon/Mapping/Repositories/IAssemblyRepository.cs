using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping.Repositories
{
  public interface IAssemblyRepository
  {
    Assembly FindAssembly(AssemblyName name);
    IEnumerable<Assembly> FindAll();
    void SaveAssembly(Assembly assembly);
  }
}