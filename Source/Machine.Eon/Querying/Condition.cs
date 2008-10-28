using System;
using System.Collections.Generic;

using Machine.Eon.Mapping;

namespace Machine.Eon.Querying
{
  public class Condition
  {
    public virtual bool Matches(Node node)
    {
      return false;
    }
  }
  public class AnyAssemblyCondition : Condition
  {
  }
  public class AnyNamespaceCondition : Condition
  {
  }
  public class AnyTypeCondition : Condition
  {
  }
  public class AnyMethodCondition : Condition
  {
  }
  public class AnyPropertyCondition : Condition
  {
  }
  public class SpecificNamespaceCondition : Condition
  {
  }
  public class SpecificTypeCondition : Condition
  {
  }
  public class SystemTypeCondition : Condition
  {
  }
  public class SpecificPropertyCondition : Condition
  {
  }
  public class PropertyGetterCondition : Condition
  {
  }
  public class PropertySetterCondition : Condition
  {
  }
  public abstract class BooleanCondition : Condition
  {
    protected Condition[] _conditions;

    protected BooleanCondition(params Condition[] conditions)
    {
      _conditions = conditions;
    }
  }
  public class AndCondition : BooleanCondition
  {
    public AndCondition(params Condition[] conditions)
      : base(conditions)
    {
    }

    public override bool Matches(Node node)
    {
      foreach (Condition c in _conditions)
      {
        if (!c.Matches(node))
        {
          return false;
        }
      }
      return true;
    }
  }
  public class OrCondition : BooleanCondition
  {
    public OrCondition(params Condition[] conditions)
      : base(conditions)
    {
    }

    public override bool Matches(Node node)
    {
      foreach (Condition c in _conditions)
      {
        if (c.Matches(node))
        {
          return true;
        }
      }
      return false;
    }
  }
  public class NotCondition : Condition
  {
    private readonly Condition _condition;

    public NotCondition(Condition condition)
    {
      _condition = condition;
    }

    public override bool Matches(Node node)
    {
      return !_condition.Matches(node);
    }
  }
}
