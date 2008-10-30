using System;
using System.Collections.Generic;
using System.Linq;

using Machine.Eon.Mapping;
using Type = Machine.Eon.Mapping.Type;

namespace Machine.Eon
{
  public class QueryRoot
  {
    private readonly IEnumerable<Assembly> _assemblies;

    public QueryRoot(IEnumerable<Assembly> assemblies)
    {
      _assemblies = assemblies;
    }

    public IEnumerable<Assembly> Assemblies
    {
      get { return _assemblies; }
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
        return from ns in Namespaces
               from type in ns.Types
               select type;
      }
    }

    public IEnumerable<Method> Methods
    {
      get
      {
        return from type in Types
               from method in type.Methods
               select method;
      }
    }

    public IEnumerable<Property> Properties
    {
      get
      {
        return from type in Types
               from property in type.Properties
               select property;
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
