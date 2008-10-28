using System;
using System.Collections.Generic;

using Machine.Eon.Mapping.Repositories;
using Machine.Eon.Mapping.Repositories.Impl;

namespace Machine.Eon.Mapping
{
  public class Listener
  {
    private readonly IMemberRepository _memberRepository = new MemberRepository();
    private readonly ITypeRepository _typeRepository = new TypeRepository();
    private readonly Stack<AssemblyName> _assemblies = new Stack<AssemblyName>();
    private readonly Stack<NamespaceName> _namespaces = new Stack<NamespaceName>();
    private readonly Stack<TypeName> _types = new Stack<TypeName>();
    private readonly Stack<MethodName> _methods = new Stack<MethodName>();
    private readonly Stack<PropertyName> _properties = new Stack<PropertyName>();

    public Listener()
    {
      _types.Push(TypeName.None);
      _methods.Push(MethodName.None);
    }

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

    public void StartProperty(PropertyName name)
    {
      _properties.Push(name);
    }

    public void EndProperty()
    {
      _properties.Pop();
    }

    public void StartMethod(MethodName name)
    {
      _methods.Push(name);
    }

    public void UseType(TypeName name)
    {
      AssemblyName assemblyName = _assemblies.Peek();
      TypeName typeName = _types.Peek();
      MethodName methodName = _methods.Peek();
      if (typeName == TypeName.None)
      {
        return;
      }
      if (methodName != MethodName.None)
      {
        Method method = _memberRepository.FindMethod(assemblyName, methodName);
        method.UseType(name);
      }
      else
      {
        Type type = _typeRepository.FindType(assemblyName, typeName);
        type.UseType(name);
      }
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