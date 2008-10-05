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
    }

    public void StartNamespace(NamespaceName name)
    {
    }

    public void StartType(TypeName name)
    {
    }

    public void StartMethod(MethodName name)
    {
    }

    public void UseType(TypeName name)
    {
    }

    public void EndMethod()
    {
    }

    public void EndType()
    {
    }

    public void EndNamespace()
    {
    }

    public void EndAssembly()
    {
    }
  }
}