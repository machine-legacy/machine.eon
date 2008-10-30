using System;
using System.Collections.Generic;

using Machine.Eon.Mapping.Repositories;

namespace Machine.Eon.Mapping.Inspection
{
  public class Listener
  {
    private readonly ITypeRepository _typeRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly Stack<AssemblyName> _assemblies = new Stack<AssemblyName>();
    private readonly Stack<NamespaceName> _namespaces = new Stack<NamespaceName>();
    private readonly Stack<TypeName> _types = new Stack<TypeName>();
    private readonly Stack<MethodName> _methods = new Stack<MethodName>();
    private readonly Stack<PropertyName> _properties = new Stack<PropertyName>();
    private readonly Stack<FieldName> _fields = new Stack<FieldName>();

    public Listener(ITypeRepository typeRepository, IMemberRepository memberRepository)
    {
      _types.Push(TypeName.None);
      _methods.Push(MethodName.None);
      _properties.Push(PropertyName.None);
      _fields.Push(FieldName.None);
      _typeRepository = typeRepository;
      _memberRepository = memberRepository;
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
      if (baseTypeName == null) throw new ArgumentNullException("baseTypeName");
      Type type = _typeRepository.FindType(baseTypeName);
      GetCurrentType().BaseType = type;
    }

    public void SetFieldType(TypeName name)
    {
      if (name == null) throw new ArgumentNullException("name");
      GetCurrentField().FieldType = _typeRepository.FindType(name);
    }

    public void SetPropertyType(TypeName name)
    {
      if (name == null) throw new ArgumentNullException("name");
      GetCurrentProperty().PropertyType = _typeRepository.FindType(name);
    }

    public void SetMethodPrototype(TypeName returnTypeName, ICollection<TypeName> parameterTypeNames)
    {
      if (returnTypeName == null) throw new ArgumentNullException("returnTypeName");
      if (parameterTypeNames == null) throw new ArgumentNullException("parameterTypeNames");
      List<Parameter> parameters = new List<Parameter>();
      foreach (TypeName name in parameterTypeNames)
      {
        parameters.Add(new Parameter(_typeRepository.FindType(name)));
      }
      Method method = GetCurrentMethod();
      method.ReturnType = _typeRepository.FindType(returnTypeName);
      method.SetParameters(parameters);
    }

    public void ImplementsInterface(TypeName interfaceTypeName)
    {
      if (interfaceTypeName == null) throw new ArgumentNullException("interfaceTypeName");
      Type type = _typeRepository.FindType(interfaceTypeName);
      GetCurrentType().AddInterface(type);
    }

    public void HasAttribute(TypeName typeName)
    {
      if (typeName == null) throw new ArgumentNullException("typeName");
      Type type = _typeRepository.FindType(typeName);
      GetCurrentCanHaveAttributes().AddAttribute(type);
    }

    public void SetTypeFlags(bool isInterface, bool isAbstract)
    {
      TypeFlags flags = TypeFlags.None;
      if (isInterface) flags |= TypeFlags.Interface;
      if (isAbstract) flags |= TypeFlags.Abstract;
      GetCurrentType().TypeFlags = flags;
    }

    public void SetMethodFlags(bool isConstructor, bool isAbstract, bool isVirtual)
    {
      MethodFlags flags = MethodFlags.None;
      if (isConstructor) flags |= MethodFlags.Constructor;
      if (isAbstract) flags |= MethodFlags.Abstract;
      if (isVirtual) flags |= MethodFlags.Virtual;
      GetCurrentMethod().MethodFlags = flags;
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
      Field field = GetCurrentField();
      if (field != null) return field;
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