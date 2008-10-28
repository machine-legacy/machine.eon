using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Assembly
  {
    private readonly List<Namespace> _namespaces = new List<Namespace>();
    private readonly AssemblyName _name;

    public AssemblyName Name
    {
      get { return _name; }
    }

    public IEnumerable<Namespace> Namespaces
    {
      get { return _namespaces; }
    }

    public Assembly(AssemblyName name)
    {
      _name = name;
    }

    public Namespace FindOrCreateNamespace(NamespaceName name)
    {
      foreach (Namespace ns in _namespaces)
      {
        if (ns.Name.Equals(name))
        {
          return ns;
        }
      }
      Namespace newNs = new Namespace(name);
      _namespaces.Add(newNs);
      return newNs;
    }

    public Type FindOrCreateType(TypeName typeName)
    {
      Namespace ns = FindOrCreateNamespace(typeName.Namespace);
      return ns.FindOrCreateType(typeName);
    }

    public Method FindOrCreateMethod(MethodName methodName)
    {
      Type type = FindOrCreateType(methodName.TypeName);
      return type.FindOrCreateMethod(methodName);
    }

    public Property FindOrCreateProperty(PropertyName propertyName)
    {
      Type type = FindOrCreateType(propertyName.TypeName);
      return type.FindOrCreateProperty(propertyName);
    }
  }
}
