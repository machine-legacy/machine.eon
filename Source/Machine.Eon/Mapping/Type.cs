using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  [Flags]
  public enum TypeFlags
  {
    None = 0,
    Invalid = 1,
    Interface = 2,
    Abstract = 4,
    Static = 8
  }
  public class Type : Node, IType, ICanUseNodes, IHaveDirectUses, IHaveIndirectUses, ICanHaveAttributes
  {
    private readonly Namespace _namespace;
    private readonly TypeKey _key;
    private readonly List<Method> _methods = new List<Method>();
    private readonly List<Property> _properties = new List<Property>();
    private readonly List<Field> _fields = new List<Field>();
    private readonly List<Event> _events = new List<Event>();
    private readonly List<Type> _interfaces = new List<Type>();
    private readonly List<Type> _attributes = new List<Type>();
    private readonly UsageSet _usages = new UsageSet();
    private TypeFlags _flags = TypeFlags.Invalid;
    private Type _baseType;

    public TypeKey Key
    {
      get { return _key; }
    }

    public Namespace Namespace
    {
      get { return _namespace; }
    }

    public TypeFlags TypeFlags
    {
      get { return _flags; }
      set { _flags = value; }
    }

    public bool IsValid
    {
      get { return !IsPendingCreation; }
    }

    public bool IsPendingCreation
    {
      get { return (_flags & TypeFlags.Invalid) == TypeFlags.Invalid; }
    }

    public bool IsClass
    {
      get { return !IsInterface; }
    }

    public bool IsInterface
    {
      get { return (_flags & TypeFlags.Interface) == TypeFlags.Interface; }
    }

    public bool IsAbstract
    {
      get { return (_flags & TypeFlags.Abstract) == TypeFlags.Abstract; }
    }

    /*
    public bool IsStatic
    {
      get { return (_flags & TypeFlags.Static) == TypeFlags.Static; }
    }
    */
    
    public Type BaseType
    {
      get { return _baseType; }
      set { _baseType = value; }
    }

    public Type(Namespace ns, TypeKey key)
      : this(ns, key, null)
    {
    }

    private Type(Namespace ns, TypeKey key, Type baseType)
    {
      _namespace = ns;
      _key = key;
      _baseType = baseType;
    }

    public IEnumerable<Type> Attributes
    {
      get { return _attributes; }
    }

    public IEnumerable<Type> Interfaces
    {
      get { return _interfaces; }
    }

    public IEnumerable<Field> Fields
    {
      get { return _fields; }
    }

    public IEnumerable<Property> Properties
    {
      get { return _properties; }
    }

    public IEnumerable<Event> Events
    {
      get { return _events; }
    }

    public IEnumerable<Method> Methods
    {
      get { return _methods; }
    }

    public IEnumerable<Method> MethodsNotPartOfProperties
    {
      get
      {
        List<Method> methods = new List<Method>(_methods);
        foreach (Property property in _properties)
        {
          methods.Remove(property.Getter);
          methods.Remove(property.Setter);
        }
        return methods;
      }
    }

    public IEnumerable<Member> Members
    {
      get
      {
        foreach (Field field in _fields) yield return field;
        foreach (Property property in _properties) yield return property;
        foreach (Method method in _methods) yield return method;
      }
    }

    public Method FindOrCreateMethod(MethodKey key)
    {
      if (!key.TypeKey.Equals(_key)) throw new ArgumentException("name");
      foreach (Method method in _methods)
      {
        if (method.Key.Equals(key))
        {
          return method;
        }
      }
      Method newMember = new Method(this, key);
      _methods.Add(newMember);
      return newMember;
    }

    public Property FindOrCreateProperty(PropertyKey key)
    {
      if (!key.TypeKey.Equals(_key)) throw new ArgumentException("name");
      foreach (Property property in _properties)
      {
        if (property.Key.Equals(key))
        {
          return property;
        }
      }
      Property newMember = new Property(this, key);
      _properties.Add(newMember);
      return newMember;
    }

    public Field FindOrCreateField(FieldKey key)
    {
      if (!key.TypeKey.Equals(_key)) throw new ArgumentException("name");
      foreach (Field field in _fields)
      {
        if (field.Key.Equals(key))
        {
          return field;
        }
      }
      Field newMember = new Field(this, key);
      _fields.Add(newMember);
      return newMember;
    }

    public Event FindOrCreateEvent(EventKey key)
    {
      if (!key.TypeKey.Equals(_key)) throw new ArgumentException("name");
      foreach (Event anEvent in _events)
      {
        if (anEvent.Key.Equals(key))
        {
          return anEvent;
        }
      }
      Event newMember = new Event(this, key);
      _events.Add(newMember);
      return newMember;
    }

    public override Usage CreateUsage()
    {
      return new TypeUsage(this);
    }

    public void Use(Node node)
    {
      _usages.Add(node);
    }

    public UsageSet DirectlyUses
    {
      get { return UsageSet.Union(_usages, DirectUsesOfMembers); }
    }

    private UsageSet DirectUsesOfMembers
    {
      get
      {
        List<UsageSet> usages = new List<UsageSet>();
        foreach (Member member in this.Members)
        {
          usages.Add(member.DirectlyUses);
        }
        return UsageSet.Union(usages.ToArray());
      }
    }

    public UsageSet DirectUsesAttributesInterfacesAndMethods
    {
      get
      {
        UsageSet set = UsageSet.Union(_usages, UsageSet.MakeFrom(_attributes), UsageSet.MakeFrom(_interfaces), UsageSet.MakeFrom(_methods));
        set.Add(this);
        if (this.BaseType != null)
        {
          set.Add(this.BaseType);
        }
        return set;
      }
    }

    public IndirectUses IndirectlyUses
    {
      get { return DirectUsesAttributesInterfacesAndMethods.CreateIndirectUses(); }
    }

    public void AddInterface(Type type)
    {
      if (!_interfaces.Contains(type))
      {
        _interfaces.Add(type);
      }
    }

    public void AddAttribute(Type type)
    {
      if (!_attributes.Contains(type))
      {
        _attributes.Add(type);
      }
    }

    public override string ToString()
    {
      return _key.ToString();
    }

    public bool IsA(Type type)
    {
      return IsA(type.Key);
    }

    public bool IsA(TypeKey key)
    {
      if (this.Key.Equals(key)) return true;
      foreach (Type interfaceType in _interfaces)
      {
        if (interfaceType.IsA(key)) return true;
      }
      if (_baseType == null)
      {
        return false;
      }
      return _baseType.IsA(key);
    }
  }
}