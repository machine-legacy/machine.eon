using System;
using System.Collections.Generic;
using System.Linq;

using Machine.Eon.Mapping;
using Machine.Eon.Mapping.Repositories.Impl;

namespace Machine.Eon
{
  public class QueryRoot
  {
    public IEnumerable<Assembly> Assemblies
    {
      get { return Storage.InMemory.Assemblies.Values; }
    }

    public IEnumerable<Namespace> Namespaces
    {
      get
      {
        return from assembly in Assemblies
               from ns in assembly.Namespaces
               select ns;
      }
    }

    public IEnumerable<Machine.Eon.Mapping.Type> Types
    {
      get
      {
        return from assembly in Assemblies
               from ns in assembly.Namespaces
               from type in ns.Types
               select type;
      }
    }
  }
}
