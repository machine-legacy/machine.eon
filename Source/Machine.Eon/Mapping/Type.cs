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
    private readonly TypeName _name;
    private readonly List<Method> _methods = new List<Method>();
    private readonly List<Property> _properties = new List<Property>();
    private readonly List<Field> _fields = new List<Field>();
    private readonly List<Type> _interfaces = new List<Type>();
    private readonly List<Type> _attributes = new List<Type>();
    private readonly UsageSet _usages = new UsageSet();
    private TypeFlags _flags = TypeFlags.Invalid;
    private Type _baseType;

    public TypeName Name
    {
      get { return _name; }
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

    public Type(Namespace ns, TypeName name)
      : this(ns, name, null)
    {
    }

    private Type(Namespace ns, TypeName name, Type baseType)
    {
      _namespace = ns;
      _name = name;
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
      Method newMember = new Method(this, name);
      _methods.Add(newMember);
      return newMember;
    }

    public Property FindOrCreateProperty(PropertyName name)
    {
      if (!name.TypeName.Equals(_name)) throw new ArgumentException("name");
      foreach (Property property in _properties)
      {
        if (property.Name.Equals(name))
        {
          return property;
        }
      }
      Property newMember = new Property(this, name);
      _properties.Add(newMember);
      return newMember;
    }

    public Field FindOrCreateField(FieldName name)
    {
      if (!name.TypeName.Equals(_name)) throw new ArgumentException("name");
      foreach (Field field in _fields)
      {
        if (field.Name.Equals(name))
        {
          return field;
        }
      }
      Field newMember = new Field(this, name);
      _fields.Add(newMember);
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
      return _name.ToString();
    }

    public bool IsA(Type type)
    {
      return IsA(type.Name);
    }

    public bool IsA(TypeName name)
    {
      if (this.Name.Equals(name)) return true;
      foreach (Type interfaceType in _interfaces)
      {
        if (interfaceType.IsA(name)) return true;
      }
      if (_baseType == null)
      {
        return false;
      }
      return _baseType.IsA(name);
    }
  }
}