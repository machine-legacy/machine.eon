using System;
using System.Collections.Generic;

using Machine.Eon.Mapping.Repositories;

namespace Machine.Eon.Mapping.Inspection
{
  public class ModelCreator
  {
    private readonly ITypeRepository _typeRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly Stack<AssemblyKey> _assemblies = new Stack<AssemblyKey>();
    private readonly Stack<NamespaceKey> _namespaces = new Stack<NamespaceKey>();
    private readonly Stack<TypeKey> _types = new Stack<TypeKey>();
    private readonly Stack<MethodKey> _methods = new Stack<MethodKey>();
    private readonly Stack<PropertyKey> _properties = new Stack<PropertyKey>();
    private readonly Stack<FieldKey> _fields = new Stack<FieldKey>();
    private readonly Stack<EventKey> _events = new Stack<EventKey>();

    public ModelCreator(ITypeRepository typeRepository, IMemberRepository memberRepository)
    {
      _types.Push(TypeKey.None);
      _methods.Push(MethodKey.None);
      _properties.Push(PropertyKey.None);
      _fields.Push(FieldKey.None);
      _events.Push(EventKey.None);
      _typeRepository = typeRepository;
      _memberRepository = memberRepository;
    }

    public void StartAssembly(AssemblyKey key)
    {
      if (key == null) throw new ArgumentNullException("key");
      _assemblies.Push(key);
    }

    public void StartNamespace(NamespaceKey key)
    {
      if (key == null) throw new ArgumentNullException("key");
      _namespaces.Push(key);
    }

    public void StartType(TypeKey key)
    {
      if (key == null) throw new ArgumentNullException("key");
      _types.Push(key);
      GetCurrentType();
    }

    public void StartProperty(PropertyKey key)
    {
      if (key == null) throw new ArgumentNullException("key");
      _properties.Push(key);
      GetCurrentProperty();
    }

    public void EndProperty()
    {
      _properties.Pop();
    }

    public void StartField(FieldKey key)
    {
      if (key == null) throw new ArgumentNullException("key");
      _fields.Push(key);
      GetCurrentField();
    }

    public void EndField()
    {
      _fields.Pop();
    }

    public void StartEvent(EventKey key)
    {
      if (key == null) throw new ArgumentNullException("key");
      _events.Push(key);
      GetCurrentEvent();
    }

    public void EndEvent()
    {
      _events.Pop();
    }

    public void StartMethod(MethodKey key)
    {
      if (key == null) throw new ArgumentNullException("key");
      _methods.Push(key);
      GetCurrentMethod();
    }

    public void UseType(TypeKey key)
    {
      if (key == null) throw new ArgumentNullException("key");
      UseInCurrentContext(_typeRepository.FindType(key));
    }

    public void UseMethod(MethodKey key)
    {
      if (key == null) throw new ArgumentNullException("key");
      UseInCurrentContext(_memberRepository.FindMethod(key));
    }

    public void UseProperty(PropertyKey key)
    {
      if (key == null) throw new ArgumentNullException("key");
      UseInCurrentContext(_memberRepository.FindProperty(key));
    }

    public void SetPropertyGetter(PropertyKey propertyKey, MethodKey methodKey)
    {
      if (propertyKey == null) throw new ArgumentNullException("propertyKey");
      if (methodKey == null) throw new ArgumentNullException("methodKey");
      Property property = _memberRepository.FindProperty(propertyKey);
      property.Getter = _memberRepository.FindMethod(methodKey);
    }

    public void SetPropertySetter(PropertyKey propertyKey, MethodKey methodKey)
    {
      if (propertyKey == null) throw new ArgumentNullException("propertyKey");
      if (methodKey == null) throw new ArgumentNullException("methodKey");
      Property property = _memberRepository.FindProperty(propertyKey);
      property.Setter = _memberRepository.FindMethod(methodKey);
    }

    public void SetEventAdder(EventKey eventKey, MethodKey methodKey)
    {
      if (eventKey == null) throw new ArgumentNullException("eventKey");
      if (methodKey == null) throw new ArgumentNullException("methodKey");
      Event theEvent = _memberRepository.FindEvent(eventKey);
      theEvent.Adder = _memberRepository.FindMethod(methodKey);
    }

    public void SetEventRemover(EventKey eventKey, MethodKey methodKey)
    {
      if (eventKey == null) throw new ArgumentNullException("eventKey");
      if (methodKey == null) throw new ArgumentNullException("methodKey");
      Event theEvent = _memberRepository.FindEvent(eventKey);
      theEvent.Remover = _memberRepository.FindMethod(methodKey);
    }

    public void SetBaseType(TypeKey baseTypeKey)
    {
      if (baseTypeKey == null) throw new ArgumentNullException("baseTypeKey");
      Type type = _typeRepository.FindType(baseTypeKey);
      GetCurrentType().BaseType = type;
    }

