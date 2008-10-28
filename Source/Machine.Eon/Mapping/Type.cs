using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class Type : Node, ICanUse
  {
    private readonly TypeName _name;
    private readonly List<Method> _methods = new List<Method>();
    private readonly List<Property> _properties = new List<Property>();
    private readonly UsageSet _usages = new UsageSet();

    public TypeName Name
    {
      get { return _name; }
    }

    public Type(TypeName name)
    {
      _name = name;
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
      Method newMember = new Method(name);
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
      Property newMember = new Property(name);
      _properties.Add(newMember);
      return newMember;
    }

    public void UseType(TypeName name)
    {
      _usages.Add(name);
    }

    public void Use(Node node)
    {
    }

    public override string ToString()
    {
      return _name.ToString();
    }
  }
}