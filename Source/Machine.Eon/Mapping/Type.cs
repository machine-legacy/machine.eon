using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Type : Node, IType, ICanUse, IHaveUses
  {
    private readonly Namespace _namespace;
    private readonly TypeName _name;
    private readonly List<Method> _methods = new List<Method>();
    private readonly List<Property> _properties = new List<Property>();
    private readonly UsageSet _usages = new UsageSet();

    public Namespace Namespace
    {
      get { return _namespace; }
    }

    public TypeName Name
    {
      get { return _name; }
    }

    public TypeName TypeName
    {
      get { return _name; }
    }

    public Type(Namespace ns, TypeName name)
    {
      _namespace = ns;
      _name = name;
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

    public override NodeName NodeName
    {
      get { return _name; }
    }

    public override Usage CreateUsage()
    {
      return new TypeUsage(this);
    }

    public void Use(Node node)
    {
      _usages.Add(node);
    }

    public UsageSet DirectUses
    {
      get { return _usages; }
    }

    public UsageSet Uses
    {
      get { return UsageSet.Union(this.DirectUses, UsageSet.Union(_methods)); }
    }

    public override string ToString()
    {
      return _name.ToString();
    }
  }
}