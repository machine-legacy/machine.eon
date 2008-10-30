using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  [Flags]
  public enum TypeFlags
  {
    None,
    Interface,
    Abstract
  }
  public class Type : Node, IType, ICanUseNodes, ICanHaveAttributes, IHaveUses
  {
    private readonly Namespace _namespace;
    private readonly TypeName _name;
    private readonly List<Method> _methods = new List<Method>();
    private readonly List<Property> _properties = new List<Property>();
    private readonly List<Type> _interfaces = new List<Type>();
    private readonly List<Type> _attributes = new List<Type>();
    private readonly UsageSet _usages = new UsageSet();
    private TypeFlags _typeFlags;
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
      get { return _typeFlags; }
      set { _typeFlags = value; }
    }

    public bool IsClass
    {
      get { return !IsInterface; }
    }

    public bool IsInterface
    {
      get { return (this.TypeFlags & TypeFlags.Interface) == TypeFlags.Interface; }
    }

    public bool IsAbstract
    {
      get { return (this.TypeFlags & TypeFlags.Abstract) == TypeFlags.Abstract; }
    }

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
      get { return _usages; }
    }

    public UsageSet IndirectlyUses
    {
      get { return UsageSet.Union(this.DirectlyUses, UsageSet.Union(_methods)); }
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