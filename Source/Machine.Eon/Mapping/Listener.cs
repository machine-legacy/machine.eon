using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Listener
  {
    private readonly Stack<AssemblyName> _assemblies = new Stack<AssemblyName>();
    private readonly Stack<NamespaceName> _namespaces = new Stack<NamespaceName>();
    private readonly Stack<TypeName> _types = new Stack<TypeName>();
    private readonly Stack<MethodName> _methods = new Stack<MethodName>();

    public void StartAssembly(AssemblyName name)
    {
      _assemblies.Push(name);
    }

    public void StartNamespace(NamespaceName name)
    {
      _namespaces.Push(name);
    }

    public void StartType(TypeName name)
    {
      _types.Push(name);
    }

    public void StartMethod(MethodName name)
    {
      _methods.Push(name);
    }

    public void UseType(TypeName name)
    {
    }

    public void EndMethod()
    {
      _methods.Pop();
    }

    public void EndType()
    {
      _types.Pop();
    }

    public void EndNamespace()
    {
      _namespaces.Pop();
    }

    public void EndAssembly()
    {
      _assemblies.Pop();
    }
  }
}