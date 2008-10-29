using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Assembly : Node, IAssembly, IHaveUses
  {
    private readonly List<Namespace> _namespaces = new List<Namespace>();
    private readonly AssemblyName _name;

    public AssemblyName Name
    {
      get { return _name; }
    }

    public AssemblyName AssemblyName
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

    public override NodeName NodeName
    {
      get { return _name; }
    }

    public override Usage CreateUsage()
    {
      throw new InvalidOperationException();
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
      Namespace newNs = new Namespace(this, name);
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

    public override string ToString()
    {
      return _name.ToString();
    }

    public UsageSet Uses
    {
      get { return UsageSet.Union(_namespaces); }
    }
  }
}
