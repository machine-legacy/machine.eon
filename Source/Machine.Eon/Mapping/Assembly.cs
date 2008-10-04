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