    public void SetFieldType(TypeKey key)
    {
      if (key == null) throw new ArgumentNullException("key");
      GetCurrentField().FieldType = _typeRepository.FindType(key);
    }

    public void SetPropertyType(TypeKey key)
    {
      if (key == null) throw new ArgumentNullException("key");
      GetCurrentProperty().PropertyType = _typeRepository.FindType(key);
    }

    public void SetEventType(TypeKey key)
    {
      if (key == null) throw new ArgumentNullException("key");
      GetCurrentEvent().EventType = _typeRepository.FindType(key);
    }

    public void SetMethodPrototype(TypeKey returnTypeKey, ICollection<TypeKey> parameterTypeNames)
    {
      if (returnTypeKey == null) throw new ArgumentNullException("returnTypeKey");
      if (parameterTypeNames == null) throw new ArgumentNullException("parameterTypeNames");
      List<Parameter> parameters = new List<Parameter>();
      foreach (TypeKey name in parameterTypeNames)
      {
        parameters.Add(new Parameter(_typeRepository.FindType(name)));
      }
      Method method = GetCurrentMethod();
      method.ReturnType = _typeRepository.FindType(returnTypeKey);
      method.SetParameters(parameters);
    }

    public void ImplementsInterface(TypeKey interfaceTypeKey)
    {
      if (interfaceTypeKey == null) throw new ArgumentNullException("interfaceTypeKey");
      Type type = _typeRepository.FindType(interfaceTypeKey);
      GetCurrentType().AddInterface(type);
    }

    public void HasAttribute(TypeKey typeKey)
    {
      if (typeKey == null) throw new ArgumentNullException("typeKey");
      Type type = _typeRepository.FindType(typeKey);
      GetCurrentCanHaveAttributes().AddAttribute(type);
    }

    public void SetTypeFlags(bool isInterface, bool isAbstract, bool isStatic)
    {
      TypeFlags flags = TypeFlags.None;
      if (isInterface) flags |= TypeFlags.Interface;
      if (isAbstract) flags |= TypeFlags.Abstract;
      if (isStatic) flags |= TypeFlags.Static;
      GetCurrentType().TypeFlags = flags;
    }

    public void SetMethodFlags(bool isConstructor, bool isAbstract, bool isVirtual, bool isStatic)
    {
      MethodFlags flags = MethodFlags.None;
      if (isConstructor) flags |= MethodFlags.Constructor;
      if (isAbstract) flags |= MethodFlags.Abstract;
      if (isVirtual) flags |= MethodFlags.Virtual;
      if (isStatic) flags |= MethodFlags.Static;
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
      TypeKey typeKey = _types.Peek();
      if (typeKey == TypeKey.None)
      {
        return null;
      }
      return _typeRepository.FindType(typeKey);
    }

    private Method GetCurrentMethod()
    {
      MethodKey methodKey = _methods.Peek();
      TypeKey typeKey = _types.Peek();
      if (typeKey == TypeKey.None) return null;
      if (methodKey == MethodKey.None) return null;
      return _memberRepository.FindMethod(methodKey);
    }

    private Property GetCurrentProperty()
    {
      PropertyKey propertyKey = _properties.Peek();
      TypeKey typeKey = _types.Peek();
      if (typeKey == TypeKey.None) return null;
      if (propertyKey == PropertyKey.None) return null;
      return _memberRepository.FindProperty(propertyKey);
    }

    private Field GetCurrentField()
    {
      FieldKey fieldKey = _fields.Peek();
      TypeKey typeKey = _types.Peek();
      if (typeKey == TypeKey.None) return null;
      if (fieldKey == FieldKey.None) return null;
      return _memberRepository.FindField(fieldKey);
    }

    private Event GetCurrentEvent()
    {
      EventKey eventKey = _events.Peek();
      TypeKey typeKey = _types.Peek();
      if (typeKey == TypeKey.None) return null;
      if (eventKey == EventKey.None) return null;
      return _memberRepository.FindEvent(eventKey);
    }

    private ICanHaveAttributes GetCurrentCanHaveAttributes()
    {
      Method method = GetCurrentMethod();
      if (method != null) return method;
      Property property = GetCurrentProperty();
      if (property != null) return property;
      Field field = GetCurrentField();
      if (field != null) return field;
      Event anEvent = GetCurrentEvent();
      if (anEvent != null) return anEvent;
      Type type = GetCurrentType();
      return type;
    }

    private ICanUseNodes GetCurrentCanUse()
    {
      Method method = GetCurrentMethod();
      if (method != null) return method;
      Field field = GetCurrentField();
      if (field != null) return field;
      Property property = GetCurrentProperty();
      if (property != null) return property;
      Event anEvent = GetCurrentEvent();
      if (anEvent != null) return anEvent;
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