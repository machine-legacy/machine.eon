using System;
using System.Collections.Generic;
using System.Linq;

using Machine.Eon.Mapping;
using Machine.Eon.Mapping.Repositories.Impl;
using Type = Machine.Eon.Mapping.Type;

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

    public IEnumerable<Type> Types
    {
      get
      {
        return from assembly in Assemblies
               from ns in assembly.Namespaces
               from type in ns.Types
               select type;
      }
    }

    public IEnumerable<Namespace> NamespacesNamed(string name)
    {
      return from ns in Namespaces
             where ns.Name.Equals(new NamespaceName(AssemblyName.Any, name))
             select ns;
    }

    public IEnumerable<Assembly> AssembliesNamed(string name)
    {
      return from assembly in Assemblies
             where assembly.Name.Equals(new AssemblyName(name))
             select assembly;
    }
  }
}
