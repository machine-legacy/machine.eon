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
  }
  public class Namespace
  {
    private readonly NamespaceName _name;
    private readonly List<Type> _types = new List<Type>();

    public NamespaceName Name
    {
      get { return _name; }
    }

    public IEnumerable<Type> Types
    {
      get { return _types; }
    }

    public Namespace(NamespaceName name)
    {
      _name = name;
    }

    public Type FindOrCreateType(TypeName name)
    {
      if (!name.Namespace.Equals(_name)) throw new ArgumentException("name");
      foreach (Type type in _types)
      {
        if (type.Name.Equals(name))
        {
          return type;
        }
      }
      Type newType = new Type(name);
      _types.Add(newType);
      return newType;
    }
  }
  public class Type
  {
    private readonly TypeName _name;
    private readonly List<Method> _methods = new List<Method>();

    public TypeName Name
    {
      get { return _name; }
    }

    public IEnumerable<Method> Methods
    {
      get { return _methods; }
    }

    public Type(TypeName name)
    {
      _name = name;
    }

    public Method FindOrCreateMethod(MethodName name)
    {
      if (!name.TypeName.Equals(_name)) throw new ArgumentException("name");
      foreach (Method method in _methods)
      {
        if (method.Name.Equals(name))
        {
          return method;
        }
      }
      Method newMethod = new Method(name);
      _methods.Add(newMethod);
      return newMethod;
    }
  }
  public class Method
  {
    private readonly MethodName _name;

    public MethodName Name
    {
      get { return _name; }
    }

    public Method(MethodName name)
    {
      _name = name;
    }
  }
  public class TypeUsage
  {
    private readonly TypeName _typeName;

    public TypeName TypeName
    {
      get { return _typeName; }
    }

    public TypeUsage(TypeName typeName)
    {
      _typeName = typeName;
    }
  }
}
