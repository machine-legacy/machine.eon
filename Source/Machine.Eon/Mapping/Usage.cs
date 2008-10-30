using System;
using System.Collections.Generic;

namespace Machine.Eon.Mapping
{
  public class RelativeUsage
  {
    private readonly Int32 _depth;
    private readonly Node _node;

    public Int32 Depth
    {
      get { return _depth; }
    }

    public Node Node
    {
      get { return _node; }
    }

    public RelativeUsage(Int32 depth, Node node)
    {
      _depth = depth;
      _node = node;
    }

    public override string ToString()
    {
      return "Relative<" + _depth + ", " + _node + ">";
    }
  }

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
