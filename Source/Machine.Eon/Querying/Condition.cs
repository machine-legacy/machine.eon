using System;
using System.Collections.Generic;

using Machine.Eon.Mapping;

namespace Machine.Eon.Querying
{
  public abstract class Condition
  {
    public abstract bool Matches(Node node);
  }
  public class NoneCondition : Condition
  {
    public override bool Matches(Node node)
    {
      return false;
    }
  }
  public class AllCondition : Condition
  {
    public override bool Matches(Node node)
    {
      return true;
    }
  }
  public class NamedCondition : Condition
  {
    private readonly NodeName _name;

    public NamedCondition(NodeName name)
    {
      _name = name;
    }

    public override bool Matches(Node node)
    {
      return node.NodeName.Equals(_name);
    }
  }
  public class SystemTypeCondition : Condition
  {
    public override bool Matches(Node node)
    {
      IType type = node as IType;
      if (type == null)
      {
        throw new InvalidOperationException("This condition only applies to Type's");
      }
      return type.TypeName.AssemblyName.Name.StartsWith("System");
    }
  }
  public class PropertyGetterCondition : Condition
  {
    public override bool Matches(Node node)
    {
      throw new NotImplementedException();
    }
  }
  public class PropertySetterCondition : Condition
  {
    public override bool Matches(Node node)
    {
      throw new NotImplementedException();
    }
  }
}
