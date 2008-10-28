using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public abstract class Usage
  {
  }

  public abstract class UsageByName<TNode, TName> : Usage where TNode : Node where TName : class
  {
    private readonly TNode _node;
    private readonly TName _name;

    protected UsageByName(TNode node, TName name)
    {
      _node = node;
      _name = name;
    }

    public TName Name
    {
      get { return _name; }
    }

    public override bool Equals(object obj)
    {
      if (obj is UsageByName<TNode, TName>)
      {
        return ((UsageByName<TNode, TName>)obj).Name.Equals(this.Name);
      }
      return false;
    }
    
    public override Int32 GetHashCode()
    {
      return this.Name.GetHashCode();
    }

    public override string ToString()
    {
      return "Usage<" + this.Name + ">";
    }
  }
  
  public class TypeUsage : UsageByName<Type, TypeName>
  {
    public TypeUsage(Type type, TypeName name)
      : base(type, name)
    {
    }
  }
  
  public class MethodUsage : UsageByName<Method, MethodName>
  {
    public MethodUsage(Method method, MethodName name)
      : base(method, name)
    {
    }
  }
  
  public class PropertyUsage : UsageByName<Property, PropertyName>
  {
    public PropertyUsage(Property property, PropertyName name)
      : base(property, name)
    {
    }
  }
}
