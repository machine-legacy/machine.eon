using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public abstract class Usage
  {
  }

  public abstract class UsageByKey<TNode, TKey> : Usage where TNode : Node, IKeyedNode<TKey> where TKey : NodeKey
  {
    private readonly TNode _node;

    protected UsageByKey(TNode node)
    {
      _node = node;
    }

    public TNode Node
    {
      get { return _node; }
    }

    private TKey Key
    {
      get { return _node.Key; }
    }

    public override bool Equals(object obj)
    {
      if (obj is UsageByKey<TNode, TKey>)
      {
        return ((UsageByKey<TNode, TKey>)obj).Key.Equals(this.Key);
      }
      return false;
    }
    
    public override Int32 GetHashCode()
    {
      return this.Key.GetHashCode();
    }

    public override string ToString()
    {
      return "Usage<" + this.Key + ">";
    }
  }
  
  public class TypeUsage : UsageByKey<Type, TypeKey>
  {
    public TypeUsage(Type type)
      : base(type)
    {
    }
  }
  
  public class MethodUsage : UsageByKey<Method, MethodKey>
  {
    public MethodUsage(Method method)
      : base(method)
    {
    }
  }
  
  public class PropertyUsage : UsageByKey<Property, PropertyKey>
  {
    public PropertyUsage(Property property)
      : base(property)
    {
    }
  }
}
