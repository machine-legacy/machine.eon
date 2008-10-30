using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public abstract class Usage
  {
  }

  public abstract class UsageByName<TNode, TName> : Usage where TNode : Node, INodeNamed<TName> where TName : NodeName
  {
    private readonly TNode _node;

    protected UsageByName(TNode node)
    {
      _node = node;
    }

    public TNode Node
    {
      get { return _node; }
    }

    private TName Name
    {
      get { return _node.Name; }
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
    public TypeUsage(Type type)
      : base(type)
    {
    }
  }
  
  public class MethodUsage : UsageByName<Method, MethodName>
  {
    public MethodUsage(Method method)
      : base(method)
    {
    }
  }
  
  public class PropertyUsage : UsageByName<Property, PropertyName>
  {
    public PropertyUsage(Property property)
      : base(property)
    {
    }
  }
}
