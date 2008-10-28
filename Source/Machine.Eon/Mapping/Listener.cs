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
      _properties.Push(PropertyName.None);
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
      ICanUse can = GetCurrentCanUse();
      if (can != null)
      {
        can.Use(_typeRepository.FindType(name));
      }
    }

    public void UseMethod(MethodName name)
    {
      ICanUse can = GetCurrentCanUse();
      if (can != null)
      {
        //can.Use(_memberRepository.FindMethod());
      }
    }

    public void UseProperty(PropertyName name)
    {
      ICanUse can = GetCurrentCanUse();
      if (can != null)
      {
        //can.Use(_memberRepository.FindProperty());
      }
    }

    public void SetPropertyGetter(PropertyName propertyName, MethodName methodName)
    {
      AssemblyName assemblyName = _assemblies.Peek();
      Property property = _memberRepository.FindProperty(assemblyName, propertyName);
      property.Getter = _memberRepository.FindMethod(assemblyName, methodName);
    }

    public void SetPropertySetter(PropertyName propertyName, MethodName methodName)
    {
      AssemblyName assemblyName = _assemblies.Peek();
      Property property = _memberRepository.FindProperty(assemblyName, propertyName);
      property.Setter = _memberRepository.FindMethod(assemblyName, methodName);
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

    private Type GetCurrentType()
    {
      AssemblyName assemblyName = _assemblies.Peek();
      TypeName typeName = _types.Peek();
      if (typeName == TypeName.None)
      {
        return null;
      }
      return _typeRepository.FindType(assemblyName, typeName);
    }

    private Method GetCurrentMethod()
    {
      AssemblyName assemblyName = _assemblies.Peek();
      MethodName methodName = _methods.Peek();
      TypeName typeName = _types.Peek();
      if (typeName == TypeName.None)
      {
        return null;
      }
      if (methodName == MethodName.None)
      {
        return null;
      }
      return _memberRepository.FindMethod(assemblyName, methodName);
    }

    private ICanUse GetCurrentCanUse()
    {
      Method method = GetCurrentMethod();
      if (method != null)
      {
        return method;
      }
      Type type = GetCurrentType();
      if (type != null)
      {
        return type;
      }
      return null;
    }
  }
}