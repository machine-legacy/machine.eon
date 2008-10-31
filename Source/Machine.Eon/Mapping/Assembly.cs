using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Assembly : Node, IAssembly
  {
    private readonly List<Namespace> _namespaces = new List<Namespace>();
    private readonly AssemblyKey _key;

    public AssemblyKey Key
    {
      get { return _key; }
    }

    public IEnumerable<Namespace> Namespaces
    {
      get { return _namespaces; }
    }

    public Assembly(AssemblyKey key)
    {
      _key = key;
    }

    public Namespace FindOrCreateNamespace(NamespaceKey key)
    {
      foreach (Namespace ns in _namespaces)
      {
        if (ns.Key.Equals(key))
        {
          return ns;
        }
      }
      Namespace newNs = new Namespace(this, key);
      _namespaces.Add(newNs);
      return newNs;
    }

    public Type FindOrCreateType(TypeKey typeKey)
    {
      Namespace ns = FindOrCreateNamespace(typeKey.Namespace);
      return ns.FindOrCreateType(typeKey);
    }

    public Method FindOrCreateMethod(MethodKey methodKey)
    {
      Type type = FindOrCreateType(methodKey.TypeKey);
      return type.FindOrCreateMethod(methodKey);
    }

    public Property FindOrCreateProperty(PropertyKey propertyKey)
    {
      Type type = FindOrCreateType(propertyKey.TypeKey);
      return type.FindOrCreateProperty(propertyKey);
    }

    public Field FindOrCreateField(FieldKey fieldKey)
    {
      Type type = FindOrCreateType(fieldKey.TypeKey);
      return type.FindOrCreateField(fieldKey);
    }

    public override string ToString()
    {
      return _key.ToString();
    }
  }
}
