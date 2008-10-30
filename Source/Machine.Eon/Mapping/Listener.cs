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
    private readonly Stack<FieldName> _fields = new Stack<FieldName>();

    public Listener()
    {
      _types.Push(TypeName.None);
      _methods.Push(MethodName.None);
      _properties.Push(PropertyName.None);
      _fields.Push(FieldName.None);
    }

    public void StartAssembly(AssemblyName name)
    {
      if (name == null) throw new ArgumentNullException("name");
      _assemblies.Push(name);
    }

    public void StartNamespace(NamespaceName name)
    {
      if (name == null) throw new ArgumentNullException("name");
      _namespaces.Push(name);
    }

    public void StartType(TypeName name)
    {
      if (name == null) throw new ArgumentNullException("name");
      _types.Push(name);
      GetCurrentType();
    }

    public void StartProperty(PropertyName name)
    {
      if (name == null) throw new ArgumentNullException("name");
      _properties.Push(name);
      GetCurrentProperty();
    }

    public void EndProperty()
    {
      _properties.Pop();
    }

    public void StartField(FieldName name)
    {
      if (name == null) throw new ArgumentNullException("name");
      _fields.Push(name);
      GetCurrentField();
    }

    public void EndField()
    {
      _fields.Pop();
    }

    public void StartMethod(MethodName name)
    {
      if (name == null) throw new ArgumentNullException("name");
      _methods.Push(name);
      GetCurrentMethod();
    }

    public void UseType(TypeName name)
    {
      if (name == null) throw new ArgumentNullException("name");
      UseInCurrentContext(_typeRepository.FindType(name));
    }

    public void UseMethod(MethodName name)
    {
      if (name == null) throw new ArgumentNullException("name");
      UseInCurrentContext(_memberRepository.FindMethod(name));
    }

    public void UseProperty(PropertyName name)
    {
      if (name == null) throw new ArgumentNullException("name");
      UseInCurrentContext(_memberRepository.FindProperty(name));
    }

    public void SetPropertyGetter(PropertyName propertyName, MethodName methodName)
    {
      if (propertyName == null) throw new ArgumentNullException("propertyName");
      if (methodName == null) throw new ArgumentNullException("methodName");
      Property property = _memberRepository.FindProperty(propertyName);
      property.Getter = _memberRepository.FindMethod(methodName);
    }

    public void SetPropertySetter(PropertyName propertyName, MethodName methodName)
    {
      if (propertyName == null) throw new ArgumentNullException("propertyName");
      if (methodName == null) throw new ArgumentNullException("methodName");
      Property property = _memberRepository.FindProperty(propertyName);
      property.Setter = _memberRepository.FindMethod(methodName);
    }

    public void SetBaseType(TypeName baseTypeName)
    {
      Type type = _typeRepository.FindType(baseTypeName);
      GetCurrentType().BaseType = type;
    }

    public void ImplementsInterface(TypeName interfaceTypeName)
    {
      Type type = _typeRepository.FindType(interfaceTypeName);
      GetCurrentType().AddInterface(type);
    }

    public void HasAttribute(TypeName interfaceTypeName)
    {
      Type type = _typeRepository.FindType(interfaceTypeName);
      GetCurrentCanHaveAttributes().AddAttribute(type);
    }

    public void SetTypeFlags(bool isInterface, bool isAbstract)
    {
      TypeFlags typeFlags = TypeFlags.None;
      if (isInterface) typeFlags |= TypeFlags.Interface;
      if (isAbstract) typeFlags |= TypeFlags.Abstract;
      GetCurrentType().TypeFlags = typeFlags;
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
      TypeName typeName = _types.Peek();
      if (typeName == TypeName.None)
      {
        return null;
      }
      return _typeRepository.FindType(typeName);
    }

    private Method GetCurrentMethod()
    {
      MethodName methodName = _methods.Peek();
      TypeName typeName = _types.Peek();
      if (typeName == TypeName.None) return null;
      if (methodName == MethodName.None) return null;
      return _memberRepository.FindMethod(methodName);
    }

    private Property GetCurrentProperty()
    {
      PropertyName propertyName = _properties.Peek();
      TypeName typeName = _types.Peek();
      if (typeName == TypeName.None) return null;
      if (propertyName == PropertyName.None) return null;
      return _memberRepository.FindProperty(propertyName);
    }

    private Field GetCurrentField()
    {
      FieldName fieldName = _fields.Peek();
      TypeName typeName = _types.Peek();
      if (typeName == TypeName.None) return null;
      if (fieldName == FieldName.None) return null;
      return _memberRepository.FindField(fieldName);
    }

    private ICanHaveAttributes GetCurrentCanHaveAttributes()
    {
      Method method = GetCurrentMethod();
      if (method != null) return method;
      Property property = GetCurrentProperty();
      if (property != null) return property;
      Field field = GetCurrentField();
      if (field != null) return field;
      Type type = GetCurrentType();
      return type;
    }

    private ICanUseNodes GetCurrentCanUse()
    {
      Method method = GetCurrentMethod();
      if (method != null) return method;
      Type type = GetCurrentType();
      return type;
    }

    private void UseInCurrentContext(Node node)
    {
      ICanUseNodes can = GetCurrentCanUse();
      if (can != null)
      {
        can.Use(node);
      }
    }
  }
}