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
      get { return from assembly in Assemblies from ns in assembly.Namespaces select ns; }
    }

    public IEnumerable<Type> Types
    {
      get { return from ns in Namespaces from type in ns.Types select type; }
    }

    public IEnumerable<Method> Methods
    {
      get { return from type in Types from method in type.Methods select method; }
    }

    public IEnumerable<Property> Properties
    {
      get { return from type in Types from property in type.Properties select property; }
    }

    public IEnumerable<Namespace> NamespacesNamed(string name)
    {
      return from ns in Namespaces where ns.Key.Equals(new NamespaceKey(AssemblyKey.Any, name)) select ns;
    }

    public IEnumerable<Assembly> AssembliesNamed(string name)
    {
      return from assembly in Assemblies where assembly.Key.Equals(new AssemblyKey(name)) select assembly;
    }

    public Type SystemObject
    {
      get { return FromSystemType(typeof(Object)); }
    }

    public ICollection<Type> FromSystemType(params System.Type[] runtimeTypes)
    {
      List<Type> types = new List<Type>();
      foreach (System.Type runtimeType in runtimeTypes)
      {
        types.Add(FromSystemType(runtimeType));
      }
      return types.ToArray();
    }

    public Type FromSystemType(System.Type runtimeType)
    {
      if (runtimeType.IsGenericType)
      {
        runtimeType = runtimeType.GetGenericTypeDefinition();
      }
      var query = from type in Types where type.Key.FullName.Equals(runtimeType.FullName) select type;
      return query.Single();
    }

    public Type FromTypeName(TypeKey typeKey)
    {
      var query = from type in Types where type.Key.Equals(typeKey) select type;
      return query.Single();
    }

    public Type this[System.Type runtimeType]
    {
      get { return FromSystemType(runtimeType); }
    }

    public Type this[TypeKey typeKey]
    {
      get { return FromTypeName(typeKey); }
    }

    public IEnumerable<Type> TypesThatAre(System.Type runtimeType)
    {
      return TypesThatAre(FromSystemType(runtimeType));
    }

    public IEnumerable<Type> TypesThatAre(Type baseType)
    {
      return from type in Types where type.IsA(baseType.Key) select type;
    }
  }
}
